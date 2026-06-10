using EVANS.Application.Catalogo.Ports;
using EVANS.Application.Common;
using EVANS.Domain.Catalogo;
using MediatR;

namespace EVANS.Application.Catalogo.Commands;

public sealed class CreateVehiculoCommandHandler(IRepository<Vehiculo> repository)
    : IRequestHandler<CreateVehiculoCommand, Result<int>>
{
    private readonly CreateVehiculoCommandValidator _validator = new();

    public async Task<Result<int>> Handle(CreateVehiculoCommand request, CancellationToken cancellationToken)
    {
        var validation = _validator.Validate(request);
        if (!validation.IsValid) return Result<int>.Fail(validation.Errors[0].ErrorMessage);

        try
        {
            var entity = Vehiculo.Crear(
                request.Marca,
                request.Placa,
                request.ConfiguracionVehicular,
                request.CertificadoInscripcion,
                request.EmpresaCodigo);
            return Result<int>.Ok(await repository.AddAsync(entity, cancellationToken));
        }
        catch (DomainException ex)
        {
            return Result<int>.Fail(ex.Message);
        }
    }
}

public sealed class UpdateVehiculoCommandHandler(IRepository<Vehiculo> repository)
    : IRequestHandler<UpdateVehiculoCommand, Result<bool>>
{
    private readonly UpdateVehiculoCommandValidator _validator = new();

    public async Task<Result<bool>> Handle(UpdateVehiculoCommand request, CancellationToken cancellationToken)
    {
        var validation = _validator.Validate(request);
        if (!validation.IsValid) return Result<bool>.Fail(validation.Errors[0].ErrorMessage);

        var existing = await repository.GetByIdAsync(request.Codigo, cancellationToken);
        if (existing is null) return Result<bool>.Fail("Vehiculo not found.");

        try
        {
            var entity = Vehiculo.Materializar(
                request.Codigo,
                request.Marca,
                request.Placa,
                request.ConfiguracionVehicular,
                request.CertificadoInscripcion,
                request.EmpresaCodigo,
                existing.EstadoCodigo);
            await repository.UpdateAsync(entity, cancellationToken);
            return Result<bool>.Ok(true);
        }
        catch (DomainException ex)
        {
            return Result<bool>.Fail(ex.Message);
        }
    }
}

public sealed class DeactivateVehiculoCommandHandler(IRepository<Vehiculo> repository)
    : IRequestHandler<DeactivateVehiculoCommand, Result<bool>>
{
    private readonly DeactivateVehiculoCommandValidator _validator = new();

    public async Task<Result<bool>> Handle(DeactivateVehiculoCommand request, CancellationToken cancellationToken) =>
        await CatalogoCommandHandlerHelpers.DeactivateAsync(request.Codigo, _validator.Validate(request), repository, "Vehiculo not found.", cancellationToken);
}

public sealed class CreateCarretaCommandHandler(IRepository<Carreta> repository)
    : IRequestHandler<CreateCarretaCommand, Result<int>>
{
    private readonly CreateCarretaCommandValidator _validator = new();

    public async Task<Result<int>> Handle(CreateCarretaCommand request, CancellationToken cancellationToken)
    {
        var validation = _validator.Validate(request);
        if (!validation.IsValid) return Result<int>.Fail(validation.Errors[0].ErrorMessage);

        try
        {
            var entity = Carreta.Crear(request.Placa, request.Marca, request.Certificado, request.EmpresaCodigo);
            return Result<int>.Ok(await repository.AddAsync(entity, cancellationToken));
        }
        catch (DomainException ex)
        {
            return Result<int>.Fail(ex.Message);
        }
    }
}

public sealed class UpdateCarretaCommandHandler(IRepository<Carreta> repository)
    : IRequestHandler<UpdateCarretaCommand, Result<bool>>
{
    private readonly UpdateCarretaCommandValidator _validator = new();

    public async Task<Result<bool>> Handle(UpdateCarretaCommand request, CancellationToken cancellationToken)
    {
        var validation = _validator.Validate(request);
        if (!validation.IsValid) return Result<bool>.Fail(validation.Errors[0].ErrorMessage);

        var existing = await repository.GetByIdAsync(request.Codigo, cancellationToken);
        if (existing is null) return Result<bool>.Fail("Carreta not found.");

        try
        {
            var entity = Carreta.Materializar(
                request.Codigo,
                request.Placa,
                request.Marca,
                request.Certificado,
                request.EmpresaCodigo,
                existing.EstadoCodigo);
            await repository.UpdateAsync(entity, cancellationToken);
            return Result<bool>.Ok(true);
        }
        catch (DomainException ex)
        {
            return Result<bool>.Fail(ex.Message);
        }
    }
}

public sealed class DeactivateCarretaCommandHandler(IRepository<Carreta> repository)
    : IRequestHandler<DeactivateCarretaCommand, Result<bool>>
{
    private readonly DeactivateCarretaCommandValidator _validator = new();

    public async Task<Result<bool>> Handle(DeactivateCarretaCommand request, CancellationToken cancellationToken) =>
        await CatalogoCommandHandlerHelpers.DeactivateAsync(request.Codigo, _validator.Validate(request), repository, "Carreta not found.", cancellationToken);
}

public sealed class CreateChoferCommandHandler(IRepository<Chofer> repository)
    : IRequestHandler<CreateChoferCommand, Result<int>>
{
    private readonly CreateChoferCommandValidator _validator = new();

    public async Task<Result<int>> Handle(CreateChoferCommand request, CancellationToken cancellationToken)
    {
        var validation = _validator.Validate(request);
        if (!validation.IsValid) return Result<int>.Fail(validation.Errors[0].ErrorMessage);

        try
        {
            var entity = Chofer.Crear(
                request.NombreCompleto,
                request.Licencia,
                request.Telefono,
                request.Direccion,
                request.EmpresaCodigo,
                request.EstadoCodigo);
            return Result<int>.Ok(await repository.AddAsync(entity, cancellationToken));
        }
        catch (DomainException ex)
        {
            return Result<int>.Fail(ex.Message);
        }
    }
}

public sealed class UpdateChoferCommandHandler(IRepository<Chofer> repository)
    : IRequestHandler<UpdateChoferCommand, Result<bool>>
{
    private readonly UpdateChoferCommandValidator _validator = new();

    public async Task<Result<bool>> Handle(UpdateChoferCommand request, CancellationToken cancellationToken)
    {
        var validation = _validator.Validate(request);
        if (!validation.IsValid) return Result<bool>.Fail(validation.Errors[0].ErrorMessage);

        var existing = await repository.GetByIdAsync(request.Codigo, cancellationToken);
        if (existing is null) return Result<bool>.Fail("Chofer not found.");

        try
        {
            var entity = Chofer.Materializar(
                request.Codigo,
                request.NombreCompleto,
                request.Licencia,
                request.Telefono,
                request.Direccion,
                request.EmpresaCodigo,
                request.EstadoCodigo);
            await repository.UpdateAsync(entity, cancellationToken);
            return Result<bool>.Ok(true);
        }
        catch (DomainException ex)
        {
            return Result<bool>.Fail(ex.Message);
        }
    }
}

public sealed class DeactivateChoferCommandHandler(IRepository<Chofer> repository)
    : IRequestHandler<DeactivateChoferCommand, Result<bool>>
{
    private readonly DeactivateChoferCommandValidator _validator = new();

    public async Task<Result<bool>> Handle(DeactivateChoferCommand request, CancellationToken cancellationToken) =>
        await CatalogoCommandHandlerHelpers.DeactivateAsync(request.Codigo, _validator.Validate(request), repository, "Chofer not found.", cancellationToken);
}

public sealed class CreateDestinoCommandHandler(IRepository<Destino> repository)
    : IRequestHandler<CreateDestinoCommand, Result<int>>
{
    private readonly CreateDestinoCommandValidator _validator = new();

    public async Task<Result<int>> Handle(CreateDestinoCommand request, CancellationToken cancellationToken)
    {
        var validation = _validator.Validate(request);
        if (!validation.IsValid) return Result<int>.Fail(validation.Errors[0].ErrorMessage);

        try
        {
            var entity = Destino.Crear(request.Descripcion, request.DistanciaVirtual, request.EstadoCodigo);
            return Result<int>.Ok(await repository.AddAsync(entity, cancellationToken));
        }
        catch (DomainException ex)
        {
            return Result<int>.Fail(ex.Message);
        }
    }
}

public sealed class UpdateDestinoCommandHandler(IRepository<Destino> repository)
    : IRequestHandler<UpdateDestinoCommand, Result<bool>>
{
    private readonly UpdateDestinoCommandValidator _validator = new();

    public async Task<Result<bool>> Handle(UpdateDestinoCommand request, CancellationToken cancellationToken)
    {
        var validation = _validator.Validate(request);
        if (!validation.IsValid) return Result<bool>.Fail(validation.Errors[0].ErrorMessage);

        var existing = await repository.GetByIdAsync(request.Codigo, cancellationToken);
        if (existing is null) return Result<bool>.Fail("Destino not found.");

        try
        {
            var entity = Destino.Materializar(
                request.Codigo,
                request.Descripcion,
                request.DistanciaVirtual,
                request.EstadoCodigo);
            await repository.UpdateAsync(entity, cancellationToken);
            return Result<bool>.Ok(true);
        }
        catch (DomainException ex)
        {
            return Result<bool>.Fail(ex.Message);
        }
    }
}

public sealed class DeactivateDestinoCommandHandler(IRepository<Destino> repository)
    : IRequestHandler<DeactivateDestinoCommand, Result<bool>>
{
    private readonly DeactivateDestinoCommandValidator _validator = new();

    public async Task<Result<bool>> Handle(DeactivateDestinoCommand request, CancellationToken cancellationToken) =>
        await CatalogoCommandHandlerHelpers.DeactivateAsync(request.Codigo, _validator.Validate(request), repository, "Destino not found.", cancellationToken);
}

internal static class CatalogoCommandHandlerHelpers
{
    public static async Task<Result<bool>> DeactivateAsync<T>(
        int codigo,
        FluentValidation.Results.ValidationResult validation,
        IRepository<T> repository,
        string notFoundMessage,
        CancellationToken cancellationToken)
        where T : class
    {
        if (!validation.IsValid) return Result<bool>.Fail(validation.Errors[0].ErrorMessage);

        var entity = await repository.GetByIdAsync(codigo, cancellationToken);
        if (entity is null) return Result<bool>.Fail(notFoundMessage);

        await repository.DeactivateAsync(codigo, cancellationToken);
        return Result<bool>.Ok(true);
    }
}

