Imports System.Data.SqlClient

Public Class frmMantCliente

    Dim objCliente As New clsCliente
    Dim bolAccion As Boolean = True

    Private Sub frmMantCliente_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing

        Try
            If objConexion.State = ConnectionState.Open Then
                objConexion.Close()
            End If

            LimpiarRAM()

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
            dgvListado.Rows.Clear()
            Dim lstLista As New List(Of clsCliente)

            lstLista = objCliente.Listar()

            For i As Integer = 0 To lstLista.Count - 1
                dgvListado.Rows.Add()
                dgvListado.Rows(i).Cells(0).Value = lstLista(i).Codigo
                dgvListado.Rows(i).Cells(1).Value = lstLista(i).Nombre
                dgvListado.Rows(i).Cells(2).Value = lstLista(i).TipoID.Descripcion
                dgvListado.Rows(i).Cells(3).Value = lstLista(i).NumeroID
            Next

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

    Private Sub optBuscar_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles optBuscar.CheckedChanged, optNro.CheckedChanged

        Try
            dgvListado.Rows.Clear()
            txtBuscar.Enabled = True
            txtBuscar.Focus()
            btnBuscar.Enabled = True

        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            If objConexion.State = ConnectionState.Open Then
                objConexion.Close()
            End If
        End Try

    End Sub

    Private Sub btnNuevo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNuevo.Click

        Try
            objCliente = New clsCliente

            'Comportamiento del control TabControl
            TabControl1.TabPages(1).Enabled = True
            TabControl1.TabPages(0).Enabled = False
            TabControl1.SelectTab(1)
            txtNombre.Focus()

            'Limpiar y activa cajas de texto
            LimpiarControles(TabControl1, 1)
            DesactivarControles(TabControl1, 1, False)
            txtCodigo.ReadOnly = True

            'Comportamiento de botones
            btnNuevo.Enabled = False
            btnGrabar.Enabled = True
            btnCancelar.Enabled = True

        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            If objConexion.State = ConnectionState.Open Then
                objConexion.Close()
            End If
        End Try

    End Sub

    Private Sub btnGrabar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnGrabar.Click

        Try
            If txtNombre.Text = "" Or cbTipoID.Text = "" Or txtNroID.Text = "" Then
                MessageBox.Show("Datos incompletos", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return
            End If

            objCliente.Nombre = UCase(txtNombre.Text)
            objCliente.TipoID.Codigo = BuscarCodigo("iden_codigo", "iden_descripcion", cbTipoID.Text, "TipoIdentificacion")
            objCliente.NumeroID = txtNroID.Text
            objCliente.Telefono = txtTelefono.Text
            objCliente.Fax = txtFax.Text
            objCliente.Email = txtEmail.Text
            objCliente.Representante = txtRepresentante.Text

            objCliente.Direccion.Clear()
            Dim i As Integer
            For i = 0 To dgvDireccion.Rows.Count - 2
                Dim item As New clsDireccion
                item.Direccion = IIf(dgvDireccion.Rows(i).Cells(0).Value = Nothing, "", dgvDireccion.Rows(i).Cells(0).Value)
                item.Ciudad = IIf(dgvDireccion.Rows(i).Cells(1).Value = Nothing, "", dgvDireccion.Rows(i).Cells(1).Value)
                item.Provincia = IIf(dgvDireccion.Rows(i).Cells(2).Value = Nothing, "", dgvDireccion.Rows(i).Cells(2).Value)
                objCliente.Direccion.Add(item)
                item = Nothing
            Next

            If bolAccion = True Then
                If objCliente.Grabar() = False Then
                    Return
                End If
            Else
                If objCliente.Actualizar() = False Then
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
            txtNombre.Focus()

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
                dgvListado.Rows.Clear()
                MessageBox.Show("Ingrese nombre a buscar", "Datos insuficientes", MessageBoxButtons.OK, MessageBoxIcon.Information)
                txtBuscar.Focus()
            Else

                If optBuscar.Checked = True Then
                    objCliente.Nombre = txtBuscar.Text
                    Dim drLista As SqlDataReader
                    drLista = objCliente.BuscarXNombre()
                    dgvListado.Rows.Clear()
                    Dim i As Integer = 0
                    While drLista.Read()
                        dgvListado.Rows.Add()
                        dgvListado.Rows(i).Cells(0).Value = NullToString(drLista(0))
                        dgvListado.Rows(i).Cells(1).Value = NullToString(drLista(1))
                        dgvListado.Rows(i).Cells(2).Value = NullToString(drLista(2))
                        dgvListado.Rows(i).Cells(3).Value = NullToString(drLista(3))
                        i = i + 1
                    End While
                    drLista.Close()
                End If

                If optNro.Checked = True Then
                    dgvListado.Rows.Clear()
                    objCliente = objCliente.BuscarXID(txtBuscar.Text)
                    dgvListado.Rows.Add()
                    dgvListado.Rows(0).Cells(0).Value = objCliente.Codigo
                    dgvListado.Rows(0).Cells(1).Value = objCliente.Nombre
                    dgvListado.Rows(0).Cells(2).Value = objCliente.TipoID.Descripcion
                    dgvListado.Rows(0).Cells(3).Value = objCliente.NumeroID
                End If

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

    Private Sub cbTipoID_DropDown(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbTipoID.DropDown

        Try
            LLenarCombo("iden_codigo", "iden_descripcion", "TipoIdentificacion", cbTipoID, False)

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

    Private Sub cbTipoID_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbTipoID.SelectedIndexChanged

        Try
            txtNroID.Clear()

            If LCase(cbTipoID.Text) = "sin documento" Then
                txtNroID.Text = "00000000"
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

    Private Sub txtNroID_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtNroID.GotFocus

        Try
            txtNroID.MaxLength = 0

            If LCase(cbTipoID.Text) = "dni" Then
                txtNroID.MaxLength = 8
            End If

            If LCase(cbTipoID.Text) = "ruc" Then
                txtNroID.MaxLength = 11
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

    Private Sub dgvListado_DoubleClick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles dgvListado.DoubleClick

        Try
            Dim strCodigo As String
            strCodigo = dgvListado.SelectedRows(0).Cells(0).Value

            objCliente.Codigo = strCodigo
            If objCliente.BuscarXCodigo() Then

                TabControl1.TabPages(1).Enabled = True
                TabControl1.TabPages(0).Enabled = False
                TabControl1.SelectTab(1)

                txtCodigo.Text = objCliente.Codigo
                txtNombre.Text = objCliente.Nombre
                cbTipoID.Text = objCliente.TipoID.Descripcion
                txtNroID.Text = objCliente.NumeroID
                txtTelefono.Text = objCliente.Telefono
                dgvDireccion.Rows.Clear()

                For i As Integer = 0 To objCliente.Direccion.Count - 1
                    dgvDireccion.Rows.Add()
                    dgvDireccion.Rows(i).Cells(0).Value = IIf(objCliente.Direccion(i).Direccion = "", Nothing, objCliente.Direccion(i).Direccion)
                    dgvDireccion.Rows(i).Cells(1).Value = IIf(objCliente.Direccion(i).Ciudad = "", Nothing, objCliente.Direccion(i).Ciudad)
                    dgvDireccion.Rows(i).Cells(2).Value = IIf(objCliente.Direccion(i).Provincia = "", Nothing, objCliente.Direccion(i).Provincia)
                Next

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

    Private Sub txtBuscarCodigo_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtBuscarCodigo.KeyPress

        Try
            If e.KeyChar = ChrW(Keys.Enter) Then
                e.Handled = True
                objCliente = New clsCliente
                dgvListado.Rows.Clear()
                objCliente.Codigo = txtBuscarCodigo.Text
                If objCliente.BuscarXCodigo = True Then
                    If objCliente.Nombre <> "" Then
                        dgvListado.Rows.Add()
                        dgvListado.Rows(0).Cells(0).Value = objCliente.Codigo
                        dgvListado.Rows(0).Cells(1).Value = objCliente.Nombre
                        dgvListado.Rows(0).Cells(2).Value = objCliente.TipoID.Descripcion
                        dgvListado.Rows(0).Cells(3).Value = objCliente.NumeroID
                    End If
                Else
                    dgvListado.Rows.Clear()
                End If
            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try

    End Sub

    Private Sub TabControl1_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TabControl1.SelectedIndexChanged

        If TabControl1.SelectedIndex = 1 Then
            txtBuscarCodigo.Enabled = False
        Else
            txtBuscarCodigo.Enabled = True
        End If
    End Sub
End Class