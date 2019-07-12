import { FILE_EXTENSION } from "../const/eb-file.const";
import { BOM_UINT8_ARRAY, HTTP_CONTENT_TYPE, HTTP_CONTENT_TYPE_PDF, HTTP_CONTENT_TYPE_XLSX } from "./html.util";

export class FileUtil {
  // ファイル選択のダイアログの表示
  public static ShowOpenFileDialog() {
    return new Promise(resolve => {
      const input = document.createElement('input');
      input.type = 'file';
      input.accept = '.txt, text/plain';
      input.multiple = true;
      input.onchange = event => { resolve(event); };
      input.click();
    });
  };

  public static ShowOpenFolderDialog() {
    return new Promise(resolve => {
      const input = document.createElement('input');
      input.type = 'file';
      input.accept = '.txt, text/plain';
      input.onchange = event => { resolve(event); };
      input.click();
    });
  };

  /**
   * ダウンロード処理
   * @param data 出力内容
   * @param fileName 出力ファイル名
   */
  public static download(datas: any[], fileName: string, extension: string) {

    if (datas.length <= 0 || datas[0].size <= 0) return;

    const anchor = document.createElement('a');
    let blob = new Blob();
    fileName += extension;

    switch (extension) {
      case FILE_EXTENSION.CSV:
      case FILE_EXTENSION.LOG:
        blob = new Blob([BOM_UINT8_ARRAY, datas[0]], { type: HTTP_CONTENT_TYPE });
        break;

      case FILE_EXTENSION.PDF:
        blob = new Blob(datas, { type: HTTP_CONTENT_TYPE_PDF });
        break;

      case FILE_EXTENSION.XLSX:
        blob = new Blob(datas, { type: HTTP_CONTENT_TYPE_XLSX });
        break;

      default:
        console.log('Error: 設定されていないファイル拡張子が選択されました。');
        return null;
    };

    if (window.navigator.msSaveBlob) {
      // ie
      window.navigator.msSaveBlob(blob, fileName);

    } else if (window.URL && anchor.download !== undefined) {
      // chrome, firefox, etc.
      anchor.download = fileName;
      anchor.href = window.URL.createObjectURL(blob);
      document.body.appendChild(anchor);
      anchor.click();
      anchor.parentNode.removeChild(anchor);

    } else {
      window.location.href =
        'data:attachment/csv;charset=utf-8,' + encodeURIComponent(BOM_UINT8_ARRAY + datas[0]);
    }
  };

  /**
   * CSVを出力する際に、項目を「'」で囲む
   * @param dataItem 1行分のデータ
   */
  public static encloseItemBySymbol(dataItem: Array<any>): Array<any> {
    const symbol = "\"";
    let result = [];

    for (let i = 0; i < dataItem.length; i++) {
      if (dataItem[i] === undefined || dataItem[i] === null || dataItem[i].length === 0) {
        result.push(symbol + symbol);
      } else {
        result.push(symbol + dataItem[i] + symbol);
      }
    }
    return result;
  };

  /**
   * 必要がある field だけ エスケープ処理を行う
   * @param fields 要素の配列
   * @param force 強制的にすべての項目をダブルクォートで囲って返す
   * @param removeCrLf 改行コードの削除を行うかどうか
   */
  static escapeCsvFields(fields: any[], force: boolean = false, removeCrLf: boolean = false): any[] {
    const comma           = /,/g;
    const doubleQuote     = /"/g;
    const carriageReturn  = /\r/g;
    const linefeed        = /\n/g;
    return fields.map(x => {
      if (removeCrLf && (carriageReturn.test(x) || linefeed.test(x))) {
        x = `${x}`.replace(carriageReturn, '').replace(linefeed, '');
      }
      const require = force ||
        ( x != undefined &&
          ( comma.test(x) || doubleQuote.test(x) || carriageReturn.test(x) || linefeed.test(x) ) );
      if (doubleQuote.test(x)) {
        x = `${x}`.replace(doubleQuote, '""');
      }
      return require ? `"${x}"` : x;
    });
  }

  /**
   * Nullを「''」に置き換える
   * @param dataObj 登録・編集するデータオブジェクト
   */
  public static replaceNull(dataObj: Object): any {
    for (let i in dataObj) {
      if (dataObj[i] == null) {
        dataObj[i] = '';
      }
    }
    return dataObj;
  };

}
