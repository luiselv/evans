using EVANS.Application.Catalogo.DTOs;
using EVANS.Application.Common;
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
    bool EsPropia) : IRequest<Result<int>>;

public sealed record UpdateEmpresaCommand(
    int Codigo,
    string RazonSocial,
    string? Direccion,
    string? Telefono,
    string Ruc,
    bool EsPropia) : IRequest<Result<bool>>;

public sealed record DeactivateEmpresaCommand(int Codigo) : IRequest<Result<bool>>;

public sealed record CreateVehiculoCommand(
    string? Marca,
    string Placa,
    string ConfiguracionVehicular,
    string? CertificadoInscripcion,
    int EmpresaCodigo) : IRequest<Result<int>>;

public sealed record UpdateVehiculoCommand(
    int Codigo,
    string? Marca,
    string Placa,
    string ConfiguracionVehicular,
    string? CertificadoInscripcion,
    int EmpresaCodigo) : IRequest<Result<bool>>;

public sealed record DeactivateVehiculoCommand(int Codigo) : IRequest<Result<bool>>;

public sealed record CreateCarretaCommand(
    string Placa,
    string? Marca,
    string? Certificado,
    int EmpresaCodigo) : IRequest<Result<int>>;

public sealed record UpdateCarretaCommand(
    int Codigo,
    string Placa,
    string? Marca,
    string? Certificado,
    int EmpresaCodigo) : IRequest<Result<bool>>;

public sealed record DeactivateCarretaCommand(int Codigo) : IRequest<Result<bool>>;

public sealed record CreateChoferCommand(
    string NombreCompleto,
    string Licencia,
    string? Telefono,
    string? Direccion,
    int EmpresaCodigo) : IRequest<Result<int>>;

public sealed record UpdateChoferCommand(
    int Codigo,
    string NombreCompleto,
    string Licencia,
    string? Telefono,
    string? Direccion,
    int EmpresaCodigo) : IRequest<Result<bool>>;

public sealed record DeactivateChoferCommand(int Codigo) : IRequest<Result<bool>>;

public sealed record CreateDestinoCommand(string Descripcion, double DistanciaVirtual) : IRequest<Result<int>>;

public sealed record UpdateDestinoCommand(int Codigo, string Descripcion, double DistanciaVirtual) : IRequest<Result<bool>>;

public sealed record DeactivateDestinoCommand(int Codigo) : IRequest<Result<bool>>;

public sealed record CreateEstadoCommand(string Descripcion) : IRequest<Result<int>>;

public sealed record UpdateEstadoCommand(int Codigo, string Descripcion) : IRequest<Result<bool>>;

public sealed record CreateTipoIdentificacionCommand(string Descripcion) : IRequest<Result<int>>;

public sealed record UpdateTipoIdentificacionCommand(int Codigo, string Descripcion) : IRequest<Result<bool>>;
