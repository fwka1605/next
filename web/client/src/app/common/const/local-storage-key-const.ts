export class GridSizeKey{
  public static readonly BILLING_SIZE="BILLING_SIZE";
  public static readonly RECEIPT_SIZE="RECEIPT_SIZE";
  public static readonly COLLATION_SIZE="BILLING_SIZE";
  public static readonly COLLATION_BILLING_SIZE="COLLATION_BILLING_SIZE";
  public static readonly COLLATION_RECEIPT_SIZE="COLLATION_RECEIPT_SIZE";
}

export class RangeSearchKey{
  public static readonly LOGIN_USER_CODE="LOGIN_USER_CODE";

  public static readonly COM0000_MENU_STATUS="COM0000_MENU_STATS";

  // 学習履歴データ管理
  public static readonly PB1001_CUSTOMER="PB1001_CUSTOMER";
  // 請求データ入力
  public static readonly PC0201_CAL_AMOUNT="PC0201_CAL_AMOUNT";
  // 請求検索
  public static readonly PC0301_DEPARTMENT="PC0301_DEPARTMENT";
  public static readonly PC0301_STAFF="PC0301_STAFF";
  public static readonly PC0301_CUSTOMER="PC0301_CUSTOMER";
  public static readonly PC0301_AMOUNT="PC0301_AMOUNT";
  public static readonly PC0301_USE_RECEIPT_SECTION="PC0301_USE_RECEIPT_SECTION";
  // 入金予定日変更
  public static readonly PC0501_DEPARTMENT="PC0501_DEPARTMENT";
  public static readonly PC0501_STAFF="PC0501_STAFF";
  public static readonly PC0501_CUSTOMER="PC0501_CUSTOMER";
  public static readonly PC0501_USE_RECEIPT_SECTION="PC0501_USE_RECEIPT_SECTION";
  // 入金EBデータ取込
  public static readonly PD0101_SELECTED_EB_FILE="PD0101_SELECTED_EB_FILE";
  // 入金検索
  public static readonly PD0501_CUSTOMER="PD0501_CUSTOMER";
  public static readonly PD0501_RECEIPT_SECTION="PD0501_RECEIPT_SECTION";
  public static readonly PD0501_RECEIPT_CATEGORY="PD0501_RECEIPT_CATEGORY";
  public static readonly PD0501_USE_RECEIPT_SECTION="PD0501_USE_RECEIPT_SECTION";
  public static readonly PD0501_AMOUNT="PD0501_AMOUNT";
  // 前受一括計上
  public static readonly PD0601_CUSTOMER="PD0601_CUSTOMER";
  public static readonly PD0601_RECEIPT_CATEGORY="PD0601_RECEIPT_CATEGORY";
  public static readonly PD0601_RECEIPT_SECTION="PC0601_RECEIPT_SECTION";
  public static readonly PD0601_USE_RECEIPT_SECTION="PD0501_USE_RECEIPT_SECTION";

  // 入金部門振分処理
  public static readonly PD0801_CUSTOMER="PD0801_CUSTOMER";
  
  // 消込仕訳出力
  public static readonly PE0201_MATCH_RECEIPT="PE0201_MATCH_RECEIPT";
  public static readonly PE0201_ADVANCE_RECEIPT_OCCUR="PE0201_ADVANCE_RECEIPT_OCCUR";
  public static readonly PE0201_ADVANCE_RECEIPT_MATCH="PE0201_ADVANCE_RECEIPT_MATCH";
  // 消込履歴データ検索
  public static readonly PE0301_CUSTOMER="PE0301_CUSTOMER";
  public static readonly PE0301_DEPARTMENT="PE0301_DEPARTMENT";
  public static readonly PE0301_LOGIN_USER="PE0301_LOGIN_USER";
  public static readonly PE0301_SECTION="PE0301_SECTION";
  public static readonly PE0301_USE_RECEIPT_SECTION="PE0301_USE_RECEIPT_SECTION";
  // 未消込請求データ削除
  public static readonly PE0501_DEPARTMENT="PE0501_DEPARTMENT";
  public static readonly PE0501_STAFF="PE0501_STAFF";
  public static readonly PE0501_CUSTOMER="PE0501_CUSTOMER";
  public static readonly PE0501_AMOUNT="PE0501_AMOUNT";
  public static readonly PE0501_USE_RECEIPT_SECTION="PE0501_USE_RECEIPT_SECTION";
  // 未消込入金データ削除
  public static readonly PE0601_CUSTOMER="PE0601_CUSTOMER";
  public static readonly PE0601_RECEIPT_CATEGORY="PE0601_RECEIPT_CATEGORY";
  public static readonly PE0601_AMOUNT="PE0601_AMOUNT";
  public static readonly PE0601_SECTION="PE0601_SECTION";
  public static readonly PE0601_USE_RECEIPT_SECTION="PE0601_USE_RECEIPT_SECTION";
  // MFクラウド会計 消込結果連携
  public static readonly PE1001_SUB_ACCOUNT_TITLE="PE1001_SUB_ACCOUNT_TITLE";
  // 請求残高年齢表
  public static readonly PF0101_DEPARTMENT="PF0101_DEPARTMENT";
  public static readonly PF0101_STAFF="PF0101_STAFF";
  public static readonly PF0101_CUSTOMER="PF0101_CUSTOMER";
  // 債権総額管理表
  public static readonly PF0201_DEPARTMENT="PF0201_DEPARTMENT";
  public static readonly PF0201_STAFF="PF0201_STAFF";
  public static readonly PF0201_CUSTOMER="PF0201_CUSTOMER";  
  // 入金予定明細表
  public static readonly PF0301_DEPARTMENT="PF0301_DEPARTMENT";
  public static readonly PF0301_STAFF="PF0301_STAFF";
  public static readonly PF0301_CUSTOMER="PF0301_CUSTOMER";
  // 滞留明細一覧表
  public static readonly PF0401_DEPARTMENT="PF0401_DEPARTMENT";
  public static readonly PF0401_STAFF="PF0401_STAFF";
  // 得意先別消込台帳
  public static readonly PF0501_CUSTOMER="PF0501_CUSTOMER";
  // 回収予定表
  public static readonly PF0601_DEPARTMENT="PF0601_DEPARTMENT";
  public static readonly PF0601_STAFF="PF0601_STAFF";
  public static readonly PF0601_CUSTOMER="PF0601_CUSTOMER";

  // 得意先マスタ
  public static readonly PB0501_CUSTOMER="PB0501_CUSTOMER";
  public static readonly PB0501_STAFF="PB0501_STAFF";

  // 入金部門対応マスタ
  public static readonly PB1201_DEPARTMENT = "PB1201_DEPARTMENT";
  public static readonly PB1301_SECTION = "PB1301_SECTION";

}