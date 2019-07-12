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
    public class NettingQueryProcessor :
        INettingQueryProcessor,
        IAddNettingQueryProcessor,
        IUpdateNettingQueryProcessor
    {
        private readonly IDbHelper dbHelper;

        public NettingQueryProcessor(IDbHelper dbHelper)
        {
            this.dbHelper = dbHelper;
        }

        public Task<Netting> GetByIdAsync(long Id, CancellationToken token = default(CancellationToken))
        {
            var query = @"
SELECT n.*
     , cs.Kana [CustomerKana]
  FROM Netting n
  LEFT JOIN Customer cs  ON cs.Id = n.CustomerId
 WHERE n.Id = @Id";
            return dbHelper.ExecuteAsync<Netting>(query, new { Id }, token);
        }
        public Task<IEnumerable<Netting>> GetByMatchingHeaderIdAsync(long headerId, CancellationToken token = default(CancellationToken))
        {
            var query = @"
SELECT n.*
     , cs.[Kana] [CustomerKana]
  FROM (
       SELECT DISTINCT m.ReceiptId
         FROM Matching m
        WHERE m.MatchingHeaderId                = @headerId
       ) m
 INNER JOIN Netting n       ON n.ReceiptId      = m.ReceiptId
                           AND n.AssignmentFlag = 1
  LEFT JOIN Customer cs     ON cs.Id            = n.CustomerId
";
            return dbHelper.GetItemsAsync<Netting>(query, new { headerId }, token);
        }

        public async Task<bool> ExistReceiptCategoryAsync(int CategoryId, CancellationToken token = default(CancellationToken))
        {
            var query = @"SELECT TOP 1 1 FROM Netting 
                            WHERE ReceiptCategoryId = @CategoryId ";
            return (await dbHelper.ExecuteAsync<int?>(query, new { CategoryId }, token)).HasValue;
        }

        public async Task<bool> ExistCustomerAsync(int CustomerId, CancellationToken token = default(CancellationToken))
        {
            var query = @"SELECT TOP 1 1 FROM Netting 
                            WHERE CustomerId = @CustomerId ";
            return (await dbHelper.ExecuteAsync<int?>(query, new { CustomerId }, token)).HasValue;
        }
        public async Task<bool> ExistSectionAsync(int SectionId, CancellationToken token = default(CancellationToken))
        {
            var query = @"SELECT TOP 1  1 FROM Netting
                             WHERE SectionId = @SectionId";
            return (await dbHelper.ExecuteAsync<int?>(query, new { SectionId }, token)).HasValue;
        }

        public async Task<bool> ExistCurrencyAsync(int CurrencyId, CancellationToken token = default(CancellationToken))
        {
            var query = @"SELECT 1
                    WHERE EXISTS( SELECT 1
                    FROM Netting
                    WHERE CurrencyId = @CurrencyId)";
            return (await dbHelper.ExecuteAsync<int?>(query, new { CurrencyId }, token)).HasValue;
        }

        public Task<Netting> SaveAsync(Netting Netting, CancellationToken token = default(CancellationToken))
        {
            var query = @"
INSERT INTO Netting
(   CompanyId,
    CurrencyId,
    CustomerId,
    ReceiptCategoryId,  
    SectionId, 
    RecordedAt,
    DueAt,
    Amount,
    AssignmentFlag,
    Note,
    ReceiptMemo
)
OUTPUT inserted.*
VALUES
(
    @CompanyId,
    @CurrencyId,
    @CustomerId,
    @ReceiptCategoryId,
    @SectionId,
    @RecordedAt,
    @DueAt,
    @Amount,
    @AssignmentFlag,
    @Note,
    @ReceiptMemo
);";
            return dbHelper.ExecuteAsync<Netting>(query, Netting, token);
        }
        public Task<Netting> UpdateMatchingNettingAsync(int CompanyId, long ReceiptId, long Id, int CancelFlg, CancellationToken token = default(CancellationToken))
        {
            var query = @"
UPDATE Netting
   SET ReceiptId        = CASE WHEN @CancelFlg = 0 THEN @ReceiptId  ELSE NULL   END
     , AssignmentFlag   = CASE WHEN @CancelFlg = 0 THEN 1           ELSE 0      END
OUTPUT inserted.*
 WHERE Id           = @Id
   AND CompanyId    = @CompanyId";
            return dbHelper.ExecuteAsync<Netting>(query, new
            {
                ReceiptId,
                Id,
                CompanyId,
                CancelFlg
            }, token);
        }

    }
}
