using GrapeCity.Win.MultiRow;
using Rac.VOne.Client.Common;
using Rac.VOne.Client.Common.MultiRow;
using Rac.VOne.Client.Screen.BankAccountTypeMasterService;
using Rac.VOne.Web.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace Rac.VOne.Client.Screen.Dialogs
{
    public class BankAccountTypeGridLoader : IGridLoader<BankAccountType>
    {
        public IApplication Application { get; private set; }

        /// <summary>入金データ用 ※択一</summary>
        public bool UseReceipt { private get; set; }
        /// <summary>口座振替用   ※択一</summary>
        public bool UseTransfer { private get; set; }

        public BankAccountTypeGridLoader(IApplication app, bool useReceipt = false, bool useTransfer = false)
        {
            Application = app;
            UseReceipt  = useReceipt;
            UseTransfer = useTransfer;
        }

        public Template CreateGridTemplate()
        {
            var builder = Application.CreateGcMultirowTemplateBuilder(ColorSetting.Current);
            var height = builder.DefaultRowHeight;
            var center = MultiRowContentAlignment.MiddleCenter;
            builder.Items.AddRange(new[] {
                new CellSetting(height,  40, "Header", cell: builder.GetRowHeaderCell()),
                new CellSetting(height, 170, nameof(BankAccountType.Id)  , dataField: nameof(BankAccountType.Id)  , sortable: true, caption: "預金種別"  , cell: builder.GetTextBoxCell(center)),
                new CellSetting(height, 229, nameof(BankAccountType.Name), dataField: nameof(BankAccountType.Name), sortable: true, caption: "預金種別名"),
            });
            return builder.Build();
        }

        public async Task<IEnumerable<BankAccountType>> SearchByKey(params string[] keys)
        {
            var result = await ServiceProxyFactory.DoAsync(async (BankAccountTypeMasterClient client)
                => (await client.GetItemsAsync(Application.Login.SessionKey))?.BankAccountTypes) ?? new List<BankAccountType>();
            if (UseReceipt)
                result = result.Where(x => x.UseReceipt == 1).ToList();
            else if (UseTransfer)
                result = result.Where(x => x.UseTransfer == 1).ToList();
            if (keys?.Any() ?? false)
            {
                var target = keys.First();
                result = result.Where(x => x.Id.ToString().RoughlyContains(target) || x.Name.RoughlyContains(target)).ToList();
            }
            return result;
        }

        public async Task<IEnumerable<BankAccountType>> SearchInfo() => await SearchByKey();

    }
}
