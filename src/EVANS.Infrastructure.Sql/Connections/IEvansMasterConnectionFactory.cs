using Microsoft.Data.SqlClient;

namespace EVANS.Infrastructure.Sql.Connections;

public interface IEvansMasterConnectionFactory
{
    SqlConnection Create();
}
