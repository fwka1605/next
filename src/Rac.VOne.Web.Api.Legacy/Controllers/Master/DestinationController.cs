using System.Web.Http;
using Rac.VOne.Web.Common;
using Rac.VOne.Web.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Api.Legacy.Controllers
{
    /// <summary>
    ///  仕向先マスター
    /// </summary>
    public class DestinationController : ApiControllerAuthorized
    {
        private readonly IDestinationProcessor destinationProcessor;

        /// <summary>
        /// constructor
        /// </summary>
        public DestinationController(
            IDestinationProcessor destinationProcessor
            )
        {
            this.destinationProcessor = destinationProcessor;
        }

        /// <summary>
        /// 取得 配列
        /// </summary>
        /// <param name="option">取得条件</param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IEnumerable<Destination>> GetItems(DestinationSearch option, CancellationToken token)
            => (await destinationProcessor.GetAsync(option, token)).ToArray();

        /// <summary>
        /// 登録
        /// </summary>
        /// <param name="destination">仕向先</param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Destination> Save(Destination destination, CancellationToken token)
            => await destinationProcessor.SaveAsync(destination, token);

        /// <summary>
        /// 削除
        /// </summary>
        /// <param name="destination">仕向先IDを指定</param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<int> Delete(Destination destination, CancellationToken token)
            => await destinationProcessor.DeleteAsync(destination.Id, token);

    }
}
