Public Class frmImprimirFactura

    Dim ModoEdicion As Boolean = False

    Private Sub btnCancelar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancelar.Click
        Me.Close()
    End Sub

    Private Sub btnImprimir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnImprimir.Click

        If Imprimir() = True Then
            MarcarImpreso(objComprobante.Codigo, "comp_codigo", "comp_impreso", "Comprobante")
            Me.Close()
        End If

    End Sub

    Private Sub frmImprimirFactura_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing

        LimpiarRAM()

    End Sub

    Private Sub frmImprimirComp_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Me.Top = 0
        Me.Left = 0
        btnEditar.Image = ImageList1.Images(1)
        IncrementoDesplazamiento = tbIncremento.Value

        'CrearXML("factura", Me)
        UbicarControles("factura", Me)

    End Sub

    Private Function Imprimir() As Boolean

        Try
            btnImprimir.Visible = False
            btnCancelar.Visible = False
            btnEditar.Visible = False
            lblPosicion.Visible = False
            PictureBox1.Visible = False

            Cursor.Current = Cursors.WaitCursor

            With Me.PrintForm1
                .PrintAction = Printing.PrintAction.PrintToPrinter
                .PrinterSettings.DefaultPageSettings.Landscape = False
                .PrinterSettings.DefaultPageSettings.Margins.Top = 10
                .PrinterSettings.DefaultPageSettings.Margins.Left = 20
                .PrinterSettings.DefaultPageSettings.Margins.Right = 5
                .Print(Me, PowerPacks.Printing.PrintForm.PrintOption.Scrollable)
            End With

            Cursor.Current = Cursors.Default

            btnImprimir.Visible = True
            btnCancelar.Visible = True
            PictureBox1.Visible = True
            btnEditar.Visible = True
            lblPosicion.Visible = True

            Return True

        Catch ex As Exception
            Cursor.Current = Cursors.Default
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return False
        End Try

    End Function

    Private Sub btnEditar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEditar.Click

        If ModoEdicion = False Then  'empieza la edicion
            ControlElegido = New Label
            ControlAnterior = New Label
            btnImprimir.Enabled = False
            ModoEdicion = True
            AsignarEventos(Me)
            gbIncremento.Visible = True
            btnEditar.Image = ImageList1.Images(0)
            btnEditar.Text = "Grabar"
            LlenarComboFuentes()
        Else
            ModoEdicion = False   'graba y termina la edicion
            btnImprimir.Enabled = True
            gbIncremento.Visible = False
            GrabarUbicaciones("factura", Me)
            EliminarEventos(Me)
            btnEditar.Image = ImageList1.Images(1)
            btnEditar.Text = "Editar"
            If Not ControlElegido Is Nothing Then
                ControlElegido.BackColor = Color.White
                ControlElegido.ForeColor = Color.Black
            End If
        End If

    End Sub

    Private Sub tbIncremento_Scroll(ByVal sender As System.Object, ByVal e As System.EventArgs)

        IncrementoDesplazamiento = tbIncremento.Value

    End Sub

    Private Sub LlenarComboFuentes()

        Try
            cbFonts.Items.Clear()
            Dim fonts() As String = CargarFuentes()
            For Each font As String In fonts
                cbFonts.Items.Add(font)
            Next

        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try

    End Sub

    Private Sub cbFonts_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbFonts.SelectedIndexChanged

        Try
            If ControlElegido Is Nothing Then
                Exit Sub
            End If
            Dim estilo As New FontStyle
            estilo = IIf(chkBold.Checked = True, FontStyle.Bold, FontStyle.Regular)
            Dim Fuente As New System.Drawing.Font(cbFonts.SelectedItem.ToString, nudSize.Value, estilo)
            ControlElegido.Font = Fuente

        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try

    End Sub

    Private Sub nudSize_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles nudSize.Click

        Try
            If ControlElegido Is Nothing Then
                Exit Sub
            End If
            Dim estilo As New FontStyle
            estilo = IIf(chkBold.Checked = True, FontStyle.Bold, FontStyle.Regular)
            Dim Fuente As New Font(cbFonts.SelectedItem.ToString, nudSize.Value, estilo)
            ControlElegido.Font = Fuente

        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try

    End Sub

    Private Sub chkBold_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkBold.CheckedChanged

        Try
            If ControlElegido Is Nothing Then
                Exit Sub
            End If
            Dim estilo As New FontStyle
            estilo = IIf(chkBold.Checked = True, FontStyle.Bold, FontStyle.Regular)
            Dim Fuente As New System.Drawing.Font(cbFonts.SelectedItem.ToString, nudSize.Value, estilo)
            ControlElegido.Font = Fuente

        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try

    End Sub

End Class