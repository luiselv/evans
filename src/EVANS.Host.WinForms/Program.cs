using EVANS.Application.DependencyInjection;
using EVANS.Domain.DependencyInjection;
using EVANS.Host.WinForms.Shell;
using EVANS.Infrastructure.External.DependencyInjection;
using EVANS.Infrastructure.Sql.DependencyInjection;
using EVANS.Reports.DependencyInjection;
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
                services.AddEvansInfrastructureExternal(ctx.Configuration);
                services.AddEvansReports();
                services.AddEvansWinFormsShell();
            })
            .Build();

        host.Start();
        Services = host.Services;

        var mainForm = host.Services.GetRequiredService<frmPrincipal>();
        WinFormsApp.Run(mainForm);
    }
}
