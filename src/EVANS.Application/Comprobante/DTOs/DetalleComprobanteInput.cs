namespace EVANS.Application.Comprobante.DTOs;

public record DetalleComprobanteInput(
    int Cantidad,
    string Descripcion,
    decimal PrecioUnitario,
    decimal Flete);
