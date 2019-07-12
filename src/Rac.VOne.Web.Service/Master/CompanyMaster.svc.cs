using Rac.VOne.Web.Models;
using Rac.VOne.Web.Common;
using Rac.VOne.Web.Service.Extensions;
using Rac.VOne.Common.Logging;
using NLog;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Service.Master
{
    public class CompanyMaster : ICompanyMaster
    {
        private readonly IAuthorizationProcessor authorizationProcessor;
        private readonly ICompanyProcessor companyProcessor;
        private readonly ICompanyInitializeProcessor companyInitializeProcessor;
        private readonly ICompanyLogoProcessor companyLogoProcessor;

        private readonly ILogger logger;

        public CompanyMaster(
            IAuthorizationProcessor authorizationProcessor,
            ICompanyProcessor companyProcessor,
            ICompanyInitializeProcessor companyInitializeProcessor,
            ICompanyLogoProcessor companyLogoProcessor,
            ILogManager logManager
            )
        {
            this.authorizationProcessor = authorizationProcessor;
            this.companyProcessor = companyProcessor;
            this.companyInitializeProcessor = companyInitializeProcessor;
            this.companyLogoProcessor = companyLogoProcessor;
            logger = logManager.GetLogger(typeof(CompanyMaster));
        }

        public async Task<CountResult> DeleteAsync(string SessionKey, int CompanyId)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = await companyProcessor.DeleteAsync(CompanyId, token);
                return new CountResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    Count = result
                };
            }, logger);
        }

        public async Task<CountResult> DeleteLogoAsync(string SessionKey, int CompanyId)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = await companyLogoProcessor.DeleteByCompanyIdAsync(CompanyId, token);
                return new CountResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    Count = result
                };
            }, logger);
        }

        public async Task<CountResult> DeleteLogosAsync(string SessionKey, IEnumerable<CompanyLogo> CompanyLogos)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = await companyLogoProcessor.DeleteAsync(CompanyLogos, token);
                return new CountResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    Count = result
                };
            }, logger);
        }


        public async Task<CompanyResult> GetAsync(string SessionKey, int CompanyId)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = (await companyProcessor.GetAsync(new CompanySearch { Id = CompanyId, }, token)).FirstOrDefault();

                return new CompanyResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    Company = result
                };
            }, logger);
        }

        public async Task<CompanyResult> GetByCodeAsync(string SessionKey, string CompanyCode)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = (await companyProcessor.GetAsync(new CompanySearch { Code = CompanyCode }, token)).FirstOrDefault();

                return new CompanyResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    Company = result
                };
            }, logger);
        }

        public async Task<CompanyLogoResult> GetLogoAsync(string SessionKey, int CompanyId)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = (await companyLogoProcessor.GetAsync(CompanyId, token)).FirstOrDefault();
                return new CompanyLogoResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    CompanyLogo = result
                };
            }, logger);
        }

        public async Task<CompanyLogosResult> GetLogosAsync(string SessionKey, int CompanyId)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = (await companyLogoProcessor.GetAsync(CompanyId, token)).ToList();
                return new CompanyLogosResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    CompanyLogos = result,
                };
            }, logger);
        }

        public async Task<CompanyResult> SaveAsync(string SessionKey, Company Company)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = await companyProcessor.SaveAsync(Company, token);
                return new CompanyResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    Company = result
                };
            }, logger);
        }

        public async Task<CompanyLogoResult> SaveLogoAsync(string SessionKey, CompanyLogo CompanyLogo)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = (await companyLogoProcessor.SaveAsync(new[] { CompanyLogo }, token)).First();
                return new CompanyLogoResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    CompanyLogo = result
                };
            }, logger);
        }

        public async Task<CompanyLogosResult> SaveLogosAsync(string SessionKey, IEnumerable<CompanyLogo> CompanyLogos)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = (await companyLogoProcessor.SaveAsync(CompanyLogos, token)).ToList();
                return new CompanyLogosResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    CompanyLogos = result
                };
            }, logger);
        }

        public async Task<CompaniesResult> GetItemsAsync(string SessionKey, string Name)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = (await companyProcessor.GetAsync(new CompanySearch { Name = Name }, token)).ToList();
                return new CompaniesResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    Companies = result,
                };
            },logger);
        }

        public async Task<CompanyResult> CreateAsync(string SessionKey, CompanySource CompanySource)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var company = await companyInitializeProcessor.InitializeAsync(CompanySource, token);
                return new CompanyResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    Company = company
                };
            }, logger);
        }
    }
}
