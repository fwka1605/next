import { FormControl } from '@angular/forms';
import { StringUtil } from "./string-util";
import { Moment } from "moment";
import { NgbDate } from '@ng-bootstrap/ng-bootstrap';
import { format } from 'url';
import { Data } from '@angular/router';
import { DAY_ADD_HOUR } from '../const/company.const';

export class DateUtil {


  /**
   * 文字列のNULL・空のチェック
   * @param str - チェックする文字列
   * @return boolean - true:Nullまたは空、false:Null、空でない。
   */
  static ConvertFormat(fromDate: string): string {

    let toDate: string;
    let format: string = "-"

    if (fromDate.length == 10) {
      toDate = fromDate.substr(0, 4) + format + fromDate.substr(5, 2) + format + fromDate.substr(8, 2);
    }
    else {
      toDate = fromDate;
    }

    console.log(fromDate + ":" + toDate);
    return toDate;


  }

  /**
 * 文字列のNULL・空のチェック
 * @param str - チェックする文字列
 * @return boolean - true:Nullまたは空、false:Null、空でない。
 */
  static ConvertFromDatepicker(formControl: FormControl): string {

    if (!StringUtil.IsNullOrEmpty(formControl.value) && !StringUtil.IsNullOrEmpty(formControl.value.year) ) {
      return formControl.value.year + "/" + formControl.value.month + "/" + formControl.value.day;
    }
    else {
      return "";
    }
  }


  /**
   * Datepicker から 日時の開始 を取得
   * @param {FormControl} formControl 日付を取得する Datepicker
   * @return {string}                 日付が入力されていない場合 空文字
   *                                  日付が入力されている場合、yyyy/M/dT00:00:00.000Z
   */
  static ConvertFromDatepickerToStart(formControl: FormControl): string {

    if (!StringUtil.IsNullOrEmpty(formControl.value)&& !StringUtil.IsNullOrEmpty(formControl.value.year)) {
      return `${formControl.value.year}/${formControl.value.month}/${formControl.value.day}T00:00:00.000Z`;
    }
    else {
      return "";
    }
  }

  /**
   * Datepicker から 日時の終了 を取得
   * @param {FormControl} formControl 日付を取得する Datepicker
   * @return {string}                 日付が入力されていない場合 空文字
   *                                  日付が入力されている場合、yyyy/M/dT29:59:59.999Z
   */
  static ConvertFromDatepickerToEnd(formControl: FormControl): string {

    if (!StringUtil.IsNullOrEmpty(formControl.value)&& !StringUtil.IsNullOrEmpty(formControl.value.year)) {
      return `${formControl.value.year}/${formControl.value.month}/${formControl.value.day}T23:59:59.999Z`;
    }
    else {
      return "";
    }
  }


  /**
 * 文字列のNULL・空のチェック
 * @param str - チェックする文字列
 * @return boolean - true:Nullまたは空、false:Null、空でない。
 */
  static ConvertFromMomentToIso(formControl: FormControl): string {

    let tmp: string;
    if (!StringUtil.IsNullOrEmpty(formControl.value)&& !StringUtil.IsNullOrEmpty(formControl.value.year)) {

      let tmpMomemnt = formControl.value.clone();
      tmp = tmpMomemnt.add("D", 1).toISOString();
      return tmp;
    }
    else {
      return "";
    }
  }

  static ConvertFromIso(date: string, convertType: CALENDAR_CONVERT_TYPE) {

    let rtnDate: string;
    switch (convertType) {
      case CALENDAR_CONVERT_TYPE.DAY_END: {
        break;
      }
      case CALENDAR_CONVERT_TYPE.HOUR_END: {

        rtnDate = date.substr(0, 10) + "T23:59:59.999Z";
        break;
      }
      case CALENDAR_CONVERT_TYPE.DAY_HOUR_END: {
        break;
      }
      case CALENDAR_CONVERT_TYPE.DAY_START: {
        break;
      }
      case CALENDAR_CONVERT_TYPE.HOUR_START: {
        rtnDate = date.substr(0, 10) + "T00:00:00.000Z";
        break;
      }
      case CALENDAR_CONVERT_TYPE.DAY_HOUR_START: {
        break;
      }
      default: {
        rtnDate = date;
        break;
      }
    }
    return rtnDate;
  }


  /**
   * DBから取得した日付を「/」に置き換える
   * @param value DBから取得した日付
   */
  static convertDateString(value: string): string {
    let convertValue;
    if (value == null || value.length == 0) return "";
    convertValue = value.substr(0, 10);
    convertValue = convertValue.replace(/-/g, '/');
    return convertValue;
  }

  /**
   * 当日かつ指定日の日付を取得する
   * @param formatType フォーマット種別
   * [0: YYYYMMDD, 1:YYYY/MM/DD, 2:年月, 3:年月時間, 4:YYYY/MM/DD HH:MM, 5:YYYY/MM/DD HH:MM:SS]
   */
  static getYYYYMMDD(formatType: number, argDate: string = ""): string {
    let result: string = '';
    let day = StringUtil.IsNullOrEmpty(argDate) ? new Date() : new Date(argDate);
    day.setHours(day.getHours() + DAY_ADD_HOUR);
    
    let month: string = "" + (day.getMonth() + 1);
    let date: string = "" + (day.getDate());
    let hour: string = "" + (day.getHours());
    let min: string = "" + (day.getMinutes());
    let sec: string = "" + (day.getSeconds());

    month = StringUtil.setPaddingFrontZero(month, 2, true);
    date = StringUtil.setPaddingFrontZero(date, 2, true);
    hour = StringUtil.setPaddingFrontZero(hour, 2, true);
    min = StringUtil.setPaddingFrontZero(min, 2, true);
    sec = StringUtil.setPaddingFrontZero(sec, 2, true);

    switch (formatType) {
      case 1:
        result = "" + day.getFullYear() + "/" + month + "/" + date;
        break;

      case 2:
      case 3:
        result = day.getFullYear() + "年" + month + "月" + date + "日";
        if (formatType == 3) {
          result += " " + day.getHours() + "時" + day.getMinutes() + "分" + day.getSeconds() + "秒";
        }
        break;

      case 4:
      case 5:
        result = "" + day.getFullYear() + "/" + month + "/" + date + " " + hour + ":" + min;
        if (formatType == 5) {
          result = result + ":" + sec;
        }
        break;

      default:
        result = "" + day.getFullYear() + month + date;
        break;
    }

    return result;
  }

  static isValidRange(date1: FormControl, date2: FormControl): boolean {

    if (date1.value != undefined && date2.value != undefined) {
      let fromDate = new NgbDate(date1.value.year, date1.value.month, date1.value.day);
      let toDate = new NgbDate(date2.value.year, date2.value.month, date2.value.day);

      if (fromDate.after(toDate)) return false;

    }
    return true;
  }

  static convertYYYYMMDD(formControl: FormControl, isSlash: boolean = false): string {
    let day = new Date(this.ConvertFromDatepicker(formControl));

    let month: string = "" + (day.getMonth() + 1);
    let date: string = "" + (day.getDate());

    month = StringUtil.setPaddingFrontZero(month, 2, true);
    date = StringUtil.setPaddingFrontZero(date, 2, true);

    if (isSlash) {
      return day.getFullYear() + "/" + month + "/" + date
    } else {
      return "" + day.getFullYear() + month + date;
    }
  }

  /**
   * 文字列をデータ型に変換する
   * @param formControl フォームの値
   */
  static convertStringFromData(formControl: FormControl): Data {
    let dataValue: Data;
    if (StringUtil.IsNullOrEmpty(formControl.value)) {
      return;
    }

    dataValue = new Date(formControl.value.year, formControl.value.month - 1, formControl.value.day);
    return dataValue;
  }

  static convertStringFromNgbDate(data: string): NgbDate {
    let dataValue;
    if (data.length == 8 && data.indexOf("/") < 0) {
      dataValue = data.substr(0, 4) + "/" + data.substr(4, 2) + "/" + data.substr(6, 2);  
    } else {
      dataValue = data;
    }
    
    let day = new Date(dataValue);
    let dt = new NgbDate(day.getFullYear(), day.getMonth() + 1, day.getDay() - 1);
    return dt;
  }
}

export enum CALENDAR_CONVERT_TYPE {
  DAY_END,
  HOUR_END,
  DAY_HOUR_END,
  DAY_START,
  HOUR_START,
  DAY_HOUR_START,
}
