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
    // メモ: [リファクター] メニューの [名前の変更] コマンドを使用すると、コードと config ファイルの両方で同時にインターフェイス名 "ISectionWithLoginUserMaster" を変更できます。
    [ServiceContract]
    public interface ISectionWithLoginUserMaster
    {
        [OperationContract] Task<SectionWithLoginUsersResult> GetByLoginUserAsync(string SessionKey, int CompanyId, int LoginUserId);
        [OperationContract] Task<SectionWithLoginUsersResult> GetItemsAsync(string SessionKey, int CompanyId,
            SectionWithLoginUserSearch SectionWithLoginUserSearch);
        [OperationContract] Task<SectionWithLoginUserResult> SaveAsync(string SessionKey, SectionWithLoginUser[] AddList, 
            SectionWithLoginUser[] DeleteList);
        [OperationContract] Task<ImportResultSectionWithLoginUser> ImportAsync(string SessionKey,SectionWithLoginUser[] InsertList,
            SectionWithLoginUser[] UpdateList, SectionWithLoginUser[] DeleteList);
        [OperationContract] Task<ExistResult> ExistLoginUserAsync(string SessionKey, int LoginUserId);
        [OperationContract] Task<ExistResult> ExistSectionAsync(string SessionKey, int SectionId);
    }
}
