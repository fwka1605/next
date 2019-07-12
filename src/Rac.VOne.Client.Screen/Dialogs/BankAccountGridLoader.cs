using GrapeCity.Win.MultiRow;
using Rac.VOne.Client.Common;
using Rac.VOne.Client.Common.MultiRow;
using Rac.VOne.Client.Screen.BankAccountMasterService;
using Rac.VOne.Web.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Rac.VOne.Client.Screen.Dialogs
{
    public class BankAccountGridLoader : IGridLoader<BankAccount>
    {
        public IApplication Application { get; private set; }

        public BankAccountGridLoader(IApplication app)
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
                new CellSetting(rowHeight, 40 , "Header"         , cell: builder.GetRowHeaderCell() ),
                new CellSetting(rowHeight, 115, "BankCode"       , caption: "銀行コード", dataField: nameof(BankAccount.BankCode)       , cell: builder.GetTextBoxCell(middleCenter), sortable: true ),
                new CellSetting(rowHeight, 185, "BankName"       , caption: "銀行名"    , dataField: nameof(BankAccount.BankName)       , cell: builder.GetTextBoxCell()            , sortable: true ),
                new CellSetting(rowHeight, 115, "BranchCode"     , caption: "支店コード", dataField: nameof(BankAccount.BranchCode)     , cell: builder.GetTextBoxCell(middleCenter), sortable: true ),
                new CellSetting(rowHeight, 185, "BranchName"     , caption: "支店名"    , dataField: nameof(BankAccount.BranchName)     , cell: builder.GetTextBoxCell()            , sortable: true ),
                new CellSetting(rowHeight, 120, "AccountTypeName", caption: "預金種別"  , dataField: nameof(BankAccount.AccountTypeName), cell: builder.GetTextBoxCell()            , sortable: true ),
                new CellSetting(rowHeight, 119, "AccountNumber"  , caption: "口座番号"  , dataField: nameof(BankAccount.AccountNumber)  , cell: builder.GetTextBoxCell()            , sortable: true ),
                new CellSetting(rowHeight, 0  , "Id"             , visible: false, dataField: nameof(BankAccount.Id) ),
                new CellSetting(rowHeight, 0  , "AccountTypeId"  , visible: false, dataField: nameof(BankAccount.AccountTypeId) ),
            });

            return builder.Build();
        }
        #endregion

        #region 全情報取得
        public async Task<IEnumerable<BankAccount>> SearchInfo()
        {
            List<BankAccount> list = null;
            await ServiceProxyFactory.LifeTime(async factory =>
            {
                var service = factory.Create<BankAccountMasterClient>();
                BankAccountsResult result = await service.GetItemsAsync(
                    Application.Login.SessionKey, Application.Login.CompanyId, new BankAccountSearch());

                if (result.ProcessResult.Result)
                    list = result.BankAccounts;
            });
            return list;
        }
        #endregion

        #region 検索情報取得
        public async Task<IEnumerable<BankAccount>> SearchByKey(params string[] keys)
        {
            List<BankAccount> list = null;
            await ServiceProxyFactory.LifeTime(async factory =>
            {
                var service = factory.Create<BankAccountMasterClient>();
                BankAccountsResult result = await service.GetItemsAsync(
                    Application.Login.SessionKey, Application.Login.CompanyId, new BankAccountSearch()
                    {
                        UseCommonSearch = true,
                        BankName = keys[0],
                    });

                if (result.ProcessResult.Result)
                    list = result.BankAccounts;
            });
            return list;
        }
        #endregion
    }
}
