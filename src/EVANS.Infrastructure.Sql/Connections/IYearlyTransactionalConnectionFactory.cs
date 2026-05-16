using Microsoft.Data.SqlClient;

namespace EVANS.Infrastructure.Sql.Connections;

public interface IYearlyTransactionalConnectionFactory
{
    SqlConnection Create(int year);
    SqlConnection CreateForCurrentYear();
}
