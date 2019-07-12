import { Component, OnInit, ElementRef, ComponentFactoryResolver, ViewContainerRef, AfterViewInit, ViewChild } from '@angular/core';
import { ReceiptsResult } from 'src/app/model/receipts-result.model';
import { ReceiptSearch } from 'src/app/model/receipt-search.model';
import { TABLE_INDEX } from 'src/app/common/const/table-name.const';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { ReceiptService } from 'src/app/service/receipt.service';
import { CategoryMasterService } from 'src/app/service/Master/category-master.service';
import { KEY_CODE, EVENT_TYPE, BUTTON_ACTION } from 'src/app/common/const/event.const';
import { ModalMasterComponent } from 'src/app/component/modal/modal-master/modal-master.component';
import { HtmlUtil } from 'src/app/common/util/html.util';
import { StringUtil } from 'src/app/common/util/string-util';
import { BaseComponent } from 'src/app/component/common/base/base-component';
import { FormatterUtil } from 'src/app/common/util/formatter.util';
import { MODAL_STATUS_TYPE, PROCESS_RESULT_RESULT_TYPE } from 'src/app/common/const/status.const';
import { UserInfoService } from 'src/app/service/common/user-info.service';
import { NgbDateParserFormatter } from '@ng-bootstrap/ng-bootstrap';
import { NgbDateCustomParserFormatter } from 'src/app/common/util/dateformat-util';
import { CategoryType, MATCHING_ASSIGNMENT_FLAG_DICTIONARY, CODE_TYPE, RECEIPT_STATUS_MESSAGE_DICTIONARY } from 'src/app/common/const/kbn.const';
import { CustomerMasterService } from 'src/app/service/Master/customer-master.service';
import { SectionMasterService } from 'src/app/service/Master/section-master.service';
import { CurrencyMasterService } from 'src/app/service/Master/currency-master.service';
import { JuridicalPersonalityMasterService } from 'src/app/service/Master/juridical-personality-master.service';
import { ColumnNameSettingMasterService } from 'src/app/service/Master/column-name-setting-master.service';
import { JuridicalPersonalitysResult } from 'src/app/model/juridical-personalitys-result.model';
import { ColumnNameSettingsResult } from 'src/app/model/column-name-settings-result.model';
import { forkJoin } from 'rxjs';
import { DateUtil } from 'src/app/common/util/date-util';
import { FreeImporterFormatType } from 'src/app/common/common-const';
import { AdvanceReceived } from 'src/app/model/advance-received.model';
import { AdvanceReceivedResult } from 'src/app/model/advance-received-result.model';
import { ColumnNameSetting } from 'src/app/model/column-name-setting.model';
import { EbDataHelper } from 'src/app/common/util/eb-data-helper';
import { NavigationEnd, Router, ActivatedRoute, ParamMap } from '@angular/router';
import { PageUtil } from 'src/app/common/util/page-util';
import { ProcessResultService } from 'src/app/service/common/process-result.service';
import { MSG_WNG, MSG_ERR, MSG_ITEM_NUM, MSG_INF } from 'src/app/common/const/message.const';
import { LocalStorageItem } from 'src/app/model/custom-model/local-storage-item';
import { RangeSearchKey } from 'src/app/common/const/local-storage-key-const';
import { LocalStorageManageService } from 'src/app/service/common/local-storage-manage.service';
import { Receipt } from 'src/app/model/receipt.model';
import { ModalRouterProgressComponent } from 'src/app/component/modal/modal-router-progress/modal-router-progress.component';
import { MatAutocompleteTrigger, MatExpansionPanel } from '@angular/material';
import { ReceiptHelper } from 'src/app/model/helper/receipt-helper.model';
import { ModalMemoComponent } from 'src/app/component/modal/modal-memo/modal-memo.component';
import { ReceiptMemo } from 'src/app/model/receipt-memo.model';

@Component({
  selector: 'app-pd0601-receipt-advance-received',
  templateUrl: './pd0601-receipt-advance-received.component.html',
  styleUrls: ['./pd0601-receipt-advance-received.component.css'],
  providers: [
    { provide: NgbDateParserFormatter, useClass: NgbDateCustomParserFormatter }
  ]

})
export class Pd0601ReceiptAdvanceReceivedComponent extends BaseComponent implements OnInit,AfterViewInit {

  public MATCHING_ASSIGNMENT_FLAG_DICTIONARY: typeof MATCHING_ASSIGNMENT_FLAG_DICTIONARY = MATCHING_ASSIGNMENT_FLAG_DICTIONARY;
  public RECEIPT_STATUS_MESSAGE_DICTIONARY: typeof RECEIPT_STATUS_MESSAGE_DICTIONARY = RECEIPT_STATUS_MESSAGE_DICTIONARY;

  // 検索パネルの開閉フラグ
  public panelOpenState: boolean;

  public juridicalPersonalitiesResult: JuridicalPersonalitysResult;
  public columnNameSettingsResult: ColumnNameSettingsResult;
  public receiptsResult: ReceiptsResult;

  // これをしないとEnum型がテンプレートで見えなかったため
  public TABLE_INDEX: typeof TABLE_INDEX = TABLE_INDEX;

  public cbxCheckAllCtrl:FormControl; // 全選択・全解除

  public recordedAtFromCtrl: FormControl;  // 入金日
  public recordedAtToCtrl: FormControl;

  public payerNameCtrl: FormControl; // 振込依頼人名

  public receiptCategoryCodeFromCtrl: FormControl; // 入金区分
  public receiptCategoryNameFromCtrl: FormControl;
  public receiptCategoryCodeToCtrl: FormControl;
  public receiptCategoryNameToCtrl: FormControl;
  public cbxReceiptCategoryCtrl: FormControl;

  public cbxFullAssignmentFlagCtrl: FormControl; // 消込区分
  public cbxPartAssignmentFlagCtrl: FormControl;
  public cbxNoAssignmentFlagCtrl: FormControl;

  public cbxMemoCtrl: FormControl; // メモ
  public receiptMemoCtrl: FormControl;

  public cbxAdvanceReceiptCtrl: FormControl;  // 前受のデータのみ表示

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

  public cbxUseReceiptSectionCtrl: FormControl;  // 入金部門対応マスターを使用

  public currencyCodeCtrl: FormControl;  // 通貨コード

  public chkAdvanceReceivedCtrls: Array<FormControl>  // チェック
  public detailCustomerCodeCtrls: Array<FormControl>  // 顧客コード
  public detailCustomerIdCtrls: FormControl[];
  public detailCustomerNameCtrls: Array<FormControl>  // 顧客名

  public showAdvanceReceiptFlag: boolean = false; //  前受のデータ表示フラグ

  public dispSumCount: number;
  public dispSumReceiptAmount: number;
  public dispSumRemainAmount: number;  

  public undefineCtrl: FormControl; // 未定用

  @ViewChild('receiptCategoryCodeFromInput', { read: MatAutocompleteTrigger }) receiptCategoryCodeFromTrigger: MatAutocompleteTrigger;
  @ViewChild('receiptCategoryCodeToInput', { read: MatAutocompleteTrigger }) receiptCategoryCodeToTrigger: MatAutocompleteTrigger;
  @ViewChild('customerCodeFromInput', { read: MatAutocompleteTrigger }) customerCodeFromTrigger: MatAutocompleteTrigger;
  @ViewChild('customerCodeToInput', { read: MatAutocompleteTrigger }) customerCodeToTrigger: MatAutocompleteTrigger;
  @ViewChild('sectionCodeFromInput', { read: MatAutocompleteTrigger }) sectionCodeFromTrigger: MatAutocompleteTrigger;
  @ViewChild('sectionCodeToInput', { read: MatAutocompleteTrigger }) sectionCodeToTrigger: MatAutocompleteTrigger;

  @ViewChild('panel') panel:MatExpansionPanel;


  constructor(
    public router: Router,
    public activatedRoute: ActivatedRoute,
    public elementRef: ElementRef,
    public componentFactoryResolver: ComponentFactoryResolver,
    public viewContainerRef: ViewContainerRef,
    public userInfoService: UserInfoService,
    public receiptService: ReceiptService,
    public categoryService: CategoryMasterService,
    public customerService: CustomerMasterService,
    public sectionService: SectionMasterService,
    public currencyService: CurrencyMasterService,
    public juridicalPersonalityService: JuridicalPersonalityMasterService,
    public columnNameSettingService: ColumnNameSettingMasterService,
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

    this.activatedRoute.paramMap.subscribe((params: ParamMap) => {
      // ルートパラメータと同じ要領で Matrix URI パラメータが取得できる
      let paramProecess = params.get("process");
    
      if (!StringUtil.IsNullOrEmpty(paramProecess) && paramProecess == "back") {
        this.showAdvanceReceiptFlag = true;
        // 検索条件の設定
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

    let juridicalPersonalityResponse =
      this.juridicalPersonalityService.GetItems();

    let columnNameSettingResponse =
      this.columnNameSettingService.Get(FreeImporterFormatType.Receipt)

    this.juridicalPersonalitiesResult = new JuridicalPersonalitysResult();
    this.columnNameSettingsResult = new ColumnNameSettingsResult();
    this.columnNameSettingsResult.columnNames = new Array<ColumnNameSetting>();

    // ３つの処理の待機
    forkJoin(
      juridicalPersonalityResponse,
      columnNameSettingResponse
    )
      .subscribe(responseList => {

        if (responseList.length != 2
          || responseList[0] == PROCESS_RESULT_RESULT_TYPE.FAILURE
          || responseList[1] == PROCESS_RESULT_RESULT_TYPE.FAILURE
        ) {
        }
        else {
          this.juridicalPersonalitiesResult.juridicalPersonalities = responseList[0];
          this.columnNameSettingsResult.columnNames = responseList[1];
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

    this.cbxCheckAllCtrl = <FormControl>this.MyFormGroup.controls['cbxCheckAllCtrl'];  // 全選択・全解除

    this.recordedAtFromCtrl = <FormControl>this.MyFormGroup.controls['recordedAtFromCtrl'];  // 入金日
    this.recordedAtToCtrl = <FormControl>this.MyFormGroup.controls['recordedAtToCtrl'];
    this.payerNameCtrl = <FormControl>this.MyFormGroup.controls['payerNameCtrl']; // 振込依頼人名
    this.receiptCategoryCodeFromCtrl = <FormControl>this.MyFormGroup.controls['receiptCategoryCodeFromCtrl']; // 入金区分
    this.receiptCategoryNameFromCtrl = <FormControl>this.MyFormGroup.controls['receiptCategoryNameFromCtrl'];
    this.receiptCategoryCodeToCtrl = <FormControl>this.MyFormGroup.controls['receiptCategoryCodeToCtrl'];
    this.receiptCategoryNameToCtrl = <FormControl>this.MyFormGroup.controls['receiptCategoryNameToCtrl'];
    this.cbxReceiptCategoryCtrl = <FormControl>this.MyFormGroup.controls['cbxReceiptCategoryCtrl'];
    this.cbxFullAssignmentFlagCtrl = <FormControl>this.MyFormGroup.controls['cbxFullAssignmentFlagCtrl']; // 消込区分
    this.cbxPartAssignmentFlagCtrl = <FormControl>this.MyFormGroup.controls['cbxPartAssignmentFlagCtrl'];
    this.cbxNoAssignmentFlagCtrl = <FormControl>this.MyFormGroup.controls['cbxNoAssignmentFlagCtrl'];
    this.cbxMemoCtrl = <FormControl>this.MyFormGroup.controls['cbxMemoCtrl']; // メモ
    this.receiptMemoCtrl = <FormControl>this.MyFormGroup.controls['receiptMemoCtrl'];
    this.cbxAdvanceReceiptCtrl = <FormControl>this.MyFormGroup.controls['cbxAdvanceReceiptCtrl']; // 前受のデータのみ表示
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
    this.cbxUseReceiptSectionCtrl = <FormControl>this.MyFormGroup.controls['cbxUseReceiptSectionCtrl'];  // 入金部門対応マスターを使用
    this.currencyCodeCtrl = <FormControl>this.MyFormGroup.controls['currencyCodeCtrl'];  // 通貨コード
    this.undefineCtrl = <FormControl>this.MyFormGroup.controls['undefineCtrl']; // 未定用;    

  }
  
  public setControlInit() {

    
    this.cbxCheckAllCtrl = new FormControl(null);  // 全選択・全解除

    this.recordedAtFromCtrl = new FormControl("", [Validators.maxLength(10)]);  // 入金日
    this.recordedAtToCtrl = new FormControl("", [Validators.maxLength(10)]);

    this.payerNameCtrl = new FormControl("", [Validators.maxLength(140)]); // 振込依頼人名

    this.receiptCategoryCodeFromCtrl = new FormControl("", [Validators.maxLength(2)]); // 入金区分
    this.receiptCategoryNameFromCtrl = new FormControl("");
    this.receiptCategoryCodeToCtrl = new FormControl("", [Validators.maxLength(2)]);
    this.receiptCategoryNameToCtrl = new FormControl("");
    this.cbxReceiptCategoryCtrl = new FormControl("");

    this.cbxFullAssignmentFlagCtrl = new FormControl(""); // 消込区分
    this.cbxPartAssignmentFlagCtrl = new FormControl("");
    this.cbxNoAssignmentFlagCtrl = new FormControl("");

    this.cbxMemoCtrl = new FormControl(""); // メモ
    this.receiptMemoCtrl = new FormControl("", [Validators.maxLength(100)]);

    this.cbxAdvanceReceiptCtrl = new FormControl(""); // 前受のデータのみ表示

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

    this.cbxUseReceiptSectionCtrl = new FormControl("");  // 入金部門対応マスターを使用

    this.currencyCodeCtrl = new FormControl("", [Validators.maxLength(3)]);  // 通貨コード

    this.undefineCtrl = new FormControl(""); // 未定用;


  }

  public setValidator() {
    this.MyFormGroup = new FormGroup({

      cbxCheckAllCtrl:this.cbxCheckAllCtrl, // 全選択・全解除

      recordedAtFromCtrl: this.recordedAtFromCtrl,  // 入金日
      recordedAtToCtrl: this.recordedAtToCtrl,

      payerNameCtrl: this.payerNameCtrl, // 振込依頼人名

      receiptCategoryCodeFromCtrl: this.receiptCategoryCodeFromCtrl, // 入金区分
      receiptCategoryNameFromCtrl: this.receiptCategoryNameFromCtrl,
      receiptCategoryCodeToCtrl: this.receiptCategoryCodeToCtrl,
      receiptCategoryNameToCtrl: this.receiptCategoryNameToCtrl,
      cbxReceiptCategoryCtrl: this.cbxReceiptCategoryCtrl,

      cbxFullAssignmentFlagCtrl: this.cbxFullAssignmentFlagCtrl, // 消込区分
      cbxPartAssignmentFlagCtrl: this.cbxPartAssignmentFlagCtrl,
      cbxNoAssignmentFlagCtrl: this.cbxNoAssignmentFlagCtrl,

      cbxMemoCtrl: this.cbxMemoCtrl, // メモ
      receiptMemoCtrl: this.receiptMemoCtrl,

      cbxAdvanceReceiptCtrl: this.cbxAdvanceReceiptCtrl, // 前受のデータのみ表示

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

      cbxUseReceiptSectionCtrl: this.cbxUseReceiptSectionCtrl,  // 入金部門対応マスターを使用

      currencyCodeCtrl: this.currencyCodeCtrl,  // 通貨コード

      undefineCtrl: this.undefineCtrl, // 未定用

    });


  }

  public setFormatter() {

    FormatterUtil.setNumberFormatter(this.receiptCategoryCodeFromCtrl); // 入金区分
    FormatterUtil.setNumberFormatter(this.receiptCategoryCodeToCtrl);

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

    FormatterUtil.setAlphabetFormatter(this.currencyCodeCtrl); // 通貨コード

  }

  public setAutoComplete(){

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
    if (this.receiptCategoryCodeFromTrigger!= undefined){this.receiptCategoryCodeFromTrigger.closePanel();}
    if (this.receiptCategoryCodeToTrigger!= undefined){this.receiptCategoryCodeToTrigger.closePanel();}
    if (this.customerCodeFromTrigger!= undefined){this.customerCodeFromTrigger.closePanel();}
    if (this.customerCodeToTrigger!= undefined){this.customerCodeToTrigger.closePanel();}
    if (this.sectionCodeFromTrigger!= undefined){this.sectionCodeFromTrigger.closePanel();}
    if (this.sectionCodeToTrigger!= undefined){this.sectionCodeToTrigger.closePanel();}
  }

  public openMasterModal(table: TABLE_INDEX,  type: string, index: number = -1) {

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
              if (index < 0) {
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
              }
              else {
                this.detailCustomerCodeCtrls[index].setValue(componentRef.instance.SelectedCode);
                this.detailCustomerIdCtrls[index].setValue(componentRef.instance.SelectedId);
                this.detailCustomerNameCtrls[index].setValue(componentRef.instance.SelectedName);
              }
              break;
            }
        }
      }

      componentRef.destroy();
    });
}

  public columnNameSetting(tableName: string, columnName: string): string {

    for (let index = 0; index < this.columnNameSettingsResult.columnNames.length; index++) {
      if (
        this.columnNameSettingsResult.columnNames[index].tableName == tableName
        && this.columnNameSettingsResult.columnNames[index].columnName == columnName
      ) {
        if (!StringUtil.IsNullOrEmpty(this.columnNameSettingsResult.columnNames[index].alias)) {
          return this.columnNameSettingsResult.columnNames[index].alias;
        }
        else {
          return this.columnNameSettingsResult.columnNames[index].originalName;
        }
      }
    }
  }

  public clear() {

    this.panelOpenState = true;
    this.panel.open();

    this.MyFormGroup.reset();


    this.cbxAdvanceReceiptCtrl.enable();

    //this.receiptMemoCtrl.disable();

    this.showAdvanceReceiptFlag = false;

    this.cbxFullAssignmentFlagCtrl.setValue(""); // 消込区分
    this.cbxFullAssignmentFlagCtrl.disable();
    this.cbxPartAssignmentFlagCtrl.setValue(true);
    this.cbxNoAssignmentFlagCtrl.setValue(true);

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
    let cbxReceiptCategory = this.localStorageManageService.get(RangeSearchKey.PD0601_RECEIPT_CATEGORY);
    let cbxCustomer = this.localStorageManageService.get(RangeSearchKey.PD0601_CUSTOMER);
    let cbxSection = this.localStorageManageService.get(RangeSearchKey.PD0601_RECEIPT_SECTION);
    let cbxUseReceiptSection = this.localStorageManageService.get(RangeSearchKey.PD0601_USE_RECEIPT_SECTION);

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
      default:
        console.log('buttonAction Error.');
        break;
    }
    
  }    


  public search() {

    if (!this.validInput()) return;

    let componentFactory = this.componentFactoryResolver.resolveComponentFactory(ModalRouterProgressComponent);
    let modalRouterProgressComponentRef = this.viewContainerRef.createComponent(componentFactory);
    //modalRouterProgressComponentRef.destroy();

    let receiptSearch = this.getReceiptSearch();

    this.receiptService.GetItems(receiptSearch)
      .subscribe(response => {

        this.processCustomResult = this.processResultService.processAtGetData(
          this.processCustomResult, response, true, this.partsResultMessageComponent);

        if (response != PROCESS_RESULT_RESULT_TYPE.FAILURE ) {

          if (response.length > 0) {

            this.receiptsResult = new ReceiptsResult();
            this.receiptsResult.receipts = response;
            this.chkAdvanceReceivedCtrls = new Array(this.receiptsResult.receipts.length);
            this.detailCustomerCodeCtrls = new Array(this.receiptsResult.receipts.length);
            this.detailCustomerIdCtrls = new Array(this.receiptsResult.receipts.length);
            this.detailCustomerNameCtrls = new Array(this.receiptsResult.receipts.length);

            for (let i = 0; i < this.chkAdvanceReceivedCtrls.length; i++) {
              this.chkAdvanceReceivedCtrls[i] = new FormControl();
              this.detailCustomerCodeCtrls[i] = new FormControl(this.receiptsResult.receipts[i].customerCode);
              this.detailCustomerIdCtrls[i] = new FormControl(this.receiptsResult.receipts[i].customerId);
              this.detailCustomerNameCtrls[i] = new FormControl(this.receiptsResult.receipts[i].customerName);

              if (this.receiptsResult.receipts[i].receiptStatusFlag != RECEIPT_STATUS_MESSAGE_DICTIONARY[0].id) {
                this.chkAdvanceReceivedCtrls[i].disable();
              }

              this.detailCustomerCodeCtrls[i].disable();
              if (this.userInfoService.ApplicationControl.customerCodeType == CODE_TYPE.NUMBER) {
                FormatterUtil.setNumberFormatter(this.detailCustomerCodeCtrls[i]);
              }
              else if (this.userInfoService.ApplicationControl.customerCodeType == CODE_TYPE.HANKANA) {
                FormatterUtil.setCustomerCodeKanaFormatter(this.detailCustomerCodeCtrls[i]);
              }
              else {
                FormatterUtil.setCustomerCodeAlphaFormatter(this.detailCustomerCodeCtrls[i]);
              }
  
              this.MyFormGroup.removeControl("chkAdvanceReceivedCtrl" + i);
              this.MyFormGroup.removeControl("detailCustomerCodeCtrl" + i);
              this.MyFormGroup.removeControl("detailCustomerIdCtrl" + i);
              this.MyFormGroup.removeControl("detailCustomerNameCtrl" + i);

              this.MyFormGroup.addControl("chkAdvanceReceivedCtrl" + i, this.chkAdvanceReceivedCtrls[i]);
              this.MyFormGroup.addControl("detailCustomerCodeCtrl" + i, this.detailCustomerCodeCtrls[i]);
              this.MyFormGroup.addControl("detailCustomerIdCtrl" + i, this.detailCustomerIdCtrls[i]);
              this.MyFormGroup.addControl("detailCustomerNameCtrl" + i, this.detailCustomerNameCtrls[i]);

            }
            // 
            this.cbxAdvanceReceiptCtrl.disable();

            this.setDispFooterTotalSum();

            // 戻る対応のための検索結果を退避
            this.receiptService.ReceiptSearchFormGroup = this.MyFormGroup;

            this.panelOpenState = false;
          }
          else{
            this.panelOpenState=true;
          }
        }
        else {
          this.panelOpenState=true;
        }

        modalRouterProgressComponentRef.destroy();
      });

  }

  public validInput(): boolean {

    if (this.cbxFullAssignmentFlagCtrl.value == false &&
        this.cbxPartAssignmentFlagCtrl.value == false &&
        this.cbxNoAssignmentFlagCtrl.value == false ) {
          this.processCustomResult = this.processResultService.processAtWarning(
          this.processCustomResult, MSG_WNG.SELECTION_REQUIRED.replace(MSG_ITEM_NUM.FIRST, '消込区分'),
            this.partsResultMessageComponent);
          HtmlUtil.nextFocusByName(this.elementRef, 'cbxPartAssignmentFlagCtrl', EVENT_TYPE.NONE);          
      return false;
    }

    return true;
  }

  public getReceiptSearch(): ReceiptSearch {

    let receiptSearch = new ReceiptSearch();
    receiptSearch.companyId = this.userInfoService.Company.id;
    receiptSearch.recordedAtFrom = DateUtil.ConvertFromDatepicker(this.recordedAtFromCtrl);
    receiptSearch.recordedAtTo = DateUtil.ConvertFromDatepicker(this.recordedAtToCtrl);

    receiptSearch.payerName = this.payerNameCtrl.value;  // 振込依頼人名

    receiptSearch.receiptCategoryCodeFrom = this.receiptCategoryCodeFromCtrl.value;  // 入金区分
    receiptSearch.receiptCategoryCodeTo = this.receiptCategoryCodeToCtrl.value;

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

    receiptSearch.customerCodeFrom = this.customerCodeFromCtrl.value;  // 得意先コード
    receiptSearch.customerCodeTo = this.customerCodeToCtrl.value;

    receiptSearch.sectionCodeFrom = this.sectionCodeFromCtrl.value;  // 得意先コード
    receiptSearch.sectionCodeTo = this.sectionCodeToCtrl.value;

    // メモ
    if (this.cbxMemoCtrl.value) {
      receiptSearch.existsMemo = 1;
    }
    else {
      receiptSearch.existsMemo = 0;
    }
    receiptSearch.receiptMemo = this.receiptMemoCtrl.value;

    if (this.cbxAdvanceReceiptCtrl.value) {
      receiptSearch.advanceReceivedFlg = 1;
    }
    else {
      receiptSearch.advanceReceivedFlg = 0;
    }

    return receiptSearch;
  }

  public update() {

    if (!this.ValidateInputValue()) return;

    var isCancel: boolean = this.cbxAdvanceReceiptCtrl.value;


    let updateItems = new Array<AdvanceReceived>();


    for (let index = 0; index < this.receiptsResult.receipts.length; index++) {
      if (this.chkAdvanceReceivedCtrls[index].value) {
        let advanceReceived = new AdvanceReceived();
        advanceReceived.receiptId = isCancel ? this.receiptsResult.receipts[index].id : 0;
        advanceReceived.originalReceiptId = isCancel ? this.receiptsResult.receipts[index].originalReceiptId : this.receiptsResult.receipts[index].id;
        advanceReceived.customerId = this.detailCustomerIdCtrls[index].value;
        advanceReceived.originalUpdateAt = this.receiptsResult.receipts[index].updateAt;
        advanceReceived.receiptCategoryId = this.receiptsResult.receipts[index].receiptCategoryId;
        advanceReceived.companyId = this.userInfoService.Company.id;
        advanceReceived.loginUserId = this.userInfoService.LoginUser.id;

        updateItems.push(advanceReceived);
      }
    }

    let AdvanceReceiveresponse: any;
    if (isCancel) {
      AdvanceReceiveresponse = this.receiptService.CancelAdvanceReceived(updateItems)
    }
    else {
      AdvanceReceiveresponse = this.receiptService.SaveAdvanceReceived(updateItems)
    }

    let componentFactory = this.componentFactoryResolver.resolveComponentFactory(ModalRouterProgressComponent);
    let modalRouterProgressComponentRef = this.viewContainerRef.createComponent(componentFactory);
    //modalRouterProgressComponentRef.destroy();

    let advanceReceivedResult = new AdvanceReceivedResult();
    forkJoin(AdvanceReceiveresponse)
      .subscribe(response=>{
        if (response[0] == PROCESS_RESULT_RESULT_TYPE.FAILURE) {        
          this.processResultService.processAtSuccess(
            this.processCustomResult, MSG_INF.PROCESS_FINISH, this.partsResultMessageComponent);
          this.processResultService.createdLog(this.processCustomResult.logData);
        }
        else{
          this.processResultService.processAtFailure(
            this.processCustomResult, MSG_ERR.UPDATE_ERROR, this.partsResultMessageComponent);
        }
        modalRouterProgressComponentRef.destroy();
        this.search();
      });

  }

  public ValidateInputValue() {

    let allEmpty: boolean = true;
    // チェックの数を確認
    this.chkAdvanceReceivedCtrls.forEach(element => {
      if (element.value) {
        allEmpty = false;
      }
    });
    if (allEmpty) {
      this.processCustomResult = this.processResultService.processAtWarning(
        this.processCustomResult, MSG_WNG.SELECTION_REQUIRED.replace(MSG_ITEM_NUM.FIRST, '更新するデータ'),
        this.partsResultMessageComponent);
      return false;
    }

    for (let index = 0; index < this.receiptsResult.receipts.length; index++) {
      if (
        this.chkAdvanceReceivedCtrls[index].value
        && (StringUtil.IsNullOrEmpty(this.detailCustomerCodeCtrls[index].value))
      ) {
        this.processCustomResult = this.processResultService.processAtWarning(
          this.processCustomResult, MSG_WNG.SELECTION_REQUIRED.replace(MSG_ITEM_NUM.FIRST, '得意先コード'),
          this.partsResultMessageComponent);
        return false;
      }
    }


    return true;
    // 締め処理のチェックを入れる

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

  public selectLine(lineNo: number) {
    //this.router.navigate(['main/PC0201']);
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
    HtmlUtil.nextFocusByName(this.elementRef, 'receiptCategoryCodeFromCtrl', eventType);
  }

  public inputPayerName() {
    let value = this.payerNameCtrl.value;
    value = EbDataHelper.convertToValidEbkana(value);
    value = EbDataHelper.removePersonalities(value, this.juridicalPersonalitiesResult.juridicalPersonalities);
    this.payerNameCtrl.setValue(value);
  }


  ///////////////////////////////////////////////////////////

  public setReceiptCategoryCodeFrom(eventType: string) {

    if(eventType!=EVENT_TYPE.BLUR){
      this.closeAutoCompletePanel();
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
      this.closeAutoCompletePanel();
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
          HtmlUtil.nextFocusByNames(this.elementRef, ['cbxFullAssignmentFlagCtrl', 'cbxPartAssignmentFlagCtrl'], eventType);
        });

    }
    else {
      this.receiptCategoryCodeToCtrl.setValue("");
      this.receiptCategoryNameToCtrl.setValue("");
      HtmlUtil.nextFocusByNames(this.elementRef, ['cbxFullAssignmentFlagCtrl', 'cbxPartAssignmentFlagCtrl'], eventType);
    }

  }

  public setCbxReceiptCategory(eventType: string) {
    let localstorageItem = new LocalStorageItem();
    localstorageItem.key = RangeSearchKey.PD0601_RECEIPT_CATEGORY;
    localstorageItem.value = this.cbxReceiptCategoryCtrl.value;
    this.localStorageManageService.set(localstorageItem);

    HtmlUtil.nextFocusByName(this.elementRef, "receiptCategoryCodeFromCtrl", eventType);
  }

  ///////////////////////////////////////////////////////////
  public setCbxMemo(eventType: string) {
    // if (this.cbxMemoCtrl.value === true) {
    //   this.receiptMemoCtrl.enable();
    // }
    // else {
    //   this.receiptMemoCtrl.disable();
    // }
  }

  public setReceiptMemo(eventType: string) {
    HtmlUtil.nextFocusByName(this.elementRef, 'customerCodeFromCtrl', eventType);
  }

  ///////////////////////////////////////////////////////////
  public setCbxAdvanceReceipt(eventType: string) {
    if (this.cbxAdvanceReceiptCtrl.value === true) {
      this.showAdvanceReceiptFlag = true;
      this.cbxFullAssignmentFlagCtrl.enable();      
    }
    else {
      this.showAdvanceReceiptFlag = false;
      this.cbxFullAssignmentFlagCtrl.setValue("");
      this.cbxFullAssignmentFlagCtrl.disable();
    }
  }

  ///////////////////////////////////////////////////////////

  public setCustomerCodeFrom(eventType: string) {

    if(eventType!=EVENT_TYPE.BLUR){
      this.closeAutoCompletePanel();
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

    if(eventType!=EVENT_TYPE.BLUR){
      this.closeAutoCompletePanel();
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

            HtmlUtil.nextFocusByName(this.elementRef, 'sectionCodeFromCtrl', eventType);
          }
          else {
            //this.customerCodeToCtrl.setValue("");
            this.customerNameToCtrl.setValue("");
            HtmlUtil.nextFocusByName(this.elementRef, 'sectionCodeFromCtrl', eventType);
          }
        });
    }
    else {
      this.customerCodeToCtrl.setValue("");
      this.customerNameToCtrl.setValue("");
      HtmlUtil.nextFocusByName(this.elementRef, 'sectionCodeFromCtrl', eventType);
    }
  }

  public setCbxCustomer(eventType: string) {
    let localstorageItem = new LocalStorageItem();
    localstorageItem.key = RangeSearchKey.PD0601_CUSTOMER;
    localstorageItem.value = this.cbxCustomerCtrl.value;
    this.localStorageManageService.set(localstorageItem);

    HtmlUtil.nextFocusByName(this.elementRef, "customerCodeFromCtrl", eventType);
  }

  ///////////////////////////////////////////////////////////

  public setSectionCodeFrom(eventType: string) {

    if(eventType!=EVENT_TYPE.BLUR){
      this.closeAutoCompletePanel();
    }

    if (!StringUtil.IsNullOrEmpty(this.sectionCodeFromCtrl.value)) {

      if (this.userInfoService.ApplicationControl.sectionCodeType == CODE_TYPE.NUMBER) {
        this.sectionCodeFromCtrl.setValue(StringUtil.setPaddingFrontZero(this.sectionCodeFromCtrl.value, this.userInfoService.ApplicationControl.sectionCodeLength));
      }      

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
            HtmlUtil.nextFocusByName(this.elementRef, 'sectionCodeToCtrl', eventType);
          }
          else {
            //this.sectionCodeFromCtrl.setValue("");
            this.sectionNameFromCtrl.setValue("");
            if (this.cbxSectionCtrl.value == true) {
              this.sectionCodeToCtrl.setValue(this.sectionCodeFromCtrl.value);
              this.sectionNameToCtrl.setValue("");
            }            
            HtmlUtil.nextFocusByName(this.elementRef, 'sectionCodeToCtrl', eventType);
          }
        });
    }
    else {
      this.sectionCodeFromCtrl.setValue("");
      this.customerNameFromCtrl.setValue("");
      HtmlUtil.nextFocusByName(this.elementRef, 'sectionCodeToCtrl', eventType);
    }

  }


  public setSectionCodeTo(eventType: string) {

    if(eventType!=EVENT_TYPE.BLUR){
      this.closeAutoCompletePanel();
    }

    if (!StringUtil.IsNullOrEmpty(this.sectionCodeToCtrl.value)) {

      if (this.userInfoService.ApplicationControl.sectionCodeType == CODE_TYPE.NUMBER) {
        this.sectionCodeToCtrl.setValue(StringUtil.setPaddingFrontZero(this.sectionCodeToCtrl.value, this.userInfoService.ApplicationControl.sectionCodeLength));
      }      

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
          HtmlUtil.nextFocusByNames(this.elementRef, ['currencyCodeCtrl', 'recordedAtFromCtrl'], eventType);
        });
    }
    else {
      this.sectionCodeToCtrl.setValue("");
      this.sectionNameToCtrl.setValue("");
      HtmlUtil.nextFocusByNames(this.elementRef, ['currencyCodeCtrl', 'recordedAtFromCtrl'], eventType);
    }
  }

  public setCbxSection(eventType: string) {
    let localstorageItem = new LocalStorageItem();
    localstorageItem.key = RangeSearchKey.PD0601_RECEIPT_SECTION;
    localstorageItem.value = this.cbxSectionCtrl.value;
    this.localStorageManageService.set(localstorageItem);

    HtmlUtil.nextFocusByName(this.elementRef, "sectionCodeFromCtrl", eventType);
  }

  ///////////////////////////////////////////////////////////
  public setCbxUseReceiptSection(eventType: string) {
    let localstorageItem = new LocalStorageItem();
    localstorageItem.key = RangeSearchKey.PD0601_USE_RECEIPT_SECTION;
    localstorageItem.value = this.cbxUseReceiptSectionCtrl.value;
    this.localStorageManageService.set(localstorageItem);

    HtmlUtil.nextFocusByName(this.elementRef, "currencyCodeCtrl", eventType);
  }

  

  ///////////////////////////////////////////////////////////
  public setCurrencyCode(eventType: string) {

    if (!StringUtil.IsNullOrEmpty(this.currencyCodeCtrl.value)) {

      this.loadStart();
      this.currencyService.GetItems(this.currencyCodeCtrl.value)
        .subscribe(response => {
          this.loadEnd();
          if (response != PROCESS_RESULT_RESULT_TYPE.FAILURE && response.length > 0) {
            this.currencyCodeCtrl.setValue(response[0].code);
            HtmlUtil.nextFocusByName(this.elementRef, 'recordedAtFromCtrl', eventType);
          }
          else {
            this.currencyCodeCtrl.setValue("");
            HtmlUtil.nextFocusByName(this.elementRef, 'recordedAtFromCtrl', eventType);
          }
        });
    }
    else {
      this.currencyCodeCtrl.setValue("");
      HtmlUtil.nextFocusByName(this.elementRef, 'recordedAtFromCtrl', eventType);
    }
  }

  ///////////////////////////////////////////////////////////
  public setChkAdvanceReceived(index: number) {

    const advanceReceiptData = this.cbxAdvanceReceiptCtrl.value;

    if (!advanceReceiptData) {
      if (this.chkAdvanceReceivedCtrls[index].value === true) {
        this.detailCustomerCodeCtrls[index].enable();
        HtmlUtil.nextFocusByName(this.elementRef, 'detailCustomerCodeCtrl' + index, EVENT_TYPE.NONE);
      }
      else {
        this.detailCustomerCodeCtrls[index].disable();
      }  
    }
    else {
      let originalReceiptId = this.receiptsResult.receipts[index].originalReceiptId;
      let setFlag: string;
  
      if (this.chkAdvanceReceivedCtrls[index].value === true) {
        setFlag = "true";
      }
      else {
        setFlag = null;
      }
  
      for (let i = 0; i < this.receiptsResult.receipts.length; i++) {
        if (this.receiptsResult.receipts[i].originalReceiptId == originalReceiptId) {
          this.chkAdvanceReceivedCtrls[i].setValue(setFlag);
        }
      }
    }

  }

  ///////////////////////////////////////////////////////////
  public setDetailCustomerCode(eventType: string, index: number) {
    if (!StringUtil.IsNullOrEmpty(this.detailCustomerCodeCtrls[index].value)) {

      if (this.userInfoService.ApplicationControl.customerCodeType == CODE_TYPE.NUMBER) {
        this.detailCustomerCodeCtrls[index].setValue(StringUtil.setPaddingFrontZero(this.detailCustomerCodeCtrls[index].value, this.userInfoService.ApplicationControl.customerCodeLength));
      }      

      this.loadStart();
      this.customerService.GetItems(this.detailCustomerCodeCtrls[index].value)
        .subscribe(response => {
          this.loadEnd();
          if (response != PROCESS_RESULT_RESULT_TYPE.FAILURE && response.length > 0) {
            this.detailCustomerCodeCtrls[index].setValue(response[0].code);
            this.detailCustomerIdCtrls[index].setValue(response[0].id);
            this.detailCustomerNameCtrls[index].setValue(response[0].name);

          }
          else {
            this.detailCustomerCodeCtrls[index].setValue("");
            this.detailCustomerIdCtrls[index].setValue("");
            this.detailCustomerNameCtrls[index].setValue("");
          }
        });

    }
    else {
      this.detailCustomerCodeCtrls[index].setValue("");
      this.detailCustomerIdCtrls[index].setValue("");
      this.detailCustomerNameCtrls[index].setValue("");
    }

  }

  public selectAll() {
    for (let index = 0; index < this.receiptsResult.receipts.length; index++) {
      this.chkAdvanceReceivedCtrls[index].setValue("true");
      this.detailCustomerCodeCtrls[index].enable();
    }
  }

  public clearAll() {
    for (let index = 0; index < this.receiptsResult.receipts.length; index++) {
      this.chkAdvanceReceivedCtrls[index].setValue("");
      this.detailCustomerCodeCtrls[index].disable();
    }

  }

  ///////////////////////////////////////////////////////////
  public selectUpdateLine(lineNo: number) {
    
    if (!this.showAdvanceReceiptFlag) return;

    let selectReceipt = this.receiptsResult.receipts[lineNo];

    let msg = null;
    if (selectReceipt.remainAmountFlag > 0) {
      msg = MSG_WNG.ORIGINAL_RECEIPT_REMAIN_AMOUNT_NOT_ZERO;
    }
    else if (selectReceipt.receiptStatusFlag == 1) {
      msg = MSG_WNG.ORIGINAL_RECEIPT_ALREADY_DELETED;
    }

    if (msg != null) {
      this.processCustomResult = this.processResultService.processAtWarning(
        this.processCustomResult, msg, this.partsResultMessageComponent);
      return;
    }

    this.router.navigate(['main/PD0602', { id: selectReceipt.originalReceiptId }]);
  }

  ///////////////////////////////////////////////////////////
  public getAssignmentValue(receipt: Receipt):string {

    let strRtn = "";
    if(receipt.assignmentFlag==0){
      strRtn = '<span class="tag--noAssignment">';
    }
    else if(receipt.assignmentFlag==1){
      strRtn = '<span class="tag--partAssignment">';
    }
    else if(receipt.assignmentFlag==2){
      strRtn = '<span class="tag--fullAssignment">';
    }
    return strRtn + MATCHING_ASSIGNMENT_FLAG_DICTIONARY[receipt.assignmentFlag].val + "</span>";    
  }

  ///////////////////////////////////////////////////////////
  public openReceiptMemoModal(index: number) {

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


  public checkAll() {

    if(this.cbxCheckAllCtrl.value){
      this.selectAll();
    }
    else{
      this.clearAll();
    }

    this.processResultService.clearProcessCustomMsg(this.processCustomResult);
  }  

}
