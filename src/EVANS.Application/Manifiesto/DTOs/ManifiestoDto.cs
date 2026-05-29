namespace EVANS.Application.Manifiesto.DTOs;

public record ManifiestoDto(
    int Codigo,
    string Numero,
    DateTime Fecha,
    int TransportistaCodigo,
    string TransportistaNombre,
    int VehiculoCodigo,
    string VehiculoPlaca,
    int? CarretaCodigo,
    string? CarretaPlaca,
    int ChoferCodigo,
    string ChoferNombre,
    decimal Importe,
    decimal Peso,
    int NroGuias,
    int EstadoCodigo,
    string EstadoNombre,
    int UsuarioCodigo,
    IReadOnlyList<ManifiestoLineaDto> Lineas);
