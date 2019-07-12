using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Rac.VOne.Web.Models
{
    [DataContract]
    public class Closing : IByCompany
    {
        [DataMember] public int CompanyId { get; set; }
        [DataMember] public DateTime ClosingMonth { get; set; }
        [DataMember] public int UpdateBy  { get; set; }
        [DataMember] public DateTime UpdateAt  { get; set; }
    }
    [DataContract]
    public class ClosingHistory
    {
        [DataMember] public bool Selected { get; set; }
        [DataMember] public DateTime ClosingMonth { get; set; }
        [DataMember] public bool IsClosed { get; set; }
        [DataMember] public long BillingCount { get; set; }
        [DataMember] public long ReceiptCount { get; set; }
        public string IsClosedDisplay => IsClosed ? "〇" : string.Empty;
    }
    [DataContract]
    public class ClosingResult : IProcessResult
    {
        [DataMember] public ProcessResult ProcessResult { get; set; }
        [DataMember] public Closing Closing { get; set; }
    }
    [DataContract]
    public class ClosingHistorysResult : IProcessResult
    {
        [DataMember] public ProcessResult ProcessResult { get; set; }
        [DataMember] public List<ClosingHistory> ClosingHistorys { get; set; }
    }
    [DataContract]
    public class ClosingInformation
    {
        [DataMember]
        public bool UseClosing { get; set; }
        [DataMember]
        public Closing Closing { get; set; }
        public string ClosingDisplay
            => Closing?.ClosingMonth.ToString("締済年月:yyyy年MM月") ?? "締済年月:";
    }
    [DataContract]
    public class ClosingInformationResult : IProcessResult
    {
        [DataMember] public ProcessResult ProcessResult { get; set; }
        [DataMember] public ClosingInformation ClosingInformation { get; set; }
    }
}
