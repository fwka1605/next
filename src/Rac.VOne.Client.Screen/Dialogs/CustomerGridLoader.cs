using GrapeCity.Win.MultiRow;
using Rac.VOne.Client.Common;
using Rac.VOne.Client.Common.MultiRow;
using Rac.VOne.Client.Screen.CustomerMasterService;
using Rac.VOne.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rac.VOne.Client.Screen.Dialogs
{
    public class CustomerGridLoader : IGridLoader<Customer>
    {
        public IApplication Application { get; private set; }
        public SearchCustomer Key { get; set; }
        public int[] CustomerId { get; set; }

        public enum SearchCustomer
        {
            Default,
            Group,
            IsParent,
            OtherChildrenWithoutGroup,
            WithList,
        }

        public CustomerGridLoader(IApplication app)
        {
            Application = app;
        }

        #region テンプレート作成
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
        #endregion

        #region 全情報取得
        public async Task<IEnumerable<Customer>> SearchInfo()
        {
            List<Customer> list = null;
            CustomersResult result = null;
            await ServiceProxyFactory.LifeTime(async factory =>
            {
                var service = factory.Create<CustomerMasterClient>();
                switch (Key)
                {
                    case SearchCustomer.Group:
                        result = await service.GetCustomerGroupAsync(Application.Login.SessionKey, Application.Login.CompanyId, CustomerId[0]);
                        break;
                    case SearchCustomer.IsParent:
                        result = await service.GetParentItemsAsync(Application.Login.SessionKey, Application.Login.CompanyId);
                        break;
                    case SearchCustomer.OtherChildrenWithoutGroup:
                        result = await service.GetByChildDetailsAsync(Application.Login.SessionKey, Application.Login.CompanyId, CustomerId[0]);
                        break;
                    case SearchCustomer.WithList:
                        result = await service.GetCustomerWithListAsync(Application.Login.SessionKey, Application.Login.CompanyId, CustomerId?.Distinct().ToArray());
                        break;
                    default:
                        result = await service.GetItemsAsync(Application.Login.SessionKey, Application.Login.CompanyId, null);
                        break;
                }

                if (result.ProcessResult.Result)
                    list = result.Customers;
            });
            return list;
        }
        #endregion

        #region 検索情報取得
        public async Task<IEnumerable<Customer>> SearchByKey(params string[] keys)
        {
            var items = await SearchInfo();
            var list = items?.ToList();
            if (list != null)
            {
                list = list.FindAll(
                    f => (!string.IsNullOrEmpty(f.Name) && f.Name.RoughlyContains(keys[0]))
                      || (!string.IsNullOrEmpty(f.Kana) && f.Kana.RoughlyContains(keys[0]))
                      || (!string.IsNullOrEmpty(f.Code) && f.Code.RoughlyContains(keys[0]))
                    );
            }
            return list;
        }
        #endregion
    }
}
