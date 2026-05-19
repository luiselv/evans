namespace EVANS.Application.GuiaRemision.DTOs;

public record CatalogosGuiaDto(
    IReadOnlyList<CatalogoItemDto> Clientes,
    IReadOnlyList<CatalogoItemDto> Destinos,
    IReadOnlyList<CatalogoItemDto> Vehiculos,
    IReadOnlyList<CatalogoItemDto> Carretas,
    IReadOnlyList<CatalogoItemDto> Choferes,
    IReadOnlyList<CatalogoItemDto> TiposComprobante,
    decimal IgvRate);

public record CatalogoItemDto(int Id, string Nombre);
