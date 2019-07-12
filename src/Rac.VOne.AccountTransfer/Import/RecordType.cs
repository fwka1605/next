namespace Rac.VOne.AccountTransfer.Import
{
    /// <summary>
    /// RecordOrderStateMachineで使用する。
    /// ステートマシンの状態を遷移させるためのパラメータ。
    /// </summary>
    public enum RecordType
    {
        Header,
        Data,
        Trailer,
        End,

        /// <summary>ファイル終端</summary>
        EOF,
    }
}
