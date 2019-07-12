using GrapeCity.ActiveReports.SectionReportModel;
using System.Collections.Generic;
using System.Linq;
using System;
using Rac.VOne.Web.Models;

namespace Rac.VOne.Client.Reports
{
    public static class Util
    {
        public static void Truncate(this TextBox text, decimal unitPrice, int precision)
        {
            var value = 0M;
            if (unitPrice == 0M
                || !decimal.TryParse(text.Text, out value)) return;
            if (precision == 0)
            {
                text.Value = decimal.Truncate(value / unitPrice);
            }
            else
            {
                text.Value = value / unitPrice;
            }
        }

        public static SearchData GetSearchData(this List<ReportSetting> setting, int displayOrder)
        {
            var item = setting.Find(x => x.DisplayOrder == displayOrder);
            if (item == null) return null;
            return new SearchData(item.Caption, item.ItemValue);
        }
    }


}
