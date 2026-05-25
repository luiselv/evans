# EVANS — Plan de Upgrade de Arquitectura

## Context

EVANS es un sistema de gestión de transporte de carga en producción (Perú) escrito en VB.NET 2.0 / .NET Framework 2.0 / Windows Forms. El estado actual presenta acoplamiento severo entre UI, lógica de negocio y acceso a datos:

- **`modMetodos.vb`** es un "god module" con 2 conexiones SQL globales mutables, singletons de clases de negocio, flags globales, manipulación directa de controles de forms por nombre, helpers de SQL con concatenación de strings, P/Invoke a kernel32 y ~1184 LOC mezclando 7 responsabilidades.
- Las clases `cls*.vb` son **Active Record** con propiedades + CRUD inline (SQL de varios KB con reglas de negocio en WHERE clauses), transacciones internas hardcoded en `IsolationLevel.Serializable`, y fugas de `SqlDataReader` hacia los forms.
- Hay **301 hits de SQL en 21 forms** (Lectura/escritura directa desde el code-behind). Los forms usan instancias por defecto de VB (singletons por convención de lenguaje).
- **Cero abstracciones**: 0 interfaces, 0 clases base, 0 DI, 0 repositories, 0 tests automatizados.
- **Dependencias bloqueantes para .NET 8**: Crystal Reports 10.5 (1 .rpt), `Microsoft.VisualBasic.PowerPacks.PrintForm` (todos los `frmImprimir*`), Dotnetrix.TabControl 1.0.1.4 (24 forms), WebBrowser IE-based (consulta RUC SUNAT).

**Decisiones tomadas con el usuario** (senior architect, solo dev, sin deadline duro):
- **Runtime**: .NET 8/10 directo (no escalón intermedio en 4.8)
- **Lenguaje**: C# para todo el código nuevo. VB queda en los forms legacy hasta que cada slice se reescriba
- **Reportes**: Reemplazar Crystal Reports con QuestPDF
- **Estrategia**: Strangler Fig a nivel de aplicación — solución legacy y nueva conviven hasta cutover por contexto

**Outcome esperado**: Sistema testeable (cobertura útil de unit + integration + acceptance), escalable (Hexagonal + Vertical Slices por bounded context), moderno (.NET 8 LTS, luego .NET 10 LTS cuando salga), y mantenible (sin globales, sin Active Record, sin code-behind con SQL).

---

## Modo de Ejecución

**Este plan se persiste en el proyecto** como `C:\development\EVANS\docs\UPGRADE_PLAN.md` y es la guía oficial de implementación. Antes de tocar código, ese archivo debe existir.

**Cadencia por fase** (no negociable):
1. Anunciar inicio de fase con su scope concreto
2. Ejecutar los entregables de la fase
3. Correr la verificación de la fase (sección "Verificación end-to-end")
4. Reportar resultados de la verificación al usuario
5. **STOP**: esperar confirmación explícita del usuario para pasar a la siguiente fase
6. Sin confirmación → no se arranca la fase siguiente, no se hacen "adelantos"

Esta disciplina existe porque cada fase tiene riesgo de regresión en producción (tax compliance, integridad transaccional). El usuario es solo dev y necesita visibilidad real de qué cambió antes de seguir.

---

## Arquitectura Objetivo

```
┌──────────────────────────────────────────────────────────────────────┐
│ EVANS.Host.WinForms (.NET 8, C#)                                     │
│  - Program.cs (composition root, Generic Host)                      │
│  - frmPrincipal (MDI shell, INavigationService)                     │
└──────────────────────────┬───────────────────────────────────────────┘
   ┌───────────────────────┼───────────────────────────────────────┐
   ▼                       ▼                                       ▼
┌─────────────────┐  ┌─────────────────────┐         ┌────────────────────┐
│ EVANS.UI.WinForms│  │ EVANS.UI.Shared     │         │ EVANS.Reports      │
│ (C#, forms por  │  │  (C#)                │         │ (C#, QuestPDF,     │
│  bounded context)│  │  - ViewModels        │         │  IDocumentPrinter) │
└────────┬─────────┘  │  - INavigationService│         └────────────────────┘
         │            │  - IDialogService    │
         │            │  - IMessenger        │
         │            └──────────┬──────────┘
         │                       │
         ▼                       ▼
┌───────────────────────────────────────────────────────────────────────┐
│ EVANS.Application (C#, vertical slices por contexto)                  │
│   GuiaRemision/  Manifiesto/  Comprobante/  Recepcion/                │
│   Catalogo/      Identidad/                                           │
│   - Commands / Queries (MediatR-style)                                │
│   - Validators (FluentValidation)                                     │
│   - Ports: IGuiaRepository, INumeradorService, ISunatRucService...    │
└───────────────────────────┬───────────────────────────────────────────┘
                            ▼
┌───────────────────────────────────────────────────────────────────────┐
│ EVANS.Domain (C#, puro, sin SQL ni UI)                                │
│   - Entidades POCO: Guia, Manifiesto, Comprobante, Cliente...        │
│   - Value Objects: Money, Ruc, Dni, NumeroGuia(Serie, Numero)         │
│   - Domain services: GeneradorNumeroDocumento                         │
│   - Invariantes en constructores                                      │
└───────────────────────────────────────────────────────────────────────┘
              ▲                                       ▲
┌─────────────┴───────────────┐         ┌─────────────┴────────────────┐
│ EVANS.Infrastructure.Sql    │         │ EVANS.Infrastructure.External│
│  (C#, Dapper)                │         │  (C#)                        │
│  - Repositories (Dapper)     │         │  - ISunatRucService          │
│  - UnitOfWork (SqlConnection │         │   (HttpClient + JSON)        │
│   scope-per-use-case)        │         │  - SmtpClient adapter        │
│  - YearlyDbResolver          │         │  - Excel exporter (ClosedXML)│
└─────────────────────────────┘         └──────────────────────────────┘

┌───────────────────────────────────────────────────────────────────────┐
│ EVANS.Legacy.WinForms (.NET Framework 4.8, VB.NET)                    │
│   El código actual, congelado salvo bugfixes críticos.                │
│   Conectado a la MISMA SQL Server. Vive en paralelo durante el cutover│
│   por contexto. Se retira al final.                                   │
└───────────────────────────────────────────────────────────────────────┘

┌───────────────────────────────────────────────────────────────────────┐
│ TESTS (C#, xUnit + NSubstitute + FluentAssertions)                    │
│   EVANS.Domain.Tests          (puro, sin DB) — milisegundos          │
│   EVANS.Application.Tests     (con substitutes, sin DB)               │
│   EVANS.Infrastructure.Tests  (Testcontainers SQL Server + Respawn)   │
│   EVANS.Acceptance.Tests      (end-to-end, mismos containers)         │
└───────────────────────────────────────────────────────────────────────┘
```

**¿Por qué Hexagonal + Vertical Slices y no DDD táctico completo o pure Onion?**
- Screaming Architecture a nivel de carpeta: el dominio grita "transporte de carga" en cada slice
- Hexagonal en el borde: Domain no conoce SQL, WinForms ni QuestPDF
- Vertical Slices dentro de Application: `CrearGuia`, `GenerarManifiesto`, `ImprimirBoleta` son commands/queries independientes
- NO event sourcing — overkill para un TMS de este tamaño con un solo dev

---

## Fases

Cada fase debe dejar el sistema **shippable** y con **safety net de tests**. La fase 0 es no negociable: sin tests no hay refactor seguro.

### Fase 0 — Safety net sobre el legacy (Semanas 1–3)

**Objetivo**: tener red de regresión ANTES de tocar nada. Esto se hace SOBRE el legacy 4.8, no sobre código nuevo.

Entregables:
1. **Migrar `EVANS.vbproj` a SDK-style sobre .NET Framework 4.8** (no .NET 8 todavía). Esto es solo para tener: PackageReference, build moderno en Visual Studio 2022, y compatibilidad con herramientas modernas. Eliminar la referencia muerta `System.Web.Services 2.0`.
2. **Renombrar la solución actual** a `EVANS.Legacy.sln`. Mover código a carpeta `legacy/`.
3. **Setup CI**: GitHub Actions o Azure DevOps. Build sobre `windows-latest`. Cache NuGet.
4. **Setup test infrastructure**:
   - Proyecto `EVANS.Legacy.AcceptanceTests` (xUnit)
   - Testcontainers para SQL Server (`mcr.microsoft.com/mssql/server:2019-latest`)
   - Respawn para limpieza entre tests
   - Aplicar `scripts/01 main.sql` y `scripts/02 transactions.sql` al levantar fixture
5. **Escribir 8 acceptance tests "golden-path"** contra el legacy:
   - Crear Guía → Grabar → BuscarXCodigo → campos coinciden
   - Crear Manifiesto con 2 Guías → Grabar → totales correctos + numerador incrementa
   - Generar Boleta → Grabar → numerador incrementa
   - Generar Factura → Grabar
   - Crear Recepción → Generar Guía desde Recepción → `bolGenerandoGuia` produce update en `Recepcion`
   - Buscar Cliente por nombre devuelve filas (verifica `SqlDataReader` patrón)
   - Crear/Listar Cliente (CRUD completo)
   - Login + Autenticar setea `objUsuarioActual` correctamente
6. **F0-DB: Endurecimiento aditivo de BDs anuales transaccionales** — las BDs anuales (`2010`, `2014`, `2019`, `2026`…) tienen CERO PKs/índices en producción (causa raíz: `CrearBD()` usa `SELECT TOP 0 * INTO` que no copia constraints). Entregables:
   - `db/schema-yearly.sql`: PKs sobre columnas IDENTITY existentes + índices sobre joins y filtros frecuentes. Solo aditivo — sin tocar tipos, nullability ni queries (ADR 0003 intacto).
   - `db/indexes-yearly.sql`: script idempotente (`IF NOT EXISTS`) para aplicar los mismos constraints sobre BDs históricas ya existentes sin recrearlas ni mover datos.
   - `EVANS\modMetodos.vb` `CrearBD()`: agregar `ALTER TABLE ... ADD CONSTRAINT PK_*` y `CREATE NONCLUSTERED INDEX IX_*` después del clone por `SELECT TOP 0 * INTO`, para que cada BD de año nuevo nazca con PKs e índices.
   - 3 tests en `EVANS.Tests/LegacySchemaTests.cs` que verifican PKs e índices en `sys.key_constraints` / `sys.indexes` dentro del contenedor Testcontainers.

**Acceptance**: build verde en CI, 8 tests pasando contra SQL Server en Docker, ADRs (Architecture Decision Records) iniciales escritos en `docs/adr/`, 3 tests de schema pasando en EVANS.Tests.

**Riesgo**: la migración a SDK-style puede romper la regeneración de `Designer.vb` para los 24 forms que usan Dotnetrix.TabControl. **Mitigación**: smoke test (abrir cada form en el designer) antes de mergear.

### Fase 1 — Bootstrap de la solución nueva (Semanas 4–5)

**Status**: ✅ Complete — 2026-05-18

**Objetivo**: tener la estructura objetivo vacía y compilando, sin código de negocio todavía.

Entregables:
1. **Crear `EVANS.sln`** (nueva solución, .NET 8) con todos los proyectos C# vacíos según el diagrama:
   - `EVANS.Domain`
   - `EVANS.Application`
   - `EVANS.Infrastructure.Sql` (Dapper, Microsoft.Data.SqlClient)
   - `EVANS.Infrastructure.External`
   - `EVANS.Reports` (QuestPDF)
   - `EVANS.UI.Shared`
   - `EVANS.UI.WinForms`
   - `EVANS.Host.WinForms`
   - Tests: `EVANS.Domain.Tests`, `EVANS.Application.Tests`, `EVANS.Infrastructure.Tests`, `EVANS.Acceptance.Tests`
2. **Configurar composition root** en `Program.cs` usando `Microsoft.Extensions.Hosting` 8.x:
   ```csharp
   using var host = Host.CreateDefaultBuilder()
       .ConfigureServices((ctx, services) =>
       {
           services.AddEvansDomain();
           services.AddEvansApplication();
           services.AddEvansInfrastructureSql(ctx.Configuration);
           services.AddEvansInfrastructureExternal(ctx.Configuration);
           services.AddEvansWinFormsShell();
       })
       .Build();
   Application.EnableVisualStyles();
   Application.Run(host.Services.GetRequiredService<frmPrincipal>());
   ```
3. **Implementar `IDbConnectionFactory`** con dos factories tipadas:
   - `IEvansMasterConnectionFactory` (DB `EVANS`)
   - `IYearlyTransactionalConnectionFactory` (DB `YYYY` — recibe año como parámetro)
   - Connection strings desde `appsettings.json` con `Microsoft.Extensions.Configuration.Json`
4. **MDI shell vacía**: `frmPrincipal` con menú estructural (sin handlers todavía). Compila y arranca con DB de prueba.
5. **ADR**: documentar la decisión "rewrite-then-cutover via strangler fig por contexto".

**Acceptance**: `EVANS.sln` compila, arranca, conecta a SQL Server local, muestra MDI shell vacía con menú. CI corre ambas soluciones.

### Fase 2 — Primer slice end-to-end: GuiaRemision (Semanas 6–11)

**Status**: ✅ Complete — 2026-05-19 | 83 tests green (30 Domain + 17 Application + 12 Infra + 2 UI + 7 Reports + 5 Acceptance + 10 Legacy) | 0 CRITICALs | SDD archived

**Objetivo**: implementar el contexto MÁS COMPLEJO primero como template para todos los demás. Si esto sale bien, los otros 5 contextos siguen el mismo patrón mecánicamente.

¿Por qué GuiaRemision primero?
- 842 LOC en `clsGuiaRemision`, 1214 LOC en `frmGuiaRemision`, 33 SQL hits inline en el form
- `Grabar()` muestra TODOS los antipatterns: efecto cruzado (`Parametros` numerador), escritura condicional cross-aggregate (`Recepcion`), transacción Serializable interna, MessageBox en el catch
- Si esto se puede extraer limpio, todos los demás son estrictamente más fáciles

Entregables:

1. **Domain** (`EVANS.Domain/GuiaRemision/`):
   - `Guia` (POCO con invariantes en constructor)
   - `DetalleGuia`
   - Value objects: `NumeroGuia(Serie, Numero)`, `Direccion`, `Peso`
   - Sin SQL, sin UI, sin referencias externas

2. **Application** (`EVANS.Application/GuiaRemision/`):
   - Commands: `CrearGuiaCommand`, `ActualizarGuiaCommand`, `EliminarGuiaCommand`
   - Queries: `ObtenerGuiaPorCodigoQuery`, `BuscarGuiasQuery`
   - Handlers que orquestan: el flag `bolGenerandoGuia` legacy se convierte en input tipado `OrigenGuia.DesdeRecepcion(recepcionId)` del command
   - Ports:
     - `IGuiaRepository` — devuelve entidades de dominio, NUNCA `SqlDataReader`
     - `INumeradorService` — saca la actualización de `Parametros` afuera del aggregate
     - `IRecepcionVinculadaService` — el efecto cruzado a `Recepcion` se hace explícito en el handler
   - Validators con FluentValidation

3. **Infrastructure** (`EVANS.Infrastructure.Sql/Repositories/`):
   - `GuiaRepositoryDapper` — usa Dapper, MANTIENE la SQL existente (tax compliance importa)
   - `UnitOfWork` — abre `SqlConnection` + `SqlTransaction(IsolationLevel.Serializable)`, dispose al final del use case
   - El flujo "Crear Guía desde Recepción" hace los 3 writes (Guia, Parametros++, Recepcion update) dentro de UNA transacción explícita en el handler

4. **UI** (`EVANS.UI.WinForms/GuiaRemision/`):
   - `frmGuiaRemisionNew` en **C#** (nuevo form, NO migrar el VB designer)
   - Code-behind: solo construye el command desde controles, envía vía MediatR, maneja result
   - Sin `SqlCommand`, sin `objConexion`, sin `MessageBox` en catch del dominio

5. **Reports** (`EVANS.Reports/GuiaRemision/`):
   - `GuiaPdfRenderer` usando QuestPDF — reemplaza el PrintForm del `frmImprimirGuia`
   - `IDocumentPrinter` con implementación basada en `System.Drawing.Printing` para impresión física

6. **Tests**:
   - Domain: 25+ tests de invariantes (peso > 0, fechas válidas, etc.) — corren en ms
   - Application: 15+ tests de handlers con substitutes (incluye flujo desde Recepción)
   - Integration: 10+ tests de `GuiaRepositoryDapper` contra Testcontainers
   - Acceptance: 3 tests end-to-end (crear, actualizar, buscar)
   - Visual regression: 5 PDFs "golden" comparados pixel-a-pixel contra el output legacy (con tolerancia mínima)

7. **Cutover**:
   - Feature flag en `frmPrincipal` legacy: si flag activo, el menú "Guía Remisión" ejecuta el `EVANS.Host.WinForms` nuevo (proceso separado por ahora). Si no, abre el legacy.
   - **Validación paralela durante 2 semanas**: ambas implementaciones disponibles, comparar outputs en tax-critical workflows

**Acceptance**: contexto `GuiaRemision` totalmente funcional en el nuevo stack, 55+ tests verdes, PDFs equivalentes a Crystal/PrintForm legacy. Legacy `clsGuiaRemision.vb` queda `<Obsolete>` pero todavía vivo.

### Fase 3 — Replicar el patrón en los contextos restantes (Semanas 12–25)

Cada contexto sigue el template de Fase 2. Orden por dificultad descendente:

| Contexto | Semanas | Notas |
|---|---|---|
| Comprobante (Boleta/Factura) | 3 | **Status**: ✅ Complete — 2026-05-19 | 186 tests green | 0 CRITICALs | SDD archived |
| Manifiesto | 2.5 | **Status**: ✅ Complete — 2026-05-22 | 217 tests green | 0 CRITICALs | SDD archived |
| Recepcion | 2 | Acoplado con Guia — ya se preparó el port en Fase 2 |
| Catalogo (Cliente, Chofer, Vehiculo, Carreta, Destino, Empresa, Estado, TipoID, Agencia) | 3.5 | CRUD repetitivo — usar `IRepository<T>` genérico para acelerar |
| Identidad (Usuario, Acceso, Parametros) | 1.5 | Incluye rewrite del login flow + reemplazo de `frmConsultaRUC` WebBrowser con `HttpClient` + SUNAT API JSON |
| Reportes/Consultas (ConsEnviosMensuales, ConsGuiasPorCliente, ReporteVentas) | 1.5 | CQRS read side puro — más simple. Excel export con ClosedXML reemplaza Office Interop |

**Durante esta fase**:
- Cada contexto migrado retira sus equivalentes legacy (delete del form + cls + entradas en `modMetodos`)
- Cuando el último consumer de `modMetodos.vb` desaparece, se borra el archivo. PR titulado "RIP modMetodos.vb"
- WebBrowser SUNAT → `ISunatRucService` con HttpClient + JSON (proveedor público) + fallback manual
- PowerPacks PrintForm → `IDocumentPrinter` con `System.Drawing.Printing`
- Dotnetrix.TabControl → BCL `TabControl` o `Krypton.Toolkit` (gratis, soporta .NET 8)
- Excel Interop → ClosedXML (puro .NET, sin Office)

**Acceptance**: legacy solution borrada, .NET 8 / C# es el único stack, 400+ unit tests + 80+ integration tests + 30+ acceptance tests, todo corre en CI en menos de 5 minutos.

### Fase 4 — Endurecer y consolidar (Semanas 26–28)

Una vez que todo está en .NET 8 con tests:
1. Performance pass: medir y optimizar queries Dapper (los índices base de las BDs anuales se agregaron en Fase 0 — F0-DB)
2. Logging estructurado con Serilog + sinks (archivo local + opcional Seq para desarrollo)
3. Telemetría con OpenTelemetry (preparado para producción cuando haga falta)
4. Migración opcional a .NET 10 LTS (cuando salga, ~noviembre 2026) — debería ser cambio de TargetFramework + verificar tests
5. Documentación: README, ADRs completos, runbook de deploy

**Acceptance**: producto endurecido, observabilidad básica, listo para crecimiento.

### Fase 5 (opcional, on demand) — Modernización de UI

Solo si hay razón de negocio:
- WPF (más control que WinForms, sigue siendo desktop Windows)
- Avalonia (cross-platform si interesa Linux/Mac)
- Blazor Hybrid (si quieren reuso con web futura)

Con Hexagonal ya en lugar, el cambio de UI es 2–3 meses, no 18.

---

## Matriz de Decisiones

| Decisión | Elección | Razón |
|---|---|---|
| Runtime | **.NET 8 LTS** (luego .NET 10 cuando salga) | Decisión del usuario. Salto directo aceptable porque hay strangler fig por contexto y no hay deadline |
| Lenguaje código nuevo | **C#** | Mejor tooling, librerías modernas asumen C# |
| Forms legacy | **Quedan en VB.NET hasta retirar** | No vale el costo de migrar Designer.vb si el form se va a borrar pronto |
| ORM | **Dapper** | Mantiene el SQL hand-tuned existente (multi-KB queries con reglas de negocio); EF Core sería pelear con `FromSqlRaw` |
| DI | **Microsoft.Extensions.DependencyInjection 8.x** | Estándar moderno, ya viene con Generic Host |
| Mediator | **MediatR** | Patrón estándar para vertical slices; alternativa: handlers manuales con factory si se quiere evitar la dependencia |
| Tests | **xUnit + NSubstitute + FluentAssertions + Testcontainers + Respawn + Verify** | Stack moderno completo; Verify para snapshot de PDFs |
| Reportes | **QuestPDF (MIT)** | Reemplaza Crystal (1 .rpt) y PrintForm (varios `frmImprimir*`) |
| Excel export | **ClosedXML** | Sin dependencia de Office instalado |
| RUC SUNAT | **HttpClient + JSON API público** | Reemplaza WebBrowser IE-based |
| TabControl | **BCL TabControl** primero; si insuficiente, Krypton.Toolkit | Sin licencias, mantenido |
| Logging | **Serilog** | Estructurado, sinks variados |

---

## El problema más difícil: matar `modMetodos.vb`

En la solución nueva, las 7 responsabilidades del god module se distribuyen así:

| # | Responsabilidad legacy | Nuevo hogar | Cuándo |
|---|---|---|---|
| 1 | Conexiones globales (`objConexion`, `objConexion2`) | `IDbConnectionFactory` + factories tipadas en `Infrastructure.Sql` | Fase 1 |
| 2 | SQL helpers (`BuscarCodigo`, `LLenarCombo`, `MostrarGuia`, `MostrarComprobante`) | `IXxxRepository` por aggregate; queries específicas en handlers | Fase 2 (Guia), Fase 3 (resto) |
| 3 | UI helpers (`DesactivarControles`, `LimpiarControles`, `EsperarMouse`) | `EVANS.UI.Shared.UiHelpers` (C#, extensiones para WinForms) | Fase 1 |
| 4 | Singletons de negocio (`objUsuarioActual`, `objGuia`, `objComprobante`) | `ICurrentSession` (singleton-scoped); contexts por command | Fase 2+ |
| 5 | Flags globales (`bolGenerandoGuia`, `bolGenerandoComprobante`) | Inputs tipados en command (`OrigenGuia.DesdeRecepcion(id)`) | Fase 2+ |
| 6 | State del visual designer (`ControlElegido`, `ControlAnterior`, `IncrementoDesplazamiento`) | Estado scoped en `PrintDesignerSession` o eliminado al rehacer printing | Fase 3 (Comprobante/Guia) |
| 7 | P/Invoke `SetProcessWorkingSetSize` | **Eliminar.** En .NET 8 con GC moderno es cargo cult y potencialmente dañino | Fase 1 |

El legacy `modMetodos.vb` queda intacto en `EVANS.Legacy.sln` durante todo el cutover. Al borrarse el último contexto legacy, se elimina junto con la solución.

---

## Estrategia de Testing

**Pirámide objetivo al finalizar Fase 3**:
- ~400 unit tests (Domain + Application, sin DB) — corren en segundos
- ~80 integration tests (Infrastructure con Testcontainers) — corren en ~1 min
- ~30 acceptance tests (end-to-end) — corren en ~2 min
- Total CI: < 5 min

**Stack final**:
- **xUnit** — runner (paralelo por defecto)
- **NSubstitute** — substitutes (sintaxis más limpia que Moq)
- **FluentAssertions** — asserts legibles
- **Testcontainers** — SQL Server en Docker para integration
- **Respawn** — reset de DB entre tests (faster que recrear)
- **Verify** — snapshot tests para PDFs generados (visual regression crítico para tax compliance)

**Reglas**:
- Domain tests: SIEMPRE puros, sin mocks, sin DB. Si necesitás un mock para testear Domain, mal modelaste.
- Application tests: con substitutes para los ports. Verifican orquestación.
- Infrastructure tests: contra DB real (Testcontainers). Verifican queries SQL y mapping Dapper.
- Acceptance tests: caja negra desde la API de Application. Sirven de safety net durante refactor.

---

## Riesgos y Mitigaciones

| Riesgo | Probabilidad | Impacto | Mitigación |
|---|---|---|---|
| Regresión tax/legal en Boleta/Factura/Guía impresa | Media | Crítico | Visual regression con PDFs golden, validación paralela legacy vs nuevo por 2 semanas por contexto |
| Cambio de comportamiento en `Grabar()` reescrito (orden de side effects, isolation) | Media | Alto | Fase 2 MANTIENE la SQL exacta inicialmente, solo relocaliza; integration tests verifican atomicidad del numerador |
| QuestPDF vs Crystal: deltas visuales | Media | Medio | Pixel-diff testing; mantener exportador legacy detrás de feature flag una release después del cutover |
| SUNAT API: cambios o rate limit | Media | Bajo | Cache local de RUCs consultados; fallback a entrada manual; múltiples proveedores detrás de `ISunatRucService` |
| Dotnetrix.TabControl no portea | Bajo | Medio | Forms nuevos en C# no lo usan; los forms legacy quedan en 4.8 hasta retirar |
| Bug de `Actualizar` en `clsManifiesto.vb:337` (variable `intCodigo` no declarada) | Alto | Bajo | Ya identificado durante la exploración — fix puntual en Fase 0 (no requiere refactor) |
| Burnout / falta de progreso visible | Media | Alto | Cada fase debe terminar con algo demoable. Mantener changelog con wins semanales |
| .NET 8 LTS termina soporte (nov 2026) durante el plan | Bajo | Bajo | Plan estima ~28 semanas (~7 meses) — termina mucho antes. Si demora, saltar a .NET 10 LTS (esperado nov 2026) |

---

## Archivos críticos

**Legacy (referencia y testing)**:
- `C:\development\EVANS\EVANS\EVANS.vbproj` — Migrar a SDK-style en Fase 0
- `C:\development\EVANS\EVANS\modMetodos.vb` — God module a desmontar
- `C:\development\EVANS\EVANS\clsGuiaRemision.vb` — Template de Active Record a reemplazar
- `C:\development\EVANS\EVANS\frmGuiaRemision.vb` — Form con 33 SQL hits, modelo del problema
- `C:\development\EVANS\EVANS\clsManifiesto.vb` — Tiene bug en línea 337 (variable no declarada)
- `C:\development\EVANS\EVANS\Manifiesto.rpt` — Único Crystal Report, a reemplazar con QuestPDF
- `C:\development\EVANS\EVANS\frmConsultaRUC.vb` — WebBrowser SUNAT a reemplazar con HttpClient
- `C:\development\EVANS\scripts\01 main.sql` — Schema base de la DB EVANS
- `C:\development\EVANS\scripts\02 transactions.sql` — Schema de DB anual

**Nuevos (a crear)**:
- `C:\development\EVANS\EVANS.sln` — Solución nueva en .NET 8
- `C:\development\EVANS\src\EVANS.Domain\` — POCO entities + value objects
- `C:\development\EVANS\src\EVANS.Application\GuiaRemision\` — Primer slice end-to-end
- `C:\development\EVANS\src\EVANS.Infrastructure.Sql\Repositories\` — Dapper repositories
- `C:\development\EVANS\src\EVANS.Reports\` — QuestPDF templates
- `C:\development\EVANS\src\EVANS.Host.WinForms\Program.cs` — Composition root
- `C:\development\EVANS\tests\EVANS.Acceptance.Tests\` — Acceptance tests (Fase 0 sobre legacy, Fase 2+ sobre nuevo)
- `C:\development\EVANS\docs\adr\` — Architecture Decision Records

---

## Verificación end-to-end

**Por fase**:
- **Fase 0**: `dotnet test EVANS.Legacy.AcceptanceTests` corre verde contra Testcontainers SQL Server. CI ejecuta en GitHub Actions. 8 acceptance tests pasan.
- **Fase 1**: `dotnet run --project EVANS.Host.WinForms` levanta la MDI shell vacía, conecta a `appsettings.Development.json` con SQL Server local, muestra menú.
- **Fase 2**: usuario puede crear, buscar y modificar una Guía de Remisión usando el nuevo stack. Visual regression de PDF impreso vs legacy < 1% delta. 55+ tests verdes.
- **Fase 3**: cada contexto migrado tiene su propio conjunto de tests (>40 cada uno). El menú legacy de `frmPrincipal` redirige al nuevo host. Al final de la fase, `EVANS.Legacy.sln` se borra.
- **Fase 4**: app .NET 8 corriendo en producción, logs estructurados en archivo, métricas básicas.

**Smoke test manual end-to-end** (cualquier fase post-cutover):
1. Login con usuario de prueba
2. Buscar cliente por nombre
3. Crear Guía de Remisión con 2 detalles
4. Generar Manifiesto incluyendo esa Guía
5. Generar Boleta para esa Guía
6. Imprimir los 3 documentos
7. Exportar consulta a Excel
8. Logout

Si los 8 pasos funcionan sin error, la fase está OK.
