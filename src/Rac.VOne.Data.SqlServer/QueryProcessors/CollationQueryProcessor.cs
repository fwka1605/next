using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Rac.VOne.Data.QueryProcessors;
using Rac.VOne.Web.Models;
using static Rac.VOne.Common.Constants;

namespace Rac.VOne.Data.SqlServer.QueryProcessors
{
    public class CollationQueryProcessor : ICollationQueryProcessor
    {
        private readonly IDbHelper dbHelper;
        public CollationQueryProcessor(IDbHelper dbHelper)
        {
            this.dbHelper = dbHelper;
        }
        public Task<int> InitializeAsync(CollationSearch collationSearch, CancellationToken token = default(CancellationToken))
        {
            var query = @"
EXECUTE [dbo].[uspCollationInitialize]
  @ClientKey
, @CompanyId
, @CurrencyId
, @RecordedAtFrom
, @RecordedAtTo
, @DueAtFrom
, @DueAtTo
, @BillingType
, @LimitDateType
, @AmountType
, @UseDepartmentWork, @UseSectionWork
";
            return dbHelper.ExecuteAsync(query, collationSearch, token);
        }

        public Task<int> CollatePayerCodeAsync(CollationSearch collationSearch, CancellationToken token = default(CancellationToken))
        {
            var query = @"
EXECUTE [dbo].[uspCollationPayerCode] @ClientKey
";
            return dbHelper.ExecuteAsync(query, collationSearch, token);
        }

        public Task<int> CollateCustomerIdAsync(CollationSearch collationSearch, CancellationToken token = default(CancellationToken))
        {
            var query = @"
EXECUTE [dbo].[uspCollationCustomerId] @ClientKey
";
            return dbHelper.ExecuteAsync(query, collationSearch, token);
        }

        public Task<int> CollateHistoryAsync(CollationSearch collationSearch, CancellationToken token = default(CancellationToken))
        {
            var query = @"
EXECUTE [dbo].[uspCollationHistory] @ClientKey
";
            return dbHelper.ExecuteAsync(query, collationSearch, token);
        }

        public Task<int> CollatePayerNameAsync(CollationSearch collationSearch, CancellationToken token = default(CancellationToken))
        {
            var query = @"
EXECUTE [dbo].[uspCollationPayerName] @ClientKey
";
            return dbHelper.ExecuteAsync(query, collationSearch, token);
        }

        public Task<int> CollateKeyAsync(CollationSearch collationSearch, CancellationToken token = default(CancellationToken))
        {
            var query = @"
EXECUTE [dbo].[uspCollationKey] @ClientKey
";
            return dbHelper.ExecuteAsync(query, collationSearch, token);
        }

        public Task<int> CollateMissingAsync(CollationSearch collationSearch, CancellationToken token = default(CancellationToken))
        {
            var query = @"
EXECUTE [dbo].[uspCollationMissing] @ClientKey
";
            return dbHelper.ExecuteAsync(query, collationSearch, token);
        }
        public Task<IEnumerable<Collation>> GetItemsAsync(CollationSearch option, CancellationToken token = default(CancellationToken))
        {
            var receiptDisplayOrder = "";
            switch (option.SortOrderDirection)
            {
                case SortOrderColumnType.MinRecordedAt:
                    receiptDisplayOrder = "COALESCE(wc.MinReceiptRecordedAt, '9999/12/31') ASC";
                    break;
                case SortOrderColumnType.MaxRecordedAt:
                    receiptDisplayOrder = "COALESCE(wc.MaxReceiptRecordedAt, '9999/12/31') DESC";
                    break;
                case SortOrderColumnType.MinReceiptId:
                    receiptDisplayOrder = "wc.MinReceiptId ASC";
                    break;
                case SortOrderColumnType.MaxReceiptId:
                    receiptDisplayOrder = "wc.MaxReceiptId DESC";
                    break;
                case SortOrderColumnType.PayerNameDesc:
                    receiptDisplayOrder = "wc.PayerName DESC";
                    break;
                default:
                    receiptDisplayOrder = "wc.PayerName ASC";
                    break;
            }

            var query = $@"
SELECT wc.BillingAmount
     , wc.BillingCount
     , wc.ReceiptAmount
     , wc.ReceiptCount
     , wc.ParentCustomerId
     , wc.ParentCustomerId [CustomerId]
     , wc.PaymentAgencyId
     , wc.PayerName
     , wc.CurrencyId
     , wc.AdvanceReceivedCount
     , cs.Code [CustomerCode]
     , cs.Name [CustomerName]
     , pa.Code [PaymentAgencyCode]
     , pa.Name [PaymentAgencyName]
     , cs.IsParent
     , cy.Code [CurrencyCode]
     , cy.DisplayOrder [DisplayOrder]
     , cy.Tolerance [CurrencyTolerance]
     , COALESCE(pa.ShareTransferFee, cs.ShareTransferFee) [ShareTransferFee]
     , COALESCE(pa.UseFeeTolerance , cs.UseFeeTolerance)  [UseFeeTolerance]
     , COALESCE(pa.UseFeeLearning  , cs.UseFeeLearning )  [UseFeeLearning]
     , COALESCE(pa.UseKanaLearning , cs.UseKanaLearning)  [UseKanaLearning]
     , COALESCE(pa.Code, cs.Code) [DispCustomerCode]
     , COALESCE(pa.Name, cs.Name) [DispCustomerName]
     , COALESCE(pa.Kana, cs.Kana) [DispCustomerKana]
     , CASE wc.BillingCount WHEN 0 THEN NULL ELSE wc.BillingAmount END [DispBillingAmount]
     , CASE wc.ReceiptCount WHEN 0 THEN NULL ELSE wc.ReceiptAmount END [DispReceiptAmount]
     , CASE wc.BillingCount WHEN 0 THEN NULL ELSE wc.BillingCount  END [DispBillingCount]
     , CASE wc.ReceiptCount WHEN 0 THEN NULL ELSE wc.ReceiptCount  END [DispReceiptCount]
     , CASE wc.BillingCount WHEN 0 THEN 1    ELSE 0                END [BillingPriority]
     , CASE wc.ReceiptCount WHEN 0 THEN 1    ELSE 0                END [ReceiptPriority]
     , wc.ForceMatchingIndividually
     , 0 [Checked]
     , 0 [BankTransferFee]
     , CASE WHEN dupe.[CurrencyId] IS NULL THEN 0 ELSE 1 END [DupeCheck]
     , cs.PrioritizeMatchingIndividually
     , ROW_NUMBER() OVER (ORDER BY COALESCE(pa.Code, cs.Code))    [BillingDisplayOrder]
     , ROW_NUMBER() OVER (ORDER BY {receiptDisplayOrder} )        [ReceiptDisplayOrder]
 FROM (
      SELECT wc.CurrencyId
           , wc.ParentCustomerId
           , wc.PaymentAgencyId
           , SUM(DISTINCT BillingCount) [BillingCount]
           , SUM(DISTINCT CASE @AmountType WHEN 0 THEN BillingAmount ELSE BillingRemainAmount END) [BillingAmount]
           , SUM(ReceiptCount) [ReceiptCount]
           , SUM(ReceiptRemainAmount) [ReceiptAmount]
           , CASE SUM(AdvanceReceivedCount) WHEN 0 THEN 0 WHEN SUM(ReceiptCount) THEN 2 ELSE 1 END [AdvanceReceivedCount]
           , CASE MIN(COALESCE(wc.PayerName, '')) WHEN '' THEN NULL ELSE MIN(COALESCE(wc.PayerName, '')) END [PayerName]
           , MAX(wc.ForceMatchingIndividually) [ForceMatchingIndividually]
           , MIN(wc.MinReceiptRecordedAt) [MinReceiptRecordedAt]
           , MAX(wc.MaxReceiptRecordedAt) [MaxReceiptRecordedAt]
           , MIN(wc.MinReceiptId) [MinReceiptId]
           , MAX(wc.MaxReceiptId) [MaxReceiptId]
        FROM WorkCollation wc
       WHERE wc.ClientKey = @ClientKey
         AND wc.CompanyId = @CompanyId
       GROUP BY
             wc.CurrencyId
           , wc.ParentCustomerId
           , wc.PaymentAgencyId
           , CASE WHEN wc.ParentCustomerId = 0 AND wc.PaymentAgencyId = 0 THEN wc.PayerName END
       ) wc
  LEFT JOIN Customer cs      ON cs.Id         = wc.ParentCustomerId
  LEFT JOIN PaymentAgency pa ON pa.Id         = wc.PaymentAgencyId
  LEFT JOIN Currency cy      ON cy.Id         = wc.CurrencyId
  LEFT JOIN (
       SELECT DISTINCT
              wc.CurrencyId
            , wc.ParentCustomerId
            , wc.PaymentAgencyId
         FROM (
              SELECT wc.PayerName
                   , wc.PayerCode
                   , wc.BankCode
                   , wc.BranchCode
                   , wc.SourceBankName
                   , wc.SourceBranchName
                   , wc.CustomerId
                   , wc.CollationKey
                   , wc.CurrencyId
                FROM WorkCollation wc
               WHERE wc.ClientKey         = @ClientKey
                 AND wc.CompanyId         = @CompanyId
                 AND wc.CurrencyId        > 0
                 AND wc.ParentCustomerId >= 0
                 AND wc.PaymentAgencyId  >= 0
                 AND wc.CollationType     > 0
               GROUP BY
                     wc.PayerName
                   , wc.PayerCode
                   , wc.BankCode
                   , wc.BranchCode
                   , wc.SourceBankName
                   , wc.SourceBranchName
                   , wc.CustomerId
                   , wc.CollationKey
                   , wc.CurrencyId
              HAVING COUNT(1) > 1
              ) dupe
        INNER JOIN WorkCollation wc
           ON wc.PayerName         = dupe.PayerName
          AND wc.PayerCode         = dupe.PayerCode
          AND wc.BankCode          = dupe.BankCode
          ANd wc.BranchCode        = dupe.BranchCode
          AND wc.SourceBankName    = dupe.SourceBankName
          AND wc.SourceBranchName  = dupe.SourceBranchName
          AND wc.CustomerId        = dupe.CustomerId
          AND wc.CollationKey      = dupe.CollationKey
          AND wc.CurrencyId        = dupe.CurrencyId
          AND wc.ClientKey         = @ClientKey
          AND wc.CompanyId         = @CompanyId
          AND wc.CurrencyId        > 0
          AND wc.ParentCustomerId >= 0
          AND wc.PaymentAgencyId  >= 0
          AND wc.CollationType     > 0
       ) dupe
    ON wc.CurrencyId        = dupe.CurrencyId
   AND wc.ParentCustomerId  = dupe.ParentCustomerId
   AND wc.PaymentAgencyId   = dupe.PaymentAgencyId
";
            return dbHelper.GetItemsAsync<Collation>(query, option, token);
        }
    }
}
