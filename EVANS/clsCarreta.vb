Imports System.Data.SqlClient

Public Class clsCarreta
    Private intCodigo As Integer
    Private strPlaca As String
    Private strMarca As String
    Private strCertificado As String
    Private objEmpresa As New clsEmpresa
    Private objEstado As New clsEstado

    Public Property Codigo() As Integer
        Get
            Return intCodigo
        End Get
        Set(ByVal value As Integer)
            intCodigo = value
        End Set
    End Property

    Public Property Placa() As String
        Get
            Return strPlaca
        End Get
        Set(ByVal value As String)
            strPlaca = value
        End Set
    End Property

    Public Property Marca() As String
        Get
            Return strMarca
        End Get
        Set(ByVal value As String)
            strMarca = value
        End Set
    End Property

    Public Property CertificadoInscripcion() As String
        Get
            Return strCertificado
        End Get
        Set(ByVal value As String)
            strCertificado = value
        End Set
    End Property

    Public Property Empresa() As clsEmpresa
        Get
            Return objEmpresa
        End Get
        Set(ByVal value As clsEmpresa)
            objEmpresa = value
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


    Public Function Listar() As List(Of clsCarreta)

        Dim cmdListar As New SqlCommand
        Dim drListar As SqlDataReader
        Dim lstLista As New List(Of clsCarreta)

        Try
            cmdListar.CommandText = "Select CA.carr_codigo, CA.carr_placa, CA.carr_marca, CA.empr_codigo, EM.empr_nombre, CA.esta_codigo, ES.esta_descripcion from Carreta CA inner join Empresa EM on CA.empr_codigo = EM.empr_codigo inner join Estado ES on CA.esta_codigo = ES.esta_codigo;"
            cmdListar.CommandType = CommandType.Text
            cmdListar.Connection = objConexion

            If objConexion.State = ConnectionState.Closed Then
                objConexion.Open()
            End If

            drListar = cmdListar.ExecuteReader()
            While drListar.Read()
                Dim objCarreta As New clsCarreta
                objCarreta.Codigo = NullToString(drListar(0))
                objCarreta.Placa = NullToString(drListar(1))
                objCarreta.Marca = NullToString(drListar(2))
                objCarreta.Empresa.Codigo = NullToString(drListar(3))
                objCarreta.Empresa.Nombre = NullToString(drListar(4))
                objCarreta.Estado.Codigo = NullToString(drListar(5))
                objCarreta.Estado.Descripcion = NullToString(drListar(6))
                lstLista.Add(objCarreta)
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
            cmdBuscar.CommandText = "Select CA.carr_codigo, CA.carr_placa, CA.carr_marca, CA.carr_certificado, CA.empr_codigo, EM.empr_nombre, CA.esta_codigo, E.esta_descripcion from Carreta CA inner join Empresa EM on EM.empr_codigo = CA.empr_codigo inner join Estado E on E.esta_codigo = CA.esta_codigo where CA.carr_codigo = @carr_codigo"
            cmdBuscar.Parameters.Add("@carr_codigo", SqlDbType.Int)
            cmdBuscar.Parameters("@carr_codigo").Value = Me.Codigo

            If objConexion.State = ConnectionState.Closed Then
                objConexion.Open()
            End If

            drBuscar = cmdBuscar.ExecuteReader()

            If drBuscar.Read Then
                Me.Placa = NullToString(drBuscar(1))
                Me.Marca = NullToString(drBuscar(2))
                Me.CertificadoInscripcion = NullToString(drBuscar(3))
                Me.Empresa.Codigo = NullToString(drBuscar(4))
                Me.Empresa.Nombre = NullToString(drBuscar(5))
                Me.Estado.Codigo = NullToString(drBuscar(6))
                Me.Estado.Descripcion = NullToString(drBuscar(7))
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

    Public Function BuscarXPlaca() As SqlDataReader

        Dim cmdBuscar As New SqlCommand
        Dim drBuscar As SqlDataReader
        Try
            cmdBuscar.CommandType = CommandType.Text
            cmdBuscar.Connection = objConexion
            cmdBuscar.CommandText = "Select CA.carr_codigo, CA.carr_marca, CA.carr_placa, EM.empr_nombre from Carreta CA inner join Empresa EM on EM.empr_codigo = CA.empr_codigo where CA.carr_placa like @carr_placa + '%'"
            cmdBuscar.Parameters.Add("@carr_placa", SqlDbType.NVarChar, 10)
            cmdBuscar.Parameters("@carr_placa").Value = Me.Placa

            If objConexion.State = ConnectionState.Closed Then
                objConexion.Open()
            End If

            drBuscar = cmdBuscar.ExecuteReader()
            Return drBuscar

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
            cmd.CommandText = "Insert into Carreta (carr_placa, carr_marca, carr_certificado, empr_codigo, esta_codigo) values (@carr_placa, @carr_marca, @carr_certificado, @empr_codigo, @esta_codigo)"
            cmd.Parameters.Add("@carr_placa", SqlDbType.NVarChar, 10).Value = Me.Placa
            cmd.Parameters.Add("@carr_marca", SqlDbType.NVarChar, 50).Value = Me.Marca
            cmd.Parameters.Add("@carr_certificado", SqlDbType.NVarChar, 15).Value = Me.strCertificado
            cmd.Parameters.Add("@empr_codigo", SqlDbType.Int).Value = Me.Empresa.Codigo
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
            cmd.CommandText = "Update Carreta set carr_placa = @carr_placa, carr_marca = @carr_marca, carr_certificado = @carr_certificado, empr_codigo = @empr_codigo, esta_codigo = @esta_codigo where carr_codigo = @carr_codigo"
            cmd.Parameters.Add("@carr_codigo", SqlDbType.Int).Value = Me.Codigo
            cmd.Parameters.Add("@carr_placa", SqlDbType.NVarChar, 10).Value = Me.Placa
            cmd.Parameters.Add("@carr_marca", SqlDbType.NVarChar, 50).Value = Me.Marca
            cmd.Parameters.Add("@carr_certificado", SqlDbType.NVarChar, 15).Value = Me.strCertificado
            cmd.Parameters.Add("@empr_codigo", SqlDbType.Int).Value = Me.Empresa.Codigo
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
