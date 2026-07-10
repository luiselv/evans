using EVANS.Application.Identidad.Commands;
using EVANS.Application.Identidad.DTOs;
using EVANS.Application.Identidad.Ports;
using MediatR;

namespace EVANS.UI.WinForms.Identidad;

public partial class frmLogin : Form
{
    private readonly IMediator? _mediator;
    private readonly IYearlyDatabaseCatalog? _databaseCatalog;
    private readonly IYearlyDatabaseProvisioner? _databaseProvisioner;

    public frmLogin() => InitializeComponent();

    public frmLogin(
        IMediator mediator,
        IYearlyDatabaseCatalog databaseCatalog,
        IYearlyDatabaseProvisioner databaseProvisioner)
    {
        _mediator = mediator;
        _databaseCatalog = databaseCatalog;
        _databaseProvisioner = databaseProvisioner;
        InitializeComponent();
    }

    public UsuarioSesionDto? AuthenticatedUser { get; private set; }

    public int SelectedYear
    {
        get
        {
            var selected = cbBD.SelectedItem?.ToString();
            return int.TryParse(selected, out var year) ? year : DateTime.Today.Year;
        }
    }

    internal IReadOnlyList<int> AvailableYears => cbBD.Items.Cast<object>()
        .Select(item => int.TryParse(item.ToString(), out var year) ? year : (int?)null)
        .Where(year => year.HasValue)
        .Select(year => year!.Value)
        .ToList()
        .AsReadOnly();

    internal async Task<bool> SubmitAsync()
    {
        lblError.Text = string.Empty;

        if (cbBD.SelectedItem is null)
        {
            lblError.Text = "Seleccione una Base de Datos.";
            cbBD.Focus();
            return false;
        }

        if (string.IsNullOrWhiteSpace(txtUsuario.Text) || string.IsNullOrWhiteSpace(txtClave.Text))
        {
            lblError.Text = "Ingrese su nombre de ususario y contraseña.";
            txtUsuario.Focus();
            return false;
        }

        var result = await Mediator.Send(new AutenticarUsuarioCommand(
            txtUsuario.Text,
            txtClave.Text));

        if (!result.IsSuccess)
        {
            lblError.Text = "Error en el inicio de sesión.";
            txtUsuario.Focus();
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

    internal async Task<bool> ConnectForTestsAsync() => await ConnectAsync(showMessages: false);

    internal async Task<bool> CreateCurrentYearForTestsAsync() => await CreateCurrentYearAsync(showMessages: false);

    internal string ErrorMessage => lblError.Text;

    private async void btnConectar_Click(object? sender, EventArgs e)
    {
        await ConnectAsync(showMessages: true);
    }

    private async void btnCrear_Click(object? sender, EventArgs e)
    {
        var year = DateTime.Today.Year;
        if (MessageBox.Show(
                $"¿Confirma que desea crear una nueva Base de Datos para el año {year}?",
                "Confirmación",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question) != DialogResult.Yes)
            return;

        await CreateCurrentYearAsync(showMessages: true);
    }

    private async void cbAceptar_Click(object? sender, EventArgs e)
    {
        cbAceptar.Enabled = false;
        try
        {
            if (await SubmitAsync())
            {
                DialogResult = DialogResult.OK;
                Close();
            }
            else if (!string.IsNullOrWhiteSpace(lblError.Text))
            {
                MessageBox.Show(lblError.Text, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        finally
        {
            cbAceptar.Enabled = true;
        }
    }

    private void cbSalir_Click(object? sender, EventArgs e)
    {
        DialogResult = DialogResult.Cancel;
        Close();
    }

    private void txtUsuario_KeyPress(object? sender, KeyPressEventArgs e)
    {
        if (e.KeyChar == (char)Keys.Enter)
        {
            e.Handled = true;
            SelectNextControl(txtUsuario, forward: true, tabStopOnly: true, nested: true, wrap: true);
        }
    }

    private void txtClave_KeyPress(object? sender, KeyPressEventArgs e)
    {
        if (e.KeyChar == (char)Keys.Enter)
        {
            e.Handled = true;
            SelectNextControl(txtClave, forward: true, tabStopOnly: true, nested: true, wrap: true);
        }
    }

    private void txtServidor_KeyPress(object? sender, KeyPressEventArgs e)
    {
        if (e.KeyChar == (char)Keys.Enter)
        {
            e.Handled = true;
            btnConectar.PerformClick();
        }
    }

    private async Task<bool> ConnectAsync(bool showMessages)
    {
        lblError.Text = string.Empty;
        SetLoginControlsEnabled(false);
        btnCrear.Enabled = false;
        cbBD.Enabled = false;

        try
        {
            var years = await DatabaseCatalog.ListYearsAsync();
            BindYears(years);

            cbBD.Enabled = true;
            btnCrear.Enabled = true;
            SetLoginControlsEnabled(true);
            txtUsuario.Focus();

            if (!years.Contains(DateTime.Today.Year))
            {
                var message = "No se encontró Base de Datos correspondiente al año actual. Se recomienda crear una nueva.";
                lblError.Text = message;
                if (showMessages)
                    MessageBox.Show(message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            return true;
        }
        catch (Exception ex)
        {
            SetFailure(ex.Message, showMessages, MessageBoxIcon.Error);
            return false;
        }
    }

    private async Task<bool> CreateCurrentYearAsync(bool showMessages)
    {
        var year = DateTime.Today.Year;
        if (AvailableYears.Contains(year))
        {
            SetFailure("Ya existe una Base de Datos para el año actual", showMessages, MessageBoxIcon.Error);
            return false;
        }

        btnCrear.Enabled = false;
        SetLoginControlsEnabled(false);

        try
        {
            await DatabaseProvisioner.CreateYearAsync(year);
            if (!await ConnectAsync(showMessages: false))
                return false;

            var message = "La nueva Base de Datos fue creada con éxito.";
            lblError.Text = message;
            if (showMessages)
                MessageBox.Show(message, "Confirmación", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return true;
        }
        catch (Exception ex)
        {
            SetFailure(ex.Message, showMessages, MessageBoxIcon.Error);
            return false;
        }
        finally
        {
            btnCrear.Enabled = true;
            SetLoginControlsEnabled(true);
        }
    }

    private void BindYears(IReadOnlyList<int> years)
    {
        cbBD.Items.Clear();
        foreach (var year in years.OrderBy(year => year))
            cbBD.Items.Add(year.ToString());

        var currentYear = DateTime.Today.Year.ToString();
        if (cbBD.Items.Contains(currentYear))
            cbBD.SelectedItem = currentYear;
    }

    private void SetLoginControlsEnabled(bool enabled)
    {
        txtUsuario.Enabled = enabled;
        txtClave.Enabled = enabled;
        cbAceptar.Enabled = enabled;
    }

    private void SetFailure(string message, bool showMessages, MessageBoxIcon icon)
    {
        lblError.Text = message;
        if (showMessages)
            MessageBox.Show(message, icon == MessageBoxIcon.Error ? "Error" : "Aviso", MessageBoxButtons.OK, icon);
    }

    private IMediator Mediator => _mediator
        ?? throw new InvalidOperationException("Use the IMediator constructor at runtime. The parameterless constructor is for the WinForms designer only.");

    private IYearlyDatabaseCatalog DatabaseCatalog => _databaseCatalog
        ?? throw new InvalidOperationException("Use the IYearlyDatabaseCatalog constructor at runtime. The parameterless constructor is for the WinForms designer only.");

    private IYearlyDatabaseProvisioner DatabaseProvisioner => _databaseProvisioner
        ?? throw new InvalidOperationException("Use the IYearlyDatabaseProvisioner constructor at runtime. The parameterless constructor is for the WinForms designer only.");
}
