using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using Rac.VOne.Client.Reports.Settings;
using Rac.VOne.Common;
using Rac.VOne.Data.QueryProcessors;
using Rac.VOne.Web.Common.Spreadsheets;
using Rac.VOne.Web.Models;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Rac.VOne.Web.Spreadsheets.Processors;

namespace Rac.VOne.Web.Spreadsheets
{
    /// <summary>
    /// 滞留明細一覧 Spreadsheet 作成
    /// </summary>
    public class ArrearagesListSpreadsheetProcessor : IArrearagesListSpreadsheetProcessor
    {
        private readonly ICompanyQueryProcessor companyQueryProcessor;
        private readonly IColumnNameSettingQueryProcessor columnNameSettingQueryProcessor;
        private readonly IMasterGetByCodeQueryProcessor<Currency> currencyGetByCodeQueryProcessor;
        private readonly IReportSettingQueryProcessor reportSettingQueryProcessor;
        private readonly IArrearagesListQueryProcessor arrearagesListQueryProcessor;
        private const string ReportId = nameof(PF0401);

        public ArrearagesListSpreadsheetProcessor(
            ICompanyQueryProcessor companyQueryProcessor,
            IColumnNameSettingQueryProcessor columnNameSettingQueryProcessor,
            IMasterGetByCodeQueryProcessor<Currency> currencyGetByCodeQueryProcessor,
            IReportSettingQueryProcessor reportSettingQueryProcessor,
            IArrearagesListQueryProcessor arrearagesListQueryProcessor
            )
        {
            this.companyQueryProcessor = companyQueryProcessor;
            this.columnNameSettingQueryProcessor = columnNameSettingQueryProcessor;
            this.currencyGetByCodeQueryProcessor = currencyGetByCodeQueryProcessor;
            this.reportSettingQueryProcessor = reportSettingQueryProcessor;
            this.arrearagesListQueryProcessor = arrearagesListQueryProcessor;
        }

        public async Task<byte[]> GetAsync(ArrearagesListSearch option, CancellationToken token = default(CancellationToken))
        {

            var companyTask = companyQueryProcessor.GetAsync(new CompanySearch { Id = option.CompanyId, }, token);
            var columnTask = columnNameSettingQueryProcessor.GetAsync(new ColumnNameSetting { CompanyId = option.CompanyId, TableName = nameof(Billing), }, token);
            var settingTask = reportSettingQueryProcessor.GetAsync(option.CompanyId, ReportId, token);
            var loadTask = arrearagesListQueryProcessor.GetAsync(option, token);
            var ccyCode = string.IsNullOrEmpty(option.CurrencyCode) ? Constants.DefaultCurrencyCode : option.CurrencyCode;
            var ccyTask = currencyGetByCodeQueryProcessor.GetByCodeAsync(option.CompanyId, ccyCode);

            var tasks = new List<Task>() { companyTask, columnTask, loadTask, ccyTask };
            var requireSetting = !(option.ReportSettings?.Any() ?? false);
            if (requireSetting) tasks.Add(settingTask);

            await Task.WhenAll(tasks);

            var company = companyTask.Result.First();
            var columns = columnTask.Result.ToList();
            var items = loadTask.Result.ToList();
            var precision = ccyTask.Result.Precision;

            if (!items.Any()) return null;

            if (requireSetting) option.ReportSettings = settingTask.Result.ToList();

            var processor = option.CustomerSummaryFlag ?
                (IProcessor)new ArrearagesListSummaryDocumentProcessor {
                Company     = company,
                Items       = items,
                Precision   = precision,
            } : new ArrearagesListDocumentProcessor {
                Company     = company,
                Items       = items,
                Precision   = precision,
                Note1       = columns.FirstOrDefault(x => x.ColumnName == nameof(ArrearagesList.Note1))?.DisplayColumnName ?? "備考1",
            };

            using (var stream = new MemoryStream())
            {
                using (var document = SpreadsheetDocument.Create(stream, SpreadsheetDocumentType.Workbook))
                {
                    processor.Process(document);
                }
                return stream.ToArray();
            }
        }
    }
}
