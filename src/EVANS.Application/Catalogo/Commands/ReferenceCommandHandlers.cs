using EVANS.Application.Catalogo.Ports;
using EVANS.Application.Common;
using EVANS.Domain.Catalogo;
using EVANS.Domain.Shared;
using MediatR;

namespace EVANS.Application.Catalogo.Commands;

public sealed class CreateClienteCommandHandler(IClienteRepository repository)
    : IRequestHandler<CreateClienteCommand, Result<int>>
{
    private readonly CreateClienteCommandValidator _validator = new();

    public async Task<Result<int>> Handle(CreateClienteCommand request, CancellationToken cancellationToken)
    {
        var validation = _validator.Validate(request);
        if (!validation.IsValid) return Result<int>.Fail(validation.Errors[0].ErrorMessage);

        try
        {
            var cliente = Cliente.Crear(
                request.RazonSocial,
                request.TipoIdCodigo,
                request.NroIdentificacion,
                request.LongitudRequerida,
                request.Telefono,
                request.Email,
                request.Direcciones.Select(ToDireccion).ToList());

            return Result<int>.Ok(await repository.AddAsync(cliente, cancellationToken));
        }
        catch (DomainException ex)
        {
            return Result<int>.Fail(ex.Message);
        }
    }

    private static Direccion ToDireccion(DTOs.DireccionDto dto) =>
        new(dto.Calle, dto.Ciudad, dto.Provincia);
}

public sealed class UpdateClienteCommandHandler(IClienteRepository repository)
    : IRequestHandler<UpdateClienteCommand, Result<bool>>
{
    private readonly UpdateClienteCommandValidator _validator = new();

    public async Task<Result<bool>> Handle(UpdateClienteCommand request, CancellationToken cancellationToken)
    {
        var validation = _validator.Validate(request);
        if (!validation.IsValid) return Result<bool>.Fail(validation.Errors[0].ErrorMessage);

        var cliente = await repository.GetByIdAsync(request.Codigo, cancellationToken);
        if (cliente is null) return Result<bool>.Fail("Cliente not found.");

        try
        {
            cliente.Actualizar(
                request.RazonSocial,
                request.TipoIdCodigo,
                request.NroIdentificacion,
                request.LongitudRequerida,
                request.Telefono,
                request.Email,
                request.Direcciones.Select(ToDireccion).ToList());

            await repository.UpdateAsync(cliente, cancellationToken);
            return Result<bool>.Ok(true);
        }
        catch (DomainException ex)
        {
            return Result<bool>.Fail(ex.Message);
        }
    }

    private static Direccion ToDireccion(DTOs.DireccionDto dto) =>
        new(dto.Calle, dto.Ciudad, dto.Provincia);
}

public sealed class CreateEstadoCommandHandler(IEstadoRepository repository)
    : IRequestHandler<CreateEstadoCommand, Result<int>>
{
    private readonly CreateEstadoCommandValidator _validator = new();

    public async Task<Result<int>> Handle(CreateEstadoCommand request, CancellationToken cancellationToken)
    {
        var validation = _validator.Validate(request);
        if (!validation.IsValid) return Result<int>.Fail(validation.Errors[0].ErrorMessage);

        try
        {
            return Result<int>.Ok(await repository.AddAsync(Estado.Crear(request.Descripcion), cancellationToken));
        }
        catch (DomainException ex)
        {
            return Result<int>.Fail(ex.Message);
        }
    }
}

public sealed class UpdateEstadoCommandHandler(IEstadoRepository repository)
    : IRequestHandler<UpdateEstadoCommand, Result<bool>>
{
    private readonly UpdateEstadoCommandValidator _validator = new();

    public async Task<Result<bool>> Handle(UpdateEstadoCommand request, CancellationToken cancellationToken)
    {
        var validation = _validator.Validate(request);
        if (!validation.IsValid) return Result<bool>.Fail(validation.Errors[0].ErrorMessage);

        var existing = await repository.GetByIdAsync(request.Codigo, cancellationToken);
        if (existing is null) return Result<bool>.Fail("Estado not found.");

        try
        {
            await repository.UpdateAsync(
                Estado.Materializar(request.Codigo, request.Descripcion),
                cancellationToken);
            return Result<bool>.Ok(true);
        }
        catch (DomainException ex)
        {
            return Result<bool>.Fail(ex.Message);
        }
    }
}

public sealed class CreateTipoIdentificacionCommandHandler(ITipoIdentificacionRepository repository)
    : IRequestHandler<CreateTipoIdentificacionCommand, Result<int>>
{
    private readonly CreateTipoIdentificacionCommandValidator _validator = new();

    public async Task<Result<int>> Handle(CreateTipoIdentificacionCommand request, CancellationToken cancellationToken)
    {
        var validation = _validator.Validate(request);
        if (!validation.IsValid) return Result<int>.Fail(validation.Errors[0].ErrorMessage);

        try
        {
            return Result<int>.Ok(await repository.AddAsync(
                TipoIdentificacion.Crear(request.Descripcion),
                cancellationToken));
        }
        catch (DomainException ex)
        {
            return Result<int>.Fail(ex.Message);
        }
    }
}

public sealed class UpdateTipoIdentificacionCommandHandler(ITipoIdentificacionRepository repository)
    : IRequestHandler<UpdateTipoIdentificacionCommand, Result<bool>>
{
    private readonly UpdateTipoIdentificacionCommandValidator _validator = new();

    public async Task<Result<bool>> Handle(UpdateTipoIdentificacionCommand request, CancellationToken cancellationToken)
    {
        var validation = _validator.Validate(request);
        if (!validation.IsValid) return Result<bool>.Fail(validation.Errors[0].ErrorMessage);

        var existing = await repository.GetByIdAsync(request.Codigo, cancellationToken);
        if (existing is null) return Result<bool>.Fail("TipoIdentificacion not found.");

        try
        {
            await repository.UpdateAsync(
                TipoIdentificacion.Materializar(request.Codigo, request.Descripcion),
                cancellationToken);
            return Result<bool>.Ok(true);
        }
        catch (DomainException ex)
        {
            return Result<bool>.Fail(ex.Message);
        }
    }
}
