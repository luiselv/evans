using EVANS.Application.Recepcion.Ports;
using EVANS.Domain.GuiaRemision;
using EVANS.Reports.Comprobante;
using EVANS.UI.WinForms.Comprobante;
using EVANS.UI.WinForms.GuiaRemision;
using EVANS.UI.WinForms.Recepcion;
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
        mnuGuias.Click        += mnuGuias_Click;
        mnuComprobantes.Click += mnuComprobantes_Click;
        mnuRecepciones.Click  += mnuRecepciones_Click;
    }

    private void mnuGuias_Click(object? sender, EventArgs e)
    {
        var mediator = _services.GetRequiredService<IMediator>();
        using var form = new frmGuiaRemision(mediator, new Standalone(), DateTime.Today.Year);
        form.ShowDialog(this);
    }

    private void mnuComprobantes_Click(object? sender, EventArgs e)
    {
        if (FeatureFlags.ComprobanteV2Enabled)
        {
            var mediator = _services.GetRequiredService<IMediator>();
            var factory  = _services.GetRequiredService<DocumentPrinterFactory>();

            using var form = new frmComprobante(
                mediator,
                guiaRef: null,
                printerResolver: factory.For);

            form.MdiParent = this;
            form.Show();
        }
        // else: legacy path — frmComprobante VB not yet wired; menu is a no-op for now
    }

    private void mnuRecepciones_Click(object? sender, EventArgs e)
    {
        if (FeatureFlags.RecepcionV2Enabled)
        {
            var mediator  = _services.GetRequiredService<IMediator>();
            var catalogos = _services.GetRequiredService<ICatalogosRecepcionRepository>();

            using var form = new frmRecepcion(mediator, catalogos, DateTime.Today.Year);
            form.MdiParent = this;
            form.Show();
        }
        // else: legacy path — legacy frmRecepcion.vb handles this
    }
}
