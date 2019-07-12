using Rac.VOne.Data.QueryProcessors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Rac.VOne.Web.Models;

namespace Rac.VOne.Data.SqlServer.QueryProcessors
{
    public class CollationSettingQueryProcessor :
        ICollationSettingByCompanyIdQueryProcessor,
        IAddCollationSettingQueryProcessor
    {
        private readonly IDbHelper dbHelper;

        public CollationSettingQueryProcessor(IDbHelper dbHelper)
        {
            this.dbHelper = dbHelper;
        }

        public Task<CollationSetting> SaveAsync(CollationSetting CollationSetting, CancellationToken token = default(CancellationToken))
        {
            #region insert query
            var query = @"
INSERT INTO CollationSetting
(CompanyId
,RequiredCustomer
,AutoAssignCustomer
,LearnKanaHistory
,UseApportionMenu
,ReloadCollationData
,UseAdvanceReceived
,AdvanceReceivedRecordedDateType
,AutoMatching
,AutoSortMatchingEnabledData
,PrioritizeMatchingIndividuallyMultipleReceipts
,ForceShareTransferFee
,LearnSpecifiedCustomerKana
,MatchingSilentSortedData
,BillingReceiptDisplayOrder
,RemoveSpaceFromPayerName
,PrioritizeMatchingIndividuallyTaxTolerance
,JournalizingPattern
,CalculateTaxByInputId
,UseFromToNarrowing
,SetSystemDateToCreateAtFilter
,SortOrderColumn
,SortOrder
)
OUTPUT inserted.*
VALUES
(@CompanyId
,@RequiredCustomer
,@AutoAssignCustomer
,@LearnKanaHistory
,@UseApportionMenu
,@ReloadCollationData
,@UseAdvanceReceived
,@AdvanceReceivedRecordedDateType
,@AutoMatching
,@AutoSortMatchingEnabledData
,@PrioritizeMatchingIndividuallyMultipleReceipts
,@ForceShareTransferFee
,@LearnSpecifiedCustomerKana
,@MatchingSilentSortedData
,@BillingReceiptDisplayOrder
,@RemoveSpaceFromPayerName
,@PrioritizeMatchingIndividuallyTaxTolerance
,@JournalizingPattern
,@CalculateTaxByInputId
,@UseFromToNarrowing
,@SetSystemDateToCreateAtFilter
,@SortOrderColumn
,@SortOrder
)";
            #endregion
            return dbHelper.ExecuteAsync<CollationSetting>(query, CollationSetting, token);
        }

        public Task<CollationOrder> SaveCollationOrderAsync(CollationOrder CollationOrder, CancellationToken token = default(CancellationToken))
        {
            var query = @"
INSERT INTO CollationOrder
(CompanyId,
 CollationTypeId,
 ExecutionOrder,
 Available,
 CreateBy,
 CreateAt,
 UpdateBy,
 UpdateAt)
OUTPUT inserted.*
VALUES
(@CompanyId,
@CollationTypeId,
@ExecutionOrder,
@Available,
@CreateBy,
GETDATE(),
@UpdateBy,
GETDATE())";
            return dbHelper.ExecuteAsync<CollationOrder>(query, CollationOrder, token);
        }

        public Task<MatchingOrder> SaveMatchingOrderAsync(MatchingOrder MatchingOrder, CancellationToken token = default(CancellationToken))
        {
            var query = @"
INSERT INTO MatchingOrder
(CompanyId,
 TransactionCategory,
 ItemName,
 ExecutionOrder,
 Available,
 SortOrder,
 CreateBy,
 CreateAt,
 UpdateBy,
 UpdateAt)
OUTPUT inserted.*
VALUES(
@CompanyId,
@TransactionCategory,
@ItemName,
@ExecutionOrder,
@Available,
@SortOrder,
@CreateBy,
GETDATE(),
@UpdateBy,
GETDATE())";
            return dbHelper.ExecuteAsync<MatchingOrder>(query, MatchingOrder, token);
        }


        public Task<IEnumerable<MatchingOrder>> GetMatchingBillingOrderAsync(int CompanyId, CancellationToken token = default(CancellationToken))
        {
            var query = @"
SELECT *
  FROM MatchingOrder
 WHERE CompanyId = @CompanyId
   AND TransactionCategory = 1
 ORDER BY ExecutionOrder;";
            return dbHelper.GetItemsAsync<MatchingOrder>(query, new { CompanyId }, token);
        }

        public Task<IEnumerable<MatchingOrder>> GetMatchingReceiptOrderAsync(int CompanyId, CancellationToken token = default(CancellationToken))
        {
            var query = @"
SELECT *
  FROM MatchingOrder
 WHERE CompanyId = @CompanyId
   AND TransactionCategory = 2
 ORDER BY ExecutionOrder;";
            return dbHelper.GetItemsAsync<MatchingOrder>(query, new { CompanyId }, token);
        }

    }
}
