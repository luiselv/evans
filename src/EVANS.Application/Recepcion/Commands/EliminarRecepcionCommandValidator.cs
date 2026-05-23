using FluentValidation;

namespace EVANS.Application.Recepcion.Commands;

public sealed class EliminarRecepcionCommandValidator : AbstractValidator<EliminarRecepcionCommand>
{
    public EliminarRecepcionCommandValidator()
    {
        RuleFor(c => c.Codigo)
            .GreaterThan(0).WithMessage("Codigo de recepcion requerido.");

        RuleFor(c => c.Year)
            .GreaterThan(2000).WithMessage("Year debe ser mayor a 2000.");
    }
}
