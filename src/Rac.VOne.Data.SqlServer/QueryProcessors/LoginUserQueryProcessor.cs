using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rac.VOne.Web.Models;
using Rac.VOne.Data.QueryProcessors;
using System.Threading;

namespace Rac.VOne.Data.SqlServer.QueryProcessors
{
    public class LoginUserQueryProcessor :
        ILoginUserQueryProcessor,
        IAddLoginUserQueryProcessor
    {
        private readonly IDbHelper dbHelper;

        public LoginUserQueryProcessor(IDbHelper helper)
        {
            dbHelper = helper;
        }
        public Task<IEnumerable<LoginUser>> GetAsync(LoginUserSearch option, CancellationToken token = default(CancellationToken))
        {
            var query = @"
SELECT        lu.*
            , dt.Code as DepartmentCode
            , dt.Name AS DepartmentName
            , st.Code AS StaffCode
            , st.Name AS StaffName
FROM        LoginUser AS lu
LEFT JOIN   Department AS dt    ON dt.Id    = lu.DepartmentId
LEFT JOIN   Staff AS st         ON st.Id    = lu.AssignedStaffId
WHERE       lu.Id               = lu.Id";
            if (option.CompanyId.HasValue) query += @"
AND         lu.CompanyId        = @CompanyId";
            if (option.Ids?.Any() ?? false) query += @"
AND         lu.Id               IN (SELECT Id   FROM @Ids)";

            if (option.Codes?.Any() ?? false) query += @"
AND         lu.Code             IN (SELECT Code FROM @Codes)";
            if (!string.IsNullOrWhiteSpace(option.Name))
            {
                option.Name = Sql.GetWrappedValue(option.Name);
                query += @"
AND         lu.Name             LIKE @Name";
            }
            if (option.UseClient.HasValue) query += @"
AND         lu.UseClient        = @UseClient";
            if (option.ExcludeCodes?.Any() ?? false) query += @"
AND         EXISTS (
            SELECT      1
            FROM        SectionWithLoginUser swl
            WHERE       swl.LoginUserId     = lu.Id
            )
AND         lu.Code         NOT IN (SELECT Code FROM @ExcludeCodes)";

            query += @"
ORDER BY      lu.CompanyId      ASC
            , lu.Code           ASC";

            return dbHelper.GetItemsAsync<LoginUser>(query, new {
                                    option.CompanyId,
                                    option.Name,
                                    option.UseClient,
                Ids             =   option.Ids  .GetTableParameter(),
                Codes           =   option.Codes.GetTableParameter(),
                ExcludeCodes    =   option.ExcludeCodes.GetTableParameter(),
            }, token);
        }

        public Task<LoginUser> SaveAsync(LoginUser loginUser, CancellationToken token = default(CancellationToken))
        {
            var query = @"
MERGE INTO LoginUser AS target
USING (
    SELECT 
             @CompanyId         CompanyId
            ,@Code              Code 
            ,@DepartmentId      DepartmentId 
            ,@AssignedStaffId   AssignedStaffId 
) AS source
ON (
        target.CompanyId    = source.CompanyId
    AND target.Code         = source.Code
  )
WHEN MATCHED THEN
    UPDATE SET
            Name            = @Name
          , DepartmentId    = @DepartmentId
          , Mail            = @Mail
          , MenuLevel       = @MenuLevel
          , FunctionLevel   = @FunctionLevel
          , UseClient       = @UseClient
          , UseWebViewer    = @UseWebViewer
          , AssignedStaffId = @AssignedStaffId
          , StringValue1    = @StringValue1
          , StringValue2    = @StringValue2
          , StringValue3    = @StringValue3
          , StringValue4    = @StringValue4
          , StringValue5    = @StringValue5
          , UpdateBy        = @UpdateBy
          , UpdateAt        = GETDATE()
WHEN NOT MATCHED THEN
    INSERT (CompanyId,Code,Name,DepartmentId,Mail,MenuLevel,FunctionLevel,UseClient,UseWebViewer,AssignedStaffId,StringValue1
    ,StringValue2,StringValue3,StringValue4,StringValue5,CreateBy,CreateAt,UpdateBy,UpdateAt)
    VALUES(@CompanyId,@Code,@Name,@DepartmentId,@Mail,@MenuLevel,@FunctionLevel,@UseClient,@UseWebViewer,@AssignedStaffId
    ,@StringValue1,@StringValue2,@StringValue3,@StringValue4,@StringValue5,@CreateBy,GETDATE(),@UpdateBy,GETDATE()) 
    OUTPUT inserted.*;";
            return dbHelper.ExecuteAsync<LoginUser>(query, loginUser, token);
        }

        public async Task<bool> ExitStaffAsync(int StaffId, CancellationToken token = default(CancellationToken))
        {
            var query = @"
SELECT TOP (1) 1 FROM LoginUser
 WHERE AssignedStaffId = @StaffId";
            return (await dbHelper.ExecuteAsync<int?>(query, new { StaffId }, token)).HasValue;
        }

    }
}
