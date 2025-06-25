Imports System.Data.SqlClient

Public Class clsTipoID
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
            cmdListar.CommandText = "Select iden_codigo, iden_descripcion from TipoIdentificacion order by iden_codigo ASC"
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
            cmdBuscar.CommandText = "Select iden_descripcion from TipoIdentificacion where iden_codigo = @iden_codigo"
            cmdBuscar.Parameters.Add("@iden_codigo", SqlDbType.Int)
            cmdBuscar.Parameters("@iden_codigo").Value = Me.Codigo

            If objConexion.State = ConnectionState.Closed Then
                objConexion.Open()
            End If

            drBuscar = cmdBuscar.ExecuteReader()

            If drBuscar.Read Then
                Me.Descripcion = nullToString(drBuscar("iden_descripcion"))
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
            cmdBuscar.CommandText = "Select iden_codigo, iden_descripcion from TipoIdentificacion where iden_descripcion like @iden_descripcion + '%'"
            cmdBuscar.Parameters.Add("@iden_descripcion", SqlDbType.NVarChar, 10)
            cmdBuscar.Parameters("@iden_descripcion").Value = Me.Descripcion

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
            cmdGrabar.CommandText = "Insert into TipoIdentificacion (iden_descripcion) values (@iden_descripcion)"
            cmdGrabar.Parameters.Add("@iden_descripcion", SqlDbType.NVarChar, 10)
            cmdGrabar.Parameters("@iden_descripcion").Value = Me.Descripcion

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
            cmdActualizar.CommandText = "Update TipoIdentificacion set iden_descripcion = @iden_descripcion where iden_codigo = @iden_codigo"
            cmdActualizar.Parameters.Add("@iden_codigo", SqlDbType.Int)
            cmdActualizar.Parameters.Add("@iden_descripcion", SqlDbType.NVarChar, 10)

            cmdActualizar.Parameters("@iden_codigo").Value = Me.Codigo
            cmdActualizar.Parameters("@iden_descripcion").Value = Me.Descripcion

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
