namespace EVANS.Application.Recepcion.DTOs;

public record DetalleRecepcionDto(
    decimal Cantidad,
    string Descripcion,
    decimal Peso,
    string Unidad,
    decimal Costo,
    string TipoDoc,
    string NroDoc);
