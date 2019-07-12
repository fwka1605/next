
import { Component, OnInit, ElementRef, ComponentFactoryResolver, ViewContainerRef, AfterViewInit, ViewChild } from '@angular/core';
import { TABLE_INDEX } from 'src/app/common/const/table-name.const';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Router, ActivatedRoute, NavigationEnd } from '@angular/router';
import { ReceiptService } from 'src/app/service/receipt.service';
import { BaseComponent } from 'src/app/component/common/base/base-component';
import { ModalMasterComponent } from 'src/app/component/modal/modal-master/modal-master.component';
import { HtmlUtil } from 'src/app/common/util/html.util';
import { KEY_CODE, EVENT_TYPE, BUTTON_ACTION } from 'src/app/common/const/event.const';
import { StringUtil } from 'src/app/common/util/string-util';
import { CategoryMasterService } from 'src/app/service/Master/category-master.service';
import { RECEIPT_INPUT_TYPE_DICTIONARY, RECEIPT_EXCLUDE_FLAG_DICTIONARY, CategoryType, GridId, MATCHING_ASSIGNMENT_FLAG_DICTIONARY, RECEIPT_AMOUNT_RANGE_DICTIONARY, CODE_TYPE, CATEGORY_TYPE_DICTIONARY } from 'src/app/common/const/kbn.const';
import { ReceiptsResult } from 'src/app/model/receipts-result.model';
import { ReceiptSearch } from 'src/app/model/receipt-search.model';
import { FormatterUtil, } from 'src/app/common/util/formatter.util';
import { MODAL_STATUS_TYPE, PROCESS_RESULT_RESULT_TYPE, COMPONENT_STATUS_TYPE, PROCESS_RESULT_STATUS_TYPE } from 'src/app/common/const/status.const';
import { UserInfoService } from 'src/app/service/common/user-info.service';
import { CustomerMasterService } from 'src/app/service/Master/customer-master.service';
import { SectionMasterService } from 'src/app/service/Master/section-master.service';
import { DateUtil } from 'src/app/common/util/date-util';
import { Receipt } from 'src/app/model/receipt.model';
import { ComponentId } from 'src/app/common/const/component-name.const';
import { MatchingService } from 'src/app/service/matching.service';
import { EbDataHelper } from 'src/app/common/util/eb-data-helper';
import { PageUtil } from 'src/app/common/util/page-util';
import { ProcessResultService } from 'src/app/service/common/process-result.service';
import { LocalStorageManageService } from 'src/app/service/common/local-storage-manage.service';
import { LocalStorageItem } from 'src/app/model/custom-model/local-storage-item';
import { RangeSearchKey } from 'src/app/common/const/local-storage-key-const';
import { GridSettingHelper } from 'src/app/model/grid-setting-helper.model';
import { ModalRouterProgressComponent } from 'src/app/component/modal/modal-router-progress/modal-router-progress.component';
import { NumberUtil } from 'src/app/common/util/number-util';
import { ModalMemoComponent } from 'src/app/component/modal/modal-memo/modal-memo.component';
import { NgbDateParserFormatter } from '@ng-bootstrap/ng-bootstrap';
import { NgbDateCustomParserFormatter } from 'src/app/common/util/dateformat-util';
import { ReceiptMemo } from 'src/app/model/receipt-memo.model';
import { MSG_WNG, MSG_ITEM_NUM } from 'src/app/common/const/message.const';
import { ReceiptSectionTransfer } from 'src/app/model/receipt-section-transfer.model';
import { GridSetting } from 'src/app/model/grid-setting.model';
import { CurrencyMasterService } from 'src/app/service/Master/currency-master.service';
import { Currency } from 'src/app/model/currency.model';
import { RacCurrencyPipe } from 'src/app/pipe/currency.pipe';
import { BillingHelper } from 'src/app/model/helper/billing-helper.model';
import { MatAutocompleteTrigger, MatExpansionPanel } from '@angular/material';

@Component({
  selector: 'app-pd0801-receipt-section-transfer',
  templateUrl: './pd0801-receipt-section-transfer.component.html',
  styleUrls: ['./pd0801-receipt-section-transfer.component.css'],
  providers: [
    { provide: NgbDateParserFormatter, useClass: NgbDateCustomParserFormatter }
  ]
})
export class Pd0801ReceiptSectionTransferComponent extends BaseComponent implements OnInit, AfterViewInit {

  // 遷移元のコンポーネント
  public paramFrom: ComponentId;

  // これをしないとEnum型がテンプレートで見えなかったため
  public TABLE_INDEX: typeof TABLE_INDEX = TABLE_INDEX;
  public MATCHING_ASSIGNMENT_FLAG_DICTIONARY: typeof MATCHING_ASSIGNMENT_FLAG_DICTIONARY = MATCHING_ASSIGNMENT_FLAG_DICTIONARY;
  public RECEIPT_INPUT_TYPE_DICTIONARY: typeof RECEIPT_INPUT_TYPE_DICTIONARY = RECEIPT_INPUT_TYPE_DICTIONARY;

  public readonly inputTypeDictionary = RECEIPT_INPUT_TYPE_DICTIONARY;
  public readonly excludeFlagDictionary = RECEIPT_EXCLUDE_FLAG_DICTIONARY;
  public readonly receiptAmountRangeDictionary = RECEIPT_AMOUNT_RANGE_DICTIONARY;

  // 検索パネルの開閉フラグ
  public panelOpenState: boolean;

  public selectReceipt: Receipt;

  public receiptsResult: ReceiptsResult;
  public receiptSearch: ReceiptSearch;

  public recordedAtFromCtrl: FormControl;  // 入金日
  public recordedAtToCtrl: FormControl;

  public payerNameCtrl: FormControl; // 振込依頼人名

  public customerCodeFromCtrl: FormControl;  // 得意先コード
  public customerNameFromCtrl: FormControl;
  public customerCodeToCtrl: FormControl;
  public customerNameToCtrl: FormControl;
  public cbxCustomerCtrl: FormControl;

  public sectionCodeCtrl: FormControl; // 入金部門コード
  public sectionNameCtrl: FormControl;

  public cbxMemoCtrl: FormControl; // メモ
  public receiptMemoCtrl: FormControl;

  public receiptCategoryCodeCtrl: FormControl; // 入金区分
  public receiptCategoryNameCtrl: FormControl;

  public cbxPartAssignmentFlagCtrl: FormControl;// 消込区分
  public cbxNoAssignmentFlagCtrl: FormControl;

  public currencyCodeCtrl: FormControl;
  public undefineCtrl: FormControl; // 未定用

  public updateById: number | null;
  public currencyId: number;

  public dispSumCount: number;
  public dispSumReceiptAmount: number;
  public dispSumRemainAmount: number;

  public gridSettingHelper = new GridSettingHelper();

  public cbxCancelFlags: FormControl[];
  public transferSectionCodeCtrls: FormControl[];
  public transferSectionNames: string[];
  public transferSectionIds: number[];
  public transferAmountCtrls: FormControl[];
  public transferMemoCtrls: string[];

  public receiptAmounts: Array<string>;
  public remainAmounts: Array<string>;
  public isTransferMemoButtons: Array<boolean>;
  public setting: GridSetting;

  @ViewChild('receiptCategoryCodeInput', { read: MatAutocompleteTrigger }) receiptCategoryCodeTrigger: MatAutocompleteTrigger;
  @ViewChild('customerCodeFromInput', { read: MatAutocompleteTrigger }) customerCodeFromTrigger: MatAutocompleteTrigger;
  @ViewChild('customerCodeToInput', { read: MatAutocompleteTrigger }) customerCodeToTrigger: MatAutocompleteTrigger;
  @ViewChild('sectionCodeInput', { read: MatAutocompleteTrigger }) sectionCodeToTrigger: MatAutocompleteTrigger;

  @ViewChild('panel') panel:MatExpansionPanel;

  constructor(
    public router: Router,
    public activatedRoute: ActivatedRoute,
    public elementRef: ElementRef,
    public componentFactoryResolver: ComponentFactoryResolver,
    public viewContainerRef: ViewContainerRef,
    public userInfoService: UserInfoService,
    public categoryService: CategoryMasterService,
    public receiptService: ReceiptService,
    public customerService: CustomerMasterService,
    public sectionService: SectionMasterService,
    public matchingService: MatchingService,
    public processResultService: ProcessResultService,
    public localStorageManageService: LocalStorageManageService,
    public billingHelper: BillingHelper,
    public currencyService: CurrencyMasterService,
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

    this.setting = new GridSetting();
    this.setting.companyId = this.userInfoService.Company.id;
    this.setting.loginUserId = this.userInfoService.LoginUser.id;
    this.setting.columnName = "AssignmentState";
  }

  ngAfterViewInit() {
    HtmlUtil.nextFocusByName(this.elementRef, 'recordedAtFromCtrl', EVENT_TYPE.NONE);
  }

  private setControlInit() {
    this.recordedAtFromCtrl = new FormControl("", [Validators.maxLength(10)]);  // 入金日
    this.recordedAtToCtrl = new FormControl("", [Validators.maxLength(10)]);

    this.payerNameCtrl = new FormControl("", [Validators.maxLength(140)]); // 振込依頼人名

    this.customerCodeFromCtrl = new FormControl("", [Validators.maxLength(this.userInfoService.ApplicationControl.customerCodeLength)]);  // 得意先コード
    this.customerNameFromCtrl = new FormControl("");
    this.customerCodeToCtrl = new FormControl("", [Validators.maxLength(this.userInfoService.ApplicationControl.customerCodeLength)]);
    this.customerNameToCtrl = new FormControl("");
    this.cbxCustomerCtrl = new FormControl("");

    this.sectionCodeCtrl = new FormControl("", [Validators.maxLength(this.userInfoService.ApplicationControl.sectionCodeLength)]); // 入金部門コード
    this.sectionNameCtrl = new FormControl("");

    this.cbxMemoCtrl = new FormControl(""); // メモ
    this.receiptMemoCtrl = new FormControl("", [Validators.maxLength(100)]);

    this.receiptCategoryCodeCtrl = new FormControl("", [Validators.maxLength(2)]); // 入金区分
    this.receiptCategoryNameCtrl = new FormControl("");

    this.cbxPartAssignmentFlagCtrl = new FormControl(""); // 消込区分
    this.cbxNoAssignmentFlagCtrl = new FormControl("");

    this.currencyCodeCtrl = new FormControl("");

    this.undefineCtrl = new FormControl(""); // 未定用;

  }

  private setValidator() {
    this.MyFormGroup = new FormGroup({
      recordedAtFromCtrl: this.recordedAtFromCtrl,  // 入金日
      recordedAtToCtrl: this.recordedAtToCtrl,

      payerNameCtrl: this.payerNameCtrl, // 振込依頼人名

      customerCodeFromCtrl: this.customerCodeFromCtrl,  // 得意先コード
      customerNameFromCtrl: this.customerNameFromCtrl,
      customerCodeToCtrl: this.customerCodeToCtrl,
      customerNameToCtrl: this.customerNameToCtrl,
      cbxCustomerCtrl: this.cbxCustomerCtrl,

      sectionCodeCtrl: this.sectionCodeCtrl, // 入金部門子コード
      sectionNameCtrl: this.sectionNameCtrl,

      cbxMemoCtrl: this.cbxMemoCtrl, // メモ
      receiptMemoCtrl: this.receiptMemoCtrl,

      receiptCategoryCodeCtrl: this.receiptCategoryCodeCtrl, // 入金区分
      receiptCategoryNameCtrl: this.receiptCategoryNameCtrl,

      cbxPartAssignmentFlagCtrl: this.cbxPartAssignmentFlagCtrl,  // 消込区分
      cbxNoAssignmentFlagCtrl: this.cbxNoAssignmentFlagCtrl,

      currencyCodeCtrl: this.currencyCodeCtrl,

      undefineCtrl: this.undefineCtrl, // 未定用
    });
  }

  private setFormatter() {
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
      FormatterUtil.setNumberFormatter(this.sectionCodeCtrl); // 入金部門コード
    }
    else {
      FormatterUtil.setCodeFormatter(this.sectionCodeCtrl);
    }
    FormatterUtil.setNumberFormatter(this.receiptCategoryCodeCtrl); // 入金区分
  }

  public clear() {
    this.panelOpenState = true;
    this.panel.open();

    this.MyFormGroup.reset();


    this.cbxPartAssignmentFlagCtrl.setValue(true);  // 消込区分
    this.cbxNoAssignmentFlagCtrl.setValue(true);

    this.selectReceipt = null;
    this.updateById = null;

    /*
    this.receiptsResult = null;
    this.dispSumCount = 0;
    this.dispSumReceiptAmount = 0;
    this.dispSumRemainAmount = 0;
    */
   
    let cbxCustomer = this.localStorageManageService.get(RangeSearchKey.PD0801_CUSTOMER);
    if (cbxCustomer != null) {
      this.cbxCustomerCtrl.setValue(cbxCustomer.value);
    }

    this.cbxMemoCtrl.setValue(false);

    HtmlUtil.nextFocusByName(this.elementRef, 'recordedAtFromCtrl', EVENT_TYPE.NONE);
  }

  public setAutoComplete() {

    // 入金区分
    this.initAutocompleteCategories(CategoryType.Receipt, this.receiptCategoryCodeCtrl, this.categoryService, 0);

    // 得意先
    this.initAutocompleteCustomers(this.customerCodeFromCtrl, this.customerService, 0);
    this.initAutocompleteCustomers(this.customerCodeToCtrl, this.customerService, 1);

    // 入金部門
    this.initAutocompleteSections(this.sectionCodeCtrl, this.sectionService, 0);


  }

  /**
   * 各コードの参照
   * @param table 参照先のテーブル
   * @param keyCode キーコード
   * @param type 指定の文字列
   */
  public openMasterModal(table: TABLE_INDEX, type: string = null) {

    this.receiptCategoryCodeTrigger.closePanel();
    this.customerCodeFromTrigger.closePanel();
    this.customerCodeToTrigger.closePanel();

    let componentFactory = this.componentFactoryResolver.resolveComponentFactory(ModalMasterComponent);
    let componentRef = this.viewContainerRef.createComponent(componentFactory);

    componentRef.instance.TableIndex = table;

    componentRef.instance.Closing.subscribe(() => {
      if (componentRef.instance.ModalStatus == MODAL_STATUS_TYPE.SELECT) {
        switch (table) {
          case TABLE_INDEX.MASTER_RECEIPT_CATEGORY:
            {
              this.receiptCategoryCodeCtrl.setValue(componentRef.instance.SelectedCode);
              this.receiptCategoryNameCtrl.setValue(componentRef.instance.SelectedName);
              break;
            }
          case TABLE_INDEX.MASTER_SECTION:
            {
              if (type == null) {
                this.sectionCodeCtrl.setValue(componentRef.instance.SelectedCode);
                this.sectionNameCtrl.setValue(componentRef.instance.SelectedName);

              } else {
                this.transferSectionCodeCtrls[type].setValue(componentRef.instance.SelectedCode);
                this.transferSectionNames[type] = componentRef.instance.SelectedName;
                this.transferSectionIds[type] = componentRef.instance.selectedId;
                this.setTransferSection(Number(type));
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
              const currency = componentRef.instance.selectedObject;
              this.currencyId = currency.id;
              break;
            }
        }
      }
      componentRef.destroy();
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
      case BUTTON_ACTION.SEARCH:
        this.search();
        break;
      case BUTTON_ACTION.REGISTRY:
        this.registry();
        break;
    }
  }

  private search() {
    let msg:string = null;
    // 必須チェック
    if (!this.cbxPartAssignmentFlagCtrl.value && !this.cbxNoAssignmentFlagCtrl.value) {
      msg = MSG_WNG.NOT_EXIST_UPDATE_DATA.replace(MSG_ITEM_NUM.FIRST, "消込区分");
    }
    if (this.userInfoService.ApplicationControl.useForeignCurrency == 1
      && StringUtil.IsNullOrEmpty(this.currencyCodeCtrl.value)) {
      msg = MSG_WNG.INPUT_REQUIRED.replace(MSG_ITEM_NUM.FIRST, "通貨コード");
    }

    if (msg.length != 0) {
      this.processCustomResult = this.processResultService.processAtWarning(
        this.processCustomResult, msg, this.partsResultMessageComponent
      );
      return;
    }
    this.setSearchOption();
    this.searchRequest();
  }

  private setSearchOption() {
    this.receiptSearch = new ReceiptSearch();
    this.receiptSearch.companyId = this.userInfoService.Company.id;
    this.receiptSearch.loginUserId = this.userInfoService.LoginUser.id;

    this.receiptSearch.recordedAtFrom = DateUtil.ConvertFromDatepicker(this.recordedAtFromCtrl);  // 入金日
    this.receiptSearch.recordedAtTo = DateUtil.ConvertFromDatepicker(this.recordedAtToCtrl);  // 入金日

    this.receiptSearch.payerName = this.payerNameCtrl.value;  // 振込依頼人名

    this.receiptSearch.updateBy = this.updateById;  // 最終更新者

    this.receiptSearch.customerCodeFrom = this.customerCodeFromCtrl.value;  // 得意先コード
    this.receiptSearch.customerCodeTo = this.customerCodeToCtrl.value;

    this.receiptSearch.sectionCode = this.sectionCodeCtrl.value;  // 入金部門コード

    // メモ
    if (this.cbxMemoCtrl.value) {
      this.receiptSearch.existsMemo = 1;
    }
    else {
      this.receiptSearch.existsMemo = 0;
    }
    this.receiptSearch.receiptMemo = this.receiptMemoCtrl.value;

    this.receiptSearch.receiptCategoryCode = this.receiptCategoryCodeCtrl.value;  // 入金区分

    // 消込・未消込・一部消込
    // 2進数
    // 消込済み；0x004 = 4
    // 一部	；0x002 = 2
    // 未消込	；0x001 = 1
    this.receiptSearch.assignmentFlag = 0;
    if (this.cbxPartAssignmentFlagCtrl.value) {
      this.receiptSearch.assignmentFlag = this.receiptSearch.assignmentFlag + 2;
    }
    if (this.cbxNoAssignmentFlagCtrl.value) {
      this.receiptSearch.assignmentFlag = this.receiptSearch.assignmentFlag + 1;
    }

    // 通貨コード
    if (this.userInfoService.ApplicationControl.useForeignCurrency == 1) {
      this.receiptSearch.useForeignCurrencyFlg = 1;
      this.receiptSearch.currencyId = this.currencyId;
    }

  }

  /**
   * データ検索
   */
  private searchRequest() {
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

            this.setSearchFormControl();
            this.setSearchResult();
            this.panelOpenState = false;

          }
          else {
            // 該当データなし
            this.receiptsResult = new ReceiptsResult();
            this.receiptsResult.receipts = response;
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

  /**
   * 検索後のフォーム設定
   */
  private setSearchFormControl() {
    // 戻る対応のための検索結果を退避
    this.receiptService.ReceiptSearch = this.receiptSearch;
    this.receiptService.ReceiptSearchFormGroup = this.MyFormGroup;

    // Formcontrol初期化
    this.cbxCancelFlags = new Array<FormControl>(this.receiptsResult.receipts.length);
    this.transferSectionCodeCtrls = new Array<FormControl>(this.receiptsResult.receipts.length);
    this.transferSectionNames = new Array<string>(this.receiptsResult.receipts.length);
    this.transferSectionIds = new Array<number>(this.receiptsResult.receipts.length);
    this.transferAmountCtrls = new Array<FormControl>(this.receiptsResult.receipts.length);
    this.transferMemoCtrls = new Array<string>(this.receiptsResult.receipts.length);
    this.isTransferMemoButtons = new Array<boolean>(this.receiptsResult.receipts.length);

    for (let i = 0; i < this.receiptsResult.receipts.length; i++) {
      this.cbxCancelFlags[i] = new FormControl();
      this.MyFormGroup.removeControl("cbxCancelFlags" + i);
      this.MyFormGroup.addControl("cbxCancelFlags" + i, this.cbxCancelFlags[i]);

      this.transferSectionCodeCtrls[i] = new FormControl();
      this.MyFormGroup.removeControl("transferSectionCodeCtrls" + i);
      this.MyFormGroup.addControl("transferSectionCodeCtrls" + i, this.transferSectionCodeCtrls[i]);
      if (this.userInfoService.ApplicationControl.sectionCodeType == CODE_TYPE.NUMBER) {
        FormatterUtil.setNumberFormatter(this.transferSectionCodeCtrls[i]);
      } else {
        FormatterUtil.setCodeFormatter(this.transferSectionCodeCtrls[i]);
      }

      this.transferAmountCtrls[i] = new FormControl();
      this.MyFormGroup.removeControl("transferAmountCtrls" + i);
      this.MyFormGroup.addControl("transferAmountCtrls" + i, this.transferAmountCtrls[i]);
      FormatterUtil.setCurrencyFormatter(this.transferAmountCtrls[i]);
      this.transferAmountCtrls[i].disable();
      this.isTransferMemoButtons[i] = true;
    }
  }

  /**
   * 検索結果の表示設定・値処理
   */
  private setSearchResult() {
    this.receiptAmounts = new Array<string>();
    this.remainAmounts = new Array<string>();
    this.dispSumCount = this.receiptsResult.receipts.length;
    this.dispSumReceiptAmount = 0;
    this.dispSumRemainAmount = 0;

    for (let i = 0; i < this.receiptsResult.receipts.length; i++) {
      let item = this.receiptsResult.receipts[i];
      // 入金日
      this.receiptsResult.receipts[i].recordedAt = DateUtil.convertDateString(item.recordedAt);
      // 金額
      this.receiptAmounts.push(NumberUtil.Separate(item.receiptAmount));
      this.remainAmounts.push(NumberUtil.Separate(item.remainAmount));

      this.dispSumReceiptAmount += item.receiptAmount;
      this.dispSumRemainAmount += item.remainAmount;
    }
  }

  /**
   * データ登録
   */
  private registry() {
    // 必須チェック
    let registryCounter: number = 0;
    let useAdvanceReceivedCounter: number = 0;
    let registryReceiptsIndexs = new Array<number>();
    for (let i = 0; i < this.receiptsResult.receipts.length; i++) {
      let item = this.receiptsResult.receipts[i];
      // 振替済みの中で、チェックボックスのチェックされた数を数える
      if (item.cancelFlag) {
        if (this.cbxCancelFlags[i].value) {
          registryCounter++;
          registryReceiptsIndexs.push(i);
        }
      } else {
        // 未振替の中で、振替入金部門が設定されている数を数える
        if (!StringUtil.IsNullOrEmpty(this.transferSectionCodeCtrls[i].value)) {
          registryCounter++;
          registryReceiptsIndexs.push(i);

          // 未振替の中で、振替入金部門が設定されている入金データが前受の場合（UseAdvanceReceived==1)はエラーメッセージを表示
          if (item.useAdvanceReceived == 1) {
            useAdvanceReceivedCounter++;
          }
        }
      }
    }

    if (registryCounter == 0 || 0 < useAdvanceReceivedCounter) {
      this.processCustomResult.status = PROCESS_RESULT_STATUS_TYPE.DONE;
      this.processCustomResult.result = PROCESS_RESULT_RESULT_TYPE.WARNING;
      if (registryCounter == 0) this.processCustomResult.message = MSG_WNG.NO_DATA.replace(MSG_ITEM_NUM.FIRST, "登録する");
      if (0 < useAdvanceReceivedCounter) this.processCustomResult.message = MSG_WNG.ADVANCED_RECEIPT_DATA_CANNOTXXX.replace(MSG_ITEM_NUM.FIRST, "編集")
      return;
    }

    // 登録
    let registryDatas = new Array<ReceiptSectionTransfer>();
    registryReceiptsIndexs.forEach(index => {
      let receiptItem = this.receiptsResult.receipts[index];
      let registryData = new ReceiptSectionTransfer();

      registryData.receiptId = receiptItem.id;
      registryData.cancelFlag = receiptItem.cancelFlag;
      registryData.sourceSectionId = receiptItem.sectionId;
      registryData.destinationSectionId = this.transferSectionIds[index];
      registryData.sourceAmount = receiptItem.remainAmount;
      registryData.destinationAmount = NumberUtil.ParseInt(this.transferAmountCtrls[index].value);
      registryData.transferMemo = this.transferMemoCtrls[index];
      registryData.updateBy = this.userInfoService.LoginUser.id;
      if (!receiptItem.cancelFlag) registryData.createBy = this.userInfoService.LoginUser.id;
      registryDatas.push(registryData);
    });

    this.receiptService.SaveReceiptSectionTransfer(registryDatas)
      .subscribe(result => {
        this.processCustomResult = this.processResultService.processAtSave(
          this.processCustomResult, result, true, this.partsResultMessageComponent);
        if (this.processCustomResult.result == PROCESS_RESULT_RESULT_TYPE.SUCCESS) {
          this.search();
        }
      });
  }

  /**
   * 振替メモモーダル呼出
   * @param index 選択した行番号
   */
  public openTransferMemoModal(index: number) {
    let componentFactory = this.componentFactoryResolver.resolveComponentFactory(ModalMemoComponent);
    let componentRef = this.viewContainerRef.createComponent(componentFactory);
    let memoValue = new ReceiptMemo();
    memoValue.memo = this.receiptsResult.receipts[index].transferMemo;

    componentRef.instance.ComponentStatus = COMPONENT_STATUS_TYPE.CREATE;
    componentRef.instance.processModalCustomResult = this.processModalCustomResult;
    componentRef.instance.LineNo = index;
    componentRef.instance.receiptMemo = memoValue;
    componentRef.instance.categoryType = CategoryType.Receipt;
    componentRef.instance.isTransterMemo = true;

    componentRef.instance.Closing.subscribe(() => {
      this.receiptsResult.receipts[index].transferMemo = componentRef.instance.receiptMemo.memo;
      this.transferMemoCtrls[index] = componentRef.instance.receiptMemo.memo;
      componentRef.destroy();
    });
  }


  ///// Enterキー押下時の処理 ////////////////////////////////////////////////////////
  public setRecordedAt(eventType: string, type: string = 'from') {
    if (type == 'from') {
      HtmlUtil.nextFocusByName(this.elementRef, 'recordedAtToCtrl', eventType);
    } else {
      HtmlUtil.nextFocusByName(this.elementRef, 'payerNameCtrl', eventType);
    }
  }

  public setPayerName(eventType: string) {
    HtmlUtil.nextFocusByName(this.elementRef, 'receiptCategoryCodeCtrl', eventType);
  }

  public inputPayerName() {
    let value = this.payerNameCtrl.value;
    value = EbDataHelper.convertToValidEbkana(value);
    this.payerNameCtrl.setValue(value);
  }

  ///////////////////////////////////////////////////////////

  public setReceiptCategoryCode(eventType: string) {
    if (eventType != EVENT_TYPE.BLUR) {
      this.receiptCategoryCodeTrigger.closePanel();
    }

    if (!StringUtil.IsNullOrEmpty(this.receiptCategoryCodeCtrl.value)) {

      this.receiptCategoryCodeCtrl.setValue(StringUtil.setPaddingFrontZero(this.receiptCategoryCodeCtrl.value, 2));

      this.loadStart();
      this.categoryService.GetItems(CategoryType.Receipt, this.receiptCategoryCodeCtrl.value)
        .subscribe(response => {
          this.loadEnd();
          if (response != PROCESS_RESULT_RESULT_TYPE.FAILURE && response.length > 0) {
            this.receiptCategoryCodeCtrl.setValue(response[0].code);
            this.receiptCategoryNameCtrl.setValue(response[0].name);
            HtmlUtil.nextFocusByName(this.elementRef, 'customerCodeFromCtrl', eventType);
          }
          else {
            //this.receiptCategoryCodeCtrl.setValue("");
            this.receiptCategoryNameCtrl.setValue("");

          }
        });
    }
    else {
      this.receiptCategoryCodeCtrl.setValue("");
      this.receiptCategoryNameCtrl.setValue("");
      HtmlUtil.nextFocusByName(this.elementRef, 'customerCodeFromCtrl', eventType);
    }

  }

  ///////////////////////////////////////////////////////////

  public setCustomerCodeFrom(eventType: string) {

    if (eventType != EVENT_TYPE.BLUR) {
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

    if (eventType != EVENT_TYPE.BLUR) {
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

            HtmlUtil.nextFocusByName(this.elementRef, 'sectionCodeCtrl', eventType);
          }
          else {
            //this.customerCodeToCtrl.setValue("");
            this.customerNameToCtrl.setValue("");
            HtmlUtil.nextFocusByName(this.elementRef, 'sectionCodeCtrl', eventType);
          }
        });
    }
    else {
      this.customerCodeToCtrl.setValue("");
      this.customerNameToCtrl.setValue("");
      HtmlUtil.nextFocusByName(this.elementRef, 'sectionCodeCtrl', eventType);
    }
  }

  public setCbxCustomer(eventType: string) {
    let localstorageItem = new LocalStorageItem();
    localstorageItem.key = RangeSearchKey.PD0501_CUSTOMER;
    localstorageItem.value = this.cbxCustomerCtrl.value;
    this.localStorageManageService.set(localstorageItem);

    HtmlUtil.nextFocusByName(this.elementRef, "customerCodeFromCtrl", eventType);

  }


  public setSectionCode(eventType: string, index: number = -1) {

    if (eventType != EVENT_TYPE.BLUR) {
      this.sectionCodeToTrigger.closePanel();
    }

    let formControl: FormControl;
    if (index < 0) {
      formControl = this.sectionCodeCtrl;
    } else {
      formControl = this.transferSectionCodeCtrls[index];
    }

    if (!StringUtil.IsNullOrEmpty(formControl.value)) {

      if (this.userInfoService.ApplicationControl.sectionCodeType == CODE_TYPE.NUMBER) {
        formControl.setValue(StringUtil.setPaddingFrontZero(formControl.value, this.userInfoService.ApplicationControl.sectionCodeLength));
      }

      this.loadStart();
      this.sectionService.GetItems(formControl.value)
        .subscribe(response => {
          this.loadEnd();
          if (response != PROCESS_RESULT_RESULT_TYPE.FAILURE && response.length > 0) {
            formControl.setValue(response[0].code);
            if (index < 0) {
              this.sectionNameCtrl.setValue(response[0].name);
              HtmlUtil.nextFocusByName(this.elementRef, 'receiptMemoCtrl', eventType);

            } else {
              this.transferSectionNames[index] = response[0].name;
              this.transferSectionIds[index] = response[0].id;
              this.setTransferSection(index);
              HtmlUtil.nextFocusByName(this.elementRef, 'transferAmountCtrls' + index, eventType);
            }

          }
          else {
            // formControl.setValue("");
            if (index < 0) {
              this.sectionNameCtrl.setValue('');
            } else {
              this.transferSectionNames[index] = null;
              this.transferSectionIds[index] = null;
              this.transferMemoCtrls[index] = null;
              this.transferAmountCtrls[index].setValue("");
              this.transferAmountCtrls[index].disable();
              this.isTransferMemoButtons[index] = true;
            }
          }
        });
    }
    else {
      formControl.setValue("");
      if (index < 0) {
        this.sectionNameCtrl.setValue('');
        HtmlUtil.nextFocusByName(this.elementRef, 'receiptMemoCtrl', eventType);
      } else {
        this.transferSectionNames[index] = null;
        this.transferSectionIds[index] = null;
        this.transferMemoCtrls[index] = null;
        this.transferAmountCtrls[index].setValue("");
        this.transferAmountCtrls[index].disable();
        this.isTransferMemoButtons[index] = true;
      }
    }
  }

  private setTransferSection(index: number) {
    if (this.receiptsResult.receipts[index].sectionCode == this.transferSectionCodeCtrls[index].value) {
      this.processCustomResult.result = PROCESS_RESULT_RESULT_TYPE.WARNING;
      this.processCustomResult.message = MSG_WNG.CANNOT_TRANSFER_SAME_RECEIPT_SECTION;

      this.transferSectionCodeCtrls[index].setValue("");
      this.transferSectionNames[index] = null;
      this.transferSectionIds[index] = null;
      this.transferAmountCtrls[index].setValue("");

      this.transferAmountCtrls[index].disable();
      this.isTransferMemoButtons[index] = true;

    } else {
      this.transferAmountCtrls[index].enable();
      this.transferAmountCtrls[index].setValue(this.receiptsResult.receipts[index].remainAmount);
      this.onFocusTransferAmount(index);
      this.isTransferMemoButtons[index] = false;
    }
  }

  public setCbxMemo(eventType: string) {
    HtmlUtil.nextFocusByName(this.elementRef, 'receiptMemoCtrl', eventType);
  }

  public setReceiptMemo(eventType: string) {
    HtmlUtil.nextFocusByName(this.elementRef, 'recordedAtFromCtrl', eventType);
  }

  public setTransferAmount(eventType: string, index: number) {
    let ctrlValue = this.transferAmountCtrls[index].value;
    let maxValue = this.receiptsResult.receipts[index].remainAmount;
    if (!StringUtil.IsNullOrEmpty(ctrlValue)) {
      if (ctrlValue == 0) {
        this.processCustomResult.result = PROCESS_RESULT_RESULT_TYPE.WARNING;
        this.processCustomResult.message = MSG_WNG.INPUT_REQUIRED.replace(MSG_ITEM_NUM.FIRST, "振替金額");
        this.transferAmountCtrls[index].setValue(maxValue);

      } else {
        if (maxValue < ctrlValue) {
          ctrlValue = maxValue;
        }

        this.transferAmountCtrls[index].setValue(ctrlValue);
        HtmlUtil.nextFocusByName(this.elementRef, 'transferAmountCtrl' + index, eventType);

      }
    }
  }

  public setCbxCancelFlag(eventType: string, index: number) {
    if (this.cbxCancelFlags[index].value) {
      /**
      振替入金部門コード、振替入金部門名、振替メモ をクリア
      振替金額 に 入金残の値をセット
      振替入金部門コード、振替金額、振替メモ を使用不可に
      */
      this.transferSectionCodeCtrls[index].setValue("");
      this.transferSectionNames[index] = null;
      this.transferMemoCtrls[index] = null;
      this.transferAmountCtrls[index].setValue(NumberUtil.Separate(this.receiptsResult.receipts[index].remainAmount));
      this.transferSectionCodeCtrls[index].disable();
      this.transferAmountCtrls[index].disable();
      this.isTransferMemoButtons[index] = true;

    } else {
      // 振替入金部門コード、振替金額、振替メモ を使用可能
      this.transferSectionCodeCtrls[index].enable();
      this.transferAmountCtrls[index].setValue("");
      this.transferAmountCtrls[index].disable();
    }
  }

  public setCurrencyCode(eventType: string) {
    if (!StringUtil.IsNullOrEmpty(this.currencyCodeCtrl.value)) {
      this.loadStart();
      this.currencyService.GetItems(this.currencyCodeCtrl.value)
        .subscribe(response => {
          this.loadEnd();
          if (response != PROCESS_RESULT_RESULT_TYPE.FAILURE && response.length > 0) {
            const currency = response[0] as Currency;
            this.currencyCodeCtrl.setValue(currency.code);
            this.currencyId = currency.id;
            HtmlUtil.nextFocusByName(this.elementRef, 'recordedAtFromCtrl', eventType);
          } else {
            this.currencyCodeCtrl.setValue("");
          }
        });
    }
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

  public onFocusTransferAmount(index: number) {
    this.onFocusCurrencyControl(this.transferAmountCtrls[index]);
  }

  public onLeaveTransferAmount(index: number) {
    this.onLeaveCurrencyControl(this.transferAmountCtrls[index]);
  }

}
