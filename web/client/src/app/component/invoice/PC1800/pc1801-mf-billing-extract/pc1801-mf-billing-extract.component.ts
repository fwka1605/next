import { Component, OnInit, ComponentFactoryResolver, ViewContainerRef, ElementRef, AfterViewInit, ViewChild } from '@angular/core';
import { BaseComponent } from 'src/app/component/common/base/base-component';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { TABLE_INDEX } from 'src/app/common/const/table-name.const';
import { HtmlUtil } from 'src/app/common/util/html.util';
import { UserInfoService } from 'src/app/service/common/user-info.service';
import { NgbDateParserFormatter, NgbDate } from '@ng-bootstrap/ng-bootstrap';
import { NgbDateCustomParserFormatter } from 'src/app/common/util/dateformat-util';
import { ComponentId } from 'src/app/common/const/component-name.const';
import { Router, ParamMap, ActivatedRoute, NavigationEnd } from '@angular/router';
import { KEY_CODE, BUTTON_ACTION, EVENT_TYPE } from 'src/app/common/const/event.const';
import { MODAL_STATUS_TYPE, PROCESS_RESULT_RESULT_TYPE } from 'src/app/common/const/status.const';
import { WebApiSettingMasterService } from 'src/app/service/Master/web-api-setting-master.service';
import { MfHttpRequestServiceService } from 'src/app/service/common/mf-http-request-service.service';
import { MFWebApiOption } from 'src/app/model/mf-web-api-option.model';
import { MSG_INF, MSG_WNG, MSG_ITEM_NUM } from 'src/app/common/const/message.const';
import { WebApiType, CategoryType, TaxClassId, MFBILLING_PAYMENT_STATUS, BILL_INPUT_TYPE_DICTIONARY } from 'src/app/common/const/kbn.const';
import { FormatterUtil } from 'src/app/common/util/formatter.util';
import { CustomValidators } from 'ng5-validation';
import { ModalMasterComponent } from 'src/app/component/modal/modal-master/modal-master.component';
import { CategoryMasterService } from 'src/app/service/Master/category-master.service';
import { CategoriesResult } from 'src/app/model/categories-result.model';
import { Category } from 'src/app/model/category.model';
import { forkJoin } from 'rxjs';
import { MSG_ERR } from 'src/app/common/const/message.const';
import { StringUtil } from 'src/app/common/util/string-util';
import { StaffMasterService } from 'src/app/service/Master/staff-master.service';
import { Staff } from 'src/app/model/staff.model';
import { ProcessResultService } from 'src/app/service/common/process-result.service';
import { WebApiSetting } from 'src/app/model/web-api-setting.model';
import { WebApiMFExtractSetting } from 'src/app/model/web-api-mf-extract-setting.model';
import { StaffsResult } from 'src/app/model/staffs-result.model';
import { PageUtil } from 'src/app/common/util/page-util';
import { DateUtil } from 'src/app/common/util/date-util';
import { MfBilling } from 'src/app/model/mf-billing/mf-billing.model';
import { MFBilling } from 'src/app/model/mf-billing.model';
import { MFBillingService } from 'src/app/service/mfbilling.service';
import { MFBillingSource } from 'src/app/model/mf-billing-source.model';
import { CustomerMasterService } from 'src/app/service/Master/customer-master.service';
import { Customer } from 'src/app/model/customer.model';
import { Billing } from 'src/app/model/billing.model';
import { JuridicalPersonalitysResult } from 'src/app/model/juridical-personalitys-result.model';
import { JuridicalPersonalityMasterService } from 'src/app/service/Master/juridical-personality-master.service';
import { EbDataHelper } from 'src/app/common/util/eb-data-helper';
import { partner } from 'src/app/model/mf-billing/mf-partner.model';
import { MfDepartment } from 'src/app/model/mf-billing/mf-department.model';
import { CurrencyMasterService } from 'src/app/service/Master/currency-master.service';
import { CurrenciesResult } from 'src/app/model/currencies-result.model';
import { ModalRouterProgressComponent } from 'src/app/component/modal/modal-router-progress/modal-router-progress.component';
import { MatAutocompleteTrigger } from '@angular/material';

@Component({
  selector: 'app-pc1801-mf-billing-extract',
  templateUrl: './pc1801-mf-billing-extract.component.html',
  styleUrls: ['./pc1801-mf-billing-extract.component.css'],
  providers: [
    { provide: NgbDateParserFormatter, useClass: NgbDateCustomParserFormatter }
  ]
})
export class Pc1801MfBillingExtractComponent extends BaseComponent implements OnInit,AfterViewInit {

  public panelOpenState:boolean;

  public billingCategoryResult: CategoriesResult = new CategoriesResult();
  public collectCategoryResult: CategoriesResult = new CategoriesResult();
  public staffsResult: StaffsResult = new StaffsResult();
  public webApiSetting: WebApiSetting;
  public mfBillings: MfBilling[];
  public juridicalPersonalitysResult: JuridicalPersonalitysResult;
  public currenciesResult: CurrenciesResult;

  public collectCategories = new Array<Category>();

  public cbxCheckAllCtrl:FormControl; // 全選択・全解除

  public billedAtFromCtrl: FormControl;          // 請求日
  public billedAtToCtrl: FormControl;
  public billingCategoryCodeCtrl: FormControl;   // 請求区分
  public billingCategoryNameCtrl: FormControl;
  public staffCodeCtrl: FormControl;             // 営業担当者
  public staffNameCtrl: FormControl;             // 営業担当者名

  /* 日付設定 */
  public closingDayCtrl: FormControl;            // 締め日
  public cbxIssueBillEachTimeCtrl: FormControl;  // 都度請求
  public collectOffsetMonthCtrl: FormControl;    // 回収予定月
  public collectOffsetDayCtrl: FormControl;      // 回収予定日

  /* 回収設定 */
  public cmbCollectCategoryIdCtrl: FormControl;  // 回収方法

  public billingCategoryId: number;
  public collectCategoryId: number;
  public staffId: number;

  public cbxCreateBillingCtrls: Array<FormControl>  // チェック

  public extractionCount: number = 0;

  public collectOffsetMonthLabel1: string = "ヶ月後の";
  public collectOffsetMonthLabel2: string = "※末尾(28日以降)=99";
  public collectOffsetDayLabel: string = "日";

  public registedMFBillings: MFBilling[];

  public partners = new Map<string, partner>();
  public currencyId: number = 0;

  public UndefineCtrl: FormControl;

  @ViewChild('billingCategoryCodeInput', { read: MatAutocompleteTrigger }) billingCategoryCodeTrigger: MatAutocompleteTrigger;
  @ViewChild('staffCodeInput', { read: MatAutocompleteTrigger }) staffCodeTrigger: MatAutocompleteTrigger;

  constructor(
    public router: Router,
    public activatedRoute: ActivatedRoute,
    public elementRef: ElementRef,
    public componentFactoryResolver: ComponentFactoryResolver,
    public viewContainerRef: ViewContainerRef,
    public userInfoService: UserInfoService,
    public webApiSettingService: WebApiSettingMasterService,
    public mfHttpRequestServiceService: MfHttpRequestServiceService,
    public categoryService: CategoryMasterService,
    public staffService: StaffMasterService,
    public processResultService: ProcessResultService,
    public mfBillingService:MFBillingService,
    public customerService: CustomerMasterService,
    public juridicalPersonalityService: JuridicalPersonalityMasterService,
    public currencyService: CurrencyMasterService,
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

    let componentFactory = this.componentFactoryResolver.resolveComponentFactory(ModalRouterProgressComponent);
    let modalRouterProgressComponentRef = this.viewContainerRef.createComponent(componentFactory);

    this.activatedRoute.paramMap.subscribe((params: ParamMap) => {

      this.setControlInit();
      this.setValidator();
      this.setFormatter();
      this.clear();
      this.setAutoComplete();
      this.setBilledAtDate();
      let param = params.get("process");
      const fromWebApiSetting = !StringUtil.IsNullOrEmpty(param) && param == "back";
      if (fromWebApiSetting) {
        this.MyFormGroup = this.mfBillingService.Pc1801myFormGroup;
        this.readControlInit();
      }

      // 各データ取得
      let billingCategoriesResponse     = this.categoryService.GetItemsByCategoryType(CategoryType.Billing);
      let collectCategoriesResponse     = this.categoryService.GetItemsByCategoryType(CategoryType.Collection);
      let staffsResponse                = this.staffService.GetItems();
      let webApiSettingResponse         = this.webApiSettingService.GetByIdAsync(WebApiType.MoneyForward);
      let juridicalPersonalitysResponse = this.juridicalPersonalityService.GetItems();
      let currencyResponse              = this.currencyService.GetItems();

      let mfWebApiOption = new MFWebApiOption();
      mfWebApiOption.companyId    = this.userInfoService.Company.id;
      let mfHttpResquestResponse  = this.mfHttpRequestServiceService.ValidateToken(mfWebApiOption);

      this.billingCategoryResult.categories = new Array<Category>();
      this.collectCategoryResult.categories = new Array<Category>();
      this.staffsResult.staffs              = new Array<Staff>();
      this.juridicalPersonalitysResult      = new JuridicalPersonalitysResult();
      this.currenciesResult                 = new CurrenciesResult();

      forkJoin(
        billingCategoriesResponse,
        collectCategoriesResponse,
        staffsResponse,
        webApiSettingResponse,
        juridicalPersonalitysResponse,
        currencyResponse,
        mfHttpResquestResponse,
      ).subscribe(responseList => {
        if (
              responseList!=undefined
          &&  responseList.length == 7
          &&  responseList[0] != PROCESS_RESULT_RESULT_TYPE.FAILURE
          &&  responseList[1] != PROCESS_RESULT_RESULT_TYPE.FAILURE
          &&  responseList[2] != PROCESS_RESULT_RESULT_TYPE.FAILURE
          &&  responseList[3] != PROCESS_RESULT_RESULT_TYPE.FAILURE
          &&  responseList[4] != PROCESS_RESULT_RESULT_TYPE.FAILURE
          &&  responseList[5] != PROCESS_RESULT_RESULT_TYPE.FAILURE
          &&  responseList[6] != PROCESS_RESULT_RESULT_TYPE.FAILURE
        ) {

          this.billingCategoryResult.categories = responseList[0];
          this.collectCategoryResult.categories = responseList[1];
          this.staffsResult.staffs              = responseList[2];
          this.webApiSetting                    = responseList[3];
          this.juridicalPersonalitysResult.juridicalPersonalities = responseList[4];
          this.currenciesResult.currencies      = responseList[5];
          let bApiToken: boolean                = responseList[6];

          this.collectCategories = this.collectCategoryResult.categories.filter(x => x.useLimitDate == 0 && x.code != "00");

          if (this.webApiSetting == null) {
            this.setWarningWebApiSetting();
          }
          else if (!bApiToken) {
            this.processResultService.processAtWarning(
              this.processCustomResult, MSG_WNG.EXPIRED_TOKEN, this.partsResultMessageComponent);
          }

          if (fromWebApiSetting) {
            this.setMasterId();
          }

          this.setExtractSetting();
        }
        modalRouterProgressComponentRef.destroy();
      });

    });

  }

  ngAfterViewInit(){
    HtmlUtil.nextFocusByName(this.elementRef, 'billedAtFromCtrl', EVENT_TYPE.NONE);
  }

  public setWarningWebApiSetting() {
    this.processResultService.processAtWarning(
      this.processCustomResult, MSG_WNG.NOT_SETTING_MASTER.replace(MSG_ITEM_NUM.FIRST,'MFクラウド請求書 WebAPI 連携設定'),
      this.partsResultMessageComponent);
  }

  public readControlInit() {

    this.billedAtFromCtrl         = <FormControl>this.MyFormGroup.controls['billedAtFromCtrl'];  // 請求日
    this.billedAtToCtrl           = <FormControl>this.MyFormGroup.controls['billedAtToCtrl'];
    this.billingCategoryCodeCtrl  = <FormControl>this.MyFormGroup.controls['billingCategoryCodeCtrl'];  // 請求区分
    this.billingCategoryNameCtrl  = <FormControl>this.MyFormGroup.controls['billingCategoryNameCtrl'];
    this.staffCodeCtrl            = <FormControl>this.MyFormGroup.controls['staffCodeCtrl'];
    this.staffNameCtrl            = <FormControl>this.MyFormGroup.controls['staffNameCtrl'];
    this.closingDayCtrl           = <FormControl>this.MyFormGroup.controls['closingDayCtrl'];
    this.cbxIssueBillEachTimeCtrl = <FormControl>this.MyFormGroup.controls['cbxIssueBillEachTimeCtrl'];
    this.collectOffsetMonthCtrl   = <FormControl>this.MyFormGroup.controls['collectOffsetMonthCtrl'];
    this.collectOffsetDayCtrl     = <FormControl>this.MyFormGroup.controls['collectOffsetDayCtrl'];
    this.cmbCollectCategoryIdCtrl = <FormControl>this.MyFormGroup.controls['cmbCollectCategoryIdCtrl'];

    this.UndefineCtrl             = <FormControl>this.MyFormGroup.controls['UndefineCtrl'];

  }

  public setMasterId() {
    this.billingCategoryId = 0;
    if (!StringUtil.IsNullOrEmpty(this.billingCategoryCodeCtrl.value)) {
      const category = this.billingCategoryResult.categories.filter(x => x.code == this.billingCategoryCodeCtrl.value)[0];
      if (category != undefined) {
        this.billingCategoryId = category.id;
      }
    }
    this.staffId = 0;
    if (!StringUtil.IsNullOrEmpty(this.staffCodeCtrl.value)) {
      const staff = this.staffsResult.staffs.filter(x => x.code == this.staffCodeCtrl.value)[0];
      if (staff != undefined) {
        this.staffId = staff.id;
      }
    }
    this.collectCategoryId = 0;
    if (!StringUtil.IsNullOrEmpty(this.cmbCollectCategoryIdCtrl.value)) {
      const category = this.collectCategoryResult.categories.filter(x => x.id == this.cmbCollectCategoryIdCtrl.value)[0];
      if (category != undefined) {
        this.collectCategoryId = category.id;
      }
    }

  }

  public setControlInit() {

    this.cbxCheckAllCtrl = new FormControl(null);

    this.billedAtFromCtrl         = new FormControl(null, [Validators.maxLength(10)]);  // 請求日
    this.billedAtToCtrl           = new FormControl(null, [Validators.maxLength(10)]);
    this.billingCategoryCodeCtrl  = new FormControl("",[Validators.required, Validators.maxLength(2),]);  // 請求区分
    this.billingCategoryNameCtrl  = new FormControl("");

    this.staffCodeCtrl = new FormControl("", [Validators.required, Validators.maxLength(this.userInfoService.ApplicationControl.staffCodeLength)]);
    this.staffNameCtrl = new FormControl();

    // 日程設定
    this.closingDayCtrl           = new FormControl("", [Validators.required, CustomValidators.number]);
    this.cbxIssueBillEachTimeCtrl = new FormControl();
    this.collectOffsetMonthCtrl   = new FormControl("", [Validators.required, CustomValidators.number]);
    this.collectOffsetDayCtrl     = new FormControl("", [Validators.required, CustomValidators.number]);

    // 回収設定
    this.cmbCollectCategoryIdCtrl = new FormControl("", [Validators.required]);

    this.UndefineCtrl = new FormControl("");
  }

  public setValidator() {
    this.MyFormGroup = new FormGroup({
      cbxCheckAllCtrl:this.cbxCheckAllCtrl, // 全選択・全解除

      billedAtFromCtrl: this.billedAtFromCtrl,  // 請求日
      billedAtToCtrl: this.billedAtToCtrl,
      billingCategoryCodeCtrl: this.billingCategoryCodeCtrl,  // 請求区分
      billingCategoryNameCtrl: this.billingCategoryNameCtrl,

      staffCodeCtrl: this.staffCodeCtrl,
      staffNameCtrl: this.staffNameCtrl,

      // 日程設定
      closingDayCtrl: this.closingDayCtrl,
      cbxIssueBillEachTimeCtrl: this.cbxIssueBillEachTimeCtrl,
      collectOffsetMonthCtrl: this.collectOffsetMonthCtrl,
      collectOffsetDayCtrl: this.collectOffsetDayCtrl,

      // 回収設定
      cmbCollectCategoryIdCtrl: this.cmbCollectCategoryIdCtrl,


      UndefineCtrl: this.UndefineCtrl,
    });
  }

  public setFormatter() {
    FormatterUtil.setNumberFormatter(this.billingCategoryCodeCtrl);// 請求区分
    FormatterUtil.setCodeFormatter(this.staffCodeCtrl);
    FormatterUtil.setNumberFormatter(this.closingDayCtrl);
    FormatterUtil.setNumberFormatter(this.collectOffsetMonthCtrl);
    FormatterUtil.setNumberFormatter(this.collectOffsetDayCtrl);

  }

  public setAutoComplete() {
    
    // 営業担当者
    this.initAutocompleteStaffs(this.staffCodeCtrl,this.staffService,0);

    // 請求区分
    this.initAutocompleteCategories(CategoryType.Billing,this.billingCategoryCodeCtrl,this.categoryService,0);    
  }

  /**
   * 各テーブルのデータを取得する
   * @param table テーブル種別
   * @param keyCode イベント種別
   */
  public openMasterModal(table: TABLE_INDEX) {


    let componentFactory = this.componentFactoryResolver.resolveComponentFactory(ModalMasterComponent);
    let componentRef = this.viewContainerRef.createComponent(componentFactory);
    componentRef.instance.TableIndex = table;

    componentRef.instance.Closing.subscribe(() => {
      if (componentRef.instance.ModalStatus == MODAL_STATUS_TYPE.SELECT) {
        switch (table) {  
          case TABLE_INDEX.MASTER_BILLING_CATEGORY:
          {
            let category = componentRef.instance.SelectedObject as Category;
            this.billingCategoryCodeCtrl.setValue(category.code);
            this.billingCategoryNameCtrl.setValue(category.name);
            this.billingCategoryId = category.id;
            break;
          }
          case TABLE_INDEX.MASTER_STAFF:
          {
            let staff = componentRef.instance.SelectedObject as Staff;
            this.staffCodeCtrl.setValue(staff.code);
            this.staffNameCtrl.setValue(staff.name);
            this.staffId = staff.id;
            break;
          }
        }
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

      case BUTTON_ACTION.EXTRACT:
        this.extract();
        break;

      case BUTTON_ACTION.REGISTRY:
        this.registry();
        break;

      default:
        console.log('buttonAction Error.');
        break;
    }
  }

  public setBilledAtDate(){

    // 月初
    let date = new Date();
    this.billedAtFromCtrl.setValue(new NgbDate(date.getFullYear(), date.getMonth() + 1, 1 ));

    // 月末
    date.setDate(1);
    date.setMonth(date.getMonth() + 1);
    date.setDate(0);
    this.billedAtToCtrl.setValue(new NgbDate(date.getFullYear(), date.getMonth() + 1, date.getDate()));

  }

  public setExtractSetting() {

    if (this.webApiSetting == null ||
      StringUtil.IsNullOrEmpty(this.webApiSetting.extractSetting)) return;

    let extractSetting = JSON.parse(this.webApiSetting.extractSetting) as WebApiMFExtractSetting;

    let billingCategory = this.billingCategoryResult.categories.filter(x => x.id == extractSetting.billingCategoryId)[0];
    if (billingCategory != undefined) {
      this.billingCategoryCodeCtrl.setValue(billingCategory.code);
      this.billingCategoryNameCtrl.setValue(billingCategory.name);
      this.billingCategoryId = billingCategory.id;
    }
    else {
      this.billingCategoryCodeCtrl.setValue("");
      this.billingCategoryNameCtrl.setValue("");
      this.billingCategoryId = 0;
    }

    let staff = this.staffsResult.staffs.filter(x => x.id == extractSetting.staffId)[0];
    if (staff != undefined) {
      this.staffCodeCtrl.setValue(staff.code);
      this.staffNameCtrl.setValue(staff.name);
      this.staffId = staff.id;
    }
    else {
      this.staffCodeCtrl.setValue("");
      this.staffNameCtrl.setValue("");
    }

    let closingDay = extractSetting.closingDay;
    const perInvoice = (closingDay != undefined && closingDay == 0);
    this.changeExtractSetting(perInvoice);
    this.cbxIssueBillEachTimeCtrl.setValue(perInvoice);

    if (closingDay != undefined) {
      this.closingDayCtrl.setValue(StringUtil.setPaddingFrontZero(closingDay.toString(), 2, true));
    }
    else {
      this.closingDayCtrl.setValue("");
    }

    let collectOffsetMonth = extractSetting.collectOffsetMonth;
    if (collectOffsetMonth != undefined) {
      let value = collectOffsetMonth == 0 ? "" : collectOffsetMonth.toString();
      this.collectOffsetMonthCtrl.setValue(value);
    }
    else {
      this.collectOffsetMonthCtrl.setValue("");
    }

    let collectOffsetDay = extractSetting.collectOffsetDay;
    if (collectOffsetDay != undefined) {
      this.collectOffsetDayCtrl.setValue(collectOffsetDay);
    }
    else {
      this.collectOffsetDayCtrl.setValue("");
    }

    let collectCategory = this.collectCategoryResult.categories.filter(x => x.id == extractSetting.collectCategoryId)[0];
    if (collectCategory != undefined) {
      this.cmbCollectCategoryIdCtrl.setValue(collectCategory.id);
      this.collectCategoryId = collectCategory.id;
    }
    else {
      this.collectCategoryId = 0;
      this.cmbCollectCategoryIdCtrl.setValue(this.collectCategoryId);
    }

  }

  public getExtractSetting(): WebApiMFExtractSetting {

    return {
      billingCategoryId:  this.billingCategoryId,
      collectCategoryId:  this.collectCategoryId,
      staffId:            this.staffId,
      closingDay:         Number(this.closingDayCtrl.value),
      collectOffsetMonth: Number(this.collectOffsetMonthCtrl.value),
      collectOffsetDay:   Number(this.collectOffsetDayCtrl.value),
    } as WebApiMFExtractSetting;
  }

  public setExtractSettingToWebApiSetting() {
    if (this.webApiSetting == undefined) {
      return;
    }
    this.webApiSetting.extractSetting = JSON.stringify(this.getExtractSetting());
  }

  public saveWebApiSetting() {
    this.setExtractSettingToWebApiSetting();
    this.webApiSettingService.SaveAsync(this.webApiSetting)
      .subscribe();
  }

  public extract() {

    let componentFactory = this.componentFactoryResolver.resolveComponentFactory(ModalRouterProgressComponent);
    let modalRouterProgressComponentRef = this.viewContainerRef.createComponent(componentFactory);

    this.saveWebApiSetting();

    
    let source = new MFBillingSource();
    source.companyId       = this.userInfoService.Company.id;
    let mfBillingResponse           = this.mfBillingService.Get(source);

    let option = new MFWebApiOption();
    option.companyId        = this.userInfoService.Company.id;
    option.loginUserId      = this.userInfoService.LoginUser.id;
    option.billingDateFrom  = DateUtil.ConvertFromDatepicker(this.billedAtFromCtrl);
    option.billingDateTo    = DateUtil.ConvertFromDatepicker(this.billedAtToCtrl);
    let getMfBillingResponse        = this.mfHttpRequestServiceService.GetBillings(option);

    let customerResponse            = this.customerService.GetItems();

    forkJoin(
      mfBillingResponse,
      customerResponse,
      getMfBillingResponse,
      ).subscribe(responseList =>{

        if (responseList.length != 3 || responseList.indexOf(undefined) >= 0) {
          this.processCustomResult = this.processResultService.processAtFailure(
            this.processCustomResult, MSG_ERR.DATA_SEARCH, this.partsResultMessageComponent);
          modalRouterProgressComponentRef.destroy();
          return;
        }

        this.registedMFBillings       = responseList[0];
        let customers: Customer[]     = responseList[1];
        let tmpBillings: MfBilling[]  = responseList[2];

        if(tmpBillings.length == 0){
          this.processCustomResult = this.processResultService.processAtWarning(
            this.processCustomResult, MSG_WNG.NOT_EXIST_SEARCH_DATA, this.partsResultMessageComponent);
          modalRouterProgressComponentRef.destroy();
          return;
        }

        let amountOver: boolean = false;
        this.mfBillings = new Array<MfBilling>();

        for(let mfBilling of tmpBillings){
          if(mfBilling.total_price > 99999999999){
            amountOver = true;
            continue;
          }
          if(mfBilling.total_price != 0
            && !this.registedMFBillings.some(x => x.id == mfBilling.id)
            && mfBilling.status.payment != MFBILLING_PAYMENT_STATUS[2].val){
              mfBilling.customerCode = mfBilling.partner_id.substring(0, this.userInfoService.ApplicationControl.customerCodeLength).toUpperCase();
              mfBilling.customer = customers.find(x => x.code == mfBilling.customerCode);
              this.mfBillings.push(mfBilling);
          }
        }

        this.extractionCount = this.mfBillings.length;
        if(this.extractionCount == 0){
          this.processCustomResult = this.processResultService.processAtWarning(
            this.processCustomResult, MSG_WNG.NOT_EXIST_SEARCH_DATA, this.partsResultMessageComponent);
            modalRouterProgressComponentRef.destroy();
            return;
        }
        else if(amountOver){
          this.processCustomResult = this.processResultService.processAtWarning(
            this.processCustomResult, MSG_WNG.OVER_AMOUNT, this.partsResultMessageComponent);
        }
        else{
          this.processCustomResult = this.processResultService.processAtSuccess(
            this.processCustomResult, MSG_INF.DATA_EXTRACTED, this.partsResultMessageComponent);
        }

        this.cbxCreateBillingCtrls = new Array(this.mfBillings.length);

        for(let i=0; i<this.mfBillings.length; i++){
          this.cbxCreateBillingCtrls[i] = new FormControl(null);
        }

        for(let i=0; i<this.cbxCreateBillingCtrls.length; i++){
          this.MyFormGroup.removeControl("cbxCreateBillingCtrl"+i)
          this.MyFormGroup.addControl("cbxCreateBillingCtrl"+i, this.cbxCreateBillingCtrls[i]);
        }

        this.selectAll();
        this.getPartners();

        modalRouterProgressComponentRef.destroy();
        
      });
  }

  /**
   * VOneに登録済みのMF請求データ一覧を取得
   */
  public getRegisteredMfBillings(){

    let mfBillingSource = new MFBillingSource();
    mfBillingSource.companyId = this.userInfoService.Company.id;

    this.mfBillingService.Get(mfBillingSource)
      .subscribe(result =>{
        if(result != undefined && result.length>0){
          this.registedMFBillings = result as MFBilling[];
        }
      });
  }

  public clear(clearMsg: boolean = true){

    this.MyFormGroup.reset();
    this.mfBillings = null;
    if (this.webApiSetting == null) {
      this.setWarningWebApiSetting();
    }
    else if (clearMsg) {
      this.processCustomResult = this.processResultService.clearProcessCustomMsg(this.processCustomResult);
    }

    this.extractionCount = 0;
    this.setBilledAtDate();
    this.setExtractSetting();

    this.panelOpenState=true;

    HtmlUtil.nextFocusByName(this.elementRef, 'billedAtFromCtrl', EVENT_TYPE.NONE);
    
  }

  public registry(){

    let componentFactory = this.componentFactoryResolver.resolveComponentFactory(ModalRouterProgressComponent);
    let modalRouterProgressComponentRef = this.viewContainerRef.createComponent(componentFactory);

    let targetBillings = new Array<Billing>();
    let targetCustomers = new Array<Customer>();
    let targetCount: number = 0;
    let departmentId: number = 0;

    if(this.currenciesResult == undefined) return;
    this.currencyId = this.currenciesResult.currencies.find(x => x.code == "JPY").id;

    let extractSetting = JSON.stringify(this.getExtractSetting());

    if(StringUtil.IsNullOrEmpty(this.webApiSetting.extractSetting) || this.webApiSetting.extractSetting !== extractSetting){
      this.webApiSetting.extractSetting = extractSetting;

      this.webApiSettingService.SaveAsync(this.webApiSetting)
        .subscribe(result => {
          if (result == PROCESS_RESULT_RESULT_TYPE.FAILURE){
            this.processResultService.processAtFailure(
              this.processCustomResult, MSG_ERR.POST_PROCESS_FAILURE, this.partsResultMessageComponent);
          }
        });
    }

    for(let index = 0; index < this.mfBillings.length; index++){
      if(this.cbxCreateBillingCtrls[index].value){

        targetCount ++;

        let mfBilling : MfBilling = this.mfBillings[index];
        let partner = this.partners.get(mfBilling.partner_id) as partner;
        if(mfBilling.customer == undefined){
          //  新規得意先マスター作成
          let customer = this.createNewCustomer(mfBilling, partner);
          targetCustomers.push(customer);
        }
        else{
          //  departmentId設定
          departmentId = this.staffsResult.staffs.find(x => x.id == mfBilling.customer.staffId).departmentId;
        }

        let department = partner == undefined ? null : partner.departments.find(x => x.id == mfBilling.department_id) as MfDepartment;

        //  VOne請求データ作成
        let billing = this.getBilling(mfBilling, department, departmentId);

        targetBillings.push(billing);
      }
    }

    let mfBillingSource = new MFBillingSource();
    mfBillingSource.companyId = this.userInfoService.Company.id;
    mfBillingSource.billings  = targetBillings;
    mfBillingSource.customers = targetCustomers;

    this.mfBillingService.Save(mfBillingSource)
      .subscribe(result => {
        this.processCustomResult = this.processResultService.processAtSave(
          this.processCustomResult, result, true, this.partsResultMessageComponent);

        if(result.length == targetCount){
          this.clear(false);
        }

        modalRouterProgressComponentRef.destroy();
      });
  }

  public getPartners(){

    this.partners.clear();

    let groups = this.mfBillings.reduce(function(obj, item){
      obj[item.partner_id] = obj[item.partner_id] || [];
      obj[item.partner_id].push(item.partner_name);
      return obj;
    }, {});

    let partnerIds = Object.keys(groups).map(function(key){
      return key;
    });

    let mfWebApiOption = new MFWebApiOption();
    mfWebApiOption.companyId    = this.userInfoService.Company.id;
    mfWebApiOption.loginUserId  = this.userInfoService.LoginUser.id;

    for(let index=0; index<partnerIds.length; index++){

      mfWebApiOption.partnerId = partnerIds[index];
      this.mfHttpRequestServiceService.GetPartner(mfWebApiOption)
        .subscribe(result => {
          let partner = result as partner;
          this.partners.set(partnerIds[index], partner);
        });
    }

  }

  public createNewCustomer(mfBilling: MfBilling, partner: partner): Customer{

    let department = partner == null ? null : partner.departments[0] as MfDepartment;

    let customer = new Customer();
    customer.companyId  = this.userInfoService.Company.id;
    customer.code       = mfBilling.customerCode;
    customer.name       = mfBilling.partner_name.substr(0, 140);

    let kana = !StringUtil.IsNullOrEmpty(partner.name_kana) ? partner.name_kana : mfBilling.customerCode;
    customer.kana       = this.getCustomerNameKana(kana);

    customer.postalCode = department == null ? "" : StringUtil.IsNullOrEmpty(department.zip) ? "" : department.zip;
    customer.address1   = department == null ? "" : StringUtil.IsNullOrEmpty(department.address1) ? "" : department.address1;
    customer.address2   = department == null ? "" : StringUtil.IsNullOrEmpty(department.address2) ? "" : department.address2;
    customer.tel        = department == null ? "" : StringUtil.IsNullOrEmpty(department.tel) ? "" : department.tel;
    customer.staffName  = department == null ? "" : StringUtil.IsNullOrEmpty(department.person_name) ? "" : department.person_name;
    customer.note       = StringUtil.IsNullOrEmpty(partner.memo) ? "" : partner.memo;

    let extractSetting = this.getExtractSetting();
    customer.closingDay         = extractSetting.closingDay;
    customer.collectCategoryId  = extractSetting.collectCategoryId;
    customer.collectOffsetMonth = extractSetting.collectOffsetMonth;
    customer.collectOffsetDay   = extractSetting.collectOffsetDay;
    customer.staffId            = extractSetting.staffId;
    customer.receiveAccountId1  = 1;
    customer.receiveAccountId2  = 1;
    customer.receiveAccountId3  = 1;
    customer.useKanaLearning    = 1;
    customer.collationKey       = department == null ? "" :  StringUtil.IsNullOrEmpty(department.tel) ? "" : department.tel.substr(0, 48);
    customer.createBy           = this.userInfoService.LoginUser.id;
    customer.updateBy           = this.userInfoService.LoginUser.id;

    return customer;
  }

  public getCustomerNameKana(kana: string): string{

    let value: string =  kana;
    value = EbDataHelper.convertToValidEbkana(value);
    value = EbDataHelper.removePersonalities(value, this.juridicalPersonalitysResult.juridicalPersonalities);

    return value.substr(0, 140);
  }

  /**
   * VOne側請求データ作成
   */
  public getBilling(mfBilling: MfBilling, department: MfDepartment, departmentId: number): Billing {
    const inputType = 1;
    let extractSetting = this.getExtractSetting();

    let billing = new Billing();
    billing.companyId         = this.userInfoService.Company.id;
    billing.currencyId        = this.currencyId;
    billing.billingCategoryId = extractSetting.billingCategoryId;
    billing.inputType         = BILL_INPUT_TYPE_DICTIONARY[inputType].id;
    billing.billedAt          = mfBilling.billing_date;
    billing.closingAt         = mfBilling.billing_date;
    billing.salesAt           = mfBilling.sales_date;
    billing.dueAt             = mfBilling.due_date;
    billing.billingAmount     = mfBilling.total_price;
    billing.remainAmount      = mfBilling.total_price;
    billing.approved          = 1;
    billing.invoiceCode       = StringUtil.IsNullOrEmpty(mfBilling.billing_number) ? "" : mfBilling.billing_number.substr(0, 100);
    billing.taxClassId        = TaxClassId.NotCovered;

    let note1 = "No." + mfBilling.billing_number + mfBilling.partner_name;
    billing.note1             = note1.substr(0, 100);
    billing.note2             = StringUtil.IsNullOrEmpty(mfBilling.memo) ? "" : mfBilling.memo.substr(0, 100);
    billing.note3             = department == undefined ? "" : StringUtil.IsNullOrEmpty(department.name) ? "" : department.name.substr(0, 100);

    let note4 = department == undefined ? "" : (department.person_name || '') + (department.cc_emails || '');
    billing.note4             = note4.substr(0, 100);
    billing.note5             = mfBilling.tags.join(" ").substr(0, 100);
    billing.note6             = mfBilling.partner_name.substr(0, 100);
    billing.note7             = "";
    billing.note8             = "";

    billing.customerKana      = mfBilling.id;

    if(mfBilling.customer == undefined){
      billing.customerCode    = mfBilling.customerCode;
    }
    else{
      billing.customerId        = mfBilling.customer.id;
      billing.departmentId      = departmentId;
      billing.staffId           = mfBilling.customer.staffId;
      billing.collectCategoryId = mfBilling.customer.collectCategoryId;
    }

    return billing;
  }

  public openApiSetting(){
    if (this.webApiSetting == undefined) {
      this.mfBillingService.ExtractSetting = this.getExtractSetting();
    }
    this.mfBillingService.Pc1801myFormGroup = this.MyFormGroup;
    //this.mfBillingService.CollectCategories = this.collectCategories;

    this.router.navigate(['main/PH1401', { from: ComponentId.PC1801 }]);
  }

  public selectAll(){
    for(let index = 0; index < this.cbxCreateBillingCtrls.length; index++){
      this.cbxCreateBillingCtrls[index].setValue("true");
    }
  }

  public deselectAll(){
    for(let index = 0; index < this.cbxCreateBillingCtrls.length; index++){
      this.cbxCreateBillingCtrls[index].setValue("");
    }
  }

  ///////////////////////////////////////////////////////////////////////
  public setBilledAtFrom(eventType:string){
    HtmlUtil.nextFocusByName(this.elementRef, 'billedAtToCtrl', eventType);
  }

  ///////////////////////////////////////////////////////////////////////
  public setBilledAtTo(eventType:string){
    HtmlUtil.nextFocusByName(this.elementRef, 'billedAtToCtrl', eventType);
  }

  ///////////////////////////////////////////////////////////////////////
  public setClosingDay(eventType:string){

    this.closingDayCtrl.setValue(
      this.GetFormattedClosingDay(
        this.closingDayCtrl.value, !this.cbxIssueBillEachTimeCtrl.value));

    if(this.closingDayCtrl.value=="00"){
      this.collectOffsetMonthCtrl.setValue("");
      this.collectOffsetMonthCtrl.disable();
      this.cbxIssueBillEachTimeCtrl.setValue(true);
    }

    HtmlUtil.nextFocusByName(this.elementRef, 'collectOffsetMonthCtrl', eventType);
  }

  ///////////////////////////////////////////////////////////////////////
  public setCollectOffsetMonth(eventType:string){
    HtmlUtil.nextFocusByName(this.elementRef, 'collectOffsetDayCtrl', eventType);
  }

  ///////////////////////////////////////////////////////////////////////
  public setCollectOffsetDay(eventType:string){

    this.collectOffsetDayCtrl.setValue(
      this.GetFormattedClosingDay(
          this.collectOffsetDayCtrl.value, !this.cbxIssueBillEachTimeCtrl.value));

    HtmlUtil.nextFocusByName(this.elementRef, 'cmbCollectCategoryIdCtrl', eventType);
  }

  public GetFormattedClosingDay(dayString:string , adjust99:boolean = true):string 
  {
      if (StringUtil.IsNullOrEmpty(dayString))
      {
          return null;
      }

      var day = parseInt(dayString);

      if (adjust99 && 28 <= day)
      {
          day = 99;
      }

    return StringUtil.setPaddingFrontZero("" + day, 2, true);
  }

  ///////////////////////////////////////////////////////////////////////
  public setBillingCategoryCode(eventType:string){
    let code = this.billingCategoryCodeCtrl.value;
    if (!StringUtil.IsNullOrEmpty(code)) {
      code = StringUtil.setPaddingFrontZero(this.billingCategoryCodeCtrl.value,2);

      this.loadStart();
      this.categoryService.GetItems(CategoryType.Billing, code)
        .subscribe(response => {
          this.loadEnd();
          if (response != PROCESS_RESULT_RESULT_TYPE.FAILURE && response.length > 0) {
            let category = response[0] as Category;
            this.billingCategoryCodeCtrl.setValue(category.code);
            this.billingCategoryNameCtrl.setValue(category.name);
            this.billingCategoryId = category.id;
          }
          else {
            this.billingCategoryCodeCtrl.setValue("");
            this.billingCategoryNameCtrl.setValue("");
            this.billingCategoryId = 0;
          }
        });
    }
    else {
      this.billingCategoryCodeCtrl.setValue("");
      this.billingCategoryNameCtrl.setValue("");
      this.billingCategoryId = 0;
    }
    HtmlUtil.nextFocusByName(this.elementRef, 'staffCodeCtrl', eventType);
  }

  ///////////////////////////////////////////////////////////////////////
  public setStaffCode(eventType:string){

    if (!StringUtil.IsNullOrEmpty(this.staffCodeCtrl.value)) {
      this.loadStart();
      this.staffService.GetItems(this.staffCodeCtrl.value)
        .subscribe(response => {
          this.loadEnd();
          if (response != PROCESS_RESULT_RESULT_TYPE.FAILURE && response.length > 0) {
            let staff = response[0] as Staff;
            this.staffCodeCtrl.setValue(staff.code);
            this.staffNameCtrl.setValue(staff.name);
            this.staffId = staff.id;
            HtmlUtil.nextFocusByName(this.elementRef, 'closingDayCtrl', eventType);
          }
          else {
            this.staffCodeCtrl.setValue("");
            this.staffNameCtrl.setValue("");
          }
        });
    }
    else {
      this.staffCodeCtrl.setValue("");
      this.staffNameCtrl.setValue("");
    }

  }

  ///////////////////////////////////////////////////////////////////////
  public setCollectCategoryId(eventType:string){

    if(!StringUtil.IsNullOrEmpty(this.cmbCollectCategoryIdCtrl.value)){
      let id = this.cmbCollectCategoryIdCtrl.value;
      let category = this.collectCategoryResult.categories.find(x => x.id == id);
      if(category != undefined){
        this.collectCategoryId = category.id;
      }
      else{
        this.collectCategoryId = 0;
      }
    }
    else{
      this.collectCategoryId = 0;
    }

  }

  ///////////////////////////////////////////////////////////////////////
  public setCbxIssueBillEachTime(eventType:string){
    let perInvoice: boolean = this.cbxIssueBillEachTimeCtrl.value;
    const nextControlName = perInvoice ? 'collectOffsetDayCtrl' : 'closingDayCtrl';
    this.changeExtractSetting(perInvoice);
    HtmlUtil.nextFocusByName(this.elementRef, nextControlName, eventType);
  }

  public changeExtractSetting(perInvoice: boolean) {
    if (perInvoice) {
      this.closingDayCtrl.setValue('00');
      this.closingDayCtrl.disable();
      this.collectOffsetMonthCtrl.setValue('');
      this.collectOffsetMonthCtrl.setValidators([]);
      this.collectOffsetMonthCtrl.disable();
    }
    else {
      this.closingDayCtrl.setValue('');
      this.closingDayCtrl.enable();
      this.collectOffsetMonthCtrl.setValidators([Validators.required, CustomValidators.number]);
      this.collectOffsetMonthCtrl.enable();
    }
    this.collectOffsetMonthCtrl.updateValueAndValidity();
    this.collectOffsetMonthLabel1 = perInvoice ? '請求日後' : 'ヶ月後の';
    this.collectOffsetMonthLabel2 = perInvoice ? '' : '※末尾(28日以降)=99';
    this.collectOffsetDayLabel    = perInvoice ? '日以内' : '日';
  }

  ///////////////////////////////////////////////////////////////////////
  public setCbxCreateBilling(index:number){
    for (let i = 0; i < this.cbxCreateBillingCtrls.length; i++) {
      if(this.cbxCreateBillingCtrls[i].value){
        break;
      }
    }
  }

  ///////////////////////////////////////////////////////////////////////
  public getCustomerName(mfBilling: MfBilling): string{

    return mfBilling.customer == undefined ? mfBilling.partner_name : mfBilling.customer.name;
  }

  /**
   * 抽出ボタン 利用不可の状況
   * WebApiSetting が 未登録 または
   * MFC請求書の請求データを抽出済
   */
  public disableExtract(): boolean {
    return this.webApiSetting == undefined ||
      this.mfBillings != undefined && this.mfBillings.length > 0;
  }

  /**
   * 登録ボタン 利用不可の状況
   * MFC請求書の請求データが未抽出 または 一件もグリッドの登録 チェックボックスがチェックされていない
   */
  public disableRegister(): boolean {
    return this.mfBillings == undefined ||
      this.mfBillings.length == 0 ||
      this.cbxCreateBillingCtrls.every(x => !x.value);
  }

  public checkAll() {
    if(this.cbxCreateBillingCtrls!=null&&this.cbxCreateBillingCtrls.length>0){
      this.cbxCreateBillingCtrls.map(x => x.setValue(this.cbxCheckAllCtrl.value));
    }
    this.processResultService.clearProcessCustomMsg(this.processCustomResult);
  }

}
