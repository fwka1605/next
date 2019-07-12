using DocumentFormat.OpenXml.Packaging;

namespace Rac.VOne.Web.Spreadsheets
{
    public interface IProcessor
    {
        void Process(SpreadsheetDocument document);
    }
}
