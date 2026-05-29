using EVANS.Application.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

namespace EVANS.Application.Catalogo.DependencyInjection;

public static class CatalogoApplicationExtensions
{
    public static IServiceCollection AddEvansCatalogoApplication(this IServiceCollection services)
    {
        return services.AddEvansApplication();
    }
}
