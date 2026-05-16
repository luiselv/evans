using Dapper;
using EVANS.Legacy.AcceptanceTests.Infrastructure;
using FluentAssertions;

namespace EVANS.Legacy.AcceptanceTests;

public class ComprobanteTests(SqlFixture fixture) : AcceptanceTestBase(fixture)
{
    [Fact]
    public async Task Grabar_Boleta_IncrementaNumeradorYPersiste()
    {
        // Arrange
        var remitente = await InsertClienteAsync("Remitente Boleta");
        var destinatario = await InsertClienteAsync("Destinatario Boleta");

        using var mainConn = OpenMain();
        var paramsRow = await mainConn.QuerySingleAsync(
            "SELECT PARA_BOLSERIE, PARA_BOLNRO1 FROM PARAMETROS");
        string serie = paramsRow.PARA_BOLSERIE;
        string nroActual = paramsRow.PARA_BOLNRO1;
        int nroInt = int.Parse(nroActual);

        // Act — replica clsComprobante.Grabar() para Boleta
        using var yearConn = OpenYear();
        var compCodigo = await yearConn.ExecuteScalarAsync<int>(@"
            INSERT INTO Comprobante (COMP_SERIE, COMP_NUMERO, COMP_FECHA,
                CLIE_DESTINATARIO, COMP_DIRECCION, TICO_CODIGO, ESTA_CODIGO,
                CLIE_REMITENTE, EMPR_CODIGO, DEST_CODIGO,
                COMP_VALORVENTA, COMP_IGV, COMP_TOTAL, COMP_IMPRESO, USU_CODIGO)
            VALUES (@serie, @nro, GETDATE(),
                @dest, 'Av. Test 123', 2, 1,
                @remi, 1, 1,
                84.75, 15.25, 100.0, 0, 1);
            SELECT CAST(SCOPE_IDENTITY() AS INT);",
            new { serie, nro = nroActual, dest = destinatario, remi = remitente });

        await mainConn.ExecuteAsync(
            "UPDATE PARAMETROS SET PARA_BOLNRO1 = @nuevo",
            new { nuevo = (nroInt + 1).ToString("D6") });

        // Assert — comprobante persiste
        var comp = await yearConn.QuerySingleAsync(
            "SELECT COMP_SERIE, COMP_NUMERO, COMP_TOTAL FROM Comprobante WHERE COMP_CODIGO = @id",
            new { id = compCodigo });

        ((string)comp.COMP_SERIE).Should().Be(serie);
        ((string)comp.COMP_NUMERO).Should().Be(nroActual);
        ((double)comp.COMP_TOTAL).Should().BeApproximately(100.0, 0.001);

        // Assert — numerador incrementó
        var nuevoNro = await mainConn.ExecuteScalarAsync<string>(
            "SELECT PARA_BOLNRO1 FROM PARAMETROS");
        int.Parse(nuevoNro!).Should().Be(nroInt + 1);
    }

    [Fact]
    public async Task Grabar_Factura_IncrementaNumeradorYPersiste()
    {
        // Arrange
        var remitente = await InsertClienteAsync("Remitente Factura");
        var destinatario = await InsertClienteAsync("Destinatario Factura");

        using var mainConn = OpenMain();
        var paramsRow = await mainConn.QuerySingleAsync(
            "SELECT PARA_FACTSERIE, PARA_FACTNRO1 FROM PARAMETROS");
        string serie = paramsRow.PARA_FACTSERIE;
        string nroActual = paramsRow.PARA_FACTNRO1;
        int nroInt = int.Parse(nroActual);

        // Act — replica clsComprobante.Grabar() para Factura
        using var yearConn = OpenYear();
        var compCodigo = await yearConn.ExecuteScalarAsync<int>(@"
            INSERT INTO Comprobante (COMP_SERIE, COMP_NUMERO, COMP_FECHA,
                CLIE_DESTINATARIO, COMP_DIRECCION, TICO_CODIGO, ESTA_CODIGO,
                CLIE_REMITENTE, EMPR_CODIGO, DEST_CODIGO,
                COMP_VALORVENTA, COMP_IGV, COMP_TOTAL, COMP_IMPRESO, USU_CODIGO)
            VALUES (@serie, @nro, GETDATE(),
                @dest, 'Jr. Test 456', 1, 1,
                @remi, 1, 1,
                169.49, 30.51, 200.0, 0, 1);
            SELECT CAST(SCOPE_IDENTITY() AS INT);",
            new { serie, nro = nroActual, dest = destinatario, remi = remitente });

        await mainConn.ExecuteAsync(
            "UPDATE PARAMETROS SET PARA_FACTNRO1 = @nuevo",
            new { nuevo = (nroInt + 1).ToString("D6") });

        // Assert
        var comp = await yearConn.QuerySingleAsync(
            "SELECT COMP_TOTAL, COMP_IGV FROM Comprobante WHERE COMP_CODIGO = @id",
            new { id = compCodigo });

        ((double)comp.COMP_TOTAL).Should().BeApproximately(200.0, 0.001);

        var nuevoNro = await mainConn.ExecuteScalarAsync<string>(
            "SELECT PARA_FACTNRO1 FROM PARAMETROS");
        int.Parse(nuevoNro!).Should().Be(nroInt + 1);
    }
}
