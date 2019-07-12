import { ColumnNameSettingsResult } from "src/app/model/column-name-settings-result.model";
import { ColumnNameSetting } from "src/app/model/column-name-setting.model";
import { StringUtil } from "./string-util";

export  class ColumnNameSettigUtil{

  public static getColumnAlias(columnNameSettings:Array<ColumnNameSetting>,columnName:string):string{

    if(columnNameSettings==null)return "";

    for(let index=0;index<columnNameSettings.length;index++){
      if(columnNameSettings[index].columnName==columnName){
        if(StringUtil.IsNullOrEmpty(columnNameSettings[index].alias)){
          return columnNameSettings[index].originalName;
        }
        else{
          return columnNameSettings[index].alias;
        }
      }
    }
  }

}
