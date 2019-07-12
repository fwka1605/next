using System.Threading;
using System.Threading.Tasks;
using Rac.VOne.Data.QueryProcessors;
using Rac.VOne.Web.Models;

namespace Rac.VOne.Web.Common
{
    public class PasswordPolicyProcessor : IPasswordPolicyProcessor
    {
        private readonly IAddPasswordPolicyQueryProcessor addPasswordPolicyQueryProcessor;
        private readonly IByCompanyGetEntityQueryProcessor<PasswordPolicy> getPasswordPolicyQueryProcessor;

        public PasswordPolicyProcessor(
            IAddPasswordPolicyQueryProcessor addPasswordPolicyQueryProcessor,
            IByCompanyGetEntityQueryProcessor<PasswordPolicy> getPasswordPolicyQueryProcessor)
        {
            this.addPasswordPolicyQueryProcessor = addPasswordPolicyQueryProcessor;
            this.getPasswordPolicyQueryProcessor = getPasswordPolicyQueryProcessor;
        }

        public async Task<PasswordPolicy> GetAsync(int CompanyId, CancellationToken token = default(CancellationToken))
            => await getPasswordPolicyQueryProcessor.GetAsync(CompanyId, token);


        public async Task<PasswordPolicy> SaveAsync(PasswordPolicy PasswordPolicy, CancellationToken token = default(CancellationToken))
            => await addPasswordPolicyQueryProcessor.SaveAsync(PasswordPolicy, token);

    }
}
