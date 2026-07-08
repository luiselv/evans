namespace EVANS.UI.WinForms.Catalogo;

partial class frmMantCliente
{
    private System.ComponentModel.IContainer components = null!;
    private Button btnNuevo = null!;
    private Button btnGrabar = null!;
    private Button btnEditar = null!;
    private Button btnCancelar = null!;
    private Button btnSalir = null!;
    private TabControl tabControl1 = null!;
    private TabPage tabPageListado = null!;
    private DataGridView dgvListado = null!;
    private DataGridViewTextBoxColumn columnCodigo = null!;
    private DataGridViewTextBoxColumn columnNombre = null!;
    private DataGridViewTextBoxColumn columnTipoDoc = null!;
    private DataGridViewTextBoxColumn columnNumero = null!;
    private Button btnBuscar = null!;
    private TextBox txtBuscar = null!;
    private Label lblListadoInfo = null!;
    private RadioButton optTodos = null!;
    private RadioButton optNro = null!;
    private RadioButton optBuscar = null!;
    private TabPage tabPageDetalles = null!;
    private DataGridView dgvDireccion = null!;
    private DataGridViewTextBoxColumn columnDireccion = null!;
    private DataGridViewTextBoxColumn columnCiudad = null!;
    private DataGridViewTextBoxColumn columnProvincia = null!;
    private ComboBox cbTipoID = null!;
    private TextBox txtNombre = null!;
    private TextBox txtNroID = null!;
    private TextBox txtFax = null!;
    private TextBox txtRepresentante = null!;
    private TextBox txtEmail = null!;
    private TextBox txtTelefono = null!;
    private TextBox txtCodigo = null!;
    private Label lblCodigo = null!;
    private Label lblTipoId = null!;
    private Label lblNroId = null!;
    private Label lblTelefono = null!;
    private Label lblRepresentante = null!;
    private Label lblFax = null!;
    private Label lblEmail = null!;
    private Label lblDireccion = null!;
    private Label lblmsg = null!;
    private Label lblNombre = null!;
    private Label lblNumeroRequired = null!;
    private Label lblNombreRequired = null!;
    private TextBox txtBuscarCodigo = null!;
    private Label lblBuscarCodigo = null!;

    protected override void Dispose(bool disposing)
    {
        if (disposing) components?.Dispose();
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        components = new System.ComponentModel.Container();
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMantCliente));
        btnNuevo = new Button();
        btnGrabar = new Button();
        btnEditar = new Button();
        btnCancelar = new Button();
        btnSalir = new Button();
        tabControl1 = new TabControl();
        tabPageListado = new TabPage();
        dgvListado = new DataGridView();
        columnCodigo = new DataGridViewTextBoxColumn();
        columnNombre = new DataGridViewTextBoxColumn();
        columnTipoDoc = new DataGridViewTextBoxColumn();
        columnNumero = new DataGridViewTextBoxColumn();
        btnBuscar = new Button();
        txtBuscar = new TextBox();
        lblListadoInfo = new Label();
        optTodos = new RadioButton();
        optNro = new RadioButton();
        optBuscar = new RadioButton();
        tabPageDetalles = new TabPage();
        dgvDireccion = new DataGridView();
        columnDireccion = new DataGridViewTextBoxColumn();
        columnCiudad = new DataGridViewTextBoxColumn();
        columnProvincia = new DataGridViewTextBoxColumn();
        cbTipoID = new ComboBox();
        txtNombre = new TextBox();
        txtNroID = new TextBox();
        txtFax = new TextBox();
        txtRepresentante = new TextBox();
        txtEmail = new TextBox();
        txtTelefono = new TextBox();
        txtCodigo = new TextBox();
        lblCodigo = new Label();
        lblTipoId = new Label();
        lblNroId = new Label();
        lblTelefono = new Label();
        lblRepresentante = new Label();
        lblFax = new Label();
        lblEmail = new Label();
        lblDireccion = new Label();
        lblmsg = new Label();
        lblNombre = new Label();
        lblNumeroRequired = new Label();
        lblNombreRequired = new Label();
        txtBuscarCodigo = new TextBox();
        lblBuscarCodigo = new Label();
        tabControl1.SuspendLayout();
        tabPageListado.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)dgvListado).BeginInit();
        tabPageDetalles.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)dgvDireccion).BeginInit();
        SuspendLayout();

        btnNuevo.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        btnNuevo.BackColor = SystemColors.Control;
        btnNuevo.Cursor = Cursors.Default;
        btnNuevo.ForeColor = SystemColors.ControlText;
        btnNuevo.Image = (Image)resources.GetObject("btnNuevo.Image")!;
        btnNuevo.Location = new Point(561, 86);
        btnNuevo.Name = "btnNuevo";
        btnNuevo.RightToLeft = RightToLeft.No;
        btnNuevo.Size = new Size(62, 48);
        btnNuevo.TabIndex = 20;
        btnNuevo.Text = "Nuevo";
        btnNuevo.TextAlign = ContentAlignment.BottomCenter;
        btnNuevo.UseVisualStyleBackColor = false;

        btnGrabar.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        btnGrabar.BackColor = SystemColors.Control;
        btnGrabar.Cursor = Cursors.Default;
        btnGrabar.ForeColor = SystemColors.ControlText;
        btnGrabar.Image = (Image)resources.GetObject("btnGrabar.Image")!;
        btnGrabar.Location = new Point(561, 140);
        btnGrabar.Name = "btnGrabar";
        btnGrabar.RightToLeft = RightToLeft.No;
        btnGrabar.Size = new Size(62, 48);
        btnGrabar.TabIndex = 21;
        btnGrabar.Text = "Grabar";
        btnGrabar.TextAlign = ContentAlignment.BottomCenter;
        btnGrabar.UseVisualStyleBackColor = false;

        btnEditar.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        btnEditar.BackColor = SystemColors.Control;
        btnEditar.Cursor = Cursors.Default;
        btnEditar.ForeColor = SystemColors.ControlText;
        btnEditar.Image = (Image)resources.GetObject("btnEditar.Image")!;
        btnEditar.Location = new Point(561, 194);
        btnEditar.Name = "btnEditar";
        btnEditar.RightToLeft = RightToLeft.No;
        btnEditar.Size = new Size(62, 48);
        btnEditar.TabIndex = 22;
        btnEditar.Text = "Editar";
        btnEditar.TextAlign = ContentAlignment.BottomCenter;
        btnEditar.UseVisualStyleBackColor = false;

        btnCancelar.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        btnCancelar.BackColor = SystemColors.Control;
        btnCancelar.Cursor = Cursors.Default;
        btnCancelar.ForeColor = SystemColors.ControlText;
        btnCancelar.Image = (Image)resources.GetObject("btnCancelar.Image")!;
        btnCancelar.Location = new Point(561, 248);
        btnCancelar.Name = "btnCancelar";
        btnCancelar.RightToLeft = RightToLeft.No;
        btnCancelar.Size = new Size(62, 48);
        btnCancelar.TabIndex = 23;
        btnCancelar.Text = "Cancelar";
        btnCancelar.TextAlign = ContentAlignment.BottomCenter;
        btnCancelar.UseVisualStyleBackColor = false;

        btnSalir.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        btnSalir.BackColor = SystemColors.Control;
        btnSalir.Cursor = Cursors.Default;
        btnSalir.ForeColor = SystemColors.ControlText;
        btnSalir.Image = (Image)resources.GetObject("btnSalir.Image")!;
        btnSalir.Location = new Point(561, 302);
        btnSalir.Name = "btnSalir";
        btnSalir.RightToLeft = RightToLeft.No;
        btnSalir.Size = new Size(62, 48);
        btnSalir.TabIndex = 24;
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
        tabControl1.TabIndex = 25;

        tabPageListado.BackColor = SystemColors.Control;
        tabPageListado.Controls.Add(dgvListado);
        tabPageListado.Controls.Add(btnBuscar);
        tabPageListado.Controls.Add(txtBuscar);
        tabPageListado.Controls.Add(lblListadoInfo);
        tabPageListado.Controls.Add(optTodos);
        tabPageListado.Controls.Add(optNro);
        tabPageListado.Controls.Add(optBuscar);
        tabPageListado.Location = new Point(4, 24);
        tabPageListado.Name = "TabPage1";
        tabPageListado.Padding = new Padding(3);
        tabPageListado.Size = new Size(529, 430);
        tabPageListado.TabIndex = 0;
        tabPageListado.Text = "Listado";

        dgvListado.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        dgvListado.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        dgvListado.Columns.AddRange([columnCodigo, columnNombre, columnTipoDoc, columnNumero]);
        dgvListado.Location = new Point(23, 51);
        dgvListado.MultiSelect = false;
        dgvListado.Name = "dgvListado";
        dgvListado.ReadOnly = true;
        dgvListado.RowHeadersWidth = 25;
        dgvListado.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgvListado.Size = new Size(486, 346);
        dgvListado.TabIndex = 26;

        columnCodigo.HeaderText = "ID";
        columnCodigo.Name = "Column3";
        columnCodigo.ReadOnly = true;
        columnCodigo.Width = 40;
        columnNombre.HeaderText = "Nombre";
        columnNombre.Name = "Column4";
        columnNombre.ReadOnly = true;
        columnNombre.Width = 220;
        columnTipoDoc.HeaderText = "Tipo Doc";
        columnTipoDoc.Name = "Column5";
        columnTipoDoc.ReadOnly = true;
        columnTipoDoc.Width = 80;
        columnNumero.HeaderText = "Número";
        columnNumero.Name = "Column6";
        columnNumero.ReadOnly = true;
        columnNumero.Width = 80;

        btnBuscar.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        btnBuscar.BackColor = SystemColors.Control;
        btnBuscar.Cursor = Cursors.Default;
        btnBuscar.ForeColor = SystemColors.ControlText;
        btnBuscar.Image = (Image)resources.GetObject("btnBuscar.Image")!;
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
        txtBuscar.Location = new Point(256, 16);
        txtBuscar.MaxLength = 0;
        txtBuscar.Name = "txtBuscar";
        txtBuscar.Size = new Size(175, 20);
        txtBuscar.TabIndex = 8;

        lblListadoInfo.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        lblListadoInfo.BackColor = Color.FromArgb(128, 128, 128);
        lblListadoInfo.Cursor = Cursors.Default;
        lblListadoInfo.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
        lblListadoInfo.ForeColor = Color.White;
        lblListadoInfo.Location = new Point(23, 400);
        lblListadoInfo.Name = "Label6";
        lblListadoInfo.RightToLeft = RightToLeft.No;
        lblListadoInfo.Size = new Size(486, 17);
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

        optNro.BackColor = SystemColors.Control;
        optNro.Cursor = Cursors.Default;
        optNro.ForeColor = SystemColors.ControlText;
        optNro.Location = new Point(170, 13);
        optNro.Name = "optNro";
        optNro.RightToLeft = RightToLeft.No;
        optNro.Size = new Size(89, 25);
        optNro.TabIndex = 7;
        optNro.TabStop = true;
        optNro.Text = "Por Nro Doc";
        optNro.UseVisualStyleBackColor = false;

        optBuscar.BackColor = SystemColors.Control;
        optBuscar.Cursor = Cursors.Default;
        optBuscar.ForeColor = SystemColors.ControlText;
        optBuscar.Location = new Point(84, 13);
        optBuscar.Name = "optBuscar";
        optBuscar.RightToLeft = RightToLeft.No;
        optBuscar.Size = new Size(89, 25);
        optBuscar.TabIndex = 7;
        optBuscar.TabStop = true;
        optBuscar.Text = "Por Nombre";
        optBuscar.UseVisualStyleBackColor = false;

        tabPageDetalles.BackColor = SystemColors.Control;
        tabPageDetalles.Controls.Add(dgvDireccion);
        tabPageDetalles.Controls.Add(cbTipoID);
        tabPageDetalles.Controls.Add(txtNombre);
        tabPageDetalles.Controls.Add(txtNroID);
        tabPageDetalles.Controls.Add(txtFax);
        tabPageDetalles.Controls.Add(txtRepresentante);
        tabPageDetalles.Controls.Add(txtEmail);
        tabPageDetalles.Controls.Add(txtTelefono);
        tabPageDetalles.Controls.Add(txtCodigo);
        tabPageDetalles.Controls.Add(lblCodigo);
        tabPageDetalles.Controls.Add(lblTipoId);
        tabPageDetalles.Controls.Add(lblNroId);
        tabPageDetalles.Controls.Add(lblTelefono);
        tabPageDetalles.Controls.Add(lblRepresentante);
        tabPageDetalles.Controls.Add(lblFax);
        tabPageDetalles.Controls.Add(lblEmail);
        tabPageDetalles.Controls.Add(lblDireccion);
        tabPageDetalles.Controls.Add(lblmsg);
        tabPageDetalles.Controls.Add(lblNombre);
        tabPageDetalles.Controls.Add(lblNumeroRequired);
        tabPageDetalles.Controls.Add(lblNombreRequired);
        tabPageDetalles.Location = new Point(4, 24);
        tabPageDetalles.Name = "TabPage2";
        tabPageDetalles.Padding = new Padding(3);
        tabPageDetalles.Size = new Size(529, 430);
        tabPageDetalles.TabIndex = 1;
        tabPageDetalles.Text = "Detalles";

        dgvDireccion.AllowUserToResizeRows = false;
        dgvDireccion.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        dgvDireccion.Columns.AddRange([columnDireccion, columnCiudad, columnProvincia]);
        dgvDireccion.Location = new Point(128, 188);
        dgvDireccion.Name = "dgvDireccion";
        dgvDireccion.RowHeadersWidth = 25;
        dgvDireccion.Size = new Size(366, 150);
        dgvDireccion.TabIndex = 58;

        columnDireccion.HeaderText = "Dirección";
        columnDireccion.Name = "Direcciones";
        columnDireccion.Width = 250;
        columnCiudad.HeaderText = "Ciudad";
        columnCiudad.Name = "Column1";
        columnProvincia.HeaderText = "Provincia";
        columnProvincia.Name = "Column2";

        cbTipoID.FormattingEnabled = true;
        cbTipoID.Location = new Point(130, 74);
        cbTipoID.MaxLength = 15;
        cbTipoID.Name = "cbTipoID";
        cbTipoID.Size = new Size(160, 21);
        cbTipoID.TabIndex = 45;

        txtNombre.Location = new Point(130, 48);
        txtNombre.MaxLength = 70;
        txtNombre.Name = "txtNombre";
        txtNombre.Size = new Size(364, 20);
        txtNombre.TabIndex = 43;

        txtNroID.Location = new Point(352, 74);
        txtNroID.MaxLength = 11;
        txtNroID.Name = "txtNroID";
        txtNroID.Size = new Size(142, 20);
        txtNroID.TabIndex = 46;

        txtFax.Location = new Point(352, 101);
        txtFax.MaxLength = 30;
        txtFax.Name = "txtFax";
        txtFax.Size = new Size(142, 20);
        txtFax.TabIndex = 47;

        txtRepresentante.Location = new Point(130, 153);
        txtRepresentante.MaxLength = 70;
        txtRepresentante.Name = "txtRepresentante";
        txtRepresentante.Size = new Size(364, 20);
        txtRepresentante.TabIndex = 49;

        txtEmail.Location = new Point(130, 127);
        txtEmail.MaxLength = 50;
        txtEmail.Name = "txtEmail";
        txtEmail.Size = new Size(160, 20);
        txtEmail.TabIndex = 48;

        txtTelefono.Location = new Point(130, 101);
        txtTelefono.MaxLength = 50;
        txtTelefono.Name = "txtTelefono";
        txtTelefono.Size = new Size(160, 20);
        txtTelefono.TabIndex = 47;

        txtCodigo.Location = new Point(130, 22);
        txtCodigo.Name = "txtCodigo";
        txtCodigo.ReadOnly = true;
        txtCodigo.Size = new Size(145, 20);
        txtCodigo.TabIndex = 42;

        lblCodigo.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
        lblCodigo.Location = new Point(72, 25);
        lblCodigo.Name = "Label4";
        lblCodigo.Size = new Size(56, 13);
        lblCodigo.TabIndex = 57;
        lblCodigo.Text = "Código :";

        lblTipoId.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
        lblTipoId.Location = new Point(30, 77);
        lblTipoId.Name = "Label5";
        lblTipoId.Size = new Size(98, 13);
        lblTipoId.TabIndex = 57;
        lblTipoId.Text = "Tipo Doc. :";

        lblNroId.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
        lblNroId.Location = new Point(296, 77);
        lblNroId.Name = "Label3";
        lblNroId.Size = new Size(54, 13);
        lblNroId.TabIndex = 49;
        lblNroId.Text = "Número :";

        lblTelefono.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
        lblTelefono.Location = new Point(56, 104);
        lblTelefono.Name = "Label2";
        lblTelefono.Size = new Size(72, 13);
        lblTelefono.TabIndex = 49;
        lblTelefono.Text = "Teléfono :";

        lblRepresentante.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
        lblRepresentante.Location = new Point(27, 156);
        lblRepresentante.Name = "Label7";
        lblRepresentante.Size = new Size(101, 13);
        lblRepresentante.TabIndex = 49;
        lblRepresentante.Text = "Representante :";

        lblFax.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
        lblFax.Location = new Point(315, 104);
        lblFax.Name = "Label10";
        lblFax.Size = new Size(35, 13);
        lblFax.TabIndex = 49;
        lblFax.Text = "Fax :";

        lblEmail.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
        lblEmail.Location = new Point(77, 130);
        lblEmail.Name = "Label12";
        lblEmail.Size = new Size(51, 13);
        lblEmail.TabIndex = 49;
        lblEmail.Text = "E-mail :";

        lblDireccion.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
        lblDireccion.Location = new Point(48, 188);
        lblDireccion.Name = "Label9";
        lblDireccion.Size = new Size(80, 13);
        lblDireccion.TabIndex = 49;
        lblDireccion.Text = "Dirección :";

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

        lblNombre.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
        lblNombre.Location = new Point(62, 51);
        lblNombre.Name = "Label1";
        lblNombre.Size = new Size(66, 13);
        lblNombre.TabIndex = 51;
        lblNombre.Text = "Nombre :";

        lblNumeroRequired.BackColor = SystemColors.Control;
        lblNumeroRequired.Font = new Font("Microsoft Sans Serif", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
        lblNumeroRequired.ForeColor = SystemColors.ControlText;
        lblNumeroRequired.Location = new Point(500, 74);
        lblNumeroRequired.Name = "Label8";
        lblNumeroRequired.Size = new Size(9, 17);
        lblNumeroRequired.TabIndex = 53;
        lblNumeroRequired.Text = "*";

        lblNombreRequired.BackColor = SystemColors.Control;
        lblNombreRequired.Font = new Font("Microsoft Sans Serif", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
        lblNombreRequired.ForeColor = SystemColors.ControlText;
        lblNombreRequired.Location = new Point(500, 48);
        lblNombreRequired.Name = "Label11";
        lblNombreRequired.Size = new Size(9, 17);
        lblNombreRequired.TabIndex = 53;
        lblNombreRequired.Text = "*";

        txtBuscarCodigo.Location = new Point(555, 450);
        txtBuscarCodigo.Name = "txtBuscarCodigo";
        txtBuscarCodigo.Size = new Size(68, 20);
        txtBuscarCodigo.TabIndex = 26;

        lblBuscarCodigo.AutoSize = false;
        lblBuscarCodigo.Location = new Point(552, 434);
        lblBuscarCodigo.Name = "Label13";
        lblBuscarCodigo.Size = new Size(46, 13);
        lblBuscarCodigo.TabIndex = 27;
        lblBuscarCodigo.Text = "Código :";

        AutoScaleDimensions = new SizeF(6F, 13F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(635, 490);
        Controls.Add(lblBuscarCodigo);
        Controls.Add(txtBuscarCodigo);
        Controls.Add(btnNuevo);
        Controls.Add(btnGrabar);
        Controls.Add(btnEditar);
        Controls.Add(btnCancelar);
        Controls.Add(btnSalir);
        Controls.Add(tabControl1);
        Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
        Icon = (Icon)resources.GetObject("$this.Icon")!;
        Name = "frmMantCliente";
        Text = "Registro de Clientes";
        tabControl1.ResumeLayout(false);
        tabPageListado.ResumeLayout(false);
        tabPageListado.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)dgvListado).EndInit();
        tabPageDetalles.ResumeLayout(false);
        tabPageDetalles.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)dgvDireccion).EndInit();
        ResumeLayout(false);
        PerformLayout();
    }
}
