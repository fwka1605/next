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
    public interface IPasswordPolicyMaster
    {

        [OperationContract]
        Task<PasswordPolicyResult> GetAsync(string SessionKey, int CompanyId);

        [OperationContract]
        Task<PasswordPolicyResult> SaveAsync(string SessionKey, PasswordPolicy PasswordPolicy);
    }
}
