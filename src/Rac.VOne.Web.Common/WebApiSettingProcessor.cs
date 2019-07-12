using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Rac.VOne.Data.QueryProcessors;
using Rac.VOne.Web.Models;

namespace Rac.VOne.Web.Common
{
    public class WebApiSettingProcessor
        : IWebApiSettingProcessor
    {
        private readonly IAddWebApiSettingQueryProcessor addWebApiSettingQueryProcessor;
        private readonly IDeleteWebApiSettingQueryProcessor deleteWebApiSettingQueryProcessor;
        private readonly IWebApiSettingQueryProcessor webApiSettingQueryProcessor;

        public WebApiSettingProcessor(
            IWebApiSettingQueryProcessor webApiSettingQueryProcessor,
            IAddWebApiSettingQueryProcessor addWebApiSettingQueryProcessor,
            IDeleteWebApiSettingQueryProcessor deleteWebApiSettingQueryProcessor)
        {
            this.webApiSettingQueryProcessor = webApiSettingQueryProcessor;
            this.addWebApiSettingQueryProcessor = addWebApiSettingQueryProcessor;
            this.deleteWebApiSettingQueryProcessor = deleteWebApiSettingQueryProcessor;
        }

        public async Task<int> DeleteAsync(int companyId, int? apiTypeId,
            CancellationToken token = default(CancellationToken))
            => await deleteWebApiSettingQueryProcessor.DeleteAsync(companyId, apiTypeId, token);

        public async Task<WebApiSetting> GetByIdAsync(int companyId, int apiTypeId,
            CancellationToken token = default(CancellationToken))
            => await webApiSettingQueryProcessor.GetByIdAsync(companyId, apiTypeId, token);

        public async Task<int> SaveAsync(WebApiSetting setting,
            CancellationToken token = default(CancellationToken))
            => await addWebApiSettingQueryProcessor.SaveAsync(setting, token);
    }
}
