using GrapeCity.Win.MultiRow;
using Rac.VOne.Client.Common;
using Rac.VOne.Client.Common.MultiRow;
using Rac.VOne.Client.Screen.BillingDivisionContractMasterService;
using Rac.VOne.Web.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Rac.VOne.Client.Screen.Dialogs
{
    class BillingDivisionContractGridLoader : IGridLoader<BillingDivisionContract>
    {
        public IApplication Application { get; private set; }
        public int CustomerId { get; set; }

        public BillingDivisionContractGridLoader(IApplication app)
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
                new CellSetting(rowHeight, 40 , "Header"        , cell: builder.GetRowHeaderCell() ),
                new CellSetting(rowHeight, 110, "ContractId"    , caption: "Id"          , dataField: nameof(BillingDivisionContract.Id)            , cell: builder.GetTextBoxCell(), sortable: true ),
                new CellSetting(rowHeight, 110, "ContractNumber", caption: "契約番号"    , dataField: nameof(BillingDivisionContract.ContractNumber), cell: builder.GetTextBoxCell(), sortable: true ),
                new CellSetting(rowHeight, 110, "Monthly"       , caption: "計上サイクル", dataField: nameof(BillingDivisionContract.Monthly)       , cell: builder.GetTextBoxCell(), sortable: true ),
                new CellSetting(rowHeight, 140, "CancelDate"    , caption: "基準日"      , dataField: nameof(BillingDivisionContract.CancelDate)    , cell: builder.GetTextBoxCell(), sortable: true ),
                new CellSetting(rowHeight, 140, "DivisionCount" , caption: "回数"        , dataField: nameof(BillingDivisionContract.DivisionCount) , cell: builder.GetTextBoxCell(MultiRowContentAlignment.MiddleRight), sortable: true ),
                new CellSetting(rowHeight, 0  , "Id"            , visible: false         , dataField: nameof(BillingDivisionContract.Id) ),
            });

            return builder.Build();
        }
        #endregion

        #region 全情報取得
        public async Task<IEnumerable<BillingDivisionContract>> SearchInfo()
        {
            List<BillingDivisionContract> list = null;
            await ServiceProxyFactory.LifeTime(async factory =>
            {
                var service = factory.Create<BillingDivisionContractMasterClient>();
                BillingDivisionContractsResult result = await service.GetItemsAsync(
                    Application.Login.SessionKey, Application.Login.CompanyId, CustomerId);

                if (result.ProcessResult.Result)
                    list = result.BillingDivisionContracts;
            });
            return list;
        }
        #endregion

        #region 検索情報取得
        public async Task<IEnumerable<BillingDivisionContract>> SearchByKey(params string[] keys)
        {
            List<BillingDivisionContract> list = null;
            await ServiceProxyFactory.LifeTime(async factory =>
            {
                var service = factory.Create<BillingDivisionContractMasterClient>();
                BillingDivisionContractsResult result = await service.GetItemsAsync(
                    Application.Login.SessionKey, Application.Login.CompanyId, CustomerId);

                if (result.ProcessResult.Result)
                    list = result.BillingDivisionContracts;
            });
            return list;
        }
        #endregion
    }
}
