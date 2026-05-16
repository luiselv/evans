# ADR 0002 — C# para todo el código nuevo

**Fecha**: 2026-05-15
**Estado**: Aceptado

## Contexto

El sistema legacy está en VB.NET. Para el código nuevo (Domain, Application, Infrastructure, Reports, UI forms nuevos) se evaluó:
- **VB.NET**: coherencia con legacy, sin fricción de interop, pero lenguaje feature-frozen en Microsoft
- **C#**: mejor tooling, librerías modernas asumen C#, comunidad activa, AI-assistance optimizada para C#

## Decisión

**C# para todo el código nuevo**. El código VB.NET legacy queda intacto hasta que su bounded context sea migrado. Soluciones mixtas VB+C# son first-class en .NET — compilan y se referencian mutuamente sin fricción.

## Consecuencias

- Code-behind de forms VB legacy puede quedar en VB (no vale el costo de migrar Designer.vb si el form se borrará pronto)
- Nuevos forms se escriben en C# directamente
- Toda la capa de domain, application e infrastructure es C# desde el inicio
- `Option Strict Off` en VB legacy puede causar sorpresas en interop con C# — activar `Option Strict On` en módulos que hagan interop explícito
