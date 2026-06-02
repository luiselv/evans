using EVANS.Application.Shared.DTOs;

namespace EVANS.Application.Identidad.DTOs;

public sealed record SesionActualDto(
    UsuarioSesionDto Usuario,
    ParametrosDto Parametros,
    int Year);
