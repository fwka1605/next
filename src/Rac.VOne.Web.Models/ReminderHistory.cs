using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Models
{
    [DataContract]
    public class ReminderHistory : ITransactional
    {
        [DataMember] public long Id { get; set; }
        [DataMember] public int ReminderId { get; set; }
        [DataMember] public int CompanyId { get; set; }
        [DataMember] public int StatusId { get; set; }
        [DataMember] public string StatusCode { get; set; } = string.Empty;
        [DataMember] public string StatusName { get; set; } = string.Empty;
        public string StatusCodeAndName
            => StatusCode + ":" + StatusName;
        [DataMember] public string Memo { get; set; }
        [DataMember] public int OutputFlag { get; set; }
        public string OutputFlagName
            => (OutputFlag == 0 ? "未発行" : "発行済");
        [DataMember] public int InputType { get; set; }
        public string InputTypeName
        {
            get
            {
                if (InputType == (int)ReminderHistoryInputType.Create)             return "督促管理開始";
                if (InputType == (int)ReminderHistoryInputType.StatusChange)       return "入力";
                if (InputType == (int)ReminderHistoryInputType.Output)             return "督促状発行";
                if (InputType == (int)ReminderHistoryInputType.ReOutput)           return "督促状再発行";
                if (InputType == (int)ReminderHistoryInputType.MatchingSequential) return "一括消込";
                if (InputType == (int)ReminderHistoryInputType.MatchingIndividual) return "個別消込";
                if (InputType == (int)ReminderHistoryInputType.CancelMatching)     return "消込解除";
                return "";
            }
        }
        [DataMember] public DateTime CreateAt { get; set; }
        [DataMember] public int CreateBy { get; set; }
        [DataMember] public string CreateByName { get; set; }
        [DataMember] public string CustomerCode { get; set; }
        [DataMember] public string CustomerName { get; set; }
        [DataMember] public DateTime? CalculateBaseDate { get; set; }
        [DataMember] public int? BillingCount { get; set; }
        [DataMember] public string CurrencyCode { get; set; }
        [DataMember] public decimal? BillingAmount { get; set; }
        [DataMember] public decimal? ReminderAmount { get; set; }
        [DataMember] public bool IsUpdateStatusMemo { get; set; }
        [DataMember] public int ArrearsDays { get; set; }
        public bool IsTheTop { get; set; }
        public string CustomerCodeDisplay
            => IsTheTop ? CustomerCode : string.Empty;
        public string CustomerNameDisplay
            => IsTheTop ? CustomerName : string.Empty;
        public DateTime? CalculateBaseDateDisplay
            => IsTheTop ? CalculateBaseDate : null;
        public int? ArrearsDaysDisplay
        {
            get
            {
                if (IsTheTop)
                {
                   return ArrearsDays;
                }
                else
                {
                    return null;
                }
            }
        }
        public  string MemoRemoveLine
            => Memo.Replace("\r", "").Replace("\n", "").Replace("\t","");
        public enum ReminderHistoryInputType
        {
            /// <summary>督促管理開始</summary>
            Create = 0,
            /// <summary>入力</summary>
            StatusChange = 1,
            /// <summary>督促状発行</summary>
            Output = 2,
            /// <summary>督促状再発行</summary>
            ReOutput = 3,
            /// <summary>一括消込</summary>
            MatchingSequential = 4,
            /// <summary>個別消込</summary>
            MatchingIndividual = 5,
            /// <summary>消込解除</summary>
            CancelMatching = 6,
        }
    }

    public class ReminderHistoryResult : IProcessResult
    {
        [DataMember] public ProcessResult ProcessResult { get; set; }
        [DataMember] public ReminderHistory ReminderHistory { get; set; }
    }

    public class ReminderHistoriesResult : IProcessResult
    {
        [DataMember] public ProcessResult ProcessResult { get; set; }
        [DataMember] public List<ReminderHistory> ReminderHistories { get; set; }
}
}
