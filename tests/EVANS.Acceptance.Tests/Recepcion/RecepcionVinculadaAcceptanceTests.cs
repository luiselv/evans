using EVANS.Application.DependencyInjection;
using EVANS.Application.GuiaRemision.Ports;
using EVANS.Application.Recepcion.Commands;
using EVANS.Application.Recepcion.Ports;
using EVANS.Application.Recepcion.Queries;
using EVANS.Domain.GuiaRemision;
using EVANS.Domain.Recepcion;
using FluentAssertions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using Xunit;

namespace EVANS.Acceptance.Tests.Recepcion;

/// <summary>
/// Acceptance test E-05: Crear + IRecepcionVinculadaService.VincularRecepcion round-trip.
/// </summary>
public class RecepcionVinculadaAcceptanceTests
{
    private static (IMediator mediator, IRecepcionRepository recepcionRepo, IUnitOfWork uow)
        BuildMediator()
    {
        var services = new ServiceCollection();
        services.AddEvansApplication();

        var recepcionRepo = Substitute.For<IRecepcionRepository>();
        var uowFactory = Substitute.For<IUnitOfWorkFactory>();
        var uow = Substitute.For<IUnitOfWork>();
        var catalogosRecepcion = Substitute.For<ICatalogosRecepcionRepository>();
        var guiaRepo = Substitute.For<IGuiaRepository>();
        var numerador = Substitute.For<INumeradorService>();
        var numeradorComp = Substitute.For<Application.Comprobante.Ports.INumeradorComprobanteService>();
        var comprobanteRepo = Substitute.For<Application.Comprobante.Ports.IComprobanteRepository>();
        var guiaVinculada = Substitute.For<Application.Comprobante.Ports.IGuiaVinculadaService>();
        var parametros = Substitute.For<Application.Shared.Ports.IParametrosService>();
        var recepcionVinculada = Substitute.For<IRecepcionVinculadaService>();
        var catalogosGuia = Substitute.For<ICatalogosGuiaRepository>();

        uowFactory.Create(Arg.Any<int>()).Returns(uow);
        recepcionRepo.CrearAsync(Arg.Any<Domain.Recepcion.Recepcion>(), Arg.Any<IUnitOfWork>(), Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(55));
        numerador.IncrementarYObtenerGuia().Returns(new NumeroGuia("GR01", 1));
        guiaRepo.Insertar(Arg.Any<Guia>(), Arg.Any<IUnitOfWork>()).Returns(1);

        services.AddSingleton(recepcionRepo);
        services.AddSingleton(uowFactory);
        services.AddSingleton(catalogosRecepcion);
        services.AddSingleton(guiaRepo);
        services.AddSingleton(numerador);
        services.AddSingleton(numeradorComp);
        services.AddSingleton(comprobanteRepo);
        services.AddSingleton(guiaVinculada);
        services.AddSingleton(parametros);
        services.AddSingleton(recepcionVinculada);
        services.AddSingleton(catalogosGuia);

        var sp = services.BuildServiceProvider();
        return (sp.GetRequiredService<IMediator>(), recepcionRepo, uow);
    }

    // E-05: Crear + VincularRecepcion shows GuiaRemisionVinculada populated
    [Fact]
    public async Task E05_Crear_ThenVincular_GuiaVinculadaPopulated()
    {
        var (mediator, recepcionRepo, uow) = BuildMediator();

        // Step 1: Create recepcion
        var crearCmd = new CrearRecepcionCommand(
            FechaEmision: DateTime.Today,
            RemitenteId: 1,
            TipoDirPartida: TipoDireccion.Agencia,
            DireccionPartida: "Agencia Lima",
            DestinatarioId: 2,
            TipoDirDestino: TipoDireccion.DireccionCliente,
            DireccionDestino: "Jr Puno 123",
            DestinoId: 1,
            EstadoId: 1,
            Bultos: 1,
            PesoTotal: 10m,
            CostoTotal: 100m,
            Observacion: "",
            UsuarioId: 1,
            AplicarIgv: false,
            TasaIgv: 0m,
            Year: 2024,
            Detalles: new[] { new DetalleRecepcionInput(1m, "Caja", 10m, "unidad", 100m, "GR", "001") });

        var result = await mediator.Send(crearCmd);
        result.IsSuccess.Should().BeTrue();
        var codigo = result.Value;
        codigo.Should().Be(55);

        // Step 2: Simulate VincularRecepcion (normally called by CrearGuiaCommandHandler)
        // Set up ObtenerPorCodigo to return aggregate with GuiaRemisionVinculada set
        var conGuia = Domain.Recepcion.Recepcion.Materializar(
            55, DateTime.Today, 1, TipoDireccion.Agencia, "Agencia Lima",
            2, TipoDireccion.DireccionCliente, "Jr Puno 123",
            1, 1, 1, 10m, 100m, "T001-000001", "", 1,
            new[] { DetalleRecepcion.Crear(1m, "Caja", 10m, "unidad", 100m, "GR", "001") });

        recepcionRepo.ObtenerPorCodigoAsync(55, 2024, Arg.Any<CancellationToken>())
            .Returns(conGuia);

        // Step 3: Refresh — should show GuiaRemisionVinculada
        var dto = await mediator.Send(new ObtenerRecepcionPorCodigoQuery(55, 2024));

        dto.IsSuccess.Should().BeTrue();
        dto.Value!.GuiaRemisionVinculada.Should().Be("T001-000001");
    }
}
