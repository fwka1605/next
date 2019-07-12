using System;
using System.Collections.Generic;
using System.Linq;
using Rac.VOne.Web.Common;
using Rac.VOne.Web.Models;
using Rac.VOne.Web.Service.Extensions;
using Rac.VOne.Common.Logging;
using NLog;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Service.Master
{
    public class DepartmentMaster : IDepartmentMaster
    {
        private readonly IAuthorizationProcessor authorizationProcessor;
        private readonly IDepartmentProcessor departmentProcessor;
        private readonly ILogger logger;

        public DepartmentMaster(
            IAuthorizationProcessor authorizationProcessor,
            IDepartmentProcessor departmentProcessor,
            ILogManager logManager
            )
        {
            this.authorizationProcessor = authorizationProcessor;
            this.departmentProcessor = departmentProcessor;
            logger = logManager.GetLogger(typeof(DepartmentMaster));
        }

        public async Task<DepartmentResult> SaveAsync(string SessionKey, Department Department)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = (await departmentProcessor.SaveAsync(new[] { Department }, token)).First();
                return new DepartmentResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    Department = result,
                };
            }, logger);
        }

        public async Task<CountResult> DeleteAsync(string SessionKey, int Id)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = await departmentProcessor.DeleteAsync(Id, token);
                return new CountResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    Count = result,
                };
            }, logger);
        }

        public async Task<DepartmentsResult> GetAsync(string SessionKey, int[] Id)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = (await departmentProcessor.GetAsync(new DepartmentSearch { Ids = Id }, token)).ToList();
                return new DepartmentsResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    Departments = result,
                };
            }, logger);
        }

        public async Task<DepartmentsResult> GetByCodeAsync(string SessionKey, int CompanyId, string[] Code)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = (await departmentProcessor.GetAsync(new DepartmentSearch {
                    CompanyId   = CompanyId,
                    Codes       = Code,
                }, token)).ToList();
                return new DepartmentsResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    Departments = result,
                };
            }, logger);
        }

        public async Task<DepartmentResult> GetByCodeAndStaffAsync(string SessionKey, int CompanyId, string Code)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = (await departmentProcessor.GetAsync(new DepartmentSearch {
                    CompanyId   = CompanyId,
                    Codes       = new[] { Code },
                }, token)).FirstOrDefault();
                return new DepartmentResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    Department = result,
                };
            }, logger);
        }

        public async Task<DepartmentsResult> GetItemsAsync(string SessionKey, int CompanyId)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = (await departmentProcessor.GetAsync(new DepartmentSearch { CompanyId = CompanyId, }, token)).ToList();
                return new DepartmentsResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    Departments = result,
                };
            }, logger);
        }

        public async Task<DepartmentsResult> GetDepartmentAndStaffAsync(string SessionKey, int CompanyId)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = (await departmentProcessor.GetAsync(new DepartmentSearch { CompanyId = CompanyId, }, token)).ToList();
                return new DepartmentsResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    Departments = result,
                };
            }, logger);
        }

        public async Task<ImportResult> ImportAsync(string SessionKey,
                Department[] InsertList, Department[] UpdateList, Department[] DeleteList)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = await departmentProcessor.ImportAsync(InsertList, UpdateList, DeleteList, token);
                return result;
            }, logger);
        }

        public async Task<MasterDatasResult> GetImportItemsStaffAsync(string SessionKey, int CompanyId, string[] Code)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = (await departmentProcessor.GetImportItemsStaffAsync(CompanyId, Code, token)).ToList();
                return new MasterDatasResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    MasterDatas = result,
                };

            }, logger);
        }

        public async  Task<MasterDatasResult> GetImportItemsSectionWithDepartmentAsync(string SessionKey, int CompanyId, string[] Code)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = (await departmentProcessor.GetImportItemsSectionWithDepartmentAsync(CompanyId, Code, token)).ToList();
                return new MasterDatasResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    MasterDatas = result,
                };

            }, logger);
        }

        public async Task<MasterDatasResult> GetImportItemsBillingAsync(string SessionKey, int CompanyId, string[] Code)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = (await departmentProcessor.GetImportItemsBillingAsync(CompanyId, Code, token)).ToList();

                return new MasterDatasResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    MasterDatas = result,
                };

            }, logger);
        }

        public async Task<DepartmentsResult> DepartmentWithSectionAsync(string SessionKey, int CompanyId, int SectionId, int[] GridDepartmentId)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = (await departmentProcessor.GetAsync(new DepartmentSearch {
                    CompanyId       = CompanyId,
                    WithSectionId   = SectionId,
                    SkipIds         = GridDepartmentId,
                }, token)).ToList();

                return new DepartmentsResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    Departments = result
                };
            }, logger);
        }

        public async Task<DepartmentsResult> GetWithoutSectionAsync(string SessionKey, int CompanyId, int SectionId)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = (await departmentProcessor.GetAsync(new DepartmentSearch {
                    CompanyId       = CompanyId,
                    WithSectionId   = SectionId,
                }, token)).ToList();
                return new DepartmentsResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    Departments = result,
                };
            }, logger);
        }

        public async Task<DepartmentsResult> GetByLoginUserIdAsync(string SessionKey, int CompanyId, int LoginUserId)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = (await departmentProcessor.GetAsync(new DepartmentSearch {
                    CompanyId   = CompanyId,
                    LoginUserId = LoginUserId,
                }, token)).ToList();
                return new DepartmentsResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    Departments = result,
                };
            }, logger);
        }
    }
}
