namespace EVANS.UI.WinForms.Identidad;

partial class frmLogin
{
    private System.ComponentModel.IContainer components = null;
    private GroupBox GroupBox1;
    private ComboBox cbBD;
    private Button btnCrear;
    private Button btnConectar;
    private TextBox txtServidor;
    private TextBox txtClave;
    private TextBox txtUsuario;
    private Label Label4;
    private Label Label3;
    private Label Label2;
    private Label Label1;
    private Button cbAceptar;
    private Button cbSalir;
    private FolderBrowserDialog FolderBrowserDialog1;
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
        var resources = new System.ComponentModel.ComponentResourceManager(typeof(frmLogin));
        GroupBox1 = new GroupBox();
        cbBD = new ComboBox();
        btnCrear = new Button();
        btnConectar = new Button();
        txtServidor = new TextBox();
        txtClave = new TextBox();
        txtUsuario = new TextBox();
        Label4 = new Label();
        Label3 = new Label();
        Label2 = new Label();
        Label1 = new Label();
        cbAceptar = new Button();
        cbSalir = new Button();
        FolderBrowserDialog1 = new FolderBrowserDialog();
        lblError = new Label();
        GroupBox1.SuspendLayout();
        SuspendLayout();
        //
        // GroupBox1
        //
        GroupBox1.Controls.Add(cbBD);
        GroupBox1.Controls.Add(btnCrear);
        GroupBox1.Controls.Add(btnConectar);
        GroupBox1.Controls.Add(txtServidor);
        GroupBox1.Controls.Add(txtClave);
        GroupBox1.Controls.Add(txtUsuario);
        GroupBox1.Controls.Add(Label4);
        GroupBox1.Controls.Add(Label3);
        GroupBox1.Controls.Add(Label2);
        GroupBox1.Controls.Add(Label1);
        GroupBox1.Location = new Point(12, 12);
        GroupBox1.Name = "GroupBox1";
        GroupBox1.Size = new Size(320, 145);
        GroupBox1.TabIndex = 0;
        GroupBox1.TabStop = false;
        GroupBox1.Text = "Acceso al sistema";
        //
        // cbBD
        //
        cbBD.DropDownStyle = ComboBoxStyle.DropDownList;
        cbBD.Enabled = false;
        cbBD.FormattingEnabled = true;
        cbBD.Location = new Point(105, 53);
        cbBD.Name = "cbBD";
        cbBD.Size = new Size(126, 21);
        cbBD.TabIndex = 3;
        //
        // btnCrear
        //
        btnCrear.Enabled = false;
        btnCrear.Location = new Point(237, 53);
        btnCrear.Name = "btnCrear";
        btnCrear.Size = new Size(75, 23);
        btnCrear.TabIndex = 4;
        btnCrear.Text = "Crear...";
        btnCrear.UseVisualStyleBackColor = true;
        btnCrear.Click += btnCrear_Click;
        //
        // btnConectar
        //
        btnConectar.Location = new Point(237, 23);
        btnConectar.Name = "btnConectar";
        btnConectar.Size = new Size(75, 23);
        btnConectar.TabIndex = 2;
        btnConectar.Text = "Conectar";
        btnConectar.UseVisualStyleBackColor = true;
        btnConectar.Click += btnConectar_Click;
        //
        // txtServidor
        //
        txtServidor.CharacterCasing = CharacterCasing.Upper;
        txtServidor.Location = new Point(105, 25);
        txtServidor.Name = "txtServidor";
        txtServidor.Size = new Size(126, 20);
        txtServidor.TabIndex = 1;
        txtServidor.KeyPress += txtServidor_KeyPress;
        //
        // txtClave
        //
        txtClave.CharacterCasing = CharacterCasing.Upper;
        txtClave.Enabled = false;
        txtClave.Location = new Point(105, 110);
        txtClave.Name = "txtClave";
        txtClave.PasswordChar = '*';
        txtClave.Size = new Size(126, 20);
        txtClave.TabIndex = 6;
        txtClave.KeyPress += txtClave_KeyPress;
        //
        // txtUsuario
        //
        txtUsuario.CharacterCasing = CharacterCasing.Upper;
        txtUsuario.Enabled = false;
        txtUsuario.Location = new Point(105, 82);
        txtUsuario.Name = "txtUsuario";
        txtUsuario.Size = new Size(126, 20);
        txtUsuario.TabIndex = 5;
        txtUsuario.KeyPress += txtUsuario_KeyPress;
        //
        // Label4
        //
        Label4.AutoSize = true;
        Label4.Location = new Point(16, 58);
        Label4.Name = "Label4";
        Label4.Size = new Size(83, 13);
        Label4.TabIndex = 1;
        Label4.Text = "Base de Datos :";
        //
        // Label3
        //
        Label3.AutoSize = true;
        Label3.Location = new Point(47, 28);
        Label3.Name = "Label3";
        Label3.Size = new Size(52, 13);
        Label3.TabIndex = 1;
        Label3.Text = "Servidor :";
        //
        // Label2
        //
        Label2.AutoSize = true;
        Label2.Location = new Point(32, 114);
        Label2.Name = "Label2";
        Label2.Size = new Size(67, 13);
        Label2.TabIndex = 1;
        Label2.Text = "Contraseña :";
        //
        // Label1
        //
        Label1.AutoSize = true;
        Label1.Location = new Point(50, 85);
        Label1.Name = "Label1";
        Label1.Size = new Size(49, 13);
        Label1.TabIndex = 0;
        Label1.Text = "Usuario :";
        //
        // cbAceptar
        //
        cbAceptar.Enabled = false;
        cbAceptar.Location = new Point(77, 168);
        cbAceptar.Name = "cbAceptar";
        cbAceptar.Size = new Size(75, 23);
        cbAceptar.TabIndex = 5;
        cbAceptar.Text = "Ingresar";
        cbAceptar.UseVisualStyleBackColor = true;
        cbAceptar.Click += cbAceptar_Click;
        //
        // cbSalir
        //
        cbSalir.Location = new Point(192, 168);
        cbSalir.Name = "cbSalir";
        cbSalir.Size = new Size(75, 23);
        cbSalir.TabIndex = 6;
        cbSalir.Text = "Salir";
        cbSalir.UseVisualStyleBackColor = true;
        cbSalir.Click += cbSalir_Click;
        //
        // lblError
        //
        lblError.ForeColor = Color.Firebrick;
        lblError.Location = new Point(12, 194);
        lblError.Name = "lblError";
        lblError.Size = new Size(320, 18);
        lblError.TabIndex = 7;
        lblError.TextAlign = ContentAlignment.MiddleCenter;
        lblError.Visible = false;
        //
        // frmLogin
        //
        AcceptButton = cbAceptar;
        CancelButton = cbSalir;
        AutoScaleDimensions = new SizeF(6F, 13F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(344, 201);
        Controls.Add(lblError);
        Controls.Add(cbSalir);
        Controls.Add(cbAceptar);
        Controls.Add(GroupBox1);
        Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        Icon = (Icon)resources.GetObject("$this.Icon")!;
        MaximizeBox = false;
        MinimizeBox = false;
        Name = "frmLogin";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "EVANS Cargo S.A.C.";
        GroupBox1.ResumeLayout(false);
        GroupBox1.PerformLayout();
        ResumeLayout(false);
    }
}
