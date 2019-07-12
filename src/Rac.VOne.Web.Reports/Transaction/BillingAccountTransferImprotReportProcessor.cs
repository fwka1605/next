using MsgPack.Serialization;
using Rac.VOne.Client.Reports;
using Rac.VOne.Data.QueryProcessors;
using Rac.VOne.Web.Common;
using Rac.VOne.Web.Common.Reports;
using Rac.VOne.Web.Models;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;


namespace Rac.VOne.Web.Reports
{
    public class BillingAccountTransferImprotReportProcessor : IBillingAccountTransferImportReportProcessor
    {
        private readonly IIdenticalEntityGetByIdsQueryProcessor<Company> companyGetByIdsQueryProcessor;
        private readonly IImportDataProcessor importDataProcessor;
        private readonly MessagePackSerializer<AccountTransferSource> serializer;

        public BillingAccountTransferImprotReportProcessor(
            IIdenticalEntityGetByIdsQueryProcessor<Company> companyGetByIdsQueryProcessor,
            IImportDataProcessor importDataProcessor
            )
        {
            this.companyGetByIdsQueryProcessor = companyGetByIdsQueryProcessor;
            this.importDataProcessor = importDataProcessor;

            serializer = MessagePackSerializer.Get<AccountTransferSource>();
        }

        public async Task<byte[]> GetAsync(AccountTransferImportSource source, CancellationToken token = default(CancellationToken))
        {
            var importDataId = source.ImportDataId ?? 0;

            var importDataTask = importDataProcessor.GetAsync(importDataId, token: token);
            var companyTask = companyGetByIdsQueryProcessor.GetByIdsAsync(new[] { source.CompanyId }, token);

            await Task.WhenAll(importDataTask, companyTask);

            var importData = importDataTask.Result;
            var company = companyTask.Result.First();
            var sources = importData.Details.Select(x => serializer.UnpackSingleObject(x.RecordItem)).ToArray();

            var reportSource = sources.Select(x => new AccountTransferImportReport.ReportRow
            {
                Billing                 = x.Billings?.Single(b => b.Id == x.Billings.Min(y => y.Id)),
                TransferResultCode      = x.TransferResultCode,
                TransferAmount          = x.TransferAmount,
                TransferBankName        = x.TransferBankName,
                TransferBranchName      = x.TransferBranchName,
                TransferCustomerCode    = x.TransferCustomerCode,
                TransferAccountName     = x.TransferAccountName,
            }).ToArray();

            if (!(reportSource?.Any() ?? false)) return null;

            var report = new AccountTransferImportReport()
            {
                Name = $"口座振替結果データ一覧_{DateTime.Now:yyyyMMdd_HHmmss}"
            };

            // 帳票生成
            report.SetBasicPageSetting(company.Code, company.Name);
            report.SetData(reportSource);
            report.Run();

            return report.Convert();
        }
    }
}
