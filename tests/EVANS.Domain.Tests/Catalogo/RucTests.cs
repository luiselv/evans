using EVANS.Domain.Shared;
using FluentAssertions;

namespace EVANS.Domain.Tests.Catalogo;

public class RucTests
{
    [Fact]
    public void TryCreate_ValidRuc_ReturnsTrueAndPreservesValue()
    {
        var result = Ruc.TryCreate("20123456789", out var ruc);

        result.Should().BeTrue();
        ruc.Value.Should().Be("20123456789");
        ruc.ToString().Should().Be("20123456789");
    }

    [Fact]
    public void TryCreate_RucWithLeadingZero_ReturnsTrueAndPreservesLeadingZero()
    {
        var result = Ruc.TryCreate("00123456789", out var ruc);

        result.Should().BeTrue();
        ruc.Value.Should().Be("00123456789");
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("2012345678")]
    [InlineData("201234567890")]
    [InlineData("2012345678X")]
    public void TryCreate_InvalidRuc_ReturnsFalse(string? value)
    {
        var result = Ruc.TryCreate(value, out var ruc);

        result.Should().BeFalse();
        ruc.Should().Be(default(Ruc));
    }

    [Fact]
    public void Parse_InvalidRuc_ThrowsArgumentException()
    {
        Action act = () => Ruc.Parse("123");

        act.Should().Throw<ArgumentException>()
            .WithMessage("Invalid RUC*");
    }
}
