using EVANS.Application.Manifiesto.DTOs;
using EVANS.Reports.Manifiesto;
using UglyToad.PdfPig;

namespace EVANS.Reports.Tests.Manifiesto;

[UsesVerify]
public class ManifestoPdfRendererTests
{
    private static readonly ManifestoPdfRenderer Renderer = new();

    private static ManifiestoDto BuildDto(IReadOnlyList<ManifiestoLineaDto>? lineas = null) => new(
        Codigo: 1,
        Numero: "2024-1",
        Fecha: new DateTime(2024, 6, 15),
        TransportistaCodigo: 1,
        TransportistaNombre: "Transportes SA",
        VehiculoCodigo: 1,
        VehiculoPlaca: "ABC-123",
        CarretaCodigo: null,
        CarretaPlaca: null,
        ChoferCodigo: 1,
        ChoferNombre: "Juan Perez",
        Importe: 500.00m,
        Peso: 150m,
        NroGuias: 3,
        EstadoCodigo: 1,
        EstadoNombre: "Activo",
        UsuarioCodigo: 1,
        Lineas: lineas ?? BuildDefaultLineas());

    private static IReadOnlyList<ManifiestoLineaDto> BuildDefaultLineas() =>
    [
        new ManifiestoLineaDto(1, "GR01-000001", 1, "Lima", 50m, 100m),
        new ManifiestoLineaDto(2, "GR01-000002", 2, "Trujillo", 60m, 150m),
        new ManifiestoLineaDto(3, "GR01-000003", 1, "Lima", 40m, 250m),
    ];

    private static IReadOnlyList<ManifiestoLineaDto> BuildMultiDestinationLineas() =>
    [
        new ManifiestoLineaDto(1, "GR01-000001", 1, "Lima", 50m, 100m),
        new ManifiestoLineaDto(2, "GR01-000002", 1, "Lima", 40m, 80m),
        new ManifiestoLineaDto(3, "GR01-000003", 2, "Trujillo", 60m, 150m),
    ];

    // ------------------------------------------------------------------
    // R-01: pdf_render_smoke_no_lanza — non-empty PDF bytes
    // ------------------------------------------------------------------

    [Fact]
    public void Render_RetornaBytesPdf_NoVacios()
    {
        var dto = BuildDto();

        var bytes = Renderer.Render(dto);

        bytes.Should().NotBeNullOrEmpty();
        bytes.Length.Should().BeGreaterThan(1000);
        System.Text.Encoding.ASCII.GetString(bytes[..4]).Should().Be("%PDF");
    }

    // ------------------------------------------------------------------
    // R-02: pdf_agrupa_por_destino — PDF contains destination group headers
    // ------------------------------------------------------------------

    [Fact]
    public void Render_AgrupaPorDestino_EmiteSubtotales()
    {
        var lineas = BuildMultiDestinationLineas();
        var dto = BuildDto(lineas);

        var bytes = Renderer.Render(dto);
        var text = ExtractText(bytes);
        var groups = ManifiestoDestinationGrouper.GroupByDestino(dto.Lineas);

        bytes.Should().NotBeNullOrEmpty();
        text.Should().Contain("Lima");
        text.Should().Contain("Trujillo");
        text.Should().Contain("180");
        text.Should().Contain("150");

        groups.Should().HaveCount(2);

        var lima = groups.Single(g => g.DestinoNombre == "Lima");
        lima.Lineas.Should().HaveCount(2);
        lima.TotalPeso.Should().Be(90m);
        lima.TotalFlete.Should().Be(180m);

        var trujillo = groups.Single(g => g.DestinoNombre == "Trujillo");
        trujillo.Lineas.Should().HaveCount(1);
        trujillo.TotalPeso.Should().Be(60m);
        trujillo.TotalFlete.Should().Be(150m);
    }

    // ------------------------------------------------------------------
    // R-03: snapshot approval — isolated to Manifiesto/ directory
    // ------------------------------------------------------------------

    [Fact]
    public Task Render_Snapshot()
    {
        var text = ExtractText(Renderer.Render(BuildDto()));
        return Verify(text)
            .UseDirectory("Snapshots")
            .UseMethodName(nameof(Render_Snapshot));
    }

    // ------------------------------------------------------------------
    // Helpers
    // ------------------------------------------------------------------

    private static string ExtractText(byte[] pdf)
    {
        using var doc = PdfDocument.Open(pdf);
        var pages = doc.GetPages().Select((p, i) =>
            $"=== Page {i + 1} ===\n{p.Text.Trim()}");
        return string.Join("\n\n", pages);
    }
}
