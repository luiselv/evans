using FluentValidation;

namespace EVANS.Application.Recepcion.Commands;

public sealed class ActualizarRecepcionCommandValidator : AbstractValidator<ActualizarRecepcionCommand>
{
    public ActualizarRecepcionCommandValidator()
    {
        RuleFor(c => c.Codigo)
            .GreaterThan(0).WithMessage("Codigo de recepcion requerido.");

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

        RuleFor(c => c.Detalles)
            .NotEmpty().WithMessage("Se requiere al menos un detalle.");

        RuleFor(c => c.Year)
            .GreaterThan(2000).WithMessage("Year debe ser mayor a 2000.");
    }
}
