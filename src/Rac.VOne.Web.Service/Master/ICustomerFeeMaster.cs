using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using Rac.VOne.Web.Models;

namespace Rac.VOne.Web.Service.Master
{
    // メモ: [リファクター] メニューの [名前の変更] コマンドを使用すると、コードと config ファイルの両方で同時にインターフェイス名 "ICustomerFeeMaster" を変更できます。
    [ServiceContract]
    public interface ICustomerFeeMaster
    {
        [OperationContract]
        Task<CustomerFeeResult> SaveAsync(string SessionKey, int CustomerId, int CurrencyId, CustomerFee[] CustomerFees);

        [OperationContract]
        Task<CustomerFeeResult> GetAsync(string SessionKey, int CustomerId, int CurrencyId);

        [OperationContract]
        Task<CustomerFeesResult> GetForPrintAsync(string SessionKey, int CompanyId);

        [OperationContract]
        Task<CustomerFeeResult> GetForExportAsync(string SessionKey, int CompanyId);

        [OperationContract]
        Task<ImportResult> ImportAsync(string SessionKey,
                CustomerFee[] insertList, CustomerFee[] updateList, CustomerFee[] deleteList);
    }
}
