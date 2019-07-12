using System;
using System.Runtime.Serialization;
using Rac.VOne.Common;

namespace Rac.VOne.Web.Models
{
    [DataContract]
    public class ReminderCommonSetting : IByCompany
    {
        [DataMember] public int CompanyId { get; set; }
        [DataMember] public string OwnDepartmentName { get; set; } = string.Empty;
        [DataMember] public string AccountingStaffName { get; set; } = string.Empty;
        [DataMember] public int OutputDetail { get; set; }
        [DataMember] public string OutputDetailItem { get; set; } = string.Empty;
        [DataMember] public int ReminderManagementMode { get; set; }
        [DataMember] public int DepartmentSummaryMode { get; set; }
        [DataMember] public int CalculateBaseDate { get; set; }
        [DataMember] public int IncludeOnTheDay { get; set; }
        [DataMember] public int DisplayArrearsInterest { get; set; }
        [DataMember] public decimal? ArrearsInterestRate { get; set; }
        [DataMember] public int CreateBy { get; set; }
        [DataMember] public DateTime CreateAt { get; set; }
        [DataMember] public int UpdateBy { get; set; }
        [DataMember] public DateTime UpdateAt { get; set; }
        public string BaseDateCaption
        {
            get
            {
                if (CalculateBaseDate == (int)Constants.CalculateBaseDate.DueAt)    return "入金予定日";
                if (CalculateBaseDate == (int)Constants.CalculateBaseDate.BilledAt) return "請求日";
                return "当初予定日";
            }
        }
    }

    [DataContract]
    public class ReminderCommonSettingResult : IProcessResult
    {
        [DataMember] public ProcessResult ProcessResult { get; set; }
        [DataMember] public ReminderCommonSetting ReminderCommonSetting { get; set; }
    }

}
