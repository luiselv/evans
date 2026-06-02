namespace EVANS.Application.Reportes.DTOs;

public sealed record EnviosMensualesFiltro(
    int Year,
    DateTime FechaDesde,
    DateTime FechaHasta,
    IReadOnlyList<int> DestinoCodigos);
