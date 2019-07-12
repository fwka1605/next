import { Component, OnInit, EventEmitter, AfterViewInit, ElementRef } from '@angular/core';
import { BaseComponent } from '../../common/base/base-component';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { MODAL_STATUS_TYPE } from 'src/app/common/const/status.const';
import { Customer } from 'src/app/model/customer.model';
import { CustomerMasterService } from 'src/app/service/Master/customer-master.service';
import { ProcessResultService } from 'src/app/service/common/process-result.service';
import { EVENT_TYPE } from 'src/app/common/const/event.const';
import { HtmlUtil } from 'src/app/common/util/html.util';

@Component({
  selector: 'app-modal-confirm-matching-advanced-customer',
  templateUrl: './modal-confirm-matching-advanced-customer.component.html',
  styleUrls: ['./modal-confirm-matching-advanced-customer.component.css']
})
export class ModalConfirmMatchingAdvancedCustomerComponent extends BaseComponent implements OnInit,AfterViewInit {

  public customerIdCtrl: FormControl;

  public customerIds:Array<number>;
  public customers:Array<Customer>;

  constructor(
    public customerService:CustomerMasterService,
    public elementRef: ElementRef,
    public processResultService:ProcessResultService
  ) {
    super();
   }

  ngOnInit() {

    this.setControlInit();
    this.setValidator();

    this.customerService.GetItemsByIds(this.customerIds)
    .subscribe(response=>{
      if(response!=undefined){
        this.customers=response;
      }
      else{
        this.customers=null;
      }
    })
  }

  ngAfterViewInit(){

    HtmlUtil.nextFocusByName(this.elementRef, 'customerIdCtrl', EVENT_TYPE.NONE);

    
  }

  public setControlInit() {
    this.customerIdCtrl = new FormControl("",[Validators.required]);
  }

  public setValidator() {
    this.MyFormGroup = new FormGroup({
      customerIdCtrl: this.customerIdCtrl,
    });
  }

  public closing = new EventEmitter<{}>();
  public get Closing() {
    return this.closing;
  }
  public set Closing(value) {
    this.closing = value;
  }

  public select() {
    this.ModalStatus = MODAL_STATUS_TYPE.SELECT;
    this.processModalCustomResult = this.processResultService.clearProcessCustomMsg(this.processModalCustomResult);
    this.closing.emit({});
  }  

  public cancel() {
    this.ModalStatus = MODAL_STATUS_TYPE.CANCEL;
    this.processModalCustomResult = this.processResultService.clearProcessCustomMsg(this.processModalCustomResult);
    this.closing.emit({});
  }  

  public close() {
    this.ModalStatus = MODAL_STATUS_TYPE.CANCEL;
    this.processModalCustomResult = this.processResultService.clearProcessCustomMsg(this.processModalCustomResult);
    this.closing.emit({});
  }

  public clear() {
    this.MyFormGroup.reset();

  }

}
