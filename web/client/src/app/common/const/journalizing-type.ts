export  class JournalizingType
{
    /// <summary>0 : 入金仕訳</summary>
    public static readonly Receipt = 0;
    /// <summary>1 : 消込仕訳</summary>
    public static readonly Matching = 1;
    /// <summary>2 : 前受計上仕訳</summary>
    public static readonly AdvanceReceivedOccured = 2;
    /// <summary>3 : 前受振替仕訳</summary>
    public static readonly AdvanceReceivedTransfer = 3;
    /// <summary>4 : 入金対象外仕訳</summary>
    public static readonly ReceiptExclude = 4;

}

export enum JournalizingName{
  "入金仕訳",
  "消込仕訳",
  "前受計上仕訳",
  "前受振替仕訳",
  "対象外仕訳",
}
