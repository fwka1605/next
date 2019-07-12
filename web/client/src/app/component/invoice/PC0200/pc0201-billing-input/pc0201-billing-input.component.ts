
import { Component, OnInit, ComponentFactoryResolver, ViewContainerRef, ElementRef, AfterViewInit, ViewChild, } from '@angular/core';
import { BaseComponent } from 'src/app/component/common/base/base-component';
import { ModalMasterComponent } from 'src/app/component/modal/modal-master/modal-master.component';
import { FormControl, Validators, FormGroup } from '@angular/forms';
import { Billing } from 'src/app/model/billing.model';
import { CategoriesResult } from 'src/app/model/categories-result.model';
import { CategoryMasterService } from 'src/app/service/Master/category-master.service';
import { StringUtil } from 'src/app/common/util/string-util';
import { DateUtil } from 'src/app/common/util/date-util';
import { TaxClassMasterService } from 'src/app/service/Master/tax-class-master.service';
import { AccountTitleMasterService } from 'src/app/service/Master/account-title-master.service';
import { TABLE_INDEX } from 'src/app/common/const/table-name.const';
import { ModalInvoiceMemoComponent } from 'src/app/component/modal/modal-invoice-memo/modal-invoice-memo.component';
import { BillingMemo } from 'src/app/model/billing-memo.model';
import { NumberUtil } from 'src/app/common/util/number-util';
import { Router, ActivatedRoute, ParamMap, NavigationEnd } from '@angular/router';
import { FormatterUtil } from 'src/app/common/util/formatter.util';
import { RacCurrencyPipe } from 'src/app/pipe/currency.pipe';
import { HtmlUtil } from 'src/app/common/util/html.util';
import { KEY_CODE, EVENT_TYPE, BUTTON_ACTION } from 'src/app/common/const/event.const';
import { COMPONENT_STATUS_TYPE, MODAL_STATUS_TYPE, PROCESS_RESULT_RESULT_TYPE, COMPONENT_DETAIL_STATUS_TYPE } from 'src/app/common/const/status.const';
import { GeneralSetting } from 'src/app/model/general-setting.model';
import { Customer } from 'src/app/model/customer.model';
import { CustomerMasterService } from 'src/app/service/Master/customer-master.service';
import { DepartmentMasterService } from 'src/app/service/Master/department-master.service';
import { StaffMasterService } from 'src/app/service/Master/staff-master.service';
import { CUSTOMER_HOLIDAY_FLAG_DICTIONARY, BILL_INPUT_TYPE_DICTIONARY, CategoryType, RoundingType, TaxClassId, CODE_TYPE, FunctionType } from 'src/app/common/const/kbn.const';
import { TaxClassResult } from 'src/app/model/tax-class-result.model';
import { ProcessResult } from 'src/app/model/process-result.model';
import { TaxClass } from 'src/app/model/tax-class.model';
import { Category } from 'src/app/model/category.model';
import { UserInfoService } from 'src/app/service/common/user-info.service';
import { NgbDateParserFormatter, NgbDateStruct, NgbDate } from '@ng-bootstrap/ng-bootstrap';
import { NgbDateCustomParserFormatter } from 'src/app/common/util/dateformat-util';
import { DestinationMasterService } from 'src/app/service/Master/destination-master.service';
import { DepartmentsResult } from 'src/app/model/departments-result.model';
import { StaffsResult } from 'src/app/model/staffs-result.model';
import { HolidayCalendarMasterService } from 'src/app/service/Master/holiday-calendar-master.service';
import { HolidayCalendarsResult } from 'src/app/model/holiday-calendars-result.model';
import { forkJoin } from 'rxjs';
import { HolidayCalendarSearch } from 'src/app/model/holiday-calendar-search.model';
import { ColumnNameSettingMasterService } from 'src/app/service/Master/column-name-setting-master.service';
import { ColumnNameSettingsResult } from 'src/app/model/column-name-settings-result.model';
import { ModalConfirmComponent } from 'src/app/component/modal/modal-confirm/modal-confirm.component';
import { BillingDeleteSource } from 'src/app/model/billing-delete-source.model';
import { BillingService } from 'src/app/service/billing.service';
import { CollationSettingMasterService } from 'src/app/service/Master/collation-setting-master.service';
import { CollationSettingResult } from 'src/app/model/collation-setting-result.model';
import { BillingSearch } from 'src/app/model/billing-search.model';
import { CurrencyMasterService } from 'src/app/service/Master/currency-master.service';
import { CurrenciesResult } from 'src/app/model/currencies-result.model';
import { Department } from 'src/app/model/department.model';
import { Staff } from 'src/app/model/staff.model';
import { BillingResult } from 'src/app/model/billing-result.model';
import { Destination } from 'src/app/model/destination.model';
import { AccountTitle } from 'src/app/model/account-title.model';
import { Currency } from 'src/app/model/currency.model';
import { DestinationsResult } from 'src/app/model/destinations-result.model';
import { CustomerResult } from 'src/app/model/customer-result.model';
import { AccountTitlesResult } from 'src/app/model/account-titles-result.model';
import { ProcessResultService } from 'src/app/service/common/process-result.service';
import { MSG_WNG, MSG_ERR, MSG_ITEM_NUM, MSG_INF } from 'src/app/common/const/message.const';
import { PageUtil } from 'src/app/common/util/page-util';
import { ComponentId } from 'src/app/common/const/component-name.const';
import { LocalStorageItem } from 'src/app/model/custom-model/local-storage-item';
import { RangeSearchKey } from 'src/app/common/const/local-storage-key-const';
import { LocalStorageManageService } from 'src/app/service/common/local-storage-manage.service';
import { ModalRouterProgressComponent } from 'src/app/component/modal/modal-router-progress/modal-router-progress.component';
import { MatAutocompleteTrigger } from '@angular/material';
import { BillingDisplay } from 'src/app/model/input/billing-display.model';

enum FOCUS {
  NO_TAX_AMOUNT,
  TAX_AMOUNT,
  AMOUNT
}

@Component({
  selector: 'app-pc0201-billing-input',
  templateUrl: './pc0201-billing-input.component.html',
  styleUrls: ['./pc0201-billing-input.component.css'],
  providers: [
    { provide: NgbDateParserFormatter, useClass: NgbDateCustomParserFormatter }
  ]
})
export class Pc0201BillingInputComponent extends BaseComponent implements OnInit,AfterViewInit {

  public PC0201_INPUT: typeof PC0201_INPUT = PC0201_INPUT;
  public FunctionType: typeof FunctionType = FunctionType;

  public currentFocus: FOCUS;

  public paramId: string = "";
  public paramInputId: string = "";
  public paramProcess: string = "";
  public paramFrom: ComponentId;
  public currentBilling: Billing;
  public currentBillingsByInputId: Billing[];

  public generalSettings: Array<GeneralSetting>;
  public billingCount: number = 1;
  public billingMaxCount:number=0;
  public billings: Billing[];
  public billingMemos: Array<BillingMemo>;

  public detailDispBillings = new Array<BillingDisplay>();

  public collectCategoriesResult: CategoriesResult;
  public billingCategoryResult: CategoriesResult;
  public departmentsResult: DepartmentsResult;
  public staffsResult: StaffsResult;
  public holidaysResult: HolidayCalendarsResult;
  public columnNameSettingResult: ColumnNameSettingsResult;
  public collationSettingResult: CollationSettingResult;
  public currencyResult: CurrenciesResult;

  public readonly holidayFlagDictionary = CUSTOMER_HOLIDAY_FLAG_DICTIONARY;

  public taxCalculationRoundingType: RoundingType;
  public priceCalculationRoundingType: RoundingType;
  public precision: number = 0;
  public isEditable: boolean = true;
  public isFirstRowOnly: boolean = false;

  public componentDetailStatus:COMPONENT_DETAIL_STATUS_TYPE;
  public detailEditLineNo:number;

  // ヘッダー情報
  public billingIdCtrl: FormControl;  // 請求入力ID
  public billedAtCtrl: FormControl;  // 請求日
  public closingAtCtrl: FormControl;  // 請求締日
  public invoiceCodeCtrl: FormControl;  // 請求書番号
  public currencyCodeCtrl: FormControl; // 通貨コード
  public saleAtCtrl: FormControl;  // 売上日
  public dueAtCtrl: FormControl;  // 予定日
  public collectCategoryCodeCtrl: FormControl;  // 回収区分
  public customerCodeCtrl: FormControl;  // 得意先コード
  public customerNameCtrl: FormControl;
  public departmentCodeCtrl: FormControl;  // 請求部門コード
  public departmentNameCtrl: FormControl;
  public staffCodeCtrl: FormControl;  // 担当者コード
  public staffNameCtrl: FormControl;
  public destinationCodeCtrl: FormControl;  // 送付先コード
  public destinationNameCtrl: FormControl;

  /////////////////////////////////////////////////////
  // 詳細
  /////////////////////////////////////////////////////
  //契約番号
  public notInputContractNumFlags: Array<boolean> = new Array<boolean>(this.billingCount);
  public detailSalesAtCtrls: Array<FormControl> = new Array(this.billingCount);  // 売上日
  public detailBillingCategoryIdCtrls: Array<FormControl> = new Array(this.billingCount);  // 請求区分IDs
  public detailBillingCategoryCodeCtrls: Array<FormControl> = new Array(this.billingCount);  // 請求区分
  public detailTaxClassIdCtrls: Array<FormControl> = new Array(this.billingCount);  // 税区分
  public detailDebitAccountTitleIdCtrls: Array<FormControl> = new Array(this.billingCount);  // 債権科目
  public detailDebitAccountTitleIds: Array<FormControl> = new Array(this.billingCount);  //  債権科目IDs
  public detailMemoCtrls: Array<FormControl> = new Array(this.billingCount);  // 備考
  public detailNoteFlagCtrls: Array<FormControl> = new Array(this.billingCount);  // メモフラグ
  public detailBillingIdCtrls: Array<FormControl> = new Array(this.billingCount);  // 請求ID
  public detailContractNumberCtrls: Array<FormControl> = new Array(this.billingCount);  // 契約番号
  public detailQuantityCtrls: Array<FormControl> = new Array(this.billingCount);  // 数量
  public detailUnitSymbolCtrls: Array<FormControl> = new Array(this.billingCount);  // 単位
  public detailUnitPriceCtrls: Array<FormControl> = new Array(this.billingCount);  // 単価
  public detailBillingNoTaxAmountCtrls: Array<FormControl> = new Array(this.billingCount);  // 請求額(抜)
  public detailTaxAmountCtrls: Array<FormControl> = new Array(this.billingCount);  // 消費税
  public detailBillingAmountCtrls: Array<FormControl> = new Array(this.billingCount);  // 請求額

  // 合計
  public sumBillingNoTaxAmountCtls: FormControl;  // 請求額(抜)
  public sumTaxAmountCtls: FormControl;  // 消費税
  public sumBillingAmountCtls: FormControl;  // 請求額

  public rdoCalAmountCtrl: FormControl; // 金額計算

  public undefineCtrl: FormControl; // 未定用

  public customer: Customer;  // 選択されたCustomer
  public staffId: number;   //  選択された担当者ID
  public departmentId: number;  //  選択された請求部門ID
  public destinationId;  //  送付先ID

  // 日付のフォーマット
  dateValue: NgbDateStruct;

  // オートコンプリート入力欄
  @ViewChild('customerCodeInput', { read: MatAutocompleteTrigger }) customerCodeTrigger: MatAutocompleteTrigger;
  @ViewChild('departmentCodeInput', { read: MatAutocompleteTrigger }) departmentCodeTrigger: MatAutocompleteTrigger;
  @ViewChild('staffCodeInput', { read: MatAutocompleteTrigger }) staffCodeTrigger: MatAutocompleteTrigger;


  @ViewChild('detailBillingCategoryCodeInput', { read: MatAutocompleteTrigger }) detailBillingCategoryCodeTrigger: MatAutocompleteTrigger;
  @ViewChild('detailTaxClassIdInput', { read: MatAutocompleteTrigger }) detailTaxClassTrigger: MatAutocompleteTrigger;
  @ViewChild('detailDebitAccountTitleIdInput', { read: MatAutocompleteTrigger }) detailBebitAccountTitleIdTrigger: MatAutocompleteTrigger;


  constructor(
    public router: Router,
    public activatedRoute: ActivatedRoute,
    public elementRef: ElementRef,
    public componentFactoryResolver: ComponentFactoryResolver,
    public viewContainerRef: ViewContainerRef,
    public categoryService: CategoryMasterService,
    public userInfoService: UserInfoService,
    public taxClassService: TaxClassMasterService,
    public accountTitleService: AccountTitleMasterService,
    public customerService: CustomerMasterService,
    public departmentService: DepartmentMasterService,
    public staffService: StaffMasterService,
    public destinationService: DestinationMasterService,
    public columnNameSettingService: ColumnNameSettingMasterService,
    public holidayService: HolidayCalendarMasterService,
    public collationSettingService: CollationSettingMasterService,
    public billingService: BillingService,
    public currencyService: CurrencyMasterService,
    public processResultService: ProcessResultService,
    public localStorageManageService:LocalStorageManageService,

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
    
    let bCount: number = 0;
    this.securityHideShow = true;
    this.activatedRoute.paramMap.subscribe((params: ParamMap) => {
      // ルートパラメータと同じ要領で Matrix URI パラメータが取得
      this.paramId = params.get("id");
      this.paramInputId = params.get("inputId");
      this.paramFrom = parseInt(params.get("form"));
      this.paramProcess = params.get("process");
      bCount = Number(params.get("count"));
    });

    this.generalSettings = this.userInfoService.GeneralSettings;
    this.billingMaxCount = NumberUtil.ParseInt(this.generalSettings.find(x => x.code == '請求入力明細件数').value);


    this.setControlInit();
    this.setValidator();
    this.setFormatter();
    this.clear();
    this.setAutoComplete();

    this.taxCalculationRoundingType = this.getRoundingType("消費税計算端数処理");
    this.priceCalculationRoundingType = this.getRoundingType("金額計算端数処理");

    let collectCategoryResponse = this.categoryService.GetItems(CategoryType.Collection);
    let billingCategoryResponse = this.categoryService.GetItems(CategoryType.Billing);
    let departmentResponse = this.departmentService.GetItems();
    let staffResponse = this.staffService.GetItems();
    let columnNamesResponse = this.columnNameSettingService.Get(CategoryType.Billing);
    let collationSettingResponse = this.collationSettingService.Get();
    let currencyResponse = this.currencyService.GetItems();

    let holOpt = new HolidayCalendarSearch();
    holOpt.companyId = this.userInfoService.Company.id;
    let holidayResponse = this.holidayService.GetItems(holOpt);

    forkJoin(
      collectCategoryResponse,
      billingCategoryResponse,
      departmentResponse,
      staffResponse,
      columnNamesResponse,
      collationSettingResponse,
      currencyResponse,
      holidayResponse,
    )
      .subscribe(
        responseList => {
          if (responseList != undefined && responseList.length == 8) {
            this.collectCategoriesResult = new CategoriesResult();
            this.billingCategoryResult = new CategoriesResult();
            this.departmentsResult = new DepartmentsResult();
            this.staffsResult = new StaffsResult();
            this.columnNameSettingResult = new ColumnNameSettingsResult();
            this.collationSettingResult = new CollationSettingResult();
            this.currencyResult = new CurrenciesResult();
            this.holidaysResult = new HolidayCalendarsResult();

            this.collectCategoriesResult.categories = responseList[0];
            this.billingCategoryResult.categories = responseList[1];
            this.departmentsResult.departments = responseList[2];
            this.staffsResult.staffs = responseList[3];
            this.columnNameSettingResult.columnNames = responseList[4];
            this.collationSettingResult.collationSetting = responseList[5];
            this.currencyResult.currencies = responseList[6];
            this.holidaysResult.holidayCalendars = responseList[7];

            if (StringUtil.IsNullOrEmpty(this.paramId)) {
              this.ComponentStatus = this.COMPONENT_STATUS_TYPE.CREATE;
              this.currentBilling = null;
              modalRouterProgressComponentRef.destroy();
            }
            else {
              if (!this.userInfoService.isFunctionAvailable(FunctionType.ModifyBilling)) {
                this.securityHideShow = false;
              }
              // 詰め替えを行う。
              let option = new BillingSearch();
              option.companyId = this.userInfoService.Company.id;
              if (StringUtil.IsNullOrEmpty(this.paramInputId)) {
                option.billingId = parseInt(this.paramId);
              }
              else {
                option.billingInputId = parseInt(this.paramInputId);
              }

              this.billingService.GetItems(option)
                .subscribe(response => {
                  if (response == PROCESS_RESULT_RESULT_TYPE.FAILURE || response.length == 0) {
                    this.processCustomResult = this.processResultService.processAtWarning(
                      this.processCustomResult, MSG_WNG.NOT_EXIST_SEARCH_DATA, this.partsResultMessageComponent);
                      modalRouterProgressComponentRef.destroy();
                      return;
                  }

                  let billing: Billing = response[0];
                  this.isEditable = this.canUpdate(billing);

                  if (this.paramProcess == "refCreate") {
                    this.ComponentStatus = this.COMPONENT_STATUS_TYPE.CREATE;
                  }
                  else if (this.isEditable) {
                    this.ComponentStatus = this.COMPONENT_STATUS_TYPE.UPDATE;
                  }
                  else {
                    this.ComponentStatus = this.COMPONENT_STATUS_TYPE.REFERE;
                  }

                  if (billing.billingInputId != undefined) {
                    this.currentBillingsByInputId = response;
                    this.setBillingHeader(this.currentBillingsByInputId[0]);
                    this.setBillingDetails(this.currentBillingsByInputId);
                  }
                  else {
                    this.currentBilling = billing;
                    this.setBillingHeader(this.currentBilling);
                    this.setBillingDetails([this.currentBilling]);
                  }

                  this.isFirstRowOnly = this.isEditable && billing.inputType == BILL_INPUT_TYPE_DICTIONARY[1].id;
                  if (this.isFirstRowOnly) {
                    for (let i = 1; i < this.billingCount; i++) {
                      this.detailSalesAtCtrls[i].setValue(null);
                      this.detailSalesAtCtrls[i].disable();
                      this.detailBillingCategoryCodeCtrls[i].disable();
                      this.detailTaxClassIdCtrls[i].disable();
                      this.detailDebitAccountTitleIdCtrls[i].disable();
                      this.detailMemoCtrls[i].disable();
                      this.detailNoteFlagCtrls[i].disable();
                      this.detailBillingIdCtrls[i].disable();
                      this.detailContractNumberCtrls[i].disable();
                      this.detailQuantityCtrls[i].disable();
                      this.detailUnitSymbolCtrls[i].disable();
                      this.detailUnitPriceCtrls[i].disable();
                      this.detailBillingNoTaxAmountCtrls[i].disable();
                      this.detailTaxAmountCtrls[i].disable();
                      this.detailBillingAmountCtrls[i].disable();

                    }
                  }

                  this.setSum();

                  modalRouterProgressComponentRef.destroy();

                });
            }

          }
          else{
            modalRouterProgressComponentRef.destroy();
          }
        }
      );
  }

  ngAfterViewInit(){
    HtmlUtil.nextFocusByName(this.elementRef, 'billedAtCtrl', EVENT_TYPE.NONE);
  }

  public setBillingHeader(billing: Billing) {
    //ヘッダー情報
    if (this.paramProcess != "refCreate") {
      this.billingIdCtrl.setValue(billing.billingInputId);
      let tmpDate: Date;
      tmpDate = new Date(billing.billedAt);
      this.billedAtCtrl.setValue(new NgbDate(tmpDate.getFullYear(), tmpDate.getMonth() + 1, tmpDate.getDate()));
      tmpDate = new Date(billing.closingAt);
      this.closingAtCtrl.setValue(new NgbDate(tmpDate.getFullYear(), tmpDate.getMonth() + 1, tmpDate.getDate()));
      this.invoiceCodeCtrl.setValue(billing.invoiceCode);
      tmpDate = new Date(billing.salesAt);
      this.saleAtCtrl.setValue(new NgbDate(tmpDate.getFullYear(), tmpDate.getMonth() + 1, tmpDate.getDate()));
      tmpDate = new Date(billing.dueAt);
      this.dueAtCtrl.setValue(new NgbDate(tmpDate.getFullYear(), tmpDate.getMonth() + 1, tmpDate.getDate()));  
    }

    if (this.userInfoService.ApplicationControl.useForeignCurrency == 1) {
      this.currencyCodeCtrl.setValue(billing.currencyCode);
    }

    this.collectCategoryCodeCtrl.setValue(billing.collectCategoryCode);

    //////////////////////////////////////////////////////
    this.customerService.GetItemsById(billing.customerId)
      .subscribe(response => {
        let customerResult = new CustomerResult();
        customerResult.customer = response[0];
        if (response != PROCESS_RESULT_RESULT_TYPE.FAILURE && response.length > 0) {
          this.customer = customerResult.customer;
        }
      });
    this.customerCodeCtrl.setValue(billing.customerCode);
    this.customerNameCtrl.setValue(billing.customerName);

    this.departmentCodeCtrl.setValue(billing.departmentCode);
    this.departmentNameCtrl.setValue(billing.departmentName);
    this.departmentId = billing.departmentId;

    this.staffCodeCtrl.setValue(billing.staffCode);
    this.staffNameCtrl.setValue(billing.staffName);
    this.staffId = billing.staffId;

    //////////////////////////////////////////////////////
    if (billing.destinationId != undefined) {
      this.destinationService.GetItems()
        .subscribe(response => {
          if (response != PROCESS_RESULT_RESULT_TYPE.FAILURE && response.length > 0) {
            let destinationResult = new DestinationsResult();
            destinationResult.destinations = response;
            let destination = destinationResult.destinations.find(x => x.id == billing.destinationId);
            this.destinationCodeCtrl.setValue(destination.code);
            this.destinationNameCtrl.setValue(destination.name);
          }
        });
    }
    this.destinationId = billing.destinationId;

  }

  public setBillingDetails(billings: Billing[]) {

    let pipe = new RacCurrencyPipe();

    for (let i = 0; i < billings.length; i++) {
      let billing = billings[i];

      if (this.paramProcess != "refCreate") {
        let tmpDate: Date;
        tmpDate = new Date(billing.salesAt);
        this.detailSalesAtCtrls[i].setValue(new NgbDate(tmpDate.getFullYear(), tmpDate.getMonth() + 1, tmpDate.getDate()));
      }

      this.detailBillingCategoryIdCtrls[i].setValue(billing.billingCategoryId);
      this.detailBillingCategoryCodeCtrls[i].setValue(billing.billingCategoryCode + ":" + billing.billingCategoryName);

      this.detailDebitAccountTitleIds[i].setValue(billing.debitAccountTitleId);

      this.detailMemoCtrls[i].setValue(billing.note1);

      if (this.ComponentStatus != COMPONENT_STATUS_TYPE.CREATE) {
        this.detailBillingIdCtrls[i].setValue(billing.id);
        this.detailContractNumberCtrls[i].setValue(billing.contractNumber);
      }

      this.detailQuantityCtrls[i].setValue(billing.quantity);
      this.detailUnitSymbolCtrls[i].setValue(billing.unitSymbol);
      this.detailUnitPriceCtrls[i].setValue(pipe.transform(billing.unitPrice));

      this.detailBillingNoTaxAmountCtrls[i].setValue(pipe.transform(billing.price));
      this.detailTaxAmountCtrls[i].setValue(pipe.transform(billing.taxAmount));
      this.detailBillingAmountCtrls[i].setValue(pipe.transform(billing.billingAmount));

      this.billings[i].note2 = billing.note2;
      this.billings[i].note3 = billing.note3;
      this.billings[i].note4 = billing.note4;
      this.billings[i].note5 = billing.note5;
      this.billings[i].note6 = billing.note6;
      this.billings[i].note7 = billing.note7;
      this.billings[i].note8 = billing.note8;
      this.billingMemos[i].memo = billing.memo;

      if (this.isAnyNoteAndMemoInputed(i)) {
        this.detailNoteFlagCtrls[i].setValue("");
      }
      else {
        this.detailNoteFlagCtrls[i].setValue("〇");
      }


      //////////////////////////////////////////////////////
      let res = new Array<any>();

      res.push(this.taxClassService.GetItems(0));

      //////////////////////////////////////////////////////
      if (billing.debitAccountTitleId != undefined) {
        res.push(this.accountTitleService.Get());
      }

      forkJoin(
        res
      )
      .subscribe(
        responseList => {
          if (responseList[0] != PROCESS_RESULT_RESULT_TYPE.FAILURE && responseList[0].length > 0) {
            let taxClassIds = responseList[0] as TaxClass[];
            let taxClass = taxClassIds.find(x => x.id == billing.taxClassId);
            this.detailTaxClassIdCtrls[i].setValue(taxClass.id + ":" + taxClass.name);
          }

          if (billing.debitAccountTitleId != undefined&&responseList.length==2) {
            if (responseList[1] != PROCESS_RESULT_RESULT_TYPE.FAILURE ) {
              let accountsResult = new AccountTitlesResult();
              accountsResult.accountTitles = responseList[1] as AccountTitle[];
              let accountTitle = accountsResult.accountTitles.find(x => x.id == billing.debitAccountTitleId);
              this.detailDebitAccountTitleIdCtrls[i].setValue(accountTitle.code + ":" + accountTitle.name);
            }
          }

          // Disp部分に追加する
          this.detailAdd(i,true);

        }
      );




    }

    if (this.paramProcess != "refCreate") {
      let billing = billings[0];
      let tmpDate = new Date(billing.salesAt);
      for (let i = billings.length; i < this.billingCount; i++) {
        this.detailSalesAtCtrls[i].setValue(new NgbDate(tmpDate.getFullYear(), tmpDate.getMonth() + 1, tmpDate.getDate()));
      }
    }


  }

  public setControlInit() {
    this.billingIdCtrl = new FormControl("");
    this.billedAtCtrl = new FormControl("", [Validators.required, Validators.maxLength(10)]);
    this.closingAtCtrl = new FormControl("", [Validators.required, Validators.maxLength(10)]);
    this.invoiceCodeCtrl = new FormControl("", [Validators.maxLength(20)]);
    this.currencyCodeCtrl = new FormControl("", [Validators.maxLength(3)]);
    this.saleAtCtrl = new FormControl("", [Validators.maxLength(10)]);
    this.dueAtCtrl = new FormControl("", [Validators.required, Validators.maxLength(10)]);
    this.collectCategoryCodeCtrl = new FormControl("", [Validators.required]);
    this.customerCodeCtrl = new FormControl("", [Validators.required, Validators.maxLength(this.userInfoService.ApplicationControl.customerCodeLength),]);
    this.customerNameCtrl = new FormControl("");
    this.departmentCodeCtrl = new FormControl("", [Validators.required, Validators.maxLength(this.userInfoService.ApplicationControl.departmentCodeLength),]);
    this.departmentNameCtrl = new FormControl("");
    this.staffCodeCtrl = new FormControl("", [Validators.required, Validators.maxLength(this.userInfoService.ApplicationControl.staffCodeLength),]);
    this.staffNameCtrl = new FormControl("");
    this.destinationCodeCtrl = new FormControl("");
    this.destinationNameCtrl = new FormControl("");

    this.undefineCtrl = new FormControl("");

    this.billings = new Array(this.billingCount);
    this.billingMemos = new Array(this.billingCount);

    for (let i = 0; i < this.billingCount; i++) {

      this.billings[i] = new Billing(); //20190305
      this.billingMemos[i] = new BillingMemo(); //20190305      

      this.notInputContractNumFlags[i] = false;

      this.detailSalesAtCtrls[i] = new FormControl("", [Validators.maxLength(10)]);
      this.detailBillingCategoryIdCtrls[i] = new FormControl("");
      this.detailBillingCategoryCodeCtrls[i] = new FormControl("");
      this.detailTaxClassIdCtrls[i] = new FormControl("");
      this.detailDebitAccountTitleIdCtrls[i] = new FormControl("");
      this.detailDebitAccountTitleIds[i] = new FormControl("");
      this.detailMemoCtrls[i] = new FormControl("", [Validators.maxLength(100)]);
      this.detailNoteFlagCtrls[i] = new FormControl("");
      this.detailBillingIdCtrls[i] = new FormControl("");
      this.detailContractNumberCtrls[i] = new FormControl("", [Validators.maxLength(20)]);
      this.detailQuantityCtrls[i] = new FormControl("", [Validators.maxLength(9), Validators.pattern('^([1-9][0-9]{0,5}|0)(\.[0-9]{1,2})?$')]);
      this.detailUnitSymbolCtrls[i] = new FormControl("", [Validators.maxLength(3)]);
      this.detailUnitPriceCtrls[i] = new FormControl("", [Validators.maxLength(10)]);
      this.detailBillingNoTaxAmountCtrls[i] = new FormControl("", [Validators.maxLength(16)]);
      this.detailTaxAmountCtrls[i] = new FormControl("", [Validators.maxLength(16)]);
      this.detailBillingAmountCtrls[i] = new FormControl("", [Validators.maxLength(16)]);

      this.setNotInput(false, i);
    }

    this.sumBillingNoTaxAmountCtls = new FormControl("");
    this.sumTaxAmountCtls = new FormControl("");
    this.sumBillingAmountCtls = new FormControl("");

    this.rdoCalAmountCtrl = new FormControl("", [Validators.required]);

  }
  
  public setValidator() {
    this.MyFormGroup = new FormGroup({

      billingIdCtrl: this.billingIdCtrl,
      billedAtCtrl: this.billedAtCtrl,
      closingAtCtrl: this.closingAtCtrl,
      invoiceCodeCtrl: this.invoiceCodeCtrl,
      currencyCodeCtrl: this.currencyCodeCtrl,
      saleAtCtrl: this.saleAtCtrl,
      dueAtCtrl: this.dueAtCtrl,
      collectCategoryCodeCtrl: this.collectCategoryCodeCtrl,
      customerCodeCtrl: this.customerCodeCtrl,
      customerNameCtrl: this.customerNameCtrl,
      departmentCodeCtrl: this.departmentCodeCtrl,
      departmentNameCtrl: this.departmentNameCtrl,
      staffCodeCtrl: this.staffCodeCtrl,
      staffNameCtrl: this.staffNameCtrl,
      destinationCodeCtrl: this.destinationCodeCtrl,
      destinationNameCtrl: this.destinationNameCtrl,

      sumBillingNoTaxAmountCtls: this.sumBillingNoTaxAmountCtls,
      sumTaxAmountCtls: this.sumTaxAmountCtls,
      sumBillingAmountCtls: this.sumBillingAmountCtls,

      rdoCalAmountCtrl: this.rdoCalAmountCtrl,

      UndefineCtrl: this.undefineCtrl,

    });

    for (let i = 0; i < this.billingCount; i++) {
      this.MyFormGroup.addControl("detailSalesAtCtrl" + i, this.detailSalesAtCtrls[i]);
      this.MyFormGroup.addControl("detailBillingCategoryIdCtrl" + i, this.detailBillingCategoryIdCtrls[i]);
      this.MyFormGroup.addControl("detailBillingCategoryCodeCtrl" + i, this.detailBillingCategoryCodeCtrls[i]);
      this.MyFormGroup.addControl("detailTaxClassIdCtrl" + i, this.detailTaxClassIdCtrls[i]);
      this.MyFormGroup.addControl("detailDebitAccountTitleIdCtrl" + i, this.detailDebitAccountTitleIdCtrls[i]);
      this.MyFormGroup.addControl("detailDebitAccountTitleId" + i, this.detailDebitAccountTitleIds[i]);
      this.MyFormGroup.addControl("detailMemoCtrl" + i, this.detailMemoCtrls[i]);
      this.MyFormGroup.addControl("detailNoteFlagCtrl" + i, this.detailNoteFlagCtrls[i]);
      this.MyFormGroup.addControl("detailBillingIdCtrl" + i, this.detailBillingIdCtrls[i]);
      this.MyFormGroup.addControl("detailContractNumberCtrl" + i, this.detailContractNumberCtrls[i]);
      this.MyFormGroup.addControl("detailQuantityCtrl" + i, this.detailQuantityCtrls[i]);
      this.MyFormGroup.addControl("detailUnitSymbolCtrl" + i, this.detailUnitSymbolCtrls[i]);
      this.MyFormGroup.addControl("detailUnitPriceCtrl" + i, this.detailUnitPriceCtrls[i]);
      this.MyFormGroup.addControl("detailBillingNoTaxAmountCtrl" + i, this.detailBillingNoTaxAmountCtrls[i]);
      this.MyFormGroup.addControl("detailTaxAmountCtrl" + i, this.detailTaxAmountCtrls[i]);
      this.MyFormGroup.addControl("detailBillingAmountCtrl" + i, this.detailBillingAmountCtrls[i]);
    }

  }

  public setFormatter() {

    FormatterUtil.setAlphabetFormatter(this.currencyCodeCtrl);
    if (this.userInfoService.ApplicationControl.customerCodeType == CODE_TYPE.NUMBER) {
      FormatterUtil.setNumberFormatter(this.customerCodeCtrl);
    }
    else if (this.userInfoService.ApplicationControl.customerCodeType == CODE_TYPE.HANKANA) {
      FormatterUtil.setCustomerCodeKanaFormatter(this.customerCodeCtrl);
    }
    else {
      FormatterUtil.setCustomerCodeAlphaFormatter(this.customerCodeCtrl);
    }
    if (this.userInfoService.ApplicationControl.staffCodeType == CODE_TYPE.NUMBER) {
      FormatterUtil.setNumberFormatter(this.staffCodeCtrl);
    }
    else {
      FormatterUtil.setCodeFormatter(this.staffCodeCtrl);
    }
    if (this.userInfoService.ApplicationControl.departmentCodeType == CODE_TYPE.NUMBER) {
      FormatterUtil.setNumberFormatter(this.departmentCodeCtrl);
    }
    else {
      FormatterUtil.setCodeFormatter(this.departmentCodeCtrl);
    }
    FormatterUtil.setCodeFormatter(this.destinationCodeCtrl);

    for (let index = 0; index < this.billingCount; index++) {

      FormatterUtil.setCurrencyPeriodFormatter(this.detailQuantityCtrls[index]);
      FormatterUtil.setCurrencyPeriodFormatter(this.detailUnitPriceCtrls[index]);

      this.detailBillingNoTaxAmountCtrls[index].valueChanges.subscribe(
        (value: string) => {
          if (typeof value === "string") {
            if (value.match(/[^-0-9,.]/)) {
              this.detailBillingNoTaxAmountCtrls[index].setValue(value.replace(/[^-0-9,.]/g, ''));
            }
            else {
              this.setDetailBillingNoTaxAmount(index);
            }
          }
        });

      this.detailTaxAmountCtrls[index].valueChanges.subscribe(
        (value: string) => {
          if (typeof value === "string") {
            if (value.match(/[^-0-9,.]/)) {
              this.detailTaxAmountCtrls[index].setValue(value.replace(/[^-0-9,.]/g, ''));
            }
            else {
              this.setdetailTaxAmountCtrl(index);
            }

          }
        });

      this.detailBillingAmountCtrls[index].valueChanges.subscribe(
        (value: string) => {
          if (typeof value === "string") {
            if (value.match(/[^-0-9,.]/)) {
              this.detailBillingAmountCtrls[index].setValue(value.replace(/[^-0-9,.]/g, ''));
            }
            else {
              this.setDetailBillingAmountCtrl(index);
            }
          }
        });

    }

    /*
    for (let i = 0; i < this.billingCount; i++) {
      FormatterUtil.setNumberFormatter(this.detailBillingNoTaxAmountCtrls[i]);
      FormatterUtil.setNumberFormatter(this.detailTaxAmountCtrls[i]);
      FormatterUtil.setNumberFormatter(this.detailBillingAmountCtrls[i]);
    }
    */

  }


  public setAutoComplete(){
    // 得意先
    this.initAutocompleteCustomers(this.customerCodeCtrl,this.customerService,0);

    // 請求部門
    this.initAutocompleteDepartments(this.departmentCodeCtrl,this.departmentService,0);

    // 担当者
    this.initAutocompleteStaffs(this.staffCodeCtrl,this.staffService,0);

    // // 請求区分
    this.initAutocompleteCategories(CategoryType.Billing,this.detailBillingCategoryCodeCtrls[0],this.categoryService,0);
    
    // // 税区
    this.initautocompleteTaxes(this.detailTaxClassIdCtrls[0],this.taxClassService,0);

    // // 科目
    this.initautocompleteAccountTitle(this.detailDebitAccountTitleIdCtrls[0],this.accountTitleService,0);



  }  

  public setNotInput(flag: boolean, index: number) {

    if (flag) {
      this.detailContractNumberCtrls[index].enable();
    }
    else {
      this.detailContractNumberCtrls[index].disable();
      this.detailContractNumberCtrls[index].setValue("");
    }
  }

  /*
    table:Master名称
    keyCode:キーコードがある場合はF9のみモーダルを開く
    index:明細行の行No
  */
  public openMasterModal(table: TABLE_INDEX,  index: number = -1) {

    // オートコンプリートパネルを閉じる
    this.customerCodeTrigger.closePanel();
    this.departmentCodeTrigger.closePanel();
    this.staffCodeTrigger.closePanel();


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
              this.customer = componentRef.instance.SelectedObject;
              this.setHeaderInfo();
              break;
            }
          case TABLE_INDEX.MASTER_DEPARTMENT:
            {
              this.departmentCodeCtrl.setValue(componentRef.instance.SelectedCode);
              this.departmentNameCtrl.setValue(componentRef.instance.SelectedName);
              let department = componentRef.instance.SelectedObject as Department;
              this.departmentId = department.id;
              break;
            }
          case TABLE_INDEX.MASTER_STAFF:
            {
              this.staffCodeCtrl.setValue(componentRef.instance.SelectedCode);
              this.staffNameCtrl.setValue(componentRef.instance.SelectedName);
              let staff = componentRef.instance.SelectedObject as Staff;
              this.staffId = staff.id;
              break;
            }
          case TABLE_INDEX.MASTER_DESTINATION:
            {
              this.destinationCodeCtrl.setValue(componentRef.instance.SelectedCode);
              this.destinationNameCtrl.setValue(componentRef.instance.SelectedName);
              let destination = componentRef.instance.SelectedObject as Destination;
              this.destinationId = destination.id;
              break;
            }
          case TABLE_INDEX.MASTER_BILLING_CATEGORY:
            {
              this.detailBillingCategoryCodeCtrls[index].setValue(
                componentRef.instance.SelectedCode + ":" + componentRef.instance.SelectedName);
              let billingCategory = componentRef.instance.SelectedObject as Category;
              this.detailBillingCategoryIdCtrls[index].setValue(billingCategory.id);
              //this.notInputContractNumFlags[index] = billingCategory.useLongTermAdvanceReceived == 1 ? true : false;
              this.notInputContractNumFlags[index] = false;

              this.setNotInput(this.notInputContractNumFlags[index], index);
              break;
            }
          case TABLE_INDEX.MASTER_TAX_CLASS:
            {
              this.detailTaxClassIdCtrls[index].setValue(
                componentRef.instance.SelectedId + ":" + componentRef.instance.SelectedName);
              break;
            }
          case TABLE_INDEX.MASTER_ACCOUNT_TITLE:
            {
              this.detailDebitAccountTitleIdCtrls[index].setValue(componentRef.instance.SelectedCode + ":" + componentRef.instance.SelectedName);
              let accountTitle = componentRef.instance.SelectedObject as AccountTitle;
              this.detailDebitAccountTitleIds[index].setValue(accountTitle.id);
              break;
            }
          case TABLE_INDEX.MASTER_CURRENCY:
            {
              this.currencyCodeCtrl.setValue(componentRef.instance.SelectedCode);
              let currency = componentRef.instance.SelectedObject as Currency;
              this.precision = currency.precision;
              break;
            }

        }
      }
      componentRef.destroy();
    });

  }

  ///////////////////////////////////////////////////////////////////////////
  public setBilledAt(eventType: string) {
    // 売上日を設定
    this.saleAtCtrl.setValue(this.billedAtCtrl.value);
    // ヘッダー情報を設定
    this.setSalesAt();

    if (this.customer != undefined) {
      let dClosingAt = this.getClosingAt(DateUtil.ConvertFromDatepicker(this.billedAtCtrl));
      this.closingAtCtrl.setValue(new NgbDate(dClosingAt.getFullYear(), dClosingAt.getMonth() + 1, dClosingAt.getDate()));

      let dDueAt = this.getDueAt(DateUtil.ConvertFromDatepicker(this.closingAtCtrl));
      this.dueAtCtrl.setValue(new NgbDate(dDueAt.getFullYear(), dDueAt.getMonth() + 1, dDueAt.getDate()));
    }
    HtmlUtil.nextFocusByName(this.elementRef, 'closingAtCtrl', eventType);
  }

  ///////////////////////////////////////////////////////////////////////////
  public setClosingAt(eventType: string) {

    HtmlUtil.nextFocusByName(this.elementRef, 'invoiceCodeCtrl', eventType);

  }

  ///////////////////////////////////////////////////////////////////////////
  public setInvoiceCode(eventType: string) {

    HtmlUtil.nextFocusByNames(this.elementRef, ['currencyCodeCtrl', 'saleAtCtrl'], eventType);

  }

  ///////////////////////////////////////////////////////////////////////////
  public setCurrencyCode(eventType: string) {

    if (!StringUtil.IsNullOrEmpty(this.currencyCodeCtrl.value)) {
      this.currencyService.GetItems(this.currencyCodeCtrl.value)
        .subscribe(response => {
          if (response != PROCESS_RESULT_RESULT_TYPE.FAILURE && response.length > 0) {
            let currency = response as Currency;
            this.precision = currency.precision;
          }
          else {
            this.precision = 0;
          }
        });
    }
    else {
      this.precision = 0;
    }

    HtmlUtil.nextFocusByName(this.elementRef, 'saleAtCtrl', eventType);
  }

  ///////////////////////////////////////////////////////////////////////////
  public setSaleAt(eventType: string) {
    // 明細の売上日を設定
    this.setSalesAt();

    HtmlUtil.nextFocusByName(this.elementRef, 'dueAtCtrl', eventType);
  }

  public setSalesAt() {
    // 明細の売上日を設定
    for (let i = 0; i < this.billingCount; i++) {
      this.detailSalesAtCtrls[i].setValue(this.saleAtCtrl.value);
    }
  }

  ///////////////////////////////////////////////////////////////////////////
  public setDueAt(eventType: string) {

    HtmlUtil.nextFocusByName(this.elementRef, 'collectCategoryCodeCtrl', eventType);
  }

  ///////////////////////////////////////////////////////////////////////////
  public setCollectCategoryCode(eventType: string) {

    if (!StringUtil.IsNullOrEmpty(this.collectCategoryCodeCtrl.value)) {
      let collectCategory = this.collectCategoriesResult.categories.find(x => x.code == this.collectCategoryCodeCtrl.value);
    }

    HtmlUtil.nextFocusByName(this.elementRef, 'departmentCodeCtrl', eventType);

  }

  ///////////////////////////////////////////////////////////////////////////
  public setCustomerCode(eventType: string) {

    if (eventType != EVENT_TYPE.BLUR) {
      this.customerCodeTrigger.closePanel();
    }

    if (!StringUtil.IsNullOrEmpty(this.customerCodeCtrl.value)) {
      if ((this.customer != undefined) && (this.customer.code == this.customerCodeCtrl.value)) return;

      this.loadStart();
      this.customerService.GetItems(this.customerCodeCtrl.value)
        .subscribe(response => {
          this.loadEnd();
          if (response != PROCESS_RESULT_RESULT_TYPE.FAILURE && response.length > 0) {
            this.customerCodeCtrl.setValue(response[0].code);
            this.customerNameCtrl.setValue(response[0].name);
            this.customer = response[0];
            this.setHeaderInfo();
            HtmlUtil.nextFocusByName(this.elementRef, 'departmentCodeCtrl', eventType);
          }
          else {
            //this.customerCodeCtrl.setValue("");
            this.customerNameCtrl.setValue("");
            this.departmentCodeCtrl.setValue("");
            this.departmentNameCtrl.setValue("");
            this.staffCodeCtrl.setValue("");
            this.staffNameCtrl.setValue("");
            this.collectCategoryCodeCtrl.setValue("");
            this.closingAtCtrl.setValue("");
            this.dueAtCtrl.setValue("");
            this.customer = null;
          }
        });

    }
    else {
      this.customerCodeCtrl.setValue("");
      this.customerNameCtrl.setValue("");
      this.departmentCodeCtrl.setValue("");
      this.departmentNameCtrl.setValue("");
      this.staffCodeCtrl.setValue("");
      this.staffNameCtrl.setValue("");
      this.collectCategoryCodeCtrl.setValue("");
      this.closingAtCtrl.setValue("");
      this.dueAtCtrl.setValue("");
      this.customer = null;
      HtmlUtil.nextFocusByName(this.elementRef, 'departmentCodeCtrl', eventType);
    }

  }

  ///////////////////////////////////////////////////////////////////////////
  public setDepartmentCode(eventType: string) {

    if (eventType != EVENT_TYPE.BLUR) {
      this.departmentCodeTrigger.closePanel();
    }

    if (!StringUtil.IsNullOrEmpty(this.departmentCodeCtrl.value)) {

      this.loadStart();
      this.departmentService.GetItems(this.departmentCodeCtrl.value)
        .subscribe(response => {
          this.loadEnd();
          if (response != PROCESS_RESULT_RESULT_TYPE.FAILURE && response.length > 0) {
            this.departmentCodeCtrl.setValue(response[0].code);
            this.departmentNameCtrl.setValue(response[0].name);
            HtmlUtil.nextFocusByName(this.elementRef, 'staffCodeCtrl', eventType);
          }
          else {
            //this.departmentCodeCtrl.setValue("");
            this.departmentNameCtrl.setValue("");
          }
        });
    }
    else {
      this.departmentCodeCtrl.setValue("");
      this.departmentNameCtrl.setValue("");
      HtmlUtil.nextFocusByName(this.elementRef, 'staffCodeCtrl', eventType);
    }

  }

  ///////////////////////////////////////////////////////////////////////////
  public setStaffCode(eventType: string) {

    if (eventType != EVENT_TYPE.BLUR) {
      this.staffCodeTrigger.closePanel();
    }

    if (!StringUtil.IsNullOrEmpty(this.staffCodeCtrl.value)) {

      this.loadStart();
      this.staffService.GetItems(this.staffCodeCtrl.value)
        .subscribe(response => {
          this.loadEnd();
          if (response != PROCESS_RESULT_RESULT_TYPE.FAILURE && response.length > 0) {
            this.staffCodeCtrl.setValue(response[0].code);
            this.staffNameCtrl.setValue(response[0].name);
            HtmlUtil.nextFocusByNames(this.elementRef, ['destinationCodeCtrl', 'billedAtCtrl'], eventType);
          }
          else {
            //this.staffCodeCtrl.setValue("");
            this.staffNameCtrl.setValue("");
          }
        });

    }
    else {
      this.staffCodeCtrl.setValue("");
      this.staffNameCtrl.setValue("");
      HtmlUtil.nextFocusByNames(this.elementRef, ['destinationCodeCtrl', 'billedAtCtrl'], eventType);
    }
  }

  ///////////////////////////////////////////////////////////////////////////
  public setDestinationName(eventType: string) {

    if (!StringUtil.IsNullOrEmpty(this.destinationCodeCtrl.value)) {

      this.loadStart();
      this.destinationService.GetItems(this.destinationCodeCtrl.value)
        .subscribe(response => {
          this.loadEnd();
          if (response != undefined && response.length > 0) {
            this.destinationCodeCtrl.setValue(response[0].code);
            this.destinationNameCtrl.setValue(response[0].name);
            HtmlUtil.nextFocusByName(this.elementRef, 'billedAtCtrl', eventType);
          }
          else {
            this.destinationCodeCtrl.setValue("");
            this.destinationNameCtrl.setValue("");
            HtmlUtil.nextFocusByName(this.elementRef, 'billedAtCtrl', eventType);
          }
        });
    }
    else {
      this.destinationCodeCtrl.setValue("");
      this.destinationNameCtrl.setValue("");
      HtmlUtil.nextFocusByName(this.elementRef, 'billedAtCtrl', eventType);
    }

  }

  ///////////////////////////////////////////////////////////////////////////
  public setHeaderInfo() {
    // 請求部門・担当者コード・回収区分を設定
    if (!StringUtil.IsNullOrEmpty(this.customerCodeCtrl.value)) {

      if (StringUtil.IsNullOrEmpty(this.departmentCodeCtrl.value) && StringUtil.IsNullOrEmpty(this.staffCodeCtrl.value)) {

        let staff = this.staffsResult.staffs.find(x => x.id == this.customer.staffId);
        let department = this.departmentsResult.departments.find(x => x.id == staff.departmentId);
        this.departmentCodeCtrl.setValue(department.code);
        this.departmentNameCtrl.setValue(department.name);
        this.departmentId = department.id;

        this.staffCodeCtrl.setValue(staff.code);
        this.staffNameCtrl.setValue(staff.name);
        this.staffId = staff.id;
        this.collectCategoryCodeCtrl.setValue(this.customer.collectCategoryCode);
      }

      if ((this.customer != undefined) && (this.customer.code == this.customerCodeCtrl.value)  && (this.billedAtCtrl.value != undefined)) {
        let dClosingAt = this.getClosingAt(DateUtil.ConvertFromDatepicker(this.billedAtCtrl));
        this.closingAtCtrl.setValue(new NgbDate(dClosingAt.getFullYear(), dClosingAt.getMonth() + 1, dClosingAt.getDate()));

        let dDueAt = this.getDueAt(DateUtil.ConvertFromDatepicker(this.closingAtCtrl));
        this.dueAtCtrl.setValue(new NgbDate(dDueAt.getFullYear(), dDueAt.getMonth() + 1, dDueAt.getDate()));
      }

    }
    else {
      this.departmentCodeCtrl.setValue("");
      this.departmentNameCtrl.setValue("");
      this.staffCodeCtrl.setValue("");
      this.staffNameCtrl.setValue("");
      this.collectCategoryCodeCtrl.setValue("");
    }
  }

  public openInvoiceMemoModal(index: number) {
    let componentFactory = this.componentFactoryResolver.resolveComponentFactory(ModalInvoiceMemoComponent);
    let componentRef = this.viewContainerRef.createComponent(componentFactory);

    this.billingMemos[index].billingId = index + 1;
    componentRef.instance.Billing = this.billings[index];
    componentRef.instance.BillingMemo = this.billingMemos[index];
    componentRef.instance.ColumnNameSettings = this.columnNameSettingResult.columnNames;

    componentRef.instance.Closing.subscribe(() => {
      componentRef.destroy();
      this.setDetailNoteFlag(index);
    });
  }

  /////////////////////////////////////////////////////////////////////////////
  // 詳細部分
  /////////////////////////////////////////////////////////////////////////////

  /////////////////////////////////////////////////////////////////////////////
  public setDetailSalesAt(eventType: any, index: number) {
    HtmlUtil.nextFocusByName(this.elementRef, 'detailBillingCategoryCodeCtrl' + index, eventType);
  }

  /////////////////////////////////////////////////////////////////////////////
  public setDetailBillingCategoryCode(eventType: any, index: number) {
  
    if(eventType!=EVENT_TYPE.BLUR){
      this.detailBillingCategoryCodeTrigger.closePanel();
    }


    if (!StringUtil.IsNullOrEmpty(this.detailBillingCategoryCodeCtrls[index].value)) {

      if (eventType == "blur") {
        this.detailBillingCategoryCodeCtrls[index].setValue(this.detailBillingCategoryCodeCtrls[index].value.split(":")[0]);
      }

      this.detailBillingCategoryCodeCtrls[index].setValue(StringUtil.setPaddingFrontZero(this.detailBillingCategoryCodeCtrls[index].value, 2));

      this.loadStart();
      this.categoryService.GetItems(CategoryType.Billing, this.detailBillingCategoryCodeCtrls[index].value)
        .subscribe(response => {
          this.loadEnd();
          if (response != PROCESS_RESULT_RESULT_TYPE.FAILURE && response.length > 0) {
            let billingCategory = response[0] as Category;
            this.detailBillingCategoryCodeCtrls[index].setValue(billingCategory.code + ":" + billingCategory.name);
            this.detailBillingCategoryIdCtrls[index].setValue(billingCategory.id);

            //税区
            this.detailTaxClassIdCtrls[index].setValue(billingCategory.taxClassId.toString());

            //契約番号
            //this.notInputContractNumFlags[index] = billingCategory.useLongTermAdvanceReceived == 1 ? true : false;
            this.notInputContractNumFlags[index] = false;

            HtmlUtil.nextFocusByName(this.elementRef, 'detailTaxClassIdCtrl' + index, eventType);
          }
          else {
            //this.detailBillingCategoryCodeCtrls[index].setValue("");
            this.detailBillingCategoryIdCtrls[index].setValue("");
            this.detailTaxClassIdCtrls[index].setValue("");
            this.notInputContractNumFlags[index] = false;
            //HtmlUtil.nextFocusByName(this.elementRef, 'detailTaxClassIdCtrl' + index, eventType);
          }

          this.setNotInput(this.notInputContractNumFlags[index], index);

        });
    }
    else {
      HtmlUtil.nextFocusByName(this.elementRef, 'detailTaxClassIdCtrl' + index, eventType);
      this.detailBillingCategoryCodeCtrls[index].setValue("");
      this.detailBillingCategoryIdCtrls[index].setValue("");
      this.detailTaxClassIdCtrls[index].setValue("");
      this.notInputContractNumFlags[index] = false;

      this.setNotInput(this.notInputContractNumFlags[index], index);

    }
  }

  public initDetailBillingCategoryCode(index: number) {

    if (!StringUtil.IsNullOrEmpty(this.detailBillingCategoryCodeCtrls[index].value)) {
      let tmp = this.detailBillingCategoryCodeCtrls[index].value;
      tmp = tmp.split(":")[0];

      this.detailBillingCategoryCodeCtrls[index].setValue(tmp);
    }
  }

  /////////////////////////////////////////////////////////////////////////////
  public setDetailTaxClassId(eventType: string, index: number) {

    if(eventType!=EVENT_TYPE.BLUR){
      this.detailTaxClassTrigger.closePanel();
    }


    if (!StringUtil.IsNullOrEmpty(this.detailTaxClassIdCtrls[index].value)) {
      let taxIdTmp = ""+this.detailTaxClassIdCtrls[index].value;
      taxIdTmp = taxIdTmp.split(":")[0];
      this.loadStart();
      this.taxClassService.GetItems(0)
        .subscribe(response => {
          this.loadEnd();
          if (response != PROCESS_RESULT_RESULT_TYPE.FAILURE && response.length > 0) {

            let taxClassResult = new TaxClassResult();
            taxClassResult.processResult = new ProcessResult();
            taxClassResult.processResult.result = true;
            taxClassResult.taxClass = new Array<TaxClass>();
            taxClassResult.taxClass = response;

            let taxClasses = taxClassResult.taxClass.filter(
              function (taxClassTmp: TaxClass) {
                return taxClassTmp.id == parseInt(taxIdTmp);
              }
            )

            if (taxClasses.length == 1) {
              this.detailTaxClassIdCtrls[index].setValue(taxClasses[0].id + ":" + taxClasses[0].name);
              HtmlUtil.nextFocusByName(this.elementRef, 'detailDebitAccountTitleIdCtrl' + index, eventType);
            }
            else {
              // this.detailTaxClassIdCtrls[index].setValue("");
              // HtmlUtil.nextFocusByName(this.elementRef, 'detailDebitAccountTitleIdCtrl' + index, eventType);
            }
          }
          else {
            this.detailTaxClassIdCtrls[index].setValue("");
            HtmlUtil.nextFocusByName(this.elementRef, 'detailDebitAccountTitleIdCtrl' + index, eventType);
          }
        });

    }
    else {
      HtmlUtil.nextFocusByName(this.elementRef, 'detailDebitAccountTitleIdCtrl' + index, eventType);
      this.detailTaxClassIdCtrls[index].setValue("");

    }

  }

  public initDetailTaxClassId(index: number) {

    if (!StringUtil.IsNullOrEmpty(this.detailTaxClassIdCtrls[index].value)) {
      let tmp = ""+this.detailTaxClassIdCtrls[index].value;
      tmp = tmp.split(":")[0];

      this.detailTaxClassIdCtrls[index].setValue(tmp);
    }

  }

  /////////////////////////////////////////////////////////////////////////////
  public setDetailDebitAccountTitleId(eventType: string, index: number) {

    if(eventType!=EVENT_TYPE.BLUR){
      this.detailBebitAccountTitleIdTrigger.closePanel();
    }

    if (!StringUtil.IsNullOrEmpty(this.detailDebitAccountTitleIdCtrls[index].value)) {

      let accountTitleCodeTmp = this.detailDebitAccountTitleIdCtrls[index].value;
      accountTitleCodeTmp = accountTitleCodeTmp.split(":")[0];
      this.loadStart();
      this.accountTitleService.Get(accountTitleCodeTmp)
        .subscribe(response => {
          this.loadEnd();
          if (response != PROCESS_RESULT_RESULT_TYPE.FAILURE && response.length > 0) {
            this.detailDebitAccountTitleIdCtrls[index].setValue(response[0].code + ":" + response[0].name);
            this.detailDebitAccountTitleIds[index].setValue(response[0].id);
            HtmlUtil.nextFocusByName(this.elementRef, 'detailMemoCtrl' + index, eventType);
          }
          else {
            // this.detailDebitAccountTitleIdCtrls[index].setValue("");
            // this.detailDebitAccountTitleIds[index].setValue("");
            // HtmlUtil.nextFocusByName(this.elementRef, 'detailMemoCtrl' + index, eventType);
          }
        });

    }
    else {
      HtmlUtil.nextFocusByName(this.elementRef, 'detailMemoCtrl' + index, eventType);
      this.detailDebitAccountTitleIdCtrls[index].setValue("");

    }

  }

  public initDetailDebitAccountTitleId(index: number) {

    if (!StringUtil.IsNullOrEmpty(this.detailDebitAccountTitleIdCtrls[index].value)) {
      let tmp = this.detailDebitAccountTitleIdCtrls[index].value;
      tmp = tmp.split(":")[0];

      this.detailDebitAccountTitleIdCtrls[index].setValue(tmp);
    }
  }

  /////////////////////////////////////////////////////////////////////////////
  public setDetailMemo(eventType: string, index: number) {
    HtmlUtil.nextFocusByNames(this.elementRef,
      ['detailContractNumberCtrl' + index, 'detailQuantityCtrl' + index], eventType);
  }

  public setDetailNoteFlag(index: number) {

    if (this.isAnyNoteAndMemoInputed(index)) {
      this.detailNoteFlagCtrls[index].setValue("");
    }
    else {
      this.detailNoteFlagCtrls[index].setValue("〇");
    }
  }

  public isAnyNoteAndMemoInputed(index: number): boolean {

    let bRtn =
      StringUtil.IsNullOrEmpty(this.billingMemos[index].memo)
      && StringUtil.IsNullOrEmpty(this.billings[index].note2)
      && StringUtil.IsNullOrEmpty(this.billings[index].note3)
      && StringUtil.IsNullOrEmpty(this.billings[index].note4)
      && StringUtil.IsNullOrEmpty(this.billings[index].note5)
      && StringUtil.IsNullOrEmpty(this.billings[index].note6)
      && StringUtil.IsNullOrEmpty(this.billings[index].note7)
      && StringUtil.IsNullOrEmpty(this.billings[index].note8)

    return bRtn;
  }


  /////////////////////////////////////////////////////////////////////////////
  public setDetailContractNumber(eventType: string, index: number) {
    HtmlUtil.nextFocusByName(this.elementRef, 'detailQuantityCtrl' + index, eventType);
  }

  /////////////////////////////////////////////////////////////////////////////

  public setdetailQuantityCtrl(eventType: string, index: number) {


    this.setBillingAmounFromQuantityWithUnitPrice(index);
    this.setSum();

    HtmlUtil.nextFocusByName(this.elementRef, 'detailUnitSymbolCtrl' + index, eventType);
  }

  /////////////////////////////////////////////////////////////////////////////

  public setdetailUnitSymbolCtrl(eventType: string, index: number) {

    HtmlUtil.nextFocusByName(this.elementRef, 'detailUnitPriceCtrl' + index, eventType);
  }

  /////////////////////////////////////////////////////////////////////////////

  public ignoreTransform(control: FormControl) {
    return control==undefined || control.value==undefined || StringUtil.IsNullOrEmpty(control.value) || control.value === '0';
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
    control.setValue(pipe.transform(control.value, false));
  }

  /////////////////////////////////////////////////////////////////////////////

  public setdetailUnitPriceCtrl(eventType: string, index: number) {

    this.setBillingAmounFromQuantityWithUnitPrice(index);
    this.setSum();

    HtmlUtil.nextFocusByName(this.elementRef, 'detailBillingNoTaxAmountCtrl' + index, eventType);
  }

  public setCurrencyForDetailUnitPrice(eventType: string, index: number) {
    this.onLeaveCurrencyControl(this.detailUnitPriceCtrls[index]);
  }

  public initCurrencyForDetailUnitPrice(index: number) {
    this.onFocusCurrencyControl(this.detailUnitPriceCtrls[index]);
  }

  /////////////////////////////////////////////////////////////////////////////

  public setDetailBillingNoTaxAmount(index: number) {

    if (this.currentFocus != FOCUS.NO_TAX_AMOUNT) {
      return;
    }

    let pipe = new RacCurrencyPipe();

    if ((NumberUtil.ParseInt(this.detailBillingNoTaxAmountCtrls[index].value) != 0)) {

      let taxId = this.detailTaxClassIdCtrls[index].value?this.detailTaxClassIdCtrls[index].value.split(":")[0]:"";
      let isTax = (taxId == TaxClassId.TaxExclusive) || (taxId == TaxClassId.TaxInclusive);
      let taxAmount: number;
      if (this.billedAtCtrl.value == undefined || this.billedAtCtrl.value == null) {
        taxAmount = 0
      }
      else {
        let rate = !isTax ? 0 : this.getTaxRate(DateUtil.convertYYYYMMDD(this.billedAtCtrl));
        taxAmount = this.calculateRounding(this.taxCalculationRoundingType, NumberUtil.ParseInt(this.detailBillingNoTaxAmountCtrls[index].value) * rate, this.precision);        
      }

      if (taxAmount == 0) {
        this.detailTaxAmountCtrls[index].setValue(0);
      }
      else {
        this.detailTaxAmountCtrls[index].setValue(pipe.transform(taxAmount));
      }

      this.detailBillingAmountCtrls[index].setValue(
        pipe.transform(
          NumberUtil.ParseInt(this.detailBillingNoTaxAmountCtrls[index].value) + NumberUtil.ParseInt(this.detailTaxAmountCtrls[index].value)
        )
      );

    }

    this.setSum();
  }

  public nextFocusDetailBillingNoTaxAmount(eventType: string, index: number) {
    HtmlUtil.nextFocusByName(this.elementRef, 'detailTaxAmountCtrl' + index, eventType);
  }

  public setCurrencyForDetailBillingNoTaxAmount(eventType: string, index: number) {
    this.onLeaveCurrencyControl(this.detailBillingNoTaxAmountCtrls[index]);
  }

  public initCurrencyForDetailBillingNoTaxAmount(index: number) {

    this.currentFocus = FOCUS.NO_TAX_AMOUNT;
    this.onFocusCurrencyControl(this.detailBillingNoTaxAmountCtrls[index]);
  }

  /////////////////////////////////////////////////////////////////////////////

  public setdetailTaxAmountCtrl(index: number) {

    let pipe = new RacCurrencyPipe();

    if (this.currentFocus != FOCUS.TAX_AMOUNT) {
      return;
    }

    this.detailBillingNoTaxAmountCtrls[index].setValue(
      pipe.transform(
        (NumberUtil.ParseInt(this.detailBillingAmountCtrls[index].value)
          - NumberUtil.ParseInt(this.detailTaxAmountCtrls[index].value))
      )
    );
    this.setSum();
  }

  public nextFocusdetailTaxAmountCtrl(eventType: string, index: number) {
    HtmlUtil.nextFocusByName(this.elementRef, 'detailBillingAmountCtrl' + index, eventType);
  }

  public setCurrencyForDetailTaxAmount(eventType: string, index: number) {    
    this.onLeaveCurrencyControl(this.detailTaxAmountCtrls[index]);
  }

  public initCurrencyForDetailTaxAmount(index: number) {

    this.currentFocus = FOCUS.TAX_AMOUNT;
    this.onFocusCurrencyControl(this.detailTaxAmountCtrls[index]);
  }
  /////////////////////////////////////////////////////////////////////////////

  public setDetailBillingAmountCtrl(index: number) {

    let pipe = new RacCurrencyPipe();

    if (this.currentFocus != FOCUS.AMOUNT || this.detailTaxClassIdCtrls[index].value == undefined) {
      return;
    }

    if (this.detailTaxClassIdCtrls[index].value != undefined) {
      let taxId = this.detailTaxClassIdCtrls[index].value?this.detailTaxClassIdCtrls[index].value.split(":")[0]:"";
      let isTax = (taxId == TaxClassId.TaxExclusive) || (taxId == TaxClassId.TaxInclusive);
      if (this.billedAtCtrl.value != undefined) {
        let rate = !isTax ? 0 : this.getTaxRate(DateUtil.convertYYYYMMDD(this.billedAtCtrl));
        this.detailTaxAmountCtrls[index].setValue(
          pipe.transform(
            this.calculateRounding(this.taxCalculationRoundingType, NumberUtil.ParseInt(this.detailBillingAmountCtrls[index].value) / (1 + rate) * rate, this.precision)
          ));
      }
      else {
        this.detailTaxAmountCtrls[index].setValue(0);
      }
    }
    else {
      this.detailTaxAmountCtrls[index].setValue(0);
    }

    this.detailBillingNoTaxAmountCtrls[index].setValue(
      pipe.transform(
        (NumberUtil.ParseInt(this.detailBillingAmountCtrls[index].value)
          - NumberUtil.ParseInt(this.detailTaxAmountCtrls[index].value))
      )
    );

    this.setSum();
  }

  public nextFocusDetailBillingAmountCtl(eventType: string, index: number) {
    if(index+1<this.billingCount){
      HtmlUtil.nextFocusByName(this.elementRef, 'detailSalesAtCtrl' + (index + 1), eventType);
    }
    else{
      HtmlUtil.nextFocusByName(this.elementRef, 'detailSalesAtCtrl0', eventType);
    }
  }

  public setCurrencyForDetailBillingAmount(eventType: string, index: number) {
    this.onLeaveCurrencyControl(this.detailBillingAmountCtrls[index]);
  }

  public initCurrencyForDetailBillingAmount(index: number) {

    this.currentFocus = FOCUS.AMOUNT;

    this.onFocusCurrencyControl(this.detailBillingAmountCtrls[index]);
  }

  /////////////////////////////////////////////////////////////////////////////

  public detailClear(index: number) {

    //this.billings[index];
    this.billingMemos[index].memo = "";

    this.detailSalesAtCtrls[index].setValue(null);
    this.detailBillingCategoryIdCtrls[index].setValue(null);
    this.detailBillingCategoryCodeCtrls[index].setValue("");
    this.detailTaxClassIdCtrls[index].setValue("");
    this.detailDebitAccountTitleIdCtrls[index].setValue("");
    this.detailDebitAccountTitleIds[index].setValue(null);
    this.detailMemoCtrls[index].setValue("");
    this.detailNoteFlagCtrls[index].setValue("");
    this.detailBillingIdCtrls[index].setValue("");
    this.detailContractNumberCtrls[index].setValue("");
    this.detailQuantityCtrls[index].setValue("");
    this.detailUnitSymbolCtrls[index].setValue("");
    this.detailUnitPriceCtrls[index].setValue("");
    this.detailBillingAmountCtrls[index].setValue("");
    this.detailBillingNoTaxAmountCtrls[index].setValue("");
    this.detailTaxAmountCtrls[index].setValue("");

    //契約番号を初期化
    this.notInputContractNumFlags[index] = false;
    this.setNotInput(this.notInputContractNumFlags[index], index);

    // 詳細入力モードの初期化
    this.componentDetailStatus=COMPONENT_DETAIL_STATUS_TYPE.CREATE;
    this.detailEditLineNo=-1;
    this.setSalesAt();
  }

  public setBillingAmounFromQuantityWithUnitPrice(index: number) {
    if (this.rdoCalAmountCtrl.value == "1") return;

    let quantity: number = 0;
    let unitPrice: number = 0;
    if (!(this.detailQuantityCtrls[index].value == undefined || StringUtil.IsNullOrEmpty(this.detailQuantityCtrls[index].value))) {
      quantity = parseFloat(this.detailQuantityCtrls[index].value.toString().replace(",", ""));
    }

    if (!(this.detailUnitPriceCtrls[index].value == undefined || StringUtil.IsNullOrEmpty(this.detailUnitPriceCtrls[index].value))) {
      unitPrice = parseFloat(this.detailUnitPriceCtrls[index].value.toString().replace(",", ""));
    }

    if (quantity == 0 && unitPrice == 0) return;

    let amount = this.calculateRounding(this.priceCalculationRoundingType, parseFloat((quantity * unitPrice).toFixed(2)), this.precision);

    if (this.detailTaxClassIdCtrls[index].value == undefined ||
      (parseInt(this.detailTaxClassIdCtrls[index].value.split(":")[0]) != TaxClassId.TaxExclusive)) {
      this.detailBillingNoTaxAmountCtrls[index].setValue("");
      this.detailTaxAmountCtrls[index].setValue("");
      this.detailBillingAmountCtrls[index].setValue(amount);
      return;
    }

    if (quantity == 0 || unitPrice == 0 || amount == 0) {
      this.detailBillingNoTaxAmountCtrls[index].setValue("");
      this.detailTaxAmountCtrls[index].setValue("");
      this.detailBillingAmountCtrls[index].setValue("");
      return;
    }

    let tax = this.calculateExclusiveTax(amount);
    this.detailBillingNoTaxAmountCtrls[index].setValue(amount);
    this.detailTaxAmountCtrls[index].setValue(tax);
    let billingAmount = amount + tax;
    const maxAmount = 99999999999.99999;
    if (billingAmount <= maxAmount) {
      this.detailBillingAmountCtrls[index].setValue(billingAmount);
    }

  }

  public setRdoCalAmount(){
    let localstorageItem = new LocalStorageItem();
    localstorageItem.key = RangeSearchKey.PC0201_CAL_AMOUNT;
    localstorageItem.value = this.rdoCalAmountCtrl.value;
    this.localStorageManageService.set(localstorageItem);


  }

  public setRadio() {
    let rdoCalAmount = this.localStorageManageService.get(RangeSearchKey.PC0201_CAL_AMOUNT);

    if (rdoCalAmount != null) {
      this.rdoCalAmountCtrl.setValue(rdoCalAmount.value);
    }
  }


  public setSum() {

    let sumNoTaxAmount: number = 0;
    let sumTaxAmountCtls: number = 0;
    let sumAmount: number = 0;
    let tmpNumber: number = 0;

    for (let index = 0; index < this.detailDispBillings.length; index++) {

      sumNoTaxAmount = sumNoTaxAmount + NumberUtil.ParseInt(this.detailDispBillings[index].billingNoTaxAmount);
      sumTaxAmountCtls = sumTaxAmountCtls + NumberUtil.ParseInt(this.detailDispBillings[index].taxAmount);
      sumAmount = sumAmount + NumberUtil.ParseInt(this.detailDispBillings[index].billingAmount);
    }

    this.sumBillingNoTaxAmountCtls.setValue(sumNoTaxAmount);
    this.sumTaxAmountCtls.setValue(sumTaxAmountCtls);
    this.sumBillingAmountCtls.setValue(sumAmount);

  }


  public clear() {
    this.MyFormGroup.reset();

    for (let i = 0; i < this.billingCount; i++) {
      this.billings[i];
      this.billingMemos[i].memo = "";
    }

    this.rdoCalAmountCtrl.setValue("0");

    this.customer = null;
    this.currentBilling = null;
    this.currentBillingsByInputId = null;

    this.sumBillingNoTaxAmountCtls.setValue(0);
    this.sumTaxAmountCtls.setValue(0);    
    this.sumBillingAmountCtls.setValue(0);

    this.componentDetailStatus=COMPONENT_DETAIL_STATUS_TYPE.CREATE;
    this.detailEditLineNo=-1;

    this.detailDispBillings = new Array<BillingDisplay>();

    this.setRadio();

    HtmlUtil.nextFocusByName(this.elementRef, 'billedAtCtrl', EVENT_TYPE.NONE);
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

      case BUTTON_ACTION.REGISTRY:
        this.registry();
        break;

      case BUTTON_ACTION.DELETE:
        this.delete();
        break;

      default:
        console.log('buttonAction Error.');
        break;
    }
    
  }



  public registry() {

    if (this.ComponentStatus == COMPONENT_STATUS_TYPE.CREATE || this.ComponentStatus == COMPONENT_STATUS_TYPE.UPDATE) {
      if (!this.validateInput()) {
        return;
      }

      if(this.detailDispBillings.length==0){
        this.processCustomResult = this.processResultService.processAtWarning(
          this.processCustomResult, MSG_WNG.INPUT_GRID_ONE_REQUIRED, this.partsResultMessageComponent);
          return;
      }
    }
    else{
      return;
    }

    let billings = this.getBillingInputDataForRegistry();

    // 動作中のコンポーネントを開く
    let componentFactory = this.componentFactoryResolver.resolveComponentFactory(ModalRouterProgressComponent);
    let modalRouterProgressComponentRef = this.viewContainerRef.createComponent(componentFactory);

    if (this.ComponentStatus == COMPONENT_STATUS_TYPE.UPDATE) {
      let option = new BillingSearch();
      option.companyId = this.userInfoService.Company.id;
      if (StringUtil.IsNullOrEmpty(this.paramInputId)) {
        option.billingId = parseInt(this.paramId);
      }
      else {
        option.billingInputId = parseInt(this.paramInputId);
      }

      this.billingService.GetItems(option)
        .subscribe(response => {
          if (response != PROCESS_RESULT_RESULT_TYPE.FAILURE && response.length > 0) {
            let billing = response[0] as Billing;
            if (billing.billingInputId != undefined) {
              this.currentBillingsByInputId = response[0];
              // if(!this.canUpdate(this.currentBillingsByInputId)){
              //   return;
              // }
            }
            else {
              this.currentBilling = billing;
              // if(!this.canUpdate(this.currentBilling[0])){
              //   return;
              // }
            }

            this.billingService.Save(billings)
              .subscribe(response => {
                this.processCustomResult = this.processResultService.processAtSave(
                  this.processCustomResult, response, false, this.partsResultMessageComponent);
                if (response != PROCESS_RESULT_RESULT_TYPE.FAILURE) {
                  if (this.paramFrom == ComponentId.PC0301) {
                    this.router.navigate(['main/PC0301', { "process": "back" }]);
                  }
                  else {
                    let billingResult: BillingResult = response;
                    this.ComponentStatus = COMPONENT_STATUS_TYPE.CREATE;
                    this.clear();                    
                  }
                }
                modalRouterProgressComponentRef.destroy();
              });

          }
          else{
            this.processCustomResult = this.processResultService.processAtFailure(
              this.processCustomResult, MSG_ERR.UPDATE_ERROR, this.partsResultMessageComponent);
            modalRouterProgressComponentRef.destroy();
          }
        });
    }
    else if (this.ComponentStatus == COMPONENT_STATUS_TYPE.CREATE) {
      this.billingService.Save(billings)
        .subscribe(
          response => {
            this.processCustomResult = this.processResultService.processAtSave(
              this.processCustomResult, response, true, this.partsResultMessageComponent);

            if (response != PROCESS_RESULT_RESULT_TYPE.FAILURE) {
              let billingInputsResult: BillingResult = response;
              this.ComponentStatus = COMPONENT_STATUS_TYPE.CREATE;
              this.clear();
            }
            modalRouterProgressComponentRef.destroy();

        });
    }

  }

  public canUpdate(billing: Billing): boolean {

    let bRtn = 
      !(
          billing.assignmentFlag != 0 // 未消込以外
      ||  !StringUtil.IsNullOrEmpty(billing.outputAt) // 仕訳出力済み
      ||  billing.inputType == BILL_INPUT_TYPE_DICTIONARY[3].id  // 期日入金予定
      ||  billing.resultCode == 0  // 口座振替
      ||  !StringUtil.IsNullOrEmpty(billing.deleteAt)  // 削除
      ||  !StringUtil.IsNullOrEmpty(billing.billingInputPublishAt)); // 請求書発行済み

    return bRtn;
  }


  public getBillingInputDataForRegistry(): Billing[] {

    let currencyId: number;
    if (this.userInfoService.ApplicationControl.useForeignCurrency == 1) {
      currencyId = this.currencyResult.currencies.find(x => x.code == this.currencyCodeCtrl.value).id;
    }
    else {
      currencyId = this.currencyResult.currencies.find(x => x.code == "JPY").id;
    }

    let billings = new Array<Billing>();
    for (let index = 0; index < this.detailDispBillings.length; index++) {

      let billing = new Billing();

      billing.companyId = this.userInfoService.Company.id;
      billing.invoiceCode = StringUtil.IsNullOrEmpty(this.invoiceCodeCtrl.value) ? "" : this.invoiceCodeCtrl.value;
      billing.currencyId = currencyId;

      billing.billingInputId = StringUtil.IsNullOrEmpty(this.billingIdCtrl.value) ? null : parseInt(this.billingIdCtrl.value);

      billing.customerId = this.customer.id;
      billing.customerCode = this.customerCodeCtrl.value;
      billing.billedAt = DateUtil.ConvertFromDatepicker(this.billedAtCtrl);
      billing.closingAt = DateUtil.ConvertFromDatepicker(this.closingAtCtrl);
      billing.dueAt = DateUtil.ConvertFromDatepicker(this.dueAtCtrl);
      billing.collectCategoryId = this.collectCategoriesResult.categories.find(x => x.code == this.collectCategoryCodeCtrl.value).id;
      billing.staffId = this.staffId;
      billing.departmentId = this.departmentId;
      billing.destinationId = this.destinationId == undefined ? null : this.destinationId;
      billing.createBy = this.userInfoService.LoginUser.id;
      billing.updateBy = this.userInfoService.LoginUser.id;

      billing.billingDiscountId = null;   //  いる？要確認
      billing.contractNumber = StringUtil.IsNullOrEmpty(this.detailDispBillings[index].contractNumber) ? null : this.detailDispBillings[index].contractNumber;
      billing.billingDivisionContract = 0;  //  長期前受契約
      billing.id = StringUtil.IsNullOrEmpty(this.detailDispBillings[index].billingId) ? 0 : parseInt(this.detailDispBillings[index].billingId);
      billing.salesAt = this.detailDispBillings[index].salesAt;
      billing.billingCategoryId = parseInt(this.detailDispBillings[index].billingCategoryId);

      if (this.notInputContractNumFlags[index]) {
        billing.useLongTermAdvancedReceived = this.billingCategoryResult.categories.find(x => x.id == billing.billingCategoryId).useLongTermAdvanceReceived;
      }
      else {
        billing.useLongTermAdvancedReceived = 0;
      }
      
      billing.taxClassId = StringUtil.IsNullOrEmpty(this.detailDispBillings[index].taxClassId) ? 0 :
        parseInt(this.detailDispBillings[index].taxClassId.split(":")[0]);

      billing.debitAccountTitleId = this.detailDispBillings[index].debitAccountTitle;

      billing.quantity = StringUtil.IsNullOrEmpty(this.detailDispBillings[index].quantity) ? null :
        parseFloat(this.detailDispBillings[index].quantity);

      billing.unitSymbol = StringUtil.IsNullOrEmpty(this.detailDispBillings[index].unitSymbol) ? null :
        this.detailDispBillings[index].unitSymbol;

      billing.unitPrice = StringUtil.IsNullOrEmpty(this.detailDispBillings[index].unitPrice) ? null :
        parseFloat(this.detailDispBillings[index].unitPrice.toString().replace(",", ""));

      billing.price = StringUtil.IsNullOrEmpty(this.detailDispBillings[index].billingNoTaxAmount) ? 0 :
        parseFloat(this.detailDispBillings[index].billingNoTaxAmount.toString().replace(",", ""));

      billing.taxAmount = StringUtil.IsNullOrEmpty(this.detailDispBillings[index].taxAmount) ? 0 :
        parseFloat(this.detailDispBillings[index].taxAmount.toString().replace(",", ""));

      billing.billingAmount = StringUtil.IsNullOrEmpty(this.detailDispBillings[index].billingAmount) ? 0 :
        parseFloat(this.detailDispBillings[index].billingAmount.toString().replace(",", ""));

      billing.note1 = StringUtil.IsNullOrEmpty(this.detailDispBillings[index].note1) ? "" : this.detailDispBillings[index].note1;
      billing.note2 = StringUtil.IsNullOrEmpty(this.detailDispBillings[index].note2) ? "" : this.detailDispBillings[index].note2;
      billing.note3 = StringUtil.IsNullOrEmpty(this.detailDispBillings[index].note3) ? "" : this.detailDispBillings[index].note3;
      billing.note4 = StringUtil.IsNullOrEmpty(this.detailDispBillings[index].note4) ? "" : this.detailDispBillings[index].note4;
      billing.note5 = StringUtil.IsNullOrEmpty(this.detailDispBillings[index].note5) ? "" : this.detailDispBillings[index].note5;
      billing.note6 = StringUtil.IsNullOrEmpty(this.detailDispBillings[index].note6) ? "" : this.detailDispBillings[index].note6;
      billing.note7 = StringUtil.IsNullOrEmpty(this.detailDispBillings[index].note7) ? "" : this.detailDispBillings[index].note7;
      billing.note8 = StringUtil.IsNullOrEmpty(this.detailDispBillings[index].note8) ? "" : this.detailDispBillings[index].note8;
      billing.memo = StringUtil.IsNullOrEmpty(this.detailDispBillings[index].memo) ? "" : this.detailDispBillings[index].memo;
      billing.inputType = BILL_INPUT_TYPE_DICTIONARY[2].id;

      billing.creditAccountTitleId = null;
      billing.accountTransferLogId = 0;
      billing.approved = 0;
      billing.assignmentFlag = 0;
      billing.useDiscount = false;

      billings.push(billing);

    }

    return billings;
  }  

  public getBillingDisplayDataForAdd(): BillingDisplay[] {

    let billings = new Array<BillingDisplay>();
    for (let index = 0; index < this.billingCount; index++) {
      if (!this.validateGridEmptyByRow(index)) {
        let billingDisplay = new BillingDisplay();

        billingDisplay.salesAt = DateUtil.ConvertFromDatepicker(this.detailSalesAtCtrls[index]);
        billingDisplay.billingCategory = this.detailBillingCategoryCodeCtrls[index].value;
        billingDisplay.taxClass = this.detailTaxClassIdCtrls[index].value;
        billingDisplay.debitAccountTitle = this.detailDebitAccountTitleIds[index].value;


        // public memo: string;                // 備考
        // public note1:string;                // メモフラグ

        billingDisplay.note1 = this.detailMemoCtrls[index].value;
        billingDisplay.note2 = this.billings[index].note2;
        billingDisplay.note3 = this.billings[index].note3;
        billingDisplay.note4 = this.billings[index].note4;
        billingDisplay.note5 = this.billings[index].note5;
        billingDisplay.note6 = this.billings[index].note6;
        billingDisplay.note7 = this.billings[index].note7;
        billingDisplay.note8 = this.billings[index].note8;
        billingDisplay.memo = this.billingMemos[index].memo;        

        billingDisplay.billingId = this.detailBillingIdCtrls[index].value;   // 請求ID

        billingDisplay.contractNumber = this.detailContractNumberCtrls[index].value;  // 契約番号

        billingDisplay.quantity = this.detailQuantityCtrls[index].value;  // 数量

        billingDisplay.unitSymbol = this.detailUnitSymbolCtrls[index].value;  // 単位

        billingDisplay.unitPrice = this.detailUnitPriceCtrls[index].value;  // 単価

        billingDisplay.billingNoTaxAmount = this.detailBillingNoTaxAmountCtrls[index].value;  // 請求額(抜)

        billingDisplay.taxAmount = this.detailTaxAmountCtrls[index].value;  // 消費税

        billingDisplay.billingAmount = this.detailBillingAmountCtrls[index].value;     // 請求額     

        billingDisplay.billingCategoryId = this.detailBillingCategoryIdCtrls[index].value; // 請求区分ID
        billingDisplay.billingCategoryCode = this.detailBillingCategoryCodeCtrls[index].value;  // 請求区分
        billingDisplay.taxClassId=this.detailTaxClassIdCtrls[index].value;  // 税区分
        billingDisplay.debitAccountTitleId=this.detailDebitAccountTitleIdCtrls[index].value;  // 債権科目Id

        billings.push(billingDisplay);
      }
    }

    return billings;
  }


  public setBillingFromDisplay(lineNo:number) {

    let billingDisplay = this.detailDispBillings[lineNo];

    let index=0;

    // 売上日
    let year = billingDisplay.salesAt.split('/')[0];
    let month = billingDisplay.salesAt.split('/')[1];
    let date = billingDisplay.salesAt.split('/')[2];
    this.detailSalesAtCtrls[index].setValue(new NgbDate(parseInt(year), parseInt(month) , parseInt(date)));

    this.detailBillingCategoryCodeCtrls[index].setValue(billingDisplay.billingCategory);  // 請求区分
    this.detailBillingCategoryIdCtrls[index].setValue(billingDisplay.billingCategoryId); // 請求区分ID

    this.detailTaxClassIdCtrls[index].setValue(billingDisplay.taxClassId);  // 税区分

    this.detailDebitAccountTitleIdCtrls[index].setValue(billingDisplay.debitAccountTitleId);  //科目

    this.detailMemoCtrls[index].setValue(billingDisplay.note1); // 備考

    this.billings[index].note2 = billingDisplay.note2;  // メモ
    this.billings[index].note3 = billingDisplay.note3;
    this.billings[index].note4 = billingDisplay.note4;
    this.billings[index].note5 = billingDisplay.note5;
    this.billings[index].note6 = billingDisplay.note6;
    this.billings[index].note7 = billingDisplay.note7;
    this.billings[index].note8 = billingDisplay.note8;
    this.billingMemos[index].memo = billingDisplay.memo;

    this.detailBillingIdCtrls[index].setValue(billingDisplay.billingId);  // 請求ID
    this.detailContractNumberCtrls[index].setValue(billingDisplay.contractNumber); // 契約番号
    this.detailQuantityCtrls[index].setValue(billingDisplay.quantity);  // 数量
    this.detailUnitSymbolCtrls[index].setValue(billingDisplay.unitSymbol); // 単位
    this.detailUnitPriceCtrls[index].setValue(billingDisplay.unitPrice); // 単価

    this.detailBillingNoTaxAmountCtrls[index].setValue(billingDisplay.billingNoTaxAmount); // 請求額(抜)
    this.detailTaxAmountCtrls[index].setValue(billingDisplay.taxAmount); // 消費税
    this.detailBillingAmountCtrls[index].setValue(billingDisplay.billingAmount); // 請求額

  }


  public referenceNew() {
    this.router.navigate(["/main/PC0301",{"process":"refCreate"}]);
  }

  public detailCopy() {
    let tmpLineData: string;
    let dataIndex: number = -1;
    for (let i = this.billingCount - 1; i >= 0; i--) {

      tmpLineData = "";

      if (
        (!StringUtil.IsNullOrEmpty(this.detailBillingCategoryCodeCtrls[i].value))
        || (!StringUtil.IsNullOrEmpty(this.detailTaxClassIdCtrls[i].value))
        || (!StringUtil.IsNullOrEmpty(this.detailDebitAccountTitleIdCtrls[i].value))
        || (!StringUtil.IsNullOrEmpty(this.detailMemoCtrls[i].value))
        || (!StringUtil.IsNullOrEmpty(this.detailNoteFlagCtrls[i].value))
        || (!StringUtil.IsNullOrEmpty(this.detailBillingIdCtrls[i].value))
        || (!StringUtil.IsNullOrEmpty(this.detailContractNumberCtrls[i].value))
        || (!StringUtil.IsNullOrEmpty(this.detailQuantityCtrls[i].value))
        || (!StringUtil.IsNullOrEmpty(this.detailUnitSymbolCtrls[i].value))
        || (!StringUtil.IsNullOrEmpty(this.detailUnitPriceCtrls[i].value))
        || (!StringUtil.IsNullOrEmpty(this.detailBillingNoTaxAmountCtrls[i].value))
        || (!StringUtil.IsNullOrEmpty(this.detailTaxAmountCtrls[i].value))
        || (!StringUtil.IsNullOrEmpty(this.detailBillingAmountCtrls[i].value))
      ) {
        dataIndex = i;
        break;
      }
    }

    if (dataIndex == -1) {
      this.processCustomResult = this.processResultService.processAtWarning(
        this.processCustomResult, MSG_WNG.INPUT_REQUIRED.replace(MSG_ITEM_NUM.FIRST, '明細行'),
        this.partsResultMessageComponent);
    }
    else if (dataIndex == (this.billingCount - 1)) {
      this.processCustomResult = this.processResultService.processAtWarning(
        this.processCustomResult, MSG_WNG.ALL_DETAIL_LINES_FILLED, this.partsResultMessageComponent);
    }
    else {
      //this.detailSalesAtCtrls[dataIndex + 1].setValue(this.detailSalesAtCtrls[dataIndex].value);
      this.detailBillingCategoryCodeCtrls[dataIndex + 1].setValue(this.detailBillingCategoryCodeCtrls[dataIndex].value);
      this.detailBillingCategoryIdCtrls[dataIndex + 1].setValue(this.detailBillingCategoryIdCtrls[dataIndex].value);
      this.detailTaxClassIdCtrls[dataIndex + 1].setValue(this.detailTaxClassIdCtrls[dataIndex].value);
      this.detailDebitAccountTitleIdCtrls[dataIndex + 1].setValue(this.detailDebitAccountTitleIdCtrls[dataIndex].value);
      this.detailDebitAccountTitleIds[dataIndex + 1].setValue(this.detailDebitAccountTitleIds[dataIndex].value);
      this.detailMemoCtrls[dataIndex + 1].setValue(this.detailMemoCtrls[dataIndex].value);
      this.detailNoteFlagCtrls[dataIndex + 1].setValue(this.detailNoteFlagCtrls[dataIndex].value);
      this.detailContractNumberCtrls[dataIndex + 1].setValue(this.detailContractNumberCtrls[dataIndex].value);
      this.detailQuantityCtrls[dataIndex + 1].setValue(this.detailQuantityCtrls[dataIndex].value);
      this.detailUnitSymbolCtrls[dataIndex + 1].setValue(this.detailUnitSymbolCtrls[dataIndex].value);
      this.detailUnitPriceCtrls[dataIndex + 1].setValue(this.detailUnitPriceCtrls[dataIndex].value);
      this.detailBillingNoTaxAmountCtrls[dataIndex + 1].setValue(this.detailBillingNoTaxAmountCtrls[dataIndex].value);
      this.detailTaxAmountCtrls[dataIndex + 1].setValue(this.detailTaxAmountCtrls[dataIndex].value);
      this.detailBillingAmountCtrls[dataIndex + 1].setValue(this.detailBillingAmountCtrls[dataIndex].value);
    }
  }

  public delete() {

    let ids = new Array<number>();
    for (let i = 0; i < this.billingCount; i++) {
      if (this.detailBillingIdCtrls[i].value != undefined) {
        ids.push(Number(this.detailBillingIdCtrls[i].value));
      }
    }

    let billingDeleteSource = new BillingDeleteSource();
    billingDeleteSource.companyId = this.userInfoService.Company.id;
    billingDeleteSource.ids = ids;

    // 削除処理
    let componentFactory = this.componentFactoryResolver.resolveComponentFactory(ModalConfirmComponent);
    let componentRef = this.viewContainerRef.createComponent(componentFactory);
    componentRef.instance.ActionName = "削除"
    componentRef.instance.Closing.subscribe(() => {
      if (componentRef.instance.ModalStatus == MODAL_STATUS_TYPE.OK) {

        // 動作中のコンポーネントを開く
        let componentFactory = this.componentFactoryResolver.resolveComponentFactory(ModalRouterProgressComponent);
        let modalRouterProgressComponentRef = this.viewContainerRef.createComponent(componentFactory);
        modalRouterProgressComponentRef.instance.Closing.subscribe(() => {
          modalRouterProgressComponentRef.destroy();
        });

        this.billingService.Delete(billingDeleteSource)
          .subscribe(result => {
            this.processCustomResult = 
              this.processResultService.processAtDelete(
                this.processCustomResult, result, this.partsResultMessageComponent);
            if (result != PROCESS_RESULT_RESULT_TYPE.FAILURE) {
              this.router.navigate(['main/PC0301', { "process": "back" }]);
            }
            modalRouterProgressComponentRef.destroy();

          });
      }
      else{
        this.processCustomResult = 
              this.processResultService.processAtWarning(
                this.processCustomResult, MSG_WNG.CANCEL_PROCESS.replace("{0}","削除"),
                this.partsResultMessageComponent);
      }
      componentRef.destroy();
    });



  }

  public back() {

    this.router.navigate(['main/PC0301', { "process": "back" }]);
  }

  /*
    入力内容のチェック
      ヘッダー部
  */
  public validateInput(): boolean {

    //請求日不要
    //請求締日不要
    //予定日不要
    //得意先コード不要
    //請求部門コード不要
    //担当者コード不要

    //得意先マスターの回収区分が約定かを確認する
    if (this.collectCategoryCodeCtrl.value == "00") {
      let collectCategoryId = this.collectCategoriesResult.categories.find(x => x.code == this.collectCategoryCodeCtrl.value).id;
      if (this.customer != undefined && this.customer.collectCategoryId != collectCategoryId) {
        this.processCustomResult = this.processResultService.processAtWarning(
          this.processCustomResult, 
          MSG_WNG.NOT_CONTRACT_COLLECT_CATEGORY_CUSTOMER.replace(MSG_ITEM_NUM.FIRST, this.customerCodeCtrl.value),
          this.partsResultMessageComponent);
        HtmlUtil.nextFocusByName(this.elementRef, 'customerCodeCtrl', EVENT_TYPE.NONE);
        return false;
      }
    }

    //売上日>請求日締日
    let msg = null;
    if (!DateUtil.isValidRange(this.billedAtCtrl, this.closingAtCtrl)) {
      msg = MSG_WNG.INPUT_VALID_DATE_FOR_PARAMETERS.replace(MSG_ITEM_NUM.FIRST, '請求締日')
                                                    .replace(MSG_ITEM_NUM.SECOND, '請求日');
      this.processCustomResult = this.processResultService.processAtWarning(
        this.processCustomResult, msg, this.partsResultMessageComponent);
      HtmlUtil.nextFocusByName(this.elementRef, 'billedAtCtrl', EVENT_TYPE.NONE);
      return false;
    }
    //請求締日>予定日
    if (!DateUtil.isValidRange(this.closingAtCtrl, this.dueAtCtrl)) {
      msg = MSG_WNG.INPUT_VALID_DATE_FOR_PARAMETERS.replace(MSG_ITEM_NUM.FIRST, '予定日')
                                                    .replace(MSG_ITEM_NUM.SECOND, '請求締日');
      this.processCustomResult = this.processResultService.processAtWarning(
        this.processCustomResult, msg, this.partsResultMessageComponent);
      HtmlUtil.nextFocusByName(this.elementRef, 'closingAtCtrl', EVENT_TYPE.NONE);
      return false;
    }
    //消費税合計と請求書単位消費税合計が一致していません。
    //if(this.collationSettingResult.collationSetting.calculateTaxByInputId == 1)

    return true;
  }

  /*
    入力内容のチェック
      明細部
  */  
  public setGridInput(method:string,clearFlag:boolean): boolean {
    let bRtn: boolean = false;

    let allEmpty = false;
    for (let index = 0; index < this.billingCount; index++) {
      if (this.validateGridEmptyByRow(index)) {
        allEmpty = true;
      }
      else {
        allEmpty = false;
        break;
      }

      if (allEmpty) {
        this.processCustomResult = this.processResultService.processAtWarning(
          this.processCustomResult, MSG_WNG.INPUT_GRID_REQUIRED, this.partsResultMessageComponent);
        HtmlUtil.nextFocusByName(this.elementRef, 'detailSalesAtCtrl'+index, EVENT_TYPE.NONE);
        return false;
      }
    }


    let exitRes = new Array<any>();

    // 請求区分、税区、科目の存在チェック
    let categoryCode = this.detailBillingCategoryCodeCtrls[0].value;
    categoryCode = categoryCode.split(":")[0];
    exitRes.push(this.categoryService.GetItems(CategoryType.Billing,categoryCode));
    
    //////////////////////////////////////////////////////
    let taxId = this.detailTaxClassIdCtrls[0].value;
    taxId = taxId.split(":")[0];
    exitRes.push(this.taxClassService.GetItems(taxId));

    //////////////////////////////////////////////////////
    let accountTitleRes = null;
    let accountTitleCode = this.detailDebitAccountTitleIdCtrls[0].value;
    accountTitleCode = StringUtil.IsNullOrEmpty(accountTitleCode)?"":accountTitleCode.split(":")[0];
    if (!StringUtil.IsNullOrEmpty(accountTitleCode)) {
      exitRes.push(this.accountTitleService.Get(accountTitleCode));
    }

    forkJoin(
      exitRes
    )
    .subscribe(
      responseList => {
        // 請求区分の存在チェック
        if(responseList[0]== PROCESS_RESULT_RESULT_TYPE.FAILURE || responseList[0].length!=1){
          // 請求区分エラー
          this.processCustomResult = this.processResultService.processAtWarning(
            this.processCustomResult, MSG_WNG.INPUT_NOT_EXIST.replace(MSG_ITEM_NUM.FIRST, '請求区分'),
            this.partsResultMessageComponent);
            HtmlUtil.nextFocusByName(this.elementRef, 'detailBillingCategoryCodeCtrl0', EVENT_TYPE.NONE);
          return false;
        }

        // 税区分の存在チェック
        if(responseList[1]== PROCESS_RESULT_RESULT_TYPE.FAILURE ){
            // 税区分エラー
            this.processCustomResult = this.processResultService.processAtWarning(
              this.processCustomResult, MSG_WNG.INPUT_NOT_EXIST.replace(MSG_ITEM_NUM.FIRST, '税区分'),
              this.partsResultMessageComponent);
              HtmlUtil.nextFocusByName(this.elementRef, 'detailTaxClassIdCtrl0', EVENT_TYPE.NONE);
            return false;
        }
        else{

          let taxClasses = responseList[1];

          let result=false;

          taxClasses.forEach(element => {
            if (element.id==taxId) result=true;
          });

          if(result==false){
            // 税区分エラー
            this.processCustomResult = this.processResultService.processAtWarning(
              this.processCustomResult, MSG_WNG.INPUT_NOT_EXIST.replace(MSG_ITEM_NUM.FIRST, '税区分'),
              this.partsResultMessageComponent);
              HtmlUtil.nextFocusByName(this.elementRef, 'detailTaxClassIdCtrl0', EVENT_TYPE.NONE);
            return false;
          }
        }

        // 勘定科目の存在チェック
        if( !StringUtil.IsNullOrEmpty(accountTitleCode) ){
          if(responseList[2]== PROCESS_RESULT_RESULT_TYPE.FAILURE || responseList[2].length!=1){
            // 勘定科目エラー
            this.processCustomResult = this.processResultService.processAtWarning(
              this.processCustomResult, MSG_WNG.INPUT_NOT_EXIST.replace(MSG_ITEM_NUM.FIRST, '勘定科目'),
              this.partsResultMessageComponent);
              HtmlUtil.nextFocusByName(this.elementRef, 'detailDebitAccountTitleIdCtrl0', EVENT_TYPE.NONE);
            return false;
          }        
        }

        for (let index = 0; index < this.billingCount; index++) {
          if (!this.validateGridEmptyByRow(index)) {
            //売上日未入力は明細で確認
            if (StringUtil.IsNullOrEmpty(this.detailSalesAtCtrls[index].value)){
              this.processCustomResult = this.processResultService.processAtWarning(
                this.processCustomResult, MSG_WNG.INPUT_REQUIRED.replace(MSG_ITEM_NUM.FIRST, '売上日(明細)'),
                this.partsResultMessageComponent);
                HtmlUtil.nextFocusByName(this.elementRef, 'detailSalesAtCtrl'+index, EVENT_TYPE.NONE);
              return false;
            }
            if (StringUtil.IsNullOrEmpty(this.detailBillingCategoryCodeCtrls[index].value)) {
              this.processCustomResult = this.processResultService.processAtWarning(
                this.processCustomResult, MSG_WNG.INPUT_REQUIRED.replace(MSG_ITEM_NUM.FIRST, '請求区分(明細)'),
                this.partsResultMessageComponent);
                HtmlUtil.nextFocusByName(this.elementRef, 'detailBillingCategoryCodeCtrl'+index, EVENT_TYPE.NONE);
              return false;
            }
            if (StringUtil.IsNullOrEmpty(this.detailTaxClassIdCtrls[index].value)) {
              this.processCustomResult = this.processResultService.processAtWarning(
                this.processCustomResult, MSG_WNG.INPUT_REQUIRED.replace(MSG_ITEM_NUM.FIRST, '税区(明細)'),
                this.partsResultMessageComponent);
                HtmlUtil.nextFocusByName(this.elementRef, 'detailTaxClassIdCtrl'+index, EVENT_TYPE.NONE);
              return false;
            }
            if (this.notInputContractNumFlags[index] && StringUtil.IsNullOrEmpty(this.detailContractNumberCtrls[index].value)) {
              this.processCustomResult = this.processResultService.processAtWarning(
                this.processCustomResult, MSG_WNG.INPUT_REQUIRED.replace(MSG_ITEM_NUM.FIRST, '契約番号(明細)'),
                this.partsResultMessageComponent);
                HtmlUtil.nextFocusByName(this.elementRef, 'detailContractNumberCtrl'+index, EVENT_TYPE.NONE);
              return false;
            }
            if (StringUtil.IsNullOrEmpty(this.detailBillingAmountCtrls[index].value)) {
              this.processCustomResult = this.processResultService.processAtWarning(
                this.processCustomResult, MSG_WNG.INPUT_REQUIRED.replace(MSG_ITEM_NUM.FIRST, '請求額(明細)'),
                this.partsResultMessageComponent);
                HtmlUtil.nextFocusByName(this.elementRef, 'detailBillingAmountCtrl'+index, EVENT_TYPE.NONE);
              return false;
            }
            if (Number.parseInt(this.detailBillingAmountCtrls[index].value) == 0) {
              this.processCustomResult = this.processResultService.processAtWarning(
                this.processCustomResult, MSG_WNG.INPUT_EXCEPT_ZERO_AMT.replace(MSG_ITEM_NUM.FIRST, '請求額(明細)'),
                this.partsResultMessageComponent);
                HtmlUtil.nextFocusByName(this.elementRef, 'detailBillingAmountCtrl'+index, EVENT_TYPE.NONE);
              return false;          
            }
          }
        }
    
        // 明細行の内容を表示部にコピーする
        let billingsDisplay = this.getBillingDisplayDataForAdd();
    
        // 明細行の内容を設定する
        // 新規モードは配列に追加、更新モードは配列の中身を置き換える。
    
        if(method=="detailAdd"){
          this.detailDispBillings.push(billingsDisplay[0]);
        }
        else{
          if(this.componentDetailStatus==COMPONENT_DETAIL_STATUS_TYPE.UPDATE && this.detailEditLineNo>=0){
            this.detailDispBillings[this.detailEditLineNo] = billingsDisplay[0];
          }
        }
    
        this.setSum();
        
        if(clearFlag){
          this.detailClear(0);          
        }

      }
    );






    return true;
  }

  public validateGridEmptyByRow(index: number): boolean {

    if (!StringUtil.IsNullOrEmpty(this.detailSalesAtCtrls[index].value)) { return false; }  //  売上日
    if (!StringUtil.IsNullOrEmpty(this.detailBillingCategoryIdCtrls[index].value)) { return false; }  //  請求区分
    if (!StringUtil.IsNullOrEmpty(this.detailBillingCategoryCodeCtrls[index].value)) { return false; }  //  請求区分
    if (!StringUtil.IsNullOrEmpty(this.detailTaxClassIdCtrls[index].value)) { return false; }  //  税区
    if (!StringUtil.IsNullOrEmpty(this.detailDebitAccountTitleIdCtrls[index].value)) { return false; }  //  債権科目
    if (!StringUtil.IsNullOrEmpty(this.detailMemoCtrls[index].value)) { return false; }  //  備考 
    if (!StringUtil.IsNullOrEmpty(this.detailNoteFlagCtrls[index].value)) { return false; }  //  メモフラグ
    if (!StringUtil.IsNullOrEmpty(this.detailBillingIdCtrls[index].value)) { return false; }  //  請求ID
    if (!StringUtil.IsNullOrEmpty(this.detailContractNumberCtrls[index].value)) { return false; }  //  契約番号
    if (!StringUtil.IsNullOrEmpty(this.detailQuantityCtrls[index].value)) { return false; }  //  数量 
    if (!StringUtil.IsNullOrEmpty(this.detailUnitSymbolCtrls[index].value)) { return false; }  //  単位
    if (!StringUtil.IsNullOrEmpty(this.detailUnitPriceCtrls[index].value)) { return false; }  //  単価
    if (!StringUtil.IsNullOrEmpty(this.detailBillingNoTaxAmountCtrls[index].value)) { return false; }  //  請求額(抜)
    if (!StringUtil.IsNullOrEmpty(this.detailTaxAmountCtrls[index].value)) { return false; }  //  消費税
    if (!StringUtil.IsNullOrEmpty(this.detailBillingAmountCtrls[index].value)) { return false; }  //  請求額    

    return true;
  }

  public getClosingAt(billingDate: string): Date {
    let result = new Date(billingDate);
    let closingDay = this.customer.closingDay;

    if (closingDay == 0) {
      return result;
    }
    if (closingDay < result.getDate()) {
      result.setMonth(result.getMonth() + 1);
    }
    if (closingDay < 28) {
      return new Date(result.getFullYear(), result.getMonth(), closingDay);
    }
    else {
      return new Date(result.getFullYear(), result.getMonth() + 1, 0);
    }

  }

  public getDueAt(closingAt: string): Date {
    let closingDate = new Date(closingAt);

    let offsetMonth = this.customer.collectOffsetMonth;
    let offsetDay = this.customer.collectOffsetDay;
    let closingDay = this.customer.closingDay;

    if (offsetMonth == 0) {
      offsetDay = Math.max(offsetDay, closingDay);
    }

    let dueAt: Date;
    if (this.customer.closingDay == 0) {
      dueAt = new Date(closingDate.setDate(closingDate.getDate() + offsetDay));
    }
    else {
      //closingDate.setMonth(closingDate.getMonth() + offsetMonth);
      if (offsetDay < 28) {
        dueAt = new Date(closingDate.getFullYear(), closingDate.getMonth(), offsetDay);
      }
      else {
        //dueAt = new Date(closingDate.getFullYear(), closingDate.getMonth()+1, 0);
        dueAt = new Date(closingDate.getFullYear(), (closingDate.getMonth() + 1) + offsetMonth, 0);
      }
    }

    return this.getBusinessDay(dueAt);
  }

  public getBusinessDay(tDate: Date): Date {
    if (this.customer.holidayFlag == this.holidayFlagDictionary[0].id) {
      return tDate;
    }

    let vector = this.customer.holidayFlag == this.holidayFlagDictionary[1].id ? -1 : 1;
    while (true) {
      let sat = tDate.getDay() == 6;
      let sun = tDate.getDay() == 0;
      let hol = this.holidaysResult.holidayCalendars.some(x => new Date(x.holiday).toLocaleDateString() == tDate.toLocaleDateString());

      if (!(sat || sun || hol)) {
        break;
      }

      tDate.setDate(tDate.getDate() + vector);
    }
    return tDate;
  }

  public getRoundingType(code: string): RoundingType {
    let value = this.generalSettings.find(x => x.code == code).value;
    if (StringUtil.IsNullOrEmpty(value)) return RoundingType.Floor;
    return parseInt(value) as RoundingType;
  }

  public getTaxRate(target: string): number {
    let taxRate: number;
    let applyDate1 = this.generalSettings.find(x => x.code == "新税率開始年月日").value;
    let applyDate2 = this.generalSettings.find(x => x.code == "新税率開始年月日2").value;
    let applyDate3 = this.generalSettings.find(x => x.code == "新税率開始年月日3").value;

    let key: string;
    if (target.localeCompare(applyDate1) < 0) {
      key = "旧消費税率";
    }
    else if (applyDate1.localeCompare(target) <= 0 && target.localeCompare(applyDate2) < 0) {
      key = "新消費税率";
    }
    else if (applyDate2.localeCompare(target) <= 0 && target.localeCompare(applyDate3) < 0) {
      key = "新消費税率2";
    }
    else if (applyDate3.localeCompare(target) < 0) {
      key = "新消費税率3";
    }
    if (StringUtil.IsNullOrEmpty(key)) return 0;
    taxRate = parseFloat(this.generalSettings.find(x => x.code == key).value) / 10000;

    return taxRate;
  }

  public getColumnName(columnName: string): string {

    if (this.columnNameSettingResult == undefined) return "";

    let columnNameSetting = this.columnNameSettingResult.columnNames.find(x => x.columnName == columnName);
    return StringUtil.IsNullOrEmpty(columnNameSetting.alias) ? columnNameSetting.originalName : columnNameSetting.alias;
  }

  public calculateRounding(type: RoundingType, value: number, precision: number): number {
    let powered: number = value * this.pow10(precision);
    let result: number = 0;

    switch (type) {
      case RoundingType.Floor:
      case RoundingType.Error:
        result = Math.abs(Math.floor(powered)) * Math.sign(powered);
        break;
      case RoundingType.Ceil:
        result = Math.abs(Math.ceil(powered)) * Math.sign(powered);
        break;
      case RoundingType.Round:
        result = Math.round(powered);
        break;
    }
    let val = result * this.pow10(-precision);
    return val;
  }

  public pow10(y: number): number {
    let x: number = 1;
    let value: number = y > 0 ? 10 : 0.1;
    let times: number = Math.abs(y);

    while (times-- > 0) {
      x *= value;
    }
    return x;
  }

  public calculateExclusiveTax(amount: number): number {

    let tax: number = 0;
    if (this.billedAtCtrl.value == undefined || this.billedAtCtrl.value == null) return tax;
    let rate = this.getTaxRate(DateUtil.convertYYYYMMDD(this.billedAtCtrl));
    if (rate == 0) return tax;

    tax = this.calculateRounding(this.taxCalculationRoundingType, amount * rate, this.precision);
    return tax;

  }

  public firstRowButtonFlag(index: number): boolean {
    if (index == 0) return false;
    return index > 0 && this.isFirstRowOnly;
  }

  


  /*
    明細行の追加ボタン
  */
  public detailAdd(lineNo:number,clearFlag:boolean=false){

    this.processResultService.clearProcessCustomMsg(this.processCustomResult);

    // 最大行数のチェック
    if(this.billingMaxCount==this.detailDispBillings.length){
      this.processCustomResult = 
      this.processResultService.processAtWarning(
        this.processCustomResult, MSG_WNG.INPUT_GRID_MAX_COUNT.replace(MSG_ITEM_NUM.FIRST, "" + this.billingMaxCount),
        this.partsResultMessageComponent);
      return;

    }

    this.setGridInput("detailAdd",clearFlag);

  }


  /*
    明細行の更新ボタン
  */
  public detailUpdate(lineNo:number,clearFlag:boolean=false){

    this.processResultService.clearProcessCustomMsg(this.processCustomResult);

    this.setGridInput("detailUpdate",clearFlag);

  }

  /*
    明細行の編集ボタン
  */
  public detailEdit(lineNo:number){

    this.componentDetailStatus = COMPONENT_DETAIL_STATUS_TYPE.UPDATE;
    this.detailEditLineNo=lineNo;


    // 表示部の内容を明細に設定する
    this.setBillingFromDisplay(lineNo);
  }


  /*
    明細行の削除ボタン
  */
  public detailDelete(lineNo:number){

    this.detailDispBillings = this.detailDispBillings.filter((value,index)=>{
      if(index==lineNo){
        return false;
      } 
      return true;
    })

  }



  /*
    明細行のヘッダーの背景色の変更処理
  */
  public currentInputItem:PC0201_INPUT;
  public currentLineNo:number;

  public setFocus(lineNo:number,inputItem:PC0201_INPUT){
    this.currentLineNo=lineNo;
    this.currentInputItem=inputItem;
  }

  public setBlur(lineNo:number,inputItem:PC0201_INPUT){
    this.currentInputItem=PC0201_INPUT.none;
  }

  public getTdColor(inputItem:PC0201_INPUT){
    return this.currentInputItem==inputItem;
  }

  public getTdNoColor(lineNo:number){
    return this.currentLineNo==lineNo;
  }  
}

export enum PC0201_INPUT{
  none=0,                         // なし
  ////////////////////////////////////////
  detailSalesAtCtrl,              // 売上日
  detailBillingCategoryCodeCtrl,  // 請求区分
  detailTaxClassIdCtrl,           // 税区
  detailDebitAccountTitleIdCtrl,  // 債権科目
  detailMemoCtrl,                 // コメント
  // ノートフラグ
  // 備考ボタン
  ////////////////////////////////////////
  detailBillingIdCtrl,           // 請求ID
  detailContractNumberCtrl,      // 契約番号
  detailQuantityCtrl,            // 数量
  detailUnitSymbolCtrl,          // 単位
  detailUnitPriceCtrl,           // 単価
  detailBillingNoTaxAmountCtrl,  // 金額(抜)
  detailTaxAmountCtrl,           // 消費税
  detailBillingAmountCtrl,       // 請求額
  // クリアボタン
}