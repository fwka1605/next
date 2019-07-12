using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rac.VOne.Web.Models;
using Rac.VOne.Data.QueryProcessors;
using System.Threading;

namespace Rac.VOne.Data.SqlServer.QueryProcessors
{
    public class MenuAuthorityQueryProcessor :
        IAddMenuAuthorityQueryProcessor,
        IMenuAuthorityQueryProcessor
    {
        private readonly IDbHelper dbHelper;

        public MenuAuthorityQueryProcessor(IDbHelper dbHelper)
        {
            this.dbHelper = dbHelper;
        }

        public Task<MenuAuthority> SaveAsync(MenuAuthority menu, CancellationToken token = default(CancellationToken))
        {
            var query = @"
MERGE INTO MenuAuthority target
USING (SELECT @CompanyId        CompanyId
            , @MenuId           MenuId
            , @AuthorityLevel   AuthorityLevel
      ) source
   ON (    target.CompanyId         = source.CompanyId
       AND target.MenuId            = source.MenuId
       AND target.AuthorityLevel    = source.AuthorityLevel
      )
 WHEN MATCHED THEN
      UPDATE SET
             Available              = @Available
           , UpdateBy               = @UpdateBy
           , UpdateAt               = GETDATE()
 WHEN NOT MATCHED THEN
      INSERT ( CompanyId,  MenuId,  AuthorityLevel
           ,  Available
           ,  CreateBy,  CreateAt,  UpdateBy, UpdateAt )
      VALUES (@CompanyId, @MenuId, @AuthorityLevel
           , @Available
           , @CreateBy, GETDATE(), @UpdateBy, GETDATE() )
      OUTPUT inserted.*;";
            return dbHelper.ExecuteAsync<MenuAuthority>(query, menu, token);
        }

        public Task<IEnumerable<MenuAuthority>> GetAsync(MenuAuthoritySearch option, CancellationToken token = default(CancellationToken))
        {
            string query;

            if (option.CompanyId.HasValue)
            {
                query = @"
SELECT ma.*
     , mn.MenuName
     , mn.MenuCategory
     , mn.Sequence
  FROM MenuAuthority ma
 INNER JOIN Menu mn
    ON mn.MenuId    = ma.MenuId
   AND ma.CompanyId = @CompanyId
 INNER JOIN ApplicationControl ac
    ON ac.CompanyId = ma.CompanyId";
                if (option.LoginUserId.HasValue) query += @"
 INNER JOIN LoginUser lu
    ON lu.Id        = @LoginUserId
   AND lu.MenuLevel = ma.AuthorityLevel
   AND ma.Available = 1";
                query += @"
ORDER BY
      mn.MenuId         ASC
    , ma.AuthorityLevel ASC";
            }

            // 会社ID無指定時は初期メニュー一覧を取得する ※ Menuテーブルの全項目(絞り込み条件なし)
            else
            {
                query = @"
SELECT 0 [CompanyId]
     , m.MenuId
     , ma.AuthorityLevel
     , ma.Available
     , 0                                [CreateBy]
     , CAST('1753-1-1' AS DATETIME)     [CreateAt]
     , 0                                [UpdateBy]
     , CAST('1753-1-1' AS DATETIME)     [UpdateAt]
     , m.MenuName
     , m.MenuCategory
     , m.Sequence
  FROM Menu m
 INNER JOIN (
                SELECT 1 [AuthorityLevel], 1 [Available]
    UNION ALL   SELECT 2, 0
    UNION ALL   SELECT 3, 0
    UNION ALL   SELECT 4, 0
    ) ma
   ON m.MenuId      = m.MenuId
ORDER BY
      m.Sequence
    , ma.AuthorityLevel
";
            }
            return dbHelper.GetItemsAsync<MenuAuthority>(query, option, token);
        }

    }
}
