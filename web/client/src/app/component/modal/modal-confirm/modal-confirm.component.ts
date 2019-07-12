import { Component, OnInit, EventEmitter } from '@angular/core';
import { BaseComponent } from '../../common/base/base-component';
import { MODAL_STATUS_TYPE } from 'src/app/common/const/status.const';
import { ProcessResultService } from 'src/app/service/common/process-result.service';

@Component({
  selector: 'app-modal-confirm',
  templateUrl: './modal-confirm.component.html',
  styleUrls: ['./modal-confirm.component.css']
})
export class ModalConfirmComponent extends BaseComponent implements OnInit {

  constructor(
    public processResultService: ProcessResultService,
  ) {
    super();
   }
   
  public closing = new EventEmitter<{}>();
  public get Closing() {
    return this.closing;
  }

  public set Closing(value) {
    this.closing = value;
  }

  public close() {
    this.ModalStatus=MODAL_STATUS_TYPE.CLOSE;
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
  public prefixDetail:string = "";
  public sufixDetail:string = "";

  public get ActionName() {
    return this.actionName;
  }

  public set ActionName(value) {
    this.actionName = value;
    
  }  

  public get PrefixDetail() {
    return this.prefixDetail;
  }

  public set PrefixDetail(value) {
    this.prefixDetail = value;
  }  

  public get SufixDetail() {
    return this.sufixDetail;
  }  

  public set SufixDetail(value) {
    this.sufixDetail = value;
  }  


  ngOnInit() {
  }

}
