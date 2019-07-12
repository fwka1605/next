using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Rac.VOne.Web.Common;
using Rac.VOne.Web.Models;

namespace Rac.VOne.Web.Api.Controllers
{
    /// <summary>
    /// ���j���[�����}�X�^�[
    /// </summary>
    [TypeFilter(typeof(Filters.AuthorizationFilter))]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class MenuAuthorityController : ControllerBase
    {
        private readonly IMenuAuthorityProcessor menuAuthorityProcessor;

        /// <summary>
        /// constructor
        /// </summary>
        public MenuAuthorityController(
            IMenuAuthorityProcessor menuAuthorityProcessor
            )
        {
            this.menuAuthorityProcessor = menuAuthorityProcessor;
        }

        /// <summary>
        /// �폜
        /// </summary>
        /// <param name="option">���ID ���w��</param>
        /// <param name="token">�����o�C���h</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<int>> Delete(MenuAuthoritySearch option, CancellationToken token)
            => await menuAuthorityProcessor.DeleteAsync(option.CompanyId.Value, token);

        /// <summary>
        /// �擾 �z��
        /// </summary>
        /// <param name="option">���ID, ���O�C�����[�U�[ID ���w��</param>
        /// <param name="token">�����o�C���h</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<IEnumerable<MenuAuthority>>> GetItems(MenuAuthoritySearch option, CancellationToken token)
            => (await menuAuthorityProcessor.GetAsync(option, token)).ToArray();


        /// <summary>
        /// �o�^
        /// </summary>
        /// <param name="menus"></param>
        /// <param name="token">�����o�C���h</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<IEnumerable<MenuAuthority>>> Save(IEnumerable<MenuAuthority> menus, CancellationToken token)
            => (await menuAuthorityProcessor.SaveAsync(menus, token)).ToArray();

    }
}
