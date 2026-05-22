namespace EVANS.Application.Manifiesto.DTOs;

public record ManifiestoResumenDto(
    int Codigo,
    string Numero,
    DateTime Fecha,
    string TransportistaNombre,
    string VehiculoPlaca,
    string ChoferNombre,
    decimal Importe,
    int NroGuias,
    string EstadoNombre);
