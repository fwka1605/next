using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Rac.VOne.Web.Models;
using Rac.VOne.Web.Service.Extensions;
using Rac.VOne.Common.Logging;
using Rac.VOne.Web.Common;
using NLog;
using System.Threading.Tasks;


namespace Rac.VOne.Web.Service
{
    public class MFBillingService : IMFBillingService
    {
        private readonly IAuthorizationProcessor authorizationProcessor;
        private readonly IMFBillingProcessor mfBillingProcessor;
        private readonly ILogger logger;

        public MFBillingService(
            IAuthorizationProcessor authorizationProcessor,
            IMFBillingProcessor mfBillingProcessor,
            ILogManager logManager)
        {
            this.authorizationProcessor = authorizationProcessor;
            this.mfBillingProcessor = mfBillingProcessor;
            logger = logManager.GetLogger(typeof(BillingService));

        }

        public async Task<BillingsResult> SavingSetAsync(string SessionKey, IEnumerable<Billing> billings, IEnumerable<Customer> customers)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = (await mfBillingProcessor.SaveAsync(new MFBillingSource {
                    Billings    = billings.ToArray(),
                    Customers   = customers.ToArray(),
                }, token)).ToList();
                return new BillingsResult() { Billings = result, ProcessResult = new ProcessResult() { Result = true } };
            }, logger);
        }

        public async Task<MFBillingsResult> GetItemsAsync(string SessionKey, int companyId)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = (await mfBillingProcessor.GetAsync(new MFBillingSource { CompanyId = companyId }, token)).ToList();
                return new MFBillingsResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    MFBillings = result,
                };
            }, logger);
        }

        /// <summary>MFC請求書 連携用の <see cref="MFBilling"/> を取得
        /// 入金ステータス変更 MFC請求書の id を取得するため
        /// </summary>
        /// <param name="isMatched">true : 消込済 / false : 未消込 を指定</param>
        public async Task<MFBillingsResult> GetByBillingIdsAsync(string sessionKey, IEnumerable<long> ids, bool isMatched)
        {
            return await authorizationProcessor.DoAuthorizeAsync(sessionKey, async token => {
                var result = (await mfBillingProcessor.GetAsync(new MFBillingSource {
                    Ids         = ids.ToArray(),
                    IsMatched   = isMatched,
                }, token)).ToList();
                return new MFBillingsResult {
                    ProcessResult = new ProcessResult { Result = true },
                    MFBillings = result,
                };
            }, logger);
        }
    }
}
