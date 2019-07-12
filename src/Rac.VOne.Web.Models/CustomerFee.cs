using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Models
{
    [DataContract]
    public class CustomerFee
    {
        [DataMember] public int CustomerId { get; set; }
        [DataMember] public int CurrencyId { get; set; }
        [DataMember] public decimal? Fee { get; set; }
        [DataMember] public int CreateBy { get; set; }
        [DataMember] public DateTime? CreateAt { get; set; }
        [DataMember] public int UpdateBy { get; set; }
        [DataMember] public DateTime UpdateAt { get; set; }

        //other table fields
        [DataMember] public string CustomerCode { get; set; }
        [DataMember] public string CustomerName { get; set; }
        [DataMember] public string CurrencyCode { get; set; }
        [DataMember] public decimal Fee1 { get; set; }
        [DataMember] public decimal Fee2 { get; set; }
        [DataMember] public decimal Fee3 { get; set; }
        [DataMember] public DateTime? UpdateAt1 { get; set; }
        [DataMember] public DateTime? UpdateAt2 { get; set; }
        [DataMember] public DateTime? UpdateAt3 { get; set; }
        [DataMember] public string CompanyCode { get; set; }
        [DataMember] public int CompanyId { get; set; }
        [DataMember] public decimal? NewFee { get; set; }
        [DataMember] public int CurrencyPrecision { get; set; }
    }

    [DataContract]
    public class CustomerFeesResult : IProcessResult
    {
        [DataMember] public ProcessResult ProcessResult { get; set; }
        [DataMember] public List<CustomerFee> CustomerFees { get; set; }
        [DataMember] public CustomerFee[] CustomerFeePrint { get; set; }
    }

    [DataContract]
    public class CustomerFeeResult : IProcessResult
    {
        [DataMember] public ProcessResult ProcessResult { get; set; }
        [DataMember] public CustomerFee[] CustomerFee { get; set; }
    }

    public static class CustomerFeeExtension
    {
        public static bool IsBankTransferFeeRegistered(this SortedList<int, CustomerFee[]> list, int customerId, int currencyId, decimal fee, bool checkFee)
            => list != null && list.ContainsKey(customerId)
            && (!checkFee || list[customerId].Any(x => x.CurrencyId == currencyId && x.Fee == fee));
    }
}
