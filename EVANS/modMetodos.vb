Imports System.Data.SqlClient
Imports System.Xml

Module modMetodos
    'Declaración del API para liberar memoria RAM
    Private Declare Auto Function SetProcessWorkingSetSize Lib "kernel32.dll" (ByVal procHandle As IntPtr, ByVal min As Int32, ByVal max As Int32) As Boolean

    'Variables de conexion
    Public strServidor As String
    Public strUsuario As String = "sa"
    Public strClave As String = "sa"
    Public blAutenticacion As Boolean = True 'autenticacion windows
    Public strBD As String = "EVANS"
    Public strBD2 As String
    Public objConexion As New SqlConnection() 'conexion para BD general de la empresa
    Public objConexion2 As New SqlConnection() 'conexion para BD de comprobantes

    'Variables de sistema
    Public objParametros As New clsParametro
    Public objUsuarioActual As New clsUsuario
    Public objComprobante As New clsComprobante
    Public objGuia As New clsGuiaRemision
    Public objRecepcion As New clsRecepcion
    Public objManifiesto As New clsManifiesto
    Public bolGenerandoComprobante As Boolean
    Public bolGenerandoGuia As Boolean

    'Variables de impresion
    Public ControlElegido As Label
    Public ControlAnterior As Label
    Public IncrementoDesplazamiento As Integer


    Public Sub LimpiarRAM()

        Try
            Dim Mem As Process
            Mem = Process.GetCurrentProcess()
            SetProcessWorkingSetSize(Mem.Handle, -1, -1)
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try

    End Sub

    Public Function Conectar() As Boolean

        If blAutenticacion = True Then
            objConexion.ConnectionString = "Data Source=" & strServidor & ";Integrated Security=" & blAutenticacion & ";Initial Catalog=" & strBD & ";"
        Else
            objConexion.ConnectionString = "Data Source=" & strServidor & ";Integrated Security=" & blAutenticacion & ";User=" & strUsuario & ";Password=" & strClave & ";Initial Catalog=" & strBD & ";"
        End If

        Try
            objConexion.Open()
            objConexion.Close()
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

    Public Function Autenticar() As Boolean

        If blAutenticacion = True Then
            objConexion2.ConnectionString = "Data Source=" & strServidor & ";Integrated Security=" & blAutenticacion & ";Initial Catalog=" & strBD2 & ";"
        Else
            objConexion2.ConnectionString = "Data Source=" & strServidor & ";Integrated Security=" & blAutenticacion & ";User=" & strUsuario & ";Password=" & strClave & ";Initial Catalog=" & strBD2 & ";"
        End If

        Try
            objConexion2.Open()
            objConexion2.Close()

            Dim cmdUsuario As New SqlCommand
            Dim drUsuario As SqlDataReader
            cmdUsuario.Connection = objConexion
            cmdUsuario.CommandType = CommandType.Text
            cmdUsuario.Parameters.Add("@NombreUsuario", SqlDbType.VarChar, 30)
            cmdUsuario.Parameters("@NombreUsuario").Value = strUsuario
            cmdUsuario.CommandText = "Select usu_codigo, usu_NombreUsuario, usu_Clave, usu_NombreCompleto, usu_Admin, esta_codigo from Usuario where usu_NombreUsuario = @NombreUsuario"
            If objConexion.State = ConnectionState.Closed Then
                objConexion.Open()
            End If
            drUsuario = cmdUsuario.ExecuteReader

            While drUsuario.Read()
                objUsuarioActual.Codigo = NullToString(drUsuario("usu_codigo"))
                objUsuarioActual.NombreUsuario = NullToString(drUsuario("usu_NombreUsuario"))
                objUsuarioActual.Clave = NullToString(drUsuario("usu_Clave"))
                objUsuarioActual.NombreCompleto = NullToString(drUsuario("usu_NombreCompleto"))
                objUsuarioActual.Administrador = NullToString(drUsuario("usu_Admin"))
                objUsuarioActual.Estado.Codigo = NullToString(drUsuario("esta_codigo"))
            End While
            drUsuario.Close()

            If strUsuario = objUsuarioActual.NombreUsuario And strClave = objUsuarioActual.Clave Then
                Return True
            Else
                Return False
            End If

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

            If objConexion2.State = ConnectionState.Open Then
                objConexion2.Close()
            End If
        End Try

    End Function

    Public Function NullToString(ByVal objObjeto As Object) As String
        Dim strCadena As String
        strCadena = IIf(IsDBNull(objObjeto), "", objObjeto)
        Return strCadena
    End Function

    Public Sub DesactivarControles(ByVal ctlTab As TabControl, ByVal intPage As Integer, ByVal bolValor As Boolean)

        Try
            Dim ctl As Control
            For Each ctl In ctlTab.TabPages(intPage).Controls
                Dim tb As TextBox
                Dim chk As CheckBox
                Dim cb As ComboBox
                Dim dtp As DateTimePicker
                Dim btn As Button
                Dim lv As ListView
                Dim mtb As MaskedTextBox
                Dim dgv As DataGridView
                Dim gb As GroupBox

                If TypeOf ctl Is GroupBox Then
                    gb = CType(ctl, GroupBox)
                    Dim ctrl As Control
                    For Each ctrl In gb.Controls
                        If TypeOf ctrl Is TextBox Then
                            tb = CType(ctrl, TextBox)
                            tb.ReadOnly = bolValor
                        End If

                        If TypeOf ctrl Is CheckBox Then
                            chk = CType(ctrl, CheckBox)
                            chk.Enabled = Not bolValor
                        End If

                        If TypeOf ctrl Is ComboBox Then
                            cb = CType(ctrl, ComboBox)
                            cb.Enabled = Not bolValor
                        End If

                        If TypeOf ctrl Is DateTimePicker Then
                            dtp = CType(ctrl, DateTimePicker)
                            dtp.Enabled = Not bolValor
                        End If

                        If TypeOf ctrl Is Button Then
                            btn = CType(ctrl, Button)
                            btn.Enabled = Not bolValor
                        End If

                        If TypeOf ctrl Is ListView Then
                            lv = CType(ctrl, ListView)
                            lv.Enabled = Not bolValor
                        End If

                        If TypeOf ctrl Is MaskedTextBox Then
                            mtb = CType(ctrl, MaskedTextBox)
                            mtb.ReadOnly = bolValor
                        End If

                        If TypeOf ctrl Is DataGridView Then
                            dgv = CType(ctrl, DataGridView)
                            dgv.ReadOnly = bolValor
                        End If
                    Next
                End If

                If TypeOf ctl Is TextBox Then
                    tb = CType(ctl, TextBox)
                    tb.ReadOnly = bolValor
                End If

                If TypeOf ctl Is CheckBox Then
                    chk = CType(ctl, CheckBox)
                    chk.Enabled = Not bolValor
                End If

                If TypeOf ctl Is ComboBox Then
                    cb = CType(ctl, ComboBox)
                    cb.Enabled = Not bolValor
                End If

                If TypeOf ctl Is DateTimePicker Then
                    dtp = CType(ctl, DateTimePicker)
                    dtp.Enabled = Not bolValor
                End If

                If TypeOf ctl Is Button Then
                    btn = CType(ctl, Button)
                    btn.Enabled = Not bolValor
                End If

                If TypeOf ctl Is ListView Then
                    lv = CType(ctl, ListView)
                    lv.Enabled = Not bolValor
                End If

                If TypeOf ctl Is MaskedTextBox Then
                    mtb = CType(ctl, MaskedTextBox)
                    mtb.ReadOnly = bolValor
                End If

                If TypeOf ctl Is DataGridView Then
                    dgv = CType(ctl, DataGridView)
                    dgv.ReadOnly = bolValor
                End If
            Next

        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End Try
        

    End Sub

    Public Sub LimpiarControles(ByVal ctlTab As TabControl, ByVal intPage As Integer)

        Try
            Dim ctl As Control
            For Each ctl In ctlTab.TabPages(intPage).Controls
                Dim tb As TextBox
                Dim cb As ComboBox
                Dim dtp As DateTimePicker
                Dim lv As ListView
                Dim mtb As MaskedTextBox
                Dim dgv As DataGridView
                Dim gb As GroupBox

                If TypeOf ctl Is GroupBox Then
                    gb = CType(ctl, GroupBox)
                    Dim ctrl As Control
                    For Each ctrl In gb.Controls
                        If TypeOf ctrl Is TextBox Then
                            tb = CType(ctrl, TextBox)
                            tb.Clear()
                        End If

                        If TypeOf ctrl Is ComboBox Then
                            cb = CType(ctrl, ComboBox)
                            cb.Text = ""
                        End If

                        If TypeOf ctrl Is DateTimePicker Then
                            dtp = CType(ctrl, DateTimePicker)
                            dtp.Value = Date.Today
                        End If

                        If TypeOf ctrl Is ListView Then
                            lv = CType(ctrl, ListView)
                            lv.Items.Clear()
                        End If

                        If TypeOf ctrl Is MaskedTextBox Then
                            mtb = CType(ctrl, MaskedTextBox)
                            mtb.Clear()
                        End If

                        If TypeOf ctrl Is DataGridView Then
                            dgv = CType(ctrl, DataGridView)
                            dgv.Rows.Clear()
                        End If
                    Next
                End If

                If TypeOf ctl Is TextBox Then
                    tb = CType(ctl, TextBox)
                    tb.Clear()
                End If

                If TypeOf ctl Is ComboBox Then
                    cb = CType(ctl, ComboBox)
                    cb.Text = ""
                End If

                If TypeOf ctl Is DateTimePicker Then
                    dtp = CType(ctl, DateTimePicker)
                    dtp.Value = Date.Today
                End If

                If TypeOf ctl Is ListView Then
                    lv = CType(ctl, ListView)
                    lv.Items.Clear()
                End If

                If TypeOf ctl Is MaskedTextBox Then
                    mtb = CType(ctl, MaskedTextBox)
                    mtb.Clear()
                End If

                If TypeOf ctl Is DataGridView Then
                    dgv = CType(ctl, DataGridView)
                    dgv.Rows.Clear()
                End If
            Next

        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End Try
    End Sub

    Public Function BuscarCodigo(ByVal CampoCodigo As String, ByVal CampoABuscar As String, ByVal TextoABuscar As String, ByVal Tabla As String) As Integer

        Try
            Dim cmdBuscar As New SqlCommand
            Dim strConsulta As String
            Dim intCodigo As Integer
            cmdBuscar.Connection = objConexion
            cmdBuscar.CommandType = CommandType.Text
            strConsulta = "Select " + CampoCodigo + " From " + Tabla + " Where " + CampoABuscar + " = '" + TextoABuscar + "'"
            cmdBuscar.CommandText = strConsulta
            If objConexion.State = ConnectionState.Closed Then
                objConexion.Open()
            End If
            intCodigo = cmdBuscar.ExecuteScalar

            Return intCodigo

        Catch ex As SqlException
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            If objConexion.State = ConnectionState.Open Then
                objConexion.Close()
            End If
        End Try

    End Function

    Public Sub LLenarCombo(ByVal CampoCodigo As String, ByVal CampoNombre As String, ByVal NombreTabla As String, ByVal NombreCombo As ComboBox, ByVal SoloActivos As Boolean)

        Try
            Dim cmdListar As New SqlCommand
            Dim drListar As SqlDataReader
            Dim strConsulta As String
            cmdListar.CommandType = CommandType.Text
            cmdListar.Connection = objConexion
            If SoloActivos = True Then
                strConsulta = "Select " + CampoCodigo + ", " + CampoNombre + " From " + NombreTabla + " Where esta_codigo = 1"
            Else
                strConsulta = "Select " + CampoCodigo + ", " + CampoNombre + " From " + NombreTabla
            End If
            cmdListar.CommandText = strConsulta
            If objConexion.State = ConnectionState.Closed Then
                objConexion.Open()
            End If
            drListar = cmdListar.ExecuteReader
            Dim Lista As New List(Of clsComboItem)
            While drListar.Read
                Dim item As New clsComboItem
                item.Codigo = NullToString(drListar(0))
                item.Nombre = NullToString(drListar(1))
                Lista.Add(item)
            End While
            drListar.Close()
            NombreCombo.DataSource = Nothing
            NombreCombo.DataSource = Lista
            NombreCombo.DisplayMember = "Nombre"
            NombreCombo.ValueMember = "Codigo"

        Catch ex As SqlException
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            If objConexion.State = ConnectionState.Open Then
                objConexion.Close()
            End If
        End Try

    End Sub

    Public Function CrearBD() As Boolean

        Dim nombreBD As String = Now.Year.ToString
        Dim cmd As New SqlCommand
        Dim ruta As String = Application.StartupPath() + "\DATA\"
        Try
            If Not System.IO.Directory.Exists(ruta) Then
                System.IO.Directory.CreateDirectory(ruta)
            End If

            If objConexion.State = ConnectionState.Closed Then
                objConexion.Open()
            End If

            cmd.Connection = objConexion
            cmd.CommandType = CommandType.Text
            cmd.CommandText = "CREATE DATABASE [" + nombreBD + "] ON  PRIMARY " & vbCrLf & _
            "( NAME = N'" + nombreBD + "', FILENAME = N'" + ruta + nombreBD + ".mdf' , SIZE = 10240KB , MAXSIZE = UNLIMITED, FILEGROWTH = 10240KB )" & vbCrLf & _
            " LOG ON " & vbCrLf & _
            "( NAME = N'" + nombreBD + "_log', FILENAME = N'" + ruta + nombreBD + "_log.LDF' , SIZE = 3072KB , MAXSIZE = 1048576KB , FILEGROWTH = 10%);"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Select Top 0 * into [" + nombreBD + "].dbo.DetalleGuia From [2010].dbo.DetalleGuia;" & vbCrLf & _
            "Select Top 0 * into [" + nombreBD + "].dbo.GuiaRemision From [2010].dbo.GuiaRemision;" & vbCrLf & _
            "Select Top 0 * into [" + nombreBD + "].dbo.DetalleComprobante From [2010].dbo.DetalleComprobante;" & vbCrLf & _
            "Select Top 0 * into [" + nombreBD + "].dbo.Comprobante From [2010].dbo.Comprobante;" & vbCrLf & _
            "Select Top 0 * into [" + nombreBD + "].dbo.DetalleManifiesto From [2010].dbo.DetalleManifiesto;" & vbCrLf & _
            "Select Top 0 * into [" + nombreBD + "].dbo.Manifiesto From [2010].dbo.Manifiesto;" & vbCrLf & _
            "Select Top 0 * into [" + nombreBD + "].dbo.DetalleRecepcion From [2010].dbo.DetalleRecepcion;" & vbCrLf & _
            "Select Top 0 * into [" + nombreBD + "].dbo.Recepcion From [2010].dbo.Recepcion;"
            cmd.ExecuteNonQuery()
            Return True

        Catch ex As SqlException
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            cmd.CommandText = "ALTER DATABASE [" + nombreBD + "] SET SINGLE_USER WITH ROLLBACK IMMEDIATE; DROP DATABASE [" + nombreBD + "];"
            cmd.ExecuteNonQuery()
            Return False
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            cmd.CommandText = "ALTER DATABASE [" + nombreBD + "] SET SINGLE_USER WITH ROLLBACK IMMEDIATE; DROP DATABASE [" + nombreBD + "];"
            cmd.ExecuteNonQuery()
            Return False
        Finally
            If objConexion.State = ConnectionState.Open Then
                objConexion.Close()
            End If
        End Try

    End Function

    Public Function NumerosALetras(ByVal numero As String) As String

        Try
            '********Declara variables de tipo cadena************
            Dim palabras, entero, dec, flag As String

            '********Declara variables de tipo entero***********
            Dim num, x, y As Integer

            flag = "N"

            '**********Número Negativo***********
            If Mid(numero, 1, 1) = "-" Then
                numero = Mid(numero, 2, numero.ToString.Length - 1).ToString
                palabras = "menos "
            End If

            '**********Si tiene ceros a la izquierda*************
            For x = 1 To numero.ToString.Length
                If Mid(numero, 1, 1) = "0" Then
                    numero = Trim(Mid(numero, 2, numero.ToString.Length).ToString)
                    If Trim(numero.ToString.Length) = 0 Then palabras = ""
                Else
                    Exit For
                End If
            Next

            '*********Dividir parte entera y decimal************
            For y = 1 To Len(numero)
                If Mid(numero, y, 1) = "." Then
                    flag = "S"
                Else
                    If flag = "N" Then
                        entero = entero + Mid(numero, y, 1)
                    Else
                        dec = dec + Mid(numero, y, 1)
                    End If
                End If
            Next y

            If Len(dec) = 0 Then dec = "00"

            If Len(dec) = 1 Then dec = dec & "0"


            '**********proceso de conversión***********
            flag = "N"

            If Val(numero) <= 999999999 Then
                For y = Len(entero) To 1 Step -1
                    num = Len(entero) - (y - 1)
                    Select Case y
                        Case 3, 6, 9
                            '**********Asigna las palabras para las centenas***********
                            Select Case Mid(entero, num, 1)
                                Case "1"
                                    If Mid(entero, num + 1, 1) = "0" And Mid(entero, num + 2, 1) = "0" Then
                                        palabras = palabras & "cien "
                                    Else
                                        palabras = palabras & "ciento "
                                    End If
                                Case "2"
                                    palabras = palabras & "doscientos "
                                Case "3"
                                    palabras = palabras & "trescientos "
                                Case "4"
                                    palabras = palabras & "cuatrocientos "
                                Case "5"
                                    palabras = palabras & "quinientos "
                                Case "6"
                                    palabras = palabras & "seiscientos "
                                Case "7"
                                    palabras = palabras & "setecientos "
                                Case "8"
                                    palabras = palabras & "ochocientos "
                                Case "9"
                                    palabras = palabras & "novecientos "
                            End Select
                        Case 2, 5, 8
                            '*********Asigna las palabras para las decenas************
                            Select Case Mid(entero, num, 1)
                                Case "1"
                                    If Mid(entero, num + 1, 1) = "0" Then
                                        flag = "S"
                                        palabras = palabras & "diez "
                                    End If
                                    If Mid(entero, num + 1, 1) = "1" Then
                                        flag = "S"
                                        palabras = palabras & "once "
                                    End If
                                    If Mid(entero, num + 1, 1) = "2" Then
                                        flag = "S"
                                        palabras = palabras & "doce "
                                    End If
                                    If Mid(entero, num + 1, 1) = "3" Then
                                        flag = "S"
                                        palabras = palabras & "trece "
                                    End If
                                    If Mid(entero, num + 1, 1) = "4" Then
                                        flag = "S"
                                        palabras = palabras & "catorce "
                                    End If
                                    If Mid(entero, num + 1, 1) = "5" Then
                                        flag = "S"
                                        palabras = palabras & "quince "
                                    End If
                                    If Mid(entero, num + 1, 1) > "5" Then
                                        flag = "N"
                                        palabras = palabras & "dieci"
                                    End If
                                Case "2"
                                    If Mid(entero, num + 1, 1) = "0" Then
                                        palabras = palabras & "veinte "
                                        flag = "S"
                                    Else
                                        palabras = palabras & "veinti"
                                        flag = "N"
                                    End If
                                Case "3"
                                    If Mid(entero, num + 1, 1) = "0" Then
                                        palabras = palabras & "treinta "
                                        flag = "S"
                                    Else
                                        palabras = palabras & "treinta y "
                                        flag = "N"
                                    End If
                                Case "4"
                                    If Mid(entero, num + 1, 1) = "0" Then
                                        palabras = palabras & "cuarenta "
                                        flag = "S"
                                    Else
                                        palabras = palabras & "cuarenta y "
                                        flag = "N"
                                    End If
                                Case "5"
                                    If Mid(entero, num + 1, 1) = "0" Then
                                        palabras = palabras & "cincuenta "
                                        flag = "S"
                                    Else
                                        palabras = palabras & "cincuenta y "
                                        flag = "N"
                                    End If
                                Case "6"
                                    If Mid(entero, num + 1, 1) = "0" Then
                                        palabras = palabras & "sesenta "
                                        flag = "S"
                                    Else
                                        palabras = palabras & "sesenta y "
                                        flag = "N"
                                    End If
                                Case "7"
                                    If Mid(entero, num + 1, 1) = "0" Then
                                        palabras = palabras & "setenta "
                                        flag = "S"
                                    Else
                                        palabras = palabras & "setenta y "
                                        flag = "N"
                                    End If
                                Case "8"
                                    If Mid(entero, num + 1, 1) = "0" Then
                                        palabras = palabras & "ochenta "
                                        flag = "S"
                                    Else
                                        palabras = palabras & "ochenta y "
                                        flag = "N"
                                    End If
                                Case "9"
                                    If Mid(entero, num + 1, 1) = "0" Then
                                        palabras = palabras & "noventa "
                                        flag = "S"
                                    Else
                                        palabras = palabras & "noventa y "
                                        flag = "N"
                                    End If
                            End Select
                        Case 1, 4, 7
                            '*********Asigna las palabras para las unidades*********
                            Select Case Mid(entero, num, 1)
                                Case "1"
                                    If flag = "N" Then
                                        If y = 1 Then
                                            palabras = palabras & "uno "
                                        Else
                                            palabras = palabras & "un "
                                        End If
                                    End If
                                Case "2"
                                    If flag = "N" Then palabras = palabras & "dos "
                                Case "3"
                                    If flag = "N" Then palabras = palabras & "tres "
                                Case "4"
                                    If flag = "N" Then palabras = palabras & "cuatro "
                                Case "5"
                                    If flag = "N" Then palabras = palabras & "cinco "
                                Case "6"
                                    If flag = "N" Then palabras = palabras & "seis "
                                Case "7"
                                    If flag = "N" Then palabras = palabras & "siete "
                                Case "8"
                                    If flag = "N" Then palabras = palabras & "ocho "
                                Case "9"
                                    If flag = "N" Then palabras = palabras & "nueve "
                            End Select
                    End Select

                    '***********Asigna la palabra mil***************
                    If y = 4 Then
                        If Mid(entero, 6, 1) <> "0" Or Mid(entero, 5, 1) <> "0" Or Mid(entero, 4, 1) <> "0" Or _
                        (Mid(entero, 6, 1) = "0" And Mid(entero, 5, 1) = "0" And Mid(entero, 4, 1) = "0" And _
                        Len(entero) <= 6) Then palabras = palabras & "mil "
                    End If

                    '**********Asigna la palabra millón*************
                    If y = 7 Then
                        If Len(entero) = 7 And Mid(entero, 1, 1) = "1" Then
                            palabras = palabras & "millón "
                        Else
                            palabras = palabras & "millones "
                        End If
                    End If
                Next y

                '**********Une la parte entera y la parte decimal*************
                If dec <> "" Then
                    NumerosALetras = palabras & "con " & dec
                Else
                    NumerosALetras = palabras
                End If
            Else
                NumerosALetras = ""
            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return ""
        End Try
    End Function

    Public Function MarcarImpreso(ByVal Codigo As Integer, ByVal CampoCodigo As String, ByVal CampoImpresion As String, ByVal NombreTabla As String) As Boolean

        Dim Transaccion As SqlTransaction
        Try
            If objConexion2.State = ConnectionState.Closed Then
                objConexion2.Open()
            End If
            Transaccion = objConexion2.BeginTransaction
            Dim cmd As New SqlCommand
            cmd.CommandType = CommandType.Text
            cmd.Connection = objConexion2
            cmd.Transaction = Transaccion
            cmd.CommandText = "Update " + NombreTabla + " Set " + CampoImpresion + " = 1 where " + CampoCodigo + " = @Codigo"
            cmd.Parameters.Add("@Codigo", SqlDbType.Int).Value = Codigo
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

    Public Sub MostrarGuia(ByVal codigo As Integer)

        Try
            Dim objGuia As New clsGuiaRemision

            objGuia.Codigo = codigo
            If objGuia.BuscarXCodigo() Then

                If frmGuiaRemision.Visible Then
                    frmGuiaRemision.Activate()
                Else
                    frmGuiaRemision.MdiParent = frmPrincipal
                    frmGuiaRemision.Show()
                End If

                frmGuiaRemision.TabControl1.TabPages(1).Enabled = True
                frmGuiaRemision.TabControl1.TabPages(0).Enabled = False
                frmGuiaRemision.TabControl1.SelectTab(1)

                frmGuiaRemision.txtCodigo.Text = objGuia.Codigo
                frmGuiaRemision.txtSerie.Text = objGuia.Serie
                frmGuiaRemision.txtNumero.Text = objGuia.Numero
                frmGuiaRemision.dtpEmision.Value = objGuia.FechaEmision
                frmGuiaRemision.dtpTraslado.Value = objGuia.FechaTraslado
                frmGuiaRemision.txtRemitente.Text = objGuia.Remitente.Nombre
                frmGuiaRemision.cbRemiID.Text = objGuia.Remitente.TipoID.Descripcion
                frmGuiaRemision.cbRemiID.SelectedValue = objGuia.Remitente.TipoID.Codigo
                frmGuiaRemision.txtRemiNroID.Text = objGuia.Remitente.NumeroID
                If objGuia.TipoDireccionPartida = 0 Then
                    frmGuiaRemision.optAgencia1.Checked = True
                    frmGuiaRemision.optDireccion1.Checked = False
                Else
                    frmGuiaRemision.optDireccion1.Checked = True
                    frmGuiaRemision.optAgencia1.Checked = False
                End If
                frmGuiaRemision.cbDireccionPartida.Text = objGuia.DireccionPartida
                frmGuiaRemision.txtDestinatario.Text = objGuia.Destinatario.Nombre
                frmGuiaRemision.cbDestiID.Text = objGuia.Destinatario.TipoID.Descripcion
                frmGuiaRemision.cbDestiID.SelectedValue = objGuia.Destinatario.TipoID.Codigo
                frmGuiaRemision.txtDestiNroID.Text = objGuia.Destinatario.NumeroID
                If objGuia.TipoDireccionDestino = 0 Then
                    frmGuiaRemision.optAgencia2.Checked = True
                    frmGuiaRemision.optDireccion2.Checked = False
                Else
                    frmGuiaRemision.optDireccion2.Checked = True
                    frmGuiaRemision.optAgencia2.Checked = False
                End If
                frmGuiaRemision.cbDireccionDestino.Text = objGuia.DireccionDestino
                frmGuiaRemision.cbDestino.Text = objGuia.Destino.Nombre
                frmGuiaRemision.cbDestino.SelectedValue = objGuia.Destino.Codigo
                frmGuiaRemision.txtDistancia.Text = objGuia.Destino.DistanciaVirtual
                frmGuiaRemision.cbVehiculo.Text = objGuia.Vehiculo.Placa
                frmGuiaRemision.cbVehiculo.SelectedValue = objGuia.Vehiculo.Codigo
                frmGuiaRemision.cbCarreta.Text = objGuia.Carreta.Placa
                frmGuiaRemision.cbCarreta.SelectedValue = objGuia.Carreta.Codigo
                frmGuiaRemision.cbChofer.Text = objGuia.Chofer.Nombre
                frmGuiaRemision.cbChofer.SelectedValue = objGuia.Chofer.Codigo
                frmGuiaRemision.cbSubcontratada.Text = objGuia.Empresa.Nombre
                frmGuiaRemision.cbSubcontratada.SelectedValue = objGuia.Empresa.Codigo
                frmGuiaRemision.cbEstado.Text = objGuia.Estado.Descripcion
                frmGuiaRemision.cbEstado.SelectedValue = objGuia.Estado.Codigo
                frmGuiaRemision.lblBultosTotales.Text = objGuia.BultosTotales
                frmGuiaRemision.lblPesoTotal.Text = FormatNumber(objGuia.PesoTotal, 2)
                frmGuiaRemision.lblCostoTotal.Text = FormatNumber(objGuia.CostoTotal, 2)
                frmGuiaRemision.lblImpreso.Visible = objGuia.Impreso
                frmGuiaRemision.cbDVTipo.Text = objGuia.TipoDocumentoVenta.Descripcion
                frmGuiaRemision.cbDVTipo.SelectedValue = objGuia.TipoDocumentoVenta.Codigo
                frmGuiaRemision.txtDocVenta.Text = objGuia.DocumentoVenta
                frmGuiaRemision.txtObservaciones.Text = objGuia.Observacion
                frmGuiaRemision.txtUsuario.Text = objGuia.Usuario.NombreCompleto
                frmGuiaRemision.chkManifiesto.Checked = IIf(objGuia.TieneManifiesto = 0, True, False)
                frmGuiaRemision.txtManifiesto.Text = objGuia.NroManifiesto

                frmGuiaRemision.dgvDetalle.Rows.Clear()
                Dim i As Integer
                For i = 0 To objGuia.Detalles.Count - 1
                    frmGuiaRemision.dgvDetalle.Rows.Add()
                    frmGuiaRemision.dgvDetalle.Rows(i).Cells(0).Value = objGuia.Detalles(i).TipoDocumento
                    frmGuiaRemision.dgvDetalle.Rows(i).Cells(1).Value = objGuia.Detalles(i).NumeroDocumento
                    frmGuiaRemision.dgvDetalle.Rows(i).Cells(2).Value = IIf(objGuia.Detalles(i).Cantidad = 0, Nothing, objGuia.Detalles(i).Cantidad)
                    frmGuiaRemision.dgvDetalle.Rows(i).Cells(3).Value = IIf(objGuia.Detalles(i).Descripcion = "", Nothing, objGuia.Detalles(i).Descripcion)
                    frmGuiaRemision.dgvDetalle.Rows(i).Cells(4).Value = IIf(objGuia.Detalles(i).Peso = 0, Nothing, objGuia.Detalles(i).Peso)
                    frmGuiaRemision.dgvDetalle.Rows(i).Cells(5).Value = IIf(objGuia.Detalles(i).Unidad = "", Nothing, objGuia.Detalles(i).Unidad)
                    frmGuiaRemision.dgvDetalle.Rows(i).Cells(6).Value = IIf(objGuia.Detalles(i).Costo = 0, Nothing, objGuia.Detalles(i).Costo)
                Next i

                'Desactivar controles
                DesactivarControles(frmGuiaRemision.TabControl1, 1, True)

                'Comportamiento de botones
                frmGuiaRemision.btnNuevo.Enabled = True
                frmGuiaRemision.btnGrabar.Enabled = False
                frmGuiaRemision.btnEditar.Enabled = True
                frmGuiaRemision.btnCancelar.Enabled = True

                frmGuiaRemision.btnGrabar.Text = "Grabar"

            Else
                Return
            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End Try

    End Sub

    Public Sub UbicarControles(ByVal nombreNodo As String, ByVal frm As Form)

        Try
            Dim XML As New XmlDocument
            Dim ListaNodos As XmlNodeList
            Dim Coordenada As Point

            XML.Load(My.Application.Info.DirectoryPath + "\Config\" + nombreNodo + ".xml")
            ListaNodos = XML.SelectNodes("/" + nombreNodo + "/control")

            For Each ctrl As Control In frm.Controls
                If TypeOf ctrl Is Label Then
                    Dim lbl As Label
                    lbl = CType(ctrl, Label)
                    For Each Nodo As XmlNode In ListaNodos
                        If lbl.Name.ToLower = Nodo.Attributes.GetNamedItem("name").Value.ToLower Then
                            Coordenada.X = Convert.ToInt32(Nodo.Attributes.GetNamedItem("x").Value)
                            Coordenada.Y = Convert.ToInt32(Nodo.Attributes.GetNamedItem("y").Value)
                            lbl.Location = Coordenada
                            Dim tamaño As Single
                            Dim nombreFuente As String
                            Dim negrita As Boolean
                            nombreFuente = Nodo.Attributes.GetNamedItem("font").Value.ToString
                            tamaño = Convert.ToSingle(Nodo.Attributes.GetNamedItem("size").Value)
                            negrita = Convert.ToBoolean(Nodo.Attributes.GetNamedItem("bold").Value)
                            If negrita = True Then
                                Dim Fuente As New System.Drawing.Font(nombreFuente, tamaño, FontStyle.Bold)
                                lbl.Font = Fuente
                            Else
                                Dim Fuente As New System.Drawing.Font(nombreFuente, tamaño, FontStyle.Regular)
                                lbl.Font = Fuente
                            End If
                            lbl.BringToFront()
                        End If
                    Next
                End If
            Next

        Catch ex As XmlException
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try

    End Sub

    Public Sub GrabarUbicaciones(ByVal nombreNodo As String, ByVal frm As Form)

        Try
            Dim XML As New XmlDocument
            Dim ListaNodos As XmlNodeList

            XML.Load(My.Application.Info.DirectoryPath + "\Config\" + nombreNodo + ".xml")
            ListaNodos = XML.SelectNodes("/" + nombreNodo + "/control")

            For Each ctrl As Control In frm.Controls
                If TypeOf ctrl Is Label Then
                    Dim lbl As Label
                    lbl = CType(ctrl, Label)
                    For Each Nodo As XmlNode In ListaNodos
                        If lbl.Name.ToLower = Nodo.Attributes.GetNamedItem("name").Value.ToLower Then
                            Nodo.Attributes.GetNamedItem("x").Value = lbl.Location.X.ToString
                            Nodo.Attributes.GetNamedItem("y").Value = lbl.Location.Y.ToString
                            Nodo.Attributes.GetNamedItem("font").Value = lbl.Font.Name
                            Nodo.Attributes.GetNamedItem("size").Value = lbl.Font.Size.ToString
                            Nodo.Attributes.GetNamedItem("bold").Value = lbl.Font.Bold.ToString
                        End If
                    Next
                End If
            Next

            XML.Save(My.Application.Info.DirectoryPath + "\Config\" + nombreNodo + ".xml")

        Catch ex As XmlException
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try

    End Sub

    Public Sub CrearXML(ByVal nombre As String, ByVal frm As Form)

        Try
            Dim myXmlTextWriter As XmlTextWriter = New XmlTextWriter(nombre + ".xml", System.Text.Encoding.UTF8)
            myXmlTextWriter.Formatting = System.Xml.Formatting.Indented
            myXmlTextWriter.WriteStartDocument(False)

            'Crear el elemento de documento principal.
            myXmlTextWriter.WriteStartElement(nombre)

            For Each ctrl As Control In frm.Controls
                If TypeOf ctrl Is Label And ctrl.Name <> "lblPosicion" Then
                    Dim lbl As Label
                    lbl = CType(ctrl, Label)
                    myXmlTextWriter.WriteStartElement("control")
                    myXmlTextWriter.WriteAttributeString("name", lbl.Name.ToLower)
                    myXmlTextWriter.WriteAttributeString("x", lbl.Location.X.ToString)
                    myXmlTextWriter.WriteAttributeString("y", lbl.Location.Y.ToString)
                    myXmlTextWriter.WriteAttributeString("font", lbl.Font.Name)
                    myXmlTextWriter.WriteAttributeString("size", lbl.Font.Size.ToString)
                    myXmlTextWriter.WriteAttributeString("bold", lbl.Font.Bold.ToString)
                    myXmlTextWriter.WriteEndElement()
                End If
            Next

            'Cerrar el elemento principal.
            myXmlTextWriter.WriteEndElement()

            myXmlTextWriter.Flush()
            myXmlTextWriter.Close()

        Catch ex As XmlException
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Public Sub AsignarEventos(ByVal frm As Form)

        Try
            For Each ctrl As Control In frm.Controls
                If TypeOf ctrl Is Label And ctrl.Name <> "lblPosicion" Then
                    AddHandler ctrl.MouseClick, AddressOf Control_MouseClick
                End If
            Next

            AddHandler frm.KeyPress, AddressOf Control_KeyPress

        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try

    End Sub

    Public Sub EliminarEventos(ByVal frm As Form)

        Try
            For Each ctrl As Control In frm.Controls
                If TypeOf ctrl Is Label And ctrl.Name <> "lblPosicion" Then
                    RemoveHandler ctrl.MouseClick, AddressOf Control_MouseClick
                End If
            Next

            RemoveHandler frm.KeyPress, AddressOf Control_KeyPress

        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try

    End Sub

    Public Sub Control_MouseClick(ByVal sender As Object, ByVal e As System.EventArgs)

        Try
            ControlAnterior = ControlElegido
            ControlElegido = CType(sender, Label)
            ControlElegido.BackColor = Color.Red
            ControlElegido.ForeColor = Color.White

            If ControlElegido.Name = "lblGRR" Then
                Dim rc As clsResizeableControl
                rc = New clsResizeableControl(ControlElegido)
            End If
            
            ControlElegido.Focus()
            If Not ControlAnterior Is Nothing Then
                ControlAnterior.BackColor = Color.White
                ControlAnterior.ForeColor = Color.Black
            End If

            MostrarCoordenadas()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try

    End Sub

    Public Sub MostrarCoordenadas()

        Try
            Dim frm As Form
            frm = CType(ControlElegido.Parent, Form)
            For Each ctrl As Control In frm.Controls
                If TypeOf ctrl Is GroupBox Then
                    Dim gb As GroupBox
                    gb = CType(ctrl, GroupBox)

                    For Each ctl As Control In gb.Controls

                        If TypeOf ctl Is Label Then
                            Dim lbl As Label
                            lbl = CType(ctl, Label)
                            If lbl.Name = "lblPosicion" Then
                                lbl.Text = "Coordenadas de ubicación: " + ControlElegido.Location.X.ToString + "," + ControlElegido.Location.Y.ToString
                            End If
                        End If

                        If TypeOf ctl Is ComboBox Then
                            Dim cb As ComboBox
                            cb = CType(ctl, ComboBox)
                            If cb.Name = "cbFonts" Then
                                cb.SelectedItem = ControlElegido.Font.Name
                            End If
                        End If

                        If TypeOf ctl Is NumericUpDown Then
                            Dim nud As NumericUpDown
                            nud = CType(ctl, NumericUpDown)
                            If nud.Name = "nudSize" Then
                                nud.Value = Convert.ToDecimal(ControlElegido.Font.Size)
                            End If
                        End If

                        If TypeOf ctl Is CheckBox Then
                            Dim chk As CheckBox
                            chk = CType(ctl, CheckBox)
                            If chk.Name = "chkBold" Then
                                chk.Checked = IIf(ControlElegido.Font.Bold = True, True, False)
                            End If
                        End If

                    Next
                End If
            Next

        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try

    End Sub

    Public Sub Control_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)

        Try
            If Not ControlElegido Is Nothing Then
                Select Case e.KeyChar
                    Case ChrW(Keys.D8) 'mover hacia arriba
                        Dim coordenada As Point
                        coordenada.X = ControlElegido.Location.X
                        coordenada.Y = ControlElegido.Location.Y - IncrementoDesplazamiento
                        ControlElegido.Location = coordenada
                        MostrarCoordenadas()
                    Case ChrW(Keys.D2) 'mover hacia abajo
                        Dim coordenada As Point
                        coordenada.X = ControlElegido.Location.X
                        coordenada.Y = ControlElegido.Location.Y + IncrementoDesplazamiento
                        ControlElegido.Location = coordenada
                        MostrarCoordenadas()
                    Case ChrW(Keys.D4) 'mover hacia la izquierda
                        Dim coordenada As Point
                        coordenada.X = ControlElegido.Location.X - IncrementoDesplazamiento
                        coordenada.Y = ControlElegido.Location.Y
                        ControlElegido.Location = coordenada
                        MostrarCoordenadas()
                    Case ChrW(Keys.D6) 'mover hacia la derecha
                        Dim coordenada As Point
                        coordenada.X = ControlElegido.Location.X + IncrementoDesplazamiento
                        coordenada.Y = ControlElegido.Location.Y
                        ControlElegido.Location = coordenada
                        MostrarCoordenadas()
                End Select
            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try

    End Sub

    Public Function CargarFuentes() As String()

        Try
            Dim fuentesInstaladas As New Drawing.Text.InstalledFontCollection()

            ' Obtenemos un array con los objetos FontFamily
            Dim fontFamilies() As FontFamily = fuentesInstaladas.Families

            Dim fonts(fontFamilies.Length - 1) As String
            Dim n As Integer

            ' Recorremos el array de objetos FontFamily
            For Each font As FontFamily In fontFamilies
                fonts(n) = font.Name
                n += 1
            Next

            Return fonts

        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return Nothing
        End Try

    End Function

    Function ExportarAExcel(ByVal ElGrid As DataGridView) As Boolean

        'Creamos las variables
        Dim exApp As New Microsoft.Office.Interop.Excel.Application
        Dim exLibro As Microsoft.Office.Interop.Excel.Workbook
        Dim exHoja As Microsoft.Office.Interop.Excel.Worksheet

        Try
            'Añadimos el Libro al programa, y la hoja al libro
            exLibro = exApp.Workbooks.Add
            exHoja = exLibro.Worksheets.Add()

            ' ¿Cuantas columnas y cuantas filas?
            Dim NCol As Integer = ElGrid.ColumnCount
            Dim NRow As Integer = ElGrid.RowCount

            'Aqui recorremos todas las filas, y por cada fila todas las columnas y vamos escribiendo.
            For i As Integer = 1 To NCol
                exHoja.Cells.Item(1, i) = ElGrid.Columns(i - 1).Name.ToString
                'exHoja.Cells.Item(1, i).HorizontalAlignment = 3
            Next

            For Fila As Integer = 0 To NRow - 1
                For Col As Integer = 0 To NCol - 1
                    exHoja.Cells.Item(Fila + 2, Col + 1) = ElGrid.Rows(Fila).Cells(Col).Value
                Next
            Next
            'Titulo en negrita, Alineado al centro y que el tamaño de la columna se
            'ajuste al texto
            exHoja.Rows.Item(1).Font.Bold = 1
            exHoja.Rows.Item(1).HorizontalAlignment = 3
            exHoja.Columns.AutoFit()


            'Aplicación visible
            exApp.Application.Visible = True

            exHoja = Nothing
            exLibro = Nothing
            exApp = Nothing

            Return True

        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical, "Error al exportar a Excel")
            Return False
        End Try

    End Function

End Module
