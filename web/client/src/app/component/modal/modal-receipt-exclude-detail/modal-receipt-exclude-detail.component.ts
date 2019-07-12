import { ReceiptService } from 'src/app/service/receipt.service';
import { Component, OnInit, EventEmitter, ComponentFactoryResolver, ViewContainerRef } from '@angular/core';
import { BaseComponent } from '../../common/base/base-component';
import { FormGroup, FormControl } from '@angular/forms';
import { MODAL_STATUS_TYPE, PROCESS_RESULT_RESULT_TYPE } from 'src/app/common/const/status.const';
import { Receipt } from 'src/app/model/receipt.model';
import { ReceiptExcludesResult } from 'src/app/model/receipt-excludes-result.model';
import { UserInfoService } from 'src/app/service/common/user-info.service';
import { ReceiptExcludeService } from 'src/app/service/receipt-exclude.service';
import { DateUtil } from 'src/app/common/util/date-util';
import { RacCurrencyPipe } from 'src/app/pipe/currency.pipe';
import { CategoriesResult } from 'src/app/model/categories-result.model';
import { CategoryMasterService } from 'src/app/service/Master/category-master.service';
import { CategoryType } from 'src/app/common/const/kbn.const';
import { ModalConfirmComponent } from '../modal-confirm/modal-confirm.component';
import { ReceiptExclude } from 'src/app/model/receipt-exclude.model';
import { StringUtil } from 'src/app/common/util/string-util';
import { ProcessResultService } from 'src/app/service/common/process-result.service';
import { MSG_INF, MSG_ERR, MSG_ITEM_NUM } from 'src/app/common/const/message.const';

@Component({
  selector: 'app-modal-receipt-exclude-detail',
  templateUrl: './modal-receipt-exclude-detail.component.html',
  styleUrls: ['./modal-receipt-exclude-detail.component.css']
})
export class ModalReceiptExcludeDetailComponent extends BaseComponent implements OnInit {

  public excludeCategoriesResult:CategoriesResult;
  public receiptExcludesResult:ReceiptExcludesResult;

  public readonly receiptExcludeCount = 10;

  public pipe = new RacCurrencyPipe();
  
  public excludeAmountCtrls:FormControl[];
  public excludeCategoryIdCtrls:FormControl[];
  public outputAtCtrls:FormControl[];

  constructor(
    public componentFactoryResolver: ComponentFactoryResolver,
    public viewContainerRef: ViewContainerRef,
    public userInfoService:UserInfoService,
    public categoryService:CategoryMasterService,
    public receiptExcludeService:ReceiptExcludeService,    
    public receiptService:ReceiptService,    
    public processResultService:ProcessResultService
  ) {
    super();
   }

   ngOnInit() {
    this.setControlInit();
    this.setValidator();

    this.excludeCategoriesResult = new CategoriesResult();
    this.excludeCategoriesResult = new CategoriesResult();
    this.categoryService.GetItemsByCategoryType(CategoryType.Exclude)
      .subscribe(response=>{
        this.excludeCategoriesResult.categories = response;
      });

   }  


   public setControlInit() {

    this.excludeAmountCtrls = new Array<FormControl>(this.receiptExcludeCount);
    this.excludeCategoryIdCtrls = new Array<FormControl>(this.receiptExcludeCount);
    this.outputAtCtrls = new Array<FormControl>(this.receiptExcludeCount);

    for(let index=0;index<this.receiptExcludeCount;index++){
      this.excludeAmountCtrls[index] = new FormControl("");
      this.excludeCategoryIdCtrls[index] = new FormControl("");
      this.outputAtCtrls[index] = new FormControl(null);
    }

   }

   public setValidator() {
    this.MyFormGroup = new FormGroup({});

    for(let index=0;index<this.receiptExcludeCount;index++){

      this.MyFormGroup.removeControl("excludeAmountCtrl"+index);
      this.MyFormGroup.removeControl("excludeCategoryIdCtrl"+index);
      this.MyFormGroup.removeControl("outputAtCtrl"+index);

      this.MyFormGroup.addControl("excludeAmountCtrl"+index,this.excludeAmountCtrls[index]);
      this.MyFormGroup.addControl("excludeCategoryIdCtrl"+index,this.excludeCategoryIdCtrls[index]);
      this.MyFormGroup.addControl("outputAtCtrl"+index,this.outputAtCtrls[index]);
    }
  }

  public selectReceipt: Receipt = new Receipt();
  public get SelectReceipt(): Receipt {
    return this.selectReceipt;
  }
  public set SelectReceipt(value: Receipt) {
    this.selectReceipt = value;

    this.receiptExcludesResult = new ReceiptExcludesResult();

    this.receiptExcludeService.GetByReceiptId(this.selectReceipt.id)
      .subscribe(response=>{
        this.receiptExcludesResult.receiptExcludes=response;

        // 値の設定
        for(let index=0;index<this.receiptExcludesResult.receiptExcludes.length;index++){
          this.excludeAmountCtrls[index].setValue(this.pipe.transform(this.receiptExcludesResult.receiptExcludes[index].excludeAmount));
          this.excludeCategoryIdCtrls[index].setValue(this.receiptExcludesResult.receiptExcludes[index].excludeCategoryId);
          if (!StringUtil.IsNullOrEmpty(this.receiptExcludesResult.receiptExcludes[index].outputAt)) {
            this.outputAtCtrls[index].setValue(DateUtil.getYYYYMMDD(5, this.receiptExcludesResult.receiptExcludes[index].outputAt));
          }
        }

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
    this.ModalStatus = MODAL_STATUS_TYPE.CLOSE;
    this.processModalCustomResult = this.processResultService.clearProcessCustomMsg(this.processModalCustomResult);
    this.closing.emit({});
  }


  public cancel() {
    this.ModalStatus=MODAL_STATUS_TYPE.CANCEL;
    this.processModalCustomResult = this.processResultService.clearProcessCustomMsg(this.processModalCustomResult);
    this.closing.emit({});
  }

  public registry(){
    let updateItems = new Array<ReceiptExclude>();

    for(let index=0;index<this.receiptExcludeCount;index++){
      if(
            !StringUtil.IsNullOrEmpty(this.excludeAmountCtrls[index].value)
        ||  !StringUtil.IsNullOrEmpty(this.excludeCategoryIdCtrls[index].value)
      ){


        let receiptExclude = new ReceiptExclude();
      
        receiptExclude.receiptId           = this.selectReceipt.id;
        receiptExclude.excludeFlag         = 1;
        receiptExclude.excludeAmount       = parseInt(this.pipe.reverceTransform(this.excludeAmountCtrls[index].value));
        receiptExclude.excludeCategoryId   = this.excludeCategoryIdCtrls[index].value;
        receiptExclude.createBy            = this.userInfoService.LoginUser.id;
        receiptExclude.updateBy            = this.userInfoService.LoginUser.id;
        receiptExclude.receiptUpdateAt     = this.selectReceipt.updateAt;
      
        updateItems.push(receiptExclude);

      }
    }
    this.updateExclude(updateItems,this.selectReceipt.id,true);
  }

  public delete(){
    // 削除処理
    let componentFactory = this.componentFactoryResolver.resolveComponentFactory(ModalConfirmComponent);
    let componentRef = this.viewContainerRef.createComponent(componentFactory);
    componentRef.instance.ActionName = "削除"
    componentRef.instance.Closing.subscribe(() => {

      if (componentRef.instance.ModalStatus == MODAL_STATUS_TYPE.OK) {

        let updateItems = new Array<ReceiptExclude>();


        let receiptExclude = new ReceiptExclude();
      
        receiptExclude.receiptId           = this.selectReceipt.id;
        receiptExclude.excludeFlag         = 0;
        receiptExclude.excludeAmount       = 0;
        receiptExclude.excludeCategoryId   = null;
        receiptExclude.createBy            = this.userInfoService.LoginUser.id;
        receiptExclude.updateBy            = this.userInfoService.LoginUser.id;
        receiptExclude.receiptUpdateAt     = this.selectReceipt.updateAt;
            
        updateItems.push(receiptExclude);

        this.updateExclude(updateItems,this.selectReceipt.id,false);

        this.myFormGroup.reset();
      }
      componentRef.destroy();
    });    
  }


public updateExclude(updateItems:Array<ReceiptExclude>,receiptId:number,isRegistry:boolean){

    this.receiptService.SaveExcludeAmount(updateItems)
      .subscribe(response=>{

        if (response != PROCESS_RESULT_RESULT_TYPE.WARNING) {
          if(isRegistry){
            this.processModalCustomResult = this.processResultService.processAtSuccess(
              this.processModalCustomResult, MSG_INF.SAVE_SUCCESS, this.partsResultMessageComponent);
          }
          else{
            this.processModalCustomResult = this.processResultService.processAtSuccess(
              this.processModalCustomResult, MSG_INF.DELETE_SUCCESS, this.partsResultMessageComponent);
          }
  

          this.receiptExcludeService.GetByReceiptId(receiptId)
            .subscribe(response=>{
              this.receiptExcludesResult.receiptExcludes = response;
          });
        }
        else{
          this.processModalCustomResult = this.processResultService.processAtFailure(
            this.processModalCustomResult, MSG_ERR.SOMETHING_ERROR.replace(MSG_ITEM_NUM.FIRST,"処理"), this.partsResultMessageComponent);

        }
    });

  }

  public detailClear(index:number){
    this.excludeAmountCtrls[index].setValue(null);
    this.excludeCategoryIdCtrls[index].setValue(null);
    this.outputAtCtrls[index].setValue(null);
  }

  //////////////////////////////////////////////////////////////////
  public setExcludeAmount(eventType: string, index: number) {

  }

  public inputExcludeAmount() {


  }

  public setCurrencyForExcludeAmount(index:number){

    this.excludeAmountCtrls[index].setValue(this.pipe.transform(this.excludeAmountCtrls[index].value));
  }
  
  public initCurrencyForExcludeAmount(index:number){
    this.excludeAmountCtrls[index].setValue(this.pipe.reverceTransform(this.excludeAmountCtrls[index].value));
  }

}
