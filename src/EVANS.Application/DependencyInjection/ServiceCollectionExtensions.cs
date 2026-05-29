using EVANS.Application.GuiaRemision.Validators;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace EVANS.Application.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddEvansApplication(this IServiceCollection services)
    {
        if (services.Any(service => service.ServiceType == typeof(EvansApplicationRegistrationMarker)))
            return services;

        services.AddSingleton<EvansApplicationRegistrationMarker>();
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(ServiceCollectionExtensions).Assembly));
        services.AddValidatorsFromAssembly(typeof(ServiceCollectionExtensions).Assembly);
        return services;
    }

    private sealed class EvansApplicationRegistrationMarker;
}
