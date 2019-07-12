using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Rac.VOne.Web.Models;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Service.Master
{

    [ServiceContract]
    public interface ILoginUserPasswordMaster
    {
        [OperationContract] Task<LoginPasswordChangeResult> ChangeAsync(string SessionKey, int CompanyId, int LoginUserId, string OldPassword, string NewPassword);
        [OperationContract] Task<LoginProcessResult> LoginAsync(string SessionKey, int CompanyId, int LoginUserId, string Password);
        [OperationContract] Task<LoginPasswordChangeResult> SaveAsync(string SessionKey, int CompanyId, int LoginUserId, string Password);
    }
}
