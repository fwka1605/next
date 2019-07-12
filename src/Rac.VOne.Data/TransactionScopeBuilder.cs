using System;
using System.Transactions;

namespace Rac.VOne.Data
{
    public class TransactionScopeBuilder : ITransactionScopeBuilder
    {
        private readonly ITimeOutSetter timeOutSetter;
        public TransactionScopeBuilder(
            ITimeOutSetter timeOutSetter
            )
        {
            this.timeOutSetter = timeOutSetter;
            timeout = timeOutSetter.GetTimeOut();
        }

        private TransactionScopeOption scopeOption = TransactionScopeOption.Required;
        public TransactionScopeBuilder ScopeOption(TransactionScopeOption option)
        {
            scopeOption = option;
            return this;
        }

        private IsolationLevel isolationLevel = System.Transactions.IsolationLevel.Serializable;
        public TransactionScopeBuilder IsolationLevel(IsolationLevel level)
        {
            isolationLevel = level;
            return this;
        }

        private TimeSpan timeout;
        public TransactionScopeBuilder Timeout(TimeSpan scopeTimeout)
        {
            timeout = scopeTimeout;
            return this;
        }

        public TransactionScope Create()
        {
            return new TransactionScope(scopeOption,
                new TransactionOptions
                {
                    IsolationLevel = isolationLevel,
                    Timeout = timeout
                }, TransactionScopeAsyncFlowOption.Enabled);
        }
    }
}
