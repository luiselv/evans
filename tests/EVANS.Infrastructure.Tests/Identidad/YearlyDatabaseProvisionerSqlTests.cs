using Dapper;
using EVANS.Infrastructure.Sql.Connections;
using EVANS.Infrastructure.Sql.Identidad;
using EVANS.Infrastructure.Tests.Catalogo;
using EVANS.Infrastructure.Tests.GuiaRemision;
using Microsoft.Data.SqlClient;

namespace EVANS.Infrastructure.Tests.Identidad;

[Collection("GuiaRepository")]
public sealed class YearlyDatabaseProvisionerSqlTests : IAsyncLifetime
{
    private readonly GuiaRepositoryFixture _fixture;
    private int _provisionedYear;
    private int _existingYear;
    private readonly List<int> _createdYears = [];

    public YearlyDatabaseProvisionerSqlTests(GuiaRepositoryFixture fixture) => _fixture = fixture;

    public async Task InitializeAsync()
    {
        (_provisionedYear, _existingYear) = await ReserveFreeYearsAsync();
    }

    public async Task DisposeAsync()
    {
        foreach (var year in _createdYears)
            await DropDatabaseIfExistsAsync(year);
    }

    [Fact]
    public async Task CreateYearAsync_CreatesDatabaseWithYearlySchemaAndCatalogListsIt()
    {
        var provisioner = CreateProvisioner();
        var catalog = new YearlyDatabaseCatalogSql(new FixedMasterConnectionFactory(_fixture.MasterConnectionString));

        await provisioner.CreateYearAsync(_provisionedYear, CancellationToken.None);
        _createdYears.Add(_provisionedYear);

        await using var yearly = CreateYearConnection(_provisionedYear);
        await yearly.OpenAsync();
        var tableCount = await yearly.ExecuteScalarAsync<int>(
            "SELECT COUNT(*) FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME IN ('GuiaRemision', 'Comprobante', 'Manifiesto', 'Recepcion')");
        var keyCount = await yearly.ExecuteScalarAsync<int>(
            "SELECT COUNT(*) FROM sys.key_constraints WHERE name IN ('PK_GuiaRemision', 'PK_Comprobante', 'PK_Manifiesto', 'PK_Recepcion')");
        var years = await catalog.ListYearsAsync(CancellationToken.None);

        tableCount.Should().Be(4);
        keyCount.Should().Be(4);
        years.Should().Contain(_provisionedYear);
    }

    [Fact]
    public async Task CreateYearAsync_WhenDatabaseAlreadyExists_ThrowsAndLeavesItInPlace()
    {
        await CreateEmptyDatabaseAsync(_existingYear);
        _createdYears.Add(_existingYear);
        var provisioner = CreateProvisioner();

        var act = () => provisioner.CreateYearAsync(_existingYear, CancellationToken.None);

        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("Ya existe una Base de Datos para el año actual");
        (await DatabaseExistsAsync(_existingYear)).Should().BeTrue();
    }

    private async Task<(int ProvisionedYear, int ExistingYear)> ReserveFreeYearsAsync()
    {
        var reserved = new List<int>();
        for (var year = 2999; year >= 2900 && reserved.Count < 2; year--)
        {
            if (!await DatabaseExistsAsync(year))
                reserved.Add(year);
        }

        if (reserved.Count < 2)
            throw new InvalidOperationException("Could not find two free yearly database names for provisioning tests.");

        return (reserved[0], reserved[1]);
    }

    private YearlyDatabaseProvisionerSql CreateProvisioner() => new(
        new FixedMasterConnectionFactory(_fixture.MasterConnectionString),
        new TemplateYearlyConnectionFactory(_fixture.MasterConnectionString));

    private SqlConnection CreateYearConnection(int year)
    {
        var builder = new SqlConnectionStringBuilder(_fixture.MasterConnectionString)
        {
            InitialCatalog = year.ToString()
        };
        return new SqlConnection(builder.ConnectionString);
    }

    private async Task CreateEmptyDatabaseAsync(int year)
    {
        await using var conn = new SqlConnection(_fixture.MasterConnectionString);
        await conn.OpenAsync();
        await conn.ExecuteAsync($"CREATE DATABASE [{year}]");
    }

    private async Task<bool> DatabaseExistsAsync(int year)
    {
        await using var conn = new SqlConnection(_fixture.MasterConnectionString);
        await conn.OpenAsync();
        var count = await conn.ExecuteScalarAsync<int>(
            "SELECT COUNT(*) FROM sys.databases WHERE name = @name",
            new { name = year.ToString() });
        return count > 0;
    }

    private async Task DropDatabaseIfExistsAsync(int year)
    {
        await using var conn = new SqlConnection(_fixture.MasterConnectionString);
        await conn.OpenAsync();
        await conn.ExecuteAsync($@"
            IF EXISTS (SELECT 1 FROM sys.databases WHERE name = '{year}')
            BEGIN
                ALTER DATABASE [{year}] SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
                DROP DATABASE [{year}];
            END");
    }

    private sealed class TemplateYearlyConnectionFactory(string masterConnectionString) : IYearlyTransactionalConnectionFactory
    {
        public SqlConnection Create(int year)
        {
            var builder = new SqlConnectionStringBuilder(masterConnectionString)
            {
                InitialCatalog = year.ToString()
            };
            return new SqlConnection(builder.ConnectionString);
        }

        public SqlConnection CreateForCurrentYear() => Create(DateTime.Today.Year);
    }
}

