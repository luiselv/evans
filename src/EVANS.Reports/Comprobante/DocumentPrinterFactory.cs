using EVANS.Application.Comprobante.DTOs;
using EVANS.Application.Comprobante.Ports;
using EVANS.Domain.Comprobante;

namespace EVANS.Reports.Comprobante;

/// <summary>
/// Factory that dispatches rendering to the correct renderer by TipoComprobante.
/// Register as Singleton — both renderers are stateless.
/// </summary>
public sealed class DocumentPrinterFactory
{
    private readonly BoletaPdfRenderer _boletaRenderer;
    private readonly FacturaPdfRenderer _facturaRenderer;

    public DocumentPrinterFactory(BoletaPdfRenderer boletaRenderer, FacturaPdfRenderer facturaRenderer)
    {
        _boletaRenderer = boletaRenderer;
        _facturaRenderer = facturaRenderer;
    }

    /// <summary>Returns the correct IDocumentPrinter for the given TipoComprobante.</summary>
    public IDocumentPrinter For(TipoComprobante tipo) => tipo switch
    {
        TipoComprobante.Boleta => _boletaRenderer,
        TipoComprobante.Factura => _facturaRenderer,
        _ => throw new InvalidOperationException($"No renderer registered for TipoComprobante '{tipo}'.")
    };

    /// <summary>
    /// Convenience method: dispatches directly to the appropriate renderer.
    /// Adapts to IDocumentPrinter.Render(ComprobanteDto) without exposing the factory to callers.
    /// </summary>
    public byte[] Render(ComprobanteDto comprobante) =>
        For(comprobante.Tipo).Render(comprobante);
}
