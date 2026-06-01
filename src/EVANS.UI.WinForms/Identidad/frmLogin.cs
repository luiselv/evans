using EVANS.Application.Identidad.Commands;
using EVANS.Application.Identidad.DTOs;
using MediatR;

namespace EVANS.UI.WinForms.Identidad;

public partial class frmLogin : Form
{
    private readonly IMediator _mediator;

    public frmLogin(IMediator mediator)
    {
        _mediator = mediator;
        InitializeComponent();
    }

    public UsuarioSesionDto? AuthenticatedUser { get; private set; }

    internal async Task<bool> SubmitAsync()
    {
        lblError.Text = string.Empty;

        var result = await _mediator.Send(new AutenticarUsuarioCommand(
            txtUsuario.Text,
            txtClave.Text));

        if (!result.IsSuccess)
        {
            lblError.Text = result.Error ?? "No se pudo iniciar sesión.";
            return false;
        }

        AuthenticatedUser = result.Value;
        return true;
    }

    internal void SetTestCredentials(string username, string password)
    {
        txtUsuario.Text = username;
        txtClave.Text = password;
    }

    internal string ErrorMessage => lblError.Text;

    private async void btnIngresar_Click(object? sender, EventArgs e)
    {
        btnIngresar.Enabled = false;
        try
        {
            if (await SubmitAsync())
            {
                DialogResult = DialogResult.OK;
                Close();
            }
        }
        finally
        {
            btnIngresar.Enabled = true;
        }
    }

    private void btnSalir_Click(object? sender, EventArgs e)
    {
        DialogResult = DialogResult.Cancel;
        Close();
    }
}
