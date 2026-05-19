using EVANS.Domain.GuiaRemision;
using EVANS.UI.WinForms.GuiaRemision;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace EVANS.Host.WinForms.Shell;

public partial class frmPrincipal : Form
{
    private readonly IServiceProvider _services;

    public frmPrincipal(IServiceProvider services)
    {
        _services = services;
        InitializeComponent();
        mnuGuias.Click += mnuGuias_Click;
    }

    private void mnuGuias_Click(object? sender, EventArgs e)
    {
        var mediator = _services.GetRequiredService<IMediator>();
        using var form = new frmGuiaRemision(mediator, new Standalone(), DateTime.Today.Year);
        form.ShowDialog(this);
    }
}
