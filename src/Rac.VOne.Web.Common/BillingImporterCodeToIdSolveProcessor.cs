using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Rac.VOne.Web.Models;
using Rac.VOne.Data.QueryProcessors;

namespace Rac.VOne.Web.Common
{
    public class BillingImporterCodeToIdSolveProcessor : IBillingImporterCodeToIdSolveProcessor
    {
        private readonly IMasterGetByCodesQueryProcessor<Currency> currencyGetByCodesQueryProcessor;
        private readonly IMasterGetByCodesQueryProcessor<Customer> customerGetByCodesQueryProcessor;
        private readonly IMasterGetByCodesQueryProcessor<Department> departmentGetByCodesQueryProcessor;
        private readonly IMasterGetByCodesQueryProcessor<Staff> staffGetByCodesQueryProcessor;
        private readonly IMasterGetByCodesQueryProcessor<AccountTitle> accountTitleGetByCodesQueryProcessor;
        private readonly ICategoryByCodeQueryProcessor categoryByCodeQueryProcessor;
        public BillingImporterCodeToIdSolveProcessor(
            IMasterGetByCodesQueryProcessor<Currency> currencyGetByCodesQueryProcessor,
            IMasterGetByCodesQueryProcessor<Customer> customerGetByCodesQueryProcessor,
            IMasterGetByCodesQueryProcessor<Department> departmentGetByCodesQueryProcessor,
            IMasterGetByCodesQueryProcessor<Staff> staffGetByCodesQueryProcessor,
            IMasterGetByCodesQueryProcessor<AccountTitle> accountTitleGetByCodesQueryProcessor,
            ICategoryByCodeQueryProcessor categoryByCodeQueryProcessor
            )
        {
            this.currencyGetByCodesQueryProcessor = currencyGetByCodesQueryProcessor;
            this.customerGetByCodesQueryProcessor = customerGetByCodesQueryProcessor;
            this.departmentGetByCodesQueryProcessor = departmentGetByCodesQueryProcessor;
            this.staffGetByCodesQueryProcessor = staffGetByCodesQueryProcessor;
            this.accountTitleGetByCodesQueryProcessor = accountTitleGetByCodesQueryProcessor;
            this.categoryByCodeQueryProcessor = categoryByCodeQueryProcessor;
        }
        public async Task SolveAsync(int CompanyId, IEnumerable<BillingImport> billingImport, CancellationToken token)
        {
            var currencyCodes = new HashSet<string>();
            var customerCodes = new HashSet<string>();
            var departmentCodes = new HashSet<string>();
            var staffCodes = new HashSet<string>();
            var accountTitleCods = new HashSet<string>();
            var billingCategoryCodes = new HashSet<string>();
            var collectCategoryCodes = new HashSet<string>();
            foreach (var x in billingImport)
            {
                if (x.IsCurrencyIdEmpty() && !currencyCodes.Contains(x.CurrencyCode)) currencyCodes.Add(x.CurrencyCode);
                if (x.IsCustomerIdEmpty() && !customerCodes.Contains(x.CustomerCode)) customerCodes.Add(x.CustomerCode);
                if (x.IsDepartmentIdEmpty() && !departmentCodes.Contains(x.DepartmentCode)) departmentCodes.Add(x.DepartmentCode);
                if (x.IsStaffIdEmpty() && !staffCodes.Contains(x.StaffCode)) staffCodes.Add(x.StaffCode);
                if (x.IsAccountTitleIdEmpty() && !accountTitleCods.Contains(x.DebitAccountTitleCode)) accountTitleCods.Add(x.DebitAccountTitleCode);
                if (x.IsBillingCategoryIdEmpty() && !billingCategoryCodes.Contains(x.BillingCategoryCode)) billingCategoryCodes.Add(x.BillingCategoryCode);
                if (x.IsCollectCategoryIdEmpty() && !collectCategoryCodes.Contains(x.CollectCategoryCode)) collectCategoryCodes.Add(x.CollectCategoryCode);
            }
            Dictionary<string, int> currencyDic         = null;
            Dictionary<string, int> customerDic         = null;
            Dictionary<string, int> departmentDic       = null;
            Dictionary<string, int> staffDic            = null;
            Dictionary<string, int> accountTitleDic     = null;
            Dictionary<string, int> billingCategoryDic  = null;
            Dictionary<string, int> collectCategoryDic  = null;
            //if (currencyCodes       .Any()) currencyDic         = await GetCurrencyDictionary(CompanyId, currencyCodes, token);
            //if (customerCodes       .Any()) customerDic         = await GetCustomerDictionary(CompanyId, customerCodes, token);
            //if (departmentCodes     .Any()) departmentDic       = await GetDepartmentDictionary(CompanyId, departmentCodes, token);
            //if (staffCodes          .Any()) staffDic            = await GetStaffDictionary(CompanyId, staffCodes, token);
            //if (accountTitleCods    .Any()) accountTitleDic     = await GetAccountTitleDictionary(CompanyId, accountTitleCods, token);
            //if (billingCategoryCodes.Any()) billingCategoryDic  = await GetBillingCategoryDictionary(CompanyId, billingCategoryCodes, token);
            //if (collectCategoryCodes.Any()) collectCategoryDic  = await GetCollectCategoryDictionary(CompanyId, collectCategoryCodes, token);
            var tasks = new List<Task>();
            if (currencyCodes       .Any()) tasks.Add(Task.Run(async () => currencyDic         = await GetCurrencyDictionary(CompanyId, currencyCodes, token)));
            if (customerCodes       .Any()) tasks.Add(Task.Run(async () => customerDic         = await GetCustomerDictionary(CompanyId, customerCodes, token)));
            if (departmentCodes     .Any()) tasks.Add(Task.Run(async () => departmentDic       = await GetDepartmentDictionary(CompanyId, departmentCodes, token)));
            if (staffCodes          .Any()) tasks.Add(Task.Run(async () => staffDic            = await GetStaffDictionary(CompanyId, staffCodes, token)));
            if (accountTitleCods    .Any()) tasks.Add(Task.Run(async () => accountTitleDic     = await GetAccountTitleDictionary(CompanyId, accountTitleCods, token)));
            if (billingCategoryCodes.Any()) tasks.Add(Task.Run(async () => billingCategoryDic  = await GetBillingCategoryDictionary(CompanyId, billingCategoryCodes, token)));
            if (collectCategoryCodes.Any()) tasks.Add(Task.Run(async () => collectCategoryDic  = await GetCollectCategoryDictionary(CompanyId, collectCategoryCodes, token)));

            await Task.WhenAll(tasks);
            foreach (var x in billingImport)
            {
                if (x.IsCurrencyIdEmpty()           && currencyDic          != null) x.CurrencyId = currencyDic[x.CurrencyCode];
                if (x.IsCustomerIdEmpty()           && customerDic          != null && customerDic.ContainsKey(x.CustomerCode)) x.CustomerId = customerDic[x.CustomerCode];
                if (x.IsDepartmentIdEmpty()         && departmentDic        != null) x.DepartmentId = departmentDic[x.DepartmentCode];
                if (x.IsStaffIdEmpty()              && staffDic             != null) x.StaffId = staffDic[x.StaffCode];
                if (x.IsAccountTitleIdEmpty()       && accountTitleDic      != null) x.DebitAccountTitleId = accountTitleDic[x.DebitAccountTitleCode];
                if (x.IsBillingCategoryIdEmpty()    && billingCategoryDic   != null) x.BillingCategoryId = billingCategoryDic[x.BillingCategoryCode];
                if (x.IsCollectCategoryIdEmpty()    && collectCategoryDic   != null) x.CollectCategoryId = collectCategoryDic[x.CollectCategoryCode];
            }
        }
        private async Task<Dictionary<string, int>> GetCurrencyDictionary(int companyId, IEnumerable<string> codes, CancellationToken token)
            => (await currencyGetByCodesQueryProcessor.GetByCodesAsync(companyId, codes, token)).ToDictionary(x => x.Code, x => x.Id);

        private async Task<Dictionary<string, int>> GetCustomerDictionary(int comapnyId, IEnumerable<string> codes, CancellationToken token)
            => (await customerGetByCodesQueryProcessor.GetByCodesAsync(comapnyId, codes, token)).ToDictionary(x => x.Code, x => x.Id);

        private async Task<Dictionary<string, int>> GetDepartmentDictionary(int companyId, IEnumerable<string> codes, CancellationToken token)
            => (await departmentGetByCodesQueryProcessor.GetByCodesAsync(companyId, codes, token)).ToDictionary(x => x.Code, x => x.Id);

        private async Task<Dictionary<string, int>> GetStaffDictionary(int companyId, IEnumerable<string> codes, CancellationToken token)
            => (await staffGetByCodesQueryProcessor.GetByCodesAsync(companyId, codes, token)).ToDictionary(x => x.Code, x => x.Id);

        private async Task<Dictionary<string, int>> GetAccountTitleDictionary(int companyId, IEnumerable<string> codes, CancellationToken token)
            => (await accountTitleGetByCodesQueryProcessor.GetByCodesAsync(companyId, codes, token)).ToDictionary(x => x.Code, x => x.Id);

        private async Task<Dictionary<string, int>> GetBillingCategoryDictionary(int companyId, IEnumerable<string> codes, CancellationToken token)
            => (await categoryByCodeQueryProcessor.GetAsync(new CategorySearch {
                CompanyId       = companyId,
                CategoryType    = Rac.VOne.Common.CategoryType.Billing,
                Codes           = codes.ToArray(),
            }, token)).ToDictionary(x => x.Code, x => x.Id);

        private async Task<Dictionary<string, int>> GetCollectCategoryDictionary(int companyId, IEnumerable<string> codes, CancellationToken token)
            => (await categoryByCodeQueryProcessor.GetAsync(new CategorySearch {
                CompanyId       = companyId,
                CategoryType    = Rac.VOne.Common.CategoryType.Collect,
                Codes           = codes.ToArray(),
            }, token)).ToDictionary(x => x.Code, x => x.Id);

    }
}
