using FluentAssertions;
using Xunit;
using Agg = EVANS.Domain.Recepcion.Recepcion;
using Det = EVANS.Domain.Recepcion.DetalleRecepcion;
using DomEx = EVANS.Domain.Recepcion.DomainException;
using TipoDir = EVANS.Domain.Recepcion.TipoDireccion;

namespace EVANS.Domain.Tests.Recepcion;

public class RecepcionTests
{
    private static IReadOnlyList<Det> OneDetalle() =>
        new[] { Det.Crear(1m, "Carga", 5m, "KG", 50m, "GR", "001") };

    private static Agg ValidRecepcion(IReadOnlyList<Det>? detalles = null) =>
        Agg.Crear(
            fechaEmision: DateTime.Today,
            remitenteId: 1,
            tipoDirPartida: TipoDir.Agencia,
            direccionPartida: "Agencia Lima",
            destinatarioId: 2,
            tipoDirDestino: TipoDir.DireccionCliente,
            direccionDestino: "Av Arequipa 123",
            destinoId: 1,
            estadoId: 1,
            bultos: null,
            pesoTotal: null,
            costoTotal: 100m,
            observacion: null,
            usuarioId: 1,
            detalles: detalles ?? OneDetalle());

    // D-01: Crear succeeds with valid data
    [Fact]
    public void Crear_ConDatosValidos_RetornaAggregate()
    {
        var recepcion = ValidRecepcion();

        recepcion.Codigo.Should().Be(0);
        recepcion.Detalles.Should().HaveCount(1);
        recepcion.RemitenteId.Should().Be(1);
    }

    // D-02: REC005 — empty detalles
    [Fact]
    public void Crear_DetallesVacios_ThrowsREC005()
    {
        var act = () => ValidRecepcion(detalles: Array.Empty<Det>());

        act.Should().Throw<DomEx>().Which.Code.Should().Be("REC005");
    }

    // D-03: REC001 — RemitenteId zero
    [Fact]
    public void Crear_RemitenteIdCero_ThrowsREC001()
    {
        var act = () => Agg.Crear(
            DateTime.Today, 0, TipoDir.Agencia, "Dir",
            2, TipoDir.Agencia, "Dir2",
            1, 1, null, null, 100m, null, 1, OneDetalle());

        act.Should().Throw<DomEx>().Which.Code.Should().Be("REC001");
    }

    // D-04: REC003 — DestinoId zero
    [Fact]
    public void Crear_DestinoIdCero_ThrowsREC003()
    {
        var act = () => Agg.Crear(
            DateTime.Today, 1, TipoDir.Agencia, "Dir",
            2, TipoDir.Agencia, "Dir2",
            0, 1, null, null, 100m, null, 1, OneDetalle());

        act.Should().Throw<DomEx>().Which.Code.Should().Be("REC003");
    }

    // D-05: REC004 — EstadoId zero
    [Fact]
    public void Crear_EstadoIdCero_ThrowsREC004()
    {
        var act = () => Agg.Crear(
            DateTime.Today, 1, TipoDir.Agencia, "Dir",
            2, TipoDir.Agencia, "Dir2",
            1, 0, null, null, 100m, null, 1, OneDetalle());

        act.Should().Throw<DomEx>().Which.Code.Should().Be("REC004");
    }

    // D-06: REC006 — default FechaEmision
    [Fact]
    public void Crear_FechaEmisionDefault_ThrowsREC006()
    {
        var act = () => Agg.Crear(
            default, 1, TipoDir.Agencia, "Dir",
            2, TipoDir.Agencia, "Dir2",
            1, 1, null, null, 100m, null, 1, OneDetalle());

        act.Should().Throw<DomEx>().Which.Code.Should().Be("REC006");
    }

    // REC002 — DestinatarioId zero
    [Fact]
    public void Crear_DestinatarioIdCero_ThrowsREC002()
    {
        var act = () => Agg.Crear(
            DateTime.Today, 1, TipoDir.Agencia, "Dir",
            0, TipoDir.Agencia, "Dir2",
            1, 1, null, null, 100m, null, 1, OneDetalle());

        act.Should().Throw<DomEx>().Which.Code.Should().Be("REC002");
    }

    // REC007 — UsuarioId zero
    [Fact]
    public void Crear_UsuarioIdCero_ThrowsREC007()
    {
        var act = () => Agg.Crear(
            DateTime.Today, 1, TipoDir.Agencia, "Dir",
            2, TipoDir.Agencia, "Dir2",
            1, 1, null, null, 100m, null, 0, OneDetalle());

        act.Should().Throw<DomEx>().Which.Code.Should().Be("REC007");
    }

    // D-12: Actualizar replaces detail list entirely
    [Fact]
    public void Actualizar_ReplacesDetalleList()
    {
        var recepcion = ValidRecepcion(OneDetalle());
        recepcion.SetCodigo(1);

        var nuevosDetalles = new[]
        {
            Det.Crear(3m, "Nuevo A", 1m, "UND", 30m, "GR", "002"),
            Det.Crear(1m, "Nuevo B", 2m, "KG", 20m, "GR", "003"),
            Det.Crear(2m, "Nuevo C", 0m, "UND", 10m, "GR", "004"),
        };

        recepcion.Actualizar(
            DateTime.Today, 1, TipoDir.Agencia, "Dir",
            2, TipoDir.Agencia, "Dir2",
            1, 1, null, null, 60m, null, nuevosDetalles);

        recepcion.Detalles.Should().HaveCount(3);
    }

    // AplicarIgvEnDetalles adjusts all detail costs
    [Fact]
    public void AplicarIgvEnDetalles_AjustaTodosLosDetalles()
    {
        var detalles = new[]
        {
            Det.Crear(1m, "Item A", 0m, "UND", 118m, "GR", "001"),
            Det.Crear(1m, "Item B", 0m, "UND", 59m, "GR", "002"),
        };
        var recepcion = ValidRecepcion(detalles);

        recepcion.AplicarIgvEnDetalles(0.18m);

        recepcion.Detalles[0].Costo.Should().Be(100m);
        recepcion.Detalles[1].Costo.Should().BeApproximately(50m, 0.01m);
    }

    // SetCodigo sets Codigo
    [Fact]
    public void SetCodigo_SetsCodigoCorrectly()
    {
        var recepcion = ValidRecepcion();

        recepcion.SetCodigo(42);

        recepcion.Codigo.Should().Be(42);
    }

    // SetGuiaRemisionVinculada sets GuiaRemisionVinculada
    [Fact]
    public void SetGuiaRemisionVinculada_SetsValue()
    {
        var recepcion = ValidRecepcion();

        recepcion.SetGuiaRemisionVinculada("T001-000001");

        recepcion.GuiaRemisionVinculada.Should().Be("T001-000001");
    }

    // Materializar bypasses invariants (for repo hydration)
    [Fact]
    public void Materializar_CreatesInstanceWithoutInvariantCheck()
    {
        var detalles = new[]
        {
            Det.Crear(1m, "Item", 0m, "UND", 50m, "GR", "001")
        };

        var recepcion = Agg.Materializar(
            codigo: 5,
            fechaEmision: DateTime.Today,
            remitenteId: 1,
            tipoDirPartida: TipoDir.Agencia,
            direccionPartida: "Agencia",
            destinatarioId: 2,
            tipoDirDestino: TipoDir.DireccionCliente,
            direccionDestino: "Dir",
            destinoId: 1,
            estadoId: 1,
            bultos: null,
            pesoTotal: null,
            costoTotal: 50m,
            guiaRemisionVinculada: "T001-000001",
            observacion: null,
            usuarioId: 1,
            detalles: detalles);

        recepcion.Codigo.Should().Be(5);
        recepcion.GuiaRemisionVinculada.Should().Be("T001-000001");
    }
}
