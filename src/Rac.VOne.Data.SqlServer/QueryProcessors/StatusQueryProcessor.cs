using System.Collections.Generic;
using System.Linq;
using Rac.VOne.Web.Models;
using Rac.VOne.Data.QueryProcessors;
using System.Threading;
using System.Threading.Tasks;

namespace Rac.VOne.Data.SqlServer.QueryProcessors
{
    public class StatusQueryProcessor :
        IStatusQueryProcessor,
        IStatusExistsQueryProcessor,
        IAddStatusQueryProcessor,
        IDeleteStatusQueryProcessor
    {
        private readonly IDbHelper dbHelper;
        public StatusQueryProcessor(IDbHelper dbHelper)
        {
            this.dbHelper = dbHelper;
        }
        public Task<IEnumerable<Status>> GetAsync(StatusSearch option, CancellationToken token = default(CancellationToken))
        {
            var query = @"
SELECT      * 
FROM        StatusMaster
WHERE       CompanyId       = @CompanyId
AND         StatusType      = @StatusType";
            if (option.Codes?.Any() ?? false) query += @"
AND         Code            IN (SELECT Code FROM @Codes) ";
            query += @"
ORDER BY    DisplayOrder    ASC";
            return dbHelper.GetItemsAsync<Status>(query, option, token);
        }

        public Task<int> InitializeAsync(int companyId, int loginUserId, CancellationToken token = default(CancellationToken))
        {
            var query = @"
INSERT INTO StatusMaster
( CompanyId
, StatusType
, Code
, Name
, Completed
, DisplayOrder
, CreateBy
, CreateAt
, UpdateBy
, UpdateAt
)
SELECT 
  @CompanyId
,  StatusType
,  Code
,  Name
,  Completed
, 0
, @LoginUserId
, GETDATE()
, @LoginUserId
, GETDATE()
FROM StatusMasterBase
";
            return dbHelper.ExecuteAsync(query, new { CompanyId = companyId, LoginUserId = loginUserId }, token);
        }

        public Task<Status> SaveAsync(Status Status, CancellationToken token = default(CancellationToken))
        {
            #region merge query
            var query = @"
MERGE INTO StatusMaster target
USING (
    SELECT @CompanyId   [CompanyId]
         , @StatusType  [StatusType]
         , @Code        [Code]
) source
ON    (
        target.CompanyId    = source.CompanyId
    AND target.StatusType   = source.StatusType
    AND target.Code         = source.Code
)
WHEN MATCHED THEN
    UPDATE SET 
         StatusType = @StatusType
        ,Code = @Code
        ,Name = @Name
        ,Completed = @Completed
        ,DisplayOrder = @DisplayOrder
        ,UpdateBy = @UpdateBy
        ,UpdateAt = GETDATE()
WHEN NOT MATCHED THEN 
    INSERT (CompanyId, StatusType, Code, Name, Completed, DisplayOrder, CreateBy, CreateAt, UpdateBy, UpdateAt) 
    VALUES (@CompanyId, @StatusType, @Code, @Name, @Completed, @DisplayOrder, @UpdateBy, GETDATE(), @UpdateBy, GETDATE()) 
OUTPUT inserted.*; ";
            #endregion

            return dbHelper.ExecuteAsync<Status>(query, Status, token);
        }

        public Task<int> DeleteAsync(int Id, CancellationToken token = default(CancellationToken))
        {
            var query = $@"
DELETE      StatusMaster 
WHERE       Id      = @Id";
            return dbHelper.ExecuteAsync(query, new { Id }, token);
        }

        public async Task<bool> ExistReminderAsync(int StatusId, CancellationToken token = default(CancellationToken))
        {
            var query = @"
SELECT 1
 WHERE EXISTS(
       SELECT 1 FROM Reminder WHERE StatusId = @StatusId)";
            return (await dbHelper.ExecuteAsync<int?>(query, new { StatusId })).HasValue;
        }

        public async Task<bool> ExistReminderHistoryAsync(int StatusId, CancellationToken token = default(CancellationToken))
        {
            var query = @"
SELECT 1
 WHERE EXISTS(
       SELECT 1 FROM ReminderHistory WHERE StatusId = @StatusId)";
            return (await dbHelper.ExecuteAsync<int?>(query, new { StatusId })).HasValue;
        }
    }
}
