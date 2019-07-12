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
    public class HatarakuDBJournalizingQueryProcessor :
        IHatarakuDBJournalizingQueryProcessor
    {
        private readonly IDbHelper dbHelper;
        public HatarakuDBJournalizingQueryProcessor(IDbHelper dbHelper)
        {
            this.dbHelper = dbHelper;
        }
        public Task<IEnumerable<HatarakuDBData>> ExtractAsync(JournalizingOption option, CancellationToken token = default(CancellationToken))
        {
            var conditionMatching = (option.OutputAt == null || !option.OutputAt.Any())
                ? " AND m.OUtputAt IS NULL"
                : " AND m.OutputAt IN @OutputAt";
            if (option.RecordedAtFrom.HasValue) conditionMatching += @"
         AND m.RecordedAt   >= @RecordedAtFrom";
            if (option.RecordedAtTo.HasValue) conditionMatching += @"
         AND m.RecordedAt   <= @RecordedAtTo";

            var query = $@"
SELECT b.[InvoiceCode]
     , t.[RecordedAt]
     , b.[AssignmentAmount]
     , b.[BillingAmount]
FROM (
      SELECT m.BillingId
           , MAX( m.RecordedAt ) [RecordedAt]
        FROM MatchingHeader mh
       INNER JOIN Matching m
          ON mh.Id            = m.MatchingHeaderId
         AND mh.CompanyId     = @CompanyId
         AND mh.CurrencyId    = @CurrencyId
         AND mh.Approved      = 1
{conditionMatching}
       GROUP BY m.BillingId
       ) t
 INNER JOIN Billing b       ON b.Id     = t.BillingId
 ORDER BY b.Id";
            return dbHelper.GetItemsAsync<HatarakuDBData>(query, option, token);
        }

        public Task<IEnumerable<JournalizingSummary>> GetSummaryAsync(JournalizingOption option, CancellationToken token = default(CancellationToken))
        {
            var matchingCondition = (option.IsOutputted)
                ? @"    AND m.OutputAt     IS NOT NULL"
                : @"    AND m.OutputAt     IS     NULL";
            if (option.RecordedAtFrom.HasValue) matchingCondition += @"
         AND m.RecordedAt       >= @RecordedAtFrom";
            if (option.RecordedAtTo.HasValue) matchingCondition += @"
         AND m.RecordedAt       <= @RecordedAtTo";
            var query = $@"
SELECT t.OutputAt
     , COUNT(*)             [Count]
     , SUM( t.Amount   )    [Amount]
     , MAX( t.UpdateAt )    [UpdateAt]
FROM (
      SELECT m.OutputAt
           , m.BillingId
           , SUM( m.Amount   ) [Amount]
           , MAX( m.UpdateAt ) [UpdateAt]
        FROM MatchingHeader mh
       INNER JOIN Matching m
          ON mh.Id            = m.MatchingHeaderId
         AND mh.CompanyId     = @CompanyId
         AND mh.CurrencyId    = @CurrencyId
         AND mh.Approved      = 1
{matchingCondition}
       GROUP BY
             m.OutputAt
           , m.BillingId
       ) t
 GROUP BY OutputAt
 ORDER BY OutputAt DESC";
            return dbHelper.GetItemsAsync<JournalizingSummary>(query, option, token);
        }
    }
}
