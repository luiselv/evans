using EVANS.Application.DependencyInjection;
using EVANS.Application.Identidad.Ports;
using EVANS.Application.Manifiesto.Ports;
using EVANS.Application.Shared.Ports;
using EVANS.Domain.DependencyInjection;
using EVANS.Host.WinForms.Shell;
using EVANS.Infrastructure.External.DependencyInjection;
using EVANS.Infrastructure.Sql.DependencyInjection;
using EVANS.Reports.DependencyInjection;
using EVANS.Reports.Manifiesto;
using EVANS.UI.WinForms.Identidad;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using WinFormsApp = System.Windows.Forms.Application;
using GenericHost = Microsoft.Extensions.Hosting.Host;

namespace EVANS.Host.WinForms;

static class Program
{
    /// <summary>
    /// The host service provider — exposed for forms that need service location
    /// as a last resort (e.g. resolving DocumentPrinterFactory without injecting Reports into UI.WinForms).
    /// </summary>
    internal static IServiceProvider Services { get; private set; } = null!;

    [STAThread]
    static void Main()
    {
        WinFormsApp.EnableVisualStyles();
        WinFormsApp.SetCompatibleTextRenderingDefault(false);

        using var host = GenericHost.CreateDefaultBuilder()
            .ConfigureServices((ctx, services) =>
            {
                services.AddEvansDomain();
                services.AddEvansApplication();
                services.AddEvansInfrastructureSql(ctx.Configuration);
                services.AddEvansGuiaRemision();
                services.AddEvansComprobante();
                services.AddEvansManifiesto();
                services.AddEvansRecepcion();
                services.AddEvansIdentidad();
                services.AddEvansReportesConsultas();
                services.AddEvansInfrastructureExternal(ctx.Configuration);
                services.AddEvansReports();

                // Wire IManifiestoDocumentPrinter factory — keeps UI.WinForms off EVANS.Reports reference.
                services.AddSingleton<ManifestoPdfRenderer>();
                services.AddSingleton<Func<IManifiestoDocumentPrinter>>(sp =>
                {
                    var renderer = sp.GetRequiredService<ManifestoPdfRenderer>();
                    return DocumentPrinterManifiestoFactory.CreateFactory(renderer);
                });
                services.AddEvansWinFormsShell();
            })
            .Build();

        host.Start();
        Services = host.Services;

        using var loginForm = host.Services.GetRequiredService<frmLogin>();
        if (loginForm.ShowDialog() != DialogResult.OK)
            return;

        var parametrosService = host.Services.GetRequiredService<IParametrosService>();
        var parametros = parametrosService.ObtenerParametrosAsync().GetAwaiter().GetResult();

        var currentSession = host.Services.GetRequiredService<ICurrentSession>();
        currentSession.Start(loginForm.AuthenticatedUser!, parametros, DateTime.Today.Year);

        var mainForm = host.Services.GetRequiredService<frmPrincipal>();
        mainForm.Text = $"{mainForm.Text} | Usuario: {currentSession.Current!.Usuario.NombreCompleto}";

        WinFormsApp.Run(mainForm);
    }
}
