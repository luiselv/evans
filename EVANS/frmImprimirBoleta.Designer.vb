<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmImprimirBoleta
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmImprimirBoleta))
        Me.PictureBox1 = New System.Windows.Forms.PictureBox
        Me.btnCancelar = New System.Windows.Forms.Button
        Me.btnImprimir = New System.Windows.Forms.Button
        Me.lblDia = New System.Windows.Forms.Label
        Me.lblMes = New System.Windows.Forms.Label
        Me.lblAño = New System.Windows.Forms.Label
        Me.lblCliente = New System.Windows.Forms.Label
        Me.lblDireccion = New System.Windows.Forms.Label
        Me.lblDNI = New System.Windows.Forms.Label
        Me.lblCantidad1 = New System.Windows.Forms.Label
        Me.lblCantidad2 = New System.Windows.Forms.Label
        Me.lblCantidad3 = New System.Windows.Forms.Label
        Me.lblCantidad4 = New System.Windows.Forms.Label
        Me.lblCantidad5 = New System.Windows.Forms.Label
        Me.lblCantidad6 = New System.Windows.Forms.Label
        Me.lblCantidad7 = New System.Windows.Forms.Label
        Me.lblDesc1 = New System.Windows.Forms.Label
        Me.lblDesc2 = New System.Windows.Forms.Label
        Me.lblDesc3 = New System.Windows.Forms.Label
        Me.lblDesc4 = New System.Windows.Forms.Label
        Me.lblDesc5 = New System.Windows.Forms.Label
        Me.lblDesc6 = New System.Windows.Forms.Label
        Me.lblDesc7 = New System.Windows.Forms.Label
        Me.lblFlete1 = New System.Windows.Forms.Label
        Me.lblFlete2 = New System.Windows.Forms.Label
        Me.lblFlete3 = New System.Windows.Forms.Label
        Me.lblFlete4 = New System.Windows.Forms.Label
        Me.lblFlete5 = New System.Windows.Forms.Label
        Me.lblFlete6 = New System.Windows.Forms.Label
        Me.lblFlete7 = New System.Windows.Forms.Label
        Me.lblTotal = New System.Windows.Forms.Label
        Me.lblGRT = New System.Windows.Forms.Label
        Me.btnEditar = New System.Windows.Forms.Button
        Me.PrintForm1 = New Microsoft.VisualBasic.PowerPacks.Printing.PrintForm(Me.components)
        Me.ImageList1 = New System.Windows.Forms.ImageList(Me.components)
        Me.gbIncremento = New System.Windows.Forms.GroupBox
        Me.cbFonts = New System.Windows.Forms.ComboBox
        Me.chkBold = New System.Windows.Forms.CheckBox
        Me.nudSize = New System.Windows.Forms.NumericUpDown
        Me.tbIncremento = New System.Windows.Forms.TrackBar
        Me.lblPosicion = New System.Windows.Forms.Label
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.gbIncremento.SuspendLayout()
        CType(Me.nudSize, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.tbIncremento, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'PictureBox1
        '
        Me.PictureBox1.Image = CType(resources.GetObject("PictureBox1.Image"), System.Drawing.Image)
        Me.PictureBox1.Location = New System.Drawing.Point(-25, -50)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(795, 607)
        Me.PictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.PictureBox1.TabIndex = 0
        Me.PictureBox1.TabStop = False
        '
        'btnCancelar
        '
        Me.btnCancelar.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnCancelar.BackColor = System.Drawing.SystemColors.Control
        Me.btnCancelar.Cursor = System.Windows.Forms.Cursors.Default
        Me.btnCancelar.ForeColor = System.Drawing.SystemColors.ControlText
        Me.btnCancelar.Image = CType(resources.GetObject("btnCancelar.Image"), System.Drawing.Image)
        Me.btnCancelar.Location = New System.Drawing.Point(224, 12)
        Me.btnCancelar.Name = "btnCancelar"
        Me.btnCancelar.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.btnCancelar.Size = New System.Drawing.Size(62, 48)
        Me.btnCancelar.TabIndex = 44
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
        Me.btnImprimir.Location = New System.Drawing.Point(88, 12)
        Me.btnImprimir.Name = "btnImprimir"
        Me.btnImprimir.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.btnImprimir.Size = New System.Drawing.Size(62, 48)
        Me.btnImprimir.TabIndex = 43
        Me.btnImprimir.Text = "Imprimir"
        Me.btnImprimir.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.btnImprimir.UseVisualStyleBackColor = False
        '
        'lblDia
        '
        Me.lblDia.AutoSize = True
        Me.lblDia.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDia.Location = New System.Drawing.Point(56, 141)
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
        Me.lblMes.Location = New System.Drawing.Point(134, 141)
        Me.lblMes.Name = "lblMes"
        Me.lblMes.Size = New System.Drawing.Size(35, 15)
        Me.lblMes.TabIndex = 0
        Me.lblMes.Text = "XXXX"
        Me.lblMes.UseMnemonic = False
        '
        'lblAño
        '
        Me.lblAño.AutoSize = True
        Me.lblAño.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblAño.Location = New System.Drawing.Point(250, 141)
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
        Me.lblCliente.Location = New System.Drawing.Point(79, 166)
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
        Me.lblDireccion.Location = New System.Drawing.Point(79, 194)
        Me.lblDireccion.Name = "lblDireccion"
        Me.lblDireccion.Size = New System.Drawing.Size(35, 15)
        Me.lblDireccion.TabIndex = 0
        Me.lblDireccion.Text = "XXXX"
        Me.lblDireccion.UseMnemonic = False
        '
        'lblDNI
        '
        Me.lblDNI.AutoSize = True
        Me.lblDNI.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDNI.Location = New System.Drawing.Point(520, 194)
        Me.lblDNI.Name = "lblDNI"
        Me.lblDNI.Size = New System.Drawing.Size(35, 15)
        Me.lblDNI.TabIndex = 0
        Me.lblDNI.Text = "XXXX"
        Me.lblDNI.UseMnemonic = False
        '
        'lblCantidad1
        '
        Me.lblCantidad1.AutoSize = True
        Me.lblCantidad1.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCantidad1.Location = New System.Drawing.Point(29, 256)
        Me.lblCantidad1.Name = "lblCantidad1"
        Me.lblCantidad1.Size = New System.Drawing.Size(35, 15)
        Me.lblCantidad1.TabIndex = 1
        Me.lblCantidad1.Text = "XXXX"
        Me.lblCantidad1.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.lblCantidad1.UseMnemonic = False
        '
        'lblCantidad2
        '
        Me.lblCantidad2.AutoSize = True
        Me.lblCantidad2.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCantidad2.Location = New System.Drawing.Point(29, 281)
        Me.lblCantidad2.Name = "lblCantidad2"
        Me.lblCantidad2.Size = New System.Drawing.Size(35, 15)
        Me.lblCantidad2.TabIndex = 2
        Me.lblCantidad2.Text = "XXXX"
        Me.lblCantidad2.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.lblCantidad2.UseMnemonic = False
        '
        'lblCantidad3
        '
        Me.lblCantidad3.AutoSize = True
        Me.lblCantidad3.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCantidad3.Location = New System.Drawing.Point(29, 306)
        Me.lblCantidad3.Name = "lblCantidad3"
        Me.lblCantidad3.Size = New System.Drawing.Size(35, 15)
        Me.lblCantidad3.TabIndex = 3
        Me.lblCantidad3.Text = "XXXX"
        Me.lblCantidad3.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.lblCantidad3.UseMnemonic = False
        '
        'lblCantidad4
        '
        Me.lblCantidad4.AutoSize = True
        Me.lblCantidad4.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCantidad4.Location = New System.Drawing.Point(29, 331)
        Me.lblCantidad4.Name = "lblCantidad4"
        Me.lblCantidad4.Size = New System.Drawing.Size(35, 15)
        Me.lblCantidad4.TabIndex = 4
        Me.lblCantidad4.Text = "XXXX"
        Me.lblCantidad4.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.lblCantidad4.UseMnemonic = False
        '
        'lblCantidad5
        '
        Me.lblCantidad5.AutoSize = True
        Me.lblCantidad5.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCantidad5.Location = New System.Drawing.Point(29, 356)
        Me.lblCantidad5.Name = "lblCantidad5"
        Me.lblCantidad5.Size = New System.Drawing.Size(35, 15)
        Me.lblCantidad5.TabIndex = 5
        Me.lblCantidad5.Text = "XXXX"
        Me.lblCantidad5.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.lblCantidad5.UseMnemonic = False
        '
        'lblCantidad6
        '
        Me.lblCantidad6.AutoSize = True
        Me.lblCantidad6.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCantidad6.Location = New System.Drawing.Point(29, 381)
        Me.lblCantidad6.Name = "lblCantidad6"
        Me.lblCantidad6.Size = New System.Drawing.Size(35, 15)
        Me.lblCantidad6.TabIndex = 6
        Me.lblCantidad6.Text = "XXXX"
        Me.lblCantidad6.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.lblCantidad6.UseMnemonic = False
        '
        'lblCantidad7
        '
        Me.lblCantidad7.AutoSize = True
        Me.lblCantidad7.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCantidad7.Location = New System.Drawing.Point(29, 406)
        Me.lblCantidad7.Name = "lblCantidad7"
        Me.lblCantidad7.Size = New System.Drawing.Size(35, 15)
        Me.lblCantidad7.TabIndex = 7
        Me.lblCantidad7.Text = "XXXX"
        Me.lblCantidad7.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.lblCantidad7.UseMnemonic = False
        '
        'lblDesc1
        '
        Me.lblDesc1.AutoSize = True
        Me.lblDesc1.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDesc1.Location = New System.Drawing.Point(99, 256)
        Me.lblDesc1.Name = "lblDesc1"
        Me.lblDesc1.Size = New System.Drawing.Size(35, 15)
        Me.lblDesc1.TabIndex = 11
        Me.lblDesc1.Text = "XXXX"
        Me.lblDesc1.UseMnemonic = False
        '
        'lblDesc2
        '
        Me.lblDesc2.AutoSize = True
        Me.lblDesc2.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDesc2.Location = New System.Drawing.Point(99, 281)
        Me.lblDesc2.Name = "lblDesc2"
        Me.lblDesc2.Size = New System.Drawing.Size(35, 15)
        Me.lblDesc2.TabIndex = 12
        Me.lblDesc2.Text = "XXXX"
        Me.lblDesc2.UseMnemonic = False
        '
        'lblDesc3
        '
        Me.lblDesc3.AutoSize = True
        Me.lblDesc3.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDesc3.Location = New System.Drawing.Point(99, 306)
        Me.lblDesc3.Name = "lblDesc3"
        Me.lblDesc3.Size = New System.Drawing.Size(35, 15)
        Me.lblDesc3.TabIndex = 13
        Me.lblDesc3.Text = "XXXX"
        Me.lblDesc3.UseMnemonic = False
        '
        'lblDesc4
        '
        Me.lblDesc4.AutoSize = True
        Me.lblDesc4.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDesc4.Location = New System.Drawing.Point(99, 331)
        Me.lblDesc4.Name = "lblDesc4"
        Me.lblDesc4.Size = New System.Drawing.Size(35, 15)
        Me.lblDesc4.TabIndex = 14
        Me.lblDesc4.Text = "XXXX"
        Me.lblDesc4.UseMnemonic = False
        '
        'lblDesc5
        '
        Me.lblDesc5.AutoSize = True
        Me.lblDesc5.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDesc5.Location = New System.Drawing.Point(99, 356)
        Me.lblDesc5.Name = "lblDesc5"
        Me.lblDesc5.Size = New System.Drawing.Size(35, 15)
        Me.lblDesc5.TabIndex = 15
        Me.lblDesc5.Text = "XXXX"
        Me.lblDesc5.UseMnemonic = False
        '
        'lblDesc6
        '
        Me.lblDesc6.AutoSize = True
        Me.lblDesc6.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDesc6.Location = New System.Drawing.Point(99, 381)
        Me.lblDesc6.Name = "lblDesc6"
        Me.lblDesc6.Size = New System.Drawing.Size(35, 15)
        Me.lblDesc6.TabIndex = 16
        Me.lblDesc6.Text = "XXXX"
        Me.lblDesc6.UseMnemonic = False
        '
        'lblDesc7
        '
        Me.lblDesc7.AutoSize = True
        Me.lblDesc7.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDesc7.Location = New System.Drawing.Point(99, 406)
        Me.lblDesc7.Name = "lblDesc7"
        Me.lblDesc7.Size = New System.Drawing.Size(35, 15)
        Me.lblDesc7.TabIndex = 17
        Me.lblDesc7.Text = "XXXX"
        Me.lblDesc7.UseMnemonic = False
        '
        'lblFlete1
        '
        Me.lblFlete1.AutoSize = True
        Me.lblFlete1.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblFlete1.Location = New System.Drawing.Point(667, 256)
        Me.lblFlete1.Name = "lblFlete1"
        Me.lblFlete1.Size = New System.Drawing.Size(35, 15)
        Me.lblFlete1.TabIndex = 21
        Me.lblFlete1.Text = "XXXX"
        Me.lblFlete1.TextAlign = System.Drawing.ContentAlignment.BottomRight
        Me.lblFlete1.UseMnemonic = False
        '
        'lblFlete2
        '
        Me.lblFlete2.AutoSize = True
        Me.lblFlete2.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblFlete2.Location = New System.Drawing.Point(667, 281)
        Me.lblFlete2.Name = "lblFlete2"
        Me.lblFlete2.Size = New System.Drawing.Size(35, 15)
        Me.lblFlete2.TabIndex = 22
        Me.lblFlete2.Text = "XXXX"
        Me.lblFlete2.TextAlign = System.Drawing.ContentAlignment.BottomRight
        Me.lblFlete2.UseMnemonic = False
        '
        'lblFlete3
        '
        Me.lblFlete3.AutoSize = True
        Me.lblFlete3.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblFlete3.Location = New System.Drawing.Point(667, 306)
        Me.lblFlete3.Name = "lblFlete3"
        Me.lblFlete3.Size = New System.Drawing.Size(35, 15)
        Me.lblFlete3.TabIndex = 23
        Me.lblFlete3.Text = "XXXX"
        Me.lblFlete3.TextAlign = System.Drawing.ContentAlignment.BottomRight
        Me.lblFlete3.UseMnemonic = False
        '
        'lblFlete4
        '
        Me.lblFlete4.AutoSize = True
        Me.lblFlete4.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblFlete4.Location = New System.Drawing.Point(667, 331)
        Me.lblFlete4.Name = "lblFlete4"
        Me.lblFlete4.Size = New System.Drawing.Size(35, 15)
        Me.lblFlete4.TabIndex = 24
        Me.lblFlete4.Text = "XXXX"
        Me.lblFlete4.TextAlign = System.Drawing.ContentAlignment.BottomRight
        Me.lblFlete4.UseMnemonic = False
        '
        'lblFlete5
        '
        Me.lblFlete5.AutoSize = True
        Me.lblFlete5.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblFlete5.Location = New System.Drawing.Point(667, 356)
        Me.lblFlete5.Name = "lblFlete5"
        Me.lblFlete5.Size = New System.Drawing.Size(35, 15)
        Me.lblFlete5.TabIndex = 25
        Me.lblFlete5.Text = "XXXX"
        Me.lblFlete5.TextAlign = System.Drawing.ContentAlignment.BottomRight
        Me.lblFlete5.UseMnemonic = False
        '
        'lblFlete6
        '
        Me.lblFlete6.AutoSize = True
        Me.lblFlete6.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblFlete6.Location = New System.Drawing.Point(667, 381)
        Me.lblFlete6.Name = "lblFlete6"
        Me.lblFlete6.Size = New System.Drawing.Size(35, 15)
        Me.lblFlete6.TabIndex = 26
        Me.lblFlete6.Text = "XXXX"
        Me.lblFlete6.TextAlign = System.Drawing.ContentAlignment.BottomRight
        Me.lblFlete6.UseMnemonic = False
        '
        'lblFlete7
        '
        Me.lblFlete7.AutoSize = True
        Me.lblFlete7.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblFlete7.Location = New System.Drawing.Point(667, 406)
        Me.lblFlete7.Name = "lblFlete7"
        Me.lblFlete7.Size = New System.Drawing.Size(35, 15)
        Me.lblFlete7.TabIndex = 27
        Me.lblFlete7.Text = "XXXX"
        Me.lblFlete7.TextAlign = System.Drawing.ContentAlignment.BottomRight
        Me.lblFlete7.UseMnemonic = False
        '
        'lblTotal
        '
        Me.lblTotal.AutoSize = True
        Me.lblTotal.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTotal.Location = New System.Drawing.Point(667, 506)
        Me.lblTotal.Name = "lblTotal"
        Me.lblTotal.Size = New System.Drawing.Size(35, 15)
        Me.lblTotal.TabIndex = 0
        Me.lblTotal.Text = "XXXX"
        Me.lblTotal.TextAlign = System.Drawing.ContentAlignment.BottomRight
        Me.lblTotal.UseMnemonic = False
        '
        'lblGRT
        '
        Me.lblGRT.AutoSize = True
        Me.lblGRT.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblGRT.Location = New System.Drawing.Point(99, 459)
        Me.lblGRT.Name = "lblGRT"
        Me.lblGRT.Size = New System.Drawing.Size(35, 15)
        Me.lblGRT.TabIndex = 0
        Me.lblGRT.Text = "XXXX"
        Me.lblGRT.UseMnemonic = False
        '
        'btnEditar
        '
        Me.btnEditar.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnEditar.BackColor = System.Drawing.SystemColors.Control
        Me.btnEditar.Cursor = System.Windows.Forms.Cursors.Default
        Me.btnEditar.ForeColor = System.Drawing.SystemColors.ControlText
        Me.btnEditar.Location = New System.Drawing.Point(156, 12)
        Me.btnEditar.Name = "btnEditar"
        Me.btnEditar.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.btnEditar.Size = New System.Drawing.Size(62, 48)
        Me.btnEditar.TabIndex = 48
        Me.btnEditar.Text = "Editar"
        Me.btnEditar.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.btnEditar.UseVisualStyleBackColor = False
        '
        'PrintForm1
        '
        Me.PrintForm1.DocumentName = "document"
        Me.PrintForm1.Form = Me
        Me.PrintForm1.PrintAction = System.Drawing.Printing.PrintAction.PrintToPrinter
        Me.PrintForm1.PrinterSettings = CType(resources.GetObject("PrintForm1.PrinterSettings"), System.Drawing.Printing.PrinterSettings)
        Me.PrintForm1.PrintFileName = Nothing
        '
        'ImageList1
        '
        Me.ImageList1.ImageStream = CType(resources.GetObject("ImageList1.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.ImageList1.TransparentColor = System.Drawing.Color.Transparent
        Me.ImageList1.Images.SetKeyName(0, "Guardar.bmp")
        Me.ImageList1.Images.SetKeyName(1, "Editar.bmp")
        '
        'gbIncremento
        '
        Me.gbIncremento.BackColor = System.Drawing.Color.White
        Me.gbIncremento.Controls.Add(Me.cbFonts)
        Me.gbIncremento.Controls.Add(Me.chkBold)
        Me.gbIncremento.Controls.Add(Me.nudSize)
        Me.gbIncremento.Controls.Add(Me.tbIncremento)
        Me.gbIncremento.Controls.Add(Me.lblPosicion)
        Me.gbIncremento.Location = New System.Drawing.Point(436, 7)
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
        'frmImprimirBoleta
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.White
        Me.ClientSize = New System.Drawing.Size(774, 562)
        Me.Controls.Add(Me.gbIncremento)
        Me.Controls.Add(Me.btnEditar)
        Me.Controls.Add(Me.lblDNI)
        Me.Controls.Add(Me.lblGRT)
        Me.Controls.Add(Me.lblTotal)
        Me.Controls.Add(Me.lblFlete7)
        Me.Controls.Add(Me.lblDesc7)
        Me.Controls.Add(Me.lblCantidad7)
        Me.Controls.Add(Me.lblFlete6)
        Me.Controls.Add(Me.lblDesc6)
        Me.Controls.Add(Me.lblCantidad6)
        Me.Controls.Add(Me.lblFlete5)
        Me.Controls.Add(Me.lblDesc5)
        Me.Controls.Add(Me.lblCantidad5)
        Me.Controls.Add(Me.lblFlete4)
        Me.Controls.Add(Me.lblDesc4)
        Me.Controls.Add(Me.lblCantidad4)
        Me.Controls.Add(Me.lblFlete3)
        Me.Controls.Add(Me.lblDesc3)
        Me.Controls.Add(Me.lblCantidad3)
        Me.Controls.Add(Me.lblFlete2)
        Me.Controls.Add(Me.lblDesc2)
        Me.Controls.Add(Me.lblCantidad2)
        Me.Controls.Add(Me.lblFlete1)
        Me.Controls.Add(Me.lblDesc1)
        Me.Controls.Add(Me.lblCantidad1)
        Me.Controls.Add(Me.lblDireccion)
        Me.Controls.Add(Me.lblCliente)
        Me.Controls.Add(Me.lblAño)
        Me.Controls.Add(Me.lblMes)
        Me.Controls.Add(Me.lblDia)
        Me.Controls.Add(Me.btnCancelar)
        Me.Controls.Add(Me.btnImprimir)
        Me.Controls.Add(Me.PictureBox1)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.KeyPreview = True
        Me.MaximizeBox = False
        Me.Name = "frmImprimirBoleta"
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
    Public WithEvents btnCancelar As System.Windows.Forms.Button
    Public WithEvents btnImprimir As System.Windows.Forms.Button
    Friend WithEvents lblDia As System.Windows.Forms.Label
    Friend WithEvents lblMes As System.Windows.Forms.Label
    Friend WithEvents lblAño As System.Windows.Forms.Label
    Friend WithEvents lblCliente As System.Windows.Forms.Label
    Friend WithEvents lblDireccion As System.Windows.Forms.Label
    Friend WithEvents lblDNI As System.Windows.Forms.Label
    Friend WithEvents lblCantidad1 As System.Windows.Forms.Label
    Friend WithEvents lblCantidad2 As System.Windows.Forms.Label
    Friend WithEvents lblCantidad3 As System.Windows.Forms.Label
    Friend WithEvents lblCantidad4 As System.Windows.Forms.Label
    Friend WithEvents lblCantidad5 As System.Windows.Forms.Label
    Friend WithEvents lblCantidad6 As System.Windows.Forms.Label
    Friend WithEvents lblCantidad7 As System.Windows.Forms.Label
    Friend WithEvents lblDesc1 As System.Windows.Forms.Label
    Friend WithEvents lblDesc2 As System.Windows.Forms.Label
    Friend WithEvents lblDesc3 As System.Windows.Forms.Label
    Friend WithEvents lblDesc4 As System.Windows.Forms.Label
    Friend WithEvents lblDesc5 As System.Windows.Forms.Label
    Friend WithEvents lblDesc6 As System.Windows.Forms.Label
    Friend WithEvents lblDesc7 As System.Windows.Forms.Label
    Friend WithEvents lblFlete1 As System.Windows.Forms.Label
    Friend WithEvents lblFlete2 As System.Windows.Forms.Label
    Friend WithEvents lblFlete3 As System.Windows.Forms.Label
    Friend WithEvents lblFlete4 As System.Windows.Forms.Label
    Friend WithEvents lblFlete5 As System.Windows.Forms.Label
    Friend WithEvents lblFlete6 As System.Windows.Forms.Label
    Friend WithEvents lblFlete7 As System.Windows.Forms.Label
    Friend WithEvents lblTotal As System.Windows.Forms.Label
    Friend WithEvents lblGRT As System.Windows.Forms.Label
    Public WithEvents btnEditar As System.Windows.Forms.Button
    Friend WithEvents PrintForm1 As Microsoft.VisualBasic.PowerPacks.Printing.PrintForm
    Friend WithEvents ImageList1 As System.Windows.Forms.ImageList
    Friend WithEvents gbIncremento As System.Windows.Forms.GroupBox
    Friend WithEvents cbFonts As System.Windows.Forms.ComboBox
    Friend WithEvents chkBold As System.Windows.Forms.CheckBox
    Friend WithEvents nudSize As System.Windows.Forms.NumericUpDown
    Friend WithEvents tbIncremento As System.Windows.Forms.TrackBar
    Friend WithEvents lblPosicion As System.Windows.Forms.Label
End Class
