using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Rac.VOne.Web.Models;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Service
{
    // メモ: [リファクター] メニューの [名前の変更] コマンドを使用すると、コードと config ファイルの両方で同時にインターフェイス名 "INettingService" を変更できます。
    [ServiceContract]
    public interface INettingService
    {
        [OperationContract]
        Task<ExistResult> ExistCustomerAsync(string SessionKey, int CustomerId);
        [OperationContract]
        Task<ExistResult> ExistReceiptCategoryAsync(string SessionKey, int CategoryId);
        [OperationContract]
        Task<ExistResult> ExistCurrencyAsync(string SessionKey, int CurrencyId);
        [OperationContract]
        Task<ExistResult> ExistSectionAsync(string SectionKey, int SectionId);
        [OperationContract]
        Task<NettingResult> SaveAsync(string SessionKey, Models.Netting[] Netting);
        [OperationContract]
        Task<NettingsResult> GetItemsAsync(string SessionKey, int CompanyId, int CustomerId, int CurrencyId);
        [OperationContract]
        Task<NettingResult> DeleteAsync(string SessionKey, long[] NettingId);
    }
}
