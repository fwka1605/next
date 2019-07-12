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
    public class SectionWithLoginUserReportProcessor : ISectionWithLoginUserReportProcessor
    {
        private readonly ICompanyQueryProcessor companyQueryProcessor;
        private readonly ISectionWithLoginUserQueryProcessor sectionWithLoginUserQueryProcessor;

        public SectionWithLoginUserReportProcessor(
            ICompanyQueryProcessor companyQueryProcessor,
            ISectionWithLoginUserQueryProcessor sectionWithLoginUserQueryProcessor
            )
        {
            this.companyQueryProcessor = companyQueryProcessor;
            this.sectionWithLoginUserQueryProcessor = sectionWithLoginUserQueryProcessor;
        }

        public async Task<byte[]> GetAsync(int companyId, CancellationToken token = default(CancellationToken))
        {
            var report = new SectionWithLoginUserReport();
            return (await report.BuildAsync("入金部門・担当者対応マスター一覧" + DateTime.Now.ToString("yyyyMMdd"),
                companyQueryProcessor.GetAsync(new CompanySearch { Id = companyId }, token),
                sectionWithLoginUserQueryProcessor.GetAsync(new SectionWithLoginUserSearch { CompanyId = companyId, }, token)))?.Convert();
        }
    }
}
