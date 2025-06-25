<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmRecepcion
    Inherits System.Windows.Forms.Form

    'Form reemplaza a Dispose para limpiar la lista de componentes.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Requerido por el Diseñador de Windows Forms
    Private components As System.ComponentModel.IContainer

    'NOTA: el Diseñador de Windows Forms necesita el siguiente procedimiento
    'Se puede modificar usando el Diseñador de Windows Forms.  
    'No lo modifique con el editor de código.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmRecepcion))
        Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle3 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Me.lblBultosTotales = New System.Windows.Forms.Label
        Me.btnSalir = New System.Windows.Forms.Button
        Me.lblCostoTotal = New System.Windows.Forms.Label
        Me.txtDestinatario = New System.Windows.Forms.TextBox
        Me.Label11 = New System.Windows.Forms.Label
        Me.lblPesoTotal = New System.Windows.Forms.Label
        Me.Label12 = New System.Windows.Forms.Label
        Me.Label24 = New System.Windows.Forms.Label
        Me.txtNroGuia = New System.Windows.Forms.TextBox
        Me.Label23 = New System.Windows.Forms.Label
        Me.cbEstado = New System.Windows.Forms.ComboBox
        Me.Label22 = New System.Windows.Forms.Label
        Me.txtObservaciones = New System.Windows.Forms.TextBox
        Me.cbDestiID = New System.Windows.Forms.ComboBox
        Me.txtDestiNroID = New System.Windows.Forms.TextBox
        Me.Descripcion = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.GroupBox5 = New System.Windows.Forms.GroupBox
        Me.Cantidad = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.GroupBox3 = New System.Windows.Forms.GroupBox
        Me.GroupBox1 = New System.Windows.Forms.GroupBox
        Me.txtCodigo = New System.Windows.Forms.TextBox
        Me.Label21 = New System.Windows.Forms.Label
        Me.Label5 = New System.Windows.Forms.Label
        Me.Peso = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Label16 = New System.Windows.Forms.Label
        Me.GroupBox2 = New System.Windows.Forms.GroupBox
        Me.Costo = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Unidad = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.txtRemiNroID = New System.Windows.Forms.TextBox
        Me.GroupBox6 = New System.Windows.Forms.GroupBox
        Me.cbDireccionPartida = New System.Windows.Forms.ComboBox
        Me.optDireccion1 = New System.Windows.Forms.RadioButton
        Me.optAgencia1 = New System.Windows.Forms.RadioButton
        Me.btnEditar = New System.Windows.Forms.Button
        Me.btnCancelar = New System.Windows.Forms.Button
        Me.btnEliminar = New System.Windows.Forms.Button
        Me.btnGenerarGuia = New System.Windows.Forms.Button
        Me.cbRemiID = New System.Windows.Forms.ComboBox
        Me.txtRemitente = New System.Windows.Forms.TextBox
        Me.Label10 = New System.Windows.Forms.Label
        Me.GroupBox7 = New System.Windows.Forms.GroupBox
        Me.cbDireccionDestino = New System.Windows.Forms.ComboBox
        Me.optDireccion2 = New System.Windows.Forms.RadioButton
        Me.optAgencia2 = New System.Windows.Forms.RadioButton
        Me.GroupBox4 = New System.Windows.Forms.GroupBox
        Me.Label9 = New System.Windows.Forms.Label
        Me.Column12 = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Column11 = New System.Windows.Forms.DataGridViewComboBoxColumn
        Me.btnBuscar = New System.Windows.Forms.Button
        Me.txtFecha2 = New System.Windows.Forms.MaskedTextBox
        Me.GroupBox12 = New System.Windows.Forms.GroupBox
        Me.Label27 = New System.Windows.Forms.Label
        Me.txtFecha1 = New System.Windows.Forms.MaskedTextBox
        Me.cbMes = New System.Windows.Forms.ComboBox
        Me.optIntervalo = New System.Windows.Forms.RadioButton
        Me.optMes = New System.Windows.Forms.RadioButton
        Me.optHoy = New System.Windows.Forms.RadioButton
        Me.TabPage2 = New System.Windows.Forms.TabPage
        Me.dtpEmision = New System.Windows.Forms.DateTimePicker
        Me.txtDistancia = New System.Windows.Forms.TextBox
        Me.txtUsuario = New System.Windows.Forms.TextBox
        Me.Label8 = New System.Windows.Forms.Label
        Me.Label20 = New System.Windows.Forms.Label
        Me.Label1 = New System.Windows.Forms.Label
        Me.cbDestino = New System.Windows.Forms.ComboBox
        Me.Label3 = New System.Windows.Forms.Label
        Me.Label25 = New System.Windows.Forms.Label
        Me.dgvDetalle = New System.Windows.Forms.DataGridView
        Me.TabControl1 = New Dotnetrix.Controls.TabControl
        Me.TabPage1 = New System.Windows.Forms.TabPage
        Me.dgvListado = New System.Windows.Forms.DataGridView
        Me.Column1 = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Column3 = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Column4 = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Column5 = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Column8 = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Column9 = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.btnGrabar = New System.Windows.Forms.Button
        Me.btnNuevo = New System.Windows.Forms.Button
        Me.lblCantReg = New System.Windows.Forms.Label
        Me.Label4 = New System.Windows.Forms.Label
        Me.GroupBox5.SuspendLayout()
        Me.GroupBox3.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        Me.GroupBox6.SuspendLayout()
        Me.GroupBox7.SuspendLayout()
        Me.GroupBox4.SuspendLayout()
        Me.GroupBox12.SuspendLayout()
        Me.TabPage2.SuspendLayout()
        CType(Me.dgvDetalle, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.TabControl1.SuspendLayout()
        Me.TabPage1.SuspendLayout()
        CType(Me.dgvListado, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'lblBultosTotales
        '
        Me.lblBultosTotales.BackColor = System.Drawing.Color.Red
        Me.lblBultosTotales.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblBultosTotales.ForeColor = System.Drawing.Color.White
        Me.lblBultosTotales.Location = New System.Drawing.Point(214, 502)
        Me.lblBultosTotales.Name = "lblBultosTotales"
        Me.lblBultosTotales.Size = New System.Drawing.Size(76, 21)
        Me.lblBultosTotales.TabIndex = 59
        Me.lblBultosTotales.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'btnSalir
        '
        Me.btnSalir.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnSalir.BackColor = System.Drawing.SystemColors.ControlLight
        Me.btnSalir.Cursor = System.Windows.Forms.Cursors.Default
        Me.btnSalir.ForeColor = System.Drawing.SystemColors.ControlText
        Me.btnSalir.Image = CType(resources.GetObject("btnSalir.Image"), System.Drawing.Image)
        Me.btnSalir.Location = New System.Drawing.Point(767, 417)
        Me.btnSalir.Name = "btnSalir"
        Me.btnSalir.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.btnSalir.Size = New System.Drawing.Size(62, 48)
        Me.btnSalir.TabIndex = 42
        Me.btnSalir.Text = "Salir"
        Me.btnSalir.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.btnSalir.UseVisualStyleBackColor = False
        '
        'lblCostoTotal
        '
        Me.lblCostoTotal.BackColor = System.Drawing.Color.Red
        Me.lblCostoTotal.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCostoTotal.ForeColor = System.Drawing.Color.White
        Me.lblCostoTotal.Location = New System.Drawing.Point(614, 502)
        Me.lblCostoTotal.Name = "lblCostoTotal"
        Me.lblCostoTotal.Size = New System.Drawing.Size(88, 21)
        Me.lblCostoTotal.TabIndex = 61
        Me.lblCostoTotal.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'txtDestinatario
        '
        Me.txtDestinatario.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtDestinatario.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtDestinatario.Location = New System.Drawing.Point(63, 46)
        Me.txtDestinatario.Multiline = True
        Me.txtDestinatario.Name = "txtDestinatario"
        Me.txtDestinatario.Size = New System.Drawing.Size(264, 33)
        Me.txtDestinatario.TabIndex = 2
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Location = New System.Drawing.Point(10, 22)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(50, 13)
        Me.Label11.TabIndex = 1
        Me.Label11.Text = "Doc. ID :"
        '
        'lblPesoTotal
        '
        Me.lblPesoTotal.BackColor = System.Drawing.Color.Red
        Me.lblPesoTotal.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPesoTotal.ForeColor = System.Drawing.Color.White
        Me.lblPesoTotal.Location = New System.Drawing.Point(406, 502)
        Me.lblPesoTotal.Name = "lblPesoTotal"
        Me.lblPesoTotal.Size = New System.Drawing.Size(81, 21)
        Me.lblPesoTotal.TabIndex = 63
        Me.lblPesoTotal.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label12
        '
        Me.Label12.AutoSize = True
        Me.Label12.Location = New System.Drawing.Point(10, 60)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(50, 13)
        Me.Label12.TabIndex = 1
        Me.Label12.Text = "Nombre :"
        '
        'Label24
        '
        Me.Label24.AutoSize = True
        Me.Label24.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label24.Location = New System.Drawing.Point(121, 505)
        Me.Label24.Name = "Label24"
        Me.Label24.Size = New System.Drawing.Size(96, 13)
        Me.Label24.TabIndex = 64
        Me.Label24.Text = "Bultos Totales :"
        '
        'txtNroGuia
        '
        Me.txtNroGuia.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtNroGuia.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtNroGuia.Location = New System.Drawing.Point(8, 19)
        Me.txtNroGuia.Name = "txtNroGuia"
        Me.txtNroGuia.Size = New System.Drawing.Size(268, 21)
        Me.txtNroGuia.TabIndex = 0
        '
        'Label23
        '
        Me.Label23.AutoSize = True
        Me.Label23.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label23.Location = New System.Drawing.Point(518, 505)
        Me.Label23.Name = "Label23"
        Me.Label23.Size = New System.Drawing.Size(96, 13)
        Me.Label23.TabIndex = 62
        Me.Label23.Text = "C.Total (+IGV) :"
        '
        'cbEstado
        '
        Me.cbEstado.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cbEstado.FormattingEnabled = True
        Me.cbEstado.Location = New System.Drawing.Point(13, 19)
        Me.cbEstado.Name = "cbEstado"
        Me.cbEstado.Size = New System.Drawing.Size(181, 21)
        Me.cbEstado.TabIndex = 0
        '
        'Label22
        '
        Me.Label22.AutoSize = True
        Me.Label22.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label22.Location = New System.Drawing.Point(332, 506)
        Me.Label22.Name = "Label22"
        Me.Label22.Size = New System.Drawing.Size(76, 13)
        Me.Label22.TabIndex = 60
        Me.Label22.Text = "Peso Total :"
        '
        'txtObservaciones
        '
        Me.txtObservaciones.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtObservaciones.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtObservaciones.Location = New System.Drawing.Point(119, 533)
        Me.txtObservaciones.Name = "txtObservaciones"
        Me.txtObservaciones.Size = New System.Drawing.Size(583, 21)
        Me.txtObservaciones.TabIndex = 57
        '
        'cbDestiID
        '
        Me.cbDestiID.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cbDestiID.FormattingEnabled = True
        Me.cbDestiID.Location = New System.Drawing.Point(63, 19)
        Me.cbDestiID.Name = "cbDestiID"
        Me.cbDestiID.Size = New System.Drawing.Size(121, 21)
        Me.cbDestiID.TabIndex = 3
        '
        'txtDestiNroID
        '
        Me.txtDestiNroID.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtDestiNroID.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtDestiNroID.Location = New System.Drawing.Point(190, 19)
        Me.txtDestiNroID.Name = "txtDestiNroID"
        Me.txtDestiNroID.Size = New System.Drawing.Size(137, 21)
        Me.txtDestiNroID.TabIndex = 4
        '
        'Descripcion
        '
        Me.Descripcion.HeaderText = "DESCRIPCION"
        Me.Descripcion.Name = "Descripcion"
        Me.Descripcion.Resizable = System.Windows.Forms.DataGridViewTriState.[False]
        Me.Descripcion.Width = 215
        '
        'GroupBox5
        '
        Me.GroupBox5.Controls.Add(Me.txtDestiNroID)
        Me.GroupBox5.Controls.Add(Me.cbDestiID)
        Me.GroupBox5.Controls.Add(Me.txtDestinatario)
        Me.GroupBox5.Controls.Add(Me.Label11)
        Me.GroupBox5.Controls.Add(Me.Label12)
        Me.GroupBox5.Location = New System.Drawing.Point(363, 132)
        Me.GroupBox5.Name = "GroupBox5"
        Me.GroupBox5.Size = New System.Drawing.Size(339, 90)
        Me.GroupBox5.TabIndex = 48
        Me.GroupBox5.TabStop = False
        Me.GroupBox5.Text = "Destinatario"
        '
        'Cantidad
        '
        Me.Cantidad.HeaderText = "CANT."
        Me.Cantidad.Name = "Cantidad"
        Me.Cantidad.Resizable = System.Windows.Forms.DataGridViewTriState.[False]
        Me.Cantidad.Width = 50
        '
        'GroupBox3
        '
        Me.GroupBox3.Controls.Add(Me.txtNroGuia)
        Me.GroupBox3.Location = New System.Drawing.Point(228, 74)
        Me.GroupBox3.Name = "GroupBox3"
        Me.GroupBox3.Size = New System.Drawing.Size(282, 52)
        Me.GroupBox3.TabIndex = 47
        Me.GroupBox3.TabStop = False
        Me.GroupBox3.Text = "Guía de Remisión"
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.txtCodigo)
        Me.GroupBox1.Controls.Add(Me.Label21)
        Me.GroupBox1.Controls.Add(Me.Label5)
        Me.GroupBox1.Location = New System.Drawing.Point(516, 11)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(186, 115)
        Me.GroupBox1.TabIndex = 45
        Me.GroupBox1.TabStop = False
        '
        'txtCodigo
        '
        Me.txtCodigo.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtCodigo.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtCodigo.Location = New System.Drawing.Point(78, 53)
        Me.txtCodigo.Name = "txtCodigo"
        Me.txtCodigo.ReadOnly = True
        Me.txtCodigo.Size = New System.Drawing.Size(62, 21)
        Me.txtCodigo.TabIndex = 75
        '
        'Label21
        '
        Me.Label21.AutoSize = True
        Me.Label21.Location = New System.Drawing.Point(32, 56)
        Me.Label21.Name = "Label21"
        Me.Label21.Size = New System.Drawing.Size(46, 13)
        Me.Label21.TabIndex = 70
        Me.Label21.Text = "Código :"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Verdana", 15.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.Location = New System.Drawing.Point(20, 16)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(148, 25)
        Me.Label5.TabIndex = 1
        Me.Label5.Text = "RECEPCIÓN"
        '
        'Peso
        '
        Me.Peso.HeaderText = "PESO"
        Me.Peso.Name = "Peso"
        Me.Peso.Resizable = System.Windows.Forms.DataGridViewTriState.[False]
        Me.Peso.Width = 55
        '
        'Label16
        '
        Me.Label16.AutoSize = True
        Me.Label16.Location = New System.Drawing.Point(29, 536)
        Me.Label16.Name = "Label16"
        Me.Label16.Size = New System.Drawing.Size(84, 13)
        Me.Label16.TabIndex = 55
        Me.Label16.Text = "Observaciones :"
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.cbEstado)
        Me.GroupBox2.Location = New System.Drawing.Point(24, 74)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(200, 52)
        Me.GroupBox2.TabIndex = 46
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "Estado"
        '
        'Costo
        '
        DataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        DataGridViewCellStyle1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Costo.DefaultCellStyle = DataGridViewCellStyle1
        Me.Costo.HeaderText = "COSTO"
        Me.Costo.Name = "Costo"
        Me.Costo.Resizable = System.Windows.Forms.DataGridViewTriState.[False]
        Me.Costo.Width = 60
        '
        'Unidad
        '
        Me.Unidad.HeaderText = "UNIDAD"
        Me.Unidad.Name = "Unidad"
        Me.Unidad.Resizable = System.Windows.Forms.DataGridViewTriState.[False]
        Me.Unidad.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.Unidad.Width = 80
        '
        'txtRemiNroID
        '
        Me.txtRemiNroID.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtRemiNroID.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtRemiNroID.Location = New System.Drawing.Point(190, 19)
        Me.txtRemiNroID.Name = "txtRemiNroID"
        Me.txtRemiNroID.Size = New System.Drawing.Size(137, 21)
        Me.txtRemiNroID.TabIndex = 4
        '
        'GroupBox6
        '
        Me.GroupBox6.Controls.Add(Me.cbDireccionPartida)
        Me.GroupBox6.Controls.Add(Me.optDireccion1)
        Me.GroupBox6.Controls.Add(Me.optAgencia1)
        Me.GroupBox6.Location = New System.Drawing.Point(23, 228)
        Me.GroupBox6.Name = "GroupBox6"
        Me.GroupBox6.Size = New System.Drawing.Size(339, 79)
        Me.GroupBox6.TabIndex = 49
        Me.GroupBox6.TabStop = False
        Me.GroupBox6.Text = "Dirección de partida"
        '
        'cbDireccionPartida
        '
        Me.cbDireccionPartida.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cbDireccionPartida.FormattingEnabled = True
        Me.cbDireccionPartida.Location = New System.Drawing.Point(14, 45)
        Me.cbDireccionPartida.Name = "cbDireccionPartida"
        Me.cbDireccionPartida.Size = New System.Drawing.Size(313, 21)
        Me.cbDireccionPartida.TabIndex = 1
        '
        'optDireccion1
        '
        Me.optDireccion1.AutoSize = True
        Me.optDireccion1.Location = New System.Drawing.Point(99, 22)
        Me.optDireccion1.Name = "optDireccion1"
        Me.optDireccion1.Size = New System.Drawing.Size(75, 17)
        Me.optDireccion1.TabIndex = 0
        Me.optDireccion1.Text = "Del cliente"
        Me.optDireccion1.UseVisualStyleBackColor = True
        '
        'optAgencia1
        '
        Me.optAgencia1.AutoSize = True
        Me.optAgencia1.Location = New System.Drawing.Point(19, 22)
        Me.optAgencia1.Name = "optAgencia1"
        Me.optAgencia1.Size = New System.Drawing.Size(64, 17)
        Me.optAgencia1.TabIndex = 0
        Me.optAgencia1.Text = "Agencia"
        Me.optAgencia1.UseVisualStyleBackColor = True
        '
        'btnEditar
        '
        Me.btnEditar.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnEditar.BackColor = System.Drawing.SystemColors.Control
        Me.btnEditar.Cursor = System.Windows.Forms.Cursors.Default
        Me.btnEditar.ForeColor = System.Drawing.SystemColors.ControlText
        Me.btnEditar.Image = CType(resources.GetObject("btnEditar.Image"), System.Drawing.Image)
        Me.btnEditar.Location = New System.Drawing.Point(767, 201)
        Me.btnEditar.Name = "btnEditar"
        Me.btnEditar.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.btnEditar.Size = New System.Drawing.Size(62, 48)
        Me.btnEditar.TabIndex = 44
        Me.btnEditar.Text = "Editar"
        Me.btnEditar.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.btnEditar.UseVisualStyleBackColor = False
        '
        'btnCancelar
        '
        Me.btnCancelar.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnCancelar.BackColor = System.Drawing.SystemColors.Control
        Me.btnCancelar.Cursor = System.Windows.Forms.Cursors.Default
        Me.btnCancelar.ForeColor = System.Drawing.SystemColors.ControlText
        Me.btnCancelar.Image = CType(resources.GetObject("btnCancelar.Image"), System.Drawing.Image)
        Me.btnCancelar.Location = New System.Drawing.Point(767, 255)
        Me.btnCancelar.Name = "btnCancelar"
        Me.btnCancelar.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.btnCancelar.Size = New System.Drawing.Size(62, 48)
        Me.btnCancelar.TabIndex = 45
        Me.btnCancelar.Text = "Cancelar"
        Me.btnCancelar.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.btnCancelar.UseVisualStyleBackColor = False
        '
        'btnEliminar
        '
        Me.btnEliminar.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnEliminar.BackColor = System.Drawing.SystemColors.ControlLight
        Me.btnEliminar.Cursor = System.Windows.Forms.Cursors.Default
        Me.btnEliminar.ForeColor = System.Drawing.SystemColors.ControlText
        Me.btnEliminar.Image = CType(resources.GetObject("btnEliminar.Image"), System.Drawing.Image)
        Me.btnEliminar.Location = New System.Drawing.Point(767, 309)
        Me.btnEliminar.Name = "btnEliminar"
        Me.btnEliminar.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.btnEliminar.Size = New System.Drawing.Size(62, 48)
        Me.btnEliminar.TabIndex = 41
        Me.btnEliminar.Text = "Eliminar"
        Me.btnEliminar.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.btnEliminar.UseVisualStyleBackColor = False
        '
        'btnGenerarGuia
        '
        Me.btnGenerarGuia.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnGenerarGuia.BackColor = System.Drawing.SystemColors.ControlLight
        Me.btnGenerarGuia.Cursor = System.Windows.Forms.Cursors.Default
        Me.btnGenerarGuia.ForeColor = System.Drawing.SystemColors.ControlText
        Me.btnGenerarGuia.Location = New System.Drawing.Point(767, 363)
        Me.btnGenerarGuia.Name = "btnGenerarGuia"
        Me.btnGenerarGuia.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.btnGenerarGuia.Size = New System.Drawing.Size(62, 48)
        Me.btnGenerarGuia.TabIndex = 40
        Me.btnGenerarGuia.Text = "Guía Remisión"
        Me.btnGenerarGuia.UseVisualStyleBackColor = False
        '
        'cbRemiID
        '
        Me.cbRemiID.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cbRemiID.FormattingEnabled = True
        Me.cbRemiID.Location = New System.Drawing.Point(63, 19)
        Me.cbRemiID.Name = "cbRemiID"
        Me.cbRemiID.Size = New System.Drawing.Size(121, 21)
        Me.cbRemiID.TabIndex = 3
        '
        'txtRemitente
        '
        Me.txtRemitente.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtRemitente.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtRemitente.Location = New System.Drawing.Point(63, 45)
        Me.txtRemitente.Multiline = True
        Me.txtRemitente.Name = "txtRemitente"
        Me.txtRemitente.Size = New System.Drawing.Size(264, 33)
        Me.txtRemitente.TabIndex = 2
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Location = New System.Drawing.Point(10, 22)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(50, 13)
        Me.Label10.TabIndex = 1
        Me.Label10.Text = "Doc. ID :"
        '
        'GroupBox7
        '
        Me.GroupBox7.Controls.Add(Me.cbDireccionDestino)
        Me.GroupBox7.Controls.Add(Me.optDireccion2)
        Me.GroupBox7.Controls.Add(Me.optAgencia2)
        Me.GroupBox7.Location = New System.Drawing.Point(363, 228)
        Me.GroupBox7.Name = "GroupBox7"
        Me.GroupBox7.Size = New System.Drawing.Size(339, 79)
        Me.GroupBox7.TabIndex = 50
        Me.GroupBox7.TabStop = False
        Me.GroupBox7.Text = "Dirección de llegada"
        '
        'cbDireccionDestino
        '
        Me.cbDireccionDestino.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cbDireccionDestino.FormattingEnabled = True
        Me.cbDireccionDestino.Location = New System.Drawing.Point(13, 45)
        Me.cbDireccionDestino.Name = "cbDireccionDestino"
        Me.cbDireccionDestino.Size = New System.Drawing.Size(314, 21)
        Me.cbDireccionDestino.TabIndex = 1
        '
        'optDireccion2
        '
        Me.optDireccion2.AutoSize = True
        Me.optDireccion2.Location = New System.Drawing.Point(92, 22)
        Me.optDireccion2.Name = "optDireccion2"
        Me.optDireccion2.Size = New System.Drawing.Size(75, 17)
        Me.optDireccion2.TabIndex = 0
        Me.optDireccion2.Text = "Del cliente"
        Me.optDireccion2.UseVisualStyleBackColor = True
        '
        'optAgencia2
        '
        Me.optAgencia2.AutoSize = True
        Me.optAgencia2.Location = New System.Drawing.Point(13, 22)
        Me.optAgencia2.Name = "optAgencia2"
        Me.optAgencia2.Size = New System.Drawing.Size(64, 17)
        Me.optAgencia2.TabIndex = 0
        Me.optAgencia2.Text = "Agencia"
        Me.optAgencia2.UseVisualStyleBackColor = True
        '
        'GroupBox4
        '
        Me.GroupBox4.Controls.Add(Me.txtRemiNroID)
        Me.GroupBox4.Controls.Add(Me.cbRemiID)
        Me.GroupBox4.Controls.Add(Me.txtRemitente)
        Me.GroupBox4.Controls.Add(Me.Label10)
        Me.GroupBox4.Controls.Add(Me.Label9)
        Me.GroupBox4.Location = New System.Drawing.Point(23, 132)
        Me.GroupBox4.Name = "GroupBox4"
        Me.GroupBox4.Size = New System.Drawing.Size(339, 90)
        Me.GroupBox4.TabIndex = 51
        Me.GroupBox4.TabStop = False
        Me.GroupBox4.Text = "Remitente"
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Location = New System.Drawing.Point(7, 60)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(50, 13)
        Me.Label9.TabIndex = 1
        Me.Label9.Text = "Nombre :"
        '
        'Column12
        '
        Me.Column12.HeaderText = "Nro.Doc"
        Me.Column12.Name = "Column12"
        Me.Column12.Width = 80
        '
        'Column11
        '
        Me.Column11.HeaderText = "T.Doc"
        Me.Column11.Items.AddRange(New Object() {"GUIA", "FACT", "BOL"})
        Me.Column11.Name = "Column11"
        Me.Column11.Width = 70
        '
        'btnBuscar
        '
        Me.btnBuscar.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnBuscar.BackColor = System.Drawing.SystemColors.Control
        Me.btnBuscar.Cursor = System.Windows.Forms.Cursors.Default
        Me.btnBuscar.ForeColor = System.Drawing.SystemColors.ControlText
        Me.btnBuscar.Image = CType(resources.GetObject("btnBuscar.Image"), System.Drawing.Image)
        Me.btnBuscar.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnBuscar.Location = New System.Drawing.Point(617, 16)
        Me.btnBuscar.Name = "btnBuscar"
        Me.btnBuscar.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.btnBuscar.Size = New System.Drawing.Size(72, 25)
        Me.btnBuscar.TabIndex = 10
        Me.btnBuscar.Text = "Buscar"
        Me.btnBuscar.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnBuscar.UseVisualStyleBackColor = False
        '
        'txtFecha2
        '
        Me.txtFecha2.Location = New System.Drawing.Point(515, 20)
        Me.txtFecha2.Mask = "00/00/0000"
        Me.txtFecha2.Name = "txtFecha2"
        Me.txtFecha2.Size = New System.Drawing.Size(66, 20)
        Me.txtFecha2.TabIndex = 2
        Me.txtFecha2.ValidatingType = GetType(Date)
        '
        'GroupBox12
        '
        Me.GroupBox12.Controls.Add(Me.btnBuscar)
        Me.GroupBox12.Controls.Add(Me.txtFecha2)
        Me.GroupBox12.Controls.Add(Me.Label27)
        Me.GroupBox12.Controls.Add(Me.txtFecha1)
        Me.GroupBox12.Controls.Add(Me.cbMes)
        Me.GroupBox12.Controls.Add(Me.optIntervalo)
        Me.GroupBox12.Controls.Add(Me.optMes)
        Me.GroupBox12.Controls.Add(Me.optHoy)
        Me.GroupBox12.Location = New System.Drawing.Point(6, 6)
        Me.GroupBox12.Name = "GroupBox12"
        Me.GroupBox12.Size = New System.Drawing.Size(713, 52)
        Me.GroupBox12.TabIndex = 3
        Me.GroupBox12.TabStop = False
        Me.GroupBox12.Text = "Búsqueda"
        '
        'Label27
        '
        Me.Label27.AutoSize = True
        Me.Label27.Location = New System.Drawing.Point(494, 24)
        Me.Label27.Name = "Label27"
        Me.Label27.Size = New System.Drawing.Size(15, 13)
        Me.Label27.TabIndex = 1
        Me.Label27.Text = "al"
        '
        'txtFecha1
        '
        Me.txtFecha1.Location = New System.Drawing.Point(422, 19)
        Me.txtFecha1.Mask = "00/00/0000"
        Me.txtFecha1.Name = "txtFecha1"
        Me.txtFecha1.Size = New System.Drawing.Size(67, 20)
        Me.txtFecha1.TabIndex = 2
        Me.txtFecha1.ValidatingType = GetType(Date)
        '
        'cbMes
        '
        Me.cbMes.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cbMes.Enabled = False
        Me.cbMes.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cbMes.FormattingEnabled = True
        Me.cbMes.Items.AddRange(New Object() {"ENERO", "FEBRERO", "MARZO", "ABRIL", "MAYO", "JUNIO", "JULIO", "AGOSTO", "SETIEMBRE", "OCTUBRE", "NOVIEMBRE", "DICIEMBRE"})
        Me.cbMes.Location = New System.Drawing.Point(168, 18)
        Me.cbMes.Name = "cbMes"
        Me.cbMes.Size = New System.Drawing.Size(137, 21)
        Me.cbMes.TabIndex = 1
        '
        'optIntervalo
        '
        Me.optIntervalo.AutoSize = True
        Me.optIntervalo.Location = New System.Drawing.Point(375, 20)
        Me.optIntervalo.Name = "optIntervalo"
        Me.optIntervalo.Size = New System.Drawing.Size(41, 17)
        Me.optIntervalo.TabIndex = 3
        Me.optIntervalo.Text = "Del"
        Me.optIntervalo.UseVisualStyleBackColor = True
        '
        'optMes
        '
        Me.optMes.AutoSize = True
        Me.optMes.Location = New System.Drawing.Point(117, 21)
        Me.optMes.Name = "optMes"
        Me.optMes.Size = New System.Drawing.Size(45, 17)
        Me.optMes.TabIndex = 0
        Me.optMes.Text = "Mes"
        Me.optMes.UseVisualStyleBackColor = True
        '
        'optHoy
        '
        Me.optHoy.AutoSize = True
        Me.optHoy.Checked = True
        Me.optHoy.Location = New System.Drawing.Point(24, 20)
        Me.optHoy.Name = "optHoy"
        Me.optHoy.Size = New System.Drawing.Size(44, 17)
        Me.optHoy.TabIndex = 0
        Me.optHoy.TabStop = True
        Me.optHoy.Text = "Hoy"
        Me.optHoy.UseVisualStyleBackColor = True
        '
        'TabPage2
        '
        Me.TabPage2.BackColor = System.Drawing.SystemColors.Control
        Me.TabPage2.Controls.Add(Me.dtpEmision)
        Me.TabPage2.Controls.Add(Me.txtDistancia)
        Me.TabPage2.Controls.Add(Me.txtUsuario)
        Me.TabPage2.Controls.Add(Me.Label8)
        Me.TabPage2.Controls.Add(Me.Label20)
        Me.TabPage2.Controls.Add(Me.Label1)
        Me.TabPage2.Controls.Add(Me.cbDestino)
        Me.TabPage2.Controls.Add(Me.Label3)
        Me.TabPage2.Controls.Add(Me.Label25)
        Me.TabPage2.Controls.Add(Me.dgvDetalle)
        Me.TabPage2.Controls.Add(Me.lblBultosTotales)
        Me.TabPage2.Controls.Add(Me.lblCostoTotal)
        Me.TabPage2.Controls.Add(Me.Label24)
        Me.TabPage2.Controls.Add(Me.lblPesoTotal)
        Me.TabPage2.Controls.Add(Me.Label23)
        Me.TabPage2.Controls.Add(Me.Label22)
        Me.TabPage2.Controls.Add(Me.txtObservaciones)
        Me.TabPage2.Controls.Add(Me.Label16)
        Me.TabPage2.Controls.Add(Me.GroupBox5)
        Me.TabPage2.Controls.Add(Me.GroupBox2)
        Me.TabPage2.Controls.Add(Me.GroupBox3)
        Me.TabPage2.Controls.Add(Me.GroupBox1)
        Me.TabPage2.Controls.Add(Me.GroupBox7)
        Me.TabPage2.Controls.Add(Me.GroupBox4)
        Me.TabPage2.Controls.Add(Me.GroupBox6)
        Me.TabPage2.Location = New System.Drawing.Point(4, 23)
        Me.TabPage2.Name = "TabPage2"
        Me.TabPage2.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage2.Size = New System.Drawing.Size(725, 561)
        Me.TabPage2.TabIndex = 1
        Me.TabPage2.Text = "Detalles"
        '
        'dtpEmision
        '
        Me.dtpEmision.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.dtpEmision.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.dtpEmision.Location = New System.Drawing.Point(110, 20)
        Me.dtpEmision.Name = "dtpEmision"
        Me.dtpEmision.Size = New System.Drawing.Size(90, 21)
        Me.dtpEmision.TabIndex = 65
        '
        'txtDistancia
        '
        Me.txtDistancia.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtDistancia.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtDistancia.Location = New System.Drawing.Point(388, 46)
        Me.txtDistancia.Name = "txtDistancia"
        Me.txtDistancia.Size = New System.Drawing.Size(68, 21)
        Me.txtDistancia.TabIndex = 74
        '
        'txtUsuario
        '
        Me.txtUsuario.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtUsuario.Font = New System.Drawing.Font("Verdana", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtUsuario.Location = New System.Drawing.Point(110, 46)
        Me.txtUsuario.Name = "txtUsuario"
        Me.txtUsuario.ReadOnly = True
        Me.txtUsuario.Size = New System.Drawing.Size(216, 20)
        Me.txtUsuario.TabIndex = 75
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(25, 50)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(79, 13)
        Me.Label8.TabIndex = 70
        Me.Label8.Text = "Registrado por:"
        '
        'Label20
        '
        Me.Label20.AutoSize = True
        Me.Label20.Location = New System.Drawing.Point(332, 49)
        Me.Label20.Name = "Label20"
        Me.Label20.Size = New System.Drawing.Size(57, 13)
        Me.Label20.TabIndex = 71
        Me.Label20.Text = "Distancia :"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(61, 24)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(43, 13)
        Me.Label1.TabIndex = 69
        Me.Label1.Text = "Fecha :"
        '
        'cbDestino
        '
        Me.cbDestino.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cbDestino.FormattingEnabled = True
        Me.cbDestino.Location = New System.Drawing.Point(388, 19)
        Me.cbDestino.Name = "cbDestino"
        Me.cbDestino.Size = New System.Drawing.Size(121, 21)
        Me.cbDestino.TabIndex = 73
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(340, 24)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(49, 13)
        Me.Label3.TabIndex = 72
        Me.Label3.Text = "Destino :"
        '
        'Label25
        '
        Me.Label25.AutoSize = True
        Me.Label25.Location = New System.Drawing.Point(459, 50)
        Me.Label25.Name = "Label25"
        Me.Label25.Size = New System.Drawing.Size(25, 13)
        Me.Label25.TabIndex = 68
        Me.Label25.Text = "Km."
        '
        'dgvDetalle
        '
        Me.dgvDetalle.AllowUserToResizeColumns = False
        Me.dgvDetalle.AllowUserToResizeRows = False
        Me.dgvDetalle.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvDetalle.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.Column11, Me.Column12, Me.Cantidad, Me.Descripcion, Me.Peso, Me.Unidad, Me.Costo})
        Me.dgvDetalle.Location = New System.Drawing.Point(23, 313)
        Me.dgvDetalle.Name = "dgvDetalle"
        Me.dgvDetalle.Size = New System.Drawing.Size(679, 188)
        Me.dgvDetalle.TabIndex = 52
        '
        'TabControl1
        '
        Me.TabControl1.Controls.Add(Me.TabPage1)
        Me.TabControl1.Controls.Add(Me.TabPage2)
        Me.TabControl1.Location = New System.Drawing.Point(12, 6)
        Me.TabControl1.Name = "TabControl1"
        Me.TabControl1.SelectedIndex = 0
        Me.TabControl1.Size = New System.Drawing.Size(733, 588)
        Me.TabControl1.TabIndex = 43
        '
        'TabPage1
        '
        Me.TabPage1.BackColor = System.Drawing.SystemColors.Control
        Me.TabPage1.Controls.Add(Me.dgvListado)
        Me.TabPage1.Controls.Add(Me.GroupBox12)
        Me.TabPage1.Location = New System.Drawing.Point(4, 23)
        Me.TabPage1.Name = "TabPage1"
        Me.TabPage1.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage1.Size = New System.Drawing.Size(725, 561)
        Me.TabPage1.TabIndex = 0
        Me.TabPage1.Text = "Listado"
        '
        'dgvListado
        '
        Me.dgvListado.AllowDrop = True
        Me.dgvListado.AllowUserToResizeRows = False
        Me.dgvListado.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvListado.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.Column1, Me.Column3, Me.Column4, Me.Column5, Me.Column8, Me.Column9})
        Me.dgvListado.Location = New System.Drawing.Point(6, 64)
        Me.dgvListado.Name = "dgvListado"
        Me.dgvListado.Size = New System.Drawing.Size(713, 491)
        Me.dgvListado.TabIndex = 2
        '
        'Column1
        '
        DataGridViewCellStyle2.Format = "N0"
        DataGridViewCellStyle2.NullValue = Nothing
        Me.Column1.DefaultCellStyle = DataGridViewCellStyle2
        Me.Column1.HeaderText = "Cod"
        Me.Column1.Name = "Column1"
        Me.Column1.Width = 40
        '
        'Column3
        '
        Me.Column3.HeaderText = "Remitente"
        Me.Column3.Name = "Column3"
        Me.Column3.Width = 190
        '
        'Column4
        '
        Me.Column4.HeaderText = "Destinatario"
        Me.Column4.Name = "Column4"
        Me.Column4.Width = 190
        '
        'Column5
        '
        Me.Column5.HeaderText = "Fecha"
        Me.Column5.Name = "Column5"
        Me.Column5.Width = 75
        '
        'Column8
        '
        DataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        Me.Column8.DefaultCellStyle = DataGridViewCellStyle3
        Me.Column8.HeaderText = "Importe"
        Me.Column8.Name = "Column8"
        Me.Column8.Width = 60
        '
        'Column9
        '
        Me.Column9.HeaderText = "G. Rem."
        Me.Column9.Name = "Column9"
        Me.Column9.Width = 75
        '
        'btnGrabar
        '
        Me.btnGrabar.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnGrabar.BackColor = System.Drawing.SystemColors.ControlLight
        Me.btnGrabar.Cursor = System.Windows.Forms.Cursors.Default
        Me.btnGrabar.Enabled = False
        Me.btnGrabar.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnGrabar.ForeColor = System.Drawing.SystemColors.ControlText
        Me.btnGrabar.Image = CType(resources.GetObject("btnGrabar.Image"), System.Drawing.Image)
        Me.btnGrabar.Location = New System.Drawing.Point(767, 147)
        Me.btnGrabar.Name = "btnGrabar"
        Me.btnGrabar.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.btnGrabar.Size = New System.Drawing.Size(62, 48)
        Me.btnGrabar.TabIndex = 39
        Me.btnGrabar.Text = "Grabar"
        Me.btnGrabar.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.btnGrabar.UseVisualStyleBackColor = False
        '
        'btnNuevo
        '
        Me.btnNuevo.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnNuevo.BackColor = System.Drawing.SystemColors.ControlLight
        Me.btnNuevo.Cursor = System.Windows.Forms.Cursors.Default
        Me.btnNuevo.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnNuevo.ForeColor = System.Drawing.SystemColors.ControlText
        Me.btnNuevo.Image = CType(resources.GetObject("btnNuevo.Image"), System.Drawing.Image)
        Me.btnNuevo.Location = New System.Drawing.Point(767, 93)
        Me.btnNuevo.Name = "btnNuevo"
        Me.btnNuevo.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.btnNuevo.Size = New System.Drawing.Size(62, 48)
        Me.btnNuevo.TabIndex = 37
        Me.btnNuevo.Text = "Nuevo"
        Me.btnNuevo.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.btnNuevo.UseVisualStyleBackColor = False
        '
        'lblCantReg
        '
        Me.lblCantReg.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCantReg.ForeColor = System.Drawing.Color.Red
        Me.lblCantReg.Location = New System.Drawing.Point(753, 61)
        Me.lblCantReg.Name = "lblCantReg"
        Me.lblCantReg.Size = New System.Drawing.Size(76, 24)
        Me.lblCantReg.TabIndex = 48
        Me.lblCantReg.Text = "0"
        Me.lblCantReg.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(760, 35)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(69, 13)
        Me.Label4.TabIndex = 49
        Me.Label4.Text = "Nro registros:"
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'frmRecepcion
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(841, 606)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.lblCantReg)
        Me.Controls.Add(Me.btnSalir)
        Me.Controls.Add(Me.btnEditar)
        Me.Controls.Add(Me.btnCancelar)
        Me.Controls.Add(Me.btnEliminar)
        Me.Controls.Add(Me.btnGenerarGuia)
        Me.Controls.Add(Me.TabControl1)
        Me.Controls.Add(Me.btnGrabar)
        Me.Controls.Add(Me.btnNuevo)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.Name = "frmRecepcion"
        Me.Text = "Recepción"
        Me.GroupBox5.ResumeLayout(False)
        Me.GroupBox5.PerformLayout()
        Me.GroupBox3.ResumeLayout(False)
        Me.GroupBox3.PerformLayout()
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox6.ResumeLayout(False)
        Me.GroupBox6.PerformLayout()
        Me.GroupBox7.ResumeLayout(False)
        Me.GroupBox7.PerformLayout()
        Me.GroupBox4.ResumeLayout(False)
        Me.GroupBox4.PerformLayout()
        Me.GroupBox12.ResumeLayout(False)
        Me.GroupBox12.PerformLayout()
        Me.TabPage2.ResumeLayout(False)
        Me.TabPage2.PerformLayout()
        CType(Me.dgvDetalle, System.ComponentModel.ISupportInitialize).EndInit()
        Me.TabControl1.ResumeLayout(False)
        Me.TabPage1.ResumeLayout(False)
        CType(Me.dgvListado, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents lblBultosTotales As System.Windows.Forms.Label
    Public WithEvents btnSalir As System.Windows.Forms.Button
    Friend WithEvents lblCostoTotal As System.Windows.Forms.Label
    Friend WithEvents txtDestinatario As System.Windows.Forms.TextBox
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents lblPesoTotal As System.Windows.Forms.Label
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents Label24 As System.Windows.Forms.Label
    Friend WithEvents txtNroGuia As System.Windows.Forms.TextBox
    Friend WithEvents Label23 As System.Windows.Forms.Label
    Friend WithEvents cbEstado As System.Windows.Forms.ComboBox
    Friend WithEvents Label22 As System.Windows.Forms.Label
    Friend WithEvents txtObservaciones As System.Windows.Forms.TextBox
    Friend WithEvents cbDestiID As System.Windows.Forms.ComboBox
    Friend WithEvents txtDestiNroID As System.Windows.Forms.TextBox
    Friend WithEvents Descripcion As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents GroupBox5 As System.Windows.Forms.GroupBox
    Friend WithEvents Cantidad As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents GroupBox3 As System.Windows.Forms.GroupBox
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Peso As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Label16 As System.Windows.Forms.Label
    Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
    Friend WithEvents Costo As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Unidad As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents txtRemiNroID As System.Windows.Forms.TextBox
    Friend WithEvents GroupBox6 As System.Windows.Forms.GroupBox
    Friend WithEvents cbDireccionPartida As System.Windows.Forms.ComboBox
    Friend WithEvents optDireccion1 As System.Windows.Forms.RadioButton
    Friend WithEvents optAgencia1 As System.Windows.Forms.RadioButton
    Public WithEvents btnEditar As System.Windows.Forms.Button
    Public WithEvents btnCancelar As System.Windows.Forms.Button
    Public WithEvents btnEliminar As System.Windows.Forms.Button
    Public WithEvents btnGenerarGuia As System.Windows.Forms.Button
    Friend WithEvents cbRemiID As System.Windows.Forms.ComboBox
    Friend WithEvents txtRemitente As System.Windows.Forms.TextBox
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents GroupBox7 As System.Windows.Forms.GroupBox
    Friend WithEvents cbDireccionDestino As System.Windows.Forms.ComboBox
    Friend WithEvents optDireccion2 As System.Windows.Forms.RadioButton
    Friend WithEvents optAgencia2 As System.Windows.Forms.RadioButton
    Friend WithEvents GroupBox4 As System.Windows.Forms.GroupBox
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents Column12 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Column11 As System.Windows.Forms.DataGridViewComboBoxColumn
    Public WithEvents btnBuscar As System.Windows.Forms.Button
    Friend WithEvents txtFecha2 As System.Windows.Forms.MaskedTextBox
    Friend WithEvents GroupBox12 As System.Windows.Forms.GroupBox
    Friend WithEvents Label27 As System.Windows.Forms.Label
    Friend WithEvents txtFecha1 As System.Windows.Forms.MaskedTextBox
    Friend WithEvents cbMes As System.Windows.Forms.ComboBox
    Friend WithEvents optIntervalo As System.Windows.Forms.RadioButton
    Friend WithEvents optMes As System.Windows.Forms.RadioButton
    Friend WithEvents optHoy As System.Windows.Forms.RadioButton
    Friend WithEvents TabPage2 As System.Windows.Forms.TabPage
    Friend WithEvents dtpEmision As System.Windows.Forms.DateTimePicker
    Friend WithEvents txtDistancia As System.Windows.Forms.TextBox
    Friend WithEvents txtUsuario As System.Windows.Forms.TextBox
    Friend WithEvents txtCodigo As System.Windows.Forms.TextBox
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents Label20 As System.Windows.Forms.Label
    Friend WithEvents Label21 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents cbDestino As System.Windows.Forms.ComboBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label25 As System.Windows.Forms.Label
    Friend WithEvents dgvDetalle As System.Windows.Forms.DataGridView
    Friend WithEvents TabControl1 As Dotnetrix.Controls.TabControl
    Friend WithEvents TabPage1 As System.Windows.Forms.TabPage
    Friend WithEvents dgvListado As System.Windows.Forms.DataGridView
    Public WithEvents btnGrabar As System.Windows.Forms.Button
    Public WithEvents btnNuevo As System.Windows.Forms.Button
    Friend WithEvents Column1 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Column3 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Column4 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Column5 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Column8 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Column9 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents lblCantReg As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
End Class
