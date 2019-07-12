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
    public class WebApiSettingQueryProcessor :
        IAddWebApiSettingQueryProcessor,
        IDeleteWebApiSettingQueryProcessor,
        IWebApiSettingQueryProcessor
    {
        private readonly IDbHelper dbHelper;

        public WebApiSettingQueryProcessor(IDbHelper dbHelper)
        {
            this.dbHelper = dbHelper;
        }

        public Task<int> DeleteAsync(int companyId, int? apiTypeId, CancellationToken token = default(CancellationToken))
        {
            var query = @"
DELETE WebApiSetting
 WHERE CompanyId        = @companyId";
            if (apiTypeId.HasValue) query += @"
   AND ApiTypeId        = @apiTypeId";
            return dbHelper.ExecuteAsync(query, new { companyId, apiTypeId }, token);
        }

        public Task<WebApiSetting> GetByIdAsync(int companyId, int apiTypeId, CancellationToken token = default(CancellationToken))
        {
            var query = @"
SELECT *
  FROM WebApiSetting
 WHERE CompanyId        = @companyId
   AND ApiTypeId        = @apiTypeId
";
            return dbHelper.ExecuteAsync<WebApiSetting>(query, new { companyId, apiTypeId }, token);
        }

        public Task<int> SaveAsync(WebApiSetting setting, CancellationToken token = default(CancellationToken))
        {
            var query = @"
MERGE INTO WebApiSetting target
USING (
    SELECT @CompanyId [CompanyId]
         , @ApiTypeId [ApiTypeId]
) source ON (
        target.CompanyId    = source.CompanyId
    AND target.ApiTypeId    = source.ApiTypeId
)
WHEN MATCHED THEN
    UPDATE SET
           BaseUri          = @BaseUri
         , ApiVersion       = @ApiVersion
         , AccessToken      = @AccessToken
         , RefreshToken     = @RefreshToken
         , ClientId         = @ClientId
         , ClientSecret     = @ClientSecret
         , ExtractSetting   = @ExtractSetting
         , OutputSetting    = @OutputSetting
         , UpdateBy         = @UpdateBy
         , UpdateAt         = GETDATE()
WHEN NOT MATCHED THEN
    INSERT
         ( CompanyId
         , ApiTypeId
         , BaseUri
         , ApiVersion
         , AccessToken
         , RefreshToken
         , ClientId
         , ClientSecret
         , ExtractSetting
         , OutputSetting
         , CreateBy
         , CreateAt
         , UpdateBy
         , UpdateAt )
    VALUES
         ( @CompanyId
         , @ApiTypeId
         , @BaseUri
         , @ApiVersion
         , @AccessToken
         , @RefreshToken
         , @ClientId
         , @ClientSecret
         , @ExtractSetting
         , @OutputSetting
         , @CreateBy
         , GETDATE()
         , @UpdateBy
         , GETDATE() )
;";
            return dbHelper.ExecuteAsync(query, setting, token);
        }
    }
}
