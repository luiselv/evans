Imports System.Data.SqlClient

Public Class clsDestino

    Private intCodigo As Integer
    Private strNombre As String
    Private dblDistancia As Double
    Private objEstado As New clsEstado

    Public Property Codigo() As Integer
        Get
            Return intCodigo
        End Get
        Set(ByVal value As Integer)
            intCodigo = value
        End Set
    End Property

    Public Property Nombre() As String
        Get
            Return strNombre
        End Get
        Set(ByVal value As String)
            strNombre = value
        End Set
    End Property

    Public Property DistanciaVirtual() As Double
        Get
            Return dblDistancia
        End Get
        Set(ByVal value As Double)
            dblDistancia = value
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
            cmdListar.CommandText = "Select dest_codigo, dest_nombre, dest_distanciavirtual from Destino order by dest_codigo ASC"
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
            cmdBuscar.CommandText = "Select dest_codigo, dest_nombre, dest_distanciavirtual, esta_descripcion from Destino inner join Estado on Estado.esta_codigo = Destino.esta_codigo where dest_codigo = @dest_codigo"
            cmdBuscar.Parameters.Add("@dest_codigo", SqlDbType.Int)
            cmdBuscar.Parameters("@dest_codigo").Value = Me.Codigo

            If objConexion.State = ConnectionState.Closed Then
                objConexion.Open()
            End If

            drBuscar = cmdBuscar.ExecuteReader()

            If drBuscar.Read Then
                Me.Nombre = NullToString(drBuscar("dest_nombre"))
                Me.DistanciaVirtual = NullToString(drBuscar("dest_distanciavirtual"))
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

    Public Function BuscarXNombre() As SqlDataReader

        Dim cmdBuscar As New SqlCommand
        Dim drResultados As SqlDataReader

        Try
            cmdBuscar.CommandType = CommandType.Text
            cmdBuscar.Connection = objConexion
            cmdBuscar.CommandText = "Select dest_codigo, dest_nombre, dest_distanciavirtual from Destino where dest_nombre like @dest_nombre + '%'"
            cmdBuscar.Parameters.Add("@dest_nombre", SqlDbType.NVarChar, 40)
            cmdBuscar.Parameters("@dest_nombre").Value = Me.Nombre

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
            If objConexion.State = ConnectionState.Closed Then
                objConexion.Open()
            End If
            Transaccion = objConexion.BeginTransaction
            Dim cmdGrabar As New SqlCommand
            cmdGrabar.CommandType = CommandType.Text
            cmdGrabar.Connection = objConexion
            cmdGrabar.Transaction = Transaccion
            cmdGrabar.CommandText = "Insert into Destino (dest_nombre, dest_distanciavirtual, esta_codigo) values (@dest_nombre, @dest_distanciavirtual, @esta_codigo)"
            cmdGrabar.Parameters.Add("@dest_nombre", SqlDbType.NVarChar, 40).Value = Me.Nombre
            cmdGrabar.Parameters.Add("@dest_distanciavirtual", SqlDbType.Float).Value = Me.DistanciaVirtual
            cmdGrabar.Parameters.Add("@esta_codigo", SqlDbType.Int).Value = Me.Estado.Codigo

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
            cmdActualizar.CommandText = "Update Destino set dest_nombre = @dest_nombre, dest_distanciavirtual = @dest_distanciavirtual, esta_codigo = @esta_codigo where dest_codigo = @dest_codigo"
            cmdActualizar.Parameters.Add("@dest_codigo", SqlDbType.Int).Value = Me.Codigo
            cmdActualizar.Parameters.Add("@dest_nombre", SqlDbType.NVarChar, 40).Value = Me.Nombre
            cmdActualizar.Parameters.Add("@dest_distanciavirtual", SqlDbType.Float).Value = Me.DistanciaVirtual
            cmdActualizar.Parameters.Add("@esta_codigo", SqlDbType.Int).Value = Me.Estado.Codigo

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
