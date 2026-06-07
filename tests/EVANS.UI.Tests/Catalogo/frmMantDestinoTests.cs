using EVANS.Application.Catalogo.Commands;
using EVANS.Application.Catalogo.DTOs;
using EVANS.Application.Catalogo.Queries;
using EVANS.Application.Common;
using EVANS.Domain.Catalogo;
using EVANS.UI.WinForms.Catalogo;
using MediatR;

namespace EVANS.UI.Tests.Catalogo;

public sealed class FrmMantDestinoTests
{
    [WinFormsFact]
    public void DesignerConstructor_InitializesLayoutWithoutRuntimeServices()
    {
        using var form = new frmMantDestino();

        form.Text.Should().Be("Registro de Destinos");
        form.ClientSize.Should().Be(new Size(635, 490));
        form.Controls.Find("TabControl1", searchAllChildren: true).Should().ContainSingle();
    }

    [WinFormsFact]
    public void Constructor_MatchesLegacyLayoutContract()
    {
        var mediator = Substitute.For<IMediator>();

        using var form = new frmMantDestino(mediator);

        form.Text.Should().Be("Registro de Destinos");
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

        var txtBuscar = form.Controls.Find("txtBuscar", searchAllChildren: true).Single();
        txtBuscar.Location.Should().Be(new Point(200, 16));
        txtBuscar.Size.Should().Be(new Size(231, 20));

        var lvListado = form.Controls.Find("lvListado", searchAllChildren: true).Single().Should().BeOfType<ListView>().Subject;
        lvListado.Location.Should().Be(new Point(20, 47));
        lvListado.Size.Should().Be(new Size(489, 341));
        lvListado.HeaderStyle.Should().Be(ColumnHeaderStyle.Nonclickable);
        lvListado.View.Should().Be(View.Details);
        lvListado.FullRowSelect.Should().BeTrue();
        lvListado.Columns.Cast<ColumnHeader>().Select(column => (column.Text, column.Width))
            .Should().Equal(("ID", 31), ("Nombre", 125), ("Distancia Virtual (km)", 150));

        form.Controls.Find("txtCodigo", searchAllChildren: true).Single().Location.Should().Be(new Point(158, 22));
        form.Controls.Find("txtNombre", searchAllChildren: true).Single().Location.Should().Be(new Point(158, 59));
        form.Controls.Find("txtDistancia", searchAllChildren: true).Single().Location.Should().Be(new Point(158, 99));
        form.Controls.Find("cbEstado", searchAllChildren: true).Single().Location.Should().Be(new Point(158, 137));
        form.Controls.Find("Label4", searchAllChildren: true).Single().Location.Should().Be(new Point(108, 25));
        form.Controls.Find("Label1", searchAllChildren: true).Single().Location.Should().Be(new Point(104, 63));
        form.Controls.Find("Label2", searchAllChildren: true).Single().Location.Should().Be(new Point(65, 102));
        form.Controls.Find("Label5", searchAllChildren: true).Single().Location.Should().Be(new Point(108, 140));
    }

    [WinFormsFact]
    public void Constructor_MatchesLegacyInitialState()
    {
        var mediator = Substitute.For<IMediator>();

        using var form = new frmMantDestino(mediator);

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
    public async Task LoadDestinosAsync_DisplaysLegacyListColumnsDataIncludingInactive()
    {
        var mediator = CreateMediatorWithCatalogs();

        using var form = new frmMantDestino(mediator);

        await form.LoadDestinosAsync();

        form.ListCount.Should().Be(2);
        form.FirstListName.Should().Be("LIMA");
        form.FirstListDistance.Should().Be("0");
    }

    [WinFormsFact]
    public async Task BuscarAsync_FiltersByNamePrefix()
    {
        var mediator = CreateMediatorWithCatalogs();

        using var form = new frmMantDestino(mediator);
        form.SetTestSearchText("CAL");

        var success = await form.BuscarAsync();

        success.Should().BeTrue();
        form.ListCount.Should().Be(1);
        form.FirstListName.Should().Be("CALLAO");
    }

    [WinFormsFact]
    public async Task BuscarAsync_EmptySearch_ShowsLegacyInsufficientDataMessage()
    {
        var mediator = Substitute.For<IMediator>();
        using var form = new frmMantDestino(mediator);
        form.SetTestSearchText("");

        var success = await form.BuscarAsync();

        success.Should().BeFalse();
        form.StatusMessage.Should().Be("Ingrese nombre a buscar");
        await mediator.DidNotReceive().Send(Arg.Any<ListDestinosMaintenanceQuery>(), Arg.Any<CancellationToken>());
    }

    [WinFormsFact]
    public async Task SaveAsync_NewDestino_UppercasesNameAndSendsSelectedStatus()
    {
        var mediator = CreateMediatorWithCatalogs();
        mediator.Send(Arg.Any<CreateDestinoCommand>(), Arg.Any<CancellationToken>())
            .Returns(Result<int>.Ok(5));

        using var form = new frmMantDestino(mediator);
        await form.LoadDestinosAsync();
        form.BeginNewForTest();
        form.SetTestNombre("piura");
        form.SetTestDistancia("12.5");
        form.SetTestEstado(CatalogoEstado.Inactivo);

        var success = await form.SaveAsync();

        success.Should().BeTrue();
        await mediator.Received(1).Send(
            Arg.Is<CreateDestinoCommand>(command =>
                command.Descripcion == "PIURA"
                && command.DistanciaVirtual == 12.5
                && command.EstadoCodigo == CatalogoEstado.Inactivo),
            Arg.Any<CancellationToken>());
    }

    [WinFormsFact]
    public async Task OpenAndEditDestino_UsesGetByIdThenUpdateCommand()
    {
        var mediator = CreateMediatorWithCatalogs();
        mediator.Send(Arg.Any<GetDestinoByIdQuery>(), Arg.Any<CancellationToken>())
            .Returns(new DestinoDto(2, "CALLAO", 15, CatalogoEstado.Activo));
        mediator.Send(Arg.Any<UpdateDestinoCommand>(), Arg.Any<CancellationToken>())
            .Returns(Result<bool>.Ok(true));

        using var form = new frmMantDestino(mediator);
        await form.LoadDestinosAsync();

        await form.OpenFirstForTestAsync();
        form.SelectedTabIndex.Should().Be(1);
        form.EditarEnabled.Should().BeTrue();

        form.BeginEditForTest();
        form.GrabarText.Should().Be("Actualizar");
        form.SetTestNombre("callao norte");
        form.SetTestDistancia("16");
        form.SetTestEstado(CatalogoEstado.Inactivo);

        var success = await form.SaveAsync();

        success.Should().BeTrue();
        await mediator.Received(1).Send(
            Arg.Is<UpdateDestinoCommand>(command =>
                command.Codigo == 2
                && command.Descripcion == "CALLAO NORTE"
                && command.DistanciaVirtual == 16
                && command.EstadoCodigo == CatalogoEstado.Inactivo),
            Arg.Any<CancellationToken>());
    }

    private static IMediator CreateMediatorWithCatalogs()
    {
        var mediator = Substitute.For<IMediator>();
        mediator.Send(Arg.Any<ListEstadosQuery>(), Arg.Any<CancellationToken>())
            .Returns([new EstadoDto(CatalogoEstado.Activo, "ACTIVO"), new EstadoDto(CatalogoEstado.Inactivo, "INACTIVO")]);
        mediator.Send(Arg.Any<ListDestinosMaintenanceQuery>(), Arg.Any<CancellationToken>())
            .Returns([
                new DestinoDto(1, "LIMA", 0, CatalogoEstado.Activo),
                new DestinoDto(2, "CALLAO", 15, CatalogoEstado.Inactivo)
            ]);
        return mediator;
    }
}
