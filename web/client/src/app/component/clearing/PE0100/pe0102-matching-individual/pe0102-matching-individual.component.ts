import { Component, OnInit, ElementRef, ComponentFactoryResolver, ViewContainerRef, ComponentRef } from '@angular/core';
import { FormGroup, FormControl } from '@angular/forms';
import { Router, ActivatedRoute, ParamMap, NavigationEnd } from '@angular/router';
import { DatePipe } from '@angular/common';
import { DecimalPipe } from '@angular/common';

import { MatchingIndividualReportSource } from '../../../../model/matching-individual-report-source.model';
import { BaseComponent } from 'src/app/component/common/base/base-component';
import { UserInfoService } from 'src/app/service/common/user-info.service';
import { MatchingService } from 'src/app/service/matching.service';
import { CollationData } from 'src/app/model/collation/collation-data.model';
import { CollationSearch } from 'src/app/model/collation-search.model';
import { Currency } from 'src/app/model/currency.model';
import { GridSetting } from 'src/app/model/grid-setting.model';
import { GridSettingMasterService } from 'src/app/service/Master/grid-setting-master.service';
import { GridId, CategoryType, CheckBoxStatus, MatchingErrorType, BILL_INPUT_TYPE_DICTIONARY, ACCOUNT_TYPE_DICTIONARY, ADVANCED_RECEIVED_RECORDED_DATA_TYPE } from 'src/app/common/const/kbn.const';
import { CurrencyMasterService } from 'src/app/service/Master/currency-master.service';
import { forkJoin, Observable, Subject } from 'rxjs';
import { MatchingBillingSearch } from 'src/app/model/matching-billing-search.model';
import { MatchingReceiptSearch } from 'src/app/model/matching-receipt-search.model';
import { StringUtil } from 'src/app/common/util/string-util';
import { FormatterUtil } from 'src/app/common/util/formatter.util';
import { ModalIndividualMemoComponent } from 'src/app/component/modal/modal-individual-memo/modal-individual-memo.component';
import { MODAL_STATUS_TYPE, PROCESS_RESULT_RESULT_TYPE, COMPONENT_STATUS_TYPE } from 'src/app/common/const/status.const';
import { ReceiptService } from 'src/app/service/receipt.service';
import { BillingService } from 'src/app/service/billing.service';
import { BillingMemo } from 'src/app/model/billing-memo.model';
import { ReceiptMemo } from 'src/app/model/receipt-memo.model';
import { ComponentId } from 'src/app/common/const/component-name.const';
import { NumberUtil } from 'src/app/common/util/number-util';
import { ModalCustomerDetailComponent } from 'src/app/component/modal/modal-customer-detail/modal-customer-detail.component';
import { HtmlUtil } from 'src/app/common/util/html.util';
import { RacCurrencyPipe } from 'src/app/pipe/currency.pipe';
import { ModalConfirmComponent } from 'src/app/component/modal/modal-confirm/modal-confirm.component';
import { SortItem, SortOrder } from 'src/app/model/collation/sort-item';
import { MSG_WNG, MSG_INF, MSG_ERR, MSG_ITEM_NUM } from 'src/app/common/const/message.const';
import { IndividualBilling } from 'src/app/model/collation/individual-billing';
import { IndividualReceipt } from 'src/app/model/collation/individual-receipt';
import { Receipt } from 'src/app/model/receipt.model';
import { SortUtil } from 'src/app/common/util/sort-util';
import { ExportMatchingIndividualSub } from 'src/app/model/collation/export-matching-individual-sub';
import { LINE_FEED_CODE } from 'src/app/common/const/report-setting-kbn.const';
import { FileUtil } from 'src/app/common/util/file.util';
import { DateUtil } from 'src/app/common/util/date-util';
import { FILE_EXTENSION } from 'src/app/common/const/eb-file.const';
import { BUTTON_ACTION, EVENT_TYPE } from 'src/app/common/const/event.const';
import { Customer } from 'src/app/model/customer.model';
import { CustomerGroup } from 'src/app/model/customer-group.model';
import { Billing } from 'src/app/model/billing.model';
import { CustomerGroupMasterService } from 'src/app/service/Master/customer-group-master.service';
import { ModalMatchingRecordedAtComponent } from 'src/app/component/modal/modal-matching-recorded-at/modal-matching-recorded-at.component';
import { MatchingSource } from 'src/app/model/matching-source.model';
import { ModalSelectParentCustomerComponent } from 'src/app/component/modal/modal-select-parent-customer/modal-select-parent-customer.component';
import { ModalConfirmMatchingComponent } from 'src/app/component/modal/modal-confirm-matching/modal-confirm-matching.component';
import { ModalConfirmMatchingAdvancedCustomerComponent } from 'src/app/component/modal/modal-confirm-matching-advanced-customer/modal-confirm-matching-advanced-customer.component';
import { MatchingResult } from 'src/app/model/matching-result.model';
import { CustomerMasterService } from 'src/app/service/Master/customer-master.service';
import { PaymentAgencyMasterService } from 'src/app/service/Master/payment-agency-master.service';
import { ProcessResultService } from 'src/app/service/common/process-result.service';
import { PageUtil } from 'src/app/common/util/page-util';
import {  LocalStorageManageService } from 'src/app/service/common/local-storage-manage.service';
import { LocalStorageItem } from 'src/app/model/custom-model/local-storage-item';
import { GridSizeKey } from 'src/app/common/const/local-storage-key-const';
import { GridSettingHelper } from 'src/app/model/grid-setting-helper.model';
import { ModalRouterProgressComponent } from 'src/app/component/modal/modal-router-progress/modal-router-progress.component';
import { map } from 'rxjs/operators';
import { formControlBinding } from '@angular/forms/src/directives/ng_model';
import { MatchingUtil } from 'src/app/common/util/matching-util';
import { MatchingOrder } from 'src/app/model/matching-order.model';

const ymd = 'yyyy/MM/dd';

@Component({
  selector: 'app-pe0102-matching-individual',
  templateUrl: './pe0102-matching-individual.component.html',
  styleUrls: ['./pe0102-matching-individual.component.css']
})
export class Pe0102MatchingIndividualComponent extends BaseComponent implements OnInit{

  public billingPanelOpenState:boolean;
  public receiptPanelOpenState:boolean;


  public readonly CategoryType: typeof CategoryType = CategoryType;
  public readonly SortOrder: typeof SortOrder = SortOrder;

  public paramFrom: ComponentId;
  public paramProcess: string;


  constructor(
    public elementRef: ElementRef,
    public componentFactoryResolver: ComponentFactoryResolver,
    public viewContainerRef: ViewContainerRef,
    public router: Router,
    public activatedRoute: ActivatedRoute,
    public userInfoService: UserInfoService,
    public matchingService: MatchingService,
    public gridSettingService: GridSettingMasterService,
    public currencyService: CurrencyMasterService,
    public receiptService: ReceiptService,
    public billingService: BillingService,
    public customerGroupService: CustomerGroupMasterService,
    public customerMasterService: CustomerMasterService,
    public paymentAgencyMasterService: PaymentAgencyMasterService,
    public processResultService: ProcessResultService,
    public localStorageResultLineManageService:LocalStorageManageService,
    public datePipe: DatePipe,
    public decimalPipe: DecimalPipe,
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

  public pipe = new RacCurrencyPipe();


  public displayTypeButton: string;


  //////////////////////////////////////////
  // 表示・登録項目
  /////////////////////////////////////////

  // 全選択・全解除チェック
  public cbxInvoiceCheckAllCtrl:FormControl;
  public cbxReceiptCheckAllCtrl:FormControl;


  //顧客情報
  public customerName: string;         // 代表得意先名
  public ReceiptRecordedAtTo: string;  // 入金日
  public BillingDueAtTo: string;       // 入金予定日
  public CurrencyCode: string;         // 通貨コード

  public cbxAdvanceReceived: boolean;      // 繰越
  public advanceReceived: string;          // 繰越表記

  // 請求
  public billingCount: string;       // 件数
  public billingAmountTotal: number; // 総額
  public billingTaxDifferenceCtrl: FormControl; // 消費税誤差
  public discountCtrl: FormControl; // 歩引き率
  public billingTargetAmount: number;  // 消込対象額
  public billingMatchingAmount: number;  // 消込予定額
  public billingMatchingRemain: number;  // 消込差額

  // 入金
  public receiptCount: string;     // 件数
  public receiptAmountTotal: number; // 総額
  public receiptTaxDifferenceCtrl: FormControl; // 消費税誤差
  public transferFeeCtrl: FormControl;  // 手数料
  public receiptTargetAmount: number;  // 消込対象額
  public receiptMatchingAmount: number;  // 消込予定額
  public receiptMatchingRemain: number;  // 消込差額

  //////////////////////////////////////////
  // DB検索結果
  /////////////////////////////////////////
  public billings: Array<IndividualBilling>;
  public receipts: Array<IndividualReceipt>;

  public cbxDetailBillingCtrls: Map<number,FormControl>;
  public cbxDetailReceiptCtrls: Map<number,FormControl>;

  public detailTargetAmountCtrls: Map<number,FormControl>;

  public receiptGridSettings: Array<GridSetting>;
  public billingGridSettings: Array<GridSetting>;

  public currencies: Array<Currency>;
  public customerFee: number;


  // 前頁からの引継ぎ情報（次・前ページ移動で変動なし）
  public collationSearch: CollationSearch = this.matchingService.collationInfo.collationSearch;

  public sections = this.matchingService.collationInfo.sections;
  public sectionsWithLoginUser = this.matchingService.collationInfo.sectionsWithLoginUser;
  public sectionIds = this.matchingService.collationInfo.sectionIds;

  public departments = this.matchingService.collationInfo.departments;
  public departmentsWithLoginUser = this.matchingService.collationInfo.departmentsWithLoginUser;
  public departmentIds = this.matchingService.collationInfo.departmentIds;

  public collationSetting = this.matchingService.collationInfo.collationSetting;

  public IsMatched = this.matchingService.collationInfo.isMatched;
  public IsNotAgency = StringUtil.IsNullOrEmpty(this.matchingService.collationInfo.collations[this.matchingService.collationInfo.individualIndexNo].paymentAgencyCode);

  // 前頁からの引継ぎ情報（次・前ページ移動で変動あり）
  public collationData: CollationData = this.matchingService.collationInfo.collations[this.matchingService.collationInfo.individualIndexNo];

  /// <summary>
  ///  消込時に 選択されている請求データの 親（子）得意先の 債権代表者フラグ
  ///  Collation.IsParent または 消 チェックで選択した代表得意先 が 債権代表者となっているかどうか
  ///  消込済データ表示時には 一切考慮する必用なし
  /// </summary>
  public GridSelectedParentCustomerIsParent = this.collationData.isParent;
  public GridSelectedParentCustomerCode = this.collationData.customerCode;

  ///////////////////////////////////////////////////////////

  /// <summary>
  ///  新規債権代表者となる得意先ID
  /// </summary>
  public NewSelectedParentCustomerId: number = 0;
  public GridSelectedParentCustomerId: number;

  public BillingGridChanged: boolean = false;
  public ReceiptGridChanged: boolean = false;
  public Search: Array<object>;
  public NoOfPrecision: number;

  public IsReceiptEdited: boolean;
  public loadingData = true;

  public machedCollationIndices = new Array<number>();

  public billingSortItem:SortItem;
  public receiptSortItem:SortItem;
  public gridSettingHelper = new GridSettingHelper();

  // 改行有無の制御
  public billingNowrapCtrl:FormControl;
  public receiptNowrapCtrl:FormControl;

  ngOnInit() {


    this.activatedRoute.paramMap.subscribe((params: ParamMap) => {
      // ルートパラメータと同じ要領で Matrix URI パラメータが取得できる
      this.paramProcess = params.get("process");
      this.paramFrom = parseInt(params.get("from"));


      this.setControlInit();
      this.setValidator();
      this.setFormatter();
      this.clear();

      this.LoadInitialDataAsync();


    });


  }


  public setControlInit() {

    // 全選択・全解除ボタン
    this.cbxInvoiceCheckAllCtrl = new FormControl(true);
    this.cbxReceiptCheckAllCtrl = new FormControl(true);

    this.billingTaxDifferenceCtrl = new FormControl("0"); // 消費税誤差
    this.discountCtrl = new FormControl("0"); // 歩引き率

    this.receiptTaxDifferenceCtrl = new FormControl("0");  // 消費税誤差
    this.transferFeeCtrl = new FormControl("0");   // 手数料


    // 改行有無の制御
    this.billingNowrapCtrl = new FormControl("true");
    this.receiptNowrapCtrl = new FormControl("true");


  }

  public setValidator() {
    this.MyFormGroup = new FormGroup({

      cbxInvoiceCheckAllCtrl:this.cbxInvoiceCheckAllCtrl,
      cbxReceiptCheckAllCtrl:this.cbxReceiptCheckAllCtrl,     


      billingTaxDifferenceCtrl: this.billingTaxDifferenceCtrl,
      discountCtrl: this.discountCtrl,
      receiptTaxDifferenceCtrl: this.receiptTaxDifferenceCtrl,
      transferFeeCtrl: this.transferFeeCtrl,

      billingNowrapCtrl: this.billingNowrapCtrl,
      receiptNowrapCtrl: this.receiptNowrapCtrl,
    });
  }

  public setFormatter() {
    FormatterUtil.setCurrencyFormatter(this.billingTaxDifferenceCtrl, false);
    FormatterUtil.setCurrencyFormatter(this.discountCtrl, false);
    FormatterUtil.setCurrencyFormatter(this.receiptTaxDifferenceCtrl, false);
    FormatterUtil.setCurrencyFormatter(this.transferFeeCtrl, false);
  }


  public LoadInitialDataAsync() {

    // Receipt/BillingのGridSetting、Currency
    let billingGridSettingResponse =
      this.gridSettingService.GetItems(GridId.BillingMatchingIndividual);

    let receiptGridSettingResponse =
      this.gridSettingService.GetItems(GridId.ReceiptMatchingIndividual);

    let currencyResponse =
      this.currencyService.GetItems();


    forkJoin(
      billingGridSettingResponse,
      receiptGridSettingResponse,
      currencyResponse,
    )
      .subscribe(responseList => {
        if (responseList != undefined && responseList.length == 3) {
          this.billingGridSettings = responseList[0];
          this.receiptGridSettings = responseList[1];

          let app = this.userInfoService.ApplicationControl;
          let isMatched = this.IsMatched;

          this.billingGridSettings = this.billingGridSettings.filter(function (element, index) {
            if (element.displayWidth <= 0) return false;

            if (!isMatched) {
              if (element.columnName == "MatchingAmount") return false;
            }
            else {
              if (element.columnName == "TargetAmount") return false;
            }
            return true;
          });

          this.receiptGridSettings = this.receiptGridSettings.filter(function (element, index) {
            if (element.displayWidth <= 0) return false;
            if (!app.useReceiptSection) {
              if (element.columnName == "SectionCode" || element.columnName == "SectionName") return false;
            }

            if (!isMatched) {
              if (element.columnName == "TargetAmount") return false;
            }
            else {
              if (element.columnName == "NettingState") return false;
            }
            return true;
          })

          this.currencies = responseList[2];



          this.LoadCollationData();
        }
        else {
          this.processCustomResult = this.processResultService.processAtFailure(
            this.processCustomResult, MSG_ERR.SOMETHING_ERROR.replace(MSG_ITEM_NUM.FIRST, '初期化'),
            this.partsResultMessageComponent);
        }
      })

  }


  public clear() {
    this.MyFormGroup.reset();

    if (this.matchingService.collationInfo.displayTypeVertical) {
      this.displayTypeButton = "左右表示";
    }
    else {
      this.displayTypeButton = "上下表示";
    }
  


    this.cbxAdvanceReceived = false;
    this.advanceReceived = "繰越を請求残とする";


    this.cbxInvoiceCheckAllCtrl.setValue(true);
    this.cbxReceiptCheckAllCtrl.setValue(true);  

    this.billingSortItem = null;
    this.receiptSortItem = null;

    this.billingPanelOpenState = true;
    this.receiptPanelOpenState = true;  
  }

  public LoadCollationData() {

    let componentFactory = this.componentFactoryResolver.resolveComponentFactory(ModalRouterProgressComponent);
    let modalRouterProgressComponentRef = this.viewContainerRef.createComponent(componentFactory);
    //modalRouterProgressComponentRef.destroy();


    this.loadingData = true;
    // 初期データの設定
    forkJoin(
      this.LoadBillingsAsync(),
      this.LoadReceiptsAsync()
    )
      .subscribe(responseList => {
        if (responseList != undefined && responseList.length == 2) {
          this.billings = responseList[0];
          this.receipts = responseList[1];

          if (this.paramProcess == "search") {
            // 入金検索から遷移した場合
            if (this.paramFrom == ComponentId.PD0501) {

              // 検索結果の入金データ追加する。
              this.matchingService.collationInfo.searchReceipts.forEach(element => {
                // 検索から来た場合はチェックボックスの初期値はOffにする。
                let tmpElement = <IndividualReceipt>element;
                tmpElement.checkBox = CheckBoxStatus.OFF;
                this.receipts.push(tmpElement);
              });

              // 請求検索のデータがあればそれも追加する。
              if(this.matchingService.collationInfo.searchBillings!=null && this.matchingService.collationInfo.searchBillings.length>0){
                this.matchingService.collationInfo.searchBillings.forEach(element => {
                  // 検索から来た場合はチェックボックスの初期値はOffにする。
                  let tmpElement = <IndividualBilling>element;
                  if(tmpElement.checkBox==undefined){tmpElement.checkBox = CheckBoxStatus.OFF};
                  this.billings.push(tmpElement);
                });
              }
            }
            // 請求検索から遷移した場合
            else if (this.paramFrom == ComponentId.PC0301) {

              // 検索結果の請求データ追加する。
              this.matchingService.collationInfo.searchBillings.forEach(element => {
                // 検索から来た場合はチェックボックスの初期値はOffにする。
                let tmpElement = <IndividualBilling>element;
                tmpElement.checkBox = CheckBoxStatus.OFF;
                this.billings.push(tmpElement);
              });

              // 入金検索のデータがあればそれも追加する。
              if(this.matchingService.collationInfo.searchReceipts!=null && this.matchingService.collationInfo.searchReceipts.length>0){
                this.matchingService.collationInfo.searchReceipts.forEach(element => {
                  // 検索から来た場合はチェックボックスの初期値はOffにする。
                  let tmpElement = <IndividualReceipt>element;
                  if(tmpElement.checkBox==undefined){tmpElement.checkBox = CheckBoxStatus.OFF};
                  this.receipts.push(tmpElement);
                });
              }
            }
          }
          else if (this.paramProcess == "registry") {
            // 入金入力から遷移した場合
            if (this.paramFrom == ComponentId.PD0301) {

              // 入力結果の入金データ追加する。
              this.matchingService.collationInfo.registryReceipts.forEach(element => {
                let tmpReceipt: IndividualReceipt = <IndividualReceipt>element;
                tmpReceipt.checkBox = CheckBoxStatus.ON;
                this.receipts.push(tmpReceipt);
              });

              // 入金検索のデータがあればそれも追加する。
              if(this.matchingService.collationInfo.searchReceipts!=null && this.matchingService.collationInfo.searchReceipts.length>0){
                this.matchingService.collationInfo.searchReceipts.forEach(element => {
                  // 検索から来た場合はチェックボックスの初期値はOffにする。
                  let tmpElement = <IndividualReceipt>element;
                  if(tmpElement.checkBox==undefined){tmpElement.checkBox = CheckBoxStatus.OFF};
                  this.receipts.push(tmpElement);
                });
              }

              // 請求検索のデータがあればそれも追加する。
              if(this.matchingService.collationInfo.searchBillings!=null && this.matchingService.collationInfo.searchBillings.length>0){
                this.matchingService.collationInfo.searchBillings.forEach(element => {
                  // 検索から来た場合はチェックボックスの初期値はOffにする。
                  let tmpElement = <IndividualBilling>element;
                  if(tmpElement.checkBox==undefined){tmpElement.checkBox = CheckBoxStatus.OFF};
                  this.billings.push(tmpElement);
                });
              }

            }
          }
          else if (this.paramProcess == "update") {
            if (this.paramFrom == ComponentId.PD0301) {
              let newReceipts: Array<IndividualReceipt> = new Array<IndividualReceipt>();
              for (let index = 0; index < this.receipts.length; index++) {
                if (this.receipts[index].id == this.matchingService.collationInfo.editRecipt.id) {
                  let tmpReceipt: IndividualReceipt = <IndividualReceipt>this.matchingService.collationInfo.editRecipt;
                  tmpReceipt.checkBox = CheckBoxStatus.ON;
                  newReceipts.push(tmpReceipt);
                }
                else {
                  newReceipts.push(this.receipts[index]);
                }
              }
            }
          }
          else if (this.paramProcess == "delete") {
            if (this.paramFrom == ComponentId.PD0301) {
              let deleteReceiptId = this.matchingService.collationInfo.deleteReceiptId;
              this.receipts.filter(function (value, index) {
                if (deleteReceiptId == value.id) { return false; }
                else { return true; }
              });
            }
          }

          this.paramFrom = null;
          this.paramProcess = null;
          this.InitializeControlsEnabled();

          this.loadingData = false;



          modalRouterProgressComponentRef.destroy();

        }
        else {

          modalRouterProgressComponentRef.destroy();

          this.processCustomResult = this.processResultService.processAtFailure(
            this.processCustomResult, MSG_ERR.SOMETHING_ERROR.replace(MSG_ITEM_NUM.FIRST, '初期化'),
            this.partsResultMessageComponent);

        }
      });

  }

  public InitializeControlsEnabled() {


    this.ReceiptRecordedAtTo = this.collationSearch.recordedAtTo;
    this.BillingDueAtTo = this.collationSearch.dueAtTo;


    if (!this.IsMatched) {
      this.customerName = this.collationData.dispCustomerName;
      this.GridSelectedParentCustomerId = this.collationData.customerId;
      this.GridSelectedParentCustomerCode = this.collationData.customerCode;
      this.GridSelectedParentCustomerIsParent = this.collationData.isParent;
    }
    else {
      this.customerName = this.collationData.dispCustomerName;
      this.GridSelectedParentCustomerId = this.collationData.customerId;
      this.GridSelectedParentCustomerCode = this.collationData.customerCode;
    }

    this.SetBillingData();




    this.SetReceiptData();

  }


  public SetBillingData() {
    //////////////////////////////////////////////////
    this.cbxDetailBillingCtrls = new Map();
    this.detailTargetAmountCtrls = new Map();

    for (let index = 0; index < this.billings.length; index++) {
      let tmpCbxDetailBillingCtrl = new FormControl();
      this.cbxDetailBillingCtrls.set(this.billings[index].id, tmpCbxDetailBillingCtrl);
      if (this.IsMatched) this.cbxDetailBillingCtrls.get(this.billings[index].id).disable();
      this.MyFormGroup.removeControl("cbxDetailBillingCtrl" + index);
      this.MyFormGroup.addControl("cbxDetailBillingCtrl" + index, this.cbxDetailBillingCtrls.get(this.billings[index].id));

      let tmpDetailTargetAmountCtrl = new FormControl();
      tmpDetailTargetAmountCtrl.setValue(this.pipe.transform(this.billings[index].targetAmount));
      FormatterUtil.setCurrencyFormatter(tmpDetailTargetAmountCtrl);
      this.detailTargetAmountCtrls.set(this.billings[index].id,tmpDetailTargetAmountCtrl);
      if (this.IsMatched) this.detailTargetAmountCtrls.get(this.billings[index].id).disable();
      this.MyFormGroup.removeControl("detailTargetAmountCtrl" + index);
      this.MyFormGroup.addControl("detailTargetAmountCtrl" + index, this.detailTargetAmountCtrls.get(this.billings[index].id));

      if (this.billings[index].checkBox == undefined) this.billings[index].checkBox = CheckBoxStatus.ON;

    }

    this.discountCtrl.setValue("0"); // 歩引き率

    this.billingAmountTotal = 0; // 総額
    this.billingTargetAmount = 0;  // 消込対象額
    this.billingMatchingAmount = 0;  // 消込予定額
    this.billingMatchingRemain = 0;  // 消込残

    // 表示の際は入力欄を無効にする。
    if (this.IsMatched) {
      this.billingTaxDifferenceCtrl.disable(); // 消費税誤差
      this.discountCtrl.disable(); // 歩引き率

    }

    var taxDiff = this.collationData.taxDifference;
    if (taxDiff > 0) {
      this.billingTaxDifferenceCtrl.setValue(taxDiff);
    }
    else {
      this.billingTaxDifferenceCtrl.setValue(0);
    }

    this.CalculateAmount(false, true);
  }

  public SetReceiptData() {
    //////////////////////////////////////////////////
    this.cbxDetailReceiptCtrls = new Map();

    for (let index = 0; index < this.receipts.length; index++) {
      let tmpCbxDetailReceiptCtrl = new FormControl();
      this.cbxDetailReceiptCtrls.set(this.receipts[index].id, tmpCbxDetailReceiptCtrl);

      if (this.IsMatched) this.cbxDetailReceiptCtrls.get(this.receipts[index].id).disable();

      this.MyFormGroup.removeControl("cbxDetailReceiptCtrl" + index);
      this.MyFormGroup.addControl("cbxDetailReceiptCtrl" + index, this.cbxDetailReceiptCtrls.get(this.receipts[index].id));

      if (this.receipts[index].checkBox == undefined) this.receipts[index].checkBox = CheckBoxStatus.ON;
    }

    this.transferFeeCtrl.setValue(this.collationData.bankTransferFee);
    var taxDiff = this.collationData.taxDifference;
    if (taxDiff < 0) {
      this.receiptTaxDifferenceCtrl.setValue(taxDiff * -1);
    }
    else {
      this.receiptTaxDifferenceCtrl.setValue(0);
    }


    this.receiptAmountTotal = 0; // 総額
    this.receiptTargetAmount = 0;  // 消込対象額
    this.receiptMatchingAmount = 0;  // 消込予定額
    this.receiptMatchingRemain = 0;  // 消込差額  

    // 表示の際は入力欄を無効にする。
    if (this.IsMatched) {
      this.receiptTaxDifferenceCtrl.disable(); // 消費税誤差
      this.transferFeeCtrl.disable();  // 手数料

    }

    this.CalculateAmount(true, false);
  }

  public CalculateAmount(skipBillingSide: boolean, skipReceiptSide: boolean) {
    if (!skipBillingSide) this.CalculateBillingTotal();
    if (!skipReceiptSide) this.CalculateReceiptTotal();
    this.CalculateMatchingTargetAmount();
  }

  public CalculateBillingTotal() {
    let items = this.billings;
    let totalCount = this.billings.length;
    let billingTotal = 0;
    let targetTotal = 0;
    let discountTotal = 0;

    items = items.filter(function (item) {
      return item.checkBox == CheckBoxStatus.ON;
    });

    let checkedCount = items.length;
    for (let index = 0; index < checkedCount; index++) {
      billingTotal += items[index].billingAmount;
      targetTotal += (this.IsMatched ? items[index].assignmentAmount : items[index].targetAmount);
      discountTotal += items[index].discountAmount;
    }
    this.billingCount = `${checkedCount} / ${totalCount}`;

    this.discountCtrl.setValue(discountTotal);
    targetTotal += ((NumberUtil.ParseInt(this.billingTaxDifferenceCtrl.value)) - (NumberUtil.ParseInt(this.discountCtrl.value)));
    this.billingAmountTotal = billingTotal;
    this.billingTargetAmount = targetTotal;
  }

  /// <summary>合計計算「入金側」</summary>
  public CalculateReceiptTotal() {
    var items = this.receipts;
    var totalCount = items.length;
    var receiptSum = 0;
    var remainSum = 0;

    items = items.filter(function (item) {
      return item.checkBox == CheckBoxStatus.ON;
    });

    var checkedCount = items.length;
    for (let index = 0; index < checkedCount; index++) {
      receiptSum += items[index].receiptAmount;
      remainSum += (this.IsMatched ? items[index].assignmentAmount : items[index].remainAmount);
    }

    this.receiptCount = `${checkedCount} / ${totalCount}`;
    remainSum += ((NumberUtil.ParseInt(this.receiptTaxDifferenceCtrl.value)) + (NumberUtil.ParseInt(this.transferFeeCtrl.value)));
    this.receiptAmountTotal = receiptSum;
    this.receiptTargetAmount = remainSum;
  }

  /// <summary>消込予定額計算</summary>
  public CalculateMatchingTargetAmount() {
    var billingTarget = this.billingTargetAmount;
    var receiptTarget = this.receiptTargetAmount;

    var billingAllMinus = this.billings.filter(function (element, index) {
      if (element.targetAmount < 0) {
        return true;
      }
      else {
        return false;
      }

    });

    var receiptAllMinus = this.receipts.filter(function (element, index) {
      if (element.remainAmount < 0) {
        return true;
      }
      else {
        return false;
      }

    });

    let allMinus = billingAllMinus.length > 0 && receiptAllMinus.length > 0;

    let targetAmount = 0;
    if (allMinus) {
      targetAmount = billingTarget > receiptTarget ? billingTarget : receiptTarget;
    }
    else {
      targetAmount = billingTarget > receiptTarget ? receiptTarget : billingTarget;
    }

    this.billingMatchingAmount = targetAmount;
    this.receiptMatchingAmount = targetAmount;
    this.receiptMatchingRemain = receiptTarget - targetAmount; //消込予定額
    this.billingMatchingRemain = billingTarget - targetAmount; //消込予定額
  }


  /*
  public OpenIndividualComponent(){

    // 確認ダイアログの表示
    let componentFactory = this.componentFactoryResolver.resolveComponentFactory(ModalIndividualCollateComponent);
    let componentRef = this.viewContainerRef.createComponent(componentFactory);

    componentRef.instance.billings = this.billings;
    componentRef.instance.receipts = this.receipts;
    componentRef.instance.IsMatched = this.IsMatched;

    componentRef.instance.cbxDetailReceiptCtrls = this.cbxDetailReceiptCtrls;
    componentRef.instance.cbxDetailBillingCtrls = this.cbxDetailBillingCtrls;

    componentRef.instance.Closing.subscribe(() => {
    
      componentRef.destroy();
    });  
  }
  */

  public onDoubleClickBillingDetail(index: number, setting: GridSetting) {
    if (setting.columnName === 'Memo') {
      this.openMemoModal(CategoryType.Billing, index);
    }
    if (setting.columnName === 'CustomerCode' ||
        setting.columnName === 'CustomerName'
    ) {
      this.openCustomerModal(index);
    }
  }

  public openCustomerModal(index: number) {

    this.customerMasterService.GetItemsById(this.billings[index].customerId)
      .subscribe(response => {
        if (response != PROCESS_RESULT_RESULT_TYPE.FAILURE) {
          let componentFactory = this.componentFactoryResolver.resolveComponentFactory(ModalCustomerDetailComponent);
          let componentRef = this.viewContainerRef.createComponent(componentFactory);

          componentRef.instance.Customer = response[0]; // 0～99のランダム値を渡す
          componentRef.instance.ComponentStatus = COMPONENT_STATUS_TYPE.UPDATE;
          componentRef.instance.fromPageId = ComponentId.PE0102;
          componentRef.instance.processModalCustomResult = this.processModalCustomResult;

          this.customerMasterService.selectCustmer = response[0];

          componentRef.instance.Closing.subscribe(() => {
            componentRef.destroy();
          });
        }
      });
  }


  public openMemoModal(categoryType: CategoryType, index: number) {

    // 表示＋入金メモの場合は修正不可にする。
    if (this.IsMatched && categoryType == CategoryType.Receipt) {
      return;
    }

    let componentFactory = this.componentFactoryResolver.resolveComponentFactory(ModalIndividualMemoComponent);
    let componentRef = this.viewContainerRef.createComponent(componentFactory);

    componentRef.instance.categoryType = categoryType;
    if (categoryType == CategoryType.Billing) {
      componentRef.instance.memo = this.billings[index].memo;
    }
    else if (categoryType == CategoryType.Receipt) {
      componentRef.instance.memo = this.receipts[index].receiptMemo;
    }


    componentRef.instance.Closing.subscribe(() => {

      if (componentRef.instance.ModalStatus == MODAL_STATUS_TYPE.OK) {
        if (categoryType == CategoryType.Billing) {
          this.billings[index].memo = componentRef.instance.memoCtrl.value;

          let billingMemo = new BillingMemo();
          billingMemo.billingId = this.billings[index].billingId;
          billingMemo.memo = this.billings[index].memo;

          this.billingService.SaveMemo(billingMemo)
            .subscribe(response => {
              if (response == undefined) {
                this.processCustomResult = this.processResultService.processAtFailure(
                  this.processCustomResult, MSG_ERR.SAVE_ERROR, this.partsResultMessageComponent);
              }
            });
        }
        else if (categoryType == CategoryType.Receipt) {
          this.receipts[index].receiptMemo = componentRef.instance.memoCtrl.value;

          let receiptMemo = new ReceiptMemo();
          receiptMemo.receiptId = this.receipts[index].id;
          receiptMemo.memo = this.receipts[index].receiptMemo;

          this.receiptService.SaveMemo(receiptMemo)
            .subscribe(response => {
              if (response == undefined) {
                this.processCustomResult = this.processResultService.processAtFailure(
                  this.processCustomResult, MSG_ERR.SAVE_ERROR, this.partsResultMessageComponent);
              }
            });
        }
      }
      componentRef.destroy();
    });

  }

  public LoadBillingsAsync(): Observable<any> {

    var option = this.GetBillingSearchCondition();

    return this.matchingService.SearchBillingData(option)
  }

  public LoadReceiptsAsync(): Observable<any> {

    var option = this.GetReceiptSearchCondtion();

    return this.matchingService.SearchReceiptData(option)
  }

  public GetBillingSearchCondition(): MatchingBillingSearch {

    var option = new MatchingBillingSearch();

    option.companyId = this.userInfoService.Company.id;
    option.billingDataType = this.collationSearch.billingType;
    option.dueAtFrom = this.collationSearch.dueAtFrom;
    option.dueAtTo = this.collationSearch.dueAtTo;
    option.useDepartmentWork = this.collationSearch.useDepartmentWork;
    option.clientKey = this.collationSearch.clientKey;
    option.useCashOnDueDates = this.userInfoService.ApplicationControl.useCashOnDueDates;

    if (this.IsMatched) {
      option.currencyId = this.collationData.currencyId;
      option.paymentAgencyId = this.collationData.paymentAgencyId;
      option.parentCustomerId = this.collationData.customerId;
      option.isParent = this.GridSelectedParentCustomerIsParent;
      option.matchingHeaderId = this.collationData.id;
    }
    else {
      option.currencyId = this.collationData.currencyId;
      option.paymentAgencyId = this.collationData.paymentAgencyId;
      option.parentCustomerId = this.collationData.customerId;
      option.isParent = this.collationData.isParent;

    }
    return option;
  }


  public GetReceiptSearchCondtion(): MatchingReceiptSearch {
    var option = new MatchingReceiptSearch();
    option.companyId = this.userInfoService.Company.id;

    option.recordedAtFrom = this.collationSearch.recordedAtFrom;
    option.recordedAtTo = this.collationSearch.recordedAtTo;
    option.billingDataType = this.collationSearch.billingType;
    option.useCashOnDueDates = this.userInfoService.ApplicationControl.useCashOnDueDates;
    option.useScheduledPayment = this.userInfoService.ApplicationControl.useScheduledPayment;
    option.clientKey = this.collationSearch.clientKey;
    if (this.IsMatched) {
      option.paymentAgencyId = this.collationData.paymentAgencyId;
      option.parentCustomerId = this.collationData.customerId;
      option.matchingHeaderId = this.collationData.id;
    }
    else {
      option.paymentAgencyId = this.collationData.paymentAgencyId;
      option.parentCustomerId = this.collationData.customerId;
      option.payerName = StringUtil.IsNullOrEmpty(this.collationData.payerName) ? "" : this.collationData.payerName;
      option.currencyId = this.collationData.currencyId;
    }

    return option;
  }


  public setAdvanceReceived() {
    this.cbxAdvanceReceived = !this.cbxAdvanceReceived;

    if (this.cbxAdvanceReceived) {
      this.advanceReceived = "繰越を前受とする";
    }
    else {
      this.advanceReceived = "繰越を請求残とする";
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
      case BUTTON_ACTION.MATCHING:
        this.matching();
        break;
      case BUTTON_ACTION.PREV:
        this.prevCollation();
        break;
      case BUTTON_ACTION.NEXT:
        this.nextCollation();
        break;
      case BUTTON_ACTION.PRINT:
        this.print();
        break;
      case BUTTON_ACTION.REGISTRY_RECEIPT:
        this.registryReceipt();
        break;
      case BUTTON_ACTION.SEARCH_BILLING:
        this.searchBilling();
        break;
      case BUTTON_ACTION.SEARCH_RECEIPT:
        this.searchReceipt();
        break;
      case BUTTON_ACTION.EXPORT:
        this.export();
        break;

    }
  }

  public back() {
    this.router.navigate(['main/PE0101', { "process": "back" }]);
  }

  public modalRouterProgressComponentRef:ComponentRef<ModalRouterProgressComponent>;
  public startMatching(){
    Console.log("startMatching");

    let componentFactory = this.componentFactoryResolver.resolveComponentFactory(ModalRouterProgressComponent);
    this.modalRouterProgressComponentRef = this.viewContainerRef.createComponent(componentFactory);
  }

  public endSubProcess(processName:string,endFlag:boolean,subject:Subject<boolean>){

    Console.log("endProcess:processName=" + processName + ",endFlag=" + endFlag);
    this.modalRouterProgressComponentRef.destroy();

    subject.next(endFlag);
    subject.complete();

    if(!endFlag){
      this.modalRouterProgressComponentRef.destroy();
    }
  }

  public endSubProcessAny(processName:string,endObj:any,subject:Subject<any>){

    Console.log("endProcess:processName=" + processName + ",endFlag=" + endObj);
    this.modalRouterProgressComponentRef.destroy();

    subject.next(endObj);
    subject.complete();

    if(endObj==null){
      this.modalRouterProgressComponentRef.destroy();
    }
  }  

  public endMatching(){
    Console.log("startMatching");
    this.modalRouterProgressComponentRef.destroy();
  }

  public matching() {

    var newChildren = new Map<number, Customer>();
    var groups = new Array<CustomerGroup>();

    // Matchingの開始
    this.startMatching();
    Console.log("matching start");

    // 入力内容のチェック
    Console.log("ValidateForMatching start:");
    this.ValidateForMatching(newChildren, groups)
      .subscribe(response => {
        let result: boolean = response

        Console.log("ValidateForMatching end:" + result);

        if (!result) {
          this.endMatching();
          return false
        };

        let checkedReceipts = this.receipts.filter(function (value, index) {
          if (value.checkBox == CheckBoxStatus.ON) return true;
          else return false;
        });

        let checkedBillings = this.billings.filter(function (value, index) {
          if (value.checkBox == CheckBoxStatus.ON) return true;
          else return false;
        });

        // 前受金がある場合は消込日を入力する。
        let existAdvanceReceived: boolean = false;
        checkedReceipts.forEach(element => {
          if (element.originalReceiptId != null && element.originalReceiptId > 0) {
            existAdvanceReceived = true;
          }
        });

        // 参照渡しをするために配列を利用。
        let recordedAts: Array<string> = new Array<string>();

        if (existAdvanceReceived) {
          Console.log("ConfirmMatchingRecordedAt start");
          this.ConfirmMatchingRecordedAt(existAdvanceReceived, recordedAts)
            .subscribe(response => {
              let result: boolean = response;
              Console.log("ConfirmMatchingRecordedAt end:" + result);

              if (!result) {
                this.endMatching();
                return false;
              }

              // 消込を継続する
              this.MatcihingSub(
                newChildren,
                checkedBillings, checkedReceipts,
                recordedAts,
                groups
              )
                .subscribe(response => {
                  this.endMatching();
                  result = response;
                  return response;
                });

            });
        }
        else {
          // 消込を継続する
          this.MatcihingSub(
            newChildren,
            checkedBillings, checkedReceipts,
            recordedAts,
            groups
          )
            .subscribe(response => {
              this.endMatching();
              result = response;
              return response;

            });
        }

      });

    Console.log("matching end");

  }

  public MatcihingSub(
    newChildren: Map<number, Customer>,
    checkedBillings: Array<IndividualBilling>, checkedReceipts: Array<IndividualReceipt>,
    recordedAts: Array<string>,
    groups: Array<CustomerGroup>
  ): Subject<boolean> {

    let matchingSub: Subject<boolean> = new Subject();

    var groupchildCusId: Array<number> = new Array<number>();


    if (this.GridSelectedParentCustomerIsParent == 1) {

      // 追加するカスタマーコードを表示用に纏める。
      let addGroupCustomerCode: string = "";
      let addGroupCustomerId = new Array<number>();
      newChildren.forEach((value, key) => {
        addGroupCustomerCode += value.code + ",";
        addGroupCustomerId.push(value.id);
      });
      addGroupCustomerCode = addGroupCustomerCode.slice(0, -1);

      // 追加するカスタマーコードがある場合は確認する。
      if (!StringUtil.IsNullOrEmpty(addGroupCustomerCode)) {
        Console.log("ConfirmCustomerGroup start");
        this.ConfirmAddCustomerGroup(addGroupCustomerCode, addGroupCustomerId, groupchildCusId)
          .subscribe(response => {
            let result: boolean = response;
            Console.log("ConfirmCustomerGroup end:" + result);

            if (!result) {

              this.endSubProcess("MatcihingSub",false,matchingSub);
            }

            // 消込を継続する
            this.MatchingSub2(
              newChildren,
              checkedBillings, checkedReceipts,
              recordedAts,
              groups, groupchildCusId
            )
              .subscribe(response => {
                result = response;
                this.endSubProcess("MatcihingSub(MatchingSub2())",result,matchingSub);
              });
          });
      }
      else {
        let result: boolean;

        // 消込を継続する
        this.MatchingSub2(
          newChildren,
          checkedBillings, checkedReceipts,
          recordedAts,
          groups, groupchildCusId
        )
          .subscribe(response => {
            result = response;
            this.endSubProcess("MatcihingSub(MatchingSub2())",result,matchingSub);
          });
      }
    }
    else {
      let result: boolean;

      if (this.GridSelectedParentCustomerIsParent == 0
        && newChildren.size > 0) {
        let newChildrenExit: boolean = false;
        newChildren.forEach(element => {
          if (this.GridSelectedParentCustomerId != element.parentCustomerId) {
            newChildrenExit = true;
          }
        });

        if (newChildrenExit) {
          // 代表債権者の選択
          Console.log("ConfirmCustomerGroup ConfirmCustomerGroupSub start");
          this.ConfirmParentCustomerGroup(newChildren, groupchildCusId)
            .subscribe(response => {
              Console.log("ConfirmCustomerGroup ConfirmCustomerGroupSub end");
              let result: boolean = response;

              if (!result) {
                this.endSubProcess("MatcihingSub(ConfirmParentCustomerGroup())",false,matchingSub);
              }
              else{

                // 消込を継続する
                this.MatchingSub2(
                  newChildren,
                  checkedBillings, checkedReceipts,
                  recordedAts,
                  groups, groupchildCusId
                )
                  .subscribe(response => {
                    result = response;
                    this.endSubProcess("MatcihingSub(MatchingSub2())",result,matchingSub);
                  });
              }

            });
        }
        else {
          // 消込を継続する
          this.MatchingSub2(
            newChildren,
            checkedBillings, checkedReceipts,
            recordedAts,
            groups, groupchildCusId
          )
            .subscribe(response => {
              result = response;
              this.endSubProcess("MatcihingSub(MatchingSub2())",result,matchingSub);
            });
        }
      }
      else {
        // 消込を継続する
        this.MatchingSub2(
          newChildren,
          checkedBillings, checkedReceipts,
          recordedAts,
          groups, groupchildCusId
        )
          .subscribe(response => {
            result = response;
            this.endSubProcess("MatcihingSub(MatchingSub2())",result,matchingSub);
          });
      }


    }

    return matchingSub;
  }

  public MatchingSub2(
    newChildren: Map<number, Customer>,
    checkedBillings: Array<IndividualBilling>, checkedReceipts: Array<IndividualReceipt>,
    recordedAts: Array<string>,
    groups: Array<CustomerGroup>, groupchildCusId: Array<number>
  ): Subject<boolean> {

    let matchingSub2 = new Subject<boolean>();

    let option = this.collationSearch;
    Console.log("SolveMatchingPattern start");
    this.SolveMatchingPattern(
      option,
      checkedBillings, checkedReceipts,
      recordedAts,
      groupchildCusId,
      newChildren, groups
    )
      .subscribe(response => {
        let source: MatchingSource = response;
        Console.log("SolveMatchingPattern end:" + source);
        if (source == null || source.matchings == null) {

          this.endSubProcess("MatchingSub2(SolveMatchingPattern())",false,matchingSub2);
        }
        else {
          Console.log("SaveMatchingAsync start");
          this.SaveMatchingAsync(source)
            .subscribe(response => {
              Console.log("SaveMatchingAsync end:" + response);

              let result: MatchingResult = response;

              if (result == null) {
                this.processResultService.processAtFailure(
                  this.processCustomResult, MSG_ERR.MATCHING_ERROR, this.partsResultMessageComponent);

                this.endSubProcess("MatchingSub2(SaveMatchingAsync())",false,matchingSub2);
              }
              else if (result.matchingErrorType == MatchingErrorType.BillingRemainChanged) {
                this.processResultService.processAtWarning(
                this.processCustomResult, MSG_WNG.INCLUDE_OTHER_USER_MATCHED_DATA.replace(MSG_ITEM_NUM.FIRST, "請求"),
                this.partsResultMessageComponent);

                this.endSubProcess("MatchingSub2(SaveMatchingAsync(請求))",false,matchingSub2);
              }
              else if (result.matchingErrorType == MatchingErrorType.BillingDiscountChanged) {
                this.processResultService.processAtWarning(
                this.processCustomResult, MSG_WNG.INCLUDE_OTHER_USER_MATCHED_DATA.replace(MSG_ITEM_NUM.FIRST, "歩引"),
                this.partsResultMessageComponent);

                this.endSubProcess("MatchingSub2(SaveMatchingAsync(歩引))",false,matchingSub2);
              }
              else if (result.matchingErrorType == MatchingErrorType.ReceiptRemainChanged) {
                this.processResultService.processAtWarning(
                this.processCustomResult, MSG_WNG.INCLUDE_OTHER_USER_MATCHED_DATA.replace(MSG_ITEM_NUM.FIRST, "入金"),
                this.partsResultMessageComponent);

                this.endSubProcess("MatchingSub2(SaveMatchingAsync(入金))",false,matchingSub2);
              }
              else if (result.matchingErrorType == MatchingErrorType.PostProcessError) {
                this.processResultService.processAtFailure(
                  this.processCustomResult, MSG_ERR.POST_PROCESS_FAILURE, this.partsResultMessageComponent);

                this.endSubProcess("MatchingSub2(SaveMatchingAsync(PostProcessError))",false,matchingSub2);
              }
              else {
                this.machedCollationIndices.push(this.matchingService.collationInfo.individualIndexNo);

                this.transferFeeCtrl.setValue("0");
                this.billingTaxDifferenceCtrl.setValue("0");
                this.receiptTaxDifferenceCtrl.setValue("0");

                this.LoadCollationData();

                this.processResultService.processAtSuccess(
                  this.processCustomResult, MSG_INF.PROCESS_FINISH, this.partsResultMessageComponent);

                this.endSubProcess("MatchingSub2()",true,matchingSub2);

              }
            });
        }

      });

    return matchingSub2;

  }




  /// <summary>消込前 債権代表者グループ 確定処理
  /// グループに含まれない 子の得意先をグループに加えるか
  /// グループに属していない異なる得意先の代表者をどれに設定するか
  /// </summary>
  /// <param name="newChildren"></param>
  /// <param name="groupchildCusId"></param>
  /// <returns></returns>
  public ConfirmAddCustomerGroup(
    // 追加するカスタマーコードを表示用に纏める。
    addGroupCustomerCode: string,
    addGroupCustomerId: Array<number>,
    groupchildCusId: Array<number>
  ): Subject<boolean> {

    let confirmCustomerGroup: Subject<boolean> = new Subject();

    let matchingComponentFactory = this.componentFactoryResolver.resolveComponentFactory(ModalConfirmComponent);
    let matchingComponentRef = this.viewContainerRef.createComponent(matchingComponentFactory);
    matchingComponentRef.instance.PrefixDetail = "得意先コード：" + addGroupCustomerCode + "は債権代表者グループに登録されていません。";
    matchingComponentRef.instance.ActionName = this.customerName + "の債権代表者グループへの登録";


    matchingComponentRef.instance.Closing.subscribe(() => {
      if (matchingComponentRef.instance.ModalStatus == MODAL_STATUS_TYPE.OK) {
        addGroupCustomerId.forEach(element => { groupchildCusId.push(element) });
        matchingComponentRef.destroy();

        this.endSubProcess("ConfirmAddCustomerGroup",true,confirmCustomerGroup);

      }
      else {
        this.processResultService.processAtWarning(
          this.processCustomResult, MSG_WNG.CANCEL_PROCESS.replace(MSG_ITEM_NUM.FIRST, "個別消込処理"),
          this.partsResultMessageComponent);
        matchingComponentRef.destroy();
        this.endSubProcess("ConfirmAddCustomerGroup",false,confirmCustomerGroup);
      }
    });


    confirmCustomerGroup = new Subject();
    return confirmCustomerGroup;
  }

  /// <summary>消込前 債権代表者グループ 確定処理
  /// グループに含まれない 子の得意先をグループに加えるか
  /// グループに属していない異なる得意先の代表者をどれに設定するか
  /// </summary>
  /// <param name="newChildren"></param>
  /// <param name="groupchildCusId"></param>
  /// <returns></returns>
  public ConfirmParentCustomerGroup(newChildren: Map<number, Customer>, groupchildCusId: Array<number>): Subject<boolean> {

    let confirmCustomerGroupSub: Subject<boolean> = new Subject();

    let tmpNewcustomer = new Customer();
    tmpNewcustomer.id = this.GridSelectedParentCustomerId;
    tmpNewcustomer.code = this.GridSelectedParentCustomerCode;
    tmpNewcustomer.name = this.customerName;
    tmpNewcustomer.isParent = 0
    newChildren.set(this.GridSelectedParentCustomerId, tmpNewcustomer);

    // 債権代表者の選択
    let selectParetnComponentFactory = this.componentFactoryResolver.resolveComponentFactory(ModalSelectParentCustomerComponent);
    let selectParetnComponentRef = this.viewContainerRef.createComponent(selectParetnComponentFactory);

    selectParetnComponentRef.instance.customers = new Array<Customer>();

    newChildren.forEach((value, key) => { selectParetnComponentRef.instance.customers.push(value) });

    selectParetnComponentRef.instance.Closing.subscribe(() => {
      let parentId = 0;
      if (selectParetnComponentRef.instance.ModalStatus == MODAL_STATUS_TYPE.OK) {
        parentId = selectParetnComponentRef.instance.parentCustomerIdCtrl.value;
        this.NewSelectedParentCustomerId = parentId;

        newChildren.forEach((value, key) => {
          if (key != parentId) {
            groupchildCusId.push(key);
          }
        });

        selectParetnComponentRef.destroy();
        this.endSubProcess("ConfirmParentCustomerGroup()",true,confirmCustomerGroupSub);
      }
      else {
        this.processResultService.processAtWarning(
          this.processCustomResult, MSG_WNG.CANCEL_PROCESS.replace(MSG_ITEM_NUM.FIRST, "個別消込処理"),
          this.partsResultMessageComponent);
        selectParetnComponentRef.destroy();
        this.endSubProcess("ConfirmParentCustomerGroup()",false,confirmCustomerGroupSub);
      }
    });


    confirmCustomerGroupSub = new Subject();
    return confirmCustomerGroupSub;
  }


  /// <summary>
  ///  前受消込時に伝票日付を取得する処理
  /// </summary>
  /// <param name="existAdvanceReceived">日付入力が必要かのフラグ</param>
  /// <param name="recordedAt">入力された日付（参照渡しとして利用しているので、オブジェクトはNewしないこと）</param>
  /// <returns></returns>
  public ConfirmMatchingRecordedAt(existAdvanceReceived: boolean, recordedAts: Array<string>): Subject<boolean> {

    let confirmMatchingRecordedAt: Subject<boolean> = new Subject();

    // 前受あり時の消込処理日時の入力
    let advancedComponentFactory = this.componentFactoryResolver.resolveComponentFactory(ModalMatchingRecordedAtComponent);
    let advancedComponentRef = this.viewContainerRef.createComponent(advancedComponentFactory);
    advancedComponentRef.instance.CollationSetting = this.collationSetting;
    advancedComponentRef.instance.AdvancedDate = this.getAdvanceDate();
    
    advancedComponentRef.instance.Closing.subscribe(() => {

      if (advancedComponentRef.instance.ModalStatus == MODAL_STATUS_TYPE.OK) {
        let advanceReceivedRecordedAt = DateUtil.ConvertFromDatepicker(advancedComponentRef.instance.MatchingDateCtrl);
        advancedComponentRef.destroy();
        recordedAts.push(advanceReceivedRecordedAt);

        this.endSubProcess("ConfirmMatchingRecordedAt",true,confirmMatchingRecordedAt);

      }
      else {
        advancedComponentRef.destroy();
        this.processResultService.processAtWarning(
          this.processCustomResult, MSG_WNG.CANCEL_PROCESS.replace(MSG_ITEM_NUM.FIRST, "個別消込処理"),
          this.partsResultMessageComponent);

        this.endSubProcess("ConfirmMatchingRecordedAt",false,confirmMatchingRecordedAt);
      }
    });

    confirmMatchingRecordedAt = new Subject();
    return confirmMatchingRecordedAt;
  }


  /// <summary>消込データ登録処理</summary>
  /// <param name="source"></param>
  /// <returns></returns>
  public SaveMatchingAsync(source: MatchingSource): Subject<MatchingResult> {

    let saveMatchingAsync: Subject<MatchingResult>;

    let result: MatchingResult = null;

    let resultInit: boolean;
    Console.log("SaveMatchingAsync InitSave start");
    this.InitSave(source)
      .subscribe(response => {
        Console.log("SaveMatchingAsync InitSave end:" + response);
        resultInit = response;
        if (!resultInit) {

          this.endSubProcessAny("SaveMatchingAsync",null,saveMatchingAsync);
        }

        let resultMatch: boolean;
        Console.log("SaveMatchingAsync MatchingIndividually start");
        this.matchingService.MatchingIndividually(source)
          .subscribe(response => {
            Console.log("SaveMatchingAsync MatchingIndividually end:" + response);
            if (response != PROCESS_RESULT_RESULT_TYPE.FAILURE) {
              resultMatch = true;
              result = response;

              if (resultMatch
                //&&NettingPostProcessor != null
              ) {
                let syncResult = true;
                if (result.nettingReceipts != null && result.nettingReceipts.length > 0) {
                  //syncResult = NettingPostProcessor.Invoke(result.nettingReceipts);          
                }
                resultMatch = resultMatch && syncResult;
                if (!syncResult && result != null) {
                  result.matchingErrorType = MatchingErrorType.PostProcessError;
                }

              }

              if (resultMatch
                //&& this.IsPostProcessorImplemented
              ) {
                var syncResult = true;
                if (result.advanceReceiveds != null && result.advanceReceiveds.length > 0) {
                  let advanceReceiptIds = new Array<number>()
                  result.advanceReceiveds.forEach(element => {
                    advanceReceiptIds.push(element.receiptId);
                  });

                  let advanceReceived: Array<Receipt>;
                  // = await GetNewReceiptsByIds(advanceReceiptIds);
                  if (advanceReceived != null && advanceReceived.length > 0) {
                    //syncResult = AdvanceReceivedPostProcessor.Invoke(advanceReceived.Select(x => x as ITransactionData));
                  }
                  resultMatch = resultMatch && syncResult;
                }
                if (result.matchings != null && result.matchings.length > 0) {
                  //syncResult = MatchingPostProcessor.Invoke(result.Matchings.Select(x => x as ITransactionData));
                }
                resultMatch = resultMatch && syncResult;
                if (!syncResult && result != null) {
                  result.matchingErrorType = MatchingErrorType.PostProcessError;
                }
              }

              this.endSubProcessAny("SaveMatchingAsync",result,saveMatchingAsync);

            }
            else {

              this.endSubProcessAny("SaveMatchingAsync",null,saveMatchingAsync);

            }
          });
      });

    saveMatchingAsync = new Subject();
    return saveMatchingAsync;

  }

  public InitSave(source: MatchingSource): Subject<boolean> {

    let initSave: Subject<boolean> = new Subject();

    let paymentAgencyId: number = this.collationData.paymentAgencyId;
    if (paymentAgencyId == 0) paymentAgencyId = null;
    source.paymentAgencyId = paymentAgencyId;

    let paymentAgencyResponse
    if (paymentAgencyId != null) {
      paymentAgencyResponse = this.paymentAgencyMasterService.GetItemsById(paymentAgencyId);

    }

    let customerId: number =
      this.NewSelectedParentCustomerId > 0 ?
        this.NewSelectedParentCustomerId : this.GridSelectedParentCustomerId;
    if (customerId == 0) customerId = null;
    source.customerId = customerId;

    let customerResponse = null;
    if (customerId != null) {
      customerResponse = this.customerMasterService.GetItemsById(customerId)

    }

    if (paymentAgencyResponse != null) {
      forkJoin(paymentAgencyResponse)
        .subscribe(response => {
          if (response != undefined && response.length == 1) {
            source.useKanaLearning = response[0][0].useKanaLearning;
            source.useFeeLearning = response[0][0].useFeeLearning;
          }
          else {
            source.useKanaLearning = 0;
            source.useFeeLearning = 0;
          }

          this.endSubProcess("initSave",true,initSave);
        });
    }
    else if (customerResponse != null) {
      forkJoin(customerResponse)
        .subscribe(response => {
          if (response != undefined && response.length == 1) {
            source.useKanaLearning = response[0][0].useKanaLearning;
            source.useFeeLearning = response[0][0].useFeeLearning;
          }
          else {
            source.useKanaLearning = 0;
            source.useFeeLearning = 0;
          }
          this.endSubProcess("initSave",true,initSave);
        });
    }
    else {
      this.endSubProcess("initSave",true,initSave);
    }

    initSave = new Subject();
    return initSave;


  }


  /// <summary>請求/入金から、消込の組み合わせを取得する処理</summary>
  /// <param name="source"></param>
  /// <param name="option"></param>
  /// <returns></returns>
  public SolveMatchingPattern(
    option: CollationSearch,
    checkedBillings: Array<Billing>, checkedReceipts: Array<Receipt>,
    recordedsAt: Array<string>,
    groupchildCusId: Array<number>,
    newChildren: Map<number, Customer>,
    groups: Array<CustomerGroup>,
  ): Subject<MatchingSource> {

    let solveMatchingPattern: Subject<MatchingSource> = new Subject();

    let receiptTaxDifference =
      StringUtil.IsNullOrEmpty(this.receiptTaxDifferenceCtrl.value) ?
        0 : this.receiptTaxDifferenceCtrl.value;
    let billingTaxDifference =
      StringUtil.IsNullOrEmpty(this.billingTaxDifferenceCtrl.value) ?
        0 : this.billingTaxDifferenceCtrl.value;
    var taxDiff = billingTaxDifference - receiptTaxDifference;

    let requestSource = new MatchingSource();
    requestSource.billings = checkedBillings;
    requestSource.receipts = checkedReceipts;
    requestSource.bankTransferFee =
      StringUtil.IsNullOrEmpty(this.transferFeeCtrl.value) ?
        0 : this.transferFeeCtrl.value;
    requestSource.taxDifference = taxDiff;

    option.advanceReceivedRecordedAt = recordedsAt.length > 0 ? recordedsAt[0] : "";
    option.doTransferAdvanceReceived = this.cbxAdvanceReceived;
    option.useAdvanceReceived =
      this.collationSetting.useAdvanceReceived == 1
      && this.collationData.parentCustomerId > 0;

    requestSource.option = option;

    let source: MatchingSource = null;
    Console.log("SolveMatchingPattern Solve start");
    this.matchingService.Solve(requestSource)
      .subscribe(response => {
        Console.log("SolveMatchingPattern Solve end");

        if (response != PROCESS_RESULT_RESULT_TYPE.FAILURE) {
          source = response;
          if (source == null || source.matchings == null) {
            this.processResultService.processAtFailure(
              this.processCustomResult, MSG_ERR.MATCHING_ERROR, this.partsResultMessageComponent);

            this.endSubProcessAny("SolveMatchingPattern",null,solveMatchingPattern);
          }
          else {
            source.companyId = this.userInfoService.Company.id;
            source.clientKey = option.clientKey;
            source.matchingProcessType = 1;
            source.loginUserId = this.userInfoService.LoginUser.id;
            source.childCustomerIds = groupchildCusId;

            Console.log("SolveMatchingPattern ConfirmMatchingDoOrNotWithAdvanceReceived start");
            this.ConfirmMatchingDoOrNotWithAdvanceReceived(source)
              .subscribe(response => {
                Console.log("SolveMatchingPattern ConfirmMatchingDoOrNotWithAdvanceReceived end");
                let result: boolean = response;

                if (!result) {
                  this.endSubProcessAny("SolveMatchingPattern(ConfirmMatchingDoOrNotWithAdvanceReceived())",null,solveMatchingPattern);
                }
                else {
                  if (source.remainType == 3) {
                    Console.log("SolveMatchingPattern ConfirmAdvanceReceivedCustomerId start");

                    let groupsCheckSet: Set<number> = new Set();

                    groups.forEach(element => {
                      groupsCheckSet.add(element.childCustomerId);
                      groupsCheckSet.add(element.parentCustomerId);
                    });
                    newChildren.forEach((value, key) => { groupsCheckSet.add(key) });
                    groupsCheckSet.add(this.GridSelectedParentCustomerId);
                    if (groupsCheckSet.size == 1) {
                      source.advanceReceivedCustomerId = Array.from(groupsCheckSet)[0];

                      this.endSubProcessAny("SolveMatchingPattern()",source,solveMatchingPattern);
                    }
                    else {
                      this.ConfirmAdvanceReceivedCustomerId(source, groupsCheckSet)
                      .subscribe(response => {
                        let result: boolean = response;
                        Console.log("SolveMatchingPattern ConfirmAdvanceReceivedCustomerId end:" + result);
                        if (!result) {

                          this.endSubProcessAny("SolveMatchingPattern(ConfirmAdvanceReceivedCustomerId())",null,solveMatchingPattern);
                        }
                        else {
                          this.endSubProcessAny("SolveMatchingPattern(ConfirmAdvanceReceivedCustomerId())",source,solveMatchingPattern);
                        }
                      });
                  }
                  }
                  else {
                    this.endSubProcessAny("SolveMatchingPattern()",source,solveMatchingPattern);
                  }
                }
              });
          }
        }
        else {
          this.processResultService.processAtFailure(
            this.processCustomResult, MSG_ERR.MATCHING_ERROR, this.partsResultMessageComponent);
          this.endSubProcessAny("SolveMatchingPattern()",null,solveMatchingPattern);
        }
      });

    solveMatchingPattern = new Subject();
    return solveMatchingPattern;
  }

  /// <summary>消込の実施/ 前受振替の実施を確認する処理</summary>
  /// <param name="source"></param>
  /// <returns></returns>
  public ConfirmMatchingDoOrNotWithAdvanceReceived(source: MatchingSource): Subject<boolean> {

    let confirmMatchingDoOrNotWithAdvanceReceived: Subject<boolean> = new Subject();

    let remainAmount = 0;
    let carryOverAmount = 0;

    if (source.remainType == 1) {
      remainAmount = this.billingMatchingRemain;
    }
    else if (source.remainType == 2) {
      remainAmount = this.receiptMatchingRemain;
    }
    else if (source.remainType == 3) {
      remainAmount = this.receiptMatchingRemain;
      carryOverAmount = source.matchings[source.matchings.length - 1].receiptRemain;
    }

    let confirmMatchingComponentFactory = this.componentFactoryResolver.resolveComponentFactory(ModalConfirmMatchingComponent);
    let confirmMatchingComponentRef = this.viewContainerRef.createComponent(confirmMatchingComponentFactory);

    confirmMatchingComponentRef.instance.RemainType = source.remainType;
    confirmMatchingComponentRef.instance.RemainAmount = remainAmount;
    confirmMatchingComponentRef.instance.CarryOverAmount = carryOverAmount;
    confirmMatchingComponentRef.instance.CustomerName = this.customerName

    confirmMatchingComponentRef.instance.Closing.subscribe(() => {
      confirmMatchingComponentRef.destroy();
      if (
        confirmMatchingComponentRef.instance.ModalStatus == MODAL_STATUS_TYPE.CANCEL
        || (confirmMatchingComponentRef.instance.ModalStatus == MODAL_STATUS_TYPE.NO && source.remainType != 3)
      ) {
        this.processResultService.processAtWarning(
          this.processCustomResult, MSG_WNG.CANCEL_PROCESS.replace(MSG_ITEM_NUM.FIRST, "個別消込処理"),
          this.partsResultMessageComponent);

        this.endSubProcess("ConfirmMatchingDoOrNotWithAdvanceReceived",false,confirmMatchingDoOrNotWithAdvanceReceived);
      }
      else {
        if (source.remainType == 3 && confirmMatchingComponentRef.instance.ModalStatus == MODAL_STATUS_TYPE.NO) {
          source.remainType = 2;
        }
        source.matchingHeader.memo = confirmMatchingComponentRef.instance.MemoCtrl.value;
        this.endSubProcess("ConfirmMatchingDoOrNotWithAdvanceReceived",true,confirmMatchingDoOrNotWithAdvanceReceived);
      }

    });

    confirmMatchingDoOrNotWithAdvanceReceived = new Subject();
    return confirmMatchingDoOrNotWithAdvanceReceived;
  }

  /// <summary>前受振替時、得意先の選択処理</summary>
  /// <param name="source"></param>
  /// <param name="groups"></param>
  /// <param name="newChildren"></param>
  /// <returns></returns>
  public ConfirmAdvanceReceivedCustomerId(
    source: MatchingSource,
    groupsCheckSet: Set<number>): Subject<boolean> {

      let confirmAdvanceReceivedCustomerId: Subject<boolean> = new Subject();

      let confirmCustomerComponentFactory = this.componentFactoryResolver.resolveComponentFactory(ModalConfirmMatchingAdvancedCustomerComponent);
      let confirmCustomerLedgerComponentRef = this.viewContainerRef.createComponent(confirmCustomerComponentFactory);

      confirmCustomerLedgerComponentRef.instance.customerIds = Array.from(groupsCheckSet);

      confirmCustomerLedgerComponentRef.instance.Closing.subscribe(() => {
        confirmCustomerLedgerComponentRef.destroy();
        if (confirmCustomerLedgerComponentRef.instance.ModalStatus == MODAL_STATUS_TYPE.CANCEL) {
          this.processResultService.processAtWarning(
            this.processCustomResult, MSG_WNG.CANCEL_PROCESS.replace(MSG_ITEM_NUM.FIRST, "個別消込処理"),
            this.partsResultMessageComponent);

          this.endSubProcess("ConfirmAdvanceReceivedCustomerId",false,confirmAdvanceReceivedCustomerId);
       
        }
        else {
          source.advanceReceivedCustomerId = confirmCustomerLedgerComponentRef.instance.customerIdCtrl.value;

          this.endSubProcess("ConfirmAdvanceReceivedCustomerId",true,confirmAdvanceReceivedCustomerId);
        }
      });

    confirmAdvanceReceivedCustomerId = new Subject();
    return confirmAdvanceReceivedCustomerId;
  }



  /// <summary>消込前 検証処理</summary>
  /// <remarks>
  ///  未選択確認
  ///   請求側
  ///    消込対象額 金額検証
  ///    債権代表者 グループ検証
  ///  正負混在の場合、金額不一致は消込不可
  /// </remarks>
  public ValidateForMatching(newChildren: Map<number, Customer>, groups: Array<CustomerGroup>): Subject<boolean> {

    let validateForMatching: Subject<boolean> = new Subject();

    if (!this.ValidateBillingDataForMatching()) {

      this.endSubProcess("ValidateForMatching(ValidateBillingDataForMatching)",false,validateForMatching);

    }
    else {
      let receiptsTmp = this.receipts.filter(function (value, index) {
        if (value.checkBox == CheckBoxStatus.ON) {
          return true;
        }
        else {
          return false;
        }
      });

      if (receiptsTmp.length == 0) {
        this.processResultService.processAtWarning(
          this.processCustomResult, MSG_WNG.NO_SELECT_DATA_FOR_MATCHING.replace(MSG_ITEM_NUM.FIRST, "入金データ"),
          this.partsResultMessageComponent);
        this.endSubProcess("ValidateForMatching(receipts.length==0)",false,validateForMatching);
      }
      else {
        let billingsTmp = this.billings.filter(function (value, index) {
          if (value.checkBox == CheckBoxStatus.ON) {
            return true;
          }
          else {
            return false;
          }
        });

        let isPaymentAgency = this.collationData.paymentAgencyId > 0;

        this.ValidateBillingCustomerGroupForMatching(this.billings, newChildren, groups)
          .subscribe(response => {
            let bRtn: boolean = response;
            if (!isPaymentAgency) {
              this.endSubProcess("ValidateForMatching(ValidateBillingCustomerGroupForMatching)",bRtn,validateForMatching);
            }
            else {

              this.endSubProcess(
                  "ValidateForMatching(ValidateTargetAmountSign)",
                  this.ValidateTargetAmountSign(this.billings, this.receipts),validateForMatching);

            }
          });
      }
    }

    validateForMatching = new Subject();
    return validateForMatching;

  }

  /// <summary>請求側 検証処理</summary>
  /// <returns></returns>
  public ValidateBillingDataForMatching(): boolean {
    var billingCheckedAny = false;
    for (let i = 0; i < this.billings.length; i++) {
      var doMatching = this.billings[i].checkBox == CheckBoxStatus.ON;
      if (!doMatching) continue;
      billingCheckedAny = true;

      var remain = this.billings[i].remainAmount;
      var target = this.billings[i].targetAmount;

      // TODO : discount
      if (target == 0) {
        HtmlUtil.nextFocusByName(this.elementRef, 'detailTargetAmountCtrl' + i, EVENT_TYPE.SELECT);
        this.processResultService.processAtWarning(
          this.processCustomResult, MSG_WNG.NOT_ALLOWD_TARGET_AMOUNT_IS_ZERO,
          this.partsResultMessageComponent);
        return false;
      }

      var arg1 = "";
      if (0 < remain) {
        if (!this.userInfoService.ApplicationControl.useScheduledPayment && remain < target) {
          arg1 = "請求残以下の「消込対象額」";
        }
        if (target < 0) {
          arg1 = (!this.userInfoService.ApplicationControl.useForeignCurrency || this.collationData.currencyCode == "JPY")
            ? "1円以上の「消込対象額」" : "1 以上の「消込対象額」";
        }
      }
      else {
        if (!this.userInfoService.ApplicationControl.useScheduledPayment && target < remain) {
          arg1 = " 請求残以上の「消込対象額」";
        }
        if (0 < target) {
          arg1 = (!this.userInfoService.ApplicationControl.useForeignCurrency || this.collationData.currencyCode == "JPY")
            ? "-1円以下の「消込対象額」" : "-1 以下の「消込対象額」";
        }
      }

      if (!StringUtil.IsNullOrEmpty(arg1)) {
        HtmlUtil.nextFocusByName(this.elementRef, 'detailTargetAmountCtrl' + i, EVENT_TYPE.SELECT);
        this.processResultService.processAtWarning(
          this.processCustomResult, MSG_WNG.INPUT_REQUIRED.replace(MSG_ITEM_NUM.FIRST, arg1),
          this.partsResultMessageComponent);
        return false;
      }
    }
    if (!billingCheckedAny) {
      this.processResultService.processAtWarning(
        this.processCustomResult, MSG_WNG.NO_SELECT_DATA_FOR_MATCHING.replace(MSG_ITEM_NUM.FIRST, "請求データ"),
        this.partsResultMessageComponent);
      return false;
    }
    return true;
  }

  /// <summary>
  /// 選択された請求データと、債権代表者グループの検証処理
  /// 選択された債権代表者(単独得意先) と 他の得意先コードの整合性検証
  /// 下記の場合に検証処理成功とする
  /// 1. 親得意先が選択されている場合 他の得意先が、
  ///   -a 親のグループに所属している
  ///   -b どのグループにも属していない 単独得意先
  /// 2. 子得意先が選択されている場合 他の得意先が、
  ///   -b どのグループにも属していない 単独得意先
  /// 上記 2. の場合は、債権代表者を選択するダイアログを表示し、
  /// 子の得意先を債権代表者へと変更する必用がある
  /// </summary>
  /// <param name="billings">選択された <see cref="Billing"/>一覧</param>
  /// <returns></returns>
  /// <remarks>
  /// 選択された 債権代表者グループ を取得
  /// 選択された 請求データ の 得意先ID が 上記 グループに含まれるか
  /// 含まれていない場合、 得意先が 債権代表者 となってはいないか
  /// 他の債権代表者 グループに含まれてはないか
  /// ダイアログ表示後に確認すべき事項は何か
  /// </remarks>

  public ValidateBillingCustomerGroupForMatching(
    billings: Array<Billing>,
    newChildren: Map<number, Customer>,
    customerGroups: Array<CustomerGroup>): Subject<boolean> {

    let validateCustomerGroup: Subject<boolean> = new Subject();

    let valid = false;
    this.customerGroupService.GetItems(this.GridSelectedParentCustomerId)
      .subscribe(response => {

        if (response != undefined) {
          response.forEach(element => {
            customerGroups.push(element);
          });
        }

        let tmpCustomers = new Map<number, Customer>();
        this.billings.forEach(element => {
          if(element.checkBox==CheckBoxStatus.ON){
            let tmpCustomer = new Customer();
            tmpCustomer.id = element.customerId;
            tmpCustomer.code = element.customerCode;
            tmpCustomer.name = element.customerName
            tmpCustomer.parentCustomerId = element.parentCustomerId
            tmpCustomer.isParent = element.isParent
  
            tmpCustomers.set(element.id, tmpCustomer);
  
          }
        });

        let customCount = 0;
        tmpCustomers.forEach((customer, key) => {
          if (!(this.GridSelectedParentCustomerId == customer.id)) {
            customerGroups.filter(function (value, index) {
              if (value.childCustomerId == customer.id) return true;
              else return false;
            });
            if (!(this.GridSelectedParentCustomerIsParent == 1 && customerGroups.length > 0)) {
              this.customerGroupService.ExistCustomer(customer.id)
                .subscribe(response => {
                  if (response != undefined) {
                    let exitFlag = response;

                    if (customer.isParent == 1 && exitFlag) {
                      this.processResultService.processAtWarning(
                        this.processCustomResult, MSG_WNG.OTHER_CHILD_CUSTOMER.replace(MSG_ITEM_NUM.FIRST, customer.code),
                        this.partsResultMessageComponent);

                      this.endSubProcess("ValidateBillingCustomerGroupForMatching()",false,validateCustomerGroup);

                    }
                    else {
                      newChildren.set(customer.id, customer);
                      customCount++;
                      if (customCount == tmpCustomers.size) {

                        this.endSubProcess("ValidateBillingCustomerGroupForMatching()",true,validateCustomerGroup);

                      }
                    }
                  }
                });
            }
            else {
              customCount++;
              if (customCount == tmpCustomers.size) {
                this.endSubProcess("ValidateBillingCustomerGroupForMatching()",true,validateCustomerGroup);
              }
            }
          }
          else {
            customCount++;
            if (customCount == tmpCustomers.size) {
              this.endSubProcess("ValidateBillingCustomerGroupForMatching()",true,validateCustomerGroup);
            }
          }
        });

      });

    validateCustomerGroup = new Subject();
    return validateCustomerGroup;
  }

  public ValidateTargetAmountSign(billings: Array<Billing>, receipts: Array<Receipt>): boolean {
    let billingAmount = 0;
    let isAllNegativeBilling = true;

    for (let index = 0; index < billings.length; index++) {
      billingAmount += billings[index].targetAmount;
      if (billings[index].targetAmount > 0) isAllNegativeBilling = false;
    }

    let receiptAmount = 0;
    let isAllNegativeReceipt = true;

    for (let index = 0; index < receipts.length; index++) {
      receiptAmount += receipts[index].remainAmount;
      if (receipts[index].remainAmount > 0) isAllNegativeReceipt = false;
    }

    let isAllNegative = isAllNegativeBilling && isAllNegativeReceipt;
    let billTaxDiff = StringUtil.IsNullOrEmpty(this.billingTaxDifferenceCtrl.value)
      ? 0 : this.billingTaxDifferenceCtrl.value;
    let rcptTaxDiff = StringUtil.IsNullOrEmpty(this.receiptTaxDifferenceCtrl.value)
      ? 0 : this.receiptTaxDifferenceCtrl.value;
    let discount = StringUtil.IsNullOrEmpty(this.discountCtrl.value)
      ? 0 : this.discountCtrl.value;
    let bankFee = StringUtil.IsNullOrEmpty(this.transferFeeCtrl.value)
      ? 0 : this.transferFeeCtrl.value;

    let billingTarget = billingAmount + billTaxDiff - discount;
    let receiptTarget = receiptAmount + rcptTaxDiff + bankFee;

    if (isAllNegative && billingTarget < 0 && receiptTarget < 0) return true;

    if (billingTarget != receiptTarget && billingTarget < 0) {
      this.processResultService.processAtWarning(
        this.processCustomResult, MSG_WNG.NOT_ALLOWED_RECEIPT_REMAIN_INCREASE,
        this.partsResultMessageComponent);
      this.modalRouterProgressComponentRef.destroy();
      return false;
    }

    if (billingTarget != receiptTarget && receiptTarget < 0) {
      this.processResultService.processAtWarning(
        this.processCustomResult, MSG_WNG.NOT_ALLOWED_BILLING_REMAIN_INCREASE,
        this.partsResultMessageComponent);
      this.modalRouterProgressComponentRef.destroy();
      return false;
    }

    return true;
  }

  public prevCollation() {
    if (this.matchingService.collationInfo.individualIndexNo > 0) {
      this.matchingService.collationInfo.individualIndexNo -= 1;
    }
    else {
      return;
    }

    this.billingSortItem = null;
    this.receiptSortItem = null;

    this.collationData = this.matchingService.collationInfo.collations[this.matchingService.collationInfo.individualIndexNo];
    this.GridSelectedParentCustomerIsParent = this.collationData.isParent;
    this.GridSelectedParentCustomerCode = this.collationData.customerCode;


    this.LoadCollationData();
  }

  public nextCollation() {
    if (this.matchingService.collationInfo.individualIndexNo < this.matchingService.collationInfo.collations.length) {
      this.matchingService.collationInfo.individualIndexNo += 1;
    }
    else {
      return;
    }

    this.billingSortItem = null;
    this.receiptSortItem = null;

    this.collationData = this.matchingService.collationInfo.collations[this.matchingService.collationInfo.individualIndexNo];
    this.GridSelectedParentCustomerIsParent = this.collationData.isParent;
    this.GridSelectedParentCustomerCode = this.collationData.customerCode;

    this.LoadCollationData();
  }

  public registryReceipt() {
    this.router.navigate(['main/PD0301', { from: ComponentId.PE0102 }]);
  }

  public searchReceipt() {
    this.router.navigate(['main/PD0501', { from: ComponentId.PE0102 }]);
  }

  public editReceipt(index: number) {
    // 表示の場合は入金の更新を不可にする。
    if (this.IsMatched) return;

    //入金データ修正画面の表示
    let receipt = this.receipts[index];

    if (receipt.nettingId > 0) {
      this.processCustomResult = this.processResultService.processAtWarning(
        this.processCustomResult, MSG_WNG.DELETE_CONSTRAINT.replace(MSG_ITEM_NUM.FIRST, '相殺'),
        this.partsResultMessageComponent);
      return;
    }

    let currentReceipt: Receipt = null;

    this.receiptService.Get(receipt.id)
      .subscribe(response => {
        if (response != undefined) {
          currentReceipt = response[0];

          if (currentReceipt.inputType != 2) {
            this.processCustomResult = this.processResultService.processAtWarning(
              this.processCustomResult, MSG_WNG.DELETE_CONSTRAINT.replace(MSG_ITEM_NUM.FIRST, '入力以外'),
              this.partsResultMessageComponent);
            return;
          }

          if (currentReceipt.assignmentFlag != 0) {
            this.processCustomResult = this.processResultService.processAtWarning(
              this.processCustomResult, MSG_WNG.DELETE_CONSTRAINT.replace(MSG_ITEM_NUM.FIRST, '消込済'),
              this.partsResultMessageComponent);
            return;
          }

          if (currentReceipt.excludeFlag == 1) {
            this.processCustomResult = this.processResultService.processAtWarning(
              this.processCustomResult, MSG_WNG.DELETE_CONSTRAINT.replace(MSG_ITEM_NUM.FIRST, '対象外'),
              this.partsResultMessageComponent);
            return;
          }

          if (!StringUtil.IsNullOrEmpty(currentReceipt.outputAt)) {
            this.processCustomResult = this.processResultService.processAtWarning(
              this.processCustomResult, MSG_WNG.DELETE_CONSTRAINT.replace(MSG_ITEM_NUM.FIRST, '仕訳出力済'),
              this.partsResultMessageComponent);
            return;
          }

          if (currentReceipt.originalReceiptId != undefined && currentReceipt.originalReceiptId > 0) {
            this.processCustomResult = this.processResultService.processAtWarning(
              this.processCustomResult, MSG_WNG.DELETE_CONSTRAINT.replace(MSG_ITEM_NUM.FIRST, '前受'),
              this.partsResultMessageComponent);
            return;
          }

          var existAdvanceReceived = false;
          this.receiptService.ExistOriginalReceipt(receipt.id)
            .subscribe(response => {
              if (response != undefined) {
                existAdvanceReceived = response;

                if (existAdvanceReceived) {
                  this.processCustomResult = this.processResultService.processAtWarning(
                    this.processCustomResult, MSG_WNG.DELETE_CONSTRAINT.replace(MSG_ITEM_NUM.FIRST, '前受振替済'),
                    this.partsResultMessageComponent);
                }

                //this.matchingService.collationInfo.editRecipt = this.receipts[index];
                this.router.navigate(['main/PD0301',
                  { id: this.receipts[index].id, from: ComponentId.PE0102 }]);

              }
            });
        }
      });

  }

  public searchBilling() {
    this.router.navigate(['main/PC0301', { from: ComponentId.PE0102 }]);
  }


  //////////////////////////////////////
  public checkInvoicesAll(){
    if(this.cbxInvoiceCheckAllCtrl.value){
      this.selectBillings();
    }
    else{
      this.clearBillings();
    }
  }


  public selectBillings() {
    this.processCustomResult = this.processResultService.clearProcessCustomMsg(this.processCustomResult);

    this.billings.forEach(element => {
      element.checkBox = CheckBoxStatus.ON;
    });

    this.CalculateAmount(false, true);

    if (this.collationData.paymentAgencyId == 0) {
      this.SelectParentCustomer();
    }

  }

  public clearBillings() {
    this.processCustomResult = this.processResultService.clearProcessCustomMsg(this.processCustomResult);

    this.billings.forEach(element => {
      element.checkBox = CheckBoxStatus.OFF;
    });

    this.CalculateAmount(false, true);

    if (this.collationData.paymentAgencyId == 0) {
      this.SelectParentCustomer();
    }
  }

  public clearBilling() {
    this.cbxInvoiceCheckAllCtrl.setValue(true);
    this.LoadBillingsAsync()
      .subscribe(response => {
        if (response != undefined) {
          this.billings = response;
          this.SetBillingData();
          if (this.collationData.paymentAgencyId == 0) {
            this.SelectParentCustomer();
          }
        }
      });
  }

  //////////////////////////////////////
  public checkReceiptsAll(){
    if(this.cbxReceiptCheckAllCtrl.value){
      this.selectReceipts();
    }
    else{
      this.clearReceipts();
    }
  }

  public selectReceipts() {
    this.receipts.forEach(element => {
      element.checkBox = CheckBoxStatus.ON;
    });

    this.CalculateAmount(true, false);
  }

  public clearReceipts() {
    this.receipts.forEach(element => {
      element.checkBox = CheckBoxStatus.OFF;
    });

    this.CalculateAmount(true, false);

  }

  public clearReceipt() {
    this.cbxReceiptCheckAllCtrl.setValue(true);
    this.LoadReceiptsAsync()
      .subscribe(response => {
        if (response != undefined) {
          this.receipts = response;
          this.SetReceiptData();
        }
      });
  }

  public simulation() {
    this.processCustomResult = this.processResultService.clearProcessCustomMsg(this.processCustomResult);

    let receiptTmp = this.receipts.filter(function (receipt) {
      return receipt.checkBox == CheckBoxStatus.ON;
    });

    let remainAmount = 0;
    receiptTmp.forEach(element => { remainAmount += element.remainAmount; });

    if (this.billings.length == 0
      || this.receipts.length == 0
      || remainAmount <= 0) {
      this.processResultService.processAtWarning(
        this.processCustomResult, MSG_WNG.NO_DATA.replace(MSG_ITEM_NUM.FIRST, "該当"), this.partsResultMessageComponent);
      return;
    }

    remainAmount = remainAmount + NumberUtil.ParseInt(this.receiptTaxDifferenceCtrl.value);
    var transferFee = NumberUtil.ParseInt(this.transferFeeCtrl.value);
    remainAmount = remainAmount + transferFee;

    // 確認ダイアログの表示
    let matchingComponentFactory = this.componentFactoryResolver.resolveComponentFactory(ModalConfirmComponent);
    let matchingComponentRef = this.viewContainerRef.createComponent(matchingComponentFactory);
    matchingComponentRef.instance.ActionName = "シミュレート"

    matchingComponentRef.instance.Closing.subscribe(() => {

      if (matchingComponentRef.instance.ModalStatus == MODAL_STATUS_TYPE.OK) {

        let sortItems = new Array<SortItem>();
        sortItems.push(new SortItem("dueAt", SortOrder.Ascending));
        sortItems.push(new SortItem("targetAmount", SortOrder.Ascending));

        this.billings = SortUtil.Sort(sortItems, this.billings);

        var indices: Array<number>;
        this.matchingService.Simulate(this.billings, remainAmount)
          .subscribe(response => {
            if (response != undefined) {
              indices = response;
              if (indices.length == 0) {
                this.processCustomResult
                  = this.processResultService.processAtWarning(
                    this.processCustomResult, MSG_WNG.CANNOT_FOUND_PAIR_PATTERN, this.partsResultMessageComponent);

              }
              else {
                for (var i = 0; i < this.billings.length; i++) {
                  if (indices.indexOf(i) >= 0) {
                    this.billings[i].checkBox = CheckBoxStatus.ON;
                  }
                  else {
                    this.billings[i].checkBox = CheckBoxStatus.OFF;
                  }
                }

                sortItems = new Array<SortItem>();
                sortItems.push(new SortItem("checkBox", SortOrder.Ascending));
                sortItems.push(new SortItem("dueAt", SortOrder.Ascending));
                sortItems.push(new SortItem("targetAmount", SortOrder.Ascending));

                this.billings = SortUtil.Sort(sortItems, this.billings);

                this.CalculateAmount(false, true);

                if (this.collationData.paymentAgencyId == 0) {
                  this.SelectParentCustomer();
                }
                this.processResultService.processAtSuccess(
                  this.processCustomResult, MSG_INF.NOT_FOUND_MATCHING_AMT, this.partsResultMessageComponent);
              }
            }
            else {
              this.processResultService.processAtFailure(
                this.processCustomResult, MSG_ERR.SOMETHING_ERROR.replace(MSG_ITEM_NUM.FIRST, "シミュレーション"),
                this.partsResultMessageComponent);
              return;
            }
          });



      }
      else {
        this.processCustomResult = this.processResultService.processAtWarning(
          this.processCustomResult, MSG_WNG.CANCEL_PROCESS.replace(MSG_ITEM_NUM.FIRST, 'シミュレーション'),
          this.partsResultMessageComponent);

      }
      matchingComponentRef.destroy();
    });


  }


  public changeDisplayType() {

    this.matchingService.collationInfo.displayTypeVertical = !this.matchingService.collationInfo.displayTypeVertical;
    if (this.matchingService.collationInfo.displayTypeVertical) {
      this.displayTypeButton = "左右表示";
    }
    else {
      this.displayTypeButton = "上下表示";
    }


  }

  public setCbxDetailReceipt(index: number) {
    if (this.cbxDetailReceiptCtrls.get(this.receipts[index].id).value) {
      this.receipts[index].checkBox = CheckBoxStatus.ON;
    }
    else {
      this.receipts[index].checkBox = CheckBoxStatus.OFF;
    }

    this.CalculateAmount(true, false);
  }

  public getCbxDetailReceipt(index: number): boolean {

    if (this.receipts[index].checkBox == CheckBoxStatus.ON) {
      return true;
    }
    else {
      return null;
    }
  }

  public setCbxDetailBilling(index: number) {
    if (this.cbxDetailBillingCtrls.get(this.billings[index].id).value) {
      this.billings[index].checkBox = CheckBoxStatus.ON;
    }
    else {
      this.billings[index].checkBox = CheckBoxStatus.OFF;
    }

    this.CalculateAmount(false, true);

    if (this.collationData.paymentAgencyId == 0) {
      this.SelectParentCustomer();
    }
  }

  public getCbxDetailBilling(index: number): boolean {

    if (this.billings[index].checkBox == CheckBoxStatus.ON) {
      return true;
    }
    else {
      return null;
    }
  }

  public SelectParentCustomer() {
    let billingsTmp = this.billings.filter(function (billing) {
      return billing.checkBox == CheckBoxStatus.ON;
    });

    let first: IndividualBilling = null;
    billingsTmp.forEach(element => {
      if (element.isParent == 1 || element.customerCode != element.parentCustomerCode) {
        if (first == null) {
          first = element;
        }
      }
    });

    if (first == null && billingsTmp.length > 0) {
      first = billingsTmp[0];
    }

    if (first == null) {
      this.customerName = "";
      this.GridSelectedParentCustomerId = 0;
      this.GridSelectedParentCustomerCode = "";
      this.GridSelectedParentCustomerIsParent = 0;
    }
    else {
      this.customerName = first.parentCustomerName;
      this.GridSelectedParentCustomerId = first.parentCustomerId;
      this.GridSelectedParentCustomerCode = first.parentCustomerCode;
      this.GridSelectedParentCustomerIsParent =
        (first.isParent == 1 || first.customerCode != first.parentCustomerCode) ? 1 : 0;

    }

  }


  public print() {

    let list = new Array<ExportMatchingIndividualSub>();

    let billingList = this.billings;
    let receiptList = this.receipts;

    if (billingList.length == 0 && receiptList.length == 0) {
      this.processResultService.processAtWarning(
        this.processCustomResult, MSG_WNG.NO_EXPORT_DATA, this.partsResultMessageComponent)
      return;
    }

    let maxCount = billingList.length > receiptList.length ? billingList.length : receiptList.length;

    for (let i = 0; i < maxCount; i++) {
      let b = billingList.length > i ? billingList[i] : null;
      let r = receiptList.length > i ? receiptList[i] : null;

      let tmpExport = new ExportMatchingIndividualSub();
      tmpExport.setData(b, r);
      list.push(tmpExport);
    }


    for (let index = 0; index < list.length; index++) {

      if (index < billingList.length && billingList[index].checkBox == CheckBoxStatus.ON) {
        list[index].billCheck = "レ";
      }
      if (index < receiptList.length && receiptList[index].checkBox == CheckBoxStatus.ON) {
        list[index].recCheck = "レ";
      }
      if (this.userInfoService.ApplicationControl.useForeignCurrency) {
        list[index].currencyCode = this.userInfoService.Currency.code;
      }
    }

    // 印刷処理
    let matchingIndividualReportSource = new MatchingIndividualReportSource();

    matchingIndividualReportSource.companyId = this.userInfoService.Company.id;
    matchingIndividualReportSource.priorReceipt = this.matchingService.collationInfo.collationSetting.billingReceiptDisplayOrder == 1;
    matchingIndividualReportSource.precision = 0;
    matchingIndividualReportSource.billingTaxDiff = this.billingTaxDifferenceCtrl.value;
    matchingIndividualReportSource.receiptTaxDiff = this.receiptTaxDifferenceCtrl.value;
    matchingIndividualReportSource.bankFee = this.transferFeeCtrl.value;
    matchingIndividualReportSource.discountAmount = this.discountCtrl.value;
    matchingIndividualReportSource.items = list;
    matchingIndividualReportSource.billingGridSettings = this.billingGridSettings;
    matchingIndividualReportSource.receiptGridSettings = this.receiptGridSettings;

    let result;
    this.matchingService.GetIndividualReport(matchingIndividualReportSource)
      .subscribe(response => {
        try {
          FileUtil.download([response.body], this.Title + DateUtil.getYYYYMMDD(0), FILE_EXTENSION.PDF);
          result = true;

        } catch (error) {
          console.error(error);
        }
        this.processResultService.processAtOutput(
          this.processCustomResult, result, 1, this.partsResultMessageComponent);
        this.openOptions();
      });
  }


  public getCsvLine(items: any[]): string {
    return FileUtil.escapeCsvFields(items).join(',') + LINE_FEED_CODE;
  }

  public export() {

    let billingList = this.billings;
    let receiptList = this.receipts;
    
    if (billingList.length == 0 && receiptList.length == 0) {
      this.processResultService.processAtWarning(
        this.processCustomResult, MSG_WNG.NO_EXPORT_DATA, this.partsResultMessageComponent)
      return;
    }

    let componentFactory = this.componentFactoryResolver.resolveComponentFactory(ModalRouterProgressComponent);
    let modalRouterProgressComponentRef = this.viewContainerRef.createComponent(componentFactory);
    //modalRouterProgressComponentRef.destroy();    

    const billingSettings = this.billingGridSettings.filter(x => this.filterBilling(x) && x.displayWidth > 0).sort((x, y) => x.displayOrder - y.displayOrder);
    const receiptSettings = this.receiptGridSettings.filter(x => this.filterReceipt(x) && x.displayWidth > 0).sort((x, y) => x.displayOrder - y.displayOrder);
    let csv = '';
    let header1 = [];
    if (this.userInfoService.ApplicationControl.useForeignCurrency == 1) {
      header1.push('');
    }
    header1 = header1.concat(
      receiptSettings.map(x => x.columnName === 'AssignmentFlag' ? '入金情報' : ''),
      billingSettings.map(x => x.columnName === 'AssignmentFlag' ? '請求情報' : ''));
    csv += this.getCsvLine(header1);

    let header2 = [];
    if (this.userInfoService.ApplicationControl.useForeignCurrency == 1) {
      header2.push('通貨コード');
    }
    header2 = header2.concat(
      receiptSettings.map(x => x.columnNameJp),
      billingSettings.map(x => x.columnNameJp));
    csv += this.getCsvLine(header2);

    const maxCount = Math.max(billingList.length, receiptList.length);
    for (let i = 0; i < maxCount; i++) {
      const receipt = receiptList.length < i ? undefined : receiptList[i];
      const billing = billingList.length < i ? undefined : billingList[i];
      let fields = [];
      if (this.userInfoService.ApplicationControl.useForeignCurrency == 1) {
        fields.push(receipt == undefined ? billing.currencyCode : receipt.currencyCode);
      }
      fields = fields.concat(
        receiptSettings.map(x => this.getReceiptValueInner(receipt, x)),
        billingSettings.map(x => this.getBillingValueInner(billing, x)));

      csv += this.getCsvLine(fields);
    }

    const resultDatas = [csv];
    try {
      FileUtil.download(resultDatas, this.Title + DateUtil.getYYYYMMDD(0), FILE_EXTENSION.CSV);
      this.processCustomResult.result = PROCESS_RESULT_RESULT_TYPE.SUCCESS;
      this.processResultService.processAtOutput(
        this.processCustomResult, true, 0, this.partsResultMessageComponent);
    }
    catch (error) {
      this.processCustomResult.result = PROCESS_RESULT_RESULT_TYPE.FAILURE;
      this.processResultService.processAtOutput(
        this.processCustomResult, false, 0, this.partsResultMessageComponent);
    }

    modalRouterProgressComponentRef.destroy();  

  }

  //////////////////////////////////////////////////////////////////
  public setDetailTargetAmount(eventType: string, index: number) {
    HtmlUtil.nextFocusByName(this.elementRef, 'detailTargetAmountCtrl' + (index + 1), eventType);
  }

  public inputDetailTargetAmount(index: number) {

    if (this.billings[index].checkBox == CheckBoxStatus.ON) {
      if (this.detailTargetAmountCtrls.get(this.billings[index].id).value == 0) {
        this.processCustomResult = this.processResultService.processAtWarning(
          this.processCustomResult, MSG_WNG.NOT_ALLOWD_TARGET_AMOUNT_IS_ZERO, this.partsResultMessageComponent);

          this.detailTargetAmountCtrls.get(this.billings[index].id).setValue(this.pipe.transform(this.billings[index].targetAmount));
      }
      this.billings[index].targetAmount = parseInt(this.detailTargetAmountCtrls.get(this.billings[index].id).value);
      this.CalculateAmount(false, true);
    }
  }

  public setCurrencyForDetailTargetAmount(index: number) {
    this.detailTargetAmountCtrls.get(this.billings[index].id).setValue(this.pipe.transform(this.detailTargetAmountCtrls.get(this.billings[index].id).value));
  }

  public initCurrencyForDetailTargetAmount(index: number) {
    // FormControlの中身の確認ログ（不要ならば削除してください。）
    // this.formControllog();

    this.detailTargetAmountCtrls.get(this.billings[index].id).setValue(this.pipe.reverceTransform(this.detailTargetAmountCtrls.get(this.billings[index].id).value));
  }

  public formControllog(){
    console.log("----------------------------------------");
    console.log("detailTargetAmountCtrls");
    this.billings.forEach((element,index)=>{
      console.log("" + index + ":" + this.detailTargetAmountCtrls.get(element.id).value);
    });
    console.log("----------------------------------------");
    console.log("billings");
    this.billings.forEach((element,index)=>{
      console.log("" + index + ":" + element.targetAmount);
    });

    console.log("----------------------------------------");
    console.log("this.myFormGroup.controls['detailTargetAmountCtrl'+index]");
    for(let index=0;index<this.billings.length;index++){
      console.log("" + index + ":" + this.myFormGroup.controls["detailTargetAmountCtrl"+index].value);
    }

  }

  //////////////////////////////////////////////////////////////////
  public setReceiptTaxDifference(eventType: string) {
    HtmlUtil.nextFocusByName(this.elementRef, 'transferFeeCtrl', eventType);
  }

  public inputReceiptTaxDifference() {
    this.CalculateAmount(true, false);
  }

  public setCurrencyForReceiptTaxDifference() {
    this.receiptTaxDifferenceCtrl.setValue(this.pipe.transform(this.receiptTaxDifferenceCtrl.value));
  }

  public initCurrencyForReceiptTaxDifference() {
    this.receiptTaxDifferenceCtrl.setValue(this.pipe.reverceTransform(this.receiptTaxDifferenceCtrl.value));
  }

  //////////////////////////////////////////////////////////////////
  public setTransferFee(eventType: string) {
    HtmlUtil.nextFocusByName(this.elementRef, 'billingTaxDifferenceCtrl', eventType);
  }

  public inputTransferFee() {
    this.CalculateAmount(true, false);
  }

  public setCurrencyForTransferFee() {
    this.transferFeeCtrl.setValue(this.pipe.transform(this.transferFeeCtrl.value));
  }

  public initCurrencyForTransferFee() {
    this.transferFeeCtrl.setValue(this.pipe.reverceTransform(this.transferFeeCtrl.value));
  }

  //////////////////////////////////////////////////////////////////
  public setBillingTaxDifference(eventType: string) {
    HtmlUtil.nextFocusByName(this.elementRef, 'discountCtrl', eventType);
  }

  public inputBillingTaxDifference() {
    this.CalculateAmount(false, true);
  }

  public setCurrencyForBillingTaxDifference() {
    this.billingTaxDifferenceCtrl.setValue(this.pipe.transform(this.billingTaxDifferenceCtrl.value));
  }

  public initCurrencyForBillingTaxDifference() {
    this.billingTaxDifferenceCtrl.setValue(this.pipe.reverceTransform(this.billingTaxDifferenceCtrl.value));
  }

  //////////////////////////////////////////////////////////////////
  public setDiscount(eventType: string) {
    HtmlUtil.nextFocusByName(this.elementRef, 'receiptTaxDifferenceCtrl', eventType);
  }

  public inputDiscount() {
    this.CalculateAmount(false, true);
  }

  public setCurrencyForDiscount() {
    this.billingTaxDifferenceCtrl.setValue(this.pipe.transform(this.billingTaxDifferenceCtrl.value));
  }

  public initCurrencyForDiscount() {
    this.billingTaxDifferenceCtrl.setValue(this.pipe.reverceTransform(this.billingTaxDifferenceCtrl.value));
  }

  public setInitSort() {

    let sortBillingItems = new Array<SortItem>();
    let sortReceiptItems = new Array<SortItem>();
    let sortItem = null;

    let matchingOrder:MatchingOrder = null;

    // case "BillingRemainSign" : return "請求残の正負";
    matchingOrder = MatchingUtil.getMatchingOrder(this.matchingService.collationInfo.matchingBillingOrders,"BillingRemainSign");
    if(matchingOrder.available==1){
      sortItem = new SortItem("BillingRemainSign", matchingOrder.sortOrder);
      sortBillingItems.push(sortItem);
    }
    // case "CashOnDueDatesFlag" : return "期日入金予定フラグ";
    matchingOrder = MatchingUtil.getMatchingOrder(this.matchingService.collationInfo.matchingBillingOrders,"CashOnDueDatesFlag");
    if(matchingOrder.available==1){
      sortItem = new SortItem("CashOnDueDatesFlag", matchingOrder.sortOrder);
      sortBillingItems.push(sortItem);
    }
    // case "DueAt" : return  "入金予定日";
    matchingOrder = MatchingUtil.getMatchingOrder(this.matchingService.collationInfo.matchingBillingOrders,"DueAt");
    if(matchingOrder.available==1){
      sortItem = new SortItem("DueAt", matchingOrder.sortOrder);
      sortBillingItems.push(sortItem);
    }
    // case "CustomerCode" : return "得意先コード";
    matchingOrder = MatchingUtil.getMatchingOrder(this.matchingService.collationInfo.matchingBillingOrders,"CustomerCode");
    if(matchingOrder.available==1){
      sortItem = new SortItem("CustomerCode", matchingOrder.sortOrder);
      sortBillingItems.push(sortItem);
    }
    // case "BilledAt" : return "請求日";
    matchingOrder = MatchingUtil.getMatchingOrder(this.matchingService.collationInfo.matchingBillingOrders,"BilledAt");
    if(matchingOrder.available==1){
      sortItem = new SortItem("BilledAt", matchingOrder.sortOrder);
      sortBillingItems.push(sortItem);
    }
    // case "BillingRemainAmount" : return "請求残（入金予定額）の絶対値";
    matchingOrder = MatchingUtil.getMatchingOrder(this.matchingService.collationInfo.matchingBillingOrders,"BillingRemainAmount");
    if(matchingOrder.available==1){
      sortItem = new SortItem("BillingRemainAmount", matchingOrder.sortOrder);
      sortBillingItems.push(sortItem);
    }
    // case "BillingCategory" : return "請求区分";
    matchingOrder = MatchingUtil.getMatchingOrder(this.matchingService.collationInfo.matchingBillingOrders,"BillingCategory");
    if(matchingOrder.available==1){
      sortItem = new SortItem("BillingCategory", matchingOrder.sortOrder);
      sortBillingItems.push(sortItem);
    }
    SortUtil.Sort(sortBillingItems, this.billings);

    this.matchingService.collationInfo.matchingReceiptOrders

    // case "NettingFlag": return "相殺データ";
    matchingOrder = MatchingUtil.getMatchingOrder(this.matchingService.collationInfo.matchingReceiptOrders,"NettingFlag");
    if(matchingOrder.available==1){
      sortItem = new SortItem("NettingFlag", matchingOrder.sortOrder);
      sortBillingItems.push(sortItem);
    }
    // case "ReceiptRemainSign": return "入金残の正負";
    matchingOrder = MatchingUtil.getMatchingOrder(this.matchingService.collationInfo.matchingReceiptOrders,"ReceiptRemainSign");
    if(matchingOrder.available==1){
      sortItem = new SortItem("ReceiptRemainSign", matchingOrder.sortOrder);
      sortBillingItems.push(sortItem);
    }
    // case "RecordedAt": return "入金日";
    matchingOrder = MatchingUtil.getMatchingOrder(this.matchingService.collationInfo.matchingReceiptOrders,"RecordedAt");
    if(matchingOrder.available==1){
      sortItem = new SortItem("RecordedAt", matchingOrder.sortOrder);
      sortBillingItems.push(sortItem);
    }
    // case "PayerName": return "振込依頼人名";
    matchingOrder = MatchingUtil.getMatchingOrder(this.matchingService.collationInfo.matchingReceiptOrders,"PayerName");
    if(matchingOrder.available==1){
      sortItem = new SortItem("PayerName", matchingOrder.sortOrder);
      sortBillingItems.push(sortItem);
    }
    // case "SourceBankName": return "仕向銀行";
    matchingOrder = MatchingUtil.getMatchingOrder(this.matchingService.collationInfo.matchingReceiptOrders,"SourceBankName");
    if(matchingOrder.available==1){
      sortItem = new SortItem("SourceBankName", matchingOrder.sortOrder);
      sortBillingItems.push(sortItem);
    }
    // case "SourceBranchName": return "仕向支店";
    matchingOrder = MatchingUtil.getMatchingOrder(this.matchingService.collationInfo.matchingReceiptOrders,"SourceBranchName");
    if(matchingOrder.available==1){
      sortItem = new SortItem("SourceBranchName", matchingOrder.sortOrder);
      sortBillingItems.push(sortItem);
    }
    // case "ReceiptRemainAmount": return "入金残の絶対値";
    matchingOrder = MatchingUtil.getMatchingOrder(this.matchingService.collationInfo.matchingReceiptOrders,"ReceiptRemainAmount");
    if(matchingOrder.available==1){
      sortItem = new SortItem("ReceiptRemainAmount", matchingOrder.sortOrder);
      sortBillingItems.push(sortItem);
    }
    // case "ReceiptCategory": return "入金区分";
    matchingOrder = MatchingUtil.getMatchingOrder(this.matchingService.collationInfo.matchingReceiptOrders,"ReceiptCategory");
    if(matchingOrder.available==1){
      sortItem = new SortItem("ReceiptCategory", matchingOrder.sortOrder);
      sortBillingItems.push(sortItem);
    }

    SortUtil.Sort(sortReceiptItems, this.receipts);

  }

  public getAdvanceDate(): string {
    let advanceDate: string = "";
    let sortData: Array<any>;
    let sortItems = new Array<SortItem>();
    let columnName: string = "";

    switch (this.collationSetting.advanceReceivedRecordedDateType) {
      case ADVANCED_RECEIVED_RECORDED_DATA_TYPE[0].id:
        return advanceDate;
      case ADVANCED_RECEIVED_RECORDED_DATA_TYPE[1].id:
        return advanceDate = this.datePipe.transform(Date.now(),ymd);
      case ADVANCED_RECEIVED_RECORDED_DATA_TYPE[2].id:
        columnName = 'billedAt';
        sortData = this.billings.filter(x => x.checkBox == CheckBoxStatus.ON);
        break;
      case ADVANCED_RECEIVED_RECORDED_DATA_TYPE[3].id:
        columnName = 'salesAt';
        sortData = this.billings.filter(x => x.checkBox == CheckBoxStatus.ON);
        break;
      case ADVANCED_RECEIVED_RECORDED_DATA_TYPE[4].id:
        columnName = 'closingAt';
        sortData = this.billings.filter(x => x.checkBox == CheckBoxStatus.ON);
        break;
      case ADVANCED_RECEIVED_RECORDED_DATA_TYPE[5].id:
        columnName = 'dueAt';
        sortData = this.billings.filter(x => x.checkBox == CheckBoxStatus.ON);
        break;
      case ADVANCED_RECEIVED_RECORDED_DATA_TYPE[6].id:
        columnName = 'recordedAt';
        sortData = this.receipts.filter(x => x.checkBox == CheckBoxStatus.ON && x.originalReceiptId != null) ;
        break;
    }

    let sortItem = new SortItem(columnName, SortOrder.Descending);
    sortItems.push(sortItem);
    SortUtil.Sort(sortItems, sortData);

    switch (this.collationSetting.advanceReceivedRecordedDateType) {
      case ADVANCED_RECEIVED_RECORDED_DATA_TYPE[2].id:
        advanceDate = this.datePipe.transform(sortData[0].billedAt, ymd);
        break;
      case ADVANCED_RECEIVED_RECORDED_DATA_TYPE[3].id:
          advanceDate = this.datePipe.transform(sortData[0].salesAt, ymd);
        break;
      case ADVANCED_RECEIVED_RECORDED_DATA_TYPE[4].id:
          advanceDate = this.datePipe.transform(sortData[0].closingAt, ymd);
        break;
      case ADVANCED_RECEIVED_RECORDED_DATA_TYPE[5].id:
          advanceDate = this.datePipe.transform(sortData[0].dueAt, ymd);
        break;
      case ADVANCED_RECEIVED_RECORDED_DATA_TYPE[6].id:
          advanceDate = this.datePipe.transform(sortData[0].recordedAt, ymd);
        break;
    }    
    
    return advanceDate;
  }  

  public setSort(categoryType: CategoryType, columnName: string) {

    let sortItems = new Array<SortItem>();
    let sortItem = null;

    if (columnName == "AssignmentFlag") {
      columnName = "checkBox"
    }

    if (categoryType == CategoryType.Billing) {

      if(this.billingSortItem==null){
        sortItem = new SortItem(columnName, SortOrder.Ascending);
      }
      else if(this.billingSortItem.propertyName.toUpperCase()==columnName.toUpperCase()){
        if(this.billingSortItem.sortOrder==SortOrder.Ascending){
          sortItem = new SortItem(columnName, SortOrder.Descending);
        }
        else{
          sortItem = new SortItem(columnName, SortOrder.Ascending);
        }
      }
      else{
        sortItem = new SortItem(columnName, SortOrder.Ascending);
      }

      this.billingSortItem = sortItem;

      sortItems.push(sortItem);

      SortUtil.Sort(sortItems, this.billings);

    }
    else {


      if(this.receiptSortItem==null){
        sortItem = new SortItem(columnName, SortOrder.Ascending);
      }
      else if(this.receiptSortItem.propertyName.toUpperCase()==columnName.toUpperCase()){
        if(this.receiptSortItem.sortOrder==SortOrder.Ascending){
          sortItem = new SortItem(columnName, SortOrder.Descending);
        }
        else{
          sortItem = new SortItem(columnName, SortOrder.Ascending);
        }
      }
      else{
        sortItem = new SortItem(columnName, SortOrder.Ascending);
      }
      
      this.receiptSortItem = sortItem;

      sortItems.push(sortItem);
      SortUtil.Sort(sortItems, this.receipts);
    }

  }


  public getSortRow(categoryType: CategoryType, columnName: string) {

    if (categoryType == CategoryType.Billing) {

      if(this.billingSortItem==null){
        return "";
      }
      else if(
        this.billingSortItem.propertyName.toUpperCase()==columnName.toUpperCase()
        || (this.billingSortItem.propertyName=="checkBox" && columnName=="AssignmentFlag")
      ){
        if(this.billingSortItem.sortOrder==SortOrder.Ascending){
          return "△";
        }
        else{
          return "▽";
        }
      }
      else{
        return "";
      }
    }
    else {
      if(this.receiptSortItem==null){
        return "";
      }
      else if(
        this.receiptSortItem.propertyName.toUpperCase()==columnName.toUpperCase()
        || (this.receiptSortItem.propertyName=="checkBox" && columnName=="AssignmentFlag")
      ){
        if(this.receiptSortItem.sortOrder==SortOrder.Ascending){
          return "△";
        }
        else{
          return "▽";
        }
      }
      else{
        return "";
      }
      
    }

  }




  public getSourceBank(receipt: Receipt): string {
    if (!StringUtil.IsNullOrEmpty(receipt.sourceBankName) && !StringUtil.IsNullOrEmpty(receipt.sourceBranchName)) {
      return receipt.sourceBankName + '/' + receipt.sourceBranchName;
    }
    return receipt.sourceBankName + receipt.sourceBranchName;
  }

  public getNeetingState(receipt: Receipt): string {
    return receipt.nettingId != 0 ? '*' : '';
  }

  public filterReceipt(setting: GridSetting): boolean {
    if (setting.columnName === 'SectionCode' ||
        setting.columnName === 'SectionName') {
      return this.userInfoService.ApplicationControl.useReceiptSection === 1;
    }
    return true;
  }

  public getReceiptValue(receipt: IndividualReceipt, setting: GridSetting) {
    return this.getReceiptValueInner(receipt, setting, true);
  }

  public currencyFormat = '1.0-0';

  public getReceiptValueInner(receipt: IndividualReceipt, setting: GridSetting, forDisplay: boolean = false): any {
    const defaultValue: any = forDisplay ? undefined : '';
    if (receipt == undefined) {
      return defaultValue;
    }
    switch (setting.columnName) {
      case 'AssignmentFlag':              return forDisplay ? receipt.checkBox : (receipt.checkBox == CheckBoxStatus.ON ? 'レ' : defaultValue);
      case 'PayerName':                   return receipt.payerName;
      case 'PayerNameRaw':                return receipt.payerNameRaw;
      case 'RecordedAt':                  return this.datePipe.transform(receipt.recordedAt, ymd);
      case 'ReceiptCategoryName':         return `${receipt.categoryCode}:${receipt.categoryName}`;
      case 'ReceiptAmount':               return forDisplay ? this.decimalPipe.transform(receipt.receiptAmount    , this.currencyFormat) : receipt.receiptAmount;
      case 'RemainAmount':                return forDisplay ? this.decimalPipe.transform(receipt.remainAmount     , this.currencyFormat) : receipt.remainAmount;
      case 'TargetAmount':                return forDisplay ? this.decimalPipe.transform(receipt.assignmentAmount , this.currencyFormat) : receipt.assignmentAmount;
      case 'NettingState':                return this.getNeetingState(receipt);
      case 'SourceBank':                  return this.getSourceBank(receipt);
      case 'BankCode':                    return receipt.bankCode;
      case 'BankName':                    return receipt.bankName;
      case 'BranchCode':                  return receipt.branchCode;
      case 'BranchName':                  return receipt.branchName;
      case 'AccountTypeName':             return receipt.accountTypeId ? ACCOUNT_TYPE_DICTIONARY[receipt.accountTypeId].val : defaultValue;
      case 'AccountNumber':               return receipt.accountNumber;
      case 'SectionCode':                 return receipt.sectionCode;
      case 'SectionName':                 return receipt.sectionName;
      case 'BranchName':                  return receipt.branchName;
      case 'DueAt':                       return this.datePipe.transform(receipt.dueAt, ymd);
      case 'ExcludeCategoryName':         return receipt.excludeCategoryName;
      case 'VirtualBranchCode':           return receipt.payerCode.substring(0, 3);
      case 'VirtualAccountNumber':        return receipt.payerCode.substring(3);
      case 'CustomerCode':                return receipt.customerCode;
      case 'CustomerName':                return receipt.customerName;
      case 'Note1':                       return receipt.note1;
      case 'Note2':                       return receipt.note2;
      case 'Note3':                       return receipt.note3;
      case 'Note4':                       return receipt.note4;
      case 'Memo':                        return receipt.receiptMemo;
    }
    return defaultValue;
  }

  public filterBilling(setting: GridSetting): boolean {
    if (setting.columnName === 'DiscountAmountSummary') {
      return this.userInfoService.ApplicationControl.useDiscount === 1;
    }
    return true;
  }

  public getBillingValue(billing: IndividualBilling, setting: GridSetting): any {
    return this.getBillingValueInner(billing, setting, true);
  }

  public getBillingValueInner(billing: IndividualBilling, setting: GridSetting, forDisplay: boolean = false): any {
    const defaultValue: any = forDisplay ? undefined : '';
    if (billing == undefined) {
      return defaultValue;
    }
    switch (setting.columnName) {
      case 'AssignmentFlag':              return forDisplay ? billing.checkBox : (billing.checkBox == CheckBoxStatus.ON ? 'レ' : defaultValue);
      case 'CustomerCode':                return billing.customerCode;
      case 'CustomerName':                return billing.customerName;
      case 'BilledAt':                    return this.datePipe.transform(billing.billedAt, ymd);
      case 'SalesAt':                     return this.datePipe.transform(billing.salesAt , ymd);
      case 'DueAt':                       return this.datePipe.transform(billing.dueAt   , ymd);
      case 'BillingAmount':               return forDisplay ? this.decimalPipe.transform( billing.billingAmount   , this.currencyFormat) : billing.billingAmount
      case 'RemainAmount':                return forDisplay ? this.decimalPipe.transform( billing.remainAmount    , this.currencyFormat) : billing.remainAmount
      case 'DiscountAmountSummary':       return forDisplay ? this.decimalPipe.transform( billing.discountAmount  , this.currencyFormat) : billing.discountAmount
      case 'TargetAmount':                return forDisplay ? this.decimalPipe.transform( billing.targetAmount    , this.currencyFormat) : billing.targetAmount
      case 'MatchingAmount':              return forDisplay ? this.decimalPipe.transform( billing.assignmentAmount, this.currencyFormat) : billing.assignmentAmount
      case 'InvoiceCode':                 return billing.invoiceCode;
      case 'BillingCategory':             return `${billing.billingCategoryCode}:${billing.billingCategoryName}`;
      case 'DepartmentName':              return billing.departmentName;
      case 'Note1':                       return billing.note1;
      case 'Note2':                       return billing.note2;
      case 'Note3':                       return billing.note3;
      case 'Note4':                       return billing.note4;
      case 'Memo':                        return billing.memo;
      case 'InputType':                   return BILL_INPUT_TYPE_DICTIONARY[billing.inputType].val;
      case 'ScheduledPaymentKey':         return billing.scheduledPaymentKey;
      case 'Note5':                       return billing.note5;
      case 'Note6':                       return billing.note6;
      case 'Note7':                       return billing.note7;
      case 'Note8':                       return billing.note8;
    }
    return defaultValue;
  }

  public isFreeBillingColumn(setting: GridSetting): boolean {
    return !(
                  setting.columnName === 'AssignmentFlag' 
              ||  setting.columnName === 'TargetAmount'
              ||  setting.columnName === 'CustomerCode'
              ||  setting.columnName === 'CustomerName'
            );
  }

  public isFreeReceiptColumn(setting: GridSetting): boolean {
    return !(setting.columnName === 'AssignmentFlag');
  }

  public setBillingNowrap(){
    event.stopPropagation();
  }

  public setReceiptNowrap(){
    event.stopPropagation();
  }
  
}

class Console {
  public static flag: boolean = true;
  public static log(logData: string) {
    if (Console.flag) {
      console.log(logData);
    }
  }
}
