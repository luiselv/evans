using EVANS.Host.WinForms.Shell;
using EVANS.Application.Identidad.DTOs;
using EVANS.Application.Identidad.Ports;
using EVANS.Application.Manifiesto.DTOs;
using EVANS.Application.Manifiesto.Ports;
using EVANS.Application.Manifiesto.Queries;
using EVANS.Application.Recepcion.DTOs;
using EVANS.Application.Recepcion.Ports;
using EVANS.Application.Shared.Ports;
using EVANS.Application.Shared.DTOs;
using EVANS.Reports.Comprobante;
using EVANS.UI.WinForms.Catalogo;
using EVANS.UI.WinForms.Comprobante;
using EVANS.UI.WinForms.Identidad;
using EVANS.UI.WinForms.Manifiesto;
using EVANS.UI.WinForms.Recepcion;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace EVANS.UI.Tests.Shell;

public sealed class FrmPrincipalTests
{
    [WinFormsFact]
    public void CatalogosEstadosMenu_OpensEstadoFormAsMdiChild()
    {
        var services = new ServiceCollection()
            .AddSingleton(Substitute.For<IMediator>())
            .BuildServiceProvider();

        using var form = new frmPrincipal(services);
        form.Show();

        var estadosMenu = FindMenuItem(form, "Estados");
        estadosMenu.Should().NotBeNull();

        estadosMenu!.PerformClick();

        form.MdiChildren.Should().ContainSingle()
            .Which.Should().BeOfType<frmMantEstado>();
    }

    [WinFormsFact]
    public void CatalogosDestinosMenu_OpensDestinoFormAsMdiChild()
    {
        var services = new ServiceCollection()
            .AddSingleton(Substitute.For<IMediator>())
            .BuildServiceProvider();

        using var form = new frmPrincipal(services);
        form.Show();

        var destinosMenu = FindMenuItem(form, "Destinos");
        destinosMenu.Should().NotBeNull();

        destinosMenu!.PerformClick();

        form.MdiChildren.Should().ContainSingle()
            .Which.Should().BeOfType<frmMantDestino>();
    }

    [WinFormsFact]
    public void CatalogosEmpresasMenu_OpensEmpresaFormAsMdiChild()
    {
        var services = new ServiceCollection()
            .AddSingleton(Substitute.For<IMediator>())
            .BuildServiceProvider();

        using var form = new frmPrincipal(services);
        form.Show();

        var empresasMenu = FindMenuItem(form, "Empresas");
        empresasMenu.Should().NotBeNull();

        empresasMenu!.PerformClick();

        form.MdiChildren.Should().ContainSingle()
            .Which.Should().BeOfType<frmMantEmpresa>();
    }

    [WinFormsFact]
    public void CatalogosClientesMenu_OpensClienteFormAsMdiChild()
    {
        var services = new ServiceCollection()
            .AddSingleton(Substitute.For<IMediator>())
            .BuildServiceProvider();

        using var form = new frmPrincipal(services);
        form.Show();

        var clientesMenu = FindMenuItem(form, "Clientes");
        clientesMenu.Should().NotBeNull();

        clientesMenu!.PerformClick();

        form.MdiChildren.Should().ContainSingle()
            .Which.Should().BeOfType<frmMantCliente>();
    }

    [WinFormsFact]
    public void CatalogosChoferesMenu_OpensChoferFormAsMdiChild()
    {
        var services = new ServiceCollection()
            .AddSingleton(Substitute.For<IMediator>())
            .BuildServiceProvider();

        using var form = new frmPrincipal(services);
        form.Show();

        var choferesMenu = FindMenuItem(form, "Choferes");
        choferesMenu.Should().NotBeNull();

        choferesMenu!.PerformClick();

        form.MdiChildren.Should().ContainSingle()
            .Which.Should().BeOfType<frmMantChofer>();
    }

    [WinFormsFact]
    public void CatalogosTiposIdentificacionMenu_OpensTipoIDFormAsMdiChild()
    {
        var services = new ServiceCollection()
            .AddSingleton(Substitute.For<IMediator>())
            .BuildServiceProvider();

        using var form = new frmPrincipal(services);
        form.Show();

        var tiposMenu = FindMenuItem(form, "Tipos de Identificación");
        tiposMenu.Should().NotBeNull();

        tiposMenu!.PerformClick();

        form.MdiChildren.Should().ContainSingle()
            .Which.Should().BeOfType<frmMantTipoID>();
    }

    [WinFormsFact]
    public void CatalogosCarretasMenu_OpensCarretaFormAsMdiChild()
    {
        var services = new ServiceCollection()
            .AddSingleton(Substitute.For<IMediator>())
            .BuildServiceProvider();

        using var form = new frmPrincipal(services);
        form.Show();

        var carretasMenu = FindMenuItem(form, "Carretas");
        carretasMenu.Should().NotBeNull();

        carretasMenu!.PerformClick();

        form.MdiChildren.Should().ContainSingle()
            .Which.Should().BeOfType<frmMantCarreta>();
    }

    [WinFormsFact]
    public void CatalogosVehiculosMenu_OpensVehiculoFormAsMdiChild()
    {
        var services = new ServiceCollection()
            .AddSingleton(Substitute.For<IMediator>())
            .BuildServiceProvider();

        using var form = new frmPrincipal(services);
        form.Show();

        var vehiculosMenu = FindMenuItem(form, "Vehículos");
        vehiculosMenu.Should().NotBeNull();

        vehiculosMenu!.PerformClick();

        form.MdiChildren.Should().ContainSingle()
            .Which.Should().BeOfType<frmMantVehiculo>();
    }

    [WinFormsFact]
    public void ConfiguracionParametrosMenu_OpensParametrosFormAsMdiChild()
    {
        var services = new ServiceCollection()
            .AddSingleton(Substitute.For<IMediator>())
            .AddSingleton(Substitute.For<IParametrosService>())
            .BuildServiceProvider();

        using var form = new frmPrincipal(services);
        form.Show();

        var parametrosMenu = FindMenuItem(form, "Parámetros del sistema");
        parametrosMenu.Should().NotBeNull();

        parametrosMenu!.PerformClick();

        form.MdiChildren.Should().ContainSingle()
            .Which.Should().BeOfType<frmParametros>();
    }

    [WinFormsFact]
    public void ManifiestosMenu_OpensManifiestoFormAsMdiChild()
    {
        var mediator = Substitute.For<IMediator>();
        mediator.Send(Arg.Any<BuscarManifiestosQuery>(), Arg.Any<CancellationToken>())
            .Returns(Task.FromResult<IReadOnlyList<ManifiestoResumenDto>>([]));

        var catalogos = Substitute.For<ICatalogosManifiestoRepository>();
        catalogos.ObtenerCatalogosAsync(Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(new CatalogosManifiestoDto([], [], [], [], [])));

        var currentSession = Substitute.For<ICurrentSession>();
        currentSession.Current.Returns(new SesionActualDto(
            new UsuarioSesionDto("admin", "Administrador", true),
            new ParametrosDto(0.18m, "F", "1", "2", "B", "1", "2", "G", "1", "2", "M", "", "", "", "", 0),
            2026));

        var services = new ServiceCollection()
            .AddSingleton(mediator)
            .AddSingleton(catalogos)
            .AddSingleton(currentSession)
            .BuildServiceProvider();

        using var form = new frmPrincipal(services);
        form.Show();

        var manifiestosMenu = FindMenuItem(form, "Manifiestos");
        manifiestosMenu.Should().NotBeNull();

        manifiestosMenu!.PerformClick();

        form.MdiChildren.Should().ContainSingle()
            .Which.Should().BeOfType<frmManifiesto>();
    }

    [WinFormsFact]
    public void ComprobantesMenu_OpensComprobanteFormAsMdiChild()
    {
        var services = new ServiceCollection()
            .AddSingleton(Substitute.For<IMediator>())
            .AddSingleton(new DocumentPrinterFactory(new BoletaPdfRenderer(), new FacturaPdfRenderer()))
            .BuildServiceProvider();

        using var form = new frmPrincipal(services);
        form.Show();

        var comprobantesMenu = FindMenuItem(form, "Comprobantes");
        comprobantesMenu.Should().NotBeNull();

        comprobantesMenu!.PerformClick();

        form.MdiChildren.Should().ContainSingle()
            .Which.Should().BeOfType<frmComprobante>();
    }

    [WinFormsFact]
    public void RecepcionesMenu_OpensRecepcionFormAsMdiChild()
    {
        var catalogos = Substitute.For<ICatalogosRecepcionRepository>();
        catalogos.ListarClientesAsync(Arg.Any<CancellationToken>())
            .Returns(Task.FromResult<IReadOnlyList<ClienteLookupDto>>([]));
        catalogos.ListarDestinosAsync(Arg.Any<CancellationToken>())
            .Returns(Task.FromResult<IReadOnlyList<DestinoLookupDto>>([]));
        catalogos.ListarEstadosAsync(Arg.Any<CancellationToken>())
            .Returns(Task.FromResult<IReadOnlyList<EstadoLookupDto>>([]));

        var currentSession = Substitute.For<ICurrentSession>();
        currentSession.Current.Returns(new SesionActualDto(
            new UsuarioSesionDto("admin", "Administrador", true),
            new ParametrosDto(0.18m, "F", "1", "2", "B", "1", "2", "G", "1", "2", "M", "", "", "", "", 0),
            2026));

        var services = new ServiceCollection()
            .AddSingleton(Substitute.For<IMediator>())
            .AddSingleton(catalogos)
            .AddSingleton(currentSession)
            .BuildServiceProvider();

        using var form = new frmPrincipal(services);
        form.Show();

        var recepcionesMenu = FindMenuItem(form, "Recepciones");
        recepcionesMenu.Should().NotBeNull();

        recepcionesMenu!.PerformClick();

        form.MdiChildren.Should().ContainSingle()
            .Which.Should().BeOfType<frmRecepcion>();
    }

    private static ToolStripMenuItem? FindMenuItem(Form form, string text)
    {
        var menuStrip = form.Controls.OfType<MenuStrip>().Single();
        return menuStrip.Items
            .OfType<ToolStripMenuItem>()
            .SelectMany(Flatten)
            .SingleOrDefault(item => item.Text == text);
    }

    private static IEnumerable<ToolStripMenuItem> Flatten(ToolStripMenuItem item)
    {
        yield return item;

        foreach (var child in item.DropDownItems.OfType<ToolStripMenuItem>().SelectMany(Flatten))
            yield return child;
    }
}
