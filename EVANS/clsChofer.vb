Imports System.Data.SqlClient

Public Class clsChofer

    Private intCodigo As Integer
    Private strNombre As String
    Private strLicencia As String
    Private strTelefono As String
    Private strDireccion As String
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

    Public Property Nombre() As String
        Get
            Return strNombre
        End Get
        Set(ByVal value As String)
            strNombre = value
        End Set
    End Property

    Public Property Licencia() As String
        Get
            Return strLicencia
        End Get
        Set(ByVal value As String)
            strLicencia = value
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

    Public Property Direccion() As String
        Get
            Return strDireccion
        End Get
        Set(ByVal value As String)
            strDireccion = value
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


    Public Function Listar() As List(Of clsChofer)

        Dim cmdListar As New SqlCommand
        Dim drListar As SqlDataReader
        Dim lstLista As New List(Of clsChofer)

        Try
            cmdListar.CommandText = "Select chof_codigo, chof_nombre, chof_licencia, empr_nombre from Chofer inner join Empresa on Chofer.empr_codigo = Empresa.empr_codigo order by chof_codigo ASC"
            cmdListar.CommandType = CommandType.Text
            cmdListar.Connection = objConexion

            If objConexion.State = ConnectionState.Closed Then
                objConexion.Open()
            End If

            drListar = cmdListar.ExecuteReader()
            While drListar.Read()
                Dim objChofer As New clsChofer
                objChofer.Codigo = NullToString(drListar("chof_codigo"))
                objChofer.Nombre = NullToString(drListar("chof_nombre"))
                objChofer.Licencia = NullToString(drListar("chof_licencia"))
                objChofer.Empresa.Nombre = NullToString(drListar("empr_nombre"))
                lstLista.Add(objChofer)
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
        End Try

    End Function

    Public Function BuscarXCodigo() As Boolean

        Try
            Dim cmdBuscar As New SqlCommand
            Dim drBuscar As SqlDataReader
            cmdBuscar.CommandType = CommandType.Text
            cmdBuscar.Connection = objConexion
            cmdBuscar.CommandText = "Select chof_codigo, chof_nombre, chof_licencia, chof_telefono, chof_direccion, empr_nombre, esta_descripcion from Chofer inner join Empresa on Chofer.empr_codigo = Empresa.empr_codigo inner join Estado on Estado.esta_codigo = Chofer.esta_codigo where chof_codigo = @chof_codigo"
            cmdBuscar.Parameters.Add("@chof_codigo", SqlDbType.Int)
            cmdBuscar.Parameters("@chof_codigo").Value = Me.Codigo

            If objConexion.State = ConnectionState.Closed Then
                objConexion.Open()
            End If

            drBuscar = cmdBuscar.ExecuteReader()

            If drBuscar.Read Then
                Me.Nombre = NullToString(drBuscar("chof_nombre"))
                Me.Licencia = NullToString(drBuscar("chof_licencia"))
                Me.Telefono = NullToString(drBuscar("chof_telefono"))
                Me.Direccion = NullToString(drBuscar("chof_direccion"))
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

    Public Function BuscarXNombre() As SqlDataReader

        Dim cmdBuscar As New SqlCommand
        Dim drResultados As SqlDataReader

        Try
            cmdBuscar.CommandType = CommandType.Text
            cmdBuscar.Connection = objConexion
            cmdBuscar.CommandText = "Select chof_codigo, chof_nombre, chof_licencia, empr_nombre from Chofer inner join Empresa on Chofer.empr_codigo = Empresa.empr_codigo where chof_nombre like @chof_nombre + '%'"
            cmdBuscar.Parameters.Add("@chof_nombre", SqlDbType.NVarChar, 70)
            cmdBuscar.Parameters("@chof_nombre").Value = Me.Nombre

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
            cmdGrabar.CommandText = "Insert into Chofer (chof_nombre, chof_licencia, chof_telefono, chof_direccion, empr_codigo, esta_codigo) values (@chof_nombre, @chof_licencia, @chof_telefono, @chof_direccion, @empr_codigo, @esta_codigo)"
            cmdGrabar.Parameters.Add("@chof_nombre", SqlDbType.NVarChar, 70).Value = Me.Nombre
            cmdGrabar.Parameters.Add("@chof_licencia", SqlDbType.NVarChar, 15).Value = Me.Licencia
            cmdGrabar.Parameters.Add("@chof_telefono", SqlDbType.NVarChar, 15).Value = Me.Telefono
            cmdGrabar.Parameters.Add("@chof_direccion", SqlDbType.NVarChar, 70).Value = Me.Direccion
            cmdGrabar.Parameters.Add("@empr_codigo", SqlDbType.Int).Value = Me.Empresa.Codigo
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
            If objConexion.State = ConnectionState.Closed Then
                objConexion.Open()
            End If
            Transaccion = objConexion.BeginTransaction
            Dim cmdActualizar As New SqlCommand
            cmdActualizar.CommandType = CommandType.Text
            cmdActualizar.Connection = objConexion
            cmdActualizar.Transaction = Transaccion
            cmdActualizar.CommandText = "Update Chofer set chof_nombre = @chof_nombre, chof_licencia = @chof_licencia, chof_telefono = @chof_telefono, chof_direccion = @chof_direccion, empr_codigo = @empr_codigo, esta_codigo = @esta_codigo where chof_codigo = @chof_codigo"
            cmdActualizar.Parameters.Add("@chof_codigo", SqlDbType.Int).Value = Me.Codigo
            cmdActualizar.Parameters.Add("@chof_nombre", SqlDbType.NVarChar, 70).Value = Me.Nombre
            cmdActualizar.Parameters.Add("@chof_licencia", SqlDbType.NVarChar, 15).Value = Me.Licencia
            cmdActualizar.Parameters.Add("@chof_telefono", SqlDbType.NVarChar, 15).Value = Me.Telefono
            cmdActualizar.Parameters.Add("@chof_direccion", SqlDbType.NVarChar, 70).Value = Me.Direccion
            cmdActualizar.Parameters.Add("@empr_codigo", SqlDbType.Int).Value = Me.Empresa.Codigo
            cmdActualizar.Parameters.Add("@esta_codigo", SqlDbType.Int).Value = Me.Estado.Codigo

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
