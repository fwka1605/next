using NLog;
using Rac.VOne.Common.Logging;
using Rac.VOne.Web.Common;
using Rac.VOne.Web.Models;
using Rac.VOne.Web.Service.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Service
{
    public class ReceiptMemoService : IReceiptMemoService
    {
        private readonly IAuthorizationProcessor authorizationProcessor;
        private readonly IReceiptMemoProcessor receiptMemoProcessor;
        private readonly ILogger logger;

        public ReceiptMemoService(
            IAuthorizationProcessor authorizationProcessor,
            IReceiptMemoProcessor receiptMemoProcessor,
            ILogManager logManager)
        {
            this.authorizationProcessor = authorizationProcessor;
            this.receiptMemoProcessor = receiptMemoProcessor;
            logger = logManager.GetLogger(typeof(ReceiptMemoService));
        }

        public async Task<ReceiptMemosResult> GetItemsAsync(string SessionKey, long[] receiptIds)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = (await receiptMemoProcessor.GetItemsAsync(receiptIds, token)).ToArray();
                return new ReceiptMemosResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    ReceiptMemo = result,
                };
            }, logger);
        }
    }
}
