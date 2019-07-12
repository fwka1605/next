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
    public class LoginUserReportProcessor : ILoginUserReportProcessor
    {
        private readonly ICompanyQueryProcessor companyQueryProcessor;
        private readonly ILoginUserQueryProcessor loginUserQueryProcessor;

        public LoginUserReportProcessor(
            ICompanyQueryProcessor companyQueryProcessor,
            ILoginUserQueryProcessor loginUserQueryProcessor
            )
        {
            this.companyQueryProcessor = companyQueryProcessor;
            this.loginUserQueryProcessor = loginUserQueryProcessor;
        }

        public async Task<byte[]> GetAsync(int companyId, CancellationToken token = default(CancellationToken))
        {
            var report = new LoginUserSectionReport();
            return (await report.BuildAsync("ログインユーザーマスター" + DateTime.Today.ToString("yyyyMMdd"),
                companyQueryProcessor.GetAsync(new CompanySearch { Id = companyId }, token),
                loginUserQueryProcessor.GetAsync(new LoginUserSearch { CompanyId = companyId, }, token)))?.Convert();
        }
    }
}
