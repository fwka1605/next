using Rac.VOne.Web.Common;
using Rac.VOne.Web.Models;
using Rac.VOne.Web.Service.Extensions;
using System.Linq;
using Rac.VOne.Common.Logging;
using NLog;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Service
{
    public class ReceiptHeaderService : IReceiptHeaderService
    {
        private readonly IAuthorizationProcessor authorizationProcessor;
        private readonly IReceiptHeaderProcessor receiptHeaderProcessor;
        private readonly ILogger logger;

        public ReceiptHeaderService(
            IAuthorizationProcessor authorizationProcessor,
            IReceiptHeaderProcessor receiptHeaderProcessor,
            ILogManager logManager)
        {
            this.authorizationProcessor = authorizationProcessor;
            this.receiptHeaderProcessor = receiptHeaderProcessor;
            logger = logManager.GetLogger(typeof(ReceiptHeaderService));
        }

        public async Task<ReceiptHeadersResult> GetAsync(string SessionKey, long[] Ids)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = (await receiptHeaderProcessor.GetByIdsAsync(Ids, token)).ToList();
                return new ReceiptHeadersResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    ReceiptHeaders = result,
                };
            }, logger);
        }

        public async Task<ReceiptHeaderResult> UpdateReceiptHeaderAsync(string Sessionkey, int ComapnyId, int LoginId)
        {
            return await authorizationProcessor.DoAuthorizeAsync(Sessionkey, async token =>
            {
                await receiptHeaderProcessor.UpdateAsync(new ReceiptHeaderUpdateOption {
                    CompanyId = ComapnyId,
                    UpdateBy = LoginId,
                }, token);
                return new ReceiptHeaderResult
                {
                    ProcessResult = new ProcessResult { Result = false },
                };
            }, logger);
        }

    }
}
