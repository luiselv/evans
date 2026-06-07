using System.Globalization;
using EVANS.Application.Catalogo.Commands;
using EVANS.Application.Catalogo.DTOs;
using EVANS.Application.Catalogo.Queries;
using MediatR;

namespace EVANS.UI.WinForms.Catalogo;

public partial class frmMantDestino : Form
{
    private readonly IMediator? _mediator;
    private IReadOnlyList<DestinoDto> _destinos = [];
    private IReadOnlyList<EstadoDto> _estados = [];
    private bool _isCreateMode = true;

    public frmMantDestino()
    {
        InitializeComponent();
    }

    public frmMantDestino(IMediator mediator)
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
    internal string DistanciaText => txtDistancia.Text;
    internal string EstadoText => cbEstado.Text;
    internal string StatusMessage => lblmsg.Text;
    internal int ListCount => lvListado.Items.Count;
    internal string? FirstListName => lvListado.Items.Count == 0 ? null : lvListado.Items[0].SubItems[1].Text;
    internal string? FirstListDistance => lvListado.Items.Count == 0 ? null : lvListado.Items[0].SubItems[2].Text;

    internal void SetTestSearchText(string value) => txtBuscar.Text = value;
    internal void SetTestNombre(string value) => txtNombre.Text = value;
    internal void SetTestDistancia(string value) => txtDistancia.Text = value;
    internal void SetTestEstado(int codigo) => SelectEstado(codigo);
    internal void BeginNewForTest() => btnNuevo_Click();
    internal void BeginEditForTest() => btnEditar_Click();

    internal async Task OpenFirstForTestAsync()
    {
        if (lvListado.Items.Count == 0)
            return;

        await OpenDestinoAsync(int.Parse(lvListado.Items[0].SubItems[0].Text, CultureInfo.InvariantCulture));
    }

    internal async Task LoadDestinosAsync()
    {
        await EnsureEstadosAsync();
        _destinos = await Mediator.Send(new ListDestinosMaintenanceQuery());
        BindDestinos(_destinos);
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

        if (_destinos.Count == 0)
            _destinos = await Mediator.Send(new ListDestinosMaintenanceQuery());

        var search = txtBuscar.Text.Trim();
        BindDestinos(_destinos.Where(destino => destino.Descripcion.StartsWith(search, StringComparison.CurrentCultureIgnoreCase)));
        SetStatus("Los campos marcados con asterisco (*) son obligatorios");
        return true;
    }

    internal async Task<bool> SaveAsync(bool showMessages = false)
    {
        if (string.IsNullOrWhiteSpace(txtNombre.Text) || cbEstado.SelectedValue is null)
        {
            SetStatus("Datos incompletos");
            if (showMessages)
                MessageBox.Show("Datos incompletos", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return false;
        }

        var nombre = txtNombre.Text.ToUpperInvariant();
        var distancia = ParseDistanciaVirtual(txtDistancia.Text);
        var estadoCodigo = Convert.ToInt32(cbEstado.SelectedValue, CultureInfo.InvariantCulture);

        if (_isCreateMode)
        {
            var result = await Mediator.Send(new CreateDestinoCommand(nombre, distancia, estadoCodigo));
            if (!result.IsSuccess)
                return ShowFailure(result.Error, showMessages);
        }
        else
        {
            var result = await Mediator.Send(new UpdateDestinoCommand(
                int.Parse(txtCodigo.Text, CultureInfo.InvariantCulture),
                nombre,
                distancia,
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
        Load += async (_, _) => await LoadEstadosOnlyAsync();
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
        cbEstado.DropDown += async (_, _) => await LoadEstadosOnlyAsync();
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
        if (_destinos.Count == 0)
            _destinos = await Mediator.Send(new ListDestinosMaintenanceQuery());

        BindDestinos(_destinos);
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

        await OpenDestinoAsync(int.Parse(lvListado.SelectedItems[0].SubItems[0].Text, CultureInfo.InvariantCulture));
    }

    private async Task OpenDestinoAsync(int codigo)
    {
        await EnsureEstadosAsync();
        var destino = await Mediator.Send(new GetDestinoByIdQuery(codigo));
        if (destino is null)
            return;

        tabPageDetalles.Enabled = true;
        tabPageListado.Enabled = false;
        tabControl1.SelectTab(tabPageDetalles);

        txtCodigo.Text = destino.Codigo.ToString(CultureInfo.InvariantCulture);
        txtNombre.Text = destino.Descripcion;
        txtDistancia.Text = destino.DistanciaVirtual.ToString(CultureInfo.CurrentCulture);
        SelectEstado(destino.EstadoCodigo);
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
        txtDistancia.Clear();
        cbEstado.SelectedIndex = _estados.Count == 0 ? -1 : 0;
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

        _destinos = await Mediator.Send(new ListDestinosMaintenanceQuery());
        lvListado.Items.Clear();
    }

    private async Task LoadEstadosOnlyAsync()
    {
        await EnsureEstadosAsync();
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

    private void SelectEstado(int codigo)
    {
        if (_estados.Count == 0)
            return;

        cbEstado.SelectedValue = codigo;
    }

    private void SetDetailInputsEnabled(bool enabled)
    {
        txtCodigo.Enabled = enabled;
        txtNombre.Enabled = enabled;
        txtDistancia.Enabled = enabled;
        cbEstado.Enabled = enabled;
    }

    private void BindDestinos(IEnumerable<DestinoDto> destinos)
    {
        lvListado.Items.Clear();
        foreach (var destino in destinos)
        {
            var item = lvListado.Items.Add(destino.Codigo.ToString(CultureInfo.InvariantCulture));
            item.SubItems.Add(destino.Descripcion);
            item.SubItems.Add(destino.DistanciaVirtual.ToString(CultureInfo.CurrentCulture));
        }
    }

    private static double ParseDistanciaVirtual(string value) =>
        string.IsNullOrWhiteSpace(value)
            ? 0
            : double.TryParse(value, NumberStyles.Float, CultureInfo.CurrentCulture, out var result)
                ? result
                : double.TryParse(value, NumberStyles.Float, CultureInfo.InvariantCulture, out result)
                    ? result
                    : 0;

    private bool ShowFailure(string? error, bool showMessages)
    {
        var message = error ?? "No se pudo guardar el destino.";
        SetStatus(message);
        if (showMessages)
            MessageBox.Show(message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        return false;
    }

    private void SetStatus(string message) => lblmsg.Text = message;

    private IMediator Mediator => _mediator
        ?? throw new InvalidOperationException("Use the IMediator constructor at runtime. The parameterless constructor is for the WinForms designer only.");
}
