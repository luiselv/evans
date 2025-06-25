Imports System.Data.SqlClient

Public Class frmRptManifiesto

    Private Sub frmRptManifiesto_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Try
            rdManifiesto.Load("Manifiesto.rpt")
            rdManifiesto.DataSourceConnections.Item(0).SetConnection(strServidor, strBD2, True)
            rdManifiesto.SetParameterValue("@mani_codigo", objManifiesto.Codigo)
            crvManifiesto.ReportSource = rdManifiesto

            Dim txtTitulo As CrystalDecisions.CrystalReports.Engine.TextObject = rdManifiesto.ReportDefinition.Sections(1).ReportObjects("itxtTitulo")
            Dim txtTransportista As CrystalDecisions.CrystalReports.Engine.TextObject = rdManifiesto.ReportDefinition.Sections(1).ReportObjects("itxtTransportista")
            Dim txtRUC As CrystalDecisions.CrystalReports.Engine.TextObject = rdManifiesto.ReportDefinition.Sections(1).ReportObjects("itxtRUC")
            Dim txtPlaca As CrystalDecisions.CrystalReports.Engine.TextObject = rdManifiesto.ReportDefinition.Sections(1).ReportObjects("itxtPlaca")
            Dim txtChofer As CrystalDecisions.CrystalReports.Engine.TextObject = rdManifiesto.ReportDefinition.Sections(1).ReportObjects("itxtChofer")
            Dim txtLicencia As CrystalDecisions.CrystalReports.Engine.TextObject = rdManifiesto.ReportDefinition.Sections(1).ReportObjects("itxtLicencia")
            Dim txtDireccion As CrystalDecisions.CrystalReports.Engine.TextObject = rdManifiesto.ReportDefinition.Sections(1).ReportObjects("itxtDireccion")

            Dim txtNroGuiasT As CrystalDecisions.CrystalReports.Engine.TextObject = rdManifiesto.ReportDefinition.Sections(4).ReportObjects("itxtNroGuiasT")
            Dim txtNroGuiasC As CrystalDecisions.CrystalReports.Engine.TextObject = rdManifiesto.ReportDefinition.Sections(4).ReportObjects("itxtNroGuiasC")
            Dim txtPesoT As CrystalDecisions.CrystalReports.Engine.TextObject = rdManifiesto.ReportDefinition.Sections(4).ReportObjects("itxtPesoT")
            Dim txtPesoC As CrystalDecisions.CrystalReports.Engine.TextObject = rdManifiesto.ReportDefinition.Sections(4).ReportObjects("itxtPesoC")
            Dim txtImporteT As CrystalDecisions.CrystalReports.Engine.TextObject = rdManifiesto.ReportDefinition.Sections(4).ReportObjects("itxtImporteT")
            Dim txtImporteC As CrystalDecisions.CrystalReports.Engine.TextObject = rdManifiesto.ReportDefinition.Sections(4).ReportObjects("itxtImporteC")
            Dim txtFirma As CrystalDecisions.CrystalReports.Engine.TextObject = rdManifiesto.ReportDefinition.Sections(4).ReportObjects("itxtFirma")

            Dim nroNuiasT As Integer = 0
            Dim nroNuiasC As Integer = 0
            Dim pesoT As Double = 0.0
            Dim pesoC As Double = 0.0
            Dim importeT As Double = 0.0
            Dim importeC As Double = 0.0

            txtTitulo.Text = "MANIFIESTO DE CARGA " + objManifiesto.Numero.ToString
            txtTransportista.Text = objManifiesto.Transportista.Nombre.ToString
            txtRUC.Text = objManifiesto.Transportista.RUC.ToString
            txtDireccion.Text = objManifiesto.Transportista.Direccion.ToString
            txtPlaca.Text = objManifiesto.Vehiculo.Placa.ToString + " / " + objManifiesto.Carreta.Placa.ToString
            txtChofer.Text = objManifiesto.Chofer.Nombre.ToString
            txtLicencia.Text = objManifiesto.Chofer.Licencia.ToString
            txtFirma.Text = objManifiesto.Chofer.Nombre.ToString

            For i As Integer = 0 To objManifiesto.GuiasSeleccionadas.Count - 1
                If objManifiesto.GuiasSeleccionadas(i).Destino.Codigo = 1 Then
                    nroNuiasT = nroNuiasT + 1
                    pesoT = pesoT + objManifiesto.GuiasSeleccionadas(i).PesoTotal
                    importeT = importeT + objManifiesto.GuiasSeleccionadas(i).CostoTotal
                End If
                If objManifiesto.GuiasSeleccionadas(i).Destino.Codigo = 2 Then
                    nroNuiasC = nroNuiasC + 1
                    pesoC = pesoC + objManifiesto.GuiasSeleccionadas(i).PesoTotal
                    importeC = importeC + objManifiesto.GuiasSeleccionadas(i).CostoTotal
                End If
            Next

            txtNroGuiasT.Text = nroNuiasT
            txtNroGuiasC.Text = nroNuiasC
            txtPesoT.Text = FormatNumber(pesoT, 2)
            txtPesoC.Text = FormatNumber(pesoC, 2)
            txtImporteT.Text = FormatNumber(importeT / (1 + (objParametros.PorcentajeIGV / 100)), 2)
            txtImporteC.Text = FormatNumber(importeC / (1 + (objParametros.PorcentajeIGV / 100)), 2)

        Catch ex As SqlException
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            If objConexion2.State = ConnectionState.Open Then
                objConexion2.Close()
            End If
        End Try

    End Sub
End Class