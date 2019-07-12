using System;
using Rac.VOne.Web.Models;
using Rac.VOne.Data.QueryProcessors;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Rac.VOne.Data.SqlServer.QueryProcessors
{
   public class TaskScheduleQueryProcessor : ITaskScheduleQueryProcessor
    {
        private readonly IDbHelper dbHelper;

        public TaskScheduleQueryProcessor(IDbHelper dbHelper)
        {
            this.dbHelper = dbHelper;
        }


        public Task<IEnumerable<TaskSchedule>> GetAsync(TaskScheduleSearch option, CancellationToken token = default(CancellationToken))
        {
            var query = @"
SELECT        ts.*
            , uc.Name AS CreateUserName
            , uu.Name AS UpdateUserName
FROM        TaskSchedule ts
LEFT JOIN   LoginUser uc        ON ts.CreateBy  = uc.id
LEFT JOIN   LoginUser uu        ON ts.UpdateBy  = uu.id
WHERE       ts.CompanyId        = @CompanyId";
            if (option.ImportType.HasValue) query += @"
AND         ts.ImportType       = @ImportType";
            if (option.ImportSubType.HasValue) query += @"
AND         ts.ImportSubType    = @ImportSubType";
            return dbHelper.GetItemsAsync<TaskSchedule>(query, option, token);
        }
        public async Task<bool> ExistsAsync(TaskScheduleSearch option, CancellationToken token = default(CancellationToken))
        {
            var query = @"
SELECT      1
WHERE EXISTS (
            SELECT      1
            FROM        TaskSchedule ts
            WHERE       ts.CompanyId        = @CompanyId";
            if (option.ImportType.HasValue) query += @"
            AND         ts.ImportType       = @ImportType";
            if (option.ImportSubType.HasValue) query += @"
            AND         ts.ImportSubType    = @ImportSubType";
            query += @"
            )";
            return (await dbHelper.ExecuteAsync<int?>(query, option, token)).HasValue;
        }

        public Task<TaskSchedule> SaveAsync(TaskSchedule TaskSchedule, CancellationToken token = default(CancellationToken))
        {
            var query = @"
MERGE INTO
    TaskSchedule target
USING (
    SELECT
        @CompanyId      CompanyId,
        @ImportType     ImportType,
        @ImportSubType  ImportSubType
) source ON (
        target.CompanyId        = source.CompanyId
    AND target.ImportType       = source.ImportType
    AND target.ImportSubType    = source.ImportSubType
)
WHEN MATCHED THEN
    UPDATE SET
          Duration                = @Duration
        , StartDate               = @StartDate
        , Interval                = @Interval
        , WeekDay                 = @WeekDay
        , ImportDirectory         = @ImportDirectory
        , SuccessDirectory        = @SuccessDirectory
        , FailedDirectory         = @FailedDirectory
        , LogDestination          = @LogDestination
        , TargetBillingAssignment = @TargetBillingAssignment
        , BillingAmount           = @BillingAmount
        , UpdateSameCustomer      = @UpdateSameCustomer
        , UpdateBy                = @UpdateBy
        , UpdateAt                = GETDATE()
        , ImportMode              = @ImportMode
WHEN NOT MATCHED THEN
    INSERT (
          CompanyId
       ,  ImportType
       ,  ImportSubType
       ,  Duration
       ,  StartDate
       ,  Interval
       ,  WeekDay
       ,  ImportDirectory
       ,  SuccessDirectory
       ,  FailedDirectory
       ,  LogDestination
       ,  TargetBillingAssignment
       ,  BillingAmount
       ,  UpdateSameCustomer
       ,  CreateBy
       ,  CreateAt
       ,  UpdateBy
       ,  UpdateAt
       ,  ImportMode
    ) VALUES (
         @CompanyId
       , @ImportType
       , @ImportSubType
       , @Duration
       , @StartDate
       , @Interval
       , @WeekDay
       , @ImportDirectory
       , @SuccessDirectory
       , @FailedDirectory
       , @LogDestination
       , @TargetBillingAssignment
       , @BillingAmount
       , @UpdateSameCustomer
       , @CreateBy
       , GETDATE()
       , @UpdateBy
       , GETDATE()
       , @ImportMode
    )
OUTPUT inserted.*;
";
            return dbHelper.ExecuteAsync<TaskSchedule>(query, TaskSchedule, token);
        }

        public Task<int> DeleteAsync(int CompanyId, int ImportType, int ImportSubType, CancellationToken token = default(CancellationToken))
        {
            var query = @"
DELETE FROM TaskSchedule
 WHERE CompanyId        = @CompanyId
   AND ImportType       = @ImportType
   AND ImportSubType    = @ImportSubType
";
            return dbHelper.ExecuteAsync(query, new { CompanyId, ImportType, ImportSubType }, token);
        }


    }
}
