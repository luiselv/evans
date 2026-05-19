using EVANS.Application.Common;
using EVANS.Application.Comprobante.Commands;
using EVANS.Application.Comprobante.DTOs;
using EVANS.Application.Comprobante.Ports;
using EVANS.Application.Comprobante.Queries;
using EVANS.Domain.Comprobante;
using MediatR;

namespace EVANS.UI.WinForms.Comprobante;

/// <summary>
/// New C# WinForms form for Comprobante (Boleta / Factura).
/// Injected with IMediator and a printer resolver delegate — no direct SQL, no objConexion.
/// When opened from a GuiaRemision context, caller passes the guiaRef string;
/// the form builds OrigenComprobante.DesdeGuia automatically.
/// </summary>
public partial class frmComprobante : Form
{
    private readonly IMediator _mediator;
    private readonly string? _guiaRef;             // null = Standalone; non-null = DesdeGuia
    private readonly Func<TipoComprobante, IDocumentPrinter>? _printerResolver;

    // Holds the codigo assigned by the handler after a successful save.
    private int? _savedCodigo;

    // Test-settable fields (populated from controls in normal use; set directly in tests)
    internal int _clienteCodigo;
    internal string _rucODni = string.Empty;
    internal string _direccion = string.Empty;
    internal TipoComprobante _tipo = TipoComprobante.Boleta;

    /// <param name="mediator">MediatR mediator (required).</param>
    /// <param name="guiaRef">
    ///     Optional GuiaRemision reference string. When non-null the form is in
    ///     "desde guia" mode and the GuiaRef field is pre-filled and read-only.
    /// </param>
    /// <param name="printerResolver">
    ///     Factory delegate that returns the correct IDocumentPrinter for the given tipo.
    ///     Injected from Host so that UI.WinForms does not take a dependency on EVANS.Reports.
    /// </param>
    public frmComprobante(
        IMediator mediator,
        string? guiaRef = null,
        Func<TipoComprobante, IDocumentPrinter>? printerResolver = null)
    {
        InitializeComponent();
        _mediator        = mediator;
        _guiaRef         = guiaRef;
        _printerResolver = printerResolver;

        // Populate tipo combo
        cmbTipoComprobante.Items.AddRange(["Boleta", "Factura"]);
        cmbTipoComprobante.SelectedIndex = 0;

        // If opened from a Guia context, show the reference
        if (!string.IsNullOrWhiteSpace(_guiaRef))
        {
            lblGuiaRef.Visible = true;
            lblGuiaRef.Text    = $"Guía: {_guiaRef}";
        }

        // Wire combo selection change to update internal _clienteCodigo
        cmbCliente.SelectedIndexChanged += (_, _) =>
        {
            if (cmbCliente.SelectedValue is int id) _clienteCodigo = id;
        };
    }

    // ----------------------------------------------------------------
    // Button handlers
    // ----------------------------------------------------------------

    private async void btnGuardar_Click(object? sender, EventArgs e)
    {
        btnGuardar.Enabled = false;
        try
        {
            await GuardarAsync();
        }
        finally
        {
            btnGuardar.Enabled = true;
        }
    }

    private async void btnImprimir_Click(object? sender, EventArgs e)
    {
        if (_savedCodigo is null)
        {
            MessageBox.Show("Guarde el comprobante antes de imprimir.",
                "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        btnImprimir.Enabled = false;
        try
        {
            await ImprimirAsync(_savedCodigo.Value);
        }
        finally
        {
            btnImprimir.Enabled = true;
        }
    }

    private void btnCancelar_Click(object? sender, EventArgs e) => Close();

    // ----------------------------------------------------------------
    // Core operations (internal for testability)
    // ----------------------------------------------------------------

    internal async Task<bool> GuardarAsync()
    {
        SyncFieldsFromControls();
        var detalles = ReadDetallesFromGrid();

        var cmd = new CrearComprobanteCommand(
            Tipo: _tipo,
            ClienteCodigo: _clienteCodigo,
            RucODni: _rucODni,
            Direccion: _direccion,
            Detalles: detalles,
            GuiaRef: string.IsNullOrWhiteSpace(_guiaRef) ? null : _guiaRef,
            Year: DateTime.Today.Year);

        Result<int?> result;
        try
        {
            result = await _mediator.Send(cmd);
        }
        catch (FluentValidation.ValidationException vex)
        {
            var errors = string.Join(Environment.NewLine,
                vex.Errors.Select(err => err.ErrorMessage));
            MessageBox.Show(errors, "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return false;
        }
        catch (DomainException dex)
        {
            MessageBox.Show(dex.Message, "Error de negocio",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
            return false;
        }

        if (!result.IsSuccess)
        {
            MessageBox.Show(result.Error, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return false;
        }

        _savedCodigo       = result.Value;
        btnImprimir.Enabled = true;
        lblNumero.Text      = $"Comprobante #{_savedCodigo}";

        return true;
    }

    internal async Task ImprimirAsync(int codigo)
    {
        ComprobanteDto? dto;
        try
        {
            dto = await _mediator.Send(new ObtenerComprobantePorCodigoQuery(codigo));
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error al recuperar el comprobante: {ex.Message}",
                "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }

        if (dto is null)
        {
            MessageBox.Show("Comprobante no encontrado.",
                "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }

        if (_printerResolver is null)
        {
            MessageBox.Show("No hay renderizador PDF configurado.",
                "Configuración", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        var printer = _printerResolver(dto.Tipo);
        byte[] pdfBytes;
        try
        {
            pdfBytes = printer.Render(dto);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error al generar PDF: {ex.Message}",
                "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }

        // Send to printer via PrintDocument (PDF bytes available for preview extensions)
        using var printDoc = new System.Drawing.Printing.PrintDocument();
        printDoc.PrintPage += (_, args) =>
        {
            // Simplified: in production use a PDF viewer/print stream
            args.HasMorePages = false;
        };

        try
        {
            printDoc.Print();

            // Mark as printed — best-effort
            try
            {
                await _mediator.Send(new MarcarImpresoCommand(codigo, DateTime.Today.Year));
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(
                    $"MarcarImpreso failed for codigo={codigo}: {ex.Message}");
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error al imprimir: {ex.Message}",
                "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        _ = pdfBytes; // available for future preview / save-to-disk feature
    }

    // ----------------------------------------------------------------
    // Designer event handlers
    // ----------------------------------------------------------------

    private void cmbTipoComprobante_SelectedIndexChanged(object? sender, EventArgs e)
    {
        _tipo = cmbTipoComprobante.SelectedIndex == 1
            ? TipoComprobante.Factura
            : TipoComprobante.Boleta;

        lblRucDni.Text = _tipo == TipoComprobante.Factura ? "RUC:" : "DNI:";
    }

    // ----------------------------------------------------------------
    // Test-support helpers
    // ----------------------------------------------------------------

    /// <summary>Called by tests to set form fields without UI interaction.</summary>
    internal void SetTestFields(
        int clienteCodigo,
        string rucODni,
        string direccion,
        TipoComprobante tipo)
    {
        _clienteCodigo = clienteCodigo;
        _rucODni       = rucODni;
        _direccion     = direccion;
        _tipo          = tipo;
    }

    // ----------------------------------------------------------------
    // Private helpers
    // ----------------------------------------------------------------

    private void SyncFieldsFromControls()
    {
        if (cmbCliente.SelectedValue is int id && id > 0) _clienteCodigo = id;

        var rawRuc = txtRucDni.Text.Trim();
        if (!string.IsNullOrWhiteSpace(rawRuc)) _rucODni = rawRuc;

        var rawDir = txtDireccion.Text.Trim();
        if (!string.IsNullOrWhiteSpace(rawDir)) _direccion = rawDir;

        _tipo = cmbTipoComprobante.SelectedIndex == 1
            ? TipoComprobante.Factura
            : TipoComprobante.Boleta;
    }

    private IReadOnlyList<DetalleComprobanteInput> ReadDetallesFromGrid()
    {
        var result = new List<DetalleComprobanteInput>();

        foreach (DataGridViewRow row in dgvDetalles.Rows)
        {
            if (row.IsNewRow) continue;

            var cantidad       = Convert.ToInt32(row.Cells["colCantidad"].Value ?? 1);
            var descripcion    = row.Cells["colDescripcion"].Value?.ToString() ?? string.Empty;
            var precioUnitario = Convert.ToDecimal(row.Cells["colPrecioUnitario"].Value ?? 0m);
            var flete          = Convert.ToDecimal(row.Cells["colFlete"].Value ?? 0m);

            result.Add(new DetalleComprobanteInput(cantidad, descripcion, precioUnitario, flete));
        }

        return result;
    }
}
