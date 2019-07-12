using Rac.VOne.Web.Models;
using Rac.VOne.Data.QueryProcessors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace Rac.VOne.Data.SqlServer.QueryProcessors
{
    public class EBExcludeAccountSettingQueryProcessor : IEBExcludeAccountSettingQueryProcessor
    {
        private readonly IDbHelper dbHelper;

        public EBExcludeAccountSettingQueryProcessor(IDbHelper dbHelper)
        {
            this.dbHelper = dbHelper;
        }

        public Task<EBExcludeAccountSetting> SaveAsync(EBExcludeAccountSetting setting, CancellationToken token = default(CancellationToken))
        {
            #region insert query
            var query = @"
INSERT INTO EBExcludeAccountSetting
( CompanyId
, BankCode
, BranchCode
, AccountTypeId
, PayerCode
, CreateBy
, CreateAt
, UpdateBy
, UpdateAt
) OUTPUT inserted.* VALUES
(@CompanyId
,@BankCode
,@BranchCode
,@AccountTypeId
,@PayerCode
,@CreateBy
,GETDATE()
,@UpdateBy
,GETDATE()
)
";
            #endregion
            return dbHelper.ExecuteAsync<EBExcludeAccountSetting>(query, setting, token);
        }

        public Task<int> DeleteAsync(EBExcludeAccountSetting setting, CancellationToken token = default(CancellationToken))
        {
            var query = @"
DELETE
    EBExcludeAccountSetting
WHERE
        CompanyId       = @CompanyId
    AND BankCode        = @BankCode
    AND BranchCode      = @BranchCode
    AND AccountTypeId   = @AccountTypeId
    AND PayerCode       = @PayerCode
";
            return dbHelper.ExecuteAsync(query, setting, token);
        }


    }
}
