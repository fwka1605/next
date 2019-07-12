using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Rac.VOne.Web.Models;
using Rac.VOne.Data.QueryProcessors;

namespace Rac.VOne.Data.SqlServer.QueryProcessors
{
    public class InvoiceTemplateSettingQueryProcessor :
        IInvoiceTemplateSettingQueryProcessor,
        IAddInvoiceTemplateSettingQueryProcessor
    {
        private readonly IDbHelper dbHelper;

        public InvoiceTemplateSettingQueryProcessor(IDbHelper dbHelper)
        {
            this.dbHelper = dbHelper;
        }


        public async Task<bool> ExistCollectCategoryAsync(int CollectCategoryId, CancellationToken token = default(CancellationToken))
        {
            var query = @"
SELECT TOP(1) 1
 FROM InvoiceTemplateSetting
WHERE
 CollectCategoryId = @CollectCategoryId";
            return (await dbHelper.ExecuteAsync<int?>(query, new { CollectCategoryId }, token)).HasValue;
        }

        public Task<InvoiceTemplateSetting> SaveAsync(InvoiceTemplateSetting setting, CancellationToken token = default(CancellationToken))
        {
            #region merge query
            var query = @"
MERGE INTO InvoiceTemplateSetting target
USING (
    SELECT @CompanyId   [CompanyId]
          ,@Code        [Code]
) source
ON    (
        target.CompanyId    = source.CompanyId
    AND target.Code         = source.Code
)
WHEN MATCHED THEN
    UPDATE SET 
         Name     = @Name
        ,CollectCategoryId  = @CollectCategoryId
        ,Title              = @Title
        ,Greeting           = @Greeting
        ,DisplayStaff       = @DisplayStaff
        ,DueDateComment     = @DueDateComment
        ,DueDateFormat      = @DueDateFormat
        ,TransferFeeComment = @TransferFeeComment
        ,FixedString        = @FixedString
        ,UpdateBy           = @UpdateBy
        ,UpdateAt           = GETDATE()
WHEN NOT MATCHED THEN 
    INSERT (CompanyId, Code, Name, CollectCategoryId, Title, Greeting, DisplayStaff, DueDateComment, DueDateFormat, TransferFeeComment, FixedString, CreateBy, CreateAt, UpdateBy, UpdateAt) 
    VALUES (@CompanyId, @Code, @Name, @CollectCategoryId, @Title, @Greeting, @DisplayStaff, @DueDateComment, @DueDateFormat, @TransferFeeComment, @FixedString, @UpdateBy, GETDATE(), @UpdateBy, GETDATE()) 
OUTPUT inserted.*; ";
            #endregion

            return dbHelper.ExecuteAsync<InvoiceTemplateSetting>(query, setting, token);
        }

    }
}
