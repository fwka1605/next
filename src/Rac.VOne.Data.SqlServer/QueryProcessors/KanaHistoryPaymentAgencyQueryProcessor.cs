using System;
using System.Threading;
using System.Threading.Tasks;
using Rac.VOne.Web.Models;
using Rac.VOne.Data.QueryProcessors;
using System.Collections.Generic;


namespace Rac.VOne.Data.SqlServer.QueryProcessors
{
    public class KanaHistoryPaymentAgencyQueryProcessor :
        IKanaHistoryPaymentAgencyQueryProcessor,
        IAddKanaHistoryPaymentAgencyQueryProcessor,
        IUpdateKanaHistoryPaymentAgencyQueryProcessor,
        IDeleteKanaHistoryPaymentAgencyQueryProcessor
    {
        private readonly IDbHelper dbHelper;

        public KanaHistoryPaymentAgencyQueryProcessor(IDbHelper dbHelper)
        {
            this.dbHelper = dbHelper;
        }

        public Task<IEnumerable<KanaHistoryPaymentAgency>> GetAsync(KanaHistorySearch option, CancellationToken token = default(CancellationToken))
        {
            var query = @"
SELECT      kh.*
          , pa.Code PaymentAgencyCode
          , pa.Name PaymentAgencyName
FROM        KanaHistoryPaymentAgency kh
INNER JOIN  PaymentAgency pa            ON pa.Id    = kh.PaymentAgencyId
WHERE       kh.CompanyId        = @CompanyId";
            if (!string.IsNullOrWhiteSpace(option.PayerName))
            {
                option.PayerName = Sql.GetWrappedValue(option.PayerName);
                query += @"
AND         kh.PayerName        LIKE @PayerName";
            }
            if (!string.IsNullOrWhiteSpace(option.CodeFrom)) query += @"
AND         pa.Code             >= @CodeFrom";
            if (!string.IsNullOrWhiteSpace(option.CodeTo)) query += @"
AND         pa.Code             <= @CodeTo";
            query += @"
ORDER BY    pa.Code             ASC
          , kh.PayerName        ASC";
            return dbHelper.GetItemsAsync<KanaHistoryPaymentAgency>(query, option, token);
        }


        public Task<KanaHistoryPaymentAgency> SaveAsync(KanaHistoryPaymentAgency history, CancellationToken token = default(CancellationToken))
        {
            var query = @"
MERGE INTO KanaHistoryPaymentAgency AS target
USING (
    SELECT @CompanyId         CompanyId
         , @PayerName         PayerName
         , @SourceBankName    SourceBankName
         , @SourceBranchName  SourceBranchName
         , @PaymentAgencyId   PaymentAgencyId
) AS source
ON (
        target.CompanyId        = source.CompanyId
    AND target.PayerName        = source.PayerName
    AND target.SourceBankName   = source.SourceBankName
    AND target.SourceBranchName = source.SourceBranchName
    AND target.PaymentAgencyId  = source.PaymentAgencyId
)
WHEN MATCHED THEN
    UPDATE
       SET HitCount = @HitCount
         , UpdateBy = @UpdateBy
         , UpdateAt = GETDATE()
WHEN NOT MATCHED THEN
    INSERT ( CompanyId,  PayerName,  SourceBankName,  SourceBranchName,  PaymentAgencyId,  HitCount,  CreateBy, CreateAt, UpdateBy, UpdateAt)
    VALUES (@CompanyId, @PayerName, @SourceBankName, @SourceBranchName, @PaymentAgencyId, @HitCount, @UpdateBy, GETDATE(), @UpdateBy, GETDATE())
OUTPUT inserted.*;
";
            return dbHelper.ExecuteAsync<KanaHistoryPaymentAgency>(query, history, token);
        }

        public Task<int> UpdateAsync(KanaHistoryPaymentAgency KanaHistoryPayment, CancellationToken token = default(CancellationToken))
        {
            var query = @"
MERGE INTO KanaHistoryPaymentAgency AS target
USING (
    SELECT @CompanyId         CompanyId
         , @PayerName         PayerName
         , @SourceBankName    SourceBankName
         , @SourceBranchName  SourceBranchName
         , @PaymentAgencyId   PaymentAgencyId
) AS source
ON (
        target.CompanyId        = source.CompanyId
    AND target.PayerName        = source.PayerName
    AND target.SourceBankName   = source.SourceBankName
    AND target.SourceBranchName = source.SourceBranchName
    AND target.PaymentAgencyId  = source.PaymentAgencyId
)
WHEN MATCHED THEN
    UPDATE
       SET HitCount = HitCount + @HitCount
         , UpdateBy = @UpdateBy
         , UpdateAt = @UpdateAt
WHEN NOT MATCHED THEN
    INSERT ( CompanyId,  PayerName,  SourceBankName,  SourceBranchName,  PaymentAgencyId,  HitCount,  CreateBy,  CreateAt,  UpdateBy,  UpdateAt)
    VALUES (@CompanyId, @PayerName, @SourceBankName, @SourceBranchName, @PaymentAgencyId, @HitCount, @CreateBy, @CreateAt, @UpdateBy, @UpdateAt)
;
";
            return dbHelper.ExecuteAsync(query, KanaHistoryPayment, token);
        }


        public Task<int> DeleteAsync(KanaHistoryPaymentAgency history, CancellationToken token = default(CancellationToken))
        {
            var query = @"
DELETE KanaHistoryPaymentAgency 
 WHERE CompanyId        = @CompanyId
   AND PayerName        = @PayerName
   AND SourceBankName   = @SourceBankName
   AND SourceBranchName = @SourceBranchName
   AND PaymentAgencyId  = @PaymentAgencyId";
            return dbHelper.ExecuteAsync(query, history, token);
        }

    }
}
