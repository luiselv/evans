namespace EVANS.Application.Comprobante.DTOs;

public record DetalleComprobanteDto(
    int Cantidad,
    string Descripcion,
    decimal PrecioUnitario,
    decimal Flete);
