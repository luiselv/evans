using EVANS.Application.Catalogo.DTOs;
using EVANS.Application.Catalogo.Queries;
using EVANS.Application.Common;
using EVANS.Application.Identidad.Commands;
using EVANS.Application.Identidad.DTOs;
using EVANS.Application.Identidad.Ports;
using EVANS.Application.Identidad.Queries;
using MediatR;

namespace EVANS.UI.WinForms.Identidad;

public partial class frmMantUsuarios : Form
{
    private readonly IMediator? _mediator;
    private readonly ICurrentSession? _currentSession;
    private IReadOnlyList<UsuarioCuentaDto> _usuarios = [];
    private IReadOnlyList<EstadoDto> _estados = [];
    private UsuarioCuentaDto? _selectedUsuario;
    private bool _isCreateMode = true;
    private bool _suppressAdminCheck;

    public frmMantUsuarios() => InitializeComponent();

    public frmMantUsuarios(IMediator mediator, ICurrentSession currentSession)
    {
        _mediator = mediator;
        _currentSession = currentSession;
        InitializeComponent();
        WireEvents();
        ApplyInitialState();
    }

    internal int SelectedTabIndex => tabControl1.SelectedIndex;
    internal bool DetailsTabEnabled => tabPageDetalles.Enabled;
    internal bool ListingTabEnabled => tabPageListado.Enabled;
    internal bool BuscarEnabled => btnBuscar.Enabled;
    internal bool BuscarTextEnabled => txtBuscar.Enabled;
    internal bool NuevoEnabled => btnNuevo.Enabled;
    internal bool GrabarEnabled => btnGrabar.Enabled;
    internal bool EditarEnabled => btnEditar.Enabled;
    internal bool CancelarEnabled => btnCancelar.Enabled;
    internal string GrabarText => btnGrabar.Text;
    internal string CodigoText => txtCodigo.Text;
    internal string EmpleadoText => txtEmpleado.Text;
    internal string UsuarioText => txtUsuario.Text;
    internal string StatusMessage => lblmsg.Text;
    internal int ListCount => lvListado.Items.Count;
    internal string? FirstListUsuario => lvListado.Items.Count == 0 ? null : lvListado.Items[0].SubItems[2].Text;
    internal bool AdminChecked => chkAdmin.Checked;

    internal void SetTestSearchText(string value) => txtBuscar.Text = value;
    internal void SetTestEmpleado(string value) => txtEmpleado.Text = value;
    internal void SetTestUsuario(string value) => txtUsuario.Text = value;
    internal void SetTestClave(string value) => txtClave.Text = value;
    internal void SetTestRepetir(string value) => txtRepetir.Text = value;
    internal void SetTestEstado(int codigo)
    {
        EnsureEstadoItem(codigo, codigo == 1 ? "ACTIVO" : codigo.ToString());
        cbEstado.SelectedValue = codigo;
    }
    internal void SetTestAdmin(bool value) => chkAdmin.Checked = value;
    internal void BeginNewForTest() => btnNuevo_Click();
    internal void BeginEditForTest() => btnEditar_Click();

    internal async Task OpenFirstForTestAsync()
    {
        if (lvListado.Items.Count == 0)
            return;

        await OpenUsuarioAsync(int.Parse(lvListado.Items[0].SubItems[0].Text));
    }

    internal async Task LoadUsuariosAsync()
    {
        _usuarios = await Mediator.Send(new BuscarUsuariosQuery(null));
        BindUsuarios(_usuarios);
    }

    internal async Task<bool> BuscarAsync(bool showMessages = false)
    {
        if (string.IsNullOrWhiteSpace(txtBuscar.Text))
        {
            lvListado.Items.Clear();
            return ShowInformation("Ingrese nombre a buscar", "Datos insuficientes", showMessages);
        }

        _usuarios = await Mediator.Send(new BuscarUsuariosQuery(txtBuscar.Text.Trim()));
        BindUsuarios(_usuarios);
        SetStatus(DefaultStatusMessage);
        return true;
    }

    internal async Task<bool> SaveAsync(bool showMessages = false)
    {
        if (string.IsNullOrWhiteSpace(txtEmpleado.Text) ||
            string.IsNullOrWhiteSpace(txtUsuario.Text) ||
            string.IsNullOrWhiteSpace(txtClave.Text) ||
            string.IsNullOrWhiteSpace(txtRepetir.Text) ||
            cbEstado.SelectedValue is null)
        {
            return ShowError("Datos incompletos", showMessages);
        }

        if (!string.Equals(txtClave.Text, txtRepetir.Text, StringComparison.Ordinal))
            return ShowError("La nueva clave no concuerda con la confirmación.", showMessages);

        if (!_isCreateMode && !CanEditSelectedUsuario())
            return ShowError("Solo un usuario administrador o el propietario de la cuenta pueden modificar este registro.", showMessages);

        var estadoCodigo = (int)cbEstado.SelectedValue;
        if (_isCreateMode)
        {
            var result = await Mediator.Send(new CrearUsuarioCommand(
                txtUsuario.Text,
                txtClave.Text,
                txtEmpleado.Text,
                chkAdmin.Checked,
                estadoCodigo));
            if (!result.IsSuccess)
                return ShowFailure(result, showMessages);
        }
        else
        {
            var result = await Mediator.Send(new ActualizarUsuarioCommand(
                int.Parse(txtCodigo.Text),
                txtUsuario.Text,
                txtClave.Text,
                txtEmpleado.Text,
                chkAdmin.Checked,
                estadoCodigo));
            if (!result.IsSuccess)
                return ShowFailure(result, showMessages);
        }

        await ReturnToListingAsync();
        return true;
    }

    private void WireEvents()
    {
        optTodos.CheckedChanged += async (_, _) => await optTodos_CheckedChanged();
        optBuscar.CheckedChanged += (_, _) => optBuscar_CheckedChanged();
        lvListado.DoubleClick += async (_, _) => await lvListado_DoubleClick();
        btnNuevo.Click += (_, _) => btnNuevo_Click();
        btnGrabar.Click += async (_, _) => await SaveAsync(showMessages: true);
        btnEditar.Click += (_, _) => btnEditar_Click();
        btnCancelar.Click += async (_, _) => await ReturnToListingAsync();
        btnSalir.Click += (_, _) => Close();
        btnBuscar.Click += async (_, _) => await BuscarAsync(showMessages: true);
        txtBuscar.KeyPress += async (_, e) =>
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                e.Handled = true;
                await BuscarAsync(showMessages: true);
            }
        };
        cbEstado.DropDown += async (_, _) => await LoadEstadosAsync();
        chkAdmin.CheckedChanged += (_, _) => chkAdmin_CheckedChanged();
    }

    private void ApplyInitialState()
    {
        Top = 0;
        Left = 0;

        tabPageDetalles.Enabled = false;
        txtBuscar.Enabled = false;

        btnGrabar.Enabled = false;
        btnCancelar.Enabled = false;
        btnEditar.Enabled = false;
        btnBuscar.Enabled = false;

        optBuscar.Checked = true;
    }

    private async Task optTodos_CheckedChanged()
    {
        if (!optTodos.Checked)
            return;

        txtBuscar.Clear();
        _usuarios = await Mediator.Send(new BuscarUsuariosQuery(null));
        BindUsuarios(_usuarios);
        txtBuscar.Enabled = false;
        btnBuscar.Enabled = false;
    }

    private void optBuscar_CheckedChanged()
    {
        if (!optBuscar.Checked)
            return;

        lvListado.Items.Clear();
        txtBuscar.Enabled = true;
        txtBuscar.Focus();
        btnBuscar.Enabled = true;
    }

    private async Task lvListado_DoubleClick()
    {
        if (lvListado.SelectedItems.Count == 0)
            return;

        await OpenUsuarioAsync(int.Parse(lvListado.SelectedItems[0].SubItems[0].Text));
    }

    private async Task OpenUsuarioAsync(int codigo)
    {
        var usuario = await Mediator.Send(new ObtenerUsuarioPorCodigoQuery(codigo));
        if (usuario is null)
            return;

        _selectedUsuario = usuario;
        await LoadEstadosAsync();
        EnsureEstadoItem(usuario.EstadoCodigo, usuario.EstadoCodigo.ToString());

        tabPageDetalles.Enabled = true;
        tabPageListado.Enabled = false;
        tabControl1.SelectTab(tabPageDetalles);

        txtCodigo.Text = usuario.Codigo.ToString();
        txtEmpleado.Text = usuario.NombreCompleto;
        txtUsuario.Text = usuario.NombreUsuario;
        txtClave.Text = usuario.Clave;
        txtRepetir.Text = usuario.Clave;
        cbEstado.SelectedValue = usuario.EstadoCodigo;
        SetAdminChecked(usuario.EsAdministrador);
        SetDetailInputsEnabled(false);

        btnNuevo.Enabled = true;
        btnGrabar.Enabled = false;
        btnEditar.Enabled = true;
        btnCancelar.Enabled = true;
        btnGrabar.Text = "Grabar";
    }

    private void btnNuevo_Click()
    {
        _isCreateMode = true;
        _selectedUsuario = null;
        tabPageDetalles.Enabled = true;
        tabPageListado.Enabled = false;
        tabControl1.SelectTab(tabPageDetalles);

        ClearDetails();
        SetDetailInputsEnabled(true);
        txtCodigo.ReadOnly = true;
        txtEmpleado.Focus();

        btnNuevo.Enabled = false;
        btnGrabar.Enabled = true;
        btnCancelar.Enabled = true;
        btnEditar.Enabled = false;
        btnGrabar.Text = "Grabar";
    }

    private void btnEditar_Click()
    {
        SetDetailInputsEnabled(true);
        txtCodigo.ReadOnly = true;
        txtEmpleado.Focus();

        btnNuevo.Enabled = false;
        btnGrabar.Enabled = true;
        btnEditar.Enabled = false;
        btnCancelar.Enabled = true;
        btnGrabar.Text = "Actualizar";
        _isCreateMode = false;
    }

    private async Task ReturnToListingAsync()
    {
        tabPageListado.Enabled = true;
        tabPageDetalles.Enabled = false;
        tabControl1.SelectTab(tabPageListado);

        _isCreateMode = true;
        _selectedUsuario = null;
        btnGrabar.Text = "Grabar";
        btnNuevo.Enabled = true;
        btnGrabar.Enabled = false;
        btnEditar.Enabled = false;
        btnCancelar.Enabled = false;
        optBuscar.Checked = true;
        lvListado.Items.Clear();
        await Task.CompletedTask;
    }

    private async Task LoadEstadosAsync()
    {
        if (_estados.Count != 0)
            return;

        _estados = await Mediator.Send(new ListEstadosQuery());
        BindEstados();
    }

    private void BindEstados()
    {
        cbEstado.DisplayMember = nameof(EstadoDto.Descripcion);
        cbEstado.ValueMember = nameof(EstadoDto.Codigo);
        cbEstado.DataSource = _estados.ToList();
    }

    private void EnsureEstadoItem(int codigo, string descripcion)
    {
        if (_estados.Any(estado => estado.Codigo == codigo))
        {
            BindEstados();
            return;
        }

        _estados = _estados.Append(new EstadoDto(codigo, descripcion)).ToList();
        BindEstados();
    }

    private void SetDetailInputsEnabled(bool enabled)
    {
        txtCodigo.Enabled = enabled;
        txtEmpleado.Enabled = enabled;
        txtUsuario.Enabled = enabled;
        txtClave.Enabled = enabled;
        txtRepetir.Enabled = enabled;
        cbEstado.Enabled = enabled;
        chkAdmin.Enabled = enabled;
    }

    private void ClearDetails()
    {
        txtCodigo.Clear();
        txtEmpleado.Clear();
        txtUsuario.Clear();
        txtClave.Clear();
        txtRepetir.Clear();
        cbEstado.SelectedIndex = -1;
        SetAdminChecked(false);
    }

    private void BindUsuarios(IEnumerable<UsuarioCuentaDto> usuarios)
    {
        lvListado.Items.Clear();
        foreach (var usuario in usuarios)
        {
            var item = lvListado.Items.Add(usuario.Codigo.ToString());
            item.SubItems.Add(usuario.NombreCompleto);
            item.SubItems.Add(usuario.NombreUsuario);
        }
    }

    private void chkAdmin_CheckedChanged()
    {
        if (_suppressAdminCheck || !chkAdmin.Checked || CurrentUserIsAdmin)
            return;

        ShowInformation("Solo un usuario administrador puede modificar esta opción.", "Mensaje", showMessages: false);
        SetAdminChecked(false);
    }

    private void SetAdminChecked(bool value)
    {
        _suppressAdminCheck = true;
        chkAdmin.Checked = value;
        _suppressAdminCheck = false;
    }

    private bool CanEditSelectedUsuario()
    {
        if (CurrentUserIsAdmin)
            return true;

        var currentUserName = _currentSession?.Current?.Usuario.NombreUsuario;
        var selectedUserName = _selectedUsuario?.NombreUsuario;
        return !string.IsNullOrWhiteSpace(currentUserName) &&
               string.Equals(currentUserName, selectedUserName, StringComparison.OrdinalIgnoreCase);
    }

    private bool CurrentUserIsAdmin => _currentSession?.Current?.Usuario.EsAdministrador == true;

    private bool ShowFailure<T>(Result<T> result, bool showMessages) =>
        ShowError(result.Error ?? "No se pudo guardar el usuario.", showMessages);

    private bool ShowError(string message, bool showMessages)
    {
        SetStatus(message);
        if (showMessages)
            MessageBox.Show(message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        return false;
    }

    private bool ShowInformation(string message, string caption, bool showMessages)
    {
        SetStatus(message);
        if (showMessages)
            MessageBox.Show(message, caption, MessageBoxButtons.OK, MessageBoxIcon.Information);
        txtBuscar.Focus();
        return false;
    }

    private void SetStatus(string message) => lblmsg.Text = message;

    private const string DefaultStatusMessage = "Los campos marcados con asterisco (*) son obligatorios";

    private IMediator Mediator => _mediator
        ?? throw new InvalidOperationException("Use the IMediator constructor at runtime. The parameterless constructor is for the WinForms designer only.");
}
