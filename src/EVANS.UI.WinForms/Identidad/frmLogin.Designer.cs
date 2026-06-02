namespace EVANS.UI.WinForms.Identidad;

partial class frmLogin
{
    private System.ComponentModel.IContainer components = null;
    private GroupBox grpAcceso;
    private Label lblUsuario;
    private Label lblClave;
    private TextBox txtUsuario;
    private TextBox txtClave;
    private Button btnIngresar;
    private Button btnSalir;
    private Label lblError;

    protected override void Dispose(bool disposing)
    {
        if (disposing && components is not null)
            components.Dispose();
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        components = new System.ComponentModel.Container();
        grpAcceso = new GroupBox();
        lblUsuario = new Label();
        lblClave = new Label();
        txtUsuario = new TextBox();
        txtClave = new TextBox();
        btnIngresar = new Button();
        btnSalir = new Button();
        lblError = new Label();

        grpAcceso.SuspendLayout();
        SuspendLayout();

        grpAcceso.Controls.Add(lblUsuario);
        grpAcceso.Controls.Add(lblClave);
        grpAcceso.Controls.Add(txtUsuario);
        grpAcceso.Controls.Add(txtClave);
        grpAcceso.Location = new Point(12, 12);
        grpAcceso.Name = "grpAcceso";
        grpAcceso.Size = new Size(320, 98);
        grpAcceso.TabIndex = 0;
        grpAcceso.TabStop = false;
        grpAcceso.Text = "Acceso al sistema";

        lblUsuario.AutoSize = true;
        lblUsuario.Location = new Point(42, 30);
        lblUsuario.Name = "lblUsuario";
        lblUsuario.Size = new Size(50, 15);
        lblUsuario.TabIndex = 0;
        lblUsuario.Text = "Usuario:";

        txtUsuario.CharacterCasing = CharacterCasing.Upper;
        txtUsuario.Location = new Point(108, 27);
        txtUsuario.Name = "txtUsuario";
        txtUsuario.Size = new Size(180, 23);
        txtUsuario.TabIndex = 1;

        lblClave.AutoSize = true;
        lblClave.Location = new Point(19, 63);
        lblClave.Name = "lblClave";
        lblClave.Size = new Size(73, 15);
        lblClave.TabIndex = 2;
        lblClave.Text = "Contraseña:";

        txtClave.CharacterCasing = CharacterCasing.Upper;
        txtClave.Location = new Point(108, 60);
        txtClave.Name = "txtClave";
        txtClave.PasswordChar = '*';
        txtClave.Size = new Size(180, 23);
        txtClave.TabIndex = 3;

        btnIngresar.Location = new Point(85, 151);
        btnIngresar.Name = "btnIngresar";
        btnIngresar.Size = new Size(75, 23);
        btnIngresar.TabIndex = 2;
        btnIngresar.Text = "Ingresar";
        btnIngresar.UseVisualStyleBackColor = true;
        btnIngresar.Click += btnIngresar_Click;

        btnSalir.Location = new Point(184, 151);
        btnSalir.Name = "btnSalir";
        btnSalir.Size = new Size(75, 23);
        btnSalir.TabIndex = 3;
        btnSalir.Text = "Salir";
        btnSalir.UseVisualStyleBackColor = true;
        btnSalir.Click += btnSalir_Click;

        lblError.ForeColor = Color.Firebrick;
        lblError.Location = new Point(12, 117);
        lblError.Name = "lblError";
        lblError.Size = new Size(320, 23);
        lblError.TabIndex = 1;
        lblError.TextAlign = ContentAlignment.MiddleCenter;

        AcceptButton = btnIngresar;
        CancelButton = btnSalir;
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(344, 191);
        Controls.Add(grpAcceso);
        Controls.Add(lblError);
        Controls.Add(btnIngresar);
        Controls.Add(btnSalir);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        Name = "frmLogin";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "EVANS Cargo S.A.C.";

        grpAcceso.ResumeLayout(false);
        grpAcceso.PerformLayout();
        ResumeLayout(false);
    }
}
