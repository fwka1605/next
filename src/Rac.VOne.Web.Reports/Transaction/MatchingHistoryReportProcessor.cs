using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Rac.VOne.Client.Reports;
using Rac.VOne.Data.QueryProcessors;
using Rac.VOne.Web.Common.Reports;
using Rac.VOne.Web.Models;

namespace Rac.VOne.Web.Reports
{
    public class MatchingHistoryReportProcessor : IMatchingHistoryReportProcessor
    {
        public MatchingHistoryReportProcessor()
        {
        }

        public Task<byte[]> GetAsync(MatchingHistorySearch option, CancellationToken token = default(CancellationToken))
        {
            throw new NotImplementedException();

            //// todo: data の移し替え
            //// todo: 出力済 フラグの更新
            //// todo: UI 系の指示内容を パラメータ化

            //var company = new Company();
            //var report = new MatchingHistorySectionReport();

            //report.SetBasicPageSetting(company.Code, company.Name, option.RequireSubtotal);
            //var reportData = new List<ExportMatchingHistory>();
            ////reportData = PrepareMatchingHistoryData(true);

            ////var timeSort = (rdoMatchingCreatedOrder.Checked && rdoTakeTotal.Checked);
            ////report.SetPageDataSetting(reportData, timeSort, PrecisionLength, ColumnNameCaptionDictionary["ReceiptNote1"]);

            //report.Name = "消込履歴データ一覧" + DateTime.Now.ToString("yyyyMMdd");
            //report.Run();

            //return report.Convert();
        }
    }
}
