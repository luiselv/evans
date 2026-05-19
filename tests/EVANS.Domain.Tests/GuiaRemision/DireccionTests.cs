using EVANS.Domain.GuiaRemision;
using FluentAssertions;

namespace EVANS.Domain.Tests.GuiaRemision;

public class DireccionTests
{
    [Fact]
    public void Serialize_ThenParse_RoundTrip()
    {
        var original = new Direccion("Av Lima 123", "Lima", "Lima");
        var parsed = Direccion.Parse(original.Serialize());
        parsed.Should().Be(original);
    }

    [Fact]
    public void Parse_TwoSegments_FillsMissingWithEmpty()
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
        var dir = new Direccion("Calle 1", "Arequipa", "Arequipa");
        dir.Serialize().Should().Be("Calle 1|Arequipa|Arequipa");
    }

    [Fact]
    public void ValueEquality_SameValues_AreEqual()
    {
        var a = new Direccion("Av Lima", "Lima", "Lima");
        var b = new Direccion("Av Lima", "Lima", "Lima");
        a.Should().Be(b);
    }
}
