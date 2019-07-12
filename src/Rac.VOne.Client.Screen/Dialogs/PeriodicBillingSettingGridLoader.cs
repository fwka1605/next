using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GrapeCity.Win.MultiRow;
using Rac.VOne.Client.Common;
using Rac.VOne.Client.Common.MultiRow;
using Rac.VOne.Web.Models;
using Header = Rac.VOne.Web.Models.PeriodicBillingSetting;

namespace Rac.VOne.Client.Screen.Dialogs
{
    public class PeriodicBillingSettingGridLoader : IGridLoader<Header>
    {
        public IApplication Application { get; private set; }
        public PeriodicBillingSettingGridLoader(IApplication application)
        {
            Application = application;
        }

        public Template CreateGridTemplate()
        {
            var builder = Application.CreateGcMultirowTemplateBuilder(ColorSetting.Default);
            var height = builder.DefaultRowHeight;
            var center = MultiRowContentAlignment.MiddleCenter;
            builder.Items.AddRange(new[] {
                new CellSetting(height,  40, "Header", cell: builder.GetRowHeaderCell()),
                new CellSetting(height, 100, nameof(Header.Name)        , nameof(Header.Name)        , "パターン名"  , sortable: true),
                new CellSetting(height,  90, nameof(Header.CustomerCode), nameof(Header.CustomerCode), "得意先コード", sortable: true, cell: builder.GetTextBoxCell(center)),
                new CellSetting(height, 150, nameof(Header.CustomerName), nameof(Header.CustomerName), "得意先名"    , sortable: true),
            });
            return builder.Build();
        }

        public async Task<IEnumerable<Header>> SearchByKey(params string[] keys)
        {
            var option = new PeriodicBillingSettingSearch {
                CompanyId = Application.Login.CompanyId,
            };
            if ((keys?.Any() ?? false) && !string.IsNullOrWhiteSpace(keys.First()))
                option.Name = keys.First();
            else
                option.Name = string.Empty;
            var result = await ServiceProxyFactory.DoAsync(async (PeriodicBillingSettingMasterService.PeriodicBillingSettingMasterClient client)
                => await client.GetItemsAsync(Application.Login.SessionKey, option));
            if (result.ProcessResult.Result)
                return result.PeriodicBillingSettings;
            return new List<Header>();
        }

        public async Task<IEnumerable<Header>> SearchInfo()
            => await SearchByKey();
    }
}
