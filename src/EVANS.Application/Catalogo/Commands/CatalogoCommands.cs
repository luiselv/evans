using EVANS.Application.Catalogo.DTOs;
using EVANS.Application.Common;
using EVANS.Domain.Catalogo;
using MediatR;

namespace EVANS.Application.Catalogo.Commands;

public sealed record CreateClienteCommand(
    string RazonSocial,
    int TipoIdCodigo,
    string NroIdentificacion,
    int LongitudRequerida,
    string? Telefono,
    string? Email,
    IReadOnlyList<DireccionDto> Direcciones) : IRequest<Result<int>>;

public sealed record UpdateClienteCommand(
    int Codigo,
    string RazonSocial,
    int TipoIdCodigo,
    string NroIdentificacion,
    int LongitudRequerida,
    string? Telefono,
    string? Email,
    IReadOnlyList<DireccionDto> Direcciones) : IRequest<Result<bool>>;

public sealed record CreateEmpresaCommand(
    string RazonSocial,
    string? Direccion,
    string? Telefono,
    string Ruc,
    bool EsPropia,
    int EstadoCodigo = CatalogoEstado.Activo) : IRequest<Result<int>>;

public sealed record UpdateEmpresaCommand(
    int Codigo,
    string RazonSocial,
    string? Direccion,
    string? Telefono,
    string Ruc,
    bool EsPropia,
    int EstadoCodigo = CatalogoEstado.Activo) : IRequest<Result<bool>>;

public sealed record DeactivateEmpresaCommand(int Codigo) : IRequest<Result<bool>>;

public sealed record CreateVehiculoCommand(
    string? Marca,
    string Placa,
    string ConfiguracionVehicular,
    string? CertificadoInscripcion,
    int EmpresaCodigo,
    int EstadoCodigo = CatalogoEstado.Activo) : IRequest<Result<int>>;

public sealed record UpdateVehiculoCommand(
    int Codigo,
    string? Marca,
    string Placa,
    string ConfiguracionVehicular,
    string? CertificadoInscripcion,
    int EmpresaCodigo,
    int EstadoCodigo = CatalogoEstado.Activo) : IRequest<Result<bool>>;

public sealed record DeactivateVehiculoCommand(int Codigo) : IRequest<Result<bool>>;

public sealed record CreateCarretaCommand(
    string Placa,
    string? Marca,
    string? Certificado,
    int EmpresaCodigo,
    int EstadoCodigo = CatalogoEstado.Activo) : IRequest<Result<int>>;

public sealed record UpdateCarretaCommand(
    int Codigo,
    string Placa,
    string? Marca,
    string? Certificado,
    int EmpresaCodigo,
    int EstadoCodigo = CatalogoEstado.Activo) : IRequest<Result<bool>>;

public sealed record DeactivateCarretaCommand(int Codigo) : IRequest<Result<bool>>;

public sealed record CreateChoferCommand(
    string NombreCompleto,
    string Licencia,
    string? Telefono,
    string? Direccion,
    int EmpresaCodigo,
    int EstadoCodigo = CatalogoEstado.Activo) : IRequest<Result<int>>;

public sealed record UpdateChoferCommand(
    int Codigo,
    string NombreCompleto,
    string Licencia,
    string? Telefono,
    string? Direccion,
    int EmpresaCodigo,
    int EstadoCodigo = CatalogoEstado.Activo) : IRequest<Result<bool>>;

public sealed record DeactivateChoferCommand(int Codigo) : IRequest<Result<bool>>;

public sealed record CreateDestinoCommand(
    string Descripcion,
    double DistanciaVirtual,
    int EstadoCodigo = CatalogoEstado.Activo) : IRequest<Result<int>>;

public sealed record UpdateDestinoCommand(
    int Codigo,
    string Descripcion,
    double DistanciaVirtual,
    int EstadoCodigo = CatalogoEstado.Activo) : IRequest<Result<bool>>;

public sealed record DeactivateDestinoCommand(int Codigo) : IRequest<Result<bool>>;

public sealed record CreateEstadoCommand(string Descripcion) : IRequest<Result<int>>;

public sealed record UpdateEstadoCommand(int Codigo, string Descripcion) : IRequest<Result<bool>>;

public sealed record CreateTipoIdentificacionCommand(string Descripcion) : IRequest<Result<int>>;

public sealed record UpdateTipoIdentificacionCommand(int Codigo, string Descripcion) : IRequest<Result<bool>>;
