<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmImprimirGuia
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmImprimirGuia))
        Me.PictureBox1 = New System.Windows.Forms.PictureBox
        Me.btnCancelar = New System.Windows.Forms.Button
        Me.btnImprimir = New System.Windows.Forms.Button
        Me.lblDia1 = New System.Windows.Forms.Label
        Me.lblMes1 = New System.Windows.Forms.Label
        Me.lblAño1 = New System.Windows.Forms.Label
        Me.lblDia2 = New System.Windows.Forms.Label
        Me.lblMes2 = New System.Windows.Forms.Label
        Me.lblAño2 = New System.Windows.Forms.Label
        Me.lblDireccionPartida = New System.Windows.Forms.Label
        Me.lblCiudadPartida = New System.Windows.Forms.Label
        Me.lblCiudadDestino = New System.Windows.Forms.Label
        Me.lblDireccionDestino = New System.Windows.Forms.Label
        Me.lblRemitente = New System.Windows.Forms.Label
        Me.lblDestinatario = New System.Windows.Forms.Label
        Me.lblRemiRUC = New System.Windows.Forms.Label
        Me.lblDestiRUC = New System.Windows.Forms.Label
        Me.lblRemiDNI = New System.Windows.Forms.Label
        Me.lblDestiDNI = New System.Windows.Forms.Label
        Me.lblCant1 = New System.Windows.Forms.Label
        Me.lblCant2 = New System.Windows.Forms.Label
        Me.lblCant3 = New System.Windows.Forms.Label
        Me.lblCant4 = New System.Windows.Forms.Label
        Me.lblCant5 = New System.Windows.Forms.Label
        Me.lblCant6 = New System.Windows.Forms.Label
        Me.lblGRR = New System.Windows.Forms.Label
        Me.lblDesc1 = New System.Windows.Forms.Label
        Me.lblDesc2 = New System.Windows.Forms.Label
        Me.lblDesc3 = New System.Windows.Forms.Label
        Me.lblDesc4 = New System.Windows.Forms.Label
        Me.lblDesc5 = New System.Windows.Forms.Label
        Me.lblDesc6 = New System.Windows.Forms.Label
        Me.lblPeso1 = New System.Windows.Forms.Label
        Me.lblPeso2 = New System.Windows.Forms.Label
        Me.lblPeso3 = New System.Windows.Forms.Label
        Me.lblPeso4 = New System.Windows.Forms.Label
        Me.lblPeso5 = New System.Windows.Forms.Label
        Me.lblPeso6 = New System.Windows.Forms.Label
        Me.lblUM1 = New System.Windows.Forms.Label
        Me.lblUM2 = New System.Windows.Forms.Label
        Me.lblUM3 = New System.Windows.Forms.Label
        Me.lblUM4 = New System.Windows.Forms.Label
        Me.lblUM5 = New System.Windows.Forms.Label
        Me.lblUM6 = New System.Windows.Forms.Label
        Me.lblCosto1 = New System.Windows.Forms.Label
        Me.lblCosto2 = New System.Windows.Forms.Label
        Me.lblCosto3 = New System.Windows.Forms.Label
        Me.lblCosto4 = New System.Windows.Forms.Label
        Me.lblCosto5 = New System.Windows.Forms.Label
        Me.lblCosto6 = New System.Windows.Forms.Label
        Me.lblDistancia = New System.Windows.Forms.Label
        Me.lblVehiculo = New System.Windows.Forms.Label
        Me.lblPlaca = New System.Windows.Forms.Label
        Me.lblConf = New System.Windows.Forms.Label
        Me.lblCertificado = New System.Windows.Forms.Label
        Me.lblLicencia = New System.Windows.Forms.Label
        Me.lblSubcontratada = New System.Windows.Forms.Label
        Me.lblDireccion = New System.Windows.Forms.Label
        Me.lblRUC = New System.Windows.Forms.Label
        Me.lblObservaciones = New System.Windows.Forms.Label
        Me.PrintForm1 = New Microsoft.VisualBasic.PowerPacks.Printing.PrintForm(Me.components)
        Me.btnEditar = New System.Windows.Forms.Button
        Me.ImageList1 = New System.Windows.Forms.ImageList(Me.components)
        Me.lblPosicion = New System.Windows.Forms.Label
        Me.gbIncremento = New System.Windows.Forms.GroupBox
        Me.cbFonts = New System.Windows.Forms.ComboBox
        Me.chkBold = New System.Windows.Forms.CheckBox
        Me.nudSize = New System.Windows.Forms.NumericUpDown
        Me.tbIncremento = New System.Windows.Forms.TrackBar
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.gbIncremento.SuspendLayout()
        CType(Me.nudSize, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.tbIncremento, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'PictureBox1
        '
        Me.PictureBox1.Image = CType(resources.GetObject("PictureBox1.Image"), System.Drawing.Image)
        Me.PictureBox1.Location = New System.Drawing.Point(-48, -104)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(840, 763)
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
        Me.btnCancelar.Location = New System.Drawing.Point(288, 5)
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
        Me.btnImprimir.Location = New System.Drawing.Point(152, 5)
        Me.btnImprimir.Name = "btnImprimir"
        Me.btnImprimir.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.btnImprimir.Size = New System.Drawing.Size(62, 48)
        Me.btnImprimir.TabIndex = 43
        Me.btnImprimir.Text = "Imprimir"
        Me.btnImprimir.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.btnImprimir.UseVisualStyleBackColor = False
        '
        'lblDia1
        '
        Me.lblDia1.AutoSize = True
        Me.lblDia1.BackColor = System.Drawing.Color.White
        Me.lblDia1.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDia1.Location = New System.Drawing.Point(81, 67)
        Me.lblDia1.Name = "lblDia1"
        Me.lblDia1.Size = New System.Drawing.Size(28, 15)
        Me.lblDia1.TabIndex = 0
        Me.lblDia1.Tag = "Dia1"
        Me.lblDia1.Text = "XXX"
        Me.lblDia1.UseMnemonic = False
        '
        'lblMes1
        '
        Me.lblMes1.AutoSize = True
        Me.lblMes1.BackColor = System.Drawing.Color.White
        Me.lblMes1.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblMes1.Location = New System.Drawing.Point(115, 67)
        Me.lblMes1.Name = "lblMes1"
        Me.lblMes1.Size = New System.Drawing.Size(28, 15)
        Me.lblMes1.TabIndex = 0
        Me.lblMes1.Tag = "Mes1"
        Me.lblMes1.Text = "XXX"
        Me.lblMes1.UseMnemonic = False
        '
        'lblAño1
        '
        Me.lblAño1.AutoSize = True
        Me.lblAño1.BackColor = System.Drawing.Color.White
        Me.lblAño1.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblAño1.Location = New System.Drawing.Point(149, 67)
        Me.lblAño1.Name = "lblAño1"
        Me.lblAño1.Size = New System.Drawing.Size(28, 15)
        Me.lblAño1.TabIndex = 0
        Me.lblAño1.Tag = "Año1"
        Me.lblAño1.Text = "XXX"
        Me.lblAño1.UseMnemonic = False
        '
        'lblDia2
        '
        Me.lblDia2.AutoSize = True
        Me.lblDia2.BackColor = System.Drawing.Color.White
        Me.lblDia2.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDia2.Location = New System.Drawing.Point(314, 67)
        Me.lblDia2.Name = "lblDia2"
        Me.lblDia2.Size = New System.Drawing.Size(28, 15)
        Me.lblDia2.TabIndex = 0
        Me.lblDia2.Tag = "Dia2"
        Me.lblDia2.Text = "XXX"
        Me.lblDia2.UseMnemonic = False
        '
        'lblMes2
        '
        Me.lblMes2.AutoSize = True
        Me.lblMes2.BackColor = System.Drawing.Color.White
        Me.lblMes2.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblMes2.Location = New System.Drawing.Point(348, 67)
        Me.lblMes2.Name = "lblMes2"
        Me.lblMes2.Size = New System.Drawing.Size(28, 15)
        Me.lblMes2.TabIndex = 0
        Me.lblMes2.Tag = "Mes2"
        Me.lblMes2.Text = "XXX"
        Me.lblMes2.UseMnemonic = False
        '
        'lblAño2
        '
        Me.lblAño2.AutoSize = True
        Me.lblAño2.BackColor = System.Drawing.Color.White
        Me.lblAño2.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblAño2.Location = New System.Drawing.Point(382, 67)
        Me.lblAño2.Name = "lblAño2"
        Me.lblAño2.Size = New System.Drawing.Size(28, 15)
        Me.lblAño2.TabIndex = 0
        Me.lblAño2.Tag = "Año2"
        Me.lblAño2.Text = "XXX"
        Me.lblAño2.UseMnemonic = False
        '
        'lblDireccionPartida
        '
        Me.lblDireccionPartida.AutoSize = True
        Me.lblDireccionPartida.BackColor = System.Drawing.Color.White
        Me.lblDireccionPartida.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDireccionPartida.Location = New System.Drawing.Point(10, 106)
        Me.lblDireccionPartida.Name = "lblDireccionPartida"
        Me.lblDireccionPartida.Size = New System.Drawing.Size(28, 15)
        Me.lblDireccionPartida.TabIndex = 0
        Me.lblDireccionPartida.Text = "XXX"
        Me.lblDireccionPartida.UseMnemonic = False
        '
        'lblCiudadPartida
        '
        Me.lblCiudadPartida.AutoSize = True
        Me.lblCiudadPartida.BackColor = System.Drawing.Color.White
        Me.lblCiudadPartida.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCiudadPartida.Location = New System.Drawing.Point(10, 129)
        Me.lblCiudadPartida.Name = "lblCiudadPartida"
        Me.lblCiudadPartida.Size = New System.Drawing.Size(28, 15)
        Me.lblCiudadPartida.TabIndex = 0
        Me.lblCiudadPartida.Text = "XXX"
        Me.lblCiudadPartida.UseMnemonic = False
        '
        'lblCiudadDestino
        '
        Me.lblCiudadDestino.AutoSize = True
        Me.lblCiudadDestino.BackColor = System.Drawing.Color.White
        Me.lblCiudadDestino.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCiudadDestino.Location = New System.Drawing.Point(398, 129)
        Me.lblCiudadDestino.Name = "lblCiudadDestino"
        Me.lblCiudadDestino.Size = New System.Drawing.Size(28, 15)
        Me.lblCiudadDestino.TabIndex = 0
        Me.lblCiudadDestino.Text = "XXX"
        Me.lblCiudadDestino.UseMnemonic = False
        '
        'lblDireccionDestino
        '
        Me.lblDireccionDestino.AutoSize = True
        Me.lblDireccionDestino.BackColor = System.Drawing.Color.White
        Me.lblDireccionDestino.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDireccionDestino.Location = New System.Drawing.Point(398, 106)
        Me.lblDireccionDestino.Name = "lblDireccionDestino"
        Me.lblDireccionDestino.Size = New System.Drawing.Size(28, 15)
        Me.lblDireccionDestino.TabIndex = 0
        Me.lblDireccionDestino.Text = "XXX"
        Me.lblDireccionDestino.UseMnemonic = False
        '
        'lblRemitente
        '
        Me.lblRemitente.AutoSize = True
        Me.lblRemitente.BackColor = System.Drawing.Color.White
        Me.lblRemitente.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblRemitente.Location = New System.Drawing.Point(10, 181)
        Me.lblRemitente.Name = "lblRemitente"
        Me.lblRemitente.Size = New System.Drawing.Size(28, 15)
        Me.lblRemitente.TabIndex = 0
        Me.lblRemitente.Text = "XXX"
        Me.lblRemitente.UseMnemonic = False
        '
        'lblDestinatario
        '
        Me.lblDestinatario.AutoSize = True
        Me.lblDestinatario.BackColor = System.Drawing.Color.White
        Me.lblDestinatario.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDestinatario.Location = New System.Drawing.Point(398, 181)
        Me.lblDestinatario.Name = "lblDestinatario"
        Me.lblDestinatario.Size = New System.Drawing.Size(28, 15)
        Me.lblDestinatario.TabIndex = 0
        Me.lblDestinatario.Text = "XXX"
        Me.lblDestinatario.UseMnemonic = False
        '
        'lblRemiRUC
        '
        Me.lblRemiRUC.AutoSize = True
        Me.lblRemiRUC.BackColor = System.Drawing.Color.White
        Me.lblRemiRUC.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblRemiRUC.Location = New System.Drawing.Point(45, 201)
        Me.lblRemiRUC.Name = "lblRemiRUC"
        Me.lblRemiRUC.Size = New System.Drawing.Size(28, 15)
        Me.lblRemiRUC.TabIndex = 0
        Me.lblRemiRUC.Text = "XXX"
        Me.lblRemiRUC.UseMnemonic = False
        '
        'lblDestiRUC
        '
        Me.lblDestiRUC.AutoSize = True
        Me.lblDestiRUC.BackColor = System.Drawing.Color.White
        Me.lblDestiRUC.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDestiRUC.Location = New System.Drawing.Point(433, 201)
        Me.lblDestiRUC.Name = "lblDestiRUC"
        Me.lblDestiRUC.Size = New System.Drawing.Size(28, 15)
        Me.lblDestiRUC.TabIndex = 0
        Me.lblDestiRUC.Text = "XXX"
        Me.lblDestiRUC.UseMnemonic = False
        '
        'lblRemiDNI
        '
        Me.lblRemiDNI.AutoSize = True
        Me.lblRemiDNI.BackColor = System.Drawing.Color.White
        Me.lblRemiDNI.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblRemiDNI.Location = New System.Drawing.Point(118, 222)
        Me.lblRemiDNI.Name = "lblRemiDNI"
        Me.lblRemiDNI.Size = New System.Drawing.Size(28, 15)
        Me.lblRemiDNI.TabIndex = 0
        Me.lblRemiDNI.Text = "XXX"
        Me.lblRemiDNI.UseMnemonic = False
        '
        'lblDestiDNI
        '
        Me.lblDestiDNI.AutoSize = True
        Me.lblDestiDNI.BackColor = System.Drawing.Color.White
        Me.lblDestiDNI.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDestiDNI.Location = New System.Drawing.Point(514, 222)
        Me.lblDestiDNI.Name = "lblDestiDNI"
        Me.lblDestiDNI.Size = New System.Drawing.Size(28, 15)
        Me.lblDestiDNI.TabIndex = 0
        Me.lblDestiDNI.Text = "XXX"
        Me.lblDestiDNI.UseMnemonic = False
        '
        'lblCant1
        '
        Me.lblCant1.AutoSize = True
        Me.lblCant1.BackColor = System.Drawing.Color.White
        Me.lblCant1.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCant1.Location = New System.Drawing.Point(10, 276)
        Me.lblCant1.Name = "lblCant1"
        Me.lblCant1.Size = New System.Drawing.Size(28, 15)
        Me.lblCant1.TabIndex = 1
        Me.lblCant1.Text = "XXX"
        Me.lblCant1.UseMnemonic = False
        '
        'lblCant2
        '
        Me.lblCant2.AutoSize = True
        Me.lblCant2.BackColor = System.Drawing.Color.White
        Me.lblCant2.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCant2.Location = New System.Drawing.Point(10, 298)
        Me.lblCant2.Name = "lblCant2"
        Me.lblCant2.Size = New System.Drawing.Size(28, 15)
        Me.lblCant2.TabIndex = 2
        Me.lblCant2.Text = "XXX"
        Me.lblCant2.UseMnemonic = False
        '
        'lblCant3
        '
        Me.lblCant3.AutoSize = True
        Me.lblCant3.BackColor = System.Drawing.Color.White
        Me.lblCant3.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCant3.Location = New System.Drawing.Point(10, 320)
        Me.lblCant3.Name = "lblCant3"
        Me.lblCant3.Size = New System.Drawing.Size(28, 15)
        Me.lblCant3.TabIndex = 3
        Me.lblCant3.Text = "XXX"
        Me.lblCant3.UseMnemonic = False
        '
        'lblCant4
        '
        Me.lblCant4.AutoSize = True
        Me.lblCant4.BackColor = System.Drawing.Color.White
        Me.lblCant4.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCant4.Location = New System.Drawing.Point(10, 342)
        Me.lblCant4.Name = "lblCant4"
        Me.lblCant4.Size = New System.Drawing.Size(28, 15)
        Me.lblCant4.TabIndex = 4
        Me.lblCant4.Text = "XXX"
        Me.lblCant4.UseMnemonic = False
        '
        'lblCant5
        '
        Me.lblCant5.AutoSize = True
        Me.lblCant5.BackColor = System.Drawing.Color.White
        Me.lblCant5.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCant5.Location = New System.Drawing.Point(10, 364)
        Me.lblCant5.Name = "lblCant5"
        Me.lblCant5.Size = New System.Drawing.Size(28, 15)
        Me.lblCant5.TabIndex = 5
        Me.lblCant5.Text = "XXX"
        Me.lblCant5.UseMnemonic = False
        '
        'lblCant6
        '
        Me.lblCant6.AutoSize = True
        Me.lblCant6.BackColor = System.Drawing.Color.White
        Me.lblCant6.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCant6.Location = New System.Drawing.Point(10, 386)
        Me.lblCant6.Name = "lblCant6"
        Me.lblCant6.Size = New System.Drawing.Size(28, 15)
        Me.lblCant6.TabIndex = 6
        Me.lblCant6.Text = "XXX"
        Me.lblCant6.UseMnemonic = False
        '
        'lblGRR
        '
        Me.lblGRR.BackColor = System.Drawing.Color.White
        Me.lblGRR.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblGRR.Location = New System.Drawing.Point(63, 408)
        Me.lblGRR.Name = "lblGRR"
        Me.lblGRR.Size = New System.Drawing.Size(412, 45)
        Me.lblGRR.TabIndex = 0
        Me.lblGRR.Text = "XXX" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "XXX" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "XXX"
        Me.lblGRR.UseMnemonic = False
        '
        'lblDesc1
        '
        Me.lblDesc1.AutoSize = True
        Me.lblDesc1.BackColor = System.Drawing.Color.White
        Me.lblDesc1.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDesc1.Location = New System.Drawing.Point(63, 276)
        Me.lblDesc1.Name = "lblDesc1"
        Me.lblDesc1.Size = New System.Drawing.Size(357, 15)
        Me.lblDesc1.TabIndex = 11
        Me.lblDesc1.Text = "XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX"
        Me.lblDesc1.UseMnemonic = False
        '
        'lblDesc2
        '
        Me.lblDesc2.AutoSize = True
        Me.lblDesc2.BackColor = System.Drawing.Color.White
        Me.lblDesc2.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDesc2.Location = New System.Drawing.Point(63, 298)
        Me.lblDesc2.Name = "lblDesc2"
        Me.lblDesc2.Size = New System.Drawing.Size(28, 15)
        Me.lblDesc2.TabIndex = 12
        Me.lblDesc2.Text = "XXX"
        Me.lblDesc2.UseMnemonic = False
        '
        'lblDesc3
        '
        Me.lblDesc3.AutoSize = True
        Me.lblDesc3.BackColor = System.Drawing.Color.White
        Me.lblDesc3.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDesc3.Location = New System.Drawing.Point(63, 320)
        Me.lblDesc3.Name = "lblDesc3"
        Me.lblDesc3.Size = New System.Drawing.Size(28, 15)
        Me.lblDesc3.TabIndex = 13
        Me.lblDesc3.Text = "XXX"
        Me.lblDesc3.UseMnemonic = False
        '
        'lblDesc4
        '
        Me.lblDesc4.AutoSize = True
        Me.lblDesc4.BackColor = System.Drawing.Color.White
        Me.lblDesc4.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDesc4.Location = New System.Drawing.Point(63, 342)
        Me.lblDesc4.Name = "lblDesc4"
        Me.lblDesc4.Size = New System.Drawing.Size(28, 15)
        Me.lblDesc4.TabIndex = 14
        Me.lblDesc4.Text = "XXX"
        Me.lblDesc4.UseMnemonic = False
        '
        'lblDesc5
        '
        Me.lblDesc5.AutoSize = True
        Me.lblDesc5.BackColor = System.Drawing.Color.White
        Me.lblDesc5.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDesc5.Location = New System.Drawing.Point(63, 364)
        Me.lblDesc5.Name = "lblDesc5"
        Me.lblDesc5.Size = New System.Drawing.Size(28, 15)
        Me.lblDesc5.TabIndex = 15
        Me.lblDesc5.Text = "XXX"
        Me.lblDesc5.UseMnemonic = False
        '
        'lblDesc6
        '
        Me.lblDesc6.AutoSize = True
        Me.lblDesc6.BackColor = System.Drawing.Color.White
        Me.lblDesc6.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDesc6.Location = New System.Drawing.Point(63, 386)
        Me.lblDesc6.Name = "lblDesc6"
        Me.lblDesc6.Size = New System.Drawing.Size(28, 15)
        Me.lblDesc6.TabIndex = 16
        Me.lblDesc6.Text = "XXX"
        Me.lblDesc6.UseMnemonic = False
        '
        'lblPeso1
        '
        Me.lblPeso1.AutoSize = True
        Me.lblPeso1.BackColor = System.Drawing.Color.White
        Me.lblPeso1.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPeso1.Location = New System.Drawing.Point(496, 276)
        Me.lblPeso1.Name = "lblPeso1"
        Me.lblPeso1.Size = New System.Drawing.Size(28, 15)
        Me.lblPeso1.TabIndex = 21
        Me.lblPeso1.Text = "XXX"
        Me.lblPeso1.UseMnemonic = False
        '
        'lblPeso2
        '
        Me.lblPeso2.AutoSize = True
        Me.lblPeso2.BackColor = System.Drawing.Color.White
        Me.lblPeso2.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPeso2.Location = New System.Drawing.Point(496, 298)
        Me.lblPeso2.Name = "lblPeso2"
        Me.lblPeso2.Size = New System.Drawing.Size(28, 15)
        Me.lblPeso2.TabIndex = 22
        Me.lblPeso2.Text = "XXX"
        Me.lblPeso2.UseMnemonic = False
        '
        'lblPeso3
        '
        Me.lblPeso3.AutoSize = True
        Me.lblPeso3.BackColor = System.Drawing.Color.White
        Me.lblPeso3.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPeso3.Location = New System.Drawing.Point(496, 320)
        Me.lblPeso3.Name = "lblPeso3"
        Me.lblPeso3.Size = New System.Drawing.Size(28, 15)
        Me.lblPeso3.TabIndex = 23
        Me.lblPeso3.Text = "XXX"
        Me.lblPeso3.UseMnemonic = False
        '
        'lblPeso4
        '
        Me.lblPeso4.AutoSize = True
        Me.lblPeso4.BackColor = System.Drawing.Color.White
        Me.lblPeso4.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPeso4.Location = New System.Drawing.Point(496, 342)
        Me.lblPeso4.Name = "lblPeso4"
        Me.lblPeso4.Size = New System.Drawing.Size(28, 15)
        Me.lblPeso4.TabIndex = 24
        Me.lblPeso4.Text = "XXX"
        Me.lblPeso4.UseMnemonic = False
        '
        'lblPeso5
        '
        Me.lblPeso5.AutoSize = True
        Me.lblPeso5.BackColor = System.Drawing.Color.White
        Me.lblPeso5.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPeso5.Location = New System.Drawing.Point(496, 364)
        Me.lblPeso5.Name = "lblPeso5"
        Me.lblPeso5.Size = New System.Drawing.Size(28, 15)
        Me.lblPeso5.TabIndex = 25
        Me.lblPeso5.Text = "XXX"
        Me.lblPeso5.UseMnemonic = False
        '
        'lblPeso6
        '
        Me.lblPeso6.AutoSize = True
        Me.lblPeso6.BackColor = System.Drawing.Color.White
        Me.lblPeso6.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPeso6.Location = New System.Drawing.Point(496, 386)
        Me.lblPeso6.Name = "lblPeso6"
        Me.lblPeso6.Size = New System.Drawing.Size(28, 15)
        Me.lblPeso6.TabIndex = 26
        Me.lblPeso6.Text = "XXX"
        Me.lblPeso6.UseMnemonic = False
        '
        'lblUM1
        '
        Me.lblUM1.AutoSize = True
        Me.lblUM1.BackColor = System.Drawing.Color.White
        Me.lblUM1.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblUM1.Location = New System.Drawing.Point(569, 276)
        Me.lblUM1.Name = "lblUM1"
        Me.lblUM1.Size = New System.Drawing.Size(28, 15)
        Me.lblUM1.TabIndex = 31
        Me.lblUM1.Text = "XXX"
        Me.lblUM1.UseMnemonic = False
        '
        'lblUM2
        '
        Me.lblUM2.AutoSize = True
        Me.lblUM2.BackColor = System.Drawing.Color.White
        Me.lblUM2.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblUM2.Location = New System.Drawing.Point(569, 298)
        Me.lblUM2.Name = "lblUM2"
        Me.lblUM2.Size = New System.Drawing.Size(28, 15)
        Me.lblUM2.TabIndex = 32
        Me.lblUM2.Text = "XXX"
        Me.lblUM2.UseMnemonic = False
        '
        'lblUM3
        '
        Me.lblUM3.AutoSize = True
        Me.lblUM3.BackColor = System.Drawing.Color.White
        Me.lblUM3.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblUM3.Location = New System.Drawing.Point(569, 320)
        Me.lblUM3.Name = "lblUM3"
        Me.lblUM3.Size = New System.Drawing.Size(28, 15)
        Me.lblUM3.TabIndex = 33
        Me.lblUM3.Text = "XXX"
        Me.lblUM3.UseMnemonic = False
        '
        'lblUM4
        '
        Me.lblUM4.AutoSize = True
        Me.lblUM4.BackColor = System.Drawing.Color.White
        Me.lblUM4.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblUM4.Location = New System.Drawing.Point(569, 342)
        Me.lblUM4.Name = "lblUM4"
        Me.lblUM4.Size = New System.Drawing.Size(28, 15)
        Me.lblUM4.TabIndex = 34
        Me.lblUM4.Text = "XXX"
        Me.lblUM4.UseMnemonic = False
        '
        'lblUM5
        '
        Me.lblUM5.AutoSize = True
        Me.lblUM5.BackColor = System.Drawing.Color.White
        Me.lblUM5.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblUM5.Location = New System.Drawing.Point(569, 364)
        Me.lblUM5.Name = "lblUM5"
        Me.lblUM5.Size = New System.Drawing.Size(28, 15)
        Me.lblUM5.TabIndex = 35
        Me.lblUM5.Text = "XXX"
        Me.lblUM5.UseMnemonic = False
        '
        'lblUM6
        '
        Me.lblUM6.AutoSize = True
        Me.lblUM6.BackColor = System.Drawing.Color.White
        Me.lblUM6.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblUM6.Location = New System.Drawing.Point(569, 386)
        Me.lblUM6.Name = "lblUM6"
        Me.lblUM6.Size = New System.Drawing.Size(28, 15)
        Me.lblUM6.TabIndex = 36
        Me.lblUM6.Text = "XXX"
        Me.lblUM6.UseMnemonic = False
        '
        'lblCosto1
        '
        Me.lblCosto1.AutoSize = True
        Me.lblCosto1.BackColor = System.Drawing.Color.White
        Me.lblCosto1.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCosto1.Location = New System.Drawing.Point(635, 276)
        Me.lblCosto1.Name = "lblCosto1"
        Me.lblCosto1.Size = New System.Drawing.Size(28, 15)
        Me.lblCosto1.TabIndex = 41
        Me.lblCosto1.Text = "XXX"
        Me.lblCosto1.UseMnemonic = False
        '
        'lblCosto2
        '
        Me.lblCosto2.AutoSize = True
        Me.lblCosto2.BackColor = System.Drawing.Color.White
        Me.lblCosto2.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCosto2.Location = New System.Drawing.Point(635, 298)
        Me.lblCosto2.Name = "lblCosto2"
        Me.lblCosto2.Size = New System.Drawing.Size(28, 15)
        Me.lblCosto2.TabIndex = 42
        Me.lblCosto2.Text = "XXX"
        Me.lblCosto2.UseMnemonic = False
        '
        'lblCosto3
        '
        Me.lblCosto3.AutoSize = True
        Me.lblCosto3.BackColor = System.Drawing.Color.White
        Me.lblCosto3.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCosto3.Location = New System.Drawing.Point(635, 320)
        Me.lblCosto3.Name = "lblCosto3"
        Me.lblCosto3.Size = New System.Drawing.Size(28, 15)
        Me.lblCosto3.TabIndex = 43
        Me.lblCosto3.Text = "XXX"
        Me.lblCosto3.UseMnemonic = False
        '
        'lblCosto4
        '
        Me.lblCosto4.AutoSize = True
        Me.lblCosto4.BackColor = System.Drawing.Color.White
        Me.lblCosto4.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCosto4.Location = New System.Drawing.Point(635, 342)
        Me.lblCosto4.Name = "lblCosto4"
        Me.lblCosto4.Size = New System.Drawing.Size(28, 15)
        Me.lblCosto4.TabIndex = 44
        Me.lblCosto4.Text = "XXX"
        Me.lblCosto4.UseMnemonic = False
        '
        'lblCosto5
        '
        Me.lblCosto5.AutoSize = True
        Me.lblCosto5.BackColor = System.Drawing.Color.White
        Me.lblCosto5.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCosto5.Location = New System.Drawing.Point(635, 364)
        Me.lblCosto5.Name = "lblCosto5"
        Me.lblCosto5.Size = New System.Drawing.Size(28, 15)
        Me.lblCosto5.TabIndex = 45
        Me.lblCosto5.Text = "XXX"
        Me.lblCosto5.UseMnemonic = False
        '
        'lblCosto6
        '
        Me.lblCosto6.AutoSize = True
        Me.lblCosto6.BackColor = System.Drawing.Color.White
        Me.lblCosto6.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCosto6.Location = New System.Drawing.Point(635, 386)
        Me.lblCosto6.Name = "lblCosto6"
        Me.lblCosto6.Size = New System.Drawing.Size(28, 15)
        Me.lblCosto6.TabIndex = 46
        Me.lblCosto6.Text = "XXX"
        Me.lblCosto6.UseMnemonic = False
        '
        'lblDistancia
        '
        Me.lblDistancia.AutoSize = True
        Me.lblDistancia.BackColor = System.Drawing.Color.White
        Me.lblDistancia.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDistancia.Location = New System.Drawing.Point(105, 445)
        Me.lblDistancia.Name = "lblDistancia"
        Me.lblDistancia.Size = New System.Drawing.Size(28, 15)
        Me.lblDistancia.TabIndex = 0
        Me.lblDistancia.Text = "XXX"
        Me.lblDistancia.UseMnemonic = False
        '
        'lblVehiculo
        '
        Me.lblVehiculo.AutoSize = True
        Me.lblVehiculo.BackColor = System.Drawing.Color.White
        Me.lblVehiculo.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblVehiculo.Location = New System.Drawing.Point(105, 480)
        Me.lblVehiculo.Name = "lblVehiculo"
        Me.lblVehiculo.Size = New System.Drawing.Size(28, 15)
        Me.lblVehiculo.TabIndex = 0
        Me.lblVehiculo.Text = "XXX"
        Me.lblVehiculo.UseMnemonic = False
        '
        'lblPlaca
        '
        Me.lblPlaca.AutoSize = True
        Me.lblPlaca.BackColor = System.Drawing.Color.White
        Me.lblPlaca.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPlaca.Location = New System.Drawing.Point(63, 499)
        Me.lblPlaca.Name = "lblPlaca"
        Me.lblPlaca.Size = New System.Drawing.Size(28, 15)
        Me.lblPlaca.TabIndex = 0
        Me.lblPlaca.Text = "XXX"
        Me.lblPlaca.UseMnemonic = False
        '
        'lblConf
        '
        Me.lblConf.AutoSize = True
        Me.lblConf.BackColor = System.Drawing.Color.White
        Me.lblConf.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblConf.Location = New System.Drawing.Point(116, 519)
        Me.lblConf.Name = "lblConf"
        Me.lblConf.Size = New System.Drawing.Size(28, 15)
        Me.lblConf.TabIndex = 0
        Me.lblConf.Text = "XXX"
        Me.lblConf.UseMnemonic = False
        '
        'lblCertificado
        '
        Me.lblCertificado.AutoSize = True
        Me.lblCertificado.BackColor = System.Drawing.Color.White
        Me.lblCertificado.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCertificado.Location = New System.Drawing.Point(116, 537)
        Me.lblCertificado.Name = "lblCertificado"
        Me.lblCertificado.Size = New System.Drawing.Size(28, 15)
        Me.lblCertificado.TabIndex = 0
        Me.lblCertificado.Text = "XXX"
        Me.lblCertificado.UseMnemonic = False
        '
        'lblLicencia
        '
        Me.lblLicencia.AutoSize = True
        Me.lblLicencia.BackColor = System.Drawing.Color.White
        Me.lblLicencia.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblLicencia.Location = New System.Drawing.Point(116, 555)
        Me.lblLicencia.Name = "lblLicencia"
        Me.lblLicencia.Size = New System.Drawing.Size(28, 15)
        Me.lblLicencia.TabIndex = 0
        Me.lblLicencia.Text = "XXX"
        Me.lblLicencia.UseMnemonic = False
        '
        'lblSubcontratada
        '
        Me.lblSubcontratada.AutoSize = True
        Me.lblSubcontratada.BackColor = System.Drawing.Color.White
        Me.lblSubcontratada.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblSubcontratada.Location = New System.Drawing.Point(299, 499)
        Me.lblSubcontratada.Name = "lblSubcontratada"
        Me.lblSubcontratada.Size = New System.Drawing.Size(28, 15)
        Me.lblSubcontratada.TabIndex = 0
        Me.lblSubcontratada.Text = "XXX"
        Me.lblSubcontratada.UseMnemonic = False
        '
        'lblDireccion
        '
        Me.lblDireccion.AutoSize = True
        Me.lblDireccion.BackColor = System.Drawing.Color.White
        Me.lblDireccion.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDireccion.Location = New System.Drawing.Point(299, 537)
        Me.lblDireccion.Name = "lblDireccion"
        Me.lblDireccion.Size = New System.Drawing.Size(28, 15)
        Me.lblDireccion.TabIndex = 0
        Me.lblDireccion.Text = "XXX"
        Me.lblDireccion.UseMnemonic = False
        '
        'lblRUC
        '
        Me.lblRUC.AutoSize = True
        Me.lblRUC.BackColor = System.Drawing.Color.White
        Me.lblRUC.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblRUC.Location = New System.Drawing.Point(336, 555)
        Me.lblRUC.Name = "lblRUC"
        Me.lblRUC.Size = New System.Drawing.Size(28, 15)
        Me.lblRUC.TabIndex = 0
        Me.lblRUC.Text = "XXX"
        Me.lblRUC.UseMnemonic = False
        '
        'lblObservaciones
        '
        Me.lblObservaciones.AutoSize = True
        Me.lblObservaciones.BackColor = System.Drawing.Color.White
        Me.lblObservaciones.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblObservaciones.Location = New System.Drawing.Point(94, 582)
        Me.lblObservaciones.Name = "lblObservaciones"
        Me.lblObservaciones.Size = New System.Drawing.Size(28, 15)
        Me.lblObservaciones.TabIndex = 0
        Me.lblObservaciones.Text = "XXX"
        Me.lblObservaciones.UseMnemonic = False
        '
        'PrintForm1
        '
        Me.PrintForm1.DocumentName = "document"
        Me.PrintForm1.Form = Me
        Me.PrintForm1.PrintAction = System.Drawing.Printing.PrintAction.PrintToPrinter
        Me.PrintForm1.PrinterSettings = CType(resources.GetObject("PrintForm1.PrinterSettings"), System.Drawing.Printing.PrinterSettings)
        Me.PrintForm1.PrintFileName = Nothing
        '
        'btnEditar
        '
        Me.btnEditar.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnEditar.BackColor = System.Drawing.SystemColors.Control
        Me.btnEditar.Cursor = System.Windows.Forms.Cursors.Default
        Me.btnEditar.ForeColor = System.Drawing.SystemColors.ControlText
        Me.btnEditar.ImageList = Me.ImageList1
        Me.btnEditar.Location = New System.Drawing.Point(220, 5)
        Me.btnEditar.Name = "btnEditar"
        Me.btnEditar.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.btnEditar.Size = New System.Drawing.Size(62, 48)
        Me.btnEditar.TabIndex = 47
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
        'gbIncremento
        '
        Me.gbIncremento.BackColor = System.Drawing.Color.White
        Me.gbIncremento.Controls.Add(Me.cbFonts)
        Me.gbIncremento.Controls.Add(Me.chkBold)
        Me.gbIncremento.Controls.Add(Me.nudSize)
        Me.gbIncremento.Controls.Add(Me.tbIncremento)
        Me.gbIncremento.Controls.Add(Me.lblPosicion)
        Me.gbIncremento.Location = New System.Drawing.Point(432, 5)
        Me.gbIncremento.Name = "gbIncremento"
        Me.gbIncremento.Size = New System.Drawing.Size(326, 103)
        Me.gbIncremento.TabIndex = 49
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
        'frmImprimirGuia
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.White
        Me.ClientSize = New System.Drawing.Size(775, 615)
        Me.Controls.Add(Me.lblCosto6)
        Me.Controls.Add(Me.lblCosto5)
        Me.Controls.Add(Me.lblCosto4)
        Me.Controls.Add(Me.lblCosto3)
        Me.Controls.Add(Me.lblCosto2)
        Me.Controls.Add(Me.lblCosto1)
        Me.Controls.Add(Me.lblUM6)
        Me.Controls.Add(Me.lblPeso6)
        Me.Controls.Add(Me.lblDesc6)
        Me.Controls.Add(Me.lblCant6)
        Me.Controls.Add(Me.lblUM5)
        Me.Controls.Add(Me.lblPeso5)
        Me.Controls.Add(Me.lblDesc5)
        Me.Controls.Add(Me.lblCant5)
        Me.Controls.Add(Me.lblUM4)
        Me.Controls.Add(Me.lblPeso4)
        Me.Controls.Add(Me.lblDesc4)
        Me.Controls.Add(Me.lblCant4)
        Me.Controls.Add(Me.lblUM3)
        Me.Controls.Add(Me.lblPeso3)
        Me.Controls.Add(Me.lblDesc3)
        Me.Controls.Add(Me.lblCant3)
        Me.Controls.Add(Me.lblUM2)
        Me.Controls.Add(Me.lblPeso2)
        Me.Controls.Add(Me.lblDesc2)
        Me.Controls.Add(Me.lblCant2)
        Me.Controls.Add(Me.lblUM1)
        Me.Controls.Add(Me.lblPeso1)
        Me.Controls.Add(Me.lblDesc1)
        Me.Controls.Add(Me.lblCant1)
        Me.Controls.Add(Me.lblDistancia)
        Me.Controls.Add(Me.lblLicencia)
        Me.Controls.Add(Me.lblCertificado)
        Me.Controls.Add(Me.lblConf)
        Me.Controls.Add(Me.lblPlaca)
        Me.Controls.Add(Me.lblVehiculo)
        Me.Controls.Add(Me.lblObservaciones)
        Me.Controls.Add(Me.lblRUC)
        Me.Controls.Add(Me.lblDireccion)
        Me.Controls.Add(Me.lblSubcontratada)
        Me.Controls.Add(Me.gbIncremento)
        Me.Controls.Add(Me.btnEditar)
        Me.Controls.Add(Me.lblDia1)
        Me.Controls.Add(Me.lblAño2)
        Me.Controls.Add(Me.lblMes2)
        Me.Controls.Add(Me.lblDia2)
        Me.Controls.Add(Me.lblAño1)
        Me.Controls.Add(Me.lblMes1)
        Me.Controls.Add(Me.lblDireccionDestino)
        Me.Controls.Add(Me.lblCiudadDestino)
        Me.Controls.Add(Me.lblCiudadPartida)
        Me.Controls.Add(Me.lblDireccionPartida)
        Me.Controls.Add(Me.lblGRR)
        Me.Controls.Add(Me.lblDestinatario)
        Me.Controls.Add(Me.lblDestiDNI)
        Me.Controls.Add(Me.lblRemiDNI)
        Me.Controls.Add(Me.lblDestiRUC)
        Me.Controls.Add(Me.lblRemiRUC)
        Me.Controls.Add(Me.lblRemitente)
        Me.Controls.Add(Me.btnCancelar)
        Me.Controls.Add(Me.btnImprimir)
        Me.Controls.Add(Me.PictureBox1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.KeyPreview = True
        Me.MaximizeBox = False
        Me.Name = "frmImprimirGuia"
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
    Friend WithEvents lblDia1 As System.Windows.Forms.Label
    Friend WithEvents lblMes1 As System.Windows.Forms.Label
    Friend WithEvents lblAño1 As System.Windows.Forms.Label
    Friend WithEvents lblDia2 As System.Windows.Forms.Label
    Friend WithEvents lblMes2 As System.Windows.Forms.Label
    Friend WithEvents lblAño2 As System.Windows.Forms.Label
    Friend WithEvents lblDireccionPartida As System.Windows.Forms.Label
    Friend WithEvents lblCiudadPartida As System.Windows.Forms.Label
    Friend WithEvents lblCiudadDestino As System.Windows.Forms.Label
    Friend WithEvents lblDireccionDestino As System.Windows.Forms.Label
    Friend WithEvents lblRemitente As System.Windows.Forms.Label
    Friend WithEvents lblDestinatario As System.Windows.Forms.Label
    Friend WithEvents lblRemiRUC As System.Windows.Forms.Label
    Friend WithEvents lblDestiRUC As System.Windows.Forms.Label
    Friend WithEvents lblRemiDNI As System.Windows.Forms.Label
    Friend WithEvents lblDestiDNI As System.Windows.Forms.Label
    Friend WithEvents lblCant1 As System.Windows.Forms.Label
    Friend WithEvents lblCant2 As System.Windows.Forms.Label
    Friend WithEvents lblCant3 As System.Windows.Forms.Label
    Friend WithEvents lblCant4 As System.Windows.Forms.Label
    Friend WithEvents lblCant5 As System.Windows.Forms.Label
    Friend WithEvents lblCant6 As System.Windows.Forms.Label
    Friend WithEvents lblGRR As System.Windows.Forms.Label
    Friend WithEvents lblDesc1 As System.Windows.Forms.Label
    Friend WithEvents lblDesc2 As System.Windows.Forms.Label
    Friend WithEvents lblDesc3 As System.Windows.Forms.Label
    Friend WithEvents lblDesc4 As System.Windows.Forms.Label
    Friend WithEvents lblDesc5 As System.Windows.Forms.Label
    Friend WithEvents lblDesc6 As System.Windows.Forms.Label
    Friend WithEvents lblPeso1 As System.Windows.Forms.Label
    Friend WithEvents lblPeso2 As System.Windows.Forms.Label
    Friend WithEvents lblPeso3 As System.Windows.Forms.Label
    Friend WithEvents lblPeso4 As System.Windows.Forms.Label
    Friend WithEvents lblPeso5 As System.Windows.Forms.Label
    Friend WithEvents lblPeso6 As System.Windows.Forms.Label
    Friend WithEvents lblUM1 As System.Windows.Forms.Label
    Friend WithEvents lblUM2 As System.Windows.Forms.Label
    Friend WithEvents lblUM3 As System.Windows.Forms.Label
    Friend WithEvents lblUM4 As System.Windows.Forms.Label
    Friend WithEvents lblUM5 As System.Windows.Forms.Label
    Friend WithEvents lblUM6 As System.Windows.Forms.Label
    Friend WithEvents lblCosto1 As System.Windows.Forms.Label
    Friend WithEvents lblCosto2 As System.Windows.Forms.Label
    Friend WithEvents lblCosto3 As System.Windows.Forms.Label
    Friend WithEvents lblCosto4 As System.Windows.Forms.Label
    Friend WithEvents lblCosto5 As System.Windows.Forms.Label
    Friend WithEvents lblCosto6 As System.Windows.Forms.Label
    Friend WithEvents lblDistancia As System.Windows.Forms.Label
    Friend WithEvents lblVehiculo As System.Windows.Forms.Label
    Friend WithEvents lblPlaca As System.Windows.Forms.Label
    Friend WithEvents lblConf As System.Windows.Forms.Label
    Friend WithEvents lblCertificado As System.Windows.Forms.Label
    Friend WithEvents lblLicencia As System.Windows.Forms.Label
    Friend WithEvents lblSubcontratada As System.Windows.Forms.Label
    Friend WithEvents lblDireccion As System.Windows.Forms.Label
    Friend WithEvents lblRUC As System.Windows.Forms.Label
    Friend WithEvents lblObservaciones As System.Windows.Forms.Label
    Friend WithEvents PrintForm1 As Microsoft.VisualBasic.PowerPacks.Printing.PrintForm
    Public WithEvents btnEditar As System.Windows.Forms.Button
    Friend WithEvents lblPosicion As System.Windows.Forms.Label
    Friend WithEvents ImageList1 As System.Windows.Forms.ImageList
    Friend WithEvents gbIncremento As System.Windows.Forms.GroupBox
    Friend WithEvents tbIncremento As System.Windows.Forms.TrackBar
    Friend WithEvents chkBold As System.Windows.Forms.CheckBox
    Friend WithEvents nudSize As System.Windows.Forms.NumericUpDown
    Friend WithEvents cbFonts As System.Windows.Forms.ComboBox
End Class
