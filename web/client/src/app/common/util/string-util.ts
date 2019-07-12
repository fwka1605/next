export class StringUtil {

  /**
   * 文字列のNULL・空のチェック
   * @param str - チェックする文字列
   * @return boolean - true:Nullまたは空、false:Null、空でない。
   */
  static IsNullOrEmpty(str: string): boolean {
    return !str;
  }
  // テンプレート側ではstaticを直接呼べなく、コンポーネントでnewして使用するため。
  public IsNullOrEmpty(str: string): boolean {
    return StringUtil.IsNullOrEmpty(str);
  }

  static setPaddingFrontZero(input:string,size:number, isUse:boolean = false):string{
    if (!isUse) return input;

    let strRtn:string;

    if(input == undefined || input==null) return "";

    if(size-input.length>0){
      strRtn=input;
      for(let index=0;size-input.length>index;index++){
        strRtn = "0" + strRtn;
      }
    }
    else{
      strRtn=input;
    }

    return strRtn;
  }

  static ConvertEmpty(value:string):string{
    return value==undefined || value==null?value="":value=value;
  }

  /**
   * 大文字に変換
   * @param input 変換元文字列
   */
  static setUpperCase(input: string): string {
    return input.toUpperCase();
  }
}