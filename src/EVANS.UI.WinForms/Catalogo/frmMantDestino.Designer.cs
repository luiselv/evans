namespace EVANS.UI.WinForms.Catalogo;

partial class frmMantDestino
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
    private ColumnHeader columnHeaderNombre = null!;
    private ColumnHeader columnHeaderDistancia = null!;
    private Label lblListadoInfo = null!;
    private RadioButton optTodos = null!;
    private RadioButton optBuscar = null!;
    private TabPage tabPageDetalles = null!;
    private ComboBox cbEstado = null!;
    private TextBox txtDistancia = null!;
    private TextBox txtNombre = null!;
    private TextBox txtCodigo = null!;
    private Label lblCodigo = null!;
    private Label lblKilometros = null!;
    private Label lblEstado = null!;
    private Label lblmsg = null!;
    private Label lblDistancia = null!;
    private Label lblNombre = null!;
    private Label lblEstadoRequired = null!;
    private Label lblNombreRequired = null!;

    protected override void Dispose(bool disposing)
    {
        if (disposing)
            components?.Dispose();
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMantDestino));
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
        columnHeaderNombre = new ColumnHeader();
        columnHeaderDistancia = new ColumnHeader();
        lblListadoInfo = new Label();
        optTodos = new RadioButton();
        optBuscar = new RadioButton();
        tabPageDetalles = new TabPage();
        cbEstado = new ComboBox();
        txtDistancia = new TextBox();
        txtNombre = new TextBox();
        txtCodigo = new TextBox();
        lblCodigo = new Label();
        lblKilometros = new Label();
        lblEstado = new Label();
        lblmsg = new Label();
        lblDistancia = new Label();
        lblNombre = new Label();
        lblEstadoRequired = new Label();
        lblNombreRequired = new Label();
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
        lvListado.Columns.AddRange(new ColumnHeader[] { columnHeaderCodigo, columnHeaderNombre, columnHeaderDistancia });
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
        columnHeaderCodigo.Width = 31;
        columnHeaderNombre.Text = "Nombre";
        columnHeaderNombre.Width = 125;
        columnHeaderDistancia.Text = "Distancia Virtual (km)";
        columnHeaderDistancia.Width = 150;

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
        optBuscar.Text = "Por Nombre :";
        optBuscar.UseVisualStyleBackColor = false;

        tabPageDetalles.BackColor = SystemColors.Control;
        tabPageDetalles.Controls.Add(cbEstado);
        tabPageDetalles.Controls.Add(txtDistancia);
        tabPageDetalles.Controls.Add(txtNombre);
        tabPageDetalles.Controls.Add(txtCodigo);
        tabPageDetalles.Controls.Add(lblCodigo);
        tabPageDetalles.Controls.Add(lblKilometros);
        tabPageDetalles.Controls.Add(lblEstado);
        tabPageDetalles.Controls.Add(lblmsg);
        tabPageDetalles.Controls.Add(lblDistancia);
        tabPageDetalles.Controls.Add(lblNombre);
        tabPageDetalles.Controls.Add(lblEstadoRequired);
        tabPageDetalles.Controls.Add(lblNombreRequired);
        tabPageDetalles.Location = new Point(4, 23);
        tabPageDetalles.Name = "TabPage2";
        tabPageDetalles.Padding = new Padding(3);
        tabPageDetalles.Size = new Size(529, 431);
        tabPageDetalles.TabIndex = 1;
        tabPageDetalles.Text = "Detalles";

        cbEstado.FormattingEnabled = true;
        cbEstado.Location = new Point(158, 137);
        cbEstado.MaxLength = 10;
        cbEstado.Name = "cbEstado";
        cbEstado.Size = new Size(145, 21);
        cbEstado.TabIndex = 45;

        txtDistancia.Location = new Point(158, 99);
        txtDistancia.MaxLength = 70;
        txtDistancia.Name = "txtDistancia";
        txtDistancia.Size = new Size(145, 20);
        txtDistancia.TabIndex = 44;

        txtNombre.Location = new Point(158, 59);
        txtNombre.MaxLength = 70;
        txtNombre.Name = "txtNombre";
        txtNombre.Size = new Size(313, 20);
        txtNombre.TabIndex = 43;

        txtCodigo.Location = new Point(158, 22);
        txtCodigo.MaxLength = 0;
        txtCodigo.Name = "txtCodigo";
        txtCodigo.ReadOnly = true;
        txtCodigo.Size = new Size(145, 20);
        txtCodigo.TabIndex = 42;

        lblCodigo.AutoSize = false;
        lblCodigo.Location = new Point(108, 25);
        lblCodigo.Name = "Label4";
        lblCodigo.Size = new Size(46, 13);
        lblCodigo.TabIndex = 57;
        lblCodigo.Text = "Código :";

        lblKilometros.AutoSize = false;
        lblKilometros.BackColor = SystemColors.Control;
        lblKilometros.Cursor = Cursors.Default;
        lblKilometros.ForeColor = SystemColors.ControlText;
        lblKilometros.Location = new Point(306, 103);
        lblKilometros.Name = "Label3";
        lblKilometros.RightToLeft = RightToLeft.No;
        lblKilometros.Size = new Size(24, 13);
        lblKilometros.TabIndex = 49;
        lblKilometros.Text = "km.";

        lblEstado.AutoSize = false;
        lblEstado.BackColor = SystemColors.Control;
        lblEstado.Cursor = Cursors.Default;
        lblEstado.ForeColor = SystemColors.ControlText;
        lblEstado.Location = new Point(108, 140);
        lblEstado.Name = "Label5";
        lblEstado.RightToLeft = RightToLeft.No;
        lblEstado.Size = new Size(46, 13);
        lblEstado.TabIndex = 49;
        lblEstado.Text = "Estado :";

        lblmsg.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        lblmsg.BackColor = Color.FromArgb(128, 128, 128);
        lblmsg.Cursor = Cursors.Default;
        lblmsg.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
        lblmsg.ForeColor = Color.White;
        lblmsg.Location = new Point(20, 391);
        lblmsg.Name = "lblmsg";
        lblmsg.RightToLeft = RightToLeft.No;
        lblmsg.Size = new Size(489, 17);
        lblmsg.TabIndex = 48;
        lblmsg.Text = "Los campos marcados con asterisco (*) son obligatorios";
        lblmsg.TextAlign = ContentAlignment.TopCenter;

        lblDistancia.AutoSize = false;
        lblDistancia.BackColor = SystemColors.Control;
        lblDistancia.Cursor = Cursors.Default;
        lblDistancia.ForeColor = SystemColors.ControlText;
        lblDistancia.Location = new Point(65, 102);
        lblDistancia.Name = "Label2";
        lblDistancia.RightToLeft = RightToLeft.No;
        lblDistancia.Size = new Size(89, 13);
        lblDistancia.TabIndex = 50;
        lblDistancia.Text = "Distancia Virtual :";

        lblNombre.AutoSize = false;
        lblNombre.BackColor = SystemColors.Control;
        lblNombre.Cursor = Cursors.Default;
        lblNombre.ForeColor = SystemColors.ControlText;
        lblNombre.Location = new Point(104, 63);
        lblNombre.Name = "Label1";
        lblNombre.RightToLeft = RightToLeft.No;
        lblNombre.Size = new Size(50, 13);
        lblNombre.TabIndex = 51;
        lblNombre.Text = "Nombre :";

        lblEstadoRequired.BackColor = SystemColors.Control;
        lblEstadoRequired.Cursor = Cursors.Default;
        lblEstadoRequired.Font = new Font("Microsoft Sans Serif", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
        lblEstadoRequired.ForeColor = SystemColors.ControlText;
        lblEstadoRequired.Location = new Point(309, 138);
        lblEstadoRequired.Name = "Label7";
        lblEstadoRequired.RightToLeft = RightToLeft.No;
        lblEstadoRequired.Size = new Size(9, 17);
        lblEstadoRequired.TabIndex = 53;
        lblEstadoRequired.Text = "*";

        lblNombreRequired.BackColor = SystemColors.Control;
        lblNombreRequired.Cursor = Cursors.Default;
        lblNombreRequired.Font = new Font("Microsoft Sans Serif", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
        lblNombreRequired.ForeColor = SystemColors.ControlText;
        lblNombreRequired.Location = new Point(476, 59);
        lblNombreRequired.Name = "Label11";
        lblNombreRequired.RightToLeft = RightToLeft.No;
        lblNombreRequired.Size = new Size(9, 17);
        lblNombreRequired.TabIndex = 53;
        lblNombreRequired.Text = "*";

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
        Name = "frmMantDestino";
        Text = "Registro de Destinos";
        tabControl1.ResumeLayout(false);
        tabPageListado.ResumeLayout(false);
        tabPageListado.PerformLayout();
        tabPageDetalles.ResumeLayout(false);
        tabPageDetalles.PerformLayout();
        ResumeLayout(false);
    }
}
