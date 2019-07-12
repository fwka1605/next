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
    public class SectionReportProcessor : ISectionReportProcessor
    {
        private readonly ICompanyQueryProcessor companyQueryProcessor;
        private readonly ISectionQueryProcessor sectionQueryProcessor;
        public SectionReportProcessor(
            ICompanyQueryProcessor companyQueryProcessor,
            ISectionQueryProcessor sectionQueryProcessor
            )
        {
            this.companyQueryProcessor = companyQueryProcessor;
            this.sectionQueryProcessor = sectionQueryProcessor;
        }

        public async Task<byte[]> GetAsync(int companyId, CancellationToken token = default(CancellationToken))
        {
            var report = new SectionMasterReport();
            return (await report.BuildAsync("入金部門マスター" + DateTime.Today.ToString("yyyyMMdd"),
                companyQueryProcessor.GetAsync(new CompanySearch { Id = companyId }, token),
                sectionQueryProcessor.GetAsync(new SectionSearch { CompanyId = companyId, }, token)))?.Convert();
        }
    }
}
