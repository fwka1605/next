import { Component, OnInit, ViewContainerRef, ComponentFactoryResolver, ElementRef, AfterViewInit, ViewChild } from '@angular/core';
import { BaseComponent } from 'src/app/component/common/base/base-component';
import { Router, ActivatedRoute, ParamMap, NavigationEnd } from '@angular/router';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { TABLE_INDEX } from 'src/app/common/const/table-name.const';
import { KEY_CODE, EVENT_TYPE, BUTTON_ACTION } from 'src/app/common/const/event.const';
import { ModalMasterComponent } from 'src/app/component/modal/modal-master/modal-master.component';
import { HtmlUtil } from 'src/app/common/util/html.util';
import { RECEIPT_INPUT_TYPE_DICTIONARY, RECEIPT_EXCLUDE_FLAG_DICTIONARY, ACCOUNT_TYPE_DICTIONARY, CategoryType,  CODE_TYPE, MATCHING_PROCESS_TYPE_DICTIONARY } from 'src/app/common/const/kbn.const';
import { ReceiptService } from 'src/app/service/receipt.service';
import { CategoryMasterService } from 'src/app/service/Master/category-master.service';
import { FormatterUtil } from 'src/app/common/util/formatter.util';
import { ModalMasterBankAccountComponent } from 'src/app/component/modal/modal-master-bank-account/modal-master-bank-account.component';
import { StringUtil } from 'src/app/common/util/string-util';
import { RacCurrencyPipe } from 'src/app/pipe/currency.pipe';
import { MODAL_STATUS_TYPE, PROCESS_RESULT_RESULT_TYPE } from 'src/app/common/const/status.const';
import { UserInfoService } from 'src/app/service/common/user-info.service';
import { NgbDateParserFormatter } from '@ng-bootstrap/ng-bootstrap';
import { NgbDateCustomParserFormatter } from 'src/app/common/util/dateformat-util';
import { CurrencyMasterService } from 'src/app/service/Master/currency-master.service';
import { CustomerMasterService } from 'src/app/service/Master/customer-master.service';
import { SectionMasterService } from 'src/app/service/Master/section-master.service';
import { LoginUserMasterService } from 'src/app/service/Master/login-user-master.service';
import { JuridicalPersonalityMasterService } from 'src/app/service/Master/juridical-personality-master.service';
import { ColumnNameSettingMasterService } from 'src/app/service/Master/column-name-setting-master.service';
import { forkJoin } from 'rxjs';
import { CategoriesResult } from 'src/app/model/categories-result.model';
import { Category } from 'src/app/model/category.model';
import { ColumnNameSettingsResult } from 'src/app/model/column-name-settings-result.model';
import { JuridicalPersonalitysResult } from 'src/app/model/juridical-personalitys-result.model';
import { MatchingHistoryService } from 'src/app/service/matching-history.service';
import { MatchingHistorySearch } from 'src/app/model/matching-history-search.model';
import { MatchingHistorysResult } from 'src/app/model/matching-historys-result.model';
import { SearchData } from 'src/app/model/report/search-data';
import { DateUtil } from 'src/app/common/util/date-util';
import { DepartmentMasterService } from 'src/app/service/Master/department-master.service';
import { ColumnNameSettigUtil } from 'src/app/common/util/column-name-setting-util';
import { MatchingHistory } from 'src/app/model/matching-history.model';
import { ProcessResultService } from 'src/app/service/common/process-result.service';
import { MSG_ERR, MSG_WNG, MSG_ITEM_NUM, MSG_INF } from 'src/app/common/const/message.const';
import { PageUtil } from 'src/app/common/util/page-util';
import { ExportMatchingIndividual } from 'src/app/model/export-matching-individual.model';
import { FILE_EXTENSION } from 'src/app/common/const/eb-file.const';
import { FileUtil } from 'src/app/common/util/file.util';
import { LINE_FEED_CODE } from 'src/app/common/const/report-setting-kbn.const';
import { EbDataHelper } from 'src/app/common/util/eb-data-helper';
import { stringify } from '@angular/core/src/render3/util';
import { LocalStorageItem } from 'src/app/model/custom-model/local-storage-item';
import { RangeSearchKey } from 'src/app/common/const/local-storage-key-const';
import { LocalStorageManageService } from 'src/app/service/common/local-storage-manage.service';
import { ModalRouterProgressComponent } from 'src/app/component/modal/modal-router-progress/modal-router-progress.component';
import { MatAutocompleteTrigger, MatExpansionPanel } from '@angular/material';

@Component({
  selector: 'app-pe0301-matching-history-search',
  templateUrl: './pe0301-matching-history-search.component.html',
  styleUrls: ['./pe0301-matching-history-search.component.css'],
  providers: [
    { provide: NgbDateParserFormatter, useClass: NgbDateCustomParserFormatter }
  ]
})
export class Pe0301MatchingHistorySearchComponent extends BaseComponent implements OnInit,AfterViewInit {

  // これをしないとEnum型がテンプレートで見えなかったため
  public readonly TABLE_INDEX: typeof TABLE_INDEX = TABLE_INDEX;

  public readonly accountTypeDictionary = ACCOUNT_TYPE_DICTIONARY;

  public readonly MATCHING_PROCESS_TYPE_DICTIONARY = MATCHING_PROCESS_TYPE_DICTIONARY;


  public billingCategoriesResult: CategoriesResult;
  public billingColumnNameSettingsResult: ColumnNameSettingsResult;
  public receiptColumnNameSettingsResult: ColumnNameSettingsResult;
  public juridicalPersonalitysResult: JuridicalPersonalitysResult;

  public enableBackButton: boolean;

  //////////////////////////////////////////////////////////////
  public readonly inputTypeDictionary = RECEIPT_INPUT_TYPE_DICTIONARY;
  public readonly excludeFlagDictionary = RECEIPT_EXCLUDE_FLAG_DICTIONARY;

  public matchingHistorysResult: MatchingHistorysResult;
  public matchingHistorySearch: MatchingHistorySearch;
  //////////////////////////////////////////////////////////////

  public rdoOrderCtrl: FormControl;  // 出力順指定(取込日,得意先コード順,入金ID順)

  public createdAtFromCtrl: FormControl;  // 取込日
  public createdAtToCtrl: FormControl;

  public recordedAtFromCtrl: FormControl;  // 入金日
  public recordedAtToCtrl: FormControl;

  public receiptIdFromCtrl: FormControl;  // 入金ID
  public receiptIdToCtrl: FormControl;

  public currencyCodeCtrl: FormControl;  // 通貨コード

  public bankCodeCtrl: FormControl;  // 銀行情報
  public branchCodeCtrl: FormControl;
  public accountNumberCtrl: FormControl;
  public accountTypeIdCtrl: FormControl;

  public rdoDispalyVirtualAccountCtrl: FormControl;    // 専用入金口座(表示する,表示しない)

  public rdoTaxExcludedCtrl: FormControl; // 税抜金額(表示する,表示しない)

  public clearedAtFromCtrl: FormControl; // 消込日
  public clearedAtHourFromCtrl: FormControl;
  public clearedAtToCtrl: FormControl; // 消込日
  public clearedAtHourToCtrl: FormControl;

  public customerCodeFromCtrl: FormControl;  // 得意先コード
  public customerNameFromCtrl: FormControl;
  public customerCodeToCtrl: FormControl;
  public customerNameToCtrl: FormControl;
  public cbxCustomerCtrl: FormControl;

  public departmentCodeFromCtrl: FormControl;  // 請求部門
  public departmentNameFromCtrl: FormControl;
  public departmentCodeToCtrl: FormControl;
  public departmentNameToCtrl: FormControl;
  public cbxDepartmentCtrl: FormControl;

  public sectionCodeFromCtrl: FormControl; // 入金部門子コード
  public sectionNameFromCtrl: FormControl;
  public sectionCodeToCtrl: FormControl;
  public sectionNameToCtrl: FormControl;
  public cbxSectionCtrl: FormControl;

  public cbxUseReceiptSectionCtrl: FormControl;  // 入金部門対応マスターを使用

  public loginUserCodeFromCtrl: FormControl; // 実行ユーザ
  public loginUserNameFromCtrl: FormControl;
  public loginUserCodeToCtrl: FormControl;
  public loginUserNameToCtrl: FormControl;
  public cbxLoginUserCtrl: FormControl;

  public cbxMemoCtrl: FormControl; // メモ
  public receiptMemoCtrl: FormControl;

  public cbxSequentialAssignmentFlagCtrl: FormControl; // 消込種別(一括)
  public cbxIndividualAssignmentFlagCtrl: FormControl; // 消込種別(個別)

  public billingAmountFromCtrl: FormControl;// 請求額(税込)
  public billingAmountToCtrl: FormControl;

  public receiptAmountFromCtrl: FormControl;// 入金額
  public receiptAmountToCtrl: FormControl;

  public payerNameCtrl: FormControl;// 振込依頼人名

  public billingCategoryIdCtrl: FormControl; // 請求区分
  public billingCategoryCodeCtrl: FormControl; // 請求区分
  public billingCategoryNameCtrl: FormControl; // 請求区分

  public cbxOputputAtCtrl: FormControl; // 未出力分のみ

  public rdoTakeTotalCtrl: FormControl; // 計(とる,とらない)

  public undefineCtrl: FormControl; // 未定用


  public panelOpenState: boolean; // パネルのオープン、クローズ

  public simplePageSearch:boolean=true; // 簡易・詳細検索

  @ViewChild('customerCodeFromInput', { read: MatAutocompleteTrigger }) customerCodeFromTrigger: MatAutocompleteTrigger;
  @ViewChild('customerCodeToInput', { read: MatAutocompleteTrigger }) customerCodeToTrigger: MatAutocompleteTrigger;
  @ViewChild('departmentCodeFromInput', { read: MatAutocompleteTrigger }) departmentCodeFromTrigger: MatAutocompleteTrigger;
  @ViewChild('departmentCodeToInput', { read: MatAutocompleteTrigger }) departmentCodeToTrigger: MatAutocompleteTrigger;
  @ViewChild('sectionCodeFromInput', { read: MatAutocompleteTrigger }) sectionCodeFromTrigger: MatAutocompleteTrigger;
  @ViewChild('sectionCodeToInput', { read: MatAutocompleteTrigger }) sectionCodeToTrigger: MatAutocompleteTrigger;
  @ViewChild('loginUserCodeFromInput', { read: MatAutocompleteTrigger }) loginUserCodeFromTrigger: MatAutocompleteTrigger;
  @ViewChild('loginUserCodeToInput', { read: MatAutocompleteTrigger }) loginUserCodeToTrigger: MatAutocompleteTrigger;
  @ViewChild('billingCategoryCodeInput', { read: MatAutocompleteTrigger }) billingCategoryCodeTrigger: MatAutocompleteTrigger;

  @ViewChild('panel') panel:MatExpansionPanel;

  constructor(
    public elementRef: ElementRef,
    public componentFactoryResolver: ComponentFactoryResolver,
    public viewContainerRef: ViewContainerRef,
    public router: Router,
    public activatedRoute: ActivatedRoute,
    public userInfoService: UserInfoService,
    public categoryService: CategoryMasterService,
    public columnNameSettingService: ColumnNameSettingMasterService,
    public juridicalPersonalityService: JuridicalPersonalityMasterService,
    public receiptSearvice: ReceiptService,
    public currencyService: CurrencyMasterService,
    public customerService: CustomerMasterService,
    public sectionService: SectionMasterService,
    public loginUserService: LoginUserMasterService,
    public matchingHistoryService: MatchingHistoryService,
    public departmentService: DepartmentMasterService,
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

    this.enableBackButton = false;

    this.activatedRoute.paramMap.subscribe((params: ParamMap) => {
      // ルートパラメータと同じ要領で Matrix URI パラメータが取得できる
      let param = params.get("process");

      if (!StringUtil.IsNullOrEmpty(param) && param == "from") {
        this.enableBackButton = true;
      }
      this.setControlInit();
      this.setValidator();
      this.setFormatter();
      this.clear();
      this.setAutoComplete();
    });



    let categoryResponse = this.categoryService.GetItems(CategoryType.Billing);
    let billingColumnNameSettingResponse = this.columnNameSettingService.Get(CategoryType.Billing);
    let receiptColumnNameSettingResponse = this.columnNameSettingService.Get(CategoryType.Receipt);
    let juridicalPersonalityResponse = this.juridicalPersonalityService.GetItems();

    forkJoin(
      categoryResponse,
      billingColumnNameSettingResponse,
      receiptColumnNameSettingResponse,
      juridicalPersonalityResponse
    )
      .subscribe(responseList => {

        if (responseList != undefined && responseList.length == 4) {
          this.billingCategoriesResult = new CategoriesResult();
          this.billingCategoriesResult.categories = responseList[0];

          this.billingColumnNameSettingsResult = new ColumnNameSettingsResult();
          this.billingColumnNameSettingsResult.columnNames = responseList[1];

          this.receiptColumnNameSettingsResult = new ColumnNameSettingsResult();
          this.receiptColumnNameSettingsResult.columnNames = responseList[2];

          this.juridicalPersonalitysResult = new JuridicalPersonalitysResult();
          this.juridicalPersonalitysResult.juridicalPersonalities = responseList[3];

        }
        else {
          this.processCustomResult = this.processResultService.processAtFailure(
            this.processCustomResult, MSG_ERR.SOMETHING_ERROR.replace(MSG_ITEM_NUM.FIRST, '初期化'),
            this.partsResultMessageComponent);
        }
      });
  }

  ngAfterViewInit(){
    HtmlUtil.nextFocusByName(this.elementRef, 'createdAtFromCtrl', EVENT_TYPE.NONE);

  }

  public setControlInit() {

    this.rdoOrderCtrl = new FormControl("0");  // 出力順指定(取込日,得意先コード順,入金ID順)

    this.createdAtFromCtrl = new FormControl("", [Validators.maxLength(10)]);  // 取込日
    this.createdAtToCtrl = new FormControl("", [Validators.maxLength(10)]);

    this.recordedAtFromCtrl = new FormControl("", [Validators.maxLength(10)]);  // 入金日
    this.recordedAtToCtrl = new FormControl("", [Validators.maxLength(10)]);

    this.receiptIdFromCtrl = new FormControl("");  // 入金ID
    this.receiptIdToCtrl = new FormControl("");

    if (this.userInfoService.ApplicationControl.useForeignCurrency == 1) {
      this.currencyCodeCtrl = new FormControl("", [Validators.required, Validators.maxLength(3)]);  // 通貨コード
    }
    else {
      this.currencyCodeCtrl = new FormControl("", [Validators.maxLength(3)]);  // 通貨コード
    }

    this.bankCodeCtrl = new FormControl("", [Validators.maxLength(4)]);  // 銀行情報
    this.branchCodeCtrl = new FormControl("", [Validators.maxLength(3)]);
    this.accountNumberCtrl = new FormControl("", [Validators.maxLength(7)]);
    this.accountTypeIdCtrl = new FormControl(this.accountTypeDictionary[0]);

    this.rdoDispalyVirtualAccountCtrl = new FormControl("");  // 専用入金口座(表示する,表示しない)

    this.rdoTaxExcludedCtrl = new FormControl("");  // 税抜金額(表示する,表示しない)

    this.clearedAtFromCtrl = new FormControl("", [Validators.maxLength(10)]);  // 消込日
    this.clearedAtToCtrl = new FormControl("", [Validators.maxLength(10)]);
    this.clearedAtHourToCtrl = new FormControl("0");
    this.clearedAtHourFromCtrl = new FormControl("0");

    this.customerCodeFromCtrl = new FormControl("", [Validators.maxLength(this.userInfoService.ApplicationControl.customerCodeLength)]);  // 得意先コード
    this.customerNameFromCtrl = new FormControl("");
    this.customerCodeToCtrl = new FormControl("", [Validators.maxLength(this.userInfoService.ApplicationControl.customerCodeLength)]);
    this.customerNameToCtrl = new FormControl("");
    this.cbxCustomerCtrl = new FormControl("");

    this.departmentCodeFromCtrl = new FormControl("", [Validators.maxLength(this.userInfoService.ApplicationControl.departmentCodeLength)]);  // 請求部門
    this.departmentNameFromCtrl = new FormControl("");
    this.departmentCodeToCtrl = new FormControl("", [Validators.maxLength(this.userInfoService.ApplicationControl.departmentCodeLength)]);
    this.departmentNameToCtrl = new FormControl("");
    this.cbxDepartmentCtrl = new FormControl("");

    this.sectionCodeFromCtrl = new FormControl("", [Validators.maxLength(this.userInfoService.ApplicationControl.sectionCodeLength)]); // 入金部門コード
    this.sectionNameFromCtrl = new FormControl("");
    this.sectionCodeToCtrl = new FormControl("", [Validators.maxLength(this.userInfoService.ApplicationControl.sectionCodeLength)]);
    this.sectionNameToCtrl = new FormControl("");
    this.cbxSectionCtrl = new FormControl("");

    this.cbxUseReceiptSectionCtrl = new FormControl("");  // 入金部門対応マスターを使用

    this.loginUserCodeFromCtrl = new FormControl("", [Validators.maxLength(this.userInfoService.ApplicationControl.loginUserCodeLength)]); // 実行ユーザ
    this.loginUserNameFromCtrl = new FormControl("");
    this.loginUserCodeToCtrl = new FormControl("", [Validators.maxLength(this.userInfoService.ApplicationControl.loginUserCodeLength)]);
    this.loginUserNameToCtrl = new FormControl("");
    this.cbxLoginUserCtrl = new FormControl({ value: false });

    this.cbxMemoCtrl = new FormControl(""); // メモ
    this.receiptMemoCtrl = new FormControl("", [Validators.maxLength(100)]);

    this.cbxSequentialAssignmentFlagCtrl = new FormControl(""); // 消込種別(一括)
    this.cbxIndividualAssignmentFlagCtrl = new FormControl(""); // 消込種別(個別)

    this.billingAmountFromCtrl = new FormControl("", [Validators.maxLength(15)]); // 請求額(税込)
    this.billingAmountToCtrl = new FormControl("", [Validators.maxLength(15)]);

    this.receiptAmountFromCtrl = new FormControl("", [Validators.maxLength(15)]); // 入金額
    this.receiptAmountToCtrl = new FormControl("", [Validators.maxLength(15)]);

    this.payerNameCtrl = new FormControl(""); // 振込依頼人名

    this.billingCategoryIdCtrl = new FormControl(""); // 請求区分
    this.billingCategoryCodeCtrl = new FormControl(""); // 請求区分
    this.billingCategoryNameCtrl = new FormControl(""); // 請求区分

    this.cbxOputputAtCtrl = new FormControl(""); // 未出力分のみ

    this.rdoTakeTotalCtrl = new FormControl("0"); // 計(とる,とらない)

    this.undefineCtrl = new FormControl(""); // 未定用;


  }

  public setValidator() {
    this.MyFormGroup = new FormGroup({

      rdoOrderCtrl: this.rdoOrderCtrl,  // 出力順指定(取込日,得意先コード順,入金ID順)

      createdAtFromCtrl: this.createdAtFromCtrl,  // 取込日
      createdAtToCtrl: this.createdAtToCtrl,

      recordedAtFromCtrl: this.recordedAtFromCtrl,  // 入金日
      recordedAtToCtrl: this.recordedAtToCtrl,

      receiptIdFromCtrl: this.receiptIdFromCtrl,  // 入金ID
      receiptIdToCtrl: this.receiptIdToCtrl,

      currencyCodeCtrl: this.currencyCodeCtrl,  // 通貨コード

      bankCodeCtrl: this.bankCodeCtrl,  // 銀行情報
      branchCodeCtrl: this.branchCodeCtrl,
      accountNumberCtrl: this.accountNumberCtrl,
      accountTypeIdCtrl: this.accountTypeIdCtrl,

      rdoDispalyVirtualAccountCtrl: this.rdoDispalyVirtualAccountCtrl,  // 専用入金口座(表示する,表示しない)

      rdoTaxExcludedCtrl: this.rdoTaxExcludedCtrl,  // 税抜金額(表示する,表示しない)

      clearedAtFromCtrl: this.clearedAtFromCtrl,  // 消込日
      clearedAtToCtrl: this.clearedAtToCtrl,
      clearedAtHourToCtrl: this.clearedAtHourToCtrl,
      clearedAtHourFromCtrl: this.clearedAtHourFromCtrl,

      customerCodeFromCtrl: this.customerCodeFromCtrl,  // 得意先コード
      customerNameFromCtrl: this.customerNameFromCtrl,
      customerCodeToCtrl: this.customerCodeToCtrl,
      customerNameToCtrl: this.customerNameToCtrl,
      cbxCustomerCtrl: this.cbxCustomerCtrl,

      departmentCodeFromCtrl: this.departmentCodeFromCtrl,  // 請求部門
      departmentNameFromCtrl: this.departmentNameFromCtrl,
      departmentCodeToCtrl: this.departmentCodeToCtrl,
      departmentNameToCtrl: this.departmentNameToCtrl,
      cbxDepartmentCtrl: this.cbxDepartmentCtrl,

      sectionCodeFromCtrl: this.sectionCodeFromCtrl, // 入金部門子コード
      sectionNameFromCtrl: this.sectionNameFromCtrl,
      sectionCodeToCtrl: this.sectionCodeToCtrl,
      sectionNameToCtrl: this.sectionNameToCtrl,
      cbxSectionCtrl: this.cbxSectionCtrl,

      cbxUseReceiptSectionCtrl: this.cbxUseReceiptSectionCtrl,  // 入金部門対応マスターを使用

      loginUserCodeFromCtrl: this.loginUserCodeFromCtrl, // 実行ユーザ
      loginUserNameFromCtrl: this.loginUserNameFromCtrl,
      loginUserCodeToCtrl: this.loginUserCodeToCtrl,
      loginUserNameToCtrl: this.loginUserNameToCtrl,
      cbxLoginUserCtrl: this.cbxLoginUserCtrl,

      cbxMemoCtrl: this.cbxMemoCtrl, // メモ
      receiptMemoCtrl: this.receiptMemoCtrl,

      cbxSequentialAssignmentFlagCtrl: this.cbxSequentialAssignmentFlagCtrl, // 消込種別(一括)
      cbxIndividualAssignmentFlagCtrl: this.cbxIndividualAssignmentFlagCtrl,  // 消込種別(個別)

      billingAmountFromCtrl: this.billingAmountFromCtrl,  // 請求額(税込)
      billingAmountToCtrl: this.billingAmountToCtrl,

      receiptAmountFromCtrl: this.receiptAmountFromCtrl,  // 入金額
      receiptAmountToCtrl: this.receiptAmountToCtrl,

      payerNameCtrl: this.payerNameCtrl,  // 振込依頼人名

      billingCategoryIdCtrl: this.billingCategoryIdCtrl,  // 請求区分
      billingCategoryCodeCtrl: this.billingCategoryCodeCtrl,  // 請求区分
      billingCategoryNameCtrl: this.billingCategoryNameCtrl,  // 請求区分

      cbxOputputAtCtrl: this.cbxOputputAtCtrl,  // 未出力分のみ

      rdoTakeTotalCtrl: this.rdoTakeTotalCtrl,  // 計(とる,とらない)


      undefineCtrl: this.undefineCtrl, // 未定用

    });


  }

  public setFormatter() {

    FormatterUtil.setNumberFormatter(this.receiptIdFromCtrl);// 入金ID
    FormatterUtil.setNumberFormatter(this.receiptIdToCtrl);

    FormatterUtil.setNumberFormatter(this.bankCodeCtrl);// 銀行情報
    FormatterUtil.setNumberFormatter(this.branchCodeCtrl);
    FormatterUtil.setNumberFormatter(this.accountNumberCtrl);

    FormatterUtil.setAlphabetFormatter(this.currencyCodeCtrl);  // 通貨コード

    if (this.userInfoService.ApplicationControl.customerCodeType == CODE_TYPE.NUMBER) {
      FormatterUtil.setNumberFormatter(this.customerCodeFromCtrl);// 得意先コード
      FormatterUtil.setNumberFormatter(this.customerCodeToCtrl);
    }
    else if(this.userInfoService.ApplicationControl.customerCodeType == CODE_TYPE.HANKANA) {
      FormatterUtil.setCustomerCodeKanaFormatter(this.customerCodeFromCtrl);
      FormatterUtil.setCustomerCodeKanaFormatter(this.customerCodeToCtrl);
    }
    else {
      FormatterUtil.setCustomerCodeAlphaFormatter(this.customerCodeFromCtrl);
      FormatterUtil.setCustomerCodeAlphaFormatter(this.customerCodeToCtrl);
    }

    if (this.userInfoService.ApplicationControl.departmentCodeType == CODE_TYPE.NUMBER) {
      FormatterUtil.setNumberFormatter(this.departmentCodeFromCtrl);// 請求部門コード
      FormatterUtil.setNumberFormatter(this.departmentCodeToCtrl);
    }
    else {
      FormatterUtil.setCodeFormatter(this.departmentCodeFromCtrl);
      FormatterUtil.setCodeFormatter(this.departmentCodeToCtrl);
    }

    if (this.userInfoService.ApplicationControl.sectionCodeType == CODE_TYPE.NUMBER) {
      FormatterUtil.setNumberFormatter(this.sectionCodeFromCtrl);// 入金部門コード
      FormatterUtil.setNumberFormatter(this.sectionCodeToCtrl);
    }
    else {
      FormatterUtil.setCodeFormatter(this.sectionCodeFromCtrl);
      FormatterUtil.setCodeFormatter(this.sectionCodeToCtrl);
    }

    if (this.userInfoService.ApplicationControl.loginUserCodeType == CODE_TYPE.NUMBER) {
      FormatterUtil.setNumberFormatter(this.loginUserCodeFromCtrl);// 実行ユーザ
      FormatterUtil.setNumberFormatter(this.loginUserCodeToCtrl);
    }
    else {
      FormatterUtil.setCodeFormatter(this.loginUserCodeFromCtrl);// 実行ユーザ
      FormatterUtil.setCodeFormatter(this.loginUserCodeToCtrl);
    }

    FormatterUtil.setCurrencyFormatter(this.billingAmountFromCtrl); // 請求額
    FormatterUtil.setCurrencyFormatter(this.billingAmountToCtrl);

    FormatterUtil.setCurrencyFormatter(this.receiptAmountFromCtrl); // 入金額
    FormatterUtil.setCurrencyFormatter(this.receiptAmountToCtrl);
  }

  
  public setAutoComplete(){

    // 得意先
    this.initAutocompleteCustomers(this.customerCodeFromCtrl,this.customerService,0);
    this.initAutocompleteCustomers(this.customerCodeToCtrl,this.customerService,1);

    // 請求部門
    this.initAutocompleteDepartments(this.departmentCodeFromCtrl,this.departmentService,0);
    this.initAutocompleteDepartments(this.departmentCodeToCtrl,this.departmentService,1);

    // 入金部門
    this.initAutocompleteSections(this.sectionCodeFromCtrl,this.sectionService,0);
    this.initAutocompleteSections(this.sectionCodeToCtrl,this.sectionService,1);

    // 最終更新者
    this.initAutocompleteLoginUsers(this.loginUserCodeFromCtrl,this.loginUserService,0);
    this.initAutocompleteLoginUsers(this.loginUserCodeToCtrl,this.loginUserService,1);

    // 請求区分
    this.initAutocompleteCategories(CategoryType.Billing,this.billingCategoryCodeCtrl,this.categoryService,0);

  }

  public closeAutoCompletePanel(){

    if (this.customerCodeFromTrigger!= undefined){this.customerCodeFromTrigger.closePanel();}
    if (this.customerCodeToTrigger!= undefined){this.customerCodeToTrigger.closePanel();}
    if (this.sectionCodeFromTrigger!= undefined){this.departmentCodeFromTrigger.closePanel();}
    if (this.sectionCodeToTrigger!= undefined){this.departmentCodeToTrigger.closePanel();}
    if (this.sectionCodeFromTrigger!= undefined){this.sectionCodeFromTrigger.closePanel();}
    if (this.sectionCodeToTrigger!= undefined){this.sectionCodeToTrigger.closePanel();}
    if (this.loginUserCodeFromTrigger!= undefined){this.loginUserCodeFromTrigger.closePanel();}
    if (this.loginUserCodeToTrigger!= undefined){this.loginUserCodeToTrigger.closePanel();}
    if (this.billingCategoryCodeTrigger!= undefined){this.billingCategoryCodeTrigger.closePanel();}
  
  }

  public openBankAccountMasterModal() {
    this.closeAutoCompletePanel();
    let componentFactory = this.componentFactoryResolver.resolveComponentFactory(ModalMasterBankAccountComponent);
    let componentRef = this.viewContainerRef.createComponent(componentFactory);

    componentRef.instance.Closing.subscribe(() => {
      this.bankCodeCtrl.setValue(componentRef.instance.SelectedBankCode);
      this.branchCodeCtrl.setValue(componentRef.instance.SelectedBranchCode);
      this.accountTypeIdCtrl.setValue(this.accountTypeDictionary[componentRef.instance.SelectedAccountTypeId]);
      this.accountNumberCtrl.setValue(componentRef.instance.SelectedAccountNumber);

      componentRef.destroy();
    });

  }

  public openMasterModal(table: TABLE_INDEX,  type: string) {

    this.closeAutoCompletePanel();
      
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
          case TABLE_INDEX.MASTER_SECTION:
            {
              if (type === "from") {
                this.sectionCodeFromCtrl.setValue(componentRef.instance.SelectedCode);
                this.sectionNameFromCtrl.setValue(componentRef.instance.SelectedName);

                if (this.cbxSectionCtrl.value == true) {
                  this.sectionCodeToCtrl.setValue(componentRef.instance.SelectedCode);
                  this.sectionNameToCtrl.setValue(componentRef.instance.SelectedName);
                }


              }
              else {
                this.sectionCodeToCtrl.setValue(componentRef.instance.SelectedCode);
                this.sectionNameToCtrl.setValue(componentRef.instance.SelectedName);
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
              if (type === "from") {
                this.loginUserCodeFromCtrl.setValue(componentRef.instance.SelectedCode);
                this.loginUserNameFromCtrl.setValue(componentRef.instance.SelectedName);

                if (this.cbxLoginUserCtrl.value == true) {
                  this.loginUserCodeToCtrl.setValue(componentRef.instance.SelectedCode);
                  this.loginUserNameToCtrl.setValue(componentRef.instance.SelectedName);
                }
              }
              else {
                this.loginUserCodeToCtrl.setValue(componentRef.instance.SelectedCode);
                this.loginUserNameToCtrl.setValue(componentRef.instance.SelectedName);
              }
              break;
            }

          case TABLE_INDEX.MASTER_BILLING_CATEGORY:
            {
              this.billingCategoryIdCtrl.setValue(componentRef.instance.SelectedId);
              this.billingCategoryCodeCtrl.setValue(componentRef.instance.SelectedCode);
              this.billingCategoryNameCtrl.setValue(componentRef.instance.SelectedName);
              break;
            }
        }
      }

      componentRef.destroy();
    });
  }

  public getBillingColumnAlias(columnName: string): string {
    if (this.billingColumnNameSettingsResult != null) {
      let tmp = ColumnNameSettigUtil.getColumnAlias(this.billingColumnNameSettingsResult.columnNames, columnName);
      return tmp;
    }
    return "";
  }

  public getReceiptColumnAlias(columnName: string): string {
    if (this.receiptColumnNameSettingsResult != null) {
      let tmp = ColumnNameSettigUtil.getColumnAlias(this.receiptColumnNameSettingsResult.columnNames, columnName);
      return tmp;
    }
    return "";
  }


  public clear() {

    this.MyFormGroup.reset();

    //this.matchingHistorysResult = null;

    this.rdoOrderCtrl.setValue("0");   //出力順指定(消込日順)

    this.accountTypeIdCtrl.setValue(this.accountTypeDictionary[0]);

    this.rdoDispalyVirtualAccountCtrl.setValue("0");  // 専用入金口座(表示する)
    this.rdoTaxExcludedCtrl.setValue("1");  // 税抜き金額(表示しない)


    this.cbxSequentialAssignmentFlagCtrl.setValue("true");  // 消込種別(一括)
    this.cbxIndividualAssignmentFlagCtrl.setValue("true");  // 消込種別(個別)

    this.cbxOputputAtCtrl.setValue("true"); // 未出力分のみ
    this.rdoTakeTotalCtrl.setValue("0"); // 未出力分のみ(計をとる)
    this.rdoTakeTotalCtrl.enable();

    this.panelOpenState = true;
    this.panel.open();

    HtmlUtil.nextFocusByName(this.elementRef, 'createdAtFromCtrl', EVENT_TYPE.NONE);


    this.setRangeCheckbox();
  }


  public setRangeCheckbox() {
    let cbxCustomer = this.localStorageManageService.get(RangeSearchKey.PE0301_CUSTOMER);
    let cbxDepartment = this.localStorageManageService.get(RangeSearchKey.PE0301_DEPARTMENT);
    let cbxSection = this.localStorageManageService.get(RangeSearchKey.PE0301_SECTION);
    let cbxUseReceiptSection = this.localStorageManageService.get(RangeSearchKey.PE0301_USE_RECEIPT_SECTION);
    let cbxLoginUser = this.localStorageManageService.get(RangeSearchKey.PE0301_LOGIN_USER);

    if (cbxCustomer != null) {
      this.cbxCustomerCtrl.setValue(cbxCustomer.value);
    }

    if (cbxDepartment != null) {
      this.cbxDepartmentCtrl.setValue(cbxDepartment.value);
    }

    if (cbxSection != null) {
      this.cbxSectionCtrl.setValue(cbxSection.value);
    }

    if (cbxUseReceiptSection != null) {
      this.cbxUseReceiptSectionCtrl.setValue(cbxUseReceiptSection.value);
    }

    if (cbxLoginUser != null) {
      this.cbxLoginUserCtrl.setValue(cbxLoginUser.value);
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

    this.processCustomResult = this.processResultService.clearProcessCustomMsg(this.processCustomResult);

    if (!this.ValidateInputData()) return;

    let componentFactory = this.componentFactoryResolver.resolveComponentFactory(ModalRouterProgressComponent);
    let modalRouterProgressComponentRef = this.viewContainerRef.createComponent(componentFactory);
    //modalRouterProgressComponentRef.destroy();    

    this.matchingHistorySearch = this.CreateSearchCondition();

    this.matchingHistoryService.Get(this.matchingHistorySearch)
      .subscribe(response => {

        this.processResultService.processAtGetData(this.processCustomResult, response, true, this.partsResultMessageComponent);

        if (response != PROCESS_RESULT_RESULT_TYPE.FAILURE) {
          this.matchingHistorysResult = new MatchingHistorysResult();
          this.matchingHistorysResult.matchingHistorys = this.aggreagteHistory(response as MatchingHistory[]);

          if (this.matchingHistorysResult.matchingHistorys == null || this.matchingHistorysResult.matchingHistorys.length == 0) {
            this.panelOpenState = true;

          }
          else {
            this.panelOpenState = false;
            //this.setSumData()
          }
        }
        modalRouterProgressComponentRef.destroy();   
      });

  }



  public export( ) {

    let data: string = "";
    let matchingHistorys:Array<MatchingHistory> = this.matchingHistorysResult.matchingHistorys;

    let componentFactory = this.componentFactoryResolver.resolveComponentFactory(ModalRouterProgressComponent);
    let modalRouterProgressComponentRef = this.viewContainerRef.createComponent(componentFactory);
    //modalRouterProgressComponentRef.destroy();    

    // 入金部門
    // 専用入金口座

    // カラム名設定(入金・請求)

    // 請求額

    data = "消込日時,";
    if (this.useReceiptSection) {
      data += "入金部門コード,入金部門名,";
    }
    data += "請求部門コード,請求部門名,得意先コード,得意先名,請求日,売上日,請求書番号,請求区分,回収区分,請求金額(税込),";
    if (this.displayBillingExclude){
      data += "請求金額(税抜),消費税,";
    }
    data += "消込金額,請求残,"
              + this.getBillingColumnAlias("Note1") + ","
              + this.getBillingColumnAlias("Note2") + ","
              + this.getBillingColumnAlias("Note3") + ","
              + this.getBillingColumnAlias("Note4") + ","
              + "入金日,入金ID,入金区分,入金額,前受,入金残,振込依頼人名,銀行コード,銀行名,支店コード,支店名,口座番号,"
              + this.getReceiptColumnAlias("Note1") + ","
              + this.getReceiptColumnAlias("Note2") + ","
              + this.getReceiptColumnAlias("Note3") + ","
              + this.getReceiptColumnAlias("Note4") + ",";
    if (this.displayVirtualAccount){
      data += "仮想支店コード,仮想口座番号,";
    }
    data += "消込実行ユーザー,消込,消込メモ";
    data = FileUtil.encloseItemBySymbol(data.split(",")).join(",") + LINE_FEED_CODE;

    matchingHistorys.forEach(history => {
      let line = '';

      if (this.isDataRecord(history)) {
        line = this.getDataRecordLine(history);
      }
      else if (this.isSubtotalRecord(history)) {
        line = this.getSubtotalRecordLine(history);
      }
      else if (this.isGrandtotalRecord(history)) {
        line = this.getGrandtotalRecordLine(history);
      }

      data += line + LINE_FEED_CODE;

    });

    let result: boolean = false;
    let resultDatas: Array<any> = [];
    resultDatas.push(data);
    FileUtil.download(resultDatas, this.Title + DateUtil.getYYYYMMDD(0), FILE_EXTENSION.CSV);

    const idsDuped = matchingHistorys.filter(x => x.matchingHeaderId != undefined).map(x => x.matchingHeaderId);
    const ids = Array.from(new Set(idsDuped));
    this.matchingHistoryService.saveOutput(ids).subscribe();

    result = true;
    this.processResultService.processAtOutput(
      this.processCustomResult, result, 0, this.partsResultMessageComponent);    

    modalRouterProgressComponentRef.destroy();  
  }

  public getDataRecordLine(history: MatchingHistory): string {
    const fields = [];

    fields.push(DateUtil.getYYYYMMDD(5, history.createAt));

    if (this.useReceiptSection) {
      fields.push(history.sectionCode);
      fields.push(history.sectionName);
    }

    fields.push(history.departmentCode);
    fields.push(history.departmentName);
    fields.push(history.customerCode);
    fields.push(history.customerName);

    fields.push(DateUtil.convertDateString(history.billedAt));
    fields.push(DateUtil.convertDateString(history.salesAt));

    fields.push(history.invoiceCode);

    fields.push(this.getBillingCategoryInfo(history));
    fields.push(this.getCollectCategoryInfo(history));

    fields.push(history.billingAmount);

    if (this.displayBillingExclude) {
      fields.push(history.billingAmountExcludingTax);
      fields.push(history.taxAmount);
    }

    fields.push(history.matchingAmount);

    fields.push(history.billingRemain);

    fields.push(history.billingNote1);
    fields.push(history.billingNote2);
    fields.push(history.billingNote3);
    fields.push(history.billingNote4);


    fields.push(DateUtil.convertDateString( history.recordedAt));
    fields.push(history.receiptId || '');

    fields.push(this.getReceiptCategoryInfo(history));
    fields.push(history.receiptAmount);
    fields.push(this.getAdvancedReceivedOccured(history));
    fields.push(history.receiptRemain);

    fields.push(history.payerName);
    fields.push(history.bankCode);
    fields.push(history.bankName);
    fields.push(history.branchCode);
    fields.push(history.branchName);
    fields.push(history.accountNumber);

    fields.push(history.receiptNote1);
    fields.push(history.receiptNote2);
    fields.push(history.receiptNote3);
    fields.push(history.receiptNote4);

    if (this.displayVirtualAccount) {
      if (StringUtil.IsNullOrEmpty(history.payerCode)) {
        fields.push("");
        fields.push("");
      }
      else {
        fields.push(history.payerCode.substr(0, 3));
        fields.push(history.payerCode.substr(3, 7));
      }
    }

    fields.push(history.loginUserName);
    fields.push(MATCHING_PROCESS_TYPE_DICTIONARY[history.matchingProcessType].val);
    fields.push(history.matchingMemo);

    return FileUtil.encloseItemBySymbol(fields).join(',');
  }

  public getSubtotalRecordLine(history: MatchingHistory): string {
    const fields = [];
    fields.push(`*消込日時（消込単位）計：${DateUtil.getYYYYMMDD(5, history.createAt)}`);
    this.addBlankFields(fields, 9);

    if (this.useReceiptSection) {
      this.addBlankFields(fields, 2);
    }

    fields.push(history.billingAmount);
    if (this.displayBillingExclude) {
      fields.push(history.billingAmountExcludingTax);
      fields.push(history.taxAmount);
    }
    fields.push(history.matchingAmount);
    fields.push(history.billingRemain);

    this.addBlankFields(fields, 7);

    fields.push(history.receiptAmount);
    fields.push('');
    fields.push(history.receiptRemain);

    this.addBlankFields(fields, 10);
    if (this.displayVirtualAccount) {
      this.addBlankFields(fields, 2);
    }

    fields.push(history.loginUserName);
    fields.push(MATCHING_PROCESS_TYPE_DICTIONARY[history.matchingProcessType].val);
    fields.push(history.matchingMemo);

    return FileUtil.encloseItemBySymbol(fields).join(',');
  }

  public getGrandtotalRecordLine(history: MatchingHistory): string {
    const fields = [];
    fields.push('*総合計*');

    this.addBlankFields(fields, 10);
    if (this.useReceiptSection) {
      this.addBlankFields(fields, 2);
    }
    if (this.displayBillingExclude) {
      this.addBlankFields(fields, 2);
    }

    fields.push(history.matchingAmount);
    this.addBlankFields(fields, 23);
    if (this.displayVirtualAccount) {
      this.addBlankFields(fields, 2);
    }
    return FileUtil.encloseItemBySymbol(fields).join(',');
  }

  public addBlankFields(fields: any[], count: number) {
    for (let i = 0; i < count; i++) {
      fields.push('');
    }
  }


  public print() {

    this.processCustomResult = this.processResultService.clearProcessCustomMsg(this.processCustomResult);

    if (!this.ValidateInputData()) return;

    this.matchingHistorySearch = this.CreateSearchCondition();

    this.matchingHistoryService.Get(this.matchingHistorySearch)
      .subscribe(response => {
        if (response != undefined) {
          this.matchingHistorysResult = new MatchingHistorysResult();
          this.matchingHistorysResult.matchingHistorys = response;

          console.log(this.matchingHistorysResult.matchingHistorys.length);
          if (this.matchingHistorysResult.matchingHistorys == null || this.matchingHistorysResult.matchingHistorys.length == 0) {
            this.processCustomResult = this.processResultService.processAtWarning(
              this.processCustomResult, MSG_WNG.NOT_EXIST_SEARCH_DATA, this.partsResultMessageComponent);
          }
          else {
            this.setSumData()
          }
        }
        else {
          this.processCustomResult = this.processResultService.processAtFailure(
            this.processCustomResult, MSG_ERR.SOMETHING_ERROR.replace(MSG_ITEM_NUM.FIRST, '消込履歴データ検索'),
            this.partsResultMessageComponent);
        }
      });

  }


  public ValidateInputData(): boolean {

    if (
      StringUtil.IsNullOrEmpty(this.cbxIndividualAssignmentFlagCtrl.value)
      && StringUtil.IsNullOrEmpty(this.cbxSequentialAssignmentFlagCtrl.value)
    ) {
      this.processCustomResult = this.processResultService.processAtWarning(
        this.processCustomResult, MSG_WNG.SELECTION_REQUIRED.replace(MSG_ITEM_NUM.FIRST, '消込種別'),
        this.partsResultMessageComponent);
      return false;
    }

    if (
      (
        StringUtil.IsNullOrEmpty(this.clearedAtFromCtrl.value)
        && !StringUtil.IsNullOrEmpty(this.clearedAtHourFromCtrl.value)
      )
      || (
        !StringUtil.IsNullOrEmpty(this.clearedAtFromCtrl.value)
        && StringUtil.IsNullOrEmpty(this.clearedAtHourFromCtrl.value)
      )
      || (
        !StringUtil.IsNullOrEmpty(this.clearedAtToCtrl.value)
        && StringUtil.IsNullOrEmpty(this.clearedAtHourToCtrl.value)
      )
      || (
        !StringUtil.IsNullOrEmpty(this.clearedAtToCtrl.value)
        && StringUtil.IsNullOrEmpty(this.clearedAtHourToCtrl.value)
      )
    ) {
      this.processCustomResult = this.processResultService.processAtWarning(
        this.processCustomResult, MSG_WNG.INPUT_REQUIRED.replace(MSG_ITEM_NUM.FIRST, '消込日付と時間'),
        this.partsResultMessageComponent);
      return false;
    }


    return true;

  }

  public setSumData() {


    if (this.rdoOrderCtrl.value != "0") {
      let matchingHistoryTmp = new MatchingHistory();

      matchingHistoryTmp.departmentCode = "総計";
      matchingHistoryTmp.billingAmount = 0;
      matchingHistoryTmp.matchingAmount = 0;

      this.matchingHistorysResult.matchingHistorys.forEach(element => { matchingHistoryTmp.matchingAmount += element.matchingAmount; });

      for (let index = 0; index < this.matchingHistorysResult.matchingHistorys.length; index++) {

        matchingHistoryTmp.billingAmount += this.matchingHistorysResult.matchingHistorys[index].billingAmount;
        matchingHistoryTmp.matchingAmount += this.matchingHistorysResult.matchingHistorys[index].matchingAmount;

        console.log("" + index + ":" + matchingHistoryTmp.matchingAmount);
        console.log("" + index + ":" + matchingHistoryTmp.billingAmount);

      }
      //this.matchingHistorysResult.matchingHistorys.forEach(element=>{matchingHistoryTmp.billingAmount+= element.billingAmount; });
      this.matchingHistorysResult.matchingHistorys.push(matchingHistoryTmp);
      return;
    };

    let newMachingHistorys = new Array<MatchingHistory>();

    let prevCreateAt: string = "";

    let matchingHistoryTmp: MatchingHistory;
    let sumMatchingHistoryTmp: MatchingHistory;
    for (let index = 0; index < this.matchingHistorysResult.matchingHistorys.length; index++) {

      if (StringUtil.IsNullOrEmpty(prevCreateAt)) {
        prevCreateAt = this.matchingHistorysResult.matchingHistorys[index].createAt;
        matchingHistoryTmp = new MatchingHistory();
        matchingHistoryTmp.billingAmount = 0;
        matchingHistoryTmp.billingAmountExcludingTax = 0;
        matchingHistoryTmp.taxAmount = 0;
        sumMatchingHistoryTmp = new MatchingHistory();
        sumMatchingHistoryTmp.departmentCode = "総計";
        sumMatchingHistoryTmp.billingAmount = 0;
      }
      else if (prevCreateAt != this.matchingHistorysResult.matchingHistorys[index].createAt) {
        matchingHistoryTmp.departmentCode = "※消込日時(消込単位) 計：" + prevCreateAt;

        newMachingHistorys.push(matchingHistoryTmp);
        matchingHistoryTmp = new MatchingHistory();
        matchingHistoryTmp.billingAmount = 0;
        matchingHistoryTmp.billingAmountExcludingTax = 0;
        matchingHistoryTmp.taxAmount = 0;

      }
      else {
        matchingHistoryTmp.billingAmount += this.matchingHistorysResult.matchingHistorys[index].billingAmount;
        matchingHistoryTmp.billingAmountExcludingTax += this.matchingHistorysResult.matchingHistorys[index].billingAmountExcludingTax;
        matchingHistoryTmp.taxAmount += this.matchingHistorysResult.matchingHistorys[index].taxAmount;
      }
      newMachingHistorys.push(this.matchingHistorysResult.matchingHistorys[index]);
      sumMatchingHistoryTmp.billingAmount += this.matchingHistorysResult.matchingHistorys[index].billingAmount;

    }

    matchingHistoryTmp.createAt = "※消込日時(消込単位) 計：" + prevCreateAt;
    newMachingHistorys.push(matchingHistoryTmp);
    newMachingHistorys.push(sumMatchingHistoryTmp);

    this.matchingHistorysResult.matchingHistorys = newMachingHistorys;

  }

  public CreateSearchCondition(): MatchingHistorySearch {
    var condition = new MatchingHistorySearch();

    condition.companyId = this.userInfoService.Company.id;
    condition.loginUserId = this.userInfoService.LoginUser.id;

    let sorting: number = 0;
    let sortOrder: string = "";

    if(this.simplePageSearch){
    }
    else{

    }


    if (this.rdoOrderCtrl.value == "0") {
      sorting = 0;
      sortOrder = "消込日時順";
    }
    else if (this.rdoOrderCtrl.value == "1") {
      sorting = 1;
      sortOrder = "得意先コード順";
    }
    else if (this.rdoOrderCtrl.value == "2") {
      sorting = 2;
      sortOrder = "入金ID順";
    }

    condition.outputOrder = sorting;

    if (!StringUtil.IsNullOrEmpty(this.createdAtFromCtrl.value)) {
      condition.inputAtFrom = DateUtil.ConvertFromDatepicker(this.createdAtFromCtrl);
    }
    else {
      condition.inputAtFrom = null;
    }

    if (!StringUtil.IsNullOrEmpty(this.createdAtToCtrl.value)) {
      condition.inputAtTo = DateUtil.ConvertFromDatepicker(this.createdAtToCtrl);
    }
    else {
      condition.inputAtTo = null;
    }

    if (!StringUtil.IsNullOrEmpty(this.recordedAtFromCtrl.value)) {
      condition.recordedAtFrom = DateUtil.ConvertFromDatepicker(this.recordedAtFromCtrl);
    }
    else {
      condition.recordedAtFrom = null;
    }

    if (!StringUtil.IsNullOrEmpty(this.recordedAtToCtrl.value)) {
      condition.recordedAtTo = DateUtil.ConvertFromDatepicker(this.recordedAtToCtrl);
    }
    else {
      condition.recordedAtTo = null;
    }

    if (!StringUtil.IsNullOrEmpty(this.receiptIdFromCtrl.value)) {
      condition.receiptIdFrom = this.receiptIdFromCtrl.value;
    }
    if (!StringUtil.IsNullOrEmpty(this.receiptIdToCtrl.value)) {
      condition.receiptIdTo = this.receiptIdToCtrl.value;
    }
    else {
    }

    if (!StringUtil.IsNullOrEmpty(this.bankCodeCtrl.value)) {
      condition.bankCode = this.bankCodeCtrl.value;
    }
    else {
      condition.bankCode = "";
    }

    if (!StringUtil.IsNullOrEmpty(this.branchCodeCtrl.value)) {
      condition.branchCode = this.branchCodeCtrl.value;
    }
    else {
      condition.branchCode = "";
    }

    if (parseInt(this.accountTypeIdCtrl.value.id) > 0) {
      condition.accountType = parseInt(this.accountTypeIdCtrl.value.id);
    }
    else {
      condition.accountType = null;
    }

    if (!StringUtil.IsNullOrEmpty(this.accountNumberCtrl.value)) {
      condition.accountNumber = this.accountNumberCtrl.value;
    }
    else {
      condition.accountNumber = "";
    }

    if (this.userInfoService.ApplicationControl.useForeignCurrency) {
      if (!StringUtil.IsNullOrEmpty(this.currencyCodeCtrl.value)) {
        condition.currencyCode = this.currencyCodeCtrl.value;
      }
      else {
      }
    }
    else {
      condition.currencyCode = "";
    }

    if (!StringUtil.IsNullOrEmpty(this.clearedAtFromCtrl.value)) {
      condition.createAtFrom = DateUtil.ConvertFromDatepicker(this.clearedAtFromCtrl) +
        'T' + this.clearedAtHourFromCtrl.value + ":00:00";
    }
    else {
      condition.createAtFrom = null;
    }

    if (!StringUtil.IsNullOrEmpty(this.clearedAtToCtrl.value)) {
      condition.createAtTo = DateUtil.ConvertFromDatepicker(this.clearedAtToCtrl) +
        'T' + this.clearedAtHourToCtrl.value + ":59:59.999";
    }
    else {
      condition.createAtTo = null;
    }

    if (!StringUtil.IsNullOrEmpty(this.customerCodeFromCtrl.value)) {
      condition.customerCodeFrom = this.customerCodeFromCtrl.value;
    }
    else {
      condition.customerCodeFrom = "";
    }

    if (!StringUtil.IsNullOrEmpty(this.customerCodeToCtrl.value)) {
      condition.customerCodeTo = this.customerCodeToCtrl.value;
    }
    else {
      condition.customerCodeTo = "";
    }

    if (!StringUtil.IsNullOrEmpty(this.departmentCodeFromCtrl.value)) {
      condition.departmentCodeFrom = this.departmentCodeFromCtrl.value;
    }
    else {
      condition.departmentCodeFrom = "";
    }
    if (!StringUtil.IsNullOrEmpty(this.departmentCodeToCtrl.value)) {
      condition.departmentCodeTo = "";
      condition.departmentCodeTo = this.departmentCodeToCtrl.value;
    }
    else {
      condition.departmentCodeTo = "";
    }

    if (this.userInfoService.ApplicationControl.useReceiptSection) {
      condition.useSectionMaster = true;
      if (!StringUtil.IsNullOrEmpty(this.sectionCodeFromCtrl.value)) {
        condition.sectionCodeFrom = this.sectionCodeFromCtrl.value;
      }
      else {
        condition.sectionCodeFrom = "";
      }
      if (!StringUtil.IsNullOrEmpty(this.sectionCodeToCtrl.value)) {
        condition.sectionCodeTo = this.sectionCodeToCtrl.value;
      }
      else {
        condition.sectionCodeTo = "";
      }

      if (this.cbxUseReceiptSectionCtrl.value) {
        condition.useSectionMaster = true;
      }
      else {
        condition.useSectionMaster = false;
      }
    }
    else {
      condition.useSectionMaster = false;
      condition.sectionCodeFrom = null;
      condition.sectionCodeTo = null;
    }

    if (!StringUtil.IsNullOrEmpty(this.loginUserCodeFromCtrl.value)) {
      condition.loginUserFrom = this.loginUserCodeFromCtrl.value;
    }
    else {
      condition.loginUserFrom = "";
    }

    if (!StringUtil.IsNullOrEmpty(this.loginUserCodeToCtrl.value)) {
      condition.loginUserTo = this.loginUserCodeToCtrl.value;
    }
    else {
      condition.loginUserTo = "";
    }

    condition.existsMemo = this.cbxMemoCtrl.value ? true : false;

    if (this.cbxMemoCtrl.value) {
      condition.existsMemo = true;
    }
    else {
      condition.existsMemo = false;
    }

    if (!StringUtil.IsNullOrEmpty(this.receiptMemoCtrl.value)) {
      condition.memo = this.receiptMemoCtrl.value;
    }
    else {
      condition.memo = "";
    }


    if (!StringUtil.IsNullOrEmpty(this.billingAmountFromCtrl.value)) {
      condition.billingAmountFrom = this.billingAmountFromCtrl.value;
    }
    if (!StringUtil.IsNullOrEmpty(this.billingAmountToCtrl.value)) {
      condition.billingAmountTo = this.billingAmountToCtrl.value;
    }

    if (!StringUtil.IsNullOrEmpty(this.receiptAmountFromCtrl.value)) {
      condition.receiptAmountFrom = this.receiptAmountFromCtrl.value;
    }
    if (!StringUtil.IsNullOrEmpty(this.receiptAmountToCtrl.value)) {
      condition.receiptAmountTo = this.receiptAmountToCtrl.value;
    }

    condition.billingCategoryId = this.billingCategoryIdCtrl.value;

    if (!StringUtil.IsNullOrEmpty(this.payerNameCtrl.value)) {
      condition.payerName = EbDataHelper.convertToValidEbkana(this.payerNameCtrl.value);
    }

    if (this.cbxOputputAtCtrl.value) {
      condition.onlyNonOutput = true;
    }
    else {
      condition.onlyNonOutput = false;
    }

    if (this.cbxSequentialAssignmentFlagCtrl.value && this.cbxIndividualAssignmentFlagCtrl.value) {
      condition.matchingProcessType = null;
    }
    else if (this.cbxSequentialAssignmentFlagCtrl.value) {
      condition.matchingProcessType = MATCHING_PROCESS_TYPE_DICTIONARY[0].id;
    }
    else if (this.cbxIndividualAssignmentFlagCtrl.value) {
      condition.matchingProcessType = MATCHING_PROCESS_TYPE_DICTIONARY[1].id;
    }

    return condition;
  }

  public selectLine(lineNo: number) {
    //this.router.navigate(['main/PC0201']);
  }

  public back() {
    this.router.navigate(['main/PE0101', { "process": "back" }]);
  }


  ///////////////////////////////////////////////////////////////////////
  public setCreatedAtFrom(eventType: string) {
    HtmlUtil.nextFocusByName(this.elementRef, 'createdAtToCtrl', eventType);
  }

  public setCreatedAtTo(eventType: string) {
    HtmlUtil.nextFocusByName(this.elementRef, 'recordedAtFromCtrl', eventType);
  }

  ///////////////////////////////////////////////////////////////////////
  public setRecordedAtFrom(eventType: string) {
    HtmlUtil.nextFocusByName(this.elementRef, 'recordedAtToCtrl', eventType);
  }

  public setRecordedAtTo(eventType: string) {
    HtmlUtil.nextFocusByName(this.elementRef, 'receiptIdFromCtrl', eventType);
  }

  ///////////////////////////////////////////////////////////////////////
  public setReceiptIdFrom(eventType: string) {
    HtmlUtil.nextFocusByName(this.elementRef, 'receiptIdToCtrl', eventType);
  }

  public setReceiptIdTo(eventType: string) {
    HtmlUtil.nextFocusByNames(this.elementRef, ['bankCodeCtrl', 'clearedAtFromCtrl'], eventType);
  }

  ///////////////////////////////////////////////////////////////////////
  public setBankCode(eventType: string) {
    if (StringUtil.IsNullOrEmpty(this.bankCodeCtrl.value)) {
      this.bankCodeCtrl.setValue("");
    }
    else {
      this.bankCodeCtrl.setValue(StringUtil.setPaddingFrontZero(this.bankCodeCtrl.value, 4, true));
    }
    HtmlUtil.nextFocusByName(this.elementRef, 'branchCodeCtrl', eventType);
  }

  public setBranchCode(eventType: string) {
    if (StringUtil.IsNullOrEmpty(this.branchCodeCtrl.value)) {
      this.branchCodeCtrl.setValue("");
    }
    else {
      this.branchCodeCtrl.setValue(StringUtil.setPaddingFrontZero(this.branchCodeCtrl.value, 3, true));
    }
    HtmlUtil.nextFocusByName(this.elementRef, 'accountTypeIdCtrl', eventType);
  }

  public setAccountNumber(eventType: string) {

    if (StringUtil.IsNullOrEmpty(this.accountNumberCtrl.value)) {
      this.accountNumberCtrl.setValue("");
    }
    else {
      this.accountNumberCtrl.setValue(StringUtil.setPaddingFrontZero(this.accountNumberCtrl.value, 7, true));
    }
    HtmlUtil.nextFocusByNames(this.elementRef, ['currencyCodeCtrl','clearedAtFromCtrl'], eventType);
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
            HtmlUtil.nextFocusByName(this.elementRef, 'clearedAtFromCtrl', eventType);
          }
          else {
            this.currencyCodeCtrl.setValue("");
            HtmlUtil.nextFocusByName(this.elementRef, 'clearedAtFromCtrl', eventType);
          }
        });
    }
    else {
      this.currencyCodeCtrl.setValue("");
      HtmlUtil.nextFocusByName(this.elementRef, 'clearedAtFromCtrl', eventType);
    }
  }
  ///////////////////////////////////////////////////////////////////////
  public setClearedAtFrom(eventType: string) {
    HtmlUtil.nextFocusByName(this.elementRef, 'clearedAtToCtrl', eventType);
  }

  public setClearedAtTo(eventType: string) {
    HtmlUtil.nextFocusByName(this.elementRef, 'customerCodeFromCtrl', eventType);
  }


  ///////////////////////////////////////////////////////////

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
        if (this.cbxCustomerCtrl.value) {
          this.customerCodeToCtrl.setValue(this.customerCodeFromCtrl.value);
          this.customerNameToCtrl.setValue(this.customerNameFromCtrl.value);
        }
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
    HtmlUtil.nextFocusByNames(this.elementRef, ['departmentCodeFromCtrl', 'loginUserCodeFromCtrl'], eventType);
  }

  public setCbxCustomer(eventType: string) {
    let localstorageItem = new LocalStorageItem();
    localstorageItem.key = RangeSearchKey.PE0301_CUSTOMER;
    localstorageItem.value = this.cbxCustomerCtrl.value;
    this.localStorageManageService.set(localstorageItem);

    HtmlUtil.nextFocusByName(this.elementRef, "customerCodeFromCtrl", eventType);

  }

  ///////////////////////////////////////////////////////////
  public setDepartmentCodeFrom(eventType: string) {

    if(eventType!=EVENT_TYPE.BLUR){
      this.departmentCodeFromTrigger.closePanel();
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
      this.departmentCodeFromCtrl.setValue("");
      this.departmentNameFromCtrl.setValue("");
      if (this.cbxDepartmentCtrl.value) {
        this.departmentCodeToCtrl.setValue(this.departmentCodeFromCtrl.value);
        this.departmentNameToCtrl.setValue(this.departmentNameFromCtrl.value);
      }
    }
    HtmlUtil.nextFocusByName(this.elementRef, 'departmentCodeToCtrl', eventType);
  }

  public setDepartmentCodeTo(eventType: string) {

    if(eventType!=EVENT_TYPE.BLUR){
      this.departmentCodeToTrigger.closePanel();
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
      this.departmentCodeToCtrl.setValue("");
      this.departmentNameToCtrl.setValue("");
    }
    HtmlUtil.nextFocusByNames(this.elementRef, ['sectionCodeFromCtrl','loginUserCodeFromCtrl'], eventType);
  }

  public setCbxDepartment(eventType: string) {
    let localstorageItem = new LocalStorageItem();
    localstorageItem.key = RangeSearchKey.PE0301_DEPARTMENT;
    localstorageItem.value = this.cbxDepartmentCtrl.value;
    this.localStorageManageService.set(localstorageItem);

    HtmlUtil.nextFocusByName(this.elementRef, "departmentCodeFromCtrl", eventType);

  }  

  ///////////////////////////////////////////////////////////

  public setSectionCodeFrom(eventType: string) {

    if(eventType!=EVENT_TYPE.BLUR){
      this.sectionCodeFromTrigger.closePanel();
    }

    if (!StringUtil.IsNullOrEmpty(this.sectionCodeFromCtrl.value)) {

      this.loadStart();
      this.sectionService.GetItems(this.sectionCodeFromCtrl.value)
        .subscribe(response => {
          this.loadEnd();
          if (response != undefined && response.length > 0) {
            this.sectionCodeFromCtrl.setValue(response[0].code);
            this.sectionNameFromCtrl.setValue(response[0].name);
          }
          else {
            this.sectionNameFromCtrl.setValue("");
          }
          if (this.cbxSectionCtrl.value) {
            this.sectionCodeToCtrl.setValue(this.sectionCodeFromCtrl.value);
            this.sectionNameToCtrl.setValue(this.sectionNameFromCtrl.value);
          }
      });
    }
    else {
      this.sectionCodeFromCtrl.setValue("");
      this.sectionNameFromCtrl.setValue("");
      if (this.cbxSectionCtrl.value) {
        this.sectionCodeToCtrl.setValue(this.sectionCodeFromCtrl.value);
        this.sectionNameToCtrl.setValue(this.sectionNameFromCtrl.value);
      }
    }
    HtmlUtil.nextFocusByName(this.elementRef, 'sectionCodeToCtrl', eventType);
  }

  public setSectionCodeTo(eventType: string) {

    if(eventType!=EVENT_TYPE.BLUR){
      this.sectionCodeToTrigger.closePanel();
    }

    if (!StringUtil.IsNullOrEmpty(this.sectionCodeToCtrl.value)) {

      this.loadStart();
      this.sectionService.GetItems(this.sectionCodeToCtrl.value)
        .subscribe(response => {
          this.loadEnd();
          if (response != undefined && response.length > 0) {
            this.sectionCodeToCtrl.setValue(response[0].code);
            this.sectionNameToCtrl.setValue(response[0].name);
          }
          else {
            this.sectionNameToCtrl.setValue("");
          }
        });
    }
    else {
      this.sectionCodeToCtrl.setValue("");
      this.sectionNameToCtrl.setValue("");
    }
    HtmlUtil.nextFocusByName(this.elementRef, 'loginUserCodeFromCtrl', eventType);
  }

  public setCbxSection(eventType: string) {
    let localstorageItem = new LocalStorageItem();
    localstorageItem.key = RangeSearchKey.PE0301_SECTION;
    localstorageItem.value = this.cbxSectionCtrl.value;
    this.localStorageManageService.set(localstorageItem);

    HtmlUtil.nextFocusByName(this.elementRef, "sectionCodeFromCtrl", eventType);

  }    

  public setCbxUseReceiptSection(eventType: string) {
    let localstorageItem = new LocalStorageItem();
    localstorageItem.key = RangeSearchKey.PE0301_USE_RECEIPT_SECTION;
    localstorageItem.value = this.cbxUseReceiptSectionCtrl.value;
    this.localStorageManageService.set(localstorageItem);

  }    

  ///////////////////////////////////////////////////////////////////////

  public setLoginUserCodeFrom(eventType: string) {

    if(eventType!=EVENT_TYPE.BLUR){
      this.loginUserCodeFromTrigger.closePanel();
    }    

    if (!StringUtil.IsNullOrEmpty(this.loginUserCodeFromCtrl.value)) {

      this.loadStart();
      this.loginUserService.GetItems(this.loginUserCodeFromCtrl.value)
        .subscribe(response => {
          this.loadEnd();
          if (response != undefined && response.length > 0) {
            this.loginUserCodeFromCtrl.setValue(response[0].code);
            this.loginUserNameFromCtrl.setValue(response[0].name);
          }
          else {
            this.loginUserNameFromCtrl.setValue("");
          }
          if (this.cbxLoginUserCtrl.value) {
            this.loginUserCodeToCtrl.setValue(this.loginUserCodeFromCtrl.value);
            this.loginUserNameToCtrl.setValue(this.loginUserNameFromCtrl.value);
          }
      });
    }
    else {
      this.loginUserCodeFromCtrl.setValue("");
      this.loginUserNameFromCtrl.setValue("");
      if (this.cbxLoginUserCtrl.value) {
        this.loginUserCodeToCtrl.setValue(this.loginUserCodeFromCtrl.value);
        this.loginUserNameToCtrl.setValue(this.loginUserNameFromCtrl.value);
      }
    }
    HtmlUtil.nextFocusByName(this.elementRef, 'loginUserCodeToCtrl', eventType);
  }

  public setLoginUserCodeTo(eventType: string) {

    if(eventType!=EVENT_TYPE.BLUR){
      this.loginUserCodeToTrigger.closePanel();
    }    

    if (!StringUtil.IsNullOrEmpty(this.loginUserCodeToCtrl.value)) {

      this.loadStart();
      this.loginUserService.GetItems(this.loginUserCodeToCtrl.value)
        .subscribe(response => {
          this.loadEnd();
          if (response != undefined && response.length > 0) {
            this.loginUserCodeToCtrl.setValue(response[0].code);
            this.loginUserNameToCtrl.setValue(response[0].name);
          }
          else {
            this.loginUserNameToCtrl.setValue("");
          }
        });
    }
    else {
      this.loginUserCodeToCtrl.setValue("");
      this.loginUserNameToCtrl.setValue("");
    }
    HtmlUtil.nextFocusByNames(this.elementRef, ['receiptMemoCtrl', 'billingAmountFromCtrl'], eventType);
  }

  public setCbxLoginUser(eventType: string) {
    let localstorageItem = new LocalStorageItem();
    localstorageItem.key = RangeSearchKey.PE0301_LOGIN_USER;
    localstorageItem.value = this.cbxLoginUserCtrl.value;
    this.localStorageManageService.set(localstorageItem);

    HtmlUtil.nextFocusByName(this.elementRef, "loginUserCodeFromCtrl", eventType);

  }    

  ///////////////////////////////////////////////////////////////////////
  public setCbxMemo(eventType: string) {
    if (this.cbxMemoCtrl.value == true) {
      HtmlUtil.nextFocusByName(this.elementRef, 'receiptMemoCtrl', eventType);
    }
    else {
      HtmlUtil.nextFocusByName(this.elementRef, 'billingAmountFromCtrl', eventType);
    }
  }

  public setReceiptMemo(eventType: string) {
    HtmlUtil.nextFocusByName(this.elementRef, 'billingAmountFromCtrl', eventType);
  }



  ///////////////////////////////////////////////////////////////////////
  public setBillingAmountFrom(eventType: string) {

    HtmlUtil.nextFocusByName(this.elementRef, 'billingAmountToCtrl', eventType);
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

  public setCurrencyForBillingAmountFrom() {
    this.onLeaveCurrencyControl(this.billingAmountFromCtrl);
  }

  public initCurrencyForBillingAmountFrom() {
    this.onFocusCurrencyControl(this.billingAmountFromCtrl);
  }

  public setBillingAmountTo(eventType: string) {

    HtmlUtil.nextFocusByName(this.elementRef, 'receiptAmountFromCtrl', eventType);
  }

  public setCurrencyForBillingAmountTo() {
    this.onLeaveCurrencyControl(this.billingAmountToCtrl);
  }

  public initCurrencyForBillingAmountTo() {
    this.onFocusCurrencyControl(this.billingAmountToCtrl);
  }
  ///////////////////////////////////////////////////////////////////////

  public setReceiptAmountFrom(eventType: string) {
    HtmlUtil.nextFocusByName(this.elementRef, 'receiptAmountToCtrl', eventType);
  }

  public setCurrencyForReceiptAmountFrom() {
    this.onLeaveCurrencyControl(this.receiptAmountFromCtrl);
  }

  public initCurrencyForReceiptAmountFrom() {
    this.onFocusCurrencyControl(this.receiptAmountFromCtrl);
  }

  public setReceiptAmountTo(eventType: string) {

    HtmlUtil.nextFocusByName(this.elementRef, 'payerNameCtrl', eventType);
  }

  public setCurrencyForReceiptAmountTo() {
    this.onLeaveCurrencyControl(this.receiptAmountToCtrl);
  }

  public initCurrencyForReceiptAmountTo() {
    this.onFocusCurrencyControl(this.receiptAmountToCtrl);
  }

  ///////////////////////////////////////////////////////////////////////
  public setPayerName(eventType: string) {

    HtmlUtil.nextFocusByName(this.elementRef, 'billingCategoryCodeCtrl', eventType);
  }

  ///////////////////////////////////////////////////////////

  public setBillingCategoryCode(eventType: string) {

    if(eventType!=EVENT_TYPE.BLUR){
      this.billingCategoryCodeTrigger.closePanel();
    }  

    let code: string = this.billingCategoryCodeCtrl.value;
    if (!StringUtil.IsNullOrEmpty(code)) {
      code = StringUtil.setPaddingFrontZero(code, 2);
      this.loadStart();
      this.categoryService.GetItems(CategoryType.Billing, code)
        .subscribe(response => {
          this.loadEnd();
          if (response != undefined) {
            let category = response[0] as Category;

            if (category !== undefined) {
              this.billingCategoryIdCtrl  .setValue(category.id);
              this.billingCategoryCodeCtrl.setValue(category.code);
              this.billingCategoryNameCtrl.setValue(category.name);
            }
            else {
              this.billingCategoryIdCtrl  .setValue('');
              // this.billingCategoryCodeCtrl.setValue('');
              this.billingCategoryNameCtrl.setValue('');
            }
          }
        });
    }
    else {
      this.billingCategoryIdCtrl  .setValue('');
      this.billingCategoryCodeCtrl.setValue('');
      this.billingCategoryNameCtrl.setValue('');
    }

  }

  ///////////////////////////////////////////////////////////////////////

  public onOrderItemChanged() {
    if (this.rdoOrderCtrl.value === '0') {
      this.rdoTakeTotalCtrl.enable();
      this.rdoTakeTotalCtrl.setValue("0");
    }
    else {
      this.rdoTakeTotalCtrl.disable();
      this.rdoTakeTotalCtrl.setValue("1");
    }
  }


  public getReceiptCategoryInfo(history: MatchingHistory) {
    return StringUtil.IsNullOrEmpty(history.receiptCategoryCode) || StringUtil.IsNullOrEmpty(history.receiptCategoryName)
      ? ''
      : `${history.receiptCategoryCode}：${history.receiptCategoryName}`;
  }

  public getBillingCategoryInfo(history: MatchingHistory) {
    return StringUtil.IsNullOrEmpty(history.billingCategoryCode) || StringUtil.IsNullOrEmpty(history.billingCategoryName)
      ? ''
      : `${history.billingCategoryCode}：${history.billingCategoryName}`;
  }

  public getCollectCategoryInfo(history: MatchingHistory) {
    return StringUtil.IsNullOrEmpty(history.collectCategoryCode) || StringUtil.IsNullOrEmpty(history.collectCategoryName)
      ? ''
      : `${history.collectCategoryCode}：${history.collectCategoryName}`;
  }

  public getAdvancedReceivedOccured(history: MatchingHistory) {
    return history.advanceReceivedOccured ? '〇' : '';
  }

  public aggreagteHistory(items: MatchingHistory[]): MatchingHistory[] {
    if (items == undefined ||
        items.length === 0) {
      return items;
    }
    const requireSubtotal = this.rdoTakeTotalCtrl.value === '0';
    const result: MatchingHistory[] = [];
    let headerId = items[0].matchingHeaderId;
    let itemBuf: MatchingHistory = undefined;

    let grandTotal      = 0;

    for (let item of items) {
      if (requireSubtotal) {
        if (headerId !== item.matchingHeaderId) {
          result.push(itemBuf);
          itemBuf = undefined;
        }
        if (itemBuf === undefined) {
          itemBuf = this.getSubtotal(item);
        }
        else {
          itemBuf.billingAmount             += (item.billingAmount              || 0);
          itemBuf.billingAmountExcludingTax += (item.billingAmountExcludingTax  || 0);
          itemBuf.taxAmount                 += (item.taxAmount                  || 0);
          itemBuf.matchingAmount            += (item.matchingAmount             || 0);
          itemBuf.billingRemain             += (item.billingRemain              || 0);
          itemBuf.receiptAmount             += (item.receiptAmount              || 0);
          itemBuf.receiptRemain             += (item.receiptRemain              || 0);
        }
      }
      grandTotal += (item.matchingAmount || 0);
      result.push(item);
      headerId = item.matchingHeaderId;
    }
    result.push(this.getGranttoal(grandTotal));
    return result;
  }
  public getSubtotal(item: MatchingHistory): MatchingHistory {
    return {
      createAt:                   item.createAt,
      billingAmount:              item.billingAmount              || 0,
      billingAmountExcludingTax:  item.billingAmountExcludingTax  || 0,
      taxAmount:                  item.taxAmount                  || 0,
      matchingAmount:             item.matchingAmount             || 0,
      billingRemain:              item.billingRemain              || 0,
      receiptAmount:              item.receiptAmount              || 0,
      receiptRemain:              item.receiptRemain              || 0,
      matchingHeaderId:           item.matchingHeaderId,
      matchingProcessType:        item.matchingProcessType,
      loginUserName:              item.loginUserName,
    } as MatchingHistory;
  }
  public getGranttoal(total: number): MatchingHistory {
    return {
      matchingAmount:     total,
    } as MatchingHistory;
  }

  public isDataRecord(item: MatchingHistory): boolean {
    return item.billingId !== undefined || item.receiptId !== undefined;
  }
  public isSubtotalRecord(item: MatchingHistory): boolean {
    return item.matchingHeaderId !== undefined && item.billingId === undefined && item.receiptId === undefined;
  }
  public isGrandtotalRecord(item: MatchingHistory): boolean {
    return item.matchingProcessType == undefined;
  }

  public get displayVirtualAccount(): boolean {
    return this.rdoDispalyVirtualAccountCtrl.value == 0;
  }
  public get displayBillingExclude(): boolean {
    return this.rdoTaxExcludedCtrl.value == 0;
  }
  public get useReceiptSection(): boolean {
    return this.userInfoService.ApplicationControl.useReceiptSection === 1;
  }

  public getSubtotalCaptionClass(): string {
    let width = this.useReceiptSection ? 1610 : 1340;
    return this.getColumnWidthClass(width);
  }
  public getSubtotalCaptionColSpan(): number {
    return this.useReceiptSection ? 12 : 10;
  }
  public getColumnWidthClass(width: number): string {
    return `col-size--${width} col-align-left`;
  }

  public getSubtotalReceiptClass(): string {
    let width = this.displayVirtualAccount ? 1520 : 1280;
    return this.getColumnWidthClass(width);
  }
  public getSubtotalReceiptColSpan(): number {
    return this.displayVirtualAccount ? 12 : 10;
  }


  public getGrandtotalCaptionClass(): string {
    let width = 1540;
    if (this.useReceiptSection) {
      width += 270;
    }
    if (this.displayBillingExclude) {
      width += 400;
    }
    return this.getColumnWidthClass(width);
  }

  public getGrandtotalCaptionColSpan(): number {
    let span = 11;
    if (this.useReceiptSection) {
      span += 2;
    }
    if (this.displayBillingExclude) {
      span += 2;
    }
    return span;
  }

  public getGrandtotalRightSideClass(): string {
    let width = this.displayVirtualAccount ? 3660: 3420;
    return this.getColumnWidthClass(width);
  }

  public getGrantotalRightSideColSpan(): number {
    return this.displayVirtualAccount ? 26 : 24;
  }

  public setSearchType(event:any){
    this.simplePageSearch=!this.simplePageSearch;
    event.stopPropagation();
  }
}
