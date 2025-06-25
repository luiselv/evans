Imports System.Data.SqlClient

Public Class frmMantTipoID

    Dim objTipoID As New clsTipoID
    Dim bolAccion As Boolean = True

    Private Sub frmMantTipoID_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing

        If objConexion.State = ConnectionState.Open Then
            objConexion.Close()

        End If
        LimpiarRAM()

    End Sub

    Private Sub frmMantTipoID_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Me.Top = 0
        Me.Left = 0

        TabControl1.TabPages(1).Enabled = False
        txtBuscar.Enabled = False

        btnGrabar.Enabled = False
        btnCancelar.Enabled = False
        btnEditar.Enabled = False
        btnBuscar.Enabled = False

        optBuscar.Checked = True

    End Sub

    Private Sub optTodos_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles optTodos.CheckedChanged

        txtBuscar.Clear()
        lvListado.Items.Clear()
        Dim drLista As SqlDataReader

        drLista = objTipoID.Listar()

        While drLista.Read()
            Dim itemLista As ListViewItem
            itemLista = lvListado.Items.Add(nullToString(drLista("iden_codigo")))
            itemLista.SubItems.Add(nullToString(drLista("iden_descripcion")))
        End While
        drLista.Close()
        drLista.Dispose()

        txtBuscar.Enabled = False
        btnBuscar.Enabled = False

    End Sub

    Private Sub optBuscar_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles optBuscar.CheckedChanged

        lvListado.Items.Clear()
        txtBuscar.Enabled = True
        txtBuscar.Focus()
        btnBuscar.Enabled = True

    End Sub

    Private Sub lvListado_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles lvListado.DoubleClick

        Dim strCodigo As String
        strCodigo = lvListado.SelectedItems.Item(0).SubItems(0).Text

        objTipoID.Codigo = strCodigo
        If objTipoID.BuscarXCodigo() Then

            TabControl1.TabPages(1).Enabled = True
            TabControl1.TabPages(0).Enabled = False
            TabControl1.SelectTab(1)

            txtCodigo.Text = objTipoID.Codigo
            txtDescripcion.Text = objTipoID.Descripcion

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

    End Sub

    Private Sub btnNuevo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNuevo.Click

        objTipoID = New clsTipoID

        'Comportamiento del control TabControl
        TabControl1.TabPages(1).Enabled = True
        TabControl1.TabPages(0).Enabled = False
        TabControl1.SelectTab(1)
        txtDescripcion.Focus()

        'Limpiar y activa cajas de texto
        LimpiarControles(TabControl1, 1)
        DesactivarControles(TabControl1, 1, False)
        txtCodigo.ReadOnly = True

        'Comportamiento de botones
        btnNuevo.Enabled = False
        btnGrabar.Enabled = True
        btnCancelar.Enabled = True

    End Sub

    Private Sub btnGrabar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnGrabar.Click

        If txtDescripcion.Text = "" Then
            MessageBox.Show("Datos incompletos", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End If

        objTipoID.Descripcion = txtDescripcion.Text

        If bolAccion = True Then
            If objTipoID.Grabar() = False Then
                Return
            End If
        Else
            If objTipoID.Actualizar() = False Then
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

    End Sub

    Private Sub btnCancelar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancelar.Click

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

    End Sub

    Private Sub btnEditar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEditar.Click

        'Activa las cajas de texto
        DesactivarControles(TabControl1, 1, False)
        txtCodigo.ReadOnly = True
        txtDescripcion.Focus()

        'Comportamiento de botones
        btnNuevo.Enabled = False
        btnGrabar.Enabled = True
        btnEditar.Enabled = False
        btnCancelar.Enabled = True

        btnGrabar.Text = "Actualizar"
        bolAccion = False

    End Sub

    Private Sub btnSalir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSalir.Click
        Me.Close()
    End Sub

    Private Sub btnBuscar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBuscar.Click

        If txtBuscar.Text = "" Then
            lvListado.Items.Clear()
            MessageBox.Show("Ingrese nombre a buscar", "Datos insuficientes", MessageBoxButtons.OK, MessageBoxIcon.Information)
            txtBuscar.Focus()
        Else
            lvListado.Items.Clear()
            objTipoID.Descripcion = txtBuscar.Text

            Dim drLista As SqlDataReader

            drLista = objTipoID.BuscarXNombre()

            While drLista.Read()
                Dim itemLista As ListViewItem
                itemLista = lvListado.Items.Add(nullToString(drLista("iden_codigo")))
                itemLista.SubItems.Add(nullToString(drLista("iden_descripcion")))
            End While
            drLista.Close()
        End If

    End Sub

    Private Sub txtBuscar_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtBuscar.KeyPress

        If e.KeyChar = ChrW(Keys.Enter) Then
            e.Handled = True
            btnBuscar.PerformClick()
        End If

    End Sub
End Class