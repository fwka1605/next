using System;
using System.Linq;
using System.Collections.Generic;
using Rac.VOne.Common.TypeMapping;
using Rac.VOne.Data.QueryProcessors;
using Rac.VOne.Web.Models;
using System.Threading;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Common
{
    public class ColumnNameSettingProcessor : IColumnNameSettingProcessor
    {
        private readonly IAddColumnNameSettingQueryProcessor addColumnNameSettingQueryProcessor;
        private readonly IColumnNameSettingQueryProcessor columnNameSettingQueryProcessor;

        public ColumnNameSettingProcessor(
            IAddColumnNameSettingQueryProcessor addColumnNameSettingQueryProcessor,
            IColumnNameSettingQueryProcessor columnNameSettingQueryProcessor
            )
        {
            this.addColumnNameSettingQueryProcessor = addColumnNameSettingQueryProcessor;
            this.columnNameSettingQueryProcessor = columnNameSettingQueryProcessor;
        }

        public async Task<IEnumerable<ColumnNameSetting>> GetAsync(ColumnNameSetting setting, CancellationToken token = default(CancellationToken))
            => await columnNameSettingQueryProcessor.GetAsync(setting, token);


        public async Task<ColumnNameSetting> SaveAsync(ColumnNameSetting ColumnName, CancellationToken token = default(CancellationToken))
            => await addColumnNameSettingQueryProcessor.SaveAsync(ColumnName, token);

    }
}
