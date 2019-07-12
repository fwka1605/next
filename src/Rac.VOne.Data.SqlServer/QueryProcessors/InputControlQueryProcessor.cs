using System.Collections.Generic;
using Rac.VOne.Data.QueryProcessors;
using Rac.VOne.Web.Models;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Rac.VOne.Data.SqlServer.QueryProcessors
{
    public class InputControlQueryProcessor :
        IInputControlQueryProcessor,
        IAddInputControlQueryProcessor,
        IDeleteInputControlQueryProcessor
    {
        private readonly IDbHelper dbHelper;

        public InputControlQueryProcessor(IDbHelper dbHelper)
        {
            this.dbHelper = dbHelper;
        }

        public Task<IEnumerable<InputControl>> GetAsync(InputControl control, CancellationToken token = default(CancellationToken))
        {
            var query = @"
SELECT ic.CompanyId
     , ic.LoginUserId
     , ic.InputGridTypeId
     , ic.ColumnName
     , COALESCE(cn.Alias, ic.ColumnNameJp) [ColumnNameJp]
     , ic.ColumnOrder
     , ic.TabStop
     , ic.TabIndex
  FROM InputControl ic
  LEFT JOIN ColumnNameSetting cn
    ON cn.CompanyId       = ic.CompanyId
   AND cn.TableName       = (
                                CASE
                                    WHEN @InputGridTypeId = 2 THEN N'Receipt'
                                    ELSE N'Billing'
                                END
                            )
   AND cn.ColumnName      = ic.ColumnName
 WHERE ic.CompanyId       = @CompanyId
   AND ic.LoginUserId     = @LoginUserId
   AND ic.InputGridTypeId = @InputGridTypeId
 ORDER BY ic.ColumnOrder";
            return dbHelper.GetItemsAsync<InputControl>(query, control, token);
        }

        public Task<InputControl> SaveAsync(InputControl control, CancellationToken token = default(CancellationToken))
        {
            var query = @"
INSERT INTO InputControl
(  CompanyId
,  LoginUserId
,  InputGridTypeId
,  ColumnName
,  ColumnNameJp
,  ColumnOrder
,  TabStop
,  TabIndex
)
OUTPUT inserted.*
VALUES (
  @CompanyId
, @LoginUserId
, @InputGridTypeId
, @ColumnName
, @ColumnNameJp
, @ColumnOrder
, @TabStop
, @TabIndex
)";
            return dbHelper.ExecuteAsync<InputControl>(query, control, token);
        }

        public Task<int> DeleteAsync(int CompanyId, int LoginUserId, int InputGridTypeId, CancellationToken token = default(CancellationToken))
        {
            var query = @"
DELETE InputControl 
 WHERE CompanyId        = @CompanyId 
   AND LoginUserId      = @LoginUserId
   AND InputGridTypeId  = @InputGridTypeId";
            return dbHelper.ExecuteAsync(query, new { CompanyId, LoginUserId, InputGridTypeId, }, token);
        }

    }
}
