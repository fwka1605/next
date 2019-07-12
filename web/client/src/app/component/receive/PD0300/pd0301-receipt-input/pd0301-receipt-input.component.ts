import { RacCurrencyPipe } from '../../../../pipe/currency.pipe';
import { ReceiptService } from '../../../../service/receipt.service';
import { Component, OnInit, ElementRef, ComponentFactoryResolver, ViewContainerRef, AfterViewInit, ViewChild } from '@angular/core';
import { Receipt } from 'src/app/model/receipt.model';
import { ReceiptMemo } from 'src/app/model/receipt-memo.model';
import { CategoriesResult } from 'src/app/model/categories-result.model';
import { TABLE_INDEX } from 'src/app/common/const/table-name.const';
import { FormControl, Validators, FormGroup } from '@angular/forms';
import { CategoryMasterService } from 'src/app/service/Master/category-master.service';
import { Router, ActivatedRoute, ParamMap, NavigationEnd } from '@angular/router';
import { BaseComponent } from 'src/app/component/common/base/base-component';
import { FormatterUtil } from 'src/app/common/util/formatter.util';
import { ModalMasterComponent } from 'src/app/component/modal/modal-master/modal-master.component';
import { StringUtil } from 'src/app/common/util/string-util';
import { HtmlUtil } from 'src/app/common/util/html.util';
import { ModalMemoComponent } from 'src/app/component/modal/modal-memo/modal-memo.component';
import { BankBranchMasterService } from 'src/app/service/Master/bank-branch-master.service';
import { NumberUtil } from 'src/app/common/util/number-util';
import { KEY_CODE, EVENT_TYPE, BUTTON_ACTION } from 'src/app/common/const/event.const';
import { ModalMasterBankComponent } from 'src/app/component/modal/modal-master-bank/modal-master-bank.component';
import { COMPONENT_STATUS_TYPE, MODAL_STATUS_TYPE, PROCESS_RESULT_RESULT_TYPE, COMPONENT_DETAIL_STATUS_TYPE } from 'src/app/common/const/status.const';
import { UserInfoService } from 'src/app/service/common/user-info.service';
import { NgbDateParserFormatter, NgbDate, NgbCalendar } from '@ng-bootstrap/ng-bootstrap';
import { NgbDateCustomParserFormatter } from 'src/app/common/util/dateformat-util';
import { CustomerMasterService } from 'src/app/service/Master/customer-master.service';
import { SectionMasterService } from 'src/app/service/Master/section-master.service';
import { CurrencyMasterService } from 'src/app/service/Master/currency-master.service';
import { CategoryType, RECEIPT_INPUT_TYPE_DICTIONARY, CODE_TYPE, FunctionType } from 'src/app/common/const/kbn.const';
import { DateUtil } from 'src/app/common/util/date-util';
import { ReceiptSaveItem } from 'src/app/model/receipt-save-item.model';
import { ReceiptInput } from 'src/app/model/receipt-input.model';
import { ReceiptInputsResult } from 'src/app/model/receipt-inputs-result.model';
import { CollationSettingMasterService } from 'src/app/service/Master/collation-setting-master.service';
import { InputControlMasterService } from 'src/app/service/Master/input-control-master.service';
import { ColumnNameSettingMasterService } from 'src/app/service/Master/column-name-setting-master.service';
import { CollationSettingResult } from 'src/app/model/collation-setting-result.model';
import { SectionsResult } from 'src/app/model/sections-result.model';
import { BankBranchsResult } from 'src/app/model/bank-branchs-result.model';
import { ColumnNameSettingsResult } from 'src/app/model/column-name-settings-result.model';
import { forkJoin } from 'rxjs';
import { CurrenciesResult } from 'src/app/model/currencies-result.model';
import { CustomersResult } from 'src/app/model/customers-result.model';
import { ClosingService } from 'src/app/service/closing.service';
import { ClosingInformationResult } from 'src/app/model/closing-information-result.model';
import { Category } from 'src/app/model/category.model';
import { Customer } from 'src/app/model/customer.model';
import { ModalConfirmComponent } from 'src/app/component/modal/modal-confirm/modal-confirm.component';
import { ReceiptInputType } from 'src/app/common/common-const';
import { ColumnNameSettigUtil } from 'src/app/common/util/column-name-setting-util';
import { ComponentId } from 'src/app/common/const/component-name.const';
import { MatchingService } from 'src/app/service/matching.service';
import { PageUtil } from 'src/app/common/util/page-util';
import { ProcessResultService } from 'src/app/service/common/process-result.service';
import { MSG_WNG, MSG_ERR, MSG_ITEM_NUM, MSG_INF } from 'src/app/common/const/message.const';
import { ModalRouterProgressComponent } from 'src/app/component/modal/modal-router-progress/modal-router-progress.component';
import { MatAutocompleteTrigger } from '@angular/material';
import { CustomerSearch } from 'src/app/model/customer-search.model';
import { ReceiptDisplay } from 'src/app/model/input/receipt-display.model';
import { Section } from 'src/app/model/section.model';
import { CategoryResult } from 'src/app/model/category-result.model';

@Component({
  selector: 'app-pd0301-receipt-input',
  templateUrl: './pd0301-receipt-input.component.html',
  styleUrls: ['./pd0301-receipt-input.component.css'],
  providers: [
    { provide: NgbDateParserFormatter, useClass: NgbDateCustomParserFormatter }
  ]

})

export class Pd0301ReceiptInputComponent extends BaseComponent implements OnInit,AfterViewInit {

  // これをしないとEnum型がテンプレートで見えなかったため
  public TABLE_INDEX: typeof TABLE_INDEX = TABLE_INDEX;
  public PD0301_INPUT: typeof PD0301_INPUT = PD0301_INPUT;
  public FunctionType: typeof FunctionType = FunctionType;


  public paramId: string = "";
  public paramFrom: ComponentId;
  public currentReceipt: Receipt;

  public receiptCount: number = 1;
  public receiptMaxCount:number=5;
  public receipts: Receipt[];
  public receiptMemos: Array<ReceiptMemo>;

  public detailDispReceipts = new Array<ReceiptDisplay>();

  public collationSettingResult: CollationSettingResult;
  public sectionsResult: SectionsResult;
  public currenciesResult: CurrenciesResult;
  public categoriesResult: CategoriesResult;
  public bankBranchsResult: BankBranchsResult;
  public columnNameSettingsResult: ColumnNameSettingsResult;
  public closingInformationResult: ClosingInformationResult;

  public matchingNewInputFlg: boolean = false;

  public componentDetailStatus:COMPONENT_DETAIL_STATUS_TYPE;
  public detailEditLineNo:number;  

  // ヘッダー情報
  public receiptIdCtrl: FormControl;  // 入金ID
  public recordedAtCtrl: FormControl;  // 入金日
  public customerCodeCtrl: FormControl;  // 得意先コード
  public selectedCustomer: Customer;
  public customerNameCtrl: FormControl;
  public sectionCodeCtrl: FormControl;  // 入金部門コード
  public sectionId: number;
  public sectionNameCtrl: FormControl;
  public payerNameCtrl: FormControl;  // 振込依頼人名
  public currencyCodeCtrl: FormControl; // 通貨コード
  public cbxSaveKanaHistoryCtrl: FormControl; // 学習履歴に登録する
  public matchingRecordedAtFromToCtrl: FormControl;  //  消込処理　入金日検索条件From～To

  public visibleSaveKanaHistory:boolean =false ;  // 学習履歴に登録するの表示
  /////////////////////////////////////////////////////
  // 詳細
  /////////////////////////////////////////////////////
  // 期日入力の無フラグ
  public notInputDueFlags: Array<boolean> = new Array<boolean>(this.receiptCount);


  public detailReceiptCategoryIdCtrls: Array<FormControl> = new Array(this.receiptCount);  // 入金区分
  public detailReceiptCategoryCodeCtrls: Array<FormControl> = new Array(this.receiptCount);  // 入金区分
  public detailReceiptCategory: Category;

  public detailNote1Ctrls: Array<FormControl> = new Array(this.receiptCount);  // 備考
  public detailDueAtCtrls: Array<FormControl> = new Array(this.receiptCount);  // 入金期日
  public detailReceiptAmountCtrls: Array<FormControl> = new Array(this.receiptCount);  // 金額
  public detailNoteFlagCtrls: Array<FormControl> = new Array(this.receiptCount);  // メモ有無フラグ

  public detailBillNumberCtrls: Array<FormControl> = new Array(this.receiptCount);  // 手形番号
  public detailBankCodeCtrls: Array<FormControl> = new Array(this.receiptCount);  // 銀行コード
  public detailBranchCodeCtrls: Array<FormControl> = new Array(this.receiptCount);  // 支店コード
  public detailBillBankCtrls: Array<FormControl> = new Array(this.receiptCount);  // 手形券面銀行
  public detailBillDrawAtCtrls: Array<FormControl> = new Array(this.receiptCount);  // 提出日
  public detailBillDrawerCtrls: Array<FormControl> = new Array(this.receiptCount);  // 振出人
  public detailNote2Ctrls: Array<FormControl> = new Array(this.receiptCount);  // 備考２
  public detailNote3Ctrls: Array<FormControl> = new Array(this.receiptCount);  // 受注番号
  public detailNote4Ctrls: Array<FormControl> = new Array(this.receiptCount);  // 入金番号

  // 合計
  public sumReceiptAmountCtrl: FormControl;  // 入金額計
  //個別消込画面呼出用
  public remainAmountCtrl: FormControl;      //  請求残
  public amountDifferenceCtrl: FormControl;  //  差額

  public undefineCtrl: FormControl; // 未定用

  @ViewChild('customerCodeInput', { read: MatAutocompleteTrigger }) customerCodeTrigger: MatAutocompleteTrigger;
  @ViewChild('sectionCodeInput', { read: MatAutocompleteTrigger }) sectionCodeTrigger: MatAutocompleteTrigger;

  @ViewChild('detailReceiptCategoryCodeInput', { read: MatAutocompleteTrigger }) detailReceiptCategoryCodeTrigger: MatAutocompleteTrigger;


  constructor(
    public router: Router,
    public activatedRoute: ActivatedRoute,
    public elementRef: ElementRef,
    public componentFactoryResolver: ComponentFactoryResolver,
    public viewContainerRef: ViewContainerRef,
    public userInfoService: UserInfoService,
    public collationSettingService: CollationSettingMasterService,
    public sectionService: SectionMasterService,
    public currencyService: CurrencyMasterService,
    public categoryService: CategoryMasterService,
    public bankBranchService: BankBranchMasterService,
    public inputControlService: InputControlMasterService,
    public columnNameSettingService: ColumnNameSettingMasterService,
    public customerService: CustomerMasterService,
    public closingService: ClosingService,
    public receiptService: ReceiptService,
    public calendar: NgbCalendar,
    public matchingService: MatchingService,
    public processResultService: ProcessResultService
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


    this.receipts = new Array(this.receiptCount);
    this.receiptMemos = new Array(this.receiptCount);

    for (let i = 0; i < this.receiptCount; i++) {
      this.receipts[i] = new Receipt();
      this.receiptMemos[i] = new ReceiptMemo();
    }

  }


  ngOnInit() {

    let componentFactory = this.componentFactoryResolver.resolveComponentFactory(ModalRouterProgressComponent);
    let modalRouterProgressComponentRef = this.viewContainerRef.createComponent(componentFactory);

    let collationSettingResponse = this.collationSettingService.Get();
    let SectionResponse = this.sectionService.GetItems();
    let currencyResponse = this.currencyService.GetItems();
    let categoryResponse = this.categoryService.GetItems(CategoryType.Receipt);
    let bankBranchResponse = this.bankBranchService.GetItems();
    let closingResponse = this.closingService.GetClosingInformation();
    //this.inputControlService
    let columnNameSettingResponse = this.columnNameSettingService.Get(CategoryType.Receipt);
    this.securityHideShow = true;


    this.setControlInit();
    this.setValidator();
    this.clear();
    this.setFormatter();
    this.setAutoComplete();

    forkJoin(
      collationSettingResponse,
      SectionResponse,
      currencyResponse,
      categoryResponse,
      bankBranchResponse,
      columnNameSettingResponse,
      closingResponse
    )
      .subscribe(
        responseList => {
          if (responseList != undefined && responseList.length == 7) {
            this.collationSettingResult = new CollationSettingResult();
            this.sectionsResult = new SectionsResult();
            this.currenciesResult = new CurrenciesResult();
            this.categoriesResult = new CategoriesResult();
            this.bankBranchsResult = new BankBranchsResult();
            this.columnNameSettingsResult = new ColumnNameSettingsResult();
            this.closingInformationResult = new ClosingInformationResult();

            this.collationSettingResult.collationSetting = responseList[0];
            this.sectionsResult.sections = responseList[1];
            this.currenciesResult.currencies = responseList[2];
            this.categoriesResult.categories = responseList[3];
            this.bankBranchsResult.bankBranches = responseList[4];
            this.columnNameSettingsResult.columnNames = responseList[5];
            this.closingInformationResult.closingInformation = responseList[6];


            this.activatedRoute.paramMap.subscribe((params: ParamMap) => {

              let param = params.get("process");
              if (!StringUtil.IsNullOrEmpty(param) && param == "back") {
                // 入力内容の設定
                this.MyFormGroup = this.receiptService.ReceiptInputFormGroup;
                this.readControlInit();
              }

              // 引数を受け取る
              this.paramId = params.get('id');
              this.paramFrom = parseInt(params.get('from'));

              if (this.paramFrom == ComponentId.PE0102){

                let collationData = this.matchingService.collationInfo.collations[this.matchingService.collationInfo.individualIndexNo];
                if (StringUtil.IsNullOrEmpty(this.paramId)) {

                  this.customerService.GetItemsById(collationData.customerId)
                  .subscribe(response => {
                    if(response!=PROCESS_RESULT_RESULT_TYPE.FAILURE){
                      let customersResult = new CustomersResult();
                      customersResult.customers = response;
                      this.customerCodeCtrl.setValue(customersResult.customers[0].code);  // 得意先コード
                      this.selectedCustomer = customersResult.customers[0];
                      this.customerNameCtrl.setValue(customersResult.customers[0].name);
                    }
                    else{
                      this.customerCodeCtrl.setValue("");  // 得意先コード
                      this.selectedCustomer = null;
                      this.customerNameCtrl.setValue("");
                    }
                  });
                  
                  //消込検索条件の入金日を設定
                  let toDate = DateUtil.getYYYYMMDD(1, this.matchingService.collationInfo.collationSearch.recordedAtTo);
                  let fromDate = StringUtil.IsNullOrEmpty(this.matchingService.collationInfo.collationSearch.recordedAtFrom) ? "" :
                                  DateUtil.getYYYYMMDD(1, this.matchingService.collationInfo.collationSearch.recordedAtFrom) + "～";
                  this.matchingRecordedAtFromToCtrl.setValue(fromDate + toDate);

                  this.payerNameCtrl.setValue(collationData.payerName);
                  this.remainAmountCtrl.setValue(collationData.different);
                  this.amountDifferenceCtrl.setValue(collationData.different);
                  this.sumReceiptAmountCtrl.setValue(0);
                  this.matchingNewInputFlg = true;
                }

                if (collationData.isParent == 0 || (!StringUtil.IsNullOrEmpty(this.paramId))){
                  this.customerCodeCtrl.disable();
                }
              }

              if (StringUtil.IsNullOrEmpty(this.paramId)) {
                this.ComponentStatus = COMPONENT_STATUS_TYPE.CREATE;
                this.currentReceipt = null;
                modalRouterProgressComponentRef.destroy();
              }
              else {
                this.ComponentStatus = COMPONENT_STATUS_TYPE.UPDATE;

                if (!this.userInfoService.isFunctionAvailable(FunctionType.RecoverBilling)) {
                  this.securityHideShow = false;
                }

                // 詰め替えを行う。

                let receiptResponse = this.receiptService.Get(parseInt(this.paramId));
                let receiptMemoResponse = this.receiptService.GetMemo(parseInt(this.paramId));

                forkJoin(
                  receiptResponse,
                  receiptMemoResponse
                )
                  .subscribe(response => {

                    if (response == undefined || response.length != 2) {
                      this.processCustomResult = this.processResultService.processAtWarning(
                        this.processCustomResult, MSG_WNG.NOT_EXIST_SEARCH_DATA, this.partsResultMessageComponent);
                      modalRouterProgressComponentRef.destroy();
                      return;
                    }

                    this.currentReceipt = response[0][0];

                    /////////////////////////////////////////////////////
                    // ヘッダー情報(同期部分)
                    /////////////////////////////////////////////////////
                    this.receiptIdCtrl.setValue(this.currentReceipt.id);  // 入金ID

                    let tmpDate = new Date(this.currentReceipt.recordedAt);  // 入金日
                    this.recordedAtCtrl.setValue(new NgbDate(tmpDate.getFullYear(), tmpDate.getMonth() + 1, tmpDate.getDate()));

                    this.payerNameCtrl.setValue(this.currentReceipt.payerName);  // 振込依頼人名

                    /////////////////////////////////////////////////////
                    this.currencyCodeCtrl.setValue("");
                    this.currenciesResult.currencies.forEach(element => {
                      if (element.id === this.currentReceipt.currencyId) {
                        this.currencyCodeCtrl.setValue(element.code); // 通貨コード
                      }
                    });

                    /////////////////////////////////////////////////////
                    // 詳細
                    /////////////////////////////////////////////////////

                    /////////////////////////////////////////////////////
                    this.categoriesResult.categories.forEach(element => {
                      if (element.id == this.currentReceipt.receiptCategoryId) {
                        this.detailReceiptCategoryIdCtrls[0].setValue(element.id);  // 入金区分
                        this.detailReceiptCategoryCodeCtrls[0].setValue(element.code + ":" + element.name);
                        this.detailReceiptCategory = element;
                        // 期日入力ありの場合は、期日入力無フラグをFalseにする。
                        this.notInputDueFlags[0] = element.useLimitDate == 1 ? true : false;
                        this.setNotInput(this.notInputDueFlags[0], 0);

                      }
                    })

                    this.detailNote1Ctrls[0].setValue(this.currentReceipt.note1);  // 備考

                    // 入金期日
                    if (this.currentReceipt.dueAt == undefined || StringUtil.IsNullOrEmpty(this.currentReceipt.dueAt)) {
                      this.detailDueAtCtrls[0].setValue("");
                    }
                    else {
                      tmpDate = new Date(this.currentReceipt.dueAt);
                      this.detailDueAtCtrls[0].setValue(new NgbDate(tmpDate.getFullYear(), tmpDate.getMonth() + 1, tmpDate.getDate()));
                    }

                    let racCurrencyPipe = new RacCurrencyPipe();
                    this.detailReceiptAmountCtrls[0].setValue(racCurrencyPipe.transform(this.currentReceipt.receiptAmount));  // 金額

                    this.detailNoteFlagCtrls[0].setValue(StringUtil.IsNullOrEmpty(this.currentReceipt.receiptMemo) ? "" : "〇");  // メモ有無フラグ

                    this.detailBillNumberCtrls[0].setValue(this.currentReceipt.billNumber);  // 手形番号

                    this.detailBankCodeCtrls[0].setValue(this.currentReceipt.billBankCode);  // 銀行コード
                    this.detailBranchCodeCtrls[0].setValue(this.currentReceipt.billBranchCode);  // 支店コード
                    this.setDetailBankInfo(0);     // 手形券面銀行

                    // 振出日
                    if (this.currentReceipt.billDrawAt == undefined || StringUtil.IsNullOrEmpty(this.currentReceipt.billDrawAt)) {
                      this.detailBillDrawAtCtrls[0].setValue("");
                    }
                    else {
                      tmpDate = new Date(this.currentReceipt.billDrawAt);
                      this.detailBillDrawAtCtrls[0].setValue(new NgbDate(tmpDate.getFullYear(), tmpDate.getMonth() + 1, tmpDate.getDate()));
                    }

                    this.detailBillDrawerCtrls[0].setValue(this.currentReceipt.billDrawer);  // 振出人

                    this.detailNote2Ctrls[0].setValue(this.currentReceipt.note2);  // 備考２
                    this.detailNote3Ctrls[0].setValue(this.currentReceipt.note3);  // 受注番号
                    this.detailNote4Ctrls[0].setValue(this.currentReceipt.note4);  // 入金番号            

                    if (response[1] != undefined && !StringUtil.IsNullOrEmpty(response[1])) {
                      //this.receiptMemos = new Array<ReceiptMemo>(1);
                      this.receiptMemos[0] = new ReceiptMemo();
                      this.receiptMemos[0].receiptId = parseInt(this.paramId);
                      this.receiptMemos[0].memo = response[1];
                      this.detailNoteFlagCtrls[0].setValue("〇");
                    }
                    else {
                      //this.receiptMemos = new Array<ReceiptMemo>(1);
                      this.detailNoteFlagCtrls[0].setValue("");
                    }

                    // 詳細を無効にする
                    for (let i = 1; i < this.receiptCount; i++) {
                      this.detailReceiptCategoryIdCtrls[i].disable();  // 入金区分
                      this.detailReceiptCategoryCodeCtrls[i].disable();  // 入金区分
                      this.detailNote1Ctrls[i].disable();  // 備考
                      this.detailDueAtCtrls[i].disable();  // 入金期日
                      this.detailReceiptAmountCtrls[i].disable();  // 金額
                      this.detailNoteFlagCtrls[i].disable();  // メモ有無フラグ

                      this.detailBillNumberCtrls[i].disable();  // 手形番号
                      this.detailBankCodeCtrls[i].disable();  // 銀行コード
                      this.detailBranchCodeCtrls[i].disable();  // 支店コード
                      this.detailBillBankCtrls[i].disable();  // 手形券面銀行
                      this.detailBillDrawAtCtrls[i].disable();  // 提出日
                      this.detailBillDrawerCtrls[i].disable();  // 振出人
                      this.detailNote2Ctrls[i].disable();  // 備考２
                      this.detailNote3Ctrls[i].disable();  // 受注番号
                      this.detailNote4Ctrls[i].disable();  // 入金番号

                    }

                    // 学習履歴に登録するの設定
                    if (this.currentReceipt.inputType == ReceiptInputType.EbFile)
                    {
                        this.visibleSaveKanaHistory = true;

                        this.collationSettingResult.collationSetting.learnSpecifiedCustomerKana == 1
                          ? this.cbxSaveKanaHistoryCtrl.setValue(true):this.cbxSaveKanaHistoryCtrl.setValue(null);
                    }
                    else
                    {
                      this.cbxSaveKanaHistoryCtrl.setValue(null);
                      this.visibleSaveKanaHistory = false;
                    }


                    /////////////////////////////////////////////////////
                    // ヘッダー情報(非同期部分)
                    /////////////////////////////////////////////////////
                    let customerRes:any = null;
                    if (this.currentReceipt.customerId != undefined && this.currentReceipt.customerId > 0) {
                      customerRes = this.customerService.GetItemsById(this.currentReceipt.customerId);
                    }

                    /////////////////////////////////////////////////////
                    let sectionRes:any = null;
                    if (this.currentReceipt.sectionId != undefined && this.currentReceipt.sectionId > 0) {
                      sectionRes = this.sectionService.GetItemsById(this.currentReceipt.sectionId)
                    }
                    else {
                      this.sectionCodeCtrl.setValue("");  // 入金部門コード
                      this.sectionNameCtrl.setValue("");
                      this.sectionId = null;
                    }

                    forkJoin(
                      customerRes,
                      sectionRes
                    )
                    .subscribe(
                      responseList => {

                        let customersResult = new CustomersResult();
                        if (this.currentReceipt.customerId != undefined && this.currentReceipt.customerId > 0) {
                          customersResult.customers = <Array<Customer>>responseList[0];
                          this.customerCodeCtrl.setValue(customersResult.customers[0].code);  // 得意先コード
                          this.selectedCustomer = customersResult.customers[0];
                          this.customerNameCtrl.setValue(customersResult.customers[0].name);
                        }

                        if (this.currentReceipt.sectionId != undefined && this.currentReceipt.sectionId > 0) {
                          if (responseList[1] != undefined ) {
                            let sectionsResult = new SectionsResult();
                            sectionsResult.sections = <Array<Section>>responseList[1];
                            this.sectionCodeCtrl.setValue(sectionsResult.sections[0].code);  // 入金部門コード
                            this.sectionId = sectionsResult.sections[0].id;
                            this.sectionNameCtrl.setValue(sectionsResult.sections[0].name);
                          }
                          else {
                            this.sectionCodeCtrl.setValue("");  // 入金部門コード
                            this.sectionNameCtrl.setValue("");
                            this.sectionId = null;
                          }
                        }

                        // Disp部分に追加する
                        this.detailAdd(0,true);
                    });





                    modalRouterProgressComponentRef.destroy();
                  });
              }
            });
          }
          else{
            modalRouterProgressComponentRef.destroy();
          }
        }
      );

  }

  ngAfterViewInit(){
    HtmlUtil.nextFocusByName(this.elementRef, 'recordedAtCtrl', EVENT_TYPE.NONE);

  }

  public readControlInit() {
    this.recordedAtCtrl = <FormControl>this.MyFormGroup.controls['recordedAtCtrl'];     // 入金日
    this.customerCodeCtrl = <FormControl>this.MyFormGroup.controls['customerCodeCtrl']; // 得意先コード
    this.customerNameCtrl = <FormControl>this.MyFormGroup.controls['customerNameCtrl'];
    this.payerNameCtrl = <FormControl>this.MyFormGroup.controls['payerNameCtrl'];
    this.sectionCodeCtrl = <FormControl>this.MyFormGroup.controls['sectionCodeCtrl'];
    this.sectionNameCtrl = <FormControl>this.MyFormGroup.controls['sectionNameCtrl'];
    this.currencyCodeCtrl = <FormControl>this.MyFormGroup.controls['currencyCodeCtrl'];
    this.cbxSaveKanaHistoryCtrl = <FormControl>this.MyFormGroup.controls['cbxSaveKanaHistoryCtrl'];

    this.undefineCtrl = <FormControl>this.MyFormGroup.controls['undefineCtrl'];

    for (let i = 0; i < this.receiptCount; i++) {
      
      this.detailReceiptCategoryIdCtrls[i] = <FormControl>this.MyFormGroup.controls['detailReceiptCategoryIdCtrl'+i];
      this.detailReceiptCategoryCodeCtrls[i] = <FormControl>this.MyFormGroup.controls['detailReceiptCategoryCodeCtrl'+i];
      this.detailNote1Ctrls[i] = <FormControl>this.MyFormGroup.controls['detailNote1Ctrl'+i];
      this.detailDueAtCtrls[i] = <FormControl>this.MyFormGroup.controls['detailDueAtCtrl'+i];
      this.detailReceiptAmountCtrls[i] = <FormControl>this.MyFormGroup.controls['detailReceiptAmountCtrl'+i];
      this.detailNoteFlagCtrls[i] = <FormControl>this.MyFormGroup.controls['detailNoteFlagCtrl'+i];
      this.detailBillNumberCtrls[i] = <FormControl>this.MyFormGroup.controls['detailBillNumberCtrl'+i];
      this.detailBankCodeCtrls[i] = <FormControl>this.MyFormGroup.controls['detailBankCodeCtrl'+i];
      this.detailBranchCodeCtrls[i] = <FormControl>this.MyFormGroup.controls['detailBranchCodeCtrl'+i];
      this.detailBillBankCtrls[i] = <FormControl>this.MyFormGroup.controls['detailBillBankCtrl'+i];
      this.detailBillDrawAtCtrls[i] = <FormControl>this.MyFormGroup.controls['detailBillDrawAtCtrl'+i];
      this.detailBillDrawerCtrls[i] = <FormControl>this.MyFormGroup.controls['detailBillDrawerCtrl'+i];
      this.detailNote2Ctrls[i] = <FormControl>this.MyFormGroup.controls['detailNote2Ctrl'+i];
      this.detailNote3Ctrls[i] = <FormControl>this.MyFormGroup.controls['detailNote3Ctrl'+i];
      this.detailNote4Ctrls[i] = <FormControl>this.MyFormGroup.controls['detailNote4Ctrl'+i];
    }

    this.sumReceiptAmountCtrl = <FormControl>this.MyFormGroup.controls['sumReceiptAmountCtrl'];

  }

  public setControlInit() {

    this.receiptIdCtrl = new FormControl("");
    this.recordedAtCtrl = new FormControl("", [Validators.required, Validators.maxLength(10)]);
    this.customerCodeCtrl = new FormControl("", [Validators.required, Validators.maxLength(this.userInfoService.ApplicationControl.customerCodeLength)]);
    this.customerNameCtrl = new FormControl("");
    this.payerNameCtrl = new FormControl("");

    if(this.userInfoService.ApplicationControl.useReceiptSection==1){
      this.sectionCodeCtrl = new FormControl("", [Validators.required,Validators.maxLength(10)]);
    }
    else{
      this.sectionCodeCtrl = new FormControl("", [Validators.maxLength(10)]);
    }


    this.sectionNameCtrl = new FormControl("");
    this.currencyCodeCtrl = new FormControl("", [Validators.maxLength(3)]);
    this.cbxSaveKanaHistoryCtrl = new FormControl("");
    this.matchingRecordedAtFromToCtrl = new FormControl("");

    this.undefineCtrl = new FormControl("");

    for (let i = 0; i < this.receiptCount; i++) {

      this.notInputDueFlags[i] = false;

      this.detailReceiptCategoryIdCtrls[i] = new FormControl("");
      this.detailReceiptCategoryCodeCtrls[i] = new FormControl("");
      this.detailNote1Ctrls[i] = new FormControl("", [Validators.maxLength(100)]);
      this.detailDueAtCtrls[i] = new FormControl(null, [Validators.maxLength(10)]);
      this.detailReceiptAmountCtrls[i] = new FormControl("", [Validators.maxLength(16)]);
      this.detailNoteFlagCtrls[i] = new FormControl("");
      this.detailBillNumberCtrls[i] = new FormControl("", [Validators.maxLength(20)]);
      this.detailBankCodeCtrls[i] = new FormControl("", [Validators.maxLength(4)]);
      this.detailBranchCodeCtrls[i] = new FormControl("", [Validators.maxLength(3)]);
      this.detailBillBankCtrls[i] = new FormControl("");
      this.detailBillDrawAtCtrls[i] = new FormControl(null, [Validators.maxLength(10)]);
      this.detailBillDrawerCtrls[i] = new FormControl("", [Validators.maxLength(48)]);
      this.detailNote2Ctrls[i] = new FormControl("", [Validators.maxLength(100)]);
      this.detailNote3Ctrls[i] = new FormControl("", [Validators.maxLength(100)]);
      this.detailNote4Ctrls[i] = new FormControl("", [Validators.maxLength(100)]);

      this.setNotInput(false, i);
    }
    this.sumReceiptAmountCtrl = new FormControl("");
    this.remainAmountCtrl = new FormControl("");
    this.amountDifferenceCtrl = new FormControl("");
  }

  public setValidator() {
    this.MyFormGroup = new FormGroup({


      receiptIdCtrl: this.receiptIdCtrl,
      recordedAtCtrl: this.recordedAtCtrl,
      customerCodeCtrl: this.customerCodeCtrl,
      customerNameCtrl: this.customerNameCtrl,
      payerNameCtrl: this.payerNameCtrl,
      sectionCodeCtrl: this.sectionCodeCtrl,
      sectionNameCtrl: this.sectionNameCtrl,
      currencyCodeCtrl: this.currencyCodeCtrl,
      cbxSaveKanaHistoryCtrl: this.cbxSaveKanaHistoryCtrl,
      matchingRecordedAtFromToCtrl: this.matchingRecordedAtFromToCtrl,

      sumReceiptAmountCtrl: this.sumReceiptAmountCtrl,
      remainAmountCtrl: this.remainAmountCtrl,
      amountDifferenceCtrl: this.amountDifferenceCtrl,

      UndefineCtrl: this.undefineCtrl,

    });

    for (let i = 0; i < this.receiptCount; i++) {

      this.MyFormGroup.addControl("detailReceiptCategoryIdCtrl" + i, this.detailReceiptCategoryIdCtrls[i]);
      this.MyFormGroup.addControl("detailReceiptCategoryCodeCtrl" + i, this.detailReceiptCategoryCodeCtrls[i]);
      this.MyFormGroup.addControl("detailNote1Ctrl" + i, this.detailNote1Ctrls[i]);
      this.MyFormGroup.addControl("detailDueAtCtrl" + i, this.detailDueAtCtrls[i]);
      this.MyFormGroup.addControl("detailReceiptAmountCtrl" + i, this.detailReceiptAmountCtrls[i]);
      this.MyFormGroup.addControl("detailNoteFlagCtrl" + i, this.detailNoteFlagCtrls[i]);

      this.MyFormGroup.addControl("detailBillNumberCtrl" + i, this.detailBillNumberCtrls[i]);
      this.MyFormGroup.addControl("detailBankCodeCtrl" + i, this.detailBankCodeCtrls[i]);
      this.MyFormGroup.addControl("detailBranchCodeCtrl" + i, this.detailBranchCodeCtrls[i]);
      this.MyFormGroup.addControl("detailBillBankCtrl" + i, this.detailBillBankCtrls[i]);
      this.MyFormGroup.addControl("detailBillDrawAtCtrl" + i, this.detailBillDrawAtCtrls[i]);
      this.MyFormGroup.addControl("detailBillDrawerCtrl" + i, this.detailBillDrawerCtrls[i]);
      this.MyFormGroup.addControl("detailNote2Ctrl" + i, this.detailNote2Ctrls[i]);
      this.MyFormGroup.addControl("detailNote3Ctrl" + i, this.detailNote3Ctrls[i]);
      this.MyFormGroup.addControl("detailNote4Ctrl" + i, this.detailNote4Ctrls[i]);
    }

  }

  public setFormatter() {

    FormatterUtil.setAlphabetFormatter(this.currencyCodeCtrl);

    if (this.userInfoService.ApplicationControl.customerCodeType == CODE_TYPE.NUMBER) {
      FormatterUtil.setNumberFormatter(this.customerCodeCtrl);
    }
    else if (this.userInfoService.ApplicationControl.customerCodeType == CODE_TYPE.HANKANA) {
      FormatterUtil.setCustomerCodeKanaFormatter(this.customerCodeCtrl);
    }
    else {
      FormatterUtil.setCustomerCodeAlphaFormatter(this.customerCodeCtrl);
    }

    if (this.userInfoService.ApplicationControl.sectionCodeType == CODE_TYPE.NUMBER) {
      FormatterUtil.setNumberFormatter(this.sectionCodeCtrl);
    }
    else {
      FormatterUtil.setCodeFormatter(this.sectionCodeCtrl);
    }

    for (let index = 0; index < this.receiptCount; index++) {

      FormatterUtil.setCurrencyFormatter(this.detailReceiptAmountCtrls[index]);

      FormatterUtil.setCodeFormatter(this.detailBillNumberCtrls[index]);
      FormatterUtil.setNumberFormatter(this.detailBankCodeCtrls[index]);
      FormatterUtil.setNumberFormatter(this.detailBranchCodeCtrls[index]);

    }

  }


  public setAutoComplete(){
    // 得意先
    this.initAutocompleteCustomers(this.customerCodeCtrl,this.customerService,0);

    //入金部門
    this.initAutocompleteSections(this.sectionCodeCtrl,this.sectionService,0);

    // 入金区分
    this.initAutocompleteCategories(CategoryType.Receipt,this.detailReceiptCategoryCodeCtrls[0],this.categoryService,0);

  }    
  public setNotInput(flag: boolean, index: number) {

    if (flag) {
      this.detailDueAtCtrls[index].enable();

      this.detailBillNumberCtrls[index].enable();
      this.detailBankCodeCtrls[index].enable();
      this.detailBranchCodeCtrls[index].enable();
      this.detailBillBankCtrls[index].enable();
      this.detailBillDrawAtCtrls[index].enable();
      this.detailBillDrawerCtrls[index].enable();
    }
    else {
      this.detailDueAtCtrls[index].disable();
      this.detailBillNumberCtrls[index].disable();
      this.detailBankCodeCtrls[index].disable();
      this.detailBranchCodeCtrls[index].disable();
      this.detailBillBankCtrls[index].disable();
      this.detailBillDrawAtCtrls[index].disable();
      this.detailBillDrawerCtrls[index].disable();

      this.detailDueAtCtrls[index].setValue(null);
      this.detailBillNumberCtrls[index].setValue("");
      this.detailBankCodeCtrls[index].setValue("");
      this.detailBranchCodeCtrls[index].setValue("");
      this.detailBillBankCtrls[index].setValue("");
      this.detailBillDrawAtCtrls[index].setValue(null);
      this.detailBillDrawerCtrls[index].setValue("");

    }

  }


  public getColumnAlias(columnName: string): string {
    if (this.columnNameSettingsResult != null) {
      return ColumnNameSettigUtil.getColumnAlias(this.columnNameSettingsResult.columnNames, columnName);
    }
    return "";
  }

  public openBankMasterModal(index: number) {
    
    this.customerCodeTrigger.closePanel();
    this.sectionCodeTrigger.closePanel();


    let componentFactory = this.componentFactoryResolver.resolveComponentFactory(ModalMasterBankComponent);
    let componentRef = this.viewContainerRef.createComponent(componentFactory);

    componentRef.instance.Closing.subscribe(() => {
      if (componentRef.instance.ModalStatus == MODAL_STATUS_TYPE.SELECT) {
        this.detailBankCodeCtrls[index].setValue(componentRef.instance.SelectedBankCode);
        this.detailBranchCodeCtrls[index].setValue(componentRef.instance.SelectedBranchCode);
        this.detailBillBankCtrls[index].setValue(componentRef.instance.SelectedBankName + " " + componentRef.instance.SelectedBranchName);
      }

      componentRef.destroy();
    });
  }

  public openMasterModal(table: TABLE_INDEX, index: number = -1) {


    this.customerCodeTrigger.closePanel();
    this.sectionCodeTrigger.closePanel();

    let componentFactory = this.componentFactoryResolver.resolveComponentFactory(ModalMasterComponent);
    let componentRef = this.viewContainerRef.createComponent(componentFactory);
    componentRef.instance.TableIndex = table;

    componentRef.instance.Closing.subscribe(() => {

      if (componentRef.instance.ModalStatus == MODAL_STATUS_TYPE.SELECT) {
        switch (table) {

          case TABLE_INDEX.MASTER_CUSTOMER:
            {
              this.customerCodeCtrl.setValue(componentRef.instance.SelectedCode);
              this.customerNameCtrl.setValue(componentRef.instance.SelectedName);
              this.payerNameCtrl.setValue(componentRef.instance.SelectedObject.kana);

              break;
            }
          case TABLE_INDEX.MASTER_SECTION:
            {
              this.sectionCodeCtrl.setValue(componentRef.instance.SelectedCode);
              this.sectionNameCtrl.setValue(componentRef.instance.SelectedName);
              this.sectionId = componentRef.instance.SelectedId;
              break;
            }
          case TABLE_INDEX.MASTER_CURRENCY:
            {
              this.currencyCodeCtrl.setValue(componentRef.instance.SelectedCode);
              break;
            }
          case TABLE_INDEX.MASTER_RECEIPT_CATEGORY:
            {
              this.detailReceiptCategoryCodeCtrls[index].setValue(
                componentRef.instance.SelectedCode + ":" + componentRef.instance.SelectedName);

              this.detailReceiptCategoryIdCtrls[index].setValue(
                componentRef.instance.SelectedObject.Id);

              this.detailReceiptCategory = componentRef.instance.SelectedObject;

              this.notInputDueFlags[index] = componentRef.instance.SelectedObject.useLimitDate == 1 ? true : false;

              HtmlUtil.nextFocusByName(this.elementRef, 'detailNote1Ctrl' + index, EVENT_TYPE.SELECT);
              this.setNotInput(this.notInputDueFlags[index], index);

              break;
            }

        }
      }


      componentRef.destroy();
    });
  }

  public back() {
    if (this.paramFrom == ComponentId.PD0501) {
      this.router.navigate(['main/PD0501', { "process": "back" }]);
    }
    else if (this.paramFrom == ComponentId.PE0102) {
      this.router.navigate(['main/PE0102', { "process": "back" }]);
    }
  }

  public clear() {
    this.MyFormGroup.reset();


    for (let i = 0; i < this.receiptCount; i++) {
      this.receipts[i];
      this.receiptMemos[i].memo = "";

      // 期日入力を初期化
      this.notInputDueFlags[i] = false;
      this.setNotInput(this.notInputDueFlags[i], i);

    }

    this.selectedCustomer = null;
    this.sectionId = null;
    this.recordedAtCtrl.setValue(this.calendar.getToday());

    this.processCustomResult = this.processResultService.clearProcessCustomMsg(this.processCustomResult);;

    this.ComponentStatus = this.COMPONENT_STATUS_TYPE.CREATE;

    this.componentDetailStatus=COMPONENT_DETAIL_STATUS_TYPE.CREATE;
    this.detailEditLineNo=-1;
    
    HtmlUtil.nextFocusByName(this.elementRef, 'recordedAtCtrl', EVENT_TYPE.NONE);


  }


  public validateInput(): boolean {
    let bRtn: boolean = false;

    // 入力チェック
    // 入金日の必須チェック(不要)
    // 締め日の確認

    // 一括(個別)消込画面から遷移のみチェック
    // 一括(個別)消込画面で指定した「入金日」の範囲外の場合

    // 通貨コード(不要)

    // 得意先コード(不要)

    // 入金部門コード

    bRtn = true;
    return true;
  }

  public setGridInput(method:string,clearFlag:boolean=false): boolean {
    let bRtn: boolean = false;

    let allEmpty = false;
    for (let index = 0; index < this.receiptCount; index++) {
      if (this.validateGridEmptyByRow(index)) {
        allEmpty = true;
      }
      else {
        allEmpty = false;
        break;
      }

      if (allEmpty) {
        this.processCustomResult = this.processResultService.processAtWarning(
          this.processCustomResult, MSG_WNG.INPUT_GRID_REQUIRED, this.partsResultMessageComponent);
          HtmlUtil.nextFocusByName(this.elementRef, 'detailReceiptCategoryCodeCtrl0' + index, EVENT_TYPE.NONE);
        return false;
      }
    }

    // 入金区分の存在チェックをする
    let categoryCode = this.detailReceiptCategoryCodeCtrls[0].value;
    categoryCode = categoryCode.split(":")[0];
    let categoryResponse = this.categoryService.GetItems(CategoryType.Receipt,categoryCode);

    forkJoin(
      categoryResponse
    )
    .subscribe(responseList=>{
      // 入金区分の存在チェック
      if(responseList[0]== PROCESS_RESULT_RESULT_TYPE.FAILURE || responseList[0].length!=1){
        // 入金区分エラー
        this.processCustomResult = this.processResultService.processAtWarning(
          this.processCustomResult, MSG_WNG.INPUT_NOT_EXIST.replace(MSG_ITEM_NUM.FIRST, '入金区分'),
          this.partsResultMessageComponent);
          HtmlUtil.nextFocusByName(this.elementRef, 'detailReceiptCategoryCodeCtrl0', EVENT_TYPE.NONE);
        return false;
      }
      else{
        for (let index = 0; index < this.receiptCount; index++) {
          if (!this.validateGridEmptyByRow(index)) {
            if (StringUtil.IsNullOrEmpty(this.detailReceiptCategoryCodeCtrls[index].value)) {
              this.processCustomResult = this.processResultService.processAtWarning(
                this.processCustomResult, MSG_WNG.INPUT_REQUIRED.replace(MSG_ITEM_NUM.FIRST, '入金区分'),
                this.partsResultMessageComponent);
                HtmlUtil.nextFocusByName(this.elementRef, 'detailReceiptCategoryCodeCtrl' + index, EVENT_TYPE.NONE);
              return false;
            }
    
            // ファクタリング
    
            // 期日
            if (this.detailReceiptCategory.useLimitDate == 1
              && StringUtil.IsNullOrEmpty(this.detailDueAtCtrls[index].value)) {
              this.processCustomResult = this.processResultService.processAtWarning(
                this.processCustomResult, MSG_WNG.INPUT_REQUIRED.replace(MSG_ITEM_NUM.FIRST, '期日'),
                this.partsResultMessageComponent);
                HtmlUtil.nextFocusByName(this.elementRef, 'detailDueAtCtrl' + index, EVENT_TYPE.NONE);
              return false;
            }
    
            // 金額
            if (StringUtil.IsNullOrEmpty(this.detailReceiptAmountCtrls[index].value)) {
              this.processCustomResult = this.processResultService.processAtWarning(
                this.processCustomResult, MSG_WNG.INPUT_REQUIRED.replace(MSG_ITEM_NUM.FIRST, '金額'),
                this.partsResultMessageComponent);
                HtmlUtil.nextFocusByName(this.elementRef, 'detailReceiptAmountCtrl' + index, EVENT_TYPE.NONE);
              return false;
            }
    
            if (parseInt(this.detailReceiptAmountCtrls[index].value) == 0) {
              this.processCustomResult = this.processResultService.processAtWarning(
                this.processCustomResult, MSG_WNG.INPUT_EXCEPT_ZERO_AMT.replace(MSG_ITEM_NUM.FIRST, '金額'),
                this.partsResultMessageComponent);
                HtmlUtil.nextFocusByName(this.elementRef, 'detailReceiptAmountCtrl' + index, EVENT_TYPE.NONE);
              return false;
    
            }
    
            //期日入力フラグがONの場合のみチェックする
            let msg = null;
            if (this.notInputDueFlags[index] == true) {
              // 銀行コード・支店コードの存在チェックは入力時に実行
              let tmpDetailDueAt = new NgbDate(this.detailDueAtCtrls[index].value.year, this.detailDueAtCtrls[index].value.month, this.detailDueAtCtrls[index].value.day)
              // 入金日と期日の関係
              if (
                !StringUtil.IsNullOrEmpty(this.detailDueAtCtrls[index].value)
                && (tmpDetailDueAt.before(this.recordedAtCtrl.value))
              ) {
                msg = MSG_WNG.INPUT_PARAM1_DATE_AFTER_PARAM2_DATE.replace(MSG_ITEM_NUM.FIRST, '期日')
                                                                .replace(MSG_ITEM_NUM.SECOND, '入金日');
                this.processCustomResult = this.processResultService.processAtWarning(
                  this.processCustomResult, msg, this.partsResultMessageComponent);
                HtmlUtil.nextFocusByName(this.elementRef, 'detailDueAtCtrl' + index, EVENT_TYPE.NONE);
                return false;
              }
    
              if (!StringUtil.IsNullOrEmpty(this.detailBillDrawAtCtrls[index].value)) {
                let tmpBillDrawAt = new NgbDate(this.detailBillDrawAtCtrls[index].value.year, this.detailBillDrawAtCtrls[index].value.month, this.detailBillDrawAtCtrls[index].value.day)
                if (tmpBillDrawAt.after(this.recordedAtCtrl.value)) {
                  msg = MSG_WNG.INPUT_PARAM1_DATE_BEFORE_PARAM2_DATE.replace(MSG_ITEM_NUM.FIRST, '振出日')
                    .replace(MSG_ITEM_NUM.SECOND, '入金日');
                  this.processCustomResult = this.processResultService.processAtWarning(
                    this.processCustomResult, msg, this.partsResultMessageComponent);
                  HtmlUtil.nextFocusByName(this.elementRef, 'detailBillDrawAtCtrl' + index, EVENT_TYPE.NONE);
                  return false;
                }
              }
            }
          }
        }
        
        // 表示用の請求データの作成
        let receiptsDisplay = this.getReceiptDisplayDataForAdd();

        // 明細行の内容を設定する
        // 新規モードは配列に追加、更新モードは配列の中身を置き換える。
        if(method=="detailAdd"){
          this.detailDispReceipts.push(receiptsDisplay[0]);
        }
        else{
          if(this.componentDetailStatus==COMPONENT_DETAIL_STATUS_TYPE.UPDATE && this.detailEditLineNo>=0){
            this.detailDispReceipts[this.detailEditLineNo] = receiptsDisplay[0];
          }
        }

        this.inputDetailReceiptAmount();

        if(clearFlag){
          this.detailClear(0);   
        }

      }
    });



    return true;
  }

  public validateGridEmptyByRow(index: number): boolean {
    if (!StringUtil.IsNullOrEmpty(this.detailReceiptCategoryIdCtrls[index].value)) { return false; }  // 入金区分
    if (!StringUtil.IsNullOrEmpty(this.detailNote1Ctrls[index].value)) { return false; }  // 備考
    if (!StringUtil.IsNullOrEmpty(this.detailDueAtCtrls[index].value)) { return false; }  // 入金期日
    if (!StringUtil.IsNullOrEmpty(this.detailReceiptAmountCtrls[index].value)) { return false; }  // 金額
    if (!StringUtil.IsNullOrEmpty(this.detailNoteFlagCtrls[index].value)) { return false; }  // メモ有無フラグ

    if (!StringUtil.IsNullOrEmpty(this.detailBillNumberCtrls[index].value)) { return false; }  // 手形番号
    if (!StringUtil.IsNullOrEmpty(this.detailBankCodeCtrls[index].value)) { return false; }  // 銀行コード
    if (!StringUtil.IsNullOrEmpty(this.detailBranchCodeCtrls[index].value)) { return false; }  // 支店コード
    if (!StringUtil.IsNullOrEmpty(this.detailBillBankCtrls[index].value)) { return false; } // 手形券面銀行
    if (!StringUtil.IsNullOrEmpty(this.detailBillDrawAtCtrls[index].value)) { return false; }  // 提出日
    if (!StringUtil.IsNullOrEmpty(this.detailBillDrawerCtrls[index].value)) { return false; }  // 振出人
    if (!StringUtil.IsNullOrEmpty(this.detailNote2Ctrls[index].value)) { return false; }  // 備考２
    if (!StringUtil.IsNullOrEmpty(this.detailNote3Ctrls[index].value)) { return false; }  // 受注番号
    if (!StringUtil.IsNullOrEmpty(this.detailNote4Ctrls[index].value)) { return false; }  // 入金番号


    return true;
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

  public registry() {


    // 入力チェック
    if (this.ComponentStatus == COMPONENT_STATUS_TYPE.CREATE || this.ComponentStatus == COMPONENT_STATUS_TYPE.UPDATE) {
      if (!this.validateInput() ) {
        return;
      }
    }

    // 登録・更新データの設定
    let receiptSaveItem = new ReceiptSaveItem();
    receiptSaveItem.receipts = this.getReceiptInputDataForRegistry();

    // 動作中のコンポーネントを開く
    let componentFactory = this.componentFactoryResolver.resolveComponentFactory(ModalRouterProgressComponent);
    let modalRouterProgressComponentRef = this.viewContainerRef.createComponent(componentFactory);
    modalRouterProgressComponentRef.instance.Closing.subscribe(() => {
      modalRouterProgressComponentRef.destroy();
    });


    // 更新の場合は事前に更新可能かチェックする。
    if (this.ComponentStatus == COMPONENT_STATUS_TYPE.UPDATE) {
      // 詰め替えを行う。
      this.receiptService.Get(parseInt(this.paramId))
        .subscribe(response => {
          if(response!=PROCESS_RESULT_RESULT_TYPE.FAILURE){
            this.currentReceipt = response[0];
            if (!this.canUpdate()) {
              modalRouterProgressComponentRef.destroy();
              return;
            }
            else {
              this.receiptService.Save(receiptSaveItem)
                .subscribe(
                  response => {
  
                    this.processCustomResult = this.processResultService.processAtSave(
                      this.processCustomResult, response, false, this.partsResultMessageComponent);
  
                    if (response != PROCESS_RESULT_RESULT_TYPE.FAILURE ) {
                      let receiptInputsResult:ReceiptInputsResult = response;
                      this.ComponentStatus=COMPONENT_STATUS_TYPE.CREATE;
                      this.clear();
  
                      if (this.paramFrom == ComponentId.PE0102) {
                        this.receiptService.Get(receiptInputsResult.receiptInputs[0].id)
                          .subscribe(response => {
                            if (response != PROCESS_RESULT_RESULT_TYPE.FAILURE) {
                              let receipt: Receipt[] = response;
                              this.matchingService.collationInfo.editRecipt = receipt[0];
                              this.router.navigate(['main/PE0102', { "process": "update", from: ComponentId.PD0301 }]);
                              modalRouterProgressComponentRef.destroy();
                            }
                            else {
                              modalRouterProgressComponentRef.destroy();
                              this.processCustomResult = this.processResultService.processAtFailure(
                                this.processCustomResult, MSG_ERR.SAVE_ERROR, this.partsResultMessageComponent);
                            }
                          });
                      }
                      else if (this.paramFrom == ComponentId.PD0501) {
                        modalRouterProgressComponentRef.destroy();
                        this.router.navigate(['main/PD0501', { "process": "back" }]);
                      }
                      else{
                        modalRouterProgressComponentRef.destroy();
                      }
                    }
                  });
            }
          }
          else{
            modalRouterProgressComponentRef.destroy();
            this.processCustomResult = this.processResultService.processAtFailure(
              this.processCustomResult,MSG_ERR.SAVE_ERROR + "(更新対象のデータの取得に失敗しました。)",
              this.partsResultMessageComponent);
          }

        });
    }
    else if (this.ComponentStatus == COMPONENT_STATUS_TYPE.CREATE) {
      this.receiptService.Save(receiptSaveItem)
        .subscribe(
          response => {
            if (response != PROCESS_RESULT_RESULT_TYPE.FAILURE ) {
              let receiptInputsResult:ReceiptInputsResult = response;

              this.ComponentStatus = COMPONENT_STATUS_TYPE.CREATE;
              this.clear();

              if (this.paramFrom == ComponentId.PE0102) {
                this.receiptService.Get(receiptInputsResult.receiptInputs[0].id)
                  .subscribe(response => {
                    if (response != PROCESS_RESULT_RESULT_TYPE.FAILURE ) {
                      let receipt: Receipt[] = response;
                      this.matchingService.collationInfo.registryReceipts = receipt;
                      modalRouterProgressComponentRef.destroy();
                      this.router.navigate(['main/PE0102', { "process": "registry", from: ComponentId.PD0301 }]);
                    }
                    else {
                      modalRouterProgressComponentRef.destroy();
                      this.processCustomResult = this.processResultService.processAtFailure(
                        this.processCustomResult, MSG_ERR.SAVE_ERROR, this.partsResultMessageComponent);
                    }
                  });
              }
              else{
                modalRouterProgressComponentRef.destroy();
              }
            }
            else{
              modalRouterProgressComponentRef.destroy();
            }
            this.processCustomResult = this.processResultService.processAtSave(
              this.processCustomResult, response, true, this.partsResultMessageComponent);

          });
    }

  }


  public canUpdate(): boolean {

    if (this.currentReceipt == undefined || this.currenciesResult == null) {
      this.processCustomResult = this.processResultService.processAtWarning(
        this.processCustomResult, MSG_WNG.INVALID_RECEIPT_ID_FOR_RECEIPT.replace(MSG_ITEM_NUM.FIRST, this.paramId),
        this.partsResultMessageComponent);
      return false;
    }

    if (this.currentReceipt.assignmentFlag > 0) {
      this.processCustomResult = this.processResultService.processAtWarning(
        this.processCustomResult, '消込済みの入金データは修正できません。',
        this.partsResultMessageComponent);
      return false;
    }

    if (!StringUtil.IsNullOrEmpty(this.currentReceipt.outputAt)) {
      this.processCustomResult = this.processResultService.processAtWarning(
        this.processCustomResult, '仕訳出力済みの入金データは修正できません。',
        this.partsResultMessageComponent);
      return false;
    }

    if (this.currentReceipt.originalReceiptId != undefined && this.currentReceipt.originalReceiptId > 0) {
      this.processCustomResult = this.processResultService.processAtWarning(
        this.processCustomResult, '前受済みの入金データは修正できません。',
        this.partsResultMessageComponent);

      return false;
    }

    if (!StringUtil.IsNullOrEmpty(this.currentReceipt.deleteAt)) {
      this.processCustomResult = this.processResultService.processAtWarning(
        this.processCustomResult, '削除済みの入金データは修正できません。',
        this.partsResultMessageComponent);
      return false;
    }

    return true;

  }

  public getReceiptInputDataForRegistry():Array<ReceiptInput>{

    let receiptInputs = new Array<ReceiptInput>();
    for (let index = 0; index < this.detailDispReceipts.length; index++) {

      let receiptInput = new ReceiptInput();

      receiptInput.id = this.detailDispReceipts[index].id;
      receiptInput.companyId = this.userInfoService.Company.id;

      this.currenciesResult.currencies.forEach(element => {
        if (element.code === "JPY") {
          receiptInput.currencyId = element.id;
        }
      });

      if (this.ComponentStatus == COMPONENT_STATUS_TYPE.UPDATE) {
        receiptInput.receiptHeaderId = this.currentReceipt.receiptHeaderId;
      }
      else {
        receiptInput.receiptHeaderId = null;
      }

      receiptInput.receiptCategoryId = this.detailDispReceipts[index].receiptCategory.id;
      receiptInput.receiptCategoryCode = this.detailDispReceipts[index].receiptCategory.code;
      receiptInput.customerId = this.selectedCustomer.id;
      receiptInput.sectionId = this.sectionId;

      if (this.ComponentStatus == COMPONENT_STATUS_TYPE.UPDATE) {
        receiptInput.inputType = this.currentReceipt.inputType;
      }
      else {
        receiptInput.inputType = RECEIPT_INPUT_TYPE_DICTIONARY[2].id;
      }

      receiptInput.apportioned = 1;
      receiptInput.approved = 1;
      // receiptInput.workday
      receiptInput.recordedAt = DateUtil.ConvertFromDatepicker(this.recordedAtCtrl);
      receiptInput.originalRecordedAt = null;
      receiptInput.receiptAmount = StringUtil.IsNullOrEmpty(this.detailDispReceipts[index].receiptAmount) ? 0 : parseInt(this.detailDispReceipts[index].receiptAmount.replace(",", ""));
      receiptInput.assignmentAmount = 0;
      receiptInput.remainAmount = receiptInput.receiptAmount;
      receiptInput.assignmentFlag = 0;


      if (this.ComponentStatus == COMPONENT_STATUS_TYPE.UPDATE && this.currentReceipt.inputType == ReceiptInputType.EbFile) {
        receiptInput.payerCode = this.currentReceipt.payerCode;
        receiptInput.payerName = this.currentReceipt.payerName;
        receiptInput.payerNameRaw = this.currentReceipt.payerNameRaw;
        receiptInput.sourceBankName = this.currentReceipt.sourceBankName;
        receiptInput.sourceBranchName = this.currentReceipt.sourceBranchName;
      }
      else {
        receiptInput.payerCode = ""
        receiptInput.payerName = this.payerNameCtrl.value;
        receiptInput.payerNameRaw = this.payerNameCtrl.value;
        receiptInput.sourceBankName = "";
        receiptInput.sourceBranchName = "";
      }

        receiptInput.outputAt = null;
        receiptInput.dueAt = this.detailDispReceipts[index].dueAt;
        receiptInput.mailedAt = null;
        receiptInput.originalReceiptId = null;
        receiptInput.excludeFlag = 0;
        receiptInput.excludeCategoryId = null;
        receiptInput.excludeAmount = 0;
        receiptInput.referenceNumber = "";
        receiptInput.recordNumber = "";
        receiptInput.densaiRegisterAt = null;
        receiptInput.note1 = this.detailDispReceipts[index].note1 == null ? "" : this.detailDispReceipts[index].note1;
        receiptInput.note2 = this.detailDispReceipts[index].note2 == null ? "" : this.detailDispReceipts[index].note2;
        receiptInput.note3 = this.detailDispReceipts[index].note3 == null ? "" : this.detailDispReceipts[index].note3;
        receiptInput.note4 = this.detailDispReceipts[index].note4 == null ? "" : this.detailDispReceipts[index].note4;
        receiptInput.billNumber = this.detailDispReceipts[index].billNumber == null ? "" : this.detailDispReceipts[index].billNumber;
        receiptInput.billBankCode = this.detailDispReceipts[index].billBankCode == null ? "" : this.detailDispReceipts[index].billBankCode;
        receiptInput.billBranchCode = this.detailDispReceipts[index].billBranchCode == null ? "" : this.detailDispReceipts[index].billBranchCode;
        receiptInput.billDrawAt = this.detailDispReceipts[index].billDrawAt;
        receiptInput.billDrawer = this.detailDispReceipts[index].billDrawer == null ? "" : this.detailDispReceipts[index].billDrawer;
        receiptInput.deleteAt = null;
        receiptInput.createBy = this.userInfoService.LoginUser.id;
        //receiptInput.createAt
        receiptInput.updateBy = this.userInfoService.LoginUser.id;
        if (this.ComponentStatus == COMPONENT_STATUS_TYPE.UPDATE) {
          receiptInput.updateAt = this.currentReceipt.updateAt;
        }

        receiptInput.memo = this.detailDispReceipts[index].memo;
        receiptInput.learnKanaHistory = this.cbxSaveKanaHistoryCtrl.value;
        receiptInput.lineNo = (index + 1);
        receiptInput.customerCode = this.customerCodeCtrl.value;
        receiptInput.currencyCode = "JPY";

        // receiptInput.recordAtForPrint: string | null;
        // receiptInput.dueAtForPrint: string | null;
        // receiptInput.billDrawAtForPrint: string | null;
        // receiptInput.receiptAmountForPrint: string | null;
        receiptInput.sectionCode = this.sectionCodeCtrl.value;
        //receiptInput.collationKey: string | null;
        //receiptInput.bankCode: string | null;
        //receiptInput.bankName: string | null;
        //receiptInput.branchCode: string | null;
        //receiptInput.branchName: string | null;
        //receiptInput.accountTypeId: number | null;
        //receiptInput.accountNumber: string | null;
        //receiptInput.accountName: string | null;

        receiptInputs.push(receiptInput);
    }

    return receiptInputs;
  }

  public getReceiptDisplayDataForAdd(): ReceiptDisplay[] {

    let recceiptDisplaies = new Array<ReceiptDisplay>();
    for (let index = 0; index < this.receiptCount; index++) {
      if (!this.validateGridEmptyByRow(index)) {
        let receiptDisplay = new ReceiptDisplay();

        receiptDisplay.id=this.receipts[index].id;
        receiptDisplay.receiptCategoryId = this.detailReceiptCategoryIdCtrls[index].value;
        receiptDisplay.receiptCategoryCode = this.detailReceiptCategoryCodeCtrls[index].value;
        receiptDisplay.receiptCategory = this.detailReceiptCategory;
        receiptDisplay.notInputDueFlag = this.notInputDueFlags[index];

        receiptDisplay.receiptAmount = this.detailReceiptAmountCtrls[index].value;

        receiptDisplay.dueAt = DateUtil.ConvertFromDatepicker(this.detailDueAtCtrls[index]);

        receiptDisplay.note1 = this.detailNote1Ctrls[index].value == null ? "" : this.detailNote1Ctrls[index].value;
        receiptDisplay.note2 = this.detailNote2Ctrls[index].value == null ? "" : this.detailNote2Ctrls[index].value;
        receiptDisplay.note3 = this.detailNote3Ctrls[index].value == null ? "" : this.detailNote3Ctrls[index].value;
        receiptDisplay.note4 = this.detailNote4Ctrls[index].value == null ? "" : this.detailNote4Ctrls[index].value;

        receiptDisplay.memo = this.receiptMemos[index]!=null?this.receiptMemos[index].memo:"";
        receiptDisplay.noteFlag = StringUtil.IsNullOrEmpty(this.detailNoteFlagCtrls[index].value)?false:true;

        receiptDisplay.billNumber = this.detailBillNumberCtrls[index].value == null ? "" : this.detailBillNumberCtrls[index].value;
        receiptDisplay.billBankCode = this.detailBankCodeCtrls[index].value == null ? "" : this.detailBankCodeCtrls[index].value;
        receiptDisplay.billBranchCode = this.detailBranchCodeCtrls[index].value == null ? "" : this.detailBranchCodeCtrls[index].value;
        receiptDisplay.billBank = this.detailBillBankCtrls[index].value == null ? "" : this.detailBillBankCtrls[index].value;
        receiptDisplay.billDrawAt = DateUtil.ConvertFromDatepicker(this.detailBillDrawAtCtrls[index]);
        receiptDisplay.billDrawer = this.detailBillDrawerCtrls[index].value == null ? "" : this.detailBillDrawerCtrls[index].value;

        recceiptDisplaies.push(receiptDisplay);
      }
    }

    return recceiptDisplaies;
  }


  public setReceiptFromDisplay(lineNo:number) {

    let receiptDisplay = this.detailDispReceipts[lineNo];

    let index=0;


    this.detailReceiptCategoryIdCtrls[index].setValue(receiptDisplay.receiptCategoryId);
    this.detailReceiptCategoryCodeCtrls[index].setValue(receiptDisplay.receiptCategoryCode);
    this.detailReceiptCategory = receiptDisplay.receiptCategory;

    this.notInputDueFlags[index] = receiptDisplay.notInputDueFlag;

    this.detailReceiptAmountCtrls[index].setValue(receiptDisplay.receiptAmount);

    let year = receiptDisplay.dueAt.split('/')[0];
    let month = receiptDisplay.dueAt.split('/')[1];
    let date = receiptDisplay.dueAt.split('/')[2];
    this.detailDueAtCtrls[index].setValue(new NgbDate(parseInt(year), parseInt(month) , parseInt(date)));

    this.detailNote1Ctrls[index].setValue(receiptDisplay.note1);
    this.detailNote2Ctrls[index].setValue(receiptDisplay.note2);
    this.detailNote3Ctrls[index].setValue(receiptDisplay.note3);
    this.detailNote4Ctrls[index].setValue(receiptDisplay.note4);

    if(receiptDisplay.noteFlag){
      this.detailNoteFlagCtrls[index].setValue("〇");
      this.receiptMemos[index] = new ReceiptMemo();
      this.receiptMemos[index].receiptId=receiptDisplay.id;
      this.receiptMemos[index].memo=receiptDisplay.memo;
    }
    else{
      this.detailNoteFlagCtrls[index].setValue("");
      this.receiptMemos[index] = new ReceiptMemo();
      this.receiptMemos[index].receiptId=0;
      this.receiptMemos[index].memo="";
    }

    this.detailBillNumberCtrls[index].setValue(receiptDisplay.billNumber);
    this.detailBankCodeCtrls[index].setValue(receiptDisplay.billBankCode);
    this.detailBranchCodeCtrls[index].setValue(receiptDisplay.billBranchCode);
    this.detailBillBankCtrls[index].setValue(receiptDisplay.billBank);

    year = receiptDisplay.billDrawAt.split('/')[0];
    month = receiptDisplay.billDrawAt.split('/')[1];
    date = receiptDisplay.billDrawAt.split('/')[2];
    this.detailBillDrawAtCtrls[index].setValue(new NgbDate(parseInt(year), parseInt(month) , parseInt(date)));

    this.detailBillDrawerCtrls[index].setValue(receiptDisplay.billDrawer);
    
  }


  public delete() {

    // 削除処理
    let componentFactory = this.componentFactoryResolver.resolveComponentFactory(ModalConfirmComponent);
    let componentRef = this.viewContainerRef.createComponent(componentFactory);
    componentRef.instance.ActionName = "削除"
    componentRef.instance.Closing.subscribe(() => {

      if (componentRef.instance.ModalStatus == MODAL_STATUS_TYPE.OK) {

        // 動作中のコンポーネントを開く
        let componentFactory = this.componentFactoryResolver.resolveComponentFactory(ModalRouterProgressComponent);
        let modalRouterProgressComponentRef = this.viewContainerRef.createComponent(componentFactory);
        modalRouterProgressComponentRef.instance.Closing.subscribe(() => {
          modalRouterProgressComponentRef.destroy();
        });

        this.receiptService.Delete(this.receiptIdCtrl.value)
          .subscribe(result => {
            this.processCustomResult = this.processResultService.processAtDelete(
              this.processCustomResult, result, this.partsResultMessageComponent);
            modalRouterProgressComponentRef.destroy();

            if(result!=PROCESS_RESULT_RESULT_TYPE.FAILURE){
              if (this.paramFrom == ComponentId.PE0102) {
                this.matchingService.collationInfo.deleteReceiptId = this.receiptIdCtrl.value;
                this.router.navigate(['main/PE0102', { "process": "delete", from: ComponentId.PD0301 }]);
              }
              else {
                this.router.navigate(['main/PD0501', { "process": "back" }]);
              }
            }
          });
      }
      else{
        this.processCustomResult = this.processResultService.processAtWarning(
          this.processCustomResult, MSG_WNG.CANCEL_PROCESS, this.partsResultMessageComponent);

      }
      componentRef.destroy();
    });
  }


  public billingSearch() {
    this.receiptService.ReceiptInputFormGroup = this.MyFormGroup;
    let customerCode: string = this.customerCodeCtrl.value == undefined ? "" : this.customerCodeCtrl.value;
    let customerName: string = this.customerNameCtrl.value == undefined ? "" : this.customerNameCtrl.value;
    this.router.navigate(["/main/PC0301", { from: ComponentId.PD0301, code: customerCode, name: customerName }]);
  }

  public receiptSearch() {
    this.router.navigate(["/main/PD0501", { from: ComponentId.PD0301 }]);
  }

  //////////////////////////////////////////////////////////////////
  public setRecordedAt(eventType: string) {
    HtmlUtil.nextFocusByName(this.elementRef, 'customerCodeCtrl', eventType);
  }

  //////////////////////////////////////////////////////////////////
  public setCustomerCode(eventType: string) {

    if (eventType != EVENT_TYPE.BLUR) {
      this.customerCodeTrigger.closePanel();
    }
    
    if (!StringUtil.IsNullOrEmpty(this.customerCodeCtrl.value)) {

      if (this.userInfoService.ApplicationControl.customerCodeType == CODE_TYPE.NUMBER) {
        this.customerCodeCtrl.setValue(StringUtil.setPaddingFrontZero(this.customerCodeCtrl.value, this.userInfoService.ApplicationControl.customerCodeLength));
      }

      this.loadStart();
      this.customerService.GetItems(this.customerCodeCtrl.value)
        .subscribe(response => {
          this.loadEnd();
          if (response != undefined && response.length > 0) {
            this.customerCodeCtrl.setValue(response[0].code);
            this.selectedCustomer = response[0];
            this.customerNameCtrl.setValue(response[0].name);
            this.payerNameCtrl.setValue(response[0].kana);
            HtmlUtil.nextFocusByNames(this.elementRef, ['sectionCodeCtrl', 'detailReceiptCategoryCodeCtrl0'], eventType);
          }
          else {
            // let msg = MSG_WNG.MASTER_NOT_EXIST.split(MSG_ITEM_NUM.FIRST).join("得意先");
            // msg = msg.replace(MSG_ITEM_NUM.SECOND, this.customerCodeCtrl.value);
            // this.processCustomResult = this.processResultService.processAtWarning(this.processCustomResult, msg);
            //this.customerCodeCtrl.setValue("");
            this.selectedCustomer = null;
            this.customerNameCtrl.setValue("");
            this.payerNameCtrl.setValue("");
          }
        });
    }
    else {
      this.customerCodeCtrl.setValue("");
      this.selectedCustomer = null;
      this.customerNameCtrl.setValue("");
      HtmlUtil.nextFocusByNames(this.elementRef, ['sectionCodeCtrl', 'detailReceiptCategoryCodeCtrl0'], eventType);
    }

  }

  //////////////////////////////////////////////////////////////////
  public setSectionCode(eventType: string) {

    if (eventType != EVENT_TYPE.BLUR) {
      this.sectionCodeTrigger.closePanel();
    }

    if (!StringUtil.IsNullOrEmpty(this.sectionCodeCtrl.value)) {

      if (this.userInfoService.ApplicationControl.sectionCodeType == CODE_TYPE.NUMBER) {
        this.sectionCodeCtrl.setValue(StringUtil.setPaddingFrontZero(this.sectionCodeCtrl.value, this.userInfoService.ApplicationControl.sectionCodeLength));
      }

      this.loadStart();
      this.sectionService.GetItems(this.sectionCodeCtrl.value)
        .subscribe(response => {
          this.loadEnd();
          if (response != undefined && response.length > 0) {
            this.sectionCodeCtrl.setValue(response[0].code);
            this.sectionId = response[0].id;
            this.sectionNameCtrl.setValue(response[0].name);
            HtmlUtil.nextFocusByNames(this.elementRef, ['currencyCodeCtrl', 'detailReceiptCategoryCodeCtrl0'], eventType);
          }
          else {
            // let msg = MSG_WNG.MASTER_NOT_EXIST.split(MSG_ITEM_NUM.FIRST).join("入金部門");
            // msg = msg.replace(MSG_ITEM_NUM.SECOND, this.sectionCodeCtrl.value);
            // this.processCustomResult = this.processResultService.processAtWarning(this.processCustomResult, msg);
            //this.sectionCodeCtrl.setValue("");
            this.sectionNameCtrl.setValue("");
            this.sectionId = 0;
          }
        });
    }
    else {
      this.sectionCodeCtrl.setValue("");
      this.sectionNameCtrl.setValue("");
      this.sectionId = 0;
      HtmlUtil.nextFocusByName(this.elementRef, 'sectionCodeCtrl', eventType);
    }
  }

  //////////////////////////////////////////////////////////////////
  public setCurrencyCode(eventType: string) {

    if (!StringUtil.IsNullOrEmpty(this.currencyCodeCtrl.value)) {

      this.currencyService.GetItems(this.currencyCodeCtrl.value)
        .subscribe(response => {
          if (response != undefined && response.length > 0) {
            this.currencyCodeCtrl.setValue(response[0].code);
            HtmlUtil.nextFocusByName(this.elementRef, 'recordedAtCtrl', eventType);
          }
          else {
            this.currencyCodeCtrl.setValue("");
            HtmlUtil.nextFocusByName(this.elementRef, 'recordedAtCtrl', eventType);
          }
        });
    }
    else {
      this.currencyCodeCtrl.setValue("");
      HtmlUtil.nextFocusByName(this.elementRef, 'recordedAtCtrl', eventType);
    }


  }

  //////////////////////////////////////////////////////////////////
  // 詳細
  //////////////////////////////////////////////////////////////////



  //////////////////////////////////////////////////////////////////
  public setDetailReceiptCategoryCode(eventType: any, index: number) {

    if(eventType!=EVENT_TYPE.BLUR){
      this.detailReceiptCategoryCodeTrigger.closePanel();
    }

    if (!StringUtil.IsNullOrEmpty(this.detailReceiptCategoryCodeCtrls[index].value)) {

      if (eventType == "blur") {
        this.detailReceiptCategoryCodeCtrls[index].setValue(this.detailReceiptCategoryCodeCtrls[index].value.split(":")[0]);
      }

      this.detailReceiptCategoryCodeCtrls[index].setValue(StringUtil.setPaddingFrontZero(this.detailReceiptCategoryCodeCtrls[index].value, 2));

      this.loadStart();
      this.categoryService.GetItems(CategoryType.Receipt, this.detailReceiptCategoryCodeCtrls[index].value)
        .subscribe(response => {
          this.loadEnd();
          if (response != undefined && response.length > 0) {
            this.detailReceiptCategoryCodeCtrls[index].setValue(response[0].code + ":" + response[0].name);
            this.detailReceiptCategoryIdCtrls[index].setValue(response[0].id);
            this.detailReceiptCategory = response[0];

            // 期日入力ありの場合は、期日入力無フラグをFalseにする。
            this.notInputDueFlags[index] = response[0].useLimitDate == 1 ? true : false;

            HtmlUtil.nextFocusByName(this.elementRef, 'detailNote1Ctrl' + index, eventType);
          }
          else {
            this.notInputDueFlags[index] = false;
            // this.detailReceiptCategoryCodeCtrls[index].setValue("");
            this.detailReceiptCategoryIdCtrls[index].setValue("");
            this.detailReceiptCategory = null;
            // HtmlUtil.nextFocusByName(this.elementRef, 'detailNote1Ctrl' + index, eventType);
          }

          this.setNotInput(this.notInputDueFlags[index], index);


        });
    }
    else {
      HtmlUtil.nextFocusByName(this.elementRef, 'detailNote1Ctrl' + index, eventType);
      this.detailReceiptCategoryIdCtrls[index].setValue("");
      this.detailReceiptCategoryCodeCtrls[index].setValue("");
      this.detailReceiptCategory = null;

      this.notInputDueFlags[index] = false;
      this.setNotInput(this.notInputDueFlags[index], index);

    }
  }

  public initDetailReceiptCategoryCode(index: number) {

    if (!StringUtil.IsNullOrEmpty(this.detailReceiptCategoryCodeCtrls[index].value)) {
      let tmp = this.detailReceiptCategoryCodeCtrls[index].value;
      tmp = tmp.split(":")[0];

      this.detailReceiptCategoryCodeCtrls[index].setValue(tmp);
    }

  }

  //////////////////////////////////////////////////////////////////
  public setDetailNote1(eventType: string, index: number) {
    HtmlUtil.nextFocusByNames(this.elementRef, ['detailDueAtCtrl' + index, 'detailReceiptAmountCtrl' + index], eventType);
  }

  //////////////////////////////////////////////////////////////////
  public setDetailDueAt(eventType: string, index: number) {
    HtmlUtil.nextFocusByName(this.elementRef, 'detailReceiptAmountCtrl' + index, eventType);
  }

  //////////////////////////////////////////////////////////////////
  public setDetailReceiptAmount(eventType: string, index: number) {
    HtmlUtil.nextFocusByNames(this.elementRef,
      ['detailBillNumberCtrl' + index, 'detailNote2Ctrl' + index], eventType);
  }

  public inputDetailReceiptAmount() {

    let sumAmount: number = 0;

    for (let index = 0; index < this.detailDispReceipts.length; index++) {

      sumAmount = sumAmount + NumberUtil.ParseInt(this.detailDispReceipts[index].receiptAmount);
    }

    this.sumReceiptAmountCtrl.setValue(sumAmount);
    if (this.paramFrom == ComponentId.PE0102 && StringUtil.IsNullOrEmpty(this.paramId)){
      this.amountDifferenceCtrl.setValue(this.remainAmountCtrl.value - sumAmount);
    }
  }

  public setCurrencyForDetailReceiptAmount(index: number) {
    let pipe = new RacCurrencyPipe();
    this.detailReceiptAmountCtrls[index].setValue(pipe.transform(this.detailReceiptAmountCtrls[index].value));
  }

  public initCurrencyForDetailReceiptAmount(index: number) {
    
    if (this.detailReceiptAmountCtrls[index].value == undefined) return;

    let pipe = new RacCurrencyPipe();
    this.detailReceiptAmountCtrls[index].setValue(pipe.reverceTransform(this.detailReceiptAmountCtrls[index].value));
  }

  //////////////////////////////////////////////////////////////////
  public setDetailNoteFlag(index: number) {

    if (
      StringUtil.IsNullOrEmpty(this.receiptMemos[index].memo)
    ) {
      this.detailNoteFlagCtrls[index].setValue("");
    }
    else {
      this.detailNoteFlagCtrls[index].setValue("〇");
    }
  }

  //////////////////////////////////////////////////////////////////
  public openReceiptMemoModal(index: number) {
    let componentFactory = this.componentFactoryResolver.resolveComponentFactory(ModalMemoComponent);
    let componentRef = this.viewContainerRef.createComponent(componentFactory);

    this.receiptMemos[index].receiptId = index + 1;
    componentRef.instance.receipt = this.receipts[index];
    componentRef.instance.receiptMemo = this.receiptMemos[index];
    componentRef.instance.categoryType = CategoryType.Receipt;

    componentRef.instance.Closing.subscribe(() => {
      componentRef.destroy();
      this.setDetailNoteFlag(index);
    });
  }

  public detailClear(index: number) {

    //this.billings[index];
    this.receiptMemos[index].memo = "";

    this.detailReceiptCategoryIdCtrls[index].setValue("");
    this.detailReceiptCategoryCodeCtrls[index].setValue("");
    this.detailReceiptCategory = null;

    this.detailNote1Ctrls[index].setValue("");
    this.detailDueAtCtrls[index].setValue(null);
    this.detailReceiptAmountCtrls[index].setValue("");
    this.detailNoteFlagCtrls[index].setValue("");

    this.detailBillNumberCtrls[index].setValue("");
    this.detailBankCodeCtrls[index].setValue("");
    this.detailBranchCodeCtrls[index].setValue("");
    this.detailBillBankCtrls[index].setValue("");
    this.detailBillDrawAtCtrls[index].setValue(null);
    this.detailBillDrawerCtrls[index].setValue("");
    this.detailNote2Ctrls[index].setValue("");
    this.detailNote3Ctrls[index].setValue("");
    this.detailNote4Ctrls[index].setValue("");

    this.componentDetailStatus=COMPONENT_DETAIL_STATUS_TYPE.CREATE;
    this.detailEditLineNo=-1;

    // 期日入力を初期化
    this.notInputDueFlags[index] = false;
    this.setNotInput(this.notInputDueFlags[index], index);

  }

  //////////////////////////////////////////////////////////////////
  public setDetailBillNumber(eventType: string, index: number) {
    HtmlUtil.nextFocusByName(this.elementRef, 'detailBankCodeCtrl' + index, eventType);
  }

  //////////////////////////////////////////////////////////////////
  public setDetailBankCode(eventType: any, index: number) {

    if (StringUtil.IsNullOrEmpty(this.detailBankCodeCtrls[index].value)) {
      HtmlUtil.nextFocusByName(this.elementRef, 'detailBranchCodeCtrl' + index, eventType);
      return;
    }

    this.detailBankCodeCtrls[index].setValue(StringUtil.setPaddingFrontZero(this.detailBankCodeCtrls[index].value, 4));

    if (this.setDetailBankInfo(index)) {
      HtmlUtil.nextFocusByName(this.elementRef, 'detailBillDrawAtCtrl' + index, eventType);
    }

  }

  public setDetailBranchCode(eventType: any, index: number) {

    if (StringUtil.IsNullOrEmpty(this.detailBranchCodeCtrls[index].value)) {
      HtmlUtil.nextFocusByName(this.elementRef, 'detailBillDrawAtCtrl' + index, eventType);
      return;
    }

    this.detailBranchCodeCtrls[index].setValue(StringUtil.setPaddingFrontZero(this.detailBranchCodeCtrls[index].value, 3));

    if (this.setDetailBankInfo(index)) {
      HtmlUtil.nextFocusByName(this.elementRef, 'detailBillDrawAtCtrl' + index, eventType);
    }

  }

  public setDetailBankInfo(index: number): boolean {

    let bRtn: boolean = false;
    if (
      !StringUtil.IsNullOrEmpty(this.detailBankCodeCtrls[index].value)
      && !StringUtil.IsNullOrEmpty(this.detailBranchCodeCtrls[index].value)
    ) {

      this.bankBranchsResult.bankBranches.forEach(element => {
        if (
          element.bankCode == this.detailBankCodeCtrls[index].value
          && element.branchCode == this.detailBranchCodeCtrls[index].value
        ) {
          this.detailBillBankCtrls[index].setValue(element.bankName + " " + element.branchName);
          bRtn = true;
        }

      });
    }

    return bRtn;

  }

  //////////////////////////////////////////////////////////////////
  public setDetailBillDrawAt(eventType: string, index: number) {
    HtmlUtil.nextFocusByName(this.elementRef, 'detailBillDrawerCtrl' + index, eventType);
  }

  //////////////////////////////////////////////////////////////////
  public setDetailBillDrawer(eventType: string, index: number) {
    HtmlUtil.nextFocusByName(this.elementRef, 'detailNote2Ctrl' + index, eventType);
  }

  //////////////////////////////////////////////////////////////////
  public setDetailNote2(eventType: string, index: number) {
    HtmlUtil.nextFocusByName(this.elementRef, 'detailNote3Ctrl' + index, eventType);
  }

  //////////////////////////////////////////////////////////////////
  public setDetailNote3(eventType: string, index: number) {
    HtmlUtil.nextFocusByName(this.elementRef, 'detailNote4Ctrl' + index, eventType);
  }

  //////////////////////////////////////////////////////////////////
  public setDetailNote4(eventType: string, index: number) {

    if (index + 1 < this.receiptCount) {
      HtmlUtil.nextFocusByName(this.elementRef, 'detailReceiptCategoryCodeCtrl' + (index + 1), eventType);
    }
    else{
      HtmlUtil.nextFocusByName(this.elementRef, 'detailReceiptCategoryCodeCtrl0', eventType);
    }
  }

  public disableBack(): boolean {

    // 更新処理の場合
    if (this.ComponentStatus == COMPONENT_STATUS_TYPE.UPDATE) return false;

    // 個別消込から遷移した場合
    if (this.paramFrom == ComponentId.PE0102) return false;

    return true;

  }


  /*
    明細行の追加ボタン
  */
  public detailAdd(lineNo:number,clearFlag:boolean=false){

    this.processResultService.clearProcessCustomMsg(this.processCustomResult);

    // 最大行数のチェック
    if(this.receiptMaxCount==this.detailDispReceipts.length){
      this.processCustomResult = 
      this.processResultService.processAtWarning(
        this.processCustomResult, MSG_WNG.INPUT_GRID_MAX_COUNT.replace(MSG_ITEM_NUM.FIRST, "" + this.receiptMaxCount),
        this.partsResultMessageComponent);
      return;

    }

    this.setGridInput("detailAdd",clearFlag);

  }


  /*
    明細行の更新ボタン
  */
  public detailUpdate(lineNo:number,clearFlag:boolean=false){

    this.processResultService.clearProcessCustomMsg(this.processCustomResult);

    this.setGridInput("detailUpdate",clearFlag);
    
  }

  /*
    明細行の編集ボタン
  */
  public detailEdit(lineNo:number){

    this.componentDetailStatus = COMPONENT_DETAIL_STATUS_TYPE.UPDATE;
    this.detailEditLineNo=lineNo;


    // 表示部の内容を明細に設定する
    this.setReceiptFromDisplay(lineNo);
  }


  /*
    明細行の削除ボタン
  */
  public detailDelete(lineNo:number){

    this.detailDispReceipts = this.detailDispReceipts.filter((value,index)=>{
      if(index==lineNo){
        return false;
      } 
      return true;
    })

  }



  /*
    明細行のヘッダーの背景色の変更処理
  */

  public currentInputItem:PD0301_INPUT;
  public currentLineNo:number;


  public setFocus(lineNo:number,inputItem:PD0301_INPUT){
    this.currentLineNo=lineNo;
    this.currentInputItem=inputItem;
  }

  public setBlur(lineNo:number,inputItem:PD0301_INPUT){
    this.currentInputItem=PD0301_INPUT.none;
  }

  public getTdColor(inputItem:PD0301_INPUT){
    return this.currentInputItem==inputItem;
  }

  public getTdNoColor(lineNo:number){
    return this.currentLineNo==lineNo;
  }  
}

export enum PD0301_INPUT{
  none=0,                         // なし
  ////////////////////////////////////////
  detailReceiptCategoryCodeCtrl,  // 入金区分
  detailNote1Ctrl,                // 備考１
  detailDueAtCtrl,                // 期日
  detailReceiptAmountCtrl,        // 金額
  detailNoteFlagCtrl,             // メモ入力フラグ
  // メモボタン
  // クリアボタン
  ////////////////////////////////////////
  detailBillNumberCtrl,           // 手形番号
  detailBankCodeCtrl,             // 銀行コード
  detailBranchCodeCtrl,           // 支店コード
  detailBillBankCtrl,             // 手形券面銀行
  detailBillDrawAtCtrl,           // 提出日
  detailBillDrawerCtrl,           // 差出人
  ////////////////////////////////////////
  detailNote2Ctrl,                // 備考2名称
  detailNote3Ctrl,                // 備考3
  detailNote4Ctrl,                // 備考4
}
