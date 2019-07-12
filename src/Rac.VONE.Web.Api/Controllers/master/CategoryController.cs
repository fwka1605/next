using Microsoft.AspNetCore.Mvc;
using Rac.VOne.Web.Common;
using Rac.VOne.Web.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Api.Controllers
{
    /// <summary>
    ///  区分マスター
    /// </summary>
    [TypeFilter(typeof(Filters.AuthorizationFilter))]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryProcessor categoryProcessor;

        /// <summary>constructor</summary>
        public CategoryController(
            ICategoryProcessor categoryProcessor
            )
        {
            this.categoryProcessor = categoryProcessor;
        }

        /// <summary>区分マスター 登録</summary>
        /// <param name="category"></param>
        /// <param name="token">自動バインド</param>
        [HttpPost]
        public async Task<ActionResult<Category>> Save(Category category, CancellationToken token)
            => await categoryProcessor.SaveAsync(category, token);

        /// <summary>区分マスター 削除</summary>
        /// <param name="category"> Category.Id を指定</param>
        /// <param name="token">自動バインド</param>
        [HttpPost]
        public async Task<ActionResult<int>> Delete(Category category, CancellationToken token)
            => await categoryProcessor.DeleteAsync(category.Id, token);

        /// <summary>区分マスター取得</summary>
        /// <param name="option">検索条件</param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<IEnumerable<Category>>> GetItems(CategorySearch option, CancellationToken token)
            => (await categoryProcessor.GetAsync(option, token)).ToArray();


        /// <summary>区分マスター 科目登録確認</summary>
        /// <param name="acccountTitleId">科目ID</param>
        /// <param name="token">自動バインド</param>
        [HttpPost]
        public async Task<ActionResult<bool>> ExistAccountTitle([FromBody] int acccountTitleId, CancellationToken token)
            => await categoryProcessor.ExistAccountTitleAsync(acccountTitleId, token);

        /// <summary>区分マスター 決済代行会社登録確認</summary>
        /// <param name="paymentAgencyId">決済代行会社ID</param>
        /// <param name="token">自動バインド</param>
        [HttpPost]
        public async Task<ActionResult<bool>> ExistPaymentAgency([FromBody] int paymentAgencyId, CancellationToken token)
            => await categoryProcessor.ExistPaymentAgencyAsync(paymentAgencyId, token);
    }
}
