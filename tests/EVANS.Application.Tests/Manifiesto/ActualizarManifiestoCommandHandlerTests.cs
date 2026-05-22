using EVANS.Application.Common;
using EVANS.Application.GuiaRemision.Ports;
using EVANS.Application.Manifiesto.Commands;
using EVANS.Application.Manifiesto.DTOs;
using EVANS.Application.Manifiesto.Handlers;
using EVANS.Application.Manifiesto.Ports;
using EVANS.Domain.Manifiesto;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NSubstitute.ExceptionExtensions;

namespace EVANS.Application.Tests.Manifiesto;

public class ActualizarManifiestoCommandHandlerTests
{
    private readonly IManifiestoRepository _repo;
    private readonly IUnitOfWorkFactory _uowFactory;
    private readonly IUnitOfWork _uow;
    private readonly IGuiasManifiestoService _guiasService;
    private readonly ILogger<ActualizarManifiestoCommandHandler> _logger;

    public ActualizarManifiestoCommandHandlerTests()
    {
        _repo = Substitute.For<IManifiestoRepository>();
        _uowFactory = Substitute.For<IUnitOfWorkFactory>();
        _uow = Substitute.For<IUnitOfWork>();
        _guiasService = Substitute.For<IGuiasManifiestoService>();
        _logger = Substitute.For<ILogger<ActualizarManifiestoCommandHandler>>();

        _uowFactory.Create(Arg.Any<int>()).Returns(_uow);

        // Default: existing manifiesto found
        _repo.ObtenerPorCodigoAsync(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<CancellationToken>())
             .Returns(Task.FromResult<ManifiestoDto?>(BuildManifiestoDto(codigo: 7, guiaIds: new[] { 1, 2 })));

        _guiasService.MarcarGuiasDisponiblesAsync(
                Arg.Any<IReadOnlyList<int>>(), Arg.Any<int>(), Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(new GuiasMarcadoResult(2, Array.Empty<int>())));

        _guiasService.MarcarGuiasEnviadasAsync(
                Arg.Any<IReadOnlyList<int>>(), Arg.Any<string>(), Arg.Any<DateTime>(),
                Arg.Any<CarrierInfo>(), Arg.Any<int>(), Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(new GuiasMarcadoResult(2, Array.Empty<int>())));
    }

    private ActualizarManifiestoCommandHandler CreateHandler() =>
        new(_repo, _uowFactory, _guiasService, _logger);

    private static ActualizarManifiestoCommand BuildCommand(
        int codigo = 7,
        IReadOnlyList<int>? guiaIds = null) => new(
        Codigo: codigo,
        Fecha: DateTime.Today,
        TransportistaCodigo: 5,
        VehiculoCodigo: 3,
        CarretaCodigo: null,
        ChoferCodigo: 7,
        Importe: 100m,
        Peso: 50m,
        EstadoCodigo: 1,
        UsuarioCodigo: 2,
        GuiaIds: guiaIds ?? new List<int> { 1, 2 },
        Year: 2024);

    private static ManifiestoDto BuildManifiestoDto(int codigo, int[] guiaIds) =>
        new(Codigo: codigo, Numero: "2024-1", Fecha: DateTime.Today,
            TransportistaCodigo: 5, TransportistaNombre: "Transportes SA",
            VehiculoCodigo: 3, VehiculoPlaca: "ABC-123",
            CarretaCodigo: null, CarretaPlaca: null,
            ChoferCodigo: 7, ChoferNombre: "Juan Perez",
            Importe: 100m, Peso: 50m, NroGuias: guiaIds.Length,
            EstadoCodigo: 1, EstadoNombre: "Activo",
            UsuarioCodigo: 2,
            Lineas: guiaIds.Select(id => new ManifiestoLineaDto(id, $"G-{id}", 1, "Lima", 10m, 50m)).ToList());

    // A-05: Actualizar NO llama al numerador (numerador NOT injected into actualizar handler)
    [Fact]
    public async Task Actualizar_NoLlamaNumerador_NumeradorNoEsInyectado()
    {
        // The handler constructor does NOT accept INumeradorManifiestoService
        // This test verifies via reflection / design
        var handler = CreateHandler();
        var result = await handler.Handle(BuildCommand(), CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        // No numerador mock was set up — if handler tried to call it, it would fail
    }

    // A-06: Actualizar usa el Codigo real (no cero) — bug fix validation
    [Fact]
    public async Task Actualizar_UsaCodigoReal_NoCodigoCero()
    {
        EVANS.Domain.Manifiesto.Manifiesto? captured = null;
        _repo.When(r => r.ActualizarAsync(
                Arg.Any<EVANS.Domain.Manifiesto.Manifiesto>(),
                Arg.Any<IUnitOfWork>(),
                Arg.Any<CancellationToken>()))
            .Do(ci => captured = ci.Arg<EVANS.Domain.Manifiesto.Manifiesto>());

        await CreateHandler().Handle(BuildCommand(codigo: 7), CancellationToken.None);

        captured.Should().NotBeNull();
        captured!.Codigo.Should().Be(7);
        captured.Codigo.Should().NotBe(0);
    }

    // A-07: post-commit reconcilia guias (disponibles para guias removidas, enviadas para añadidas)
    [Fact]
    public async Task Actualizar_PostCommit_ReconciliaGuias()
    {
        // Existing has guias [1,2]; command sends [2,3] → remove 1, add 3
        _repo.ObtenerPorCodigoAsync(7, 2024, Arg.Any<CancellationToken>())
             .Returns(Task.FromResult<ManifiestoDto?>(BuildManifiestoDto(codigo: 7, guiaIds: new[] { 1, 2 })));

        var command = BuildCommand(guiaIds: new List<int> { 2, 3 });
        var result = await CreateHandler().Handle(command, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();

        // MarcarGuiasDisponibles called for removed guias (id=1)
        await _guiasService.Received(1).MarcarGuiasDisponiblesAsync(
            Arg.Is<IReadOnlyList<int>>(ids => ids.Contains(1) && !ids.Contains(2)),
            2024,
            Arg.Any<CancellationToken>());

        // MarcarGuiasEnviadas called for added guias (id=3)
        await _guiasService.Received(1).MarcarGuiasEnviadasAsync(
            Arg.Is<IReadOnlyList<int>>(ids => ids.Contains(3) && !ids.Contains(1)),
            Arg.Any<string>(),
            Arg.Any<DateTime>(),
            Arg.Any<CarrierInfo>(),
            2024,
            Arg.Any<CancellationToken>());
    }

    // A-08 variant: not-found returns fail
    [Fact]
    public async Task Actualizar_NotFound_ReturnsFail()
    {
        _repo.ObtenerPorCodigoAsync(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<CancellationToken>())
             .Returns(Task.FromResult<ManifiestoDto?>(null));

        var result = await CreateHandler().Handle(BuildCommand(codigo: 9999), CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
        await _repo.DidNotReceive().ActualizarAsync(
            Arg.Any<EVANS.Domain.Manifiesto.Manifiesto>(),
            Arg.Any<IUnitOfWork>(),
            Arg.Any<CancellationToken>());
    }
}
