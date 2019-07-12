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
    public class StaffProcessor : IStaffProcessor
    {
        private readonly IDeleteIdenticalEntityQueryProcessor<Staff> deleteStaffQueryProcessor;
        private readonly IStaffQueryProcessor staffQueryProcessor;
        private readonly IAddStaffQueryProcessor addstaffQueryProcessor;
        private readonly ITransactionScopeBuilder transactionScopeBuilder;

        public StaffProcessor(
            IDeleteIdenticalEntityQueryProcessor<Staff> deleteStaffQueryProcessor,
            IStaffQueryProcessor staffQueryProcessor,
            IAddStaffQueryProcessor addstaffQueryProcessor,
            ITransactionScopeBuilder transactionScopeBuilder
            )
        {
            this.deleteStaffQueryProcessor = deleteStaffQueryProcessor;
            this.staffQueryProcessor = staffQueryProcessor;
            this.addstaffQueryProcessor = addstaffQueryProcessor;
            this.transactionScopeBuilder = transactionScopeBuilder;
        }

        public async Task<IEnumerable<Staff>> GetAsync(StaffSearch option, CancellationToken token = default(CancellationToken))
            => await staffQueryProcessor.GetAsync(option, token);
        public async Task<bool> ExistDepartmentAsync(int DepartmentId, CancellationToken token = default(CancellationToken))
            => await staffQueryProcessor.ExistDepartmentAsync(DepartmentId, token);

        public async Task<IEnumerable<MasterData>> GetImportItemsLoginUserAsync(int CompanyId, IEnumerable<string> Code, CancellationToken token = default(CancellationToken))
            => await staffQueryProcessor.GetImportItemsLoginUserAsync(CompanyId, Code, token);


        public async Task<IEnumerable<MasterData>> GetImportItemsCustomerAsync(int CompanyId, IEnumerable<string> Code, CancellationToken token = default(CancellationToken))
            => await staffQueryProcessor.GetImportItemsCustomerAsync(CompanyId, Code, token);

        public async Task<IEnumerable<MasterData>> GetImportItemsBillingAsync(int CompanyId, IEnumerable<string> Code, CancellationToken token = default(CancellationToken))
            => await staffQueryProcessor.GetImportItemsBillingAsync(CompanyId, Code, token);


        public async Task<Staff> SaveAsync(Staff Staff, CancellationToken token = default(CancellationToken))
            => await addstaffQueryProcessor.SaveAsync(Staff, token);

        public async Task<int> DeleteAsync(int Id, CancellationToken token = default(CancellationToken))
            => await deleteStaffQueryProcessor.DeleteAsync(Id, token);

        public async Task<ImportResult> ImportAsync(IEnumerable<Staff> insert, IEnumerable<Staff> update, IEnumerable<Staff> delete, CancellationToken token = default(CancellationToken))
        {
            using (var scope = transactionScopeBuilder.Create())
            {
                var deleteCount = 0;
                var updateCount = 0;
                var insertCount = 0;
                var items = new List<Staff>();
                foreach (var x in delete)
                {
                    await deleteStaffQueryProcessor.DeleteAsync(x.Id, token);
                    deleteCount++;
                }

                foreach (var x in update)
                {
                    var item = await addstaffQueryProcessor.SaveAsync(x, token);
                    items.Add(item);
                    updateCount++;
                }

                foreach (var x in insert)
                {
                    var item = await addstaffQueryProcessor.SaveAsync(x, token);
                    items.Add(item);
                    insertCount++;
                }

                scope.Complete();

                return new ImportResultStaff
                {
                    ProcessResult = new ProcessResult { Result = true },
                    InsertCount = insertCount,
                    UpdateCount = updateCount,
                    DeleteCount = deleteCount,
                    Staffs = items,
                };

            }
        }
    }
}
