Namespace Microsoft.VisualBasic.PowerPacks.Printing

    ' Stub — reemplaza Microsoft.VisualBasic.PowerPacks.Vs (descontinuado)
    ' Fase 0: permite compilar el legacy. Reemplazar en Fase 2+ con System.Drawing.Printing.
    Public Class PrintForm
        Inherits System.ComponentModel.Component

        Public Sub New()
        End Sub

        Public Sub New(container As System.ComponentModel.IContainer)
            If container IsNot Nothing Then container.Add(Me)
        End Sub

        Public Enum PrintOption
            Scrollable = 0
            ClientAreaOnly = 1
            FullPage = 2
        End Enum

        Public Property DocumentName As String
        Public Property Form As System.Windows.Forms.Form
        Public Property PrintAction As System.Drawing.Printing.PrintAction
        Public Property PrinterSettings As System.Drawing.Printing.PrinterSettings
        Public Property PrintFileName As String

        Public Sub Print(frm As System.Windows.Forms.Form, printOption As PrintOption)
            ' stub — sin implementación funcional
        End Sub

    End Class

End Namespace
