using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Rac.VOne.Web.Models;
using Rac.VOne.Data.QueryProcessors;

namespace Rac.VOne.Data.SqlServer.QueryProcessors
{
    public class AccountTransferLogQueryProcessor :
        IAccountTransferLogQueryProcessor,
        IAddAccountTransferLogQueryProcessor
    {
        private readonly IDbHelper dbHelper;

        public AccountTransferLogQueryProcessor(IDbHelper dbHelper)
        {
            this.dbHelper = dbHelper;
        }
        public Task<AccountTransferLog> AddAsync(AccountTransferLog AccountTransferLog, CancellationToken token = default(CancellationToken))
        {
            var query = @"
INSERT INTO [dbo].[AccountTransferLog]
     ( [CompanyId]
     , [CollectCategoryId]
     , [PaymentAgencyId]
     , [RequestDate]
     , [DueAt]
     , [OutputCount]
     , [OutputAmount]
     , [CreateBy]
     , [CreateAt] )
OUTPUT inserted.*
VALUES
     ( @CompanyId
     , @CollectCategoryId
     , @PaymentAgencyId
     , CAST(GETDATE() AS DATE)
     , @DueAt
     , @OutputCount
     , @OutputAmount
     , @CreateBy
     , GETDATE() )
";
            return dbHelper.ExecuteAsync<AccountTransferLog>(query, AccountTransferLog, token);
        }

        public Task<IEnumerable<AccountTransferLog>> GetAsync(int CompanyId, CancellationToken token = default(CancellationToken))
        {
            var query = @"
SELECT atl.*
     , pa.[Code]    [PaymentAgencyCode]
     , pa.[Name]    [PaymentAgencyName]
     , cc.[Code]    [CollectCategoryCode]
     , cc.[Name]    [CollectCategoryName]
  FROM [dbo].[AccountTransferLog] atl
  LEFT JOIN [dbo].[PaymentAgency] pa        ON pa.[Id]  = atl.[PaymentAgencyId]
  LEFT JOIN [dbo].[Category] cc             ON cc.[Id]  = atl.[CollectCategoryId]
 WHERE atl.[CompanyId]          = @CompanyId
 ORDER BY atl.[Id]              DESC
";
            return dbHelper.GetItemsAsync<AccountTransferLog>(query, new { CompanyId }, token);
        }
    }
}
