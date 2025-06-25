Imports System.Data.SqlClient

Public Class frmExportar

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click

        'Dim Transaccion As SqlTransaction
        'Try
        '    If objConexion.State = ConnectionState.Closed Then
        '        objConexion.Open()
        '    End If
        '    Transaccion = objConexion.BeginTransaction
        '    Dim cmd As New SqlCommand
        '    cmd.CommandType = CommandType.Text
        '    cmd.Connection = objConexion
        '    cmd.Transaction = Transaccion
        '    cmd.CommandText = "Select clie_codigo, clie_nombre from Cliente"
        '    Dim dr As SqlDataReader
        '    dr = cmd.ExecuteReader()

        '    Dim cliente As String
        '    Dim codigo As Integer
        '    Dim direcciones As New List(Of String)
        '    Dim cmd1 As New SqlCommand
        '    cmd1.CommandType = CommandType.Text
        '    cmd1.Connection = objConexion2
        '    While dr.Read()
        '        codigo = NullToString(dr("clie_codigo"))
        '        cliente = NullToString(dr("clie_nombre"))
        '        cmd1.CommandText = "Select clidirecc from [2004].dbo.Clientes where clidescri = @cliente"
        '        cmd1.Parameters.Add("@cliente", SqlDbType.VarChar, 80).Value = cliente
        '        Dim dr1 As SqlDataReader
        '        If objConexion2.State = ConnectionState.Closed Then
        '            objConexion2.Open()
        '        End If
        '        dr1 = cmd1.ExecuteReader()
        '        direcciones.Clear()
        '        While dr1.Read()
        '            direcciones.Add(NullToString(dr1("clidirecc")))
        '        End While
        '        dr1.Close()

        '        cmd1.Parameters.Clear()
        '        cmd1.CommandText = "Insert into EVANS.dbo.DireccionCliente (clie_codigo, clie_direccion) values (@codigo, @direccion)"
        '        cmd1.Parameters.Add("@codigo", SqlDbType.Int).Value = codigo
        '        cmd1.Parameters.Add("@direccion", SqlDbType.VarChar, 100)
        '        For i As Integer = 0 To direcciones.Count - 1
        '            cmd1.Parameters("@direccion").Value = direcciones(i)
        '            cmd1.ExecuteNonQuery()
        '        Next
        '    End While
        '    dr.Close()
        '    Transaccion.Commit()

        '    MsgBox("Transaccion exitosa")

        'Catch ex As SqlException
        '    Transaccion.Rollback()
        '    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        '    Return
        'Catch ex As Exception
        '    Transaccion.Rollback()
        '    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        '    Return
        'Finally
        '    If objConexion.State = ConnectionState.Open Then
        '        objConexion.Close()
        '    End If
        '    If objConexion2.State = ConnectionState.Open Then
        '        objConexion2.Close()
        '    End If
        'End Try

    End Sub

    Private Sub frmExportar_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim rc As clsResizeableControl
        rc = New clsResizeableControl(Label1)
    End Sub

End Class