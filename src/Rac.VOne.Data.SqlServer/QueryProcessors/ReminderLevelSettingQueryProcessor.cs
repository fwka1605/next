using Rac.VOne.Web.Models;
using Rac.VOne.Data.QueryProcessors;
using System.Threading;
using System.Threading.Tasks;

namespace Rac.VOne.Data.SqlServer.QueryProcessors
{
    public class ReminderLevelSettingQueryProcessor :
        IReminderLevelSettingQueryProcessor,
        IAddReminderLevelSettingQueryProcessor,
        IDeleteReminderLevelSettingQueryProcessor
    {
        private readonly IDbHelper dbHelper;

        public ReminderLevelSettingQueryProcessor(IDbHelper dbHelper)
        {
            this.dbHelper = dbHelper;
        }

        public async Task<bool> ExistReminderTemplateAsync(int ReminderTemplateId, CancellationToken token = default(CancellationToken))
        {
            var query = @"
SELECT TOP(1) 1
 FROM ReminderLevelSetting
WHERE
 ReminderTemplateId = @ReminderTemplateId";
            return (await dbHelper.ExecuteAsync<int?>(query, new { ReminderTemplateId }, token)).HasValue;
        }

        public Task<int> GetMaxLevelAsync(int CompanyId, CancellationToken token = default(CancellationToken))
        {
            var query = @"
SELECT ISNULL(MAX(ReminderLevel), 0) + 1
 FROM ReminderLevelSetting
WHERE 
 CompanyId = @CompanyId";
            return dbHelper.ExecuteAsync<int>(query, new { CompanyId }, token);
        }

        public Task<ReminderLevelSetting> GetItemByLevelAsync(int CompanyId, int ReminderLevel, CancellationToken token = default(CancellationToken))
        {
            var query = @"
SELECT rt.Code [ReminderTemplateCode]
      ,rt.Name [ReminderTemplateName]
      ,rl.ArrearDays
      ,rl.ReminderTemplateId
  FROM ReminderLevelSetting rl
LEFT JOIN ReminderTemplateSetting rt ON rt.Id = rl.ReminderTemplateId
WHERE 
    rl.CompanyId     = @CompanyId
AND rl.ReminderLevel = @ReminderLevel";
            return dbHelper.ExecuteAsync<ReminderLevelSetting>(query, new { CompanyId, ReminderLevel }, token);
        }

        public Task<ReminderLevelSetting> SaveAsync(ReminderLevelSetting ReminderLevelSetting, CancellationToken token = default(CancellationToken))
        {
            #region merge query
            var query = @"
MERGE INTO ReminderLevelSetting target
USING (
    SELECT @CompanyId       [CompanyId]
         , @ReminderLevel   [ReminderLevel]
) source
ON    (
        target.CompanyId        = source.CompanyId
    AND target.ReminderLevel    = source.ReminderLevel
)
WHEN MATCHED THEN
    UPDATE SET 
         ReminderTemplateId = @ReminderTemplateId
        ,ArrearDays         = @ArrearDays
        ,UpdateBy           = @UpdateBy
        ,UpdateAt           = GETDATE()
WHEN NOT MATCHED THEN 
    INSERT (CompanyId, ReminderLevel, ReminderTemplateId, ArrearDays, CreateBy, CreateAt, UpdateBy, UpdateAt) 
    VALUES (@CompanyId, @ReminderLevel, @ReminderTemplateId, @ArrearDays, @UpdateBy, GETDATE(), @UpdateBy, GETDATE()) 
OUTPUT inserted.*; ";
            #endregion

            return dbHelper.ExecuteAsync<ReminderLevelSetting>(query, ReminderLevelSetting, token);
        }

        public Task<int> DeleteAsync(int CompanyId, int ReminderLevel, CancellationToken token = default(CancellationToken))
        {
            var query = @"
DELETE FROM ReminderLevelSetting
 WHERE CompanyId        = @CompanyId 
   AND ReminderLevel    = @ReminderLevel";
            return dbHelper.ExecuteAsync(query, new { CompanyId, ReminderLevel }, token);
        }

    }
}
