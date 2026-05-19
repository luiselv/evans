# AGENTS.md — EVANS Code Review Rules

## Architecture
- Hexagonal + Vertical Slice: Domain has zero outward dependencies
- No SQL, no UI references in Domain or Application layers
- Application ports are interfaces only — no concrete types from Infrastructure
- IUnitOfWork port exposes only `Commit()` + `IDisposable` — no SqlConnection/SqlTransaction on the port

## C# conventions
- Use `record` for value objects; enforce invariants in constructors (throw `DomainException`)
- Prefer `IReadOnlyList<T>` over `List<T>` on public APIs
- Commands return `Result<T>` — never throw for business validation failures
- No `MessageBox` or UI concerns in Application or Domain layers

## SQL / Dapper
- All writes under `IsolationLevel.Serializable` via `SqlUnitOfWork`
- Never include `grem_serie` or `grem_numero` in UPDATE statements (SUNAT fiscal compliance)
- `GREM_MANIFIESTO=1` maps to `HasManifest=true` — no inversion
- Repository cast from `IUnitOfWork` to `SqlUnitOfWork` is allowed only within the Infrastructure assembly

## Tests
- Domain tests: pure xUnit, no mocks, no DB
- Application tests: NSubstitute for ports; verify call ordering for side effects
- Infrastructure tests: Testcontainers SQL Server; no mocks
- Acceptance tests: full DI container, real or mocked infrastructure as appropriate
