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
    public class ReceiptSectionTransferReportProcessor : IReceiptSectionTransferReportProcessor
    {
        private readonly ICompanyQueryProcessor companyQueryProcessor;
        private readonly IByCompanyGetEntityQueryProcessor<ApplicationControl> applicationControlGetByCompanyQueryProcessor;
        private readonly IReceiptSectionTransferQueryProcessor receiptSectionTransferQueryProcessor;


        public ReceiptSectionTransferReportProcessor(
            ICompanyQueryProcessor companyQueryProcessor,
            IByCompanyGetEntityQueryProcessor<ApplicationControl> applicationControlGetByCompanyQueryProcessor,
            IReceiptSectionTransferQueryProcessor receiptSectionTransferQueryProcessor
            )
        {
            this.companyQueryProcessor = companyQueryProcessor;
            this.applicationControlGetByCompanyQueryProcessor = applicationControlGetByCompanyQueryProcessor;
            this.receiptSectionTransferQueryProcessor = receiptSectionTransferQueryProcessor;
        }

        public async Task<byte[]> GetAsync(int companyId, CancellationToken token = default(CancellationToken))
        {
            var companyTask = companyQueryProcessor.GetAsync(new CompanySearch { Id = companyId, }, token);
            var appConTask = applicationControlGetByCompanyQueryProcessor.GetAsync(companyId, token);
            var loadTask = receiptSectionTransferQueryProcessor.GetReceiptSectionTransferForPrintAsync(companyId, token);

            await Task.WhenAll(companyTask, appConTask, loadTask);

            var company = companyTask.Result.First();
            var appCon = appConTask.Result;
            var items = loadTask.Result.ToList();

            if (!items.Any()) return null;

            var useForeignCurrency = appCon.UseForeignCurrency == 1;
            var precition = !useForeignCurrency ? 0 : items.Max(x => x.Precision);

            var report = new ReceiptSectionTransferSectionReport();
            report.SetBasicPageSetting(company.Code, company.Name);
            report.Name = "入金部門振替済チェックリスト" + DateTime.Today.ToString("yyyyMMdd");
            report.SetData(items, useForeignCurrency, precition);

            report.Run();

            return report.Convert();
        }
    }
}
