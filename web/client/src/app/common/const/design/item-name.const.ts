
/*
  BTN:ボタン
  LBL:ラベル
    LBL_RESULT：結果ラベル
*/

export class ItemNameConst
{

  /*
    ボタンの名称
  */
  public readonly BTN_LOGIN = "ログイン";
  public readonly BTN_REGISTRY  = "登録";
  public readonly BTN_REGISTRY_NEW  = "新規登録";
  public readonly BTN_REGISTRY_FEE  = "手数料登録";
  public readonly BTN_REGISTRY_REFERENCE  = "参照新規";
  public readonly BTN_DELETE  = "削除";
  public readonly BTN_CANCEL  = "キャンセル";
  public readonly BTN_UPDATE  = "更新";
  public readonly BTN_EDIT    = "編集";
  public readonly BTN_RESTORE = "復元";
  public readonly BTN_SAVE = "保存";
  public readonly BTN_OVERWRITING = "上書";

  public readonly BTN_MATCHING  = "消込";
  public readonly BTN_MATCHING_SEQUENTIAL  = "一括消込";
  public readonly BTN_DO  = "実行";
  public readonly BTN_BULK_DELETE  = "一括削除";
  public readonly BTN_BULK_APPORTION  = "一括振分";
  public readonly BTN_DIVISION_NOT_TARGET  = "分割対象外";
  public readonly BTN_BULK_NOT_TARGET  = "一括対象外";
  public readonly BTN_IMPORT  = "インポート";
  public readonly BTN_READ  = "読込";
  public readonly BTN_SEARCH  = "検索";
  public readonly BTN_EXTRACT  = "抽出";
  public readonly BTN_INQUIRY  = "照会";
  public readonly BTN_INQUIRY_DISPLAY  = "照合・表示";
  public readonly BTN_PREV = "前へ";
  public readonly BTN_NEXT = "次へ";
  public readonly BTN_CLEAR  = "クリア";
  public readonly BTN_INITIALIZE  = "初期化";
  public readonly BTN_REDISPLAY  = "再表示";
  public readonly BTN_ELESE  = "その他";

  public readonly BTN_DOWNLOAD  = "エクスポート";
  public readonly BTN_DOWNLOAD_ARROW  = "エクスポート";

  public readonly BTN_EXPORT_EXCEL = "Excel";
  public readonly BTN_EXPORT_CSV = "CSV";


  public readonly BTN_OUTPUT  = "出力";
  public readonly BTN_REOUTPUT  = "再出力";
  public readonly BTN_OUTPUT_CUSTOMER  = "得意先出力";
  public readonly BTN_SELECT_ALL  = "全選択";
  public readonly BTN_SELECT  = "選択";

  public readonly BTN_CANCEL_ALL  = "全解除";
  public readonly BTN_ADD  = "追加";
  public readonly BTN_COPY  = "行コピー";
  public readonly BTN_SETTING_OUTPUT  = "出力設定";
  public readonly BTN_SETTING_REGISTRY  = "取込設定";
  public readonly BTN_SETTING_CONNECTION  = "連携設定";
  public readonly BTN_SETTING_NOT_TARGET  = "対象外設定";
  public readonly BTN_SEARCH_BILLING  = "請求検索";
  public readonly BTN_SEARCH_RECEIPT  = "入金検索";
  public readonly BTN_REGISTRY_RECEIPT  = "入金入力";
  public readonly BTN_BACK  = "戻る";

  public readonly BTN_SELECT_FILE = "ファイルを選択";

  public readonly BTN_ADD_SEARCH = "照合条件を追加";
  public readonly BTN_ADD_HIDDEN = "照合条件を隠す";

  public readonly BTN_SEARCH_ADD = "検索条件を追加";
  public readonly BTN_SEARCH_HIDE = "検索条件を隠す";

  public readonly BTN_TO_SIMPLE_SEARCH = "簡易検索へ";
  public readonly BTN_TO_DETAIL_SEARCH = "詳細検索へ";

  public readonly BTN_ACCOUNT_DETAILS = "口座詳細";

  public readonly BTN_OFFSET_INPUT = "相殺入力";

  public readonly BTN_MEMO = "メモ";

  public readonly BTN_SYNC = "同期";

  public readonly BTN_REGISTRATION_HISTORY = "取込履歴";

  public readonly BTN_YES = "はい";
  public readonly BTN_NO = "いいえ";

  public readonly BTN_INPUT_ITEM_HIDDEN = "登録項目を隠す";
  public readonly BTN_INPUT_ITEM_DISPLAY = "登録項目を表示"

  public readonly BTN_SEARCH_ITEM_HIDDEN = "登録項目を隠す";
  public readonly BTN_SEARCH_ITEM_DISPLAY = "登録項目を表示"

  public readonly BTN_OFFSET_DOWNLOAD = "後でちゃんと名前を決める。";

  public readonly BTN_DELETE_HISTORY = "履歴削除";

  public readonly BTN_CLOSE = "閉じる";
  /*
    ラベル
  */

  // 必須
  public readonly LBL_REQUIRED = "※";

  // 説明文
  public readonly LBL_CAN_SET_SEARCH_CONDITION = "検索条件を入力できます。";
  public readonly LBL_CAN_SET_EXPORT_CONDITION = "抽出条件を入力できます。";
  public readonly LBL_CAN_SET_SETTING_COMPANY = "会社設定を入力できます。";
  public readonly LBL_CAN_SET_SETTING_FUNCTION = "機能設定を入力できます。";
  public readonly LBL_CAN_SET_SETTING_AUTHORITY = "権限設定を入力できます。";
  public readonly LBL_CAN_SET_SETTING_PASSWORD = "パスワードを入力できます。";
  public readonly LBL_CAN_SET_SETTING_INQUIRY = "照会条件を入力できます。";

  public readonly LBL_SET_DOWNLOAD_DESTINATION = "ダウンロード先を指定してください。";
  public readonly LBL_DO_SELECT_IMPORT_METHOD = "インポート方法を選択してください。";

  public readonly LBL_DOING_ACTION = "処理中です。";

  // 入力等の指定
  public readonly LBL_INVOICE_ENTRY = "請求データ入力";
  public readonly LBL_RECEIPT_ENTRY = "入金データ入力";
  public readonly LBL_DELETE_NOT_NEED_DATA = "不要データ削除";
  public readonly LBL_SETTING_BASE = "基本設定";
  public readonly LBL_SETTING_ONE = "設定１";
  public readonly LBL_SETTING_TWO = "設定２";
  public readonly LBL_SETTING_ITEM = "項目の設定";
  public readonly LBL_SETTING_PREVIEW = "設定プレビュー";
  public readonly LBL_SETTING_COMPANY = "会社設定";
  public readonly LBL_SETTING_FUNCTION = "機能設定";
  public readonly LBL_SETTING_AUTHORITY = "権限設定";
  public readonly LBL_SETTING_REPORT = "出力設定";
  public readonly LBL_SETTING_PASSWORD = "パスワード";
  public readonly LBL_SELECT_IMPORT_METHOD = "インポート方法の選択";
  public readonly LBL_SELECT_IMPORT_CONTENTS = "インポート内容の選択";


  // 検索・取込等の指定
  public readonly LBL_SEARCH_CONDITION = "検索条件";
  public readonly LBL_SETTING_IMPORT = "取込条件";
  public readonly LBL_SETTING_EXPORT = "抽出条件";
  public readonly LBL_SETTING_COLLATION = "照合条件";
  public readonly LBL_SETTING_INQUIRY = "照会条件";
  public readonly LBL_SETTING_EXCLUDE = "対象外設定";
  public readonly LBL_EB_FILE_SETTING = "EBファイル設定";

  // 実行結果
  public readonly LBL_RESULT_SEARCH = "検索結果";
  public readonly LBL_RESULT_IMPORT_HISTORY = "取込履歴";
  public readonly LBL_RESULT_EXPORT_HISTORY = "抽出履歴";
  public readonly LBL_RESULT_EXPORT = "抽出結果";
  public readonly LBL_RESULT_COLLATE_RESULT = "照合結果";
  public readonly LBL_RESULT_INQUIRY_RESULT = "照会結果";
  public readonly LBL_RESULT_TEST_CONNECTION_RESULT = "テスト接続結果";
  public readonly LBL_RESULT_UNTRANSFER = "振替不能データ一覧";
  
  // 登録済み
  public readonly LBL_REGISTER_EB_FILE_SETTING = "登録済みのEBファイル設定";
  public readonly LBL_REGISTER_ITEM = "登録済みの項目";
  public readonly LBL_REGISTER_STAFF = "登録済みの営業担当者";
  public readonly LBL_REGISTER_LOGIN_USER = "登録済みのログインユーザー名";
  public readonly LBL_REGISTER_BANK_ACCOUNT = "登録済みの銀行口座";
  public readonly LBL_REGISTER_ACCOUNT_TITLE = "登録済みの科目";
  public readonly LBL_REGISTER_CATEGORY = "登録済みの区分";
  public readonly LBL_REGISTER_GENERAL_SETTING = "登録済みの管理情報";
  public readonly LBL_REGISTER_SECTION = "登録済みの入金部門";
  public readonly LBL_REGISTER_IGNORE_KANA = "登録済みの除外カナ";
  public readonly LBL_REGISTER_HOLIDAY = "登録済み休業日";
  public readonly LBL_REGISTER_JURIDICAL_PERSONALITY = "登録済みの法人格";

  public readonly IMPORT_DETAIL_SETTING = "取込列指定";
  
  public readonly LBL_OPERATION_LOG_MANAGEMENT = "操作ログ管理";
  public readonly LBL_MANAGEMENT_CODE = "管理コード";

  public readonly LBL_HISTORY_CLEARING = "消込履歴";

  // 変更前後
  public readonly LBL_CHANGE_CUSTOMER_BEFORE = "変更前の得意先";
  public readonly LBL_CHANGE_CUSTOMER_AFTER = "変更後の得意先";
  public readonly LBL_CHANGE_DEPARTMENT_BEFORE = "変更前の請求部門";
  public readonly LBL_CHANGE_DEPARTMENT_AFTER = "変更後の請求部門";
  public readonly LBL_CHANGE_SECTION_BEFORE = "変更前の入金部門";
  public readonly LBL_CHANGE_SECTION_AFTER = "変更後の入金部門";

}