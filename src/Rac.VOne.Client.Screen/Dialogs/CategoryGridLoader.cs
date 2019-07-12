using GrapeCity.Win.MultiRow;
using Rac.VOne.Client.Common;
using Rac.VOne.Client.Common.MultiRow;
using Rac.VOne.Client.Screen.CategoryMasterService;
using Rac.VOne.Web.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace Rac.VOne.Client.Screen.Dialogs
{
    public class CategoryGridLoader : IGridLoader<Category>
    {
        public IApplication Application { get; private set; }
        private CategorySearch CategorySearch { get; set; }


        public CategoryGridLoader(IApplication app, CategorySearch categorySearch)
        {
            Application = app;
            CategorySearch = categorySearch;
        }

        #region テンプレート作成
        public Template CreateGridTemplate()
        {
            var name = string.Empty;
            switch(CategorySearch.CategoryType)
            {
                case 1: name = "請求";   break;
                case 2: name = "入金";   break;
                case 3: name = "回収";   break;
                case 4: name = "対象外"; break;
                default:                 break;
            }

            var builder = Application.CreateGcMultirowTemplateBuilder(ColorSetting.Default);
            var rowHeight = builder.DefaultRowHeight;

            builder.Items.AddRange(new CellSetting[]
            {
                new CellSetting( rowHeight,  40, "Header", cell: builder.GetRowHeaderCell() ),
                new CellSetting( rowHeight, 115, "Code"  , caption: name + "区分コード", dataField: nameof(Category.Code), sortable: true,  cell: builder.GetTextBoxCell(MultiRowContentAlignment.MiddleCenter) ),
                new CellSetting( rowHeight, 284, "Name"  , caption: name + "区分名"    , dataField: nameof(Category.Name), sortable: true,  cell: builder.GetTextBoxCell() ),
                new CellSetting( rowHeight,   0, "Id"    , visible: false              , dataField: nameof(Category.Id) ),
            });

            return builder.Build();
        }
        #endregion

        #region 全情報取得
        public async Task<IEnumerable<Category>> SearchInfo()
        {
            List<Category> list = null;
            await ServiceProxyFactory.LifeTime(async factory =>
            {
                var service = factory.Create<CategoryMasterClient>();
                CategoriesResult result = await service.GetItemsAsync(Application.Login.SessionKey,
                    CategorySearch);

                if (result.ProcessResult.Result)
                    list = result.Categories;
            });
            if (CategorySearch.SearchPredicate != null)
                list = list.Where(x => CategorySearch.SearchPredicate(x)).ToList();
            return list;
        }
        #endregion

        #region 検索情報取得
        public async Task<IEnumerable<Category>> SearchByKey(params string[] keys)
        {
            List<Category> list = null;
            await ServiceProxyFactory.LifeTime(async factory =>
            {
                var service = factory.Create<CategoryMasterClient>();
                CategorySearch.UseCommonSearch = true;
                CategorySearch.Name = keys[0];
                CategoriesResult result = await service.GetItemsAsync(Application.Login.SessionKey,
                    CategorySearch);

                if (result.ProcessResult.Result)
                    list = result.Categories;
            });
            if (CategorySearch.SearchPredicate != null)
                list = list.Where(x => CategorySearch.SearchPredicate(x)).ToList();
            return list;
        }
        #endregion
    }
}
