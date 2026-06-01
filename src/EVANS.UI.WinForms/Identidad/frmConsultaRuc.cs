using EVANS.Application.Identidad.Queries;
using MediatR;

namespace EVANS.UI.WinForms.Identidad;

public partial class frmConsultaRuc : Form
{
    private readonly IMediator _mediator;

    public frmConsultaRuc(IMediator mediator)
    {
        _mediator = mediator;
        InitializeComponent();
    }

    internal async Task<bool> ConsultarAsync()
    {
        lblError.Text = string.Empty;
        lblRazonSocial.Text = string.Empty;
        lblEstado.Text = string.Empty;
        lblCondicion.Text = string.Empty;
        lblDireccion.Text = string.Empty;

        var result = await _mediator.Send(new ConsultarRucQuery(txtRuc.Text));
        if (!result.IsSuccess)
        {
            lblError.Text = result.Error ?? "No se pudo consultar el RUC.";
            return false;
        }

        var data = result.Value!;
        lblRazonSocial.Text = data.RazonSocial;
        lblEstado.Text = data.Estado;
        lblCondicion.Text = data.Condicion;
        lblDireccion.Text = data.Direccion;
        return true;
    }

    internal void SetTestRuc(string ruc) => txtRuc.Text = ruc;
    internal string ErrorMessage => lblError.Text;
    internal string RazonSocial => lblRazonSocial.Text;

    private async void btnConsultar_Click(object? sender, EventArgs e)
    {
        btnConsultar.Enabled = false;
        try
        {
            await ConsultarAsync();
        }
        finally
        {
            btnConsultar.Enabled = true;
        }
    }

    private void btnCerrar_Click(object? sender, EventArgs e) => Close();
}
