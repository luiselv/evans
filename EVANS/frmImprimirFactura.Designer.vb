<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmImprimirFactura
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
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmImprimirFactura))
        Me.PictureBox1 = New System.Windows.Forms.PictureBox
        Me.lblDia = New System.Windows.Forms.Label
        Me.lblMes = New System.Windows.Forms.Label
        Me.lblAño = New System.Windows.Forms.Label
        Me.lblCliente = New System.Windows.Forms.Label
        Me.lblDireccion = New System.Windows.Forms.Label
        Me.lblRUC = New System.Windows.Forms.Label
        Me.lblCantidad1 = New System.Windows.Forms.Label
        Me.lblCantidad2 = New System.Windows.Forms.Label
        Me.lblCantidad3 = New System.Windows.Forms.Label
        Me.lblCantidad4 = New System.Windows.Forms.Label
        Me.lblCantidad5 = New System.Windows.Forms.Label
        Me.lblGRT = New System.Windows.Forms.Label
        Me.btnCancelar = New System.Windows.Forms.Button
        Me.btnImprimir = New System.Windows.Forms.Button
        Me.lblCantidad6 = New System.Windows.Forms.Label
        Me.lblCantidad7 = New System.Windows.Forms.Label
        Me.lblDescripcion1 = New System.Windows.Forms.Label
        Me.lblDescripcion2 = New System.Windows.Forms.Label
        Me.lblDescripcion3 = New System.Windows.Forms.Label
        Me.lblDescripcion4 = New System.Windows.Forms.Label
        Me.lblDescripcion5 = New System.Windows.Forms.Label
        Me.lblDescripcion6 = New System.Windows.Forms.Label
        Me.lblDescripcion7 = New System.Windows.Forms.Label
        Me.lblPU1 = New System.Windows.Forms.Label
        Me.lblPU2 = New System.Windows.Forms.Label
        Me.lblPU3 = New System.Windows.Forms.Label
        Me.lblPU4 = New System.Windows.Forms.Label
        Me.lblPU5 = New System.Windows.Forms.Label
        Me.lblPU6 = New System.Windows.Forms.Label
        Me.lblPU7 = New System.Windows.Forms.Label
        Me.lblFlete1 = New System.Windows.Forms.Label
        Me.lblFlete2 = New System.Windows.Forms.Label
        Me.lblFlete3 = New System.Windows.Forms.Label
        Me.lblFlete4 = New System.Windows.Forms.Label
        Me.lblFlete5 = New System.Windows.Forms.Label
        Me.lblFlete6 = New System.Windows.Forms.Label
        Me.lblFlete7 = New System.Windows.Forms.Label
        Me.lblVVenta = New System.Windows.Forms.Label
        Me.lblIGV = New System.Windows.Forms.Label
        Me.lblTotal = New System.Windows.Forms.Label
        Me.lblPorcentajeIGV = New System.Windows.Forms.Label
        Me.lblLetras = New System.Windows.Forms.Label
        Me.lblRemitente = New System.Windows.Forms.Label
        Me.lblTransportista = New System.Windows.Forms.Label
        Me.lblTranspRUC = New System.Windows.Forms.Label
        Me.lblDestino = New System.Windows.Forms.Label
        Me.lblManifiesto = New System.Windows.Forms.Label
        Me.btnEditar = New System.Windows.Forms.Button
        Me.ImageList1 = New System.Windows.Forms.ImageList(Me.components)
        Me.PrintForm1 = New Microsoft.VisualBasic.PowerPacks.Printing.PrintForm(Me.components)
        Me.gbIncremento = New System.Windows.Forms.GroupBox
        Me.cbFonts = New System.Windows.Forms.ComboBox
        Me.chkBold = New System.Windows.Forms.CheckBox
        Me.nudSize = New System.Windows.Forms.NumericUpDown
        Me.tbIncremento = New System.Windows.Forms.TrackBar
        Me.lblPosicion = New System.Windows.Forms.Label
        Me.Label1 = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.Label3 = New System.Windows.Forms.Label
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.gbIncremento.SuspendLayout()
        CType(Me.nudSize, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.tbIncremento, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'PictureBox1
        '
        Me.PictureBox1.Image = CType(resources.GetObject("PictureBox1.Image"), System.Drawing.Image)
        Me.PictureBox1.Location = New System.Drawing.Point(-20, -24)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(846, 566)
        Me.PictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.PictureBox1.TabIndex = 0
        Me.PictureBox1.TabStop = False
        '
        'lblDia
        '
        Me.lblDia.AutoSize = True
        Me.lblDia.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDia.Location = New System.Drawing.Point(60, 137)
        Me.lblDia.Name = "lblDia"
        Me.lblDia.Size = New System.Drawing.Size(21, 15)
        Me.lblDia.TabIndex = 0
        Me.lblDia.Text = "XX"
        Me.lblDia.UseMnemonic = False
        '
        'lblMes
        '
        Me.lblMes.AutoSize = True
        Me.lblMes.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblMes.Location = New System.Drawing.Point(117, 137)
        Me.lblMes.Name = "lblMes"
        Me.lblMes.Size = New System.Drawing.Size(77, 15)
        Me.lblMes.TabIndex = 0
        Me.lblMes.Text = "XXXXXXXXXX"
        Me.lblMes.UseMnemonic = False
        '
        'lblAño
        '
        Me.lblAño.AutoSize = True
        Me.lblAño.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblAño.Location = New System.Drawing.Point(257, 137)
        Me.lblAño.Name = "lblAño"
        Me.lblAño.Size = New System.Drawing.Size(21, 15)
        Me.lblAño.TabIndex = 0
        Me.lblAño.Text = "XX"
        Me.lblAño.UseMnemonic = False
        '
        'lblCliente
        '
        Me.lblCliente.AutoSize = True
        Me.lblCliente.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCliente.Location = New System.Drawing.Point(88, 165)
        Me.lblCliente.Name = "lblCliente"
        Me.lblCliente.Size = New System.Drawing.Size(35, 15)
        Me.lblCliente.TabIndex = 0
        Me.lblCliente.Text = "XXXX"
        Me.lblCliente.UseMnemonic = False
        '
        'lblDireccion
        '
        Me.lblDireccion.AutoSize = True
        Me.lblDireccion.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDireccion.Location = New System.Drawing.Point(88, 190)
        Me.lblDireccion.Name = "lblDireccion"
        Me.lblDireccion.Size = New System.Drawing.Size(35, 15)
        Me.lblDireccion.TabIndex = 0
        Me.lblDireccion.Text = "XXXX"
        Me.lblDireccion.UseMnemonic = False
        '
        'lblRUC
        '
        Me.lblRUC.AutoSize = True
        Me.lblRUC.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblRUC.Location = New System.Drawing.Point(536, 190)
        Me.lblRUC.Name = "lblRUC"
        Me.lblRUC.Size = New System.Drawing.Size(35, 15)
        Me.lblRUC.TabIndex = 0
        Me.lblRUC.Text = "XXXX"
        Me.lblRUC.UseMnemonic = False
        '
        'lblCantidad1
        '
        Me.lblCantidad1.AutoSize = True
        Me.lblCantidad1.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCantidad1.Location = New System.Drawing.Point(51, 243)
        Me.lblCantidad1.Name = "lblCantidad1"
        Me.lblCantidad1.Size = New System.Drawing.Size(35, 15)
        Me.lblCantidad1.TabIndex = 1
        Me.lblCantidad1.Text = "XXXX"
        Me.lblCantidad1.TextAlign = System.Drawing.ContentAlignment.BottomRight
        Me.lblCantidad1.UseMnemonic = False
        '
        'lblCantidad2
        '
        Me.lblCantidad2.AutoSize = True
        Me.lblCantidad2.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCantidad2.Location = New System.Drawing.Point(51, 261)
        Me.lblCantidad2.Name = "lblCantidad2"
        Me.lblCantidad2.Size = New System.Drawing.Size(35, 15)
        Me.lblCantidad2.TabIndex = 2
        Me.lblCantidad2.Text = "XXXX"
        Me.lblCantidad2.TextAlign = System.Drawing.ContentAlignment.BottomRight
        Me.lblCantidad2.UseMnemonic = False
        '
        'lblCantidad3
        '
        Me.lblCantidad3.AutoSize = True
        Me.lblCantidad3.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCantidad3.Location = New System.Drawing.Point(51, 281)
        Me.lblCantidad3.Name = "lblCantidad3"
        Me.lblCantidad3.Size = New System.Drawing.Size(35, 15)
        Me.lblCantidad3.TabIndex = 3
        Me.lblCantidad3.Text = "XXXX"
        Me.lblCantidad3.TextAlign = System.Drawing.ContentAlignment.BottomRight
        Me.lblCantidad3.UseMnemonic = False
        '
        'lblCantidad4
        '
        Me.lblCantidad4.AutoSize = True
        Me.lblCantidad4.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCantidad4.Location = New System.Drawing.Point(51, 301)
        Me.lblCantidad4.Name = "lblCantidad4"
        Me.lblCantidad4.Size = New System.Drawing.Size(35, 15)
        Me.lblCantidad4.TabIndex = 4
        Me.lblCantidad4.Text = "XXXX"
        Me.lblCantidad4.TextAlign = System.Drawing.ContentAlignment.BottomRight
        Me.lblCantidad4.UseMnemonic = False
        '
        'lblCantidad5
        '
        Me.lblCantidad5.AutoSize = True
        Me.lblCantidad5.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCantidad5.Location = New System.Drawing.Point(51, 319)
        Me.lblCantidad5.Name = "lblCantidad5"
        Me.lblCantidad5.Size = New System.Drawing.Size(35, 15)
        Me.lblCantidad5.TabIndex = 5
        Me.lblCantidad5.Text = "XXXX"
        Me.lblCantidad5.TextAlign = System.Drawing.ContentAlignment.BottomRight
        Me.lblCantidad5.UseMnemonic = False
        '
        'lblGRT
        '
        Me.lblGRT.AutoSize = True
        Me.lblGRT.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblGRT.Location = New System.Drawing.Point(107, 377)
        Me.lblGRT.Name = "lblGRT"
        Me.lblGRT.Size = New System.Drawing.Size(35, 15)
        Me.lblGRT.TabIndex = 0
        Me.lblGRT.Text = "XXXX"
        Me.lblGRT.UseMnemonic = False
        '
        'btnCancelar
        '
        Me.btnCancelar.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnCancelar.BackColor = System.Drawing.SystemColors.Control
        Me.btnCancelar.Cursor = System.Windows.Forms.Cursors.Default
        Me.btnCancelar.ForeColor = System.Drawing.SystemColors.ControlText
        Me.btnCancelar.Image = CType(resources.GetObject("btnCancelar.Image"), System.Drawing.Image)
        Me.btnCancelar.Location = New System.Drawing.Point(246, 12)
        Me.btnCancelar.Name = "btnCancelar"
        Me.btnCancelar.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.btnCancelar.Size = New System.Drawing.Size(62, 48)
        Me.btnCancelar.TabIndex = 42
        Me.btnCancelar.Text = "Cancelar"
        Me.btnCancelar.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.btnCancelar.UseVisualStyleBackColor = False
        '
        'btnImprimir
        '
        Me.btnImprimir.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnImprimir.BackColor = System.Drawing.SystemColors.ControlLight
        Me.btnImprimir.Cursor = System.Windows.Forms.Cursors.Default
        Me.btnImprimir.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnImprimir.ForeColor = System.Drawing.SystemColors.ControlText
        Me.btnImprimir.Image = CType(resources.GetObject("btnImprimir.Image"), System.Drawing.Image)
        Me.btnImprimir.Location = New System.Drawing.Point(110, 12)
        Me.btnImprimir.Name = "btnImprimir"
        Me.btnImprimir.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.btnImprimir.Size = New System.Drawing.Size(62, 48)
        Me.btnImprimir.TabIndex = 41
        Me.btnImprimir.Text = "Imprimir"
        Me.btnImprimir.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.btnImprimir.UseVisualStyleBackColor = False
        '
        'lblCantidad6
        '
        Me.lblCantidad6.AutoSize = True
        Me.lblCantidad6.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCantidad6.Location = New System.Drawing.Point(51, 338)
        Me.lblCantidad6.Name = "lblCantidad6"
        Me.lblCantidad6.Size = New System.Drawing.Size(35, 15)
        Me.lblCantidad6.TabIndex = 6
        Me.lblCantidad6.Text = "XXXX"
        Me.lblCantidad6.TextAlign = System.Drawing.ContentAlignment.BottomRight
        Me.lblCantidad6.UseMnemonic = False
        '
        'lblCantidad7
        '
        Me.lblCantidad7.AutoSize = True
        Me.lblCantidad7.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCantidad7.Location = New System.Drawing.Point(51, 358)
        Me.lblCantidad7.Name = "lblCantidad7"
        Me.lblCantidad7.Size = New System.Drawing.Size(35, 15)
        Me.lblCantidad7.TabIndex = 7
        Me.lblCantidad7.Text = "XXXX"
        Me.lblCantidad7.TextAlign = System.Drawing.ContentAlignment.BottomRight
        Me.lblCantidad7.UseMnemonic = False
        '
        'lblDescripcion1
        '
        Me.lblDescripcion1.AutoSize = True
        Me.lblDescripcion1.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDescripcion1.Location = New System.Drawing.Point(107, 243)
        Me.lblDescripcion1.Name = "lblDescripcion1"
        Me.lblDescripcion1.Size = New System.Drawing.Size(35, 15)
        Me.lblDescripcion1.TabIndex = 11
        Me.lblDescripcion1.Text = "XXXX"
        Me.lblDescripcion1.UseMnemonic = False
        '
        'lblDescripcion2
        '
        Me.lblDescripcion2.AutoSize = True
        Me.lblDescripcion2.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDescripcion2.Location = New System.Drawing.Point(107, 261)
        Me.lblDescripcion2.Name = "lblDescripcion2"
        Me.lblDescripcion2.Size = New System.Drawing.Size(35, 15)
        Me.lblDescripcion2.TabIndex = 12
        Me.lblDescripcion2.Text = "XXXX"
        Me.lblDescripcion2.UseMnemonic = False
        '
        'lblDescripcion3
        '
        Me.lblDescripcion3.AutoSize = True
        Me.lblDescripcion3.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDescripcion3.Location = New System.Drawing.Point(107, 281)
        Me.lblDescripcion3.Name = "lblDescripcion3"
        Me.lblDescripcion3.Size = New System.Drawing.Size(35, 15)
        Me.lblDescripcion3.TabIndex = 13
        Me.lblDescripcion3.Text = "XXXX"
        Me.lblDescripcion3.UseMnemonic = False
        '
        'lblDescripcion4
        '
        Me.lblDescripcion4.AutoSize = True
        Me.lblDescripcion4.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDescripcion4.Location = New System.Drawing.Point(107, 301)
        Me.lblDescripcion4.Name = "lblDescripcion4"
        Me.lblDescripcion4.Size = New System.Drawing.Size(35, 15)
        Me.lblDescripcion4.TabIndex = 14
        Me.lblDescripcion4.Text = "XXXX"
        Me.lblDescripcion4.UseMnemonic = False
        '
        'lblDescripcion5
        '
        Me.lblDescripcion5.AutoSize = True
        Me.lblDescripcion5.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDescripcion5.Location = New System.Drawing.Point(107, 319)
        Me.lblDescripcion5.Name = "lblDescripcion5"
        Me.lblDescripcion5.Size = New System.Drawing.Size(35, 15)
        Me.lblDescripcion5.TabIndex = 15
        Me.lblDescripcion5.Text = "XXXX"
        Me.lblDescripcion5.UseMnemonic = False
        '
        'lblDescripcion6
        '
        Me.lblDescripcion6.AutoSize = True
        Me.lblDescripcion6.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDescripcion6.Location = New System.Drawing.Point(107, 338)
        Me.lblDescripcion6.Name = "lblDescripcion6"
        Me.lblDescripcion6.Size = New System.Drawing.Size(35, 15)
        Me.lblDescripcion6.TabIndex = 16
        Me.lblDescripcion6.Text = "XXXX"
        Me.lblDescripcion6.UseMnemonic = False
        '
        'lblDescripcion7
        '
        Me.lblDescripcion7.AutoSize = True
        Me.lblDescripcion7.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDescripcion7.Location = New System.Drawing.Point(107, 358)
        Me.lblDescripcion7.Name = "lblDescripcion7"
        Me.lblDescripcion7.Size = New System.Drawing.Size(35, 15)
        Me.lblDescripcion7.TabIndex = 17
        Me.lblDescripcion7.Text = "XXXX"
        Me.lblDescripcion7.UseMnemonic = False
        '
        'lblPU1
        '
        Me.lblPU1.AutoSize = True
        Me.lblPU1.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPU1.Location = New System.Drawing.Point(594, 243)
        Me.lblPU1.Name = "lblPU1"
        Me.lblPU1.Size = New System.Drawing.Size(35, 15)
        Me.lblPU1.TabIndex = 21
        Me.lblPU1.Text = "XXXX"
        Me.lblPU1.TextAlign = System.Drawing.ContentAlignment.BottomRight
        Me.lblPU1.UseMnemonic = False
        '
        'lblPU2
        '
        Me.lblPU2.AutoSize = True
        Me.lblPU2.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPU2.Location = New System.Drawing.Point(594, 261)
        Me.lblPU2.Name = "lblPU2"
        Me.lblPU2.Size = New System.Drawing.Size(35, 15)
        Me.lblPU2.TabIndex = 22
        Me.lblPU2.Text = "XXXX"
        Me.lblPU2.TextAlign = System.Drawing.ContentAlignment.BottomRight
        Me.lblPU2.UseMnemonic = False
        '
        'lblPU3
        '
        Me.lblPU3.AutoSize = True
        Me.lblPU3.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPU3.Location = New System.Drawing.Point(594, 281)
        Me.lblPU3.Name = "lblPU3"
        Me.lblPU3.Size = New System.Drawing.Size(35, 15)
        Me.lblPU3.TabIndex = 23
        Me.lblPU3.Text = "XXXX"
        Me.lblPU3.TextAlign = System.Drawing.ContentAlignment.BottomRight
        Me.lblPU3.UseMnemonic = False
        '
        'lblPU4
        '
        Me.lblPU4.AutoSize = True
        Me.lblPU4.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPU4.Location = New System.Drawing.Point(594, 301)
        Me.lblPU4.Name = "lblPU4"
        Me.lblPU4.Size = New System.Drawing.Size(35, 15)
        Me.lblPU4.TabIndex = 24
        Me.lblPU4.Text = "XXXX"
        Me.lblPU4.TextAlign = System.Drawing.ContentAlignment.BottomRight
        Me.lblPU4.UseMnemonic = False
        '
        'lblPU5
        '
        Me.lblPU5.AutoSize = True
        Me.lblPU5.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPU5.Location = New System.Drawing.Point(594, 319)
        Me.lblPU5.Name = "lblPU5"
        Me.lblPU5.Size = New System.Drawing.Size(35, 15)
        Me.lblPU5.TabIndex = 25
        Me.lblPU5.Text = "XXXX"
        Me.lblPU5.TextAlign = System.Drawing.ContentAlignment.BottomRight
        Me.lblPU5.UseMnemonic = False
        '
        'lblPU6
        '
        Me.lblPU6.AutoSize = True
        Me.lblPU6.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPU6.Location = New System.Drawing.Point(594, 338)
        Me.lblPU6.Name = "lblPU6"
        Me.lblPU6.Size = New System.Drawing.Size(35, 15)
        Me.lblPU6.TabIndex = 26
        Me.lblPU6.Text = "XXXX"
        Me.lblPU6.TextAlign = System.Drawing.ContentAlignment.BottomRight
        Me.lblPU6.UseMnemonic = False
        '
        'lblPU7
        '
        Me.lblPU7.AutoSize = True
        Me.lblPU7.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPU7.Location = New System.Drawing.Point(594, 358)
        Me.lblPU7.Name = "lblPU7"
        Me.lblPU7.Size = New System.Drawing.Size(35, 15)
        Me.lblPU7.TabIndex = 27
        Me.lblPU7.Text = "XXXX"
        Me.lblPU7.TextAlign = System.Drawing.ContentAlignment.BottomRight
        Me.lblPU7.UseMnemonic = False
        '
        'lblFlete1
        '
        Me.lblFlete1.AutoSize = True
        Me.lblFlete1.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblFlete1.Location = New System.Drawing.Point(691, 243)
        Me.lblFlete1.Name = "lblFlete1"
        Me.lblFlete1.Size = New System.Drawing.Size(35, 15)
        Me.lblFlete1.TabIndex = 31
        Me.lblFlete1.Text = "XXXX"
        Me.lblFlete1.TextAlign = System.Drawing.ContentAlignment.BottomRight
        Me.lblFlete1.UseMnemonic = False
        '
        'lblFlete2
        '
        Me.lblFlete2.AutoSize = True
        Me.lblFlete2.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblFlete2.Location = New System.Drawing.Point(691, 261)
        Me.lblFlete2.Name = "lblFlete2"
        Me.lblFlete2.Size = New System.Drawing.Size(35, 15)
        Me.lblFlete2.TabIndex = 32
        Me.lblFlete2.Text = "XXXX"
        Me.lblFlete2.TextAlign = System.Drawing.ContentAlignment.BottomRight
        Me.lblFlete2.UseMnemonic = False
        '
        'lblFlete3
        '
        Me.lblFlete3.AutoSize = True
        Me.lblFlete3.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblFlete3.Location = New System.Drawing.Point(691, 281)
        Me.lblFlete3.Name = "lblFlete3"
        Me.lblFlete3.Size = New System.Drawing.Size(35, 15)
        Me.lblFlete3.TabIndex = 33
        Me.lblFlete3.Text = "XXXX"
        Me.lblFlete3.TextAlign = System.Drawing.ContentAlignment.BottomRight
        Me.lblFlete3.UseMnemonic = False
        '
        'lblFlete4
        '
        Me.lblFlete4.AutoSize = True
        Me.lblFlete4.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblFlete4.Location = New System.Drawing.Point(691, 301)
        Me.lblFlete4.Name = "lblFlete4"
        Me.lblFlete4.Size = New System.Drawing.Size(35, 15)
        Me.lblFlete4.TabIndex = 34
        Me.lblFlete4.Text = "XXXX"
        Me.lblFlete4.TextAlign = System.Drawing.ContentAlignment.BottomRight
        Me.lblFlete4.UseMnemonic = False
        '
        'lblFlete5
        '
        Me.lblFlete5.AutoSize = True
        Me.lblFlete5.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblFlete5.Location = New System.Drawing.Point(691, 319)
        Me.lblFlete5.Name = "lblFlete5"
        Me.lblFlete5.Size = New System.Drawing.Size(35, 15)
        Me.lblFlete5.TabIndex = 35
        Me.lblFlete5.Text = "XXXX"
        Me.lblFlete5.TextAlign = System.Drawing.ContentAlignment.BottomRight
        Me.lblFlete5.UseMnemonic = False
        '
        'lblFlete6
        '
        Me.lblFlete6.AutoSize = True
        Me.lblFlete6.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblFlete6.Location = New System.Drawing.Point(691, 338)
        Me.lblFlete6.Name = "lblFlete6"
        Me.lblFlete6.Size = New System.Drawing.Size(35, 15)
        Me.lblFlete6.TabIndex = 36
        Me.lblFlete6.Text = "XXXX"
        Me.lblFlete6.TextAlign = System.Drawing.ContentAlignment.BottomRight
        Me.lblFlete6.UseMnemonic = False
        '
        'lblFlete7
        '
        Me.lblFlete7.AutoSize = True
        Me.lblFlete7.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblFlete7.Location = New System.Drawing.Point(691, 358)
        Me.lblFlete7.Name = "lblFlete7"
        Me.lblFlete7.Size = New System.Drawing.Size(35, 15)
        Me.lblFlete7.TabIndex = 37
        Me.lblFlete7.Text = "XXXX"
        Me.lblFlete7.TextAlign = System.Drawing.ContentAlignment.BottomRight
        Me.lblFlete7.UseMnemonic = False
        '
        'lblVVenta
        '
        Me.lblVVenta.AutoSize = True
        Me.lblVVenta.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblVVenta.Location = New System.Drawing.Point(691, 406)
        Me.lblVVenta.Name = "lblVVenta"
        Me.lblVVenta.Size = New System.Drawing.Size(35, 15)
        Me.lblVVenta.TabIndex = 0
        Me.lblVVenta.Text = "XXXX"
        Me.lblVVenta.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.lblVVenta.UseMnemonic = False
        '
        'lblIGV
        '
        Me.lblIGV.AutoSize = True
        Me.lblIGV.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblIGV.Location = New System.Drawing.Point(691, 429)
        Me.lblIGV.Name = "lblIGV"
        Me.lblIGV.Size = New System.Drawing.Size(35, 15)
        Me.lblIGV.TabIndex = 0
        Me.lblIGV.Text = "XXXX"
        Me.lblIGV.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.lblIGV.UseMnemonic = False
        '
        'lblTotal
        '
        Me.lblTotal.AutoSize = True
        Me.lblTotal.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTotal.Location = New System.Drawing.Point(691, 456)
        Me.lblTotal.Name = "lblTotal"
        Me.lblTotal.Size = New System.Drawing.Size(35, 15)
        Me.lblTotal.TabIndex = 0
        Me.lblTotal.Text = "XXXX"
        Me.lblTotal.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.lblTotal.UseMnemonic = False
        '
        'lblPorcentajeIGV
        '
        Me.lblPorcentajeIGV.AutoSize = True
        Me.lblPorcentajeIGV.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPorcentajeIGV.Location = New System.Drawing.Point(615, 429)
        Me.lblPorcentajeIGV.Name = "lblPorcentajeIGV"
        Me.lblPorcentajeIGV.Size = New System.Drawing.Size(35, 15)
        Me.lblPorcentajeIGV.TabIndex = 0
        Me.lblPorcentajeIGV.Text = "XXXX"
        Me.lblPorcentajeIGV.UseMnemonic = False
        '
        'lblLetras
        '
        Me.lblLetras.AutoSize = True
        Me.lblLetras.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblLetras.Location = New System.Drawing.Point(66, 435)
        Me.lblLetras.Name = "lblLetras"
        Me.lblLetras.Size = New System.Drawing.Size(35, 15)
        Me.lblLetras.TabIndex = 0
        Me.lblLetras.Text = "XXXX"
        Me.lblLetras.UseMnemonic = False
        '
        'lblRemitente
        '
        Me.lblRemitente.AutoSize = True
        Me.lblRemitente.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblRemitente.Location = New System.Drawing.Point(89, 458)
        Me.lblRemitente.Name = "lblRemitente"
        Me.lblRemitente.Size = New System.Drawing.Size(35, 15)
        Me.lblRemitente.TabIndex = 0
        Me.lblRemitente.Text = "XXXX"
        Me.lblRemitente.UseMnemonic = False
        '
        'lblTransportista
        '
        Me.lblTransportista.AutoSize = True
        Me.lblTransportista.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTransportista.Location = New System.Drawing.Point(114, 479)
        Me.lblTransportista.Name = "lblTransportista"
        Me.lblTransportista.Size = New System.Drawing.Size(35, 15)
        Me.lblTransportista.TabIndex = 0
        Me.lblTransportista.Text = "XXXX"
        Me.lblTransportista.UseMnemonic = False
        '
        'lblTranspRUC
        '
        Me.lblTranspRUC.AutoSize = True
        Me.lblTranspRUC.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTranspRUC.Location = New System.Drawing.Point(67, 500)
        Me.lblTranspRUC.Name = "lblTranspRUC"
        Me.lblTranspRUC.Size = New System.Drawing.Size(35, 15)
        Me.lblTranspRUC.TabIndex = 0
        Me.lblTranspRUC.Text = "XXXX"
        Me.lblTranspRUC.UseMnemonic = False
        '
        'lblDestino
        '
        Me.lblDestino.AutoSize = True
        Me.lblDestino.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDestino.Location = New System.Drawing.Point(344, 478)
        Me.lblDestino.Name = "lblDestino"
        Me.lblDestino.Size = New System.Drawing.Size(35, 15)
        Me.lblDestino.TabIndex = 0
        Me.lblDestino.Text = "XXXX"
        Me.lblDestino.UseMnemonic = False
        '
        'lblManifiesto
        '
        Me.lblManifiesto.AutoSize = True
        Me.lblManifiesto.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblManifiesto.Location = New System.Drawing.Point(325, 456)
        Me.lblManifiesto.Name = "lblManifiesto"
        Me.lblManifiesto.Size = New System.Drawing.Size(35, 15)
        Me.lblManifiesto.TabIndex = 0
        Me.lblManifiesto.Text = "XXXX"
        Me.lblManifiesto.UseMnemonic = False
        '
        'btnEditar
        '
        Me.btnEditar.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnEditar.BackColor = System.Drawing.SystemColors.Control
        Me.btnEditar.Cursor = System.Windows.Forms.Cursors.Default
        Me.btnEditar.ForeColor = System.Drawing.SystemColors.ControlText
        Me.btnEditar.Location = New System.Drawing.Point(178, 12)
        Me.btnEditar.Name = "btnEditar"
        Me.btnEditar.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.btnEditar.Size = New System.Drawing.Size(62, 48)
        Me.btnEditar.TabIndex = 48
        Me.btnEditar.Text = "Editar"
        Me.btnEditar.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.btnEditar.UseVisualStyleBackColor = False
        '
        'ImageList1
        '
        Me.ImageList1.ImageStream = CType(resources.GetObject("ImageList1.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.ImageList1.TransparentColor = System.Drawing.Color.Transparent
        Me.ImageList1.Images.SetKeyName(0, "Guardar.bmp")
        Me.ImageList1.Images.SetKeyName(1, "Editar.bmp")
        '
        'PrintForm1
        '
        Me.PrintForm1.DocumentName = "document"
        Me.PrintForm1.Form = Me
        Me.PrintForm1.PrintAction = System.Drawing.Printing.PrintAction.PrintToPrinter
        Me.PrintForm1.PrinterSettings = CType(resources.GetObject("PrintForm1.PrinterSettings"), System.Drawing.Printing.PrinterSettings)
        Me.PrintForm1.PrintFileName = Nothing
        '
        'gbIncremento
        '
        Me.gbIncremento.BackColor = System.Drawing.Color.White
        Me.gbIncremento.Controls.Add(Me.cbFonts)
        Me.gbIncremento.Controls.Add(Me.chkBold)
        Me.gbIncremento.Controls.Add(Me.nudSize)
        Me.gbIncremento.Controls.Add(Me.tbIncremento)
        Me.gbIncremento.Controls.Add(Me.lblPosicion)
        Me.gbIncremento.Location = New System.Drawing.Point(464, 9)
        Me.gbIncremento.Name = "gbIncremento"
        Me.gbIncremento.Size = New System.Drawing.Size(326, 103)
        Me.gbIncremento.TabIndex = 50
        Me.gbIncremento.TabStop = False
        Me.gbIncremento.Text = "Incremento de desplazamiento"
        Me.gbIncremento.Visible = False
        '
        'cbFonts
        '
        Me.cbFonts.FormattingEnabled = True
        Me.cbFonts.Location = New System.Drawing.Point(11, 76)
        Me.cbFonts.Name = "cbFonts"
        Me.cbFonts.Size = New System.Drawing.Size(148, 21)
        Me.cbFonts.TabIndex = 52
        '
        'chkBold
        '
        Me.chkBold.AutoSize = True
        Me.chkBold.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkBold.Location = New System.Drawing.Point(241, 77)
        Me.chkBold.Name = "chkBold"
        Me.chkBold.Size = New System.Drawing.Size(67, 17)
        Me.chkBold.TabIndex = 51
        Me.chkBold.Text = "Negrita"
        Me.chkBold.UseVisualStyleBackColor = True
        '
        'nudSize
        '
        Me.nudSize.CausesValidation = False
        Me.nudSize.InterceptArrowKeys = False
        Me.nudSize.Location = New System.Drawing.Point(165, 76)
        Me.nudSize.Maximum = New Decimal(New Integer() {20, 0, 0, 0})
        Me.nudSize.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
        Me.nudSize.Name = "nudSize"
        Me.nudSize.Size = New System.Drawing.Size(66, 20)
        Me.nudSize.TabIndex = 50
        Me.nudSize.Value = New Decimal(New Integer() {9, 0, 0, 0})
        '
        'tbIncremento
        '
        Me.tbIncremento.AutoSize = False
        Me.tbIncremento.LargeChange = 1
        Me.tbIncremento.Location = New System.Drawing.Point(11, 18)
        Me.tbIncremento.Maximum = 20
        Me.tbIncremento.Minimum = 1
        Me.tbIncremento.Name = "tbIncremento"
        Me.tbIncremento.Size = New System.Drawing.Size(298, 29)
        Me.tbIncremento.TabIndex = 49
        Me.tbIncremento.Value = 1
        '
        'lblPosicion
        '
        Me.lblPosicion.BackColor = System.Drawing.Color.Red
        Me.lblPosicion.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPosicion.ForeColor = System.Drawing.Color.White
        Me.lblPosicion.Location = New System.Drawing.Point(10, 49)
        Me.lblPosicion.Name = "lblPosicion"
        Me.lblPosicion.Size = New System.Drawing.Size(301, 23)
        Me.lblPosicion.TabIndex = 48
        Me.lblPosicion.Text = "[Posición]"
        Me.lblPosicion.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(664, 406)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(21, 15)
        Me.Label1.TabIndex = 51
        Me.Label1.Text = "S/"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(664, 456)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(21, 15)
        Me.Label2.TabIndex = 51
        Me.Label2.Text = "S/"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(664, 243)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(21, 15)
        Me.Label3.TabIndex = 51
        Me.Label3.Text = "S/"
        '
        'frmImprimirFactura
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.White
        Me.ClientSize = New System.Drawing.Size(823, 553)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.gbIncremento)
        Me.Controls.Add(Me.btnEditar)
        Me.Controls.Add(Me.btnCancelar)
        Me.Controls.Add(Me.btnImprimir)
        Me.Controls.Add(Me.lblTranspRUC)
        Me.Controls.Add(Me.lblManifiesto)
        Me.Controls.Add(Me.lblDestino)
        Me.Controls.Add(Me.lblTransportista)
        Me.Controls.Add(Me.lblRemitente)
        Me.Controls.Add(Me.lblLetras)
        Me.Controls.Add(Me.lblGRT)
        Me.Controls.Add(Me.lblTotal)
        Me.Controls.Add(Me.lblIGV)
        Me.Controls.Add(Me.lblVVenta)
        Me.Controls.Add(Me.lblFlete7)
        Me.Controls.Add(Me.lblPorcentajeIGV)
        Me.Controls.Add(Me.lblPU7)
        Me.Controls.Add(Me.lblDescripcion7)
        Me.Controls.Add(Me.lblCantidad7)
        Me.Controls.Add(Me.lblFlete6)
        Me.Controls.Add(Me.lblPU6)
        Me.Controls.Add(Me.lblDescripcion6)
        Me.Controls.Add(Me.lblCantidad6)
        Me.Controls.Add(Me.lblFlete5)
        Me.Controls.Add(Me.lblPU5)
        Me.Controls.Add(Me.lblDescripcion5)
        Me.Controls.Add(Me.lblCantidad5)
        Me.Controls.Add(Me.lblFlete4)
        Me.Controls.Add(Me.lblPU4)
        Me.Controls.Add(Me.lblDescripcion4)
        Me.Controls.Add(Me.lblCantidad4)
        Me.Controls.Add(Me.lblFlete3)
        Me.Controls.Add(Me.lblPU3)
        Me.Controls.Add(Me.lblDescripcion3)
        Me.Controls.Add(Me.lblCantidad3)
        Me.Controls.Add(Me.lblFlete2)
        Me.Controls.Add(Me.lblPU2)
        Me.Controls.Add(Me.lblDescripcion2)
        Me.Controls.Add(Me.lblCantidad2)
        Me.Controls.Add(Me.lblFlete1)
        Me.Controls.Add(Me.lblPU1)
        Me.Controls.Add(Me.lblDescripcion1)
        Me.Controls.Add(Me.lblCantidad1)
        Me.Controls.Add(Me.lblRUC)
        Me.Controls.Add(Me.lblDireccion)
        Me.Controls.Add(Me.lblCliente)
        Me.Controls.Add(Me.lblAño)
        Me.Controls.Add(Me.lblMes)
        Me.Controls.Add(Me.lblDia)
        Me.Controls.Add(Me.PictureBox1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.KeyPreview = True
        Me.MaximizeBox = False
        Me.Name = "frmImprimirFactura"
        Me.Text = "Vista Previa de Impresión"
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.gbIncremento.ResumeLayout(False)
        Me.gbIncremento.PerformLayout()
        CType(Me.nudSize, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.tbIncremento, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents PictureBox1 As System.Windows.Forms.PictureBox
    Friend WithEvents lblDia As System.Windows.Forms.Label
    Friend WithEvents lblMes As System.Windows.Forms.Label
    Friend WithEvents lblAño As System.Windows.Forms.Label
    Friend WithEvents lblCliente As System.Windows.Forms.Label
    Friend WithEvents lblDireccion As System.Windows.Forms.Label
    Friend WithEvents lblRUC As System.Windows.Forms.Label
    Friend WithEvents lblCantidad1 As System.Windows.Forms.Label
    Friend WithEvents lblCantidad2 As System.Windows.Forms.Label
    Friend WithEvents lblCantidad3 As System.Windows.Forms.Label
    Friend WithEvents lblCantidad4 As System.Windows.Forms.Label
    Friend WithEvents lblCantidad5 As System.Windows.Forms.Label
    Friend WithEvents lblGRT As System.Windows.Forms.Label
    Public WithEvents btnCancelar As System.Windows.Forms.Button
    Public WithEvents btnImprimir As System.Windows.Forms.Button
    Friend WithEvents lblDescripcion7 As System.Windows.Forms.Label
    Friend WithEvents lblCantidad7 As System.Windows.Forms.Label
    Friend WithEvents lblDescripcion6 As System.Windows.Forms.Label
    Friend WithEvents lblCantidad6 As System.Windows.Forms.Label
    Friend WithEvents lblDescripcion5 As System.Windows.Forms.Label
    Friend WithEvents lblDescripcion4 As System.Windows.Forms.Label
    Friend WithEvents lblDescripcion3 As System.Windows.Forms.Label
    Friend WithEvents lblDescripcion2 As System.Windows.Forms.Label
    Friend WithEvents lblDescripcion1 As System.Windows.Forms.Label
    Friend WithEvents lblPU7 As System.Windows.Forms.Label
    Friend WithEvents lblPU6 As System.Windows.Forms.Label
    Friend WithEvents lblPU5 As System.Windows.Forms.Label
    Friend WithEvents lblPU4 As System.Windows.Forms.Label
    Friend WithEvents lblPU3 As System.Windows.Forms.Label
    Friend WithEvents lblPU2 As System.Windows.Forms.Label
    Friend WithEvents lblPU1 As System.Windows.Forms.Label
    Friend WithEvents lblTranspRUC As System.Windows.Forms.Label
    Friend WithEvents lblManifiesto As System.Windows.Forms.Label
    Friend WithEvents lblDestino As System.Windows.Forms.Label
    Friend WithEvents lblTransportista As System.Windows.Forms.Label
    Friend WithEvents lblRemitente As System.Windows.Forms.Label
    Friend WithEvents lblLetras As System.Windows.Forms.Label
    Friend WithEvents lblTotal As System.Windows.Forms.Label
    Friend WithEvents lblIGV As System.Windows.Forms.Label
    Friend WithEvents lblVVenta As System.Windows.Forms.Label
    Friend WithEvents lblFlete7 As System.Windows.Forms.Label
    Friend WithEvents lblPorcentajeIGV As System.Windows.Forms.Label
    Friend WithEvents lblFlete6 As System.Windows.Forms.Label
    Friend WithEvents lblFlete5 As System.Windows.Forms.Label
    Friend WithEvents lblFlete4 As System.Windows.Forms.Label
    Friend WithEvents lblFlete3 As System.Windows.Forms.Label
    Friend WithEvents lblFlete2 As System.Windows.Forms.Label
    Friend WithEvents lblFlete1 As System.Windows.Forms.Label
    Public WithEvents btnEditar As System.Windows.Forms.Button
    Friend WithEvents ImageList1 As System.Windows.Forms.ImageList
    Friend WithEvents PrintForm1 As Microsoft.VisualBasic.PowerPacks.Printing.PrintForm
    Friend WithEvents gbIncremento As System.Windows.Forms.GroupBox
    Friend WithEvents cbFonts As System.Windows.Forms.ComboBox
    Friend WithEvents chkBold As System.Windows.Forms.CheckBox
    Friend WithEvents nudSize As System.Windows.Forms.NumericUpDown
    Friend WithEvents tbIncremento As System.Windows.Forms.TrackBar
    Friend WithEvents lblPosicion As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
End Class
