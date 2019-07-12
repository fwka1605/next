using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Rac.VOne.Web.Models;

namespace Rac.VOne.Web.Common.Spreadsheets
{
    /// <summary>滞留明細一覧表 帳票</summary>
    public interface IArrearagesListSpreadsheetProcessor
    {
        /// <summary>
        /// 集約するかどうか オプション
        /// </summary>
        /// <param name="option"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        Task<byte[]> GetAsync(ArrearagesListSearch option, CancellationToken token = default(CancellationToken));
    }
}
