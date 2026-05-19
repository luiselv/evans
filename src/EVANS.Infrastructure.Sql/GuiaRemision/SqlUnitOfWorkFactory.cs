using EVANS.Application.GuiaRemision.Ports;
using EVANS.Infrastructure.Sql.Connections;

namespace EVANS.Infrastructure.Sql.GuiaRemision;

/// <summary>
/// Creates SqlUnitOfWork instances for a given year.
/// Registered as Transient — each call to Create() produces a new independent unit of work.
/// </summary>
public sealed class SqlUnitOfWorkFactory(IYearlyTransactionalConnectionFactory connectionFactory) : IUnitOfWorkFactory
{
    public IUnitOfWork Create(int year) => new SqlUnitOfWork(connectionFactory, year);
}
