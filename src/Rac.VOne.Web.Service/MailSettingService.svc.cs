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
    // メモ: [リファクター] メニューの [名前の変更] コマンドを使用すると、コード、svc、および config ファイルで同時にクラス名 "MailSettingService" を変更できます。
    // 注意: このサービスをテストするために WCF テスト クライアントを起動するには、ソリューション エクスプローラーで MailSettingService.svc または MailSettingService.svc.cs を選択し、デバッグを開始してください。
    public class MailSettingService : IMailSettingService
    {
        public Task<MailSettingResult> GetAsync(string SessionKey, int CompanyId)
        {
            throw new NotImplementedException();
        }

        public Task<MailSettingResult> SaveAsync(string SessionKey, MailSetting mailSetting)
        {
            throw new NotImplementedException();
        }
    }
}
