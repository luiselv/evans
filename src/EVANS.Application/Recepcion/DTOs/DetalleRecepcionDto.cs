namespace EVANS.Application.Recepcion.DTOs;

/// <summary>DTO for a single DetalleRecepcion line — read-only projection.</summary>
public record DetalleRecepcionDto(
    decimal Cantidad,
    string Descripcion,
    decimal Peso,
    string Unidad,
    decimal Costo,
    string TipoDoc,
    string NroDoc);
