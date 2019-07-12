using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Rac.VOne.Web.Models;
using Rac.VOne.Web.Common;
using Rac.VOne.Web.Service.Extensions;
using Rac.VOne.Common.Logging;
using NLog;

namespace Rac.VOne.Web.Service.Master
{
    public class ColumnNameSettingMaster : IColumnNameSettingMaster
    {

        private readonly IAuthorizationProcessor authorizationProcessor;
        private readonly IColumnNameSettingProcessor columnNameSettingProcessor;
        private readonly ILogger logger;

        public ColumnNameSettingMaster(
            IAuthorizationProcessor authorizationProcessor,
            IColumnNameSettingProcessor columnNameSettingProcessor,
            ILogManager logManager)
        {
            this.authorizationProcessor = authorizationProcessor;
            this.columnNameSettingProcessor = columnNameSettingProcessor;
            logger = logManager.GetLogger(typeof(ColumnNameSettingMaster));
        }


        public async Task<ColumnNameSettingResult> GetAsync(string SessionKey, int CompanyId, string TableName, string ColumnName)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = (await columnNameSettingProcessor.GetAsync(new ColumnNameSetting {
                    CompanyId       = CompanyId,
                    TableName       = TableName,
                    ColumnName      = ColumnName,
                }, token)).First();

                return new ColumnNameSettingResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    ColumnName = result
                };
            }, logger);
        }

        public async Task<ColumnNameSettingsResult> GetItemsAsync(string SessionKey, int CompanyId)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = (await columnNameSettingProcessor.GetAsync(new ColumnNameSetting { CompanyId = CompanyId }, token)).ToList();
                return new ColumnNameSettingsResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    ColumnNames = result
                };
            }, logger);
        }

        public async Task<ColumnNameSettingResult> SaveAsync(string SessionKey, ColumnNameSetting ColumnName)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {

                var result = await columnNameSettingProcessor.SaveAsync(ColumnName, token);
                return new ColumnNameSettingResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    ColumnName = result
                };
            }, logger);
        }
    }
}
