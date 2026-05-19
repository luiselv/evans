using EVANS.Application.Comprobante.DTOs;
using EVANS.Application.GuiaRemision.Ports;
using Agg = EVANS.Domain.Comprobante.Comprobante;

namespace EVANS.Application.Comprobante.Ports;

public interface IComprobanteRepository
{
    int Insertar(Agg comprobante, IUnitOfWork unitOfWork);
    void Actualizar(Agg comprobante, IUnitOfWork unitOfWork);
    void Eliminar(int codigo, IUnitOfWork unitOfWork);
    void MarcarImpreso(int codigo, IUnitOfWork unitOfWork);
    ComprobanteDto? ObtenerPorCodigo(int codigo);
    IReadOnlyList<ComprobanteResumenDto> Buscar(BuscarComprobantesFiltro filtro);
}
