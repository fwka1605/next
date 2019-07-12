import { Component, OnInit, ComponentFactoryResolver, ViewContainerRef, ElementRef, ViewChild } from '@angular/core';
import { BaseComponent } from 'src/app/component/common/base/base-component';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { TABLE_INDEX } from 'src/app/common/const/table-name.const';
import { ModalMasterComponent } from 'src/app/component/modal/modal-master/modal-master.component';
import { BankAccountsResult } from 'src/app/model/bank-accounts-result.model';
import { BankAccountMasterService } from 'src/app/service/Master/bank-account-master.service';
import { HtmlUtil } from 'src/app/common/util/html.util';
import { KEY_CODE, BUTTON_ACTION, EVENT_TYPE } from 'src/app/common/const/event.const';
import { FormatterUtil } from 'src/app/common/util/formatter.util';
import { UserInfoService } from 'src/app/service/common/user-info.service';
import { CustomValidators } from 'ng5-validation';
import { COMPONENT_STATUS_TYPE, MODAL_STATUS_TYPE, PROCESS_RESULT_RESULT_TYPE } from 'src/app/common/const/status.const';
import { BankAccount } from 'src/app/model/bank-account.model';
import { ModalConfirmComponent } from 'src/app/component/modal/modal-confirm/modal-confirm.component';
import { DateUtil } from 'src/app/common/util/date-util';
import { FILE_EXTENSION } from 'src/app/common/const/eb-file.const';
import { FileUtil } from 'src/app/common/util/file.util';
import { REPORT_HEADER, LINE_FEED_CODE } from 'src/app/common/const/report-setting-kbn.const';
import { EXCLUSIVE_ACCOUNT_TYPE, CategoryType, FunctionType } from 'src/app/common/const/kbn.const';
import { ModalImportMethodSelectorComponent } from 'src/app/component/modal/modal-import-method-selector/modal-import-method-selector.component';
import { ProcessResultService } from 'src/app/service/common/process-result.service';
import { MSG_WNG } from 'src/app/common/const/message.const';
import { Router, ActivatedRoute, NavigationEnd } from '@angular/router';
import { PageUtil } from 'src/app/common/util/page-util';
import { StringUtil } from 'src/app/common/util/string-util';
import { CategoryMasterService } from 'src/app/service/Master/category-master.service';
import { SectionMasterService } from 'src/app/service/Master/section-master.service';
import { ModalRouterProgressComponent } from 'src/app/component/modal/modal-router-progress/modal-router-progress.component';
import { MatAutocompleteTrigger, MatExpansionPanel } from '@angular/material';

@Component({
  selector: 'app-pb0801-bank-account-master',
  templateUrl: './pb0801-bank-account-master.component.html',
  styleUrls: ['./pb0801-bank-account-master.component.css']
})
export class Pb0801BankAccountMasterComponent extends BaseComponent implements OnInit {

  public readonly exclusiveAccountType = EXCLUSIVE_ACCOUNT_TYPE;
  public FunctionType: typeof FunctionType = FunctionType;

  public panelOpenState;

  public bankAccountsResult: BankAccountsResult;

  public bankCodeCtrl: FormControl; // 銀行コード
  public bankNameCtrl: FormControl;

  public branchCodeCtrl: FormControl; // 支店コード
  public branchNameCtrl: FormControl;

  public accountTypeIdCtrl: FormControl;  // 預金種別

  public accountNumberCtrl: FormControl;  // 口座番号

  public receiptCategoryCodeCtrl: FormControl; // 入金区分コード
  public receiptCategoryNameCtrl: FormControl;

  public sectionCodeCtrl: FormControl;  // 入金部門
  public sectionNameCtrl: FormControl;

  public cbxImportSkippingCtrl: FormControl; // 取込対象外

  public selectIndex: number;
  public sectionId: number;
  public receiptCategoryId: number;

  @ViewChild('sectionCodeInput', { read: MatAutocompleteTrigger }) sectionCodeTrigger: MatAutocompleteTrigger;
  @ViewChild('receiptCategoryCodeInput', { read: MatAutocompleteTrigger }) receiptCategoryCodeTrigger: MatAutocompleteTrigger;
  @ViewChild('panel') panel: MatExpansionPanel;


  constructor(
    public router: Router,
    public activatedRoute: ActivatedRoute,
    public elementRef: ElementRef,
    public componentFactoryResolver: ComponentFactoryResolver,
    public viewContainerRef: ViewContainerRef,
    public userInfoService: UserInfoService,
    public bankAccountService: BankAccountMasterService,
    public processResultService: ProcessResultService,
    public categoryService: CategoryMasterService,
    public sectionService: SectionMasterService

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

    if (!this.userInfoService.isFunctionAvailable(FunctionType.MasterImport)
      && !this.userInfoService.isFunctionAvailable(FunctionType.MasterExport)) {
      this.securityHideShow = false;
    } else {
      this.securityHideShow = true;
    }

    this.getBankAccountData();
    this.setAutoComplete();

  }

  public setControlInit() {

    this.bankCodeCtrl = new FormControl('', [Validators.required, CustomValidators.number]); // 銀行コード
    this.bankNameCtrl = new FormControl('', [Validators.required]);

    this.branchCodeCtrl = new FormControl('', [Validators.required, CustomValidators.number]); // 支店コード
    this.branchNameCtrl = new FormControl('', [Validators.required]);

    this.accountTypeIdCtrl = new FormControl('', [Validators.required]); // 預金種別

    this.accountNumberCtrl = new FormControl('', [Validators.required, CustomValidators.number]); // 口座番号

    this.receiptCategoryCodeCtrl = new FormControl('', [Validators.required, CustomValidators.number]); // 入金区分コード
    this.receiptCategoryNameCtrl = new FormControl('');

    this.sectionCodeCtrl = new FormControl('', [Validators.maxLength(this.userInfoService.ApplicationControl.sectionCodeLength)]); // 入金部門
    this.sectionNameCtrl = new FormControl('');

    this.cbxImportSkippingCtrl = new FormControl(''); // 取込対象外

  }

  public setValidator() {
    this.MyFormGroup = new FormGroup({

      bankCodeCtrl: this.bankCodeCtrl,  // 銀行コード
      bankNameCtrl: this.bankNameCtrl,

      branchCodeCtrl: this.branchCodeCtrl,  // 支店コード
      branchNameCtrl: this.branchNameCtrl,

      accountTypeIdCtrl: this.accountTypeIdCtrl,  // 預金種別

      accountNumberCtrl: this.accountNumberCtrl,  // 口座番号

      receiptCategoryCodeCtrl: this.receiptCategoryCodeCtrl,  // 入金区分コード
      receiptCategoryNameCtrl: this.receiptCategoryNameCtrl,

      sectionCodeCtrl: this.sectionCodeCtrl,  // 入金部門
      sectionNameCtrl: this.sectionNameCtrl,

      cbxImportSkippingCtrl: this.cbxImportSkippingCtrl,  // 取込対象外
    })
  }

  public setFormatter() {

    FormatterUtil.setNumberFormatter(this.bankCodeCtrl);
    FormatterUtil.setNumberFormatter(this.branchCodeCtrl);
    FormatterUtil.setNumberFormatter(this.accountNumberCtrl);
    FormatterUtil.setNumberFormatter(this.receiptCategoryCodeCtrl);
  }

  public clear() {
    this.MyFormGroup.reset();
    this.sectionId = null;
    this.receiptCategoryId = null;
    this.ComponentStatus = COMPONENT_STATUS_TYPE.CREATE;

    this.panelOpenState = false;
    this.panel.close();
  }


  public setAutoComplete() {

    // 入金区分
    this.initAutocompleteCategories(CategoryType.Receipt, this.receiptCategoryCodeCtrl, this.categoryService, 0);

    // 入金部門
    this.initAutocompleteSections(this.sectionCodeCtrl, this.sectionService, 0);


  }

  /**
   * データ取得
   */
  public getBankAccountData() {

    let processComponentFactory = this.componentFactoryResolver.resolveComponentFactory(ModalRouterProgressComponent);
    let processComponentRef = this.viewContainerRef.createComponent(processComponentFactory);
    // processComponentRef.destroy();   

    this.bankAccountService.GetItems()
      .subscribe(result => {
        this.processCustomResult = this.processResultService.processAtGetData(
          this.processCustomResult, result, false, this.partsResultMessageComponent);
        if (this.processCustomResult.result == PROCESS_RESULT_RESULT_TYPE.SUCCESS) {
          this.bankAccountsResult = new BankAccountsResult();
          this.bankAccountsResult.bankAccounts = new Array<BankAccount>();
          this.bankAccountsResult.bankAccounts = result;
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
      case BUTTON_ACTION.REGISTRY:
        this.registry();
        break;

      case BUTTON_ACTION.DELETE:
        this.delete();
        break;

      case BUTTON_ACTION.IMPORT:
        this.openImportMethodSelector();
        break;

      case BUTTON_ACTION.EXPORT:
        this.export();
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
    let registryData = new BankAccount();
    let isRegistry: boolean = false;

    if (this.ComponentStatus === COMPONENT_STATUS_TYPE.CREATE) {
      registryData.id = 0;
      registryData.createBy = this.userInfoService.LoginUser.id;
      isRegistry = true;

    } else if (this.ComponentStatus = COMPONENT_STATUS_TYPE.UPDATE) {
      registryData = this.bankAccountsResult.bankAccounts[this.selectIndex];

    } else {
      return;
    }

    let processComponentFactory = this.componentFactoryResolver.resolveComponentFactory(ModalRouterProgressComponent);
    let processComponentRef = this.viewContainerRef.createComponent(processComponentFactory);
    // processComponentRef.destroy();       

    registryData.bankCode = this.bankCodeCtrl.value;
    registryData.bankName = this.bankNameCtrl.value;
    registryData.branchCode = this.branchCodeCtrl.value;
    registryData.branchName = this.branchNameCtrl.value;
    registryData.accountTypeId = this.accountTypeIdCtrl.value;
    registryData.accountNumber = this.accountNumberCtrl.value;
    registryData.receiptCategoryId = this.receiptCategoryId;
    registryData.importSkipping = this.cbxImportSkippingCtrl.value === true ? 1 : 0;
    registryData.sectionId = this.sectionId;

    this.bankAccountService.Save(registryData)
      .subscribe(result => {
        this.processCustomResult = this.processResultService.processAtSave(
          this.processCustomResult, result, isRegistry, this.partsResultMessageComponent);
        if (this.processCustomResult.result == PROCESS_RESULT_RESULT_TYPE.SUCCESS) {
          this.getBankAccountData();
          this.clear();
        }
        processComponentRef.destroy();
      });
  }

  /**
   * データ削除
   */
  public delete() {
    let bankAccount = this.bankAccountsResult.bankAccounts[this.selectIndex];
    let componentFactory = this.componentFactoryResolver.resolveComponentFactory(ModalConfirmComponent);
    let componentRef = this.viewContainerRef.createComponent(componentFactory);

    componentRef.instance.ActionName = "削除"
    componentRef.instance.Closing.subscribe(() => {

      if (componentRef.instance.ModalStatus == MODAL_STATUS_TYPE.OK) {

        componentRef.destroy();

        let processComponentFactory = this.componentFactoryResolver.resolveComponentFactory(ModalRouterProgressComponent);
        let processComponentRef = this.viewContainerRef.createComponent(processComponentFactory);
        // processComponentRef.destroy();   

        this.bankAccountService.Delete(bankAccount)
          .subscribe(result => {
            this.processCustomResult = this.processResultService.processAtDelete(
              this.processCustomResult, result, this.partsResultMessageComponent);
            if (this.processCustomResult.result == PROCESS_RESULT_RESULT_TYPE.SUCCESS) {
              this.clear();
              this.getBankAccountData();
            }
            processComponentRef.destroy();
          });
      }
    });
  }

  /**
   * 選択した行のデータをフォームに表示する
   * @param index 選択した行番号
   */
  public selectLine(index: number) {

    this.panelOpenState = true;
    this.panel.open();

    let bankAccountItem = this.bankAccountsResult.bankAccounts[index];

    this.bankCodeCtrl.setValue(bankAccountItem.bankCode); // 銀行コード
    this.bankNameCtrl.setValue(bankAccountItem.bankName);

    this.branchCodeCtrl.setValue(bankAccountItem.branchCode); // 支店コード
    this.branchNameCtrl.setValue(bankAccountItem.branchName);

    this.accountTypeIdCtrl.setValue(bankAccountItem.accountTypeId); // 預金種別

    this.accountNumberCtrl.setValue(bankAccountItem.accountNumber); // 口座番号

    this.receiptCategoryCodeCtrl.setValue(bankAccountItem.receiptCategoryCode); // 入金区分コード
    this.receiptCategoryNameCtrl.setValue(bankAccountItem.receiptCategoryName);
    this.receiptCategoryId = bankAccountItem.receiptCategoryId;

    this.sectionCodeCtrl.setValue(bankAccountItem.sectionCode); // 入金部門
    this.sectionNameCtrl.setValue(bankAccountItem.sectionName);
    this.sectionId = bankAccountItem.sectionId;

    this.cbxImportSkippingCtrl.setValue(bankAccountItem.importSkipping === 1 ? true : false); // 取込対象外

    this.selectIndex = index;
    this.ComponentStatus = COMPONENT_STATUS_TYPE.UPDATE;

    this.processResultService.clearProcessCustomMsg(this.processCustomResult);
  }

  /**
   * 各テーブルのデータ取得
   * @param table 
   * @param keyCode 
   */
  public openMasterModal(table: TABLE_INDEX, ) {

    if(this.sectionCodeTrigger != null){this.sectionCodeTrigger.closePanel();}
    if(this.receiptCategoryCodeTrigger != null){this.receiptCategoryCodeTrigger.closePanel();}

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
          case TABLE_INDEX.MASTER_RECEIPT_CATEGORY:
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

  /**
   * インポート
   */
  public openImportMethodSelector() {
    let componentFactory = this.componentFactoryResolver.resolveComponentFactory(ModalImportMethodSelectorComponent);
    let componentRef = this.viewContainerRef.createComponent(componentFactory);

    componentRef.instance.TableIndex = TABLE_INDEX.MASTER_BANK_ACCOUNT;
    componentRef.instance.processModalCustomResult = this.processModalCustomResult;

    componentRef.instance.Closing.subscribe(() => {
      componentRef.destroy();
      this.getBankAccountData();
    });
    this.openOptions();
  }

  /**
   * エクスポート
   */
  public export() {
    let dataList = this.bankAccountsResult.bankAccounts;
    let receiptSectionOption: boolean = this.userInfoService.ApplicationControl.useReceiptSection == 0 ? false : true;
    let headerList = REPORT_HEADER.BANK_ACCOUNT_MASTER;

    // 件数チェック
    if (dataList.length <= 0) {
      this.processCustomResult = this.processResultService.processAtWarning(
        this.processCustomResult, MSG_WNG.NO_EXPORT_DATA, this.partsResultMessageComponent);
      return;
    }

    let processComponentFactory = this.componentFactoryResolver.resolveComponentFactory(ModalRouterProgressComponent);
    let processComponentRef = this.viewContainerRef.createComponent(processComponentFactory);
    // processComponentRef.destroy();

    // ヘッダー設定
    if (receiptSectionOption) {
      headerList = headerList.concat(REPORT_HEADER.BANK_ACCOUNT_MASTER_OPTION);
    }

    let headers = FileUtil.encloseItemBySymbol(headerList);
    let data: string = headers.join(",") + LINE_FEED_CODE;

    for (let index = 0; index < dataList.length; index++) {
      let dataItem: Array<any> = [];
      dataItem.push(this.userInfoService.Company.code);
      dataItem.push(dataList[index].bankCode);
      dataItem.push(dataList[index].bankName);
      dataItem.push(dataList[index].branchCode);
      dataItem.push(dataList[index].branchName);
      dataItem.push(dataList[index].accountTypeId);
      dataItem.push(dataList[index].accountNumber);
      dataItem.push(dataList[index].receiptCategoryCode);
      dataItem.push(dataList[index].importSkipping);

      // 入金部門管理使用時のみ表示
      if (receiptSectionOption) {
        dataItem.push(dataList[index].sectionCode)
      }
      dataItem = FileUtil.encloseItemBySymbol(dataItem);

      data = data + dataItem.join(",") + LINE_FEED_CODE;
    }
    let isTryResult: boolean = false;
    let resultDatas: Array<any> = [];
    resultDatas.push(data);

    try {
      FileUtil.download(resultDatas, this.Title + DateUtil.getYYYYMMDD(0), FILE_EXTENSION.CSV);
      isTryResult = true;

    } catch (error) {
      console.error(error);
    }
    this.processResultService.processAtOutput(
      this.processCustomResult, isTryResult, 0, this.partsResultMessageComponent);
    this.openOptions();
    processComponentRef.destroy();
  }


  ///// Enterキー押下時の処理 //////////////////////////////////////////////////////
  public setBankCode(eventType: string) {
    if (StringUtil.IsNullOrEmpty(this.bankCodeCtrl.value)) {
      this.bankCodeCtrl.setValue("");
    }
    else {
      this.bankCodeCtrl.setValue(StringUtil.setPaddingFrontZero(this.bankCodeCtrl.value, 4, true));
    }
    HtmlUtil.nextFocusByName(this.elementRef, 'bankNameCtrl', eventType);
  }

  public setBankName(eventType: string) {
    HtmlUtil.nextFocusByName(this.elementRef, 'branchCodeCtrl', eventType);
  }

  public setBranchCode(eventType: string) {
    if (StringUtil.IsNullOrEmpty(this.branchCodeCtrl.value)) {
      this.branchCodeCtrl.setValue("");
    }
    else {
      this.branchCodeCtrl.setValue(StringUtil.setPaddingFrontZero(this.branchCodeCtrl.value, 3, true));
    }
    HtmlUtil.nextFocusByName(this.elementRef, 'branchNameCtrl', eventType);
  }

  public setBranchName(eventType: string) {
    HtmlUtil.nextFocusByName(this.elementRef, 'accountTypeIdCtrl', eventType);
  }

  public setAccountTypeId(eventType: string) {
    HtmlUtil.nextFocusByName(this.elementRef, 'accountNumberCtrl', eventType);
  }

  public setAccountNumber(eventType: string) {
    if (StringUtil.IsNullOrEmpty(this.accountNumberCtrl.value)) {
      this.accountNumberCtrl.setValue("");
    }
    else {
      this.accountNumberCtrl.setValue(StringUtil.setPaddingFrontZero(this.accountNumberCtrl.value, 7, true));
    }
    HtmlUtil.nextFocusByName(this.elementRef, 'receiptCategoryCodeCtrl', eventType);

  }

  ///////////////////////////////////////////////////////////

  public setReceiptCategoryCode(eventType: string) {

    if(eventType!=EVENT_TYPE.BLUR){
      if(this.receiptCategoryCodeTrigger != null){this.receiptCategoryCodeTrigger.closePanel();}
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
            HtmlUtil.nextFocusByName(this.elementRef, 'sectionCodeCtrl', eventType);
          } else {
            //this.receiptCategoryCodeCtrl.setValue("");
            this.receiptCategoryNameCtrl.setValue("");
          }
        });
    } else {
      this.receiptCategoryNameCtrl.setValue("");
      HtmlUtil.nextFocusByName(this.elementRef, 'sectionCodeCtrl', eventType);
    }
  }

  ///////////////////////////////////////////////////////////

  public setSectionCode(eventType: string) {

    if(eventType!=EVENT_TYPE.BLUR){
      if(this.sectionCodeTrigger != null){this.sectionCodeTrigger.closePanel();}
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
