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
    public class StaffMaster : IStaffMaster
    {
        private readonly IAuthorizationProcessor authorizationProcessor;
        private readonly IStaffProcessor staffProcessor;
        private readonly ILogger logger;

        public StaffMaster(
            IAuthorizationProcessor authorizationProcessor,
            IStaffProcessor staffProcessor,
            ILogManager logManager)
        {
            this.authorizationProcessor = authorizationProcessor;
            this.staffProcessor = staffProcessor;
            logger = logManager.GetLogger(typeof(StaffMaster));
        }

        public async Task<StaffResult> SaveAsync(string SessionKey, Staff Staff)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = await staffProcessor.SaveAsync(Staff, token);
                return new StaffResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    Staff = result,
                };

            }, logger);
        }

        public async Task<CountResult> DeleteAsync(string SessionKey, int Id)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = await staffProcessor.DeleteAsync(Id, token);
                return new CountResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    Count = result,
                };
            }, logger);
        }

        public async Task< StaffsResult > GetAsync(string SessionKey, int[] Id)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = (await staffProcessor.GetAsync(new StaffSearch { Ids = Id }, token)).ToList();
                return new StaffsResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    Staffs = result,
                };
            }, logger);
        }

        public async Task<StaffsResult> GetByCodeAsync(string SessionKey, int CompanyId, string[] Code)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = (await staffProcessor.GetAsync(new StaffSearch {
                    CompanyId   = CompanyId,
                    Codes       = Code,
                }, token)).ToList();
                return new StaffsResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    Staffs = result,
                };

            }, logger);
        }

        public async Task<StaffsResult> GetItemsAsync(string SessionKey, StaffSearch option)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = (await staffProcessor.GetAsync(option, token)).ToList();
                return new StaffsResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    Staffs = result,
                };
            }, logger);
        }

        public async Task<ExistResult> ExistDepartmentAsync(string SessionKey, int DepartmentId)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = await staffProcessor.ExistDepartmentAsync(DepartmentId, token);
                return new ExistResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    Exist = result,
                };
            }, logger);
        }

        public async Task<ImportResultStaff> ImportAsync(string SessionKey,
                Staff[] InsertList, Staff[] UpdateList, Staff[] DeleteList)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = (await staffProcessor.ImportAsync(InsertList, UpdateList, DeleteList, token)) as ImportResultStaff;
                return result;
            }, logger);
        }

        //ForImporting
        public async Task<MasterDatasResult> GetImportItemsLoginUserAsync(string SessionKey, int CompanyId, string[] Code)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = (await staffProcessor.GetImportItemsLoginUserAsync(CompanyId, Code, token)).ToList();
                return new MasterDatasResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    MasterDatas = result,
                };

            }, logger);
        }

        public async Task<MasterDatasResult> GetImportItemsCustomerAsync(string SessionKey, int CompanyId, string[] Code)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = (await staffProcessor.GetImportItemsCustomerAsync(CompanyId, Code, token)).ToList();
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
                var result = (await staffProcessor.GetImportItemsBillingAsync(CompanyId, Code, token)).ToList();
                return new MasterDatasResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    MasterDatas = result,
                };
            }, logger);
        }
    }
}
