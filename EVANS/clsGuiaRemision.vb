Imports System.Data.SqlClient

Public Class clsGuiaRemision

    Private intCodigo As Integer
    Private strSerie As String
    Private strNumero As String
    Private datEmision As Date
    Private datTraslado As Date
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
    Private strGRR As String
    Private objDVTipo As New clsTipoComprobante 'tipo de documento de venta (boleta o factura)
    Private strDV As String 'numero documento de venta
    Private objDestino As New clsDestino
    Private objVehiculo As New clsVehiculo
    Private objCarreta As New clsCarreta
    Private objChofer As New clsChofer
    Private objEmpresa As New clsEmpresa
    Private objEstado As New clsEstado
    Private intBultos As Integer
    Private dblPesoTotal As Double
    Private dblCostoTotal As Double
    Private bolImpreso As Integer
    Private strObservacion As String
    Private lstDetalle As New List(Of clsDetalleGuia)
    Private objUsuario As New clsUsuario
    Private intEnviado As Integer
    Private intManifiesto As Integer 'Indica si la guia pertenece a un manifiesto
    Private strNroManifiesto As String
    Private intNroRecepcion As Integer

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

    Public Property FechaEmision() As Date
        Get
            Return datEmision
        End Get
        Set(ByVal value As Date)
            datEmision = value
        End Set
    End Property

    Public Property FechaTraslado() As Date
        Get
            Return datTraslado
        End Get
        Set(ByVal value As Date)
            datTraslado = value
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

    Public Property GuiaRemisionRemitente() As String
        Get
            Return strGRR
        End Get
        Set(ByVal value As String)
            strGRR = value
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

    Public Property Vehiculo() As clsVehiculo
        Get
            Return objVehiculo
        End Get
        Set(ByVal value As clsVehiculo)
            objVehiculo = value
        End Set
    End Property

    Public Property Carreta() As clsCarreta
        Get
            Return objCarreta
        End Get
        Set(ByVal value As clsCarreta)
            objCarreta = value
        End Set
    End Property

    Public Property Chofer() As clsChofer
        Get
            Return objChofer
        End Get
        Set(ByVal value As clsChofer)
            objChofer = value
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

    Public Property Detalles() As List(Of clsDetalleGuia)
        Get
            Return lstDetalle
        End Get
        Set(ByVal value As List(Of clsDetalleGuia))
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

    Public Property Impreso() As Integer
        Get
            Return bolImpreso
        End Get
        Set(ByVal value As Integer)
            bolImpreso = value
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

    Public Property TipoDocumentoVenta() As clsTipoComprobante
        Get
            Return objDVTipo
        End Get
        Set(ByVal value As clsTipoComprobante)
            objDVTipo = value
        End Set
    End Property

    Public Property DocumentoVenta() As String
        Get
            Return strDV
        End Get
        Set(ByVal value As String)
            strDV = value
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

    Public Property Enviado() As Integer
        Get
            Return intEnviado
        End Get
        Set(ByVal value As Integer)
            intEnviado = value
        End Set
    End Property

    Public Property TieneManifiesto() As Integer
        Get
            Return intManifiesto
        End Get
        Set(ByVal value As Integer)
            intManifiesto = value
        End Set
    End Property

    Public Property NroManifiesto() As String
        Get
            Return strNroManifiesto
        End Get
        Set(ByVal value As String)
            strNroManifiesto = value
        End Set
    End Property

    Public Property NroRecepcion() As Integer
        Get
            Return intNroRecepcion
        End Get
        Set(ByVal value As Integer)
            intNroRecepcion = value
        End Set
    End Property

    'FUNCIONES

    Public Function BuscarXCodigo() As Boolean

        Try
            Dim cmdBuscar As New SqlCommand
            Dim drBuscar As SqlDataReader
            cmdBuscar.CommandType = CommandType.Text
            cmdBuscar.Connection = objConexion2
            cmdBuscar.Parameters.Add("@grem_codigo", SqlDbType.Int)
            cmdBuscar.Parameters("@grem_codigo").Value = Me.Codigo
            cmdBuscar.CommandText = "Select grem_enviado, grem_manifiesto from GuiaRemision where grem_codigo = @grem_codigo"
            If objConexion2.State = ConnectionState.Closed Then
                objConexion2.Open()
            End If
            Dim intEnviado, intManifiesto As Integer
            drBuscar = cmdBuscar.ExecuteReader()
            While drBuscar.Read()
                intEnviado = NullToString(drBuscar("grem_enviado"))
                intManifiesto = NullToString(drBuscar("grem_manifiesto"))
            End While
            drBuscar.Close()

            If intEnviado = 0 And intManifiesto = 1 Then
                cmdBuscar.CommandText = "Select G.grem_codigo, G.grem_serie, G.grem_numero, G.clie_remitente, R.clie_nombre, R.clie_nroidentificacion, TR.iden_codigo, TR.iden_descripcion, G.grem_tipodirpartida, G.grem_direccionpartida, G.clie_destinatario, D.clie_nombre, D.clie_nroidentificacion, TD.iden_codigo, TD.iden_descripcion, G.grem_tipodirdestino, G.grem_direcciondestino, G.grem_fechaemision, G.grem_fechatraslado, G.grem_docventa, G.grem_bultos, G.grem_pesototal, G.grem_costototal, G.esta_codigo, E.esta_descripcion, G.grem_impreso, G.dest_codigo, X.dest_nombre, X.dest_distanciavirtual, G.grem_observacion, G.tico_codigo, TC.tico_descripcion, G.usu_codigo, U.usu_nombrecompleto, G.grem_enviado, G.grem_manifiesto, G.grem_nromanifiesto From guiaremision G inner join EVANS.dbo.Cliente R on R.clie_codigo = G.clie_remitente inner join EVANS.dbo.Cliente D on D.clie_codigo = G.clie_destinatario inner join EVANS.dbo.Estado E on E.esta_codigo = G.esta_codigo inner join EVANS.dbo.TipoIdentificacion TR on TR.iden_codigo = R.iden_codigo inner join EVANS.dbo.TipoIdentificacion TD on TD.iden_codigo = D.iden_codigo inner join EVANS.dbo.Destino X on X.dest_codigo = G.dest_codigo inner join EVANS.dbo.TipoComprobante TC on TC.tico_codigo = G.tico_codigo inner join EVANS.dbo.Usuario U on U.usu_codigo = G.usu_codigo where G.grem_codigo = @grem_codigo"
                drBuscar = cmdBuscar.ExecuteReader()

                While drBuscar.Read()
                    Me.Serie = NullToString(drBuscar(1))
                    Me.Numero = NullToString(drBuscar(2))
                    Me.Remitente.Codigo = NullToString(drBuscar(3))
                    Me.Remitente.Nombre = NullToString(drBuscar(4))
                    Me.Remitente.NumeroID = NullToString(drBuscar(5))
                    Me.Remitente.TipoID.Codigo = NullToString(drBuscar(6))
                    Me.Remitente.TipoID.Descripcion = NullToString(drBuscar(7))
                    Me.TipoDireccionPartida = NullToString(drBuscar(8))
                    Me.DireccionPartida = NullToString(drBuscar(9))
                    Me.Destinatario.Codigo = NullToString(drBuscar(10))
                    Me.Destinatario.Nombre = NullToString(drBuscar(11))
                    Me.Destinatario.NumeroID = NullToString(drBuscar(12))
                    Me.Destinatario.TipoID.Codigo = NullToString(drBuscar(13))
                    Me.Destinatario.TipoID.Descripcion = NullToString(drBuscar(14))
                    Me.TipoDireccionDestino = NullToString(drBuscar(15))
                    Me.DireccionDestino = NullToString(drBuscar(16))
                    Me.FechaEmision = NullToString(drBuscar(17))
                    Me.FechaTraslado = NullToString(drBuscar(18))
                    Me.DocumentoVenta = NullToString(drBuscar(19))
                    Me.BultosTotales = NullToString(drBuscar(20))
                    Me.PesoTotal = NullToString(drBuscar(21))
                    Me.CostoTotal = NullToString(drBuscar(22))
                    Me.Estado.Codigo = NullToString(drBuscar(23))
                    Me.Estado.Descripcion = NullToString(drBuscar(24))
                    Me.Impreso = NullToString(drBuscar(25))
                    Me.Destino.Codigo = NullToString(drBuscar(26))
                    Me.Destino.Nombre = NullToString(drBuscar(27))
                    Me.Destino.DistanciaVirtual = NullToString(drBuscar(28))
                    Me.Observacion = NullToString(drBuscar(29))
                    Me.TipoDocumentoVenta.Codigo = NullToString(drBuscar(30))
                    Me.TipoDocumentoVenta.Descripcion = NullToString(drBuscar(31))
                    Me.Usuario.Codigo = NullToString(drBuscar(32))
                    Me.Usuario.NombreCompleto = NullToString(drBuscar(33))
                    Me.Enviado = NullToString(drBuscar(34))
                    Me.TieneManifiesto = NullToString(drBuscar(35))
                    Me.NroManifiesto = NullToString(drBuscar(36))

                End While

            Else
                cmdBuscar.CommandText = "Select G.grem_codigo, G.grem_serie, G.grem_numero, G.clie_remitente, R.clie_nombre, R.clie_nroidentificacion, TR.iden_codigo, TR.iden_descripcion, G.grem_tipodirpartida, G.grem_direccionpartida, G.clie_destinatario, D.clie_nombre, D.clie_nroidentificacion, TD.iden_codigo, TD.iden_descripcion, G.grem_tipodirdestino, G.grem_direcciondestino, G.grem_fechaemision, G.grem_fechatraslado, G.grem_docventa, G.grem_bultos, G.grem_pesototal, G.grem_costototal, G.esta_codigo, E.esta_descripcion, G.grem_impreso, G.dest_codigo, X.dest_nombre, X.dest_distanciavirtual, G.chof_codigo, CH.chof_nombre, CH.chof_licencia, G.vehi_codigo, V.vehi_placa, V.vehi_marca, V.vehi_confvehicular, V.vehi_certinscripcion, S.empr_codigo, S.empr_nombre, S.empr_direccion,S.empr_ruc, G.grem_observacion, G.tico_codigo, TC.tico_descripcion, CA.carr_codigo, CA.carr_placa, G.usu_codigo, U.usu_nombrecompleto, G.grem_enviado, G.grem_manifiesto, G.grem_nromanifiesto From guiaremision G inner join EVANS.dbo.Cliente R on R.clie_codigo = G.clie_remitente inner join EVANS.dbo.Cliente D on D.clie_codigo = G.clie_destinatario inner join EVANS.dbo.Estado E on E.esta_codigo = G.esta_codigo inner join EVANS.dbo.TipoIdentificacion TR on TR.iden_codigo = R.iden_codigo inner join EVANS.dbo.TipoIdentificacion TD on TD.iden_codigo = D.iden_codigo inner join EVANS.dbo.Destino X on X.dest_codigo = G.dest_codigo inner join EVANS.dbo.Chofer CH on CH.chof_codigo = G.chof_codigo inner join EVANS.dbo.Vehiculo V on V.vehi_codigo = G.vehi_codigo inner join EVANS.dbo.Empresa S on S.empr_codigo = G.empr_codigo inner join EVANS.dbo.TipoComprobante TC on TC.tico_codigo = G.tico_codigo inner join EVANS.dbo.Carreta CA on CA.carr_codigo = G.carr_codigo inner join EVANS.dbo.Usuario U on U.usu_codigo = G.usu_codigo where G.grem_codigo = @grem_codigo"
                drBuscar = cmdBuscar.ExecuteReader()

                While drBuscar.Read()
                    Me.Serie = NullToString(drBuscar(1))
                    Me.Numero = NullToString(drBuscar(2))
                    Me.Remitente.Codigo = NullToString(drBuscar(3))
                    Me.Remitente.Nombre = NullToString(drBuscar(4))
                    Me.Remitente.NumeroID = NullToString(drBuscar(5))
                    Me.Remitente.TipoID.Codigo = NullToString(drBuscar(6))
                    Me.Remitente.TipoID.Descripcion = NullToString(drBuscar(7))
                    Me.TipoDireccionPartida = NullToString(drBuscar(8))
                    Me.DireccionPartida = NullToString(drBuscar(9))
                    Me.Destinatario.Codigo = NullToString(drBuscar(10))
                    Me.Destinatario.Nombre = NullToString(drBuscar(11))
                    Me.Destinatario.NumeroID = NullToString(drBuscar(12))
                    Me.Destinatario.TipoID.Codigo = NullToString(drBuscar(13))
                    Me.Destinatario.TipoID.Descripcion = NullToString(drBuscar(14))
                    Me.TipoDireccionDestino = NullToString(drBuscar(15))
                    Me.DireccionDestino = NullToString(drBuscar(16))
                    Me.FechaEmision = NullToString(drBuscar(17))
                    Me.FechaTraslado = NullToString(drBuscar(18))
                    Me.DocumentoVenta = NullToString(drBuscar(19))
                    Me.BultosTotales = NullToString(drBuscar(20))
                    Me.PesoTotal = NullToString(drBuscar(21))
                    Me.CostoTotal = NullToString(drBuscar(22))
                    Me.Estado.Codigo = NullToString(drBuscar(23))
                    Me.Estado.Descripcion = NullToString(drBuscar(24))
                    Me.Impreso = NullToString(drBuscar(25))
                    Me.Destino.Codigo = NullToString(drBuscar(26))
                    Me.Destino.Nombre = NullToString(drBuscar(27))
                    Me.Destino.DistanciaVirtual = NullToString(drBuscar(28))
                    Me.Chofer.Codigo = NullToString(drBuscar(29))
                    Me.Chofer.Nombre = NullToString(drBuscar(30))
                    Me.Chofer.Licencia = NullToString(drBuscar(31))
                    Me.Vehiculo.Codigo = NullToString(drBuscar(32))
                    Me.Vehiculo.Placa = NullToString(drBuscar(33))
                    Me.Vehiculo.Marca = NullToString(drBuscar(34))
                    Me.Vehiculo.ConfiguracionVehicular = NullToString(drBuscar(35))
                    Me.Vehiculo.CertificadoInscripcion = NullToString(drBuscar(36))
                    Me.Empresa.Codigo = NullToString(drBuscar(37))
                    Me.Empresa.Nombre = NullToString(drBuscar(38))
                    Me.Empresa.Direccion = NullToString(drBuscar(39))
                    Me.Empresa.RUC = NullToString(drBuscar(40))
                    Me.Observacion = NullToString(drBuscar(41))
                    Me.TipoDocumentoVenta.Codigo = NullToString(drBuscar(42))
                    Me.TipoDocumentoVenta.Descripcion = NullToString(drBuscar(43))
                    Me.Carreta.Codigo = NullToString(drBuscar(44))
                    Me.Carreta.Placa = NullToString(drBuscar(45))
                    Me.Usuario.Codigo = NullToString(drBuscar(46))
                    Me.Usuario.NombreCompleto = NullToString(drBuscar(47))
                    Me.Enviado = NullToString(drBuscar(48))
                    Me.TieneManifiesto = NullToString(drBuscar(49))
                    Me.NroManifiesto = NullToString(drBuscar(50))
                    
                End While
            End If
            
            drBuscar.Close()
            cmdBuscar.CommandText = "Select degr_cantidad, degr_descripcion, degr_peso, degr_unidad, degr_costo, degr_tipodoc, degr_nrodoc from DetalleGuia where grem_codigo = @grem_codigo"

            If objConexion2.State = ConnectionState.Closed Then
                objConexion2.Open()
            End If

            drBuscar = cmdBuscar.ExecuteReader()

            Me.Detalles.Clear()
            While drBuscar.Read
                Dim item As New clsDetalleGuia
                item.Cantidad = NullToString(drBuscar("degr_cantidad"))
                item.Descripcion = NullToString(drBuscar("degr_descripcion"))
                item.Peso = NullToString(drBuscar("degr_peso"))
                item.Unidad = NullToString(drBuscar("degr_unidad"))
                item.Costo = NullToString(drBuscar("degr_costo"))
                item.TipoDocumento = NullToString(drBuscar("degr_tipodoc"))
                item.NumeroDocumento = NullToString(drBuscar("degr_nrodoc"))
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

    ''' <summary>
    ''' Busca una Guia de Remision por numero de documento
    ''' </summary>
    ''' <param name="NroDoc">Numero de la guia a buscar</param>
    ''' <returns>"clsGuiaRemision"</returns>
    ''' <remarks></remarks>
    Public Function BuscarXNroDocumento(ByVal NroDoc As String) As List(Of clsGuiaRemision)

        Try
            Dim cmd As New SqlCommand
            cmd.Connection = objConexion2
            cmd.CommandType = CommandType.Text
            cmd.CommandText = "Select G.grem_codigo, G.grem_serie, G.grem_numero, R.clie_nombre, D.clie_nombre, G.grem_fechaemision, G.grem_fechatraslado, G.grem_docventa, G.grem_costototal, G.grem_enviado, G.grem_impreso from guiaremision G inner join [EVANS].dbo.Cliente R on R.clie_codigo = G.clie_remitente inner join [EVANS].dbo.Cliente D on D.clie_codigo = G.clie_destinatario where G.grem_numero = @NroDoc"
            cmd.Parameters.Add("@NroDoc", SqlDbType.NVarChar, 6).Value = NroDoc
            If objConexion2.State = ConnectionState.Closed Then
                objConexion2.Open()
            End If
            Dim dr As SqlDataReader
            dr = cmd.ExecuteReader

            Dim lista As New List(Of clsGuiaRemision)
            While dr.Read()
                Dim objGuia As New clsGuiaRemision
                objGuia.Codigo = NullToString(dr(0))
                objGuia.Serie = NullToString(dr(1))
                objGuia.Numero = NullToString(dr(2))
                objGuia.Remitente.Nombre = NullToString(dr(3))
                objGuia.Destinatario.Nombre = NullToString(dr(4))
                objGuia.FechaEmision = NullToString(dr(5))
                objGuia.FechaTraslado = NullToString(dr(6))
                objGuia.DocumentoVenta = NullToString(dr(7))
                objGuia.CostoTotal = NullToString(dr(8))
                objGuia.Enviado = NullToString(dr(9))
                objGuia.Impreso = NullToString(dr(10))
                lista.Add(objGuia)
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

    ''' <summary>
    ''' Graba una nueva Guia de Remision
    ''' </summary>
    ''' <param name="CboObjeto">Nombre de Combo</param>
    ''' <param name="Display">Nombre de la columna q se va a mostrar</param>
    ''' <param name="Value">Nombre de la columna q se va a tener el value</param>
    ''' <returns>"Boolean"</returns>
    ''' <remarks></remarks>
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
            cmd.CommandText = "Update EVANS.dbo.Parametros set para_gremnro1 = (Select para_gremnro1 + 1 from EVANS.dbo.Parametros); Select para_gremnro1 from EVANS.dbo.Parametros;"
            Dim numero As String
            numero = cmd.ExecuteScalar()
            'MsgBox("Dentro")
            cmd.CommandText = "Insert into GuiaRemision (grem_serie, grem_numero, grem_fechaemision, grem_fechatraslado, clie_remitente, grem_tipodirpartida, grem_direccionpartida, clie_destinatario, grem_tipodirdestino, grem_direcciondestino, dest_codigo, vehi_codigo, carr_codigo, chof_codigo, empr_codigo, esta_codigo, grem_bultos, grem_pesototal, grem_costototal, grem_impreso, tico_codigo, grem_docventa, grem_observacion, usu_codigo, grem_enviado, grem_manifiesto) values (@grem_serie, @grem_numero, @grem_fechaemision, @grem_fechatraslado, @clie_remitente, @grem_tipodirpartida, @grem_direccionpartida, @clie_destinatario, @grem_tipodirdestino, @grem_direcciondestino, @dest_codigo, @vehi_codigo, @carr_codigo, @chof_codigo, @empr_codigo, @esta_codigo, @grem_bultos, @grem_pesototal, @grem_costototal, @grem_impreso, @tico_codigo, @grem_docventa, @grem_observacion, @usu_codigo, @grem_enviado, @grem_manifiesto); Select SCOPE_IDENTITY();"
            cmd.Parameters.Add("@grem_serie", SqlDbType.NVarChar, 4).Value = Me.Serie
            cmd.Parameters.Add("@grem_numero", SqlDbType.NVarChar, 6).Value = numero.PadLeft(6, "0")
            cmd.Parameters.Add("@grem_fechaemision", SqlDbType.DateTime).Value = Me.FechaEmision
            cmd.Parameters.Add("@grem_fechatraslado", SqlDbType.DateTime).Value = Me.FechaTraslado
            cmd.Parameters.Add("@clie_remitente", SqlDbType.Int).Value = Me.Remitente.Codigo
            cmd.Parameters.Add("@grem_tipodirpartida", SqlDbType.Int).Value = Me.TipoDireccionPartida
            cmd.Parameters.Add("@grem_direccionpartida", SqlDbType.NVarChar, 100).Value = Me.DireccionPartida
            cmd.Parameters.Add("@clie_destinatario", SqlDbType.Int).Value = Me.Destinatario.Codigo
            cmd.Parameters.Add("@grem_tipodirdestino", SqlDbType.Int).Value = Me.TipoDireccionDestino
            cmd.Parameters.Add("@grem_direcciondestino", SqlDbType.NVarChar, 100).Value = Me.DireccionDestino
            cmd.Parameters.Add("@dest_codigo", SqlDbType.Int).Value = Me.Destino.Codigo
            cmd.Parameters.Add("@vehi_codigo", SqlDbType.Int).Value = Me.Vehiculo.Codigo
            cmd.Parameters.Add("@carr_codigo", SqlDbType.Int).Value = Me.Carreta.Codigo
            cmd.Parameters.Add("@chof_codigo", SqlDbType.Int).Value = Me.Chofer.Codigo
            cmd.Parameters.Add("@empr_codigo", SqlDbType.Int).Value = Me.Empresa.Codigo
            cmd.Parameters.Add("@esta_codigo", SqlDbType.Int).Value = Me.Estado.Codigo
            cmd.Parameters.Add("@grem_bultos", SqlDbType.Int).Value = Me.BultosTotales
            cmd.Parameters.Add("@grem_pesototal", SqlDbType.Float).Value = Me.PesoTotal
            cmd.Parameters.Add("@grem_costototal", SqlDbType.Float).Value = Me.CostoTotal
            cmd.Parameters.Add("@grem_impreso", SqlDbType.Int).Value = Me.Impreso
            cmd.Parameters.Add("@tico_codigo", SqlDbType.Int).Value = Me.TipoDocumentoVenta.Codigo
            cmd.Parameters.Add("@grem_docventa", SqlDbType.NVarChar, 20).Value = Me.DocumentoVenta
            cmd.Parameters.Add("@grem_observacion", SqlDbType.NVarChar, 250).Value = Me.Observacion
            cmd.Parameters.Add("@usu_codigo", SqlDbType.Int).Value = Me.Usuario.Codigo
            cmd.Parameters.Add("@grem_enviado", SqlDbType.Int).Value = Me.Enviado
            cmd.Parameters.Add("@grem_manifiesto", SqlDbType.Int).Value = Me.TieneManifiesto

            Dim i, IDGuia As Integer
            IDGuia = cmd.ExecuteScalar()

            'GRABAR DETALLES DE GUIA
            cmd.Parameters.Clear()
            cmd.Parameters.Add("@grem_codigo", SqlDbType.Int)
            cmd.Parameters.Add("@degr_cantidad", SqlDbType.Float)
            cmd.Parameters.Add("@degr_descripcion", SqlDbType.NVarChar, 100)
            cmd.Parameters.Add("@degr_peso", SqlDbType.Float)
            cmd.Parameters.Add("@degr_unidad", SqlDbType.NVarChar, 30)
            cmd.Parameters.Add("@degr_costo", SqlDbType.Float)
            cmd.Parameters.Add("@degr_tipodoc", SqlDbType.VarChar, 20)
            cmd.Parameters.Add("@degr_nrodoc", SqlDbType.VarChar, 20)

            cmd.CommandText = "Insert into DetalleGuia (grem_codigo, degr_cantidad, degr_descripcion, degr_peso, degr_unidad, degr_costo, degr_tipodoc, degr_nrodoc) VALUES (@grem_codigo, @degr_cantidad, @degr_descripcion, @degr_peso, @degr_unidad, @degr_costo, @degr_tipodoc, @degr_nrodoc)"
            For i = 0 To Me.Detalles.Count - 1
                cmd.Parameters("@grem_codigo").Value = IDGuia
                cmd.Parameters("@degr_cantidad").Value = Me.Detalles(i).Cantidad
                cmd.Parameters("@degr_descripcion").Value = Me.Detalles(i).Descripcion
                cmd.Parameters("@degr_peso").Value = Me.Detalles(i).Peso
                cmd.Parameters("@degr_unidad").Value = Me.Detalles(i).Unidad
                cmd.Parameters("@degr_costo").Value = Me.Detalles(i).Costo
                cmd.Parameters("@degr_tipodoc").Value = Me.Detalles(i).TipoDocumento
                cmd.Parameters("@degr_nrodoc").Value = Me.Detalles(i).NumeroDocumento
                If objConexion2.State = ConnectionState.Closed Then
                    objConexion2.Open()
                End If
                cmd.ExecuteNonQuery()
            Next

            'SI LA GUIA ES GENERADA DE LA RECEPCION, ACTUALIZAR LA RECEPCION
            If bolGenerandoGuia = True Then
                cmd.Parameters.Clear()
                cmd.CommandText = "Update Recepcion set rece_guiaremision = @rece_guiaremision where rece_codigo = @rece_codigo"
                cmd.Parameters.Add("@rece_codigo", SqlDbType.Int).Value = Me.NroRecepcion
                cmd.Parameters.Add("@rece_guiaremision", SqlDbType.NVarChar, 20).Value = Me.Serie + "-" + numero.PadLeft(6, "0")
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
            cmd.CommandText = "Update GuiaRemision set grem_serie = @grem_serie, grem_numero = @grem_numero, grem_fechaemision = @grem_fechaemision, grem_fechatraslado = @grem_fechatraslado, clie_remitente = @clie_remitente, grem_tipodirpartida = @grem_tipodirpartida, grem_direccionpartida = @grem_direccionpartida, clie_destinatario = @clie_destinatario, grem_tipodirdestino = @grem_tipodirdestino, grem_direcciondestino = @grem_direcciondestino, dest_codigo = @dest_codigo, vehi_codigo = @vehi_codigo, carr_codigo = @carr_codigo, chof_codigo = @chof_codigo, empr_codigo = @empr_codigo, esta_codigo = @esta_codigo, grem_bultos = @grem_bultos, grem_pesototal = @grem_pesototal, grem_costototal = @grem_costototal, grem_impreso = @grem_impreso, tico_codigo = @tico_codigo, grem_docventa = @grem_docventa, grem_observacion = @grem_observacion, usu_codigo = @usu_codigo, grem_enviado = @grem_enviado where grem_codigo = @grem_codigo"
            cmd.Parameters.Add("@grem_codigo", SqlDbType.Int)
            cmd.Parameters.Add("@grem_serie", SqlDbType.NVarChar, 4)
            cmd.Parameters.Add("@grem_numero", SqlDbType.NVarChar, 6)
            cmd.Parameters.Add("@grem_fechaemision", SqlDbType.DateTime)
            cmd.Parameters.Add("@grem_fechatraslado", SqlDbType.DateTime)
            cmd.Parameters.Add("@clie_remitente", SqlDbType.Int)
            cmd.Parameters.Add("@grem_tipodirpartida", SqlDbType.Int)
            cmd.Parameters.Add("@grem_direccionpartida", SqlDbType.NVarChar, 100)
            cmd.Parameters.Add("@clie_destinatario", SqlDbType.Int)
            cmd.Parameters.Add("@grem_tipodirdestino", SqlDbType.Int)
            cmd.Parameters.Add("@grem_direcciondestino", SqlDbType.NVarChar, 100)
            cmd.Parameters.Add("@dest_codigo", SqlDbType.Int)
            cmd.Parameters.Add("@vehi_codigo", SqlDbType.Int)
            cmd.Parameters.Add("@carr_codigo", SqlDbType.Int)
            cmd.Parameters.Add("@chof_codigo", SqlDbType.Int)
            cmd.Parameters.Add("@empr_codigo", SqlDbType.Int)
            cmd.Parameters.Add("@esta_codigo", SqlDbType.Int)
            cmd.Parameters.Add("@grem_bultos", SqlDbType.Int)
            cmd.Parameters.Add("@grem_pesototal", SqlDbType.Float)
            cmd.Parameters.Add("@grem_costototal", SqlDbType.Float)
            cmd.Parameters.Add("@grem_impreso", SqlDbType.Int)
            cmd.Parameters.Add("@tico_codigo", SqlDbType.Int)
            cmd.Parameters.Add("@grem_docventa", SqlDbType.NVarChar, 20)
            cmd.Parameters.Add("@grem_observacion", SqlDbType.NVarChar, 250)
            cmd.Parameters.Add("@usu_codigo", SqlDbType.Int)
            cmd.Parameters.Add("@grem_enviado", SqlDbType.Int)

            cmd.Parameters("@grem_codigo").Value = Me.Codigo
            cmd.Parameters("@grem_serie").Value = Me.Serie
            cmd.Parameters("@grem_numero").Value = Me.Numero
            cmd.Parameters("@grem_fechaemision").Value = Me.FechaEmision
            cmd.Parameters("@grem_fechatraslado").Value = Me.FechaTraslado
            cmd.Parameters("@clie_remitente").Value = Me.Remitente.Codigo
            cmd.Parameters("@grem_direccionpartida").Value = Me.DireccionPartida
            cmd.Parameters("@grem_tipodirpartida").Value = Me.TipoDireccionPartida
            cmd.Parameters("@clie_destinatario").Value = Me.Destinatario.Codigo
            cmd.Parameters("@grem_tipodirdestino").Value = Me.TipoDireccionDestino
            cmd.Parameters("@grem_direcciondestino").Value = Me.DireccionDestino
            cmd.Parameters("@dest_codigo").Value = Me.Destino.Codigo
            cmd.Parameters("@vehi_codigo").Value = Me.Vehiculo.Codigo
            cmd.Parameters("@carr_codigo").Value = Me.Carreta.Codigo
            cmd.Parameters("@chof_codigo").Value = Me.Chofer.Codigo
            cmd.Parameters("@empr_codigo").Value = Me.Empresa.Codigo
            cmd.Parameters("@esta_codigo").Value = Me.Estado.Codigo
            cmd.Parameters("@grem_bultos").Value = Me.BultosTotales
            cmd.Parameters("@grem_pesototal").Value = Me.PesoTotal
            cmd.Parameters("@grem_costototal").Value = Me.CostoTotal
            cmd.Parameters("@grem_impreso").Value = Me.Impreso
            cmd.Parameters("@tico_codigo").Value = Me.TipoDocumentoVenta.Codigo
            cmd.Parameters("@grem_docventa").Value = Me.DocumentoVenta
            cmd.Parameters("@grem_observacion").Value = Me.Observacion
            cmd.Parameters("@usu_codigo").Value = Me.Usuario.Codigo
            cmd.Parameters("@grem_enviado").Value = Me.Enviado
            cmd.ExecuteNonQuery()

            'ACTUALIZAR DETALLE DE GUIA
            cmd.CommandText = "Delete from DetalleGuia where grem_codigo = @grem_codigo;"
            cmd.ExecuteNonQuery()

            cmd.Parameters.Clear()
            cmd.CommandText = "Insert into DetalleGuia (grem_codigo, degr_cantidad, degr_descripcion, degr_peso, degr_unidad, degr_costo, degr_tipodoc, degr_nrodoc) VALUES (@grem_codigo, @degr_cantidad, @degr_descripcion, @degr_peso, @degr_unidad, @degr_costo, @degr_tipodoc, @degr_nrodoc)"
            cmd.Parameters.Add("@grem_codigo", SqlDbType.Int)
            cmd.Parameters.Add("@degr_cantidad", SqlDbType.Float)
            cmd.Parameters.Add("@degr_descripcion", SqlDbType.NVarChar, 100)
            cmd.Parameters.Add("@degr_peso", SqlDbType.Float)
            cmd.Parameters.Add("@degr_unidad", SqlDbType.NVarChar, 30)
            cmd.Parameters.Add("@degr_costo", SqlDbType.Float)
            cmd.Parameters.Add("@degr_tipodoc", SqlDbType.VarChar, 20)
            cmd.Parameters.Add("@degr_nrodoc", SqlDbType.VarChar, 20)

            For i = 0 To Me.Detalles.Count - 1
                cmd.Parameters("@grem_codigo").Value = Me.Codigo
                cmd.Parameters("@degr_cantidad").Value = Me.Detalles(i).Cantidad
                cmd.Parameters("@degr_descripcion").Value = Me.Detalles(i).Descripcion
                cmd.Parameters("@degr_peso").Value = Me.Detalles(i).Peso
                cmd.Parameters("@degr_unidad").Value = Me.Detalles(i).Unidad
                cmd.Parameters("@degr_costo").Value = Me.Detalles(i).Costo
                cmd.Parameters("@degr_tipodoc").Value = Me.Detalles(i).TipoDocumento
                cmd.Parameters("@degr_nrodoc").Value = Me.Detalles(i).NumeroDocumento
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
            cmd.CommandText = "Delete from DetalleGuia where grem_codigo = @grem_codigo"
            cmd.Parameters.Add("@grem_codigo", SqlDbType.Int).Value = codigo
            cmd.ExecuteNonQuery()
            cmd.CommandText = "Delete from GuiaRemision where grem_codigo = @grem_codigo"
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
