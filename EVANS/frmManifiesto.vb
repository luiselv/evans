Imports System.Data.SqlClient
Public Class frmManifiesto

    Dim bolAccion As Boolean = True

    Private Sub frmManifiesto_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing

        Try
            LimpiarRAM()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            If objConexion.State = ConnectionState.Open Then
                objConexion.Close()
            End If

            If objConexion2.State = ConnectionState.Open Then
                objConexion2.Close()
            End If
        End Try

    End Sub

    Private Sub frmManifiesto_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Try
            Me.Top = 0
            Me.Left = 0

            TabControl1.TabPages(1).Enabled = False

            btnGrabar.Enabled = False
            btnCancelar.Enabled = False
            btnEditar.Enabled = False

        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End Try

    End Sub

    Private Sub ListarGuiasPendientes()
        Try
            Dim cmd As New SqlCommand
            Dim dr As SqlDataReader
            cmd.CommandType = CommandType.Text
            cmd.Connection = objConexion2
            cmd.CommandText = "Select G.grem_codigo, (G.grem_serie + '-' + G.grem_numero) as NroDoc, G.grem_fechaemision, D.dest_nombre, G.grem_costototal, G.grem_pesototal, G.grem_impreso from GuiaRemision G inner join EVANS.dbo.Destino D on D.dest_codigo = G.dest_codigo where G.grem_enviado = 0 and G.esta_codigo = 1"
            If objConexion2.State = ConnectionState.Closed Then
                objConexion2.Open()
            End If
            dr = cmd.ExecuteReader()

            dgvPendientes.Rows.Clear()
            Dim i As Integer = 0
            While dr.Read
                dgvPendientes.Rows.Add()
                dgvPendientes.Rows(i).Cells(0).Value = NullToString(dr(0))
                dgvPendientes.Rows(i).Cells(1).Value = NullToString(dr(1))
                dgvPendientes.Rows(i).Cells(2).Value = NullToString(dr(2))
                dgvPendientes.Rows(i).Cells(3).Value = NullToString(dr(3))
                dgvPendientes.Rows(i).Cells(4).Value = FormatNumber(NullToString(dr(4)), 2)
                dgvPendientes.Rows(i).Cells(5).Value = FormatNumber(NullToString(dr(5)), 2)

                'pintar las filas de guias de transbordo
                'If FormatDateTime(dr(2), DateFormat.ShortDate) < Date.Today.ToShortDateString And NullToString(dr("grem_impreso")) = 1 Then   ----> modificado el 02/08/2012
                If FormatDateTime(dr(2), DateFormat.ShortDate) < Date.Today.ToShortDateString Then
                    dgvPendientes.Rows(i).DefaultCellStyle.BackColor = Color.LightBlue
                End If
                i = i + 1
            End While

        Catch ex As SqlException
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        Finally
            If objConexion2.State = ConnectionState.Open Then
                objConexion2.Close()
            End If
        End Try

    End Sub

    Private Sub btnNuevo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNuevo.Click
        Try
            objManifiesto = New clsManifiesto

            'Comportamiento del control TabControl
            TabControl1.TabPages(1).Enabled = True
            TabControl1.TabPages(0).Enabled = False
            TabControl1.SelectTab(1)

            'Limpiar y activa cajas de texto
            LimpiarControles(TabControl1, 1)
            DesactivarControles(TabControl1, 1, False)
            txtCodigo.ReadOnly = True
            txtUsuario.ReadOnly = True
            txtNumero.ReadOnly = True

            'Comportamiento de botones
            btnNuevo.Enabled = False
            btnGrabar.Enabled = True
            btnCancelar.Enabled = True

            cbEstado.Text = "ACTIVO"
            txtUsuario.Text = objUsuarioActual.NombreCompleto
            ListarGuiasPendientes()
            dgvEnviar.ReadOnly = True
            dgvPendientes.ReadOnly = True
            LLenarInfoCarga()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End Try

    End Sub

    Private Sub btnAgregar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAgregar.Click
        Try
            For Each Seleccion As DataGridViewRow In dgvPendientes.SelectedRows

                'agregar la nueva fila al DataGridView destino
                Dim nuevaFila As New DataGridViewRow
                nuevaFila.CreateCells(dgvEnviar)
                nuevaFila.Cells(0).Value = Seleccion.Cells(0).Value
                nuevaFila.Cells(1).Value = Seleccion.Cells(1).Value
                nuevaFila.Cells(2).Value = Seleccion.Cells(2).Value
                nuevaFila.Cells(3).Value = Seleccion.Cells(3).Value
                nuevaFila.Cells(4).Value = Seleccion.Cells(4).Value
                nuevaFila.Cells(5).Value = Seleccion.Cells(5).Value
                dgvEnviar.Rows.Add(nuevaFila)

                'eliminar la fila del DataGridView origen
                dgvPendientes.Rows.Remove(Seleccion)
            Next
            LLenarInfoCarga()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End Try

    End Sub

    Private Sub btnQuitar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnQuitar.Click

        Try
            For Each Seleccion As DataGridViewRow In dgvEnviar.SelectedRows

                'agregar la nueva fila al DataGridView destino
                Dim nuevaFila As New DataGridViewRow
                nuevaFila.CreateCells(dgvEnviar)
                nuevaFila.Cells(0).Value = Seleccion.Cells(0).Value
                nuevaFila.Cells(1).Value = Seleccion.Cells(1).Value
                nuevaFila.Cells(2).Value = Seleccion.Cells(2).Value
                nuevaFila.Cells(3).Value = Seleccion.Cells(3).Value
                nuevaFila.Cells(4).Value = Seleccion.Cells(4).Value
                nuevaFila.Cells(5).Value = Seleccion.Cells(5).Value
                dgvPendientes.Rows.Add(nuevaFila)

                'eliminar la fila del DataGridView origen
                dgvEnviar.Rows.Remove(Seleccion)
            Next
            LLenarInfoCarga()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End Try

    End Sub

    Private Sub btnAgregarTodo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAgregarTodo.Click

        Try
            For Each Fila As DataGridViewRow In dgvPendientes.Rows
                Dim nuevaFila As New DataGridViewRow
                nuevaFila.CreateCells(dgvEnviar)
                If Not Fila.Cells(0).Value = Nothing Then
                    nuevaFila.Cells(0).Value = Fila.Cells(0).Value
                    nuevaFila.Cells(1).Value = Fila.Cells(1).Value
                    nuevaFila.Cells(2).Value = Fila.Cells(2).Value
                    nuevaFila.Cells(3).Value = Fila.Cells(3).Value
                    nuevaFila.Cells(4).Value = Fila.Cells(4).Value
                    nuevaFila.Cells(5).Value = Fila.Cells(5).Value
                    dgvEnviar.Rows.Add(nuevaFila)
                End If
            Next
            dgvPendientes.Rows.Clear()
            LLenarInfoCarga()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End Try

    End Sub

    Private Sub btnQuitarTodo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnQuitarTodo.Click

        Try
            For Each Fila As DataGridViewRow In dgvEnviar.Rows
                Dim nuevaFila As New DataGridViewRow
                nuevaFila.CreateCells(dgvEnviar)
                If Not Fila.Cells(0).Value = Nothing Then
                    nuevaFila.Cells(0).Value = Fila.Cells(0).Value
                    nuevaFila.Cells(1).Value = Fila.Cells(1).Value
                    nuevaFila.Cells(2).Value = Fila.Cells(2).Value
                    nuevaFila.Cells(3).Value = Fila.Cells(3).Value
                    nuevaFila.Cells(4).Value = Fila.Cells(4).Value
                    nuevaFila.Cells(5).Value = Fila.Cells(5).Value
                    dgvPendientes.Rows.Add(nuevaFila)
                End If
            Next
            dgvEnviar.Rows.Clear()
            LLenarInfoCarga()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End Try

    End Sub

    Private Sub cbTransportista_DropDown(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbTransportista.DropDown

        Try
            LLenarCombo("empr_codigo", "empr_nombre", "Empresa", cbTransportista, True)
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End Try

    End Sub

    Private Sub cbChofer_DropDown(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbChofer.DropDown

        Try
            LLenarCombo("chof_codigo", "chof_nombre", "Chofer", cbChofer, True)
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End Try

    End Sub

    Private Sub cbVehiculo_DropDown(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbVehiculo.DropDown

        Try
            LLenarCombo("vehi_codigo", "vehi_placa", "Vehiculo", cbVehiculo, True)
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End Try

    End Sub

    Private Sub cbCarreta_DropDown(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbCarreta.DropDown

        Try
            LLenarCombo("carr_codigo", "carr_placa", "Carreta", cbCarreta, True)
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

    Private Sub btnGrabar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnGrabar.Click

        Try
            If cbTransportista.Text = "" Or cbChofer.Text = "" Or cbVehiculo.Text = "" Or cbCarreta.Text = "" Or cbEstado.Text = "" Or dgvEnviar.Rows.Count < 2 Then
                MessageBox.Show("Datos incompletos", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Exit Sub
            End If

            objManifiesto.Transportista.Codigo = BuscarCodigo("empr_codigo", "empr_nombre", cbTransportista.Text, "Empresa")
            objManifiesto.Chofer.Codigo = BuscarCodigo("chof_codigo", "chof_nombre", cbChofer.Text, "Chofer")
            objManifiesto.Vehiculo.Codigo = BuscarCodigo("vehi_codigo", "vehi_placa", cbVehiculo.Text, "Vehiculo")
            objManifiesto.Carreta.Codigo = BuscarCodigo("carr_codigo", "carr_placa", cbCarreta.Text, "Carreta")
            objManifiesto.Estado.Codigo = BuscarCodigo("esta_codigo", "esta_descripcion", cbEstado.Text, "Estado")
            objManifiesto.Fecha = dtpFecha.Value
            objManifiesto.Usuario.Codigo = objUsuarioActual.Codigo
            objManifiesto.Importe = txtImporte.Text
            objManifiesto.NroGuias = txtNroGuias.Text
            objManifiesto.Peso = txtPeso.Text

            objManifiesto.GuiasSeleccionadas.Clear()
            For i As Integer = 0 To dgvEnviar.Rows.Count - 2
                Dim item As New clsGuiaRemision
                item.Codigo = dgvEnviar.Rows(i).Cells(0).Value
                objManifiesto.GuiasSeleccionadas.Add(item)
            Next

            objManifiesto.GuiasDisponibles.Clear()
            For i As Integer = 0 To dgvPendientes.Rows.Count - 2
                objManifiesto.GuiasDisponibles.Add(dgvPendientes.Rows(i).Cells(0).Value)
            Next

            If bolAccion = True Then
                If objManifiesto.Grabar() = False Then
                    Exit Sub
                End If
            Else
                If objManifiesto.Actualizar() = False Then
                    Exit Sub
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
            btnBuscar.PerformClick()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End Try

    End Sub

    Private Sub btnEditar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEditar.Click

        Try
            'Activar controles
            DesactivarControles(TabControl1, 1, False)
            txtCodigo.ReadOnly = True
            txtUsuario.ReadOnly = True
            txtNumero.ReadOnly = True

            'Comportamiento de botones
            btnNuevo.Enabled = False
            btnGrabar.Enabled = True
            btnEditar.Enabled = False
            btnCancelar.Enabled = True

            btnGrabar.Text = "Actualizar"
            bolAccion = False
            dgvEnviar.ReadOnly = True
            dgvPendientes.ReadOnly = True

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

    Private Sub btnImprimir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnImprimir.Click

        Try
            If dgvListado.SelectedRows.Count = 0 Or dgvListado.Rows.Count < 2 Then
                Exit Sub
            End If
            objManifiesto.Codigo = dgvListado.SelectedRows(0).Cells(0).Value
            If objManifiesto.BuscarXCodigo() = True Then
                frmRptManifiesto.Show()
            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
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

    Private Sub LLenarInfoCarga()

        Try
            Dim intNroGuias As Integer = 0
            Dim dblPeso As Double = 0.0
            Dim dblImporte As Double = 0.0

            For i As Integer = 0 To dgvPendientes.Rows.Count - 2
                intNroGuias = intNroGuias + 1
                dblImporte = dblImporte + dgvPendientes.Rows(i).Cells(4).Value
                dblPeso = dblPeso + dgvPendientes.Rows(i).Cells(5).Value
            Next
            txtNroGuias1.Text = intNroGuias
            txtPeso1.Text = FormatNumber(dblPeso, 2)
            txtImporte1.Text = FormatNumber(dblImporte / (1 + (objParametros.PorcentajeIGV / 100)), 2)

            intNroGuias = 0
            dblPeso = 0.0
            dblImporte = 0.0
            For i As Integer = 0 To dgvEnviar.Rows.Count - 2
                intNroGuias = intNroGuias + 1
                dblImporte = dblImporte + dgvEnviar.Rows(i).Cells(4).Value
                dblPeso = dblPeso + dgvEnviar.Rows(i).Cells(5).Value
            Next
            txtNroGuias.Text = intNroGuias
            txtPeso.Text = FormatNumber(dblPeso, 2)
            txtImporte.Text = FormatNumber(dblImporte / (1 + (objParametros.PorcentajeIGV / 100)), 2)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End Try

    End Sub

    Private Sub optHoy_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles optHoy.CheckedChanged

        cbMes.Enabled = False
        txtFecha1.Enabled = False

    End Sub

    Private Sub optMes_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles optMes.CheckedChanged

        cbMes.Enabled = True
        txtFecha1.Enabled = False

    End Sub

    Private Sub optIntervalo_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles optIntervalo.CheckedChanged

        cbMes.Enabled = False
        txtFecha1.Enabled = True
        
    End Sub

    Private Sub btnBuscar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBuscar.Click

        Try
            Dim drBuscar As SqlDataReader
            Dim cmdBuscar As New SqlCommand
            Dim strSQL As String = ""
            cmdBuscar.CommandType = CommandType.Text
            cmdBuscar.Connection = objConexion2
            cmdBuscar.Parameters.Clear()
            If optHoy.Checked = True Then
                strSQL = "Select M.mani_codigo, M.mani_numero, M.mani_fecha, E.empr_nombre, V.vehi_placa, M.mani_importe, M.mani_nroguias, M.mani_peso from Manifiesto M inner join EVANS.dbo.Empresa E on E.empr_codigo = M.empr_codigo inner join EVANS.dbo.Vehiculo V on V.vehi_codigo = M.vehi_codigo where day(M.mani_fecha) = @dia and month(M.mani_fecha) = @mes and year(M.mani_fecha) = @año"
                cmdBuscar.Parameters.Add("@dia", SqlDbType.NVarChar, 2).Value = Today.Day
                cmdBuscar.Parameters.Add("@mes", SqlDbType.NVarChar, 2).Value = Today.Month
                cmdBuscar.Parameters.Add("@año", SqlDbType.NVarChar, 4).Value = strBD2
            End If

            If optMes.Checked = True Then
                strSQL = "Select M.mani_codigo, M.mani_numero, M.mani_fecha, E.empr_nombre, V.vehi_placa, M.mani_importe, M.mani_nroguias, M.mani_peso from Manifiesto M inner join EVANS.dbo.Empresa E on E.empr_codigo = M.empr_codigo inner join EVANS.dbo.Vehiculo V on V.vehi_codigo = M.vehi_codigo where month(M.mani_fecha) = @mes and year(M.mani_fecha) = @año"
                cmdBuscar.Parameters.Add("@mes", SqlDbType.NVarChar, 2).Value = cbMes.SelectedIndex + 1
                cmdBuscar.Parameters.Add("@año", SqlDbType.NVarChar, 4).Value = strBD2
            End If

            If optIntervalo.Checked = True Then
                strSQL = "Select M.mani_codigo, M.mani_numero, M.mani_fecha, E.empr_nombre, V.vehi_placa, M.mani_importe, M.mani_nroguias, M.mani_peso from Manifiesto M inner join EVANS.dbo.Empresa E on E.empr_codigo = M.empr_codigo inner join EVANS.dbo.Vehiculo V on V.vehi_codigo = M.vehi_codigo where day(M.mani_fecha) = @dia and month(M.mani_fecha) = @mes and year(M.mani_fecha) = @año"
                Dim fecha As Date = Convert.ToDateTime(txtFecha1.Text)
                cmdBuscar.Parameters.Add("@dia", SqlDbType.NVarChar, 2).Value = fecha.Day
                cmdBuscar.Parameters.Add("@mes", SqlDbType.NVarChar, 2).Value = fecha.Month
                cmdBuscar.Parameters.Add("@año", SqlDbType.NVarChar, 4).Value = fecha.Year
            End If

            cmdBuscar.CommandText = strSQL
            If objConexion2.State = ConnectionState.Closed Then
                objConexion2.Open()
            End If
            drBuscar = cmdBuscar.ExecuteReader

            dgvListado.Rows.Clear()
            Dim i As Integer
            i = 0
            While drBuscar.Read
                dgvListado.Rows.Add()
                dgvListado.Rows(i).Cells(0).Value = NullToString(drBuscar(0))
                dgvListado.Rows(i).Cells(1).Value = NullToString(drBuscar(1))
                dgvListado.Rows(i).Cells(2).Value = NullToString(drBuscar(2))
                dgvListado.Rows(i).Cells(3).Value = NullToString(drBuscar(3))
                dgvListado.Rows(i).Cells(4).Value = NullToString(drBuscar(4))
                dgvListado.Rows(i).Cells(5).Value = FormatNumber(NullToString(drBuscar(5)), 2)
                dgvListado.Rows(i).Cells(6).Value = NullToString(drBuscar(6))
                dgvListado.Rows(i).Cells(7).Value = FormatNumber(NullToString(drBuscar(7)), 2)
                i = i + 1
            End While
            drBuscar.Close()

        Catch ex As SqlException
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        Finally
            If objConexion2.State = ConnectionState.Open Then
                objConexion2.Close()
            End If
        End Try
    End Sub

    Private Sub dgvListado_DoubleClick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles dgvListado.DoubleClick

        Try
            Dim strCodigo As String
            strCodigo = dgvListado.SelectedRows(0).Cells(0).Value

            objManifiesto.Codigo = strCodigo
            If objManifiesto.BuscarXCodigo() Then

                TabControl1.TabPages(1).Enabled = True
                TabControl1.TabPages(0).Enabled = False
                TabControl1.SelectTab(1)

                txtCodigo.Text = objManifiesto.Codigo
                cbTransportista.Text = objManifiesto.Transportista.Nombre
                cbChofer.Text = objManifiesto.Chofer.Nombre
                txtNumero.Text = objManifiesto.Numero
                cbVehiculo.Text = objManifiesto.Vehiculo.Placa
                cbCarreta.Text = objManifiesto.Carreta.Placa
                dtpFecha.Value = objManifiesto.Fecha
                cbEstado.Text = objManifiesto.Estado.Descripcion
                txtUsuario.Text = objManifiesto.Usuario.NombreCompleto
                txtPeso.Text = FormatNumber(objManifiesto.Peso, 2)
                txtNroGuias.Text = objManifiesto.NroGuias
                txtImporte.Text = FormatNumber(objManifiesto.Importe, 2)

                dgvEnviar.Rows.Clear()
                Dim i As Integer
                For i = 0 To objManifiesto.GuiasSeleccionadas.Count - 1
                    dgvEnviar.Rows.Add()
                    dgvEnviar.Rows(i).Cells(0).Value = objManifiesto.GuiasSeleccionadas(i).Codigo
                    dgvEnviar.Rows(i).Cells(1).Value = objManifiesto.GuiasSeleccionadas(i).Serie + "-" + objManifiesto.GuiasSeleccionadas(i).Numero
                    dgvEnviar.Rows(i).Cells(2).Value = objManifiesto.GuiasSeleccionadas(i).FechaEmision
                    dgvEnviar.Rows(i).Cells(3).Value = objManifiesto.GuiasSeleccionadas(i).Destino.Nombre
                    dgvEnviar.Rows(i).Cells(4).Value = FormatNumber(objManifiesto.GuiasSeleccionadas(i).CostoTotal, 2)
                    dgvEnviar.Rows(i).Cells(5).Value = FormatNumber(objManifiesto.GuiasSeleccionadas(i).PesoTotal, 2)
                Next i

                'Desactivar controles
                DesactivarControles(TabControl1, 1, True)

                'Comportamiento de botones
                btnNuevo.Enabled = True
                btnGrabar.Enabled = False
                btnEditar.Enabled = True
                btnCancelar.Enabled = True

                btnGrabar.Text = "Grabar"
                ListarGuiasPendientes()
                LLenarInfoCarga()

            Else
                Return
            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End Try

    End Sub

    Private Sub TabControl1_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TabControl1.SelectedIndexChanged

        If TabControl1.SelectedIndex = 1 Then
            btnImprimir.Enabled = False
            btnEliminar.Enabled = False
        Else
            btnImprimir.Enabled = True
            btnEliminar.Enabled = True
        End If

    End Sub

    Private Sub dgvPendientes_DoubleClick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles dgvPendientes.DoubleClick

        Try
            MostrarGuia(dgvPendientes.SelectedRows(0).Cells(0).Value)
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End Try

    End Sub

    Private Sub dgvEnviar_DoubleClick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles dgvEnviar.DoubleClick

        Try
            MostrarGuia(dgvEnviar.SelectedRows(0).Cells(0).Value)
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End Try

    End Sub

    Private Sub btnEliminar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEliminar.Click

        Try
            If MessageBox.Show("¿Confirma que desea eliminar los registros seleccionados?", "Precaución", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) = Windows.Forms.DialogResult.No Then
                Exit Sub
            End If

            For Each seleccion As DataGridViewRow In dgvListado.SelectedRows
                If objManifiesto.Eliminar(Convert.ToInt32(seleccion.Cells(0).Value)) = False Then
                    MessageBox.Show("Error al eliminar el manifiesto Nº " + seleccion.Cells(1).Value.ToString, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                End If
            Next

            MessageBox.Show("Finalizó la eliminación de registros.", "Confirmación", MessageBoxButtons.OK, MessageBoxIcon.Information)
            btnBuscar.PerformClick()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End Try

    End Sub
End Class