using System;
using Rac.VOne.Web.Models;
using Rac.VOne.Data.QueryProcessors;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Rac.VOne.Data.SqlServer.QueryProcessors
{
    public  class BillingMemoQueryProcessor :
        IBillingMemoQueryProcessor,
        IUpdateBillingMemoQueryProcessor,
        IDeleteBillingMemoQueryProcessor
    {
        private readonly IDbHelper dbHelper;

        public BillingMemoQueryProcessor(IDbHelper dbHelper)
        {
            this.dbHelper = dbHelper;
        }

        public Task<string> GetMemoAsync(long BillingId, CancellationToken token = default(CancellationToken))
        {
            var query = @"SElECT Memo FROM BillingMemo WHERE BillingId = @BillingId";
            return dbHelper.ExecuteAsync<string>(query, new { BillingId }, token);
        }

        public Task<int> SaveMemoAsync(long BillingId, string BillingMemo, CancellationToken token = default(CancellationToken))
        {
            #region merge query
            var query = @"
MERGE INTO BillingMemo AS Bm 
USING ( 
    SELECT 
        @BillingId AS BillingId 
        ,@BillingMemo AS Memo 

) AS Target 
ON ( 
    Bm.BillingId = @BillingId 
   
) 
WHEN MATCHED THEN 
    UPDATE SET 
    Memo = @BillingMemo 

WHEN NOT MATCHED THEN 
    INSERT (BillingId, Memo) 
    VALUES (@BillingId, @BillingMemo)  
OUTPUT inserted.*;";
            #endregion
            return dbHelper.ExecuteAsync(query, new { BillingId, BillingMemo }, token);
        }

        public Task<int> DeleteAsync(long BillingId, CancellationToken token = default(CancellationToken))
        {
            var query = @"DELETE BillingMemo WHERE BillingId = @BillingId";
            return dbHelper.ExecuteAsync(query, new { BillingId }, token);
        }
    }
}
