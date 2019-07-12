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
    // メモ: [リファクター] メニューの [名前の変更] コマンドを使用すると、コードと config ファイルの両方で同時にインターフェイス名 "IPaymentAgencyFeeMaster" を変更できます。
    [ServiceContract]
    public interface IPaymentAgencyFeeMaster
    {
        [OperationContract]
        Task<PaymentAgencyFeesResult> SaveAsync(string SessionKey, int PaymentAgencyId, int CurrencyId, PaymentAgencyFee[] PaymentAgencyFee);

        [OperationContract]
        Task<PaymentAgencyFeesResult> GetAsync(string SessionKey, int PaymentAgencyId, int CurrencyId);

        [OperationContract]
        Task<PaymentAgencyFeesResult> GetForExportAsync(string SessionKey, int CompanyId);
    }
}
