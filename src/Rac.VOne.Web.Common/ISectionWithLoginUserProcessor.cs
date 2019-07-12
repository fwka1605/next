using Rac.VOne.Data;
using Rac.VOne.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Common
{
    public interface ISectionWithLoginUserProcessor
    {
        Task<IEnumerable<SectionWithLoginUser>> GetAsync(SectionWithLoginUserSearch option, CancellationToken token = default(CancellationToken));
        Task<bool> ExistLoginUserAsync(int LoginUserId, CancellationToken token = default(CancellationToken));
        Task<bool> ExistSectionAsync(int SectionId, CancellationToken token = default(CancellationToken));
        Task<IEnumerable<SectionWithLoginUser>> SaveAsync(IEnumerable<SectionWithLoginUser> upsert, IEnumerable<SectionWithLoginUser> delete, CancellationToken token = default(CancellationToken));

        Task<ImportResult> ImportAsync(
            IEnumerable<SectionWithLoginUser> insert,
            IEnumerable<SectionWithLoginUser> update,
            IEnumerable<SectionWithLoginUser> delete, CancellationToken token = default(CancellationToken));
    }
}
