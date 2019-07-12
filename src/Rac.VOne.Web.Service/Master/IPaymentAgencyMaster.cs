using Rac.VOne.Web.Models;
using System.ServiceModel;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Service.Master
{
    [ServiceContract]
    public interface IPaymentAgencyMaster
    {

        [OperationContract]
        Task<PaymentAgenciesResult> GetItemsAsync(string SessionKey, int CompanyId);

        [OperationContract]
        Task<PaymentAgenciesResult> GetByCodeAsync(string SessionKey, int CompanyId, string[] Code);

        [OperationContract]
        Task<PaymentAgencyResult> SaveAsync(string SessionKey, PaymentAgency PaymentAgency);

        [OperationContract]
        Task<CountResult> DeleteAsync(string SessionKey, int Id);

        [OperationContract]
        Task<PaymentAgenciesResult> GetAsync(string SessionKey, int[] Id);

        [OperationContract]
        Task<PaymentFileFormatResult> GetFormatItemsAsync(string SessionKey);

    }
}
