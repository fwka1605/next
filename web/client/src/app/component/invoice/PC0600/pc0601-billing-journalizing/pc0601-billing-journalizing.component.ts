import { Component, OnInit, ViewContainerRef, ComponentFactoryResolver, ElementRef, AfterViewInit } from '@angular/core';
import { StringUtil } from 'src/app/common/util/string-util';
import { FormControl, Validators, FormGroup } from '@angular/forms';
import { BaseComponent } from 'src/app/component/common/base/base-component';
import { HtmlUtil } from 'src/app/common/util/html.util';
import { TABLE_INDEX } from 'src/app/common/const/table-name.const';
import { ModalMasterComponent } from 'src/app/component/modal/modal-master/modal-master.component';
import { JournalizingSummariesResult } from 'src/app/model/journalizing-summaries-result.model';
import { BillingService } from 'src/app/service/billing.service';
import { KEY_CODE, EVENT_TYPE, BUTTON_ACTION } from 'src/app/common/const/event.const';
import { FormatterUtil } from 'src/app/common/util/formatter.util';
import { MODAL_STATUS_TYPE, PROCESS_RESULT_RESULT_TYPE } from 'src/app/common/const/status.const';
import { UserInfoService } from 'src/app/service/common/user-info.service';
import { CurrencyMasterService } from 'src/app/service/Master/currency-master.service';
import { NgbDateParserFormatter } from '@ng-bootstrap/ng-bootstrap';
import { NgbDateCustomParserFormatter } from 'src/app/common/util/dateformat-util';
import { ColumnNameSettingsResult } from 'src/app/model/column-name-settings-result.model';
import { CollationSettingResult } from 'src/app/model/collation-setting-result.model';
import { CurrenciesResult } from 'src/app/model/currencies-result.model';
import { BillingJournalizingsResult } from 'src/app/model/billing-journalizings-result.model';
import { ColumnNameSettingMasterService } from 'src/app/service/Master/column-name-setting-master.service';
import { CollationSettingMasterService } from 'src/app/service/Master/collation-setting-master.service';
import { FreeImporterFormatType } from 'src/app/common/common-const';
import { forkJoin } from 'rxjs';
import { DateUtil } from 'src/app/common/util/date-util';
import { FILE_EXTENSION } from 'src/app/common/const/eb-file.const';
import { ModalConfirmComponent } from 'src/app/component/modal/modal-confirm/modal-confirm.component';
import { BillingJournalizing } from 'src/app/model/billing-journalizing.model';
import { FileUtil } from 'src/app/common/util/file.util';
import { BillingJournalizingOption } from 'src/app/model/billing-journalizing-option.model';
import { LINE_FEED_CODE } from 'src/app/common/const/report-setting-kbn.const';
import { Router, ActivatedRoute, NavigationEnd } from '@angular/router';
import { PageUtil } from 'src/app/common/util/page-util';
import { ProcessResultService } from 'src/app/service/common/process-result.service';
import { MSG_WNG, MSG_ERR, MSG_INF, MSG_ITEM_NUM } from 'src/app/common/const/message.const';
import { ModalRouterProgressComponent } from 'src/app/component/modal/modal-router-progress/modal-router-progress.component';

@Component({
  selector: 'app-pc0601-billing-journalizing',
  templateUrl: './pc0601-billing-journalizing.component.html',
  styleUrls: ['./pc0601-billing-journalizing.component.css'],
  providers: [
    { provide: NgbDateParserFormatter, useClass: NgbDateCustomParserFormatter }
  ]

})
export class Pc0601BillingJournalizingComponent extends BaseComponent implements OnInit,AfterViewInit {

  public panelOpenState: boolean;


  public columnNameSettingsResult:ColumnNameSettingsResult;
  public collationSettingResult:CollationSettingResult;
  public currenciesResult:CurrenciesResult;;
  
  public journalizingSummariesResult:JournalizingSummariesResult;
  public billingJournalizingsResult:BillingJournalizingsResult;

  public billedAtFromCtrl: FormControl;  // 請求日
  public billedAtToCtrl: FormControl;

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
    public billingService:BillingService,
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

    let columnNameSettingsResponse = this.columnNameSettingService.Get(FreeImporterFormatType.Billing);
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
    HtmlUtil.nextFocusByName(this.elementRef, 'billedAtFromCtrl', EVENT_TYPE.NONE);
  }
  
  public setControlInit() {

    this.billedAtFromCtrl = new FormControl("", [Validators.maxLength(10)]);  // 請求日
    this.billedAtToCtrl = new FormControl("", [Validators.maxLength(10)]);

    this.currencyCodeCtrl = new FormControl("", [Validators.maxLength(3)]);  // 通貨コード

    this.UndefineCtrl = new FormControl("");


  }

  public setValidator() {
    this.MyFormGroup = new FormGroup({
      billedAtFromCtrl: this.billedAtFromCtrl,  // 請求日
      currencyCodeCtrl: this.currencyCodeCtrl, // 通貨コード
      billedAtToCtrl: this.billedAtToCtrl,
      UndefineCtrl: this.UndefineCtrl,
    });

  }

  public setFormatter(){
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
    this.billedAtFromCtrl.enable();
    this.billedAtToCtrl.enable();

    this.selectedJournalSummary=false;  

    this.panelOpenState = true;

    HtmlUtil.nextFocusByName(this.elementRef, 'billedAtFromCtrl', EVENT_TYPE.NONE);

  }


  public getJournalizingSummariesResult(){

    let journalizingOption = new BillingJournalizingOption();
    journalizingOption.companyId= this.userInfoService.Company.id;
    journalizingOption.currencyId= this.currenciesResult.currencies[0].id;
    journalizingOption.isOutuptted= true;

    this.journalizingSummariesResult = new JournalizingSummariesResult();
    this.billingService.GetBillingJournalizingSummary(journalizingOption)
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
    this.processCustomResult = this.processResultService.clearProcessCustomMsg(this.processCustomResult);  
    let isStandardPattern:boolean = this.collationSettingResult.collationSetting.journalizingPattern == 0;

    // 動作中のコンポーネントを開く
    let componentFactory = this.componentFactoryResolver.resolveComponentFactory(ModalRouterProgressComponent);
    let modalRouterProgressComponentRef = this.viewContainerRef.createComponent(componentFactory);
    modalRouterProgressComponentRef.instance.Closing.subscribe(() => {
      modalRouterProgressComponentRef.destroy();
    });

    if(isStandardPattern){
      let journalizingOption = this.getJournalizingOption(false);

      this.billingService.ExtractBillingJournalizing(journalizingOption)
        .subscribe(response=>{

          this.processResultService.processAtGetData(
            this.processCustomResult, response, true, this.partsResultMessageComponent);

          this.billingJournalizingsResult = new BillingJournalizingsResult();
          this.billingJournalizingsResult.billingJournalizings=response;
  
          if(response != PROCESS_RESULT_RESULT_TYPE.FAILURE && response.length==0){
            this.extractionCount = 0;
            this.extractionAmount = 0;
            this.outputCount = 0;
            this.outputAmount = 0;



          }
          else{
            this.extractionCount = 0;
            this.extractionAmount = 0;  
            this.billingJournalizingsResult.billingJournalizings.forEach(element=>{
              this.extractionCount++;
              this.extractionAmount += element.billingAmount;
            });
            this.outputCount = 0;
            this.outputAmount = 0;
  
            // 
            this.billedAtFromCtrl.disable();  // 入金日
            this.billedAtToCtrl.disable();

          }
          modalRouterProgressComponentRef.destroy();

        });
    }
    else{
      modalRouterProgressComponentRef.destroy();
    }
  }

  public getJournalizingOption(reAction:boolean):BillingJournalizingOption{
    let journalizingOption = new BillingJournalizingOption();

    journalizingOption.companyId= this.userInfoService.Company.id;
    journalizingOption.currencyId = this.currenciesResult.currencies[0].id;
    //journalizingOption.customerId
    journalizingOption.isOutuptted=false;
    journalizingOption.loginUserId=this.userInfoService.LoginUser.id;
    //journalizingOption.updateAt
    journalizingOption.billedAtFrom = DateUtil.ConvertFromDatepicker(this.billedAtFromCtrl);
    journalizingOption.billedAtTo = DateUtil.ConvertFromDatepicker(this.billedAtToCtrl);
    journalizingOption.outputAt = new Array<string>();
    //journalizingOption.useDiscount = (this.userInfoService.ApplicationControl.useDiscount==1);
    //journalizingOption.containAdvanceReceivedOccured
    //journalizingOption.containAdvanceReceivedMatching
    //journalizingOption.createAtFrom
    //journalizingOption.createAtTo
    //journalizingOption.journalizingTypes
    //journalizingOption.isGeneral = (this.collationSettingResult.collationSetting.journalizingPattern==1);
    //journalizingOption.outputCustoemrName
    //journalizingOption.precision    

    if(reAction){
      for(let index=0;index<this.journalizingSummariesResult.journalizingsSummaries.length;index++){
        if(this.cbxJournalSummaryCtrls[index].value){
          journalizingOption.outputAt.push(this.journalizingSummariesResult.journalizingsSummaries[index].outputAt);
        }
      }

      journalizingOption.billedAtFrom=null;
      journalizingOption.billedAtTo = null;

      journalizingOption.isOutuptted= true;

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

    this.billingService.GetJournalizingReport(journalizingOption)
    .subscribe(response=>{
      if(response !=PROCESS_RESULT_RESULT_TYPE.FAILURE){
        FileUtil.download([response.body], this.Title + DateUtil.getYYYYMMDD(0), FILE_EXTENSION.PDF);

        this.processCustomResult = this.processResultService.processAtSuccess(
          this.processCustomResult, MSG_INF.PROCESS_FINISH, this.partsResultMessageComponent);

        this.processResultService.createdLog(this.processCustomResult.logData);
        
      }
      else{
        this.processCustomResult = this.processResultService.processAtFailure(
          this.processCustomResult, MSG_ERR.SOMETHING_ERROR.replace(MSG_ITEM_NUM.FIRST, '請求仕訳の印刷'),
          this.partsResultMessageComponent);
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

    // 動作中のコンポーネントを開く
    let componentFactory = this.componentFactoryResolver.resolveComponentFactory(ModalRouterProgressComponent);
    let modalRouterProgressComponentRef = this.viewContainerRef.createComponent(componentFactory);
    modalRouterProgressComponentRef.instance.Closing.subscribe(() => {
      modalRouterProgressComponentRef.destroy();
    });

    let searchJournalizingOption = this.getJournalizingOption(reOutput);    

    this.billingService.GetBillingJournalizingSummary(searchJournalizingOption)
      .subscribe(response=>{
        if(response!=PROCESS_RESULT_RESULT_TYPE.FAILURE){
          let csvJournalizingSummariesResult = new JournalizingSummariesResult();
          let csvDetailReceiptJournalizings = new BillingJournalizingsResult();

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

            this.ExportJournalizing(this.billingJournalizingsResult.billingJournalizings);

            // フラグの設定
            let updateJournalizingOption = this.getJournalizingOption(reOutput);    
            this.billingService.UpdateOutputAt(updateJournalizingOption)
              .subscribe(response=>{
                if(response!=PROCESS_RESULT_RESULT_TYPE.FAILURE){
                  this.processCustomResult = this.processResultService.processAtSuccess(
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

            this.billingService.ExtractBillingJournalizing(journalizingOption)
              .subscribe(response=>{

                if(response!=PROCESS_RESULT_RESULT_TYPE.FAILURE){

                  this.processCustomResult = this.processResultService.processAtSuccess(
                    this.processCustomResult, MSG_INF.PROCESS_FINISH, this.partsResultMessageComponent);
                  this.processResultService.createdLog(this.processCustomResult.logData);

                  csvDetailReceiptJournalizings = new BillingJournalizingsResult();
                  csvDetailReceiptJournalizings.billingJournalizings=response;
  
                  // CSVの出力
                  this.ExportJournalizing(csvDetailReceiptJournalizings.billingJournalizings);
                }
                else{
                  this.processResultService.processAtFailure(this.processCustomResult,
                    MSG_ERR.SOMETHING_ERROR.replace(MSG_ITEM_NUM.FIRST, "再出力"), this.partsResultMessageComponent);
                }
                modalRouterProgressComponentRef.destroy();

              });            
          }
        }
        else{
          modalRouterProgressComponentRef.destroy();
          if(!reOutput){
            this.processResultService.processAtFailure(this.processCustomResult,
              MSG_ERR.SOMETHING_ERROR.replace(MSG_ITEM_NUM.FIRST, "出力"), this.partsResultMessageComponent);
          }
          else{
            this.processResultService.processAtFailure(this.processCustomResult,
              MSG_ERR.SOMETHING_ERROR.replace(MSG_ITEM_NUM.FIRST, "再出力"), this.partsResultMessageComponent);
          }
        }
      });
  }


  public ExportJournalizing(billingJournalizings:Array<BillingJournalizing>)
  {

    let data: string = "";
    let dataList = billingJournalizings;


    billingJournalizings.forEach(billingJournalizing=>{
      
      let dataItem: Array<any> = [];

      dataItem.push(this.userInfoService.Company.code);
      dataItem.push(DateUtil.convertDateString(billingJournalizing.billedAt.substr(0,10)));
      dataItem.push(billingJournalizing.slipNumber);
      dataItem.push(billingJournalizing.debitDepartmentCode);
      dataItem.push(billingJournalizing.debitDepartmentName);
      dataItem.push(billingJournalizing.debitAccountTitleCode);
      dataItem.push(billingJournalizing.debitAccountTitleName);
      dataItem.push(billingJournalizing.debitSubCode);
      dataItem.push(billingJournalizing.debitSubName);
      dataItem.push(billingJournalizing.creditDepartmentCode);
      dataItem.push(billingJournalizing.creditDepartmentName);
      dataItem.push(billingJournalizing.creditAccountTitleCode);
      dataItem.push(billingJournalizing.creditAccountTitleName);
      dataItem.push(billingJournalizing.creditSubCode);
      dataItem.push(billingJournalizing.creditSubName);
      dataItem.push(billingJournalizing.billingAmount);
      dataItem.push(billingJournalizing.note);
      dataItem.push(billingJournalizing.customerCode);
      dataItem.push(billingJournalizing.customerName);
      dataItem.push(billingJournalizing.invoiceCode);
      dataItem.push(billingJournalizing.staffCode);
      dataItem.push(billingJournalizing.payerCode);
      dataItem.push(billingJournalizing.payerName);
      dataItem.push(billingJournalizing.sourceBankName);
      dataItem.push(billingJournalizing.sourceBranchName);
      //dataItem.push(billingJournalizing.dueAt);
      dataItem.push("");
      dataItem.push(billingJournalizing.bankCode);
      dataItem.push(billingJournalizing.bankName);
      dataItem.push(billingJournalizing.branchCode);
      dataItem.push(billingJournalizing.branchName);
      dataItem.push(billingJournalizing.accountType);
      dataItem.push(billingJournalizing.accountNumber);
      dataItem.push(billingJournalizing.currencyCode);
      dataItem = FileUtil.encloseItemBySymbol(dataItem);
      data = data + dataItem.join(",") + LINE_FEED_CODE;

    });

    let resultDatas: Array<any> = [];
    resultDatas.push(data);    
    FileUtil.download(resultDatas, this.Title + DateUtil.getYYYYMMDD(0), FILE_EXTENSION.CSV);
  }



  public historyDelete(){
    
    this.processCustomResult = this.processResultService.clearProcessCustomMsg(this.processCustomResult);;
    let componentFactory = this.componentFactoryResolver.resolveComponentFactory(ModalConfirmComponent);
    let componentRef = this.viewContainerRef.createComponent(componentFactory);
    componentRef.instance.ActionName = "選択した仕訳の履歴削除"
    componentRef.instance.Closing.subscribe(() => {

　    if (componentRef.instance.ModalStatus == MODAL_STATUS_TYPE.OK) {

        // 動作中のコンポーネントを開く
        let componentFactory = this.componentFactoryResolver.resolveComponentFactory(ModalRouterProgressComponent);
        let modalRouterProgressComponentRef = this.viewContainerRef.createComponent(componentFactory);
        modalRouterProgressComponentRef.instance.Closing.subscribe(() => {
          modalRouterProgressComponentRef.destroy();
        });

        let journalizingOption = this.getJournalizingOption(true);

        this.billingService.CancelBillingJournalizing(journalizingOption)
          .subscribe(response=>{
            if(response!=PROCESS_RESULT_RESULT_TYPE.FAILURE){
              this.clear();
              this.getJournalizingSummariesResult();

              this.processResultService.processAtSuccess(
                this.processCustomResult, MSG_INF.PROCESS_FINISH, this.partsResultMessageComponent);
              this.processResultService.createdLog(this.processCustomResult.logData);
              
            }
            else{
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
  public setBilledAtFrom(eventType:string){
    HtmlUtil.nextFocusByName(this.elementRef, 'billedAtToCtrl', eventType);
  }

  public setBilledAtTo(eventType:string){
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

  public setCbxJournalSummary(index:number){
    this.selectedJournalSummary = false;
    for (let i = 0; i < this.cbxJournalSummaryCtrls.length; i++) {
      if(this.cbxJournalSummaryCtrls[i].value){
        this.selectedJournalSummary = true;
        break;
      }
    }    
  }

}
