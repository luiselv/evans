Imports System.Data.SqlClient

Public Class clsRecepcion
    Private intCodigo As Integer
    Private datEmision As Date
    Private objRemitente As New clsCliente
    Private intTipoDirPartida As Integer
    Private strPartida As String
    Private strDistritoPartida As String
    Private strCiudadPartida As String
    Private objDestinatario As New clsCliente
    Private intTipoDirDestino As Integer
    Private strDestino As String
    Private strDistritoDestino As String
    Private strCiudadDestino As String
    Private strGuiaRemision As String 'numero de guia de remision (en caso de tener)
    Private objDestino As New clsDestino
    Private objEstado As New clsEstado
    Private intBultos As Integer
    Private dblPesoTotal As Double
    Private dblCostoTotal As Double
    Private strObservacion As String
    Private lstDetalle As New List(Of clsDetalleRecepcion)
    Private objUsuario As New clsUsuario


    Public Property Codigo() As Integer
        Get
            Return intCodigo
        End Get
        Set(ByVal value As Integer)
            intCodigo = value
        End Set
    End Property

    Public Property FechaEmision() As Date
        Get
            Return datEmision
        End Get
        Set(ByVal value As Date)
            datEmision = value
        End Set
    End Property

    Public Property Remitente() As clsCliente
        Get
            Return objRemitente
        End Get
        Set(ByVal value As clsCliente)
            objRemitente = value
        End Set
    End Property

    Public Property TipoDireccionPartida() As Integer
        Get
            Return intTipoDirPartida
        End Get
        Set(ByVal value As Integer)
            intTipoDirPartida = value
        End Set
    End Property

    Public Property DireccionPartida() As String
        Get
            Return strPartida
        End Get
        Set(ByVal value As String)
            strPartida = value
        End Set
    End Property

    Public Property DistritoPartida() As String
        Get
            Return strDistritoPartida
        End Get
        Set(ByVal value As String)
            strDistritoPartida = value
        End Set
    End Property

    Public Property CiudadPartida() As String
        Get
            Return strCiudadPartida
        End Get
        Set(ByVal value As String)
            strCiudadPartida = value
        End Set
    End Property

    Public Property Destinatario() As clsCliente
        Get
            Return objDestinatario
        End Get
        Set(ByVal value As clsCliente)
            objDestinatario = value
        End Set
    End Property

    Public Property TipoDireccionDestino() As Integer
        Get
            Return intTipoDirDestino
        End Get
        Set(ByVal value As Integer)
            intTipoDirDestino = value
        End Set
    End Property

    Public Property DireccionDestino() As String
        Get
            Return strDestino
        End Get
        Set(ByVal value As String)
            strDestino = value
        End Set
    End Property

    Public Property DistritoDestino() As String
        Get
            Return strDistritoDestino
        End Get
        Set(ByVal value As String)
            strDistritoDestino = value
        End Set
    End Property

    Public Property CiudadDestino() As String
        Get
            Return strCiudadDestino
        End Get
        Set(ByVal value As String)
            strCiudadDestino = value
        End Set
    End Property

    Public Property GuiaRemision() As String
        Get
            Return strGuiaRemision
        End Get
        Set(ByVal value As String)
            strGuiaRemision = value
        End Set
    End Property

    Public Property Destino() As clsDestino
        Get
            Return objDestino
        End Get
        Set(ByVal value As clsDestino)
            objDestino = value
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

    Public Property Detalles() As List(Of clsDetalleRecepcion)
        Get
            Return lstDetalle
        End Get
        Set(ByVal value As List(Of clsDetalleRecepcion))
            lstDetalle = value
        End Set
    End Property

    Public Property BultosTotales() As Integer
        Get
            Return intBultos
        End Get
        Set(ByVal value As Integer)
            intBultos = value
        End Set
    End Property

    Public Property PesoTotal() As Double
        Get
            Return dblPesoTotal
        End Get
        Set(ByVal value As Double)
            dblPesoTotal = value
        End Set
    End Property

    Public Property CostoTotal() As Double
        Get
            Return dblCostoTotal
        End Get
        Set(ByVal value As Double)
            dblCostoTotal = value
        End Set
    End Property

    Public Property Observacion() As String
        Get
            Return strObservacion
        End Get
        Set(ByVal value As String)
            strObservacion = value
        End Set
    End Property

    Public Property Usuario() As clsUsuario
        Get
            Return objUsuario
        End Get
        Set(ByVal value As clsUsuario)
            objUsuario = value
        End Set
    End Property

    'FUNCIONES

    Public Function BuscarXCodigo() As Boolean

        Try
            Dim cmdBuscar As New SqlCommand
            Dim drBuscar As SqlDataReader
            cmdBuscar.CommandType = CommandType.Text
            cmdBuscar.Connection = objConexion2
            cmdBuscar.Parameters.Add("@rece_codigo", SqlDbType.Int)
            cmdBuscar.Parameters("@rece_codigo").Value = Me.Codigo
            cmdBuscar.CommandText = "Select G.rece_codigo, G.clie_remitente, R.clie_nombre, R.clie_nroidentificacion, TR.iden_codigo, TR.iden_descripcion, G.rece_tipodirpartida, G.rece_direccionpartida, G.clie_destinatario, D.clie_nombre, D.clie_nroidentificacion, TD.iden_codigo, TD.iden_descripcion, G.rece_tipodirdestino, G.rece_direcciondestino, G.rece_fechaemision, G.rece_guiaremision, G.rece_bultos, G.rece_pesototal, G.rece_costototal, G.esta_codigo, E.esta_descripcion, G.dest_codigo, X.dest_nombre, X.dest_distanciavirtual, G.rece_observacion, G.usu_codigo, U.usu_nombrecompleto From recepcion G inner join EVANS.dbo.Cliente R on R.clie_codigo = G.clie_remitente inner join EVANS.dbo.Cliente D on D.clie_codigo = G.clie_destinatario inner join EVANS.dbo.Estado E on E.esta_codigo = G.esta_codigo inner join EVANS.dbo.TipoIdentificacion TR on TR.iden_codigo = R.iden_codigo inner join EVANS.dbo.TipoIdentificacion TD on TD.iden_codigo = D.iden_codigo inner join EVANS.dbo.Destino X on X.dest_codigo = G.dest_codigo inner join EVANS.dbo.Usuario U on U.usu_codigo = G.usu_codigo where G.rece_codigo = @rece_codigo"
            If objConexion2.State = ConnectionState.Closed Then
                objConexion2.Open()
            End If
            drBuscar = cmdBuscar.ExecuteReader()

            While drBuscar.Read()
                Me.Remitente.Codigo = NullToString(drBuscar(1))
                Me.Remitente.Nombre = NullToString(drBuscar(2))
                Me.Remitente.NumeroID = NullToString(drBuscar(3))
                Me.Remitente.TipoID.Codigo = NullToString(drBuscar(4))
                Me.Remitente.TipoID.Descripcion = NullToString(drBuscar(5))
                Me.TipoDireccionPartida = NullToString(drBuscar(6))
                Me.DireccionPartida = NullToString(drBuscar(7))
                Me.Destinatario.Codigo = NullToString(drBuscar(8))
                Me.Destinatario.Nombre = NullToString(drBuscar(9))
                Me.Destinatario.NumeroID = NullToString(drBuscar(10))
                Me.Destinatario.TipoID.Codigo = NullToString(drBuscar(11))
                Me.Destinatario.TipoID.Descripcion = NullToString(drBuscar(12))
                Me.TipoDireccionDestino = NullToString(drBuscar(13))
                Me.DireccionDestino = NullToString(drBuscar(14))
                Me.FechaEmision = NullToString(drBuscar(15))
                Me.GuiaRemision = NullToString(drBuscar(16))
                Me.BultosTotales = NullToString(drBuscar(17))
                Me.PesoTotal = NullToString(drBuscar(18))
                Me.CostoTotal = NullToString(drBuscar(19))
                Me.Estado.Codigo = NullToString(drBuscar(20))
                Me.Estado.Descripcion = NullToString(drBuscar(21))
                Me.Destino.Codigo = NullToString(drBuscar(22))
                Me.Destino.Nombre = NullToString(drBuscar(23))
                Me.Destino.DistanciaVirtual = NullToString(drBuscar(24))
                Me.Observacion = NullToString(drBuscar(25))
                Me.Usuario.Codigo = NullToString(drBuscar(26))
                Me.Usuario.NombreCompleto = NullToString(drBuscar(27))
            End While

            drBuscar.Close()
            cmdBuscar.CommandText = "Select dere_cantidad, dere_descripcion, dere_peso, dere_unidad, dere_costo, dere_tipodoc, dere_nrodoc from DetalleRecepcion where rece_codigo = @rece_codigo"

            If objConexion2.State = ConnectionState.Closed Then
                objConexion2.Open()
            End If

            drBuscar = cmdBuscar.ExecuteReader()

            Me.Detalles.Clear()
            While drBuscar.Read
                Dim item As New clsDetalleRecepcion
                item.Cantidad = NullToString(drBuscar("dere_cantidad"))
                item.Descripcion = NullToString(drBuscar("dere_descripcion"))
                item.Peso = NullToString(drBuscar("dere_peso"))
                item.Unidad = NullToString(drBuscar("dere_unidad"))
                item.Costo = NullToString(drBuscar("dere_costo"))
                item.TipoDocumento = NullToString(drBuscar("dere_tipodoc"))
                item.NumeroDocumento = NullToString(drBuscar("dere_nrodoc"))
                Me.Detalles.Add(item)
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
            If objConexion2.State = ConnectionState.Open Then
                objConexion2.Close()
            End If
        End Try

    End Function

    Public Function Grabar() As Boolean

        Dim Transaccion As SqlTransaction
        Try
            If objConexion2.State = ConnectionState.Closed Then
                objConexion2.Open()
            End If
            Transaccion = objConexion2.BeginTransaction(IsolationLevel.Serializable)
            Dim cmd As New SqlCommand
            cmd.CommandType = CommandType.Text
            cmd.Connection = objConexion2
            cmd.Transaction = Transaccion
            cmd.CommandText = "Insert into Recepcion (rece_fechaemision, clie_remitente, rece_tipodirpartida, rece_direccionpartida, clie_destinatario, rece_tipodirdestino, rece_direcciondestino, dest_codigo, esta_codigo, rece_bultos, rece_pesototal, rece_costototal, rece_guiaremision, rece_observacion, usu_codigo) values (@rece_fechaemision, @clie_remitente, @rece_tipodirpartida, @rece_direccionpartida, @clie_destinatario, @rece_tipodirdestino, @rece_direcciondestino, @dest_codigo, @esta_codigo, @rece_bultos, @rece_pesototal, @rece_costototal, @rece_guiaremision, @rece_observacion, @usu_codigo); Select SCOPE_IDENTITY();"
            cmd.Parameters.Add("@rece_fechaemision", SqlDbType.DateTime).Value = Me.FechaEmision
            cmd.Parameters.Add("@clie_remitente", SqlDbType.Int).Value = Me.Remitente.Codigo
            cmd.Parameters.Add("@rece_tipodirpartida", SqlDbType.Int).Value = Me.TipoDireccionPartida
            cmd.Parameters.Add("@rece_direccionpartida", SqlDbType.NVarChar, 100).Value = Me.DireccionPartida
            cmd.Parameters.Add("@clie_destinatario", SqlDbType.Int).Value = Me.Destinatario.Codigo
            cmd.Parameters.Add("@rece_tipodirdestino", SqlDbType.Int).Value = Me.TipoDireccionDestino
            cmd.Parameters.Add("@rece_direcciondestino", SqlDbType.NVarChar, 100).Value = Me.DireccionDestino
            cmd.Parameters.Add("@dest_codigo", SqlDbType.Int).Value = Me.Destino.Codigo
            cmd.Parameters.Add("@esta_codigo", SqlDbType.Int).Value = Me.Estado.Codigo
            cmd.Parameters.Add("@rece_bultos", SqlDbType.Int).Value = Me.BultosTotales
            cmd.Parameters.Add("@rece_pesototal", SqlDbType.Float).Value = Me.PesoTotal
            cmd.Parameters.Add("@rece_costototal", SqlDbType.Float).Value = Me.CostoTotal
            cmd.Parameters.Add("@rece_guiaremision", SqlDbType.NVarChar, 20).Value = Me.GuiaRemision
            cmd.Parameters.Add("@rece_observacion", SqlDbType.NVarChar, 250).Value = Me.Observacion
            cmd.Parameters.Add("@usu_codigo", SqlDbType.Int).Value = Me.Usuario.Codigo

            Dim i, IDGuia As Integer
            IDGuia = cmd.ExecuteScalar()

            'GRABAR DETALLES DE GUIA
            cmd.Parameters.Clear()
            cmd.CommandText = "Insert into DetalleRecepcion (rece_codigo, dere_cantidad, dere_descripcion, dere_peso, dere_unidad, dere_costo, dere_tipodoc, dere_nrodoc) VALUES (@rece_codigo, @dere_cantidad, @dere_descripcion, @dere_peso, @dere_unidad, @dere_costo, @dere_tipodoc, @dere_nrodoc)"
            cmd.Parameters.Add("@rece_codigo", SqlDbType.Int)
            cmd.Parameters.Add("@dere_cantidad", SqlDbType.Float)
            cmd.Parameters.Add("@dere_descripcion", SqlDbType.NVarChar, 100)
            cmd.Parameters.Add("@dere_peso", SqlDbType.Float)
            cmd.Parameters.Add("@dere_unidad", SqlDbType.NVarChar, 30)
            cmd.Parameters.Add("@dere_costo", SqlDbType.Float)
            cmd.Parameters.Add("@dere_tipodoc", SqlDbType.VarChar, 20)
            cmd.Parameters.Add("@dere_nrodoc", SqlDbType.VarChar, 20)

            For i = 0 To Me.Detalles.Count - 1
                cmd.Parameters("@rece_codigo").Value = IDGuia
                cmd.Parameters("@dere_cantidad").Value = Me.Detalles(i).Cantidad
                cmd.Parameters("@dere_descripcion").Value = Me.Detalles(i).Descripcion
                cmd.Parameters("@dere_peso").Value = Me.Detalles(i).Peso
                cmd.Parameters("@dere_unidad").Value = Me.Detalles(i).Unidad
                cmd.Parameters("@dere_costo").Value = Me.Detalles(i).Costo
                cmd.Parameters("@dere_tipodoc").Value = Me.Detalles(i).TipoDocumento
                cmd.Parameters("@dere_nrodoc").Value = Me.Detalles(i).NumeroDocumento
                If objConexion2.State = ConnectionState.Closed Then
                    objConexion2.Open()
                End If
                cmd.ExecuteNonQuery()
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
            If objConexion2.State = ConnectionState.Open Then
                objConexion2.Close()
            End If
        End Try

    End Function

    Public Function Actualizar() As Boolean

        Dim Transaccion As SqlTransaction
        Try
            If objConexion2.State = ConnectionState.Closed Then
                objConexion2.Open()
            End If
            Transaccion = objConexion2.BeginTransaction

            'ACTUALIZAR CABECERA DE GUIA
            Dim cmd As New SqlCommand
            cmd.CommandType = CommandType.Text
            cmd.Connection = objConexion2
            cmd.Transaction = Transaccion
            cmd.CommandText = "Update Recepcion set rece_fechaemision = @rece_fechaemision, clie_remitente = @clie_remitente, rece_tipodirpartida = @rece_tipodirpartida, rece_direccionpartida = @rece_direccionpartida, clie_destinatario = @clie_destinatario, rece_tipodirdestino = @rece_tipodirdestino, rece_direcciondestino = @rece_direcciondestino, dest_codigo = @dest_codigo, esta_codigo = @esta_codigo, rece_bultos = @rece_bultos, rece_pesototal = @rece_pesototal, rece_costototal = @rece_costototal, rece_guiaremision = @rece_guiaremision, rece_observacion = @rece_observacion, usu_codigo = @usu_codigo where rece_codigo = @rece_codigo"
            cmd.Parameters.Add("@rece_codigo", SqlDbType.Int).Value = Me.Codigo
            cmd.Parameters.Add("@rece_fechaemision", SqlDbType.DateTime).Value = Me.FechaEmision
            cmd.Parameters.Add("@clie_remitente", SqlDbType.Int).Value = Me.Remitente.Codigo
            cmd.Parameters.Add("@rece_tipodirpartida", SqlDbType.Int).Value = Me.TipoDireccionPartida
            cmd.Parameters.Add("@rece_direccionpartida", SqlDbType.NVarChar, 100).Value = Me.DireccionPartida
            cmd.Parameters.Add("@clie_destinatario", SqlDbType.Int).Value = Me.Destinatario.Codigo
            cmd.Parameters.Add("@rece_tipodirdestino", SqlDbType.Int).Value = Me.TipoDireccionDestino
            cmd.Parameters.Add("@rece_direcciondestino", SqlDbType.NVarChar, 100).Value = Me.DireccionDestino
            cmd.Parameters.Add("@dest_codigo", SqlDbType.Int).Value = Me.Destino.Codigo
            cmd.Parameters.Add("@esta_codigo", SqlDbType.Int).Value = Me.Estado.Codigo
            cmd.Parameters.Add("@rece_bultos", SqlDbType.Int).Value = Me.BultosTotales
            cmd.Parameters.Add("@rece_pesototal", SqlDbType.Float).Value = Me.PesoTotal
            cmd.Parameters.Add("@rece_costototal", SqlDbType.Float).Value = Me.CostoTotal
            cmd.Parameters.Add("@rece_guiaremision", SqlDbType.NVarChar, 20).Value = Me.GuiaRemision
            cmd.Parameters.Add("@rece_observacion", SqlDbType.NVarChar, 250).Value = Me.Observacion
            cmd.Parameters.Add("@usu_codigo", SqlDbType.Int).Value = Me.Usuario.Codigo
            cmd.ExecuteNonQuery()

            'ACTUALIZAR DETALLE DE GUIA
            cmd.CommandText = "Delete from DetalleRecepcion where rece_codigo = @rece_codigo;"
            cmd.ExecuteNonQuery()

            cmd.Parameters.Clear()
            cmd.CommandText = "Insert into DetalleRecepcion (rece_codigo, dere_cantidad, dere_descripcion, dere_peso, dere_unidad, dere_costo, dere_tipodoc, dere_nrodoc) VALUES (@rece_codigo, @dere_cantidad, @dere_descripcion, @dere_peso, @dere_unidad, @dere_costo, @dere_tipodoc, @dere_nrodoc)"
            cmd.Parameters.Add("@rece_codigo", SqlDbType.Int)
            cmd.Parameters.Add("@dere_cantidad", SqlDbType.Float)
            cmd.Parameters.Add("@dere_descripcion", SqlDbType.NVarChar, 100)
            cmd.Parameters.Add("@dere_peso", SqlDbType.Float)
            cmd.Parameters.Add("@dere_unidad", SqlDbType.NVarChar, 30)
            cmd.Parameters.Add("@dere_costo", SqlDbType.Float)
            cmd.Parameters.Add("@dere_tipodoc", SqlDbType.VarChar, 20)
            cmd.Parameters.Add("@dere_nrodoc", SqlDbType.VarChar, 20)

            For i = 0 To Me.Detalles.Count - 1
                cmd.Parameters("@rece_codigo").Value = Me.Codigo
                cmd.Parameters("@dere_cantidad").Value = Me.Detalles(i).Cantidad
                cmd.Parameters("@dere_descripcion").Value = Me.Detalles(i).Descripcion
                cmd.Parameters("@dere_peso").Value = Me.Detalles(i).Peso
                cmd.Parameters("@dere_unidad").Value = Me.Detalles(i).Unidad
                cmd.Parameters("@dere_costo").Value = Me.Detalles(i).Costo
                cmd.Parameters("@dere_tipodoc").Value = Me.Detalles(i).TipoDocumento
                cmd.Parameters("@dere_nrodoc").Value = Me.Detalles(i).NumeroDocumento
                If objConexion2.State = ConnectionState.Closed Then
                    objConexion2.Open()
                End If
                cmd.ExecuteNonQuery()
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
            If objConexion2.State = ConnectionState.Open Then
                objConexion2.Close()
            End If
        End Try

    End Function

    Public Function Eliminar(ByVal codigo As Integer) As Boolean

        Dim Transaccion As SqlTransaction
        Try
            If objConexion2.State = ConnectionState.Closed Then
                objConexion2.Open()
            End If
            Transaccion = objConexion2.BeginTransaction(IsolationLevel.Serializable)
            Dim cmd As New SqlCommand
            cmd.CommandType = CommandType.Text
            cmd.Connection = objConexion2
            cmd.Transaction = Transaccion
            cmd.CommandText = "Delete from DetalleRecepcion where rece_codigo = @rece_codigo; Delete from Recepcion where rece_codigo = @rece_codigo"
            cmd.Parameters.Add("@rece_codigo", SqlDbType.Int).Value = codigo
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
            If objConexion2.State = ConnectionState.Open Then
                objConexion2.Close()
            End If
        End Try

    End Function
End Class
