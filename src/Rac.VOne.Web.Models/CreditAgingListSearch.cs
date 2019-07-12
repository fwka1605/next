using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Models
{
    [DataContract]
    public class CreditAgingListSearch
    {
        /// <summary>会社ID</summary>
        [DataMember] public int CompanyId { get; set; }
        /// <summary> 処理年月 日付部分は 会社別/得意先別(画面入力)の締日を設定すること</summary>
        [DataMember] public DateTime YearMonth { get; set; }
        /// <summary>得意先ごとの締日残高 締日
        /// 債権代表者グループを考慮せず</summary>
        [DataMember] public int? ClosingDay { get; set; }
        [DataMember] public string DepartmentCodeFrom { get; set; }
        [DataMember] public string DepartmentCodeTo { get; set; }
        [DataMember] public string StaffCodeFrom { get; set; }
        [DataMember] public string StaffCodeTo { get; set; }
        [DataMember] public string CustomerCodeFrom { get; set; }
        [DataMember] public string CustomerCodeTo { get; set; }
        /// <summary>与信残高が マイナスのもののみ表示
        /// 与信残高が プラスのものは除外</summary>
        [DataMember] public bool FilterPositiveCreditBalance { get; set; }
        /// <summary>債権代表者の総計額で計算
        /// 与信限度額 残高計算に債権代表者グループを考慮する</summary>
        [DataMember] public bool ConsiderGroupWithCalculate { get; set; }
        /// <summary>与信限度額が０のものは、残計算をしない
        /// 得意先マスター 与信限度額 登録済の場合だけ、与信残高計算を行う</summary>
        [DataMember] public bool CalculateCreditLimitRegistered { get; set; }

        #region 帳票設定
        /// <summary>帳票設定 請求部門計の出力</summary>
        [DataMember] public bool RequireDepartmentTotal { get; set; }
        /// <summary>帳票設定 担当者計の出力</summary>
        [DataMember] public bool RequireStaffTotal { get; set; }
        /// <summary>帳票設定 担当者集計方法 得意先マスター</summary>
        [DataMember] public bool UseMasterStaff { get; set; }
        /// <summary>帳票設定 債権代表者考慮要否
        /// 得意先集計方法 債権代表者/得意先, 債権代表者 の場合に true</summary>
        [DataMember] public bool ConsiderCustomerGroup { get; set; }
        /// <summary>帳票設定 債権総額計算方法 : 1 : 入金額を使用する</summary>
        [DataMember] public bool ConsiderReceiptAmount { get; set; }


        /// <summary>帳票設定 集計基準日 : 1 : 請求日</summary>
        [DataMember] public bool UseBilledAt { get; set; }
        /// <summary>帳票設定 単位 円</summary>
        [DataMember] public decimal UnitPrice { get; set; }
        /// <summary>帳票設定 与信限度額集計方法 : 1 : 債権代表者の与信限度額を使用する</summary>
        [DataMember] public bool UseParentCustomerCredit { get; set; }

        [DataMember] public bool DisplayCustomerCode { get; set; }
        /// <summary>処理年月の指定</summary>
        /// <param name="ym"></param>
        /// <param name="closingDay"></param>
        public void SetYearMonth(DateTime ym, int closingDay)
        {
            var day = ClosingDay ?? closingDay;
            if (day < 1 || 28 <= day) day = DateTime.DaysInMonth(ym.Year, ym.Month);
            YearMonth = new DateTime(ym.Year, ym.Month, day);
        }
        public void InitializeYearMonthValue()
        {
            var dat = YearMonth;
            ym0f = dat.AddDays(1).AddMonths(-1);
            ym0t = dat;
            ym1f = ym0t.AddDays(1);
            ym1t = ym1f.AddMonths(1).AddDays(-1);
            ym2f = ym1t.AddDays(1);
            ym2t = ym2f.AddMonths(1).AddDays(-1);
            ym3f = ym2t.AddDays(1);
            ym3t = ym3f.AddMonths(1).AddDays(-1);
            ym4f = ym3t.AddDays(1);
            ym4t = ym4f.AddMonths(1).AddDays(-1);
            ympt = ym0f.AddDays(-1);
            ympf = new DateTime(2000, 1, 1);
        }

        public string ArrivalDueDate1 => $"{YearMonth.AddMonths(1):MM}月期日到来";
        public string ArrivalDueDate2 => $"{YearMonth.AddMonths(2):MM}月期日到来";
        public string ArrivalDueDate3 => $"{YearMonth.AddMonths(3):MM}月期日到来";
        public string ArrivalDueDate4 => $"{YearMonth.AddMonths(4):MM}月以降期日到来";


        #endregion

        #region QueryProcessor 用
        public DateTime ympf { get; set; }
        public DateTime ympt { get; set; }
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

        #endregion

        public string ConnectionId { get; set; }
    }
}
