import { Component, OnInit, EventEmitter, ComponentFactoryResolver, ViewContainerRef, ElementRef, AfterViewInit, ViewChild } from '@angular/core';
import { Customer } from 'src/app/model/customer.model';

import { FormGroup, FormControl, Validators } from '@angular/forms';
import { CustomValidators } from 'ng5-validation';
import { BaseComponent } from '../../common/base/base-component';
import { CustomerMasterService } from 'src/app/service/Master/customer-master.service';
import { ModalMasterComponent } from '../modal-master/modal-master.component';
import { HtmlUtil } from 'src/app/common/util/html.util';
import { COMPONENT_STATUS_TYPE, COMPONENT_STATUS_NAME, MODAL_STATUS_TYPE, PROCESS_RESULT_RESULT_TYPE, PROCESS_RESULT_STATUS_TYPE } from 'src/app/common/const/status.const';
import { FormatterUtil, FormatStyle } from 'src/app/common/util/formatter.util';
import { SHARE_TRANSFER_FEE_DICTIONARY, FRACTIONAL_UNIT, EXCLUSIVE_ACCOUNT_TYPE, TRANSFER_ACCOUNT_TYPE, CategoryType, CODE_TYPE } from 'src/app/common/const/kbn.const';
import { CategoryMasterService } from 'src/app/service/Master/category-master.service';
import { CategoriesResult } from 'src/app/model/categories-result.model';
import { Category } from 'src/app/model/category.model';
import { UserInfoService } from 'src/app/service/common/user-info.service';
import { TABLE_INDEX, TABLES } from 'src/app/common/const/table-name.const';
import { KEY_CODE, BUTTON_ACTION, EVENT_TYPE } from 'src/app/common/const/event.const';
import { CustomerPaymentContractMasterService } from 'src/app/service/Master/customer-payment-contract-master.service';
import { CustomerPaymentContract } from 'src/app/model/customer-payment-contract.model';
import { StringUtil } from 'src/app/common/util/string-util';
import { CompanyMasterService } from 'src/app/service/Master/company-master.service';
import { CompanyResult } from 'src/app/model/company-result.model';
import { forkJoin } from 'rxjs';
import { Company } from 'src/app/model/company.model';
import { CustomerGroupMasterService } from 'src/app/service/Master/customer-group-master.service';
import { BillingService } from 'src/app/service/billing.service';
import { KanaHistoryCustomerMasterService } from 'src/app/service/Master/kana-history-customer-master.service';
import { ReceiptService } from 'src/app/service/receipt.service';
import { NettingMasterService } from 'src/app/service/Master/netting-master.service';
import { KanaHistoryCustomer } from 'src/app/model/kana-history-customer.model';
import { ModalConfirmComponent } from '../modal-confirm/modal-confirm.component';
import { FileUtil } from 'src/app/common/util/file.util';
import { ModalTransferFeeComponent } from '../modal-transfer-fee/modal-transfer-fee.component';
import { ExistDeleteResult } from 'src/app/model/custom-model/process-result-custom.model';
import { MSG_WNG, MSG_ERR, MSG_ITEM_NUM } from 'src/app/common/const/message.const';
import { JuridicalPersonalitysResult } from 'src/app/model/juridical-personalitys-result.model';
import { JuridicalPersonalityMasterService } from 'src/app/service/Master/juridical-personality-master.service';
import { EbDataHelper } from 'src/app/common/util/eb-data-helper';
import { ProcessResultService } from 'src/app/service/common/process-result.service';
import { ComponentId } from 'src/app/common/const/component-name.const';
import { StaffMasterService } from 'src/app/service/Master/staff-master.service';
import { NumberUtil } from 'src/app/common/util/number-util';
import { ModalRouterProgressComponent } from '../modal-router-progress/modal-router-progress.component';
import { MatAutocompleteTrigger } from '@angular/material';
import { element } from '@angular/core/src/render3';


@Component({
  selector: 'app-modal-customer-detail',
  templateUrl: './modal-customer-detail.component.html',
  styleUrls: ['./modal-customer-detail.component.css']
})
export class ModalCustomerDetailComponent extends BaseComponent implements OnInit,AfterViewInit {

  public readonly shareTransferFeeDictionary = SHARE_TRANSFER_FEE_DICTIONARY;
  public readonly fractionalUnit = FRACTIONAL_UNIT;
  public readonly exclusiveAccountType = EXCLUSIVE_ACCOUNT_TYPE;
  public readonly transferAccountType = TRANSFER_ACCOUNT_TYPE;

  public categoryResult: CategoriesResult = new CategoriesResult();
  public lessThanCollectCategoryResult: CategoriesResult = new CategoriesResult();
  public greaterThanCollectCategoryResult: CategoriesResult = new CategoriesResult();
  public companyResult: CompanyResult = new CompanyResult();
  public juridicalPersonalitysResult: JuridicalPersonalitysResult;
  public isRegistrationFeeButton: boolean =  true;
  public isDeleteButton: boolean =  false;

  public collectOffsetMonthLabel1: string = "ヶ月後の";
  public collectOffsetMonthLabel2: string = "※末尾(28日以降)=99";
  public collectOffsetDayLabel: string = "日";

  public collectCategoryCode: string;
  public useLimitDate1: number;
  public useLimitDate2: number;
  public useLimitDate3: number;

  public fromPageId: number;

  @ViewChild('staffCodeInput', { read: MatAutocompleteTrigger }) staffCodeTrigger: MatAutocompleteTrigger;


  public closing = new EventEmitter<{}>();
  public get Closing() {
    return this.closing;
  }
  public set Closing(value) {
    this.closing = value;
  }

  public customer: Customer = new Customer();
  public get Customer(): Customer {
    return this.customer;
  }
  public set Customer(value: Customer) {
    this.customer = value;
  }

  public get ProcessModalCustomResult() {
    return this.processModalCustomResult;
  }
  public set ProcessModalCustomResult(value) {
    this.processModalCustomResult = value;
  }

  public get FromPageId() {
    return this.fromPageId;
  }
  public set FromPageId(value) {
    this.fromPageId = value;
  }

  /* 基本設定 */
  public customerCodeCtrl: FormControl;        // 得意先コード
  public cbxCustomerIsParentCtrl: FormControl; // 債権代表者
  public cbxUseKanaLearningCtrl: FormControl;  // カナ自動学習
  public cbxUseFeeLearningCtrl: FormControl;   // 手数料自動学習
  public cbxUseFeeToleranceCtrl: FormControl;  // 手数料誤差利用
  public cbxPrioritizeMatchingIndividuallyCtrl: FormControl; // 一括消込対象外
  public cbxExcludeInvoicePublishCtrl: FormControl;  // 請求書発行対象外
  public cbxExcludeReminderPublishCtrl: FormControl; // 督促状発行対象外
  public customerNameCtrl: FormControl;    // 得意先名
  public customerKanaCtrl: FormControl;    // 得意先名カナ
  public parentCustomerCodeCtrl: FormControl;  // 債権代表者コード
  public parentCustomerNameCtrl: FormControl;  // 債権代表者名
  public cmbShareTransferFeeCtrl: FormControl; // 手数料負担区分
  public collationKeyCtrl: FormControl;    // 照合番号
  public staffCodeCtrl: FormControl;       // 営業担当者コード
  public staffNameCtrl: FormControl;       // 営業担当者名

  /* 日付設定 */
  public closingDayCtrl: FormControl;          // 締め日
  public cbxIssueBillEachTimeCtrl: FormControl;// 都度請求
  public collectOffsetMonthCtrl: FormControl;  // 回収予定月
  public collectOffsetDayCtrl: FormControl;    // 回収予定日
  public cmbHolidayFlagCtrl: FormControl;      // 休業日設定

  /* 回収設定 */
  public cmbCollectCategoryIdCtrl: FormControl;// 回収方法
  public sightOfBillCtrl: FormControl;         // 回収サイト
  public thresholdValueCtrl: FormControl;      // 約定金額
  public cmbLessThanCollectCategoryIdCtrl: FormControl;      // 約定金額未満

  public cmbGreaterThanCollectCategoryId1Ctrl: FormControl;  // 約定金額以上⓵
  public greaterThanRate1Ctrl: FormControl;    // 分割⓵
  public cmbGreaterThanRoundingMode1Ctrl: FormControl;       // 端数単位⓵
  public greaterThanSightOfBill1Ctrl: FormControl;           // 回収サイト⓵

  public cmbGreaterThanCollectCategoryId2Ctrl: FormControl;  // 約定金額以上⓶
  public greaterThanRate2Ctrl: FormControl;    // 分割⓶
  public cmbGreaterThanRoundingMode2Ctrl: FormControl;       // 端数単位⓶
  public greaterThanSightOfBill2Ctrl: FormControl;           // 回収サイト⓶

  public cmbGreaterThanCollectCategoryId3Ctrl: FormControl;  // 約定金額以上⓷
  public greaterThanRate3Ctrl: FormControl;    // 分割⓷
  public cmbGreaterThanRoundingMode3Ctrl: FormControl;       // 端数単位⓷
  public greaterThanSightOfBill3Ctrl: FormControl;           // 回収サイト⓷

  /* 債権管理 */
  public densaiCodeCtrl: FormControl;  // 電子手形用企業コード
  public creditCodeCtrl: FormControl;  // 信用調査用企業コード
  public creditLimitCtrl: FormControl; // 与信限度額
  public creditRankCtrl: FormControl;  // 与信ランク

  /* 専用入金口座（仮想口座） */
  public exclusiveBankCodeCtrl: FormControl;   // 専用入金口座・銀行コード
  public exclusiveBankNameCtrl: FormControl;		// 専用入金口座・銀行名
  public exclusiveBranchCodeCtrl: FormControl; // 専用入金口座・支店コード
  public exclusiveVirtualBranchCodeCtrl: FormControl;    // 専用入金口座・仮想支店コード
  public exclusiveVirtualBranchNameCtrl: FormControl;		// 専用入金口座・仮想支店名
  public exclusiveAccountNumberCtrl: FormControl;		    // 専用入金口座・口座番号
  public cmbExclusiveAccountTypeIdCtrl: FormControl;		  // 専用入金口座・預金種別

  /* 口座振替用口座 */
  public transferBankCodeCtrl: FormControl;		// 口座振替用口座・銀行コード
  public transferBankNameCtrl: FormControl;		// 口座振替用口座・銀行名
  public transferBrachCodeCtrl: FormControl;		// 口座振替用口座・支店コード
  public transferBranchNameCtrl: FormControl;	// 口座振替用口座・支店名
  public transferAccountNumberCtrl: FormControl;		      // 口座振替用口座・口座番号
  public transferAccountTypeIdCtrl: FormControl;		      // 口座振替用口座・預金種別
  public transferNewCodeCtrl: FormControl;		  // 口座振替用口座・新規コード
  public transferAccountNameCtrl: FormControl;	          // 口座振替用口座・預金者名
  public transferCustomerCodeCtrl: FormControl;          // 口座振替用口座・顧客コード

  /* 住所 */
  public postalCodeCtrl: FormControl;  // 郵便番号
  public address1Ctrl: FormControl;    // 住所１
  public address2Ctrl: FormControl;		// 住所２
  public telCtrl: FormControl;		      // 電話番号
  public faxCtrl: FormControl;		      // FAX番号
  public destinationDepartmentNameCtrl: FormControl;		  // 相手先部署
  public customerStaffNameCtrl: FormControl;		// 相手先担当者名
  public honorificCtrl: FormControl;		// 敬称
  public noteCtrl: FormControl;		    // 備考

  /* 被振込口座設定 */
  public cbxReceiveAccountId1Ctrl: FormControl;    // 被振込口座設定１
  public cbxReceiveAccountId2Ctrl: FormControl;		// 被振込口座設定２
  public cbxReceiveAccountId3Ctrl: FormControl;		// 被振込口座設定３

  /* 歩引設定 */
  public minValueCtrl: FormControl; // 歩引・最低実行金額
  public rate1Ctrl: FormControl; // 歩引・歩引率１
  public cmbRoundingMode1Ctrl: FormControl; // 歩引・端数処理１
  public departmentId1Ctrl: FormControl; // 歩引・部門コード１
  public departmentName1Ctrl: FormControl; // 歩引・部門コード１
  public accountTitleId1Ctrl: FormControl; // 歩引・科目コード１
  public accountTitleName1Ctrl: FormControl; // 歩引・科目コード１
  public subCode1Ctrl: FormControl; // 歩引・補助コード１
  public rate2Ctrl: FormControl; // 歩引・歩引率２
  public cmbRoundingMode2Ctrl: FormControl; // 歩引・端数処理２
  public departmentId2Ctrl: FormControl; // 歩引・部門コード２
  public departmentName2Ctrl: FormControl; // 歩引・部門コード２
  public accountTitleId2Ctrl: FormControl; // 歩引・科目コード２
  public accountTitleName2Ctrl: FormControl; // 歩引・科目コード２
  public subCode2Ctrl: FormControl; // 歩引・補助コード２
  public rate3Ctrl: FormControl; // 歩引・歩引率３
  public cmbRoundingMode3Ctrl: FormControl; // 歩引・端数処理３
  public departmentId3Ctrl: FormControl; // 歩引・部門コード３
  public departmentName3Ctrl: FormControl; // 歩引・部門コード３
  public accountTitleId3Ctrl: FormControl; // 歩引・科目コード３
  public accountTitleName3Ctrl: FormControl; // 歩引・科目コード３
  public subCode3Ctrl: FormControl; // 歩引・補助コード３
  public rate4Ctrl: FormControl; // 歩引・歩引率４
  public cmbRoundingMode4Ctrl: FormControl; // 歩引・端数処理４
  public departmentId4Ctrl: FormControl; // 歩引・部門コード４
  public departmentName4Ctrl: FormControl; // 歩引・部門コード４
  public accountTitleId4Ctrl: FormControl; // 歩引・科目コード４
  public accountTitleName4Ctrl: FormControl; // 歩引・科目コード４
  public subCode4Ctrl: FormControl; // 歩引・補助コード４
  public rate5Ctrl: FormControl; // 歩引・歩引率５
  public cmbRoundingMode5Ctrl: FormControl; // 歩引・端数処理５
  public departmentId5Ctrl: FormControl; // 歩引・部門コード５
  public departmentName5Ctrl: FormControl; // 歩引・部門コード５
  public accountTitleId5Ctrl: FormControl; // 歩引・科目コード５
  public accountTitleName5Ctrl: FormControl; // 歩引・科目コード５
  public subCode5Ctrl: FormControl; // 歩引・補助コード５

  public UndefineCtrl: FormControl;

  public staffId: number;
  public isCustomerPaymentContractRegistry: boolean = true;

  public bankName1Ctrl: FormControl;
  public bankName2Ctrl: FormControl;
  public bankName3Ctrl: FormControl;
  public branchName1Ctrl: FormControl;
  public branchName2Ctrl: FormControl;
  public branchName3Ctrl: FormControl;
  public accountType1Ctrl: FormControl;
  public accountType2Ctrl: FormControl;
  public accountType3Ctrl: FormControl;
  public accountNumber1Ctrl: FormControl;
  public accountNumber2Ctrl: FormControl;
  public accountNumber3Ctrl: FormControl;

  constructor(
    public elementRef: ElementRef,
    public componentFactoryResolver: ComponentFactoryResolver,
    public viewContainerRef: ViewContainerRef,
    public userInfoService: UserInfoService,
    public categoryService: CategoryMasterService,
    public customerService: CustomerMasterService,
    public customerPaymentContractService: CustomerPaymentContractMasterService,
    public companyService: CompanyMasterService,
    public customerGroupService: CustomerGroupMasterService,
    public billingServide: BillingService,
    public kanaHistoryCustomerMaster: KanaHistoryCustomerMasterService,
    public receiptService: ReceiptService,
    public nettingService: NettingMasterService,
    public juridicalPersonalityService: JuridicalPersonalityMasterService,
    public processResultService: ProcessResultService,
    public staffService:StaffMasterService,

  ) {
    super();
  }

  ngOnInit() {
    // 入力チェックの設定
    this.setControlInit();
    this.setValidator();
    this.setFormatter();
    this.clear();
    this.setForm();
    this.setAutoComplete();

    this.Title = "得意先　【" + COMPONENT_STATUS_NAME[this.ComponentStatus] + "】";

    // 各データ取得
    let getCategoriesResponse = this.categoryService.GetItemsByCategoryType(CategoryType.Collection);
    let getCompanyResponse = this.companyService.GetItems(this.userInfoService.Company.code);
    let getJuridicalPersonalitysResponse = this.juridicalPersonalityService.GetItems();
    let getHasChildResponse = null;
    if (this.customer.id != undefined) {
      getHasChildResponse = this.customerGroupService.HasChild(this.customer.id);
    } else {
      getHasChildResponse = this.customerGroupService.HasChild(0);
    }

    this.categoryResult.categories = new Array<Category>();
    this.lessThanCollectCategoryResult.categories = new Array<Category>();
    this.greaterThanCollectCategoryResult.categories = new Array<Category>();
    this.companyResult.company = new Company();
    this.juridicalPersonalitysResult = new JuridicalPersonalitysResult();

    // 3つの処理の待機
    forkJoin(
      getCategoriesResponse,
      getCompanyResponse,
      getJuridicalPersonalitysResponse,
      getHasChildResponse
    ).subscribe(responseList => {
      if (responseList.length != 4 || responseList.indexOf(undefined) >= 0) {
        this.processCustomResult = this.processResultService.processAtFailure(
          this.processCustomResult, MSG_ERR.DATA_SEARCH, this.partsResultMessageComponent);

        return;
      }

      // 債権代表者フラグ制御
      if (this.customer.id != undefined && responseList[3]) {
        this.cbxCustomerIsParentCtrl.disable();
      }

      let categories = responseList[0];
      this.companyResult.company = responseList[1];
      this.juridicalPersonalitysResult.juridicalPersonalities = responseList[2];

      // カテゴリ
      this.categoryResult.categories = categories;
      for (let i = 0; i < categories.length; i++) {
        this.lessThanCollectCategoryResult.categories.push(categories[i]);
        this.greaterThanCollectCategoryResult.categories.push(categories[i]);

        if (categories[i].name == '約定') {
          this.lessThanCollectCategoryResult.categories.pop();
          this.greaterThanCollectCategoryResult.categories.pop();
        }

        if (categories[i].name == '手形' || categories[i].name == '期日現金') {
          this.lessThanCollectCategoryResult.categories.pop();
        }
      }

      // 回収方法の値に合わせて入力欄を制御する
      this.setCollectCategoryId();

      // 被口座情報
      this.bankName1Ctrl.setValue(this.companyResult.company[0].bankName1);
      this.bankName2Ctrl.setValue(this.companyResult.company[0].bankName2);
      this.bankName3Ctrl.setValue(this.companyResult.company[0].bankName3);
      this.branchName1Ctrl.setValue(this.companyResult.company[0].branchName1);
      this.branchName2Ctrl.setValue(this.companyResult.company[0].branchName2);
      this.branchName3Ctrl.setValue(this.companyResult.company[0].branchName3);
      this.accountType1Ctrl.setValue(this.companyResult.company[0].accountType1);
      this.accountType2Ctrl.setValue(this.companyResult.company[0].accountType2);
      this.accountType3Ctrl.setValue(this.companyResult.company[0].accountType3);
      this.accountNumber1Ctrl.setValue(this.companyResult.company[0].accountNumber1);
      this.accountNumber2Ctrl.setValue(this.companyResult.company[0].accountNumber2);
      this.accountNumber3Ctrl.setValue(this.companyResult.company[0].accountNumber3);

    },
      error => {
        console.log(error)
      }
    );
  }

  ngAfterViewInit(){
    //HtmlUtil.nextFocusByName(this.elementRef, 'customerNameCtrl', EVENT_TYPE.NONE);
  }
  
  public setControlInit() {
    // 基本設定
    this.customerCodeCtrl = new FormControl("", [Validators.required, Validators.maxLength(this.userInfoService.ApplicationControl.customerCodeLength)]);
    this.cbxCustomerIsParentCtrl = new FormControl();
    this.cbxUseKanaLearningCtrl = new FormControl();
    this.cbxUseFeeLearningCtrl = new FormControl();
    this.cbxUseFeeToleranceCtrl = new FormControl();
    this.cbxPrioritizeMatchingIndividuallyCtrl = new FormControl();
    this.cbxExcludeInvoicePublishCtrl = new FormControl();
    this.cbxExcludeReminderPublishCtrl = new FormControl();
    this.customerNameCtrl = new FormControl("", [Validators.required]);
    this.customerKanaCtrl = new FormControl("", [Validators.required]);
    this.parentCustomerCodeCtrl = new FormControl();
    this.parentCustomerNameCtrl = new FormControl();
    this.cmbShareTransferFeeCtrl = new FormControl("", [Validators.required]);
    this.collationKeyCtrl = new FormControl();
    this.staffCodeCtrl = new FormControl("", [Validators.required, Validators.maxLength(this.userInfoService.ApplicationControl.staffCodeLength)]);
    this.staffNameCtrl = new FormControl();

    // 日程設定
    this.closingDayCtrl = new FormControl("", [Validators.required, CustomValidators.number]);
    this.cbxIssueBillEachTimeCtrl = new FormControl();
    this.collectOffsetMonthCtrl = new FormControl("", [Validators.required, CustomValidators.number]);
    this.collectOffsetDayCtrl = new FormControl("", [Validators.required, CustomValidators.number]);
    this.cmbHolidayFlagCtrl = new FormControl();

    // 回収設定
    this.cmbCollectCategoryIdCtrl = new FormControl("", [Validators.required]);
    this.sightOfBillCtrl = new FormControl();
    this.thresholdValueCtrl = new FormControl();
    this.cmbLessThanCollectCategoryIdCtrl = new FormControl();
    this.cmbGreaterThanCollectCategoryId1Ctrl = new FormControl();
    this.greaterThanRate1Ctrl = new FormControl();
    this.cmbGreaterThanRoundingMode1Ctrl = new FormControl();
    this.greaterThanSightOfBill1Ctrl = new FormControl();
    this.cmbGreaterThanCollectCategoryId2Ctrl = new FormControl();
    this.greaterThanRate2Ctrl = new FormControl();
    this.cmbGreaterThanRoundingMode2Ctrl = new FormControl();
    this.greaterThanSightOfBill2Ctrl = new FormControl();
    this.cmbGreaterThanCollectCategoryId3Ctrl = new FormControl();
    this.greaterThanRate3Ctrl = new FormControl();
    this.cmbGreaterThanRoundingMode3Ctrl = new FormControl();
    this.greaterThanSightOfBill3Ctrl = new FormControl();

    // 債権管理
    this.densaiCodeCtrl = new FormControl();
    this.creditCodeCtrl = new FormControl();
    this.creditLimitCtrl = new FormControl();
    this.creditRankCtrl = new FormControl();

    // 専用入金口座（仮想口座）
    this.exclusiveBankCodeCtrl = new FormControl("", [CustomValidators.number]);
    this.exclusiveBankNameCtrl = new FormControl("");
    this.exclusiveBranchCodeCtrl = new FormControl("", [CustomValidators.number]);
    this.exclusiveVirtualBranchCodeCtrl = new FormControl("", [CustomValidators.number]);
    this.exclusiveVirtualBranchNameCtrl = new FormControl("");
    this.exclusiveAccountNumberCtrl = new FormControl("", [CustomValidators.number]);
    this.cmbExclusiveAccountTypeIdCtrl = new FormControl("");

    // 口座振替用口座
    this.transferBankCodeCtrl = new FormControl("", [CustomValidators.number]);
    this.transferBankNameCtrl = new FormControl("");
    this.transferBrachCodeCtrl = new FormControl("", [CustomValidators.number]);
    this.transferBranchNameCtrl = new FormControl("");
    this.transferAccountNumberCtrl = new FormControl("", [CustomValidators.number]);
    this.transferAccountTypeIdCtrl = new FormControl("");
    this.transferNewCodeCtrl = new FormControl("", [CustomValidators.number]);
    this.transferAccountNameCtrl = new FormControl("");
    this.transferCustomerCodeCtrl = new FormControl("");

    // 住所
    this.postalCodeCtrl = new FormControl("", [Validators.pattern(new RegExp(FormatStyle.ZIP))]);
    this.address1Ctrl = new FormControl("");
    this.address2Ctrl = new FormControl("");
    // this.telCtrl = new FormControl("", [Validators.pattern(new RegExp(FormatStyle.TEL))]);
    // this.faxCtrl = new FormControl("", [Validators.pattern(new RegExp(FormatStyle.TEL))]);
    this.telCtrl = new FormControl("");
    this.faxCtrl = new FormControl("");
    this.destinationDepartmentNameCtrl = new FormControl("");
    this.customerStaffNameCtrl = new FormControl("");
    this.honorificCtrl = new FormControl("");
    this.noteCtrl = new FormControl("");

    // 被振込口座設定
    this.cbxReceiveAccountId1Ctrl = new FormControl("");
    this.cbxReceiveAccountId2Ctrl = new FormControl("");
    this.cbxReceiveAccountId3Ctrl = new FormControl("");

    this.bankName1Ctrl = new FormControl("");
    this.bankName2Ctrl = new FormControl("");
    this.bankName3Ctrl = new FormControl("");
    this.branchName1Ctrl = new FormControl("");
    this.branchName2Ctrl = new FormControl("");
    this.branchName3Ctrl = new FormControl("");
    this.accountType1Ctrl = new FormControl("");
    this.accountType2Ctrl = new FormControl("");
    this.accountType3Ctrl = new FormControl("");
    this.accountNumber1Ctrl = new FormControl("");
    this.accountNumber2Ctrl = new FormControl("");
    this.accountNumber3Ctrl = new FormControl("");
    // 歩引設定
    this.minValueCtrl = new FormControl(""); // 歩引・最低実行金額
    this.rate1Ctrl = new FormControl(""); // 歩引・歩引率１
    this.cmbRoundingMode1Ctrl = new FormControl(""); // 歩引・端数処理１
    this.departmentId1Ctrl = new FormControl(""); // 歩引・部門コード１
    this.departmentName1Ctrl = new FormControl(""); // 歩引・部門コード１
    this.accountTitleId1Ctrl = new FormControl(""); // 歩引・科目コード１
    this.accountTitleName1Ctrl = new FormControl(""); // 歩引・科目コード１
    this.subCode1Ctrl = new FormControl(""); // 歩引・補助コード１
    this.rate2Ctrl = new FormControl(""); // 歩引・歩引率２
    this.cmbRoundingMode2Ctrl = new FormControl(""); // 歩引・端数処理２
    this.departmentId2Ctrl = new FormControl(""); // 歩引・部門コード２
    this.departmentName2Ctrl = new FormControl(""); // 歩引・部門コード２
    this.accountTitleId2Ctrl = new FormControl(""); // 歩引・科目コード２
    this.accountTitleName2Ctrl = new FormControl(""); // 歩引・科目コード２
    this.subCode2Ctrl = new FormControl(""); // 歩引・補助コード２
    this.rate3Ctrl = new FormControl(""); // 歩引・歩引率３
    this.cmbRoundingMode3Ctrl = new FormControl(""); // 歩引・端数処理３
    this.departmentId3Ctrl = new FormControl(""); // 歩引・部門コード３
    this.departmentName3Ctrl = new FormControl(""); // 歩引・部門コード３
    this.accountTitleId3Ctrl = new FormControl(""); // 歩引・科目コード３
    this.accountTitleName3Ctrl = new FormControl(""); // 歩引・科目コード３
    this.subCode3Ctrl = new FormControl(""); // 歩引・補助コード３
    this.rate4Ctrl = new FormControl(""); // 歩引・歩引率４
    this.cmbRoundingMode4Ctrl = new FormControl(""); // 歩引・端数処理４
    this.departmentId4Ctrl = new FormControl(""); // 歩引・部門コード４
    this.departmentName4Ctrl = new FormControl(""); // 歩引・部門コード４
    this.accountTitleId4Ctrl = new FormControl(""); // 歩引・科目コード４
    this.accountTitleName4Ctrl = new FormControl(""); // 歩引・科目コード４
    this.subCode4Ctrl = new FormControl(""); // 歩引・補助コード４
    this.rate5Ctrl = new FormControl(""); // 歩引・歩引率５
    this.cmbRoundingMode5Ctrl = new FormControl(""); // 歩引・端数処理５
    this.departmentId5Ctrl = new FormControl(""); // 歩引・部門コード５
    this.departmentName5Ctrl = new FormControl(""); // 歩引・部門コード５
    this.accountTitleId5Ctrl = new FormControl(""); // 歩引・科目コード５
    this.accountTitleName5Ctrl = new FormControl(""); // 歩引・科目コード５
    this.subCode5Ctrl = new FormControl(""); // 歩引・補助コード５

    this.UndefineCtrl = new FormControl();
  }

  public setValidator() {
    this.MyFormGroup = new FormGroup({

      // 基本設定
      customerCodeCtrl: this.customerCodeCtrl,
      cbxCustomerIsParentCtrl: this.cbxCustomerIsParentCtrl,
      cbxUseKanaLearningCtrl: this.cbxUseKanaLearningCtrl,
      cbxUseFeeLearningCtrl: this.cbxUseFeeLearningCtrl,
      cbxUseFeeToleranceCtrl: this.cbxUseFeeToleranceCtrl,
      cbxPrioritizeMatchingIndividuallyCtrl: this.cbxPrioritizeMatchingIndividuallyCtrl,
      cbxExcludeInvoicePublishCtrl: this.cbxExcludeInvoicePublishCtrl,
      cbxExcludeReminderPublishCtrl: this.cbxExcludeReminderPublishCtrl,
      customerNameCtrl: this.customerNameCtrl,
      customerKanaCtrl: this.customerKanaCtrl,
      parentCustomerCodeCtrl: this.parentCustomerCodeCtrl,
      parentCustomerNameCtrl: this.parentCustomerNameCtrl,
      cmbShareTransferFeeCtrl: this.cmbShareTransferFeeCtrl,
      collationKeyCtrl: this.collationKeyCtrl,
      staffCodeCtrl: this.staffCodeCtrl,
      staffNameCtrl: this.staffNameCtrl,

      // 日程設定
      closingDayCtrl: this.closingDayCtrl,
      cbxIssueBillEachTimeCtrl: this.cbxIssueBillEachTimeCtrl,
      collectOffsetMonthCtrl: this.collectOffsetMonthCtrl,
      collectOffsetDayCtrl: this.collectOffsetDayCtrl,
      cmbHolidayFlagCtrl: this.cmbHolidayFlagCtrl,

      // 回収設定
      cmbCollectCategoryIdCtrl: this.cmbCollectCategoryIdCtrl,
      sightOfBillCtrl: this.sightOfBillCtrl,
      thresholdValueCtrl: this.thresholdValueCtrl,
      cmbLessThanCollectCategoryIdCtrl: this.cmbLessThanCollectCategoryIdCtrl,
      cmbGreaterThanCollectCategoryId1Ctrl: this.cmbGreaterThanCollectCategoryId1Ctrl,
      greaterThanRate1Ctrl: this.greaterThanRate1Ctrl,
      cmbGreaterThanRoundingMode1Ctrl: this.cmbGreaterThanRoundingMode1Ctrl,
      greaterThanSightOfBill1Ctrl: this.greaterThanSightOfBill1Ctrl,
      cmbGreaterThanCollectCategoryId2Ctrl: this.cmbGreaterThanCollectCategoryId2Ctrl,
      greaterThanRate2Ctrl: this.greaterThanRate2Ctrl,
      cmbGreaterThanRoundingMode2Ctrl: this.cmbGreaterThanRoundingMode2Ctrl,
      greaterThanSightOfBill2Ctrl: this.greaterThanSightOfBill2Ctrl,
      cmbGreaterThanCollectCategoryId3Ctrl: this.cmbGreaterThanCollectCategoryId3Ctrl,
      greaterThanRate3Ctrl: this.greaterThanRate3Ctrl,
      cmbGreaterThanRoundingMode3Ctrl: this.cmbGreaterThanRoundingMode3Ctrl,
      greaterThanSightOfBill3Ctrl: this.greaterThanSightOfBill3Ctrl,

      // 債権管理
      densaiCodeCtrl: this.densaiCodeCtrl,
      creditCodeCtrl: this.creditCodeCtrl,
      creditLimitCtrl: this.creditLimitCtrl,
      creditRankCtrl: this.creditRankCtrl,

      // 専用入金口座（仮想口座）
      exclusiveBankCodeCtrl: this.exclusiveBankCodeCtrl,
      exclusiveBankNameCtrl: this.exclusiveBankNameCtrl,
      exclusiveBranchCodeCtrl: this.exclusiveBranchCodeCtrl,
      exclusiveVirtualBranchCodeCtrl: this.exclusiveVirtualBranchCodeCtrl,
      exclusiveVirtualBranchNameCtrl: this.exclusiveVirtualBranchNameCtrl,
      exclusiveAccountNumberCtrl: this.exclusiveAccountNumberCtrl,
      cmbExclusiveAccountTypeIdCtrl: this.cmbExclusiveAccountTypeIdCtrl,

      // 口座振替用口座
      transferBankCodeCtrl: this.transferBankCodeCtrl,
      transferBankNameCtrl: this.transferBankNameCtrl,
      transferBrachCodeCtrl: this.transferBrachCodeCtrl,
      transferBranchNameCtrl: this.transferBranchNameCtrl,
      transferAccountNumberCtrl: this.transferAccountNumberCtrl,
      transferAccountTypeIdCtrl: this.transferAccountTypeIdCtrl,
      transferNewCodeCtrl: this.transferNewCodeCtrl,
      transferAccountNameCtrl: this.transferAccountNameCtrl,
      transferCustomerCodeCtrl: this.transferCustomerCodeCtrl,

      // 住所
      postalCodeCtrl: this.postalCodeCtrl,
      address1Ctrl: this.address1Ctrl,
      address2Ctrl: this.address2Ctrl,
      telCtrl: this.telCtrl,
      faxCtrl: this.faxCtrl,
      destinationDepartmentNameCtrl: this.destinationDepartmentNameCtrl,
      customerStaffNameCtrl: this.customerStaffNameCtrl,
      honorificCtrl: this.honorificCtrl,
      noteCtrl: this.noteCtrl,

      // 被振込口座設定
      cbxReceiveAccountId1Ctrl: this.cbxReceiveAccountId1Ctrl,
      cbxReceiveAccountId2Ctrl: this.cbxReceiveAccountId2Ctrl,
      cbxReceiveAccountId3Ctrl: this.cbxReceiveAccountId3Ctrl,

      bankName1Ctrl: this.bankName1Ctrl,
      bankName2Ctrl: this.bankName2Ctrl,
      bankName3Ctrl: this.bankName3Ctrl,
      branchName1Ctrl: this.branchName1Ctrl,
      branchName2Ctrl: this.branchName2Ctrl,
      branchName3Ctrl: this.branchName3Ctrl,
      accountType1Ctrl: this.accountType1Ctrl,
      accountType2Ctrl: this.accountType2Ctrl,
      accountType3Ctrl: this.accountType3Ctrl,
      accountNumber1Ctrl: this.accountNumber1Ctrl,
      accountNumber2Ctrl: this.accountNumber2Ctrl,
      accountNumber3Ctrl: this.accountNumber3Ctrl,

      // 歩引設定
      minValueCtrl: this.minValueCtrl, // 歩引・最低実行金額
      rate1Ctrl: this.rate1Ctrl, // 歩引・歩引率１
      cmbRoundingMode1Ctrl: this.cmbRoundingMode1Ctrl, // 歩引・端数処理１
      departmentId1Ctrl: this.departmentId1Ctrl, // 歩引・部門コード１
      departmentName1Ctrl: this.departmentName1Ctrl, // 歩引・部門コード１
      accountTitleId1Ctrl: this.accountTitleId1Ctrl, // 歩引・科目コード１
      accountTitleName1Ctrl: this.accountTitleName1Ctrl, // 歩引・科目コード１
      subCode1Ctrl: this.subCode1Ctrl, // 歩引・補助コード１
      rate2Ctrl: this.rate2Ctrl, // 歩引・歩引率２
      cmbRoundingMode2Ctrl: this.cmbRoundingMode2Ctrl, // 歩引・端数処理２
      departmentId2Ctrl: this.departmentId2Ctrl, // 歩引・部門コード２
      departmentName2Ctrl: this.departmentName2Ctrl, // 歩引・部門コード２
      accountTitleId2Ctrl: this.accountTitleId2Ctrl, // 歩引・科目コード２
      accountTitleName2Ctrl: this.accountTitleName2Ctrl, // 歩引・科目コード２
      subCode2Ctrl: this.subCode2Ctrl, // 歩引・補助コード２
      rate3Ctrl: this.rate3Ctrl, // 歩引・歩引率３
      cmbRoundingMode3Ctrl: this.cmbRoundingMode3Ctrl, // 歩引・端数処理３
      departmentId3Ctrl: this.departmentId3Ctrl, // 歩引・部門コード３
      departmentName3Ctrl: this.departmentName3Ctrl, // 歩引・部門コード３
      accountTitleId3Ctrl: this.accountTitleId3Ctrl, // 歩引・科目コード３
      accountTitleName3Ctrl: this.accountTitleName3Ctrl, // 歩引・科目コード３
      subCode3Ctrl: this.subCode3Ctrl, // 歩引・補助コード３
      rate4Ctrl: this.rate4Ctrl, // 歩引・歩引率４
      cmbRoundingMode4Ctrl: this.cmbRoundingMode4Ctrl, // 歩引・端数処理４
      departmentId4Ctrl: this.departmentId4Ctrl, // 歩引・部門コード４
      departmentName4Ctrl: this.departmentName4Ctrl, // 歩引・部門コード４
      accountTitleId4Ctrl: this.accountTitleId4Ctrl, // 歩引・科目コード４
      accountTitleName4Ctrl: this.accountTitleName4Ctrl, // 歩引・科目コード４
      subCode4Ctrl: this.subCode4Ctrl, // 歩引・補助コード４
      rate5Ctrl: this.rate5Ctrl, // 歩引・歩引率５
      cmbRoundingMode5Ctrl: this.cmbRoundingMode5Ctrl, // 歩引・端数処理５
      departmentId5Ctrl: this.departmentId5Ctrl, // 歩引・部門コード５
      departmentName5Ctrl: this.departmentName5Ctrl, // 歩引・部門コード５
      accountTitleId5Ctrl: this.accountTitleId5Ctrl, // 歩引・科目コード５
      accountTitleName5Ctrl: this.accountTitleName5Ctrl, // 歩引・科目コード５
      subCode5Ctrl: this.subCode5Ctrl, // 歩引・補助コード５

      UndefineCtrl: this.UndefineCtrl,
    });
  }

  public setFormatter() {
    if (this.userInfoService.ApplicationControl.customerCodeType == CODE_TYPE.NUMBER) {
      FormatterUtil.setNumberFormatter(this.customerCodeCtrl);
    }
    else if(this.userInfoService.ApplicationControl.customerCodeType == CODE_TYPE.HANKANA) {
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
    FormatterUtil.setNumberFormatter(this.collationKeyCtrl);

    FormatterUtil.setNumberFormatter(this.closingDayCtrl);
    FormatterUtil.setNumberFormatter(this.collectOffsetMonthCtrl);
    FormatterUtil.setNumberFormatter(this.collectOffsetDayCtrl);
    FormatterUtil.setNumberFormatter(this.sightOfBillCtrl);
    FormatterUtil.setCurrencyFormatter(this.thresholdValueCtrl);

    FormatterUtil.setNumberFormatter(this.greaterThanSightOfBill1Ctrl);
    FormatterUtil.setNumberFormatter(this.greaterThanSightOfBill2Ctrl);
    FormatterUtil.setNumberFormatter(this.greaterThanSightOfBill3Ctrl);
    FormatterUtil.setNumberPeriodFormatter(this.greaterThanRate1Ctrl)
    FormatterUtil.setNumberPeriodFormatter(this.greaterThanRate2Ctrl)
    FormatterUtil.setNumberPeriodFormatter(this.greaterThanRate3Ctrl)

    FormatterUtil.setCodeFormatter(this.densaiCodeCtrl);
    FormatterUtil.setCodeFormatter(this.creditCodeCtrl);
    FormatterUtil.setCurrencyFormatter(this.creditLimitCtrl);
    FormatterUtil.setCodeFormatter(this.creditRankCtrl);

    FormatterUtil.setNumberFormatter(this.exclusiveBankCodeCtrl);
    FormatterUtil.setNumberFormatter(this.exclusiveBranchCodeCtrl);
    FormatterUtil.setNumberFormatter(this.exclusiveVirtualBranchCodeCtrl);
    FormatterUtil.setNumberFormatter(this.exclusiveAccountNumberCtrl);
    FormatterUtil.setNumberFormatter(this.transferBankCodeCtrl);
    FormatterUtil.setNumberFormatter(this.transferBrachCodeCtrl);
    FormatterUtil.setNumberFormatter(this.transferNewCodeCtrl);
    FormatterUtil.setCodeFormatter(this.transferCustomerCodeCtrl);

    FormatterUtil.setZipFormatter(this.postalCodeCtrl);
    FormatterUtil.setTelFormatter(this.telCtrl);
    FormatterUtil.setTelFormatter(this.faxCtrl);

    FormatterUtil.setCurrencyFormatter(this.minValueCtrl)

    FormatterUtil.setNumberPeriodFormatter(this.rate1Ctrl)
    FormatterUtil.setCodeFormatter(this.departmentId1Ctrl)
    FormatterUtil.setCodeFormatter(this.accountTitleId1Ctrl)
    FormatterUtil.setCodeFormatter(this.subCode1Ctrl)

    FormatterUtil.setNumberPeriodFormatter(this.rate2Ctrl)
    FormatterUtil.setCodeFormatter(this.departmentId2Ctrl)
    FormatterUtil.setCodeFormatter(this.accountTitleId2Ctrl)
    FormatterUtil.setCodeFormatter(this.subCode2Ctrl)

    FormatterUtil.setNumberPeriodFormatter(this.rate3Ctrl)
    FormatterUtil.setCodeFormatter(this.departmentId3Ctrl)
    FormatterUtil.setCodeFormatter(this.accountTitleId3Ctrl)
    FormatterUtil.setCodeFormatter(this.subCode3Ctrl)

    FormatterUtil.setNumberPeriodFormatter(this.rate4Ctrl)
    FormatterUtil.setCodeFormatter(this.departmentId4Ctrl)
    FormatterUtil.setCodeFormatter(this.accountTitleId4Ctrl)
    FormatterUtil.setCodeFormatter(this.subCode4Ctrl)

    FormatterUtil.setNumberPeriodFormatter(this.rate5Ctrl)
    FormatterUtil.setCodeFormatter(this.departmentId5Ctrl)
    FormatterUtil.setCodeFormatter(this.accountTitleId5Ctrl)
    FormatterUtil.setCodeFormatter(this.subCode5Ctrl)
  }

  public clear() {
    if (this.customer.code != undefined) {
      this.customerCodeCtrl.setValue(this.customer.code);
      this.customerCodeCtrl.disable();

      this.cbxCustomerIsParentCtrl.setValue(this.customer.isParent);

      this.cbxUseKanaLearningCtrl.setValue(this.customer.useKanaLearning);
      this.cbxUseFeeLearningCtrl.setValue(this.customer.useFeeLearning);
      this.cbxUseFeeToleranceCtrl.setValue(this.customer.useFeeTolerance);
      this.cbxPrioritizeMatchingIndividuallyCtrl.setValue(this.customer.prioritizeMatchingIndividually)
      this.cbxExcludeInvoicePublishCtrl.setValue(this.customer.excludeInvoicePublish)
      this.cbxExcludeReminderPublishCtrl.setValue(this.customer.excludeReminderPublish)
      if (this.customer.code != undefined && this.customer.shareTransferFee == 0) {
        this.cbxUseFeeLearningCtrl.disable();
        this.cbxUseFeeToleranceCtrl.disable();
      }      

      this.customerNameCtrl.setValue(this.customer.name);
      this.customerKanaCtrl.setValue(this.customer.kana);

      this.parentCustomerCodeCtrl.setValue(this.customer.parentCode);
      this.parentCustomerNameCtrl.setValue(this.customer.parentName);

      this.cmbShareTransferFeeCtrl.setValue(this.customer.shareTransferFee);
      this.isRegistrationFeeButton = this.ComponentStatus == COMPONENT_STATUS_TYPE.UPDATE
        && this.customer.shareTransferFee == 1 ? false : true;

      this.collationKeyCtrl.setValue(this.customer.collationKey);

      this.staffId = this.customer.staffId;
      this.staffCodeCtrl.setValue(this.customer.staffCode);
      this.staffNameCtrl.setValue(this.customer.staffName);

      this.closingDayCtrl.setValue(this.customer.closingDay);

      if(this.closingDayCtrl.value=="00"){
        this.collectOffsetMonthCtrl.disable();
        this.collectOffsetMonthCtrl.setValue("");
      }
      else{
        this.collectOffsetMonthCtrl.enable();
        this.collectOffsetMonthCtrl.setValue(this.customer.collectOffsetMonth);
      }

      this.collectOffsetDayCtrl.setValue(this.customer.collectOffsetDay);

      if (this.customer.holidayFlag == null || this.customer.holidayFlag == undefined) {
        this.cmbHolidayFlagCtrl.setValue(0);
      } else {
        this.cmbHolidayFlagCtrl.setValue(this.customer.holidayFlag);
      }

      //this.cbxIssueBillEachTimeCtrl.setValue(this.customer.is);

      this.cmbCollectCategoryIdCtrl.setValue(this.customer.collectCategoryId);
      this.sightOfBillCtrl.setValue(this.customer.sightOfBill);

      this.thresholdValueCtrl.setValue(this.customer.thresholdValue); // 約定金額
      this.separateValue(this.thresholdValueCtrl);
      if (this.customer.thresholdValue.valueOf() > 0) {
        this.isCustomerPaymentContractRegistry = false;
      }

      this.cmbLessThanCollectCategoryIdCtrl.setValue(this.customer.lessThanCollectCategoryId); // 約定金額未満

      this.cmbGreaterThanCollectCategoryId1Ctrl.setValue(this.customer.greaterThanCollectCategoryId1); // 約定金額以上①
      this.greaterThanRate1Ctrl.setValue(this.customer.greaterThanRate1); // 分割
      this.cmbGreaterThanRoundingMode1Ctrl.setValue(this.customer.greaterThanRoundingMode1); // 端数単位

      if (this.customer.greaterThanSightOfBill1 != 0) {
        this.greaterThanSightOfBill1Ctrl.setValue(this.customer.greaterThanSightOfBill1); // 回収サイト
      }

      if (this.customer.greaterThanCollectCategoryId2 != null) {
        this.cmbGreaterThanCollectCategoryId2Ctrl.setValue(this.customer.greaterThanCollectCategoryId2); // 約定金額以上②
        this.greaterThanRate2Ctrl.setValue(this.customer.greaterThanRate2); // 分割
        this.cmbGreaterThanRoundingMode2Ctrl.setValue(this.customer.greaterThanRoundingMode2); // 端数単位
        this.greaterThanSightOfBill2Ctrl.setValue(this.customer.greaterThanSightOfBill2); // 回収サイト  
      }
      else {
        this.cmbGreaterThanCollectCategoryId2Ctrl.disable();
        this.greaterThanRate2Ctrl.disable();
        this.cmbGreaterThanRoundingMode2Ctrl.disable();
      }

      if (this.customer.greaterThanCollectCategoryId3 != null) {
        this.cmbGreaterThanCollectCategoryId3Ctrl.setValue(this.customer.greaterThanCollectCategoryId3); // 約定金額以上③
        this.greaterThanRate3Ctrl.setValue(this.customer.greaterThanRate3); // 分割
        this.cmbGreaterThanRoundingMode3Ctrl.setValue(this.customer.greaterThanRoundingMode3); // 端数単位
        this.greaterThanSightOfBill3Ctrl.setValue(this.customer.greaterThanSightOfBill3); // 回収サイト  
      }
      else {
        this.cmbGreaterThanCollectCategoryId3Ctrl.disable();
        this.greaterThanRate3Ctrl.disable();
        this.cmbGreaterThanRoundingMode3Ctrl.disable();
      }

      this.densaiCodeCtrl.setValue(this.customer.densaiCode);  // 電子手形用企業コード

      this.creditCodeCtrl.setValue(this.customer.creditCode);  // 信用調査用企業コード
      this.creditLimitCtrl.setValue(this.customer.creditLimit); // 与信限度額
      this.creditRankCtrl.setValue(this.customer.creditRank);  // 与信ランク    

      this.exclusiveBankCodeCtrl.setValue(this.customer.exclusiveBankCode);		// 専用入金口座・銀行コード
      this.exclusiveBankNameCtrl.setValue(this.customer.exclusiveBankName);		// 専用入金口座・銀行名
      this.exclusiveBranchCodeCtrl.setValue(this.customer.exclusiveBranchCode);		// 専用入金口座・支店コード
      this.exclusiveVirtualBranchCodeCtrl.setValue(this.customer.exclusiveBranchCode);		// 専用入金口座・仮想支店コード
      this.exclusiveVirtualBranchNameCtrl.setValue(this.customer.exclusiveBranchName);		// 専用入金口座・仮想支店名
      this.exclusiveAccountNumberCtrl.setValue(this.customer.exclusiveAccountNumber.substr(3));		// 専用入金口座・口座番号
      this.cmbExclusiveAccountTypeIdCtrl.setValue(this.customer.exclusiveAccountTypeId);		// 専用入金口座・預金種別

      this.transferBankCodeCtrl.setValue(this.customer.transferBankCode);		// 口座振替用口座・銀行コード
      this.transferBankNameCtrl.setValue(this.customer.transferBankName);		// 口座振替用口座・銀行名
      this.transferBrachCodeCtrl.setValue(this.customer.transferBranchCode);		// 口座振替用口座・支店コード
      this.transferBranchNameCtrl.setValue(this.customer.transferBranchName);		// 口座振替用口座・支店名
      this.transferAccountNumberCtrl.setValue(this.customer.transferAccountNumber);		// 口座振替用口座・口座番号
      this.transferAccountTypeIdCtrl.setValue(this.customer.transferAccountTypeId);		// 口座振替用口座・預金種別
      this.transferNewCodeCtrl.setValue(this.customer.transferNewCode);		// 口座振替用口座・新規コード
      this.transferAccountNameCtrl.setValue(this.customer.transferAccountName);		// 口座振替用口座・預金者名
      this.transferCustomerCodeCtrl.setValue(this.customer.transferCustomerCode);		// 口座振替用口座・顧客コード

      this.postalCodeCtrl.setValue(this.customer.postalCode);		// 郵便番号
      this.address1Ctrl.setValue(this.customer.address1);		// 住所１
      this.address2Ctrl.setValue(this.customer.address2);		// 住所２
      this.telCtrl.setValue(this.customer.tel);		// 電話番号
      this.faxCtrl.setValue(this.customer.fax);		// FAX番号
      this.destinationDepartmentNameCtrl.setValue(this.customer.destinationDepartmentName);		// 相手先部署
      this.customerStaffNameCtrl.setValue(this.customer.customerStaffName);		// 相手先担当者名
      this.honorificCtrl.setValue(this.customer.honorific);		// 敬称
      this.noteCtrl.setValue(this.customer.note);		// 備考

      this.cbxReceiveAccountId1Ctrl.setValue(this.customer.receiveAccountId1);		// 被振込口座設定１
      this.cbxReceiveAccountId2Ctrl.setValue(this.customer.receiveAccountId2);		// 被振込口座設定２
      this.cbxReceiveAccountId3Ctrl.setValue(this.customer.receiveAccountId3);		// 被振込口座設定３

      HtmlUtil.nextFocusByName(this.elementRef, 'customerNameCtrl', EVENT_TYPE.NONE);
      if (this.processModalCustomResult.result == PROCESS_RESULT_RESULT_TYPE.SUCCESS) {
        this.processResultService.clearProcessCustomMsg(this.processModalCustomResult);
      }

    }
    else{
      this.MyFormGroup.reset();
      this.closingDayCtrl.enable();
      this.collectOffsetMonthCtrl.setValidators([Validators.required, CustomValidators.number]);
      this.collectOffsetMonthCtrl.enable();
      // 休業日設定の初期値を設定する。
      this.cmbHolidayFlagCtrl.setValue(0);

      this.cbxReceiveAccountId1Ctrl.setValue(true);		// 被振込口座設定１
      this.cbxReceiveAccountId2Ctrl.setValue(true);		// 被振込口座設定２
      this.cbxReceiveAccountId3Ctrl.setValue(true);

      this.collectCategoryCode = "";

      HtmlUtil.nextFocusByName(this.elementRef, 'customerCodeCtrl', EVENT_TYPE.NONE);

    }
  }

  public setForm() {
    switch (this.fromPageId) {
      case ComponentId.PE0101:
      this.isDeleteButton = true;
        this.MyFormGroup.disable();
        this.customerNameCtrl.enable();
        this.customerKanaCtrl.enable();
        this.cbxCustomerIsParentCtrl.enable();
        this.cbxUseKanaLearningCtrl.enable();
        this.cmbShareTransferFeeCtrl.enable();
        this.staffCodeCtrl.enable();
        this.closingDayCtrl.enable();
        this.collectOffsetMonthCtrl.enable();
        this.collectOffsetDayCtrl.enable();
        this.cmbCollectCategoryIdCtrl.enable();
        break;

      case ComponentId.PE0102:
        this.isDeleteButton = true;
        this.MyFormGroup.disable();
        break;

      default:
        break;
    }

  }



  public setAutoComplete(){

    // 担当者
    this.initAutocompleteStaffs(this.staffCodeCtrl,this.staffService,0);

  }

  /**
   * ボタン操作によるメソッド呼び出し
   * @param action ボタン操作
   */
  public buttonActionInner(action: BUTTON_ACTION) {
    this.processResultService.processResultStart(this.processModalCustomResult, action);
    if (this.processModalCustomResult.result == PROCESS_RESULT_RESULT_TYPE.FAILURE) {
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

  /**
   * データ登録・編集
   */
  public registry() {
    let registryCustomerData = new Customer();
    let registryCustomerPaymentContractData = new CustomerPaymentContract();
    let isRegistry: boolean = false;

    let checkValue = this.checkContent();
    if (!checkValue) {
      return;
    }

    if (this.customerService.selectCustmer.id == undefined) {
      registryCustomerData.id = 0;
      isRegistry = true;

    } else {
      registryCustomerData = this.customerService.selectCustmer;

    }

    let componentFactory = this.componentFactoryResolver.resolveComponentFactory(ModalRouterProgressComponent);
    let componentRef = this.viewContainerRef.createComponent(componentFactory);
    // componentRef.destroy();    

    // Customerデータ取得
    this.setFormContentToData(registryCustomerData);
    FileUtil.replaceNull(registryCustomerData); 

    this.customerService.Save(registryCustomerData)
      .subscribe(customerResult => {
        this.processModalCustomResult = this.processResultService.processAtSave(
          this.processModalCustomResult, customerResult, isRegistry, this.partsResultMessageComponent);
        if (this.processModalCustomResult.result == PROCESS_RESULT_RESULT_TYPE.SUCCESS) {
          if (registryCustomerData.collectCategoryCode == "00") {
            // CustomerPaymentContrac データ取得
            registryCustomerPaymentContractData.customerId = customerResult.id;
            registryCustomerPaymentContractData.thresholdValue = registryCustomerData.thresholdValue;
            registryCustomerPaymentContractData.lessThanCollectCategoryId = registryCustomerData.lessThanCollectCategoryId;
            registryCustomerPaymentContractData.greaterThanCollectCategoryId1 = registryCustomerData.greaterThanCollectCategoryId1;
            registryCustomerPaymentContractData.greaterThanRate1 = registryCustomerData.greaterThanRate1;
            registryCustomerPaymentContractData.greaterThanRoundingMode1 = registryCustomerData.greaterThanRoundingMode1;
            if (registryCustomerData.greaterThanSightOfBill1 == null || registryCustomerData.greaterThanSightOfBill1 == 0) {
              registryCustomerPaymentContractData.greaterThanSightOfBill1 = 0;
            } else {
              registryCustomerPaymentContractData.greaterThanSightOfBill1 = registryCustomerData.greaterThanSightOfBill1;
            }
            registryCustomerPaymentContractData.greaterThanCollectCategoryId2 = registryCustomerData.greaterThanCollectCategoryId2;
            registryCustomerPaymentContractData.greaterThanRate2 = registryCustomerData.greaterThanRate2;
            registryCustomerPaymentContractData.greaterThanRoundingMode2 = registryCustomerData.greaterThanRoundingMode2;
            registryCustomerPaymentContractData.greaterThanSightOfBill2 = registryCustomerData.greaterThanSightOfBill2;
            registryCustomerPaymentContractData.greaterThanCollectCategoryId3 = registryCustomerData.greaterThanCollectCategoryId3;
            registryCustomerPaymentContractData.greaterThanRate3 = registryCustomerData.greaterThanRate3;
            registryCustomerPaymentContractData.greaterThanRoundingMode3 = registryCustomerData.greaterThanRoundingMode3;
            registryCustomerPaymentContractData.greaterThanSightOfBill3 = registryCustomerData.greaterThanSightOfBill3;
            registryCustomerPaymentContractData.greaterThanCode1 = registryCustomerData.greaterThanCollectCategoryCode1;
            registryCustomerPaymentContractData.greaterThanCode2 = registryCustomerData.greaterThanCollectCategoryCode2;
            registryCustomerPaymentContractData.greaterThanCode3 = registryCustomerData.greaterThanCollectCategoryCode3;
            registryCustomerPaymentContractData.lessThanCode = registryCustomerData.lessThanCollectCategoryCode;
            registryCustomerPaymentContractData.collectCategoryCode = registryCustomerData.collectCategoryCode;
            if (this.isCustomerPaymentContractRegistry) {
              registryCustomerPaymentContractData.createBy = registryCustomerData.updateBy;
              isRegistry = true;
            }
            registryCustomerPaymentContractData.updateBy = registryCustomerData.updateBy;

            this.customerPaymentContractService.Save(registryCustomerPaymentContractData)
              .subscribe(result => {
                this.processModalCustomResult = this.processResultService.processAtSave(
                  this.processModalCustomResult, result, isRegistry, this.partsResultMessageComponent);
                if (this.processModalCustomResult.result == PROCESS_RESULT_RESULT_TYPE.SUCCESS) {
                  this.customer = registryCustomerData;
                }
              });
          } else if (registryCustomerData.thresholdValue == 0 || registryCustomerData.thresholdValue == null) {
            if (!this.isCustomerPaymentContractRegistry) {
              this.customerPaymentContractService.Delete(customerResult.id)
                .subscribe(result => {
                  this.processModalCustomResult = this.processResultService.processAtDelete(
                    this.processModalCustomResult, result, this.partsResultMessageComponent);
                  if (this.processModalCustomResult.result == PROCESS_RESULT_RESULT_TYPE.SUCCESS) {
                    this.customer = registryCustomerData;
                  }
                });
            }

          } else {
            this.customer = registryCustomerData;
          }
        }
        componentRef.destroy();
      });
  }

  public goPageTop(eventType: string) {
    HtmlUtil.nextFocusByNames(this.elementRef, ['customerCodeCtrl', 'customerNameCtrl'], eventType);
  }

  public isShowDelete(): boolean {
    return this.ComponentStatus != COMPONENT_STATUS_TYPE.CREATE;
  }

  /**
   * データ削除
   */
  public delete() {
    let existDeleteResultList = new Array<ExistDeleteResult>();

    for (let i = 0; i < 5; i++) {
      existDeleteResultList[i] = new ExistDeleteResult();
      existDeleteResultList[i].idName = '得意先コード';
    }

    let existCustomerGroupResponse = this.customerGroupService.ExistCustomer(this.customer.id);
    existDeleteResultList[0].masterName = TABLES.MASTER_CUSTOMER_GROUP.name;

    let existBillingResponse = this.billingServide.ExistCustomer(this.customer.id);
    existDeleteResultList[1].masterName = TABLES.BILLING.name;

    let history: KanaHistoryCustomer = new KanaHistoryCustomer();
    history.customerId = this.customer.id;
    let existKanaHistoryCustomerResponse = this.kanaHistoryCustomerMaster.ExistCustomer(history);
    existDeleteResultList[2].masterName = TABLES.MASTER_KANA_HISTORY_CUSTOMER.name;

    let existReceiptResponse = this.receiptService.ExistCustomer(this.customer.id);
    existDeleteResultList[3].masterName = TABLES.RECEIPT.name;

    let existNettingResponse = this.nettingService.ExistCustomer(this.customer.id);
    existDeleteResultList[4].masterName = TABLES.MASTER_NETTING.name;

    // 5つの処理の待機
    forkJoin(
      existCustomerGroupResponse,
      existBillingResponse,
      existKanaHistoryCustomerResponse,
      existReceiptResponse,
      existNettingResponse
    ).subscribe(
      responseList => {
        this.processModalCustomResult = this.processResultService.processAtExist(
          this.processModalCustomResult, responseList, existDeleteResultList, this.partsResultMessageComponent);
        if (this.processModalCustomResult.result == PROCESS_RESULT_RESULT_TYPE.WARNING) {
          return;
        }

        // 削除処理
        let componentFactory = this.componentFactoryResolver.resolveComponentFactory(ModalConfirmComponent);
        let componentRef = this.viewContainerRef.createComponent(componentFactory);
        componentRef.instance.ActionName = "削除"
        componentRef.instance.Closing.subscribe(() => {

          if (componentRef.instance.ModalStatus == MODAL_STATUS_TYPE.OK) {

            let componentFactory = this.componentFactoryResolver.resolveComponentFactory(ModalRouterProgressComponent);
            let componentRef = this.viewContainerRef.createComponent(componentFactory);
            // componentRef.destroy();   

            this.customerService.Delete(this.customer.id)
              .subscribe(result => {
                this.processModalCustomResult = this.processResultService.processAtDelete(
                  this.processModalCustomResult, result, this.partsResultMessageComponent);
                if (this.processModalCustomResult.result == PROCESS_RESULT_RESULT_TYPE.SUCCESS) {
                }
                componentRef.destroy(); 
              });
          }
          componentRef.destroy();
        });
      },
      error => {
        console.log(error)
        this.processModalCustomResult = this.processResultService.processAtDelete(
          this.processModalCustomResult, 0, this.partsResultMessageComponent);
      }
    );
  }

  public close() {
    this.ModalStatus = MODAL_STATUS_TYPE.CLOSE;
    this.processModalCustomResult = this.processResultService.clearProcessCustomMsg(this.processModalCustomResult);
    this.closing.emit({});
  }

  public cancel() {
    this.ModalStatus = MODAL_STATUS_TYPE.CANCEL;
    this.processModalCustomResult = this.processResultService.clearProcessCustomMsg(this.processModalCustomResult);
    this.closing.emit({});
  }


  /**
   * 各テーブルのデータを取得する
   * @param table テーブル種別
   * @param keyCode イベント種別
   */
  public openMasterModal(table: TABLE_INDEX) {
    if (this.staffCodeTrigger != null) this.staffCodeTrigger.closePanel();
      let componentFactory = this.componentFactoryResolver.resolveComponentFactory(ModalMasterComponent);
      let componentRef = this.viewContainerRef.createComponent(componentFactory);
      componentRef.instance.TableIndex = table;
      // モーダルのモーダルのためz-indexを2倍にする。
      componentRef.instance.zIndex=componentRef.instance.zIndexDefSize*2;

      componentRef.instance.Closing.subscribe(() => {

        if (componentRef.instance.ModalStatus == MODAL_STATUS_TYPE.SELECT) {
          switch (table) {

            case TABLE_INDEX.MASTER_STAFF:
              {
                this.staffId = componentRef.instance.SelectedObject.id;
                this.staffCodeCtrl.setValue(componentRef.instance.SelectedCode);
                this.staffNameCtrl.setValue(componentRef.instance.SelectedName);
                break;
              }
          }
        }
        componentRef.destroy();
      });
  }

  /**
   * 登録手数料設定呼び出し
   */
  public openTransferFeeModal() {
    let componentFactory = this.componentFactoryResolver.resolveComponentFactory(ModalTransferFeeComponent);
    let componentRef = this.viewContainerRef.createComponent(componentFactory);
    componentRef.instance.processModalCustomResult = this.processModalCustomResult;
    componentRef.instance.CustomerId = this.customer.id;

    componentRef.instance.Closing.subscribe(() => {
      componentRef.destroy();
    });
  }

  ///// 選択した値によるフォーム操作 ////////////////////////////////////////////////////  
  public setCustomerCode(eventType: string) {
    if (!StringUtil.IsNullOrEmpty(this.customerCodeCtrl.value)) {
      this.customerService.GetItems(this.customerCodeCtrl.value)
        .subscribe(response => {
          if (response != PROCESS_RESULT_RESULT_TYPE.FAILURE && response.length > 0) {
            this.customer = response[0];
            this.clear();
          }
          HtmlUtil.nextFocusByName(this.elementRef, 'customerNameCtrl', eventType);
        });
    }    
    HtmlUtil.nextFocusByName(this.elementRef, 'customerNameCtrl', eventType);
  }

  public setCustomerName(eventType: string) {
    HtmlUtil.nextFocusByName(this.elementRef, 'customerKanaCtrl', eventType);
  }


  public setCustomerKana(eventType: string) {
    let value = this.customerKanaCtrl.value;
    value = EbDataHelper.convertToValidEbkana(value);
    value = EbDataHelper.removePersonalities(value, this.juridicalPersonalitysResult.juridicalPersonalities);
    this.customerKanaCtrl.setValue(value);

    HtmlUtil.nextFocusByName(this.elementRef, 'cmbShareTransferFeeCtrl', eventType);

  }

  public setCollationKey(eventType: string) {
    HtmlUtil.nextFocusByName(this.elementRef, 'staffCodeCtrl', eventType);
  }


  /////////////////////////////////////////////////////////////////////////////////

  public setStaffCode(eventType: string) {

    if(eventType!=EVENT_TYPE.BLUR){
      this.staffCodeTrigger.closePanel();
    }
    
    if (!StringUtil.IsNullOrEmpty(this.staffCodeCtrl.value)) {

      this.loadStart();
      this.staffService.GetItems(this.staffCodeCtrl.value)
        .subscribe(response => {
          this.loadEnd();
          if (response != PROCESS_RESULT_RESULT_TYPE.FAILURE && response.length > 0) {
            this.staffId = response[0].id;
            this.staffCodeCtrl.setValue(response[0].code);
            this.staffNameCtrl.setValue(response[0].name);

            HtmlUtil.nextFocusByName(this.elementRef, 'closingDayCtrl', eventType);
          }
          else {
            this.staffId=0;
            //this.staffCodeCtrl.setValue("");
            this.staffNameCtrl.setValue("");
          }
        });
    }
    else {
      this.staffId=0;
      this.staffCodeCtrl.setValue("");
      this.staffNameCtrl.setValue("");
      HtmlUtil.nextFocusByName(this.elementRef, 'closingDayCtrl', eventType);
    }  
  }

  public setClosingDay(eventType: string) {

    this.closingDayCtrl.setValue(
      this.GetFormattedClosingDay(
        this.closingDayCtrl.value, !this.cbxIssueBillEachTimeCtrl.value));

    if(this.closingDayCtrl.value=="00"){
      this.collectOffsetMonthCtrl.setValue("");
      this.collectOffsetMonthCtrl.disable();
      this.cbxIssueBillEachTimeCtrl.setValue(true);
    }

    HtmlUtil.nextFocusByNames(this.elementRef, ['collectOffsetMonthCtrl','collectOffsetDayCtrl'], eventType);
  }

  public setCbxIssueBillEachTime1(eventType: string){
    if(this.cbxIssueBillEachTimeCtrl.value){
      this.closingDayCtrl.setValue("00");
      this.closingDayCtrl.disable();
      this.collectOffsetMonthCtrl.setValue("");
      this.collectOffsetMonthCtrl.disable();
      this.collectOffsetDayCtrl.setValue("");
      this.collectOffsetDayCtrl.enable();
      HtmlUtil.nextFocusByName(this.elementRef, 'collectOffsetDayCtrl', eventType);

    }
    else{
      this.closingDayCtrl.setValue("");
      this.closingDayCtrl.enable();
      this.collectOffsetMonthCtrl.setValue("");
      this.collectOffsetMonthCtrl.enable();
      this.collectOffsetDayCtrl.setValue("");
      this.collectOffsetDayCtrl.enable();
      HtmlUtil.nextFocusByName(this.elementRef, 'closingDayCtrl', eventType);
    }

  }

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

  public setCollectOffsetMonth(eventType: string) {
    HtmlUtil.nextFocusByName(this.elementRef, 'collectOffsetDayCtrl', eventType);

  }

  public setCollectOffsetDay(eventType: string) {

    this.collectOffsetDayCtrl.setValue(
        this.GetFormattedClosingDay(
            this.collectOffsetDayCtrl.value, !this.cbxIssueBillEachTimeCtrl.value));

    HtmlUtil.nextFocusByName(this.elementRef, 'cmbHolidayFlagCtrl', eventType);

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


  public setCollectCategoryId() {
    // 約定の場合
    if (this.cmbCollectCategoryIdCtrl.value != undefined) {
      this.collectCategoryCode = this.categoryResult.categories.find(x => x.id == this.cmbCollectCategoryIdCtrl.value).code;
    }
    
    if (this.collectCategoryCode == "00") {
      this.thresholdValueCtrl.enable();
      this.cmbLessThanCollectCategoryIdCtrl.enable();
      this.cmbGreaterThanCollectCategoryId1Ctrl.enable();
    } else {
      this.thresholdValueCtrl.reset();
      this.cmbLessThanCollectCategoryIdCtrl.reset();
      this.cmbGreaterThanCollectCategoryId1Ctrl.reset();
      this.cmbGreaterThanCollectCategoryId2Ctrl.reset();
      this.cmbGreaterThanCollectCategoryId3Ctrl.reset();
      this.greaterThanRate1Ctrl.reset();
      this.greaterThanRate2Ctrl.reset();
      this.greaterThanRate3Ctrl.reset();
      this.cmbGreaterThanRoundingMode1Ctrl.reset();
      this.cmbGreaterThanRoundingMode2Ctrl.reset();
      this.cmbGreaterThanRoundingMode3Ctrl.reset();

      this.sightOfBillCtrl.disable();
      this.thresholdValueCtrl.disable();
      this.cmbLessThanCollectCategoryIdCtrl.disable();
      this.cmbGreaterThanCollectCategoryId1Ctrl.disable();
      this.cmbGreaterThanCollectCategoryId2Ctrl.disable();
      this.cmbGreaterThanCollectCategoryId3Ctrl.disable();
      this.greaterThanRate1Ctrl.disable();
      this.greaterThanRate2Ctrl.disable();
      this.greaterThanRate3Ctrl.disable();
      this.cmbGreaterThanRoundingMode1Ctrl.disable();
      this.cmbGreaterThanRoundingMode2Ctrl.disable();
      this.cmbGreaterThanRoundingMode3Ctrl.disable();

    }

    // 回収サイトの有効無効
    this.setSightOfBillCtrl(this.cmbCollectCategoryIdCtrl);

  }

  public setSightOfBillCtrl(ctrlName: FormControl) {
    let isDisable: boolean = true;

    // 手形・期日現金の場合
    let useLimitDate: number = 0;

    if (ctrlName.value != undefined) {
      useLimitDate = this.categoryResult.categories.find(x => x.id == ctrlName.value).useLimitDate;
    }

    if (useLimitDate == 1) {
      isDisable = false;
    }

    switch (ctrlName) {
      case this.cmbCollectCategoryIdCtrl:
        if (isDisable) {
          this.sightOfBillCtrl.disable();
          if (this.greaterThanSightOfBill1Ctrl.value == undefined) {
            this.greaterThanSightOfBill1Ctrl.disable();
          }
          if (this.greaterThanSightOfBill2Ctrl.value == undefined) {
            this.greaterThanSightOfBill2Ctrl.disable();
          }
          if (this.greaterThanSightOfBill3Ctrl.value == undefined) {
            this.greaterThanSightOfBill3Ctrl.disable();
          }
        } else {
          this.sightOfBillCtrl.enable();
        }
        break;

      case this.cmbGreaterThanCollectCategoryId1Ctrl:
        this.greaterThanRate1Ctrl.enable();
        this.cmbGreaterThanRoundingMode1Ctrl.enable();
        this.useLimitDate1 = useLimitDate;
        if (isDisable) {
          this.greaterThanSightOfBill1Ctrl.disable();
        } else {
          this.greaterThanSightOfBill1Ctrl.enable();
        }
        break;

      case this.cmbGreaterThanCollectCategoryId2Ctrl:
        this.greaterThanRate2Ctrl.enable();
        this.cmbGreaterThanRoundingMode2Ctrl.enable();
        this.useLimitDate2 = useLimitDate;
        if (isDisable) {
          this.greaterThanSightOfBill2Ctrl.disable();
        } else {
          this.greaterThanSightOfBill2Ctrl.enable();
        }
        break;

      case this.cmbGreaterThanCollectCategoryId3Ctrl:
        this.greaterThanRate3Ctrl.enable();
        this.cmbGreaterThanRoundingMode3Ctrl.enable();
        this.useLimitDate3 = useLimitDate;
        if (isDisable) {
          this.greaterThanSightOfBill3Ctrl.disable();
        } else {
          this.greaterThanSightOfBill3Ctrl.enable();
        }
        break;

      default:
        break;
    }
  }

  public setGreaterThanRate1(eventType: string) {

    if (this.greaterThanRate1Ctrl.value != undefined && !StringUtil.IsNullOrEmpty(this.greaterThanRate1Ctrl.value)) {
      let value = this.greaterThanRate1Ctrl.value * 10;
      this.greaterThanRate1Ctrl.setValue(Math.round(value) / 10);
      if (value == 0) {
        this.greaterThanRate1Ctrl.reset();
      }
    }

    if (this.greaterThanRate1Ctrl.value == undefined || this.greaterThanRate1Ctrl.value == 0 || this.greaterThanRate1Ctrl.value >= 100) {
      // 約定金額以上２
      this.cmbGreaterThanCollectCategoryId2Ctrl.reset();
      this.cmbGreaterThanCollectCategoryId2Ctrl.disable();
      this.greaterThanRate2Ctrl.reset();
      this.greaterThanRate2Ctrl.disable();
      this.cmbGreaterThanRoundingMode2Ctrl.reset();
      this.cmbGreaterThanRoundingMode2Ctrl.disable();
      this.greaterThanSightOfBill2Ctrl.reset();
      this.greaterThanSightOfBill2Ctrl.disable();
      // 約定金額以上３
      this.cmbGreaterThanCollectCategoryId3Ctrl.reset();
      this.cmbGreaterThanCollectCategoryId3Ctrl.disable();
      this.greaterThanRate3Ctrl.reset();
      this.greaterThanRate3Ctrl.disable();
      this.cmbGreaterThanRoundingMode3Ctrl.reset();
      this.cmbGreaterThanRoundingMode3Ctrl.disable();
      this.greaterThanSightOfBill3Ctrl.reset();
      this.greaterThanSightOfBill3Ctrl.disable();
    }
    else {
      this.cmbGreaterThanCollectCategoryId2Ctrl.enable();
    }

    HtmlUtil.nextFocusByName(this.elementRef, 'cmbGreaterThanRoundingMode1Ctrl', eventType);
  }

  public setGreaterThanRate2(eventType: string) {

    if (this.greaterThanRate2Ctrl.value != undefined && !StringUtil.IsNullOrEmpty(this.greaterThanRate2Ctrl.value)) {
      let value = this.greaterThanRate2Ctrl.value * 10;
      this.greaterThanRate2Ctrl.setValue(Math.round(value) / 10);
      if (value == 0) {
        this.greaterThanRate2Ctrl.reset();
      }
    }

    let rate = this.greaterThanRate1Ctrl.value + this.greaterThanRate2Ctrl.value;

    if (this.greaterThanRate2Ctrl.value == undefined || this.greaterThanRate2Ctrl.value == 0 || rate >= 100) {
      // 約定金額以上３
      this.cmbGreaterThanCollectCategoryId3Ctrl.reset();
      this.cmbGreaterThanCollectCategoryId3Ctrl.disable();
      this.greaterThanRate3Ctrl.reset();
      this.greaterThanRate3Ctrl.disable();
      this.cmbGreaterThanRoundingMode3Ctrl.reset();
      this.cmbGreaterThanRoundingMode3Ctrl.disable();
      this.greaterThanSightOfBill3Ctrl.reset();
      this.greaterThanSightOfBill3Ctrl.disable();
    }
    else {
      this.cmbGreaterThanCollectCategoryId3Ctrl.enable();
    }

    HtmlUtil.nextFocusByName(this.elementRef, 'cmbGreaterThanRoundingMode2Ctrl', eventType);
  }

  public setGreaterThanRate3(eventType: string) {

    if (this.greaterThanRate3Ctrl.value != undefined && !StringUtil.IsNullOrEmpty(this.greaterThanRate3Ctrl.value)) {
      let value = this.greaterThanRate3Ctrl.value * 10;
      this.greaterThanRate3Ctrl.setValue(Math.round(value) / 10);
      if (value == 0) {
        this.greaterThanRate3Ctrl.reset();
      }
    }

    HtmlUtil.nextFocusByName(this.elementRef, 'cmbGreaterThanRoundingMode3Ctrl', eventType);
  }

  public setDensaiCode(eventType: string) {
    HtmlUtil.nextFocusByName(this.elementRef, 'creditCodeCtrl', eventType);
  }

  public setCreditCode(eventType: string) {
    HtmlUtil.nextFocusByName(this.elementRef, 'creditLimitCtrl', eventType);
  }

  public setCreditLimit(eventType: string) {
    if (!StringUtil.IsNullOrEmpty(this.creditLimitCtrl.value)) {
      this.separateValue(this.creditLimitCtrl)
    }
    HtmlUtil.nextFocusByName(this.elementRef, 'creditRankCtrl', eventType);
  }

  public setCreditRank(eventType: string) {
    HtmlUtil.nextFocusByName(this.elementRef, 'exclusiveBankCodeCtrl', eventType);
  }

  public setExclusiveBankCode(eventType: string) {
    if(!StringUtil.IsNullOrEmpty(this.exclusiveBankCodeCtrl.value)){
      this.exclusiveBankCodeCtrl.setValue(StringUtil.setPaddingFrontZero(this.exclusiveBankCodeCtrl.value, 4, true));
    }
    HtmlUtil.nextFocusByName(this.elementRef, 'exclusiveBankNameCtrl', eventType);
  }

  public setExclusiveBankName(eventType: string) {
    HtmlUtil.nextFocusByName(this.elementRef, 'exclusiveBranchCodeCtrl', eventType);
  }

  public setExclusiveBranchCode(eventType: string) {
    if(!StringUtil.IsNullOrEmpty(this.exclusiveBranchCodeCtrl.value)){
      this.exclusiveBranchCodeCtrl.setValue(StringUtil.setPaddingFrontZero(this.exclusiveBranchCodeCtrl.value, 3, true));
    }
    HtmlUtil.nextFocusByName(this.elementRef, 'exclusiveVirtualBranchCodeCtrl', eventType);
  }

  public setExclusiveVirtualBranchCode(eventType: string) {
    if(!StringUtil.IsNullOrEmpty(this.exclusiveVirtualBranchCodeCtrl.value)){
      this.exclusiveVirtualBranchCodeCtrl.setValue(StringUtil.setPaddingFrontZero(this.exclusiveVirtualBranchCodeCtrl.value, 3, true));
    }
    HtmlUtil.nextFocusByName(this.elementRef, 'exclusiveVirtualBranchNameCtrl', eventType);
  }

  public setExclusiveVirtualBranchName(eventType: string) {
    HtmlUtil.nextFocusByName(this.elementRef, 'exclusiveAccountNumberCtrl', eventType);
  }

  public setExclusiveAccountNumber(eventType: string) {
    if(!StringUtil.IsNullOrEmpty(this.exclusiveAccountNumberCtrl.value)){
      this.exclusiveAccountNumberCtrl.setValue(StringUtil.setPaddingFrontZero(this.exclusiveAccountNumberCtrl.value, 7, true));
    }

    HtmlUtil.nextFocusByName(this.elementRef, 'transferBankCodeCtrl', eventType);
  }
  
  public setTransferBankCode(eventType: string) {

    if(!StringUtil.IsNullOrEmpty(this.transferBankCodeCtrl.value)){
      this.transferBankCodeCtrl.setValue(StringUtil.setPaddingFrontZero(this.transferBankCodeCtrl.value, 4, true));
    }    
    HtmlUtil.nextFocusByName(this.elementRef, 'transferBankNameCtrl', eventType);
  }


  public setTransferBankName(eventType: string) {
    let value = this.transferBankNameCtrl.value;
    value = EbDataHelper.convertToValidEbkana(value);
    value = EbDataHelper.removePersonalities(value, this.juridicalPersonalitysResult.juridicalPersonalities);
    this.transferBankNameCtrl.setValue(value);

    HtmlUtil.nextFocusByName(this.elementRef, 'transferBrachCodeCtrl', eventType);

  }

  public setTransferBrachCode(eventType: string) {

    if(!StringUtil.IsNullOrEmpty(this.transferBrachCodeCtrl.value)){
      this.transferBrachCodeCtrl.setValue(StringUtil.setPaddingFrontZero(this.transferBrachCodeCtrl.value, 3, true));
    }

    HtmlUtil.nextFocusByName(this.elementRef, 'transferBranchNameCtrl', eventType);
  }

  public setTransferBranchName(eventType: string) {
    let value = this.transferBranchNameCtrl.value;
    value = EbDataHelper.convertToValidEbkana(value);
    value = EbDataHelper.removePersonalities(value, this.juridicalPersonalitysResult.juridicalPersonalities);
    this.transferBranchNameCtrl.setValue(value);

    HtmlUtil.nextFocusByName(this.elementRef, 'transferAccountNumberCtrl', eventType);

  }

  public setTransferAccountNumber(eventType: string) {

    if(!StringUtil.IsNullOrEmpty(this.transferAccountNumberCtrl.value)){
      this.transferAccountNumberCtrl.setValue(StringUtil.setPaddingFrontZero(this.transferAccountNumberCtrl.value, 7, true));
    }

    HtmlUtil.nextFocusByName(this.elementRef, 'transferNewCodeCtrl', eventType);
  }

  public setTransferNewCode(eventType: string) {
    HtmlUtil.nextFocusByName(this.elementRef, 'transferAccountNameCtrl', eventType);
  }


  public setTransferAccountName(eventType: string) {
    this.transferAccountNameCtrl.setValue(EbDataHelper.convertToKanaHalf(this.transferAccountNameCtrl.value));
    HtmlUtil.nextFocusByName(this.elementRef, 'transferCustomerCodeCtrl', eventType);

  }

  public setTransferCustomerCode(eventType: string) {
    HtmlUtil.nextFocusByName(this.elementRef, 'postalCodeCtrl', eventType);
  }

  public setShareTransferFee(eventType: string) {
    this.isRegistrationFeeButton = this.cmbShareTransferFeeCtrl.value.id == 1 ? false : true;
    this.cbxUseFeeLearningCtrl.setValue('');
    this.cbxUseFeeToleranceCtrl.setValue('');

    if (this.cmbShareTransferFeeCtrl.value.id == 1) {
      this.cbxUseFeeLearningCtrl.enable();
      this.cbxUseFeeToleranceCtrl.enable();
    } else {
      this.cbxUseFeeLearningCtrl.disable();
      this.cbxUseFeeToleranceCtrl.disable();
    }
  }
  
  public setPostalCode(eventType: string) {
    HtmlUtil.nextFocusByName(this.elementRef, 'address1Ctrl', eventType);
  }  

  public setAddress1(eventType: string) {
    HtmlUtil.nextFocusByName(this.elementRef, 'address2Ctrl', eventType);
  }  

  public setAddress2(eventType: string) {
    HtmlUtil.nextFocusByName(this.elementRef, 'telCtrl', eventType);
  }  

  public setTel(eventType: string) {
    HtmlUtil.nextFocusByName(this.elementRef, 'faxCtrl', eventType);
  }  

  public setFax(eventType: string) {
    HtmlUtil.nextFocusByName(this.elementRef, 'destinationDepartmentNameCtrl', eventType);
  }  
  
  public setDestinationDepartmentName(eventType: string) {
    HtmlUtil.nextFocusByName(this.elementRef, 'customerStaffNameCtrl', eventType);
  }  

  public setCustomerStaffName(eventType: string) {
    HtmlUtil.nextFocusByName(this.elementRef, 'honorificCtrl', eventType);
  }  

  public setHonorific(eventType: string) {
    HtmlUtil.nextFocusByName(this.elementRef, 'noteCtrl', eventType);
  }  
  

  /**
   * フォームの内容チェック
   */
  public checkContent(): boolean {
    let findIndex = this.categoryResult.categories.findIndex((item) => {
      return (item.code === "00");
    });
    if (0 <= findIndex) {
      if (this.cmbCollectCategoryIdCtrl.value != this.categoryResult.categories[findIndex].id) {
        return true;
      }
    }

    // 約定関係未入力チェック
    if (this.thresholdValueCtrl.value == undefined && this.collectCategoryCode == "00") {
      this.processModalCustomResult = this.processResultService.processAtWarning(
        this.processModalCustomResult, MSG_WNG.INPUT_REQUIRED.replace(MSG_ITEM_NUM.FIRST, '約定金額'),
        this.partsResultMessageComponent);
      HtmlUtil.nextFocusByName(this.elementRef, 'thresholdValueCtrl', EVENT_TYPE.NONE);
      return false;
    }
    else if (this.cmbLessThanCollectCategoryIdCtrl.value == undefined && this.collectCategoryCode == "00") {
      this.processModalCustomResult = this.processResultService.processAtWarning(
        this.processModalCustomResult, MSG_WNG.INPUT_REQUIRED.replace(MSG_ITEM_NUM.FIRST, '約定金額未満'),
        this.partsResultMessageComponent);
      HtmlUtil.nextFocusByName(this.elementRef, 'cmbLessThanCollectCategoryIdCtrl', EVENT_TYPE.NONE);
      return false;
    }
    else if (this.cmbGreaterThanCollectCategoryId1Ctrl.value == undefined && this.collectCategoryCode == "00") {
      this.processModalCustomResult = this.processResultService.processAtWarning(
        this.processModalCustomResult, MSG_WNG.INPUT_REQUIRED.replace(MSG_ITEM_NUM.FIRST, '約定金額以上①'),
        this.partsResultMessageComponent);
      HtmlUtil.nextFocusByName(this.elementRef, 'cmbGreaterThanCollectCategoryId1Ctrl', EVENT_TYPE.NONE);
      return false;
    }
    else if (this.cmbGreaterThanCollectCategoryId1Ctrl.value != undefined &&  this.greaterThanRate1Ctrl.value == undefined) {
      this.processModalCustomResult = this.processResultService.processAtWarning(
        this.processModalCustomResult, MSG_WNG.INPUT_REQUIRED.replace(MSG_ITEM_NUM.FIRST, '分割①'),
        this.partsResultMessageComponent);
      HtmlUtil.nextFocusByName(this.elementRef, 'greaterThanRate1Ctrl', EVENT_TYPE.NONE);
      return false;
    }
    else if (this.cmbGreaterThanCollectCategoryId1Ctrl.value != undefined && this.cmbGreaterThanRoundingMode1Ctrl.value == undefined) {
      this.processModalCustomResult = this.processResultService.processAtWarning(
        this.processModalCustomResult, MSG_WNG.INPUT_REQUIRED.replace(MSG_ITEM_NUM.FIRST, '端数単位'),
        this.partsResultMessageComponent);
      HtmlUtil.nextFocusByName(this.elementRef, 'cmbGreaterThanRoundingMode1Ctrl', EVENT_TYPE.NONE);
      return false;
    }
    else if (this.useLimitDate1 == 1 && (this.greaterThanSightOfBill1Ctrl.value == undefined || StringUtil.IsNullOrEmpty(this.greaterThanSightOfBill1Ctrl.value))) {
      this.processModalCustomResult = this.processResultService.processAtWarning(
        this.processModalCustomResult, MSG_WNG.INPUT_REQUIRED.replace(MSG_ITEM_NUM.FIRST, '回収サイト'),
        this.partsResultMessageComponent);
      HtmlUtil.nextFocusByName(this.elementRef, 'greaterThanSightOfBill1Ctrl', EVENT_TYPE.NONE);
      return false;
    }
    else if (this.cmbGreaterThanCollectCategoryId2Ctrl.enabled && this.cmbGreaterThanCollectCategoryId2Ctrl.value == undefined && this.collectCategoryCode == "00") {
      this.processModalCustomResult = this.processResultService.processAtWarning(
        this.processModalCustomResult, MSG_WNG.INPUT_REQUIRED.replace(MSG_ITEM_NUM.FIRST, '約定金額以上②'),
        this.partsResultMessageComponent);
      HtmlUtil.nextFocusByName(this.elementRef, 'cmbGreaterThanCollectCategoryId2Ctrl', EVENT_TYPE.NONE);
      return false;
    }
    else if (this.cmbGreaterThanCollectCategoryId2Ctrl.value != undefined &&  this.greaterThanRate2Ctrl.value == undefined) {
      this.processModalCustomResult = this.processResultService.processAtWarning(
        this.processModalCustomResult, MSG_WNG.INPUT_REQUIRED.replace(MSG_ITEM_NUM.FIRST, '分割②'),
        this.partsResultMessageComponent);
      HtmlUtil.nextFocusByName(this.elementRef, 'greaterThanRate2Ctrl', EVENT_TYPE.NONE);
      return false;
    }
    else if (this.cmbGreaterThanCollectCategoryId2Ctrl.value != undefined && this.cmbGreaterThanRoundingMode2Ctrl.value == undefined) {
      this.processModalCustomResult = this.processResultService.processAtWarning(
        this.processModalCustomResult, MSG_WNG.INPUT_REQUIRED.replace(MSG_ITEM_NUM.FIRST, '端数単位'),
        this.partsResultMessageComponent);
      HtmlUtil.nextFocusByName(this.elementRef, 'cmbGreaterThanRoundingMode2Ctrl', EVENT_TYPE.NONE);
      return false;
    }
    else if (this.useLimitDate2 == 1 && (this.greaterThanSightOfBill2Ctrl.value == undefined || StringUtil.IsNullOrEmpty(this.greaterThanSightOfBill2Ctrl.value))) {
      this.processModalCustomResult = this.processResultService.processAtWarning(
        this.processModalCustomResult, MSG_WNG.INPUT_REQUIRED.replace(MSG_ITEM_NUM.FIRST, '回収サイト'),
        this.partsResultMessageComponent);
      HtmlUtil.nextFocusByName(this.elementRef, 'greaterThanSightOfBill2Ctrl', EVENT_TYPE.NONE);
      return false;
    }
    else if (this.cmbGreaterThanCollectCategoryId3Ctrl.enabled && this.cmbGreaterThanCollectCategoryId3Ctrl.value == undefined && this.collectCategoryCode == "00") {
      this.processModalCustomResult = this.processResultService.processAtWarning(
        this.processModalCustomResult, MSG_WNG.INPUT_REQUIRED.replace(MSG_ITEM_NUM.FIRST, '約定金額以上③'),
        this.partsResultMessageComponent);
      HtmlUtil.nextFocusByName(this.elementRef, 'cmbGreaterThanCollectCategoryId3Ctrl', EVENT_TYPE.NONE);
      return false;
    }
    else if (this.cmbGreaterThanCollectCategoryId3Ctrl.value != undefined &&  this.greaterThanRate3Ctrl.value == undefined) {
      this.processModalCustomResult = this.processResultService.processAtWarning(
        this.processModalCustomResult, MSG_WNG.INPUT_REQUIRED.replace(MSG_ITEM_NUM.FIRST, '分割③'),
        this.partsResultMessageComponent);
      HtmlUtil.nextFocusByName(this.elementRef, 'greaterThanRate3Ctrl', EVENT_TYPE.NONE);
      return false;
    }
    else if (this.cmbGreaterThanCollectCategoryId3Ctrl.value != undefined && this.cmbGreaterThanRoundingMode3Ctrl.value == undefined) {
      this.processModalCustomResult = this.processResultService.processAtWarning(
        this.processModalCustomResult, MSG_WNG.INPUT_REQUIRED.replace(MSG_ITEM_NUM.FIRST, '端数単位'),
        this.partsResultMessageComponent);
      HtmlUtil.nextFocusByName(this.elementRef, 'cmbGreaterThanRoundingMode3Ctrl', EVENT_TYPE.NONE);
      return false;
    }
    else if (this.useLimitDate3 == 1 && (this.greaterThanSightOfBill3Ctrl.value == undefined || StringUtil.IsNullOrEmpty(this.greaterThanSightOfBill3Ctrl.value))) {
      this.processModalCustomResult = this.processResultService.processAtWarning(
        this.processModalCustomResult, MSG_WNG.INPUT_REQUIRED.replace(MSG_ITEM_NUM.FIRST, '回収サイト'),
        this.partsResultMessageComponent);
      HtmlUtil.nextFocusByName(this.elementRef, 'greaterThanSightOfBill3Ctrl', EVENT_TYPE.NONE);
      return false;
    }

    // 約定金額分割
    let total: number = 0;
    total = Number(this.greaterThanRate1Ctrl.value) + Number(this.greaterThanRate2Ctrl.value)
      + Number(this.greaterThanRate3Ctrl.value);
    if (0 < total && total != 100) {
      this.processModalCustomResult = this.processResultService.processAtWarning(
        this.processModalCustomResult, '約定金額以上の分割の値が合計「100」になるようにしてください', this.partsResultMessageComponent);
      return false;
    }

    if (this.cmbGreaterThanRoundingMode1Ctrl.enabled
     || this.cmbGreaterThanRoundingMode2Ctrl.enabled
     || this.cmbGreaterThanRoundingMode3Ctrl.enabled)
    {
      //約定検証処理を実装する
      if (!
          ((this.cmbGreaterThanRoundingMode1Ctrl.value == FRACTIONAL_UNIT[0].id && this.cmbGreaterThanRoundingMode2Ctrl.value != FRACTIONAL_UNIT[0].id && this.cmbGreaterThanRoundingMode3Ctrl.value != FRACTIONAL_UNIT[0].id)
        || (this.cmbGreaterThanRoundingMode1Ctrl.value != FRACTIONAL_UNIT[0].id && this.cmbGreaterThanRoundingMode2Ctrl.value == FRACTIONAL_UNIT[0].id && this.cmbGreaterThanRoundingMode3Ctrl.value != FRACTIONAL_UNIT[0].id)
        || (this.cmbGreaterThanRoundingMode1Ctrl.value != FRACTIONAL_UNIT[0].id && this.cmbGreaterThanRoundingMode2Ctrl.value != FRACTIONAL_UNIT[0].id && this.cmbGreaterThanRoundingMode3Ctrl.value == FRACTIONAL_UNIT[0].id))
         )
      {
        this.processModalCustomResult = this.processResultService.processAtWarning(
          this.processModalCustomResult, MSG_WNG.SELECT_ATLEAST_ONE_ROUNDING, this.partsResultMessageComponent);
        HtmlUtil.nextFocusByName(this.elementRef, 'cmbGreaterThanRoundingMode1Ctrl', EVENT_TYPE.NONE);
        return false;
      }
  
    }

    return true;
  }


  /**
   * フォームの内容をデータに設定する
   * @param customer 設定したいデータ
   */
  public setFormContentToData(customer: Customer) {
    // 基本設定
    customer.code = this.customerCodeCtrl.value;
    customer.name = this.customerNameCtrl.value;
    customer.kana = this.customerKanaCtrl.value;
    customer.isParent = this.cbxCustomerIsParentCtrl.value == true ? 1 : 0;
    customer.useKanaLearning = this.cbxUseKanaLearningCtrl.value == true ? 1 : 0;
    customer.useFeeLearning = this.cbxUseFeeLearningCtrl.value == true ? 1 : 0;
    customer.useFeeTolerance = this.cbxUseFeeToleranceCtrl.value == true ? 1 : 0;
    // customer.parentCustomerId
    customer.prioritizeMatchingIndividually = this.cbxPrioritizeMatchingIndividuallyCtrl.value == true ? 1 : 0;
    customer.excludeInvoicePublish = this.cbxExcludeInvoicePublishCtrl.value == true ? 1 : 0;
    customer.excludeReminderPublish = this.cbxExcludeReminderPublishCtrl.value == true ? 1 : 0;
    customer.shareTransferFee = this.cmbShareTransferFeeCtrl.value.id;
    customer.collationKey = this.collationKeyCtrl.value;
    customer.staffId = this.staffId;

    // 日付設定
    customer.closingDay = this.closingDayCtrl.value;
    customer.collectOffsetMonth = this.collectOffsetMonthCtrl.value;
    customer.collectOffsetDay = this.collectOffsetDayCtrl.value;
    customer.holidayFlag = this.cmbHolidayFlagCtrl.value;

    // 回収設定
    customer.collectCategoryId = this.cmbCollectCategoryIdCtrl.value;
    customer.collectCategoryCode = this.categoryResult.categories.find(x => x.id == customer.collectCategoryId).code;
    customer.sightOfBill = this.sightOfBillCtrl.value;
    customer.thresholdValue = this.thresholdValueCtrl.value;
    customer.lessThanCollectCategoryId = this.cmbLessThanCollectCategoryIdCtrl.value;
    customer.greaterThanCollectCategoryId1 = this.cmbGreaterThanCollectCategoryId1Ctrl.value;
    customer.greaterThanCollectCategoryId2 = this.cmbGreaterThanCollectCategoryId2Ctrl.value;
    customer.greaterThanCollectCategoryId3 = this.cmbGreaterThanCollectCategoryId3Ctrl.value;
    customer.greaterThanRate1 = this.greaterThanRate1Ctrl.value;
    customer.greaterThanRate2 = this.greaterThanRate2Ctrl.value;
    customer.greaterThanRate3 = this.greaterThanRate3Ctrl.value;
    customer.greaterThanRoundingMode1 = this.cmbGreaterThanRoundingMode1Ctrl.value;
    customer.greaterThanRoundingMode2 = this.cmbGreaterThanRoundingMode2Ctrl.value;
    customer.greaterThanRoundingMode3 = this.cmbGreaterThanRoundingMode3Ctrl.value;
    customer.greaterThanSightOfBill1 = this.greaterThanSightOfBill1Ctrl.value;
    customer.greaterThanSightOfBill2 = this.greaterThanSightOfBill2Ctrl.value;
    customer.greaterThanSightOfBill3 = this.greaterThanSightOfBill3Ctrl.value;

    // 債権管理
    customer.densaiCode = this.densaiCodeCtrl.value;
    customer.creditCode = this.creditCodeCtrl.value;
    customer.creditLimit = this.creditLimitCtrl.value;
    customer.creditRank = this.creditRankCtrl.value;

    // 専用入金口座(仮想口座)
    customer.exclusiveBankCode = this.exclusiveBankCodeCtrl.value;
    customer.exclusiveBankName = this.exclusiveBankNameCtrl.value;
    customer.exclusiveBranchCode = this.exclusiveBranchCodeCtrl.value;
    customer.exclusiveBranchName = this.exclusiveVirtualBranchNameCtrl.value;
    // 専用入金口座・仮想支店コードと専用入金口座・口座番号を結合
    if (this.exclusiveBranchCodeCtrl.value == undefined && this.exclusiveAccountNumberCtrl.value == undefined) {
      customer.exclusiveAccountNumber = null;
    } else {
      customer.exclusiveAccountNumber = this.exclusiveBranchCodeCtrl.value + this.exclusiveAccountNumberCtrl.value;
    }
    customer.exclusiveAccountTypeId = this.cmbExclusiveAccountTypeIdCtrl.value;

    // 口座振替用口座
    customer.transferBankCode = this.transferBankCodeCtrl.value;
    customer.transferBankName = this.transferBankNameCtrl.value;
    customer.transferBranchCode = this.transferBrachCodeCtrl.value;
    customer.transferBranchName = this.transferBankNameCtrl.value;
    customer.transferAccountNumber = this.transferAccountNumberCtrl.value;
    customer.transferAccountTypeId = this.transferAccountTypeIdCtrl.value;
    customer.transferNewCode = this.transferNewCodeCtrl.value;
    customer.transferAccountName = this.transferAccountNameCtrl.value;
    customer.transferCustomerCode = this.transferCustomerCodeCtrl.value;

    // 住所
    customer.postalCode = this.postalCodeCtrl.value;
    customer.address1 = this.address1Ctrl.value;
    customer.address2 = this.address2Ctrl.value;
    customer.tel = this.telCtrl.value;
    customer.fax = this.faxCtrl.value;
    customer.destinationDepartmentName = this.destinationDepartmentNameCtrl.value;
    customer.customerStaffName = this.customerStaffNameCtrl.value;
    customer.honorific = this.honorificCtrl.value;
    customer.note = this.noteCtrl.value;

    // 被振込口座設定
    customer.receiveAccountId1 = this.cbxReceiveAccountId1Ctrl.value?1:0;
    customer.receiveAccountId2 = this.cbxReceiveAccountId2Ctrl.value?1:0;
    customer.receiveAccountId3 = this.cbxReceiveAccountId3Ctrl.value?1:0;
  }

  /**
   * 数値をカンマ形式にする
   * @param formControl フォームコントロール
   */
  public separateValue(formControl: FormControl) {
    let num = NumberUtil.Separate(formControl.value);
    formControl.setValue(num);
  }

  public setThreshholdValue(formControl: FormControl, eventType: string) {
    this.separateValue(formControl);
    HtmlUtil.nextFocusByName(this.elementRef, 'cmbLessThanCollectCategoryIdCtrl', eventType);
  }

}
