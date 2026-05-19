using EVANS.Application.Common;
using EVANS.Application.GuiaRemision.Commands;
using EVANS.Application.GuiaRemision.DTOs;
using EVANS.Application.GuiaRemision.Queries;
using EVANS.Domain.GuiaRemision;
using MediatR;

namespace EVANS.UI.WinForms.GuiaRemision;

public partial class frmGuiaRemision : Form
{
    private readonly IMediator _mediator;
    private readonly OrigenGuia _origen;
    private readonly int _year;
    private readonly int? _codigoGuia;

    // Set by LoadAsync for use in GrabarAsync
    private decimal _igvRate;

    // Test-settable fields (populated from combos in real use; set directly in tests)
    internal int _remitenteId;
    internal int _destinatarioId;
    internal string _dirPartida = string.Empty;
    internal string _dirLlegada = string.Empty;

    public frmGuiaRemision(IMediator mediator, OrigenGuia origen, int year, int? codigoGuia = null)
    {
        InitializeComponent();
        _mediator = mediator;
        _origen = origen;
        _year = year;
        _codigoGuia = codigoGuia;

        // Wire combo selection changes to update internal fields
        cmbRemitente.SelectedIndexChanged += (_, _) =>
        {
            if (cmbRemitente.SelectedValue is int id) _remitenteId = id;
        };
        cmbDestinatario.SelectedIndexChanged += (_, _) =>
        {
            if (cmbDestinatario.SelectedValue is int id) _destinatarioId = id;
        };
    }

    private async void frmGuiaRemision_Load(object? sender, EventArgs e)
    {
        await LoadAsync();
    }

    internal async Task LoadAsync()
    {
        var catalogos = await _mediator.Send(new ObtenerCatalogosGuiaQuery());
        _igvRate = catalogos.IgvRate;
        BindCombos(catalogos);

        if (_codigoGuia.HasValue)
        {
            var guia = await _mediator.Send(new ObtenerGuiaPorCodigoQuery(_codigoGuia.Value, _year));
            if (guia != null)
                PopulateFromGuia(guia);
        }
    }

    internal async Task<bool> GrabarAsync()
    {
        var detalles = ReadDetallesFromGrid();

        if (_codigoGuia.HasValue)
        {
            var cmd = new ActualizarGuiaCommand(
                Codigo: _codigoGuia.Value,
                RemitenteId: _remitenteId,
                DestinatarioId: _destinatarioId,
                DireccionPartida: _dirPartida,
                DireccionLlegada: _dirLlegada,
                HasManifest: chkHasManifest.Checked,
                VehiculoId: cmbVehiculo.SelectedValue as int?,
                CarretaId: cmbCarreta.SelectedValue as int?,
                ChoferId: cmbChofer.SelectedValue as int?,
                Igv: _igvRate,
                Year: _year,
                Detalles: detalles);
            var result = await _mediator.Send(cmd);
            if (!result.IsSuccess)
            {
                MessageBox.Show(result.Error, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }
        else
        {
            var cmd = new CrearGuiaCommand(
                RemitenteId: _remitenteId,
                DestinatarioId: _destinatarioId,
                DireccionPartida: _dirPartida,
                DireccionLlegada: _dirLlegada,
                HasManifest: chkHasManifest.Checked,
                VehiculoId: cmbVehiculo.SelectedValue as int?,
                CarretaId: cmbCarreta.SelectedValue as int?,
                ChoferId: cmbChofer.SelectedValue as int?,
                Igv: _igvRate,
                Origen: _origen,
                Year: _year,
                Detalles: detalles);
            var result = await _mediator.Send(cmd);
            if (!result.IsSuccess)
            {
                MessageBox.Show(result.Error, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        return true;
    }

    private async void btnGrabar_Click(object? sender, EventArgs e)
    {
        btnGrabar.Enabled = false;
        try
        {
            if (await GrabarAsync())
                Close();
        }
        finally
        {
            btnGrabar.Enabled = true;
        }
    }

    private void btnCancelar_Click(object? sender, EventArgs e)
    {
        Close();
    }

    // Called by tests to set fields without UI interaction
    internal void SetTestFields(int remitenteId, int destinatarioId, string dirPartida, string dirLlegada)
    {
        _remitenteId = remitenteId;
        _destinatarioId = destinatarioId;
        _dirPartida = dirPartida;
        _dirLlegada = dirLlegada;
    }

    private void BindCombos(CatalogosGuiaDto catalogos)
    {
        BindCombo(cmbRemitente, catalogos.Clientes);
        BindCombo(cmbDestinatario, catalogos.Clientes);
        BindCombo(cmbVehiculo, catalogos.Vehiculos);
        BindCombo(cmbCarreta, catalogos.Carretas);
        BindCombo(cmbChofer, catalogos.Choferes);
        lblIgv.Text = $"IGV: {catalogos.IgvRate:P0}";
    }

    private static void BindCombo(ComboBox combo, IReadOnlyList<CatalogoItemDto> items)
    {
        combo.DataSource = items.ToList();
        combo.DisplayMember = nameof(CatalogoItemDto.Nombre);
        combo.ValueMember = nameof(CatalogoItemDto.Id);
    }

    private void PopulateFromGuia(GuiaDetalleDto guia)
    {
        _remitenteId = guia.RemitenteId;
        _destinatarioId = guia.DestinatarioId;
        _dirPartida = guia.DireccionPartida;
        _dirLlegada = guia.DireccionLlegada;
        chkHasManifest.Checked = guia.HasManifest;

        SetComboValue(cmbVehiculo, guia.VehiculoId);
        SetComboValue(cmbCarreta, guia.CarretaId);
        SetComboValue(cmbChofer, guia.ChoferId);

        dgvDetalles.Rows.Clear();
        foreach (var d in guia.Detalles)
            dgvDetalles.Rows.Add(d.Descripcion, d.PesoValor, d.PrecioUnitario, d.PrecioTotal, d.Cantidad);
    }

    private static void SetComboValue(ComboBox combo, int? value)
    {
        if (value.HasValue)
            combo.SelectedValue = value.Value;
    }

    private IReadOnlyList<DetalleGuiaInput> ReadDetallesFromGrid()
    {
        var result = new List<DetalleGuiaInput>();
        foreach (DataGridViewRow row in dgvDetalles.Rows)
        {
            if (row.IsNewRow) continue;
            var descripcion = row.Cells["colDescripcion"].Value?.ToString() ?? string.Empty;
            var peso = Convert.ToDecimal(row.Cells["colPeso"].Value ?? 0);
            var precioUnitario = Convert.ToDecimal(row.Cells["colPrecioUnitario"].Value ?? 0);
            var precioTotal = Convert.ToDecimal(row.Cells["colPrecioTotal"].Value ?? 0);
            var cantidad = Convert.ToInt32(row.Cells["colCantidad"].Value ?? 0);
            result.Add(new DetalleGuiaInput(descripcion, new Peso(peso), precioUnitario, precioTotal, cantidad));
        }
        return result;
    }
}
