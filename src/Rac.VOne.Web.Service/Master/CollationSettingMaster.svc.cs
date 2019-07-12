using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Rac.VOne.Web.Models;
using Rac.VOne.Web.Common;
using Rac.VOne.Web.Service.Extensions;
using NLog;
using Rac.VOne.Common.Logging;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Service.Master
{
    public class CollationSettingMaster : ICollationSettingMaster
    {
        private readonly IAuthorizationProcessor authorizationProcessor;
        private readonly ICollationSettingProcessor collationSettingProcessor;
        private readonly ILogger logger;

        public CollationSettingMaster(
            IAuthorizationProcessor authorizationProcessor,
            ICollationSettingProcessor collationSettingProcessor,
            ILogManager logManager
            )
        {
            this.authorizationProcessor = authorizationProcessor;
            this.collationSettingProcessor = collationSettingProcessor;
            logger = logManager.GetLogger(typeof(CollationSettingMaster));
        }

        public async Task<CollationSettingResult> GetAsync(string SessionKey, int CompanyId)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = await collationSettingProcessor.GetAsync(CompanyId);

                return new CollationSettingResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    CollationSetting = result,
                };
            }, logger);
        }

        public async Task<MatchingOrdersResult> GetMatchingBillingOrderAsync(string SessionKey, int CompanyId)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = (await collationSettingProcessor.GetMatchingBillingOrderAsync(CompanyId, token)).ToList();

                return new MatchingOrdersResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    MatchingOrders = result,
                };
            }, logger);
        }

        public async Task<MatchingOrdersResult> GetMatchingReceiptOrderAsync(string SessionKey, int CompanyId)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = (await collationSettingProcessor.GetMatchingReceiptOrderAsync(CompanyId, token)).ToList();

                return new MatchingOrdersResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    MatchingOrders = result,
                };
            }, logger);
        }

        public async Task<CollationOrdersResult> GetCollationOrderAsync(string SessionKey, int CompanyId)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = (await collationSettingProcessor.GetCollationOrderAsync(CompanyId, token)).ToList();

                return new CollationOrdersResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    CollationOrders = result
                };
            }, logger);
        }

        public async Task<CollationSettingResult> SaveAsync(string SessionKey, CollationSetting CollationSetting, CollationOrder[] CollationOrder,
           MatchingOrder[] MatchingBillingOrder, MatchingOrder[] MatchingReceiptOrder)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                CollationSetting.CollationOrders = CollationOrder;
                CollationSetting.BillingMatchingOrders = MatchingBillingOrder;
                CollationSetting.ReceiptMatchingOrders = MatchingReceiptOrder;

                var result = await collationSettingProcessor.SaveAsync(CollationSetting, token);

                return new CollationSettingResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    CollationSetting = result,
                };
            }, logger);
        }
    }
}
