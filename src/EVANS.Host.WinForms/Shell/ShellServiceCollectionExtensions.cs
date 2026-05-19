using EVANS.Host.WinForms.Shell;
using Microsoft.Extensions.DependencyInjection;

namespace EVANS.Host.WinForms.Shell;

public static class ShellServiceCollectionExtensions
{
    public static IServiceCollection AddEvansWinFormsShell(
        this IServiceCollection services)
    {
        return services.AddSingleton<frmPrincipal>();
    }
}
