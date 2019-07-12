using Rac.VOne.Web.Models;
using Rac.VOne.Data.QueryProcessors;
using System.Text;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Rac.VOne.Data.SqlServer.QueryProcessors
{
    public class ReminderSummarySettingQueryProcessor :
        IAddReminderSummarySettingQueryProcessor,
        IReminderSummarySettingQueryProcessor
    {
        private readonly IDbHelper dbHelper;
        public ReminderSummarySettingQueryProcessor(IDbHelper dbHelper)
        {
            this.dbHelper = dbHelper;
        }

        public async Task<IEnumerable<ReminderSummarySetting>> GetAsync(int CompanyId, int UseForeignCurrency, CancellationToken token = default(CancellationToken))
        {
            var exists = (await dbHelper.ExecuteAsync<int?>(@"
SELECT 1
 WHERE EXISTS (
       SELECT *
         FROM [dbo].[ReminderSummarySetting]
        WHERE CompanyId             = @CompanyId )", new { CompanyId })).HasValue;


            var query = new StringBuilder();
            if (!exists)
            {
                query.Append(@"
SELECT 0 [Id]
     , b.[ColumnName]
     , b.[ColumnNameJp]
     , b.[DisplayOrder]
     , b.[Available]
  FROM [dbo].[ReminderSummarySettingBase] b ");
                if (UseForeignCurrency == 0)
                {
                    query.Append(@"
 WHERE   b.[ColumnName] <> 'CurrencyCode' ");
                }
                query.Append(@"
 ORDER BY b.[DisplayOrder] ASC");

            }
            else
            {
                query.Append(@"
SELECT *
  FROM [dbo].[ReminderSummarySetting]
 WHERE [CompanyId]              = @CompanyId ");
                if (UseForeignCurrency == 0)
                {
                    query.Append(@"
   AND [ColumnName]              <> 'CurrencyCode' ");
                }
                query.Append(@"
 ORDER BY [DisplayOrder] ASC");
            }
            return await dbHelper.GetItemsAsync<ReminderSummarySetting>(query.ToString(), new { CompanyId }, token);
        }

        public Task<ReminderSummarySetting> SaveAsync(ReminderSummarySetting setting, CancellationToken token = default(CancellationToken))
        {
            #region merge query
            var query = @"
MERGE INTO ReminderSummarySetting target
USING (
    SELECT @CompanyId     [CompanyId]
         , @ColumnName    [ColumnName]
) source
ON    (
        target.CompanyId     = source.CompanyId
    AND target.ColumnName    = source.ColumnName
)
WHEN MATCHED THEN
    UPDATE SET 
         Available    = @Available
        ,UpdateBy     = @UpdateBy
        ,UpdateAt     = GETDATE()
WHEN NOT MATCHED THEN 
    INSERT (CompanyId, ColumnName, ColumnNameJp, DisplayOrder, Available, CreateBy, CreateAt, UpdateBy, UpdateAt) 
    VALUES (@CompanyId, @ColumnName, @ColumnNameJp, @DisplayOrder, @Available, @UpdateBy, GETDATE(), @UpdateBy, GETDATE()) 
OUTPUT inserted.*; ";
            #endregion

            return dbHelper.ExecuteAsync<ReminderSummarySetting>(query, setting, token);
        }
    }
}
