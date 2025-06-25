Imports System.Data.SqlClient

Public Class frmMantUsuarios

    Dim objUsuario As New clsUsuario
    Dim bolAccion As Boolean = True

    Private Sub frmMantCliente_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing

        Try
            If objConexion.State = ConnectionState.Open Then
                objConexion.Close()
            End If

            LimpiarRAM()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End Try

    End Sub

    Private Sub frmMantCliente_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Try
            Me.Top = 0
            Me.Left = 0

            TabControl1.TabPages(1).Enabled = False
            txtBuscar.Enabled = False

            btnGrabar.Enabled = False
            btnCancelar.Enabled = False
            btnEditar.Enabled = False
            btnBuscar.Enabled = False

            optBuscar.Checked = True

        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End Try

    End Sub

    Private Sub optTodos_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles optTodos.CheckedChanged

        Try
            txtBuscar.Clear()
            lvListado.Items.Clear()
            Dim lstLista As New List(Of clsUsuario)

            lstLista = objUsuario.Listar()

            For i As Integer = 0 To lstLista.Count - 1
                Dim itemLista As ListViewItem
                itemLista = lvListado.Items.Add(lstLista(i).Codigo)
                itemLista.SubItems.Add(lstLista(i).NombreCompleto)
                itemLista.SubItems.Add(lstLista(i).NombreUsuario)
            Next

            txtBuscar.Enabled = False
            btnBuscar.Enabled = False

        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End Try

    End Sub

    Private Sub optBuscar_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles optBuscar.CheckedChanged

        Try
            lvListado.Items.Clear()
            txtBuscar.Enabled = True
            txtBuscar.Focus()
            btnBuscar.Enabled = True

        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End Try

    End Sub

    Private Sub lvListado_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles lvListado.DoubleClick

        Try
            Dim strCodigo As String
            strCodigo = lvListado.SelectedItems.Item(0).SubItems(0).Text

            objUsuario.Codigo = strCodigo
            If objUsuario.BuscarXCodigo() Then

                TabControl1.TabPages(1).Enabled = True
                TabControl1.TabPages(0).Enabled = False
                TabControl1.SelectTab(1)
                LimpiarControles(TabControl1, 1)

                txtCodigo.Text = objUsuario.Codigo
                txtEmpleado.Text = objUsuario.NombreCompleto
                txtUsuario.Text = objUsuario.NombreUsuario
                txtClave.Text = objUsuario.Clave
                txtRepetir.Text = objUsuario.Clave
                cbEstado.Text = objUsuario.Estado.Descripcion
                chkAdmin.Checked = IIf(objUsuario.Administrador = 1, True, False)

                'Desactivar controles
                DesactivarControles(TabControl1, 1, True)

                'Comportamiento de botones
                btnNuevo.Enabled = True
                btnGrabar.Enabled = False
                btnEditar.Enabled = True
                btnCancelar.Enabled = True

                btnGrabar.Text = "Grabar"

            Else
                Return
            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End Try

    End Sub

    Private Sub btnNuevo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNuevo.Click

        Try
            objUsuario = New clsUsuario

            'Comportamiento del control TabControl
            TabControl1.TabPages(1).Enabled = True
            TabControl1.TabPages(0).Enabled = False
            TabControl1.SelectTab(1)
            txtEmpleado.Focus()

            'Limpiar y activa cajas de texto
            LimpiarControles(TabControl1, 1)
            DesactivarControles(TabControl1, 1, False)
            txtCodigo.ReadOnly = True

            'Comportamiento de botones
            btnNuevo.Enabled = False
            btnGrabar.Enabled = True
            btnCancelar.Enabled = True

            objUsuario = New clsUsuario

        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End Try

    End Sub

    Private Sub btnGrabar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnGrabar.Click

        Try
            If txtEmpleado.Text = "" Or txtUsuario.Text = "" Or txtClave.Text = "" Or txtRepetir.Text = "" Or cbEstado.Text = "" Then
                MessageBox.Show("Datos incompletos", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Exit Sub
            End If

            If txtClave.Text = txtRepetir.Text Then
                objUsuario.Clave = txtClave.Text
            Else
                MessageBox.Show("La nueva clave no concuerda con la confirmación.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Exit Sub
            End If

            objUsuario.NombreCompleto = txtEmpleado.Text
            objUsuario.NombreUsuario = txtUsuario.Text
            objUsuario.Estado.Codigo = BuscarCodigo("esta_codigo", "esta_descripcion", cbEstado.Text, "Estado")
            objUsuario.Administrador = IIf(chkAdmin.Checked = True, 1, 0)

            If bolAccion = True Then
                If objUsuario.Grabar() = False Then
                    Return
                End If
            Else
                If objUsuarioActual.Codigo = objUsuario.Codigo Or objUsuarioActual.Administrador = 1 Then
                    If objUsuario.Actualizar() = False Then
                        Return
                    Else
                        bolAccion = True
                    End If
                Else
                    MessageBox.Show("Solo un usuario administrador o el propietario de la cuenta pueden modificar este registro.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Return
                End If
                
            End If

            'Comportamiento del TabControl
            TabControl1.TabPages(0).Enabled = True
            TabControl1.TabPages(1).Enabled = False
            TabControl1.SelectTab(0)

            'Comportamiento de Botones
            btnNuevo.Enabled = True
            btnGrabar.Enabled = False
            btnEditar.Enabled = False
            btnCancelar.Enabled = False

            btnGrabar.Text = "Grabar"

            optBuscar.Checked = True

        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End Try

    End Sub

    Private Sub btnEditar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEditar.Click

        Try
            'Activa las cajas de texto
            DesactivarControles(TabControl1, 1, False)
            txtCodigo.ReadOnly = True
            txtEmpleado.Focus()

            'Comportamiento de botones
            btnNuevo.Enabled = False
            btnGrabar.Enabled = True
            btnEditar.Enabled = False
            btnCancelar.Enabled = True

            btnGrabar.Text = "Actualizar"
            bolAccion = False

        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End Try

    End Sub

    Private Sub btnCancelar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancelar.Click

        Try
            'Comportamiento del TabControl
            TabControl1.TabPages(0).Enabled = True
            TabControl1.TabPages(1).Enabled = False
            TabControl1.SelectTab(0)

            bolAccion = True
            btnGrabar.Text = "Grabar"

            'Comportamiento de botones
            btnNuevo.Enabled = True
            btnGrabar.Enabled = False
            btnEditar.Enabled = False
            btnCancelar.Enabled = False

        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End Try

    End Sub

    Private Sub btnSalir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSalir.Click

        Try
            Me.Close()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End Try

    End Sub

    Private Sub btnBuscar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBuscar.Click

        Try
            If txtBuscar.Text = "" Then
                lvListado.Items.Clear()
                MessageBox.Show("Ingrese nombre a buscar", "Datos insuficientes", MessageBoxButtons.OK, MessageBoxIcon.Information)
                txtBuscar.Focus()
            Else
                lvListado.Items.Clear()
                objUsuario.NombreCompleto = txtBuscar.Text

                Dim drLista As SqlDataReader

                drLista = objUsuario.BuscarXNombre()

                While drLista.Read()
                    Dim itemLista As ListViewItem
                    itemLista = lvListado.Items.Add(NullToString(drLista(0)))
                    itemLista.SubItems.Add(NullToString(drLista(1)))
                    itemLista.SubItems.Add(NullToString(drLista(2)))
                    itemLista.SubItems.Add(NullToString(drLista(3)))
                End While
                drLista.Close()
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

    Private Sub txtBuscar_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtBuscar.KeyPress

        Try
            If e.KeyChar = ChrW(Keys.Enter) Then
                e.Handled = True
                btnBuscar.PerformClick()
            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End Try

    End Sub


    Private Sub cbEstado_DropDown(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbEstado.DropDown

        Try
            LLenarCombo("esta_codigo", "esta_descripcion", "Estado", cbEstado, False)
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End Try

    End Sub

    Private Sub chkAdmin_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkAdmin.CheckedChanged

        If chkAdmin.Checked = True Then
            If objUsuarioActual.Administrador <> 1 Then
                MessageBox.Show("Solo un usuario administrador puede modificar esta opción.", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Information)
                chkAdmin.Checked = False
            End If
        End If

    End Sub
End Class