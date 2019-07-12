export class EVENT_TYPE {
  public static readonly NONE = "none";       // イベントなし
  public static readonly CLICK = "click";       // クリック
  public static readonly DBCLICK = "dblclick";  // ダブルクリック
  public static readonly MOUSEDOWN = "mousedown";  // マウスボタンを押したとき
  public static readonly MOUSEUP = "mouseup";  // マウスボタンを離したとき
  public static readonly MOUSEENTER = "mouseenter";  // マウスポインターが要素に入ったとき
  public static readonly MOUSEMOVE = "mousemove";  // マウスポインターが要素内を移動したとき
  public static readonly MOUSELEAVE = "mouseleave";  // マウスポインターが要素を離れたとき
  public static readonly FOCUS = "focus";  // 要素にフォーカスしたとき
  public static readonly BLUR = "blur";  // 要素からフォーカスが外れたとき
  public static readonly KEYDOWN = "keydown";  // キーを押したとき
  public static readonly KEYPRESS = "keypress";  // キーを押し続けたとき
  public static readonly KEYPU = "keyup";  // キーを破綻したとき
  public static readonly INPUT = "input";  // 入力内容が変更さえたとき
  public static readonly SELECT = "select";  // テキストが選択されたとき
  public static readonly RESET = "reset";  // リセットのとき
  public static readonly SUBMIT = "submit";  // サブミットのとき
}

export class KEY_CODE {
  public static readonly F9 = "120";
}

export enum BUTTON_ACTION {

  SEARCH = 'search',  // 検索
  REGISTRY = "registry",  // 登録
  DELETE = "delete",  // 削除
  UPDATE = "update",  // 更新
  PRINT = "print",  // 印刷
  IMPORT = "import",  // インポート
  EXPORT = "export",  // エクスポート
  REEXPORT = "reexport",  // 再出力
  READ = "read",  // 読込
  REDISPLAY = 'redisplay',  // 再表示
  CAPTURE = 'capture',  // 取込
  OPEN_MODAL="openModal", // モーダルオープン
  NEXT ="next",  // 次へ
  PREV="prev",  // 前へ
  EXTRACT="extract",  // 抽出
  CANCEL="cancel",  // キャンセル
  HISTORY_DELETE="historyDelete",
  RESET="reset",  // 初期化
  MATCHING="matching",  // 消込
  COLLATE="collate",  // 照合  
  REGISTRY_BILLING = "registryBilling" ,  // 
  SEARCH_BILLING = "searchBilling",
  REGISTRY_RECEIPT = "registryReceipt",
  SEARCH_RECEIPT = "searchReceipt",
  SETTING = "setting",  // 取込設定
  OUTPUT    = 'output',   // 出力
  REOUTPUT  = 'reoutput', // 再出力
  REFERE_NEW = "refereNew", //参照新規
  SELECT_ALL = "selectAll", // 全選択
  CANCEL_ALL = "cancelAll", // 全解除
  NEETING_INPUT = "neetingInput", // 相殺入力
}

export const OPERATION_NAME = new Map()
OPERATION_NAME.set(BUTTON_ACTION.REGISTRY, "登録");
OPERATION_NAME.set(BUTTON_ACTION.DELETE, "削除");
OPERATION_NAME.set(BUTTON_ACTION.PRINT, "印刷");
OPERATION_NAME.set(BUTTON_ACTION.IMPORT, "インポート");
OPERATION_NAME.set(BUTTON_ACTION.EXPORT, "エクスポート");
OPERATION_NAME.set(BUTTON_ACTION.READ, "読込")
OPERATION_NAME.set(BUTTON_ACTION.REDISPLAY, "再表示");
OPERATION_NAME.set(BUTTON_ACTION.CAPTURE, "取込");
OPERATION_NAME.set(BUTTON_ACTION.SEARCH, "検索");
OPERATION_NAME.set(BUTTON_ACTION.EXTRACT, "抽出");
OPERATION_NAME.set(BUTTON_ACTION.CANCEL, '取消');
OPERATION_NAME.set(BUTTON_ACTION.OUTPUT, '出力');
OPERATION_NAME.set(BUTTON_ACTION.REOUTPUT, '再出力');