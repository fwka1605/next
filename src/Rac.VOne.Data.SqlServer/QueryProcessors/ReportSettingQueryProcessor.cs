using System.Collections.Generic;
using Rac.VOne.Data.QueryProcessors;
using Rac.VOne.Web.Models;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Rac.VOne.Data.SqlServer.QueryProcessors
{
    public class ReportSettingQueryProcessor :
        IReportSettingQueryProcessor,
        IAddReportSettingQueryProcessor,
        IDeleteReportSettingQueryProcessor
    {
        private readonly IDbHelper dbHelper;

        public ReportSettingQueryProcessor(IDbHelper dbHelper)
        {
            this.dbHelper = dbHelper;
        }

        public Task<IEnumerable<ReportSetting>> GetAsync(int CompanyId, string ReportId, CancellationToken token = default(CancellationToken))
        {
            var query = @"
SELECT COALESCE(rs.CompanyId, @CompanyId) [CompanyId]
     , rsb.ReportId
     , rsb.DisplayOrder
     , rsb.IsText
     , COALESCE(rsb.Alias, sd.[Description]) [Caption]
     , rsb.ItemId
     , COALESCE(rs.ItemKey, rsb.ItemKey) [ItemKey]
     , s.ItemValue
 FROM ReportSettingBase rsb
 LEFT JOIN ReportSetting rs
   ON rsb.ReportId     = rs.ReportId
  AND rsb.DisplayOrder = rs.DisplayOrder
  AND  rs.CompanyId    = @CompanyId
 LEFT JOIN SettingDefinition sd
   ON sd.ItemId        = rsb.ItemId
 LEFT JOIN Setting s
   ON s.ItemId  = sd.ItemId
  AND s.ItemKey = COALESCE(rs.ItemKey, rsb.ItemKey)
WHERE rsb.ReportId = @ReportId
ORDER BY rsb.DisplayOrder";
            return dbHelper.GetItemsAsync<ReportSetting>(query, new { CompanyId, ReportId }, token);
        }

        public Task<ReportSetting> SaveAsync(ReportSetting setting, CancellationToken token = default(CancellationToken))
        {
            var query = @"
INSERT INTO ReportSetting
( CompanyId
, ReportId
, DisplayOrder
, ItemId
, ItemKey )
OUTPUT inserted.*
VALUES
(@CompanyId
,@ReportId
,@DisplayOrder
,@ItemId
,@ItemKey )";

            return dbHelper.ExecuteAsync<ReportSetting>(query, setting, token);
        }

        public Task<int> DeleteAsync(int CompanyId, string ReportId, CancellationToken token = default(CancellationToken))
        {
            var query = @"
DELETE ReportSetting 
 WHERE CompanyId    = @CompanyId 
   AND ReportId     = @ReportId";

            return dbHelper.ExecuteAsync(query, new { CompanyId, ReportId }, token);
        }

    }
}
