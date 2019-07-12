using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Rac.VOne.Web.Models;

namespace Rac.VOne.Web.Common
{
    public interface IPasswordPolicyProcessor
    {
        Task<PasswordPolicy> GetAsync(int CompanyId, CancellationToken token = default(CancellationToken));
        Task<PasswordPolicy> SaveAsync(PasswordPolicy PasswordPolicy, CancellationToken token = default(CancellationToken));
    }
}
