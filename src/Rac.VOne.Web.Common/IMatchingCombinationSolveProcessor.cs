﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rac.VOne.Web.Models;

namespace Rac.VOne.Web.Common
{
    public interface IMatchingCombinationSolveProcessor
    {
        List<int> Solve(IEnumerable<Billing> Billings, decimal TargetAmount);
    }
}
