using System.Net;
using EVANS.Infrastructure.External.Sunat;

namespace EVANS.Infrastructure.Tests.Sunat;

public sealed class ApisNetPeSunatRucServiceTests
{
    [Fact]
    public async Task ConsultarAsync_SuccessResponse_MapsPayload()
    {
        HttpRequestMessage? captured = null;
        var service = BuildService(new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent("""
            {
              "numeroDocumento": "20123456789",
              "razonSocial": "ACME SAC",
              "estado": "ACTIVO",
              "condicion": "HABIDO",
              "direccion": "AV LIMA 123"
            }
            """)
        }, request => captured = request);

        var result = await service.ConsultarAsync("20123456789", CancellationToken.None);

        result.Should().NotBeNull();
        result!.Ruc.Should().Be("20123456789");
        result.RazonSocial.Should().Be("ACME SAC");
        captured.Should().NotBeNull();
        captured!.RequestUri!.PathAndQuery.Should().Be("/v2/sunat/ruc?numero=20123456789");
        captured.Headers.Accept.ToString().Should().Contain("application/json");
    }

    [Fact]
    public async Task ConsultarAsync_NotFound_ReturnsNull()
    {
        var service = BuildService(new HttpResponseMessage(HttpStatusCode.NotFound));

        var result = await service.ConsultarAsync("20123456789", CancellationToken.None);

        result.Should().BeNull();
    }

    private static ApisNetPeSunatRucService BuildService(
        HttpResponseMessage response,
        Action<HttpRequestMessage>? capture = null)
    {
        var handler = new StubHttpMessageHandler(response, capture);
        var client = new HttpClient(handler) { BaseAddress = new Uri("https://api.apis.net.pe") };
        return new ApisNetPeSunatRucService(client, new ApisNetPeSunatRucOptions());
    }

    private sealed class StubHttpMessageHandler(
        HttpResponseMessage response,
        Action<HttpRequestMessage>? capture)
        : HttpMessageHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            capture?.Invoke(request);
            return Task.FromResult(response);
        }
    }
}
