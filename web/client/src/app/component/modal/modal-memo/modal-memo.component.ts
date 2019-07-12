import { Component, OnInit, EventEmitter, AfterViewInit, ElementRef } from '@angular/core';
import { BaseComponent } from '../../common/base/base-component';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Receipt } from 'src/app/model/receipt.model';
import { ReceiptMemo } from 'src/app/model/receipt-memo.model';
import { MODAL_STATUS_TYPE } from 'src/app/common/const/status.const';
import { ProcessResultService } from 'src/app/service/common/process-result.service';
import { EVENT_TYPE } from 'src/app/common/const/event.const';
import { HtmlUtil } from 'src/app/common/util/html.util';
import { BillingMemo } from 'src/app/model/billing-memo.model';
import { Billing } from 'src/app/model/billing.model';
import { CategoryType } from 'src/app/common/const/kbn.const';

@Component({
  selector: 'app-modal-memo',
  templateUrl: './modal-memo.component.html',
  styleUrls: ['./modal-memo.component.css']
})
export class ModalMemoComponent extends BaseComponent implements OnInit, AfterViewInit {

  public readonly CategoryType: typeof CategoryType = CategoryType;

  public MemoCtrl: FormControl;

  public UndefineCtrl: FormControl;

  public receipt: Receipt;
  public receiptMemo: ReceiptMemo;

  public billing: Billing;
  public billingMemo: BillingMemo;

  public categoryType:CategoryType;

  public lineNo: number;

  public closing = new EventEmitter<{}>();

  public isTransterMemo: boolean = false;

  constructor(
    public elementRef: ElementRef,
    public processResultService:ProcessResultService
  ) {
    super();
  }

  ngOnInit() {
    // 入力チェックの設定
    //this.setBasicValidator();
    this.setControlInit();
    this.setValidator();
    this.initDisplay();
  }

  ngAfterViewInit() {
    HtmlUtil.nextFocusByName(this.elementRef, 'MemoCtrl', EVENT_TYPE.NONE);
  }

  public get Closing() {
    return this.closing;
  }
  public set Closing(value) {
    this.closing = value;
  }

  public get LineNo(): number {
    return this.lineNo;
  }
  public set LineNo(value: number) {
    this.lineNo = value;
  }

  public get ProcessModalCustomResult() {
    return this.processModalCustomResult;
  }
  public set ProcessModalCustomResult(value) {
    this.processModalCustomResult = value;
  }

  public setControlInit() {
    this.MemoCtrl = new FormControl("", [Validators.maxLength(100)]);
    this.UndefineCtrl = new FormControl();
  }

  public setValidator() {
    this.MyFormGroup = new FormGroup({
      MemoCtrl: this.MemoCtrl,
      UndefineCtrl: this.UndefineCtrl,
    });
  }

  public close() {
    this.ModalStatus = MODAL_STATUS_TYPE.CLOSE;
    this.processModalCustomResult = this.processResultService.clearProcessCustomMsg(this.processModalCustomResult);
    this.closing.emit({});
  }

  public cancel() {
    this.ModalStatus = MODAL_STATUS_TYPE.CANCEL;
    this.processModalCustomResult = this.processResultService.clearProcessCustomMsg(this.processModalCustomResult);
    this.closing.emit({});
  }


  public registry() {

    if(this.categoryType==CategoryType.Billing){
      this.billingMemo.memo = this.MemoCtrl.value;

    }
    else if(this.categoryType==CategoryType.Receipt){
      this.receiptMemo.memo = this.MemoCtrl.value;
    }

    this.ModalStatus = MODAL_STATUS_TYPE.OK;
    this.processModalCustomResult = this.processResultService.clearProcessCustomMsg(this.processModalCustomResult);
    this.closing.emit({});
  }

  public initDisplay() {

    if(this.categoryType==CategoryType.Billing){
      this.MemoCtrl.setValue(this.billingMemo.memo);
    }
    else if(this.categoryType==CategoryType.Receipt){
      this.MemoCtrl.setValue(this.receiptMemo.memo);
    }    
  }

  public clear() {
    this.MyFormGroup.reset();
  }

  public getTitleLabelName(isHeader: boolean):string {
    let labelName: string;
    let titleName: string;
    
    labelName = this.categoryType == CategoryType.Billing ? "請求メモ"
                : this.isTransterMemo ? "振替メモ" : "入金メモ";

    titleName = this.categoryType == CategoryType.Billing ? "請求メモ入力"
                : this.isTransterMemo ? "振替メモ入力" : "入金メモ入力";

    return isHeader ? titleName : labelName;
  }

}
