using Dapper;
using EVANS.Legacy.AcceptanceTests.Infrastructure;
using FluentAssertions;

namespace EVANS.Legacy.AcceptanceTests;

public class ClienteTests(SqlFixture fixture) : AcceptanceTestBase(fixture)
{
    [Fact]
    public async Task Grabar_NuevoCliente_PersisteCampos()
    {
        // Arrange
        using var conn = OpenMain();

        // Act — replica lo que hace clsCliente.Grabar()
        var id = await conn.ExecuteScalarAsync<int>(@"
            INSERT INTO CLIENTE (CLIE_NOMBRE, IDEN_CODIGO, CLIE_NROIDENTIFICACION, CLIE_TELEFONO, CLIE_EMAIL)
            VALUES (@nombre, 1, @ruc, @tel, @email);
            SELECT CAST(SCOPE_IDENTITY() AS INT);",
            new
            {
                nombre = "Empresa Ejemplo SAC",
                ruc = "20456789012",
                tel = "01-2345678",
                email = "contacto@ejemplo.com"
            });

        // Assert
        var row = await conn.QuerySingleAsync(
            "SELECT CLIE_NOMBRE, CLIE_NROIDENTIFICACION, CLIE_TELEFONO FROM CLIENTE WHERE CLIE_CODIGO = @id",
            new { id });

        ((string)row.CLIE_NOMBRE).Should().Be("Empresa Ejemplo SAC");
        ((string)row.CLIE_NROIDENTIFICACION).Should().Be("20456789012");
        ((string)row.CLIE_TELEFONO).Should().Be("01-2345678");
    }

    [Fact]
    public async Task BuscarPorNombre_RetornaFilasCorrectas()
    {
        // Arrange
        await InsertClienteAsync("Transportes Norte SRL");
        await InsertClienteAsync("Transportes Sur SAC");
        await InsertClienteAsync("Distribuidora Este EIRL");

        // Act — replica clsCliente.BuscarXNombre()
        using var conn = OpenMain();
        var results = await conn.QueryAsync<string>(
            "SELECT CLIE_NOMBRE FROM CLIENTE WHERE CLIE_NOMBRE LIKE @filtro ORDER BY CLIE_NOMBRE",
            new { filtro = "%Transportes%" });

        // Assert
        results.Should().HaveCount(2);
        results.Should().Contain("Transportes Norte SRL");
        results.Should().Contain("Transportes Sur SAC");
    }
}
