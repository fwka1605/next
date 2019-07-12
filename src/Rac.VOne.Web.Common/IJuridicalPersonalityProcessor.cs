using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Rac.VOne.Web.Models;

namespace Rac.VOne.Web.Common
{
    public interface IJuridicalPersonalityProcessor
    {
        Task<IEnumerable<JuridicalPersonality>> GetAsync(JuridicalPersonality personality, CancellationToken token = default(CancellationToken));

        Task<JuridicalPersonality> SaveAsync(JuridicalPersonality personality, CancellationToken token = default(CancellationToken));

        Task<int> DeleteAsync(int CompanyId, string Kana, CancellationToken token = default(CancellationToken));

        Task<ImportResult> ImportAsync(
            IEnumerable<JuridicalPersonality> insert,
            IEnumerable<JuridicalPersonality> update,
            IEnumerable<JuridicalPersonality> delete, CancellationToken token = default(CancellationToken));
    }
}
