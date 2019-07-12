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
    public class SectionWithDepartmentReportProcessor : ISectionWithDepartmentReportProcessor
    {
        private readonly ICompanyQueryProcessor companyQueryProcessor;
        private readonly ISectionWithDepartmentQueryProcessor sectionWithDepartmentQueryProcessor;

        public SectionWithDepartmentReportProcessor(
            ICompanyQueryProcessor companyQueryProcessor,
            ISectionWithDepartmentQueryProcessor sectionWithDepartmentQueryProcessor
            )
        {
            this.companyQueryProcessor = companyQueryProcessor;
            this.sectionWithDepartmentQueryProcessor = sectionWithDepartmentQueryProcessor;
        }

        public async Task<byte[]> GetAsync(int companyId, CancellationToken token = default(CancellationToken))
        {
            var report = new SectionWithDepartmentReport();
            return (await report.BuildAsync("入金・請求部門対応マスター一覧" + DateTime.Now.ToString("yyyyMMdd"),
                companyQueryProcessor.GetAsync(new CompanySearch { Id = companyId }, token),
                sectionWithDepartmentQueryProcessor.GetAsync(new SectionWithDepartmentSearch { CompanyId = companyId, }, token)))?.Convert();
        }
    }
}
