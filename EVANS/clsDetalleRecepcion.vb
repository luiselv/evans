Imports System.Data.SqlClient

Public Class clsDetalleRecepcion

    Private intCodigo As Integer
    Private strTipoDoc As String
    Private strNroDoc As String
    Private dblCantidad As Double
    Private strDescripcion As String
    Private dblPeso As Double
    Private strUnidad As String
    Private dblCosto As Double

    Public Property Codigo() As Integer
        Get
            Return intCodigo
        End Get
        Set(ByVal value As Integer)
            intCodigo = value
        End Set
    End Property

    Public Property TipoDocumento() As String
        Get
            Return strTipoDoc
        End Get
        Set(ByVal value As String)
            strTipoDoc = value
        End Set
    End Property

    Public Property NumeroDocumento() As String
        Get
            Return strNroDoc
        End Get
        Set(ByVal value As String)
            strNroDoc = value
        End Set
    End Property

    Public Property Cantidad() As Double
        Get
            Return dblCantidad
        End Get
        Set(ByVal value As Double)
            dblCantidad = value
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

    Public Property Peso() As Double
        Get
            Return dblPeso
        End Get
        Set(ByVal value As Double)
            dblPeso = value
        End Set
    End Property

    Public Property Unidad() As String
        Get
            Return strUnidad
        End Get
        Set(ByVal value As String)
            strUnidad = value
        End Set
    End Property

    Public Property Costo() As Double
        Get
            Return dblCosto
        End Get
        Set(ByVal value As Double)
            dblCosto = value
        End Set
    End Property

End Class
