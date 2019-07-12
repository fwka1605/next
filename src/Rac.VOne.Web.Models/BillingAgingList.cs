using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Rac.VOne.Web.Models
{
    /// <summary>
    /// 請求残高年齢表 データレコード定義
    /// </summary>
    [DataContract]
    public class BillingAgingList
    {

        /// <summary>　データ種別 (明細行/合計行 の別)
        ///  0 : 通常
        ///  1 : 担当者計
        ///  2 : 部門計
        ///  3 : 総合(通貨)計
        /// </summary>
        [DataMember] public int RecordType { get; set; }
        [DataMember] public int CurrencyId { get; set; }
        [DataMember] public int? ParentCustomerId { get; set; }
        [DataMember] public int CustomerId { get; set; }
        [DataMember] public int ParentCustomerFlag { get; set; }

        [DataMember] public string CurrencyCode { get; set; }
        [DataMember] public string CustomerCode { get; set; }
        [DataMember] public string CustomerName { get; set; }
        [DataMember] public string DepartmentCode { get; set; }
        [DataMember] public string DepartmentName { get; set; }
        [DataMember] public string StaffCode { get; set; }
        [DataMember] public string StaffName { get; set; }

        [DataMember] public string ParentCustomerCode { get; set; }
        [DataMember] public string ParentCustomerName { get; set; }

        /// <summary>前月まで 残高</summary>
        [DataMember] public decimal? LastMonthRemain { get; set; }
        /// <summary>当月請求</summary>
        [DataMember] public decimal CurrentMonthSales { get; set; }
        /// <summary>当月入金</summary>
        [DataMember] public decimal? CurrentMonthReceipt { get; set; }
        /// <summary>当月消込</summary>
        [DataMember] public decimal CurrentMonthMatching { get; set; }
        /// <summary>当月残</summary>
        [DataMember] public decimal? CurrentMonthRemain { get; set; }
        /// <summary>前月まで残</summary>
        [DataMember] public decimal? MonthlyRemain0 { get; set; }
        /// <summary>一カ月前残</summary>
        [DataMember] public decimal? MonthlyRemain1 { get; set; }
        /// <summary>二カ月前残</summary>
        [DataMember] public decimal? MonthlyRemain2 { get; set; }
        /// <summary>三カ月前残</summary>
        [DataMember] public decimal? MonthlyRemain3 { get; set; }

        public string Code
        {
            get
            {
                switch (RecordType)
                {
                    case 0: return CustomerCode;
                    case 1: return StaffCode;
                    case 2: return DepartmentCode;
                    default: return string.Empty;
                }
            }
        }
        public string Name
        {
            get
            {
                switch (RecordType)
                {
                    case 0: return CustomerName;
                    case 1: return StaffName;
                    case 2: return DepartmentName;
                    default: return string.Empty;
                }
            }
        }
        public string TotalText
        {
            get
            {
                switch (RecordType)
                {
                    case 1: return "担当計";
                    case 2: return "部門計";
                    case 3: return "総合計";
                    default: return string.Empty;
                }
            }
        }

        public decimal Balance { get; set; }

        public decimal BillingAmountK { get; set; }
        public decimal BillingAmount0 { get; set; }
        public decimal BillingAmount1 { get; set; }
        public decimal BillingAmount2 { get; set; }
        public decimal BillingAmount3 { get; set; }
        public decimal BillingAmount4 { get; set; }
        public decimal ReceiptAmountK { get; set; }
        public decimal ReceiptAmount0 { get; set; }
        public decimal ReceiptAmount1 { get; set; }
        public decimal ReceiptAmount2 { get; set; }
        public decimal ReceiptAmount3 { get; set; }
        public decimal ReceiptAmount4 { get; set; }
        public decimal BillingMatchingAmountK { get; set; }
        public decimal BillingMatchingAmount0 { get; set; }
        public decimal BillingMatchingAmount1 { get; set; }
        public decimal BillingMatchingAmount2 { get; set; }
        public decimal BillingMatchingAmount3 { get; set; }
        public decimal BillingMatchingAmount4 { get; set; }
        public decimal MatchingAmountK { get; set; }
        public decimal MatchingAmount0 { get; set; }
        public decimal MatchingAmount1 { get; set; }
        public decimal MatchingAmount2 { get; set; }
        public decimal MatchingAmount3 { get; set; }
        public decimal MatchingAmount4 { get; set; }

        public int DepartmentId { get; set; }
        public int StaffId { get; set; }

    }

    [DataContract]
    public class BillingAgingListsResult : IProcessResult
    {
        [DataMember]
        public ProcessResult ProcessResult { get; set; }
        [DataMember]
        public List<BillingAgingList> BillingAgingLists { get; set; }
    }
}
