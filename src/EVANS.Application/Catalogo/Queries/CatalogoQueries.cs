using EVANS.Application.Catalogo.DTOs;
using EVANS.Application.Catalogo.Ports;
using EVANS.Domain.Catalogo;
using MediatR;

namespace EVANS.Application.Catalogo.Queries;

public sealed record GetClienteByIdQuery(int Codigo) : IRequest<ClienteDto?>;
public sealed record ListClientesQuery : IRequest<IReadOnlyList<ClienteDto>>;

public sealed record GetEmpresaByIdQuery(int Codigo) : IRequest<EmpresaDto?>;
public sealed record ListEmpresasQuery : IRequest<IReadOnlyList<EmpresaDto>>;
public sealed record ListEmpresasMaintenanceQuery : IRequest<IReadOnlyList<EmpresaDto>>;

public sealed record GetVehiculoByIdQuery(int Codigo) : IRequest<VehiculoDto?>;
public sealed record ListVehiculosQuery : IRequest<IReadOnlyList<VehiculoDto>>;
public sealed record ListVehiculosMaintenanceQuery : IRequest<IReadOnlyList<VehiculoDto>>;

public sealed record GetCarretaByIdQuery(int Codigo) : IRequest<CarretaDto?>;
public sealed record ListCarretasQuery : IRequest<IReadOnlyList<CarretaDto>>;
public sealed record ListCarretasMaintenanceQuery : IRequest<IReadOnlyList<CarretaDto>>;

public sealed record GetChoferByIdQuery(int Codigo) : IRequest<ChoferDto?>;
public sealed record ListChoferesQuery : IRequest<IReadOnlyList<ChoferDto>>;
public sealed record ListChoferesMaintenanceQuery : IRequest<IReadOnlyList<ChoferDto>>;

public sealed record GetDestinoByIdQuery(int Codigo) : IRequest<DestinoDto?>;
public sealed record ListDestinosQuery : IRequest<IReadOnlyList<DestinoDto>>;
public sealed record ListDestinosMaintenanceQuery : IRequest<IReadOnlyList<DestinoDto>>;

public sealed record GetEstadoByIdQuery(int Codigo) : IRequest<EstadoDto?>;
public sealed record ListEstadosQuery : IRequest<IReadOnlyList<EstadoDto>>;

public sealed record GetTipoIdentificacionByIdQuery(int Codigo) : IRequest<TipoIdentificacionDto?>;
public sealed record ListTiposIdentificacionQuery : IRequest<IReadOnlyList<TipoIdentificacionDto>>;

public sealed record GetAgenciaByIdQuery(int Codigo) : IRequest<AgenciaDto?>;
public sealed record ListAgenciasQuery : IRequest<IReadOnlyList<AgenciaDto>>;

public sealed class GetClienteByIdQueryHandler(IClienteRepository repository)
    : IRequestHandler<GetClienteByIdQuery, ClienteDto?>
{
    public async Task<ClienteDto?> Handle(GetClienteByIdQuery request, CancellationToken cancellationToken) =>
        (await repository.GetByIdAsync(request.Codigo, cancellationToken))?.ToDto();
}

public sealed class ListClientesQueryHandler(IClienteRepository repository)
    : IRequestHandler<ListClientesQuery, IReadOnlyList<ClienteDto>>
{
    public async Task<IReadOnlyList<ClienteDto>> Handle(ListClientesQuery request, CancellationToken cancellationToken) =>
        (await repository.ListAsync(cancellationToken)).Select(CatalogoMappings.ToDto).ToList();
}

public sealed class GetEmpresaByIdQueryHandler(IRepository<Empresa> repository)
    : IRequestHandler<GetEmpresaByIdQuery, EmpresaDto?>
{
    public async Task<EmpresaDto?> Handle(GetEmpresaByIdQuery request, CancellationToken cancellationToken) =>
        (await repository.GetByIdAsync(request.Codigo, cancellationToken))?.ToDto();
}

public sealed class ListEmpresasQueryHandler(IRepository<Empresa> repository)
    : IRequestHandler<ListEmpresasQuery, IReadOnlyList<EmpresaDto>>
{
    public async Task<IReadOnlyList<EmpresaDto>> Handle(ListEmpresasQuery request, CancellationToken cancellationToken) =>
        (await repository.ListActiveAsync(cancellationToken)).Select(CatalogoMappings.ToDto).ToList();
}

public sealed class ListEmpresasMaintenanceQueryHandler(IEmpresaMaintenanceRepository repository)
    : IRequestHandler<ListEmpresasMaintenanceQuery, IReadOnlyList<EmpresaDto>>
{
    public async Task<IReadOnlyList<EmpresaDto>> Handle(ListEmpresasMaintenanceQuery request, CancellationToken cancellationToken) =>
        (await repository.ListAllAsync(cancellationToken)).Select(CatalogoMappings.ToDto).ToList();
}

public sealed class GetVehiculoByIdQueryHandler(IRepository<Vehiculo> repository)
    : IRequestHandler<GetVehiculoByIdQuery, VehiculoDto?>
{
    public async Task<VehiculoDto?> Handle(GetVehiculoByIdQuery request, CancellationToken cancellationToken) =>
        (await repository.GetByIdAsync(request.Codigo, cancellationToken))?.ToDto();
}

public sealed class ListVehiculosQueryHandler(IRepository<Vehiculo> repository)
    : IRequestHandler<ListVehiculosQuery, IReadOnlyList<VehiculoDto>>
{
    public async Task<IReadOnlyList<VehiculoDto>> Handle(ListVehiculosQuery request, CancellationToken cancellationToken) =>
        (await repository.ListActiveAsync(cancellationToken)).Select(CatalogoMappings.ToDto).ToList();
}

public sealed class ListVehiculosMaintenanceQueryHandler(IVehiculoMaintenanceRepository repository)
    : IRequestHandler<ListVehiculosMaintenanceQuery, IReadOnlyList<VehiculoDto>>
{
    public async Task<IReadOnlyList<VehiculoDto>> Handle(ListVehiculosMaintenanceQuery request, CancellationToken cancellationToken) =>
        (await repository.ListAllAsync(cancellationToken)).Select(CatalogoMappings.ToDto).ToList();
}

public sealed class GetCarretaByIdQueryHandler(IRepository<Carreta> repository)
    : IRequestHandler<GetCarretaByIdQuery, CarretaDto?>
{
    public async Task<CarretaDto?> Handle(GetCarretaByIdQuery request, CancellationToken cancellationToken) =>
        (await repository.GetByIdAsync(request.Codigo, cancellationToken))?.ToDto();
}

public sealed class ListCarretasQueryHandler(IRepository<Carreta> repository)
    : IRequestHandler<ListCarretasQuery, IReadOnlyList<CarretaDto>>
{
    public async Task<IReadOnlyList<CarretaDto>> Handle(ListCarretasQuery request, CancellationToken cancellationToken) =>
        (await repository.ListActiveAsync(cancellationToken)).Select(CatalogoMappings.ToDto).ToList();
}

public sealed class ListCarretasMaintenanceQueryHandler(ICarretaMaintenanceRepository repository)
    : IRequestHandler<ListCarretasMaintenanceQuery, IReadOnlyList<CarretaDto>>
{
    public async Task<IReadOnlyList<CarretaDto>> Handle(ListCarretasMaintenanceQuery request, CancellationToken cancellationToken) =>
        (await repository.ListAllAsync(cancellationToken)).Select(CatalogoMappings.ToDto).ToList();
}

public sealed class GetChoferByIdQueryHandler(IRepository<Chofer> repository)
    : IRequestHandler<GetChoferByIdQuery, ChoferDto?>
{
    public async Task<ChoferDto?> Handle(GetChoferByIdQuery request, CancellationToken cancellationToken) =>
        (await repository.GetByIdAsync(request.Codigo, cancellationToken))?.ToDto();
}

public sealed class ListChoferesQueryHandler(IRepository<Chofer> repository)
    : IRequestHandler<ListChoferesQuery, IReadOnlyList<ChoferDto>>
{
    public async Task<IReadOnlyList<ChoferDto>> Handle(ListChoferesQuery request, CancellationToken cancellationToken) =>
        (await repository.ListActiveAsync(cancellationToken)).Select(CatalogoMappings.ToDto).ToList();
}

public sealed class ListChoferesMaintenanceQueryHandler(IChoferMaintenanceRepository repository)
    : IRequestHandler<ListChoferesMaintenanceQuery, IReadOnlyList<ChoferDto>>
{
    public async Task<IReadOnlyList<ChoferDto>> Handle(ListChoferesMaintenanceQuery request, CancellationToken cancellationToken) =>
        (await repository.ListAllAsync(cancellationToken)).Select(CatalogoMappings.ToDto).ToList();
}

public sealed class GetDestinoByIdQueryHandler(IRepository<Destino> repository)
    : IRequestHandler<GetDestinoByIdQuery, DestinoDto?>
{
    public async Task<DestinoDto?> Handle(GetDestinoByIdQuery request, CancellationToken cancellationToken) =>
        (await repository.GetByIdAsync(request.Codigo, cancellationToken))?.ToDto();
}

public sealed class ListDestinosQueryHandler(IRepository<Destino> repository)
    : IRequestHandler<ListDestinosQuery, IReadOnlyList<DestinoDto>>
{
    public async Task<IReadOnlyList<DestinoDto>> Handle(ListDestinosQuery request, CancellationToken cancellationToken) =>
        (await repository.ListActiveAsync(cancellationToken)).Select(CatalogoMappings.ToDto).ToList();
}

public sealed class ListDestinosMaintenanceQueryHandler(IDestinoMaintenanceRepository repository)
    : IRequestHandler<ListDestinosMaintenanceQuery, IReadOnlyList<DestinoDto>>
{
    public async Task<IReadOnlyList<DestinoDto>> Handle(ListDestinosMaintenanceQuery request, CancellationToken cancellationToken) =>
        (await repository.ListAllAsync(cancellationToken)).Select(CatalogoMappings.ToDto).ToList();
}

public sealed class GetEstadoByIdQueryHandler(IEstadoRepository repository)
    : IRequestHandler<GetEstadoByIdQuery, EstadoDto?>
{
    public async Task<EstadoDto?> Handle(GetEstadoByIdQuery request, CancellationToken cancellationToken) =>
        (await repository.GetByIdAsync(request.Codigo, cancellationToken))?.ToDto();
}

public sealed class ListEstadosQueryHandler(IEstadoRepository repository)
    : IRequestHandler<ListEstadosQuery, IReadOnlyList<EstadoDto>>
{
    public async Task<IReadOnlyList<EstadoDto>> Handle(ListEstadosQuery request, CancellationToken cancellationToken) =>
        (await repository.ListAsync(cancellationToken)).Select(CatalogoMappings.ToDto).ToList();
}

public sealed class GetTipoIdentificacionByIdQueryHandler(ITipoIdentificacionRepository repository)
    : IRequestHandler<GetTipoIdentificacionByIdQuery, TipoIdentificacionDto?>
{
    public async Task<TipoIdentificacionDto?> Handle(GetTipoIdentificacionByIdQuery request, CancellationToken cancellationToken) =>
        (await repository.GetByIdAsync(request.Codigo, cancellationToken))?.ToDto();
}

public sealed class ListTiposIdentificacionQueryHandler(ITipoIdentificacionRepository repository)
    : IRequestHandler<ListTiposIdentificacionQuery, IReadOnlyList<TipoIdentificacionDto>>
{
    public async Task<IReadOnlyList<TipoIdentificacionDto>> Handle(ListTiposIdentificacionQuery request, CancellationToken cancellationToken) =>
        (await repository.ListAsync(cancellationToken)).Select(CatalogoMappings.ToDto).ToList();
}

public sealed class GetAgenciaByIdQueryHandler(IAgenciaRepository repository)
    : IRequestHandler<GetAgenciaByIdQuery, AgenciaDto?>
{
    public async Task<AgenciaDto?> Handle(GetAgenciaByIdQuery request, CancellationToken cancellationToken) =>
        (await repository.GetByIdAsync(request.Codigo, cancellationToken))?.ToDto();
}

public sealed class ListAgenciasQueryHandler(IAgenciaRepository repository)
    : IRequestHandler<ListAgenciasQuery, IReadOnlyList<AgenciaDto>>
{
    public async Task<IReadOnlyList<AgenciaDto>> Handle(ListAgenciasQuery request, CancellationToken cancellationToken) =>
        (await repository.ListActiveAsync(cancellationToken)).Select(CatalogoMappings.ToDto).ToList();
}

internal static class CatalogoMappings
{
    public static ClienteDto ToDto(this Cliente cliente) => new(
        cliente.Codigo,
        cliente.RazonSocial,
        cliente.TipoIdCodigo,
        cliente.NroIdentificacion,
        cliente.Telefono,
        cliente.Email,
        cliente.Direcciones.Select(d => new DireccionDto(d.Calle, d.Ciudad, d.Provincia)).ToList());

    public static EmpresaDto ToDto(this Empresa empresa) => new(
        empresa.Codigo,
        empresa.RazonSocial,
        empresa.Direccion,
        empresa.Telefono,
        empresa.Ruc.Value,
        empresa.EsPropia,
        empresa.EstadoCodigo);

    public static VehiculoDto ToDto(this Vehiculo vehiculo) => new(
        vehiculo.Codigo,
        vehiculo.Marca,
        vehiculo.Placa,
        vehiculo.ConfiguracionVehicular,
        vehiculo.CertificadoInscripcion,
        vehiculo.EmpresaCodigo,
        vehiculo.EstadoCodigo);

    public static CarretaDto ToDto(this Carreta carreta) => new(
        carreta.Codigo,
        carreta.Placa,
        carreta.Marca,
        carreta.Certificado,
        carreta.EmpresaCodigo,
        carreta.EstadoCodigo);

    public static ChoferDto ToDto(this Chofer chofer) => new(
        chofer.Codigo,
        chofer.NombreCompleto,
        chofer.Licencia,
        chofer.Telefono,
        chofer.Direccion,
        chofer.EmpresaCodigo,
        chofer.EstadoCodigo);

    public static DestinoDto ToDto(this Destino destino) => new(
        destino.Codigo,
        destino.Descripcion,
        destino.DistanciaVirtual,
        destino.EstadoCodigo);

    public static EstadoDto ToDto(this Estado estado) => new(estado.Codigo, estado.Descripcion);

    public static TipoIdentificacionDto ToDto(this TipoIdentificacion tipo) =>
        new(tipo.Codigo, tipo.Descripcion, tipo.LongitudRequerida);

    public static AgenciaDto ToDto(this Agencia agencia) =>
        new(agencia.Codigo, agencia.Direccion, agencia.DestinoCodigo, agencia.EstadoCodigo);
}
