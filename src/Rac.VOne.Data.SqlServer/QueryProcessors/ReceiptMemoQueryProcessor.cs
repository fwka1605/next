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
    public class ReceiptMemoQueryProcessor :
        IReceiptMemoQueryProcessor,
        IAddReceiptMemoQueryProcessor,
        IDeleteReceiptMemoQueryProcessor
    {
        private readonly IDbHelper dbHelper;

        public ReceiptMemoQueryProcessor(IDbHelper dbHelper)
        {
            this.dbHelper = dbHelper;
        }

        public Task<ReceiptMemo> GetAsync(long ReceiptId, CancellationToken token = default(CancellationToken))
        {
            var query = @"SElECT * FROM ReceiptMemo WHERE ReceiptId = @ReceiptId";
            return dbHelper.ExecuteAsync<ReceiptMemo>(query, new { ReceiptId }, token);
        }

        public async Task<IEnumerable<ReceiptMemo>> GetItemsAsync(IEnumerable<long> receiptIds, CancellationToken token = default(CancellationToken))
        {
            if (!(receiptIds?.Any() ?? false))
                return Enumerable.Empty<ReceiptMemo>();

            var query = @"
SELECT * FROM ReceiptMemo WHERE ReceiptId IN (SELECT Id FROM @receiptIds)";
            return await dbHelper.GetItemsAsync<ReceiptMemo>(query, new { receiptIds = receiptIds.GetTableParameter() }, token);
        }

        private string GetQueryMergeIntoReceiptMemo() => @"
MERGE INTO ReceiptMemo AS Rm 
USING ( 
    SELECT 
        @ReceiptId AS ReceiptId 
        ,@Memo AS Memo 
) AS Target 
ON ( 
    Rm.ReceiptId = @ReceiptId 
) 
WHEN MATCHED THEN 
    UPDATE SET 
    Memo = @Memo 
WHEN NOT MATCHED THEN 
    INSERT (ReceiptId, Memo)
    VALUES (@ReceiptId, @Memo)
OUTPUT inserted.*;
";

        public Task<ReceiptMemo> SaveAsync(long ReceiptId, string Memo, CancellationToken token = default(CancellationToken))
            => dbHelper.ExecuteAsync<ReceiptMemo>(GetQueryMergeIntoReceiptMemo(), new { ReceiptId, Memo }, token);


        public Task<int> DeleteAsync(long ReceiptId, CancellationToken token = default(CancellationToken))
        {
            var query = @"DELETE ReceiptMemo WHERE ReceiptId = @ReceiptId";
            return dbHelper.ExecuteAsync(query, new { ReceiptId }, token);
        }

    }
}
