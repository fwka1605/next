using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Rac.VOne.Web.Models;
using System.Threading;
using System.Threading.Tasks;


namespace Rac.VOne.Web.Common
{
    public interface IPdfOutputSettingProcessor
    {
        Task<PdfOutputSetting> GetAsync(int CompanyId, int ReportType, int UserId, CancellationToken token = default(CancellationToken));
        Task<PdfOutputSetting> SaveAsync(PdfOutputSetting Setting, CancellationToken token = default(CancellationToken));
    }
}
