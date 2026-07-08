using EVANS.Application.Shared.Commands;
using EVANS.Application.Shared.DTOs;
using EVANS.Application.Shared.Ports;
using NSubstitute;

namespace EVANS.Application.Tests.Shared;

public sealed class ParametrosCommandHandlerTests
{
    [Fact]
    public async Task ActualizarParametrosCommandHandler_ValidCommand_UpdatesParametros()
    {
        var service = Substitute.For<IParametrosService>();
        var parametros = ValidParametros();

        var result = await new ActualizarParametrosCommandHandler(service).Handle(
            new ActualizarParametrosCommand(parametros),
            CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeTrue();
        await service.Received(1).ActualizarParametrosAsync(parametros, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task ActualizarParametrosCommandHandler_InvalidIgv_ReturnsFailureWithoutServiceCall()
    {
        var service = Substitute.For<IParametrosService>();
        var parametros = ValidParametros() with { IgvRate = 0m };

        var result = await new ActualizarParametrosCommandHandler(service).Handle(
            new ActualizarParametrosCommand(parametros),
            CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be("IGV rate is required.");
        await service.DidNotReceive().ActualizarParametrosAsync(Arg.Any<ParametrosDto>(), Arg.Any<CancellationToken>());
    }

    private static ParametrosDto ValidParametros() => new(
        IgvRate: 0.18m,
        FacturaSerie: "F001",
        FacturaNro1: "000001",
        FacturaNro2: "0",
        BoletaSerie: "B001",
        BoletaNro1: "000001",
        BoletaNro2: "0",
        GuiaRemisionSerie: "GR01",
        GuiaRemisionNro1: "000001",
        GuiaRemisionNro2: "0",
        Manifiesto: "M001",
        Remitente: "EVANS",
        EmailRemitente: "evans@example.com",
        PassRemitente: "secret",
        Smtp: "smtp.example.com",
        Puerto: 587);
}
