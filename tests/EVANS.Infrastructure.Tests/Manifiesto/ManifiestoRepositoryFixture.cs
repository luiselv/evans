using Dapper;
using Microsoft.Data.SqlClient;
using Respawn;
using Testcontainers.MsSql;

namespace EVANS.Infrastructure.Tests.Manifiesto;

/// <summary>
/// Shared integration test fixture for Manifiesto infrastructure tests.
/// Primary: MsSqlContainer (Testcontainers) — requires Docker.
/// Fallback: LocalDB (MSSQLLocalDB) — used when Docker is not available.
/// Uses the same schema files as ComprobanteRepositoryFixture.
/// Seeds PARAMETROS with PARA_MANIFIESTO='0' for NumeradorManifiestoServiceSql tests.
/// </summary>
public class ManifiestoRepositoryFixture : IAsyncLifetime
{
    private const string LocalDbServer = @"(LocalDb)\MSSQLLocalDB";
    private const string LocalDbMasterName = "EVANS_ManifiestoTest";
    private const string LocalDbYearlyName = "ManifiestoTest_Year";

    private MsSqlContainer? _container;
    private Respawner? _masterRespawner;
    private Respawner? _yearlyRespawner;
    private bool _usingLocalDb;

    public const string MasterDbName = "EVANS_ManifiestoTest";
    public const string YearlyDbName = "ManifiestoTest_Year";
    public static readonly int TestYear = 2024;

    public string MasterConnectionString { get; private set; } = string.Empty;
    public string YearlyConnectionString { get; private set; } = string.Empty;

    public async Task InitializeAsync()
    {
        if (await TryStartContainerAsync())
            _usingLocalDb = false;
        else
        {
            _usingLocalDb = true;
            UseLocalDb();
        }

        await CreateDatabasesAsync();
        await ApplyMasterSchemaAsync();
        await ApplyYearlySchemaAsync();
        await SeedAsync();

        _masterRespawner = await Respawner.CreateAsync(MasterConnectionString, new RespawnerOptions
        {
            DbAdapter = DbAdapter.SqlServer
        });

        _yearlyRespawner = await Respawner.CreateAsync(YearlyConnectionString, new RespawnerOptions
        {
            DbAdapter = DbAdapter.SqlServer
        });
    }

    public async Task ResetAsync()
    {
        if (_masterRespawner is not null)
            await _masterRespawner.ResetAsync(MasterConnectionString);
        if (_yearlyRespawner is not null)
            await _yearlyRespawner.ResetAsync(YearlyConnectionString);
        await SeedAsync();
    }

    public async Task DisposeAsync()
    {
        if (_container is not null)
            await _container.DisposeAsync();
    }

    // ------------------------------------------------------------------
    // Initialization helpers
    // ------------------------------------------------------------------

    private async Task<bool> TryStartContainerAsync()
    {
        try
        {
            _container = new MsSqlBuilder()
                .WithImage("mcr.microsoft.com/mssql/server:2022-latest")
                .Build();

            await _container.StartAsync();

            var serverCs = _container.GetConnectionString();

            MasterConnectionString = new SqlConnectionStringBuilder(serverCs)
            {
                InitialCatalog = MasterDbName,
                TrustServerCertificate = true
            }.ConnectionString;

            YearlyConnectionString = new SqlConnectionStringBuilder(serverCs)
            {
                InitialCatalog = YearlyDbName,
                TrustServerCertificate = true
            }.ConnectionString;

            return true;
        }
        catch (Exception)
        {
            _container = null;
            return false;
        }
    }

    private void UseLocalDb()
    {
        MasterConnectionString =
            $"Server={LocalDbServer};Database={LocalDbMasterName};Integrated Security=True;TrustServerCertificate=True;";
        YearlyConnectionString =
            $"Server={LocalDbServer};Database={LocalDbYearlyName};Integrated Security=True;TrustServerCertificate=True;";
    }

    private async Task CreateDatabasesAsync()
    {
        string serverCs;

        if (_usingLocalDb)
        {
            serverCs = $"Server={LocalDbServer};Database=master;Integrated Security=True;TrustServerCertificate=True;";
        }
        else
        {
            var b = new SqlConnectionStringBuilder(_container!.GetConnectionString())
            {
                InitialCatalog = "master",
                TrustServerCertificate = true
            };
            serverCs = b.ConnectionString;
        }

        using var conn = new SqlConnection(serverCs);
        await conn.OpenAsync();
        await conn.ExecuteAsync($"IF NOT EXISTS (SELECT 1 FROM sys.databases WHERE name = '{MasterDbName}') CREATE DATABASE [{MasterDbName}]");
        await conn.ExecuteAsync($"IF NOT EXISTS (SELECT 1 FROM sys.databases WHERE name = '{YearlyDbName}') CREATE DATABASE [{YearlyDbName}]");
    }

    private async Task ApplyMasterSchemaAsync()
    {
        if (await TableExistsAsync(MasterConnectionString, "ESTADO")) return;
        var schemaPath = FindSchemaFile("schema-evans.sql");
        var sql = await File.ReadAllTextAsync(schemaPath);
        await RunBatchesAsync(MasterConnectionString, sql);
    }

    private async Task ApplyYearlySchemaAsync()
    {
        if (await TableExistsAsync(YearlyConnectionString, "Manifiesto")) return;
        var schemaPath = FindSchemaFile("schema-yearly.sql");
        var sql = await File.ReadAllTextAsync(schemaPath);
        await RunBatchesAsync(YearlyConnectionString, sql);
    }

    private static async Task<bool> TableExistsAsync(string connStr, string tableName)
    {
        using var conn = new SqlConnection(connStr);
        await conn.OpenAsync();
        var count = await conn.ExecuteScalarAsync<int>(
            "SELECT COUNT(*) FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = @tableName",
            new { tableName });
        return count > 0;
    }

    private static async Task RunBatchesAsync(string connStr, string sql)
    {
        using var conn = new SqlConnection(connStr);
        await conn.OpenAsync();

        foreach (var batch in SplitGoBatches(sql))
        {
            if (!string.IsNullOrWhiteSpace(batch))
            {
                try
                {
                    await conn.ExecuteAsync(batch);
                }
                catch (SqlException ex) when (IsAlreadyExistsError(ex))
                {
                    // Schema already applied — idempotent
                }
            }
        }
    }

    private static bool IsAlreadyExistsError(SqlException ex) =>
        ex.Number is 2714 or 1913 or 2705 or 1779 or 8111 or 3728;

    private async Task SeedAsync()
    {
        using var masterConn = new SqlConnection(MasterConnectionString);
        await masterConn.OpenAsync();

        await masterConn.ExecuteAsync(@"
            SET IDENTITY_INSERT ESTADO ON;
            IF NOT EXISTS (SELECT 1 FROM ESTADO WHERE ESTA_CODIGO = 1)
                INSERT INTO ESTADO (ESTA_CODIGO, ESTA_DESCRIPCION) VALUES (1, 'Activo');
            SET IDENTITY_INSERT ESTADO OFF;

            SET IDENTITY_INSERT EMPRESA ON;
            IF NOT EXISTS (SELECT 1 FROM EMPRESA WHERE EMPR_CODIGO = 1)
                INSERT INTO EMPRESA (EMPR_CODIGO, EMPR_NOMBRE, EMPR_RUC, EMPR_PROPIEDAD, ESTA_CODIGO)
                VALUES (1, 'Empresa Test', '20123456789', 1, 1);
            SET IDENTITY_INSERT EMPRESA OFF;

            SET IDENTITY_INSERT DESTINO ON;
            IF NOT EXISTS (SELECT 1 FROM DESTINO WHERE DEST_CODIGO = 1)
                INSERT INTO DESTINO (DEST_CODIGO, DEST_NOMBRE, DEST_DISTANCIAVIRTUAL, ESTA_CODIGO)
                VALUES (1, 'Lima', 0, 1);
            IF NOT EXISTS (SELECT 1 FROM DESTINO WHERE DEST_CODIGO = 2)
                INSERT INTO DESTINO (DEST_CODIGO, DEST_NOMBRE, DEST_DISTANCIAVIRTUAL, ESTA_CODIGO)
                VALUES (2, 'Trujillo', 500, 1);
            SET IDENTITY_INSERT DESTINO OFF;

            SET IDENTITY_INSERT VEHICULO ON;
            IF NOT EXISTS (SELECT 1 FROM VEHICULO WHERE VEHI_CODIGO = 1)
                INSERT INTO VEHICULO (VEHI_CODIGO, VEHI_MARCA, VEHI_PLACA, VEHI_CONFVEHICULAR, EMPR_CODIGO, ESTA_CODIGO)
                VALUES (1, 'Toyota', 'ABC-123', 'C2', 1, 1);
            SET IDENTITY_INSERT VEHICULO OFF;

            SET IDENTITY_INSERT CARRETA ON;
            IF NOT EXISTS (SELECT 1 FROM CARRETA WHERE CARR_CODIGO = 1)
                INSERT INTO CARRETA (CARR_CODIGO, CARR_PLACA, CARR_MARCA, EMPR_CODIGO, ESTA_CODIGO)
                VALUES (1, 'XYZ-789', 'Volvo', 1, 1);
            SET IDENTITY_INSERT CARRETA OFF;

            SET IDENTITY_INSERT CHOFER ON;
            IF NOT EXISTS (SELECT 1 FROM CHOFER WHERE CHOF_CODIGO = 1)
                INSERT INTO CHOFER (CHOF_CODIGO, CHOF_NOMBRE, CHOF_LICENCIA, EMPR_CODIGO, ESTA_CODIGO)
                VALUES (1, 'Juan Perez', 'A-12345', 1, 1);
            SET IDENTITY_INSERT CHOFER OFF;

            IF NOT EXISTS (SELECT 1 FROM PARAMETROS)
                INSERT INTO PARAMETROS (PARA_IGV, PARA_GREMSERIE, PARA_GREMNRO1, PARA_GREMNRO2,
                    PARA_FACTSERIE, PARA_FACTNRO1, PARA_FACTNRO2,
                    PARA_BOLSERIE, PARA_BOLNRO1, PARA_BOLNRO2,
                    PARA_MANIFIESTO)
                VALUES (0.18, 'GR01', '000000', '000000', 'F001', '000000', '000000',
                    'B001', '000000', '000000', '0');

            IF NOT EXISTS (SELECT 1 FROM USUARIO WHERE USU_NOMBREUSUARIO = 'testuser')
                INSERT INTO USUARIO (USU_NOMBREUSUARIO, USU_CLAVE, USU_NOMBRECOMPLETO, USU_ADMIN, ESTA_CODIGO)
                VALUES ('testuser', 'testpass', 'Test User', 1, 1);
        ");

        // Seed GuiaRemision rows in yearly DB for GuiasManifiestoService tests.
        // These are always available; Respawn resets and re-seeds before each test.
        using var yearlyConn = new SqlConnection(YearlyConnectionString);
        await yearlyConn.OpenAsync();

        await yearlyConn.ExecuteAsync(@"
            IF NOT EXISTS (SELECT 1 FROM GuiaRemision WHERE GREM_CODIGO = 1)
            BEGIN
                SET IDENTITY_INSERT GuiaRemision ON;
                INSERT INTO GuiaRemision (
                    GREM_CODIGO, GREM_SERIE, GREM_NUMERO,
                    GREM_FECHAEMISION, GREM_FECHATRASLADO,
                    CLIE_REMITENTE, CLIE_DESTINATARIO,
                    DEST_CODIGO, VEHI_CODIGO, CARR_CODIGO, CHOF_CODIGO, EMPR_CODIGO,
                    ESTA_CODIGO, GREM_COSTOTOTAL, GREM_PESOTOTAL,
                    GREM_ENVIADO, GREM_MANIFIESTO, GREM_NROMANIFIESTO
                )
                VALUES (
                    1, 'GR01', '000001',
                    '2024-06-01', '2024-06-02',
                    1, 1,
                    1, 1, 1, 1, 1,
                    1, 100.0, 50.0,
                    0, 0, ''
                );
                SET IDENTITY_INSERT GuiaRemision OFF;
            END

            IF NOT EXISTS (SELECT 1 FROM GuiaRemision WHERE GREM_CODIGO = 2)
            BEGIN
                SET IDENTITY_INSERT GuiaRemision ON;
                INSERT INTO GuiaRemision (
                    GREM_CODIGO, GREM_SERIE, GREM_NUMERO,
                    GREM_FECHAEMISION, GREM_FECHATRASLADO,
                    CLIE_REMITENTE, CLIE_DESTINATARIO,
                    DEST_CODIGO, VEHI_CODIGO, CARR_CODIGO, CHOF_CODIGO, EMPR_CODIGO,
                    ESTA_CODIGO, GREM_COSTOTOTAL, GREM_PESOTOTAL,
                    GREM_ENVIADO, GREM_MANIFIESTO, GREM_NROMANIFIESTO
                )
                VALUES (
                    2, 'GR01', '000002',
                    '2024-06-01', '2024-06-02',
                    1, 1,
                    2, 1, 1, 1, 1,
                    1, 80.0, 30.0,
                    0, 0, ''
                );
                SET IDENTITY_INSERT GuiaRemision OFF;
            END
        ");
    }

    private static string[] SplitGoBatches(string sql) =>
        System.Text.RegularExpressions.Regex.Split(
            sql,
            @"^\s*GO\s*$",
            System.Text.RegularExpressions.RegexOptions.Multiline |
            System.Text.RegularExpressions.RegexOptions.IgnoreCase);

    private static string FindSchemaFile(string fileName)
    {
        var dir = new DirectoryInfo(Directory.GetCurrentDirectory());
        while (dir is not null)
        {
            var candidate = Path.Combine(dir.FullName, "db", fileName);
            if (File.Exists(candidate)) return candidate;
            dir = dir.Parent;
        }
        throw new FileNotFoundException($"Schema file '{fileName}' not found under any parent of cwd.");
    }
}
