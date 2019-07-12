import { Injectable, SecurityContext }   from '@angular/core';
import { DatePipe }     from '@angular/common';
import { DecimalPipe }  from '@angular/common';


import { RECEIPT_INPUT_TYPE_DICTIONARY } from 'src/app/common/const/kbn.const';
import { MATCHING_ASSIGNMENT_FLAG_DICTIONARY } from 'src/app/common/const/kbn.const';
import { GridSetting }  from '../grid-setting.model';
import { Receipt } from '../receipt.model';
import { HtmlTipsConst } from '../../common/const/design/html-tips.const';
import { DomSanitizer } from '@angular/platform-browser';

import { MatchedReceipt } from 'src/app/model/matched-receipt.model';
import { ExportFieldSetting } from 'src/app/model/export-field-setting.model';
import { TIME_ZONE } from 'src/app/common/const/company.const';

const ymd = 'yyyy/MM/dd';
const ymdhms = 'yyyy/MM/dd HH:mm:ss';
const ymd_short_year = 'yy/MM/dd';
const ymd_no_slash = 'yyyyMMdd';
const ymd_short_year_no_slash = 'yyMMdd';

@Injectable({
   providedIn: 'root'
})
export class ReceiptHelper {
  constructor(
    private datePipe: DatePipe,
    private decimalPipe: DecimalPipe,
    private sanitizer: DomSanitizer
  ) {
  }
  private currencyFormat = '1.0-0';

  public readonly htmlTipsConst:HtmlTipsConst = new HtmlTipsConst();

  setCurrencyFormat(precision: number) {
    this.currencyFormat = `1.${precision}-${precision}`;
  }

  getValue(receipt: Receipt, setting: GridSetting): any {
    switch (setting.columnName) {
      case 'AssignmentState':
        {
          let strRtn = "";

          if(receipt.assignmentFlag==0){
            strRtn = '<span class="tag--noAssignment">';
          }
          else if(receipt.assignmentFlag==1){
            strRtn = '<span class="tag--partAssignment">';
          }
          else if(receipt.assignmentFlag==2){
            strRtn = '<span class="tag--fullAssignment">';
          }
          return strRtn + MATCHING_ASSIGNMENT_FLAG_DICTIONARY[receipt.assignmentFlag].val + "</span>";
        }      
      case 'InputType':               return RECEIPT_INPUT_TYPE_DICTIONARY[receipt.inputType].val;
      case 'RecordedAt':              return this.datePipe.transform(receipt.recordedAt , ymd);
      case 'OutputAt':                return this.datePipe.transform(receipt.outputAt, ymdhms, TIME_ZONE);
      case 'DueAt':                   return this.datePipe.transform(receipt.dueAt      , ymd);
      case 'BillDrawAt':              return this.datePipe.transform(receipt.billDrawAt , ymd);
      case 'ReceiptAmount':           return this.decimalPipe.transform(receipt.receiptAmount, this.currencyFormat);
      case 'RemainAmount':            return this.decimalPipe.transform(receipt.remainAmount , this.currencyFormat);
      case 'ExcludeAmount':           return this.decimalPipe.transform(receipt.excludeAmount, this.currencyFormat);
      case 'Id':                      return receipt.id;
      case 'CustomerCode':            return receipt.customerCode==undefined?"":receipt.customerCode;
      case 'CustomerName':            return receipt.customerName==undefined?"":receipt.customerName;
      case 'CurrencyCode':            return receipt.currencyCode;
      case 'SectionCode':             return receipt.sectionCode;
      case 'SectionName':             return receipt.sectionName;
      case 'PayerName':               return receipt.payerName;
      case 'ReceiptCategoryName':     return `${receipt.categoryCode}:${receipt.categoryName}`;
      case 'BankCode':                return receipt.bankCode;
      case 'BankName':                return receipt.bankName;
      case 'BranchCode':              return receipt.branchCode;
      case 'BranchName':              return receipt.branchName;
      case 'AccountNumber':           return receipt.accountNumber;
      case 'SourceBankName':          return receipt.sourceBankName;
      case 'SourceBranchName':        return receipt.sourceBranchName;
      case 'VirtualBranchCode':       return receipt.payerCode.substring(0, 3);
      case 'VirtualAccountNumber':    return receipt.payerCode.substring(3);
      case 'Memo':                    return receipt.receiptMemo==undefined?"":receipt.receiptMemo;
      case 'Note1':                   return receipt.note1;
      case 'Note2':                   return receipt.note2;
      case 'Note3':                   return receipt.note3;
      case 'Note4':                   return receipt.note4;
      case 'BillNumber':              return receipt.billNumber;
      case 'BillBankCode':            return receipt.billBankCode;
      case 'BillBranchCode':          return receipt.billBranchCode;
      case 'BillDrawer':              return receipt.billDrawer;
    }
    return undefined;
  }

  getExportValue(matchedReceipt: MatchedReceipt, setting: ExportFieldSetting): any {

    let dateFormat: string
    if (setting.columnName == 'BilledAt' || 
        setting.columnName == 'RecordedAt' ||
        setting.columnName == 'DueAt' ||
        setting.columnName == 'BillDrawAt')
    {
      switch (setting.dataFormat) {
        case 1:
          dateFormat = ymd_short_year;
          break;
        case 2:
          dateFormat = ymd_no_slash;
          break;
        case 3:
          dateFormat = ymd_short_year_no_slash;
          break;
        default:
          dateFormat = ymd;
        }
    }

    switch (setting.columnName) {
      case 'CompanyCode':                 return matchedReceipt.companyCode;
      case 'SlipNumber':                  return matchedReceipt.slipNumber;
      case 'CustomerCode':                return matchedReceipt.customerCode;
      case 'CustomerName':                return matchedReceipt.customerName;
      case 'InvoiceCode':                 return matchedReceipt.invoiceCode;
      case 'BilledAt':                    return this.datePipe.transform(matchedReceipt.billedAt, dateFormat);
      case 'ReceiptCategoryCode':         return matchedReceipt.receiptCategoryCode;
      case 'ReceiptCategoryName':         return matchedReceipt.receiptCategoryName;
      case 'RecordedAt':                  return this.datePipe.transform(matchedReceipt.recordedAt, dateFormat);
      case 'DueAt':                       return this.datePipe.transform(matchedReceipt.dueAt, dateFormat);
      case 'Amount':                      return this.decimalPipe.transform(matchedReceipt.amount, this.currencyFormat);
      case 'DepartmentCode':              return matchedReceipt.departmentCode;
      case 'DepartmentName':              return matchedReceipt.departmentName;
      case 'CurrencyCode':                return matchedReceipt.currencyCode;
      case 'ReceiptAmount':               return this.decimalPipe.transform(matchedReceipt.receiptAmount, this.currencyFormat);
      case 'ReceiptId':                   return matchedReceipt.id;
      case 'BillingNote1':                return matchedReceipt.billingNote1;
      case 'BillingNote2':                return matchedReceipt.billingNote2;
      case 'BillingNote3':                return matchedReceipt.billingNote3;
      case 'BillingNote4':                return matchedReceipt.billingNote4;
      case 'ReceiptNote1':                return matchedReceipt.receiptNote1;
      case 'ReceiptNote2':                return matchedReceipt.receiptNote2;
      case 'ReceiptNote3':                return matchedReceipt.receiptNote3;
      case 'ReceiptNote4':                return matchedReceipt.receiptNote4;
      case 'BillNumber':                  return matchedReceipt.billNumber;
      case 'BillBankCode':                return matchedReceipt.billBankCode;
      case 'BillBranchCode':              return matchedReceipt.billBranchCode;
      case 'BillDrawAt':                  return this.datePipe.transform(matchedReceipt.billDrawAt, dateFormat);
      case 'BillDrawer':                  return matchedReceipt.billDrawer;
      case 'BillingMemo':                 return matchedReceipt.billingMemo;
      case 'ReceiptMemo':                 return matchedReceipt.receiptMemo;
      case 'MatchingMemo':                return matchedReceipt.matchingMemo;
      case 'BankCode':                    return matchedReceipt.bankCode;
      case 'BankName':                    return matchedReceipt.bankName;
      case 'BranchCode':                  return matchedReceipt.branchCode;
      case 'BranchName':                  return matchedReceipt.branchName;
      case 'AccountNumber':               return matchedReceipt.accountNumber;
      case 'SourceBankName':              return matchedReceipt.sourceBankName;
      case 'SourceBranchName':            return matchedReceipt.sourceBranchName;
      case 'VirtualBranchCode':           return matchedReceipt.virtualBranchCode;
      case 'VirtualAccountNumber':        return matchedReceipt.virtualAccountNumber;
      case 'SectionCode':                 return matchedReceipt.sectionCode;
      case 'SectionName':                 return matchedReceipt.sectionName;
      case 'ReceiptCategoryExternalCode': return matchedReceipt.receiptCategoryExternalCode;
      case 'OriginalReceiptId':           return matchedReceipt.originalReceiptId;
      case 'JournalizingCategory':        return matchedReceipt.journalizingCategory;

    }
    return undefined;
    
    }

}
