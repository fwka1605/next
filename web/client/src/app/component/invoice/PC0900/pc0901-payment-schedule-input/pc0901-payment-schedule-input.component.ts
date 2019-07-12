import { Component, OnInit, ElementRef, ComponentFactoryResolver, ViewContainerRef, AfterViewInit, ViewChild } from '@angular/core';
import { BaseComponent } from 'src/app/component/common/base/base-component';
import { Router, ParamMap, ActivatedRoute, NavigationEnd } from '@angular/router';
import { CategoryMasterService } from 'src/app/service/Master/category-master.service';
import { TABLE_INDEX } from 'src/app/common/const/table-name.const';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { ModalMasterComponent } from 'src/app/component/modal/modal-master/modal-master.component';
import { BillingsResult } from 'src/app/model/billings-result.model';
import { BillingSearch } from 'src/app/model/billing-search.model';
import { BillingService } from 'src/app/service/billing.service';
import { HtmlUtil } from 'src/app/common/util/html.util';
import { KEY_CODE, EVENT_TYPE, BUTTON_ACTION } from 'src/app/common/const/event.const';
import { DepartmentMasterService } from 'src/app/service/Master/department-master.service';
import { StaffMasterService } from 'src/app/service/Master/staff-master.service';
import { CustomerMasterService } from 'src/app/service/Master/customer-master.service';
import { FormatterUtil } from 'src/app/common/util/formatter.util';
import { BILL_INPUT_TYPE_DICTIONARY, MATCHING_ASSIGNMENT_FLAG_DICTIONARY, BANK_TRANSFER_RESULT_DICTIONARY, BILLING_AMOUNT_RANGE_DICTIONARY, CategoryType, GridId, CODE_TYPE } from 'src/app/common/const/kbn.const';
import { RacCurrencyPipe } from 'src/app/pipe/currency.pipe';
import { MODAL_STATUS_TYPE, PROCESS_RESULT_RESULT_TYPE } from 'src/app/common/const/status.const';
import { UserInfoService } from 'src/app/service/common/user-info.service';
import { NgbDateParserFormatter, NgbDate } from '@ng-bootstrap/ng-bootstrap';
import { NgbDateCustomParserFormatter } from 'src/app/common/util/dateformat-util';
import { CurrencyMasterService } from 'src/app/service/Master/currency-master.service';
import { GeneralSetting } from 'src/app/model/general-setting.model';
import { Billing } from 'src/app/model/billing.model';
import { BillingHelper } from 'src/app/model/helper/billing-helper.model';
import { Category } from 'src/app/model/category.model';
import { Currency } from 'src/app/model/currency.model';
import { GridSettingsResult } from 'src/app/model/grid-settings-result.model';
import { JuridicalPersonalitysResult } from 'src/app/model/juridical-personalitys-result.model';
import { GridSettingMasterService } from 'src/app/service/Master/grid-setting-master.service';
import { JuridicalPersonalityMasterService } from 'src/app/service/Master/juridical-personality-master.service';
import { forkJoin } from 'rxjs';
import { StringUtil } from 'src/app/common/util/string-util';
import { DateUtil } from 'src/app/common/util/date-util';
import { NumberUtil } from 'src/app/common/util/number-util';
import { FILE_EXTENSION } from 'src/app/common/const/eb-file.const';
import { FileUtil } from 'src/app/common/util/file.util';
import { LINE_FEED_CODE } from 'src/app/common/const/report-setting-kbn.const';
import { ModalRouterProgressComponent } from 'src/app/component/modal/modal-router-progress/modal-router-progress.component';
import { GridSetting } from 'src/app/model/grid-setting.model';
import { GridSettingHelper } from 'src/app/model/grid-setting-helper.model';
import { MatchingService } from 'src/app/service/matching.service';
import { ComponentId } from 'src/app/common/const/component-name.const';
import { ProcessResultService } from 'src/app/service/common/process-result.service';
import { MSG_WNG, MSG_ITEM_NUM,  MSG_INF } from 'src/app/common/const/message.const';
import { LocalStorageManageService } from 'src/app/service/common/local-storage-manage.service';
import { LoginUserMasterService } from 'src/app/service/Master/login-user-master.service';
import { LocalStorageItem } from 'src/app/model/custom-model/local-storage-item';
import { RangeSearchKey } from 'src/app/common/const/local-storage-key-const';
import {  MatAutocompleteTrigger } from '@angular/material';
import { PageUtil } from 'src/app/common/util/page-util';
import { element } from '@angular/core/src/render3';


@Component({
  selector: 'app-pc0901-payment-schedule-input',
  templateUrl: './pc0901-payment-schedule-input.component.html',
  styleUrls: ['./pc0901-payment-schedule-input.component.css']
})
export class Pc0901PaymentScheduleInputComponent extends BaseComponent implements OnInit ,AfterViewInit{

  errorMessage: string;

  public panelOpenState: boolean;

  public readonly inputTypeDictionary = BILL_INPUT_TYPE_DICTIONARY;
  public readonly assignmentFlagDictionary = MATCHING_ASSIGNMENT_FLAG_DICTIONARY;
  public readonly transferResultDictionary = BANK_TRANSFER_RESULT_DICTIONARY;
  public readonly amountRangeDictionary = BILLING_AMOUNT_RANGE_DICTIONARY;


  // これをしないとEnum型がテンプレートで見えなかったため
  public TABLE_INDEX: typeof TABLE_INDEX = TABLE_INDEX;
  public Component_Id: typeof ComponentId = ComponentId;
  
  //////////////////////////////////////////////////////////////////////
  public selectBilling: Billing;

  public billingsResult: BillingsResult;
  public billingSearch: BillingSearch;
  public billingCategory: Category;
  public currency: Currency;
  public bsBillingCategoryId: number;
  public currencyId: number;

  /////////////////////////////////////////////////////////////////////////////

  public customerCodeCtrl: FormControl;  // 得意先コード
  public customerNameCtrl: FormControl;
  public customerId:number;

  public billedAtFromCtrl: FormControl;  // 請求日
  public billedAtToCtrl: FormControl;

  public dueAtFromCtrl: FormControl;  // 入金予定日
  public dueAtToCtrl: FormControl;

  public paymentCodeKeyCtrl:FormControl;  // 入金予定キー

  public currencyCodeCtrl: FormControl; // 通貨コード

  public invoiceCodeFromCtrl: FormControl;  // 請求書番号
  public invoiceCodeToCtrl: FormControl;
  public invoiceCodeCtrl: FormControl;

  public billingCategoryCodeCtrl: FormControl;  // 請求区分
  public billingCategoryNameCtrl: FormControl;

  public cbxUseReceiptSectionCtrl: FormControl; // 入金部門対応マスタを使用

  public undefineCtrl: FormControl; // 未定用

  /////////////////////////////////////////////////////////////////////////////

  public detailCbxUpdateFlagCtrls: Map<number,FormControl>; // 更新
  public detailPaymentAmountCtrls: Map<number,FormControl>; // 入金予定額
  public detailOffsetAmountCtrls:Map<number,FormControl>; // 違算

  /////////////////////////////////////////////////////////////////////////////

  public isDisableBackBtn: boolean;
  public paramFrom: ComponentId;
  public departmentIdsInner: number[];
  public AllGridSettings: GridSetting[];
  public gridSettingHelper = new GridSettingHelper();

  public dispSumCount: number;
  public dispSumBillingAmount: number;
  public dispSumRemainAmount: number;

  public refProcess: string;

  // 必須項目の選択値(相殺用の画面に渡すために設定)
  // 顧客・通貨指定時に下記の変数も設定が必要。
  public currentCustomerId: number;
  public currentCustomerCode: string;
  public currentCustomerName: string;
  public currentCustomerParentFlag:number;
  public currentCurrencyId: number;
  public currentCurrencyCode: string;



  @ViewChild('billingCategoryCodeInput', { read: MatAutocompleteTrigger }) billingCategoryCodeTrigger: MatAutocompleteTrigger;
  @ViewChild('customerCodeInput', { read: MatAutocompleteTrigger }) customerCodeTrigger: MatAutocompleteTrigger;

  constructor(
    public router: Router,
    public activatedRoute: ActivatedRoute,
    public elementRef: ElementRef,
    public componentFactoryResolver: ComponentFactoryResolver,
    public viewContainerRef: ViewContainerRef,
    public userInfoService: UserInfoService,
    public categoryService: CategoryMasterService,
    public billingService: BillingService,
    public departmentService: DepartmentMasterService,
    public staffService: StaffMasterService,
    public customerService: CustomerMasterService,
    public currencyService: CurrencyMasterService,
    public gridSettingService: GridSettingMasterService,
    public juridicalPersonalityService: JuridicalPersonalityMasterService,
    public matchingService: MatchingService,
    public processResultService: ProcessResultService,
    public loginUserService: LoginUserMasterService,
    public billingHelper:      BillingHelper,
    public localStorageManageService:LocalStorageManageService,
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

  }

  ngAfterViewInit(){
    //HtmlUtil.nextFocusByName(this.elementRef, 'customerCodeCtrl', EVENT_TYPE.NONE);
  }

  public setControlInit() {

    this.customerCodeCtrl = new FormControl("", [Validators.required,Validators.maxLength(this.userInfoService.ApplicationControl.customerCodeLength),]);  // 得意先コード
    this.customerNameCtrl = new FormControl("");

    this.billedAtFromCtrl = new FormControl("", [Validators.maxLength(10)]);  // 請求日
    this.billedAtToCtrl = new FormControl("", [Validators.maxLength(10)]);

    this.dueAtFromCtrl = new FormControl("", [Validators.maxLength(10)]);  // 入金予定日
    this.dueAtToCtrl = new FormControl("", [Validators.maxLength(10)]);

    this.paymentCodeKeyCtrl = new FormControl("");  // 入金予定キー

    if(this.userInfoService.ApplicationControl.useForeignCurrency==1){
      this.currencyCodeCtrl = new FormControl("", [Validators.required,Validators.maxLength(3),]); // 通貨コード
    }
    else{
      this.currencyCodeCtrl = new FormControl("", [Validators.maxLength(3),]); // 通貨コード
    }

    this.invoiceCodeFromCtrl = new FormControl("", [Validators.maxLength(20),]);  // 請求書番号
    this.invoiceCodeToCtrl = new FormControl("", [Validators.maxLength(20),]);
    this.invoiceCodeCtrl = new FormControl("", [Validators.maxLength(20),]);

    this.billingCategoryCodeCtrl = new FormControl("", [Validators.maxLength(2),]);  // 請求区分
    this.billingCategoryNameCtrl = new FormControl("");

    this.cbxUseReceiptSectionCtrl = new FormControl(""); // 入金部門対応マスタを使用

    this.undefineCtrl = new FormControl(""); // 未定用;

  }

  public setValidator() {
    this.MyFormGroup = new FormGroup({

      customerCodeCtrl: this.customerCodeCtrl,  // 得意先コード
      customerNameCtrl: this.customerNameCtrl,
      
      billedAtFromCtrl: this.billedAtFromCtrl,  // 請求日
      billedAtToCtrl: this.billedAtToCtrl,

      dueAtFromCtrl: this.dueAtFromCtrl,  // 入金予定日
      dueAtToCtrl: this.dueAtToCtrl,

      paymentCodeKeyCtrl:this.paymentCodeKeyCtrl, // 入金予定キー

      currencyCodeCtrl: this.currencyCodeCtrl, // 通貨コード

      invoiceCodeFromCtrl: this.invoiceCodeFromCtrl,  // 請求書番号
      invoiceCodeToCtrl: this.invoiceCodeToCtrl,
      invoiceCodeCtrl: this.invoiceCodeCtrl,

      billingCategoryCodeCtrl: this.billingCategoryCodeCtrl,  // 請求区分
      billingCategoryNameCtrl: this.billingCategoryNameCtrl,
      
      cbxUseReceiptSectionCtrl: this.cbxUseReceiptSectionCtrl, // 入金部門対応マスタを使用

      undefineCtrl: this.undefineCtrl, // 未定用;

    });


  }

  public setFormatter() {

    FormatterUtil.setNumberFormatter(this.billingCategoryCodeCtrl);// 請求区分


    if (this.userInfoService.ApplicationControl.customerCodeType == CODE_TYPE.NUMBER) {
      FormatterUtil.setNumberFormatter(this.customerCodeCtrl); // 得意先コード
    }
    else if (this.userInfoService.ApplicationControl.customerCodeType == CODE_TYPE.HANKANA)  {
      FormatterUtil.setCustomerCodeKanaFormatter(this.customerCodeCtrl);
    }
    else {
      FormatterUtil.setCustomerCodeAlphaFormatter(this.customerCodeCtrl);
    }

    FormatterUtil.setAlphabetFormatter(this.currencyCodeCtrl); // 通貨コード
  }

  public setAutoComplete(){

    // 請求区分
    this.initAutocompleteCategories(CategoryType.Billing,this.billingCategoryCodeCtrl,this.categoryService,0);
    
    // 得意先
    this.initAutocompleteCustomers(this.customerCodeCtrl,this.customerService,0);

  }

  /*
    table:Master名称
    keyCode:キーコードがある場合はF9のみモーダルを開く
    index:明細行の行No
  */
  public openMasterModal(table: TABLE_INDEX, index: number = -1) {


    this.billingCategoryCodeTrigger.closePanel();
    this.customerCodeTrigger.closePanel();


    let componentFactory = this.componentFactoryResolver.resolveComponentFactory(ModalMasterComponent);
    let componentRef = this.viewContainerRef.createComponent(componentFactory);

    componentRef.instance.TableIndex = table;

    componentRef.instance.Closing.subscribe(() => {

      if (componentRef.instance.ModalStatus == MODAL_STATUS_TYPE.SELECT) {
        switch (table) {
          case TABLE_INDEX.MASTER_CUSTOMER:
            {
              this.customerCodeCtrl.setValue(componentRef.instance.SelectedCode);
              this.customerNameCtrl.setValue(componentRef.instance.SelectedName);
              this.customerId = componentRef.instance.selectedId;

              break;
            }
          case TABLE_INDEX.MASTER_CURRENCY:
            {
              this.currencyCodeCtrl.setValue(componentRef.instance.SelectedCode);
              this.currency = componentRef.instance.SelectedObject;
              this.currencyId = this.currency.id;
              break;
            }
        }
      }

      componentRef.destroy();
    });

  }

  public clear() {
    this.panelOpenState = true;
    this.MyFormGroup.reset();

    this.customerId=0;

    this.billingsResult=null;
    this.detailCbxUpdateFlagCtrls = null; // 更新
    this.detailPaymentAmountCtrls = null; // 入金予定額
    this.detailOffsetAmountCtrls = null; // 違算

  
    //HtmlUtil.nextFocusByName(this.elementRef, 'customerCodeCtrl', EVENT_TYPE.NONE);

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

      case BUTTON_ACTION.UPDATE:
        this.search();
        break;

      case BUTTON_ACTION.NEETING_INPUT:
        this.search();
        break;
      case BUTTON_ACTION.EXPORT:
        this.export();
        break;
      case BUTTON_ACTION.SELECT_ALL:
        this.selectAll();
        break;
      case BUTTON_ACTION.CANCEL_ALL:
        this.cancelAll();
        break;

      default:
        console.log('buttonAction Error.');
        break;
    } 
  }

  public search() {

    //入力チェック必須　日付等　fromDate > toDate
    if (!this.isValidSearchOption()) return;

    this.billingSearch = this.searchCondition();

    let confirmComponentFactory = this.componentFactoryResolver.resolveComponentFactory(ModalRouterProgressComponent);
    let confirmComponentRef = this.viewContainerRef.createComponent(confirmComponentFactory);

    this.billingService.GetItems(this.billingSearch)
      .subscribe(response => {

        this.processCustomResult = this.processResultService.processAtGetData(
          this.processCustomResult, response, true, this.partsResultMessageComponent);
        if (response != PROCESS_RESULT_RESULT_TYPE.FAILURE && response.length > 0) {
          this.billingsResult = new BillingsResult();
          this.billingsResult.billings = new Array<Billing>();
          this.billingsResult.billings = response;

          this.setSearchResult();
          this.panelOpenState = false;
          this.setDispFooterTotalSum();

        }
        else {
          this.billingsResult = new BillingsResult();
          this.billingsResult.billings = new Array<Billing>();

          this.panelOpenState = true;

          this.detailCbxUpdateFlagCtrls = null; // 更新
          this.detailPaymentAmountCtrls = null; // 入金予定額
          this.detailOffsetAmountCtrls = null;  // 違算

        }

        confirmComponentRef.destroy();
      });
  }

  public isValidSearchOption(): boolean {
    let msg: string = "";

    if (!DateUtil.isValidRange(this.billedAtFromCtrl, this.billedAtToCtrl)) {
      msg = MSG_WNG.INPUT_VALID_DATE_FOR_PARAMETERS.replace(MSG_ITEM_NUM.FIRST, '請求日終了')
        .replace(MSG_ITEM_NUM.SECOND, '請求日開始');
    }
    
    if (!DateUtil.isValidRange(this.dueAtFromCtrl, this.dueAtToCtrl)) {
      msg = MSG_WNG.INPUT_VALID_DATE_FOR_PARAMETERS.replace(MSG_ITEM_NUM.FIRST, '入金予定日終了')
        .replace(MSG_ITEM_NUM.SECOND, '入金予定日開始');
    }

    // 請求書番号の範囲チェックはしない。

    if (msg.length != 0) {
      this.processCustomResult = this.processResultService.processAtWarning(
        this.processCustomResult, msg, this.partsResultMessageComponent);
      return false;
    }

    return true;
  }

  public searchCondition(): BillingSearch {
    let billingItem = new BillingSearch();

    billingItem.companyId = this.userInfoService.Company.id;
    billingItem.loginUserId = this.userInfoService.LoginUser.id;

    billingItem.bsCustomerCode = this.customerCodeCtrl.value;  //  得意先コード
    billingItem.isParentFlg = this.currentCustomerParentFlag;
    billingItem.customerId = this.customerId;  //  得意先コード
    
    
    billingItem.bsBilledAtFrom = DateUtil.ConvertFromDatepicker(this.billedAtFromCtrl);  //  請求日
    billingItem.bsBilledAtTo = DateUtil.ConvertFromDatepicker(this.billedAtToCtrl);

    billingItem.bsDueAtFrom = DateUtil.ConvertFromDatepicker(this.dueAtFromCtrl);  //  入金予定日
    billingItem.bsDueAtTo = DateUtil.ConvertFromDatepicker(this.dueAtToCtrl);

    billingItem.bsScheduledPaymentKey = this.paymentCodeKeyCtrl.value;  // 入金予定キー

    if(this.userInfoService.ApplicationControl.useForeignCurrency==1){
      billingItem.currencyId = this.currencyId;  //  通貨コード
    }

    billingItem.bsInvoiceCodeFrom = this.invoiceCodeFromCtrl.value;  //  請求書番号
    billingItem.bsInvoiceCodeTo = this.invoiceCodeToCtrl.value;
    billingItem.bsInvoiceCode = this.invoiceCodeCtrl.value;

    billingItem.bsBillingCategoryId = this.bsBillingCategoryId;  //  請求区分

    billingItem.useSectionMaster = 
          this.userInfoService.ApplicationControl.useReceiptSection == 1 
        && this.cbxUseReceiptSectionCtrl.value;  //  入金部門対応マスターを使用


    // 消込・未消込・一部消込
    // 2進数
    // 消込済み；0x004 = 4
    // 一部	；0x002 = 2
    // 未消込	；0x001 = 1
    billingItem.assignmentFlg = 0;
    billingItem.assignmentFlg = billingItem.assignmentFlg + 2;
    billingItem.assignmentFlg = billingItem.assignmentFlg + 1;

    return billingItem;
  }


  public setSearchResult() {
    // 戻る対応のための検索結果を退避
    this.billingService.BillingSearch = this.billingSearch;
    this.billingService.BillingSearchFormGroup = this.MyFormGroup;

    // 対象外の設定
    this.detailCbxUpdateFlagCtrls = new Map<number,FormControl>();
    this.detailPaymentAmountCtrls = new Map<number,FormControl>();
    this.detailOffsetAmountCtrls = new Map<number,FormControl>();
    

    for (let index: number = 0; index < this.billingsResult.billings.length; index++) {

      let cbxUpdateFlagCtrl = new FormControl("true");


      const pipe = new RacCurrencyPipe();
      let paymentAmountCtrl = new FormControl(pipe.transform(""+(this.billingsResult.billings[index].remainAmount-this.billingsResult.billings[index].offsetAmount),false));
      let offsetAmountCtrl = new FormControl(""+pipe.transform(""+this.billingsResult.billings[index].offsetAmount,false));

      this.detailCbxUpdateFlagCtrls.set(this.billingsResult.billings[index].id,cbxUpdateFlagCtrl);
      this.detailPaymentAmountCtrls.set(this.billingsResult.billings[index].id,paymentAmountCtrl);
      this.detailOffsetAmountCtrls.set(this.billingsResult.billings[index].id,offsetAmountCtrl);

      if(this.billingsResult.billings[index].remainAmount<0){
        FormatterUtil.setCurrencyFormatter(offsetAmountCtrl);// 違算
      }
      else{
        FormatterUtil.setCurrencyFormatter(offsetAmountCtrl, false);// 違算
      }

      this.MyFormGroup.removeControl("detailCbxUpdateFlagCtrl" + index);
      this.MyFormGroup.removeControl("detailPaymentAmountCtrl" + index);
      this.MyFormGroup.removeControl("detailOffsetAmountCtrl" + index);

      this.MyFormGroup.addControl("detailCbxUpdateFlagCtrl" + index, cbxUpdateFlagCtrl);
      this.MyFormGroup.addControl("detailPaymentAmountCtrl" + index, paymentAmountCtrl);
      this.MyFormGroup.addControl("detailOffsetAmountCtrl" + index, offsetAmountCtrl);

      paymentAmountCtrl.valueChanges.subscribe(
        (value: string) => {
          if (typeof value === "string") {
            if (value.match(/[^-0-9,.]/)) {
              paymentAmountCtrl.setValue(value.replace(/[^-0-9,.]/g, ''));
            }
            else {
              this.setDetailPaymentAmount(index);
            }
          }
        });

    }

    this.selectAll();

    // 選択行の設定
    this.selectBilling = this.billingsResult.billings[0];
  }

  /**
   * エクスポート処理
   */
  public export() {

    // 動作中のコンポーネントを開く
    let componentFactory = this.componentFactoryResolver.resolveComponentFactory(ModalRouterProgressComponent);
    let modalRouterProgressComponentRef = this.viewContainerRef.createComponent(componentFactory);
    modalRouterProgressComponentRef.instance.Closing.subscribe(() => {
      modalRouterProgressComponentRef.destroy();
    });
    
    this.processCustomResult = this.processResultService.clearProcessCustomMsg(this.processCustomResult);

    let data: string = "";
    let dataList = this.billingsResult.billings;


    for (let index = 0; index < dataList.length; index++) {
      let dataItem: Array<any> = [];


      data = data + dataItem.join(",") + LINE_FEED_CODE;
    }
    let resultDatas: Array<any> = [];
    resultDatas.push(data);

    FileUtil.download(resultDatas, "請求データ" + DateUtil.getYYYYMMDD(0), FILE_EXTENSION.CSV);

    this.processResultService.processAtSuccess(
      this.processCustomResult, MSG_INF.PROCESS_FINISH, this.partsResultMessageComponent);
    this.processResultService.createdLog(this.processCustomResult.logData);

    modalRouterProgressComponentRef.destroy();

  }
  
  public selectAll(){
    this.detailCbxUpdateFlagCtrls.forEach(element=>{element.setValue("true");})
    this.detailPaymentAmountCtrls.forEach(element=>{element.enable();})
  }

  public cancelAll(){
    this.detailCbxUpdateFlagCtrls.forEach(element=>{element.setValue("");})
    this.detailPaymentAmountCtrls.forEach(element=>{element.disable();})
  }


  /////////////////////////////////////////////////////////////////////////////////

  public setCustomerCode(eventType: string) {

    if(eventType!=EVENT_TYPE.BLUR){
      this.customerCodeTrigger.closePanel();
    }

    if (!StringUtil.IsNullOrEmpty(this.customerCodeCtrl.value)) {

      if (this.userInfoService.ApplicationControl.customerCodeType == CODE_TYPE.NUMBER) {
        this.customerCodeCtrl.setValue(StringUtil.setPaddingFrontZero(this.customerCodeCtrl.value, this.userInfoService.ApplicationControl.customerCodeLength));
      }      

      this.loadStart();
      this.customerService.GetItems(this.customerCodeCtrl.value)
      .subscribe(response => {
        this.loadEnd();
          if (response != PROCESS_RESULT_RESULT_TYPE.FAILURE && response.length > 0) {
            this.customerCodeCtrl.setValue(response[0].code);
            this.customerNameCtrl.setValue(response[0].name);
            this.customerId = response[0].id;

            //////////////////////////////////////////////////////////////////////////////////
            HtmlUtil.nextFocusByName(this.elementRef, 'customerCodeToCtrl', eventType);
            //////////////////////////////////////////////////////////////////////////////////
          }
          else {
            //this.customerCodeFromCtrl.setValue("");
            this.customerNameCtrl.setValue("");
            this.customerId = 0;

            //////////////////////////////////////////////////////////////////////////////////
            HtmlUtil.nextFocusByName(this.elementRef, 'customerCodeToCtrl', eventType);
            //////////////////////////////////////////////////////////////////////////////////
          }
        });
    }
    else {
      this.customerCodeCtrl.setValue("");
      this.customerNameCtrl.setValue("");
      this.customerId=0;

      //////////////////////////////////////////////////////////////////////////////////
      HtmlUtil.nextFocusByName(this.elementRef, 'customerCodeToCtrl', eventType);
      //////////////////////////////////////////////////////////////////////////////////
    }

  }


  /////////////////////////////////////////////////////////////////////////////////
  public setBilledAtFrom(eventType: string) {
    HtmlUtil.nextFocusByName(this.elementRef, 'billedAtToCtrl', eventType);
  }

  public setBilledAtTo(eventType: string) {
    HtmlUtil.nextFocusByName(this.elementRef, 'salesAtFromCtrl', eventType);
  }

  /////////////////////////////////////////////////////////////////////////////////
  public setDueAtFrom(eventType: string) {
    HtmlUtil.nextFocusByName(this.elementRef, 'dueAtToCtrl', eventType);
  }

  public setDueAtTo(eventType: string) {
    HtmlUtil.nextFocusByNames(this.elementRef, ['invoiceCodeFromCtrl', 'invoiceCodeCtrl'], eventType);
  }

  /////////////////////////////////////////////////////////////////////////////////
  public setPaymentCodeKey(eventType: string) {
    HtmlUtil.nextFocusByName(this.elementRef, 'dueAtToCtrl', eventType);
  }

  /////////////////////////////////////////////////////////////////////////////////
  public setCurrencyCode(eventType: string) {

    if (!StringUtil.IsNullOrEmpty(this.currencyCodeCtrl.value)) {

      this.currencyService.GetItems(this.currencyCodeCtrl.value)
        .subscribe(response => {
          if (response != PROCESS_RESULT_RESULT_TYPE.FAILURE && response.length > 0) {
            this.currencyCodeCtrl.setValue(response[0].code);
            this.currencyId = response[0].id;
            HtmlUtil.nextFocusByName(this.elementRef, 'amountRangeCtrl', eventType);
          }
          else {
            this.currencyCodeCtrl.setValue("");
            this.currencyId = 0;
            HtmlUtil.nextFocusByName(this.elementRef, 'amountRangeCtrl', eventType);
          }
        });
    }
    else {
      this.currencyCodeCtrl.setValue("");
      this.currencyId = 0;
      HtmlUtil.nextFocusByName(this.elementRef, 'amountRangeCtrl', eventType);
    }

  }

  /////////////////////////////////////////////////////////////////////////////////
  public setInvoiceCodeFrom(eventType: string) {
    HtmlUtil.nextFocusByName(this.elementRef, 'invoiceCodeToCtrl', eventType);
  }

  public setInvoiceCodeTo(eventType: string) {
    HtmlUtil.nextFocusByName(this.elementRef, 'invoiceCodeCtrl', eventType);
  }

  public setInvoiceCode(eventType: string) {
    HtmlUtil.nextFocusByNames(this.elementRef, ['updateAtFromCtrl','customerNameCtrl'], eventType);
  }

  /////////////////////////////////////////////////////////////////////////////////

  public setBillingCategoryCode(eventType: string) {

    if(eventType!=EVENT_TYPE.BLUR){
      this.billingCategoryCodeTrigger.closePanel();
    }

    if (!StringUtil.IsNullOrEmpty(this.billingCategoryCodeCtrl.value)) {

      this.billingCategoryCodeCtrl.setValue(StringUtil.setPaddingFrontZero(this.billingCategoryCodeCtrl.value, 2));

      this.loadStart();
      this.categoryService.GetItems(CategoryType.Billing, this.billingCategoryCodeCtrl.value)
        .subscribe(response => {
          this.loadEnd();
          if (response != PROCESS_RESULT_RESULT_TYPE.FAILURE && response.length > 0) {

            this.billingCategoryCodeCtrl.setValue(response[0].code);
            this.billingCategoryNameCtrl.setValue(response[0].name);
            this.bsBillingCategoryId = response[0].id;
            //////////////////////////////////////////////////////////////////////
            HtmlUtil.nextFocusByName(this.elementRef, 'collectCategoryCodeCtrl', eventType);
            //////////////////////////////////////////////////////////////////////

          }
          else {
            this.billingCategoryCodeCtrl.setValue("");
            this.billingCategoryNameCtrl.setValue("");
            this.bsBillingCategoryId = 0;
            //////////////////////////////////////////////////////////////////////
            HtmlUtil.nextFocusByName(this.elementRef, 'collectCategoryCodeCtrl', eventType);
            //////////////////////////////////////////////////////////////////////
          }
        });

    }
    else {
      this.billingCategoryCodeCtrl.setValue("");
      this.billingCategoryNameCtrl.setValue("");
      this.bsBillingCategoryId = 0;

      //////////////////////////////////////////////////////////////////////
      HtmlUtil.nextFocusByName(this.elementRef, 'collectCategoryCodeCtrl', eventType);
      //////////////////////////////////////////////////////////////////////
    }

  }

  /////////////////////////////////////////////////////////////////////////////////

  public setCbxUseReceiptSection(eventType: string) {

    let localstorageItem = new LocalStorageItem();
    localstorageItem.key = RangeSearchKey.PC0301_USE_RECEIPT_SECTION;
    localstorageItem.value = this.cbxUseReceiptSectionCtrl.value;
    this.localStorageManageService.set(localstorageItem);

    HtmlUtil.nextFocusByName(this.elementRef, 'customerKanaCtrl', eventType);

  }    

  public setDispFooterTotalSum() {
    this.dispSumCount = this.billingsResult.billings.length;
    this.dispSumBillingAmount = 0;
    this.dispSumRemainAmount = 0;

    this.billingsResult.billings.forEach(element => {
      this.dispSumBillingAmount += element.billingAmount;
      this.dispSumRemainAmount += element.remainAmount;
    });
  }

  public getBillingCategoryInfo(billing: Billing) {
    return StringUtil.IsNullOrEmpty(billing.billingCategoryCode) || StringUtil.IsNullOrEmpty(billing.billingCategoryName)
      ? ''
      : `${billing.billingCategoryCode}：${billing.billingCategoryName}`;
  }  

  public setDetailCbxUpdateFlag(eventType: string, index: number) {

    let billingId = this.billingsResult.billings[index].id;

    let cbxUpdateFlagCtrl = this.detailCbxUpdateFlagCtrls.get(billingId);
    let paymentAmountCtrl = this.detailPaymentAmountCtrls.get(billingId);

    if(cbxUpdateFlagCtrl.value){
      paymentAmountCtrl.enable();
    }
    else{
      paymentAmountCtrl.disable();

      // 入金予定額も違算も元に戻す。
    }


  }

  public nextFocusDetailPaymentAmount(eventType: string, index: number) {
    HtmlUtil.nextFocusByName(this.elementRef, 'detailTaxAmountCtrl' + index, eventType);
  }

  public setDetailPaymentAmount(index: number) {
    const pipe = new RacCurrencyPipe();

    let paymentAmountCtrl = this.detailPaymentAmountCtrls.get(this.billingsResult.billings[index].id);
    let offsetAmountctrl =  this.detailOffsetAmountCtrls.get(this.billingsResult.billings[index].id);

    let paymentAmount = NumberUtil.ParseInt(pipe.reverceTransform(paymentAmountCtrl.value, false));

    let offsetAmount = this.billingsResult.billings[index].remainAmount -paymentAmount;

    offsetAmountctrl.setValue(pipe.transform(""+offsetAmount,true));

  }


  public setCurrencyForDetailPaymentAmount(index: number){
    let paymentAmountCtrl = this.detailPaymentAmountCtrls.get(this.billingsResult.billings[index].id);
    this.onLeaveCurrencyControl(paymentAmountCtrl);
  }
 
  public initCurrencyForDetailPaymentAmount(index: number){
    let paymentAmountCtrl = this.detailPaymentAmountCtrls.get(this.billingsResult.billings[index].id);
    this.onFocusCurrencyControl(this.detailPaymentAmountCtrls[index]);
  }

  public onFocusCurrencyControl(control: FormControl) {
    if (this.ignoreTransform(control)) {
      return;
    }
    const pipe = new RacCurrencyPipe();
    control.setValue(pipe.reverceTransform(control.value, false));
  }

  public onLeaveCurrencyControl(control: FormControl) {
    if (this.ignoreTransform(control)) {
      return;
    }
    const pipe = new RacCurrencyPipe();
    let value = +control.value;
    control.setValue(pipe.transform(value, false));
  }


  public ignoreTransform(control: FormControl) {
    return control==undefined || control.value==undefined || StringUtil.IsNullOrEmpty(control.value) || control.value === '0';
  }

}
