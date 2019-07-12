using Microsoft.AspNetCore.Mvc;
using Rac.VOne.Web.Common;
using Rac.VOne.Web.Models;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace Rac.VOne.Web.Api.Controllers
{
    /// <summary>
    ///  得意先 約定マスター
    /// </summary>
    [TypeFilter(typeof(Filters.AuthorizationFilter))]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CustomerPaymentContractController : ControllerBase
    {
        private readonly ICustomerPaymentContractProcessor customerPaymentContractProcessor;

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="customerPaymentContractProcessor"></param>
        public CustomerPaymentContractController(
            ICustomerPaymentContractProcessor customerPaymentContractProcessor
            )
        {
            this.customerPaymentContractProcessor = customerPaymentContractProcessor;
        }

        /// <summary>
        /// 約定マスターに 指定した区分ID が登録されているか確認
        /// </summary>
        /// <param name="categoryId">区分ID</param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<bool>> ExistCategory([FromBody] int categoryId, CancellationToken token)
            => await customerPaymentContractProcessor.ExistCategoryAsync(categoryId, token);


        /// <summary>
        /// 得意先回収条件 約定情報の取得
        /// </summary>
        /// <param name="customerIds">得意先ID の 配列</param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<IEnumerable<CustomerPaymentContract>>> GetItems([FromBody] IEnumerable<int> customerIds, CancellationToken token)
            => (await customerPaymentContractProcessor.GetAsync(customerIds, token)).ToArray();

        /// <summary>
        /// 得意先回収条件 約定情報の登録
        /// </summary>
        /// <param name="contract">約定情報</param>
        /// <param name="token">自動バインド</param>
        [HttpPost]
        public async Task<ActionResult<CustomerPaymentContract>> Save(CustomerPaymentContract contract, CancellationToken token)
            => await customerPaymentContractProcessor.SaveAsync(contract, token);

        /// <summary>
        /// 得意先回収条件 約定情報の削除
        /// </summary>
        /// <param name="customerId">得意先ID</param>
        /// <param name="token">自動バインド</param>
        [HttpPost]
        public async Task<ActionResult<int>> Delete([FromBody] int customerId, CancellationToken token)
            => await customerPaymentContractProcessor.DeleteAsync(customerId, token);


    }
}
