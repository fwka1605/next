import { Injectable }   from '@angular/core';
import { DatePipe }     from '@angular/common';
import { DecimalPipe }  from '@angular/common';

import { BANK_TRANSFER_RESULT_DICTIONARY } from 'src/app/common/const/kbn.const';
import { BILL_INPUT_TYPE_DICTIONARY } from 'src/app/common/const/kbn.const';
import { MATCHING_ASSIGNMENT_FLAG_DICTIONARY } from 'src/app/common/const/kbn.const';
import { Billing } from '../billing.model';
import { GridSetting } from '../grid-setting.model';

const ymd = 'yyyy/MM/dd';

@Injectable({
   providedIn: 'root'
})
export class BillingHelper {

  constructor(
    private datePipe: DatePipe,
    private decimalPipe: DecimalPipe,
  ) {
  }

  private currencyFormat = '1.0-0';

  setCurrencyFormat(precision: number) {
    this.currencyFormat = `1.${precision}-${precision}`;
  }

  getValue(billing: Billing, setting: GridSetting): any {
    switch (setting.columnName) {
      case 'Id':                      return billing.id;
      case 'AssignmentState':
        {
          let strRtn = "";

          if(billing.assignmentFlag==0){
            strRtn = '<span class="tag--noAssignment">';
          }
          else if(billing.assignmentFlag==1){
            strRtn = '<span class="tag--partAssignment">';
          }
          else if(billing.assignmentFlag==2){
            strRtn = '<span class="tag--fullAssignment">';
          }
          return strRtn + MATCHING_ASSIGNMENT_FLAG_DICTIONARY[billing.assignmentFlag].val + "</span>";
        }
      case 'CustomerCode':            return billing.customerCode==undefined?"":billing.customerCode;
      case 'BilledAt':                return this.datePipe.transform(billing.billedAt   , ymd);
      case 'SalesAt':                 return this.datePipe.transform(billing.salesAt    , ymd);
      case 'ClosingAt':               return this.datePipe.transform(billing.closingAt  , ymd);
      case 'DueAt':                   return this.datePipe.transform(billing.dueAt      , ymd);
      case 'CurrencyCode':            return billing.currencyCode;
      case 'BillingAmount':           return this.decimalPipe.transform(billing.billingAmount   , this.currencyFormat);
      case 'RemainAmount':            return this.decimalPipe.transform(billing.remainAmount    , this.currencyFormat);
      case 'InvoiceCode':             return billing.invoiceCode;
      case 'BillingCategory':         return `${billing.billingCategoryCode}:${billing.billingCategoryName}`;
      case 'CollectCategory':         return `${billing.collectCategoryCode}:${billing.collectCategoryName}`;
      case 'InputType':               return BILL_INPUT_TYPE_DICTIONARY[billing.inputType].val;
      case 'Note1':                   return billing.note1;
      case 'Memo':                    return billing.memo;
      case 'DepartmentCode':          return billing.departmentCode;
      case 'DepartmentName':          return billing.departmentName;
      case 'StaffCode':               return billing.staffCode;
      case 'StaffName':               return billing.staffName;
      case 'ContractNumber':          return billing.contractNumber;
      case 'Confirm':                 return billing.confirm ? BANK_TRANSFER_RESULT_DICTIONARY[billing.confirm].val : undefined;
      case 'RequestDate':             return this.datePipe.transform(billing.requestDate, ymd);
      case 'ResultCode':              return billing.resultCode==undefined?"":billing.resultCode;
      case 'Note2':                   return billing.note2;
      case 'Note3':                   return billing.note3;
      case 'Note4':                   return billing.note4;
      case 'DiscountAmount1':         return this.decimalPipe.transform(billing.discountAmount1 , this.currencyFormat);
      case 'DiscountAmount2':         return this.decimalPipe.transform(billing.discountAmount1 , this.currencyFormat);
      case 'DiscountAmount3':         return this.decimalPipe.transform(billing.discountAmount1 , this.currencyFormat);
      case 'DiscountAmount4':         return this.decimalPipe.transform(billing.discountAmount1 , this.currencyFormat);
      case 'DiscountAmount5':         return this.decimalPipe.transform(billing.discountAmount1 , this.currencyFormat);
      case 'FirstRecordedAt':         return this.datePipe.transform(billing.firstRecordedAt   , ymd);
      case 'LastRecordedAt':          return this.datePipe.transform(billing.lastRecordedAt   , ymd);
      case 'Price':                   return this.decimalPipe.transform(billing.billingAmount - billing.taxAmount, this.currencyFormat);
      case 'TaxAmount':               return this.decimalPipe.transform(billing.taxAmount       , this.currencyFormat);
      case 'Note5':                   return billing.note5;
      case 'Note6':                   return billing.note6;
      case 'Note7':                   return billing.note7;
      case 'Note8':                   return billing.note8;

      case 'CustomerName':            return billing.customerName==undefined?"":billing.customerName;;
      case 'DiscountAmountSummary':   return this.decimalPipe.transform(billing.discountAmount  , this.currencyFormat);
    }
    return undefined;
}
}
