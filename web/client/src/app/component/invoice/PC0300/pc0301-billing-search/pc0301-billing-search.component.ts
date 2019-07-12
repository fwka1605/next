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
import { BILL_INPUT_TYPE_DICTIONARY, MATCHING_ASSIGNMENT_FLAG_DICTIONARY, BANK_TRANSFER_RESULT_DICTIONARY, BILLING_AMOUNT_RANGE_DICTIONARY, CategoryType, GridId, CODE_TYPE, FunctionType } from 'src/app/common/const/kbn.const';
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
import { ComponentId, ComponentName } from 'src/app/common/const/component-name.const';
import { ModalMultiMasterComponent } from 'src/app/component/modal/modal-multi-master/modal-multi-master.component';
import { ModalFormSettingComponent } from 'src/app/component/modal/modal-form-setting/modal-form-setting.component';
import { EbDataHelper } from 'src/app/common/util/eb-data-helper';
import { PageUtil } from 'src/app/common/util/page-util';
import { ProcessResultService } from 'src/app/service/common/process-result.service';
import { MSG_WNG, MSG_ITEM_NUM, MSG_ERR, MSG_INF } from 'src/app/common/const/message.const';
import { LocalStorageManageService } from 'src/app/service/common/local-storage-manage.service';
import { LoginUser } from 'src/app/model/login-user.model'
import { LoginUserMasterService } from 'src/app/service/Master/login-user-master.service';
import { LocalStorageItem } from 'src/app/model/custom-model/local-storage-item';
import { GridSizeKey, RangeSearchKey } from 'src/app/common/const/local-storage-key-const';
import { MatAutocomplete, MatAutocompleteTrigger, MatExpansionPanel } from '@angular/material';
import { ModalMemoComponent } from 'src/app/component/modal/modal-memo/modal-memo.component';
import { BillingMemo } from 'src/app/model/billing-memo.model';


@Component({
  selector: 'app-pc0301-billing-search',
  templateUrl: './pc0301-billing-search.component.html',
  styleUrls: ['./pc0301-billing-search.component.css'],
  providers: [
    { provide: NgbDateParserFormatter, useClass: NgbDateCustomParserFormatter }
  ]

})
export class Pc0301BillingSearchComponent extends BaseComponent implements OnInit ,AfterViewInit{

  public panelOpenState: boolean;

  public readonly inputTypeDictionary = BILL_INPUT_TYPE_DICTIONARY;
  public readonly assignmentFlagDictionary = MATCHING_ASSIGNMENT_FLAG_DICTIONARY;
  public readonly transferResultDictionary = BANK_TRANSFER_RESULT_DICTIONARY;
  public readonly amountRangeDictionary = BILLING_AMOUNT_RANGE_DICTIONARY;

  public selectBilling: Billing;

  public generalSetting: GeneralSetting;
  public billingsResult: BillingsResult;
  public billingSearch: BillingSearch;
  public billingCategory: Category;
  public collectCategory: Category;
  public currency: Currency;
  public bsBillingCategoryId: number;
  public collectCategoryId: number;
  public currencyId: number;
  public previousBillingMonths: number;
  public updateById: number | null;

  // これをしないとEnum型がテンプレートで見えなかったため
  public TABLE_INDEX: typeof TABLE_INDEX = TABLE_INDEX;
  public Component_Id: typeof ComponentId = ComponentId;

  public billedAtFromCtrl: FormControl;  // 請求日
  public billedAtToCtrl: FormControl;
  public salesAtFromCtrl: FormControl;  // 売上日
  public salesAtToCtrl: FormControl;
  public dueAtFromCtrl: FormControl;  // 入金予定日
  public dueAtToCtrl: FormControl;
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
  public collectCategoryCodeCtrl: FormControl;  // 回収区分
  public collectCategoryNameCtrl: FormControl;
  public inputTypeCtrl: FormControl;  // 入力区分
  public departmentNameCtrl: FormControl;  //請求部門
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
  public customerKanaCtrl: FormControl;
  public cbxRequestDataCtrl: FormControl;  // 口座依頼作成済みデータを表示
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

  public undefineCtrl: FormControl; // 未定用

  public gridSettingsResult: GridSettingsResult;  // グリッド設定
  public juridicalPersonalitiesResult: JuridicalPersonalitysResult;  // 法人格除去用

  public isEnableBackBtn: boolean;
  public paramFrom: ComponentId;
  public departmentIdsInner: number[];
  public AllGridSettings: GridSetting[];
  public gridSettingHelper = new GridSettingHelper();

  public dispSumCount: number;
  public dispSumBillingAmount: number;
  public dispSumRemainAmount: number;

  public refProcess: string;

  public simplePageSearch:boolean=true; // 簡易・詳細検索
  public cbxColumnName: string; // チェックボックスの列名


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
    public localStorageManageService:LocalStorageManageService,
    public loginUserService: LoginUserMasterService,
    public billingHelper:      BillingHelper,
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

    if (this.userInfoService.isFunctionAvailable(FunctionType.ModifyBilling)) {
      this.cbxColumnName = '修正';
    } else {
      this.cbxColumnName = '参照';
    }
    let doSearch: boolean = false;
    this.activatedRoute.paramMap.subscribe((params: ParamMap) => {

      this.getPreviousBillingMonths();

      // ルートパラメータと同じ要領で Matrix URI パラメータが取得できる
      this.paramFrom = parseInt(params.get("from"));
      this.isEnableBackBtn = this.paramFrom == ComponentId.PE0102 || this.paramFrom == ComponentId.PD0301;
                    
      let param = params.get("process");
      if (!StringUtil.IsNullOrEmpty(param) && param == "back") {
        // 検索条件の設定
        this.MyFormGroup = this.billingService.BillingSearchFormGroup;
        this.readControlInit();

        // 検索の実行
        doSearch = true;

      }
      else {
        this.setControlInit();
        this.setValidator();
        this.setFormatter();
        this.clear();
        this.setAutoComplete();
      }

      if (param == "refCreate") {
        this.refProcess = param;
      }

      if (this.paramFrom == ComponentId.PE0102) {
        this.initializeDepartmentSelection();
      }

      if(this.paramFrom == ComponentId.PD0301){
        let customerCode = params.get("code");
        let customerName = params.get("name");
        this.customerCodeFromCtrl.setValue(customerCode);
        this.customerNameFromCtrl.setValue(customerName);
        this.customerCodeToCtrl.setValue(customerCode);
        this.customerNameToCtrl.setValue(customerName);
      }

    });

    // アプリケーションコントロール、メニュー権限、機能権限の取得、管理マスタ、通貨
    let gridSettingResponse =
      this.gridSettingService.GetItems(GridId.BillingSearch);

    let juridicalPersonalityResponse =
      this.juridicalPersonalityService.GetItems();

    // 処理の待機
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
          this.gridSettingsResult.gridSettings = (responseList[0] as GridSetting[]).filter(x => x.displayWidth != 0);
          // 検索条件用絞込無
          this.AllGridSettings = responseList[0];

          this.juridicalPersonalitiesResult = new JuridicalPersonalitysResult();
          this.juridicalPersonalitiesResult.juridicalPersonalities = responseList[1];
          
          if (doSearch) {
            this.search();
          }
        }
      },
        error => {
        }
      );
    
  }

  ngAfterViewInit(){
    HtmlUtil.nextFocusByName(this.elementRef, 'billedAtFromCtrl', EVENT_TYPE.NONE);
  }

  public readControlInit() {

    this.billedAtFromCtrl = <FormControl>this.MyFormGroup.controls['billedAtFromCtrl'];  // 請求日
    this.billedAtToCtrl = <FormControl>this.MyFormGroup.controls['billedAtToCtrl'];
    this.salesAtFromCtrl = <FormControl>this.MyFormGroup.controls['salesAtFromCtrl'];  // 売上日
    this.salesAtToCtrl = <FormControl>this.MyFormGroup.controls['salesAtToCtrl'];
    this.dueAtFromCtrl = <FormControl>this.MyFormGroup.controls['dueAtFromCtrl'];  // 入金予定日
    this.dueAtToCtrl = <FormControl>this.MyFormGroup.controls['dueAtToCtrl'];
    this.invoiceCodeFromCtrl = <FormControl>this.MyFormGroup.controls['invoiceCodeFromCtrl'];  // 請求書番号
    this.invoiceCodeToCtrl = <FormControl>this.MyFormGroup.controls['invoiceCodeToCtrl'];
    this.invoiceCodeCtrl = <FormControl>this.MyFormGroup.controls['invoiceCodeCtrl'];
    this.updateAtFromCtrl = <FormControl>this.MyFormGroup.controls['updateAtFromCtrl'];  // 最終更新日
    this.updateAtToCtrl = <FormControl>this.MyFormGroup.controls['updateAtToCtrl'];
    this.updateByCodeCtrl = <FormControl>this.MyFormGroup.controls['updateByCodeCtrl'];  // 最終更新者
    this.updateByNameCtrl = <FormControl>this.MyFormGroup.controls['updateByNameCtrl'];  // 最終更新者
    this.memoCtrl = <FormControl>this.MyFormGroup.controls['memoCtrl']; // メモ
    this.cbxMemoCtrl = <FormControl>this.MyFormGroup.controls['cbxMemoCtrl'];
    this.billingCategoryCodeCtrl = <FormControl>this.MyFormGroup.controls['billingCategoryCodeCtrl'];  // 請求区分
    this.billingCategoryNameCtrl = <FormControl>this.MyFormGroup.controls['billingCategoryNameCtrl'];
    this.collectCategoryCodeCtrl = <FormControl>this.MyFormGroup.controls['collectCategoryCodeCtrl'];  // 回収区分
    this.collectCategoryNameCtrl = <FormControl>this.MyFormGroup.controls['collectCategoryNameCtrl'];
    this.inputTypeCtrl = <FormControl>this.MyFormGroup.controls['inputTypeCtrl'];  // 入力区分
    this.departmentNameCtrl = <FormControl>this.MyFormGroup.controls['departmentNameCtrl'];  // 請求部門
    this.departmentCodeFromCtrl = <FormControl>this.MyFormGroup.controls['departmentCodeFromCtrl'];  // 請求部門コード
    this.departmentNameFromCtrl = <FormControl>this.MyFormGroup.controls['departmentNameFromCtrl'];
    this.departmentCodeToCtrl = <FormControl>this.MyFormGroup.controls['departmentCodeToCtrl'];
    this.departmentNameToCtrl = <FormControl>this.MyFormGroup.controls['departmentNameToCtrl'];
    this.cbxDepartmentCtrl = <FormControl>this.MyFormGroup.controls['cbxDepartmentCtrl'];
    this.staffCodeFromCtrl = <FormControl>this.MyFormGroup.controls['staffCodeFromCtrl'];  // 担当者コード
    this.staffNameFromCtrl = <FormControl>this.MyFormGroup.controls['staffNameFromCtrl'];
    this.staffCodeToCtrl = <FormControl>this.MyFormGroup.controls['staffCodeToCtrl'];
    this.staffNameToCtrl = <FormControl>this.MyFormGroup.controls['staffNameToCtrl'];
    this.cbxStaffCtrl = <FormControl>this.MyFormGroup.controls['cbxStaffCtrl'];
    this.customerCodeFromCtrl = <FormControl>this.MyFormGroup.controls['customerCodeFromCtrl'];  // 得意先コード
    this.customerNameFromCtrl = <FormControl>this.MyFormGroup.controls['customerNameFromCtrl'];
    this.customerCodeToCtrl = <FormControl>this.MyFormGroup.controls['customerCodeToCtrl'];
    this.customerNameToCtrl = <FormControl>this.MyFormGroup.controls['customerNameToCtrl'];
    this.cbxCustomerCtrl = <FormControl>this.MyFormGroup.controls['cbxCustomerCtrl'];
    this.customerNameCtrl = <FormControl>this.MyFormGroup.controls['customerNameCtrl'];
    this.cbxUseReceiptSectionCtrl = <FormControl>this.MyFormGroup.controls['cbxUseReceiptSectionCtrl']; // 入金部門対応マスタを使用
    this.customerKanaCtrl = <FormControl>this.MyFormGroup.controls['customerKanaCtrl'];
    this.cbxRequestDataCtrl = <FormControl>this.MyFormGroup.controls['cbxRequestDataCtrl'];  // 口座依頼作成済みデータを表示
    this.currencyCodeCtrl = <FormControl>this.MyFormGroup.controls['currencyCodeCtrl']; // 通貨コード
    this.cbxFullAssignmentCtrl = <FormControl>this.MyFormGroup.controls['cbxFullAssignmentCtrl'];  // 消込区分
    this.cbxPartAssignmentCtrl = <FormControl>this.MyFormGroup.controls['cbxPartAssignmentCtrl'];
    this.cbxNoAssignmentCtrl = <FormControl>this.MyFormGroup.controls['cbxNoAssignmentCtrl'];
    this.amountRangeCtrl = <FormControl>this.MyFormGroup.controls['amountRangeCtrl']; // 金額範囲
    this.amountFromCtrl = <FormControl>this.MyFormGroup.controls['amountFromCtrl'];
    this.amountToCtrl = <FormControl>this.MyFormGroup.controls['amountToCtrl'];
    this.cbxAmountCtrl = <FormControl>this.MyFormGroup.controls['cbxAmountCtrl'];
    this.note1Ctrl = <FormControl>this.MyFormGroup.controls['note1Ctrl'];  // 備考
    this.note2Ctrl = <FormControl>this.MyFormGroup.controls['note2Ctrl'];
    this.note3Ctrl = <FormControl>this.MyFormGroup.controls['note3Ctrl'];
    this.note4Ctrl = <FormControl>this.MyFormGroup.controls['note4Ctrl'];
    this.note5Ctrl = <FormControl>this.MyFormGroup.controls['note5Ctrl'];
    this.note6Ctrl = <FormControl>this.MyFormGroup.controls['note6Ctrl'];
    this.note7Ctrl = <FormControl>this.MyFormGroup.controls['note7Ctrl'];
    this.note8Ctrl = <FormControl>this.MyFormGroup.controls['note8Ctrl'];

    this.undefineCtrl = <FormControl>this.MyFormGroup.controls['undefineCtrl']; // 未定用;
  }

  public setControlInit() {

    this.billedAtFromCtrl = new FormControl("", [Validators.maxLength(10)]);  // 請求日
    this.billedAtToCtrl = new FormControl("", [Validators.maxLength(10)]);
    this.salesAtFromCtrl = new FormControl("", [Validators.maxLength(10)]);  // 売上日
    this.salesAtToCtrl = new FormControl("", [Validators.maxLength(10)]);
    this.dueAtFromCtrl = new FormControl("", [Validators.maxLength(10)]);  // 入金予定日
    this.dueAtToCtrl = new FormControl("", [Validators.maxLength(10)]);
    this.invoiceCodeFromCtrl = new FormControl("", [Validators.maxLength(20),]);  // 請求書番号
    this.invoiceCodeToCtrl = new FormControl("", [Validators.maxLength(20),]);
    this.invoiceCodeCtrl = new FormControl("", [Validators.maxLength(20),]);
    this.updateAtFromCtrl = new FormControl("", [Validators.maxLength(10)]);  // 最終更新日
    this.updateAtToCtrl = new FormControl("", [Validators.maxLength(10)]);
    this.updateByCodeCtrl = new FormControl("", [Validators.maxLength(this.userInfoService.ApplicationControl.loginUserCodeLength)]);  // 最終更新者
    this.updateByNameCtrl = new FormControl("");  // 最終更新者
    this.memoCtrl = new FormControl(""); // メモ
    this.cbxMemoCtrl = new FormControl("");
    this.billingCategoryCodeCtrl = new FormControl("", [Validators.maxLength(2),]);  // 請求区分
    this.billingCategoryNameCtrl = new FormControl("");
    this.collectCategoryCodeCtrl = new FormControl("", [Validators.maxLength(2),]);  // 回収区分
    this.collectCategoryNameCtrl = new FormControl("");
    this.inputTypeCtrl = new FormControl("");  // 入力区分
    this.departmentNameCtrl = new FormControl("");  // 請求部門
    this.departmentCodeFromCtrl = new FormControl("", [Validators.maxLength(this.userInfoService.ApplicationControl.departmentCodeLength),]);  // 請求部門コード
    this.departmentNameFromCtrl = new FormControl("");
    this.departmentCodeToCtrl = new FormControl("", [Validators.maxLength(this.userInfoService.ApplicationControl.departmentCodeLength),]);
    this.departmentNameToCtrl = new FormControl("");
    this.cbxDepartmentCtrl = new FormControl("");
    this.staffCodeFromCtrl = new FormControl("", [Validators.maxLength(this.userInfoService.ApplicationControl.staffCodeLength),]);  // 担当者コード
    this.staffNameFromCtrl = new FormControl("");
    this.staffCodeToCtrl = new FormControl("", [Validators.maxLength(this.userInfoService.ApplicationControl.staffCodeLength),]);
    this.staffNameToCtrl = new FormControl("");
    this.cbxStaffCtrl = new FormControl("");
    this.customerCodeFromCtrl = new FormControl("", [Validators.maxLength(this.userInfoService.ApplicationControl.customerCodeLength),]);  // 得意先コード
    this.customerNameFromCtrl = new FormControl("");
    this.customerCodeToCtrl = new FormControl("", [Validators.maxLength(this.userInfoService.ApplicationControl.customerCodeLength),]);
    this.customerNameToCtrl = new FormControl("");
    this.cbxCustomerCtrl = new FormControl("");
    this.customerNameCtrl = new FormControl("");
    this.cbxUseReceiptSectionCtrl = new FormControl(""); // 入金部門対応マスタを使用
    this.customerKanaCtrl = new FormControl("");
    this.cbxRequestDataCtrl = new FormControl("");  // 口座依頼作成済みデータを表示
    this.currencyCodeCtrl = new FormControl("", [Validators.maxLength(3),]); // 通貨コード
    this.cbxFullAssignmentCtrl = new FormControl(false);  // 消込区分
    this.cbxPartAssignmentCtrl = new FormControl(false);
    this.cbxNoAssignmentCtrl = new FormControl(false);
    this.amountRangeCtrl = new FormControl(""); // 金額範囲
    this.amountFromCtrl = new FormControl("", [Validators.maxLength(16),]);
    this.amountToCtrl = new FormControl("", [Validators.maxLength(16),]);
    this.cbxAmountCtrl = new FormControl("");
    this.note1Ctrl = new FormControl("", [Validators.maxLength(100),]);  // 備考
    this.note2Ctrl = new FormControl("", [Validators.maxLength(100),]);
    this.note3Ctrl = new FormControl("", [Validators.maxLength(100),]);
    this.note4Ctrl = new FormControl("", [Validators.maxLength(100),]);
    this.note5Ctrl = new FormControl("", [Validators.maxLength(100),]);
    this.note6Ctrl = new FormControl("", [Validators.maxLength(100),]);
    this.note7Ctrl = new FormControl("", [Validators.maxLength(100),]);
    this.note8Ctrl = new FormControl("", [Validators.maxLength(100),]);

    this.undefineCtrl = new FormControl(""); // 未定用;


  }

  public setValidator() {
    this.MyFormGroup = new FormGroup({

      billedAtFromCtrl: this.billedAtFromCtrl,  // 請求日
      billedAtToCtrl: this.billedAtToCtrl,
      salesAtFromCtrl: this.salesAtFromCtrl,  // 売上日
      salesAtToCtrl: this.salesAtToCtrl,
      dueAtFromCtrl: this.dueAtFromCtrl,  // 入金予定日
      dueAtToCtrl: this.dueAtToCtrl,
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
      departmentNameCtrl: this.departmentNameCtrl,  // 請求部門
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
      cbxRequestDataCtrl: this.cbxRequestDataCtrl,  // 口座依頼作成済みデータを表示
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

    FormatterUtil.setNumberFormatter(this.billingCategoryCodeCtrl);// 請求区分
    FormatterUtil.setNumberFormatter(this.collectCategoryCodeCtrl);// 回収区分

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

    FormatterUtil.setCurrencyFormatter(this.amountFromCtrl);
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

  public openMultiMasterModal(table: TABLE_INDEX) {

    let componentFactory = this.componentFactoryResolver.resolveComponentFactory(ModalMultiMasterComponent);
    let componentRef = this.viewContainerRef.createComponent(componentFactory);

    componentRef.instance.TableIndex = table;
    componentRef.instance.InitialIds = this.departmentIdsInner;

    componentRef.instance.Closing.subscribe(() => {

      if (componentRef.instance.ModalStatus == MODAL_STATUS_TYPE.SELECT) {
        switch (table) {
          case TABLE_INDEX.MASTER_DEPARTMENT:
            {
              this.departmentNameCtrl.setValue(componentRef.instance.isSelectedAll ? "すべて" : "請求部門絞込有");
              this.departmentIdsInner = componentRef.instance.SelectedIds;
              break;
            }
        }
      }
      componentRef.destroy();
    });
  }

  /*
    table:Master名称
    keyCode:キーコードがある場合はF9のみモーダルを開く
    index:明細行の行No
  */
  public openMasterModal(table: TABLE_INDEX, type: string = null,  index: number = -1) {


    this.updateByCodeTrigger.closePanel();
    this.billingCategoryCodeTrigger.closePanel();
    this.collectCategoryCodeTrigger.closePanel();
    this.departmentCodeFromigger.closePanel();
    this.departmentCodeToigger.closePanel();
    this.staffCodeFromTrigger.closePanel();
    this.staffCodeToTrigger.closePanel();
    this.customerCodeFromTrigger.closePanel();
    this.customerCodeToTrigger.closePanel();


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
              this.billingCategory = componentRef.instance.SelectedObject;
              this.bsBillingCategoryId = this.billingCategory.id;
              break;
            }
          case TABLE_INDEX.MASTER_COLLECT_CATEGORY:
            {
              this.collectCategoryCodeCtrl.setValue(componentRef.instance.SelectedCode);
              this.collectCategoryNameCtrl.setValue(componentRef.instance.SelectedName);
              this.collectCategory = componentRef.instance.SelectedObject;
              this.collectCategoryId = this.collectCategory.id;
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
              let loginUser = componentRef.instance.SelectedObject as LoginUser;
              this.updateById = loginUser.id;
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
              this.currency = componentRef.instance.SelectedObject;
              this.currencyId = this.currency.id;
              break;
            }
        }
      }

      componentRef.destroy();
    });
  }


  public openBillingMemoModal(index: number) {

    // とりあえず制限は入れない。
    let componentFactory = this.componentFactoryResolver.resolveComponentFactory(ModalMemoComponent);
    let componentRef = this.viewContainerRef.createComponent(componentFactory);

    let tmpBillingMemo = new BillingMemo();
    tmpBillingMemo.billingId = this.billingsResult.billings[index].id;
    tmpBillingMemo.memo = this.billingsResult.billings[index].memo;

    componentRef.instance.billingMemo = tmpBillingMemo;
    componentRef.instance.categoryType = CategoryType.Billing;

    componentRef.instance.Closing.subscribe(() => {
      if (componentRef.instance.ModalStatus == MODAL_STATUS_TYPE.OK) {
        tmpBillingMemo = componentRef.instance.billingMemo;

        this.billingService.SaveMemo(tmpBillingMemo)
          .subscribe(response => {
            if (response == PROCESS_RESULT_RESULT_TYPE.FAILURE) {
              this.processCustomResult = this.processResultService.processAtFailure(
                this.processCustomResult, MSG_ERR.SOMETHING_ERROR.replace(MSG_ITEM_NUM.FIRST, 'メモの登録'),
                this.partsResultMessageComponent);
            }
            else {
              let index = this.billingsResult.billings.findIndex(billing => {
                return billing.id == tmpBillingMemo.billingId
              });
              if (index != -1) {
                this.billingsResult.billings[index].memo = tmpBillingMemo.memo;
              }
            }
          });
      }
      componentRef.destroy();
    });
  }  

  public getGridSetting(columnName: string): string {

    if (this.AllGridSettings == undefined) return "";

    let columnSetting = this.AllGridSettings.find(x => x.columnName == columnName);
    return columnSetting == undefined ? "" : columnSetting.columnNameJp;
  }

  public getAssignmentFlag(id: number): string {
    return this.assignmentFlagDictionary[id].val;
  }

  public getBillingInputType(id: number): string {
    return this.inputTypeDictionary[id].val;
  }

  public getBankTransferResult(id: any): string {

    if (id == undefined) return "";
    return this.transferResultDictionary[id].val;
  }

  public getPreviousBillingMonths() {
    this.generalSetting = this.userInfoService.GeneralSettings.find(x => x.code == '請求データ検索開始月範囲');
    this.previousBillingMonths = NumberUtil.ParseInt(this.generalSetting.value);
  }

  public clear() {
    this.panelOpenState = true;
    this.panel.open();
    
    this.MyFormGroup.reset();
    this.ComponentStatus = null;
    this.bsBillingCategoryId = 0;
    this.collectCategoryId = 0;
    this.currencyId = 0;
    this.inputTypeCtrl.setValue(this.inputTypeDictionary[0]);
    this.amountRangeCtrl.setValue(this.amountRangeDictionary[0]);
    this.cbxPartAssignmentCtrl.setValue(true);
    this.cbxNoAssignmentCtrl.setValue(true);
    this.setDefaultDateBilledAtFrom();

    this.selectBilling = null;
    this.updateById = null;

    /*
    this.billingsResult = null;
    this.dispSumCount = 0;
    this.dispSumBillingAmount = 0;
    this.dispSumRemainAmount = 0;
    */
   
    this.setRangeCheckbox();

    //this.simplePageSearch=true;

    HtmlUtil.nextFocusByName(this.elementRef, 'billedAtFromCtrl', EVENT_TYPE.NONE);

  }

  public setRangeCheckbox() {
    let cbxDepartment = this.localStorageManageService.get(RangeSearchKey.PC0301_DEPARTMENT);
    let cbxStaff = this.localStorageManageService.get(RangeSearchKey.PC0301_STAFF);
    let cbxCustomer = this.localStorageManageService.get(RangeSearchKey.PC0301_CUSTOMER);
    let cbxAmount = this.localStorageManageService.get(RangeSearchKey.PC0301_AMOUNT);
    let cbxUseReceiptSection = this.localStorageManageService.get(RangeSearchKey.PC0301_USE_RECEIPT_SECTION);

    if (cbxDepartment != null) {
      this.cbxDepartmentCtrl.setValue(cbxDepartment.value);
    }

    if (cbxStaff != null) {
      this.cbxStaffCtrl.setValue(cbxStaff.value);
    }

    if (cbxCustomer != null) {
      this.cbxCustomerCtrl.setValue(cbxCustomer.value);
    }

    if (cbxAmount != null) {
      this.cbxAmountCtrl.setValue(cbxAmount.value);
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

      case BUTTON_ACTION.EXPORT:
        this.export();
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

    if (this.paramFrom == ComponentId.PE0102 && this.billingSearch.useDepartmentWork) {
      this.matchingService.SaveWorkDepartmentTarget(this.matchingService.collationInfo.clientKey, this.departmentIdsInner)
        .subscribe(response => {
          if (response == PROCESS_RESULT_RESULT_TYPE.FAILURE) {
            this.processCustomResult = this.processResultService.processAtFailure(
              this.processCustomResult, MSG_ERR.SOMETHING_ERROR.replace(MSG_ITEM_NUM.FIRST, "検索"),
              this.partsResultMessageComponent);
            return;
          }
        });
    }

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

          // 戻る対応のための検索結果を退避
          this.billingService.BillingSearchFormGroup = this.MyFormGroup;

          // 選択行の設定
          this.selectBilling = this.billingsResult.billings[0];

          this.panelOpenState = false;

          if (this.paramFrom == ComponentId.PE0102) {
            this.matchingService.collationInfo.searchBillings = this.billingsResult.billings;
            this.router.navigate(['main/PE0102', { process: "search", from: ComponentId.PC0301 }]);
          }

          this.setDispFooterTotalSum();

        }
        else {
          this.billingsResult = new BillingsResult();
          this.billingsResult.billings = new Array<Billing>();

          this.panelOpenState = true;

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
    
    if (!DateUtil.isValidRange(this.salesAtFromCtrl, this.salesAtToCtrl)) {
      msg = MSG_WNG.INPUT_VALID_DATE_FOR_PARAMETERS.replace(MSG_ITEM_NUM.FIRST, '売上日終了')
        .replace(MSG_ITEM_NUM.SECOND, '売上日開始');
    }

    if (!DateUtil.isValidRange(this.dueAtFromCtrl, this.dueAtToCtrl)) {
      msg = MSG_WNG.INPUT_VALID_DATE_FOR_PARAMETERS.replace(MSG_ITEM_NUM.FIRST, '入金予定日終了')
        .replace(MSG_ITEM_NUM.SECOND, '入金予定日開始');
    }

    if (!DateUtil.isValidRange(this.updateAtFromCtrl, this.updateAtToCtrl)) {
      msg = MSG_WNG.INPUT_VALID_DATE_FOR_PARAMETERS.replace(MSG_ITEM_NUM.FIRST, '最終更新日終了')
        .replace(MSG_ITEM_NUM.SECOND, '最終更新日開始');
    }

    //下記は後で実装
    //this.invoiceCodeFromCtrl
    //this.departmentCodeFromCtrl
    //this.staffCodeFromCtrl
    //this.customerCodeFromCtrl

    //this.amountFromCtrl

    if (!this.cbxFullAssignmentCtrl.value && !this.cbxPartAssignmentCtrl.value && !this.cbxNoAssignmentCtrl.value) {
      msg = MSG_WNG.INPUT_REQUIRED.replace(MSG_ITEM_NUM.FIRST, '消込区分');
    }

    if (msg.length != 0) {
      this.processCustomResult = this.processResultService.processAtWarning(
        this.processCustomResult, msg, this.partsResultMessageComponent);
        return false;
    }

    return true;
  }

  public searchCondition(): BillingSearch {
    let billingItem = new BillingSearch();

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

      billingItem.bsBilledAtFrom = DateUtil.ConvertFromDatepicker(this.billedAtFromCtrl);  //  請求日
      billingItem.bsBilledAtTo = DateUtil.ConvertFromDatepicker(this.billedAtToCtrl);

      billingItem.bsSalesAtFrom = DateUtil.ConvertFromDatepicker(this.salesAtFromCtrl);  //  売上日
      billingItem.bsSalesAtTo = DateUtil.ConvertFromDatepicker(this.salesAtToCtrl);

      billingItem.bsDueAtFrom = DateUtil.ConvertFromDatepicker(this.dueAtFromCtrl);  //  入金予定日
      billingItem.bsDueAtTo = DateUtil.ConvertFromDatepicker(this.dueAtToCtrl);

      billingItem.bsInvoiceCodeFrom = this.invoiceCodeFromCtrl.value;  //  請求書番号
      billingItem.bsInvoiceCodeTo = this.invoiceCodeToCtrl.value;
      billingItem.bsInvoiceCode = this.invoiceCodeCtrl.value;

      billingItem.bsUpdateAtFrom = DateUtil.ConvertFromDatepicker(this.updateAtFromCtrl);  //最終更新日
      billingItem.bsUpdateAtTo = DateUtil.ConvertFromDatepicker(this.updateAtToCtrl);

      billingItem.loginUserCode = this.updateByCodeCtrl.value; //  最終更新者
      billingItem.userId = this.updateById;  //  最終更新者

      billingItem.existsMemo = this.cbxMemoCtrl.value; //  メモ有り
      billingItem.bsMemo = this.memoCtrl.value;

      billingItem.bsBillingCategoryId = this.bsBillingCategoryId;  //  請求区分
      billingItem.collectCategoryId = this.collectCategoryId;  //  回収区分
      billingItem.bsInputType = this.inputTypeCtrl.value.id; //  入力区分

      billingItem.bsDepartmentCodeFrom = this.departmentCodeFromCtrl.value;  //  請求部門コード
      billingItem.bsDepartmentCodeTo = this.departmentCodeToCtrl.value;

      billingItem.bsStaffCodeFrom = this.staffCodeFromCtrl.value;  //  担当者コード
      billingItem.bsStaffCodeTo = this.staffCodeToCtrl.value;

      billingItem.bsCustomerCodeFrom = this.customerCodeFromCtrl.value;  //  得意先コード、名称、カナ
      billingItem.bsCustomerCodeTo = this.customerCodeToCtrl.value;
      billingItem.bsCustomerName = this.customerNameCtrl.value;
      billingItem.bsCustomerNameKana = this.customerKanaCtrl.value;

      billingItem.useSectionMaster = this.userInfoService.ApplicationControl.useReceiptSection == 1 && this.cbxUseReceiptSectionCtrl.value;  //  入金部門対応マスターを使用
      billingItem.requestDate = this.cbxRequestDataCtrl.value ? 1 : 0; //  口振依頼作成済データを表示
      //billingItem.currencyId = this.currencyCodeCtrl.value;  //  通貨コード
      billingItem.currencyId = this.currencyId;  //  通貨コード

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

      billingItem.bsNote1 = this.note1Ctrl.value;  //  備考
      billingItem.bsNote2 = this.note2Ctrl.value;
      billingItem.bsNote3 = this.note3Ctrl.value;
      billingItem.bsNote4 = this.note4Ctrl.value;
      billingItem.bsNote5 = this.note5Ctrl.value;
      billingItem.bsNote6 = this.note6Ctrl.value;
      billingItem.bsNote7 = this.note7Ctrl.value;
      billingItem.bsNote8 = this.note8Ctrl.value;

      let gSetting = this.AllGridSettings.filter((gridSetting: {columnName: string, displayWidth: number}) => {
        return ((gridSetting.columnName == "FirstRecordedAt" || gridSetting.columnName == "LastRecordedAt") && gridSetting.displayWidth > 0) 
      });
      billingItem.requireRecordedAt = gSetting.length > 0;

      if (this.paramFrom == ComponentId.PE0102) {

        billingItem.useDepartmentWork = this.matchingService.collationInfo.departments.length != this.departmentIdsInner.length;

        let collationData = this.matchingService.collationInfo.collations[this.matchingService.collationInfo.individualIndexNo];
        if (collationData.parentCustomerId != undefined && collationData.parentCustomerId != 0) {
          billingItem.parentCustomerId = collationData.parentCustomerId;
        }

      }
    }

    return billingItem;
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

    let result: boolean = false;
    let data: string = "";
    let dataList = this.billingsResult.billings;

    let headerData = FileUtil.encloseItemBySymbol(this.gridSettingsResult.gridSettings.map(gridSetting => { return gridSetting.columnNameJp }));
    data = data + headerData.join(",") + LINE_FEED_CODE;

    for (let index = 0; index < dataList.length; index++) {
      let dataItem: Array<any> = [];

      this.gridSettingsResult.gridSettings.forEach(gridSetting => {
        switch (gridSetting.columnName) {
          case 'Id':
            dataItem.push(dataList[index].id);
            break;
          case 'AssignmentState':
            dataItem.push(this.getAssignmentFlag(dataList[index].assignmentFlag));
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
            dataItem.push(this.getBillingInputType(dataList[index].inputType));
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
    let resultDatas: Array<any> = [];
    resultDatas.push(data);

    FileUtil.download(resultDatas, "請求データ" + DateUtil.getYYYYMMDD(0), FILE_EXTENSION.CSV);
    result = true;
    this.processResultService.processAtOutput(
      this.processCustomResult, result, 0, this.partsResultMessageComponent);    

    modalRouterProgressComponentRef.destroy();

  }

  public print() {

    this.processCustomResult = this.processResultService.clearProcessCustomMsg(this.processCustomResult);

    if (!this.isValidSearchOption()) return;

    let componentFactory = this.componentFactoryResolver.resolveComponentFactory(ModalRouterProgressComponent);
    let componentRef = this.viewContainerRef.createComponent(componentFactory);
    componentRef.instance.Closing.subscribe(() => {
      componentRef.destroy();
    });

    this.billingSearch = this.searchCondition();

    this.billingService.GetReport(this.billingSearch)
      .subscribe(response => {
        FileUtil.download([response.body], this.Title + DateUtil.getYYYYMMDD(0), FILE_EXTENSION.PDF);
        componentRef.destroy();
      });
  }

  public update() {

    this.router.navigate(['main/PC0201', { id: this.selectBilling.id }]);
  }

  public selectUpdateLine(lineNo: number) {

    if (this.paramFrom == ComponentId.PE0102 || this.paramFrom == ComponentId.PD0301) return;

    this.selectBilling = this.billingsResult.billings[lineNo];
    let billingInputId = this.selectBilling.billingInputId == undefined ? "" : this.selectBilling.billingInputId.toString();
    let billingCount: number = 1;
    if (!StringUtil.IsNullOrEmpty(billingInputId)){
      billingCount = this.billingsResult.billings.filter(x => x.billingInputId === this.selectBilling.billingInputId).length;
    }

    this.router.navigate(['main/PC0201', { id: this.selectBilling.id, inputId: billingInputId, count: billingCount, form : ComponentId.PC0301, process : this.refProcess==null?"":this.refProcess }])
  }

  public get ReportSettingButtonDisableFlag(): boolean {
    let bRtn = false;

    if (this.billingsResult != undefined) {
      bRtn = true;
    }
    return bRtn;
  }

  /**
   * 帳票設定
   */
  public setReportSetting() {

    let componentFactory = this.componentFactoryResolver.resolveComponentFactory(ModalFormSettingComponent);
    let componentRef = this.viewContainerRef.createComponent(componentFactory);

    componentRef.instance.ReportId = ComponentName[this.ComponentId];
    componentRef.instance.processModalCustomResult = this.processModalCustomResult;

    componentRef.instance.Closing.subscribe(() => {
      componentRef.destroy();
    });

  }

  /**
   * 戻る
   */
  public back() {
    if(this.paramFrom == ComponentId.PD0301){
      this.router.navigate(['main/PD0301', { process: "back" }]);
    }
    else if(this.paramFrom == ComponentId.PE0102){
      this.matchingService.collationInfo.searchBillings = null;
      this.router.navigate(['main/PE0102', { process: "back" }]);
    }
  }

  public setDefaultDateBilledAtFrom() {
    this.getPreviousBillingMonths();
    let date = new Date();
    date.setMonth(date.getMonth() - this.previousBillingMonths);
    this.billedAtFromCtrl.setValue(new NgbDate(date.getFullYear(), date.getMonth() + 1, date.getDate()));
  }

  public selectLine(lineNo: number) {
    this.selectBilling = this.billingsResult.billings[lineNo];
  }

  /////////////////////////////////////////////////////////////////////////////////
  public setBilledAtFrom(eventType: string) {
    HtmlUtil.nextFocusByName(this.elementRef, 'billedAtToCtrl', eventType);
  }

  public setBilledAtTo(eventType: string) {
    HtmlUtil.nextFocusByName(this.elementRef, 'salesAtFromCtrl', eventType);
  }

  /////////////////////////////////////////////////////////////////////////////////
  public setSalesAtFrom(eventType: string) {
    // if(this.salesAtFromCtrl.value.length===8) {
    //   let date = 
    //         new NgbDate(
    //               NumberUtil.ParseInt(this.salesAtFromCtrl.value.substr(0,4)),
    //               NumberUtil.ParseInt(this.salesAtFromCtrl.value.substr(4,2)),
    //               NumberUtil.ParseInt(this.salesAtFromCtrl.value.substr(6,2)));
    //   this.salesAtFromCtrl.setValue(date);
    // }
    
    HtmlUtil.nextFocusByName(this.elementRef, 'salesAtToCtrl', eventType);
  }

  public setSalesAtTo(eventType: string) {
    HtmlUtil.nextFocusByName(this.elementRef, 'dueAtFromCtrl', eventType);
  }

  /////////////////////////////////////////////////////////////////////////////////
  public setDueAtFrom(eventType: string) {
    HtmlUtil.nextFocusByName(this.elementRef, 'dueAtToCtrl', eventType);
  }

  public setDueAtTo(eventType: string) {
    HtmlUtil.nextFocusByNames(this.elementRef, ['invoiceCodeFromCtrl', 'invoiceCodeCtrl'], eventType);
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
      if (this.userInfoService.ApplicationControl.loginUserCodeType == CODE_TYPE.NUMBER) {
        this.updateByCodeCtrl.setValue(StringUtil.setPaddingFrontZero(this.updateByCodeCtrl.value, this.userInfoService.ApplicationControl.customerCodeLength));
      }

      this.loadStart();
      this.loginUserService.GetItems(this.updateByCodeCtrl.value)
        .subscribe(response => {
          this.loadEnd();
          if (response != PROCESS_RESULT_RESULT_TYPE.FAILURE && response.length > 0) {
            this.updateByCodeCtrl.setValue(response[0].code);
            this.updateByNameCtrl.setValue(response[0].name);
            this.updateById = response[0].id;
            HtmlUtil.nextFocusByName(this.elementRef, 'memoCtrl', eventType);
          }
          else {
            //this.updateByCodeCtrl.setValue("");
            this.updateByNameCtrl.setValue("");
            this.updateById = null;
          }
        });
    }
    else {
      this.updateByCodeCtrl.setValue("");
      this.updateByNameCtrl.setValue("");
      this.updateById = null;
      HtmlUtil.nextFocusByName(this.elementRef, 'memoCtrl', eventType);
    }

  }

  /////////////////////////////////////////////////////////////////////////////////
  public setCbxMemo(eventType: string) {
    if (this.cbxMemoCtrl.value == true) {
      HtmlUtil.nextFocusByName(this.elementRef, 'memoCtrl', eventType);
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
            HtmlUtil.nextFocusByName(this.elementRef, 'collectCategoryCodeCtrl', eventType);

          }
          else {
            //this.billingCategoryCodeCtrl.setValue("");
            this.billingCategoryNameCtrl.setValue("");
            this.bsBillingCategoryId = 0;
          }
        });

    }
    else {
      this.billingCategoryCodeCtrl.setValue("");
      this.billingCategoryNameCtrl.setValue("");
      this.bsBillingCategoryId = 0;
      HtmlUtil.nextFocusByName(this.elementRef, 'collectCategoryCodeCtrl', eventType);
    }

  }

  /////////////////////////////////////////////////////////////////////////////////

  public setCollectCategoryCode(eventType: string) {

    if(eventType!=EVENT_TYPE.BLUR){
      this.collectCategoryCodeTrigger.closePanel();
    }

    if (!StringUtil.IsNullOrEmpty(this.collectCategoryCodeCtrl.value)) {

      this.collectCategoryCodeCtrl.setValue(StringUtil.setPaddingFrontZero(this.collectCategoryCodeCtrl.value, 2));

      this.loadStart();
      this.categoryService.GetItems(CategoryType.Collection, this.collectCategoryCodeCtrl.value)
        .subscribe(response => {
          this.loadEnd();
          if (response != PROCESS_RESULT_RESULT_TYPE.FAILURE && response.length > 0) {

            this.collectCategoryCodeCtrl.setValue(response[0].code);
            this.collectCategoryNameCtrl.setValue(response[0].name);
            this.collectCategoryId = response[0].id;
            HtmlUtil.nextFocusByName(this.elementRef, 'inputTypeCtrl', eventType);
          }
          else {
            // this.collectCategoryCodeCtrl.setValue("");
            this.collectCategoryNameCtrl.setValue("");
            this.collectCategoryId = 0;
          }
        });

    }
    else {
      this.collectCategoryCodeCtrl.setValue("");
      this.collectCategoryNameCtrl.setValue("");
      this.collectCategoryId = 0;
      HtmlUtil.nextFocusByName(this.elementRef, 'inputTypeCtrl', eventType);
    }

  }

  /////////////////////////////////////////////////////////////////////////////////
  public setInputType(eventType: string) {
    HtmlUtil.nextFocusByName(this.elementRef, 'departmentCodeFromCtrl', eventType);
  }

  /////////////////////////////////////////////////////////////////////////////////

  public setDepartmentCodeFrom(eventType: string) {

    if(eventType!=EVENT_TYPE.BLUR){
      this.departmentCodeFromigger.closePanel();
    }

    if (!StringUtil.IsNullOrEmpty(this.departmentCodeFromCtrl.value)) {

      if (this.userInfoService.ApplicationControl.departmentCodeType == CODE_TYPE.NUMBER) {
        this.departmentCodeFromCtrl.setValue(StringUtil.setPaddingFrontZero(this.departmentCodeFromCtrl.value, this.userInfoService.ApplicationControl.departmentCodeLength));
      }

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
      this.departmentCodeToigger.closePanel();
    }

    if (!StringUtil.IsNullOrEmpty(this.departmentCodeToCtrl.value)) {

      if (this.userInfoService.ApplicationControl.departmentCodeType == CODE_TYPE.NUMBER) {
        this.departmentCodeToCtrl.setValue(StringUtil.setPaddingFrontZero(this.departmentCodeToCtrl.value, this.userInfoService.ApplicationControl.departmentCodeLength));
      }      

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
    localstorageItem.key = RangeSearchKey.PC0301_DEPARTMENT;
    localstorageItem.value = this.cbxDepartmentCtrl.value;
    this.localStorageManageService.set(localstorageItem);


    HtmlUtil.nextFocusByName(this.elementRef, 'departmentCodeFromCtrl', eventType);

  }

  /////////////////////////////////////////////////////////////////////////////////

  public setStaffCodeFrom(eventType: string) {

    if(eventType!=EVENT_TYPE.BLUR){
      this.staffCodeFromTrigger.closePanel();
    }

    if (!StringUtil.IsNullOrEmpty(this.staffCodeFromCtrl.value)) {

      if (this.userInfoService.ApplicationControl.staffCodeType == CODE_TYPE.NUMBER) {
        this.staffCodeFromCtrl.setValue(StringUtil.setPaddingFrontZero(this.staffCodeFromCtrl.value, this.userInfoService.ApplicationControl.staffCodeLength));
      }

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
          }
          else {
            //this.staffCodeFromCtrl.setValue("");
            this.staffNameFromCtrl.setValue("");
            if (this.cbxStaffCtrl.value == true) {
              this.staffCodeToCtrl.setValue(this.staffCodeFromCtrl.value);
              this.staffNameToCtrl.setValue("");
            }
          }
          HtmlUtil.nextFocusByName(this.elementRef, 'staffCodeToCtrl', eventType);
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
      this.staffCodeToTrigger.closePanel();
    }

    if (!StringUtil.IsNullOrEmpty(this.staffCodeToCtrl.value)) {

      if (this.userInfoService.ApplicationControl.staffCodeType == CODE_TYPE.NUMBER) {
        this.staffCodeToCtrl.setValue(StringUtil.setPaddingFrontZero(this.staffCodeToCtrl.value, this.userInfoService.ApplicationControl.staffCodeLength));
      }

      this.loadStart();
      this.staffService.GetItems(this.staffCodeToCtrl.value)
        .subscribe(response => {
          this.loadEnd();
          if (response != PROCESS_RESULT_RESULT_TYPE.FAILURE && response.length > 0) {
            this.staffCodeToCtrl.setValue(response[0].code);
            this.staffNameToCtrl.setValue(response[0].name);
          }
          else {
            //this.staffCodeToCtrl.setValue("");
            this.staffNameToCtrl.setValue("");
          }
          HtmlUtil.nextFocusByName(this.elementRef, 'customerCodeFromCtrl', eventType);
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
    localstorageItem.key = RangeSearchKey.PC0301_STAFF;
    localstorageItem.value = this.cbxStaffCtrl.value;
    this.localStorageManageService.set(localstorageItem);


    HtmlUtil.nextFocusByName(this.elementRef, 'staffCodeFromCtrl', eventType);

  } 

  /////////////////////////////////////////////////////////////////////////////////

  public setCustomerCodeFrom(eventType: string) {

    if(eventType!=EVENT_TYPE.BLUR){
      this.customerCodeFromTrigger.closePanel();
    }

    if (!StringUtil.IsNullOrEmpty(this.customerCodeFromCtrl.value)) {

      if (this.userInfoService.ApplicationControl.customerCodeType == CODE_TYPE.NUMBER) {
        this.customerCodeFromCtrl.setValue(StringUtil.setPaddingFrontZero(this.customerCodeFromCtrl.value, this.userInfoService.ApplicationControl.customerCodeLength));
      }      

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
      this.customerCodeToTrigger.closePanel();
    }

    if (!StringUtil.IsNullOrEmpty(this.customerCodeToCtrl.value)) {

      if (this.userInfoService.ApplicationControl.customerCodeType == CODE_TYPE.NUMBER) {
        this.customerCodeToCtrl.setValue(StringUtil.setPaddingFrontZero(this.customerCodeToCtrl.value, this.userInfoService.ApplicationControl.customerCodeLength));
      }

      this.loadStart();
      this.customerService.GetItems(this.customerCodeToCtrl.value)
        .subscribe(response => {
          this.loadEnd();
          if (response != PROCESS_RESULT_RESULT_TYPE.FAILURE && response.length > 0) {
            this.customerCodeToCtrl.setValue(response[0].code);
            this.customerNameToCtrl.setValue(response[0].name);

            HtmlUtil.nextFocusByName(this.elementRef, 'customerNameCtrl', eventType);
          }
          else {
            //this.customerCodeToCtrl.setValue("");
            this.customerNameToCtrl.setValue("");
            HtmlUtil.nextFocusByName(this.elementRef, 'customerNameCtrl', eventType);
          }
        });
    }
    else {
      this.customerCodeToCtrl.setValue("");
      this.customerNameToCtrl.setValue("");
      HtmlUtil.nextFocusByName(this.elementRef, 'customerNameCtrl', eventType);
    }
  }

  public setCustomerName(eventType: string) {
    HtmlUtil.nextFocusByNames(this.elementRef, ['customerKanaCtrl', 'amountRangeCtrl'], eventType);
  }

  public setCustomerKana(eventType: string) {
    HtmlUtil.nextFocusByNames(this.elementRef, ['currencyCodeCtrl', 'amountRangeCtrl'], eventType);
  }

  public inputCustomerKana() {
    let value = this.customerKanaCtrl.value;
    value = EbDataHelper.convertToValidEbkana(value);
    value = EbDataHelper.removePersonalities(value, this.juridicalPersonalitiesResult.juridicalPersonalities);
    this.customerKanaCtrl.setValue(value);
  }


  public setCbxCustomer(eventType: string) {

    let localstorageItem = new LocalStorageItem();
    localstorageItem.key = RangeSearchKey.PC0301_CUSTOMER;
    localstorageItem.value = this.cbxCustomerCtrl.value;
    this.localStorageManageService.set(localstorageItem);

    HtmlUtil.nextFocusByName(this.elementRef, 'customerCodeFromCtrl', eventType);

  }    
  /////////////////////////////////////////////////////////////////////////////////

  public setCbxUseReceiptSection(eventType: string) {

    let localstorageItem = new LocalStorageItem();
    localstorageItem.key = RangeSearchKey.PC0301_USE_RECEIPT_SECTION;
    localstorageItem.value = this.cbxUseReceiptSectionCtrl.value;
    this.localStorageManageService.set(localstorageItem);

    HtmlUtil.nextFocusByName(this.elementRef, 'customerKanaCtrl', eventType);

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
  public setAmountRange(eventType: string) {
    HtmlUtil.nextFocusByName(this.elementRef, 'amountFromCtrl', eventType);
  }

  public setAmountFrom(eventType: string) {

    if (this.cbxAmountCtrl.value == true) {
      HtmlUtil.nextFocusByName(this.elementRef, 'note1Ctrl', eventType);
    }
    else {
      HtmlUtil.nextFocusByName(this.elementRef, 'amountToCtrl', eventType);
    }
  }

  public setCbxAmount(eventType: string) {

    let localstorageItem = new LocalStorageItem();
    localstorageItem.key = RangeSearchKey.PC0301_AMOUNT;
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

  /////////////////////////////////////////////////////////////////////////////////
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

  public setDispFooterTotalSum() {
    this.dispSumCount = this.billingsResult.billings.length;
    this.dispSumBillingAmount = 0;
    this.dispSumRemainAmount = 0;

    this.billingsResult.billings.forEach(element => {
      this.dispSumBillingAmount += element.billingAmount;
      this.dispSumRemainAmount += element.remainAmount;
    });
  }

  public initializeDepartmentSelection() {
    if (this.matchingService.collationInfo.departments.length == this.matchingService.collationInfo.departmentIds.length) {
      this.departmentNameCtrl.setValue("すべて");
    }
    else if (this.matchingService.collationInfo.departmentIds.length == 1) {
      this.departmentNameCtrl.setValue(this.matchingService.collationInfo.departments[0].name);
    }
    else {
      this.departmentNameCtrl.setValue("請求部門絞込有");
    }

    this.departmentIdsInner = this.matchingService.collationInfo.departmentIds;
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
    event.stopPropagation();
  }

  public isFixedColumn(setting: GridSetting): boolean {
    return (
                setting.columnName === 'Memo'
          );
  }  

}
