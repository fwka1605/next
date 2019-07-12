using Rac.VOne.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Common
{
    public interface ISessionStorageProcessor
    {
        Task<Session> GetAsync(string SessionKey, CancellationToken token = default(CancellationToken));
    }
}
