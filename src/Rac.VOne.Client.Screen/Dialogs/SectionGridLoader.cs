using System;
using System.Collections.Generic;
using GrapeCity.Win.MultiRow;
using Rac.VOne.Client.Common;
using Rac.VOne.Client.Common.MultiRow;
using Rac.VOne.Web.Models;
using Rac.VOne.Client.Screen.SectionMasterService;
using System.Threading.Tasks;
using Section = Rac.VOne.Web.Models.Section;

namespace Rac.VOne.Client.Screen.Dialogs
{
    public class SectionGridLoader : IGridLoader<Section>
    {
        public IApplication Application { get; private set; }
        public SearchSection Key { get; set; }

        public enum SearchSection
        {
            Default,
            WithLoginUser
        }

        public SectionGridLoader(IApplication app)
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
                new CellSetting(rowHeight, 115, "Code"  , caption: "入金部門コード", dataField: nameof(Section.Code), cell: builder.GetTextBoxCell(MultiRowContentAlignment.MiddleCenter), sortable: true ),
                new CellSetting(rowHeight, 284, "Name"  , caption: "入金部門名"    , dataField: nameof(Section.Name), cell: builder.GetTextBoxCell(), sortable: true ),
                new CellSetting(rowHeight,   0, "Id"    , visible: false           , dataField: nameof(Section.Id) ),
            });

            return builder.Build();
        }
        #endregion

        #region 全情報取得
        public async Task<IEnumerable<Section>> SearchInfo()
        {
            List<Section> list = null;
            SectionsResult result;
            await ServiceProxyFactory.LifeTime(async factory =>
            {
                var service = factory.Create<SectionMasterClient>();
                switch (Key)
                {
                    case SearchSection.WithLoginUser:
                        result = await service.GetByLoginUserIdAsync(Application.Login.SessionKey, Application.Login.UserId);
                        break;

                    default:
                        result = await service.GetByCodeAsync(Application.Login.SessionKey, Application.Login.CompanyId, Code: null);
                        break;
                }

                if (result.ProcessResult.Result)
                    list = result.Sections;
            });
            return list;
        }
        #endregion

        #region 検索情報取得
        public async Task<IEnumerable<Section>> SearchByKey(params string[] keys)
        {
            List<Section> list = null;
            SectionsResult result;
            await ServiceProxyFactory.LifeTime(async factory =>
            {
                var service = factory.Create<SectionMasterClient>();
                switch (Key)
                {
                    case SearchSection.WithLoginUser:
                        result = await service.GetByLoginUserIdAsync(Application.Login.SessionKey, Application.Login.UserId);
                        break;

                    default:
                        result = await service.GetByCodeAsync(Application.Login.SessionKey, Application.Login.CompanyId, Code: null);
                        break;
                }

                if (result.ProcessResult.Result)
                    list = result.Sections;
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
