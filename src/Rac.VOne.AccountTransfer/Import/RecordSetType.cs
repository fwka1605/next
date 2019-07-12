namespace Rac.VOne.AccountTransfer.Import
{
    /// <summary>
    /// ファイルがどの種類のレコードで構成されているかを列挙
    /// </summary>
    public enum RecordSetType
    {
        /// <summary>Header, Data</summary>
        HD,

        /// <summary>Header, Data, Trailer</summary>
        HDT,

        /// <summary>Header, Data, Trailer, End</summary>
        HDTE,
    }
}
