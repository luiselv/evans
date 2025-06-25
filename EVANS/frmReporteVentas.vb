Imports System.Data.SqlClient

Public Class frmReporteVentas

    Private Sub frmReporteVentas_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            Me.Top = 0
            Me.Left = 0

        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            If objConexion2.State = ConnectionState.Open Then
                objConexion2.Close()
            End If
        End Try
    End Sub

    Private Sub btnBuscar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBuscar.Click

        Try
            Dim cmd As New SqlCommand
            cmd.Connection = objConexion2
            cmd.CommandType = CommandType.Text
            cmd.Parameters.Add("@fecha1", SqlDbType.DateTime).Value = dtpFecha1.Value
            cmd.Parameters.Add("@fecha2", SqlDbType.DateTime).Value = dtpFecha2.Value
            cmd.Parameters.Add("@param1", SqlDbType.Int).Value = IIf(chkFacturas.Checked, 1, 0)
            cmd.Parameters.Add("@param2", SqlDbType.Int).Value = IIf(chkBoletas.Checked, 2, 0)

            If rbTodos.Checked Then
                'cmd.CommandText = "Select CO.comp_fecha, CO.tico_codigo, CO.comp_serie, CO.comp_numero, C.clie_nroidentificacion, C.clie_nombre, CO.comp_valorventa, CO.comp_igv, CO.comp_total from Comprobante CO inner join EVANS.dbo.TipoComprobante TC on TC.tico_codigo = CO.tico_codigo inner join EVANS.dbo.Cliente C on C.clie_codigo = CO.clie_destinatario where CO.tico_codigo = 1 and CO.comp_fecha >= @fecha1 and CO.comp_fecha <= @fecha2 order by CO.comp_fecha ASC, CO.comp_numero ASC;"
                cmd.CommandText = "Select CO.comp_fecha, CO.tico_codigo, CO.comp_serie, CO.comp_numero, C.clie_nroidentificacion, C.clie_nombre, CO.comp_valorventa, CO.comp_igv, CO.comp_total from Comprobante CO inner join EVANS.dbo.TipoComprobante TC on TC.tico_codigo = CO.tico_codigo inner join EVANS.dbo.Cliente C on C.clie_codigo = CO.clie_destinatario where CO.tico_codigo in (@param1, @param2)  and CO.comp_fecha >= @fecha1 and CO.comp_fecha <= @fecha2 order by CO.comp_fecha ASC, CO.comp_numero ASC;"
            End If
            If rbCliente.Checked Then
                cmd.Parameters.Add("@Cliente", SqlDbType.Int).Value = Convert.ToInt32(cbCliente.SelectedValue)
                'cmd.CommandText = "Select CO.comp_fecha, CO.tico_codigo, CO.comp_serie, CO.comp_numero, C.clie_nroidentificacion, C.clie_nombre, CO.comp_valorventa, CO.comp_igv, CO.comp_total from Comprobante CO inner join EVANS.dbo.TipoComprobante TC on TC.tico_codigo = CO.tico_codigo inner join EVANS.dbo.Cliente C on C.clie_codigo = CO.clie_destinatario where CO.tico_codigo = 1 and CO.comp_fecha >= @fecha1 and CO.comp_fecha <= @fecha2 and CO.clie_destinatario = @Cliente  order by CO.comp_fecha ASC, CO.comp_numero ASC;"
                cmd.CommandText = "Select CO.comp_fecha, CO.tico_codigo, CO.comp_serie, CO.comp_numero, C.clie_nroidentificacion, C.clie_nombre, CO.comp_valorventa, CO.comp_igv, CO.comp_total from Comprobante CO inner join EVANS.dbo.TipoComprobante TC on TC.tico_codigo = CO.tico_codigo inner join EVANS.dbo.Cliente C on C.clie_codigo = CO.clie_destinatario where CO.tico_codigo in (@param1, @param2) and CO.comp_fecha >= @fecha1 and CO.comp_fecha <= @fecha2 and CO.clie_destinatario = @Cliente  order by CO.comp_fecha ASC, CO.comp_numero ASC;"
            End If

            If objConexion2.State = ConnectionState.Closed Then
                objConexion2.Open()
            End If
            Dim dr As SqlDataReader
            dr = cmd.ExecuteReader

            dgvDetalles.Rows.Clear()
            Dim total6, total7, total8 As Double
            Dim i As Integer = 0
            While dr.Read()
                dgvDetalles.Rows.Add()
                dgvDetalles.Rows(i).Cells(0).Value = NullToString(dr(0))
                dgvDetalles.Rows(i).Cells(1).Value = NullToString(dr(1))
                dgvDetalles.Rows(i).Cells(2).Value = NullToString(dr(2))
                dgvDetalles.Rows(i).Cells(3).Value = NullToString(dr(3))
                dgvDetalles.Rows(i).Cells(4).Value = NullToString(dr(4))
                dgvDetalles.Rows(i).Cells(5).Value = NullToString(dr(5))
                dgvDetalles.Rows(i).Cells(6).Value = NullToString(dr(6))
                total6 = total6 + dgvDetalles.Rows(i).Cells(6).Value
                dgvDetalles.Rows(i).Cells(7).Value = NullToString(dr(7))
                total7 = total7 + dgvDetalles.Rows(i).Cells(7).Value
                dgvDetalles.Rows(i).Cells(8).Value = NullToString(dr(8))
                total8 = total8 + dgvDetalles.Rows(i).Cells(8).Value
                i = i + 1
            End While
            dr.Close()
            Dim fuente1 As New Font(dgvDetalles.Font, FontStyle.Bold)
            Dim fuente2 As New Font(dgvDetalles.Font, FontStyle.Bold)

            dgvDetalles.Rows.Add()
            dgvDetalles.Rows(i).Cells(5).Value = "TOTAL"
            dgvDetalles.Rows(i).Cells(5).Style.Font = fuente1
            dgvDetalles.Rows(i).Cells(5).Style.ForeColor = Color.Red
            dgvDetalles.Rows(i).Cells(6).Value = total6
            dgvDetalles.Rows(i).Cells(6).Style.Font = fuente2
            dgvDetalles.Rows(i).Cells(6).Style.ForeColor = Color.Green
            dgvDetalles.Rows(i).Cells(7).Value = total7
            dgvDetalles.Rows(i).Cells(7).Style.Font = fuente2
            dgvDetalles.Rows(i).Cells(7).Style.ForeColor = Color.Green
            dgvDetalles.Rows(i).Cells(8).Value = total8
            dgvDetalles.Rows(i).Cells(8).Style.Font = fuente2
            dgvDetalles.Rows(i).Cells(8).Style.ForeColor = Color.Green

            If dgvDetalles.Rows.Count > 0 Then
                btnExportar.Enabled = True
            Else
                btnExportar.Enabled = False
            End If


        Catch ex As SqlException
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            If objConexion2.State = ConnectionState.Open Then
                objConexion2.Close()
            End If
        End Try

    End Sub

    Private Sub rbCliente_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rbCliente.CheckedChanged

        If rbCliente.Checked Then
            btnExportar.Enabled = False
            dgvDetalles.Rows.Clear()
            cbCliente.Enabled = True
            LLenarCombo("clie_codigo", "clie_nombre", "Cliente", cbCliente, False)
        End If

    End Sub

    Private Sub rbTodos_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rbTodos.CheckedChanged

        If rbTodos.Checked Then
            btnExportar.Enabled = False
            dgvDetalles.Rows.Clear()
            cbCliente.Enabled = False
            cbCliente.DataSource = Nothing
            cbCliente.Items.Clear()
        End If

    End Sub

    Private Sub btnExportar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExportar.Click

        Try
            If dgvDetalles.Rows.Count < 1 Then
                MessageBox.Show("No hay registros para exportar." + vbCrLf + "Realice una búsqueda primero.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Exit Sub
            End If
            frmBarraProgreso.Show()
            frmBarraProgreso.lblAccion.Text = "Exportando a Microsoft Excel..."

            BackgroundWorker1.RunWorkerAsync()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End Try

    End Sub

    Private Sub BackgroundWorker1_DoWork(ByVal sender As System.Object, ByVal e As System.ComponentModel.DoWorkEventArgs) Handles BackgroundWorker1.DoWork

        Try
            If ExportarAExcel(dgvDetalles) = True Then
                e.Result = 100
                BackgroundWorker1.ReportProgress(100)
            End If

        Catch ex As Exception
            frmBarraProgreso.Close()
            BackgroundWorker1.CancelAsync()
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try

    End Sub

    Private Sub BackgroundWorker1_RunWorkerCompleted(ByVal sender As Object, ByVal e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles BackgroundWorker1.RunWorkerCompleted

        frmBarraProgreso.Close()
        If e.Result = 100 Then
            MessageBox.Show("Registros exportados correctamente.", "Confirmación", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If

    End Sub

    Private Sub cbMes_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        dgvDetalles.Rows.Clear()
    End Sub
End Class