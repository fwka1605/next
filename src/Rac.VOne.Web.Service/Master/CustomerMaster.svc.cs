using System;
using System.Collections.Generic;
using System.Linq;
using Rac.VOne.Data;
using Rac.VOne.Web.Common;
using Rac.VOne.Web.Models;
using Rac.VOne.Web.Service.Extensions;
using Rac.VOne.Common.Logging;
using NLog;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Service.Master
{
    public class CustomerMaster : ICustomerMaster
    {
        private readonly IAuthorizationProcessor authorizationProcessor;
        private readonly ICustomerProcessor customerProcessor;
        private readonly ICustomerPaymentContractProcessor customerPaymentContractProcessor;
        private readonly ICustomerDiscountProcessor customerDiscountProcessor;
        private readonly ILogger logger;

        public CustomerMaster(IAuthorizationProcessor authorizationProcessor,
            ICustomerProcessor customerProcessor,
            ICustomerPaymentContractProcessor customerPaymentContractProcessor,
            ICustomerDiscountProcessor customerDiscountProcessor,
            ILogManager logManager)
        {
            this.authorizationProcessor = authorizationProcessor;
            this.customerProcessor = customerProcessor;
            this.customerPaymentContractProcessor = customerPaymentContractProcessor;
            this.customerDiscountProcessor = customerDiscountProcessor;
            logger = logManager.GetLogger(typeof(CustomerMaster));
        }

        public async Task<CustomerResult> SaveAsync(string SessionKey, Customer Customer)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var customer = await customerProcessor.SaveAsync(Customer, requireIsParentUpdate: true, token: token);
                return new CustomerResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    Customer = customer,
                };
            }, logger);
        }

        public async Task<CustomersResult> SaveItemsAsync(string SessionKey, Customer[] Customer)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = (await customerProcessor.SaveItemsAsync(Customer, token)).ToList();
                return new CustomersResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    Customers = result
                };
            }, logger);
        }

        public async Task<CustomersResult> GetParentItemsAsync(string SessionKey, int CompanyId)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = (await customerProcessor.GetAsync(new CustomerSearch {
                    CompanyId   = CompanyId,
                    IsParent    = 1,
                }, token)).ToList();
                return new CustomersResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    Customers = result,
                };
            }, logger);
        }

        public async Task<CustomersResult> GetCustomerDetailItemsAsync(int CompanyId, string Code, string Name, int ShareTransferFee,
            int ClosingDay, string SessionKey)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = (await customerProcessor.GetAsync(new CustomerSearch {
                    CompanyId           = CompanyId,
                    Codes               = new[] { Code },
                    ShareTransferFee    = ShareTransferFee,
                    ClosingDay          = ClosingDay,
                }, token)).ToList();

                return new CustomersResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    Customers = result,
                };

            }, logger);
        }

        public async Task<CustomersResult> GetByCodeAsync(string SessionKey, int CompanyId, string[] Code)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = (await customerProcessor.GetAsync(new CustomerSearch {
                    CompanyId   = CompanyId,
                    Codes       = Code,
                }, token)).ToList();

                return new CustomersResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    Customers = result,
                };

            }, logger);
        }

        public async Task<CustomersResult> GetItemsWithAsync(int CompanyId, string SessionKey)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = (await customerProcessor.GetAsync(new CustomerSearch {
                    CompanyId   = CompanyId,
                    IsParent    = 1,
                }, token)).ToList();

                return new CustomersResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    Customers = result,
                };

            }, logger);
        }


        public async Task<ExistResult> ExistStaffAsync(string SessionKey, int StaffId)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = await customerProcessor.ExistStaffAsync(StaffId, token);
                return new ExistResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    Exist = result,
                };
            }, logger);
        }

        public async Task<CustomersResult> GetItemsAsync(string Sessionkey, int CompanyId, CustomerSearch CustomerSearch)
        {
            return await authorizationProcessor.DoAuthorizeAsync(Sessionkey, async token =>
            {
                (CustomerSearch = CustomerSearch ?? new CustomerSearch()).CompanyId = CompanyId;
                var result = (await customerProcessor.GetAsync(CustomerSearch, token)).ToList();
                return new CustomersResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    Customers = result,
                };
            }, logger);
        }

        public async Task<ExistResult> ExistCategoryAsync(string SessionKey, int CategoryId)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = await customerProcessor.ExistCategoryAsync(CategoryId, token);

                return new ExistResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    Exist = result,
                };
            }, logger);
        }

        public async Task<ExistResult> ExistCompanyAsync(string SessionKey, int CompanyId)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = await customerProcessor.ExistCompanyAsync(CompanyId, token);

                return new ExistResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    Exist = result,
                };
            }, logger);
        }

        public async Task<CustomersResult> GetByChildDetailsAsync(string SessionKey, int CompanyId, int ParentId)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = (await customerProcessor.GetAsync(new CustomerSearch {
                    CompanyId           = CompanyId,
                    XorParentCustomerId = ParentId,
                    IsParent            = 0,
                }, token)).ToList();

                return new CustomersResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    Customers = result,
                };
            }, logger);
        }

        public async Task<CountResult> DeleteAsync(string SessionKey, int CustomerId)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = await customerProcessor.DeleteAsync(CustomerId, token);
                return new CountResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    Count = result,
                };
            }, logger);

        }

        public async Task<CustomerPaymentContractResult> SavePaymentContractAsync(string SessionKey, CustomerPaymentContract CustomerPaymentContract)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = await customerPaymentContractProcessor.SaveAsync(CustomerPaymentContract, token);
                return new CustomerPaymentContractResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    Payment =  result ,
                };
            }, logger);
        }

        public async Task<CountResult> DeletePaymentContractAsync(string SessionKey, int CustomerId)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = await customerPaymentContractProcessor.DeleteAsync(CustomerId, token);
                return new CountResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    Count = result,
                };
            }, logger);
        }

        public async Task<CustomerDiscountResult> SaveDiscountAsync(string SessionKey, CustomerDiscount CustomerDiscount)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = await customerDiscountProcessor.SaveAsync(CustomerDiscount, token);
                return new CustomerDiscountResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    CustomerDiscount = result,
                };
            }, logger);
        }

        public async Task<CountResult> DeleteDiscountAsync(string SessionKey, int CustomerId, int Sequence)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = await customerDiscountProcessor.DeleteAsync(new CustomerDiscount { CustomerId = CustomerId, Sequence = Sequence, }, token);
                return new CountResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    Count = result,
                };
            }, logger);
        }

        public async Task<CustomerDiscountsResult> GetDiscountItemsAsync(string SessionKey, CustomerSearch CustomerSearch)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = (await customerDiscountProcessor.GetItemsAsync(CustomerSearch, token)).ToList();

                return new CustomerDiscountsResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    CustomerDiscounts = result,
                };
            }, logger);
        }

        public async Task<CustomerPaymentContractsResult> GetPaymentContractAsync(string SessionKey, int[] CustomerId)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = (await customerPaymentContractProcessor.GetAsync(CustomerId, token)).ToList();
                return new CustomerPaymentContractsResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    Payments = result,
                };

            }, logger);

        }

        public async Task<CustomerDiscountsResult> GetDiscountAsync(string SessionKey,int customerId)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = (await customerDiscountProcessor.GetAsync(customerId, token)).ToList();
                return new CustomerDiscountsResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    CustomerDiscounts = result,
                };

            }, logger);
        }

        public async Task<CustomersResult> GetAsync(string SessionKey, int[] Id)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = (await customerProcessor.GetAsync(new CustomerSearch { Ids = Id }, token)).ToList();

                return new CustomersResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    Customers = result,
                };

            }, logger);
        }

        public async Task<CustomersResult> GetCustomerGroupAsync(string SessionKey, int CompanyId, int ParentId)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = (await customerProcessor.GetAsync(new CustomerSearch {
                    CompanyId           = CompanyId,
                    ParentCustomerId    = ParentId,
                }, token)).ToList();

                return new CustomersResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    Customers = result,
                };
            }, logger);
        }

        public async Task<CustomersResult> GetCustomerWithListAsync(string SessionKey, int CompanyId, int[] CusId)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = (await customerProcessor.GetAsync(new CustomerSearch {
                    CompanyId   = CompanyId,
                    Ids         = CusId,
                }, token)).ToList();

                return new CustomersResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    Customers = result,
                };
            }, logger);
        }

        public async Task<CustomerResult> GetTopCustomerAsync(string SessionKey, Customer Customer)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var customerResult = (await customerProcessor.GetAsync(new CustomerSearch {
                    CompanyId               = Customer.CompanyId,
                    ExclusiveBankCode       = Customer.ExclusiveBankCode,
                    ExclusiveBranchCode     = Customer.ExclusiveBranchCode,
                    ExclusiveAccountNumber  = Customer.ExclusiveAccountNumber,
                }, token)).FirstOrDefault();

                return new CustomerResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    Customer = customerResult,
                };

            }, logger);
        }

        #region for import (replace mode)

        public async Task< MasterDatasResult > GetImportItemsForCustomerGroupParentAsync(string SessionKey, int CompanyId, string[] Code)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = (await customerProcessor.GetImportForCustomerGroupParentAsync(CompanyId, Code, token)).ToList();
                return new MasterDatasResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    MasterDatas = result,
                };
            }, logger);
        }

        public async Task<MasterDatasResult> GetImportItemsForCustomerGroupChildAsync(string SessionKey, int CompanyId, string[] Code)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = (await customerProcessor.GetImportForCustomerGroupChildAsync(CompanyId, Code, token)).ToList();

                return new MasterDatasResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    MasterDatas = result,
                };

            }, logger);
        }

        public async Task<MasterDatasResult> GetImportItemsForKanaHistoryAsync(string SessionKey, int CompanyId, string[] Code)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = (await customerProcessor.GetImportForKanaHistoryAsync(CompanyId, Code, token)).ToList();
                return new MasterDatasResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    MasterDatas = result,
                };
            }, logger);
        }

        public async Task<MasterDatasResult> GetImportItemsForBillingAsync(string SessionKey, int CompanyId, string[] Code)
        {

            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = (await customerProcessor.GetImportForBillingAsync(CompanyId, Code, token)).ToList();
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
                var result = (await customerProcessor.GetImportForReceiptAsync(CompanyId, Code, token)).ToList();
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
                var result = (await customerProcessor.GetImportForNettingAsync(CompanyId, Code, token)).ToList();
                return new MasterDatasResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    MasterDatas = result,
                };
            }, logger);
        }

        #endregion

        public async Task<ImportResult> ImportAsync(string SessionKey,
            Customer[] InsertList, Customer[] UpdateList, Customer[] DeleteList)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                return await customerProcessor.ImportAsync(InsertList, UpdateList, DeleteList, token);
            }, logger);
        }

        public async Task<CustomerMinsResult> GetMinItemsAsync(string SessionKey, int CompanyId)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var items = (await customerProcessor.GetMinItemsAsync(CompanyId, token)).ToList();
                return new CustomerMinsResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    Customers = items,
                };
            }, logger);
        }
    }
}



