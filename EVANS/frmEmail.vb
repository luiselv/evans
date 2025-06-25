Imports System.Net.Mail

Public Class frmEmail

    'creamos un nuevo mensaje de correo
    Dim correo As New MailMessage()

    'creamos un servidor SMTP para enviar el correo
    Dim smtp As New SmtpClient()


    Private Sub frmEmail_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing

        LimpiarRAM()

    End Sub

    Private Sub frmEmail_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Me.Top = 0
        Me.Left = 0

        txtRemitente.Text = objParametros.Remitente
        txtDe.Text = objParametros.EmailRemitente

    End Sub

    Private Sub btnAgregar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAgregar.Click

        If ofdAgregar.ShowDialog = Windows.Forms.DialogResult.OK Then
            lvAdjuntos.Items.Add(ofdAgregar.FileName.ToString)
        End If

    End Sub

    Private Sub btnEliminar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEliminar.Click

        Try
            lvAdjuntos.Items.Remove(lvAdjuntos.SelectedItems(0))

        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try

    End Sub

    Private Sub btnSalir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSalir.Click
        Me.Close()
    End Sub

    Private Sub btnEnviar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEnviar.Click

        Try
            If txtPara.Text = "" Or (txtMensaje.Text = "" And lvAdjuntos.Items.Count = 0) Then
                MessageBox.Show("Información incompleta.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Exit Sub
            End If

            frmBarraProgreso.Show()
            frmBarraProgreso.lblAccion.Text = "Enviando..."

            'De
            correo.From = New MailAddress(objParametros.EmailRemitente, objParametros.Remitente)

            'Para
            correo.To.Clear()
            For Each strPara As String In txtPara.Text.Split(",")
                correo.To.Add(strPara.Trim())
            Next

            'Asunto
            correo.Subject = txtAsunto.Text

            'Cuerpo del correo
            correo.Body = txtMensaje.Text

            'Mostrar como HTML
            correo.IsBodyHtml = False

            'Prioridad de el correo
            correo.Priority = MailPriority.High

            'Agregamos los archivos adjuntos (si existiera alguno)
            correo.Attachments.Clear()
            If lvAdjuntos.Items.Count > 0 Then
                For i As Integer = 0 To lvAdjuntos.Items.Count - 1
                    Dim archivoAdjunto As New Attachment(lvAdjuntos.Items(i).Text)
                    correo.Attachments.Add(archivoAdjunto)
                Next
            End If

            smtp.Host = objParametros.SMTP
            smtp.Port = objParametros.Puerto
            smtp.Credentials = New System.Net.NetworkCredential(objParametros.EmailRemitente, objParametros.PassRemitente)
            smtp.EnableSsl = True

            BackgroundWorker1.RunWorkerAsync()

        Catch ex As Net.Mail.SmtpException
            frmBarraProgreso.Close()
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Catch ex As Exception
            frmBarraProgreso.Close()
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try

    End Sub

    Private Sub BackgroundWorker1_DoWork(ByVal sender As System.Object, ByVal e As System.ComponentModel.DoWorkEventArgs) Handles BackgroundWorker1.DoWork

        Try
            smtp.Send(correo)
            e.Result = 100
            BackgroundWorker1.ReportProgress(100)

        Catch ex As Net.Mail.SmtpException
            frmBarraProgreso.Close()
            BackgroundWorker1.CancelAsync()
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Catch ex As Exception
            frmBarraProgreso.Close()
            BackgroundWorker1.CancelAsync()
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
        
    End Sub

    Private Sub BackgroundWorker1_RunWorkerCompleted(ByVal sender As Object, ByVal e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles BackgroundWorker1.RunWorkerCompleted

        frmBarraProgreso.Close()
        If e.Result = 100 Then
            MessageBox.Show("Email enviado con éxito.", "Confirmación", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If

    End Sub

    Private Sub btnNuevo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNuevo.Click
        txtPara.Clear()
        txtAsunto.Clear()
        txtMensaje.Clear()
        lvAdjuntos.Items.Clear()
    End Sub
End Class