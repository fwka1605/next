using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Rac.VOne.Web.Models;

namespace Rac.VOne.Web.Api.Controllers
{
    /// <summary>
    ///  �����O��_��}�X�^�[
    /// </summary>
    [TypeFilter(typeof(Filters.AuthorizationFilter))]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class LongTermAdvanceReceivedContractController : ControllerBase
    {
        /// <summary>
        /// constructor
        /// </summary>
        public LongTermAdvanceReceivedContractController(
            )
        {

        }

        /// <summary>
        /// �폜
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult<int> Delete(int id)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// �擾
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult<LongTermAdvanceReceivedContract> Get(int id)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// �擾
        /// </summary>
        /// <param name="id"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult<LongTermAdvanceReceivedContract> GetByCode(int id, string code)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// �擾
        /// </summary>
        /// <param name="companyId"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult<IEnumerable<LongTermAdvanceReceivedContract>> GetItems(int companyId)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// �o�^
        /// </summary>
        /// <param name="contract"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult<LongTermAdvanceReceivedContract> Save(LongTermAdvanceReceivedContract contract)
        {
            throw new NotImplementedException();
        }
    }
}
