using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rac.VOne.Common
{
    /// <summary>
    /// (dynamic)client wrapper
    /// to resolve type inference
    /// </summary>
    public class ProgressNotifier : IProgressNotifier
    {
        private readonly dynamic client;
        public ProgressNotifier(dynamic client)
        {
            this.client = client;
        }
        public void UpdateState() => client?.UpdateState();

        public void Abort() => client?.Abort();

    }
}
