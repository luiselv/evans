namespace EVANS.Application.Reportes.DTOs;

public sealed record GuiaPorClienteDto(
    int Codigo,
    string NroDoc,
    string Remitente,
    string Destinatario,
    DateTime FechaEmision,
    DateTime FechaTraslado,
    int Bultos,
    decimal CostoTotal,
    bool Enviado);
