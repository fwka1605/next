using Microsoft.AspNetCore.Mvc;
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
    ///  得意先手数料マスター
    /// </summary>
    [TypeFilter(typeof(Filters.AuthorizationFilter))]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CustomerFeeController : ControllerBase
    {

        private readonly ICustomerFeeProcessor customerFeeProcessor;
        private readonly ICustomerFeeFileImportProcessor customerFeeImportProcessor;

        /// <summary>
        /// constructor
        /// </summary>
        public CustomerFeeController(
            ICustomerFeeProcessor customerFeeProcessor,
            ICustomerFeeFileImportProcessor customerFeeImportProcessor
            )
        {
            this.customerFeeProcessor = customerFeeProcessor;
            this.customerFeeImportProcessor = customerFeeImportProcessor;
        }


        /// <summary>
        /// 配列 取得
        /// </summary>
        /// <param name="option">取得したい 得意先ID、通貨ID を設定した 手数料</param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<IEnumerable<CustomerFee>>> Get(CustomerFeeSearch option, CancellationToken token)
            => (await customerFeeProcessor.GetAsync(option, token)).ToArray();

        /// <summary>
        /// 登録
        /// </summary>
        /// <param name="customerFees"></param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<IEnumerable<CustomerFee>>> Save(IEnumerable<CustomerFee> customerFees, CancellationToken token = default(CancellationToken))
            => (await customerFeeProcessor.SaveAsync(customerFees, token)).ToArray();

        /// <summary>
        /// インポート処理
        /// </summary>
        /// <param name="source"></param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<ImportResult>> Import(MasterImportSource source, CancellationToken token = default(CancellationToken))
            => await customerFeeImportProcessor.ImportAsync(source, token);

    }
}
