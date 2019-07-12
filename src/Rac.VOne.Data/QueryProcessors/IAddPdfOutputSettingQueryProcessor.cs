using System;
using System.Collections.Generic;
using System.Text;
using Rac.VOne.Web.Models;
using System.Threading;
using System.Threading.Tasks;

namespace Rac.VOne.Data.QueryProcessors
{
    public interface IAddPdfOutputSettingQueryProcessor
    {
        Task<PdfOutputSetting> SaveAsync(PdfOutputSetting setting, CancellationToken token = default(CancellationToken));
    }
}
