using EVANS.Application.DependencyInjection;
using EVANS.Application.GuiaRemision.Ports;
using EVANS.Application.Recepcion.Commands;
using EVANS.Application.Recepcion.DependencyInjection;
using EVANS.Application.Recepcion.DTOs;
using EVANS.Application.Recepcion.Ports;
using EVANS.Application.Recepcion.Queries;
using EVANS.Domain.GuiaRemision;
using EVANS.Domain.Recepcion;
using FluentAssertions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using Xunit;
using Agg = EVANS.Domain.Recepcion.Recepcion;

namespace EVANS.Acceptance.Tests.Recepcion;

/// <summary>
/// Acceptance tests E-01 to E-04: full CRUD vertical slice end-to-end via IMediator.
/// No real DB — uses NSubstitute mocks with a shared in-memory store.
/// </summary>
public class RecepcionAcceptanceTests
{
    private const int TestYear = 2024;

    // ------------------------------------------------------------------
    // DI helper: builds IMediator wired with an in-memory mock repository
    // ------------------------------------------------------------------

    private static (IMediator mediator, InMemoryRecepcionStore store) BuildMediator()
    {
        var services = new ServiceCollection();
        services.AddEvansApplication();
        services.AddEvansRecepcionApplication();

        var store = new InMemoryRecepcionStore();

        var repo = Substitute.For<IRecepcionRepository>();
        var catalogos = Substitute.For<ICatalogosRecepcionRepository>();
        var uowFactory = Substitute.For<IUnitOfWorkFactory>();
        var uow = Substitute.For<IUnitOfWork>();

        uowFactory.Create(Arg.Any<int>()).Returns(uow);

        // CrearAsync: assign Codigo via SetCodigo, persist to store
        repo.CrearAsync(Arg.Any<Agg>(), Arg.Any<IUnitOfWork>(), Arg.Any<CancellationToken>())
            .Returns(ci =>
            {
                var agg = ci.Arg<Agg>();
                var codigo = store.NextId();
                agg.SetCodigo(codigo);
                store.Save(agg);
                return Task.FromResult(codigo);
            });

        // ActualizarAsync: replace in store
        repo.ActualizarAsync(Arg.Any<Agg>(), Arg.Any<IUnitOfWork>(), Arg.Any<CancellationToken>())
            .Returns(ci =>
            {
                var agg = ci.Arg<Agg>();
                store.Save(agg);
                return Task.CompletedTask;
            });

        // EliminarAsync: remove from store
        repo.EliminarAsync(Arg.Any<int>(), Arg.Any<IUnitOfWork>(), Arg.Any<CancellationToken>())
            .Returns(ci =>
            {
                var codigo = ci.Arg<int>();
                store.Remove(codigo);
                return Task.CompletedTask;
            });

        // ObtenerPorCodigoAsync: read from store
        repo.ObtenerPorCodigoAsync(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<CancellationToken>())
            .Returns(ci =>
            {
                var codigo = ci.ArgAt<int>(0);
                return Task.FromResult(store.Find(codigo));
            });

        // BuscarPorRangoFechasAsync: return matching items from store
        repo.BuscarPorRangoFechasAsync(Arg.Any<DateRange>(), Arg.Any<int>(), Arg.Any<CancellationToken>())
            .Returns(ci =>
            {
                var rango = ci.Arg<DateRange>();
                IReadOnlyList<RecepcionListItemDto> items = store.FindByRange(rango);
                return Task.FromResult(items);
            });

        services.AddSingleton(repo);
        services.AddSingleton(catalogos);
        services.AddSingleton(uowFactory);

        var sp = services.BuildServiceProvider();
        return (sp.GetRequiredService<IMediator>(), store);
    }

    private static CrearRecepcionCommand ValidCrearCommand(DateTime? fecha = null) => new(
        FechaEmision: fecha ?? DateTime.Today,
        RemitenteId: 1,
        TipoDirPartida: TipoDireccion.Agencia,
        DireccionPartida: "Agencia Lima",
        DestinatarioId: 2,
        TipoDirDestino: TipoDireccion.DireccionCliente,
        DireccionDestino: "Av Arequipa 123",
        DestinoId: 1,
        EstadoId: 1,
        Bultos: null,
        PesoTotal: null,
        CostoTotal: 100m,
        Observacion: null,
        UsuarioId: 1,
        Detalles: new[]
        {
            new DetalleRecepcionInput(1m, "Carga general", 0m, "UND", 100m, "GR", "001")
        },
        Year: TestYear);

    // ------------------------------------------------------------------
    // E-01: Crear then ObtenerPorCodigo round-trip
    // ------------------------------------------------------------------

    [Fact]
    public async Task E01_Crear_ThenObtener_RoundTripMatchesCommand()
    {
        var (mediator, _) = BuildMediator();
        var cmd = ValidCrearCommand();

        var createResult = await mediator.Send(cmd);

        createResult.IsSuccess.Should().BeTrue();
        var codigo = createResult.Value;
        codigo.Should().BeGreaterThan(0);

        var obtener = await mediator.Send(new ObtenerRecepcionPorCodigoQuery(codigo, TestYear));

        obtener.IsSuccess.Should().BeTrue();
        obtener.Value.Should().NotBeNull();
        obtener.Value!.Codigo.Should().Be(codigo);
        obtener.Value.RemitenteCodigo.Should().Be(cmd.RemitenteId);
        obtener.Value.DestinatarioCodigo.Should().Be(cmd.DestinatarioId);
        obtener.Value.Detalles.Should().HaveCount(1);
    }

    // ------------------------------------------------------------------
    // E-02: Actualizar changes header and replaces details
    // ------------------------------------------------------------------

    [Fact]
    public async Task E02_Actualizar_ReemplazaDetalles()
    {
        var (mediator, _) = BuildMediator();

        // Create with 1 detail
        var createResult = await mediator.Send(ValidCrearCommand());
        createResult.IsSuccess.Should().BeTrue();
        var codigo = createResult.Value;

        // Update with 2 new details
        var updateCmd = new ActualizarRecepcionCommand(
            Codigo: codigo,
            FechaEmision: DateTime.Today,
            RemitenteId: 1,
            TipoDirPartida: TipoDireccion.Agencia,
            DireccionPartida: "Agencia Lima",
            DestinatarioId: 2,
            TipoDirDestino: TipoDireccion.DireccionCliente,
            DireccionDestino: "Av Arequipa 123",
            DestinoId: 1,
            EstadoId: 1,
            Bultos: null,
            PesoTotal: null,
            CostoTotal: 200m,
            Observacion: null,
            Detalles: new[]
            {
                new DetalleRecepcionInput(2m, "Nuevo Item 1", 0m, "UND", 100m, "GR", "A01"),
                new DetalleRecepcionInput(3m, "Nuevo Item 2", 0m, "UND", 100m, "GR", "A02")
            },
            Year: TestYear);

        var updateResult = await mediator.Send(updateCmd);
        updateResult.IsSuccess.Should().BeTrue();

        var obtener = await mediator.Send(new ObtenerRecepcionPorCodigoQuery(codigo, TestYear));
        obtener.Value.Should().NotBeNull();
        obtener.Value!.Detalles.Should().HaveCount(2);
    }

    // ------------------------------------------------------------------
    // E-03: Eliminar removes the record
    // ------------------------------------------------------------------

    [Fact]
    public async Task E03_Eliminar_RemoveRecord()
    {
        var (mediator, _) = BuildMediator();

        var createResult = await mediator.Send(ValidCrearCommand());
        var codigo = createResult.Value;

        var eliminarResult = await mediator.Send(new EliminarRecepcionCommand(codigo, TestYear));
        eliminarResult.IsSuccess.Should().BeTrue();

        var obtener = await mediator.Send(new ObtenerRecepcionPorCodigoQuery(codigo, TestYear));
        obtener.IsSuccess.Should().BeTrue();
        obtener.Value.Should().BeNull();
    }

    // ------------------------------------------------------------------
    // E-04: Buscar by date range returns only matching records
    // ------------------------------------------------------------------

    [Fact]
    public async Task E04_Buscar_PorRangoFechas_RetornaSoloCoincidentes()
    {
        var (mediator, _) = BuildMediator();

        // Create one in May, one in June
        await mediator.Send(ValidCrearCommand(new DateTime(2024, 5, 10)));
        await mediator.Send(ValidCrearCommand(new DateTime(2024, 6, 10)));

        var rango = DateRange.Intervalo(new DateTime(2024, 5, 1), new DateTime(2024, 5, 31));
        var buscar = await mediator.Send(new BuscarRecepcionesQuery(rango, TestYear));

        buscar.IsSuccess.Should().BeTrue();
        buscar.Value.Should().HaveCount(1);
        buscar.Value[0].Fecha.Month.Should().Be(5);
    }
}

/// <summary>
/// Simple in-memory store for acceptance test scenarios.
/// Thread-safe enough for sequential test execution.
/// </summary>
internal sealed class InMemoryRecepcionStore
{
    private readonly Dictionary<int, Agg> _data = new();
    private int _nextId = 1;

    public int NextId() => _nextId++;

    public void Save(Agg agg) => _data[agg.Codigo] = agg;

    public void Remove(int codigo) => _data.Remove(codigo);

    public Agg? Find(int codigo) => _data.TryGetValue(codigo, out var a) ? a : null;

    public IReadOnlyList<RecepcionListItemDto> FindByRange(DateRange rango) =>
        _data.Values
            .Where(a => a.FechaEmision >= rango.Inicio && a.FechaEmision <= rango.Fin)
            .Select(a => new RecepcionListItemDto(
                a.Codigo, a.FechaEmision,
                a.RemitenteId, string.Empty,
                a.DestinatarioId, string.Empty,
                a.DestinoId, string.Empty,
                a.EstadoId, string.Empty,
                a.CostoTotal, a.GuiaRemisionVinculada))
            .ToList()
            .AsReadOnly();
}
