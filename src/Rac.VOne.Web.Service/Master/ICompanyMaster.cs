using System.ServiceModel;
using Rac.VOne.Web.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Service.Master
{
    [ServiceContract]
    public interface ICompanyMaster
    {
        [OperationContract]
        Task<CountResult> DeleteAsync(string SessionKey, int CompanyId);

        [OperationContract]
        Task<CountResult> DeleteLogoAsync(string SessionKey, int CompanyId);

        [OperationContract]
        Task<CountResult> DeleteLogosAsync(string SessionKey, IEnumerable<CompanyLogo> CompanyLogos);

        [OperationContract]
        Task<CompanyResult> GetAsync(string SessionKey, int CompanyId);

        [OperationContract]
        Task<CompanyResult> GetByCodeAsync(string SessionKey, string CompanyCode);

        [OperationContract]
        Task<CompanyLogoResult> GetLogoAsync(string SessionKey, int CompanyId);

        [OperationContract]
        Task<CompanyLogosResult> GetLogosAsync(string SessionKey, int CompanyId);

        [OperationContract]
        Task<CompanyResult> SaveAsync(string SessionKey, Company Company);

        [OperationContract]
        Task<CompanyLogoResult> SaveLogoAsync(string SessionKey, CompanyLogo CompanyLogo);

        [OperationContract]
        Task<CompanyLogosResult> SaveLogosAsync(string SessionKey, IEnumerable<CompanyLogo> CompanyLogos);

        [OperationContract]
        Task<CompaniesResult> GetItemsAsync(string SessionKey, string Name);

        [OperationContract]
        Task<CompanyResult> CreateAsync(string SessionKey, CompanySource CompanySource);
    }
}
