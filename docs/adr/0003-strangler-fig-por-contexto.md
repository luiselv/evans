# ADR 0003 — Strangler Fig a nivel de aplicación por bounded context

**Fecha**: 2026-05-15
**Estado**: Aceptado

## Contexto

El sistema legacy tiene 6 bounded contexts: GuiaRemision, Manifiesto, Comprobante, Recepcion, Catalogo, Identidad. Opciones de migración:
- **Big bang rewrite**: alto riesgo, sistema offline durante rewrite
- **Strangler Fig a nivel de archivo**: migrar cls* y frm* uno por uno en la misma solución — messy
- **Strangler Fig a nivel de aplicación**: dos soluciones paralelas (`EVANS.Legacy.sln` y `EVANS.sln`), misma DB, cutover por contexto

## Decisión

**Strangler Fig a nivel de aplicación**. La solución legacy (`EVANS.Legacy.sln`) se congela (solo bugfixes críticos). La solución nueva (`EVANS.sln`) se construye context por context. Al completar cada contexto, el legacy equivalente se retira.

Ambas soluciones comparten la misma SQL Server (EVANS master + YYYY anual). No hay migración de datos.

## Consecuencias

- Sistema siempre shippable durante la transición
- Cutover por contexto minimiza riesgo (un contexto a la vez)
- La solución legacy puede coexistir meses hasta que todo esté migrado
- GuiaRemision va primero — es el contexto más complejo y sienta el template
- Al terminar el último contexto, `EVANS.Legacy.sln` se elimina
