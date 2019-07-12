using Rac.VOne.Common;
using Rac.VOne.Common.DataHandling;
using Rac.VOne.Import;
using Rac.VOne.Web.Common;
using Rac.VOne.Web.Common.Importers;
using Rac.VOne.Web.Models;
using Rac.VOne.Web.Models.Files;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Import
{
    /// <summary>除外カナマスター インポート処理</summary>
    public class IgnoreKanaFileImportProcessor : IIgnoreKanaFileImportProcessor
    {
        private readonly ICompanyProcessor companyProcessor;
        private readonly ILoginUserProcessor loginUserProcessor;
        private readonly IApplicationControlProcessor applicationControlProcessor;
        private readonly ICategoryProcessor categoryProcessor;
        private readonly IIgnoreKanaProcessor ignoreKanaProcessor;

        /// <summary>constructor</summary>
        public IgnoreKanaFileImportProcessor(
            ICompanyProcessor companyProcessor,
            ILoginUserProcessor loginUserProcessor,
            IApplicationControlProcessor applicationControlProcessor,
            ICategoryProcessor categoryProcessor,
            IIgnoreKanaProcessor ignoreKanaProcessor
            )
        {
            this.companyProcessor = companyProcessor;
            this.loginUserProcessor = loginUserProcessor;
            this.applicationControlProcessor = applicationControlProcessor;
            this.categoryProcessor = categoryProcessor;
            this.ignoreKanaProcessor = ignoreKanaProcessor;
        }

        /// <summary>インポート処理</summary>
        /// <param name="source"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<ImportResult> ImportAsync(MasterImportSource source, CancellationToken token = default(CancellationToken))
        {
            var mode = (ImportMethod)source.ImportMethod;
            var encoding = Encoding.GetEncoding(source.EncodingCodePage);
            var csv = encoding.GetString(source.Data);

            var companyTask = companyProcessor.GetAsync(new CompanySearch { Id = source.CompanyId, }, token);
            var loginUserTask = loginUserProcessor.GetAsync(new LoginUserSearch { Ids = new[] { source.LoginUserId }, }, token);
            var appConTask = applicationControlProcessor.GetAsync(source.CompanyId, token);
            var categoryTask = categoryProcessor.GetAsync(new CategorySearch { CompanyId = source.CompanyId, CategoryType = CategoryType.Exclude, }, token);

            await Task.WhenAll(companyTask, loginUserTask, appConTask, categoryTask);

            var company = companyTask.Result.First();
            var loginUser = loginUserTask.Result.First();
            var appCon = appConTask.Result;
            var categoryDictionary = categoryTask.Result.ToDictionary(x => x.Code);

            var definition = new IgnoreKanaFileDefinition(new DataExpression(appCon));
            var parser = new CsvParser {
                Encoding        = encoding,
                StreamCreator   = new PlainTextMemoryStreamCreator(),
            };

            definition.ExcludeCategoryIdField.GetModelsByCode = val => categoryDictionary;

            var importer = definition.CreateImporter(m => m.Kana, parser);
            importer.UserId         = source.LoginUserId;
            importer.UserCode       = loginUser.Code;
            importer.CompanyId      = source.CompanyId;
            importer.CompanyCode    = company.Code;
            importer.LoadAsync = () => ignoreKanaProcessor.GetAsync(new IgnoreKana { CompanyId = source.CompanyId, }, token);
            importer.RegisterAsync = x => ignoreKanaProcessor.ImportAsync(x.New, x.Dirty, x.Removed, token);

            var result = await importer.ImportAsync(csv, mode, token, null);
            result.Logs = importer.GetErrorLogs();

            return result;
        }
    }
}