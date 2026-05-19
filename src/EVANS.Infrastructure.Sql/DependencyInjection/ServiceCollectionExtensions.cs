using EVANS.Application.GuiaRemision.Ports;
using EVANS.Infrastructure.Sql.Connections;
using EVANS.Infrastructure.Sql.GuiaRemision;
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
}
