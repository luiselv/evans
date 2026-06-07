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
