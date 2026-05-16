using EVANS.Application.DependencyInjection;
using EVANS.Domain.DependencyInjection;
using EVANS.Host.WinForms.Shell;
using EVANS.Infrastructure.Sql.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using WinFormsApp = System.Windows.Forms.Application;
using GenericHost = Microsoft.Extensions.Hosting.Host;

namespace EVANS.Host.WinForms;

static class Program
{
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
                services.AddEvansInfrastructureSql();
                services.AddSingleton<frmPrincipal>();
            })
            .Build();

        host.Start();

        var mainForm = host.Services.GetRequiredService<frmPrincipal>();
        WinFormsApp.Run(mainForm);
    }
}
