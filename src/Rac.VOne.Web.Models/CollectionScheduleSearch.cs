using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Rac.VOne.Web.Models
{
    [DataContract]
    public class CollectionScheduleSearch
    {
        /// <summary>会社ID</summary>
        [DataMember] public int CompanyId { get; set; }
        /// <summary>処理年月</summary>
        [DataMember] public DateTime YearMonth { get; set; }
        [DataMember] public string DepartmentCodeFrom { get; set; }
        [DataMember] public string DepartmentCodeTo { get; set; }
        [DataMember] public string StaffCodeFrom { get; set; }
        [DataMember] public string StaffCodeTo { get; set; }
        [DataMember] public string CustomerCodeFrom { get; set; }
        [DataMember] public string CustomerCodeTo { get; set; }

        /// <summary>帳票設定 金額単位</summary>
        [DataMember] public decimal UnitPrice { get; set; }
        /// <summary>帳票設定 担当者集計方法：得意先担当</summary>
        [DataMember] public bool UseMasterStaff { get; set; }
        /// <summary>帳票設定 得意先集計方法：債権代表者</summary>
        [DataMember] public bool DisplayParent { get; set; }
        /// <summary>帳票設定 請求部門ごと表示  </summary>
        [DataMember] public bool DisplayDepartment { get; set; }
        [DataMember] public bool NewPagePerDepartment { get; set; }
        [DataMember] public bool NewPagePerStaff { get; set; }
        /// <summary>帳票出力用データ取得</summary>
        /// <remarks>合計行計算もすべて Web.Common に集約</remarks>
        [DataMember] public bool IsPrint { get; set; }

        public DateTime dtprt { get; set; }
        public DateTime dt0f { get; set; }
        public DateTime dt0t { get; set; }
        public DateTime dt1f { get; set; }
        public DateTime dt1t { get; set; }
        public DateTime dt2f { get; set; }
        public DateTime dt2t { get; set; }
        public DateTime dt3f { get; set; }
        public DateTime dt3t { get; set; }

        /// <summary>処理年月の初期化</summary>
        /// <param name="closingDay"></param>
        /// <param name="yearMonth"></param>
        /// <returns></returns>
        public bool InitializeYearMonth()
        {
            var closingDay = YearMonth.Day;

            dt0t = YearMonth;
            dt0f = YearMonth.AddDays(1).AddMonths(-1);
            dtprt = dt0f.AddMonths(0).AddDays(-1);
            dt1f  = dt0f.AddMonths(1).AddDays(0);
            dt1t  = dt0f.AddMonths(2).AddDays(-1);
            dt2f  = dt0f.AddMonths(2).AddDays(0);
            dt2t  = dt0f.AddMonths(3).AddDays(-1);
            dt3f  = dt0f.AddMonths(3).AddDays(0);
            dt3t  = dt0f.AddMonths(4).AddDays(-1);
            return true;
        }
        public string UncollectedAmountLast => $"{YearMonth.AddMonths(-1):MM}月迄未回収";
        public string UncollectedAmount0    => $"{YearMonth.AddMonths(0):MM}月";
        public string UncollectedAmount1    => $"{YearMonth.AddMonths(1):MM}月";
        public string UncollectedAmount2    => $"{YearMonth.AddMonths(2):MM}月";
        public string UncollectedAmount3    => $"{YearMonth.AddMonths(3):MM}月以降";


        public string ConnectionId { get; set; }
    }
}
