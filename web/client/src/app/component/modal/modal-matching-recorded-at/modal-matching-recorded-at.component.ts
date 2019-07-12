import { Component, OnInit, EventEmitter, AfterViewInit, ElementRef } from '@angular/core';
import { BaseComponent } from '../../common/base/base-component';
import { MODAL_STATUS_TYPE } from 'src/app/common/const/status.const';
import { FormControl, FormGroup } from '@angular/forms';
import { ADVANCED_RECEIVED_RECORDED_DATA_TYPE } from 'src/app/common/const/kbn.const';
import { NgbCalendar } from '@ng-bootstrap/ng-bootstrap';
import { StringUtil } from 'src/app/common/util/string-util';
import { CollationSetting } from 'src/app/model/collation-setting.model';
import { ProcessResultService } from 'src/app/service/common/process-result.service';
import { HtmlUtil } from 'src/app/common/util/html.util';
import { EVENT_TYPE } from 'src/app/common/const/event.const';
import { NgbDateParserFormatter, NgbDate } from '@ng-bootstrap/ng-bootstrap';
import { NgbDateCustomParserFormatter } from 'src/app/common/util/dateformat-util';


@Component({
  selector: 'app-modal-matching-recorded-at',
  templateUrl: './modal-matching-recorded-at.component.html',
  styleUrls: ['./modal-matching-recorded-at.component.css'],
  providers: [
    { provide: NgbDateParserFormatter, useClass: NgbDateCustomParserFormatter }
  ]  
})
export class ModalMatchingRecordedAtComponent extends BaseComponent implements OnInit,AfterViewInit {

  public readonly receiptRecordedAtItems = ADVANCED_RECEIVED_RECORDED_DATA_TYPE;
  
   constructor(
    public elementRef: ElementRef,
    public calendar: NgbCalendar,
    public processResultService:ProcessResultService

  ) {
    super();
   }   

   ngOnInit() {


    this.setControlInit();
    this.setValidator();
    this.setFormatter();
    this.clear();
    this.initMatchingDate();
   }

   ngAfterViewInit(){
    HtmlUtil.nextFocusByName(this.elementRef, 'matchingDateCtrl', EVENT_TYPE.NONE);

    
   }

   public setControlInit(){
    this.matchingDateCtrl = new FormControl("");

  }

  public setValidator() {
    this.MyFormGroup = new FormGroup({
      matchingDateCtrl: this.matchingDateCtrl,   // 消込日


    });
  }  

  public setFormatter(){
    // 消込日
    // FormatterUtil.setDateFormatter(this.matchingDateCtrl);
  }  

  public clear(){
    this.MyFormGroup.reset();
    HtmlUtil.nextFocusByName(this.elementRef, 'matchingDateCtrl', EVENT_TYPE.NONE);

  }

  public initMatchingDate(){

    if (!StringUtil.IsNullOrEmpty(this.advancedDate)) {
      const date = new Date(this.advancedDate);
      this.matchingDateCtrl.setValue(new NgbDate(date.getFullYear(), date.getMonth() + 1, date.getDate()));
    }
    else {
      this.matchingDateCtrl.reset();
    }
    
  }


  public closing = new EventEmitter<{}>();
  public get Closing() {
    return this.closing;
  }

  public set Closing(value) {
    this.closing = value;
  }

  public close() {
    this.ModalStatus = MODAL_STATUS_TYPE.CANCEL;
    this.processModalCustomResult = this.processResultService.clearProcessCustomMsg(this.processModalCustomResult);
    this.closing.emit({});
  }

  public ok() {
    this.ModalStatus=MODAL_STATUS_TYPE.OK;
    this.processModalCustomResult = this.processResultService.clearProcessCustomMsg(this.processModalCustomResult);
    this.closing.emit({});
  }

  public cancel() {
    this.ModalStatus=MODAL_STATUS_TYPE.CANCEL;
    this.processModalCustomResult = this.processResultService.clearProcessCustomMsg(this.processModalCustomResult);
    this.closing.emit({});
  }

  public actionName:string = "処理";

  public get ActionName() {
    return this.actionName;
  }

  public set ActionName(value) {
    this.actionName = value;
  }  

  public matchingDateCtrl: FormControl;  // 消込日
  public get MatchingDateCtrl() {
    return this.matchingDateCtrl;
  }

  public set MatchingDateCtrl(value:FormControl) {
    this.matchingDateCtrl = value;
  }

  public collationSetting: CollationSetting;  //照合設定
  public get CollationSetting() {
    return this.collationSetting;
  }

  public set CollationSetting(value:CollationSetting) {
    this.collationSetting = value;
  }

  /// <summary>前受金消込処理年月日</summary>
  /// <remarks>
  /// 一括消込の場合、Serviceで設定する為、指定不要
  /// 個別消込の場合、前受伝票日付設定方法により指定が必要
  /// </remarks>
  public advancedDate: string;
  public get AdvancedDate() {
    return this.advancedDate;
  }

  public set AdvancedDate(value:string) {
    this.advancedDate = value;
  }



}
