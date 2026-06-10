using EVANS.Application.Catalogo.Commands;
using EVANS.Application.Catalogo.DTOs;
using EVANS.Application.Catalogo.Queries;
using EVANS.Application.Common;
using EVANS.Domain.Catalogo;
using EVANS.UI.WinForms.Catalogo;
using MediatR;

namespace EVANS.UI.Tests.Catalogo;

public sealed class FrmMantChoferTests
{
    [WinFormsFact]
    public void DesignerConstructor_InitializesLayoutWithoutRuntimeServices()
    {
        using var form = new frmMantChofer();

        form.Text.Should().Be("Registro de Choferes");
        form.ClientSize.Should().Be(new Size(635, 490));
        form.Controls.Find("TabControl1", searchAllChildren: true).Should().ContainSingle();
    }

    [WinFormsFact]
    public void Constructor_MatchesLegacyLayoutContract()
    {
        var mediator = Substitute.For<IMediator>();

        using var form = new frmMantChofer(mediator);

        form.Text.Should().Be("Registro de Choferes");
        form.ClientSize.Should().Be(new Size(635, 490));
        form.Font.Name.Should().Be("Microsoft Sans Serif");
        form.Font.Size.Should().BeApproximately(8.25f, 0.01f);

        var btnNuevo = form.Controls.Find("btnNuevo", searchAllChildren: true).Single().Should().BeOfType<Button>().Subject;
        btnNuevo.Location.Should().Be(new Point(561, 86));
        btnNuevo.Size.Should().Be(new Size(62, 48));
        btnNuevo.Text.Should().Be("Nuevo");
        btnNuevo.Image.Should().NotBeNull();
        btnNuevo.TextAlign.Should().Be(ContentAlignment.BottomCenter);

        var btnGrabar = form.Controls.Find("btnGrabar", searchAllChildren: true).Single().Should().BeOfType<Button>().Subject;
        btnGrabar.Location.Should().Be(new Point(561, 140));
        btnGrabar.Size.Should().Be(new Size(62, 48));
        btnGrabar.Text.Should().Be("Grabar");
        btnGrabar.Image.Should().NotBeNull();
        btnGrabar.TextAlign.Should().Be(ContentAlignment.BottomCenter);

        form.Controls.Find("btnEditar", searchAllChildren: true).Single().Location.Should().Be(new Point(561, 194));
        form.Controls.Find("btnCancelar", searchAllChildren: true).Single().Location.Should().Be(new Point(561, 248));
        form.Controls.Find("btnSalir", searchAllChildren: true).Single().Location.Should().Be(new Point(561, 302));

        var tabControl = form.Controls.Find("TabControl1", searchAllChildren: true).Single();
        tabControl.Location.Should().Be(new Point(12, 12));
        tabControl.Size.Should().Be(new Size(537, 458));

        var lvListado = form.Controls.Find("lvListado", searchAllChildren: true).Single().Should().BeOfType<ListView>().Subject;
        lvListado.Location.Should().Be(new Point(20, 47));
        lvListado.Size.Should().Be(new Size(489, 341));
        lvListado.HeaderStyle.Should().Be(ColumnHeaderStyle.Nonclickable);
        lvListado.View.Should().Be(View.Details);
        lvListado.FullRowSelect.Should().BeTrue();
        lvListado.Columns.Cast<ColumnHeader>().Select(column => (column.Text, column.Width))
            .Should().Equal(("ID", 29), ("Nombre", 136), ("Licencia", 82), ("Empresa", 123));

        form.Controls.Find("txtCodigo", searchAllChildren: true).Single().Location.Should().Be(new Point(158, 37));
        form.Controls.Find("txtNombre", searchAllChildren: true).Single().Location.Should().Be(new Point(158, 74));
        form.Controls.Find("txtDireccion", searchAllChildren: true).Single().Location.Should().Be(new Point(158, 114));
        form.Controls.Find("txtLicencia", searchAllChildren: true).Single().Location.Should().Be(new Point(158, 153));
        form.Controls.Find("txtTelefono", searchAllChildren: true).Single().Location.Should().Be(new Point(158, 194));
        form.Controls.Find("cbEmpresa", searchAllChildren: true).Single().Location.Should().Be(new Point(158, 234));
        form.Controls.Find("cbEstado", searchAllChildren: true).Single().Location.Should().Be(new Point(158, 276));
    }

    [WinFormsFact]
    public void Constructor_MatchesLegacyInitialState()
    {
        var mediator = Substitute.For<IMediator>();

        using var form = new frmMantChofer(mediator);

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
    public async Task LoadChoferesAsync_DisplaysLegacyListColumnsDataIncludingInactive()
    {
        var mediator = CreateMediatorWithCatalogs();

        using var form = new frmMantChofer(mediator);

        await form.LoadChoferesAsync();

        form.ListCount.Should().Be(2);
        form.FirstListName.Should().Be("JUAN PEREZ");
        form.FirstListLicencia.Should().Be("Q12345678");
        form.FirstListEmpresa.Should().Be("EVANS CARGO");
    }

    [WinFormsFact]
    public async Task BuscarAsync_FiltersByNamePrefix()
    {
        var mediator = CreateMediatorWithCatalogs();

        using var form = new frmMantChofer(mediator);
        form.SetTestSearchText("PED");

        var success = await form.BuscarAsync();

        success.Should().BeTrue();
        form.ListCount.Should().Be(1);
        form.FirstListName.Should().Be("PEDRO DIAZ");
        form.FirstListEmpresa.Should().Be("EVANS CARGO");
    }

    [WinFormsFact]
    public async Task BuscarAsync_EmptySearch_ShowsLegacyInsufficientDataMessage()
    {
        var mediator = Substitute.For<IMediator>();
        using var form = new frmMantChofer(mediator);
        form.SetTestSearchText("");

        var success = await form.BuscarAsync();

        success.Should().BeFalse();
        form.StatusMessage.Should().Be("Ingrese nombre a buscar");
        await mediator.DidNotReceive().Send(Arg.Any<ListChoferesMaintenanceQuery>(), Arg.Any<CancellationToken>());
    }

    [WinFormsFact]
    public async Task SaveAsync_NewChofer_UppercasesTextAndSendsSelectedCompanyAndStatus()
    {
        var mediator = CreateMediatorWithCatalogs();
        mediator.Send(Arg.Any<CreateChoferCommand>(), Arg.Any<CancellationToken>())
            .Returns(Result<int>.Ok(5));

        using var form = new frmMantChofer(mediator);
        await form.LoadChoferesAsync();
        form.BeginNewForTest();
        form.SetTestNombre("carlos paz");
        form.SetTestDireccion("av lima");
        form.SetTestLicencia("q555");
        form.SetTestTelefono("555");
        form.SetTestEmpresa(2);
        form.SetTestEstado(CatalogoEstado.Inactivo);

        var success = await form.SaveAsync();

        success.Should().BeTrue();
        await mediator.Received(1).Send(
            Arg.Is<CreateChoferCommand>(command =>
                command.NombreCompleto == "CARLOS PAZ"
                && command.Direccion == "AV LIMA"
                && command.Licencia == "Q555"
                && command.Telefono == "555"
                && command.EmpresaCodigo == 2
                && command.EstadoCodigo == CatalogoEstado.Inactivo),
            Arg.Any<CancellationToken>());
    }

    [WinFormsFact]
    public async Task OpenAndEditChofer_UsesGetByIdThenUpdateCommand()
    {
        var mediator = CreateMediatorWithCatalogs();
        mediator.Send(Arg.Any<GetChoferByIdQuery>(), Arg.Any<CancellationToken>())
            .Returns(new ChoferDto(2, "PEDRO DIAZ", "Q87654321", "999", "Av Norte", 1, CatalogoEstado.Activo));
        mediator.Send(Arg.Any<UpdateChoferCommand>(), Arg.Any<CancellationToken>())
            .Returns(Result<bool>.Ok(true));

        using var form = new frmMantChofer(mediator);
        await form.LoadChoferesAsync();

        await form.OpenFirstForTestAsync();
        form.SelectedTabIndex.Should().Be(1);
        form.EditarEnabled.Should().BeTrue();

        form.BeginEditForTest();
        form.GrabarText.Should().Be("Actualizar");
        form.SetTestNombre("pedro sur");
        form.SetTestDireccion("av sur");
        form.SetTestLicencia("q999");
        form.SetTestTelefono("777");
        form.SetTestEmpresa(2);
        form.SetTestEstado(CatalogoEstado.Inactivo);

        var success = await form.SaveAsync();

        success.Should().BeTrue();
        await mediator.Received(1).Send(
            Arg.Is<UpdateChoferCommand>(command =>
                command.Codigo == 2
                && command.NombreCompleto == "PEDRO SUR"
                && command.Direccion == "AV SUR"
                && command.Licencia == "Q999"
                && command.Telefono == "777"
                && command.EmpresaCodigo == 2
                && command.EstadoCodigo == CatalogoEstado.Inactivo),
            Arg.Any<CancellationToken>());
    }

    private static IMediator CreateMediatorWithCatalogs()
    {
        var mediator = Substitute.For<IMediator>();
        mediator.Send(Arg.Any<ListEstadosQuery>(), Arg.Any<CancellationToken>())
            .Returns([new EstadoDto(CatalogoEstado.Activo, "ACTIVO"), new EstadoDto(CatalogoEstado.Inactivo, "INACTIVO")]);
        mediator.Send(Arg.Any<ListEmpresasQuery>(), Arg.Any<CancellationToken>())
            .Returns([
                new EmpresaDto(1, "EVANS CARGO", "Av Lima", "123", "20123456789", true, CatalogoEstado.Activo),
                new EmpresaDto(2, "NORTE SAC", "Av Norte", "555", "20987654321", false, CatalogoEstado.Activo)
            ]);
        mediator.Send(Arg.Any<ListEmpresasMaintenanceQuery>(), Arg.Any<CancellationToken>())
            .Returns([
                new EmpresaDto(1, "EVANS CARGO", "Av Lima", "123", "20123456789", true, CatalogoEstado.Activo),
                new EmpresaDto(2, "NORTE SAC", "Av Norte", "555", "20987654321", false, CatalogoEstado.Activo)
            ]);
        mediator.Send(Arg.Any<ListChoferesMaintenanceQuery>(), Arg.Any<CancellationToken>())
            .Returns([
                new ChoferDto(1, "JUAN PEREZ", "Q12345678", "555", "Av Lima", 1, CatalogoEstado.Activo),
                new ChoferDto(2, "PEDRO DIAZ", "Q87654321", null, null, 1, CatalogoEstado.Inactivo)
            ]);
        return mediator;
    }
}
