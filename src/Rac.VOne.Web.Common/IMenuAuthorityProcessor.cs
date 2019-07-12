using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Rac.VOne.Web.Models;

namespace Rac.VOne.Web.Common
{
    public interface IMenuAuthorityProcessor
    {
        Task<int> DeleteAsync(int CompanyId, CancellationToken token = default(CancellationToken));
        Task<IEnumerable<MenuAuthority>> GetAsync(MenuAuthoritySearch option, CancellationToken token = default(CancellationToken));
        Task<IEnumerable<MenuAuthority>> SaveAsync(IEnumerable<MenuAuthority> menus, CancellationToken token = default(CancellationToken));
    }
}
