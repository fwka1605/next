import { ExportMatchingIndividual } from "../export-matching-individual.model";
import { Billing } from "../billing.model";
import { Receipt } from "../receipt.model";
import { BILL_INPUT_TYPE_DICTIONARY } from "src/app/common/const/kbn.const";

export class ExportMatchingIndividualSub extends ExportMatchingIndividual {
  DateUtil: any;

  
  constructor(){
    super();
  }


  public setData(bill:Billing ,  receipt:Receipt)
  {
      if (bill != null)
      {
        this.id = bill.id;
        this.companyId = bill.companyId;
        this.currencyId = bill.currencyId;
        this.customerId = bill.customerId;
        this.departmentId = bill.departmentId;
        this.staffId = bill.staffId;
        this.billingCategoryId = bill.billingCategoryId;
        this.inputType = bill.inputType;
        this.billingInputTypeId = bill.billingInputTypeId;
        this.billedAt = bill.billedAt;
        this.closingAt = bill.closingAt;
        this.salesAt = bill.salesAt;
        this.dueAt = bill.dueAt;
        this.billingAmount = bill.billingAmount;
        this.taxAmount = bill.taxAmount;
        this.assignmentAmount = bill.assignmentAmount;
        this.remainAmount = bill.remainAmount;
        this.targetAmount = bill.targetAmount;
        this.customerCode = bill.customerCode;
        this.offsetAmount = bill.offsetAmount;
        this.assignmentFlag = bill.assignmentFlag;
        this.approved = bill.approved;
        this.collectCategoryId = bill.collectCategoryId;
        this.originalCollectCategoryId = bill.originalCollectCategoryId;
        this.debitAccountTitleId = bill.debitAccountTitleId;
        this.creditAccountTitleId = bill.creditAccountTitleId;
        this.originalDueAt = bill.originalDueAt;
        this.outputAt = bill.outputAt;
        this.publishAt = bill.publishAt;
        this.invoiceCode = bill.invoiceCode;
        this.taxClassId = bill.taxClassId;
        this.note1 = bill.note1;
        this.note2 = bill.note2;
        this.note3 = bill.note3;
        this.note4 = bill.note4;
        this.note5 = bill.note5;
        this.note6 = bill.note6;
        this.note7 = bill.note7;
        this.note8 = bill.note8;
        this.deleteAt = bill.deleteAt;
        this.requestDate = bill.requestDate;
        this.resultCode = bill.resultCode;
        this.transferOriginalDueAt = bill.transferOriginalDueAt;
        this.scheduledPaymentKey = bill.scheduledPaymentKey;
        this.quantity = bill.quantity;
        this.unitPrice = bill.unitPrice;
        this.unitSymbol = bill.unitSymbol;
        this.price = bill.price;
        this.createBy = bill.createBy;
        this.createAt = bill.createAt;
        this.updateBy = bill.updateBy;
        this.updateAt = bill.updateAt;
        this.currencyCode = bill.currencyCode;
        this.customerName = bill.customerName;
        this.customerKana = bill.customerKana;
        this.departmentCode = bill.departmentCode;
        this.departmentName = bill.departmentName;
        this.staffName = bill.staffName;
        this.staffCode = bill.staffCode;
        this.memo = bill.memo;
        this.billingCategoryCode = bill.billingCategoryCode;
        this.billingCategoryName = bill.billingCategoryName;
        this.collectCategoryCode = bill.collectCategoryCode;
        this.collectCategoryName = bill.collectCategoryName;
        this.loginUserCode = bill.loginUserCode;
        this.loginUserName = bill.loginUserName;
        this.contractNumber = bill.contractNumber;
        this.confirm = bill.confirm;
        this.discountAmount = bill.discountAmount;
        this.discountAmount1 = bill.discountAmount1;
        this.discountAmount2 = bill.discountAmount2;
        this.discountAmount3 = bill.discountAmount3;
        this.discountAmount4 = bill.discountAmount4;
        this.discountAmount5 = bill.discountAmount5;
        this.billingId = bill.billingId;
        this.parentCustomerCode = bill.parentCustomerCode;
        this.companyCode = bill.companyCode;
        this.accountTitleCode = bill.accountTitleCode;
        this.paymentAmount = bill.paymentAmount;
        this.categoryCodeAndName = bill.categoryCodeAndName;
        this.orgCategoryCodeAndName = bill.orgCategoryCodeAndName;
        ///////////////////////////////////////////////
        this.billingCategoryCodeAndName = bill.categoryCodeAndName;
        this.collectCategoryCodeAndName = bill.collectCategoryName;

        BILL_INPUT_TYPE_DICTIONARY.forEach(element=>{
            if(element.id==bill.inputType){
              this.inputTypeName= element.val;
            }
          });

        this.assignmentFlagName = ""+bill.assignmentFlag;
        this.inputTypeNameAndIndex = ""+bill.inputType;
      }

      //forReceipt
      if (receipt != null)
      {
          this.amount = receipt.amount;
          this.receiptId = receipt.id;
          this.receiptCompanyId = receipt.companyId;
          this.receiptCurrencyId = receipt.currencyId;
          this.privateBankCode = receipt.privateBankCode;
          this.receiptHeaderId = receipt.receiptHeaderId;
          this.receiptCategoryId = receipt.receiptCategoryId;
          this.receiptCustomerId = receipt.customerId;
          this.sectionId = receipt.sectionId;
          this.receiptInputType = receipt.inputType;
          this.apportioned = receipt.apportioned;
          this.receiptApproved = receipt.approved;
          this.workday = receipt.workday;
          this.recordedAt = receipt.recordedAt;
          this.originalRecordedAt = receipt.originalRecordedAt;
          this.receiptAmount = receipt.receiptAmount;
          this.receiptAssignmentAmount = receipt.assignmentAmount;
          this.receiptRemainAmount = receipt.remainAmount;
          this.receiptAssignmentFlag = receipt.assignmentFlag;
          this.payerCode = receipt.payerCode;
          this.payerName = receipt.payerName;
          this.payerNameRaw = receipt.payerNameRaw;
          this.sourceBankName = receipt.sourceBankName;
          this.sourceBranchName = receipt.sourceBranchName;
          this.receiptOutputAt = receipt.outputAt;
          this.receiptDueAt = receipt.dueAt;
          this.mailedAt = receipt.mailedAt;
          this.originalReceiptId = receipt.originalReceiptId;
          this.excludeFlag = receipt.excludeFlag;
          this.excludeCategoryId = receipt.excludeCategoryId;
          this.excludeCategoryName = receipt.excludeCategoryName;
          this.excludeAmount = receipt.excludeAmount;
          this.referenceNumber = receipt.referenceNumber;
          this.recordNumber = receipt.recordNumber;
          this.densaiRegisterAt = receipt.densaiRegisterAt;
          this.receiptNote1 = receipt.note1;
          this.receiptNote2 = receipt.note2;
          this.receiptNote3 = receipt.note3;
          this.receiptNote4 = receipt.note4;
          this.billNumber = receipt.billNumber;
          this.billBankCode = receipt.billBankCode;
          this.billBranchCode = receipt.billBranchCode;
          this.billDrawAt = receipt.billDrawAt;
          this.billDrawer = receipt.billDrawer;
          this.receiptDeleteAt = receipt.deleteAt;
          this.receiptCreateBy = receipt.createBy;
          this.receiptCreateAt = receipt.createAt;
          this.receiptUpdateBy = receipt.updateBy;
          this.receiptUpdateAt = receipt.updateAt;
          this.categoryName = receipt.categoryName;
          this.receiptCustomerCode = receipt.customerCode;
          this.receiptCustomerName = receipt.customerName;
          this.receiptCurrencyCode = receipt.currencyCode;
          this.sectionCode = receipt.sectionCode;
          this.sectionName = receipt.sectionName;
          this.bankCode = receipt.bankCode;
          this.bankName = receipt.bankName;
          this.branchCode = receipt.branchCode;
          this.branchName = receipt.branchName;
          //////////////////////////////////////////////
          this.payerCodePrefix = receipt.payerCode.substr(0,3);
          this.payerCodeSuffix = receipt.payerCode.substr(4);
          this.useAdvanceReceived = receipt.useAdvanceReceived;
          this.useForeignCurrency = receipt.useForeignCurrency;
          this.accountNumber = receipt.accountNumber;
          this.receiptMemo = receipt.receiptMemo;
          this.sourceBank = receipt.sourceBankName;
          this.useFeeTolerance = receipt.useFeeTolerance;
          this.customerFee = receipt.customerFee;
          this.generalFee = receipt.generalFee;
          this.originalUpdateAt = receipt.originalUpdateAt;
          this.bankTransferFee = receipt.bankTransferFee;
          this.nettingId = receipt.nettingId;
          this.recExcOutputAt = receipt.recExcOutputAt;
          this.useCashOnDueDates = receipt.useCashOnDueDates;
          //////////////////////////////////////////////
          this.nettingState = ""+receipt.nettingId;
          this.accountTypeName = receipt.accountTypeName;
          //////////////////////////////////////////////
          this.receiptAssignmentFlagName = ""+receipt.assignmentFlag;
          this.receiptCategroyCodeAndName =receipt.categoryCode + ":" + receipt.categoryName;
      }
  }  

}

