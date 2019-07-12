using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Models
{
    [DataContract]
    public class CreditAgingList
    {
        /// <summary>データ種別（明細行/合計行）
        /// 0 : 明細
        /// 1 : 担当者計
        /// 2 : 部門計
        /// 3 : 総合計</summary>
        [DataMember] public int RecordType { get; set; }
        /// <summary>得意先ID</summary>
        [DataMember] public int CustomerId { get; set; }
        /// <summary>親得意先ID 債権代表者グループ未所属の場合 null</summary>
        [DataMember] public int? ParentCustomerId { get; set; }
        /// <summary>債権代表者フラグ</summary>
        [DataMember] public int ParentCustomerFlag { get; set; }

        /// <summary>請求部門コード</summary>
        [DataMember] public string DepartmentCode { get; set; }
        /// <summary>請求部門名</summary>
        [DataMember] public string DepartmentName { get; set; }
        /// <summary>担当者コード</summary>
        [DataMember] public string StaffCode { get; set; }
        /// <summary>担当者名</summary>
        [DataMember] public string StaffName { get; set; }
        /// <summary>得意先コード</summary>
        [DataMember] public string CustomerCode { get; set; }
        /// <summary>得意先名</summary>
        [DataMember] public string CustomerName { get; set; }
        /// <summary>親得意先コード</summary>
        [DataMember] public string ParentCustomerCode { get; set; }
        /// <summary>親得意先名</summary>
        [DataMember] public string ParentCustomerName { get; set; }
        /// <summary>回収区分名</summary>
        [DataMember] public string CollectCategory { get; set; }

        /// <summary>当月債権総額</summary>
        [DataMember] public decimal CreditAmount { get; set; }
        /// <summary>当月末未決済残高</summary>
        [DataMember] public decimal UnsettledRemain { get; set; }
        /// <summary>当月請求残高</summary>
        [DataMember] public decimal BillingRemain { get; set; }
        /// <summary>与信限度額</summary>
        [DataMember] public decimal? CreditLimit { get; set; }
        /// <summary>当月与信残高</summary>
        [DataMember] public decimal? CreditBalance { get; set; }
        /// <summary>期日到来</summary>
        [DataMember] public decimal ArrivalDueDate1 { get; set; }
        [DataMember] public decimal ArrivalDueDate2 { get; set; }
        [DataMember] public decimal ArrivalDueDate3 { get; set; }
        [DataMember] public decimal ArrivalDueDate4 { get; set; }

        public string ParentCollectCategoryName { get; set; }

        public int DepartmentId { get; set; }
        public int StaffId { get; set; }
        public decimal BillingAmount { get; set; }
        public decimal ReceiptAmount { get; set; }
        public decimal MatchingAmount { get; set; }
        public decimal ReceivableAmount { get {
                return ReceivableAmount_0
                     + ReceivableAmount_1
                     + ReceivableAmount_2
                     + ReceivableAmount_3
                     + ReceivableAmount_4;
            } }
        public decimal ReceivableAmount_0 { get; set; }
        public decimal ReceivableAmount_1 { get; set; }
        public decimal ReceivableAmount_2 { get; set; }
        public decimal ReceivableAmount_3 { get; set; }
        public decimal ReceivableAmount_4 { get; set; }

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
                    case 0: return CollectCategory;
                    case 1: return "担当計";
                    case 2: return "部門計";
                    case 3: return "総合計";
                    default: return string.Empty;
                }
            }
        }
        public bool IsMinusCreditBalance { get { return CreditBalance < 0M; } }
        /// <summary>当月債権残高 マイナス時の表示</summary>
        public string CreditBalanceMark { get { return IsMinusCreditBalance ? "*" : ""; } }
    }

    [DataContract]
    public class CreditAgingListsResult : IProcessResult
    {
        [DataMember] public ProcessResult ProcessResult { get; set; }
        [DataMember] public List<CreditAgingList> CreditAgingLists { get; set; }
    }
}
