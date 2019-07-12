using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Rac.VOne.Web.Models
{
    [DataContract]
    public class ReminderSummaryHistory : ITransactional
    {
        [DataMember] public long Id { get; set; }
        [DataMember] public int ReminderSummaryId { get; set; }
        [DataMember] public int CompanyId { get; set; }
        [DataMember] public string Memo { get; set; } = string.Empty;
        [DataMember] public int InputType { get; set; }
        public string InputTypeName
        {
            get
            {
                if (InputType == (int)ReminderSummaryHistoryInputType.StatusChange) return "入力";
                if (InputType == (int)ReminderSummaryHistoryInputType.Output)       return "督促状発行";
                return "";
            }
        }
        [DataMember] public decimal ReminderAmount { get; set; }
        [DataMember] public DateTime CreateAt { get; set; }
        [DataMember] public int CreateBy { get; set; }
        [DataMember] public string CreateByName { get; set; }

        [DataMember] public bool IsUpdateSummaryMemo { get; set; }

        public enum ReminderSummaryHistoryInputType
        {
            /// <summary>入力</summary>
            StatusChange = 1,
            /// <summary>督促状発行</summary>
            Output = 2,
        }

    }

    public class ReminderSummaryHistoryResult : IProcessResult
    {
        [DataMember] public ProcessResult ProcessResult { get; set; }

        [DataMember] public ReminderSummaryHistory ReminderSummaryHistory { get; set; }
    }

    public class ReminderSummaryHistoriesResult : IProcessResult
    {
        [DataMember] public ProcessResult ProcessResult { get; set; }
        [DataMember] public List<ReminderSummaryHistory> ReminderSummaryHistories { get; set; }
    }
}
