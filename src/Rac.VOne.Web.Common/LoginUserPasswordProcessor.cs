using System.Threading.Tasks;
using Rac.VOne.Common.Security;
using Rac.VOne.Data;
using Rac.VOne.Data.QueryProcessors;
using Rac.VOne.Web.Models;
using System;
using System.Threading;

namespace Rac.VOne.Web.Common
{
    public class LoginUserPasswordProcessor :
        ILoginUserPasswordProcessor
    {
        private readonly IHashAlgorithm hashAlgorithm;
        private readonly IPasswordPolicyProcessor passwordPolicyProcessor;
        private readonly IAddLoginUserPasswordQueryProcessor addLoginUserPasswordQueryProcessor;
        private readonly ILoginUserPasswordQueryProcessor loginUserPasswordQueryProcessor;
        private readonly IDeleteLoginUserPasswordQueryProcessor deleteloginUserPasswordQueryProcessor;

        public LoginUserPasswordProcessor(
            IHashAlgorithm hashAlgorithm,
            IPasswordPolicyProcessor passwordPolicyProcessor,
            IAddLoginUserPasswordQueryProcessor addLoginUserPasswordQueryProcessor,
            ILoginUserPasswordQueryProcessor loginUserPasswordQueryProcessor,
            IDeleteLoginUserPasswordQueryProcessor deleteloginUserPasswordQueryProcessor
            )
        {
            this.hashAlgorithm = hashAlgorithm;
            this.passwordPolicyProcessor = passwordPolicyProcessor;
            this.addLoginUserPasswordQueryProcessor = addLoginUserPasswordQueryProcessor;
            this.loginUserPasswordQueryProcessor = loginUserPasswordQueryProcessor;
            this.deleteloginUserPasswordQueryProcessor = deleteloginUserPasswordQueryProcessor;
        }

        public async Task<PasswordChangeResult> ChangeAsync(int CompanyId, int LoginUserId, string OldPassword, string NewPassword, CancellationToken token = default(CancellationToken))
        {
            var policy = await passwordPolicyProcessor.GetAsync(CompanyId, token);
            var oldHash = hashAlgorithm.Compute(policy.Convert(OldPassword));

            var password = await loginUserPasswordQueryProcessor.GetAsync(CompanyId, LoginUserId, token);
            if (password.PasswordHash != oldHash)
            {
                return PasswordChangeResult.Failed;
            }

            var newHash = hashAlgorithm.Compute(policy.Convert(NewPassword));


            if (policy.HistoryCount > 0)
            {
                if (!password.Validate(newHash, policy.HistoryCount))
                    return PasswordChangeResult.ProhibitionSamePassword;
            }

            var type = typeof(LoginUserPassword);
            for (var i = 9; i > 0; i--)
            {
                var nextHash = type.GetProperty($"PasswordHash{i}");
                var prevHash = type.GetProperty($"PasswordHash{(i - 1)}");
                var nextValue = policy.HistoryCount <= i
                    ? string.Empty
                    : prevHash.GetValue(password);
                nextHash.SetValue(password, nextValue);
            }

            if (policy.HistoryCount > 0)
            {
                password.PasswordHash0 = password.PasswordHash;
            }
            else
            {
                password.PasswordHash0 = string.Empty;
            }

            password.PasswordHash = newHash;
            await addLoginUserPasswordQueryProcessor.SaveAsync(password, token);
            return PasswordChangeResult.Success;
        }


        public async Task<LoginResult> LoginAsync(int CompanyId, int LoginUserId, string Password, CancellationToken token = default(CancellationToken))
        {
            var policy = await passwordPolicyProcessor.GetAsync(CompanyId, token);
            var hash = hashAlgorithm.Compute(policy.Convert(Password));
            var password = await loginUserPasswordQueryProcessor.GetAsync(CompanyId, LoginUserId, token);

            if (password.PasswordHash != hash) return LoginResult.Failed;

            if (policy.ExpirationDays > 0)
            {
                try
                {
                    var expired = password.UpdateAt.Date.AddDays(policy.ExpirationDays);
                    if (expired < DateTime.Today)
                        return LoginResult.Expired;
                }
                catch
                {
                    return LoginResult.Failed;
                }
            }
            return LoginResult.Success;
        }


        public async Task<int> ResetAsync(int LoginUserId, CancellationToken token = default(CancellationToken))
            => await deleteloginUserPasswordQueryProcessor.DeleteAsync(LoginUserId, token);


        public async Task<PasswordChangeResult> SaveAsync(int CompanyId, int LoginUserId, string Password, CancellationToken token = default(CancellationToken))
        {
            var policy = await passwordPolicyProcessor.GetAsync(CompanyId);
            var hash = hashAlgorithm.Compute(policy.Convert(Password));
            var password = new LoginUserPassword
            {
                LoginUserId     = LoginUserId,
                PasswordHash    = hash,
            };
            await addLoginUserPasswordQueryProcessor.SaveAsync(password, token);
            return PasswordChangeResult.Success;
        }

    }
}
