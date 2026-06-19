# UI parity audit for migrated WinForms screens

This audit tracks whether migrated C# WinForms screens replicate the legacy VB UI in design, behavior, and operation. The current finding is direct: earlier migrations were mostly functional/architectural, not strict UI parity migrations.

## Current decision

Do not retire any additional legacy form unless its migrated replacement has an explicit parity check against the legacy designer and behavior.

## Audit method

This first pass compares designer metadata that is cheap and objective:

- form title
- client size
- control count
- button count
- obvious structural mismatch

This is not a full visual QA pass. A form can pass metadata and still fail visual/behavior parity; metadata failures are enough to require remediation.

## Migration parity matrix

| Area | Legacy form | New form | Legacy size | New size | Legacy title | New title | Result |
|------|-------------|----------|-------------|----------|--------------|-----------|--------|
| Login | `frmAcceso` | `frmLogin` | `344,201` | `344,191` | `EVANS Cargo S.A.C.` | `EVANS Cargo S.A.C.` | Needs parity remediation: size/control/button mismatch |
| Consulta RUC | `frmConsultaRUC` | `frmConsultaRuc` | `783,499` | `464,291` | `Consulta de RUC` | `Consulta RUC` | Intentional exception: direct query/result UI replaces legacy embedded WebBrowser; behavior covered by UI tests |
| Recepción | `frmRecepcion` | `frmRecepcion` | `841,606` | `800,640` | `Recepción` | `Recepcion de Carga` | Needs parity remediation: size/title/control/button mismatch |
| Guía Remisión | `frmGuiaRemision` | `frmGuiaRemision` | `841,606` | `784,497` | `Guía de Remisión` | `Guía de Remisión` | Needs parity remediation: size/control/button mismatch |
| Comprobante | `frmComprobante` | `frmComprobante` | `841,604` | `784,457` | `Comprobantes de Pago` | `Comprobante` | Needs parity remediation: size/title/control/button mismatch |
| Manifiesto | `frmManifiesto` | `frmManifiesto` | `823,604` | `800,473` | `Manifiestos de Carga` | `Manifiestos` | Needs parity remediation: size/title/control/button mismatch |
| Envíos Mensuales | `frmConsEnviosMensuales` | `frmConsEnviosMensuales` | `628,601` | `628,601` | `Envios Mensuales` | `Envios Mensuales` | Metadata parity covered by UI test; visual QA still pending |
| Guías por Cliente | `frmConsGuiasPorCliente` | `frmConsGuiasPorCliente` | `821,602` | `821,602` | `Consulta de Guias por Cliente` | `Consulta de Guias por Cliente` | Metadata parity covered by UI test; visual QA still pending |
| Reporte Ventas | `frmReporteVentas` | `frmReporteVentas` | `804,599` | `804,599` | `Reporte de Ventas` | `Reporte de Ventas` | Metadata parity covered by UI test; visual QA still pending |
| Estado | `frmMantEstado` | `frmMantEstado` | `635,490` | `635,490` | `Registro de Estados` | `Registro de Estados` | Legacy form retired after metadata/icon/control parity coverage; `clsEstado.vb` remains because other legacy screens still consume it |
| Destino | `frmMantDestino` | `frmMantDestino` | `635,490` | `635,490` | `Registro de Destinos` | `Registro de Destinos` | Metadata/icon/control parity and behavior tests added; visual QA and legacy retirement still pending |
| Empresa | `frmMantEmpresa` | `frmMantEmpresa` | `635,490` | `635,490` | `Registro de Empresas` | `Registro de Empresas` | Metadata/icon/control parity and behavior tests added; legacy preserved until the new implementation is 100% complete and confirmed |
| Chofer | `frmMantChofer` | `frmMantChofer` | `635,490` | `635,490` | `Registro de Choferes` | `Registro de Choferes` | Metadata/icon/control parity, behavior tests, and local render QA added; legacy preserved until the new implementation is 100% complete and confirmed |
| Carreta | `frmMantCarreta` | `frmMantCarreta` | `635,490` | `635,490` | `Registro de Carretas` | `Registro de Carretas` | Metadata/icon/control parity and behavior tests added; legacy preserved until the new implementation is 100% complete and confirmed |
| Tipo Identificación | `frmMantTipoID` | `frmMantTipoID` | `635,490` | `635,490` | `Registro de Tipos de Identificación` | `Registro de Tipos de Identificación` | Metadata/icon/control parity and behavior tests added; legacy preserved until the new implementation is 100% complete and confirmed |

## Immediate remediation order

1. Add parity tests/checks to already migrated screens before more legacy deletion.
2. Prioritize forms whose legacy equivalents were already deleted: Reportes/Consultas now have metadata parity tests; Consulta RUC is documented as an intentional non-parity exception.
3. Revisit larger transactional forms (`Guía`, `Comprobante`, `Recepción`, `Manifiesto`) with a dedicated parity pass; they are too different for a mechanical small fix.

## Required checklist before retiring any legacy form

- [ ] Legacy designer reviewed as source of truth.
- [ ] New designer matches title and client size, unless an explicit product decision says otherwise.
- [ ] Main controls, labels, buttons, tab order, and default enabled/disabled states are mapped.
- [ ] Behavior tests cover initial state, search/list/detail flow, save/update/cancel/exit behavior.
- [ ] Visual QA performed against the legacy screen.
- [ ] Any intentional mismatch is documented with a reason.

## Notes

`frmConsultaRUC` is a special case and is accepted as an intentional mismatch. The legacy screen embedded the SUNAT web page in a `WebBrowser`, while the new screen exposes a direct query/result UI with MediatR-backed behavior tests. Do not reintroduce an embedded browser just to match the legacy shell; preserve the direct lookup behavior unless the product requirement changes.

`frmMantEstado` keeps `Label4` and `Label1` with fixed legacy sizes instead of `AutoSize=true` because .NET 8 recalculates those labels to a wider/taller box than the VB legacy designer. This is a visual-parity choice: size/position match wins over preserving the legacy `AutoSize` flag.
