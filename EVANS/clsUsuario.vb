Imports System.Data.SqlClient

Public Class clsUsuario

    Private intCodigo As Integer
    Private strNombreUsuario As String
    Private strClave As String
    Private strNombreCompleto As String
    Private intAdmin As Integer
    Private objEstado As New clsEstado

    Public Property Codigo() As Integer
        Get
            Return intCodigo
        End Get
        Set(ByVal value As Integer)
            intCodigo = value
        End Set
    End Property

    Public Property NombreUsuario() As String
        Get
            Return strNombreUsuario
        End Get
        Set(ByVal value As String)
            strNombreUsuario = value
        End Set
    End Property

    Public Property Clave() As String
        Get
            Return strClave
        End Get
        Set(ByVal value As String)
            strClave = value
        End Set
    End Property

    Public Property NombreCompleto() As String
        Get
            Return strNombreCompleto
        End Get
        Set(ByVal value As String)
            strNombreCompleto = value
        End Set
    End Property

    Public Property Administrador() As Integer
        Get
            Return intAdmin
        End Get
        Set(ByVal value As Integer)
            intAdmin = value
        End Set
    End Property

    Public Property Estado() As clsEstado
        Get
            Return objEstado
        End Get
        Set(ByVal value As clsEstado)
            objEstado = value
        End Set
    End Property


    Public Function Listar() As List(Of clsUsuario)

        Dim cmdListar As New SqlCommand
        Dim drListar As SqlDataReader
        Dim lstLista As New List(Of clsUsuario)

        Try
            cmdListar.CommandText = "Select usu_codigo, usu_nombrecompleto, usu_nombreusuario from Usuario"
            cmdListar.CommandType = CommandType.Text
            cmdListar.Connection = objConexion

            If objConexion.State = ConnectionState.Closed Then
                objConexion.Open()
            End If

            drListar = cmdListar.ExecuteReader()
            While drListar.Read()
                Dim objUsuario As New clsUsuario
                objUsuario.Codigo = NullToString(drListar(0))
                objUsuario.NombreCompleto = NullToString(drListar(1))
                objUsuario.NombreUsuario = NullToString(drListar(2))
                lstLista.Add(objUsuario)
            End While
            drListar.Close()
            Return lstLista

        Catch ex As SqlException
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            lstLista = Nothing
            Return lstLista
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            lstLista = Nothing
            Return lstLista
        Finally
            drListar.Close()
            If objConexion.State = ConnectionState.Open Then
                objConexion.Close()
            End If
        End Try

    End Function

    Public Function BuscarXCodigo() As Boolean

        Try
            Dim cmdBuscar As New SqlCommand
            Dim drBuscar As SqlDataReader
            cmdBuscar.CommandType = CommandType.Text
            cmdBuscar.Connection = objConexion
            cmdBuscar.CommandText = "Select U.usu_codigo, U.usu_nombreusuario, U.usu_clave, U.usu_nombrecompleto, U.usu_admin, U.esta_codigo, E.esta_descripcion from Usuario U inner join Estado E on E.esta_codigo = U.esta_codigo where U.usu_codigo = @usu_codigo"
            cmdBuscar.Parameters.Add("@usu_codigo", SqlDbType.Int)
            cmdBuscar.Parameters("@usu_codigo").Value = Me.Codigo

            If objConexion.State = ConnectionState.Closed Then
                objConexion.Open()
            End If

            drBuscar = cmdBuscar.ExecuteReader()

            While drBuscar.Read()
                Me.Codigo = NullToString(drBuscar(0))
                Me.NombreUsuario = NullToString(drBuscar(1))
                Me.Clave = NullToString(drBuscar(2))
                Me.NombreCompleto = NullToString(drBuscar(3))
                Me.Administrador = NullToString(drBuscar(4))
                Me.Estado.Codigo = NullToString(drBuscar(5))
                Me.Estado.Descripcion = NullToString(drBuscar(6))
            End While
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
            cmdBuscar.CommandText = "Select U.usu_codigo, U.usu_nombreusuario, U.usu_clave, U.usu_nombrecompleto, U.usu_admin, U.esta_codigo, E.esta_descripcion from Usuario U inner join Estado E on E.esta_codigo = U.esta_codigo where U.usu_nombrecompleto like @usu_nombrecompleto + '%'"
            cmdBuscar.Parameters.Add("@usu_nombrecompleto", SqlDbType.NVarChar, 70).Value = Me.NombreCompleto

            If objConexion.State = ConnectionState.Closed Then
                objConexion.Open()
            End If

            drResultados = cmdBuscar.ExecuteReader()
            Return drResultados

        Catch ex As SqlException
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return Nothing
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return Nothing
        End Try

    End Function

    Public Function Grabar() As Boolean

        Dim Transaccion As SqlTransaction
        Try
            Dim cmd As New SqlCommand
            cmd.CommandType = CommandType.Text
            cmd.Connection = objConexion
            cmd.CommandText = "Insert into Usuario (usu_nombreusuario, usu_clave, usu_nombrecompleto, usu_admin, esta_codigo) values (@usu_nombreusuario, @usu_clave, @usu_nombrecompleto, @usu_admin, @esta_codigo)"
            cmd.Parameters.Add("@usu_nombreusuario", SqlDbType.NVarChar, 50).Value = Me.NombreUsuario
            cmd.Parameters.Add("@usu_clave", SqlDbType.NVarChar, 30).Value = Me.Clave
            cmd.Parameters.Add("@usu_nombrecompleto", SqlDbType.NVarChar, 70).Value = Me.NombreCompleto
            cmd.Parameters.Add("@usu_admin", SqlDbType.Int).Value = Me.Administrador
            cmd.Parameters.Add("@esta_codigo", SqlDbType.Int).Value = Me.Estado.Codigo

            If objConexion.State = ConnectionState.Closed Then
                objConexion.Open()
            End If
            Transaccion = objConexion.BeginTransaction
            cmd.Transaction = Transaccion
            cmd.ExecuteNonQuery()
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
            Dim cmd As New SqlCommand
            cmd.CommandType = CommandType.Text
            cmd.Connection = objConexion
            cmd.CommandText = "Update Usuario set usu_nombreusuario = @usu_nombreusuario, usu_clave = @usu_clave, usu_nombrecompleto = @usu_nombrecompleto, usu_admin = @usu_admin, esta_codigo = @esta_codigo where usu_codigo = @usu_codigo"
            cmd.Parameters.Add("@usu_codigo", SqlDbType.Int).Value = Me.Codigo
            cmd.Parameters.Add("@usu_nombreusuario", SqlDbType.NVarChar, 50).Value = Me.NombreUsuario
            cmd.Parameters.Add("@usu_clave", SqlDbType.NVarChar, 30).Value = Me.Clave
            cmd.Parameters.Add("@usu_nombrecompleto", SqlDbType.NVarChar, 70).Value = Me.NombreCompleto
            cmd.Parameters.Add("@usu_admin", SqlDbType.Int).Value = Me.Administrador
            cmd.Parameters.Add("@esta_codigo", SqlDbType.Int).Value = Me.Estado.Codigo

            If objConexion.State = ConnectionState.Closed Then
                objConexion.Open()
            End If
            Transaccion = objConexion.BeginTransaction
            cmd.Transaction = Transaccion
            cmd.ExecuteNonQuery()
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
