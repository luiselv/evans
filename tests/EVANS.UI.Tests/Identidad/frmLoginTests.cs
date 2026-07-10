using EVANS.Application.Common;
using EVANS.Application.Identidad.Commands;
using EVANS.Application.Identidad.DTOs;
using EVANS.Application.Identidad.Ports;
using EVANS.UI.WinForms.Identidad;
using MediatR;

namespace EVANS.UI.Tests.Identidad;

public sealed class FrmLoginTests
{
    [WinFormsFact]
    public void DesignerConstructor_InitializesLayoutWithoutRuntimeServices()
    {
        using var form = new frmLogin();

        form.Text.Should().Be("EVANS Cargo S.A.C.");
        form.Controls.Find("GroupBox1", searchAllChildren: true).Should().ContainSingle();
    }

    [WinFormsFact]
    public void Constructor_MatchesLegacyLayoutContract()
    {
        using var form = CreateForm();

        form.Text.Should().Be("EVANS Cargo S.A.C.");
        form.ClientSize.Should().Be(new Size(344, 201));
        form.Font.Name.Should().Be("Microsoft Sans Serif");
        form.Font.Size.Should().BeApproximately(8.25f, 0.01f);
        form.StartPosition.Should().Be(FormStartPosition.CenterScreen);
        form.MaximizeBox.Should().BeFalse();
        form.MinimizeBox.Should().BeFalse();
        form.Icon.Should().NotBeNull();

        var group = form.Controls.Find("GroupBox1", searchAllChildren: true).Single().Should().BeOfType<GroupBox>().Subject;
        group.Text.Should().Be("Acceso al sistema");
        group.Location.Should().Be(new Point(12, 12));
        group.Size.Should().Be(new Size(320, 145));

        form.Controls.Find("txtServidor", true).Single().Location.Should().Be(new Point(105, 25));
        form.Controls.Find("cbBD", true).Single().Location.Should().Be(new Point(105, 53));
        form.Controls.Find("txtUsuario", true).Single().Location.Should().Be(new Point(105, 82));
        form.Controls.Find("txtClave", true).Single().Location.Should().Be(new Point(105, 110));
        form.Controls.Find("btnConectar", true).Single().Location.Should().Be(new Point(237, 23));
        form.Controls.Find("btnCrear", true).Single().Location.Should().Be(new Point(237, 53));
        form.Controls.Find("cbAceptar", true).Single().Location.Should().Be(new Point(77, 168));
        form.Controls.Find("cbSalir", true).Single().Location.Should().Be(new Point(192, 168));

        form.Controls.Find("Label3", true).Single().Text.Should().Be("Servidor :");
        form.Controls.Find("Label4", true).Single().Text.Should().Be("Base de Datos :");
        form.Controls.Find("Label1", true).Single().Text.Should().Be("Usuario :");
        form.Controls.Find("Label2", true).Single().Text.Should().Be("Contraseña :");
    }

    [WinFormsFact]
    public void Constructor_MatchesLegacyInitialState()
    {
        using var form = CreateForm();

        form.Controls.Find("cbBD", true).Single().Enabled.Should().BeFalse();
        form.Controls.Find("btnCrear", true).Single().Enabled.Should().BeFalse();
        form.Controls.Find("txtUsuario", true).Single().Enabled.Should().BeFalse();
        form.Controls.Find("txtClave", true).Single().Enabled.Should().BeFalse();
        form.Controls.Find("cbAceptar", true).Single().Enabled.Should().BeFalse();
        form.Controls.Find("btnConectar", true).Single().Enabled.Should().BeTrue();
        form.Controls.Find("cbSalir", true).Single().Enabled.Should().BeTrue();
    }

    [WinFormsFact]
    public async Task ConnectForTestsAsync_LoadsAnnualDatabasesAndSelectsCurrentYear()
    {
        var catalog = Substitute.For<IYearlyDatabaseCatalog>();
        catalog.ListYearsAsync(Arg.Any<CancellationToken>())
            .Returns([2010, DateTime.Today.Year]);
        using var form = CreateForm(databaseCatalog: catalog);

        var success = await form.ConnectForTestsAsync();

        success.Should().BeTrue();
        form.Controls.Find("cbBD", true).Single().Enabled.Should().BeTrue();
        form.Controls.Find("btnCrear", true).Single().Enabled.Should().BeTrue();
        form.Controls.Find("txtUsuario", true).Single().Enabled.Should().BeTrue();
        form.Controls.Find("txtClave", true).Single().Enabled.Should().BeTrue();
        form.Controls.Find("cbAceptar", true).Single().Enabled.Should().BeTrue();
        form.AvailableYears.Should().Equal(2010, DateTime.Today.Year);
        form.SelectedYear.Should().Be(DateTime.Today.Year);
        await catalog.Received(1).ListYearsAsync(Arg.Any<CancellationToken>());
    }

    [WinFormsFact]
    public async Task ConnectForTestsAsync_CurrentYearMissing_ShowsLegacyWarningAndKeepsManualSelectionAvailable()
    {
        var catalog = Substitute.For<IYearlyDatabaseCatalog>();
        catalog.ListYearsAsync(Arg.Any<CancellationToken>()).Returns([2010, 2014]);
        using var form = CreateForm(databaseCatalog: catalog);

        var success = await form.ConnectForTestsAsync();

        success.Should().BeTrue();
        form.AvailableYears.Should().Equal(2010, 2014);
        form.Controls.Find("cbBD", true).Single().Enabled.Should().BeTrue();
        form.ErrorMessage.Should().Be("No se encontró Base de Datos correspondiente al año actual. Se recomienda crear una nueva.");
    }

    [WinFormsFact]
    public async Task ConnectForTestsAsync_CatalogFailure_KeepsLoginDisabledAndShowsError()
    {
        var catalog = Substitute.For<IYearlyDatabaseCatalog>();
        catalog.ListYearsAsync(Arg.Any<CancellationToken>()).Returns<Task<IReadOnlyList<int>>>(_ => throw new InvalidOperationException("SQL unavailable"));
        using var form = CreateForm(databaseCatalog: catalog);

        var success = await form.ConnectForTestsAsync();

        success.Should().BeFalse();
        form.Controls.Find("txtUsuario", true).Single().Enabled.Should().BeFalse();
        form.Controls.Find("txtClave", true).Single().Enabled.Should().BeFalse();
        form.Controls.Find("cbAceptar", true).Single().Enabled.Should().BeFalse();
        form.ErrorMessage.Should().Be("SQL unavailable");
    }

    [WinFormsFact]
    public async Task CreateCurrentYearForTestsAsync_WhenYearIsMissing_CreatesDatabaseRefreshesListAndShowsSuccess()
    {
        var catalog = Substitute.For<IYearlyDatabaseCatalog>();
        catalog.ListYearsAsync(Arg.Any<CancellationToken>()).Returns(
            Task.FromResult<IReadOnlyList<int>>([2010, 2014]),
            Task.FromResult<IReadOnlyList<int>>([2010, 2014, DateTime.Today.Year]));
        var provisioner = Substitute.For<IYearlyDatabaseProvisioner>();
        using var form = CreateForm(databaseCatalog: catalog, databaseProvisioner: provisioner);
        await form.ConnectForTestsAsync();

        var success = await form.CreateCurrentYearForTestsAsync();

        success.Should().BeTrue();
        await provisioner.Received(1).CreateYearAsync(DateTime.Today.Year, Arg.Any<CancellationToken>());
        form.AvailableYears.Should().Equal(2010, 2014, DateTime.Today.Year);
        form.SelectedYear.Should().Be(DateTime.Today.Year);
        form.ErrorMessage.Should().Be("La nueva Base de Datos fue creada con éxito.");
    }

    [WinFormsFact]
    public async Task CreateCurrentYearForTestsAsync_WhenYearAlreadyExists_DoesNotCreateDatabase()
    {
        var provisioner = Substitute.For<IYearlyDatabaseProvisioner>();
        using var form = CreateForm(databaseProvisioner: provisioner);
        await form.ConnectForTestsAsync();

        var success = await form.CreateCurrentYearForTestsAsync();

        success.Should().BeFalse();
        await provisioner.DidNotReceive().CreateYearAsync(Arg.Any<int>(), Arg.Any<CancellationToken>());
        form.ErrorMessage.Should().Be("Ya existe una Base de Datos para el año actual");
    }

    [WinFormsFact]
    public async Task CreateCurrentYearForTestsAsync_ProvisionerFailure_ShowsError()
    {
        var catalog = Substitute.For<IYearlyDatabaseCatalog>();
        catalog.ListYearsAsync(Arg.Any<CancellationToken>()).Returns([2010, 2014]);
        var provisioner = Substitute.For<IYearlyDatabaseProvisioner>();
        provisioner.CreateYearAsync(Arg.Any<int>(), Arg.Any<CancellationToken>())
            .Returns<Task>(_ => throw new InvalidOperationException("create failed"));
        using var form = CreateForm(databaseCatalog: catalog, databaseProvisioner: provisioner);
        await form.ConnectForTestsAsync();

        var success = await form.CreateCurrentYearForTestsAsync();

        success.Should().BeFalse();
        form.ErrorMessage.Should().Be("create failed");
    }

    [WinFormsFact]
    public async Task SubmitAsync_ValidCredentials_StoresAuthenticatedUser()
    {
        var mediator = Substitute.For<IMediator>();
        mediator.Send(Arg.Any<AutenticarUsuarioCommand>(), Arg.Any<CancellationToken>())
            .Returns(Result<UsuarioSesionDto>.Ok(new UsuarioSesionDto("testuser", "Test User", true)));

        using var form = CreateForm(mediator);
        await form.ConnectForTestsAsync();
        form.SetTestCredentials("testuser", "testpass");

        var success = await form.SubmitAsync();

        success.Should().BeTrue();
        form.AuthenticatedUser.Should().NotBeNull();
        form.AuthenticatedUser!.NombreUsuario.Should().Be("testuser");
        form.SelectedYear.Should().Be(DateTime.Today.Year);
        await mediator.Received(1).Send(
            Arg.Is<AutenticarUsuarioCommand>(cmd =>
                cmd.NombreUsuario == "testuser" &&
                cmd.Clave == "testpass"),
            Arg.Any<CancellationToken>());
    }

    [WinFormsFact]
    public async Task SubmitAsync_MissingDatabase_ShowsRequiredDatabaseMessageAndDoesNotAuthenticate()
    {
        var mediator = Substitute.For<IMediator>();
        using var form = CreateForm(mediator);
        form.SetTestCredentials("testuser", "testpass");

        var success = await form.SubmitAsync();

        success.Should().BeFalse();
        form.AuthenticatedUser.Should().BeNull();
        form.ErrorMessage.Should().Be("Seleccione una Base de Datos.");
        await mediator.DidNotReceive().Send(Arg.Any<AutenticarUsuarioCommand>(), Arg.Any<CancellationToken>());
    }

    [WinFormsFact]
    public async Task SubmitAsync_MissingCredentials_ShowsLegacyRequiredMessageAndDoesNotAuthenticate()
    {
        var mediator = Substitute.For<IMediator>();
        using var form = CreateForm(mediator);
        await form.ConnectForTestsAsync();

        var success = await form.SubmitAsync();

        success.Should().BeFalse();
        form.AuthenticatedUser.Should().BeNull();
        form.ErrorMessage.Should().Be("Ingrese su nombre de ususario y contraseña.");
        await mediator.DidNotReceive().Send(Arg.Any<AutenticarUsuarioCommand>(), Arg.Any<CancellationToken>());
    }

    [WinFormsFact]
    public async Task SubmitAsync_InvalidCredentials_ShowsLegacyErrorAndKeepsUserNull()
    {
        var mediator = Substitute.For<IMediator>();
        mediator.Send(Arg.Any<AutenticarUsuarioCommand>(), Arg.Any<CancellationToken>())
            .Returns(Result<UsuarioSesionDto>.Fail("Invalid username or password."));

        using var form = CreateForm(mediator);
        await form.ConnectForTestsAsync();
        form.SetTestCredentials("testuser", "wrongpass");

        var success = await form.SubmitAsync();

        success.Should().BeFalse();
        form.AuthenticatedUser.Should().BeNull();
        form.ErrorMessage.Should().Be("Error en el inicio de sesión.");
    }

    private static frmLogin CreateForm(
        IMediator? mediator = null,
        IYearlyDatabaseCatalog? databaseCatalog = null,
        IYearlyDatabaseProvisioner? databaseProvisioner = null)
    {
        mediator ??= Substitute.For<IMediator>();
        databaseCatalog ??= CurrentYearCatalog();
        databaseProvisioner ??= Substitute.For<IYearlyDatabaseProvisioner>();
        return new frmLogin(mediator, databaseCatalog, databaseProvisioner);
    }

    private static IYearlyDatabaseCatalog CurrentYearCatalog()
    {
        var catalog = Substitute.For<IYearlyDatabaseCatalog>();
        catalog.ListYearsAsync(Arg.Any<CancellationToken>()).Returns([DateTime.Today.Year]);
        return catalog;
    }
}
