import { Component, OnInit, ViewContainerRef, ComponentFactoryResolver, ElementRef, AfterViewInit, ViewChild } from '@angular/core';
import { BaseComponent } from 'src/app/component/common/base/base-component';
import { Router, NavigationEnd, ActivatedRoute} from '@angular/router';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { TABLE_INDEX } from 'src/app/common/const/table-name.const';
import { KEY_CODE, EVENT_TYPE, BUTTON_ACTION } from 'src/app/common/const/event.const';
import { ModalMasterComponent } from 'src/app/component/modal/modal-master/modal-master.component';
import { HtmlUtil } from 'src/app/common/util/html.util';
import { StringUtil } from 'src/app/common/util/string-util';
import { MatchingJournalizingDetailsResult } from 'src/app/model/matching-journalizing-details-result.model';
import { MatchingService } from 'src/app/service/matching.service';
import { FormatterUtil } from 'src/app/common/util/formatter.util';
import { MODAL_STATUS_TYPE, PROCESS_RESULT_RESULT_TYPE } from 'src/app/common/const/status.const';
import { UserInfoService } from 'src/app/service/common/user-info.service';
import { NgbDateParserFormatter } from '@ng-bootstrap/ng-bootstrap';
import { NgbDateCustomParserFormatter } from 'src/app/common/util/dateformat-util';
import { CustomerMasterService } from 'src/app/service/Master/customer-master.service';
import { CurrencyMasterService } from 'src/app/service/Master/currency-master.service';
import { JournalizingOption } from 'src/app/model/journalizing-option.model';
import { DateUtil } from 'src/app/common/util/date-util';
import { JournalizingType, JournalizingName } from 'src/app/common/const/journalizing-type';
import { MatchingJournalizingDetail } from 'src/app/model/matching-journalizing-detail.model';
import { FILE_EXTENSION } from 'src/app/common/const/eb-file.const';
import { MatchingJournalizingCancellationReportSource } from 'src/app/model/matching-journalizing-cancellation-report-source.model';
import { FileUtil } from 'src/app/common/util/file.util';
import { ModalConfirmComponent } from 'src/app/component/modal/modal-confirm/modal-confirm.component';
import { ModalRouterProgressComponent } from 'src/app/component/modal/modal-router-progress/modal-router-progress.component';
import { LINE_FEED_CODE } from 'src/app/common/const/report-setting-kbn.const';
import { ProcessResultService } from 'src/app/service/common/process-result.service';
import { MSG_ERR, MSG_WNG, MSG_ITEM_NUM } from 'src/app/common/const/message.const';
import { PageUtil } from 'src/app/common/util/page-util';
import { CODE_TYPE } from 'src/app/common/const/kbn.const';
import { MatAutocompleteTrigger, MatExpansionPanel } from '@angular/material';

@Component({
  selector: 'app-pe0401-matching-journalizing-cancellation',
  templateUrl: './pe0401-matching-journalizing-cancellation.component.html',
  styleUrls: ['./pe0401-matching-journalizing-cancellation.component.css'],
  providers: [
    { provide: NgbDateParserFormatter, useClass: NgbDateCustomParserFormatter }
  ]
})
export class Pe0401MatchingJournalizingCancellationComponent extends BaseComponent implements OnInit,AfterViewInit {

  // これをしないとEnum型がテンプレートで見えなかったため
  public TABLE_INDEX: typeof TABLE_INDEX = TABLE_INDEX;
  public JournalizingName: typeof JournalizingName = JournalizingName;

  public selectJournal: boolean = false;

  //////////////////////////////////////////////////////////////

  public matchingJournalizingDetailsResult: MatchingJournalizingDetailsResult;
  //////////////////////////////////////////////////////////////
  public cbxCheckAllCtrl:FormControl; // 全選択・全解除

  public updatedAtFromCtrl: FormControl;  // 更新日
  public updatedAtToCtrl: FormControl;

  public customerCodeCtrl: FormControl;  // 得意先コード
  public customerNameCtrl: FormControl;
  public customerId: number;


  public currencyCodeCtrl: FormControl;  // 通貨コード

  public cbxMatchingCtrl: FormControl;  // 仕訳区分(消込仕訳)
  public cbxReceiptExcludeCtrl: FormControl;  // 仕訳区分(対象外仕訳)
  public cbxAdvanceReceivedOccuredCtrl: FormControl;  // 仕訳区分(前受計上仕訳)
  public cbxAdvanceReceivedTransferCtrl: FormControl;  // 仕訳区分(前受振替仕訳)

  public undefineCtrl: FormControl; // 未定用;

  public cbxDeleteFlgCtrls: FormControl[]; // 解除

  public panelOpenState:boolean;

  @ViewChild('customerCodeInput', { read: MatAutocompleteTrigger }) customerCodeTrigger: MatAutocompleteTrigger;

  @ViewChild('panel') panel:MatExpansionPanel;

  constructor(
   public router: Router,
   public activatedRoute: ActivatedRoute,
   public elementRef: ElementRef,
   public componentFactoryResolver: ComponentFactoryResolver,
   public viewContainerRef: ViewContainerRef,
   public userInfoService: UserInfoService,
   public matchingService: MatchingService,
   public customerService:CustomerMasterService,
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
    this.setAutoComplete();
  }

  ngAfterViewInit(){
    HtmlUtil.nextFocusByName(this.elementRef, 'updatedAtFromCtrl', EVENT_TYPE.NONE);
  }

  public setControlInit() {
    this.cbxCheckAllCtrl = new FormControl(null);

    this.updatedAtFromCtrl = new FormControl("", [Validators.required, Validators.maxLength(10)]);  // 更新日
    this.updatedAtToCtrl = new FormControl("", [Validators.required, Validators.maxLength(10)]);

    this.customerCodeCtrl = new FormControl("", [Validators.maxLength(10)]);  // 得意先コード
    this.customerNameCtrl = new FormControl("");

    if (this.userInfoService.ApplicationControl.useForeignCurrency === 1) {
      this.currencyCodeCtrl = new FormControl("", [Validators.required, Validators.maxLength(3)]);  // 通貨コード
    }
    else {
      this.currencyCodeCtrl = new FormControl("", [Validators.maxLength(3)]);  // 通貨コード
    }

    this.cbxMatchingCtrl = new FormControl("");  // 仕訳区分(消込仕訳)
    this.cbxReceiptExcludeCtrl = new FormControl("");  // 仕訳区分(対象外仕訳)
    this.cbxAdvanceReceivedOccuredCtrl = new FormControl("");  // 仕訳区分(前受計上仕訳)
    this.cbxAdvanceReceivedTransferCtrl = new FormControl("");  // 仕訳区分(前受振替仕訳)

    this.undefineCtrl = new FormControl(""); // 未定用;
  }

  public setValidator() {
    this.MyFormGroup = new FormGroup({

      cbxCheckAllCtrl:this.cbxCheckAllCtrl, // 全選択・全解除
      updatedAtFromCtrl: this.updatedAtFromCtrl,  // 更新日
      updatedAtToCtrl: this.updatedAtToCtrl,

      customerCodeCtrl: this.customerCodeCtrl,  // 得意先コード
      customerNameCtrl: this.customerNameCtrl,

      currencyCodeCtrl: this.currencyCodeCtrl,  // 通貨コード

      cbxMatchingCtrl: this.cbxMatchingCtrl,  // 仕訳区分(消込仕訳)
      cbxReceiptExcludeCtrl: this.cbxReceiptExcludeCtrl,  // 仕訳区分(対象外仕訳)
      cbxAdvanceReceivedOccuredCtrl: this.cbxAdvanceReceivedOccuredCtrl,  // 仕訳区分(前受計上仕訳)
      cbxAdvanceReceivedTransferCtrl: this.cbxAdvanceReceivedTransferCtrl,  // 仕訳区分(前受振替仕訳)

      undefineCtrl: this.undefineCtrl, // 未定用

    });

  }

  public setFormatter() {

    if (this.userInfoService.ApplicationControl.customerCodeType == CODE_TYPE.NUMBER) {
      FormatterUtil.setNumberFormatter(this.customerCodeCtrl);
    }
    else if (this.userInfoService.ApplicationControl.customerCodeType == CODE_TYPE.HANKANA) {
      FormatterUtil.setCustomerCodeKanaFormatter(this.customerCodeCtrl);
    }
    else {
      FormatterUtil.setCustomerCodeAlphaFormatter(this.customerCodeCtrl);
    }

    FormatterUtil.setAlphabetFormatter(this.currencyCodeCtrl); // 通貨コード

  }


  public setAutoComplete(){

    // 得意先
    this.initAutocompleteCustomers(this.customerCodeCtrl,this.customerService,0);

  }

  public openMasterModal(table: TABLE_INDEX ) {

    this.customerCodeTrigger.closePanel();

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
          case TABLE_INDEX.MASTER_CUSTOMER:
            {
              this.customerCodeCtrl.setValue(componentRef.instance.SelectedCode);
              this.customerNameCtrl.setValue(componentRef.instance.SelectedName);
              this.customerId = componentRef.instance.SelectedId;

              break;
            }

        }

      }
      componentRef.destroy();
    });

  }

  public clear() {
    this.MyFormGroup.reset();
    //this.matchingJournalizingDetailsResult = null;

    this.cbxMatchingCtrl.setValue("true");
    this.cbxReceiptExcludeCtrl.setValue("true");
    this.cbxAdvanceReceivedOccuredCtrl.setValue("true");
    this.cbxAdvanceReceivedTransferCtrl.setValue("true");

    this.selectJournal = false;

    this.processCustomResult = this.processResultService.clearProcessCustomMsg(this.processCustomResult);;

    HtmlUtil.nextFocusByName(this.elementRef, 'updatedAtFromCtrl', EVENT_TYPE.NONE);

    this.panelOpenState = true;
    this.panel.open();
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

      case BUTTON_ACTION.SEARCH:
        this.search();
        break;

      case BUTTON_ACTION.CANCEL:
        this.cancel();
        break;
      default:
        console.log('buttonAction Error.');
        break;
    }
    
  }  


  public search() {
    this.processCustomResult = this.processResultService.clearProcessCustomMsg(this.processCustomResult);;

    let journalizingOption = this.GetSearchDataCondition();
    this.SearchMatchingJournalizingDetail(journalizingOption)

  }

  public GetSearchDataCondition(): JournalizingOption {
    var option = new JournalizingOption();
    option.companyId = this.userInfoService.Company.id;
    option.createAtFrom = DateUtil.ConvertFromDatepickerToStart(this.updatedAtFromCtrl);
    option.createAtTo = DateUtil.ConvertFromDatepickerToEnd(this.updatedAtToCtrl);
    if (!StringUtil.IsNullOrEmpty(this.customerCodeCtrl.value)) option.customerId = this.customerId;
    if (!StringUtil.IsNullOrEmpty(this.currencyCodeCtrl.value)) option.currencyId = this.currencyCodeCtrl.value;
    var types = new Array<number>();
    if (this.cbxMatchingCtrl.value) types.push(JournalizingType.Matching);
    if (this.cbxAdvanceReceivedOccuredCtrl.value) types.push(JournalizingType.AdvanceReceivedOccured);
    if (this.cbxAdvanceReceivedTransferCtrl.value) types.push(JournalizingType.AdvanceReceivedTransfer);
    if (this.cbxReceiptExcludeCtrl.value) types.push(JournalizingType.ReceiptExclude);
    option.journalizingTypes = types;

    //ReportSearchCondition();

    return option;
  }

  /// <summary> SearchMatchingJournalizingDetail Data </summary>
  public SearchMatchingJournalizingDetail(journalizingOption: JournalizingOption) {

    let componentFactory = this.componentFactoryResolver.resolveComponentFactory(ModalRouterProgressComponent);
    let modalRouterProgressComponentRef = this.viewContainerRef.createComponent(componentFactory);
    //modalRouterProgressComponentRef.destroy();

    let list: Array<MatchingJournalizingDetail> = null;

    this.matchingJournalizingDetailsResult = new MatchingJournalizingDetailsResult();

    this.matchingService.GetMatchingJournalizingDetail(journalizingOption)
      .subscribe(response => {

        this.processCustomResult = this.processResultService.processAtGetData(this.processCustomResult, response, true, this.partsResultMessageComponent);

        if (response != PROCESS_RESULT_RESULT_TYPE.FAILURE) {
          this.matchingJournalizingDetailsResult.matchingJournalizingDetails = response;
          if (
            this.matchingJournalizingDetailsResult.matchingJournalizingDetails == null
            || this.matchingJournalizingDetailsResult.matchingJournalizingDetails.length == 0
          ) {
            this.processCustomResult = this.processResultService.processAtWarning(
              this.processCustomResult, MSG_WNG.NOT_EXIST_SEARCH_DATA, this.partsResultMessageComponent);

            this.panelOpenState=true;
          }
          else {
            this.panelOpenState=false;
            this.cbxDeleteFlgCtrls = new Array<FormControl>(this.matchingJournalizingDetailsResult.matchingJournalizingDetails.length);

            for (let index = 0; index < this.matchingJournalizingDetailsResult.matchingJournalizingDetails.length; index++) {
              this.cbxDeleteFlgCtrls[index] = new FormControl(null);
              this.MyFormGroup.removeControl("cbxDeleteFlgCtrl" + index);
              this.MyFormGroup.addControl("cbxDeleteFlgCtrl" + index, this.cbxDeleteFlgCtrls[index]);
            }
          }
        }

        modalRouterProgressComponentRef.destroy();        
      });
  }

  public checkAll(){
    if(this.cbxCheckAllCtrl.value){
      this.selectAll();
    }
    else{
      this.clearAll();
    }
  }


  public selectAll() {
    this.processCustomResult = this.processResultService.clearProcessCustomMsg(this.processCustomResult);;

    if (this.matchingJournalizingDetailsResult == null) {
      this.selectJournal = false;
      return;
    }

    for (let index = 0; index < this.matchingJournalizingDetailsResult.matchingJournalizingDetails.length; index++) {
      this.cbxDeleteFlgCtrls[index].setValue("true");
    }

    if(this.cbxDeleteFlgCtrls.length==0){
      this.selectJournal = false;
    }
    else{
      this.selectJournal = true;
    }
  }

  public clearAll() {
    this.processCustomResult = this.processResultService.clearProcessCustomMsg(this.processCustomResult);;

    if (this.matchingJournalizingDetailsResult == null) return;

    for (let index = 0; index < this.matchingJournalizingDetailsResult.matchingJournalizingDetails.length; index++) {
      this.cbxDeleteFlgCtrls[index].setValue(null);
    }

    this.selectJournal = false;
  }


  public cancel() {
    this.processCustomResult = this.processResultService.clearProcessCustomMsg(this.processCustomResult);;

    // 削除対象
    let deleteList = new Array<MatchingJournalizingDetail>();

    // チェックされたもの（仕訳出力対象）
    let checkList = new Array<MatchingJournalizingDetail>();

    // 重複制御
    var listId = new Array<number>();

    if (!this.selectJournal) return;


    for (let index = 0; index < this.matchingJournalizingDetailsResult.matchingJournalizingDetails.length; index++) {
      if (this.cbxDeleteFlgCtrls[index].value) {

        checkList.push(this.matchingJournalizingDetailsResult.matchingJournalizingDetails[index]);
        if (!listId.includes(this.matchingJournalizingDetailsResult.matchingJournalizingDetails[index].headerId)) {
          listId.push(this.matchingJournalizingDetailsResult.matchingJournalizingDetails[index].headerId);

          let deleatJournal = new MatchingJournalizingDetail();
          deleatJournal.journalizingType = this.matchingJournalizingDetailsResult.matchingJournalizingDetails[index].journalizingType;
          deleatJournal.id = this.matchingJournalizingDetailsResult.matchingJournalizingDetails[index].headerId;
          deleatJournal.updateBy = this.userInfoService.LoginUser.id;
          //deleatJournal.updateAt = this.matchingJournalizingDetailsResult.matchingJournalizingDetails[index].updateAt;

          deleteList.push(deleatJournal);
        }
      }
    }


    if (deleteList.length > 0) {


      // 削除処理
      let componentFactory = this.componentFactoryResolver.resolveComponentFactory(ModalConfirmComponent);
      let componentRef = this.viewContainerRef.createComponent(componentFactory);
      componentRef.instance.ActionName = "削除"
      componentRef.instance.Closing.subscribe(() => {

        if (componentRef.instance.ModalStatus == MODAL_STATUS_TYPE.OK) {
          componentRef.destroy();

          if (!this.Export(checkList)) return;
          this.Print(checkList);
          this.CancelJournal(deleteList);

        }
        else {
          componentRef.destroy();
        }
      });

    }
    else {
      this.processCustomResult = this.processResultService.processAtWarning(
        this.processCustomResult, MSG_WNG.SELECTION_REQUIRED.replace(MSG_ITEM_NUM.FIRST, '取込を行うデータ'),
        this.partsResultMessageComponent);
    }
  }


  public Export(checkList: Array<MatchingJournalizingDetail>): boolean {

    let data: string = "";
    let dataList = checkList;

    for (let index = 0; index < dataList.length; index++) {
      let dataItem: Array<any> = [];
      dataItem.push(this.userInfoService.Company.code);
      dataItem.push(DateUtil.getYYYYMMDD(5, dataList[index].createAt));
      dataItem.push(JournalizingName[dataList[index].journalizingType]);
      dataItem.push(dataList[index].customerCode);
      dataItem.push(dataList[index].customerName);
      if (this.userInfoService.ApplicationControl.useForeignCurrency == 1) {
        dataItem.push(dataList[index].currencyCode);
      }
      dataItem.push(dataList[index].amount);
      dataItem.push(DateUtil.getYYYYMMDD(5, dataList[index].outputAt));
      dataItem.push(dataList[index].receiptAmount);
      dataItem.push(DateUtil.getYYYYMMDD(1, dataList[index].recordedAt));
      dataItem.push(dataList[index].payerName);
      dataItem.push(DateUtil.getYYYYMMDD(1, dataList[index].billedAt));
      dataItem.push(dataList[index].invoiceCode);
      dataItem.push(dataList[index].billingAmount);

      data = data + dataItem.join(",") + LINE_FEED_CODE;
    }

    let resultDatas: Array<any> = [];
    resultDatas.push(data);

    FileUtil.download(resultDatas, this.Title + DateUtil.getYYYYMMDD(0), FILE_EXTENSION.CSV);

    return true;
  }

  public Print(checkList: Array<MatchingJournalizingDetail>) {
    let cacel = new MatchingJournalizingCancellationReportSource();

    cacel.companyId = this.userInfoService.Company.id;
    cacel.precision = this.userInfoService.Currency.precision;
    cacel.items = checkList;

    this.matchingService.GetJournalizingCancelReport(cacel)
      .subscribe(response => {
        FileUtil.download([response.body], this.Title + DateUtil.getYYYYMMDD(0), FILE_EXTENSION.PDF);
      });


  }

  public CancelJournal(deleteList: Array<MatchingJournalizingDetail>) {

    let componentProgressFactory = this.componentFactoryResolver.resolveComponentFactory(ModalRouterProgressComponent);
    let componentProgressRef = this.viewContainerRef.createComponent(componentProgressFactory);

    this.matchingService.CancelMatchingJournalizingDetail(deleteList)
      .subscribe(response => {
        if (response != undefined) {
          let matchingJournalizingProcessResult = response;
          this.clear();
        }
        else {
          this.processCustomResult = this.processResultService.processAtFailure(
            this.processCustomResult, MSG_ERR.SOMETHING_ERROR.replace(MSG_ITEM_NUM.FIRST, '取消処理'),
            this.partsResultMessageComponent);
        }
        componentProgressRef.destroy();
      });

  }

  ///////////////////////////////////////////////////////////////////////
  public setUpdatedAtFrom(eventType: string) {
    HtmlUtil.nextFocusByName(this.elementRef, 'updatedAtToCtrl', eventType);
  }

  public setUpdatedAtTo(eventType: string) {
    HtmlUtil.nextFocusByName(this.elementRef, 'customerCodeCtrl', eventType);
  }


  ///////////////////////////////////////////////////////////////////////

  public setCustomerCode(eventType: string) {

    if (eventType != EVENT_TYPE.BLUR) {
      this.customerCodeTrigger.closePanel();
    }

    if (!StringUtil.IsNullOrEmpty(this.customerCodeCtrl.value)) {
  
      this.loadStart();
      this.customerService.GetItems(this.customerCodeCtrl.value)
        .subscribe(response => {
          this.loadEnd();
          if (response != undefined && response.length > 0) {
            this.customerCodeCtrl.setValue(response[0].code);
            this.customerNameCtrl.setValue(response[0].name);
            this.customerId = response[0].id;
            HtmlUtil.nextFocusByNames(this.elementRef, ['currencyCodeCtrl','cbxMatchingCtrl'], eventType);
          }
          else {
            // this.customerCodeCtrl.setValue("");
            this.customerNameCtrl.setValue("");
            this.customerId = null;
          }
        });
    }
    else {
      this.customerCodeCtrl.setValue("");
      this.customerNameCtrl.setValue("");
      HtmlUtil.nextFocusByNames(this.elementRef, ['currencyCodeCtrl', 'cbxMatchingCtrl'], eventType);
    }
  }

  ///////////////////////////////////////////////////////////////////////
  public setCurrencyCode(eventType: string) {

    if (!StringUtil.IsNullOrEmpty(this.currencyCodeCtrl.value)) {

      this.currencyService.GetItems(this.currencyCodeCtrl.value)
        .subscribe(response => {
          if (response != undefined && response.length > 0) {
            this.currencyCodeCtrl.setValue(response[0].code);
            HtmlUtil.nextFocusByName(this.elementRef, 'updatedAtFromCtrl', eventType);
          }
          else {
            this.currencyCodeCtrl.setValue("");
            HtmlUtil.nextFocusByName(this.elementRef, 'updatedAtFromCtrl', eventType);
          }
        });
    }
    else {
      this.currencyCodeCtrl.setValue("");
      HtmlUtil.nextFocusByName(this.elementRef, 'updatedAtFromCtrl', eventType);
    }

  }


  public setCbxDeleteFlg(index) {
    this.selectJournal = false;

    let selectedCustomerCode = this.matchingJournalizingDetailsResult.matchingJournalizingDetails[index].customerCode;
    let setFlag: string;
    if (this.cbxDeleteFlgCtrls[index].value) {
      setFlag = "true";
    }
    else {
      setFlag = null;
    }

    for (let index = 0; index < this.matchingJournalizingDetailsResult.matchingJournalizingDetails.length; index++) {
      if (this.matchingJournalizingDetailsResult.matchingJournalizingDetails[index].customerCode == selectedCustomerCode) {
        this.cbxDeleteFlgCtrls[index].setValue(setFlag);
      }
    }

    this.cbxDeleteFlgCtrls.forEach(element => { if (element.value) { this.selectJournal = true; } });
  }

}