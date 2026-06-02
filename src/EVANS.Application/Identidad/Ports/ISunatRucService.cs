using EVANS.Application.Identidad.DTOs;

namespace EVANS.Application.Identidad.Ports;

public interface ISunatRucService
{
    Task<SunatRucDto?> ConsultarAsync(string ruc, CancellationToken cancellationToken = default);
}
