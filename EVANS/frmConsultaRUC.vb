Public Class frmConsultaRUC

    Private Sub frmConsultaRUC_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        LimpiarRAM()
    End Sub

    Private Sub frmConsultaRUC_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Try
            Me.Top = 0
            Me.Left = 0
            Me.WindowState = FormWindowState.Maximized
            Dim nuevaURL As New Uri("https://e-consultaruc.sunat.gob.pe/cl-ti-itmrconsruc/jcrS00Alias")
            WebBrowser1.Url = nuevaURL

        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try

    End Sub
End Class