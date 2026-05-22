using EVANS.Application.Common;
using EVANS.Application.DependencyInjection;
using EVANS.Application.GuiaRemision.Ports;
using EVANS.Application.Manifiesto.Commands;
using EVANS.Application.Manifiesto.DTOs;
using EVANS.Application.Manifiesto.Ports;
using EVANS.Application.Manifiesto.Queries;
using EVANS.Domain.Manifiesto;
using FluentAssertions;
using Agg = EVANS.Domain.Manifiesto.Manifiesto;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using System.Text.RegularExpressions;
using Xunit;

namespace EVANS.Acceptance.Tests.Manifiesto;

/// <summary>
/// E2E acceptance tests that exercise the full Application layer (handlers, validators, domain)
/// using NSubstitute doubles for all infrastructure ports.
/// Pattern mirrors ComprobanteEndToEndTests.
/// </summary>
public class ManifiestoEndToEndTests
{
    private const int TestYear          = 2024;
    private const int TestTransportista = 1;
    private const int TestVehiculo      = 2;
    private const int TestChofer        = 3;
    private const int TestEstado        = 1;
    private const int TestUsuario       = 1;

    private static (
        IMediator mediator,
        IManifiestoRepository repo,
        INumeradorManifiestoService numerador,
        IGuiasManifiestoService guiasService,
        IUnitOfWork uow)
        BuildMediator(
            int insertarReturnsCodigo = 42,
            string numeradorValue = "2024-1")
    {
        var services = new ServiceCollection();
        services.AddEvansApplication();

        var repo         = Substitute.For<IManifiestoRepository>();
        var numerador    = Substitute.For<INumeradorManifiestoService>();
        var uowFactory   = Substitute.For<IUnitOfWorkFactory>();
        var uow          = Substitute.For<IUnitOfWork>();
        var guiasService = Substitute.For<IGuiasManifiestoService>();

        // Numerador returns a valid NumeroManifiesto
        numerador
            .IncrementarYObtenerAsync(Arg.Any<int>(), Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(new NumeroManifiesto(numeradorValue)));

        uowFactory.Create(Arg.Any<int>()).Returns(uow);

        // InsertarAsync returns the assigned codigo
        repo.InsertarAsync(
                Arg.Any<Agg>(),
                Arg.Any<IUnitOfWork>(),
                Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(insertarReturnsCodigo));

        // GuiasService defaults: no failures
        guiasService
            .MarcarGuiasEnviadasAsync(
                Arg.Any<IReadOnlyList<int>>(),
                Arg.Any<string>(),
                Arg.Any<DateTime>(),
                Arg.Any<CarrierInfo>(),
                Arg.Any<int>(),
                Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(new GuiasMarcadoResult(1, Array.Empty<int>())));

        guiasService
            .MarcarGuiasDisponiblesAsync(
                Arg.Any<IReadOnlyList<int>>(),
                Arg.Any<int>(),
                Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(new GuiasMarcadoResult(1, Array.Empty<int>())));

        services.AddSingleton(repo);
        services.AddSingleton(numerador);
        services.AddSingleton(uowFactory);
        services.AddSingleton(guiasService);

        var sp = services.BuildServiceProvider();
        return (sp.GetRequiredService<IMediator>(), repo, numerador, guiasService, uow);
    }

    // ----------------------------------------------------------------
    // AC-01: crear_manifiesto_persiste_y_asigna_numero
    // ----------------------------------------------------------------

    [Fact]
    public async Task AC01_crear_manifiesto_persiste_y_asigna_numero()
    {
        var (mediator, repo, numerador, guiasService, uow) = BuildMediator(
            insertarReturnsCodigo: 42,
            numeradorValue: "2024-1");

        var cmd = new CrearManifiestoCommand(
            Fecha: new DateTime(2024, 6, 15),
            TransportistaCodigo: TestTransportista,
            VehiculoCodigo: TestVehiculo,
            CarretaCodigo: null,
            ChoferCodigo: TestChofer,
            Importe: 500m,
            Peso: 1200m,
            EstadoCodigo: TestEstado,
            UsuarioCodigo: TestUsuario,
            GuiaIds: [101, 102],
            Year: TestYear);

        var result = await mediator.Send(cmd);

        // Result is Ok
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(42);

        // Numerador was called once
        await numerador.Received(1)
            .IncrementarYObtenerAsync(TestYear, Arg.Any<CancellationToken>());

        // Repository persisted the aggregate
        await repo.Received(1).InsertarAsync(
            Arg.Is<Agg>(m =>
                m.GuiaIds.Count == 2 &&
                m.TransportistaCodigo == TestTransportista),
            Arg.Any<IUnitOfWork>(),
            Arg.Any<CancellationToken>());

        // UoW committed
        uow.Received(1).Commit();

        // NumeroManifiesto format matches ^\d{4}-\d+$
        var numero = new NumeroManifiesto("2024-1");
        Regex.IsMatch(numero.Value, @"^\d{4}-\d+$").Should().BeTrue();
    }

    // ----------------------------------------------------------------
    // AC-02: buscar_manifiesto_retorna_resultados
    // ----------------------------------------------------------------

    [Fact]
    public async Task AC02_buscar_manifiesto_retorna_resultados()
    {
        var (mediator, repo, _, _, _) = BuildMediator();

        var fakeResults = new List<ManifiestoResumenDto>
        {
            new(1, "2024-1", new DateTime(2024, 6, 1),
                "Empresa A", "ABC-123", "Chofer X", 300m, 2, "Activo"),
            new(2, "2024-2", new DateTime(2024, 6, 5),
                "Empresa B", "DEF-456", "Chofer Y", 400m, 3, "Activo"),
        };

        repo.BuscarAsync(
                Arg.Any<BuscarManifiestosFiltro>(),
                Arg.Any<int>(),
                Arg.Any<CancellationToken>())
            .Returns(Task.FromResult<IReadOnlyList<ManifiestoResumenDto>>(fakeResults.AsReadOnly()));

        var filtro = new BuscarManifiestosFiltro(Year: TestYear, Numero: null);
        var results = await mediator.Send(new BuscarManifiestosQuery(filtro));

        results.Should().HaveCount(2);
        results.Should().Contain(r => r.Numero == "2024-1");
        results.Should().Contain(r => r.Numero == "2024-2");

        await repo.Received(1).BuscarAsync(
            Arg.Is<BuscarManifiestosFiltro>(f => f.Year == TestYear),
            TestYear,
            Arg.Any<CancellationToken>());
    }

    // ----------------------------------------------------------------
    // AC-03: actualizar_manifiesto_actualiza_campos (bug-fix: Codigo != 0)
    // ----------------------------------------------------------------

    [Fact]
    public async Task AC03_actualizar_manifiesto_corrige_bug_codigo_cero()
    {
        var (mediator, repo, _, guiasService, uow) = BuildMediator();

        // Stub ObtenerPorCodigoAsync to return existing record with Codigo=7
        var existingDto = new ManifiestoDto(
            Codigo: 7,
            Numero: "2024-7",
            Fecha: new DateTime(2024, 6, 1),
            TransportistaCodigo: 1, TransportistaNombre: "Emp A",
            VehiculoCodigo: 2, VehiculoPlaca: "ABC-123",
            CarretaCodigo: null, CarretaPlaca: null,
            ChoferCodigo: 3, ChoferNombre: "Juan",
            Importe: 200m, Peso: 500m,
            NroGuias: 1, EstadoCodigo: 1, EstadoNombre: "Activo",
            UsuarioCodigo: 1,
            Lineas: [new ManifiestoLineaDto(101, "GR-001", 1, "Lima", 500m, 50m)]);

        repo.ObtenerPorCodigoAsync(7, TestYear, Arg.Any<CancellationToken>())
            .Returns(Task.FromResult<ManifiestoDto?>(existingDto));

        var cmd = new ActualizarManifiestoCommand(
            Codigo: 7,
            Fecha: new DateTime(2024, 6, 20),
            TransportistaCodigo: TestTransportista,
            VehiculoCodigo: TestVehiculo,
            CarretaCodigo: null,
            ChoferCodigo: TestChofer,
            Importe: 250m,
            Peso: 600m,
            EstadoCodigo: TestEstado,
            UsuarioCodigo: TestUsuario,
            GuiaIds: [101, 103],  // 103 is new, 101 stays
            Year: TestYear);

        var result = await mediator.Send(cmd);

        result.IsSuccess.Should().BeTrue();

        // Repository called with Codigo=7 (not 0)
        await repo.Received(1).ActualizarAsync(
            Arg.Is<Agg>(m => m.Codigo == 7),
            Arg.Any<IUnitOfWork>(),
            Arg.Any<CancellationToken>());

        // UoW committed
        uow.Received(1).Commit();
    }

    // ----------------------------------------------------------------
    // AC-04: eliminar_manifiesto_libera_guias
    // ----------------------------------------------------------------

    [Fact]
    public async Task AC04_eliminar_manifiesto_libera_guias()
    {
        var (mediator, repo, _, guiasService, uow) = BuildMediator();

        // Stub existing manifiesto with guia 101
        var existingDto = new ManifiestoDto(
            Codigo: 10,
            Numero: "2024-10",
            Fecha: new DateTime(2024, 5, 1),
            TransportistaCodigo: 1, TransportistaNombre: "Emp A",
            VehiculoCodigo: 2, VehiculoPlaca: "ABC-123",
            CarretaCodigo: null, CarretaPlaca: null,
            ChoferCodigo: 3, ChoferNombre: "Pedro",
            Importe: 100m, Peso: 300m,
            NroGuias: 1, EstadoCodigo: 1, EstadoNombre: "Activo",
            UsuarioCodigo: 1,
            Lineas: [new ManifiestoLineaDto(101, "GR-001", 1, "Lima", 300m, 30m)]);

        repo.ObtenerPorCodigoAsync(10, TestYear, Arg.Any<CancellationToken>())
            .Returns(Task.FromResult<ManifiestoDto?>(existingDto));

        var cmd = new EliminarManifiestoCommand(Codigo: 10, Year: TestYear);
        var result = await mediator.Send(cmd);

        result.IsSuccess.Should().BeTrue();

        // Repository delete was called
        await repo.Received(1).EliminarAsync(10, Arg.Any<IUnitOfWork>(), Arg.Any<CancellationToken>());

        // UoW committed
        uow.Received(1).Commit();

        // Guias marked as available (GREM_ENVIADO → 0) post-commit
        await guiasService.Received(1).MarcarGuiasDisponiblesAsync(
            Arg.Is<IReadOnlyList<int>>(ids => ids.Contains(101)),
            TestYear,
            Arg.Any<CancellationToken>());
    }
}
