using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Models
{
    public class SynchronizationResult<TData> : IProcessResult
    {
        public List<TData> Synchronizations { get; set; }

        public ProcessResult ProcessResult { get; set; }
    }

    //public class SynchronizeTransactionsResult : IProcessResult
    //{
    //    public List<Transaction> Synchronizations { get; set; }

    //    public ProcessResult ProcessResult { get; set; }
    //}

}
