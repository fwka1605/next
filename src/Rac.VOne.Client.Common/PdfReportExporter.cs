using GrapeCity.ActiveReports;
using GrapeCity.ActiveReports.Export.Pdf.Section;
using System;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rac.VOne.Client.Common
{
    public class PdfReportExporter
    {
        public void PdfExport(SectionReport report, string path)
        {
            using(var exporter = new PdfExport())
            {
                exporter.Export(report.Document, path);
            }
        }
    }
}
