using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Rac.VOne.Web.Models;
using Rac.VOne.Data;
using Rac.VOne.Data.QueryProcessors;
using Rac.VOne.Common.Importer.Billing;

namespace Rac.VOne.Web.Common
{
    public class BillingImporterProcessor : IBillingImporterProcessor
    {
        private readonly IBillingImporterCodeToIdSolveProcessor billingImporterCodeToIdSolveProcessor;
        private readonly IBillingProcessor billingProcessor;
        private readonly IBillingSaveProcessor billingSaveProcessor;

        private readonly IImporterSettingDetailQueryProcessor importerSettingDetailQueryProcessor;
        private readonly ICategoryByCodeQueryProcessor categoryByCodeQueryProcessor;
        private readonly IIdenticalEntityGetByIdsQueryProcessor<Category> categoryIdenticalEntityGetByIdsQueryProcessor;
        private readonly ICustomerQueryProcessor customerQueryProcessor;
        private readonly IAddCustomerQueryProcessor addCustomerQueryProcessor;
        private readonly IUpdateCustomerQueryProcessor updateCustomerQueryProcessor;
        private readonly IBillingSearchForImportQueryProcessor billingSearchForImportQueryProcessor;
        private readonly IUpdateBillingQueryProcessor updatebillingQueryProcessor;
        private readonly IBillingDivisionContractQueryProcessor billingDivisionContractQueryProcessor;
        private readonly IUpdateBillingDivisionContractQueryProcessor updateBillingDivisionContractQueryProcessor;
        private readonly IByCompanyGetEntityQueryProcessor<BillingDivisionSetting> billingDivisionSettingQueryProcessor;
        private readonly IAddBillingDivisionContractQueryProcessor addBillingDivisionContractQueryProcessor;
        private readonly ICustomerDiscountQueryProcessor customerDiscountQueryProcessor;
        private readonly IAddBillingDiscountQueryProcessor addBillingDiscountQueryProcessor;

        private readonly ITransactionScopeBuilder transactionScopeBuilder;

        public BillingImporterProcessor(
            IBillingImporterCodeToIdSolveProcessor billingImporterCodeToIdSolveProcessor,
            IBillingProcessor billingProcessor,
            IBillingSaveProcessor billingSaveProcessor,
            IImporterSettingDetailQueryProcessor importerSettingDetailQueryProcessor,
            ICategoryByCodeQueryProcessor categoryByCodeQueryProcessor,
            IIdenticalEntityGetByIdsQueryProcessor<Category> categoryIdenticalEntityGetByIdsQueryProcessor,
            ICustomerQueryProcessor customerQueryProcessor,
            IAddCustomerQueryProcessor addCustomerQueryProcessor,
            IUpdateCustomerQueryProcessor updateCustomerQueryProcessor,
            IBillingSearchForImportQueryProcessor billingSearchForImportQueryProcessor,
            IUpdateBillingQueryProcessor updatebillingQueryProcessor,
            IBillingDivisionContractQueryProcessor billingDivisionContractQueryProcessor,
            IUpdateBillingDivisionContractQueryProcessor updateBillingDivisionContractQueryProcessor,
            IByCompanyGetEntityQueryProcessor<BillingDivisionSetting> billingDivisionSettingQueryProcessor,
            IAddBillingDivisionContractQueryProcessor addBillingDivisionContractQueryProcessor,
            ICustomerDiscountQueryProcessor customerDiscountQueryProcessor,
            IAddBillingDiscountQueryProcessor addBillingDiscountQueryProcessor,
            ITransactionScopeBuilder transactionScopeBuilder
            )
        {
            this.billingImporterCodeToIdSolveProcessor = billingImporterCodeToIdSolveProcessor;
            this.billingProcessor = billingProcessor;
            this.billingSaveProcessor = billingSaveProcessor;
            this.importerSettingDetailQueryProcessor = importerSettingDetailQueryProcessor;
            this.categoryByCodeQueryProcessor = categoryByCodeQueryProcessor;
            this.categoryIdenticalEntityGetByIdsQueryProcessor = categoryIdenticalEntityGetByIdsQueryProcessor;
            this.customerQueryProcessor = customerQueryProcessor;
            this.addCustomerQueryProcessor = addCustomerQueryProcessor;
            this.updateCustomerQueryProcessor = updateCustomerQueryProcessor;
            this.billingSearchForImportQueryProcessor = billingSearchForImportQueryProcessor;
            this.updatebillingQueryProcessor = updatebillingQueryProcessor;
            this.billingDivisionContractQueryProcessor = billingDivisionContractQueryProcessor;
            this.updateBillingDivisionContractQueryProcessor = updateBillingDivisionContractQueryProcessor;
            this.billingDivisionSettingQueryProcessor = billingDivisionSettingQueryProcessor;
            this.addBillingDivisionContractQueryProcessor = addBillingDivisionContractQueryProcessor;
            this.customerDiscountQueryProcessor = customerDiscountQueryProcessor;
            this.addBillingDiscountQueryProcessor = addBillingDiscountQueryProcessor;
            this.transactionScopeBuilder = transactionScopeBuilder;
        }

        public async Task<BillingImportResult> ImportAsync(BillingImportItems items, CancellationToken token)
        {
            var details = (await importerSettingDetailQueryProcessor.GetAsync(new ImporterSetting { Id = items.ImporterSettingId }, token)).ToList();

            var billingImport = items.Items;
            var CompanyId = items.CompanyId;
            var LoginUserId = items.LoginUserId;
            var newCustomers = billingImport.Where(x => x.AutoCreationCustomerFlag == 1)
                .GroupBy(x => x.CustomerCode).ToDictionary(x => x.Key);

            using (var scope = transactionScopeBuilder.Create())
            {
                foreach (var key in newCustomers.Keys)
                {
                    var customer = await PrepareDataForCustomerAsync(newCustomers[key].First(), items.CompanyId, details, LoginUserId, token);
                    var res = await addCustomerQueryProcessor.SaveAsync(customer, token: token);
                    foreach (var x in newCustomers[key])
                        x.CustomerId = res.Id;
                }

                await billingImporterCodeToIdSolveProcessor.SolveAsync(CompanyId, billingImport, token);

                var billingCategories = await GetBillingCategoryDictionary(billingImport, token);

                if (details.Any(x => x.DoOverwrite == 1))
                {
                    var toDeleteIds = new HashSet<long>();
                    foreach (var option in billingImport.Select(x => PrepareDataForOverWrite(x, items.CompanyId, details)).Where(x => x != null))
                    {
                        var ids = (await billingSearchForImportQueryProcessor.GetItemsForImportAsync(option, token)).ToArray();
                        foreach (var id in ids)
                            if (!toDeleteIds.Contains(id)) toDeleteIds.Add(id);
                    }
                    if (toDeleteIds.Count > 0)
                        await updatebillingQueryProcessor.UpdateDeleteAtAsync(toDeleteIds, LoginUserId, token);
                }

                foreach (var importData in billingImport)
                {
                    var customerId = importData.CustomerId;
                    if (importData.AutoCreationCustomerFlag != 1)
                    {
                        /* request no. 419: 最後に更新した値が有効となる */
                        if (!string.IsNullOrEmpty(importData.ExclusiveBranchCode)
                         && !string.IsNullOrEmpty(importData.ExclusiveBankCode)
                         && !string.IsNullOrEmpty(importData.ExclusiveVirtualBranchCode)
                         && !string.IsNullOrEmpty(importData.ExclusiveAccountNumber))
                        {
                            var customer = new Customer {
                                CompanyId               = CompanyId,
                                Code                    = importData.CustomerCode,
                                ExclusiveBankCode       = importData.ExclusiveBankCode,
                                ExclusiveBranchCode     = importData.ExclusiveBranchCode,
                                ExclusiveAccountNumber  = importData.ExclusiveVirtualBranchCode + importData.ExclusiveAccountNumber,
                                UpdateBy = LoginUserId,
                            };
                            await updateCustomerQueryProcessor.UpdateForBilingImportAsync(customer, token);
                        }
                    }

                    var billing = importData.ConvertToBilling(LoginUserId);
                    var billingSaveResult = await billingSaveProcessor.SaveAsync(billing, token);
                    var billingId = billingSaveResult.Id;

                    var category = billingCategories[billing.BillingCategoryId];
                    if (category.UseLongTermAdvanceReceived == 1)
                    {
                        if (importData.RegisterContractInAdvance == 1)
                        {
                            var contract = (await billingDivisionContractQueryProcessor.GetItemsAsync(
                                new BillingDivisionContractSearch {
                                    CompanyId           = billing.CompanyId,
                                    CustomerId          = billing.CustomerId,
                                    ContractNumber      = importData.ContractNumber,
                                }, token)).FirstOrDefault();
                            if (contract == null || contract.BillingId.HasValue) return new BillingImportResult
                            {
                                ProcessResult = new ProcessResult { Result = false },
                            };
                            contract.BillingId = billingId;
                            await updateBillingDivisionContractQueryProcessor.UpdateBillingAsync(contract, token);
                        }
                        else
                        {
                            var contract = await PrepareDataForBillingDivisionContract(importData, billingId, CompanyId, token);
                            if (contract == null) return new BillingImportResult
                            {
                                ProcessResult = new ProcessResult { Result = false },
                            };
                            contract.CreateBy = LoginUserId;
                            contract.UpdateBy = LoginUserId;
                            contract.CompanyId = CompanyId;
                            await addBillingDivisionContractQueryProcessor.SaveAsync(contract, token);
                        }
                    }

                    if (importData.UseDiscount == 1)
                    {
                        var discount = await PrepareDataForBillingDiscount(importData, billingId, CompanyId, customerId, token);
                        if (discount.Count > 0)
                            foreach (var saveDiscount in discount)
                                await addBillingDiscountQueryProcessor.SaveAsync(saveDiscount, token);
                    }
                }

                scope.Complete();
            }

            return new BillingImportResult
            {
                ProcessResult = new ProcessResult { Result = true },
            };
        }

        private async Task<Customer> PrepareDataForCustomerAsync(BillingImport billingImport, int CompanyId, List<ImporterSettingDetail> ImporterSettingDetailList, int LoginUserId, CancellationToken token)
        {
            var customer = new Customer();
            Category collectCategory = null;
            if (!string.IsNullOrEmpty(billingImport.CollectCategoryCode))
            {
                collectCategory = (await categoryByCodeQueryProcessor.GetAsync(new CategorySearch {
                    CompanyId = CompanyId,
                    CategoryType = Rac.VOne.Common.CategoryType.Collect,
                    Codes = new[] { billingImport.CollectCategoryCode },
                }, token)).FirstOrDefault();
            }
            else
            {
                collectCategory = (await categoryByCodeQueryProcessor.GetAsync(
                    new CategorySearch { Ids = new[] { billingImport.CollectCategoryId } }, token)).FirstOrDefault();
            }

            var existCustomer = (await customerQueryProcessor.GetAsync(
                new CustomerSearch { CompanyId = CompanyId, Codes = new[] { billingImport.CustomerCode } }, token)).FirstOrDefault();

            if (existCustomer == null)
            {
                customer = billingImport.ConvertToCustomer(ImporterSettingDetailList, LoginUserId, collectCategory);
            }
            else
            {
                customer = existCustomer;
                var updExBankCode = (ImporterSettingDetailList.FirstOrDefault(c => c.Sequence == (int)Fields.ExclusiveBankCode).ImportDivision == 1) && string.IsNullOrEmpty(billingImport.ExclusiveBankCode ?? "");
                var updExBranchCode = (ImporterSettingDetailList.FirstOrDefault(c => c.Sequence == (int)Fields.ExclusiveBranchCode).ImportDivision == 1) && string.IsNullOrEmpty(billingImport.ExclusiveBranchCode ?? "");
                var updVirBranchCode = (ImporterSettingDetailList.FirstOrDefault(c => c.Sequence == (int)Fields.ExclusiveVirtualBranchCode).ImportDivision == 1) && string.IsNullOrEmpty(billingImport.ExclusiveVirtualBranchCode ?? "");
                var updExAccountNumber = (ImporterSettingDetailList.FirstOrDefault(c => c.Sequence == (int)Fields.ExclusiveAccountNumber).ImportDivision == 1) && string.IsNullOrEmpty(billingImport.ExclusiveAccountNumber ?? "");

                if (updExBankCode)
                    customer.ExclusiveBankCode = billingImport.ExclusiveBankCode;
                if (updExBranchCode)
                    customer.ExclusiveBranchCode = billingImport.ExclusiveBranchCode;
                if (updVirBranchCode || updExAccountNumber)
                {
                    var exclusiveVirtualBranchCode = updVirBranchCode ? billingImport.ExclusiveVirtualBranchCode : customer.ExclusiveAccountNumber.Substring(0, 3);
                    var exclusiveAccountNumber = updExAccountNumber ? billingImport.ExclusiveAccountNumber : customer.ExclusiveAccountNumber.Substring(3, 7);
                    customer.ExclusiveAccountNumber = exclusiveVirtualBranchCode + exclusiveAccountNumber;
                }
                customer.UpdateBy = LoginUserId;
            }
            return customer;
        }

        private async Task<Dictionary<int, Category>> GetBillingCategoryDictionary(IEnumerable<BillingImport> billingImport, CancellationToken token)
            => (await categoryIdenticalEntityGetByIdsQueryProcessor.GetByIdsAsync(billingImport.Select(x => x.BillingCategoryId).Distinct().ToArray(), token))
                .ToDictionary(x => x.Id, x => x);

        private BillingSearch PrepareDataForOverWrite(BillingImport billingImport, int CompanyId,
            List<ImporterSettingDetail> detailList)
        {
            if (!detailList.Any(x => x.DoOverwrite == 1)) return null;
            var option = new BillingSearch();
            option.CompanyId = CompanyId;
            foreach (var detail in detailList.Where(x => x.DoOverwrite == 1))
            {
                if (detail.Sequence == (int)Fields.CustomerCode)
                    option.CustomerId = billingImport.CustomerId;
                if (detail.Sequence == (int)Fields.BilledAt)
                    option.BilledAt = billingImport.BilledAt;
                if (detail.Sequence == (int)Fields.InvoiceCode)
                    option.InvoiceCode = billingImport.InvoiceCode;
                if (detail.Sequence == (int)Fields.Note1)
                    option.Note1 = billingImport.Note1;
                if (detail.Sequence == (int)Fields.Note2)
                    option.Note2 = billingImport.Note2;
                if (detail.Sequence == (int)Fields.Note3)
                    option.Note3 = billingImport.Note3;
                if (detail.Sequence == (int)Fields.Note4)
                    option.Note4 = billingImport.Note4;
                if (detail.Sequence == (int)Fields.Note5)
                    option.Note5 = billingImport.Note5;
                if (detail.Sequence == (int)Fields.Note6)
                    option.Note6 = billingImport.Note6;
                if (detail.Sequence == (int)Fields.Note7)
                    option.Note7 = billingImport.Note7;
                if (detail.Sequence == (int)Fields.Note8)
                    option.Note8 = billingImport.Note8;
                if (detail.Sequence == (int)Fields.CurrencyCode)
                    option.CurrencyCode = billingImport.CurrencyCode;
            }
            return option;
        }

        private async Task<BillingDivisionContract> PrepareDataForBillingDivisionContract(BillingImport billingImport, long billingSaveId, int CompanyId, CancellationToken token)
        {
            var setting = await billingDivisionSettingQueryProcessor.GetAsync(CompanyId, token);
            if (setting == null) return null;

            var newNumber = await billingDivisionContractQueryProcessor.GetNewContractNumberAsync(CompanyId, token);
            var contract = new BillingDivisionContract {
                ContractNumber          = newNumber.ToString(),
                CustomerId              = billingImport.CustomerId,
                FirstDateType           = setting.FirstDateType,
                Monthly                 = setting.Monthly,
                BasisDay                = setting.BasisDay,
                DivisionCount           = setting.DivisionCount,
                RoundingMode            = setting.RoundingMode,
                RemainsApportionment    = setting.RemainsApportionment,
                BillingId               = billingSaveId,
                BillingAmount           = billingImport.BillingAmount,
                Comfirm                 = 0,
            };
            return contract;

        }

        private async Task<List<BillingDiscount>> PrepareDataForBillingDiscount(BillingImport billingImport, long billingId, int CompanyId, int NewCustomerId, CancellationToken token)
        {
            var discounts = new List<BillingDiscount>();
            var customerDiscounts = (await customerDiscountQueryProcessor.GetAsync(billingImport.CustomerId, token)).ToList();
            foreach (var csDiscount in customerDiscounts)
            {
                var discount = new BillingDiscount {
                    BillingId       = billingId,
                    DiscountType    = csDiscount.Sequence,
                    DiscountAmount  = billingImport.BillingAmount * csDiscount.Rate,
                    AssignmentFlag  = 0,
                };
                discounts.Add(discount);
            }
            return discounts;
        }

    }
}
