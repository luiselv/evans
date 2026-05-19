namespace EVANS.Application.GuiaRemision.Ports;

public interface IUnitOfWorkFactory
{
    IUnitOfWork Create(int year);
}
