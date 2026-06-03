using EVANS.Host.WinForms.Shell;
using EVANS.UI.WinForms.Catalogo;
using EVANS.UI.WinForms.Identidad;
using EVANS.UI.WinForms.Reportes;
using Microsoft.Extensions.DependencyInjection;

namespace EVANS.Host.WinForms.Shell;

public static class ShellServiceCollectionExtensions
{
    public static IServiceCollection AddEvansWinFormsShell(
        this IServiceCollection services)
    {
        services.AddTransient<frmLogin>();
        services.AddTransient<frmMantEstado>();
        services.AddTransient<frmConsEnviosMensuales>();
        services.AddTransient<frmConsGuiasPorCliente>();
        services.AddTransient<frmReporteVentas>();
        services.AddSingleton<frmPrincipal>();
        return services;
    }
}
