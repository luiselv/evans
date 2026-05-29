using EVANS.Domain.Recepcion;

namespace EVANS.Application.Recepcion.Commands;

/// <summary>Input record for a single detail line within Crear/Actualizar commands.</summary>
public record DetalleRecepcionInput(
    decimal Cantidad,
    string Descripcion,
    decimal Peso,
    string Unidad,
    decimal Costo,
    string TipoDoc,
    string NroDoc);
