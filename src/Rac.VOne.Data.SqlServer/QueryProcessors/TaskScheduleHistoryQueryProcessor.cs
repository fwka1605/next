using Rac.VOne.Web.Models;
using Rac.VOne.Data.QueryProcessors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace Rac.VOne.Data.SqlServer.QueryProcessors
{
    public class TaskScheduleHistoryQueryProcessor :
        ITaskScheduleHistoryQueryProcessor,
        IAddTaskScheduleHistoryQueryProcessor
    {
        private readonly IDbHelper dbHelper;

        public TaskScheduleHistoryQueryProcessor(IDbHelper dbHelper)
        {
            this.dbHelper = dbHelper;
        }


        public Task<IEnumerable<TaskScheduleHistory>> GetAsync(TaskScheduleHistorySearch option, CancellationToken token = default(CancellationToken))
        {
            var query = $@"
SELECT
    Id,
    CompanyId,
    ImportType,
    ImportSubType,
    StartAt,
    EndAt,
    Result,
    Errors
FROM
    TaskScheduleHistory
WHERE
    { GetWhereConditionsForGetItems(option) }
";
            return dbHelper.GetItemsAsync<TaskScheduleHistory>(query, option, token);
        }

        private string GetWhereConditionsForGetItems(TaskScheduleHistorySearch conditions)
        {
            var result = new List<string>
            {
                "CompanyId = @CompanyId"
            };

            if (conditions.ImportType.HasValue)
            {
                result.Add("ImportType = @ImportType");
            }
            if (conditions.ImportSubType.HasValue)
            {
                result.Add("ImportSubType = @ImportSubType");
            }
            if (conditions.Result.HasValue)
            {
                result.Add("Result = @Result");
            }

            if (conditions.StartAt_From.HasValue && conditions.StartAt_To.HasValue)
            {
                result.Add("StartAt BETWEEN @StartAt_From AND @StartAt_To");
            }
            else if (conditions.StartAt_From.HasValue)
            {
                result.Add("StartAt >= @StartAt_From");
            }
            else if (conditions.StartAt_To.HasValue)
            {
                result.Add("StartAt <= @StartAt_To");
            }

            if (conditions.EndAt_From.HasValue && conditions.EndAt_To.HasValue)
            {
                result.Add("EndAt BETWEEN @EndAt_From AND @EndAt_To");
            }
            else if (conditions.EndAt_From.HasValue)
            {
                result.Add("EndAt >= @EndAt_From");
            }
            else if (conditions.EndAt_To.HasValue)
            {
                result.Add("EndAt <= @EndAt_To");
            }

            return string.Join(@"
AND ", result);
        }

        public Task<TaskScheduleHistory> AddAsync(TaskScheduleHistory history, CancellationToken token = default(CancellationToken))
        {
            var query = @"
INSERT INTO [dbo].[TaskScheduleHistory]
     ( [CompanyId]
     , [ImportType]
     , [ImportSubType]
     , [StartAt]
     , [EndAt]
     , [Result]
     , [Errors])
OUTPUT inserted.*
VALUES (@CompanyId
     ,  @ImportType
     ,  @ImportSubType
     ,  @StartAt
     ,  @EndAt
     ,  @Result
     ,  @Errors)
";
            return dbHelper.ExecuteAsync<TaskScheduleHistory>(query, history, token);
        }


    }
}
