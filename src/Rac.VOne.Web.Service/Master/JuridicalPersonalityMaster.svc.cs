using System;
using System.Collections.Generic;
using System.Linq;
using Rac.VOne.Web.Models;
using Rac.VOne.Web.Common;
using Rac.VOne.Web.Service.Extensions;
using Rac.VOne.Common.Logging;
using NLog;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Service.Master
{
    public class JuridicalPersonalityMaster : IJuridicalPersonalityMaster
    {
        private readonly IAuthorizationProcessor authorizationProcessor;
        private readonly IJuridicalPersonalityProcessor juridicalPersonalityProcessor;
        private readonly ILogger logger;

        public JuridicalPersonalityMaster(IAuthorizationProcessor authorizationProcessor,
            IJuridicalPersonalityProcessor juridicalPersonalityProcessor,
            ILogManager logManager)
        {
            this.authorizationProcessor = authorizationProcessor;
            this.juridicalPersonalityProcessor = juridicalPersonalityProcessor;
            logger = logManager.GetLogger(typeof(JuridicalPersonalityMaster));
        }

        public async Task<CountResult> DeleteAsync(string SessionKey, int CompanyId, string Kana)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = await juridicalPersonalityProcessor.DeleteAsync(CompanyId, Kana);
                return new CountResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    Count = result,
                };
            }, logger);
        }

        public async Task<JuridicalPersonalityResult> SaveAsync(string SessionKey, JuridicalPersonality JuridicalPersonality)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = await juridicalPersonalityProcessor.SaveAsync(JuridicalPersonality, token);
                return new JuridicalPersonalityResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    JuridicalPersonality = result,
                };
            }, logger);
        }

        public async Task<JuridicalPersonalityResult> GetAsync(string SessionKey, int CompanyId, string Kana)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = (await juridicalPersonalityProcessor.GetAsync(new JuridicalPersonality {
                    CompanyId   = CompanyId,
                    Kana        = Kana,
                }, token)).FirstOrDefault();
                return new JuridicalPersonalityResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    JuridicalPersonality = result,
                };
            }, logger);
        }

        public async Task<JuridicalPersonalitysResult> GetItemsAsync(string SessionKey, int CompanyId)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = (await juridicalPersonalityProcessor.GetAsync(new JuridicalPersonality { CompanyId = CompanyId, }, token)).ToList();
                return new JuridicalPersonalitysResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    JuridicalPersonalities = result,
                };
            }, logger);
        }

        public async Task<ImportResult> ImportAsync(string SessionKey,
            JuridicalPersonality[] InsertList, JuridicalPersonality[] UpdateList, JuridicalPersonality[] DeleteList)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = await juridicalPersonalityProcessor.ImportAsync(InsertList, UpdateList, DeleteList, token);
                return result;
            }, logger);
        }
    }
}
