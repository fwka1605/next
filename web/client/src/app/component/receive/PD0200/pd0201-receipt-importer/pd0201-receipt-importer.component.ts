import { ModalMasterComponent } from 'src/app/component/modal/modal-master/modal-master.component';


import { Component, OnInit, ViewContainerRef, ComponentFactoryResolver, ElementRef, ComponentRef, AfterViewInit, ViewChild } from '@angular/core';
import { FormControl, Validators, FormGroup } from '@angular/forms';
import { BaseComponent } from 'src/app/component/common/base/base-component';
import { ModalImporterSettingComponent } from 'src/app/component/modal/modal-importer-setting/modal-importer-setting.component';
import { StringUtil } from 'src/app/common/util/string-util';
import { KEY_CODE, EVENT_TYPE, BUTTON_ACTION } from 'src/app/common/const/event.const';
import { FormatterUtil } from 'src/app/common/util/formatter.util';
import { TABLE_INDEX } from 'src/app/common/const/table-name.const';
import { MODAL_STATUS_TYPE, PROCESS_RESULT_RESULT_TYPE } from 'src/app/common/const/status.const';
import { UserInfoService } from 'src/app/service/common/user-info.service';
import { FreeImporterFormatType } from 'src/app/common/common-const';
import { ImporterSettingService } from 'src/app/service/Master/importer-setting.service';
import { HtmlUtil } from 'src/app/common/util/html.util';
import { ColumnNameSettingsResult } from 'src/app/model/column-name-settings-result.model';
import { CollationSettingMasterService } from 'src/app/service/Master/collation-setting-master.service';
import { ModalRouterProgressComponent } from 'src/app/component/modal/modal-router-progress/modal-router-progress.component';
import { TransactionImportSource } from 'src/app/model/transaction-import-source.model';
import { ReceiptService } from 'src/app/service/receipt.service';
import { ImportDataResult } from 'src/app/model/import-data-result.model';
import { DateUtil } from 'src/app/common/util/date-util';
import { FILE_EXTENSION, ENCODE, MAX_FILE_SIZE } from 'src/app/common/const/eb-file.const';
import { FileUtil } from 'src/app/common/util/file.util';
import { LINE_FEED_CODE } from 'src/app/common/const/report-setting-kbn.const';
import { ModalWarningFileSizeComponent } from 'src/app/component/modal/modal-warning-file-size/modal-warning-file-size.component';
import { NavigationEnd, Router, ActivatedRoute } from '@angular/router';
import { PageUtil } from 'src/app/common/util/page-util';
import { ProcessResultService } from 'src/app/service/common/process-result.service';
import { MSG_ERR, MSG_INF, MSG_ITEM_NUM, MSG_WNG } from 'src/app/common/const/message.const';
import { CategoryType } from 'src/app/common/const/kbn.const';
import { MatAutocompleteTrigger } from '@angular/material';

@Component({
  selector: 'app-pd0201-receipt-importer',
  templateUrl: './pd0201-receipt-importer.component.html',
  styleUrls: ['./pd0201-receipt-importer.component.css']
})
export class Pd0201ReceiptImporterComponent extends BaseComponent implements OnInit,AfterViewInit {

  public ColumnNameSettingsResult: ColumnNameSettingsResult;

  public importDataResult: ImportDataResult;

  public modalRouterProgressComponentRef: ComponentRef<ModalRouterProgressComponent>;

  public importFile: any; // インポートファイルのイベント

  public patternNoCtrl: FormControl;
  public patternNameCtrl: FormControl;
  public patternId: number;
  public importFileCtrl: FormControl;
  public importFileNameCtrl: FormControl;
  public printDataCtrl: FormControl;

  public readCount: number;
  public validCount: number;
  public invalidCount: number;
  public saveCount: number;
  public saveAmount: number;

  public newCustomerCount:number;

  public UndefineCtrl: FormControl;

  @ViewChild('patternNoInput', { read: MatAutocompleteTrigger }) patternNoTrigger: MatAutocompleteTrigger;

  constructor(
    public router: Router,
    public activatedRoute: ActivatedRoute,
    public elementRef: ElementRef,
    public componentFactoryResolver: ComponentFactoryResolver,
    public viewContainerRef: ViewContainerRef,
    public importerSettingService: ImporterSettingService,
    public userInfoService: UserInfoService,
    public collationSettingService: CollationSettingMasterService,
    public receiptService: ReceiptService,
    public processResultService: ProcessResultService
  ) {
    super();

    router.events.subscribe(event => {
      if (event instanceof NavigationEnd) {
        this.Title = PageUtil.GetTitle(router.routerState, router.routerState.root).join('');
        this.ComponentId = parseInt(PageUtil.GetComponentId(router.routerState, router.routerState.root).join(''));
        const path: string[] = PageUtil.GetPath(this.activatedRoute.snapshot.pathFromRoot);
        this.Path = path[1];
        this.processCustomResult = this.processResultService.processResultInit(this.ComponentId, this.Title);
        this.processModalCustomResult = this.processResultService.processResultInit(this.ComponentId,this.Title,true);
      }
    });

  }

  ngOnInit() {
    this.setControlInit();
    this.setValidator();
    this.setFormatter();
    this.clear();
    this.setAutoComplete();

    this.collationSettingService.Get()
      .subscribe(response => {
        this.ColumnNameSettingsResult = new ColumnNameSettingsResult();
        this.ColumnNameSettingsResult.columnNames = response;
      });
  }

  ngAfterViewInit(){
    //オートコンプリートが動くためコメントアウト。
    //HtmlUtil.nextFocusByName(this.elementRef, "patternNoCtrl", EVENT_TYPE.NONE);
  }


  public setControlInit() {
    this.patternNoCtrl = new FormControl("", [Validators.required, Validators.maxLength(2)]);
    this.patternNameCtrl = new FormControl("");
    this.importFileCtrl = new FormControl("");
    this.importFileNameCtrl = new FormControl("", [Validators.required]);
    this.printDataCtrl = new FormControl("", [Validators.required]);
    this.UndefineCtrl = new FormControl("");
  }

  public setValidator() {
    this.MyFormGroup = new FormGroup({
      patternNoCtrl: this.patternNoCtrl,
      patternNameCtrl: this.patternNameCtrl,
      importFileCtrl: this.importFileCtrl,
      importFileNameCtrl: this.importFileNameCtrl,
      printDataCtrl: this.printDataCtrl,
      UndefineCtrl: this.UndefineCtrl,
    });
  }

  public setFormatter() {
    FormatterUtil.setNumberFormatter(this.patternNoCtrl);
  }

  public clear() {
    this.MyFormGroup.reset();

    this.processCustomResult = this.processResultService.clearProcessCustomMsg(this.processCustomResult);;

    this.patternId = 0;

    // 初期化処理
    this.patternNoCtrl.enable();
    this.printDataCtrl.setValue("1");
    this.importFileNameCtrl.setValue("");

    this.importFile = null;
    this.importDataResult = null;
    this.readCount = null;
    this.validCount = null;
    this.invalidCount = null;
    this.saveCount = null;
    this.saveAmount = null;

    //オートコンプリートが動くためコメントアウト。
    //HtmlUtil.nextFocusByName(this.elementRef, "patternNoCtrl", EVENT_TYPE.NONE);
  }

  public setAutoComplete(){
    // パターンNo
    this.initAutocompleteImporterSetting(FreeImporterFormatType.Receipt,this.patternNoCtrl,this.importerSettingService,0);
  }  
  
  public openImporterSettingModal() {
    let componentFactory = this.componentFactoryResolver.resolveComponentFactory(ModalImporterSettingComponent);
    let componentRef = this.viewContainerRef.createComponent(componentFactory);
    componentRef.instance.Title = "入金";
    componentRef.instance.processModalCustomResult = this.processModalCustomResult;
    componentRef.instance.Closing.subscribe(() => {
      componentRef.destroy();
    });
  }

  public openMasterModal() {

    this.patternNoTrigger.closePanel();

    let componentFactory = this.componentFactoryResolver.resolveComponentFactory(ModalMasterComponent);
    let componentRef = this.viewContainerRef.createComponent(componentFactory);
    componentRef.instance.TableIndex = TABLE_INDEX.MASTER_RECEIPT_IMPORTER_SETTING;
    componentRef.instance.Closing.subscribe(() => {

      if (componentRef.instance.ModalStatus == MODAL_STATUS_TYPE.SELECT) {
        this.patternNoCtrl.disable();
        this.patternNoCtrl.setValue(componentRef.instance.SelectedCode);
        this.patternNameCtrl.setValue(componentRef.instance.SelectedName);
        this.patternId = componentRef.instance.SelectedId;
      }

      componentRef.destroy();
    });
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

      case BUTTON_ACTION.READ:
        this.read();
        break;

      case BUTTON_ACTION.REGISTRY:
        this.registry();
        break;

      default:
        console.log('buttonAction Error.');
        break;
    }

  }

  /**
   * インポート
   */
  public read() {

    this.processCustomResult = this.processResultService.clearProcessCustomMsg(this.processCustomResult);


    // 動作中のコンポーネントを開く
    let componentFactory = this.componentFactoryResolver.resolveComponentFactory(ModalRouterProgressComponent);
    this.modalRouterProgressComponentRef = this.viewContainerRef.createComponent(componentFactory);
    this.modalRouterProgressComponentRef.instance.Closing.subscribe(() => {
      this.modalRouterProgressComponentRef.destroy();
    });

    let reader = new FileReader();

    // CSV読込
    reader.readAsText(this.importFile, ENCODE[1].val);
    new Promise((resolve, reject) => {
      reader.onload = () => {
        let transactionImportSource: TransactionImportSource = new TransactionImportSource();

        transactionImportSource.companyId = this.userInfoService.Company.id;
        transactionImportSource.loginUserId = this.userInfoService.LoginUser.id;
        transactionImportSource.importerSettingId = this.patternId;
        transactionImportSource.encodingCodePage = ENCODE[0].id;
        transactionImportSource.data = btoa(unescape(encodeURIComponent(reader.result.toString())));

        this.receiptService.Read(transactionImportSource)
          .subscribe(
            response => {
              if (response == PROCESS_RESULT_RESULT_TYPE.FAILURE) {
                this.processResultService.processAtFailure(
                  this.processCustomResult,MSG_ERR.SOMETHING_ERROR.replace(MSG_ITEM_NUM.FIRST,"取込"),
                  this.partsResultMessageComponent)
                this.modalRouterProgressComponentRef.destroy();
                return;
              }
              else {
                this.importDataResult = response;
                this.readCount = this.importDataResult.readCount;
                this.validCount = this.importDataResult.validCount;
                this.invalidCount = this.importDataResult.invalidCount;

                if (response.logs != undefined && response.logs.length>0) {
                  this.processResultService.processAtFailure(this.processCustomResult,
                    MSG_ERR.SOMETHING_ERROR.replace(MSG_ITEM_NUM.FIRST,"取込")+" 詳細はログファイルを確認してください。",
                    this.partsResultMessageComponent);
                  this.downloadErrorLog(response.logs);
                }
                else if(response.logs==undefined || response.logs.length==0) {

                  this.processResultService.processAtSuccess(
                    this.processCustomResult, MSG_INF.EB_FILE_IMPORT_FINISH, this.partsResultMessageComponent);
                  this.processResultService.createdLog(this.processCustomResult.logData);

                }
              }

              // 確認モーダルを閉じる
              this.modalRouterProgressComponentRef.destroy();
            }
          )
      };

      reader.onerror = () => {
        this.downloadErrorLog(reader.error);

        // 確認モーダルを閉じる
        this.modalRouterProgressComponentRef.destroy();
      };
    })
  }


  /**
   * 登録
   */
  public registry() {

    this.processCustomResult = this.processResultService.clearProcessCustomMsg(this.processCustomResult);


    // 動作中のコンポーネントを開く
    let componentFactory = this.componentFactoryResolver.resolveComponentFactory(ModalRouterProgressComponent);
    this.modalRouterProgressComponentRef = this.viewContainerRef.createComponent(componentFactory);
    this.modalRouterProgressComponentRef.instance.Closing.subscribe(() => {
      this.modalRouterProgressComponentRef.destroy();
    });

    let reader = new FileReader();

    let transactionImportSource: TransactionImportSource = new TransactionImportSource();

    transactionImportSource.companyId = this.userInfoService.Company.id;
    transactionImportSource.loginUserId = this.userInfoService.LoginUser.id;
    transactionImportSource.importDataId = this.importDataResult.importData.id;

    this.receiptService.Import(transactionImportSource)
      .subscribe(
        response => {
          if (response == 500) {
            this.processCustomResult = this.processResultService.processAtFailure(
              this.processCustomResult, MSG_ERR.SAVE_ERROR, this.partsResultMessageComponent);
            this.modalRouterProgressComponentRef.destroy();
            return;
          }
          else {
            this.importDataResult = response;
            this.saveCount = this.importDataResult.saveCount;
            this.saveAmount = this.importDataResult.saveAmount;

            if (response.logs != undefined && response.logs.length > 0) {
              let msg = MSG_ERR.SAVE_ERROR;
              msg += '詳細はログファイルを確認してください。';
              this.processCustomResult = this.processResultService.processAtFailure(
                this.processCustomResult, msg, this.partsResultMessageComponent);
              this.downloadErrorLog(response.logs);
            }
            else if (response.logs == undefined || response.logs.length == 0) {
              this.processResultService.processAtSuccess(
                this.processCustomResult, MSG_INF.PROCESS_FINISH, this.partsResultMessageComponent);
            }

            this.processResultService.createdLog(this.processCustomResult.logData);
          }
          
          this.processResultService.createdLog(this.processCustomResult.logData)

          // 確認モーダルを閉じる
          this.modalRouterProgressComponentRef.destroy();
        }
      );
  }




  /**
   * 印刷
   */
  public print() {

    this.processCustomResult = this.processResultService.clearProcessCustomMsg(this.processCustomResult);


    // 動作中のコンポーネントを開く
    let componentFactory = this.componentFactoryResolver.resolveComponentFactory(ModalRouterProgressComponent);
    this.modalRouterProgressComponentRef = this.viewContainerRef.createComponent(componentFactory);
    this.modalRouterProgressComponentRef.instance.Closing.subscribe(() => {
      this.modalRouterProgressComponentRef.destroy();
    });

    let reader = new FileReader();

    let transactionImportSource: TransactionImportSource = new TransactionImportSource();

    transactionImportSource.companyId = this.userInfoService.Company.id;
    transactionImportSource.loginUserId = this.userInfoService.LoginUser.id;
    if (this.printDataCtrl.value == "1") {
      transactionImportSource.isValidData = true;
    }
    else {
      transactionImportSource.isValidData = false;
    }
    transactionImportSource.importDataId = this.importDataResult.importData.id;

    this.receiptService.GetImportValidationReport(transactionImportSource)
      .subscribe(response => {
        FileUtil.download([response.body], this.Title + DateUtil.getYYYYMMDD(0), FILE_EXTENSION.PDF);
        this.modalRouterProgressComponentRef.destroy();
      });
  }


  /**
   * エラーログを出力する
   * @param error エラー
   */
  public downloadErrorLog(error: any) {
    let errorMsg = new Array<String>();
    errorMsg.push(DateUtil.getYYYYMMDD(3));
    errorMsg.push("入金データ：" + this.importFileNameCtrl.value);
    errorMsg = errorMsg.concat(error);

    let errorData = new Array<any>();
    errorData.push(errorMsg.join(LINE_FEED_CODE));

    FileUtil.download(errorData, DateUtil.getYYYYMMDD(0) + "_Import", FILE_EXTENSION.LOG);
  }

  ////////////////////////////////////////////////////////////////////////////////////////////////////////

  public setPatternNo(eventType: string) {

    if(eventType!=EVENT_TYPE.BLUR){
      this.patternNoTrigger.closePanel();
    }

    if (!StringUtil.IsNullOrEmpty(this.patternNoCtrl.value)) {
      this.patternNoCtrl.setValue(StringUtil.setPaddingFrontZero(this.patternNoCtrl.value, 2));
      this.loadStart();
      this.importerSettingService.GetHeader(FreeImporterFormatType.Receipt, this.patternNoCtrl.value)
        .subscribe(response => {
          this.loadEnd();
          if (response != PROCESS_RESULT_RESULT_TYPE.FAILURE && response != null && response.length > 0) {
            this.processResultService.clearProcessCustomMsg(this.processCustomResult);
            this.patternNoCtrl.setValue(response[0].code);
            this.patternNoCtrl.disable();
            this.patternNameCtrl.setValue(response[0].name);
            this.patternId = response[0].id
            HtmlUtil.nextFocusByName(this.elementRef, "importFileButtonCtrl", eventType);
          }
          else {
            if (!StringUtil.IsNullOrEmpty(this.patternNoCtrl.value)) {
              let msg = MSG_WNG.NOT_REGIST_PATTERN_NO.replace(MSG_ITEM_NUM.FIRST, this.patternNoCtrl.value);
              this.processCustomResult = this.processResultService.processAtWarning(
                this.processCustomResult, msg, this.partsResultMessageComponent);
              this.patternNoCtrl.setValue("");
            }

            // this.patternNoCtrl.setValue("");
            this.patternNameCtrl.setValue("");
            this.patternId = 0;
          }
        });

    }
    else {
      this.patternNoCtrl.setValue("");
      this.patternNameCtrl.setValue("");
      HtmlUtil.nextFocusByName(this.elementRef, "importFileButtonCtrl", eventType);
    }
  }

  public fileSelect(evt: any) {
    this.processCustomResult = this.processResultService.clearProcessCustomMsg(this.processCustomResult);
    if (evt.target.files.length == 0) return;
    if (evt.target.files[0].size > MAX_FILE_SIZE) {
      let componentFactory = this.componentFactoryResolver.resolveComponentFactory(ModalWarningFileSizeComponent);
      let componentRef = this.viewContainerRef.createComponent(componentFactory);

      componentRef.instance.MainDetail = "ファイルサイズ超過";
      componentRef.instance.SufixDetail = "上限は" + Math.round(MAX_FILE_SIZE / 1024 / 1024) + "MBです。"

      componentRef.instance.Closing.subscribe(() => {
        componentRef.destroy();
      });
      this.importFile = null;
      this.importFileNameCtrl.setValue("");
    }
    else {
      this.importFile = evt.target.files[0];
      this.importFileNameCtrl.setValue(evt.target.files[0].name);
    }
  }

  public onDragOver(event: any) {
    event.preventDefault();
  }

  public onDrop(event: any) {
    this.processCustomResult = this.processResultService.clearProcessCustomMsg(this.processCustomResult);
    event.preventDefault();

    if (event.dataTransfer.files[0].size > MAX_FILE_SIZE) {
      let componentFactory = this.componentFactoryResolver.resolveComponentFactory(ModalWarningFileSizeComponent);
      let componentRef = this.viewContainerRef.createComponent(componentFactory);

      componentRef.instance.MainDetail = "ファイルサイズ超過";
      componentRef.instance.SufixDetail = "上限は" + Math.round(MAX_FILE_SIZE / 1024 / 1024) + "MBです。"

      componentRef.instance.Closing.subscribe(() => {
        componentRef.destroy();
      });
      this.importFile = null;
      this.importFileNameCtrl.setValue("");
    }
    else {
      this.importFile = event.dataTransfer.files[0];
      this.importFileNameCtrl.setValue(event.dataTransfer.files[0].name)
    }
  }
}
