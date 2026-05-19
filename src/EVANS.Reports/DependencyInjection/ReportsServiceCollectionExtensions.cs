using EVANS.Reports.GuiaRemision;
using Microsoft.Extensions.DependencyInjection;

namespace EVANS.Reports.DependencyInjection;

public static class ReportsServiceCollectionExtensions
{
    public static IServiceCollection AddEvansReports(this IServiceCollection services)
    {
        services.AddSingleton<GuiaPdfRenderer>();
        return services;
    }
}
