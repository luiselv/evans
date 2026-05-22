using EVANS.Application.Manifiesto.DTOs;

namespace EVANS.Application.Manifiesto.Ports;

public interface IManifiestoDocumentPrinter
{
    void Imprimir(ManifiestoDto data);
}
