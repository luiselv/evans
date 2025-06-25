Imports System.Data.SqlClient

Public Class clsParametro

    Private dblPorcentajeIGV As Double
    Private strFactSerie As String
    Private strFactNro1 As String
    Private strFactNro2 As String
    Private strBolSerie As String
    Private strBolNro1 As String
    Private strBolNro2 As String
    Private strGRemSerie As String
    Private strGRemNro1 As String
    Private strGRemNro2 As String
    Private strManifiesto As String
    'Parametros de envio de email
    Private strRemitente As String
    Private strEmailRemitente As String
    Private strPassRemitente As String
    Private strSMTP As String
    Private intPuerto As Integer


    Public Property PorcentajeIGV() As Double
        Get
            Return dblPorcentajeIGV
        End Get
        Set(ByVal value As Double)
            dblPorcentajeIGV = value
        End Set
    End Property

    Public Property FacturaSerie() As String
        Get
            Return strFactSerie
        End Get
        Set(ByVal value As String)
            strFactSerie = value
        End Set
    End Property

    Public Property FacturaNro1() As String
        Get
            Return strFactNro1
        End Get
        Set(ByVal value As String)
            strFactNro1 = value
        End Set
    End Property

    Public Property FacturaNro2() As String
        Get
            Return strFactNro2
        End Get
        Set(ByVal value As String)
            strFactNro2 = value
        End Set
    End Property

    Public Property BoletaSerie() As String
        Get
            Return strBolSerie
        End Get
        Set(ByVal value As String)
            strBolSerie = value
        End Set
    End Property

    Public Property BoletaNro1() As String
        Get
            Return strBolNro1
        End Get
        Set(ByVal value As String)
            strBolNro1 = value
        End Set
    End Property

    Public Property BoletaNro2() As String
        Get
            Return strBolNro2
        End Get
        Set(ByVal value As String)
            strBolNro2 = value
        End Set
    End Property

    Public Property GRemisionSerie() As String
        Get
            Return strGRemSerie
        End Get
        Set(ByVal value As String)
            strGRemSerie = value
        End Set
    End Property

    Public Property GRemisionNro1() As String
        Get
            Return strGRemNro1
        End Get
        Set(ByVal value As String)
            strGRemNro1 = value
        End Set
    End Property

    Public Property GRemisionNro2() As String
        Get
            Return strGRemNro2
        End Get
        Set(ByVal value As String)
            strGRemNro2 = value
        End Set
    End Property

    Public Property Manifiesto() As String
        Get
            Return strManifiesto
        End Get
        Set(ByVal value As String)
            strManifiesto = value
        End Set
    End Property

    'PROPIEDADES DE CONFIGURACION DE EMAIL

    Public Property Remitente() As String
        Get
            Return strRemitente
        End Get
        Set(ByVal value As String)
            strRemitente = value
        End Set
    End Property

    Public Property EmailRemitente() As String
        Get
            Return strEmailRemitente
        End Get
        Set(ByVal value As String)
            strEmailRemitente = value
        End Set
    End Property

    Public Property PassRemitente() As String
        Get
            Return strPassRemitente
        End Get
        Set(ByVal value As String)
            strPassRemitente = value
        End Set
    End Property

    Public Property SMTP() As String
        Get
            Return strSMTP
        End Get
        Set(ByVal value As String)
            strSMTP = value
        End Set
    End Property

    Public Property Puerto() As Integer
        Get
            Return intPuerto
        End Get
        Set(ByVal value As Integer)
            intPuerto = value
        End Set
    End Property


    'FUNCIONES

    Public Sub CargarParametros()

        Try
            Dim drParametros As SqlDataReader
            Dim cmdParametros As New SqlCommand
            cmdParametros.CommandType = CommandType.Text
            cmdParametros.Connection = objConexion
            cmdParametros.CommandText = "Select * from EVANS.dbo.Parametros"
            If objConexion.State = ConnectionState.Closed Then
                objConexion.Open()
            End If
            drParametros = cmdParametros.ExecuteReader
            While drParametros.Read
                Me.PorcentajeIGV = NullToString(drParametros("para_igv"))
                Me.FacturaSerie = NullToString(drParametros("para_factserie"))
                Me.FacturaNro1 = NullToString(drParametros("para_factnro1"))
                Me.FacturaNro2 = NullToString(drParametros("para_factnro2"))
                Me.BoletaSerie = NullToString(drParametros("para_bolserie"))
                Me.BoletaNro1 = NullToString(drParametros("para_bolnro1"))
                Me.BoletaNro2 = NullToString(drParametros("para_bolnro2"))
                Me.GRemisionSerie = NullToString(drParametros("para_gremserie"))
                Me.GRemisionNro1 = NullToString(drParametros("para_gremnro1"))
                Me.GRemisionNro2 = NullToString(drParametros("para_gremnro2"))
                Me.Manifiesto = NullToString(drParametros("para_manifiesto"))
                Me.Remitente = NullToString(drParametros("para_remitente"))
                Me.EmailRemitente = NullToString(drParametros("para_emailremitente"))
                Me.PassRemitente = NullToString(drParametros("para_passremitente"))
                Me.SMTP = NullToString(drParametros("para_smtp"))
                Me.Puerto = NullToString(drParametros("para_puerto"))
            End While
            drParametros.Close()

        Catch ex As SqlException
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        Finally
            If objConexion.State = ConnectionState.Open Then
                objConexion.Close()
            End If
        End Try

    End Sub

    Public Function Grabar() As Boolean

        Dim Transaccion As SqlTransaction
        Try
            If objConexion.State = ConnectionState.Closed Then
                objConexion.Open()
            End If
            Transaccion = objConexion.BeginTransaction
            Dim cmd As New SqlCommand
            cmd.CommandType = CommandType.Text
            cmd.Connection = objConexion
            cmd.Transaction = Transaccion
            cmd.CommandText = "Update Parametros set para_igv = @para_igv, para_factserie = @para_factserie, para_factnro1 = @para_factnro1, para_factnro2 = @para_factnro2, para_bolserie = @para_bolserie, para_bolnro1 = @para_bolnro1, para_bolnro2 = @para_bolnro2, para_gremserie = @para_gremserie, para_gremnro1 = @para_gremnro1, para_gremnro2 = @para_gremnro2, para_manifiesto = @para_manifiesto, para_remitente = @para_remitente, para_emailremitente = @para_emailremitente, para_passremitente = @para_passremitente, para_smtp = @para_smtp, para_puerto = @para_puerto"
            cmd.Parameters.Add("@para_igv", SqlDbType.Float).Value = Me.PorcentajeIGV
            cmd.Parameters.Add("@para_factserie", SqlDbType.NVarChar, 4).Value = Me.FacturaSerie
            cmd.Parameters.Add("@para_factnro1", SqlDbType.NVarChar, 6).Value = Me.FacturaNro1
            cmd.Parameters.Add("@para_factnro2", SqlDbType.NVarChar, 6).Value = Me.FacturaNro2
            cmd.Parameters.Add("@para_bolserie", SqlDbType.NVarChar, 4).Value = Me.BoletaSerie
            cmd.Parameters.Add("@para_bolnro1", SqlDbType.NVarChar, 6).Value = Me.BoletaNro1
            cmd.Parameters.Add("@para_bolnro2", SqlDbType.NVarChar, 6).Value = Me.BoletaNro2
            cmd.Parameters.Add("@para_gremserie", SqlDbType.NVarChar, 4).Value = Me.GRemisionSerie
            cmd.Parameters.Add("@para_gremnro1", SqlDbType.NVarChar, 6).Value = Me.GRemisionNro1
            cmd.Parameters.Add("@para_gremnro2", SqlDbType.NVarChar, 6).Value = Me.GRemisionNro2
            cmd.Parameters.Add("@para_manifiesto", SqlDbType.NVarChar, 15).Value = Me.Manifiesto
            cmd.Parameters.Add("@para_remitente", SqlDbType.NVarChar, 70).Value = Me.Remitente
            cmd.Parameters.Add("@para_emailremitente", SqlDbType.NVarChar, 70).Value = Me.EmailRemitente
            cmd.Parameters.Add("@para_passremitente", SqlDbType.NVarChar, 30).Value = Me.PassRemitente
            cmd.Parameters.Add("@para_smtp", SqlDbType.NVarChar, 50).Value = Me.SMTP
            cmd.Parameters.Add("@para_puerto", SqlDbType.Int).Value = Me.Puerto
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
