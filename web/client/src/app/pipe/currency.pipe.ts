import { PipeTransform, Pipe } from "@angular/core";
import { StringUtil } from "../common/util/string-util";

@Pipe({
  name: 'currency_pipe'
})
export class RacCurrencyPipe implements PipeTransform {

  transform(fromData: any,returnZero:boolean=true): string {

    let tmpfromData = ""+fromData;

    if (StringUtil.IsNullOrEmpty(tmpfromData)) {
      if(returnZero){
        return "0";
      }
      else{
        return "";
      }
    }
    else if (tmpfromData=="0") {
      if(returnZero){
        return "0";
      }
      else{
        return "";
      }
    }

    if (typeof fromData === 'number') {
      if (navigator.language == "ja-JP" || navigator.language == "ja") {

        let tmpNumber = fromData;
        tmpNumber = Math.round(tmpNumber);
        return String("" + tmpNumber).replace(/(\d)(?=(\d{3})+(?!\d))/g, '$1,');
      }
    }
    else if (typeof fromData === 'string') {


      try {
        let tmpNumber = parseFloat(fromData.replace(/,/g, '').replace(/-/g, ''));
        if(fromData.indexOf('-')>=0){tmpNumber=tmpNumber*-1;}
        tmpNumber = Math.round(tmpNumber);
        return String("" + tmpNumber).replace(/(\d)(?=(\d{3})+(?!\d))/g, '$1,');
      }
      catch{
      }
    }
  }


  reverceTransform(fromData: any,returnZero:boolean=true): string {

    let tmpfromData = ""+fromData;

    let toData:string = "";

    if (StringUtil.IsNullOrEmpty(tmpfromData)) {
      if(returnZero){
        return "0";
      }
      else{
        return "";
      }
    }
    else if (tmpfromData=="0") {
      return "";
    }
    
    if (typeof fromData === 'string') {
      toData = fromData.replace(/[^-0-9]/g, '');
    }
    else{
      toData = String(fromData).replace(/[^-0-9]/g, '');
    }
    return toData;
  }



}
