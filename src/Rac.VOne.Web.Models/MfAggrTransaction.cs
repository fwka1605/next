using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Rac.VOne.Web.Models
{
    [DataContract] public class MfAggrTransaction : ITransactional
    {
        [DataMember] public long Id { get; set; }
        [DataMember] public long? ReceiptId { get; set; }
        [DataMember] public int CompanyId { get; set; }
        [DataMember] public int CurrencyId { get; set; }
        [DataMember] public decimal Amount { get; set; }
        [DataMember] public long AccountId { get; set; }
        [DataMember] public long SubAccountId { get; set; }
        [DataMember] public string Content { get; set; }
        [DataMember] public string PayerCode { get; set; } = string.Empty;
        [DataMember] public string PayerName { get; set; } = string.Empty;
        [DataMember] public string PayerNameRaw { get; set; } = string.Empty;
        [DataMember] public DateTime RecordedAt { get; set; }
        [DataMember] public DateTime MfCreatedAt { get; set; }
        [DataMember] public decimal Rate { get; set; }
        [DataMember] public decimal ConvertedAmount { get; set; }
        [DataMember] public int ToCurrencyId { get; set; }
        [DataMember] public int? ExcludeCategoryId { get; set; }
        [DataMember] public int CreateBy { get; set; }
        [DataMember] public DateTime CreateAt { get; set; }

        [DataMember] public string AccountName { get; set; }
        [DataMember] public string SubAccountName { get; set; }
        [DataMember] public string AccountTypeName { get; set; }
        [DataMember] public string AccountNumber { get; set; }

        /// <summary>グリッド表示 削除用</summary>
        public bool Checked { get; set; }

        /// <summary>金額で判定 入金か否か</summary>
        public bool IsIncome => Amount > 0M;
        /// <summary>グリッド表示用 入金の場合値を表示 出金の場合 null</summary>
        public decimal? IncomeAmount    => IsIncome ? Amount as decimal? : null;
        /// <summary>グリッド表示用 出金の場合 -1 を掛けた 正の値を表示 入金の場合 null</summary>
        public decimal? OutgoingsAmount => IsIncome ? null   as decimal? : -1m * Amount;


        public Receipt ConvertReceipt(Func<long, MfAggrAccount> accountGetter, Func<long, MfAggrSubAccount> subAccountGetter)
        {
            var account     = accountGetter?.Invoke(AccountId);
            var subAccount  = subAccountGetter?.Invoke(SubAccountId);
            // 除外カナ設定
            var isExcluded = ExcludeCategoryId.HasValue;
            var receipt = new Receipt {
                CompanyId               = CompanyId,
                CurrencyId              = CurrencyId,
                ReceiptCategoryId       = subAccount.ReceiptCategoryId,
                SectionId               = subAccount.SectionId,
                InputType               = (int)Rac.VOne.Common.Constants.ReceiptInputType.MfAggregation,
                Apportioned             = 1,
                Approved                = 1,
                Workday                 = RecordedAt,
                RecordedAt              = RecordedAt,
                ReceiptAmount           = Amount,
                RemainAmount            = isExcluded ? 0M     : Amount,
                AssignmentAmount        = isExcluded ? Amount : 0M    ,
                PayerCode               = PayerCode,    // TODO: set payer code
                PayerName               = PayerName,    // TODO: 半角カナだけ
                PayerNameRaw            = PayerNameRaw, // TODO: 半角カナだけ
                Note1                   = Content,
                BankCode                = account?.BankCode ?? "",
                BankName                = account?.DisplayName ?? "",
                BranchCode              = subAccount?.BranchCode ?? "",
                BranchName              = subAccount?.Name ?? "",
                AccountTypeId           = subAccount?.AccountTypeId,
                AccountNumber           = subAccount?.AccountNumber ?? "",
                AccountName             = subAccount?.Name ?? "", // TODO: 消すかも
                ExcludeFlag             = isExcluded ? 1 : 0,
                ExcludeCategoryId       = ExcludeCategoryId,
                ExcludeAmount           = isExcluded ? Amount : 0M,
                CreateBy                = CreateBy,
                UpdateBy                = CreateBy,
            };
            return receipt;
        }
    }
    [DataContract] public class MfAggrTransactionsResult : IProcessResult
    {
        [DataMember] public List<MfAggrTransaction> Transactions { get; set; }
        [DataMember] public ProcessResult ProcessResult { get; set; }
    }
}
