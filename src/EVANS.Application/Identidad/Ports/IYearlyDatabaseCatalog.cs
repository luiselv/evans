namespace EVANS.Application.Identidad.Ports;

public interface IYearlyDatabaseCatalog
{
    Task<IReadOnlyList<int>> ListYearsAsync(CancellationToken cancellationToken = default);
}
