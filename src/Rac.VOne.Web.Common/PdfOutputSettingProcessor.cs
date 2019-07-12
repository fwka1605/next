using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Rac.VOne.Data.QueryProcessors;
using Rac.VOne.Web.Models;

namespace Rac.VOne.Web.Common
{
    public class PdfOutputSettingProcessor : IPdfOutputSettingProcessor
    {
        private readonly IPdfOutputSettingQueryProcessor pdfOutputSettingQueryProcessor;
        private readonly IAddPdfOutputSettingQueryProcessor addPdfOutputSettingQueryProcessor;
        public PdfOutputSettingProcessor(
            IPdfOutputSettingQueryProcessor pdfOutputSettingQueryProcessor,
            IAddPdfOutputSettingQueryProcessor addPdfOutputSettingQueryProcessor
            )
        {
            this.pdfOutputSettingQueryProcessor = pdfOutputSettingQueryProcessor;
            this.addPdfOutputSettingQueryProcessor = addPdfOutputSettingQueryProcessor;
        }

        public async Task<PdfOutputSetting> GetAsync(int CompanyId, int ReportType, int UserId, CancellationToken token = default(CancellationToken))
        {
            var setting = await pdfOutputSettingQueryProcessor.GetAsync(CompanyId, ReportType, token);
            if (setting != null) return setting;

            var defaultSetting = new PdfOutputSetting {
                CompanyId = CompanyId,
                ReportType = ReportType,
                OutputUnit = (int)PdfOutputSettingOutputUnit.AllInOne,
                CreateBy = UserId,
                UpdateBy = UserId
            };
            return await addPdfOutputSettingQueryProcessor.SaveAsync(defaultSetting, token);
        }

        public async Task<PdfOutputSetting> SaveAsync(PdfOutputSetting setting, CancellationToken token = default(CancellationToken))
            => await addPdfOutputSettingQueryProcessor.SaveAsync(setting, token);
    }
}
