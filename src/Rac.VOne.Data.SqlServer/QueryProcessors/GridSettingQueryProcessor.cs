using Rac.VOne.Web.Models;
using Rac.VOne.Data.QueryProcessors;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Rac.VOne.Data.SqlServer.QueryProcessors
{
    public class GridSettingQueryProcessor :
        IGridSettingQueryProcessor,
        IAddGridSettingQueryProcessor
    {
        private readonly IDbHelper dbHelper;

        public GridSettingQueryProcessor(IDbHelper dbHelper)
        {
            this.dbHelper = dbHelper;
        }

        public Task<IEnumerable<GridSetting>> GetAsync(GridSettingSearch option, CancellationToken token = default(CancellationToken))
        {
            var query = option.IsDefault ?
                @"
SELECT @CompanyId [CompanyId]
     , @LoginUserId [LoginUserId]
     , gsb.GridId
     , gsb.ColumnName
     , COALESCE(cn.Alias, gsb.ColumnNameJp) [ColumnNameJp]
     , gsb.DisplayOrder
     , gsb.DisplayWidth
  FROM GridSettingBase gsb
  LEFT JOIN ColumnNameSetting cn
    ON cn.CompanyId = @CompanyId
   AND cn.TableName = CASE WHEN gsb.GridId IN (1, 3, 5) THEN N'Billing'
                           WHEN gsb.GridId IN (2, 4   ) THEN N'Receipt'
                      END
   AND cn.ColumnName = gsb.ColumnName
 WHERE gsb.GridId = COALESCE(@GridId, gsb.GridId)
 ORDER BY gsb.GridId ASC, gsb.DisplayOrder ASC;" :
                @"
SELECT COALESCE(gs.CompanyId  , @CompanyId)       [CompanyId]
     , COALESCE(gs.LoginUserId, @LoginUserId)     [LoginUserId]
     , gsb.GridId
     , gsb.ColumnName
     , COALESCE(cn.Alias, gsb.ColumnNameJp)  [ColumnNameJp]
     , COALESCE(gs.DisplayOrder, gsb.DisplayOrder)  [DisplayOrder]
     , COALESCE(gs.DisplayWidth, gsb.DisplayWidth)  [DisplayWidth]
     , gs.CreateBy
     , gs.CreateAt
     , gs.UpdateBy
     , gs.UpdateAt
  FROM GridSettingBase  gsb
  LEFT JOIN GridSetting gs
    ON gsb.GridId       = gs.GridId
   AND gsb.ColumnName   = gs.ColumnName
   AND gs.CompanyId     = @CompanyId
   AND gs.LoginUserId   = @LoginUserId
  LEFT JOIN ColumnNameSetting cn
    ON cn.CompanyId     = @CompanyId
   AND cn.TableName     = CASE WHEN gsb.GridId IN (1, 3, 5) THEN N'Billing'
                               WHEN gsb.GridId IN (2, 4   ) THEN N'Receipt'
                          END
   AND cn.ColumnName    = gsb.ColumnName
 WHERE gsb.GridId       = COALESCE(@GridId, gsb.GridId)
 ORDER BY gsb.GridId ASC
, COALESCE(gs.DisplayOrder, gsb.DisplayOrder) ASC;";
            return dbHelper.GetItemsAsync<GridSetting>(query, option, token);
        }

        public Task<GridSetting> SaveAsync(GridSetting GridSetting, bool updateAllUsers, CancellationToken token = default(CancellationToken))
        {
            var query = updateAllUsers ? @"
MERGE INTO GridSetting AS target
USING (
    SELECT  @CompanyId      AS CompanyId
          , @LoginUserId    AS LoginUserId
          , @GridId         AS GridId
          , @ColumnName     AS ColumnName
     ) AS source ON (
            target.CompanyId    = source.CompanyId
        AND target.GridId       = source.GridId
        AND target.ColumnName   = source.ColumnName
)
      WHEN MATCHED THEN
         UPDATE SET
         DisplayWidth = @DisplayWidth
        ,DisplayOrder = @DisplayOrder
        ,UpdateAt = GETDATE()
     WHEN NOT MATCHED THEN
         INSERT ( CompanyId,       LoginUserId, GridId, ColumnName, DisplayOrder, DisplayWidth, CreateBy, CreateAt, UpdateBy, UpdateAt)
         VALUES (@CompanyId,source.LoginUserId,@GridId,@ColumnName,@DisplayOrder,@DisplayWidth,@CreateBy,GETDATE(),@UpdateBy,GETDATE())
     OUTPUT inserted.*;" : @"
MERGE INTO GridSetting AS target
USING (
    SELECT  @CompanyId      AS CompanyId
          , @LoginUserId    AS LoginUserId
          , @GridId         AS GridId
          , @ColumnName     AS ColumnName
) AS source ON (
            target.CompanyId        = source.CompanyId
        AND target.LoginUserId      = source.LoginUserId
        AND target.GridId           = source.GridId
        AND target.ColumnName       = source.ColumnName
)
 WHEN MATCHED THEN
    UPDATE SET
    DisplayWidth    = @DisplayWidth
   ,DisplayOrder    = @DisplayOrder
   ,UpdateAt        = GETDATE()
WHEN NOT MATCHED THEN
    INSERT ( CompanyId,  LoginUserId,  GridId,  ColumnName,  DisplayOrder,  DisplayWidth,  CreateBy,  CreateAt,  UpdateBy,  UpdateAt)
    VALUES (@CompanyId, @LoginUserId, @GridId, @ColumnName, @DisplayOrder, @DisplayWidth, @CreateBy, GETDATE(), @UpdateBy, GETDATE())
OUTPUT inserted.*; ";
            return dbHelper.ExecuteAsync<GridSetting>(query, GridSetting, token);
        }


    }
}
