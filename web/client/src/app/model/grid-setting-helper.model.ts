import { GridId } from '../common/const/kbn.const';
import { GridSetting } from './grid-setting.model';

const right = 'col-align-right';
const center = 'col-align-center';

export class GridSettingHelper {
  /**
   * グリッド表示設定の テキスト揃え text-align を返す
   */
  getAlign(setting: GridSetting): string {
    switch (setting.gridId as GridId) {
      case GridId.BillingSearch:              return this.getBillingSearchAlign(setting.columnName);
      case GridId.ReceiptSearch:              return this.getReceiptSearchAlign(setting.columnName);
      case GridId.BillingMatchingIndividual:  return this.getBillingMatchingIndividualAlign(setting.columnName);
      case GridId.ReceiptMatchingIndividual:  return this.getReceiptMatchingIndividualAlign(setting.columnName);
      case GridId.PaymentScheduleInput:       return this.getPaymentScheduleInputAlign(setting.columnName);
      case GridId.BillingInvoicePublish:      return this.getBillingPublishAlign(setting.columnName);
    }
    return '';
  }

  private getBillingSearchAlign(columnName: string): string {
    switch (columnName) {
      case 'AssignmentState':
      case 'CurrencyCode':
      case 'CustomerCode':
      case 'BilledAt':
      case 'SalesAt':
      case 'ClosingAt':
      case 'DueAt':
      case 'InputType':
      case 'DepartmentCode':
      case 'StaffCode':
      case 'RequestDate':
      case 'ResultCode':
      case 'FirstRecordedAt':
      case 'LastRecordedAt':
      case 'Confirm':
      {
        return center;
      }
      case 'Id':
      case 'BillingAmount':
      case 'RemainAmount':
      case 'DiscountAmount1':
      case 'DiscountAmount2':
      case 'DiscountAmount3':
      case 'DiscountAmount4':
      case 'DiscountAmount5':
      case 'DiscountAmountSummary':
      case 'Price':
      case 'TaxAmount':
      {
        return right;
      }
    }
    return '';
  }

  private getReceiptSearchAlign(columnName: string): string {
    switch (columnName) {
      case 'ExcludeFlag':
      case 'AssignmentState':
      case 'RecordedAt':
      case 'CustomerCode':
      case 'OutputAt':
      case 'InputType':
      case 'DueAt':
      case 'BankCode':
      case 'BranchCode':
      case 'AccountNumber':
      case 'VirtualBranchCode':
      case 'VirtualAccountNumber':
      case 'BillBankCode':
      case 'BillBranchCode':
      case 'BillDrawAt':
      case 'CurrencyCode':
      case 'SectionCode':
      {
        return center;
      }
      case 'Id':
      case 'ReceiptAmount':
      case 'RemainAmount':
      case 'ExcludeAmount':
      {
        return right;
      }
    }
    return '';
  }

  private getBillingMatchingIndividualAlign(columnName: string): string {
    switch (columnName) {
      case 'AssignmentFlag':
      case 'CustomerCode':
      case 'BilledAt':
      case 'SalesAt':
      case 'DueAt':
      case 'InputType':
      {
        return center;
      }
      case 'BillingAmount':
      case 'RemainAmount':
      case 'DiscountAmountSummary':
      case 'TargetAmount':
      case 'MatchingAmount':
      {
        return right;
      }
    }
    return '';
  }

  private getReceiptMatchingIndividualAlign(columnName: string): string {
    switch (columnName) {
      case 'AssignmentFlag':
      case 'RecordedAt':
      case 'NettingState':
      case 'BankCode':
      case 'BranchCode':
      case 'AccountTypeName':
      case 'AccountNumber':
      case 'DueAt':
      case 'VirtualBranchCode':
      case 'VirtualAccountNumber':
      case 'CustomerCode':
      case 'BillBankCode':
      case 'BillBranchCode':
      case 'BillDrawAt':
      case 'SectionCode':
      {
        return center;
      }

      case 'ReceiptAmount':
      case 'RemainAmount':
      case 'TargetAmount':
      {
        return right;
      }
    }
    return '';
  }

  private getPaymentScheduleInputAlign(columnName: string): string {
    switch (columnName) {
      case 'UpdateFlag':
      case 'CustomerCode':
      case 'DepartmentCode':
      case 'CurrencyCode':
      case 'BilledAt':
      case 'BillingDueAt':
      case 'SalesAt':
      case 'ClosingAt':
      case 'StaffCode':
      case 'InputType':
      {
        return center;
      }
      case 'BillingId':
      case 'BillingAmount':
      case 'DiscountAmountSummary':
      case 'RemainAmount':
      case 'PaymentAmount':
      case 'OffsetAmount':
      {
        return right;
      }
    }
    return '';
  }

  private getBillingPublishAlign(columnName: string): string {
    switch (columnName) {
      case 'Checked':
      case 'DetailsCount':
      case 'CustomerCode':
      case 'ClosingAt':
      case 'BilledAt':
      case 'DepartmentCode':
      case 'StaffCode':
      case 'PublishAt':
      case 'PublishAt1st':
      {
        return center;
      }
      case 'InvoiceTemplateId':
      case 'AmountSum':
      case 'RemainAmountSum':
      {
        return right;
      }
    }
    return '';
  }
}
