Public Class frmParametros

    Private Sub frmParametros_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing

        Try

            LimpiarRAM()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        Finally
            If objConexion.State = ConnectionState.Open Then
                objConexion.Close()
            End If
        End Try

    End Sub

    Private Sub frmParametros_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Try
            Me.Top = 0
            Me.Left = 0

            objParametros.CargarParametros()
            txtIGV.Text = objParametros.PorcentajeIGV
            txtFacturaSerie.Text = objParametros.FacturaSerie
            txtFacturaNro1.Text = objParametros.FacturaNro1
            txtBoletaSerie.Text = objParametros.BoletaSerie
            txtBoletaNro1.Text = objParametros.BoletaNro1
            txtGRemisionSerie.Text = objParametros.GRemisionSerie
            txtGRemisionNro1.Text = objParametros.GRemisionNro1
            txtManifiesto.Text = objParametros.Manifiesto
            DesactivaControles()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        Finally
            If objConexion.State = ConnectionState.Open Then
                objConexion.Close()
            End If
        End Try

        

    End Sub

    Private Sub DesactivaControles()

        txtFacturaSerie.ReadOnly = True
        txtFacturaNro1.ReadOnly = True
        txtBoletaSerie.ReadOnly = True
        txtBoletaNro1.ReadOnly = True
        txtIGV.ReadOnly = True
        txtGRemisionSerie.ReadOnly = True
        txtGRemisionNro1.ReadOnly = True
        txtManifiesto.ReadOnly = True

    End Sub

    Private Sub ActivaControles()

        txtFacturaSerie.ReadOnly = False
        txtFacturaNro1.ReadOnly = False
        txtBoletaSerie.ReadOnly = False
        txtBoletaNro1.ReadOnly = False
        txtIGV.ReadOnly = False
        txtGRemisionSerie.ReadOnly = False
        txtGRemisionNro1.ReadOnly = False
        txtManifiesto.ReadOnly = False

    End Sub

    Private Sub btnEditar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEditar.Click

        ActivaControles()
        btnGrabar.Enabled = True
        btnEditar.Enabled = False

    End Sub

    Private Sub btnGrabar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnGrabar.Click

        Try
            If (MessageBox.Show("La modificación involuntaria de estos parámetros podria afectar el buen funcionamiento del sistema." + vbCrLf + "¿Confirma que desea continuar?", "Precaución", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) = Windows.Forms.DialogResult.No) Then
                Exit Sub
            End If

            With objParametros
                .PorcentajeIGV = txtIGV.Text
                .FacturaSerie = txtFacturaSerie.Text
                .FacturaNro1 = txtFacturaNro1.Text
                .FacturaNro2 = 0
                .BoletaSerie = txtBoletaSerie.Text
                .BoletaNro1 = txtBoletaNro1.Text
                .BoletaNro2 = 0
                .GRemisionSerie = txtGRemisionSerie.Text
                .GRemisionNro1 = txtGRemisionNro1.Text
                .GRemisionNro2 = 0
                .Manifiesto = txtManifiesto.Text
            End With

            If objParametros.Grabar() Then
                btnGrabar.Enabled = False
                btnEditar.Enabled = True
                DesactivaControles()
                objParametros.CargarParametros()
            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        Finally
            If objConexion.State = ConnectionState.Open Then
                objConexion.Close()
            End If
        End Try
        
    End Sub

End Class