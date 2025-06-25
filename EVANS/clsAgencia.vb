Imports System.Data.SqlClient

Public Class clsAgencia
    Private intCodigo As Integer
    Private strDireccion As String
    Private objCiudad As New clsDestino
    Private objEstado As New clsEstado


    Public Property Codigo() As Integer
        Get
            Return intCodigo
        End Get
        Set(ByVal value As Integer)
            intCodigo = value
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

    Public Property Ciudad() As clsDestino
        Get
            Return objCiudad
        End Get
        Set(ByVal value As clsDestino)
            objCiudad = value
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


    Public Function Listar() As List(Of clsAgencia)

        Dim cmdListar As New SqlCommand
        Dim drListar As SqlDataReader
        Dim lstLista As New List(Of clsAgencia)

        Try
            cmdListar.CommandText = "select A.agen_codigo, A.agen_direccion, D.dest_codigo, D.dest_nombre, E.esta_codigo, E.esta_descripcion from Agencia A inner join Destino D on D.dest_codigo = A.dest_codigo inner join Estado E on E.esta_codigo = A.esta_codigo;"
            cmdListar.CommandType = CommandType.Text
            cmdListar.Connection = objConexion

            If objConexion.State = ConnectionState.Closed Then
                objConexion.Open()
            End If

            drListar = cmdListar.ExecuteReader()
            While drListar.Read()
                Dim objAgencia As New clsAgencia
                objAgencia.Codigo = NullToString(drListar(0))
                objAgencia.Direccion = NullToString(drListar(1))
                objAgencia.Ciudad.Codigo = NullToString(drListar(2))
                objAgencia.Ciudad.Nombre = NullToString(drListar(3))
                objAgencia.Estado.Codigo = NullToString(drListar(4))
                objAgencia.Estado.Descripcion = NullToString(drListar(5))
                lstLista.Add(objAgencia)
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

End Class
