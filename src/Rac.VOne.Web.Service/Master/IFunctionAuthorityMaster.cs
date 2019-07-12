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
    public interface IFunctionAuthorityMaster
    {
        [OperationContract]
        Task<FunctionAuthoritiesResult> SaveAsync(string SessionKey, FunctionAuthority[] functionAuthorities);

        [OperationContract]
        Task<CountResult> DeleteAsync(string SessionKey, int CompanyId, int FunctionType, int AuthorityLevel);

        [OperationContract]
        Task<FunctionAuthorityResult> GetAsync(string SessionKey, int CompanyId, int FunctionType, int AuthorityLevel);

        [OperationContract]
        Task<FunctionAuthoritiesResult> GetByFunctionTypeAsync(string SessionKey, int CompanyId, int FunctionType);

        [OperationContract]
        Task<FunctionAuthoritiesResult> GetItemsAsync(string SessionKey, int CompanyId);

        [OperationContract]
        Task<FunctionAuthoritiesResult> GetByLoginUserAsync(string SessionKey, int CompanyId, string LoginUserCode, int[] FunctionType);
    }
}