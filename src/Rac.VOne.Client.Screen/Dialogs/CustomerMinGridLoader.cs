using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rac.VOne.Web.Models;
using Rac.VOne.Client.Common;
using Rac.VOne.Client.Common.MultiRow;
using Rac.VOne.Client.Screen.CustomerMasterService;
using GrapeCity.Win.MultiRow;

namespace Rac.VOne.Client.Screen.Dialogs
{
    public class CustomerMinGridLoader : IGridLoader<CustomerMin>
    {
        public IApplication Application { get; private set; }

        public CustomerMinGridLoader(IApplication app)
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
                new CellSetting(rowHeight, 115, "Code"  , caption: "得意先コード"  , dataField: nameof(Customer.Code), cell: builder.GetTextBoxCell(MultiRowContentAlignment.MiddleCenter), sortable: true ),
                new CellSetting(rowHeight, 245, "Name"  , caption: "得意先名"      , dataField: nameof(Customer.Name), cell: builder.GetTextBoxCell(), sortable: true ),
                new CellSetting(rowHeight, 239, "Kana"  , caption: "得意先名カナ"  , dataField: nameof(Customer.Kana), cell: builder.GetTextBoxCell(), sortable: true ),
                new CellSetting(rowHeight,   0, "Id"    , caption: "Id"            , dataField: nameof(Customer.Id)  , cell: builder.GetTextBoxCell(), sortable: true ),
            });

            return builder.Build();
        }
    

        public async Task<IEnumerable<CustomerMin>> SearchByKey(params string[] keys)
        {
            var list = await SearchInfo();
            if (list != null)
            {
                var word = keys[0];
                list = list.Where(x =>
                    !string.IsNullOrEmpty(x.Name) && x.Name.RoughlyContains(word)
                 || !string.IsNullOrEmpty(x.Kana) && x.Kana.RoughlyContains(word)
                 || !string.IsNullOrEmpty(x.Code) && x.Code.RoughlyContains(word));
            }
            return list.ToList();
        }

        public async Task<IEnumerable<CustomerMin>> SearchInfo()
        {
            return await ServiceProxyFactory.DoAsync(async (CustomerMasterClient client) =>
            {
                var result = await client.GetMinItemsAsync(Application.Login.SessionKey, Application.Login.CompanyId);
                if (result.ProcessResult.Result)
                    return result.Customers;
                return new List<CustomerMin>();
            });
        }
    }
}
