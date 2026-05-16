using Dapper;
using EVANS.Legacy.AcceptanceTests.Infrastructure;
using FluentAssertions;

namespace EVANS.Legacy.AcceptanceTests;

public class UsuarioTests(SqlFixture fixture) : AcceptanceTestBase(fixture)
{
    [Fact]
    public async Task Autenticar_CredencialesCorrectas_RetornaUsuario()
    {
        // Arrange — seed data ya insertado en SqlFixture.SeedBaseDataAsync
        using var conn = OpenMain();

        // Act — replica modMetodos.Autenticar()
        var row = await conn.QuerySingleOrDefaultAsync(@"
            SELECT USU_NOMBREUSUARIO, USU_NOMBRECOMPLETO, USU_ADMIN
            FROM USUARIO
            WHERE USU_NOMBREUSUARIO = @user AND USU_CLAVE = @clave AND ESTA_CODIGO = 1",
            new { user = "testuser", clave = "testpass" });

        // Assert
        ((object)row).Should().NotBeNull();
        ((string)row.USU_NOMBREUSUARIO).Should().Be("testuser");
        ((string)row.USU_NOMBRECOMPLETO).Should().Be("Test User");
        ((int)row.USU_ADMIN).Should().Be(1);
    }

    [Fact]
    public async Task Autenticar_CredencialesIncorrectas_RetornaNulo()
    {
        using var conn = OpenMain();

        var row = await conn.QuerySingleOrDefaultAsync(@"
            SELECT USU_NOMBREUSUARIO FROM USUARIO
            WHERE USU_NOMBREUSUARIO = @user AND USU_CLAVE = @clave AND ESTA_CODIGO = 1",
            new { user = "testuser", clave = "wrongpass" });

        ((object?)row).Should().BeNull();
    }
}
