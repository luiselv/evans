namespace EVANS.UI.WinForms.Manifiesto;

partial class frmManifiesto
{
    private System.ComponentModel.IContainer components = null;

    // Header — date
    private System.Windows.Forms.Label lblFecha;
    private System.Windows.Forms.DateTimePicker dtpFecha;

    // Header — transportista
    private System.Windows.Forms.Label lblTransportista;
    private System.Windows.Forms.ComboBox cmbTransportista;

    // Header — vehiculo
    private System.Windows.Forms.Label lblVehiculo;
    private System.Windows.Forms.ComboBox cmbVehiculo;

    // Header — carreta
    private System.Windows.Forms.Label lblCarreta;
    private System.Windows.Forms.ComboBox cmbCarreta;

    // Header — chofer
    private System.Windows.Forms.Label lblChofer;
    private System.Windows.Forms.ComboBox cmbChofer;

    // Header — estado
    private System.Windows.Forms.Label lblEstado;
    private System.Windows.Forms.ComboBox cmbEstado;

    // Header — importe
    private System.Windows.Forms.Label lblImporte;
    private System.Windows.Forms.TextBox txtImporte;

    // Header — peso
    private System.Windows.Forms.Label lblPeso;
    private System.Windows.Forms.TextBox txtPeso;

    // Numero display after save
    private System.Windows.Forms.Label lblNumero;

    // Guia IDs input (comma-separated)
    private System.Windows.Forms.Label lblGuiaIds;
    private System.Windows.Forms.TextBox txtGuiaIds;

    // Search panel
    private System.Windows.Forms.Label lblBuscar;
    private System.Windows.Forms.TextBox txtBuscar;
    private System.Windows.Forms.Button btnBuscar;

    // Results grid
    private System.Windows.Forms.DataGridView dgvManifiestos;
    private System.Windows.Forms.DataGridViewTextBoxColumn colCodigo;
    private System.Windows.Forms.DataGridViewTextBoxColumn colNumero;
    private System.Windows.Forms.DataGridViewTextBoxColumn colFecha;
    private System.Windows.Forms.DataGridViewTextBoxColumn colTransportista;
    private System.Windows.Forms.DataGridViewTextBoxColumn colEstado;

    // Buttons
    private System.Windows.Forms.Button btnGuardar;
    private System.Windows.Forms.Button btnEliminar;
    private System.Windows.Forms.Button btnImprimir;
    private System.Windows.Forms.Button btnNuevo;
    private System.Windows.Forms.Button btnCancelar;

    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
            components.Dispose();
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        components = new System.ComponentModel.Container();

        // --- Labels ---
        lblFecha         = new System.Windows.Forms.Label();
        lblTransportista = new System.Windows.Forms.Label();
        lblVehiculo      = new System.Windows.Forms.Label();
        lblCarreta       = new System.Windows.Forms.Label();
        lblChofer        = new System.Windows.Forms.Label();
        lblEstado        = new System.Windows.Forms.Label();
        lblImporte       = new System.Windows.Forms.Label();
        lblPeso          = new System.Windows.Forms.Label();
        lblNumero        = new System.Windows.Forms.Label();
        lblGuiaIds       = new System.Windows.Forms.Label();
        lblBuscar        = new System.Windows.Forms.Label();

        // --- Date picker ---
        dtpFecha = new System.Windows.Forms.DateTimePicker();

        // --- Combos ---
        cmbTransportista = new System.Windows.Forms.ComboBox();
        cmbVehiculo      = new System.Windows.Forms.ComboBox();
        cmbCarreta       = new System.Windows.Forms.ComboBox();
        cmbChofer        = new System.Windows.Forms.ComboBox();
        cmbEstado        = new System.Windows.Forms.ComboBox();

        // --- TextBoxes ---
        txtImporte = new System.Windows.Forms.TextBox();
        txtPeso    = new System.Windows.Forms.TextBox();
        txtGuiaIds = new System.Windows.Forms.TextBox();
        txtBuscar  = new System.Windows.Forms.TextBox();

        // --- Grid ---
        dgvManifiestos   = new System.Windows.Forms.DataGridView();
        colCodigo        = new System.Windows.Forms.DataGridViewTextBoxColumn();
        colNumero        = new System.Windows.Forms.DataGridViewTextBoxColumn();
        colFecha         = new System.Windows.Forms.DataGridViewTextBoxColumn();
        colTransportista = new System.Windows.Forms.DataGridViewTextBoxColumn();
        colEstado        = new System.Windows.Forms.DataGridViewTextBoxColumn();

        // --- Buttons ---
        btnGuardar  = new System.Windows.Forms.Button();
        btnEliminar = new System.Windows.Forms.Button();
        btnImprimir = new System.Windows.Forms.Button();
        btnNuevo    = new System.Windows.Forms.Button();
        btnCancelar = new System.Windows.Forms.Button();
        btnBuscar   = new System.Windows.Forms.Button();

        ((System.ComponentModel.ISupportInitialize)dgvManifiestos).BeginInit();
        this.SuspendLayout();

        // --- lblFecha ---
        lblFecha.AutoSize = true;
        lblFecha.Location = new System.Drawing.Point(12, 15);
        lblFecha.Name     = "lblFecha";
        lblFecha.Text     = "Fecha:";
        lblFecha.TabIndex = 0;

        // --- dtpFecha ---
        dtpFecha.Format   = System.Windows.Forms.DateTimePickerFormat.Short;
        dtpFecha.Location = new System.Drawing.Point(140, 12);
        dtpFecha.Name     = "dtpFecha";
        dtpFecha.Size     = new System.Drawing.Size(140, 23);
        dtpFecha.TabIndex = 1;

        // --- lblTransportista ---
        lblTransportista.AutoSize = true;
        lblTransportista.Location = new System.Drawing.Point(12, 48);
        lblTransportista.Name     = "lblTransportista";
        lblTransportista.Text     = "Transportista:";
        lblTransportista.TabIndex = 2;

        // --- cmbTransportista ---
        cmbTransportista.DropDownStyle     = System.Windows.Forms.ComboBoxStyle.DropDownList;
        cmbTransportista.FormattingEnabled = true;
        cmbTransportista.Location          = new System.Drawing.Point(140, 45);
        cmbTransportista.Name              = "cmbTransportista";
        cmbTransportista.Size              = new System.Drawing.Size(240, 23);
        cmbTransportista.TabIndex          = 3;

        // --- lblVehiculo ---
        lblVehiculo.AutoSize = true;
        lblVehiculo.Location = new System.Drawing.Point(12, 81);
        lblVehiculo.Name     = "lblVehiculo";
        lblVehiculo.Text     = "Vehículo:";
        lblVehiculo.TabIndex = 4;

        // --- cmbVehiculo ---
        cmbVehiculo.DropDownStyle     = System.Windows.Forms.ComboBoxStyle.DropDownList;
        cmbVehiculo.FormattingEnabled = true;
        cmbVehiculo.Location          = new System.Drawing.Point(140, 78);
        cmbVehiculo.Name              = "cmbVehiculo";
        cmbVehiculo.Size              = new System.Drawing.Size(160, 23);
        cmbVehiculo.TabIndex          = 5;

        // --- lblCarreta ---
        lblCarreta.AutoSize = true;
        lblCarreta.Location = new System.Drawing.Point(320, 81);
        lblCarreta.Name     = "lblCarreta";
        lblCarreta.Text     = "Carreta:";
        lblCarreta.TabIndex = 6;

        // --- cmbCarreta ---
        cmbCarreta.DropDownStyle     = System.Windows.Forms.ComboBoxStyle.DropDownList;
        cmbCarreta.FormattingEnabled = true;
        cmbCarreta.Location          = new System.Drawing.Point(400, 78);
        cmbCarreta.Name              = "cmbCarreta";
        cmbCarreta.Size              = new System.Drawing.Size(140, 23);
        cmbCarreta.TabIndex          = 7;

        // --- lblChofer ---
        lblChofer.AutoSize = true;
        lblChofer.Location = new System.Drawing.Point(12, 114);
        lblChofer.Name     = "lblChofer";
        lblChofer.Text     = "Chofer:";
        lblChofer.TabIndex = 8;

        // --- cmbChofer ---
        cmbChofer.DropDownStyle     = System.Windows.Forms.ComboBoxStyle.DropDownList;
        cmbChofer.FormattingEnabled = true;
        cmbChofer.Location          = new System.Drawing.Point(140, 111);
        cmbChofer.Name              = "cmbChofer";
        cmbChofer.Size              = new System.Drawing.Size(240, 23);
        cmbChofer.TabIndex          = 9;

        // --- lblEstado ---
        lblEstado.AutoSize = true;
        lblEstado.Location = new System.Drawing.Point(400, 114);
        lblEstado.Name     = "lblEstado";
        lblEstado.Text     = "Estado:";
        lblEstado.TabIndex = 10;

        // --- cmbEstado ---
        cmbEstado.DropDownStyle     = System.Windows.Forms.ComboBoxStyle.DropDownList;
        cmbEstado.FormattingEnabled = true;
        cmbEstado.Location          = new System.Drawing.Point(460, 111);
        cmbEstado.Name              = "cmbEstado";
        cmbEstado.Size              = new System.Drawing.Size(140, 23);
        cmbEstado.TabIndex          = 11;

        // --- lblImporte ---
        lblImporte.AutoSize = true;
        lblImporte.Location = new System.Drawing.Point(12, 147);
        lblImporte.Name     = "lblImporte";
        lblImporte.Text     = "Importe (S/.):";
        lblImporte.TabIndex = 12;

        // --- txtImporte ---
        txtImporte.Location  = new System.Drawing.Point(140, 144);
        txtImporte.Name      = "txtImporte";
        txtImporte.Size      = new System.Drawing.Size(100, 23);
        txtImporte.TabIndex  = 13;
        txtImporte.Text      = "0.00";

        // --- lblPeso ---
        lblPeso.AutoSize = true;
        lblPeso.Location = new System.Drawing.Point(260, 147);
        lblPeso.Name     = "lblPeso";
        lblPeso.Text     = "Peso (kg):";
        lblPeso.TabIndex = 14;

        // --- txtPeso ---
        txtPeso.Location  = new System.Drawing.Point(350, 144);
        txtPeso.Name      = "txtPeso";
        txtPeso.Size      = new System.Drawing.Size(100, 23);
        txtPeso.TabIndex  = 15;
        txtPeso.Text      = "0.00";

        // --- lblNumero ---
        lblNumero.AutoSize = true;
        lblNumero.Font     = new System.Drawing.Font(SystemFonts.DefaultFont, System.Drawing.FontStyle.Bold);
        lblNumero.Location = new System.Drawing.Point(620, 15);
        lblNumero.Name     = "lblNumero";
        lblNumero.Size     = new System.Drawing.Size(160, 15);
        lblNumero.TabIndex = 16;
        lblNumero.Text     = string.Empty;

        // --- lblGuiaIds ---
        lblGuiaIds.AutoSize = true;
        lblGuiaIds.Location = new System.Drawing.Point(12, 180);
        lblGuiaIds.Name     = "lblGuiaIds";
        lblGuiaIds.Text     = "Guías (IDs, comma-separated):";
        lblGuiaIds.TabIndex = 17;

        // --- txtGuiaIds ---
        txtGuiaIds.Location  = new System.Drawing.Point(220, 177);
        txtGuiaIds.Name      = "txtGuiaIds";
        txtGuiaIds.Size      = new System.Drawing.Size(360, 23);
        txtGuiaIds.TabIndex  = 18;

        // --- lblBuscar ---
        lblBuscar.AutoSize = true;
        lblBuscar.Location = new System.Drawing.Point(12, 213);
        lblBuscar.Name     = "lblBuscar";
        lblBuscar.Text     = "Buscar:";
        lblBuscar.TabIndex = 19;

        // --- txtBuscar ---
        txtBuscar.Location  = new System.Drawing.Point(80, 210);
        txtBuscar.Name      = "txtBuscar";
        txtBuscar.Size      = new System.Drawing.Size(240, 23);
        txtBuscar.TabIndex  = 20;

        // --- btnBuscar ---
        btnBuscar.Location = new System.Drawing.Point(332, 209);
        btnBuscar.Name     = "btnBuscar";
        btnBuscar.Size     = new System.Drawing.Size(75, 23);
        btnBuscar.TabIndex = 21;
        btnBuscar.Text     = "Buscar";
        btnBuscar.UseVisualStyleBackColor = true;
        btnBuscar.Click += btnBuscar_Click;

        // --- dgvManifiestos ---
        dgvManifiestos.AllowUserToAddRows    = false;
        dgvManifiestos.AllowUserToDeleteRows = false;
        dgvManifiestos.Anchor = System.Windows.Forms.AnchorStyles.Top
            | System.Windows.Forms.AnchorStyles.Bottom
            | System.Windows.Forms.AnchorStyles.Left
            | System.Windows.Forms.AnchorStyles.Right;
        dgvManifiestos.ColumnHeadersHeightSizeMode =
            System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        dgvManifiestos.Columns.AddRange(colCodigo, colNumero, colFecha, colTransportista, colEstado);
        dgvManifiestos.Location         = new System.Drawing.Point(12, 245);
        dgvManifiestos.MultiSelect       = false;
        dgvManifiestos.Name              = "dgvManifiestos";
        dgvManifiestos.ReadOnly          = true;
        dgvManifiestos.SelectionMode     = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
        dgvManifiestos.Size              = new System.Drawing.Size(760, 180);
        dgvManifiestos.TabIndex          = 22;
        dgvManifiestos.SelectionChanged += dgvManifiestos_SelectionChanged;

        // --- colCodigo ---
        colCodigo.DataPropertyName = "Codigo";
        colCodigo.HeaderText       = "Código";
        colCodigo.Name             = "colCodigo";
        colCodigo.Width            = 60;

        // --- colNumero ---
        colNumero.DataPropertyName = "Numero";
        colNumero.HeaderText       = "Número";
        colNumero.Name             = "colNumero";
        colNumero.Width            = 100;

        // --- colFecha ---
        colFecha.DataPropertyName = "FechaTraslado";
        colFecha.HeaderText       = "Fecha";
        colFecha.Name             = "colFecha";
        colFecha.Width            = 90;

        // --- colTransportista ---
        colTransportista.DataPropertyName = "TransportistaNombre";
        colTransportista.HeaderText       = "Transportista";
        colTransportista.Name             = "colTransportista";
        colTransportista.Width            = 200;

        // --- colEstado ---
        colEstado.DataPropertyName = "EstadoNombre";
        colEstado.HeaderText       = "Estado";
        colEstado.Name             = "colEstado";
        colEstado.Width            = 100;

        // --- btnNuevo ---
        btnNuevo.Anchor   = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
        btnNuevo.Location = new System.Drawing.Point(376, 438);
        btnNuevo.Name     = "btnNuevo";
        btnNuevo.Size     = new System.Drawing.Size(75, 23);
        btnNuevo.TabIndex = 23;
        btnNuevo.Text     = "Nuevo";
        btnNuevo.UseVisualStyleBackColor = true;
        btnNuevo.Click += btnNuevo_Click;

        // --- btnGuardar ---
        btnGuardar.Anchor   = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
        btnGuardar.Location = new System.Drawing.Point(457, 438);
        btnGuardar.Name     = "btnGuardar";
        btnGuardar.Size     = new System.Drawing.Size(75, 23);
        btnGuardar.TabIndex = 24;
        btnGuardar.Text     = "Guardar";
        btnGuardar.UseVisualStyleBackColor = true;
        btnGuardar.Click += btnGuardar_Click;

        // --- btnEliminar ---
        btnEliminar.Anchor   = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
        btnEliminar.Enabled  = false;
        btnEliminar.Location = new System.Drawing.Point(538, 438);
        btnEliminar.Name     = "btnEliminar";
        btnEliminar.Size     = new System.Drawing.Size(75, 23);
        btnEliminar.TabIndex = 25;
        btnEliminar.Text     = "Eliminar";
        btnEliminar.UseVisualStyleBackColor = true;
        btnEliminar.Click += btnEliminar_Click;

        // --- btnImprimir ---
        btnImprimir.Anchor   = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
        btnImprimir.Enabled  = false;
        btnImprimir.Location = new System.Drawing.Point(619, 438);
        btnImprimir.Name     = "btnImprimir";
        btnImprimir.Size     = new System.Drawing.Size(75, 23);
        btnImprimir.TabIndex = 26;
        btnImprimir.Text     = "Imprimir";
        btnImprimir.UseVisualStyleBackColor = true;
        btnImprimir.Click += btnImprimir_Click;

        // --- btnCancelar ---
        btnCancelar.Anchor   = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
        btnCancelar.Location = new System.Drawing.Point(700, 438);
        btnCancelar.Name     = "btnCancelar";
        btnCancelar.Size     = new System.Drawing.Size(75, 23);
        btnCancelar.TabIndex = 27;
        btnCancelar.Text     = "Cerrar";
        btnCancelar.UseVisualStyleBackColor = true;
        btnCancelar.Click += btnCancelar_Click;

        // --- frmManifiesto ---
        this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
        this.AutoScaleMode       = System.Windows.Forms.AutoScaleMode.Font;
        this.ClientSize          = new System.Drawing.Size(800, 473);
        this.Controls.Add(lblFecha);
        this.Controls.Add(dtpFecha);
        this.Controls.Add(lblTransportista);
        this.Controls.Add(cmbTransportista);
        this.Controls.Add(lblVehiculo);
        this.Controls.Add(cmbVehiculo);
        this.Controls.Add(lblCarreta);
        this.Controls.Add(cmbCarreta);
        this.Controls.Add(lblChofer);
        this.Controls.Add(cmbChofer);
        this.Controls.Add(lblEstado);
        this.Controls.Add(cmbEstado);
        this.Controls.Add(lblImporte);
        this.Controls.Add(txtImporte);
        this.Controls.Add(lblPeso);
        this.Controls.Add(txtPeso);
        this.Controls.Add(lblNumero);
        this.Controls.Add(lblGuiaIds);
        this.Controls.Add(txtGuiaIds);
        this.Controls.Add(lblBuscar);
        this.Controls.Add(txtBuscar);
        this.Controls.Add(btnBuscar);
        this.Controls.Add(dgvManifiestos);
        this.Controls.Add(btnNuevo);
        this.Controls.Add(btnGuardar);
        this.Controls.Add(btnEliminar);
        this.Controls.Add(btnImprimir);
        this.Controls.Add(btnCancelar);
        this.MinimumSize = new System.Drawing.Size(820, 512);
        this.Name        = "frmManifiesto";
        this.Text        = "Manifiestos";

        ((System.ComponentModel.ISupportInitialize)dgvManifiestos).EndInit();
        this.ResumeLayout(false);
        this.PerformLayout();
    }
}
