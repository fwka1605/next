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

namespace Rac.VOne.Web.Service.Master
{
    public class EBExcludeAccountSettingMaster : IEBExcludeAccountSettingMaster
    {
        private readonly IAuthorizationProcessor authorizationProcessor;
        private readonly IEBExcludeAccountSettingProcessor ebExcludeAccountSettingProcessor;
        private readonly ILogger logger;

        public EBExcludeAccountSettingMaster(IAuthorizationProcessor authorizationProcessor,
            IEBExcludeAccountSettingProcessor ebExcludeAccountSettingProcessor,
            ILogManager logManager)
        {
            this.authorizationProcessor = authorizationProcessor;
            this.ebExcludeAccountSettingProcessor = ebExcludeAccountSettingProcessor;
            this.logger = logManager.GetLogger(typeof(EBExcludeAccountSettingMaster));
        }

        public async Task<EBExcludeAccountSettingListResult> GetItemsAsync(string SessionKey, int CompanyId)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = (await ebExcludeAccountSettingProcessor.GetAsync(CompanyId, token)).ToList();

                return new EBExcludeAccountSettingListResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    EBExcludeAccountSettingList = result,
                };
            }, logger);
        }

        public async Task<EBExcludeAccountSettingResult> SaveAsync(string SessionKey, EBExcludeAccountSetting ebExcludeAccountSetting)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = await ebExcludeAccountSettingProcessor.SaveAsync(ebExcludeAccountSetting, token);

                return new EBExcludeAccountSettingResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    EBExcludeAccountSetting = result,
                };
            }, logger);
        }

        public async Task<CountResult> DeleteAsync(string SessionKey, EBExcludeAccountSetting ebExcludeAccountSetting)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = await ebExcludeAccountSettingProcessor.DeleteAsync(ebExcludeAccountSetting, token);

                return new CountResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    Count = result,
                };
            }, logger);
        }



    }
}
