import { Component, OnInit, EventEmitter, ElementRef, ComponentFactoryResolver, ViewContainerRef } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { BaseComponent } from '../../common/base/base-component';
import { EBExcludeAccountSettingMasterService } from 'src/app/service/Master/ebexclude-account-setting-master.service';
import { EXCLUSIVE_ACCOUNT_TYPE } from 'src/app/common/const/kbn.const';
import { EBExcludeAccountSettingListResult } from 'src/app/model/eb-exclude-account-setting-list-result.model';
import { FormatterUtil } from 'src/app/common/util/formatter.util';
import { HtmlUtil } from 'src/app/common/util/html.util';
import { StringUtil } from 'src/app/common/util/string-util';
import { EBExcludeAccountSetting } from 'src/app/model/eb-exclude-account-setting.model';
import { UserInfoService } from 'src/app/service/common/user-info.service';
import { ModalConfirmComponent } from '../modal-confirm/modal-confirm.component';
import { MODAL_STATUS_TYPE, PROCESS_RESULT_RESULT_TYPE } from 'src/app/common/const/status.const';
import { ProcessResultService } from 'src/app/service/common/process-result.service';
import { BUTTON_ACTION } from 'src/app/common/const/event.const';
import { ModalRouterProgressComponent } from '../modal-router-progress/modal-router-progress.component';

@Component({
  selector: 'app-modal-eb-exclude-accounting-setting',
  templateUrl: './modal-eb-exclude-accounting-setting.component.html',
  styleUrls: ['./modal-eb-exclude-accounting-setting.component.css']
})
export class ModalEbExcludeAccountingSettingComponent extends BaseComponent implements OnInit {

  public closing = new EventEmitter<{}>();

  public ebExcludeAccountSettingsResult: EBExcludeAccountSettingListResult

  public selectedEbExcludeIndex:number=-1;

  public readonly accountTypeDictionary = EXCLUSIVE_ACCOUNT_TYPE;

  public bankCodeCtrl: FormControl;
  public branchCodeCtrl: FormControl;
  public accountTypeIdCtrl: FormControl;
  public payerCode1Ctrl: FormControl;
  public payerCode2Ctrl: FormControl;

  public undefineCtrl: FormControl;


  constructor(
    public elementRef:ElementRef,
    public componentFactoryResolver: ComponentFactoryResolver,
    public viewContainerRef: ViewContainerRef,
    public userInfoService:UserInfoService,
    public eBExcludeAccountSettingService: EBExcludeAccountSettingMasterService,
    public processResultService:ProcessResultService
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

    this.bankCodeCtrl = new FormControl("", [Validators.required,Validators.maxLength(4)]);
    this.branchCodeCtrl = new FormControl("", [Validators.required,Validators.maxLength(3)]);
    this.accountTypeIdCtrl = new FormControl("", [Validators.required]);
    this.payerCode1Ctrl = new FormControl("", [Validators.required,Validators.maxLength(3)]);
    this.payerCode2Ctrl = new FormControl("", [Validators.required, ,Validators.maxLength(7)]);

    this.undefineCtrl = new FormControl("");
  }

  public setValidator() {
    this.MyFormGroup = new FormGroup({

      bankCodeCtrl: this.bankCodeCtrl,
      branchCodeCtrl: this.branchCodeCtrl,
      accountTypeIdCtrl: this.accountTypeIdCtrl,
      payerCode1Ctrl: this.payerCode1Ctrl,
      payerCode2Ctrl: this.payerCode2Ctrl,

      UndefineCtrl: this.undefineCtrl,
    });

  }

  public setFormatter(){
    FormatterUtil.setNumberFormatter(this.bankCodeCtrl);
    FormatterUtil.setNumberFormatter(this.branchCodeCtrl);
    FormatterUtil.setNumberFormatter(this.payerCode1Ctrl);
    FormatterUtil.setNumberFormatter(this.payerCode2Ctrl);
  }

  public get Closing() {
    return this.closing;
  }
  public set Closing(value) {
    this.closing = value;
  }

  public get ProcessModalCustomResult() {
    return this.processModalCustomResult;
  }
  public set ProcessModalCustomResult(value) {
    this.processModalCustomResult = value;
  }

  public clear() {
    this.MyFormGroup.reset();

    this.selectedEbExcludeIndex=-1;
    this.GetItems();

    this.processResultService.clearProcessCustomMsg(this.processModalCustomResult);
  }

  public GetItems(){
    this.eBExcludeAccountSettingService.GetItems()
      .subscribe(response=>{
        this.ebExcludeAccountSettingsResult = new EBExcludeAccountSettingListResult();
        this.ebExcludeAccountSettingsResult.eBExcludeAccountSettingList = response;
        
      });

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


  /**
   * ボタン操作によるメソッド呼び出し
   * @param action ボタン操作
   */
  public buttonActionInner(action: BUTTON_ACTION) {
    this.processResultService.processResultStart(this.processModalCustomResult, action);
    if (this.processModalCustomResult.result == PROCESS_RESULT_RESULT_TYPE.FAILURE) {
      return;
    }

    switch (action) {

      case BUTTON_ACTION.REGISTRY:
        this.registry();
        break;
      case BUTTON_ACTION.DELETE:
        this.delete();
        break;
      default:
        console.log('buttonAction Error.');
        break;
    }
    
  }


  public registry() {


    let processComponentFactory = this.componentFactoryResolver.resolveComponentFactory(ModalRouterProgressComponent);
    let processComponentRef = this.viewContainerRef.createComponent(processComponentFactory);
    // processComponentRef.destroy();       

    let ebExcludeAccountSetting = new EBExcludeAccountSetting;
    ebExcludeAccountSetting.companyId = this.userInfoService.Company.id;
    ebExcludeAccountSetting.bankCode = this.bankCodeCtrl.value;
    ebExcludeAccountSetting.branchCode = this.branchCodeCtrl.value;
    ebExcludeAccountSetting.accountTypeId = this.accountTypeIdCtrl.value;
    ebExcludeAccountSetting.payerCode = this.payerCode1Ctrl.value + this.payerCode2Ctrl.value;
    ebExcludeAccountSetting.createBy = this.userInfoService.LoginUser.id;
    //ebExcludeAccountSetting.createAt
    ebExcludeAccountSetting.updateBy= this.userInfoService.LoginUser.id;
    //ebExcludeAccountSetting.updateAt

    this.eBExcludeAccountSettingService.Save(ebExcludeAccountSetting)
      .subscribe(response=>{

        this.processResultService.processAtSave(
          this.processModalCustomResult, response, true, this.partsResultMessageComponent);

        this.ebExcludeAccountSettingsResult = new EBExcludeAccountSettingListResult();
        this.ebExcludeAccountSettingsResult.eBExcludeAccountSettingList = response;
        this.GetItems();

        processComponentRef.destroy();  
      })
  }

  public delete() {

    // 削除処理
    let componentFactory = this.componentFactoryResolver.resolveComponentFactory(ModalConfirmComponent);
    let componentRef = this.viewContainerRef.createComponent(componentFactory);
    componentRef.instance.ActionName = "削除"
    componentRef.instance.Closing.subscribe(() => {

      if (componentRef.instance.ModalStatus == MODAL_STATUS_TYPE.OK) {

        let processComponentFactory = this.componentFactoryResolver.resolveComponentFactory(ModalRouterProgressComponent);
        let processComponentRef = this.viewContainerRef.createComponent(processComponentFactory);
        // processComponentRef.destroy();    

        this.eBExcludeAccountSettingService.Delete(this.ebExcludeAccountSettingsResult.eBExcludeAccountSettingList[this.selectedEbExcludeIndex])
          .subscribe(response=>{

            this.processResultService.processAtDelete(
              this.processModalCustomResult, response, this.partsResultMessageComponent);

            this.ebExcludeAccountSettingsResult = new EBExcludeAccountSettingListResult();
            this.ebExcludeAccountSettingsResult.eBExcludeAccountSettingList = response;
            this.GetItems();
          })
        this.clear();
        processComponentRef.destroy();  
      }
      componentRef.destroy();
    });

  }

  public selectSetting(index: number) {

    this.selectedEbExcludeIndex = index;
    let tmpSetting = this.ebExcludeAccountSettingsResult.eBExcludeAccountSettingList[index];

    this.bankCodeCtrl.setValue(tmpSetting.bankCode);
    this.branchCodeCtrl.setValue(tmpSetting.branchCode);
    this.accountTypeIdCtrl.setValue(tmpSetting.accountTypeId);
    this.payerCode1Ctrl.setValue(tmpSetting.payerCode.substring(0,3));
    this.payerCode2Ctrl.setValue(tmpSetting.payerCode.substring(3));

  }

  public setBankCode(eventType:string){
    if (!StringUtil.IsNullOrEmpty(this.bankCodeCtrl.value)){
      this.bankCodeCtrl.setValue(StringUtil.setPaddingFrontZero(this.bankCodeCtrl.value, 4, true))
    }
    HtmlUtil.nextFocusByName(this.elementRef, 'branchCodeCtrl', eventType);
  }

  public setBranchCode(eventType:string){
    if (!StringUtil.IsNullOrEmpty(this.branchCodeCtrl.value)){
      this.branchCodeCtrl.setValue(StringUtil.setPaddingFrontZero(this.branchCodeCtrl.value, 3, true))
    }
    HtmlUtil.nextFocusByName(this.elementRef, 'payerCode1Ctrl', eventType);
  }

  public setPayerCode1(eventType:string){
    if (!StringUtil.IsNullOrEmpty(this.payerCode1Ctrl.value)){
      this.payerCode1Ctrl.setValue(StringUtil.setPaddingFrontZero(this.payerCode1Ctrl.value, 3, true))
    }
    HtmlUtil.nextFocusByName(this.elementRef, 'payerCode2Ctrl', eventType);
  }

  public setPayerCode2(eventType:string){
    if (!StringUtil.IsNullOrEmpty(this.payerCode2Ctrl.value)){
      this.payerCode2Ctrl.setValue(StringUtil.setPaddingFrontZero(this.payerCode2Ctrl.value, 7, true))
    }
    HtmlUtil.nextFocusByName(this.elementRef, 'bankCodeCtrl', eventType);
  }

}
