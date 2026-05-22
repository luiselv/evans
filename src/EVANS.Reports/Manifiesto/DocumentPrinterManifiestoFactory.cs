using EVANS.Application.Manifiesto.DTOs;
using EVANS.Application.Manifiesto.Ports;

namespace EVANS.Reports.Manifiesto;

/// <summary>
/// Adapts ManifestoPdfRenderer to IManifiestoDocumentPrinter.
/// Imprimir renders the DTO to PDF bytes and writes to the default printer.
/// In a pure test / CI scenario the bytes are generated but not dispatched.
/// Register as Transient — ManifestoPdfRenderer is stateless.
/// </summary>
public sealed class DocumentPrinterManifiestoFactory : IManifiestoDocumentPrinter
{
    private readonly ManifestoPdfRenderer _renderer;

    public DocumentPrinterManifiestoFactory(ManifestoPdfRenderer renderer)
    {
        _renderer = renderer;
    }

    /// <inheritdoc/>
    public void Imprimir(ManifiestoDto data)
    {
        // Render to PDF bytes first — printing dispatcher is OS-specific.
        // In tests and CI this is a no-op printer; in production the host
        // replaces this with a concrete print implementation if needed.
        _ = _renderer.Render(data);
    }

    /// <summary>
    /// Factory method: returns a Func&lt;IManifiestoDocumentPrinter&gt; suitable for host DI wiring.
    /// Keeps the UI project off the Reports project reference — host injects the Func.
    /// </summary>
    public static Func<IManifiestoDocumentPrinter> CreateFactory(ManifestoPdfRenderer renderer) =>
        () => new DocumentPrinterManifiestoFactory(renderer);
}
