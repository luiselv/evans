Public Class frmConfigEmail

    Private Sub frmConfigEmail_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing

        LimpiarRAM()

    End Sub

    Private Sub frmConfigEmail_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Try
            Me.Top = 0
            Me.Left = 0

            DesactivaControles()
            txtRemitente.Text = objParametros.Remitente
            txtEmail.Text = objParametros.EmailRemitente
            txtPass.Text = objParametros.PassRemitente
            txtConfirmar.Text = ""
            txtSMTP.Text = objParametros.SMTP
            txtPuerto.Text = objParametros.Puerto

        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try

    End Sub

    Private Sub btnGrabar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnGrabar.Click

        Try
            If txtConfirmar.Text = "" Or txtEmail.Text = "" Or txtPass.Text = "" Or txtPuerto.Text = "" Or txtRemitente.Text = "" Or txtSMTP.Text = "" Then
                MessageBox.Show("Información incompleta.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Exit Sub
            End If

            If txtPass.Text <> txtConfirmar.Text Then
                MessageBox.Show("El password y la confirmación no coinciden.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Exit Sub
            End If

            With objParametros
                .Remitente = txtRemitente.Text
                .EmailRemitente = txtEmail.Text
                .PassRemitente = txtPass.Text
                .SMTP = txtSMTP.Text
                .Puerto = Convert.ToInt32(txtPuerto.Text)
            End With

            If objParametros.Grabar() Then
                btnGrabar.Enabled = False
                btnEditar.Enabled = True
                DesactivaControles()
                objParametros.CargarParametros()
            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try

    End Sub

    Private Sub DesactivaControles()
        txtRemitente.ReadOnly = True
        txtEmail.ReadOnly = True
        txtPass.ReadOnly = True
        txtConfirmar.ReadOnly = True
        txtSMTP.ReadOnly = True
        txtPuerto.ReadOnly = True
    End Sub

    Private Sub ActivaControles()
        txtRemitente.ReadOnly = False
        txtEmail.ReadOnly = False
        txtPass.ReadOnly = False
        txtConfirmar.ReadOnly = False
        txtSMTP.ReadOnly = False
        txtPuerto.ReadOnly = False
    End Sub

    Private Sub btnEditar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEditar.Click
        ActivaControles()
        btnGrabar.Enabled = True
        btnEditar.Enabled = False
    End Sub
End Class