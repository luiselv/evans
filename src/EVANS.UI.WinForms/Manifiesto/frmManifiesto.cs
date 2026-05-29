using EVANS.Application.Common;
using EVANS.Application.Manifiesto.Commands;
using EVANS.Application.Manifiesto.DTOs;
using EVANS.Application.Manifiesto.Handlers;
using EVANS.Application.Manifiesto.Ports;
using EVANS.Application.Manifiesto.Queries;
using EVANS.Domain.Manifiesto;
using MediatR;

namespace EVANS.UI.WinForms.Manifiesto;

/// <summary>
/// New C# WinForms form for Manifiesto management.
/// Injected with IMediator and a printer factory delegate — no direct SQL, no objConexion.
/// </summary>
public partial class frmManifiesto : Form
{
    private readonly IMediator _mediator;
    private readonly ICatalogosManifiestoRepository _catalogos;
    private readonly Func<IManifiestoDocumentPrinter>? _printerFactory;

    // Holds the codigo assigned after a successful save; null = new (unsaved) record.
    private int? _savedCodigo;

    // Year used for yearly-DB queries (defaults to current year)
    private readonly int _year;

    // Test-settable fields
    internal int _transportistaCodigo;
    internal int _vehiculoCodigo;
    internal int? _carretaCodigo;
    internal int _choferCodigo;
    internal int _estadoCodigo;
    internal int _usuarioCodigo = 1;
    internal decimal _importe;
    internal decimal _peso;
    internal DateTime _fecha = DateTime.Today;
    internal IReadOnlyList<int> _guiaIds = [];

    /// <param name="mediator">MediatR mediator (required).</param>
    /// <param name="catalogos">Catalogs repository for combo population (required).</param>
    /// <param name="printerFactory">
    ///     Factory delegate that returns an IManifiestoDocumentPrinter on demand.
    ///     Injected from Host so UI.WinForms does not take a dependency on EVANS.Reports.
    /// </param>
    /// <param name="year">Yearly DB year; defaults to current year.</param>
    public frmManifiesto(
        IMediator mediator,
        ICatalogosManifiestoRepository catalogos,
        Func<IManifiestoDocumentPrinter>? printerFactory = null,
        int year = 0)
    {
        InitializeComponent();
        _mediator       = mediator;
        _catalogos      = catalogos;
        _printerFactory = printerFactory;
        _year           = year > 0 ? year : DateTime.Today.Year;

        Load += frmManifiesto_Load;

        cmbTransportista.SelectedIndexChanged += (_, _) =>
        {
            if (cmbTransportista.SelectedValue is int id) _transportistaCodigo = id;
        };
        cmbVehiculo.SelectedIndexChanged += (_, _) =>
        {
            if (cmbVehiculo.SelectedValue is int id) _vehiculoCodigo = id;
        };
        cmbCarreta.SelectedIndexChanged += (_, _) =>
        {
            _carretaCodigo = cmbCarreta.SelectedValue is int id && id > 0 ? id : null;
        };
        cmbChofer.SelectedIndexChanged += (_, _) =>
        {
            if (cmbChofer.SelectedValue is int id) _choferCodigo = id;
        };
        cmbEstado.SelectedIndexChanged += (_, _) =>
        {
            if (cmbEstado.SelectedValue is int id) _estadoCodigo = id;
        };
    }

    // ----------------------------------------------------------------
    // Form load
    // ----------------------------------------------------------------

    private async void frmManifiesto_Load(object? sender, EventArgs e)
    {
        await LoadCatalogosAsync();
        await RefreshGridAsync();
    }

    // ----------------------------------------------------------------
    // Button handlers
    // ----------------------------------------------------------------

    private void btnNuevo_Click(object? sender, EventArgs e) => ClearForm();

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

    private async void btnEliminar_Click(object? sender, EventArgs e)
    {
        if (_savedCodigo is null) return;
        var confirm = MessageBox.Show(
            $"¿Eliminar el manifiesto {lblNumero.Text}?",
            "Confirmar eliminación",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Question);
        if (confirm != DialogResult.Yes) return;

        btnEliminar.Enabled = false;
        try
        {
            await EliminarAsync(_savedCodigo.Value);
        }
        finally
        {
            btnEliminar.Enabled = _savedCodigo.HasValue;
        }
    }

    private async void btnImprimir_Click(object? sender, EventArgs e)
    {
        if (_savedCodigo is null)
        {
            MessageBox.Show("Guarde el manifiesto antes de imprimir.",
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
            btnImprimir.Enabled = _savedCodigo.HasValue;
        }
    }

    private async void btnBuscar_Click(object? sender, EventArgs e)
    {
        await RefreshGridAsync();
    }

    private void btnCancelar_Click(object? sender, EventArgs e) => Close();

    // ----------------------------------------------------------------
    // Grid selection
    // ----------------------------------------------------------------

    private void dgvManifiestos_SelectionChanged(object? sender, EventArgs e)
    {
        if (dgvManifiestos.CurrentRow?.DataBoundItem is ManifiestoResumenDto resumen)
        {
            _ = LoadManifiestoAsync(resumen.Codigo);
        }
    }

    // ----------------------------------------------------------------
    // Core operations (internal for testability)
    // ----------------------------------------------------------------

    internal async Task<bool> GuardarAsync()
    {
        SyncFieldsFromControls();

        Result<int?> result;

        if (_savedCodigo is null)
        {
            var cmd = new CrearManifiestoCommand(
                Fecha: _fecha,
                TransportistaCodigo: _transportistaCodigo,
                VehiculoCodigo: _vehiculoCodigo,
                CarretaCodigo: _carretaCodigo,
                ChoferCodigo: _choferCodigo,
                Importe: _importe,
                Peso: _peso,
                EstadoCodigo: _estadoCodigo,
                UsuarioCodigo: _usuarioCodigo,
                GuiaIds: _guiaIds,
                Year: _year);

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
        }
        else
        {
            var cmd = new ActualizarManifiestoCommand(
                Codigo: _savedCodigo.Value,
                Fecha: _fecha,
                TransportistaCodigo: _transportistaCodigo,
                VehiculoCodigo: _vehiculoCodigo,
                CarretaCodigo: _carretaCodigo,
                ChoferCodigo: _choferCodigo,
                Importe: _importe,
                Peso: _peso,
                EstadoCodigo: _estadoCodigo,
                UsuarioCodigo: _usuarioCodigo,
                GuiaIds: _guiaIds,
                Year: _year);

            Result<bool> updateResult;
            try
            {
                updateResult = await _mediator.Send(cmd);
            }
            catch (DomainException dex)
            {
                MessageBox.Show(dex.Message, "Error de negocio",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (!updateResult.IsSuccess)
            {
                MessageBox.Show(updateResult.Error, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            await RefreshGridAsync();
            return true;
        }

        if (!result.IsSuccess)
        {
            MessageBox.Show(result.Error, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return false;
        }

        _savedCodigo        = result.Value;
        btnEliminar.Enabled = true;
        btnImprimir.Enabled = true;
        lblNumero.Text      = $"Manifiesto #{_savedCodigo}";

        await RefreshGridAsync();
        return true;
    }

    internal async Task EliminarAsync(int codigo)
    {
        var cmd = new EliminarManifiestoCommand(Codigo: codigo, Year: _year);

        Result<bool> result;
        try
        {
            result = await _mediator.Send(cmd);
        }
        catch (DomainException dex)
        {
            MessageBox.Show(dex.Message, "Error de negocio",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }

        if (!result.IsSuccess)
        {
            MessageBox.Show(result.Error, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }

        ClearForm();
        await RefreshGridAsync();
        MessageBox.Show("Manifiesto eliminado correctamente.", "Información",
            MessageBoxButtons.OK, MessageBoxIcon.Information);
    }

    internal async Task ImprimirAsync(int codigo)
    {
        ManifiestoDto? dto;
        try
        {
            dto = await _mediator.Send(new ObtenerManifiestoPorCodigoQuery(codigo, _year));
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error al recuperar el manifiesto: {ex.Message}",
                "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }

        if (dto is null)
        {
            MessageBox.Show("Manifiesto no encontrado.",
                "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }

        if (_printerFactory is null)
        {
            MessageBox.Show("No hay renderizador PDF configurado.",
                "Configuración", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        var printer = _printerFactory();
        try
        {
            printer.Imprimir(dto);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error al imprimir: {ex.Message}",
                "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    // ----------------------------------------------------------------
    // Test-support helpers
    // ----------------------------------------------------------------

    /// <summary>Called by tests to set form fields without UI interaction.</summary>
    internal void SetTestFields(
        int transportistaCodigo,
        int vehiculoCodigo,
        int? carretaCodigo,
        int choferCodigo,
        int estadoCodigo,
        decimal importe,
        decimal peso,
        DateTime fecha,
        IReadOnlyList<int> guiaIds,
        int? savedCodigo = null)
    {
        _transportistaCodigo = transportistaCodigo;
        _vehiculoCodigo      = vehiculoCodigo;
        _carretaCodigo       = carretaCodigo;
        _choferCodigo        = choferCodigo;
        _estadoCodigo        = estadoCodigo;
        _importe             = importe;
        _peso                = peso;
        _fecha               = fecha;
        _guiaIds             = guiaIds;
        if (savedCodigo.HasValue) _savedCodigo = savedCodigo;
    }

    // ----------------------------------------------------------------
    // Private helpers
    // ----------------------------------------------------------------

    private async Task LoadCatalogosAsync()
    {
        try
        {
            var catalogosDto = await _catalogos.ObtenerCatalogosAsync(CancellationToken.None);

            PopulateCombo(cmbTransportista, catalogosDto.Transportistas,
                t => t.Codigo, t => t.RazonSocial, withBlank: false);
            PopulateCombo(cmbVehiculo, catalogosDto.Vehiculos,
                v => v.Codigo, v => v.Placa, withBlank: false);
            PopulateCombo(cmbCarreta, catalogosDto.Carretas,
                c => c.Codigo, c => c.Placa, withBlank: true);
            PopulateCombo(cmbChofer, catalogosDto.Choferes,
                c => c.Codigo, c => c.NombreCompleto, withBlank: false);
            PopulateCombo(cmbEstado, catalogosDto.Estados,
                e => e.Codigo, e => e.Descripcion, withBlank: false);
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"LoadCatalogos failed: {ex.Message}");
        }
    }

    private static void PopulateCombo<T>(
        ComboBox combo,
        IReadOnlyList<T> items,
        Func<T, int> keySelector,
        Func<T, string> displaySelector,
        bool withBlank)
    {
        var source = new List<KeyValuePair<int, string>>();
        if (withBlank) source.Add(new KeyValuePair<int, string>(0, "(ninguna)"));
        source.AddRange(items.Select(i => new KeyValuePair<int, string>(keySelector(i), displaySelector(i))));

        combo.DataSource    = source;
        combo.DisplayMember = "Value";
        combo.ValueMember   = "Key";
    }

    private async Task RefreshGridAsync()
    {
        var buscar = txtBuscar.Text.Trim();
        var filtro = new BuscarManifiestosFiltro(
            Year: _year,
            Numero: string.IsNullOrWhiteSpace(buscar) ? null : buscar);

        try
        {
            var results = await _mediator.Send(new BuscarManifiestosQuery(filtro));
            dgvManifiestos.DataSource = results.ToList();
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"RefreshGrid failed: {ex.Message}");
        }
    }

    private async Task LoadManifiestoAsync(int codigo)
    {
        try
        {
            var dto = await _mediator.Send(new ObtenerManifiestoPorCodigoQuery(codigo, _year));
            if (dto is null) return;

            _savedCodigo         = dto.Codigo;
            _fecha               = dto.Fecha;
            _transportistaCodigo = dto.TransportistaCodigo;
            _vehiculoCodigo      = dto.VehiculoCodigo;
            _carretaCodigo       = dto.CarretaCodigo;
            _choferCodigo        = dto.ChoferCodigo;
            _importe             = dto.Importe;
            _peso                = dto.Peso;
            _estadoCodigo        = dto.EstadoCodigo;
            _guiaIds             = dto.Lineas.Select(l => l.GuiaId).ToList();

            // Update UI fields
            dtpFecha.Value   = dto.Fecha;
            txtImporte.Text  = dto.Importe.ToString("F2");
            txtPeso.Text     = dto.Peso.ToString("F2");
            txtGuiaIds.Text  = string.Join(",", _guiaIds);
            lblNumero.Text   = $"Manifiesto #{dto.Codigo} — {dto.Numero}";

            SetComboValue(cmbTransportista, dto.TransportistaCodigo);
            SetComboValue(cmbVehiculo, dto.VehiculoCodigo);
            SetComboValue(cmbCarreta, dto.CarretaCodigo ?? 0);
            SetComboValue(cmbChofer, dto.ChoferCodigo);
            SetComboValue(cmbEstado, dto.EstadoCodigo);

            btnEliminar.Enabled = true;
            btnImprimir.Enabled = true;
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"LoadManifiesto failed: {ex.Message}");
        }
    }

    private static void SetComboValue(ComboBox combo, int value)
    {
        combo.SelectedValue = value;
    }

    private void ClearForm()
    {
        _savedCodigo         = null;
        _transportistaCodigo = 0;
        _vehiculoCodigo      = 0;
        _carretaCodigo       = null;
        _choferCodigo        = 0;
        _estadoCodigo        = 0;
        _importe             = 0m;
        _peso                = 0m;
        _fecha               = DateTime.Today;
        _guiaIds             = [];

        dtpFecha.Value  = DateTime.Today;
        txtImporte.Text = "0.00";
        txtPeso.Text    = "0.00";
        txtGuiaIds.Text = string.Empty;
        lblNumero.Text  = string.Empty;

        if (cmbTransportista.Items.Count > 0) cmbTransportista.SelectedIndex = 0;
        if (cmbVehiculo.Items.Count > 0) cmbVehiculo.SelectedIndex = 0;
        if (cmbCarreta.Items.Count > 0) cmbCarreta.SelectedIndex = 0;
        if (cmbChofer.Items.Count > 0) cmbChofer.SelectedIndex = 0;
        if (cmbEstado.Items.Count > 0) cmbEstado.SelectedIndex = 0;

        btnEliminar.Enabled = false;
        btnImprimir.Enabled = false;
    }

    private void SyncFieldsFromControls()
    {
        if (cmbTransportista.SelectedValue is int t && t > 0) _transportistaCodigo = t;
        if (cmbVehiculo.SelectedValue is int v && v > 0) _vehiculoCodigo = v;
        _carretaCodigo = cmbCarreta.SelectedValue is int c && c > 0 ? c : null;
        if (cmbChofer.SelectedValue is int ch && ch > 0) _choferCodigo = ch;
        if (cmbEstado.SelectedValue is int e && e > 0) _estadoCodigo = e;

        if (decimal.TryParse(txtImporte.Text, out var imp)) _importe = imp;
        if (decimal.TryParse(txtPeso.Text, out var peso)) _peso = peso;

        _fecha = dtpFecha.Value;

        var rawIds = txtGuiaIds.Text.Trim();
        if (!string.IsNullOrWhiteSpace(rawIds))
        {
            _guiaIds = rawIds
                .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                .Select(s => int.TryParse(s, out var id) ? id : 0)
                .Where(id => id > 0)
                .ToList();
        }
    }
}
