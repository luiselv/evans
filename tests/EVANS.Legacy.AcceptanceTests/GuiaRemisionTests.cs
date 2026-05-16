using Dapper;
using EVANS.Legacy.AcceptanceTests.Infrastructure;
using FluentAssertions;
using Microsoft.Data.SqlClient;

namespace EVANS.Legacy.AcceptanceTests;

public class GuiaRemisionTests(SqlFixture fixture) : AcceptanceTestBase(fixture)
{
    [Fact]
    public async Task Grabar_NuevaGuia_PersisteCamposYIncrementaNumerador()
    {
        // Arrange
        var remitente = await InsertClienteAsync("Remitente Test");
        var destinatario = await InsertClienteAsync("Destinatario Test");

        using var mainConn = OpenMain();
        var paramsRow = await mainConn.QuerySingleAsync(
            "SELECT PARA_GREMSERIE, PARA_GREMNRO1 FROM PARAMETROS");
        string serie = paramsRow.PARA_GREMSERIE;
        string nroActual = paramsRow.PARA_GREMNRO1;
        int nroInt = int.Parse(nroActual);

        // Act — replica clsGuiaRemision.Grabar() dentro de transacción Serializable
        using var yearConn = OpenYear();
        await yearConn.OpenAsync();
        using var tx = yearConn.BeginTransaction(System.Data.IsolationLevel.Serializable);
        try
        {
            var now = DateTime.Now;
            var gremCodigo = await yearConn.ExecuteScalarAsync<int>(@"
                INSERT INTO GuiaRemision (
                    GREM_SERIE, GREM_NUMERO, GREM_FECHAEMISION, GREM_FECHATRASLADO,
                    CLIE_REMITENTE, GREM_TIPODIRPARTIDA, GREM_DIRECCIONPARTIDA,
                    CLIE_DESTINATARIO, GREM_TIPODIRDESTINO, GREM_DIRECCIONDESTINO,
                    DEST_CODIGO, VEHI_CODIGO, CHOF_CODIGO, EMPR_CODIGO,
                    ESTA_CODIGO, GREM_BULTOS, GREM_PESOTOTAL, GREM_COSTOTOTAL,
                    GREM_IMPRESO, USU_CODIGO, GREM_ENVIADO, GREM_MANIFIESTO)
                VALUES (@serie, @nro, @emision, @traslado,
                    @remitente, 1, 'Av. Lima 123',
                    @destinatario, 1, 'Av. Arequipa 456',
                    1, 1, 1, 1,
                    1, 2, 50.5, 100.0,
                    0, 1, 0, 0);
                SELECT CAST(SCOPE_IDENTITY() AS INT);",
                new
                {
                    serie,
                    nro = nroActual,
                    emision = now.Date,
                    traslado = now.Date.AddDays(1),
                    remitente,
                    destinatario
                }, tx);

            await yearConn.ExecuteAsync(@"
                INSERT INTO DetalleGuia (GREM_CODIGO, DEGR_CANTIDAD, DEGR_DESCRIPCION, DEGR_PESO, DEGR_UNIDAD, DEGR_COSTO)
                VALUES (@codigo, 2, 'Cajas de mercadería', 50.5, 'KG', 100.0)",
                new { codigo = gremCodigo }, tx);

            // Incrementar numerador (efecto cruzado a la DB principal — modMetodos style)
            // En el legacy esto cruza DBs desde la misma transacción; aquí lo hacemos separado
            // para validar el contrato de que AMBOS deben cambiar.
            tx.Commit();

            // Actualizar numerador en main DB (en legacy va dentro de la misma txn cross-DB)
            await mainConn.ExecuteAsync(
                "UPDATE PARAMETROS SET PARA_GREMNRO1 = @nuevo",
                new { nuevo = (nroInt + 1).ToString("D6") });

            // Assert — guia persiste
            var guia = await yearConn.QuerySingleAsync(
                "SELECT GREM_SERIE, GREM_NUMERO, CLIE_REMITENTE, GREM_PESOTOTAL FROM GuiaRemision WHERE GREM_CODIGO = @id",
                new { id = gremCodigo });

            ((string)guia.GREM_SERIE).Should().Be(serie);
            ((string)guia.GREM_NUMERO).Should().Be(nroActual);
            ((int)guia.CLIE_REMITENTE).Should().Be(remitente);
            ((double)guia.GREM_PESOTOTAL).Should().BeApproximately(50.5, 0.001);

            // Assert — detalle persiste
            var detalleCount = await yearConn.ExecuteScalarAsync<int>(
                "SELECT COUNT(*) FROM DetalleGuia WHERE GREM_CODIGO = @id", new { id = gremCodigo });
            detalleCount.Should().Be(1);

            // Assert — numerador incrementó
            var nuevoNro = await mainConn.ExecuteScalarAsync<string>(
                "SELECT PARA_GREMNRO1 FROM PARAMETROS");
            int.Parse(nuevoNro!).Should().Be(nroInt + 1);
        }
        catch
        {
            tx.Rollback();
            throw;
        }
    }

    [Fact]
    public async Task BuscarPorCodigo_GuiaExistente_RetornaCamposCorrectos()
    {
        // Arrange — insertar guía directamente
        var remitente = await InsertClienteAsync("Remitente Buscar");
        var destinatario = await InsertClienteAsync("Destinatario Buscar");

        using var yearConn = OpenYear();
        var id = await yearConn.ExecuteScalarAsync<int>(@"
            INSERT INTO GuiaRemision (GREM_SERIE, GREM_NUMERO, GREM_FECHAEMISION, GREM_FECHATRASLADO,
                CLIE_REMITENTE, GREM_TIPODIRPARTIDA, GREM_DIRECCIONPARTIDA,
                CLIE_DESTINATARIO, GREM_TIPODIRDESTINO, GREM_DIRECCIONDESTINO,
                DEST_CODIGO, ESTA_CODIGO, GREM_BULTOS, GREM_PESOTOTAL, GREM_COSTOTOTAL,
                GREM_IMPRESO, USU_CODIGO, GREM_ENVIADO, GREM_MANIFIESTO)
            VALUES ('T001', '000042', GETDATE(), DATEADD(day,1,GETDATE()),
                @remitente, 1, 'Calle Test 1',
                @destinatario, 1, 'Calle Test 2',
                1, 1, 3, 75.0, 150.0,
                0, 1, 0, 0);
            SELECT CAST(SCOPE_IDENTITY() AS INT);",
            new { remitente, destinatario });

        // Act — buscar por codigo
        var row = await yearConn.QuerySingleOrDefaultAsync(
            "SELECT GREM_CODIGO, GREM_SERIE, GREM_NUMERO, GREM_BULTOS FROM GuiaRemision WHERE GREM_CODIGO = @id",
            new { id });

        // Assert
        ((object)row).Should().NotBeNull();
        ((string)row.GREM_SERIE).Should().Be("T001");
        ((string)row.GREM_NUMERO).Should().Be("000042");
        ((int)row.GREM_BULTOS).Should().Be(3);
    }
}
