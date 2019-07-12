import { StringUtil } from "src/app/common/util/string-util";

/// <summary>
/// サブアカウント
/// </summary>
export class SubAccount
{
  /// <summary>
  /// サブアカウントのID
  /// </summary>
  public id:number;

  /// <summary>金融機関の支店名</summary>
  public name:string;

  /// <summary>口座種別</summary>
  public type:string;

  /// <summary>口座番号</summary>
  public number:string;
  
  /// <summary>支店コード</summary>
  public branch_code:string;
  
  /// <summary>サブアカウントに設定されたタグのリスト</summary>
  public tags:String[];
   
  public IsEmpty():boolean{    
    return  StringUtil.IsNullOrEmpty(this.name) &&
            StringUtil.IsNullOrEmpty(this.type) &&
            StringUtil.IsNullOrEmpty(this.number);
  } 

}