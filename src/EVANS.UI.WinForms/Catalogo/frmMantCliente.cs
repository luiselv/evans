using System.Globalization;
using EVANS.Application.Catalogo.Commands;
using EVANS.Application.Catalogo.DTOs;
using EVANS.Application.Catalogo.Queries;
using MediatR;

namespace EVANS.UI.WinForms.Catalogo;

public partial class frmMantCliente : Form
{
    private readonly IMediator? _mediator;
    private IReadOnlyList<ClienteDto> _clientes = [];
    private IReadOnlyList<TipoIdentificacionDto> _tiposIdentificacion = [];
    private bool _isCreateMode = true;

    public frmMantCliente() => InitializeComponent();

    public frmMantCliente(IMediator mediator)
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
    internal int ListCount => dgvListado.Rows.Cast<DataGridViewRow>().Count(row => !row.IsNewRow);
    internal string? FirstListNombre => ListCount == 0 ? null : Convert.ToString(dgvListado.Rows[0].Cells[1].Value, CultureInfo.CurrentCulture);
    internal string? FirstListNumero => ListCount == 0 ? null : Convert.ToString(dgvListado.Rows[0].Cells[3].Value, CultureInfo.CurrentCulture);
    internal void SetTestSearchText(string value) => txtBuscar.Text = value;
    internal void SetTestNombre(string value) => txtNombre.Text = value;
    internal void SetTestNroId(string value) => txtNroID.Text = value;
    internal void SetTestTelefono(string value) => txtTelefono.Text = value;
    internal void SetTestFax(string value) => txtFax.Text = value;
    internal void SetTestEmail(string value) => txtEmail.Text = value;
    internal void SetTestRepresentante(string value) => txtRepresentante.Text = value;
    internal void SetTestTipoId(int codigo) => SelectTipoIdentificacion(codigo);
    internal void AddTestDireccion(string calle, string ciudad, string provincia) => dgvDireccion.Rows.Add(calle, ciudad, provincia);
    internal void SelectAllSearchForTest() => optTodos.Checked = true;
    internal void SelectNameSearchForTest() => optBuscar.Checked = true;
    internal void SelectNumberSearchForTest() => optNro.Checked = true;
    internal void BeginNewForTest() => btnNuevo_Click();
    internal void BeginEditForTest() => btnEditar_Click();

    internal async Task OpenFirstForTestAsync()
    {
        if (ListCount > 0)
            await OpenClienteAsync(Convert.ToInt32(dgvListado.Rows[0].Cells[0].Value, CultureInfo.InvariantCulture));
    }

    internal async Task LoadClientesAsync()
    {
        await EnsureTiposIdentificacionAsync();
        _clientes = await Mediator.Send(new ListClientesQuery());
        BindClientes(_clientes);
    }

    internal async Task<bool> BuscarAsync(bool showMessages = false)
    {
        if (string.IsNullOrWhiteSpace(txtBuscar.Text))
        {
            dgvListado.Rows.Clear();
            SetStatus("Ingrese nombre a buscar");
            if (showMessages)
                MessageBox.Show("Ingrese nombre a buscar", "Datos insuficientes", MessageBoxButtons.OK, MessageBoxIcon.Information);
            txtBuscar.Focus();
            return false;
        }

        if (_clientes.Count == 0)
            _clientes = await Mediator.Send(new ListClientesQuery());

        var search = txtBuscar.Text.Trim();
        var filtered = optNro.Checked
            ? _clientes.Where(cliente => string.Equals(cliente.NroIdentificacion, search, StringComparison.CurrentCultureIgnoreCase))
            : _clientes.Where(cliente => cliente.RazonSocial.StartsWith(search, StringComparison.CurrentCultureIgnoreCase));

        BindClientes(filtered);
        SetStatus("Los campos marcados con asterisco (*) son obligatorios");
        return true;
    }

    internal async Task<bool> SaveAsync(bool showMessages = false)
    {
        var tipoIdentificacion = ResolveTipoIdentificacion();
        if (string.IsNullOrWhiteSpace(txtNombre.Text) || tipoIdentificacion is null || string.IsNullOrWhiteSpace(txtNroID.Text))
        {
            SetStatus("Datos incompletos");
            if (showMessages)
                MessageBox.Show("Datos incompletos", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return false;
        }

        var razonSocial = txtNombre.Text.Trim().ToUpperInvariant();
        var numeroIdentificacion = txtNroID.Text.Trim();
        var direcciones = ReadDirecciones();

        if (_isCreateMode)
        {
            var result = await Mediator.Send(new CreateClienteCommand(
                razonSocial,
                tipoIdentificacion.Codigo,
                numeroIdentificacion,
                tipoIdentificacion.LongitudRequerida,
                EmptyToNull(txtTelefono.Text),
                EmptyToNull(txtFax.Text),
                EmptyToNull(txtEmail.Text),
                EmptyToNull(txtRepresentante.Text),
                direcciones));
            if (!result.IsSuccess) return ShowFailure(result.Error, showMessages);
        }
        else
        {
            var result = await Mediator.Send(new UpdateClienteCommand(
                int.Parse(txtCodigo.Text, CultureInfo.InvariantCulture),
                razonSocial,
                tipoIdentificacion.Codigo,
                numeroIdentificacion,
                tipoIdentificacion.LongitudRequerida,
                EmptyToNull(txtTelefono.Text),
                EmptyToNull(txtFax.Text),
                EmptyToNull(txtEmail.Text),
                EmptyToNull(txtRepresentante.Text),
                direcciones));
            if (!result.IsSuccess) return ShowFailure(result.Error, showMessages);
            _isCreateMode = true;
        }

        await ReturnToListingAsync();
        return true;
    }

    private void WireEvents()
    {
        Load += async (_, _) => await EnsureTiposIdentificacionAsync();
        optTodos.CheckedChanged += async (_, _) => await optTodos_CheckedChanged();
        optBuscar.CheckedChanged += (_, _) => SearchOption_CheckedChanged(optBuscar);
        optNro.CheckedChanged += (_, _) => SearchOption_CheckedChanged(optNro);
        dgvListado.DoubleClick += async (_, _) => await dgvListado_DoubleClick();
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
        cbTipoID.DropDown += async (_, _) => await EnsureTiposIdentificacionAsync();
        cbTipoID.SelectedIndexChanged += (_, _) => cbTipoID_SelectedIndexChanged();
        txtNroID.GotFocus += (_, _) => txtNroID_GotFocus();
        txtBuscarCodigo.KeyPress += async (_, e) => await txtBuscarCodigo_KeyPress(e);
        tabControl1.SelectedIndexChanged += (_, _) => txtBuscarCodigo.Enabled = tabControl1.SelectedIndex == 0;
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
        await EnsureTiposIdentificacionAsync();
        if (_clientes.Count == 0)
            _clientes = await Mediator.Send(new ListClientesQuery());
        BindClientes(_clientes);
        txtBuscar.Enabled = false;
        btnBuscar.Enabled = false;
    }

    private void SearchOption_CheckedChanged(RadioButton option)
    {
        if (!option.Checked) return;
        dgvListado.Rows.Clear();
        txtBuscar.Enabled = true;
        txtBuscar.Focus();
        btnBuscar.Enabled = true;
    }

    private async Task dgvListado_DoubleClick()
    {
        var row = dgvListado.SelectedRows.Cast<DataGridViewRow>().FirstOrDefault(row => !row.IsNewRow)
            ?? dgvListado.CurrentRow;
        if (row is not null && !row.IsNewRow)
            await OpenClienteAsync(Convert.ToInt32(row.Cells[0].Value, CultureInfo.InvariantCulture));
    }

    private async Task OpenClienteAsync(int codigo)
    {
        await EnsureTiposIdentificacionAsync();
        var cliente = await Mediator.Send(new GetClienteByIdQuery(codigo));
        if (cliente is null) return;

        tabPageDetalles.Enabled = true;
        tabPageListado.Enabled = false;
        tabControl1.SelectTab(tabPageDetalles);
        txtCodigo.Text = cliente.Codigo.ToString(CultureInfo.InvariantCulture);
        txtNombre.Text = cliente.RazonSocial;
        SelectTipoIdentificacion(cliente.TipoIdCodigo);
        txtNroID.Text = cliente.NroIdentificacion;
        txtTelefono.Text = cliente.Telefono ?? string.Empty;
        txtFax.Text = cliente.Fax ?? string.Empty;
        txtEmail.Text = cliente.Email ?? string.Empty;
        txtRepresentante.Text = cliente.Representante ?? string.Empty;
        dgvDireccion.Rows.Clear();
        foreach (var direccion in cliente.Direcciones)
            dgvDireccion.Rows.Add(direccion.Calle, direccion.Ciudad, direccion.Provincia);

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
        ClearDetails();
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
        _clientes = await Mediator.Send(new ListClientesQuery());
        dgvListado.Rows.Clear();
    }

    private async Task EnsureTiposIdentificacionAsync()
    {
        if (_tiposIdentificacion.Count > 0) return;
        _tiposIdentificacion = await Mediator.Send(new ListTiposIdentificacionQuery());
        cbTipoID.DataSource = _tiposIdentificacion.ToList();
        cbTipoID.DisplayMember = nameof(TipoIdentificacionDto.Descripcion);
        cbTipoID.ValueMember = nameof(TipoIdentificacionDto.Codigo);
    }

    private void SelectTipoIdentificacion(int codigo)
    {
        if (_tiposIdentificacion.Count == 0) return;
        cbTipoID.SelectedValue = codigo;
        if (!Equals(cbTipoID.SelectedValue, codigo))
            cbTipoID.Text = _tiposIdentificacion.FirstOrDefault(tipo => tipo.Codigo == codigo)?.Descripcion ?? string.Empty;
    }

    private TipoIdentificacionDto? ResolveTipoIdentificacion()
    {
        if (cbTipoID.SelectedValue is not null)
        {
            var codigo = Convert.ToInt32(cbTipoID.SelectedValue, CultureInfo.InvariantCulture);
            return _tiposIdentificacion.FirstOrDefault(tipo => tipo.Codigo == codigo);
        }

        return _tiposIdentificacion.FirstOrDefault(tipo =>
            string.Equals(tipo.Descripcion, cbTipoID.Text, StringComparison.CurrentCultureIgnoreCase));
    }

    private void cbTipoID_SelectedIndexChanged()
    {
        txtNroID.Clear();
        if (string.Equals(cbTipoID.Text, "sin documento", StringComparison.CurrentCultureIgnoreCase))
            txtNroID.Text = "00000000";
    }

    private void txtNroID_GotFocus()
    {
        txtNroID.MaxLength = 32767;
        if (string.Equals(cbTipoID.Text, "dni", StringComparison.CurrentCultureIgnoreCase))
            txtNroID.MaxLength = 8;
        if (string.Equals(cbTipoID.Text, "ruc", StringComparison.CurrentCultureIgnoreCase))
            txtNroID.MaxLength = 11;
    }

    private async Task txtBuscarCodigo_KeyPress(KeyPressEventArgs e)
    {
        if (e.KeyChar != (char)Keys.Enter) return;
        e.Handled = true;
        dgvListado.Rows.Clear();
        if (!int.TryParse(txtBuscarCodigo.Text, NumberStyles.Integer, CultureInfo.InvariantCulture, out var codigo)) return;
        var cliente = await Mediator.Send(new GetClienteByIdQuery(codigo));
        if (cliente is not null)
            BindClientes([cliente]);
    }

    private void ClearDetails()
    {
        txtCodigo.Clear();
        txtNombre.Clear();
        cbTipoID.SelectedIndex = -1;
        txtNroID.Clear();
        txtTelefono.Clear();
        txtFax.Clear();
        txtEmail.Clear();
        txtRepresentante.Clear();
        dgvDireccion.Rows.Clear();
    }

    private void SetDetailInputsEnabled(bool enabled)
    {
        txtCodigo.Enabled = enabled;
        txtNombre.Enabled = enabled;
        cbTipoID.Enabled = enabled;
        txtNroID.Enabled = enabled;
        txtTelefono.Enabled = enabled;
        txtFax.Enabled = enabled;
        txtEmail.Enabled = enabled;
        txtRepresentante.Enabled = enabled;
        dgvDireccion.Enabled = enabled;
    }

    private IReadOnlyList<DireccionDto> ReadDirecciones()
    {
        var direcciones = new List<DireccionDto>();
        foreach (DataGridViewRow row in dgvDireccion.Rows)
        {
            if (row.IsNewRow) continue;
            var calle = Convert.ToString(row.Cells[0].Value, CultureInfo.CurrentCulture) ?? string.Empty;
            var ciudad = Convert.ToString(row.Cells[1].Value, CultureInfo.CurrentCulture) ?? string.Empty;
            var provincia = Convert.ToString(row.Cells[2].Value, CultureInfo.CurrentCulture) ?? string.Empty;
            if (string.IsNullOrWhiteSpace(calle) && string.IsNullOrWhiteSpace(ciudad) && string.IsNullOrWhiteSpace(provincia)) continue;
            direcciones.Add(new DireccionDto(calle, ciudad, provincia));
        }
        return direcciones;
    }

    private void BindClientes(IEnumerable<ClienteDto> clientes)
    {
        dgvListado.Rows.Clear();
        foreach (var cliente in clientes)
        {
            var rowIndex = dgvListado.Rows.Add();
            var row = dgvListado.Rows[rowIndex];
            row.Cells[0].Value = cliente.Codigo;
            row.Cells[1].Value = cliente.RazonSocial;
            row.Cells[2].Value = GetTipoIdentificacionDescripcion(cliente.TipoIdCodigo);
            row.Cells[3].Value = cliente.NroIdentificacion;
        }
    }

    private string GetTipoIdentificacionDescripcion(int codigo) =>
        _tiposIdentificacion.FirstOrDefault(tipo => tipo.Codigo == codigo)?.Descripcion ?? string.Empty;

    private bool ShowFailure(string? error, bool showMessages)
    {
        var message = error ?? "No se pudo guardar el cliente.";
        SetStatus(message);
        if (showMessages) MessageBox.Show(message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        return false;
    }

    private void SetStatus(string message) => lblmsg.Text = message;

    private static string? EmptyToNull(string value) => string.IsNullOrWhiteSpace(value) ? null : value.Trim();

    private IMediator Mediator => _mediator
        ?? throw new InvalidOperationException("Use the IMediator constructor at runtime. The parameterless constructor is for the WinForms designer only.");
}
