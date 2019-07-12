import { ElementRef } from "@angular/core";
import { EVENT_TYPE } from "../const/event.const";
import { StringUtil } from "./string-util";

export const BOM_UINT8_ARRAY = new Uint8Array([0xEF, 0xBB, 0xBF]);

export const HTTP_CONTENT_TYPE = 'plain/text;';
export const HTTP_CONTENT_TYPE_PDF= 'application/pdf;';
export const HTTP_CONTENT_TYPE_XLSX= 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet;';


export class HtmlUtil {
  /* 次の項目にフォーカスを移動する */
  public static nextFocusByName(elementRef: ElementRef, elementName: string, eventType: string) {
    var matches;
    var nextElement;

    matches = elementRef.nativeElement.ownerDocument.getElementsByName(elementName);
    if (matches.length == 1) {
      nextElement = matches[0];
    }
    else if (matches.length > 1) {
      nextElement = matches[matches.length-1];
    }
    else {
      return;
    }

    if (eventType != EVENT_TYPE.BLUR) {
      nextElement.focus();
    }

  }


  /* 次の項目にフォーカスを移動する */
  public static nextFocusByNames(elementRef: ElementRef, elementNames: string[], eventType: string) {
    var matches;
    var nextElement;

    for(let index=0; index<elementNames.length;index++){
      matches = elementRef.nativeElement.ownerDocument.getElementsByName(elementNames[index]);
      if (matches.length == 1 && !matches[0].disabled) {
        nextElement = matches[0];
        break;
      }
    };

    if (!StringUtil.IsNullOrEmpty(nextElement) && eventType != EVENT_TYPE.BLUR) {
      nextElement.focus();
    }


  }  

}
