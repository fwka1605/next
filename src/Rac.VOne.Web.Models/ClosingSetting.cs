using System;
using System.Runtime.Serialization;


namespace Rac.VOne.Web.Models
{
    [DataContract] public class ClosingSetting : IByCompany
    {
        [DataMember] public int CompanyId { get; set; }
        [DataMember] public int BaseDate { get; set; }
        [DataMember] public bool AllowReceptJournalPending { get; set; }
        [DataMember] public bool AllowMutchingPending { get; set; }
        [DataMember] public int UpdateBy { get; set; }
        [DataMember] public DateTime UpdateAt { get; set; }

        public static class BaseDateValues
        {
            public const int BilledAt = 0;
            public const int SalesAt = 1;
        }
    }
    [DataContract]
    public class ClosingSettingResult : IProcessResult
    {
        [DataMember] public ProcessResult ProcessResult { get; set; }
        [DataMember] public ClosingSetting ClosingSetting { get; set; }
    }
}
