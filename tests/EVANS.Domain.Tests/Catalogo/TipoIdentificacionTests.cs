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
    public void Materializar_UnknownCodigo_ThrowsDomainException()
    {
        Action act = () => TipoIdentificacion.Materializar(99, "OTRO");

        act.Should().Throw<DomainException>().Where(e => e.Code == "CAT-TID-002");
    }
}
