using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Rac.VOne.Web.Models
{
    [DataContract]
    public class CollectionSchedule
    {
        /// <summary>
        /// グリッド表示用 行ID
        /// 罫線描画用
        /// 得意先（債権代表者）、行種別 変更でインクリメント
        /// </summary>
        [DataMember] public int? RowId { get; set; }

        /// <summary>行種別
        /// 0 : データ行
        /// 1 : 担当者計
        /// 2 : 部門計
        /// 3 : 回収区分 合計行
        /// 4 : 総合計
        /// </summary>
        [DataMember] public int RecordType { get; set; }
        public int CustomerId { get; set; }
        [DataMember] public string CustomerInfo { get; set; }
        [DataMember] public string CustomerCode { get; set; }
        [DataMember] public string CustomerName { get; set; }
        [DataMember] public string CollectCategoryCode { get; set; }
        [DataMember] public string CollectCategoryName { get; set; }
        [DataMember] public int? ClosingDay { get; set; }
        [DataMember] public string StaffCode { get; set; }
        [DataMember] public string StaffName { get; set; }
        [DataMember] public string DepartmentCode { get; set; }
        [DataMember] public string DepartmentName { get; set; }
        [DataMember] public decimal? UncollectedAmountLast { get; set; }
        [DataMember] public decimal? UncollectedAmount0 { get; set; }
        [DataMember] public decimal? UncollectedAmount1 { get; set; }
        [DataMember] public decimal? UncollectedAmount2 { get; set; }
        [DataMember] public decimal? UncollectedAmount3 { get; set; }

        public int CustomerCollectCategoryId { get; set; }
        public int? CustomerSightOfBill { get; set; }
        public decimal? UncollectedAmountTotal
        {
            get
            {
                return UncollectedAmountLast
                   + UncollectedAmount0
                   + UncollectedAmount1
                   + UncollectedAmount2
                   + UncollectedAmount3;
            }
        }

        public bool HasAnyValue
        {
            get
            {
                return UncollectedAmountTotal != 0M
                    || UncollectedAmountLast != 0M
                    || UncollectedAmount0 != 0M
                    || UncollectedAmount1 != 0M
                    || UncollectedAmount2 != 0M
                    || UncollectedAmount3 != 0M;
            }
        }

        public string Closing
        {
            get
            {
                if (ClosingDay > 0)
                    return ClosingDay.Equals(99) ? "末" : ClosingDay.ToString();
                else
                    return string.Empty;
            }
        }

        public void Truncate(decimal unit)
        {
            if (UncollectedAmountLast.HasValue) UncollectedAmountLast = UncollectedAmountLast.Value / unit;
            if (UncollectedAmount0.HasValue) UncollectedAmount0 = UncollectedAmount0.Value / unit;
            if (UncollectedAmount1.HasValue) UncollectedAmount1 = UncollectedAmount1.Value / unit;
            if (UncollectedAmount2.HasValue) UncollectedAmount2 = UncollectedAmount2.Value / unit;
            if (UncollectedAmount3.HasValue) UncollectedAmount3 = UncollectedAmount3.Value / unit;
        }
    }

    [DataContract]
    public class CollectionSchedulesResult : IProcessResult
    {
        [DataMember] public ProcessResult ProcessResult { get; set; }
        [DataMember] public List<CollectionSchedule> CollectionSchedules { get; set; }
    }
}
