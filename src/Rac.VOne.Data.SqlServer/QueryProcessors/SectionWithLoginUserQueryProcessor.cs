using System;
using Rac.VOne.Web.Models;
using Rac.VOne.Data.QueryProcessors;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Rac.VOne.Data.SqlServer.QueryProcessors
{
    public class SectionWithLoginUserQueryProcessor :
        ISectionWithLoginUserQueryProcessor,
        IAddSectionWithLoginUserQueryProcessor,
        IDeleteSectionWithLoginUserQueryProcessor
    {
        private readonly IDbHelper dbHelper;

        public SectionWithLoginUserQueryProcessor(IDbHelper dbHelper)
        {
            this.dbHelper = dbHelper;
        }
        public Task<IEnumerable<SectionWithLoginUser>> GetAsync(SectionWithLoginUserSearch option, CancellationToken token = default(CancellationToken))
        {
            var query = @"
SELECT        sl.*
            , sc.Code           SectionCode
            , sc.Name           SectionName
            , lu.Code           LoginUserCode
            , lu.Name           LoginUserName
FROM        SectionWithLoginUser sl
INNER JOIN  Section sc      ON sc.Id    = sl.SectionId
INNER JOIN  LoginUser lu    ON lu.Id    = sl.LoginUserId
WHERE       sl.LoginUserId      = sl.LoginUserId";
            if (option.CompanyId.HasValue) query += @"
AND         sc.CompanyId        = @CompanyId";
            if (option.SectionId.HasValue) query += @"
AND         sl.SectionId        = @SectionId";
            if (option.LoginUserId.HasValue) query += @"
AND         sl.LoginUserId      = @LoginUserId";
            if (option.SectionCodes?.Any() ?? false) query += @"
AND         sc.Code             IN (SELECT Code FROM @SectionCodes)";
            query += @"
ORDER BY      sc.Code           ASC
            , lu.Code           ASC";
            return dbHelper.GetItemsAsync<SectionWithLoginUser>(query, new {
                                option.CompanyId,
                                option.SectionId,
                                option.LoginUserId,
                SectionCodes =  option.SectionCodes.GetTableParameter(),
            }, token);
        }

        public async Task<bool> ExistLoginUserAsync(int LoginUserId, CancellationToken token = default(CancellationToken))
        {
            var query = @"SELECT TOP 1 1
                            FROM SectionWithLoginUser
                            WHERE LoginUserId = @LoginUserId";
            return (await dbHelper.ExecuteAsync<int?>(query, new { LoginUserId }, token)).HasValue;
        }

        public async Task<bool> ExistSectionAsync(int SectionId, CancellationToken token = default(CancellationToken))
        {
            var query = @"SELECT TOP 1 1
                            FROM SectionWithLoginUser
                            WHERE SectionId = @SectionId";
            return (await dbHelper.ExecuteAsync<int?>(query, new { SectionId }, token)).HasValue;
        }

        public Task<SectionWithLoginUser> SaveAsync(SectionWithLoginUser SectionWithLoginUser, CancellationToken token = default(CancellationToken))
        {
            var query = @"
MERGE INTO SectionWithLoginUser target
USING (
    SELECT
              @LoginUserId  LoginUserId
            , @SectionId    SectionId
) AS source
ON (
        target.LoginUserId  = source.LoginUserId
    AND target.SectionId    = source.SectionId
)
WHEN MATCHED THEN
    UPDATE SET
            UpdateBy = @UpdateBy
          , UpdateAt = GETDATE()
WHEN NOT MATCHED THEN
    INSERT (LoginUserId, SectionId, CreateBy, CreateAt, UpdateBy, UpdateAt)
    VALUES (@LoginUserId, @SectionId, @UpdateBy, GETDATE(), @UpdateBy, GETDATE())
OUTPUT inserted.*; ";
            return dbHelper.ExecuteAsync<SectionWithLoginUser>(query, SectionWithLoginUser, token);
        }

        public Task<int> DeleteAsync(int LoginUserId, int SectionId, CancellationToken token = default(CancellationToken))
        {
            var query = @"
DELETE      SectionWithLoginUser
WHERE       LoginUserId = @LoginUserId
AND         SectionId   = @SectionId";
            return dbHelper.ExecuteAsync(query, new { LoginUserId, SectionId}, token);
        }

    }
}
