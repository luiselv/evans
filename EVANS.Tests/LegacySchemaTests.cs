using EVANS.Tests.Infrastructure;
using Microsoft.Data.SqlClient;

namespace EVANS.Tests;

public class LegacySchemaTests : TestBase
{
    public LegacySchemaTests(DatabaseFixture db) : base(db) { }

    [Fact]
    public async Task Seed_EvansDb_HasTwoClients()
    {
        await using var seed = EvansConn();
        await SeedData.InsertMinimalMasterData(seed);

        await using var q = EvansConn();
        var count = await ExecScalarAsync(q, "SELECT COUNT(*) FROM dbo.CLIENTE");

        Assert.Equal(2, count);
    }

    [Fact]
    public async Task Seed_EvansDb_HasParametrosRow()
    {
        await using var seed = EvansConn();
        await SeedData.InsertMinimalMasterData(seed);

        await using var q = EvansConn();
        var count = await ExecScalarAsync(q, "SELECT COUNT(*) FROM dbo.PARAMETROS");

        Assert.Equal(1, count);
    }

    [Fact]
    public async Task YearlyDb_AfterReset_HasNoGuiaRemision()
    {
        await using var q = YearlyConn();
        var count = await ExecScalarAsync(q, "SELECT COUNT(*) FROM dbo.GuiaRemision");

        Assert.Equal(0, count);
    }

    [Fact]
    public async Task GuiaRemision_Insert_AssignsIdentity()
    {
        await using var conn = YearlyConn();
        var id = await ExecScalarAsync(conn, @"
            INSERT INTO dbo.GuiaRemision
                (grem_serie,grem_numero,grem_fechaemision,grem_fechatraslado,
                 clie_remitente,grem_tipodirpartida,grem_direccionpartida,
                 clie_destinatario,grem_tipodirdestino,grem_direcciondestino,
                 dest_codigo,vehi_codigo,carr_codigo,chof_codigo,empr_codigo,
                 esta_codigo,tico_codigo,usu_codigo)
            VALUES
                ('T001','000001',GETDATE(),GETDATE(),
                 1,1,'Av. Partida 123',
                 2,2,'Av. Destino 456',
                 1,1,1,1,1,1,1,1);
            SELECT SCOPE_IDENTITY()");

        Assert.True(id > 0);
    }

    [Fact]
    public async Task DetalleGuia_Insert_CanBeQueried()
    {
        await using var conn = YearlyConn();
        var count = await ExecScalarAsync(conn, @"
            DECLARE @id INT;
            INSERT INTO dbo.GuiaRemision
                (grem_serie,grem_numero,grem_fechaemision,grem_fechatraslado,
                 clie_remitente,grem_tipodirpartida,grem_direccionpartida,
                 clie_destinatario,grem_tipodirdestino,grem_direcciondestino,
                 dest_codigo,vehi_codigo,carr_codigo,chof_codigo,empr_codigo,
                 esta_codigo,tico_codigo,usu_codigo)
            VALUES
                ('T001','000001',GETDATE(),GETDATE(),
                 1,1,'Av. Partida 123',
                 2,2,'Av. Destino 456',
                 1,1,1,1,1,1,1,1);
            SET @id = SCOPE_IDENTITY();
            INSERT INTO dbo.DetalleGuia (grem_codigo,degr_cantidad,degr_descripcion,degr_peso,degr_costo)
            VALUES (@id,2.0,'Paquete de prueba',5.0,10.0);
            SELECT COUNT(*) FROM dbo.DetalleGuia WHERE grem_codigo = @id");

        Assert.Equal(1, count);
    }

    [Fact]
    public async Task Comprobante_Insert_AssignsIdentity()
    {
        await using var conn = YearlyConn();
        var id = await ExecScalarAsync(conn, @"
            INSERT INTO dbo.Comprobante
                (comp_serie,comp_numero,comp_fecha,
                 clie_destinatario,tico_codigo,esta_codigo,
                 clie_remitente,empr_codigo,dest_codigo,usu_codigo)
            VALUES
                ('F001','000001',GETDATE(),
                 2,1,1,1,1,1,1);
            SELECT SCOPE_IDENTITY()");

        Assert.True(id > 0);
    }

    [Fact]
    public async Task Manifiesto_Insert_CanLinkGuia()
    {
        await using var conn = YearlyConn();
        var count = await ExecScalarAsync(conn, @"
            DECLARE @grem INT, @mani INT;
            INSERT INTO dbo.GuiaRemision
                (grem_serie,grem_numero,grem_fechaemision,grem_fechatraslado,
                 clie_remitente,grem_tipodirpartida,grem_direccionpartida,
                 clie_destinatario,grem_tipodirdestino,grem_direcciondestino,
                 dest_codigo,vehi_codigo,carr_codigo,chof_codigo,empr_codigo,
                 esta_codigo,tico_codigo,usu_codigo)
            VALUES
                ('T001','000001',GETDATE(),GETDATE(),
                 1,1,'Av. Partida 123',
                 2,2,'Av. Destino 456',
                 1,1,1,1,1,1,1,1);
            SET @grem = SCOPE_IDENTITY();
            INSERT INTO dbo.Manifiesto
                (mani_numero,mani_fecha,empr_codigo,vehi_codigo,
                 carr_codigo,chof_codigo,esta_codigo,usu_codigo)
            VALUES ('M-001',GETDATE(),1,1,1,1,1,1);
            SET @mani = SCOPE_IDENTITY();
            INSERT INTO dbo.DetalleManifiesto (mani_codigo,grem_codigo) VALUES (@mani,@grem);
            SELECT COUNT(*) FROM dbo.DetalleManifiesto WHERE mani_codigo = @mani AND grem_codigo = @grem");

        Assert.Equal(1, count);
    }

    [Fact]
    public async Task Recepcion_Insert_AssignsIdentity()
    {
        await using var conn = YearlyConn();
        var id = await ExecScalarAsync(conn, @"
            INSERT INTO dbo.Recepcion
                (rece_fechaemision,clie_remitente,rece_tipodirpartida,rece_direccionpartida,
                 clie_destinatario,rece_tipodirdestino,rece_direcciondestino,
                 dest_codigo,esta_codigo,usu_codigo)
            VALUES
                (GETDATE(),1,1,'Av. Partida 123',
                 2,2,'Av. Destino 456',
                 1,1,1);
            SELECT SCOPE_IDENTITY()");

        Assert.True(id > 0);
    }
}
