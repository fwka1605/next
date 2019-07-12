import { Component, OnInit, ElementRef, ComponentFactoryResolver, ViewContainerRef, AfterViewInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { NgbDateParserFormatter } from '@ng-bootstrap/ng-bootstrap';
import { forkJoin, Observable } from 'rxjs';

import { BaseComponent } from 'src/app/component/common/base/base-component';
import { FILE_EXTENSION } from 'src/app/common/const/eb-file.const';
import { CategoryType, AccountTransferFileFormatId } from 'src/app/common/const/kbn.const';
import { MODAL_STATUS_TYPE, PROCESS_RESULT_RESULT_TYPE } from 'src/app/common/const/status.const';
import { TABLE_INDEX } from 'src/app/common/const/table-name.const';
import { NgbDateCustomParserFormatter } from 'src/app/common/util/dateformat-util';
import { DateUtil } from 'src/app/common/util/date-util';
import { FileUtil } from 'src/app/common/util/file.util';
import { FormatterUtil } from 'src/app/common/util/formatter.util';
import { HtmlUtil } from 'src/app/common/util/html.util';
import { StringUtil } from 'src/app/common/util/string-util';
import { ModalConfirmComponent } from 'src/app/component/modal/modal-confirm/modal-confirm.component';
import { AccountTransferDetail } from 'src/app/model/account-transfer-detail.model';
import { AccountTransferDetailsResult } from 'src/app/model/account-transfer-details-result.model';
import { AccountTransferExtractResult } from 'src/app/model/account-transfer-extract-result.model';
import { AccountTransferLog } from 'src/app/model/account-transfer-log.model';
import { AccountTransferLogsResult } from 'src/app/model/account-transfer-logs-result.model';
import { AccountTransferSearch } from 'src/app/model/account-transfer-search.model';
import { Category } from 'src/app/model/category.model';
import { CategoriesResult } from 'src/app/model/categories-result.model';
import { PaymentAgenciesResult } from 'src/app/model/payment-agencies-result.model'
import { PaymentAgency } from 'src/app/model/payment-agency.model';
import { PaymentFileFormatResult } from 'src/app/model/payment-file-format-result.model'
import { ExporterBase } from 'src/app/model/account-transfer-export/exporter-base';
import { InternetJPBankExporter } from 'src/app/model/account-transfer-export/internet-jpbank-exporter';
import { MizuhoAspExporter } from 'src/app/model/account-transfer-export/mizuho-asp-exporter';
import { MizuhoFactorExporter } from 'src/app/model/account-transfer-export/mizuho-factor-exporter';
import { MUFJFactorExporter } from 'src/app/model/account-transfer-export/mufj-factor-exporter';
import { MUFJNicosExporter } from 'src/app/model/account-transfer-export/mufj-nicos-exporter';
import { SMBCKoufuriExporter } from 'src/app/model/account-transfer-export/smbc-koufuri-exporter';
import { ZenginCommaExporter } from 'src/app/model/account-transfer-export/zengin-comma-exporter';
import { ZenginFixedExporter } from 'src/app/model/account-transfer-export/zengin-fixed-exporter';
import { AccountTransferService } from 'src/app/service/account-transfer.service';
import { UserInfoService } from 'src/app/service/common/user-info.service';
import { CategoryMasterService } from 'src/app/service/Master/category-master.service';
import { PaymentAgencyMasterService } from 'src/app/service/Master/payment-agency-master.service';
import { LINE_FEED_CODE } from 'src/app/common/const/report-setting-kbn.const';
import { EVENT_TYPE, BUTTON_ACTION } from 'src/app/common/const/event.const';
import { ProcessResultService } from 'src/app/service/common/process-result.service';
import { MSG_WNG, MSG_INF, MSG_ERR, MSG_ITEM_NUM } from 'src/app/common/const/message.const';
import { Router, ActivatedRoute, NavigationEnd } from '@angular/router';
import { PageUtil } from 'src/app/common/util/page-util';
import { ModalRouterProgressComponent } from 'src/app/component/modal/modal-router-progress/modal-router-progress.component';

@Component({
  selector: 'app-pc0701-account-transfer-create',
  templateUrl: './pc0701-account-transfer-create.component.html',
  styleUrls: ['./pc0701-account-transfer-create.component.css'],
  providers: [
    { provide: NgbDateParserFormatter, useClass: NgbDateCustomParserFormatter }
  ]

})
export class Pc0701AccountTransferCreateComponent extends BaseComponent implements OnInit,AfterViewInit {

  // これをしないとEnum型がテンプレートで見えなかったため
  public TABLE_INDEX: typeof TABLE_INDEX = TABLE_INDEX;

  public panelOpenState: boolean;


  public collectCategoriesResult: CategoriesResult;
  public accountTransferLogsResult: AccountTransferLogsResult;
  public paymentAgenciesResult: PaymentAgenciesResult;
  public paymentFileFormatResult: PaymentFileFormatResult;

  public cmbCollectCategoryCtrl: FormControl; // 回収区分

  public paymentAgencyCodeCtrl: FormControl; // 決済代行会社
  public paymentAgencyNameCtrl: FormControl;

  public fileFormatNameCtrl: FormControl; // フォーマット

  public outputFilePathCtrl: FormControl; // 出力ファイル名

  public dueAtFromCtrl: FormControl; // 入金予定日
  public dueAtToCtrl: FormControl;

  public dueAtCtrl: FormControl; // 引落日
  public dueAt2ndCtrl: FormControl; // 再引落日
  public cbxLogCtrls: FormControl[]; // 選択

  public undefineCtrl: FormControl; // 未定用

  public extractionCount: number = 0; //  抽出件数
  public invalidCount: number = 0; //出力不可能件数
  public extractionAmount: number = 0;  //  抽出金額

  public outputCount: number = 0;   //出力件数
  public outputAmount: number = 0;  //出力金額

  public selectedAccountSummary: boolean; //

  public collectCategoryId: number;
  public selectedFileFormatId: number = 0;
  public extractRawData: AccountTransferDetail[];
  public extractAggregateData: AccountTransferDetail[];
  public paymentAgency: PaymentAgency;

  constructor(
    public router: Router,
    public activatedRoute: ActivatedRoute,    
    public elementRef: ElementRef,
    public componentFactoryResolver: ComponentFactoryResolver,
    public viewContainerRef: ViewContainerRef,
    public userInfoService: UserInfoService,
    public categoryService: CategoryMasterService,
    public paymentAgencyService: PaymentAgencyMasterService,
    public accountTransferService: AccountTransferService,
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

    let collectCategoryResponse = this.categoryService.GetItems(CategoryType.Collection);
    let paymentAgencyResponse = this.paymentAgencyService.GetItems();
    let fileFormatResponse = this.paymentAgencyService.GetFileFormat();

    forkJoin(
      collectCategoryResponse,
      paymentAgencyResponse,
      fileFormatResponse,
    )
      .subscribe(
        responseList => {
          if (
                 responseList != undefined 
              && responseList.length == 3
              && responseList[0] != PROCESS_RESULT_RESULT_TYPE.FAILURE
              && responseList[1] != PROCESS_RESULT_RESULT_TYPE.FAILURE
              && responseList[2] != PROCESS_RESULT_RESULT_TYPE.FAILURE
              
          ) {
            this.collectCategoriesResult = new CategoriesResult();
            this.paymentAgenciesResult = new PaymentAgenciesResult();
            this.paymentFileFormatResult = new PaymentFileFormatResult();

            let collectCategories: Category[] = responseList[0];
            this.collectCategoriesResult.categories = collectCategories.filter(x => x.useAccountTransfer == 1);

            this.paymentAgenciesResult.paymentAgencies = responseList[1];
            this.paymentFileFormatResult.paymentFileFormats = responseList[2];
            this.getAccountTransferLogResult();

          }
        });

  }

  ngAfterViewInit(){
    HtmlUtil.nextFocusByName(this.elementRef, 'cmbCollectCategoryCtrl', EVENT_TYPE.NONE);
  }

  public setControlInit() {
    this.cmbCollectCategoryCtrl = new FormControl("", [Validators.required]); // 回収区分

    this.paymentAgencyCodeCtrl = new FormControl(""); // 決済代行会社
    this.paymentAgencyNameCtrl = new FormControl("");

    this.fileFormatNameCtrl = new FormControl(""); // フォーマット

    this.outputFilePathCtrl = new FormControl(""); // 出力ファイル名

    this.dueAtFromCtrl = new FormControl("", [Validators.maxLength(10)]); // 入金予定日
    this.dueAtToCtrl = new FormControl("", [Validators.maxLength(10)]);

    this.dueAtCtrl = new FormControl("", [Validators.maxLength(10)]); // 引落日

    this.dueAt2ndCtrl = new FormControl("", [Validators.maxLength(10)]); // 再引落日

    this.undefineCtrl = new FormControl(""); // 未定用;

  }

  public setValidator() {
    this.MyFormGroup = new FormGroup({

      cmbCollectCategoryCtrl: this.cmbCollectCategoryCtrl, // 回収区分

      paymentAgencyCodeCtrl: this.paymentAgencyCodeCtrl, // 決済代行会社
      paymentAgencyNameCtrl: this.paymentAgencyNameCtrl,

      fileFormatNameCtrl: this.fileFormatNameCtrl, // フォーマット

      outputFilePathCtrl: this.outputFilePathCtrl, // 出力ファイル名

      dueAtFromCtrl: this.dueAtFromCtrl, // 入金予定日
      dueAtToCtrl: this.dueAtToCtrl,

      dueAtCtrl: this.dueAtCtrl, // 引落日

      dueAt2ndCtrl: this.dueAt2ndCtrl, // 再引落日

      undefineCtrl: this.undefineCtrl, // 未定用;

    });


  }

  public setFormatter() {
    //FormatterUtil.setCodeFormatter(this.paymentAgencyCodeCtrl);
  }

  public clear() {
    this.MyFormGroup.reset();
    // 初期化処理
    this.extractionCount = 0;
    this.invalidCount = 0;
    this.extractionAmount = 0;
    this.outputCount = 0;
    this.outputAmount = 0;
    this.selectedAccountSummary = false;

    HtmlUtil.nextFocusByName(this.elementRef, 'cmbCollectCategoryCtrl', EVENT_TYPE.NONE);

    this.cmbCollectCategoryCtrl.enable();
    this.dueAtFromCtrl.enable();
    this.dueAtToCtrl.enable();    

    this.dueAtCtrl.disable();
    this.dueAt2ndCtrl.disable();

    this.panelOpenState = true;
    
  }

  public getAccountTransferLogResult() {

    this.accountTransferLogsResult = new AccountTransferLogsResult();
    this.accountTransferService.Get()
      .subscribe(response => {
        if (response != PROCESS_RESULT_RESULT_TYPE.FAILURE) {
          this.accountTransferLogsResult.accountTransferLog = response;

          this.cbxLogCtrls = new Array<FormControl>(this.accountTransferLogsResult.accountTransferLog.length);

          for (let i = 0; i < this.cbxLogCtrls.length; i++) {
            this.cbxLogCtrls[i] = new FormControl(null);
          }

          for (let i = 0; i < this.cbxLogCtrls.length; i++) {
            this.MyFormGroup.removeControl("cbxLogCtrl" + i);
            this.MyFormGroup.addControl("cbxLogCtrl" + i, this.cbxLogCtrls[i]);
          }
        }
        else{
          this.processCustomResult = this.processResultService.processAtFailure(
            this.processCustomResult,MSG_ERR.SOMETHING_ERROR.replace(MSG_ITEM_NUM.FIRST,"履歴の検索"),
            this.partsResultMessageComponent)
        }
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

      case BUTTON_ACTION.EXTRACT:
        this.extract();
        break;
      case BUTTON_ACTION.EXPORT:
        this.output();
        break;
      case BUTTON_ACTION.REEXPORT:
        this.reOutput();
        break;
      case BUTTON_ACTION.CANCEL:
        this.cancel();
        break;


      default:
        console.log('buttonAction Error.');
        break;
    }
    
  }  


  public extract() {
    this.extractionCount = 0;
    this.invalidCount = 0;
    this.extractionAmount = 0;
    this.outputCount = 0;
    this.outputAmount = 0;

    if (!this.isValidForExtract()) {
      return;
    }

    // 動作中のコンポーネントを開く
    let componentFactory = this.componentFactoryResolver.resolveComponentFactory(ModalRouterProgressComponent);
    let modalRouterProgressComponentRef = this.viewContainerRef.createComponent(componentFactory);
    modalRouterProgressComponentRef.instance.Closing.subscribe(() => {
      modalRouterProgressComponentRef.destroy();
    });    

    let accountTransferSearch = this.getAccountTransferSearch(false);
    this.accountTransferService.Extract(accountTransferSearch)
      .subscribe(response => {

        let result:AccountTransferExtractResult = response;

        if (response != PROCESS_RESULT_RESULT_TYPE.FAILURE ) {

          this.extractRawData = result.details;
          this.processResultService.processAtGetData(
            this.processCustomResult, this.extractRawData, true, this.partsResultMessageComponent);

          if (this.isDoAggregate()) {
            this.extractAggregateData = this.aggregateByKey(this.extractRawData);
          }
          else {
            this.extractAggregateData = this.extractRawData;
          }

          this.extractAggregateData.forEach(item => {
            this.extractionCount++;
            this.extractionAmount += item.billingAmount;
            if (!this.isValid(item)) {
              this.invalidCount++;
            }
          });

          if (result.logs.length > 0) {
            this.processCustomResult = this.processResultService.processAtWarning(
              this.processCustomResult, '抽出時に検証エラーがあります。詳細はログファイルを確認してください。',
              this.partsResultMessageComponent);

            this.exportInvalidLogs(result.logs);
          }

          if(this.extractRawData.length >0){
            this.cmbCollectCategoryCtrl.disable();
            this.dueAtFromCtrl.disable();
            this.dueAtToCtrl.disable();
          }

        }
        else{
          this.processCustomResult = this.processResultService.processAtFailure(
            this.processCustomResult, MSG_ERR.SOMETHING_ERROR.replace(MSG_ITEM_NUM.FIRST,"抽出"),
            this.partsResultMessageComponent);

        }
        modalRouterProgressComponentRef.destroy();
      });

  }

  public exportInvalidLogs(logs: string[]) {
    const datas = [
      ([DateUtil.getYYYYMMDD(3)]
        .concat(...logs)).join(LINE_FEED_CODE),
    ];
    FileUtil.download(datas, DateUtil.getYYYYMMDD(0) + '_extract', FILE_EXTENSION.LOG);
  }

  public isValidForExtract(): boolean {

    if (!DateUtil.isValidRange(this.dueAtFromCtrl, this.dueAtToCtrl)) {
      return false;
    }

    return true;
  }

  public getAccountTransferSearch(reAction: boolean): AccountTransferSearch {

    let accountTransferSearch = new AccountTransferSearch();
    accountTransferSearch.companyId = this.userInfoService.Company.id;
    accountTransferSearch.collectCategoryId = this.collectCategoryId;
    accountTransferSearch.dueAtFrom = DateUtil.ConvertFromDatepicker(this.dueAtFromCtrl);
    accountTransferSearch.dueAtTo = DateUtil.ConvertFromDatepicker(this.dueAtToCtrl);

    if (reAction) {
      accountTransferSearch.accountTransferLogIds = new Array<number>();

      for (let index = 0; index < this.accountTransferLogsResult.accountTransferLog.length; index++) {
        if (this.cbxLogCtrls[index].value) {
          accountTransferSearch.accountTransferLogIds.push(this.accountTransferLogsResult.accountTransferLog[index].id);
        }
      }
      accountTransferSearch.dueAtFrom = null;
      accountTransferSearch.dueAtTo = null;
    }

    return accountTransferSearch;
  }

  public output() {


    if (this.selectedFileFormatId == AccountTransferFileFormatId.InternetJPBankFixed){

      if(this.dueAtCtrl.value == undefined) {

        this.processCustomResult = this.processResultService.processAtWarning(
          this.processCustomResult, MSG_WNG.INPUT_REQUIRED.replace(MSG_ITEM_NUM.FIRST,"引落日"),
          this.partsResultMessageComponent);
  
        HtmlUtil.nextFocusByName(this.elementRef, 'dueAt2ndCtrl', "");
        return;
      }

      if(this.dueAt2ndCtrl.value == undefined) {

        this.processCustomResult = this.processResultService.processAtWarning(
          this.processCustomResult, MSG_WNG.INPUT_REQUIRED.replace(MSG_ITEM_NUM.FIRST,"再引落日"),
          this.partsResultMessageComponent);
  
        HtmlUtil.nextFocusByName(this.elementRef, 'dueAt2ndCtrl', "");
        return;
      }

    } 
    
    if (this.dueAtCtrl.value == undefined) {

      this.processCustomResult = this.processResultService.processAtWarning(
        this.processCustomResult, MSG_WNG.INPUT_REQUIRED.replace(MSG_ITEM_NUM.FIRST,"引落日"),
        this.partsResultMessageComponent);

      HtmlUtil.nextFocusByName(this.elementRef, 'dueAtCtrl', "");
      return;
    }

    // 動作中のコンポーネントを開く
    let componentFactory = this.componentFactoryResolver.resolveComponentFactory(ModalRouterProgressComponent);
    let modalRouterProgressComponentRef = this.viewContainerRef.createComponent(componentFactory);
    modalRouterProgressComponentRef.instance.Closing.subscribe(() => {
      modalRouterProgressComponentRef.destroy();
    });

    this.extractRawData.forEach(item => {
      item.createBy = this.userInfoService.LoginUser.id;
      item.newDueAt = DateUtil.ConvertFromDatepicker(this.dueAtCtrl);
      item.dueAt2nd = DateUtil.ConvertFromDatepicker(this.dueAt2ndCtrl);
    });

    let dueAt: string = DateUtil.ConvertFromDatepicker(this.dueAtCtrl);
    let dueAt2nd: string = DateUtil.ConvertFromDatepicker(this.dueAt2ndCtrl);
    let exporter = this.getExporter(this.paymentAgency, dueAt, dueAt2nd);
    let expData = exporter.Export(this.extractAggregateData);
    let resultDatas: Array<any> = [];
    resultDatas.push(expData);

    let fileName: string = "";
    if (this.paymentAgency.appendDate == 1) {
      fileName = this.paymentAgency.outputFileName + DateUtil.getYYYYMMDD(0);
    }
    else {
      fileName = this.paymentAgency.outputFileName;
    }

    FileUtil.download(resultDatas, fileName, FILE_EXTENSION.CSV);
    this.openOptions();

    this.accountTransferService.Save(this.extractRawData)
      .subscribe(response => {

        if (response != PROCESS_RESULT_RESULT_TYPE.FAILURE && response.length > 0) {

          this.processCustomResult = this.processResultService.processAtSuccess(
            this.processCustomResult, MSG_INF.PROCESS_FINISH, this.partsResultMessageComponent);
          this.processResultService.createdLog(this.processCustomResult.logData);


          this.outputAmount = this.extractAggregateData.reduce((tis, org) => tis + org.billingAmount, 0);
          this.outputCount = this.extractAggregateData.length;

          this.getAccountTransferLogResult();

        }
        else {
          this.processCustomResult = this.processResultService.processAtFailure(
            this.processCustomResult, MSG_ERR.SOMETHING_ERROR.replace(MSG_ITEM_NUM.FIRST,"出力"),
            this.partsResultMessageComponent);
        }
        modalRouterProgressComponentRef.destroy();

      });

  }

  public reOutput() {

    let accountTransferSearch = this.getAccountTransferSearch(true);

    // 動作中のコンポーネントを開く
    let componentFactory = this.componentFactoryResolver.resolveComponentFactory(ModalRouterProgressComponent);
    let modalRouterProgressComponentRef = this.viewContainerRef.createComponent(componentFactory);
    modalRouterProgressComponentRef.instance.Closing.subscribe(() => {
      modalRouterProgressComponentRef.destroy();
    });

    this.accountTransferService.Extract(accountTransferSearch)
      .subscribe(response => {
        if (response != PROCESS_RESULT_RESULT_TYPE.FAILURE && response.details.length > 0) {
          let source = new Array<AccountTransferDetail>();
          source = response.details;
          let firstData: AccountTransferDetail = source[0];
          let paymentAgency = this.paymentAgenciesResult.paymentAgencies.find(x => x.id == firstData.paymentAgencyId);

          let exporter = this.getExporter(paymentAgency, firstData.dueAt, firstData.dueAt2nd);
          let exportData = exporter.Export(source);

          let resultDatas: Array<any> = [];
          resultDatas.push(exportData);

          let fileName: string = "";
          if (paymentAgency.appendDate == 1) {
            fileName = "Re_" + paymentAgency.outputFileName + DateUtil.getYYYYMMDD(0);
          }
          else {
            fileName = "Re_" + paymentAgency.outputFileName;
          }

          FileUtil.download(resultDatas, fileName, FILE_EXTENSION.CSV);
          this.openOptions();


          this.processCustomResult = this.processResultService.processAtSuccess(
            this.processCustomResult, MSG_INF.PROCESS_FINISH, this.partsResultMessageComponent);
          this.processResultService.createdLog(this.processCustomResult.logData);          
        }
        else{
          this.processCustomResult = this.processResultService.processAtFailure(
            this.processCustomResult, MSG_ERR.SOMETHING_ERROR.replace(MSG_ITEM_NUM.FIRST,"再出力"),
            this.partsResultMessageComponent);
        }
        modalRouterProgressComponentRef.destroy();
      });

  }

  public getExporter(paymentAgency: PaymentAgency, dueAt: string, dueAt2nd: string): ExporterBase {

    let exporter: ExporterBase;

    switch (paymentAgency.fileFormatId) {
      case AccountTransferFileFormatId.ZenginCsv:
      case AccountTransferFileFormatId.RicohLeaseCollectCsv:
      case AccountTransferFileFormatId.RisonaNetCsv:
      case AccountTransferFileFormatId.ShinkinJohoService:
        exporter = new ZenginCommaExporter();
        break;
      case AccountTransferFileFormatId.ZenginFixed:
        exporter = new ZenginFixedExporter();
        break;
      case AccountTransferFileFormatId.MizuhoFactorWebFixed:
        exporter = new MizuhoFactorExporter();
        break;
      case AccountTransferFileFormatId.MitsubishiUfjFactorCsv:
        exporter = new MUFJFactorExporter();
        break;
      case AccountTransferFileFormatId.SMBCFixed:
        exporter = new SMBCKoufuriExporter();
        break;
      case AccountTransferFileFormatId.MitsubishiUfjNicosCsv:
        exporter = new MUFJNicosExporter();
        break;
      case AccountTransferFileFormatId.MizuhoFactorAspCsv:
        exporter = new MizuhoAspExporter();
        break;
      case AccountTransferFileFormatId.InternetJPBankFixed:
        exporter = new InternetJPBankExporter();
        exporter.DueAt2nd = dueAt2nd;
        break;
    }

    exporter.Company = this.userInfoService.Company;
    exporter.PaymentAgency = paymentAgency;
    exporter.DueAt = dueAt;

    return exporter;
  }

  public print() {
    this.processCustomResult = this.processResultService.clearProcessCustomMsg(this.processCustomResult);
    this.printPdf(false);
  }

  public rePrint() {
    this.processCustomResult = this.processResultService.clearProcessCustomMsg(this.processCustomResult);
    this.printPdf(true);
  }

  public printPdf(rePrint: boolean) {

    let accountTransferSearch = this.getAccountTransferSearch(rePrint);
    this.accountTransferService.Extract(accountTransferSearch)
      .subscribe(response => {
        if (response != PROCESS_RESULT_RESULT_TYPE.FAILURE) {

          this.processCustomResult = this.processResultService.processAtSuccess(
            this.processCustomResult, MSG_INF.PROCESS_FINISH, this.partsResultMessageComponent);
          
          this.processResultService.createdLog(this.processCustomResult.logData);

          FileUtil.download([response.body], this.Title + DateUtil.getYYYYMMDD(0), FILE_EXTENSION.PDF);
        }
        else {
          this.processCustomResult = this.processResultService.processAtFailure(
            this.processCustomResult, MSG_ERR.CREATE_REPORT_ERROR, this.partsResultMessageComponent);
        }
      });
  }

  public cancel() {
    let componentFactory = this.componentFactoryResolver.resolveComponentFactory(ModalConfirmComponent);
    let componentRef = this.viewContainerRef.createComponent(componentFactory);
    componentRef.instance.ActionName = "選択した口振の取消"
    componentRef.instance.Closing.subscribe(() => {
      if (componentRef.instance.ModalStatus == MODAL_STATUS_TYPE.OK) {

        let logs = new Array<AccountTransferLog>();
        for (let index = 0; index < this.accountTransferLogsResult.accountTransferLog.length; index++) {
          if (this.cbxLogCtrls[index].value) {
            logs.push(this.accountTransferLogsResult.accountTransferLog[index]);
          }
        }

        if (logs.length > 0) {

          // 動作中のコンポーネントを開く
          let componentFactory = this.componentFactoryResolver.resolveComponentFactory(ModalRouterProgressComponent);
          let modalRouterProgressComponentRef = this.viewContainerRef.createComponent(componentFactory);
          modalRouterProgressComponentRef.instance.Closing.subscribe(() => {
            modalRouterProgressComponentRef.destroy();
          }); 

          this.accountTransferService.Cancel(logs)
            .subscribe(response => {
              if (response != PROCESS_RESULT_RESULT_TYPE.FAILURE) {
                this.clear();
                this.getAccountTransferLogResult();
                this.processCustomResult = this.processResultService.processAtSuccess(
                  this.processCustomResult, MSG_INF.PROCESS_FINISH, this.partsResultMessageComponent);
                
                this.processResultService.createdLog(this.processCustomResult.logData);

              }
              else {
                this.processCustomResult = this.processResultService.processAtFailure(
                  this.processCustomResult, MSG_ERR.SOMETHING_ERROR.replace(MSG_ITEM_NUM.FIRST, '取消'),
                  this.partsResultMessageComponent);
              }
              modalRouterProgressComponentRef.destroy();

            });
        }
      }
      else{
        this.processCustomResult = this.processResultService.processAtWarning(
          this.processCustomResult, MSG_WNG.PROCESS_CANCELED, this.partsResultMessageComponent);        
      }
      componentRef.destroy();
    });

  }

  /////////////////////////////////////////////////////////////////////////////////////////
  public setCmbCollectCategory(eventType: string) {
    HtmlUtil.nextFocusByName(this.elementRef, 'dueAtToCtrl', eventType);

    if (!StringUtil.IsNullOrEmpty(this.cmbCollectCategoryCtrl.value)) {

      let collectCategory = this.collectCategoriesResult.categories.find(x => x.code == this.cmbCollectCategoryCtrl.value);
      this.collectCategoryId = collectCategory.id;
      let paymentAgency = this.paymentAgenciesResult.paymentAgencies.find(x => x.id == collectCategory.paymentAgencyId);
      this.paymentAgencyCodeCtrl.setValue(paymentAgency.code);
      this.paymentAgencyNameCtrl.setValue(paymentAgency.name);
      this.paymentAgency = paymentAgency;

      let paymentFileFormats = this.paymentFileFormatResult.paymentFileFormats;
      this.fileFormatNameCtrl.setValue(paymentFileFormats.find(x => x.id == paymentAgency.fileFormatId).name);
      this.selectedFileFormatId = paymentAgency.fileFormatId;

      if (paymentAgency.fileFormatId == AccountTransferFileFormatId.InternetJPBankFixed) {
        //ゆうちょFileFormatIdは再引落日を有効化する
        this.dueAtCtrl.enable();
        this.dueAt2ndCtrl.enable();

      }
      else{
        this.dueAtCtrl.enable();
      }

    }

    HtmlUtil.nextFocusByName(this.elementRef, 'dueAtFromCtrl', eventType);

  }

  public setOutputFilePath(eventType: string) {
    HtmlUtil.nextFocusByName(this.elementRef, 'dueAtFromCtrl', eventType);
  }

  public setDueAtFrom(eventType: string) {
    HtmlUtil.nextFocusByName(this.elementRef, 'dueAtToCtrl', eventType);
  }

  public setDueAtTo(eventType: string) {
    HtmlUtil.nextFocusByName(this.elementRef, 'dueAtCtrl', eventType);
  }

  public setDueAt(eventType: string) {
    HtmlUtil.nextFocusByName(this.elementRef, 'dueAt2ndCtrl', eventType);
  }

  public setDueAt2nd(eventType: string) {
    HtmlUtil.nextFocusByName(this.elementRef, 'cmbCollectCategoryCtrl', eventType);
  }

  public setCbxLog(index: number) {
    this.selectedAccountSummary = false;
    for (let i = 0; i < this.cbxLogCtrls.length; i++) {
      if (this.cbxLogCtrls[i].value) {
        this.selectedAccountSummary = true;
        break;
      }
    }
  }

  public isFormatInternetJPBankFixed(): boolean {
    return this.selectedFileFormatId != AccountTransferFileFormatId.InternetJPBankFixed;
  }

  public isDoAggregate(): boolean {
    return this.userInfoService.Company.transferAggregate == 1;
  }

  public aggregateByKey(source: AccountTransferDetail[]): AccountTransferDetail[] {
    let accountTransferDetails = source.reduce(function (result, current) {
      var element = result.find(function (p) {
        return p.transferBankCode == current.transferBankCode
          && p.transferBranchCode == current.transferBranchCode
          && p.transferAccountTypeId == current.transferAccountTypeId
          && p.transferAccountNumber == current.transferAccountNumber
          && p.transferCustomerCode == current.transferCustomerCode
          && p.accountTransferLogId == current.accountTransferLogId
      });

      if (element) {
        element.accountTransferLogId = current.accountTransferLogId;
        element.billingId = (element.billingId < current.billingId) ? element.billingId : current.billingId;
        element.companyId = current.companyId;
        element.departmentId = 0;
        element.staffId = 0;
        element.billedAt = null;
        element.salesAt = null;
        element.dueAt = null;
        element.billingAmount += current.billingAmount;
        element.incvoiceCode = "";
        element.note1 = "";
        element.transferBankCode = current.transferBankCode;
        element.transferBankName = (element.transferBankName > current.transferBankName) ? element.transferBankName : current.transferBankName;
        element.transferBranchCode = current.transferBranchCode;
        element.transferBranchName = (element.transferBranchName > current.transferBranchName) ? element.transferBranchName : current.transferBranchName;
        element.transferAccountTypeId = current.transferAccountTypeId;
        element.transferAccountNumber = current.transferAccountNumber;
        element.transferAccountName = (element.transferAccountName > current.transferAccountName) ? element.transferAccountName : current.transferAccountName;
        element.transferCustomerCode = current.transferCustomerCode;
        element.transferNewCode = (element.transferNewCode < current.transferNewCode) ? element.transferNewCode : current.transferNewCode;
        element.fileFormatId = (element.fileFormatId < current.fileFormatId) ? element.fileFormatId : current.fileFormatId;
        element.customerCode = (element.customerCode < current.customerCode) ? element.customerCode : current.customerCode;
      }
      else {
        result.push(current);
      }

      return result;
    }, []);

    for (let index = 0; index < accountTransferDetails.length; index++) {
      let tmpDetail = source.find(function (x) {
        return x.customerCode = accountTransferDetails[index].customerCode;
      });

      accountTransferDetails[index].id = index;
      accountTransferDetails[index].customerId = tmpDetail.customerId;
      accountTransferDetails[index].customerName = tmpDetail.customerName;
    }

    return accountTransferDetails;
  }

  public isValid(detail: AccountTransferDetail): boolean {

    const maxAmount: number = 9999999999;
    let bRtn =
      !StringUtil.IsNullOrEmpty(detail.transferBankCode)
      && !StringUtil.IsNullOrEmpty(detail.transferBankName)
      && !StringUtil.IsNullOrEmpty(detail.transferBranchCode)
      && !StringUtil.IsNullOrEmpty(detail.transferBranchName)
      && detail.transferAccountTypeId != 0
      && !StringUtil.IsNullOrEmpty(detail.transferAccountNumber)
      && !StringUtil.IsNullOrEmpty(detail.transferAccountName)
      && (!this.requiredCustomerCode(detail) || !StringUtil.IsNullOrEmpty(detail.transferCustomerCode))
      && detail.transferCustomerCode.length <= this.maxTransferCustomerCodeLength(detail)
      && this.validateNewCode(detail)
      && (0 < detail.billingAmount && detail.billingAmount < maxAmount);

    return bRtn;
  }

  public requiredCustomerCode(detail: AccountTransferDetail): boolean {

    return !(detail.fileFormatId == AccountTransferFileFormatId.ZenginCsv ||
      detail.fileFormatId == AccountTransferFileFormatId.ZenginFixed);

  }

  public maxTransferCustomerCodeLength(detail: AccountTransferDetail): number {

    return detail.fileFormatId == AccountTransferFileFormatId.ZenginCsv
      || detail.fileFormatId == AccountTransferFileFormatId.ZenginFixed
      || detail.fileFormatId == AccountTransferFileFormatId.MizuhoFactorWebFixed
      || detail.fileFormatId == AccountTransferFileFormatId.RisonaNetCsv
      || detail.fileFormatId == AccountTransferFileFormatId.InternetJPBankFixed
      || detail.fileFormatId == AccountTransferFileFormatId.ShinkinJohoService ? 20
      : detail.fileFormatId == AccountTransferFileFormatId.MitsubishiUfjFactorCsv
        || detail.fileFormatId == AccountTransferFileFormatId.RicohLeaseCollectCsv ? 15
        : detail.fileFormatId == AccountTransferFileFormatId.MizuhoFactorAspCsv ? 14
          : detail.fileFormatId == AccountTransferFileFormatId.SMBCFixed ? 12
            : detail.fileFormatId == AccountTransferFileFormatId.MitsubishiUfjNicosCsv ? 7
              : 0;
  }

  public validateNewCode(detail: AccountTransferDetail): boolean {

    return detail.fileFormatId == AccountTransferFileFormatId.InternetJPBankFixed
      ? true
      : this.newCodeAllowedCharacters(detail).includes(detail.transferNewCode);

  }

  public newCodeAllowedCharacters(detail: AccountTransferDetail): string[] {
    return detail.fileFormatId == AccountTransferFileFormatId.ZenginCsv
      || detail.fileFormatId == AccountTransferFileFormatId.ZenginFixed
      || detail.fileFormatId == AccountTransferFileFormatId.RisonaNetCsv
      || detail.fileFormatId == AccountTransferFileFormatId.ShinkinJohoService ? ["", "0", "1"]
      : detail.fileFormatId == AccountTransferFileFormatId.MitsubishiUfjFactorCsv
        || detail.fileFormatId == AccountTransferFileFormatId.MitsubishiUfjNicosCsv ? ["0", "1"]
        : detail.fileFormatId == AccountTransferFileFormatId.MizuhoFactorWebFixed
          || detail.fileFormatId == AccountTransferFileFormatId.MizuhoFactorAspCsv
          || detail.fileFormatId == AccountTransferFileFormatId.SMBCFixed
          || detail.fileFormatId == AccountTransferFileFormatId.RicohLeaseCollectCsv ? ["0", "1", "2"]
          : [];
  }

}
