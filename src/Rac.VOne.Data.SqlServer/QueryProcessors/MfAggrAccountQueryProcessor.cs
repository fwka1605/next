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
    public class MfAggrAccountQueryProcessor :
        IAddMfAggrAccountQueryProcessor,
        IMfAggrAccountQueryProcessor
    {
        private readonly IDbHelper dbHelper;
        public MfAggrAccountQueryProcessor(IDbHelper dbHelper)
        {
            this.dbHelper = dbHelper;
        }

        public Task<MfAggrAccount> AddAsync(MfAggrAccount account, CancellationToken token = default(CancellationToken))
        {
            var query = @"
MERGE INTO [dbo].[MfAggrAccount] target
USING (
    SELECT @Id [Id]
) source ON (
        target.Id                   = source.Id
)
WHEN MATCHED THEN
    UPDATE SET
            DisplayName             = @DisplayName
         ,  LastAggregatedAt        = @LastAggregatedAt
         ,  LastLoginAt             = @LastLoginAt
         ,  LastSucceededAt         = @LastSucceededAt
         ,  AggregationStartDate    = @AggregationStartDate
         ,  Status                  = @Status
         ,  IsSuspended             = @IsSuspended
         ,  BankCode                = @BankCode
WHEN NOT MATCHED THEN
    INSERT
         (  Id
         ,  DisplayName
         ,  LastAggregatedAt
         ,  LastLoginAt
         ,  LastSucceededAt
         ,  AggregationStartDate
         ,  Status
         ,  IsSuspended
         ,  BankCode
         )
    VALUES
         ( @Id
         , @DisplayName
         , @LastAggregatedAt
         , @LastLoginAt
         , @LastSucceededAt
         , @AggregationStartDate
         , @Status
         , @IsSuspended
         , @BankCode
         )
OUTPUT inserted.*;";
            return dbHelper.ExecuteAsync<MfAggrAccount>(query, account, token);
        }

        public Task<IEnumerable<MfAggrAccount>> GetAsync(CancellationToken token = default(CancellationToken))
        {
            var query = @"
SELECT      *
FROM        [dbo].[MfAggrAccount]
ORDER BY    [Id] ASC
";
            return dbHelper.GetItemsAsync<MfAggrAccount>(query, cancellatioToken: token);
        }
    }
}
