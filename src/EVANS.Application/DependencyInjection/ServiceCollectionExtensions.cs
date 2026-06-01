using EVANS.Application.GuiaRemision.Validators;
using EVANS.Application.Identidad.Ports;
using EVANS.Application.Identidad.Services;
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
        services.AddSingleton<ICurrentSession, CurrentSession>();
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(ServiceCollectionExtensions).Assembly));
        services.AddValidatorsFromAssembly(typeof(ServiceCollectionExtensions).Assembly);
        return services;
    }

    private sealed class EvansApplicationRegistrationMarker;
}
