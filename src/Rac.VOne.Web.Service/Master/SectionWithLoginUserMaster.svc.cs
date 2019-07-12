using Rac.VOne.Common.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Rac.VOne.Web.Common;
using Rac.VOne.Web.Models;
using Rac.VOne.Web.Service.Extensions;
using NLog;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Service.Master
{
    public class SectionWithLoginUserMaster : ISectionWithLoginUserMaster
    {
        private readonly IAuthorizationProcessor authorizationProcessor;
        private readonly ISectionWithLoginUserProcessor sectionWithLoginUserProcessor;
        private readonly ILogger logger;

        public SectionWithLoginUserMaster(
            IAuthorizationProcessor authorizationProcessor,
            ISectionWithLoginUserProcessor sectionWithLoginUserProcessor,
            ILogManager logManager
            )
        {
            this.authorizationProcessor = authorizationProcessor;
            this.sectionWithLoginUserProcessor = sectionWithLoginUserProcessor;
            logger = logManager.GetLogger(typeof(SectionWithLoginUserMaster));
        }

        public async Task<SectionWithLoginUserResult> SaveAsync(string SessionKey, SectionWithLoginUser[] AddList, SectionWithLoginUser[] DeleteList)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = (await sectionWithLoginUserProcessor.SaveAsync(AddList, DeleteList, token)).First();
                return new SectionWithLoginUserResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    SectionWithLoginUser = result,
                };
            }, logger);
        }

        public async Task<SectionWithLoginUsersResult> GetByLoginUserAsync(string SessionKey, int CompanyId, int LoginUserId)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = (await sectionWithLoginUserProcessor.GetAsync(new SectionWithLoginUserSearch {
                    CompanyId       = CompanyId,
                    LoginUserId     = LoginUserId,
                }, token)).ToList();
                return new SectionWithLoginUsersResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    SectionWithLoginUsers = result,
                };
            }, logger);
        }

        public async Task<SectionWithLoginUsersResult> GetItemsAsync(string SessionKey, int CompanyId,SectionWithLoginUserSearch SectionWithLoginUserSearch)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = (await sectionWithLoginUserProcessor.GetAsync(SectionWithLoginUserSearch, token)).ToList();

                return new SectionWithLoginUsersResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    SectionWithLoginUsers = result,
                };
            }, logger);
        }

        public async Task<ImportResultSectionWithLoginUser> ImportAsync(string SessionKey,
             SectionWithLoginUser[] InsertList, SectionWithLoginUser[] UpdateList, SectionWithLoginUser[] DeleteList)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                return (await sectionWithLoginUserProcessor.ImportAsync(InsertList, UpdateList, DeleteList, token)) as ImportResultSectionWithLoginUser;
            }, logger);
        }

        public async Task<ExistResult> ExistLoginUserAsync(string SessionKey,int LoginUserId)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = await sectionWithLoginUserProcessor.ExistLoginUserAsync(LoginUserId, token);

                return new ExistResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    Exist = result,
                };
            }, logger);
        }
        public async Task<ExistResult> ExistSectionAsync(string SessionKey, int SectionId)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = await sectionWithLoginUserProcessor.ExistSectionAsync(SectionId, token);

                return new ExistResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    Exist = result,
                };
            }, logger);
        }
    }
}
