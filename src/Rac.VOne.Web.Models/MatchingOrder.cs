using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Models
{
    [DataContract]
    public class MatchingOrder : IByCompany
    {
        [DataMember] public int CompanyId { get; set; }
        [DataMember] public int TransactionCategory { get; set; }
        [DataMember] public string ItemName { get; set; }
        [DataMember] public int ExecutionOrder { get; set; }
        [DataMember] public int Available { get; set; }
        [DataMember] public int SortOrder { get; set; }
        [DataMember] public int CreateBy { get; set; }
        [DataMember] public DateTime CreateAt { get; set; }
        [DataMember] public int UpdateBy { get; set; }
        [DataMember] public DateTime UpdateAt { get; set; }
        /// <summary>
        ///  <see cref="SortOrder"/>
        ///   0 : ASC
        ///   1 : DESC
        /// </summary>
        public string SortOrderDirection { get { return SortOrder == 0 ? "ASC" : "DESC"; } }

        public string SortOrderJp { get { return SortOrder == 0 ? "昇順" : "降順"; } }

        public string ItemNameJp
        {
            get
            {
                if (TransactionCategory == 1)
                {
                    switch (ItemName)
                    {
                        case "BillingRemainSign" : return "請求残の正負";
                        case "CashOnDueDatesFlag" : return "期日入金予定フラグ";
                        case "DueAt" : return  "入金予定日";
                        case "CustomerCode" : return "得意先コード";
                        case "BilledAt" : return "請求日";
                        case "BillingRemainAmount" : return "請求残（入金予定額）の絶対値";
                        case "BillingCategory" : return "請求区分";
                    }
                }
                else if (TransactionCategory == 2)
                {
                    switch (ItemName)
                    {
                        case "NettingFlag": return "相殺データ";
                        case "ReceiptRemainSign": return "入金残の正負";
                        case "RecordedAt": return "入金日";
                        case "PayerName": return "振込依頼人名";
                        case "SourceBankName": return "仕向銀行";
                        case "SourceBranchName": return "仕向支店";
                        case "ReceiptRemainAmount": return "入金残の絶対値";
                        case "ReceiptCategory": return "入金区分";
                    }
                }
                return "";
            }
        }

    }

    [DataContract]
    public class MatchingOrderResult : IProcessResult
    {
        [DataMember] public ProcessResult ProcessResult { get; set; }
        [DataMember] public MatchingOrder MatchingOrder { get; set; }
    }

    [DataContract]
    public class MatchingOrdersResult : IProcessResult
    {
        [DataMember] public ProcessResult ProcessResult { get; set; }
        [DataMember] public List<MatchingOrder> MatchingOrders { get; set; }

    }
}
