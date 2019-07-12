using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using Rac.VOne.Web.Common;
using Rac.VOne.Web.Models;

namespace Rac.VOne.Web.Api.Legacy.Controllers
{
    /// <summary>
    /// パスワードポリシーマスター
    /// </summary>
    public class PasswordPolicyController : ApiControllerAuthorized
    {
        private readonly IPasswordPolicyProcessor passwordPolicyProcessor;

        /// <summary>
        /// constructor
        /// </summary>
        public PasswordPolicyController(
            IPasswordPolicyProcessor passwordPolicyProcessor
            )
        {
            this.passwordPolicyProcessor = passwordPolicyProcessor;
        }

        /// <summary>
        /// 取得
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task< PasswordPolicy > Get([FromBody] int companyId, CancellationToken token)
            => await passwordPolicyProcessor.GetAsync(companyId, token);

        /// <summary>
        /// 登録
        /// </summary>
        /// <param name="policy"></param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<PasswordPolicy> Save(PasswordPolicy policy, CancellationToken token)
            => await passwordPolicyProcessor.SaveAsync(policy, token);
    }
}
