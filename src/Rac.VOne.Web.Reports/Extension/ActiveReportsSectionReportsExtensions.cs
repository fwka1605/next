using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using GrapeCity.ActiveReports.Export.Pdf.Section;
using Rac.VOne.Client.Reports;
using Rac.VOne.Data.QueryProcessors;
using Rac.VOne.Web.Models;

namespace Rac.VOne.Web.Reports
{
    /// <summary>
    /// <see cref="GrapeCity.ActiveReports.SectionReport"/>用の拡張メソッド用
    /// </summary>
    public static class ActiveReportsSectionReportsExtensions
    {
        /// <summary>PDFドキュメント形式のバイト配列へ変換</summary>
        /// <param name="report">すべて値を設定し、
        /// <see cref="GrapeCity.ActiveReports.SectionReport.Run()"/>を実施したドキュメントを連携</param>
        /// <returns></returns>
        public static byte[] Convert(this GrapeCity.ActiveReports.SectionReport report)
        {
            byte[] result = null;
            using (var stream = new MemoryStream())
            {
                new PdfExport().Export(report.Document, stream);
                result = stream.ToArray();
            }
            return result;
        }


        public static async Task<GrapeCity.ActiveReports.SectionReport> BuildAsync<T>(this IMasterSectionReport<T> report,
            string reportName,
            Task<IEnumerable<Company>> companyTask,
            Task<IEnumerable<T>> masterLoadTask
            )
        {
            await Task.WhenAll(companyTask, masterLoadTask);
            var company = companyTask.Result.First();
            var items = masterLoadTask.Result.ToList();

            if (!items.Any()) return null;

            report.Name = reportName;
            report.SetBasicPageSetting(company.Code, company.Name);
            report.SetData(items);
            report.Run();

            return report as GrapeCity.ActiveReports.SectionReport;
        }
    }
}