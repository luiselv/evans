Imports System.Data.SqlClient

Public Class frmConsGuiasPorCliente

    Dim objCliente As New clsCliente

    Private Sub frmConsGuiasPorCliente_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        LimpiarRAM()
    End Sub

    Private Sub frmConsGuiasPorCliente_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Try
            Me.Top = 0
            Me.Left = 0

            LLenarCombo("clie_codigo", "clie_nombre", "Cliente", cbCliente, False)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End Try

    End Sub

    Private Sub txtNroID_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtNroID.KeyPress

        Try
            If e.KeyChar = ChrW(Keys.Enter) Then
                e.Handled = True
                cbCliente.Text = ""
                objCliente = objCliente.BuscarXID(txtNroID.Text)
                cbCliente.Text = objCliente.Nombre
            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try

    End Sub

    Private Sub cbCliente_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbCliente.SelectedIndexChanged

        Try
            txtNroID.Clear()
            objCliente = objCliente.BuscarClienteXNombre(cbCliente.Text)
            txtNroID.Text = objCliente.NumeroID

        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            If objConexion.State = ConnectionState.Open Then
                objConexion.Close()
            End If
        End Try

    End Sub

    Private Sub btnBuscar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBuscar.Click

        Try
            Dim strSQL As String = ""
            If chkPendientes.Checked = True Then
                strSQL = "Select G.grem_codigo, (G.grem_serie + '-' + G.grem_numero) as NroDoc, R.clie_nombre as Remitente, D.clie_nombre as Destinatario, G.grem_fechaemision, G.grem_fechatraslado, G.grem_bultos, G.grem_costototal, G.grem_enviado from GuiaRemision G inner join EVANS.dbo.Cliente R on R.clie_codigo = G.clie_remitente inner join EVANS.dbo.Cliente D on D.clie_codigo = G.clie_destinatario where (G.clie_remitente = @cliente or G.clie_destinatario = @cliente) and ((G.grem_fechaemision between convert(datetime, @fecha1) and convert(datetime, @fecha2)) or (G.grem_fechatraslado between convert(datetime, @fecha1) and convert(datetime, @fecha2))) and G.grem_enviado = 0"
            Else
                strSQL = "Select G.grem_codigo, (G.grem_serie + '-' + G.grem_numero) as NroDoc, R.clie_nombre as Remitente, D.clie_nombre as Destinatario, G.grem_fechaemision, G.grem_fechatraslado, G.grem_bultos, G.grem_costototal, G.grem_enviado from GuiaRemision G inner join EVANS.dbo.Cliente R on R.clie_codigo = G.clie_remitente inner join EVANS.dbo.Cliente D on D.clie_codigo = G.clie_destinatario where (G.clie_remitente = @cliente or G.clie_destinatario = @cliente) and ((G.grem_fechaemision between convert(datetime, @fecha1) and convert(datetime, @fecha2)) or (G.grem_fechatraslado between convert(datetime, @fecha1) and convert(datetime, @fecha2)))"
            End If

            Dim cmd As New SqlCommand
            cmd.Connection = objConexion2
            cmd.CommandType = CommandType.Text
            cmd.CommandText = strSQL
            cmd.Parameters.Add("@cliente", SqlDbType.Int).Value = cbCliente.SelectedValue
            cmd.Parameters.Add("@fecha1", SqlDbType.DateTime).Value = dtpFecha1.Value
            cmd.Parameters.Add("@fecha2", SqlDbType.DateTime).Value = dtpFecha2.Value
            Dim dr As SqlDataReader
            If objConexion2.State = ConnectionState.Closed Then
                objConexion2.Open()
            End If
            dr = cmd.ExecuteReader()

            dgvListado.Rows.Clear()
            Dim i As Integer = 0
            While dr.Read()
                dgvListado.Rows.Add()
                dgvListado.Rows(i).Cells(0).Value = NullToString(dr(0))
                dgvListado.Rows(i).Cells(1).Value = NullToString(dr(1))
                dgvListado.Rows(i).Cells(2).Value = NullToString(dr(2))
                dgvListado.Rows(i).Cells(3).Value = NullToString(dr(3))
                dgvListado.Rows(i).Cells(4).Value = NullToString(dr(4))
                dgvListado.Rows(i).Cells(5).Value = NullToString(dr(5))
                dgvListado.Rows(i).Cells(6).Value = NullToString(dr(6))
                dgvListado.Rows(i).Cells(7).Value = FormatNumber(NullToString(dr(7)), 2)
                dgvListado.Rows(i).Cells(8).Value = IIf(NullToString(dr(8)) = 1, "SI", "NO")
                i = i + 1
            End While
            dr.Close()

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

    Private Sub dgvListado_DoubleClick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles dgvListado.DoubleClick

        If dgvListado.SelectedRows.Count = 0 Or dgvListado.Rows.Count < 2 Then
            Exit Sub
        End If

        MostrarGuia(dgvListado.SelectedRows(0).Cells(0).Value)

    End Sub

End Class