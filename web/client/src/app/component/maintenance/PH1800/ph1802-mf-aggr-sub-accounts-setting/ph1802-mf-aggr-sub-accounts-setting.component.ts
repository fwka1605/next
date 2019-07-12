import { Component, OnInit, ComponentFactoryResolver, ViewContainerRef, ElementRef, ViewChild } from '@angular/core';
import { BaseComponent } from 'src/app/component/common/base/base-component';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { TABLE_INDEX } from 'src/app/common/const/table-name.const';
import { ModalMasterComponent } from 'src/app/component/modal/modal-master/modal-master.component';
import { HtmlUtil } from 'src/app/common/util/html.util';
import { BUTTON_ACTION, EVENT_TYPE } from 'src/app/common/const/event.const';
import { FormatterUtil } from 'src/app/common/util/formatter.util';
import { UserInfoService } from 'src/app/service/common/user-info.service';
import { CustomValidators } from 'ng5-validation';
import { COMPONENT_STATUS_TYPE, MODAL_STATUS_TYPE, PROCESS_RESULT_RESULT_TYPE } from 'src/app/common/const/status.const';
import { CategoryType } from 'src/app/common/const/kbn.const';
import { ProcessResultService } from 'src/app/service/common/process-result.service';
import { Router, ActivatedRoute, ParamMap, NavigationEnd } from '@angular/router';
import { PageUtil } from 'src/app/common/util/page-util';
import { StringUtil } from 'src/app/common/util/string-util';
import { CategoryMasterService } from 'src/app/service/Master/category-master.service';
import { Category } from 'src/app/model/category.model';
import { SectionMasterService } from 'src/app/service/Master/section-master.service';
import { Section } from 'src/app/model/section.model';
import { ModalRouterProgressComponent } from 'src/app/component/modal/modal-router-progress/modal-router-progress.component';
import { MatAutocompleteTrigger } from '@angular/material';
import { forkJoin } from 'rxjs';
import { MfAggrMasterService } from 'src/app/service/Master/mf-aggr-master.service'
import { MfAggrAccount } from 'src/app/model/mf-aggr-account.model';
import { MfAggrSubAccount } from 'src/app/model/mf-aggr-sub-account.model';
import { CategoriesResult } from 'src/app/model/categories-result.model';
import { SectionResult } from 'src/app/model/section-result.model';
import { ComponentId } from 'src/app/common/const/component-name.const';


@Component({
  selector: 'app-ph1802-mf-aggr-sub-accounts-setting',
  templateUrl: './ph1802-mf-aggr-sub-accounts-setting.component.html',
  styleUrls: ['./ph1802-mf-aggr-sub-accounts-setting.component.css']
})
export class Ph1802MfAggrSubAccountsSettingComponent extends BaseComponent implements OnInit {
  
  public paramFrom: ComponentId;

  public accountNameCtrl: FormControl;          // 金融機関名
  public subAccountNameCtrl: FormControl;       // 支店名
  public accountTypeNameCtrl: FormControl;      // 口座種別
  public accountNumberCtrl: FormControl;        // 口座番号 ← old
  public bankCodeCtrl: FormControl;             // 銀行コード ← old
  public branchCodeCtrl: FormControl;           // 支店コード ← old
  public receiptCategoryCodeCtrl: FormControl;  // 入金区分コード ← old
  public receiptCategoryNameCtrl: FormControl;
  public sectionCodeCtrl: FormControl;          // 入金部門
  public sectionNameCtrl: FormControl;

  public selectIndex: number;
  public sectionId: number;
  public receiptCategoryId: number;

  public categoriesResult: CategoriesResult;
  public sectionResult: SectionResult;
  
  public VOneAccounts: Array<MfAggrAccount>;
  public accountDetails = new Array<AccountDetailModel>();


  @ViewChild('sectionCodeInput', { read: MatAutocompleteTrigger }) sectionCodeTrigger: MatAutocompleteTrigger;
  @ViewChild('receiptCategoryCodeInput', { read: MatAutocompleteTrigger }) receiptCategoryCodeTrigger: MatAutocompleteTrigger;


  constructor(
    public router: Router,
    public activatedRoute: ActivatedRoute,
    public elementRef: ElementRef,
    public componentFactoryResolver: ComponentFactoryResolver,
    public viewContainerRef: ViewContainerRef,
    public userInfoService: UserInfoService,
    public processResultService: ProcessResultService,
    public categoryService: CategoryMasterService,
    public sectionService: SectionMasterService,
    public mfAggrMasterService: MfAggrMasterService

  ) {
    super();

    router.events.subscribe(event => {
      if (event instanceof NavigationEnd) {
        this.Title = PageUtil.GetTitle(router.routerState, router.routerState.root).join('');
        this.ComponentId = parseInt(PageUtil.GetComponentId(router.routerState, router.routerState.root).join(''));
        const path: string[] = PageUtil.GetPath(this.activatedRoute.snapshot.pathFromRoot);
        this.Path = path[1];
        this.processCustomResult = this.processResultService.processResultInit(this.ComponentId, this.Title);
        this.processModalCustomResult = this.processResultService.processResultInit(this.ComponentId, this.Title, true);
      }
    });

  }

  ngOnInit() {
    this.setControlInit();
    this.setValidator();
    this.setFormatter();
    this.clear();

    let receiptCategoryResponse = this.categoryService.GetItems(CategoryType.Receipt);
    let sectionMasterResponse = this.sectionService.GetItems();

    forkJoin(
      receiptCategoryResponse,
      sectionMasterResponse
    )
      .subscribe(responseList => {
        if (responseList.length != 2 
         || responseList[0] == PROCESS_RESULT_RESULT_TYPE.FAILURE 
         || responseList[1] == PROCESS_RESULT_RESULT_TYPE.FAILURE) {

        }
        else {
          this.categoriesResult = new CategoriesResult();
          this.categoriesResult.categories = responseList[0];

          this.sectionResult = new SectionResult();
          this.sectionResult.section = responseList[1];

          this.getParamFrom();
          this.loadMfAggrAccounts();

        }
      });

    this.setAutoComplete();

  }

  public setControlInit() {

    this.accountNameCtrl = new FormControl('');     //  金融機関名
    this.subAccountNameCtrl = new FormControl('');  //  支店名
    this.accountTypeNameCtrl = new FormControl(''); //  口座種別
    this.accountNumberCtrl = new FormControl('');   // 口座番号
    this.bankCodeCtrl = new FormControl('');        // 銀行コード    
    this.branchCodeCtrl = new FormControl('');      // 支店コード

    this.receiptCategoryCodeCtrl = new FormControl('', [Validators.required, CustomValidators.number]); // 入金区分
    this.receiptCategoryNameCtrl = new FormControl('');

    // 入金部門コード
    if(this.userInfoService.ApplicationControl.useReceiptSection == 1) {
      this.sectionCodeCtrl = new FormControl('', [Validators.required,Validators.maxLength(this.userInfoService.ApplicationControl.sectionCodeLength)]);
    }
    else{
      this.sectionCodeCtrl = new FormControl('', [Validators.maxLength(this.userInfoService.ApplicationControl.sectionCodeLength)]);
    }

    this.sectionNameCtrl = new FormControl('');

  }

  public setValidator() {
    this.MyFormGroup = new FormGroup({

      accountNameCtrl: this.accountNameCtrl,          //  金融機関名
      subAccountNameCtrl: this.subAccountNameCtrl,    //  支店名
      accountTypeNameCtrl: this.accountTypeNameCtrl,  //  口座種別
      accountNumberCtrl: this.accountNumberCtrl,      //  口座番号
      bankCodeCtrl: this.bankCodeCtrl,                //  銀行コード
      branchCodeCtrl: this.branchCodeCtrl,            //  支店コード
      receiptCategoryCodeCtrl: this.receiptCategoryCodeCtrl,  // 入金区分
      receiptCategoryNameCtrl: this.receiptCategoryNameCtrl,
      sectionCodeCtrl: this.sectionCodeCtrl,          // 入金部門コード
      sectionNameCtrl: this.sectionNameCtrl,

    })
  }

  public setFormatter() {

    FormatterUtil.setNumberFormatter(this.bankCodeCtrl);
    FormatterUtil.setNumberFormatter(this.branchCodeCtrl);
    FormatterUtil.setNumberFormatter(this.receiptCategoryCodeCtrl);
  }

  public clear() {
    this.MyFormGroup.reset();
    this.sectionId = null;
    this.receiptCategoryId = null;
    this.accountDetails.length = 0;
    this.ComponentStatus = COMPONENT_STATUS_TYPE.CREATE;
  }


  public setAutoComplete() {

    // 入金区分
    this.initAutocompleteCategoriesExcludeUseLimitDate(CategoryType.Receipt, this.receiptCategoryCodeCtrl, this.categoryService, 0);

    // 入金部門コード
    this.initAutocompleteSections(this.sectionCodeCtrl, this.sectionService, 0);

  }

  /**
   * データ取得
   */
  public loadMfAggrAccounts() {

    let processComponentFactory = this.componentFactoryResolver.resolveComponentFactory(ModalRouterProgressComponent);
    let processComponentRef = this.viewContainerRef.createComponent(processComponentFactory);

    this.mfAggrMasterService.getAccounts()
      .subscribe(result => {
        this.processCustomResult = this.processResultService.processAtGetData(
          this.processCustomResult, result, false, this.partsResultMessageComponent);

          if (this.processCustomResult.result == PROCESS_RESULT_RESULT_TYPE.SUCCESS) {            
            this.VOneAccounts = result;

            this.VOneAccounts.forEach(account => {
              account.subAccounts.forEach(subAccount => {
                const accountDetail = new AccountDetailModel();
                accountDetail.Account = account;
                accountDetail.SubAccount = subAccount;

                const category = this.categoriesResult.categories.find(x => x.id == subAccount.receiptCategoryId);
                accountDetail.Category = category;

                if (this.userInfoService.ApplicationControl.useReceiptSection == 1 && subAccount.sectionId != undefined) {
                  const section = this.sectionResult.section.find(x => x.id == subAccount.sectionId);
                  accountDetail.Section = section;
                }

                this.accountDetails.push(accountDetail);
              });
            });

          }
          processComponentRef.destroy();
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
      case BUTTON_ACTION.UPDATE:
        this.update();
        break;

      case BUTTON_ACTION.REDISPLAY:
        this.reload();
        break;

      default:
        console.log('buttonAction Error.');
        break;
    }
  }

  //////////////////////////////////////////////////////////////
  public getParamFrom() {
    this.activatedRoute.paramMap.subscribe((params: ParamMap) => {
      this.paramFrom = parseInt(params.get('from'));
    });    
  }

  public back() {
    // PD1301 入金自動明細連携 データ抽出 連続画面した場合、データ抽出画面まで戻る
    if (this.paramFrom == ComponentId.PD1301) {
      this.router.navigate(['main/PH1801', { from: ComponentId.PD1301 }]);
    }
    else {
      this.router.navigate(['main/PH1801', { }]);
    }
  }

  /**
   * データ編集
   */
  public update() {

    if(this.VOneAccounts == undefined || this.VOneAccounts.length == 0) return;

    this.setUpdateData();

    let processComponentFactory = this.componentFactoryResolver.resolveComponentFactory(ModalRouterProgressComponent);
    let processComponentRef = this.viewContainerRef.createComponent(processComponentFactory);

    this.mfAggrMasterService.saveAccounts(this.VOneAccounts)
      .subscribe(result => {
        this.processCustomResult = this.processResultService.processAtSave(
          this.processCustomResult, result, false, this.partsResultMessageComponent);

        if (this.processCustomResult.result == PROCESS_RESULT_RESULT_TYPE.SUCCESS) {
          this.clear();
          this.loadMfAggrAccounts();
        }
        processComponentRef.destroy();
      });

  }

  public setUpdateData() {

    if (StringUtil.IsNullOrEmpty(this.bankCodeCtrl.value)) {
      this.accountDetails[this.selectIndex].BankCode = "";
    }
    else {
      this.accountDetails[this.selectIndex].BankCode = this.bankCodeCtrl.value;
    }

    if (StringUtil.IsNullOrEmpty(this.branchCodeCtrl.value)) {
      this.accountDetails[this.selectIndex].BranchCode = "";
    }
    else {
      this.accountDetails[this.selectIndex].BranchCode = this.branchCodeCtrl.value;
    }

    this.accountDetails[this.selectIndex].Category = this.categoriesResult.categories.find(x => x.id == this.receiptCategoryId);

    if (this.userInfoService.ApplicationControl.useReceiptSection == 1) {
      this.accountDetails[this.selectIndex].Section = this.sectionResult.section.find(x => x.id == this.sectionId);
    }

  }

  /**
   * 再表示
   */
  public reload() {
    this.clear();
    this.loadMfAggrAccounts();
  }

  /**
   * 選択した行のデータをフォームに表示する
   * @param index 選択した行番号
   */
  public selectLine(index: number) {

    let accountDetail = this.accountDetails[index];

    this.accountNameCtrl.setValue(accountDetail.AccountName);           //  金融機関名
    this.subAccountNameCtrl.setValue(accountDetail.SubAccountName);     //  支店名
    this.accountTypeNameCtrl.setValue(accountDetail.AccountTypeName);   //  口座種別
    this.accountNumberCtrl.setValue(accountDetail.AccountNumber);       //  口座番号
    this.bankCodeCtrl.setValue(accountDetail.BankCode);                 // 銀行コード
    this.branchCodeCtrl.setValue(accountDetail.BranchCode);             // 支店コード

    this.receiptCategoryCodeCtrl.setValue(accountDetail.ReceiptCategoryCode); // 入金区分
    this.receiptCategoryNameCtrl.setValue(accountDetail.ReceiptCategoryName);
    this.receiptCategoryId = accountDetail.ReceiptCategoryId;

    this.sectionCodeCtrl.setValue(accountDetail.SectionCode); // 入金部門コード
    this.sectionNameCtrl.setValue(accountDetail.SectionName);
    this.sectionId = accountDetail.SectionId;

    this.selectIndex = index;
    this.ComponentStatus = COMPONENT_STATUS_TYPE.UPDATE;

    this.processResultService.clearProcessCustomMsg(this.processCustomResult);
  }

  /**
   * 各テーブルのデータ取得
   * @param table 
   * @param keyCode 
   */
  public openMasterModal(table: TABLE_INDEX) {
    this.sectionCodeTrigger.closePanel();
    this.receiptCategoryCodeTrigger.closePanel();

    let componentFactory = this.componentFactoryResolver.resolveComponentFactory(ModalMasterComponent);
    let componentRef = this.viewContainerRef.createComponent(componentFactory);
    componentRef.instance.TableIndex = table;

    componentRef.instance.Closing.subscribe(() => {
      if (componentRef.instance.ModalStatus == MODAL_STATUS_TYPE.SELECT) {
        switch (table) {

          case TABLE_INDEX.MASTER_SECTION:
            {
              this.sectionId = componentRef.instance.SelectedObject.id;
              this.sectionCodeCtrl.setValue(componentRef.instance.SelectedCode);
              this.sectionNameCtrl.setValue(componentRef.instance.SelectedName);
              break;
            }
          case TABLE_INDEX.MASTER_RECEIPT_CATEGORY_EXCLUDE_USE_LIMIT_DATE:
            {
              // 入金区分コードモーダル呼び出し
              this.receiptCategoryId = componentRef.instance.SelectedObject.id;
              this.receiptCategoryCodeCtrl.setValue(componentRef.instance.SelectedCode);
              this.receiptCategoryNameCtrl.setValue(componentRef.instance.SelectedName);
              break;
            }

        }
      }

      componentRef.destroy();
    });

  }

  ///// Enterキー押下時の処理 //////////////////////////////////////////////////////
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
    HtmlUtil.nextFocusByName(this.elementRef, 'receiptCategoryCodeCtrl', eventType);
  }

  ///////////////////////////////////////////////////////////
  public setReceiptCategoryCode(eventType: string) {

    if (eventType != EVENT_TYPE.BLUR) {
      this.receiptCategoryCodeTrigger.closePanel();
    }

    this.receiptCategoryId = null;
    if (!StringUtil.IsNullOrEmpty(this.receiptCategoryCodeCtrl.value)) {
      this.loadStart();
      this.categoryService.GetItems(3, this.receiptCategoryCodeCtrl.value)
        .subscribe(response => {
          this.loadEnd();
          if (response != undefined && response != null && 0 < response.length) {
            this.receiptCategoryId = response[0].id;
            this.receiptCategoryCodeCtrl.setValue(response[0].code);
            this.receiptCategoryNameCtrl.setValue(response[0].name);
            HtmlUtil.nextFocusByNames(this.elementRef, ['sectionCodeCtrl','bankCodeCtrl'], eventType);
          } else {
            //this.receiptCategoryCodeCtrl.setValue("");
            this.receiptCategoryNameCtrl.setValue("");
          }
        });
    } else {
      this.receiptCategoryNameCtrl.setValue("");
      HtmlUtil.nextFocusByNames(this.elementRef, ['sectionCodeCtrl','bankCodeCtrl'], eventType);
    }
  }

  ///////////////////////////////////////////////////////////
  public setSectionCode(eventType: string) {

    if (eventType != EVENT_TYPE.BLUR) {
      this.sectionCodeTrigger.closePanel();
    }

    this.sectionId = null;
    if (!StringUtil.IsNullOrEmpty(this.sectionCodeCtrl.value)) {
      this.loadStart();
      this.sectionService.GetItems(this.sectionCodeCtrl.value)
        .subscribe(response => {
          this.loadEnd();
          if (response != undefined && response != null && 0 < response.length) {
            this.sectionId = response[0].id;
            this.sectionCodeCtrl.setValue(response[0].code);
            this.sectionNameCtrl.setValue(response[0].name);
            HtmlUtil.nextFocusByName(this.elementRef, 'bankCodeCtrl', eventType);
          } else {
            //this.sectionCodeCtrl.setValue("");
            this.sectionNameCtrl.setValue("");
          }
        });
    } else {
      this.sectionNameCtrl.setValue("");
      HtmlUtil.nextFocusByName(this.elementRef, 'bankCodeCtrl', eventType);
    }
  }

}

export class AccountDetailModel {

  private account: MfAggrAccount;
  public get Account(): MfAggrAccount {
    return this.account;
  }
  public set Account(value: MfAggrAccount) {
    this.account = value;
  }

  private subAccount: MfAggrSubAccount;
  public get SubAccount(): MfAggrSubAccount {
    return this.subAccount;
  }
  public set SubAccount(value: MfAggrSubAccount) {
    this.subAccount = value;
  }

  private category: Category;
  public get Category(): Category {
    return this.category;
  }
  public set Category(value: Category) {
    this.category = value;
    if (this.SubAccount != undefined && this.category != undefined) {
      this.SubAccount.receiptCategoryId = this.category.id;
    }
  }

  private section: Section;
  public get Section(): Section {
    return this.section;
  }
  public set Section(value: Section) {
    this.section = value;
    if (this.subAccount != undefined && this.section != undefined) {
      this.subAccount.sectionId = this.section.id;
    }
  }

  public get BankCode(): string {
    return this.Account.bankCode;
  }
  public set BankCode(value: string) {
    this.Account.bankCode = value;
  }

  public get BranchCode(): string {
    return this.SubAccount.branchCode;
  }
  public set BranchCode(value: string) {
    this.SubAccount.branchCode = value;
  }

  public get AccountName(): string {
    return  this.Account==undefined ? "" : this.Account.displayName;
  }

  public get SubAccountName(): string {
    return this.SubAccount == undefined  ? "" : this.SubAccount.name;
  }

  public get AccountTypeName(): string {
    return this.SubAccount == undefined ? "" : this.SubAccount.accountTypeName;
  }

  public get AccountNumber(): string {
    return this.SubAccount == undefined ? "" : this.SubAccount.accountNumber;
  }

  public get ReceiptCategoryCode(): string {
    return this.Category == undefined ? "" : this.Category.code;
  }

  public get ReceiptCategoryName(): string {
    return this.Category == undefined ? "" : this.Category.name;
  }

  public get ReceiptCategoryId(): number {
    return this.SubAccount == undefined ? 0 : this.SubAccount.receiptCategoryId;
  }

  public get SectionCode(): string {
    return this.Section == undefined ? "" : this.Section.code;
  }

  public get SectionName(): string {
    return this.Section == undefined ? "" : this.Section.name;
  }

  public get SectionId(): number {
    return this.Section == undefined ? 0 : this.Section.id;
  }

}