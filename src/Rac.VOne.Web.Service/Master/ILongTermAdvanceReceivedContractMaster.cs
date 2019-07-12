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
    // メモ: [リファクター] メニューの [名前の変更] コマンドを使用すると、コードと config ファイルの両方で同時にインターフェイス名 "ILongTermAdvanceReceivedContractMaster" を変更できます。
    [ServiceContract]
    public interface ILongTermAdvanceReceivedContractMaster
    {
        [OperationContract]
        Task<LongTermAdvanceReceivedContractResult> SaveAsync(string SessionKey, LongTermAdvanceReceivedContract longTermAdvanceReceivedContract);

        [OperationContract]
        Task<CountResult> DeleteAsync(string SessionKey, int Id);

        [OperationContract]
        Task<LongTermAdvanceReceivedContractResult> GetAsync(string SessionKey, int Id);

        [OperationContract]
        Task<LongTermAdvanceReceivedContractResult> GetByCodeAsync(string SessionKey, int Id, string Code);

        [OperationContract]
        Task<LongTermAdvanceReceivedContractsResult> GetItemsAsync(string SessionKey, int CompanyId);
    }
}