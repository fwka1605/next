namespace Rac.VOne.AccountTransfer.Import
{
    /// <summary>
    /// 状態遷移処理結果
    /// </summary>
    public enum RecordOrderStateTransitionResult
    {
        Success,

        NoHeader,
        DuplicateHeader,
        NoData,
        NoTrailer,
        DuplicateTrailer,
        NoEnd,
        DuplicateEnd,

        AlreadyCompleted,
        AlreadyError,
        AlreadyEnded,

        MultiHeaderNotSupported,
        TrailerNotSupported,
        EndNotSupported,
    }
}
