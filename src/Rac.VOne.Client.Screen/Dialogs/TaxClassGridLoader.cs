using System;
using System.Collections.Generic;
using GrapeCity.Win.MultiRow;
using Rac.VOne.Client.Common;
using Rac.VOne.Client.Common.MultiRow;
using Rac.VOne.Web.Models;
using System.Threading.Tasks;
using System.Diagnostics;
using Rac.VOne.Client.Screen.TaxClassMasterService;

namespace Rac.VOne.Client.Screen.Dialogs
{
    public class TaxClassGridLoader : IGridLoader<TaxClass>
    {
        public IApplication Application { get; private set; }

        public TaxClassGridLoader(IApplication app)
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
                new CellSetting(rowHeight, 115, "Code"  , caption: "税区分コード", dataField: nameof(TaxClass.Id)  , cell: builder.GetTextBoxCell(MultiRowContentAlignment.MiddleCenter), sortable: true ),
                new CellSetting(rowHeight, 234, "Name"  , caption: "税区分名"    , dataField: nameof(TaxClass.Name), cell: builder.GetTextBoxCell(), sortable: true ),
                new CellSetting(rowHeight,   0, "Id"    , visible: false         , dataField: nameof(TaxClass.Id) ),
            });

            return builder.Build();
        }
        #endregion

        #region 全情報取得
        public async Task<IEnumerable<TaxClass>> SearchInfo()
        {
            List<TaxClass> list = null;
            await ServiceProxyFactory.LifeTime(async factory =>
            {
                var service = factory.Create<TaxClassMasterClient>();
                TaxClassResult result = await service.GetItemsAsync(Application.Login.SessionKey);

                if (result.ProcessResult.Result)
                    list = result.TaxClass;
            });
            return list;
        }
        #endregion

        #region 検索情報取得
        public async Task<IEnumerable<TaxClass>> SearchByKey(params string[] keys)
        {
            List<TaxClass> list = null;
            await ServiceProxyFactory.LifeTime(async factory =>
            {
                var service = factory.Create<TaxClassMasterClient>();
                TaxClassResult result = await service.GetItemsAsync(Application.Login.SessionKey);

                if (result.ProcessResult.Result)
                    list = result.TaxClass;
            });

            if (list != null)
            {
                list = list.FindAll(
                    f => (!string.IsNullOrEmpty(f.Name) && f.Name.RoughlyContains(keys[0]))
                    );
            }
            return list;
        }
        #endregion
    }
}
