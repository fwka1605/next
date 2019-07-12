import { Collation } from "../collation.model";
import { StringUtil } from "src/app/common/util/string-util";
import { CollationSetting } from "../collation-setting.model";
import { MatchingHeader } from "../matching-header.model";
import { FormControl } from "@angular/forms";

export class CollationData extends Collation {

  public checkable:boolean;

  public id:number;


  public checkBox:number;

  public approved:number;
  public amount:number;
  public matchingProcessType:number;
  public memo:string;
  public createBy:number;
  public createAt:string;
  public updateBy:number;
  public updateAt:string;
  public matchingUpdateAt:string;

  public setDataFromCollation(collation:CollationData){

    this.checkBox=0;
    
    this.checkable=false; // チェックできるかどうか

    this.checked = collation.checked;
    this.compnayId = collation.compnayId;
    this.currencyId = collation.currencyId;
    this.parentCustomerId = collation.parentCustomerId;
    this.paymentAgencyId = collation.paymentAgencyId;
    this.customerId = collation.customerId;
    this.paymentAgencyCode = collation.paymentAgencyCode;
    this.customerName = collation.customerName;
    this.paymentAgencyName = collation.paymentAgencyName;
    this.billingCount = collation.billingCount;
    this.billingAmount = collation.billingAmount;
    this.payerName = collation.payerName;
    this.receiptCount = collation.receiptCount;
    this.receiptAmount = collation.receiptAmount;
    this.advanceReceivedCount = collation.advanceReceivedCount;
    this.bankTransferFee = collation.bankTransferFee;
    this.taxDifference = collation.taxDifference;
    this.shareTransferFee = collation.shareTransferFee;
    this.useFeeTolerance = collation.useFeeTolerance;
    this.useFeeLearning = collation.useFeeLearning;
    this.useKanaLearning = collation.useKanaLearning;
    this.prioritizeMatchingIndividually = collation.prioritizeMatchingIndividually;
    this.forceMatchingIndividually = collation.forceMatchingIndividually;
    this.dispCustomerCode = collation.dispCustomerCode;
    this.dispCustomerName = collation.dispCustomerName;
    this.dispCustomerKana = collation.dispCustomerKana;
    this.isParent = collation.isParent;
    this.displayOrder = collation.displayOrder;
    this.updateFlag = collation.updateFlag;
    this.dispBillingAmount = collation.dispBillingAmount;
    this.dispReceiptAmount = collation.dispReceiptAmount;
    this.dispBillingCount = collation.dispBillingCount;
    this.dispReceiptCount = collation.dispReceiptCount;
    this.currencyCode = collation.currencyCode;
    this.currencyTolerance = collation.currencyTolerance;
    this.customerCode = collation.customerCode;
    this.billingOrder = collation.billingOrder;
    this.receiptOrder = collation.receiptOrder;
    this.dupeCheck = collation.dupeCheck;
    this.billingPriority = collation.billingPriority;
    this.receiptPriority = collation.receiptPriority;
    this.billingDisplayOrder = collation.billingDisplayOrder;
    this.receiptDisplayOrder = collation.receiptDisplayOrder;

    this.dispShareTransferFee = 
      this.dispBillingCount == 0 ? ""
      : (this.shareTransferFee == 0) ? "相手先"
      : (this.shareTransferFee == 1) ? "自社" : "";
      
    this.dispAdvanceReceivedCount = 
      this.dispReceiptCount==0 ? ""
      : (this.advanceReceivedCount == 2) ? "○"
      : (this.advanceReceivedCount == 1) ? "△" : "";

    /// <summary>差額  請求残 - 入金残</summary>
    this.different = this.billingAmount - this.receiptAmount; 

    this.dispDifferent = 
      (this.dispBillingCount==null || this.dispReceiptCount==null)?null: this.different;

    this.reportDispShareTransferFee = 
      this.dispBillingCount == 0 ? ""
      : (this.shareTransferFee == 0) ? "0：相手先"
      : (this.shareTransferFee == 1) ? "1：自社" : "";

      /// <summary>差額が0</summary>
    this.isEqual = this.dispDifferent== 0;

  }

/*
  public getMatchingHeader():MatchingHeader{
    
    let matchingHeader = new MatchingHeader();

    matchingHeader.id = this.id;
    matchingHeader.companyId =  this.compnayId;
    matchingHeader.currencyId =  this.currencyId;
    matchingHeader.customerId = this.customerId;
    matchingHeader.paymentAgencyId = this.paymentAgencyId;

    matchingHeader.approved = this.approved;
    matchingHeader.receiptCount = this.receiptCount;
    matchingHeader.billingCount = this.billingCount;
    matchingHeader.amount = this.amount;
    matchingHeader.bankTransferFee = this.bankTransferFee;
    matchingHeader.taxDifference = this.taxDifference;
    matchingHeader.matchingProcessType = this.matchingProcessType;
    matchingHeader.memo = this.memo;

    matchingHeader.createBy = this.createBy;
    matchingHeader.createAt = this.createAt;
    matchingHeader.updateBy = updateBy;
    matchingHeader.updateAt: string;

    matchingHeader.receiptAmount: number;
    matchingHeader.billingAmount: number;

    matchingHeader.currencyCode: string | null;

    matchingHeader.dispCustomerCode: string | null;
    matchingHeader.dispCustomerName: string | null;

    matchingHeader.shareTransferFee: number;
    matchingHeader.payerName: string | null;
    matchingHeader.customerCode: string | null;

    matchingHeader.paymentAgencyCode: string | null;
    matchingHeader.matchingUpdateAt: string;
    matchingHeader.billingDisplayOrder: number;
    matchingHeader.receiptDisplayOrder: number;



  }
  */

  public setDataFromMatchingHeader(header:MatchingHeader){

    this.checkBox=0;
    this.checkable=true;

    this.id=header.id;
    this.approved=header.approved;
    this.amount=header.amount;
    this.matchingProcessType=header.matchingProcessType;
    this.memo=header.memo;
    this.createBy=header.createBy;
    this.createAt=header.createAt;
    this.updateBy=header.updateBy;
    this.updateAt=header.updateAt;
    this.matchingUpdateAt=header.matchingUpdateAt;


    // this.checked = collation.checked;
    this.compnayId = header.companyId;
    this.currencyId = header.currencyId;
    // this.parentCustomerId = collation.parentCustomerId;
    this.paymentAgencyId = header.paymentAgencyId;
    this.customerId = header.customerId;
    this.paymentAgencyCode = header.paymentAgencyCode;
    // this.customerName = collation.customerName;
    // this.paymentAgencyName = collation.paymentAgencyName;
    this.billingCount = header.billingCount;
    this.billingAmount = header.billingAmount;
    this.payerName = header.payerName;
    this.receiptCount = header.receiptCount;
    this.receiptAmount = header.receiptAmount;
    // this.advanceReceivedCount = collation.advanceReceivedCount;
    this.bankTransferFee = header.bankTransferFee;
    this.taxDifference = header.taxDifference;
    this.shareTransferFee = header.shareTransferFee;
    // this.useFeeTolerance = collation.useFeeTolerance;
    // this.useFeeLearning = collation.useFeeLearning;
    // this.useKanaLearning = collation.useKanaLearning;
    // this.prioritizeMatchingIndividually = collation.prioritizeMatchingIndividually;
    // this.forceMatchingIndividually = collation.forceMatchingIndividually;
    this.dispCustomerCode = header.dispCustomerCode;
    this.dispCustomerName = header.dispCustomerName;
    // this.dispCustomerKana = collation.dispCustomerKana;
    // this.isParent = collation.isParent;
    // this.displayOrder = collation.displayOrder;
    // this.updateFlag = collation.updateFlag;
    this.dispBillingAmount = header.billingAmount;
    this.dispReceiptAmount = header.receiptAmount;
    this.dispBillingCount = header.billingCount;
    this.dispReceiptCount = header.receiptCount;
    this.currencyCode = header.currencyCode;
    // this.currencyTolerance = collation.currencyTolerance;
    // this.customerCode = collation.customerCode;
    // this.billingOrder = collation.billingOrder;
    // this.receiptOrder = collation.receiptOrder;
    this.dupeCheck = 0;
    // this.billingPriority = collation.billingPriority;
    // this.receiptPriority = collation.receiptPriority;
    this.billingDisplayOrder = header.billingDisplayOrder;
    this.receiptDisplayOrder = header.receiptDisplayOrder;

    this.dispShareTransferFee = 
      this.dispBillingCount==0 ? ""
      : (this.shareTransferFee == 0) ? "相手先"
      : (this.shareTransferFee == 1) ? "自社" : "";

    this.dispAdvanceReceivedCount = 
      this.dispReceiptCount==0 ? ""
      : (!StringUtil.IsNullOrEmpty(header.memo)) ? "○" : "";

    /// <summary>差額  請求残 - 入金残</summary>
    this.different = this.billingAmount - this.receiptAmount; 

    this.dispDifferent = 
      (this.dispBillingCount==null || this.dispReceiptCount==null)?null: this.different;

    this.reportDispShareTransferFee = 
      this.dispBillingCount == 0 ? ""
      : (this.shareTransferFee == 0) ? "0：相手先"
      : (this.shareTransferFee == 1) ? "1：自社" : "";

    /// <summary>差額が0</summary>
    this.isEqual = this.dispDifferent== 0;

  }  
  


  public dispShareTransferFee:string;

  public dispAdvanceReceivedCount:string;

  public different:number;

  public dispDifferent:number;

  public reportDispShareTransferFee:string;

  public isEqual:boolean;
 
  /// <summary>
  /// 一括消込可 不可 判定処理
  /// </summary>
  /// <param name="setting">照合設定</param>
  /// <param name="feeTolerance">手数料 誤差範囲</param>
  /// <param name="taxTolerance">消費税 誤差範囲</param>
  /// <param name="useForeignCurrency">外貨対応フラグ</param>
  /// <param name="checkFeeRegistered">得意先/決済代行会社 の任意の手数料登録確認のデリゲート</param>
  /// <param name="checkFeeExist">得意先/決済代行会社 の 差額が手数料として登録されているか確認するデリゲート</param>
  /// <remarks>
  /// 
  /// </remarks>
  public VerifyCheckable(
      setting:CollationSetting ,
      feeTolerance:number,
      taxTolerance:number,
      useForeignCurrency:boolean,
      checkFeeRegistered:boolean,
      checkFeeExist:boolean
  ):boolean{
      // 照合不能 or 一括消込不可設定
      if(this.dispDifferent == null || this.forceMatchingIndividually == 1)
      {
          this.checked = false;
          this.bankTransferFee = 0;
          this.taxDifference = 0;

          return false;
      }

      var checkable = false; // 照合結果一覧で一括消込可能か否か(チェックボックスのクリック可否)
      var docheck = false;   // 初期状態でチェックしておくか否か
      var bankFee = 0;
      var taxDifference = 0;

      if (this.isEqual)
      {
          checkable = true;
          if (this.shareTransferFee == 0 || (this.shareTransferFee == 1 && setting.forceShareTransferFee == 0))
          {
              docheck = true;
          }
      }
      else if (this.IsTaxToleranceEnabled(useForeignCurrency, taxTolerance))
      {
          checkable = true;
          docheck = setting.prioritizeMatchingIndividuallyTaxTolerance == 0;
          taxDifference = -this.different; // 入金側 誤差+ は 符合逆転
      }
      else if (this.IsFeeRegistered(checkFeeRegistered))
      {
          if (checkFeeExist) // 同じ金額で登録済
          {
              checkable = true;
              docheck = true;
              bankFee = this.different;
              this.useFeeLearning = 0;
          }
      }
      else if (this.IsFeeToleranceEnabled(feeTolerance))
      {
          checkable = true;
          docheck = true;
          bankFee = this.different;
      }

      // 複数入金時に一括消込対象から外す設定で、複数入金
      if (docheck && setting.prioritizeMatchingIndividuallyMultipleReceipts == 1 && 1 < this.receiptCount)
      {
          docheck = false;
      }

      //一括消込対象外（得意先マスター）
      if (docheck && this.prioritizeMatchingIndividually == 1)
      {
          docheck = false;
      }

      this.checked = docheck;
      this.bankTransferFee = bankFee;
      this.taxDifference = taxDifference;
      this.checkable = checkable;

      return checkable;
  }



  /// <summary>消費税誤差 有効
  /// 外貨 非利用 かつ JPY 消費税誤差設定が 0以外 で 誤差が 絶対値以内で有効</summary>
  /// <param name="useForeignCurrency"></param>
  /// <param name="taxTolerance"></param>
  /// <returns></returns>
  private IsTaxToleranceEnabled(useForeignCurrency:boolean, taxTolerance:number):boolean{
    return  !useForeignCurrency
    && this.currencyCode == "JPY"
    && taxTolerance != 0
    && Math.abs(this.different) <= taxTolerance;
  }

  /// <summary>手数料登録済  <see cref="ShareTransferFee"/>が 1 : 自社負担 かつ 手数料登録済</summary>
  /// <param name="registered">対象 得意先/決済代行会社でなんらかの登録がある</param>
  /// <returns></returns>
  private IsFeeRegistered(registered:boolean):boolean{
    return this.shareTransferFee == 1 && registered;
  }

  /// <summary>手数料誤差 有効
  /// 手数料誤差 利用 かつ 誤差金額が 0 より大きく 設定されている 手数料誤差金額以内</summary>
  /// <param name="feeTolerance">消費税誤差 範囲</param>
  /// <returns></returns>
  private IsFeeToleranceEnabled(feeTolerance:number):boolean{
    return this.useFeeTolerance == 1 && 0 < this.different && this.different <= feeTolerance;
  }
}

