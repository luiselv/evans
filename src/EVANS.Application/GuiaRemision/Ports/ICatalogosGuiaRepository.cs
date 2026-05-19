using EVANS.Application.GuiaRemision.DTOs;

namespace EVANS.Application.GuiaRemision.Ports;

public interface ICatalogosGuiaRepository
{
    CatalogosGuiaDto ObtenerCatalogos();
}
