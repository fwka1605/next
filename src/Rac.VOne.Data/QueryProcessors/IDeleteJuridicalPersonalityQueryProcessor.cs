using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Rac.VOne.Data.QueryProcessors
{
    public interface IDeleteJuridicalPersonalityQueryProcessor
    {
        Task<int> DeleteAsync(int CompanyId, string Kana, CancellationToken token = default(CancellationToken));
    }
}
