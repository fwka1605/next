using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Rac.VOne.Client.Reports;
using Rac.VOne.Common;
using Rac.VOne.Data.QueryProcessors;
using Rac.VOne.Web.Common.Reports;
using Rac.VOne.Web.Models;

namespace Rac.VOne.Web.Reports
{
    public class BillingInvoiceReportProcessor : IBillingInvoiceReportProcessor
    {
        public BillingInvoiceReportProcessor()
        {
        }

        public Task<byte[]> GetAsync(int companyId, CancellationToken token = default(CancellationToken))
        {
            //var report = new BillingInvoiceReport();
            throw new NotImplementedException();
        }
    }
}
