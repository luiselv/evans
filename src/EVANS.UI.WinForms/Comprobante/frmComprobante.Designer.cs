namespace EVANS.UI.WinForms.Comprobante;

partial class frmComprobante
{
    private System.ComponentModel.IContainer components = null;

    // Header — tipo + cliente
    private System.Windows.Forms.Label lblTipo;
    private System.Windows.Forms.ComboBox cmbTipoComprobante;
    private System.Windows.Forms.Label lblCliente;
    private System.Windows.Forms.ComboBox cmbCliente;

    // Header — RUC / DNI
    private System.Windows.Forms.Label lblRucDni;
    private System.Windows.Forms.TextBox txtRucDni;

    // Header — dirección
    private System.Windows.Forms.Label lblDireccion;
    private System.Windows.Forms.TextBox txtDireccion;

    // Header — total
    private System.Windows.Forms.Label lblTotal;
    private System.Windows.Forms.TextBox txtTotal;

    // GuiaRef display (read-only when opened from a Guia context)
    private System.Windows.Forms.Label lblGuiaRef;

    // NumeroComprobante display after save
    private System.Windows.Forms.Label lblNumero;

    // Details grid
    private System.Windows.Forms.DataGridView dgvDetalles;
    private System.Windows.Forms.DataGridViewTextBoxColumn colCantidad;
    private System.Windows.Forms.DataGridViewTextBoxColumn colDescripcion;
    private System.Windows.Forms.DataGridViewTextBoxColumn colPrecioUnitario;
    private System.Windows.Forms.DataGridViewTextBoxColumn colFlete;

    // Buttons
    private System.Windows.Forms.Button btnGuardar;
    private System.Windows.Forms.Button btnImprimir;
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
        lblTipo       = new System.Windows.Forms.Label();
        lblCliente    = new System.Windows.Forms.Label();
        lblRucDni     = new System.Windows.Forms.Label();
        lblDireccion  = new System.Windows.Forms.Label();
        lblTotal      = new System.Windows.Forms.Label();
        lblGuiaRef    = new System.Windows.Forms.Label();
        lblNumero     = new System.Windows.Forms.Label();

        // --- Combos ---
        cmbTipoComprobante = new System.Windows.Forms.ComboBox();
        cmbCliente         = new System.Windows.Forms.ComboBox();

        // --- TextBoxes ---
        txtRucDni    = new System.Windows.Forms.TextBox();
        txtDireccion = new System.Windows.Forms.TextBox();
        txtTotal     = new System.Windows.Forms.TextBox();

        // --- Grid ---
        dgvDetalles        = new System.Windows.Forms.DataGridView();
        colCantidad        = new System.Windows.Forms.DataGridViewTextBoxColumn();
        colDescripcion     = new System.Windows.Forms.DataGridViewTextBoxColumn();
        colPrecioUnitario  = new System.Windows.Forms.DataGridViewTextBoxColumn();
        colFlete           = new System.Windows.Forms.DataGridViewTextBoxColumn();

        // --- Buttons ---
        btnGuardar  = new System.Windows.Forms.Button();
        btnImprimir = new System.Windows.Forms.Button();
        btnCancelar = new System.Windows.Forms.Button();

        ((System.ComponentModel.ISupportInitialize)dgvDetalles).BeginInit();
        this.SuspendLayout();

        // --- lblTipo ---
        lblTipo.AutoSize = true;
        lblTipo.Location = new System.Drawing.Point(12, 15);
        lblTipo.Name     = "lblTipo";
        lblTipo.Text     = "Tipo:";
        lblTipo.TabIndex = 0;

        // --- cmbTipoComprobante ---
        cmbTipoComprobante.DropDownStyle      = System.Windows.Forms.ComboBoxStyle.DropDownList;
        cmbTipoComprobante.FormattingEnabled  = true;
        cmbTipoComprobante.Location           = new System.Drawing.Point(130, 12);
        cmbTipoComprobante.Name               = "cmbTipoComprobante";
        cmbTipoComprobante.Size               = new System.Drawing.Size(160, 23);
        cmbTipoComprobante.TabIndex           = 1;
        cmbTipoComprobante.SelectedIndexChanged += cmbTipoComprobante_SelectedIndexChanged;

        // --- lblCliente ---
        lblCliente.AutoSize = true;
        lblCliente.Location = new System.Drawing.Point(12, 48);
        lblCliente.Name     = "lblCliente";
        lblCliente.Text     = "Cliente:";
        lblCliente.TabIndex = 2;

        // --- cmbCliente ---
        cmbCliente.DropDownStyle     = System.Windows.Forms.ComboBoxStyle.DropDownList;
        cmbCliente.FormattingEnabled = true;
        cmbCliente.Location          = new System.Drawing.Point(130, 45);
        cmbCliente.Name              = "cmbCliente";
        cmbCliente.Size              = new System.Drawing.Size(280, 23);
        cmbCliente.TabIndex          = 3;

        // --- lblRucDni ---
        lblRucDni.AutoSize = true;
        lblRucDni.Location = new System.Drawing.Point(12, 81);
        lblRucDni.Name     = "lblRucDni";
        lblRucDni.Text     = "RUC:";
        lblRucDni.TabIndex = 4;

        // --- txtRucDni ---
        txtRucDni.Location  = new System.Drawing.Point(130, 78);
        txtRucDni.MaxLength = 15;
        txtRucDni.Name      = "txtRucDni";
        txtRucDni.Size      = new System.Drawing.Size(200, 23);
        txtRucDni.TabIndex  = 5;

        // --- lblDireccion ---
        lblDireccion.AutoSize = true;
        lblDireccion.Location = new System.Drawing.Point(12, 114);
        lblDireccion.Name     = "lblDireccion";
        lblDireccion.Text     = "Dirección:";
        lblDireccion.TabIndex = 6;

        // --- txtDireccion ---
        txtDireccion.Location  = new System.Drawing.Point(130, 111);
        txtDireccion.Name      = "txtDireccion";
        txtDireccion.Size      = new System.Drawing.Size(400, 23);
        txtDireccion.TabIndex  = 7;

        // --- lblTotal ---
        lblTotal.AutoSize = true;
        lblTotal.Location = new System.Drawing.Point(12, 147);
        lblTotal.Name     = "lblTotal";
        lblTotal.Text     = "Total (S/.):";
        lblTotal.TabIndex = 8;

        // --- txtTotal ---
        txtTotal.Location  = new System.Drawing.Point(130, 144);
        txtTotal.Name      = "txtTotal";
        txtTotal.Size      = new System.Drawing.Size(120, 23);
        txtTotal.TabIndex  = 9;
        txtTotal.Text      = "0.00";

        // --- lblGuiaRef ---
        lblGuiaRef.AutoSize = true;
        lblGuiaRef.Location = new System.Drawing.Point(420, 15);
        lblGuiaRef.Name     = "lblGuiaRef";
        lblGuiaRef.Size     = new System.Drawing.Size(100, 15);
        lblGuiaRef.TabIndex = 10;
        lblGuiaRef.Visible  = false;

        // --- lblNumero ---
        lblNumero.AutoSize = true;
        lblNumero.Font     = new System.Drawing.Font(SystemFonts.DefaultFont, System.Drawing.FontStyle.Bold);
        lblNumero.Location = new System.Drawing.Point(420, 48);
        lblNumero.Name     = "lblNumero";
        lblNumero.Size     = new System.Drawing.Size(200, 15);
        lblNumero.TabIndex = 11;
        lblNumero.Text     = string.Empty;

        // --- dgvDetalles ---
        dgvDetalles.AllowUserToAddRows    = true;
        dgvDetalles.AllowUserToDeleteRows = true;
        dgvDetalles.Anchor = System.Windows.Forms.AnchorStyles.Top
            | System.Windows.Forms.AnchorStyles.Bottom
            | System.Windows.Forms.AnchorStyles.Left
            | System.Windows.Forms.AnchorStyles.Right;
        dgvDetalles.ColumnHeadersHeightSizeMode =
            System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        dgvDetalles.Columns.AddRange(colCantidad, colDescripcion, colPrecioUnitario, colFlete);
        dgvDetalles.Location  = new System.Drawing.Point(12, 180);
        dgvDetalles.Name      = "dgvDetalles";
        dgvDetalles.Size      = new System.Drawing.Size(760, 230);
        dgvDetalles.TabIndex  = 12;

        // --- colCantidad ---
        colCantidad.HeaderText = "Cantidad";
        colCantidad.Name       = "colCantidad";
        colCantidad.Width      = 70;

        // --- colDescripcion ---
        colDescripcion.HeaderText = "Descripción";
        colDescripcion.Name       = "colDescripcion";
        colDescripcion.Width      = 280;

        // --- colPrecioUnitario ---
        colPrecioUnitario.HeaderText = "Precio Unit.";
        colPrecioUnitario.Name       = "colPrecioUnitario";
        colPrecioUnitario.Width      = 110;

        // --- colFlete ---
        colFlete.HeaderText = "Flete";
        colFlete.Name       = "colFlete";
        colFlete.Width      = 110;

        // --- btnGuardar ---
        btnGuardar.Anchor   = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
        btnGuardar.Location = new System.Drawing.Point(535, 422);
        btnGuardar.Name     = "btnGuardar";
        btnGuardar.Size     = new System.Drawing.Size(75, 23);
        btnGuardar.TabIndex = 13;
        btnGuardar.Text     = "Guardar";
        btnGuardar.UseVisualStyleBackColor = true;
        btnGuardar.Click += btnGuardar_Click;

        // --- btnImprimir ---
        btnImprimir.Anchor   = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
        btnImprimir.Enabled  = false;   // enabled after a successful save
        btnImprimir.Location = new System.Drawing.Point(616, 422);
        btnImprimir.Name     = "btnImprimir";
        btnImprimir.Size     = new System.Drawing.Size(75, 23);
        btnImprimir.TabIndex = 14;
        btnImprimir.Text     = "Imprimir";
        btnImprimir.UseVisualStyleBackColor = true;
        btnImprimir.Click += btnImprimir_Click;

        // --- btnCancelar ---
        btnCancelar.Anchor   = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
        btnCancelar.Location = new System.Drawing.Point(697, 422);
        btnCancelar.Name     = "btnCancelar";
        btnCancelar.Size     = new System.Drawing.Size(75, 23);
        btnCancelar.TabIndex = 15;
        btnCancelar.Text     = "Cancelar";
        btnCancelar.UseVisualStyleBackColor = true;
        btnCancelar.Click += btnCancelar_Click;

        // --- frmComprobante ---
        this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
        this.AutoScaleMode       = System.Windows.Forms.AutoScaleMode.Font;
        this.ClientSize          = new System.Drawing.Size(784, 457);
        this.Controls.Add(lblTipo);
        this.Controls.Add(cmbTipoComprobante);
        this.Controls.Add(lblCliente);
        this.Controls.Add(cmbCliente);
        this.Controls.Add(lblRucDni);
        this.Controls.Add(txtRucDni);
        this.Controls.Add(lblDireccion);
        this.Controls.Add(txtDireccion);
        this.Controls.Add(lblTotal);
        this.Controls.Add(txtTotal);
        this.Controls.Add(lblGuiaRef);
        this.Controls.Add(lblNumero);
        this.Controls.Add(dgvDetalles);
        this.Controls.Add(btnGuardar);
        this.Controls.Add(btnImprimir);
        this.Controls.Add(btnCancelar);
        this.MinimumSize = new System.Drawing.Size(800, 496);
        this.Name        = "frmComprobante";
        this.Text        = "Comprobante";

        ((System.ComponentModel.ISupportInitialize)dgvDetalles).EndInit();
        this.ResumeLayout(false);
        this.PerformLayout();
    }
}
