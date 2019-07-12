using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using Rac.VOne.Web.Common;
using Rac.VOne.Web.Models;
using Rac.VOne.Web.Service.Extensions;
using Rac.VOne.Common.Logging;
using NLog;

namespace Rac.VOne.Web.Service.Master
{
    public class SectionWithDepartmentMaster : ISectionWithDepartmentMaster
    {
        private readonly IAuthorizationProcessor authorizationProcessor;
        private readonly ISectionWithDepartmentProcessor sectionWithDepartmentProcessor;
        private readonly ILogger logger;

        public SectionWithDepartmentMaster(
            IAuthorizationProcessor authorizationProcessor,
            ISectionWithDepartmentProcessor sectionWithDepartmentProcessor,
            ILogManager logManager
            )
        {
            this.authorizationProcessor = authorizationProcessor;
            this.sectionWithDepartmentProcessor = sectionWithDepartmentProcessor;
            logger = logManager.GetLogger(typeof(SectionWithDepartmentMaster));
        }

        public async Task<SectionWithDepartmentResult> SaveAsync(string sessionKey, SectionWithDepartment[]AddList, SectionWithDepartment[] DeleteList)
        {
            return await authorizationProcessor.DoAuthorizeAsync(sessionKey, async token =>
            {
                var result = (await sectionWithDepartmentProcessor.SaveAsync(AddList, DeleteList, token)).First();
                return new SectionWithDepartmentResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    SectionDepartment = result,
                };
            }, logger);
        }

        public async Task<SectionWithDepartmentResult> GetByDepartmentAsync(string sessionKey, int CompanyId, int DepartmentId)
        {
            return await authorizationProcessor.DoAuthorizeAsync(sessionKey, async token =>
            {
                var result = (await sectionWithDepartmentProcessor.GetAsync(new SectionWithDepartmentSearch{
                    CompanyId       = CompanyId,
                    DepartmentId    = DepartmentId,
                }, token)).FirstOrDefault();

                return new SectionWithDepartmentResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    SectionDepartment = result,
                };
            }, logger);
        }

        public async Task<SectionWithDepartmentsResult> GetBySectionAsync(string SessionKey, int CompanyId, int SectionId)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = (await sectionWithDepartmentProcessor.GetAsync(new SectionWithDepartmentSearch {
                    CompanyId = CompanyId,
                    SectionId = SectionId,
                }, token)).ToList();

                return new SectionWithDepartmentsResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    SectionDepartments = result,
                };
            }, logger);
        }

        public async Task<SectionWithDepartmentsResult> GetItemsAsync(string SessionKey, int CompanyId, SectionWithDepartmentSearch SectionWithDepartmentSearch)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = (await sectionWithDepartmentProcessor.GetAsync(SectionWithDepartmentSearch, token)).ToList();
                return new SectionWithDepartmentsResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    SectionDepartments = result,
                };
            }, logger);
        }

        public async Task<ImportResult> ImportAsync(string sessionKey,
                SectionWithDepartment[] insertList, SectionWithDepartment[] updateList, SectionWithDepartment[] deleteList)
        {
            return await authorizationProcessor.DoAuthorizeAsync(sessionKey, async token =>
            {
                return await sectionWithDepartmentProcessor.ImportAsync(
                    insertList, updateList, deleteList, token);
            }, logger);
        }

        public async Task<ExistResult> ExistSectionAsync(string SessionKey, int SectionId)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = await sectionWithDepartmentProcessor.ExistSectionAsync(SectionId, token);
                return new ExistResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    Exist = result,
                };
            }, logger);
        }

        public async Task<ExistResult> ExistDepartmentAsync(string SessionKey, int DepartmentId)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = await sectionWithDepartmentProcessor.ExistDepartmentAsync(DepartmentId, token);
                return new ExistResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    Exist = result,
                };
            }, logger);
        }
    }
}
