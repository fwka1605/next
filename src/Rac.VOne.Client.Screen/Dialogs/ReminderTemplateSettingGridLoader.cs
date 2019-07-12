using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Rac.VOne.Web.Models;
using Rac.VOne.Client.Common;
using Rac.VOne.Client.Common.MultiRow;
using Rac.VOne.Client.Screen.ReminderSettingService;
using GrapeCity.Win.MultiRow;

namespace Rac.VOne.Client.Screen.Dialogs
{
    public class ReminderTemplateSettingGridLoader : IGridLoader<ReminderTemplateSetting>
    {
        public IApplication Application { get; private set; }

        public ReminderTemplateSettingGridLoader(IApplication app)
        {
            Application = app;
        }

        public Template CreateGridTemplate()
        {
            var builder = Application.CreateGcMultirowTemplateBuilder(ColorSetting.Current);
            var rowHeight = builder.DefaultRowHeight;
            builder.Items.AddRange(new CellSetting[]
            {
                new CellSetting(rowHeight,  40, "Header", cell: builder.GetRowHeaderCell() ),
                new CellSetting(rowHeight, 170, "Code"  , caption: "パターンコード", dataField: nameof(ReminderTemplateSetting.Code), cell: builder.GetTextBoxCell(MultiRowContentAlignment.MiddleCenter), sortable: true ),
                new CellSetting(rowHeight, 429, "Name"  , caption: "パターン名"    , dataField: nameof(ReminderTemplateSetting.Name), cell: builder.GetTextBoxCell(), sortable: true ),
                new CellSetting(rowHeight,   0, "Id"    , visible: false          , dataField: nameof(ReminderTemplateSetting.Id) ),
            });
            return builder.Build();
        }

        public async Task<IEnumerable<ReminderTemplateSetting>> SearchByKey(params string[] keys)
        {
            var list = await SearchInfo();
            if (list != null)
            {
                var word = keys[0];
                list = list.Where(x =>
                    !string.IsNullOrEmpty(x.Name) && x.Name.RoughlyContains(word));
            }
            return list.ToList();
        }

        public async Task<IEnumerable<ReminderTemplateSetting>> SearchInfo()
        {
            return await ServiceProxyFactory.DoAsync(async (ReminderSettingServiceClient client) =>
            {
                var result = await client.GetReminderTemplateSettingsAsync(Application.Login.SessionKey, Application.Login.CompanyId);
                if (result.ProcessResult.Result)
                    return result.ReminderTemplateSettings;
                return new List<ReminderTemplateSetting>();
            });
        }

    }
}
