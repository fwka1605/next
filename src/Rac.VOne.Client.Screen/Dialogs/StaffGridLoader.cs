using System.Collections.Generic;
using System.Linq;
using GrapeCity.Win.MultiRow;
using Rac.VOne.Client.Common;
using Rac.VOne.Client.Common.MultiRow;
using Rac.VOne.Web.Models;
using Rac.VOne.Client.Screen.StaffMasterService;
using System.Threading.Tasks;

namespace Rac.VOne.Client.Screen.Dialogs
{
    public class StaffGridLoader : IGridLoader<Staff>
    {
        public IApplication Application { get; private set; }
        public SearchStaff Key { get; set; }

        public enum SearchStaff
        {
            Default,
            MaildAddress
        }

        public StaffGridLoader(IApplication app)
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
                new CellSetting(rowHeight,  40, "Header" , cell: builder.GetRowHeaderCell() ),
                new CellSetting(rowHeight, 115, "Code"   , caption: "担当者コード"  , dataField: nameof(Staff.Code)          , cell: builder.GetTextBoxCell(MultiRowContentAlignment.MiddleCenter), sortable: true ),
                new CellSetting(rowHeight, 235, "Name"   , caption: "担当者名"      , dataField: nameof(Staff.Name)          , cell: builder.GetTextBoxCell()                                     , sortable: true ),
                new CellSetting(rowHeight, 115, "DepCode", caption: "請求部門コード", dataField: nameof(Staff.DepartmentCode), cell: builder.GetTextBoxCell(MultiRowContentAlignment.MiddleCenter), sortable: true ),
                new CellSetting(rowHeight, 234, "DepName", caption: "請求部門名"    , dataField: nameof(Staff.DepartmentName), cell: builder.GetTextBoxCell()                                     , sortable: true ),
                new CellSetting(rowHeight,   0, "Id"     , visible: false           , dataField: nameof(Staff.Id) ),
            });

            return builder.Build();
        }
        #endregion

        #region 全情報取得

        public async Task<IEnumerable<Staff>> SearchInfo() => await SearchByKey();

        #endregion

        #region 検索情報取得

        public async Task<IEnumerable<Staff>> SearchByKey(params string[] keys)
            => await ServiceProxyFactory.DoAsync(async (StaffMasterClient client) =>
            {
                var option = new StaffSearch {
                    CompanyId = Application.Login.CompanyId,
                    UseCommonSearch = true,
                };
                if (keys?.Any() ?? false) option.Name = keys.First();
                var result = await client.GetItemsAsync(Application.Login.SessionKey, option);
                if (result.ProcessResult.Result)
                    return result.Staffs;
                return null;
            });

        #endregion
    }
}
