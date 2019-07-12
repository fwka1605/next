using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace Rac.VOne.Web.Models
{
    [DataContract]
    public class PaymentAgencyFee
    {
        [DataMember] public int PaymentAgencyId { get; set; }
        [DataMember] public int CurrencyId { get; set; }
        [DataMember] public decimal? Fee { get; set; }
        [DataMember] public int CreateBy { get; set; }
        [DataMember] public DateTime? CreateAt { get; set; }
        [DataMember] public int UpdateBy { get; set; }
        [DataMember] public DateTime UpdateAt { get; set; }
        [DataMember] public decimal? NewFee { get; set; }//for forieng key

        #region other table columns
        [DataMember] public string CompanyCode { get; set; }
        [DataMember] public string PaymentAgencyCode { get; set; }
        [DataMember] public string CurrencyCode { get; set; }
        #endregion
    }

    [DataContract]
    public class PaymentAgencyFeeResult : IProcessResult
    {
        [DataMember] public ProcessResult ProcessResult { get; set; }
        [DataMember] public PaymentAgencyFee PaymentAgencyFee { get; set; }
    }

    [DataContract]
    public class PaymentAgencyFeesResult : IProcessResult
    {
        [DataMember] public ProcessResult ProcessResult { get; set; }
        [DataMember] public List<PaymentAgencyFee> PaymentAgencyFees { get; set; }
    }

    public static class PaymentAgencyFeeExtension
    {
        public static bool IsBankTransferFeeRegistered(this SortedList<int, PaymentAgencyFee[]> list, int paymentAgencyId, int currencyId, decimal fee, bool checkFee)
            => list != null && list.ContainsKey(paymentAgencyId)
            && (!checkFee || list[paymentAgencyId].Any(x => x.CurrencyId == currencyId && x.Fee == fee));
    }
}
