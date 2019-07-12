namespace Rac.VOne.Client.Reports
{
    /// <summary>
    ///  請求データ集計
    /// </summary>
    public enum ReportTotalByDay
    {
        /// <summary>
        ///  0 : する(請求区分)
        /// </summary>
        ByBillingCategory = 0,
        /// <summary>
        ///  1 : する(債権科目)
        /// </summary>
        ByCreditAccountTitle = 1,
        /// <summary>
        ///  2 : しない
        /// </summary>
        None = 2,

    }
}
