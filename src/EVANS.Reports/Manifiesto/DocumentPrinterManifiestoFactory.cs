using System.Diagnostics;
using EVANS.Application.Manifiesto.DTOs;
using EVANS.Application.Manifiesto.Ports;
using Microsoft.Extensions.Logging;

namespace EVANS.Reports.Manifiesto;

/// <summary>
/// Adapts ManifestoPdfRenderer to IManifiestoDocumentPrinter.
/// Imprimir renders the DTO to a temp PDF file and opens it with the OS default PDF handler.
/// Best-effort: errors are logged but never thrown (same philosophy as post-commit services).
/// Register as Transient — ManifestoPdfRenderer is stateless.
/// </summary>
public sealed class DocumentPrinterManifiestoFactory : IManifiestoDocumentPrinter
{
    private readonly ManifestoPdfRenderer _renderer;
    private readonly ILogger<DocumentPrinterManifiestoFactory>? _logger;

    public DocumentPrinterManifiestoFactory(
        ManifestoPdfRenderer renderer,
        ILogger<DocumentPrinterManifiestoFactory>? logger = null)
    {
        _renderer = renderer;
        _logger = logger;
    }

    /// <inheritdoc/>
    public void Imprimir(ManifiestoDto data)
    {
        try
        {
            var bytes = _renderer.Render(data);

            // Write to a temp file with .pdf extension so the OS dispatches to the default PDF handler
            var tempPath = Path.ChangeExtension(Path.GetTempFileName(), ".pdf");
            File.WriteAllBytes(tempPath, bytes);

            Process.Start(new ProcessStartInfo
            {
                FileName = tempPath,
                UseShellExecute = true
            });
        }
        catch (Exception ex)
        {
            _logger?.LogWarning(ex,
                "PDF print dispatch failed for ManifiestoNumero={Numero} — continuing (best-effort).",
                data.Numero);
        }
    }

    /// <summary>
    /// Factory method: returns a Func&lt;IManifiestoDocumentPrinter&gt; suitable for host DI wiring.
    /// Keeps the UI project off the Reports project reference — host injects the Func.
    /// </summary>
    public static Func<IManifiestoDocumentPrinter> CreateFactory(ManifestoPdfRenderer renderer) =>
        () => new DocumentPrinterManifiestoFactory(renderer);
}
