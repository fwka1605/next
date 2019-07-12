using System;
using System.Collections.Generic;
using GrapeCity.Win.MultiRow;
using Rac.VOne.Client.Common;
using Rac.VOne.Client.Common.MultiRow;
using Rac.VOne.Web.Models;
using Rac.VOne.Client.Screen.PaymentAgencyMasterService;
using System.Threading.Tasks;

namespace Rac.VOne.Client.Screen.Dialogs
{
    public class PaymentAgencyGridLoader : IGridLoader<PaymentAgency>
    {
        public IApplication Application { get; private set; }

        public PaymentAgencyGridLoader(IApplication app)
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
                new CellSetting(rowHeight, 150, "Code"  , caption: "決済代行会社コード", dataField: nameof(PaymentAgency.Code), cell: builder.GetTextBoxCell(MultiRowContentAlignment.MiddleCenter), sortable: true ),
                new CellSetting(rowHeight, 225, "Name"  , caption: "決済代行会社名"    , dataField: nameof(PaymentAgency.Name), cell: builder.GetTextBoxCell(), sortable: true ),
                new CellSetting(rowHeight, 224, "Kana"  , caption: "決済代行会社カナ"  , dataField: nameof(PaymentAgency.Kana), cell: builder.GetTextBoxCell(), sortable: true ),
                new CellSetting(rowHeight,   0, "Id"    , visible: false               , dataField: nameof(PaymentAgency.Id)  , cell: builder.GetTextBoxCell() )
            });

            return builder.Build();
        }
        #endregion

        #region 全情報取得
        public async Task<IEnumerable<PaymentAgency>> SearchInfo()
        {
            List<PaymentAgency> list = null;
            await ServiceProxyFactory.LifeTime(async factory =>
            {
                var service = factory.Create<PaymentAgencyMasterClient>();
                PaymentAgenciesResult result = await service.GetItemsAsync(Application.Login.SessionKey, Application.Login.CompanyId);

                if (result.ProcessResult.Result)
                    list = result.PaymentAgencies;
            });
            return list;
        }
        #endregion

        #region 検索情報取得
        public async Task<IEnumerable<PaymentAgency>> SearchByKey(params string[] keys)
        {
            List<PaymentAgency> list = null;
            await ServiceProxyFactory.LifeTime(async factory =>
            {
                var service = factory.Create<PaymentAgencyMasterClient>();
                PaymentAgenciesResult result = await service.GetItemsAsync(Application.Login.SessionKey, Application.Login.CompanyId);

                if (result.ProcessResult.Result)
                    list = result.PaymentAgencies;
            });

            if (list != null)
            {
                list = list.FindAll(
                    f => (!string.IsNullOrEmpty(f.Name) && f.Name.RoughlyContains(keys[0]))
                      || (!string.IsNullOrEmpty(f.Kana) && f.Kana.RoughlyContains(keys[0]))
                    );
            }
            return list;
        }
        #endregion
    }
}
