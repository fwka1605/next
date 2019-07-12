import { SEP } from "@angular/material";

export const CATEGORY_TYPE_DICTIONARY = [
  { id: 1, val: "請求" },
  { id: 2, val: "入金" },
  { id: 3, val: "回収" },
  { id: 4, val: "対象外" }
];

export enum CategoryType {
  Billing = 1,
  Receipt = 2,
  Collection = 3,
  Exclude = 4,
}

export const MATCHING_PROCESS_TYPE_DICTIONARY = [
    /// <summary>一括消込</summary>
    { id: 0, val: "一括" },
    /// <summary>個別消込</summary>
    { id: 1, val: "個別" },
    /// <summary>決済結果自動照合</summary>
    { id: 2, val: "自動照合" },
    /// <summary>決済結果手動照合</summary>
    { id: 3, val: "手動照合" },
];



export enum ImportMethod {
  /// <summary> 上書 </summary>
  Replace,
  /// <summary> 追加 </summary>
  InsertOnly,
  /// <summary> 更新 </summary>
  InsertAndUpdate,
}

export enum GridId {

  ///  1 : 請求データ検索(未消込削除)
  BillingSearch = 1,
  ///  2 : 入金データ検索(未消込削除)
  ReceiptSearch = 2,
  ///  3 : 請求データ 個別消込明細
  BillingMatchingIndividual = 3,
  ///  4 : 入金データ 個別消込明細
  ReceiptMatchingIndividual = 4,
  ///  5 : 入金予定入力
  PaymentScheduleInput = 5,
  /// 6：請求書発行
  BillingInvoicePublish = 6,
}


export enum FunctionType {
  /// <summary>0:マスターインポート</summary>
  MasterImport = 0,
  /// <summary>1:マスターエクスポート</summary>
  MasterExport,
  /// <summary>2:請求データ修正・削除</summary>
  ModifyBilling,
  /// <summary>3:請求データ復活</summary>
  RecoverBilling,
  /// <summary>4:入金データ修正・削除</summary>
  ModifyReceipt,
  /// <summary>5:入金データ復活</summary>
  RecoverReceipt,
  /// <summary>6:消込解除</summary>
  CancelMatching,
}

/// <summary>端数処理種別</summary>
export enum RoundingType
{
  /// <summary>0:切り捨て</summary>
  Floor = 0,
  /// <summary>1:切り上げ</summary>
  Ceil,
  /// <summary>2:四捨五入</summary>
  Round,
  /// <summary>3:取込不可</summary>
  Error,
}

export enum TaxClassId
{
  /// <summary> 外税課税 </summary>
  TaxExclusive = 0,
  /// <summary> 内税課税 </summary>
  TaxInclusive,
  /// <summary> 非課税 </summary>
  TaxFree,
  /// <summary> 免税 </summary>
  TaxExemption,
  /// <summary> 対象外 </summary>
  NotCovered
}

export enum AccountTransferFileFormatId
{
  /// <summary>全銀（口座振替 カンマ区切り）</summary>
  ZenginCsv = 1,
  /// <summary>全銀（口座振替 固定長）</summary>
  ZenginFixed = 2,
  /// <summary>みずほファクター（Web伝送）</summary>
  MizuhoFactorWebFixed = 3,
  /// <summary>三菱UFJファクター</summary>
  MitsubishiUfjFactorCsv = 4,
  /// <summary>SMBC（口座振替 固定長）</summary>
  SMBCFixed = 5,
  /// <summary>三菱UFJニコス</summary>
  MitsubishiUfjNicosCsv = 6,
  /// <summary>みずほファクター（ASPサービス）</summary>
  MizuhoFactorAspCsv = 7,
  /// <summary>リコーリースコレクト！</summary>
  RicohLeaseCollectCsv = 8,
  /// <summary>インターネット伝送ゆうちょ形式</summary>
  InternetJPBankFixed = 9,
  /// <summary>りそなネット</summary>
  RisonaNetCsv = 10,
  /// <summary>しんきん情報サービス</summary>
  ShinkinJohoService = 11,
}

/// <summary>外部連携 API のタイプ</summary>
export enum WebApiType
{
  /// <summary>1 : 働くDB</summary>
  HatarakuDb = 1,
  /// <summary>2 : PCA クラウド</summary>
  PcaDx = 2,
  /// <summary>3 : Money Forward クラウド</summary>
  MoneyForward = 3,
  /// <summary>4 : 奉行クラウド</summary>
  BugyoCloud = 4,
  /// <summary> 5 : 入金データ自動連携</summary>
  MfAggregation = 5,

}

export const TAX_CLASS_TYPE_DICTIONARY = [
  { id: 0, val: "外税課税" },
  { id: 1, val: "内税課税" },
  { id: 2, val: "非課税" },
  { id: 3, val: "免税" },
  { id: 4, val: "対象外" }
];

export const ACCOUNT_TYPE_DICTIONARY = [
  { id: 0, val: "全て" },
  { id: 1, val: "普通預金" },
  { id: 2, val: "当座預金" },
  { id: 4, val: "貯蓄預金" },
  { id: 5, val: "通知預金" }
];

export const EXCLUSIVE_ACCOUNT_TYPE = [
  { id: 1, val: "普通預金" },
  { id: 2, val: "当座預金" },
  { id: 4, val: "貯蓄預金" },
  { id: 5, val: "通知預金" }
];

export const TRANSFER_ACCOUNT_TYPE = [
  { id: 1, val: "普通" },
  { id: 2, val: "当座" },
  { id: 3, val: "納税準備" },
  { id: 9, val: "その他" }
];

export const SHARE_TRANSFER_FEE_DICTIONARY = [
  { id: 0, val: "相手先" },
  { id: 1, val: "自社" },
];

export const BILL_INPUT_TYPE_DICTIONARY = [
  { id: 0, val: "全て" },
  { id: 1, val: "取込" },
  { id: 2, val: "入力" },
  { id: 3, val: "期日入金予定" },
  { id: 4, val: "定期請求" }
];

export const RECEIPT_INPUT_TYPE_DICTIONARY = [
  { id: 0, val: "全て" },
  { id: 1, val: "EB取込" },
  { id: 2, val: "入力" },
  { id: 3, val: "インポーター取込" },
  { id: 4, val: "電債取込" }
];

export const RECEIPT_EXCLUDE_FLAG_DICTIONARY = [
  { id: 0, val: "通常入金" },
  { id: 1, val: "対象外入金" },
  { id: 2, val: "すべて" },
];

export const MATCHING_ASSIGNMENT_FLAG_DICTIONARY = [
  { id: 1, val: "未消込" },
  { id: 2, val: "一部消込" },
  { id: 4, val: "消込済" },
];

export const BANK_TRANSFER_RESULT_DICTIONARY = [
  { id: 0, val: "振替済" },
  { id: 1, val: "振替不能" },
];

export const BILLING_AMOUNT_RANGE_DICTIONARY = [
  { id: 0, val: "請求金額（税込）" },
  { id: 1, val: "請求残" },
];

export const RECEIPT_AMOUNT_RANGE_DICTIONARY = [
  { id: 0, val: "入金額" },
  { id: 1, val: "入金残" },
];

export const MATCHING_BILLING_TYPE_DICTIONARY = [
  { id: 0, val: "すべて" },
  { id: 1, val: "通常請求" },
  { id: 2, val: "期日入金予定" },
  { id: 3, val: "ファクタリング債権" },
];

export const FRACTIONAL_UNIT = [
  { id: 0, val: "端数" },
  { id: 1, val: "一" },
  { id: 2, val: "十" },
  { id: 3, val: "百" },
  { id: 4, val: "千" },
  { id: 5, val: "万" },
  { id: 6, val: "十万" },
];

export const CUSTOMER_HOLIDAY_FLAG_DICTIONARY = [
  { id: 0, val: "考慮しない" },
  { id: 1, val: "休業日の前" },
  { id: 2, val: "休業日の後" },
];

export const USE_LIMIT_DATE_DICTIONARY = [
  { id: 0, val: "行わない" },
  { id: 1, val: "行う" }
];

export const ADVANCED_RECEIVED_RECORDED_DATA_TYPE = [
  { id: 0, val: '未入力'       },
  { id: 1, val: 'システム日付' },
  { id: 2, val: '請求日'       },
  { id: 3, val: '売上日'       },
  { id: 4, val: '請求締日'     },
  { id: 5, val: '入金予定日'   },
  { id: 6, val: '入金日'       },
];

export const MFBILLING_PAYMENT_STATUS = [
  { id: 0, val: "未設定" },
  { id: 1, val: "未入金" },
  { id: 2, val: "入金済み" },
];

export const CUSTOMER_PARENT_FLAG_DICTIONARY = [
  { id: 0, val: "通常得意先" },
  { id: 1, val: "債権代表者" },
];

export const YES_NO_FLAG_DICTIONARY = [
  { id: 0, val: "しない" },
  { id: 1, val: "する" },
];

export enum CODE_TYPE {
  NUMBER=0,
  ALPHA=1,
  HANKANA=2
}

export enum CheckBoxStatus{
  ON=0,
  OFF=1
}


/// <summary>
/// 消込エラータイプ
/// </summary>
export enum MatchingErrorType
{
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

export const RECEIPT_STATUS_MESSAGE_DICTIONARY = [
  /// <summary>None:空欄</summary>
  { id: 0, val: "" },
  /// <summary>Delete:削除済み</summary>
  { id: 1, val: "元の入金データが削除されています。" },
  /// <summary>Journalized：仕訳出力済</summary>
  { id: 2, val: "仕訳出力済みのデータです。" },
  /// <summary>PartOrFullAssigned:一部消込、もしくは消込済</summary>
  { id: 3, val: "一部消込、もしくは消込済みのデータです。" },
  /// <summary>AdvancedReceived:前受振替・分割済み</summary>
  { id: 4, val: "前受振替・分割が行われています。" },
  /// <summary>SectionTransfered:入金部門振替済み</summary>
  { id: 5, val: "入金部門振替が行われています。" }
];


export enum AutoCompleteType {
  Customer = 1,
}

/// <summary>
/// CSVエクスポート項目設定
/// </summary>
export enum CsvExportFileType
{
  /// <summary>消込済み入金データ</summary>
  MatchedReceiptData = 1,
  /// <summary>請求書発行データ</summary>
  PublishInvoiceData = 2,
}


export enum LoginResult
{
    /// <summary>データベースエラー</summary>
    DBError = 0,
    /// <summary>成功</summary>
    Success = 1,
    /// <summary>失敗</summary>
    Failed = 2,
    /// <summary>期限切れ</summary>
    Expired = 3,
}