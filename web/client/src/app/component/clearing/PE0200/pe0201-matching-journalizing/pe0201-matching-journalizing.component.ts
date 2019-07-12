import { Component, OnInit, ElementRef, ComponentFactoryResolver, ViewContainerRef, AfterViewInit } from '@angular/core';
import { BaseComponent } from 'src/app/component/common/base/base-component';
import { FormControl, Validators, FormGroup } from '@angular/forms';
import { HtmlUtil } from 'src/app/common/util/html.util';
import { StringUtil } from 'src/app/common/util/string-util';
import { TABLE_INDEX } from 'src/app/common/const/table-name.const';
import { ModalMasterComponent } from 'src/app/component/modal/modal-master/modal-master.component';
import { ModalOutputSettingComponent } from 'src/app/component/modal/modal-output-setting/modal-output-setting.component';
import { KEY_CODE, EVENT_TYPE, BUTTON_ACTION } from 'src/app/common/const/event.const';
import { JournalizingSummariesResult } from 'src/app/model/journalizing-summaries-result.model'
import { MODAL_STATUS_TYPE, PROCESS_RESULT_RESULT_TYPE } from 'src/app/common/const/status.const';
import { FormatterUtil } from 'src/app/common/util/formatter.util';
import { UserInfoService } from 'src/app/service/common/user-info.service';
import { NgbDateParserFormatter } from '@ng-bootstrap/ng-bootstrap';
import { NgbDateCustomParserFormatter } from 'src/app/common/util/dateformat-util';
import { CurrencyMasterService } from 'src/app/service/Master/currency-master.service';
import { JournalizingOption } from 'src/app/model/journalizing-option.model';
import { ColumnNameSettingMasterService } from 'src/app/service/Master/column-name-setting-master.service';
import { CollationSettingMasterService } from 'src/app/service/Master/collation-setting-master.service';
import { MatchingService } from 'src/app/service/matching.service';
import { ColumnNameSettingsResult } from 'src/app/model/column-name-settings-result.model';
import { CollationSettingResult } from 'src/app/model/collation-setting-result.model';
import { CurrenciesResult } from 'src/app/model/currencies-result.model';
import { MatchingJournalizingsResult } from 'src/app/model/matching-journalizings-result.model';
import { forkJoin } from 'rxjs';
import { DateUtil } from 'src/app/common/util/date-util';
import { FILE_EXTENSION } from 'src/app/common/const/eb-file.const';
import { MatchingJournalizing } from 'src/app/model/matching-journalizing.model';
import { ModalConfirmComponent } from 'src/app/component/modal/modal-confirm/modal-confirm.component';
import { MatchingJournalizingReportSource } from 'src/app/model/matching-journalizing-report-source.model';
import { FileUtil } from 'src/app/common/util/file.util';
import { ModalRouterProgressComponent } from 'src/app/component/modal/modal-router-progress/modal-router-progress.component';
import { LINE_FEED_CODE } from 'src/app/common/const/report-setting-kbn.const';
import { Router, ActivatedRoute, NavigationEnd } from '@angular/router';
import { PageUtil } from 'src/app/common/util/page-util';
import { ProcessResultService } from 'src/app/service/common/process-result.service';
import { MSG_WNG, MSG_ERR, MSG_INF, MSG_ITEM_NUM } from 'src/app/common/const/message.const';
import { LocalStorageItem } from 'src/app/model/custom-model/local-storage-item';
import { RangeSearchKey } from 'src/app/common/const/local-storage-key-const';
import { LocalStorageManageService } from 'src/app/service/common/local-storage-manage.service';
import { ExportFieldSettingMasterService } from 'src/app/service/Master/export-field-setting-master.service';
import { ExportFieldSetting } from 'src/app/model/export-field-setting.model';
import { ExportFieldSettingsResult } from 'src/app/model/export-field-settings-result.model';
import { CsvExportFileType } from 'src/app/common/const/kbn.const';
import { MatchedReceiptsResult } from 'src/app/model/matched-receipts-result.model';
import { ReceiptHelper } from 'src/app/model/helper/receipt-helper.model';

@Component({
  selector: 'app-pe0201-matching-journalizing',
  templateUrl: './pe0201-matching-journalizing.component.html',
  styleUrls: ['./pe0201-matching-journalizing.component.css'],
  providers: [
    { provide: NgbDateParserFormatter, useClass: NgbDateCustomParserFormatter }
  ]

})
export class Pe0201MatchingJournalizingComponent extends BaseComponent implements OnInit,AfterViewInit {

  public columnNameSettingsResult: ColumnNameSettingsResult;
  public collationSettingResult: CollationSettingResult;
  public currenciesResult: CurrenciesResult;
  public matchedReceiptsResult: MatchedReceiptsResult;
  public exportFieldsSettingsResult: ExportFieldSettingsResult;

  public journalizingSummariesResult: JournalizingSummariesResult;
  public matchingJournalizingsResult: MatchingJournalizingsResult;


  public clearedAtFromCtrl: FormControl;  // 消込日
  public clearedAtToCtrl: FormControl;

  public currencyCodeCtrl: FormControl;  // 通貨コード

  public cbxJournalSummaryCtrls: Array<FormControl>  // チェック

  public cbxExportMatchedReceiptDataCtrl: FormControl;      // 消込済入金データ出力
  public cbxContainAdvanceReceivedOccuredCtrl: FormControl;  // 前受発生を含める
  public cbxContainAdvanceReceivedMatchingCtrl: FormControl;  // 前受消込を含める

  public selectedJournalSummary: boolean; // 

  public extractionCount: number = 0;
  public extractionAmount: number = 0;

  public outputCount: number = 0;
  public outputAmount: number = 0;

  public UndefineCtrl: FormControl;

  public panelOpenState:boolean; // 検索パネル

  constructor(
    public router: Router,
    public activatedRoute: ActivatedRoute,
    public elementRef: ElementRef,
    public componentFactoryResolver: ComponentFactoryResolver,
    public viewContainerRef: ViewContainerRef,
    public userInfoService: UserInfoService,
    public matchingService:MatchingService,
    public columnNameSettingService:ColumnNameSettingMasterService,
    public collationSettingService:CollationSettingMasterService,
    public currencyService:CurrencyMasterService,
    public processResultService:ProcessResultService,
    public localStorageManageService:LocalStorageManageService,
    public exportFieldSettingMasterService:ExportFieldSettingMasterService,
    public receiptHelper: ReceiptHelper,
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

    this.columnNameSettingsResult = new ColumnNameSettingsResult();
    this.collationSettingResult = new CollationSettingResult();
    this.currenciesResult = new CurrenciesResult();

    let columnNameSettingsResponse = this.columnNameSettingService.Get(-1);
    let collationSettingResponse = this.collationSettingService.Get();
    let currencyResponse = this.currencyService.GetItems();

    forkJoin(
      columnNameSettingsResponse,
      collationSettingResponse,
      currencyResponse
    )
      .subscribe(responseList => {
        this.columnNameSettingsResult.columnNames = responseList[0];
        this.collationSettingResult.collationSetting = responseList[1];
        this.currenciesResult.currencies = responseList[2];
        this.getJournalizingSummariesResult();
      });

  }

  ngAfterViewInit(){
    HtmlUtil.nextFocusByName(this.elementRef, 'clearedAtFromCtrl', EVENT_TYPE.NONE);
  }

  public setControlInit() {

    this.clearedAtFromCtrl = new FormControl("", [Validators.maxLength(10)]);  // 請求日
    this.clearedAtToCtrl = new FormControl("", [Validators.maxLength(10)]);

    if (this.userInfoService.ApplicationControl.useForeignCurrency == 1) {
      this.currencyCodeCtrl = new FormControl("", [Validators.required, Validators.maxLength(3),]);  // 通貨コード
    }
    else {
      this.currencyCodeCtrl = new FormControl("", [Validators.maxLength(3),]);
    }

    this.cbxExportMatchedReceiptDataCtrl = new FormControl("");      // 消込済入金データ出力
    this.cbxContainAdvanceReceivedOccuredCtrl = new FormControl("");  // 前受発生を含める
    this.cbxContainAdvanceReceivedMatchingCtrl = new FormControl("");  // 前受消込を含める

    this.UndefineCtrl = new FormControl("");

  }

  public setValidator() {
    this.MyFormGroup = new FormGroup({
      clearedAtFromCtrl: this.clearedAtFromCtrl,  // 請求日
      clearedAtToCtrl: this.clearedAtToCtrl,
      currencyCodeCtrl: this.currencyCodeCtrl, // 通貨コード

      cbxExportMatchedReceiptDataCtrl: this.cbxExportMatchedReceiptDataCtrl, // 消込済入金データ出力
      cbxContainAdvanceReceivedOccuredCtrl: this.cbxContainAdvanceReceivedOccuredCtrl, // 前受発生を含める
      cbxContainAdvanceReceivedMatchingCtrl: this.cbxContainAdvanceReceivedMatchingCtrl, // 前受消込を含める

      UndefineCtrl: this.UndefineCtrl,
    });
  }

  public setFormatter() {
    // 通貨コード
    FormatterUtil.setAlphabetFormatter(this.currencyCodeCtrl);
  }

  public clear() {
    this.MyFormGroup.reset();
    // 初期化処理
    this.extractionCount = 0;
    this.extractionAmount = 0;
    this.outputCount = 0;
    this.outputAmount = 0;

    // 消込日
    this.clearedAtFromCtrl.enable();
    this.clearedAtToCtrl.enable();

    this.selectedJournalSummary = false;

    HtmlUtil.nextFocusByName(this.elementRef, 'clearedAtFromCtrl', EVENT_TYPE.NONE);

    this.panelOpenState=true;


    this.setRangeCheckbox();
  }

  public setRangeCheckbox() {
    let cbxMatchReceipt = this.localStorageManageService.get(RangeSearchKey.PE0201_MATCH_RECEIPT);
    let cbxAdvanceOccur = this.localStorageManageService.get(RangeSearchKey.PE0201_ADVANCE_RECEIPT_OCCUR);
    let cbxAdvanceMatch = this.localStorageManageService.get(RangeSearchKey.PE0201_ADVANCE_RECEIPT_MATCH);

    if (cbxMatchReceipt != null) {
      this.cbxExportMatchedReceiptDataCtrl.setValue(cbxMatchReceipt.value);
    }

    if (cbxAdvanceOccur != null) {
      this.cbxContainAdvanceReceivedOccuredCtrl.setValue(cbxAdvanceOccur.value);
    }

    if (cbxAdvanceMatch != null) {
      this.cbxContainAdvanceReceivedMatchingCtrl.setValue(cbxAdvanceMatch.value);
    }

    if ((cbxMatchReceipt == null) || (!cbxMatchReceipt.value)) {
      this.cbxContainAdvanceReceivedOccuredCtrl.disable();
      this.cbxContainAdvanceReceivedMatchingCtrl.disable();
    }
  }  

  public getJournalizingSummariesResult() {

    let journalizingOption = this.getJournalizingOption(false);

    journalizingOption.isOutputted = true;
    journalizingOption.recordedAtFrom = null;
    journalizingOption.recordedAtTo = null;

    this.journalizingSummariesResult = new JournalizingSummariesResult();
    this.matchingService.GetMatchingJournalizingSummary(journalizingOption)
      .subscribe(respnse => {
        this.journalizingSummariesResult.journalizingsSummaries = respnse;

        this.cbxJournalSummaryCtrls = new Array(this.journalizingSummariesResult.journalizingsSummaries.length);

        for (let i = 0; i < this.cbxJournalSummaryCtrls.length; i++) {
          this.cbxJournalSummaryCtrls[i] = new FormControl(null);
        }

        for (let i = 0; i < this.cbxJournalSummaryCtrls.length; i++) {
          this.MyFormGroup.removeControl("cbxJournalSummaryCtrl" + i);
          this.MyFormGroup.addControl("cbxJournalSummaryCtrl" + i, this.cbxJournalSummaryCtrls[i]);
        }
      });

  }


  public getJournalizingOption(reAction: boolean): JournalizingOption {
    let journalizingOption = new JournalizingOption();

    journalizingOption.companyId = this.userInfoService.Company.id;
    const currencyCode = this.userInfoService.ApplicationControl.useForeignCurrency == 1 ? this.currencyCodeCtrl.value : 'JPY';
    const currency = this.currenciesResult.currencies.filter(x => x.code == currencyCode)[0];
    if (currency !== undefined) {
      journalizingOption.currencyId = currency.id;
    }
    //journalizingOption.customerId
    journalizingOption.isOutputted = false;
    journalizingOption.loginUserId = this.userInfoService.LoginUser.id;
    //journalizingOption.updateAt
    journalizingOption.recordedAtFrom = DateUtil.ConvertFromDatepicker(this.clearedAtFromCtrl);
    journalizingOption.recordedAtTo = DateUtil.ConvertFromDatepicker(this.clearedAtToCtrl);
    journalizingOption.outputAt = new Array<string>();
    journalizingOption.useDiscount = (this.userInfoService.ApplicationControl.useDiscount == 1);
    // ファクタリング
    journalizingOption.containAdvanceReceivedOccured = this.cbxContainAdvanceReceivedMatchingCtrl.value ? true : false;
    journalizingOption.containAdvanceReceivedMatching = this.cbxContainAdvanceReceivedOccuredCtrl.value ? true : false;
    //journalizingOption.createAtFrom
    //journalizingOption.createAtTo
    //journalizingOption.journalizingTypes
    journalizingOption.isGeneral = (this.collationSettingResult.collationSetting.journalizingPattern == 1);
    //journalizingOption.outputCustoemrName
    //journalizingOption.precision    

    if (reAction) {
      for (let index = 0; index < this.journalizingSummariesResult.journalizingsSummaries.length; index++) {
        if (this.cbxJournalSummaryCtrls[index].value) {
          journalizingOption.outputAt.push(this.journalizingSummariesResult.journalizingsSummaries[index].outputAt);
        }
      }

      journalizingOption.recordedAtFrom = null;
      journalizingOption.recordedAtTo = null;

      journalizingOption.isOutputted = true;

    }

    return journalizingOption;
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
        this.outputCsv(false);
        break;
      case BUTTON_ACTION.REEXPORT:
        this.outputCsv(true);
        break;
      case BUTTON_ACTION.HISTORY_DELETE:
        this.historyDelete();
        break;
      case BUTTON_ACTION.SETTING:
        this.openOutputSettingModal()
        break;  

      default:
        console.log('buttonAction Error.');
        break;
    }
    
  }    

  public extract() {

    this.processCustomResult = this.processResultService.clearProcessCustomMsg(this.processCustomResult);

    let isStandardPattern: boolean = this.collationSettingResult.collationSetting.journalizingPattern == 0;

    if (isStandardPattern) {

      let componentFactory = this.componentFactoryResolver.resolveComponentFactory(ModalRouterProgressComponent);
      let modalRouterProgressComponentRef = this.viewContainerRef.createComponent(componentFactory);
      //modalRouterProgressComponentRef.destroy();

      let journalizingOption = this.getJournalizingOption(false);

      this.matchingService.ExtractMatchingJournalizing(journalizingOption)
        .subscribe(response => {

          this.processResultService.processAtGetData(this.processCustomResult, response, true, this.partsResultMessageComponent);

          this.matchingJournalizingsResult = new MatchingJournalizingsResult();
          this.matchingJournalizingsResult.matchingJournalizings = response;

          if(response != PROCESS_RESULT_RESULT_TYPE.FAILURE && response.length==0){
            this.extractionCount = 0;
            this.extractionAmount = 0;
            this.outputCount = 0;
            this.outputAmount = 0;

          }
          else {
            this.extractionCount = 0;
            this.extractionAmount = 0;
            this.matchingJournalizingsResult.matchingJournalizings.forEach(element => {
              this.extractionCount++;
              this.extractionAmount += element.amount;
            });
            this.outputCount = 0;
            this.outputAmount = 0;

            this.clearedAtFromCtrl.disable();  // 入金日
            this.clearedAtToCtrl.disable();

          }
          modalRouterProgressComponentRef.destroy();
        });
    }
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
    this.processCustomResult = this.processResultService.clearProcessCustomMsg(this.processCustomResult);


    let componentFactory = this.componentFactoryResolver.resolveComponentFactory(ModalRouterProgressComponent);
    let componentRef = this.viewContainerRef.createComponent(componentFactory);
    componentRef.instance.Closing.subscribe(() => {
      componentRef.destroy();
    });


    let matchingJournalizingReportSource = new MatchingJournalizingReportSource();

    matchingJournalizingReportSource.companyId = this.userInfoService.Company.id;
    matchingJournalizingReportSource.reOutput = rePrint;

    if (rePrint) {
      if (this.userInfoService.ApplicationControl.useAuthorization == 1) {
        matchingJournalizingReportSource.items = this.matchingJournalizingsResult.matchingJournalizings.filter(journalizing => {
          if (journalizing.approved) {
            true;
          }
          else {
            false;
          }
        });
      }
      else {

        let journalizingOption = this.getJournalizingOption(rePrint);

        this.matchingService.ExtractMatchingJournalizing(journalizingOption)
          .subscribe(response => {
            let reprintJournalizingsResult = new MatchingJournalizingsResult();
            reprintJournalizingsResult.matchingJournalizings = response;

            if (response.length == 0) {

              this.processCustomResult = this.processResultService.processAtWarning(
                this.processCustomResult, MSG_WNG.NOT_EXIST_SEARCH_DATA, this.partsResultMessageComponent);
            }
            else {
              matchingJournalizingReportSource.items = reprintJournalizingsResult.matchingJournalizings;
            }
          });


      }

    }
    else {
      if (this.userInfoService.ApplicationControl.useAuthorization == 1) {
        matchingJournalizingReportSource.items = this.matchingJournalizingsResult.matchingJournalizings.filter(journalizing => {
          if (journalizing.approved) {
            true;
          }
          else {
            false;
          }
        });
      }
      else {
        matchingJournalizingReportSource.items = this.matchingJournalizingsResult.matchingJournalizings;
      }
    }

    this.matchingService.GetJournalizingReport(matchingJournalizingReportSource)
      .subscribe(response => {
        if (response != undefined) {
          FileUtil.download([response.body], this.Title + DateUtil.getYYYYMMDD(0), FILE_EXTENSION.PDF);
        }
        else {
          this.processCustomResult = this.processResultService.processAtFailure(
            this.processCustomResult, MSG_ERR.SOMETHING_ERROR.replace(MSG_ITEM_NUM.FIRST, '消込仕訳の印刷'),
            this.partsResultMessageComponent);

        }
        componentRef.destroy();

      });

  }

  public outputCsv(reOutput: boolean) {


    let componentFactory = this.componentFactoryResolver.resolveComponentFactory(ModalRouterProgressComponent);
    let modalRouterProgressComponentRef = this.viewContainerRef.createComponent(componentFactory);
    //modalRouterProgressComponentRef.destroy();

    let searchJournalizingOption = this.getJournalizingOption(reOutput);

    this.matchingService.GetMatchingJournalizingSummary(searchJournalizingOption)
      .subscribe(response => {
        if(response!=PROCESS_RESULT_RESULT_TYPE.FAILURE){
          let csvJournalizingSummariesResult = new JournalizingSummariesResult();
          let csvDetailMatchingJournalizings = new MatchingJournalizingsResult();

          csvJournalizingSummariesResult.journalizingsSummaries = response;

          // 抽出したデータとサマリーしたデータの比較
          let csvAmount = 0;


          if (!reOutput) {
            csvJournalizingSummariesResult.journalizingsSummaries.forEach(element => {
              csvAmount += element.amount;
            });
            if (this.extractionAmount != csvAmount) {
              this.processCustomResult = this.processResultService.processAtWarning(
                this.processCustomResult, MSG_WNG.NOT_EQUAL_ABSTRACT_AMOUNT_AND_UPDATE_AMOUNT, this.partsResultMessageComponent);

              return;
            }

            this.ExportJournalizing(this.matchingJournalizingsResult.matchingJournalizings);

            if (this.cbxExportMatchedReceiptDataCtrl.value) {
              this.getAndExportReceiptMatchedJournalizing();
            }

            // フラグの設定
            let updateJournalizingOption = this.getJournalizingOption(reOutput);
            this.matchingService.UpdateOutputAt(updateJournalizingOption)
              .subscribe(response => {
                if(response!=PROCESS_RESULT_RESULT_TYPE.FAILURE){
                  this.processCustomResult = this.processResultService.processAtSuccess(
                    this.processCustomResult, MSG_INF.PROCESS_FINISH, this.partsResultMessageComponent);
                  this.processResultService.createdLog(this.processCustomResult.logData);
                  
                  this.outputAmount = this.extractionAmount;
                  this.outputCount = this.extractionCount;
                  this.getJournalizingSummariesResult();
                }
                else {
                  this.processCustomResult = this.processResultService.processAtFailure(
                    this.processCustomResult, MSG_ERR.SOMETHING_ERROR.replace(MSG_ITEM_NUM.FIRST, 'CSVの出力'),
                    this.partsResultMessageComponent);
                }
              });


          }
          else {
            let journalizingOption = this.getJournalizingOption(true);

            this.matchingService.ExtractMatchingJournalizing(journalizingOption)
              .subscribe(response => {

                if(response!=PROCESS_RESULT_RESULT_TYPE.FAILURE){

                  this.processCustomResult = this.processResultService.processAtSuccess(
                    this.processCustomResult, MSG_INF.PROCESS_FINISH, this.partsResultMessageComponent);
                  this.processResultService.createdLog(this.processCustomResult.logData);

                  csvDetailMatchingJournalizings = new MatchingJournalizingsResult();
                  csvDetailMatchingJournalizings.matchingJournalizings = response;

                  // CSVの出力
                  this.ExportJournalizing(csvDetailMatchingJournalizings.matchingJournalizings);

                  if (this.cbxExportMatchedReceiptDataCtrl.value) {
                    this.getAndExportReceiptMatchedJournalizing(journalizingOption);
                  }

                }
                else{
                  this.processResultService.processAtFailure(
                    this.processCustomResult,MSG_ERR.SOMETHING_ERROR.replace("{0}","再出力"),
                    this.partsResultMessageComponent);
                }
              });
          }
        }
        else {
          this.processCustomResult = this.processResultService.processAtFailure(
            this.processCustomResult, MSG_ERR.SOMETHING_ERROR.replace(MSG_ITEM_NUM.FIRST, 'CSVの出力'),
            this.partsResultMessageComponent);
        }
        modalRouterProgressComponentRef.destroy();
      });
  }



  public ExportJournalizing(matchingJournalizings: Array<MatchingJournalizing>) {

    let data: string = "";
    let dataList = matchingJournalizings;


    matchingJournalizings.forEach(matchingJournalizing => {

      let dataItem: Array<any> = [];

      dataItem.push(this.userInfoService.Company.code);
      dataItem.push(DateUtil.convertDateString(matchingJournalizing.recordedAt.substr(0, 10)));
      dataItem.push(matchingJournalizing.slipNumber);
      dataItem.push(matchingJournalizing.debitDepartmentCode);
      dataItem.push(matchingJournalizing.debitDepartmentName);
      dataItem.push(matchingJournalizing.debitAccountTitleCode);
      dataItem.push(matchingJournalizing.debitAccountTitleName);
      dataItem.push(matchingJournalizing.debitSubCode);
      dataItem.push(matchingJournalizing.debitSubName);
      dataItem.push(matchingJournalizing.creditDepartmentCode);
      dataItem.push(matchingJournalizing.creditDepartmentName);
      dataItem.push(matchingJournalizing.creditAccountTitleCode);
      dataItem.push(matchingJournalizing.creditAccountTitleName);
      dataItem.push(matchingJournalizing.creditSubCode);
      dataItem.push(matchingJournalizing.creditSubName);
      dataItem.push(matchingJournalizing.amount);
      dataItem.push(matchingJournalizing.note);
      dataItem.push(matchingJournalizing.customerCode);
      dataItem.push(matchingJournalizing.customerName);
      dataItem.push(matchingJournalizing.invoiceCode);
      dataItem.push(matchingJournalizing.staffCode);
      dataItem.push(matchingJournalizing.payerCode);
      dataItem.push(matchingJournalizing.payerName);
      dataItem.push(matchingJournalizing.sourceBankName);
      dataItem.push(matchingJournalizing.sourceBranchName);
      //dataItem.push(matchingJournalizing.dueAt);
      dataItem.push(matchingJournalizing.bankCode);
      dataItem.push(matchingJournalizing.bankName);
      dataItem.push(matchingJournalizing.branchCode);
      dataItem.push(matchingJournalizing.branchName);
      dataItem.push(matchingJournalizing.accountTypeId);
      dataItem.push(matchingJournalizing.accountNumber);
      dataItem.push(matchingJournalizing.taxClassId);
      if (this.userInfoService.ApplicationControl.useForeignCurrency == 1) {
        dataItem.push(matchingJournalizing.currencyCode);
      }
      dataItem = FileUtil.encloseItemBySymbol(dataItem);
      data = data + dataItem.join(",") + LINE_FEED_CODE;

    });

    let resultDatas: Array<any> = [];
    resultDatas.push(data);
    FileUtil.download(resultDatas, this.Title + DateUtil.getYYYYMMDD(0), FILE_EXTENSION.CSV);
  }

  public getAndExportReceiptMatchedJournalizing(journalizingOption: JournalizingOption = null) {

    if (journalizingOption == null) {
      journalizingOption = this.getJournalizingOption(false);
    }

    let setting = new ExportFieldSetting();
    setting.companyId = this.userInfoService.Company.id;
    setting.exportFileType = CsvExportFileType.MatchedReceiptData;

    let exportFieldSettingResponse = this.exportFieldSettingMasterService.GetItems(setting);
    let receiptMatchedJournalizingResponse = this.matchingService.GetMatchedReceipt(journalizingOption);

    forkJoin(
      exportFieldSettingResponse,
      receiptMatchedJournalizingResponse,
    )
      .subscribe(responseList => {
        this.exportFieldsSettingsResult = new ExportFieldSettingsResult();
        this.matchedReceiptsResult = new MatchedReceiptsResult();
        this.exportFieldsSettingsResult.exportFieldSettings = responseList[0];
        this.matchedReceiptsResult.matchedReceipts = responseList[1];

        this.exportReceiptMatchedJournalizing();

      });

  }

  public exportReceiptMatchedJournalizing() {
    if (this.matchedReceiptsResult.matchedReceipts.length == 0) return;

    let fileName: string = "消込済入金データ";
    let data: string = "";
    const settings = this.exportFieldsSettingsResult.exportFieldSettings.filter(item => { return item.allowExport === 1 });

    let isRequireHeader = settings.some(x => x.columnName === "RequireHeader" && x.allowExport === 1);
    if (isRequireHeader) {
      let dataItem: Array<any> = [];
      settings.filter(x => x.columnName != "RequireHeader").forEach(item => {
        dataItem.push(item.caption);
      });
      dataItem = FileUtil.encloseItemBySymbol(dataItem);
      data = data + dataItem.join(",") + LINE_FEED_CODE;
    }

    this.matchedReceiptsResult.matchedReceipts.forEach(matchedReceipt => {
      let dataItem: Array<any> = [];
      settings.filter(x => x.columnName != "RequireHeader").forEach(setting => {
        dataItem.push(this.receiptHelper.getExportValue(matchedReceipt, setting));
      });
      dataItem = FileUtil.encloseItemBySymbol(dataItem);
      data = data + dataItem.join(",") + LINE_FEED_CODE;
    });

    let resultDatas: Array<any> = [];
    resultDatas.push(data);
    FileUtil.download(resultDatas, fileName + DateUtil.getYYYYMMDD(0), FILE_EXTENSION.CSV);
    
  }

  public historyDelete() {
    this.processCustomResult = this.processResultService.clearProcessCustomMsg(this.processCustomResult);;
    let componentFactory = this.componentFactoryResolver.resolveComponentFactory(ModalConfirmComponent);
    let componentRef = this.viewContainerRef.createComponent(componentFactory);
    componentRef.instance.ActionName = "選択した仕訳の履歴削除"
    componentRef.instance.Closing.subscribe(() => {

      if (componentRef.instance.ModalStatus == MODAL_STATUS_TYPE.OK) {

        let componentFactory = this.componentFactoryResolver.resolveComponentFactory(ModalRouterProgressComponent);
        let modalRouterProgressComponentRef = this.viewContainerRef.createComponent(componentFactory);
        //modalRouterProgressComponentRef.destroy();

        let journalizingOption = this.getJournalizingOption(true);

        this.matchingService.CancelMatchingJournalizing(journalizingOption)
          .subscribe(response => {
            if(response!=PROCESS_RESULT_RESULT_TYPE.FAILURE){
              this.clear();
              this.getJournalizingSummariesResult();

              this.processResultService.processAtSuccess(
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
      else{
        this.processResultService.processAtWarning(
          this.processCustomResult, MSG_WNG.PROCESS_CANCELED, this.partsResultMessageComponent);
      }

      componentRef.destroy();
    });
  }

  public openMasterModal(table: TABLE_INDEX, ) {

    let componentFactory = this.componentFactoryResolver.resolveComponentFactory(ModalMasterComponent);
    let componentRef = this.viewContainerRef.createComponent(componentFactory);

    componentRef.instance.TableIndex = table;

    componentRef.instance.Closing.subscribe(() => {

      if (componentRef.instance.ModalStatus == MODAL_STATUS_TYPE.SELECT) {
        switch (table) {
          case TABLE_INDEX.MASTER_CURRENCY:
            {
              this.currencyCodeCtrl.setValue(componentRef.instance.SelectedCode);
              break;
            }
        }
      }
      componentRef.destroy();
    });
}

  public openOutputSettingModal() {
    let componentFactory = this.componentFactoryResolver.resolveComponentFactory(ModalOutputSettingComponent);
    let componentRef = this.viewContainerRef.createComponent(componentFactory);
    componentRef.instance.Title = this.Title;
    componentRef.instance.processModalCustomResult = this.processModalCustomResult;
    componentRef.instance.exportFileType = CsvExportFileType.MatchedReceiptData;
    componentRef.instance.Closing.subscribe(() => {
      componentRef.destroy();
    });
  }

  ///////////////////////////////////////////////////////////////////////
  public setClearedAtFrom(eventType: string) {
    HtmlUtil.nextFocusByName(this.elementRef, 'clearedAtToCtrl', eventType);
  }

  public setClearedAtTo(eventType: string) {
    HtmlUtil.nextFocusByName(this.elementRef, 'currencyCodeCtrl', eventType);
  }

  ///////////////////////////////////////////////////////////////////////
  public setCurrencyCode(eventType: string) {

    if (!StringUtil.IsNullOrEmpty(this.currencyCodeCtrl.value)) {

      this.currencyService.GetItems(this.currencyCodeCtrl.value)
        .subscribe(response => {
          if (response != undefined && response.length > 0) {
            this.currencyCodeCtrl.setValue(response[0].code);
          }
          else {
            this.currencyCodeCtrl.setValue("");
          }
        });
    }
    else {
      this.currencyCodeCtrl.setValue("");
    }
  }

  public setCbxJournalSummary(index: number) {
    this.selectedJournalSummary = false;
    for (let i = 0; i < this.cbxJournalSummaryCtrls.length; i++) {
      if (this.cbxJournalSummaryCtrls[i].value) {
        this.selectedJournalSummary = true;
        break;
      }
    }
  }

  public setCbxExportMatchedReceiptData(eventType: string) {
    let localstorageItem = new LocalStorageItem();
    localstorageItem.key = RangeSearchKey.PE0201_MATCH_RECEIPT;
    localstorageItem.value = this.cbxExportMatchedReceiptDataCtrl.value;
    this.localStorageManageService.set(localstorageItem);

    if (!this.cbxExportMatchedReceiptDataCtrl.value) {
      this.cbxContainAdvanceReceivedOccuredCtrl.setValue(false);
      this.setCbxContainAdvanceReceivedOccured("");
      this.cbxContainAdvanceReceivedOccuredCtrl.disable();
      this.cbxContainAdvanceReceivedMatchingCtrl.setValue(false);
      this.setCbxContainAdvanceReceivedMatching("");
      this.cbxContainAdvanceReceivedMatchingCtrl.disable();
    }
    else {
      this.cbxContainAdvanceReceivedOccuredCtrl.enable();
      this.cbxContainAdvanceReceivedMatchingCtrl.enable();
    }

    HtmlUtil.nextFocusByName(this.elementRef, "receiptCategoryCodeFromCtrl", eventType);
  }


  public setCbxContainAdvanceReceivedOccured(eventType: string) {
    let localstorageItem = new LocalStorageItem();
    localstorageItem.key = RangeSearchKey.PE0201_ADVANCE_RECEIPT_OCCUR;
    localstorageItem.value = this.cbxContainAdvanceReceivedOccuredCtrl.value;
    this.localStorageManageService.set(localstorageItem);

    HtmlUtil.nextFocusByName(this.elementRef, "receiptCategoryCodeFromCtrl", eventType);
  }

  public setCbxContainAdvanceReceivedMatching(eventType: string) {
    let localstorageItem = new LocalStorageItem();
    localstorageItem.key = RangeSearchKey.PE0201_ADVANCE_RECEIPT_MATCH;
    localstorageItem.value = this.cbxContainAdvanceReceivedMatchingCtrl.value;
    this.localStorageManageService.set(localstorageItem);

    HtmlUtil.nextFocusByName(this.elementRef, "receiptCategoryCodeFromCtrl", eventType);
  }  
}
