# ADR 0004 — Dapper sobre EF Core para acceso a datos

**Fecha**: 2026-05-15
**Estado**: Aceptado

## Contexto

Las queries existentes en los `cls*` legacy son SQL hand-tuned con multi-KB de lógica (joins de 11 tablas, WHERE clauses con reglas de negocio). Opciones:
- **EF Core**: LINQ, migrations, change tracking — pero habría que pelear con `FromSqlRaw` para queries complejas
- **Dapper**: micro-ORM, SQL explícito, materialización automática, scoping de conexión

## Decisión

**Dapper** para repositorios. Las queries SQL existentes se reusan intactas — solo se relocalizan de los `cls*` a los repositorios. Esto es crítico para tax compliance (comportamiento idéntico al legacy).

## Consecuencias

- SQL hand-tuned se mantiene — no hay riesgo de regresión en queries complejas
- `UnitOfWork` gestiona `SqlConnection` + `SqlTransaction` scoped por use case
- `IsolationLevel.Serializable` se preserva para operaciones críticas (Grabar Guia, Grabar Manifiesto)
- No hay migrations — el schema existe en los scripts `01 main.sql` / `02 transactions.sql`
- EF Core puede agregarse más adelante para contexts nuevos si se desea LINQ
