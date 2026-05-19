using EVANS.Domain.GuiaRemision;
using FluentAssertions;

namespace EVANS.Domain.Tests.GuiaRemision;

public class NumeroGuiaTests
{
    [Fact]
    public void Constructor_SerieLength4_IsValid()
    {
        var numero = new NumeroGuia("T001", 1);
        numero.Serie.Should().Be("T001");
    }

    [Fact]
    public void Constructor_PositiveNumber_IsValid()
    {
        var numero = new NumeroGuia("T001", 123);
        numero.Numero.Should().Be(123);
    }

    [Fact]
    public void ToString_FormatsWithSixDigitPadding()
    {
        var numero = new NumeroGuia("T001", 123);
        numero.ToString().Should().Be("T001-000123");
    }

    [Fact]
    public void Parse_RoundTrip_ReturnsEquivalent()
    {
        var original = new NumeroGuia("T001", 456);
        var parsed = NumeroGuia.Parse(original.ToString());
        parsed.Should().Be(original);
    }

    [Fact]
    public void Constructor_SerieNotLength4_Throws()
    {
        Action act = () => new NumeroGuia("TO", 1);
        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void Constructor_NegativeNumber_Throws()
    {
        Action act = () => new NumeroGuia("T001", -1);
        act.Should().Throw<ArgumentException>();
    }
}
