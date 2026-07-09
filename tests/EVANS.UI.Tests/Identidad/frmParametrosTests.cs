using EVANS.Application.Common;
using EVANS.Application.Shared.Commands;
using EVANS.Application.Shared.DTOs;
using EVANS.Application.Shared.Ports;
using EVANS.UI.WinForms.Identidad;
using MediatR;

namespace EVANS.UI.Tests.Identidad;

public sealed class FrmParametrosTests
{
    [WinFormsFact]
    public void DesignerConstructor_InitializesLayoutWithoutRuntimeServices()
    {
        using var form = new frmParametros();

        form.Text.Should().Be("Parámetros");
        form.ClientSize.Should().Be(new Size(403, 289));
        form.Controls.Find("GroupBox1", searchAllChildren: true).Should().ContainSingle();
    }

    [WinFormsFact]
    public void Constructor_MatchesLegacyLayoutContract()
    {
        using var form = new frmParametros(Substitute.For<IMediator>(), Substitute.For<IParametrosService>());

        form.Text.Should().Be("Parámetros");
        form.ClientSize.Should().Be(new Size(403, 289));
        form.Controls.Find("GroupBox1", true).Single().Location.Should().Be(new Point(12, 12));
        form.Controls.Find("GroupBox1", true).Single().Size.Should().Be(new Size(188, 88));
        form.Controls.Find("GroupBox2", true).Single().Location.Should().Be(new Point(206, 12));
        form.Controls.Find("GroupBox3", true).Single().Location.Should().Be(new Point(12, 106));
        form.Controls.Find("GroupBox5", true).Single().Location.Should().Be(new Point(206, 106));
        form.Controls.Find("GroupBox4", true).Single().Location.Should().Be(new Point(126, 200));

        var btnEditar = form.Controls.Find("btnEditar", true).Single().Should().BeOfType<Button>().Subject;
        btnEditar.Location.Should().Be(new Point(14, 19));
        btnEditar.Size.Should().Be(new Size(62, 48));
        btnEditar.Image.Should().NotBeNull();
        btnEditar.TextAlign.Should().Be(ContentAlignment.BottomCenter);

        var btnGrabar = form.Controls.Find("btnGrabar", true).Single().Should().BeOfType<Button>().Subject;
        btnGrabar.Location.Should().Be(new Point(83, 19));
        btnGrabar.Size.Should().Be(new Size(62, 48));
        btnGrabar.Image.Should().NotBeNull();
        btnGrabar.Enabled.Should().BeFalse();

        form.Controls.Find("txtFacturaSerie", true).Single().Location.Should().Be(new Point(84, 28));
        form.Controls.Find("txtFacturaNro1", true).Single().Location.Should().Be(new Point(84, 54));
        form.Controls.Find("txtBoletaSerie", true).Single().Location.Should().Be(new Point(85, 25));
        form.Controls.Find("txtBoletaNro1", true).Single().Location.Should().Be(new Point(85, 54));
        form.Controls.Find("txtGRemisionSerie", true).Single().Location.Should().Be(new Point(85, 27));
        form.Controls.Find("txtGRemisionNro1", true).Single().Location.Should().Be(new Point(85, 53));
        form.Controls.Find("txtIGV", true).Single().Location.Should().Be(new Point(85, 27));
        form.Controls.Find("txtManifiesto", true).Single().Location.Should().Be(new Point(85, 53));
    }

    [WinFormsFact]
    public async Task LoadParametrosAsync_BindsValuesAndLeavesFieldsReadOnly()
    {
        var service = Substitute.For<IParametrosService>();
        service.ObtenerParametrosAsync(Arg.Any<CancellationToken>()).Returns(Parametros());

        using var form = new frmParametros(Substitute.For<IMediator>(), service);

        await form.LoadParametrosAsync();

        form.IgvText.Should().Be("0.18");
        form.FacturaSerieText.Should().Be("F001");
        form.BoletaSerieText.Should().Be("B001");
        form.GuiaRemisionSerieText.Should().Be("GR01");
        form.ManifiestoText.Should().Be("M001");
        form.FieldsReadOnly.Should().BeTrue();
        form.GrabarEnabled.Should().BeFalse();
        form.EditarEnabled.Should().BeTrue();
    }

    [WinFormsFact]
    public async Task BeginEdit_EnablesFieldsAndSaveButton()
    {
        var service = Substitute.For<IParametrosService>();
        service.ObtenerParametrosAsync(Arg.Any<CancellationToken>()).Returns(Parametros());

        using var form = new frmParametros(Substitute.For<IMediator>(), service);
        await form.LoadParametrosAsync();

        form.BeginEditForTest();

        form.FieldsReadOnly.Should().BeFalse();
        form.GrabarEnabled.Should().BeTrue();
        form.EditarEnabled.Should().BeFalse();
    }

    [WinFormsFact]
    public async Task SaveAsync_SendsUpdateCommandAndRestoresReadOnlyState()
    {
        var mediator = Substitute.For<IMediator>();
        mediator.Send(Arg.Any<ActualizarParametrosCommand>(), Arg.Any<CancellationToken>())
            .Returns(Result<bool>.Ok(true));
        var service = Substitute.For<IParametrosService>();
        service.ObtenerParametrosAsync(Arg.Any<CancellationToken>()).Returns(Parametros(), Parametros() with { FacturaSerie = "F002" });

        using var form = new frmParametros(mediator, service);
        await form.LoadParametrosAsync();
        form.BeginEditForTest();
        form.SetTestIgv("0.19");
        form.SetTestFacturaSerie("F002");
        form.SetTestFacturaNro1("000123");
        form.SetTestBoletaSerie("B002");
        form.SetTestBoletaNro1("000456");
        form.SetTestGuiaRemisionSerie("GR02");
        form.SetTestGuiaRemisionNro1("000789");
        form.SetTestManifiesto("M002");

        var success = await form.SaveAsync();

        success.Should().BeTrue();
        form.FieldsReadOnly.Should().BeTrue();
        form.GrabarEnabled.Should().BeFalse();
        form.EditarEnabled.Should().BeTrue();
        await mediator.Received(1).Send(
            Arg.Is<ActualizarParametrosCommand>(command =>
                command.Parametros.IgvRate == 0.19m &&
                command.Parametros.FacturaSerie == "F002" &&
                command.Parametros.FacturaNro1 == "000123" &&
                command.Parametros.FacturaNro2 == "0" &&
                command.Parametros.BoletaSerie == "B002" &&
                command.Parametros.BoletaNro1 == "000456" &&
                command.Parametros.BoletaNro2 == "0" &&
                command.Parametros.GuiaRemisionSerie == "GR02" &&
                command.Parametros.GuiaRemisionNro1 == "000789" &&
                command.Parametros.GuiaRemisionNro2 == "0" &&
                command.Parametros.Manifiesto == "M002" &&
                command.Parametros.EmailRemitente == "evans@example.com"),
            Arg.Any<CancellationToken>());
    }

    [WinFormsFact]
    public async Task SaveAsync_InvalidIgv_DoesNotSendCommand()
    {
        var mediator = Substitute.For<IMediator>();
        var service = Substitute.For<IParametrosService>();
        service.ObtenerParametrosAsync(Arg.Any<CancellationToken>()).Returns(Parametros());

        using var form = new frmParametros(mediator, service);
        await form.LoadParametrosAsync();
        form.BeginEditForTest();
        form.SetTestIgv("0");

        var success = await form.SaveAsync();

        success.Should().BeFalse();
        form.StatusMessage.Should().Be("IGV inválido.");
        await mediator.DidNotReceive().Send(Arg.Any<ActualizarParametrosCommand>(), Arg.Any<CancellationToken>());
    }

    private static ParametrosDto Parametros() => new(
        IgvRate: 0.18m,
        FacturaSerie: "F001",
        FacturaNro1: "000001",
        FacturaNro2: "0",
        BoletaSerie: "B001",
        BoletaNro1: "000002",
        BoletaNro2: "0",
        GuiaRemisionSerie: "GR01",
        GuiaRemisionNro1: "000003",
        GuiaRemisionNro2: "0",
        Manifiesto: "M001",
        Remitente: "EVANS",
        EmailRemitente: "evans@example.com",
        PassRemitente: "secret",
        Smtp: "smtp.example.com",
        Puerto: 587);
}
