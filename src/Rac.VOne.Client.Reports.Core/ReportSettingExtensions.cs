using System;
using System.Collections.Generic;
using System.Text;
using Rac.VOne.Web.Models;

namespace Rac.VOne.Client.Reports
{
    public static class ReportSettingExtensions
    {
        public static TEnum GetReportSetting<TEnum>(this List<ReportSetting> setting, int displayOrder) where TEnum : struct
        {
            var result = default(TEnum);
            var value = setting.Find(x => x.DisplayOrder == displayOrder).ItemKey;
            Enum.TryParse(value, true, out result);
            return result;
        }

    }
}
