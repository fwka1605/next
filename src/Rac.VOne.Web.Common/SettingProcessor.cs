using System;
using System.Collections.Generic;
using System.Linq;
using Rac.VOne.Data;
using Rac.VOne.Data.QueryProcessors;
using Rac.VOne.Web.Models;
using System.Threading;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Common
{
    public class SettingProcessor : ISettingProcessor
    {
        private readonly ISettingQueryProcessor settingQueryProcessor;

        public SettingProcessor(
            ISettingQueryProcessor settingQueryProcessor
            )
        {
            this.settingQueryProcessor = settingQueryProcessor;
        }

        public async Task<IEnumerable<Setting>> GetAsync(IEnumerable<string> ItemId, CancellationToken token = default(CancellationToken))
            => await settingQueryProcessor.GetAsync(ItemId, token);
    }
}
