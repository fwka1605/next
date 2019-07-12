﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using Rac.VOne.Web.Common;
using Rac.VOne.Web.Common.Importers;
using Rac.VOne.Web.Models;

namespace Rac.VOne.Web.Api.Legacy.Controllers
{
    /// <summary>
    ///  EB取込履歴
    /// </summary>
    public class ImportFileLogController : ApiControllerAuthorized
    {
        private readonly IImportFileLogProcessor importFileLogProcessor;
        private readonly IEbFileImportProcessor ebFileImportProcessor;

        /// <summary>
        /// constructor
        /// </summary>
        public ImportFileLogController(
            IImportFileLogProcessor importFileLogProcessor,
            IEbFileImportProcessor ebFileImportProcessor
            )
        {
            this.importFileLogProcessor = importFileLogProcessor;
            this.ebFileImportProcessor = ebFileImportProcessor;
        }

        /// <summary>
        /// 取得
        /// </summary>
        /// <param name="comapnyId"></param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IEnumerable<ImportFileLog>> GetHistory([FromBody] int comapnyId, CancellationToken token)
            => (await importFileLogProcessor.GetHistoryAsync(comapnyId, token)).ToArray();

        /// <summary>
        /// 登録 廃止予定
        /// </summary>
        /// <param name="token">自動バインド</param>
        /// <param name="logs"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IEnumerable<ImportFileLog>> Save(IEnumerable<ImportFileLog> logs, CancellationToken token)
            => (await importFileLogProcessor.SaveAsync(logs, token)).ToArray();

        /// <summary>
        /// 削除
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<int> DeleteItems([FromBody] IEnumerable<int> ids, CancellationToken token)
            => await importFileLogProcessor.DeleteAsync(ids, token);

        /// <summary>EBファイルインポート処理</summary>
        /// <param name="files"></param>
        /// <param name="token"></param>
        /// <returns></returns>

        [HttpPost]
        public async Task<IEnumerable<EbFileInformation>> Import(IEnumerable<EbFileInformation> files, CancellationToken token)
            => await ebFileImportProcessor.ImportAsync(files, token);
    }
}
