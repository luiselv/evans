using EVANS.Domain.Catalogo;
using FluentAssertions;

namespace EVANS.Domain.Tests.Catalogo;

public class TipoIdentificacionTests
{
    [Theory]
    [InlineData(1, 11)]
    [InlineData(2, 8)]
    public void Materializar_KnownCodigo_ComputesLongitudRequerida(int codigo, int expectedLength)
    {
        var tipo = TipoIdentificacion.Materializar(codigo, "DOC");

        tipo.LongitudRequerida.Should().Be(expectedLength);
    }

    [Fact]
    public void Crear_NewTipoIdentificacion_UsesTransientCodigoAndZeroLength()
    {
        var tipo = TipoIdentificacion.Crear("OTRO");

        tipo.Codigo.Should().Be(0);
        tipo.Descripcion.Should().Be("OTRO");
        tipo.LongitudRequerida.Should().Be(0);
    }

    [Fact]
    public void Materializar_UnknownLegacyCodigo_UsesZeroLength()
    {
        var tipo = TipoIdentificacion.Materializar(99, "OTRO");

        tipo.Codigo.Should().Be(99);
        tipo.LongitudRequerida.Should().Be(0);
    }
}
