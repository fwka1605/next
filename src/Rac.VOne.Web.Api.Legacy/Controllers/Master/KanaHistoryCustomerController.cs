﻿using Rac.VOne.Web.Common;
using Rac.VOne.Web.Common.Importers;
using Rac.VOne.Web.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

namespace Rac.VOne.Web.Api.Legacy.Controllers
{
    /// <summary>
    ///  得意先カナ学習履歴
    /// </summary>
    public class KanaHistoryCustomerController : ApiControllerAuthorized
    {
        private readonly IKanaHistoryCustomerProcessor kanaHistoryCustomerProcessor;
        private readonly IKanaHistoryCustomerFileImportProcessor kanaHistoryCustomerImportProcessor;

        /// <summary>
        /// constructor
        /// </summary>
        public KanaHistoryCustomerController(
            IKanaHistoryCustomerProcessor kanaHistoryCustomerProcessor,
            IKanaHistoryCustomerFileImportProcessor kanaHistoryCustomerImportProcessor
            )
        {
            this.kanaHistoryCustomerProcessor = kanaHistoryCustomerProcessor;
            this.kanaHistoryCustomerImportProcessor = kanaHistoryCustomerImportProcessor;
        }

        /// <summary>
        /// 得意先ID が登録されているか確認
        /// </summary>
        /// <param name="history"><see cref="KanaHistoryCustomer.CustomerId"/>得意先ID のみ設定</param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<bool> ExistCustomer(KanaHistoryCustomer history, CancellationToken token)
            => await kanaHistoryCustomerProcessor.ExistCustomerAsync(history.CustomerId, token);

        /// <summary>
        /// 取得 配列
        /// </summary>
        /// <param name="option">検索条件</param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IEnumerable<KanaHistoryCustomer>> GetItems(KanaHistorySearch option, CancellationToken token)
            => (await kanaHistoryCustomerProcessor.GetAsync(option, token)).ToArray();

        /// <summary>
        /// 削除
        /// </summary>
        /// <param name="history">キー項目をすべて設定</param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<int> Delete(KanaHistoryCustomer history, CancellationToken token)
            => await kanaHistoryCustomerProcessor.DeleteAsync(history, token);

        /// <summary>
        /// インポート
        /// </summary>
        /// <param name="source"></param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ImportResult> Import(MasterImportSource source, CancellationToken token)
            => await kanaHistoryCustomerImportProcessor.ImportAsync(source, token);

    }
}
