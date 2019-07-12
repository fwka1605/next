using Rac.VOne.Common.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Rac.VOne.Web.Models;
using Rac.VOne.Web.Common;
using Rac.VOne.Web.Service.Extensions;
using Rac.VOne.Data;
using NLog;

namespace Rac.VOne.Web.Service.Master
{
    public class CustomerGroupMaster : ICustomerGroupMaster
    {
        private readonly IAuthorizationProcessor authorizationProcessor;
        private readonly ICustomerGroupProcessor customerGroupProcessor;
        private readonly ILogger logger;

        public CustomerGroupMaster(
            IAuthorizationProcessor authorizationProcessor,
            ICustomerGroupProcessor customerGroupProcessor,
            ILogManager logManager
            )
        {
            this.authorizationProcessor = authorizationProcessor;
            this.customerGroupProcessor = customerGroupProcessor;
            logger = logManager.GetLogger(typeof(CustomerGroupMaster));
        }

        public async Task<CustomerGroupsResult> GetByParentAsync(string SessionKey, int ParentCustomerId)
        {

            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = (await customerGroupProcessor.GetAsync(
                    new CustomerGroupSearch { ParentIds = new[] { ParentCustomerId }, }, token)).ToList();
                return new CustomerGroupsResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    CustomerGroups = result,
                };
            }, logger);
        }

        public async Task<CustomerGroupsResult> GetByChildIdAsync(string SessionKey, int[] ChildCustomerId)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token => {
                var result = (await customerGroupProcessor.GetAsync(
                    new CustomerGroupSearch { ChildIds = ChildCustomerId }, token)).ToList();
                return new CustomerGroupsResult {
                    ProcessResult = new ProcessResult { Result = true, },
                    CustomerGroups = result,
                };

            }, logger);
        }

        public async Task<CustomerGroupsResult> GetPrintCustomerDataAsync(string SessionKey, int CompanyId)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = (await customerGroupProcessor.GetAsync(new CustomerGroupSearch { CompanyId = CompanyId, }, token)).ToList();

                return new CustomerGroupsResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    CustomerGroups = result,
                };

            }, logger);
        }

        public async Task<CustomerGroupResult> GetCustomerForCustomerGroupAsync(string SessionKey, int CompanyId, string Code)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = (await customerGroupProcessor.GetAsync(new CustomerGroupSearch {
                    CompanyId   = CompanyId,
                    Code        = Code,
                    RequireSingleCusotmerRelation = true,
                }, token)).FirstOrDefault();
                return new CustomerGroupResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    CustomerGroup = result,
                };
            }, logger);
        }

        //PB0601 for import
        public async Task<CustomerGroupsResult> GetItemsAsync(string SessionKey, int CompanyId)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = (await customerGroupProcessor.GetAsync(new CustomerGroupSearch { CompanyId = CompanyId, }, token)).ToList();
                return new CustomerGroupsResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    CustomerGroups = result,
                };
            }, logger);
        }

        public async Task<ExistResult> HasChildAsync(string SessionKey, int ParentCustomerId)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var hasChild = await customerGroupProcessor.HasChildAsync(ParentCustomerId, token);
                return new ExistResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    Exist = hasChild,
                };
            }, logger);
        }

        public async Task<CountResult> GetUniqueGroupCountAsync(string SessionKey, int[] Ids)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = await customerGroupProcessor.GetUniqueGroupCountAsync(Ids, token);
                return new CountResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    Count = result,
                };
            }, logger);
        }

        public async Task<ExistResult> ExistCustomerAsync(string SessionKey, int CustomerId)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = await customerGroupProcessor.ExistCustomerAsync(CustomerId, token);

                return new ExistResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    Exist = result,
                };
            }, logger);
        }

        public async Task< CustomerGroupResult > SaveAsync(string SessionKey, CustomerGroup[] AddList, CustomerGroup[] DeleteList)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var item = new MasterImportData<CustomerGroup>();
                item.InsertItems = new List<CustomerGroup>(AddList);
                item.DeleteItems = new List<CustomerGroup>(DeleteList);
                var result = (await customerGroupProcessor.SaveAsync(item, token)).FirstOrDefault();
                return new CustomerGroupResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    CustomerGroup = result,
                };
            }, logger);
        }

        public async Task<ImportResultCustomerGroup> ImportAsync(string SessionKey,
              CustomerGroup[] InsertList, CustomerGroup[] UpdateList, CustomerGroup[] DeleteList)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                return (await customerGroupProcessor.ImportAsync(InsertList, UpdateList, DeleteList, token)) as ImportResultCustomerGroup;
            }, logger);
        }

    }
}
