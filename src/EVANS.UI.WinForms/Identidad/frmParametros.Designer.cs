namespace EVANS.UI.WinForms.Identidad;

partial class frmParametros
{
    private System.ComponentModel.IContainer components = null!;
    private Button btnGrabar = null!;
    private Button btnEditar = null!;
    private GroupBox groupBoxFactura = null!;
    private GroupBox groupBoxBoleta = null!;
    private GroupBox groupBoxGuiaRemision = null!;
    private GroupBox groupBoxOtros = null!;
    private GroupBox groupBoxActions = null!;
    private Label lblFacturaSerie = null!;
    private Label lblFacturaNumero = null!;
    private Label lblBoletaSerie = null!;
    private Label lblBoletaNumero = null!;
    private Label lblGuiaSerie = null!;
    private Label lblGuiaNumero = null!;
    private Label lblIgv = null!;
    private Label lblPorcentaje = null!;
    private Label lblManifiesto = null!;
    private TextBox txtFacturaSerie = null!;
    private TextBox txtFacturaNro1 = null!;
    private TextBox txtBoletaSerie = null!;
    private TextBox txtBoletaNro1 = null!;
    private TextBox txtGRemisionSerie = null!;
    private TextBox txtGRemisionNro1 = null!;
    private TextBox txtIGV = null!;
    private TextBox txtManifiesto = null!;

    protected override void Dispose(bool disposing)
    {
        if (disposing)
            components?.Dispose();
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        components = new System.ComponentModel.Container();
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmParametros));
        btnGrabar = new Button();
        btnEditar = new Button();
        groupBoxActions = new GroupBox();
        groupBoxGuiaRemision = new GroupBox();
        txtGRemisionNro1 = new TextBox();
        txtGRemisionSerie = new TextBox();
        lblGuiaNumero = new Label();
        lblGuiaSerie = new Label();
        groupBoxBoleta = new GroupBox();
        txtBoletaNro1 = new TextBox();
        txtBoletaSerie = new TextBox();
        lblBoletaNumero = new Label();
        lblBoletaSerie = new Label();
        groupBoxFactura = new GroupBox();
        txtFacturaNro1 = new TextBox();
        txtFacturaSerie = new TextBox();
        lblFacturaNumero = new Label();
        lblFacturaSerie = new Label();
        groupBoxOtros = new GroupBox();
        txtManifiesto = new TextBox();
        txtIGV = new TextBox();
        lblManifiesto = new Label();
        lblPorcentaje = new Label();
        lblIgv = new Label();
        groupBoxActions.SuspendLayout();
        groupBoxGuiaRemision.SuspendLayout();
        groupBoxBoleta.SuspendLayout();
        groupBoxFactura.SuspendLayout();
        groupBoxOtros.SuspendLayout();
        SuspendLayout();

        groupBoxActions.Controls.Add(btnGrabar);
        groupBoxActions.Controls.Add(btnEditar);
        groupBoxActions.Location = new Point(126, 200);
        groupBoxActions.Name = "GroupBox4";
        groupBoxActions.Size = new Size(159, 82);
        groupBoxActions.TabIndex = 7;
        groupBoxActions.TabStop = false;

        btnGrabar.BackColor = SystemColors.Control;
        btnGrabar.Cursor = Cursors.Default;
        btnGrabar.Enabled = false;
        btnGrabar.ForeColor = SystemColors.ControlText;
        btnGrabar.Image = (Image)resources.GetObject("btnGrabar.Image")!;
        btnGrabar.Location = new Point(83, 19);
        btnGrabar.Name = "btnGrabar";
        btnGrabar.RightToLeft = RightToLeft.No;
        btnGrabar.Size = new Size(62, 48);
        btnGrabar.TabIndex = 9;
        btnGrabar.Text = "Grabar";
        btnGrabar.TextAlign = ContentAlignment.BottomCenter;
        btnGrabar.UseVisualStyleBackColor = false;

        btnEditar.BackColor = SystemColors.Control;
        btnEditar.Cursor = Cursors.Default;
        btnEditar.ForeColor = SystemColors.ControlText;
        btnEditar.Image = (Image)resources.GetObject("btnEditar.Image")!;
        btnEditar.Location = new Point(14, 19);
        btnEditar.Name = "btnEditar";
        btnEditar.RightToLeft = RightToLeft.No;
        btnEditar.Size = new Size(62, 48);
        btnEditar.TabIndex = 8;
        btnEditar.Text = "Editar";
        btnEditar.TextAlign = ContentAlignment.BottomCenter;
        btnEditar.UseVisualStyleBackColor = false;

        groupBoxGuiaRemision.Controls.Add(txtGRemisionNro1);
        groupBoxGuiaRemision.Controls.Add(txtGRemisionSerie);
        groupBoxGuiaRemision.Controls.Add(lblGuiaNumero);
        groupBoxGuiaRemision.Controls.Add(lblGuiaSerie);
        groupBoxGuiaRemision.Location = new Point(12, 106);
        groupBoxGuiaRemision.Name = "GroupBox3";
        groupBoxGuiaRemision.Size = new Size(188, 88);
        groupBoxGuiaRemision.TabIndex = 6;
        groupBoxGuiaRemision.TabStop = false;
        groupBoxGuiaRemision.Text = "Guia de Remisión";

        lblGuiaNumero.AutoSize = true;
        lblGuiaNumero.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
        lblGuiaNumero.Location = new Point(20, 56);
        lblGuiaNumero.Name = "Label8";
        lblGuiaNumero.Size = new Size(58, 13);
        lblGuiaNumero.TabIndex = 9;
        lblGuiaNumero.Text = "Número :";

        lblGuiaSerie.AutoSize = true;
        lblGuiaSerie.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
        lblGuiaSerie.Location = new Point(34, 30);
        lblGuiaSerie.Name = "Label9";
        lblGuiaSerie.Size = new Size(44, 13);
        lblGuiaSerie.TabIndex = 8;
        lblGuiaSerie.Text = "Serie :";

        groupBoxBoleta.Controls.Add(txtBoletaNro1);
        groupBoxBoleta.Controls.Add(txtBoletaSerie);
        groupBoxBoleta.Controls.Add(lblBoletaNumero);
        groupBoxBoleta.Controls.Add(lblBoletaSerie);
        groupBoxBoleta.Location = new Point(206, 12);
        groupBoxBoleta.Name = "GroupBox2";
        groupBoxBoleta.Size = new Size(185, 88);
        groupBoxBoleta.TabIndex = 5;
        groupBoxBoleta.TabStop = false;
        groupBoxBoleta.Text = "Boleta";

        lblBoletaNumero.AutoSize = true;
        lblBoletaNumero.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
        lblBoletaNumero.Location = new Point(21, 54);
        lblBoletaNumero.Name = "Label3";
        lblBoletaNumero.Size = new Size(58, 13);
        lblBoletaNumero.TabIndex = 5;
        lblBoletaNumero.Text = "Número :";

        lblBoletaSerie.AutoSize = true;
        lblBoletaSerie.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
        lblBoletaSerie.Location = new Point(35, 28);
        lblBoletaSerie.Name = "Label4";
        lblBoletaSerie.Size = new Size(44, 13);
        lblBoletaSerie.TabIndex = 4;
        lblBoletaSerie.Text = "Serie :";

        groupBoxFactura.Controls.Add(txtFacturaNro1);
        groupBoxFactura.Controls.Add(txtFacturaSerie);
        groupBoxFactura.Controls.Add(lblFacturaNumero);
        groupBoxFactura.Controls.Add(lblFacturaSerie);
        groupBoxFactura.Location = new Point(12, 12);
        groupBoxFactura.Name = "GroupBox1";
        groupBoxFactura.Size = new Size(188, 88);
        groupBoxFactura.TabIndex = 4;
        groupBoxFactura.TabStop = false;
        groupBoxFactura.Text = "Factura";

        lblFacturaNumero.AutoSize = true;
        lblFacturaNumero.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
        lblFacturaNumero.Location = new Point(20, 57);
        lblFacturaNumero.Name = "Label2";
        lblFacturaNumero.Size = new Size(58, 13);
        lblFacturaNumero.TabIndex = 1;
        lblFacturaNumero.Text = "Número :";

        lblFacturaSerie.AutoSize = true;
        lblFacturaSerie.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
        lblFacturaSerie.Location = new Point(34, 31);
        lblFacturaSerie.Name = "Label1";
        lblFacturaSerie.Size = new Size(44, 13);
        lblFacturaSerie.TabIndex = 0;
        lblFacturaSerie.Text = "Serie :";

        groupBoxOtros.Controls.Add(txtManifiesto);
        groupBoxOtros.Controls.Add(txtIGV);
        groupBoxOtros.Controls.Add(lblManifiesto);
        groupBoxOtros.Controls.Add(lblPorcentaje);
        groupBoxOtros.Controls.Add(lblIgv);
        groupBoxOtros.Location = new Point(206, 106);
        groupBoxOtros.Name = "GroupBox5";
        groupBoxOtros.Size = new Size(185, 88);
        groupBoxOtros.TabIndex = 6;
        groupBoxOtros.TabStop = false;
        groupBoxOtros.Text = "Otros";

        lblManifiesto.AutoSize = true;
        lblManifiesto.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
        lblManifiesto.Location = new Point(6, 56);
        lblManifiesto.Name = "Label14";
        lblManifiesto.Size = new Size(73, 13);
        lblManifiesto.TabIndex = 8;
        lblManifiesto.Text = "Manifiesto :";

        lblPorcentaje.AutoSize = true;
        lblPorcentaje.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
        lblPorcentaje.Location = new Point(149, 30);
        lblPorcentaje.Name = "Label6";
        lblPorcentaje.Size = new Size(16, 13);
        lblPorcentaje.TabIndex = 1;
        lblPorcentaje.Text = "%";

        lblIgv.AutoSize = true;
        lblIgv.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
        lblIgv.Location = new Point(43, 30);
        lblIgv.Name = "Label5";
        lblIgv.Size = new Size(36, 13);
        lblIgv.TabIndex = 1;
        lblIgv.Text = "IGV :";

        txtFacturaSerie.Location = new Point(84, 28);
        txtFacturaSerie.Name = "txtFacturaSerie";
        txtFacturaSerie.Size = new Size(80, 20);
        txtFacturaSerie.TabIndex = 3;
        txtFacturaSerie.TextAlign = HorizontalAlignment.Center;

        txtFacturaNro1.Location = new Point(84, 54);
        txtFacturaNro1.Name = "txtFacturaNro1";
        txtFacturaNro1.Size = new Size(80, 20);
        txtFacturaNro1.TabIndex = 4;
        txtFacturaNro1.TextAlign = HorizontalAlignment.Center;

        txtBoletaSerie.Location = new Point(85, 25);
        txtBoletaSerie.Name = "txtBoletaSerie";
        txtBoletaSerie.Size = new Size(80, 20);
        txtBoletaSerie.TabIndex = 6;
        txtBoletaSerie.TextAlign = HorizontalAlignment.Center;

        txtBoletaNro1.Location = new Point(85, 54);
        txtBoletaNro1.Name = "txtBoletaNro1";
        txtBoletaNro1.Size = new Size(80, 20);
        txtBoletaNro1.TabIndex = 7;
        txtBoletaNro1.TextAlign = HorizontalAlignment.Center;

        txtGRemisionSerie.Location = new Point(85, 27);
        txtGRemisionSerie.Name = "txtGRemisionSerie";
        txtGRemisionSerie.Size = new Size(79, 20);
        txtGRemisionSerie.TabIndex = 10;
        txtGRemisionSerie.TextAlign = HorizontalAlignment.Center;

        txtGRemisionNro1.Location = new Point(85, 53);
        txtGRemisionNro1.Name = "txtGRemisionNro1";
        txtGRemisionNro1.Size = new Size(79, 20);
        txtGRemisionNro1.TabIndex = 11;
        txtGRemisionNro1.TextAlign = HorizontalAlignment.Center;

        txtIGV.Location = new Point(85, 27);
        txtIGV.Name = "txtIGV";
        txtIGV.Size = new Size(44, 20);
        txtIGV.TabIndex = 9;
        txtIGV.TextAlign = HorizontalAlignment.Center;

        txtManifiesto.Location = new Point(85, 53);
        txtManifiesto.Name = "txtManifiesto";
        txtManifiesto.Size = new Size(80, 20);
        txtManifiesto.TabIndex = 10;
        txtManifiesto.TextAlign = HorizontalAlignment.Center;

        AutoScaleDimensions = new SizeF(6F, 13F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(403, 289);
        Controls.Add(groupBoxActions);
        Controls.Add(groupBoxGuiaRemision);
        Controls.Add(groupBoxBoleta);
        Controls.Add(groupBoxFactura);
        Controls.Add(groupBoxOtros);
        Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
        FormBorderStyle = FormBorderStyle.FixedSingle;
        Icon = (Icon)resources.GetObject("$this.Icon")!;
        MaximizeBox = false;
        Name = "frmParametros";
        Text = "Parámetros";
        groupBoxActions.ResumeLayout(false);
        groupBoxGuiaRemision.ResumeLayout(false);
        groupBoxGuiaRemision.PerformLayout();
        groupBoxBoleta.ResumeLayout(false);
        groupBoxBoleta.PerformLayout();
        groupBoxFactura.ResumeLayout(false);
        groupBoxFactura.PerformLayout();
        groupBoxOtros.ResumeLayout(false);
        groupBoxOtros.PerformLayout();
        ResumeLayout(false);
    }
}
