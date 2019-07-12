using System.Collections.Generic;
using GrapeCity.Win.MultiRow;
using Rac.VOne.Client.Common;
using Rac.VOne.Client.Common.MultiRow;
using Rac.VOne.Web.Models;
using Rac.VOne.Client.Screen.AccountTitleMasterService;
using System.Threading.Tasks;

namespace Rac.VOne.Client.Screen.Dialogs
{
    public class AccountTitleGridLoader : IGridLoader<AccountTitle>
    {
        public IApplication Application { get; private set; }

        public AccountTitleGridLoader(IApplication app)
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
                new CellSetting(rowHeight, 40 , "Header", cell: builder.GetRowHeaderCell() ),
                new CellSetting(rowHeight, 115, "Code"  , caption: "科目コード", dataField: nameof(AccountTitle.Code), sortable: true, cell: builder.GetTextBoxCell(MultiRowContentAlignment.MiddleCenter) ),
                new CellSetting(rowHeight, 284, "Name"  , caption: "科目名"    , dataField: nameof(AccountTitle.Name), sortable: true ),
                new CellSetting(rowHeight, 0  , "Id"    , caption: "Id"        , dataField: nameof(AccountTitle.Id)  , visible: false )
            });
            return builder.Build();
        }
        #endregion

        #region 全情報取得
        public async Task<IEnumerable<AccountTitle>> SearchInfo()
        {
            List<AccountTitle> list = null;
            await ServiceProxyFactory.LifeTime(async factory =>
            {
                var service = factory.Create<AccountTitleMasterClient>();
                AccountTitlesResult result = await service.GetItemsAsync(
                Application.Login.SessionKey, new AccountTitleSearch { CompanyId = Application.Login.CompanyId, });

                if (result.ProcessResult.Result)
                    list = result.AccountTitles;
            });
            return list;
        }
        #endregion

        #region 検索情報取得
        public async Task<IEnumerable<AccountTitle>> SearchByKey(params string[] keys)
        {
            List<AccountTitle> list = null;
            await ServiceProxyFactory.LifeTime(async factory =>
            {
                var service = factory.Create<AccountTitleMasterClient>();
                AccountTitlesResult result = await service.GetItemsAsync(
                    Application.Login.SessionKey,
                    new AccountTitleSearch()
                    {
                        CompanyId = Application.Login.CompanyId,
                        UseCommonSearch = true,
                        Name = keys[0]
                    });

                if (result.ProcessResult.Result)
                    list = result.AccountTitles;
            });
            return list;
        }
        #endregion
    }
}
