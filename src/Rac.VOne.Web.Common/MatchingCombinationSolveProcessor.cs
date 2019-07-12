using Rac.VOne.Common.DataHandling;
using Rac.VOne.Web.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Rac.VOne.Web.Common
{
    public class MatchingCombinationSolveProcessor :
        IMatchingCombinationSolveProcessor
    {
        private const int TimeOut = 30;
        private TimeSpan Span
        { get { return TimeSpan.FromSeconds(TimeOut); } }

        public List<int> Solve(IEnumerable<Billing> Billings, decimal TargetAmount)
        {
            var swatch = new Stopwatch();
            swatch.Start();
            var billings = Billings.ToArray();
            var itemCount = billings.Length;
            for (var selectCount = 1; selectCount <= itemCount; selectCount++)
            {
                var enumerator = new IndicesCombinationEnumerable(itemCount, selectCount);
                foreach (var item in enumerator)
                {
                    if (swatch.Elapsed > Span) throw new TimeoutException();
                    if (TargetAmount == item.Select(x => billings[x].TargetAmount).Sum())
                    {
                        return item;
                    }
                }
            }
            return new List<int>();
        }

    }
}
