namespace EVANS.Application.Reportes.DTOs;

public sealed record GuiasPorClienteFiltro(
    int Year,
    int ClienteCodigo,
    DateTime FechaDesde,
    DateTime FechaHasta,
    bool SoloPendientes);
