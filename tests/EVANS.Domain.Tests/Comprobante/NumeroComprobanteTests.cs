using EVANS.Domain.Comprobante;
using FluentAssertions;

namespace EVANS.Domain.Tests.Comprobante;

public class NumeroComprobanteTests
{
    // --- Serie format: ^[A-Z]\d{3}$ ---

    [Theory]
    [InlineData("F001")]
    [InlineData("B001")]
    [InlineData("A999")]
    public void Constructor_ValidSerie_DoesNotThrow(string serie)
    {
        var action = () => new NumeroComprobante(serie, "000042");
        action.Should().NotThrow();
    }

    [Theory]
    [InlineData("1ABC")]    // starts with digit
    [InlineData("FA01")]    // two letters
    [InlineData("F0001")]   // too long
    [InlineData("F01")]     // too short (letter + 2 digits)
    [InlineData("f001")]    // lowercase
    [InlineData("")]
    [InlineData(null)]
    public void Constructor_InvalidSerie_ThrowsWithCode(string? serie)
    {
        var action = () => new NumeroComprobante(serie!, "000042");
        action.Should()
            .Throw<ArgumentException>()
            .WithMessage("*SERIE_FORMATO_INVALIDO*");
    }

    // --- Numero format: ^\d{6}$ ---

    [Theory]
    [InlineData("000042")]
    [InlineData("000001")]
    [InlineData("999999")]
    public void Constructor_ValidNumero_DoesNotThrow(string numero)
    {
        var action = () => new NumeroComprobante("F001", numero);
        action.Should().NotThrow();
    }

    [Theory]
    [InlineData("42")]        // too short
    [InlineData("0000042")]   // too long
    [InlineData("A00042")]    // contains letter
    [InlineData("")]
    [InlineData(null)]
    public void Constructor_InvalidNumero_ThrowsWithCode(string? numero)
    {
        var action = () => new NumeroComprobante("F001", numero!);
        action.Should()
            .Throw<ArgumentException>()
            .WithMessage("*NUMERO_FORMATO_INVALIDO*");
    }

    // --- Immutability (record equality) ---

    [Fact]
    public void Records_SameValues_AreEqual()
    {
        var a = new NumeroComprobante("F001", "000042");
        var b = new NumeroComprobante("F001", "000042");
        a.Should().Be(b);
    }

    [Fact]
    public void Records_DifferentValues_AreNotEqual()
    {
        var a = new NumeroComprobante("F001", "000042");
        var b = new NumeroComprobante("B001", "000001");
        a.Should().NotBe(b);
    }

    [Fact]
    public void ToString_ReturnsDashSeparated()
    {
        var numero = new NumeroComprobante("F001", "000042");
        numero.ToString().Should().Be("F001-000042");
    }
}
