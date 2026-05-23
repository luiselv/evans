using Dapper;
using Microsoft.Data.SqlClient;
using Respawn;
using Testcontainers.MsSql;

namespace EVANS.Infrastructure.Tests.Recepcion;

/// <summary>
/// Shared integration test fixture for Recepcion infrastructure tests.
/// Primary: MsSqlContainer (Testcontainers) — requires Docker.
/// Fallback: LocalDB (MSSQLLocalDB) — used when Docker is not available.
/// Uses the same schema files as ManifiestoRepositoryFixture.
/// </summary>
public class RecepcionRepositoryFixture : IAsyncLifetime
{
    private const string LocalDbServer = @"(LocalDb)\MSSQLLocalDB";
    private const string LocalDbMasterName = "EVANS_RecepcionTest";
    private const string LocalDbYearlyName = "RecepcionTest_Year";

    private MsSqlContainer? _container;
    private Respawner? _masterRespawner;
    private Respawner? _yearlyRespawner;
    private bool _usingLocalDb;

    public const string MasterDbName = "EVANS_RecepcionTest";
    public const string YearlyDbName = "RecepcionTest_Year";
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
        if (await TableExistsAsync(YearlyConnectionString, "Recepcion")) return;
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

            SET IDENTITY_INSERT TIPOIDENTIFICACION ON;
            IF NOT EXISTS (SELECT 1 FROM TIPOIDENTIFICACION WHERE IDEN_CODIGO = 1)
                INSERT INTO TIPOIDENTIFICACION (IDEN_CODIGO, IDEN_DESCRIPCION) VALUES (1, 'RUC');
            SET IDENTITY_INSERT TIPOIDENTIFICACION OFF;

            SET IDENTITY_INSERT CLIENTE ON;
            IF NOT EXISTS (SELECT 1 FROM CLIENTE WHERE CLIE_CODIGO = 1)
                INSERT INTO CLIENTE (CLIE_CODIGO, CLIE_NOMBRE, IDEN_CODIGO, CLIE_NROIDENTIFICACION)
                VALUES (1, 'Remitente Test SA', 1, '20111111111');
            IF NOT EXISTS (SELECT 1 FROM CLIENTE WHERE CLIE_CODIGO = 2)
                INSERT INTO CLIENTE (CLIE_CODIGO, CLIE_NOMBRE, IDEN_CODIGO, CLIE_NROIDENTIFICACION)
                VALUES (2, 'Destinatario Test SA', 1, '20222222222');
            SET IDENTITY_INSERT CLIENTE OFF;

            SET IDENTITY_INSERT DESTINO ON;
            IF NOT EXISTS (SELECT 1 FROM DESTINO WHERE DEST_CODIGO = 1)
                INSERT INTO DESTINO (DEST_CODIGO, DEST_NOMBRE, DEST_DISTANCIAVIRTUAL, ESTA_CODIGO)
                VALUES (1, 'Lima', 0, 1);
            SET IDENTITY_INSERT DESTINO OFF;

            IF NOT EXISTS (SELECT 1 FROM USUARIO WHERE USU_NOMBREUSUARIO = 'testuser')
                INSERT INTO USUARIO (USU_NOMBREUSUARIO, USU_CLAVE, USU_NOMBRECOMPLETO, USU_ADMIN, ESTA_CODIGO)
                VALUES ('testuser', 'testpass', 'Test User', 1, 1);
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
