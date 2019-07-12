using System;
using System.Collections.Generic;
using System.Linq;
using Rac.VOne.Common.Logging;
using Rac.VOne.Web.Common;
using Rac.VOne.Web.Models;
using Rac.VOne.Web.Service.Extensions;
using NLog;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Service.Master
{
    public class IgnoreKanaMaster : IIgnoreKanaMaster
    {
        private readonly IAuthorizationProcessor authorizationProcessor;
        private readonly IIgnoreKanaProcessor ignoreKanaProcessor;
        private readonly ILogger logger;

        public IgnoreKanaMaster(
            IAuthorizationProcessor authorizationProcessor,
            IIgnoreKanaProcessor ignoreKanaProcessor,
            ILogManager logManager
            )
        {
            this.authorizationProcessor = authorizationProcessor;
            this.ignoreKanaProcessor = ignoreKanaProcessor;
            logger = logManager.GetLogger(typeof(IgnoreKanaMaster));
        }

        public async Task<IgnoreKanasResult> GetItemsAsync(string sessionKey, int companyId)
        {
            return await authorizationProcessor.DoAuthorizeAsync(sessionKey, async token =>
            {
                var result = (await ignoreKanaProcessor.GetAsync(new IgnoreKana { CompanyId = companyId }, token)).ToList();

                return new IgnoreKanasResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    IgnoreKanas = result,
                };
            }, logger);
        }

        public async Task<IgnoreKanaResult> SaveAsync(string sessionKey, IgnoreKana kana)
        {
            return await authorizationProcessor.DoAuthorizeAsync(sessionKey, async token =>
            {
                var result = await ignoreKanaProcessor.SaveAsync(kana, token);
                return new IgnoreKanaResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    IgnoreKana = result,
                };
            }, logger);
        }

        public async Task<CountResult> DeleteAsync(string sessionKey,
            int companyId, string kana)
        {
            return await authorizationProcessor.DoAuthorizeAsync(sessionKey, async token =>
            {
                var result = await ignoreKanaProcessor.DeleteAsync(new IgnoreKana {
                    CompanyId   = companyId,
                    Kana        = kana,
                }, token);
                return new CountResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    Count = result,
                };
            }, logger);
        }

        public async Task<ImportResult> ImportAsync(string sessionKey,
            IgnoreKana[] insertList, IgnoreKana[] updateList, IgnoreKana[] deleteList)
        {
            return await authorizationProcessor.DoAuthorizeAsync(sessionKey, async token =>
            {
                var result = await ignoreKanaProcessor.ImportAsync(insertList, updateList, deleteList, token);
                return result;
            }, logger);
        }

        public async Task<ExistResult> ExistCategoryAsync(string sessionKey, int id)
        {
            return await authorizationProcessor.DoAuthorizeAsync(sessionKey, async token =>
            {
                var result = await ignoreKanaProcessor.ExistCategoryAsync(id, token);

                return new ExistResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    Exist = result,
                };
            }, logger);
        }

        public async Task<IgnoreKanaResult> GetAsync(string sessionKey, int companyId, string kana)
        {
            return await authorizationProcessor.DoAuthorizeAsync(sessionKey, async token =>
            {
                var result = (await ignoreKanaProcessor.GetAsync(new IgnoreKana {
                    CompanyId = companyId,
                    Kana = kana,
                }, token)).FirstOrDefault();

                return new IgnoreKanaResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    IgnoreKana = result,
                };
            }, logger);
        }

    }
}
