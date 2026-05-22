using EVANS.Application.Manifiesto.Commands;
using FluentValidation;

namespace EVANS.Application.Manifiesto.Validators;

public sealed class CrearManifiestoCommandValidator : AbstractValidator<CrearManifiestoCommand>
{
    public CrearManifiestoCommandValidator()
    {
        RuleFor(c => c.GuiaIds)
            .NotEmpty().WithMessage("Se requiere al menos una guía.");

        RuleFor(c => c.TransportistaCodigo)
            .GreaterThan(0).WithMessage("TransportistaCodigo debe ser mayor a cero.");

        RuleFor(c => c.VehiculoCodigo)
            .GreaterThan(0).WithMessage("VehiculoCodigo debe ser mayor a cero.");

        RuleFor(c => c.ChoferCodigo)
            .GreaterThan(0).WithMessage("ChoferCodigo debe ser mayor a cero.");

        RuleFor(c => c.EstadoCodigo)
            .GreaterThan(0).WithMessage("EstadoCodigo debe ser mayor a cero.");

        RuleFor(c => c.Importe)
            .GreaterThanOrEqualTo(0).WithMessage("El importe no puede ser negativo.");

        RuleFor(c => c.Year)
            .GreaterThan(2000).WithMessage("Year debe ser mayor a 2000.");
    }
}
