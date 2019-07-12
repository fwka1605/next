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
    // メモ: [リファクター] メニューの [名前の変更] コマンドを使用すると、コードと config ファイルの両方で同時にインターフェイス名 "IJuridicalPersonalityMaster" を変更できます。
    [ServiceContract]
    public interface IJuridicalPersonalityMaster
    {
        [OperationContract]
        Task<JuridicalPersonalityResult> SaveAsync(string SessionKey, JuridicalPersonality JuridicalPersonality);

        [OperationContract]
        Task<CountResult> DeleteAsync(string SessionKey, int CompanyId, string Kana);

        [OperationContract]
        Task<JuridicalPersonalityResult> GetAsync(string SessionKey, int CompanyId, string Kana);

        [OperationContract]
        Task<JuridicalPersonalitysResult> GetItemsAsync(string SessionKey, int CompanyId);

        [OperationContract]
        Task<ImportResult> ImportAsync(string SessionKey,
                JuridicalPersonality[] InsertList, JuridicalPersonality[] UpdateList, JuridicalPersonality[] DeleteList);
    }
}