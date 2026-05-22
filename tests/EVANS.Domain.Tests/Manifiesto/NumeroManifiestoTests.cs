using EVANS.Domain.Manifiesto;
using FluentAssertions;

namespace EVANS.Domain.Tests.Manifiesto;

public class NumeroManifiestoTests
{
    // D-12: valid format "2024-1"
    [Fact]
    public void Constructor_ValidFormat_2024_1_DoesNotThrow()
    {
        var action = () => new NumeroManifiesto("2024-1");
        action.Should().NotThrow();
    }

    // D-13: valid format "2024-999"
    [Fact]
    public void Constructor_ValidFormat_2024_999_DoesNotThrow()
    {
        var action = () => new NumeroManifiesto("2024-999");
        action.Should().NotThrow();
    }

    // D-10: invalid format — text prefix
    [Fact]
    public void Constructor_InvalidFormat_TextPrefix_ThrowsArgumentException()
    {
        var action = () => new NumeroManifiesto("ABC-123");
        action.Should()
            .Throw<ArgumentException>()
            .WithMessage("*NUMERO_FORMATO_INVALIDO*");
    }

    // D-10: invalid format — no dash
    [Fact]
    public void Constructor_InvalidFormat_NoDash_ThrowsArgumentException()
    {
        var action = () => new NumeroManifiesto("20241");
        action.Should()
            .Throw<ArgumentException>()
            .WithMessage("*NUMERO_FORMATO_INVALIDO*");
    }

    // D-11: empty string
    [Fact]
    public void Constructor_EmptyString_ThrowsArgumentException()
    {
        var action = () => new NumeroManifiesto("");
        action.Should()
            .Throw<ArgumentException>()
            .WithMessage("*NUMERO_FORMATO_INVALIDO*");
    }

    // ToString returns Value
    [Fact]
    public void ToString_ReturnsValue()
    {
        var numero = new NumeroManifiesto("2024-42");
        numero.ToString().Should().Be("2024-42");
    }

    // Record equality
    [Fact]
    public void Records_SameValue_AreEqual()
    {
        var a = new NumeroManifiesto("2024-1");
        var b = new NumeroManifiesto("2024-1");
        a.Should().Be(b);
    }
}
