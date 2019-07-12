using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Rac.VOne.Data.QueryProcessors;
using Rac.VOne.Web.Models;

namespace Rac.VOne.Data.SqlServer.QueryProcessors
{
    public class BankAccountTypeQueyProcessor : IBankAccountTypeQueryProcessor
    {
        private readonly IDbHelper dbHelper;

        public BankAccountTypeQueyProcessor(IDbHelper dbHelper)
        {
            this.dbHelper = dbHelper;
        }
        public Task<IEnumerable<BankAccountType>> GetAsync(CancellationToken token = default(CancellationToken))
        {
            var query = @"SELECT * FROM [dbo].[BankAccountType]";
            return dbHelper.GetItemsAsync<BankAccountType>(query);
        }
    }
}
