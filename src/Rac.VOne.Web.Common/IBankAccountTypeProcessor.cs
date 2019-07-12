using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Rac.VOne.Web.Models;

namespace Rac.VOne.Web.Common
{
    /// <summary>預金種別 （銀行口座 口座種別）</summary>
    public interface IBankAccountTypeProcessor
    {
        Task<IEnumerable<BankAccountType>> GetAsync(CancellationToken token = default(CancellationToken));
    }
}
