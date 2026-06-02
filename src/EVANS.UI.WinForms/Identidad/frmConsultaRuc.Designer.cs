namespace EVANS.UI.WinForms.Identidad;

partial class frmConsultaRuc
{
    private System.ComponentModel.IContainer components = null;
    private Label lblRuc;
    private TextBox txtRuc;
    private Button btnConsultar;
    private Button btnCerrar;
    private Label lblError;
    private Label lblRazonSocialCaption;
    private Label lblEstadoCaption;
    private Label lblCondicionCaption;
    private Label lblDireccionCaption;
    private Label lblRazonSocial;
    private Label lblEstado;
    private Label lblCondicion;
    private Label lblDireccion;

    protected override void Dispose(bool disposing)
    {
        if (disposing && components is not null)
            components.Dispose();
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        components = new System.ComponentModel.Container();
        lblRuc = new Label();
        txtRuc = new TextBox();
        btnConsultar = new Button();
        btnCerrar = new Button();
        lblError = new Label();
        lblRazonSocialCaption = new Label();
        lblEstadoCaption = new Label();
        lblCondicionCaption = new Label();
        lblDireccionCaption = new Label();
        lblRazonSocial = new Label();
        lblEstado = new Label();
        lblCondicion = new Label();
        lblDireccion = new Label();

        SuspendLayout();

        lblRuc.AutoSize = true;
        lblRuc.Location = new Point(24, 24);
        lblRuc.Name = "lblRuc";
        lblRuc.Size = new Size(32, 15);
        lblRuc.TabIndex = 0;
        lblRuc.Text = "RUC:";

        txtRuc.Location = new Point(120, 21);
        txtRuc.MaxLength = 11;
        txtRuc.Name = "txtRuc";
        txtRuc.Size = new Size(140, 23);
        txtRuc.TabIndex = 1;

        btnConsultar.Location = new Point(278, 20);
        btnConsultar.Name = "btnConsultar";
        btnConsultar.Size = new Size(84, 25);
        btnConsultar.TabIndex = 2;
        btnConsultar.Text = "Consultar";
        btnConsultar.UseVisualStyleBackColor = true;
        btnConsultar.Click += btnConsultar_Click;

        lblError.ForeColor = Color.Firebrick;
        lblError.Location = new Point(24, 55);
        lblError.Name = "lblError";
        lblError.Size = new Size(338, 22);
        lblError.TabIndex = 3;

        lblRazonSocialCaption.AutoSize = true;
        lblRazonSocialCaption.Location = new Point(24, 92);
        lblRazonSocialCaption.Name = "lblRazonSocialCaption";
        lblRazonSocialCaption.Size = new Size(76, 15);
        lblRazonSocialCaption.TabIndex = 4;
        lblRazonSocialCaption.Text = "Razón Social:";

        lblRazonSocial.Location = new Point(120, 92);
        lblRazonSocial.Name = "lblRazonSocial";
        lblRazonSocial.Size = new Size(320, 15);
        lblRazonSocial.TabIndex = 5;

        lblEstadoCaption.AutoSize = true;
        lblEstadoCaption.Location = new Point(24, 124);
        lblEstadoCaption.Name = "lblEstadoCaption";
        lblEstadoCaption.Size = new Size(45, 15);
        lblEstadoCaption.TabIndex = 6;
        lblEstadoCaption.Text = "Estado:";

        lblEstado.Location = new Point(120, 124);
        lblEstado.Name = "lblEstado";
        lblEstado.Size = new Size(180, 15);
        lblEstado.TabIndex = 7;

        lblCondicionCaption.AutoSize = true;
        lblCondicionCaption.Location = new Point(24, 156);
        lblCondicionCaption.Name = "lblCondicionCaption";
        lblCondicionCaption.Size = new Size(66, 15);
        lblCondicionCaption.TabIndex = 8;
        lblCondicionCaption.Text = "Condición:";

        lblCondicion.Location = new Point(120, 156);
        lblCondicion.Name = "lblCondicion";
        lblCondicion.Size = new Size(180, 15);
        lblCondicion.TabIndex = 9;

        lblDireccionCaption.AutoSize = true;
        lblDireccionCaption.Location = new Point(24, 188);
        lblDireccionCaption.Name = "lblDireccionCaption";
        lblDireccionCaption.Size = new Size(60, 15);
        lblDireccionCaption.TabIndex = 10;
        lblDireccionCaption.Text = "Dirección:";

        lblDireccion.Location = new Point(120, 188);
        lblDireccion.Name = "lblDireccion";
        lblDireccion.Size = new Size(320, 45);
        lblDireccion.TabIndex = 11;

        btnCerrar.Location = new Point(365, 248);
        btnCerrar.Name = "btnCerrar";
        btnCerrar.Size = new Size(75, 23);
        btnCerrar.TabIndex = 12;
        btnCerrar.Text = "Cerrar";
        btnCerrar.UseVisualStyleBackColor = true;
        btnCerrar.Click += btnCerrar_Click;

        AcceptButton = btnConsultar;
        CancelButton = btnCerrar;
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(464, 291);
        Controls.Add(lblRuc);
        Controls.Add(txtRuc);
        Controls.Add(btnConsultar);
        Controls.Add(lblError);
        Controls.Add(lblRazonSocialCaption);
        Controls.Add(lblRazonSocial);
        Controls.Add(lblEstadoCaption);
        Controls.Add(lblEstado);
        Controls.Add(lblCondicionCaption);
        Controls.Add(lblCondicion);
        Controls.Add(lblDireccionCaption);
        Controls.Add(lblDireccion);
        Controls.Add(btnCerrar);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        Name = "frmConsultaRuc";
        StartPosition = FormStartPosition.CenterParent;
        Text = "Consulta RUC";

        ResumeLayout(false);
        PerformLayout();
    }
}
