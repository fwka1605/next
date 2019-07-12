namespace Rac.VOne.AccountTransfer.Import
{
    public enum RecordOrderState
    {
        /// <summary>読み込み開始時点。まだレコードを読み込んでいない。</summary>
        Start,

        /// <summary>ヘッダーレコードを読み込んだ状態</summary>
        Header,

        /// <summary>データレコードを読み込んだ状態</summary>
        Data,

        /// <summary>トレーラーレコードを読み込んだ状態</summary>
        Trailer,

        /// <summary>エンドレコードを読み込んだ状態</summary>
        End,

        /// <summary>全レコードを読み込み終えた(EOF)状態</summary>
        Completed,

        /// <summary>何らかの異常(レコード順序エラー)が発生している状態</summary>
        Error,
    }
}
