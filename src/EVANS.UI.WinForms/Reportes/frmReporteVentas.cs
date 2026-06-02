using EVANS.Application.Reportes.DTOs;
using EVANS.Application.Reportes.Ports;
using EVANS.Application.Reportes.Queries;
using MediatR;

namespace EVANS.UI.WinForms.Reportes;

public partial class frmReporteVentas : Form
{
    private readonly IMediator _mediator;
    private readonly IReportesConsultaRepository _repository;
    private readonly IReporteVentasExcelExporter _excelExporter;
    private readonly int _year;
    private IReadOnlyList<VentaReporteDto> _rows = Array.Empty<VentaReporteDto>();

    public frmReporteVentas(
        IMediator mediator,
        IReportesConsultaRepository repository,
        IReporteVentasExcelExporter excelExporter,
        int year)
    {
        _mediator = mediator;
        _repository = repository;
        _excelExporter = excelExporter;
        _year = year;

        InitializeComponent();
        dtpFechaDesde.Value = new DateTime(year, 1, 1);
        dtpFechaHasta.Value = new DateTime(year, 12, 31);
        rbTodos.Checked = true;
        chkFacturas.Checked = true;
        chkBoletas.Checked = true;
        Load += async (_, _) => await CargarClientesAsync();
    }

    internal async Task CargarClientesAsync()
    {
        var clientes = await _repository.ListarClientesAsync(CancellationToken.None);
        cbCliente.DataSource = clientes.ToList();
        cbCliente.DisplayMember = nameof(ClienteReporteDto.Nombre);
        cbCliente.ValueMember = nameof(ClienteReporteDto.Codigo);
        cbCliente.Enabled = rbCliente.Checked;
    }

    internal async Task<bool> BuscarAsync()
    {
        lblError.Text = string.Empty;
        dgvDetalles.Rows.Clear();
        btnExportar.Enabled = false;

        var clienteCodigo = rbCliente.Checked && cbCliente.SelectedValue is int codigo ? codigo : (int?)null;
        if (rbCliente.Checked && clienteCodigo is null)
        {
            lblError.Text = "Seleccione un cliente.";
            _rows = Array.Empty<VentaReporteDto>();
            return false;
        }

        var filtro = new ReporteVentasFiltro(
            _year,
            dtpFechaDesde.Value.Date,
            dtpFechaHasta.Value.Date,
            chkFacturas.Checked,
            chkBoletas.Checked,
            clienteCodigo);

        _rows = await _mediator.Send(new ConsultarReporteVentasQuery(filtro));
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

    internal void SetTestCliente(int codigo)
    {
        rbCliente.Checked = true;
        cbCliente.Enabled = true;
        cbCliente.SelectedValue = codigo;
    }

    internal void SetTestTipos(bool facturas, bool boletas)
    {
        chkFacturas.Checked = facturas;
        chkBoletas.Checked = boletas;
    }

    internal int ResultCount => dgvDetalles.Rows.Cast<DataGridViewRow>().Count(r => !r.IsNewRow);
    internal bool ExportEnabled => btnExportar.Enabled;
    internal string ErrorMessage => lblError.Text;
    internal string? FirstCliente => ResultCount == 0 ? null : dgvDetalles.Rows[0].Cells[colCliente.Name].Value?.ToString();

    private void BindResultados(IReadOnlyList<VentaReporteDto> rows)
    {
        dgvDetalles.Rows.Clear();
        foreach (var row in rows)
        {
            dgvDetalles.Rows.Add(
                row.Fecha.ToString("dd/MM/yyyy"),
                row.TipoCodigo,
                row.Serie,
                row.Numero,
                row.ClienteNumeroIdentificacion,
                row.ClienteNombre,
                row.ValorVenta.ToString("0.00"),
                row.Igv.ToString("0.00"),
                row.Total.ToString("0.00"));
        }

        if (rows.Count == 0)
            return;

        dgvDetalles.Rows.Add(
            string.Empty,
            string.Empty,
            string.Empty,
            string.Empty,
            string.Empty,
            "TOTAL",
            rows.Sum(row => row.ValorVenta).ToString("0.00"),
            rows.Sum(row => row.Igv).ToString("0.00"),
            rows.Sum(row => row.Total).ToString("0.00"));
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
            FileName = $"reporte-ventas-{_year}.xlsx"
        };

        if (dialog.ShowDialog(this) != DialogResult.OK)
            return;

        File.WriteAllBytes(dialog.FileName, bytes);
        MessageBox.Show("Registros exportados correctamente.", "Reportes", MessageBoxButtons.OK, MessageBoxIcon.Information);
    }

    private void rbTodos_CheckedChanged(object? sender, EventArgs e)
    {
        if (rbTodos.Checked)
            cbCliente.Enabled = false;
    }

    private void rbCliente_CheckedChanged(object? sender, EventArgs e) => cbCliente.Enabled = rbCliente.Checked;
}
