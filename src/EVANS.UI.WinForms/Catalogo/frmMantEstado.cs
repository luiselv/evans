using EVANS.Application.Catalogo.Commands;
using EVANS.Application.Catalogo.DTOs;
using EVANS.Application.Catalogo.Queries;
using MediatR;

namespace EVANS.UI.WinForms.Catalogo;

public partial class frmMantEstado : Form
{
    private readonly IMediator _mediator;
    private IReadOnlyList<EstadoDto> _estados = [];
    private bool _isCreateMode = true;

    public frmMantEstado(IMediator mediator)
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
    internal string DescripcionText => txtDescripcion.Text;
    internal string StatusMessage => lblmsg.Text;
    internal int ListCount => lvListado.Items.Count;
    internal string? FirstListDescription => lvListado.Items.Count == 0 ? null : lvListado.Items[0].SubItems[1].Text;

    internal void SetTestSearchText(string value) => txtBuscar.Text = value;
    internal void SetTestDescripcion(string value) => txtDescripcion.Text = value;
    internal void BeginNewForTest() => btnNuevo_Click();
    internal void BeginEditForTest() => btnEditar_Click();
    internal async Task OpenFirstForTestAsync()
    {
        if (lvListado.Items.Count == 0)
            return;

        await OpenEstadoAsync(int.Parse(lvListado.Items[0].SubItems[0].Text));
    }

    internal async Task LoadEstadosAsync()
    {
        _estados = await _mediator.Send(new ListEstadosQuery());
        BindEstados(_estados);
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

        if (_estados.Count == 0)
            _estados = await _mediator.Send(new ListEstadosQuery());

        var search = txtBuscar.Text.Trim();
        BindEstados(_estados.Where(estado => estado.Descripcion.StartsWith(search, StringComparison.CurrentCultureIgnoreCase)));
        SetStatus("Los campos marcados con asterisco (*) son obligatorios");
        return true;
    }

    internal async Task<bool> SaveAsync(bool showMessages = false)
    {
        if (string.IsNullOrWhiteSpace(txtDescripcion.Text))
        {
            SetStatus("Datos incompletos");
            if (showMessages)
                MessageBox.Show("Datos incompletos", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return false;
        }

        var descripcion = txtDescripcion.Text.ToUpperInvariant();
        if (_isCreateMode)
        {
            var result = await _mediator.Send(new CreateEstadoCommand(descripcion));
            if (!result.IsSuccess)
                return ShowFailure(result.Error, showMessages);
        }
        else
        {
            var result = await _mediator.Send(new UpdateEstadoCommand(int.Parse(txtCodigo.Text), descripcion));
            if (!result.IsSuccess)
                return ShowFailure(result.Error, showMessages);
            _isCreateMode = true;
        }

        await ReturnToListingAsync();
        return true;
    }

    private void WireEvents()
    {
        Load += async (_, _) => await LoadEstadosAsync();
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
        if (_estados.Count == 0)
            _estados = await _mediator.Send(new ListEstadosQuery());

        BindEstados(_estados);
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

        await OpenEstadoAsync(int.Parse(lvListado.SelectedItems[0].SubItems[0].Text));
    }

    private async Task OpenEstadoAsync(int codigo)
    {
        var estado = await _mediator.Send(new GetEstadoByIdQuery(codigo));
        if (estado is null)
            return;

        tabPageDetalles.Enabled = true;
        tabPageListado.Enabled = false;
        tabControl1.SelectTab(tabPageDetalles);

        txtCodigo.Text = estado.Codigo.ToString();
        txtDescripcion.Text = estado.Descripcion;
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
        txtDescripcion.Clear();
        SetDetailInputsEnabled(true);
        txtCodigo.ReadOnly = true;
        txtDescripcion.Focus();

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
        txtDescripcion.Focus();

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

        _estados = await _mediator.Send(new ListEstadosQuery());
        lvListado.Items.Clear();
    }

    private void SetDetailInputsEnabled(bool enabled)
    {
        txtDescripcion.Enabled = enabled;
        txtCodigo.Enabled = enabled;
    }

    private void BindEstados(IEnumerable<EstadoDto> estados)
    {
        lvListado.Items.Clear();
        foreach (var estado in estados)
        {
            var item = lvListado.Items.Add(estado.Codigo.ToString());
            item.SubItems.Add(estado.Descripcion);
        }
    }

    private bool ShowFailure(string? error, bool showMessages)
    {
        var message = error ?? "No se pudo guardar el estado.";
        SetStatus(message);
        if (showMessages)
            MessageBox.Show(message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        return false;
    }

    private void SetStatus(string message) => lblmsg.Text = message;
}
