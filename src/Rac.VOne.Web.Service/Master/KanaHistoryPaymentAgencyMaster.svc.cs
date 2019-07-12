using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Rac.VOne.Web.Models;
using Rac.VOne.Web.Common;
using Rac.VOne.Web.Service.Extensions;
using Rac.VOne.Common.Logging;
using NLog;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Service.Master
{
    public class KanaHistoryPaymentAgencyMaster : IKanaHistoryPaymentAgencyMaster
    {
        private readonly IAuthorizationProcessor authorizationProcessor;
        private readonly IKanaHistoryPaymentAgencyProcessor kanaHistoryPaymentAgencyProcessor;
        private readonly ILogger logger;

        public KanaHistoryPaymentAgencyMaster(
            IAuthorizationProcessor authorizationProcessor,
            IKanaHistoryPaymentAgencyProcessor kanaHistoryPaymentAgencyProcessor,
            ILogManager logManager
            )
        {
            this.authorizationProcessor = authorizationProcessor;
            this.kanaHistoryPaymentAgencyProcessor = kanaHistoryPaymentAgencyProcessor;
            logger = logManager.GetLogger(typeof(KanaHistoryPaymentAgencyMaster));
        }

        public async Task<KanaHistoryPaymentAgencysResult> GetListAsync(string SessionKey, int CompanyId, string PayerName, string PaymentAgencyCodeFrom, string PaymentAgencyCodeTo)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = (await kanaHistoryPaymentAgencyProcessor.GetAsync(new KanaHistorySearch {
                    CompanyId   = CompanyId,
                    PayerName   = PayerName,
                    CodeFrom    = PaymentAgencyCodeFrom,
                    CodeTo      = PaymentAgencyCodeTo,
                }, token)).ToList();
                return new KanaHistoryPaymentAgencysResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    KanaHistoryPaymentAgencys = result,
                };
            }, logger);
        }

        public async Task<CountResult> DeleteAsync(string SessionKey, int CompanyId, string PayerName, string SourceBankName, string SourceBranchName, int PaymentAgencyId)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = await kanaHistoryPaymentAgencyProcessor.DeleteAsync(new KanaHistoryPaymentAgency {
                    CompanyId           = CompanyId,
                    PayerName           = PayerName,
                    SourceBankName      = SourceBankName,
                    SourceBranchName    = SourceBranchName,
                    PaymentAgencyId     = PaymentAgencyId,
                }, token);
                return new CountResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    Count = result,
                };
            }, logger);
        }

        public async Task<ImportResult> ImportAsync(string SessionKey,
            KanaHistoryPaymentAgency[] InsertList, KanaHistoryPaymentAgency[] UpdateList, KanaHistoryPaymentAgency[] DeleteList)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = await kanaHistoryPaymentAgencyProcessor.ImportAsync(InsertList, UpdateList, DeleteList, token);
                return result;
            }, logger);
        }

    }
}
