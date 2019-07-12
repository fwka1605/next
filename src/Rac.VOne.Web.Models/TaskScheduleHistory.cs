using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Rac.VOne.Web.Models
{
    /// <summary>
    /// タイムスケジューラーログ
    /// </summary>
    [DataContract]
    public class TaskScheduleHistory : IByCompany
    {
        [DataMember] public long Id { get; set; }
        [DataMember] public int CompanyId { get; set; }
        [DataMember] public int ImportType { get; set; }
        [DataMember] public int ImportSubType { get; set; }
        [DataMember] public DateTime StartAt { get; set; }
        [DataMember] public DateTime EndAt { get; set; }
        [DataMember] public int Result { get; set; }
        [DataMember] public string Errors { get; set; }
    }

    /// <summary>
    /// タイムスケジューラーログ 検索条件
    /// </summary>
    [DataContract]
    public class TaskScheduleHistorySearch
    {
        [DataMember] public int CompanyId { get; set; }
        [DataMember] public int? ImportType { get; set; }
        [DataMember] public int? ImportSubType { get; set; }
        [DataMember] public int? Result { get; set; }
        [DataMember] public DateTime? StartAt_From { get; set; }
        [DataMember] public DateTime? StartAt_To { get; set; }
        [DataMember] public DateTime? EndAt_From { get; set; }
        [DataMember] public DateTime? EndAt_To { get; set; }
    }

    [DataContract]
    public class TaskScheduleHistoryResult : IProcessResult
    {
        [DataMember]
        public ProcessResult ProcessResult { get; set; }
        [DataMember]
        public List<TaskScheduleHistory> TaskScheduleHistoryList { get; set; }
    }
}
