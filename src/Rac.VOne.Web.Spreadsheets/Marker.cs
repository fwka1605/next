namespace Rac.VOne.Web.Spreadsheets
{
    public class Marker : IMarker
    {
        public string ColumnId { get; set; }
        public string RowId { get; set; }
        public string ColumnOffset { get; set; }
        public string RowOffset { get; set; }
        public Marker(string address) : this(address.GetColumnIndex(), address.GetRowIndex())
        {
        }

        public Marker(uint columnIndex, uint rowIndex) : this()
        {
            ColumnId    = (columnIndex  - 1).ToInvaliantString();
            RowId       = (rowIndex     - 1).ToInvaliantString();
        }

        public Marker()
        {
            ColumnId        = "0";
            RowId           = "0";
            ColumnOffset    = "0";
            RowOffset       = "0";
        }
    }
}
