Imports System.Data.SqlClient

Public Class clsCliente

    Private intCodigo As Integer
    Private strNombre As String
    Private objTipoID As New clsTipoID
    Private strNroID As String
    Private lstDireccion As New List(Of clsDireccion)
    Private strTelefono As String
    Private strFax As String
    Private strEmail As String
    Private strRepresentante As String


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

    Public Property TipoID() As clsTipoID
        Get
            Return objTipoID
        End Get
        Set(ByVal value As clsTipoID)
            objTipoID = value
        End Set
    End Property

    Public Property NumeroID() As String
        Get
            Return strNroID
        End Get
        Set(ByVal value As String)
            strNroID = value
        End Set
    End Property

    Public Property Direccion() As List(Of clsDireccion)
        Get
            Return lstDireccion
        End Get
        Set(ByVal value As List(Of clsDireccion))
            lstDireccion = value
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

    Public Property Fax() As String
        Get
            Return strFax
        End Get
        Set(ByVal value As String)
            strFax = value
        End Set
    End Property

    Public Property Email() As String
        Get
            Return strEmail
        End Get
        Set(ByVal value As String)
            strEmail = value
        End Set
    End Property

    Public Property Representante() As String
        Get
            Return strRepresentante
        End Get
        Set(ByVal value As String)
            strRepresentante = value
        End Set
    End Property


    Public Function Listar() As List(Of clsCliente)

        Dim cmdListar As New SqlCommand
        Dim drListar As SqlDataReader
        Dim lstLista As New List(Of clsCliente)

        Try
            cmdListar.CommandText = "Select C.clie_codigo, C.clie_nombre, T.iden_descripcion, C.clie_nroidentificacion from Cliente C inner join TipoIdentificacion T on T.iden_codigo = C.iden_codigo order by C.clie_nombre ASC"
            cmdListar.CommandType = CommandType.Text
            cmdListar.Connection = objConexion

            If objConexion.State = ConnectionState.Closed Then
                objConexion.Open()
            End If

            drListar = cmdListar.ExecuteReader()
            While drListar.Read()
                Dim objCliente As New clsCliente
                objCliente.Codigo = NullToString(drListar(0))
                objCliente.Nombre = NullToString(drListar(1))
                objCliente.TipoID.Descripcion = NullToString(drListar(2))
                objCliente.NumeroID = NullToString(drListar(3))
                lstLista.Add(objCliente)
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
            cmdBuscar.CommandText = "Select C.clie_nombre, T.iden_codigo, T.iden_descripcion, C.clie_nroidentificacion, C.clie_telefono, C.clie_fax, C.clie_email, C.clie_representante from Cliente C inner join TipoIdentificacion T on T.iden_codigo = C.iden_codigo where C.clie_codigo = @clie_codigo"
            cmdBuscar.Parameters.Add("@clie_codigo", SqlDbType.Int)
            cmdBuscar.Parameters("@clie_codigo").Value = Me.Codigo

            If objConexion.State = ConnectionState.Closed Then
                objConexion.Open()
            End If

            drBuscar = cmdBuscar.ExecuteReader()

            If drBuscar.Read Then
                Me.Nombre = NullToString(drBuscar("clie_nombre"))
                Me.TipoID.Codigo = NullToString(drBuscar("iden_codigo"))
                Me.TipoID.Descripcion = NullToString(drBuscar("iden_descripcion"))
                Me.NumeroID = NullToString(drBuscar("clie_nroidentificacion"))
                Me.Telefono = NullToString(drBuscar("clie_telefono"))
                Me.Fax = NullToString(drBuscar("clie_fax"))
                Me.Email = NullToString(drBuscar("clie_email"))
                Me.Representante = NullToString(drBuscar("clie_representante"))
            End If
            drBuscar.Close()

            cmdBuscar.CommandText = "Select clie_direccion, clie_ciudad, clie_provincia from DireccionCliente where clie_codigo = @clie_codigo"
            drBuscar = cmdBuscar.ExecuteReader()

            Me.Direccion.Clear()
            While drBuscar.Read
                Dim item As New clsDireccion
                item.Direccion = NullToString(drBuscar("clie_direccion"))
                item.Ciudad = NullToString(drBuscar("clie_ciudad"))
                item.Provincia = NullToString(drBuscar("clie_provincia"))
                Me.Direccion.Add(item)
                item = Nothing
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
            cmdBuscar.CommandText = "Select C.clie_codigo, C.clie_nombre, T.iden_descripcion, C.clie_nroidentificacion from Cliente C inner join TipoIdentificacion T on T.iden_codigo = C.iden_codigo where C.clie_nombre like @clie_nombre + '%' order by C.clie_nombre ASC"
            cmdBuscar.Parameters.Add("@clie_nombre", SqlDbType.NVarChar, 70)
            cmdBuscar.Parameters("@clie_nombre").Value = Me.Nombre

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

    Public Function BuscarClienteXNombre(ByVal nombre As String) As clsCliente

        Dim objCliente As New clsCliente
        Dim drBuscar As SqlDataReader
        Dim cmdBuscar As New SqlCommand

        Try
            cmdBuscar.CommandType = CommandType.Text
            cmdBuscar.Connection = objConexion
            cmdBuscar.CommandText = "Select top 1 * from Cliente where clie_nombre = @clie_nombre"
            cmdBuscar.Parameters.Add("@clie_nombre", SqlDbType.NVarChar, 70).Value = nombre
            If objConexion.State = ConnectionState.Closed Then
                objConexion.Open()
            End If
            drBuscar = cmdBuscar.ExecuteReader

            While drBuscar.Read()
                objCliente.Codigo = NullToString(drBuscar("clie_codigo"))
                objCliente.Nombre = NullToString(drBuscar("clie_nombre"))
                objCliente.TipoID.Codigo = NullToString(drBuscar("iden_codigo"))
                objCliente.NumeroID = NullToString(drBuscar("clie_nroidentificacion"))
                objCliente.Telefono = NullToString(drBuscar("clie_telefono"))
                objCliente.Fax = NullToString(drBuscar("clie_fax"))
                objCliente.Email = NullToString(drBuscar("clie_email"))
                objCliente.Representante = NullToString(drBuscar("clie_representante"))
            End While
            drBuscar.Close()

            cmdBuscar.Parameters.Clear()
            cmdBuscar.CommandText = "Select clie_direccion, clie_ciudad, clie_provincia from DireccionCliente where clie_codigo = @clie_codigo"
            cmdBuscar.Parameters.Add("@clie_codigo", SqlDbType.Int).Value = objCliente.Codigo
            drBuscar = cmdBuscar.ExecuteReader

            objCliente.Direccion.Clear()
            While drBuscar.Read
                Dim item As New clsDireccion
                item.Direccion = NullToString(drBuscar("clie_direccion"))
                item.Ciudad = NullToString(drBuscar("clie_ciudad"))
                item.Provincia = NullToString(drBuscar("clie_provincia"))
                objCliente.Direccion.Add(item)
                item = Nothing
            End While
            drBuscar.Close()

            Return objCliente

        Catch ex As SqlException
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return Nothing
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return Nothing
        Finally
            If objConexion.State = ConnectionState.Open Then
                objConexion.Close()
            End If
        End Try

    End Function

    Public Function BuscarXID(ByVal strID As String) As clsCliente

        Dim objCliente As New clsCliente
        Dim drBuscar As SqlDataReader
        Dim cmdBuscar As New SqlCommand

        Try
            cmdBuscar.CommandType = CommandType.Text
            cmdBuscar.Connection = objConexion
            cmdBuscar.CommandText = "Select C.clie_codigo, C.clie_nombre, C.iden_codigo, T.iden_descripcion, C.clie_nroidentificacion, C.clie_telefono, C.clie_fax, C.clie_email, C.clie_representante from Cliente C inner join TipoIdentificacion T on T.iden_codigo = C.iden_codigo where clie_nroidentificacion = @clie_nroidentificacion"
            cmdBuscar.Parameters.Add("@clie_nroidentificacion", SqlDbType.NVarChar, 11)
            cmdBuscar.Parameters("@clie_nroidentificacion").Value = strID
            If objConexion.State = ConnectionState.Closed Then
                objConexion.Open()
            End If
            drBuscar = cmdBuscar.ExecuteReader

            While drBuscar.Read()
                objCliente.Codigo = NullToString(drBuscar(0))
                objCliente.Nombre = NullToString(drBuscar(1))
                objCliente.TipoID.Codigo = NullToString(drBuscar(2))
                objCliente.TipoID.Descripcion = NullToString(drBuscar(3))
                objCliente.NumeroID = NullToString(drBuscar(4))
                objCliente.Telefono = NullToString(drBuscar(5))
                objCliente.Fax = NullToString(drBuscar(6))
                objCliente.Email = NullToString(drBuscar(7))
                objCliente.Representante = NullToString(drBuscar(8))
            End While
            drBuscar.Close()

            cmdBuscar.Parameters.Clear()
            cmdBuscar.CommandText = "Select clie_direccion, clie_ciudad, clie_provincia from DireccionCliente where clie_codigo = @clie_codigo"
            cmdBuscar.Parameters.Add("@clie_codigo", SqlDbType.Int)
            cmdBuscar.Parameters("@clie_codigo").Value = objCliente.Codigo
            drBuscar = cmdBuscar.ExecuteReader

            objCliente.Direccion.Clear()
            While drBuscar.Read
                Dim item As New clsDireccion
                item.Direccion = NullToString(drBuscar("clie_direccion"))
                item.Ciudad = NullToString(drBuscar("clie_ciudad"))
                item.Provincia = NullToString(drBuscar("clie_provincia"))
                objCliente.Direccion.Add(item)
                item = Nothing
            End While
            drBuscar.Close()

            Return objCliente

        Catch ex As SqlException
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return Nothing
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return Nothing
        Finally
            If objConexion.State = ConnectionState.Open Then
                objConexion.Close()
            End If
        End Try

    End Function

    Public Function Grabar() As Boolean

        Dim Transaccion As SqlTransaction
        Try
            'GRABAR CLIENTE
            If objConexion.State = ConnectionState.Closed Then
                objConexion.Open()
            End If
            Transaccion = objConexion.BeginTransaction
            Dim cmdGrabar As New SqlCommand
            cmdGrabar.Connection = objConexion
            cmdGrabar.Transaction = Transaccion
            cmdGrabar.CommandType = CommandType.Text
            cmdGrabar.CommandText = "Insert into Cliente (clie_nombre, iden_codigo, clie_nroidentificacion, clie_telefono, clie_fax, clie_email, clie_representante) values (@clie_nombre, @iden_codigo, @clie_nroidentificacion, @clie_telefono, @clie_fax, @clie_email, @clie_representante); Select SCOPE_IDENTITY();"
            cmdGrabar.Parameters.Add("@clie_nombre", SqlDbType.NVarChar, 70).Value = Me.Nombre
            cmdGrabar.Parameters.Add("@iden_codigo", SqlDbType.Int).Value = Me.TipoID.Codigo
            cmdGrabar.Parameters.Add("@clie_nroidentificacion", SqlDbType.NVarChar, 11).Value = Me.NumeroID
            cmdGrabar.Parameters.Add("@clie_telefono", SqlDbType.NVarChar, 50).Value = Me.Telefono
            cmdGrabar.Parameters.Add("@clie_fax", SqlDbType.NVarChar, 30).Value = Me.Fax
            cmdGrabar.Parameters.Add("@clie_email", SqlDbType.NVarChar, 50).Value = Me.Email
            cmdGrabar.Parameters.Add("@clie_representante", SqlDbType.NVarChar, 70).Value = Me.Representante

            Dim intNuevoCodigo As Integer
            intNuevoCodigo = cmdGrabar.ExecuteScalar()

            'GRABAR DIRECCIONES
            cmdGrabar = New SqlCommand
            cmdGrabar.Connection = objConexion
            cmdGrabar.Transaction = Transaccion
            cmdGrabar.CommandType = CommandType.Text
            cmdGrabar.CommandText = "Insert into DireccionCliente values (@clie_codigo, @clie_direccion, @clie_ciudad, @clie_provincia)"
            cmdGrabar.Parameters.Add("@clie_codigo", SqlDbType.SmallInt)
            cmdGrabar.Parameters.Add("@clie_direccion", SqlDbType.NVarChar, 70)
            cmdGrabar.Parameters.Add("@clie_ciudad", SqlDbType.NVarChar, 50)
            cmdGrabar.Parameters.Add("@clie_provincia", SqlDbType.NVarChar, 50)
            cmdGrabar.Parameters("@clie_codigo").Value = intNuevoCodigo

            Dim i As Integer
            For i = 0 To Me.Direccion.Count - 1
                cmdGrabar.Parameters("@clie_direccion").Value = Me.Direccion(i).Direccion
                cmdGrabar.Parameters("@clie_ciudad").Value = Me.Direccion(i).Ciudad
                cmdGrabar.Parameters("@clie_provincia").Value = Me.Direccion(i).Provincia
                cmdGrabar.ExecuteNonQuery()
            Next

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
            'ACTUALIZAR CLIENTE
            If objConexion.State = ConnectionState.Closed Then
                objConexion.Open()
            End If
            Transaccion = objConexion.BeginTransaction
            Dim cmdActualizar As New SqlCommand
            cmdActualizar.CommandType = CommandType.Text
            cmdActualizar.Connection = objConexion
            cmdActualizar.Transaction = Transaccion
            cmdActualizar.CommandText = "Update Cliente set clie_nombre = @clie_nombre, iden_codigo = @iden_codigo, clie_nroidentificacion = @clie_nroidentificacion, clie_telefono = @clie_telefono, clie_fax = @clie_fax, clie_email = @clie_email, clie_representante = @clie_representante where clie_codigo = @clie_codigo;"
            cmdActualizar.Parameters.Add("@clie_codigo", SqlDbType.Int).Value = Me.Codigo
            cmdActualizar.Parameters.Add("@clie_nombre", SqlDbType.NVarChar, 70).Value = Me.Nombre
            cmdActualizar.Parameters.Add("@iden_codigo", SqlDbType.Int).Value = Me.TipoID.Codigo
            cmdActualizar.Parameters.Add("@clie_nroidentificacion", SqlDbType.NVarChar, 11).Value = Me.NumeroID
            cmdActualizar.Parameters.Add("@clie_telefono", SqlDbType.NVarChar, 50).Value = Me.Telefono
            cmdActualizar.Parameters.Add("@clie_fax", SqlDbType.NVarChar, 30).Value = Me.Fax
            cmdActualizar.Parameters.Add("@clie_email", SqlDbType.NVarChar, 50).Value = Me.Email
            cmdActualizar.Parameters.Add("@clie_representante", SqlDbType.NVarChar, 70).Value = Me.Representante

            cmdActualizar.ExecuteNonQuery()

            'ACTUALIZAR DIRECCIONES
            cmdActualizar = New SqlCommand
            cmdActualizar.Connection = objConexion
            cmdActualizar.Transaction = Transaccion
            cmdActualizar.CommandType = CommandType.Text
            cmdActualizar.CommandText = "Delete from DireccionCliente where clie_codigo = @clie_codigo;"
            cmdActualizar.Parameters.Add("@clie_codigo", SqlDbType.SmallInt).Value = Me.Codigo
            cmdActualizar.ExecuteNonQuery()
            cmdActualizar.CommandText = "Insert into DireccionCliente values (@clie_codigo, @clie_direccion, @clie_ciudad, @clie_provincia)"
            cmdActualizar.Parameters.Add("@clie_direccion", SqlDbType.NVarChar, 70)
            cmdActualizar.Parameters.Add("@clie_ciudad", SqlDbType.NVarChar, 50)
            cmdActualizar.Parameters.Add("@clie_provincia", SqlDbType.NVarChar, 50)

            Dim i As Integer
            For i = 0 To Me.Direccion.Count - 1
                cmdActualizar.Parameters("@clie_direccion").Value = Me.Direccion(i).Direccion
                cmdActualizar.Parameters("@clie_ciudad").Value = Me.Direccion(i).Ciudad
                cmdActualizar.Parameters("@clie_provincia").Value = Me.Direccion(i).Provincia
                cmdActualizar.ExecuteNonQuery()
            Next

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
