import { Component, OnInit, EventEmitter, AfterViewInit, ElementRef } from '@angular/core';
import { BaseComponent } from '../../common/base/base-component';
import { FormControl, FormGroup } from '@angular/forms';
import { MODAL_STATUS_TYPE } from 'src/app/common/const/status.const';
import { CategoryType } from 'src/app/common/const/kbn.const';
import { ProcessResultService } from 'src/app/service/common/process-result.service';
import { HtmlUtil } from 'src/app/common/util/html.util';
import { EVENT_TYPE } from 'src/app/common/const/event.const';

@Component({
  selector: 'app-modal-individual-memo',
  templateUrl: './modal-individual-memo.component.html',
  styleUrls: ['./modal-individual-memo.component.css']
})
export class ModalIndividualMemoComponent extends BaseComponent implements OnInit,AfterViewInit {

  public memoCtrl: FormControl;
  public memo: string;
  public categoryType: CategoryType;


  public UndefineCtrl: FormControl;


  public closing = new EventEmitter<{}>();
  
  constructor(
    public elementRef: ElementRef,
    public processResultService:ProcessResultService
  ) {
    super();
   }

  ngOnInit() {

    // 入力チェックの設定
    this.setControlInit();
    this.setValidator();

    this.initDisplay();

  }

  ngAfterViewInit(){
    HtmlUtil.nextFocusByName(this.elementRef, 'memoCtrl', EVENT_TYPE.NONE);
  }

  public setControlInit() {
    this.memoCtrl = new FormControl(this.memo);
    this.UndefineCtrl = new FormControl();
  }

  public setValidator() {
    this.MyFormGroup = new FormGroup({
      memoCtrl: this.memoCtrl,
      UndefineCtrl: this.UndefineCtrl,
    });
  }


  public get Closing() {
    return this.closing;
  }
  public set Closing(value) {
    this.closing = value;
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

    this.ModalStatus = MODAL_STATUS_TYPE.OK;
    this.closing.emit({});
  }

  public initDisplay() {

  }

  public clear() {
    this.MyFormGroup.reset();

  }




}
