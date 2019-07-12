using System;
using System.Collections.Generic;
using System.Text;
using Rac.VOne.Web.Models;
using System.Threading;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Common
{
    public interface ICompanyInitializeProcessor
    {
        Task<Company> InitializeAsync(CompanySource source, CancellationToken token = default(CancellationToken));
    }
}
