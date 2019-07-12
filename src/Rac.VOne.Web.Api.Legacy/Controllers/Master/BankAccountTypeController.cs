using System.Web.Http;
using Rac.VOne.Web.Common;
using Rac.VOne.Web.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Api.Legacy.Controllers
{
    /// <summary>
    ///  預金種別用
    /// </summary>
    public class BankAccountTypeController : ApiControllerAuthorized
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
        public async Task<IEnumerable<BankAccountType>> GetItems()
            => (await bankAccountTypeProcessor.GetAsync()).ToArray();

    }
}
