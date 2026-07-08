using EVANS.Application.Catalogo.DTOs;
using EVANS.Application.Catalogo.Queries;
using EVANS.Application.Common;
using EVANS.Application.Identidad.Commands;
using EVANS.Application.Identidad.DTOs;
using EVANS.Application.Identidad.Ports;
using EVANS.Application.Identidad.Queries;
using EVANS.Application.Shared.DTOs;
using EVANS.UI.WinForms.Identidad;
using MediatR;

namespace EVANS.UI.Tests.Identidad;

public sealed class FrmMantUsuariosTests
{
    [WinFormsFact]
    public void DesignerConstructor_InitializesLayoutWithoutRuntimeServices()
    {
        using var form = new frmMantUsuarios();

        form.Text.Should().Be("Registro de Usuarios");
        form.ClientSize.Should().Be(new Size(635, 490));
        form.Controls.Find("TabControl1", searchAllChildren: true).Should().ContainSingle();
    }

    [WinFormsFact]
    public void Constructor_MatchesLegacyLayoutContract()
    {
        using var form = new frmMantUsuarios(Substitute.For<IMediator>(), CurrentSession("ADMIN", isAdmin: true));

        form.Text.Should().Be("Registro de Usuarios");
        form.ClientSize.Should().Be(new Size(635, 490));
        form.Font.Name.Should().Be("Microsoft Sans Serif");
        form.Font.Size.Should().BeApproximately(8.25f, 0.01f);

        var btnNuevo = form.Controls.Find("btnNuevo", searchAllChildren: true).Single().Should().BeOfType<Button>().Subject;
        btnNuevo.Location.Should().Be(new Point(561, 86));
        btnNuevo.Size.Should().Be(new Size(62, 48));
        btnNuevo.Image.Should().NotBeNull();
        btnNuevo.TextAlign.Should().Be(ContentAlignment.BottomCenter);
        form.Controls.Find("btnGrabar", true).Single().Location.Should().Be(new Point(561, 140));
        form.Controls.Find("btnEditar", true).Single().Location.Should().Be(new Point(561, 194));
        form.Controls.Find("btnCancelar", true).Single().Location.Should().Be(new Point(561, 248));
        form.Controls.Find("btnSalir", true).Single().Location.Should().Be(new Point(561, 302));

        foreach (var name in new[] { "btnGrabar", "btnEditar", "btnCancelar", "btnSalir", "btnBuscar" })
            form.Controls.Find(name, true).Single().Should().BeOfType<Button>().Subject.Image.Should().NotBeNull();

        var tabControl = form.Controls.Find("TabControl1", true).Single();
        tabControl.Location.Should().Be(new Point(12, 12));
        tabControl.Size.Should().Be(new Size(537, 458));
        form.Controls.Find("TabPage1", true).Single().Size.Should().Be(new Size(529, 431));
        form.Controls.Find("TabPage2", true).Single().Size.Should().Be(new Size(529, 431));

        form.Controls.Find("optTodos", true).Single().Location.Should().Be(new Point(23, 13));
        form.Controls.Find("optBuscar", true).Single().Location.Should().Be(new Point(105, 13));
        form.Controls.Find("txtBuscar", true).Single().Location.Should().Be(new Point(200, 16));
        form.Controls.Find("txtBuscar", true).Single().Size.Should().Be(new Size(231, 20));

        var list = form.Controls.Find("lvListado", true).Single().Should().BeOfType<ListView>().Subject;
        list.Location.Should().Be(new Point(20, 47));
        list.Size.Should().Be(new Size(489, 341));
        list.Columns.Cast<ColumnHeader>().Select(column => (column.Text, column.Width))
            .Should().Equal(("ID", 32), ("Nombre completo", 265), ("Usuario", 181));

        form.Controls.Find("txtCodigo", true).Single().Location.Should().Be(new Point(209, 40));
        form.Controls.Find("txtEmpleado", true).Single().Location.Should().Be(new Point(209, 77));
        form.Controls.Find("txtUsuario", true).Single().Location.Should().Be(new Point(209, 113));
        form.Controls.Find("txtClave", true).Single().Location.Should().Be(new Point(209, 149));
        form.Controls.Find("txtRepetir", true).Single().Location.Should().Be(new Point(209, 187));
        form.Controls.Find("cbEstado", true).Single().Location.Should().Be(new Point(209, 223));
        form.Controls.Find("chkAdmin", true).Single().Location.Should().Be(new Point(209, 262));
    }

    [WinFormsFact]
    public void Constructor_MatchesLegacyInitialState()
    {
        using var form = new frmMantUsuarios(Substitute.For<IMediator>(), CurrentSession("ADMIN", isAdmin: true));

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
    public async Task LoadUsuariosAsync_DisplaysLegacyListColumnsData()
    {
        var mediator = Substitute.For<IMediator>();
        mediator.Send(Arg.Any<BuscarUsuariosQuery>(), Arg.Any<CancellationToken>())
            .Returns([new UsuarioCuentaDto(1, "JDOE", "secret", "JOHN DOE", false, 1)]);

        using var form = new frmMantUsuarios(mediator, CurrentSession("ADMIN", isAdmin: true));

        await form.LoadUsuariosAsync();

        form.ListCount.Should().Be(1);
        form.FirstListUsuario.Should().Be("JDOE");
    }

    [WinFormsFact]
    public async Task BuscarAsync_EmptySearch_ShowsLegacyInsufficientDataMessage()
    {
        var mediator = Substitute.For<IMediator>();
        using var form = new frmMantUsuarios(mediator, CurrentSession("ADMIN", isAdmin: true));
        form.SetTestSearchText("");

        var success = await form.BuscarAsync();

        success.Should().BeFalse();
        form.StatusMessage.Should().Be("Ingrese nombre a buscar");
        await mediator.DidNotReceive().Send(Arg.Any<BuscarUsuariosQuery>(), Arg.Any<CancellationToken>());
    }

    [WinFormsFact]
    public async Task SaveAsync_NewUsuario_SendsCreateCommandAndReturnsToListing()
    {
        var mediator = Substitute.For<IMediator>();
        mediator.Send(Arg.Any<CrearUsuarioCommand>(), Arg.Any<CancellationToken>())
            .Returns(Result<int>.Ok(5));

        using var form = new frmMantUsuarios(mediator, CurrentSession("ADMIN", isAdmin: true));
        form.BeginNewForTest();
        form.SetTestEmpleado("JOHN DOE");
        form.SetTestUsuario("JDOE");
        form.SetTestClave("secret");
        form.SetTestRepetir("secret");
        form.SetTestEstado(1);
        form.SetTestAdmin(true);

        var success = await form.SaveAsync();

        success.Should().BeTrue();
        form.SelectedTabIndex.Should().Be(0);
        form.DetailsTabEnabled.Should().BeFalse();
        await mediator.Received(1).Send(
            Arg.Is<CrearUsuarioCommand>(command =>
                command.NombreCompleto == "JOHN DOE" &&
                command.NombreUsuario == "JDOE" &&
                command.Clave == "secret" &&
                command.EsAdministrador &&
                command.EstadoCodigo == 1),
            Arg.Any<CancellationToken>());
    }

    [WinFormsFact]
    public async Task SaveAsync_PasswordMismatch_ShowsLegacyMessageWithoutCommand()
    {
        var mediator = Substitute.For<IMediator>();
        using var form = new frmMantUsuarios(mediator, CurrentSession("ADMIN", isAdmin: true));
        form.BeginNewForTest();
        form.SetTestEmpleado("JOHN DOE");
        form.SetTestUsuario("JDOE");
        form.SetTestClave("secret");
        form.SetTestRepetir("other");
        form.SetTestEstado(1);

        var success = await form.SaveAsync();

        success.Should().BeFalse();
        form.StatusMessage.Should().Be("La nueva clave no concuerda con la confirmación.");
        await mediator.DidNotReceive().Send(Arg.Any<CrearUsuarioCommand>(), Arg.Any<CancellationToken>());
    }

    [WinFormsFact]
    public void CheckingAdminAsNonAdminUser_RevertsAndShowsLegacyMessage()
    {
        using var form = new frmMantUsuarios(Substitute.For<IMediator>(), CurrentSession("JDOE", isAdmin: false));

        form.BeginNewForTest();
        form.SetTestAdmin(true);

        form.AdminChecked.Should().BeFalse();
        form.StatusMessage.Should().Be("Solo un usuario administrador puede modificar esta opción.");
    }

    [WinFormsFact]
    public async Task SaveAsync_UpdateOtherUserAsNonAdmin_IsBlocked()
    {
        var mediator = Substitute.For<IMediator>();
        mediator.Send(Arg.Any<BuscarUsuariosQuery>(), Arg.Any<CancellationToken>())
            .Returns([new UsuarioCuentaDto(2, "OTHER", "secret", "OTHER USER", false, 1)]);
        mediator.Send(Arg.Any<ObtenerUsuarioPorCodigoQuery>(), Arg.Any<CancellationToken>())
            .Returns(new UsuarioCuentaDto(2, "OTHER", "secret", "OTHER USER", false, 1));
        mediator.Send(Arg.Any<ListEstadosQuery>(), Arg.Any<CancellationToken>())
            .Returns([new EstadoDto(1, "ACTIVO")]);

        using var form = new frmMantUsuarios(mediator, CurrentSession("JDOE", isAdmin: false));
        await form.LoadUsuariosAsync();
        await form.OpenFirstForTestAsync();
        form.BeginEditForTest();
        form.SetTestClave("secret2");
        form.SetTestRepetir("secret2");

        var success = await form.SaveAsync();

        success.Should().BeFalse();
        form.StatusMessage.Should().Be("Solo un usuario administrador o el propietario de la cuenta pueden modificar este registro.");
        await mediator.DidNotReceive().Send(Arg.Any<ActualizarUsuarioCommand>(), Arg.Any<CancellationToken>());
    }

    [WinFormsFact]
    public async Task SaveAsync_UpdateOwnUser_SendsUpdateCommand()
    {
        var mediator = Substitute.For<IMediator>();
        mediator.Send(Arg.Any<BuscarUsuariosQuery>(), Arg.Any<CancellationToken>())
            .Returns([new UsuarioCuentaDto(2, "JDOE", "secret", "JOHN DOE", false, 1)]);
        mediator.Send(Arg.Any<ObtenerUsuarioPorCodigoQuery>(), Arg.Any<CancellationToken>())
            .Returns(new UsuarioCuentaDto(2, "JDOE", "secret", "JOHN DOE", false, 1));
        mediator.Send(Arg.Any<ListEstadosQuery>(), Arg.Any<CancellationToken>())
            .Returns([new EstadoDto(1, "ACTIVO")]);
        mediator.Send(Arg.Any<ActualizarUsuarioCommand>(), Arg.Any<CancellationToken>())
            .Returns(Result<bool>.Ok(true));

        using var form = new frmMantUsuarios(mediator, CurrentSession("JDOE", isAdmin: false));
        await form.LoadUsuariosAsync();
        await form.OpenFirstForTestAsync();
        form.BeginEditForTest();
        form.SetTestClave("secret2");
        form.SetTestRepetir("secret2");

        var success = await form.SaveAsync();

        success.Should().BeTrue();
        await mediator.Received(1).Send(
            Arg.Is<ActualizarUsuarioCommand>(command => command.Codigo == 2 && command.Clave == "secret2"),
            Arg.Any<CancellationToken>());
    }

    private static ICurrentSession CurrentSession(string username, bool isAdmin)
    {
        var session = Substitute.For<ICurrentSession>();
        session.Current.Returns(new SesionActualDto(
            new UsuarioSesionDto(username, username, isAdmin),
            new ParametrosDto(0.18m, "F001", "1", "2", "B001", "1", "2", "T001", "1", "2", "M", "R", "e", "p", "smtp", 25),
            2026));
        session.IsAuthenticated.Returns(true);
        return session;
    }
}
