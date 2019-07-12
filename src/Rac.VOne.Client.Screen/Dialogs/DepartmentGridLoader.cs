using System;
using System.Collections.Generic;
using GrapeCity.Win.MultiRow;
using Rac.VOne.Client.Common;
using Rac.VOne.Client.Common.MultiRow;
using Rac.VOne.Web.Models;
using Rac.VOne.Client.Screen.DepartmentMasterService;
using System.Threading.Tasks;
using System.Linq;

namespace Rac.VOne.Client.Screen.Dialogs
{
    public class DepartmentGridLoader : IGridLoader<Department>
    {
        public IApplication Application { get; private set; }
        public SearchDepartment Key { get; set; }
        public int[] DepartmentId { get; set; }
        public int SectionId { get; set; }

        public enum SearchDepartment
        {
            Default,
            WithSection,
        }

        public DepartmentGridLoader(IApplication app)
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
                new CellSetting(rowHeight, 115, "Code"  , caption: "請求部門コード", dataField: nameof(Department.Code), cell: builder.GetTextBoxCell(MultiRowContentAlignment.MiddleCenter), sortable: true ),
                new CellSetting(rowHeight, 284, "Name"  , caption: "請求部門名"    , dataField: nameof(Department.Name), cell: builder.GetTextBoxCell(), sortable: true ),
                new CellSetting(rowHeight, 0  , "Id"    , visible: false           , dataField: nameof(Department.Id) )
            });

            return builder.Build();
        }
        #endregion

        #region 全情報取得
        public async Task<IEnumerable<Department>> SearchInfo()
        {
            List<Department> list = null;
            DepartmentsResult result = null;
            await ServiceProxyFactory.LifeTime(async factory =>
            {
                var service = factory.Create<DepartmentMasterClient>();
                switch (Key)
                {
                    case SearchDepartment.WithSection:
                        result = await service.DepartmentWithSectionAsync(Application.Login.SessionKey, Application.Login.CompanyId,
                            SectionId, DepartmentId?.Distinct().ToArray());
                        break;

                    default:
                        result = await service.GetItemsAsync(Application.Login.SessionKey, Application.Login.CompanyId);
                        break;
                }

                if (result.ProcessResult.Result)
                    list = result.Departments;
            });
            return list;
        }
        #endregion

        #region 検索情報取得
        public async Task<IEnumerable<Department>> SearchByKey(params string[] keys)
        {
            List<Department> list = null;
            DepartmentsResult result = null;
            await ServiceProxyFactory.LifeTime(async factory =>
            {
                var service = factory.Create<DepartmentMasterClient>();
                switch (Key)
                {
                    case SearchDepartment.WithSection:
                        result = await service.DepartmentWithSectionAsync(Application.Login.SessionKey, Application.Login.CompanyId,
                            SectionId, DepartmentId?.Distinct().ToArray());
                        break;

                    default:
                        result = await service.GetItemsAsync(Application.Login.SessionKey, Application.Login.CompanyId);
                        break;
                }

                if (result.ProcessResult.Result)
                    list = result.Departments;
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
