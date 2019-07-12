using NLog;
using Rac.VOne.Common.Logging;
using Rac.VOne.Web.Common;
using Rac.VOne.Web.Models;
using Rac.VOne.Web.Service.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Service
{
    // メモ: [リファクター] メニューの [名前の変更] コマンドを使用すると、コード、svc、および config ファイルで同時にクラス名 "ClientKey" を変更できます。
    // 注意: このサービスをテストするために WCF テスト クライアントを起動するには、ソリューション エクスプローラーで ClientKey.svc または ClientKey.svc.cs を選択し、デバッグを開始してください。
    public class ClientKey : IDBService
    {
        private readonly IAuthorizationProcessor authorizationProcessor;
        private readonly IDbFunctionProcessor dbfunctionProcessor;
        private readonly ILogger logger;

        public ClientKey(
               IAuthorizationProcessor authorizationProcessor,
               IDbFunctionProcessor dbfunctionProcessor,
               ILogManager logManager)
        {
            this.authorizationProcessor = authorizationProcessor;
            this.dbfunctionProcessor = dbfunctionProcessor;
            logger = logManager.GetLogger(typeof(ClientKey));
        }

        public async Task<ClientKeyResult> GetClientKeyAsync(string SessionKey, string ProgramId, string ClientName, string CompanyCode, string LoginUserCode)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = await dbfunctionProcessor.CreateClientKeyAsync(new ClientKeySearch {
                    ProgramId       = ProgramId,
                    ClientName      = ClientName,
                    CompanyCode     = CompanyCode,
                    LoginUserCode   = LoginUserCode,
                }, token);
                return new ClientKeyResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    ClientKey = result,
                };
            }, logger);
        }
    }
}
