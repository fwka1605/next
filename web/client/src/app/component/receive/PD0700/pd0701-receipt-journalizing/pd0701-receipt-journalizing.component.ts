import { Component, OnInit, ElementRef, ComponentFactoryResolver, ViewContainerRef, AfterViewInit } from '@angular/core';
import { JournalizingSummariesResult } from 'src/app/model/journalizing-summaries-result.model';
import { FormControl, Validators, FormGroup } from '@angular/forms';
import { ReceiptService } from 'src/app/service/receipt.service';
import { BaseComponent } from 'src/app/component/common/base/base-component';
import { TABLE_INDEX } from 'src/app/common/const/table-name.const';
import { ModalMasterComponent } from 'src/app/component/modal/modal-master/modal-master.component';
import { HtmlUtil } from 'src/app/common/util/html.util';
import { StringUtil } from 'src/app/common/util/string-util';
import { KEY_CODE, EVENT_TYPE, BUTTON_ACTION } from 'src/app/common/const/event.const';
import { FormatterUtil } from 'src/app/common/util/formatter.util';
import { MODAL_STATUS_TYPE, PROCESS_RESULT_RESULT_TYPE } from 'src/app/common/const/status.const';
import { UserInfoService } from 'src/app/service/common/user-info.service';
import { NgbDateParserFormatter } from '@ng-bootstrap/ng-bootstrap';
import { NgbDateCustomParserFormatter } from 'src/app/common/util/dateformat-util';
import { CurrencyMasterService } from 'src/app/service/Master/currency-master.service';
import { JournalizingOption } from 'src/app/model/journalizing-option.model';
import { DateUtil } from 'src/app/common/util/date-util';
import { ReceiptJournalizingsResult } from 'src/app/model/receipt-journalizings-result.model';
import { CurrenciesResult } from 'src/app/model/currencies-result.model';
import { ColumnNameSettingsResult } from 'src/app/model/column-name-settings-result.model';
import { CollationSettingResult } from 'src/app/model/collation-setting-result.model';
import { ColumnNameSettingMasterService } from 'src/app/service/Master/column-name-setting-master.service';
import { CollationSettingMasterService } from 'src/app/service/Master/collation-setting-master.service';
import { forkJoin } from 'rxjs';
import { FreeImporterFormatType } from 'src/app/common/common-const';
import { ModalConfirmComponent } from 'src/app/component/modal/modal-confirm/modal-confirm.component';
import { ReceiptJournalizing } from 'src/app/model/receipt-journalizing.model';
import { FILE_EXTENSION } from 'src/app/common/const/eb-file.const';
import { FileUtil } from 'src/app/common/util/file.util';
import { LINE_FEED_CODE } from 'src/app/common/const/report-setting-kbn.const';
import { ProcessResultService } from 'src/app/service/common/process-result.service';
import { Router, ActivatedRoute, NavigationEnd } from '@angular/router';
import { PageUtil } from 'src/app/common/util/page-util';
import { MSG_ERR, MSG_INF, MSG_WNG, MSG_ITEM_NUM } from 'src/app/common/const/message.const';
import { ModalRouterProgressComponent } from 'src/app/component/modal/modal-router-progress/modal-router-progress.component';

@Component({
  selector: 'app-pd0701-receipt-journalizing',
  templateUrl: './pd0701-receipt-journalizing.component.html',
  styleUrls: ['./pd0701-receipt-journalizing.component.css'],
  providers: [
    { provide: NgbDateParserFormatter, useClass: NgbDateCustomParserFormatter }
  ]

})
export class Pd0701ReceiptJournalizingComponent extends BaseComponent implements OnInit,AfterViewInit {

  public panelOpenState: boolean;

  public columnNameSettingsResult:ColumnNameSettingsResult;
  public collationSettingResult:CollationSettingResult;
  public currenciesResult:CurrenciesResult;;
  
  public journalizingSummariesResult:JournalizingSummariesResult;
  public receiptJournalizingsResult:ReceiptJournalizingsResult;


  public recordedAtFromCtrl: FormControl;  // 入金日
  public recordedAtToCtrl: FormControl;

  public currencyCodeCtrl:FormControl;  // 通貨コード

  public cbxJournalSummaryCtrls: Array<FormControl>  // チェック

  public selectedJournalSummary:boolean; // 

  public extractionCount: number = 123456;
  public extractionAmount: number = 123456;

  public outputCount: number = 123456;
  public outputAmount: number = 123456;

  public UndefineCtrl: FormControl;

  constructor(
    public router: Router,
    public activatedRoute: ActivatedRoute,
    public elementRef: ElementRef,
    public componentFactoryResolver: ComponentFactoryResolver,
    public viewContainerRef: ViewContainerRef,
    public userInfoService: UserInfoService,
    public receiptService:ReceiptService,
    public columnNameSettingService:ColumnNameSettingMasterService,
    public collationSettingService:CollationSettingMasterService,
    public currencyService:CurrencyMasterService,
    public processResultService:ProcessResultService

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

    let columnNameSettingsResponse = this.columnNameSettingService.Get(FreeImporterFormatType.Receipt);
    let collationSettingResponse = this.collationSettingService.Get();
    let currencyResponse = this.currencyService.GetItems('JPY');

    

    forkJoin(
      columnNameSettingsResponse,
      collationSettingResponse,
      currencyResponse
    )
      .subscribe(responseList=>{
        this.columnNameSettingsResult.columnNames=responseList[0];
        this.collationSettingResult.collationSetting=responseList[1];
        this.currenciesResult.currencies=responseList[2];
        this.getJournalizingSummariesResult();
      });
  }

  ngAfterViewInit(){
    HtmlUtil.nextFocusByName(this.elementRef, 'recordedAtFromCtrl', EVENT_TYPE.NONE);
  }

  public setControlInit() {


    this.recordedAtFromCtrl = new FormControl("", [Validators.maxLength(10)]);  // 請求日
    this.recordedAtToCtrl = new FormControl("", [Validators.maxLength(10)]);

    this.currencyCodeCtrl = new FormControl("", [Validators.maxLength(3)]);  // 通貨コード

    this.UndefineCtrl = new FormControl("");
  }

  public setValidator() {
    this.MyFormGroup = new FormGroup({
      recordedAtFromCtrl: this.recordedAtFromCtrl,  // 請求日
      currencyCodeCtrl: this.currencyCodeCtrl, // 通貨コード
      recordedAtToCtrl: this.recordedAtToCtrl,
      UndefineCtrl: this.UndefineCtrl,
    });


  }

  public setFormatter(){
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

    // 入金日
    this.recordedAtFromCtrl.enable();
    this.recordedAtToCtrl.enable();

    this.selectedJournalSummary=false;

    HtmlUtil.nextFocusByName(this.elementRef, 'recordedAtFromCtrl', EVENT_TYPE.NONE);

    this.panelOpenState = true;

  }

  public getJournalizingSummariesResult(){

    let journalizingOption = new JournalizingOption();
    journalizingOption.companyId= this.userInfoService.Company.id;
    journalizingOption.currencyId= this.currenciesResult.currencies[0].id;
    journalizingOption.isOutputted= true;


    this.journalizingSummariesResult = new JournalizingSummariesResult();
    this.receiptService.GetReceiptJournalizingSummary(journalizingOption)
      .subscribe(respnse=>{
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
      case BUTTON_ACTION.HISTORY_DELETE:
        this.historyDelete();
        break;


      default:
        console.log('buttonAction Error.');
        break;
    }
    
  }  


  public extract() {
    let isStandardPattern:boolean = this.collationSettingResult.collationSetting.journalizingPattern == 0;

    if(isStandardPattern){
      let journalizingOption = this.getJournalizingOption(false);

      let componentFactory = this.componentFactoryResolver.resolveComponentFactory(ModalRouterProgressComponent);
      let modalRouterProgressComponentRef = this.viewContainerRef.createComponent(componentFactory);
      //modalRouterProgressComponentRef.destroy();

      this.receiptService.ExtractReceiptJournalizingAsync(journalizingOption)
        .subscribe(response=>{
          this.processResultService.processAtGetData(
            this.processCustomResult, response, true, this.partsResultMessageComponent);

          this.receiptJournalizingsResult = new ReceiptJournalizingsResult();
          this.receiptJournalizingsResult.receiptJournalizings=response;
  
          if(response.length==0){
            this.extractionCount = 0;
            this.extractionAmount = 0;
            this.outputCount = 0;
            this.outputAmount = 0;

          }
          else{
            this.extractionCount = 0;
            this.extractionAmount = 0;  
            this.receiptJournalizingsResult.receiptJournalizings.forEach(element=>{
              this.extractionCount++;
              this.extractionAmount += element.amount;
            });
            this.outputCount = 0;
            this.outputAmount = 0;
  
            // 
            this.recordedAtFromCtrl.disable();  // 入金日
            this.recordedAtToCtrl.disable();
          }

          modalRouterProgressComponentRef.destroy();          
        });
    }
  }

  public getJournalizingOption(reAction:boolean):JournalizingOption{
    let journalizingOption = new JournalizingOption();

    journalizingOption.companyId= this.userInfoService.Company.id;
    journalizingOption.currencyId = this.currenciesResult.currencies[0].id;
    //journalizingOption.customerId
    journalizingOption.isOutputted=false;
    journalizingOption.loginUserId=this.userInfoService.LoginUser.id;
    //journalizingOption.updateAt
    journalizingOption.recordedAtFrom = DateUtil.ConvertFromDatepicker(this.recordedAtFromCtrl);
    journalizingOption.recordedAtTo = DateUtil.ConvertFromDatepicker(this.recordedAtToCtrl);
    journalizingOption.outputAt = new Array<string>();
    journalizingOption.useDiscount = (this.userInfoService.ApplicationControl.useDiscount==1);
    //journalizingOption.containAdvanceReceivedOccured
    //journalizingOption.containAdvanceReceivedMatching
    //journalizingOption.createAtFrom
    //journalizingOption.createAtTo
    //journalizingOption.journalizingTypes
    journalizingOption.isGeneral = (this.collationSettingResult.collationSetting.journalizingPattern==1);
    //journalizingOption.outputCustoemrName
    //journalizingOption.precision    

    if(reAction){
      for(let index=0;index<this.journalizingSummariesResult.journalizingsSummaries.length;index++){
        if(this.cbxJournalSummaryCtrls[index].value){
          journalizingOption.outputAt.push(this.journalizingSummariesResult.journalizingsSummaries[index].outputAt);
        }
      }

      journalizingOption.recordedAtFrom=null;
      journalizingOption.recordedAtTo = null;

      journalizingOption.isOutputted= true;

    }

    return journalizingOption;
  }



  public print(){
    this.printPdf(false);
  }

  public rePrint(){
    this.printPdf(true);
  }

  public printPdf(rePrint:boolean){

    let journalizingOption = this.getJournalizingOption(rePrint);

    this.receiptService.GetJournalizingReport(journalizingOption)
    .subscribe(response=>{
      if(response !=this.PROCESS_RESULT_RESULT_TYPE.FAILURE){
        this.processResultService.processAtFailure(
          this.processCustomResult, MSG_ERR.SOMETHING_ERROR.replace(MSG_ITEM_NUM.FIRST, "印刷"), this.partsResultMessageComponent);
        FileUtil.download([response.body], this.Title + DateUtil.getYYYYMMDD(0), FILE_EXTENSION.PDF);
      }
      else{
        this.processResultService.processAtFailure(
          this.processCustomResult, MSG_ERR.SOMETHING_ERROR.replace(MSG_ITEM_NUM.FIRST, "印刷"), this.partsResultMessageComponent);
  }
    });

  }



  public output() {
    this.outputCsv(false);
  }


  public reOutput() {
    this.outputCsv(true);
  }

  public outputCsv(reOutput:boolean){

    let componentFactory = this.componentFactoryResolver.resolveComponentFactory(ModalRouterProgressComponent);
    let modalRouterProgressComponentRef = this.viewContainerRef.createComponent(componentFactory);
    //modalRouterProgressComponentRef.destroy();


    let searchJournalizingOption = this.getJournalizingOption(reOutput);    

    this.receiptService.GetReceiptJournalizingSummary(searchJournalizingOption)
      .subscribe(response=>{

        if(response!=PROCESS_RESULT_RESULT_TYPE.FAILURE){
          let csvJournalizingSummariesResult = new JournalizingSummariesResult();
          let csvDetailReceiptJournalizings = new ReceiptJournalizingsResult();

          csvJournalizingSummariesResult.journalizingsSummaries = response;

          // 抽出したデータとサマリーしたデータの比較
          let csvAmount = 0;
          

          if(!reOutput){
            csvJournalizingSummariesResult.journalizingsSummaries.forEach(element=>{
              csvAmount += element.amount;
            });
            if(this.extractionAmount != csvAmount){

              this.processCustomResult = this.processResultService.processAtWarning(
                this.processCustomResult, MSG_WNG.NOT_EQUAL_ABSTRACT_AMOUNT_AND_UPDATE_AMOUNT,
                this.partsResultMessageComponent);
              return;
            }

            this.ExportJournalizing(this.receiptJournalizingsResult.receiptJournalizings);

            // フラグの設定
            let updateJournalizingOption = this.getJournalizingOption(reOutput);    
            this.receiptService.UpdateOutputAtAsync(updateJournalizingOption)
              .subscribe(response=>{
                if(response!=PROCESS_RESULT_RESULT_TYPE.FAILURE){
                  this.processResultService.processAtSuccess(
                    this.processCustomResult, MSG_INF.PROCESS_FINISH, this.partsResultMessageComponent);
                  this.processResultService.createdLog(this.processCustomResult.logData);

                  this.outputAmount = this.extractionAmount;
                  this.outputCount = this.extractionCount;
                  this.getJournalizingSummariesResult();
                }
                else{
                  this.processCustomResult = this.processResultService.processAtFailure(
                    this.processCustomResult, MSG_ERR.EXPORT_ERROR, this.partsResultMessageComponent);
                }

                modalRouterProgressComponentRef.destroy();

              });
          }
          else{
            let journalizingOption = this.getJournalizingOption(true);

            this.receiptService.ExtractReceiptJournalizingAsync(journalizingOption)
              .subscribe(response=>{

                if(response!=PROCESS_RESULT_RESULT_TYPE.FAILURE){
                  this.processResultService.processAtSuccess(
                    this.processCustomResult, MSG_INF.PROCESS_FINISH, this.partsResultMessageComponent);
                  this.processResultService.createdLog(this.processCustomResult.logData);

                  csvDetailReceiptJournalizings = new ReceiptJournalizingsResult();
                  csvDetailReceiptJournalizings.receiptJournalizings=response;
  
                  // CSVの出力
                  this.ExportJournalizing(csvDetailReceiptJournalizings.receiptJournalizings);
                }
                else{
                  this.processResultService.processAtFailure(
                    this.processCustomResult, MSG_ERR.SOMETHING_ERROR.replace(MSG_ITEM_NUM.FIRST,"再出力"),
                    this.partsResultMessageComponent);
                }
                modalRouterProgressComponentRef.destroy();

              });            
          }
        }
        else{

          modalRouterProgressComponentRef.destroy()

          if(!reOutput){
            this.processResultService.processAtFailure(
              this.processCustomResult, MSG_ERR.SOMETHING_ERROR.replace(MSG_ITEM_NUM.FIRST,"出力"),
              this.partsResultMessageComponent);
          }
          else{
            this.processResultService.processAtFailure(
              this.processCustomResult, MSG_ERR.SOMETHING_ERROR.replace(MSG_ITEM_NUM.FIRST,"再出力"),
              this.partsResultMessageComponent);
          }
        }
      });
  }

  public ExportJournalizing(receiptJournalizings:Array<ReceiptJournalizing>)
  {

    let data: string = "";
    let dataList = receiptJournalizings;


    receiptJournalizings.forEach(receiptJournalizing=>{
      
      let dataItem: Array<any> = [];

      dataItem.push(this.userInfoService.Company.code);
      dataItem.push(DateUtil.convertDateString(receiptJournalizing.recordedAt.substr(0,10)));
      dataItem.push(receiptJournalizing.slipNumber);
      dataItem.push(receiptJournalizing.debitDepartmentCode);
      dataItem.push(receiptJournalizing.debitDepartmentName);
      dataItem.push(receiptJournalizing.debitAccountTitleCode);
      dataItem.push(receiptJournalizing.debitAccountTitleName);
      dataItem.push(receiptJournalizing.debitSubCode);
      dataItem.push(receiptJournalizing.debitSubName);
      dataItem.push(receiptJournalizing.creditDepartmentCode);
      dataItem.push(receiptJournalizing.creditDepartmentName);
      dataItem.push(receiptJournalizing.creditAccountTitleCode);
      dataItem.push(receiptJournalizing.creditAccountTitleName);
      dataItem.push(receiptJournalizing.creditSubCode);
      dataItem.push(receiptJournalizing.creditSubName);
      dataItem.push(receiptJournalizing.amount);
      dataItem.push(receiptJournalizing.note);
      dataItem.push(receiptJournalizing.customerCode);
      dataItem.push(receiptJournalizing.customerName);
      dataItem.push(receiptJournalizing.invoiceCode);
      dataItem.push(receiptJournalizing.staffCode);
      dataItem.push(receiptJournalizing.payerCode);
      dataItem.push(receiptJournalizing.payerName);
      dataItem.push(receiptJournalizing.sourceBankName);
      dataItem.push(receiptJournalizing.sourceBranchName);
      if (receiptJournalizing.dueAt == undefined){
        dataItem.push(receiptJournalizing.dueAt);
      }
      else {
        dataItem.push(DateUtil.convertDateString(receiptJournalizing.dueAt.substr(0,10)));
      }
      dataItem.push(receiptJournalizing.bankCode);
      dataItem.push(receiptJournalizing.bankName);
      dataItem.push(receiptJournalizing.branchCode);
      dataItem.push(receiptJournalizing.branchName);
      if (receiptJournalizing.accountTypeId == 0) {
        dataItem.push("");
      }
      else {
        dataItem.push(receiptJournalizing.accountTypeId);
      }
      dataItem.push(receiptJournalizing.accountNumber);
      if (this.userInfoService.ApplicationControl.useForeignCurrency == 1) {
        dataItem.push(receiptJournalizing.currencyCode);
      }
      dataItem = FileUtil.encloseItemBySymbol(dataItem);
      data = data + dataItem.join(",") + LINE_FEED_CODE;

    });

    let resultDatas: Array<any> = [];
    resultDatas.push(data);    
    FileUtil.download(resultDatas, this.Title + DateUtil.getYYYYMMDD(0), FILE_EXTENSION.CSV);
  }



  public historyDelete(){
    let componentFactory = this.componentFactoryResolver.resolveComponentFactory(ModalConfirmComponent);
    let componentRef = this.viewContainerRef.createComponent(componentFactory);
    componentRef.instance.ActionName = "選択した仕訳の履歴削除"
    componentRef.instance.Closing.subscribe(() => {

　    if (componentRef.instance.ModalStatus == MODAL_STATUS_TYPE.OK) {

        let componentFactory = this.componentFactoryResolver.resolveComponentFactory(ModalRouterProgressComponent);
        let modalRouterProgressComponentRef = this.viewContainerRef.createComponent(componentFactory);
        //modalRouterProgressComponentRef.destroy();

        let journalizingOption = this.getJournalizingOption(true);

        this.receiptService.CancelReceiptJournalizingAsync(journalizingOption)
          .subscribe(response=>{
            if(response!=PROCESS_RESULT_RESULT_TYPE.FAILURE){
              this.clear();
              this.getJournalizingSummariesResult();

              this.processResultService.processAtSuccess(
                this.processCustomResult, MSG_INF.PROCESS_FINISH, this.partsResultMessageComponent);
              this.processResultService.createdLog(this.processCustomResult.logData);

            }
            else{
              this.processResultService.processAtFailure(
                this.processCustomResult, MSG_ERR.SOMETHING_ERROR.replace(MSG_ITEM_NUM.FIRST,"取消処理"),
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

  public openMasterModal(table: TABLE_INDEX ,type: string) {

    let componentFactory = this.componentFactoryResolver.resolveComponentFactory(ModalMasterComponent);
    let componentRef = this.viewContainerRef.createComponent(componentFactory);

    componentRef.instance.TableIndex = table;

    componentRef.instance.Closing.subscribe(() => {

      if(componentRef.instance.ModalStatus==MODAL_STATUS_TYPE.SELECT){
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
  
  ///////////////////////////////////////////////////////////////////////
  public setRecordedAtFrom(eventType:string){
    HtmlUtil.nextFocusByName(this.elementRef, 'recordedAtToCtrl', eventType);
  }

  public setRecordedAtTo(eventType:string){
    HtmlUtil.nextFocusByName(this.elementRef, 'currencyCodeCtrl', eventType);
  }

  ///////////////////////////////////////////////////////////////////////
  public setCurrencyCode(eventType:string) {

    if (!StringUtil.IsNullOrEmpty(this.currencyCodeCtrl.value)) {

      this.currencyService.GetItems(this.currencyCodeCtrl.value)
        .subscribe(response => {
          if (response != PROCESS_RESULT_RESULT_TYPE.FAILURE && response.length > 0) {
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

  public setChkJournalSummary(index:number){
    this.selectedJournalSummary = false;
    for (let i = 0; i < this.cbxJournalSummaryCtrls.length; i++) {

      //if(this.MyFormGroup.controls["cbxJournalSummaryCtrl"+ i].value){
      if(this.cbxJournalSummaryCtrls[i].value){
        this.selectedJournalSummary = true;
        break;
      }
    }    
  }
}
