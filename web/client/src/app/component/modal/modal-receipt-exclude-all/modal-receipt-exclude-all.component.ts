import { Component, OnInit, EventEmitter } from '@angular/core';
import { BaseComponent } from '../../common/base/base-component';
import { MODAL_STATUS_TYPE } from 'src/app/common/const/status.const';
import { Category } from 'src/app/model/category.model';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { StringUtil } from 'src/app/common/util/string-util';
import { ProcessResultService } from 'src/app/service/common/process-result.service';
import { MSG_WNG, MSG_ITEM_NUM } from 'src/app/common/const/message.const';

@Component({
  selector: 'app-modal-receipt-exclude-all',
  templateUrl: './modal-receipt-exclude-all.component.html',
  styleUrls: ['./modal-receipt-exclude-all.component.css']
})
export class ModalReceiptExcludeAllComponent extends BaseComponent implements OnInit {

  public excludeCategoryIdCtrl:FormControl;

  public selectedId: string;
  public selectedCode: string;
  public selectedName: string;


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

    this.excludeCategoryIdCtrl = new FormControl("",[Validators.required]); 
   }

   public setValidator() {
    this.MyFormGroup = new FormGroup({

      excludeCategoryIdCtrl: this.excludeCategoryIdCtrl,
    });
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

  public select() {
    this.ModalStatus=MODAL_STATUS_TYPE.SELECT;

    if(StringUtil.IsNullOrEmpty(this.excludeCategoryIdCtrl.value)){
      this.processModalCustomResult = this.processResultService.processAtWarning(
        this.processModalCustomResult, MSG_WNG.SELECTION_REQUIRED.replace(MSG_ITEM_NUM.FIRST, '対象外区分'),
        this.partsResultMessageComponent);
    }
    else{
      this.selectedId=this.excludeCategoryIdCtrl.value;
      this.processModalCustomResult = this.processResultService.clearProcessCustomMsg(this.processModalCustomResult);
      this.closing.emit({});
    }
  }

  public cancel() {
    this.ModalStatus=MODAL_STATUS_TYPE.CANCEL;
    this.processModalCustomResult = this.processResultService.clearProcessCustomMsg(this.processModalCustomResult);
    this.closing.emit({});
  }

  public excludeCategories: Array<Category>;
  public get ExcludeCategories(): Array<Category> {
    return this.excludeCategories;
  }
  public set ExcludeCategories(value: Array<Category>) {
    this.excludeCategories = value;
  }

  public get SelectedId(): string {
    return this.selectedId;
  }
  public set SelectedId(value: string) {
    this.selectedId = value;
  }

  public get SelectedCode(): string {
    return this.selectedCode;
  }
  public set SelectedCode(value: string) {
    this.selectedCode = value;
  }

  public get SelectedName(): string {
    return this.selectedName;
  }
  public set SelectedName(value: string) {
    this.selectedName = value;
  }  
}
