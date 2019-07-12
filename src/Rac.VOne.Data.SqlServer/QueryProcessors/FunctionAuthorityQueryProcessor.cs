using Rac.VOne.Data.QueryProcessors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rac.VOne.Web.Models;
using System.Threading;

namespace Rac.VOne.Data.SqlServer.QueryProcessors
{
    public class FunctionAuthorityQueryProcessor :
        IAddFunctionAuthorityQueryProcessor,
        IFunctionAuthorityByLoginUserIdQueryProcessor
    {
        private readonly IDbHelper dbHelper;

        public FunctionAuthorityQueryProcessor(IDbHelper dbHelper)
        {
            this.dbHelper = dbHelper;
        }

        public Task<IEnumerable<FunctionAuthority>> GetAsync(FunctionAuthoritySearch option, CancellationToken token = default(CancellationToken))
        {
            var query = @"
SELECT      fa.*
FROM        FunctionAuthority fa
WHERE       fa.CompanyId         = @CompanyId";
            if (option.LoginUserId.HasValue) query += @"
AND         fa.AuthorityLevel   IN (
            SELECT      lu.FunctionLevel
            FROM        LoginUser lu
            WHERE       lu.Id           = @LoginUserId
            )";
            if (option.FunctionTypes?.Any() ?? false) query += @"
AND         fa.FunctionType     IN (SELECT Id   FROM @FunctionTypes)";

            return dbHelper.GetItemsAsync<FunctionAuthority>(query, new {
                                    option.CompanyId,
                                    option.LoginUserId,
                FunctionTypes   =   option.FunctionTypes.GetTableParameter()
            }, token);
        }

        public Task<FunctionAuthority> SaveAsync(FunctionAuthority authority, CancellationToken token = default(CancellationToken))
        {
            #region merge query
            var query = @"
MERGE INTO
    FunctionAuthority target
USING (
    SELECT
        @CompanyId      CompanyId,
        @AuthorityLevel AuthorityLevel,
        @FunctionType   FunctionType
) source ON (
        target.CompanyId        = source.CompanyId
    AND target.AuthorityLevel   = source.AuthorityLevel
    AND target.FunctionType     = source.FunctionType
)
WHEN MATCHED THEN
    UPDATE SET
        Available   = @Available,
        UpdateBy    = @UpdateBy,
        UpdateAt    = GETDATE()
WHEN NOT MATCHED THEN
    INSERT (
        CompanyId,
        AuthorityLevel,
        FunctionType,
        Available,
        CreateBy,
        CreateAt,
        UpdateBy,
        UpdateAt
    ) VALUES (
        @CompanyId,
        @AuthorityLevel,
        @FunctionType,
        @Available,
        @CreateBy,
        GETDATE(),
        @UpdateBy,
        GETDATE()
    )
OUTPUT inserted.*;
";
            #endregion
            return dbHelper.ExecuteAsync<FunctionAuthority>(query, authority, token);
        }

    }
}
