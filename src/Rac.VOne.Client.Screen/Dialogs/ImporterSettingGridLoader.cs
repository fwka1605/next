using GrapeCity.Win.MultiRow;
using Rac.VOne.Client.Common;
using Rac.VOne.Client.Common.MultiRow;
using Rac.VOne.Client.Screen.ImporterSettingService;
using Rac.VOne.Web.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rac.VOne.Client.Screen.Dialogs
{
    public class ImporterSettingGridLoader : IGridLoader<ImporterSetting>
    {
        public IApplication Application { get; private set; }
        public Rac.VOne.Common.Constants.FreeImporterFormatType FormatType { get; set; }

        public ImporterSettingGridLoader(IApplication app)
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
                new CellSetting(rowHeight, 170, "Code"  , caption: "取込パターンコード", dataField: nameof(ImporterSetting.Code), cell: builder.GetTextBoxCell(MultiRowContentAlignment.MiddleCenter), sortable: true ),
                new CellSetting(rowHeight, 429, "Name"  , caption: "取込パターン名"    , dataField: nameof(ImporterSetting.Name), cell: builder.GetTextBoxCell(), sortable: true ),
                new CellSetting(rowHeight,   0, "Id"    , visible: false               , dataField: nameof(ImporterSetting.Id) ),
            });

            return builder.Build();
        }
        #endregion

        #region 全情報取得
        public async Task<IEnumerable<ImporterSetting>> SearchInfo() => await SearchByKey();
        #endregion

        #region 検索情報取得
        public async Task<IEnumerable<ImporterSetting>> SearchByKey(params string[] keys)
        {
            var list = await ServiceProxyFactory.DoAsync(async (ImporterSettingServiceClient client) =>
            {
                var result = await client.GetHeaderAsync(Application.Login.SessionKey, Application.Login.CompanyId, (int)FormatType);
                if (result.ProcessResult.Result)
                    return result.ImporterSettings;
                return null;
            });

            if (list != null && (keys?.Any() ?? false))
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
