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
    public class JuridicalPersonalityReportProcessor : IJuridicalPersonalityReportProcessor
    {
        private readonly ICompanyQueryProcessor companyQueryProcessor;
        private readonly IJuridicalPersonalityQueryProcessor juridicalPersonalityQueryProcessor;
        public JuridicalPersonalityReportProcessor(
            ICompanyQueryProcessor companyQueryProcessor,
            IJuridicalPersonalityQueryProcessor juridicalPersonalityQueryProcessor
            )
        {
            this.companyQueryProcessor = companyQueryProcessor;
            this.juridicalPersonalityQueryProcessor = juridicalPersonalityQueryProcessor;
        }

        public async Task<byte[]> GetAsync(int companyId, CancellationToken token = default(CancellationToken))
        {
            var report = new JuridicalPersonalityReport();
            return (await report.BuildAsync("法人格マスター" + DateTime.Today.ToString("yyyyMMdd"),
                companyQueryProcessor.GetAsync(new CompanySearch { Id = companyId }, token),
                juridicalPersonalityQueryProcessor.GetAsync(new JuridicalPersonality { CompanyId = companyId }, token)))?.Convert();
        }
    }
}
