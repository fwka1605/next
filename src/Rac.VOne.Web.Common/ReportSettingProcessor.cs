using Rac.VOne.Data;
using Rac.VOne.Data.QueryProcessors;
using Rac.VOne.Web.Models;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Common
{
    public class ReportSettingProcessor : IReportSettingProcessor
    {
        private readonly IReportSettingQueryProcessor reportSettingQueryProcessor;
        private readonly IAddReportSettingQueryProcessor addReportSettingQueryProcessor;
        private readonly IDeleteReportSettingQueryProcessor deleteReportSettingQueryProcessor;
        private readonly ITransactionScopeBuilder transactionScopeBuilder;

        public ReportSettingProcessor(
            IReportSettingQueryProcessor reportSettingQueryProcessor,
            IAddReportSettingQueryProcessor addReportSettingQueryProcessor,
            IDeleteReportSettingQueryProcessor deleteReportSettingQueryProcessor,
            ITransactionScopeBuilder transactionScopeBuilder
            )
        {
            this.reportSettingQueryProcessor = reportSettingQueryProcessor;
            this.addReportSettingQueryProcessor = addReportSettingQueryProcessor;
            this.deleteReportSettingQueryProcessor = deleteReportSettingQueryProcessor;
            this.transactionScopeBuilder = transactionScopeBuilder;
        }

        public async Task<IEnumerable<ReportSetting>> GetAsync(int CompanyId, string ReportId, CancellationToken token = default(CancellationToken))
            => await reportSettingQueryProcessor.GetAsync(CompanyId, ReportId, token);

        public async Task<IEnumerable<ReportSetting>> SaveAsync(IEnumerable<ReportSetting> settings, CancellationToken token = default(CancellationToken))
        {
            using (var scope = transactionScopeBuilder.Create())
            {
                var first = settings.First();
                var companyid = first.CompanyId;
                var reportId = first.ReportId;
                await deleteReportSettingQueryProcessor.DeleteAsync(companyid, reportId, token);

                var result = new List<ReportSetting>();
                foreach (var setting in settings)
                    result.Add(await addReportSettingQueryProcessor.SaveAsync(setting, token));

                scope.Complete();

                return result;
            }
        }
    }
}