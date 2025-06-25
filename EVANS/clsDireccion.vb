Public Class clsDireccion

    Private strDireccion As String
    Private strCiudad As String
    Private strProvincia As String


    Public Property Direccion() As String
        Get
            Return strDireccion
        End Get
        Set(ByVal value As String)
            strDireccion = value
        End Set
    End Property

    Public Property Ciudad() As String
        Get
            Return strCiudad
        End Get
        Set(ByVal value As String)
            strCiudad = value
        End Set
    End Property

    Public Property Provincia() As String
        Get
            Return strProvincia
        End Get
        Set(ByVal value As String)
            strProvincia = value
        End Set
    End Property

End Class
