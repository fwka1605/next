using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rac.VOne.Web.Models;
using Rac.VOne.Data;
using Rac.VOne.Data.QueryProcessors;
using static Rac.VOne.Common.Constants;
using System.Threading;

namespace Rac.VOne.Web.Common
{
    public class BillingSaveProcessor :
        IBillingSaveProcessor,
        IBillingSaveForInputProcessor
    {
        private readonly IBillingProcessor billingProcessor;

        private readonly IBillingQueryProcessor billingQueryProcessor;
        private readonly IAddBillingQueryProcessor addBillingQueryProcessor;
        private readonly IUpdateBillingQueryProcessor updatebillingQueryProcessor;
        private readonly ITransactionalGetByIdsQueryProcessor<Billing> billingGetByIdsQueryProcessor;
        private readonly IAddBillingInputQueryProcessor addBillingInputQueryProcessor;
        private readonly IBillingMemoProcessor billingMemoProcessor;
        private readonly ICustomerDiscountQueryProcessor customerDiscountQueryProcessor;
        private readonly IDeleteBillingDiscountQueryProcessor deleteBillingDiscountQueryProcessor;
        private readonly IAddBillingDiscountQueryProcessor addBillingDiscountQueryProcessor;
        private readonly IByCompanyGetEntityQueryProcessor<BillingDivisionSetting> billingDivisionSettingQueryProcessor;
        private readonly IBillingDivisionContractQueryProcessor billingDivisionContractQueryProcessor;
        private readonly IAddBillingDivisionContractQueryProcessor addBillingDivisionContractQueryProcessor;
        private readonly IUpdateBillingDivisionContractQueryProcessor updateBillingDivisionContractQueryProcessor;
        private readonly IDeleteBillingDivisionContractQueryProcessor deleteBillingDivisionContractQueryProcessor;
        private readonly IByCompanyGetEntityQueryProcessor<ApplicationControl> applicationControlQueryProcessor;
        private readonly ITransactionScopeBuilder transactionScopeBuilder;

        public BillingSaveProcessor(
            IBillingProcessor billingProcessor,
            IBillingQueryProcessor billingQueryProcessor,
            IAddBillingQueryProcessor addBillingQueryProcessor,
            IUpdateBillingQueryProcessor updatebillingQueryProcessor,
            ITransactionalGetByIdsQueryProcessor<Billing> billingGetByIdsQueryProcessor,
            IAddBillingInputQueryProcessor addBillingInputQueryProcessor,
            IBillingMemoProcessor billingMemoProcessor,
            ICustomerDiscountQueryProcessor customerDiscountQueryProcessor,
            IDeleteBillingDiscountQueryProcessor deleteBillingDiscountQueryProcessor,
            IAddBillingDiscountQueryProcessor addBillingDiscountQueryProcessor,
            IByCompanyGetEntityQueryProcessor<BillingDivisionSetting> billingDivisionSettingQueryProcessor,
            IBillingDivisionContractQueryProcessor billingDivisionContractQueryProcessor,
            IAddBillingDivisionContractQueryProcessor addBillingDivisionContractQueryProcessor,
            IUpdateBillingDivisionContractQueryProcessor updateBillingDivisionContractQueryProcessor,
            IDeleteBillingDivisionContractQueryProcessor deleteBillingDivisionContractQueryProcessor,
            IByCompanyGetEntityQueryProcessor<ApplicationControl> applicationControlQueryProcessor,
            ITransactionScopeBuilder transactionScopeBuilder
            )
        {
            this.billingProcessor = billingProcessor;
            this.billingQueryProcessor = billingQueryProcessor;
            this.addBillingQueryProcessor = addBillingQueryProcessor;
            this.updatebillingQueryProcessor = updatebillingQueryProcessor;
            this.billingGetByIdsQueryProcessor = billingGetByIdsQueryProcessor;
            this.addBillingInputQueryProcessor = addBillingInputQueryProcessor;
            this.billingMemoProcessor = billingMemoProcessor;
            this.customerDiscountQueryProcessor = customerDiscountQueryProcessor;
            this.deleteBillingDiscountQueryProcessor = deleteBillingDiscountQueryProcessor;
            this.addBillingDiscountQueryProcessor = addBillingDiscountQueryProcessor;
            this.billingDivisionSettingQueryProcessor = billingDivisionSettingQueryProcessor;
            this.billingDivisionContractQueryProcessor = billingDivisionContractQueryProcessor;
            this.addBillingDivisionContractQueryProcessor = addBillingDivisionContractQueryProcessor;
            this.updateBillingDivisionContractQueryProcessor = updateBillingDivisionContractQueryProcessor;
            this.deleteBillingDivisionContractQueryProcessor = deleteBillingDivisionContractQueryProcessor;
            this.applicationControlQueryProcessor = applicationControlQueryProcessor;
            this.transactionScopeBuilder = transactionScopeBuilder;
        }

        public async Task<IEnumerable<Billing>> SaveItemsAsync(IEnumerable<Billing> billings, CancellationToken token = default(CancellationToken))
        {
            var source = billings.ToArray();
            var result = new List<Billing>();

            using (var scope = transactionScopeBuilder.Create())
            {

                BillingInput billingInput = null;
                var index = 0;
                if (source[0].BillingInputId == null && source[0].Id == 0)
                    billingInput = await addBillingInputQueryProcessor.AddAsync(token);

                foreach (var billing in source)
                {
                    if (billingInput != null)
                        billing.BillingInputId = billingInput.Id;

                    result.Add(await SaveInnerAsync(billing));

                    if (billing.UseDiscount)
                    {
                        var discounts = (await customerDiscountQueryProcessor.GetAsync(billing.CustomerId, token)).ToList();

                        if (discounts.Count > 0)
                        {
                            var discount = new BillingDiscount();
                            var billingId = result[index].Id;
                            discount.BillingId = billingId;
                            await deleteBillingDiscountQueryProcessor.DeleteAsync(billingId, token);

                            for (var i = 0; i < discounts.Count; i++)
                            {
                                var customerDiscount = discounts[i];
                                discount.DiscountType = customerDiscount.Sequence;
                                discount.DiscountAmount = customerDiscount.GetRoundingValue(billing.BillingAmount, billing.CurrencyPrecision);
                                await addBillingDiscountQueryProcessor.SaveAsync(discount, token);
                            }
                        }
                    }
                    else if (!billing.UseDiscount && billing.BillingDiscountId != null)
                    {
                        var billingId = result[index].Id;
                        await deleteBillingDiscountQueryProcessor.DeleteAsync(billingId, token);
                    }

                    /* query から見直し updateBilling の 修正が必要 */
                    var contract = new BillingDivisionContract();
                    contract.BillingId = result[index].Id;
                    contract.CreateBy = billing.CreateBy;
                    contract.UpdateBy = billing.UpdateBy;
                    contract.CompanyId = billing.CompanyId;
                    contract.CustomerId = billing.CustomerId;
                    contract.ContractNumber = billing.ContractNumber;

                    if (billing.BillingDivisionContract == 1)
                        await updateBillingDivisionContractQueryProcessor.UpdateBillingAsync(contract, token);
                    else if (billing.BillingDivisionContract == 2)
                        await deleteBillingDivisionContractQueryProcessor.DeleteWithBillingIdAsync(billing.Id, token);
                    else if (billing.BillingDivisionContract == 3)
                    {
                        var newContractNo = await billingDivisionContractQueryProcessor.GetNewContractNumberAsync(contract.CompanyId, token);
                        contract.ContractNumber = newContractNo.ToString();
                        var setting = await billingDivisionSettingQueryProcessor.GetAsync(billing.CompanyId, token);
                        contract.FirstDateType = setting.FirstDateType;
                        contract.Monthly = setting.Monthly;
                        contract.BasisDay = setting.BasisDay;
                        contract.DivisionCount = setting.DivisionCount;
                        contract.RoundingMode = setting.RoundingMode;
                        contract.RemainsApportionment = setting.RemainsApportionment;
                        contract.Comfirm = 0;
                        contract.CancelDate = null;
                        await addBillingDivisionContractQueryProcessor.SaveAsync(contract, token);
                    }
                    index++;
                }
                scope.Complete();
            }
            return result;
        }

        public async Task<IEnumerable<Billing>> SaveAsync(IEnumerable<Billing> billings, CancellationToken token = default(CancellationToken))
        {
            var results = new List<Billing>();

            using (var scope = transactionScopeBuilder.Create())
            {
                var source = billings.ToArray();
                var first = source.First();
                var userId = first.UpdateBy;
                var appControl = await applicationControlQueryProcessor.GetAsync(first.CompanyId, token);
                List<CustomerDiscount> discounts = null;
                var useDiscount = appControl.UseDiscount == 1;
                if (useDiscount)
                    discounts = (await customerDiscountQueryProcessor.GetAsync(first.CustomerId, token)).ToList();
                var useLongTermAdv = appControl.UseLongTermAdvanceReceived == 1;
                var contractRegistered = appControl.RegisterContractInAdvance == 1;
                BillingDivisionSetting setting = null;
                if (useLongTermAdv && !contractRegistered)
                    setting = await billingDivisionSettingQueryProcessor.GetAsync(first.CompanyId, token);
                var inputId = first.BillingInputId;
                var isInput = first.InputType == (int)BillingInputType.BillingInput ||
                              first.InputType == (int)BillingInputType.PeriodicBilling;
                var requireInputId = isInput && !inputId.HasValue;
                if (requireInputId)
                {
                    inputId = (await addBillingInputQueryProcessor.AddAsync(token)).Id;
                    foreach (var item in source)
                        item.BillingInputId = inputId;
                }
                else if (isInput && inputId.HasValue)
                {
                    var registeredIds = (await billingQueryProcessor.GetByBillingInputIdAsync(inputId.Value)).Select(x => x.Id).ToList();
                    var deleteSource = new BillingDeleteSource {
                        Ids         = registeredIds.Where(x => !source.Any(y => y.Id == x)).ToArray(),
                        CompanyId   = first.CompanyId,
                        LoginUserId = userId,
                    };
                    await billingProcessor.DeleteAsync(deleteSource, token);
                }
                var requireDiscount = source.Any(x => x.UseDiscount);

                foreach (var item in source)
                {
                    var result = await SaveInnerAsync(item, token);
                    if (useDiscount)
                    {
                        if (!item.UseDiscount && item.BillingDiscountId.HasValue)
                            await deleteBillingDiscountQueryProcessor.DeleteAsync(result.Id, token);
                        if (item.UseDiscount && discounts.Any())
                        {
                            await deleteBillingDiscountQueryProcessor.DeleteAsync(result.Id, token);
                            var billingDiscount = new BillingDiscount { BillingId = result.Id };
                            foreach (var x in discounts)
                            {
                                billingDiscount.DiscountType = x.Sequence;
                                billingDiscount.DiscountAmount = x.GetRoundingValue(item.BillingAmount, item.CurrencyPrecision);
                                await addBillingDiscountQueryProcessor.SaveAsync(billingDiscount, token);
                            }
                        }
                    }
                    if (useLongTermAdv && item.BillingDiscountId > 0)
                    {
                        var contract = new BillingDivisionContract
                        {
                            BillingId       = result.Id,
                            CreateBy        = userId,
                            UpdateBy        = userId,
                            CompanyId       = item.CompanyId,
                            CustomerId      = item.CustomerId,
                            ContractNumber  = item.ContractNumber,
                        };
                        if (item.BillingDivisionContract == 1)
                            await updateBillingDivisionContractQueryProcessor.UpdateBillingAsync(contract, token);
                        else if (item.BillingDivisionContract == 2)
                            await deleteBillingDivisionContractQueryProcessor.DeleteWithBillingIdAsync(result.Id, token);
                        else if (item.BillingDivisionContract == 3)
                        {
                            var newNumber = await billingDivisionContractQueryProcessor.GetNewContractNumberAsync(contract.CompanyId, token);
                            contract.ContractNumber = newNumber.ToString();
                            contract.FirstDateType = setting.FirstDateType;
                            contract.Monthly = setting.Monthly;
                            contract.BasisDay = setting.BasisDay;
                            contract.DivisionCount = setting.DivisionCount;
                            contract.RoundingMode = setting.RoundingMode;
                            contract.RemainsApportionment = setting.RemainsApportionment;
                            contract.Comfirm = 0;
                            await addBillingDivisionContractQueryProcessor.SaveAsync(contract, token);
                        }
                    }
                    results.Add(result);
                }
                scope.Complete();
            }

            return results;
        }

        public async Task<Billing> SaveAsync(Billing Billing, CancellationToken token = default(CancellationToken))
        {
            Billing result = null;
            using (var scope = transactionScopeBuilder.Create())
            {
                result = await SaveInnerAsync(Billing, token);
                scope.Complete();
            }
            return result;
        }

        /// <summary>
        /// 請求データ登録用 without transaction scope
        /// </summary>
        /// <param name="billing"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        private async Task<Billing> SaveInnerAsync(Billing billing, CancellationToken token = default(CancellationToken))
        {
            Billing result = null;
            if (billing.Id > 0)
            {
                var current = (await billingGetByIdsQueryProcessor.GetByIdsAsync(new long[] { billing.Id }, token)).FirstOrDefault();
                if (current == null) throw new Exception("請求データが存在しないため、修正できません。");
                if (current.AssignmentFlag != 0
                    || current.OutputAt != null
                    || current.InputType == 3
                    || current.ResultCode == 0)
                {
                    throw new Exception("修正不可能な請求データです。");
                }
                else
                {
                    result = await updatebillingQueryProcessor.UpdateAsync(billing, token);
                }
            }
            else
            {
                result = await addBillingQueryProcessor.AddAsync(billing, token);
            }

            if (string.IsNullOrEmpty(billing.Memo))
            {
                await billingMemoProcessor.DeleteAsync(result.Id, token);
            }
            else
            {
                await billingMemoProcessor.SaveMemoAsync(result.Id, billing.Memo, token);
            }
            return result;
        }
    }
}
