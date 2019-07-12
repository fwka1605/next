using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Rac.VOne.Web.Models;
using Rac.VOne.Data;
using Rac.VOne.Data.QueryProcessors;

namespace Rac.VOne.Web.Common
{
    public class ReceiptApportionProcessor : IReceiptApportionProcessor
    {
        private readonly IReceiptApportionByIdQueryProcessor receiptApportionQueryProcessor;
        private readonly IUpdateReceiptHeaderQueryProcessor updateReceiptHeaderQueryProcessor;
        private readonly IDeleteReceiptQueryProcessor deleteReceiptQueryProcessor;
        private readonly IUpdateReceiptApportionQueryProcessor updateReceiptApportionQueryProcessor;
        private readonly IAddReceiptExcludeQueryProcessor addReceiptExcludeQueryProcessor;
        private readonly IByCompanyGetEntityQueryProcessor<ApplicationControl> applicationControlGetByCompanyQueryProcessor;
        private readonly IByCompanyGetEntityQueryProcessor<CollationSetting> collationSettingGetByCompanyQueryProcessor;
        private readonly ICustomerQueryProcessor customerQueryProcessor;
        private readonly ICustomerGroupByIdQueryProcessor customerGroupByIdQueryProcessor;
        private readonly ISectionQueryProcessor sectionQueryProcessor;
        private readonly IAddKanaHistoryCustomerQueryProcessor addKanaHistoryCustomerQueryProcessor;
        private readonly IIgnoreKanaByCodeQueryProcessor ignoreKanaByCodeQueryProcessor;
        private readonly IAddIgnoreKanaQueryProcessor addIgnoreKanaQueryProcessor;
        private readonly ITransactionScopeBuilder transactionScopeBuilder;

        public ReceiptApportionProcessor(
            IReceiptApportionByIdQueryProcessor receiptApportionQueryProcessor,
            IUpdateReceiptHeaderQueryProcessor updateReceiptHeaderQueryProcessor,
            IDeleteReceiptQueryProcessor deleteReceiptQueryProcessor,
            IUpdateReceiptApportionQueryProcessor updateReceiptApportionQueryProcessor,
            IAddReceiptExcludeQueryProcessor addReceiptExcludeQueryProcessor,
            IByCompanyGetEntityQueryProcessor<ApplicationControl> applicationControlGetByCompanyQueryProcessor,
            IByCompanyGetEntityQueryProcessor<CollationSetting> collationSettingGetByCompanyQueryProcessor,
            ICustomerQueryProcessor customerQueryProcessor,
            ICustomerGroupByIdQueryProcessor customerGroupByIdQueryProcessor,
            ISectionQueryProcessor sectionQueryProcessor,
            IAddKanaHistoryCustomerQueryProcessor addKanaHistoryCustomerQueryProcessor,
            IIgnoreKanaByCodeQueryProcessor ignoreKanaByCodeQueryProcessor,
            IAddIgnoreKanaQueryProcessor addIgnoreKanaQueryProcessor,
            ITransactionScopeBuilder transactionScopeBuilder
            )
        {
            this.receiptApportionQueryProcessor = receiptApportionQueryProcessor;
            this.updateReceiptHeaderQueryProcessor = updateReceiptHeaderQueryProcessor;
            this.deleteReceiptQueryProcessor = deleteReceiptQueryProcessor;
            this.updateReceiptApportionQueryProcessor = updateReceiptApportionQueryProcessor;
            this.addReceiptExcludeQueryProcessor = addReceiptExcludeQueryProcessor;
            this.applicationControlGetByCompanyQueryProcessor = applicationControlGetByCompanyQueryProcessor;
            this.collationSettingGetByCompanyQueryProcessor = collationSettingGetByCompanyQueryProcessor;
            this.customerQueryProcessor = customerQueryProcessor;
            this.customerGroupByIdQueryProcessor = customerGroupByIdQueryProcessor;
            this.sectionQueryProcessor = sectionQueryProcessor;
            this.addKanaHistoryCustomerQueryProcessor = addKanaHistoryCustomerQueryProcessor;
            this.ignoreKanaByCodeQueryProcessor = ignoreKanaByCodeQueryProcessor;
            this.addIgnoreKanaQueryProcessor = addIgnoreKanaQueryProcessor;
            this.transactionScopeBuilder = transactionScopeBuilder;
        }

        public async Task<IEnumerable<ReceiptApportion>> GetAsync(IEnumerable<long> ids, CancellationToken token = default(CancellationToken))
        {
            var entities = (await receiptApportionQueryProcessor.GetApportionItemsAsync(ids, token)).ToArray();

            if (entities.Any(x => x.Apportioned == 0))
            {
                var companyId = entities.First().CompanyId;
                var app = await applicationControlGetByCompanyQueryProcessor.GetAsync(companyId, token);
                var collation = await collationSettingGetByCompanyQueryProcessor.GetAsync(companyId, token);

                foreach (var receipt in entities.Where(x => x.Apportioned == 0))
                {
                    var customerId = receipt.CustomerId ?? receipt.RefCustomerId;
                    if (app?.UseReceiptSection == 1 && !receipt.SectionId.HasValue && customerId.HasValue)
                    {
                        var section = (await sectionQueryProcessor.GetAsync(new SectionSearch {
                            CustomerId = customerId.Value,
                        })).FirstOrDefault();
                        if (section != null)
                        {
                            receipt.SectionId = section.Id;
                            receipt.SectionCode = section.Code;
                            receipt.SectionName = section.Name;
                        }
                    }
                    if (collation?.AutoAssignCustomer == 1 && !receipt.CustomerId.HasValue && customerId.HasValue)
                    {
                        receipt.CustomerId = receipt.RefCustomerId;
                        receipt.CustomerCode = receipt.RefCustomerCode;
                        receipt.CustomerName = receipt.RefCustomerName;
                    }

                    if (collation?.UseApportionMenu == 1 && receipt.ExcludeFlag == 0 && !receipt.ExcludeCategoryId.HasValue)
                    {
                        var ignoreKana = (await ignoreKanaByCodeQueryProcessor.GetAsync(new IgnoreKana {
                            CompanyId       = companyId,
                            Kana            = receipt.PayerNameRaw,
                        }, token)).FirstOrDefault();
                        if (ignoreKana != null)
                        {
                            receipt.ExcludeFlag = 1;
                            receipt.ExcludeCategoryId = ignoreKana.ExcludeCategoryId;
                            receipt.ExcludeAmount = receipt.ReceiptAmount;
                        }
                    }
                }
            }
            return entities;
        }

        public async Task<ReceiptApportionsResult> ApportionAsync(IEnumerable<ReceiptApportion> apportions, CancellationToken token = default(CancellationToken))
        {
            var result = new ReceiptApportionsResult {
                ProcessResult           = new ProcessResult(),
                ReceiptApportion        = new List<ReceiptApportion>(),
            };

            var updatedList = new List<ReceiptApportion>();
            using (var scope = transactionScopeBuilder.Create())
            {
                foreach (var apportion in apportions)
                {
                    if (apportion.DoDelete == 0)
                    {
                        var apportionResult = await updateReceiptApportionQueryProcessor.UpdateApportionAsync(apportion, token);

                        if (apportionResult == null) return result;

                        if (apportion.ExcludeFlag == 1)
                        {
                            var excludeResult = await addReceiptExcludeQueryProcessor.SaveAsync(new ReceiptExclude {
                                ReceiptId           = apportion.Id,
                                ExcludeAmount       = apportion.ExcludeAmount.Value,
                                ExcludeCategoryId   = apportion.ExcludeCategoryId.Value,
                                UpdateBy            = apportion.UpdateBy,
                            }, token);

                            if (excludeResult == null) return result;

                            await updateReceiptHeaderQueryProcessor.UpdateAsync(new ReceiptHeaderUpdateOption { ReceiptHeaderId = apportion.ReceiptHeaderId, UpdateBy = apportion.UpdateBy, }, token);

                            apportionResult.ReceiptExcludes.Add(excludeResult);

                        }

                        if (apportion.ExcludeFlag == 1 && apportion.LearnIgnoreKana == 1)
                        {
                            var ignoreKana = (await ignoreKanaByCodeQueryProcessor.GetAsync(new IgnoreKana { CompanyId = apportion.CompanyId, Kana = apportion.PayerNameRaw }, token)).FirstOrDefault();
                            if (ignoreKana == null)
                            {
                                var resIgnore = await addIgnoreKanaQueryProcessor.SaveAsync(new IgnoreKana {
                                    CompanyId           = apportion.CompanyId,
                                    ExcludeCategoryId   = apportion.ExcludeCategoryId.Value,
                                    Kana                = apportion.PayerNameRaw,
                                    UpdateBy            = apportion.UpdateBy,
                                }, token);

                                if (resIgnore == null) return result;
                            }
                        }

                        if (apportion.CustomerId.HasValue && apportion.LearnKanaHistory == 1)
                        {
                            var customerGroup = (await customerGroupByIdQueryProcessor.GetAsync(new CustomerGroupSearch { ChildIds = new[] { apportion.CustomerId.Value } }, token)).FirstOrDefault();
                            var code = customerGroup?.ParentCustomerCode ?? apportion.CustomerCode;
                            var customer = (await customerQueryProcessor.GetAsync(new CustomerSearch { CompanyId = apportion.CompanyId, Codes = new[] { code }, }, token)).First();
                            if (customer.UseKanaLearning == 1)
                            {
                                var kanaHistory = new KanaHistoryCustomer() {
                                    CompanyId           = apportion.CompanyId,
                                    CustomerId          = customer.Id,
                                    PayerName           = apportion.PayerName,
                                    SourceBankName      = apportion.SourceBankName,
                                    SourceBranchName    = apportion.SourceBranchName,
                                    HitCount            = 1,
                                    CreateBy            = apportion.UpdateBy,
                                    UpdateBy            = apportion.UpdateBy
                                };
                                await addKanaHistoryCustomerQueryProcessor.SaveAsync(kanaHistory, token);
                            }
                        }

                        result.ReceiptApportion.Add(apportionResult);
                    }
                    else
                    {
                        var resReceipt = await deleteReceiptQueryProcessor.OmitByApportionAsync(apportion, token);
                        if (resReceipt == 0) return result;
                        await updateReceiptHeaderQueryProcessor.UpdateAsync(new ReceiptHeaderUpdateOption { ReceiptHeaderId = apportion.ReceiptHeaderId, UpdateBy = apportion.UpdateBy }, token);
                        result.ReceiptApportion.Add(apportion);
                    }
                }
                result.ProcessResult.Result = true;
                scope.Complete();
            }
            return result;
        }

    }
}
