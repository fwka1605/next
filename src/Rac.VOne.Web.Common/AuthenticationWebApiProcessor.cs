using System;
using System.Collections.Generic;
using System.Text;
using Rac.VOne.Common.Security;
using Rac.VOne.Data;
using Rac.VOne.Data.QueryProcessors;
using Rac.VOne.Web.Models;
using System.Threading;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Common
{
    public class AuthenticationWebApiProcessor : IAuthenticationWebApiProcessor
    {

        private readonly IHashAlgorithm hashAlgorithm;
        private readonly IStringConnectionFactory stringConnectionFactory;
        private readonly IAuthenticationQueryProcessor authenticationQueryProcessor;


        public AuthenticationWebApiProcessor(
            IHashAlgorithm hashAlgorithm,
            IStringConnectionFactory stringConnectionFactory,
            IAuthenticationQueryProcessor authenticationQueryProcessor
            )
        {
            this.hashAlgorithm = hashAlgorithm;
            this.stringConnectionFactory = stringConnectionFactory;
            this.authenticationQueryProcessor = authenticationQueryProcessor;
        }

        public async Task<ProcessResult> AuthenticateAsync(string authenticationKey, string tenantCode, CancellationToken token = default(CancellationToken))
        {
            var hash = hashAlgorithm.Compute(authenticationKey);
            var exists = await authenticationQueryProcessor.ExistsAuthencticationKeyAsync(hash, token);

            if (!exists) return new ProcessResult { ErrorMessage = "認証キーが不正" };

            var connectionString = await authenticationQueryProcessor.GetConnectionStringAsync(tenantCode, token);
            var companyExist = !string.IsNullOrEmpty(connectionString);

            if (!companyExist) return new ProcessResult { ErrorMessage = "テナントコードが不正" };

            var factory = stringConnectionFactory.Create(connectionString);

            var connectable = await authenticationQueryProcessor.IsConnectableAsync(factory, token);

            if (!connectable) return new ProcessResult { ErrorCode = "DB接続に失敗" };

            return new ProcessResult { Result = true };
        }

        public async Task<ProcessResult> CreateSessionAsync(string tenantCode, CancellationToken token = default(CancellationToken))
        {
            var connectionString = await authenticationQueryProcessor.GetConnectionStringAsync(tenantCode, token);
            var tenantExists = !string.IsNullOrEmpty(connectionString);
            if (!tenantExists) return new ProcessResult { ErrorMessage = "テナントコードが不正" };

            var factory = stringConnectionFactory.Create(connectionString);
            var connectable = await authenticationQueryProcessor.IsConnectableAsync(factory, token);
            if (!connectable) return new ProcessResult { ErrorMessage = "DB接続に失敗" };

            var sessionKey = await authenticationQueryProcessor.CreateSessionKeyAsync(connectionString, token);
            var persisted = !string.IsNullOrEmpty(sessionKey);

            if (!persisted) return new ProcessResult { ErrorMessage = "セッションキー生成に失敗" };
            return new ProcessResult { Result = true, ErrorMessage = sessionKey };
        }
    }
}
