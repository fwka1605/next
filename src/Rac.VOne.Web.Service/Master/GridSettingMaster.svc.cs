using Rac.VOne.Common.Logging;
using Rac.VOne.Web.Common;
using Rac.VOne.Web.Models;
using Rac.VOne.Web.Service.Extensions;
using System.Collections.Generic;
using System.Linq;
using System;
using NLog;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Service.Master
{
    public class GridSettingMaster : IGridSettingMaster
    {
        private readonly IAuthorizationProcessor authorizationProcessor;
        private readonly IGridSettingProcessor gridSettingProcessor;
        private readonly ILogger logger;

        public GridSettingMaster(
            IAuthorizationProcessor authorizationProcessor,
            IGridSettingProcessor gridSettingProcessor,
            ILogManager logManager
            )
        {
            this.authorizationProcessor = authorizationProcessor;
            this.gridSettingProcessor = gridSettingProcessor;
            logger = logManager.GetLogger(typeof(GridSettingMaster));
        }

        public async Task<GridSettingResult> SaveAsync(string SessionKey, GridSetting[] GridSettings)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = (await gridSettingProcessor.SaveAsync(GridSettings, token)).First();
                return new GridSettingResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    GridSetting = result,
                };
            }, logger);
        }

        public async Task<GridSettingsResult> GetItemsAsync(string SessionKey, int CompanyId, int LoginUserId, int? GridId)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = (await gridSettingProcessor.GetAsync(new GridSettingSearch {
                    CompanyId       = CompanyId,
                    LoginUserId     = LoginUserId,
                    GridId          = GridId,
                }, token)).ToList();
                return new GridSettingsResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    GridSettings = result,
                };
            }, logger);
        }

        public async Task<GridSettingsResult> GetDefaultItemsAsync(string SessionKey, int CompanyId, int LoginUserId, int? GridId)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = (await gridSettingProcessor.GetAsync(new GridSettingSearch {
                    CompanyId       = CompanyId,
                    LoginUserId     = LoginUserId,
                    GridId          = GridId,
                    IsDefault       = true,
                }, token)).ToList();
                return new GridSettingsResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    GridSettings = result,
                };
            }, logger);
        }
    }
}
