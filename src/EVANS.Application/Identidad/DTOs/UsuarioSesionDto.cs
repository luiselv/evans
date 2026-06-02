namespace EVANS.Application.Identidad.DTOs;

public sealed record UsuarioSesionDto(
    string NombreUsuario,
    string NombreCompleto,
    bool EsAdministrador);
