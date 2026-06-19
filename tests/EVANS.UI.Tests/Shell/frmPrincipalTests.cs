using EVANS.Host.WinForms.Shell;
using EVANS.UI.WinForms.Catalogo;
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
