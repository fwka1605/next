using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Rac.VOne.Web.Models;

namespace Rac.VOne.Data.QueryProcessors
{
    public interface IJuridicalPersonalityQueryProcessor
    {
        Task<IEnumerable<JuridicalPersonality>> GetAsync(JuridicalPersonality personality, CancellationToken token = default(CancellationToken));
        Task<int> InitializeAsync(int companyId, int loginUserId, CancellationToken token = default(CancellationToken));
    }
}
