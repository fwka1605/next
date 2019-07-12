using Microsoft.AspNetCore.Mvc;
using NLog;
using Rac.VOne.Common.Logging;
using Rac.VOne.Web.Common;
using Rac.VOne.Web.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Api.Controllers
{
    /// <summary>
    ///  預金種別用
    /// </summary>
    [TypeFilter(typeof(Filters.AuthorizationFilter))]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class BankAccountTypeController : ControllerBase
    {
        private readonly IBankAccountTypeProcessor bankAccountTypeProcessor;

        /// <summary></summary>
        public BankAccountTypeController(
            IBankAccountTypeProcessor bankAccountTypeProcessor
            )
        {
            this.bankAccountTypeProcessor = bankAccountTypeProcessor;
        }

        /// <summary>預金種別一覧取得</summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<IEnumerable<BankAccountType>>> GetItems()
            => (await bankAccountTypeProcessor.GetAsync()).ToArray();

    }
}
