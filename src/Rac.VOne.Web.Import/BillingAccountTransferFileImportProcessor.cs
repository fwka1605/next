using MsgPack.Serialization;
using Rac.VOne.AccountTransfer.Import;
using Rac.VOne.Common;
using Rac.VOne.Common.DataHandling;
using Rac.VOne.Data.QueryProcessors;
using Rac.VOne.Web.Common;
using Rac.VOne.Web.Common.Importers;
using Rac.VOne.Web.Common.Reports;
using Rac.VOne.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using static Rac.VOne.Common.Constants;

namespace Rac.VOne.Web.Import
{
    /// <summary>口座振替結果データ取込</summary>
    public class BillingAccountTransferFileImportProcessor : IBillingAccountTransferFileImportProcessor
    {
        private readonly IIdenticalEntityGetByIdsQueryProcessor<Company> companyGetByIdQueryProcessor;
        private readonly ICurrencyQueryProcessor currencyQueryProcessor;
        private readonly IBillingQueryProcessor billingQueryProcessor;
        private readonly ICustomerQueryProcessor customerQueryProcessor;
        private readonly IIdenticalEntityGetByIdsQueryProcessor<PaymentAgency> paymentAgencyGetByIdsQueryProcessor;

        private readonly IImportDataProcessor importDataProcessor;
        private readonly IBillingAccountTransferProcessor billingAccountTransferProcessor;

        private readonly MessagePackSerializer<AccountTransferSource> serializer;

        /// <summary>constructor</summary>
        public BillingAccountTransferFileImportProcessor(
            IIdenticalEntityGetByIdsQueryProcessor<Company> companyGetByIdQueryProcessor,
            ICurrencyQueryProcessor currencyQueryProcessor,
            IBillingQueryProcessor billingQueryProcessor,
            ICustomerQueryProcessor customerQueryProcessor,
            IIdenticalEntityGetByIdsQueryProcessor<PaymentAgency> paymentAgencyGetByIdsQueryProcessor,
            IImportDataProcessor importDataProcessor,
            IBillingAccountTransferProcessor billingAccountTransferProcessor
            )
        {
            this.companyGetByIdQueryProcessor = companyGetByIdQueryProcessor;
            this.currencyQueryProcessor = currencyQueryProcessor;
            this.billingQueryProcessor = billingQueryProcessor;
            this.customerQueryProcessor = customerQueryProcessor;
            this.paymentAgencyGetByIdsQueryProcessor = paymentAgencyGetByIdsQueryProcessor;
            this.importDataProcessor = importDataProcessor;
            this.billingAccountTransferProcessor = billingAccountTransferProcessor;

            serializer = MessagePackSerializer.Get<AccountTransferSource>(new SerializationContext { DefaultDateTimeConversionMethod = DateTimeConversionMethod.Native });
        }


        /// <summary>取込処理</summary>
        public async Task<AccountTransferImportResult> ReadAsync(AccountTransferImportSource source, CancellationToken token = default(CancellationToken))
        {
            var encoding = Encoding.GetEncoding(source.EncodingCodePage);
            var csv = encoding.GetString(source.Data);

            var companyTask = companyGetByIdQueryProcessor.GetByIdsAsync(new[] { source.CompanyId }, token);
            var currencyTask = currencyQueryProcessor.GetAsync(new CurrencySearch { CompanyId = source.CompanyId, Codes = new[] { DefaultCurrencyCode }, }, token);
            var agencyTask = paymentAgencyGetByIdsQueryProcessor.GetByIdsAsync(new[] { source.PaymentAgencyId }, token);

            await Task.WhenAll(companyTask, currencyTask, agencyTask);

            var company = companyTask.Result.First();
            var currency = currencyTask.Result.First();
            var agency = agencyTask.Result.First();

            var helper                  = new Helper {
                GetBillingsAsync    = async (companyId, paymentAgencyId, dueAt) => (await billingQueryProcessor.GetAccountTransferMatchingTargetListAsync(paymentAgencyId, dueAt, currency.Id)).ToList(),
                GetCustomersAsync   = async (ids) => (await customerQueryProcessor.GetAsync(new CustomerSearch { Ids = ids, }, token)).ToDictionary(x => x.Id),
            };
            var reader = helper.CreateReader((AccountTransferFileFormatId)agency.FileFormatId);
            reader.CompanyId            = company.Id;
            reader.AggregateBillings    = company.TransferAggregate == 1;
            reader.PaymentAgencyId      = agency.Id;
            reader.TransferYear         = source.TransferYear;
            reader.Encoding             = encoding;
            reader.FileName             = source.FileName;
            reader.IsAsync              = true;
            reader.IsPlainText          = true;

            var sources = await reader.ReadAsync(csv);

            var data = new ImportData {
                CompanyId           = source.CompanyId,
                FileName            = source.FileName,
                FileSize            = encoding.GetByteCount(csv),
                CreateBy            = source.LoginUserId,
            };

            data.Details = sources.Select(x => new ImportDataDetail {
                ObjectType = 0,
                RecordItem = serializer.PackSingleObject(x),
            }).ToArray();

            var dataSaved = await importDataProcessor.SaveAsync(data, token);

            var result = new AccountTransferImportResult {
                ImportData      = dataSaved,
                ProcessResult   = new ProcessResult { Result = true },
            };
            for (var i = 0; i < sources.Count; i++)
            {
                result.ReadCount++;
                if (sources[i].TransferResultCode == 0)
                {
                    result.ValidCount++;
                    result.ValidAmount += sources[i].TransferAmount;
                }
                else
                {
                    result.InvalidCount++;
                    result.InvalidAmount += sources[i].TransferAmount;
                }
            }
            result.InvalidSources = sources.Where(x => x.TransferResultCode != 0 || !(x.Billings?.Any() ?? false)).ToList();
            result.Logs = sources.SelectMany(x => x.GetInvalidLogs()).ToList();

            return result;
        }

        /// <summary>登録処理</summary>
        public async Task<AccountTransferImportResult> ImportAsync(AccountTransferImportSource source, CancellationToken token = default(CancellationToken))
        {
            var importDataId = source.ImportDataId ?? 0;

            var data = await importDataProcessor.GetAsync(importDataId, token: token);
            var agency = (await paymentAgencyGetByIdsQueryProcessor.GetByIdsAsync(new[] { source.PaymentAgencyId }, token)).First();

            var sources = data.Details.Select(x => serializer.UnpackSingleObject(x.RecordItem)).ToArray();

            var items = sources.Select(x => new AccountTransferImportData {
                BillingIdList                   = x.Billings.Select(y => y.Id).ToArray(),
                CustomerIds                     = x.Billings.Select(y => y.CustomerId).ToArray(),
                ResultCode                      = x.TransferResultCode,
                DoUpdateAccountTransferLogId    = !x.IgnoreInitialization,
                DueDate                         = x.NewDueAt,
                UpdateBy                        = source.LoginUserId,
                DueDateOffset                   = agency.DueDateOffset,
                CollectCategoryId               = source.NewCollectCategoryId,
            }).ToArray();

            var importResult = await billingAccountTransferProcessor.ImportAsync(items, token);

            return new AccountTransferImportResult {
                ProcessResult = new ProcessResult { Result = true },
            };
        }

    }
}