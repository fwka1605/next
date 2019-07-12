import { Component, OnInit, EventEmitter } from '@angular/core';
import { BaseComponent } from '../../common/base/base-component';
import { MODAL_STATUS_TYPE } from 'src/app/common/const/status.const';
import { FormControl, Validators, FormGroup } from '@angular/forms';
import { ProcessResultService } from 'src/app/service/common/process-result.service';


@Component({
  selector: 'app-modal-collate-print',
  templateUrl: './modal-collate-print.component.html',
  styleUrls: ['./modal-collate-print.component.css']
})
export class ModalCollatePrintComponent extends BaseComponent implements OnInit {


  public rdoPrintDataCtrl: FormControl;  // 出力内容

  constructor(
    public processResultService: ProcessResultService
  ) {
    super();
  }

  ngOnInit() {
    this.setControlInit();
    this.setValidator();
    this.setFormatter();
    this.clear();
  }

  public setControlInit() {
    this.rdoPrintDataCtrl = new FormControl(null, [Validators.required]);  // 出力内容

  }

  public setValidator() {
    this.MyFormGroup = new FormGroup({
      rdoPrintDataCtrl: this.rdoPrintDataCtrl,   // 出力内容

    });
  }

  public setFormatter() {

  }

  public clear() {
    this.MyFormGroup.reset();

    // 初期値を全体にする。
    this.rdoPrintDataCtrl.setValue("0");
  }


  public closing = new EventEmitter<{}>();
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

  public ok() {
    this.ModalStatus = MODAL_STATUS_TYPE.OK;
    this.processModalCustomResult = this.processResultService.clearProcessCustomMsg(this.processModalCustomResult);
    this.closing.emit({});
  }

  public cancel() {
    this.ModalStatus = MODAL_STATUS_TYPE.CANCEL;
    this.processModalCustomResult = this.processResultService.clearProcessCustomMsg(this.processModalCustomResult);
    this.closing.emit({});
  }

  public actionName: string = "処理";

  public get ActionName() {
    return this.actionName;
  }

  public set ActionName(value) {
    this.actionName = value;
  }

}
