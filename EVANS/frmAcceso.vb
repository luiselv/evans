Imports System.Data.SqlClient

Public Class frmAcceso

    Private Sub cbAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbAceptar.Click

        Try
            If txtUsuario.Text = "" Or txtClave.Text = "" Then
                MsgBox("Ingrese su nombre de ususario y contraseña.", MsgBoxStyle.Critical + MsgBoxStyle.OkOnly, "Error")
                Exit Sub
            End If
            strUsuario = txtUsuario.Text
            strClave = txtClave.Text
            strBD2 = cbBD.SelectedItem.ToString
            If Autenticar() Then
                objParametros.CargarParametros()
                frmPrincipal.Show()
                frmPrincipal.Text = frmPrincipal.Text + " - " + strBD2 + " | Usuario: " + objUsuarioActual.NombreCompleto
                Me.Close()
            Else
                MsgBox("Error en el inicio de sesión.", MsgBoxStyle.Critical + MsgBoxStyle.OkOnly, "Error")
                txtUsuario.Focus()
            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try

    End Sub

    Private Sub cbSalir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbSalir.Click
        End
    End Sub

    Private Sub txtUsuario_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtUsuario.KeyPress
        If e.KeyChar = ChrW(Keys.Enter) Then
            e.Handled = True
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub txtClave_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtClave.KeyPress
        If e.KeyChar = ChrW(Keys.Enter) Then
            e.Handled = True
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub txtServidor_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtServidor.KeyPress
        If e.KeyChar = ChrW(Keys.Enter) Then
            e.Handled = True
            cbAceptar.PerformClick()
        End If
    End Sub

    Private Sub btnCrear_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCrear.Click
        Dim año As String
        año = Now.Year.ToString
        If MsgBox("¿Confirma que desea crear una nueva Base de Datos para el año " + año + "?", MsgBoxStyle.YesNo + MsgBoxStyle.Question, "Confirmación") = MsgBoxResult.Yes Then
            If cbBD.Items.Contains(año) Then
                MessageBox.Show("Ya existe una Base de Datos para el año actual", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Else
                If CrearBD() Then
                    LLenarComboBD()
                    cbBD.Refresh()
                    MessageBox.Show("La nueva Base de Datos fue creada con éxito.", "Confirmación", MessageBoxButtons.OK, MessageBoxIcon.Information)
                End If
            End If
        End If

    End Sub

    Private Sub btnConectar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnConectar.Click

        cbBD.Enabled = False
        btnCrear.Enabled = False
        txtUsuario.Enabled = False
        txtClave.Enabled = False
        cbAceptar.Enabled = False
        strServidor = txtServidor.Text
        If Conectar() Then
            LLenarComboBD()
            cbBD.Enabled = True
            btnCrear.Enabled = True
            VerificarBD()
            txtUsuario.Enabled = True
            txtClave.Enabled = True
            cbAceptar.Enabled = True
        End If

    End Sub

    Private Sub LLenarComboBD()

        Try
            Dim cmd As New SqlCommand
            Dim dr As SqlDataReader
            cmd.Connection = objConexion
            cmd.CommandType = CommandType.StoredProcedure
            cmd.CommandText = "sp_helpdb"
            If objConexion.State = ConnectionState.Closed Then
                objConexion.Open()
            End If
            dr = cmd.ExecuteReader
            cbBD.Items.Clear()
            While dr.Read
                If NullToString(dr("name")).StartsWith("2") Then
                    cbBD.Items.Add(NullToString(dr("name")))
                End If
            End While
            dr.Close()

        Catch ex As SqlException
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            If objConexion.State = ConnectionState.Open Then
                objConexion.Close()
            End If
        End Try

    End Sub

    Private Sub VerificarBD()

        Dim año As String
        año = Now.Year.ToString
        If cbBD.Items.Contains(año) Then
            cbBD.SelectedItem = año
        Else
            MessageBox.Show("No se encontró Base de Datos correspondiente al año actual. Se recomienda crear una nueva.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End If

    End Sub

    Private Sub frmAcceso_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        LimpiarRAM()
    End Sub

End Class