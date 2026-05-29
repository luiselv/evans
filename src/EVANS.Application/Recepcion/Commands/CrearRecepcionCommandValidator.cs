using FluentValidation;

namespace EVANS.Application.Recepcion.Commands;

public sealed class CrearRecepcionCommandValidator : AbstractValidator<CrearRecepcionCommand>
{
    public CrearRecepcionCommandValidator()
    {
        RuleFor(c => c.FechaEmision)
            .NotEqual(default(DateTime)).WithMessage("Fecha de emision requerida.");

        RuleFor(c => c.RemitenteId)
            .GreaterThan(0).WithMessage("Remitente requerido.");

        RuleFor(c => c.DestinatarioId)
            .GreaterThan(0).WithMessage("Destinatario requerido.");

        RuleFor(c => c.DestinoId)
            .GreaterThan(0).WithMessage("Destino requerido.");

        RuleFor(c => c.EstadoId)
            .GreaterThan(0).WithMessage("Estado requerido.");

        RuleFor(c => c.UsuarioId)
            .GreaterThan(0).WithMessage("Usuario requerido.");

        RuleFor(c => c.CostoTotal)
            .GreaterThanOrEqualTo(0).WithMessage("Costo total no puede ser negativo.");

        RuleFor(c => c.Detalles)
            .NotEmpty().WithMessage("Se requiere al menos un detalle.");

        RuleFor(c => c.Year)
            .GreaterThan(2000).WithMessage("Year debe ser mayor a 2000.");
    }
}
