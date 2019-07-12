using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Rac.VOne.Web.Models;


namespace Rac.VOne.Data.QueryProcessors
{
    public interface ISectionWithLoginUserQueryProcessor
    {
        Task<IEnumerable<SectionWithLoginUser>> GetAsync(SectionWithLoginUserSearch option, CancellationToken token = default(CancellationToken));
        Task<bool> ExistLoginUserAsync(int LoginUserId, CancellationToken token = default(CancellationToken));
        Task<bool> ExistSectionAsync(int SectionId, CancellationToken token = default(CancellationToken));
    }
}
