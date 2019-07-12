using GrapeCity.Win.MultiRow;
using Rac.VOne.Client.Common;
using Rac.VOne.Client.Common.MultiRow;
using Rac.VOne.Client.Screen.BankBranchMasterService;
using Rac.VOne.Web.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Rac.VOne.Client.Screen.Dialogs
{
    public class BankBranchGridLoader : IGridLoader<BankBranch>
    {
        public IApplication Application { get; private set; }

        public BankBranchGridLoader(IApplication app)
        {
            Application = app;
        }

        #region テンプレート作成
        public Template CreateGridTemplate()
        {
            var builder = Application.CreateGcMultirowTemplateBuilder(ColorSetting.Current);
            var rowHeight = builder.DefaultRowHeight;
            var middleCenter = MultiRowContentAlignment.MiddleCenter;
            builder.Items.AddRange(new CellSetting[]
            {
                new CellSetting(rowHeight, 40 , "Header"    , cell: builder.GetRowHeaderCell() ),
                new CellSetting(rowHeight, 115, "BankCode"  , caption: "銀行コード", dataField: nameof(BankBranch.BankCode)  , cell: builder.GetTextBoxCell(middleCenter), sortable: true ),
                new CellSetting(rowHeight, 150, "BankKana"  , caption: "銀行名カナ", dataField: nameof(BankBranch.BankKana)  , cell: builder.GetTextBoxCell()            , sortable: true ),
                new CellSetting(rowHeight, 155, "BankName"  , caption: "銀行名"    , dataField: nameof(BankBranch.BankName)  , cell: builder.GetTextBoxCell()            , sortable: true ),
                new CellSetting(rowHeight, 115, "BranchCode", caption: "支店コード", dataField: nameof(BankBranch.BranchCode), cell: builder.GetTextBoxCell(middleCenter), sortable: true ),
                new CellSetting(rowHeight, 150, "BranchKana", caption: "支店名カナ", dataField: nameof(BankBranch.BranchKana), cell: builder.GetTextBoxCell()            , sortable: true ),
                new CellSetting(rowHeight, 154, "BranchName", caption: "支店名"    , dataField: nameof(BankBranch.BranchName), cell: builder.GetTextBoxCell()            , sortable: true ),
                new CellSetting(rowHeight, 0  , "Id"        , visible: false       , dataField: nameof(BankBranch.BankCode) ),
            });

            return builder.Build();
        }
        #endregion

        #region 全情報取得
        public async Task<IEnumerable<BankBranch>> SearchInfo()
        {
            List<BankBranch> list = null;
            await ServiceProxyFactory.LifeTime(async factory =>
            {
                var service = factory.Create<BankBranchMasterClient>();
                BankBranchsResult result = await service.GetItemsAsync(
                    Application.Login.SessionKey, Application.Login.CompanyId, new BankBranchSearch());

                if (result.ProcessResult.Result)
                    list = result.BankBranches;
            });
            return list;
        }
        #endregion

        #region 検索情報取得
        public async Task<IEnumerable<BankBranch>> SearchByKey(params string[] keys)
        {
            List<BankBranch> list = null;
            await ServiceProxyFactory.LifeTime(async factory =>
            {
                var service = factory.Create<BankBranchMasterClient>();
                BankBranchsResult result = await service.GetItemsAsync(
                    Application.Login.SessionKey, Application.Login.CompanyId, new BankBranchSearch()
                    {
                        UseCommonSearch = true,
                        BankName = keys[0],
                        BranchName = keys[1]
                    });

                if (result.ProcessResult.Result)
                    list = result.BankBranches;
            });
            return list;
        }
        #endregion
    }
}
