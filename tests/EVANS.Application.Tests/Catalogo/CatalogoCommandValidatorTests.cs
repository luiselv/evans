using EVANS.Application.Catalogo.Commands;
using EVANS.Application.Catalogo.DTOs;
using FluentValidation.TestHelper;

namespace EVANS.Application.Tests.Catalogo;

public class CatalogoCommandValidatorTests
{
    [Fact]
    public void CreateClienteValidator_WithoutDirecciones_HasValidationError()
    {
        var validator = new CreateClienteCommandValidator();
        var command = new CreateClienteCommand("ACME", 1, "20123456789", 11, null, null, []);

        var result = validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(c => c.Direcciones);
    }

    [Fact]
    public void CreateClienteValidator_ValidDniWithLeadingZero_HasNoValidationError()
    {
        var validator = new CreateClienteCommandValidator();
        var command = new CreateClienteCommand(
            "ACME",
            2,
            "07654321",
            8,
            null,
            null,
            [new DireccionDto("Av Lima", "Lima", "Lima")]);

        var result = validator.TestValidate(command);

        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void CreateEmpresaValidator_InvalidRuc_HasValidationError()
    {
        var validator = new CreateEmpresaCommandValidator();
        var command = new CreateEmpresaCommand("TRANSPORT SA", null, null, "123", false);

        var result = validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(c => c.Ruc);
    }

    [Fact]
    public void CreateEmpresaValidator_InvalidEstadoCodigo_HasValidationError()
    {
        var validator = new CreateEmpresaCommandValidator();
        var command = new CreateEmpresaCommand("TRANSPORT SA", null, null, "20123456789", false, 0);

        var result = validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(c => c.EstadoCodigo);
    }

    [Fact]
    public void CreateClienteValidator_NullNroIdentificacion_ReturnsValidationError()
    {
        var validator = new CreateClienteCommandValidator();
        var command = new CreateClienteCommand(
            "ACME",
            2,
            null!,
            8,
            null,
            null,
            [new DireccionDto("Av Lima", "Lima", "Lima")]);

        var result = validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(c => c.NroIdentificacion);
    }

    [Fact]
    public void UpdateClienteValidator_NullNroIdentificacion_ReturnsValidationError()
    {
        var validator = new UpdateClienteCommandValidator();
        var command = new UpdateClienteCommand(
            1,
            "ACME",
            2,
            null!,
            8,
            null,
            null,
            [new DireccionDto("Av Lima", "Lima", "Lima")]);

        var result = validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(c => c.NroIdentificacion);
    }

    [Fact]
    public void CreateTipoIdentificacionValidator_WithoutDescripcion_HasValidationError()
    {
        var validator = new CreateTipoIdentificacionCommandValidator();
        var command = new CreateTipoIdentificacionCommand("");

        var result = validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(c => c.Descripcion);
    }

    [Fact]
    public void CreateVehiculoValidator_WithoutPlaca_HasValidationError()
    {
        var validator = new CreateVehiculoCommandValidator();
        var command = new CreateVehiculoCommand("Volvo", "", "C2", null, 10);

        var result = validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(c => c.Placa);
    }

    [Fact]
    public void CreateChoferValidator_InvalidEstadoCodigo_HasValidationError()
    {
        var validator = new CreateChoferCommandValidator();
        var command = new CreateChoferCommand("JUAN PEREZ", "A123", null, null, 1, 0);

        var result = validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(c => c.EstadoCodigo);
    }

    [Fact]
    public void DeactivateEmpresaValidator_CodigoZero_HasValidationError()
    {
        var validator = new DeactivateEmpresaCommandValidator();

        var result = validator.TestValidate(new DeactivateEmpresaCommand(0));

        result.ShouldHaveValidationErrorFor(c => c.Codigo);
    }

    [Theory]
    [InlineData("EVANS.Application.Catalogo.Commands.DeactivateClienteCommand")]
    [InlineData("EVANS.Application.Catalogo.Commands.DeactivateEstadoCommand")]
    [InlineData("EVANS.Application.Catalogo.Commands.DeactivateTipoIdentificacionCommand")]
    [InlineData("EVANS.Application.Catalogo.Commands.CreateAgenciaCommand")]
    [InlineData("EVANS.Application.Catalogo.Commands.UpdateAgenciaCommand")]
    [InlineData("EVANS.Application.Catalogo.Commands.DeactivateAgenciaCommand")]
    public void UnsupportedCommands_AreAbsent(string typeName)
    {
        typeof(CreateClienteCommand).Assembly.GetType(typeName).Should().BeNull();
    }
}
