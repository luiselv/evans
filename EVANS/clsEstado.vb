Imports System.Data.SqlClient

Public Class clsEstado

    Private intCodigo As Integer
    Private strDescripcion As String

    Public Property Codigo() As Integer
        Get
            Return intCodigo
        End Get
        Set(ByVal value As Integer)
            intCodigo = value
        End Set
    End Property

    Public Property Descripcion() As String
        Get
            Return strDescripcion
        End Get
        Set(ByVal value As String)
            strDescripcion = value
        End Set
    End Property


    Public Function Listar() As SqlDataReader

        Dim cmdListar As New SqlCommand
        Dim drListar As SqlDataReader

        Try
            cmdListar.CommandText = "Select esta_codigo, esta_descripcion from Estado order by esta_codigo ASC"
            cmdListar.CommandType = CommandType.Text
            cmdListar.Connection = objConexion

            If objConexion.State = ConnectionState.Closed Then
                objConexion.Open()
            End If

            drListar = cmdListar.ExecuteReader()
            Return drListar

        Catch ex As SqlException
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            drListar = Nothing
            Return drListar
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            drListar = Nothing
            Return drListar
        End Try

    End Function

    Public Function BuscarXCodigo() As Boolean

        Try
            Dim cmdBuscar As New SqlCommand
            Dim drBuscar As SqlDataReader
            cmdBuscar.CommandType = CommandType.Text
            cmdBuscar.Connection = objConexion
            cmdBuscar.CommandText = "Select esta_codigo, esta_descripcion from Estado where esta_codigo = @esta_codigo"
            cmdBuscar.Parameters.Add("@esta_codigo", SqlDbType.Int)
            cmdBuscar.Parameters("@esta_codigo").Value = Me.Codigo

            If objConexion.State = ConnectionState.Closed Then
                objConexion.Open()
            End If

            drBuscar = cmdBuscar.ExecuteReader()

            If drBuscar.Read Then
                Me.Descripcion = NullToString(drBuscar("esta_nombre"))
            End If
            drBuscar.Close()
            Return True

        Catch ex As SqlException
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return False
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return False
        Finally
            If objConexion.State = ConnectionState.Open Then
                objConexion.Close()
            End If
        End Try

    End Function

    Public Function BuscarXNombre() As SqlDataReader

        Dim cmdBuscar As New SqlCommand
        Dim drResultados As SqlDataReader

        Try
            cmdBuscar.CommandType = CommandType.Text
            cmdBuscar.Connection = objConexion
            cmdBuscar.CommandText = "Select esta_codigo, esta_descripcion from Estado where esta_descripcion like @esta_descripcion + '%'"
            cmdBuscar.Parameters.Add("@esta_descripcion", SqlDbType.NVarChar, 20)
            cmdBuscar.Parameters("@esta_descripcion").Value = Me.Descripcion

            If objConexion.State = ConnectionState.Closed Then
                objConexion.Open()
            End If

            drResultados = cmdBuscar.ExecuteReader()
            Return drResultados

        Catch ex As SqlException
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            drResultados = Nothing
            Return drResultados
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            drResultados = Nothing
            Return drResultados
        End Try

    End Function

    Public Function Grabar() As Boolean

        Dim Transaccion As SqlTransaction
        Try
            Dim cmdGrabar As New SqlCommand
            cmdGrabar.CommandType = CommandType.Text
            cmdGrabar.Connection = objConexion
            cmdGrabar.CommandText = "Insert into Estado (esta_descripcion) values (@esta_descripcion)"
            cmdGrabar.Parameters.Add("@esta_descripcion", SqlDbType.NVarChar, 20)

            cmdGrabar.Parameters("@esta_descripcion").Value = Me.Descripcion

            If objConexion.State = ConnectionState.Closed Then
                objConexion.Open()
            End If
            Transaccion = objConexion.BeginTransaction
            cmdGrabar.Transaction = Transaccion
            cmdGrabar.ExecuteNonQuery()
            Transaccion.Commit()
            Return True

        Catch ex As SqlException
            Transaccion.Rollback()
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return False
        Catch ex As Exception
            Transaccion.Rollback()
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return False
        Finally
            If objConexion.State = ConnectionState.Open Then
                objConexion.Close()
            End If
        End Try

    End Function

    Public Function Actualizar() As Boolean

        Dim Transaccion As SqlTransaction
        Try
            Dim cmdActualizar As New SqlCommand
            cmdActualizar.CommandType = CommandType.Text
            cmdActualizar.Connection = objConexion
            cmdActualizar.CommandText = "Update Estado set esta_descripcion = @esta_descripcion where esta_codigo = @esta_codigo"
            cmdActualizar.Parameters.Add("@esta_codigo", SqlDbType.Int)
            cmdActualizar.Parameters.Add("@esta_descripcion", SqlDbType.NVarChar, 20)

            cmdActualizar.Parameters("@esta_codigo").Value = Me.Codigo
            cmdActualizar.Parameters("@esta_descripcion").Value = Me.Descripcion

            If objConexion.State = ConnectionState.Closed Then
                objConexion.Open()
            End If
            Transaccion = objConexion.BeginTransaction
            cmdActualizar.Transaction = Transaccion
            cmdActualizar.ExecuteNonQuery()
            Transaccion.Commit()
            Return True

        Catch ex As SqlException
            Transaccion.Rollback()
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return False
        Catch ex As Exception
            Transaccion.Rollback()
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return False
        Finally
            If objConexion.State = ConnectionState.Open Then
                objConexion.Close()
            End If
        End Try

    End Function

End Class
