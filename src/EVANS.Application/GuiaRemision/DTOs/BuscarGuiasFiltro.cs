namespace EVANS.Application.GuiaRemision.DTOs;

public record BuscarGuiasFiltro(
    DateTime? Desde,
    DateTime? Hasta,
    int? ClienteId,
    int? EstadoId);
