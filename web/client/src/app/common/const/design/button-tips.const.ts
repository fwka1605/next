export class ButtonTipsConst
{

  // 登録
  // 削除	
  // 取消
  public readonly JOURNALIZING_CANCEL = "チェックした出力履歴を<br>削除します。削除した出力<br>履歴は、抽出・出力<br>が行えます。";
  //更新
  // 一括消込
  public readonly BULK_MATCHING = "チェックされた入金・請求<br>情報の消込、または消込<br>の解除を行います。";
  // 消込
  public readonly MATCHING = "チェックされた入金・請求<br>情報の消込を行います。";
  //実行
  // 一括削除
  public readonly BULK_DELETE = "全ての入金明細の振分<br>情報の削除を行います。";
  // 一括振分
  public readonly BULK_APPORTION = "指定された対象外区分で、<br>全ての入金明細の<br>振分を行います。";
  // 分割対象外
  public readonly SPLIT_EXCLUDING = "選択した入金データの<br>対象外区分を分割して<br>登録が行えます。";
  // 一括対象外
  public readonly BULK_SPLIT_EXCLUDING = "チェックした入金データの<br>全額を、選択した対象外<br>区分で登録します。";
  // 取込	
  public readonly EB_IMPORT = "EBファイルの取込が<br>行えます。";
  // 読込	
  public readonly READ = "ファイルを読込、チェック<br>を行います。 読込後、件数<br>等に問題がなければ登録<br>してください。";
  // 検索	
  // 抽出
  public readonly EXTRACTION="出力対象の仕訳データを<br>抽出され、件数等の<br>確認が行えます。";
  // 照会
  public readonly INQUIRY="出力対象の仕訳データを<br>抽出され、件数等の<br>確認が行えます。";
  // 照合・表示
  public readonly COLLATION="未消込情報の照会、または<br>消込完了情報の表示が<br>行えます。";
  // 前へ
  public readonly PREV_CUSTOMER="１つ前の得意先の情報を<br>表示します。";
  // 次へ
  public readonly NEXT_CUSTOMER="次の得意先の情報を<br>表示します。";
  // クリア	
  // 初期化
  public readonly INITIALIZE="登録内容を破棄し、<br>システムに登録されて<br>いる内容を再表示します。<br>";
  // 再表示
  public readonly REDISPLAY="登録内容を破棄し、<br>システムに登録されて<br>いる内容を再表示します。<br>";
  // エクスポート	
  public readonly CSV_EXPORT="CSVファイルのエクス<br>ポートが行えます。";
  // エクスポート	
  public readonly EXCEL_EXPORT="Excelファイルのエクス<br>ポートが行えます。";
  // インポート	
  public readonly CSV_IMPORT="CSVファイルのイン<br>ポートが行えます。";
  // 出力 
  public readonly JOURNALIZING_DOWNLOAD="仕訳CSVファイルのダウン<br>ロードが行えます。";
  // 出力設定
  public readonly JOURNALIZING_SETTING="仕訳CSVファイルの内容の<br>設定が行えます。";
  // 再出力 
  public readonly HISTORY_JOURNALIZING_DOWNLOAD="チェックした出力履歴の<br>仕訳CSVファイルのダウン<br>ロードが行えます。";
  // 得意先出力	
  public readonly CUSTOMER_DOWNLOAD="インポートで新たに<br>作成された得意先の<br>ダウンロードが行えます。";
  // 全選択	全解除	追加	
  // 行コピー
  public readonly INVOICE_DETAIL_COPY="先頭行の請求データの<br>コピーが行えます。";
  //一括削除
  public readonly BULK_DELETE_CUSTOMER="選択された得意先<br>変更後の得意先）を<br>全て削除します。";
  // 出力設定
  public readonly OUTPUT_CONTENT_SETTING="出力する内容の設定が<br>行えます。";
  // 取込設定
  public readonly IMPORT_LAYOUT_SETTING="インポートするファイルの<br>レイアウトの設定が<br>行えます。";
  // 新規登録	
  // 手数料登録	
  public readonly FEE_REGISTRY="手数料負担区分が自社の<br>場合に手数料の登録を<br>行えます。";
  // 参照新規	
  public readonly CREATE_FROM_HISTORY="既存の請求データを検索<br>して、請求データの登録が<br>行えます。";
  // 連携設定	
  public readonly MF_COOPERATION_SETTING="MFクラウド請求書 Web<br>API 連携設定を行えます。";
  //対象外設定	
  public readonly EXCLUDE_ACCOUNT_SETTING="消込の対象外とする<br>銀行・口座情報の<br>登録が行えます。";
  // 請求検索（入金入力）
  public readonly BILLING_SEARCH="請求検索画面を<br>開きます。";
  // 入金検索（入金入力）
  public readonly RECEIPT_SEARCH="入金検索画面を<br>開きます。";
  // 入金入力	(消込)
  public readonly RECEIPT_INPUT_FOR_CLEARING="消込するための<br>入金情報の新規登録<br>が行えます。";
  // 請求検索	(消込)
  public readonly BILLING_SEARCH_FOR_CLEARING="消込したい請求情報<br>を検索して、追加する<br>ことができます。";
  // 入金検索(消込)
  public readonly RECEIPT_SEARCH_FOR_CLEARING="消込したい入金情報<br>を検索して、追加する<br>ことができます。"
  // MF入金データ自動連携設定
  public readonly MF_STATEMENT_COOPERATION_SETTING="入金データ自動連携 Web<br>API 連携設定を行えます。";
  // MF入金データ自動新規ID登録
  public readonly MF_STATEMENT_GET_NEWID="入金データ自動連携の新規ID<br>登録を行えます。";
  // MF入金データ自動口座詳細設定
  public readonly MF_STATEMENT_SUBACCOUNT_SETTING="入金データ自動連携の<br>口座詳細設定を行えます。";
  // MF入金データ自動連携 口座一覧画面へ遷移
  public readonly MF_STATEMENT_ACCOUNT_LIST_DISPLAY="入金データ自動連携の<br>口座一覧を表示します。";

}