using System.Collections.Generic;
using GrapeCity.Win.MultiRow;
using Rac.VOne.Client.Common;
using Rac.VOne.Client.Common.MultiRow;
using Rac.VOne.Web.Models;
using Rac.VOne.Client.Screen.LoginUserMasterService;
using System.Threading.Tasks;
using LoginUser = Rac.VOne.Web.Models.LoginUser;

namespace Rac.VOne.Client.Screen.Dialogs
{
    public class LoginUserGridLoader : IGridLoader<LoginUser>
    {
        public IApplication Application { get; private set; }
        public SearchLoginUser Key { get; set; }
        private bool onlyUseClient;

        public enum SearchLoginUser
        {
            Default,
            UseClient
        }

        public LoginUserGridLoader(IApplication app,
            bool onlyUseClient = false)
        {
            Application = app;
            this.onlyUseClient = onlyUseClient;
        }

        #region テンプレート作成
        public Template CreateGridTemplate()
        {
            var builder = Application.CreateGcMultirowTemplateBuilder(ColorSetting.Current);
            var rowHeight = builder.DefaultRowHeight;

            builder.Items.AddRange(new CellSetting[]
            {
                new CellSetting(rowHeight,  40, "Header"  , cell: builder.GetRowHeaderCell() ),
                new CellSetting(rowHeight, 115, "Code"    , caption: "ユーザーコード", dataField: nameof(LoginUser.Code)          , cell: builder.GetTextBoxCell(MultiRowContentAlignment.MiddleCenter), sortable: true ),
                new CellSetting(rowHeight, 245, "Name"    , caption: "ユーザー名"    , dataField: nameof(LoginUser.Name)          , cell: builder.GetTextBoxCell()                                     , sortable: true ),
                new CellSetting(rowHeight, 115, "DeptCode", caption: "請求部門コード", dataField: nameof(LoginUser.DepartmentCode), cell: builder.GetTextBoxCell(MultiRowContentAlignment.MiddleCenter), sortable: true ),
                new CellSetting(rowHeight, 204, "DeptName", caption: "請求部門名"    , dataField: nameof(LoginUser.DepartmentName), cell: builder.GetTextBoxCell()                                     , sortable: true ),
                new CellSetting(rowHeight,   0, "Id"      , visible: false           , dataField: nameof(LoginUser.Id) ),
            });

            return builder.Build();
        }
        #endregion

        #region 全情報取得
        public async Task<IEnumerable<LoginUser>> SearchInfo()
        {
            List<LoginUser> list = null;
            await ServiceProxyFactory.LifeTime(async factory =>
            {
                var service = factory.Create<LoginUserMasterClient>();
                LoginUserSearch loginUserSearch;
                if (onlyUseClient)
                {
                    loginUserSearch = new LoginUserSearch { UseClient = 1 };
                }
                else
                {
                    loginUserSearch = new LoginUserSearch();
                }

                UsersResult result = await service.GetItemsAsync(Application.Login.SessionKey,
                    Application.Login.CompanyId, loginUserSearch);

                if (result.ProcessResult.Result)
                    list = result.Users;
            });
            return list;
        }
        #endregion

        #region 検索情報取得
        public async Task<IEnumerable<LoginUser>> SearchByKey(params string[] keys)
        {
            List<LoginUser> list = null;
            await ServiceProxyFactory.LifeTime(async factory =>
            {
                var service = factory.Create<LoginUserMasterClient>();
                UsersResult result = await service.GetItemsAsync(Application.Login.SessionKey,
                    Application.Login.CompanyId, new LoginUserSearch()
                    {
                        UseCommonSearch = true,
                        Name = keys[0]
                    });

                if (result.ProcessResult.Result)
                    list = result.Users;
            });
            return list;
        }
        #endregion
    }
}
