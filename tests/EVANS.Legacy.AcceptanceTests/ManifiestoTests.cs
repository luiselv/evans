using Dapper;
using EVANS.Legacy.AcceptanceTests.Infrastructure;
using FluentAssertions;

namespace EVANS.Legacy.AcceptanceTests;

public class ManifiestoTests(SqlFixture fixture) : AcceptanceTestBase(fixture)
{
    private async Task<int> InsertGuiaAsync(int remitente, int destinatario, double peso, double costo)
    {
        using var conn = OpenYear();
        return await conn.ExecuteScalarAsync<int>(@"
            INSERT INTO GuiaRemision (GREM_SERIE, GREM_NUMERO, GREM_FECHAEMISION, GREM_FECHATRASLADO,
                CLIE_REMITENTE, GREM_TIPODIRPARTIDA, GREM_DIRECCIONPARTIDA,
                CLIE_DESTINATARIO, GREM_TIPODIRDESTINO, GREM_DIRECCIONDESTINO,
                DEST_CODIGO, ESTA_CODIGO, GREM_BULTOS, GREM_PESOTOTAL, GREM_COSTOTOTAL,
                GREM_IMPRESO, USU_CODIGO, GREM_ENVIADO, GREM_MANIFIESTO)
            VALUES ('T001', @nro, GETDATE(), DATEADD(day,1,GETDATE()),
                @remitente, 1, 'Origen', @destinatario, 1, 'Destino',
                1, 1, 1, @peso, @costo, 0, 1, 0, 0);
            SELECT CAST(SCOPE_IDENTITY() AS INT);",
            new { nro = new Random().Next(100000, 999999).ToString(), remitente, destinatario, peso, costo });
    }

    [Fact]
    public async Task Grabar_ManifiestoConDosGuias_PersisteTotalesYMarcaGuias()
    {
        // Arrange
        var remitente = await InsertClienteAsync("Remitente Manif");
        var destinatario = await InsertClienteAsync("Destinatario Manif");
        var guia1 = await InsertGuiaAsync(remitente, destinatario, 100.0, 200.0);
        var guia2 = await InsertGuiaAsync(remitente, destinatario, 50.0, 80.0);

        using var mainConn = OpenMain();
        var paramsRow = await mainConn.QuerySingleAsync("SELECT PARA_MANIFIESTO FROM PARAMETROS");
        string nroManifiesto = paramsRow.PARA_MANIFIESTO ?? "00000000001";

        // Act — replica clsManifiesto.Grabar()
        using var yearConn = OpenYear();
        await yearConn.OpenAsync();
        using var tx = yearConn.BeginTransaction(System.Data.IsolationLevel.Serializable);

        var maniCodigo = await yearConn.ExecuteScalarAsync<int>(@"
            INSERT INTO Manifiesto (MANI_NUMERO, MANI_FECHA, EMPR_CODIGO, VEHI_CODIGO, CARR_CODIGO,
                CHOF_CODIGO, MANI_IMPORTE, MANI_NROGUIAS, MANI_PESO, ESTA_CODIGO, USU_CODIGO)
            VALUES (@nro, GETDATE(), 1, 1, NULL, 1, 280.0, 2, 150.0, 1, 1);
            SELECT CAST(SCOPE_IDENTITY() AS INT);",
            new { nro = nroManifiesto }, tx);

        await yearConn.ExecuteAsync(
            "INSERT INTO DetalleManifiesto (MANI_CODIGO, GREM_CODIGO) VALUES (@mani, @guia)",
            new { mani = maniCodigo, guia = guia1 }, tx);
        await yearConn.ExecuteAsync(
            "INSERT INTO DetalleManifiesto (MANI_CODIGO, GREM_CODIGO) VALUES (@mani, @guia)",
            new { mani = maniCodigo, guia = guia2 }, tx);

        await yearConn.ExecuteAsync(@"
            UPDATE GuiaRemision SET GREM_ENVIADO = 1, GREM_MANIFIESTO = 1, GREM_NROMANIFIESTO = @nro
            WHERE GREM_CODIGO IN (@g1, @g2)",
            new { nro = nroManifiesto, g1 = guia1, g2 = guia2 }, tx);

        tx.Commit();

        // Assert — manifiesto con 2 guías
        var mani = await yearConn.QuerySingleAsync(
            "SELECT MANI_NROGUIAS, MANI_PESO, MANI_IMPORTE FROM Manifiesto WHERE MANI_CODIGO = @id",
            new { id = maniCodigo });

        ((int)mani.MANI_NROGUIAS).Should().Be(2);
        ((double)mani.MANI_PESO).Should().BeApproximately(150.0, 0.001);
        ((double)mani.MANI_IMPORTE).Should().BeApproximately(280.0, 0.001);

        // Assert — guías marcadas como enviadas
        var guiasMarcadas = await yearConn.ExecuteScalarAsync<int>(
            "SELECT COUNT(*) FROM GuiaRemision WHERE GREM_CODIGO IN (@g1, @g2) AND GREM_ENVIADO = 1 AND GREM_MANIFIESTO = 1",
            new { g1 = guia1, g2 = guia2 });
        guiasMarcadas.Should().Be(2);

        // Assert — detalle manifiesto
        var detalles = await yearConn.ExecuteScalarAsync<int>(
            "SELECT COUNT(*) FROM DetalleManifiesto WHERE MANI_CODIGO = @id", new { id = maniCodigo });
        detalles.Should().Be(2);
    }
}
