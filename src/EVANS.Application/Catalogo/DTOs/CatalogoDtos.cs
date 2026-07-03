namespace EVANS.Application.Catalogo.DTOs;

public sealed record DireccionDto(string Calle, string Ciudad, string Provincia);

public sealed record ClienteDto(
    int Codigo,
    string RazonSocial,
    int TipoIdCodigo,
    string NroIdentificacion,
    string? Telefono,
    string? Fax,
    string? Email,
    string? Representante,
    IReadOnlyList<DireccionDto> Direcciones);

public sealed record EmpresaDto(
    int Codigo,
    string RazonSocial,
    string? Direccion,
    string? Telefono,
    string Ruc,
    bool EsPropia,
    int EstadoCodigo);

public sealed record VehiculoDto(
    int Codigo,
    string? Marca,
    string Placa,
    string ConfiguracionVehicular,
    string? CertificadoInscripcion,
    int EmpresaCodigo,
    int EstadoCodigo);

public sealed record CarretaDto(
    int Codigo,
    string Placa,
    string? Marca,
    string? Certificado,
    int EmpresaCodigo,
    int EstadoCodigo);

public sealed record ChoferDto(
    int Codigo,
    string NombreCompleto,
    string Licencia,
    string? Telefono,
    string? Direccion,
    int EmpresaCodigo,
    int EstadoCodigo);

public sealed record DestinoDto(
    int Codigo,
    string Descripcion,
    double DistanciaVirtual,
    int EstadoCodigo);

public sealed record EstadoDto(int Codigo, string Descripcion);

public sealed record TipoIdentificacionDto(int Codigo, string Descripcion, int LongitudRequerida);

public sealed record AgenciaDto(int Codigo, string Direccion, int DestinoCodigo, int EstadoCodigo);
