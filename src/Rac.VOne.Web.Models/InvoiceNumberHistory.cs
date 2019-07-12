using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Rac.VOne.Web.Models
{
    [DataContract]
    public class InvoiceNumberHistory : IIdentical, IByCompany
    {
        [DataMember ] public int Id { get; set; }
        [DataMember] public int CompanyId { get; set; }
        [DataMember] public string NumberingYear { get; set; } = string.Empty;
        [DataMember] public string NumberingMonth { get; set; } = string.Empty;
        [DataMember] public string FixedString { get; set; } = string.Empty;
        [DataMember] public long LastNumber { get; set; }
        [DataMember] public int CreateBy { get; set; }
        [DataMember] public DateTime CreateAt { get; set; }
        [DataMember] public int UpdateBy { get; set; }
        [DataMember] public DateTime UpdateAt { get; set; }
    }

    [DataContract]
    public class InvoiceNumberHistoryResult : IProcessResult
    {
        [DataMember] public ProcessResult ProcessResult { get; set; }
        [DataMember] public InvoiceNumberHistory InvoiceNumberHistory { get; set; }
    }

    [DataContract]
    public class InvoiceNumberHistoriesResult : IProcessResult
    {
        [DataMember] public ProcessResult ProcessResult { get; set; }
        [DataMember] public List<InvoiceNumberHistory> InvoiceNumberHistories { get; set; }
    }
}
