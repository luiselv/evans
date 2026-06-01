using EVANS.Application.Identidad.DTOs;
using EVANS.Application.Identidad.Ports;
using EVANS.Application.Identidad.Queries;
using NSubstitute;

namespace EVANS.Application.Tests.Identidad;

public sealed class ConsultarRucQueryHandlerTests
{
    [Fact]
    public async Task Handle_ValidRuc_ReturnsSunatData()
    {
        var service = Substitute.For<ISunatRucService>();
        service.ConsultarAsync("20123456789", Arg.Any<CancellationToken>())
            .Returns(new SunatRucDto("20123456789", "ACME SAC", "ACTIVO", "HABIDO", "AV LIMA 123"));

        var result = await new ConsultarRucQueryHandler(service)
            .Handle(new ConsultarRucQuery("20123456789"), CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Value!.RazonSocial.Should().Be("ACME SAC");
    }

    [Fact]
    public async Task Handle_InvalidRuc_ReturnsFailureWithoutCallingService()
    {
        var service = Substitute.For<ISunatRucService>();

        var result = await new ConsultarRucQueryHandler(service)
            .Handle(new ConsultarRucQuery("123"), CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
        await service.DidNotReceive().ConsultarAsync(Arg.Any<string>(), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_NotFound_ReturnsFailure()
    {
        var service = Substitute.For<ISunatRucService>();
        service.ConsultarAsync("20123456789", Arg.Any<CancellationToken>())
            .Returns((SunatRucDto?)null);

        var result = await new ConsultarRucQueryHandler(service)
            .Handle(new ConsultarRucQuery("20123456789"), CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be("RUC not found.");
    }
}
