using System;
using Rac.VOne.Web.Models;
using Rac.VOne.Data.QueryProcessors;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Rac.VOne.Data.SqlServer.QueryProcessors
{
    public class NettingSearchQueryProcessor : INettingSearchQueryProcessor
    {
        private readonly IDbHelper dbHelper;

        public NettingSearchQueryProcessor(IDbHelper dbHelper)
        {
            this.dbHelper = dbHelper;
        }

        public Task<IEnumerable<Netting>> GetAsync(int CompanyId,int CustomerId, int CurrencyId, CancellationToken token = default(CancellationToken))
        {
            var query = @"
SELECT 
        net.Id,
        net.CompanyId as CompanyId,
        net.CustomerId as CustomerId,
        net.CurrencyId as CurrencyId,
        net.RecordedAt as RecordedAt,
        net.Amount as Amount,
        cate.Code as CategoryCode,
        cate.UseAdvanceReceived  as UseAdvanceReceived,
        cate.Code + ' : ' + cate.Name as CategoryCode, 
        c.Name as CustomerName,
        c.Kana as CustomerKana

FROM Netting as net
        LEFT OUTER JOIN Customer as c      ON net.CustomerId = c.Id
        LEFT OUTER JOIN Currency as cur    ON net.CurrencyId = cur.Id
        LEFT OUTER JOIN Category as cate   ON net.ReceiptCategoryId = cate.Id

WHERE net.CompanyId     = @CompanyId
AND   net.CustomerId    = @CustomerId
AND   net.CurrencyId    = @CurrencyId
AND   net.AssignmentFlag <> 2";
            return dbHelper.GetItemsAsync<Netting> (query, new { CompanyId, CustomerId , CurrencyId }, token);
        }
    }
}
