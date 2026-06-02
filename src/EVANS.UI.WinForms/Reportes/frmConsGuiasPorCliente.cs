using EVANS.Application.Reportes.DTOs;
using EVANS.Application.Reportes.Ports;
using EVANS.Application.Reportes.Queries;
using MediatR;

namespace EVANS.UI.WinForms.Reportes;

public partial class frmConsGuiasPorCliente : Form
{
    private readonly IMediator _mediator;
    private readonly IReportesConsultaRepository _repository;
    private readonly int _year;
    private IReadOnlyList<ClienteReporteDto> _clientes = Array.Empty<ClienteReporteDto>();

    public frmConsGuiasPorCliente(
        IMediator mediator,
        IReportesConsultaRepository repository,
        int year)
    {
        _mediator = mediator;
        _repository = repository;
        _year = year;

        InitializeComponent();
        dtpFechaDesde.Value = new DateTime(year, 1, 1);
        dtpFechaHasta.Value = new DateTime(year, 12, 31);
        Load += async (_, _) => await CargarClientesAsync();
    }

    internal async Task CargarClientesAsync()
    {
        lblError.Text = string.Empty;
        _clientes = await _repository.ListarClientesAsync(CancellationToken.None);

        cbCliente.DataSource = _clientes.ToList();
        cbCliente.DisplayMember = nameof(ClienteReporteDto.Nombre);
        cbCliente.ValueMember = nameof(ClienteReporteDto.Codigo);
    }

    internal async Task<bool> BuscarAsync()
    {
        lblError.Text = string.Empty;
        dgvListado.Rows.Clear();

        if (cbCliente.SelectedValue is not int clienteCodigo)
        {
            lblError.Text = "Seleccione un cliente.";
            return false;
        }

        var filtro = new GuiasPorClienteFiltro(
            _year,
            clienteCodigo,
            dtpFechaDesde.Value.Date,
            dtpFechaHasta.Value.Date,
            chkPendientes.Checked);

        var rows = await _mediator.Send(new ConsultarGuiasPorClienteQuery(filtro));
        BindResultados(rows);
        return true;
    }

    internal void SetTestDateRange(DateTime desde, DateTime hasta)
    {
        dtpFechaDesde.Value = desde;
        dtpFechaHasta.Value = hasta;
    }

    internal void SetTestCliente(int codigo)
    {
        cbCliente.SelectedValue = codigo;
        UpdateNumeroIdentificacion();
    }

    internal void SetTestPendientes(bool value) => chkPendientes.Checked = value;

    internal int ClienteCount => cbCliente.Items.Count;
    internal int ResultCount => dgvListado.Rows.Cast<DataGridViewRow>().Count(r => !r.IsNewRow);
    internal string ErrorMessage => lblError.Text;
    internal string NumeroIdentificacion => txtNroId.Text;
    internal string? FirstNroDoc => ResultCount == 0 ? null : dgvListado.Rows[0].Cells[colNroDoc.Name].Value?.ToString();

    private void BindResultados(IReadOnlyList<GuiaPorClienteDto> rows)
    {
        dgvListado.Rows.Clear();
        foreach (var row in rows)
        {
            dgvListado.Rows.Add(
                row.Codigo,
                row.NroDoc,
                row.Remitente,
                row.Destinatario,
                row.FechaEmision.ToString("dd/MM/yyyy"),
                row.FechaTraslado.ToString("dd/MM/yyyy"),
                row.Bultos,
                row.CostoTotal.ToString("0.00"),
                row.Enviado ? "Sí" : "No");
        }
    }

    private void UpdateNumeroIdentificacion()
    {
        if (cbCliente.SelectedValue is not int codigo)
        {
            txtNroId.Text = string.Empty;
            return;
        }

        txtNroId.Text = _clientes.FirstOrDefault(cliente => cliente.Codigo == codigo)?.NumeroIdentificacion ?? string.Empty;
    }

    private void SelectClienteByNumeroIdentificacion()
    {
        var cliente = _clientes.FirstOrDefault(c => c.NumeroIdentificacion == txtNroId.Text.Trim());
        if (cliente is not null)
            cbCliente.SelectedValue = cliente.Codigo;
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

    private void cbCliente_SelectedIndexChanged(object? sender, EventArgs e) => UpdateNumeroIdentificacion();

    private void txtNroId_KeyPress(object? sender, KeyPressEventArgs e)
    {
        if (e.KeyChar != (char)Keys.Enter)
            return;

        e.Handled = true;
        SelectClienteByNumeroIdentificacion();
    }
}
