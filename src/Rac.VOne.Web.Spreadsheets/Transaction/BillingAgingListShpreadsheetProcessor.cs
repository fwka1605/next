using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using Rac.VOne.Client.Reports;
using Rac.VOne.Client.Reports.Settings;
using Rac.VOne.Common;
using Rac.VOne.Data.QueryProcessors;
using Rac.VOne.Web.Common;
using Rac.VOne.Web.Common.Spreadsheets;
using Rac.VOne.Web.Models;
using Rac.VOne.Web.Spreadsheets.Processors;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using static Rac.VOne.Client.Reports.Settings.PF0101;

namespace Rac.VOne.Web.Spreadsheets
{
    public class BillingAgingListShpreadsheetProcessor : IBillingAgingListSpreadsheetProcessor
    {
        private readonly ICompanyQueryProcessor companyQueryProcessor;
        private readonly IByCompanyGetEntityQueryProcessor<ApplicationControl> applicationControlGetByCompanyQueryProcessor;
        private readonly IIdenticalEntityGetByIdsQueryProcessor<Currency> currencyGetByIdsQueryProcessor;
        private readonly IReportSettingQueryProcessor reportSettingQueryProcessor;
        private readonly IBillingAgingListProcessor billingAgingListProcessor;
        private const string ReportId = nameof(PF0101);

        public BillingAgingListShpreadsheetProcessor(
            ICompanyQueryProcessor companyQueryProcessor,
            IByCompanyGetEntityQueryProcessor<ApplicationControl> applicationControlGetByCompanyQueryProcessor,
            IIdenticalEntityGetByIdsQueryProcessor<Currency> currencyGetByIdsQueryProcessor,
            IReportSettingQueryProcessor reportSettingQueryProcessor,
            IBillingAgingListProcessor billingAgingListProcessor
            )
        {
            this.companyQueryProcessor = companyQueryProcessor;
            this.applicationControlGetByCompanyQueryProcessor = applicationControlGetByCompanyQueryProcessor;
            this.currencyGetByIdsQueryProcessor = currencyGetByIdsQueryProcessor;
            this.reportSettingQueryProcessor = reportSettingQueryProcessor;
            this.billingAgingListProcessor = billingAgingListProcessor;
        }

        public async Task<byte[]> GetAsync(BillingAgingListSearch option, IProgressNotifier notifier = null, CancellationToken token = default(CancellationToken))
        {
            var companyTask = companyQueryProcessor.GetAsync(new CompanySearch { Id = option.CompanyId, }, token);
            var appConTask = applicationControlGetByCompanyQueryProcessor.GetAsync(option.CompanyId, token);
            var settingTask = reportSettingQueryProcessor.GetAsync(option.CompanyId, ReportId, token);
            var loadTask = billingAgingListProcessor.GetAsync(option, notifier, token);


            var tasks = new List<Task> { companyTask, appConTask, settingTask, loadTask };

            Task<IEnumerable<Currency>> currenciesTask = null;
            if (option.CurrencyId.HasValue)
            {
                currenciesTask = currencyGetByIdsQueryProcessor.GetByIdsAsync(new[] { option.CurrencyId.Value }, token);
                tasks.Add(currenciesTask);
            }
            await Task.WhenAll(tasks);

            var company = companyTask.Result.First();
            var appCon = appConTask.Result;
            var settings = settingTask.Result.ToList();
            var items = loadTask.Result.ToList();
            var precition = currenciesTask?.Result.FirstOrDefault().Precision ?? 0;

            if (!items.Any()) return null;

            var remianType = settings.GetReportSetting<ReportAdvanceReceivedType>(BillingRemainType);
            var displayCode = settings.GetReportSetting<ReportDoOrNot>(DisplayCustomerCode) == ReportDoOrNot.Do;


            var processor = remianType == ReportAdvanceReceivedType.UseMatchingAmount ?
                (IProcessor)new BillingAgingListDocumentProcessor{
                    Company                 = company,
                    Items                   = items,
                    Option                  = option,
                    Precision               = precition,
                    DisplayCustomerCode     = displayCode,
                    UseForeignCurrency      = appCon.UseForeignCurrency == 1,
                } : new BillingAgingListReceiptDocumentProcessor {
                    Company                 = company,
                    Items                   = items,
                    Option                  = option,
                    Precision               = precition,
                    DisplayCustomerCode     = displayCode,
                    UseForeignCurrency      = appCon.UseForeignCurrency == 1,
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
