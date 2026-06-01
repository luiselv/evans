namespace EVANS.Infrastructure.External.Sunat;

public sealed class ApisNetPeSunatRucOptions
{
    public string BaseUrl { get; init; } = "https://api.apis.net.pe";
    public string EndpointPath { get; init; } = "/v2/sunat/ruc";
    public string? Token { get; init; }
    public string Referer { get; init; } = "https://apis.net.pe/api-consulta-ruc";
    public TimeSpan Timeout { get; init; } = TimeSpan.FromSeconds(5);
}
