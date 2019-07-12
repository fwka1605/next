import { Component, OnInit, EventEmitter, ElementRef, AfterViewInit } from '@angular/core';
import { BaseComponent } from '../../common/base/base-component';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Billing } from 'src/app/model/billing.model';
import { BillingMemo } from 'src/app/model/billing-memo.model';
import { ColumnNameSetting } from 'src/app/model/column-name-setting.model';
import { StringUtil } from 'src/app/common/util/string-util';
import { HtmlUtil } from 'src/app/common/util/html.util';
import { EVENT_TYPE } from 'src/app/common/const/event.const';
import { MODAL_STATUS_TYPE } from 'src/app/common/const/status.const';
import { ProcessResultService } from 'src/app/service/common/process-result.service';

@Component({
  selector: 'app-modal-invoice-memo',
  templateUrl: './modal-invoice-memo.component.html',
  styleUrls: ['./modal-invoice-memo.component.css']
})
export class ModalInvoiceMemoComponent extends BaseComponent implements OnInit,AfterViewInit {

  public MemoCtrl: FormControl;
  public Note1Ctrl: FormControl;
  public Note2Ctrl: FormControl;
  public Note3Ctrl: FormControl;
  public Note4Ctrl: FormControl;
  public Note5Ctrl: FormControl;
  public Note6Ctrl: FormControl;
  public Note7Ctrl: FormControl;
  public Note8Ctrl: FormControl;

  public UndefineCtrl: FormControl;

  public billing: Billing;
  public billingMemo: BillingMemo;
  public lineNo: number;
  public columnNameSettings = new Array<ColumnNameSetting>();

  public closing = new EventEmitter<{}>();

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

  ngAfterViewInit(){
    HtmlUtil.nextFocusByName(this.elementRef, 'MemoCtrl', EVENT_TYPE.NONE);

  }

  public setControlInit() {
    this.MemoCtrl = new FormControl("", [Validators.maxLength(100)]);
    this.Note1Ctrl = new FormControl("", [Validators.maxLength(100)]);
    this.Note2Ctrl = new FormControl("", [Validators.maxLength(100)]);
    this.Note3Ctrl = new FormControl("", [Validators.maxLength(100)]);
    this.Note4Ctrl = new FormControl("", [Validators.maxLength(100)]);
    this.Note5Ctrl = new FormControl("", [Validators.maxLength(100)]);
    this.Note6Ctrl = new FormControl("", [Validators.maxLength(100)]);
    this.Note7Ctrl = new FormControl("", [Validators.maxLength(100)]);
    this.Note8Ctrl = new FormControl("", [Validators.maxLength(100)]);

    this.UndefineCtrl = new FormControl("");

  }

  public setValidator() {
    this.MyFormGroup = new FormGroup({
      MemoCtrl: this.MemoCtrl,
      Note1Ctrl: this.Note1Ctrl,
      Note2Ctrl: this.Note2Ctrl,
      Note3Ctrl: this.Note3Ctrl,
      Note4Ctrl: this.Note4Ctrl,
      Note5Ctrl: this.Note5Ctrl,
      Note6Ctrl: this.Note6Ctrl,
      Note7Ctrl: this.Note7Ctrl,
      Note8Ctrl: this.Note8Ctrl,

      UndefineCtrl: this.UndefineCtrl,
    });
  }

  public setMemoCtrl(eventType:any){
    HtmlUtil.nextFocusByName(this.elementRef, 'Note2Ctrl', eventType);
  }  

  public setNote2Ctrl(eventType:any){
    HtmlUtil.nextFocusByName(this.elementRef, 'Note3Ctrl', eventType);
  }

  public setNote3Ctrl(eventType:any){
    HtmlUtil.nextFocusByName(this.elementRef, 'Note4Ctrl', eventType);
  }

  public setNote4Ctrl(eventType:any){
    HtmlUtil.nextFocusByName(this.elementRef, 'Note5Ctrl', eventType);
  }

  public setNote5Ctrl(eventType:any){
    HtmlUtil.nextFocusByName(this.elementRef, 'Note6Ctrl', eventType);
  }

  public setNote6Ctrl(eventType:any){
    HtmlUtil.nextFocusByName(this.elementRef, 'Note7Ctrl', eventType);
  }

  public setNote7Ctrl(eventType:any){
    HtmlUtil.nextFocusByName(this.elementRef, 'Note8Ctrl', eventType);
  }

  public setNote8Ctrl(eventType:any){
    HtmlUtil.nextFocusByName(this.elementRef, 'MemoCtrl', eventType);
  }

  public get Billing(): Billing {
    return this.billing;
  }
  public set Billing(value: Billing) {
    this.billing = value;
  }

  public get BillingMemo(): BillingMemo {
    return this.billingMemo;
  }
  public set BillingMemo(value: BillingMemo) {
    this.billingMemo = value;
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

  public get ColumnNameSettings(): ColumnNameSetting[] {
    return this.columnNameSettings;
  }

  public set ColumnNameSettings(value: ColumnNameSetting[]) {
    this.columnNameSettings = value;
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

    console.log("ModalInvoiceMemoComponent" + ":" + "registry(id=" + this.billingMemo.billingId + ")");

    this.billingMemo.memo = this.MemoCtrl.value;
    this.billing.note1 = this.Note1Ctrl.value;
    this.billing.note2 = this.Note2Ctrl.value;
    this.billing.note3 = this.Note3Ctrl.value;
    this.billing.note4 = this.Note4Ctrl.value;
    this.billing.note5 = this.Note5Ctrl.value;
    this.billing.note6 = this.Note6Ctrl.value;
    this.billing.note7 = this.Note7Ctrl.value;
    this.billing.note8 = this.Note8Ctrl.value;

    this.processModalCustomResult = this.processResultService.clearProcessCustomMsg(this.processModalCustomResult);
    this.closing.emit({});
  }

  public initDisplay() {

    console.log("ModalInvoiceMemoComponent" + ":" + "initDisplay");

    this.MemoCtrl.setValue(this.billingMemo.memo);
    this.Note1Ctrl.setValue(this.billing.note1);
    this.Note2Ctrl.setValue(this.billing.note2);
    this.Note3Ctrl.setValue(this.billing.note3);
    this.Note4Ctrl.setValue(this.billing.note4);
    this.Note5Ctrl.setValue(this.billing.note5);
    this.Note6Ctrl.setValue(this.billing.note6);
    this.Note7Ctrl.setValue(this.billing.note7);
    this.Note8Ctrl.setValue(this.billing.note8);

  }

  public clear() {
    this.MyFormGroup.reset();
    console.log("ModalInvoiceMemoComponent" + ":" + "clear");

  }

  public getColumnName(columnName:string):string{

    if(this.columnNameSettings == undefined) return "";

    let columnNameSetting = this.columnNameSettings.find(x => x.columnName == columnName);
    return StringUtil.IsNullOrEmpty(columnNameSetting.alias) ? columnNameSetting.originalName : columnNameSetting.alias;
  }

}
