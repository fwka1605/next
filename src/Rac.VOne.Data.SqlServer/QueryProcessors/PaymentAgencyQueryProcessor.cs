using System.Collections.Generic;
using System.Linq;
using Rac.VOne.Web.Models;
using Rac.VOne.Data.QueryProcessors;
using System.Threading;
using System.Threading.Tasks;

namespace Rac.VOne.Data.SqlServer.QueryProcessors
{
    public class PaymentAgencyQueryProcessor :
        IAddPaymentAgencyQueryProcessor
    {
        private readonly IDbHelper dbHelper;

        public PaymentAgencyQueryProcessor(IDbHelper dbHelper)
        {
            this.dbHelper = dbHelper;
        }

        public Task<PaymentAgency> SaveAsync(PaymentAgency paymentAgency, CancellationToken token = default(CancellationToken))
        {
            var query = @"
MERGE INTO PaymentAgency AS Pa
USING ( 
    SELECT 
        @CompanyId AS CompanyId,
        @Code as Code

) AS Target 
ON ( 
    Pa.CompanyId = @CompanyId 
    AND Pa.Code = @Code
) 
WHEN MATCHED THEN 
    UPDATE SET 
    ConsigneeCode       = @ConsigneeCode
,   Name                = @Name
,   Kana                = @Kana
,   BankCode            = @BankCode
,   BankName            = @BankName
,   BranchCode          = @BranchCode
,   BranchName          = @BranchName
,   AccountTypeId       = @AccountTypeId
,   AccountNumber       = @AccountNumber
,   ShareTransferFee    = @ShareTransferFee
,   UseFeeTolerance     = @UseFeeTolerance
,   UseFeeLearning      = @UseFeeLearning
,   UseKanaLearning     = @UseKanaLearning
,   DueDateOffset       = @DueDateOffset
,   FileFormatId        = @FileFormatId
,   ConsiderUncollected = @ConsiderUncollected
,   CollectCategoryId   = @CollectCategoryId
,   UpdateBy            = @UpdateBy
,   UpdateAt            = GETDATE()
,   OutputFileName      = @OutputFileName
,   AppendDate          = @AppendDate
,   ContractCode        = @ContractCode
WHEN NOT MATCHED THEN 
INSERT (CompanyId
     , Code
     , ConsigneeCode
     , Name
     , Kana
     , BankCode
     , BankName
     , BranchCode
     , BranchName
     , AccountTypeId
     , AccountNumber
     , ShareTransferFee
     , UseFeeTolerance
     , UseFeeLearning
     , UseKanaLearning
     , DueDateOffset
     , FileFormatId
     , ConsiderUncollected
     , CollectCategoryId
     , CreateBy
     , CreateAt
     , UpdateBy
     , UpdateAt
     , OutputFileName
     , AppendDate
     , ContractCode
      )
VALUES (@CompanyId
     , @Code
     , @ConsigneeCode
     , @Name
     , @Kana
     , @BankCode
     , @BankName
     , @BranchCode
     , @BranchName
     , @AccountTypeId
     , @AccountNumber
     , @ShareTransferFee
     , @UseFeeTolerance
     , @UseFeeLearning
     , @UseKanaLearning
     , @DueDateOffset
     , @FileFormatId
     , @ConsiderUncollected
     , @CollectCategoryId
     , @CreateBy
     , GETDATE()
     , @UpdateBy
     , GETDATE()
     , @OutputFileName
     , @AppendDate
     , @ContractCode
)
OUTPUT inserted.*;";
            return dbHelper.ExecuteAsync<PaymentAgency>(query, paymentAgency, token);
        }

    }
}
