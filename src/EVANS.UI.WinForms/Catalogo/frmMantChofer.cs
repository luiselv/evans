using System.Globalization;
using EVANS.Application.Catalogo.Commands;
using EVANS.Application.Catalogo.DTOs;
using EVANS.Application.Catalogo.Queries;
using MediatR;

namespace EVANS.UI.WinForms.Catalogo;

public partial class frmMantChofer : Form
{
    private readonly IMediator? _mediator;
    private IReadOnlyList<ChoferDto> _choferes = [];
    private IReadOnlyList<EmpresaDto> _empresas = [];
    private IReadOnlyList<EmpresaDto> _empresasActivas = [];
    private IReadOnlyList<EstadoDto> _estados = [];
    private bool _isCreateMode = true;

    public frmMantChofer()
    {
        InitializeComponent();
    }

    public frmMantChofer(IMediator mediator)
    {
        _mediator = mediator;
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
    internal string NombreText => txtNombre.Text;
    internal string DireccionText => txtDireccion.Text;
    internal string LicenciaText => txtLicencia.Text;
    internal string TelefonoText => txtTelefono.Text;
    internal string EmpresaText => cbEmpresa.Text;
    internal string EstadoText => cbEstado.Text;
    internal string StatusMessage => lblmsg.Text;
    internal int ListCount => lvListado.Items.Count;
    internal string? FirstListName => lvListado.Items.Count == 0 ? null : lvListado.Items[0].SubItems[1].Text;
    internal string? FirstListLicencia => lvListado.Items.Count == 0 ? null : lvListado.Items[0].SubItems[2].Text;
    internal string? FirstListEmpresa => lvListado.Items.Count == 0 ? null : lvListado.Items[0].SubItems[3].Text;

    internal void SetTestSearchText(string value) => txtBuscar.Text = value;
    internal void SetTestNombre(string value) => txtNombre.Text = value;
    internal void SetTestDireccion(string value) => txtDireccion.Text = value;
    internal void SetTestLicencia(string value) => txtLicencia.Text = value;
    internal void SetTestTelefono(string value) => txtTelefono.Text = value;
    internal void SetTestEmpresa(int codigo) => SelectEmpresa(codigo);
    internal void SetTestEstado(int codigo) => SelectEstado(codigo);
    internal void BeginNewForTest() => btnNuevo_Click();
    internal void BeginEditForTest() => btnEditar_Click();

    internal async Task OpenFirstForTestAsync()
    {
        if (lvListado.Items.Count == 0)
            return;

        await OpenChoferAsync(int.Parse(lvListado.Items[0].SubItems[0].Text, CultureInfo.InvariantCulture));
    }

    internal async Task LoadChoferesAsync()
    {
        await EnsureCombosAsync();
        _choferes = await Mediator.Send(new ListChoferesMaintenanceQuery());
        BindChoferes(_choferes);
    }

    internal async Task<bool> BuscarAsync(bool showMessages = false)
    {
        if (string.IsNullOrWhiteSpace(txtBuscar.Text))
        {
            lvListado.Items.Clear();
            SetStatus("Ingrese nombre a buscar");
            if (showMessages)
                MessageBox.Show("Ingrese nombre a buscar", "Datos insuficientes", MessageBoxButtons.OK, MessageBoxIcon.Information);
            txtBuscar.Focus();
            return false;
        }

        await EnsureEmpresasAsync();
        if (_choferes.Count == 0)
            _choferes = await Mediator.Send(new ListChoferesMaintenanceQuery());

        var search = txtBuscar.Text.Trim();
        BindChoferes(_choferes.Where(chofer => chofer.NombreCompleto.StartsWith(search, StringComparison.CurrentCultureIgnoreCase)));
        SetStatus("Los campos marcados con asterisco (*) son obligatorios");
        return true;
    }

    internal async Task<bool> SaveAsync(bool showMessages = false)
    {
        var empresaCodigo = ResolveEmpresaCodigo();
        if (string.IsNullOrWhiteSpace(txtNombre.Text)
            || string.IsNullOrWhiteSpace(txtLicencia.Text)
            || empresaCodigo is null
            || cbEstado.SelectedValue is null)
        {
            SetStatus("Datos incompletos");
            if (showMessages)
                MessageBox.Show("Datos incompletos", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return false;
        }

        var nombre = txtNombre.Text.ToUpperInvariant();
        var direccion = txtDireccion.Text.ToUpperInvariant();
        var licencia = txtLicencia.Text.ToUpperInvariant();
        var telefono = txtTelefono.Text;
        var estadoCodigo = Convert.ToInt32(cbEstado.SelectedValue, CultureInfo.InvariantCulture);

        if (_isCreateMode)
        {
            var result = await Mediator.Send(new CreateChoferCommand(nombre, licencia, telefono, direccion, empresaCodigo.Value, estadoCodigo));
            if (!result.IsSuccess)
                return ShowFailure(result.Error, showMessages);
        }
        else
        {
            var result = await Mediator.Send(new UpdateChoferCommand(
                int.Parse(txtCodigo.Text, CultureInfo.InvariantCulture),
                nombre,
                licencia,
                telefono,
                direccion,
                empresaCodigo.Value,
                estadoCodigo));
            if (!result.IsSuccess)
                return ShowFailure(result.Error, showMessages);
            _isCreateMode = true;
        }

        await ReturnToListingAsync();
        return true;
    }

    private void WireEvents()
    {
        Load += async (_, _) => await LoadCombosOnlyAsync();
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
        cbEstado.DropDown += async (_, _) => await EnsureEstadosAsync();
        cbEmpresa.DropDown += async (_, _) => await EnsureEmpresasAsync();
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
        await EnsureEmpresasAsync();
        if (_choferes.Count == 0)
            _choferes = await Mediator.Send(new ListChoferesMaintenanceQuery());

        BindChoferes(_choferes);
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

        await OpenChoferAsync(int.Parse(lvListado.SelectedItems[0].SubItems[0].Text, CultureInfo.InvariantCulture));
    }

    private async Task OpenChoferAsync(int codigo)
    {
        await EnsureCombosAsync();
        var chofer = await Mediator.Send(new GetChoferByIdQuery(codigo));
        if (chofer is null)
            return;

        tabPageDetalles.Enabled = true;
        tabPageListado.Enabled = false;
        tabControl1.SelectTab(tabPageDetalles);

        txtCodigo.Text = chofer.Codigo.ToString(CultureInfo.InvariantCulture);
        txtNombre.Text = chofer.NombreCompleto;
        txtDireccion.Text = chofer.Direccion;
        txtLicencia.Text = chofer.Licencia;
        txtTelefono.Text = chofer.Telefono;
        SelectEmpresa(chofer.EmpresaCodigo);
        SelectEstado(chofer.EstadoCodigo);
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
        tabPageDetalles.Enabled = true;
        tabPageListado.Enabled = false;
        tabControl1.SelectTab(tabPageDetalles);

        txtCodigo.Clear();
        txtNombre.Clear();
        txtDireccion.Clear();
        txtLicencia.Clear();
        txtTelefono.Clear();
        cbEmpresa.SelectedIndex = _empresasActivas.Count == 0 ? -1 : 0;
        SelectEstadoByDescription("ACTIVO");
        SetDetailInputsEnabled(true);
        txtCodigo.ReadOnly = true;
        txtNombre.Focus();

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
        txtNombre.Focus();

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
        btnGrabar.Text = "Grabar";
        btnNuevo.Enabled = true;
        btnGrabar.Enabled = false;
        btnEditar.Enabled = false;
        btnCancelar.Enabled = false;
        optBuscar.Checked = true;

        _choferes = await Mediator.Send(new ListChoferesMaintenanceQuery());
        lvListado.Items.Clear();
    }

    private async Task LoadCombosOnlyAsync()
    {
        await EnsureCombosAsync();
    }

    private async Task EnsureCombosAsync()
    {
        await EnsureEstadosAsync();
        await EnsureEmpresasAsync();
    }

    private async Task EnsureEstadosAsync()
    {
        if (_estados.Count > 0)
            return;

        _estados = await Mediator.Send(new ListEstadosQuery());
        cbEstado.DataSource = _estados.ToList();
        cbEstado.DisplayMember = nameof(EstadoDto.Descripcion);
        cbEstado.ValueMember = nameof(EstadoDto.Codigo);
    }

    private async Task EnsureEmpresasAsync()
    {
        if (_empresas.Count == 0)
            _empresas = await Mediator.Send(new ListEmpresasMaintenanceQuery());

        if (_empresasActivas.Count > 0)
            return;

        _empresasActivas = await Mediator.Send(new ListEmpresasQuery());
        cbEmpresa.DataSource = _empresasActivas.ToList();
        cbEmpresa.DisplayMember = nameof(EmpresaDto.RazonSocial);
        cbEmpresa.ValueMember = nameof(EmpresaDto.Codigo);
    }

    private void SelectEmpresa(int codigo)
    {
        if (_empresasActivas.Count == 0)
            return;

        cbEmpresa.SelectedValue = codigo;
        if (!Equals(cbEmpresa.SelectedValue, codigo))
            cbEmpresa.Text = GetEmpresaName(codigo);
    }

    private void SelectEstado(int codigo)
    {
        if (_estados.Count == 0)
            return;

        cbEstado.SelectedValue = codigo;
    }

    private void SelectEstadoByDescription(string descripcion)
    {
        var estado = _estados.FirstOrDefault(e => string.Equals(e.Descripcion, descripcion, StringComparison.CurrentCultureIgnoreCase));
        if (estado is not null)
            SelectEstado(estado.Codigo);
        else if (_estados.Count > 0)
            cbEstado.SelectedIndex = 0;
    }

    private int? ResolveEmpresaCodigo()
    {
        if (cbEmpresa.SelectedValue is not null)
            return Convert.ToInt32(cbEmpresa.SelectedValue, CultureInfo.InvariantCulture);

        var empresa = _empresas.FirstOrDefault(e => string.Equals(e.RazonSocial, cbEmpresa.Text, StringComparison.CurrentCultureIgnoreCase));
        return empresa?.Codigo;
    }

    private void SetDetailInputsEnabled(bool enabled)
    {
        txtCodigo.Enabled = enabled;
        txtNombre.Enabled = enabled;
        txtDireccion.Enabled = enabled;
        txtLicencia.Enabled = enabled;
        txtTelefono.Enabled = enabled;
        cbEmpresa.Enabled = enabled;
        cbEstado.Enabled = enabled;
    }

    private void BindChoferes(IEnumerable<ChoferDto> choferes)
    {
        lvListado.Items.Clear();
        foreach (var chofer in choferes)
        {
            var item = lvListado.Items.Add(chofer.Codigo.ToString(CultureInfo.InvariantCulture));
            item.SubItems.Add(chofer.NombreCompleto);
            item.SubItems.Add(chofer.Licencia);
            item.SubItems.Add(GetEmpresaName(chofer.EmpresaCodigo));
        }
    }

    private string GetEmpresaName(int codigo) =>
        _empresas.FirstOrDefault(e => e.Codigo == codigo)?.RazonSocial
        ?? _empresasActivas.FirstOrDefault(e => e.Codigo == codigo)?.RazonSocial
        ?? string.Empty;

    private bool ShowFailure(string? error, bool showMessages)
    {
        var message = error ?? "No se pudo guardar el chofer.";
        SetStatus(message);
        if (showMessages)
            MessageBox.Show(message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        return false;
    }

    private void SetStatus(string message) => lblmsg.Text = message;

    private IMediator Mediator => _mediator
        ?? throw new InvalidOperationException("Use the IMediator constructor at runtime. The parameterless constructor is for the WinForms designer only.");
}
