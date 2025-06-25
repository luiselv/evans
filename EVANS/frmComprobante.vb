Imports System.Data.SqlClient

Public Class frmComprobante

    Dim objRemitente As New clsCliente
    Dim objDestinatario As New clsCliente
    Dim bolAccion As Boolean = True
    Dim bolCalculos As Boolean = False

    Private Sub frmComprobante_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing

        LimpiarRAM()

    End Sub

    Private Sub frmComprobante_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

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

    Private Sub txtSerie_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtSerie.LostFocus

        txtSerie.Text = txtSerie.Text.ToString.PadLeft(4, "0")

    End Sub

    Private Sub txtNumero_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtNumero.LostFocus

        txtNumero.Text = txtNumero.Text.ToString.PadLeft(6, "0")

    End Sub

    Private Sub txtDestiNroID_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtDestiNroID.KeyPress

        Try
            If e.KeyChar = ChrW(Keys.Enter) Then
                e.Handled = True
                objDestinatario = objDestinatario.BuscarXID(txtDestiNroID.Text)
                txtDestinatario.Clear()
                txtDestinatario.Text = objDestinatario.Nombre
                CargarDirecciones()
            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End Try

    End Sub

    Private Sub CargarDirecciones()

        Try
            cbDireccionDestino.Items.Clear()
            For i As Integer = 0 To objDestinatario.Direccion.Count - 1
                cbDireccionDestino.Items.Add(objDestinatario.Direccion(i).Direccion + " | " + objDestinatario.Direccion(i).Ciudad + " | " + objDestinatario.Direccion(i).Provincia)
            Next

        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End Try

    End Sub

    Private Sub txtRemiNroID_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtRemiNroID.KeyPress

        Try
            If e.KeyChar = ChrW(Keys.Enter) Then
                e.Handled = True
                objRemitente = objRemitente.BuscarXID(txtRemiNroID.Text)
                txtRemitente.Clear()
                txtRemitente.Text = objRemitente.Nombre
            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End Try

    End Sub

    Private Sub cbDVTipo_DropDown(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbDVTipo.DropDown

        Try
            LLenarCombo("tico_codigo", "tico_descripcion", "TipoComprobante", cbDVTipo, True)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End Try

    End Sub

    Private Sub cbEstado_DropDown(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbEstado.DropDown

        Try
            LLenarCombo("esta_codigo", "esta_descripcion", "Estado", cbEstado, False)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End Try

    End Sub

    Private Sub cbSubcontratada_DropDown(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbSubcontratada.DropDown

        Try
            LLenarCombo("empr_codigo", "empr_nombre", "Empresa", cbSubcontratada, True)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End Try

    End Sub

    Private Sub cbSubcontratada_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbSubcontratada.SelectedIndexChanged

        Try
            Dim objSubcontratada As New clsEmpresa
            objSubcontratada.Codigo = cbSubcontratada.SelectedValue
            If objSubcontratada.BuscarXCodigo() Then
                txtRUC.Text = objSubcontratada.RUC
            End If
        Catch ex As Exception
            'MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
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

    
    Private Sub btnGrabar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnGrabar.Click

        Try
            If txtDestiNroID.Text = "" Or txtDestinatario.Text = "" Or cbDireccionDestino.Text = "" Or cbDVTipo.Text = "" Or cbEstado.Text = "" Or dgvDetalle.Rows.Count < 2 Or txtRemiNroID.Text = "" Or txtRemitente.Text = "" Or cbSubcontratada.Text = "" Or txtRUC.Text = "" Or cbDestino.Text = "" Then
                MessageBox.Show("Datos incompletos.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Exit Sub
            End If

            If bolCalculos = False And bolAccion = True And bolGenerandoComprobante = False Then
                If MessageBox.Show("No se hicieron cálculos de IGV en este comprobante. ¿Desea grabarlo de todas maneras?", "Advertencia", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) = Windows.Forms.DialogResult.No Then
                    Exit Sub
                End If
            End If

            Dim intTipoComprobante As Integer
            objComprobante.Serie = objParametros.BoletaSerie
            objComprobante.Numero = txtNumero.Text
            objComprobante.Fecha = dtpEmision.Value
            objComprobante.Destinatario.Codigo = BuscarCodigo("clie_codigo", "clie_nroidentificacion", txtDestiNroID.Text, "Cliente")
            objComprobante.Direccion = cbDireccionDestino.Text
            objComprobante.TipoComprobante.Codigo = BuscarCodigo("tico_codigo", "tico_descripcion", cbDVTipo.Text, "TipoComprobante")
            objComprobante.Estado.Codigo = BuscarCodigo("esta_codigo", "esta_descripcion", cbEstado.Text, "Estado")
            objComprobante.GuiaRemisionTransportista = txtGRT.Text
            objComprobante.Remitente.Codigo = BuscarCodigo("clie_codigo", "clie_nroidentificacion", txtRemiNroID.Text, "Cliente")
            objComprobante.Transportista.Codigo = BuscarCodigo("empr_codigo", "empr_nombre", cbSubcontratada.Text, "Empresa")
            objComprobante.Destino.Codigo = BuscarCodigo("dest_codigo", "dest_nombre", cbDestino.Text, "Destino")
            objComprobante.Manifiesto = txtManifiesto.Text
            If cbDVTipo.Text = "FACTURA" Then
                objComprobante.ValorVenta = Convert.ToDouble(txtValorVenta.Text)
                objComprobante.IGV = Convert.ToDouble(txtIGV.Text)
                intTipoComprobante = 1
            Else
                objComprobante.ValorVenta = FormatNumber(0, 2)
                objComprobante.IGV = FormatNumber(0, 2)
                intTipoComprobante = 2
            End If

            objComprobante.Total = Convert.ToDouble(txtTotal.Text)
            objComprobante.Impreso = IIf(lblImpreso.Visible = True, 1, 0)
            objComprobante.Usuario = objUsuarioActual

            objComprobante.Detalles.Clear()
            Dim i As Integer
            For i = 0 To dgvDetalle.RowCount - 2
                Dim Item As New clsDetalleComprobante
                Item.Cantidad = IIf(dgvDetalle.Rows(i).Cells(0).Value = Nothing, "0", dgvDetalle.Rows(i).Cells(0).Value)
                Item.Descripcion = IIf(dgvDetalle.Rows(i).Cells(1).Value.ToString.ToUpper = Nothing, "", dgvDetalle.Rows(i).Cells(1).Value.ToString.ToUpper)
                Item.PrecioUnitario = IIf(dgvDetalle.Rows(i).Cells(2).Value = Nothing, "0", dgvDetalle.Rows(i).Cells(2).Value)
                Item.Flete = IIf(dgvDetalle.Rows(i).Cells(3).Value = Nothing, "0", dgvDetalle.Rows(i).Cells(3).Value)
                objComprobante.Detalles.Add(Item)
                Item = Nothing
            Next

            If bolAccion = True Then
                If objComprobante.Grabar(intTipoComprobante) = True Then
                    'MessageBox.Show("Registro grabado con exito", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    bolGenerandoComprobante = False
                    objParametros.CargarParametros()
                Else
                    Exit Sub
                End If
            Else
                If objComprobante.Actualizar() = True Then
                    'MessageBox.Show("Registro actualizado con exito", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    bolAccion = True
                    objParametros.CargarParametros()
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
            Exit Sub
        End Try

    End Sub

    Private Sub btnNuevo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNuevo.Click

        Try
            objRemitente = New clsCliente
            objDestinatario = New clsCliente
            objComprobante = New clsComprobante
            bolCalculos = False

            'Comportamiento del control TabControl
            TabControl1.TabPages(1).Enabled = True
            TabControl1.TabPages(0).Enabled = False
            TabControl1.SelectTab(1)

            'Limpiar y activa cajas de texto
            DesactivarControles(TabControl1, 1, False)
            LimpiarControles(TabControl1, 1)
            txtCodigo.ReadOnly = True
            txtUsuario.ReadOnly = True

            'Comportamiento de botones
            btnNuevo.Enabled = False
            btnGrabar.Enabled = True
            btnCancelar.Enabled = True

            txtDestiNroID.Focus()
            cbEstado.Text = "ACTIVO"
            txtUsuario.Text = objUsuarioActual.NombreCompleto
            cbDVTipo.Text = "FACTURA"

        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End Try

    End Sub

    Private Sub btnEditar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEditar.Click

        Try
            'Activa las cajas de texto
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
                RealizarCalculos()
                bolCalculos = True
            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End Try

    End Sub

    Private Sub RealizarCalculos()
        Try

            Dim i As Integer
            Dim dblTotal, dblValorVenta, valor As Double
            dblTotal = 0.0
            dblValorVenta = 0.0
            valor = 0.0
            If cbDVTipo.Text = "FACTURA" Then
                For i = 0 To dgvDetalle.RowCount - 2
                    dblTotal = dblTotal + Convert.ToDouble(dgvDetalle.Rows(i).Cells(3).Value)

                    valor = dgvDetalle.Rows(i).Cells(3).Value
                    valor = valor / Convert.ToDouble(1 + (objParametros.PorcentajeIGV / 100))
                    dgvDetalle.Rows(i).Cells(3).Value = FormatNumber(valor, 2)
                    dblValorVenta = dblValorVenta + Convert.ToDouble(dgvDetalle.Rows(i).Cells(3).Value)

                    'pintar las celdas de color amarillo
                    dgvDetalle.Rows(i).Cells(3).Style.BackColor = Color.LightGoldenrodYellow
                Next
                txtLetras.Text = NumerosALetras(dblTotal).ToUpper + "/100 SOLES"  'agregado para solucionar problema de convertir S/.2000 a letras (25/01/2011)
                txtTotal.Text = FormatNumber(dblTotal, 2)
                txtValorVenta.Text = dblValorVenta
                txtIGV.Text = dblTotal - dblValorVenta
            Else
                For i = 0 To dgvDetalle.RowCount - 2
                    dblTotal = dblTotal + Convert.ToDouble(dgvDetalle.Rows(i).Cells(3).Value)

                    'pintar las celdas de color amarillo
                    dgvDetalle.Rows(i).Cells(3).Style.BackColor = Color.LightGoldenrodYellow
                Next
                txtLetras.Text = NumerosALetras(dblTotal).ToUpper + "/100 SOLES"  'agregado para solucionar problema de convertir S/.2000 a letras (25/01/2011)
                txtTotal.Text = FormatNumber(dblTotal, 2)
            End If
            
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
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
                strSQL = "Select C.comp_codigo, (C.comp_serie + '-' + C.comp_numero), D.clie_nombre, C.comp_fecha, C.comp_total, TC.tico_descripcion, C.comp_impreso from Comprobante C inner join EVANS.dbo.Cliente D on D.clie_codigo = C.clie_destinatario inner join EVANS.dbo.TipoComprobante TC on TC.tico_codigo = C.tico_codigo where day(C.comp_fecha) = @dia and month(C.comp_fecha) = @mes and year(C.comp_fecha) = @año order by C.comp_codigo ASC"
                cmdBuscar.Parameters.Add("@dia", SqlDbType.NVarChar, 2).Value = Today.Day
                cmdBuscar.Parameters.Add("@mes", SqlDbType.NVarChar, 2).Value = Today.Month
                cmdBuscar.Parameters.Add("@año", SqlDbType.NVarChar, 4).Value = strBD2
            End If

            If optMes.Checked = True Then
                strSQL = "Select C.comp_codigo, (C.comp_serie + '-' + C.comp_numero), D.clie_nombre, C.comp_fecha, C.comp_total, TC.tico_descripcion, C.comp_impreso from Comprobante C inner join EVANS.dbo.Cliente D on D.clie_codigo = C.clie_destinatario inner join EVANS.dbo.TipoComprobante TC on TC.tico_codigo = C.tico_codigo where month(C.comp_fecha) = @mes and year(C.comp_fecha) = @año order by C.comp_codigo ASC"
                cmdBuscar.Parameters.Add("@mes", SqlDbType.NVarChar, 2).Value = cbMes.SelectedIndex + 1
                cmdBuscar.Parameters.Add("@año", SqlDbType.NVarChar, 4).Value = strBD2
            End If

            If optIntervalo.Checked = True Then
                strSQL = "Select C.comp_codigo, (C.comp_serie + '-' + C.comp_numero), D.clie_nombre, C.comp_fecha, C.comp_total, TC.tico_descripcion, C.comp_impreso from Comprobante C inner join EVANS.dbo.Cliente D on D.clie_codigo = C.clie_destinatario inner join EVANS.dbo.TipoComprobante TC on TC.tico_codigo = C.tico_codigo where day(C.comp_fecha) = @dia and month(C.comp_fecha) = @mes and year(C.comp_fecha) = @año order by C.comp_codigo ASC"
                Dim fecha As Date = Convert.ToDateTime(txtFecha1.Text)
                cmdBuscar.Parameters.Add("@dia", SqlDbType.NVarChar, 2).Value = fecha.Day
                cmdBuscar.Parameters.Add("@mes", SqlDbType.NVarChar, 2).Value = fecha.Month
                cmdBuscar.Parameters.Add("@año", SqlDbType.NVarChar, 4).Value = fecha.Year
            End If

            If optCliente.Checked = True Then
                strSQL = "Select C.comp_codigo, (C.comp_serie + '-' + C.comp_numero), D.clie_nombre, C.comp_fecha, C.comp_total, TC.tico_descripcion, C.comp_impreso from Comprobante C inner join EVANS.dbo.Cliente D on D.clie_codigo = C.clie_destinatario inner join EVANS.dbo.TipoComprobante TC on TC.tico_codigo = C.tico_codigo where D.clie_nombre like @clie_nombre + '%' order by C.comp_codigo ASC"
                cmdBuscar.Parameters.Add("@clie_nombre", SqlDbType.NVarChar, 100).Value = txtCliente.Text
            End If

            cmdBuscar.CommandText = strSQL
            If objConexion2.State = ConnectionState.Closed Then
                objConexion2.Open()
            End If
            drBuscar = cmdBuscar.ExecuteReader

            dgvListado.Rows.Clear()
            Dim i As Integer = 0
            While drBuscar.Read
                dgvListado.Rows.Add()
                dgvListado.Rows(i).Cells(0).Value = NullToString(drBuscar(0))
                dgvListado.Rows(i).Cells(1).Value = NullToString(drBuscar(1))
                dgvListado.Rows(i).Cells(2).Value = NullToString(drBuscar(2))
                dgvListado.Rows(i).Cells(3).Value = FormatDateTime(NullToString(drBuscar(3)), DateFormat.ShortDate)
                dgvListado.Rows(i).Cells(4).Value = FormatNumber(NullToString(drBuscar(4)), 2)
                dgvListado.Rows(i).Cells(5).Value = NullToString(drBuscar(5))
                dgvListado.Rows(i).Cells(6).Value = IIf(NullToString(drBuscar(6)) = 1, "SI", "NO")
                i = i + 1
            End While
            drBuscar.Close()

            If dgvListado.Rows.Count >= 2 Then
                dgvListado.CurrentCell = dgvListado.Rows(dgvListado.Rows.Count - 2).Cells(0)
            End If

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

    Private Sub dgvListado_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgvListado.DoubleClick

        Try
            Dim intCodigo As Integer
            intCodigo = dgvListado.SelectedRows(0).Cells(0).Value
            objComprobante.Codigo = intCodigo
            If objComprobante.BuscarXCodigo() Then

                TabControl1.TabPages(1).Enabled = True
                TabControl1.TabPages(0).Enabled = False
                TabControl1.SelectTab(1)

                lblImpreso.Visible = IIf(objComprobante.Impreso = 1, True, False)
                txtCodigo.Text = objComprobante.Codigo
                txtUsuario.Text = objComprobante.Usuario.NombreCompleto
                txtSerie.Text = objComprobante.Serie
                txtNumero.Text = objComprobante.Numero
                dtpEmision.Value = objComprobante.Fecha
                objDestinatario = objComprobante.Destinatario
                txtDestiNroID.Text = objComprobante.Destinatario.NumeroID
                txtDestinatario.Text = objComprobante.Destinatario.Nombre
                cbDireccionDestino.Text = objComprobante.Direccion
                cbDVTipo.Text = objComprobante.TipoComprobante.Descripcion
                cbDVTipo.SelectedValue = objComprobante.TipoComprobante.Codigo
                cbEstado.Text = objComprobante.Estado.Descripcion
                cbEstado.SelectedValue = objComprobante.Estado.Codigo
                txtGRT.Text = objComprobante.GuiaRemisionTransportista
                txtValorVenta.Text = FormatNumber(objComprobante.ValorVenta, 2)
                txtIGV.Text = FormatNumber(objComprobante.IGV, 2)
                txtTotal.Text = FormatNumber(objComprobante.Total, 2)
                objRemitente = objComprobante.Remitente
                txtRemiNroID.Text = objComprobante.Remitente.NumeroID
                txtRemitente.Text = objComprobante.Remitente.Nombre
                cbSubcontratada.Text = objComprobante.Transportista.Nombre
                cbSubcontratada.SelectedValue = objComprobante.Transportista.Codigo
                txtRUC.Text = objComprobante.Transportista.RUC
                cbDestino.Text = objComprobante.Destino.Nombre
                cbDestino.SelectedValue = objComprobante.Destino.Codigo
                txtManifiesto.Text = objComprobante.Manifiesto

                dgvDetalle.Rows.Clear()
                Dim i As Integer
                For i = 0 To objComprobante.Detalles.Count - 1
                    dgvDetalle.Rows.Add()
                    dgvDetalle.Rows(i).Cells(0).Value = IIf(objComprobante.Detalles(i).Cantidad = 0, Nothing, objComprobante.Detalles(i).Cantidad)
                    dgvDetalle.Rows(i).Cells(1).Value = IIf(objComprobante.Detalles(i).Descripcion = "", Nothing, objComprobante.Detalles(i).Descripcion)
                    dgvDetalle.Rows(i).Cells(2).Value = IIf(objComprobante.Detalles(i).PrecioUnitario = 0, Nothing, objComprobante.Detalles(i).PrecioUnitario)
                    dgvDetalle.Rows(i).Cells(3).Value = IIf(objComprobante.Detalles(i).Flete = 0, Nothing, objComprobante.Detalles(i).Flete)
                Next i

                txtLetras.Text = NumerosALetras(objComprobante.Total.ToString).ToUpper + "/100 SOLES"  'agregado para solucionar problema de convertir S/.2000 a letras (25/01/2011)

                'Desactivar controles
                DesactivarControles(TabControl1, 1, True)

                'Comportamiento de botones
                btnNuevo.Enabled = True
                btnGrabar.Enabled = False
                btnEditar.Enabled = True
                btnCancelar.Enabled = True

                btnGrabar.Text = "Grabar"

            Else
                Exit Sub
            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End Try

    End Sub

    Private Sub optHoy_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles optHoy.CheckedChanged

        cbMes.Enabled = False
        txtFecha1.Enabled = False
        txtCliente.Enabled = False

    End Sub

    Private Sub optMes_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles optMes.CheckedChanged

        cbMes.Enabled = True
        txtFecha1.Enabled = False
        txtCliente.Enabled = False

    End Sub

    Private Sub optIntervalo_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles optIntervalo.CheckedChanged

        cbMes.Enabled = False
        txtFecha1.Enabled = True
        txtCliente.Enabled = False

    End Sub

    'Private Sub txtTotal_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtTotal.TextChanged

    '    If txtTotal.Text <> "" Then
    '        txtLetras.Text = NumerosALetras(txtTotal.Text).ToUpper + "/100 SOLES"
    '    End If

    'End Sub

    Private Sub btnImprimir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnImprimir.Click

        Try
            If dgvListado.RowCount <= 1 Or dgvListado.SelectedRows.Count <= 0 Then
                Exit Sub
            End If

            Dim intCodigo As Integer
            intCodigo = dgvListado.SelectedRows(0).Cells(0).Value
            objComprobante.Codigo = intCodigo

            If objComprobante.BuscarXCodigo() Then
                Select Case objComprobante.TipoComprobante.Codigo
                    Case 1
                        PrevisualizarFactura()
                    Case 2
                        PrevisualizarBoleta()
                End Select

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End Try

    End Sub

    Private Sub optCliente_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles optCliente.CheckedChanged

        cbMes.Enabled = False
        txtFecha1.Enabled = False
        txtCliente.Enabled = True
        txtCliente.Focus()

    End Sub

    Private Sub PrevisualizarFactura()

        If frmImprimirFactura.Visible Then
            frmImprimirFactura.Close()
            frmImprimirFactura.MdiParent = frmPrincipal
            frmImprimirFactura.Show()
        Else
            frmImprimirFactura.MdiParent = frmPrincipal
            frmImprimirFactura.Show()
        End If

        Dim ctrl As Control
        Dim lbl As Label
        For Each ctrl In frmImprimirFactura.Controls
            If TypeOf ctrl Is Label Then
                lbl = CType(ctrl, Label)
                lbl.Text = ""
                For i As Integer = 1 To objComprobante.Detalles.Count
                    If lbl.TabIndex = i Then
                        lbl.Text = IIf(objComprobante.Detalles(i - 1).Cantidad = 0, "", objComprobante.Detalles(i - 1).Cantidad)
                    End If
                    If lbl.TabIndex = i + 10 Then
                        lbl.Text = objComprobante.Detalles(i - 1).Descripcion
                    End If
                    If lbl.TabIndex = i + 20 Then
                        lbl.Text = IIf(objComprobante.Detalles(i - 1).PrecioUnitario = 0, "", FormatNumber(objComprobante.Detalles(i - 1).PrecioUnitario, 2))
                    End If
                    If lbl.TabIndex = i + 30 Then
                        lbl.Text = IIf(objComprobante.Detalles(i - 1).Flete = 0, "", "S/ " + FormatNumber(objComprobante.Detalles(i - 1).Flete, 2))
                    End If
                Next
            End If
        Next

        frmImprimirFactura.lblDia.Text = objComprobante.Fecha.Day
        frmImprimirFactura.lblMes.Text = objComprobante.Fecha.ToString("MMMM").ToUpper
        frmImprimirFactura.lblAño.Text = objComprobante.Fecha.Year.ToString.Substring(2)
        frmImprimirFactura.lblCliente.Text = objComprobante.Destinatario.Nombre

        Dim Arreglo As New List(Of String)
        For Each texto As String In objComprobante.Direccion.Split("|")
            Arreglo.Add(texto)
        Next
        If Arreglo.Count >= 2 Then
            frmImprimirFactura.lblDireccion.Text = Arreglo(0).ToString + " - " + Arreglo(1).ToString
        End If

        frmImprimirFactura.lblRUC.Text = objComprobante.Destinatario.NumeroID
        frmImprimirFactura.lblGRT.Text = IIf(objComprobante.GuiaRemisionTransportista = "", "", "GR/T: " + objComprobante.GuiaRemisionTransportista)
        frmImprimirFactura.lblVVenta.Text = FormatNumber(objComprobante.ValorVenta, 2)
        frmImprimirFactura.lblVVenta.Text = "S/ " + frmImprimirFactura.lblVVenta.Text
        frmImprimirFactura.lblIGV.Text = FormatNumber(objComprobante.IGV, 2)
        frmImprimirFactura.lblPorcentajeIGV.Text = objParametros.PorcentajeIGV.ToString + "%"
        frmImprimirFactura.lblTotal.Text = FormatNumber(objComprobante.Total, 2)
        frmImprimirFactura.lblTotal.Text = "S/ " + frmImprimirFactura.lblTotal.Text
        frmImprimirFactura.lblRemitente.Text = objComprobante.Remitente.Nombre

        If objComprobante.Transportista.Codigo <> 1 Then
            frmImprimirFactura.lblTransportista.Text = objComprobante.Transportista.Nombre
            frmImprimirFactura.lblTranspRUC.Text = objComprobante.Transportista.RUC
        End If
        
        frmImprimirFactura.lblManifiesto.Text = objComprobante.Manifiesto
        frmImprimirFactura.lblDestino.Text = objComprobante.Destino.Nombre
        frmImprimirFactura.lblLetras.Text = NumerosALetras(objComprobante.Total.ToString).ToUpper + "/100 SOLES"

    End Sub

    Private Sub PrevisualizarBoleta()

        If frmImprimirBoleta.Visible Then
            frmImprimirBoleta.Close()
            frmImprimirBoleta.MdiParent = frmPrincipal
            frmImprimirBoleta.Show()
        Else
            frmImprimirBoleta.MdiParent = frmPrincipal
            frmImprimirBoleta.Show()
        End If

        Dim ctrl As Control
        Dim lbl As Label
        For Each ctrl In frmImprimirBoleta.Controls
            If TypeOf ctrl Is Label Then
                lbl = CType(ctrl, Label)
                lbl.Text = ""
                For i As Integer = 1 To objComprobante.Detalles.Count
                    If lbl.TabIndex = i Then
                        lbl.Text = IIf(objComprobante.Detalles(i - 1).Cantidad = 0, "", objComprobante.Detalles(i - 1).Cantidad)
                    End If
                    If lbl.TabIndex = i + 10 Then
                        lbl.Text = objComprobante.Detalles(i - 1).Descripcion
                    End If
                    If lbl.TabIndex = i + 20 Then
                        lbl.Text = IIf(objComprobante.Detalles(i - 1).Flete = 0, "", "S/ " + FormatNumber(objComprobante.Detalles(i - 1).Flete, 2))
                    End If
                Next
            End If
        Next

        frmImprimirBoleta.lblDia.Text = objComprobante.Fecha.Day
        frmImprimirBoleta.lblMes.Text = objComprobante.Fecha.ToString("MMMM").ToUpper
        frmImprimirBoleta.lblAño.Text = objComprobante.Fecha.Year.ToString.Substring(2)
        frmImprimirBoleta.lblCliente.Text = objComprobante.Destinatario.Nombre

        Dim Arreglo As New List(Of String)
        For Each texto As String In objComprobante.Direccion.Split("|")
            Arreglo.Add(texto)
        Next
        If Arreglo.Count >= 2 Then
            frmImprimirBoleta.lblDireccion.Text = Arreglo(0).ToString + " - " + Arreglo(1).ToString
        End If

        frmImprimirBoleta.lblDNI.Text = objComprobante.Destinatario.NumeroID
        frmImprimirBoleta.lblGRT.Text = IIf(objComprobante.GuiaRemisionTransportista = "", "", "GR/T: " + objComprobante.GuiaRemisionTransportista)
        frmImprimirBoleta.lblTotal.Text = FormatNumber(objComprobante.Total, 2)
        frmImprimirBoleta.lblTotal.Text = "S/ " + frmImprimirBoleta.lblTotal.Text

    End Sub

    Private Sub TabControl1_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TabControl1.SelectedIndexChanged

        If TabControl1.SelectedIndex = 1 Then
            btnImprimir.Enabled = False
            btnEliminar.Enabled = False
            txtNroDoc.Enabled = False
        Else
            btnImprimir.Enabled = True
            btnEliminar.Enabled = True
            txtNroDoc.Enabled = True
        End If

    End Sub

    Private Sub cbDVTipo_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbDVTipo.TextChanged

        txtValorVenta.Clear()
        txtIGV.Clear()
        txtTotal.Clear()
        txtLetras.Clear()

        If cbDVTipo.Text = "FACTURA" Then
            txtValorVenta.ReadOnly = False
            txtIGV.ReadOnly = False
        Else
            txtValorVenta.ReadOnly = True
            txtIGV.ReadOnly = True
        End If

    End Sub

    Private Sub btnEliminar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEliminar.Click

        Try
            If MessageBox.Show("¿Confirma que desea eliminar los registros seleccionados?", "Precaución", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) = Windows.Forms.DialogResult.No Then
                Exit Sub
            End If

            For Each seleccion As DataGridViewRow In dgvListado.SelectedRows
                If objComprobante.Eliminar(Convert.ToInt32(seleccion.Cells(0).Value)) = False Then
                    MessageBox.Show("Error al eliminar el comprobante Nº " + seleccion.Cells(1).Value.ToString, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                End If
            Next

            MessageBox.Show("Finalizó la eliminación de registros.", "Confirmación", MessageBoxButtons.OK, MessageBoxIcon.Information)
            btnBuscar.PerformClick()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End Try

    End Sub

    Private Sub txtRemitente_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtRemitente.KeyPress

        Try
            If e.KeyChar = ChrW(Keys.Enter) Then
                e.Handled = True
                objRemitente = objRemitente.BuscarClienteXNombre(txtRemitente.Text.Trim)
                txtRemiNroID.Clear()
                txtRemiNroID.Text = objRemitente.NumeroID
                CargarDirecciones()
            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End Try

    End Sub

    Private Sub txtDestinatario_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtDestinatario.KeyPress

        Try
            If e.KeyChar = ChrW(Keys.Enter) Then
                e.Handled = True
                objDestinatario = objDestinatario.BuscarClienteXNombre(txtDestinatario.Text.Trim)
                txtDestiNroID.Clear()
                txtDestiNroID.Text = objDestinatario.NumeroID
                CargarDirecciones()
            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End Try

    End Sub

    Private Sub txtNroDoc_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtNroDoc.KeyPress

        Try
            If e.KeyChar = ChrW(Keys.Enter) Then
                e.Handled = True
                txtNroDoc.Text = txtNroDoc.Text.PadLeft(6, "0")
                Dim lista As New List(Of clsComprobante)
                lista = objComprobante.BuscarXNroDocumento(txtNroDoc.Text)

                dgvListado.Rows.Clear()
                For i As Integer = 0 To lista.Count - 1
                    dgvListado.Rows.Add()
                    dgvListado.Rows(i).Cells(0).Value = lista(i).Codigo
                    dgvListado.Rows(i).Cells(1).Value = lista(i).Serie + "-" + lista(i).Numero
                    dgvListado.Rows(i).Cells(2).Value = lista(i).Destinatario.Nombre
                    dgvListado.Rows(i).Cells(3).Value = lista(i).Fecha
                    dgvListado.Rows(i).Cells(4).Value = lista(i).Total
                    dgvListado.Rows(i).Cells(5).Value = lista(i).TipoComprobante.Descripcion
                    dgvListado.Rows(i).Cells(6).Value = IIf(lista(i).Impreso = 1, "SI", "NO")
                Next i
            End If

        Catch ex As Exception
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