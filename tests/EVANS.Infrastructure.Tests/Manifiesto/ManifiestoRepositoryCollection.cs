namespace EVANS.Infrastructure.Tests.Manifiesto;

[CollectionDefinition("ManifiestoRepository")]
public class ManifiestoRepositoryCollection : ICollectionFixture<ManifiestoRepositoryFixture>
{
    // xUnit collection fixture — shared MsSqlContainer across all Manifiesto integration test classes
}
