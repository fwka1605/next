using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using Rac.VOne.Web.Models;

namespace Rac.VOne.Web.Api.Legacy.Controllers
{
    /// <summary>
    ///  �����O��_��}�X�^�[
    /// </summary>
    public class LongTermAdvanceReceivedContractController : ApiControllerAuthorized
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
        public Task<int> Delete([FromBody] int id)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// �擾
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public Task<LongTermAdvanceReceivedContract> Get([FromBody] int id)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// �擾
        /// </summary>
        /// <param name="option"></param>
        /// <returns></returns>
        [HttpPost]
        public Task<LongTermAdvanceReceivedContract> GetByCode(MasterSearchOption option)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// �擾
        /// </summary>
        /// <param name="companyId"></param>
        /// <returns></returns>
        [HttpPost]
        public Task<IEnumerable<LongTermAdvanceReceivedContract>> GetItems([FromBody] int companyId)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// �o�^
        /// </summary>
        /// <param name="contract"></param>
        /// <returns></returns>
        [HttpPost]
        public Task<LongTermAdvanceReceivedContract> Save(LongTermAdvanceReceivedContract contract)
        {
            throw new NotImplementedException();
        }
    }
}
