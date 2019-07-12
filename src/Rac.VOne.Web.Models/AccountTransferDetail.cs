using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using static Rac.VOne.Common.Constants;

namespace Rac.VOne.Web.Models
{
    [DataContract]
    public class AccountTransferDetail : ITransactional
    {
        [DataMember] public long Id { get; set; }
        [DataMember] public long AccountTransferLogId { get; set; }
        [DataMember] public long BillingId { get; set; }
        [DataMember] public int CompanyId { get; set; }
        [DataMember] public int CustomerId { get; set; }
        [DataMember] public int DepartmentId { get; set; }
        [DataMember] public int StaffId { get; set; }
        [DataMember] public DateTime? BilledAt { get; set; }
        [DataMember] public DateTime? SalesAt { get; set; }
        [DataMember] public DateTime? ClosingAt { get; set; }
        [DataMember] public DateTime? DueAt { get; set; }
        [DataMember] public DateTime? DueAt2nd { get; set; }
        [DataMember] public decimal BillingAmount { get; set; }
        [DataMember] public string InvoiceCode { get; set; }
        [DataMember] public string Note1 { get; set; }
        [DataMember] public string TransferBankCode { get; set; }
        [DataMember] public string TransferBankName { get; set; }
        [DataMember] public string TransferBranchCode { get; set; }
        [DataMember] public string TransferBranchName { get; set; }
        [DataMember] public int TransferAccountTypeId { get; set; }
        [DataMember] public string TransferAccountNumber { get; set; }
        [DataMember] public string TransferAccountName { get; set; }
        [DataMember] public string TransferCustomerCode { get; set; }
        [DataMember] public string TransferNewCode { get; set; }
        [DataMember] public int CreateBy { get; set; }
        [DataMember] public DateTime CreateAt { get; set; }

        [DataMember] public DateTime? BillingUpdateAt { get; set; }
        [DataMember] public string CustomerCode { get; set; }
        [DataMember] public string CustomerName { get; set; }
        [DataMember] public string DepartmentCode { get; set; }
        [DataMember] public string DepartmentName { get; set; }
        [DataMember] public string StaffCode { get; set; }
        [DataMember] public string StaffName { get; set; }
        [DataMember] public int CollectCategoryId { get; set; }
        [DataMember] public int PaymentAgencyId { get; set; }
        [DataMember] public string CollectCategoryCode { get; set; }
        [DataMember] public string CollectCategoryName { get; set; }
        [DataMember] public string PaymentAgencyCode { get; set; }
        [DataMember] public string PaymentAgencyName { get; set; }

        /// <summary>
        /// 決済代行会社に登録された 口座振替用フォーマットID
        /// 1 : 全銀口座振替 カンマ区切り
        /// 2 : 全銀口座振替 固定長
        /// 3 : みずほファクター
        /// 4 : 三菱UFJファクター
        /// 5 : SMBC（口座振替 固定長）
        /// 6 : 三菱UFJニコス
        /// 7 : みずほファクター（ASPサービス）
        /// 8 : リコーリースコレクト！
        /// </summary>
        [DataMember] public int FileFormatId { get; set; }
        /// <summary>
        /// 指定した引落日 出力時に指定
        /// </summary>
        [DataMember] public DateTime? NewDueAt { get; set; }

        /// <summary>
        /// 連携されたマスターの情報等が正常かどうか
        /// </summary>
        public bool Valid
        {
            get
            {
                return ValidateBankCode()
                    && ValidateBankName()
                    && ValidateBranchCode()
                    && ValidateBranchName()
                    && ValidateAccountType()
                    && ValidateAccountNumber()
                    && ValidateAccountName()
                    && ValidateCustomerCode()
                    && ValidateCustomerCodeLength()
                    && ValidateNewCode()
                    && ValidateBillingAmount()
                    ;
            }
        }
        private bool ValidateBankCode() => !string.IsNullOrEmpty(TransferBankCode);
        private bool ValidateBankName() => !string.IsNullOrEmpty(TransferBankName);
        private bool ValidateBranchCode() => !string.IsNullOrEmpty(TransferBranchCode);
        private bool ValidateBranchName() => !string.IsNullOrEmpty(TransferBranchName);
        private bool ValidateAccountType() => TransferAccountTypeId != 0;
        private bool ValidateAccountNumber() => !string.IsNullOrEmpty(TransferAccountNumber);
        private bool ValidateAccountName() => !string.IsNullOrEmpty(TransferAccountName);
        private bool ValidateCustomerCode() => !RequireCustomerCode || !string.IsNullOrEmpty(TransferCustomerCode);
        private bool ValidateCustomerCodeLength() => TransferCustomerCode.Length <= MaxTransferCustomerCodeLength;
        private bool ValidateNewCode() =>
            FileFormatId == (int)AccountTransferFileFormatId.InternetJPBankFixed
            ? true
            : NewCodeAllowedCharacters.Contains(TransferNewCode);

        /// <summary>
        ///  最大金額 10桁 マイナス金額？
        /// </summary>
        private const decimal MaxAmount = 9999999999M;
        private bool ValidateBillingAmount() => 0M < BillingAmount && BillingAmount <= MaxAmount;


        /// <summary>
        /// 印刷用 検証不正項目のメッセージング処理をラップ
        /// </summary>
        public string DisplayBankName
        {
            get
            {
                if (!ValidateBankCode())            return "※銀行コード未設定※";
                if (!ValidateBankName())            return "※銀行名未設定※";
                if (!ValidateBranchCode())          return "※支店コード未設定※";
                if (!ValidateBranchName())          return "※支店名未設定※";
                if (!ValidateAccountType())         return "※預金種別未設定※";
                if (!ValidateAccountNumber())       return "※口座番号未設定※";
                if (!ValidateAccountName())         return "※預金者名未設定※";
                if (!ValidateCustomerCode())        return "※顧客コード未設定※";
                if (!ValidateCustomerCodeLength())  return "※顧客コード桁数超過※";
                if (!ValidateNewCode())             return "※新規コード不正※";
                if (!ValidateBillingAmount())       return "※請求金額不正※";
                return TransferBankName;
            }
        }
        public IEnumerable<string> GetInvalidLogs()
        {
            if (!ValidateBankCode())            yield return $"請求ID: {BillingId}, 銀行コード未設定";
            if (!ValidateBankName())            yield return $"請求ID: {BillingId}, 銀行名未設定";
            if (!ValidateBranchCode())          yield return $"請求ID: {BillingId}, 支店コード未設定";
            if (!ValidateBranchName())          yield return $"請求ID: {BillingId}, 支店名未設定";
            if (!ValidateAccountType())         yield return $"請求ID: {BillingId}, 預金種別未設定";
            if (!ValidateAccountNumber())       yield return $"請求ID: {BillingId}, 口座番号未設定";
            if (!ValidateAccountName())         yield return $"請求ID: {BillingId}, 預金者名未設定";
            if (!ValidateCustomerCode())        yield return $"請求ID: {BillingId}, 顧客コード未設定";
            if (!ValidateCustomerCodeLength())  yield return $"請求ID: {BillingId}, 顧客コード桁数超過: {CustomerCode}";
            if (!ValidateNewCode())             yield return $"請求ID: {BillingId}, 新規コード不正: {TransferNewCode}";
            if (!ValidateBillingAmount())       yield return $"請求ID: {BillingId}, 請求金額不正: {BillingAmount:#,0}";
        }


        /// <summary>
        /// 顧客コードの最大文字列長
        /// </summary>
        public int MaxTransferCustomerCodeLength
        {
            get
            {
                return FileFormatId == (int)AccountTransferFileFormatId.ZenginCsv
                    || FileFormatId == (int)AccountTransferFileFormatId.ZenginFixed
                    || FileFormatId == (int)AccountTransferFileFormatId.MizuhoFactorWebFixed
                    || FileFormatId == (int)AccountTransferFileFormatId.RisonaNetCsv
                    || FileFormatId == (int)AccountTransferFileFormatId.InternetJPBankFixed ? 20
                     : FileFormatId == (int)AccountTransferFileFormatId.MitsubishiUfjFactorCsv
                    || FileFormatId == (int)AccountTransferFileFormatId.RicohLeaseCollectCsv ? 15
                     : FileFormatId == (int)AccountTransferFileFormatId.MizuhoFactorAspCsv ? 14
                     : FileFormatId == (int)AccountTransferFileFormatId.SMBCFixed ? 12
                     : FileFormatId == (int)AccountTransferFileFormatId.MitsubishiUfjNicosCsv ? 7
                     : 0;
            }
        }

        /// <summary>
        /// 顧客コード 入力必須
        /// ※全銀以外は必須 追加されるフォーマットを考慮して 否定形
        /// </summary>
        public bool RequireCustomerCode
        {
            get
            {
                return !(
                    FileFormatId == (int)AccountTransferFileFormatId.ZenginCsv
                 || FileFormatId == (int)AccountTransferFileFormatId.ZenginFixed);
            }
        }
        /// <summary>
        /// 新規コード フォーマット毎の許可された数値
        /// </summary>
        public string[] NewCodeAllowedCharacters
        {
            get
            {
                return FileFormatId == (int)AccountTransferFileFormatId.ZenginCsv
                    || FileFormatId == (int)AccountTransferFileFormatId.ZenginFixed
                    || FileFormatId == (int)AccountTransferFileFormatId.RisonaNetCsv ? new string[] { "", "0", "1" }
                     : FileFormatId == (int)AccountTransferFileFormatId.MitsubishiUfjFactorCsv
                    || FileFormatId == (int)AccountTransferFileFormatId.MitsubishiUfjNicosCsv ? new string[] { "0", "1" }
                     : FileFormatId == (int)AccountTransferFileFormatId.MizuhoFactorWebFixed
                    || FileFormatId == (int)AccountTransferFileFormatId.MizuhoFactorAspCsv
                    || FileFormatId == (int)AccountTransferFileFormatId.SMBCFixed
                    || FileFormatId == (int)AccountTransferFileFormatId.RicohLeaseCollectCsv ? new string[] { "0", "1", "2" }
                     : new string[] { };
            }
        }

        /// <summary>
        /// 集約するかどうか
        /// </summary>
        public bool Aggregate { get; set; }
    }

    [DataContract]
    public class AccountTransferDetailsResult : IProcessResult
    {
        [DataMember] public ProcessResult ProcessResult { get; set; }
        [DataMember] public List<AccountTransferDetail> AccountTransferDetail { get; set; }
    }

    public static class AccountTransferDetailExtensions
    {
        /// <summary>
        /// 口座振替用のキー情報で集計 集計が必要な場合に利用
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static IEnumerable<AccountTransferDetail> AggregateByKey(this IEnumerable<AccountTransferDetail> source)
        {
            if (!(source?.Any() ?? false))
                return Enumerable.Empty<AccountTransferDetail>();

            return source.GroupBy(x => new
            {
                x.TransferBankCode,
                x.TransferBranchCode,
                x.TransferAccountTypeId,
                x.TransferAccountNumber,
                x.TransferCustomerCode,
                x.AccountTransferLogId
            }).Select((g, index) => new AccountTransferDetail
            {
                Id                      = index,
                AccountTransferLogId    = g.Key.AccountTransferLogId,
                BillingId               = g.Min(x => x.BillingId),
                CompanyId               = g.Min(x => x.CompanyId),
                CustomerId              = g.First(x => x.CustomerCode == g.Min(y => y.CustomerCode)).CustomerId,
                DepartmentId            = 0,
                StaffId                 = 0,
                BilledAt                = null,
                SalesAt                 = null,
                DueAt                   = null,
                BillingAmount           = g.Sum(x => x.BillingAmount),
                InvoiceCode             = "",
                Note1                   = "",
                TransferBankCode        = g.Key.TransferBankCode,
                TransferBankName        = g.Max(x => x.TransferBankName),
                TransferBranchCode      = g.Key.TransferBranchCode,
                TransferBranchName      = g.Max(x => x.TransferBranchName),
                TransferAccountTypeId   = g.Key.TransferAccountTypeId,
                TransferAccountNumber   = g.Key.TransferAccountNumber,
                TransferAccountName     = g.Max(x => x.TransferAccountName),
                TransferCustomerCode    = g.Key.TransferCustomerCode,
                TransferNewCode         = g.Min(x => x.TransferNewCode),
                FileFormatId            = g.Min(x => x.FileFormatId),
                CustomerCode            = g.Min(x => x.CustomerCode),
                CustomerName            = g.First(x => x.CustomerCode == g.Min(y => y.CustomerCode)).CustomerName,
            });
        }
        public static AccountTransferLog ConvertToLog(this IEnumerable<AccountTransferDetail> source, AccountTransferLog accumulate)
        {
            return source.Aggregate(accumulate, (target, x) =>
                {
                    target.OutputCount++;
                    target.OutputAmount += x.BillingAmount;
                    return target;
                });
        }
        public static AccountTransferLog GetAccumulate(this IEnumerable<AccountTransferDetail> source)
        {
            var first = source.FirstOrDefault();
            if (first == null) return null;
            return new AccountTransferLog
            {
                CompanyId           = first.CompanyId,
                CollectCategoryId   = first.CollectCategoryId,
                PaymentAgencyId     = first.PaymentAgencyId,
                DueAt               = first.NewDueAt.Value,
                CreateBy            = first.CreateBy,
                CollectCategoryCode = first.CollectCategoryCode,
                CollectCategoryName = first.CollectCategoryName,
                PaymentAgencyCode   = first.PaymentAgencyCode,
                PaymentAgencyName   = first.PaymentAgencyName,
            };
        }
    }
}
