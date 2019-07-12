using System.Collections.Generic;
using System.Linq;
using Rac.VOne.Data;
using Rac.VOne.Data.QueryProcessors;
using Rac.VOne.Web.Models;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Common
{
    public class DepartmentProcessor : IDepartmentProcessor
    {
        private readonly IDepartmentByCodeQueryProcessor departmentNameByCodeQueryProcessor;
        private readonly IAddDepartmentQueryProcessor addDepartmentQueryProcessor;
        private readonly IDeleteIdenticalEntityQueryProcessor<Department> deleteDepartmentQueryProcessor;
        private readonly ITransactionScopeBuilder transactionScopeBuilder;

        public DepartmentProcessor(
            IDepartmentByCodeQueryProcessor departmentNameByCodeQueryProcessor,
            IAddDepartmentQueryProcessor addDepartmentQueryProcessor,
            IDeleteIdenticalEntityQueryProcessor<Department> deleteDepartmentQueryProcessor,
            ITransactionScopeBuilder transactionScopeBuilder
            )
        {
            this.departmentNameByCodeQueryProcessor = departmentNameByCodeQueryProcessor;
            this.addDepartmentQueryProcessor = addDepartmentQueryProcessor;
            this.deleteDepartmentQueryProcessor = deleteDepartmentQueryProcessor;
            this.transactionScopeBuilder = transactionScopeBuilder;
        }

        public async Task<IEnumerable<Department>> GetAsync(DepartmentSearch option, CancellationToken token = default(CancellationToken))
            => await departmentNameByCodeQueryProcessor.GetAsync(option, token);


        public async Task<int> DeleteAsync(int Id, CancellationToken token = default(CancellationToken))
            => await deleteDepartmentQueryProcessor.DeleteAsync(Id, token);

        public async Task<IEnumerable<Department>> SaveAsync(IEnumerable<Department> departments, CancellationToken token = default(CancellationToken))
        {
            using (var scope = transactionScopeBuilder.Create())
            {
                var result = new List<Department>();
                foreach (var department in departments)
                    result.Add(await addDepartmentQueryProcessor.SaveAsync(department));
                scope.Complete();
                return result;
            }
        }


        public async Task<IEnumerable<MasterData>> GetImportItemsStaffAsync(int CompanyId, IEnumerable<string> Code, CancellationToken token = default(CancellationToken))
            => await departmentNameByCodeQueryProcessor.GetImportItemsStaffAsync(CompanyId, Code, token);

        public async Task<IEnumerable<MasterData>> GetImportItemsSectionWithDepartmentAsync(int CompanyId, IEnumerable<string> Code, CancellationToken token = default(CancellationToken))
            => await departmentNameByCodeQueryProcessor.GetImportItemsSectionWithDepartmentAsync(CompanyId, Code, token);

        public async Task<IEnumerable<MasterData>> GetImportItemsBillingAsync(int CompanyId, IEnumerable<string> Code, CancellationToken token = default(CancellationToken))
            => await departmentNameByCodeQueryProcessor.GetImportItemsBillingAsync(CompanyId, Code, token);

        public async Task<ImportResult> ImportAsync(
            IEnumerable<Department> insert,
            IEnumerable<Department> update,
            IEnumerable<Department> delete, CancellationToken token = default(CancellationToken))
        {
            using (var scope = transactionScopeBuilder.Create())
            {
                var deleteCount = 0;
                var updateCount = 0;
                var insertCount = 0;
                foreach (var x in delete)
                {
                    await deleteDepartmentQueryProcessor.DeleteAsync(x.Id, token);
                    ++deleteCount;
                }

                foreach (var x in update)
                {
                    await addDepartmentQueryProcessor.SaveAsync(x, token);
                    ++updateCount;
                }
                foreach (var x in insert)
                {
                    await addDepartmentQueryProcessor.SaveAsync(x, token);
                    ++insertCount;
                }

                scope.Complete();

                return new ImportResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    InsertCount = insertCount,
                    UpdateCount = updateCount,
                    DeleteCount = deleteCount,
                };
            }
        }
    }
}
