namespace EVANS.Application.Identidad.DTOs;

public sealed record UsuarioCuentaDto(
    int Codigo,
    string NombreUsuario,
    string Clave,
    string NombreCompleto,
    bool EsAdministrador,
    int EstadoCodigo);
