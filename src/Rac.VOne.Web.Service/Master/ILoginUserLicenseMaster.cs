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
    // メモ: [リファクター] メニューの [名前の変更] コマンドを使用すると、コードと config ファイルの両方で同時にインターフェイス名 "ILoginUserLicenseMaster" を変更できます。
    [ServiceContract]
    public interface ILoginUserLicenseMaster
    {
        [OperationContract]
        Task<LoginUserLicensesResult> GetItemsAsync(string SessionKey, int CompanyId);

        [OperationContract]
        Task<LoginUserLicensesResult> SaveAsync(string SessionKey,int CompanyId, string[] LicenseKeys);
    }
}
