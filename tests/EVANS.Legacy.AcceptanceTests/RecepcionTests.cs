using Dapper;
using EVANS.Legacy.AcceptanceTests.Infrastructure;
using FluentAssertions;

namespace EVANS.Legacy.AcceptanceTests;

public class RecepcionTests(SqlFixture fixture) : AcceptanceTestBase(fixture)
{
    [Fact]
    public async Task CrearRecepcion_LuegoGenerarGuia_ActualizaRecepcionConNroGuia()
    {
        // Arrange
        var remitente = await InsertClienteAsync("Remitente Recepcion");
        var destinatario = await InsertClienteAsync("Destinatario Recepcion");

        using var yearConn = OpenYear();

        // Crear recepción previa
        var receId = await yearConn.ExecuteScalarAsync<int>(@"
            INSERT INTO Recepcion (RECE_FECHAEMISION, CLIE_REMITENTE, RECE_TIPODIRPARTIDA, RECE_DIRECCIONPARTIDA,
                CLIE_DESTINATARIO, RECE_TIPODIRDESTINO, RECE_DIRECCIONDESTINO,
                DEST_CODIGO, ESTA_CODIGO, RECE_BULTOS, RECE_PESOTOTAL, RECE_COSTOTOTAL, USU_CODIGO)
            VALUES (GETDATE(), @remitente, 1, 'Partida', @destinatario, 1, 'Destino',
                1, 1, 2, 30.0, 60.0, 1);
            SELECT CAST(SCOPE_IDENTITY() AS INT);",
            new { remitente, destinatario });

        // Act — replica flujo bolGenerandoGuia = True:
        // Generar Guia desde Recepción → update Recepcion con numero de guia
        using var mainConn = OpenMain();
        var paramsRow = await mainConn.QuerySingleAsync(
            "SELECT PARA_GREMSERIE, PARA_GREMNRO1 FROM PARAMETROS");
        string serie = paramsRow.PARA_GREMSERIE;
        string nroGuia = paramsRow.PARA_GREMNRO1;
        int nroInt = int.Parse(nroGuia);

        await yearConn.OpenAsync();
        using var tx = yearConn.BeginTransaction(System.Data.IsolationLevel.Serializable);

        var gremId = await yearConn.ExecuteScalarAsync<int>(@"
            INSERT INTO GuiaRemision (GREM_SERIE, GREM_NUMERO, GREM_FECHAEMISION, GREM_FECHATRASLADO,
                CLIE_REMITENTE, GREM_TIPODIRPARTIDA, GREM_DIRECCIONPARTIDA,
                CLIE_DESTINATARIO, GREM_TIPODIRDESTINO, GREM_DIRECCIONDESTINO,
                DEST_CODIGO, ESTA_CODIGO, GREM_BULTOS, GREM_PESOTOTAL, GREM_COSTOTOTAL,
                GREM_IMPRESO, USU_CODIGO, GREM_ENVIADO, GREM_MANIFIESTO)
            VALUES (@serie, @nro, GETDATE(), DATEADD(day,1,GETDATE()),
                @remitente, 1, 'Partida', @destinatario, 1, 'Destino',
                1, 1, 2, 30.0, 60.0, 0, 1, 0, 0);
            SELECT CAST(SCOPE_IDENTITY() AS INT);",
            new { serie, nro = nroGuia, remitente, destinatario }, tx);

        // Efecto cruzado: actualizar Recepcion con el nro de guia generada
        await yearConn.ExecuteAsync(
            "UPDATE Recepcion SET RECE_GUIAREMISION = @nroGuia, ESTA_CODIGO = 2 WHERE RECE_CODIGO = @receId",
            new { nroGuia = $"{serie}{nroGuia}", receId }, tx);

        tx.Commit();

        await mainConn.ExecuteAsync(
            "UPDATE PARAMETROS SET PARA_GREMNRO1 = @nuevo",
            new { nuevo = (nroInt + 1).ToString("D6") });

        // Assert — recepción actualizada con número de guía
        var rece = await yearConn.QuerySingleAsync(
            "SELECT RECE_GUIAREMISION, ESTA_CODIGO FROM Recepcion WHERE RECE_CODIGO = @id",
            new { id = receId });

        ((string)rece.RECE_GUIAREMISION).Should().Be($"{serie}{nroGuia}");
        ((int)rece.ESTA_CODIGO).Should().Be(2);

        // Assert — guía creada correctamente
        var guia = await yearConn.QuerySingleAsync(
            "SELECT GREM_SERIE FROM GuiaRemision WHERE GREM_CODIGO = @id", new { id = gremId });
        ((string)guia.GREM_SERIE).Should().Be(serie);
    }
}
