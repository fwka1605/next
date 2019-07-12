import { Component, OnInit, EventEmitter } from '@angular/core';
import { BaseComponent } from '../../common/base/base-component';
import { FormControl, FormGroup } from '@angular/forms';
import { MODAL_STATUS_TYPE } from 'src/app/common/const/status.const';
import { ProcessResultService } from 'src/app/service/common/process-result.service';

@Component({
  selector: 'app-modal-confirm-matching',
  templateUrl: './modal-confirm-matching.component.html',
  styleUrls: ['./modal-confirm-matching.component.css']
})
export class ModalConfirmMatchingComponent extends BaseComponent implements OnInit {
  public MemoCtrl: FormControl;
  public UndefineCtrl: FormControl;

  public closing = new EventEmitter<{}>();

  constructor(
    public processResultService:ProcessResultService
  ) {
    super();
   }

  ngOnInit() {

    this.setControlInit();
    this.setValidator();

  }

  public setControlInit() {
    this.MemoCtrl = new FormControl();

    this.UndefineCtrl = new FormControl();
  }

  public setValidator() {
    this.MyFormGroup = new FormGroup({
      MemoCtrl: this.MemoCtrl,
      UndefineCtrl: this.UndefineCtrl,
    });
  }

  public get Closing() {
    return this.closing;
  }
  public set Closing(value) {
    this.closing = value;
  }

  public remainType:number;
  public get RemainType(): number {
    return this.remainType;
  }
  public set RemainType(value: number) {
    this.remainType = value;
  }

  public remainAmount:number;
  public get RemainAmount(): number {
    return this.remainAmount;
  }
  public set RemainAmount(value: number) {
    this.remainAmount = value;
  }

  public carryOverAmount:number;
  public get CarryOverAmount(): number {
    return this.carryOverAmount;
  }
  public set CarryOverAmount(value: number) {
    this.carryOverAmount = value;
  }

  public customerName:string;
  public get CustomerName(): string {
    return this.customerName;
  }
  public set CustomerName(value: string) {
    this.customerName = value;
  }

  public close() {
    this.ModalStatus = MODAL_STATUS_TYPE.CANCEL;
    this.processModalCustomResult = this.processResultService.clearProcessCustomMsg(this.processModalCustomResult);
    this.closing.emit({});
  }

  public cancel() {
    this.ModalStatus = MODAL_STATUS_TYPE.CANCEL;
    this.processModalCustomResult = this.processResultService.clearProcessCustomMsg(this.processModalCustomResult);
    this.closing.emit({});
  }



  public yes() {
    this.ModalStatus = MODAL_STATUS_TYPE.OK;
    this.processModalCustomResult = this.processResultService.clearProcessCustomMsg(this.processModalCustomResult);
    this.closing.emit({});
  }

  public no() {
    this.ModalStatus = MODAL_STATUS_TYPE.NO;
    this.processModalCustomResult = this.processResultService.clearProcessCustomMsg(this.processModalCustomResult);
    this.closing.emit({});
  }

  public clear() {
    this.MyFormGroup.reset();
  }


}
