Public Class frmPrincipal

    Private Sub frmPrincipal_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        If MsgBox("¿Confirma que desea salir del sistema?", MsgBoxStyle.YesNo + MsgBoxStyle.Information, "Salir") = MsgBoxResult.No Then
            e.Cancel = True
        End If
        If objConexion.State = ConnectionState.Open Then
            objConexion.Close()
        End If
        objConexion.Dispose()
    End Sub

    Private Sub btnClientes_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClientes.Click
        ClientesToolStripMenuItem.PerformClick()
    End Sub

    Private Sub TiposDeIdentificaciónToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TiposDeIdentificaciónToolStripMenuItem.Click
        If frmMantTipoID.Visible Then
            frmMantTipoID.Activate()
        Else
            frmMantTipoID.MdiParent = Me
            frmMantTipoID.Show()
        End If
    End Sub

    Private Sub ClientesToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ClientesToolStripMenuItem.Click
        If frmMantCliente.Visible Then
            frmMantCliente.Activate()
        Else
            frmMantCliente.MdiParent = Me
            frmMantCliente.Show()
        End If
    End Sub

    Private Sub ChoferesToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ChoferesToolStripMenuItem.Click

        If frmMantChofer.Visible Then
            frmMantChofer.Activate()
        Else
            frmMantChofer.MdiParent = Me
            frmMantChofer.Show()
        End If

    End Sub

    Private Sub DestinosToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DestinosToolStripMenuItem.Click

        If frmMantDestino.Visible Then
            frmMantDestino.Activate()
        Else
            frmMantDestino.MdiParent = Me
            frmMantDestino.Show()
        End If

    End Sub

    Private Sub EmpresasToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles EmpresasToolStripMenuItem.Click

        If frmMantEmpresa.Visible Then
            frmMantEmpresa.Activate()
        Else
            frmMantEmpresa.MdiParent = Me
            frmMantEmpresa.Show()
        End If

    End Sub

    Private Sub EstadosToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles EstadosToolStripMenuItem.Click

        If frmMantEstado.Visible Then
            frmMantEstado.Activate()
        Else
            frmMantEstado.MdiParent = Me
            frmMantEstado.Show()
        End If

    End Sub

    Private Sub GuíaDeRemisiónToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles GuíaDeRemisiónToolStripMenuItem.Click

        If frmGuiaRemision.Visible Then
            frmGuiaRemision.Activate()
        Else
            frmGuiaRemision.MdiParent = Me
            frmGuiaRemision.Show()
        End If

    End Sub

    Private Sub ComprobantesToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComprobantesToolStripMenuItem.Click

        If frmComprobante.Visible Then
            frmComprobante.Activate()
        Else
            frmComprobante.MdiParent = Me
            frmComprobante.Show()
        End If

    End Sub

    Private Sub ManifiestoToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ManifiestoToolStripMenuItem.Click

        If frmManifiesto.Visible Then
            frmManifiesto.Activate()
        Else
            frmManifiesto.MdiParent = Me
            frmManifiesto.Show()
        End If

    End Sub

    Private Sub TractoresToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TractoresToolStripMenuItem.Click

        If frmMantVehiculo.Visible Then
            frmMantVehiculo.Activate()
        Else
            frmMantVehiculo.MdiParent = Me
            frmMantVehiculo.Show()
        End If

    End Sub

    Private Sub CarretasToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CarretasToolStripMenuItem.Click

        If frmMantCarreta.Visible Then
            frmMantCarreta.Activate()
        Else
            frmMantCarreta.MdiParent = Me
            frmMantCarreta.Show()
        End If

    End Sub

    Private Sub UsuariosToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles UsuariosToolStripMenuItem.Click

        If frmMantUsuarios.Visible Then
            frmMantUsuarios.Activate()
        Else
            frmMantUsuarios.MdiParent = Me
            frmMantUsuarios.Show()
        End If

    End Sub

    Private Sub GuíasPorClienteToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles GuíasPorClienteToolStripMenuItem.Click

        If frmConsGuiasPorCliente.Visible Then
            frmConsGuiasPorCliente.Activate()
        Else
            frmConsGuiasPorCliente.MdiParent = Me
            frmConsGuiasPorCliente.Show()
        End If

    End Sub

    Private Sub EnviarEmailToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles EnviarEmailToolStripMenuItem.Click

        If frmEmail.Visible Then
            frmEmail.Activate()
        Else
            frmEmail.MdiParent = Me
            frmEmail.Show()
        End If

    End Sub

    Private Sub ParámetrosDelSistemaToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ParámetrosDelSistemaToolStripMenuItem.Click
        If frmParametros.Visible Then
            frmParametros.Activate()
        Else
            frmParametros.MdiParent = Me
            frmParametros.Show()
        End If
    End Sub

    Private Sub EnvíoDeEmailToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles EnvíoDeEmailToolStripMenuItem.Click

        If objUsuarioActual.Administrador = 1 Then
            If frmConfigEmail.Visible Then
                frmConfigEmail.Activate()
            Else
                frmConfigEmail.MdiParent = Me
                frmConfigEmail.Show()
            End If
        Else
            MessageBox.Show("Sólo un usuario administrador puede ingresar a esta configuración.", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If

    End Sub

    Private Sub ConsultaDeRUCToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ConsultaDeRUCToolStripMenuItem.Click
        If frmConsultaRUC.Visible Then
            frmConsultaRUC.Activate()
        Else
            frmConsultaRUC.MdiParent = Me
            frmConsultaRUC.Show()
        End If
    End Sub

    Private Sub MostrarBarraDeHerramientasToolStripMenuItem_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles MostrarBarraDeHerramientasToolStripMenuItem.Click

        ToolStrip1.Visible = Not ToolStrip1.Visible

    End Sub

    Private Sub tsbChoferes_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbChoferes.Click
        ChoferesToolStripMenuItem.PerformClick()
    End Sub

    Private Sub tsbDestinos_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbDestinos.Click
        DestinosToolStripMenuItem.PerformClick()
    End Sub

    Private Sub tsbTractores_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbTractores.Click
        TractoresToolStripMenuItem.PerformClick()
    End Sub

    Private Sub tsbCarretas_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbCarretas.Click
        CarretasToolStripMenuItem.PerformClick()
    End Sub

    Private Sub tsbEmpresas_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbEmpresas.Click
        EmpresasToolStripMenuItem.PerformClick()
    End Sub

    Private Sub tsbUsuarios_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbUsuarios.Click
        UsuariosToolStripMenuItem.PerformClick()
    End Sub

    Private Sub tsbGuia_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbGuia.Click
        GuíaDeRemisiónToolStripMenuItem.PerformClick()
    End Sub

    Private Sub tsbComprobante_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbComprobante.Click
        ComprobantesToolStripMenuItem.PerformClick()
    End Sub

    Private Sub tsbManifiesto_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbManifiesto.Click
        ManifiestoToolStripMenuItem.PerformClick()
    End Sub

    Private Sub tsbEmail_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbEmail.Click
        EnviarEmailToolStripMenuItem.PerformClick()
    End Sub

    Private Sub ToolStripButton1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton1.Click
        ConsultaDeRUCToolStripMenuItem.PerformClick()
    End Sub

    Private Sub CerrarTodasLasVentanasToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CerrarTodasLasVentanasToolStripMenuItem.Click

        For Each frm As Form In Me.MdiChildren
            frm.Close()
        Next

    End Sub

    Private Sub EnvíosMensualesToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles EnvíosMensualesToolStripMenuItem.Click
        If frmConsEnviosMensuales.Visible Then
            frmConsEnviosMensuales.Activate()
        Else
            frmConsEnviosMensuales.MdiParent = Me
            frmConsEnviosMensuales.Show()
        End If
    End Sub

    Private Sub ReporteDeVentasToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ReporteDeVentasToolStripMenuItem.Click
        If frmReporteVentas.Visible Then
            frmReporteVentas.Activate()
        Else
            frmReporteVentas.MdiParent = Me
            frmReporteVentas.Show()
        End If
    End Sub

    Private Sub RecepciónToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RecepciónToolStripMenuItem.Click
        If frmRecepcion.Visible Then
            frmRecepcion.Activate()
        Else
            frmRecepcion.MdiParent = Me
            frmRecepcion.Show()
        End If
    End Sub

    Private Sub tsbRecepcion_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbRecepcion.Click
        RecepciónToolStripMenuItem.PerformClick()
    End Sub
End Class
