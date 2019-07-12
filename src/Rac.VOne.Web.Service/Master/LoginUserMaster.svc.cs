using System;
using System.Collections.Generic;
using System.Linq;
using Rac.VOne.Web.Common;
using Rac.VOne.Web.Models;
using Rac.VOne.Web.Service.Extensions;
using NLog;
using Rac.VOne.Common.Logging;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Service.Master
{
    public class LoginUserMaster : ILoginUserMaster
    {
        private readonly IAuthorizationProcessor authorizationProcessor;
        private readonly ILoginUserProcessor loginUserProcessor;
        private readonly ILoginUserPasswordProcessor loginUserPasswordProcessor;
        private readonly ILogger logger;

        public LoginUserMaster(
            IAuthorizationProcessor authorizationProcessor,
            ILoginUserProcessor loginUserProcessor,
            ILoginUserPasswordProcessor loginUserPasswordProcessor,
            ILogManager logManager
            )
        {
            this.authorizationProcessor = authorizationProcessor;
            this.loginUserProcessor = loginUserProcessor;
            this.loginUserPasswordProcessor = loginUserPasswordProcessor;
            logger = logManager.GetLogger(typeof(LoginUserMaster));
        }

        public async Task<UsersResult> GetAsync(string SessionKey, int[] Id)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = (await loginUserProcessor.GetAsync(new LoginUserSearch { Ids = Id, }, token)).ToList();
                return new UsersResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    Users = result,
                };
            }, logger);
        }

        public async Task<UsersResult> GetItemsAsync(string SessionKey, int CompanyId, LoginUserSearch LoginUserSearch)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                LoginUserSearch.CompanyId = CompanyId;
                var result = (await loginUserProcessor.GetAsync(LoginUserSearch, token)).ToList();
                return new UsersResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    Users = result,
                };
            }, logger);
        }

        public async Task<CountResult> ResetPasswordAsync(string SessionKey, int Id)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                int result = await loginUserPasswordProcessor.ResetAsync(Id, token);
                return new CountResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    Count =  result,
                };
            }, logger);

        }

        public async Task<CountResult> DeleteAsync(string SessionKey, int Id)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = await loginUserProcessor.DeleteAsync(Id, token);
                return new CountResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    Count = result,
                };
            }, logger);
        }

        public async Task<UsersResult> GetByCodeAsync(string SessionKey, int CompanyId, string[] Code)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = (await loginUserProcessor.GetAsync(new LoginUserSearch {
                    CompanyId   = CompanyId,
                    Codes       = Code,
                }, token)).ToList();

                return new UsersResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    Users = result,
                };
            }, logger);
        }

        public async Task<UserResult> SaveAsync(string SessionKey, LoginUser UserData)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = await loginUserProcessor.SaveAsync(UserData, token);
                return new UserResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    User = result,
                };
            }, logger);
        }

        public async Task<UsersResult> GetItemsForGridLoaderAsync(string Sessionkey, int CompanyId)
        {
            return await authorizationProcessor.DoAuthorizeAsync(Sessionkey, async token =>
            {
                var result = (await loginUserProcessor.GetAsync(new LoginUserSearch {
                    CompanyId   = CompanyId,
                    UseClient   = 1,
                }, token)).ToList();
                return new UsersResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    Users = result,
                };
            }, logger);
        }

        public async Task<CountResult> ExitStaffAsync(string SessionKey, int StaffId)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = (await loginUserProcessor.ExitStaffAsync(StaffId, token)) ? 1 : 0;
                return new CountResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    Count = result,
                };
            }, logger);
        }

        public async Task<ImportResultLoginUser> ImportAsync(string SessionKey,
            int CompanyId,
            int LoginUserId,
            LoginUser[] InsertList,
            LoginUser[] UpdateList,
            LoginUser[] DeleteList)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                return await loginUserProcessor.ImportAsync(
                    InsertList,
                    UpdateList,
                    DeleteList, token) as ImportResultLoginUser;

            }, logger);
        }


        public async Task<UsersResult> GetImportItemsForSectionAsync(string SessionKey, int CompanyId, string[] Code)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = (await loginUserProcessor.GetAsync(new LoginUserSearch {
                    CompanyId       = CompanyId,
                    ExcludeCodes    = Code,
                }, token)).ToList();

                return new UsersResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    Users = result,
                };

            }, logger);
        }

    }
}
