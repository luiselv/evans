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

public class ActualizarGuiaReplaceAllTests
{
    private static IMediator BuildMediator()
    {
        var services = new ServiceCollection();
        services.AddEvansApplication();

        var repo = Substitute.For<IGuiaRepository>();
        var numerador = Substitute.For<INumeradorService>();
        var uowFactory = Substitute.For<IUnitOfWorkFactory>();
        var uow = Substitute.For<IUnitOfWork>();
        var recepcion = Substitute.For<IRecepcionVinculadaService>();
        var catalogos = Substitute.For<ICatalogosGuiaRepository>();

        uowFactory.Create(Arg.Any<int>()).Returns(uow);

        services.AddSingleton(repo);
        services.AddSingleton(numerador);
        services.AddSingleton(uowFactory);
        services.AddSingleton(recepcion);
        services.AddSingleton(catalogos);

        return services.BuildServiceProvider().GetRequiredService<IMediator>();
    }

    [Fact]
    public async Task ActualizarGuia_ValidCommand_ReturnsOk()
    {
        var mediator = BuildMediator();
        var cmd = new ActualizarGuiaCommand(
            Codigo: 7,
            RemitenteId: 1, DestinatarioId: 2,
            DireccionPartida: "Av Lima|Lima|Lima",
            DireccionLlegada: "Av Trujillo|Trujillo|La Libertad",
            HasManifest: false,
            VehiculoId: null, CarretaId: null, ChoferId: null,
            Igv: 0.18m,
            Year: 2024,
            Detalles: [new DetalleGuiaInput("Updated", new Peso(8m), 80m, 80m, 1)]);

        var result = await mediator.Send(cmd);

        result.IsSuccess.Should().BeTrue();
    }
}
