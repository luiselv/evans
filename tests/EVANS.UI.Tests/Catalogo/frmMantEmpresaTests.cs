using EVANS.Application.Catalogo.Commands;
using EVANS.Application.Catalogo.DTOs;
using EVANS.Application.Catalogo.Queries;
using EVANS.Application.Common;
using EVANS.Domain.Catalogo;
using EVANS.UI.WinForms.Catalogo;
using MediatR;

namespace EVANS.UI.Tests.Catalogo;

public sealed class FrmMantEmpresaTests
{
    [WinFormsFact]
    public void DesignerConstructor_InitializesLayoutWithoutRuntimeServices()
    {
        using var form = new frmMantEmpresa();

        form.Text.Should().Be("Registro de Empresas");
        form.ClientSize.Should().Be(new Size(635, 490));
        form.Controls.Find("TabControl1", searchAllChildren: true).Should().ContainSingle();
    }

    [WinFormsFact]
    public void Constructor_MatchesLegacyLayoutContract()
    {
        var mediator = Substitute.For<IMediator>();

        using var form = new frmMantEmpresa(mediator);

        form.Text.Should().Be("Registro de Empresas");
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
            .Should().Equal(("ID", 32), ("Nombre", 277), ("RUC", 108), ("Propia", 58));

        form.Controls.Find("txtCodigo", searchAllChildren: true).Single().Location.Should().Be(new Point(158, 22));
        form.Controls.Find("txtNombre", searchAllChildren: true).Single().Location.Should().Be(new Point(158, 59));
        form.Controls.Find("txtDireccion", searchAllChildren: true).Single().Location.Should().Be(new Point(158, 99));
        form.Controls.Find("txtTelefono", searchAllChildren: true).Single().Location.Should().Be(new Point(158, 139));
        form.Controls.Find("txtRUC", searchAllChildren: true).Single().Location.Should().Be(new Point(158, 178));
        form.Controls.Find("cbPropiedad", searchAllChildren: true).Single().Location.Should().Be(new Point(158, 213));
        form.Controls.Find("cbEstado", searchAllChildren: true).Single().Location.Should().Be(new Point(158, 253));
    }

    [WinFormsFact]
    public void Constructor_MatchesLegacyInitialState()
    {
        var mediator = Substitute.For<IMediator>();

        using var form = new frmMantEmpresa(mediator);

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
    public async Task LoadEmpresasAsync_DisplaysLegacyListColumnsDataIncludingInactive()
    {
        var mediator = CreateMediatorWithCatalogs();

        using var form = new frmMantEmpresa(mediator);

        await form.LoadEmpresasAsync();

        form.ListCount.Should().Be(2);
        form.FirstListName.Should().Be("EVANS CARGO");
        form.FirstListRuc.Should().Be("20123456789");
        form.FirstListPropiedad.Should().Be("SI");
    }

    [WinFormsFact]
    public async Task BuscarAsync_FiltersByNamePrefixAndKeepsLegacyBooleanText()
    {
        var mediator = CreateMediatorWithCatalogs();

        using var form = new frmMantEmpresa(mediator);
        form.SetTestSearchText("OLD");

        var success = await form.BuscarAsync();

        success.Should().BeTrue();
        form.ListCount.Should().Be(1);
        form.FirstListName.Should().Be("OLD CARRIER");
        form.FirstListPropiedad.Should().Be("False");
    }

    [WinFormsFact]
    public async Task BuscarAsync_EmptySearch_ShowsLegacyInsufficientDataMessage()
    {
        var mediator = Substitute.For<IMediator>();
        using var form = new frmMantEmpresa(mediator);
        form.SetTestSearchText("");

        var success = await form.BuscarAsync();

        success.Should().BeFalse();
        form.StatusMessage.Should().Be("Ingrese nombre a buscar");
        await mediator.DidNotReceive().Send(Arg.Any<ListEmpresasMaintenanceQuery>(), Arg.Any<CancellationToken>());
    }

    [WinFormsFact]
    public async Task SaveAsync_NewEmpresa_UppercasesTextAndSendsSelectedStatusAndProperty()
    {
        var mediator = CreateMediatorWithCatalogs();
        mediator.Send(Arg.Any<CreateEmpresaCommand>(), Arg.Any<CancellationToken>())
            .Returns(Result<int>.Ok(5));

        using var form = new frmMantEmpresa(mediator);
        await form.LoadEmpresasAsync();
        form.BeginNewForTest();
        form.SetTestNombre("empresa norte");
        form.SetTestDireccion("av lima");
        form.SetTestTelefono("555");
        form.SetTestRuc("20123456789");
        form.SetTestPropiedad("SI");
        form.SetTestEstado(CatalogoEstado.Inactivo);

        var success = await form.SaveAsync();

        success.Should().BeTrue();
        await mediator.Received(1).Send(
            Arg.Is<CreateEmpresaCommand>(command =>
                command.RazonSocial == "EMPRESA NORTE"
                && command.Direccion == "AV LIMA"
                && command.Telefono == "555"
                && command.Ruc == "20123456789"
                && command.EsPropia
                && command.EstadoCodigo == CatalogoEstado.Inactivo),
            Arg.Any<CancellationToken>());
    }

    [WinFormsFact]
    public async Task OpenAndEditEmpresa_UsesGetByIdThenUpdateCommand()
    {
        var mediator = CreateMediatorWithCatalogs();
        mediator.Send(Arg.Any<GetEmpresaByIdQuery>(), Arg.Any<CancellationToken>())
            .Returns(new EmpresaDto(2, "OLD CARRIER", "Av Norte", "555", "20987654321", false, CatalogoEstado.Activo));
        mediator.Send(Arg.Any<UpdateEmpresaCommand>(), Arg.Any<CancellationToken>())
            .Returns(Result<bool>.Ok(true));

        using var form = new frmMantEmpresa(mediator);
        await form.LoadEmpresasAsync();

        await form.OpenFirstForTestAsync();
        form.SelectedTabIndex.Should().Be(1);
        form.EditarEnabled.Should().BeTrue();

        form.BeginEditForTest();
        form.GrabarText.Should().Be("Actualizar");
        form.SetTestNombre("carrier sur");
        form.SetTestDireccion("av sur");
        form.SetTestTelefono("777");
        form.SetTestRuc("20123456789");
        form.SetTestPropiedad("NO");
        form.SetTestEstado(CatalogoEstado.Inactivo);

        var success = await form.SaveAsync();

        success.Should().BeTrue();
        await mediator.Received(1).Send(
            Arg.Is<UpdateEmpresaCommand>(command =>
                command.Codigo == 2
                && command.RazonSocial == "CARRIER SUR"
                && command.Direccion == "AV SUR"
                && command.Telefono == "777"
                && command.Ruc == "20123456789"
                && !command.EsPropia
                && command.EstadoCodigo == CatalogoEstado.Inactivo),
            Arg.Any<CancellationToken>());
    }

    private static IMediator CreateMediatorWithCatalogs()
    {
        var mediator = Substitute.For<IMediator>();
        mediator.Send(Arg.Any<ListEstadosQuery>(), Arg.Any<CancellationToken>())
            .Returns([new EstadoDto(CatalogoEstado.Activo, "ACTIVO"), new EstadoDto(CatalogoEstado.Inactivo, "INACTIVO")]);
        mediator.Send(Arg.Any<ListEmpresasMaintenanceQuery>(), Arg.Any<CancellationToken>())
            .Returns([
                new EmpresaDto(1, "EVANS CARGO", "Av Lima", "123", "20123456789", true, CatalogoEstado.Activo),
                new EmpresaDto(2, "OLD CARRIER", "Av Norte", "555", "20987654321", false, CatalogoEstado.Inactivo)
            ]);
        return mediator;
    }
}
