using System;
using System.Collections.Generic;
using GrapeCity.Win.MultiRow;
using Rac.VOne.Client.Common.MultiRow;
using System.Threading.Tasks;
using Rac.VOne.Web.Models;
using Rac.VOne.Client.Common;

namespace Rac.VOne.Client.Screen.Dialogs
{
    class ShareTransferFeeGridLoader : IGridLoader<FixedValue>
    {
        public IApplication Application { get; private set; }
        private List<FixedValue> list { get; set; } = new List<FixedValue> {
            new FixedValue("0", "相手先"),
            new FixedValue("1", "自社"),
        };

        public ShareTransferFeeGridLoader(IApplication app)
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
                new CellSetting(rowHeight, 170, "Code"  , caption: "手数料負担区分"  , dataField: nameof(FixedValue.Key)  , sortable: true ),
                new CellSetting(rowHeight, 229, "Name"  , caption: "手数料負担区分名", dataField: nameof(FixedValue.Value), sortable: true ),
            });

            return builder.Build();
        }
        #endregion

        #region 全情報取得
        public Task<IEnumerable<FixedValue>> SearchInfo()
        {
            var source = new TaskCompletionSource<IEnumerable<FixedValue>>();
            source.SetResult(list);
            return source.Task;
        }
        #endregion

        #region 検索情報取得
        public Task<IEnumerable<FixedValue>> SearchByKey(params string[] keys)
        {
            var source = new TaskCompletionSource<IEnumerable<FixedValue>>();
            List<FixedValue> result = list.FindAll(
                f => (!string.IsNullOrEmpty(f.Value) && f.Value.RoughlyContains(keys[0]))
                );
            source.SetResult(result);
            return source.Task;
        }
        #endregion
    }
}
