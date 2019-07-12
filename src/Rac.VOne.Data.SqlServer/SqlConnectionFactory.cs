using System.Data;
using System.Data.SqlClient;
using Rac.VOne.Common;

namespace Rac.VOne.Data.SqlServer
{
    public class SqlConnectionFactory : IConnectionFactory
    {
        private readonly IConnectionString _connectionString;
        public SqlConnectionFactory(IConnectionString connectionString)
        {
            _connectionString = connectionString;
        }

        public IDbConnection CreateConnection()
        {
            var connectionString = _connectionString.ConnectionString;
            return new SqlConnection(connectionString);
        }
    }
}
