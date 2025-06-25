Imports System.Data.SqlClient

Public Class clsVehiculo

    Private intCodigo As Integer
    Private strMarca As String
    Private strPlaca As String
    Private strConfVehicular As String
    Private strCertInscripcion As String
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

    Public Property Marca() As String
        Get
            Return strMarca
        End Get
        Set(ByVal value As String)
            strMarca = value
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

    Public Property ConfiguracionVehicular() As String
        Get
            Return strConfVehicular
        End Get
        Set(ByVal value As String)
            strConfVehicular = value
        End Set
    End Property

    Public Property CertificadoInscripcion() As String
        Get
            Return strCertInscripcion
        End Get
        Set(ByVal value As String)
            strCertInscripcion = value
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


    Public Function Listar() As SqlDataReader

        Dim cmdListar As New SqlCommand
        Dim drListar As SqlDataReader

        Try
            cmdListar.CommandText = "Select vehi_codigo, vehi_marca, vehi_placa from Vehiculo order by vehi_codigo ASC"
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
            cmdBuscar.CommandText = "Select vehi_codigo, vehi_marca, vehi_placa, vehi_confvehicular, vehi_certinscripcion, empr_nombre, esta_descripcion from Vehiculo inner join Empresa on Empresa.empr_codigo = Vehiculo.empr_codigo inner join Estado on Estado.esta_codigo = Vehiculo.esta_codigo where vehi_codigo = @vehi_codigo"
            cmdBuscar.Parameters.Add("@vehi_codigo", SqlDbType.Int)
            cmdBuscar.Parameters("@vehi_codigo").Value = Me.Codigo

            If objConexion.State = ConnectionState.Closed Then
                objConexion.Open()
            End If

            drBuscar = cmdBuscar.ExecuteReader()

            If drBuscar.Read Then
                Me.Marca = NullToString(drBuscar("vehi_marca"))
                Me.Placa = NullToString(drBuscar("vehi_placa"))
                Me.ConfiguracionVehicular = NullToString(drBuscar("vehi_confvehicular"))
                Me.CertificadoInscripcion = NullToString(drBuscar("vehi_certinscripcion"))
                Me.Empresa.Nombre = NullToString(drBuscar("empr_nombre"))
                Me.Estado.Descripcion = NullToString(drBuscar("esta_descripcion"))
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
        Dim drResultados As SqlDataReader

        Try
            cmdBuscar.CommandType = CommandType.Text
            cmdBuscar.Connection = objConexion
            cmdBuscar.CommandText = "Select vehi_codigo, vehi_marca, vehi_placa from Vehiculo where vehi_placa like @vehi_placa + '%'"
            cmdBuscar.Parameters.Add("@vehi_placa", SqlDbType.NVarChar, 10)
            cmdBuscar.Parameters("@vehi_placa").Value = Me.Placa

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

    Public Function BuscarXMarca() As SqlDataReader

        Dim cmdBuscar As New SqlCommand
        Dim drResultados As SqlDataReader

        Try
            cmdBuscar.CommandType = CommandType.Text
            cmdBuscar.Connection = objConexion
            cmdBuscar.CommandText = "Select vehi_codigo, vehi_marca, vehi_placa from Vehiculo where vehi_marca like @vehi_marca + '%'"
            cmdBuscar.Parameters.Add("@vehi_marca", SqlDbType.NVarChar, 50)
            cmdBuscar.Parameters("@vehi_marca").Value = Me.Marca

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
            cmdGrabar.CommandText = "Insert into Vehiculo (vehi_marca, vehi_placa, vehi_confvehicular, vehi_certinscripcion, empr_codigo, esta_codigo) values (@vehi_marca, @vehi_placa, @vehi_confvehicular, @vehi_certinscripcion, @empr_codigo, @esta_codigo)"
            cmdGrabar.Parameters.Add("@vehi_marca", SqlDbType.NVarChar, 50)
            cmdGrabar.Parameters.Add("@vehi_placa", SqlDbType.NVarChar, 10)
            cmdGrabar.Parameters.Add("@vehi_confvehicular", SqlDbType.NVarChar, 5)
            cmdGrabar.Parameters.Add("@vehi_certinscripcion", SqlDbType.NVarChar, 15)
            cmdGrabar.Parameters.Add("@empr_codigo", SqlDbType.Int)
            cmdGrabar.Parameters.Add("@esta_codigo", SqlDbType.Int)

            cmdGrabar.Parameters("@vehi_marca").Value = Me.Marca
            cmdGrabar.Parameters("@vehi_placa").Value = Me.Placa
            cmdGrabar.Parameters("@vehi_confvehicular").Value = Me.ConfiguracionVehicular
            cmdGrabar.Parameters("@vehi_certinscripcion").Value = Me.CertificadoInscripcion
            cmdGrabar.Parameters("@empr_codigo").Value = Me.Empresa.Codigo
            cmdGrabar.Parameters("@esta_codigo").Value = Me.Estado.Codigo

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
            cmdActualizar.CommandText = "Update Vehiculo set vehi_marca = @vehi_marca, vehi_placa = @vehi_placa, vehi_confvehicular = @vehi_confvehicular, vehi_certinscripcion = @vehi_certinscripcion, empr_codigo = @empr_codigo, esta_codigo = @esta_codigo where vehi_codigo = @vehi_codigo"
            cmdActualizar.Parameters.Add("@vehi_codigo", SqlDbType.Int)
            cmdActualizar.Parameters.Add("@vehi_marca", SqlDbType.NVarChar, 50)
            cmdActualizar.Parameters.Add("@vehi_placa", SqlDbType.NVarChar, 10)
            cmdActualizar.Parameters.Add("@vehi_confvehicular", SqlDbType.NVarChar, 5)
            cmdActualizar.Parameters.Add("@vehi_certinscripcion", SqlDbType.NVarChar, 15)
            cmdActualizar.Parameters.Add("@empr_codigo", SqlDbType.Int)
            cmdActualizar.Parameters.Add("@esta_codigo", SqlDbType.Int)

            cmdActualizar.Parameters("@vehi_codigo").Value = Me.Codigo
            cmdActualizar.Parameters("@vehi_marca").Value = Me.Marca
            cmdActualizar.Parameters("@vehi_placa").Value = Me.Placa
            cmdActualizar.Parameters("@vehi_confvehicular").Value = Me.ConfiguracionVehicular
            cmdActualizar.Parameters("@vehi_certinscripcion").Value = Me.CertificadoInscripcion
            cmdActualizar.Parameters("@empr_codigo").Value = Me.Empresa.Codigo
            cmdActualizar.Parameters("@esta_codigo").Value = Me.Estado.Codigo

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
