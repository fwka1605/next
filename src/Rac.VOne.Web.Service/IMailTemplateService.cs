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
    // メモ: [リファクター] メニューの [名前の変更] コマンドを使用すると、コードと config ファイルの両方で同時にインターフェイス名 "IMailTemplateService" を変更できます。
    [ServiceContract]
    public interface IMailTemplateService
    {
        [OperationContract]
        Task<MailTemplateResult> SaveAsync(string sessionKey, MailSetting mailSetting);

        [OperationContract]
        Task<CountResult> DeleteAsync(string sessionKey, int id);

        [OperationContract]
        Task<MailTemplateResult> GetAsync(string sessionKey, int id);

        [OperationContract]
        Task<MailTemplatesResult> GetItemsAsync(string sessionKey, int id, int MailType);
    }
}
