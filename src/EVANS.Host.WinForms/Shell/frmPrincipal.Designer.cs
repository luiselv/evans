namespace EVANS.Host.WinForms.Shell;

partial class frmPrincipal
{
    private System.ComponentModel.IContainer components = null;
    private MenuStrip menuStrip;
    private ToolStripMenuItem mnuGuias;
    private ToolStripMenuItem mnuManifiestos;
    private ToolStripMenuItem mnuComprobantes;
    private ToolStripMenuItem mnuCatalogos;
    private ToolStripMenuItem mnuReportes;

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
        mnuCatalogos = new ToolStripMenuItem();
        mnuReportes = new ToolStripMenuItem();

        menuStrip.SuspendLayout();
        SuspendLayout();

        menuStrip.Items.AddRange([mnuGuias, mnuManifiestos, mnuComprobantes, mnuCatalogos, mnuReportes]);
        menuStrip.Location = new Point(0, 0);
        menuStrip.Size = new Size(1024, 24);

        mnuGuias.Text = "Guías de Remisión";
        mnuManifiestos.Text = "Manifiestos";
        mnuComprobantes.Text = "Comprobantes";
        mnuCatalogos.Text = "Catálogos";
        mnuReportes.Text = "Reportes";

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
