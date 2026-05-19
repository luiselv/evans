namespace EVANS.Infrastructure.Tests.Comprobante;

[CollectionDefinition("ComprobanteRepository")]
public class ComprobanteRepositoryCollection : ICollectionFixture<ComprobanteRepositoryFixture>
{
    // xUnit collection fixture — shared MsSqlContainer across all Comprobante integration test classes
}
