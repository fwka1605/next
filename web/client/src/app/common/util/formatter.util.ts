import { FormControl } from "@angular/forms";

export class FormatStyle{
  public static readonly ZIP:string='^[0-9]{3}-[0-9]{4}$';
  public static readonly HANKAKU_KANA:string='^[\uff61-\uff9f!-/:-@¥[-`{-~]+$';
  public static readonly HANKAKU_KANA_NUMBER_ALPHA:string='^[\uff61-\uff9f0-9A-Za-z!-/:-@¥[-`{-~]+$';
  public static readonly HANKAKU_NUMBER_ALPHA:string='^[0-9A-Za-z]+$';
  public static readonly TEL:string='^\\d{2,3}-\\d{1,4}-\\d{4}$';
  public static readonly CURRENCY:string='^[-0-9,]$';
  public static readonly NUMBER:string='^[0-9]$';

  public static readonly NUMBER_PERIOD:string='^[0-9.]$';
}


export class FormatterUtil {

  public static setCodeFormatter(formControl: FormControl) {
    formControl.valueChanges.subscribe(
      (value: string) => {
        if (typeof value === "string") {
          if (value.match(/[^0-9A-Za-z]/)) {
            formControl.setValue(value.replace(/[^0-9A-Za-z]/g, ''), { emitEvent: false});
          }
        }
      });
  }

  public static setAlphabetFormatter(formControl: FormControl) {
    formControl.valueChanges.subscribe(
      (value: string) => {
        if (typeof value === "string") {
          if (value.match(/[^A-Za-z]/)) {
            formControl.setValue(value.replace(/[^A-Za-z]/g, ''), { emitEvent: false});
          }
        }
      });
  }


  public static setCurrencyFormatter(formControl: FormControl, allowNegative: boolean = true) {
    if (allowNegative) {
      formControl.valueChanges.subscribe(
        (value: string) => {
          if (typeof value === "string") {
            if (value.match(/[^-0-9,]/)) {
              formControl.setValue(value.replace(/[^-0-9,]/g, ''), { emitEvent: false });
            }
          }
        });
    }
    else {
      formControl.valueChanges.subscribe(
        (value: string) => {
          if (typeof value === "string") {
            if (value.match(/[^0-9,]/)) {
              formControl.setValue(value.replace(/[^0-9,]/g, ''), { emitEvent: false});
            }
          }
        });
    }
  }

  public static setCurrencyPeriodFormatter(formControl: FormControl) {
    formControl.valueChanges.subscribe(
      (value: string) => {
        if (typeof value === "string") {
          if (value.match(/[^-0-9,.]/)) {
            formControl.setValue(value.replace(/[^0-9,.]/g, ''), { emitEvent: false});
          }
        }
      });
  }  

  public static setNumberFormatter(formControl: FormControl) {
    formControl.valueChanges.subscribe(
      (value: string) => {
        if (typeof value === "string") {
          if (value.match(/[^0-9]/)) {
            formControl.setValue(value.replace(/[^0-9]/g, ''), { emitEvent: false});
          }
        }
      });
  }  

  public static setNumberPeriodFormatter(formControl: FormControl) {
    formControl.valueChanges.subscribe(
      (value: string) => {
        if (typeof value === "string") {
          if (value.match(/[^0-9.]/)) {
            formControl.setValue(value.replace(/[^0-9.]/g, ''), { emitEvent: false});
          }
        }
      });
  }  

  public static setTelFormatter(formControl: FormControl){
    formControl.valueChanges.subscribe(
      (value: string) => {
        if (typeof value === "string") {
          if (value.match(/[^0-9-]$/)) {
            formControl.setValue(value.replace(/[^0-9-]/g, ''), { emitEvent: false});
          }
        }
      });
  }

  public static setZipFormatter(formControl: FormControl){
    formControl.valueChanges.subscribe(
      (value: string) => {
        if (typeof value === "string") {
          if (value.match(/[^0-9-]$/)) {
            formControl.setValue(value.replace(/[^0-9-]/g, ''), { emitEvent: false});
          }
        }
      });
  }  

  public static setCustomerCodeAlphaFormatter(formControl: FormControl) {
    formControl.valueChanges.subscribe(
      (value: string) => {
        if (typeof value === "string") {
          if (value.match(/[^0-9A-Za-z-_]/)) {
            formControl.setValue(value.replace(/[^0-9A-Za-z-_]/g, ''), { emitEvent: false});
          }
        }
      });
    }

    public static setCustomerCodeKanaFormatter(formControl: FormControl) {
      formControl.valueChanges.subscribe(
        (value: string) => {
          if (typeof value === "string") {
            if (value.match(/[^\uff61-\uff9f0-9A-Za-z-_]/)) {
              formControl.setValue(value.replace(/[^\uff61-\uff9f0-9A-Za-z-_]/g, ''), { emitEvent: false});
            }
          }
        });
    }
}