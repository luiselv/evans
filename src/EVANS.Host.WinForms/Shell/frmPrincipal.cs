using EVANS.Application.Identidad.Ports;
using EVANS.Application.Recepcion.Ports;
using EVANS.Domain.GuiaRemision;
using EVANS.Reports.Comprobante;
using EVANS.UI.WinForms.Catalogo;
using EVANS.UI.WinForms.Comprobante;
using EVANS.UI.WinForms.GuiaRemision;
using EVANS.UI.WinForms.Identidad;
using EVANS.UI.WinForms.Recepcion;
using EVANS.UI.WinForms.Reportes;
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
        mnuConsultaRuc.Click  += mnuConsultaRuc_Click;
        mnuClientes.Click += mnuClientes_Click;
        mnuEmpresas.Click += mnuEmpresas_Click;
        mnuChoferes.Click += mnuChoferes_Click;
        mnuVehiculos.Click += mnuVehiculos_Click;
        mnuCarretas.Click += mnuCarretas_Click;
        mnuDestinos.Click += mnuDestinos_Click;
        mnuEstados.Click += mnuEstados_Click;
        mnuTiposIdentificacion.Click += mnuTiposIdentificacion_Click;
        mnuUsuarios.Click += mnuUsuarios_Click;
        mnuEnviosMensuales.Click += mnuEnviosMensuales_Click;
        mnuGuiasPorCliente.Click += mnuGuiasPorCliente_Click;
        mnuReporteVentas.Click += mnuReporteVentas_Click;
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

    private void mnuConsultaRuc_Click(object? sender, EventArgs e)
    {
        var mediator = _services.GetRequiredService<IMediator>();
        using var form = new frmConsultaRuc(mediator);
        form.ShowDialog(this);
    }

    private void mnuEstados_Click(object? sender, EventArgs e)
    {
        var form = ActivatorUtilities.CreateInstance<frmMantEstado>(_services);
        form.MdiParent = this;
        form.Show();
    }

    private void mnuEmpresas_Click(object? sender, EventArgs e)
    {
        var form = ActivatorUtilities.CreateInstance<frmMantEmpresa>(_services);
        form.MdiParent = this;
        form.Show();
    }

    private void mnuClientes_Click(object? sender, EventArgs e)
    {
        var form = ActivatorUtilities.CreateInstance<frmMantCliente>(_services);
        form.MdiParent = this;
        form.Show();
    }

    private void mnuChoferes_Click(object? sender, EventArgs e)
    {
        var form = ActivatorUtilities.CreateInstance<frmMantChofer>(_services);
        form.MdiParent = this;
        form.Show();
    }

    private void mnuVehiculos_Click(object? sender, EventArgs e)
    {
        var form = ActivatorUtilities.CreateInstance<frmMantVehiculo>(_services);
        form.MdiParent = this;
        form.Show();
    }

    private void mnuCarretas_Click(object? sender, EventArgs e)
    {
        var form = ActivatorUtilities.CreateInstance<frmMantCarreta>(_services);
        form.MdiParent = this;
        form.Show();
    }
    private void mnuDestinos_Click(object? sender, EventArgs e)
    {
        var form = ActivatorUtilities.CreateInstance<frmMantDestino>(_services);
        form.MdiParent = this;
        form.Show();
    }

    private void mnuTiposIdentificacion_Click(object? sender, EventArgs e)
    {
        var form = ActivatorUtilities.CreateInstance<frmMantTipoID>(_services);
        form.MdiParent = this;
        form.Show();
    }

    private void mnuUsuarios_Click(object? sender, EventArgs e)
    {
        var form = ActivatorUtilities.CreateInstance<frmMantUsuarios>(_services);
        form.MdiParent = this;
        form.Show();
    }

    private void mnuEnviosMensuales_Click(object? sender, EventArgs e)
    {
        var currentSession = _services.GetRequiredService<ICurrentSession>();
        var year = currentSession.Current?.Year ?? DateTime.Today.Year;
        using var form = ActivatorUtilities.CreateInstance<frmConsEnviosMensuales>(_services, year);
        form.ShowDialog(this);
    }

    private void mnuGuiasPorCliente_Click(object? sender, EventArgs e)
    {
        var currentSession = _services.GetRequiredService<ICurrentSession>();
        var year = currentSession.Current?.Year ?? DateTime.Today.Year;
        using var form = ActivatorUtilities.CreateInstance<frmConsGuiasPorCliente>(_services, year);
        form.ShowDialog(this);
    }

    private void mnuReporteVentas_Click(object? sender, EventArgs e)
    {
        var currentSession = _services.GetRequiredService<ICurrentSession>();
        var year = currentSession.Current?.Year ?? DateTime.Today.Year;
        using var form = ActivatorUtilities.CreateInstance<frmReporteVentas>(_services, year);
        form.ShowDialog(this);
    }
}
