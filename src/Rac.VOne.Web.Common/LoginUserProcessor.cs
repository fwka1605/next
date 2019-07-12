using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Rac.VOne.Data;
using Rac.VOne.Data.QueryProcessors;
using Rac.VOne.Web.Models;

namespace Rac.VOne.Web.Common
{
    public class LoginUserProcessor : ILoginUserProcessor
    {
        private readonly ILoginUserQueryProcessor loginUserQueryProcessor;
        private readonly IAddLoginUserQueryProcessor addLoginUserQueryProcessor;
        private readonly IDeleteIdenticalEntityQueryProcessor<LoginUser> deleteIdenticalQueryProcessor;
        private readonly IByCompanyGetEntityQueryProcessor<ApplicationControl> applicationControlByCompanyId;
        private readonly ILoginUserPasswordProcessor loginUserPasswordProcessor;
        private readonly ILoginUserLicenseProcessor loginUserLicenseProcessor;
        private readonly ITransactionScopeBuilder transactionScopeBuilder;

        public LoginUserProcessor(
            ILoginUserQueryProcessor loginUserQueryProcessor,
            IAddLoginUserQueryProcessor addLoginUserQueryProcessor,
            IDeleteIdenticalEntityQueryProcessor<LoginUser> deleteIdenticalQueryProcessor,
            IByCompanyGetEntityQueryProcessor<ApplicationControl> applicationControlByCompanyId,
            ILoginUserPasswordProcessor loginUserPasswordProcessor,
            ILoginUserLicenseProcessor loginUserLicenseProcessor,
            ITransactionScopeBuilder transactionScopeBuilder
            )
        {
            this.loginUserQueryProcessor = loginUserQueryProcessor;
            this.addLoginUserQueryProcessor = addLoginUserQueryProcessor;
            this.deleteIdenticalQueryProcessor = deleteIdenticalQueryProcessor;
            this.applicationControlByCompanyId = applicationControlByCompanyId;
            this.loginUserPasswordProcessor = loginUserPasswordProcessor;
            this.loginUserLicenseProcessor = loginUserLicenseProcessor;
            this.transactionScopeBuilder = transactionScopeBuilder;
        }

        public async Task<IEnumerable<LoginUser>> GetAsync(LoginUserSearch option, CancellationToken token = default(CancellationToken))
            => await loginUserQueryProcessor.GetAsync(option, token);



        public async Task<LoginUser> SaveAsync(LoginUser loginUser, CancellationToken token = default(CancellationToken))
        {
            using (var scope = transactionScopeBuilder.Create())
            {
                var result = await addLoginUserQueryProcessor.SaveAsync(loginUser, token);

                if (result != null && !string.IsNullOrEmpty(loginUser.InitialPassword))
                {
                    await loginUserPasswordProcessor.SaveAsync(result.CompanyId, result.Id, loginUser.InitialPassword, token);
                    result.InitialPassword = loginUser.InitialPassword;
                }

                scope.Complete();

                return result;
            }
        }

        public async Task<bool> ExitStaffAsync(int StaffId, CancellationToken token = default(CancellationToken))
            => await loginUserQueryProcessor.ExitStaffAsync(StaffId, token);

        public async Task<int> DeleteAsync(int id, CancellationToken token = default(CancellationToken))
            => await deleteIdenticalQueryProcessor.DeleteAsync(id, token);


        public async Task<ImportResult> ImportAsync(
            IEnumerable<LoginUser> InsertList,
            IEnumerable<LoginUser> UpdateList,
            IEnumerable<LoginUser> DeleteList, CancellationToken token = default(CancellationToken))
        {
            using (var scope = transactionScopeBuilder.Create())
            {
                var deleteCount = 0;
                var updateCount = 0;
                var insertCount = 0;


                var first = InsertList.FirstOrDefault() ??
                            UpdateList.FirstOrDefault() ??
                            DeleteList.FirstOrDefault();
                if (first == null) throw new ArgumentNullException($"{nameof(InsertList)} or {nameof(UpdateList)}");

                var companyId = first.CompanyId;
                var loginUserId = first.UpdateBy;

                var isNotUseDistribution = (await applicationControlByCompanyId.GetAsync(companyId, token)).UseDistribution == 0;

                foreach (var delete in DeleteList)
                {
                    await deleteIdenticalQueryProcessor.DeleteAsync(delete.Id, token);
                    deleteCount++;
                }

                foreach (var update in UpdateList)
                {
                    if (isNotUseDistribution) update.UseClient = 1;
                    await addLoginUserQueryProcessor.SaveAsync(update);
                    updateCount++;
                }

                foreach (var add in InsertList)
                {
                    if (isNotUseDistribution) add.UseClient = 1;
                    var saveResult = await addLoginUserQueryProcessor.SaveAsync(add);
                    if (saveResult != null && !string.IsNullOrEmpty(add.InitialPassword))
                    {
                        await loginUserPasswordProcessor.SaveAsync(saveResult.CompanyId, saveResult.Id, add.InitialPassword);
                    }
                    insertCount++;
                }

                var result = new ImportResultLoginUser
                {
                    ProcessResult = new ProcessResult { Result = true },
                    InsertCount = insertCount,
                    UpdateCount = updateCount,
                    DeleteCount = deleteCount,
                };

                await ValidateLicenses(result, companyId, loginUserId, token);

                if (!(result.LicenseIsOrver || result.NotExistsLoginUser || result.LoginUserHasNotLoginLicense))
                    scope.Complete();

                return result;

            }

        }

        private async Task ValidateLicenses(
            ImportResultLoginUser result,
            int companyId, int loginUserId, CancellationToken token)
        {
            var licenseKeysCount = (await loginUserLicenseProcessor.GetAsync(companyId, token)).Count();
            var loginUsersUseClient = (await loginUserQueryProcessor.GetAsync( new LoginUserSearch { CompanyId = companyId, UseClient = 1, })).ToArray();

            if (loginUsersUseClient.Length > licenseKeysCount)
            {
                result.LicenseIsOrver = true;
            }

            var Ids = new int[] { loginUserId };
            var loginUser = (await loginUserQueryProcessor.GetAsync( new LoginUserSearch { Ids = Ids })).ToList();
            if (loginUser == null || loginUser.Count < 1)
            {
                result.NotExistsLoginUser = true;
            }
            else if (loginUser[0].UseClient == 0)
            {
                result.LoginUserHasNotLoginLicense = true;
            }
        }

    }
}
