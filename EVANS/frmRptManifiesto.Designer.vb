<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmRptManifiesto
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmRptManifiesto))
        Me.crvManifiesto = New CrystalDecisions.Windows.Forms.CrystalReportViewer
        Me.rdManifiesto = New EVANS.Manifiesto
        Me.SuspendLayout()
        '
        'crvManifiesto
        '
        Me.crvManifiesto.ActiveViewIndex = -1
        Me.crvManifiesto.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.crvManifiesto.Dock = System.Windows.Forms.DockStyle.Fill
        Me.crvManifiesto.Location = New System.Drawing.Point(0, 0)
        Me.crvManifiesto.Name = "crvManifiesto"
        Me.crvManifiesto.SelectionFormula = ""
        Me.crvManifiesto.Size = New System.Drawing.Size(677, 495)
        Me.crvManifiesto.TabIndex = 0
        Me.crvManifiesto.ViewTimeSelectionFormula = ""
        '
        'frmRptManifiesto
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(677, 495)
        Me.Controls.Add(Me.crvManifiesto)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "frmRptManifiesto"
        Me.Text = "Manifiesto de Carga"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents crvManifiesto As CrystalDecisions.Windows.Forms.CrystalReportViewer
    Friend WithEvents rdManifiesto As EVANS.Manifiesto
End Class
