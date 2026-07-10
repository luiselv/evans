namespace EVANS.Application.Identidad.Ports;

public interface IYearlyDatabaseProvisioner
{
    Task CreateYearAsync(int year, CancellationToken cancellationToken = default);
}
