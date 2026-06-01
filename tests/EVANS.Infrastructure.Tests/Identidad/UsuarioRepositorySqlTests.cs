using Dapper;
using EVANS.Infrastructure.Sql.Identidad;
using EVANS.Infrastructure.Tests.Catalogo;
using EVANS.Infrastructure.Tests.GuiaRemision;
using Microsoft.Data.SqlClient;

namespace EVANS.Infrastructure.Tests.Identidad;

[Collection("GuiaRepository")]
public sealed class UsuarioRepositorySqlTests : IAsyncLifetime
{
    private readonly GuiaRepositoryFixture _fixture;

    public UsuarioRepositorySqlTests(GuiaRepositoryFixture fixture) => _fixture = fixture;

    public Task InitializeAsync() => _fixture.ResetAsync();
    public Task DisposeAsync() => Task.CompletedTask;

    [Fact]
    public async Task AuthenticateAsync_ValidActiveUser_ReturnsUsuario()
    {
        var repo = new UsuarioRepositorySql(new FixedMasterConnectionFactory(_fixture.MasterConnectionString));

        var usuario = await repo.AuthenticateAsync("testuser", "testpass", CancellationToken.None);

        usuario.Should().NotBeNull();
        usuario!.NombreUsuario.Should().Be("testuser");
        usuario.NombreCompleto.Should().Be("Test User");
        usuario.EsAdministrador.Should().BeTrue();
    }

    [Fact]
    public async Task AuthenticateAsync_WrongPassword_ReturnsNull()
    {
        var repo = new UsuarioRepositorySql(new FixedMasterConnectionFactory(_fixture.MasterConnectionString));

        var usuario = await repo.AuthenticateAsync("testuser", "wrongpass", CancellationToken.None);

        usuario.Should().BeNull();
    }

    [Fact]
    public async Task AuthenticateAsync_InactiveUser_ReturnsNull()
    {
        await using var conn = new SqlConnection(_fixture.MasterConnectionString);
        await conn.OpenAsync();
        await conn.ExecuteAsync(@"
            SET IDENTITY_INSERT ESTADO ON;
            IF NOT EXISTS (SELECT 1 FROM ESTADO WHERE ESTA_CODIGO = 2)
                INSERT INTO ESTADO (ESTA_CODIGO, ESTA_DESCRIPCION) VALUES (2, 'Inactivo');
            SET IDENTITY_INSERT ESTADO OFF;

            IF NOT EXISTS (SELECT 1 FROM USUARIO WHERE USU_NOMBREUSUARIO = 'inactive')
                INSERT INTO USUARIO (USU_NOMBREUSUARIO, USU_CLAVE, USU_NOMBRECOMPLETO, USU_ADMIN, ESTA_CODIGO)
                VALUES ('inactive', 'testpass', 'Inactive User', 0, 2);");

        var repo = new UsuarioRepositorySql(new FixedMasterConnectionFactory(_fixture.MasterConnectionString));

        var usuario = await repo.AuthenticateAsync("inactive", "testpass", CancellationToken.None);

        usuario.Should().BeNull();
    }
}
