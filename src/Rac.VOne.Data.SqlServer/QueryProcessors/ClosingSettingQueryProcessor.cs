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
    public class ClosingSettingQueryProcessor : IClosingSettingQueryProcessor
    {
        private readonly IDbHelper dbHelper;
        public ClosingSettingQueryProcessor(IDbHelper dbHelper)
        {
            this.dbHelper = dbHelper;
        }

        public Task<ClosingSetting> SaveAsync(ClosingSetting setting, CancellationToken token = default(CancellationToken))
        {
            var query = @"
MERGE INTO ClosingSetting AS cs
USING (
    SELECT
     @CompanyId AS CompanyId
) AS Target 
ON (
    cs.CompanyId = @CompanyId
)
WHEN MATCHED THEN
    UPDATE SET
           BaseDate                  = @BaseDate
         , AllowReceptJournalPending = @AllowReceptJournalPending
         , AllowMutchingPending      = @AllowMutchingPending
         , UpdateBy                  = @UpdateBy
         , UpdateAt                  = GETDATE()
WHEN NOT MATCHED THEN
    INSERT ( CompanyId
           , BaseDate
           , AllowReceptJournalPending
           , AllowMutchingPending
           , UpdateBy
           , UpdateAt
) VALUES ( @CompanyId
         , @BaseDate
         , @AllowReceptJournalPending
         , @AllowMutchingPending
         , @UpdateBy
         , GETDATE() )
OUTPUT inserted.*;
";
            return dbHelper.ExecuteAsync<ClosingSetting>(query, setting, token);
        }
    }
}
