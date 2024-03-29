﻿using Microsoft.AspNetCore.Mvc;
using Rac.VOne.Web.Common;
using Rac.VOne.Web.Common.Importers;
using Rac.VOne.Web.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Api.Controllers
{
    /// <summary>
    ///  得意先歩引きマスター
    /// </summary>
    [TypeFilter(typeof(Filters.AuthorizationFilter))]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CustomerDiscountController : ControllerBase
    {

        private readonly ICustomerDiscountProcessor customerDiscountProcessor;
        private readonly ICustomerDiscountFileImportProcessor customerDiscountImportProcessor;

        /// <summary>
        /// constructor
        /// </summary>
        public CustomerDiscountController(
            ICustomerDiscountProcessor customerDiscountProcessor,
            ICustomerDiscountFileImportProcessor customerDiscountImportProcessor
            )
        {
            this.customerDiscountProcessor = customerDiscountProcessor;
            this.customerDiscountImportProcessor = customerDiscountImportProcessor;
        }

        /// <summary>
        /// 科目ID が登録されているか確認する処理
        /// </summary>
        /// <param name="accountTitleid">科目ID</param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<bool>> ExistAccountTitle([FromBody] int accountTitleid, CancellationToken token)
            => await customerDiscountProcessor.ExistAccountTitleAsync(accountTitleid, token);

        /// <summary>
        /// 取得 配列
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<IEnumerable<CustomerDiscount>>> Get([FromBody] int customerId, CancellationToken token)
            => (await customerDiscountProcessor.GetAsync(customerId, token)).ToArray();

        /// <summary>
        /// インポート
        /// </summary>
        /// <param name="source"></param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<ImportResult>> Import(MasterImportSource source, CancellationToken token)
            => await customerDiscountImportProcessor.ImportAsync(source, token);

    }
}
