using Rac.VOne.Web.Models;
using Rac.VOne.Data.QueryProcessors;
using System.Threading;
using System.Threading.Tasks;

namespace Rac.VOne.Data.SqlServer.QueryProcessors
{
    public class ReminderTemplateSettingQueryProcessor :
        IAddReminderTemplateSettingQueryProcessor
    {
        private readonly IDbHelper dbHelper;
        public ReminderTemplateSettingQueryProcessor(IDbHelper dbHelper)
        {
            this.dbHelper = dbHelper;
        }

        public Task<ReminderTemplateSetting> SaveAsync(ReminderTemplateSetting ReminderTemplateSetting, CancellationToken token = default(CancellationToken))
        {
            #region merge query
            var query = @"
MERGE INTO ReminderTemplateSetting target
USING (
    SELECT @CompanyId   [CompanyId]
         , @Code        [Code]
) source
ON    (
        target.CompanyId    = source.CompanyId
    AND target.Code         = source.Code
)
WHEN MATCHED THEN
    UPDATE SET 
         Name = @Name
        ,Title = @Title
        ,Greeting = @Greeting
        ,MainText = @MainText
        ,SubText = @SubText
        ,Conclusion = @Conclusion
        ,UpdateBy = @UpdateBy
        ,UpdateAt = GETDATE()
WHEN NOT MATCHED THEN 
    INSERT (CompanyId, Code, Name, Title, Greeting, MainText, SubText, Conclusion, CreateBy, CreateAt, UpdateBy, UpdateAt) 
    VALUES (@CompanyId, @Code, @Name, @Title, @Greeting, @MainText, @SubText, @Conclusion, @UpdateBy, GETDATE(), @UpdateBy, GETDATE()) 
OUTPUT inserted.*; ";
            #endregion

            return dbHelper.ExecuteAsync<ReminderTemplateSetting>(query, ReminderTemplateSetting, token);
        }

    }
}
