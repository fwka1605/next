using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rac.VOne.Web.Models;
using Rac.VOne.Data.QueryProcessors;
using System.Threading;

namespace Rac.VOne.Data.SqlServer.QueryProcessors
{
    public class ReceiptSectionTransferQueryProcessor :
        IReceiptSectionTransferQueryProcessor,
        IDeleteReceiptSectionTransferQueryProcessor,
        IAddReceiptSectionTransferQueryProcessor,
        IUpdateReceiptSectionTransferQueryProcessor
    {
        private readonly IDbHelper dbHelper;

        public ReceiptSectionTransferQueryProcessor(IDbHelper dbHelper)
        {
            this.dbHelper = dbHelper;
        }


        public Task<int> DeleteAsync(ReceiptSectionTransfer ReceiptSectionTransfer, CancellationToken token = default(CancellationToken))
        {
            var query = @"
DELETE 
  FROM ReceiptSectionTransfer 
 WHERE SourceReceiptId      = @SourceReceiptId 
   AND DestinationReceiptId = @DestinationReceiptId";
            return dbHelper.ExecuteAsync(query, ReceiptSectionTransfer, token);
        }


        public Task<ReceiptSectionTransfer> SaveAsync(ReceiptSectionTransfer ReceiptSectionTransfer, CancellationToken token = default(CancellationToken))
        {
            var query = @"
INSERT INTO ReceiptSectionTransfer
           (SourceReceiptId
          ,DestinationReceiptId
          ,SourceSectionId
          ,DestinationSectionId
          ,SourceAmount
          ,DestinationAmount
          ,PrintFlag
          ,CreateBy
          ,CreateAt
          ,UpdateBy
          ,UpdateAt
    )
 OUTPUT inserted.* 
 VALUES
          (@SourceReceiptId
          ,@DestinationReceiptId
          ,@SourceSectionId
          ,@DestinationSectionId
          ,@SourceAmount
          ,@DestinationAmount
          ,@PrintFlag
          ,@CreateBy
          , GETDATE()
          ,@UpdateBy
          , GETDATE()
    )";
            return dbHelper.ExecuteAsync<ReceiptSectionTransfer>(query, ReceiptSectionTransfer, token);
        }

        public Task<IEnumerable<ReceiptSectionTransfer>> GetReceiptSectionTransferForPrintAsync(int CompanyId, CancellationToken token = default(CancellationToken))
        {
            var query = @"
SELECT
  rs.Id
, rs.RecordedAt
, rs.DueAt
, c.Code [ReceiptCategoryCode]
, c.Name [ReceiptCategoryName]
, rs.InputType
, rs.PayerName
, rs.Note1
, rst.SourceReceiptId
, rst.SourceAmount
, ss.Code [SourceSectionCode]
, ss.Name [SourceSectionName]
, sd.Code [DestinationSectionCode]
, sd.Name [DestinationSectionName]
, rst.DestinationAmount
, rst.CreateAt
, rmd.Memo
, u.Code [LoginUserCode]
, u.Name [LoginUserName]
, ccy.Code [CurrencyCode]
, ccy.Precision
FROM ReceiptSectionTransfer rst
INNER JOIN Receipt rs           ON rs.Id        = rst.SourceReceiptId
INNER JOIN Receipt rd           ON rd.Id        = rst.DestinationReceiptId
INNER JOIN Category c           ON c.Id         = rs.ReceiptCategoryId
INNER JOIN Section ss           ON ss.Id        = rst.SourceSectionId
INNER JOIN Section sd           ON sd.Id        = rst.DestinationSectionId
INNER JOIN Currency ccy         ON ccy.Id       = rs.CurrencyId
LEFT JOIN ReceiptMemo rmd       ON rd.Id        = rmd.ReceiptId
LEFT JOIN LoginUser u           ON u.Id         = rst.CreateBy
WHERE rs.CompanyId  = @CompanyId
  AND rst.PrintFlag     = 0
ORDER BY ccy.Id, rs.Id
";
            return dbHelper.GetItemsAsync<ReceiptSectionTransfer>(query, new { CompanyId }, token);
        }

        public Task<IEnumerable<ReceiptSectionTransfer>> UpdateReceiptSectionTransferPrintFlagAsync(int CompanyId, CancellationToken token = default(CancellationToken))
        {
            var query = @"
UPDATE rst
   SET rst.PrintFlag    = 1
OUTPUT inserted.*
  FROM ReceiptSectionTransfer rst
 INNER JOIN Receipt rs      ON rs.Id        = rst.SourceReceiptId
                           AND rs.CompanyId = @CompanyId
 INNER JOIN Receipt rd      ON rd.Id        = rst.DestinationReceiptId
                           AND rd.CompanyId = rs.CompanyId
 WHERE rst.PrintFlag    = 0
";
            return dbHelper.GetItemsAsync<ReceiptSectionTransfer>(query, new { CompanyId }, token);
        }

        public Task<ReceiptSectionTransfer> GetItemByReceiptIdAsync(ReceiptSectionTransfer ReceiptSectionTransfer, CancellationToken token = default(CancellationToken))
        {
            var query = @"
SELECT * 
  FROM ReceiptSectionTransfer 
 WHERE SourceReceiptId      = @ReceiptId 
   AND DestinationReceiptId = @ReceiptId
                            ";
            return dbHelper.ExecuteAsync<ReceiptSectionTransfer>(query, ReceiptSectionTransfer, token);
        }

        public Task<ReceiptSectionTransfer> UpdateDestinationSectionAsync(int DestinationSectionId, int LoginUserId, long SourceReceiptId, long DestinationReceiptId, CancellationToken token = default(CancellationToken))
        {
            var query = @"
UPDATE　ReceiptSectionTransfer
   SET DestinationSectionId = @DestinationSectionId
     , UpdateAt             = GETDATE()
     , UpdateBy             = @LoginUserId
OUTPUT inserted.*
 WHERE SourceReceiptId      = @SourceReceiptId
   AND DestinationReceiptId = @DestinationReceiptId ";
            return dbHelper.ExecuteAsync<ReceiptSectionTransfer>(query, new { DestinationSectionId,LoginUserId, SourceReceiptId,DestinationReceiptId }, token);
        }

        public Task<IEnumerable<ReceiptSectionTransfer>> GetItemsAsync(ReceiptSectionTransfer ReceiptSectionTransfer, CancellationToken token = default(CancellationToken))
        {
            var query = @"
SELECT * 
  FROM ReceiptSectionTransfer
 WHERE 1 = 1
";
            var whereCondition = new StringBuilder();
            if (ReceiptSectionTransfer.SourceReceiptId != 0)
            {
                whereCondition.AppendLine(@"
                     AND SourceReceiptId = @SourceReceiptId AND SourceReceiptId <> DestinationReceiptId");
            }
            if (ReceiptSectionTransfer.DestinationReceiptId != 0)
            {
                whereCondition.AppendLine(@"
                     AND DestinationReceiptId = @DestinationReceiptId");
            }
            query += whereCondition.ToString();
            return dbHelper.GetItemsAsync<ReceiptSectionTransfer>(query, ReceiptSectionTransfer, token);
        }


    }
}
