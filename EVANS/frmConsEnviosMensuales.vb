Imports System.Data.SqlClient
Public Class frmConsEnviosMensuales

    Private Sub btnBuscar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBuscar.Click

        Try
            Dim destinos As String = ""
            For Each fila As DataGridViewRow In dgvDestinos.Rows
                If fila.Cells(0).Value = True Then
                    destinos = destinos + "," + fila.Cells(1).Value.ToString
                End If
            Next

            Dim cmd As New SqlCommand
            cmd.Connection = objConexion2
            cmd.CommandType = CommandType.Text
            cmd.CommandText = "Select C.clie_Nombre as Cliente, count(G.clie_destinatario) as NroGuias, max(G.grem_fechaemision) as UltimoEnvio from GuiaRemision G inner join EVANS.dbo.Cliente C on C.clie_codigo = G.clie_destinatario where clie_destinatario in (select clie_destinatario from guiaremision where (G.grem_fechaemision between convert(datetime,@fecha1) and convert(datetime,@fecha2)) and G.dest_codigo in (" + destinos.Substring(1).ToString + ") and G.esta_codigo=1) group by C.clie_nombre order by C.clie_nombre asc"
            cmd.Parameters.Add("@fecha1", SqlDbType.DateTime).Value = dtpFecha1.Value
            cmd.Parameters.Add("@fecha2", SqlDbType.DateTime).Value = dtpFecha2.Value
            Dim dr As SqlDataReader
            If objConexion2.State = ConnectionState.Closed Then
                objConexion2.Open()
            End If
            dr = cmd.ExecuteReader

            dgvDetalles.Rows.Clear()
            Dim i As Integer = 0
            While dr.Read()
                dgvDetalles.Rows.Add()
                dgvDetalles.Rows(i).Cells(0).Value = NullToString(dr("Cliente"))
                dgvDetalles.Rows(i).Cells(1).Value = NullToString(dr("NroGuias"))
                dgvDetalles.Rows(i).Cells(2).Value = NullToString(dr("UltimoEnvio"))
                i = i + 1
            End While
            dr.Close()

            If dgvDetalles.Rows.Count > 0 Then
                btnExportar.Enabled = True
            Else
                btnExportar.Enabled = False
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

    Private Sub CargarDestinos()

        Try
            Dim cmd As New SqlCommand
            cmd.Connection = objConexion
            cmd.CommandType = CommandType.Text
            cmd.CommandText = "Select dest_codigo, dest_nombre from Destino where esta_codigo = 1"
            If objConexion.State = ConnectionState.Closed Then
                objConexion.Open()
            End If
            Dim dr As SqlDataReader
            dr = cmd.ExecuteReader

            dgvDestinos.Rows.Clear()
            Dim i As Integer = 0
            While dr.Read()
                dgvDestinos.Rows.Add()
                dgvDestinos.Rows(i).Cells(1).Value = NullToString(dr("dest_codigo"))
                dgvDestinos.Rows(i).Cells(2).Value = NullToString(dr("dest_nombre"))
                i = i + 1
            End While
            dr.Close()

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

    Private Sub frmConsEnviosMensuales_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing

        LimpiarRAM()

    End Sub

    Private Sub frmConsEnviosMensuales_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Try
            Me.Top = 0
            Me.Left = 0
            CargarDestinos()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try

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

    Private Sub dgvDestinos_CellContentClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgvDestinos.CellContentClick
        dgvDetalles.Rows.Clear()
    End Sub
End Class