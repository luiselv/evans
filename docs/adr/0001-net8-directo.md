# ADR 0001 — Migración directa a .NET 8 (sin escalón en 4.8)

**Fecha**: 2026-05-15
**Estado**: Aceptado

## Contexto

EVANS corre en .NET Framework 2.0. Necesitamos modernizar la plataforma. Las opciones eran:
- **Path A**: .NET Framework 4.8 primero, luego .NET 8 — más seguro, pero doble trabajo
- **Path B**: .NET 8 directo — más trabajo upfront en bloqueantes, pero sin "hacer dos veces"
- **Path C**: Strangler Fig con app nueva paralela

Bloqueantes identificados para .NET 8:
- Crystal Reports 10.5 → reemplazar con QuestPDF
- `Microsoft.VisualBasic.PowerPacks.PrintForm` → `IDocumentPrinter` + `System.Drawing.Printing`
- Dotnetrix.TabControl 1.0.1.4 → BCL `TabControl` (forms nuevos en C# no lo usan)
- WebBrowser IE-based → `HttpClient` + SUNAT API JSON

## Decisión

Migración directa a **.NET 8 LTS** (luego .NET 10 LTS cuando salga). La solución legacy queda en .NET Framework 4.8 como plataforma de estabilización temporal. La solución nueva se construye en .NET 8 desde el principio.

## Consecuencias

- No hay período de deuda doble (no hay "4.8 en producción mientras se reescribe en 8")
- Cada bloqueante se resuelve antes de migrar su contexto
- El legacy corre en paralelo mientras el nuevo se construye — strangler fig por contexto
- Solo dev, sin deadline duro → riesgo de timeline es bajo
