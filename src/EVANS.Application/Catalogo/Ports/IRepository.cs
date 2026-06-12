using EVANS.Domain.Catalogo;

namespace EVANS.Application.Catalogo.Ports;

/// <summary>
/// Repository contract for catalog entities backed by an ESTA_CODIGO status column.
/// </summary>
/// <remarks>Hard deletes are intentionally not supported.</remarks>
public interface IRepository<T> where T : class
{
    Task<T?> GetByIdAsync(int codigo, CancellationToken ct);
    Task<IReadOnlyList<T>> ListActiveAsync(CancellationToken ct);
    Task<int> AddAsync(T entity, CancellationToken ct);
    Task UpdateAsync(T entity, CancellationToken ct);
    Task DeactivateAsync(int codigo, CancellationToken ct);
}

/// <summary>
/// Destino maintenance screens must match the legacy form, which lists every row regardless of ESTA_CODIGO.
/// </summary>
public interface IDestinoMaintenanceRepository
{
    Task<IReadOnlyList<Destino>> ListAllAsync(CancellationToken ct);
}

/// <summary>
/// Empresa maintenance screens must match the legacy form, which lists every row regardless of ESTA_CODIGO.
/// </summary>
public interface IEmpresaMaintenanceRepository
{
    Task<IReadOnlyList<Empresa>> ListAllAsync(CancellationToken ct);
}

/// <summary>
/// Chofer maintenance screens must match the legacy form, which lists every row regardless of ESTA_CODIGO.
/// </summary>
public interface IChoferMaintenanceRepository
{
    Task<IReadOnlyList<Chofer>> ListAllAsync(CancellationToken ct);
}

/// <summary>
/// Carreta maintenance screens must match the legacy form, which lists every row regardless of ESTA_CODIGO.
/// </summary>
public interface ICarretaMaintenanceRepository
{
    Task<IReadOnlyList<Carreta>> ListAllAsync(CancellationToken ct);
}

/// <summary>
/// Cliente has no ESTA_CODIGO in the legacy schema, so it has no deactivate operation.
/// </summary>
public interface IClienteRepository
{
    Task<Cliente?> GetByIdAsync(int codigo, CancellationToken ct);
    Task<IReadOnlyList<Cliente>> ListAsync(CancellationToken ct);
    Task<int> AddAsync(Cliente cliente, CancellationToken ct);
    Task UpdateAsync(Cliente cliente, CancellationToken ct);
}

/// <summary>
/// Estado uses ESTA_CODIGO as its primary key, not as a status column.
/// </summary>
public interface IEstadoRepository
{
    Task<Estado?> GetByIdAsync(int codigo, CancellationToken ct);
    Task<IReadOnlyList<Estado>> ListAsync(CancellationToken ct);
    Task<int> AddAsync(Estado estado, CancellationToken ct);
    Task UpdateAsync(Estado estado, CancellationToken ct);
}

/// <summary>
/// TipoIdentificacion has no ESTA_CODIGO in the legacy schema, so it has no deactivate operation.
/// </summary>
public interface ITipoIdentificacionRepository
{
    Task<TipoIdentificacion?> GetByIdAsync(int codigo, CancellationToken ct);
    Task<IReadOnlyList<TipoIdentificacion>> ListAsync(CancellationToken ct);
    Task<int> AddAsync(TipoIdentificacion tipoIdentificacion, CancellationToken ct);
    Task UpdateAsync(TipoIdentificacion tipoIdentificacion, CancellationToken ct);
}

/// <summary>
/// Agencia is read-only in the legacy application.
/// </summary>
public interface IAgenciaRepository
{
    Task<Agencia?> GetByIdAsync(int codigo, CancellationToken ct);
    Task<IReadOnlyList<Agencia>> ListActiveAsync(CancellationToken ct);
}
