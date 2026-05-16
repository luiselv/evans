# ADR 0005 — QuestPDF sobre Crystal Reports

**Fecha**: 2026-05-15
**Estado**: Aceptado

## Contexto

El sistema usa Crystal Reports 10.5 (1 archivo .rpt: Manifiesto.rpt) y `Microsoft.VisualBasic.PowerPacks.PrintForm` para `frmImprimir*`. Crystal Reports 10.5 no corre en .NET 8. PowerPacks tampoco. Opciones:
- **Crystal Reports for VS 13.x**: mantiene el .rpt, pero licencias SAP complejas y NO funciona en .NET 8/Core
- **FastReport / Telerik Reporting**: comerciales, con diseñador visual, costo de licencia
- **QuestPDF**: MIT license, C# fluent API, genera PDF, soporta .NET 8, activo community

## Decisión

**QuestPDF (community edition, MIT)** para generación de PDFs. Reemplaza Crystal Reports (1 .rpt) y PrintForm (todos los `frmImprimir*`).

Se introduce `IDocumentPrinter` como port en `EVANS.Application`. La implementación usa QuestPDF para PDF + `System.Drawing.Printing` para impresión física en impresora.

## Consecuencias

- Tax compliance risk: los documentos deben ser visualmente equivalentes al legacy. Mitigación: visual regression tests con PDFs "golden" comparados pixel-a-pixel (con Verify)
- Sin licencias propietarias
- Permite migración futura a .NET 10 sin fricción
- Curva de aprendizaje: QuestPDF requiere escribir el layout en C# fluent (no drag-and-drop como Crystal)
