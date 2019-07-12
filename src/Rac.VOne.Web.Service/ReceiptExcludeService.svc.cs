using System.Collections.Generic;
using System.Linq;
using Rac.VOne.Web.Common;
using Rac.VOne.Web.Models;
using Rac.VOne.Web.Service.Extensions;
using Rac.VOne.Common.Logging;
using NLog;
using System;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Service
{
    public class ReceiptExcludeService : IReceiptExcludeService
    {
        private readonly IAuthorizationProcessor authorizationProcessor;
        private readonly IReceiptExcludeProcessor receiptexcludeProcessor;
        private readonly ILogger logger;

        public ReceiptExcludeService(
            IAuthorizationProcessor authorizationProcessor,
            IReceiptExcludeProcessor receiptexcludeProcessor,
            ILogManager logManager)
        {
            this.authorizationProcessor = authorizationProcessor;
            this.receiptexcludeProcessor = receiptexcludeProcessor;
            logger = logManager.GetLogger(typeof(ReceiptExcludeService));
        }

        public async Task<ExistResult> ExistExcludeCategoryAsync(string SessionKey, int CategoryId)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = await receiptexcludeProcessor.ExistExcludeCategoryAsync(CategoryId, token);
                return new ExistResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    Exist = result,
                };
            }, logger);
        }

        public async Task<ReceiptExcludesResult> GetByReceiptIdAsync(string SessionKey, long ReceiptId)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = (await receiptexcludeProcessor.GetByReceiptIdAsync(ReceiptId, token)).ToList();
                return new ReceiptExcludesResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    ReceiptExcludes = result,
                };
            }, logger);
        }

        public async Task<ReceiptExcludesResult> GetByIdsAsync(string SessionKey, long[] ids)
            => await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = await receiptexcludeProcessor.GetByIdsAsync(ids, token);
                return new ReceiptExcludesResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    ReceiptExcludes = result.ToList(),
                };
            }, logger);
    }
}
