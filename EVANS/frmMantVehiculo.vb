Imports System.Data.SqlClient

Public Class frmMantVehiculo

    Dim objVehiculo As New clsVehiculo
    Dim bolAccion As Boolean = True

    Private Sub frmMantVehiculo_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing

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

    Private Sub frmMantVehiculo_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

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

        Catch ex As SqlException
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        Finally
            If objConexion.State = ConnectionState.Open Then
                objConexion.Close()
            End If
        End Try

    End Sub

    Private Sub optTodos_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles optTodos.CheckedChanged

        Try
            txtBuscar.Clear()
            lvListado.Items.Clear()
            Dim drLista As SqlDataReader

            drLista = objVehiculo.Listar()

            While drLista.Read()
                Dim itemLista As ListViewItem
                itemLista = lvListado.Items.Add(NullToString(drLista("vehi_codigo")))
                itemLista.SubItems.Add(NullToString(drLista("vehi_marca")))
                itemLista.SubItems.Add(NullToString(drLista("vehi_placa")))
            End While
            drLista.Close()
            drLista.Dispose()

            txtBuscar.Enabled = False
            btnBuscar.Enabled = False

        Catch ex As SqlException
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        Finally
            If objConexion.State = ConnectionState.Open Then
                objConexion.Close()
            End If
        End Try

    End Sub

    Private Sub lvListado_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles lvListado.DoubleClick

        Try
            Dim strCodigo As String
            strCodigo = lvListado.SelectedItems.Item(0).SubItems(0).Text

            objVehiculo.Codigo = strCodigo
            If objVehiculo.BuscarXCodigo() Then

                TabControl1.TabPages(1).Enabled = True
                TabControl1.TabPages(0).Enabled = False
                TabControl1.SelectTab(1)

                txtCodigo.Text = objVehiculo.Codigo
                txtMarca.Text = objVehiculo.Marca
                txtPlaca.Text = objVehiculo.Placa
                txtConfiguracion.Text = objVehiculo.ConfiguracionVehicular
                txtCertificado.Text = objVehiculo.CertificadoInscripcion
                cbEmpresa.Text = objVehiculo.Empresa.Nombre
                cbEstado.Text = objVehiculo.Estado.Descripcion

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

        Catch ex As SqlException
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        Finally
            If objConexion.State = ConnectionState.Open Then
                objConexion.Close()
            End If
        End Try

    End Sub

    Private Sub btnNuevo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNuevo.Click

        Try
            objVehiculo = New clsVehiculo

            'Comportamiento del control TabControl
            TabControl1.TabPages(1).Enabled = True
            TabControl1.TabPages(0).Enabled = False
            TabControl1.SelectTab(1)
            txtMarca.Focus()

            'Limpiar y activa cajas de texto
            LimpiarControles(TabControl1, 1)
            DesactivarControles(TabControl1, 1, False)
            txtCodigo.ReadOnly = True

            'Comportamiento de botones
            btnNuevo.Enabled = False
            btnGrabar.Enabled = True
            btnCancelar.Enabled = True

            cbEstado.Text = "ACTIVO"

        Catch ex As SqlException
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        Finally
            If objConexion.State = ConnectionState.Open Then
                objConexion.Close()
            End If
        End Try

    End Sub

    Private Sub btnGrabar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnGrabar.Click

        Try
            If txtMarca.Text = "" Or txtPlaca.Text = "" Or txtConfiguracion.Text = "" Or txtCertificado.Text = "" Or cbEmpresa.Text = "" Or cbEstado.Text = "" Then
                MessageBox.Show("Datos incompletos", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return
            End If

            objVehiculo.Marca = txtMarca.Text.ToUpper
            objVehiculo.Placa = txtPlaca.Text.ToUpper
            objVehiculo.ConfiguracionVehicular = txtConfiguracion.Text.ToUpper
            objVehiculo.CertificadoInscripcion = txtCertificado.Text.ToUpper
            objVehiculo.Empresa.Codigo = BuscarCodigo("empr_codigo", "empr_nombre", cbEmpresa.Text, "Empresa")
            objVehiculo.Estado.Codigo = BuscarCodigo("esta_codigo", "esta_descripcion", cbEstado.Text, "Estado")

            If bolAccion = True Then
                If objVehiculo.Grabar() = False Then
                    Return
                End If
            Else
                If objVehiculo.Actualizar() = False Then
                    Return
                Else
                    bolAccion = True
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

        Catch ex As SqlException
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        Finally
            If objConexion.State = ConnectionState.Open Then
                objConexion.Close()
            End If
        End Try

    End Sub

    Private Sub btnEditar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEditar.Click

        Try
            'Activa las cajas de texto
            DesactivarControles(TabControl1, 1, False)
            txtCodigo.ReadOnly = True
            txtMarca.Focus()

            'Comportamiento de botones
            btnNuevo.Enabled = False
            btnGrabar.Enabled = True
            btnEditar.Enabled = False
            btnCancelar.Enabled = True

            btnGrabar.Text = "Actualizar"
            bolAccion = False

        Catch ex As SqlException
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        Finally
            If objConexion.State = ConnectionState.Open Then
                objConexion.Close()
            End If
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

        Catch ex As SqlException
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        Finally
            If objConexion.State = ConnectionState.Open Then
                objConexion.Close()
            End If
        End Try

    End Sub

    Private Sub btnSalir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSalir.Click

        Try
            Me.Close()

        Catch ex As SqlException
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        Finally
            If objConexion.State = ConnectionState.Open Then
                objConexion.Close()
            End If
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
                Dim drLista As SqlDataReader
                drLista = Nothing

                If optMarca.Checked = True Then
                    objVehiculo.Marca = txtBuscar.Text
                    drLista = objVehiculo.BuscarXMarca()
                End If
                If optBuscar.Checked = True Then
                    objVehiculo.Placa = txtBuscar.Text
                    drLista = objVehiculo.BuscarXPlaca()
                End If

                While drLista.Read()
                    Dim itemLista As ListViewItem
                    itemLista = lvListado.Items.Add(NullToString(drLista("vehi_codigo")))
                    itemLista.SubItems.Add(NullToString(drLista("vehi_marca")))
                    itemLista.SubItems.Add(NullToString(drLista("vehi_placa")))
                End While
                drLista.Close()
            End If

        Catch ex As SqlException
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        Finally
            If objConexion.State = ConnectionState.Open Then
                objConexion.Close()
            End If
        End Try

    End Sub

    Private Sub optBuscar_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles optBuscar.CheckedChanged

        Try
            lvListado.Items.Clear()
            txtBuscar.Enabled = True
            txtBuscar.Focus()
            btnBuscar.Enabled = True

        Catch ex As SqlException
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
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

        Catch ex As SqlException
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        Finally
            If objConexion.State = ConnectionState.Open Then
                objConexion.Close()
            End If
        End Try

    End Sub

    Private Sub cbEmpresa_DropDown(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbEmpresa.DropDown

        Try
            LLenarCombo("empr_codigo", "empr_nombre", "Empresa", cbEmpresa, True)

        Catch ex As SqlException
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        Finally
            If objConexion.State = ConnectionState.Open Then
                objConexion.Close()
            End If
        End Try

    End Sub

    Private Sub cbEstado_DropDown(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbEstado.DropDown

        Try
            LLenarCombo("esta_codigo", "esta_descripcion", "Estado", cbEstado, False)

        Catch ex As SqlException
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        Finally
            If objConexion.State = ConnectionState.Open Then
                objConexion.Close()
            End If
        End Try

    End Sub

    Private Sub optMarca_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles optMarca.CheckedChanged

        Try
            lvListado.Items.Clear()
            txtBuscar.Enabled = True
            txtBuscar.Focus()
            btnBuscar.Enabled = True

        Catch ex As SqlException
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
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