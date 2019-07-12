
/// <summary>
/// 消込エラータイプ
/// </summary>
export enum MatchingErrorType {
    /// <summary>エラーなし</summary>
    None = 0,
    /// <summary>請求残相違</summary>
    BillingRemainChanged,
    /// <summary>請求歩引額相違</summary>
    BillingDiscountChanged,
    /// <summary>入金残相違</summary>
    ReceiptRemainChanged,
    /// <summary>請求(論理)削除済</summary>
    BillingOmitted,
    /// <summary>入金(論理)削除済</summary>
    ReceiptOmitted,
    /// <summary>期日現金管理(論理)削除済</summary>
    CashOnDueDateOmitted,
    /// <summary>消込取消 請求データなし</summary>
    NotExistBillingData,
    /// <summary>消込取消 入金データなし</summary>
    NotExistReceiptData,
    /// <summary>消込取消 汎用</summary>
    CancelError,
    /// <summary>処理キャンセル</summary>
    ProcessCanceled,
    /// <summary>なんらかDBエラー</summary>
    DBError,
    /// <summary>連携処理エラー</summary>
    PostProcessError,
    /// <summary>消込ヘッダー変更済/承認フラグの書き換え</summary>
    MatchingHeaderChanged,
}

/**
 * パスワードエラータイプ
 */
export enum PasswordValidateResult {
    /** 有効 */
    Valid = 0,
    /** アルファベット禁止 */
    ProhibitionAlphabetChar,
    /** 数字禁止 */
    ProhibitionNumberChar,
    /** 記号禁止 */
    ProhibitionSymbolChar,
    /** 不許可の記号禁止 */
    ProhibitionNotAllowedSymbolChar,
    /** アルファベット文字数不足 */
    ShortageAlphabetCharCount,
    /** 数字文字数不足 */
    ShortageNumberCharCount,
    /** 記号文字数不足 */
    ShortageSymbolCharCount,
    /** パスワード文字数不足 */
    ShortagePasswordLength,
    /** パスワード文字数超過 */
    ExceedPasswordLength,
    /** 連続同一文字 文字数超過 */
    ExceedSameRepeatedChar
}    
