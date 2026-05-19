using EVANS.Domain.GuiaRemision;

namespace EVANS.Application.GuiaRemision.Ports;

public interface INumeradorService
{
    NumeroGuia IncrementarYObtenerGuia();
}
