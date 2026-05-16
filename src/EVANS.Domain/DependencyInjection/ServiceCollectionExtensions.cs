using Microsoft.Extensions.DependencyInjection;

namespace EVANS.Domain.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddEvansDomain(this IServiceCollection services)
    {
        // Domain has no external dependencies — placeholder for future domain services
        return services;
    }
}
