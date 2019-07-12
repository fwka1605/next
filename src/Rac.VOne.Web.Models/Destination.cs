using System;
using System.Runtime.Serialization;
using System.Collections.Generic;

namespace Rac.VOne.Web.Models
{
    /// <summary>仕向先</summary>
    [DataContract] public class Destination : IMasterData, IMaster, ISynchronization
    {
        [DataMember] public int Id { get; set; }
        [DataMember] public int CompanyId { get; set; }
        [DataMember] public int CustomerId { get; set; }
        [DataMember] public string Name { get; set; }
        [DataMember] public string Code { get; set; }
        [DataMember] public string PostalCode { get; set; } = string.Empty;
        [DataMember] public string Address1 { get; set; } = string.Empty;
        [DataMember] public string Address2 { get; set; } = string.Empty;
        [DataMember] public string Addressee { get; set; } = string.Empty;
        [DataMember] public string DepartmentName { get; set; } = string.Empty;
        [DataMember] public string Honorific { get; set; } = string.Empty;
        [DataMember] public int CreateBy { get; set; }
        [DataMember] public DateTime CreateAt { get; set; }
        [DataMember] public int UpdateBy { get; set; }
        [DataMember] public DateTime UpdateAt { get; set; }

        public override string ToString()
            => string.Join(" ", (string.IsNullOrEmpty(Addressee) ? DepartmentName : Addressee), Honorific);
    }

    [DataContract]
    public class DestinationResult : IProcessResult
    {
        [DataMember] public ProcessResult ProcessResult { get; set; }
        [DataMember] public Destination Destination { get; set; }
    }

    [DataContract]
    public class DestinationsResult : IProcessResult
    {
        [DataMember] public ProcessResult ProcessResult { get; set; }
        [DataMember] public List<Destination> Destinations { get; set; }
    }

}
