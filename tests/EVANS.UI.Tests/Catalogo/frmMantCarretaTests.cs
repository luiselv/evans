using EVANS.Application.Catalogo.Commands;
using EVANS.Application.Catalogo.DTOs;
using EVANS.Application.Catalogo.Queries;
using EVANS.Application.Common;
using EVANS.Domain.Catalogo;
using EVANS.UI.WinForms.Catalogo;
using MediatR;

namespace EVANS.UI.Tests.Catalogo;

public sealed class FrmMantCarretaTests
{
    [WinFormsFact]
    public void DesignerConstructor_InitializesLayoutWithoutRuntimeServices()
    {
        using var form = new frmMantCarreta();
        form.Text.Should().Be("Registro de Carretas");
        form.ClientSize.Should().Be(new Size(635, 490));
        form.Controls.Find("TabControl1", true).Should().ContainSingle();
    }

    [WinFormsFact]
    public void Constructor_MatchesLegacyLayoutContract()
    {
        using var form = new frmMantCarreta(Substitute.For<IMediator>());
        form.Text.Should().Be("Registro de Carretas");
        form.ClientSize.Should().Be(new Size(635, 490));
        form.Font.Name.Should().Be("Microsoft Sans Serif");
        form.Font.Size.Should().BeApproximately(8.25f, 0.01f);

        var btnNuevo = form.Controls.Find("btnNuevo", true).Single().Should().BeOfType<Button>().Subject;
        btnNuevo.Location.Should().Be(new Point(561, 86));
        btnNuevo.Size.Should().Be(new Size(62, 48));
        btnNuevo.Image.Should().NotBeNull();
        btnNuevo.TextAlign.Should().Be(ContentAlignment.BottomCenter);
        form.Controls.Find("btnGrabar", true).Single().Location.Should().Be(new Point(561, 140));
        form.Controls.Find("btnEditar", true).Single().Location.Should().Be(new Point(561, 194));
        form.Controls.Find("btnCancelar", true).Single().Location.Should().Be(new Point(561, 248));
        var btnSalir = form.Controls.Find("btnSalir", true).Single().Should().BeOfType<Button>().Subject;
        btnSalir.Location.Should().Be(new Point(561, 302));
        foreach (var name in new[] { "btnGrabar", "btnEditar", "btnCancelar", "btnSalir", "btnBuscar" })
            form.Controls.Find(name, true).Single().Should().BeOfType<Button>().Subject.Image.Should().NotBeNull();

        var tabControl = form.Controls.Find("TabControl1", true).Single();
        tabControl.Location.Should().Be(new Point(12, 12));
        tabControl.Size.Should().Be(new Size(537, 458));

        var list = form.Controls.Find("lvListado", true).Single().Should().BeOfType<ListView>().Subject;
        list.Location.Should().Be(new Point(20, 47));
        list.Size.Should().Be(new Size(489, 341));
        list.View.Should().Be(View.Details);
        list.FullRowSelect.Should().BeTrue();
        list.HeaderStyle.Should().Be(ColumnHeaderStyle.Nonclickable);
        list.Columns.Cast<ColumnHeader>().Select(c => (c.Text, c.Width)).Should()
            .Equal(("ID", 32), ("Marca", 161), ("Placa", 86), ("Empresa", 196));

        form.Controls.Find("txtBuscar", true).Single().Location.Should().Be(new Point(200, 16));
        form.Controls.Find("txtCodigo", true).Single().Location.Should().Be(new Point(190, 40));
        form.Controls.Find("txtMarca", true).Single().Location.Should().Be(new Point(190, 77));
        form.Controls.Find("txtPlaca", true).Single().Location.Should().Be(new Point(190, 113));
        form.Controls.Find("txtCertificado", true).Single().Location.Should().Be(new Point(190, 149));
        form.Controls.Find("cbEmpresa", true).Single().Location.Should().Be(new Point(190, 186));
        form.Controls.Find("cbEstado", true).Single().Location.Should().Be(new Point(190, 222));
        form.Controls.Find("Label11", true).Single().Text.Should().Be("*");
        form.Controls.Find("Label7", true).Single().Text.Should().Be("*");
        form.Controls.Find("Label12", true).Single().Text.Should().Be("*");
        form.Controls.Find("Label2", true).Single().Text.Should().Be("*");
        form.Controls.Find("Label2", true).Single().Font.Bold.Should().BeTrue();
    }

    [WinFormsFact]
    public void Constructor_MatchesLegacyInitialState()
    {
        using var form = new frmMantCarreta(Substitute.For<IMediator>());
        form.SelectedTabIndex.Should().Be(0);
        form.ListingTabEnabled.Should().BeTrue();
        form.DetailsTabEnabled.Should().BeFalse();
        form.BuscarTextEnabled.Should().BeTrue();
        form.BuscarEnabled.Should().BeTrue();
        form.NuevoEnabled.Should().BeTrue();
        form.GrabarEnabled.Should().BeFalse();
        form.EditarEnabled.Should().BeFalse();
        form.CancelarEnabled.Should().BeFalse();
        form.GrabarText.Should().Be("Grabar");
    }

    [WinFormsFact]
    public async Task LoadCarretasAsync_DisplaysActiveAndInactiveLegacyRows()
    {
        var mediator = CreateMediatorWithCatalogs();
        using var form = new frmMantCarreta(mediator);

        await form.LoadCarretasAsync();

        form.ListCount.Should().Be(2);
        form.FirstListMarca.Should().Be("VOLVO");
        form.FirstListPlaca.Should().Be("XYZ-789");
        form.FirstListEmpresa.Should().Be("EVANS CARGO");
    }

    [WinFormsFact]
    public async Task BuscarAsync_FiltersByPlatePrefix()
    {
        var mediator = CreateMediatorWithCatalogs();
        using var form = new frmMantCarreta(mediator);
        form.SetTestSearchText("ABC");

        var success = await form.BuscarAsync();

        success.Should().BeTrue();
        form.ListCount.Should().Be(1);
        form.FirstListPlaca.Should().Be("ABC-123");
    }

    [WinFormsFact]
    public async Task BuscarAsync_EmptySearch_PreservesLegacyMessage()
    {
        var mediator = Substitute.For<IMediator>();
        using var form = new frmMantCarreta(mediator);
        form.SetTestSearchText("");

        var success = await form.BuscarAsync();

        success.Should().BeFalse();
        form.StatusMessage.Should().Be("Ingrese nombre a buscar");
        await mediator.DidNotReceive().Send(Arg.Any<ListCarretasMaintenanceQuery>(), Arg.Any<CancellationToken>());
    }

    [WinFormsFact]
    public async Task SaveAsync_NewCarreta_SendsUppercaseValuesAndSelectedCatalogs()
    {
        var mediator = CreateMediatorWithCatalogs();
        mediator.Send(Arg.Any<CreateCarretaCommand>(), Arg.Any<CancellationToken>()).Returns(Result<int>.Ok(3));
        using var form = new frmMantCarreta(mediator);
        await form.LoadCarretasAsync();
        form.BeginNewForTest();
        form.SetTestMarca("scania");
        form.SetTestPlaca("new-123");
        form.SetTestCertificado("cert-9");
        form.SetTestEmpresa(2);
        form.SetTestEstado(CatalogoEstado.Inactivo);

        var success = await form.SaveAsync();

        success.Should().BeTrue();
        await mediator.Received(1).Send(
            Arg.Is<CreateCarretaCommand>(c => c.Marca == "SCANIA" && c.Placa == "NEW-123"
                && c.Certificado == "CERT-9" && c.EmpresaCodigo == 2
                && c.EstadoCodigo == CatalogoEstado.Inactivo),
            Arg.Any<CancellationToken>());
    }

    [WinFormsFact]
    public async Task OpenAndEditCarreta_UsesGetByIdThenUpdateCommand()
    {
        var mediator = CreateMediatorWithCatalogs();
        mediator.Send(Arg.Any<GetCarretaByIdQuery>(), Arg.Any<CancellationToken>())
            .Returns(new CarretaDto(1, "XYZ-789", "VOLVO", "CERT", 1, CatalogoEstado.Activo));
        mediator.Send(Arg.Any<UpdateCarretaCommand>(), Arg.Any<CancellationToken>()).Returns(Result<bool>.Ok(true));
        using var form = new frmMantCarreta(mediator);
        await form.LoadCarretasAsync();

        await form.OpenFirstForTestAsync();
        form.SelectedTabIndex.Should().Be(1);
        form.CodigoText.Should().Be("1");
        form.EditarEnabled.Should().BeTrue();
        form.BeginEditForTest();
        form.SetTestMarca("scania");
        form.SetTestPlaca("upd-999");
        form.SetTestCertificado("new-cert");
        form.SetTestEmpresa(2);
        form.SetTestEstado(CatalogoEstado.Inactivo);

        var success = await form.SaveAsync();

        success.Should().BeTrue();
        await mediator.Received(1).Send(
            Arg.Is<UpdateCarretaCommand>(c => c.Codigo == 1 && c.Marca == "SCANIA"
                && c.Placa == "UPD-999" && c.Certificado == "NEW-CERT"
                && c.EmpresaCodigo == 2 && c.EstadoCodigo == CatalogoEstado.Inactivo),
            Arg.Any<CancellationToken>());
    }

    private static IMediator CreateMediatorWithCatalogs()
    {
        var mediator = Substitute.For<IMediator>();
        mediator.Send(Arg.Any<ListEstadosQuery>(), Arg.Any<CancellationToken>()).Returns([
            new EstadoDto(CatalogoEstado.Activo, "ACTIVO"),
            new EstadoDto(CatalogoEstado.Inactivo, "INACTIVO")]);
        mediator.Send(Arg.Any<ListEmpresasQuery>(), Arg.Any<CancellationToken>()).Returns([
            new EmpresaDto(1, "EVANS CARGO", "Av Lima", "123", "20123456789", true, CatalogoEstado.Activo),
            new EmpresaDto(2, "NORTE SAC", "Av Norte", "555", "20987654321", false, CatalogoEstado.Activo)]);
        mediator.Send(Arg.Any<ListEmpresasMaintenanceQuery>(), Arg.Any<CancellationToken>()).Returns([
            new EmpresaDto(1, "EVANS CARGO", "Av Lima", "123", "20123456789", true, CatalogoEstado.Activo),
            new EmpresaDto(2, "NORTE SAC", "Av Norte", "555", "20987654321", false, CatalogoEstado.Activo)]);
        mediator.Send(Arg.Any<ListCarretasMaintenanceQuery>(), Arg.Any<CancellationToken>()).Returns([
            new CarretaDto(1, "XYZ-789", "VOLVO", "CERT", 1, CatalogoEstado.Activo),
            new CarretaDto(2, "ABC-123", "SCANIA", null, 1, CatalogoEstado.Inactivo)]);
        return mediator;
    }
}
