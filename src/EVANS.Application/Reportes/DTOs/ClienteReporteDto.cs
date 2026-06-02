namespace EVANS.Application.Reportes.DTOs;

public sealed record ClienteReporteDto(
    int Codigo,
    string Nombre,
    string NumeroIdentificacion);
