Imports System.Data.SqlClient

Public Class clsManifiesto

    Private intCodigo As Integer
    Private strNumero As String
    Private datFecha As Date
    Private objTransportista As New clsEmpresa
    Private objVehiculo As New clsVehiculo
    Private objCarreta As New clsCarreta
    Private objChofer As New clsChofer
    Private dblImporte As Double
    Private intNroGuias As Integer
    Private dblPeso As Double
    Private objEstado As New clsEstado
    Private objUsuario As New clsUsuario
    Private lstSeleccionadas As New List(Of clsGuiaRemision)
    Private lstDisponibles As New List(Of Integer)

    Public Property Codigo() As Integer
        Get
            Return intCodigo
        End Get
        Set(ByVal value As Integer)
            intCodigo = value
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

    Public Property Transportista() As clsEmpresa
        Get
            Return objTransportista
        End Get
        Set(ByVal value As clsEmpresa)
            objTransportista = value
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

    Public Property Importe() As Double
        Get
            Return dblImporte
        End Get
        Set(ByVal value As Double)
            dblImporte = value
        End Set
    End Property

    Public Property NroGuias() As Integer
        Get
            Return intNroGuias
        End Get
        Set(ByVal value As Integer)
            intNroGuias = value
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

    Public Property Estado() As clsEstado
        Get
            Return objEstado
        End Get
        Set(ByVal value As clsEstado)
            objEstado = value
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

    Public Property GuiasSeleccionadas() As List(Of clsGuiaRemision)
        Get
            Return lstSeleccionadas
        End Get
        Set(ByVal value As List(Of clsGuiaRemision))
            lstSeleccionadas = value
        End Set
    End Property

    Public Property GuiasDisponibles() As List(Of Integer)
        Get
            Return lstDisponibles
        End Get
        Set(ByVal value As List(Of Integer))
            lstDisponibles = value
        End Set
    End Property


    'FUNCIONES

    Public Function BuscarXCodigo() As Boolean

        Try
            Dim cmd As New SqlCommand
            Dim dr As SqlDataReader
            cmd.CommandType = CommandType.Text
            cmd.Connection = objConexion2
            cmd.CommandText = "Select M.mani_codigo, M.mani_numero, M.empr_codigo, EM.empr_nombre, EM.empr_ruc, EM.empr_direccion, M.vehi_codigo, V.vehi_placa, M.carr_codigo, C.carr_placa, M.chof_codigo, CH.chof_nombre, CH.chof_licencia, M.mani_importe, M.mani_nroguias, M.mani_peso, M.esta_codigo, E.esta_descripcion, M.mani_fecha, M.usu_codigo, U.usu_nombrecompleto from Manifiesto M inner join EVANS.dbo.Empresa EM on Em.empr_codigo = M.empr_codigo inner join EVANS.dbo.Vehiculo V on V.vehi_codigo = M.vehi_codigo inner join EVANS.dbo.Carreta C on C.carr_codigo = M.carr_codigo inner join EVANS.dbo.Chofer CH on CH.chof_codigo = M.chof_codigo inner join EVANS.dbo.Estado E on E.esta_codigo = M.esta_codigo inner join EVANS.dbo.Usuario U on U.usu_codigo = M.usu_codigo where M.mani_codigo = @mani_codigo"
            cmd.Parameters.Add("@mani_codigo", SqlDbType.Int).Value = Me.Codigo
            If objConexion2.State = ConnectionState.Closed Then
                objConexion2.Open()
            End If
            dr = cmd.ExecuteReader()

            While dr.Read()
                Me.Codigo = NullToString(dr(0))
                Me.Numero = NullToString(dr(1))
                Me.Transportista.Codigo = NullToString(dr(2))
                Me.Transportista.Nombre = NullToString(dr(3))
                Me.Transportista.RUC = NullToString(dr(4))
                Me.Transportista.Direccion = NullToString(dr(5))
                Me.Vehiculo.Codigo = NullToString(dr(6))
                Me.Vehiculo.Placa = NullToString(dr(7))
                Me.Carreta.Codigo = NullToString(dr(8))
                Me.Carreta.Placa = NullToString(dr(9))
                Me.Chofer.Codigo = NullToString(dr(10))
                Me.Chofer.Nombre = NullToString(dr(11))
                Me.Chofer.Licencia = NullToString(dr(12))
                Me.Importe = NullToString(dr(13))
                Me.NroGuias = NullToString(dr(14))
                Me.Peso = NullToString(dr(15))
                Me.Estado.Codigo = NullToString(dr(16))
                Me.Estado.Descripcion = NullToString(dr(17))
                Me.Fecha = NullToString(dr(18))
                Me.Usuario.Codigo = NullToString(dr(19))
                Me.Usuario.NombreCompleto = NullToString(dr(20))
            End While
            dr.Close()

            cmd.CommandText = "Select G.grem_codigo, G.grem_serie, G.grem_numero, G.grem_fechaemision, G.dest_codigo, D.dest_nombre, G.grem_pesototal, G.grem_costototal from GuiaRemision G inner join DetalleManifiesto DM on G.grem_codigo = DM.grem_codigo inner join EVANS.dbo.Destino D on D.dest_codigo = G.dest_codigo where DM.mani_codigo = @mani_codigo"
            dr = cmd.ExecuteReader()

            Me.GuiasSeleccionadas.Clear()
            While dr.Read()
                Dim item As New clsGuiaRemision
                item.Codigo = NullToString(dr(0))
                item.Serie = NullToString(dr(1))
                item.Numero = NullToString(dr(2))
                item.FechaEmision = NullToString(dr(3))
                item.Destino.Codigo = NullToString(dr(4))
                item.Destino.Nombre = NullToString(dr(5))
                item.PesoTotal = NullToString(dr(6))
                item.CostoTotal = NullToString(dr(7))
                Me.GuiasSeleccionadas.Add(item)
            End While
            dr.Close()

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
            cmd.CommandText = "Update EVANS.dbo.Parametros set para_manifiesto = (Select para_manifiesto + 1 from EVANS.dbo.Parametros); Select para_manifiesto from EVANS.dbo.Parametros;"
            Dim numero As String
            numero = cmd.ExecuteScalar()
            cmd.CommandText = "Insert into Manifiesto (mani_numero, mani_fecha, empr_codigo, vehi_codigo, carr_codigo, chof_codigo, mani_importe, mani_nroguias, mani_peso, esta_codigo, usu_codigo) values (@mani_numero, @mani_fecha, @empr_codigo, @vehi_codigo, @carr_codigo, @chof_codigo, @mani_importe, @mani_nroguias, @mani_peso, @esta_codigo, @usu_codigo); Select SCOPE_IDENTITY();"
            cmd.Parameters.Add("@mani_numero", SqlDbType.VarChar, 20).Value = Date.Today.Year.ToString + "-" + numero.ToString
            Me.Numero = Date.Today.Year.ToString + "-" + numero.ToString
            cmd.Parameters.Add("@mani_fecha", SqlDbType.DateTime).Value = Me.Fecha
            cmd.Parameters.Add("@empr_codigo", SqlDbType.Int).Value = Me.Transportista.Codigo
            cmd.Parameters.Add("@vehi_codigo", SqlDbType.Int).Value = Me.Vehiculo.Codigo
            cmd.Parameters.Add("@carr_codigo", SqlDbType.Int).Value = Me.Carreta.Codigo
            cmd.Parameters.Add("@chof_codigo", SqlDbType.Int).Value = Me.Chofer.Codigo
            cmd.Parameters.Add("@mani_importe", SqlDbType.Float).Value = Me.Importe
            cmd.Parameters.Add("@mani_nroguias", SqlDbType.Int).Value = Me.NroGuias
            cmd.Parameters.Add("@mani_peso", SqlDbType.Float).Value = Me.Peso
            cmd.Parameters.Add("@esta_codigo", SqlDbType.Int).Value = Me.Estado.Codigo
            cmd.Parameters.Add("@usu_codigo", SqlDbType.Int).Value = Me.Usuario.Codigo

            Dim i, intCodigo As Integer
            intCodigo = cmd.ExecuteScalar()

            'GRABAR DETALLES DE MANIFIESTO
            cmd.Parameters.Clear()
            cmd.Parameters.Add("@mani_codigo", SqlDbType.Int).Value = intCodigo
            cmd.Parameters.Add("@grem_codigo", SqlDbType.Int)
            cmd.CommandText = "Insert into DetalleManifiesto (mani_codigo, grem_codigo) values (@mani_codigo, @grem_codigo)"

            For i = 0 To Me.GuiasSeleccionadas.Count - 1
                cmd.Parameters("@grem_codigo").Value = Me.GuiasSeleccionadas(i).Codigo
                cmd.ExecuteNonQuery()
            Next

            'Marcar guias como seleccionadas para manifiesto y actualizar sus datos
            cmd.Parameters.Clear()
            cmd.CommandText = "Update GuiaRemision set grem_fechatraslado = @grem_fechatraslado, grem_enviado = 1, empr_codigo = @empr_codigo, chof_codigo = @chof_codigo, vehi_codigo = @vehi_codigo, carr_codigo = @carr_codigo, grem_manifiesto = 1, grem_nromanifiesto = @grem_nromanifiesto where grem_codigo = @grem_codigo"
            cmd.Parameters.Add("@grem_codigo", SqlDbType.Int)
            cmd.Parameters.Add("@grem_fechatraslado", SqlDbType.DateTime).Value = Me.Fecha
            cmd.Parameters.Add("@empr_codigo", SqlDbType.Int).Value = Me.Transportista.Codigo
            cmd.Parameters.Add("@chof_codigo", SqlDbType.Int).Value = Me.Chofer.Codigo
            cmd.Parameters.Add("@vehi_codigo", SqlDbType.Int).Value = Me.Vehiculo.Codigo
            cmd.Parameters.Add("@carr_codigo", SqlDbType.Int).Value = Me.Carreta.Codigo
            cmd.Parameters.Add("@grem_nromanifiesto", SqlDbType.VarChar, 15).Value = Me.Numero
            For i = 0 To Me.GuiasSeleccionadas.Count - 1
                cmd.Parameters("@grem_codigo").Value = Me.GuiasSeleccionadas(i).Codigo
                cmd.ExecuteNonQuery()
            Next

            'Marcar guias como disponibles para manifiesto
            cmd.CommandText = "Update GuiaRemision set grem_fechatraslado = grem_fechaemision, grem_enviado = 0, grem_nromanifiesto = '' where grem_codigo = @grem_codigo"
            For i = 0 To Me.GuiasDisponibles.Count - 1
                cmd.Parameters("@grem_codigo").Value = Me.GuiasDisponibles(i)
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
            Transaccion = objConexion2.BeginTransaction(IsolationLevel.Serializable)
            Dim cmd As New SqlCommand
            cmd.CommandType = CommandType.Text
            cmd.Connection = objConexion2
            cmd.Transaction = Transaccion
            cmd.CommandText = "Update Manifiesto set mani_fecha = @mani_fecha, empr_codigo = @empr_codigo, vehi_codigo = @vehi_codigo, carr_codigo = @carr_codigo, chof_codigo = @chof_codigo, mani_importe = @mani_importe, mani_nroguias = @mani_nroguias, mani_peso = @mani_peso, esta_codigo = @esta_codigo, usu_codigo = @usu_codigo where mani_codigo = @mani_codigo"
            cmd.Parameters.Add("@mani_codigo", SqlDbType.Int).Value = Me.Codigo
            cmd.Parameters.Add("@mani_fecha", SqlDbType.DateTime).Value = Me.Fecha
            cmd.Parameters.Add("@empr_codigo", SqlDbType.Int).Value = Me.Transportista.Codigo
            cmd.Parameters.Add("@vehi_codigo", SqlDbType.Int).Value = Me.Vehiculo.Codigo
            cmd.Parameters.Add("@carr_codigo", SqlDbType.Int).Value = Me.Carreta.Codigo
            cmd.Parameters.Add("@chof_codigo", SqlDbType.Int).Value = Me.Chofer.Codigo
            cmd.Parameters.Add("@mani_importe", SqlDbType.Float).Value = Me.Importe
            cmd.Parameters.Add("@mani_nroguias", SqlDbType.Int).Value = Me.NroGuias
            cmd.Parameters.Add("@mani_peso", SqlDbType.Float).Value = Me.Peso
            cmd.Parameters.Add("@esta_codigo", SqlDbType.Int).Value = Me.Estado.Codigo
            cmd.Parameters.Add("@usu_codigo", SqlDbType.Int).Value = Me.Usuario.Codigo
            cmd.ExecuteNonQuery()

            'ACTUALIZAR DETALLES DE MANIFIESTO
            cmd.CommandText = "Delete from DetalleManifiesto where mani_codigo = @mani_codigo"
            cmd.ExecuteNonQuery()

            cmd.Parameters.Clear()
            cmd.Parameters.Add("@mani_codigo", SqlDbType.Int).Value = intCodigo
            cmd.Parameters.Add("@grem_codigo", SqlDbType.Int)
            cmd.CommandText = "Insert into DetalleManifiesto (mani_codigo, grem_codigo) values (@mani_codigo, @grem_codigo)"

            For i = 0 To Me.GuiasSeleccionadas.Count - 1
                cmd.Parameters("@grem_codigo").Value = Me.GuiasSeleccionadas(i).Codigo
                cmd.ExecuteNonQuery()
            Next

            'Marcar guias como seleccionadas para manifiesto y actualizar sus datos
            cmd.Parameters.Clear()
            cmd.CommandText = "Update GuiaRemision set grem_fechatraslado = @grem_fechatraslado, grem_enviado = 1, empr_codigo = @empr_codigo, chof_codigo = @chof_codigo, vehi_codigo = @vehi_codigo, carr_codigo = @carr_codigo, grem_manifiesto = 1, grem_nromanifiesto = @grem_nromanifiesto where grem_codigo = @grem_codigo"
            cmd.Parameters.Add("@grem_codigo", SqlDbType.Int)
            cmd.Parameters.Add("@grem_fechatraslado", SqlDbType.DateTime).Value = Me.Fecha
            cmd.Parameters.Add("@empr_codigo", SqlDbType.Int).Value = Me.Transportista.Codigo
            cmd.Parameters.Add("@chof_codigo", SqlDbType.Int).Value = Me.Chofer.Codigo
            cmd.Parameters.Add("@vehi_codigo", SqlDbType.Int).Value = Me.Vehiculo.Codigo
            cmd.Parameters.Add("@carr_codigo", SqlDbType.Int).Value = Me.Carreta.Codigo
            cmd.Parameters.Add("@grem_nromanifiesto", SqlDbType.VarChar, 15).Value = Me.Numero
            For i = 0 To Me.GuiasSeleccionadas.Count - 1
                cmd.Parameters("@grem_codigo").Value = Me.GuiasSeleccionadas(i).Codigo
                cmd.ExecuteNonQuery()
            Next

            'Marcar guias como disponibles para manifiesto
            cmd.CommandText = "Update GuiaRemision set grem_fechatraslado = grem_fechaemision, grem_enviado = 0, grem_nromanifiesto = '' where grem_codigo = @grem_codigo"
            For i = 0 To Me.GuiasDisponibles.Count - 1
                cmd.Parameters("@grem_codigo").Value = Me.GuiasDisponibles(i)
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
            cmd.CommandText = "Update GuiaRemision set grem_enviado = 0, grem_nromanifiesto = '' where grem_codigo in (Select grem_codigo from DetalleManifiesto where mani_codigo = @mani_codigo)"
            cmd.Parameters.Add("@mani_codigo", SqlDbType.Int).Value = codigo
            cmd.ExecuteNonQuery()
            cmd.CommandText = "Delete from DetalleManifiesto where mani_codigo = @mani_codigo; Delete from Manifiesto where mani_codigo = @mani_codigo;"
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
