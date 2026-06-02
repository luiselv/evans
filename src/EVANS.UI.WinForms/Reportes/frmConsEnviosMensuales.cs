using EVANS.Application.Reportes.DTOs;
using EVANS.Application.Reportes.Ports;
using EVANS.Application.Reportes.Queries;
using MediatR;

namespace EVANS.UI.WinForms.Reportes;

public partial class frmConsEnviosMensuales : Form
{
    private readonly IMediator _mediator;
    private readonly IReportesConsultaRepository _repository;
    private readonly IEnviosMensualesExcelExporter _excelExporter;
    private readonly int _year;
    private IReadOnlyList<EnvioMensualDto> _rows = Array.Empty<EnvioMensualDto>();

    public frmConsEnviosMensuales(
        IMediator mediator,
        IReportesConsultaRepository repository,
        IEnviosMensualesExcelExporter excelExporter,
        int year)
    {
        _mediator = mediator;
        _repository = repository;
        _excelExporter = excelExporter;
        _year = year;

        InitializeComponent();
        dtpFechaDesde.Value = new DateTime(year, 1, 1);
        dtpFechaHasta.Value = new DateTime(year, 12, 31);
        Load += async (_, _) => await CargarDestinosAsync();
    }

    internal async Task CargarDestinosAsync()
    {
        lblError.Text = string.Empty;
        dgvDestinos.Rows.Clear();

        var destinos = await _repository.ListarDestinosActivosAsync(CancellationToken.None);
        foreach (var destino in destinos)
            dgvDestinos.Rows.Add(false, destino.Codigo, destino.Nombre);
    }

    internal async Task<bool> BuscarAsync()
    {
        lblError.Text = string.Empty;
        dgvDetalles.Rows.Clear();
        btnExportar.Enabled = false;

        var destinoCodigos = SelectedDestinoCodigos();
        if (destinoCodigos.Count == 0)
        {
            lblError.Text = "Seleccione al menos un destino.";
            _rows = Array.Empty<EnvioMensualDto>();
            return false;
        }

        var filtro = new EnviosMensualesFiltro(
            _year,
            dtpFechaDesde.Value.Date,
            dtpFechaHasta.Value.Date,
            destinoCodigos);

        _rows = await _mediator.Send(new ConsultarEnviosMensualesQuery(filtro));
        BindResultados(_rows);
        btnExportar.Enabled = _rows.Count > 0;
        return true;
    }

    internal byte[] ExportCurrentRows() =>
        _rows.Count == 0 ? Array.Empty<byte>() : _excelExporter.Export(_rows);

    internal void SetTestDateRange(DateTime desde, DateTime hasta)
    {
        dtpFechaDesde.Value = desde;
        dtpFechaHasta.Value = hasta;
    }

    internal void SelectTestDestinos(params int[] codigos)
    {
        var selected = codigos.ToHashSet();
        foreach (DataGridViewRow row in dgvDestinos.Rows)
        {
            if (row.IsNewRow || row.Cells[colDestinoCodigo.Name].Value is not int codigo)
                continue;

            row.Cells[colDestinoSeleccionado.Name].Value = selected.Contains(codigo);
        }
    }

    internal int DestinoCount => dgvDestinos.Rows.Cast<DataGridViewRow>().Count(r => !r.IsNewRow);
    internal int ResultCount => dgvDetalles.Rows.Cast<DataGridViewRow>().Count(r => !r.IsNewRow);
    internal bool ExportEnabled => btnExportar.Enabled;
    internal string ErrorMessage => lblError.Text;
    internal string? FirstCliente => ResultCount == 0 ? null : dgvDetalles.Rows[0].Cells[colCliente.Name].Value?.ToString();

    private IReadOnlyList<int> SelectedDestinoCodigos()
    {
        var codigos = new List<int>();
        foreach (DataGridViewRow row in dgvDestinos.Rows)
        {
            if (row.IsNewRow)
                continue;

            var selected = row.Cells[colDestinoSeleccionado.Name].Value is true;
            if (selected && row.Cells[colDestinoCodigo.Name].Value is int codigo)
                codigos.Add(codigo);
        }

        return codigos;
    }

    private void BindResultados(IReadOnlyList<EnvioMensualDto> rows)
    {
        dgvDetalles.Rows.Clear();
        foreach (var row in rows)
            dgvDetalles.Rows.Add(row.Cliente, row.NroGuias, row.UltimoEnvio.ToString("dd/MM/yyyy"));
    }

    private async void btnBuscar_Click(object? sender, EventArgs e)
    {
        btnBuscar.Enabled = false;
        try
        {
            await BuscarAsync();
        }
        finally
        {
            btnBuscar.Enabled = true;
        }
    }

    private void btnExportar_Click(object? sender, EventArgs e)
    {
        var bytes = ExportCurrentRows();
        if (bytes.Length == 0)
        {
            MessageBox.Show("No hay registros para exportar.", "Reportes", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        using var dialog = new SaveFileDialog
        {
            Filter = "Excel (*.xlsx)|*.xlsx",
            FileName = $"envios-mensuales-{_year}.xlsx"
        };

        if (dialog.ShowDialog(this) != DialogResult.OK)
            return;

        File.WriteAllBytes(dialog.FileName, bytes);
        MessageBox.Show("Registros exportados correctamente.", "Reportes", MessageBoxButtons.OK, MessageBoxIcon.Information);
    }
}
