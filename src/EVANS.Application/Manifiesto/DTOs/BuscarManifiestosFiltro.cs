namespace EVANS.Application.Manifiesto.DTOs;

public record BuscarManifiestosFiltro(
    int Year,
    DateTime? FechaDesde = null,
    DateTime? FechaHasta = null,
    string? Numero = null,
    int? TransportistaCodigo = null,
    int? EstadoCodigo = null);
