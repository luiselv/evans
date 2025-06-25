Imports System.Data.SqlClient

Public Class clsComprobante

    Private intCodigo As Integer
    Private strSerie As String
    Private strNumero As String
    Private datFecha As Date
    Private objDestinatario As New clsCliente
    Private strDireccion As String
    Private objTipoComprobante As New clsTipoComprobante
    Private objEstado As New clsEstado
    Private strGRT As String
    Private objRemitente As New clsCliente
    Private objTransportista As New clsEmpresa
    Private objDestino As New clsDestino
    Private strManifiesto As String
    Private lstDetalles As New List(Of clsDetalleComprobante)
    Private dblValorVenta As Double
    Private dblIGV As Double
    Private dblTotal As Double
    Private intImpreso As Integer
    Private objUsuario As New clsUsuario



    Public Property Codigo() As Integer
        Get
            Return intCodigo
        End Get
        Set(ByVal value As Integer)
            intCodigo = value
        End Set
    End Property

    Public Property Serie() As String
        Get
            Return strSerie
        End Get
        Set(ByVal value As String)
            strSerie = value
        End Set
    End Property

    Public Property Numero() As String
        Get
            Return strNumero
        End Get
        Set(ByVal value As String)
            strNumero = value
        End Set
    End Property

    Public Property Fecha() As Date
        Get
            Return datFecha
        End Get
        Set(ByVal value As Date)
            datFecha = value
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

    Public Property Direccion() As String
        Get
            Return strDireccion
        End Get
        Set(ByVal value As String)
            strDireccion = value
        End Set
    End Property

    Public Property TipoComprobante() As clsTipoComprobante
        Get
            Return objTipoComprobante
        End Get
        Set(ByVal value As clsTipoComprobante)
            objTipoComprobante = value
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

    Public Property GuiaRemisionTransportista() As String
        Get
            Return strGRT
        End Get
        Set(ByVal value As String)
            strGRT = value
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

    Public Property Transportista() As clsEmpresa
        Get
            Return objTransportista
        End Get
        Set(ByVal value As clsEmpresa)
            objTransportista = value
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

    Public Property Manifiesto() As String
        Get
            Return strManifiesto
        End Get
        Set(ByVal value As String)
            strManifiesto = value
        End Set
    End Property

    Public Property Detalles() As List(Of clsDetalleComprobante)
        Get
            Return lstDetalles
        End Get
        Set(ByVal value As List(Of clsDetalleComprobante))
            lstDetalles = value
        End Set
    End Property

    Public Property ValorVenta() As Double
        Get
            Return dblValorVenta
        End Get
        Set(ByVal value As Double)
            dblValorVenta = value
        End Set
    End Property

    Public Property IGV() As Double
        Get
            Return dblIGV
        End Get
        Set(ByVal value As Double)
            dblIGV = value
        End Set
    End Property

    Public Property Total() As Double
        Get
            Return dblTotal
        End Get
        Set(ByVal value As Double)
            dblTotal = value
        End Set
    End Property

    Public Property Impreso() As Integer
        Get
            Return intImpreso
        End Get
        Set(ByVal value As Integer)
            intImpreso = value
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


    Public Function BuscarXCodigo() As Boolean

        Try
            Dim cmdBuscar As New SqlCommand
            Dim drBuscar As SqlDataReader
            cmdBuscar.CommandType = CommandType.Text
            cmdBuscar.Connection = objConexion2
            cmdBuscar.CommandText = "select C.comp_serie, C.comp_numero, C.comp_fecha, C.clie_destinatario, D.clie_nombre, D.clie_nroIdentificacion, C.comp_direccion, C.tico_codigo, T.tico_descripcion, C.esta_codigo, E.esta_descripcion, C.comp_grt, C.clie_remitente, R.clie_nombre, R.clie_nroIdentificacion, C.empr_codigo, EM.empr_nombre, EM.empr_ruc, C.dest_codigo, DE.dest_nombre, C.comp_manifiesto, C.comp_valorventa, C.comp_igv, C.comp_total, C.comp_impreso, C.usu_codigo, U.usu_nombrecompleto from Comprobante C inner join EVANS.dbo.Cliente D on D.clie_codigo = C.clie_destinatario inner join EVANS.dbo.TipoComprobante T on T.tico_codigo = C.tico_codigo inner join EVANS.dbo.Estado E on E.esta_codigo = C.esta_codigo inner join EVANS.dbo.Cliente R on R.clie_codigo = C.clie_remitente inner join EVANS.dbo.Empresa EM on EM.empr_codigo = C.empr_codigo inner join EVANS.dbo.Destino DE on DE.dest_codigo = C.dest_codigo inner join EVANS.dbo.Usuario U on U.usu_codigo = C.usu_codigo where C.comp_codigo = @comp_codigo"
            cmdBuscar.Parameters.Add("@comp_codigo", SqlDbType.Int).Value = Me.Codigo
            If objConexion2.State = ConnectionState.Closed Then
                objConexion2.Open()
            End If
            drBuscar = cmdBuscar.ExecuteReader()

            While drBuscar.Read()
                Me.Serie = NullToString(drBuscar(0))
                Me.Numero = NullToString(drBuscar(1))
                Me.Fecha = NullToString(drBuscar(2))
                Me.Destinatario.Codigo = NullToString(drBuscar(3))
                Me.Destinatario.Nombre = NullToString(drBuscar(4))
                Me.Destinatario.NumeroID = NullToString(drBuscar(5))
                Me.Direccion = NullToString(drBuscar(6))
                Me.TipoComprobante.Codigo = NullToString(drBuscar(7))
                Me.TipoComprobante.Descripcion = NullToString(drBuscar(8))
                Me.Estado.Codigo = NullToString(drBuscar(9))
                Me.Estado.Descripcion = NullToString(drBuscar(10))
                Me.GuiaRemisionTransportista = NullToString(drBuscar(11))
                Me.Remitente.Codigo = NullToString(drBuscar(12))
                Me.Remitente.Nombre = NullToString(drBuscar(13))
                Me.Remitente.NumeroID = NullToString(drBuscar(14))
                Me.Transportista.Codigo = NullToString(drBuscar(15))
                Me.Transportista.Nombre = NullToString(drBuscar(16))
                Me.Transportista.RUC = NullToString(drBuscar(17))
                Me.Destino.Codigo = NullToString(drBuscar(18))
                Me.Destino.Nombre = NullToString(drBuscar(19))
                Me.Manifiesto = NullToString(drBuscar(20))
                Me.ValorVenta = NullToString(drBuscar(21))
                Me.IGV = NullToString(drBuscar(22))
                Me.Total = NullToString(drBuscar(23))
                Me.Impreso = NullToString(drBuscar(24))
                Me.Usuario.Codigo = NullToString(drBuscar(25))
                Me.Usuario.NombreCompleto = NullToString(drBuscar(26))
            End While
            drBuscar.Close()

            cmdBuscar.CommandText = "Select deco_cantidad, deco_descripcion, deco_preciounitario, deco_flete from DetalleComprobante where comp_codigo = @comp_codigo"
            If objConexion2.State = ConnectionState.Closed Then
                objConexion2.Open()
            End If
            drBuscar = cmdBuscar.ExecuteReader()
            Me.Detalles.Clear()
            While drBuscar.Read()
                Dim item As New clsDetalleComprobante
                item.Cantidad = NullToString(drBuscar("deco_cantidad"))
                item.Descripcion = NullToString(drBuscar("deco_descripcion"))
                item.PrecioUnitario = NullToString(drBuscar("deco_preciounitario"))
                item.Flete = NullToString(drBuscar("deco_flete"))
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

    Public Function BuscarXNroDocumento(ByVal NroDoc As String) As List(Of clsComprobante)

        Try
            Dim cmd As New SqlCommand
            cmd.Connection = objConexion2
            cmd.CommandType = CommandType.Text
            cmd.CommandText = "Select C.comp_codigo, C.comp_serie, C.comp_numero, D.clie_nombre, C.comp_fecha, C.comp_total, TC.tico_descripcion, C.comp_impreso from Comprobante C inner join EVANS.dbo.Cliente D on D.clie_codigo = C.clie_destinatario inner join EVANS.dbo.TipoComprobante TC on TC.tico_codigo = C.tico_codigo where C.comp_numero = @NroDoc"
            cmd.Parameters.Add("@NroDoc", SqlDbType.NVarChar, 6).Value = NroDoc
            If objConexion2.State = ConnectionState.Closed Then
                objConexion2.Open()
            End If
            Dim dr As SqlDataReader
            dr = cmd.ExecuteReader

            Dim lista As New List(Of clsComprobante)
            While dr.Read()
                Dim obj As New clsComprobante
                obj.Codigo = NullToString(dr(0))
                obj.Serie = NullToString(dr(1))
                obj.Numero = NullToString(dr(2))
                obj.Destinatario.Nombre = NullToString(dr(3))
                obj.Fecha = NullToString(dr(4))
                obj.Total = NullToString(dr(5))
                obj.TipoComprobante.Descripcion = NullToString(dr(6))
                obj.Impreso = NullToString(dr(7))
                lista.Add(obj)
            End While
            dr.Close()

            Return lista

        Catch ex As SqlException
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return Nothing
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return Nothing
        Finally
            If objConexion2.State = ConnectionState.Open Then
                objConexion2.Close()
            End If
        End Try

    End Function

    Public Function Grabar(ByVal intTipo As Integer) As Boolean

        Dim cmd As New SqlCommand
        Dim Transaccion As SqlTransaction

        Try
            'GRABAR LA CABECERA DEL COMPROBANTE
            If objConexion2.State = ConnectionState.Closed Then
                objConexion2.Open()
            End If
            Transaccion = objConexion2.BeginTransaction(IsolationLevel.Serializable)
            cmd.CommandType = CommandType.Text
            cmd.Connection = objConexion2
            cmd.Transaction = Transaccion
            If intTipo = 1 Then
                cmd.CommandText = "Update EVANS.dbo.Parametros set para_factnro1 = (Select para_factnro1 + 1 from EVANS.dbo.Parametros); Select para_factnro1 from EVANS.dbo.Parametros;"
            Else
                cmd.CommandText = "Update EVANS.dbo.Parametros set para_bolnro1 = (Select para_bolnro1 + 1 from EVANS.dbo.Parametros); Select para_bolnro1 from EVANS.dbo.Parametros;"
            End If

            Dim intNumeroObtenido As String
            intNumeroObtenido = cmd.ExecuteScalar()
            Me.Numero = intNumeroObtenido.PadLeft(6, "0")
            cmd.CommandText = "Insert into Comprobante(comp_serie, comp_numero, comp_fecha, clie_destinatario, comp_direccion, tico_codigo, esta_codigo, comp_grt, clie_remitente, empr_codigo, dest_codigo, comp_manifiesto, comp_valorventa, comp_igv, comp_total, comp_impreso, usu_codigo) values (@comp_serie, @comp_numero, @comp_fecha, @clie_destinatario, @comp_direccion, @tico_codigo, @esta_codigo, @comp_grt, @clie_remitente, @empr_codigo, @dest_codigo, @comp_manifiesto, @comp_valorventa, @comp_igv, @comp_total, @comp_impreso, @usu_codigo); Select scope_identity();"
            cmd.Parameters.Add("@comp_serie", SqlDbType.NVarChar, 4).Value = Me.Serie
            cmd.Parameters.Add("@comp_numero", SqlDbType.NVarChar, 6).Value = Me.Numero
            cmd.Parameters.Add("@comp_fecha", SqlDbType.DateTime).Value = Me.Fecha
            cmd.Parameters.Add("@clie_destinatario", SqlDbType.Int).Value = Me.Destinatario.Codigo
            cmd.Parameters.Add("@comp_direccion", SqlDbType.NVarChar, 100).Value = Me.Direccion
            cmd.Parameters.Add("@tico_codigo", SqlDbType.Int).Value = Me.TipoComprobante.Codigo
            cmd.Parameters.Add("@esta_codigo", SqlDbType.Int).Value = Me.Estado.Codigo
            cmd.Parameters.Add("@comp_grt", SqlDbType.NVarChar, 20).Value = Me.GuiaRemisionTransportista
            cmd.Parameters.Add("@clie_remitente", SqlDbType.Int).Value = Me.Remitente.Codigo
            cmd.Parameters.Add("@empr_codigo", SqlDbType.Int).Value = Me.Transportista.Codigo
            cmd.Parameters.Add("@dest_codigo", SqlDbType.Int).Value = Me.Destino.Codigo
            cmd.Parameters.Add("@comp_manifiesto", SqlDbType.NVarChar, 10).Value = Me.Manifiesto
            cmd.Parameters.Add("@comp_valorventa", SqlDbType.Float).Value = Me.ValorVenta
            cmd.Parameters.Add("@comp_igv", SqlDbType.Float).Value = Me.IGV
            cmd.Parameters.Add("@comp_total", SqlDbType.Float).Value = Me.Total
            cmd.Parameters.Add("@comp_impreso", SqlDbType.Int).Value = Me.Impreso
            cmd.Parameters.Add("@usu_codigo", SqlDbType.Int).Value = Me.Usuario.Codigo
            Me.Codigo = cmd.ExecuteScalar()

            'GRABAR DETALLES DE COMPROBANTE
            cmd.Parameters.Clear()
            cmd.CommandText = "Insert into DetalleComprobante (comp_codigo, deco_cantidad, deco_descripcion, deco_preciounitario, deco_flete) values (@comp_codigo, @deco_cantidad, @deco_descripcion, @deco_preciounitario, @deco_flete)"
            cmd.Parameters.Add("@comp_codigo", SqlDbType.Int).Value = Me.Codigo
            cmd.Parameters.Add("@deco_cantidad", SqlDbType.Float)
            cmd.Parameters.Add("@deco_descripcion", SqlDbType.NVarChar, 100)
            cmd.Parameters.Add("@deco_preciounitario", SqlDbType.Float)
            cmd.Parameters.Add("@deco_flete", SqlDbType.Float)
            For i As Integer = 0 To Me.Detalles.Count - 1
                cmd.Parameters("@deco_cantidad").Value = Me.Detalles(i).Cantidad
                cmd.Parameters("@deco_descripcion").Value = Me.Detalles(i).Descripcion
                cmd.Parameters("@deco_preciounitario").Value = Me.Detalles(i).PrecioUnitario
                cmd.Parameters("@deco_flete").Value = Me.Detalles(i).Flete
                cmd.ExecuteNonQuery()
            Next

            'SI EL COMPROBANTE ES GENERADO DE LA GUIA, ACTUALIZAR LA GUIA
            If bolGenerandoComprobante = True Then
                cmd.Parameters.Clear()
                cmd.CommandText = "Update GuiaRemision set grem_docventa = @grem_docventa where (grem_serie + '-' + grem_numero) = @grem_nrodoc"
                cmd.Parameters.Add("@grem_docventa", SqlDbType.VarChar, 20).Value = Me.Serie.ToString + "-" + Me.Numero.ToString
                cmd.Parameters.Add("@grem_nrodoc", SqlDbType.VarChar, 20).Value = Me.GuiaRemisionTransportista
                cmd.ExecuteNonQuery()
            End If

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

        Dim cmd As New SqlCommand
        Dim Transaccion As SqlTransaction

        Try
            'ACTUALIZAR LA CABECERA DEL COMPROBANTE
            If objConexion2.State = ConnectionState.Closed Then
                objConexion2.Open()
            End If
            Transaccion = objConexion2.BeginTransaction
            cmd.CommandType = CommandType.Text
            cmd.Connection = objConexion2
            cmd.Transaction = Transaccion
            cmd.CommandText = "Update Comprobante set comp_serie = @comp_serie, comp_numero = @comp_numero, comp_fecha = @comp_fecha, clie_destinatario = @clie_destinatario, comp_direccion = @comp_direccion, tico_codigo = @tico_codigo, esta_codigo = @esta_codigo, comp_grt = @comp_grt, clie_remitente = @clie_remitente, empr_codigo = @empr_codigo, dest_codigo = @dest_codigo, comp_manifiesto = @comp_manifiesto, comp_valorventa = @comp_valorventa, comp_igv = @comp_igv, comp_total = @comp_total, comp_impreso = @comp_impreso, usu_codigo = @usu_codigo where comp_codigo = @comp_codigo"
            cmd.Parameters.Add("@comp_codigo", SqlDbType.Int).Value = Me.Codigo
            cmd.Parameters.Add("@comp_serie", SqlDbType.NVarChar, 4).Value = Me.Serie
            cmd.Parameters.Add("@comp_numero", SqlDbType.NVarChar, 6).Value = Me.Numero
            cmd.Parameters.Add("@comp_fecha", SqlDbType.DateTime).Value = Me.Fecha
            cmd.Parameters.Add("@clie_destinatario", SqlDbType.Int).Value = Me.Destinatario.Codigo
            cmd.Parameters.Add("@comp_direccion", SqlDbType.NVarChar, 100).Value = Me.Direccion
            cmd.Parameters.Add("@tico_codigo", SqlDbType.Int).Value = Me.TipoComprobante.Codigo
            cmd.Parameters.Add("@esta_codigo", SqlDbType.Int).Value = Me.Estado.Codigo
            cmd.Parameters.Add("@comp_grt", SqlDbType.NVarChar, 20).Value = Me.GuiaRemisionTransportista
            cmd.Parameters.Add("@clie_remitente", SqlDbType.Int).Value = Me.Remitente.Codigo
            cmd.Parameters.Add("@empr_codigo", SqlDbType.Int).Value = Me.Transportista.Codigo
            cmd.Parameters.Add("@dest_codigo", SqlDbType.Int).Value = Me.Destino.Codigo
            cmd.Parameters.Add("@comp_manifiesto", SqlDbType.NVarChar, 10).Value = Me.Manifiesto
            cmd.Parameters.Add("@comp_valorventa", SqlDbType.Float).Value = Me.ValorVenta
            cmd.Parameters.Add("@comp_igv", SqlDbType.Float).Value = Me.IGV
            cmd.Parameters.Add("@comp_total", SqlDbType.Float).Value = Me.Total
            cmd.Parameters.Add("@comp_impreso", SqlDbType.Int).Value = Me.Impreso
            cmd.Parameters.Add("@usu_codigo", SqlDbType.Int).Value = Me.Usuario.Codigo
            cmd.ExecuteNonQuery()

            'ACTUALIZAR DETALLES DE COMPROBANTE
            cmd.Parameters.Clear()
            cmd.CommandText = "Delete DetalleComprobante where comp_codigo = @comp_codigo"
            cmd.Parameters.Add("@comp_codigo", SqlDbType.Int).Value = Me.Codigo
            cmd.ExecuteNonQuery()
            cmd.CommandText = "Insert into DetalleComprobante (comp_codigo, deco_cantidad, deco_descripcion, deco_preciounitario, deco_flete) values (@comp_codigo, @deco_cantidad, @deco_descripcion, @deco_preciounitario, @deco_flete)"
            cmd.Parameters.Add("@deco_cantidad", SqlDbType.Float)
            cmd.Parameters.Add("@deco_descripcion", SqlDbType.NVarChar, 100)
            cmd.Parameters.Add("@deco_preciounitario", SqlDbType.Float)
            cmd.Parameters.Add("@deco_flete", SqlDbType.Float)
            For i As Integer = 0 To Me.Detalles.Count - 1
                cmd.Parameters("@deco_cantidad").Value = Me.Detalles(i).Cantidad
                cmd.Parameters("@deco_descripcion").Value = Me.Detalles(i).Descripcion
                cmd.Parameters("@deco_preciounitario").Value = Me.Detalles(i).PrecioUnitario
                cmd.Parameters("@deco_flete").Value = Me.Detalles(i).Flete
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
            cmd.CommandText = "Delete from DetalleComprobante where comp_codigo = @comp_codigo"
            cmd.Parameters.Add("@comp_codigo", SqlDbType.Int).Value = codigo
            cmd.ExecuteNonQuery()
            cmd.CommandText = "Delete from Comprobante where comp_codigo = @comp_codigo"
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
