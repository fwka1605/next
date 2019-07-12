namespace Rac.VOne.Web.Spreadsheets
{
    public interface IMarker
    {
        /// <summary>column id 0 から開始する index </summary>
        string ColumnId { get; set; }
        /// <summary>row id 0 から開始する index</summary>
        string RowId { get; set; }

        /// <summary>column のオフセット</summary>
        string ColumnOffset { get; set; }
        /// <summary>row のオフセット</summary>
        string RowOffset { get; set; }
    }
}
