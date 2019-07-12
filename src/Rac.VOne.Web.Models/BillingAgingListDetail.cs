using System;
using System.Collections.Generic;
using System.Runtime.Serialization;


namespace Rac.VOne.Web.Models
{
    [DataContract]
    public class BillingAgingListDetail
    {
       [DataMember]public string CustomerCode { get; set; }
       [DataMember]public string CustomerName { get; set; }
       [DataMember]public string CurrencyCode { get; set; }
       [DataMember]public DateTime BilledAt { get; set; }
       [DataMember]public DateTime DueAt { get; set; }
       [DataMember]public DateTime SalesAt { get; set; }
       [DataMember]public decimal BillingAmount { get; set; }
       [DataMember]public decimal RemainAmount { get; set; }
       [DataMember]public string StaffCode { get; set; }
       [DataMember]public string StaffName { get; set; }
       [DataMember]public string InvoiceCode { get; set; }
       [DataMember]public string Note { get; set; }

    }
    [DataContract]
    public class BillingAgingListDetailResult : IProcessResult
    {
        [DataMember] public ProcessResult ProcessResult { get; set; }
        [DataMember] public BillingAgingListDetail BillingAgingListDetail { get; set; }
    }

    [DataContract]
    public class BillingAgingListDetailsResult : IProcessResult
    {
        [DataMember] public ProcessResult ProcessResult { get; set; }
        [DataMember] public List<BillingAgingListDetail> BillingAgingListDetails { get; set; }
    }
}
