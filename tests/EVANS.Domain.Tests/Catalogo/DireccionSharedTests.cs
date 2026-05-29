using EVANS.Domain.Shared;
using FluentAssertions;

namespace EVANS.Domain.Tests.Catalogo;

public class DireccionSharedTests
{
    [Fact]
    public void Serialize_ThenParse_RoundTripsValue()
    {
        var original = new Direccion("Av Lima 123", "Lima", "Lima");

        var parsed = Direccion.Parse(original.Serialize());

        parsed.Should().Be(original);
    }

    [Fact]
    public void Parse_TwoSegments_FillsProvinciaWithEmpty()
    {
        var result = Direccion.Parse("Av Lima|Lima");

        result.Calle.Should().Be("Av Lima");
        result.Ciudad.Should().Be("Lima");
        result.Provincia.Should().Be("");
    }

    [Fact]
    public void Parse_EmptyString_ReturnsEmpty()
    {
        var result = Direccion.Parse("");

        result.Should().Be(Direccion.Empty);
    }

    [Fact]
    public void Serialize_IsStable()
    {
        var direccion = new Direccion("Calle 1", "Arequipa", "Arequipa");

        direccion.Serialize().Should().Be("Calle 1|Arequipa|Arequipa");
    }
}
