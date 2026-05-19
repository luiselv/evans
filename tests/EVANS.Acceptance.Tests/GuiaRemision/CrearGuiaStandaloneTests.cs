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

public class CrearGuiaStandaloneTests
{
    private static (IMediator mediator, IGuiaRepository repo, INumeradorService numerador, IUnitOfWork uow)
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

        numerador.IncrementarYObtenerGuia().Returns(new NumeroGuia("GREM", 1));
        uowFactory.Create(Arg.Any<int>()).Returns(uow);
        repo.Insertar(Arg.Any<Guia>(), Arg.Any<IUnitOfWork>()).Returns(42);

        services.AddSingleton(repo);
        services.AddSingleton(numerador);
        services.AddSingleton(uowFactory);
        services.AddSingleton(recepcion);
        services.AddSingleton(catalogos);

        var sp = services.BuildServiceProvider();
        return (sp.GetRequiredService<IMediator>(), repo, numerador, uow);
    }

    private static CrearGuiaCommand ValidCommand() => new(
        RemitenteId: 1,
        DestinatarioId: 2,
        DireccionPartida: "Av Lima 123|Lima|Lima",
        DireccionLlegada: "Av Trujillo 456|Trujillo|La Libertad",
        HasManifest: false,
        VehiculoId: null, CarretaId: null, ChoferId: null,
        Igv: 0.18m,
        Origen: new Standalone(),
        Year: 2024,
        Detalles: [new DetalleGuiaInput("Carga general", new Peso(10m), 100m, 100m, 1)]);

    [Fact]
    public async Task CrearGuia_Standalone_ReturnsOkWithCodigo()
    {
        var (mediator, repo, numerador, uow) = BuildMediator();

        var result = await mediator.Send(ValidCommand());

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(42);
        numerador.Received(1).IncrementarYObtenerGuia();
        repo.Received(1).Insertar(Arg.Any<Guia>(), Arg.Any<IUnitOfWork>());
        uow.Received(1).Commit();
    }

    [Fact]
    public async Task CrearGuia_InvalidRemitente_ThrowsValidationException()
    {
        var (mediator, _, _, _) = BuildMediator();
        var cmd = ValidCommand() with { RemitenteId = 0 };

        var act = async () => await mediator.Send(cmd);

        await act.Should().ThrowAsync<FluentValidation.ValidationException>();
    }
}
