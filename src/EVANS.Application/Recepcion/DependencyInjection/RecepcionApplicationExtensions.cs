using EVANS.Application.Recepcion.Commands;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace EVANS.Application.Recepcion.DependencyInjection;

public static class RecepcionApplicationExtensions
{
    /// <summary>
    /// Registers MediatR handlers and FluentValidation validators for the Recepcion slice.
    /// Call after AddEvansApplication() to extend the handler registration.
    /// </summary>
    public static IServiceCollection AddEvansRecepcionApplication(this IServiceCollection services)
    {
        services.AddMediatR(cfg =>
            cfg.RegisterServicesFromAssemblyContaining<CrearRecepcionCommand>());

        services.AddValidatorsFromAssemblyContaining<CrearRecepcionCommandValidator>();

        return services;
    }
}
