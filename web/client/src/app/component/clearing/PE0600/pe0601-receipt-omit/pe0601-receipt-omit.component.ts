import { Component, OnInit, ElementRef, ComponentFactoryResolver, ViewContainerRef, AfterViewInit, ViewChild } from '@angular/core';
import { TABLE_INDEX } from 'src/app/common/const/table-name.const';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Router, NavigationEnd, ActivatedRoute } from '@angular/router';
import { ReceiptService } from 'src/app/service/receipt.service';
import { BaseComponent } from 'src/app/component/common/base/base-component';
import { ModalMasterComponent } from 'src/app/component/modal/modal-master/modal-master.component';
import { HtmlUtil } from 'src/app/common/util/html.util';
import { KEY_CODE, EVENT_TYPE, BUTTON_ACTION } from 'src/app/common/const/event.const';
import { StringUtil } from 'src/app/common/util/string-util';
import { CategoryMasterService } from 'src/app/service/Master/category-master.service';
import { ModalMasterBankAccountComponent } from 'src/app/component/modal/modal-master-bank-account/modal-master-bank-account.component';
import { ACCOUNT_TYPE_DICTIONARY, RECEIPT_INPUT_TYPE_DICTIONARY, RECEIPT_EXCLUDE_FLAG_DICTIONARY, CategoryType, MATCHING_ASSIGNMENT_FLAG_DICTIONARY, GridId, CODE_TYPE, RECEIPT_AMOUNT_RANGE_DICTIONARY, FunctionType } from 'src/app/common/const/kbn.const';
import { ModalMasterBankComponent } from 'src/app/component/modal/modal-master-bank/modal-master-bank.component';
import { ReceiptsResult } from 'src/app/model/receipts-result.model';
import { ReceiptSearch } from 'src/app/model/receipt-search.model';
import { FormatterUtil, FormatStyle } from 'src/app/common/util/formatter.util';
import { MODAL_STATUS_TYPE, PROCESS_RESULT_RESULT_TYPE } from 'src/app/common/const/status.const';
import { UserInfoService } from 'src/app/service/common/user-info.service';
import { NgbDateParserFormatter } from '@ng-bootstrap/ng-bootstrap';
import { NgbDateCustomParserFormatter } from 'src/app/common/util/dateformat-util';
import { SectionMasterService } from 'src/app/service/Master/section-master.service';
import { CurrencyMasterService } from 'src/app/service/Master/currency-master.service';
import { CustomerMasterService } from 'src/app/service/Master/customer-master.service';
import { LoginUserMasterService } from 'src/app/service/Master/login-user-master.service';
import { JuridicalPersonalitysResult } from 'src/app/model/juridical-personalitys-result.model';
import { JuridicalPersonalityMasterService } from 'src/app/service/Master/juridical-personality-master.service';
import { forkJoin } from 'rxjs';
import { FileUtil } from 'src/app/common/util/file.util';
import { DateUtil } from 'src/app/common/util/date-util';
import { FILE_EXTENSION } from 'src/app/common/const/eb-file.const';
import { ModalRouterProgressComponent } from 'src/app/component/modal/modal-router-progress/modal-router-progress.component';
import { Transaction } from 'src/app/model/transaction.model';
import { ModalConfirmComponent } from 'src/app/component/modal/modal-confirm/modal-confirm.component';
import { OmitSource } from 'src/app/model/omit-source.model';
import { CountResult } from 'src/app/model/count-result.model';
import { LINE_FEED_CODE } from 'src/app/common/const/report-setting-kbn.const';
import { EbDataHelper } from 'src/app/common/util/eb-data-helper';
import { ProcessResultService } from 'src/app/service/common/process-result.service';
import { MSG_WNG, MSG_ERR, MSG_ITEM_NUM, MSG_INF } from 'src/app/common/const/message.const';
import { PageUtil } from 'src/app/common/util/page-util';
import { GridSettingMasterService } from 'src/app/service/Master/grid-setting-master.service';
import { GridSettingsResult } from 'src/app/model/grid-settings-result.model';
import { GridSetting } from 'src/app/model/grid-setting.model';
import { Category } from 'src/app/model/category.model';
import { RacCurrencyPipe } from 'src/app/pipe/currency.pipe';
import { LocalStorageItem } from 'src/app/model/custom-model/local-storage-item';
import { RangeSearchKey } from 'src/app/common/const/local-storage-key-const';
import { LocalStorageManageService } from 'src/app/service/common/local-storage-manage.service';
import { GridSettingHelper } from 'src/app/model/grid-setting-helper.model';
import { ReceiptHelper } from 'src/app/model/helper/receipt-helper.model';
import { MatAutocompleteTrigger, MatExpansionPanel } from '@angular/material';

@Component({
  selector: 'app-pe0601-receipt-omit',
  templateUrl: './pe0601-receipt-omit.component.html',
  styleUrls: ['./pe0601-receipt-omit.component.css'],
  providers: [
    { provide: NgbDateParserFormatter, useClass: NgbDateCustomParserFormatter }
  ]
})
export class Pe0601ReceiptOmitComponent extends BaseComponent implements OnInit, AfterViewInit {

  public MATCHING_ASSIGNMENT_FLAG_DICTIONARY: typeof MATCHING_ASSIGNMENT_FLAG_DICTIONARY = MATCHING_ASSIGNMENT_FLAG_DICTIONARY;
  public RECEIPT_INPUT_TYPE_DICTIONARY: typeof RECEIPT_INPUT_TYPE_DICTIONARY = RECEIPT_INPUT_TYPE_DICTIONARY;

  public ACCOUNT_TYPE_DICTIONARY: typeof ACCOUNT_TYPE_DICTIONARY = ACCOUNT_TYPE_DICTIONARY;
  public RECEIPT_EXCLUDE_FLAG_DICTIONARY: typeof RECEIPT_EXCLUDE_FLAG_DICTIONARY = RECEIPT_EXCLUDE_FLAG_DICTIONARY;

  public readonly receiptAmountRangeDictionary = RECEIPT_AMOUNT_RANGE_DICTIONARY;

  public gridSettingsResult: GridSettingsResult;
  public juridicalPersonalitysResult: JuridicalPersonalitysResult;
  public excludeCategories: Category[];

  public receiptsResult: ReceiptsResult;
  public receiptSearch: ReceiptSearch;

  public dispSumCount: number;
  public dispSumBillingAmount: number;
  public dispSumRemainAmount: number;

  public selectedDelete: boolean;

  public deleteOrRecoveryButton: string;

  /** 最終更新者（ログインユーザー）ID */
  public updateById: number | null = undefined;

  public cbxCheckAllCtrl:FormControl; // 全選択・全解除

  public recordedAtFromCtrl: FormControl;  // 入金日
  public recordedAtToCtrl: FormControl;

  public payerNameCtrl: FormControl; // 振込依頼人名

  public updateAtFromCtrl: FormControl;  // 最終更新日
  public updateAtToCtrl: FormControl;

  public updateByCodeCtrl: FormControl; // 最終更新者
  public updateNameCtrl: FormControl;

  public bankCodeCtrl: FormControl; // 銀行
  public branchCodeCtrl: FormControl;
  public accountTypeIdCtrl: FormControl;
  public accountNumberCtrl: FormControl;

  public privateBankCodeCtrl: FormControl; // 専用銀行
  public privateBranchCodeCtrl: FormControl;
  public privateAccountNumberCtrl: FormControl;

  public billNumberCtrl: FormControl;  // 手形券面情報
  public billBankCodeCtrl: FormControl;
  public billBranchCodeCtrl: FormControl;
  public billDrawAtFromCtrl: FormControl;
  public billDrawAtToCtrl: FormControl;
  public billDrawerCtrl: FormControl;

  public customerCodeFromCtrl: FormControl;  // 得意先コード
  public customerNameFromCtrl: FormControl;
  public customerCodeToCtrl: FormControl;
  public customerNameToCtrl: FormControl;
  public cbxCustomerCtrl: FormControl;

  public sectionCodeFromCtrl: FormControl; // 入金部門子コード
  public sectionNameFromCtrl: FormControl;
  public sectionCodeToCtrl: FormControl;
  public sectionNameToCtrl: FormControl;
  public cbxSectionCtrl: FormControl;

  public cbxMemoCtrl: FormControl; // メモ
  public receiptMemoCtrl: FormControl;

  public cbxUseReceiptSectionCtrl: FormControl;  // 入金部門対応マスターを使用

  public inputTypeCtrl: FormControl;  // 入力区分

  public receiptCategoryCodeFromCtrl: FormControl; // 入金区分
  public receiptCategoryNameFromCtrl: FormControl;
  public receiptCategoryCodeToCtrl: FormControl;
  public receiptCategoryNameToCtrl: FormControl;
  public cbxReceiptCategoryCtrl: FormControl;

  public cbxFullAssignmentFlagCtrl: FormControl; // 消込区分
  public cbxPartAssignmentFlagCtrl: FormControl;
  public cbxNoAssignmentFlagCtrl: FormControl;

  public excludeFlagCtrl: FormControl; // 対象外状態

  public excludeCategoryIdCtrl: FormControl; // 対象外区分

  public amountRangeCtrl: FormControl; // 金額範囲
  public receiptAmountFromCtrl: FormControl;
  public receiptAmountToCtrl: FormControl;
  public cbxReceiptAmountCtrl: FormControl;

  public sourceBankNameCtrl: FormControl;// 仕向
  public sourceBranchNameCtrl: FormControl;

  public note1Ctrl: FormControl;  // 備考1
  public note2Ctrl: FormControl;  // 備考2
  public note3Ctrl: FormControl;  // 備考3
  public note4Ctrl: FormControl;  // 備考4

  public currencyCodeCtrl: FormControl;  // 通貨コード
  public currencyId: number;

  public cbxDeletedFlagCtrl: FormControl; // 削除
  public deletedAtFromCtrl: FormControl;
  public deletedAtToCtrl: FormControl;

  public cbxDetailDelFlagCtrls: FormControl[];

  public undefineCtrl: FormControl; // 未定用

  public panelOpenState: boolean;
  public AllGridSettings: GridSetting[];
  public gridSettingHelper = new GridSettingHelper();

  public simplePageSearch: boolean = true;


  @ViewChild('updateByCodeInput', { read: MatAutocompleteTrigger }) updateByCodeTrigger: MatAutocompleteTrigger;
  @ViewChild('customerCodeFromInput', { read: MatAutocompleteTrigger }) customerCodeFromTrigger: MatAutocompleteTrigger;
  @ViewChild('customerCodeToInput', { read: MatAutocompleteTrigger }) customerCodeToTrigger: MatAutocompleteTrigger;
  @ViewChild('sectionCodeFromInput', { read: MatAutocompleteTrigger }) sectionCodeFromTrigger: MatAutocompleteTrigger;
  @ViewChild('sectionCodeToInput', { read: MatAutocompleteTrigger }) sectionCodeToTrigger: MatAutocompleteTrigger;
  @ViewChild('receiptCategoryCodeFromInput', { read: MatAutocompleteTrigger }) receiptCategoryCodeFromTrigger: MatAutocompleteTrigger;
  @ViewChild('receiptCategoryCodeToInput', { read: MatAutocompleteTrigger }) receiptCategoryCodeToTrigger: MatAutocompleteTrigger;

  @ViewChild('panel') panel:MatExpansionPanel;

  constructor(
    public router: Router,
    public activatedRoute: ActivatedRoute,
    public elementRef: ElementRef,
    public componentFactoryResolver: ComponentFactoryResolver,
    public viewContainerRef: ViewContainerRef,
    public userInfoService: UserInfoService,
    public juridicalPersonalityService: JuridicalPersonalityMasterService,
    public receiptSearvice: ReceiptService,
    public categoryService: CategoryMasterService,
    public currencyService: CurrencyMasterService,
    public sectionService: SectionMasterService,
    public customerService: CustomerMasterService,
    public loginUserService: LoginUserMasterService,
    public processResultService: ProcessResultService,
    public gridSettingService: GridSettingMasterService,
    public localStorageManageService: LocalStorageManageService,
    public receiptHelper: ReceiptHelper,
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

    this.securityHideShow = this.userInfoService.isFunctionAvailable(FunctionType.ModifyReceipt);

    let gridSettingResponse = this.gridSettingService.GetItems(GridId.ReceiptSearch);
    let juridicalPersonalityResponse = this.juridicalPersonalityService.GetItems();
    this.excludeCategories = [];
    const excludeCategoryResponse = this.categoryService.GetItemsByCategoryType(CategoryType.Exclude);

    forkJoin(
      gridSettingResponse,
      juridicalPersonalityResponse,
      excludeCategoryResponse
    )
      .subscribe(responseList => {

        if (responseList != undefined && responseList.length == 3) {
          this.gridSettingsResult = new GridSettingsResult();
          this.gridSettingsResult.gridSettings = responseList[0];
          this.gridSettingsResult.gridSettings = this.gridSettingsResult.gridSettings.filter(gridSetting => {
            return gridSetting.displayWidth != 0
              && gridSetting.columnName != "ExcludeFlag"
              && gridSetting.columnName != "ExcludeCategory"
          });
          // 検索条件用絞込無
          this.AllGridSettings = responseList[0];
          this.juridicalPersonalitysResult = new JuridicalPersonalitysResult();
          this.juridicalPersonalitysResult.juridicalPersonalities = responseList[1];

          this.excludeCategories = responseList[2];
        }
        else {
        }
      });

  }

  ngAfterViewInit() {
    HtmlUtil.nextFocusByName(this.elementRef, 'recordedAtFromCtrl', EVENT_TYPE.NONE);

  }

  public setControlInit() {

    this.cbxCheckAllCtrl = new FormControl(null);  // 全選択・全解除

    this.recordedAtFromCtrl = new FormControl("", [Validators.maxLength(10)]);  // 入金日
    this.recordedAtToCtrl = new FormControl("", [Validators.maxLength(10)]);

    this.payerNameCtrl = new FormControl("", [Validators.maxLength(140)]); // 振込依頼人名

    this.updateAtFromCtrl = new FormControl("", [Validators.maxLength(10)]); // 最終更新日
    this.updateAtToCtrl = new FormControl("", [Validators.maxLength(10)]);

    this.updateByCodeCtrl = new FormControl("", [Validators.maxLength(10)]); // 最終更新者
    this.updateNameCtrl = new FormControl("");

    if (this.userInfoService.ApplicationControl.useForeignCurrency === 1) {
      this.currencyCodeCtrl = new FormControl("", [Validators.required, Validators.maxLength(3)]);  // 通貨コード
    }
    else {
      this.currencyCodeCtrl = new FormControl("", [Validators.maxLength(3)]);  // 通貨コード
    }

    this.bankCodeCtrl = new FormControl("", [Validators.maxLength(4)]); // 銀行
    this.branchCodeCtrl = new FormControl("", [Validators.maxLength(3)]);
    this.accountTypeIdCtrl = new FormControl("");
    this.accountNumberCtrl = new FormControl("", [Validators.maxLength(7)]);

    this.privateBankCodeCtrl = new FormControl("", [Validators.maxLength(4)]); // 専用銀行
    this.privateBranchCodeCtrl = new FormControl("", [Validators.maxLength(3)]);
    this.privateAccountNumberCtrl = new FormControl("", [Validators.maxLength(7)]);

    this.billNumberCtrl = new FormControl("", [Validators.maxLength(20)]); // 手形券面情報
    this.billBankCodeCtrl = new FormControl("", [Validators.maxLength(4)]);
    this.billBranchCodeCtrl = new FormControl("", [Validators.maxLength(3)]);
    this.billDrawAtFromCtrl = new FormControl("", [Validators.maxLength(10)]);
    this.billDrawAtToCtrl = new FormControl("", [Validators.maxLength(10)]);
    this.billDrawerCtrl = new FormControl("", [Validators.maxLength(48)]);

    this.customerCodeFromCtrl = new FormControl("", [Validators.maxLength(this.userInfoService.ApplicationControl.customerCodeLength)]);  // 得意先コード
    this.customerNameFromCtrl = new FormControl("");
    this.customerCodeToCtrl = new FormControl("", [Validators.maxLength(this.userInfoService.ApplicationControl.customerCodeLength)]);
    this.customerNameToCtrl = new FormControl("");
    this.cbxCustomerCtrl = new FormControl("");

    this.sectionCodeFromCtrl = new FormControl("", [Validators.maxLength(this.userInfoService.ApplicationControl.sectionCodeLength)]); // 入金部門コード
    this.sectionNameFromCtrl = new FormControl("");
    this.sectionCodeToCtrl = new FormControl("", [Validators.maxLength(this.userInfoService.ApplicationControl.sectionCodeLength)]);
    this.sectionNameToCtrl = new FormControl("");
    this.cbxSectionCtrl = new FormControl("");

    this.cbxMemoCtrl = new FormControl(""); // メモ
    this.receiptMemoCtrl = new FormControl("", [Validators.maxLength(100)]);

    this.cbxUseReceiptSectionCtrl = new FormControl("");  // 入金部門対応マスターを使用

    this.inputTypeCtrl = new FormControl(""); // 入力区分

    this.receiptCategoryCodeFromCtrl = new FormControl("", [Validators.maxLength(2)]); // 入金区分
    this.receiptCategoryNameFromCtrl = new FormControl("");
    this.receiptCategoryCodeToCtrl = new FormControl("", [Validators.maxLength(2)]);
    this.receiptCategoryNameToCtrl = new FormControl("");
    this.cbxReceiptCategoryCtrl = new FormControl("");

    this.cbxFullAssignmentFlagCtrl = new FormControl(""); // 消込区分
    this.cbxPartAssignmentFlagCtrl = new FormControl("");
    this.cbxNoAssignmentFlagCtrl = new FormControl("");

    this.excludeFlagCtrl = new FormControl(""); // 対象外状態

    this.excludeCategoryIdCtrl = new FormControl(""); // 対象外区分

    this.amountRangeCtrl = new FormControl(""); // 金額範囲
    this.receiptAmountFromCtrl = new FormControl("", [Validators.maxLength(15)]);
    this.receiptAmountToCtrl = new FormControl("", [Validators.maxLength(15)]);
    this.cbxReceiptAmountCtrl = new FormControl("");

    this.sourceBankNameCtrl = new FormControl("", [Validators.maxLength(140)]); // 仕向
    this.sourceBranchNameCtrl = new FormControl("", [Validators.maxLength(15)]);

    this.note1Ctrl = new FormControl("", [Validators.maxLength(100)]); // 備考
    this.note2Ctrl = new FormControl("", [Validators.maxLength(100)]);
    this.note3Ctrl = new FormControl("", [Validators.maxLength(100)]);
    this.note4Ctrl = new FormControl("", [Validators.maxLength(100)]);

    this.cbxDeletedFlagCtrl = new FormControl(""); // 削除
    this.deletedAtFromCtrl = new FormControl("", [Validators.maxLength(10)]);
    this.deletedAtToCtrl = new FormControl("", [Validators.maxLength(10)]);

    this.undefineCtrl = new FormControl(""); // 未定用;


  }

  public setValidator() {
    this.MyFormGroup = new FormGroup({

      cbxCheckAllCtrl:this.cbxCheckAllCtrl, // 全選択・全解除

      recordedAtFromCtrl: this.recordedAtFromCtrl,  // 入金日
      recordedAtToCtrl: this.recordedAtToCtrl,

      payerNameCtrl: this.payerNameCtrl, // 振込依頼人名

      updateAtFromCtrl: this.updateAtFromCtrl,  // 最終更新日
      updateAtToCtrl: this.updateAtToCtrl,

      updateByCodeCtrl: this.updateByCodeCtrl, // 最終更新者
      updateNameCtrl: this.updateNameCtrl,

      currencyCodeCtrl: this.currencyCodeCtrl,  // 通貨コード

      bankCodeCtrl: this.bankCodeCtrl, // 銀行
      branchCodeCtrl: this.branchCodeCtrl,
      accountTypeIdCtrl: this.accountTypeIdCtrl,
      accountNumberCtrl: this.accountNumberCtrl,

      privateBankCodeCtrl: this.privateBankCodeCtrl, // 専用銀行
      privateBranchCodeCtrl: this.privateBranchCodeCtrl,
      privateAccountNumberCtrl: this.privateAccountNumberCtrl,

      billNumberCtrl: this.billNumberCtrl,  // 手形券面情報
      billBankCodeCtrl: this.billBankCodeCtrl,
      billBranchCodeCtrl: this.billBranchCodeCtrl,
      billDrawAtFromCtrl: this.billDrawAtFromCtrl,
      billDrawAtToCtrl: this.billDrawAtToCtrl,
      billDrawerCtrl: this.billDrawerCtrl,

      customerCodeFromCtrl: this.customerCodeFromCtrl,  // 得意先コード
      customerNameFromCtrl: this.customerNameFromCtrl,
      customerCodeToCtrl: this.customerCodeToCtrl,
      customerNameToCtrl: this.customerNameToCtrl,
      cbxCustomerCtrl: this.cbxCustomerCtrl,

      sectionCodeFromCtrl: this.sectionCodeFromCtrl, // 入金部門子コード
      sectionNameFromCtrl: this.sectionNameFromCtrl,
      sectionCodeToCtrl: this.sectionCodeToCtrl,
      sectionNameToCtrl: this.sectionNameToCtrl,
      cbxSectionCtrl: this.cbxSectionCtrl,

      cbxMemoCtrl: this.cbxMemoCtrl, // メモ
      receiptMemoCtrl: this.receiptMemoCtrl,

      cbxUseReceiptSectionCtrl: this.cbxUseReceiptSectionCtrl,  // 入金部門対応マスターを使用

      inputTypeCtrl: this.inputTypeCtrl,  // 入力区分

      receiptCategoryCodeFromCtrl: this.receiptCategoryCodeFromCtrl, // 入金区分
      receiptCategoryNameFromCtrl: this.receiptCategoryNameFromCtrl,
      receiptCategoryCodeToCtrl: this.receiptCategoryCodeToCtrl,
      receiptCategoryNameToCtrl: this.receiptCategoryNameToCtrl,
      cbxReceiptCategoryCtrl: this.cbxReceiptCategoryCtrl,

      cbxFullAssignmentFlagCtrl: this.cbxFullAssignmentFlagCtrl, // 消込区分
      cbxPartAssignmentFlagCtrl: this.cbxPartAssignmentFlagCtrl,
      cbxNoAssignmentFlagCtrl: this.cbxNoAssignmentFlagCtrl,

      excludeFlagCtrl: this.excludeFlagCtrl, // 対象外状態

      excludeCategoryIdCtrl: this.excludeCategoryIdCtrl, // 対象外区分

      amountRangeCtrl: this.amountRangeCtrl, // 金額範囲
      receiptAmountFromCtrl: this.receiptAmountFromCtrl,
      receiptAmountToCtrl: this.receiptAmountToCtrl,
      cbxReceiptAmountCtrl: this.cbxReceiptAmountCtrl,

      sourceBankNameCtrl: this.sourceBankNameCtrl,// 仕向
      sourceBranchNameCtrl: this.sourceBranchNameCtrl,

      note1Ctrl: this.note1Ctrl,  // 備考
      note2Ctrl: this.note2Ctrl,
      note3Ctrl: this.note3Ctrl,
      note4Ctrl: this.note4Ctrl,


      cbxDeletedFlagCtrl: this.cbxDeletedFlagCtrl, // 削除
      deletedAtFromCtrl: this.deletedAtFromCtrl,
      deletedAtToCtrl: this.deletedAtToCtrl,

      undefineCtrl: this.undefineCtrl, // 未定用

    });

  }

  public setFormatter() {

    if (this.userInfoService.ApplicationControl.loginUserCodeType == CODE_TYPE.NUMBER) {
      FormatterUtil.setNumberFormatter(this.updateByCodeCtrl); // 最終更新者
    }
    else {
      FormatterUtil.setCodeFormatter(this.updateByCodeCtrl);
    }

    FormatterUtil.setAlphabetFormatter(this.currencyCodeCtrl);  // 通貨コード

    FormatterUtil.setNumberFormatter(this.bankCodeCtrl);  // 銀行
    FormatterUtil.setNumberFormatter(this.branchCodeCtrl);
    FormatterUtil.setNumberFormatter(this.accountNumberCtrl);

    FormatterUtil.setNumberFormatter(this.privateBankCodeCtrl);  // 専用銀行
    FormatterUtil.setNumberFormatter(this.privateBranchCodeCtrl);
    FormatterUtil.setNumberFormatter(this.privateAccountNumberCtrl);

    FormatterUtil.setNumberFormatter(this.billBankCodeCtrl);  // 手形券面情報
    FormatterUtil.setNumberFormatter(this.billBranchCodeCtrl);

    if (this.userInfoService.ApplicationControl.customerCodeType == CODE_TYPE.NUMBER) {
      FormatterUtil.setNumberFormatter(this.customerCodeFromCtrl); // 得意先コード
      FormatterUtil.setNumberFormatter(this.customerCodeToCtrl);
    }
    else if (this.userInfoService.ApplicationControl.customerCodeType == CODE_TYPE.HANKANA) {
      FormatterUtil.setCustomerCodeKanaFormatter(this.customerCodeFromCtrl);
      FormatterUtil.setCustomerCodeKanaFormatter(this.customerCodeToCtrl);
    }
    else {
      FormatterUtil.setCustomerCodeAlphaFormatter(this.customerCodeFromCtrl);
      FormatterUtil.setCustomerCodeAlphaFormatter(this.customerCodeToCtrl);
    }

    if (this.userInfoService.ApplicationControl.sectionCodeType == CODE_TYPE.NUMBER) {
      FormatterUtil.setNumberFormatter(this.sectionCodeFromCtrl); // 入金部門子コード
      FormatterUtil.setNumberFormatter(this.sectionCodeToCtrl);
    }
    else {
      FormatterUtil.setCodeFormatter(this.sectionCodeFromCtrl);
      FormatterUtil.setCodeFormatter(this.sectionCodeToCtrl);
    }

    FormatterUtil.setNumberFormatter(this.receiptCategoryCodeFromCtrl);  // 入金区分
    FormatterUtil.setNumberFormatter(this.receiptCategoryCodeToCtrl);

    FormatterUtil.setCurrencyFormatter(this.receiptAmountFromCtrl);
    FormatterUtil.setCurrencyFormatter(this.receiptAmountToCtrl);
  }

  public clear() {
    this.MyFormGroup.reset();

    this.cbxDeletedFlagCtrl.enable();

    this.excludeCategoryIdCtrl.disable(); // 対象外区分

    this.cbxFullAssignmentFlagCtrl.disable();
    this.cbxPartAssignmentFlagCtrl.setValue("true");
    this.cbxNoAssignmentFlagCtrl.setValue("true");

    this.deletedAtFromCtrl.disable();
    this.deletedAtToCtrl.disable();

    this.currencyId = 0;
    this.updateById = undefined;

    this.accountTypeIdCtrl.setValue(ACCOUNT_TYPE_DICTIONARY[0].id);
    this.inputTypeCtrl.setValue(RECEIPT_INPUT_TYPE_DICTIONARY[0].id);
    this.excludeFlagCtrl.setValue(RECEIPT_EXCLUDE_FLAG_DICTIONARY[0].id);
    this.amountRangeCtrl.setValue(this.receiptAmountRangeDictionary[0].id);


    this.processCustomResult = this.processResultService.clearProcessCustomMsg(this.processCustomResult);

    this.selectedDelete = false;

    this.deleteOrRecoveryButton = this.ItemNameConst.BTN_DELETE;

    /*
    this.receiptsResult = null;
    this.dispSumCount = 0;
    this.dispSumBillingAmount = 0;
    this.dispSumRemainAmount = 0;
    */
   
    this.panelOpenState = true;
    this.panel.open();

    HtmlUtil.nextFocusByName(this.elementRef, 'recordedAtFromCtrl', EVENT_TYPE.NONE);


    this.setRangeCheckbox();
  }

  public setAutoComplete() {
    // 最終更新者
    this.initAutocompleteLoginUsers(this.updateByCodeCtrl, this.loginUserService, 0);

    // 入金区分
    this.initAutocompleteCategories(CategoryType.Receipt, this.receiptCategoryCodeFromCtrl, this.categoryService, 0);
    this.initAutocompleteCategories(CategoryType.Receipt, this.receiptCategoryCodeToCtrl, this.categoryService, 1);

    // 得意先
    this.initAutocompleteCustomers(this.customerCodeFromCtrl, this.customerService, 0);
    this.initAutocompleteCustomers(this.customerCodeToCtrl, this.customerService, 1);

    // 入金部門
    this.initAutocompleteSections(this.sectionCodeFromCtrl, this.sectionService, 0);
    this.initAutocompleteSections(this.sectionCodeToCtrl, this.sectionService, 1);


  }


  public closeAutoCompletePanel() {

    if (this.updateByCodeTrigger != undefined) { this.updateByCodeTrigger.closePanel(); }
    if (this.customerCodeFromTrigger != undefined) { this.customerCodeFromTrigger.closePanel(); }
    if (this.customerCodeToTrigger != undefined) { this.customerCodeToTrigger.closePanel(); }
    if (this.sectionCodeFromTrigger != undefined) { this.sectionCodeFromTrigger.closePanel(); }
    if (this.sectionCodeToTrigger != undefined) { this.sectionCodeToTrigger.closePanel(); }
    if (this.receiptCategoryCodeFromTrigger != undefined) { this.receiptCategoryCodeFromTrigger.closePanel(); }
    if (this.receiptCategoryCodeToTrigger != undefined) { this.receiptCategoryCodeToTrigger.closePanel(); }


  }

  public setRangeCheckbox() {
    let cbxCustomer = this.localStorageManageService.get(RangeSearchKey.PE0601_CUSTOMER);
    let cbxSection = this.localStorageManageService.get(RangeSearchKey.PE0601_SECTION);
    let cbxUseReceiptSection = this.localStorageManageService.get(RangeSearchKey.PE0601_USE_RECEIPT_SECTION);
    let cbxAmount = this.localStorageManageService.get(RangeSearchKey.PE0601_AMOUNT);
    let cbxReceiptCategory = this.localStorageManageService.get(RangeSearchKey.PE0601_RECEIPT_CATEGORY);

    if (cbxCustomer != null) {
      this.cbxCustomerCtrl.setValue(cbxCustomer.value);
    }

    if (cbxSection != null) {
      this.cbxSectionCtrl.setValue(cbxSection.value);
    }

    if (cbxUseReceiptSection != null) {
      this.cbxUseReceiptSectionCtrl.setValue(cbxUseReceiptSection.value);
    }

    if (cbxAmount != null) {
      this.cbxReceiptAmountCtrl.setValue(cbxAmount.value);
    }

    if (cbxReceiptCategory != null) {
      this.cbxReceiptCategoryCtrl.setValue(cbxReceiptCategory.value);
    }



  }

  public openBankMasterModal(type: string, ) {

    this.closeAutoCompletePanel();

    let componentFactory = this.componentFactoryResolver.resolveComponentFactory(ModalMasterBankComponent);
    let componentRef = this.viewContainerRef.createComponent(componentFactory);

    componentRef.instance.Closing.subscribe(() => {

      if (componentRef.instance.ModalStatus == MODAL_STATUS_TYPE.SELECT) {
        if (type === 'private') {
          this.privateBankCodeCtrl.setValue(componentRef.instance.SelectedBankCode);
          this.privateBranchCodeCtrl.setValue(componentRef.instance.SelectedBranchCode);
        }
        else if (type === 'bill') {
          this.billBankCodeCtrl.setValue(componentRef.instance.SelectedBankCode);
          this.billBranchCodeCtrl.setValue(componentRef.instance.SelectedBranchCode);
        }
      }

      componentRef.destroy();
    });
  }

  public openBankAccountMasterModal() {
    this.closeAutoCompletePanel();

    let componentFactory = this.componentFactoryResolver.resolveComponentFactory(ModalMasterBankAccountComponent);
    let componentRef = this.viewContainerRef.createComponent(componentFactory);

    componentRef.instance.Closing.subscribe(() => {

      if (componentRef.instance.ModalStatus == MODAL_STATUS_TYPE.SELECT) {
        this.bankCodeCtrl.setValue(componentRef.instance.SelectedBankCode);
        this.branchCodeCtrl.setValue(componentRef.instance.SelectedBranchCode);
        this.accountTypeIdCtrl.setValue(ACCOUNT_TYPE_DICTIONARY[componentRef.instance.SelectedAccountTypeId].id);
        this.accountNumberCtrl.setValue(componentRef.instance.SelectedAccountNumber);
      }

      componentRef.destroy();
    });

  }

  public openMasterModal(table: TABLE_INDEX, type: string) {

    this.closeAutoCompletePanel();

    let componentFactory = this.componentFactoryResolver.resolveComponentFactory(ModalMasterComponent);
    let componentRef = this.viewContainerRef.createComponent(componentFactory);

    componentRef.instance.TableIndex = table;

    componentRef.instance.Closing.subscribe(() => {

      if (componentRef.instance.ModalStatus == MODAL_STATUS_TYPE.SELECT) {
        switch (table) {
          case TABLE_INDEX.MASTER_RECEIPT_CATEGORY:
            {
              if (type === "from") {
                this.receiptCategoryCodeFromCtrl.setValue(componentRef.instance.SelectedCode);
                this.receiptCategoryNameFromCtrl.setValue(componentRef.instance.SelectedName);

                if (this.cbxReceiptCategoryCtrl.value === true) {
                  this.receiptCategoryCodeToCtrl.setValue(componentRef.instance.SelectedCode);
                  this.receiptCategoryNameToCtrl.setValue(componentRef.instance.SelectedName);
                }

              }
              else {
                this.receiptCategoryCodeToCtrl.setValue(componentRef.instance.SelectedCode);
                this.receiptCategoryNameToCtrl.setValue(componentRef.instance.SelectedName);
              }
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
          case TABLE_INDEX.MASTER_LOGIN_USER:
            {
              this.updateById = componentRef.instance.SelectedId;
              this.updateByCodeCtrl.setValue(componentRef.instance.SelectedCode);
              this.updateNameCtrl.setValue(componentRef.instance.SelectedName);
              break;
            }
          case TABLE_INDEX.MASTER_CURRENCY:
            {
              this.currencyCodeCtrl.setValue(componentRef.instance.SelectedCode);
              this.currencyId = componentRef.instance.SelectedId;
              break;
            }
        }
      }


      componentRef.destroy();
    });

  }

  public getGridSetting(columnName: string): string {

    if (this.AllGridSettings == undefined) return "";

    let columnSetting = this.AllGridSettings.find(x => x.columnName == columnName);
    return columnSetting == undefined ? "" : columnSetting.columnNameJp;
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

    this.receiptSearch = this.SearchCondition();
    this.SearchReceiptData();

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

    if (this.receiptsResult == null) return;

    for (let index = 0; index < this.receiptsResult.receipts.length; index++) {
      this.cbxDetailDelFlagCtrls[index].setValue("true");
    }
    this.selectedDelete = true;
  }

  public clearAll() {
    this.processCustomResult = this.processResultService.clearProcessCustomMsg(this.processCustomResult);

    if (this.receiptsResult == null) return;

    for (let index = 0; index < this.receiptsResult.receipts.length; index++) {
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
        tmpTran.id = this.receiptsResult.receipts[index].id;
        tmpTran.updateAt = this.receiptsResult.receipts[index].updateAt;
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

        this.receiptSearvice.Omit(omitSource)
          .subscribe(response => {
            if (response != PROCESS_RESULT_RESULT_TYPE.FAILURE) {
              let countResult: CountResult = response;

              if (countResult.processResult.result == true) {
                msg = "請求データの" + this.deleteOrRecoveryButton + "が完了しました。"
                this.processCustomResult = this.processResultService.processAtSuccess(
                  this.processCustomResult, msg, this.partsResultMessageComponent);

                this.SearchReceiptData();
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


    this.receiptSearch = this.SearchCondition();

    //this.billingSearch.isDeleted=true;

    this.receiptSearvice.GetReport(this.receiptSearch)
      .subscribe(response => {
        FileUtil.download([response.body], this.Title + DateUtil.getYYYYMMDD(0), FILE_EXTENSION.PDF);
        componentRef.destroy();
      });


  }


  public output() {
    this.processCustomResult = this.processResultService.clearProcessCustomMsg(this.processCustomResult);

    let data: string = "";
    let dataList = this.receiptsResult.receipts;


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
          case 'RecordedAt':
            dataItem.push(DateUtil.convertDateString(dataList[index].recordedAt));
            break;
          case 'CustomerCode':
            dataItem.push(dataList[index].customerCode);
            break;
          case 'CustomerName':
            dataItem.push(dataList[index].customerName);
            break;
          case 'PayerName':
            dataItem.push(dataList[index].payerName);
            break;
          case 'CurrencyCode':
            dataItem.push(dataList[index].currencyCode);
            break;
          case 'ReceiptAmount':
            dataItem.push(dataList[index].receiptAmount);
            break;
          case 'RemainAmount':
            dataItem.push(dataList[index].remainAmount);
            break;
          case 'ExcludeAmount':
            dataItem.push(dataList[index].excludeAmount);
            break;
          case 'ReceiptCategoryName':
            dataItem.push(dataList[index].categoryCode + ':' + dataList[index].categoryName);
            break;
          case 'InputType':
            dataItem.push(this.RECEIPT_INPUT_TYPE_DICTIONARY[dataList[index].inputType].val);
            break;
          case 'Note1':
            dataItem.push(dataList[index].note1);
            break;
          case 'Memo':
            dataItem.push(dataList[index].receiptMemo);
            break;
          case 'DueAt':
            dataItem.push(DateUtil.convertDateString(dataList[index].dueAt));
            break;
          case 'SectionCode':
            dataItem.push(dataList[index].sectionCode);
            break;
          case 'SectionName':
            dataItem.push(dataList[index].sectionName);
            break;
          case 'BankCode':
            dataItem.push(dataList[index].bankCode);
            break;
          case 'BankName':
            dataItem.push(dataList[index].bankName);
            break;
          case 'BranchCode':
            dataItem.push(dataList[index].branchCode);
            break;
          case 'BranchName':
            dataItem.push(dataList[index].branchName);
            break;
          case 'AccountNumber':
            dataItem.push(dataList[index].accountNumber);
            break;
          case 'SourceBankName':
            dataItem.push(dataList[index].sourceBankName);
            break;
          case 'SourceBranchName':
            dataItem.push(dataList[index].sourceBranchName);
            break;
          case 'VirtualBranchCode':
            dataItem.push(dataList[index].payerCode.substr(0, 3));
            break;
          case 'VirtualAccountNumber':
            dataItem.push(dataList[index].payerCode.substr(3, 7));
            break;
          case 'OutputAt':
            dataItem.push(DateUtil.getYYYYMMDD(5, dataList[index].outputAt));
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
          case 'BillNumber':
            dataItem.push(dataList[index].billNumber);
            break;
          case 'BillBankCode':
            dataItem.push(dataList[index].billBankCode);
            break;
          case 'BillBranchCode':
            dataItem.push(dataList[index].billBranchCode);
            break;
          case 'BillDrawAt':
            dataItem.push(DateUtil.convertDateString(dataList[index].billDrawAt));
            break;
          case 'BillDrawer':
            dataItem.push(dataList[index].billDrawer);
            break;
        }
      });
      
      dataItem = FileUtil.encloseItemBySymbol(dataItem);
      data = data + dataItem.join(",") + LINE_FEED_CODE;
    }
    let result: boolean = false;
    let resultDatas: Array<any> = [];
    resultDatas.push(data);

    FileUtil.download(resultDatas, "未消込入金データ削除" + DateUtil.getYYYYMMDD(0), FILE_EXTENSION.CSV);
    result = true;
    this.processResultService.processAtOutput(
      this.processCustomResult, result, 0, this.partsResultMessageComponent);

    modalRouterProgressComponentRef.destroy();
  }

  public RequiredChecking(): boolean {

    if (!this.cbxPartAssignmentFlagCtrl.value && !this.cbxNoAssignmentFlagCtrl.value) {
      this.processCustomResult = this.processResultService.processAtWarning(
        this.processCustomResult, MSG_WNG.SELECTION_REQUIRED.replace(MSG_ITEM_NUM.FIRST, '消込区分'),
        this.partsResultMessageComponent);
      return false;
    }

    return true;

  }

  public SearchReceiptData() {


    let componentFactory = this.componentFactoryResolver.resolveComponentFactory(ModalRouterProgressComponent);
    let modalRouterProgressComponentRef = this.viewContainerRef.createComponent(componentFactory);
    //modalRouterProgressComponentRef.destroy();    

    this.receiptSearvice.GetItems(this.receiptSearch)
      .subscribe(response => {
        if (response != undefined) {
          this.receiptsResult = new ReceiptsResult();

          this.receiptsResult.receipts = response;


          this.dispSumCount = this.receiptsResult.receipts.length;

          this.receiptsResult.receipts.forEach(element => {


            this.dispSumBillingAmount += element.receiptAmount;
            this.dispSumRemainAmount += element.remainAmount;
          });

          this.cbxDetailDelFlagCtrls = new Array<FormControl>(this.dispSumCount);

          for (let index = 0; index < this.dispSumCount; index++) {
            this.cbxDetailDelFlagCtrls[index] = new FormControl(null);
            this.MyFormGroup.removeControl("cbxDetailDelFlagCtrl" + index);
            this.MyFormGroup.addControl("cbxDetailDelFlagCtrl" + index, this.cbxDetailDelFlagCtrls[index]);
          }

          if (this.receiptsResult.receipts.length == 0) {
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

  public SearchCondition(): ReceiptSearch {
    let receiptSearch = new ReceiptSearch();

    receiptSearch.companyId = this.userInfoService.Company.id;
    receiptSearch.loginUserId = this.userInfoService.LoginUser.id;

    if (this.simplePageSearch) {

      receiptSearch.recordedAtFrom = DateUtil.ConvertFromDatepicker(this.recordedAtFromCtrl);
      receiptSearch.recordedAtTo = DateUtil.ConvertFromDatepicker(this.recordedAtToCtrl);

      if (!StringUtil.IsNullOrEmpty(this.payerNameCtrl.value)) {
        receiptSearch.payerName = this.payerNameCtrl.value;
      }

      if (!StringUtil.IsNullOrEmpty(this.bankCodeCtrl.value)) {
        receiptSearch.bankCode = this.bankCodeCtrl.value;
      }

      if (!StringUtil.IsNullOrEmpty(this.branchCodeCtrl.value)) {
        receiptSearch.branchCode = this.branchCodeCtrl.value;
      }

      if (this.accountTypeIdCtrl.value != "0") {
        receiptSearch.accountTypeId = this.accountTypeIdCtrl.value;
      }

      if (!StringUtil.IsNullOrEmpty(this.accountNumberCtrl.value)) {
        receiptSearch.accountNumber = this.accountNumberCtrl.value;
      }

      if (!StringUtil.IsNullOrEmpty(this.customerCodeFromCtrl.value)) {
        receiptSearch.customerCodeFrom = this.customerCodeFromCtrl.value;
      }

      if (!StringUtil.IsNullOrEmpty(this.customerCodeToCtrl.value)) {
        receiptSearch.customerCodeTo = this.customerCodeToCtrl.value;
      }

      if (!StringUtil.IsNullOrEmpty(this.receiptCategoryCodeFromCtrl.value)) {
        receiptSearch.receiptCategoryCodeFrom = this.receiptCategoryCodeFromCtrl.value;
      }

      if (!StringUtil.IsNullOrEmpty(this.receiptCategoryCodeToCtrl.value)) {
        receiptSearch.receiptCategoryCodeTo = this.receiptCategoryCodeToCtrl.value;
      }


      // 消込・未消込・一部消込
      // 2進数
      // 消込済み；0x004 = 4
      // 一部	；0x002 = 2
      // 未消込	；0x001 = 1
      receiptSearch.assignmentFlag = 0;
      if (this.cbxFullAssignmentFlagCtrl.value) {
        receiptSearch.assignmentFlag = receiptSearch.assignmentFlag + 4;
      }
      if (this.cbxPartAssignmentFlagCtrl.value) {
        receiptSearch.assignmentFlag = receiptSearch.assignmentFlag + 2;
      }
      if (this.cbxNoAssignmentFlagCtrl.value) {
        receiptSearch.assignmentFlag = receiptSearch.assignmentFlag + 1;
      }

      if (this.amountRangeCtrl.value.id == this.receiptAmountRangeDictionary[0].id) {
        receiptSearch.receiptAmountFrom = this.receiptAmountFromCtrl.value;
        receiptSearch.receiptAmountTo = this.receiptAmountToCtrl.value;
      }
      else {
        receiptSearch.remainAmountFrom = this.receiptAmountFromCtrl.value;
        receiptSearch.remainAmountTo = this.receiptAmountToCtrl.value;
      }

      receiptSearch.excludeFlag = this.excludeFlagCtrl.value;
      if (this.excludeCategoryIdCtrl.value != '0') {
        receiptSearch.excludeCategoryId = this.excludeCategoryIdCtrl.value;
      }

    }
    else {

      receiptSearch.recordedAtFrom = DateUtil.ConvertFromDatepicker(this.recordedAtFromCtrl);
      receiptSearch.recordedAtTo = DateUtil.ConvertFromDatepicker(this.recordedAtToCtrl);

      if (!StringUtil.IsNullOrEmpty(this.payerNameCtrl.value)) {
        receiptSearch.payerName = this.payerNameCtrl.value;
      }

      receiptSearch.updateAtFrom = DateUtil.ConvertFromDatepickerToStart(this.updateAtFromCtrl);
      receiptSearch.updateAtTo = DateUtil.ConvertFromDatepickerToEnd(this.updateAtToCtrl);

      if (this.updateById != undefined) {
        receiptSearch.updateBy = this.updateById;
      }

      if (this.userInfoService.ApplicationControl.useForeignCurrency) {
        receiptSearch.useForeignCurrencyFlg = 1;

        if (!StringUtil.IsNullOrEmpty(this.currencyCodeCtrl.value)) {
          receiptSearch.currencyId = this.currencyId;
        }
      }

      if (!StringUtil.IsNullOrEmpty(this.bankCodeCtrl.value)) {
        receiptSearch.bankCode = this.bankCodeCtrl.value;
      }

      if (!StringUtil.IsNullOrEmpty(this.branchCodeCtrl.value)) {
        receiptSearch.branchCode = this.branchCodeCtrl.value;
      }

      if (this.accountTypeIdCtrl.value != "0") {
        receiptSearch.accountTypeId = this.accountTypeIdCtrl.value;
      }

      if (!StringUtil.IsNullOrEmpty(this.accountNumberCtrl.value)) {
        receiptSearch.accountNumber = this.accountNumberCtrl.value;
      }

      if (!StringUtil.IsNullOrEmpty(this.privateBankCodeCtrl.value)) {
        receiptSearch.privateBankCode = this.privateBankCodeCtrl.value;
      }

      if (!StringUtil.IsNullOrEmpty(this.privateBranchCodeCtrl.value)) {
        receiptSearch.payerCodePrefix = this.privateBranchCodeCtrl.value;
      }

      if (!StringUtil.IsNullOrEmpty(this.privateAccountNumberCtrl.value)) {
        receiptSearch.payerCodeSuffix = this.privateAccountNumberCtrl.value;
      }

      if (!StringUtil.IsNullOrEmpty(this.billNumberCtrl.value)) {
        receiptSearch.billNumber = this.billNumberCtrl.value;
      }

      if (!StringUtil.IsNullOrEmpty(this.billBankCodeCtrl.value)) {
        receiptSearch.billBankCode = this.billBankCodeCtrl.value;
      }

      if (!StringUtil.IsNullOrEmpty(this.billBranchCodeCtrl.value)) {
        receiptSearch.billBranchCode = this.billBranchCodeCtrl.value;
      }

      receiptSearch.billDrawAtFrom = DateUtil.ConvertFromDatepicker(this.billDrawAtFromCtrl);
      receiptSearch.billDrawAtTo = DateUtil.ConvertFromDatepicker(this.billDrawAtToCtrl);

      if (!StringUtil.IsNullOrEmpty(this.billDrawerCtrl.value)) {
        receiptSearch.billDrawer = this.billDrawerCtrl.value;
      }

      if (!StringUtil.IsNullOrEmpty(this.customerCodeFromCtrl.value)) {
        receiptSearch.customerCodeFrom = this.customerCodeFromCtrl.value;
      }

      if (!StringUtil.IsNullOrEmpty(this.customerCodeToCtrl.value)) {
        receiptSearch.customerCodeTo = this.customerCodeToCtrl.value;
      }

      if (this.userInfoService.ApplicationControl.useReceiptSection) {
        if (!StringUtil.IsNullOrEmpty(this.sectionCodeFromCtrl.value)) {
          receiptSearch.sectionCodeFrom = this.sectionCodeFromCtrl.value;
        }

        if (!StringUtil.IsNullOrEmpty(this.sectionCodeToCtrl.value)) {
          receiptSearch.sectionCodeTo = this.sectionCodeToCtrl.value;
        }
      }

      if (this.cbxMemoCtrl.value) {
        receiptSearch.existsMemo = 1;
      }
      else {
        receiptSearch.existsMemo = 0;
      }

      if (!StringUtil.IsNullOrEmpty(this.receiptMemoCtrl.value)) {
        receiptSearch.receiptMemo = this.receiptMemoCtrl.value;
      }

      if (this.inputTypeCtrl.value != 0) {
        receiptSearch.inputType = this.inputTypeCtrl.value;
      }

      if (!StringUtil.IsNullOrEmpty(this.receiptCategoryCodeFromCtrl.value)) {
        receiptSearch.receiptCategoryCodeFrom = this.receiptCategoryCodeFromCtrl.value;
      }

      if (!StringUtil.IsNullOrEmpty(this.receiptCategoryCodeToCtrl.value)) {
        receiptSearch.receiptCategoryCodeTo = this.receiptCategoryCodeToCtrl.value;
      }


      // 消込・未消込・一部消込
      // 2進数
      // 消込済み；0x004 = 4
      // 一部	；0x002 = 2
      // 未消込	；0x001 = 1
      receiptSearch.assignmentFlag = 0;
      if (this.cbxFullAssignmentFlagCtrl.value) {
        receiptSearch.assignmentFlag = receiptSearch.assignmentFlag + 4;
      }
      if (this.cbxPartAssignmentFlagCtrl.value) {
        receiptSearch.assignmentFlag = receiptSearch.assignmentFlag + 2;
      }
      if (this.cbxNoAssignmentFlagCtrl.value) {
        receiptSearch.assignmentFlag = receiptSearch.assignmentFlag + 1;
      }

      if (this.amountRangeCtrl.value.id == this.receiptAmountRangeDictionary[0].id) {
        receiptSearch.receiptAmountFrom = this.receiptAmountFromCtrl.value;
        receiptSearch.receiptAmountTo = this.receiptAmountToCtrl.value;
      }
      else {
        receiptSearch.remainAmountFrom = this.receiptAmountFromCtrl.value;
        receiptSearch.remainAmountTo = this.receiptAmountToCtrl.value;
      }

      receiptSearch.excludeFlag = this.excludeFlagCtrl.value;
      if (this.excludeCategoryIdCtrl.value != '0') {
        receiptSearch.excludeCategoryId = this.excludeCategoryIdCtrl.value;
      }

      if (!StringUtil.IsNullOrEmpty(this.sourceBankNameCtrl.value)) {
        receiptSearch.sourceBankName = this.sourceBankNameCtrl.value;
      }

      if (!StringUtil.IsNullOrEmpty(this.sourceBranchNameCtrl.value)) {
        receiptSearch.sourceBranchName = this.sourceBranchNameCtrl.value;
      }

      if (!StringUtil.IsNullOrEmpty(this.note1Ctrl.value)) {
        receiptSearch.note1 = this.note1Ctrl.value;
      }

      if (!StringUtil.IsNullOrEmpty(this.note2Ctrl.value)) {
        receiptSearch.note2 = this.note2Ctrl.value;
      }

      if (!StringUtil.IsNullOrEmpty(this.note3Ctrl.value)) {
        receiptSearch.note3 = this.note3Ctrl.value;
      }

      if (!StringUtil.IsNullOrEmpty(this.note4Ctrl.value)) {
        receiptSearch.note4 = this.note4Ctrl.value;
      }

      if (this.cbxDeletedFlagCtrl.value) {
        receiptSearch.deleteFlg = 1;
      }
      else {
        receiptSearch.deleteFlg = 0;
      }

      receiptSearch.deleteAtFrom = DateUtil.ConvertFromDatepickerToStart(this.deletedAtFromCtrl);
      receiptSearch.deleteAtTo = DateUtil.ConvertFromDatepickerToEnd(this.deletedAtToCtrl);

    }

    return receiptSearch;
  }


  ///////////////////////////////////////////////////////////
  public setRecordedAtFrom(eventType: string) {
    HtmlUtil.nextFocusByName(this.elementRef, 'recordedAtToCtrl', eventType);
  }

  public setRecordedAtTo(eventType: string) {
    HtmlUtil.nextFocusByName(this.elementRef, 'payerNameCtrl', eventType);
  }

  ///////////////////////////////////////////////////////////
  public setPayerName(eventType: string) {
    HtmlUtil.nextFocusByNames(this.elementRef, ['updateAtFromCtrl', 'bankCodeCtrl'], eventType);
  }

  public inputPayerName() {
    let value = this.payerNameCtrl.value;
    value = EbDataHelper.convertToValidEbkana(value);
    value = EbDataHelper.removePersonalities(value, this.juridicalPersonalitysResult.juridicalPersonalities);
    this.payerNameCtrl.setValue(value);
  }

  ///////////////////////////////////////////////////////////
  public setUpdateAtFrom(eventType: string) {
    HtmlUtil.nextFocusByName(this.elementRef, 'updateAtToCtrl', eventType);
  }

  public setUpdateAtTo(eventType: string) {
    HtmlUtil.nextFocusByName(this.elementRef, 'updateByCodeCtrl', eventType);
  }

  ///////////////////////////////////////////////////////////

  public setUpdateByCode(eventType: string) {

    if (eventType != EVENT_TYPE.BLUR) {
      this.updateByCodeTrigger.closePanel();
    } if (!StringUtil.IsNullOrEmpty(this.updateByCodeCtrl.value)) {

      this.loadStart();
      this.loginUserService.GetItems(this.updateByCodeCtrl.value)
        .subscribe(response => {
          this.loadEnd();
          if (response != undefined && response.length > 0) {
            this.updateById = response[0].id;
            this.updateByCodeCtrl.setValue(response[0].code);
            this.updateNameCtrl.setValue(response[0].name);
            HtmlUtil.nextFocusByNames(this.elementRef, ['currencyCodeCtrl', 'bankCodeCtrl'], eventType);
          }
          else {
            this.updateById = undefined;
            // this.updateByCodeCtrl.setValue('');
            this.updateNameCtrl.setValue('');
          }
        });
    }
    else {
      this.updateById = undefined;
      this.updateByCodeCtrl.setValue('');
      this.updateNameCtrl.setValue('');
      HtmlUtil.nextFocusByNames(this.elementRef, ['currencyCodeCtrl', 'bankCodeCtrl'], eventType);
    }
  }

  ///////////////////////////////////////////////////////////
  public setCurrencyCode(eventType: string) {

    if (!StringUtil.IsNullOrEmpty(this.currencyCodeCtrl.value)) {

      this.loadStart();
      this.currencyService.GetItems(this.currencyCodeCtrl.value)
        .subscribe(response => {
          this.loadEnd();
          if (response != undefined && response.length > 0) {
            this.currencyCodeCtrl.setValue(response[0].code);
            this.currencyId = response[0].id;
            HtmlUtil.nextFocusByName(this.elementRef, 'bankCodeCtrl', eventType);
          }
          else {
            // this.currencyCodeCtrl.setValue("");
            this.currencyId = 0;
            HtmlUtil.nextFocusByName(this.elementRef, 'bankCodeCtrl', eventType);
          }
        });
    }
    else {
      this.currencyCodeCtrl.setValue("");
      this.currencyId = 0;
      HtmlUtil.nextFocusByName(this.elementRef, 'bankCodeCtrl', eventType);
    }


  }

  ///////////////////////////////////////////////////////////////////////
  public setBankCode(eventType: string) {
    if (StringUtil.IsNullOrEmpty(this.bankCodeCtrl.value)) {
      this.bankCodeCtrl.setValue("");
      HtmlUtil.nextFocusByName(this.elementRef, 'branchCodeCtrl', eventType);
    }
    else {
      this.bankCodeCtrl.setValue(StringUtil.setPaddingFrontZero(this.bankCodeCtrl.value, 4, true));
      HtmlUtil.nextFocusByName(this.elementRef, 'branchCodeCtrl', eventType);
    }
  }

  public setBranchCode(eventType: string) {
    if (StringUtil.IsNullOrEmpty(this.branchCodeCtrl.value)) {
      this.branchCodeCtrl.setValue("");
      HtmlUtil.nextFocusByName(this.elementRef, 'accountTypeIdCtrl', eventType);
    }
    else {
      this.branchCodeCtrl.setValue(StringUtil.setPaddingFrontZero(this.branchCodeCtrl.value, 3, true));
      HtmlUtil.nextFocusByName(this.elementRef, 'accountTypeIdCtrl', eventType);
    }
  }

  public setAccountNumber(eventType: string) {
    if (StringUtil.IsNullOrEmpty(this.accountNumberCtrl.value)) {
      this.accountNumberCtrl.setValue("");
    }
    else {
      this.accountNumberCtrl.setValue(StringUtil.setPaddingFrontZero(this.accountNumberCtrl.value, 7, true));
    }
    HtmlUtil.nextFocusByNames(this.elementRef, ['privateBankCodeCtrl', 'customerCodeFromCtrl'], eventType);
  }


  ///////////////////////////////////////////////////////////////////////
  public setPrivateBankCode(eventType: string) {
    if (StringUtil.IsNullOrEmpty(this.privateBankCodeCtrl.value)) {
      this.privateBankCodeCtrl.setValue("");
      HtmlUtil.nextFocusByName(this.elementRef, 'privateBranchCodeCtrl', eventType);
    }
    else {
      this.privateBankCodeCtrl.setValue(StringUtil.setPaddingFrontZero(this.privateBankCodeCtrl.value, 4, true));
      HtmlUtil.nextFocusByName(this.elementRef, 'privateBranchCodeCtrl', eventType);
    }
  }

  public setPrivateBranchCode(eventType: string) {
    if (StringUtil.IsNullOrEmpty(this.privateBranchCodeCtrl.value)) {
      HtmlUtil.nextFocusByName(this.elementRef, 'privateAccountNumberCtrl', eventType);
      this.privateBranchCodeCtrl.setValue("");
    }
    else {
      this.privateBranchCodeCtrl.setValue(StringUtil.setPaddingFrontZero(this.privateBranchCodeCtrl.value, 3, true));
      HtmlUtil.nextFocusByName(this.elementRef, 'privateAccountNumberCtrl', eventType);
    }
  }

  public setPrivateAccountNumber(eventType: string) {

    if (StringUtil.IsNullOrEmpty(this.privateAccountNumberCtrl.value)) {
      this.accountNumberCtrl.setValue("");
      HtmlUtil.nextFocusByName(this.elementRef, 'billNumberCtrl', eventType);
    }
    else {
      this.privateAccountNumberCtrl.setValue(StringUtil.setPaddingFrontZero(this.privateAccountNumberCtrl.value, 7, true));
      HtmlUtil.nextFocusByName(this.elementRef, 'billNumberCtrl', eventType);
    }
  }


  ///////////////////////////////////////////////////////////////////////
  public setBillNumber(eventType: string) {
    HtmlUtil.nextFocusByName(this.elementRef, 'billBankCodeCtrl', eventType);
  }

  public setBillBankCode(eventType: string) {
    if (StringUtil.IsNullOrEmpty(this.billBankCodeCtrl.value)) {
      this.billBankCodeCtrl.setValue("");
      HtmlUtil.nextFocusByName(this.elementRef, 'billBranchCodeCtrl', eventType);
    }
    else {
      this.billBankCodeCtrl.setValue(StringUtil.setPaddingFrontZero(this.billBankCodeCtrl.value, 4, true));
      HtmlUtil.nextFocusByName(this.elementRef, 'billBranchCodeCtrl', eventType);
    }
  }

  public setBillBranchCode(eventType: string) {
    if (StringUtil.IsNullOrEmpty(this.billBranchCodeCtrl.value)) {
      this.billBranchCodeCtrl.setValue("");
      HtmlUtil.nextFocusByName(this.elementRef, 'billDrawAtFromCtrl', eventType);
    }
    else {
      this.billBranchCodeCtrl.setValue(StringUtil.setPaddingFrontZero(this.billBranchCodeCtrl.value, 3, true));
      HtmlUtil.nextFocusByName(this.elementRef, 'billDrawAtFromCtrl', eventType);
    }
  }

  ///////////////////////////////////////////////////////////
  public setBillDrawAtFrom(eventType: string) {
    HtmlUtil.nextFocusByName(this.elementRef, 'billDrawAtToCtrl', eventType);
  }

  public setBillDrawAtTo(eventType: string) {
    HtmlUtil.nextFocusByName(this.elementRef, 'billDrawerCtrl', eventType);
  }

  ///////////////////////////////////////////////////////////
  public setBillDrawer(eventType: string) {
    HtmlUtil.nextFocusByName(this.elementRef, 'customerCodeFromCtrl', eventType);
  }


  ///////////////////////////////////////////////////////////

  public setCustomerCodeFrom(eventType: string) {

    if (eventType != EVENT_TYPE.BLUR) {
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
    if (this.cbxCustomerCtrl.value) {
      this.customerCodeToCtrl.setValue(this.customerCodeFromCtrl.value);
      this.customerNameToCtrl.setValue(this.customerNameFromCtrl.value);
    }
    HtmlUtil.nextFocusByName(this.elementRef, 'customerCodeToCtrl', eventType);
  }

  public setCustomerCodeTo(eventType: string) {

    if (eventType != EVENT_TYPE.BLUR) {
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
    HtmlUtil.nextFocusByNames(
      this.elementRef, ['sectionCodeFromCtrl', 'receiptMemoCtrl', 'inputTypeCtrl', 'receiptCategoryCodeFromCtrl'], eventType);
  }

  public setCbxCustomer(eventType: string) {
    let localstorageItem = new LocalStorageItem();
    localstorageItem.key = RangeSearchKey.PE0601_CUSTOMER;
    localstorageItem.value = this.cbxCustomerCtrl.value;
    this.localStorageManageService.set(localstorageItem);

    HtmlUtil.nextFocusByName(this.elementRef, "customerCodeFromCtrl", eventType);
  }

  ///////////////////////////////////////////////////////////

  public setSectionCodeFrom(eventType: string) {

    if (eventType != EVENT_TYPE.BLUR) {
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
            this.customerNameFromCtrl.setValue("");
            if (this.cbxSectionCtrl.value) {
              this.sectionCodeToCtrl.setValue(this.sectionCodeFromCtrl.value);
              this.sectionNameToCtrl.setValue(this.sectionNameFromCtrl.value);
            }
          }
        });
    }
    else {
      this.sectionCodeFromCtrl.setValue("");
      this.customerNameFromCtrl.setValue("");
    }
    if (this.cbxSectionCtrl.value) {
      this.sectionCodeToCtrl.setValue(this.sectionCodeFromCtrl.value);
      this.sectionNameToCtrl.setValue(this.sectionNameFromCtrl.value);
    }
    HtmlUtil.nextFocusByName(this.elementRef, 'sectionCodeToCtrl', eventType);
  }

  public setSectionCodeTo(eventType: string) {

    if (eventType != EVENT_TYPE.BLUR) {
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
    HtmlUtil.nextFocusByName(this.elementRef, 'receiptMemoCtrl', eventType);
  }

  public setCbxSection(eventType: string) {
    let localstorageItem = new LocalStorageItem();
    localstorageItem.key = RangeSearchKey.PE0601_SECTION;
    localstorageItem.value = this.cbxSectionCtrl.value;
    this.localStorageManageService.set(localstorageItem);

    HtmlUtil.nextFocusByName(this.elementRef, "sectionCodeFromCtrl", eventType);
  }


  ///////////////////////////////////////////////////////////
  public setCbxMemo(eventType: string) {
    HtmlUtil.nextFocusByName(this.elementRef, 'receiptMemoCtrl', eventType);
  }

  ///////////////////////////////////////////////////////////
  public setCbxUseReceiptSection(eventType: string) {
    let localstorageItem = new LocalStorageItem();
    localstorageItem.key = RangeSearchKey.PE0601_USE_RECEIPT_SECTION;
    localstorageItem.value = this.cbxUseReceiptSectionCtrl.value;
    this.localStorageManageService.set(localstorageItem);

  }


  ///////////////////////////////////////////////////////////  
  public setReceiptMemo(eventType: string) {
    HtmlUtil.nextFocusByName(this.elementRef, 'inputTypeCtrl', eventType);
  }

  public setInputType(eventType: string) {
    HtmlUtil.nextFocusByName(this.elementRef, 'receiptCategoryCodeFromCtrl', eventType);
  }

  ///////////////////////////////////////////////////////////

  public setReceiptCategoryCodeFrom(eventType: string) {

    if (eventType != EVENT_TYPE.BLUR) {
      this.receiptCategoryCodeFromTrigger.closePanel();
    }

    let code = this.receiptCategoryCodeFromCtrl.value;

    if (!StringUtil.IsNullOrEmpty(code)) {
      code = StringUtil.setPaddingFrontZero(code, 2);
      this.loadStart();
      this.categoryService.GetItems(CategoryType.Receipt, code, null)
        .subscribe(response => {
          this.loadEnd();
          if (response != undefined && response.length > 0) {

            this.receiptCategoryCodeFromCtrl.setValue(response[0].code);
            this.receiptCategoryNameFromCtrl.setValue(response[0].name);
          }
          else {
            // this.receiptCategoryCodeFromCtrl.setValue(code);
            this.receiptCategoryNameFromCtrl.setValue('');
          }
          if (this.cbxReceiptCategoryCtrl.value) {
            this.receiptCategoryCodeToCtrl.setValue(this.receiptCategoryCodeFromCtrl.value);
            this.receiptCategoryNameToCtrl.setValue(this.receiptCategoryNameFromCtrl.value);
          }
        });

    }
    else {
      // this.receiptCategoryCodeFromCtrl.setValue('');
      this.receiptCategoryNameFromCtrl.setValue('');
    }
    if (this.cbxReceiptCategoryCtrl.value) {
      this.receiptCategoryCodeToCtrl.setValue(this.receiptCategoryCodeFromCtrl.value);
      this.receiptCategoryNameToCtrl.setValue(this.receiptCategoryNameFromCtrl.value);
    }
    HtmlUtil.nextFocusByName(this.elementRef, 'receiptCategoryCodeToCtrl', eventType);
  }

  public setReceiptCategoryCodeTo(eventType: string) {

    if (eventType != EVENT_TYPE.BLUR) {
      this.receiptCategoryCodeToTrigger.closePanel();
    }

    let code = this.receiptCategoryCodeToCtrl.value;
    if (!StringUtil.IsNullOrEmpty(code)) {
      code = StringUtil.setPaddingFrontZero(code, 2);
      this.loadStart();
      this.categoryService.GetItems(CategoryType.Receipt, code, null)
        .subscribe(response => {
          this.loadEnd();
          if (response != undefined && response.length > 0) {
            this.receiptCategoryCodeToCtrl.setValue(response[0].code);
            this.receiptCategoryNameToCtrl.setValue(response[0].name);
          }
          else {
            // this.receiptCategoryCodeToCtrl.setValue(code);
            this.receiptCategoryNameToCtrl.setValue('');
          }
        });

    }
    else {
      // this.receiptCategoryCodeToCtrl.setValue("");
      this.receiptCategoryNameToCtrl.setValue("");
    }
    HtmlUtil.nextFocusByName(this.elementRef, 'excludeCategoryIdCtrl', eventType);
  }

  public setCbxReceiptCategory(eventType: string) {
    let localstorageItem = new LocalStorageItem();
    localstorageItem.key = RangeSearchKey.PE0601_RECEIPT_CATEGORY;
    localstorageItem.value = this.cbxReceiptCategoryCtrl.value;
    this.localStorageManageService.set(localstorageItem);

    HtmlUtil.nextFocusByName(this.elementRef, "receiptCategoryFromCtrl", eventType);
  }


  ///////////////////////////////////////////////////////////
  public setExcludeFlag(eventType: string) {

    if (this.excludeFlagCtrl.value == 1) {
      this.excludeCategoryIdCtrl.enable();
    }
    else {
      this.excludeCategoryIdCtrl.setValue("");
      this.excludeCategoryIdCtrl.disable();
    }
    HtmlUtil.nextFocusByName(this.elementRef, 'excludeCategoryIdCtrl', eventType);
  }

  ///////////////////////////////////////////////////////////
  public setExcludeCategoryId(eventType: string) {
    HtmlUtil.nextFocusByName(this.elementRef, 'amountRangeCtrl', eventType);
  }

  ///////////////////////////////////////////////////////////
  public setAmountRange(eventType: string) {
    HtmlUtil.nextFocusByName(this.elementRef, 'receiptAmountFromCtrl', eventType);
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

  public onFocusReceiptAmountFrom() {
    this.onFocusCurrencyControl(this.receiptAmountFromCtrl);
  }
  public onLeaveReceiptAmountFrom() {
    this.onLeaveCurrencyControl(this.receiptAmountFromCtrl);
    if (this.cbxReceiptAmountCtrl.value) {
      this.receiptAmountToCtrl.setValue(this.receiptAmountFromCtrl.value);
    }
  }

  public setReceiptAmountFrom(eventType: string) {
    HtmlUtil.nextFocusByName(this.elementRef, 'receiptAmountToCtrl', eventType);
  }

  public onFocusReceiptAmountTo() {
    this.onFocusCurrencyControl(this.receiptAmountToCtrl);
  }
  public onLeaveReceiptAmountTo() {
    this.onLeaveCurrencyControl(this.receiptAmountToCtrl);
  }

  public setReceiptAmountTo(eventType: string) {
    HtmlUtil.nextFocusByName(this.elementRef, 'sourceBankNameCtrl', eventType);
  }

  public setCbxReceiptAmount(eventType: string) {
    let localstorageItem = new LocalStorageItem();
    localstorageItem.key = RangeSearchKey.PE0601_AMOUNT;
    localstorageItem.value = this.cbxReceiptAmountCtrl.value;
    this.localStorageManageService.set(localstorageItem);

    HtmlUtil.nextFocusByName(this.elementRef, "receiptAmountFromCtrl", eventType);
  }


  ///////////////////////////////////////////////////////////
  public setSourceBankName(eventType: string) {
    HtmlUtil.nextFocusByName(this.elementRef, 'sourceBranchNameCtrl', eventType);
  }

  public setSourceBranchName(eventType: string) {
    HtmlUtil.nextFocusByName(this.elementRef, 'note1Ctrl', eventType);
  }

  public inputSourceBankName() {
    this.sourceBankNameCtrl.setValue(EbDataHelper.convertToValidEbkana(this.sourceBankNameCtrl.value));
  }

  public inputSourceBranchName() {
    this.sourceBranchNameCtrl.setValue(EbDataHelper.convertToValidEbkana(this.sourceBranchNameCtrl.value));
  }

  ///////////////////////////////////////////////////////////
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
    HtmlUtil.nextFocusByName(this.elementRef, 'cbxDeletedFlagCtrl', eventType);
  }

  public setCbxDeleteFlag(eventType: string) {

    if (this.cbxDeletedFlagCtrl.value) {
      this.deletedAtFromCtrl.setValue(null);
      this.deletedAtToCtrl.setValue(null);

      this.deletedAtFromCtrl.enable();
      this.deletedAtToCtrl.enable();

      this.deleteOrRecoveryButton = this.ItemNameConst.BTN_RESTORE;
      this.securityHideShow = this.userInfoService.isFunctionAvailable(FunctionType.RecoverReceipt);

      HtmlUtil.nextFocusByName(this.elementRef, 'deletedAtFromCtrl', eventType);

    }
    else {
      this.deletedAtFromCtrl.setValue(null);
      this.deletedAtToCtrl.setValue(null);

      this.deletedAtFromCtrl.disable();
      this.deletedAtToCtrl.disable();

      this.deleteOrRecoveryButton = "削除";
      this.securityHideShow = this.userInfoService.isFunctionAvailable(FunctionType.ModifyReceipt);
    }
    
  }


  public setDeletedAtFrom(eventType: string) {
    HtmlUtil.nextFocusByName(this.elementRef, 'deletedAtToCtrl', eventType);
  }

  public setDeletedAtTo(eventType: string) {
    HtmlUtil.nextFocusByName(this.elementRef, 'recordedAtFromCtrl', eventType);
  }

  ///////////////////////////////////////////////////////////
  public setCbxDetailDelFlag(eventType: string, index: number) {

    this.selectedDelete = false;
    this.cbxDetailDelFlagCtrls.forEach(element => {
      if (element.value) this.selectedDelete = true;
    });
  }


  public setSearchType(event:any){

    this.simplePageSearch=!this.simplePageSearch;

    if (this.recordedAtFromCtrl.errors != null) {
      this.recordedAtFromCtrl.reset();
    }
    
    if (this.recordedAtToCtrl.errors != null) {
      this.recordedAtToCtrl.reset();
    }

    if (this.updateAtFromCtrl.errors != null) {
      this.updateAtFromCtrl.reset();
    }
    
    if (this.updateAtToCtrl.errors != null) {
      this.updateAtToCtrl.reset();
    }    

    if (this.billDrawAtFromCtrl.errors != null) {
      this.billDrawAtFromCtrl.reset();
    }
    
    if (this.billDrawAtToCtrl.errors != null) {
      this.billDrawAtToCtrl.reset();
    }    

    if (this.deletedAtFromCtrl.errors != null) {
      this.deletedAtFromCtrl.reset();
    }
    
    if (this.deletedAtToCtrl.errors != null) {
      this.deletedAtToCtrl.reset();
    }

  }

}
