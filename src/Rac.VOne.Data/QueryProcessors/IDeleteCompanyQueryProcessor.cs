using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Rac.VOne.Data.QueryProcessors
{
    public interface IDeleteCompanyQueryProcessor
    {
        /// <summary>
        /// クエリの内部で関連テーブルのデータをすべて削除している
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        Task<int> DeleteAsync(int Id, CancellationToken token = default(CancellationToken));
    }
}
