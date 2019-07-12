using Rac.VOne.Web.Models;
using Rac.VOne.Data.QueryProcessors;
using System.Threading;
using System.Threading.Tasks;

namespace Rac.VOne.Data.SqlServer.QueryProcessors
{
    public class InvoiceCommonSettingQueryProcessor : IAddInvoiceCommonSettingQueryProcessor
    {
        private readonly IDbHelper dbHelper;

        public InvoiceCommonSettingQueryProcessor(IDbHelper dbHelper)
        {
            this.dbHelper = dbHelper;
        }

        public Task<InvoiceCommonSetting> SaveAsync(InvoiceCommonSetting setting, CancellationToken token = default(CancellationToken))
        {
            #region merge query
            var query = @"
MERGE INTO InvoiceCommonSetting target
USING (
    SELECT @CompanyId   [CompanyId]
) source
ON    (
        target.CompanyId    = source.CompanyId
)
WHEN MATCHED THEN
    UPDATE SET 
         ExcludeAmountZero      = @ExcludeAmountZero
        ,ExcludeMinusAmount     = @ExcludeMinusAmount
        ,ExcludeMatchedData     = @ExcludeMatchedData
        ,ControlInputCharacter  = @ControlInputCharacter
        ,UpdateBy = @UpdateBy
        ,UpdateAt = GETDATE()
WHEN NOT MATCHED THEN 
    INSERT (CompanyId, ExcludeAmountZero, ExcludeMinusAmount, ExcludeMatchedData, CreateBy, CreateAt, UpdateBy, UpdateAt, ControlInputCharacter) 
    VALUES (@CompanyId, @ExcludeAmountZero, @ExcludeMinusAmount, @ExcludeMatchedData, @UpdateBy, GETDATE(), @UpdateBy, GETDATE(), @ControlInputCharacter) 
OUTPUT inserted.*; ";
            #endregion

            return dbHelper.ExecuteAsync<InvoiceCommonSetting>(query, setting, token);
        }

    }
}
