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
    /// 請求・入金部門関連マスター インポート処理
    /// </summary>
    public class SectionWithDepartmentFileImportProcessor : ISectionWithDepartmentFileImportProcessor
    {
        private readonly ICompanyProcessor companyProcessor;
        private readonly ILoginUserProcessor loginUserProcessor;
        private readonly IApplicationControlProcessor applicationControlProcessor;
        private readonly ISectionProcessor sectionProcessor;
        private readonly IDepartmentProcessor departmentProcessor;
        private readonly ISectionWithDepartmentProcessor sectionWithDepartmentProcessor;
        /// <summary>constructor</summary>
        public SectionWithDepartmentFileImportProcessor(
            ICompanyProcessor companyProcessor,
            ILoginUserProcessor loginUserProcessor,
            IApplicationControlProcessor applicationControlProcessor,
            ISectionProcessor sectionProcessor,
            IDepartmentProcessor departmentProcessor,
            ISectionWithDepartmentProcessor sectionWithDepartmentProcessor
            )
        {
            this.companyProcessor = companyProcessor;
            this.loginUserProcessor = loginUserProcessor;
            this.applicationControlProcessor = applicationControlProcessor;
            this.sectionProcessor = sectionProcessor;
            this.departmentProcessor = departmentProcessor;
            this.sectionWithDepartmentProcessor = sectionWithDepartmentProcessor;
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
            var sectionTask = sectionProcessor.GetAsync(new SectionSearch { CompanyId = source.CompanyId, }, token);
            var departmentTask = departmentProcessor.GetAsync(new DepartmentSearch { CompanyId = source.CompanyId, }, token);

            await Task.WhenAll(companyTask, loginUserTask, appConTask, sectionTask, departmentTask);

            var company = companyTask.Result.First();
            var loginUser = loginUserTask.Result.First();
            var appCon = appConTask.Result;
            var sectionDictionary = sectionTask.Result.ToDictionary(x => x.Code);
            var departmentDictionary = departmentTask.Result.ToDictionary(x => x.Code);

            var definition = new SectionWithDepartmentFileDefinition(new DataExpression(appCon));
            var parser = new CsvParser {
                Encoding        = encoding,
                StreamCreator   = new PlainTextMemoryStreamCreator(),
            };
            definition.SectionCodeField.GetModelsByCode = codes => sectionDictionary;
            definition.DepartmentCodeField.GetModelsByCode = codes => departmentDictionary;

            var importer = definition.CreateImporter(x => new { x.SectionId, x.DepartmentId }, parser);
            importer.UserId         = source.LoginUserId;
            importer.UserCode       = loginUser.Code;
            importer.CompanyId      = source.CompanyId;
            importer.CompanyCode    = company.Code;

            importer.AdditionalWorker = async worker => {
                var dbItems = (await sectionWithDepartmentProcessor.GetAsync(new SectionWithDepartmentSearch { CompanyId = source.CompanyId, }, token)).ToList();

                var dbDepartmentCheckList = new SortedList<int, SectionWithDepartment[]>(dbItems.GroupBy(x => x.DepartmentId)
                    .ToDictionary(x => x.Key, x => x.ToArray()));
                var csvDepartmentCheckList = new SortedList<int, SectionWithDepartment[]>(worker.Models.Values
                    .GroupBy(x => x.DepartmentId)
                    .ToDictionary(x => x.Key, x => x.ToArray()));

                var def = worker.RowDef as SectionWithDepartmentFileDefinition;
                def.DepartmentCodeField.ValidateAdditional = (val, param) => {
                    var reports = new List<WorkingReport>();

                    foreach (var pair in val)
                    {
                        if (IsDuped(dbDepartmentCheckList,
                            pair.Value.DepartmentId,
                            g => g.Any(x => x.SectionId != pair.Value.SectionId)))
                        {
                            reports.Add(new WorkingReport(pair.Key,
                                def.DepartmentCodeField.FieldIndex,
                                def.DepartmentCodeField.FieldName,
                                "他の入金部門に登録されているため、インポートできません。"
                            ));
                        }

                        if (IsDuped(csvDepartmentCheckList,
                            pair.Value.DepartmentId,
                            g => g.Any(x => x.SectionId != pair.Value.SectionId && pair.Value.DepartmentId != 0)))
                        {
                            reports.Add(new WorkingReport(pair.Key,
                                def.DepartmentCodeField.FieldIndex,
                                def.DepartmentCodeField.FieldName,
                                "他の入金部門に登録されているため、インポートできません。"
                            ));
                        }
                    }
                    return reports;
                };
            };

            importer.LoadAsync = () => sectionWithDepartmentProcessor.GetAsync(new SectionWithDepartmentSearch { CompanyId = source.CompanyId, }, token);
            importer.RegisterAsync = x => sectionWithDepartmentProcessor.ImportAsync(x.New, x.Dirty, x.Removed, token);

            var result = await importer.ImportAsync(csv, mode, token, null);
            result.Logs = importer.GetErrorLogs();

            return result;
        }

        private bool IsDuped(SortedList<int, SectionWithDepartment[]> list, int id, Func<SectionWithDepartment[], bool> condition)
            => list.ContainsKey(id) && condition(list[id]);

    }
}