<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmParametros
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmParametros))
        Me.GroupBox4 = New System.Windows.Forms.GroupBox
        Me.btnGrabar = New System.Windows.Forms.Button
        Me.btnEditar = New System.Windows.Forms.Button
        Me.GroupBox3 = New System.Windows.Forms.GroupBox
        Me.Label8 = New System.Windows.Forms.Label
        Me.Label9 = New System.Windows.Forms.Label
        Me.GroupBox2 = New System.Windows.Forms.GroupBox
        Me.Label3 = New System.Windows.Forms.Label
        Me.Label4 = New System.Windows.Forms.Label
        Me.GroupBox1 = New System.Windows.Forms.GroupBox
        Me.Label2 = New System.Windows.Forms.Label
        Me.Label1 = New System.Windows.Forms.Label
        Me.Label6 = New System.Windows.Forms.Label
        Me.Label5 = New System.Windows.Forms.Label
        Me.GroupBox5 = New System.Windows.Forms.GroupBox
        Me.Label14 = New System.Windows.Forms.Label
        Me.txtFacturaSerie = New System.Windows.Forms.TextBox
        Me.txtFacturaNro1 = New System.Windows.Forms.TextBox
        Me.txtBoletaSerie = New System.Windows.Forms.TextBox
        Me.txtBoletaNro1 = New System.Windows.Forms.TextBox
        Me.txtGRemisionSerie = New System.Windows.Forms.TextBox
        Me.txtGRemisionNro1 = New System.Windows.Forms.TextBox
        Me.txtIGV = New System.Windows.Forms.TextBox
        Me.txtManifiesto = New System.Windows.Forms.TextBox
        Me.GroupBox4.SuspendLayout()
        Me.GroupBox3.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        Me.GroupBox5.SuspendLayout()
        Me.SuspendLayout()
        '
        'GroupBox4
        '
        Me.GroupBox4.Controls.Add(Me.btnGrabar)
        Me.GroupBox4.Controls.Add(Me.btnEditar)
        Me.GroupBox4.Location = New System.Drawing.Point(126, 200)
        Me.GroupBox4.Name = "GroupBox4"
        Me.GroupBox4.Size = New System.Drawing.Size(159, 82)
        Me.GroupBox4.TabIndex = 7
        Me.GroupBox4.TabStop = False
        '
        'btnGrabar
        '
        Me.btnGrabar.BackColor = System.Drawing.SystemColors.Control
        Me.btnGrabar.Cursor = System.Windows.Forms.Cursors.Default
        Me.btnGrabar.Enabled = False
        Me.btnGrabar.ForeColor = System.Drawing.SystemColors.ControlText
        Me.btnGrabar.Image = CType(resources.GetObject("btnGrabar.Image"), System.Drawing.Image)
        Me.btnGrabar.Location = New System.Drawing.Point(83, 19)
        Me.btnGrabar.Name = "btnGrabar"
        Me.btnGrabar.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.btnGrabar.Size = New System.Drawing.Size(62, 48)
        Me.btnGrabar.TabIndex = 9
        Me.btnGrabar.Text = "Grabar"
        Me.btnGrabar.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.btnGrabar.UseVisualStyleBackColor = False
        '
        'btnEditar
        '
        Me.btnEditar.BackColor = System.Drawing.SystemColors.Control
        Me.btnEditar.Cursor = System.Windows.Forms.Cursors.Default
        Me.btnEditar.ForeColor = System.Drawing.SystemColors.ControlText
        Me.btnEditar.Image = CType(resources.GetObject("btnEditar.Image"), System.Drawing.Image)
        Me.btnEditar.Location = New System.Drawing.Point(14, 19)
        Me.btnEditar.Name = "btnEditar"
        Me.btnEditar.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.btnEditar.Size = New System.Drawing.Size(62, 48)
        Me.btnEditar.TabIndex = 8
        Me.btnEditar.Text = "Editar"
        Me.btnEditar.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.btnEditar.UseVisualStyleBackColor = False
        '
        'GroupBox3
        '
        Me.GroupBox3.Controls.Add(Me.txtGRemisionNro1)
        Me.GroupBox3.Controls.Add(Me.txtGRemisionSerie)
        Me.GroupBox3.Controls.Add(Me.Label8)
        Me.GroupBox3.Controls.Add(Me.Label9)
        Me.GroupBox3.Location = New System.Drawing.Point(12, 106)
        Me.GroupBox3.Name = "GroupBox3"
        Me.GroupBox3.Size = New System.Drawing.Size(188, 88)
        Me.GroupBox3.TabIndex = 6
        Me.GroupBox3.TabStop = False
        Me.GroupBox3.Text = "Guia de Remisión"
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label8.Location = New System.Drawing.Point(20, 56)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(58, 13)
        Me.Label8.TabIndex = 9
        Me.Label8.Text = "Número :"
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label9.Location = New System.Drawing.Point(34, 30)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(44, 13)
        Me.Label9.TabIndex = 8
        Me.Label9.Text = "Serie :"
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.txtBoletaNro1)
        Me.GroupBox2.Controls.Add(Me.txtBoletaSerie)
        Me.GroupBox2.Controls.Add(Me.Label3)
        Me.GroupBox2.Controls.Add(Me.Label4)
        Me.GroupBox2.Location = New System.Drawing.Point(206, 12)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(185, 88)
        Me.GroupBox2.TabIndex = 5
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "Boleta"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(21, 54)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(58, 13)
        Me.Label3.TabIndex = 5
        Me.Label3.Text = "Número :"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(35, 28)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(44, 13)
        Me.Label4.TabIndex = 4
        Me.Label4.Text = "Serie :"
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.txtFacturaNro1)
        Me.GroupBox1.Controls.Add(Me.txtFacturaSerie)
        Me.GroupBox1.Controls.Add(Me.Label2)
        Me.GroupBox1.Controls.Add(Me.Label1)
        Me.GroupBox1.Location = New System.Drawing.Point(12, 12)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(188, 88)
        Me.GroupBox1.TabIndex = 4
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Factura"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(20, 57)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(58, 13)
        Me.Label2.TabIndex = 1
        Me.Label2.Text = "Número :"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(34, 31)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(44, 13)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Serie :"
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.Location = New System.Drawing.Point(149, 30)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(16, 13)
        Me.Label6.TabIndex = 1
        Me.Label6.Text = "%"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.Location = New System.Drawing.Point(43, 30)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(36, 13)
        Me.Label5.TabIndex = 1
        Me.Label5.Text = "IGV :"
        '
        'GroupBox5
        '
        Me.GroupBox5.Controls.Add(Me.txtManifiesto)
        Me.GroupBox5.Controls.Add(Me.txtIGV)
        Me.GroupBox5.Controls.Add(Me.Label14)
        Me.GroupBox5.Controls.Add(Me.Label5)
        Me.GroupBox5.Controls.Add(Me.Label6)
        Me.GroupBox5.Location = New System.Drawing.Point(206, 106)
        Me.GroupBox5.Name = "GroupBox5"
        Me.GroupBox5.Size = New System.Drawing.Size(185, 88)
        Me.GroupBox5.TabIndex = 6
        Me.GroupBox5.TabStop = False
        Me.GroupBox5.Text = "Otros"
        '
        'Label14
        '
        Me.Label14.AutoSize = True
        Me.Label14.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label14.Location = New System.Drawing.Point(6, 56)
        Me.Label14.Name = "Label14"
        Me.Label14.Size = New System.Drawing.Size(73, 13)
        Me.Label14.TabIndex = 8
        Me.Label14.Text = "Manifiesto :"
        '
        'txtFacturaSerie
        '
        Me.txtFacturaSerie.Location = New System.Drawing.Point(84, 28)
        Me.txtFacturaSerie.Name = "txtFacturaSerie"
        Me.txtFacturaSerie.Size = New System.Drawing.Size(80, 20)
        Me.txtFacturaSerie.TabIndex = 3
        Me.txtFacturaSerie.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'txtFacturaNro1
        '
        Me.txtFacturaNro1.Location = New System.Drawing.Point(84, 54)
        Me.txtFacturaNro1.Name = "txtFacturaNro1"
        Me.txtFacturaNro1.Size = New System.Drawing.Size(80, 20)
        Me.txtFacturaNro1.TabIndex = 4
        Me.txtFacturaNro1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'txtBoletaSerie
        '
        Me.txtBoletaSerie.Location = New System.Drawing.Point(85, 25)
        Me.txtBoletaSerie.Name = "txtBoletaSerie"
        Me.txtBoletaSerie.Size = New System.Drawing.Size(80, 20)
        Me.txtBoletaSerie.TabIndex = 6
        Me.txtBoletaSerie.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'txtBoletaNro1
        '
        Me.txtBoletaNro1.Location = New System.Drawing.Point(85, 54)
        Me.txtBoletaNro1.Name = "txtBoletaNro1"
        Me.txtBoletaNro1.Size = New System.Drawing.Size(80, 20)
        Me.txtBoletaNro1.TabIndex = 7
        Me.txtBoletaNro1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'txtGRemisionSerie
        '
        Me.txtGRemisionSerie.Location = New System.Drawing.Point(85, 27)
        Me.txtGRemisionSerie.Name = "txtGRemisionSerie"
        Me.txtGRemisionSerie.Size = New System.Drawing.Size(79, 20)
        Me.txtGRemisionSerie.TabIndex = 10
        Me.txtGRemisionSerie.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'txtGRemisionNro1
        '
        Me.txtGRemisionNro1.Location = New System.Drawing.Point(85, 53)
        Me.txtGRemisionNro1.Name = "txtGRemisionNro1"
        Me.txtGRemisionNro1.Size = New System.Drawing.Size(79, 20)
        Me.txtGRemisionNro1.TabIndex = 11
        Me.txtGRemisionNro1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'txtIGV
        '
        Me.txtIGV.Location = New System.Drawing.Point(85, 27)
        Me.txtIGV.Name = "txtIGV"
        Me.txtIGV.Size = New System.Drawing.Size(44, 20)
        Me.txtIGV.TabIndex = 9
        Me.txtIGV.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'txtManifiesto
        '
        Me.txtManifiesto.Location = New System.Drawing.Point(85, 53)
        Me.txtManifiesto.Name = "txtManifiesto"
        Me.txtManifiesto.Size = New System.Drawing.Size(80, 20)
        Me.txtManifiesto.TabIndex = 10
        Me.txtManifiesto.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'frmParametros
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(403, 289)
        Me.Controls.Add(Me.GroupBox4)
        Me.Controls.Add(Me.GroupBox5)
        Me.Controls.Add(Me.GroupBox3)
        Me.Controls.Add(Me.GroupBox2)
        Me.Controls.Add(Me.GroupBox1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.Name = "frmParametros"
        Me.Text = "Parámetros"
        Me.GroupBox4.ResumeLayout(False)
        Me.GroupBox3.ResumeLayout(False)
        Me.GroupBox3.PerformLayout()
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox2.PerformLayout()
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.GroupBox5.ResumeLayout(False)
        Me.GroupBox5.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents GroupBox4 As System.Windows.Forms.GroupBox
    Public WithEvents btnGrabar As System.Windows.Forms.Button
    Public WithEvents btnEditar As System.Windows.Forms.Button
    Friend WithEvents GroupBox3 As System.Windows.Forms.GroupBox
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents GroupBox5 As System.Windows.Forms.GroupBox
    Friend WithEvents Label14 As System.Windows.Forms.Label
    Friend WithEvents txtBoletaSerie As System.Windows.Forms.TextBox
    Friend WithEvents txtFacturaNro1 As System.Windows.Forms.TextBox
    Friend WithEvents txtFacturaSerie As System.Windows.Forms.TextBox
    Friend WithEvents txtGRemisionSerie As System.Windows.Forms.TextBox
    Friend WithEvents txtBoletaNro1 As System.Windows.Forms.TextBox
    Friend WithEvents txtGRemisionNro1 As System.Windows.Forms.TextBox
    Friend WithEvents txtManifiesto As System.Windows.Forms.TextBox
    Friend WithEvents txtIGV As System.Windows.Forms.TextBox
End Class
