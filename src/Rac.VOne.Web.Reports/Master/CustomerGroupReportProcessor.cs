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
    public class CustomerGroupReportProcessor : ICustomerGroupReportProcessor
    {
        private readonly ICompanyQueryProcessor companyQueryProcessor;
        private readonly ICustomerGroupByIdQueryProcessor customerGroupByIdQueryProcessor;
        public CustomerGroupReportProcessor(
            ICompanyQueryProcessor companyQueryProcessor,
            ICustomerGroupByIdQueryProcessor customerGroupByIdQueryProcessor
            )
        {
            this.companyQueryProcessor = companyQueryProcessor;
            this.customerGroupByIdQueryProcessor = customerGroupByIdQueryProcessor;
        }

        public async Task<byte[]> GetAsync(int companyId, CancellationToken token = default(CancellationToken))
        {
            var report = new CustomerGroupSectionReport();
            return (await report.BuildAsync("債権代表者マスター" + DateTime.Today.ToString("yyyyMMdd"),
                companyQueryProcessor.GetAsync(new CompanySearch { Id = companyId }, token),
                customerGroupByIdQueryProcessor.GetAsync(new CustomerGroupSearch { CompanyId = companyId, }, token)))?.Convert();
        }
    }
}
