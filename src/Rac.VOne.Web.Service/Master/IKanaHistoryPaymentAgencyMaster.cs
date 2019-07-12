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
    // メモ: [リファクター] メニューの [名前の変更] コマンドを使用すると、コードと config ファイルの両方で同時にインターフェイス名 "IKanaHistoryPaymentAgencyMaster" を変更できます。
    [ServiceContract]
    public interface IKanaHistoryPaymentAgencyMaster
    {
        [OperationContract]
        Task<KanaHistoryPaymentAgencysResult> GetListAsync(string SessionKey, int CompanyId, string PayerName, string PaymentAgencyCodeFrom, string PaymentAgencyCodeTo);

        [OperationContract]
        Task<CountResult> DeleteAsync(string SessionKey,
               int CompanyId, string PayerName, string SourceBankName, string SourceBranchName, int PaymentAgencyId);

        [OperationContract]
        Task<ImportResult> ImportAsync(string SessionKey,
            KanaHistoryPaymentAgency[] InsertList, KanaHistoryPaymentAgency[] UpdateList, KanaHistoryPaymentAgency[] DeleteList);

    }
}
