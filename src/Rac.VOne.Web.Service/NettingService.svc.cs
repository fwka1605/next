using System.Collections.Generic;
using System.Linq;
using Rac.VOne.Web.Common;
using Rac.VOne.Web.Models;
using Rac.VOne.Web.Service.Extensions;
using Rac.VOne.Common.Logging;
using NLog;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Service
{
    // メモ: [リファクター] メニューの [名前の変更] コマンドを使用すると、コード、svc、および config ファイルで同時にクラス名 "NettingService" を変更できます。
    // 注意: このサービスをテストするために WCF テスト クライアントを起動するには、ソリューション エクスプローラーで NettingService.svc または NettingService.svc.cs を選択し、デバッグを開始してください。
    public class NettingService : INettingService
    {
        private readonly IAuthorizationProcessor authorizationProcessor;
        private readonly INettingProcessor nettingProcessor;
        private readonly INettingSearchProcessor nettingSearchProcessor;
        private readonly ILogger logger;

        public NettingService(
            IAuthorizationProcessor authorizationProcessor,
            INettingProcessor nettingProcessor,
            INettingSearchProcessor nettingSearchProcessor,
            ILogManager logManager
            )
        {
            this.authorizationProcessor = authorizationProcessor;
            this.nettingProcessor = nettingProcessor;
            this.nettingSearchProcessor = nettingSearchProcessor;
            logger = logManager.GetLogger(typeof(NettingService));
        }

        public async Task<ExistResult> ExistReceiptCategoryAsync(string SessionKey, int CategoryId)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = await nettingProcessor.ExistReceiptCategoryAsync(CategoryId, token);
                return new ExistResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    Exist = result,
                };
            }, logger);
        }

        public async Task<ExistResult> ExistCustomerAsync(string SessionKey, int CustomerId)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = await nettingProcessor.ExistCustomerAsync(CustomerId, token);

                return new ExistResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    Exist = result,
                };
            }, logger);
        }

        public async Task<NettingsResult> GetItemsAsync(string SessionKey, int CompanyId, int CustomerId, int CurrencyId)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = (await nettingSearchProcessor.GetAsync(CompanyId, CustomerId, CurrencyId, token)).ToList();

                return new NettingsResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    Nettings = result,
                };
            }, logger);
        }

        public async Task<NettingResult> SaveAsync(string SessionKey, Netting[] Netting)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = (await nettingProcessor.SaveAsync(Netting, token)).ToArray();
                return new NettingResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    Netting = result,
                };
            }, logger);
        }


        public async Task<NettingResult> DeleteAsync(string SessionKey, long[] NettingId)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var rseult = await nettingProcessor.DeleteAsync(NettingId, token);
                return new NettingResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                };
            }, logger);
        }
        public async Task<ExistResult> ExistSectionAsync(string SectionKey, int SectionId)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SectionKey, async token =>
            {
                var result = await nettingProcessor.ExistSectionAsync(SectionId, token);
                return new ExistResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    Exist = result,
                };
            }, logger);
        }

        public async Task<ExistResult> ExistCurrencyAsync(string SessionKey, int CurrencyId)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = await nettingProcessor.ExistCurrencyAsync(CurrencyId, token);
                return new ExistResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    Exist = result,
                };
            }, logger);
        }

    }
}
