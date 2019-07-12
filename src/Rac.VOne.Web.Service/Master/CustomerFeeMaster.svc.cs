using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Rac.VOne.Common.Logging;
using Rac.VOne.Data;
using Rac.VOne.Web.Common;
using Rac.VOne.Web.Models;
using Rac.VOne.Web.Service.Extensions;
using Rac.VOne.Data.QueryProcessors;
using NLog;
using static Rac.VOne.Common.Constants;

namespace Rac.VOne.Web.Service.Master
{
    // メモ: [リファクター] メニューの [名前の変更] コマンドを使用すると、コード、svc、および config ファイルで同時にクラス名 "CustomerFeeMaster" を変更できます。
    // 注意: このサービスをテストするために WCF テスト クライアントを起動するには、ソリューション エクスプローラーで CustomerFeeMaster.svc または CustomerFeeMaster.svc.cs を選択し、デバッグを開始してください。
    public class CustomerFeeMaster : ICustomerFeeMaster
    {
        private readonly IAuthorizationProcessor authorizationProcessor;
        private readonly ICustomerFeeProcessor customerFeeProcessor;
        private readonly ILogger logger;

        public CustomerFeeMaster(
            IAuthorizationProcessor authorizationProcessor,
            ICustomerFeeProcessor customerFeeProcessor,
            ILogManager logManager
            )
        {
            this.authorizationProcessor = authorizationProcessor;
            this.customerFeeProcessor = customerFeeProcessor;
            logger = logManager.GetLogger(typeof(CustomerFeeMaster));
        }

        public async Task<CustomerFeeResult> GetAsync(string SessionKey, int CustomerId, int CurrencyId)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = (await customerFeeProcessor.GetAsync(new CustomerFeeSearch {
                    CustomerId  = CustomerId,
                    CurrencyId  = CurrencyId,
                }, token)).ToList();
                return new CustomerFeeResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    CustomerFee = result.ToArray(),
                };
            }, logger);
        }

        public async Task<CustomerFeeResult> SaveAsync(string SessionKey,int CustomerId, int CurrencyId, CustomerFee[] CustomerFees)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = (await customerFeeProcessor.SaveAsync(CustomerFees, token)).ToArray();
                return new CustomerFeeResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    CustomerFee = result.ToArray(),
                };
            }, logger);
        }

        public async Task<CustomerFeesResult> GetForPrintAsync(string SessionKey, int CompanyId)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = (await customerFeeProcessor.GetAsync(new CustomerFeeSearch {
                    CompanyId   = CompanyId,
                    ForPrint    = true,
                }, token)).ToList();
                return new CustomerFeesResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    CustomerFeePrint = result.ToArray(),
                };
            }, logger);
        }

        public async Task<CustomerFeeResult> GetForExportAsync(string SessionKey, int CompanyId)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = (await customerFeeProcessor.GetAsync(new CustomerFeeSearch {
                    CompanyId = CompanyId,
                }, token)).ToList();
                return new CustomerFeeResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    CustomerFee = result.ToArray(),
                };
            }, logger);
        }

        public async Task<ImportResult> ImportAsync(string SessionKey, CustomerFee[] insertList, CustomerFee[] updateList, CustomerFee[] deleteList)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = await customerFeeProcessor.ImportAsync(insertList, updateList, deleteList, token);
                return result;
            }, logger);
        }
    }
}
