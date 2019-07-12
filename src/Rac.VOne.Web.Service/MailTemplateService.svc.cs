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
    // メモ: [リファクター] メニューの [名前の変更] コマンドを使用すると、コード、svc、および config ファイルで同時にクラス名 "MailTemplateService" を変更できます。
    // 注意: このサービスをテストするために WCF テスト クライアントを起動するには、ソリューション エクスプローラーで MailTemplateService.svc または MailTemplateService.svc.cs を選択し、デバッグを開始してください。
    public class MailTemplateService : IMailTemplateService
    {
        public Task<CountResult> DeleteAsync(string sessionKey, int id)
        {
            throw new NotImplementedException();
        }

        public Task<MailTemplateResult> GetAsync(string sessionKey, int id)
        {
            throw new NotImplementedException();
        }

        public Task<MailTemplatesResult> GetItemsAsync(string sessionKey, int id, int MailType)
        {
            throw new NotImplementedException();
        }

        public Task<MailTemplateResult> SaveAsync(string sessionKey, MailSetting mailSetting)
        {
            throw new NotImplementedException();
        }
    }
}
