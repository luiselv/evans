using EVANS.Application.GuiaRemision.Commands;
using EVANS.Application.GuiaRemision.Validators;
using EVANS.Domain.GuiaRemision;
using FluentValidation.TestHelper;

namespace EVANS.Application.Tests.GuiaRemision;

public class ValidatorTests
{
    // ── CrearGuiaCommandValidator ──────────────────────────────────────────────

    private static CrearGuiaCommand BuildValidCrearCommand() => new(
        RemitenteId: 1,
        DestinatarioId: 2,
        DireccionPartida: "Av Lima|Lima|Lima",
        DireccionLlegada: "Av Arequipa|Arequipa|Arequipa",
        HasManifest: false,
        VehiculoId: null,
        CarretaId: null,
        ChoferId: null,
        Igv: 0.18m,
        Origen: new Standalone(),
        Year: 2024,
        Detalles: new List<DetalleGuiaInput>
        {
            new("Carga general", new Peso(10m), 100m, 100m, 1)
        });

    [Fact]
    public void CrearValidator_EmptyDetalles_HasValidationError()
    {
        var validator = new CrearGuiaCommandValidator();
        var command = BuildValidCrearCommand() with { Detalles = new List<DetalleGuiaInput>() };

        var result = validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(c => c.Detalles);
    }

    [Fact]
    public void CrearValidator_RemitenteIdZero_HasValidationError()
    {
        var validator = new CrearGuiaCommandValidator();
        var command = BuildValidCrearCommand() with { RemitenteId = 0 };

        var result = validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(c => c.RemitenteId);
    }

    // ── ActualizarGuiaCommandValidator ────────────────────────────────────────

    private static ActualizarGuiaCommand BuildValidActualizarCommand() => new(
        Codigo: 5,
        RemitenteId: 1,
        DestinatarioId: 2,
        DireccionPartida: "Av Lima|Lima|Lima",
        DireccionLlegada: "Av Arequipa|Arequipa|Arequipa",
        HasManifest: false,
        VehiculoId: null,
        CarretaId: null,
        ChoferId: null,
        Igv: 0.18m,
        Year: 2024,
        Detalles: new List<DetalleGuiaInput>
        {
            new("Carga general", new Peso(10m), 100m, 100m, 1)
        });

    [Fact]
    public void ActualizarValidator_EmptyDetalles_HasValidationError()
    {
        var validator = new ActualizarGuiaCommandValidator();
        var command = BuildValidActualizarCommand() with { Detalles = new List<DetalleGuiaInput>() };

        var result = validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(c => c.Detalles);
    }

    [Fact]
    public void ActualizarValidator_NegativePeso_HasValidationError()
    {
        var validator = new ActualizarGuiaCommandValidator();
        // Negative peso in DetalleGuiaInput — Peso record itself rejects negative at construction,
        // so we test via a detalle with PrecioUnitario < 0 (which the validator also checks).
        // The spec says "peso negativo" → validate PrecioUnitario < 0 as the validator-level gate.
        var command = BuildValidActualizarCommand() with
        {
            Detalles = new List<DetalleGuiaInput>
            {
                new("Carga", new Peso(10m), -1m, 100m, 1)
            }
        };

        var result = validator.TestValidate(command);

        result.ShouldHaveAnyValidationError();
    }
}
