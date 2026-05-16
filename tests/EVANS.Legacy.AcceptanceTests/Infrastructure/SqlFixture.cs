using Dapper;
using Microsoft.Data.SqlClient;
using Respawn;

namespace EVANS.Legacy.AcceptanceTests.Infrastructure;

public class SqlFixture : IAsyncLifetime
{
    private const string MainDbName = "EVANS_AcceptanceTest";
    private const string YearDbName = "2026_AcceptanceTest";
    private const string Server = @"(LocalDb)\MSSQLLocalDB";

    public static string MainConnectionString { get; } =
        $"Server={Server};Database={MainDbName};Integrated Security=True;TrustServerCertificate=True;";

    public static string YearConnectionString { get; } =
        $"Server={Server};Database={YearDbName};Integrated Security=True;TrustServerCertificate=True;";

    private Respawner? _mainRespawner;
    private Respawner? _yearRespawner;

    public async Task InitializeAsync()
    {
        await EnsureDatabasesAsync();
        await ApplySchemaAsync();
        await SeedBaseDataAsync();

        _mainRespawner = await Respawner.CreateAsync(MainConnectionString, new RespawnerOptions
        {
            TablesToIgnore = ["__RespawnCheckpoint"],
            DbAdapter = DbAdapter.SqlServer
        });

        _yearRespawner = await Respawner.CreateAsync(YearConnectionString, new RespawnerOptions
        {
            TablesToIgnore = ["__RespawnCheckpoint"],
            DbAdapter = DbAdapter.SqlServer
        });
    }

    public async Task ResetAsync()
    {
        if (_mainRespawner is not null)
            await _mainRespawner.ResetAsync(MainConnectionString);
        if (_yearRespawner is not null)
            await _yearRespawner.ResetAsync(YearConnectionString);
        await SeedBaseDataAsync();
    }

    public Task DisposeAsync() => Task.CompletedTask;

    public SqlConnection OpenMain() => new(MainConnectionString);
    public SqlConnection OpenYear() => new(YearConnectionString);

    private static async Task EnsureDatabasesAsync()
    {
        var masterConn = $"Server={Server};Database=master;Integrated Security=True;TrustServerCertificate=True;";
        using var conn = new SqlConnection(masterConn);
        await conn.OpenAsync();
        await conn.ExecuteAsync($"IF NOT EXISTS(SELECT 1 FROM sys.databases WHERE name='{MainDbName}') CREATE DATABASE [{MainDbName}]");
        await conn.ExecuteAsync($"IF NOT EXISTS(SELECT 1 FROM sys.databases WHERE name='{YearDbName}') CREATE DATABASE [{YearDbName}]");
    }

    private static async Task ApplySchemaAsync()
    {
        var scriptsPath = Path.Combine(FindProjectRoot(), "scripts");
        await ApplyScriptAsync(MainConnectionString, Path.Combine(scriptsPath, "01 main.sql"), MainDbName);
        await ApplyYearScriptAsync(YearConnectionString, Path.Combine(scriptsPath, "02 transactions.sql"), YearDbName);
    }

    private static async Task ApplyScriptAsync(string connStr, string scriptPath, string dbName)
    {
        var script = await File.ReadAllTextAsync(scriptPath);

        // Remove database-switching statements — we're already connected to the right DB
        script = System.Text.RegularExpressions.Regex.Replace(
            script,
            @"(?i)\bUSE\s+\[?[^\]\s]+\]?\s*\n?",
            string.Empty);

        // Replace references to EVANS with test DB name in cross-DB references
        script = script.Replace("[EVANS].", $"[{MainDbName}].", StringComparison.OrdinalIgnoreCase);

        using var conn = new SqlConnection(connStr);
        await conn.OpenAsync();

        foreach (var batch in SplitBatches(script))
        {
            if (!string.IsNullOrWhiteSpace(batch))
                await conn.ExecuteAsync(batch);
        }
    }

    private static async Task ApplyYearScriptAsync(string connStr, string scriptPath, string dbName)
    {
        var raw = await File.ReadAllTextAsync(scriptPath);
        // The script starts with dynamic CREATE DATABASE / USE — skip that block, just run DDL
        var ddlStart = raw.IndexOf("SET ANSI_NULLS ON", StringComparison.OrdinalIgnoreCase);
        if (ddlStart < 0) return;
        var ddl = raw[ddlStart..];

        using var conn = new SqlConnection(connStr);
        await conn.OpenAsync();

        foreach (var batch in SplitBatches(ddl))
        {
            if (!string.IsNullOrWhiteSpace(batch))
                await conn.ExecuteAsync(batch);
        }
    }

    private static async Task SeedBaseDataAsync()
    {
        // Use explicit IDs with IDENTITY_INSERT so IDs are predictable after every Respawn reset.
        using var conn = new SqlConnection(MainConnectionString);
        await conn.OpenAsync();

        await conn.ExecuteAsync(@"
            SET IDENTITY_INSERT ESTADO ON;
            IF NOT EXISTS (SELECT 1 FROM ESTADO WHERE ESTA_CODIGO = 1)
                INSERT INTO ESTADO (ESTA_CODIGO, ESTA_DESCRIPCION) VALUES (1, 'Activo');
            IF NOT EXISTS (SELECT 1 FROM ESTADO WHERE ESTA_CODIGO = 2)
                INSERT INTO ESTADO (ESTA_CODIGO, ESTA_DESCRIPCION) VALUES (2, 'Inactivo');
            SET IDENTITY_INSERT ESTADO OFF;

            SET IDENTITY_INSERT TIPOIDENTIFICACION ON;
            IF NOT EXISTS (SELECT 1 FROM TIPOIDENTIFICACION WHERE IDEN_CODIGO = 1)
                INSERT INTO TIPOIDENTIFICACION (IDEN_CODIGO, IDEN_DESCRIPCION) VALUES (1, 'RUC');
            IF NOT EXISTS (SELECT 1 FROM TIPOIDENTIFICACION WHERE IDEN_CODIGO = 2)
                INSERT INTO TIPOIDENTIFICACION (IDEN_CODIGO, IDEN_DESCRIPCION) VALUES (2, 'DNI');
            SET IDENTITY_INSERT TIPOIDENTIFICACION OFF;

            IF NOT EXISTS (SELECT 1 FROM USUARIO WHERE USU_NOMBREUSUARIO = 'testuser')
                INSERT INTO USUARIO (USU_NOMBREUSUARIO, USU_CLAVE, USU_NOMBRECOMPLETO, USU_ADMIN, ESTA_CODIGO)
                VALUES ('testuser', 'testpass', 'Test User', 1, 1);

            IF NOT EXISTS (SELECT 1 FROM PARAMETROS)
                INSERT INTO PARAMETROS (PARA_IGV, PARA_GREMSERIE, PARA_GREMNRO1, PARA_GREMNRO2,
                    PARA_FACTSERIE, PARA_FACTNRO1, PARA_FACTNRO2,
                    PARA_BOLSERIE, PARA_BOLNRO1, PARA_BOLNRO2, PARA_MANIFIESTO)
                VALUES (0.18, 'T001', '000001', '000000', 'F001', '000001', '000000',
                    'B001', '000001', '000000', '00000000001');

            SET IDENTITY_INSERT DESTINO ON;
            IF NOT EXISTS (SELECT 1 FROM DESTINO WHERE DEST_CODIGO = 1)
                INSERT INTO DESTINO (DEST_CODIGO, DEST_NOMBRE, DEST_DISTANCIAVIRTUAL, ESTA_CODIGO)
                VALUES (1, 'Lima', 0, 1);
            SET IDENTITY_INSERT DESTINO OFF;

            SET IDENTITY_INSERT EMPRESA ON;
            IF NOT EXISTS (SELECT 1 FROM EMPRESA WHERE EMPR_CODIGO = 1)
                INSERT INTO EMPRESA (EMPR_CODIGO, EMPR_NOMBRE, EMPR_RUC, EMPR_PROPIEDAD, ESTA_CODIGO)
                VALUES (1, 'Empresa Test', '20123456789', 1, 1);
            SET IDENTITY_INSERT EMPRESA OFF;

            SET IDENTITY_INSERT VEHICULO ON;
            IF NOT EXISTS (SELECT 1 FROM VEHICULO WHERE VEHI_CODIGO = 1)
                INSERT INTO VEHICULO (VEHI_CODIGO, VEHI_MARCA, VEHI_PLACA, VEHI_CONFVEHICULAR, EMPR_CODIGO, ESTA_CODIGO)
                VALUES (1, 'Toyota', 'ABC-123', 'C2', 1, 1);
            SET IDENTITY_INSERT VEHICULO OFF;

            SET IDENTITY_INSERT CHOFER ON;
            IF NOT EXISTS (SELECT 1 FROM CHOFER WHERE CHOF_CODIGO = 1)
                INSERT INTO CHOFER (CHOF_CODIGO, CHOF_NOMBRE, CHOF_LICENCIA, EMPR_CODIGO, ESTA_CODIGO)
                VALUES (1, 'Juan Perez', 'A-12345', 1, 1);
            SET IDENTITY_INSERT CHOFER OFF;
        ");
    }

    private static string[] SplitBatches(string script)
        => System.Text.RegularExpressions.Regex.Split(script, @"^\s*GO\s*$",
            System.Text.RegularExpressions.RegexOptions.Multiline |
            System.Text.RegularExpressions.RegexOptions.IgnoreCase);

    private static string FindProjectRoot()
    {
        var dir = new DirectoryInfo(Directory.GetCurrentDirectory());
        while (dir is not null)
        {
            if (Directory.Exists(Path.Combine(dir.FullName, "scripts")))
                return dir.FullName;
            dir = dir.Parent;
        }
        throw new InvalidOperationException("Cannot find project root with scripts/ directory.");
    }
}
