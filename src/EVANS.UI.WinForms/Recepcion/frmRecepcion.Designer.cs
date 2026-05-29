namespace EVANS.UI.WinForms.Recepcion;

partial class frmRecepcion
{
    private System.ComponentModel.IContainer components = null;

    // Header controls
    private System.Windows.Forms.Label lblFechaEmision;
    private System.Windows.Forms.DateTimePicker dtpFechaEmision;
    private System.Windows.Forms.Label lblRemitente;
    private System.Windows.Forms.ComboBox cmbRemitente;
    private System.Windows.Forms.Label lblDestinatario;
    private System.Windows.Forms.ComboBox cmbDestinatario;
    private System.Windows.Forms.Label lblDestino;
    private System.Windows.Forms.ComboBox cmbDestino;
    private System.Windows.Forms.Label lblEstado;
    private System.Windows.Forms.ComboBox cmbEstado;
    private System.Windows.Forms.Label lblDirPartida;
    private System.Windows.Forms.TextBox txtDirPartida;
    private System.Windows.Forms.Label lblDirDestino;
    private System.Windows.Forms.TextBox txtDirDestino;
    private System.Windows.Forms.Label lblObservacion;
    private System.Windows.Forms.TextBox txtObservacion;
    private System.Windows.Forms.Label lblGuiaVinculada;

    // Detail grid
    private System.Windows.Forms.DataGridView dgvDetalles;
    private System.Windows.Forms.DataGridViewTextBoxColumn colCantidad;
    private System.Windows.Forms.DataGridViewTextBoxColumn colDescripcion;
    private System.Windows.Forms.DataGridViewTextBoxColumn colPeso;
    private System.Windows.Forms.DataGridViewTextBoxColumn colUnidad;
    private System.Windows.Forms.DataGridViewTextBoxColumn colCosto;
    private System.Windows.Forms.DataGridViewTextBoxColumn colTipoDoc;
    private System.Windows.Forms.DataGridViewTextBoxColumn colNroDoc;

    // Buttons
    private System.Windows.Forms.Button btnGrabar;
    private System.Windows.Forms.Button btnEliminar;
    private System.Windows.Forms.Button btnNuevo;
    private System.Windows.Forms.Button btnBuscar;
    private System.Windows.Forms.Button btnGenerarGuia;
    private System.Windows.Forms.Button btnCancelar;

    // Search panel
    private System.Windows.Forms.DateTimePicker dtpDesde;
    private System.Windows.Forms.DateTimePicker dtpHasta;
    private System.Windows.Forms.DataGridView dgvResultados;
    private System.Windows.Forms.DataGridViewTextBoxColumn colResCodig;
    private System.Windows.Forms.DataGridViewTextBoxColumn colResFecha;
    private System.Windows.Forms.DataGridViewTextBoxColumn colResRemitente;
    private System.Windows.Forms.DataGridViewTextBoxColumn colResDestinatario;
    private System.Windows.Forms.DataGridViewTextBoxColumn colResGuia;

    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
            components.Dispose();
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        components = new System.ComponentModel.Container();

        lblFechaEmision = new System.Windows.Forms.Label();
        dtpFechaEmision = new System.Windows.Forms.DateTimePicker();
        lblRemitente = new System.Windows.Forms.Label();
        cmbRemitente = new System.Windows.Forms.ComboBox();
        lblDestinatario = new System.Windows.Forms.Label();
        cmbDestinatario = new System.Windows.Forms.ComboBox();
        lblDestino = new System.Windows.Forms.Label();
        cmbDestino = new System.Windows.Forms.ComboBox();
        lblEstado = new System.Windows.Forms.Label();
        cmbEstado = new System.Windows.Forms.ComboBox();
        lblDirPartida = new System.Windows.Forms.Label();
        txtDirPartida = new System.Windows.Forms.TextBox();
        lblDirDestino = new System.Windows.Forms.Label();
        txtDirDestino = new System.Windows.Forms.TextBox();
        lblObservacion = new System.Windows.Forms.Label();
        txtObservacion = new System.Windows.Forms.TextBox();
        lblGuiaVinculada = new System.Windows.Forms.Label();

        dgvDetalles = new System.Windows.Forms.DataGridView();
        colCantidad = new System.Windows.Forms.DataGridViewTextBoxColumn();
        colDescripcion = new System.Windows.Forms.DataGridViewTextBoxColumn();
        colPeso = new System.Windows.Forms.DataGridViewTextBoxColumn();
        colUnidad = new System.Windows.Forms.DataGridViewTextBoxColumn();
        colCosto = new System.Windows.Forms.DataGridViewTextBoxColumn();
        colTipoDoc = new System.Windows.Forms.DataGridViewTextBoxColumn();
        colNroDoc = new System.Windows.Forms.DataGridViewTextBoxColumn();

        btnGrabar = new System.Windows.Forms.Button();
        btnEliminar = new System.Windows.Forms.Button();
        btnNuevo = new System.Windows.Forms.Button();
        btnBuscar = new System.Windows.Forms.Button();
        btnGenerarGuia = new System.Windows.Forms.Button();
        btnCancelar = new System.Windows.Forms.Button();

        dtpDesde = new System.Windows.Forms.DateTimePicker();
        dtpHasta = new System.Windows.Forms.DateTimePicker();
        dgvResultados = new System.Windows.Forms.DataGridView();
        colResCodig = new System.Windows.Forms.DataGridViewTextBoxColumn();
        colResFecha = new System.Windows.Forms.DataGridViewTextBoxColumn();
        colResRemitente = new System.Windows.Forms.DataGridViewTextBoxColumn();
        colResDestinatario = new System.Windows.Forms.DataGridViewTextBoxColumn();
        colResGuia = new System.Windows.Forms.DataGridViewTextBoxColumn();

        ((System.ComponentModel.ISupportInitialize)dgvDetalles).BeginInit();
        ((System.ComponentModel.ISupportInitialize)dgvResultados).BeginInit();
        this.SuspendLayout();

        // Configure detail grid columns
        colCantidad.HeaderText = "Cantidad";
        colCantidad.Name = "colCantidad";
        colCantidad.Width = 60;

        colDescripcion.HeaderText = "Descripcion";
        colDescripcion.Name = "colDescripcion";
        colDescripcion.Width = 180;

        colPeso.HeaderText = "Peso";
        colPeso.Name = "colPeso";
        colPeso.Width = 60;

        colUnidad.HeaderText = "Unidad";
        colUnidad.Name = "colUnidad";
        colUnidad.Width = 60;

        colCosto.HeaderText = "Costo";
        colCosto.Name = "colCosto";
        colCosto.Width = 80;

        colTipoDoc.HeaderText = "TipoDoc";
        colTipoDoc.Name = "colTipoDoc";
        colTipoDoc.Width = 80;

        colNroDoc.HeaderText = "NroDoc";
        colNroDoc.Name = "colNroDoc";
        colNroDoc.Width = 100;

        dgvDetalles.Columns.AddRange(colCantidad, colDescripcion, colPeso, colUnidad, colCosto, colTipoDoc, colNroDoc);
        dgvDetalles.Location = new System.Drawing.Point(12, 240);
        dgvDetalles.Name = "dgvDetalles";
        dgvDetalles.Size = new System.Drawing.Size(760, 160);

        // Result grid columns
        colResCodig.HeaderText = "Codigo";
        colResCodig.Name = "colResCodig";
        colResCodig.Width = 60;

        colResFecha.HeaderText = "Fecha";
        colResFecha.Name = "colResFecha";
        colResFecha.Width = 100;

        colResRemitente.HeaderText = "Remitente";
        colResRemitente.Name = "colResRemitente";
        colResRemitente.Width = 180;

        colResDestinatario.HeaderText = "Destinatario";
        colResDestinatario.Name = "colResDestinatario";
        colResDestinatario.Width = 180;

        colResGuia.HeaderText = "Guia Vinculada";
        colResGuia.Name = "colResGuia";
        colResGuia.Width = 120;

        dgvResultados.Columns.AddRange(colResCodig, colResFecha, colResRemitente, colResDestinatario, colResGuia);
        dgvResultados.Location = new System.Drawing.Point(12, 430);
        dgvResultados.Name = "dgvResultados";
        dgvResultados.Size = new System.Drawing.Size(760, 160);
        dgvResultados.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
        dgvResultados.CellDoubleClick += dgvResultados_CellDoubleClick;

        // Buttons
        btnGrabar.Location = new System.Drawing.Point(12, 600);
        btnGrabar.Name = "btnGrabar";
        btnGrabar.Size = new System.Drawing.Size(80, 28);
        btnGrabar.Text = "Grabar";
        btnGrabar.Click += btnGrabar_Click;

        btnEliminar.Location = new System.Drawing.Point(100, 600);
        btnEliminar.Name = "btnEliminar";
        btnEliminar.Size = new System.Drawing.Size(80, 28);
        btnEliminar.Text = "Eliminar";
        btnEliminar.Click += btnEliminar_Click;

        btnNuevo.Location = new System.Drawing.Point(188, 600);
        btnNuevo.Name = "btnNuevo";
        btnNuevo.Size = new System.Drawing.Size(80, 28);
        btnNuevo.Text = "Nuevo";
        btnNuevo.Click += btnNuevo_Click;

        btnBuscar.Location = new System.Drawing.Point(276, 600);
        btnBuscar.Name = "btnBuscar";
        btnBuscar.Size = new System.Drawing.Size(80, 28);
        btnBuscar.Text = "Buscar";
        btnBuscar.Click += btnBuscar_Click;

        btnGenerarGuia.Location = new System.Drawing.Point(364, 600);
        btnGenerarGuia.Name = "btnGenerarGuia";
        btnGenerarGuia.Size = new System.Drawing.Size(100, 28);
        btnGenerarGuia.Text = "Generar Guia";
        btnGenerarGuia.Enabled = false;
        btnGenerarGuia.Click += btnGenerarGuia_Click;

        btnCancelar.Location = new System.Drawing.Point(472, 600);
        btnCancelar.Name = "btnCancelar";
        btnCancelar.Size = new System.Drawing.Size(80, 28);
        btnCancelar.Text = "Cerrar";
        btnCancelar.Click += btnCancelar_Click;

        // Header fields
        lblFechaEmision.Location = new System.Drawing.Point(12, 16);
        lblFechaEmision.Text = "Fecha:";
        dtpFechaEmision.Location = new System.Drawing.Point(90, 12);
        dtpFechaEmision.Size = new System.Drawing.Size(120, 22);
        dtpFechaEmision.Format = System.Windows.Forms.DateTimePickerFormat.Short;

        lblRemitente.Location = new System.Drawing.Point(12, 46);
        lblRemitente.Text = "Remitente:";
        cmbRemitente.Location = new System.Drawing.Point(90, 42);
        cmbRemitente.Size = new System.Drawing.Size(200, 22);
        cmbRemitente.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;

        lblDestinatario.Location = new System.Drawing.Point(300, 46);
        lblDestinatario.Text = "Destinatario:";
        cmbDestinatario.Location = new System.Drawing.Point(380, 42);
        cmbDestinatario.Size = new System.Drawing.Size(200, 22);
        cmbDestinatario.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;

        lblDestino.Location = new System.Drawing.Point(12, 76);
        lblDestino.Text = "Destino:";
        cmbDestino.Location = new System.Drawing.Point(90, 72);
        cmbDestino.Size = new System.Drawing.Size(200, 22);
        cmbDestino.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;

        lblEstado.Location = new System.Drawing.Point(300, 76);
        lblEstado.Text = "Estado:";
        cmbEstado.Location = new System.Drawing.Point(380, 72);
        cmbEstado.Size = new System.Drawing.Size(200, 22);
        cmbEstado.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;

        lblDirPartida.Location = new System.Drawing.Point(12, 106);
        lblDirPartida.Text = "Dir. Partida:";
        txtDirPartida.Location = new System.Drawing.Point(90, 102);
        txtDirPartida.Size = new System.Drawing.Size(300, 22);

        lblDirDestino.Location = new System.Drawing.Point(12, 136);
        lblDirDestino.Text = "Dir. Destino:";
        txtDirDestino.Location = new System.Drawing.Point(90, 132);
        txtDirDestino.Size = new System.Drawing.Size(300, 22);

        lblObservacion.Location = new System.Drawing.Point(12, 166);
        lblObservacion.Text = "Observacion:";
        txtObservacion.Location = new System.Drawing.Point(90, 162);
        txtObservacion.Size = new System.Drawing.Size(500, 22);

        lblGuiaVinculada.Location = new System.Drawing.Point(12, 196);
        lblGuiaVinculada.Size = new System.Drawing.Size(500, 22);
        lblGuiaVinculada.Text = string.Empty;
        lblGuiaVinculada.ForeColor = System.Drawing.Color.DarkBlue;

        dtpDesde.Location = new System.Drawing.Point(12, 408);
        dtpDesde.Size = new System.Drawing.Size(120, 22);
        dtpDesde.Format = System.Windows.Forms.DateTimePickerFormat.Short;

        dtpHasta.Location = new System.Drawing.Point(140, 408);
        dtpHasta.Size = new System.Drawing.Size(120, 22);
        dtpHasta.Format = System.Windows.Forms.DateTimePickerFormat.Short;

        // Form
        this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        this.ClientSize = new System.Drawing.Size(800, 640);
        this.Controls.AddRange(new System.Windows.Forms.Control[]
        {
            lblFechaEmision, dtpFechaEmision,
            lblRemitente, cmbRemitente,
            lblDestinatario, cmbDestinatario,
            lblDestino, cmbDestino,
            lblEstado, cmbEstado,
            lblDirPartida, txtDirPartida,
            lblDirDestino, txtDirDestino,
            lblObservacion, txtObservacion,
            lblGuiaVinculada,
            dgvDetalles,
            dtpDesde, dtpHasta,
            dgvResultados,
            btnGrabar, btnEliminar, btnNuevo, btnBuscar, btnGenerarGuia, btnCancelar,
        });
        this.Name = "frmRecepcion";
        this.Text = "Recepcion de Carga";

        ((System.ComponentModel.ISupportInitialize)dgvDetalles).EndInit();
        ((System.ComponentModel.ISupportInitialize)dgvResultados).EndInit();
        this.ResumeLayout(false);
    }
}
