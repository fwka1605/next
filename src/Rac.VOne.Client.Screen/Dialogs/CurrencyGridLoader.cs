using GrapeCity.Win.MultiRow;
using Rac.VOne.Client.Common;
using Rac.VOne.Client.Common.MultiRow;
using Rac.VOne.Client.Screen.CurrencyMasterService;
using Rac.VOne.Web.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Rac.VOne.Client.Screen.Dialogs
{
    class CurrencyGridLoader : IGridLoader<Currency>
    {
        public IApplication Application { get; private set; }

        public CurrencyGridLoader(IApplication app)
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
                new CellSetting(rowHeight, 115, "Code"  , caption: "通貨コード", dataField: nameof(Currency.Code), cell: builder.GetTextBoxCell(MultiRowContentAlignment.MiddleCenter), sortable: true ),
                new CellSetting(rowHeight, 284, "Name"  , caption: "名称"      , dataField: nameof(Currency.Name), cell: builder.GetTextBoxCell(), sortable: true ),
                new CellSetting(rowHeight,   0, "Id"    , visible: false       , dataField: nameof(Currency.Id) ),
            });

            return builder.Build();
        }
        #endregion

        #region 全情報取得
        public async Task<IEnumerable<Currency>> SearchInfo()
        {
            List<Currency> list = null;
            await ServiceProxyFactory.LifeTime(async factory =>
            {
                var service = factory.Create<CurrencyMasterClient>();
                CurrenciesResult result = await service.GetItemsAsync(
                    Application.Login.SessionKey, Application.Login.CompanyId, new CurrencySearch());

                if (result.ProcessResult.Result)
                    list = result.Currencies;
            });
            return list;
        }
        #endregion

        #region 検索情報取得
        public async Task<IEnumerable<Currency>> SearchByKey(params string[] keys)
        {
            List<Currency> list = null;
            await ServiceProxyFactory.LifeTime(async factory =>
            {
                var service = factory.Create<CurrencyMasterClient>();
                CurrenciesResult result = await service.GetItemsAsync(
                    Application.Login.SessionKey, Application.Login.CompanyId, new CurrencySearch()
                    {
                        UseCommonSearch = true,
                        Name = keys[0]
                    });

                if (result.ProcessResult.Result)
                    list = result.Currencies;
            });
            return list;
        }
        #endregion
    }
}
