namespace EVANS.UI.WinForms.GuiaRemision;

partial class frmGuiaRemision
{
    private System.ComponentModel.IContainer components = null;

    // Combos — clients
    private System.Windows.Forms.Label lblRemitente;
    private System.Windows.Forms.ComboBox cmbRemitente;
    private System.Windows.Forms.Label lblDestinatario;
    private System.Windows.Forms.ComboBox cmbDestinatario;

    // Combos — logistics
    private System.Windows.Forms.Label lblVehiculo;
    private System.Windows.Forms.ComboBox cmbVehiculo;
    private System.Windows.Forms.Label lblCarreta;
    private System.Windows.Forms.ComboBox cmbCarreta;
    private System.Windows.Forms.Label lblChofer;
    private System.Windows.Forms.ComboBox cmbChofer;

    // Misc
    private System.Windows.Forms.CheckBox chkHasManifest;
    private System.Windows.Forms.Label lblIgv;

    // Details grid
    private System.Windows.Forms.DataGridView dgvDetalles;
    private System.Windows.Forms.DataGridViewTextBoxColumn colDescripcion;
    private System.Windows.Forms.DataGridViewTextBoxColumn colPeso;
    private System.Windows.Forms.DataGridViewTextBoxColumn colPrecioUnitario;
    private System.Windows.Forms.DataGridViewTextBoxColumn colPrecioTotal;
    private System.Windows.Forms.DataGridViewTextBoxColumn colCantidad;

    // Buttons
    private System.Windows.Forms.Button btnGrabar;
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
        lblRemitente = new System.Windows.Forms.Label();
        lblDestinatario = new System.Windows.Forms.Label();
        lblVehiculo = new System.Windows.Forms.Label();
        lblCarreta = new System.Windows.Forms.Label();
        lblChofer = new System.Windows.Forms.Label();
        lblIgv = new System.Windows.Forms.Label();

        // --- Combos ---
        cmbRemitente = new System.Windows.Forms.ComboBox();
        cmbDestinatario = new System.Windows.Forms.ComboBox();
        cmbVehiculo = new System.Windows.Forms.ComboBox();
        cmbCarreta = new System.Windows.Forms.ComboBox();
        cmbChofer = new System.Windows.Forms.ComboBox();

        // --- Misc ---
        chkHasManifest = new System.Windows.Forms.CheckBox();

        // --- Grid ---
        dgvDetalles = new System.Windows.Forms.DataGridView();
        colDescripcion = new System.Windows.Forms.DataGridViewTextBoxColumn();
        colPeso = new System.Windows.Forms.DataGridViewTextBoxColumn();
        colPrecioUnitario = new System.Windows.Forms.DataGridViewTextBoxColumn();
        colPrecioTotal = new System.Windows.Forms.DataGridViewTextBoxColumn();
        colCantidad = new System.Windows.Forms.DataGridViewTextBoxColumn();

        // --- Buttons ---
        btnGrabar = new System.Windows.Forms.Button();
        btnCancelar = new System.Windows.Forms.Button();

        ((System.ComponentModel.ISupportInitialize)dgvDetalles).BeginInit();
        this.SuspendLayout();

        // --- lblRemitente ---
        lblRemitente.AutoSize = true;
        lblRemitente.Location = new System.Drawing.Point(12, 15);
        lblRemitente.Name = "lblRemitente";
        lblRemitente.Size = new System.Drawing.Size(63, 15);
        lblRemitente.TabIndex = 0;
        lblRemitente.Text = "Remitente:";

        // --- cmbRemitente ---
        cmbRemitente.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
        cmbRemitente.FormattingEnabled = true;
        cmbRemitente.Location = new System.Drawing.Point(130, 12);
        cmbRemitente.Name = "cmbRemitente";
        cmbRemitente.Size = new System.Drawing.Size(280, 23);
        cmbRemitente.TabIndex = 1;

        // --- lblDestinatario ---
        lblDestinatario.AutoSize = true;
        lblDestinatario.Location = new System.Drawing.Point(12, 48);
        lblDestinatario.Name = "lblDestinatario";
        lblDestinatario.Size = new System.Drawing.Size(72, 15);
        lblDestinatario.TabIndex = 2;
        lblDestinatario.Text = "Destinatario:";

        // --- cmbDestinatario ---
        cmbDestinatario.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
        cmbDestinatario.FormattingEnabled = true;
        cmbDestinatario.Location = new System.Drawing.Point(130, 45);
        cmbDestinatario.Name = "cmbDestinatario";
        cmbDestinatario.Size = new System.Drawing.Size(280, 23);
        cmbDestinatario.TabIndex = 3;

        // --- lblVehiculo ---
        lblVehiculo.AutoSize = true;
        lblVehiculo.Location = new System.Drawing.Point(12, 81);
        lblVehiculo.Name = "lblVehiculo";
        lblVehiculo.Size = new System.Drawing.Size(57, 15);
        lblVehiculo.TabIndex = 4;
        lblVehiculo.Text = "Vehículo:";

        // --- cmbVehiculo ---
        cmbVehiculo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
        cmbVehiculo.FormattingEnabled = true;
        cmbVehiculo.Location = new System.Drawing.Point(130, 78);
        cmbVehiculo.Name = "cmbVehiculo";
        cmbVehiculo.Size = new System.Drawing.Size(200, 23);
        cmbVehiculo.TabIndex = 5;

        // --- lblCarreta ---
        lblCarreta.AutoSize = true;
        lblCarreta.Location = new System.Drawing.Point(12, 114);
        lblCarreta.Name = "lblCarreta";
        lblCarreta.Size = new System.Drawing.Size(48, 15);
        lblCarreta.TabIndex = 6;
        lblCarreta.Text = "Carreta:";

        // --- cmbCarreta ---
        cmbCarreta.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
        cmbCarreta.FormattingEnabled = true;
        cmbCarreta.Location = new System.Drawing.Point(130, 111);
        cmbCarreta.Name = "cmbCarreta";
        cmbCarreta.Size = new System.Drawing.Size(200, 23);
        cmbCarreta.TabIndex = 7;

        // --- lblChofer ---
        lblChofer.AutoSize = true;
        lblChofer.Location = new System.Drawing.Point(12, 147);
        lblChofer.Name = "lblChofer";
        lblChofer.Size = new System.Drawing.Size(44, 15);
        lblChofer.TabIndex = 8;
        lblChofer.Text = "Chofer:";

        // --- cmbChofer ---
        cmbChofer.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
        cmbChofer.FormattingEnabled = true;
        cmbChofer.Location = new System.Drawing.Point(130, 144);
        cmbChofer.Name = "cmbChofer";
        cmbChofer.Size = new System.Drawing.Size(200, 23);
        cmbChofer.TabIndex = 9;

        // --- chkHasManifest ---
        chkHasManifest.AutoSize = true;
        chkHasManifest.Location = new System.Drawing.Point(12, 180);
        chkHasManifest.Name = "chkHasManifest";
        chkHasManifest.Size = new System.Drawing.Size(120, 19);
        chkHasManifest.TabIndex = 10;
        chkHasManifest.Text = "Tiene Manifiesto";
        chkHasManifest.UseVisualStyleBackColor = true;

        // --- lblIgv ---
        lblIgv.AutoSize = true;
        lblIgv.Location = new System.Drawing.Point(420, 180);
        lblIgv.Name = "lblIgv";
        lblIgv.Size = new System.Drawing.Size(60, 15);
        lblIgv.TabIndex = 11;
        lblIgv.Text = "IGV: 18%";

        // --- dgvDetalles ---
        dgvDetalles.AllowUserToAddRows = true;
        dgvDetalles.AllowUserToDeleteRows = true;
        dgvDetalles.Anchor = System.Windows.Forms.AnchorStyles.Top
            | System.Windows.Forms.AnchorStyles.Bottom
            | System.Windows.Forms.AnchorStyles.Left
            | System.Windows.Forms.AnchorStyles.Right;
        dgvDetalles.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        dgvDetalles.Columns.AddRange(colDescripcion, colPeso, colPrecioUnitario, colPrecioTotal, colCantidad);
        dgvDetalles.Location = new System.Drawing.Point(12, 210);
        dgvDetalles.Name = "dgvDetalles";
        dgvDetalles.Size = new System.Drawing.Size(760, 240);
        dgvDetalles.TabIndex = 12;

        // --- colDescripcion ---
        colDescripcion.HeaderText = "Descripción";
        colDescripcion.Name = "colDescripcion";
        colDescripcion.Width = 250;

        // --- colPeso ---
        colPeso.HeaderText = "Peso";
        colPeso.Name = "colPeso";
        colPeso.Width = 80;

        // --- colPrecioUnitario ---
        colPrecioUnitario.HeaderText = "Precio Unitario";
        colPrecioUnitario.Name = "colPrecioUnitario";
        colPrecioUnitario.Width = 110;

        // --- colPrecioTotal ---
        colPrecioTotal.HeaderText = "Precio Total";
        colPrecioTotal.Name = "colPrecioTotal";
        colPrecioTotal.Width = 110;

        // --- colCantidad ---
        colCantidad.HeaderText = "Cantidad";
        colCantidad.Name = "colCantidad";
        colCantidad.Width = 80;

        // --- btnGrabar ---
        btnGrabar.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
        btnGrabar.Location = new System.Drawing.Point(616, 462);
        btnGrabar.Name = "btnGrabar";
        btnGrabar.Size = new System.Drawing.Size(75, 23);
        btnGrabar.TabIndex = 13;
        btnGrabar.Text = "Grabar";
        btnGrabar.UseVisualStyleBackColor = true;
        btnGrabar.Click += btnGrabar_Click;

        // --- btnCancelar ---
        btnCancelar.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
        btnCancelar.Location = new System.Drawing.Point(697, 462);
        btnCancelar.Name = "btnCancelar";
        btnCancelar.Size = new System.Drawing.Size(75, 23);
        btnCancelar.TabIndex = 14;
        btnCancelar.Text = "Cancelar";
        btnCancelar.UseVisualStyleBackColor = true;
        btnCancelar.Click += btnCancelar_Click;

        // --- frmGuiaRemision ---
        this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        this.ClientSize = new System.Drawing.Size(784, 497);
        this.Controls.Add(lblRemitente);
        this.Controls.Add(cmbRemitente);
        this.Controls.Add(lblDestinatario);
        this.Controls.Add(cmbDestinatario);
        this.Controls.Add(lblVehiculo);
        this.Controls.Add(cmbVehiculo);
        this.Controls.Add(lblCarreta);
        this.Controls.Add(cmbCarreta);
        this.Controls.Add(lblChofer);
        this.Controls.Add(cmbChofer);
        this.Controls.Add(chkHasManifest);
        this.Controls.Add(lblIgv);
        this.Controls.Add(dgvDetalles);
        this.Controls.Add(btnGrabar);
        this.Controls.Add(btnCancelar);
        this.MinimumSize = new System.Drawing.Size(800, 536);
        this.Name = "frmGuiaRemision";
        this.Text = "Guía de Remisión";
        this.Load += frmGuiaRemision_Load;

        ((System.ComponentModel.ISupportInitialize)dgvDetalles).EndInit();
        this.ResumeLayout(false);
        this.PerformLayout();
    }
}
