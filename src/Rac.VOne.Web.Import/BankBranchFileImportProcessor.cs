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
    /// <summary>銀行支店マスター インポート処理</summary>
    public class BankBranchFileImportProcessor : IBankBranchFileImportProcessor
    {
        private readonly ICompanyProcessor companyProcessor;
        private readonly ILoginUserProcessor loginUserProcessor;
        private readonly IApplicationControlProcessor applicationControlProcessor;
        private readonly IBankBranchProcessor bankBranchProcessor;

        /// <summary>constructor</summary>
        public BankBranchFileImportProcessor(
            ICompanyProcessor companyProcessor,
            ILoginUserProcessor loginUserProcessor,
            IApplicationControlProcessor applicationControlProcessor,
            IBankBranchProcessor bankBranchProcessor
            )
        {
            this.companyProcessor = companyProcessor;
            this.loginUserProcessor = loginUserProcessor;
            this.applicationControlProcessor = applicationControlProcessor;
            this.bankBranchProcessor = bankBranchProcessor;
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
            var loginUsersTask = loginUserProcessor.GetAsync(new LoginUserSearch { CompanyId = source.CompanyId, }, token);
            var appConTask = applicationControlProcessor.GetAsync(source.CompanyId, token);

            await Task.WhenAll(companyTask, loginUsersTask, appConTask);

            var company = companyTask.Result.First();
            var loginUserDictionary = loginUsersTask.Result.ToDictionary(x => x.Code);
            var loginUser = loginUsersTask.Result.First(x => x.Id == source.LoginUserId);
            var appCon = appConTask.Result;

            var definition = new BankBranchFileDefinition(new DataExpression(appCon));
            var parser = new CsvParser {
                Encoding        = encoding,
                StreamCreator   = new PlainTextMemoryStreamCreator(),
            };


            var importer = definition.CreateImporter(x => new { x.BankCode, x.BranchCode }, parser);
            importer.UserId         = source.LoginUserId;
            importer.UserCode       = loginUser.Code;
            importer.CompanyId      = source.CompanyId;
            importer.CompanyCode    = company.Code;

            importer.LoadAsync = () => bankBranchProcessor.GetAsync(new BankBranchSearch { CompanyId = source.CompanyId, }, token);
            importer.RegisterAsync = async x => {
                foreach (var y in x.New     ) y.CompanyId = source.CompanyId;
                foreach (var y in x.Dirty   ) y.CompanyId = source.CompanyId;
                foreach (var y in x.Removed ) y.CompanyId = source.CompanyId;
                return await bankBranchProcessor.ImportAsync(x.New, x.Dirty, x.Removed, token);
            };

            var result = await importer.ImportAsync(csv, mode, token, null);
            result.Logs = importer.GetErrorLogs();

            return result;
        }
    }
}