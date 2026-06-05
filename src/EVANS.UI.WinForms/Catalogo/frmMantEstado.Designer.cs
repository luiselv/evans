namespace EVANS.UI.WinForms.Catalogo;

partial class frmMantEstado
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
    private ColumnHeader columnHeaderDescripcion = null!;
    private Label lblListadoInfo = null!;
    private RadioButton optTodos = null!;
    private RadioButton optBuscar = null!;
    private TabPage tabPageDetalles = null!;
    private TextBox txtDescripcion = null!;
    private TextBox txtCodigo = null!;
    private Label lblCodigo = null!;
    private Label lblmsg = null!;
    private Label lblDescripcion = null!;
    private Label lblRequired = null!;

    protected override void Dispose(bool disposing)
    {
        if (disposing)
            components?.Dispose();
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
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
        columnHeaderDescripcion = new ColumnHeader();
        lblListadoInfo = new Label();
        optTodos = new RadioButton();
        optBuscar = new RadioButton();
        tabPageDetalles = new TabPage();
        txtDescripcion = new TextBox();
        txtCodigo = new TextBox();
        lblCodigo = new Label();
        lblmsg = new Label();
        lblDescripcion = new Label();
        lblRequired = new Label();
        tabControl1.SuspendLayout();
        tabPageListado.SuspendLayout();
        tabPageDetalles.SuspendLayout();
        SuspendLayout();

        btnNuevo.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        btnNuevo.BackColor = SystemColors.Control;
        btnNuevo.Cursor = Cursors.Default;
        btnNuevo.ForeColor = SystemColors.ControlText;
        btnNuevo.Location = new Point(561, 82);
        btnNuevo.Name = "btnNuevo";
        btnNuevo.RightToLeft = RightToLeft.No;
        btnNuevo.Size = new Size(62, 48);
        btnNuevo.TabIndex = 30;
        btnNuevo.Text = "Nuevo";
        btnNuevo.TextAlign = ContentAlignment.BottomCenter;
        btnNuevo.UseVisualStyleBackColor = false;

        btnGrabar.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        btnGrabar.BackColor = SystemColors.Control;
        btnGrabar.Cursor = Cursors.Default;
        btnGrabar.ForeColor = SystemColors.ControlText;
        btnGrabar.Location = new Point(561, 136);
        btnGrabar.Name = "btnGrabar";
        btnGrabar.RightToLeft = RightToLeft.No;
        btnGrabar.Size = new Size(62, 48);
        btnGrabar.TabIndex = 29;
        btnGrabar.Text = "Grabar";
        btnGrabar.TextAlign = ContentAlignment.BottomCenter;
        btnGrabar.UseVisualStyleBackColor = false;

        btnEditar.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        btnEditar.BackColor = SystemColors.Control;
        btnEditar.Cursor = Cursors.Default;
        btnEditar.ForeColor = SystemColors.ControlText;
        btnEditar.Location = new Point(561, 190);
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
        btnCancelar.Location = new Point(561, 244);
        btnCancelar.Name = "btnCancelar";
        btnCancelar.RightToLeft = RightToLeft.No;
        btnCancelar.Size = new Size(62, 48);
        btnCancelar.TabIndex = 27;
        btnCancelar.Text = "Cancelar";
        btnCancelar.TextAlign = ContentAlignment.BottomCenter;
        btnCancelar.UseVisualStyleBackColor = false;

        btnSalir.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        btnSalir.BackColor = SystemColors.Control;
        btnSalir.Cursor = Cursors.Default;
        btnSalir.ForeColor = SystemColors.ControlText;
        btnSalir.Location = new Point(561, 298);
        btnSalir.Name = "btnSalir";
        btnSalir.RightToLeft = RightToLeft.No;
        btnSalir.Size = new Size(62, 48);
        btnSalir.TabIndex = 26;
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
        tabPageListado.Controls.Add(btnBuscar);
        tabPageListado.Controls.Add(txtBuscar);
        tabPageListado.Controls.Add(lvListado);
        tabPageListado.Controls.Add(lblListadoInfo);
        tabPageListado.Controls.Add(optTodos);
        tabPageListado.Controls.Add(optBuscar);
        tabPageListado.Location = new Point(4, 24);
        tabPageListado.Name = "TabPage1";
        tabPageListado.Padding = new Padding(3);
        tabPageListado.Size = new Size(529, 430);
        tabPageListado.TabIndex = 0;
        tabPageListado.Text = "Listado";

        btnBuscar.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        btnBuscar.BackColor = SystemColors.Control;
        btnBuscar.Cursor = Cursors.Default;
        btnBuscar.ForeColor = SystemColors.ControlText;
        btnBuscar.Location = new Point(437, 13);
        btnBuscar.Name = "btnBuscar";
        btnBuscar.RightToLeft = RightToLeft.No;
        btnBuscar.Size = new Size(72, 25);
        btnBuscar.TabIndex = 19;
        btnBuscar.Text = "Buscar";
        btnBuscar.TextAlign = ContentAlignment.MiddleRight;
        btnBuscar.UseVisualStyleBackColor = false;

        txtBuscar.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        txtBuscar.Location = new Point(171, 16);
        txtBuscar.MaxLength = 10;
        txtBuscar.Name = "txtBuscar";
        txtBuscar.Size = new Size(260, 23);
        txtBuscar.TabIndex = 21;

        lvListado.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        lvListado.Columns.AddRange(new[] { columnHeaderCodigo, columnHeaderDescripcion });
        lvListado.Font = new Font("Verdana", 8.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
        lvListado.FullRowSelect = true;
        lvListado.HeaderStyle = ColumnHeaderStyle.Nonclickable;
        lvListado.Location = new Point(20, 47);
        lvListado.Name = "lvListado";
        lvListado.Size = new Size(489, 341);
        lvListado.TabIndex = 22;
        lvListado.UseCompatibleStateImageBehavior = false;
        lvListado.View = View.Details;

        columnHeaderCodigo.Text = "ID";
        columnHeaderCodigo.Width = 40;
        columnHeaderDescripcion.Text = "Descripción";
        columnHeaderDescripcion.Width = 439;

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
        optTodos.TabIndex = 23;
        optTodos.TabStop = true;
        optTodos.Text = "Todos";
        optTodos.UseVisualStyleBackColor = false;

        optBuscar.BackColor = SystemColors.Control;
        optBuscar.Cursor = Cursors.Default;
        optBuscar.ForeColor = SystemColors.ControlText;
        optBuscar.Location = new Point(94, 13);
        optBuscar.Name = "optBuscar";
        optBuscar.RightToLeft = RightToLeft.No;
        optBuscar.Size = new Size(75, 25);
        optBuscar.TabIndex = 24;
        optBuscar.TabStop = true;
        optBuscar.Text = "Nombre :";
        optBuscar.UseVisualStyleBackColor = false;

        tabPageDetalles.BackColor = SystemColors.Control;
        tabPageDetalles.Controls.Add(txtDescripcion);
        tabPageDetalles.Controls.Add(txtCodigo);
        tabPageDetalles.Controls.Add(lblCodigo);
        tabPageDetalles.Controls.Add(lblmsg);
        tabPageDetalles.Controls.Add(lblDescripcion);
        tabPageDetalles.Controls.Add(lblRequired);
        tabPageDetalles.Location = new Point(4, 24);
        tabPageDetalles.Name = "TabPage2";
        tabPageDetalles.Padding = new Padding(3);
        tabPageDetalles.Size = new Size(529, 430);
        tabPageDetalles.TabIndex = 1;
        tabPageDetalles.Text = "Detalles";

        txtDescripcion.Location = new Point(201, 84);
        txtDescripcion.MaxLength = 10;
        txtDescripcion.Name = "txtDescripcion";
        txtDescripcion.Size = new Size(145, 23);
        txtDescripcion.TabIndex = 43;

        txtCodigo.Location = new Point(201, 47);
        txtCodigo.MaxLength = 0;
        txtCodigo.Name = "txtCodigo";
        txtCodigo.ReadOnly = true;
        txtCodigo.Size = new Size(145, 23);
        txtCodigo.TabIndex = 42;

        lblCodigo.AutoSize = true;
        lblCodigo.Location = new Point(143, 50);
        lblCodigo.Name = "Label4";
        lblCodigo.Size = new Size(52, 15);
        lblCodigo.TabIndex = 57;
        lblCodigo.Text = "Código :";

        lblmsg.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        lblmsg.BackColor = Color.FromArgb(128, 128, 128);
        lblmsg.Cursor = Cursors.Default;
        lblmsg.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
        lblmsg.ForeColor = Color.White;
        lblmsg.Location = new Point(20, 394);
        lblmsg.Name = "lblmsg";
        lblmsg.RightToLeft = RightToLeft.No;
        lblmsg.Size = new Size(489, 17);
        lblmsg.TabIndex = 48;
        lblmsg.Text = "Los campos marcados con asterisco (*) son obligatorios";
        lblmsg.TextAlign = ContentAlignment.TopCenter;

        lblDescripcion.AutoSize = true;
        lblDescripcion.BackColor = SystemColors.Control;
        lblDescripcion.Cursor = Cursors.Default;
        lblDescripcion.ForeColor = SystemColors.ControlText;
        lblDescripcion.Location = new Point(120, 87);
        lblDescripcion.Name = "Label1";
        lblDescripcion.RightToLeft = RightToLeft.No;
        lblDescripcion.Size = new Size(76, 15);
        lblDescripcion.TabIndex = 51;
        lblDescripcion.Text = "Descripción :";

        lblRequired.BackColor = SystemColors.Control;
        lblRequired.Cursor = Cursors.Default;
        lblRequired.Font = new Font("Microsoft Sans Serif", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
        lblRequired.ForeColor = SystemColors.ControlText;
        lblRequired.Location = new Point(352, 85);
        lblRequired.Name = "Label11";
        lblRequired.RightToLeft = RightToLeft.No;
        lblRequired.Size = new Size(9, 17);
        lblRequired.TabIndex = 53;
        lblRequired.Text = "*";

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
        Name = "frmMantEstado";
        Text = "Registro de Estados";
        tabControl1.ResumeLayout(false);
        tabPageListado.ResumeLayout(false);
        tabPageListado.PerformLayout();
        tabPageDetalles.ResumeLayout(false);
        tabPageDetalles.PerformLayout();
        ResumeLayout(false);
    }
}
