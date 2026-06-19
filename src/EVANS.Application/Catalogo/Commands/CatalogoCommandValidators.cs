using EVANS.Domain.Shared;
using FluentValidation;

namespace EVANS.Application.Catalogo.Commands;

public sealed class CreateClienteCommandValidator : AbstractValidator<CreateClienteCommand>
{
    public CreateClienteCommandValidator()
    {
        RuleFor(c => c.RazonSocial).NotEmpty();
        RuleFor(c => c.TipoIdCodigo).GreaterThan(0);
        RuleFor(c => c.LongitudRequerida).Must(length => length is 8 or 11);
        RuleFor(c => c.NroIdentificacion)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .Must((command, value) => value.Length == command.LongitudRequerida)
            .WithMessage("NroIdentificacion length must match LongitudRequerida.")
            .Must((command, value) => command.LongitudRequerida != 11 || Ruc.TryCreate(value, out _))
            .WithMessage("RUC must be 11 numeric characters.");
        RuleFor(c => c.Direcciones).NotEmpty();
    }
}

public sealed class UpdateClienteCommandValidator : AbstractValidator<UpdateClienteCommand>
{
    public UpdateClienteCommandValidator()
    {
        RuleFor(c => c.Codigo).GreaterThan(0);
        RuleFor(c => c.RazonSocial).NotEmpty();
        RuleFor(c => c.TipoIdCodigo).GreaterThan(0);
        RuleFor(c => c.LongitudRequerida).Must(length => length is 8 or 11);
        RuleFor(c => c.NroIdentificacion)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .Must((command, value) => value.Length == command.LongitudRequerida)
            .WithMessage("NroIdentificacion length must match LongitudRequerida.")
            .Must((command, value) => command.LongitudRequerida != 11 || Ruc.TryCreate(value, out _))
            .WithMessage("RUC must be 11 numeric characters.");
        RuleFor(c => c.Direcciones).NotEmpty();
    }
}

public sealed class CreateEmpresaCommandValidator : AbstractValidator<CreateEmpresaCommand>
{
    public CreateEmpresaCommandValidator()
    {
        RuleFor(c => c.RazonSocial).NotEmpty();
        RuleFor(c => c.Ruc).Must(value => Ruc.TryCreate(value, out _))
            .WithMessage("RUC must be 11 numeric characters.");
        RuleFor(c => c.EstadoCodigo).GreaterThan(0);
    }
}

public sealed class UpdateEmpresaCommandValidator : AbstractValidator<UpdateEmpresaCommand>
{
    public UpdateEmpresaCommandValidator()
    {
        RuleFor(c => c.Codigo).GreaterThan(0);
        RuleFor(c => c.RazonSocial).NotEmpty();
        RuleFor(c => c.Ruc).Must(value => Ruc.TryCreate(value, out _))
            .WithMessage("RUC must be 11 numeric characters.");
        RuleFor(c => c.EstadoCodigo).GreaterThan(0);
    }
}

public sealed class DeactivateEmpresaCommandValidator : AbstractValidator<DeactivateEmpresaCommand>
{
    public DeactivateEmpresaCommandValidator() => RuleFor(c => c.Codigo).GreaterThan(0);
}

public sealed class CreateVehiculoCommandValidator : AbstractValidator<CreateVehiculoCommand>
{
    public CreateVehiculoCommandValidator()
    {
        RuleFor(c => c.Placa).NotEmpty();
        RuleFor(c => c.ConfiguracionVehicular).NotEmpty().MaximumLength(5);
        RuleFor(c => c.EmpresaCodigo).GreaterThan(0);
        RuleFor(c => c.EstadoCodigo).GreaterThan(0);
    }
}

public sealed class UpdateVehiculoCommandValidator : AbstractValidator<UpdateVehiculoCommand>
{
    public UpdateVehiculoCommandValidator()
    {
        RuleFor(c => c.Codigo).GreaterThan(0);
        RuleFor(c => c.Placa).NotEmpty();
        RuleFor(c => c.ConfiguracionVehicular).NotEmpty().MaximumLength(5);
        RuleFor(c => c.EmpresaCodigo).GreaterThan(0);
        RuleFor(c => c.EstadoCodigo).GreaterThan(0);
    }
}

public sealed class DeactivateVehiculoCommandValidator : AbstractValidator<DeactivateVehiculoCommand>
{
    public DeactivateVehiculoCommandValidator() => RuleFor(c => c.Codigo).GreaterThan(0);
}

public sealed class CreateCarretaCommandValidator : AbstractValidator<CreateCarretaCommand>
{
    public CreateCarretaCommandValidator()
    {
        RuleFor(c => c.Placa).NotEmpty();
        RuleFor(c => c.EmpresaCodigo).GreaterThan(0);
        RuleFor(c => c.EstadoCodigo).GreaterThan(0);
    }
}

public sealed class UpdateCarretaCommandValidator : AbstractValidator<UpdateCarretaCommand>
{
    public UpdateCarretaCommandValidator()
    {
        RuleFor(c => c.Codigo).GreaterThan(0);
        RuleFor(c => c.Placa).NotEmpty();
        RuleFor(c => c.EmpresaCodigo).GreaterThan(0);
        RuleFor(c => c.EstadoCodigo).GreaterThan(0);
    }
}

public sealed class DeactivateCarretaCommandValidator : AbstractValidator<DeactivateCarretaCommand>
{
    public DeactivateCarretaCommandValidator() => RuleFor(c => c.Codigo).GreaterThan(0);
}

public sealed class CreateChoferCommandValidator : AbstractValidator<CreateChoferCommand>
{
    public CreateChoferCommandValidator()
    {
        RuleFor(c => c.NombreCompleto).NotEmpty();
        RuleFor(c => c.Licencia).NotEmpty();
        RuleFor(c => c.EmpresaCodigo).GreaterThan(0);
        RuleFor(c => c.EstadoCodigo).GreaterThan(0);
    }
}

public sealed class UpdateChoferCommandValidator : AbstractValidator<UpdateChoferCommand>
{
    public UpdateChoferCommandValidator()
    {
        RuleFor(c => c.Codigo).GreaterThan(0);
        RuleFor(c => c.NombreCompleto).NotEmpty();
        RuleFor(c => c.Licencia).NotEmpty();
        RuleFor(c => c.EmpresaCodigo).GreaterThan(0);
        RuleFor(c => c.EstadoCodigo).GreaterThan(0);
    }
}

public sealed class DeactivateChoferCommandValidator : AbstractValidator<DeactivateChoferCommand>
{
    public DeactivateChoferCommandValidator() => RuleFor(c => c.Codigo).GreaterThan(0);
}

public sealed class CreateDestinoCommandValidator : AbstractValidator<CreateDestinoCommand>
{
    public CreateDestinoCommandValidator()
    {
        RuleFor(c => c.Descripcion).NotEmpty();
        RuleFor(c => c.DistanciaVirtual).GreaterThanOrEqualTo(0);
        RuleFor(c => c.EstadoCodigo).GreaterThan(0);
    }
}

public sealed class UpdateDestinoCommandValidator : AbstractValidator<UpdateDestinoCommand>
{
    public UpdateDestinoCommandValidator()
    {
        RuleFor(c => c.Codigo).GreaterThan(0);
        RuleFor(c => c.Descripcion).NotEmpty();
        RuleFor(c => c.DistanciaVirtual).GreaterThanOrEqualTo(0);
        RuleFor(c => c.EstadoCodigo).GreaterThan(0);
    }
}

public sealed class DeactivateDestinoCommandValidator : AbstractValidator<DeactivateDestinoCommand>
{
    public DeactivateDestinoCommandValidator() => RuleFor(c => c.Codigo).GreaterThan(0);
}

public sealed class CreateEstadoCommandValidator : AbstractValidator<CreateEstadoCommand>
{
    public CreateEstadoCommandValidator() => RuleFor(c => c.Descripcion).NotEmpty();
}

public sealed class UpdateEstadoCommandValidator : AbstractValidator<UpdateEstadoCommand>
{
    public UpdateEstadoCommandValidator()
    {
        RuleFor(c => c.Codigo).GreaterThan(0);
        RuleFor(c => c.Descripcion).NotEmpty();
    }
}

public sealed class CreateTipoIdentificacionCommandValidator : AbstractValidator<CreateTipoIdentificacionCommand>
{
    public CreateTipoIdentificacionCommandValidator()
    {
        RuleFor(c => c.Descripcion).NotEmpty();
    }
}

public sealed class UpdateTipoIdentificacionCommandValidator : AbstractValidator<UpdateTipoIdentificacionCommand>
{
    public UpdateTipoIdentificacionCommandValidator()
    {
        RuleFor(c => c.Codigo).GreaterThan(0);
        RuleFor(c => c.Descripcion).NotEmpty();
    }
}
