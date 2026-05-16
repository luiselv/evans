using Microsoft.Extensions.DependencyInjection;

namespace EVANS.Host.WinForms.Shell;

public partial class frmPrincipal : Form
{
    private readonly IServiceProvider _services;

    public frmPrincipal(IServiceProvider services)
    {
        _services = services;
        InitializeComponent();
    }
}
