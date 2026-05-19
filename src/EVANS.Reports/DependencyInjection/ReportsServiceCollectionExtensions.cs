using EVANS.Reports.Comprobante;
using EVANS.Reports.GuiaRemision;
using Microsoft.Extensions.DependencyInjection;

namespace EVANS.Reports.DependencyInjection;

public static class ReportsServiceCollectionExtensions
{
    public static IServiceCollection AddEvansReports(this IServiceCollection services)
    {
        services.AddSingleton<GuiaPdfRenderer>();

        // Comprobante renderers and factory
        services.AddSingleton<BoletaPdfRenderer>();
        services.AddSingleton<FacturaPdfRenderer>();
        services.AddSingleton<DocumentPrinterFactory>();

        return services;
    }
}
