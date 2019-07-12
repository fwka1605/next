import { formatDate } from "@angular/common";
import { StringUtil } from "./string-util";

export class NumberUtil {

  /**
   * 文字列のNULL・空のチェック
   * @param str - チェックする文字列
   * @return boolean - true:Nullまたは空、false:Null、空でない。
   */
  static ParseInt(fromData: string): number {

    let toNumber: number;

    if (StringUtil.IsNullOrEmpty(fromData)) {
      toNumber = 0;
    }
    else {
      try {
        fromData = String(fromData).replace(/[^-0-9]/g, '');
        toNumber = parseInt(fromData);
      }
      catch (Exception) {
        toNumber = 0;
      }
    }
    return toNumber;
  }

  static Round(fromData:number):number{
     return isNaN(fromData)?fromData:Math.round(fromData);
  }

  /**
   * 数値をカンマ区切りにする
   * @param num 変換する数値
   */
  static Separate(num: number) {
    return String(num).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, '$1,');
  }

}