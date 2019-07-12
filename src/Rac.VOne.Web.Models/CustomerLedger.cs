using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Rac.VOne.Web.Models
{
    [DataContract]
    public class CustomerLedger
    {
        /// <summary>伝票日付</summary>
        [DataMember] public DateTime? RecordedAt { get; set; }
        /// <summary>伝票日付 の 所属年月</summary>
        [DataMember] public DateTime? YearMonth { get; set; }
        /// <summary>入金部門名</summary>
        [DataMember] public string SectionName { get; set; }
        /// <summary>請求部門名</summary>
        [DataMember] public string DepartmentName { get; set; }
        /// <summary>請求書番号</summary>
        [DataMember] public string InvoiceCode { get; set; }
        /// <summary>区分名</summary>
        [DataMember] public string CategoryName { get; set; }
        /// <summary>債権科目名</summary>
        [DataMember] public string AccountTitleName { get; set; }
        /// <summary>通貨コード</summary>
        [DataMember] public string CurrencyCode { get; set; }
        /// <summary>消込記号 請求</summary>
        [DataMember] public string MatchingSymbolBilling { get; set; }
        /// <summary>請求額</summary>
        [DataMember] public decimal? BillingAmount { get; set; }
        /// <summary>伝票合計</summary>
        [DataMember] public decimal? SlipTotal { get; set; }
        /// <summary>入金額</summary>
        [DataMember] public decimal? ReceiptAmount { get; set; }
        /// <summary>消込記号 消込</summary>
        [DataMember] public string MatchingSymbolReceipt { get; set; }
        /// <summary>消込金額</summary>
        [DataMember] public decimal? MatchingAmount { get; set; }
        /// <summary>残高</summary>
        [DataMember] public decimal? RemainAmount { get; set; }
        /// <summary>得意先コード</summary>
        [DataMember] public string CustomerCode { get; set; }
        /// <summary>得意先名</summary>
        [DataMember] public string CustomerName { get; set; }
        /// <summary>親得意先ID</summary>
        [DataMember] public int ParentCustomerId { get; set; }
        /// <summary>親得意先コード</summary>
        [DataMember] public string ParentCustomerCode { get; set; }
        /// <summary>親得意先名</summary>
        [DataMember] public string ParentCustomerName { get; set; }
        /// <summary>行種別 0 : データ行, 1 : 繰越行, 2 : 月次計, 3 : 総合計</summary>
        [DataMember] public int RecordType { get; set; }
        /// <summary>データ種別 1 : 請求データ, 2 : 入金データ, 3 : 消込データ, 0 : 集計行</summary>
        [DataMember] public int DataType { get; set; }
        /// <summary>テータ種別名</summary>
        [DataMember] public string RecordTypeName { get; set; }

        public long? HeaderId1 { get; set; }
        public long? HeaderId2 { get; set; }
        public long? HeaderId3 { get; set; }
        public long? HeaderId4 { get; set; }
        public long? HeaderId5 { get; set; }
        public long? HeaderId6 { get; set; }
        public long? HeaderId7 { get; set; }

        public long BillingInputId { get; set; }

        public List<long> GetKeys()
        {
            var list = new List<long>();
            if (HeaderId1.HasValue) list.Add(HeaderId1.Value);
            if (HeaderId2.HasValue) list.Add(HeaderId2.Value);
            if (HeaderId3.HasValue) list.Add(HeaderId3.Value);
            if (HeaderId4.HasValue) list.Add(HeaderId4.Value);
            if (HeaderId5.HasValue) list.Add(HeaderId5.Value);
            if (HeaderId6.HasValue) list.Add(HeaderId6.Value);
            if (HeaderId7.HasValue) list.Add(HeaderId7.Value);
            return list;
        }

        public void Truncate(decimal unit)
        {
            if (unit <= 1M) return;
            if (BillingAmount.HasValue) BillingAmount = BillingAmount.Value / unit;
            if (SlipTotal.HasValue) SlipTotal = SlipTotal.Value / unit;
            if (ReceiptAmount.HasValue) ReceiptAmount = ReceiptAmount.Value / unit;
            if (MatchingAmount.HasValue) MatchingAmount = MatchingAmount.Value / unit;
            if (RemainAmount.HasValue) RemainAmount = RemainAmount.Value / unit;
        }

        public class RecordTypeAlias
        {
            /// <summary>0：データ行</summary>
            public static int DataRecord { get; } = 0;
            /// <summary>1：繰越行</summary>
            public static int CarryOverRecord { get; } = 1;
            /// <summary>2：月次計</summary>
            public static int MonthlySubtotalRecord { get; } = 2;
            /// <summary>3：総合計</summary>
            public static int TotalRecord { get; } = 3;
        }
    }


    [DataContract]
    public class CustomerLedgersResult : IProcessResult
    {
        [DataMember] public ProcessResult ProcessResult { get; set; }
        [DataMember] public List<CustomerLedger> CustomerLedgers { get; set; }
    }
}
