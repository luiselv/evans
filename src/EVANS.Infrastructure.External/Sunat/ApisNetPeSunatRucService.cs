using System.Net.Http.Headers;
using System.Text.Json;
using EVANS.Application.Identidad.DTOs;
using EVANS.Application.Identidad.Ports;

namespace EVANS.Infrastructure.External.Sunat;

public sealed class ApisNetPeSunatRucService : ISunatRucService
{
    private readonly HttpClient _httpClient;
    private readonly ApisNetPeSunatRucOptions _options;

    public ApisNetPeSunatRucService(
        HttpClient httpClient,
        ApisNetPeSunatRucOptions options)
    {
        _httpClient = httpClient;
        _options = options;

        _httpClient.BaseAddress ??= new Uri(_options.BaseUrl);
        _httpClient.Timeout = _options.Timeout;
    }

    public async Task<SunatRucDto?> ConsultarAsync(
        string ruc,
        CancellationToken cancellationToken = default)
    {
        using var request = new HttpRequestMessage(
            HttpMethod.Get,
            $"{_options.EndpointPath}?numero={Uri.EscapeDataString(ruc)}");

        request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        request.Headers.Referrer = new Uri(_options.Referer);

        if (!string.IsNullOrWhiteSpace(_options.Token))
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _options.Token);

        try
        {
            using var response = await _httpClient.SendAsync(request, cancellationToken);
            if (!response.IsSuccessStatusCode)
                return null;

            await using var stream = await response.Content.ReadAsStreamAsync(cancellationToken);
            var payload = await JsonSerializer.DeserializeAsync<ApisNetPeRucResponse>(
                stream,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true },
                cancellationToken);

            if (payload is null || string.IsNullOrWhiteSpace(payload.NumeroDocumento))
                return null;

            return new SunatRucDto(
                payload.NumeroDocumento,
                payload.RazonSocial ?? string.Empty,
                payload.Estado ?? string.Empty,
                payload.Condicion ?? string.Empty,
                payload.Direccion ?? string.Empty);
        }
        catch (OperationCanceledException) when (!cancellationToken.IsCancellationRequested)
        {
            return null;
        }
        catch (HttpRequestException)
        {
            return null;
        }
        catch (JsonException)
        {
            return null;
        }
    }

    private sealed record ApisNetPeRucResponse(
        string? NumeroDocumento,
        string? RazonSocial,
        string? Estado,
        string? Condicion,
        string? Direccion);
}
