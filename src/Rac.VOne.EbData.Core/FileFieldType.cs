namespace Rac.VOne.EbData
{
    public enum FileFieldType
    {
        /// <summary>カンマ区切り</summary>
        CommaDelimited = 1,
        /// <summary>TAB区切り</summary>
        TabDelimited,
        /// <summary>固定長</summary>
        FixedWidth,
        /// <summary>固定長 改行コード無</summary>
        FixedWidthNoLineBreak,
    }
}
