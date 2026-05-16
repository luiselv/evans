using EVANS.Infrastructure.Sql.Connections;
using Microsoft.Extensions.DependencyInjection;

namespace EVANS.Infrastructure.Sql.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddEvansInfrastructureSql(this IServiceCollection services)
    {
        services.AddSingleton<IEvansMasterConnectionFactory, EvansMasterConnectionFactory>();
        services.AddSingleton<IYearlyTransactionalConnectionFactory, YearlyTransactionalConnectionFactory>();
        return services;
    }
}
