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

public class EliminarManifiestoCommandHandlerTests
{
    private readonly IManifiestoRepository _repo;
    private readonly IUnitOfWorkFactory _uowFactory;
    private readonly IUnitOfWork _uow;
    private readonly IGuiasManifiestoService _guiasService;
    private readonly ILogger<EliminarManifiestoCommandHandler> _logger;

    public EliminarManifiestoCommandHandlerTests()
    {
        _repo = Substitute.For<IManifiestoRepository>();
        _uowFactory = Substitute.For<IUnitOfWorkFactory>();
        _uow = Substitute.For<IUnitOfWork>();
        _guiasService = Substitute.For<IGuiasManifiestoService>();
        _logger = Substitute.For<ILogger<EliminarManifiestoCommandHandler>>();

        _uowFactory.Create(Arg.Any<int>()).Returns(_uow);

        // Default: existing manifiesto found with guias [1, 2]
        _repo.ObtenerPorCodigoAsync(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<CancellationToken>())
             .Returns(Task.FromResult<ManifiestoDto?>(BuildManifiestoDto(codigo: 5, guiaIds: new[] { 1, 2 })));

        _guiasService.MarcarGuiasDisponiblesAsync(
                Arg.Any<IReadOnlyList<int>>(), Arg.Any<int>(), Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(new GuiasMarcadoResult(2, Array.Empty<int>())));
    }

    private EliminarManifiestoCommandHandler CreateHandler() =>
        new(_repo, _uowFactory, _guiasService, _logger);

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

    // A-08: Eliminar — repo.EliminarAsync llamado y guias service llamado post-commit
    [Fact]
    public async Task Eliminar_HappyPath_RepoEliminarYGuiasServiceLlamados()
    {
        var result = await CreateHandler().Handle(new EliminarManifiestoCommand(5, 2024), CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        await _repo.Received(1).EliminarAsync(5, _uow, Arg.Any<CancellationToken>());
        _uow.Received(1).Commit();
        await _guiasService.Received(1).MarcarGuiasDisponiblesAsync(
            Arg.Is<IReadOnlyList<int>>(ids => ids.Count == 2),
            2024,
            Arg.Any<CancellationToken>());
    }

    // A-09: GuiasService throws → handler still returns Ok (best-effort)
    [Fact]
    public async Task Eliminar_GuiasServiceThrows_HandlerStillReturnsOk()
    {
        _guiasService.MarcarGuiasDisponiblesAsync(
                Arg.Any<IReadOnlyList<int>>(), Arg.Any<int>(), Arg.Any<CancellationToken>())
            .Throws(new InvalidOperationException("Guias service error"));

        var result = await CreateHandler().Handle(new EliminarManifiestoCommand(5, 2024), CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
    }

    // Not-found: returns Fail
    [Fact]
    public async Task Eliminar_NotFound_ReturnsFail()
    {
        _repo.ObtenerPorCodigoAsync(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<CancellationToken>())
             .Returns(Task.FromResult<ManifiestoDto?>(null));

        var result = await CreateHandler().Handle(new EliminarManifiestoCommand(9999, 2024), CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
        await _repo.DidNotReceive().EliminarAsync(Arg.Any<int>(), Arg.Any<IUnitOfWork>(), Arg.Any<CancellationToken>());
    }
}
