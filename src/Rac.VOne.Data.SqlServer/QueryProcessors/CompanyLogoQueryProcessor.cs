using Rac.VOne.Web.Models;
using Rac.VOne.Data.QueryProcessors;
using System.Threading;
using System.Threading.Tasks;

namespace Rac.VOne.Data.SqlServer.QueryProcessors
{
    public class CompanyLogoQueryProcessor :
        IAddCompanyLogoQueryProcessor,
        IDeleteCompanyLogoQueryProcessor
    {
        private readonly IDbHelper dbHelper;

        public CompanyLogoQueryProcessor(IDbHelper dbHelper)
        {
            this.dbHelper = dbHelper;
        }

        public Task<CompanyLogo> SaveAsync(CompanyLogo CompanyLogo, CancellationToken token = default(CancellationToken))
        {
            #region merge query
            var query = @"
MERGE INTO CompanyLogo AS target 
USING ( 
    SELECT 
        @CompanyId [CompanyId]
       ,@LogoType  [LogoType]
) AS source 
ON  ( 
        target.CompanyId = source.CompanyId 
    AND target.LogoType  = source.LogoType
    )
WHEN MATCHED THEN 
    UPDATE SET 
        Logo = @Logo
        ,UpdateBy = @UpdateBy
        ,UpdateAt = @UpdateAt
WHEN NOT MATCHED THEN 
    INSERT (
         CompanyId
        ,Logo
        ,CreateBy
        ,CreateAt
        ,UpdateBy
        ,UpdateAt
        ,LogoType
        )
    VALUES
    (@CompanyId
    ,@Logo
    ,@CreateBy
    ,GETDATE()
    ,@UpdateBy
    ,GETDATE()
    ,@LogoType)
OUTPUT inserted.*; ";
            #endregion
            return dbHelper.ExecuteAsync<CompanyLogo>(query, CompanyLogo, token);
        }

        public Task<int> DeleteAsync(int CompanyId, int LogoType, CancellationToken token = default(CancellationToken))
        {
            var query = @"
DELETE CompanyLogo
 WHERE CompanyId = @CompanyId
   AND LogoType = @LogoType
";
            return dbHelper.ExecuteAsync(query, new { CompanyId, LogoType }, token);
        }

    }
}
