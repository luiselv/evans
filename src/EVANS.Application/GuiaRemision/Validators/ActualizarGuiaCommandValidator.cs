using EVANS.Application.GuiaRemision.Commands;
using FluentValidation;

namespace EVANS.Application.GuiaRemision.Validators;

public sealed class ActualizarGuiaCommandValidator : AbstractValidator<ActualizarGuiaCommand>
{
    public ActualizarGuiaCommandValidator()
    {
        RuleFor(c => c.Codigo)
            .GreaterThan(0).WithMessage("Codigo must be greater than zero.");

        RuleFor(c => c.RemitenteId)
            .GreaterThan(0).WithMessage("RemitenteId must be greater than zero.");

        RuleFor(c => c.DestinatarioId)
            .GreaterThan(0).WithMessage("DestinatarioId must be greater than zero.");

        RuleFor(c => c.Detalles)
            .NotEmpty().WithMessage("Guia must have at least one detail.");

        RuleFor(c => c.Year)
            .GreaterThan(2000).WithMessage("Year must be greater than 2000.");

        RuleForEach(c => c.Detalles)
            .ChildRules(detalle =>
            {
                detalle.RuleFor(d => d.PrecioUnitario)
                    .GreaterThanOrEqualTo(0).WithMessage("PrecioUnitario cannot be negative.");
                detalle.RuleFor(d => d.PrecioTotal)
                    .GreaterThanOrEqualTo(0).WithMessage("PrecioTotal cannot be negative.");
                detalle.RuleFor(d => d.Cantidad)
                    .GreaterThan(0).WithMessage("Cantidad must be greater than zero.");
                detalle.RuleFor(d => d.Descripcion)
                    .NotEmpty().WithMessage("Descripcion cannot be empty.");
            });
    }
}
