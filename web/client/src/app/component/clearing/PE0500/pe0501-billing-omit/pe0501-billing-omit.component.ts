import { Component, OnInit, ElementRef, ComponentFactoryResolver, ViewContainerRef, AfterViewInit, ViewChild } from '@angular/core';
import { Router, ActivatedRoute, NavigationEnd } from '@angular/router';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { NgbDateParserFormatter } from '@ng-bootstrap/ng-bootstrap';
import { forkJoin } from 'rxjs';

import { KEY_CODE, EVENT_TYPE, BUTTON_ACTION } from 'src/app/common/const/event.const';
import { FILE_EXTENSION } from 'src/app/common/const/eb-file.const';
import { BILL_INPUT_TYPE_DICTIONARY, CategoryType, MATCHING_ASSIGNMENT_FLAG_DICTIONARY, GridId, BANK_TRANSFER_RESULT_DICTIONARY, CODE_TYPE, BILLING_AMOUNT_RANGE_DICTIONARY, FunctionType } from 'src/app/common/const/kbn.const';
import { MSG_WNG, MSG_ERR, MSG_ITEM_NUM, MSG_INF } from 'src/app/common/const/message.const';
import { RangeSearchKey } from 'src/app/common/const/local-storage-key-const';
import { LINE_FEED_CODE } from 'src/app/common/const/report-setting-kbn.const';
import { MODAL_STATUS_TYPE, PROCESS_RESULT_RESULT_TYPE } from 'src/app/common/const/status.const';
import { TABLE_INDEX } from 'src/app/common/const/table-name.const';
import { BillingInputType } from 'src/app/common/common-const';
import { EbDataHelper } from 'src/app/common/util/eb-data-helper';
import { FileUtil } from 'src/app/common/util/file.util';
import { NgbDateCustomParserFormatter } from 'src/app/common/util/dateformat-util';
import { DateUtil } from 'src/app/common/util/date-util';
import { FormatterUtil, FormatStyle } from 'src/app/common/util/formatter.util';
import { HtmlUtil } from 'src/app/common/util/html.util';
import { PageUtil } from 'src/app/common/util/page-util';
import { StringUtil } from 'src/app/common/util/string-util';
import { BaseComponent } from 'src/app/component/common/base/base-component';
import { ModalRouterProgressComponent } from 'src/app/component/modal/modal-router-progress/modal-router-progress.component';
import { ModalMasterComponent } from 'src/app/component/modal/modal-master/modal-master.component';
import { ModalConfirmComponent } from 'src/app/component/modal/modal-confirm/modal-confirm.component';
import { BillingsResult } from 'src/app/model/billings-result.model';
import { BillingSearch } from 'src/app/model/billing-search.model';
import { BillingHelper } from 'src/app/model/helper/billing-helper.model';
import { JuridicalPersonalitysResult } from 'src/app/model/juridical-personalitys-result.model';
import { OmitSource } from 'src/app/model/omit-source.model';
import { Transaction } from 'src/app/model/transaction.model';
import { CountResult } from 'src/app/model/count-result.model';
import { GridSettingsResult } from 'src/app/model/grid-settings-result.model';
import { GridSetting } from 'src/app/model/grid-setting.model';
import { GridSettingHelper } from 'src/app/model/grid-setting-helper.model';
import { LocalStorageItem } from 'src/app/model/custom-model/local-storage-item';
import { RacCurrencyPipe } from 'src/app/pipe/currency.pipe';
import { CategoryMasterService } from 'src/app/service/Master/category-master.service';
import { BillingService } from 'src/app/service/billing.service';
import { DepartmentMasterService } from 'src/app/service/Master/department-master.service';
import { StaffMasterService } from 'src/app/service/Master/staff-master.service';
import { CustomerMasterService } from 'src/app/service/Master/customer-master.service';
import { UserInfoService } from 'src/app/service/common/user-info.service';
import { CurrencyMasterService } from 'src/app/service/Master/currency-master.service';
import { LoginUserMasterService } from 'src/app/service/Master/login-user-master.service';
import { JuridicalPersonalityMasterService } from 'src/app/service/Master/juridical-personality-master.service';
import { GridSettingMasterService } from 'src/app/service/Master/grid-setting-master.service';
import { LocalStorageManageService } from 'src/app/service/common/local-storage-manage.service';
import { ProcessResultService } from 'src/app/service/common/process-result.service';
import { MatAutocompleteTrigger, MatExpansionPanel } from '@angular/material';

@Component({
  selector: 'app-pe0501-billing-omit',
  templateUrl: './pe0501-billing-omit.component.html',
  styleUrls: ['./pe0501-billing-omit.component.css'],
  providers: [
    { provide: NgbDateParserFormatter, useClass: NgbDateCustomParserFormatter }
  ]

})
export class Pe0501BillingOmitComponent extends BaseComponent implements OnInit,AfterViewInit {

  public MATCHING_ASSIGNMENT_FLAG_DICTIONARY: typeof MATCHING_ASSIGNMENT_FLAG_DICTIONARY = MATCHING_ASSIGNMENT_FLAG_DICTIONARY;
  public BILL_INPUT_TYPE_DICTIONARY: typeof BILL_INPUT_TYPE_DICTIONARY = BILL_INPUT_TYPE_DICTIONARY;
  public BANK_TRANSFER_RESULT_DICTIONARY: typeof BANK_TRANSFER_RESULT_DICTIONARY = BANK_TRANSFER_RESULT_DICTIONARY;
  public readonly amountRangeDictionary = BILLING_AMOUNT_RANGE_DICTIONARY;

  public gridSettingsResult: GridSettingsResult;
  public juridicalPersonalitysResult: JuridicalPersonalitysResult;

  public billingsResult: BillingsResult;
  public billingSearch: BillingSearch;

  public dispSumCount: number;
  public dispSumBillingAmount: number;
  public dispSumRemainAmount: number;

  public deleteOrRecoveryButton: string;

  public selectedDelete: boolean;

  // これをしないとEnum型がテンプレートで見えなかったため
  public TABLE_INDEX: typeof TABLE_INDEX = TABLE_INDEX;

  public cbxCheckAllCtrl:FormControl; // 全選択・全解除

  public billedAtFromCtrl: FormControl;  // 請求日
  public billedAtToCtrl: FormControl;
  public salesAtFromCtrl: FormControl;  // 売上日
  public salesAtToCtrl: FormControl;
  public dueAtFromCtrl: FormControl;  // 入金予定日
  public dueAtToCtrl: FormControl;
  public deletedAtFromCtrl: FormControl;  // 削除日
  public deletedAtToCtrl: FormControl;
  public invoiceCodeFromCtrl: FormControl;  // 請求書番号
  public invoiceCodeToCtrl: FormControl;
  public invoiceCodeCtrl: FormControl;
  public updateAtFromCtrl: FormControl;  // 最終更新日
  public updateAtToCtrl: FormControl;
  public updateByCodeCtrl: FormControl;  // 最終更新者
  public updateByNameCtrl: FormControl;
  public memoCtrl: FormControl; // メモ
  public cbxMemoCtrl: FormControl;
  public billingCategoryCodeCtrl: FormControl;  // 請求区分
  public billingCategoryNameCtrl: FormControl;
  public billingCategoryId: number;
  public collectCategoryCodeCtrl: FormControl;  // 回収区分
  public collectCategoryNameCtrl: FormControl;
  public collectCategoryId: number;
  public inputTypeCtrl: FormControl;  // 入力区分
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
  public cbxUseReceiptSectionCtrl: FormControl; // 入金部門対応マスタを使用
  public customerKanaCtrl: FormControl;  // 得意先カナ
  public currencyCodeCtrl: FormControl; // 通貨コード
  public cbxFullAssignmentCtrl: FormControl;  // 消込区分
  public cbxPartAssignmentCtrl: FormControl;
  public cbxNoAssignmentCtrl: FormControl;
  public amountRangeCtrl: FormControl; // 金額範囲
  public amountFromCtrl: FormControl;
  public amountToCtrl: FormControl;
  public cbxAmountCtrl: FormControl;
  public note1Ctrl: FormControl;  // 備考
  public note2Ctrl: FormControl;
  public note3Ctrl: FormControl;
  public note4Ctrl: FormControl;
  public note5Ctrl: FormControl;
  public note6Ctrl: FormControl;
  public note7Ctrl: FormControl;
  public note8Ctrl: FormControl;

  public cbxDeletedFlagCtrl: FormControl;

  public cbxDetailDelFlagCtrls: FormControl[];

  public undefineCtrl: FormControl; // 未定用

  public panelOpenState: boolean;
  public AllGridSettings: GridSetting[];
  public gridSettingHelper = new GridSettingHelper();

  public simplePageSearch:boolean=true;

  @ViewChild('updateByCodeInput', { read: MatAutocompleteTrigger }) updateByCodeTrigger: MatAutocompleteTrigger;
  @ViewChild('billingCategoryCodeInput', { read: MatAutocompleteTrigger }) billingCategoryCodeTrigger: MatAutocompleteTrigger;
  @ViewChild('collectCategoryCodeInput', { read: MatAutocompleteTrigger }) collectCategoryCodeTrigger: MatAutocompleteTrigger;
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
    public gridSettingService: GridSettingMasterService,
    public juridicalPersonalityService: JuridicalPersonalityMasterService,
    public categoryService: CategoryMasterService,
    public billingService: BillingService,
    public departmentService: DepartmentMasterService,
    public staffService: StaffMasterService,
    public customerService: CustomerMasterService,
    public currencyService: CurrencyMasterService,
    public loginUserService: LoginUserMasterService,
    public processResultService: ProcessResultService,
    public localStorageManageService:LocalStorageManageService,
    public billingHelper: BillingHelper,
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

    this.securityHideShow = this.userInfoService.isFunctionAvailable(FunctionType.ModifyBilling);

    let gridSettingResponse = this.gridSettingService.GetItems(GridId.BillingSearch);
    let juridicalPersonalityResponse = this.juridicalPersonalityService.GetItems();

    forkJoin(
      gridSettingResponse,
      juridicalPersonalityResponse,
    )
      .subscribe(responseList => {

        if (responseList.length != 2
          || responseList[0] == PROCESS_RESULT_RESULT_TYPE.FAILURE
          || responseList[1] == PROCESS_RESULT_RESULT_TYPE.FAILURE
        ) {
        }
        else {
          this.gridSettingsResult = new GridSettingsResult
          this.gridSettingsResult.gridSettings = responseList[0].filter((gridSetting: { displayWidth: number; }) => { return gridSetting.displayWidth != 0 });
          // 検索条件用絞込無
          this.AllGridSettings = responseList[0];

          this.juridicalPersonalitysResult = new JuridicalPersonalitysResult();
          this.juridicalPersonalitysResult.juridicalPersonalities = responseList[1];
        }
      },
        error => {
        }
      );
  }

  ngAfterViewInit(){
    HtmlUtil.nextFocusByName(this.elementRef, 'billedAtFromCtrl', EVENT_TYPE.NONE);
  }

  public setControlInit() {

    this.cbxCheckAllCtrl = new FormControl(null);  // 全選択・全解除

    this.billedAtFromCtrl = new FormControl("", [Validators.maxLength(10)]);  // 請求日
    this.billedAtToCtrl = new FormControl("", [Validators.maxLength(10)]);
    this.salesAtFromCtrl = new FormControl("", [Validators.maxLength(10)]);  // 売上日
    this.salesAtToCtrl = new FormControl("", [Validators.maxLength(10)]);
    this.dueAtFromCtrl = new FormControl("", [Validators.maxLength(10)]);  // 入金予定日
    this.dueAtToCtrl = new FormControl("", [Validators.maxLength(10)]);
    this.deletedAtFromCtrl = new FormControl("", [Validators.maxLength(10),]);  // 削除日
    this.deletedAtToCtrl = new FormControl("", [Validators.maxLength(10),]);
    this.invoiceCodeFromCtrl = new FormControl("", [Validators.maxLength(20)]);  // 請求書番号
    this.invoiceCodeToCtrl = new FormControl("", [Validators.maxLength(20)]);
    this.invoiceCodeCtrl = new FormControl("", [Validators.maxLength(20)]);
    this.updateAtFromCtrl = new FormControl("", [Validators.maxLength(10)]);  // 最終更新日
    this.updateAtToCtrl = new FormControl("", [Validators.maxLength(10)]);
    this.updateByCodeCtrl = new FormControl("", [Validators.maxLength(this.userInfoService.ApplicationControl.loginUserCodeLength)]);  // 最終更新者
    this.updateByNameCtrl = new FormControl("");  // 最終更新者
    this.memoCtrl = new FormControl("", [Validators.maxLength(100)]); // メモ
    this.cbxMemoCtrl = new FormControl("");
    this.billingCategoryCodeCtrl = new FormControl("", [Validators.maxLength(2)]);  // 請求区分
    this.billingCategoryNameCtrl = new FormControl("");
    this.collectCategoryCodeCtrl = new FormControl("", [Validators.maxLength(2)]);  // 回収区分
    this.collectCategoryNameCtrl = new FormControl("");
    this.inputTypeCtrl = new FormControl("");  // 入力区分

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
    this.cbxUseReceiptSectionCtrl = new FormControl(""); // 入金部門対応マスタを使用
    this.customerKanaCtrl = new FormControl("");
    if (this.userInfoService.ApplicationControl.useForeignCurrency === 1) {
      this.currencyCodeCtrl = new FormControl("", [Validators.required, Validators.maxLength(3)]); // 通貨コード
    }
    else {
      this.currencyCodeCtrl = new FormControl("", [Validators.maxLength(3)]); // 通貨コード
    }
    this.cbxFullAssignmentCtrl = new FormControl(false);  // 消込区分
    this.cbxPartAssignmentCtrl = new FormControl(false);
    this.cbxNoAssignmentCtrl = new FormControl(false);
    this.amountRangeCtrl = new FormControl(""); // 金額範囲
    this.amountFromCtrl = new FormControl("", [Validators.maxLength(16)]);
    this.amountToCtrl = new FormControl("", [Validators.maxLength(16)]);
    this.cbxAmountCtrl = new FormControl("");
    this.note1Ctrl = new FormControl("", [Validators.maxLength(100)]);  // 備考
    this.note2Ctrl = new FormControl("", [Validators.maxLength(100)]);
    this.note3Ctrl = new FormControl("", [Validators.maxLength(100)]);
    this.note4Ctrl = new FormControl("", [Validators.maxLength(100)]);
    this.note5Ctrl = new FormControl("", [Validators.maxLength(100)]);
    this.note6Ctrl = new FormControl("", [Validators.maxLength(100)]);
    this.note7Ctrl = new FormControl("", [Validators.maxLength(100)]);
    this.note8Ctrl = new FormControl("", [Validators.maxLength(100)]);

    this.cbxDeletedFlagCtrl = new FormControl("");

    this.undefineCtrl = new FormControl(""); // 未定用;

  }

  public setValidator() {
    this.MyFormGroup = new FormGroup({
      
      cbxCheckAllCtrl:this.cbxCheckAllCtrl, // 全選択・全解除
      billedAtFromCtrl: this.billedAtFromCtrl,  // 請求日
      billedAtToCtrl: this.billedAtToCtrl,
      salesAtFromCtrl: this.salesAtFromCtrl,  // 売上日
      salesAtToCtrl: this.salesAtToCtrl,
      dueAtFromCtrl: this.dueAtFromCtrl,  // 入金予定日
      dueAtToCtrl: this.dueAtToCtrl,
      deletedAtFromCtrl: this.deletedAtFromCtrl,  // 削除日
      deletedAtToCtrl: this.deletedAtToCtrl,
      invoiceCodeFromCtrl: this.invoiceCodeFromCtrl,  // 請求書番号
      invoiceCodeToCtrl: this.invoiceCodeToCtrl,
      invoiceCodeCtrl: this.invoiceCodeCtrl,
      updateAtFromCtrl: this.updateAtFromCtrl,  // 最終更新日
      updateAtToCtrl: this.updateAtToCtrl,
      updateByCodeCtrl: this.updateByCodeCtrl,  // 最終更新者
      updateByNameCtrl: this.updateByNameCtrl,
      memoCtrl: this.memoCtrl, // メモ
      cbxMemoCtrl: this.cbxMemoCtrl,
      billingCategoryCodeCtrl: this.billingCategoryCodeCtrl,  // 請求区分
      billingCategoryNameCtrl: this.billingCategoryNameCtrl,
      collectCategoryCodeCtrl: this.collectCategoryCodeCtrl,  // 回収区分
      collectCategoryNameCtrl: this.collectCategoryNameCtrl,
      inputTypeCtrl: this.inputTypeCtrl,  // 入力区分
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
      cbxUseReceiptSectionCtrl: this.cbxUseReceiptSectionCtrl, // 入金部門対応マスタを使用
      customerKanaCtrl: this.customerKanaCtrl,
      currencyCodeCtrl: this.currencyCodeCtrl, // 通貨コード
      cbxFullAssignmentCtrl: this.cbxFullAssignmentCtrl,  // 消込区分
      cbxPartAssignmentCtrl: this.cbxPartAssignmentCtrl,
      cbxNoAssignmentCtrl: this.cbxNoAssignmentCtrl,
      amountRangeCtrl: this.amountRangeCtrl, // 金額範囲
      amountFromCtrl: this.amountFromCtrl,
      amountToCtrl: this.amountToCtrl,
      cbxAmountCtrl: this.cbxAmountCtrl,
      note1Ctrl: this.note1Ctrl,  // 備考
      note2Ctrl: this.note2Ctrl,
      note3Ctrl: this.note3Ctrl,
      note4Ctrl: this.note4Ctrl,
      note5Ctrl: this.note5Ctrl,
      note6Ctrl: this.note6Ctrl,
      note7Ctrl: this.note7Ctrl,
      note8Ctrl: this.note8Ctrl,

      cbxDeletedFlagCtrl: this.cbxDeletedFlagCtrl,
      undefineCtrl: this.undefineCtrl, // 未定用;

    });

  }

  public setFormatter() {

    if (this.userInfoService.ApplicationControl.loginUserCodeType == CODE_TYPE.NUMBER) {
      FormatterUtil.setNumberFormatter(this.updateByCodeCtrl); // 最終更新者
    }
    else {
      FormatterUtil.setCodeFormatter(this.updateByCodeCtrl);
    }

    FormatterUtil.setNumberFormatter(this.billingCategoryCodeCtrl); // 請求区分
    FormatterUtil.setNumberFormatter(this.collectCategoryCodeCtrl); // 回収区分

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

    FormatterUtil.setAlphabetFormatter(this.currencyCodeCtrl); // 通貨コード

    FormatterUtil.setCurrencyFormatter(this.amountFromCtrl);  // 金額範囲
    FormatterUtil.setCurrencyFormatter(this.amountToCtrl);
  }


  public setAutoComplete(){
    // 最終更新者
    this.initAutocompleteLoginUsers(this.updateByCodeCtrl,this.loginUserService,0);

    // 請求区分
    this.initAutocompleteCategories(CategoryType.Billing,this.billingCategoryCodeCtrl,this.categoryService,0);
    // 回収区分
    this.initAutocompleteCategories(CategoryType.Collection,this.collectCategoryCodeCtrl,this.categoryService,1);
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

  public closeAutoCompletePanel(){

    if (this.updateByCodeTrigger!= undefined){this.updateByCodeTrigger.closePanel();}
    if (this.billingCategoryCodeTrigger!= undefined){this.billingCategoryCodeTrigger.closePanel();}
    if (this.collectCategoryCodeTrigger!= undefined){this.collectCategoryCodeTrigger.closePanel();}
    if (this.departmentCodeFromigger!= undefined){this.departmentCodeFromigger.closePanel();}
    if (this.departmentCodeToigger!= undefined){this.departmentCodeToigger.closePanel();}
    if (this.staffCodeFromTrigger!= undefined){this.staffCodeFromTrigger.closePanel();}
    if (this.staffCodeToTrigger!= undefined){this.staffCodeToTrigger.closePanel();}
    if (this.customerCodeFromTrigger!= undefined){this.customerCodeFromTrigger.closePanel();}
    if (this.customerCodeToTrigger!= undefined){this.customerCodeToTrigger.closePanel();}

    
  }


  /*
    table:Master名称
    index:明細行の行No
  */
  public openMasterModal(table: TABLE_INDEX, type: string = null,  index: number = -1) {


    this.closeAutoCompletePanel();
    let componentFactory = this.componentFactoryResolver.resolveComponentFactory(ModalMasterComponent);
    let componentRef = this.viewContainerRef.createComponent(componentFactory);

    componentRef.instance.TableIndex = table;

    componentRef.instance.Closing.subscribe(() => {

      if (componentRef.instance.ModalStatus == MODAL_STATUS_TYPE.SELECT) {
        switch (table) {

          case TABLE_INDEX.MASTER_BILLING_CATEGORY:
            {
              this.billingCategoryCodeCtrl.setValue(componentRef.instance.SelectedCode);
              this.billingCategoryNameCtrl.setValue(componentRef.instance.SelectedName);
              this.billingCategoryId = componentRef.instance.SelectedId;
              break;
            }
          case TABLE_INDEX.MASTER_COLLECT_CATEGORY:
            {
              this.collectCategoryCodeCtrl.setValue(componentRef.instance.SelectedCode);
              this.collectCategoryNameCtrl.setValue(componentRef.instance.SelectedName);
              this.collectCategoryId = componentRef.instance.SelectedId;
              break;
            }
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
          case TABLE_INDEX.MASTER_LOGIN_USER:
            {
              this.updateByCodeCtrl.setValue(componentRef.instance.SelectedCode);
              this.updateByNameCtrl.setValue(componentRef.instance.SelectedName);
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
            }
        }
      }

      componentRef.destroy();
    });
  }

  public clear() {
    this.MyFormGroup.reset();
    this.billingCategoryId = null;
    this.collectCategoryId = null;

    this.deletedAtFromCtrl.disable();  // 削除日
    this.deletedAtToCtrl.disable();

    this.cbxFullAssignmentCtrl.disable();
    this.cbxPartAssignmentCtrl.setValue("true");
    this.cbxNoAssignmentCtrl.setValue("true");

    this.inputTypeCtrl.setValue(BILL_INPUT_TYPE_DICTIONARY[0].id);
    this.amountRangeCtrl.setValue(this.amountRangeDictionary[0]);

    this.processCustomResult = this.processResultService.clearProcessCustomMsg(this.processCustomResult);

    this.deleteOrRecoveryButton = this.ItemNameConst.BTN_DELETE;

    this.selectedDelete = false;

    /*
    this.billingsResult = null;
    this.dispSumCount = 0;
    this.dispSumBillingAmount = 0;
    this.dispSumRemainAmount = 0;
    */
   
    this.panelOpenState = true;
    this.panel.open();
    
    this.cbxDeletedFlagCtrl.enable();

    HtmlUtil.nextFocusByName(this.elementRef, 'billedAtFromCtrl', EVENT_TYPE.NONE);

    this.setRangeCheckbox();
  }

  public setRangeCheckbox() {
    let cbxDepartment = this.localStorageManageService.get(RangeSearchKey.PE0501_DEPARTMENT);
    let cbxStaff = this.localStorageManageService.get(RangeSearchKey.PE0501_STAFF);
    let cbxCustomer = this.localStorageManageService.get(RangeSearchKey.PE0501_CUSTOMER);
    let cbxUseReceiptSection = this.localStorageManageService.get(RangeSearchKey.PE0501_USE_RECEIPT_SECTION);
    let cbxAmount = this.localStorageManageService.get(RangeSearchKey.PE0501_AMOUNT);

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

    if (cbxAmount != null) {
      this.cbxAmountCtrl.setValue(cbxAmount.value);
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
      case BUTTON_ACTION.DELETE:
        this.deleteOrRecovery();
        break;
      case BUTTON_ACTION.EXPORT:
        this.output();
        break;
      default:
        console.log('buttonAction Error.');
        break;
    }

  }


  public search() {

    this.processCustomResult = this.processResultService.clearProcessCustomMsg(this.processCustomResult);

    if (!this.RequiredChecking()) return;

    this.billingSearch = this.SearchCondition();
    this.SearchBillingData();

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
    this.processCustomResult = this.processResultService.clearProcessCustomMsg(this.processCustomResult);

    if (this.billingsResult == null) return;

    for (let index = 0; index < this.billingsResult.billings.length; index++) {
      this.cbxDetailDelFlagCtrls[index].setValue("true");
    }
    this.selectedDelete = true;
  }

  public clearAll() {
    this.processCustomResult = this.processResultService.clearProcessCustomMsg(this.processCustomResult);

    if (this.billingsResult == null) return;

    for (let index = 0; index < this.billingsResult.billings.length; index++) {
      this.cbxDetailDelFlagCtrls[index].setValue(null);
    }
    this.selectedDelete = false;

  }

  public deleteOrRecovery() {
    let msg = null;
    this.processCustomResult = this.processResultService.clearProcessCustomMsg(this.processCustomResult);

    if (!this.CheckForDeleteOrRecovery()) return;

    // 削除対象の取得
    let transaction = new Array<Transaction>();
    for (let index = 0; index < this.dispSumCount; index++) {
      if (this.cbxDetailDelFlagCtrls[index].value) {
        let tmpTran: Transaction = new Transaction();
        tmpTran.id = this.billingsResult.billings[index].id;
        tmpTran.updateAt = this.billingsResult.billings[index].updateAt;
        transaction.push(tmpTran);
      }
    }

    // 削除処理
    let componentFactory = this.componentFactoryResolver.resolveComponentFactory(ModalConfirmComponent);
    let componentRef = this.viewContainerRef.createComponent(componentFactory);

    componentRef.instance.ActionName = this.deleteOrRecoveryButton;
    componentRef.instance.Closing.subscribe(() => {

      if (componentRef.instance.ModalStatus == MODAL_STATUS_TYPE.OK) {
        let omitSource = new OmitSource();

        let componentFactory = this.componentFactoryResolver.resolveComponentFactory(ModalRouterProgressComponent);
        let modalRouterProgressComponentRef = this.viewContainerRef.createComponent(componentFactory);
        //modalRouterProgressComponentRef.destroy();

        omitSource.loginUserId = this.userInfoService.LoginUser.id;
        if (this.cbxDeletedFlagCtrl.value) {
          // 削除されたものを検索しているので、復元のためFalseにする。
          omitSource.doDelete = false;
        }
        else {
          // 削除されていないものを検索しているので、削除のためFalseにする。
          omitSource.doDelete = true;
        }
        omitSource.transactions = transaction;

        this.billingService.Omit(omitSource)
          .subscribe(response => {
            if (response != PROCESS_RESULT_RESULT_TYPE.FAILURE) {
              let countResult: CountResult = response;

              if (countResult.processResult.result == true) {
                msg = "請求データの" + this.deleteOrRecoveryButton + "が完了しました。"
                this.processCustomResult = this.processResultService.processAtSuccess(
                  this.processCustomResult, msg, this.partsResultMessageComponent);
                this.SearchBillingData();
              }
              else {
                msg = "請求データの" + this.deleteOrRecoveryButton + "に失敗しました。"
                this.processCustomResult = this.processResultService.processAtFailure(
                  this.processCustomResult, msg, this.partsResultMessageComponent);
              }

            }
            else {
              msg = "請求データの" + this.deleteOrRecoveryButton + "に失敗しました。"
              this.processCustomResult = this.processResultService.processAtFailure(
                this.processCustomResult, msg, this.partsResultMessageComponent);
            }

            modalRouterProgressComponentRef.destroy();            

          });
      }
      else {
        this.processCustomResult = this.processResultService.processAtWarning(
          this.processCustomResult, MSG_WNG.PROCESS_CANCELED, this.partsResultMessageComponent);
      }
      componentRef.destroy();
    });


  }

  public CheckForDeleteOrRecovery(): boolean {
    // 締め処理の確認
    return true;
  }



  public print() {

    this.processCustomResult = this.processResultService.clearProcessCustomMsg(this.processCustomResult);

    if (!this.RequiredChecking()) return;

    let componentFactory = this.componentFactoryResolver.resolveComponentFactory(ModalRouterProgressComponent);
    let componentRef = this.viewContainerRef.createComponent(componentFactory);
    componentRef.instance.Closing.subscribe(() => {
      componentRef.destroy();
    });


    this.billingSearch = this.SearchCondition();

    //this.billingSearch.isDeleted=true;

    this.billingService.GetReport(this.billingSearch)
      .subscribe(response => {
        FileUtil.download([response.body], this.Title + DateUtil.getYYYYMMDD(0), FILE_EXTENSION.PDF);
        componentRef.destroy();
      });


  }


  public output() {
    this.processCustomResult = this.processResultService.clearProcessCustomMsg(this.processCustomResult);

    let data: string = "";
    let dataList = this.billingsResult.billings;

    let componentFactory = this.componentFactoryResolver.resolveComponentFactory(ModalRouterProgressComponent);
    let modalRouterProgressComponentRef = this.viewContainerRef.createComponent(componentFactory);
    //modalRouterProgressComponentRef.destroy();

    let headerData = FileUtil.encloseItemBySymbol(this.gridSettingsResult.gridSettings.map(gridSetting => { return gridSetting.columnNameJp }));
    if (this.cbxDeletedFlagCtrl.value) {
      headerData = ['削除日'].concat(headerData);
    }
    data = data + headerData.join(",") + LINE_FEED_CODE;

    for (let index = 0; index < dataList.length; index++) {
      let dataItem: Array<any> = [];
      if (this.cbxDeletedFlagCtrl.value) {
        dataItem.push(DateUtil.convertDateString(dataList[index].deleteAt));
      }

      this.gridSettingsResult.gridSettings.forEach(gridSetting => {
        switch (gridSetting.columnName) {
          case 'Id':
            dataItem.push(dataList[index].id);
            break;
          case 'AssignmentState':
            dataItem.push(this.MATCHING_ASSIGNMENT_FLAG_DICTIONARY[dataList[index].assignmentFlag].val);
            break;
          case 'CustomerCode':
            dataItem.push(dataList[index].customerCode);
            break;
          case 'CustomerName':
            dataItem.push(dataList[index].customerName);
            break;
          case 'BilledAt':
            dataItem.push(DateUtil.convertDateString(dataList[index].billedAt));
            break;
          case 'SalesAt':
            dataItem.push(DateUtil.convertDateString(dataList[index].salesAt));
            break;
          case 'ClosingAt':
            dataItem.push(DateUtil.convertDateString(dataList[index].closingAt));
            break;
          case 'DueAt':
            dataItem.push(DateUtil.convertDateString(dataList[index].dueAt));
            break;
          case 'CurrencyCode':
            dataItem.push(dataList[index].currencyCode);
            break;
          case 'BillingAmount':
            dataItem.push(dataList[index].billingAmount);
            break;
          case 'RemainAmount':
            dataItem.push(dataList[index].remainAmount);
            break;
          case 'InvoiceCode':
            dataItem.push(dataList[index].invoiceCode);
            break;
          case 'BillingCategory':
            dataItem.push(dataList[index].billingCategoryCode + ':' + dataList[index].billingCategoryName);
            break;
          case 'CollectCategory':
            dataItem.push(dataList[index].collectCategoryCode + ':' + dataList[index].collectCategoryName);
            break;
          case 'InputType':
            dataItem.push(this.BILL_INPUT_TYPE_DICTIONARY[dataList[index].inputType].val);
            break;
          case 'Note1':
            dataItem.push(dataList[index].note1);
            break;
          case 'Memo':
            dataItem.push(dataList[index].memo);
            break;
          case 'DepartmentCode':
            dataItem.push(dataList[index].departmentCode);
            break;
          case 'DepartmentName':
            dataItem.push(dataList[index].departmentName);
            break;
          case 'StaffCode':
            dataItem.push(dataList[index].staffCode);
            break;
          case 'StaffName':
            dataItem.push(dataList[index].staffName);
            break;
          case 'ContractNumber':
            dataItem.push(dataList[index].contractNumber);
            break;
          case 'Confirm':
            dataItem.push(dataList[index].confirm);
            break;
          case 'RequestDate':
            dataItem.push(dataList[index].requestDate != undefined ? DateUtil.convertDateString(dataList[index].requestDate) : '');
            break;
          case 'ResultCode':
            dataItem.push(this.getBankTransferResult(dataList[index].resultCode));
            break;
          case 'Note2':
            dataItem.push(dataList[index].note2);
            break;
          case 'Note3':
            dataItem.push(dataList[index].note3);
            break;
          case 'Note4':
            dataItem.push(dataList[index].note4);
            break;
          case 'DiscountAmount1':
            dataItem.push(dataList[index].discountAmount1);
            break;
          case 'DiscountAmount2':
            dataItem.push(dataList[index].discountAmount2);
            break;
          case 'DiscountAmount3':
            dataItem.push(dataList[index].discountAmount3);
            break;
          case 'DiscountAmount4':
            dataItem.push(dataList[index].discountAmount4);
            break;
          case 'DiscountAmount5':
            dataItem.push(dataList[index].discountAmount5);
            break;
          case 'DiscountAmountSummary':
            dataItem.push(dataList[index].discountAmount);
            break;
          case 'FirstRecordedAt':
            dataItem.push(dataList[index].firstRecordedAt != undefined ? DateUtil.convertDateString(dataList[index].firstRecordedAt) : '');
            break;
          case 'LastRecordedAt':
            dataItem.push(dataList[index].lastRecordedAt != undefined ? DateUtil.convertDateString(dataList[index].lastRecordedAt) : '');
            break;
          case 'Price':
            dataItem.push(dataList[index].billingAmount - dataList[index].taxAmount);
            break;
          case 'TaxAmount':
            dataItem.push(dataList[index].taxAmount);
            break;
          case 'Note5':
            dataItem.push(dataList[index].note5);
            break;
          case 'Note6':
            dataItem.push(dataList[index].note6);
            break;
          case 'Note7':
            dataItem.push(dataList[index].note7);
            break;
          case 'Note8':
            dataItem.push(dataList[index].note8);
            break;
        }
      });
      
      dataItem = FileUtil.encloseItemBySymbol(dataItem);
      data = data + dataItem.join(",") + LINE_FEED_CODE;
    }

    let result: boolean = false;
    let resultDatas: Array<any> = [];
    resultDatas.push(data);

    FileUtil.download(resultDatas, "未消込請求データ削除" + DateUtil.getYYYYMMDD(0), FILE_EXTENSION.CSV);
    result = true;
    this.processResultService.processAtOutput(
      this.processCustomResult, result, 0, this.partsResultMessageComponent);    

    modalRouterProgressComponentRef.destroy();
  }




  public RequiredChecking(): boolean {

    if (!this.cbxPartAssignmentCtrl.value && !this.cbxNoAssignmentCtrl.value) {
      this.processCustomResult = this.processResultService.processAtWarning(
        this.processCustomResult, MSG_WNG.SELECTION_REQUIRED.replace(MSG_ITEM_NUM.FIRST, '消込区分'),
        this.partsResultMessageComponent);
      return false;
    }

    return true;

  }

  public SearchBillingData() {


    let componentFactory = this.componentFactoryResolver.resolveComponentFactory(ModalRouterProgressComponent);
    let modalRouterProgressComponentRef = this.viewContainerRef.createComponent(componentFactory);
    //modalRouterProgressComponentRef.destroy();    

    this.billingService.GetItems(this.billingSearch)
      .subscribe(response => {

        if (response != PROCESS_RESULT_RESULT_TYPE.FAILURE) {
          this.billingsResult = new BillingsResult();

          this.billingsResult.billings = response;

          this.billingsResult.billings.filter(element => {
            if (element.billingInputPublishAt == null && element.inputType != BillingInputType.Factoring) {
              return true;
            }
            else {
              return false;
            }
          });

          this.dispSumCount = this.billingsResult.billings.length;

          this.billingsResult.billings.forEach(element => {
            this.dispSumBillingAmount += element.billingAmount;
            this.dispSumRemainAmount += element.remainAmount;
          });

          this.cbxDetailDelFlagCtrls = new Array<FormControl>(this.dispSumCount);

          for (let index = 0; index < this.dispSumCount; index++) {
            this.cbxDetailDelFlagCtrls[index] = new FormControl(null);
            this.MyFormGroup.removeControl("cbxDetailDelFlagCtrl" + index);
            this.MyFormGroup.addControl("cbxDetailDelFlagCtrl" + index, this.cbxDetailDelFlagCtrls[index]);
          }

          if (this.billingsResult.billings.length == 0) {
            this.panelOpenState = true;
            this.cbxDeletedFlagCtrl.enable();

            this.processCustomResult = this.processResultService.processAtWarning(
              this.processCustomResult, MSG_WNG.NOT_EXIST_SEARCH_DATA, this.partsResultMessageComponent);
          }
          else {
            this.panelOpenState = false;
            this.cbxDeletedFlagCtrl.disable();

            this.processCustomResult = this.processResultService.processAtSuccess(
              this.processCustomResult, MSG_INF.PROCESS_FINISH, this.partsResultMessageComponent);

            this.processResultService.createdLog(this.processCustomResult.logData);
          }
        }
        else {
          this.panelOpenState = true;
          this.processCustomResult = this.processResultService.processAtFailure(
            this.processCustomResult, MSG_ERR.SOMETHING_ERROR.replace(MSG_ITEM_NUM.FIRST, '検索'),
            this.partsResultMessageComponent);
        }
        modalRouterProgressComponentRef.destroy();  
      });
  }

  public getGridSetting(columnName: string): string {

    if (this.AllGridSettings == undefined) return "";

    let columnSetting = this.AllGridSettings.find(x => x.columnName == columnName);
    return columnSetting == undefined ? "" : columnSetting.columnNameJp;
  }

  public getBankTransferResult(id: any): string {
    if (id == undefined) return "";
    return this.BANK_TRANSFER_RESULT_DICTIONARY[id].val;
  }

  public SearchCondition(): BillingSearch {
    var billingItem = new BillingSearch();


    if(this.simplePageSearch){
      billingItem.companyId = this.userInfoService.Company.id;
      billingItem.loginUserId = this.userInfoService.LoginUser.id;

      billingItem.bsBilledAtFrom = DateUtil.ConvertFromDatepicker(this.billedAtFromCtrl);  //  請求日
      billingItem.bsBilledAtTo = DateUtil.ConvertFromDatepicker(this.billedAtToCtrl);

      billingItem.bsSalesAtFrom = DateUtil.ConvertFromDatepicker(this.salesAtFromCtrl);  //  売上日
      billingItem.bsSalesAtTo = DateUtil.ConvertFromDatepicker(this.salesAtToCtrl);

      billingItem.bsDueAtFrom = DateUtil.ConvertFromDatepicker(this.dueAtFromCtrl);  //  入金予定日
      billingItem.bsDueAtTo = DateUtil.ConvertFromDatepicker(this.dueAtToCtrl);

      billingItem.bsInvoiceCode = this.invoiceCodeCtrl.value;  //  請求書番号

      billingItem.bsCustomerName = this.customerNameCtrl.value; //  得意先名称

      // 消込・未消込・一部消込
      // 2進数
      // 消込済み；0x004 = 4
      // 一部	；0x002 = 2
      // 未消込	；0x001 = 1
      billingItem.assignmentFlg = 0;
      if (this.cbxFullAssignmentCtrl.value) {
        billingItem.assignmentFlg = billingItem.assignmentFlg + 4;
      }
      if (this.cbxPartAssignmentCtrl.value) {
        billingItem.assignmentFlg = billingItem.assignmentFlg + 2;
      }
      if (this.cbxNoAssignmentCtrl.value) {
        billingItem.assignmentFlg = billingItem.assignmentFlg + 1;
      }

      this.amountRangeCtrl.value;  // 金額範囲
      if (this.amountRangeCtrl.value.id == this.amountRangeDictionary[0].id) {
        billingItem.bsBillingAmountFrom = this.amountFromCtrl.value;
        billingItem.bsBillingAmountTo = this.amountToCtrl.value;
      }
      else if (this.amountRangeCtrl.value.id == this.amountRangeDictionary[1].id) {
        billingItem.bsRemaingAmountFrom = this.amountFromCtrl.value;
        billingItem.bsRemaingAmountTo = this.amountToCtrl.value;
      }
      else {
        billingItem.bsBillingAmountFrom = 0;
        billingItem.bsBillingAmountTo = 0;
      }
    }
    else{
      billingItem.companyId = this.userInfoService.Company.id;
      billingItem.loginUserId = this.userInfoService.LoginUser.id;
  
      billingItem.bsInputType = this.inputTypeCtrl.value;
  
      billingItem.bsBilledAtFrom = DateUtil.ConvertFromDatepicker(this.billedAtFromCtrl);
      billingItem.bsBilledAtTo = DateUtil.ConvertFromDatepicker(this.billedAtToCtrl);
  
      billingItem.bsDueAtFrom = DateUtil.ConvertFromDatepicker(this.dueAtFromCtrl);
      billingItem.bsDueAtTo = DateUtil.ConvertFromDatepicker(this.dueAtToCtrl);
  
      billingItem.bsSalesAtFrom = DateUtil.ConvertFromDatepicker(this.salesAtFromCtrl);
      billingItem.bsSalesAtTo = DateUtil.ConvertFromDatepicker(this.salesAtToCtrl);
  
      billingItem.bsDeleteAtFrom = DateUtil.ConvertFromDatepickerToStart(this.deletedAtFromCtrl);
      billingItem.bsDeleteAtTo = DateUtil.ConvertFromDatepickerToEnd(this.deletedAtToCtrl);
  
      if (!StringUtil.IsNullOrEmpty(this.invoiceCodeFromCtrl.value)) {
        billingItem.bsInvoiceCodeFrom = this.invoiceCodeFromCtrl.value;
      }
  
      if (!StringUtil.IsNullOrEmpty(this.invoiceCodeToCtrl.value)) {
        billingItem.bsInvoiceCodeTo = this.invoiceCodeToCtrl.value;
      }
  
      if (!StringUtil.IsNullOrEmpty(this.invoiceCodeCtrl.value)) {
        billingItem.bsInvoiceCode = this.invoiceCodeCtrl.value;
      }
  
      billingItem.bsUpdateAtFrom = DateUtil.ConvertFromDatepickerToStart(this.updateAtFromCtrl);
      billingItem.bsUpdateAtTo = DateUtil.ConvertFromDatepickerToEnd(this.updateAtToCtrl);
  
      billingItem.loginUserCode = this.updateByCodeCtrl.value;
      billingItem.userId = this.updateByCodeCtrl.value;
  
      if (this.cbxMemoCtrl.value) {
        billingItem.existsMemo = true;
      }
  
      if (!StringUtil.IsNullOrEmpty(this.memoCtrl.value)) {
        billingItem.bsMemo = this.memoCtrl.value;
      }
  
      billingItem.bsBillingCategoryId = this.billingCategoryId;
      billingItem.collectCategoryId = this.collectCategoryId;
  
      if (!StringUtil.IsNullOrEmpty(this.departmentCodeFromCtrl.value)) {
        billingItem.bsDepartmentCodeFrom = this.departmentCodeFromCtrl.value;
      }
  
      if (!StringUtil.IsNullOrEmpty(this.departmentCodeToCtrl.value)) {
        billingItem.bsDepartmentCodeTo = this.departmentCodeToCtrl.value;
      }
  
      if (!StringUtil.IsNullOrEmpty(this.staffCodeFromCtrl.value)) {
        billingItem.bsStaffCodeFrom = this.staffCodeFromCtrl.value;
      }
  
      if (!StringUtil.IsNullOrEmpty(this.staffCodeToCtrl.value)) {
        billingItem.bsStaffCodeTo = this.staffCodeToCtrl.value;
      }
  
      if (!StringUtil.IsNullOrEmpty(this.customerCodeFromCtrl.value)) {
        billingItem.bsCustomerCodeFrom = this.customerCodeFromCtrl.value;
      }
  
      if (!StringUtil.IsNullOrEmpty(this.customerCodeToCtrl.value)) {
        billingItem.bsCustomerCodeTo = this.customerCodeToCtrl.value;
      }
  
      if (!StringUtil.IsNullOrEmpty(this.customerNameCtrl.value)) {
        billingItem.bsCustomerName = this.customerNameCtrl.value;
      }
  
      if (!StringUtil.IsNullOrEmpty(this.customerKanaCtrl.value)) {
        billingItem.bsCustomerNameKana = this.customerKanaCtrl.value;
      }
  
      if (this.cbxUseReceiptSectionCtrl.value) {
        billingItem.useSectionMaster = true;
      }
      else {
        billingItem.useSectionMaster = false;
      }
  
      billingItem.currencyId = this.userInfoService.Currency.id;
  
      // 消込・未消込・一部消込
      // 2進数
      // 消込済み；0x004 = 4
      // 一部	；0x002 = 2
      // 未消込	；0x001 = 1
      billingItem.assignmentFlg = 0;
      if (this.cbxFullAssignmentCtrl.value) {
        billingItem.assignmentFlg = billingItem.assignmentFlg + 4;
      }
      if (this.cbxPartAssignmentCtrl.value) {
        billingItem.assignmentFlg = billingItem.assignmentFlg + 2;
      }
      if (this.cbxNoAssignmentCtrl.value) {
        billingItem.assignmentFlg = billingItem.assignmentFlg + 1;
      }
  
      if (this.amountRangeCtrl.value.id == this.amountRangeDictionary[0].id) {
        billingItem.bsBillingAmountFrom = this.amountFromCtrl.value;
        billingItem.bsBillingAmountTo = this.amountToCtrl.value;
      }
      else {
        billingItem.bsRemaingAmountFrom = this.amountFromCtrl.value;
        billingItem.bsRemaingAmountTo = this.amountToCtrl.value;
  
      }
  
      billingItem.bsNote1 = this.note1Ctrl.value;
      billingItem.bsNote2 = this.note2Ctrl.value;
      billingItem.bsNote3 = this.note3Ctrl.value;
      billingItem.bsNote4 = this.note4Ctrl.value;
      billingItem.bsNote5 = this.note5Ctrl.value;
      billingItem.bsNote6 = this.note6Ctrl.value;
      billingItem.bsNote7 = this.note7Ctrl.value;
      billingItem.bsNote8 = this.note8Ctrl.value;
  
      if (this.cbxDeletedFlagCtrl.value) {
        billingItem.isDeleted = true;
      }
  

    }



    return billingItem;
  }



  /////////////////////////////////////////////////////////////////
  public setBilledAtFrom(eventType: string) {
    HtmlUtil.nextFocusByName(this.elementRef, 'billedAtToCtrl', eventType);
  }

  public setBilledAtTo(eventType: string) {
    HtmlUtil.nextFocusByName(this.elementRef, 'salesAtFromCtrl', eventType);
  }

  /////////////////////////////////////////////////////////////////
  public setSalesAtFrom(eventType: string) {
    HtmlUtil.nextFocusByName(this.elementRef, 'salesAtToCtrl', eventType);
  }

  public setSalesAtTo(eventType: string) {
    HtmlUtil.nextFocusByName(this.elementRef, 'dueAtFromCtrl', eventType);
  }

  /////////////////////////////////////////////////////////////////
  public setDueAtFrom(eventType: string) {
    HtmlUtil.nextFocusByName(this.elementRef, 'dueAtToCtrl', eventType);
  }

  public setDueAtTo(eventType: string) {
    HtmlUtil.nextFocusByNames(this.elementRef, ['deletedAtFromCtrl', 'invoiceCodeFromCtrl','invoiceCodeCtrl'], eventType);
  }

  /////////////////////////////////////////////////////////////////
  public setDeletedAtFrom(eventType: string) {
    HtmlUtil.nextFocusByName(this.elementRef, 'deletedAtToCtrl', eventType);
  }

  public setDeletedAtTo(eventType: string) {
    HtmlUtil.nextFocusByName(this.elementRef, 'invoiceCodeFromCtrl', eventType);
  }

  /////////////////////////////////////////////////////////////////
  public setInvoiceCodeFrom(eventType: string) {
    HtmlUtil.nextFocusByName(this.elementRef, 'invoiceCodeToCtrl', eventType);
  }

  public setInvoiceCodeTo(eventType: string) {
    HtmlUtil.nextFocusByName(this.elementRef, 'invoiceCodeCtrl', eventType);
  }

  public setInvoiceCode(eventType: string) {
    HtmlUtil.nextFocusByNames(this.elementRef, ['updateAtFromCtrl','customerNameCtrl'], eventType);
  }

  public setUpdateAtFrom(eventType: string) {
    HtmlUtil.nextFocusByName(this.elementRef, 'updateAtToCtrl', eventType);
  }

  public setUpdateAtTo(eventType: string) {
    HtmlUtil.nextFocusByName(this.elementRef, 'updateByCodeCtrl', eventType);
  }

  /////////////////////////////////////////////////////////////////////////////////

  public setUpdateByCode(eventType: string) {

    if(eventType!=EVENT_TYPE.BLUR){
      this.updateByCodeTrigger.closePanel();
    }
    
    if (!StringUtil.IsNullOrEmpty(this.updateByCodeCtrl.value)) {

      this.loadStart();
      this.loginUserService.GetItems(this.updateByCodeCtrl.value)
        .subscribe(response => {
          this.loadEnd();
          if (response != undefined && response.length > 0) {

            this.updateByCodeCtrl.setValue(response[0].code);
            this.updateByNameCtrl.setValue(response[0].name);
            HtmlUtil.nextFocusByName(this.elementRef, 'memoCtrl', eventType);

          }
          else {
            // this.updateByCodeCtrl.setValue("");
            this.updateByNameCtrl.setValue("");
            HtmlUtil.nextFocusByName(this.elementRef, 'memoCtrl', eventType);
          }
        });

    }
    else {
      this.updateByCodeCtrl.setValue("");
      this.updateByNameCtrl.setValue("");
      HtmlUtil.nextFocusByName(this.elementRef, 'memoCtrl', eventType);
    }

  }

  ///////////////////////////////////////////////////////////////////////
  public setCbxMemo(eventType: string) {
    if (this.cbxMemoCtrl.value == true) {
      HtmlUtil.nextFocusByName(this.elementRef, 'memoCtrl', eventType);
    }
    else {
      this.memoCtrl.setValue("");
      HtmlUtil.nextFocusByName(this.elementRef, 'billingCategoryCodeCtrl', eventType);
    }
  }

  public setMemo(eventType: string) {
    HtmlUtil.nextFocusByName(this.elementRef, 'billingCategoryCodeCtrl', eventType);
  }

  /////////////////////////////////////////////////////////////////////////////////

  public setBillingCategoryCode(eventType: string) {

    if(eventType!=EVENT_TYPE.BLUR){
      this.billingCategoryCodeTrigger.closePanel();
    }

    let code = this.billingCategoryCodeCtrl.value as string;
    if (!StringUtil.IsNullOrEmpty(code)) {
      code = StringUtil.setPaddingFrontZero(code, 2);
      this.loadStart();
      this.categoryService.GetItems(CategoryType.Billing, code)
        .subscribe(response => {
          this.loadEnd();
          if (response != undefined && response.length > 0) {

            this.billingCategoryCodeCtrl.setValue(response[0].code);
            this.billingCategoryNameCtrl.setValue(response[0].name);
            this.billingCategoryId = response[0].id;
            HtmlUtil.nextFocusByName(this.elementRef, 'collectCategoryCodeCtrl', eventType);

          }
          else {
            // this.billingCategoryCodeCtrl.setValue("");
            this.billingCategoryNameCtrl.setValue("");
            this.billingCategoryId = null;
            HtmlUtil.nextFocusByName(this.elementRef, 'collectCategoryCodeCtrl', eventType);
          }
        });

    }
    else {
      // this.billingCategoryCodeCtrl.setValue("");
      this.billingCategoryNameCtrl.setValue("");
      this.billingCategoryId = null;
      HtmlUtil.nextFocusByName(this.elementRef, 'collectCategoryCodeCtrl', eventType);
    }

  }

  /////////////////////////////////////////////////////////////////////////////////

  public setCollectCategoryCode(eventType: string) {

    if(eventType!=EVENT_TYPE.BLUR){
      this.collectCategoryCodeTrigger.closePanel();
    }
    
    let code = this.collectCategoryCodeCtrl.value as string;
    if (!StringUtil.IsNullOrEmpty(code)) {
      code = StringUtil.setPaddingFrontZero(code, 2);
      this.loadStart();
      this.categoryService.GetItems(CategoryType.Collection, code)
        .subscribe(response => {
          this.loadEnd();
          if (response != undefined && response.length > 0) {

            this.collectCategoryCodeCtrl.setValue(response[0].code);
            this.collectCategoryNameCtrl.setValue(response[0].name);
            this.collectCategoryId = response[0].id;
            HtmlUtil.nextFocusByName(this.elementRef, 'inputTypeCtrl', eventType);

          }
          else {
            // this.collectCategoryCodeCtrl.setValue("");
            this.collectCategoryNameCtrl.setValue("");
            this.collectCategoryId = null;
            HtmlUtil.nextFocusByName(this.elementRef, 'inputTypeCtrl', eventType);
          }
        });

    }
    else {
      // this.collectCategoryCodeCtrl.setValue("");
      this.collectCategoryNameCtrl.setValue("");
      this.collectCategoryId = null;
      HtmlUtil.nextFocusByName(this.elementRef, 'inputTypeCtrl', eventType);
    }

  }

  ///////////////////////////////////////////////////////////////////////
  public setInputType(eventType: string) {
    HtmlUtil.nextFocusByName(this.elementRef, 'departmentCodeFromCtrl', eventType);
  }

  /////////////////////////////////////////////////////////////////////////////////

  public setDepartmentCodeFrom(eventType: string) {

    if(eventType!=EVENT_TYPE.BLUR){
      this.departmentCodeFromigger.closePanel();
    }

    if (!StringUtil.IsNullOrEmpty(this.departmentCodeFromCtrl.value)) {

      this.loadStart();
      this.departmentService.GetItems(this.departmentCodeFromCtrl.value)
        .subscribe(response => {
          this.loadEnd();
          if (response != undefined && response.length > 0) {
            this.departmentCodeFromCtrl.setValue(response[0].code);
            this.departmentNameFromCtrl.setValue(response[0].name);
          }
          else {
            this.departmentNameFromCtrl.setValue("");
          }
          if (this.cbxDepartmentCtrl.value) {
            this.departmentCodeToCtrl.setValue(this.departmentCodeFromCtrl.value);
            this.departmentNameToCtrl.setValue(this.departmentNameFromCtrl.value);
          }
      });
      }
      else {
        // this.departmentCodeFromCtrl.setValue("");
        this.departmentNameFromCtrl.setValue("");
      }
      if (this.cbxDepartmentCtrl.value) {
        this.departmentCodeToCtrl.setValue(this.departmentCodeFromCtrl.value);
        this.departmentNameToCtrl.setValue(this.departmentNameFromCtrl.value);
      }
    HtmlUtil.nextFocusByName(this.elementRef, 'departmentCodeToCtrl', eventType);
  }

  public setDepartmentCodeTo(eventType: string) {

    if(eventType!=EVENT_TYPE.BLUR){
      this.departmentCodeToigger.closePanel();
    }

    if (!StringUtil.IsNullOrEmpty(this.departmentCodeToCtrl.value)) {

      this.loadStart();
      this.departmentService.GetItems(this.departmentCodeToCtrl.value)
        .subscribe(response => {
          this.loadEnd();
          if (response != undefined && response.length > 0) {
            this.departmentCodeToCtrl.setValue(response[0].code);
            this.departmentNameToCtrl.setValue(response[0].name);
          }
          else {
            this.departmentNameToCtrl.setValue("");
          }
        });
    }
    else {
      // this.departmentCodeToCtrl.setValue("");
      this.departmentNameToCtrl.setValue("");
    }
    HtmlUtil.nextFocusByName(this.elementRef, 'staffCodeFromCtrl', eventType);
  }

  public setCbxDepartment(eventType: string) {
    let localstorageItem = new LocalStorageItem();
    localstorageItem.key = RangeSearchKey.PE0501_DEPARTMENT;
    localstorageItem.value = this.cbxDepartmentCtrl.value;
    this.localStorageManageService.set(localstorageItem);

    HtmlUtil.nextFocusByName(this.elementRef, "departmentCodeFromCtrl", eventType);

  }  

  /////////////////////////////////////////////////////////////////////////////////

  public setStaffCodeFrom(eventType: string) {

    if(eventType!=EVENT_TYPE.BLUR){
      this.staffCodeFromTrigger.closePanel();
    }

    if (!StringUtil.IsNullOrEmpty(this.staffCodeFromCtrl.value)) {

      this.loadStart();
      this.staffService.GetItems(this.staffCodeFromCtrl.value)
        .subscribe(response => {
          this.loadEnd();
          if (response != undefined && response.length > 0) {
            this.staffCodeFromCtrl.setValue(response[0].code);
            this.staffNameFromCtrl.setValue(response[0].name);
          }
          else {
            this.staffNameFromCtrl.setValue("");
          }
          if (this.cbxStaffCtrl.value) {
            this.staffCodeToCtrl.setValue(this.staffCodeFromCtrl.value);
            this.staffNameToCtrl.setValue(this.staffNameFromCtrl.value);
          }
      });
    }
    else {
      // this.staffCodeFromCtrl.setValue("");
      this.staffNameFromCtrl.setValue("");
    }
    if (this.cbxStaffCtrl.value) {
      this.staffCodeToCtrl.setValue(this.staffCodeFromCtrl.value);
      this.staffNameToCtrl.setValue(this.staffNameFromCtrl.value);
    }
    HtmlUtil.nextFocusByName(this.elementRef, 'staffCodeToCtrl', eventType);
  }

  public setStaffCodeTo(eventType: string) {

    if(eventType!=EVENT_TYPE.BLUR){
      this.staffCodeToTrigger.closePanel();
    }

    if (!StringUtil.IsNullOrEmpty(this.staffCodeToCtrl.value)) {

      this.loadStart();
      this.staffService.GetItems(this.staffCodeToCtrl.value)
        .subscribe(response => {
          this.loadEnd();
          if (response != undefined && response.length > 0) {
            this.staffCodeToCtrl.setValue(response[0].code);
            this.staffNameToCtrl.setValue(response[0].name);
          }
          else {
            this.staffNameToCtrl.setValue("");
          }
        });
    }
    else {
      // this.staffCodeToCtrl.setValue("");
      this.staffNameToCtrl.setValue("");
    }
    HtmlUtil.nextFocusByName(this.elementRef, 'customerCodeFromCtrl', eventType);

  }

  public setCbxStaff(eventType: string) {
    let localstorageItem = new LocalStorageItem();
    localstorageItem.key = RangeSearchKey.PE0501_STAFF;
    localstorageItem.value = this.cbxStaffCtrl.value;
    this.localStorageManageService.set(localstorageItem);

    HtmlUtil.nextFocusByName(this.elementRef, "staffCodeFromCtrl", eventType);

  } 
  

  /////////////////////////////////////////////////////////////////////////////////

  public setCustomerCodeFrom(eventType: string) {

    if(eventType!=EVENT_TYPE.BLUR){
      this.customerCodeFromTrigger.closePanel();
    }

    if (!StringUtil.IsNullOrEmpty(this.customerCodeFromCtrl.value)) {

      this.loadStart();
      this.customerService.GetItems(this.customerCodeFromCtrl.value)
        .subscribe(response => {
          this.loadEnd();
          if (response != undefined && response.length > 0) {
            this.customerCodeFromCtrl.setValue(response[0].code);
            this.customerNameFromCtrl.setValue(response[0].name);
          }
          else {
            this.customerNameFromCtrl.setValue("");
          }
          if (this.cbxCustomerCtrl.value) {
            this.customerCodeToCtrl.setValue(this.customerCodeFromCtrl.value);
            this.customerNameToCtrl.setValue(this.customerNameFromCtrl.value);
          }
      });
    }
    else {
      this.customerCodeFromCtrl.setValue("");
      this.customerNameFromCtrl.setValue("");
    }
    HtmlUtil.nextFocusByName(this.elementRef, 'customerCodeToCtrl', eventType);

  }

  public setCustomerCodeTo(eventType: string) {

    if(eventType!=EVENT_TYPE.BLUR){
      this.customerCodeToTrigger.closePanel();
    }
    
    if (!StringUtil.IsNullOrEmpty(this.customerCodeToCtrl.value)) {

      this.loadStart();
      this.customerService.GetItems(this.customerCodeToCtrl.value)
        .subscribe(response => {
          this.loadEnd();
          if (response != undefined && response.length > 0) {
            this.customerCodeToCtrl.setValue(response[0].code);
            this.customerNameToCtrl.setValue(response[0].name);
          }
          else {
            this.customerNameToCtrl.setValue("");
          }
        });
    }
    else {
      this.customerCodeToCtrl.setValue("");
      this.customerNameToCtrl.setValue("");
    }
    HtmlUtil.nextFocusByName(this.elementRef, 'customerNameCtrl', eventType);
  }

  public setCustomerName(eventType: string) {
    HtmlUtil.nextFocusByName(this.elementRef, 'customerKanaCtrl', eventType);
  }

  public setCustomerKana(eventType: string) {
    HtmlUtil.nextFocusByNames(this.elementRef, ['currencyCodeCtrl', 'amountRangeCtrl'], eventType);
  }

  public inputCustomerKana() {
    let value = this.customerKanaCtrl.value;
    value = EbDataHelper.convertToValidEbkana(value);
    value = EbDataHelper.removePersonalities(value, this.juridicalPersonalitysResult.juridicalPersonalities);
    this.customerKanaCtrl.setValue(value);
  }

  public setCbxCustomer(eventType: string) {
    let localstorageItem = new LocalStorageItem();
    localstorageItem.key = RangeSearchKey.PE0501_CUSTOMER;
    localstorageItem.value = this.cbxCustomerCtrl.value;
    this.localStorageManageService.set(localstorageItem);

    HtmlUtil.nextFocusByName(this.elementRef, "customerCodeFromCtrl", eventType);

  }

  ///////////////////////////////////////////////////////////  
  public setCbxUseReceiptSection(eventType: string) {
    let localstorageItem = new LocalStorageItem();
    localstorageItem.key = RangeSearchKey.PE0501_USE_RECEIPT_SECTION;
    localstorageItem.value = this.cbxUseReceiptSectionCtrl.value;
    this.localStorageManageService.set(localstorageItem);
  }    

  ///////////////////////////////////////////////////////////////////////
  public setCurrencyCode(eventType: string) {
    if (!StringUtil.IsNullOrEmpty(this.currencyCodeCtrl.value)) {

      this.loadStart();
      this.currencyService.GetItems(this.currencyCodeCtrl.value)
        .subscribe(response => {
          this.loadEnd();
          if (response != undefined && response.length > 0) {
            this.currencyCodeCtrl.setValue(response[0].code);
            HtmlUtil.nextFocusByName(this.elementRef, 'amountRangeCtrl', eventType);
          }
          else {
            this.currencyCodeCtrl.setValue("");
            HtmlUtil.nextFocusByName(this.elementRef, 'amountRangeCtrl', eventType);
          }
        });
    }
    else {
      this.currencyCodeCtrl.setValue("");
      HtmlUtil.nextFocusByName(this.elementRef, 'amountRangeCtrl', eventType);
    }
  }

  ///////////////////////////////////////////////////////////////////////
  public setAmountRange(eventType: string) {
    HtmlUtil.nextFocusByName(this.elementRef, 'amountFromCtrl', eventType);
  }

  public setAmountFrom(eventType: string) {
    HtmlUtil.nextFocusByName(this.elementRef, 'amountToCtrl', eventType);
  }


  public setCbxAmount(eventType: string) {

    let localstorageItem = new LocalStorageItem();
    localstorageItem.key = RangeSearchKey.PE0501_AMOUNT;
    localstorageItem.value = this.cbxAmountCtrl.value;
    this.localStorageManageService.set(localstorageItem);

    HtmlUtil.nextFocusByName(this.elementRef, 'amountFromCtrl', eventType);
  }

  public ignoreTransform(control: FormControl) {
    return StringUtil.IsNullOrEmpty(control.value) || control.value === '0';
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

  public setCurrencyForAmountFrom() {
    this.onLeaveCurrencyControl(this.amountFromCtrl);
    if (this.cbxAmountCtrl.value) {
      this.amountToCtrl.setValue(this.amountFromCtrl.value);
    }
  }

  public initCurrencyForAmountFrom() {
    this.onFocusCurrencyControl(this.amountFromCtrl);

  }

  public setAmountTo(eventType: string) {
    HtmlUtil.nextFocusByName(this.elementRef, 'note1Ctrl', eventType);
  }

  public setCurrencyForAmountTo() {
    this.onLeaveCurrencyControl(this.amountToCtrl);
  }

  public initCurrencyForAmountTo() {
    this.onFocusCurrencyControl(this.amountToCtrl);
  }

  ///////////////////////////////////////////////////////////////////////
  public setNote1(eventType: string) {
    HtmlUtil.nextFocusByName(this.elementRef, 'note2Ctrl', eventType);
  }

  public setNote2(eventType: string) {
    HtmlUtil.nextFocusByName(this.elementRef, 'note3Ctrl', eventType);
  }

  public setNote3(eventType: string) {
    HtmlUtil.nextFocusByName(this.elementRef, 'note4Ctrl', eventType);
  }

  public setNote4(eventType: string) {
    HtmlUtil.nextFocusByName(this.elementRef, 'note5Ctrl', eventType);
  }

  public setNote5(eventType: string) {
    HtmlUtil.nextFocusByName(this.elementRef, 'note6Ctrl', eventType);
  }

  public setNote6(eventType: string) {
    HtmlUtil.nextFocusByName(this.elementRef, 'note7Ctrl', eventType);
  }

  public setNote7(eventType: string) {
    HtmlUtil.nextFocusByName(this.elementRef, 'note8Ctrl', eventType);
  }

  public setNote8(eventType: string) {
    HtmlUtil.nextFocusByName(this.elementRef, 'billedAtFromCtrl', eventType);
  }

  public setCbxDeleteFlag(eventType: string) {

    if (this.cbxDeletedFlagCtrl.value) {
      this.deletedAtFromCtrl.setValue(null);
      this.deletedAtToCtrl.setValue(null);

      this.deletedAtFromCtrl.enable();
      this.deletedAtToCtrl.enable();
      
      this.deleteOrRecoveryButton = this.ItemNameConst.BTN_RESTORE;
      this.securityHideShow = this.userInfoService.isFunctionAvailable(FunctionType.RecoverBilling);
      
      HtmlUtil.nextFocusByName(this.elementRef, 'deletedAtFromCtrl', eventType);
    }
    else {
      this.deletedAtFromCtrl.setValue(null);
      this.deletedAtToCtrl.setValue(null);

      this.deletedAtFromCtrl.disable();
      this.deletedAtToCtrl.disable();

      this.deleteOrRecoveryButton = "削除";
      this.securityHideShow = this.userInfoService.isFunctionAvailable(FunctionType.ModifyBilling);
    }

  }

  public setCbxDetailDelFlag(eventType: string, index: number) {

    this.selectedDelete = false;
    this.cbxDetailDelFlagCtrls.forEach(element => {
      if (element.value) this.selectedDelete = true;
    });
  }

  public setSearchType(event:any){
    this.simplePageSearch=!this.simplePageSearch;

    if (this.billedAtFromCtrl.errors != null) {
      this.billedAtFromCtrl.reset();
    }
    
    if (this.billedAtToCtrl.errors != null) {
      this.billedAtToCtrl.reset();
    }

    if (this.salesAtFromCtrl.errors != null) {
      this.salesAtFromCtrl.reset();
    }
    
    if (this.salesAtToCtrl.errors != null) {
      this.salesAtToCtrl.reset();
    }    

    if (this.dueAtFromCtrl.errors != null) {
      this.dueAtFromCtrl.reset();
    }
    
    if (this.dueAtToCtrl.errors != null) {
      this.dueAtToCtrl.reset();
    }    

    if (this.deletedAtFromCtrl.errors != null) {
      this.deletedAtFromCtrl.reset();
    }
    
    if (this.deletedAtToCtrl.errors != null) {
      this.deletedAtToCtrl.reset();
    }

    if (this.updateAtFromCtrl.errors != null) {
      this.updateAtFromCtrl.reset();
    }
    
    if (this.updateAtToCtrl.errors != null) {
      this.updateAtToCtrl.reset();
    }    
  }
}
