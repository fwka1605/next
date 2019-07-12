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
using System.Web;

namespace Rac.VOne.Web.Import
{
    /// <summary>
    /// 銀行口座マスター インポート処理
    /// </summary>
    public class BankAccountFileImportProcessor : IBankAccountFileImportProcessor
    {
        private readonly ICompanyProcessor companyProcessor;
        private readonly ILoginUserProcessor loginUserProcessor;
        private readonly IApplicationControlProcessor applicationControlProcessor;
        private readonly ICategoryProcessor categoryProcessor;
        private readonly ISectionProcessor sectionProcessor;
        private readonly IBankAccountProcessor bankAccountProcessor;

        /// <summary>constructor</summary>
        public BankAccountFileImportProcessor(
            ICompanyProcessor companyProcessor,
            ILoginUserProcessor loginUserProcessor,
            IApplicationControlProcessor applicationControlProcessor,
            ICategoryProcessor categoryProcessor,
            ISectionProcessor sectionProcessor,
            IBankAccountProcessor bankAccountProcessor
            )
        {
            this.companyProcessor = companyProcessor;
            this.loginUserProcessor = loginUserProcessor;
            this.applicationControlProcessor = applicationControlProcessor;
            this.categoryProcessor = categoryProcessor;
            this.sectionProcessor = sectionProcessor;
            this.bankAccountProcessor = bankAccountProcessor;
        }

        /// <summary>
        /// インポート処理
        /// </summary>
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
            var categoryTask = categoryProcessor.GetAsync(new CategorySearch { CompanyId = source.CompanyId, CategoryType = Rac.VOne.Common.CategoryType.Receipt, }, token);
            var sectionTask = sectionProcessor.GetAsync(new SectionSearch { CompanyId = source.CompanyId, }, token);

            await Task.WhenAll(companyTask, loginUserTask, appConTask, categoryTask, sectionTask);

            var company = companyTask.Result.First();
            var loginUser = loginUserTask.Result.First();
            var appCon = appConTask.Result;
            var categoryDictionary = categoryTask.Result.ToDictionary(x => x.Code);
            var sectionDictionary = sectionTask.Result.ToDictionary(x => x.Code);

            var definition = new BankAccountFileDefinition(new DataExpression(appCon));
            var parser = new CsvParser {
                Encoding        = encoding,
                StreamCreator   = new PlainTextMemoryStreamCreator(),
            };

            definition.CategoryIdField.GetModelsByCode = val => categoryDictionary;
            definition.SectionIdField.Ignored = appCon.UseReceiptSection == 0;
            definition.SectionIdField.GetModelsByCode = val => sectionDictionary;

            var importer = definition.CreateImporter(x => new {
                x.BankCode,
                x.BranchCode,
                x.AccountTypeId,
                x.AccountNumber,
            }, parser);
            importer.UserId         = source.LoginUserId;
            importer.UserCode       = loginUser.Code;
            importer.CompanyId      = source.CompanyId;
            importer.CompanyCode    = company.Code;
            importer.LoadAsync = () => bankAccountProcessor.GetAsync(new BankAccountSearch { CompanyId = source.CompanyId, }, token);
            importer.RegisterAsync = x => bankAccountProcessor.ImportAsync(x.New, x.Dirty, x.Removed, token);

            var result = await importer.ImportAsync(csv, mode, token, null);
            result.Logs = importer.GetErrorLogs();

            return result;
        }
    }
}