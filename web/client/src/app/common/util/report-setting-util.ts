import { ColumnNameSettingsResult } from "src/app/model/column-name-settings-result.model";
import { ColumnNameSetting } from "src/app/model/column-name-setting.model";
import { StringUtil } from "./string-util";
import { ReportSetting } from "src/app/model/report-setting.model";

export  class ReportSettigUtil{

  public static getItemKey(reportSettings:Array<ReportSetting>,itemId:string,displayOrder:number):string{

    if(reportSettings==null)return "";

    for(let index=0;index<reportSettings.length;index++){
      if(reportSettings[index].itemId==itemId&&reportSettings[index].displayOrder==displayOrder){
          return reportSettings[index].itemKey;
      }
    }

    return "";
  }


}
