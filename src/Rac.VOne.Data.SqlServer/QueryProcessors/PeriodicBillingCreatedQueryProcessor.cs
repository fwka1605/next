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
    public class PeriodicBillingCreatedQueryProcessor :
        IAddPeriodicBillingCreatedQueryProcessor
    {
        private readonly IDbHelper dbHelper;
        public PeriodicBillingCreatedQueryProcessor(IDbHelper dbHelper)
        {
            this.dbHelper = dbHelper;
        }

        public Task<PeriodicBillingCreated> SaveAsync(PeriodicBillingCreated created, CancellationToken token = default(CancellationToken))
        {

            #region merge query
            var query = @"
MERGE INTO PeriodicBillingCreated target
USING (
    SELECT @PeriodicBillingSettingId    [PeriodicBillingSettingId]
         , @CreateYearMonth             [CreateYearMonth]
) source
ON    (
        target.PeriodicBillingSettingId     = source.PeriodicBillingSettingId
    AND target.CreateYearMonth              = source.CreateYearMonth
)
WHEN MATCHED THEN
    UPDATE SET 
         UpdateBy = @UpdateBy
        ,UpdateAt = GETDATE()
WHEN NOT MATCHED THEN 
    INSERT ( PeriodicBillingSettingId,  CreateYearMonth,  CreateBy,  CreateAt,  UpdateBy,  UpdateAt)
    VALUES (@PeriodicBillingSettingId, @CreateYearMonth, @UpdateBy, GETDATE(), @UpdateBy, GETDATE())
OUTPUT inserted.*; ";
            #endregion

            return dbHelper.ExecuteAsync<PeriodicBillingCreated>(query, created, token);

        }

    }
}
