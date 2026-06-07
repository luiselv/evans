using EVANS.Application.Common;
using EVANS.Application.Identidad.DTOs;
using EVANS.Application.Identidad.Queries;
using EVANS.UI.WinForms.Identidad;
using MediatR;

namespace EVANS.UI.Tests.Identidad;

public sealed class FrmConsultaRucTests
{
    [WinFormsFact]
    public void Constructor_UsesIntentionalDirectQueryDesignInsteadOfLegacyWebBrowser()
    {
        var mediator = Substitute.For<IMediator>();

        using var form = new frmConsultaRuc(mediator);

        form.Text.Should().Be("Consulta RUC");
        form.ClientSize.Should().Be(new Size(464, 291));
        form.Controls.OfType<WebBrowser>().Should().BeEmpty();
        form.Controls.OfType<TextBox>().Should().ContainSingle();
        form.Controls.OfType<Button>().Should().HaveCount(2);
    }

    [WinFormsFact]
    public async Task ConsultarAsync_ValidRuc_DisplaysRazonSocial()
    {
        var mediator = Substitute.For<IMediator>();
        mediator.Send(Arg.Any<ConsultarRucQuery>(), Arg.Any<CancellationToken>())
            .Returns(Result<SunatRucDto>.Ok(new SunatRucDto(
                "20123456789",
                "ACME SAC",
                "ACTIVO",
                "HABIDO",
                "AV LIMA 123")));

        using var form = new frmConsultaRuc(mediator);
        form.SetTestRuc("20123456789");

        var success = await form.ConsultarAsync();

        success.Should().BeTrue();
        form.RazonSocial.Should().Be("ACME SAC");
        await mediator.Received(1).Send(
            Arg.Is<ConsultarRucQuery>(query => query.Ruc == "20123456789"),
            Arg.Any<CancellationToken>());
    }

    [WinFormsFact]
    public async Task ConsultarAsync_Failure_DisplaysInlineError()
    {
        var mediator = Substitute.For<IMediator>();
        mediator.Send(Arg.Any<ConsultarRucQuery>(), Arg.Any<CancellationToken>())
            .Returns(Result<SunatRucDto>.Fail("Invalid RUC."));

        using var form = new frmConsultaRuc(mediator);
        form.SetTestRuc("123");

        var success = await form.ConsultarAsync();

        success.Should().BeFalse();
        form.ErrorMessage.Should().Be("Invalid RUC.");
    }
}
