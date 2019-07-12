using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using Rac.VOne.Common.Logging;
using Rac.VOne.Web.Common;
using Rac.VOne.Web.Common.Importers;
using Rac.VOne.Web.Models;

namespace Rac.VOne.Web.Api.Legacy.Controllers
{
    /// <summary>
    ///  決済代行会社カナ学習履歴
    /// </summary>
    public class KanaHistoryPaymentAgencyController : ApiControllerAuthorized
    {
        private readonly IKanaHistoryPaymentAgencyProcessor kanaHistoryPaymentAgencyProcessor;
        private readonly IKanaHistoryPaymentAgencyFileImportProcessor kanaHistoryPaymentAgencyImportProcessor;

        /// <summary>
        /// constructor
        /// </summary>
        public KanaHistoryPaymentAgencyController(
            IKanaHistoryPaymentAgencyProcessor kanaHistoryPaymentAgencyProcessor,
            IKanaHistoryPaymentAgencyFileImportProcessor kanaHistoryPaymentAgencyImportProcessor
            )
        {
            this.kanaHistoryPaymentAgencyProcessor = kanaHistoryPaymentAgencyProcessor;
            this.kanaHistoryPaymentAgencyImportProcessor = kanaHistoryPaymentAgencyImportProcessor;
        }

        /// <summary>
        /// 取得 配列
        /// </summary>
        /// <param name="option">検索条件</param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IEnumerable< KanaHistoryPaymentAgency>> GetItems(KanaHistorySearch option, CancellationToken token)
            => (await kanaHistoryPaymentAgencyProcessor.GetAsync(option, token)).ToArray();

        /// <summary>
        /// 削除
        /// </summary>
        /// <param name="history"></param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<int> Delete(KanaHistoryPaymentAgency history, CancellationToken token)
            => await kanaHistoryPaymentAgencyProcessor.DeleteAsync(history, token);

        /// <summary>
        /// インポート
        /// </summary>
        /// <param name="source"></param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ImportResult> Import(MasterImportSource source, CancellationToken token)
            => await kanaHistoryPaymentAgencyImportProcessor.ImportAsync(source, token);

    }
}
