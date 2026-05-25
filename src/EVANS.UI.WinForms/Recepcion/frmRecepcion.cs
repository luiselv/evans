using EVANS.Application.GuiaRemision.Commands;
using EVANS.Application.Recepcion.Commands;
using EVANS.Application.Recepcion.DTOs;
using EVANS.Application.Recepcion.Ports;
using EVANS.Application.Recepcion.Queries;
using EVANS.Domain.GuiaRemision;
using EVANS.Domain.Recepcion;
using MediatR;

namespace EVANS.UI.WinForms.Recepcion;

/// <summary>
/// New C# WinForms form for Recepcion de Carga.
/// Injected with IMediator — no direct SQL, no globals.
/// btnGenerarGuia dispatches CrearGuiaCommand with OrigenGuia.DesdeRecepcion(id).
/// </summary>
public partial class frmRecepcion : Form
{
    private readonly IMediator _mediator;
    private readonly ICatalogosRecepcionRepository _catalogos;
    private readonly int _year;

    // Currently loaded recepcion (null = new mode)
    private RecepcionDetalleDto? _recepcionActual;

    // Test-settable fields
    internal int _remitenteId;
    internal int _destinatarioId;
    internal int _destinoId;
    internal int _estadoId;
    internal int _usuarioId = 1;

    public frmRecepcion(IMediator mediator, ICatalogosRecepcionRepository catalogos, int year)
    {
        InitializeComponent();
        _mediator = mediator;
        _catalogos = catalogos;
        _year = year;

        cmbRemitente.SelectedIndexChanged += (_, _) =>
        {
            if (cmbRemitente.SelectedValue is int id) _remitenteId = id;
        };
        cmbDestinatario.SelectedIndexChanged += (_, _) =>
        {
            if (cmbDestinatario.SelectedValue is int id) _destinatarioId = id;
        };
        cmbDestino.SelectedIndexChanged += (_, _) =>
        {
            if (cmbDestino.SelectedValue is int id) _destinoId = id;
        };
        cmbEstado.SelectedIndexChanged += (_, _) =>
        {
            if (cmbEstado.SelectedValue is int id) _estadoId = id;
        };

        dgvResultados.CellDoubleClick += dgvResultados_CellDoubleClick;

        this.Load += async (_, _) => await LoadCatalogosAsync();
    }

    // ----------------------------------------------------------------
    // Button handlers
    // ----------------------------------------------------------------

    private async void btnGrabar_Click(object? sender, EventArgs e)
    {
        btnGrabar.Enabled = false;
        try { await GrabarAsync(); }
        finally { btnGrabar.Enabled = true; }
    }

    private async void btnEliminar_Click(object? sender, EventArgs e)
    {
        if (_recepcionActual is null) return;
        if (MessageBox.Show("¿Eliminar esta recepcion?", "Confirmar",
            MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;

        btnEliminar.Enabled = false;
        try { await EliminarAsync(_recepcionActual.Codigo); }
        finally { btnEliminar.Enabled = true; }
    }

    private void btnNuevo_Click(object? sender, EventArgs e) => LimpiarFormulario();

    private async void btnBuscar_Click(object? sender, EventArgs e)
    {
        btnBuscar.Enabled = false;
        try { await BuscarAsync(); }
        finally { btnBuscar.Enabled = true; }
    }

    private async void btnGenerarGuia_Click(object? sender, EventArgs e)
    {
        if (_recepcionActual is null || _recepcionActual.Codigo <= 0) return;

        // UI creates origen and dispatches to GuiaRemision context
        var origen = new DesdeRecepcion(_recepcionActual.Codigo);
        btnGenerarGuia.Enabled = false;

        try
        {
            // Dispatch through MediatR — CrearGuiaCommandHandler handles cross-context write-back
            // via IRecepcionVinculadaService post-commit (ADR-1).
            await _mediator.Send(new CrearGuiaCommand(
                RemitenteId: _recepcionActual.RemitenteId,
                DestinatarioId: _recepcionActual.DestinatarioId,
                DireccionPartida: string.Empty,
                DireccionLlegada: string.Empty,
                HasManifest: false,
                VehiculoId: null,
                CarretaId: null,
                ChoferId: null,
                Igv: 0.18m,
                Origen: origen,
                Year: _year,
                Detalles: Array.Empty<DetalleGuiaInput>()));

            // Refresh to show the linked guia number
            await RefrescarRecepcionActualAsync();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error al generar guia: {ex.Message}", "Error",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            btnGenerarGuia.Enabled = _recepcionActual?.Codigo > 0;
        }
    }

    private void btnCancelar_Click(object? sender, EventArgs e) => this.Close();

    // ----------------------------------------------------------------
    // Core operations
    // ----------------------------------------------------------------

    internal async Task GrabarAsync()
    {
        var detalles = ReadDetallesFromGrid();

        try
        {
            if (_recepcionActual is null)
            {
                // Create
                var cmd = new CrearRecepcionCommand(
                    FechaEmision: dtpFechaEmision.Value.Date,
                    RemitenteId: _remitenteId,
                    TipoDirPartida: TipoDireccion.Agencia,
                    DireccionPartida: txtDirPartida.Text,
                    DestinatarioId: _destinatarioId,
                    TipoDirDestino: TipoDireccion.DireccionCliente,
                    DireccionDestino: txtDirDestino.Text,
                    DestinoId: _destinoId,
                    EstadoId: _estadoId,
                    Bultos: detalles.Count,
                    PesoTotal: detalles.Sum(d => d.Peso),
                    CostoTotal: detalles.Sum(d => d.Costo),
                    Observacion: txtObservacion.Text,
                    UsuarioId: _usuarioId,
                    AplicarIgv: false,
                    TasaIgv: 0m,
                    Year: _year,
                    Detalles: detalles);

                var codigo = await _mediator.Send(cmd);
                _recepcionActual = await _mediator.Send(
                    new ObtenerRecepcionPorCodigoQuery(codigo, _year));
                btnGenerarGuia.Enabled = true;
                MessageBox.Show($"Recepcion grabada. Codigo: {codigo}", "Exito",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                // Update
                var cmd = new ActualizarRecepcionCommand(
                    Codigo: _recepcionActual.Codigo,
                    FechaEmision: dtpFechaEmision.Value.Date,
                    RemitenteId: _remitenteId,
                    TipoDirPartida: TipoDireccion.Agencia,
                    DireccionPartida: txtDirPartida.Text,
                    DestinatarioId: _destinatarioId,
                    TipoDirDestino: TipoDireccion.DireccionCliente,
                    DireccionDestino: txtDirDestino.Text,
                    DestinoId: _destinoId,
                    EstadoId: _estadoId,
                    Bultos: detalles.Count,
                    PesoTotal: detalles.Sum(d => d.Peso),
                    CostoTotal: detalles.Sum(d => d.Costo),
                    Observacion: txtObservacion.Text,
                    AplicarIgv: false,
                    TasaIgv: 0m,
                    Year: _year,
                    Detalles: detalles);

                await _mediator.Send(cmd);
                MessageBox.Show("Recepcion actualizada.", "Exito",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error al grabar: {ex.Message}", "Error",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private async Task EliminarAsync(int codigo)
    {
        try
        {
            await _mediator.Send(new EliminarRecepcionCommand(codigo, _year));
            LimpiarFormulario();
            MessageBox.Show("Recepcion eliminada.", "Exito",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error al eliminar: {ex.Message}", "Error",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private async Task BuscarAsync()
    {
        var rango = DateRange.Intervalo(dtpDesde.Value.Date, dtpHasta.Value.Date);
        var resultados = await _mediator.Send(new BuscarRecepcionesQuery(rango, _year));
        BindResultados(resultados);
    }

    private async Task RefrescarRecepcionActualAsync()
    {
        if (_recepcionActual is null) return;
        _recepcionActual = await _mediator.Send(
            new ObtenerRecepcionPorCodigoQuery(_recepcionActual.Codigo, _year));
        if (_recepcionActual?.GuiaRemisionVinculada is not null)
            lblGuiaVinculada.Text = $"Guia vinculada: {_recepcionActual.GuiaRemisionVinculada}";
    }

    // ----------------------------------------------------------------
    // UI helpers
    // ----------------------------------------------------------------

    private async Task LoadCatalogosAsync()
    {
        var clientes = await _catalogos.ListarClientesAsync(CancellationToken.None);
        var destinos = await _catalogos.ListarDestinosAsync(CancellationToken.None);
        var estados = await _catalogos.ListarEstadosAsync(CancellationToken.None);

        cmbRemitente.DataSource = clientes.Select(c => new { c.Id, c.Nombre }).ToList();
        cmbRemitente.DisplayMember = "Nombre";
        cmbRemitente.ValueMember = "Id";

        cmbDestinatario.DataSource = clientes.Select(c => new { c.Id, c.Nombre }).ToList();
        cmbDestinatario.DisplayMember = "Nombre";
        cmbDestinatario.ValueMember = "Id";

        cmbDestino.DataSource = destinos.Select(d => new { d.Id, d.Nombre }).ToList();
        cmbDestino.DisplayMember = "Nombre";
        cmbDestino.ValueMember = "Id";

        cmbEstado.DataSource = estados.Select(e => new { e.Id, e.Nombre }).ToList();
        cmbEstado.DisplayMember = "Nombre";
        cmbEstado.ValueMember = "Id";
    }

    private IReadOnlyList<DetalleRecepcionInput> ReadDetallesFromGrid()
    {
        var result = new List<DetalleRecepcionInput>();
        foreach (DataGridViewRow row in dgvDetalles.Rows)
        {
            if (row.IsNewRow) continue;
            if (!decimal.TryParse(row.Cells["colCantidad"].Value?.ToString(), out var cant) || cant <= 0) continue;
            var desc = row.Cells["colDescripcion"].Value?.ToString() ?? string.Empty;
            if (string.IsNullOrWhiteSpace(desc)) continue;
            decimal.TryParse(row.Cells["colPeso"].Value?.ToString(), out var peso);
            var unidad = row.Cells["colUnidad"].Value?.ToString() ?? string.Empty;
            decimal.TryParse(row.Cells["colCosto"].Value?.ToString(), out var costo);
            var tipoDoc = row.Cells["colTipoDoc"].Value?.ToString() ?? string.Empty;
            var nroDoc = row.Cells["colNroDoc"].Value?.ToString() ?? string.Empty;
            result.Add(new DetalleRecepcionInput(cant, desc, peso, unidad, costo, tipoDoc, nroDoc));
        }
        return result;
    }

    private void BindResultados(IReadOnlyList<RecepcionListItemDto> resultados)
    {
        dgvResultados.Rows.Clear();
        foreach (var r in resultados)
        {
            dgvResultados.Rows.Add(
                r.Codigo, r.Fecha.ToShortDateString(),
                r.RemitenteNombre, r.DestinatarioNombre,
                r.GuiaRemisionVinculada ?? string.Empty);
        }
    }

    private async void dgvResultados_CellDoubleClick(object? sender, DataGridViewCellEventArgs e)
    {
        if (e.RowIndex < 0) return;
        if (!int.TryParse(dgvResultados.Rows[e.RowIndex].Cells["colResCodig"].Value?.ToString(), out var codigo)) return;

        _recepcionActual = await _mediator.Send(new ObtenerRecepcionPorCodigoQuery(codigo, _year));
        if (_recepcionActual is null) return;
        PopulateFromDto(_recepcionActual);
    }

    private void PopulateFromDto(RecepcionDetalleDto dto)
    {
        dtpFechaEmision.Value = dto.Fecha;
        txtDirPartida.Text = string.Empty;
        txtDirDestino.Text = string.Empty;
        txtObservacion.Text = dto.Observacion;
        lblGuiaVinculada.Text = dto.GuiaRemisionVinculada is not null
            ? $"Guia vinculada: {dto.GuiaRemisionVinculada}"
            : string.Empty;

        dgvDetalles.Rows.Clear();
        foreach (var d in dto.Detalles)
        {
            dgvDetalles.Rows.Add(d.Cantidad, d.Descripcion, d.Peso, d.Unidad, d.Costo, d.TipoDoc, d.NroDoc);
        }

        btnGenerarGuia.Enabled = true;
    }

    private void LimpiarFormulario()
    {
        _recepcionActual = null;
        dtpFechaEmision.Value = DateTime.Today;
        txtDirPartida.Clear();
        txtDirDestino.Clear();
        txtObservacion.Clear();
        lblGuiaVinculada.Text = string.Empty;
        dgvDetalles.Rows.Clear();
        btnGenerarGuia.Enabled = false;
    }
}
