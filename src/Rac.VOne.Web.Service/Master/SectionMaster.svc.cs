using Rac.VOne.Common.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Rac.VOne.Web.Models;
using Rac.VOne.Web.Common;
using Rac.VOne.Web.Service.Extensions;
using NLog;

namespace Rac.VOne.Web.Service.Master
{
    public class SectionMaster : ISectionMaster
    {
        private readonly IAuthorizationProcessor authorizationProcessor;
        private readonly ISectionProcessor sectionProcessor;
        private readonly ILogger logger;

        public SectionMaster(IAuthorizationProcessor authorizationProcessor,
            ISectionProcessor sectionProcessor,
            ILogManager logManager)
        {
            this.authorizationProcessor = authorizationProcessor;
            this.sectionProcessor = sectionProcessor;
            logger = logManager.GetLogger(typeof(SectionMaster));
        }

        public async Task<SectionResults> SaveAsync(string SessionKey, Section Section)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = await sectionProcessor.SaveAsync(Section, token);
                return new SectionResults
                {
                    ProcessResult = new ProcessResult { Result = true },
                    Section = result,
                };
            }, logger);
        }
        public async Task<CountResult> DeleteAsync(string SessionKey, int Id)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = await sectionProcessor.DeleteAsync(Id, token);
                return new CountResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    Count = result,
                };
            }, logger);
        }

        public async Task<SectionsResult> GetByLoginUserIdAsync(string SessionKey, int LoginUserId)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
               var result = (await sectionProcessor.GetAsync(new SectionSearch { LoginUserId = LoginUserId }, token)).ToList();
                return new SectionsResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    Sections = result,
                };
            }, logger);
        }

        public async Task<SectionsResult> GetByCodeAsync(string SessionKey, int CompanyId, string[] Code)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = (await sectionProcessor.GetAsync(new SectionSearch { CompanyId = CompanyId, Codes = Code, }, token)).ToList();

                return new SectionsResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    Sections = result,
                };
            }, logger);
        }

        public async Task<SectionsResult> GetItemsAsync(string SessionKey, int CompanyId, SectionSearch SectionSearch)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = (await sectionProcessor.GetAsync(SectionSearch, token)).ToList();
                return new SectionsResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    Sections = result,
                };
            }, logger);
        }

        public async Task<SectionResult> GetByCustomerIdAsync(string SessionKey, int CustomerId)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = (await sectionProcessor.GetAsync(new SectionSearch { CustomerId = CustomerId }, token)).ToArray();
                return new SectionResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    Section = result,
                };
            }, logger);
        }

        public async Task<ImportResultSection> ImportAsync(string SessionKey,
               Section[] InsertList, Section[] UpdateList, Section[] DeleteList)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = (await sectionProcessor.ImportAsync(InsertList, UpdateList, DeleteList, token)) as ImportResultSection;
                return result;
            }, logger);
        }

        public async Task<MasterDatasResult> GetImportItemsForBankAccountAsync(string SessionKey, int CompanyId, string[] Code)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = (await sectionProcessor.GetImportItemsBankAccountAsync(CompanyId, Code, token)).ToList();
                return new MasterDatasResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    MasterDatas = result,
                };

            }, logger);
        }

        public async Task<MasterDatasResult> GetImportItemsForSectionWithDepartmentAsync(string SessionKey, int CompanyId, string[] Code)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = (await sectionProcessor.GetImportItemsSectionWithDepartmentAsync(CompanyId, Code, token)).ToList();
                return new MasterDatasResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    MasterDatas = result,
                };

            }, logger);
        }

        public async Task<MasterDatasResult> GetImportItemsForSectionWithLoginUserAsync(string SessionKey, int CompanyId, string[] Code)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = (await sectionProcessor.GetImportItemsSectionWithLoginUserAsync(CompanyId, Code, token)).ToList();
                return new MasterDatasResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    MasterDatas = result,
                };

            }, logger);
        }

        public async Task<MasterDatasResult> GetImportItemsForReceiptAsync(string SessionKey, int CompanyId, string[] Code)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = (await sectionProcessor.GetImportItemsReceiptAsync(CompanyId, Code, token)).ToList();
                return new MasterDatasResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    MasterDatas = result,
                };

            }, logger);
        }

        public async Task<MasterDatasResult> GetImportItemsForNettingAsync(string SessionKey, int CompanyId, string[] Code)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = (await sectionProcessor.GetImportItemsNettingAsync(CompanyId, Code, token)).ToList();
                return new MasterDatasResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    MasterDatas = result,
                };

            }, logger);
        }

        public async Task<SectionsResult> GetImportItemsForSectionAsync(string SessionKey, int CompanyId, string[] PayerCode)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = new List<Section>();
                if (PayerCode.Length > 0)
                    result = (await sectionProcessor.GetAsync(new SectionSearch { CompanyId = CompanyId, PayerCodes = PayerCode.Where(x => !string.IsNullOrEmpty(x)).Distinct().ToArray() }, token)).ToList();

                return new SectionsResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    Sections = result,
                };
            }, logger);
        }
        public async Task<SectionsResult> GetAsync(string SessionKey, int[] ids)
            => await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = (await sectionProcessor.GetAsync(new SectionSearch { Ids = ids }, token)).ToList();
                return new SectionsResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    Sections = result,
                };
            }, logger);
    }
}
