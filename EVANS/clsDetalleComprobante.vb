Public Class clsDetalleComprobante

    Private intCodigo As Integer
    Private dblCantidad As Double
    Private strDescripcion As String
    Private dblPrecioUnitario As Double
    Private dblFlete As Double

    Public Property Codigo() As Integer
        Get
            Return intCodigo
        End Get
        Set(ByVal value As Integer)
            intCodigo = value
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

    Public Property PrecioUnitario() As Double
        Get
            Return dblPrecioUnitario
        End Get
        Set(ByVal value As Double)
            dblPrecioUnitario = value
        End Set
    End Property

    Public Property Flete() As Double
        Get
            Return dblFlete
        End Get
        Set(ByVal value As Double)
            dblFlete = value
        End Set
    End Property

End Class
