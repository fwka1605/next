import {BUTTON_ACTION} from '../../../../common/const/event.const';
import { ModalEbExcludeAccountingSettingComponent } from '../../../modal/modal-eb-exclude-accounting-setting/modal-eb-exclude-accounting-setting.component';
import { Component, OnInit, ElementRef, ComponentFactoryResolver, ViewContainerRef, EventEmitter, AfterViewInit } from '@angular/core';
import { EBFileSettingsResult } from 'src/app/model/eb-file-settings-result.model';
import { BaseComponent } from 'src/app/component/common/base/base-component';
import { Router, NavigationEnd, ActivatedRoute } from '@angular/router';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { StringUtil } from 'src/app/common/util/string-util';
import { ModalEbFileSettingComponent } from 'src/app/component/modal/modal-eb-file-setting/modal-eb-file-setting.component';
import { ImportFileLogService } from 'src/app/service/import-file-log.service';
import { ImportFileLogsResult } from 'src/app/model/import-file-logs-result.model';
import { FormatterUtil } from 'src/app/common/util/formatter.util';
import { UserInfoService } from 'src/app/service/common/user-info.service';
import { forkJoin } from 'rxjs';
import { EBFileSettingMasterService } from 'src/app/service/Master/ebfile-setting-master.service';
import { EBFileSetting } from 'src/app/model/eb-file-setting.model';
import { EbFileInformation } from 'src/app/model/eb-file-information.model';
import { ENCODE, ImportResult } from 'src/app/common/const/eb-file.const';
import { ModalRouterProgressComponent } from 'src/app/component/modal/modal-router-progress/modal-router-progress.component';
import { ModalConfirmComponent } from 'src/app/component/modal/modal-confirm/modal-confirm.component';
import { MODAL_STATUS_TYPE, PROCESS_RESULT_RESULT_TYPE } from 'src/app/common/const/status.const';
import { HtmlUtil } from 'src/app/common/util/html.util';
import { EVENT_TYPE } from 'src/app/common/const/event.const';
import { ProcessResultService } from 'src/app/service/common/process-result.service';
import { MSG_WNG, MSG_INF, MSG_ERR, MSG_ITEM_NUM } from 'src/app/common/const/message.const';
import { PageUtil } from 'src/app/common/util/page-util';
import { LocalStorageItem } from 'src/app/model/custom-model/local-storage-item';
import { RangeSearchKey } from 'src/app/common/const/local-storage-key-const';
import { LocalStorageManageService } from 'src/app/service/common/local-storage-manage.service';

@Component({
  selector: 'app-pd0101-eb-file-importer',
  templateUrl: './pd0101-eb-file-importer.component.html',
  styleUrls: ['./pd0101-eb-file-importer.component.css']
})
export class Pd0101EbFileImporterComponent extends BaseComponent implements OnInit,AfterViewInit {

  // ファイル読込処理用のReader
  public readers:Array<FileReader> = new Array<FileReader>();


  public ebFileSettingsResult: EBFileSettingsResult;
  public importFileLogsResult: ImportFileLogsResult

  public ebFileInfos: EbFileInfo[] = new Array<EbFileInfo>();

  public ebFileSettingCtrl: FormControl;  // EBファイル設定
  public ebFileYearCtlr: FormControl; // 入金年

  public importFileLogFlagCtrls:FormControl[];// 取込履歴チェック

  public importFlagCtrls:FormControl[];
  public ebFileSettingCtrls:FormControl[];


  public undefineCtrl: FormControl;

  public importFinishedFlag: boolean;

  public onFileDrop: EventEmitter<File[]> = new EventEmitter<File[]>();
  
  constructor(
    public router: Router,
    public activatedRoute: ActivatedRoute,
    public elementRef: ElementRef,
    public componentFactoryResolver: ComponentFactoryResolver,
    public viewContainerRef: ViewContainerRef,
    public userInfoService: UserInfoService,
    public ebFileSettingService: EBFileSettingMasterService,
    public importFileLogService: ImportFileLogService,
    public processResultService:ProcessResultService,
    public localStorageManageService:LocalStorageManageService
  ) {
    super();

    router.events.subscribe(event => {
      if (event instanceof NavigationEnd) {
        this.Title = PageUtil.GetTitle(router.routerState, router.routerState.root).join('');
        this.ComponentId = parseInt(PageUtil.GetComponentId(router.routerState, router.routerState.root).join(''));
        const path:string[] = PageUtil.GetPath(this.activatedRoute.snapshot.pathFromRoot);
        this.Path = path[1];
        this.processCustomResult = this.processResultService.processResultInit(this.ComponentId,this.Title);
        this.processModalCustomResult = this.processResultService.processResultInit(this.ComponentId,this.Title,true);
      }
    });    
  }

  ngOnInit() {

    this.setControlInit();
    this.setValidator();
    this.setFormatter();
    this.clear();

    let importFileLogsResponse = this.importFileLogService.GetHistory();
    let ebFileSettingsResponse = this.ebFileSettingService.GetItems();

    forkJoin(
      importFileLogsResponse,
      ebFileSettingsResponse
    )
      .subscribe(responseList=>{
        if(
              responseList!=undefined
          &&  responseList.length==2
          &&  responseList[0]!=PROCESS_RESULT_RESULT_TYPE.FAILURE
          &&  responseList[1]!=PROCESS_RESULT_RESULT_TYPE.FAILURE

        ){
          this.importFileLogsResult = new ImportFileLogsResult();
          this.ebFileSettingsResult = new EBFileSettingsResult();

          this.importFileLogsResult.importFileLogs = responseList[0];
          this.ebFileSettingsResult.eBFileSettings = responseList[1].filter((setting: { isUseable: number; }) => {return setting.isUseable == 1});

          this.importFileLogFlagCtrls = new Array<FormControl>(this.importFileLogsResult.importFileLogs.length);

          for(let index=0;index<this.importFileLogsResult.importFileLogs.length;index++){
            this.importFileLogFlagCtrls[index] = new FormControl("");
            this.MyFormGroup.removeControl("importFileLogFlagCtrl"+index);
            this.MyFormGroup.addControl("importFileLogFlagCtrl"+index,this.importFileLogFlagCtrls[index]);
          }
        }
        else{

        }
      });
  }

  ngAfterViewInit(){
    HtmlUtil.nextFocusByName(this.elementRef, 'ebFileSettingCtrl', EVENT_TYPE.NONE);
  }

  public setControlInit() {
    this.ebFileSettingCtrl = new FormControl("",[Validators.required]);
    this.ebFileYearCtlr = new FormControl("",[Validators.required]);


    this.undefineCtrl = new FormControl("");
  }

  public setValidator() {
    this.MyFormGroup = new FormGroup({
      ebFileSettingCtrl: this.ebFileSettingCtrl,
      ebFileYearCtlr: this.ebFileYearCtlr,

      UndefineCtrl: this.undefineCtrl,
    });
  }

  public setFormatter(){
    FormatterUtil.setNumberFormatter(this.ebFileYearCtlr);
  }

  public clear() {
    this.MyFormGroup.reset();
    let tmp = new Date();
    this.ebFileYearCtlr.disable();
    this.ebFileYearCtlr.setValue(tmp.getFullYear());

    this.ebFileInfos = new Array<EbFileInfo>();

    this.importFlagCtrls = new Array<FormControl>();
    this.ebFileSettingCtrls = new Array<FormControl>();

    this.importFinishedFlag = false;

    this.initEBFileSetting();

    HtmlUtil.nextFocusByName(this.elementRef, 'ebFileSettingCtrl', EVENT_TYPE.NONE);


  }



  public initEBFileSetting() {
    let ebFileSetting = this.localStorageManageService.get(RangeSearchKey.PD0101_SELECTED_EB_FILE);

    if (ebFileSetting != null) {
      this.ebFileSettingCtrl.setValue(ebFileSetting.value);
    }

  }  

  public get isSelectedLog():boolean{
    let bRtn:boolean;

    if(this.importFileLogFlagCtrls !=null)
    this.importFileLogFlagCtrls.forEach(element => {
      if(element.value){
        bRtn = true;
      }
    });
    
    return bRtn;
  }


  /**
   * ボタン操作によるメソッド呼び出し
   * @param action ボタン操作
   */
  public buttonActionInner(action: BUTTON_ACTION) {
    this.processResultService.processResultStart(this.processCustomResult, action);
    if (this.processCustomResult.result == PROCESS_RESULT_RESULT_TYPE.FAILURE) {
      return;
    }

    switch (action) {

      case BUTTON_ACTION.DELETE:
        this.delete();
        break;

      case BUTTON_ACTION.IMPORT:
        this.import();
        break;

      default:
        console.log('buttonAction Error.');
        break;
    }
    
  }



  public import() {


    if (this.ebFileInfos.length == 0) {
      this.processCustomResult = this.processResultService.processAtWarning(
        this.processCustomResult,MSG_WNG.INPUT_REQUIRED.replace(MSG_ITEM_NUM.FIRST,"取込むEBファイル"),
        this.partsResultMessageComponent);
      return;
    }

    let importEbFileInfos = new Array<EbFileInfo>();

    for(let index=0;index<this.ebFileInfos.length;index++){
      if (this.importFlagCtrls[index].value == true) {
        importEbFileInfos.push(this.ebFileInfos[index]);
      }
    }

    if (importEbFileInfos.length == 0) {
      this.processCustomResult = this.processResultService.processAtWarning(
        this.processCustomResult,MSG_WNG.INPUT_REQUIRED.replace(MSG_ITEM_NUM.FIRST,"取込むEBファイル"),
        this.partsResultMessageComponent);
      return;
    }


    // 削除処理
    let confirmcCmponentFactory = this.componentFactoryResolver.resolveComponentFactory(ModalConfirmComponent);
    let confirmcComponentRef = this.viewContainerRef.createComponent(confirmcCmponentFactory);
    confirmcComponentRef.instance.ActionName = "取込"
    confirmcComponentRef.instance.Closing.subscribe(() => {

      if (confirmcComponentRef.instance.ModalStatus == MODAL_STATUS_TYPE.OK) {

        confirmcComponentRef.destroy();

        let progressComponentFactory = this.componentFactoryResolver.resolveComponentFactory(ModalRouterProgressComponent);
        let progressComponentRef = this.viewContainerRef.createComponent(progressComponentFactory);
        progressComponentRef.instance.Closing.subscribe(() => {
          progressComponentRef.destroy();
        });
    
        // EBファイルの読み込み
        let ebFileInformations = new Array<EbFileInformation>();
        
        for(let index=0;index<importEbFileInfos.length;index++){
          let ebFileInformation = new EbFileInformation();
    
          let ebFileSetting = this.ebFileSettingsResult.eBFileSettings[this.ebFileSettingCtrls[index].value];
    
          ebFileInformation.file = importEbFileInfos[index].data;  // <- EBデータの文字列を指定してください
          ebFileInformation.filePath = importEbFileInfos[index].filePath;  // <- EBデータの文字列を指定してください
          //ebFileInformation.size = importEbFileInfos[index].size;  // <- EBデータのファイルサイズを指定してください
          ebFileInformation.format = ebFileSetting.ebFormatId; // <- EBFileSetting.EBFormatId(EbFormat.Id) を指定してください
          ebFileInformation.fileFieldType = ebFileSetting.fileFieldType; //<- EBFileSetting.FileFieldType を指定してください
          ebFileInformation.bankCode = ebFileSetting.bankCode;//<- EBFileSetting.BankCode を指定してください (空文字の場合もあります)
          ebFileInformation.importableValue = ebFileSetting.importableValues;// <- EBFileSetting.ImportableValues を指定してください
          ebFileInformation.useValueDate = ebFileSetting.useValueDate==1?true:false; //<- EBFileSetting.UseValueDate == 1 で true それ以外は false を指定してください
          ebFileInformation.index=index;// <- 複数ファイルの index を指定してください
          ebFileInformation.companyId = this.userInfoService.Company.id; //<- 会社ID を指定してください
          ebFileInformation.loginUserId = this.userInfoService.LoginUser.id; //<- 操作を行っているログインユーザーID を指定してください
          //Result 戻り値 Rac.VOne.EbData.Core / ImportResult の enum が int で返ります
          //BankInformation 戻り値 BankAccount が不足している場合に、不足している口座情報が返ります
          ebFileInformation.year = this.ebFileYearCtlr.value; //<- 入金年を指定してください（空文字不可）
    
          ebFileInformations.push(ebFileInformation);
    
        };
    
        let rtnEbFileInformations:Array<EbFileInformation>;
        this.importFileLogService.Import(ebFileInformations)
          .subscribe(response=>{

            if(response!=PROCESS_RESULT_RESULT_TYPE.FAILURE){

              this.importFinishedFlag = true;
              rtnEbFileInformations = response;
    
              for(let index=0;index<rtnEbFileInformations.length;index++){
                this.ebFileInfos[index].filePath=rtnEbFileInformations[index].filePath;
                this.ebFileInfos[index].importCount=rtnEbFileInformations[index].importFileLog.importCount;
                this.ebFileInfos[index].readCount=rtnEbFileInformations[index].importFileLog.readCount;
      
                if (rtnEbFileInformations[index].result == ImportResult.None) {
                  this.processCustomResult.result = PROCESS_RESULT_RESULT_TYPE.SUCCESS;
                  this.ebFileInfos[index].status = MSG_INF.FINISH_IMPORT;
                }
                if (rtnEbFileInformations[index].result == ImportResult.FileNotFound) {
                  this.processCustomResult.result = PROCESS_RESULT_RESULT_TYPE.WARNING;
                  this.ebFileInfos[index].status = MSG_WNG.OPEN_FILE_NOT_FOUND
                }
                if (rtnEbFileInformations[index].result == ImportResult.FileReadError) {
                  this.processCustomResult.result = PROCESS_RESULT_RESULT_TYPE.FAILURE;
                  this.ebFileInfos[index].status = MSG_ERR.READING_ERROR;
                }
                if (rtnEbFileInformations[index].result == ImportResult.FileFormatError) {
                  this.processCustomResult.result = PROCESS_RESULT_RESULT_TYPE.FAILURE;
                  this.ebFileInfos[index].status = MSG_ERR.READING_ERROR;
                }
                if (rtnEbFileInformations[index].result == ImportResult.BankAccountMasterError) {
                  this.processCustomResult.result = PROCESS_RESULT_RESULT_TYPE.WARNING;
                  this.ebFileInfos[index].status = MSG_WNG.NOT_EXIST_BANK_ACCOUNT_MASTER;
                }
                if (rtnEbFileInformations[index].result == ImportResult.ImportDataNotFound) {
                  this.processCustomResult.result = PROCESS_RESULT_RESULT_TYPE.WARNING;
                  this.ebFileInfos[index].status = MSG_WNG.NO_RECEIPT_DATA;
                }
                if (rtnEbFileInformations[index].result == ImportResult.DBError) {
                  this.processCustomResult.result = PROCESS_RESULT_RESULT_TYPE.FAILURE;
                  this.ebFileInfos[index].status = MSG_ERR.IMPORT_ERROR_WITHOUT_LOG;
                }
                if (rtnEbFileInformations[index].result == ImportResult.BankAccountFormatError) {
                  this.processCustomResult.result = PROCESS_RESULT_RESULT_TYPE.FAILURE;
                  this.ebFileInfos[index].status = MSG_ERR.BANK_ACCOUNT_FORMAT_ERROR;
                }
                if (rtnEbFileInformations[index].result == ImportResult.PayerCodeFormatError) {
                  this.processCustomResult.result = PROCESS_RESULT_RESULT_TYPE.FAILURE;
                  this.ebFileInfos[index].status = MSG_ERR.PAYER_CODE_FORMAT_ERROR
                }
                if (rtnEbFileInformations[index].result == ImportResult.Success) {
                  this.processCustomResult.result = PROCESS_RESULT_RESULT_TYPE.SUCCESS;
                  this.ebFileInfos[index].status = MSG_INF.FINISH_IMPORT;
                }

                if (rtnEbFileInformations[index].result != ImportResult.None && rtnEbFileInformations[index].result != ImportResult.Success) {
                  this.importFinishedFlag = false;
                }

                let importFileLogsResponse = this.importFileLogService.GetHistory()
                  .subscribe(response=>{
                    
                    if(response!=PROCESS_RESULT_RESULT_TYPE.FAILURE){
  
                      this.importFileLogsResult.importFileLogs = response;
            
                      this.importFileLogFlagCtrls = new Array<FormControl>(this.importFileLogsResult.importFileLogs.length);
            
                      for(let index=0;index<this.importFileLogsResult.importFileLogs.length;index++){
                        this.importFileLogFlagCtrls[index] = new FormControl("");
                        this.MyFormGroup.removeControl("importFileLogFlagCtrl"+index);
                        this.MyFormGroup.addControl("importFileLogFlagCtrl"+index,this.importFileLogFlagCtrls[index]);
                      }
                      this.processCustomResult = this.processResultService.processAtSuccess(
                        this.processCustomResult, MSG_INF.PROCESS_FINISH, this.partsResultMessageComponent);
                      this.processResultService.createdLog(this.processCustomResult.logData);
                    }
                    else{
                      this.processCustomResult = this.processResultService.processAtFailure(
                        this.processCustomResult,MSG_ERR.SOMETHING_ERROR.replace(MSG_ITEM_NUM.FIRST,"取込処理"),
                        this.partsResultMessageComponent);
                    }
                    progressComponentRef.destroy();
                  });
              }
            }
            else{
              this.processCustomResult = this.processResultService.processAtFailure(
                this.processCustomResult,MSG_ERR.SOMETHING_ERROR.replace(MSG_ITEM_NUM.FIRST,"取込処理"),
                this.partsResultMessageComponent);
            }
          });
      }
      else{
        this.processCustomResult = this.processResultService.processAtWarning(
          this.processCustomResult, MSG_WNG.CANCEL_PROCESS.replace(MSG_ITEM_NUM.FIRST,"取込処理"),
          this.partsResultMessageComponent);

        confirmcComponentRef.destroy();
      }
    });


  }

  public onDragOver(event:any){
    event.preventDefault();
  }
  



  public onDrop(event:any){

    
    this.processResultService.clearProcessCustomMsg(this.processCustomResult);
    
    event.preventDefault();

    let fileCount = event.dataTransfer.files.length;

    this.readers=new Array<FileReader>();
    if (StringUtil.IsNullOrEmpty(this.ebFileSettingCtrl.value)) {

      this.processCustomResult = this.processResultService.processAtWarning(
        this.processCustomResult,MSG_WNG.INPUT_REQUIRED.replace(MSG_ITEM_NUM.FIRST,"標準EBファイル設定"),
        this.partsResultMessageComponent);
      return;
    }    

    let start=this.ebFileInfos.length;
    for(let index=0; index<fileCount;index++) {
      

      this.importFlagCtrls.push(new FormControl(true));
      this.MyFormGroup.removeControl("importFlagCtrl" + (start+index));
      this.MyFormGroup.addControl("importFlagCtrl" + (start+index), this.importFlagCtrls[start+index]);

      this.ebFileSettingCtrls.push(new FormControl(this.ebFileSettingCtrl.value));
      this.MyFormGroup.removeControl("ebFileSettingCtrl" + (start+index));
      this.MyFormGroup.addControl("ebFileSettingCtrl" + (start+index), this.ebFileSettingCtrls[start+index]);

      let ebFileInfoTmp = new EbFileInfo();

      ebFileInfoTmp.ebFileSettingName = this.ebFileSettingsResult.eBFileSettings[this.ebFileSettingCtrl.value].name + "(" + this.ebFileSettingsResult.eBFileSettings[this.ebFileSettingCtrl.value].fileFieldType + ")";
      ebFileInfoTmp.filePath = event.dataTransfer.files[index].name;
      ebFileInfoTmp.size = event.dataTransfer.files[index].size;


      ebFileInfoTmp.status = "未取込";

      this.ebFileInfos.push(ebFileInfoTmp);

      // CSV読込
      this.readers[index] = new FileReader();
      this.readers[index].readAsText(event.dataTransfer.files[index],ENCODE[1].val);

      new Promise((resolve, reject) => {
        this.readers[index].onload = () => {

          if(this.readers.length==fileCount){
            let ebFileInfoTmps = this.ebFileInfos;
            this.readers.forEach(
              function(element,index,readers){
                if(element.readyState==2){
                  let ebFileInfoTmp = ebFileInfoTmps[start+index];
                  ebFileInfoTmp.data = readers[index].result.toString(); 
                  ebFileInfoTmp.readCount = 0;
                  ebFileInfoTmp.importCount = 0;


                }
              } 
            );
  
          }

        }
      });
    }

    this.setEBFileSetting();
  }


  
  public openEbFileSettingModal() {
    let componentFactory = this.componentFactoryResolver.resolveComponentFactory(ModalEbFileSettingComponent);
    let componentRef = this.viewContainerRef.createComponent(componentFactory);
    componentRef.instance.processModalCustomResult = this.processModalCustomResult;

    componentRef.instance.Closing.subscribe(() => {

      let ebFileSettingsResponse = this.ebFileSettingService.GetItems()
        .subscribe(response=>{
          if(response!=PROCESS_RESULT_RESULT_TYPE.FAILURE){
            this.ebFileSettingsResult = new EBFileSettingsResult();
            this.ebFileSettingsResult.eBFileSettings = response.filter((setting: { isUseable: number; }) => {return setting.isUseable == 1});
          }
          else{
            this.ebFileSettingsResult = new EBFileSettingsResult();
            this.ebFileSettingsResult.eBFileSettings = new Array<EBFileSetting>();
  
          }
      
        });
  

      componentRef.destroy();
    });
  }


  public openEbExcludeAccountingSettingModal() {
    let componentFactory = this.componentFactoryResolver.resolveComponentFactory(ModalEbExcludeAccountingSettingComponent);
    let componentRef = this.viewContainerRef.createComponent(componentFactory);
    componentRef.instance.processModalCustomResult = this.processModalCustomResult;

    componentRef.instance.Closing.subscribe(() => {
      componentRef.destroy();
    });
  }


  ///////////////////////////////////////////////////////
  public setEBFileSetting(){
    let localstorageItem = new LocalStorageItem();
    localstorageItem.key = RangeSearchKey.PD0101_SELECTED_EB_FILE;
    localstorageItem.value = this.ebFileSettingCtrl.value;
    this.localStorageManageService.set(localstorageItem);

  }


  ///////////////////////////////////////////////////////
  public setTableEBFileSetting(){
    for (let index = 0; index < this.ebFileSettingCtrls.length; index++) {
      let setting = this.ebFileSettingsResult.eBFileSettings[this.ebFileSettingCtrls[index].value];
      if (setting.requireYear == 1) {
        this.ebFileYearCtlr.enable();
        return;
      }
    }

    this.ebFileYearCtlr.disable();
  }

  public delete(){

    let deleteLogs = new Array<number>();

    for(let index=0;index<this.importFileLogFlagCtrls.length;index++){
      if(this.importFileLogFlagCtrls[index].value){
        deleteLogs.push(this.importFileLogsResult.importFileLogs[index].id);
      }      
    }

    if(deleteLogs.length<=0){

      this.processCustomResult = this.processResultService.processAtWarning(
        this.processCustomResult,MSG_WNG.SELECTION_REQUIRED.replace(MSG_ITEM_NUM.FIRST,"削除する取込履歴"),
        this.partsResultMessageComponent);
      return;
    }

    // 動作中のコンポーネントを開く
    let componentFactory = this.componentFactoryResolver.resolveComponentFactory(ModalRouterProgressComponent);
    let modalRouterProgressComponentRef = this.viewContainerRef.createComponent(componentFactory);
    modalRouterProgressComponentRef.instance.Closing.subscribe(() => {
      modalRouterProgressComponentRef.destroy();
    });

    this.importFileLogService.DeleteItems(deleteLogs)
      .subscribe(response=>{
        this.processCustomResult = this.processResultService.processAtDelete(
          this.processCustomResult, response, this.partsResultMessageComponent);
        this.importFileLogService.GetHistory()
          .subscribe(response=>{
            this.importFileLogsResult.importFileLogs = response;

            this.importFileLogFlagCtrls = new Array<FormControl>(this.importFileLogsResult.importFileLogs.length);
  
            for(let index=0;index<this.importFileLogsResult.importFileLogs.length;index++){
              this.importFileLogFlagCtrls[index] = new FormControl("");
              this.MyFormGroup.removeControl("importFileLogFlagCtrl"+index);
              this.MyFormGroup.addControl("importFileLogFlagCtrl"+index,this.importFileLogFlagCtrls[index]);
            }

            modalRouterProgressComponentRef.destroy();
  
          })
      })
    

  }
}

class EbFileInfo {


  public filePath: string = "";

  public ebFileSettingName: string = "";

  public readCount: number = 0;

  public importCount: number = 0;

  public status: string = "";

  public data:string = "";

  public size:number;


}

