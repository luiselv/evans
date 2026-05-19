using EVANS.Domain.GuiaRemision;
using FluentAssertions;

namespace EVANS.Domain.Tests.GuiaRemision;

public class PesoTests
{
    [Fact]
    public void Constructor_NegativeValue_Throws()
    {
        Action act = () => new Peso(-0.01m);
        act.Should().Throw<ArgumentOutOfRangeException>();
    }

    [Fact]
    public void Constructor_ZeroValue_IsAccepted()
    {
        var peso = new Peso(0m);
        peso.Valor.Should().Be(0m);
    }

    [Fact]
    public void Constructor_PositiveValue_IsAccepted()
    {
        var peso = new Peso(10.5m);
        peso.Valor.Should().Be(10.5m);
    }
}
