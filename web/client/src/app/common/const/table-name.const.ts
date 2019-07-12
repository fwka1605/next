export enum TABLE_INDEX {
  MASTER_CUSTOMER,
  MASTER_PARENT_CUSTOMER,
  MASTER_DEPARTMENT,
  MASTER_STAFF,
  MASTER_DESTINATION,
  MASTER_BILLING_IMPORTER_SETTING,
  MASTER_RECEIPT_IMPORTER_SETTING,
  MASTER_SCHEDULE_PAYMENT_IMPORTER_SETTING,
  MASTER_CUSTOMER_IMPORTER_SETTING,
  MASTER_CATEGORY,
  MASTER_BILLING_CATEGORY,
  MASTER_RECEIPT_CATEGORY,
  MASTER_COLLECT_CATEGORY,
  MASTER_EXCLUDE_CATEGORY,
  MASTER_LOGIN_USER,
  MASTER_SECTION,
  MASTER_TAX_CLASS,
  MASTER_ACCOUNT_TITLE,
  MASTER_CURRENCY,
  MASTER_PAYMENT_AGENCY,
  MASTER_ACCOUNT_TYPE,
  MASTER_BANK_ACCOUNT,
  MASTER_BANK_BRANCH,
  MASTER_JURDICAL_PERSONALITY,
  MASTER_IGNORE_KANA,
  MASTER_HOLIDAY_CALENDAR,
  MASTER_KANA_HISTORY_CUSTOMER,
  MASTER_KANA_HISTORY_PAYMENT_AGENCY,
  MASTER_CUSTOMER_GROUP,
  MASTER_CUSTOMER_FEE,
  MASTER_CUSTOMER_PARENT,
  MASTER_SHARE_TRANSFER_FEE,
  MASTER_PARENT_CUSTOMER_FLAG,
  MASTER_CUSTOMER_HOLIDAY_FLAG,
  USE_FEE_LEARNING_YES_NO_FLAG,
  USE_KANA_LEARNING_YES_NO_FLAG,
  USE_FEE_TOLERANCE_YES_NO_FLAG,
  PRIORITIZE_MATCHING_INDIVIDUAL_YES_NO_FLAG,
  MASTER_SECTION_WITH_LOGINUSER,
  MASTER_SECTION_WITH_DEPARTMENT,
  MASTER_LOGIN_USER_SECTION,
  MASTER_RECEIPT_CATEGORY_EXCLUDE_USE_LIMIT_DATE

}

export const TABLE_NAME = [
  "得意先",
  "債権代表者",
  "請求部門",
  "営業担当者",
  "送り先",
  "請求取込パターン",
  "入金取込パターン",
  "入金予定取込パターン",
  "得意先取込パターン",
  "区分",
  "請求区分",
  "入金区分",
  "回収区分",
  "対象外区分",
  "ログインユーザー",
  "入金部門",
  "税区分",
  "科目",
  "通貨",
  "決済会社",
  "預金種別",
  "銀行・預金",
  "銀行・支店",
  "法人",
  "除外カナ",
  "カレンダー",
  "学習履歴データ管理(得意先)",
  "学習履歴データ管理(振込依頼人)",
  "債権代表者マスター",
  "登録手数料",
  "得意先",
  "手数料負担区分検索",
  "債権代表者フラグ",
  "休業日の設定",
  "手数料自動学習",
  "カナ自動学習",
  "手数料誤差利用",
  "一括消込対象外",
  "",
  "請求部門",
  "最終更新者",
  "入金区分"
]

export const TABLES = ({
  MASTER_CUSTOMER: { id: 1, name: "得意先" },
  MASTER_DEPARTMENT: { id: 2, name:"請求部門"},
  MASTER_STAFF: { id: 3, name: "営業担当者" },
  MASTER_CATEGORY: { id: 9, name: "区分" },
  MASTER_LOGIN_USER: { id: 14, name: "ログインユーザー" },
  MASTER_BANK_ACCOUNT: { id: 21, name: "銀行・預金" },
  MASTER_BANK_BRANCH: { id: 22, name: "銀行・支店"},
  MASTER_IGNORE_KANA: { id: 24, name: "除外カナ" },
  MASTER_KANA_HISTORY_CUSTOMER: { id: 26, name: "学習履歴データ管理(得意先)"},
  MASTER_CUSTOMER_GROUP: { id: 28, name: "債権代表者マスター"},
  
  BILLING: { id: 30, name: "請求情報" },
  SECTION_WITH_DEPARTMENT: { id: 31, name: "部署情報" },
  RECEIPT: { id: 32, name: "領収書情報" },
  MASTER_NETTING: { id: 33, name: "入金予定相殺" },
  MASTER_CUSTOMER_DISCOUNT: { id: 34, name: "歩引き情報" },
  MASTER_CUSTOMER_PAYMENT_CONTRACT: { id: 35, name: "約定金額情報" },
  SECTION_WITH_LOGINUSER: { id: TABLE_INDEX.MASTER_SECTION, name: "入金部門情報" },
  MASTER_SECTION_WITH_DEPARTMENT: { id: TABLE_INDEX.MASTER_SECTION_WITH_DEPARTMENT, name: "入金・請求部門情報" },
  MASTER_SECTION_WITH_LOGINUSER: { id: TABLE_INDEX.MASTER_SECTION_WITH_LOGINUSER, name: "入金・担当者対応情報" },
})

export const TABLE_COLUMN = [
  ["", "得意先コード", "得意先名", "得意先カナ"],
  ["", "債権代表者コード", "債権代表者名", "債権代表者カナ"],
  ["", "請求部門コード", "請求部門名"],
  ["", "営業担当者コード", "営業担当者名",],
  ["", "送り先コード", "送り先コード",],
  ["", "請求取込パターンコード", "請求取込パターン名",],
  ["", "入金取込パターンコード", "入金取込パターン名",],
  ["", "入金予定取込パターンコード", "入金予定取込パターン名",],
  ["", "得意先取込パターンコード", "得意先取込パターン名",],
  [], // 区分はマスタ検索しないため未定義。
  ["", "請求区分コード", "請求区分名",],
  ["", "入金区分コード", "入金区分名",],
  ["", "回収区分コード", "回収区分名",],
  ["", "対象外区分コード", "対象外区分名",],
  ["", "ログインユーザーコード", "ログインユーザー名",],
  ["", "入金部門コード", "入金部門名",],
  ["", "税区分コード", "税区分名",],
  ["", "科目コード", "科目名",],
  ["", "通貨コード", "通貨コード名",],
  ["", "決済会社コード", "決済会社名称","決済会社名称カナ"],
  ["", "預金種別", "預金種別名",],
  [], // 銀行預金は専用のモーダルを利用するため、未定義。
  [], // 銀行・支店は専用のモーダルを利用するため、未定義。
  [], // 法人はマスタ検索しないため未定義。
  [], // 除外カナはマスタ検索しないため未定義。
  [], // カレンダーはマスタ検索しないため未定義。
  [], // 学習履歴データ管理(得意先)
  [], // 学習履歴データ管理(振込依頼人)
  [], // 債権代表者マスター
  [], // 登録手数料
  ["", "得意先コード", "得意先名", "得意先カナ"], // 債権代表者用得意先
  ["", "手数料負担区分", "手数料負担区分名",],  //  手数料負担区分検索
  ["", "項目選択", "項目名",],                // 債権代表者フラグ
  ["", "休業日の設定", "項目名",],            // 休業日の設定
  ["", "項目選択", "項目名",],                // 手数料自動学習
  ["", "項目選択", "項目名",],                // カナ自動学習
  ["", "項目選択", "項目名",],                // 手数料誤差利用
  ["", "項目選択", "項目名",],                // 一括消込対象外
  [],
  ["", "請求部門コード", "請求部門名",],       // 請求部門名コード
  ["", "ユーザーコード", "ユーザー名","請求部門コード", "請求部門名"],       // 請求部門名コード
  ["", "入金区分コード", "入金区分名",],        //  入金区分(期日入力を行わないのみ)

]


/** 表示するテーブルの最大行数 */
export const TABLE_MAX_DISPLAY_INDEX = 1000;