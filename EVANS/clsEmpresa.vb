Imports System.Data.SqlClient

Public Class clsEmpresa

    Private intCodigo As Integer
    Private strNombre As String
    Private strDireccion As String
    Private strTelefono As String
    Private strRUC As String
    Private bolPropiedad As Boolean
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

    Public Property Direccion() As String
        Get
            Return strDireccion
        End Get
        Set(ByVal value As String)
            strDireccion = value
        End Set
    End Property

    Public Property Telefono() As String
        Get
            Return strTelefono
        End Get
        Set(ByVal value As String)
            strTelefono = value
        End Set
    End Property

    Public Property RUC() As String
        Get
            Return strRUC
        End Get
        Set(ByVal value As String)
            strRUC = value
        End Set
    End Property

    Public Property Propiedad() As Boolean
        Get
            Return bolPropiedad
        End Get
        Set(ByVal value As Boolean)
            bolPropiedad = value
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
            cmdListar.CommandText = "Select empr_codigo, empr_nombre, empr_ruc, empr_propiedad from Empresa order by empr_codigo ASC"
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
            cmdBuscar.CommandText = "Select empr_nombre, empr_direccion, empr_telefono, empr_ruc, empr_propiedad, esta_descripcion from Empresa inner join Estado on Estado.esta_codigo = Empresa.esta_codigo where empr_codigo = @empr_codigo"
            cmdBuscar.Parameters.Add("@empr_codigo", SqlDbType.Int)
            cmdBuscar.Parameters("@empr_codigo").Value = Me.Codigo

            If objConexion.State = ConnectionState.Closed Then
                objConexion.Open()
            End If

            drBuscar = cmdBuscar.ExecuteReader()

            If drBuscar.Read Then
                Me.Nombre = NullToString(drBuscar("empr_nombre"))
                Me.Direccion = NullToString(drBuscar("empr_direccion"))
                Me.Telefono = NullToString(drBuscar("empr_telefono"))
                Me.RUC = NullToString(drBuscar("empr_ruc"))
                Me.Propiedad = NullToString(drBuscar("empr_propiedad"))
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
            cmdBuscar.CommandText = "Select empr_codigo, empr_nombre, empr_ruc, empr_propiedad from Empresa where empr_nombre like @empr_nombre + '%'"
            cmdBuscar.Parameters.Add("@empr_nombre", SqlDbType.NVarChar, 70)
            cmdBuscar.Parameters("@empr_nombre").Value = Me.Nombre

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
            cmdGrabar.CommandText = "Insert into Empresa (empr_nombre, empr_direccion, empr_telefono, empr_ruc, empr_propiedad, esta_codigo) values (@empr_nombre, @empr_direccion, @empr_telefono, @empr_ruc, @empr_propiedad, @esta_codigo)"
            cmdGrabar.Parameters.Add("@empr_nombre", SqlDbType.NVarChar, 70)
            cmdGrabar.Parameters.Add("@empr_direccion", SqlDbType.NVarChar, 70)
            cmdGrabar.Parameters.Add("@empr_telefono", SqlDbType.NVarChar, 15)
            cmdGrabar.Parameters.Add("@empr_ruc", SqlDbType.NVarChar, 11)
            cmdGrabar.Parameters.Add("@empr_propiedad", SqlDbType.Bit)
            cmdGrabar.Parameters.Add("@esta_codigo", SqlDbType.Int)

            cmdGrabar.Parameters("@empr_nombre").Value = Me.Nombre
            cmdGrabar.Parameters("@empr_direccion").Value = Me.Direccion
            cmdGrabar.Parameters("@empr_telefono").Value = Me.Telefono
            cmdGrabar.Parameters("@empr_ruc").Value = Me.RUC
            cmdGrabar.Parameters("@empr_propiedad").Value = Me.Propiedad
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
            cmdActualizar.CommandText = "Update Empresa set empr_nombre = @empr_nombre, empr_direccion = @empr_direccion, empr_telefono = @empr_telefono, empr_ruc = @empr_ruc, empr_propiedad = @empr_propiedad, esta_codigo = @esta_codigo where empr_codigo = @empr_codigo"
            cmdActualizar.Parameters.Add("@empr_codigo", SqlDbType.Int)
            cmdActualizar.Parameters.Add("@empr_nombre", SqlDbType.NVarChar, 70)
            cmdActualizar.Parameters.Add("@empr_direccion", SqlDbType.NVarChar, 70)
            cmdActualizar.Parameters.Add("@empr_telefono", SqlDbType.NVarChar, 15)
            cmdActualizar.Parameters.Add("@empr_ruc", SqlDbType.NVarChar, 11)
            cmdActualizar.Parameters.Add("@empr_propiedad", SqlDbType.Bit)
            cmdActualizar.Parameters.Add("@esta_codigo", SqlDbType.Int)

            cmdActualizar.Parameters("@empr_codigo").Value = Me.Codigo
            cmdActualizar.Parameters("@empr_nombre").Value = Me.Nombre
            cmdActualizar.Parameters("@empr_direccion").Value = Me.Direccion
            cmdActualizar.Parameters("@empr_telefono").Value = Me.Telefono
            cmdActualizar.Parameters("@empr_ruc").Value = Me.RUC
            cmdActualizar.Parameters("@empr_propiedad").Value = Me.Propiedad
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
