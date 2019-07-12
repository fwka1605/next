import { ColumnNameSettingsResult } from "src/app/model/column-name-settings-result.model";
import { ColumnNameSetting } from "src/app/model/column-name-setting.model";
import { StringUtil } from "./string-util";
import { ReportSetting } from "src/app/model/report-setting.model";
import { GeneralSetting } from "src/app/model/general-setting.model";
import { element } from "@angular/core/src/render3";

export  class GeneralSettingUtil{

  public static getGeneralSettingByCode(generalSettings:Array<GeneralSetting>,code:string):GeneralSetting{
    let generalSetting:GeneralSetting = null;

    if(generalSettings==null)return null;

    generalSettings.forEach(element=>{
      if(element.code==code){
        generalSetting=element;
      }
    });

    return generalSetting;
  }

}
