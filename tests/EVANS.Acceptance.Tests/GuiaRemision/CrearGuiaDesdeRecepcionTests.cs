using EVANS.Application.DependencyInjection;
using EVANS.Application.GuiaRemision.Commands;
using EVANS.Application.GuiaRemision.Ports;
using EVANS.Domain.GuiaRemision;
using FluentAssertions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using Xunit;

namespace EVANS.Acceptance.Tests.GuiaRemision;

public class CrearGuiaDesdeRecepcionTests
{
    private static (IMediator mediator, IRecepcionVinculadaService recepcion)
        BuildMediator()
    {
        var services = new ServiceCollection();
        services.AddEvansApplication();

        var repo = Substitute.For<IGuiaRepository>();
        var numerador = Substitute.For<INumeradorService>();
        var uowFactory = Substitute.For<IUnitOfWorkFactory>();
        var uow = Substitute.For<IUnitOfWork>();
        var recepcion = Substitute.For<IRecepcionVinculadaService>();
        var catalogos = Substitute.For<ICatalogosGuiaRepository>();

        numerador.IncrementarYObtenerGuia().Returns(new NumeroGuia("GREM", 2));
        uowFactory.Create(Arg.Any<int>()).Returns(uow);
        repo.Insertar(Arg.Any<Guia>(), Arg.Any<IUnitOfWork>()).Returns(99);

        services.AddSingleton(repo);
        services.AddSingleton(numerador);
        services.AddSingleton(uowFactory);
        services.AddSingleton(recepcion);
        services.AddSingleton(catalogos);

        var sp = services.BuildServiceProvider();
        return (sp.GetRequiredService<IMediator>(), recepcion);
    }

    [Fact]
    public async Task CrearGuia_DesdeRecepcion_VincularCalledAfterCommit()
    {
        var (mediator, recepcion) = BuildMediator();
        var cmd = new CrearGuiaCommand(
            RemitenteId: 1, DestinatarioId: 2,
            DireccionPartida: "Av Lima|Lima|Lima",
            DireccionLlegada: "Av Trujillo|Trujillo|La Libertad",
            HasManifest: false,
            VehiculoId: null, CarretaId: null, ChoferId: null,
            Igv: 0.18m,
            Origen: new DesdeRecepcion(55),
            Year: 2024,
            Detalles: [new DetalleGuiaInput("Item", new Peso(5m), 50m, 50m, 1)]);

        var result = await mediator.Send(cmd);

        result.IsSuccess.Should().BeTrue();
        recepcion.Received(1).VincularRecepcion(55, Arg.Any<NumeroGuia>(), 2024);
    }
}
