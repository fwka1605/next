using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Rac.VOne.Data;
using Rac.VOne.Data.QueryProcessors;
using Rac.VOne.Web.Models;

namespace Rac.VOne.Web.Common
{
    public class CustomerProcessor : ICustomerProcessor
    {
        private readonly IAddCustomerQueryProcessor addCustomerQueryProcessor;
        private readonly IUpdateCustomerQueryProcessor updateCustomerQueryProcessor;
        private readonly IDeleteIdenticalEntityQueryProcessor<Customer> deleteCustomerQueryProcessor;

        private readonly IMasterGetItemsQueryProcessor<Customer> customerGetItemsQueryProcessor;
        private readonly ICustomerQueryProcessor customerQueryProcessor;
        private readonly ICustomerDiscountQueryProcessor customerDiscountQueryProcessor;

        private readonly ICustomerMinQueryProcessor customerMinQueryProcessor;
        private readonly ICustomerExistsQueryProcessor customerExistsQueryProcessor;
        private readonly ICustomerImportQueryProcessor customerImportQueryProcessor;

        private readonly IAddCustomerPaymentContractQueryProcessor addCustomerPaymentContractQueryProcessor;

        private readonly ITransactionScopeBuilder transactionScopeBuilder;

        public CustomerProcessor(
            IAddCustomerQueryProcessor addCustomerQueryProcessor,
            IUpdateCustomerQueryProcessor updateCustomerQueryProcessor,
            IDeleteIdenticalEntityQueryProcessor<Customer> deleteCustomerQueryProcessor,
            ICustomerQueryProcessor customerQueryProcessor,
            ICustomerDiscountQueryProcessor customerDiscountQueryProcessor,
            IMasterGetItemsQueryProcessor<Customer> customerGetItemsQueryProcessor,
            ICustomerMinQueryProcessor customerMinQueryProcessor,
            ICustomerExistsQueryProcessor customerExistsQueryProcessor,
            ICustomerImportQueryProcessor customerImportQueryProcessor,
            IAddCustomerPaymentContractQueryProcessor addCustomerPaymentContractQueryProcessor,
            ITransactionScopeBuilder transactionScopeBuilder
            )
        {
            this.addCustomerQueryProcessor = addCustomerQueryProcessor;
            this.updateCustomerQueryProcessor = updateCustomerQueryProcessor;
            this.deleteCustomerQueryProcessor = deleteCustomerQueryProcessor;
            this.customerQueryProcessor = customerQueryProcessor;
            this.customerDiscountQueryProcessor = customerDiscountQueryProcessor;
            this.customerGetItemsQueryProcessor = customerGetItemsQueryProcessor;
            this.customerMinQueryProcessor = customerMinQueryProcessor;
            this.customerExistsQueryProcessor = customerExistsQueryProcessor;
            this.customerImportQueryProcessor = customerImportQueryProcessor;
            this.addCustomerPaymentContractQueryProcessor = addCustomerPaymentContractQueryProcessor;
            this.transactionScopeBuilder = transactionScopeBuilder;
        }

        public async Task<IEnumerable<Customer>> GetAsync(CustomerSearch option, CancellationToken token = default(CancellationToken))
            => await customerQueryProcessor.GetAsync(option, token);

        //public Customer Get(int CompanyId, string Code)
        //    => customerByCodeQueryProcessor.GetByCode(CompanyId, Code);

        //public IEnumerable<Customer> GetByCode(int CompanyId, IEnumerable<string> Code)
        //    => customerQueryProcessor.GetByCode(CompanyId, Code);

        //public IEnumerable<Customer> GetParentCustomer(int CompanyId)
        //    => customerQueryProcessor.GetParentCustomer(CompanyId);

        //public IEnumerable<Customer> GetCustomerDetails(int CompanyId, string Code, string Name, int ShareTransferFee,
        //    int ClosingDay)
        //    => customerQueryProcessor.GetCustomerDetails(CompanyId, Code, Name, ShareTransferFee, ClosingDay);

        //public IEnumerable<Customer> Get(IEnumerable<int> Id)
        //    => identicalByIdQueryProcessor.GetByIds(Id);

        //public IEnumerable<Customer> GetItems(int CompanyId, CustomerSearch CustomerSearch)
        //    => customerQueryProcessor.GetItems(CompanyId, CustomerSearch);

        //public IEnumerable<Customer> GetCustomerInformationsByParentId(int CompanyId, int ParentId)
        //    => customerQueryProcessor.GetCustomerInformationsByParentId(CompanyId, ParentId);

        //public IEnumerable<Customer> GetCustomerGroup(int CompanyId, int ParentId)
        //    => customerQueryProcessor.CustomerGroup(CompanyId, ParentId);

        //public IEnumerable<Customer> GetCustomerWithList(int CompanyId, IEnumerable<int> CustomerId)
        //    => customerQueryProcessor.CustomerWithList(CompanyId, CustomerId);

        //public Customer GetTopCustomer(Customer Customer)
        //    => customerQueryProcessor.GetTopCustomer(Customer);


        public async Task<bool> ExistCompanyAsync(int CompanyId, CancellationToken token = default(CancellationToken))
            => await customerExistsQueryProcessor.ExistCompanyAsync(CompanyId, token);

        public async Task<bool> ExistCategoryAsync(int CollectCategoryId, CancellationToken token = default(CancellationToken))
            => await customerExistsQueryProcessor.ExistCategoryAsync(CollectCategoryId, token);

        public async Task<bool> ExistStaffAsync(int StaffId, CancellationToken token = default(CancellationToken))
            => await customerExistsQueryProcessor.ExistStaffAsync(StaffId, token);

        public async Task<Customer> SaveAsync(Customer Customer, bool requireIsParentUpdate = false, CancellationToken token = default(CancellationToken))
            => await addCustomerQueryProcessor.SaveAsync(Customer, requireIsParentUpdate, token);

        public async Task<IEnumerable<Customer>> SaveItemsAsync(IEnumerable<Customer> customers, CancellationToken token = default(CancellationToken))
        {
            var result = new List<Customer>();
            using (var scope = transactionScopeBuilder.Create())
            {
                foreach (var customer in customers)
                    result.Add(await addCustomerQueryProcessor.SaveAsync(customer, token: token));
                scope.Complete();
            }
            return result;
        }

        //public int Add(Customer customer)
        //    => addCustomerQueryProcessor.Add(customer);

        //public int Update(Customer customer)
        //    => updateCustomerQueryProcessor.Update(customer);

        //public int UpdateIsParentAsync(int isParent, int loginUserId, IEnumerable<int> ids)
        //    => updateCustomerQueryProcessor.UpdateIsParent(isParent, loginUserId, ids);


        public async Task<int> DeleteAsync(int Id, CancellationToken token = default(CancellationToken))
            => await deleteCustomerQueryProcessor.DeleteAsync(Id, token);

        //public async Task<IEnumerable<CustomerDiscount>> GetDiscountItemsAsync(CustomerSearch option, CancellationToken token = default(CancellationToken))
        //    => await customerDiscountQueryProcessor.GetItemsAsync(option, token);


        public async Task<IEnumerable<MasterData>> GetImportForCustomerGroupParentAsync(int CompanyId, IEnumerable<string> Code, CancellationToken token = default(CancellationToken))
            => await customerImportQueryProcessor.GetImportItemsParentAsync(CompanyId, Code, token);

        public async Task<IEnumerable<MasterData>> GetImportForCustomerGroupChildAsync(int CompanyId, IEnumerable<string> Code, CancellationToken token = default(CancellationToken))
            => await customerImportQueryProcessor.GetImportItemsChildAsync(CompanyId, Code, token);

        public async Task<IEnumerable<MasterData>> GetImportForKanaHistoryAsync(int CompanyId, IEnumerable<string> Code, CancellationToken token = default(CancellationToken))
            => await customerImportQueryProcessor.GetImportItemsKanaHistoryAsync(CompanyId, Code, token);

        public async Task<IEnumerable<MasterData>> GetImportForBillingAsync(int CompanyId, IEnumerable<string> Code, CancellationToken token = default(CancellationToken))
            => await customerImportQueryProcessor.GetImportItemsBillingAsync(CompanyId, Code, token);

        public async Task<IEnumerable<MasterData>> GetImportForReceiptAsync(int CompanyId, IEnumerable<string> Code, CancellationToken token = default(CancellationToken))
            => await customerImportQueryProcessor.GetImportItemsBillingAsync(CompanyId, Code, token);

        public async Task<IEnumerable<MasterData>> GetImportForNettingAsync(int CompanyId, IEnumerable<string> Code, CancellationToken token = default(CancellationToken))
            => await customerImportQueryProcessor.GetImportItemsBillingAsync(CompanyId, Code, token);


        public async Task<IEnumerable<CustomerMin>> GetMinItemsAsync(int CompanyId, CancellationToken token = default(CancellationToken))
            => await customerMinQueryProcessor.GetAsync(CompanyId, token);

        public async Task<ImportResult> ImportAsync(
            IEnumerable<Customer> insert,
            IEnumerable<Customer> update,
            IEnumerable<Customer> delete, CancellationToken token = default(CancellationToken))
        {
            using (var scope = transactionScopeBuilder.Create())
            {
                int deleteCount = 0;
                int updateCount = 0;
                int insertCount = 0;

                foreach (var x in delete)
                {
                    await deleteCustomerQueryProcessor.DeleteAsync(x.Id, token);
                    deleteCount++;
                }

                foreach (var x in update)
                {
                    await addCustomerQueryProcessor.SaveAsync(x, token: token);
                    if (x.CollectCategoryCode == "00")
                    {
                        var contract = x.GetContract();
                        await addCustomerPaymentContractQueryProcessor.SaveAsync(contract, token);
                    }
                    updateCount++;
                }

                foreach (var x in insert)
                {
                    var customer = await addCustomerQueryProcessor.SaveAsync(x, token: token);
                    x.Id = customer.Id;
                    if (x.CollectCategoryCode == "00")
                    {
                        var contract = x.GetContract();
                        await addCustomerPaymentContractQueryProcessor.SaveAsync(contract, token);
                    }
                    insertCount++;
                }

                scope.Complete();
                return new ImportResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    InsertCount = insertCount,
                    UpdateCount = updateCount,
                    DeleteCount = deleteCount,
                };
            }
        }
    }
}
