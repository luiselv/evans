namespace EVANS.Application.GuiaRemision.Ports;

public interface IUnitOfWork : IDisposable
{
    void Commit();
}
