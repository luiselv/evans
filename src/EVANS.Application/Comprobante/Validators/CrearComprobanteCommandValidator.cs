using EVANS.Application.Comprobante.Commands;
using EVANS.Domain.Comprobante;
using FluentValidation;

namespace EVANS.Application.Comprobante.Validators;

public sealed class CrearComprobanteCommandValidator : AbstractValidator<CrearComprobanteCommand>
{
    public CrearComprobanteCommandValidator()
    {
        RuleFor(c => c.Tipo)
            .IsInEnum().WithMessage("TipoComprobante must be a valid value.");

        RuleFor(c => c.ClienteCodigo)
            .GreaterThan(0).WithMessage("ClienteCodigo must be greater than zero.");

        RuleFor(c => c.Detalles)
            .NotEmpty().WithMessage("Comprobante must have at least one detail.");

        RuleFor(c => c.RucODni)
            .NotEmpty().WithMessage("RucODni is required.");

        RuleFor(c => c.Direccion)
            .NotEmpty().WithMessage("Direccion is required.");

        RuleFor(c => c.Year)
            .GreaterThan(2000).WithMessage("Year must be greater than 2000.");

        RuleForEach(c => c.Detalles)
            .ChildRules(detalle =>
            {
                detalle.RuleFor(d => d.Cantidad)
                    .GreaterThan(0).WithMessage("Cantidad must be greater than zero.");
                detalle.RuleFor(d => d.Descripcion)
                    .NotEmpty().WithMessage("Descripcion cannot be empty.");
                detalle.RuleFor(d => d.Flete)
                    .GreaterThanOrEqualTo(0).WithMessage("Flete cannot be negative.");
            });
    }
}
