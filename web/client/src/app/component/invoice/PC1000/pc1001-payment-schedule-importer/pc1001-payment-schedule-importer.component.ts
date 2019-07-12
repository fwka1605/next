import { ModalMasterComponent } from 'src/app/component/modal/modal-master/modal-master.component';
import { Component, OnInit, ViewContainerRef, ComponentFactoryResolver, ElementRef, ComponentRef, AfterViewInit, ViewChild } from '@angular/core';
import { FormControl, Validators, FormGroup } from '@angular/forms';
import { BaseComponent } from 'src/app/component/common/base/base-component';
import { ModalImporterSettingComponent } from 'src/app/component/modal/modal-importer-setting/modal-importer-setting.component';
import { StringUtil } from 'src/app/common/util/string-util';
import { FormatterUtil } from 'src/app/common/util/formatter.util';
import { KEY_CODE, EVENT_TYPE, BUTTON_ACTION } from 'src/app/common/const/event.const';
import { HtmlUtil } from 'src/app/common/util/html.util';
import { CategoryType } from 'src/app/common/const/kbn.const';
import { TABLE_INDEX } from 'src/app/common/const/table-name.const';
import { MODAL_STATUS_TYPE, PROCESS_RESULT_RESULT_TYPE } from 'src/app/common/const/status.const';
import { UserInfoService } from 'src/app/service/common/user-info.service';
import { FreeImporterFormatType } from 'src/app/common/common-const';
import { ImporterSettingService } from 'src/app/service/Master/importer-setting.service';
import { ColumnNameSettingsResult } from 'src/app/model/column-name-settings-result.model';
import { ColumnNameSettingMasterService } from 'src/app/service/Master/column-name-setting-master.service';
import { BillingService } from 'src/app/service/billing.service';
import { ModalRouterProgressComponent } from 'src/app/component/modal/modal-router-progress/modal-router-progress.component';
import { TransactionImportSource } from 'src/app/model/transaction-import-source.model';
import { ImportDataResult } from 'src/app/model/import-data-result.model';
import { DateUtil } from 'src/app/common/util/date-util';
import { FILE_EXTENSION, ENCODE, MAX_FILE_SIZE } from 'src/app/common/const/eb-file.const';
import { FileUtil } from 'src/app/common/util/file.util';
import { REPORT_HEADER, LINE_FEED_CODE } from 'src/app/common/const/report-setting-kbn.const';
import { ModalWarningFileSizeComponent } from 'src/app/component/modal/modal-warning-file-size/modal-warning-file-size.component';
import { ProcessResultService } from 'src/app/service/common/process-result.service';
import { MSG_WNG, MSG_ERR, MSG_INF, MSG_ITEM_NUM } from 'src/app/common/const/message.const';
import { Router, ActivatedRoute, NavigationEnd } from '@angular/router';
import { PageUtil } from 'src/app/common/util/page-util';
import { BreakpointObserver } from '@angular/cdk/layout';
import { MatAutocompleteTrigger } from '@angular/material';
import { ModalImporterSettingPaymentScheduleComponent } from 'src/app/component/modal/modal-importer-setting-payment-schedule/modal-importer-setting-payment-schedule.component';

@Component({
  selector: 'app-pc1001-payment-schedule-importer',
  templateUrl: './pc1001-payment-schedule-importer.component.html',
  styleUrls: ['./pc1001-payment-schedule-importer.component.css']
})
export class Pc1001PaymentScheduleImporterComponent extends BaseComponent implements OnInit,AfterViewInit {

  public ColumnNameSettingsResult: ColumnNameSettingsResult;

  public importDataResult: ImportDataResult;

  public modalRouterProgressComponentRef: ComponentRef<ModalRouterProgressComponent>;

  public importFile: any; // インポートファイルのイベント

  public patternId: number;

  public rdoProcessBillingDataCtrl:FormControl;     // 処理対象請求データ
  public rdoProcessMethodCtrl:FormControl;          // 処理方法
  public rdoCustomerBillingMethodCtrl:FormControl;  // 同一得意先（債権代表者）が同一の他のデータも同時に更新します。

  public patternNoCtrl: FormControl;
  public patternNameCtrl: FormControl;
  public importFileCtrl: FormControl;
  public importFileNameCtrl: FormControl;
  public rdoPrintDataCtrl: FormControl;
  public readCount: number = 0;
  public validCount: number;
  public invalidCount: number;
  public saveCount: number;
  public saveAmount: number;

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
    public columnNameSettingService: ColumnNameSettingMasterService,
    public billingService: BillingService,
    public processResultService: ProcessResultService

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
    this.setAutoComplete();

    this.columnNameSettingService.Get(CategoryType.Receipt)
      .subscribe(response => {
        this.ColumnNameSettingsResult = new ColumnNameSettingsResult();
        this.ColumnNameSettingsResult.columnNames = response;
      });

  }

  ngAfterViewInit(){
    HtmlUtil.nextFocusByName(this.elementRef, 'patternNoCtrl', EVENT_TYPE.NONE);
  }

  public setControlInit() {
    this.patternNoCtrl = new FormControl("", [Validators.required, Validators.maxLength(2)]);
    this.patternNameCtrl = new FormControl("");
    this.importFileCtrl = new FormControl("");
    this.importFileNameCtrl = new FormControl("", [Validators.required]);
    this.rdoPrintDataCtrl = new FormControl("", [Validators.required]);

    this.rdoProcessBillingDataCtrl = new FormControl("", [Validators.required]); // 処理対象請求データ
    this.rdoProcessMethodCtrl = new FormControl("", [Validators.required]);      // 処理方法
    this.rdoCustomerBillingMethodCtrl = new FormControl("", [Validators.required]);  // 同一得意先（債権代表者）が同一の他のデータも同時に更新します。
  
    this.UndefineCtrl = new FormControl("");


  }

  public setValidator() {
    this.MyFormGroup = new FormGroup({
      patternNoCtrl: this.patternNoCtrl,
      patternNameCtrl: this.patternNameCtrl,
      importFileCtrl: this.importFileCtrl,
      importFileNameCtrl: this.importFileNameCtrl,
      rdoPrintDataCtrl: this.rdoPrintDataCtrl,

      rdoProcessBillingDataCtrl: this.rdoProcessBillingDataCtrl,
      rdoProcessMethodCtrl: this.rdoProcessMethodCtrl,
      rdoCustomerBillingMethodCtrl: this.rdoCustomerBillingMethodCtrl,

      UndefineCtrl: this.UndefineCtrl,
    });
  }

  public setFormatter() {
    FormatterUtil.setNumberFormatter(this.patternNoCtrl);
  }


  public setAutoComplete(){
    // パターンNo
    this.initAutocompleteImporterSetting(FreeImporterFormatType.Billing,this.patternNoCtrl,this.importerSettingService,0);
  }  

  public clear() {
    this.MyFormGroup.reset();

    // 初期化処理
    this.patternNoCtrl.enable();
    this.rdoPrintDataCtrl.setValue("1");
    this.importFileNameCtrl.setValue("");

    this.importFile = null;
    this.readCount = null;
    this.validCount = null;
    this.invalidCount = null;
    this.saveCount = null;
    this.saveAmount = null;
    this.patternId = 0;

    this.rdoProcessBillingDataCtrl.setValue("1");
    this.rdoProcessMethodCtrl.setValue("1");
    this.rdoCustomerBillingMethodCtrl.setValue("1");

    //HtmlUtil.nextFocusByName(this.elementRef, 'patternNoCtrl', EVENT_TYPE.NONE);

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

      case BUTTON_ACTION.EXPORT:
        this.outputCustomer();
        break;

    case BUTTON_ACTION.OPEN_MODAL:
      this.openImporterSettingModal()
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
        let transactionImportSource = this.getTransactionImportSource();
        transactionImportSource.encodingCodePage = ENCODE[0].id;
        transactionImportSource.data = btoa(unescape(encodeURIComponent(reader.result.toString())));

        this.billingService.Read(transactionImportSource)
          .subscribe(
            response => {
              if (response == PROCESS_RESULT_RESULT_TYPE.FAILURE) {
                this.processCustomResult = this.processResultService.processAtFailure(
                  this.processCustomResult, MSG_ERR.READING_ERROR, this.partsResultMessageComponent);

                this.modalRouterProgressComponentRef.destroy();
                return;
              }
              else {
                this.importDataResult = new ImportDataResult();
                this.importDataResult = response;
                this.readCount = this.importDataResult.readCount;
                this.validCount = this.importDataResult.validCount;
                this.invalidCount = this.importDataResult.invalidCount;

                if (this.validCount == 0 && this.invalidCount > 0) {
                  this.rdoPrintDataCtrl.setValue("2");
                }

                if (response.logs != undefined && response.logs.length > 0) {
                  this.processCustomResult = this.processResultService.processAtFailure(
                    this.processCustomResult, MSG_ERR.READING_ERROR+ "詳細はログファイルを確認してください。",
                    this.partsResultMessageComponent);

                  this.downloadErrorLog(response.logs);
                }
                else if (response.logs == undefined || response.logs.length == 0) {
                  this.processCustomResult = this.processResultService.processAtSuccess(
                    this.processCustomResult, MSG_INF.FINISH_FILE_READING_PROCESS, this.partsResultMessageComponent);
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

    });
  }

  public getTransactionImportSource(): TransactionImportSource {

    let transactionImportSource = new TransactionImportSource();
    transactionImportSource.companyId = this.userInfoService.Company.id;
    transactionImportSource.loginUserId = this.userInfoService.LoginUser.id;
    transactionImportSource.importerSettingId = this.patternId;

    return transactionImportSource;
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

    let transactionImportSource = this.getTransactionImportSource();
    transactionImportSource.importDataId = this.importDataResult.importData.id;

    this.billingService.Import(transactionImportSource)
      .subscribe(
        response => {
          if (response == PROCESS_RESULT_RESULT_TYPE.FAILURE) {
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
              this.processCustomResult = this.processResultService.processAtFailure(
                this.processCustomResult, MSG_ERR.SAVE_ERROR+"詳細はログファイルを確認してください。",
                this.partsResultMessageComponent);
              this.downloadErrorLog(response.logs);
            }
            else if (response.logs == undefined || response.logs.length == 0) {
              this.processCustomResult = this.processResultService.processAtSuccess(
                this.processCustomResult, MSG_INF.SAVE_SUCCESS, this.partsResultMessageComponent);
               this.processResultService.createdLog(this.processCustomResult.logData);

            }
          }

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
    if (this.rdoPrintDataCtrl.value == "1") {
      transactionImportSource.isValidData = true;
    }
    else {
      transactionImportSource.isValidData = false;
    }
    transactionImportSource.importDataId = this.importDataResult.importData.id;

    this.billingService.GetImportValidationReport(transactionImportSource)
      .subscribe(response => {
        FileUtil.download([response.body], this.Title + DateUtil.getYYYYMMDD(0), FILE_EXTENSION.PDF);
        this.modalRouterProgressComponentRef.destroy();
      });

  }

  /**
   * 新規得意先マスター印刷
   */
  public printCustomer() {

    this.processCustomResult = this.processResultService.clearProcessCustomMsg(this.processCustomResult);

    // 動作中のコンポーネントを開く
    let componentFactory = this.componentFactoryResolver.resolveComponentFactory(ModalRouterProgressComponent);
    this.modalRouterProgressComponentRef = this.viewContainerRef.createComponent(componentFactory);
    this.modalRouterProgressComponentRef.instance.Closing.subscribe(() => {
      this.modalRouterProgressComponentRef.destroy();
    });

    let transactionImportSource = this.getTransactionImportSource();
    transactionImportSource.importDataId = this.importDataResult.importData.id;
    this.billingService.GetImportNewCustomerReport(transactionImportSource)
      .subscribe(response => {

        FileUtil.download([response.body], this.Title + "新規得意先一覧" + DateUtil.getYYYYMMDD(0), FILE_EXTENSION.PDF);
        this.modalRouterProgressComponentRef.destroy();
      });
  }

  /**
   * 新規得意先マスター出力
   */
  public outputCustomer() {

    let transactionImportSource = this.getTransactionImportSource();
    transactionImportSource.importDataId = this.importDataResult.importData.id;

    this.billingService.GetImportNewCustomer(transactionImportSource)
      .subscribe(response => {

        if (response != PROCESS_RESULT_RESULT_TYPE.FAILURE) {

          let dataList = response;
          let headers = FileUtil.encloseItemBySymbol(REPORT_HEADER.CUSTOMER_MASTER);
          let data: string = headers.join(",") + LINE_FEED_CODE;

          // 件数チェック
          if (dataList.length <= 0) {
            this.processCustomResult = this.processResultService.processAtWarning(
              this.processCustomResult, MSG_WNG.NO_EXPORT_DATA, this.partsResultMessageComponent);
          }
          else{
            this.processCustomResult = this.processResultService.processAtSuccess(
              this.processCustomResult, MSG_INF.PROCESS_FINISH, this.partsResultMessageComponent);

            this.processResultService.createdLog(this.processCustomResult.logData);

              // CSV作成
            data += this.createCsv(dataList);

            let resultDatas: Array<any> = [];
            resultDatas.push(data);
            FileUtil.download(resultDatas, '新規得意先' + DateUtil.getYYYYMMDD(0), FILE_EXTENSION.CSV);              
          }

        }
        else{
          this.processCustomResult = this.processResultService.processAtFailure(
            this.processCustomResult, MSG_ERR.SOMETHING_ERROR.replace(MSG_ITEM_NUM.FIRST,"得意先出力"),
            this.partsResultMessageComponent);

        }
      });
  }

  /**
   * CSV作成
   * @param dataList 出力するデータ
   */
  public createCsv(dataList: Array<any>): string {
    let result = '';

    for (let index = 0; index < dataList.length; index++) {
      let dataItem: Array<any> = [];
      dataItem.push(this.userInfoService.Company.code);
      dataItem.push(dataList[index].code);
      dataItem.push(dataList[index].name);
      dataItem.push(dataList[index].kana);
      dataItem.push(dataList[index].exclusiveBankCode);
      dataItem.push(dataList[index].exclusiveBankName);
      dataItem.push(dataList[index].exclusiveBranchCode);
      dataItem.push(dataList[index].exclusiveBranchName);
      dataItem.push(dataList[index].exclusiveAccountNumber);
      dataItem.push(dataList[index].exclusiveAccountTypeId);
      dataItem.push(dataList[index].shareTransferFee);
      dataItem.push(dataList[index].creditLimit);
      dataItem.push(dataList[index].closingDay);
      dataItem.push(dataList[index].collectCategoryCode);
      dataItem.push(dataList[index].collectOffsetMonth);
      dataItem.push(dataList[index].collectOffsetDay);
      dataItem.push(dataList[index].staffCode);
      dataItem.push(dataList[index].isParent);
      dataItem.push(dataList[index].postalCode);
      dataItem.push(dataList[index].address1);
      dataItem.push(dataList[index].address2);
      dataItem.push(dataList[index].tel);
      dataItem.push(dataList[index].fax);
      dataItem.push(dataList[index].customerStaffName);
      dataItem.push(dataList[index].note);
      dataItem.push(dataList[index].useFeeLearning);
      dataItem.push(dataList[index].sightOfBill);
      dataItem.push(dataList[index].densaiCode);
      dataItem.push(dataList[index].creditCode);
      dataItem.push(dataList[index].creditRank);
      dataItem.push(dataList[index].transferBankCode);
      dataItem.push(dataList[index].transferBankName);
      dataItem.push(dataList[index].transferBranchCode);
      dataItem.push(dataList[index].transferBranchName);
      dataItem.push(dataList[index].transferAccountNumber);
      dataItem.push(dataList[index].transferAccountTypeId);
      dataItem.push(dataList[index].transferCustomerCode);
      dataItem.push(dataList[index].transferNewCode);
      dataItem.push(dataList[index].transferAccountName);
      dataItem.push(dataList[index].thresholdValue);
      dataItem.push(dataList[index].lessThanCollectCategoryId);
      dataItem.push(dataList[index].greaterThanCollectCategoryId1);
      dataItem.push(dataList[index].greaterThanRate1);
      dataItem.push(dataList[index].greaterThanRoundingMode1);
      dataItem.push(dataList[index].greaterThanSightOfBill1);
      dataItem.push(dataList[index].greaterThanCollectCategoryId2);
      dataItem.push(dataList[index].greaterThanRate2);
      dataItem.push(dataList[index].greaterThanRoundingMode2);
      dataItem.push(dataList[index].greaterThanSightOfBill2);
      dataItem.push(dataList[index].greaterThanCollectCategoryId3);
      dataItem.push(dataList[index].greaterThanRate3);
      dataItem.push(dataList[index].greaterThanRoundingMode3);
      dataItem.push(dataList[index].greaterThanSightOfBill3);
      dataItem.push(dataList[index].useKanaLearning);
      dataItem.push(dataList[index].holidayFlag);
      dataItem.push(dataList[index].useFeeTolerance);
      dataItem.push(dataList[index].prioritizeMatchingIndividually);
      dataItem.push(dataList[index].collationKey);
      dataItem.push(dataList[index].destinationDepartmentName);
      dataItem.push(dataList[index].honorific);
      dataItem = FileUtil.encloseItemBySymbol(dataItem);

      result = result + dataItem.join(",") + LINE_FEED_CODE;
    }

    return result;
  }

  /**
   * エラーログを出力する
   * @param error エラー
   */
  public downloadErrorLog(error: any) {
    let errorMsg = new Array<String>();
    errorMsg.push(DateUtil.getYYYYMMDD(3));
    errorMsg.push("請求データ：" + this.importFileNameCtrl.value);
    errorMsg = errorMsg.concat(error);

    let errorData = new Array<any>();
    errorData.push(errorMsg.join(LINE_FEED_CODE));

    FileUtil.download(errorData, DateUtil.getYYYYMMDD(0) + "_Import", FILE_EXTENSION.LOG);
  }

  public openImporterSettingModal() {

    let componentFactory = this.componentFactoryResolver.resolveComponentFactory(ModalImporterSettingPaymentScheduleComponent);
    let componentRef = this.viewContainerRef.createComponent(componentFactory);
    componentRef.instance.Title = "入金予定";
    componentRef.instance.processModalCustomResult = this.processModalCustomResult;

    componentRef.instance.Closing.subscribe(() => {
      componentRef.destroy();
    });
  }

  public openMasterModal() {

    this.patternNoTrigger.closePanel();

    let componentFactory = this.componentFactoryResolver.resolveComponentFactory(ModalMasterComponent);
    let componentRef = this.viewContainerRef.createComponent(componentFactory);
    componentRef.instance.TableIndex = TABLE_INDEX.MASTER_BILLING_IMPORTER_SETTING;
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

  ////////////////////////////////////////////////////////////////////////////////////////////////////////

  public setPatternNo(eventType: string) {

    if(eventType!=EVENT_TYPE.BLUR){
      this.patternNoTrigger.closePanel();
    }


    if (!StringUtil.IsNullOrEmpty(this.patternNoCtrl.value)) {
      this.patternNoCtrl.setValue(StringUtil.setPaddingFrontZero(this.patternNoCtrl.value, 2));

      this.loadStart();
      this.importerSettingService.GetHeader(FreeImporterFormatType.Billing, this.patternNoCtrl.value)
        .subscribe(response => {
          this.loadEnd();
          if (response != PROCESS_RESULT_RESULT_TYPE.FAILURE && response != null && response.length > 0) {
            this.processResultService.clearProcessCustomMsg(this.processCustomResult);
            this.patternNoCtrl.setValue(response[0].code);
            this.patternNoCtrl.disable();
            this.patternNameCtrl.setValue(response[0].name);
            this.patternId = response[0].id
          }
          else {
            if (!StringUtil.IsNullOrEmpty(this.patternNoCtrl.value)) {
              // let msg = MSG_WNG.NOT_REGIST_PATTERN_NO.replace(MSG_ITEM_NUM.FIRST, this.patternNoCtrl.value);
              // this.processCustomResult = this.processResultService.processAtWarning(this.processCustomResult, msg);
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
    }
    HtmlUtil.nextFocusByName(this.elementRef, "importFileButtonCtrl", eventType);
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