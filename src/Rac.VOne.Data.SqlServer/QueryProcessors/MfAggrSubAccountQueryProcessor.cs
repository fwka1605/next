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
    public class MfAggrSubAccountQueryProcessor :
        IAddMfAggrSubAccountQueryProcessor,
        IMfAggrSubAccountQueryProcessor
    {
        private readonly IDbHelper dbHelper;
        public MfAggrSubAccountQueryProcessor(IDbHelper dbHelper)
        {
            this.dbHelper = dbHelper;
        }

        public Task<MfAggrSubAccount> AddAsync(MfAggrSubAccount subAccount, CancellationToken token = default(CancellationToken))
        {
            var query = @"
MERGE INTO MfAggrSubAccount target
USING (
    SELECT @Id [Id]
) source ON (
        target.Id               = source.Id
)
WHEN MATCHED THEN
    UPDATE SET
            AccountId           = @AccountId
         ,  Name                = @Name
         ,  AccountTypeName     = @AccountTypeName
         ,  AccountTypeId       = @AccountTypeId
         ,  AccountNumber       = @AccountNumber
         ,  BranchCode          = @BranchCode
         ,  ReceiptCategoryId   = @ReceiptCategoryId
         ,  SectionId           = @SectionId
WHEN NOT MATCHED THEN
    INSERT
         (  Id
         ,  AccountId
         ,  Name
         ,  AccountTypeName
         ,  AccountTypeId
         ,  AccountNumber
         ,  BranchCode
         ,  ReceiptCategoryId
         ,  SectionId
         )
    VALUES
         ( @Id
         , @AccountId
         , @Name
         , @AccountTypeName
         , @AccountTypeId
         , @AccountNumber
         , @BranchCode
         , @ReceiptCategoryId
         , @SectionId
         )
OUTPUT inserted.*;";
            return dbHelper.ExecuteAsync<MfAggrSubAccount>(query, subAccount, token);
        }

        public Task<IEnumerable<MfAggrSubAccount>> GetAsync(CancellationToken token = default(CancellationToken))
        {
            var query = @"
SELECT      *
FROM        [dbo].[MfAggrSubAccount]
ORDER BY    [Id]    ASC";
            return dbHelper.GetItemsAsync<MfAggrSubAccount>(query, cancellatioToken: token);
        }
    }
}
