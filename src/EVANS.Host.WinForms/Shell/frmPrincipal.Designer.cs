namespace EVANS.Host.WinForms.Shell;

partial class frmPrincipal
{
    private System.ComponentModel.IContainer components = null;
    private MenuStrip menuStrip;
    private ToolStripMenuItem mnuGuias;
    private ToolStripMenuItem mnuManifiestos;
    private ToolStripMenuItem mnuComprobantes;
    private ToolStripMenuItem mnuRecepciones;
    private ToolStripMenuItem mnuCatalogos;
    private ToolStripMenuItem mnuConsultaRuc;
    private ToolStripMenuItem mnuReportes;
    private ToolStripMenuItem mnuEnviosMensuales;
    private ToolStripMenuItem mnuGuiasPorCliente;
    private ToolStripMenuItem mnuReporteVentas;

    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
            components.Dispose();
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        menuStrip = new MenuStrip();
        mnuGuias = new ToolStripMenuItem();
        mnuManifiestos = new ToolStripMenuItem();
        mnuComprobantes = new ToolStripMenuItem();
        mnuRecepciones = new ToolStripMenuItem();
        mnuCatalogos = new ToolStripMenuItem();
        mnuConsultaRuc = new ToolStripMenuItem();
        mnuReportes = new ToolStripMenuItem();
        mnuEnviosMensuales = new ToolStripMenuItem();
        mnuGuiasPorCliente = new ToolStripMenuItem();
        mnuReporteVentas = new ToolStripMenuItem();

        menuStrip.SuspendLayout();
        SuspendLayout();

        menuStrip.Items.AddRange([mnuGuias, mnuManifiestos, mnuComprobantes, mnuRecepciones, mnuCatalogos, mnuReportes]);
        menuStrip.Location = new Point(0, 0);
        menuStrip.Size = new Size(1024, 24);

        mnuGuias.Text = "Guías de Remisión";
        mnuManifiestos.Text = "Manifiestos";
        mnuComprobantes.Text = "Comprobantes";
        mnuRecepciones.Text = "Recepciones";
        mnuCatalogos.Text = "Catálogos";
        mnuCatalogos.DropDownItems.AddRange([mnuConsultaRuc]);

        mnuConsultaRuc.Text = "Consulta RUC";
        mnuReportes.Text = "Reportes";
        mnuReportes.DropDownItems.AddRange([mnuEnviosMensuales, mnuGuiasPorCliente, mnuReporteVentas]);
        mnuEnviosMensuales.Text = "Envíos Mensuales";
        mnuGuiasPorCliente.Text = "Guías por Cliente";
        mnuReporteVentas.Text = "Reporte Ventas";

        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(1024, 768);
        Controls.Add(menuStrip);
        IsMdiContainer = true;
        MainMenuStrip = menuStrip;
        Text = "EVANS — Sistema de Transporte";
        WindowState = FormWindowState.Maximized;

        menuStrip.ResumeLayout(false);
        menuStrip.PerformLayout();
        ResumeLayout(false);
        PerformLayout();
    }
}
