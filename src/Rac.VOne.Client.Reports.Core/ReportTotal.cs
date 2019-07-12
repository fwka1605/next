namespace Rac.VOne.Client.Reports
{
    /// <summary>
    ///  伝票集計方法
    /// </summary>
    public enum ReportTotal
    {
        /// <summary>
        ///  0 : 合計
        /// </summary>
        Total = 0,
        /// <summary>
        ///  1 : 明細(合計出力)
        /// </summary>
        DetailWithTotal = 1,
        /// <summary>
        ///  2 : 明細(合計未出力)
        /// </summary>
        DetailOnly = 2,

    }
}
