using Rac.VOne.Web.Models;
using Rac.VOne.Data.QueryProcessors;
using System.Threading;
using System.Threading.Tasks;

namespace Rac.VOne.Data.SqlServer.QueryProcessors
{
    public class InvoiceNumberSettingQueryProcessor : IAddInvoiceNumberSettingQueryProcessor
    {
        private readonly IDbHelper dbHelper;

        public InvoiceNumberSettingQueryProcessor(IDbHelper dbHelper)
        {
            this.dbHelper = dbHelper;
        }

        public Task<InvoiceNumberSetting> SaveAsync(InvoiceNumberSetting setting, CancellationToken token = default(CancellationToken))
        {
            #region merge query
            var query = @"
MERGE INTO InvoiceNumberSetting target
USING (
    SELECT @CompanyId   [CompanyId]
) source
ON    (
        target.CompanyId    = source.CompanyId
)
WHEN MATCHED THEN
    UPDATE SET 
         UseNumbering    = @UseNumbering
        ,Length          = @Length
        ,ZeroPadding     = @ZeroPadding
        ,ResetType       = @ResetType
        ,ResetMonth      = @ResetMonth
        ,FormatType      = @FormatType
        ,DateType        = @DateType
        ,DateFormat      = @DateFormat
        ,FixedStringType = @FixedStringType
        ,FixedString     = @FixedString
        ,DisplayFormat   = @DisplayFormat
        ,Delimiter       = @Delimiter
        ,UpdateBy        = @UpdateBy
        ,UpdateAt        = GETDATE()
WHEN NOT MATCHED THEN 
    INSERT (CompanyId, UseNumbering, Length, ZeroPadding, ResetType, ResetMonth, FormatType, DateType, DateFormat, FixedStringType, FixedString, DisplayFormat, Delimiter, CreateBy, CreateAt, UpdateBy, UpdateAt) 
    VALUES (@CompanyId, @UseNumbering, @Length, @ZeroPadding, @ResetType, @ResetMonth, @FormatType, @DateType, @DateFormat, @FixedStringType, @FixedString, @DisplayFormat, @Delimiter, @UpdateBy, GETDATE(), @UpdateBy, GETDATE()) 
OUTPUT inserted.*; ";
            #endregion

            return dbHelper.ExecuteAsync<InvoiceNumberSetting>(query, setting, token);
        }

    }
}
