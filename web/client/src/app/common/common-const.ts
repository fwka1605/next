export class ErrorCode {

  /// <summary>エラーなし</summary>
  public static readonly None: string = "0";

  /// <summary>原因の詳細を特定していない内部例外が発生</summary>
  public static readonly ExceptionOccured: string = "99";


  /// <summary>認証キーが不正です。</summary>
  public static readonly InvalidAuthenticationKey: string = "110";
  /// <summary>DB接続に失敗しました。</summary>
  public static readonly CompanyDataBaseConnectionFailure: string = "111";
  /// <summary>セッションキーの生成に失敗しました。</summary>
  public static readonly SessionKeyCreationFailure: string = "112";
  /// <summary>会社コードが不正です。</summary>
  public static readonly InvalidCompanyCode: string = "113";

  /// <summary>セッションキーが無効です</summary>
  public static readonly InvalidSessionKey: string = "121";
  /// <summary>セッションキーが有効期限切れです</summary>
  public static readonly SessionKeyExpired: string = "122";

  /// <summary>更新系：他ユーザーにより更新済</summary>
  public static readonly OtherUserAlreadyUpdated: string = "4031";
}

export class Constants {
  /// <summary>
  /// 標準通貨コード JPY
  /// </summary>
  public static readonly DefaultCurrencyCode: string = "JPY";
  /// <summary>
  /// リモートデスクトップサービスのクライアントパス
  /// </summary>
  public static readonly DefaultClient: string = "\\\\tsclient\\";
  /// <summary>
  /// 最大金額 11桁入力用
  /// </summary>
  public static readonly MaxAmount: number = 99999999999;
  /// <summary>
  /// 合計最大金額 13桁 表示用
  /// </summary>
  public static readonly MaxAmountTotal: number = 9999999999999;
  /// <summary>
  /// 請求書発行　備考欄印字用制限文字数 60Byte
  /// 「売上日を取引日として明細に表示」と「明細に数量・単価。単位の表示」が「する」前提で計算
  /// </summary>
  public static readonly NoteInputByteCount: number = 60;

  public static readonly StandardApplicationName: string = "Victory ONE G4";
  public static readonly CloudApplicationName: string = "V-ONE Cloud";

}

//////////////////////////////////////////////////////////////////////////////////////////////////

/// <summary> 請求 入力区分</summary>
export enum BillingInputType {
  /// <summary>請求フリーインポーター</summary>
  Importer = 1,
  /// <summary>請求データ入力</summary>
  BillingInput = 2,
  /// <summary>期日現金管理</summary>
  CashOnDueDate = 3,
  /// <summary>定期請求データ</summary>
  PeriodicBilling = 4,
  /// <summary>ファクタリング債権</summary>
  Factoring = 5,  
}

/// <summary>入金 入力区分</summary>
export enum ReceiptInputType {
  /// <summary>EBデータ取込</summary>
  EbFile = 1,
  /// <summary>入金データ入力</summary>
  ReceiptInput = 2,
  /// <summary>入金フリーインポーター</summary>
  Importer = 3,
  /// <summary>でんさい取込</summary>
  Densai = 4,
}

/// <summary>消込区分 CheckBox フラグ判定用</summary>
export enum AssignmentFlagChecked {
  /// <summary>未選択</summary>
  None = 0x000,
  /// <summary>未消込   が選択されている</summary>
  NoAssignment = 0x001,
  /// <summary>一部消込 が選択されている</summary>
  PartAssignment = 0x002,
  /// <summary>消込済   が選択されている</summary>
  FullAssignment = 0x004,
  /// <summary>全選択</summary>
  All = NoAssignment | PartAssignment | FullAssignment,
}

/// <summary>ファイル形式</summary>
export enum FileFieldTypes {
  /// <summary>カンマ区切り</summary>
  CommaDelimited = 1,
  /// <summary>タブ区切り</summary>
  TabDelimited = 2,
  /// <summary>固定長 （改行あり）</summary>
  FixedLength = 4,
  /// <summary>固定長 （改行なし）</summary>
  FixedLengthNoLineBreak = 8,
  /// <summary>すべて</summary>
  All = CommaDelimited | TabDelimited | FixedLength | FixedLengthNoLineBreak
}

/// <summary>フリーインポーターフォーマット</summary>
export enum FreeImporterFormatType {
  /// <summary>請求フリーインポーター</summary>
  Billing = 1,
  /// <summary>入金フリーインポーター</summary>
  Receipt = 2,
  /// <summary>入金予定フリーインポーター</summary>
  PaymentSchedule = 3,
  /// <summary>得意先マスターインポーター</summary>
  Customer = 4,
}

/// <summary>タイムスケジューラ　インポート種類</summary>
export enum TaskScheduleImportType {
  /// <summary>得意先マスターインポーター</summary>
  Customer = 0,
  /// <summary>債権代表者マスターインポーター</summary>
  CustomerGroup = 1,
  /// <summary>請求フリーインポーター</summary>
  Billing = 2,
  /// <summary>EBデータ取り込み</summary>
  EbData = 3,
  /// <summary>入金フリーインポーター</summary>
  Receipt = 4,
  /// <summary>入金予定フリーインポーター</summary>
  PaymentSchedule = 5,
}

/// <summary>CSVエクスポート項目設定</summary>
export enum CsvExportFileType {
  /// <summary>消込済み入金データ</summary>
  MatchedReceiptData = 1,
  /// <summary>請求書発行データ</summary>
  PublishInvoiceData = 2,
}

/// <summary>入金状態ステータス</summary>
export enum ReceiptStatus {
  /// <summary>空欄</summary>
  None = 0,
  /// <summary>元の入金データが削除されています。</summary>
  Deleted = 1,
  /// <summary>仕訳出力済みのデータです。</summary>
  Journalized = 2,
  /// <summary>一部消込、もしくは消込済みのデータです。</summary>
  PartOrFullAssigned = 3,
  /// <summary>前受振替・分割が行われています。</summary>
  AdvancedReceived = 4,
  /// <summary>入金部門振替が行われています。</summary>
  SectionTransfered = 5
}

export enum FolderBrowserType {
  /// <summary>ファイルの選択</summary>
  SelectFile,
  /// <summary>ファイルの選択</summary>
  SelectMultiFile,
  /// <summary>ファイルの保存</summary>
  SaveFile,
  /// <summary>フォルダーの選択</summary>
  SelectFolder,
  /// <summary>フォルダーの選択【新規作成・削除可】</summary>
  SelectAndEditFolder,
  /// <summary>フォルダーの選択</summary>
  SelectClientFolder
}

export enum CompanyLogoType {
  /// <summary>ロゴ</summary>
  Logo = 0,
  /// <summary>社判</summary>
  SquareSeal = 1,
  /// <summary>丸印</summary>
  RoundSeal = 2,
}


/// <summary>StatusMaster.StatusType</summary>
export enum StatusType {
  /// <summary>督促状</summary>
  Reminder = 1,
}
export enum TaxClassId {
  /// <summary>
  /// 外税課税
  /// </summary>
  TaxExclusive = 0,
  /// <summary>
  /// 内税課税
  /// </summary>
  TaxInclusive,
  /// <summary>
  /// 非課税
  /// </summary>
  TaxFree,
  /// <summary>
  /// 免税
  /// </summary>
  TaxExemption,
  /// <summary>
  /// 対象外
  /// </summary>
  NotCovered
}

/// <summary>口座振替フォーマットId</summary>
export enum AccountTransferFileFormatId {
  /// <summary>
  /// 全銀（口座振替 カンマ区切り）
  /// </summary>
  ZenginCsv = 1,
  /// <summary>
  /// 全銀（口座振替 固定長）
  /// </summary>
  ZenginFixed = 2,
  /// <summary>
  /// みずほファクター（Web伝送）
  /// </summary>
  MizuhoFactorWebFixed = 3,
  /// <summary>
  /// 三菱UFJファクター
  /// </summary>
  MitsubishiUfjFactorCsv = 4,
  /// <summary>
  /// SMBC（口座振替 固定長）
  /// </summary>
  SMBCFixed = 5,
  /// <summary>
  /// 三菱UFJニコス
  /// </summary>
  MitsubishiUfjNicosCsv = 6,
  /// <summary>
  /// みずほファクター（ASPサービス）
  /// </summary>
  MizuhoFactorAspCsv = 7,
  /// <summary>
  /// リコーリースコレクト！
  /// </summary>
  RicohLeaseCollectCsv = 8,
  /// <summary>
  /// インターネット伝送ゆうちょ形式
  /// </summary>
  InternetJPBankFixed = 9,
  /// <summary>
  /// りそなネット
  /// </summary>
  RisonaNetCsv = 10
}

/// <summary>一括消込 入金情報表示順対象項目</summary>
export enum SequencialCollationSortColumn {
  /// <summary>振込依頼人名</summary>
  PayerName = 0,
  /// <summary>入金日</summary>
  ReceiptRecordedAt = 1,
  /// <summary>入金ID</summary>
  ReceiptId = 2,
}

/// <summary>一括消込 入金情報表示順種類</summary>
export enum SortOrderColumnType {
  /// <summary>請求情報表示順</summary>
  BillingDisplayOrder = 0,
  /// <summary>振込依頼人名・昇順</summary>
  PayerNameAsc,
  /// <summary>振込依頼人名・降順</summary>
  PayerNameDesc,
  /// <summary>入金日・昇順</summary>
  MinRecordedAt,
  /// <summary>入金日・降順</summary>
  MaxRecordedAt,
  /// <summary>入金ID・昇順</summary>
  MinReceiptId,
  /// <summary>入金ID・降順</summary>
  MaxReceiptId,
}

export enum SortOrderType {
  /// <summary>昇順</summary>
  Ascending = 0,
  /// <summary>降順</summary>
  Descending,
}

/// <summary> 督促共通設定・督促管理単位 </summary>
export enum ReminderManagementMode {
  /// <summary> 督促単位毎 </summary>
  ByReminder = 0,
  /// <summary> 得意先毎 </summary>
  ByCustomer = 1,
}

/// <summary> 督促共通設定・請求部門計算方法 </summary>
export enum DepartmentSummaryMode {
  /// <summary> 営業担当者の所属部門で集計 </summary>
  StaffDepartment = 0,
  /// <summary> 請求データの請求部門で集計 </summary>
  BillDepartment = 1,
}

/// <summary> 督促共通設定・滞留日数計算方法 </summary>
export enum CalculateBaseDate {
  /// <summary> 当初予定日 </summary>
  OriginalDueAt = 0,
  /// <summary> 入金予定日 </summary>
  DueAt = 1,
  /// <summary> 請求日 </summary>
  BilledAt = 2,
}

/// <summary> 督促共通設定・滞留日数計算に当日を含める </summary>
export enum IncludeOnTheDay {
  /// <summary> 含めない </summary>
  Exclude = 0,
  /// <summary> 含める </summary>
  Include = 1,
}

/// <summary> メール配信テンプレート用メール区分 </summary>
export enum MailType {
  /// <summary> 回収通知メール </summary>
  Payment = 1,
  /// <summary> 回収遅延通知メール </summary>
  PaymentDelay = 2,
  /// <summary> 未消込入金メール </summary>
  NoAssignment = 3,
}
