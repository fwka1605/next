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
    /// 決済代行会社 学習履歴 インポート処理
    /// </summary>
    public class KanaHistoryPaymentAgencyFileImportProcessor : IKanaHistoryPaymentAgencyFileImportProcessor
    {
        private readonly ICompanyProcessor companyProcessor;
        private readonly ILoginUserProcessor loginUserProcessor;
        private readonly IApplicationControlProcessor applicationControlProcessor;
        private readonly IPaymentAgencyProcessor paymentAgencyProcessor;
        private readonly IKanaHistoryPaymentAgencyProcessor kanaHistoryPaymentAgencyProcessor;

        /// <summary>constructor</summary>
        public KanaHistoryPaymentAgencyFileImportProcessor(
            ICompanyProcessor companyProcessor,
            ILoginUserProcessor loginUserProcessor,
            IApplicationControlProcessor applicationControlProcessor,
            IPaymentAgencyProcessor paymentAgencyProcessor,
            IKanaHistoryPaymentAgencyProcessor kanaHistoryPaymentAgencyProcessor
            )
        {
            this.companyProcessor = companyProcessor;
            this.loginUserProcessor = loginUserProcessor;
            this.applicationControlProcessor = applicationControlProcessor;
            this.paymentAgencyProcessor = paymentAgencyProcessor;
            this.kanaHistoryPaymentAgencyProcessor = kanaHistoryPaymentAgencyProcessor;
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
            var agencyTask = paymentAgencyProcessor.GetAsync(new MasterSearchOption { CompanyId = source.CompanyId, }, token);

            await Task.WhenAll(companyTask, loginUserTask, appConTask, agencyTask);

            var company = companyTask.Result.First();
            var loginUser = loginUserTask.Result.First();
            var appCon = appConTask.Result;
            var agencyDictionary = agencyTask.Result.ToDictionary(x => x.Code);

            var definition = new KanaHistoryPaymentAgencyFileDefinition(new DataExpression(appCon));
            definition.PaymentAgencyIdField.GetModelsByCode = val => agencyDictionary;
            var parser = new CsvParser {
                Encoding        = encoding,
                StreamCreator   = new PlainTextMemoryStreamCreator(),
            };

            var importer = definition.CreateImporter(x => new { x.PayerName, x.SourceBankName, x.SourceBranchName, x.PaymentAgencyId }, parser);
            importer.UserId         = source.LoginUserId;
            importer.UserCode       = loginUser.Code;
            importer.CompanyId      = source.CompanyId;
            importer.CompanyCode    = company.Code;
            importer.LoadAsync = () => kanaHistoryPaymentAgencyProcessor.GetAsync(new KanaHistorySearch { CompanyId = source.CompanyId, }, token);
            importer.RegisterAsync = x => kanaHistoryPaymentAgencyProcessor.ImportAsync(x.New, x.Dirty, x.Removed, token);

            var result = await importer.ImportAsync(csv, mode, token, null);
            result.Logs = importer.GetErrorLogs();

            return result;
        }
    }
}