using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GrapeCity.Win.MultiRow;
using Rac.VOne.Web.Models;
using Rac.VOne.Client.Common;
using Rac.VOne.Client.Common.MultiRow;
using Rac.VOne.Client.Screen.CompanyMasterService;

namespace Rac.VOne.Client.Screen.Dialogs
{
    public class CompanyGridLoader : IGridLoader<Company>
    {
        public IApplication Application { get; private set; }

        public CompanyGridLoader(IApplication app)
        {
            Application = app;
        }

        public Template CreateGridTemplate()
        {
            var builder = Application.CreateGcMultirowTemplateBuilder(ColorSetting.Current);
            var height = builder.DefaultRowHeight;
            builder.Items.AddRange(new CellSetting[]
            {
                new CellSetting(height,  40, "Header"                                                                    , cell: builder.GetRowHeaderCell() ),
                new CellSetting(height, 115, nameof(Company.Code), caption: "会社コード", dataField: nameof(Company.Code), cell: builder.GetTextBoxCell(MultiRowContentAlignment.MiddleCenter) ),
                new CellSetting(height, 284, nameof(Company.Name), caption: "会社名"    , dataField: nameof(Company.Name), cell: builder.GetTextBoxCell() ),
            });
            return builder.Build();
        }

        public Task<IEnumerable<Company>> SearchByKey(params string[] keys)
        {
            return GetCompanies(keys[0]);
        }

        public Task<IEnumerable<Company>> SearchInfo()
        {
            return GetCompanies(string.Empty);
        }

        private async Task<IEnumerable<Company>> GetCompanies(string name)
        {
            List<Company> result = null;
            await ServiceProxyFactory.LifeTime(async factory =>
            {
                var client = factory.Create<CompanyMasterClient>();
                var serviceResult = await client.GetItemsAsync(Application.Login.SessionKey, name);
                if (serviceResult.ProcessResult.Result)
                    result = serviceResult.Companies;
            });
            return result;
        }
    }
}
