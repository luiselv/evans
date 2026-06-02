namespace EVANS.Application.Reportes.DTOs;

public sealed record ReporteVentasFiltro(
    int Year,
    DateTime FechaDesde,
    DateTime FechaHasta,
    bool IncluirFacturas,
    bool IncluirBoletas,
    int? ClienteCodigo);
