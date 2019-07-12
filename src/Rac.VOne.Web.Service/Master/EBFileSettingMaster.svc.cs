using NLog;
using Rac.VOne.Common.Logging;
using Rac.VOne.Web.Common;
using Rac.VOne.Web.Models;
using Rac.VOne.Web.Service.Extensions;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Service.Master
{
    public class EBFileSettingMaster : IEBFileSettingMaster
    {
        private readonly IAuthorizationProcessor authorizationProcessor;
        private readonly IEBFileSettingProcessor ebFileFormatSettingProcessor;
        private readonly ILogger logger;

        public EBFileSettingMaster(
            IAuthorizationProcessor authorizationProcessor,
            IEBFileSettingProcessor ebFileFormatSettingProcessor,
            ILogManager logManager)
        {
            this.authorizationProcessor = authorizationProcessor;
            this.ebFileFormatSettingProcessor = ebFileFormatSettingProcessor;
            logger = logManager.GetLogger(typeof(EBFileSettingMaster));
        }

        public async Task<CountResult> DeleteAsync(string SessionKey, int Id)
            => await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = await ebFileFormatSettingProcessor.DeleteAsync(Id, token);
                return new CountResult {
                    ProcessResult = new ProcessResult { Result = true },
                    Count = result,
                };
            }, logger);


        public async Task<EBFileSettingResult> GetItemAsync(string SessionKey, int Id)
            => await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token => {
                var result = (await ebFileFormatSettingProcessor.GetAsync(new EBFileSettingSearch { Ids = new[] { Id } }, token)).First();
                return new EBFileSettingResult {
                    ProcessResult = new ProcessResult { Result = true },
                    EBFileSetting = result,
                };
            }, logger);

        public async Task<EBFileSettingsResult> GetItemsAsync(string SessionKey, int CompanyId)
            => await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token => {
                var result = (await ebFileFormatSettingProcessor.GetAsync(new EBFileSettingSearch { CompanyId = CompanyId }, token)).ToList();
                return new EBFileSettingsResult {
                    ProcessResult = new ProcessResult {  Result = true },
                    EBFileSettings = result,
                };
            }, logger);

        public async Task<EBFileSettingResult> SaveAsync(string SessionKey, EBFileSetting setting)
            => await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token => {
                var result = await ebFileFormatSettingProcessor.SaveAsync(setting, token);
                return new EBFileSettingResult {
                    ProcessResult = new ProcessResult { Result = true },
                    EBFileSetting = result,
                };
            }, logger);

        public async Task<CountResult> UpdateIsUseableAsync(string SessionKey, int CompanyId, int LoginUserId, int[] ids)
            => await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token => {
                var result = await ebFileFormatSettingProcessor.UpdateIsUseableAsync(CompanyId, LoginUserId, ids, token);
                return new CountResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    Count = result,
                };
            }, logger);



    }

}
