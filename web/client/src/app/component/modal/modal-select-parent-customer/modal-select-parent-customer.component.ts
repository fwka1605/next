import { Component, OnInit, EventEmitter } from '@angular/core';
import { BaseComponent } from '../../common/base/base-component';
import { FormControl, FormGroup } from '@angular/forms';
import { MODAL_STATUS_TYPE } from 'src/app/common/const/status.const';
import { Customer } from 'src/app/model/customer.model';
import { ProcessResultService } from 'src/app/service/common/process-result.service';

@Component({
  selector: 'app-modal-select-parent-customer',
  templateUrl: './modal-select-parent-customer.component.html',
  styleUrls: ['./modal-select-parent-customer.component.css']
})
export class ModalSelectParentCustomerComponent extends BaseComponent implements OnInit {
  public parentCustomerIdCtrl: FormControl;

  public UndefineCtrl: FormControl;

  public customers: Array<Customer>;

  public closing = new EventEmitter<{}>();

  constructor(
    private processResultService:ProcessResultService
  ) {
    super();
   }

  ngOnInit() {

    // 入力チェックの設定
    this.setControlInit();
    this.setValidator();

  }

  public setControlInit() {
    this.parentCustomerIdCtrl = new FormControl();


    this.UndefineCtrl = new FormControl();

  }

  public setValidator() {
    this.MyFormGroup = new FormGroup({
      parentCustomerIdCtrl: this.parentCustomerIdCtrl,
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
    this.processCustomResult = this.processResultService.clearProcessCustomMsg(this.processCustomResult);
    this.closing.emit({});
  }

  public cancel() {
    this.ModalStatus = MODAL_STATUS_TYPE.CANCEL;
    this.processCustomResult = this.processResultService.clearProcessCustomMsg(this.processCustomResult);
    this.closing.emit({});
  }  

  public select() {
    this.ModalStatus = MODAL_STATUS_TYPE.OK;
    this.processModalCustomResult = this.processResultService.clearProcessCustomMsg(this.processModalCustomResult);
    this.closing.emit({});
  }


  public clear() {
    this.MyFormGroup.reset();
  }

}
