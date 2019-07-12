import { Component, OnInit, ElementRef, ComponentFactoryResolver, ViewContainerRef, AfterViewInit } from '@angular/core';
import { BaseComponent } from 'src/app/component/common/base/base-component';
import { FormControl, Validators, FormGroup } from '@angular/forms';
import { Router, NavigationEnd, ActivatedRoute } from '@angular/router';
import { HtmlUtil } from 'src/app/common/util/html.util';
import { EVENT_TYPE, BUTTON_ACTION } from 'src/app/common/const/event.const';
import { JournalizingSummariesResult } from 'src/app/model/journalizing-summaries-result.model'
import { UserInfoService } from 'src/app/service/common/user-info.service';
import { NgbDateParserFormatter } from '@ng-bootstrap/ng-bootstrap';
import { NgbDateCustomParserFormatter } from 'src/app/common/util/dateformat-util';
import { JournalizingOption } from 'src/app/model/journalizing-option.model';
import { PageUtil } from 'src/app/common/util/page-util';
import { ProcessResultService } from 'src/app/service/common/process-result.service';
import { MatchingService } from 'src/app/service/matching.service';
import { MatchingJournalizingsResult } from 'src/app/model/matching-journalizings-result.model';
import { CurrencyMasterService } from 'src/app/service/Master/currency-master.service';
import { CurrenciesResult } from 'src/app/model/currencies-result.model';
import { forkJoin } from 'rxjs';
import { MODAL_STATUS_TYPE, PROCESS_RESULT_RESULT_TYPE } from 'src/app/common/const/status.const';
import { DateUtil } from 'src/app/common/util/date-util';
import { CollationSettingMasterService } from 'src/app/service/Master/collation-setting-master.service';
import { CollationSettingResult } from 'src/app/model/collation-setting-result.model';
import { MSG_WNG, MSG_ERR, MSG_INF, MSG_ITEM_NUM } from 'src/app/common/const/message.const';
import { ModalConfirmComponent } from 'src/app/component/modal/modal-confirm/modal-confirm.component';
import { MatchingJournalizing } from 'src/app/model/matching-journalizing.model';
import { FileUtil } from 'src/app/common/util/file.util';
import { REPORT_HEADER, LINE_FEED_CODE } from 'src/app/common/const/report-setting-kbn.const';
import { FILE_EXTENSION } from 'src/app/common/const/eb-file.const';
import { LocalStorageItem } from 'src/app/model/custom-model/local-storage-item';
import { RangeSearchKey } from 'src/app/common/const/local-storage-key-const';
import { LocalStorageManageService } from 'src/app/service/common/local-storage-manage.service';
import { ModalRouterProgressComponent } from 'src/app/component/modal/modal-router-progress/modal-router-progress.component';


@Component({
  selector: 'app-pe1001-mf-matching-output',
  templateUrl: './pe1001-mf-matching-output.component.html',
  styleUrls: ['./pe1001-mf-matching-output.component.css'],
  providers: [
    { provide: NgbDateParserFormatter, useClass: NgbDateCustomParserFormatter }
  ]

})
export class Pe1001MfMatchingOutputComponent extends BaseComponent implements OnInit,AfterViewInit {


  public journalizingSummariesResult:JournalizingSummariesResult;
  public matchingJournalizingsResult: MatchingJournalizingsResult;

  public collationSettingResult: CollationSettingResult;
  public currenciesResult: CurrenciesResult;

  public recordedAtFromCtrl: FormControl;  // 伝票日付
  public recordedAtToCtrl: FormControl;

  public cbxSubAccountTitleAddCustomerCtrl:FormControl;  // 貸方補助科目に得意先名を出力する

  public cbxJournalSummaryCtrls: Array<FormControl>  // チェック

  public selectedJournalSummary:boolean; //

  public extractionCount: number = 0;
  public extractionAmount: number = 0;

  public outputCount: number = 0;
  public outputAmount: number = 0;

  public panelOpenState:boolean; // 検索パネル

  public UndefineCtrl: FormControl;

  constructor(
    public router: Router,
    public activatedRoute: ActivatedRoute,
    public elementRef: ElementRef,
    public componentFactoryResolver: ComponentFactoryResolver,
    public viewContainerRef: ViewContainerRef,
    public userInfoService: UserInfoService,
    public matchingService: MatchingService,
    public collationSettingService:CollationSettingMasterService,
    public currencyService: CurrencyMasterService,
    public processResultService: ProcessResultService,
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
    this.clear();

    this.collationSettingResult = new CollationSettingResult();
    this.currenciesResult = new CurrenciesResult();

    let collationSettingResponse = this.collationSettingService.Get();
    let currencyResponse = this.currencyService.GetItems('JPY');

    forkJoin(
      collationSettingResponse,
      currencyResponse
    )
      .subscribe(responseList => {
        if (
              responseList != undefined
          &&  responseList.length == 2
          &&  responseList[0] !=  PROCESS_RESULT_RESULT_TYPE.FAILURE
          &&  responseList[1] !=  PROCESS_RESULT_RESULT_TYPE.FAILURE
        ){
          this.collationSettingResult.collationSetting = responseList[0];
          this.currenciesResult.currencies = responseList[1];
          this.getJournalizingSummariesResult();
        }
      });


      this.setRangeCheckbox();
  }
  
  ngAfterViewInit(){
    HtmlUtil.nextFocusByName(this.elementRef, 'recordedAtFromCtrl', EVENT_TYPE.NONE);
  }

  public setRangeCheckbox() {
    let cbxSubAccountTitleAddCustomerCtrl = this.localStorageManageService.get(RangeSearchKey.PB1001_CUSTOMER);
      
    if (cbxSubAccountTitleAddCustomerCtrl != null) {
      this.cbxSubAccountTitleAddCustomerCtrl.setValue(cbxSubAccountTitleAddCustomerCtrl.value);
    }
  }  
        

  
  public setControlInit() {

    this.recordedAtFromCtrl = new FormControl("",[ Validators.maxLength(10)]);  // 伝票日付
    this.recordedAtToCtrl = new FormControl("",[  Validators.maxLength(10)]);

    this.cbxSubAccountTitleAddCustomerCtrl = new FormControl("");  // 貸方補助科目に得意先名を出力する

    this.UndefineCtrl = new FormControl("");

  }
  public setValidator() {
    this.MyFormGroup = new FormGroup({
      recordedAtFromCtrl: this.recordedAtFromCtrl,  // 伝票日付
      recordedAtToCtrl: this.recordedAtToCtrl,
      cbxSubAccountTitleAddCustomerCtrl: this.cbxSubAccountTitleAddCustomerCtrl,  // 貸方補助科目に得意先名を出力する
      UndefineCtrl: this.UndefineCtrl,
    });

  }

  public clear() {
    this.MyFormGroup.reset();
    // 初期化処理
    this.extractionCount = 0;
    this.extractionAmount = 0;
    this.outputCount = 0;
    this.outputAmount = 0;

    this.recordedAtFromCtrl.enable();
    this.recordedAtToCtrl.enable();
    this.cbxSubAccountTitleAddCustomerCtrl.enable();
    this.selectedJournalSummary = false;

    HtmlUtil.nextFocusByName(this.elementRef, 'recordedAtFromCtrl', EVENT_TYPE.NONE);

    this.panelOpenState = true;
  }

  public getJournalizingSummariesResult() {

    let journalizingOption = this.getJournalizingOption(false);

    journalizingOption.isOutputted = true;
    journalizingOption.recordedAtFrom = null;
    journalizingOption.recordedAtTo = null;

    this.journalizingSummariesResult = new JournalizingSummariesResult();
    this.matchingService.GetMatchingJournalizingSummary(journalizingOption)
      .subscribe(response => {
        if(response!=PROCESS_RESULT_RESULT_TYPE.FAILURE){

          this.journalizingSummariesResult.journalizingsSummaries = response;
          this.cbxJournalSummaryCtrls = new Array(this.journalizingSummariesResult.journalizingsSummaries.length);

          for (let i = 0; i < this.cbxJournalSummaryCtrls.length; i++) {
            this.cbxJournalSummaryCtrls[i] = new FormControl(null);
          }

          for (let i = 0; i < this.cbxJournalSummaryCtrls.length; i++) {
            this.MyFormGroup.removeControl("cbxJournalSummaryCtrl" + i);
            this.MyFormGroup.addControl("cbxJournalSummaryCtrl" + i, this.cbxJournalSummaryCtrls[i]);
          }

        }
        else{
          this.processCustomResult = this.processResultService.processAtFailure(
            this.processCustomResult,MSG_ERR.SOMETHING_ERROR.replace(MSG_ITEM_NUM.FIRST,"抽出処理"),
            this.partsResultMessageComponent);
        }

      });
  }

  public getJournalizingOption(reAction: boolean): JournalizingOption {
    let journalizingOption = new JournalizingOption();

    journalizingOption.companyId = this.userInfoService.Company.id;
    journalizingOption.currencyId = this.currenciesResult.currencies[0].id;
    journalizingOption.isOutputted = false;
    journalizingOption.loginUserId = this.userInfoService.LoginUser.id;
    journalizingOption.recordedAtFrom =  DateUtil.ConvertFromDatepicker(this.recordedAtFromCtrl);
    journalizingOption.recordedAtTo =  DateUtil.ConvertFromDatepicker(this.recordedAtToCtrl);
    journalizingOption.outputAt = new Array<string>();
    journalizingOption.useDiscount = (this.userInfoService.ApplicationControl.useDiscount == 1);
    journalizingOption.containAdvanceReceivedOccured = false;
    journalizingOption.containAdvanceReceivedMatching = false;
    journalizingOption.outputCustoemrName = this.cbxSubAccountTitleAddCustomerCtrl.value ? true : false;

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
      case BUTTON_ACTION.OUTPUT:
        this.output();
        break;
      case BUTTON_ACTION.REOUTPUT:
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

    this.processCustomResult = this.processResultService.clearProcessCustomMsg(this.processCustomResult);

    let journalizingOption = this.getJournalizingOption(false);


    let componentFactory = this.componentFactoryResolver.resolveComponentFactory(ModalRouterProgressComponent);
    let modalRouterProgressComponentRef = this.viewContainerRef.createComponent(componentFactory);
    //modalRouterProgressComponentRef.destroy();    

    this.matchingService.MFExtractMatchingJournalizing(journalizingOption)
      .subscribe(response => {

        this.processResultService.processAtGetData(this.processCustomResult, response, true, this.partsResultMessageComponent);

        this.matchingJournalizingsResult = new MatchingJournalizingsResult();
        this.matchingJournalizingsResult.matchingJournalizings = response;

        this.extractionCount  = 0;
        this.extractionAmount = 0;
        this.outputCount      = 0;
        this.outputAmount     = 0;

        if (response != undefined && response.length > 0) {
          this.matchingJournalizingsResult.matchingJournalizings.forEach(element => {
            this.extractionCount++;
            this.extractionAmount += element.amount;
          });

          this.recordedAtFromCtrl.disable();
          this.recordedAtToCtrl.disable();
          this.cbxSubAccountTitleAddCustomerCtrl.disable();

          this.processCustomResult = this.processResultService.processAtSuccess(
            this.processCustomResult, MSG_INF.DATA_EXTRACTED, this.partsResultMessageComponent);
          this.processResultService.createdLog(this.processCustomResult.logData);

        }
        modalRouterProgressComponentRef.destroy();  
      });

  }


  public output() {
    this.outputCsv(false);
  }

  public reOutput() {
    this.outputCsv(true);
  }

  public outputCsv(reOutput: boolean) {

    let searchJournalizingOption = this.getJournalizingOption(reOutput);

    let componentFactory = this.componentFactoryResolver.resolveComponentFactory(ModalRouterProgressComponent);
    let modalRouterProgressComponentRef = this.viewContainerRef.createComponent(componentFactory);
    //modalRouterProgressComponentRef.destroy();

    this.matchingService.GetMatchingJournalizingSummary(searchJournalizingOption)
      .subscribe(response => {
        if(response!=PROCESS_RESULT_RESULT_TYPE.FAILURE) {
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
                this.processCustomResult, MSG_WNG.NOT_EQUAL_ABSTRACT_AMOUNT_AND_UPDATE_AMOUNT,
                this.partsResultMessageComponent);

              return;
            }

            this.ExportJournalizing(this.matchingJournalizingsResult.matchingJournalizings);

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

            this.matchingService.MFExtractMatchingJournalizing(journalizingOption)
              .subscribe(response => {

                if(response!=PROCESS_RESULT_RESULT_TYPE.FAILURE){

                  this.processCustomResult = this.processResultService.processAtSuccess(
                    this.processCustomResult, MSG_INF.PROCESS_FINISH, this.partsResultMessageComponent);
                  this.processResultService.createdLog(this.processCustomResult.logData);

                  csvDetailMatchingJournalizings = new MatchingJournalizingsResult();
                  csvDetailMatchingJournalizings.matchingJournalizings = response;

                  // CSVの出力
                  this.ExportJournalizing(csvDetailMatchingJournalizings.matchingJournalizings);
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

    let headers = FileUtil.encloseItemBySymbol(REPORT_HEADER.MF_MATCHING_JOURNALIZING);
    let data: string = headers.join(",") + LINE_FEED_CODE;

    matchingJournalizings.forEach(matchingJournalizing => {

      const debitTaxCategory = "";
      const debitAmount = matchingJournalizing.amount;
      const debitTaxAmount = "";
      const creditTaxCategory = "";
      const creditAmount = matchingJournalizing.amount;
      const creditTaxAmount = "";
      const mfTag = "";
      const mfJournalizingType = "インポート";
      const closingAdjustment = "";
      const createAt = DateUtil.getYYYYMMDD(4, matchingJournalizing.createAt);

      let dataItem: Array<any> = [];

      dataItem.push(matchingJournalizing.slipNumber);               //取引No
      dataItem.push(DateUtil.convertDateString(matchingJournalizing.recordedAt.substr(0, 10))); //取引日
      dataItem.push(matchingJournalizing.debitAccountTitleName);    //借方勘定科目
      dataItem.push(matchingJournalizing.debitSubCode);             //借方補助科目
      dataItem.push(debitTaxCategory);                              //借方税区分 空欄固定
      dataItem.push(matchingJournalizing.debitDepartmentName);      //借方部門
      dataItem.push(debitAmount);                                   //借方金額(円)
      dataItem.push(debitTaxAmount);                                //借方税額 空欄固定
      dataItem.push(matchingJournalizing.creditAccountTitleName);   //貸方勘定科目
      dataItem.push(matchingJournalizing.creditSubCode);            //貸方補助科目
      dataItem.push(creditTaxCategory);                             //貸方税区分  空欄固定
      dataItem.push(matchingJournalizing.creditDepartmentName);     //貸方部門
      dataItem.push(creditAmount);                                  //貸方金額(円)
      dataItem.push(creditTaxAmount);                               //貸方税額
      dataItem.push(matchingJournalizing.note);                     //摘要
      dataItem.push(matchingJournalizing.matchingMemo);             //仕訳メモ
      dataItem.push(mfTag);                                         //タグ  空欄固定
      dataItem.push(mfJournalizingType);                            //MF仕訳タイプ 「インポート」固定
      dataItem.push(closingAdjustment);                             //決算整理仕訳  空欄固定
      dataItem.push(createAt);                                      //作成日時
      dataItem.push(createAt);                                      //最終更新日時 作成日時と同じ


      dataItem = FileUtil.encloseItemBySymbol(dataItem);
      data = data + dataItem.join(",") + LINE_FEED_CODE;

    });

    let resultDatas: Array<any> = [];
    resultDatas.push(data);
    FileUtil.download(resultDatas, this.Title + DateUtil.getYYYYMMDD(0), FILE_EXTENSION.CSV);

  }

  public cancel(){
    this.processCustomResult = this.processResultService.clearProcessCustomMsg(this.processCustomResult);;
    let componentFactory = this.componentFactoryResolver.resolveComponentFactory(ModalConfirmComponent);
    let componentRef = this.viewContainerRef.createComponent(componentFactory);
    componentRef.instance.ActionName = "選択した仕訳の取消"
    componentRef.instance.Closing.subscribe(() => {

      if (componentRef.instance.ModalStatus == MODAL_STATUS_TYPE.OK) {
        let journalizingOption = this.getJournalizingOption(true);

        let componentFactory = this.componentFactoryResolver.resolveComponentFactory(ModalRouterProgressComponent);
        let modalRouterProgressComponentRef = this.viewContainerRef.createComponent(componentFactory);
        //modalRouterProgressComponentRef.destroy();

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


  ///////////////////////////////////////////////////////////////////////
  public setWriteFile(eventType:string){
    HtmlUtil.nextFocusByName(this.elementRef, 'recordedAtFromCtrl', eventType);
  }


  ///////////////////////////////////////////////////////////////////////
  public setRecordedAtFrom(eventType:string){
    HtmlUtil.nextFocusByName(this.elementRef, 'recordedAtToCtrl', eventType);
  }

  public setRecordedAtTo(eventType:string){
    HtmlUtil.nextFocusByName(this.elementRef, 'writeFileCtrl', eventType);
  }

  ///////////////////////////////////////////////////////////////////////
  public setCbxJournalSummary(index: number) {
    this.selectedJournalSummary = false;
    for (let i = 0; i < this.cbxJournalSummaryCtrls.length; i++) {
      if (this.cbxJournalSummaryCtrls[i].value) {
        this.selectedJournalSummary = true;
        break;
      }
    }
  }

  public setCbxSubAccountTitleAddCustomerCtrl(eventType:string){
    let localstorageItem = new LocalStorageItem();
    localstorageItem.key = RangeSearchKey.PE0601_CUSTOMER;
    localstorageItem.value = this.cbxSubAccountTitleAddCustomerCtrl.value;
    this.localStorageManageService.set(localstorageItem);

  }


}

