import { Component, OnInit, ElementRef, ComponentFactoryResolver, ViewContainerRef, AfterContentInit, AfterViewInit, ViewChild } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { ModalMasterComponent } from 'src/app/component/modal/modal-master/modal-master.component';
import { TABLE_INDEX } from 'src/app/common/const/table-name.const';
import { Router, ActivatedRoute, NavigationEnd } from '@angular/router';
import { BaseComponent } from 'src/app/component/common/base/base-component';
import { KEY_CODE, BUTTON_ACTION, EVENT_TYPE } from 'src/app/common/const/event.const';
import { MODAL_STATUS_TYPE, PROCESS_RESULT_RESULT_TYPE } from 'src/app/common/const/status.const';
import { HtmlUtil } from 'src/app/common/util/html.util';
import { StringUtil } from 'src/app/common/util/string-util';
import { FormatterUtil } from 'src/app/common/util/formatter.util';
import { DateUtil } from 'src/app/common/util/date-util';
import { UserInfoService } from 'src/app/service/common/user-info.service';
import { DepartmentMasterService } from 'src/app/service/Master/department-master.service';
import { StaffMasterService } from 'src/app/service/Master/staff-master.service';
import { CustomerMasterService } from 'src/app/service/Master/customer-master.service';
import { CategoryMasterService } from 'src/app/service/Master/category-master.service';
import { CurrencyMasterService } from 'src/app/service/Master/currency-master.service';
import { BillingSearch } from 'src/app/model/billing-search.model';
import { BillingService } from 'src/app/service/billing.service';
import { BillingDueAtModifyResults } from 'src/app/model/billing-due-at-modify-results.model';
import { BillingDueAtModify } from 'src/app/model/billing-due-at-modify.model';
import { CategoryType, CODE_TYPE } from 'src/app/common/const/kbn.const';
import { NgbDateParserFormatter, NgbDate } from '@ng-bootstrap/ng-bootstrap';
import { NgbDateCustomParserFormatter } from 'src/app/common/util/dateformat-util';
import { ProcessResultService } from 'src/app/service/common/process-result.service';
import { PageUtil } from 'src/app/common/util/page-util';
import { MSG_ITEM_NUM, MSG_WNG } from 'src/app/common/const/message.const';
import { LocalStorageItem } from 'src/app/model/custom-model/local-storage-item';
import { RangeSearchKey } from 'src/app/common/const/local-storage-key-const';
import { LocalStorageManageService } from 'src/app/service/common/local-storage-manage.service';
import { ModalRouterProgressComponent } from 'src/app/component/modal/modal-router-progress/modal-router-progress.component';
import { MatAutocompleteTrigger, MatExpansionPanel } from '@angular/material';

@Component({
  selector: 'app-pc0501-billing-due-at-modify',
  templateUrl: './pc0501-billing-due-at-modify.component.html',
  styleUrls: ['./pc0501-billing-due-at-modify.component.css'],
  providers: [
    { provide: NgbDateParserFormatter, useClass: NgbDateCustomParserFormatter }
  ]

})
export class Pc0501BillingDueAtModifyComponent extends BaseComponent implements OnInit,AfterViewInit {

  public panelOpenState: boolean;
  public updateBtnFlg: boolean;

  public billingSearch: BillingSearch;
  public billingDueAtModifyResults: BillingDueAtModifyResults;
  public sourceBillings: BillingDueAtModify[]; //  編集前格納用

  // これをしないとEnum型がテンプレートで見えなかったため
  public TABLE_INDEX: typeof TABLE_INDEX = TABLE_INDEX;

  public billedAtFromCtrl: FormControl;  // 請求日
  public billedAtToCtrl: FormControl;
  public closingAtFromCtrl: FormControl;  // 請求締日
  public closingAtToCtrl: FormControl;
  public dueAtFromCtrl: FormControl;  // 入金予定日
  public dueAtToCtrl: FormControl;
  public invoiceCodeFromCtrl: FormControl;  // 請求書番号
  public invoiceCodeToCtrl: FormControl;
  public invoiceCodeCtrl: FormControl;
  public currencyCodeCtrl: FormControl;  // 通貨コード
  public cbxUseReceiptSectionCtrl: FormControl;  // 入金部門を使用する。
  public departmentCodeFromCtrl: FormControl;  // 請求部門コード
  public departmentNameFromCtrl: FormControl;
  public departmentCodeToCtrl: FormControl;
  public departmentNameToCtrl: FormControl;
  public cbxDepartmentCtrl: FormControl;
  public staffCodeFromCtrl: FormControl;  // 担当者コード
  public staffNameFromCtrl: FormControl;
  public staffCodeToCtrl: FormControl;
  public staffNameToCtrl: FormControl;
  public cbxStaffCtrl: FormControl;
  public customerCodeFromCtrl: FormControl;  // 得意先コード
  public customerNameFromCtrl: FormControl;
  public customerCodeToCtrl: FormControl;
  public customerNameToCtrl: FormControl;
  public cbxCustomerCtrl: FormControl;
  public customerNameCtrl: FormControl;

  public detailDueAtCtrls: FormControl[];  // 入金予定日
  public detailCollectCategoryCodeCtrls: FormControl[];  // 回収区分

  public undefineCtrl: FormControl; // 未定用

  @ViewChild('departmentCodeFromInput', { read: MatAutocompleteTrigger }) departmentCodeFromigger: MatAutocompleteTrigger;
  @ViewChild('departmentCodeToInput', { read: MatAutocompleteTrigger }) departmentCodeToigger: MatAutocompleteTrigger;
  @ViewChild('staffCodeFromInput', { read: MatAutocompleteTrigger }) staffCodeFromTrigger: MatAutocompleteTrigger;
  @ViewChild('staffCodeToInput', { read: MatAutocompleteTrigger }) staffCodeToTrigger: MatAutocompleteTrigger;
  @ViewChild('customerCodeFromInput', { read: MatAutocompleteTrigger }) customerCodeFromTrigger: MatAutocompleteTrigger;
  @ViewChild('customerCodeToInput', { read: MatAutocompleteTrigger }) customerCodeToTrigger: MatAutocompleteTrigger;
  @ViewChild('panel') panel:MatExpansionPanel;

  constructor(
    public router: Router,
    public activatedRoute: ActivatedRoute,
    public elementRef: ElementRef,
    public componentFactoryResolver: ComponentFactoryResolver,
    public viewContainerRef: ViewContainerRef,
    public userInfoService: UserInfoService,
    public billingSearvice: BillingService,
    public departmentService: DepartmentMasterService,
    public staffService: StaffMasterService,
    public customerService: CustomerMasterService,
    public categoryService: CategoryMasterService,
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
    this.setFormatter();
    this.clear();
    this.setAutoComplete();

  }

  ngAfterViewInit(){
    HtmlUtil.nextFocusByName(this.elementRef, 'billedAtFromCtrl', EVENT_TYPE.NONE);
  }

  public setControlInit() {
    this.billedAtFromCtrl = new FormControl("", [Validators.maxLength(10)]);  // 請求日
    this.billedAtToCtrl = new FormControl("", [Validators.maxLength(10)]);
    this.closingAtFromCtrl = new FormControl("", [Validators.maxLength(10)]);  // 請求締日
    this.closingAtToCtrl = new FormControl("", [Validators.maxLength(10)]);
    this.dueAtFromCtrl = new FormControl("", [Validators.maxLength(10)]);  // 入金予定日
    this.dueAtToCtrl = new FormControl("", [Validators.maxLength(10)]);
    this.invoiceCodeFromCtrl = new FormControl("", [Validators.maxLength(20)]);  // 請求書番号
    this.invoiceCodeToCtrl = new FormControl("", [Validators.maxLength(20)]);
    this.invoiceCodeCtrl = new FormControl("", [Validators.maxLength(20)]);
    this.currencyCodeCtrl = new FormControl("", [Validators.maxLength(3)]);  // 通貨コード
    this.cbxUseReceiptSectionCtrl = new FormControl(""); // 入金部門対応マスタを使用
    this.departmentCodeFromCtrl = new FormControl("", [Validators.maxLength(this.userInfoService.ApplicationControl.departmentCodeLength)]);  // 請求部門コード
    this.departmentNameFromCtrl = new FormControl("");
    this.departmentCodeToCtrl = new FormControl("", [Validators.maxLength(this.userInfoService.ApplicationControl.departmentCodeLength)]);
    this.departmentNameToCtrl = new FormControl("");
    this.cbxDepartmentCtrl = new FormControl("");
    this.staffCodeFromCtrl = new FormControl("", [Validators.maxLength(this.userInfoService.ApplicationControl.staffCodeLength)]);  // 担当者コード
    this.staffNameFromCtrl = new FormControl("");
    this.staffCodeToCtrl = new FormControl("", [Validators.maxLength(this.userInfoService.ApplicationControl.staffCodeLength)]);
    this.staffNameToCtrl = new FormControl("");
    this.cbxStaffCtrl = new FormControl("");
    this.customerCodeFromCtrl = new FormControl("", [Validators.maxLength(this.userInfoService.ApplicationControl.customerCodeLength)]);  // 得意先コード
    this.customerNameFromCtrl = new FormControl("");
    this.customerCodeToCtrl = new FormControl("", [Validators.maxLength(this.userInfoService.ApplicationControl.customerCodeLength)]);
    this.customerNameToCtrl = new FormControl("");
    this.cbxCustomerCtrl = new FormControl("");
    this.customerNameCtrl = new FormControl("");

    this.undefineCtrl = new FormControl(""); // 未定用;

  }

  public setValidator() {
    this.MyFormGroup = new FormGroup({
      billedAtFromCtrl: this.billedAtFromCtrl,  // 請求日
      billedAtToCtrl: this.billedAtToCtrl,
      closingAtFromCtrl: this.closingAtFromCtrl,  // 請求締め日
      closingAtToCtrl: this.closingAtToCtrl,
      dueAtFromCtrl: this.dueAtFromCtrl,  // 入金予定日
      dueAtToCtrl: this.dueAtToCtrl,
      invoiceCodeFromCtrl: this.invoiceCodeFromCtrl,  // 請求書番号
      invoiceCodeToCtrl: this.invoiceCodeToCtrl,
      invoiceCodeCtrl: this.invoiceCodeCtrl,
      currencyCodeCtrl: this.currencyCodeCtrl,  // 通貨コード
      cbxUseReceiptSectionCtrl: this.cbxUseReceiptSectionCtrl, // 入金部門対応マスタを使用
      departmentCodeFromCtrl: this.departmentCodeFromCtrl,  // 請求部門コード
      departmentNameFromCtrl: this.departmentNameFromCtrl,
      departmentCodeToCtrl: this.departmentCodeToCtrl,
      departmentNameToCtrl: this.departmentNameToCtrl,
      cbxDepartmentCtrl: this.cbxDepartmentCtrl,
      staffCodeFromCtrl: this.staffCodeFromCtrl,  // 担当者コード
      staffNameFromCtrl: this.staffNameFromCtrl,
      staffCodeToCtrl: this.staffCodeToCtrl,
      staffNameToCtrl: this.staffNameToCtrl,
      cbxStaffCtrl: this.cbxStaffCtrl,
      customerCodeFromCtrl: this.customerCodeFromCtrl,  // 得意先コード
      customerNameFromCtrl: this.customerNameFromCtrl,
      customerCodeToCtrl: this.customerCodeToCtrl,
      customerNameToCtrl: this.customerNameToCtrl,
      cbxCustomerCtrl: this.cbxCustomerCtrl,
      customerNameCtrl: this.customerNameCtrl,

      undefineCtrl: this.undefineCtrl, // 未定用;

    });


  }

  public setFormatter() {

    FormatterUtil.setAlphabetFormatter(this.currencyCodeCtrl); // 通貨コード

    if (this.userInfoService.ApplicationControl.departmentCodeType == CODE_TYPE.NUMBER) {
      FormatterUtil.setNumberFormatter(this.departmentCodeFromCtrl); // 請求部門コード
      FormatterUtil.setNumberFormatter(this.departmentCodeToCtrl); 
    }
    else {
      FormatterUtil.setCodeFormatter(this.departmentCodeFromCtrl);
      FormatterUtil.setCodeFormatter(this.departmentCodeToCtrl); 
    }

    if (this.userInfoService.ApplicationControl.staffCodeType == CODE_TYPE.NUMBER) {
      FormatterUtil.setNumberFormatter(this.staffCodeFromCtrl); // 担当者コード
      FormatterUtil.setNumberFormatter(this.staffCodeToCtrl); 
    }
    else {
      FormatterUtil.setCodeFormatter(this.staffCodeFromCtrl);
      FormatterUtil.setCodeFormatter(this.staffCodeToCtrl); 
    }

    if (this.userInfoService.ApplicationControl.customerCodeType == CODE_TYPE.NUMBER) {
      FormatterUtil.setNumberFormatter(this.customerCodeFromCtrl); // 得意先コード
      FormatterUtil.setNumberFormatter(this.customerCodeToCtrl); 
    }
    else if (this.userInfoService.ApplicationControl.customerCodeType == CODE_TYPE.HANKANA)  {
      FormatterUtil.setCustomerCodeKanaFormatter(this.customerCodeFromCtrl);
      FormatterUtil.setCustomerCodeKanaFormatter(this.customerCodeToCtrl);
    }
    else {
      FormatterUtil.setCustomerCodeAlphaFormatter(this.customerCodeFromCtrl);
      FormatterUtil.setCustomerCodeAlphaFormatter(this.customerCodeToCtrl); 
    }
  }


  public setAutoComplete(){

    // 請求部門
    this.initAutocompleteDepartments(this.departmentCodeFromCtrl,this.departmentService,0);
    this.initAutocompleteDepartments(this.departmentCodeToCtrl,this.departmentService,1);
    // 担当者
    this.initAutocompleteStaffs(this.staffCodeFromCtrl,this.staffService,0);
    this.initAutocompleteStaffs(this.staffCodeToCtrl,this.staffService,1);
    // 得意先
    this.initAutocompleteCustomers(this.customerCodeFromCtrl,this.customerService,0);
    this.initAutocompleteCustomers(this.customerCodeToCtrl,this.customerService,1);

  }

  /*
    table:Master名称
    keyCode:キーコードがある場合はF9のみモーダルを開く
    index:明細行の行No
  */
  public openMasterModal(table: TABLE_INDEX, type: string = null,  index: number = -1) {

    if(this.departmentCodeFromigger)  this.departmentCodeFromigger.closePanel();
    if(this.departmentCodeToigger)  this.departmentCodeToigger.closePanel();
    if(this.staffCodeFromTrigger) this.staffCodeFromTrigger.closePanel();
    if(this.staffCodeToTrigger) this.staffCodeToTrigger.closePanel();
    if(this.customerCodeFromTrigger)  this.customerCodeFromTrigger.closePanel();
    if(this.customerCodeToTrigger)  this.customerCodeToTrigger.closePanel();

    let componentFactory = this.componentFactoryResolver.resolveComponentFactory(ModalMasterComponent);
    let componentRef = this.viewContainerRef.createComponent(componentFactory);

    componentRef.instance.TableIndex = table;

    componentRef.instance.Closing.subscribe(() => {

      if (componentRef.instance.ModalStatus == MODAL_STATUS_TYPE.SELECT) {
        switch (table) {

          case TABLE_INDEX.MASTER_DEPARTMENT:
            {
              if (type === "from") {
                this.departmentCodeFromCtrl.setValue(componentRef.instance.SelectedCode);
                this.departmentNameFromCtrl.setValue(componentRef.instance.SelectedName);

                if (this.cbxDepartmentCtrl.value == true) {
                  this.departmentCodeToCtrl.setValue(componentRef.instance.SelectedCode);
                  this.departmentNameToCtrl.setValue(componentRef.instance.SelectedName);
                }

              }
              else {
                this.departmentCodeToCtrl.setValue(componentRef.instance.SelectedCode);
                this.departmentNameToCtrl.setValue(componentRef.instance.SelectedName);
              }
              break;
            }
          case TABLE_INDEX.MASTER_STAFF:
            {
              if (type === "from") {
                this.staffCodeFromCtrl.setValue(componentRef.instance.SelectedCode);
                this.staffNameFromCtrl.setValue(componentRef.instance.SelectedName);

                if (this.cbxStaffCtrl.value == true) {
                  this.staffCodeToCtrl.setValue(componentRef.instance.SelectedCode);
                  this.staffNameToCtrl.setValue(componentRef.instance.SelectedName);
                }

              }
              else {
                this.staffCodeToCtrl.setValue(componentRef.instance.SelectedCode);
                this.staffNameToCtrl.setValue(componentRef.instance.SelectedName);
              }
              break;
            }
          case TABLE_INDEX.MASTER_CUSTOMER:
            {
              if (type === "from") {
                this.customerCodeFromCtrl.setValue(componentRef.instance.SelectedCode);
                this.customerNameFromCtrl.setValue(componentRef.instance.SelectedName);

                if (this.cbxCustomerCtrl.value == true) {
                  this.customerCodeToCtrl.setValue(componentRef.instance.SelectedCode);
                  this.customerNameToCtrl.setValue(componentRef.instance.SelectedName);
                }

              }
              else {
                this.customerCodeToCtrl.setValue(componentRef.instance.SelectedCode);
                this.customerNameToCtrl.setValue(componentRef.instance.SelectedName);
              }
              break;
            }
          case TABLE_INDEX.MASTER_CURRENCY:
            {
              this.currencyCodeCtrl.setValue(componentRef.instance.SelectedCode);
              break;
            }
          case TABLE_INDEX.MASTER_COLLECT_CATEGORY:
            {
              this.detailCollectCategoryCodeCtrls[index].setValue(
                componentRef.instance.SelectedCode + ":" + componentRef.instance.SelectedName);
              let category = componentRef.instance.SelectedObject;
              this.billingDueAtModifyResults.billings[index].collectCategoryId = category.id;
              break;
            }
        }
      }


      componentRef.destroy();
    });
  }

  public clear() {

    this.panelOpenState = true;
    this.panel.open();
    this.updateBtnFlg = true;
    if (this.processCustomResult != undefined) {
      this.processCustomResult.message = "";
    }

    this.MyFormGroup.reset();

    //this.billingDueAtModifyResults = null;
    HtmlUtil.nextFocusByName(this.elementRef, 'billedAtFromCtrl', EVENT_TYPE.NONE);
   
    this.setRangeCheckbox();
  }


  public setRangeCheckbox() {
    let cbxDepartment = this.localStorageManageService.get(RangeSearchKey.PC0501_DEPARTMENT);
    let cbxStaff = this.localStorageManageService.get(RangeSearchKey.PC0501_STAFF);
    let cbxCustomer = this.localStorageManageService.get(RangeSearchKey.PC0501_CUSTOMER);
    let cbxUseReceiptSection = this.localStorageManageService.get(RangeSearchKey.PC0501_USE_RECEIPT_SECTION);

    if (cbxDepartment != null) {
      this.cbxDepartmentCtrl.setValue(cbxDepartment.value);
    }

    if (cbxStaff != null) {
      this.cbxStaffCtrl.setValue(cbxStaff.value);
    }

    if (cbxCustomer != null) {
      this.cbxCustomerCtrl.setValue(cbxCustomer.value);
    }


    if (cbxUseReceiptSection != null) {
      this.cbxUseReceiptSectionCtrl.setValue(cbxUseReceiptSection.value);
    }

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
        this.update();
        break;
      default:
        console.log('buttonAction Error.');
        break;
    }
  }

  public search() {

    //入力チェック必須　日付等　fromDate > toDate
    if (!this.isValidSearchOption()) return;

    // 動作中のコンポーネントを開く
    let componentFactory = this.componentFactoryResolver.resolveComponentFactory(ModalRouterProgressComponent);
    let modalRouterProgressComponentRef = this.viewContainerRef.createComponent(componentFactory);
    modalRouterProgressComponentRef.instance.Closing.subscribe(() => {
      modalRouterProgressComponentRef.destroy();
    });
    

    this.setSearchOption();

    this.billingSearvice.GetDueAtModifyItems(this.billingSearch)
      .subscribe(response => {
        this.processCustomResult = this.processResultService.processAtGetData(
          this.processCustomResult, response, true, this.partsResultMessageComponent);
        if (this.processCustomResult.result == PROCESS_RESULT_RESULT_TYPE.SUCCESS) {
          this.billingDueAtModifyResults = new BillingDueAtModifyResults();
          this.billingDueAtModifyResults.billings = new Array<BillingDueAtModify>();
          this.billingDueAtModifyResults.billings = response;
          this.sourceBillings = this.billingDueAtModifyResults.billings.map(x => Object.assign({}, x));

          // 入金予定日
          this.detailDueAtCtrls = new Array<FormControl>(response.length);
          this.detailCollectCategoryCodeCtrls = new Array<FormControl>(response.length);

          for (let index = 0; index < response.length; index++) {

            this.detailDueAtCtrls[index] = new FormControl("", [Validators.maxLength(10)]);  // 入金予定日
            this.detailCollectCategoryCodeCtrls[index] = new FormControl(""); // 回収区分

            this.MyFormGroup.removeControl("detailDueAtCtrl" + index);
            this.MyFormGroup.removeControl("detailCollectCategoryCodeCtrl" + index);

            this.MyFormGroup.addControl("detailDueAtCtrl" + index, this.detailDueAtCtrls[index]);
            this.MyFormGroup.addControl("detailCollectCategoryCodeCtrl" + index, this.detailCollectCategoryCodeCtrls[index]);

            if (response[index].modifiedDueAt != undefined) {
              let date = new Date(response[index].modifiedDueAt);
              this.detailDueAtCtrls[index].setValue(new NgbDate(date.getFullYear(), date.getMonth() + 1, date.getDate()));
            }
            else {
              this.detailDueAtCtrls[index].setValue(null);
            }

            if (response[index].collectCategoryCode != undefined) {
              this.detailCollectCategoryCodeCtrls[index].setValue(response[index].collectCategoryCode + ":" + response[index].collectCategoryName);
            }

          }

          this.panelOpenState = false;
        }
        else {
          this.panelOpenState = true;
        }
        modalRouterProgressComponentRef.destroy();

      });

  }

  public isValidSearchOption(): boolean {

    let msg:string="";

    if (!DateUtil.isValidRange(this.billedAtFromCtrl, this.billedAtToCtrl)) {

      msg = MSG_WNG.INPUT_VALID_DATE_FOR_PARAMETERS.replace(MSG_ITEM_NUM.FIRST, '請求日終了')
                                                    .replace(MSG_ITEM_NUM.SECOND, '請求日開始');
      this.processCustomResult = this.processResultService.processAtWarning(
        this.processCustomResult, msg, this.partsResultMessageComponent);

      return false;
    }
    if (!DateUtil.isValidRange(this.closingAtFromCtrl, this.closingAtToCtrl)) {

      msg = MSG_WNG.INPUT_VALID_DATE_FOR_PARAMETERS.replace(MSG_ITEM_NUM.FIRST, '請求締日終了')
                                                    .replace(MSG_ITEM_NUM.SECOND, '請求締日開始');
      this.processCustomResult = this.processResultService.processAtWarning(
        this.processCustomResult, msg, this.partsResultMessageComponent);

      return false;
    }
    if (!DateUtil.isValidRange(this.dueAtFromCtrl, this.dueAtToCtrl)) {

      msg = MSG_WNG.INPUT_VALID_DATE_FOR_PARAMETERS.replace(MSG_ITEM_NUM.FIRST, '入金予定日終了')
                                                    .replace(MSG_ITEM_NUM.SECOND, '入金予定日開始');
      this.processCustomResult = this.processResultService.processAtWarning(
        this.processCustomResult, msg, this.partsResultMessageComponent);

      return false;
    }

    return true;
  }



  public setSearchOption() {
    this.billingSearch = new BillingSearch();
    this.billingSearch.companyId = this.userInfoService.Company.id;
    this.billingSearch.loginUserId = this.userInfoService.LoginUser.id;

    this.billingSearch.bsBilledAtFrom = DateUtil.ConvertFromDatepicker(this.billedAtFromCtrl);  //  請求日
    this.billingSearch.bsBilledAtTo = DateUtil.ConvertFromDatepicker(this.billedAtToCtrl);

    this.billingSearch.bsClosingAtFrom = DateUtil.ConvertFromDatepicker(this.closingAtFromCtrl);  //  請求締日
    this.billingSearch.bsClosingAtTo = DateUtil.ConvertFromDatepicker(this.closingAtToCtrl);

    this.billingSearch.bsDueAtFrom = DateUtil.ConvertFromDatepicker(this.dueAtFromCtrl);  //  入金予定日
    this.billingSearch.bsDueAtTo = DateUtil.ConvertFromDatepicker(this.dueAtToCtrl);

    this.billingSearch.bsInvoiceCodeFrom = this.invoiceCodeFromCtrl.value;  //  請求書番号
    this.billingSearch.bsInvoiceCodeTo = this.invoiceCodeToCtrl.value;
    this.billingSearch.bsInvoiceCode = this.invoiceCodeCtrl.value;

    this.billingSearch.bsCurrencyCode = this.currencyCodeCtrl.value;  //  通貨コード
    this.billingSearch.useSectionMaster = this.userInfoService.ApplicationControl.useReceiptSection == 1 && this.cbxUseReceiptSectionCtrl.value;  //  入金部門対応マスターを使用

    this.billingSearch.bsDepartmentCodeFrom = this.departmentCodeFromCtrl.value;  //  請求部門コード
    this.billingSearch.bsDepartmentCodeTo = this.departmentCodeToCtrl.value;

    this.billingSearch.bsStaffCodeFrom = this.staffCodeFromCtrl.value;    //    担当者コード
    this.billingSearch.bsStaffCodeTo = this.staffCodeToCtrl.value;

    this.billingSearch.bsCustomerCodeFrom = this.customerCodeFromCtrl.value;  //  得意先コード
    this.billingSearch.bsCustomerCodeTo = this.customerCodeToCtrl.value;

    this.billingSearch.assignmentFlg = this.billingSearch.assignmentFlg + 1 | this.billingSearch.assignmentFlg + 2;

  }

  public update() {

    // 動作中のコンポーネントを開く
    let componentFactory = this.componentFactoryResolver.resolveComponentFactory(ModalRouterProgressComponent);
    let modalRouterProgressComponentRef = this.viewContainerRef.createComponent(componentFactory);
    modalRouterProgressComponentRef.instance.Closing.subscribe(() => {
      modalRouterProgressComponentRef.destroy();
    });

    let updBillings = new Array<BillingDueAtModify>();

    for (let i = 0; i < this.billingDueAtModifyResults.billings.length; i++) {
      let bEdit = false;
      let eBilling = this.billingDueAtModifyResults.billings[i];

      if (this.isDueAtModified(i)) {
        eBilling.newDueAt = eBilling.modifiedDueAt;
        bEdit = true;
      }
      if (this.isCollectCategoryModified(i)) {
        eBilling.newCollectCategoryId = eBilling.collectCategoryId;
        bEdit = true;
      }
      if (bEdit) {
        updBillings.push(eBilling);
      }

    }

    if (updBillings.length > 0) {
      this.billingSearvice.UpdateDueAt(updBillings)
        .subscribe(result => {
          this.processCustomResult = this.processResultService.processAtSave(
            this.processCustomResult, result, false, this.partsResultMessageComponent);
          modalRouterProgressComponentRef.destroy();
        });
    }
    else{
      modalRouterProgressComponentRef.destroy();
    }

  }

  public isDueAtModified(index: number): boolean {
    let newValue = StringUtil.IsNullOrEmpty(this.billingDueAtModifyResults.billings[index].modifiedDueAt) ? "" : DateUtil.getYYYYMMDD(1, this.billingDueAtModifyResults.billings[index].modifiedDueAt);
    let oldValue = DateUtil.convertDateString(this.sourceBillings[index].modifiedDueAt)
    return newValue != oldValue;
  }

  public isCollectCategoryModified(index: number): boolean {
    return this.billingDueAtModifyResults.billings[index].collectCategoryId != this.sourceBillings[index].collectCategoryId;
  }

  ///////////////////////////////////////////////////////////////////////
  public setBilledAtFrom(eventType: string) {
    HtmlUtil.nextFocusByName(this.elementRef, 'billedAtToCtrl', eventType);
  }

  public setBilledAtTo(eventType: string) {
    HtmlUtil.nextFocusByName(this.elementRef, 'closingAtFromCtrl', eventType);
  }

  ///////////////////////////////////////////////////////////////////////
  public setClosingAtFrom(eventType: string) {
    HtmlUtil.nextFocusByName(this.elementRef, 'closingAtToCtrl', eventType);
  }

  public setClosingAtTo(eventType: string) {
    HtmlUtil.nextFocusByName(this.elementRef, 'dueAtFromCtrl', eventType);
  }

  ///////////////////////////////////////////////////////////////////////
  public setDueAtFrom(eventType: string) {
    HtmlUtil.nextFocusByName(this.elementRef, 'dueAtToCtrl', eventType);
  }

  public setDueAtTo(eventType: string) {
    HtmlUtil.nextFocusByName(this.elementRef, 'invoiceCodeFromCtrl', eventType);
  }

  ///////////////////////////////////////////////////////////////////////
  public setInvoiceCodeFrom(eventType: string) {
    HtmlUtil.nextFocusByName(this.elementRef, 'invoiceCodeToCtrl', eventType);
  }

  public setInvoiceCodeTo(eventType: string) {
    HtmlUtil.nextFocusByName(this.elementRef, 'invoiceCodeCtrl', eventType);
  }

  ///////////////////////////////////////////////////////////////////////
  public setInvoiceCode(eventType: string) {
    HtmlUtil.nextFocusByNames(this.elementRef, ['currencyCodeCtrl','departmentCodeFromCtrl'], eventType);
  }

  ///////////////////////////////////////////////////////////////////////
  public setCurrencyCode(eventType: string) {

    if (!StringUtil.IsNullOrEmpty(this.currencyCodeCtrl.value)) {

      this.currencyService.GetItems(this.currencyCodeCtrl.value)
        .subscribe(response => {
          if (response != PROCESS_RESULT_RESULT_TYPE.FAILURE && response.length > 0) {
            this.currencyCodeCtrl.setValue(response[0].code);
            HtmlUtil.nextFocusByName(this.elementRef, 'departmentCodeFromCtrl', eventType);
          }
          else {
            this.currencyCodeCtrl.setValue("");
            HtmlUtil.nextFocusByName(this.elementRef, 'departmentCodeFromCtrl', eventType);
          }
        });

    }
    else {
      this.currencyCodeCtrl.setValue("");
      HtmlUtil.nextFocusByName(this.elementRef, 'departmentCodeFromCtrl', eventType);
    }
  }


  public setCbxUseReceiptSection(eventType: string) {

    let localstorageItem = new LocalStorageItem();
    localstorageItem.key = RangeSearchKey.PC0501_USE_RECEIPT_SECTION;
    localstorageItem.value = this.cbxUseReceiptSectionCtrl.value;
    this.localStorageManageService.set(localstorageItem);

    HtmlUtil.nextFocusByName(this.elementRef, 'departmentCodeFromCtrl', eventType);

  }    

  ///////////////////////////////////////////////////////////////////////

  public setDepartmentCodeFrom(eventType: string) {

    if(eventType!=EVENT_TYPE.BLUR){
      if(this.departmentCodeFromigger)  this.departmentCodeFromigger.closePanel();
    }    

    if (!StringUtil.IsNullOrEmpty(this.departmentCodeFromCtrl.value)) {

      this.loadStart();
      this.departmentService.GetItems(this.departmentCodeFromCtrl.value)
        .subscribe(response => {
          this.loadEnd();
          if (response != PROCESS_RESULT_RESULT_TYPE.FAILURE && response.length > 0) {
            this.departmentCodeFromCtrl.setValue(response[0].code);
            this.departmentNameFromCtrl.setValue(response[0].name);
            if (this.cbxDepartmentCtrl.value == true) {
              this.departmentCodeToCtrl.setValue(response[0].code);
              this.departmentNameToCtrl.setValue(response[0].name);
            }
            HtmlUtil.nextFocusByName(this.elementRef, 'departmentCodeToCtrl', eventType);
          }
          else {
            //this.departmentCodeFromCtrl.setValue("");
            this.departmentNameFromCtrl.setValue("");
            if (this.cbxDepartmentCtrl.value == true) {
              this.departmentCodeToCtrl.setValue(this.departmentCodeFromCtrl.value);
              this.departmentNameToCtrl.setValue("");
            }
            HtmlUtil.nextFocusByName(this.elementRef, 'departmentCodeToCtrl', eventType);
          }

        });
    }
    else {
      this.departmentCodeFromCtrl.setValue("");
      this.departmentNameFromCtrl.setValue("");
      HtmlUtil.nextFocusByName(this.elementRef, 'departmentCodeToCtrl', eventType);
    }

  }

  public setDepartmentCodeTo(eventType: string) {

    if(eventType!=EVENT_TYPE.BLUR){
      if(this.departmentCodeToigger)  this.departmentCodeToigger.closePanel();
    }    

    if (!StringUtil.IsNullOrEmpty(this.departmentCodeToCtrl.value)) {

      this.loadStart();
      this.departmentService.GetItems(this.departmentCodeToCtrl.value)
        .subscribe(response => {
          this.loadEnd();
          if (response != PROCESS_RESULT_RESULT_TYPE.FAILURE && response.length > 0) {
            this.departmentCodeToCtrl.setValue(response[0].code);
            this.departmentNameToCtrl.setValue(response[0].name);

            HtmlUtil.nextFocusByName(this.elementRef, 'staffCodeFromCtrl', eventType);
          }
          else {
            //this.departmentCodeToCtrl.setValue("");
            this.departmentNameToCtrl.setValue("");
            HtmlUtil.nextFocusByName(this.elementRef, 'staffCodeFromCtrl', eventType);
          }
        });
    }
    else {
      this.departmentCodeToCtrl.setValue("");
      this.departmentNameToCtrl.setValue("");
      HtmlUtil.nextFocusByName(this.elementRef, 'staffCodeFromCtrl', eventType);
    }
  }

  public setCbxDepartment(eventType: string) {

    let localstorageItem = new LocalStorageItem();
    localstorageItem.key = RangeSearchKey.PC0501_DEPARTMENT;
    localstorageItem.value = this.cbxDepartmentCtrl.value;
    this.localStorageManageService.set(localstorageItem);

    HtmlUtil.nextFocusByName(this.elementRef, 'departmentCodeFromCtrl', eventType);

  }

  ///////////////////////////////////////////////////////////////////////

  public setStaffCodeFrom(eventType: string) {

    if(eventType!=EVENT_TYPE.BLUR){
      if(this.staffCodeFromTrigger) this.staffCodeFromTrigger.closePanel();
    }    

    if (!StringUtil.IsNullOrEmpty(this.staffCodeFromCtrl.value)) {

      this.loadStart();
      this.staffService.GetItems(this.staffCodeFromCtrl.value)
        .subscribe(response => {
          this.loadEnd();
          if (response != PROCESS_RESULT_RESULT_TYPE.FAILURE && response.length > 0) {
            this.staffCodeFromCtrl.setValue(response[0].code);
            this.staffNameFromCtrl.setValue(response[0].name);
            if (this.cbxStaffCtrl.value == true) {
              this.staffCodeToCtrl.setValue(response[0].code);
              this.staffNameToCtrl.setValue(response[0].name);
            }
            HtmlUtil.nextFocusByName(this.elementRef, 'staffCodeToCtrl', eventType);
          }
          else {
            //this.staffCodeFromCtrl.setValue("");
            this.staffNameFromCtrl.setValue("");
            if (this.cbxStaffCtrl.value == true) {
              this.staffCodeToCtrl.setValue(this.staffCodeFromCtrl.value);
              this.staffNameToCtrl.setValue("");
            }            
            HtmlUtil.nextFocusByName(this.elementRef, 'staffCodeToCtrl', eventType);
          }
        });
    }
    else {
      this.staffCodeFromCtrl.setValue("");
      this.staffNameFromCtrl.setValue("");
      HtmlUtil.nextFocusByName(this.elementRef, 'staffCodeToCtrl', eventType);
    }
  }

  public setStaffCodeTo(eventType: string) {

    if(eventType!=EVENT_TYPE.BLUR){
      if(this.staffCodeToTrigger) this.staffCodeToTrigger.closePanel();
    }

    if (!StringUtil.IsNullOrEmpty(this.staffCodeToCtrl.value)) {

      this.loadStart();
      this.staffService.GetItems(this.staffCodeToCtrl.value)
        .subscribe(response => {
          this.loadEnd();
          if (response != PROCESS_RESULT_RESULT_TYPE.FAILURE && response.length > 0) {
            this.staffCodeToCtrl.setValue(response[0].code);
            this.staffNameToCtrl.setValue(response[0].name);

            HtmlUtil.nextFocusByName(this.elementRef, 'customerCodeFromCtrl', eventType);
          }
          else {
            //this.staffCodeToCtrl.setValue("");
            this.staffNameToCtrl.setValue("");
            HtmlUtil.nextFocusByName(this.elementRef, 'customerCodeFromCtrl', eventType);
          }
        });
    }
    else {
      this.staffCodeToCtrl.setValue("");
      this.staffNameToCtrl.setValue("");
      HtmlUtil.nextFocusByName(this.elementRef, 'customerCodeFromCtrl', eventType);
    }
  }

  public setCbxStaff(eventType: string) {

    let localstorageItem = new LocalStorageItem();
    localstorageItem.key = RangeSearchKey.PC0501_STAFF;
    localstorageItem.value = this.cbxStaffCtrl.value;
    this.localStorageManageService.set(localstorageItem);

    HtmlUtil.nextFocusByName(this.elementRef, 'staffCodeFromCtrl', eventType);
  }    

  ///////////////////////////////////////////////////////////////////////
  public setCustomerCodeFrom(eventType: string) {

    if(eventType!=EVENT_TYPE.BLUR){
      if(this.customerCodeFromTrigger)  this.customerCodeFromTrigger.closePanel();
    }

    if (!StringUtil.IsNullOrEmpty(this.customerCodeFromCtrl.value)) {

      this.loadStart();
      this.customerService.GetItems(this.customerCodeFromCtrl.value)
        .subscribe(response => {
          this.loadEnd();
          if (response != PROCESS_RESULT_RESULT_TYPE.FAILURE && response.length > 0) {
            this.customerCodeFromCtrl.setValue(response[0].code);
            this.customerNameFromCtrl.setValue(response[0].name);
            if (this.cbxCustomerCtrl.value == true) {
              this.customerCodeToCtrl.setValue(response[0].code);
              this.customerNameToCtrl.setValue(response[0].name);
            }
            HtmlUtil.nextFocusByName(this.elementRef, 'customerCodeToCtrl', eventType);
          }
          else {
            //this.customerCodeFromCtrl.setValue("");
            this.customerNameFromCtrl.setValue("");
            if (this.cbxCustomerCtrl.value == true) {
              this.customerCodeToCtrl.setValue(this.customerCodeFromCtrl.value);
              this.customerNameToCtrl.setValue("");
            }
            HtmlUtil.nextFocusByName(this.elementRef, 'customerCodeToCtrl', eventType);
          }
        });
    }
    else {
      this.customerCodeFromCtrl.setValue("");
      this.customerNameFromCtrl.setValue("");
      HtmlUtil.nextFocusByName(this.elementRef, 'customerCodeToCtrl', eventType);
    }
  }


  public setCustomerCodeTo(eventType: string) {

    if(eventType!=EVENT_TYPE.BLUR){
      if(this.customerCodeToTrigger)  this.customerCodeToTrigger.closePanel();
    }    

    if (!StringUtil.IsNullOrEmpty(this.customerCodeToCtrl.value)) {

      this.loadStart();
      this.customerService.GetItems(this.customerCodeToCtrl.value)
        .subscribe(response => {
          this.loadEnd();
          if (response != PROCESS_RESULT_RESULT_TYPE.FAILURE && response.length > 0) {
            this.customerCodeToCtrl.setValue(response[0].code);
            this.customerNameToCtrl.setValue(response[0].name);

            HtmlUtil.nextFocusByName(this.elementRef, 'billedAtFromCtrl', eventType);
          }
          else {
            //this.customerCodeToCtrl.setValue("");
            this.customerNameToCtrl.setValue("");
            HtmlUtil.nextFocusByName(this.elementRef, 'billedAtFromCtrl', eventType);
          }
        });
    }
    else {
      this.customerCodeToCtrl.setValue("");
      this.customerNameToCtrl.setValue("");
      HtmlUtil.nextFocusByName(this.elementRef, 'billedAtFromCtrl', eventType);
    }


  }

  public setCbxCustomer(eventType: string) {

    let localstorageItem = new LocalStorageItem();
    localstorageItem.key = RangeSearchKey.PC0501_CUSTOMER;
    localstorageItem.value = this.cbxDepartmentCtrl.value;
    this.localStorageManageService.set(localstorageItem);


    HtmlUtil.nextFocusByName(this.elementRef, 'customerCodeFromCtrl', eventType);

  }

  /////////////////////////////////////////////////////////////////////////////
  // 詳細部分
  /////////////////////////////////////////////////////////////////////////////

  /////////////////////////////////////////////////////////////////////////////
  public setDetailDueAt(eventType: any, index: number) {
    this.billingDueAtModifyResults.billings[index].modifiedDueAt = DateUtil.ConvertFromDatepicker(this.detailDueAtCtrls[index]);
    this.setUpdateBtnFlg();
    HtmlUtil.nextFocusByName(this.elementRef, 'detailCollectCategoryCodeCtrl' + index, eventType);
  }
  /////////////////////////////////////////////////////////////////////////////
  public setDetailCollectCategoryCode(eventType: any, index: number) {

    if (!StringUtil.IsNullOrEmpty(this.detailCollectCategoryCodeCtrls[index].value)) {
      this.detailCollectCategoryCodeCtrls[index].setValue(StringUtil.setPaddingFrontZero(this.detailCollectCategoryCodeCtrls[index].value, 2,true));

      if (eventType == "blur") {
        this.detailCollectCategoryCodeCtrls[index].setValue(this.detailCollectCategoryCodeCtrls[index].value.split(":")[0]);
      }

      this.loadStart();
      this.categoryService.GetItems(CategoryType.Collection, this.detailCollectCategoryCodeCtrls[index].value)
        .subscribe(response => {
          this.loadEnd();
          if (response != PROCESS_RESULT_RESULT_TYPE.FAILURE && response.length > 0) {
            this.detailCollectCategoryCodeCtrls[index].setValue(response[0].code + ":" + response[0].name);
            this.billingDueAtModifyResults.billings[index].collectCategoryId = response[0].id;
            this.setUpdateBtnFlg();
            HtmlUtil.nextFocusByName(this.elementRef, 'detailDueAtCtrl' + index, eventType);
          }
          else {
            this.detailCollectCategoryCodeCtrls[index].setValue("");
            HtmlUtil.nextFocusByName(this.elementRef, 'detailDueAtCtrl' + index, eventType);
          }
        });
    }
    else {
      HtmlUtil.nextFocusByName(this.elementRef, 'detailDueAtCtrl' + index, eventType);
      this.detailCollectCategoryCodeCtrls[index].setValue("");
    }
  }

  public initDetailCollectCategoryCode(index: number) {

    if (!StringUtil.IsNullOrEmpty(this.detailCollectCategoryCodeCtrls[index].value)) {
      let tmp = this.detailCollectCategoryCodeCtrls[index].value;
      tmp = tmp.split(":")[0];

      this.detailCollectCategoryCodeCtrls[index].setValue(tmp);
    }
  }

  public setUpdateBtnFlg() {

    for (let i = 0; i < this.sourceBillings.length; i++) {
      if (this.isDueAtModified(i)) {
        this.updateBtnFlg = false;
        return;
      }
      if (this.isCollectCategoryModified(i)) {
        this.updateBtnFlg = false;
        return;
      }
    }
    this.updateBtnFlg = true;

  }

}
