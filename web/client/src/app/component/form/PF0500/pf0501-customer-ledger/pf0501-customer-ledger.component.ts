import { Component, OnInit, ElementRef, ComponentFactoryResolver, ViewContainerRef, ViewChild, AfterViewInit } from '@angular/core';
import { BaseComponent } from 'src/app/component/common/base/base-component';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { FormatterUtil } from 'src/app/common/util/formatter.util';
import { Router, ActivatedRoute, NavigationEnd } from '@angular/router';
import { TABLE_INDEX } from 'src/app/common/const/table-name.const';
import { KEY_CODE, EVENT_TYPE, BUTTON_ACTION } from 'src/app/common/const/event.const';
import { ModalMasterComponent } from 'src/app/component/modal/modal-master/modal-master.component';
import { HtmlUtil } from 'src/app/common/util/html.util';
import { StringUtil } from 'src/app/common/util/string-util';
import { CustomerMasterService } from 'src/app/service/Master/customer-master.service';
import { ModalFormSettingComponent } from 'src/app/component/modal/modal-form-setting/modal-form-setting.component';
import { MODAL_STATUS_TYPE, PROCESS_RESULT_RESULT_TYPE } from 'src/app/common/const/status.const';
import { UserInfoService } from 'src/app/service/common/user-info.service';
import { CurrencyMasterService } from 'src/app/service/Master/currency-master.service';
import { MomentDateAdapter } from '@angular/material-moment-adapter';
import { DateAdapter, MAT_DATE_FORMATS, MAT_DATE_LOCALE } from '@angular/material/core';
import { MatDatepicker } from '@angular/material/datepicker';
import { YMPICKER_FORMAT } from 'src/app/common/const/ympicker-format.const';
import { Moment } from 'moment';
import { COMPONENT_INFO } from 'src/app/common/const/component-name.const';
import { CustomerLedgerSearch } from 'src/app/model/customer-ledger-search.model';
import { ReportSettingMasterService } from 'src/app/service/Master/report-setting-master.service';
import { ReportSetting } from 'src/app/model/report-setting.model';
import { SettingsResult } from 'src/app/model/settings-result.model';
import { ReportSettingsResult } from 'src/app/model/report-settings-result.model';
import { DateUtil, CALENDAR_CONVERT_TYPE } from 'src/app/common/util/date-util';
import { ReportTargetDate, REPORT_ITEM_ID, ReportDoOrNot, ReportUnitPrice, REPORT_HEADER, LINE_FEED_CODE } from 'src/app/common/const/report-setting-kbn.const';
import { ClosingService } from 'src/app/service/closing.service';
import { ClosingInformation } from 'src/app/model/closing-information.model';
import { ReportSettigUtil } from 'src/app/common/util/report-setting-util';
import { PF0501 } from 'src/app/model/report/settings/pf0501';
import { CustomerLedgersResult } from 'src/app/model/customer-ledgers-result.model';
import { CustomerLedgerService } from 'src/app/service/customer-ledger.service';
import { CustomerLedger } from 'src/app/model/customer-ledger.model';
import { CustomerPaymentContractMasterService } from 'src/app/service/Master/customer-payment-contract-master.service';
import { CustomerPaymentContract } from 'src/app/model/customer-payment-contract.model';
import { Customer } from 'src/app/model/customer.model';
import { ModalRouterProgressComponent } from 'src/app/component/modal/modal-router-progress/modal-router-progress.component';
import { FileUtil } from 'src/app/common/util/file.util';
import { FILE_EXTENSION } from 'src/app/common/const/eb-file.const';
import { ProcessResultService } from 'src/app/service/common/process-result.service';
import { MSG_WNG, MSG_ERR, MSG_ITEM_NUM } from 'src/app/common/const/message.const';
import { PageUtil } from 'src/app/common/util/page-util';
import { CODE_TYPE } from 'src/app/common/const/kbn.const';
import { NumberUtil } from 'src/app/common/util/number-util';
import { LocalStorageItem } from 'src/app/model/custom-model/local-storage-item';
import { RangeSearchKey } from 'src/app/common/const/local-storage-key-const';
import { LocalStorageManageService } from 'src/app/service/common/local-storage-manage.service';
import { MatAutocompleteTrigger, MatExpansionPanel } from '@angular/material';


@Component({
  selector: 'app-pf0501-customer-ledger',
  templateUrl: './pf0501-customer-ledger.component.html',
  styleUrls: ['./pf0501-customer-ledger.component.css'],
  providers: [
    { provide: DateAdapter, useClass: MomentDateAdapter, deps: [MAT_DATE_LOCALE] },
    { provide: MAT_DATE_FORMATS, useValue: YMPICKER_FORMAT }
  ]

})
export class Pf0501CustomerLedgerComponent extends BaseComponent implements OnInit,AfterViewInit {

  // パネルの開閉フラグ
  public panelOpenState: boolean;

  public parentCodeList: Array<string> = new Array<string>();
  public currentParentCodeIndex: number;

  public closingInformation: ClosingInformation;
  public reportSettingsResult: ReportSettingsResult;

  public customerLedgerSearch: CustomerLedgerSearch;
  public customerLedgersResult: CustomerLedgersResult;

  public currentCustomerLedgers: Array<CustomerLedger> = new Array<CustomerLedger>();;

  public customerCodeFromCtrl: FormControl;  // 得意先コード
  public customerNameFromCtrl: FormControl;
  public customerCodeToCtrl: FormControl;
  public customerNameToCtrl: FormControl;
  public cbxCustomerCtrl: FormControl;
  public customerNameCtrl: FormControl;

  public baseDateFromCtrl: FormControl; // 処理月
  public baseDateToCtrl: FormControl;

  public tmpBaseDateFromCtrl: Moment; // 処理月
  public tmpBaseDateToCtrl: Moment;

  public rdoRemainTypeCtrl: FormControl;  // 残高タイプ

  public closingDateCtrl: FormControl;  // 締め日

  public currencyCodeCtrl: FormControl; // 通貨コード

  public dispCustomerCodeCtrl: FormControl; //得意先コード
  public dispCustomerNameCtrl: FormControl;

  public dispCategoryNameCtrl: FormControl; //回収区分
  public dispThresholdCtrl: FormControl;
  public dispLessCollectCategoryNameCtrl: FormControl;

  public dispReportTargetDateCtrl: FormControl; // 集計基準日

  public dispReportTotalByDayCtrl: FormControl; // 請求データ集計
  public dispGreaterCollectCategoryName1Ctrl: FormControl;
  public dispGreaterRate1Ctrl: FormControl;
  public dispGreaterRoundingMode1Ctrl: FormControl;

  public dispDispReportTotalCtrl: FormControl;  // 伝票集計方法

  public dispReportDoOrNotCtrl: FormControl;  // 入金データ集計
  public dispGreaterCollectCategoryName2Ctrl: FormControl;
  public dispGreaterRate2Ctrl: FormControl;
  public dispGreaterRoundingMode2Ctrl: FormControl;

  public dispReportUnitPriceCtrl: FormControl;  // 金額集計

  public dispReportAdvancedReceivedTypeCtrl: FormControl; // 請求残計算
  public dispGreaterCollectCategoryName3Ctrl: FormControl;
  public dispGreaterRate3Ctrl: FormControl;
  public dispGreaterRoundingMode3Ctrl: FormControl;

  public undefineCtrl = new FormControl; // 未定用;  

  @ViewChild('customerCodeFromInput', { read: MatAutocompleteTrigger }) customerCodeFromTrigger: MatAutocompleteTrigger;
  @ViewChild('customerCodeToInput', { read: MatAutocompleteTrigger }) customerCodeToTrigger: MatAutocompleteTrigger;

  @ViewChild('panel') panel:MatExpansionPanel;

  constructor(
    public router: Router,
    public activatedRoute: ActivatedRoute,
    public elementRef: ElementRef,
    public componentFactoryResolver: ComponentFactoryResolver,
    public viewContainerRef: ViewContainerRef,
    public userInfoService: UserInfoService,
    public reportSettingService: ReportSettingMasterService,
    public customerService: CustomerMasterService,
    public currencyService: CurrencyMasterService,
    public closingService: ClosingService,
    public customerLedgerService: CustomerLedgerService,
    public customerPaymentContractService: CustomerPaymentContractMasterService,
    public processResultService: ProcessResultService,
    public localStorageManageService:LocalStorageManageService

  ) {
    super();

    router.events.subscribe(event => {
      if (event instanceof NavigationEnd) {
        this.Title = PageUtil.GetTitle(router.routerState, router.routerState.root).join('');
        this.ComponentId = parseInt(PageUtil.GetComponentId(router.routerState, router.routerState.root).join(''));
        const path:string[] = PageUtil.GetPath(this.activatedRoute.snapshot.pathFromRoot);
        this.Path = path[1];
        this.processCustomResult = this.processResultService.processResultInit(this.ComponentId,this.Title);
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

    // API呼出引数
    let reportSetting = new ReportSetting();
    reportSetting.companyId = this.userInfoService.Company.id;
    reportSetting.reportId = COMPONENT_INFO.PF0501.name;

    // API呼出結果
    let settingsResult = new SettingsResult();
    this.reportSettingService.GetItems(reportSetting)
      .subscribe(response => {
        this.reportSettingsResult = new ReportSettingsResult();
        this.reportSettingsResult.reportSettings = response;
      });

    this.closingService.GetClosingInformation(this.userInfoService.Company.id)
      .subscribe(response => {
        this.closingInformation = response;
      });
  }

  ngAfterViewInit(){
    HtmlUtil.nextFocusByName(this.elementRef, 'customerCodeFromCtrl', EVENT_TYPE.NONE);
  }

  public chosenYearHandler(normalizedYear: Moment, datepickerName: string) {
  }

  public chosenMonthHandler(normlizedMonth: Moment, datepicker: MatDatepicker<Moment>, datepickerName: string, eventType: string) {
    if (datepickerName == "baseDateFrom") {
      this.tmpBaseDateFromCtrl = normlizedMonth.clone();
      this.baseDateFromCtrl.setValue(this.tmpBaseDateFromCtrl);
      this.setBaseDateFrom('keyup');
      datepicker.close();
    } else {
      this.tmpBaseDateToCtrl = normlizedMonth.clone();
      this.baseDateToCtrl.setValue(this.tmpBaseDateToCtrl);
      this.setBaseDateTo('keyup')
      datepicker.close();
    }
  }

  public setControlInit() {

    this.customerCodeFromCtrl = new FormControl("", [Validators.required, Validators.maxLength(this.userInfoService.ApplicationControl.customerCodeLength)]);  // 得意先コード
    this.customerNameFromCtrl = new FormControl("");
    this.customerCodeToCtrl = new FormControl("", [Validators.required, Validators.maxLength(this.userInfoService.ApplicationControl.customerCodeLength)]);
    this.customerNameToCtrl = new FormControl("");
    this.cbxCustomerCtrl = new FormControl("");
    this.customerNameCtrl = new FormControl("");

    this.baseDateFromCtrl = new FormControl("", [Validators.required, Validators.maxLength(7)]);  // 処理月
    this.baseDateToCtrl = new FormControl("", [Validators.required, Validators.maxLength(7)]);

    this.rdoRemainTypeCtrl = new FormControl("");  // 残高タイプ

    this.closingDateCtrl = new FormControl("", [Validators.maxLength(2)]);  // 締め日

    this.currencyCodeCtrl = new FormControl("", [Validators.maxLength(3)]); // 通貨コード

    this.dispCustomerCodeCtrl = new FormControl("");  //得意先コード
    this.dispCustomerNameCtrl = new FormControl("");

    this.dispCategoryNameCtrl = new FormControl("");  //回収区分
    this.dispThresholdCtrl = new FormControl("");
    this.dispLessCollectCategoryNameCtrl = new FormControl("");

    this.dispReportTargetDateCtrl = new FormControl("");  //集計基準日

    this.dispReportTotalByDayCtrl = new FormControl("");  //請求データ集計
    this.dispGreaterCollectCategoryName1Ctrl = new FormControl("");
    this.dispGreaterRate1Ctrl = new FormControl("");
    this.dispGreaterRoundingMode1Ctrl = new FormControl("");

    this.dispDispReportTotalCtrl = new FormControl("");  //伝票集計方法

    this.dispReportDoOrNotCtrl = new FormControl("");  //入金データ集計
    this.dispGreaterCollectCategoryName2Ctrl = new FormControl("");
    this.dispGreaterRate2Ctrl = new FormControl("");
    this.dispGreaterRoundingMode2Ctrl = new FormControl("");

    this.dispReportUnitPriceCtrl = new FormControl("");  //金額集計

    this.dispReportAdvancedReceivedTypeCtrl = new FormControl("");  //請求残計算
    this.dispGreaterCollectCategoryName3Ctrl = new FormControl("");
    this.dispGreaterRate3Ctrl = new FormControl("");
    this.dispGreaterRoundingMode3Ctrl = new FormControl("");

    this.undefineCtrl = new FormControl(""); // 未定用

  }

  public setValidator() {
    this.MyFormGroup = new FormGroup({

      customerCodeFromCtrl: this.customerCodeFromCtrl,  // 得意先コード
      customerNameFromCtrl: this.customerNameFromCtrl,
      customerCodeToCtrl: this.customerCodeToCtrl,
      customerNameToCtrl: this.customerNameToCtrl,
      cbxCustomerCtrl: this.cbxCustomerCtrl,
      customerNameCtrl: this.customerNameCtrl,

      baseDateFromCtrl: this.baseDateFromCtrl,  // 処理月
      baseDateToCtrl: this.baseDateToCtrl,

      rdoRemainTypeCtrl: this.rdoRemainTypeCtrl,  // 残高タイプ

      closingDateCtrl: this.closingDateCtrl,  // 締め日

      currencyCodeCtrl: this.currencyCodeCtrl, // 通貨コード;

      dispCustomerCodeCtrl: this.dispCustomerCodeCtrl, //得意先コード
      dispCustomerNameCtrl: this.dispCustomerNameCtrl,

      dispCategoryNameCtrl: this.dispCategoryNameCtrl, //回収区分
      dispThresholdCtrl: this.dispThresholdCtrl,
      dispLessCollectCategoryNameCtrl: this.dispLessCollectCategoryNameCtrl,

      dispReportTargetDateCtrl: this.dispReportTargetDateCtrl, // 集計基準日

      dispReportTotalByDayCtrl: this.dispReportTotalByDayCtrl, // 請求データ集計
      dispGreaterCollectCategoryName1Ctrl: this.dispGreaterCollectCategoryName1Ctrl,
      dispGreaterRate1Ctrl: this.dispGreaterRate1Ctrl,
      dispGreaterRoundingMode1Ctrl: this.dispGreaterRoundingMode1Ctrl,

      dispDispReportTotalCtrl: this.dispDispReportTotalCtrl,  // 伝票集計方法

      dispReportDoOrNotCtrl: this.dispReportDoOrNotCtrl,  // 入金データ集計
      dispGreaterCollectCategoryName2Ctrl: this.dispGreaterCollectCategoryName2Ctrl,
      dispGreaterRate2Ctrl: this.dispGreaterRate2Ctrl,
      dispGreaterRoundingMode2Ctrl: this.dispGreaterRoundingMode2Ctrl,

      dispReportUnitPriceCtrl: this.dispReportUnitPriceCtrl,  // 金額集計

      dispReportAdvancedReceivedTypeCtrl: this.dispReportAdvancedReceivedTypeCtrl, // 請求残計算
      dispGreaterCollectCategoryName3Ctrl: this.dispGreaterCollectCategoryName3Ctrl,
      dispGreaterRate3Ctrl: this.dispGreaterRate3Ctrl,
      dispGreaterRoundingMode3Ctrl: this.dispGreaterRoundingMode3Ctrl,

      undefineCtrl: this.undefineCtrl, // 未定用;


    });


  }

  public setFormatter() {

    if (this.userInfoService.ApplicationControl.customerCodeType == CODE_TYPE.NUMBER) {
      FormatterUtil.setNumberFormatter(this.customerCodeFromCtrl); // 得意先コード
      FormatterUtil.setNumberFormatter(this.customerCodeToCtrl);
    }
    else if (this.userInfoService.ApplicationControl.customerCodeType == CODE_TYPE.HANKANA)  {
      FormatterUtil.setCustomerCodeKanaFormatter(this.customerCodeFromCtrl);
      FormatterUtil.setCustomerCodeKanaFormatter(this.customerCodeToCtrl);
    }
    else {
      FormatterUtil.setCustomerCodeAlphaFormatter(this.customerCodeFromCtrl);
      FormatterUtil.setCustomerCodeAlphaFormatter(this.customerCodeToCtrl);
    }    

    FormatterUtil.setNumberFormatter(this.closingDateCtrl);

    FormatterUtil.setAlphabetFormatter(this.currencyCodeCtrl);
  }

  public setAutoComplete(){

    // 得意先
    this.initAutocompleteCustomers(this.customerCodeFromCtrl,this.customerService,0);
    this.initAutocompleteCustomers(this.customerCodeToCtrl,this.customerService,1);

  }  

  public openFromSettingModal() {
    let componentFactory = this.componentFactoryResolver.resolveComponentFactory(ModalFormSettingComponent);
    let componentRef = this.viewContainerRef.createComponent(componentFactory);

    componentRef.instance.ReportId = COMPONENT_INFO.PF0501.name;
    componentRef.instance.processModalCustomResult = this.processModalCustomResult;

    componentRef.instance.Closing.subscribe(() => {
      componentRef.destroy();
    });

  }


  public openMasterModal(table: TABLE_INDEX, type: string = null) {
    this.customerCodeFromTrigger.closePanel();
    this.customerCodeToTrigger.closePanel();

    let componentFactory = this.componentFactoryResolver.resolveComponentFactory(ModalMasterComponent);
    let componentRef = this.viewContainerRef.createComponent(componentFactory);

    componentRef.instance.TableIndex = table;

    componentRef.instance.Closing.subscribe(() => {

      if (componentRef.instance.ModalStatus == MODAL_STATUS_TYPE.SELECT) {
        switch (table) {
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
              break;
            }
        }
      }

      componentRef.destroy();
    });
  }

  public clear() {

    this.panelOpenState = true;
    this.panel.open();

    this.MyFormGroup.reset();

    this.rdoRemainTypeCtrl.setValue("0");

    this.closingDateCtrl.setValidators([]);
    this.closingDateCtrl.updateValueAndValidity();
    this.closingDateCtrl.disable();


    //this.customerLedgersResult = null;
    //this.currentCustomerLedgers = new Array<CustomerLedger>();

    HtmlUtil.nextFocusByName(this.elementRef, 'customerCodeFromCtrl', EVENT_TYPE.NONE);

    this.setRangeCheckbox();
  }


  public setRangeCheckbox() {
    let cbxCustomer = this.localStorageManageService.get(RangeSearchKey.PF0501_CUSTOMER);

    if (cbxCustomer != null) {
      this.cbxCustomerCtrl.setValue(cbxCustomer.value);
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

      case BUTTON_ACTION.PRINT:
        this.print();
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
   * データ照会
   */
  public search(){
    // API呼出引数
    let reportSetting = new ReportSetting();
    reportSetting.companyId = this.userInfoService.Company.id;
    reportSetting.reportId = COMPONENT_INFO.PF0501.name;

    // API呼出結果
    let settingsResult = new SettingsResult();
    this.reportSettingService.GetItems(reportSetting)
      .subscribe(response => {
        this.reportSettingsResult = new ReportSettingsResult();
        this.reportSettingsResult.reportSettings = response;

        this.searchInner();
      });    
  }

  public searchInner() {
    this.currentParentCodeIndex = 0;
    this.customerLedgerSearch = this.GetSearchOption();

    let componentFactory = this.componentFactoryResolver.resolveComponentFactory(ModalRouterProgressComponent);
    let componentRef = this.viewContainerRef.createComponent(componentFactory);
    // componentRef.destroy();

    this.customerLedgerService.Get(this.customerLedgerSearch)
      .subscribe(response => {
        if (response != PROCESS_RESULT_RESULT_TYPE.FAILURE) {
          this.customerLedgersResult = new CustomerLedgersResult();
          this.customerLedgersResult.customerLedgers = response;

          if (this.customerLedgersResult == null || this.customerLedgersResult.customerLedgers.length == 0) {
            this.panelOpenState = true;
            this.processResultService.processAtWarning(
              this.processCustomResult, MSG_WNG.NOT_EXIST_SEARCH_DATA, this.partsResultMessageComponent);
          }
          else {

            this.panelOpenState = false;

            this.SetSearchOptionInformation();

            let tmpParentCodeSet = new Set<string>();
            this.customerLedgersResult.customerLedgers.forEach(element => tmpParentCodeSet.add(element.parentCustomerCode));

            this.parentCodeList = Array.from(tmpParentCodeSet);

            this.currentParentCodeIndex = 0;
            this.setCurrentCustomerLedgers(this.parentCodeList[this.currentParentCodeIndex]);
            this.processResultService.processAtSuccess(
              this.processCustomResult, "照会が完了しました。", this.partsResultMessageComponent);
            this.processResultService.createdLog(this.processCustomResult.logData);
          }
        }
        else {
          this.panelOpenState = true;
          this.processResultService.processAtFailure(this.processCustomResult,
            MSG_ERR.SOMETHING_ERROR.replace(MSG_ITEM_NUM.FIRST, '得意先別消込台帳の検索'),
            this.partsResultMessageComponent);

        }
        componentRef.destroy();
      })

  }

  public next() {

    if (this.currentParentCodeIndex == this.parentCodeList.length - 1) return;
    this.currentParentCodeIndex++;

    this.setCurrentCustomerLedgers(this.parentCodeList[this.currentParentCodeIndex]);


  }

  public prev() {

    if (this.currentParentCodeIndex == 0) return;

    this.currentParentCodeIndex--;

    this.setCurrentCustomerLedgers(this.parentCodeList[this.currentParentCodeIndex]);

  }

  public setCurrentCustomerLedgers(tmpCurrentParentCode: string) {
    this.currentCustomerLedgers = new Array<CustomerLedger>();

    this.customerLedgersResult.customerLedgers.forEach(element => {
      if (element.parentCustomerCode == tmpCurrentParentCode) {
        this.currentCustomerLedgers.push(element);
      }
    });

    this.BindCusPaymentContractData()
  }


  public BindCusPaymentContractData() {
    this.dispCustomerCodeCtrl.setValue(this.currentCustomerLedgers[0].parentCustomerCode);
    this.dispCustomerNameCtrl.setValue(this.currentCustomerLedgers[0].parentCustomerName);

    let componentFactory = this.componentFactoryResolver.resolveComponentFactory(ModalRouterProgressComponent);
    let componentRef = this.viewContainerRef.createComponent(componentFactory);
    // componentRef.destroy();

    let tmpParentCustomer: Array<number> = new Array<number>();
    tmpParentCustomer.push(this.currentCustomerLedgers[0].parentCustomerId);

    this.customerPaymentContractService.GetItems(tmpParentCustomer)
      .subscribe(response => {
        if (response != PROCESS_RESULT_RESULT_TYPE.FAILURE) {
          let customerPaymentContracts: Array<CustomerPaymentContract> = response;

          if (customerPaymentContracts != null && customerPaymentContracts.length == 1) {

            this.dispCategoryNameCtrl.setValue(customerPaymentContracts[0].collectCategoryName);

            if (!this.userInfoService.ApplicationControl.useForeignCurrency) {
              var threshold = "" + customerPaymentContracts[0].thresholdValue;
              this.dispThresholdCtrl.setValue(
                threshold != "" ? threshold.substring(0, threshold.indexOf(".")) : "0"
              )
              this.dispLessCollectCategoryNameCtrl.setValue(customerPaymentContracts[0].lessThanName);
              this.dispGreaterCollectCategoryName1Ctrl.setValue(customerPaymentContracts[0].greaterThanName1);
              this.dispGreaterCollectCategoryName2Ctrl.setValue(customerPaymentContracts[0].greaterThanName2);
              this.dispGreaterCollectCategoryName3Ctrl.setValue(customerPaymentContracts[0].greaterThanName3);
              this.dispGreaterRate1Ctrl.setValue("" + customerPaymentContracts[0].greaterThanRate1);
              this.dispGreaterRate2Ctrl.setValue("" + customerPaymentContracts[0].greaterThanRate2);
              this.dispGreaterRate3Ctrl.setValue("" + customerPaymentContracts[0].greaterThanRate3);
              this.dispGreaterRoundingMode1Ctrl.setValue(customerPaymentContracts[0].greaterThanRoundingMode1 == 0 ? "端数処理" : "");
              this.dispGreaterRoundingMode2Ctrl.setValue(customerPaymentContracts[0].greaterThanRoundingMode2 == 0 ? "端数処理" : "");
              this.dispGreaterRoundingMode3Ctrl.setValue(customerPaymentContracts[0].greaterThanRoundingMode3 == 0 ? "端数処理" : "");
            }
          }
          else {
            this.customerService.GetItems(this.currentCustomerLedgers[0].parentCustomerCode)
              .subscribe(response => {
                if (response != PROCESS_RESULT_RESULT_TYPE.FAILURE) {
                  let customers: Array<Customer> = response;

                  if (customers != null && customers.length == 1) {
                    this.dispCategoryNameCtrl.setValue(customers[0].collectCategoryName);
                  }
                  else {
                    this.dispCategoryNameCtrl.setValue("");
                  }
                }
                else {
                  this.processResultService.processAtFailure(this.processCustomResult,
                    MSG_ERR.SOMETHING_ERROR.replace(MSG_ITEM_NUM.FIRST, '顧客情報の取得'),
                    this.partsResultMessageComponent);
                }
              });

            this.dispThresholdCtrl.setValue("");
            this.dispLessCollectCategoryNameCtrl.setValue("");
            this.dispGreaterCollectCategoryName1Ctrl.setValue("");
            this.dispGreaterCollectCategoryName2Ctrl.setValue("");
            this.dispGreaterCollectCategoryName3Ctrl.setValue("");
            this.dispGreaterRate1Ctrl.setValue("");
            this.dispGreaterRate2Ctrl.setValue("");
            this.dispGreaterRate3Ctrl.setValue("");
            this.dispGreaterRoundingMode1Ctrl.setValue("");
            this.dispGreaterRoundingMode2Ctrl.setValue("");
            this.dispGreaterRoundingMode3Ctrl.setValue("");

          }
        }
        else {
          this.processResultService.processAtFailure(this.processCustomResult,
            MSG_ERR.SOMETHING_ERROR.replace(MSG_ITEM_NUM.FIRST, '契約情報の取得'),
            this.partsResultMessageComponent);
        }
        componentRef.destroy();
      });

  }


  public print() {
    let componentFactory = this.componentFactoryResolver.resolveComponentFactory(ModalRouterProgressComponent);
    let componentRef = this.viewContainerRef.createComponent(componentFactory);
    componentRef.instance.Closing.subscribe(() => {
      componentRef.destroy();
    });

    let isTryResult: boolean = false;
    this.customerLedgerSearch = this.GetSearchOption();

    this.customerLedgerService.GetReport(this.customerLedgerSearch)
      .subscribe(response => {

        if(response!=PROCESS_RESULT_RESULT_TYPE.FAILURE){
                
          FileUtil.download([response.body], this.Title + this.baseDateFromCtrl.value.format("YYMM") + "_" + this.baseDateToCtrl.value.format("YYMM"), FILE_EXTENSION.PDF);
          isTryResult = true;  
          this.processResultService.processAtOutput(
            this.processCustomResult, isTryResult, 0, this.partsResultMessageComponent);
        }
        else{
          this.processResultService.processAtOutput(
            this.processCustomResult, isTryResult, 0, this.partsResultMessageComponent);
        }
        componentRef.destroy();

      });

  }

  /**
   * エクスポート
   */
  public export() {

    let dataList = this.customerLedgersResult.customerLedgers;

    let componentFactory = this.componentFactoryResolver.resolveComponentFactory(ModalRouterProgressComponent);
    let componentRef = this.viewContainerRef.createComponent(componentFactory);
    // componentRef.destroy();

    const ignoreSectionName = !(this.userInfoService.ApplicationControl.useReceiptSection == 1
                              && !this.customerLedgerSearch.doGroupReceipt && this.customerLedgerSearch.displaySection);

    let headers = (REPORT_HEADER.CUSTOMER_LEDGER_FORM);
    if (ignoreSectionName) {
      const index = headers.indexOf("入金部門");
      if (index >= 0) headers.splice(index,1);      
    }
    if (this.userInfoService.ApplicationControl.useForeignCurrency == 0) {
      const index = headers.indexOf("通貨コード");
      if (index >= 0) headers.splice(index,1);      
    }

    let data: string = FileUtil.encloseItemBySymbol(headers).join(",") + LINE_FEED_CODE;

    for (let index = 0; index < dataList.length; index++) {
      let dataItem: Array<any> = [];

      dataList[index].billingAmount = NumberUtil.Round(dataList[index].billingAmount);
      dataList[index].slipTotal = NumberUtil.Round(dataList[index].slipTotal);
      dataList[index].receiptAmount = NumberUtil.Round(dataList[index].receiptAmount);
      dataList[index].matchingAmount = NumberUtil.Round(dataList[index].matchingAmount);
      dataList[index].remainAmount = NumberUtil.Round(dataList[index].remainAmount);


      if (dataList[index].recordType == 0) {
        
        dataItem.push(DateUtil.convertDateString(dataList[index].recordedAt));
        dataItem.push(dataList[index].parentCustomerCode);
        dataItem.push(dataList[index].parentCustomerName);

        if(!ignoreSectionName){
          dataItem.push(dataList[index].sectionName);
        }
        
        dataItem.push(dataList[index].departmentName);
        dataItem.push(dataList[index].invoiceCode);
        dataItem.push(dataList[index].categoryName);
        dataItem.push(dataList[index].accountTitleName);

        if(this.userInfoService.ApplicationControl.useForeignCurrency == 1){
          dataItem.push(dataList[index].currencyCode);
        }

        dataItem.push(dataList[index].matchingSymbolBilling);
        dataItem.push(dataList[index].billingAmount);
        dataItem.push(dataList[index].slipTotal);
        dataItem.push(dataList[index].receiptAmount);
        dataItem.push(dataList[index].matchingSymbolReceipt);
        dataItem.push(dataList[index].matchingAmount);
        dataItem.push(dataList[index].remainAmount);
        dataItem.push(dataList[index].customerCode);
        dataItem.push(dataList[index].customerName);
      }
      else if (dataList[index].recordType == 1) {
        dataItem.push("繰越");        
        dataItem.push("");
        dataItem.push("");

        if(!ignoreSectionName){
          dataItem.push(dataList[index].sectionName);
        }

        dataItem.push("");
        dataItem.push("");
        dataItem.push("");
        dataItem.push("");

        if(this.userInfoService.ApplicationControl.useForeignCurrency == 1){
          dataItem.push(dataList[index].currencyCode);
        }

        dataItem.push(dataList[index].matchingSymbolBilling);
        dataItem.push(dataList[index].billingAmount);
        dataItem.push(dataList[index].slipTotal);
        dataItem.push(dataList[index].receiptAmount);
        dataItem.push(dataList[index].matchingSymbolReceipt);
        dataItem.push(dataList[index].matchingAmount);
        dataItem.push(dataList[index].remainAmount);
        dataItem.push(dataList[index].customerCode);
        dataItem.push(dataList[index].customerName);
      }
      else if (dataList[index].recordType == 2) {
        dataItem.push(dataList[index].yearMonth.substring(0, 4) + "/" + dataList[index].yearMonth.substring(5, 7) + "合計");
        dataItem.push("");
        dataItem.push("");

        if(!ignoreSectionName){
          dataItem.push(dataList[index].sectionName);
        }

        dataItem.push("");
        dataItem.push("");
        dataItem.push("");
        dataItem.push("");

        if(this.userInfoService.ApplicationControl.useForeignCurrency == 1){
          dataItem.push(dataList[index].currencyCode);
        }

        dataItem.push(dataList[index].matchingSymbolBilling);
        dataItem.push(dataList[index].billingAmount);
        dataItem.push(dataList[index].slipTotal);
        dataItem.push(dataList[index].receiptAmount);
        dataItem.push(dataList[index].matchingSymbolReceipt);
        dataItem.push(dataList[index].matchingAmount);
        dataItem.push(dataList[index].remainAmount);
        dataItem.push(dataList[index].customerCode);
        dataItem.push(dataList[index].customerName);
      }
      else if (dataList[index].recordType == 3) {
        dataItem.push("総合計");
        dataItem.push("");
        dataItem.push("");

        if(!ignoreSectionName){
          dataItem.push(dataList[index].sectionName);
        }

        dataItem.push("");
        dataItem.push("");
        dataItem.push("");
        dataItem.push("");

        if(this.userInfoService.ApplicationControl.useForeignCurrency == 1){
          dataItem.push(dataList[index].currencyCode);
        }

        dataItem.push(dataList[index].matchingSymbolBilling);
        dataItem.push(dataList[index].billingAmount);
        dataItem.push(dataList[index].slipTotal);
        dataItem.push(dataList[index].receiptAmount);
        dataItem.push(dataList[index].matchingSymbolReceipt);
        dataItem.push(dataList[index].matchingAmount);
        dataItem.push(dataList[index].remainAmount);
        dataItem.push(dataList[index].customerCode);
        dataItem.push(dataList[index].customerName);
      }

      dataItem = FileUtil.encloseItemBySymbol(dataItem);
      data = data + dataItem.join(",") + LINE_FEED_CODE;
    }

    let isTryResult: boolean = false;
    let resultDatas: Array<any> = [];
    resultDatas.push(data);

    FileUtil.download(resultDatas, this.Title + this.baseDateFromCtrl.value.format("YYMM") + "_" + this.baseDateToCtrl.value.format("YYMM"), FILE_EXTENSION.CSV);
    isTryResult = true;
    this.processResultService.processAtOutput(
      this.processCustomResult, isTryResult, 0, this.partsResultMessageComponent);

    componentRef.destroy();
  }

  public GetSearchOption(): CustomerLedgerSearch {
    let option = new CustomerLedgerSearch();

    option.customerCodeFrom = this.customerCodeFromCtrl.value;
    option.customerCodeTo = this.customerCodeToCtrl.value;

    option.yearMonthFrom = DateUtil.ConvertFromMomentToIso(this.baseDateFromCtrl);
    option.yearMonthFrom = DateUtil.ConvertFromIso(option.yearMonthFrom, CALENDAR_CONVERT_TYPE.HOUR_START);

    option.yearMonthTo = DateUtil.ConvertFromMomentToIso(this.baseDateToCtrl);
    option.yearMonthTo = DateUtil.ConvertFromIso(option.yearMonthTo, CALENDAR_CONVERT_TYPE.HOUR_END);


    if (!StringUtil.IsNullOrEmpty(this.closingDateCtrl.value)) {
      option.closingDay = parseInt(this.closingDateCtrl.value);
    }

    this.InitializeYearMonth(option);

    option.remainType = parseInt(ReportSettigUtil.getItemKey(
      this.reportSettingsResult.reportSettings,
      REPORT_ITEM_ID.ReportAdvanceReceivedType,
      PF0501.RemainType));

    option.companyId = this.userInfoService.Company.id;
    option.currencyId = this.userInfoService.Currency.id;

    option.useBilledAt = parseInt(this.GetReportSettingTargetDate()) == ReportTargetDate.BilledAt;

    option.groupBillingType = parseInt(ReportSettigUtil.getItemKey(
      this.reportSettingsResult.reportSettings,
      REPORT_ITEM_ID.ReportTotalByDay,
      PF0501.TotalByDay));

    option.billingSlipType = parseInt(ReportSettigUtil.getItemKey(
      this.reportSettingsResult.reportSettings,
      REPORT_ITEM_ID.ReportTotal,
      PF0501.SlipTotal));


    let tmpDoGroupReceipt = parseInt(ReportSettigUtil.getItemKey(
      this.reportSettingsResult.reportSettings,
      REPORT_ITEM_ID.ReportDoOrNot,
      PF0501.ReceiptGrouping));

    option.doGroupReceipt = tmpDoGroupReceipt == ReportDoOrNot.Do;


    let tmpDisplaySection = parseInt(ReportSettigUtil.getItemKey(
      this.reportSettingsResult.reportSettings,
      REPORT_ITEM_ID.ReportDoOrNot,
      PF0501.DisplaySectionCode));

    option.displaySection = tmpDisplaySection == ReportDoOrNot.Do;


    let tmpDisplayDepartment = parseInt(ReportSettigUtil.getItemKey(
      this.reportSettingsResult.reportSettings,
      REPORT_ITEM_ID.ReportDoOrNot,
      PF0501.DisplayDepartmentCode));

    option.displayDepartment = tmpDisplayDepartment == ReportDoOrNot.Do;


    let tmpDisplayMatchingSymbol = parseInt(ReportSettigUtil.getItemKey(
      this.reportSettingsResult.reportSettings,
      REPORT_ITEM_ID.ReportDoOrNot,
      PF0501.DisplaySymbol));

    option.displayMatchingSymbol = tmpDisplayMatchingSymbol == ReportDoOrNot.Do;


    let tmpRequireMonthlyBreak = parseInt(ReportSettigUtil.getItemKey(
      this.reportSettingsResult.reportSettings,
      REPORT_ITEM_ID.ReportDoOrNot,
      PF0501.MonthlyBreak));

    option.requireMonthlyBreak = tmpRequireMonthlyBreak == ReportDoOrNot.Do;

    var unit = parseInt(ReportSettigUtil.getItemKey(
      this.reportSettingsResult.reportSettings,
      REPORT_ITEM_ID.ReportUnitPrice,
      PF0501.UnitPrice));
    option.unitPrice =
      unit == ReportUnitPrice.Per1      ? 1     :
      unit == ReportUnitPrice.Per1000   ? 1000  :
      unit == ReportUnitPrice.Per10000  ? 10000 : 1000000;

    return option;
  }

  public InitializeYearMonth(option:CustomerLedgerSearch){

    let closingDay;
    if (!StringUtil.IsNullOrEmpty(this.closingDateCtrl.value)) {
      closingDay = parseInt(this.closingDateCtrl.value);
    }    
    else{
      closingDay = this.userInfoService.Company.closingDay;
    }

    let start:Moment = this.baseDateFromCtrl.value;
    let end:Moment = this.baseDateToCtrl.value;

    let startYear = start.year();
    let startMonth = start.month();

    let endYear = end.year();
    let endMonth = end.month();

    let day = closingDay > 27 ? 99 : closingDay;

    if (startYear < 1 || 9999 <= startYear) return false;
    if (startMonth+1 < 1 || 12 < startMonth+1) return false;
    if (endYear < 1 || 9999 <= endYear) return false;
    if (endMonth+1 < 1 || 12 < endMonth+1) return false;
    if (startYear == 1 && startMonth+1 == 1) return false;

    let dat = new Date(startYear, startMonth,
        day != 99 ? day : 1);
    option.yearMonthFrom = "" + dat.getFullYear() + "/" + (dat.getMonth()+1) + "/" + dat.getDate();

    dat = new Date(endYear, endMonth,
      day != 99 ? day : end.daysInMonth());
    option.yearMonthTo = "" + dat.getFullYear() + "/" + (dat.getMonth()+1) + "/" + dat.getDate();


  }

  public GetReportSettingTargetDate(): string {
    if (this.closingInformation.useClosing) {
      return "";
    }
    else {
      return ReportSettigUtil.getItemKey(
        this.reportSettingsResult.reportSettings,
        REPORT_ITEM_ID.ReportTargetDate,
        PF0501.TargetDate
      );
    }
  }


  public SetSearchOptionInformation() {
    if (!this.userInfoService.ApplicationControl.useForeignCurrency) {
      this.dispReportUnitPriceCtrl.setValue(this.reportSettingsResult.reportSettings[8].itemValue);				// 金額単位
    }

    this.dispReportAdvancedReceivedTypeCtrl.setValue(this.reportSettingsResult.reportSettings[5].itemValue);			// 請求残計算
    this.dispReportDoOrNotCtrl.setValue(this.reportSettingsResult.reportSettings[4].itemValue);					// 入金データ集計

    let target = parseInt(this.GetReportSettingTargetDate());

    this.dispReportTargetDateCtrl.setValue(
      target == ReportTargetDate.BilledAt ? "請求日" :
        target == ReportTargetDate.SalesAt ? "売上日" : ""
    );				                    // 集計基準日

    this.dispDispReportTotalCtrl.setValue(this.reportSettingsResult.reportSettings[3].itemValue);				// 伝票集計方法
    this.dispReportTotalByDayCtrl.setValue(this.reportSettingsResult.reportSettings[1].itemValue); 			// 請求データ集計                  
  }


  /////////////////////////////////////////////////////////////////////////////////

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
      this.customerCodeToTrigger.closePanel();
    }

    if (!StringUtil.IsNullOrEmpty(this.customerCodeToCtrl.value)) {

      if (this.userInfoService.ApplicationControl.customerCodeType == CODE_TYPE.NUMBER) {
        this.customerCodeToCtrl.setValue(StringUtil.setPaddingFrontZero(this.customerCodeToCtrl.value, this.userInfoService.ApplicationControl.customerCodeLength));
      }      

      this.customerService.GetItems(this.customerCodeToCtrl.value)
        .subscribe(response => {
          if (response != PROCESS_RESULT_RESULT_TYPE.FAILURE && response.length > 0) {
            this.customerCodeToCtrl.setValue(response[0].code);
            this.customerNameToCtrl.setValue(response[0].name);

          }
          else {
            //this.customerCodeToCtrl.setValue("");
            this.customerNameToCtrl.setValue("");
          }
          HtmlUtil.nextFocusByNames(this.elementRef, ['pickerBaseDateFromCtrl','baseDateFromCtrl'], eventType);
        });
    }
    else {
      this.customerCodeToCtrl.setValue("");
      this.customerNameToCtrl.setValue("");
      HtmlUtil.nextFocusByNames(this.elementRef, ['pickerBaseDateFromCtrl', 'baseDateFromCtrl'], eventType);
    }
  }

  public setCbxCustomer(eventType: string) {

    let localstorageItem = new LocalStorageItem();
    localstorageItem.key = RangeSearchKey.PF0501_CUSTOMER;
    localstorageItem.value = this.cbxCustomerCtrl.value;
    this.localStorageManageService.set(localstorageItem);

    HtmlUtil.nextFocusByName(this.elementRef, 'customerCodeFromCtrl', eventType);

  }

  ///////////////////////////////////////////////////////
  public setBaseDateFrom(eventType: string) {
    HtmlUtil.nextFocusByName(this.elementRef, 'baseDateToCtrl', eventType);
  }

  public setBaseDateTo(eventType: string) {
    HtmlUtil.nextFocusByName(this.elementRef, 'rdoRemainTypeCtrl', eventType);
  }

  ///////////////////////////////////////////////////////
  public setRdoRemainType(eventType: string) {
    if (this.rdoRemainTypeCtrl.value == "1") {
      this.closingDateCtrl.enable();
      this.closingDateCtrl.setValue("");
      this.closingDateCtrl.setValidators([Validators.required]);
      this.closingDateCtrl.updateValueAndValidity();

      HtmlUtil.nextFocusByName(this.elementRef, 'closingDateCtrl', eventType);
    }
    else {
      this.closingDateCtrl.setValidators([]);
      this.closingDateCtrl.updateValueAndValidity();
      this.closingDateCtrl.disable();
      this.closingDateCtrl.setValue("");

      HtmlUtil.nextFocusByName(this.elementRef, 'departmentCodeFromCtrl', eventType);
    }
  }


  ///////////////////////////////////////////////////////
  public setClosingDate(eventType: string) {
    HtmlUtil.nextFocusByName(this.elementRef, 'currencyCodeCtrl', eventType);
  }

  /////////////////////////////////////////////////////////////////////////////////
  public setCurrencyCode(eventType: string) {

    if (!StringUtil.IsNullOrEmpty(this.currencyCodeCtrl.value)) {

      this.loadStart();
      this.currencyService.GetItems(this.currencyCodeCtrl.value)
        .subscribe(response => {
          this.loadEnd();
          if (response != PROCESS_RESULT_RESULT_TYPE.FAILURE && response.length > 0) {
            this.currencyCodeCtrl.setValue(response[0].code);
            HtmlUtil.nextFocusByName(this.elementRef, 'customerCodeFromCtrl', eventType);
          }
          else {
            this.currencyCodeCtrl.setValue("");
            HtmlUtil.nextFocusByName(this.elementRef, 'customerCodeFromCtrl', eventType);
          }
        });
    }
    else {
      this.currencyCodeCtrl.setValue("");
      HtmlUtil.nextFocusByName(this.elementRef, 'customerCodeFromCtrl', eventType);
    }

  }

}
