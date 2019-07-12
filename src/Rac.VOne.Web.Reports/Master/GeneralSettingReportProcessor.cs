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
    public class GeneralSettingReportProcessor : IGeneralSettingReportProcessor
    {
        private readonly ICompanyQueryProcessor companyQueryProcessor;
        private readonly IMasterGetByCodesQueryProcessor<GeneralSetting> masterGetByCodesQueryProcessor;

        public GeneralSettingReportProcessor(
            ICompanyQueryProcessor companyQueryProcessor,
            IMasterGetByCodesQueryProcessor<GeneralSetting> masterGetByCodesQueryProcessor
            )
        {
            this.companyQueryProcessor = companyQueryProcessor;
            this.masterGetByCodesQueryProcessor = masterGetByCodesQueryProcessor;
        }

        public async Task<byte[]> GetAsync(int companyId, CancellationToken token = default(CancellationToken))
        {
            var report = new GeneralSettingSectionReport();
            return (await report.BuildAsync("管理マスター" + DateTime.Now.ToString("yyyyMMdd"),
                companyQueryProcessor.GetAsync(new CompanySearch { Id = companyId }, token),
                masterGetByCodesQueryProcessor.GetByCodesAsync(companyId, new string[] { }, token)))?.Convert();

        }
    }
}
