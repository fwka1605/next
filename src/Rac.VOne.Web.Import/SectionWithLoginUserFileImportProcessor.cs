using Rac.VOne.Common.DataHandling;
using Rac.VOne.Import;
using Rac.VOne.Web.Common;
using Rac.VOne.Web.Common.Importers;
using Rac.VOne.Web.Models;
using Rac.VOne.Web.Models.Files;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Import
{
    /// <summary>
    /// 請求部門・ログインユーザー関連マスター インポート処理
    /// </summary>
    public class SectionWithLoginUserFileImportProcessor : ISectionWithLoginUserFileImportProcessor
    {
        private readonly ICompanyProcessor companyProcessor;
        private readonly ILoginUserProcessor loginUserProcessor;
        private readonly IApplicationControlProcessor applicationControlProcessor;
        private readonly ISectionProcessor sectionProcessor;
        private readonly ISectionWithLoginUserProcessor sectionWithLoginUserProcessor;

        /// <summary>constructor</summary>
        public SectionWithLoginUserFileImportProcessor(
            ICompanyProcessor companyProcessor,
            ILoginUserProcessor loginUserProcessor,
            IApplicationControlProcessor applicationControlProcessor,
            ISectionProcessor sectionProcessor,
            ISectionWithLoginUserProcessor sectionWithLoginUserProcessor
            )
        {
            this.companyProcessor = companyProcessor;
            this.loginUserProcessor = loginUserProcessor;
            this.applicationControlProcessor = applicationControlProcessor;
            this.sectionProcessor = sectionProcessor;
            this.sectionWithLoginUserProcessor = sectionWithLoginUserProcessor;
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
            var loginUsersTask = loginUserProcessor.GetAsync(new LoginUserSearch{ CompanyId = source.CompanyId, }, token);
            var appConTask = applicationControlProcessor.GetAsync(source.CompanyId, token);
            var sectionTask = sectionProcessor.GetAsync(new SectionSearch { CompanyId = source.CompanyId, }, token);

            await Task.WhenAll(companyTask, loginUsersTask, appConTask, sectionTask);

            var company = companyTask.Result.First();
            var loginUserDictionary = loginUsersTask.Result.ToDictionary(x => x.Code);
            var loginUser = loginUsersTask.Result.First(x => x.Id == source.LoginUserId);
            var appCon = appConTask.Result;
            var sectionDictionary = sectionTask.Result.ToDictionary(x => x.Code);

            var definition = new SectionWithLoginUserFileDefinition(new DataExpression(appCon));
            var parser = new CsvParser {
                Encoding        = encoding,
                StreamCreator   = new PlainTextMemoryStreamCreator(),
            };
            definition.SectionCodeField.GetModelsByCode = codes => sectionDictionary;
            definition.LoginUserCodeField.GetModelsByCode = codes => loginUserDictionary;

            var importer = definition.CreateImporter(x => new { x.SectionId, x.LoginUserId }, parser);
            importer.UserId         = source.LoginUserId;
            importer.UserCode       = loginUser.Code;
            importer.CompanyId      = source.CompanyId;
            importer.CompanyCode    = company.Code;

            importer.LoadAsync = () => sectionWithLoginUserProcessor.GetAsync(new SectionWithLoginUserSearch { CompanyId = source.CompanyId, }, token);
            importer.RegisterAsync = x => sectionWithLoginUserProcessor.ImportAsync(x.New, x.Dirty, x.Removed, token);

            var result = await importer.ImportAsync(csv, mode, token, null);
            result.Logs = importer.GetErrorLogs();

            return result;
        }
    }
}