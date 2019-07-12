using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Rac.VOne.Web.Models
{
    /// <summary>
    /// 請求残高年齢表 検索条件
    /// </summary>
    [DataContract]
    public class BillingAgingListSearch
    {
        [DataMember] public int CompanyId { get; set; }
        [DataMember] public int LoginUserId { get; set; }

        /// <summary>処理年月</summary>
        [DataMember] public DateTime YearMonth { get; set; }
        [DataMember] public int TargetDate { get; set; }
        /// <summary>締日 会計帳簿の締日(会社マスター) / 画面入力の得意先締日</summary>
        [DataMember] public int? ClosingDay { get; set; }
        /// <summary>通貨ID</summary>
        [DataMember] public int? CurrencyId { get; set; }
        [DataMember] public string DepartmentCodeFrom { get; set; }
        [DataMember] public string DepartmentCodeTo { get; set; }
        [DataMember] public string StaffCodeFrom { get; set; }
        [DataMember] public string StaffCodeTo { get; set; }
        [DataMember] public string CustomerCodeFrom { get; set; }
        [DataMember] public string CustomerCodeTo { get; set; }
        [DataMember] public string DepartmentNameFrom { get; set; }
        [DataMember] public string DepartmentNameTo { get; set; }
        [DataMember] public string StaffNameFrom { get; set; }
        [DataMember] public string StaffNameTo { get; set; }

        /// <summary>帳票設定 担当者集計方法 得意先マスターか否か</summary>
        [DataMember] public bool IsMasterStaff { get; set; }
        /// <summary>帳票設定 担当者計 出力有無</summary>
        [DataMember] public bool RequireStaffSubtotal { get; set; }
        /// <summary>帳票設定 請求部門計 出力有無</summary>
        [DataMember] public bool RequireDepartmentSubtotal { get; set; }
        /// <summary>帳票設定 債権代表者考慮要否
        /// 得意先集計方法 債権代表者/得意先, 債権代表者 の場合に true</summary>
        [DataMember] public bool ConsiderCustomerGroup { get; set; }
        /// <summary>帳票設定 請求残高計算方法
        ///  0 : 消込額を使用
        ///  1 : 入金額を使用
        ///  2 : 入金額を表示し、消込額を使用
        /// </summary>
        [DataMember] public int BillingRemainType { get; set; }
        /// <summary>帳票設定 金額単位の設定</summary>
        [DataMember] public decimal UnitValue { get; set; } = 1M;

        /// <summary>処理年月をセットする処理
        /// 事前に得意先毎の締日指定がある場合はセットしておくこと</summary>
        /// <param name="ym">画面で指定する処理年月</param>
        /// <param name="closingDay">会社マスター 締日</param>
        public void SetYearMonth(DateTime ym, int closingDay)
        {
            var day = ClosingDay ?? closingDay;
            if (day < 1 || 28 <= day) day = DateTime.DaysInMonth(ym.Year, ym.Month);
            YearMonth = new DateTime(ym.Year, ym.Month, day);
        }
        /// <summary>ym0f .. ym4t の値を設定 [DataMember]ではないので、web service 側で処理実行前に呼び出しが必要</summary>
        public void InitializeYearMonthConditions()
        {
            var dat = YearMonth;
            ym0t = dat;
            ym0f = ym0t.AddDays(1).AddMonths(-1);
            ym1t = ym0f.AddDays(-1);
            ym1f = ym1t.AddDays(1).AddMonths(-1);
            ym2t = ym1f.AddDays(-1);
            ym2f = ym2t.AddDays(1).AddMonths(-1);
            ym3t = ym2f.AddDays(-1);
            ym3f = ym3t.AddDays(1).AddMonths(-1);
            ym4t = ym3f.AddDays(-1);
            ym4f = new DateTime(2000, 1, 1);
        }
        public string MonthlyRemain0 => $"{YearMonth:MM}月発生額";
        public string MonthlyRemain1 => $"{YearMonth.AddMonths(-1):MM}月発生額";
        public string MonthlyRemain2 => $"{YearMonth.AddMonths(-2):MM}月発生額";
        public string MonthlyRemain3 => $"{YearMonth.AddMonths(-3):MM}月以前発生額";
        [DataMember] public int Precision { get; set; }
        #region for detail
        [DataMember] public int CustomerId { get; set; }
        [DataMember] public int MonthOffset { get; set; }

        #endregion
        /// <summary>帳票設定 集計基準日 : 0 : 請求日, 1 : 売上日</summary>
        public bool UseBilledAt { get { return TargetDate == 0; } }

        #region for query
        public DateTime ym0f { get; set; }
        public DateTime ym0t { get; set; }
        public DateTime ym1f { get; set; }
        public DateTime ym1t { get; set; }
        public DateTime ym2f { get; set; }
        public DateTime ym2t { get; set; }
        public DateTime ym3f { get; set; }
        public DateTime ym3t { get; set; }
        public DateTime ym4f { get; set; }
        public DateTime ym4t { get; set; }

        /// <summary>請求残高年齢表 明細表示で利用</summary>
        public DateTime ymf =>
            MonthOffset == 0 ? ym0f :
            MonthOffset == 1 ? ym1f :
            MonthOffset == 2 ? ym2f : ym4f;
        /// <summary>請求残高年齢表 明細表示で利用</summary>
        public DateTime ymt =>
            MonthOffset == 0 ? ym0t :
            MonthOffset == 1 ? ym1t :
            MonthOffset == 2 ? ym2t :
            MonthOffset == 3 ? ym3t : ym0t;

        #endregion


        public string ConnectionId { get; set; }
    }
}
