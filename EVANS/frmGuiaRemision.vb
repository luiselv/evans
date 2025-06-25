Imports System.Data.SqlClient

Public Class frmGuiaRemision
    Dim objRemitente As New clsCliente
    Dim objDestinatario As New clsCliente
    Dim objVehiculo As New clsVehiculo
    Dim objDestino As New clsDestino
    Dim bolAccion As Boolean = True
    Dim bolCalculos As Boolean = False

    Private Sub frmGuiaRemision_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing

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

    Private Sub frmGuiaRemision_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

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

    Private Sub cbChofer_DropDown(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbChofer.DropDown

        Try
            LLenarCombo("chof_codigo", "chof_nombre", "Chofer", cbChofer, True)

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

    Private Sub cbVehiculo_DropDown(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbVehiculo.DropDown

        Try
            LLenarCombo("vehi_codigo", "vehi_placa", "Vehiculo", cbVehiculo, True)

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

    Private Sub cbSubcontratada_DropDown(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbSubcontratada.DropDown

        Try
            LLenarCombo("empr_codigo", "empr_nombre", "Empresa", cbSubcontratada, True)

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
            objGuia = New clsGuiaRemision
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
            txtSerie.ReadOnly = True
            txtNumero.ReadOnly = True

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
            If cbDestino.Text = "" Or cbEstado.Text = "" Or txtRemitente.Text = "" Or txtRemiNroID.Text = "" Or cbRemiID.Text = "" Or txtDestinatario.Text = "" Or txtDestiNroID.Text = "" Or cbDestiID.Text = "" Or dgvDetalle.RowCount < 2 Or cbDVTipo.Text = "" Then
                MessageBox.Show("Datos incompletos", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Exit Sub
            End If

            If chkManifiesto.Checked = True Then
                If cbChofer.Text = "" Or cbVehiculo.Text = "" Or cbCarreta.Text = "" Or cbSubcontratada.Text = "" Then
                    MessageBox.Show("Datos incompletos", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Exit Sub
                End If
            End If

            If bolGenerandoGuia <> True Then
                If bolCalculos = False And bolAccion = True Then
                    If MessageBox.Show("No se hicieron cálculos de IGV en esta guía. ¿Desea grabarla de todas maneras?", "Advertencia", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) = Windows.Forms.DialogResult.No Then
                        Exit Sub
                    End If
                End If
            End If

            objGuia.Serie = objParametros.GRemisionSerie
            objGuia.FechaEmision = dtpEmision.Value
            objGuia.FechaTraslado = dtpTraslado.Value
            'objGuia.Remitente = objRemitente         'codigo original puesto en comentario el 12/06/2011
            objGuia.Remitente = objRemitente.BuscarXID(txtRemiNroID.Text.Trim) ' agregado el 12/06/2011 para solucionar problema de guias que desaparecen (remitente con codigo = 0)
            objGuia.TipoDireccionPartida = IIf(optAgencia1.Checked, "0", "1")
            objGuia.DireccionPartida = cbDireccionPartida.Text.ToUpper
            'objGuia.Destinatario = objDestinatario         'codigo original puesto en comentario el 12/06/2011
            objGuia.Destinatario = objDestinatario.BuscarXID(txtDestiNroID.Text.Trim) ' agregado el 12/06/2011 para solucionar problema de guias que desaparecen (remitente con codigo = 0)
            objGuia.TipoDireccionDestino = IIf(optAgencia2.Checked, "0", "1")
            objGuia.DireccionDestino = cbDireccionDestino.Text.ToUpper
            objGuia.Destino.Codigo = BuscarCodigo("dest_codigo", "dest_nombre", cbDestino.Text, "Destino")
            objGuia.Vehiculo.Codigo = BuscarCodigo("vehi_codigo", "vehi_placa", cbVehiculo.Text, "Vehiculo")
            objGuia.Carreta.Codigo = BuscarCodigo("carr_codigo", "carr_placa", cbCarreta.Text, "Carreta")
            objGuia.Chofer.Codigo = BuscarCodigo("chof_codigo", "chof_nombre", cbChofer.Text, "Chofer")
            objGuia.Empresa.Codigo = BuscarCodigo("empr_codigo", "empr_nombre", cbSubcontratada.Text, "Empresa")
            objGuia.Estado.Codigo = BuscarCodigo("esta_codigo", "esta_descripcion", cbEstado.Text, "Estado")
            objGuia.BultosTotales = Convert.ToInt32(lblBultosTotales.Text)
            objGuia.PesoTotal = Convert.ToDouble(lblPesoTotal.Text)
            objGuia.CostoTotal = Convert.ToDouble(lblCostoTotal.Text)
            objGuia.Impreso = IIf(lblImpreso.Visible = True, "1", "0")
            objGuia.TipoDocumentoVenta.Codigo = BuscarCodigo("tico_codigo", "tico_descripcion", cbDVTipo.Text, "TipoComprobante")
            objGuia.DocumentoVenta = txtDocVenta.Text.ToUpper
            objGuia.Observacion = txtObservaciones.Text.ToUpper
            objGuia.Usuario.Codigo = objUsuarioActual.Codigo
            objGuia.TieneManifiesto = IIf(chkManifiesto.Checked = True, "0", "1")
            objGuia.NroManifiesto = txtManifiesto.Text.ToUpper
            objGuia.NroRecepcion = IIf(bolGenerandoGuia = True, objRecepcion.Codigo, 0)

            objGuia.Detalles.Clear()
            Dim i As Integer
            For i = 0 To dgvDetalle.RowCount - 2
                Dim Item As New clsDetalleGuia
                Item.TipoDocumento = IIf(dgvDetalle.Rows(i).Cells(0).Value = Nothing, "", dgvDetalle.Rows(i).Cells(0).Value)
                Item.NumeroDocumento = IIf(dgvDetalle.Rows(i).Cells(1).Value = Nothing, "", dgvDetalle.Rows(i).Cells(1).Value)
                Item.Cantidad = IIf(dgvDetalle.Rows(i).Cells(2).Value = Nothing, 0, dgvDetalle.Rows(i).Cells(2).Value)
                Item.Descripcion = IIf(dgvDetalle.Rows(i).Cells(3).Value = Nothing, "", dgvDetalle.Rows(i).Cells(3).Value)
                Item.Peso = IIf(dgvDetalle.Rows(i).Cells(4).Value = Nothing, 0, dgvDetalle.Rows(i).Cells(4).Value)
                Item.Unidad = IIf(dgvDetalle.Rows(i).Cells(5).Value = Nothing, "", dgvDetalle.Rows(i).Cells(5).Value)
                Item.Costo = IIf(dgvDetalle.Rows(i).Cells(6).Value = Nothing, 0, dgvDetalle.Rows(i).Cells(6).Value)
                objGuia.Detalles.Add(Item)
                Item = Nothing
            Next

            If bolAccion = True Then
                If objGuia.Grabar() = True Then
                    'MessageBox.Show("Registro grabado con exito", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    objParametros.CargarParametros()
                Else
                    Exit Sub
                End If
            Else
                If objGuia.Actualizar() = True Then
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
            bolGenerandoGuia = False

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

    Private Sub txtSerie_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtSerie.LostFocus

        txtSerie.Text = txtSerie.Text.ToString.PadLeft(4, "0")

    End Sub

    Private Sub txtNumero_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtNumero.LostFocus

        txtNumero.Text = txtNumero.Text.ToString.PadLeft(6, "0")

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
                strSQL = "Select G.grem_codigo, (G.grem_serie + '-' + G.grem_numero) as NroDoc, R.clie_nombre, D.clie_nombre, G.grem_fechaemision, G.grem_fechatraslado, G.grem_docventa, G.grem_costototal, G.grem_enviado, G.grem_impreso from guiaremision G inner join [EVANS].dbo.Cliente R on R.clie_codigo = G.clie_remitente inner join [EVANS].dbo.Cliente D on D.clie_codigo = G.clie_destinatario where day(G.grem_fechaemision) = @dia and month(G.grem_fechaemision) = @mes and year(G.grem_fechaemision) = @año order by G.grem_codigo ASC"
                cmdBuscar.Parameters.Add("@dia", SqlDbType.NVarChar, 2)
                cmdBuscar.Parameters.Add("@mes", SqlDbType.NVarChar, 2)
                cmdBuscar.Parameters.Add("@año", SqlDbType.NVarChar, 4)
                cmdBuscar.Parameters("@dia").Value = Today.Day
                cmdBuscar.Parameters("@mes").Value = Today.Month
                cmdBuscar.Parameters("@año").Value = strBD2
            End If

            If optMes.Checked = True Then
                strSQL = "Select G.grem_codigo, (G.grem_serie + '-' + G.grem_numero) as NroDoc, R.clie_nombre, D.clie_nombre, G.grem_fechaemision, G.grem_fechatraslado, G.grem_docventa, G.grem_costototal, G.grem_enviado, G.grem_impreso from guiaremision G inner join [EVANS].dbo.Cliente R on R.clie_codigo = G.clie_remitente inner join [EVANS].dbo.Cliente D on D.clie_codigo = G.clie_destinatario where month(G.grem_fechaemision) = @mes and year(G.grem_fechaemision) = @año order by G.grem_codigo ASC"
                cmdBuscar.Parameters.Add("@mes", SqlDbType.NVarChar, 2)
                cmdBuscar.Parameters.Add("@año", SqlDbType.NVarChar, 4)
                cmdBuscar.Parameters("@mes").Value = cbMes.SelectedIndex + 1
                cmdBuscar.Parameters("@año").Value = strBD2
            End If

            If optIntervalo.Checked = True Then
                strSQL = "Select G.grem_codigo, (G.grem_serie + '-' + G.grem_numero) as NroDoc, R.clie_nombre, D.clie_nombre, G.grem_fechaemision, G.grem_fechatraslado, G.grem_docventa, G.grem_costototal, G.grem_enviado, G.grem_impreso from guiaremision G inner join [EVANS].dbo.Cliente R on R.clie_codigo = G.clie_remitente inner join [EVANS].dbo.Cliente D on D.clie_codigo = G.clie_destinatario where G.grem_fechaemision between convert(datetime,@fecha1) and convert(datetime,@fecha2) order by G.grem_codigo ASC"
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
                dgvListado.Rows(i).Cells(4).Value = NullToString(drBuscar(4))
                dgvListado.Rows(i).Cells(5).Value = NullToString(drBuscar(5))
                dgvListado.Rows(i).Cells(6).Value = NullToString(drBuscar(6))
                dgvListado.Rows(i).Cells(7).Value = FormatNumber(NullToString(drBuscar(7)), 2)
                dgvListado.Rows(i).Cells(8).Value = IIf(NullToString(drBuscar(8)) = 1, "SI", "NO")
                If NullToString(drBuscar(9)) = True Then
                    dgvListado.Rows(i).Cells(9).Value = "SI"
                Else
                    dgvListado.Rows(i).Cells(9).Value = "NO"
                End If
                i = i + 1
            End While
            drBuscar.Close()

            If dgvListado.Rows.Count >= 2 Then
                'dgvListado.Rows(dgvListado.Rows.Count - 2).Selected = True
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

            objGuia.Codigo = strCodigo
            If objGuia.BuscarXCodigo() Then

                TabControl1.TabPages(1).Enabled = True
                TabControl1.TabPages(0).Enabled = False
                TabControl1.SelectTab(1)

                txtCodigo.Text = objGuia.Codigo
                txtSerie.Text = objGuia.Serie
                txtNumero.Text = objGuia.Numero
                dtpEmision.Value = objGuia.FechaEmision
                dtpTraslado.Value = objGuia.FechaTraslado
                objRemitente.Codigo = objGuia.Remitente.Codigo
                txtRemitente.Text = objGuia.Remitente.Nombre
                cbRemiID.Text = objGuia.Remitente.TipoID.Descripcion
                cbRemiID.SelectedValue = objGuia.Remitente.TipoID.Codigo
                txtRemiNroID.Text = objGuia.Remitente.NumeroID
                If objGuia.TipoDireccionPartida = 0 Then
                    optAgencia1.Checked = True
                    optDireccion1.Checked = False
                Else
                    optDireccion1.Checked = True
                    optAgencia1.Checked = False
                End If
                cbDireccionPartida.Text = objGuia.DireccionPartida
                objDestinatario.Codigo = objGuia.Destinatario.Codigo
                txtDestinatario.Text = objGuia.Destinatario.Nombre
                cbDestiID.Text = objGuia.Destinatario.TipoID.Descripcion
                cbDestiID.SelectedValue = objGuia.Destinatario.TipoID.Codigo
                txtDestiNroID.Text = objGuia.Destinatario.NumeroID
                If objGuia.TipoDireccionDestino = 0 Then
                    optAgencia2.Checked = True
                    optDireccion2.Checked = False
                Else
                    optDireccion2.Checked = True
                    optAgencia2.Checked = False
                End If
                cbDireccionDestino.Text = objGuia.DireccionDestino
                cbDestino.Text = objGuia.Destino.Nombre
                cbDestino.SelectedValue = objGuia.Destino.Codigo
                txtDistancia.Text = objGuia.Destino.DistanciaVirtual
                cbVehiculo.Text = objGuia.Vehiculo.Placa
                cbVehiculo.SelectedValue = objGuia.Vehiculo.Codigo
                cbCarreta.Text = objGuia.Carreta.Placa
                cbCarreta.SelectedValue = objGuia.Carreta.Codigo
                cbChofer.Text = objGuia.Chofer.Nombre
                cbChofer.SelectedValue = objGuia.Chofer.Codigo
                cbSubcontratada.Text = objGuia.Empresa.Nombre
                cbSubcontratada.SelectedValue = objGuia.Empresa.Codigo
                cbEstado.Text = objGuia.Estado.Descripcion
                cbEstado.SelectedValue = objGuia.Estado.Codigo
                lblBultosTotales.Text = objGuia.BultosTotales
                lblPesoTotal.Text = FormatNumber(objGuia.PesoTotal, 2)
                lblCostoTotal.Text = FormatNumber(objGuia.CostoTotal, 2)
                lblImpreso.Visible = objGuia.Impreso
                cbDVTipo.Text = objGuia.TipoDocumentoVenta.Descripcion
                cbDVTipo.SelectedValue = objGuia.TipoDocumentoVenta.Codigo
                txtDocVenta.Text = objGuia.DocumentoVenta
                txtObservaciones.Text = objGuia.Observacion
                txtUsuario.Text = objGuia.Usuario.NombreCompleto
                chkManifiesto.Checked = IIf(objGuia.TieneManifiesto = 0, True, False)
                txtManifiesto.Text = objGuia.NroManifiesto

                dgvDetalle.Rows.Clear()
                Dim i As Integer
                For i = 0 To objGuia.Detalles.Count - 1
                    dgvDetalle.Rows.Add()
                    dgvDetalle.Rows(i).Cells(0).Value = IIf(objGuia.Detalles(i).TipoDocumento = "", Nothing, objGuia.Detalles(i).TipoDocumento)
                    dgvDetalle.Rows(i).Cells(1).Value = IIf(objGuia.Detalles(i).NumeroDocumento = "", Nothing, objGuia.Detalles(i).NumeroDocumento)
                    dgvDetalle.Rows(i).Cells(2).Value = IIf(objGuia.Detalles(i).Cantidad = 0, Nothing, objGuia.Detalles(i).Cantidad)
                    dgvDetalle.Rows(i).Cells(3).Value = IIf(objGuia.Detalles(i).Descripcion = "", Nothing, objGuia.Detalles(i).Descripcion)
                    dgvDetalle.Rows(i).Cells(4).Value = IIf(objGuia.Detalles(i).Peso = 0, Nothing, objGuia.Detalles(i).Peso)
                    dgvDetalle.Rows(i).Cells(5).Value = IIf(objGuia.Detalles(i).Unidad = "", Nothing, objGuia.Detalles(i).Unidad)
                    dgvDetalle.Rows(i).Cells(6).Value = IIf(objGuia.Detalles(i).Costo = 0, Nothing, objGuia.Detalles(i).Costo)
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

    Private Sub cbCarreta_DropDown(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbCarreta.DropDown

        Try
            LLenarCombo("carr_codigo", "carr_placa", "Carreta", cbCarreta, True)

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

    Private Sub btnEditar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEditar.Click

        Try
            'Activar controles
            DesactivarControles(TabControl1, 1, False)
            txtCodigo.ReadOnly = True
            txtUsuario.ReadOnly = True
            txtSerie.ReadOnly = True
            txtNumero.ReadOnly = True

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

    Private Sub btnDocVenta_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDocVenta.Click

        Try
            If dgvListado.SelectedRows.Count = 0 Or dgvListado.Rows.Count < 2 Then
                Exit Sub
            End If

            If dgvListado.SelectedRows(0).Cells(6).Value <> "" Then
                If MessageBox.Show("Esta guía ya tiene un documento de venta." + vbCrLf + "¿Continuar con la creación del documento de venta?", "Advertencia", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) = Windows.Forms.DialogResult.No Then
                    Exit Sub
                End If
            End If

            bolGenerandoComprobante = True
            Dim strCodigo As String
            strCodigo = dgvListado.SelectedRows(0).Cells(0).Value

            objGuia.Codigo = strCodigo
            If objGuia.BuscarXCodigo() Then

                If frmComprobante.Visible = True Then
                    frmComprobante.Activate()
                Else
                    frmComprobante.MdiParent = frmPrincipal
                    frmComprobante.Show()
                End If

                frmComprobante.btnNuevo.PerformClick()
                frmComprobante.txtDestiNroID.Text = objGuia.Destinatario.NumeroID
                frmComprobante.txtDestinatario.Text = objGuia.Destinatario.Nombre
                frmComprobante.cbDireccionDestino.Text = objGuia.DireccionDestino
                frmComprobante.cbDVTipo.Text = objGuia.TipoDocumentoVenta.Descripcion
                frmComprobante.cbDVTipo.SelectedValue = objGuia.TipoDocumentoVenta.Codigo
                frmComprobante.cbEstado.Text = objGuia.Estado.Descripcion
                frmComprobante.cbEstado.SelectedValue = objGuia.Estado.Codigo
                objComprobante.Remitente = objGuia.Remitente
                frmComprobante.txtRemiNroID.Text = objGuia.Remitente.NumeroID
                frmComprobante.txtRemitente.Text = objGuia.Remitente.Nombre
                frmComprobante.cbSubcontratada.Text = objGuia.Empresa.Nombre
                frmComprobante.cbSubcontratada.SelectedValue = objGuia.Empresa.Codigo
                frmComprobante.txtRUC.Text = objGuia.Empresa.RUC
                frmComprobante.cbDestino.Text = objGuia.Destino.Nombre
                frmComprobante.cbDestino.SelectedValue = objGuia.Destino.Codigo
                frmComprobante.txtGRT.Text = objGuia.Serie + "-" + objGuia.Numero
                frmComprobante.txtManifiesto.Text = objGuia.NroManifiesto

                Select Case objGuia.TipoDocumentoVenta.Codigo
                    Case 1
                        frmComprobante.dgvDetalle.Rows.Clear()
                        Dim dblValorVenta As Double = 0.0
                        Dim i As Integer
                        For i = 0 To objGuia.Detalles.Count - 1
                            If objGuia.Detalles(i).Cantidad <> 0 Then
                                frmComprobante.dgvDetalle.Rows.Add()
                                frmComprobante.dgvDetalle.Rows(i).Cells(0).Value = objGuia.Detalles(i).Cantidad
                                frmComprobante.dgvDetalle.Rows(i).Cells(1).Value = objGuia.Detalles(i).Descripcion
                                frmComprobante.dgvDetalle.Rows(i).Cells(3).Value = objGuia.Detalles(i).Costo
                                dblValorVenta = dblValorVenta + objGuia.Detalles(i).Costo
                            End If
                        Next i

                        frmComprobante.txtValorVenta.Enabled = True
                        frmComprobante.txtIGV.Enabled = True
                        frmComprobante.txtValorVenta.Text = FormatNumber(dblValorVenta, 2)
                        frmComprobante.txtLetras.Text = NumerosALetras(objGuia.CostoTotal).ToUpper + "/100 SOLES"  'agregado para solucionar problema de convertir S/.2000 a letras (25/01/2011)
                        frmComprobante.txtTotal.Text = FormatNumber(objGuia.CostoTotal, 2)
                        frmComprobante.txtIGV.Text = FormatNumber(frmComprobante.txtTotal.Text - frmComprobante.txtValorVenta.Text, 2)

                    Case 2
                        frmComprobante.dgvDetalle.Rows.Clear()
                        Dim i As Integer
                        For i = 0 To objGuia.Detalles.Count - 1
                            If objGuia.Detalles(i).Cantidad <> 0 Then
                                frmComprobante.dgvDetalle.Rows.Add()
                                frmComprobante.dgvDetalle.Rows(i).Cells(0).Value = objGuia.Detalles(i).Cantidad
                                frmComprobante.dgvDetalle.Rows(i).Cells(1).Value = objGuia.Detalles(i).Descripcion
                                frmComprobante.dgvDetalle.Rows(i).Cells(3).Value = FormatNumber(objGuia.Detalles(i).Costo * (1 + objParametros.PorcentajeIGV / 100), 2)
                            End If
                        Next i

                        frmComprobante.txtValorVenta.Enabled = False
                        frmComprobante.txtIGV.Enabled = False
                        frmComprobante.txtValorVenta.Clear()
                        frmComprobante.txtIGV.Clear()
                        frmComprobante.txtTotal.Text = FormatNumber(objGuia.CostoTotal, 2)

                End Select

            Else
                Return
            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End Try

    End Sub

    Private Sub btnImprimir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnImprimir.Click

        Try
            If dgvListado.RowCount <= 1 Or dgvListado.SelectedRows.Count <= 0 Then
                Exit Sub
            End If

            Dim intCodigo As Integer
            intCodigo = dgvListado.SelectedRows(0).Cells(0).Value
            objGuia.Codigo = intCodigo

            If objGuia.BuscarXCodigo() Then
                PrevisualizarGuia()
            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End Try

    End Sub

    Private Sub PrevisualizarGuia()

        If frmImprimirGuia.Visible Then
            frmImprimirGuia.Close()
            frmImprimirGuia.MdiParent = frmPrincipal
            frmImprimirGuia.Show()
        Else
            frmImprimirGuia.MdiParent = frmPrincipal
            frmImprimirGuia.Show()
        End If


        Dim ctrl As Control
        Dim lbl As Label
        Dim NroRegistros As Integer = 0
        For i As Integer = 0 To objGuia.Detalles.Count - 1
            If objGuia.Detalles(i).Descripcion <> "" Then
                NroRegistros = NroRegistros + 1
            End If
        Next
        For Each ctrl In frmImprimirGuia.Controls
            If TypeOf ctrl Is Label Then
                lbl = CType(ctrl, Label)
                lbl.Text = ""
                For i As Integer = 1 To NroRegistros
                    If lbl.TabIndex = i Then
                        lbl.Text = IIf(objGuia.Detalles(i - 1).Cantidad = 0, "", objGuia.Detalles(i - 1).Cantidad)
                    End If
                    If lbl.TabIndex = i + 10 Then
                        lbl.Text = objGuia.Detalles(i - 1).Descripcion
                    End If
                    If lbl.TabIndex = i + 20 Then
                        lbl.Text = IIf(objGuia.Detalles(i - 1).Peso = 0, "", FormatNumber(objGuia.Detalles(i - 1).Peso, 2))
                    End If
                    If lbl.TabIndex = i + 30 Then
                        lbl.Text = objGuia.Detalles(i - 1).Unidad
                    End If
                    If lbl.TabIndex = i + 40 Then
                        lbl.Text = IIf(objGuia.Detalles(i - 1).Costo = 0, "", "S/ " + FormatNumber(objGuia.Detalles(i - 1).Costo, 2))
                    End If
                    lbl.BringToFront()
                Next
            End If
        Next

        frmImprimirGuia.lblDia1.Text = objGuia.FechaEmision.Day
        frmImprimirGuia.lblMes1.Text = objGuia.FechaEmision.Month
        frmImprimirGuia.lblAño1.Text = objGuia.FechaEmision.Year
        frmImprimirGuia.lblDia2.Text = objGuia.FechaTraslado.Day
        frmImprimirGuia.lblMes2.Text = objGuia.FechaTraslado.Month
        frmImprimirGuia.lblAño2.Text = objGuia.FechaTraslado.Year

        Dim Arreglo As New List(Of String)
        For Each texto As String In objGuia.DireccionPartida.Split("|")
            Arreglo.Add(texto)
        Next
        frmImprimirGuia.lblDireccionPartida.Text = Arreglo(0).ToString.Trim
        If Arreglo.Count >= 3 Then
            frmImprimirGuia.lblCiudadPartida.Text = Arreglo(1).ToString.Trim + " - " + Arreglo(2).ToString.Trim
        End If

        Arreglo = New List(Of String)
        For Each texto As String In objGuia.DireccionDestino.Split("|")
            Arreglo.Add(texto)
        Next
        frmImprimirGuia.lblDireccionDestino.Text = Arreglo(0).ToString.Trim
        If Arreglo.Count >= 3 Then
            frmImprimirGuia.lblCiudadDestino.Text = Arreglo(1).ToString.Trim + " - " + Arreglo(2).ToString.Trim
        End If


        frmImprimirGuia.lblRemitente.Text = objGuia.Remitente.Nombre

        Select Case objGuia.Remitente.TipoID.Codigo
            Case 1
                frmImprimirGuia.lblRemiRUC.Text = objGuia.Remitente.NumeroID
            Case 2
                frmImprimirGuia.lblRemiDNI.Text = objGuia.Remitente.NumeroID
            Case 3
                frmImprimirGuia.lblRemiDNI.Text = "S/D:" + objGuia.Remitente.NumeroID
        End Select

        frmImprimirGuia.lblDestinatario.Text = objGuia.Destinatario.Nombre

        Select Case objGuia.Destinatario.TipoID.Codigo
            Case 1
                frmImprimirGuia.lblDestiRUC.Text = objGuia.Destinatario.NumeroID
            Case 2
                frmImprimirGuia.lblDestiDNI.Text = objGuia.Destinatario.NumeroID
            Case 3
                frmImprimirGuia.lblDestiDNI.Text = "S/D:" + objGuia.Destinatario.NumeroID
        End Select

        frmImprimirGuia.lblDistancia.Text = objGuia.Destino.DistanciaVirtual.ToString + " KM."
        frmImprimirGuia.lblVehiculo.Text = objGuia.Vehiculo.Marca
        frmImprimirGuia.lblPlaca.Text = objGuia.Vehiculo.Placa + "/" + objGuia.Carreta.Placa
        frmImprimirGuia.lblConf.Text = objGuia.Vehiculo.ConfiguracionVehicular
        frmImprimirGuia.lblCertificado.Text = objGuia.Vehiculo.CertificadoInscripcion
        frmImprimirGuia.lblLicencia.Text = objGuia.Chofer.Licencia

        If objGuia.Empresa.Codigo <> 1 Then
            frmImprimirGuia.lblSubcontratada.Text = objGuia.Empresa.Nombre
            frmImprimirGuia.lblDireccion.Text = objGuia.Empresa.Direccion
            frmImprimirGuia.lblRUC.Text = objGuia.Empresa.RUC
        End If

        Dim var_costotal As String = "TOTAL: S/." + FormatNumber(objGuia.CostoTotal, 2)
        frmImprimirGuia.lblObservaciones.Text = If(objGuia.Observacion = "", var_costotal, var_costotal + " | " + objGuia.Observacion)

        Dim strGRR As String = ""
        For i As Integer = 0 To objGuia.Detalles.Count - 1
            If objGuia.Detalles(i).NumeroDocumento <> "" Then
                strGRR = strGRR + ", " + objGuia.Detalles(i).NumeroDocumento
            End If
        Next
        If strGRR.Length >= 1 Then
            strGRR = strGRR.Substring(2, strGRR.Length - 2)
            frmImprimirGuia.lblGRR.Text = IIf(objGuia.Detalles(0).TipoDocumento = "GUIA", "GR/R", objGuia.Detalles(0).TipoDocumento) + ": " + strGRR
        End If

        UbicarControles("guia", frmImprimirGuia)


        If frmImprimirGuia.lblDesc6.Text = "" Then
            frmImprimirGuia.lblGRR.Top = frmImprimirGuia.lblDesc6.Top
        End If
        If frmImprimirGuia.lblDesc5.Text = "" Then
            frmImprimirGuia.lblGRR.Top = frmImprimirGuia.lblDesc5.Top
        End If
        If frmImprimirGuia.lblDesc4.Text = "" Then
            frmImprimirGuia.lblGRR.Top = frmImprimirGuia.lblDesc4.Top
        End If
        If frmImprimirGuia.lblDesc3.Text = "" Then
            frmImprimirGuia.lblGRR.Top = frmImprimirGuia.lblDesc3.Top
        End If
        If frmImprimirGuia.lblDesc2.Text = "" Then
            frmImprimirGuia.lblGRR.Top = frmImprimirGuia.lblDesc2.Top
        End If

        frmImprimirGuia.lblGRR.Left = frmImprimirGuia.lblDesc1.Left

    End Sub

    Private Sub TabControl1_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TabControl1.SelectedIndexChanged

        If TabControl1.SelectedIndex = 1 Then
            btnImprimir.Enabled = False
            btnDocVenta.Enabled = False
            btnEliminar.Enabled = False
            txtNroDoc.Enabled = False
        Else
            btnImprimir.Enabled = True
            btnDocVenta.Enabled = True
            btnEliminar.Enabled = True
            txtNroDoc.Enabled = True
        End If

    End Sub

    Private Sub chkManifiesto_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkManifiesto.CheckedChanged

        If chkManifiesto.Checked = True Then
            gbVehiculo.Visible = True
            gbSubcontratada.Visible = True
        Else
            gbVehiculo.Visible = False
            gbSubcontratada.Visible = False
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
                    If MessageBox.Show("La guia " + seleccion.Cells(1).Value.ToString + " tiene asignado el documento de venta " + seleccion.Cells(6).Value.ToString + vbCrLf + "¿Confirma que desea eliminar esta guía?" + vbCrLf + " Si responde SI asegúrese de actualizar el número de GR/T del documento de venta.", "Advertencia", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) = Windows.Forms.DialogResult.Yes Then
                        If objGuia.Eliminar(Convert.ToInt32(seleccion.Cells(0).Value)) = False Then
                            MessageBox.Show("Error al eliminar la guia Nº " + seleccion.Cells(1).Value.ToString, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        End If
                    End If
                Else
                    If objGuia.Eliminar(Convert.ToInt32(seleccion.Cells(0).Value)) = False Then
                        MessageBox.Show("Error al eliminar la guia Nº " + seleccion.Cells(1).Value.ToString, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
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

    Private Sub txtNroDoc_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtNroDoc.KeyPress

        Try
            If e.KeyChar = ChrW(Keys.Enter) Then
                e.Handled = True
                txtNroDoc.Text = txtNroDoc.Text.PadLeft(6, "0")
                Dim lista As New List(Of clsGuiaRemision)
                lista = objGuia.BuscarXNroDocumento(txtNroDoc.Text)

                dgvListado.Rows.Clear()
                For i As Integer = 0 To lista.Count - 1
                    dgvListado.Rows.Add()
                    dgvListado.Rows(i).Cells(0).Value = lista(i).Codigo
                    dgvListado.Rows(i).Cells(1).Value = lista(i).Serie + "-" + lista(i).Numero
                    dgvListado.Rows(i).Cells(2).Value = lista(i).Remitente.Nombre
                    dgvListado.Rows(i).Cells(3).Value = lista(i).Destinatario.Nombre
                    dgvListado.Rows(i).Cells(4).Value = lista(i).FechaEmision
                    dgvListado.Rows(i).Cells(5).Value = lista(i).FechaTraslado
                    dgvListado.Rows(i).Cells(6).Value = lista(i).DocumentoVenta
                    dgvListado.Rows(i).Cells(7).Value = lista(i).CostoTotal
                    dgvListado.Rows(i).Cells(8).Value = IIf(lista(i).Enviado = 1, "SI", "NO")
                    dgvListado.Rows(i).Cells(9).Value = IIf(lista(i).Impreso = 1, "SI", "NO")
                Next
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