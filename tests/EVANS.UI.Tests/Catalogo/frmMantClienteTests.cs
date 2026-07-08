using EVANS.Application.Catalogo.Commands;
using EVANS.Application.Catalogo.DTOs;
using EVANS.Application.Catalogo.Queries;
using EVANS.Application.Common;
using EVANS.UI.WinForms.Catalogo;
using MediatR;

namespace EVANS.UI.Tests.Catalogo;

public sealed class FrmMantClienteTests
{
    [WinFormsFact]
    public void DesignerConstructor_InitializesLayoutWithoutRuntimeServices()
    {
        using var form = new frmMantCliente();

        form.Text.Should().Be("Registro de Clientes");
        form.ClientSize.Should().Be(new Size(635, 490));
        form.Controls.Find("TabControl1", true).Should().ContainSingle();
    }

    [WinFormsFact]
    public void Constructor_MatchesLegacyLayoutContract()
    {
        using var form = new frmMantCliente(Substitute.For<IMediator>());

        form.Text.Should().Be("Registro de Clientes");
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
        form.Controls.Find("btnSalir", true).Single().Location.Should().Be(new Point(561, 302));
        foreach (var name in new[] { "btnGrabar", "btnEditar", "btnCancelar", "btnSalir", "btnBuscar" })
            form.Controls.Find(name, true).Single().Should().BeOfType<Button>().Subject.Image.Should().NotBeNull();

        var tabControl = form.Controls.Find("TabControl1", true).Single();
        tabControl.Location.Should().Be(new Point(12, 12));
        tabControl.Size.Should().Be(new Size(537, 458));

        var list = form.Controls.Find("dgvListado", true).Single().Should().BeOfType<DataGridView>().Subject;
        list.Location.Should().Be(new Point(23, 51));
        list.Size.Should().Be(new Size(486, 346));
        list.Columns.Cast<DataGridViewColumn>().Select(column => (column.HeaderText, column.Width)).Should()
            .Equal(("ID", 40), ("Nombre", 220), ("Tipo Doc", 80), ("Número", 80));

        form.Controls.Find("optTodos", true).Single().Location.Should().Be(new Point(23, 13));
        form.Controls.Find("optBuscar", true).Single().Location.Should().Be(new Point(84, 13));
        form.Controls.Find("optNro", true).Single().Location.Should().Be(new Point(170, 13));
        form.Controls.Find("txtBuscar", true).Single().Location.Should().Be(new Point(256, 16));

        form.Controls.Find("txtCodigo", true).Single().Location.Should().Be(new Point(130, 22));
        form.Controls.Find("txtNombre", true).Single().Location.Should().Be(new Point(130, 48));
        form.Controls.Find("cbTipoID", true).Single().Location.Should().Be(new Point(130, 74));
        form.Controls.Find("txtNroID", true).Single().Location.Should().Be(new Point(352, 74));
        form.Controls.Find("txtTelefono", true).Single().Location.Should().Be(new Point(130, 101));
        form.Controls.Find("txtFax", true).Single().Location.Should().Be(new Point(352, 101));
        form.Controls.Find("txtEmail", true).Single().Location.Should().Be(new Point(130, 127));
        form.Controls.Find("txtRepresentante", true).Single().Location.Should().Be(new Point(130, 153));
        var direcciones = form.Controls.Find("dgvDireccion", true).Single().Should().BeOfType<DataGridView>().Subject;
        direcciones.Location.Should().Be(new Point(128, 188));
        direcciones.Size.Should().Be(new Size(366, 150));
        direcciones.Columns.Cast<DataGridViewColumn>().Select(column => (column.HeaderText, column.Width)).Should()
            .Equal(("Dirección", 250), ("Ciudad", 100), ("Provincia", 100));

        foreach (var name in new[] { "Label11", "Label8" })
        {
            var required = form.Controls.Find(name, true).Single().Should().BeOfType<Label>().Subject;
            required.Text.Should().Be("*");
            required.Font.Bold.Should().BeTrue();
        }
    }

    [WinFormsFact]
    public void Constructor_MatchesLegacyInitialState()
    {
        using var form = new frmMantCliente(Substitute.For<IMediator>());

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
    public async Task LoadClientesAsync_DisplaysLegacyRows()
    {
        var mediator = CreateMediatorWithCatalogs();
        using var form = new frmMantCliente(mediator);

        await form.LoadClientesAsync();

        form.ListCount.Should().Be(2);
        form.FirstListNombre.Should().Be("EVANS SAC");
        form.FirstListNumero.Should().Be("20123456789");
    }

    [WinFormsFact]
    public async Task BuscarAsync_FiltersByNamePrefix()
    {
        var mediator = CreateMediatorWithCatalogs();
        using var form = new frmMantCliente(mediator);
        form.SetTestSearchText("NOR");

        var success = await form.BuscarAsync();

        success.Should().BeTrue();
        form.ListCount.Should().Be(1);
        form.FirstListNombre.Should().Be("NORTE SAC");
    }

    [WinFormsFact]
    public async Task BuscarAsync_FiltersByDocumentNumber()
    {
        var mediator = CreateMediatorWithCatalogs();
        using var form = new frmMantCliente(mediator);
        form.SelectNumberSearchForTest();
        form.SetTestSearchText("20123456789");

        var success = await form.BuscarAsync();

        success.Should().BeTrue();
        form.ListCount.Should().Be(1);
        form.FirstListNombre.Should().Be("EVANS SAC");
    }

    [WinFormsFact]
    public async Task BuscarAsync_EmptySearch_PreservesLegacyMessage()
    {
        var mediator = Substitute.For<IMediator>();
        using var form = new frmMantCliente(mediator);
        form.SetTestSearchText("");

        var success = await form.BuscarAsync();

        success.Should().BeFalse();
        form.StatusMessage.Should().Be("Ingrese nombre a buscar");
        await mediator.DidNotReceive().Send(Arg.Any<ListClientesQuery>(), Arg.Any<CancellationToken>());
    }

    [WinFormsFact]
    public async Task SaveAsync_NewCliente_SendsUppercaseValuesAndLegacyOptionalFields()
    {
        var mediator = CreateMediatorWithCatalogs();
        mediator.Send(Arg.Any<CreateClienteCommand>(), Arg.Any<CancellationToken>()).Returns(Result<int>.Ok(3));
        using var form = new frmMantCliente(mediator);
        await form.LoadClientesAsync();
        form.BeginNewForTest();
        form.SetTestNombre("cliente nuevo");
        form.SetTestTipoId(2);
        form.SetTestNroId("20999999999");
        form.SetTestTelefono("555");
        form.SetTestFax("999");
        form.SetTestEmail("cliente@example.com");
        form.SetTestRepresentante("apoderado");
        form.AddTestDireccion("Av Lima", "Lima", "Lima");

        var success = await form.SaveAsync();

        success.Should().BeTrue();
        await mediator.Received(1).Send(
            Arg.Is<CreateClienteCommand>(command => command.RazonSocial == "CLIENTE NUEVO"
                && command.TipoIdCodigo == 2
                && command.NroIdentificacion == "20999999999"
                && command.LongitudRequerida == 11
                && command.Telefono == "555"
                && command.Fax == "999"
                && command.Email == "cliente@example.com"
                && command.Representante == "apoderado"
                && command.Direcciones.Single().Calle == "Av Lima"),
            Arg.Any<CancellationToken>());
    }

    [WinFormsFact]
    public async Task OpenAndEditCliente_UsesGetByIdThenUpdateCommand()
    {
        var mediator = CreateMediatorWithCatalogs();
        mediator.Send(Arg.Any<GetClienteByIdQuery>(), Arg.Any<CancellationToken>())
            .Returns(new ClienteDto(
                1,
                "EVANS SAC",
                2,
                "20123456789",
                "123",
                "456",
                "ventas@example.com",
                "REPRESENTANTE",
                [new DireccionDto("Av Lima", "Lima", "Lima")]));
        mediator.Send(Arg.Any<UpdateClienteCommand>(), Arg.Any<CancellationToken>()).Returns(Result<bool>.Ok(true));
        using var form = new frmMantCliente(mediator);
        await form.LoadClientesAsync();

        await form.OpenFirstForTestAsync();
        form.SelectedTabIndex.Should().Be(1);
        form.CodigoText.Should().Be("1");
        form.EditarEnabled.Should().BeTrue();
        form.BeginEditForTest();
        form.SetTestNombre("cliente actualizado");
        form.SetTestTipoId(1);
        form.SetTestNroId("12345678");
        form.SetTestFax("777");
        form.SetTestRepresentante("nuevo representante");

        var success = await form.SaveAsync();

        success.Should().BeTrue();
        await mediator.Received(1).Send(
            Arg.Is<UpdateClienteCommand>(command => command.Codigo == 1
                && command.RazonSocial == "CLIENTE ACTUALIZADO"
                && command.TipoIdCodigo == 1
                && command.NroIdentificacion == "12345678"
                && command.LongitudRequerida == 8
                && command.Fax == "777"
                && command.Representante == "nuevo representante"),
            Arg.Any<CancellationToken>());
    }

    private static IMediator CreateMediatorWithCatalogs()
    {
        var mediator = Substitute.For<IMediator>();
        mediator.Send(Arg.Any<ListTiposIdentificacionQuery>(), Arg.Any<CancellationToken>()).Returns([
            new TipoIdentificacionDto(1, "DNI", 8),
            new TipoIdentificacionDto(2, "RUC", 11),
            new TipoIdentificacionDto(3, "SIN DOCUMENTO", 8)]);
        mediator.Send(Arg.Any<ListClientesQuery>(), Arg.Any<CancellationToken>()).Returns([
            new ClienteDto(1, "EVANS SAC", 2, "20123456789", "123", "456", "ventas@example.com", "REP", []),
            new ClienteDto(2, "NORTE SAC", 1, "12345678", null, null, null, null, [])]);
        return mediator;
    }
}
