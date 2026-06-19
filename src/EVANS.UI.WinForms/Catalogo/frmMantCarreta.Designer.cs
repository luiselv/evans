namespace EVANS.UI.WinForms.Catalogo;

partial class frmMantCarreta
{
    private System.ComponentModel.IContainer components = null!;
    private Button btnNuevo = null!;
    private Button btnGrabar = null!;
    private Button btnEditar = null!;
    private Button btnCancelar = null!;
    private Button btnSalir = null!;
    private TabControl tabControl1 = null!;
    private TabPage tabPageListado = null!;
    private Button btnBuscar = null!;
    private TextBox txtBuscar = null!;
    private ListView lvListado = null!;
    private ColumnHeader columnHeaderCodigo = null!;
    private ColumnHeader columnHeaderMarca = null!;
    private ColumnHeader columnHeaderPlaca = null!;
    private ColumnHeader columnHeaderEmpresa = null!;
    private Label lblListadoInfo = null!;
    private RadioButton optTodos = null!;
    private RadioButton optBuscar = null!;
    private TabPage tabPageDetalles = null!;
    private TextBox txtCertificado = null!;
    private ComboBox cbEstado = null!;
    private ComboBox cbEmpresa = null!;
    private TextBox txtMarca = null!;
    private TextBox txtPlaca = null!;
    private TextBox txtCodigo = null!;
    private Label lblCodigo = null!;
    private Label lblPlaca = null!;
    private Label lblCertificado = null!;
    private Label lblEmpresa = null!;
    private Label lblEstado = null!;
    private Label lblmsg = null!;
    private Label lblMarca = null!;
    private Label lblEstadoRequired = null!;
    private Label lblPlacaRequired = null!;
    private Label lblEmpresaRequired = null!;
    private Label lblMarcaRequired = null!;

    protected override void Dispose(bool disposing)
    {
        if (disposing) components?.Dispose();
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMantCarreta));
        btnNuevo = new Button();
        btnGrabar = new Button();
        btnEditar = new Button();
        btnCancelar = new Button();
        btnSalir = new Button();
        tabControl1 = new TabControl();
        tabPageListado = new TabPage();
        btnBuscar = new Button();
        txtBuscar = new TextBox();
        lvListado = new ListView();
        columnHeaderCodigo = new ColumnHeader();
        columnHeaderMarca = new ColumnHeader();
        columnHeaderPlaca = new ColumnHeader();
        columnHeaderEmpresa = new ColumnHeader();
        lblListadoInfo = new Label();
        optTodos = new RadioButton();
        optBuscar = new RadioButton();
        tabPageDetalles = new TabPage();
        txtCertificado = new TextBox();
        cbEstado = new ComboBox();
        cbEmpresa = new ComboBox();
        txtMarca = new TextBox();
        txtPlaca = new TextBox();
        txtCodigo = new TextBox();
        lblCodigo = new Label();
        lblPlaca = new Label();
        lblCertificado = new Label();
        lblEmpresa = new Label();
        lblEstado = new Label();
        lblmsg = new Label();
        lblMarca = new Label();
        lblEstadoRequired = new Label();
        lblPlacaRequired = new Label();
        lblEmpresaRequired = new Label();
        lblMarcaRequired = new Label();
        tabControl1.SuspendLayout();
        tabPageListado.SuspendLayout();
        tabPageDetalles.SuspendLayout();
        SuspendLayout();

        btnNuevo.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        btnNuevo.BackColor = SystemColors.Control;
        btnNuevo.Cursor = Cursors.Default;
        btnNuevo.ForeColor = SystemColors.ControlText;
        btnNuevo.Image = (Image)resources.GetObject("btnNuevo.Image");
        btnNuevo.Location = new Point(561, 86);
        btnNuevo.Name = "btnNuevo";
        btnNuevo.RightToLeft = RightToLeft.No;
        btnNuevo.Size = new Size(62, 48);
        btnNuevo.TabIndex = 26;
        btnNuevo.Text = "Nuevo";
        btnNuevo.TextAlign = ContentAlignment.BottomCenter;
        btnNuevo.UseVisualStyleBackColor = false;

        btnGrabar.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        btnGrabar.BackColor = SystemColors.Control;
        btnGrabar.Cursor = Cursors.Default;
        btnGrabar.ForeColor = SystemColors.ControlText;
        btnGrabar.Image = (Image)resources.GetObject("btnGrabar.Image");
        btnGrabar.Location = new Point(561, 140);
        btnGrabar.Name = "btnGrabar";
        btnGrabar.RightToLeft = RightToLeft.No;
        btnGrabar.Size = new Size(62, 48);
        btnGrabar.TabIndex = 27;
        btnGrabar.Text = "Grabar";
        btnGrabar.TextAlign = ContentAlignment.BottomCenter;
        btnGrabar.UseVisualStyleBackColor = false;

        btnEditar.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        btnEditar.BackColor = SystemColors.Control;
        btnEditar.Cursor = Cursors.Default;
        btnEditar.ForeColor = SystemColors.ControlText;
        btnEditar.Image = (Image)resources.GetObject("btnEditar.Image");
        btnEditar.Location = new Point(561, 194);
        btnEditar.Name = "btnEditar";
        btnEditar.RightToLeft = RightToLeft.No;
        btnEditar.Size = new Size(62, 48);
        btnEditar.TabIndex = 28;
        btnEditar.Text = "Editar";
        btnEditar.TextAlign = ContentAlignment.BottomCenter;
        btnEditar.UseVisualStyleBackColor = false;

        btnCancelar.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        btnCancelar.BackColor = SystemColors.Control;
        btnCancelar.Cursor = Cursors.Default;
        btnCancelar.ForeColor = SystemColors.ControlText;
        btnCancelar.Image = (Image)resources.GetObject("btnCancelar.Image");
        btnCancelar.Location = new Point(561, 248);
        btnCancelar.Name = "btnCancelar";
        btnCancelar.RightToLeft = RightToLeft.No;
        btnCancelar.Size = new Size(62, 48);
        btnCancelar.TabIndex = 29;
        btnCancelar.Text = "Cancelar";
        btnCancelar.TextAlign = ContentAlignment.BottomCenter;
        btnCancelar.UseVisualStyleBackColor = false;

        btnSalir.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        btnSalir.BackColor = SystemColors.Control;
        btnSalir.Cursor = Cursors.Default;
        btnSalir.ForeColor = SystemColors.ControlText;
        btnSalir.Image = (Image)resources.GetObject("btnSalir.Image");
        btnSalir.Location = new Point(561, 302);
        btnSalir.Name = "btnSalir";
        btnSalir.RightToLeft = RightToLeft.No;
        btnSalir.Size = new Size(62, 48);
        btnSalir.TabIndex = 30;
        btnSalir.Text = "Salir";
        btnSalir.TextAlign = ContentAlignment.BottomCenter;
        btnSalir.UseVisualStyleBackColor = false;

        tabControl1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        tabControl1.Controls.Add(tabPageListado);
        tabControl1.Controls.Add(tabPageDetalles);
        tabControl1.Location = new Point(12, 12);
        tabControl1.Name = "TabControl1";
        tabControl1.SelectedIndex = 0;
        tabControl1.Size = new Size(537, 458);
        tabControl1.TabIndex = 31;

        tabPageListado.BackColor = SystemColors.Control;
        tabPageListado.Controls.Add(btnBuscar);
        tabPageListado.Controls.Add(txtBuscar);
        tabPageListado.Controls.Add(lvListado);
        tabPageListado.Controls.Add(lblListadoInfo);
        tabPageListado.Controls.Add(optTodos);
        tabPageListado.Controls.Add(optBuscar);
        tabPageListado.Location = new Point(4, 23);
        tabPageListado.Name = "TabPage1";
        tabPageListado.Padding = new Padding(3);
        tabPageListado.Size = new Size(529, 431);
        tabPageListado.TabIndex = 0;
        tabPageListado.Text = "Listado";

        btnBuscar.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        btnBuscar.BackColor = SystemColors.Control;
        btnBuscar.Cursor = Cursors.Default;
        btnBuscar.ForeColor = SystemColors.ControlText;
        btnBuscar.Image = (Image)resources.GetObject("btnBuscar.Image");
        btnBuscar.ImageAlign = ContentAlignment.MiddleLeft;
        btnBuscar.Location = new Point(437, 13);
        btnBuscar.Name = "btnBuscar";
        btnBuscar.RightToLeft = RightToLeft.No;
        btnBuscar.Size = new Size(72, 25);
        btnBuscar.TabIndex = 9;
        btnBuscar.Text = "Buscar";
        btnBuscar.TextAlign = ContentAlignment.MiddleRight;
        btnBuscar.UseVisualStyleBackColor = false;

        txtBuscar.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        txtBuscar.Location = new Point(200, 16);
        txtBuscar.MaxLength = 0;
        txtBuscar.Name = "txtBuscar";
        txtBuscar.Size = new Size(231, 20);
        txtBuscar.TabIndex = 8;

        lvListado.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        lvListado.Columns.AddRange(new ColumnHeader[] { columnHeaderCodigo, columnHeaderMarca, columnHeaderPlaca, columnHeaderEmpresa });
        lvListado.Font = new Font("Verdana", 8.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
        lvListado.FullRowSelect = true;
        lvListado.HeaderStyle = ColumnHeaderStyle.Nonclickable;
        lvListado.Location = new Point(20, 47);
        lvListado.Name = "lvListado";
        lvListado.Size = new Size(489, 341);
        lvListado.TabIndex = 10;
        lvListado.UseCompatibleStateImageBehavior = false;
        lvListado.View = View.Details;

        columnHeaderCodigo.Text = "ID";
        columnHeaderCodigo.Width = 32;
        columnHeaderMarca.Text = "Marca";
        columnHeaderMarca.Width = 161;
        columnHeaderPlaca.Text = "Placa";
        columnHeaderPlaca.Width = 86;
        columnHeaderEmpresa.Text = "Empresa";
        columnHeaderEmpresa.Width = 196;

        lblListadoInfo.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        lblListadoInfo.BackColor = Color.FromArgb(128, 128, 128);
        lblListadoInfo.Cursor = Cursors.Default;
        lblListadoInfo.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
        lblListadoInfo.ForeColor = Color.White;
        lblListadoInfo.Location = new Point(20, 400);
        lblListadoInfo.Name = "Label6";
        lblListadoInfo.RightToLeft = RightToLeft.No;
        lblListadoInfo.Size = new Size(489, 17);
        lblListadoInfo.TabIndex = 25;
        lblListadoInfo.Text = "Haga doble clic sobre un registro para ver los detalles";
        lblListadoInfo.TextAlign = ContentAlignment.TopCenter;

        optTodos.BackColor = SystemColors.Control;
        optTodos.Cursor = Cursors.Default;
        optTodos.ForeColor = SystemColors.ControlText;
        optTodos.Location = new Point(23, 13);
        optTodos.Name = "optTodos";
        optTodos.RightToLeft = RightToLeft.No;
        optTodos.Size = new Size(55, 25);
        optTodos.TabIndex = 6;
        optTodos.TabStop = true;
        optTodos.Text = "Todos";
        optTodos.UseVisualStyleBackColor = false;

        optBuscar.BackColor = SystemColors.Control;
        optBuscar.Cursor = Cursors.Default;
        optBuscar.ForeColor = SystemColors.ControlText;
        optBuscar.Location = new Point(105, 13);
        optBuscar.Name = "optBuscar";
        optBuscar.RightToLeft = RightToLeft.No;
        optBuscar.Size = new Size(89, 25);
        optBuscar.TabIndex = 7;
        optBuscar.TabStop = true;
        optBuscar.Text = "Por placa :";
        optBuscar.UseVisualStyleBackColor = false;

        tabPageDetalles.BackColor = SystemColors.Control;
        tabPageDetalles.Controls.Add(txtCertificado);
        tabPageDetalles.Controls.Add(cbEstado);
        tabPageDetalles.Controls.Add(cbEmpresa);
        tabPageDetalles.Controls.Add(txtMarca);
        tabPageDetalles.Controls.Add(txtPlaca);
        tabPageDetalles.Controls.Add(txtCodigo);
        tabPageDetalles.Controls.Add(lblCodigo);
        tabPageDetalles.Controls.Add(lblPlaca);
        tabPageDetalles.Controls.Add(lblCertificado);
        tabPageDetalles.Controls.Add(lblEmpresa);
        tabPageDetalles.Controls.Add(lblEstado);
        tabPageDetalles.Controls.Add(lblmsg);
        tabPageDetalles.Controls.Add(lblMarca);
        tabPageDetalles.Controls.Add(lblEstadoRequired);
        tabPageDetalles.Controls.Add(lblPlacaRequired);
        tabPageDetalles.Controls.Add(lblEmpresaRequired);
        tabPageDetalles.Controls.Add(lblMarcaRequired);
        tabPageDetalles.Location = new Point(4, 23);
        tabPageDetalles.Name = "TabPage2";
        tabPageDetalles.Padding = new Padding(3);
        tabPageDetalles.Size = new Size(529, 431);
        tabPageDetalles.TabIndex = 1;
        tabPageDetalles.Text = "Detalles";

        txtCertificado.CharacterCasing = CharacterCasing.Upper;
        txtCertificado.Location = new Point(190, 149);
        txtCertificado.Name = "txtCertificado";
        txtCertificado.Size = new Size(183, 20);
        txtCertificado.TabIndex = 61;

        cbEstado.Font = new Font("Verdana", 8.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
        cbEstado.FormattingEnabled = true;
        cbEstado.Location = new Point(190, 222);
        cbEstado.Name = "cbEstado";
        cbEstado.Size = new Size(183, 21);
        cbEstado.TabIndex = 60;

        cbEmpresa.Font = new Font("Verdana", 8.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
        cbEmpresa.FormattingEnabled = true;
        cbEmpresa.Location = new Point(190, 186);
        cbEmpresa.Name = "cbEmpresa";
        cbEmpresa.Size = new Size(261, 21);
        cbEmpresa.TabIndex = 59;

        txtMarca.CharacterCasing = CharacterCasing.Upper;
        txtMarca.Font = new Font("Verdana", 8.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
        txtMarca.Location = new Point(190, 77);
        txtMarca.MaxLength = 70;
        txtMarca.Name = "txtMarca";
        txtMarca.Size = new Size(181, 21);
        txtMarca.TabIndex = 43;

        txtPlaca.CharacterCasing = CharacterCasing.Upper;
        txtPlaca.Font = new Font("Verdana", 8.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
        txtPlaca.Location = new Point(190, 113);
        txtPlaca.MaxLength = 0;
        txtPlaca.Name = "txtPlaca";
        txtPlaca.Size = new Size(181, 21);
        txtPlaca.TabIndex = 46;

        txtCodigo.CharacterCasing = CharacterCasing.Upper;
        txtCodigo.Font = new Font("Verdana", 8.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
        txtCodigo.Location = new Point(190, 40);
        txtCodigo.MaxLength = 0;
        txtCodigo.Name = "txtCodigo";
        txtCodigo.ReadOnly = true;
        txtCodigo.Size = new Size(78, 21);
        txtCodigo.TabIndex = 42;

        lblCodigo.AutoSize = false;
        lblCodigo.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
        lblCodigo.Location = new Point(132, 43);
        lblCodigo.Name = "Label4";
        lblCodigo.Size = new Size(54, 13);
        lblCodigo.TabIndex = 57;
        lblCodigo.Text = "Código :";

        lblPlaca.AutoSize = false;
        lblPlaca.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
        lblPlaca.Location = new Point(139, 116);
        lblPlaca.Name = "Label5";
        lblPlaca.Size = new Size(47, 13);
        lblPlaca.TabIndex = 49;
        lblPlaca.Text = "Placa :";

        lblCertificado.AutoSize = false;
        lblCertificado.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
        lblCertificado.Location = new Point(26, 152);
        lblCertificado.Name = "Label3";
        lblCertificado.Size = new Size(160, 13);
        lblCertificado.TabIndex = 49;
        lblCertificado.Text = "Certificado de Inscripción :";

        lblEmpresa.AutoSize = false;
        lblEmpresa.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
        lblEmpresa.Location = new Point(123, 190);
        lblEmpresa.Name = "Label10";
        lblEmpresa.Size = new Size(63, 13);
        lblEmpresa.TabIndex = 49;
        lblEmpresa.Text = "Empresa :";

        lblEstado.AutoSize = false;
        lblEstado.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
        lblEstado.Location = new Point(132, 225);
        lblEstado.Name = "Label9";
        lblEstado.Size = new Size(54, 13);
        lblEstado.TabIndex = 49;
        lblEstado.Text = "Estado :";

        lblmsg.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        lblmsg.BackColor = Color.FromArgb(128, 128, 128);
        lblmsg.Cursor = Cursors.Default;
        lblmsg.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
        lblmsg.ForeColor = Color.White;
        lblmsg.Location = new Point(20, 397);
        lblmsg.Name = "lblmsg";
        lblmsg.RightToLeft = RightToLeft.No;
        lblmsg.Size = new Size(489, 17);
        lblmsg.TabIndex = 48;
        lblmsg.Text = "Los campos marcados con asterisco (*) son obligatorios";
        lblmsg.TextAlign = ContentAlignment.TopCenter;

        lblMarca.AutoSize = false;
        lblMarca.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
        lblMarca.Location = new Point(136, 81);
        lblMarca.Name = "Label1";
        lblMarca.Size = new Size(50, 13);
        lblMarca.TabIndex = 51;
        lblMarca.Text = "Marca :";

        lblEstadoRequired.BackColor = SystemColors.Control;
        lblEstadoRequired.Font = new Font("Microsoft Sans Serif", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
        lblEstadoRequired.ForeColor = SystemColors.ControlText;
        lblEstadoRequired.Location = new Point(377, 226);
        lblEstadoRequired.Name = "Label2";
        lblEstadoRequired.Size = new Size(9, 17);
        lblEstadoRequired.TabIndex = 53;
        lblEstadoRequired.Text = "*";

        lblPlacaRequired.BackColor = SystemColors.Control;
        lblPlacaRequired.Font = new Font("Microsoft Sans Serif", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
        lblPlacaRequired.ForeColor = SystemColors.ControlText;
        lblPlacaRequired.Location = new Point(375, 114);
        lblPlacaRequired.Name = "Label7";
        lblPlacaRequired.Size = new Size(9, 17);
        lblPlacaRequired.TabIndex = 53;
        lblPlacaRequired.Text = "*";

        lblEmpresaRequired.BackColor = SystemColors.Control;
        lblEmpresaRequired.Font = new Font("Microsoft Sans Serif", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
        lblEmpresaRequired.ForeColor = SystemColors.ControlText;
        lblEmpresaRequired.Location = new Point(457, 188);
        lblEmpresaRequired.Name = "Label12";
        lblEmpresaRequired.Size = new Size(9, 17);
        lblEmpresaRequired.TabIndex = 53;
        lblEmpresaRequired.Text = "*";

        lblMarcaRequired.BackColor = SystemColors.Control;
        lblMarcaRequired.Font = new Font("Microsoft Sans Serif", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
        lblMarcaRequired.ForeColor = SystemColors.ControlText;
        lblMarcaRequired.Location = new Point(375, 77);
        lblMarcaRequired.Name = "Label11";
        lblMarcaRequired.Size = new Size(9, 17);
        lblMarcaRequired.TabIndex = 53;
        lblMarcaRequired.Text = "*";

        AutoScaleDimensions = new SizeF(6F, 13F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(635, 490);
        Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
        Controls.Add(btnNuevo);
        Controls.Add(btnGrabar);
        Controls.Add(btnEditar);
        Controls.Add(btnCancelar);
        Controls.Add(btnSalir);
        Controls.Add(tabControl1);
        Icon = (Icon)resources.GetObject("$this.Icon");
        Name = "frmMantCarreta";
        Text = "Registro de Carretas";
        tabControl1.ResumeLayout(false);
        tabPageListado.ResumeLayout(false);
        tabPageListado.PerformLayout();
        tabPageDetalles.ResumeLayout(false);
        tabPageDetalles.PerformLayout();
        ResumeLayout(false);
    }
}
