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
    public class DepartmentReportProcessor : IDepartmentReportProcessor
    {
        private readonly ICompanyQueryProcessor companyQueryProcessor;
        private readonly IDepartmentByCodeQueryProcessor departmentNameByCodeQueryProcessor;

        public DepartmentReportProcessor(
            ICompanyQueryProcessor companyQueryProcessor,
            IDepartmentByCodeQueryProcessor departmentNameByCodeQueryProcessor
            )
        {
            this.companyQueryProcessor = companyQueryProcessor;
            this.departmentNameByCodeQueryProcessor = departmentNameByCodeQueryProcessor;
        }

        public async Task<byte[]> GetAsync(int companyId, CancellationToken token = default(CancellationToken))
        {
            var report = new DepartmentReport();
            return (await report.BuildAsync("請求部門マスター一覧" + DateTime.Today.ToString("yyyyMMdd"),
                companyQueryProcessor.GetAsync(new CompanySearch { Id = companyId }, token),
                departmentNameByCodeQueryProcessor.GetAsync(new DepartmentSearch { CompanyId = companyId, }, token)))?.Convert();
        }
    }
}
