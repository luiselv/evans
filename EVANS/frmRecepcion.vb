Imports System.Data.SqlClient

Public Class frmRecepcion
    Dim objRemitente As New clsCliente
    Dim objDestinatario As New clsCliente
    Dim objVehiculo As New clsVehiculo
    Dim objDestino As New clsDestino
    Dim bolAccion As Boolean = True
    Dim bolCalculos As Boolean = False

    Private Sub frmRecepcion_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing

        Try
            LimpiarRAM()
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        Finally
            If objConexion.State = ConnectionState.Open Then
                objConexion.Close()
            End If

            If objConexion2.State = ConnectionState.Open Then
                objConexion2.Close()
            End If
        End Try

    End Sub

    Private Sub frmRecepcion_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

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

    Private Sub cbRemiID_DropDown(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbRemiID.DropDown

        Try
            LLenarCombo("iden_codigo", "iden_descripcion", "TipoIdentificacion", cbRemiID, False)

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

    Private Sub cbDestiID_DropDown(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbDestiID.DropDown

        Try
            LLenarCombo("iden_codigo", "iden_descripcion", "TipoIdentificacion", cbDestiID, False)

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

    Private Sub txtRemiNroID_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtRemiNroID.KeyPress

        Try
            If e.KeyChar = ChrW(Keys.Enter) Then
                e.Handled = True
                objRemitente = objRemitente.BuscarXID(txtRemiNroID.Text)
                txtRemitente.Text = objRemitente.Nombre
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


    Private Sub optAgencia1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles optAgencia1.Click

        Try
            LLenarCombo("agen_codigo", "agen_direccion", "Agencia", cbDireccionPartida, True)

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

    Private Sub optDireccion1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles optDireccion1.Click

        Try
            Dim i As Integer
            cbDireccionPartida.DataSource = Nothing
            cbDireccionPartida.Items.Clear()
            For i = 0 To objRemitente.Direccion.Count - 1
                cbDireccionPartida.Items.Add(objRemitente.Direccion(i).Direccion + " | " + objRemitente.Direccion(i).Ciudad + " | " + objRemitente.Direccion(i).Provincia)
            Next

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

    Private Sub txtDestiNroID_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtDestiNroID.KeyPress

        Try
            If e.KeyChar = ChrW(Keys.Enter) Then
                e.Handled = True
                objDestinatario = objDestinatario.BuscarXID(txtDestiNroID.Text)
                txtDestinatario.Text = objDestinatario.Nombre
            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End Try

    End Sub

    Private Sub optAgencia2_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles optAgencia2.Click

        Try
            LLenarCombo("agen_codigo", "agen_direccion", "Agencia", cbDireccionDestino, True)

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

    Private Sub optDireccion2_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles optDireccion2.Click

        Try
            Dim i As Integer
            cbDireccionDestino.DataSource = Nothing
            cbDireccionDestino.Items.Clear()
            For i = 0 To objDestinatario.Direccion.Count - 1
                cbDireccionDestino.Items.Add(objDestinatario.Direccion(i).Direccion + " | " + objDestinatario.Direccion(i).Ciudad + " | " + objDestinatario.Direccion(i).Provincia)
            Next

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

    Private Sub btnNuevo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNuevo.Click

        Try
            objRemitente = New clsCliente
            objDestinatario = New clsCliente
            objVehiculo = New clsVehiculo
            objRecepcion = New clsRecepcion
            objDestino = New clsDestino
            bolCalculos = False

            'Comportamiento del control TabControl
            TabControl1.TabPages(1).Enabled = True
            TabControl1.TabPages(0).Enabled = False
            TabControl1.SelectTab(1)
            txtRemiNroID.Focus()

            'Limpiar y activa cajas de texto
            LimpiarControles(TabControl1, 1)
            DesactivarControles(TabControl1, 1, False)
            txtCodigo.ReadOnly = True
            txtUsuario.ReadOnly = True

            'Comportamiento de botones
            btnNuevo.Enabled = False
            btnGrabar.Enabled = True
            btnCancelar.Enabled = True

            cbEstado.Text = "ACTIVO"
            lblBultosTotales.Text = 0
            lblCostoTotal.Text = 0
            lblPesoTotal.Text = 0
            txtUsuario.Text = objUsuarioActual.NombreCompleto

            optAgencia1.PerformClick()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End Try

    End Sub

    Private Sub btnGrabar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnGrabar.Click

        Try
            If cbDestino.Text = "" Or cbEstado.Text = "" Or txtRemitente.Text = "" Or txtRemiNroID.Text = "" Or cbRemiID.Text = "" Or txtDestinatario.Text = "" Or txtDestiNroID.Text = "" Or cbDestiID.Text = "" Or dgvDetalle.RowCount < 2 Then
                MessageBox.Show("Datos incompletos", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Exit Sub
            End If

            If bolCalculos = False And bolAccion = True Then
                If MessageBox.Show("No se hicieron cálculos de IGV en esta recepción. ¿Desea grabarla de todas maneras?", "Advertencia", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) = Windows.Forms.DialogResult.No Then
                    Exit Sub
                End If
            End If

            objRecepcion.FechaEmision = dtpEmision.Value
            objRecepcion.Remitente = objRemitente.BuscarXID(txtRemiNroID.Text.Trim) ' agregado el 12/06/2011 para solucionar problema de Recepcions que desaparecen (remitente con codigo = 0)
            objRecepcion.TipoDireccionPartida = IIf(optAgencia1.Checked, "0", "1")
            objRecepcion.DireccionPartida = cbDireccionPartida.Text.ToUpper
            objRecepcion.Destinatario = objDestinatario.BuscarXID(txtDestiNroID.Text.Trim) ' agregado el 12/06/2011 para solucionar problema de Recepcions que desaparecen (remitente con codigo = 0)
            objRecepcion.TipoDireccionDestino = IIf(optAgencia2.Checked, "0", "1")
            objRecepcion.DireccionDestino = cbDireccionDestino.Text.ToUpper
            objRecepcion.Destino.Codigo = BuscarCodigo("dest_codigo", "dest_nombre", cbDestino.Text, "Destino")
            objRecepcion.Estado.Codigo = BuscarCodigo("esta_codigo", "esta_descripcion", cbEstado.Text, "Estado")
            objRecepcion.BultosTotales = Convert.ToInt32(lblBultosTotales.Text)
            objRecepcion.PesoTotal = Convert.ToDouble(lblPesoTotal.Text)
            objRecepcion.CostoTotal = Convert.ToDouble(lblCostoTotal.Text)
            objRecepcion.GuiaRemision = txtNroGuia.Text.ToUpper
            objRecepcion.Observacion = txtObservaciones.Text.ToUpper
            objRecepcion.Usuario.Codigo = objUsuarioActual.Codigo
            objRecepcion.GuiaRemision = txtNroGuia.Text.ToUpper

            objRecepcion.Detalles.Clear()
            Dim i As Integer
            For i = 0 To dgvDetalle.RowCount - 2
                Dim Item As New clsDetalleRecepcion
                Item.TipoDocumento = IIf(dgvDetalle.Rows(i).Cells(0).Value = Nothing, "", dgvDetalle.Rows(i).Cells(0).Value)
                Item.NumeroDocumento = IIf(dgvDetalle.Rows(i).Cells(1).Value = Nothing, "", dgvDetalle.Rows(i).Cells(1).Value)
                Item.Cantidad = IIf(dgvDetalle.Rows(i).Cells(2).Value = Nothing, 0, dgvDetalle.Rows(i).Cells(2).Value)
                Item.Descripcion = IIf(dgvDetalle.Rows(i).Cells(3).Value = Nothing, "", dgvDetalle.Rows(i).Cells(3).Value)
                Item.Peso = IIf(dgvDetalle.Rows(i).Cells(4).Value = Nothing, 0, dgvDetalle.Rows(i).Cells(4).Value)
                Item.Unidad = IIf(dgvDetalle.Rows(i).Cells(5).Value = Nothing, "", dgvDetalle.Rows(i).Cells(5).Value)
                Item.Costo = IIf(dgvDetalle.Rows(i).Cells(6).Value = Nothing, 0, dgvDetalle.Rows(i).Cells(6).Value)
                objRecepcion.Detalles.Add(Item)
                Item = Nothing
            Next

            If bolAccion = True Then
                If objRecepcion.Grabar() = True Then
                    'MessageBox.Show("Registro grabado con exito", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    objParametros.CargarParametros()
                Else
                    Exit Sub
                End If
            Else
                If objRecepcion.Actualizar() = True Then
                    'MessageBox.Show("Registro actualizado con exito", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    objParametros.CargarParametros()
                    bolAccion = True
                Else
                    Exit Sub
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

    Private Sub btnSalir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSalir.Click

        Try
            Me.Close()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End Try

    End Sub

    Private Sub dgvDetalle_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dgvDetalle.KeyPress

        Try
            If e.KeyChar = ChrW(Keys.Enter) Then
                e.Handled = True
                Dim i As Integer
                Dim valor, dblCostoTotal, dblBultos, dblPeso As Double
                valor = 0.0
                dblCostoTotal = 0.0
                dblBultos = 0.0
                dblPeso = 0.0
                For i = 0 To dgvDetalle.RowCount - 2
                    dblBultos = dblBultos + dgvDetalle.Rows(i).Cells(2).Value
                    dblPeso = dblPeso + dgvDetalle.Rows(i).Cells(4).Value
                    dblCostoTotal = dblCostoTotal + dgvDetalle.Rows(i).Cells(6).Value

                    valor = dgvDetalle.Rows(i).Cells(6).Value
                    valor = FormatNumber(valor / (1 + (objParametros.PorcentajeIGV / 100)), 2)
                    dgvDetalle.Rows(i).Cells(6).Value = valor
                    dgvDetalle.Rows(i).Cells(6).Style.BackColor = Color.LightGoldenrodYellow
                Next
                lblBultosTotales.Text = dblBultos
                lblPesoTotal.Text = FormatNumber(dblPeso, 2)
                lblCostoTotal.Text = FormatNumber(dblCostoTotal, 2)
                bolCalculos = True
            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub

        End Try

    End Sub

    Private Sub cbDestino_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbDestino.SelectedIndexChanged

        Try
            Dim drDestino As SqlDataReader
            objDestino.Nombre = cbDestino.Text
            drDestino = objDestino.BuscarXNombre
            While drDestino.Read
                txtDistancia.Text = NullToString(drDestino("dest_distanciavirtual"))
            End While
            drDestino.Close()
            drDestino.Dispose()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try

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
                strSQL = "Select G.rece_codigo, R.clie_nombre, D.clie_nombre, G.rece_fechaemision, G.rece_costototal, G.rece_guiaremision from Recepcion G inner join [EVANS].dbo.Cliente R on R.clie_codigo = G.clie_remitente inner join [EVANS].dbo.Cliente D on D.clie_codigo = G.clie_destinatario where day(G.rece_fechaemision) = @dia and month(G.rece_fechaemision) = @mes and year(G.rece_fechaemision) = @año order by G.rece_codigo ASC"
                cmdBuscar.Parameters.Add("@dia", SqlDbType.NVarChar, 2)
                cmdBuscar.Parameters.Add("@mes", SqlDbType.NVarChar, 2)
                cmdBuscar.Parameters.Add("@año", SqlDbType.NVarChar, 4)
                cmdBuscar.Parameters("@dia").Value = Today.Day
                cmdBuscar.Parameters("@mes").Value = Today.Month
                cmdBuscar.Parameters("@año").Value = strBD2
            End If

            If optMes.Checked = True Then
                strSQL = "Select G.rece_codigo, R.clie_nombre, D.clie_nombre, G.rece_fechaemision, G.rece_costototal, G.rece_guiaremision from Recepcion G inner join [EVANS].dbo.Cliente R on R.clie_codigo = G.clie_remitente inner join [EVANS].dbo.Cliente D on D.clie_codigo = G.clie_destinatario where month(G.rece_fechaemision) = @mes and year(G.rece_fechaemision) = @año order by G.rece_codigo ASC"
                cmdBuscar.Parameters.Add("@mes", SqlDbType.NVarChar, 2)
                cmdBuscar.Parameters.Add("@año", SqlDbType.NVarChar, 4)
                cmdBuscar.Parameters("@mes").Value = cbMes.SelectedIndex + 1
                cmdBuscar.Parameters("@año").Value = strBD2
            End If

            If optIntervalo.Checked = True Then
                strSQL = "Select G.rece_codigo, R.clie_nombre, D.clie_nombre, G.rece_fechaemision, G.rece_costototal, G.rece_guiaremision from Recepcion G inner join [EVANS].dbo.Cliente R on R.clie_codigo = G.clie_remitente inner join [EVANS].dbo.Cliente D on D.clie_codigo = G.clie_destinatario where G.rece_fechaemision between convert(datetime,@fecha1) and convert(datetime,@fecha2) order by G.rece_codigo ASC"
                cmdBuscar.Parameters.Add("@fecha1", SqlDbType.NVarChar, 10)
                cmdBuscar.Parameters.Add("@fecha2", SqlDbType.NVarChar, 10)
                cmdBuscar.Parameters("@fecha1").Value = txtFecha1.Text
                cmdBuscar.Parameters("@fecha2").Value = txtFecha2.Text
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
                dgvListado.Rows(i).Cells(4).Value = FormatNumber(NullToString(drBuscar(4)), 2)
                dgvListado.Rows(i).Cells(5).Value = NullToString(drBuscar(5))
                i = i + 1
            End While
            drBuscar.Close()

            If dgvListado.Rows.Count >= 2 Then
                dgvListado.CurrentCell = dgvListado.Rows(dgvListado.Rows.Count - 2).Cells(0)
            End If

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

    Private Sub optHoy_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles optHoy.CheckedChanged

        cbMes.Enabled = False
        txtFecha1.Enabled = False
        txtFecha2.Enabled = False

    End Sub

    Private Sub optMes_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles optMes.CheckedChanged

        cbMes.Enabled = True
        txtFecha1.Enabled = False
        txtFecha2.Enabled = False

    End Sub

    Private Sub optIntervalo_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles optIntervalo.CheckedChanged

        cbMes.Enabled = False
        txtFecha1.Enabled = True
        txtFecha2.Enabled = True

    End Sub

    Private Sub dgvListado_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgvListado.DoubleClick

        Try
            lblBultosTotales.Text = ""
            lblCostoTotal.Text = ""
            lblPesoTotal.Text = ""

            Dim strCodigo As String
            strCodigo = dgvListado.SelectedRows(0).Cells(0).Value

            objRecepcion.Codigo = strCodigo
            If objRecepcion.BuscarXCodigo() Then

                TabControl1.TabPages(1).Enabled = True
                TabControl1.TabPages(0).Enabled = False
                TabControl1.SelectTab(1)

                txtCodigo.Text = objRecepcion.Codigo
                dtpEmision.Value = objRecepcion.FechaEmision
                objRemitente.Codigo = objRecepcion.Remitente.Codigo
                txtRemitente.Text = objRecepcion.Remitente.Nombre
                cbRemiID.Text = objRecepcion.Remitente.TipoID.Descripcion
                cbRemiID.SelectedValue = objRecepcion.Remitente.TipoID.Codigo
                txtRemiNroID.Text = objRecepcion.Remitente.NumeroID
                If objRecepcion.TipoDireccionPartida = 0 Then
                    optAgencia1.Checked = True
                    optDireccion1.Checked = False
                Else
                    optDireccion1.Checked = True
                    optAgencia1.Checked = False
                End If
                cbDireccionPartida.Text = objRecepcion.DireccionPartida
                objDestinatario.Codigo = objRecepcion.Destinatario.Codigo
                txtDestinatario.Text = objRecepcion.Destinatario.Nombre
                cbDestiID.Text = objRecepcion.Destinatario.TipoID.Descripcion
                cbDestiID.SelectedValue = objRecepcion.Destinatario.TipoID.Codigo
                txtDestiNroID.Text = objRecepcion.Destinatario.NumeroID
                If objRecepcion.TipoDireccionDestino = 0 Then
                    optAgencia2.Checked = True
                    optDireccion2.Checked = False
                Else
                    optDireccion2.Checked = True
                    optAgencia2.Checked = False
                End If
                cbDireccionDestino.Text = objRecepcion.DireccionDestino
                cbDestino.Text = objRecepcion.Destino.Nombre
                cbDestino.SelectedValue = objRecepcion.Destino.Codigo
                txtDistancia.Text = objRecepcion.Destino.DistanciaVirtual
                cbEstado.Text = objRecepcion.Estado.Descripcion
                cbEstado.SelectedValue = objRecepcion.Estado.Codigo
                lblBultosTotales.Text = objRecepcion.BultosTotales
                lblPesoTotal.Text = FormatNumber(objRecepcion.PesoTotal, 2)
                lblCostoTotal.Text = FormatNumber(objRecepcion.CostoTotal, 2)
                txtNroGuia.Text = objRecepcion.GuiaRemision
                txtObservaciones.Text = objRecepcion.Observacion
                txtUsuario.Text = objRecepcion.Usuario.NombreCompleto

                dgvDetalle.Rows.Clear()
                Dim i As Integer
                For i = 0 To objRecepcion.Detalles.Count - 1
                    dgvDetalle.Rows.Add()
                    dgvDetalle.Rows(i).Cells(0).Value = IIf(objRecepcion.Detalles(i).TipoDocumento = "", Nothing, objRecepcion.Detalles(i).TipoDocumento)
                    dgvDetalle.Rows(i).Cells(1).Value = IIf(objRecepcion.Detalles(i).NumeroDocumento = "", Nothing, objRecepcion.Detalles(i).NumeroDocumento)
                    dgvDetalle.Rows(i).Cells(2).Value = IIf(objRecepcion.Detalles(i).Cantidad = 0, Nothing, objRecepcion.Detalles(i).Cantidad)
                    dgvDetalle.Rows(i).Cells(3).Value = IIf(objRecepcion.Detalles(i).Descripcion = "", Nothing, objRecepcion.Detalles(i).Descripcion)
                    dgvDetalle.Rows(i).Cells(4).Value = IIf(objRecepcion.Detalles(i).Peso = 0, Nothing, objRecepcion.Detalles(i).Peso)
                    dgvDetalle.Rows(i).Cells(5).Value = IIf(objRecepcion.Detalles(i).Unidad = "", Nothing, objRecepcion.Detalles(i).Unidad)
                    dgvDetalle.Rows(i).Cells(6).Value = IIf(objRecepcion.Detalles(i).Costo = 0, Nothing, objRecepcion.Detalles(i).Costo)
                Next i

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
            Exit Sub
        End Try

    End Sub

    Private Sub cbDestino_DropDown(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbDestino.DropDown

        Try
            LLenarCombo("dest_codigo", "dest_nombre", "Destino", cbDestino, True)

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
            Exit Sub
        End Try

    End Sub

    Private Sub TabControl1_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TabControl1.SelectedIndexChanged

        If TabControl1.SelectedIndex = 1 Then
            btnGenerarGuia.Enabled = False
            btnEliminar.Enabled = False
        Else
            btnGenerarGuia.Enabled = True
            btnEliminar.Enabled = True
        End If

    End Sub

    Private Sub txtRemitente_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtRemitente.KeyPress

        Try
            If e.KeyChar = ChrW(Keys.Enter) Then
                e.Handled = True
                objRemitente = objRemitente.BuscarClienteXNombre(txtRemitente.Text.Trim)
                txtRemiNroID.Clear()
                txtRemiNroID.Text = objRemitente.NumeroID
            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End Try

    End Sub

    Private Sub btnEliminar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEliminar.Click

        Try
            If MessageBox.Show("¿Confirma que desea eliminar los registros seleccionados?", "Precaución", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) = Windows.Forms.DialogResult.No Then
                Exit Sub
            End If

            For Each seleccion As DataGridViewRow In dgvListado.SelectedRows
                If seleccion.Cells(6).Value <> "" Then
                    If MessageBox.Show("La Recepcion " + seleccion.Cells(1).Value.ToString + " tiene asignado el documento de venta " + seleccion.Cells(6).Value.ToString + vbCrLf + "¿Confirma que desea eliminar esta guía?" + vbCrLf + " Si responde SI asegúrese de actualizar el número de GR/T del documento de venta.", "Advertencia", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) = Windows.Forms.DialogResult.Yes Then
                        If objRecepcion.Eliminar(Convert.ToInt32(seleccion.Cells(0).Value)) = False Then
                            MessageBox.Show("Error al eliminar la Recepcion Nº " + seleccion.Cells(1).Value.ToString, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        End If
                    End If
                Else
                    If objRecepcion.Eliminar(Convert.ToInt32(seleccion.Cells(0).Value)) = False Then
                        MessageBox.Show("Error al eliminar la Recepcion Nº " + seleccion.Cells(1).Value.ToString, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    End If
                End If
            Next

            MessageBox.Show("Finalizó la eliminación de registros.", "Confirmación", MessageBoxButtons.OK, MessageBoxIcon.Information)
            btnBuscar.PerformClick()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End Try

    End Sub

    Private Sub txtDestinatario_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtDestinatario.KeyPress

        Try
            If e.KeyChar = ChrW(Keys.Enter) Then
                e.Handled = True
                objDestinatario = objDestinatario.BuscarClienteXNombre(txtDestinatario.Text.Trim)
                txtDestiNroID.Clear()
                txtDestiNroID.Text = objDestinatario.NumeroID
            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End Try

    End Sub

    Private Sub btnGenerarGuia_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnGenerarGuia.Click
        Try
            bolGenerandoGuia = True
            If dgvListado.SelectedRows.Count = 0 Or dgvListado.Rows.Count < 2 Then
                Exit Sub
            End If

            If dgvListado.SelectedRows(0).Cells(5).Value <> "" Then
                If MessageBox.Show("Esta recepción ya tiene una guia de remisión." + vbCrLf + "¿Continuar con la creación de una nueva guía?", "Advertencia", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) = Windows.Forms.DialogResult.No Then
                    Exit Sub
                End If
            End If

            Dim strCodigo As String
            strCodigo = dgvListado.SelectedRows(0).Cells(0).Value
            objRecepcion.Codigo = strCodigo
            If objRecepcion.BuscarXCodigo() Then

                If frmGuiaRemision.Visible Then
                    frmGuiaRemision.Activate()
                Else
                    frmGuiaRemision.MdiParent = frmPrincipal
                    frmGuiaRemision.Show()
                End If

                frmGuiaRemision.btnNuevo.PerformClick()
                frmGuiaRemision.txtSerie.Text = ""
                frmGuiaRemision.txtNumero.Text = ""
                frmGuiaRemision.dtpEmision.Value = dtpEmision.Value
                frmGuiaRemision.dtpTraslado.Value = dtpEmision.Value
                frmGuiaRemision.txtRemitente.Text = objRecepcion.Remitente.Nombre
                frmGuiaRemision.cbRemiID.Text = objRecepcion.Remitente.TipoID.Descripcion
                frmGuiaRemision.cbRemiID.SelectedValue = objRecepcion.Remitente.TipoID.Codigo
                frmGuiaRemision.txtRemiNroID.Text = objRecepcion.Remitente.NumeroID
                If objRecepcion.TipoDireccionPartida = 0 Then
                    frmGuiaRemision.optAgencia1.Checked = True
                    frmGuiaRemision.optDireccion1.Checked = False
                Else
                    frmGuiaRemision.optDireccion1.Checked = True
                    frmGuiaRemision.optAgencia1.Checked = False
                End If
                frmGuiaRemision.cbDireccionPartida.Text = objRecepcion.DireccionPartida
                frmGuiaRemision.txtDestinatario.Text = objRecepcion.Destinatario.Nombre
                frmGuiaRemision.cbDestiID.Text = objRecepcion.Destinatario.TipoID.Descripcion
                frmGuiaRemision.cbDestiID.SelectedValue = objRecepcion.Destinatario.TipoID.Codigo
                frmGuiaRemision.txtDestiNroID.Text = objRecepcion.Destinatario.NumeroID
                If objRecepcion.TipoDireccionDestino = 0 Then
                    frmGuiaRemision.optAgencia2.Checked = True
                    frmGuiaRemision.optDireccion2.Checked = False
                Else
                    frmGuiaRemision.optDireccion2.Checked = True
                    frmGuiaRemision.optAgencia2.Checked = False
                End If
                frmGuiaRemision.cbDireccionDestino.Text = objRecepcion.DireccionDestino
                frmGuiaRemision.cbDestino.Text = objRecepcion.Destino.Nombre
                frmGuiaRemision.cbDestino.SelectedValue = objRecepcion.Destino.Codigo
                frmGuiaRemision.txtDistancia.Text = objRecepcion.Destino.DistanciaVirtual
                frmGuiaRemision.cbVehiculo.Text = ""
                frmGuiaRemision.cbVehiculo.SelectedValue = ""
                frmGuiaRemision.cbCarreta.Text = ""
                frmGuiaRemision.cbCarreta.SelectedValue = ""
                frmGuiaRemision.cbChofer.Text = ""
                frmGuiaRemision.cbChofer.SelectedValue = ""
                frmGuiaRemision.cbSubcontratada.Text = ""
                frmGuiaRemision.cbSubcontratada.SelectedValue = ""
                frmGuiaRemision.cbEstado.Text = objRecepcion.Estado.Descripcion
                frmGuiaRemision.cbEstado.SelectedValue = objRecepcion.Estado.Codigo
                frmGuiaRemision.lblBultosTotales.Text = objRecepcion.BultosTotales
                frmGuiaRemision.lblPesoTotal.Text = FormatNumber(objRecepcion.PesoTotal, 2)
                frmGuiaRemision.lblCostoTotal.Text = FormatNumber(objRecepcion.CostoTotal, 2)
                frmGuiaRemision.cbDVTipo.Text = "FACTURA"
                frmGuiaRemision.cbDVTipo.SelectedValue = 1
                frmGuiaRemision.txtDocVenta.Text = ""
                frmGuiaRemision.txtObservaciones.Text = objRecepcion.Observacion
                frmGuiaRemision.txtUsuario.Text = objRecepcion.Usuario.NombreCompleto
                frmGuiaRemision.chkManifiesto.Checked = 0
                frmGuiaRemision.txtManifiesto.Text = ""

                frmGuiaRemision.dgvDetalle.Rows.Clear()
                Dim i As Integer
                For i = 0 To objRecepcion.Detalles.Count - 1
                    frmGuiaRemision.dgvDetalle.Rows.Add()
                    frmGuiaRemision.dgvDetalle.Rows(i).Cells(0).Value = objRecepcion.Detalles(i).TipoDocumento
                    frmGuiaRemision.dgvDetalle.Rows(i).Cells(1).Value = objRecepcion.Detalles(i).NumeroDocumento
                    frmGuiaRemision.dgvDetalle.Rows(i).Cells(2).Value = IIf(objRecepcion.Detalles(i).Cantidad = 0, Nothing, objRecepcion.Detalles(i).Cantidad)
                    frmGuiaRemision.dgvDetalle.Rows(i).Cells(3).Value = IIf(objRecepcion.Detalles(i).Descripcion = "", Nothing, objRecepcion.Detalles(i).Descripcion)
                    frmGuiaRemision.dgvDetalle.Rows(i).Cells(4).Value = IIf(objRecepcion.Detalles(i).Peso = 0, Nothing, objRecepcion.Detalles(i).Peso)
                    frmGuiaRemision.dgvDetalle.Rows(i).Cells(5).Value = IIf(objRecepcion.Detalles(i).Unidad = "", Nothing, objRecepcion.Detalles(i).Unidad)
                    frmGuiaRemision.dgvDetalle.Rows(i).Cells(6).Value = IIf(objRecepcion.Detalles(i).Costo = 0, Nothing, objRecepcion.Detalles(i).Costo)
                Next i
            
            Else
                Return
            End If

        Catch ex As Exception
            bolGenerandoGuia = False
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End Try
    End Sub

    Private Sub dgvListado_RowsAdded(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewRowsAddedEventArgs) Handles dgvListado.RowsAdded
        lblCantReg.Text = dgvListado.RowCount - 1
    End Sub

    Private Sub dgvListado_RowsRemoved(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewRowsRemovedEventArgs) Handles dgvListado.RowsRemoved
        lblCantReg.Text = dgvListado.RowCount - 1
    End Sub
End Class