using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Rac.VOne.Web.Common;
using Rac.VOne.Web.Models;
using Rac.VOne.Web.Service.Extensions;
using Rac.VOne.Common.Logging;
using NLog;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Service.Master
{
    public class KanaHistoryCustomerMaster : IKanaHistoryCustomerMaster
    {
        private readonly IAuthorizationProcessor authorizationProcessor;
        private readonly IKanaHistoryCustomerProcessor kanaHistoryCustomerProcessor;
        private readonly ILogger logger;

        public KanaHistoryCustomerMaster(
            IAuthorizationProcessor authorizationProcessor,
            IKanaHistoryCustomerProcessor kanaHistoryCustomerProcessor,
            ILogManager logManager
            )
        {
            this.authorizationProcessor = authorizationProcessor;
            this.kanaHistoryCustomerProcessor = kanaHistoryCustomerProcessor;
            logger = logManager.GetLogger(typeof(KanaHistoryCustomerMaster));
        }

        public async Task<ExistResult> ExistCustomerAsync(string SessionKey, int CustomerId)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = await kanaHistoryCustomerProcessor.ExistCustomerAsync(CustomerId, token);
                return new ExistResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    Exist = result,
                };
            }, logger);
        }

        public async Task<KanaHistoryCustomersResult> GetListAsync(string SessionKey, int CompanyId, string PayerName, string CustomerCodeFrom, string CustomerCodeTo)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = (await kanaHistoryCustomerProcessor.GetAsync(new KanaHistorySearch {
                    CompanyId   = CompanyId,
                    PayerName   = PayerName,
                    CodeFrom    = CustomerCodeFrom,
                    CodeTo      = CustomerCodeTo,
                }, token)).ToList();
                return new KanaHistoryCustomersResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    KanaHistoryCustomers = result,
                };
            }, logger);
        }

        public async Task<CountResult> DeleteAsync(string SessionKey, int CompanyId, string PayerName, string SourceBankName, string SourceBranchName, int CustomerId)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = await kanaHistoryCustomerProcessor.DeleteAsync(new KanaHistoryCustomer {
                    CompanyId           = CompanyId,
                    PayerName           = PayerName,
                    SourceBankName      = SourceBankName,
                    SourceBranchName    = SourceBranchName,
                    CustomerId          = CustomerId,
                }, token);
                return new CountResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    Count = result,
                };
            }, logger);
        }

        public async Task<ImportResult> ImportAsync(string SessionKey,
            KanaHistoryCustomer[] InsertList, KanaHistoryCustomer[] UpdateList, KanaHistoryCustomer[] DeleteList)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = await kanaHistoryCustomerProcessor.ImportAsync(InsertList, UpdateList, DeleteList, token);
                return result;
            }, logger);
        }
    }
}
