﻿using Rac.VOne.Web.Models;
using System.Threading;
using System.Threading.Tasks;

namespace Rac.VOne.Data.QueryProcessors
{
    public interface IAddReminderTemplateSettingQueryProcessor
    {
        Task<ReminderTemplateSetting> SaveAsync(ReminderTemplateSetting ReminderTemplateSetting, CancellationToken token = default(CancellationToken));
    }
}