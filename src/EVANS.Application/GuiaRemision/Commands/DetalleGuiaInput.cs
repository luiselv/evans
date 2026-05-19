using EVANS.Domain.GuiaRemision;

namespace EVANS.Application.GuiaRemision.Commands;

public record DetalleGuiaInput(
    string Descripcion,
    Peso Peso,
    decimal PrecioUnitario,
    decimal PrecioTotal,
    int Cantidad);
