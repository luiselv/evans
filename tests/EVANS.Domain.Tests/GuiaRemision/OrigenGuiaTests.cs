using EVANS.Domain.GuiaRemision;
using FluentAssertions;

namespace EVANS.Domain.Tests.GuiaRemision;

public class OrigenGuiaTests
{
    [Fact]
    public void Standalone_PatternMatch_Succeeds()
    {
        OrigenGuia origen = new Standalone();
        var isStandalone = origen is Standalone;
        isStandalone.Should().BeTrue();
    }

    [Fact]
    public void DesdeRecepcion_CarriesRecepcionId()
    {
        OrigenGuia origen = new DesdeRecepcion(42);
        origen.Should().BeOfType<DesdeRecepcion>();
        ((DesdeRecepcion)origen).RecepcionId.Should().Be(42);
    }

    [Fact]
    public void ExhaustiveSwitch_CompilesAndCovers()
    {
        OrigenGuia[] origenes = [new Standalone(), new DesdeRecepcion(1)];

        foreach (var origen in origenes)
        {
            var label = origen switch
            {
                Standalone => "standalone",
                DesdeRecepcion d => $"recepcion-{d.RecepcionId}",
                _ => throw new InvalidOperationException("Unknown type")
            };
            label.Should().NotBeNullOrEmpty();
        }
    }
}
