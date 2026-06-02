#nullable disable

namespace EVANS.UI.WinForms.Reportes;

public partial class frmConsGuiasPorCliente
{
    private System.ComponentModel.IContainer components;
    private Label lblCliente;
    private ComboBox cbCliente;
    private Label lblNroId;
    private TextBox txtNroId;
    private Label lblFechaDesde;
    private DateTimePicker dtpFechaDesde;
    private Label lblFechaHasta;
    private DateTimePicker dtpFechaHasta;
    private CheckBox chkPendientes;
    private Button btnBuscar;
    private Label lblError;
    private DataGridView dgvListado;
    private DataGridViewTextBoxColumn colCodigo;
    private DataGridViewTextBoxColumn colNroDoc;
    private DataGridViewTextBoxColumn colRemitente;
    private DataGridViewTextBoxColumn colDestinatario;
    private DataGridViewTextBoxColumn colFechaEmision;
    private DataGridViewTextBoxColumn colFechaTraslado;
    private DataGridViewTextBoxColumn colBultos;
    private DataGridViewTextBoxColumn colCostoTotal;
    private DataGridViewTextBoxColumn colEnviado;

    protected override void Dispose(bool disposing)
    {
        if (disposing && components is not null)
            components.Dispose();
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        components = new System.ComponentModel.Container();
        lblCliente = new Label();
        cbCliente = new ComboBox();
        lblNroId = new Label();
        txtNroId = new TextBox();
        lblFechaDesde = new Label();
        dtpFechaDesde = new DateTimePicker();
        lblFechaHasta = new Label();
        dtpFechaHasta = new DateTimePicker();
        chkPendientes = new CheckBox();
        btnBuscar = new Button();
        lblError = new Label();
        dgvListado = new DataGridView();
        colCodigo = new DataGridViewTextBoxColumn();
        colNroDoc = new DataGridViewTextBoxColumn();
        colRemitente = new DataGridViewTextBoxColumn();
        colDestinatario = new DataGridViewTextBoxColumn();
        colFechaEmision = new DataGridViewTextBoxColumn();
        colFechaTraslado = new DataGridViewTextBoxColumn();
        colBultos = new DataGridViewTextBoxColumn();
        colCostoTotal = new DataGridViewTextBoxColumn();
        colEnviado = new DataGridViewTextBoxColumn();

        ((System.ComponentModel.ISupportInitialize)dgvListado).BeginInit();
        SuspendLayout();

        lblCliente.AutoSize = true;
        lblCliente.Location = new Point(16, 18);
        lblCliente.Name = "lblCliente";
        lblCliente.Size = new Size(47, 15);
        lblCliente.TabIndex = 0;
        lblCliente.Text = "Cliente:";

        cbCliente.DropDownStyle = ComboBoxStyle.DropDownList;
        cbCliente.Location = new Point(72, 14);
        cbCliente.Name = "cbCliente";
        cbCliente.Size = new Size(260, 23);
        cbCliente.TabIndex = 1;
        cbCliente.SelectedIndexChanged += cbCliente_SelectedIndexChanged;

        lblNroId.AutoSize = true;
        lblNroId.Location = new Point(350, 18);
        lblNroId.Name = "lblNroId";
        lblNroId.Size = new Size(45, 15);
        lblNroId.TabIndex = 2;
        lblNroId.Text = "Nro ID:";

        txtNroId.Location = new Point(404, 14);
        txtNroId.Name = "txtNroId";
        txtNroId.Size = new Size(130, 23);
        txtNroId.TabIndex = 3;
        txtNroId.KeyPress += txtNroId_KeyPress;

        lblFechaDesde.AutoSize = true;
        lblFechaDesde.Location = new Point(16, 58);
        lblFechaDesde.Name = "lblFechaDesde";
        lblFechaDesde.Size = new Size(75, 15);
        lblFechaDesde.TabIndex = 4;
        lblFechaDesde.Text = "Fecha desde:";

        dtpFechaDesde.Format = DateTimePickerFormat.Short;
        dtpFechaDesde.Location = new Point(100, 54);
        dtpFechaDesde.Name = "dtpFechaDesde";
        dtpFechaDesde.Size = new Size(110, 23);
        dtpFechaDesde.TabIndex = 5;

        lblFechaHasta.AutoSize = true;
        lblFechaHasta.Location = new Point(230, 58);
        lblFechaHasta.Name = "lblFechaHasta";
        lblFechaHasta.Size = new Size(72, 15);
        lblFechaHasta.TabIndex = 6;
        lblFechaHasta.Text = "Fecha hasta:";

        dtpFechaHasta.Format = DateTimePickerFormat.Short;
        dtpFechaHasta.Location = new Point(310, 54);
        dtpFechaHasta.Name = "dtpFechaHasta";
        dtpFechaHasta.Size = new Size(110, 23);
        dtpFechaHasta.TabIndex = 7;

        chkPendientes.AutoSize = true;
        chkPendientes.Location = new Point(440, 56);
        chkPendientes.Name = "chkPendientes";
        chkPendientes.Size = new Size(83, 19);
        chkPendientes.TabIndex = 8;
        chkPendientes.Text = "Pendientes";
        chkPendientes.UseVisualStyleBackColor = true;

        btnBuscar.Location = new Point(544, 53);
        btnBuscar.Name = "btnBuscar";
        btnBuscar.Size = new Size(90, 26);
        btnBuscar.TabIndex = 9;
        btnBuscar.Text = "Buscar";
        btnBuscar.UseVisualStyleBackColor = true;
        btnBuscar.Click += btnBuscar_Click;

        lblError.ForeColor = Color.Firebrick;
        lblError.Location = new Point(16, 88);
        lblError.Name = "lblError";
        lblError.Size = new Size(600, 22);
        lblError.TabIndex = 10;

        dgvListado.AllowUserToAddRows = false;
        dgvListado.AllowUserToDeleteRows = false;
        dgvListado.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        dgvListado.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        dgvListado.Columns.AddRange([colCodigo, colNroDoc, colRemitente, colDestinatario, colFechaEmision, colFechaTraslado, colBultos, colCostoTotal, colEnviado]);
        dgvListado.Location = new Point(16, 116);
        dgvListado.Name = "dgvListado";
        dgvListado.ReadOnly = true;
        dgvListado.RowHeadersVisible = false;
        dgvListado.Size = new Size(940, 420);
        dgvListado.TabIndex = 11;

        colCodigo.HeaderText = "Código";
        colCodigo.Name = "colCodigo";
        colCodigo.Width = 60;

        colNroDoc.HeaderText = "Nro Doc";
        colNroDoc.Name = "colNroDoc";
        colNroDoc.Width = 90;

        colRemitente.HeaderText = "Remitente";
        colRemitente.Name = "colRemitente";
        colRemitente.Width = 170;

        colDestinatario.HeaderText = "Destinatario";
        colDestinatario.Name = "colDestinatario";
        colDestinatario.Width = 170;

        colFechaEmision.HeaderText = "F. Emisión";
        colFechaEmision.Name = "colFechaEmision";
        colFechaEmision.Width = 90;

        colFechaTraslado.HeaderText = "F. Traslado";
        colFechaTraslado.Name = "colFechaTraslado";
        colFechaTraslado.Width = 90;

        colBultos.HeaderText = "Bultos";
        colBultos.Name = "colBultos";
        colBultos.Width = 70;

        colCostoTotal.HeaderText = "Costo Total";
        colCostoTotal.Name = "colCostoTotal";
        colCostoTotal.Width = 90;

        colEnviado.HeaderText = "Enviado";
        colEnviado.Name = "colEnviado";
        colEnviado.Width = 70;

        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(976, 552);
        Controls.Add(dgvListado);
        Controls.Add(lblError);
        Controls.Add(btnBuscar);
        Controls.Add(chkPendientes);
        Controls.Add(dtpFechaHasta);
        Controls.Add(lblFechaHasta);
        Controls.Add(dtpFechaDesde);
        Controls.Add(lblFechaDesde);
        Controls.Add(txtNroId);
        Controls.Add(lblNroId);
        Controls.Add(cbCliente);
        Controls.Add(lblCliente);
        Name = "frmConsGuiasPorCliente";
        Text = "Consulta de Guías por Cliente";

        ((System.ComponentModel.ISupportInitialize)dgvListado).EndInit();
        ResumeLayout(false);
        PerformLayout();
    }
}
