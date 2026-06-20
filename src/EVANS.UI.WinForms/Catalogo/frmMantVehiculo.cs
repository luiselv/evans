using System.Globalization;
using EVANS.Application.Catalogo.Commands;
using EVANS.Application.Catalogo.DTOs;
using EVANS.Application.Catalogo.Queries;
using MediatR;

namespace EVANS.UI.WinForms.Catalogo;

public partial class frmMantVehiculo : Form
{
    private readonly IMediator? _mediator;
    private IReadOnlyList<VehiculoDto> _vehiculos = [];
    private IReadOnlyList<EstadoDto> _estados = [];
    private IReadOnlyList<EmpresaDto> _empresas = [];
    private IReadOnlyList<EmpresaDto> _empresasActivas = [];
    private bool _isCreateMode = true;

    public frmMantVehiculo() => InitializeComponent();

    public frmMantVehiculo(IMediator mediator)
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
    internal string StatusMessage => lblmsg.Text;
    internal int ListCount => lvListado.Items.Count;
    internal string? FirstListMarca => lvListado.Items.Count == 0 ? null : lvListado.Items[0].SubItems[1].Text;
    internal string? FirstListPlaca => lvListado.Items.Count == 0 ? null : lvListado.Items[0].SubItems[2].Text;
    internal void SetTestSearchText(string value) => txtBuscar.Text = value;
    internal void SetTestMarca(string value) => txtMarca.Text = value;
    internal void SetTestPlaca(string value) => txtPlaca.Text = value;
    internal void SetTestConfiguracion(string value) => txtConfiguracion.Text = value;
    internal void SetTestCertificado(string value) => txtCertificado.Text = value;
    internal void SetTestEmpresa(int codigo) => SelectEmpresa(codigo);
    internal void SetTestEstado(int codigo) => SelectEstado(codigo);
    internal void SelectAllSearchForTest() => optTodos.Checked = true;
    internal void SelectMarcaSearchForTest() => optMarca.Checked = true;
    internal void SelectPlacaSearchForTest() => optBuscar.Checked = true;
    internal void BeginNewForTest() => btnNuevo_Click();
    internal void BeginEditForTest() => btnEditar_Click();

    internal async Task OpenFirstForTestAsync()
    {
        if (lvListado.Items.Count > 0)
            await OpenVehiculoAsync(int.Parse(lvListado.Items[0].SubItems[0].Text, CultureInfo.InvariantCulture));
    }

    internal async Task LoadVehiculosAsync()
    {
        await EnsureCombosAsync();
        _vehiculos = await Mediator.Send(new ListVehiculosMaintenanceQuery());
        BindVehiculos(_vehiculos);
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

        if (_vehiculos.Count == 0)
            _vehiculos = await Mediator.Send(new ListVehiculosMaintenanceQuery());

        var search = txtBuscar.Text.Trim();
        var filtered = optMarca.Checked
            ? _vehiculos.Where(v => (v.Marca ?? string.Empty).StartsWith(search, StringComparison.CurrentCultureIgnoreCase))
            : _vehiculos.Where(v => v.Placa.StartsWith(search, StringComparison.CurrentCultureIgnoreCase));
        BindVehiculos(filtered);
        SetStatus("Los campos marcados con asterisco (*) son obligatorios");
        return true;
    }

    internal async Task<bool> SaveAsync(bool showMessages = false)
    {
        var empresaCodigo = ResolveEmpresaCodigo();
        var estadoCodigo = ResolveEstadoCodigo();
        if (string.IsNullOrWhiteSpace(txtMarca.Text) || string.IsNullOrWhiteSpace(txtPlaca.Text)
            || string.IsNullOrWhiteSpace(txtConfiguracion.Text) || string.IsNullOrWhiteSpace(txtCertificado.Text)
            || empresaCodigo is null || estadoCodigo is null)
        {
            SetStatus("Datos incompletos");
            if (showMessages)
                MessageBox.Show("Datos incompletos", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return false;
        }

        var marca = txtMarca.Text.Trim().ToUpperInvariant();
        var placa = txtPlaca.Text.Trim().ToUpperInvariant();
        var configuracion = txtConfiguracion.Text.Trim().ToUpperInvariant();
        var certificado = txtCertificado.Text.Trim().ToUpperInvariant();

        if (_isCreateMode)
        {
            var result = await Mediator.Send(new CreateVehiculoCommand(
                marca, placa, configuracion, certificado, empresaCodigo.Value, estadoCodigo.Value));
            if (!result.IsSuccess) return ShowFailure(result.Error, showMessages);
        }
        else
        {
            var result = await Mediator.Send(new UpdateVehiculoCommand(
                int.Parse(txtCodigo.Text, CultureInfo.InvariantCulture), marca, placa, configuracion, certificado,
                empresaCodigo.Value, estadoCodigo.Value));
            if (!result.IsSuccess) return ShowFailure(result.Error, showMessages);
            _isCreateMode = true;
        }

        await ReturnToListingAsync();
        return true;
    }

    private void WireEvents()
    {
        Load += async (_, _) => await EnsureCombosAsync();
        optTodos.CheckedChanged += async (_, _) => await optTodos_CheckedChanged();
        optMarca.CheckedChanged += (_, _) => SearchOption_CheckedChanged(optMarca);
        optBuscar.CheckedChanged += (_, _) => SearchOption_CheckedChanged(optBuscar);
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
        if (!optTodos.Checked) return;
        txtBuscar.Clear();
        await EnsureEmpresasAsync();
        if (_vehiculos.Count == 0)
            _vehiculos = await Mediator.Send(new ListVehiculosMaintenanceQuery());
        BindVehiculos(_vehiculos);
        txtBuscar.Enabled = false;
        btnBuscar.Enabled = false;
    }

    private void SearchOption_CheckedChanged(RadioButton option)
    {
        if (!option.Checked) return;
        lvListado.Items.Clear();
        txtBuscar.Enabled = true;
        txtBuscar.Focus();
        btnBuscar.Enabled = true;
    }

    private async Task lvListado_DoubleClick()
    {
        if (lvListado.SelectedItems.Count > 0)
            await OpenVehiculoAsync(int.Parse(lvListado.SelectedItems[0].SubItems[0].Text, CultureInfo.InvariantCulture));
    }

    private async Task OpenVehiculoAsync(int codigo)
    {
        await EnsureCombosAsync();
        var vehiculo = await Mediator.Send(new GetVehiculoByIdQuery(codigo));
        if (vehiculo is null) return;

        tabPageDetalles.Enabled = true;
        tabPageListado.Enabled = false;
        tabControl1.SelectTab(tabPageDetalles);
        txtCodigo.Text = vehiculo.Codigo.ToString(CultureInfo.InvariantCulture);
        txtMarca.Text = vehiculo.Marca;
        txtPlaca.Text = vehiculo.Placa;
        txtConfiguracion.Text = vehiculo.ConfiguracionVehicular;
        txtCertificado.Text = vehiculo.CertificadoInscripcion;
        SelectEmpresa(vehiculo.EmpresaCodigo);
        SelectEstado(vehiculo.EstadoCodigo);
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
        txtMarca.Clear();
        txtPlaca.Clear();
        txtConfiguracion.Clear();
        txtCertificado.Clear();
        cbEmpresa.SelectedIndex = -1;
        cbEstado.SelectedIndex = -1;
        SetDetailInputsEnabled(true);
        txtCodigo.ReadOnly = true;
        txtMarca.Focus();
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
        txtMarca.Focus();
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
        _vehiculos = await Mediator.Send(new ListVehiculosMaintenanceQuery());
        lvListado.Items.Clear();
    }

    private async Task EnsureCombosAsync()
    {
        await EnsureEstadosAsync();
        await EnsureEmpresasAsync();
    }

    private async Task EnsureEstadosAsync()
    {
        if (_estados.Count > 0) return;
        _estados = await Mediator.Send(new ListEstadosQuery());
        cbEstado.DataSource = _estados.ToList();
        cbEstado.DisplayMember = nameof(EstadoDto.Descripcion);
        cbEstado.ValueMember = nameof(EstadoDto.Codigo);
    }

    private async Task EnsureEmpresasAsync()
    {
        if (_empresas.Count == 0)
            _empresas = await Mediator.Send(new ListEmpresasMaintenanceQuery());
        if (_empresasActivas.Count > 0) return;
        _empresasActivas = await Mediator.Send(new ListEmpresasQuery());
        cbEmpresa.DataSource = _empresasActivas.ToList();
        cbEmpresa.DisplayMember = nameof(EmpresaDto.RazonSocial);
        cbEmpresa.ValueMember = nameof(EmpresaDto.Codigo);
    }

    private void SelectEmpresa(int codigo)
    {
        if (_empresasActivas.Count == 0) return;
        cbEmpresa.SelectedValue = codigo;
        if (!Equals(cbEmpresa.SelectedValue, codigo)) cbEmpresa.Text = GetEmpresaName(codigo);
    }

    private void SelectEstado(int codigo)
    {
        if (_estados.Count > 0) cbEstado.SelectedValue = codigo;
    }

    private int? ResolveEmpresaCodigo()
    {
        if (cbEmpresa.SelectedValue is not null)
            return Convert.ToInt32(cbEmpresa.SelectedValue, CultureInfo.InvariantCulture);
        return _empresas.FirstOrDefault(e => string.Equals(e.RazonSocial, cbEmpresa.Text, StringComparison.CurrentCultureIgnoreCase))?.Codigo;
    }

    private int? ResolveEstadoCodigo()
    {
        if (cbEstado.SelectedValue is not null)
            return Convert.ToInt32(cbEstado.SelectedValue, CultureInfo.InvariantCulture);
        return _estados.FirstOrDefault(e => string.Equals(e.Descripcion, cbEstado.Text, StringComparison.CurrentCultureIgnoreCase))?.Codigo;
    }

    private void SetDetailInputsEnabled(bool enabled)
    {
        txtCodigo.Enabled = enabled;
        txtMarca.Enabled = enabled;
        txtPlaca.Enabled = enabled;
        txtConfiguracion.Enabled = enabled;
        txtCertificado.Enabled = enabled;
        cbEmpresa.Enabled = enabled;
        cbEstado.Enabled = enabled;
    }

    private void BindVehiculos(IEnumerable<VehiculoDto> vehiculos)
    {
        lvListado.Items.Clear();
        foreach (var vehiculo in vehiculos)
        {
            var item = lvListado.Items.Add(vehiculo.Codigo.ToString(CultureInfo.InvariantCulture));
            item.SubItems.Add(vehiculo.Marca ?? string.Empty);
            item.SubItems.Add(vehiculo.Placa);
        }
    }

    private string GetEmpresaName(int codigo) =>
        _empresas.FirstOrDefault(e => e.Codigo == codigo)?.RazonSocial
        ?? _empresasActivas.FirstOrDefault(e => e.Codigo == codigo)?.RazonSocial
        ?? string.Empty;

    private bool ShowFailure(string? error, bool showMessages)
    {
        var message = error ?? "No se pudo guardar el vehículo.";
        SetStatus(message);
        if (showMessages) MessageBox.Show(message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        return false;
    }

    private void SetStatus(string message) => lblmsg.Text = message;

    private IMediator Mediator => _mediator
        ?? throw new InvalidOperationException("Use the IMediator constructor at runtime. The parameterless constructor is for the WinForms designer only.");
}
