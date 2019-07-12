export const LINE_FEED_CODE = '\r\n';

export const REPORT_ITEM_ID = {
  ReportAdvanceReceivedType: "ReportAdvanceReceivedType",
  ReportBaseDate: "ReportBaseDate",
  ReportBaseDateWithOriginal: "ReportBaseDateWithOriginal",
  ReportCalcBaseDate: "ReportCalcBaseDate",
  ReportCreditLimitType: "ReportCreditLimitType",
  ReportCustomerGroup: "ReportCustomerGroup",
  ReportDisplayType: "ReportDisplayType",
  ReportDoOrNot: "ReportDoOrNot",
  ReportIncludeOrNot: "ReportIncludeOrNot",
  ReportInvoiceAmount: "ReportInvoiceAmount",
  ReportInvoiceDate: "ReportInvoiceDate",
  ReportOutputOrder: "ReportOutputOrder",
  ReportReceiptType: "ReportReceiptType",
  ReportStaffSelection: "ReportStaffSelection",
  ReportSubtotalUnit: "ReportSubtotalUnit",
  ReportTargetDate: "ReportTargetDate",
  ReportTotal: "ReportTotal",
  ReportTotalByDay: "ReportTotalByDay",
  ReportUnitPrice: "ReportUnitPrice",
};

export const REPORT_HEADER =
{
  LOGIN_USER_MASTER: ["会社コード", "ログインユーザーコード", "ログインユーザー名", "請求部門コード", "メールアドレス", "権限レベル", "セキュリティ", "営業担当者コード", "初回パスワード"],
  LOGIN_USER_MASTER_OPTION: ["V-ONE利用", "WebViewer利用"],
  DEPARTMENT_MASTER: ["会社コード", "請求部門コード", "請求部門名", "回収責任者コード", "備考"],
  STAFF_MASTER: ["会社コード", "営業担当者コード", "営業担当者名", "請求部門コード", "メールアドレス", "電話番号", "FAX番号"],
  CUSTOMERGROUP_MASTER: ["会社コード", "債権代表者コード", "得意先コード"],
  BANK_ACCOUNT_MASTER: ["会社コード", "銀行コード", "銀行名", "支店コード", "支店名", "預金種別", "口座番号", "入金区分コード", "取込対象外"],
  BANK_ACCOUNT_MASTER_OPTION: ["入金部門コード"],
  ACCOUNT_TITLE_MASTER: ["会社コード", "科目コード", "科目名", "相手科目コード", "相手科目名", "相手科目補助コード"],
  KANA_HISTORY_CUSTOMER_MASTER: ["会社コード", "振込依頼人名", "得意先コード", "仕向銀行", "仕向支店", "消込回数"],
  KANA_HISTORY_PAYMENT_AGENCY_MASTER: ["会社コード", "振込依頼人名", "決済代行会社コード", "仕向銀行", "仕向支店", "消込回数"],
  IGNORE_KANA_MASTER: ["会社コード", "カナ名", "対象外区分コード"],
  HOLIDAY_CALENDAR_MASTER: ["会社コード", "休業日"],
  JURIDICAL_PERSONALITY_MASTER: ["会社コード", "法人挌"],
  BANK_BRANCH_MASTER: ["銀行コード", "支店コード", "銀行名カナ", "銀行名", "支店名カナ", "支店名"],
  CUSTOMER_MASTER: ["会社コード", "得意先コード", "得意先名", "得意先名カナ", "専用銀行コード", "専用銀行名", "専用支店コード",
    "専用支店名", "専用入金口座番号", "預金種別", "手数料負担区分", "与信限度額", "締日", "回収方法", "回収予定（月）", "回収予定（日）",
    "営業担当者", "債権代表者フラグ", "郵便番号", "住所1", "住所2", "TEL番号", "FAX番号", "相手先担当者名", "備考",
    "手数料自動学習", "回収サイト", "電子手形用企業コード", "信用調査用企業コード", "与信ランク", "口座振替用銀行コード",
    "口座振替用銀行名", "口座振替用支店コード", "口座振替用支店名", "口座振替用口座番号", "口座振替用預金種別", "口座振替用顧客コード",
    "口座振替用新規コード", "口座振替用預金者名", "約定金額", "約定金額未満", "約定金額以上1", "分割1", "端数1", "回収サイト1",
    "約定金額以上2", "分割2", "端数2", "回収サイト2", "約定金額以上3", "分割3", "端数3", "回収サイト3", "カナ自動学習", "休業日設定",
    "手数料誤差利用", "一括消込対象外", "照合番号", "相手先部署", "敬称"],
  CUSTOMER_MASTER_FEE: ["会社コード", "得意先コード", "登録手数料"],

  MATCHING_SEQUENTIAL:["一括","通貨","得意先コード","得意先名（代表者）","件数","金額","振込依頼人名","件数","金額","手数科","差額","前受"],
  MATCHING_INDIVIDUAL:["通貨コード","消","得意先コード","得意先名","請求日","売上日","予定日","請求額","請求残","歩引額","消込対象額","請求書番号","請求区分","請求部門名","備考1","備考2","備考3","備考4","請求メモ","データ区分","消","振込依頼人名","入金日","区分","入金額","入金残","仕向","銀行コード","銀行名","支店コード","支店名","種別","口座","入金部門コード","入金部門名","備考","入金メモ","期日","対象外理由","仮想支店コード","仮想口座番号","得意先コード","得意先名","備考2","備考3","備考4"],

  MF_MATCHING_JOURNALIZING:["取引No","取引日","借方勘定科目","借方補助科目","借方税区分","借方部門","借方金額(円)","借方税額","貸方勘定科目","貸方補助科目","貸方税区分","貸方部門","貸方金額(円)","貸方税額","摘要","仕訳メモ","タグ","MF仕訳タイプ","決算整理仕訳","作成日時","最終更新日時"],
  SECTION_MASTER: ["会社コード", "入金部門コード", "入金部門名", "備考", "仮想支店コード", "仮想口座番号"],
  SECTION_WITH_DEPARTMENT_MASTER: ["会社コード", "入金部門コード", "請求部門コード"],
  SECTION_WITH_LOGIN_MASTER: ["会社コード", "入金部門コード", "ログインユーザーコード"],
  BILLING_AGING_LIST_FORM: ["得意先コード","得意先名","前月請求残","当月売上高","当月入金","当月消込","当月請求残"],
  BILLING_AGING_LIST_DETAIL_FORM:["得意先コード","得意先名","通貨コード","請求日","入金予定日","売上日","請求金額","請求残","担当者コード","担当者名","請求書番号","備考"],
  CREDIT_AGING_LIST_FORM: ["得意先コード","得意先名","回収条件","当月債権総額","当月末未決済残高","当月請求残高","与信限度額","当月与信残高"],
  SCHEDULED_PAYMENT_LIST_FORM: ["基準日","請求ID","得意先コード","得意先名","請求日","売上日","請求締日","入金予定日","当初予定日","回収予定金額","遅延","回収区分","請求書番号","請求部門コード","請求部門名","担当者コード","担当者名"],
  ARREARAGES_LIST_FORM_P1: ["入金基準日","請求ID","得意先コード","得意先名","請求日","売上日","請求締日","入金予定日","当初予定日","回収予定金額","滞留日数","回収区分","請求書番号"],
  ARREARAGES_LIST_FORM_P2: ["メモ","相手先担当者","得意先備考","電話番号","請求部門コード","請求部門名","担当者コード","担当者名"],
  CUSTOMER_LEDGER_FORM: ["日付","代表得意先コード","代表得意先名","入金部門","請求部門","請求書番号","区分","債権科目","通貨コード","消込記号_請求","請求額","伝票合計","入金額","消込記号_消込","消込額","残高","得意先コード","得意先名"],
  COLLECTION_SCHEDULE_FORM: ["得意先/回収条件","締日","担当者名","請求部門名","区分"]

};

/// <summary>
///  集計基準日
/// </summary>
export enum ReportTargetDate {
  /// <summary>
  ///  0 : 請求日
  /// </summary>
  BilledAt = 0,
  /// <summary>
  ///  1 : 売上日
  /// </summary>
  SalesAt = 1,
}

/// <summary>
///  しない/する
/// </summary>
export enum ReportDoOrNot {
  /// <summary>
  ///  0 : しない
  /// </summary>
  NotDo = 0,

  /// <summary>
  ///  1 : する
  /// </summary>
  Do = 1,
}

/// <summary>
///  金額単位
/// </summary>
export enum ReportUnitPrice {
  /// <summary>
  ///  0 : 1円
  /// </summary>
  Per1 = 0,

  /// <summary>
  ///  1 : 千円
  /// </summary>
  Per1000 = 1,

  /// <summary>
  ///  2 : 万円
  /// </summary>
  Per10000 = 2,

  /// <summary>
  ///  3 : 百万円
  /// </summary>
  Per1000000 = 3,
}

/// <summary>
///  担当者集計方法
/// </summary>
export enum ReportStaffSelection {
  /// <summary>
  ///  0 : 請求データ
  /// </summary>
  ByBillingData = 0,

  /// <summary>
  ///  1 : 得意先マスター
  /// </summary>
  ByCustomerMaster = 1,
}

/// <summary>
///  日数計算基準
/// </summary>
export enum ReportCustomerGroup {
  /// <summary>
  ///  0 : 得意先
  /// </summary>
  PlainCusomter = 0,

  /// <summary>
  ///  1 : 債権代表者/得意先
  /// </summary>
  ParentWithChildren = 1,

  /// <summary>
  ///  2 : 債権代表者
  ///  適切な名前を何か
  /// </summary>
  ParentOnly = 2,
}

/// <summary>
///  得意先集計方法
/// </summary>
export enum ReportSubtotalUnit {
  /// <summary>
  ///  0 : 得意先
  /// </summary>
  PlainCustomer = 0,

  /// <summary>
  ///  1 : 債権代表者
  /// </summary>
  CustomerGroup = 1,
}

/// <summary>
///  消込額計算
/// </summary>
export enum ReportAdvanceReceivedType {
  /// <summary>
  ///  0 : 消込額を使用する
  /// </summary>
  UseMatchingAmount = 0,

  /// <summary>
  ///  1 : 入金額を使用する
  /// </summary>
  UseReceiptAmount = 1,

  /// <summary>
  ///  2 : 消込額を使用して入金額を表示
  /// </summary>
  UseMatchingAmountWithReceiptAmount = 2,
}

/// <summary>
///  与信限度額集計方法
/// </summary>
export enum ReportCreditLimitType {
  /// <summary>
  ///  0 : 得意先の与信限度額を集計する
  /// </summary>
  UseCustomerSummaryCredit = 0,
  
  /// <summary>
  ///  1 : 債権代表者の与信限度額を使用する
  /// </summary>
  UseParentCustomerCredit = 1,
}

export enum ReportReceiptType {
  /// <summary>
  ///  0 : 消込額を使用する
  /// </summary>
  UseMatchingAmount = 0,

  /// <summary>
  ///  1 : 入金額を使用する
  /// </summary>
  UseReceiptAmount = 1,  
}