using EVANS.Application.Comprobante.DTOs;

namespace EVANS.Application.Comprobante.Ports;

public interface IDocumentPrinter
{
    byte[] Render(ComprobanteDto comprobante);
}
