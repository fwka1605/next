import { Component, OnInit, ComponentFactoryResolver, ViewContainerRef, ElementRef, TemplateRef, ViewChild } from '@angular/core';
import { BaseComponent } from 'src/app/component/common/base/base-component';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { TABLE_INDEX, TABLES } from 'src/app/common/const/table-name.const';
import { ModalMasterComponent } from 'src/app/component/modal/modal-master/modal-master.component';
import { HtmlUtil } from 'src/app/common/util/html.util';
import { KEY_CODE, BUTTON_ACTION, EVENT_TYPE } from 'src/app/common/const/event.const';
import { FormatterUtil } from 'src/app/common/util/formatter.util';
import { CategoriesResult } from 'src/app/model/categories-result.model';
import { CategoryMasterService } from 'src/app/service/Master/category-master.service';
import { CATEGORY_TYPE_DICTIONARY, TAX_CLASS_TYPE_DICTIONARY, CODE_TYPE } from 'src/app/common/const/kbn.const';
import { UserInfoService } from 'src/app/service/common/user-info.service';
import { CustomValidators } from 'ng5-validation';
import { COMPONENT_STATUS_TYPE, MODAL_STATUS_TYPE, PROCESS_RESULT_RESULT_TYPE } from 'src/app/common/const/status.const';
import { Category } from 'src/app/model/category.model';
import { forkJoin } from 'rxjs';
import { ModalConfirmComponent } from 'src/app/component/modal/modal-confirm/modal-confirm.component';
import { FileUtil } from 'src/app/common/util/file.util';
import { BankAccountMasterService } from 'src/app/service/Master/bank-account-master.service';
import { CustomerMasterService } from 'src/app/service/Master/customer-master.service';
import { CustomerPaymentContractMasterService } from 'src/app/service/Master/customer-payment-contract-master.service';
import { IgnoreKanaMasterService } from 'src/app/service/Master/ignore-kana-master.service';
import { ReceiptService } from 'src/app/service/receipt.service';
import { NettingMasterService } from 'src/app/service/Master/netting-master.service';
import { BillingService } from 'src/app/service/billing.service';
import { ProcessResultService } from 'src/app/service/common/process-result.service';
import { ExistDeleteResult } from 'src/app/model/custom-model/process-result-custom.model';
import { Router, ActivatedRoute, NavigationEnd } from '@angular/router';
import { PageUtil } from 'src/app/common/util/page-util';
import { AccountTitleMasterService } from 'src/app/service/Master/account-title-master.service';
import { StringUtil } from 'src/app/common/util/string-util';
import { PaymentAgencyMasterService } from 'src/app/service/Master/payment-agency-master.service';
import { ModalRouterProgressComponent } from 'src/app/component/modal/modal-router-progress/modal-router-progress.component';
import { MatAutocompleteTrigger, MatExpansionPanel } from '@angular/material';

@Component({
  selector: 'app-pb0901-category-master',
  templateUrl: './pb0901-category-master.component.html',
  styleUrls: ['./pb0901-category-master.component.css']
})
export class Pb0901CategoryMasterComponent extends BaseComponent implements OnInit {

  public panelOpenState;

  /** カテゴリデータ */
  public categoriesResult: CategoriesResult;
  /** 選択した行のデータ */
  public categoryData: Category;

  /** セレクト選択肢 */
  public readonly categoryTypeDictionary = CATEGORY_TYPE_DICTIONARY;
  public readonly taxClassTypeDictionary = TAX_CLASS_TYPE_DICTIONARY;

  /** テンプレート */
  public thenBlock: TemplateRef<any> = null;
  public show: boolean = true;

  /** 区分種別 */
  public categoryTypeCtrl: FormControl;
  /** 区分コード */
  public categoryCodeCtrl: FormControl;
  /** 区分名 */
  public categoryNameCtrl: FormControl;
  /** 科目コード */
  public accountTitleCodeCtrl: FormControl;
  /** 科目名 */
  public accountTitleNameCtrl: FormControl;
  /** 補助コード */
  public categorySubCodeCtrl: FormControl;
  /** 外部コード */
  public externalCodeCtrl: FormControl;
  /** 備考 */
  public noteCtrl: FormControl;
  /** 入力時に使用する */
  public cbxUseInputCtrl: FormControl;
  /** 消費税属性 */
  public cmbTaxClassNameCtrl: FormControl;
  /** 消込順序 */
  public matchingOrderCtrl: FormControl;
  /** 長前管理を行う */
  public cbxUseLongTermAdvanceReceivedCtrl: FormControl;
  /** 歩引計算を行う */
  public cbxUseDiscountCtrl: FormControl;
  /** 一括消込対象外 */
  public cbxForeceMatchingIndividuallyCtrl: FormControl;
  /** 期日入力を行う */
  public cbxUseLimitDate: FormControl;
  /** 期日入金管理を行う */
  public cbxUseCashOnDueDates: FormControl;
  /** 口座振替を行う */
  public cbxUseAccountTransfer: FormControl;
  /** 決済代行会社コード */
  public paymentAgencyCodeCtrl: FormControl;
  /** 決済代行会社名 */
  public paymentAgencyNameCtrl: FormControl;

  /** 科目ID */
  public accountTitleId: number;
  /**　決済会社ID */
  public paymentAgencyId: number;

  /** 入力時に使用する */
  public isUseInputDisplay: boolean = false;
  /** 消費税属性 */
  public isTaxClassNameDisplay: boolean = false;
  /** 消込順序 */
  public isMatchingOrderDisplay: boolean = false;
  /** 長前管理を行う */
  public isUseLongTermAdvanceReceivedDisplay: boolean = false;
  /** 歩引計算を行う */
  public isUseDiscountDisplay: boolean = false;
  /** 一括消込対象外 */
  public isForeceMatchingIndividuallyDisplay: boolean = false;
  /** 期日入力を行う */
  public isUseLimitDate: boolean = false;
  /** 期日入金管理を行う */
  public isUseCashOnDueDates: boolean = false;
  /** 口座振替を行う */
  public isUseAccountTransfer: boolean = false;
  /** 決済代行会社 */
  public isPaymentAgencyDisplay: boolean = false;

  public categoryType: number = -1;

  @ViewChild('primaryBilling') primaryBilling: TemplateRef<any> = null;
  @ViewChild('primaryReceipt') primaryReceipt: TemplateRef<any> = null;
  @ViewChild('primaryCollection') primaryCollection: TemplateRef<any> = null;
  @ViewChild('primaryExclude') primaryExclude: TemplateRef<any> = null;

  @ViewChild('accountTitleCodeInput', { read: MatAutocompleteTrigger }) accountTitleCodeTrigger: MatAutocompleteTrigger;
  @ViewChild('paymentAgencyCodeInput', { read: MatAutocompleteTrigger }) paymentAgencyCodeigger: MatAutocompleteTrigger;

  @ViewChild('panel') panel: MatExpansionPanel;

  constructor(
    public router: Router,
    public activatedRoute: ActivatedRoute,
    public elementRef: ElementRef,
    public componentFactoryResolver: ComponentFactoryResolver,
    public viewContainerRef: ViewContainerRef,
    public userInfoService: UserInfoService,
    public categoryService: CategoryMasterService,
    public bankAccountService: BankAccountMasterService,
    public customerService: CustomerMasterService,
    public customerPaymentContractService: CustomerPaymentContractMasterService,
    public ignoreKanaService: IgnoreKanaMasterService,
    public billingService: BillingService,
    public receiptService: ReceiptService,
    public nettingService: NettingMasterService,
    public processResultService: ProcessResultService,
    public accountTitleService: AccountTitleMasterService,
    public paymentAgencyService: PaymentAgencyMasterService

  ) {
    super();

    router.events.subscribe(event => {
      if (event instanceof NavigationEnd) {
        this.Title = PageUtil.GetTitle(router.routerState, router.routerState.root).join('');
        this.ComponentId = parseInt(PageUtil.GetComponentId(router.routerState, router.routerState.root).join(''));
        const path: string[] = PageUtil.GetPath(this.activatedRoute.snapshot.pathFromRoot);
        this.Path = path[1];
        this.processCustomResult = this.processResultService.processResultInit(this.ComponentId, this.Title);
      }
    });
  }

  ngOnInit() {
    this.setControlInit();
    this.setValidator();
    this.setFormatter();
    this.clear();
    this.setAutoComplete();
    this.thenBlock = this.primaryBilling;
  }

  onReceiveEventFromChild(eventData: Category) {
    this.panelOpenState = true;
    this.panel.open();
    this.categoryData = eventData;
    this.setFormValue();
  }

  public setControlInit() {
    this.categoryTypeCtrl = new FormControl('', [Validators.required]);
    this.categoryCodeCtrl = new FormControl('', [Validators.required, CustomValidators.number]);
    this.categoryNameCtrl = new FormControl('', [Validators.required]);
    this.accountTitleCodeCtrl = new FormControl('', [Validators.maxLength(this.userInfoService.ApplicationControl.accountTitleCodeLength)]);
    this.accountTitleNameCtrl = new FormControl('');
    this.categorySubCodeCtrl = new FormControl('');
    this.cmbTaxClassNameCtrl = new FormControl('', [Validators.required]);
    this.matchingOrderCtrl = new FormControl('', [Validators.required]);
    this.externalCodeCtrl = new FormControl('');
    this.noteCtrl = new FormControl('');
    this.paymentAgencyCodeCtrl = new FormControl('', [CustomValidators.number]);
    this.paymentAgencyNameCtrl = new FormControl('');
    this.cbxForeceMatchingIndividuallyCtrl = new FormControl('');
    this.cbxUseLimitDate = new FormControl('');
    this.cbxUseCashOnDueDates = new FormControl('');
    this.cbxUseAccountTransfer = new FormControl('');
    this.cbxUseInputCtrl = new FormControl('');
    this.cbxUseLongTermAdvanceReceivedCtrl = new FormControl('');
    this.cbxUseDiscountCtrl = new FormControl('');
  }

  public setValidator() {
    this.MyFormGroup = new FormGroup({
      categoryTypeCtrl: this.categoryTypeCtrl,
      categoryCodeCtrl: this.categoryCodeCtrl,
      categoryNameCtrl: this.categoryNameCtrl,
      accountTitleCodeCtrl: this.accountTitleCodeCtrl,
      accountTitleNameCtrl: this.accountTitleNameCtrl,
      categorySubCodeCtrl: this.categorySubCodeCtrl,
      cmbTaxClassNameCtrl: this.cmbTaxClassNameCtrl,
      matchingOrderCtrl: this.matchingOrderCtrl,
      externalCodeCtrl: this.externalCodeCtrl,
      noteCtrl: this.noteCtrl,
      paymentAgencyCodeCtrl: this.paymentAgencyCodeCtrl,
      paymentAgencyNameCtrl: this.paymentAgencyNameCtrl,
      cbxForeceMatchingIndividuallyCtrl: this.cbxForeceMatchingIndividuallyCtrl,
      cbxUseLimitDate: this.cbxUseLimitDate,
      cbxUseCashOnDueDates: this.cbxUseCashOnDueDates,
      cbxUseAccountTransfer: this.cbxUseAccountTransfer,
      cbxUseInputCtrl: this.cbxUseInputCtrl,
      cbxUseLongTermAdvanceReceivedCtrl: this.cbxUseLongTermAdvanceReceivedCtrl,
      cbxUseDiscountCtrl: this.cbxUseDiscountCtrl,
    })
  }

  public setFormatter() {
    FormatterUtil.setNumberFormatter(this.categoryCodeCtrl);
    if (this.userInfoService.ApplicationControl.accountTitleCodeType == CODE_TYPE.NUMBER) {
      FormatterUtil.setNumberFormatter(this.accountTitleCodeCtrl);
    }
    else {
      FormatterUtil.setCodeFormatter(this.accountTitleCodeCtrl);
    }
    FormatterUtil.setCodeFormatter(this.categorySubCodeCtrl);
    FormatterUtil.setNumberFormatter(this.matchingOrderCtrl);
    FormatterUtil.setCodeFormatter(this.externalCodeCtrl);
  }

  public clear() {
    this.MyFormGroup.reset();
    if (0 < this.categoryType) {
      this.categoryCodeCtrl.setValue(this.categoryType);
      this.setCategoryType(this.categoryType);
    }
    this.accountTitleId = null;
    this.paymentAgencyId = null;
    this.ComponentStatus = COMPONENT_STATUS_TYPE.CREATE;
    this.categoryCodeCtrl.enable();
  }

  public setAutoComplete() {
    this.initautocompleteAccountTitle(this.accountTitleCodeCtrl, this.accountTitleService, 0);
    this.initAutocompletePaymentAgencies(this.paymentAgencyCodeCtrl, this.paymentAgencyService, 0);
  }

  /**
   * 各テーブルのデータを取得
   * @param table テーブル番号
   * @param keyCode キーイベント
   */
  public openMasterModal(table: TABLE_INDEX) {
    if (this.paymentAgencyCodeigger != null) { this.paymentAgencyCodeigger.closePanel(); }

    if (this.accountTitleCodeTrigger != null) { this.accountTitleCodeTrigger.closePanel(); }

    let componentFactory = this.componentFactoryResolver.resolveComponentFactory(ModalMasterComponent);
    let componentRef = this.viewContainerRef.createComponent(componentFactory);
    componentRef.instance.TableIndex = table;

    componentRef.instance.Closing.subscribe(() => {
      if (componentRef.instance.ModalStatus == MODAL_STATUS_TYPE.SELECT) {
        switch (table) {

          case TABLE_INDEX.MASTER_ACCOUNT_TITLE:
            {
              // 科目コードマスタ
              this.accountTitleId = componentRef.instance.SelectedObject.id;
              this.accountTitleCodeCtrl.setValue(componentRef.instance.SelectedCode);
              this.accountTitleNameCtrl.setValue(componentRef.instance.SelectedName);
              break;
            }
          case TABLE_INDEX.MASTER_PAYMENT_AGENCY:
            {
              // 銀行・支店マスタ
              this.paymentAgencyId = componentRef.instance.SelectedObject.id;
              this.paymentAgencyCodeCtrl.setValue(componentRef.instance.SelectedCode);
              this.paymentAgencyNameCtrl.setValue(componentRef.instance.SelectedName);
              break;
            }
        }
      }
      componentRef.destroy();
    });
  }

  /**
   * 選択した行のデータをフォームに表示
   * @param index 行番号
   */
  public setFormValue() {
    let categoryItem = this.categoryData;

    this.categoryCodeCtrl.setValue(categoryItem.code); // 区分
    this.categoryNameCtrl.setValue(categoryItem.name);
    this.accountTitleCodeCtrl.setValue(categoryItem.accountTitleCode); // 科目
    this.accountTitleNameCtrl.setValue(categoryItem.accountTitleName);
    this.categorySubCodeCtrl.setValue(categoryItem.subCode); // 補助コード
    this.cmbTaxClassNameCtrl.setValue(categoryItem.taxClassId); // 消費税属性
    this.matchingOrderCtrl.setValue(categoryItem.matchingOrder); // 消込順序
    this.externalCodeCtrl.setValue(categoryItem.externalCode); // 外部コード
    this.noteCtrl.setValue(categoryItem.note); // 備考
    this.paymentAgencyCodeCtrl.setValue(categoryItem.paymentAgencyCode); // 決済代行会社
    this.paymentAgencyNameCtrl.setValue(categoryItem.paymentAgencyName);
    this.cbxForeceMatchingIndividuallyCtrl.setValue(categoryItem.forceMatchingIndividually); // 一括消込対象外
    this.cbxUseLimitDate.setValue(categoryItem.useLimitDate);
    this.cbxUseCashOnDueDates.setValue(categoryItem.useCashOnDueDates);
    this.cbxUseAccountTransfer.setValue(categoryItem.useAccountTransfer);
    this.cbxUseInputCtrl.setValue(categoryItem.useInput); // 入力時に使用する
    this.cbxUseLongTermAdvanceReceivedCtrl.setValue(categoryItem.useLongTermAdvanceReceived); // 長前管理を行う
    this.cbxUseDiscountCtrl.setValue(categoryItem.useDiscount); // 歩引計算を行う

    this.ComponentStatus = COMPONENT_STATUS_TYPE.UPDATE;
    this.processResultService.clearProcessCustomMsg(this.processCustomResult);

    this.categoryCodeCtrl.disable();
  }

  /**
   * データ取得
   * @param eventType イベント種別
   */
  public setCategoryType(categoryId:number=-1) {
    this.panelOpenState = false;
    this.panel.close();
    
    if (categoryId < 0){
      categoryId = this.categoryTypeCtrl.value.id;
      this.categoryType = this.categoryTypeCtrl.value.id;
    }
    this.categoryTypeCtrl.setValue(this.categoryTypeDictionary[categoryId - 1]);

    switch (categoryId) {
      case 1:
        this.thenBlock = this.primaryBilling;
        this.isUseInputDisplay = true;
        this.isTaxClassNameDisplay = true;
        this.isMatchingOrderDisplay = true;
        if (this.userInfoService.ApplicationControl.useLongTermAdvanceReceived == 1) {
          this.isUseLongTermAdvanceReceivedDisplay = true;
        }
        if (this.userInfoService.ApplicationControl.useDiscount == 1) {
          this.isUseDiscountDisplay = true;
        }
        this.isForeceMatchingIndividuallyDisplay = false;
        this.isUseLimitDate = false;
        this.isUseCashOnDueDates = false;
        this.isUseAccountTransfer = false;
        this.isPaymentAgencyDisplay = false;
        break;

      case 2:
        this.thenBlock = this.primaryReceipt;
        this.isUseInputDisplay = true;
        this.isTaxClassNameDisplay = true;
        this.isMatchingOrderDisplay = true;
        this.isUseLongTermAdvanceReceivedDisplay = false;
        this.isUseDiscountDisplay = false;
        this.isForeceMatchingIndividuallyDisplay = true;
        this.isUseLimitDate = true;
        if (this.userInfoService.ApplicationControl.useCashOnDueDates == 1) {
          this.isUseCashOnDueDates = true;
        }
        this.isUseAccountTransfer = false;
        this.isPaymentAgencyDisplay = false;
        break;

      case 3:
        this.thenBlock = this.primaryCollection;
        this.isUseInputDisplay = true;
        this.isTaxClassNameDisplay = false;
        this.isMatchingOrderDisplay = false;
        this.isUseLongTermAdvanceReceivedDisplay = false;
        this.isUseDiscountDisplay = false;
        this.isForeceMatchingIndividuallyDisplay = false;
        this.isUseLimitDate = true;
        this.isUseCashOnDueDates = false;
        if (this.userInfoService.ApplicationControl.useAccountTransfer == 1) {
          this.isUseAccountTransfer = true;
          this.isPaymentAgencyDisplay = true;
        }

        // 項目がないが必須設定のため回避する
        this.cmbTaxClassNameCtrl.setValidators(null);
        this.matchingOrderCtrl.setValue(9999);

        break;

      case 4:
        this.thenBlock = this.primaryExclude;
        this.isUseInputDisplay = false;
        this.isTaxClassNameDisplay = true;
        this.isMatchingOrderDisplay = false;
        this.isUseLongTermAdvanceReceivedDisplay = false;
        this.isUseDiscountDisplay = false;
        this.isForeceMatchingIndividuallyDisplay = false;
        this.isUseLimitDate = false;
        this.isUseCashOnDueDates = false;
        this.isUseAccountTransfer = false;
        this.isPaymentAgencyDisplay = false;

        // 項目がないが必須設定のため回避する
        this.matchingOrderCtrl.setValue(9999);

        break;
    }

    this.categoryService.GetItemsByCategoryType(categoryId)
      .subscribe(result => {
        this.processCustomResult = this.processResultService.processAtGetData(
          this.processCustomResult, result, false, this.partsResultMessageComponent);
        if (this.processCustomResult.result == PROCESS_RESULT_RESULT_TYPE.SUCCESS) {
          this.categoriesResult = new CategoriesResult();
          this.categoriesResult.categories = new Array<Category>();
          this.categoriesResult.categories = result;
        }
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
    let registryData = new Category();
    let isRegistry: boolean = false;

    if (this.ComponentStatus == COMPONENT_STATUS_TYPE.CREATE) {
      registryData.id = 0;
      registryData.createBy = this.userInfoService.LoginUser.id;
      registryData.categoryType = this.categoryTypeCtrl.value.id;
      isRegistry = true;

    } else if (this.ComponentStatus == COMPONENT_STATUS_TYPE.UPDATE) {
      registryData = this.categoryData;

    } else {
      return;
    }

    let processComponentFactory = this.componentFactoryResolver.resolveComponentFactory(ModalRouterProgressComponent);
    let processComponentRef = this.viewContainerRef.createComponent(processComponentFactory);
    // processComponentRef.destroy();   

    registryData.code = this.categoryCodeCtrl.value;
    registryData.name = this.categoryNameCtrl.value;
    registryData.accountTitleId = this.accountTitleId;
    registryData.subCode = this.categorySubCodeCtrl.value;
    registryData.externalCode = this.externalCodeCtrl.value;
    registryData.useLongTermAdvanceReceived = this.cbxUseLongTermAdvanceReceivedCtrl.value == true ? 1 : 0;
    registryData.useDiscount = this.cbxUseDiscountCtrl.value == true ? 1 : 0;
    registryData.forceMatchingIndividually = this.cbxForeceMatchingIndividuallyCtrl.value == true ? 1 : 0;
    registryData.useLimitDate = this.cbxUseLimitDate.value == true ? 1 : 0;
    registryData.useCashOnDueDates = this.cbxUseCashOnDueDates.value == true ? 1 : 0;
    registryData.useAccountTransfer = this.cbxUseAccountTransfer.value == true ? 1 : 0;
    registryData.useInput = this.cbxUseInputCtrl.value == true ? 1 : 0;
    registryData.taxClassId = this.cmbTaxClassNameCtrl.value;
    registryData.matchingOrder = this.matchingOrderCtrl.value == 9999 ? '' : this.matchingOrderCtrl.value;
    registryData.note = this.noteCtrl.value;
    registryData.paymentAgencyId = this.paymentAgencyId;
    registryData = FileUtil.replaceNull(registryData);

    this.categoryService.Save(registryData)
      .subscribe(result => {
        this.processCustomResult = this.processResultService.processAtSave(
          this.processCustomResult, result, isRegistry, this.partsResultMessageComponent);
        if (this.processCustomResult.result == PROCESS_RESULT_RESULT_TYPE.SUCCESS) {
          this.setCategoryType(this.categoryTypeCtrl.value.id);
        }
        processComponentRef.destroy();
      });
  }

  /**
   * データ削除
   */
  public delete() {
    let processComponentFactory = this.componentFactoryResolver.resolveComponentFactory(ModalRouterProgressComponent);
    let processComponentRef = this.viewContainerRef.createComponent(processComponentFactory);

    let category = this.categoryData;
    let existDeleteResultList = new Array<ExistDeleteResult>();

    for (let i = 0; i < 9; i++) {
      existDeleteResultList[i] = new ExistDeleteResult();
      existDeleteResultList[i].idName = '区分コード';
    }

    let existBankAccountResponse = this.bankAccountService.ExistCategory(category.id);
    existDeleteResultList[0].masterName = TABLES.MASTER_BANK_ACCOUNT.name;

    let existCustomerResponse = this.customerService.ExistCategory(category.id);
    existDeleteResultList[1].masterName = TABLES.MASTER_CUSTOMER.name;

    let existCustomerPaymentContractResponse = this.customerPaymentContractService.ExistCategory(category.id);
    existDeleteResultList[2].masterName = TABLES.MASTER_CUSTOMER_PAYMENT_CONTRACT.name;

    let existIgnoreKanaResponse = this.ignoreKanaService.ExistCategory(category.id);
    existDeleteResultList[3].masterName = TABLES.MASTER_IGNORE_KANA.name;

    let existBillingCategoryResponse = this.billingService.ExistBillingCategory(category.id);
    existDeleteResultList[4].masterName = TABLES.BILLING.name;

    let existBillingCollectResponse = this.billingService.ExistCollectCategory(category.id);
    existDeleteResultList[5].masterName = TABLES.BILLING.name;

    let existReceiptCategoryResponse = this.receiptService.ExistReceiptCategory(category.id);
    existDeleteResultList[6].masterName = TABLES.RECEIPT.name;

    let existReceiptExcludeCategoryResponse = this.receiptService.ExistExcludeCategory(category.id);
    existDeleteResultList[7].masterName = TABLES.RECEIPT.name;

    let existNettingResponse = this.nettingService.ExistReceiptCategory(category.id)
    existDeleteResultList[7].masterName = TABLES.MASTER_NETTING.name;

    // 処理の待機
    forkJoin(
      existBankAccountResponse,
      existCustomerResponse,
      existCustomerPaymentContractResponse,
      existIgnoreKanaResponse,
      existBillingCategoryResponse,
      existBillingCollectResponse,
      existReceiptCategoryResponse,
      existReceiptExcludeCategoryResponse,
      existNettingResponse
    ).subscribe(
      responseList => {
        processComponentRef.destroy();   
        this.processCustomResult = this.processResultService.processAtExist(
          this.processCustomResult, responseList, existDeleteResultList, this.partsResultMessageComponent);
        if (this.processCustomResult.result == PROCESS_RESULT_RESULT_TYPE.WARNING) {
          return;
        }

        // 削除処理
        let componentFactory = this.componentFactoryResolver.resolveComponentFactory(ModalConfirmComponent);
        let componentRef = this.viewContainerRef.createComponent(componentFactory);
        componentRef.instance.ActionName = "削除"
        componentRef.instance.Closing.subscribe(() => {

          if (componentRef.instance.ModalStatus == MODAL_STATUS_TYPE.OK) {

            let processComponentFactory = this.componentFactoryResolver.resolveComponentFactory(ModalRouterProgressComponent);
            let processComponentRef = this.viewContainerRef.createComponent(processComponentFactory);
            // processComponentRef.destroy();   

            this.categoryService.Delete(category)
              .subscribe(result => {
                this.processCustomResult = this.processResultService.processAtDelete(
                  this.processCustomResult, result, this.partsResultMessageComponent);
                if (this.processCustomResult.result == PROCESS_RESULT_RESULT_TYPE.SUCCESS) {
                  this.setCategoryType(this.categoryTypeCtrl.value.id);
                }
              });
            processComponentRef.destroy();
          }
          componentRef.destroy();
        });
      },
      error => {
        console.log(error)
      }
    );
  }


  ///// Enter キー押下時の処理 /////////////////////////////////////////////////////
  public setCategoryCode(eventType: string) {
    if (StringUtil.IsNullOrEmpty(this.categoryTypeCtrl.value)) return;
    if (!StringUtil.IsNullOrEmpty(this.categoryCodeCtrl.value)) {
      this.categoryService.GetItems(this.categoryTypeCtrl.value.id, this.categoryCodeCtrl.value)
        .subscribe(response => {
          if (response != undefined && response != null && 0 < response.length) {
            this.categoryData = response[0];
            this.setFormValue();
          } else {
            HtmlUtil.nextFocusByName(this.elementRef, 'categoryNameCtrl', eventType);
          }
        });
    }
  }

  public setCategoryName(eventType: string) {
    HtmlUtil.nextFocusByName(this.elementRef, 'accountTitleCodeCtrl', eventType);
  }

  ///////////////////////////////////////////////////////////

  public setAccountTitleCode(eventType: string) {

    if (eventType != EVENT_TYPE.BLUR) {
      if (this.accountTitleCodeTrigger != null) { this.accountTitleCodeTrigger.closePanel(); }
    }

    this.accountTitleId = null;

    if (!this.StringUtil.IsNullOrEmpty(this.accountTitleCodeCtrl.value)) {

      this.loadStart();
      this.accountTitleService.Get(this.accountTitleCodeCtrl.value)
        .subscribe(response => {
          this.loadEnd();
          if (response != PROCESS_RESULT_RESULT_TYPE.FAILURE && response.length > 0) {
            this.accountTitleId = response[0].id;
            this.accountTitleCodeCtrl.setValue(response[0].code);
            this.accountTitleNameCtrl.setValue(response[0].name);
          } else {
            //this.accountTitleCodeCtrl.setValue("");
            this.accountTitleNameCtrl.setValue("");
          }
          HtmlUtil.nextFocusByName(this.elementRef, 'categorySubCodeCtrl', eventType);
        });
    }
    else {
      this.accountTitleCodeCtrl.setValue("");
      this.accountTitleNameCtrl.setValue("");
    }
    HtmlUtil.nextFocusByName(this.elementRef, 'categorySubCodeCtrl', eventType);
  }

  public setCategorySubCode(eventType: string) {
    if (!StringUtil.IsNullOrEmpty(this.categorySubCodeCtrl.value)) {
      this.categorySubCodeCtrl.setValue(StringUtil.setUpperCase(this.categorySubCodeCtrl.value));
    }
    HtmlUtil.nextFocusByName(this.elementRef, 'cmbTaxClassNameCtrl', eventType);
  }

  public setCmbTaxClassName(eventType: string) {
    HtmlUtil.nextFocusByName(this.elementRef, 'matchingOrderCtrl', eventType);
  }

  public setMatchingOrder(eventType: string) {
    HtmlUtil.nextFocusByName(this.elementRef, 'externalCodeCtrl', eventType);
  }

  public setExternalCode(eventType: string) {
    HtmlUtil.nextFocusByName(this.elementRef, 'noteCtrl', eventType);
  }

  public setNote(eventType: string) {
    HtmlUtil.nextFocusByName(this.elementRef, 'paymentAgencyCodeCtrl', eventType);
  }

  public setPaymentAgencyCode(eventType: string) {
    this.paymentAgencyId = null;
    if (eventType != EVENT_TYPE.BLUR) {
      if (this.paymentAgencyCodeigger != null) { this.paymentAgencyCodeigger.closePanel(); }
    }
    if (!StringUtil.IsNullOrEmpty(this.paymentAgencyCodeCtrl.value)) {
      this.loadStart();
      this.paymentAgencyService.GetItemsByCode(this.paymentAgencyCodeCtrl.value)
        .subscribe(response => {
          this.loadEnd();
          if (response != undefined && response != null && 0 < response.length) {
            this.paymentAgencyId = response[0].id;
            this.paymentAgencyCodeCtrl.setValue(response[0].code);
            this.paymentAgencyNameCtrl.setValue(response[0].name);
            HtmlUtil.nextFocusByName(this.elementRef, 'categoryCodeCtrl', eventType);
          } else {
            // this.paymentAgencyCodeCtrl.setValue("");
            this.paymentAgencyNameCtrl.setValue("");
          }
        });
    } else {
      // this.paymentAgencyCodeCtrl.setValue("");
      this.paymentAgencyNameCtrl.setValue("");
      HtmlUtil.nextFocusByName(this.elementRef, 'categoryCodeCtrl', eventType);
    }
  }

}
