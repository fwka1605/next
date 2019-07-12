using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Rac.VOne.Web.Models;

namespace Rac.VOne.Web.Common
{
    public interface IAuthorizationWebApiProcessor
    {
        Task<ProcessResult> AuthorizeAsync(string sessionKey);
    }
}
