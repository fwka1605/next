using System.ComponentModel;

namespace Rac.VOne.Client.Common
{
    /// <summary>
    /// 日付形式
    /// </summary>
    [DefaultValue(YearMonthDay)]
    public enum DateType
    {
        /// <summary
        /// >0 : 年月日
        /// </summary>
        YearMonthDay = 0,
        /// <summary>
        /// 1 : 年月
        /// </summary>
        YearMonth,
        /// <summary>
        /// 2 : 年
        /// </summary>
        Year,
        /// <summary>
        /// 3 : 月日
        /// </summary>
        MonthDay,
        /// <summary>
        /// 4 : 年月日時
        /// </summary>
        YearMonthDayHour,
        /// <summary>
        /// 5 : 年月日時分
        /// </summary>
        YearMonthDayHourMinute,
        /// <summary>
        /// 6 : 年月日時分
        /// </summary>
        YearMonthDayHourMinuteSecond,
    }
}
