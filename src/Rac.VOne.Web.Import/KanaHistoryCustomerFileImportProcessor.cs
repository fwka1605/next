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
    /// <summary>
    /// 得意先学習履歴 インポート処理
    /// </summary>
    public class KanaHistoryCustomerFileImportProcessor : IKanaHistoryCustomerFileImportProcessor
    {
        private readonly ICompanyProcessor companyProcessor;
        private readonly ILoginUserProcessor loginUserProcessor;
        private readonly IApplicationControlProcessor applicationControlProcessor;
        private readonly ICustomerProcessor customerProcessor;
        private readonly IKanaHistoryCustomerProcessor kanaHistoryCustomerProcessor;

        /// <summary>constructor</summary>
        public KanaHistoryCustomerFileImportProcessor(
            ICompanyProcessor companyProcessor,
            ILoginUserProcessor loginUserProcessor,
            IApplicationControlProcessor applicationControlProcessor,
            ICustomerProcessor customerProcessor,
            IKanaHistoryCustomerProcessor kanaHistoryCustomerProcessor
            )
        {
            this.companyProcessor = companyProcessor;
            this.loginUserProcessor = loginUserProcessor;
            this.applicationControlProcessor = applicationControlProcessor;
            this.customerProcessor = customerProcessor;
            this.kanaHistoryCustomerProcessor = kanaHistoryCustomerProcessor;
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
            var customerTask = customerProcessor.GetAsync(new CustomerSearch { CompanyId = source.CompanyId, }, token);

            await Task.WhenAll(companyTask, loginUserTask, appConTask, customerTask);

            var company = companyTask.Result.First();
            var loginUser = loginUserTask.Result.First();
            var appCon = appConTask.Result;
            var customerDictionary = customerTask.Result.ToDictionary(x => x.Code);

            var definition = new KanaHistoryCustomerFileDefinition(new DataExpression(appCon));
            definition.CustomerIdField.GetModelsByCode = val => customerDictionary;
            var parser = new CsvParser {
                Encoding        = encoding,
                StreamCreator   = new PlainTextMemoryStreamCreator(),
            };

            var importer = definition.CreateImporter(x => new { x.PayerName, x.SourceBankName, x.SourceBranchName, x.CustomerId }, parser);
            importer.UserId         = source.LoginUserId;
            importer.UserCode       = loginUser.Code;
            importer.CompanyId      = source.CompanyId;
            importer.CompanyCode    = company.Code;
            importer.LoadAsync = () => kanaHistoryCustomerProcessor.GetAsync(new KanaHistorySearch { CompanyId = source.CompanyId, }, token);
            importer.RegisterAsync = x => kanaHistoryCustomerProcessor.ImportAsync(x.New, x.Dirty, x.Removed, token);

            var result = await importer.ImportAsync(csv, mode, token, null);
            result.Logs = importer.GetErrorLogs();

            return result;
        }
    }
}