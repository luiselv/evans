using EVANS.Application.Identidad.Ports;
using EVANS.Application.Reportes.Ports;
using EVANS.Infrastructure.External.Excel;
using EVANS.Infrastructure.External.Sunat;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EVANS.Infrastructure.External.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddEvansInfrastructureExternal(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var section = configuration.GetSection("SunatRuc");
        var options = new ApisNetPeSunatRucOptions
        {
            BaseUrl = section["BaseUrl"] ?? "https://api.apis.net.pe",
            EndpointPath = section["EndpointPath"] ?? "/v2/sunat/ruc",
            Token = section["Token"],
            Referer = section["Referer"] ?? "https://apis.net.pe/api-consulta-ruc"
        };

        services.AddSingleton(options);
        services.AddSingleton<ISunatRucService>(sp =>
            new ApisNetPeSunatRucService(new HttpClient(), sp.GetRequiredService<ApisNetPeSunatRucOptions>()));
        services.AddSingleton<IEnviosMensualesExcelExporter, EnviosMensualesExcelExporter>();
        services.AddSingleton<IReporteVentasExcelExporter, ReporteVentasExcelExporter>();

        return services;
    }
}
