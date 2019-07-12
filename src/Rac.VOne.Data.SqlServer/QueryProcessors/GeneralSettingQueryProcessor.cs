using Dapper;
using Rac.VOne.Web.Models;
using Rac.VOne.Data.QueryProcessors;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Rac.VOne.Data.SqlServer.QueryProcessors
{
    public class GeneralSettingQueryProcessor :
        IUpdateGeneralSettingQueryProcessor,
        IGeneralSettingQueryProcessor
    {
        private readonly IDbHelper dbHelper;

        public GeneralSettingQueryProcessor(IDbHelper dbHelper)
        {
            this.dbHelper = dbHelper;
        }

        public Task<GeneralSetting> SaveAsync(GeneralSetting setting, CancellationToken token)
        {
            var query = @"
UPDATE GeneralSetting
SET
 Value = @Value,
 UpdateBy = @UpdateBy,
 UpdateAt = GETDATE()
OUTPUT inserted.*
WHERE
    Id  = @Id AND 
    CompanyId = @CompanyId";
            return dbHelper.ExecuteAsync<GeneralSetting>(query, setting, token);
        }

        public Task<int> InitializeAsync(int companyId, int loginUserId, CancellationToken token = default(CancellationToken))
        {
            var query = @"
INSERT INTO GeneralSetting
(CompanyId, Code, Value, Length, Precision, Description, CreateBy, CreateAt, UpdateBy, UpdateAt)
SELECT
  @CompanyId
, Code
, Value
, Length
, Precision
, Description
, @LoginUserId
, GETDATE()
, @LoginUserId
, GETDATE()
From GeneralSettingBase";
            return dbHelper.ExecuteAsync(query, new { CompanyId = companyId, LoginUserId = loginUserId }, token);
        }

    }
}
