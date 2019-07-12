

import { ColumnNameSettingsResult } from "src/app/model/column-name-settings-result.model";
import { ColumnNameSetting } from "src/app/model/column-name-setting.model";
import { StringUtil } from "./string-util";
import { ReportSetting } from "src/app/model/report-setting.model";
import { GeneralSetting } from "src/app/model/general-setting.model";
import { element } from "@angular/core/src/render3";
import { CategoryType } from "../const/kbn.const";
import { MatchingOrder } from "src/app/model/matching-order.model";

export  class MatchingUtil{


  public static getMatchingOrder(matchingOrders:Array<MatchingOrder>,itemName:string):MatchingOrder
  {
    let rtnMatchingOrder:MatchingOrder = null;


    matchingOrders.forEach(element => {
      if(element.itemName==itemName){
        rtnMatchingOrder = element;
      }
    });


    return rtnMatchingOrder;

  }

  public static getMatchingOrderName(itemName:string,categoryType:CategoryType):string
  {
    let itemNameJp:string = "";

    if (categoryType == 1)
    {
        switch (itemName)
        {
            case "BillingRemainSign" : return "請求残の正負";
            case "CashOnDueDatesFlag" : return "期日入金予定フラグ";
            case "DueAt" : return  "入金予定日";
            case "CustomerCode" : return "得意先コード";
            case "BilledAt" : return "請求日";
            case "BillingRemainAmount" : return "請求残（入金予定額）の絶対値";
            case "BillingCategory" : return "請求区分";
            default: return "";
        }
    }
    else if (categoryType == 2)
    {
        switch (itemName)
        {
            case "NettingFlag": return "相殺データ";
            case "ReceiptRemainSign": return "入金残の正負";
            case "RecordedAt": return "入金日";
            case "PayerName": return "振込依頼人名";
            case "SourceBankName": return "仕向銀行";
            case "SourceBranchName": return "仕向支店";
            case "ReceiptRemainAmount": return "入金残の絶対値";
            case "ReceiptCategory": return "入金区分";
            default: return "";
        }
    }

    return itemNameJp;

  }



  public static getMatchingSortOrderName(sotrOrder:number):string
  {
    switch (sotrOrder)
    {
      case 0 : return "昇順";
      case 1 : return "降順";
      default: return "";
    }

  }  
}
