using EVANS.Application.GuiaRemision.DTOs;
using EVANS.Domain.GuiaRemision;

namespace EVANS.Application.GuiaRemision.Ports;

public interface IGuiaRepository
{
    // Write methods — require an active unit of work
    int Insertar(Guia guia, IUnitOfWork uow);
    void Actualizar(Guia guia, IUnitOfWork uow);
    void Eliminar(int codigo, IUnitOfWork uow);

    // Read methods — own connection, no transaction needed
    GuiaDetalleDto? ObtenerPorCodigo(int codigo, int year);
    IReadOnlyList<GuiaResumenDto> Buscar(BuscarGuiasFiltro filtro, int year);
}
