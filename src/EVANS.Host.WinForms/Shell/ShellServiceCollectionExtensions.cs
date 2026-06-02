using EVANS.Host.WinForms.Shell;
using EVANS.UI.WinForms.Identidad;
using Microsoft.Extensions.DependencyInjection;

namespace EVANS.Host.WinForms.Shell;

public static class ShellServiceCollectionExtensions
{
    public static IServiceCollection AddEvansWinFormsShell(
        this IServiceCollection services)
    {
        services.AddTransient<frmLogin>();
        services.AddSingleton<frmPrincipal>();
        return services;
    }
}
