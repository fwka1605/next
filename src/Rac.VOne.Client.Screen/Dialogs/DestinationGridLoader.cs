using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rac.VOne.Web.Models;
using GrapeCity.Win.MultiRow;
using Rac.VOne.Client.Common;
using Rac.VOne.Client.Common.MultiRow;
using Rac.VOne.Client.Screen.DestinationMasterService;

namespace Rac.VOne.Client.Screen.Dialogs
{
    public class DestinationGridLoader : IGridLoader<Destination>
    {
        public IApplication Application { get; private set; }

        public DestinationSearch DestinationSearch { get; set; }

        public DestinationGridLoader(IApplication application, DestinationSearch option)
        {
            Application = application;
            DestinationSearch = option;
        }

        public Template CreateGridTemplate()
        {
            var builder = Application.CreateGcMultirowTemplateBuilder(ColorSetting.Default);
            var height = builder.DefaultRowHeight;
            builder.Items.AddRange(new[] {
                new CellSetting(height,  40, "Header"                            , cell: builder.GetRowHeaderCell() ),
                new CellSetting(height,  90, nameof(Destination.Code)            , nameof(Destination.Code)            , "送付先コード", sortable: true ),
                new CellSetting(height, 100, nameof(Destination.Name)            , nameof(Destination.Name)            , "送付先名", sortable: true ),
                new CellSetting(height, 100, nameof(Destination.DepartmentName)  , nameof(Destination.DepartmentName)  , "部署"        , sortable: true ),
                new CellSetting(height, 100, nameof(Destination.Addressee)       , nameof(Destination.Addressee)       , "宛名"        , sortable: true ),
                new CellSetting(height,  40, nameof(Destination.Honorific)       , nameof(Destination.Honorific)       , "敬称"        , sortable: true ),
                new CellSetting(height,  60, nameof(Destination.PostalCode)      , nameof(Destination.PostalCode)      , "郵便番号"    , sortable: true ),
                new CellSetting(height, 100, nameof(Destination.Address1)        , nameof(Destination.Address1)        , "住所1"       , sortable: true ),
                new CellSetting(height, 100, nameof(Destination.Address2)        , nameof(Destination.Address2)        , "住所2"       , sortable: true ),
            });
            return builder.Build();
        }

        public async Task<IEnumerable<Destination>> SearchByKey(params string[] keys)
        {
            var option = DestinationSearch;
            if (keys.Any()) option.Name = keys[0];
            var result = await ServiceProxyFactory.DoAsync(async (DestinationMasterClient client) => await client.GetItemsAsync(Application.Login.SessionKey, option));
            if (result.ProcessResult.Result)
                return result.Destinations;
            return new List<Destination>();
        }

        public async Task<IEnumerable<Destination>> SearchInfo() => await SearchByKey();
    }
}
