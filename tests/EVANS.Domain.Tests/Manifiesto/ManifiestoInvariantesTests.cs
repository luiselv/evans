using EVANS.Domain.Manifiesto;
using FluentAssertions;
using Agg = EVANS.Domain.Manifiesto.Manifiesto;

namespace EVANS.Domain.Tests.Manifiesto;

public class ManifiestoInvariantesTests
{
    private static IReadOnlyList<int> TwoGuias() => new List<int> { 1, 2 };
    private static IReadOnlyList<int> OneGuia() => new List<int> { 10 };

    private static Agg CrearValido(
        IReadOnlyList<int>? guiaIds = null,
        int transportistaCodigo = 5,
        int vehiculoCodigo = 3,
        int? carretaCodigo = null,
        int choferCodigo = 7,
        decimal importe = 100m,
        decimal peso = 50m,
        int estadoCodigo = 1,
        int usuarioCodigo = 2) =>
        Agg.Crear(
            fecha: DateTime.Today,
            transportistaCodigo: transportistaCodigo,
            vehiculoCodigo: vehiculoCodigo,
            carretaCodigo: carretaCodigo,
            choferCodigo: choferCodigo,
            importe: importe,
            peso: peso,
            estadoCodigo: estadoCodigo,
            usuarioCodigo: usuarioCodigo,
            guiaIds: guiaIds ?? TwoGuias());

    // D-01: happy path
    [Fact]
    public void Crear_ManifiestoValido_RetornaInstancia()
    {
        Agg m = CrearValido(guiaIds: TwoGuias());
        m.Should().NotBeNull();
        m.TransportistaCodigo.Should().Be(5);
        m.VehiculoCodigo.Should().Be(3);
        m.ChoferCodigo.Should().Be(7);
        m.Importe.Should().Be(100m);
        m.GuiaIds.Count.Should().Be(2);
    }

    // D-02: sin guias → MANIFIESTO_SIN_GUIAS
    [Fact]
    public void Crear_SinGuias_LanzaMANIFIESTO_SIN_GUIAS()
    {
        var action = () => CrearValido(guiaIds: new List<int>());
        action.Should()
            .Throw<DomainException>()
            .Which.Code.Should().Be("MANIFIESTO_SIN_GUIAS");
    }

    // D-03: transportista=0 → TRANSPORTISTA_REQUERIDO
    [Fact]
    public void Crear_TransportistaCero_LanzaTRANSPORTISTA_REQUERIDO()
    {
        var action = () => CrearValido(transportistaCodigo: 0);
        action.Should()
            .Throw<DomainException>()
            .Which.Code.Should().Be("TRANSPORTISTA_REQUERIDO");
    }

    // D-04: chofer=0 → CHOFER_REQUERIDO
    [Fact]
    public void Crear_ChoferCero_LanzaCHOFER_REQUERIDO()
    {
        var action = () => CrearValido(choferCodigo: 0);
        action.Should()
            .Throw<DomainException>()
            .Which.Code.Should().Be("CHOFER_REQUERIDO");
    }

    // D-05: vehiculo=0 → VEHICULO_REQUERIDO
    [Fact]
    public void Crear_VehiculoCero_LanzaVEHICULO_REQUERIDO()
    {
        var action = () => CrearValido(vehiculoCodigo: 0);
        action.Should()
            .Throw<DomainException>()
            .Which.Code.Should().Be("VEHICULO_REQUERIDO");
    }

    // D-06: estado=0 → ESTADO_REQUERIDO
    [Fact]
    public void Crear_EstadoCero_LanzaESTADO_REQUERIDO()
    {
        var action = () => CrearValido(estadoCodigo: 0);
        action.Should()
            .Throw<DomainException>()
            .Which.Code.Should().Be("ESTADO_REQUERIDO");
    }

    // D-07: importe negativo → IMPORTE_INVALIDO
    [Fact]
    public void Crear_ImporteNegativo_LanzaIMPORTE_INVALIDO()
    {
        var action = () => CrearValido(importe: -0.01m);
        action.Should()
            .Throw<DomainException>()
            .Which.Code.Should().Be("IMPORTE_INVALIDO");
    }

    // D-08: importe=0 es válido
    [Fact]
    public void Crear_ImporteCero_EsValido()
    {
        var action = () => CrearValido(importe: 0m);
        action.Should().NotThrow();
    }

    // D-09: carreta nula es válida
    [Fact]
    public void Crear_CarretaNula_EsValida()
    {
        var m = CrearValido(carretaCodigo: null);
        m.CarretaCodigo.Should().BeNull();
    }

    // D-14: SetCodigo asigna identidad
    [Fact]
    public void SetCodigo_AsignaIdentidad()
    {
        var m = CrearValido();
        m.SetCodigo(42);
        m.Codigo.Should().Be(42);
    }

    // D-15: lista de guias preservada (read-only)
    [Fact]
    public void Crear_MultiplesGuias_PreservaLista()
    {
        var m = CrearValido(guiaIds: new List<int> { 1, 2, 3 });
        m.GuiaIds.Count.Should().Be(3);
        m.GuiaIds.Should().NotBeNull();
    }
}
