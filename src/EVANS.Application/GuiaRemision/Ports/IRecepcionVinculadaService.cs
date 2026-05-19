using EVANS.Domain.GuiaRemision;

namespace EVANS.Application.GuiaRemision.Ports;

public interface IRecepcionVinculadaService
{
    void VincularRecepcion(int recepcionId, NumeroGuia numero, int year);
}
