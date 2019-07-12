using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Rac.VOne.Web.Models;
using Rac.VOne.Data.QueryProcessors;

namespace Rac.VOne.Data.SqlServer.QueryProcessors
{
    public class ApplicationControlQueryProcessor :
        IAddApplicationControlQueryProcessor,
        IUpdateApplicationControlQueryProcessor
    {
        private readonly IDbHelper dbHelper;

        public ApplicationControlQueryProcessor(
            IDbHelper dbHelper)
        {
            this.dbHelper = dbHelper;
        }
        public Task<int> UpdateUseOperationLogDataAsync(ApplicationControl AppData, CancellationToken token = default(CancellationToken))
        {
            var query = @"
UPDATE ApplicationControl
   SET UseOperationLogging = @UseOperationLogging
     , UpdateBy = @UpdateBy
     , UpdateAt = GETDATE()
 WHERE CompanyId = @CompanyId
";
            return dbHelper.ExecuteAsync(query, AppData, token);
        }

        public Task< ApplicationControl> AddAsync(ApplicationControl ApplicationControl, CancellationToken token = default(CancellationToken))
        {
            #region merge query
            var query = @"
MERGE INTO
    ApplicationControl target
USING (
    SELECT
        @CompanyId CompanyId
) source ON (
        target.CompanyId = source.CompanyId
)
WHEN MATCHED THEN
    UPDATE SET
             DepartmentCodeLength       = @DepartmentCodeLength
           , DepartmentCodeType         = @DepartmentCodeType
           , SectionCodeLength          = @SectionCodeLength
           , SectionCodeType            = @SectionCodeType
           , AccountTitleCodeLength     = @AccountTitleCodeLength
           , AccountTitleCodeType       = @AccountTitleCodeType
           , CustomerCodeLength         = @CustomerCodeLength
           , CustomerCodeType           = @CustomerCodeType
           , LoginUserCodeLength        = @LoginUserCodeLength
           , LoginUserCodeType          = @LoginUserCodeType
           , StaffCodeLength            = @StaffCodeLength
           , StaffCodeType              = @StaffCodeType
           , UseDepartment              = @UseDepartment
           , UseScheduledPayment        = @UseScheduledPayment
           , UseReceiptSection          = @UseReceiptSection
           , UseAuthorization           = @UseAuthorization
           , UseLongTermAdvanceReceived = @UseLongTermAdvanceReceived
           , RegisterContractInAdvance  = @RegisterContractInAdvance
           , UseCashOnDueDates          = @UseCashOnDueDates
           , UseDeclaredAmount          = @UseDeclaredAmount
           , UseDiscount                = @UseDiscount
           , UseForeignCurrency         = @UseForeignCurrency
           , UseBillingFilter           = @UseBillingFilter
           , UseDistribution            = @UseDistribution
           , UseOperationLogging        = @UseOperationLogging
           , ApplicationEdition         = @ApplicationEdition
           , LimitAccessFolder          = @LimitAccessFolder
           , RootPath                   = @RootPath
           , UsePublishInvoice          = @UsePublishInvoice
           , UseHatarakuDBWebApi        = @UseHatarakuDBWebApi
           , UsePCADXWebApi             = @UsePCADXWebApi
           , UseReminder                = @UseReminder
           , UseAccountTransfer         = @UseAccountTransfer
           , UseMFWebApi                = @UseMFWebApi
           , UseClosing                 = @UseClosing
           , UseFactoring               = @UseFactoring
           , UseMfAggregation           = @UseMfAggregation
           , UpdateBy                   = @UpdateBy
           , UpdateAt                   = GETDATE()
WHEN NOT MATCHED THEN
    INSERT ( CompanyId
          ,  DepartmentCodeLength
          ,  DepartmentCodeType
          ,  SectionCodeLength
          ,  SectionCodeType
          ,  AccountTitleCodeLength
          ,  AccountTitleCodeType
          ,  CustomerCodeLength
          ,  CustomerCodeType
          ,  LoginUserCodeLength
          ,  LoginUserCodeType
          ,  StaffCodeLength
          ,  StaffCodeType
          ,  UseDepartment
          ,  UseScheduledPayment
          ,  UseReceiptSection
          ,  UseAuthorization
          ,  UseLongTermAdvanceReceived
          ,  RegisterContractInAdvance
          ,  UseCashOnDueDates
          ,  UseDeclaredAmount
          ,  UseDiscount
          ,  UseForeignCurrency
          ,  UseBillingFilter
          ,  UseDistribution
          ,  UseOperationLogging
          ,  ApplicationEdition
          ,  LimitAccessFolder
          ,  RootPath
          ,  UsePublishInvoice
          ,  UseHatarakuDBWebApi
          ,  UsePCADXWebApi
          ,  UseReminder
          ,  UseAccountTransfer
          ,  UseMFWebApi
          ,  UseClosing
          ,  UseFactoring
          ,  UseMfAggregation
          ,  CreateBy
          ,  CreateAt
          ,  UpdateBy
          ,  UpdateAt
    ) VALUES (
            @CompanyId
          , @DepartmentCodeLength
          , @DepartmentCodeType
          , @SectionCodeLength
          , @SectionCodeType
          , @AccountTitleCodeLength
          , @AccountTitleCodeType
          , @CustomerCodeLength
          , @CustomerCodeType
          , @LoginUserCodeLength
          , @LoginUserCodeType
          , @StaffCodeLength
          , @StaffCodeType
          , @UseDepartment
          , @UseScheduledPayment
          , @UseReceiptSection
          , @UseAuthorization
          , @UseLongTermAdvanceReceived
          , @RegisterContractInAdvance
          , @UseCashOnDueDates
          , @UseDeclaredAmount
          , @UseDiscount
          , @UseForeignCurrency
          , @UseBillingFilter
          , @UseDistribution
          , @UseOperationLogging
          , @ApplicationEdition
          , @LimitAccessFolder
          , @RootPath
          , @UsePublishInvoice
          , @UseHatarakuDBWebApi
          , @UsePCADXWebApi
          , @UseReminder
          , @UseAccountTransfer
          , @UseMFWebApi
          , @UseClosing
          , @UseFactoring
          , @UseMfAggregation
          , @CreateBy
          , GETDATE()
          , @UpdateBy
          , GETDATE()
    )
OUTPUT inserted.*;
";
            #endregion
            return dbHelper.ExecuteAsync<ApplicationControl>(query, ApplicationControl, token);
        }
    }
}
