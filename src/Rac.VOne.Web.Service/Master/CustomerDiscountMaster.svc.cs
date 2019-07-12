using System.Collections.Generic;
using Rac.VOne.Common.Logging;
using Rac.VOne.Web.Common;
using Rac.VOne.Web.Models;
using Rac.VOne.Web.Service.Extensions;
using NLog;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Service.Master
{
    // メモ: [リファクター] メニューの [名前の変更] コマンドを使用すると、コード、svc、および config ファイルで同時にクラス名 "CustomerDiscount" を変更できます。
    // 注意: このサービスをテストするために WCF テスト クライアントを起動するには、ソリューション エクスプローラーで CustomerDiscount.svc または CustomerDiscount.svc.cs を選択し、デバッグを開始してください。
    public class CustomerDiscountMaster : ICustomerDiscountMaster
    {
        private readonly IAuthorizationProcessor authorizationProcessor;
        private readonly ICustomerDiscountProcessor customerDiscountProcessor;
        private readonly ILogger logger;

        public CustomerDiscountMaster(
            IAuthorizationProcessor authorizationProcessor,
            ICustomerDiscountProcessor customerDiscountProcessor,
            ILogManager logManager
            )
        {
            this.authorizationProcessor = authorizationProcessor;
            this.customerDiscountProcessor = customerDiscountProcessor;
            logger = logManager.GetLogger(typeof(CustomerDiscountMaster));
        }

        public async Task<ExistResult> ExistAccountTitleAsync(string sessionKey, int accountTitleid)
        {
            return await authorizationProcessor.DoAuthorizeAsync(sessionKey, async token =>
            {
                var result = await customerDiscountProcessor.ExistAccountTitleAsync(accountTitleid, token);
                return new ExistResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    Exist = result,
                };
            }, logger);
        }

        public async Task<ImportResult> ImportAsync(string sessionKey,
                CustomerDiscount[] insertList, CustomerDiscount[] updateList, CustomerDiscount[] deleteList)
        {
            return await authorizationProcessor.DoAuthorizeAsync(sessionKey, async token =>
            {
                var result = await customerDiscountProcessor.ImportAsync(
                    insertList, updateList, deleteList, token);
                return result;
            }, logger);
        }
    }
}
