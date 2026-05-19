using EVANS.Application.Common;
using EVANS.Application.GuiaRemision.Commands;
using EVANS.Application.GuiaRemision.DTOs;
using EVANS.Application.GuiaRemision.Queries;
using EVANS.Domain.GuiaRemision;
using EVANS.UI.WinForms.GuiaRemision;
using MediatR;
using NSubstitute;

namespace EVANS.UI.Tests.GuiaRemision;

public class FrmGuiaRemisionTests
{
    private static CatalogosGuiaDto EmptyCatalogos() => new(
        [], [], [], [], [], [], 0.18m);

    [WinFormsFact]
    public async Task LoadAsync_Sends_ObtenerCatalogosGuiaQuery()
    {
        var mediator = Substitute.For<IMediator>();
        mediator.Send(Arg.Any<ObtenerCatalogosGuiaQuery>(), Arg.Any<CancellationToken>())
            .Returns(EmptyCatalogos());

        using var form = new frmGuiaRemision(mediator, new Standalone(), 2024);
        await form.LoadAsync();

        await mediator.Received(1).Send(Arg.Any<ObtenerCatalogosGuiaQuery>(), Arg.Any<CancellationToken>());
    }

    [WinFormsFact]
    public async Task GrabarAsync_New_Sends_CrearGuiaCommand_Returns_True_On_Success()
    {
        var mediator = Substitute.For<IMediator>();
        mediator.Send(Arg.Any<ObtenerCatalogosGuiaQuery>(), Arg.Any<CancellationToken>())
            .Returns(EmptyCatalogos());
        mediator.Send(Arg.Any<CrearGuiaCommand>(), Arg.Any<CancellationToken>())
            .Returns(Result<int?>.Ok(42));

        using var form = new frmGuiaRemision(mediator, new Standalone(), 2024);
        await form.LoadAsync();
        form.SetTestFields(remitenteId: 1, destinatarioId: 2,
            dirPartida: "Av Lima 123|Lima|Lima",
            dirLlegada: "Av Arequipa 456|Arequipa|Arequipa");

        var success = await form.GrabarAsync();

        success.Should().BeTrue();
        await mediator.Received(1).Send(Arg.Any<CrearGuiaCommand>(), Arg.Any<CancellationToken>());
    }
}
