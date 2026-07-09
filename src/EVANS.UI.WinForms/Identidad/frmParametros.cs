using EVANS.Application.Common;
using EVANS.Application.Shared.Commands;
using EVANS.Application.Shared.DTOs;
using EVANS.Application.Shared.Ports;
using MediatR;

namespace EVANS.UI.WinForms.Identidad;

public partial class frmParametros : Form
{
    private readonly IMediator? _mediator;
    private readonly IParametrosService? _parametrosService;
    private ParametrosDto? _current;
    private string _statusMessage = DefaultStatusMessage;

    public frmParametros() => InitializeComponent();

    public frmParametros(IMediator mediator, IParametrosService parametrosService)
    {
        _mediator = mediator;
        _parametrosService = parametrosService;
        InitializeComponent();
        WireEvents();
    }

    internal bool GrabarEnabled => btnGrabar.Enabled;
    internal bool EditarEnabled => btnEditar.Enabled;
    internal bool FieldsReadOnly => TextFields.All(field => field.ReadOnly);
    internal string IgvText => txtIGV.Text;
    internal string FacturaSerieText => txtFacturaSerie.Text;
    internal string FacturaNro1Text => txtFacturaNro1.Text;
    internal string BoletaSerieText => txtBoletaSerie.Text;
    internal string BoletaNro1Text => txtBoletaNro1.Text;
    internal string GuiaRemisionSerieText => txtGRemisionSerie.Text;
    internal string GuiaRemisionNro1Text => txtGRemisionNro1.Text;
    internal string ManifiestoText => txtManifiesto.Text;
    internal string StatusMessage => _statusMessage;

    internal void SetTestIgv(string value) => txtIGV.Text = value;
    internal void SetTestFacturaSerie(string value) => txtFacturaSerie.Text = value;
    internal void SetTestFacturaNro1(string value) => txtFacturaNro1.Text = value;
    internal void SetTestBoletaSerie(string value) => txtBoletaSerie.Text = value;
    internal void SetTestBoletaNro1(string value) => txtBoletaNro1.Text = value;
    internal void SetTestGuiaRemisionSerie(string value) => txtGRemisionSerie.Text = value;
    internal void SetTestGuiaRemisionNro1(string value) => txtGRemisionNro1.Text = value;
    internal void SetTestManifiesto(string value) => txtManifiesto.Text = value;
    internal void BeginEditForTest() => btnEditar_Click();

    internal async Task LoadParametrosAsync()
    {
        _current = await ParametrosService.ObtenerParametrosAsync();
        BindParametros(_current);
        SetFieldsReadOnly(true);
        btnGrabar.Enabled = false;
        btnEditar.Enabled = true;
    }

    internal async Task<bool> SaveAsync(bool showMessages = false)
    {
        if (_current is null)
            _current = await ParametrosService.ObtenerParametrosAsync();

        if (!decimal.TryParse(txtIGV.Text, out var igvRate) || igvRate <= 0)
            return ShowFailure("IGV inválido.", showMessages);

        var updated = _current with
        {
            IgvRate = igvRate,
            FacturaSerie = txtFacturaSerie.Text,
            FacturaNro1 = txtFacturaNro1.Text,
            FacturaNro2 = "0",
            BoletaSerie = txtBoletaSerie.Text,
            BoletaNro1 = txtBoletaNro1.Text,
            BoletaNro2 = "0",
            GuiaRemisionSerie = txtGRemisionSerie.Text,
            GuiaRemisionNro1 = txtGRemisionNro1.Text,
            GuiaRemisionNro2 = "0",
            Manifiesto = txtManifiesto.Text
        };

        var result = await Mediator.Send(new ActualizarParametrosCommand(updated));
        if (!result.IsSuccess)
            return ShowFailure(result.Error ?? "No se pudo guardar los parámetros.", showMessages);

        _current = await ParametrosService.ObtenerParametrosAsync();
        BindParametros(_current);
        SetFieldsReadOnly(true);
        btnGrabar.Enabled = false;
        btnEditar.Enabled = true;
        SetStatus("Los parámetros fueron actualizados.");
        return true;
    }

    private void WireEvents()
    {
        Load += async (_, _) => await LoadParametrosAsync();
        btnEditar.Click += (_, _) => btnEditar_Click();
        btnGrabar.Click += async (_, _) =>
        {
            if (!ConfirmSave())
                return;

            await SaveAsync(showMessages: true);
        };
    }

    private void btnEditar_Click()
    {
        SetFieldsReadOnly(false);
        btnGrabar.Enabled = true;
        btnEditar.Enabled = false;
        txtFacturaSerie.Focus();
    }

    private void BindParametros(ParametrosDto parametros)
    {
        txtIGV.Text = parametros.IgvRate.ToString("0.##");
        txtFacturaSerie.Text = parametros.FacturaSerie;
        txtFacturaNro1.Text = parametros.FacturaNro1;
        txtBoletaSerie.Text = parametros.BoletaSerie;
        txtBoletaNro1.Text = parametros.BoletaNro1;
        txtGRemisionSerie.Text = parametros.GuiaRemisionSerie;
        txtGRemisionNro1.Text = parametros.GuiaRemisionNro1;
        txtManifiesto.Text = parametros.Manifiesto;
        SetStatus(DefaultStatusMessage);
    }

    private void SetFieldsReadOnly(bool readOnly)
    {
        foreach (var field in TextFields)
            field.ReadOnly = readOnly;
    }

    private IEnumerable<TextBox> TextFields =>
    [
        txtFacturaSerie,
        txtFacturaNro1,
        txtBoletaSerie,
        txtBoletaNro1,
        txtIGV,
        txtGRemisionSerie,
        txtGRemisionNro1,
        txtManifiesto
    ];

    private bool ConfirmSave() =>
        MessageBox.Show(
            "La modificación involuntaria de estos parámetros podria afectar el buen funcionamiento del sistema." +
            Environment.NewLine +
            "¿Confirma que desea continuar?",
            "Precaución",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Exclamation) == DialogResult.Yes;

    private bool ShowFailure(string message, bool showMessages)
    {
        SetStatus(message);
        if (showMessages)
            MessageBox.Show(message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        return false;
    }

    private void SetStatus(string message) => _statusMessage = message;

    private const string DefaultStatusMessage = "La modificación involuntaria de estos parámetros podria afectar el buen funcionamiento del sistema.";

    private IMediator Mediator => _mediator
        ?? throw new InvalidOperationException("Use the IMediator constructor at runtime. The parameterless constructor is for the WinForms designer only.");

    private IParametrosService ParametrosService => _parametrosService
        ?? throw new InvalidOperationException("Use the IParametrosService constructor at runtime. The parameterless constructor is for the WinForms designer only.");
}
