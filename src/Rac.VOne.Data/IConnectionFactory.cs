using System.Data;

namespace Rac.VOne.Data
{
    public interface IConnectionFactory
    {
        IDbConnection CreateConnection();
    }
}
