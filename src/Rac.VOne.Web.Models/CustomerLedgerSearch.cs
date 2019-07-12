using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Rac.VOne.Web.Models
{
    [DataContract]
    public class CustomerLedgerSearch
    {
        /// <summary>得意先コード From</summary>
        [DataMember] public string CustomerCodeFrom { get; set; }
        /// <summary>得意先コード To</summary>
        [DataMember] public string CustomerCodeTo { get; set; }
        /// <summary>処理年月 開始
        /// 日付部分は 会計締日 + 1 となるように指定
        /// 末日(99)の場合は、指定年月の1日（指定年月の1月前 + 1日)</summary>
        [DataMember] public DateTime YearMonthFrom { get; set; }
        /// <summary>処理年月 終了
        /// 日付部分は 会計締日 となるように指定</summary>
        [DataMember] public DateTime YearMonthTo { get; set; }
        /// <summary>得意先 締日</summary>
        [DataMember] public int? ClosingDay { get; set; }
        /// <summary>会社ID 必須</summary>
        [DataMember] public int CompanyId { get; set; }
        /// <summary>通貨ID 必須</summary>
        [DataMember] public int CurrencyId { get; set; }


        /// <summary>帳票設定　集計基準日： 1 ：請求日</summary>
        [DataMember] public bool UseBilledAt { get; set; }

        /// <summary>帳票設定 請求残高計算方法
        /// 0 : 消込額を使用する
        /// 1 : 入金額を使用する
        /// 2 : 消込額を使用して入金額を表示する</summary>
        [DataMember] public int RemainType { get; set; }

        /// <summary>入金額を使用する</summary>
        public bool UseReceipt { get { return RemainType == 1; } }

        /// <summary>入金データの表示 要否</summary>
        public bool RequireReceiptData { get { return RemainType != 0; } }

        /// <summary>帳票設定 請求部門表示</summary>
        [DataMember] public bool DisplayDepartment { get; set; }
        /// <summary>帳票設定 入金部門表示</summary>
        [DataMember] public bool DisplaySection { get; set; }

        /// <summary>帳票設定 入金データ集計</summary>
        [DataMember] public bool DoGroupReceipt { get; set; }

        /// <summary>帳票設定 消込記号表示</summary>
        [DataMember] public bool DisplayMatchingSymbol { get; set; }

        /// <summary>帳票設定 請求データ集計
        /// 0 : する（請求区分）
        /// 1 : する（債権科目）
        /// 2 : しない</summary>
        [DataMember] public int GroupBillingType { get; set; }
        /// <summary>帳票設定 伝票集計方法
        /// 0 : 合計
        /// 1 : 明細（伝票合計あり）
        /// 2 : 明細（伝票合計なし）</summary>
        [DataMember] public int BillingSlipType { get; set; }

        /// <summary>帳票設定 請求データ集計：する*</summary>
        public bool RequireBillingSubtotal { get { return GroupBillingType != 2; } }
        /// <summary>請求区分 集計要否</summary>
        public bool RequireBillingCategorySubtotal { get { return GroupBillingType == 0; } }
        /// <summary>債権科目 集計要否</summary>
        public bool RequireDebitAccountTitleSubtotal { get { return GroupBillingType == 1; } }

        /// <summary>帳票設定 伝票集計方法：合計 （入力ID での 集計 要否）</summary>
        public bool RequireBillingInputIdSubotal { get { return BillingSlipType == 0; } }


        /// <summary>帳票設定 請求区分 表示</summary>
        public bool DisplayBillingCategory { get { return RequireBillingCategorySubtotal || !RequireBillingSubtotal && !RequireBillingInputIdSubotal; } }

        /// <summary>帳票設定 債権科目 表示</summary>
        public bool DisplayDebitAccountTitle { get { return RequireDebitAccountTitleSubtotal || !RequireBillingSubtotal && !RequireBillingInputIdSubotal; } }

        /// <summary>帳票設定 請求書番号 表示</summary>
        public bool DisplayInvoiceCode { get { return !RequireBillingSubtotal; } }
        /// <summary>帳票設定 伝票集計方法 伝票合計 表示</summary>
        public bool DisplaySlipTotal { get { return !RequireBillingSubtotal && BillingSlipType != 2; } }


        /// <summary>帳票設定 月次改ページ</summary>
        /// <remarks>帳票印刷時のみ 設定すること</remarks>
        [DataMember] public bool RequireMonthlyBreak { get; set; }

        /// <summary>帳票設定　単位 円</summary>
        [DataMember] public decimal UnitPrice { get; set; }

        /// <summary>印刷時 繰越設定用に必要 帳票の設定を検討する必要有</summary>
        [DataMember] public bool IsPrint { get; set; }

        [DataMember] public int Precision { get; set; }

        private const int LimitDay = 27;
        private const int LastDay = 99;
        private bool InitializeYearMonth(int closingDay, int startYear, int startMonth, int endYear, int endMonth)
        {
            var day = closingDay > LimitDay ? LastDay : closingDay;
            if (startYear < 1 || 9999 <= startYear) return false;
            if (startMonth < 1 || 12 < startMonth) return false;
            if (endYear < 1 || 9999 <= endYear) return false;
            if (endMonth < 1 || 12 < endMonth) return false;
            if (startYear == 1 && startMonth == 1) return false;

            var dat = new DateTime(startYear, startMonth,
                (day != LastDay ? day : DateTime.DaysInMonth(startYear, startMonth)));
            YearMonthFrom = dat.AddDays(1).AddMonths(-1);
            YearMonthTo = new DateTime(endYear, endMonth,
                (day != LastDay ? day : DateTime.DaysInMonth(endYear, endMonth)));
            return true;
        }

        public bool InitializeYearMonth(int closingDay, DateTime start, DateTime end)
        {
            return InitializeYearMonth(closingDay, start.Year, start.Month, end.Year, end.Month);
        }

        public string ConnectionId { get; set; }
    }
}
