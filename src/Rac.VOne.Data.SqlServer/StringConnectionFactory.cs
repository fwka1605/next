using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rac.VOne.Data.SqlServer
{
    public class StringConnectionFactory : IStringConnectionFactory
    {
        public IConnectionFactory Create(string connectionString)
        {
            return new SqlConnectionFactorySimple(connectionString);
        }
    }
}
