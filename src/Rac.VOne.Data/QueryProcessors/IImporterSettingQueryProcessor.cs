﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Rac.VOne.Web.Models;

namespace Rac.VOne.Data.QueryProcessors
{
    public interface IImporterSettingQueryProcessor
    {
        Task<IEnumerable<ImporterSetting>> GetAsync(ImporterSetting setting, CancellationToken token = default(CancellationToken));

    }
}