using EVANS.Application.Comprobante.Ports;
using EVANS.Application.GuiaRemision.Ports;
using EVANS.Application.Manifiesto.Ports;
using EVANS.Application.Shared.Ports;
using EVANS.Infrastructure.Sql.Comprobante;
using EVANS.Infrastructure.Sql.Connections;
using EVANS.Infrastructure.Sql.GuiaRemision;
using EVANS.Infrastructure.Sql.Manifiesto;
using EVANS.Infrastructure.Sql.Shared;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EVANS.Infrastructure.Sql.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddEvansInfrastructureSql(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        _ = configuration;
        services.AddSingleton<IEvansMasterConnectionFactory, EvansMasterConnectionFactory>();
        services.AddSingleton<IYearlyTransactionalConnectionFactory, YearlyTransactionalConnectionFactory>();
        return services;
    }

    /// <summary>
    /// Registers GuiaRemision infrastructure services.
    /// Call after AddEvansInfrastructureSql (which registers the connection factories).
    /// </summary>
    public static IServiceCollection AddEvansGuiaRemision(this IServiceCollection services)
    {
        services.AddTransient<IUnitOfWorkFactory, SqlUnitOfWorkFactory>();
        services.AddTransient<IGuiaRepository, GuiaRepositoryDapper>();
        services.AddTransient<INumeradorService, NumeradorServiceSql>();
        services.AddTransient<IRecepcionVinculadaService, RecepcionVinculadaServiceSql>();
        services.AddTransient<ICatalogosGuiaRepository, CatalogosGuiaRepositoryDapper>();
        return services;
    }

    /// <summary>
    /// Registers Comprobante infrastructure services.
    /// Call after AddEvansInfrastructureSql and after AddEvansGuiaRemision
    /// (reuses the same IUnitOfWorkFactory registration).
    /// </summary>
    public static IServiceCollection AddEvansComprobante(this IServiceCollection services)
    {
        services.AddTransient<IComprobanteRepository, ComprobanteRepositoryDapper>();
        services.AddTransient<INumeradorComprobanteService, NumeradorComprobanteServiceSql>();
        services.AddTransient<IGuiaVinculadaService, GuiaVinculadaServiceSql>();
        services.AddTransient<IParametrosService, ParametrosServiceSql>();
        return services;
    }

    /// <summary>
    /// Registers Manifiesto infrastructure services.
    /// Call after AddEvansInfrastructureSql and after AddEvansComprobante.
    /// IManifiestoDocumentPrinter is NOT registered here — it is wired in the host/Reports layer
    /// via Func&lt;IManifiestoDocumentPrinter&gt; to keep UI and Infrastructure off the Reports reference.
    /// </summary>
    public static IServiceCollection AddEvansManifiesto(this IServiceCollection services)
    {
        services.AddTransient<IManifiestoRepository, ManifiestoRepositoryDapper>();
        services.AddTransient<INumeradorManifiestoService, NumeradorManifiestoServiceSql>();
        services.AddTransient<IGuiasManifiestoService, GuiasManifiestoServiceSql>();
        services.AddTransient<ICatalogosManifiestoRepository, CatalogosManifiestoRepositorySql>();
        return services;
    }
}
