namespace EVANS.Application.Manifiesto.DTOs;

public record TransportistaDto(int Codigo, string RazonSocial);
public record VehiculoDto(int Codigo, string Placa);
public record CarretaDto(int Codigo, string Placa);
public record ChoferDto(int Codigo, string NombreCompleto);
public record EstadoDto(int Codigo, string Descripcion);

public record CatalogosManifiestoDto(
    IReadOnlyList<TransportistaDto> Transportistas,
    IReadOnlyList<VehiculoDto> Vehiculos,
    IReadOnlyList<CarretaDto> Carretas,
    IReadOnlyList<ChoferDto> Choferes,
    IReadOnlyList<EstadoDto> Estados);
