﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Rac.VOne.Web.Common;
using Rac.VOne.Web.Models;
using Token = System.Threading.CancellationToken;

namespace Rac.VOne.Web.Api.Controllers
{
    /// <summary>
    /// Company Master操作用コントロール
    /// </summary>
    [TypeFilter(typeof(Filters.AuthorizationFilter))]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ReceiptExcludeController : ControllerBase
    {
        private readonly IReceiptExcludeProcessor receiptexcludeProcessor;

        /// <summary>
        /// constructor
        /// </summary>
        public ReceiptExcludeController(
            IReceiptExcludeProcessor receiptexcludeProcessor
            )
        {
            this.receiptexcludeProcessor = receiptexcludeProcessor;
        }

        /// <summary>
        /// 区分ID が登録されているか確認
        /// </summary>
        /// <param name="categoryId"></param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<bool>> ExistExcludeCategory([FromBody] int categoryId, Token token)
            => await receiptexcludeProcessor.ExistExcludeCategoryAsync(categoryId, token);


        /// <summary>
        /// 入金ID を指定して取得
        /// </summary>
        /// <param name="receiptId"></param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<IEnumerable<ReceiptExclude>>> GetByReceiptId([FromBody] long receiptId, Token token)
            => (await receiptexcludeProcessor.GetByReceiptIdAsync(receiptId, token)).ToArray();


        /// <summary>
        /// 取得 配列
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<IEnumerable<ReceiptExclude>>> GetByIdsAsync([FromBody] IEnumerable<long> ids, Token token)
            => (await receiptexcludeProcessor.GetByIdsAsync(ids, token)).ToArray();
    }
}