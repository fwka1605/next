using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Rac.VOne.Common.Logging;
using Rac.VOne.Web.Common;
using Rac.VOne.Web.Models;
using Rac.VOne.Web.Service.Extensions;
using Microsoft.AspNet.SignalR;
using NLog;

namespace Rac.VOne.Web.Service
{
    public class BillingInvoiceService : IBillingInvoiceService
    {
        private readonly IAuthorizationProcessor authorizationProcessor;
        private readonly IBillingInvoiceProcessor billingInvoiceProcessor;
        private readonly ILogger logger;
        private readonly IHubContext hubContext;

        public BillingInvoiceService(
            IAuthorizationProcessor authorizationProcessor,
            IBillingInvoiceProcessor billingInvoiceProcessor,
            ILogManager logManager)
        {
            this.authorizationProcessor = authorizationProcessor;
            this.billingInvoiceProcessor = billingInvoiceProcessor;

            logger = logManager.GetLogger(typeof(BillingInvoiceService));
            hubContext = GlobalHost.ConnectionManager.GetHubContext<Hubs.ProgressHub>();
        }

        public async Task<BillingInvoiceResult> GetAsync(string SessionKey,
            BillingInvoiceSearch searchOption,
            string connectionId)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var notifier = hubContext.CreateNotifier(connectionId);
                var result = await billingInvoiceProcessor.GetAsync(searchOption, token);
                notifier?.UpdateState();
                return new BillingInvoiceResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    BillingInvoices = result.ToList(),
                };
            }, logger, connectionId);
        }

        public async Task<BillingInputResult> PublishInvoicesAsync(string SessionKey,
            string connectionId,
            BillingInvoiceForPublish[] invoices,
            int LoginUserId)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = await billingInvoiceProcessor.PublishInvoicesAsync(invoices, LoginUserId, token);
                return result;
            }, logger, connectionId);
        }

        public async Task<BillingInvoiceDetailResult> GetDetailsForPrintAsync(string SessionKey,
            BillingInvoiceDetailSearch billingInvoiceDetailSearch)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = (await billingInvoiceProcessor.GetDetailsForPrintAsync(billingInvoiceDetailSearch, token)).ToList();
                return new BillingInvoiceDetailResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    BillingInvoicesDetails = result,
                };
            }, logger);
        }

        public async Task<CountResult> GetCountAsync(string SessionKey,
            BillingInvoiceSearch searchOption,
            string connectionId)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var notifier = hubContext.CreateNotifier(connectionId);
                var result = (await billingInvoiceProcessor.GetAsync(searchOption, token)).Count();
                notifier?.UpdateState();
                return new CountResult()
                {
                    ProcessResult = new ProcessResult { Result = true },
                    Count = result
                };
            }, logger, connectionId);
        }

        public async Task<CountResult> DeleteWorkTableAsync(string SessionKey, byte[] ClientKey)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = await billingInvoiceProcessor.DeleteWorkTableAsync(ClientKey, token);
                return new CountResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    Count = result,
                };
            }, logger);
        }

        public async Task<CountResult> CancelPublishAsync(string SessionKey,
            string connectionId,
            long[] BilinngInputIds)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = await billingInvoiceProcessor.CancelPublishAsync(BilinngInputIds, token);
                return result;
            }, logger, connectionId);
        }

        public async Task<BillingInvoiceDetailForExportResult> GetDetailsForExportAsync(
            string SessionKey,
            long[] BillingInputIds,
            int CompanyId,
            string connectionId)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var notifier = hubContext.CreateNotifier(connectionId);
                var result = (await billingInvoiceProcessor.GetDetailsForExportAsync(BillingInputIds, CompanyId, token)).ToList();
                notifier?.UpdateState();
                return new BillingInvoiceDetailForExportResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    BillingInvoicesDetails = result,
                };
            }, logger, connectionId);
        }

        public async Task<CountResult> UpdatePublishAtAsync(string SessionKey,
            string connectionId,
            long[] BilinngInputIds)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = await billingInvoiceProcessor.UpdatePublishAtAsync(BilinngInputIds, token);
                return result;
            }, logger, connectionId);
        }
    }
}
