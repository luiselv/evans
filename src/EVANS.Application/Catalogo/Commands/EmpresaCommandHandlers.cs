using EVANS.Application.Catalogo.Ports;
using EVANS.Application.Common;
using EVANS.Domain.Catalogo;
using EVANS.Domain.Shared;
using MediatR;

namespace EVANS.Application.Catalogo.Commands;

public sealed class CreateEmpresaCommandHandler(IRepository<Empresa> repository)
    : IRequestHandler<CreateEmpresaCommand, Result<int>>
{
    private readonly CreateEmpresaCommandValidator _validator = new();

    public async Task<Result<int>> Handle(CreateEmpresaCommand request, CancellationToken cancellationToken)
    {
        var validation = _validator.Validate(request);
        if (!validation.IsValid)
            return Result<int>.Fail(validation.Errors[0].ErrorMessage);

        try
        {
            var empresa = Empresa.Crear(
                request.RazonSocial,
                request.Direccion,
                request.Telefono,
                Ruc.Parse(request.Ruc),
                request.EsPropia,
                request.EstadoCodigo);

            var codigo = await repository.AddAsync(empresa, cancellationToken);
            return Result<int>.Ok(codigo);
        }
        catch (DomainException ex)
        {
            return Result<int>.Fail(ex.Message);
        }
        catch (ArgumentException ex)
        {
            return Result<int>.Fail(ex.Message);
        }
    }
}

public sealed class UpdateEmpresaCommandHandler(IRepository<Empresa> repository)
    : IRequestHandler<UpdateEmpresaCommand, Result<bool>>
{
    private readonly UpdateEmpresaCommandValidator _validator = new();

    public async Task<Result<bool>> Handle(UpdateEmpresaCommand request, CancellationToken cancellationToken)
    {
        var validation = _validator.Validate(request);
        if (!validation.IsValid)
            return Result<bool>.Fail(validation.Errors[0].ErrorMessage);

        var empresa = await repository.GetByIdAsync(request.Codigo, cancellationToken);
        if (empresa is null)
            return Result<bool>.Fail("Empresa not found.");

        try
        {
            empresa.Actualizar(
                request.RazonSocial,
                request.Direccion,
                request.Telefono,
                Ruc.Parse(request.Ruc),
                request.EsPropia,
                request.EstadoCodigo);

            await repository.UpdateAsync(empresa, cancellationToken);
            return Result<bool>.Ok(true);
        }
        catch (DomainException ex)
        {
            return Result<bool>.Fail(ex.Message);
        }
        catch (ArgumentException ex)
        {
            return Result<bool>.Fail(ex.Message);
        }
    }
}

public sealed class DeactivateEmpresaCommandHandler(IRepository<Empresa> repository)
    : IRequestHandler<DeactivateEmpresaCommand, Result<bool>>
{
    private readonly DeactivateEmpresaCommandValidator _validator = new();

    public async Task<Result<bool>> Handle(DeactivateEmpresaCommand request, CancellationToken cancellationToken)
    {
        var validation = _validator.Validate(request);
        if (!validation.IsValid)
            return Result<bool>.Fail(validation.Errors[0].ErrorMessage);

        var empresa = await repository.GetByIdAsync(request.Codigo, cancellationToken);
        if (empresa is null)
            return Result<bool>.Fail("Empresa not found.");

        await repository.DeactivateAsync(request.Codigo, cancellationToken);
        return Result<bool>.Ok(true);
    }
}
