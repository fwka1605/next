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
    public class StaffReportProcessor : IStaffReportProcessor
    {
        private readonly ICompanyQueryProcessor companyQueryProcessor;
        private readonly IStaffQueryProcessor staffQueryProcessor;

        public StaffReportProcessor(
            ICompanyQueryProcessor companyQueryProcessor,
            IStaffQueryProcessor staffQueryProcessor
            )
        {
            this.companyQueryProcessor = companyQueryProcessor;
            this.staffQueryProcessor = staffQueryProcessor;
        }

        public async Task<byte[]> GetAsync(int companyId, CancellationToken token = default(CancellationToken))
        {
            var report = new SaleRegisterSectionReport();
            return (await report.BuildAsync("営業担当者マスター" + DateTime.Now.ToString("yyyyMMdd"),
                companyQueryProcessor.GetAsync(new CompanySearch { Id = companyId }, token),
                staffQueryProcessor.GetAsync(new StaffSearch { CompanyId = companyId, }, token)))?.Convert();
        }
    }
}
