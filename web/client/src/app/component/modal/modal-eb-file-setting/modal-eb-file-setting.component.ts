import { Component, OnInit, EventEmitter, ComponentFactoryResolver, ViewContainerRef } from '@angular/core';
import { BaseComponent } from '../../common/base/base-component';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { EBFormatsResult } from 'src/app/model/eb-formats-result.model';
import { EBFormatMasterService } from 'src/app/service/Master/ebformat-master.service';
import { FILE_FIELD_TYPE_NAMES } from 'src/app/common/const/eb-file.const';
import { FormatterUtil } from 'src/app/common/util/formatter.util';
import { StringUtil } from 'src/app/common/util/string-util';
import { EBFileSetting } from 'src/app/model/eb-file-setting.model';
import { UserInfoService } from 'src/app/service/common/user-info.service';
import { EBFileSettingMasterService } from 'src/app/service/Master/ebfile-setting-master.service';
import { ProcessResultService } from 'src/app/service/common/process-result.service';
import { BUTTON_ACTION } from 'src/app/common/const/event.const';
import { PROCESS_RESULT_RESULT_TYPE, COMPONENT_STATUS_TYPE, MODAL_STATUS_TYPE } from 'src/app/common/const/status.const';
import { EBFormat } from 'src/app/model/eb-format.model';
import { ModalConfirmComponent } from '../modal-confirm/modal-confirm.component';
import { ModalRouterProgressComponent } from '../modal-router-progress/modal-router-progress.component';

@Component({
  selector: 'app-modal-eb-file-setting',
  templateUrl: './modal-eb-file-setting.component.html',
  styleUrls: ['./modal-eb-file-setting.component.css']
})
export class ModalEbFileSettingComponent extends BaseComponent implements OnInit {

  public closing = new EventEmitter<{}>();
  public fileFiledTypeNames: Array<{ id:number, val:string; }> ;

  public ebFormatsResult: EBFormatsResult;

  public isDateSelectable:boolean=false;

  public ebFileFormatIdCtrl: FormControl;  // EBフォーマット
  public fileFieldTypeCtrl: FormControl; // 区切り文字
  public fileSettingNameCtrl: FormControl; // EBファイル設定名
  public bankCodeCtrl: FormControl; // 銀行コード
  public importableValueCtrl: FormControl; // 取込区分
  public displayOrderCtrl: FormControl; // 表示順
  public receiveDateCtrl: FormControl; // 入金日指定

  public undefineCtrl: FormControl;

  public isDelete : boolean = true;
  public selectData: EBFileSetting = null;


  constructor(
    public ebFormatService: EBFormatMasterService,
    public ebFileSettingService:EBFileSettingMasterService,
    public userInfoService: UserInfoService,
    public processResultService:ProcessResultService,
    public componentFactoryResolver: ComponentFactoryResolver,
    public viewContainerRef: ViewContainerRef
  ) { 
    super(); 
  }


  ngOnInit() {
    this.setControlInit();
    this.setValidator();
    this.setFormatter();
    this.clear();

    this.ebFormatService.GetItems()
      .subscribe(response=>{
        this.ebFormatsResult = new EBFormatsResult();
        this.ebFormatsResult.eBFileFormats=response;
        if (this.selectData != null) {
          this.isDelete = false;
          this.ComponentStatus = COMPONENT_STATUS_TYPE.UPDATE;
          this.setSelectDatatoForm();
        }            
      });
  }

  public setControlInit() {
    this.ebFileFormatIdCtrl = new FormControl("",[Validators.required]);
    this.fileFieldTypeCtrl = new FormControl("",[Validators.required]);
    this.fileSettingNameCtrl = new FormControl("",[Validators.required,Validators.maxLength(40)]);
    this.bankCodeCtrl = new FormControl("",[Validators.maxLength(4)]);
    this.importableValueCtrl = new FormControl("",[Validators.maxLength(50)]);
    this.displayOrderCtrl = new FormControl("",[Validators.required,Validators.maxLength(4)]);
    this.receiveDateCtrl = new FormControl("0");
    this.undefineCtrl = new FormControl("");
  }

  public setValidator() {
    this.MyFormGroup = new FormGroup({
      ebFileFormatIdCtrl: this.ebFileFormatIdCtrl,
      fileFieldTypeCtrl: this.fileFieldTypeCtrl,
      fileSettingNameCtrl: this.fileSettingNameCtrl,
      bankCodeCtrl: this.bankCodeCtrl,
      importableValueCtrl: this.importableValueCtrl,
      displayOrderCtrl: this.displayOrderCtrl,
      receiveDateCtrl: this.receiveDateCtrl,

      UndefineCtrl: this.undefineCtrl,
    });
  }

  public setFormatter(){
    FormatterUtil.setNumberFormatter(this.bankCodeCtrl);
    FormatterUtil.setNumberFormatter(this.displayOrderCtrl);
  }

  public clear() {
    this.MyFormGroup.reset();
    this.receiveDateCtrl.setValue("0");

    if (this.ebFormatsResult != undefined) {
      this.setSelectDatatoForm();
    }

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
  
  public get SelectData() {
    return this.selectData;
  }
  public set SelectData(value) {
    this.selectData = value;
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
   * データ登録・更新
   */
  public registry() {

    let processComponentFactory = this.componentFactoryResolver.resolveComponentFactory(ModalRouterProgressComponent);
    let processComponentRef = this.viewContainerRef.createComponent(processComponentFactory);
    // processComponentRef.destroy();    


    let ebFileSetting = new EBFileSetting();

    if (this.ComponentStatus != COMPONENT_STATUS_TYPE.UPDATE) {
      ebFileSetting.id=0;
      ebFileSetting.companyId=this.userInfoService.Company.id;
      ebFileSetting.isUseable=1;
      ebFileSetting.createBy=this.userInfoService.LoginUser.id;
    //ebFileSetting.createAt=0;

    } else {
      ebFileSetting = this.selectData;
    }

    ebFileSetting.name=this.fileSettingNameCtrl.value;
    ebFileSetting.displayOrder=this.displayOrderCtrl.value;
    ebFileSetting.ebFormatId=this.ebFormatsResult.eBFileFormats[this.ebFileFormatIdCtrl.value].id;
    ebFileSetting.fileFieldType=this.fileFieldTypeCtrl.value;
    ebFileSetting.bankCode=this.bankCodeCtrl.value==null?"":this.bankCodeCtrl.value;
    ebFileSetting.useValueDate=this.receiveDateCtrl.value==1?1:0;
    ebFileSetting.importableValues=this.importableValueCtrl.value==null?"":this.importableValueCtrl.value.replace(new RegExp("^,+|,+$", "g"),'');
    ebFileSetting.filePath = "";
    ebFileSetting.updateBy=this.userInfoService.LoginUser.id;
    //ebFileSetting.updateAt=0;
    //ebFileSetting.requireYear

    this.ebFileSettingService.Save(ebFileSetting)
      .subscribe(response => {

        this.processResultService.processAtSave(
          this.processModalCustomResult, response, true, this.partsResultMessageComponent);
        if (this.processModalCustomResult.result == PROCESS_RESULT_RESULT_TYPE.SUCCESS) {
          // this.close();
        }
        processComponentRef.destroy();    
      });
  }

  /**
   * データ削除
   */
  public delete() {
    let componentFactory = this.componentFactoryResolver.resolveComponentFactory(ModalConfirmComponent);
    let componentRef = this.viewContainerRef.createComponent(componentFactory);

    componentRef.instance.ActionName = "削除"
    componentRef.instance.Closing.subscribe(() => {

      if (componentRef.instance.ModalStatus == MODAL_STATUS_TYPE.OK) {

        let processComponentFactory = this.componentFactoryResolver.resolveComponentFactory(ModalRouterProgressComponent);
        let processComponentRef = this.viewContainerRef.createComponent(processComponentFactory);
        // processComponentRef.destroy();    

        this.ebFileSettingService.Delete(this.selectData)
          .subscribe(response => {
            this.processModalCustomResult = this.processResultService.processAtDelete(
              this.processModalCustomResult, response, this.partsResultMessageComponent);
            if (this.processModalCustomResult.result == PROCESS_RESULT_RESULT_TYPE.SUCCESS) {
              // this.close();
            }
            processComponentRef.destroy();  
          });
      }
      componentRef.destroy();
    });
  }

  public setEBFileFormatId(){
    this.fileFiledTypeNames = new Array<{ id:number, val:string; }>();
    let fileFormat = new EBFormat();

    if (this.ComponentStatus != COMPONENT_STATUS_TYPE.UPDATE) {
      // 初期化
      this.fileFieldTypeCtrl.reset();
      this.fileSettingNameCtrl.reset();
      this.bankCodeCtrl.reset();
      this.displayOrderCtrl.reset();
      this.bankCodeCtrl.setValue("");
      this.receiveDateCtrl.setValue("0");
      fileFormat = this.ebFormatsResult.eBFileFormats[this.ebFileFormatIdCtrl.value];

    } else {
      fileFormat = this.ebFormatsResult.eBFileFormats[this.selectData.ebFormatId];
    }

    // フィールド設定の初期化
    switch (fileFormat.fileFieldTypes){
      case 1:
      {
        this.fileFiledTypeNames.push(FILE_FIELD_TYPE_NAMES[0]);
        break;
      }
      case 2:
      {
        this.fileFiledTypeNames.push(FILE_FIELD_TYPE_NAMES[1]);
        break;
      }
      case 3:
      {
        this.fileFiledTypeNames.push(FILE_FIELD_TYPE_NAMES[0]);
        this.fileFiledTypeNames.push(FILE_FIELD_TYPE_NAMES[1]);
        break;
      }
      case 4:
      {
        this.fileFiledTypeNames.push(FILE_FIELD_TYPE_NAMES[2]);
        break;
      }
      case 5:
      {
        this.fileFiledTypeNames.push(FILE_FIELD_TYPE_NAMES[0]);
        this.fileFiledTypeNames.push(FILE_FIELD_TYPE_NAMES[2]);
        break;
      }
      case 6:
      {
        this.fileFiledTypeNames.push(FILE_FIELD_TYPE_NAMES[1]);
        this.fileFiledTypeNames.push(FILE_FIELD_TYPE_NAMES[2]);
        break;
      }
      case 7:
      {
        this.fileFiledTypeNames.push(FILE_FIELD_TYPE_NAMES[0]);
        this.fileFiledTypeNames.push(FILE_FIELD_TYPE_NAMES[1]);
        this.fileFiledTypeNames.push(FILE_FIELD_TYPE_NAMES[2]);
        break;
      }
      case 8:
      {
        this.fileFiledTypeNames.push(FILE_FIELD_TYPE_NAMES[3]);
        break;
      }
      case 9:
      {
        this.fileFiledTypeNames.push(FILE_FIELD_TYPE_NAMES[0]);
        this.fileFiledTypeNames.push(FILE_FIELD_TYPE_NAMES[3]);
        break;
      }
      case 10:
      {
        this.fileFiledTypeNames.push(FILE_FIELD_TYPE_NAMES[1]);
        this.fileFiledTypeNames.push(FILE_FIELD_TYPE_NAMES[3]);
        break;
      }
      case 11:
      {
        this.fileFiledTypeNames.push(FILE_FIELD_TYPE_NAMES[0]);
        this.fileFiledTypeNames.push(FILE_FIELD_TYPE_NAMES[1]);
        this.fileFiledTypeNames.push(FILE_FIELD_TYPE_NAMES[3]);
        break;
      }
      case 12:
      {
        this.fileFiledTypeNames.push(FILE_FIELD_TYPE_NAMES[2]);
        this.fileFiledTypeNames.push(FILE_FIELD_TYPE_NAMES[3]);
        break;
      }
      case 13:
      {
        this.fileFiledTypeNames.push(FILE_FIELD_TYPE_NAMES[0]);
        this.fileFiledTypeNames.push(FILE_FIELD_TYPE_NAMES[2]);
        this.fileFiledTypeNames.push(FILE_FIELD_TYPE_NAMES[3]);
        break;
      }
      case 14:
      {
        this.fileFiledTypeNames.push(FILE_FIELD_TYPE_NAMES[1]);
        this.fileFiledTypeNames.push(FILE_FIELD_TYPE_NAMES[2]);
        this.fileFiledTypeNames.push(FILE_FIELD_TYPE_NAMES[3]);
        break;
      }
      case 15:
      {
        this.fileFiledTypeNames.push(FILE_FIELD_TYPE_NAMES[0]);
        this.fileFiledTypeNames.push(FILE_FIELD_TYPE_NAMES[1]);
        this.fileFiledTypeNames.push(FILE_FIELD_TYPE_NAMES[2]);
        this.fileFiledTypeNames.push(FILE_FIELD_TYPE_NAMES[3]);
        break;
      }
      default:
      {
        break;
      }
    }

    // 銀行
    if(fileFormat.requireBankCode==1){
      this.bankCodeCtrl.enable();
      this.bankCodeCtrl.setValidators([Validators.required,Validators.maxLength(4)]);
    }
    else{
      this.bankCodeCtrl.disable();
      this.bankCodeCtrl.setValidators([Validators.maxLength(4)]);
    }

    // 取込区分
    if(!StringUtil.IsNullOrEmpty(fileFormat.importableValues)){
      this.importableValueCtrl.enable();
      this.importableValueCtrl.setValue(fileFormat.importableValues);
    }
    else{
      this.importableValueCtrl.disable();
      this.importableValueCtrl.setValue("");
    }

    if(fileFormat.isDateSelectable==1){
      this.isDateSelectable =true;
    }
    else{
      this.isDateSelectable =false;
    }

  }

  public setfileFieldType(){
    let name = this.ebFormatsResult.eBFileFormats[this.ebFileFormatIdCtrl.value].name;

    let name2:string = "";
    FILE_FIELD_TYPE_NAMES.forEach(element=>{
      if(element.id==this.fileFieldTypeCtrl.value){
        name2 = element.val;
      }
    });

    if(!StringUtil.IsNullOrEmpty(name2)){
      this.fileSettingNameCtrl.setValue(name + "(" + name2 + ")");
    }
    else{
      this.fileSettingNameCtrl.setValue("");
    }

  }

  public setBankCode() {
    if (!StringUtil.IsNullOrEmpty(this.bankCodeCtrl.value)) {
      this.bankCodeCtrl.setValue(StringUtil.setPaddingFrontZero(this.bankCodeCtrl.value, 4, true));
    }
  }

  public setSelectDatatoForm() {
    this.ebFileFormatIdCtrl.setValue(this.selectData.ebFormatId);  // EBフォーマット
    this.fileFieldTypeCtrl.setValue(this.selectData.fileFieldType); // 区切り文字
    this.fileSettingNameCtrl.setValue(this.selectData.name); // EBファイル設定名
    this.bankCodeCtrl.setValue(this.selectData.bankCode); // 銀行コード
    this.importableValueCtrl.setValue(this.selectData.importableValues); // 取込区分
    this.displayOrderCtrl.setValue(this.selectData.displayOrder); // 表示順
    this.setEBFileFormatId();
  }
}
