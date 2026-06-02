#nullable disable

namespace EVANS.UI.WinForms.Reportes;

public partial class frmReporteVentas
{
    private System.ComponentModel.IContainer components;
    private Label lblFechaDesde;
    private DateTimePicker dtpFechaDesde;
    private Label lblFechaHasta;
    private DateTimePicker dtpFechaHasta;
    private CheckBox chkFacturas;
    private CheckBox chkBoletas;
    private RadioButton rbTodos;
    private RadioButton rbCliente;
    private ComboBox cbCliente;
    private Button btnBuscar;
    private Button btnExportar;
    private Label lblError;
    private DataGridView dgvDetalles;
    private DataGridViewTextBoxColumn colFecha;
    private DataGridViewTextBoxColumn colTipo;
    private DataGridViewTextBoxColumn colSerie;
    private DataGridViewTextBoxColumn colNumero;
    private DataGridViewTextBoxColumn colNroId;
    private DataGridViewTextBoxColumn colCliente;
    private DataGridViewTextBoxColumn colValorVenta;
    private DataGridViewTextBoxColumn colIgv;
    private DataGridViewTextBoxColumn colTotal;

    protected override void Dispose(bool disposing)
    {
        if (disposing && components is not null)
            components.Dispose();
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        components = new System.ComponentModel.Container();
        lblFechaDesde = new Label();
        dtpFechaDesde = new DateTimePicker();
        lblFechaHasta = new Label();
        dtpFechaHasta = new DateTimePicker();
        chkFacturas = new CheckBox();
        chkBoletas = new CheckBox();
        rbTodos = new RadioButton();
        rbCliente = new RadioButton();
        cbCliente = new ComboBox();
        btnBuscar = new Button();
        btnExportar = new Button();
        lblError = new Label();
        dgvDetalles = new DataGridView();
        colFecha = new DataGridViewTextBoxColumn();
        colTipo = new DataGridViewTextBoxColumn();
        colSerie = new DataGridViewTextBoxColumn();
        colNumero = new DataGridViewTextBoxColumn();
        colNroId = new DataGridViewTextBoxColumn();
        colCliente = new DataGridViewTextBoxColumn();
        colValorVenta = new DataGridViewTextBoxColumn();
        colIgv = new DataGridViewTextBoxColumn();
        colTotal = new DataGridViewTextBoxColumn();

        ((System.ComponentModel.ISupportInitialize)dgvDetalles).BeginInit();
        SuspendLayout();

        lblFechaDesde.AutoSize = true;
        lblFechaDesde.Location = new Point(16, 18);
        lblFechaDesde.Name = "lblFechaDesde";
        lblFechaDesde.Size = new Size(75, 15);
        lblFechaDesde.TabIndex = 0;
        lblFechaDesde.Text = "Fecha desde:";

        dtpFechaDesde.Format = DateTimePickerFormat.Short;
        dtpFechaDesde.Location = new Point(100, 14);
        dtpFechaDesde.Name = "dtpFechaDesde";
        dtpFechaDesde.Size = new Size(110, 23);
        dtpFechaDesde.TabIndex = 1;

        lblFechaHasta.AutoSize = true;
        lblFechaHasta.Location = new Point(230, 18);
        lblFechaHasta.Name = "lblFechaHasta";
        lblFechaHasta.Size = new Size(72, 15);
        lblFechaHasta.TabIndex = 2;
        lblFechaHasta.Text = "Fecha hasta:";

        dtpFechaHasta.Format = DateTimePickerFormat.Short;
        dtpFechaHasta.Location = new Point(310, 14);
        dtpFechaHasta.Name = "dtpFechaHasta";
        dtpFechaHasta.Size = new Size(110, 23);
        dtpFechaHasta.TabIndex = 3;

        chkFacturas.AutoSize = true;
        chkFacturas.Location = new Point(440, 16);
        chkFacturas.Name = "chkFacturas";
        chkFacturas.Size = new Size(70, 19);
        chkFacturas.TabIndex = 4;
        chkFacturas.Text = "Facturas";
        chkFacturas.UseVisualStyleBackColor = true;

        chkBoletas.AutoSize = true;
        chkBoletas.Location = new Point(520, 16);
        chkBoletas.Name = "chkBoletas";
        chkBoletas.Size = new Size(64, 19);
        chkBoletas.TabIndex = 5;
        chkBoletas.Text = "Boletas";
        chkBoletas.UseVisualStyleBackColor = true;

        rbTodos.AutoSize = true;
        rbTodos.Location = new Point(16, 54);
        rbTodos.Name = "rbTodos";
        rbTodos.Size = new Size(57, 19);
        rbTodos.TabIndex = 6;
        rbTodos.TabStop = true;
        rbTodos.Text = "Todos";
        rbTodos.UseVisualStyleBackColor = true;
        rbTodos.CheckedChanged += rbTodos_CheckedChanged;

        rbCliente.AutoSize = true;
        rbCliente.Location = new Point(88, 54);
        rbCliente.Name = "rbCliente";
        rbCliente.Size = new Size(63, 19);
        rbCliente.TabIndex = 7;
        rbCliente.TabStop = true;
        rbCliente.Text = "Cliente";
        rbCliente.UseVisualStyleBackColor = true;
        rbCliente.CheckedChanged += rbCliente_CheckedChanged;

        cbCliente.DropDownStyle = ComboBoxStyle.DropDownList;
        cbCliente.Enabled = false;
        cbCliente.Location = new Point(164, 52);
        cbCliente.Name = "cbCliente";
        cbCliente.Size = new Size(260, 23);
        cbCliente.TabIndex = 8;

        btnBuscar.Location = new Point(440, 50);
        btnBuscar.Name = "btnBuscar";
        btnBuscar.Size = new Size(90, 26);
        btnBuscar.TabIndex = 9;
        btnBuscar.Text = "Buscar";
        btnBuscar.UseVisualStyleBackColor = true;
        btnBuscar.Click += btnBuscar_Click;

        btnExportar.Enabled = false;
        btnExportar.Location = new Point(540, 50);
        btnExportar.Name = "btnExportar";
        btnExportar.Size = new Size(90, 26);
        btnExportar.TabIndex = 10;
        btnExportar.Text = "Exportar";
        btnExportar.UseVisualStyleBackColor = true;
        btnExportar.Click += btnExportar_Click;

        lblError.ForeColor = Color.Firebrick;
        lblError.Location = new Point(16, 86);
        lblError.Name = "lblError";
        lblError.Size = new Size(620, 22);
        lblError.TabIndex = 11;

        dgvDetalles.AllowUserToAddRows = false;
        dgvDetalles.AllowUserToDeleteRows = false;
        dgvDetalles.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        dgvDetalles.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        dgvDetalles.Columns.AddRange([colFecha, colTipo, colSerie, colNumero, colNroId, colCliente, colValorVenta, colIgv, colTotal]);
        dgvDetalles.Location = new Point(16, 116);
        dgvDetalles.Name = "dgvDetalles";
        dgvDetalles.ReadOnly = true;
        dgvDetalles.RowHeadersVisible = false;
        dgvDetalles.Size = new Size(940, 420);
        dgvDetalles.TabIndex = 12;

        colFecha.HeaderText = "Fecha";
        colFecha.Name = "colFecha";
        colFecha.Width = 90;

        colTipo.HeaderText = "Tipo";
        colTipo.Name = "colTipo";
        colTipo.Width = 60;

        colSerie.HeaderText = "Serie";
        colSerie.Name = "colSerie";
        colSerie.Width = 70;

        colNumero.HeaderText = "Número";
        colNumero.Name = "colNumero";
        colNumero.Width = 80;

        colNroId.HeaderText = "Nro ID";
        colNroId.Name = "colNroId";
        colNroId.Width = 110;

        colCliente.HeaderText = "Cliente";
        colCliente.Name = "colCliente";
        colCliente.Width = 220;

        colValorVenta.HeaderText = "Valor Venta";
        colValorVenta.Name = "colValorVenta";
        colValorVenta.Width = 90;

        colIgv.HeaderText = "IGV";
        colIgv.Name = "colIgv";
        colIgv.Width = 80;

        colTotal.HeaderText = "Total";
        colTotal.Name = "colTotal";
        colTotal.Width = 90;

        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(976, 552);
        Controls.Add(dgvDetalles);
        Controls.Add(lblError);
        Controls.Add(btnExportar);
        Controls.Add(btnBuscar);
        Controls.Add(cbCliente);
        Controls.Add(rbCliente);
        Controls.Add(rbTodos);
        Controls.Add(chkBoletas);
        Controls.Add(chkFacturas);
        Controls.Add(dtpFechaHasta);
        Controls.Add(lblFechaHasta);
        Controls.Add(dtpFechaDesde);
        Controls.Add(lblFechaDesde);
        Name = "frmReporteVentas";
        Text = "Reporte de Ventas";

        ((System.ComponentModel.ISupportInitialize)dgvDetalles).EndInit();
        ResumeLayout(false);
        PerformLayout();
    }
}
