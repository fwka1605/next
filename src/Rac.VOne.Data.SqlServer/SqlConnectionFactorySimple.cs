using System.Data;
using System.Data.SqlClient;

namespace Rac.VOne.Data.SqlServer
{
    public class SqlConnectionFactorySimple : IConnectionFactory
    {
        private readonly string _connectionString;

        public SqlConnectionFactorySimple(string connectionString)
        {
            _connectionString = connectionString;
        }

        public IDbConnection CreateConnection()
        {
            return new SqlConnection(_connectionString);
        }
    }
}
