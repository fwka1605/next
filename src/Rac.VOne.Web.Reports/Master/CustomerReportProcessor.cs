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
    public class CustomerReportProcessor : ICustomerReportProcessor
    {
        private readonly ICompanyQueryProcessor companyQueryProcessor;
        private readonly ICustomerQueryProcessor customerQueryProcessor;

        public CustomerReportProcessor(
            ICompanyQueryProcessor companyQueryProcessor,
            ICustomerQueryProcessor customerQueryProcessor
            )
        {
            this.companyQueryProcessor = companyQueryProcessor;
            this.customerQueryProcessor = customerQueryProcessor;
        }

        public async Task<byte[]> GetAsync(CustomerSearch option, CancellationToken token = default(CancellationToken))
        {
            var report = new CustomerSectionReport();
            return (await report.BuildAsync("得意先マスター一覧" + DateTime.Today.ToString("yyyyMMdd"),
                companyQueryProcessor.GetAsync(new CompanySearch { Id = option.CompanyId }, token),
                customerQueryProcessor.GetAsync(option, token)))?.Convert();
        }
    }
}
