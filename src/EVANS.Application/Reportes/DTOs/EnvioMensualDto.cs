namespace EVANS.Application.Reportes.DTOs;

public sealed record EnvioMensualDto(
    string Cliente,
    int NroGuias,
    DateTime UltimoEnvio);
