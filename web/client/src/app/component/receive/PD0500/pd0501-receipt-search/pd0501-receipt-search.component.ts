import { Component, OnInit, ElementRef, ComponentFactoryResolver, ViewContainerRef, AfterViewInit, ViewChild } from '@angular/core';
import { TABLE_INDEX } from 'src/app/common/const/table-name.const';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Router, ParamMap, ActivatedRoute, NavigationEnd } from '@angular/router';
import { ReceiptService } from 'src/app/service/receipt.service';
import { BaseComponent } from 'src/app/component/common/base/base-component';
import { ModalMasterComponent } from 'src/app/component/modal/modal-master/modal-master.component';
import { HtmlUtil } from 'src/app/common/util/html.util';
import { KEY_CODE, EVENT_TYPE, BUTTON_ACTION } from 'src/app/common/const/event.const';
import { StringUtil } from 'src/app/common/util/string-util';
import { CategoryMasterService } from 'src/app/service/Master/category-master.service';
import { ModalMasterBankAccountComponent } from 'src/app/component/modal/modal-master-bank-account/modal-master-bank-account.component';
import { ACCOUNT_TYPE_DICTIONARY, RECEIPT_INPUT_TYPE_DICTIONARY, RECEIPT_EXCLUDE_FLAG_DICTIONARY, CategoryType, GridId, MATCHING_ASSIGNMENT_FLAG_DICTIONARY, RECEIPT_AMOUNT_RANGE_DICTIONARY, CODE_TYPE, FunctionType } from 'src/app/common/const/kbn.const';
import { ModalMasterBankComponent } from 'src/app/component/modal/modal-master-bank/modal-master-bank.component';
import { ReceiptsResult } from 'src/app/model/receipts-result.model';
import { ReceiptSearch } from 'src/app/model/receipt-search.model';
import { FormatterUtil, } from 'src/app/common/util/formatter.util';
import { MODAL_STATUS_TYPE, PROCESS_RESULT_RESULT_TYPE } from 'src/app/common/const/status.const';
import { UserInfoService } from 'src/app/service/common/user-info.service';
import { NgbDateParserFormatter } from '@ng-bootstrap/ng-bootstrap';
import { NgbDateCustomParserFormatter } from 'src/app/common/util/dateformat-util';
import { LoginUserMasterService } from 'src/app/service/Master/login-user-master.service';
import { CurrencyMasterService } from 'src/app/service/Master/currency-master.service';
import { CustomerMasterService } from 'src/app/service/Master/customer-master.service';
import { SectionMasterService } from 'src/app/service/Master/section-master.service';
import { GridSettingsResult } from 'src/app/model/grid-settings-result.model';
import { CategoriesResult } from 'src/app/model/categories-result.model';
import { JuridicalPersonalitysResult } from 'src/app/model/juridical-personalitys-result.model';
import { GridSettingMasterService } from 'src/app/service/Master/grid-setting-master.service';
import { JuridicalPersonalityMasterService } from 'src/app/service/Master/juridical-personality-master.service';
import { forkJoin } from 'rxjs';
import { DateUtil } from 'src/app/common/util/date-util';
import { ReceiptExclude } from 'src/app/model/receipt-exclude.model';
import { ReceiptExcludeResult } from 'src/app/model/receipt-exclude-result.model';
import { ModalReceiptExcludeAllComponent } from 'src/app/component/modal/modal-receipt-exclude-all/modal-receipt-exclude-all.component';
import { Receipt } from 'src/app/model/receipt.model';
import { ModalReceiptExcludeDetailComponent } from 'src/app/component/modal/modal-receipt-exclude-detail/modal-receipt-exclude-detail.component';
import { ModalMemoComponent } from 'src/app/component/modal/modal-memo/modal-memo.component';
import { ReceiptMemo } from 'src/app/model/receipt-memo.model';
import { ComponentId } from 'src/app/common/const/component-name.const';
import { MatchingService } from 'src/app/service/matching.service';
import { EbDataHelper } from 'src/app/common/util/eb-data-helper';
import { PageUtil } from 'src/app/common/util/page-util';
import { ProcessResultService } from 'src/app/service/common/process-result.service';
import { MSG_WNG, MSG_ERR, MSG_ITEM_NUM, MSG_INF } from 'src/app/common/const/message.const';
import { FILE_EXTENSION } from 'src/app/common/const/eb-file.const';
import { FileUtil } from 'src/app/common/util/file.util';
import { LINE_FEED_CODE } from 'src/app/common/const/report-setting-kbn.const';
import { GridSetting } from 'src/app/model/grid-setting.model';
import { RacCurrencyPipe } from 'src/app/pipe/currency.pipe';
import { LoginUser } from 'src/app/model/login-user.model';
import {   LocalStorageManageService } from 'src/app/service/common/local-storage-manage.service';
import { LocalStorageItem } from 'src/app/model/custom-model/local-storage-item';
import { GridSizeKey, RangeSearchKey } from 'src/app/common/const/local-storage-key-const';
import { GridSettingHelper } from 'src/app/model/grid-setting-helper.model';
import { ReceiptHelper } from 'src/app/model/helper/receipt-helper.model';
import { ModalRouterProgressComponent } from 'src/app/component/modal/modal-router-progress/modal-router-progress.component';
import { MatAutocompleteTrigger, MatExpansionPanel } from '@angular/material';

@Component({
  selector: 'app-pd0501-receipt-search',
  templateUrl: './pd0501-receipt-search.component.html',
  styleUrls: ['./pd0501-receipt-search.component.css'],
  providers: [
    { provide: NgbDateParserFormatter, useClass: NgbDateCustomParserFormatter }
  ]

})
export class Pd0501ReceiptSearchComponent extends BaseComponent implements OnInit,AfterViewInit {

  // 遷移元のコンポーネント
  public paramFrom: ComponentId;


  // これをしないとEnum型がテンプレートで見えなかったため
  public TABLE_INDEX: typeof TABLE_INDEX = TABLE_INDEX;
  public MATCHING_ASSIGNMENT_FLAG_DICTIONARY: typeof MATCHING_ASSIGNMENT_FLAG_DICTIONARY = MATCHING_ASSIGNMENT_FLAG_DICTIONARY;
  public RECEIPT_INPUT_TYPE_DICTIONARY: typeof RECEIPT_INPUT_TYPE_DICTIONARY = RECEIPT_INPUT_TYPE_DICTIONARY;

  public readonly accountTypeDictionary = ACCOUNT_TYPE_DICTIONARY;
  public readonly inputTypeDictionary = RECEIPT_INPUT_TYPE_DICTIONARY;
  public readonly excludeFlagDictionary = RECEIPT_EXCLUDE_FLAG_DICTIONARY;
  public readonly receiptAmountRangeDictionary = RECEIPT_AMOUNT_RANGE_DICTIONARY;

  // 検索パネルの開閉フラグ
  public panelOpenState: boolean;

  public selectReceipt: Receipt;

  public receiptsResult: ReceiptsResult;
  public receiptSearch: ReceiptSearch;

  public cbxCheckAllCtrl:FormControl;

  public recordedAtFromCtrl: FormControl;  // 入金日
  public recordedAtToCtrl: FormControl;

  public payerNameCtrl: FormControl; // 振込依頼人名

  public updateAtFromCtrl: FormControl;  // 最終更新日
  public updateAtToCtrl: FormControl;

  public updateByCodeCtrl: FormControl; // 最終更新者
  public updateNameCtrl: FormControl;

  public currencyCodeCtrl: FormControl;  // 通貨コード

  public bankCodeCtrl: FormControl; // 銀行
  public branchCodeCtrl: FormControl;
  public accountTypeIdCtrl: FormControl;
  public accountNumberCtrl: FormControl;

  public privateBankCodeCtrl: FormControl; // 専用銀行
  public privateBranchCodeCtrl: FormControl;
  public privateAccountTypeIdCtrl: FormControl;
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

  public detailCbxExcludeFlagCtrls: FormControl[]; // 詳細 対象外区分 チェック
  public detailExcludeCategoryIdCtrls: FormControl[]; // 詳細 対象外区分

  public undefineCtrl: FormControl; // 未定用

  public gridSettingsResult: GridSettingsResult;  // グリッド設定
  public excludeCategoriesResult: CategoriesResult;  // 対象外
  public juridicalPersonalitiesResult: JuridicalPersonalitysResult;  // 法人格除去用
  public AllGridSettings: GridSetting[];
  public updateById: number | null;

  public dispSumCount: number;
  public dispSumReceiptAmount: number;
  public dispSumRemainAmount: number;

  public gridSettingHelper = new GridSettingHelper();

  public simplePageSearch:boolean=true;
  public cbxColumnName: string; // チェックボックスの列名

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
    public gridSettingService: GridSettingMasterService,
    public categoryService: CategoryMasterService,
    public juridicalPersonalityService: JuridicalPersonalityMasterService,
    public receiptService: ReceiptService,
    public loginUserService: LoginUserMasterService,
    public currencyService: CurrencyMasterService,
    public customerService: CustomerMasterService,
    public sectionService: SectionMasterService,
    public matchingService: MatchingService,
    public processResultService: ProcessResultService,
    public localStorageManageService:LocalStorageManageService,
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

    if (this.userInfoService.isFunctionAvailable(FunctionType.ModifyReceipt)) {
      this.cbxColumnName = '修正';
    } else {
      this.cbxColumnName = '参照';
    }
    
    this.activatedRoute.paramMap.subscribe((params: ParamMap) => {
      // ルートパラメータと同じ要領で Matrix URI パラメータが取得できる
      let paramProecess = params.get("process");
      this.paramFrom = parseInt(params.get("from"));

      if (!StringUtil.IsNullOrEmpty(paramProecess) && paramProecess == "back") {

        // 検索条件の設定
        //this.receiptSearch = this.receiptService.ReceiptSearch;
        this.MyFormGroup = this.receiptService.ReceiptSearchFormGroup;
        this.readControlInit();


        // 検索の実行
        this.search();

      }
      else {
        this.setControlInit();
        this.setValidator();
        this.setFormatter();
        this.clear();
        this.setAutoComplete();
      }
    });

    // アプリケーションコントロール、メニュー権限、機能権限の取得、管理マスタ、通貨
    let gridSettingResponse =
      this.gridSettingService.GetItems(GridId.ReceiptSearch);

    let excludeCategoryResponse =
      this.categoryService.GetItemsByCategoryType(CategoryType.Exclude);

    let juridicalPersonalityResponse =
      this.juridicalPersonalityService.GetItems();

    // ３つの処理の待機
    forkJoin(
      gridSettingResponse,
      excludeCategoryResponse,
      juridicalPersonalityResponse,
    )
      .subscribe(responseList => {

        if (responseList.length != 3
          || responseList[0] == PROCESS_RESULT_RESULT_TYPE.FAILURE
          || responseList[1] == PROCESS_RESULT_RESULT_TYPE.FAILURE
          || responseList[2] == PROCESS_RESULT_RESULT_TYPE.FAILURE
        ) {
        }
        else {
          this.gridSettingsResult = new GridSettingsResult();
          this.gridSettingsResult.gridSettings = responseList[0].filter((gridSetting: { displayWidth: number; }) => { return gridSetting.displayWidth != 0 });
          // 検索条件用絞込無
          this.AllGridSettings = responseList[0];
          this.excludeCategoriesResult = new CategoriesResult();
          this.excludeCategoriesResult.categories = responseList[1];
          this.juridicalPersonalitiesResult = new JuridicalPersonalitysResult();
          this.juridicalPersonalitiesResult.juridicalPersonalities = responseList[2];
        }
      },
        error => {
        }
      );

  }

  ngAfterViewInit(){
    HtmlUtil.nextFocusByName(this.elementRef, 'recordedAtFromCtrl', EVENT_TYPE.NONE);
  }

  public readControlInit() {

    this.recordedAtFromCtrl = <FormControl>this.MyFormGroup.controls['recordedAtFromCtrl'];  // 入金日
    this.recordedAtToCtrl = <FormControl>this.MyFormGroup.controls['recordedAtToCtrl'];

    this.payerNameCtrl = <FormControl>this.MyFormGroup.controls['payerNameCtrl']; // 振込依頼人名

    this.updateAtFromCtrl = <FormControl>this.MyFormGroup.controls['updateAtFromCtrl']; // 最終更新日
    this.updateAtToCtrl = <FormControl>this.MyFormGroup.controls['updateAtToCtrl'];

    this.updateByCodeCtrl = <FormControl>this.MyFormGroup.controls['updateByCodeCtrl']; // 最終更新者
    this.updateNameCtrl = <FormControl>this.MyFormGroup.controls['updateNameCtrl'];

    this.currencyCodeCtrl = <FormControl>this.MyFormGroup.controls['currencyCodeCtrl'];  // 通貨コード

    this.bankCodeCtrl = <FormControl>this.MyFormGroup.controls['bankCodeCtrl']; // 銀行
    this.branchCodeCtrl = <FormControl>this.MyFormGroup.controls['branchCodeCtrl'];
    this.accountTypeIdCtrl = <FormControl>this.MyFormGroup.controls['accountTypeIdCtrl'];
    this.accountNumberCtrl = <FormControl>this.MyFormGroup.controls['accountNumberCtrl'];

    this.privateBankCodeCtrl = <FormControl>this.MyFormGroup.controls['privateBankCodeCtrl']; // 専用銀行
    this.privateBranchCodeCtrl = <FormControl>this.MyFormGroup.controls['privateBranchCodeCtrl'];
    this.privateAccountTypeIdCtrl = <FormControl>this.MyFormGroup.controls['privateAccountTypeIdCtrl'];
    this.privateAccountNumberCtrl = <FormControl>this.MyFormGroup.controls['privateAccountNumberCtrl'];

    this.billNumberCtrl = <FormControl>this.MyFormGroup.controls['billNumberCtrl'];  // 手形券面情報
    this.billBankCodeCtrl = <FormControl>this.MyFormGroup.controls['billBankCodeCtrl'];
    this.billBranchCodeCtrl = <FormControl>this.MyFormGroup.controls['billBranchCodeCtrl'];
    this.billDrawAtFromCtrl = <FormControl>this.MyFormGroup.controls['billDrawAtFromCtrl'];
    this.billDrawAtToCtrl = <FormControl>this.MyFormGroup.controls['billDrawAtToCtrl'];
    this.billDrawerCtrl = <FormControl>this.MyFormGroup.controls['billDrawerCtrl'];

    this.customerCodeFromCtrl = <FormControl>this.MyFormGroup.controls['customerCodeFromCtrl'];  // 得意先コード
    this.customerNameFromCtrl = <FormControl>this.MyFormGroup.controls['customerNameFromCtrl'];
    this.customerCodeToCtrl = <FormControl>this.MyFormGroup.controls['customerCodeToCtrl'];
    this.customerNameToCtrl = <FormControl>this.MyFormGroup.controls['customerNameToCtrl'];
    this.cbxCustomerCtrl = <FormControl>this.MyFormGroup.controls['cbxCustomerCtrl'];

    this.sectionCodeFromCtrl = <FormControl>this.MyFormGroup.controls['sectionCodeFromCtrl']; // 入金部門コード
    this.sectionNameFromCtrl = <FormControl>this.MyFormGroup.controls['sectionNameFromCtrl'];
    this.sectionCodeToCtrl = <FormControl>this.MyFormGroup.controls['sectionCodeToCtrl'];
    this.sectionNameToCtrl = <FormControl>this.MyFormGroup.controls['sectionNameToCtrl'];
    this.cbxSectionCtrl = <FormControl>this.MyFormGroup.controls['cbxSectionCtrl'];

    this.cbxMemoCtrl = <FormControl>this.MyFormGroup.controls['cbxMemoCtrl']; // メモ
    this.receiptMemoCtrl = <FormControl>this.MyFormGroup.controls['receiptMemoCtrl'];

    this.cbxUseReceiptSectionCtrl = <FormControl>this.MyFormGroup.controls['cbxUseReceiptSectionCtrl'];  // 入金部門対応マスターを使用

    this.inputTypeCtrl = <FormControl>this.MyFormGroup.controls['inputTypeCtrl']; // 入力区分

    this.receiptCategoryCodeFromCtrl = <FormControl>this.MyFormGroup.controls['receiptCategoryCodeFromCtrl']; // 入金区分
    this.receiptCategoryNameFromCtrl = <FormControl>this.MyFormGroup.controls['receiptCategoryNameFromCtrl'];
    this.receiptCategoryCodeToCtrl = <FormControl>this.MyFormGroup.controls['receiptCategoryCodeToCtrl'];
    this.receiptCategoryNameToCtrl = <FormControl>this.MyFormGroup.controls['receiptCategoryNameToCtrl'];
    this.cbxReceiptCategoryCtrl = <FormControl>this.MyFormGroup.controls['cbxReceiptCategoryCtrl'];

    this.cbxFullAssignmentFlagCtrl = <FormControl>this.MyFormGroup.controls['cbxFullAssignmentFlagCtrl']; // 消込区分
    this.cbxPartAssignmentFlagCtrl = <FormControl>this.MyFormGroup.controls['cbxPartAssignmentFlagCtrl'];
    this.cbxNoAssignmentFlagCtrl = <FormControl>this.MyFormGroup.controls['cbxNoAssignmentFlagCtrl'];

    this.excludeFlagCtrl = <FormControl>this.MyFormGroup.controls['excludeFlagCtrl']; // 対象外状態

    this.excludeCategoryIdCtrl = <FormControl>this.MyFormGroup.controls['excludeCategoryIdCtrl'];// 対象外区分

    this.amountRangeCtrl = <FormControl>this.MyFormGroup.controls['amountRangeCtrl']; // 金額範囲
    this.receiptAmountFromCtrl = <FormControl>this.MyFormGroup.controls['receiptAmountFromCtrl'];;
    this.receiptAmountToCtrl = <FormControl>this.MyFormGroup.controls['receiptAmountToCtrl'];;
    this.cbxReceiptAmountCtrl = <FormControl>this.MyFormGroup.controls['cbxReceiptAmountCtrl'];;

    this.sourceBankNameCtrl = <FormControl>this.MyFormGroup.controls['sourceBankNameCtrl']; // 仕向
    this.sourceBranchNameCtrl = <FormControl>this.MyFormGroup.controls['sourceBranchNameCtrl'];;

    this.note1Ctrl = <FormControl>this.MyFormGroup.controls['note1Ctrl']; // 備考
    this.note2Ctrl = <FormControl>this.MyFormGroup.controls['note2Ctrl'];
    this.note3Ctrl = <FormControl>this.MyFormGroup.controls['note3Ctrl'];
    this.note4Ctrl = <FormControl>this.MyFormGroup.controls['note4Ctrl'];

    this.undefineCtrl = <FormControl>this.MyFormGroup.controls['undefineCtrl']; // 未定用;

  }
  public setControlInit() {

    this.cbxCheckAllCtrl = new FormControl(""); // 全選択・全解除

    this.recordedAtFromCtrl = new FormControl("", [Validators.maxLength(10)]);  // 入金日
    this.recordedAtToCtrl = new FormControl("", [Validators.maxLength(10)]);

    this.payerNameCtrl = new FormControl("", [Validators.maxLength(140)]); // 振込依頼人名

    this.updateAtFromCtrl = new FormControl("", [Validators.maxLength(10)]); // 最終更新日
    this.updateAtToCtrl = new FormControl("", [Validators.maxLength(10)]);

    this.updateByCodeCtrl = new FormControl("", [Validators.maxLength(this.userInfoService.ApplicationControl.loginUserCodeLength)]); // 最終更新者
    this.updateNameCtrl = new FormControl("");

    this.currencyCodeCtrl = new FormControl("", [Validators.maxLength(3)]);  // 通貨コード

    this.bankCodeCtrl = new FormControl("", [Validators.maxLength(4)]); // 銀行
    this.branchCodeCtrl = new FormControl("", [Validators.maxLength(3)]);
    this.accountTypeIdCtrl = new FormControl("");
    this.accountNumberCtrl = new FormControl("", [Validators.maxLength(7)]);

    this.privateBankCodeCtrl = new FormControl("", [Validators.maxLength(4)]); // 専用銀行
    this.privateBranchCodeCtrl = new FormControl("", [Validators.maxLength(3)]);
    this.privateAccountTypeIdCtrl = new FormControl("");
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
    this.receiptAmountFromCtrl = new FormControl("", [Validators.maxLength(16)]);
    this.receiptAmountToCtrl = new FormControl("", [Validators.maxLength(16)]);
    this.cbxReceiptAmountCtrl = new FormControl("");

    this.sourceBankNameCtrl = new FormControl("", [Validators.maxLength(140)]); // 仕向
    this.sourceBranchNameCtrl = new FormControl("", [Validators.maxLength(15)]);

    this.note1Ctrl = new FormControl("", [Validators.maxLength(100)]); // 備考
    this.note2Ctrl = new FormControl("", [Validators.maxLength(100)]);
    this.note3Ctrl = new FormControl("", [Validators.maxLength(100)]);
    this.note4Ctrl = new FormControl("", [Validators.maxLength(100)]);


    this.undefineCtrl = new FormControl(""); // 未定用;


  }

  public setValidator() {
    this.MyFormGroup = new FormGroup({

      cbxCheckAllCtrl:this.cbxCheckAllCtrl,

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
      privateAccountTypeIdCtrl: this.privateAccountTypeIdCtrl,
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

    FormatterUtil.setAlphabetFormatter(this.currencyCodeCtrl); // 通貨コード

    FormatterUtil.setNumberFormatter(this.bankCodeCtrl); // 銀行
    FormatterUtil.setNumberFormatter(this.branchCodeCtrl);
    FormatterUtil.setNumberFormatter(this.accountNumberCtrl);

    FormatterUtil.setNumberFormatter(this.privateBankCodeCtrl); // 専用銀行
    FormatterUtil.setNumberFormatter(this.privateBranchCodeCtrl);
    FormatterUtil.setNumberFormatter(this.privateAccountNumberCtrl);

    FormatterUtil.setCodeFormatter(this.billNumberCtrl); // 手形券面情報
    FormatterUtil.setNumberFormatter(this.billBankCodeCtrl);
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
      FormatterUtil.setNumberFormatter(this.sectionCodeFromCtrl); // 入金部門コード
      FormatterUtil.setNumberFormatter(this.sectionCodeToCtrl);
    }
    else {
      FormatterUtil.setCodeFormatter(this.sectionCodeFromCtrl);
      FormatterUtil.setCodeFormatter(this.sectionCodeToCtrl);
    }

    FormatterUtil.setNumberFormatter(this.receiptCategoryCodeFromCtrl); // 入金区分
    FormatterUtil.setNumberFormatter(this.receiptCategoryCodeToCtrl);

    FormatterUtil.setCurrencyFormatter(this.receiptAmountFromCtrl); // 金額
    FormatterUtil.setCurrencyFormatter(this.receiptAmountToCtrl);
  }


  public setAutoComplete(){
    // 最終更新者
    this.initAutocompleteLoginUsers(this.updateByCodeCtrl,this.loginUserService,0);

    // 入金区分
    this.initAutocompleteCategories(CategoryType.Receipt,this.receiptCategoryCodeFromCtrl,this.categoryService,0);
    this.initAutocompleteCategories(CategoryType.Receipt,this.receiptCategoryCodeToCtrl,this.categoryService,1);

    // 得意先
    this.initAutocompleteCustomers(this.customerCodeFromCtrl,this.customerService,0);
    this.initAutocompleteCustomers(this.customerCodeToCtrl,this.customerService,1);

    // 入金部門
    this.initAutocompleteSections(this.sectionCodeFromCtrl,this.sectionService,0);
    this.initAutocompleteSections(this.sectionCodeToCtrl,this.sectionService,1);


  }

  public closeAutoCompletePanel(){

    if (this.updateByCodeTrigger!= undefined){this.updateByCodeTrigger.closePanel();}
    if (this.customerCodeFromTrigger!= undefined){this.customerCodeFromTrigger.closePanel();}
    if (this.customerCodeToTrigger!= undefined){this.customerCodeToTrigger.closePanel();}
    if (this.sectionCodeFromTrigger!= undefined){this.sectionCodeFromTrigger.closePanel();}
    if (this.sectionCodeToTrigger!= undefined){this.sectionCodeToTrigger.closePanel();}
    if (this.receiptCategoryCodeFromTrigger!= undefined){this.receiptCategoryCodeFromTrigger.closePanel();}
    if (this.receiptCategoryCodeToTrigger!= undefined){this.receiptCategoryCodeToTrigger.closePanel();}
  
  }

  
  public openBankMasterModal(type: string ) {
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
        this.accountTypeIdCtrl.setValue(this.accountTypeDictionary[componentRef.instance.SelectedAccountTypeId].id);
        this.accountNumberCtrl.setValue(componentRef.instance.SelectedAccountNumber);
      }

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
              this.updateByCodeCtrl.setValue(componentRef.instance.SelectedCode);
              this.updateNameCtrl.setValue(componentRef.instance.SelectedName);
              let loginUser = componentRef.instance.SelectedObject as LoginUser;
              this.updateById = loginUser.id;
              break;
            }
          case TABLE_INDEX.MASTER_CURRENCY:
            {
              this.currencyCodeCtrl.setValue(componentRef.instance.SelectedCode);
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

  public clear() {

    this.panelOpenState = true;
    this.panel.open();

    this.MyFormGroup.reset();
    this.detailCbxExcludeFlagCtrls = null;
    this.detailExcludeCategoryIdCtrls = null;

    //this.receiptMemoCtrl.disable();
    this.excludeCategoryIdCtrl.disable();



    this.accountTypeIdCtrl.setValue(this.accountTypeDictionary[0].id);
    this.inputTypeCtrl.setValue(this.inputTypeDictionary[0].id);
    this.excludeFlagCtrl.setValue(this.excludeFlagDictionary[0].id);
    this.amountRangeCtrl.setValue(this.receiptAmountRangeDictionary[0].id);

    this.cbxFullAssignmentFlagCtrl.setValue(""); // 消込区分
    this.cbxPartAssignmentFlagCtrl.setValue(true);
    this.cbxNoAssignmentFlagCtrl.setValue(true);

    this.selectReceipt = null;
    this.updateById = null;

    /*
    this.receiptsResult = null;
    this.dispSumCount = 0;
    this.dispSumReceiptAmount = 0;
    this.dispSumRemainAmount = 0;
    */
   
    this.processCustomResult = this.processResultService.clearProcessCustomMsg(this.processCustomResult);;

    HtmlUtil.nextFocusByName(this.elementRef, 'recordedAtFromCtrl', EVENT_TYPE.NONE);

    this.setRangeCheckbox();
  }


  public setRangeCheckbox() {
    let cbxCustomer = this.localStorageManageService.get(RangeSearchKey.PD0501_CUSTOMER);
    let cbxSection = this.localStorageManageService.get(RangeSearchKey.PD0501_RECEIPT_SECTION);
    let cbxReceiptCategory = this.localStorageManageService.get(RangeSearchKey.PD0501_RECEIPT_CATEGORY);
    let cbxUseReceiptSection = this.localStorageManageService.get(RangeSearchKey.PD0501_USE_RECEIPT_SECTION);
    let cbxAmount = this.localStorageManageService.get(RangeSearchKey.PD0501_AMOUNT);

    if (cbxCustomer != null) {
      this.cbxCustomerCtrl.setValue(cbxCustomer.value);
    }

    if (cbxSection != null) {
      this.cbxSectionCtrl.setValue(cbxSection.value);
    }

    if (cbxReceiptCategory != null) {
      this.cbxReceiptCategoryCtrl.setValue(cbxReceiptCategory.value);
    }

    if (cbxUseReceiptSection != null) {
      this.cbxUseReceiptSectionCtrl.setValue(cbxUseReceiptSection.value);
    }

    if (cbxAmount != null) {
      this.cbxReceiptAmountCtrl.setValue(cbxAmount.value);
    }

  }  

  public get UpdateBulkExludeButtonDisableFlag(): boolean {

    let bRtn = true;

    if (this.detailCbxExcludeFlagCtrls != undefined && this.detailCbxExcludeFlagCtrls.length > 0) {
      for (var i = 0; i < this.detailCbxExcludeFlagCtrls.length; i++) {
        if (!!this.detailCbxExcludeFlagCtrls[i].value != !!this.receiptsResult.receipts[i].excludeFlag) {
          bRtn = false;
        }
      };
    }

    return bRtn;
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
      case BUTTON_ACTION.EXPORT:
        this.export();
        break;
      default:
        console.log('buttonAction Error.');
        break;
    }

  }

  public search() {
    this.setSearchOption();
    this.searchRequest();
  }

  public searchRequest() {

    let confirmComponentFactory = this.componentFactoryResolver.resolveComponentFactory(ModalRouterProgressComponent);
    let confirmComponentRef = this.viewContainerRef.createComponent(confirmComponentFactory);

    
    this.receiptService.GetItems(this.receiptSearch)
      .subscribe(response => {
        this.processCustomResult = this.processResultService.processAtGetData(
          this.processCustomResult, response, true, this.partsResultMessageComponent);

        if (response != PROCESS_RESULT_RESULT_TYPE.FAILURE) {

          if (response.length > 0) {
            this.receiptsResult = new ReceiptsResult();
            this.receiptsResult.receipts = response;

            this.setSearchResult();
            this.setDispFooterTotalSum();
            this.panelOpenState = false;

            if (this.paramFrom == ComponentId.PE0102) {
              this.matchingService.collationInfo.searchReceipts = this.receiptsResult.receipts;
              this.router.navigate(['main/PE0102', { process: "search", from: ComponentId.PD0501 }]);
            }

          }
          else {
            // 該当データなし
            this.receiptsResult = new ReceiptsResult();
            this.receiptsResult.receipts = response;
            this.detailCbxExcludeFlagCtrls = null;
            this.detailExcludeCategoryIdCtrls = null;
            this.panelOpenState = true;
          }

        }
        else if (response != PROCESS_RESULT_RESULT_TYPE.FAILURE && response.length === 0) {
          this.receiptsResult = new ReceiptsResult();
          this.receiptsResult.receipts = null;

          this.panelOpenState = true;

        }

        confirmComponentRef.destroy();
      });
  }

  public setSearchOption() {

    this.receiptSearch = new ReceiptSearch();
    this.receiptSearch.companyId = this.userInfoService.Company.id;
    this.receiptSearch.loginUserId = this.userInfoService.LoginUser.id;

    if(this.simplePageSearch){

      this.receiptSearch.recordedAtFrom = DateUtil.ConvertFromDatepicker(this.recordedAtFromCtrl);  // 入金日
      this.receiptSearch.recordedAtTo = DateUtil.ConvertFromDatepicker(this.recordedAtToCtrl);  // 入金日
  
      this.receiptSearch.payerName = this.payerNameCtrl.value;  // 振込依頼人名
    
      this.receiptSearch.bankCode = this.bankCodeCtrl.value;  // 銀行
      this.receiptSearch.branchCode = this.branchCodeCtrl.value;
      this.receiptSearch.accountTypeId = this.accountTypeIdCtrl.value;
      this.receiptSearch.accountNumber = this.accountNumberCtrl.value;
  
      this.receiptSearch.customerCodeFrom = this.customerCodeFromCtrl.value;  // 得意先コード
      this.receiptSearch.customerCodeTo = this.customerCodeToCtrl.value;
  
      this.receiptSearch.receiptCategoryCodeFrom = this.receiptCategoryCodeFromCtrl.value;  // 入金区分
      this.receiptSearch.receiptCategoryCodeTo = this.receiptCategoryCodeToCtrl.value;
  
      // 消込・未消込・一部消込
      // 2進数
      // 消込済み；0x004 = 4
      // 一部	；0x002 = 2
      // 未消込	；0x001 = 1
      this.receiptSearch.assignmentFlag = 0;
      if (this.cbxFullAssignmentFlagCtrl.value) {
        this.receiptSearch.assignmentFlag = this.receiptSearch.assignmentFlag + 4;
      }
      if (this.cbxPartAssignmentFlagCtrl.value) {
        this.receiptSearch.assignmentFlag = this.receiptSearch.assignmentFlag + 2;
      }
      if (this.cbxNoAssignmentFlagCtrl.value) {
        this.receiptSearch.assignmentFlag = this.receiptSearch.assignmentFlag + 1;
      }
  
      this.receiptSearch.excludeFlag = this.excludeFlagCtrl.value;  // 対象外状態
      if (this.excludeCategoryIdCtrl.value != "0") {
        this.receiptSearch.excludeCategoryId = this.excludeCategoryIdCtrl.value;  // 対象外状態
      }
  
      this.amountRangeCtrl.value;  // 金額範囲
      if (this.amountRangeCtrl.value == this.receiptAmountRangeDictionary[0].id) {
        this.receiptSearch.receiptAmountFrom = this.receiptAmountFromCtrl.value;
        this.receiptSearch.receiptAmountTo = this.receiptAmountToCtrl.value;
      }
      else if (this.amountRangeCtrl.value == this.receiptAmountRangeDictionary[1].id) {
        this.receiptSearch.remainAmountFrom = this.receiptAmountFromCtrl.value;
        this.receiptSearch.remainAmountTo = this.receiptAmountToCtrl.value;
      }
      else {
        this.receiptSearch.remainAmountFrom = 0;
        this.receiptSearch.remainAmountTo = 0;
      }

    }
    else{

      this.receiptSearch.recordedAtFrom = DateUtil.ConvertFromDatepicker(this.recordedAtFromCtrl);  // 入金日
      this.receiptSearch.recordedAtTo = DateUtil.ConvertFromDatepicker(this.recordedAtToCtrl);  // 入金日
  
      this.receiptSearch.payerName = this.payerNameCtrl.value;  // 振込依頼人名
  
      this.receiptSearch.updateAtFrom = DateUtil.ConvertFromDatepicker(this.updateAtFromCtrl);  // 最終更新日
      this.receiptSearch.updateAtTo = DateUtil.ConvertFromDatepicker(this.updateAtToCtrl);
  
      this.receiptSearch.updateBy = this.updateById;  // 最終更新者
  
      this.receiptSearch.currencyCode = this.currencyCodeCtrl.value;  // 通貨コード
  
      this.receiptSearch.bankCode = this.bankCodeCtrl.value;  // 銀行
      this.receiptSearch.branchCode = this.branchCodeCtrl.value;
      this.receiptSearch.accountTypeId = this.accountTypeIdCtrl.value;
      this.receiptSearch.accountNumber = this.accountNumberCtrl.value;
  
      this.receiptSearch.privateBankCode = this.privateBankCodeCtrl.value;  // 専用銀行
      // this.receiptSearch.privateBranchCode = this.privateBranchCodeCtrl.value;
      // this.receiptSearch.privateAccountTypeId = this.privateAccountTypeIdCtrl.value;
      // this.receiptSearch.privateAccountNumber = this.privateAccountNumberCtrl.value;
  
      this.receiptSearch.billNumber = this.billNumberCtrl.value;  // 手形券面情報
      this.receiptSearch.billBankCode = this.billBankCodeCtrl.value;
      this.receiptSearch.billBranchCode = this.billBranchCodeCtrl.value;
      this.receiptSearch.billDrawAtFrom = DateUtil.ConvertFromDatepicker(this.billDrawAtFromCtrl);
      this.receiptSearch.billDrawAtTo = DateUtil.ConvertFromDatepicker(this.billDrawAtToCtrl);
      this.receiptSearch.billDrawer = this.billDrawerCtrl.value;
  
      this.receiptSearch.customerCodeFrom = this.customerCodeFromCtrl.value;  // 得意先コード
      this.receiptSearch.customerCodeTo = this.customerCodeToCtrl.value;
  
      this.receiptSearch.sectionCodeFrom = this.sectionCodeFromCtrl.value;  // 得意先コード
      this.receiptSearch.sectionCodeTo = this.sectionCodeToCtrl.value;
  
      // メモ
      if (this.cbxMemoCtrl.value) {
        this.receiptSearch.existsMemo = 1;
      }
      else {
        this.receiptSearch.existsMemo = 0;
      }
      this.receiptSearch.receiptMemo = this.receiptMemoCtrl.value;
  
      this.receiptSearch.useSectionMaster = this.cbxUseReceiptSectionCtrl.value;  // 入金部門の使用
  
      if (this.inputTypeCtrl.value == 0) {
        //      this.receiptSearch.inputType = this.inputTypeCtrl.value;  // 入力区分
      }
      else {
        this.receiptSearch.inputType = this.inputTypeCtrl.value;  // 入力区分
      }
  
      this.receiptSearch.receiptCategoryCodeFrom = this.receiptCategoryCodeFromCtrl.value;  // 入金区分
      this.receiptSearch.receiptCategoryCodeTo = this.receiptCategoryCodeToCtrl.value;
  
      // 消込・未消込・一部消込
      // 2進数
      // 消込済み；0x004 = 4
      // 一部	；0x002 = 2
      // 未消込	；0x001 = 1
      this.receiptSearch.assignmentFlag = 0;
      if (this.cbxFullAssignmentFlagCtrl.value) {
        this.receiptSearch.assignmentFlag = this.receiptSearch.assignmentFlag + 4;
      }
      if (this.cbxPartAssignmentFlagCtrl.value) {
        this.receiptSearch.assignmentFlag = this.receiptSearch.assignmentFlag + 2;
      }
      if (this.cbxNoAssignmentFlagCtrl.value) {
        this.receiptSearch.assignmentFlag = this.receiptSearch.assignmentFlag + 1;
      }
  
  
      this.receiptSearch.excludeFlag = this.excludeFlagCtrl.value;  // 対象外状態
      if (this.excludeCategoryIdCtrl.value != "0") {
        this.receiptSearch.excludeCategoryId = this.excludeCategoryIdCtrl.value;  // 対象外状態
      }
  
      this.amountRangeCtrl.value;  // 金額範囲
      if (this.amountRangeCtrl.value == this.receiptAmountRangeDictionary[0].id) {
        this.receiptSearch.receiptAmountFrom = this.receiptAmountFromCtrl.value;
        this.receiptSearch.receiptAmountTo = this.receiptAmountToCtrl.value;
      }
      else if (this.amountRangeCtrl.value == this.receiptAmountRangeDictionary[1].id) {
        this.receiptSearch.remainAmountFrom = this.receiptAmountFromCtrl.value;
        this.receiptSearch.remainAmountTo = this.receiptAmountToCtrl.value;
      }
      else {
        this.receiptSearch.remainAmountFrom = 0;
        this.receiptSearch.remainAmountTo = 0;
      }
  
      this.receiptSearch.sourceBankName = this.sourceBankNameCtrl.value;  // 仕向
      this.receiptSearch.sourceBranchName = this.sourceBranchNameCtrl.value;
  
      this.receiptSearch.note1 = this.note1Ctrl.value;  // 備考1
      this.receiptSearch.note2 = this.note2Ctrl.value;  // 備考2
      this.receiptSearch.note3 = this.note3Ctrl.value;  // 備考3
      this.receiptSearch.note4 = this.note4Ctrl.value;  // 備考4
    }

  }

  public setSearchResult() {
    // 戻る対応のための検索結果を退避
    this.receiptService.ReceiptSearch = this.receiptSearch;
    this.receiptService.ReceiptSearchFormGroup = this.MyFormGroup;

    // 対象外の設定
    this.detailCbxExcludeFlagCtrls = new Array<FormControl>(this.receiptsResult.receipts.length);
    this.detailExcludeCategoryIdCtrls = new Array<FormControl>(this.receiptsResult.receipts.length);

    for (let index: number = 0; index < this.receiptsResult.receipts.length; index++) {

      this.detailExcludeCategoryIdCtrls[index] = new FormControl("");
      if (this.receiptsResult.receipts[index].excludeFlag == 1) {
        this.detailCbxExcludeFlagCtrls[index] = new FormControl("true");
        this.detailExcludeCategoryIdCtrls[index].enable();
        this.detailExcludeCategoryIdCtrls[index].setValue(this.receiptsResult.receipts[index].excludeCategoryId);
      }
      else {
        this.detailCbxExcludeFlagCtrls[index] = new FormControl("");
        this.detailExcludeCategoryIdCtrls[index].disable();
      }

      this.MyFormGroup.removeControl("detailCbxExcludeFlagCtrl" + index);
      this.MyFormGroup.removeControl("detailExcludeCategoryIdCtrl" + index);

      this.MyFormGroup.addControl("detailCbxExcludeFlagCtrl" + index, this.detailCbxExcludeFlagCtrls[index]);
      this.MyFormGroup.addControl("detailExcludeCategoryIdCtrl" + index, this.detailExcludeCategoryIdCtrls[index]);
    }

    // 選択行の設定
    this.selectReceipt = this.receiptsResult.receipts[0];
  }

  public setDispFooterTotalSum() {
    this.dispSumCount = this.receiptsResult.receipts.length;
    this.dispSumReceiptAmount = 0;
    this.dispSumRemainAmount = 0;

    this.receiptsResult.receipts.forEach(element => {
      this.dispSumReceiptAmount += element.receiptAmount;
      this.dispSumRemainAmount += element.remainAmount;
    });
  }

  public export() {

    let componentFactory = this.componentFactoryResolver.resolveComponentFactory(ModalRouterProgressComponent);
    let modalRouterProgressComponentRef = this.viewContainerRef.createComponent(componentFactory);
    //modalRouterProgressComponentRef.destroy();
    
    this.processCustomResult = this.processResultService.clearProcessCustomMsg(this.processCustomResult);
    let result: boolean = false;
    let data: string = "";
    let dataList = this.receiptsResult.receipts;

    let headerData = FileUtil.encloseItemBySymbol(this.gridSettingsResult.gridSettings.map(gridSetting => { return gridSetting.columnNameJp }));
    data = data + headerData.join(",") + LINE_FEED_CODE;

    for (let index = 0; index < dataList.length; index++) {
      let dataItem: Array<any> = [];

      this.gridSettingsResult.gridSettings.forEach(gridSetting => {
        switch (gridSetting.columnName) {
          case 'ExcludeFlag':
            dataItem.push(dataList[index].excludeFlag);
            break;
          case 'ExcludeCategory':
            if (dataList[index].excludeFlag == 0) {
              dataItem.push("");
            }
            else {
              //dataItem.push(dataList[index].excludeCategoryCode + ':' + dataList[index].excludeCategoryCodeName);
              dataItem.push(dataList[index].excludeCategoryName);
            }
            break;
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
            dataItem.push(this.inputTypeDictionary[dataList[index].inputType].val);
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
            //dataItem.push(DateUtil.convertDateString(dataList[index].outputAt));
            if (StringUtil.IsNullOrEmpty(dataList[index].outputAt)) {
              dataItem.push("");
            }
            else {
              dataItem.push(DateUtil.getYYYYMMDD(5,dataList[index].outputAt));
            }
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
    let resultDatas: Array<any> = [];
    resultDatas.push(data);

    FileUtil.download(resultDatas, "入金データ" + DateUtil.getYYYYMMDD(0), FILE_EXTENSION.CSV);
    result = true;
    this.processResultService.processAtOutput(
      this.processCustomResult, result, 0, this.partsResultMessageComponent);

    modalRouterProgressComponentRef.destroy();
    
  }

  public update() {

    if (this.canUpdate()) {
      this.router.navigate(['main/PD0301', { id: this.selectReceipt.id, from: ComponentId.PD0501 }]);
    }

  }

  public selectUpdateLine(lineNo: number) {

    this.selectReceipt = this.receiptsResult.receipts[lineNo];

    if (this.canUpdate()) {
      this.router.navigate(['main/PD0301', { id: this.selectReceipt.id, from: ComponentId.PD0501 }]);
    }
  }

  public openReceiptMemoModal(index: number) {
    if (this.receiptsResult.receipts[index].assignmentFlag != 0) {
      this.processCustomResult = this.processResultService.processAtWarning(
        this.processCustomResult, 
        "選択されたデータ(入金ID:" + this.receiptsResult.receipts[index].id + ")は消込済みのため修正できません。",
        this.partsResultMessageComponent);
      return;
    }

    let componentFactory = this.componentFactoryResolver.resolveComponentFactory(ModalMemoComponent);
    let componentRef = this.viewContainerRef.createComponent(componentFactory);

    let tmpReceiptMemo = new ReceiptMemo();
    tmpReceiptMemo.receiptId = this.receiptsResult.receipts[index].id;
    tmpReceiptMemo.memo = this.receiptsResult.receipts[index].receiptMemo;
    componentRef.instance.categoryType = CategoryType.Receipt;

    componentRef.instance.receiptMemo = tmpReceiptMemo;

    componentRef.instance.Closing.subscribe(() => {
      if (componentRef.instance.ModalStatus == MODAL_STATUS_TYPE.OK) {
        tmpReceiptMemo = componentRef.instance.receiptMemo;

        this.receiptService.SaveMemo(tmpReceiptMemo)
          .subscribe(response => {
            if (response == PROCESS_RESULT_RESULT_TYPE.FAILURE) {
              this.processCustomResult = this.processResultService.processAtFailure(
                this.processCustomResult, MSG_ERR.SOMETHING_ERROR.replace(MSG_ITEM_NUM.FIRST, 'メモの登録'),
                this.partsResultMessageComponent);
            }
            else {
              let index = this.receiptsResult.receipts.findIndex(receipt => {
                return receipt.id == tmpReceiptMemo.receiptId
              });
              if (index != -1) {
                this.receiptsResult.receipts[index].receiptMemo = tmpReceiptMemo.memo;
              }
            }
          });
      }
      componentRef.destroy();
    });
  }

  public canUpdate(): boolean {
    let msg = null;
    if (this.selectReceipt.excludeFlag == 1) {
      msg = "選択されたデータ(入金ID:" + this.selectReceipt.id + ")は対象外処理済みのため修正できません。";
    }

    this.receiptService.ExistOriginalReceipt(this.selectReceipt.id)
      .subscribe(response => {
        if (msg == null && response == true) {
          msg = "選択されたデータ(入金ID:" + this.selectReceipt.id + ")は前受振替済みのため修正できません。";
          this.processCustomResult = this.processResultService.processAtFailure(
            this.processCustomResult, msg, this.partsResultMessageComponent);
          return false;
        }
      });

    if (msg == null && this.selectReceipt.assignmentFlag != 0) {
      msg = "選択されたデータ(入金ID:" + this.selectReceipt.id + ")は消込済みのため修正できません。";
    }

    if (msg == null && this.selectReceipt.originalReceiptId != null) {
      msg = "選択されたデータ(入金ID:" + this.selectReceipt.id + ")は前受のため修正できません。";
    }

    if (msg == null && !StringUtil.IsNullOrEmpty(this.selectReceipt.outputAt)) {
      msg = "選択されたデータ(入金ID:" + this.selectReceipt.id + ")は仕訳出力済みのため修正できません。";
    }

    if (msg != null) {
      this.processCustomResult = this.processResultService.processAtFailure(
        this.processCustomResult, msg, this.partsResultMessageComponent);
      return false;
    }

    return true;
  }


  public savePartExclude() {

    let componentFactory = this.componentFactoryResolver.resolveComponentFactory(ModalReceiptExcludeDetailComponent);
    let componentRef = this.viewContainerRef.createComponent(componentFactory);
    componentRef.instance.SelectReceipt = this.selectReceipt;
    componentRef.instance.Closing.subscribe(() => {
      this.search();
      componentRef.destroy();
    });

  }

  /**
   * 一括対象外
   */
  public saveAllExclude() {

    if (!this.validateInputValues()){
      return;
    }

    let receiptExcludes: Array<ReceiptExclude> = new Array<ReceiptExclude>();

    for (let index = 0; index < this.detailCbxExcludeFlagCtrls.length; index++) {

      if (
        (!!this.detailCbxExcludeFlagCtrls[index].value != !!this.receiptsResult.receipts[index].excludeFlag)
        && !(this.detailCbxExcludeFlagCtrls[index].value && StringUtil.IsNullOrEmpty(this.detailExcludeCategoryIdCtrls[index].value))
      ) {
        let receiptExclude: ReceiptExclude = new ReceiptExclude();

        receiptExclude.id = 0;
        receiptExclude.receiptId = this.receiptsResult.receipts[index].id;
        receiptExclude.excludeAmount = this.detailCbxExcludeFlagCtrls[index].value ? this.receiptsResult.receipts[index].receiptAmount : 0;
        receiptExclude.excludeCategoryId = this.detailCbxExcludeFlagCtrls[index].value ? this.detailExcludeCategoryIdCtrls[index].value : null;
        //receiptExclude.outputAt
        receiptExclude.createBy = this.userInfoService.LoginUser.id;
        //receiptExclude.createAt
        receiptExclude.updateBy = this.userInfoService.LoginUser.id;
        //receiptExclude.updateAt=this.receiptsResult.receipts[index].updateAt;
        receiptExclude.excludeFlag = this.detailCbxExcludeFlagCtrls[index].value ? 1 : 0;
        // 排他制御用
        receiptExclude.receiptUpdateAt = this.receiptsResult.receipts[index].updateAt;

        receiptExcludes.push(receiptExclude);
      }

    }


    if (receiptExcludes.length > 0) {
      this.receiptService.SaveExcludeAmount(receiptExcludes)
        .subscribe(response => {
          if (response != PROCESS_RESULT_RESULT_TYPE.FAILURE) {
            this.processResultService.processAtSuccess(
              this.processCustomResult, MSG_INF.PROCESS_FINISH, this.partsResultMessageComponent);
            this.processResultService.createdLog(this.processCustomResult.logData);
            let receiptExclideResult: ReceiptExcludeResult = response;
            this.search();
          }
          else {
            this.processResultService.processAtFailure(
              this.processCustomResult, MSG_ERR.SOMETHING_ERROR.replace(MSG_ITEM_NUM.FIRST, "一括対象外処理"),
              this.partsResultMessageComponent);
          }
        });
    }

  }

  public validateInputValues(): boolean {

    for (let index = 0; index < this.detailCbxExcludeFlagCtrls.length; index++) {

      if (this.detailCbxExcludeFlagCtrls[index].value && StringUtil.IsNullOrEmpty(this.detailExcludeCategoryIdCtrls[index].value)) {
        this.processCustomResult = this.processResultService.processAtWarning(
        this.processCustomResult, MSG_WNG.SELECTION_REQUIRED.replace(MSG_ITEM_NUM.FIRST, '対象外区分'),
          this.partsResultMessageComponent);
        HtmlUtil.nextFocusByName(this.elementRef, 'detailExcludeCategoryIdCtrl' + index, EVENT_TYPE.NONE);
        return false;
      }
    }

    return true;
  }

  /**
   * 全選択
   */
  public setAllExclude() {

    this.processResultService.clearProcessCustomMsg(this.processCustomResult);

    let componentFactory = this.componentFactoryResolver.resolveComponentFactory(ModalReceiptExcludeAllComponent);
    let componentRef = this.viewContainerRef.createComponent(componentFactory);
    componentRef.instance.ExcludeCategories = this.excludeCategoriesResult.categories;
    componentRef.instance.Closing.subscribe(() => {
      if (componentRef.instance.ModalStatus == MODAL_STATUS_TYPE.SELECT) {

        for (let index = 0; index < this.detailCbxExcludeFlagCtrls.length; index++) {
          if (this.detailExcludeCategoryIdCtrls[index].disabled) {
            this.detailCbxExcludeFlagCtrls[index].setValue("true");
            this.detailExcludeCategoryIdCtrls[index].enable();
            this.detailExcludeCategoryIdCtrls[index].setValue(componentRef.instance.SelectedId);
          }
        }
      }
      else if (componentRef.instance.ModalStatus == MODAL_STATUS_TYPE.CANCEL) {
        this.cbxCheckAllCtrl.setValue(null);
        this.processCustomResult = this.processResultService.processAtWarning(
          this.processCustomResult, MSG_WNG.PROCESS_CANCELED, this.partsResultMessageComponent);
      }


      componentRef.destroy();
    });

  }

  /**
   * 全解除
   */
  public unsetAllExclude() {

    this.processResultService.clearProcessCustomMsg(this.processCustomResult);

    for (let index = 0; index < this.detailCbxExcludeFlagCtrls.length; index++) {
        this.detailCbxExcludeFlagCtrls[index].setValue("");
        this.detailExcludeCategoryIdCtrls[index].setValue("");
        this.detailExcludeCategoryIdCtrls[index].disable();
    }
  }

  public back() {
    if (this.paramFrom == ComponentId.PE0102) {
      this.router.navigate(['main/PE0102', { process: "back" }]);
    }
  }

  public selectLine(lineNo: number) {
    this.selectReceipt = this.receiptsResult.receipts[lineNo];
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
    value = EbDataHelper.removePersonalities(value, this.juridicalPersonalitiesResult.juridicalPersonalities);
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
            this.updateNameCtrl.setValue(response[0].name);
            this.updateById = response[0].id;

            HtmlUtil.nextFocusByNames(this.elementRef, ['currencyCodeCtrl', 'bankCodeCtrl'], eventType);
          }
          else {
            //this.updateByCodeCtrl.setValue("");
            this.updateNameCtrl.setValue("");
            this.updateById = null;
          }
        });
    }
    else {
      this.updateByCodeCtrl.setValue("");
      this.updateNameCtrl.setValue("");
      this.updateById = null;
      HtmlUtil.nextFocusByNames(this.elementRef, ['currencyCodeCtrl', 'bankCodeCtrl'], eventType);
    }
  }

  ///////////////////////////////////////////////////////////
  public setCurrencyCode(eventType: string) {

    if (!StringUtil.IsNullOrEmpty(this.currencyCodeCtrl.value)) {

      this.currencyService.GetItems(this.currencyCodeCtrl.value)
        .subscribe(response => {
          if (response != PROCESS_RESULT_RESULT_TYPE.FAILURE && response.length > 0) {
            this.currencyCodeCtrl.setValue(response[0].code);
            HtmlUtil.nextFocusByName(this.elementRef, 'bankCodeCtrl', eventType);
          }
          else {
            this.currencyCodeCtrl.setValue("");
            HtmlUtil.nextFocusByName(this.elementRef, 'bankCodeCtrl', eventType);
          }
        });
    }
    else {
      this.currencyCodeCtrl.setValue("");
      HtmlUtil.nextFocusByName(this.elementRef, 'bankCodeCtrl', eventType);
    }


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
    if (StringUtil.IsNullOrEmpty(this.branchCodeCtrl.value)) {
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
    }
    else {
      this.privateBankCodeCtrl.setValue(StringUtil.setPaddingFrontZero(this.privateBankCodeCtrl.value, 4, true));
    }
    HtmlUtil.nextFocusByName(this.elementRef, 'privateBranchCodeCtrl', eventType);
  }

  public setPrivateBranchCode(eventType: string) {
    if (StringUtil.IsNullOrEmpty(this.privateBranchCodeCtrl.value)) {
      this.privateBranchCodeCtrl.setValue("");
    }
    else {
      this.privateBranchCodeCtrl.setValue(StringUtil.setPaddingFrontZero(this.privateBranchCodeCtrl.value, 3, true));
    }
    HtmlUtil.nextFocusByName(this.elementRef, 'privateAccountNumberCtrl', eventType);
  }

  public setPrivateAccountNumber(eventType: string) {
    if (StringUtil.IsNullOrEmpty(this.privateAccountNumberCtrl.value)) {
      this.privateAccountNumberCtrl.setValue("");
    }
    else {
      this.privateAccountNumberCtrl.setValue(StringUtil.setPaddingFrontZero(this.privateAccountNumberCtrl.value, 7, true));
    }
    HtmlUtil.nextFocusByName(this.elementRef, 'billNumberCtrl', eventType);

  }


  ///////////////////////////////////////////////////////////////////////
  public setBillNumber(eventType: string) {
    HtmlUtil.nextFocusByName(this.elementRef, 'billBankCodeCtrl', eventType);
  }

  public setBillBankCode(eventType: string) {
    if (StringUtil.IsNullOrEmpty(this.billBankCodeCtrl.value)) {
      this.billBankCodeCtrl.setValue("");
    }
    else {
      this.billBankCodeCtrl.setValue(StringUtil.setPaddingFrontZero(this.billBankCodeCtrl.value, 4, true));
    }
    HtmlUtil.nextFocusByName(this.elementRef, 'billBranchCodeCtrl', eventType);
  }

  public setBillBranchCode(eventType: string) {
    if (StringUtil.IsNullOrEmpty(this.billBranchCodeCtrl.value)) {
      this.billBranchCodeCtrl.setValue("");
    }
    else {
      this.billBranchCodeCtrl.setValue(StringUtil.setPaddingFrontZero(this.billBranchCodeCtrl.value, 3, true));
    }
    HtmlUtil.nextFocusByName(this.elementRef, 'billDrawAtFromCtrl', eventType);
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
          }
          else {
            //this.customerCodeFromCtrl.setValue("");
            this.customerNameFromCtrl.setValue("");
            if (this.cbxCustomerCtrl.value == true) {
              this.customerCodeToCtrl.setValue(this.customerCodeFromCtrl.value);
              this.customerNameToCtrl.setValue("");
            }
          }
          HtmlUtil.nextFocusByName(this.elementRef, 'customerCodeToCtrl', eventType);
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
          }
          else {
            //this.customerCodeToCtrl.setValue("");
            this.customerNameToCtrl.setValue("");
          }
          HtmlUtil.nextFocusByNames(this.elementRef, ['sectionCodeFromCtrl', 'receiptMemoCtrl', 'receiptCategoryCodeFromCtrl'], eventType);
        });
    }
    else {
      this.customerCodeToCtrl.setValue("");
      this.customerNameToCtrl.setValue("");
      HtmlUtil.nextFocusByNames(this.elementRef, ['sectionCodeFromCtrl', 'receiptMemoCtrl', 'receiptCategoryCodeFromCtrl'], eventType);
    }
  }

  public setCbxCustomer(eventType: string) {
    let localstorageItem = new LocalStorageItem();
    localstorageItem.key = RangeSearchKey.PD0501_CUSTOMER;
    localstorageItem.value = this.cbxCustomerCtrl.value;
    this.localStorageManageService.set(localstorageItem);

    HtmlUtil.nextFocusByName(this.elementRef, "customerCodeFromCtrl", eventType);

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
          if (response != PROCESS_RESULT_RESULT_TYPE.FAILURE && response.length > 0) {
            this.sectionCodeFromCtrl.setValue(response[0].code);
            this.sectionNameFromCtrl.setValue(response[0].name);
            if (this.cbxSectionCtrl.value == true) {
              this.sectionCodeToCtrl.setValue(response[0].code);
              this.sectionNameToCtrl.setValue(response[0].name);
            }
          }
          else {
            //this.sectionCodeFromCtrl.setValue("");
            this.customerNameFromCtrl.setValue("");
            if (this.cbxSectionCtrl.value == true) {
              this.sectionCodeToCtrl.setValue(this.sectionCodeFromCtrl.value);
              this.sectionNameToCtrl.setValue("");
            }
          }
          HtmlUtil.nextFocusByName(this.elementRef, 'sectionCodeToCtrl', eventType);
        });
    }
    else {
      this.sectionCodeFromCtrl.setValue("");
      this.sectionNameFromCtrl.setValue("");
      HtmlUtil.nextFocusByName(this.elementRef, 'sectionCodeToCtrl', eventType);
    }

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
          if (response != PROCESS_RESULT_RESULT_TYPE.FAILURE && response.length > 0) {
            this.sectionCodeToCtrl.setValue(response[0].code);
            this.sectionNameToCtrl.setValue(response[0].name);
          }
          else {
            //this.sectionCodeToCtrl.setValue("");
            this.sectionNameToCtrl.setValue("");
          }
          HtmlUtil.nextFocusByName(this.elementRef, 'receiptMemoCtrl', eventType);
        });
    }
    else {
      this.sectionCodeToCtrl.setValue("");
      this.sectionNameToCtrl.setValue("");
      HtmlUtil.nextFocusByName(this.elementRef, 'receiptMemoCtrl', eventType);
    }
  }

  public setCbxSection(eventType: string) {
    let localstorageItem = new LocalStorageItem();
    localstorageItem.key = RangeSearchKey.PD0501_RECEIPT_SECTION;
    localstorageItem.value = this.cbxCustomerCtrl.value;
    this.localStorageManageService.set(localstorageItem);

    HtmlUtil.nextFocusByName(this.elementRef, "sectionCodeFromCtrl", eventType);
  }  




  ///////////////////////////////////////////////////////////
  public setCbxMemo(eventType: string) {
    if (this.cbxMemoCtrl.value === true) {
      //this.receiptMemoCtrl.enable();
    }
    else {
      // this.receiptMemoCtrl.setValue("");
      // this.receiptMemoCtrl.disable();
    }
    HtmlUtil.nextFocusByName(this.elementRef, 'receiptMemoCtrl', eventType);
  }

  

  ///////////////////////////////////////////////////////////
  public setCbxUseReceiptSection(eventType: string) {
    let localstorageItem = new LocalStorageItem();
    localstorageItem.key = RangeSearchKey.PD0501_USE_RECEIPT_SECTION;
    localstorageItem.value = this.cbxUseReceiptSectionCtrl.value;
    this.localStorageManageService.set(localstorageItem);

  }    


  ///////////////////////////////////////////////////////////
  public setReceiptMemo(eventType: string) {
    HtmlUtil.nextFocusByName(this.elementRef, 'inputTypeCtrl', eventType);
  }

  ///////////////////////////////////////////////////////////

  public setReceiptCategoryCodeFrom(eventType: string) {

    if(eventType!=EVENT_TYPE.BLUR){
      this.receiptCategoryCodeFromTrigger.closePanel();
    }

    if (!StringUtil.IsNullOrEmpty(this.receiptCategoryCodeFromCtrl.value)) {

      this.receiptCategoryCodeFromCtrl.setValue(StringUtil.setPaddingFrontZero(this.receiptCategoryCodeFromCtrl.value, 2));

      this.loadStart();
      this.categoryService.GetItems(CategoryType.Receipt, this.receiptCategoryCodeFromCtrl.value)
        .subscribe(response => {
          this.loadEnd();
          if (response != PROCESS_RESULT_RESULT_TYPE.FAILURE && response.length > 0) {

            this.receiptCategoryCodeFromCtrl.setValue(response[0].code);
            this.receiptCategoryNameFromCtrl.setValue(response[0].name);
            if (this.cbxReceiptCategoryCtrl.value === true) {
              this.receiptCategoryCodeToCtrl.setValue(response[0].code);
              this.receiptCategoryNameToCtrl.setValue(response[0].name);
            }
          }
          else {
            //this.receiptCategoryCodeFromCtrl.setValue("");
            this.receiptCategoryNameFromCtrl.setValue("");
            if (this.cbxReceiptCategoryCtrl.value === true) {
              this.receiptCategoryCodeToCtrl.setValue(this.receiptCategoryCodeFromCtrl.value);
              this.receiptCategoryNameToCtrl.setValue("");
            }
          }
          HtmlUtil.nextFocusByName(this.elementRef, 'receiptCategoryCodeToCtrl', eventType);
        });

    }
    else {
      this.receiptCategoryCodeFromCtrl.setValue("");
      this.receiptCategoryNameFromCtrl.setValue("");
      HtmlUtil.nextFocusByName(this.elementRef, 'receiptCategoryCodeToCtrl', eventType);
    }

  }

  public setReceiptCategoryCodeTo(eventType: string) {

    if(eventType!=EVENT_TYPE.BLUR){
      this.receiptCategoryCodeToTrigger.closePanel();
    }

    if (!StringUtil.IsNullOrEmpty(this.receiptCategoryCodeToCtrl.value)) {

      this.receiptCategoryCodeToCtrl.setValue(StringUtil.setPaddingFrontZero(this.receiptCategoryCodeToCtrl.value, 2));

      this.loadStart();
      this.categoryService.GetItems(CategoryType.Receipt, this.receiptCategoryCodeToCtrl.value)
        .subscribe(response => {
          this.loadEnd();
          if (response != PROCESS_RESULT_RESULT_TYPE.FAILURE && response.length > 0) {

            this.receiptCategoryCodeToCtrl.setValue(response[0].code);
            this.receiptCategoryNameToCtrl.setValue(response[0].name);
          }
          else {
            //this.receiptCategoryCodeToCtrl.setValue("");
            this.receiptCategoryNameToCtrl.setValue("");
          }
          HtmlUtil.nextFocusByName(this.elementRef, 'cbxFullAssignmentFlagCtrl', eventType);
        });

    }
    else {
      this.receiptCategoryCodeToCtrl.setValue("");
      this.receiptCategoryNameToCtrl.setValue("");
      HtmlUtil.nextFocusByName(this.elementRef, 'cbxFullAssignmentFlagCtrl', eventType);
    }

  }

  public setCbxReceiptCategory(eventType: string) {  
    let localstorageItem = new LocalStorageItem();
    localstorageItem.key = RangeSearchKey.PD0501_RECEIPT_CATEGORY;
    localstorageItem.value = this.cbxReceiptCategoryCtrl.value;
    this.localStorageManageService.set(localstorageItem);

    HtmlUtil.nextFocusByName(this.elementRef, 'receiptCategoryCodeFromCtrl', eventType);

  }    

  
  ///////////////////////////////////////////////////////////
  public isHiddenExcludeFlag(index: number) {
    if (
      (this.receiptsResult.receipts[index].remainAmount + this.receiptsResult.receipts[index].excludeAmount) != this.receiptsResult.receipts[index].receiptAmount
      || this.receiptsResult.receipts[index].recExcOutputAt == 1
    ) {
      return true;
    }
    else {
      return false;
    }
  }

  public setExcludeFlag(eventType: string) {

    if (this.excludeFlagCtrl.value === 1) {
      this.excludeCategoryIdCtrl.enable();
      this.excludeCategoryIdCtrl.setValue("0");
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
    localstorageItem.key = RangeSearchKey.PD0501_AMOUNT;
    localstorageItem.value = this.cbxReceiptCategoryCtrl.value;
    this.localStorageManageService.set(localstorageItem);

    HtmlUtil.nextFocusByName(this.elementRef, 'receiptAmountFromCtrl', eventType);
    
  }      

  ///////////////////////////////////////////////////////////
  public setSourceBankName(eventType: string) {
    HtmlUtil.nextFocusByName(this.elementRef, 'sourceBranchNameCtrl', eventType);
  }

  public inputSourceBankName() {
    this.sourceBankNameCtrl.setValue(EbDataHelper.convertToValidEbkana(this.sourceBankNameCtrl.value));
  }

  public setSourceBranchName(eventType: string) {
    HtmlUtil.nextFocusByName(this.elementRef, 'note1Ctrl', eventType);
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
    HtmlUtil.nextFocusByName(this.elementRef, 'recordedAtFromCtrl', eventType);
  }

  public setDetailCbxExcludeFlag(eventType: string, index: number) {

    if (
      this.receiptsResult.receipts[index].assignmentFlag == 0
      || this.receiptsResult.receipts[index].excludeFlag > 0
    ) {
      if (this.detailCbxExcludeFlagCtrls[index].value === true) {
        this.detailExcludeCategoryIdCtrls[index].enable();
      }
      else {
        this.detailExcludeCategoryIdCtrls[index].setValue("");
        this.detailExcludeCategoryIdCtrls[index].disable();
      }
    }

  }

  public disableBack(): boolean {

    // 個別消込から遷移した場合
    if (this.paramFrom == ComponentId.PE0102) return false;

    return true;

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
    
  }
    
  public isFixedColumn(setting: GridSetting): boolean {
    return (
                setting.columnName === 'ExcludeFlag' 
            ||  setting.columnName === 'ExcludeCategory'
            || setting.columnName === 'ExcludeAmount'
            ||  setting.columnName === 'Memo'
          );
  }

  public setCbxSectino(eventType: string) {
    let localstorageItem = new LocalStorageItem();
    localstorageItem.key = RangeSearchKey.PD0501_RECEIPT_SECTION;
    localstorageItem.value = this.cbxSectionCtrl.value;
    this.localStorageManageService.set(localstorageItem);

    HtmlUtil.nextFocusByName(this.elementRef, "sectionCodeFromCtrl", eventType);
  }

  public checkAll() {

    if(this.cbxCheckAllCtrl.value){
      this.setAllExclude();
    }
    else{
      this.unsetAllExclude();
    }

    this.processResultService.clearProcessCustomMsg(this.processCustomResult);
  }
}
