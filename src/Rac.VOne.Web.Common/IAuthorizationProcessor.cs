using System;
using System.Threading.Tasks;
using Rac.VOne.Web.Models;
using Rac.VOne.Data;

namespace Rac.VOne.Web.Common
{
    public interface IAuthorizationProcessor
    {

        Task<Tuple<ProcessResult, IConnectionFactory>> AuthorizeAsync(string Sessionkey);
    }
}
