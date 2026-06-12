using EVANS.Application.Catalogo.Commands;
using EVANS.Application.Catalogo.DTOs;
using EVANS.Application.Catalogo.Queries;
using EVANS.Application.Common;
using EVANS.UI.WinForms.Catalogo;
using MediatR;

namespace EVANS.UI.Tests.Catalogo;

public sealed class FrmMantTipoIDTests
{
    [WinFormsFact]
    public void DesignerConstructor_InitializesLayoutWithoutRuntimeServices()
    {
        using var form = new frmMantTipoID();

        form.Text.Should().Be("Registro de Tipos de Identificación");
        form.ClientSize.Should().Be(new Size(635, 490));
        form.Controls.Find("TabControl1", searchAllChildren: true).Should().ContainSingle();
    }

    [WinFormsFact]
    public void Constructor_MatchesLegacyLayoutContract()
    {
        var mediator = Substitute.For<IMediator>();

        using var form = new frmMantTipoID(mediator);

        form.Text.Should().Be("Registro de Tipos de Identificación");
        form.ClientSize.Should().Be(new Size(635, 490));
        form.Font.Name.Should().Be("Microsoft Sans Serif");
        form.Font.Size.Should().BeApproximately(8.25f, 0.01f);

        var btnNuevo = form.Controls.Find("btnNuevo", searchAllChildren: true).Single();
        btnNuevo.Location.Should().Be(new Point(561, 82));
        btnNuevo.Size.Should().Be(new Size(62, 48));
        btnNuevo.Text.Should().Be("Nuevo");
        btnNuevo.Should().BeOfType<Button>().Subject.Image.Should().NotBeNull();
        btnNuevo.Should().BeOfType<Button>().Subject.TextAlign.Should().Be(ContentAlignment.BottomCenter);

        var btnGrabar = form.Controls.Find("btnGrabar", searchAllChildren: true).Single();
        btnGrabar.Location.Should().Be(new Point(561, 136));
        btnGrabar.Size.Should().Be(new Size(62, 48));
        btnGrabar.Text.Should().Be("Grabar");
        btnGrabar.Should().BeOfType<Button>().Subject.Image.Should().NotBeNull();
        btnGrabar.Should().BeOfType<Button>().Subject.TextAlign.Should().Be(ContentAlignment.BottomCenter);

        var btnEditar = form.Controls.Find("btnEditar", searchAllChildren: true).Single();
        btnEditar.Location.Should().Be(new Point(561, 190));
        btnEditar.Size.Should().Be(new Size(62, 48));
        btnEditar.Text.Should().Be("Editar");
        btnEditar.Should().BeOfType<Button>().Subject.Image.Should().NotBeNull();
        btnEditar.Should().BeOfType<Button>().Subject.TextAlign.Should().Be(ContentAlignment.BottomCenter);

        var btnCancelar = form.Controls.Find("btnCancelar", searchAllChildren: true).Single();
        btnCancelar.Location.Should().Be(new Point(561, 244));
        btnCancelar.Size.Should().Be(new Size(62, 48));
        btnCancelar.Text.Should().Be("Cancelar");
        btnCancelar.Should().BeOfType<Button>().Subject.Image.Should().NotBeNull();
        btnCancelar.Should().BeOfType<Button>().Subject.TextAlign.Should().Be(ContentAlignment.BottomCenter);

        var btnSalir = form.Controls.Find("btnSalir", searchAllChildren: true).Single();
        btnSalir.Location.Should().Be(new Point(561, 298));
        btnSalir.Size.Should().Be(new Size(62, 48));
        btnSalir.Text.Should().Be("Salir");
        btnSalir.Should().BeOfType<Button>().Subject.Image.Should().NotBeNull();
        btnSalir.Should().BeOfType<Button>().Subject.TextAlign.Should().Be(ContentAlignment.BottomCenter);

        var tabControl = form.Controls.Find("TabControl1", searchAllChildren: true).Single();
        tabControl.Location.Should().Be(new Point(12, 12));
        tabControl.Size.Should().Be(new Size(537, 458));

        var tabListado = form.Controls.Find("TabPage1", searchAllChildren: true).Single();
        tabListado.Location.Should().Be(new Point(4, 23));
        tabListado.Size.Should().Be(new Size(529, 431));

        var tabDetalles = form.Controls.Find("TabPage2", searchAllChildren: true).Single();
        tabDetalles.Location.Should().Be(new Point(4, 23));
        tabDetalles.Size.Should().Be(new Size(529, 431));

        var txtBuscar = form.Controls.Find("txtBuscar", searchAllChildren: true).Single();
        txtBuscar.Location.Should().Be(new Point(171, 16));
        txtBuscar.Size.Should().Be(new Size(260, 20));

        var btnBuscar = form.Controls.Find("btnBuscar", searchAllChildren: true).Single().Should().BeOfType<Button>().Subject;
        btnBuscar.Image.Should().NotBeNull();
        btnBuscar.ImageAlign.Should().Be(ContentAlignment.MiddleLeft);
        btnBuscar.TextAlign.Should().Be(ContentAlignment.MiddleRight);

        var lvListado = form.Controls.Find("lvListado", searchAllChildren: true).Single().Should().BeOfType<ListView>().Subject;
        lvListado.Location.Should().Be(new Point(20, 47));
        lvListado.Size.Should().Be(new Size(489, 341));
        lvListado.HeaderStyle.Should().Be(ColumnHeaderStyle.Nonclickable);
        lvListado.View.Should().Be(View.Details);
        lvListado.FullRowSelect.Should().BeTrue();
        lvListado.Columns.Cast<ColumnHeader>().Select(column => (column.Text, column.Width))
            .Should().Equal(("ID", 69), ("Descripción", 409));

        var txtDescripcion = form.Controls.Find("txtDescripcion", searchAllChildren: true).Single();
        txtDescripcion.Location.Should().Be(new Point(201, 84));
        txtDescripcion.Size.Should().Be(new Size(145, 20));

        var txtCodigo = form.Controls.Find("txtCodigo", searchAllChildren: true).Single().Should().BeOfType<TextBox>().Subject;
        txtCodigo.Location.Should().Be(new Point(201, 47));
        txtCodigo.Size.Should().Be(new Size(145, 20));
        txtCodigo.ReadOnly.Should().BeTrue();

        var lblCodigo = form.Controls.Find("Label4", searchAllChildren: true).Single();
        lblCodigo.Location.Should().Be(new Point(143, 50));
        lblCodigo.Size.Should().Be(new Size(46, 13));

        var lblDescripcion = form.Controls.Find("Label1", searchAllChildren: true).Single();
        lblDescripcion.Location.Should().Be(new Point(120, 87));
        lblDescripcion.Size.Should().Be(new Size(69, 13));

        form.Controls.Find("Label10", searchAllChildren: true).Single().Text.Should().Be("*");
        form.Controls.Find("Label11", searchAllChildren: true).Single().Text.Should().Be("*");
    }

    [WinFormsFact]
    public void Constructor_MatchesLegacyInitialState()
    {
        var mediator = Substitute.For<IMediator>();

        using var form = new frmMantTipoID(mediator);

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
    public async Task LoadTiposIdentificacionAsync_DisplaysLegacyListColumnsData()
    {
        var mediator = Substitute.For<IMediator>();
        mediator.Send(Arg.Any<ListTiposIdentificacionQuery>(), Arg.Any<CancellationToken>())
            .Returns([new TipoIdentificacionDto(1, "RUC", 11), new TipoIdentificacionDto(2, "DNI", 8)]);

        using var form = new frmMantTipoID(mediator);

        await form.LoadTiposIdentificacionAsync();

        form.ListCount.Should().Be(2);
        form.FirstListDescription.Should().Be("RUC");
    }

    [WinFormsFact]
    public async Task BuscarAsync_EmptySearch_ShowsLegacyInsufficientDataMessage()
    {
        var mediator = Substitute.For<IMediator>();
        using var form = new frmMantTipoID(mediator);
        form.SetTestSearchText("");

        var success = await form.BuscarAsync();

        success.Should().BeFalse();
        form.StatusMessage.Should().Be("Ingrese nombre a buscar");
        await mediator.DidNotReceive().Send(Arg.Any<ListTiposIdentificacionQuery>(), Arg.Any<CancellationToken>());
    }

    [WinFormsFact]
    public async Task BuscarAsync_FiltersByNamePrefix()
    {
        var mediator = Substitute.For<IMediator>();
        mediator.Send(Arg.Any<ListTiposIdentificacionQuery>(), Arg.Any<CancellationToken>())
            .Returns([new TipoIdentificacionDto(1, "RUC", 11), new TipoIdentificacionDto(2, "DNI", 8)]);

        using var form = new frmMantTipoID(mediator);
        form.SetTestSearchText("RU");

        var success = await form.BuscarAsync();

        success.Should().BeTrue();
        form.ListCount.Should().Be(1);
        form.FirstListDescription.Should().Be("RUC");
    }

    [WinFormsFact]
    public async Task SaveAsync_NewTipoIdentificacion_UppercasesDescriptionAndReturnsToListing()
    {
        var mediator = Substitute.For<IMediator>();
        mediator.Send(Arg.Any<CreateTipoIdentificacionCommand>(), Arg.Any<CancellationToken>())
            .Returns(Result<int>.Ok(5));
        mediator.Send(Arg.Any<ListTiposIdentificacionQuery>(), Arg.Any<CancellationToken>())
            .Returns(Array.Empty<TipoIdentificacionDto>());

        using var form = new frmMantTipoID(mediator);
        form.BeginNewForTest();
        form.SetTestDescripcion("pasaporte");

        var success = await form.SaveAsync();

        success.Should().BeTrue();
        form.SelectedTabIndex.Should().Be(0);
        form.DetailsTabEnabled.Should().BeFalse();
        form.GrabarEnabled.Should().BeFalse();
        await mediator.Received(1).Send(
            Arg.Is<CreateTipoIdentificacionCommand>(command => command.Descripcion == "PASAPORTE"),
            Arg.Any<CancellationToken>());
    }

    [WinFormsFact]
    public async Task OpenAndEditTipoIdentificacion_UsesGetByIdThenUpdateCommand()
    {
        var mediator = Substitute.For<IMediator>();
        mediator.Send(Arg.Any<ListTiposIdentificacionQuery>(), Arg.Any<CancellationToken>())
            .Returns([new TipoIdentificacionDto(3, "PASAPORTE", 0)]);
        mediator.Send(Arg.Any<GetTipoIdentificacionByIdQuery>(), Arg.Any<CancellationToken>())
            .Returns(new TipoIdentificacionDto(3, "PASAPORTE", 0));
        mediator.Send(Arg.Any<UpdateTipoIdentificacionCommand>(), Arg.Any<CancellationToken>())
            .Returns(Result<bool>.Ok(true));

        using var form = new frmMantTipoID(mediator);
        await form.LoadTiposIdentificacionAsync();

        await form.OpenFirstForTestAsync();
        form.SelectedTabIndex.Should().Be(1);
        form.CodigoText.Should().Be("3");
        form.EditarEnabled.Should().BeTrue();

        form.BeginEditForTest();
        form.GrabarText.Should().Be("Actualizar");
        form.SetTestDescripcion("doc");

        var success = await form.SaveAsync();

        success.Should().BeTrue();
        await mediator.Received(1).Send(
            Arg.Is<UpdateTipoIdentificacionCommand>(command => command.Codigo == 3 && command.Descripcion == "DOC"),
            Arg.Any<CancellationToken>());
    }
}
