# ADR 0006 — Nombre canónico del shell y firma de extensiones de configuración

**Status**: Accepted  
**Date**: 2026-05-18

## Contexto

Durante la implementación de Phase 1 (bootstrap .NET 8), surgieron tres decisiones de naming y firma que divergían del UPGRADE_PLAN.md original.

## Decisiones

### 1. Nombre canónico del shell: `frmPrincipal`

El plan original usaba `frmPrincipalNew` como nombre provisional para el nuevo form MDI. Se decidió usar `frmPrincipal` directamente.

**Razón**: El prefijo `New` es un anti-patrón temporal. El form reemplazará al legacy; una vez retirado el legacy, `frmPrincipal` es el nombre correcto sin sufijos de transición.

**Alternativas descartadas**: `frmPrincipalNew` (provisional), `MainForm` (rompe convención húngara del proyecto).

### 2. Parámetro `IConfiguration` explícito en extensiones de infraestructura

`AddEvansInfrastructureSql` y `AddEvansInfrastructureExternal` reciben `IConfiguration configuration` como parámetro explícito, aunque en Phase 1 el parámetro no se usa.

**Razón**: El composition root debe ser legible como una lista de intenciones. `AddEvansInfrastructureSql(ctx.Configuration)` señala explícitamente que esta capa necesita configuración. Un param implícito (vía DI interno) oculta esa dependencia.

**Alternativas descartadas**: Inyectar `IConfiguration` internamente vía DI (oscurece la dependencia), omitir hasta Phase 2 (crea un breaking change posterior).

### 3. `AddEvansWinFormsShell()` vive en `EVANS.Host.WinForms`

La extensión que registra el shell vive en el composition root, no en `EVANS.UI.WinForms`.

**Razón**: Es una decisión de composición, no de UI. `EVANS.UI.WinForms` no debe conocer el contenedor DI.

## Consecuencias

- `UPGRADE_PLAN.md` debe actualizarse para reflejar `frmPrincipal` (no `frmPrincipalNew`).
- `EVANS.Acceptance.Tests` requiere `net8.0-windows` (referencia `Host.WinForms` transitivamente): CI debe usar Windows runner.
- El parámetro `configuration` en Phase 1 tiene `_ = configuration;` para suprimir CA1801.
