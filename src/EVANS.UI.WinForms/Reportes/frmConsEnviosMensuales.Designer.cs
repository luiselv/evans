#nullable disable

namespace EVANS.UI.WinForms.Reportes;

public partial class frmConsEnviosMensuales
{
    private System.ComponentModel.IContainer components;
    private Label lblFechaDesde;
    private DateTimePicker dtpFechaDesde;
    private Label lblFechaHasta;
    private DateTimePicker dtpFechaHasta;
    private Button btnBuscar;
    private Button btnExportar;
    private Label lblDestinos;
    private DataGridView dgvDestinos;
    private DataGridViewCheckBoxColumn colDestinoSeleccionado;
    private DataGridViewTextBoxColumn colDestinoCodigo;
    private DataGridViewTextBoxColumn colDestinoNombre;
    private Label lblError;
    private DataGridView dgvDetalles;
    private DataGridViewTextBoxColumn colCliente;
    private DataGridViewTextBoxColumn colNroGuias;
    private DataGridViewTextBoxColumn colUltimoEnvio;

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
        btnBuscar = new Button();
        btnExportar = new Button();
        lblDestinos = new Label();
        dgvDestinos = new DataGridView();
        colDestinoSeleccionado = new DataGridViewCheckBoxColumn();
        colDestinoCodigo = new DataGridViewTextBoxColumn();
        colDestinoNombre = new DataGridViewTextBoxColumn();
        lblError = new Label();
        dgvDetalles = new DataGridView();
        colCliente = new DataGridViewTextBoxColumn();
        colNroGuias = new DataGridViewTextBoxColumn();
        colUltimoEnvio = new DataGridViewTextBoxColumn();

        ((System.ComponentModel.ISupportInitialize)dgvDestinos).BeginInit();
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

        btnBuscar.Location = new Point(440, 13);
        btnBuscar.Name = "btnBuscar";
        btnBuscar.Size = new Size(90, 26);
        btnBuscar.TabIndex = 4;
        btnBuscar.Text = "Buscar";
        btnBuscar.UseVisualStyleBackColor = true;
        btnBuscar.Click += btnBuscar_Click;

        btnExportar.Enabled = false;
        btnExportar.Location = new Point(540, 13);
        btnExportar.Name = "btnExportar";
        btnExportar.Size = new Size(90, 26);
        btnExportar.TabIndex = 5;
        btnExportar.Text = "Exportar";
        btnExportar.UseVisualStyleBackColor = true;
        btnExportar.Click += btnExportar_Click;

        lblDestinos.AutoSize = true;
        lblDestinos.Location = new Point(16, 56);
        lblDestinos.Name = "lblDestinos";
        lblDestinos.Size = new Size(55, 15);
        lblDestinos.TabIndex = 6;
        lblDestinos.Text = "Destinos:";

        dgvDestinos.AllowUserToAddRows = false;
        dgvDestinos.AllowUserToDeleteRows = false;
        dgvDestinos.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
        dgvDestinos.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        dgvDestinos.Columns.AddRange([colDestinoSeleccionado, colDestinoCodigo, colDestinoNombre]);
        dgvDestinos.Location = new Point(16, 80);
        dgvDestinos.Name = "dgvDestinos";
        dgvDestinos.RowHeadersVisible = false;
        dgvDestinos.Size = new Size(260, 380);
        dgvDestinos.TabIndex = 7;

        colDestinoSeleccionado.HeaderText = "";
        colDestinoSeleccionado.Name = "colDestinoSeleccionado";
        colDestinoSeleccionado.Width = 32;

        colDestinoCodigo.HeaderText = "Código";
        colDestinoCodigo.Name = "colDestinoCodigo";
        colDestinoCodigo.ReadOnly = true;
        colDestinoCodigo.Width = 60;

        colDestinoNombre.HeaderText = "Destino";
        colDestinoNombre.Name = "colDestinoNombre";
        colDestinoNombre.ReadOnly = true;
        colDestinoNombre.Width = 160;

        lblError.ForeColor = Color.Firebrick;
        lblError.Location = new Point(292, 52);
        lblError.Name = "lblError";
        lblError.Size = new Size(520, 22);
        lblError.TabIndex = 8;

        dgvDetalles.AllowUserToAddRows = false;
        dgvDetalles.AllowUserToDeleteRows = false;
        dgvDetalles.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        dgvDetalles.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        dgvDetalles.Columns.AddRange([colCliente, colNroGuias, colUltimoEnvio]);
        dgvDetalles.Location = new Point(292, 80);
        dgvDetalles.Name = "dgvDetalles";
        dgvDetalles.ReadOnly = true;
        dgvDetalles.RowHeadersVisible = false;
        dgvDetalles.Size = new Size(520, 380);
        dgvDetalles.TabIndex = 9;

        colCliente.HeaderText = "Cliente";
        colCliente.Name = "colCliente";
        colCliente.Width = 260;

        colNroGuias.HeaderText = "Nro. Guías";
        colNroGuias.Name = "colNroGuias";
        colNroGuias.Width = 90;

        colUltimoEnvio.HeaderText = "Último Envío";
        colUltimoEnvio.Name = "colUltimoEnvio";
        colUltimoEnvio.Width = 110;

        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(628, 601);
        Controls.Add(dgvDetalles);
        Controls.Add(lblError);
        Controls.Add(dgvDestinos);
        Controls.Add(lblDestinos);
        Controls.Add(btnExportar);
        Controls.Add(btnBuscar);
        Controls.Add(dtpFechaHasta);
        Controls.Add(lblFechaHasta);
        Controls.Add(dtpFechaDesde);
        Controls.Add(lblFechaDesde);
        Name = "frmConsEnviosMensuales";
        Text = "Envios Mensuales";

        ((System.ComponentModel.ISupportInitialize)dgvDestinos).EndInit();
        ((System.ComponentModel.ISupportInitialize)dgvDetalles).EndInit();
        ResumeLayout(false);
        PerformLayout();
    }
}
