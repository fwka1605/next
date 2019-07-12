import { Component, OnInit, ElementRef, ComponentFactoryResolver, ViewContainerRef, AfterViewInit, ViewChild } from '@angular/core';
import { BaseComponent } from 'src/app/component/common/base/base-component';
import { Router, ActivatedRoute, NavigationEnd } from '@angular/router';
import { CategoryMasterService } from 'src/app/service/Master/category-master.service';
import { TABLE_INDEX } from 'src/app/common/const/table-name.const';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { ModalMasterComponent } from 'src/app/component/modal/modal-master/modal-master.component';
import { HtmlUtil } from 'src/app/common/util/html.util';
import { KEY_CODE, EVENT_TYPE, BUTTON_ACTION } from 'src/app/common/const/event.const';
import { StringUtil } from 'src/app/common/util/string-util';
import { DepartmentMasterService } from 'src/app/service/Master/department-master.service';
import { StaffMasterService } from 'src/app/service/Master/staff-master.service';
import { CustomerMasterService } from 'src/app/service/Master/customer-master.service';
import { ModalFormSettingComponent } from 'src/app/component/modal/modal-form-setting/modal-form-setting.component';
import { FormatterUtil } from 'src/app/common/util/formatter.util';
import { MODAL_STATUS_TYPE, PROCESS_RESULT_RESULT_TYPE } from 'src/app/common/const/status.const';
import { UserInfoService } from 'src/app/service/common/user-info.service';
import { NgbDateParserFormatter, NgbCalendar } from '@ng-bootstrap/ng-bootstrap';
import { NgbDateCustomParserFormatter } from 'src/app/common/util/dateformat-util';
import { CurrencyMasterService } from 'src/app/service/Master/currency-master.service';
import { COMPONENT_INFO } from 'src/app/common/const/component-name.const';
import { ColumnNameSettingsResult } from 'src/app/model/column-name-settings-result.model';
import { ColumnNameSettingMasterService } from 'src/app/service/Master/column-name-setting-master.service';
import { CategoryType, CODE_TYPE } from 'src/app/common/const/kbn.const';
import { ScheduledPaymentListSearch } from 'src/app/model/scheduled-payment-list-search.model';
import { ScheduledPaymentListsResult } from 'src/app/model/scheduled-payment-lists-result.model';
import { DateUtil } from 'src/app/common/util/date-util';
import { ReportService } from 'src/app/service/report.service';
import { ModalRouterProgressComponent } from 'src/app/component/modal/modal-router-progress/modal-router-progress.component';
import { ReportSetting } from 'src/app/model/report-setting.model';
import { ReportSettingMasterService } from 'src/app/service/Master/report-setting-master.service';
import { ReportSettingsResult } from 'src/app/model/report-settings-result.model';
import { FILE_EXTENSION } from 'src/app/common/const/eb-file.const';
import { ColumnNameSettigUtil } from 'src/app/common/util/column-name-setting-util';
import { FileUtil } from 'src/app/common/util/file.util';
import { REPORT_HEADER, LINE_FEED_CODE } from 'src/app/common/const/report-setting-kbn.const';
import { ProcessResultService } from 'src/app/service/common/process-result.service';
import { MSG_WNG, MSG_ERR, MSG_ITEM_NUM, MSG_INF } from 'src/app/common/const/message.const';
import { PageUtil } from 'src/app/common/util/page-util';
import { NumberUtil } from 'src/app/common/util/number-util';
import { LocalStorageItem } from 'src/app/model/custom-model/local-storage-item';
import { RangeSearchKey } from 'src/app/common/const/local-storage-key-const';
import { LocalStorageManageService } from 'src/app/service/common/local-storage-manage.service';
import { MatAutocompleteTrigger, MatExpansionPanel } from '@angular/material';
import { DatePipe }     from '@angular/common';

@Component({
  selector: 'app-pf0301-scheduled-payment-list',
  templateUrl: './pf0301-scheduled-payment-list.component.html',
  styleUrls: ['./pf0301-scheduled-payment-list.component.css'],
  providers: [
    { provide: NgbDateParserFormatter, useClass: NgbDateCustomParserFormatter }
  ]

})
export class Pf0301ScheduledPaymentListComponent extends BaseComponent implements OnInit,AfterViewInit {

  // パネルの開閉フラグ
  public panelOpenState: boolean;

  // これをしないとEnum型がテンプレートで見えなかったため
  public TABLE_INDEX: typeof TABLE_INDEX = TABLE_INDEX;

  public columnNameSettingsResult: ColumnNameSettingsResult;

  public scheduledPaymentListsResult: ScheduledPaymentListsResult;
  public scheduledPaymentListSearch: ScheduledPaymentListSearch;

  public sumAmount: number; // 回収予定額合計

  public baseDate: string;  //  基準日格納用

  public baseDateCtrl: FormControl; // 基準日

  public billedAtFromCtrl: FormControl;  // 請求日
  public billedAtToCtrl: FormControl;

  public dueAtFromCtrl: FormControl;  // 入金予定日
  public dueAtToCtrl: FormControl;

  public closingAtFromCtrl: FormControl;  // 請求締日
  public closingAtToCtrl: FormControl;

  public invoiceCodeFromCtrl: FormControl;  // 請求書番号
  public invoiceCodeToCtrl: FormControl;
  public invoiceCodeCtrl: FormControl;

  public collectCategoryCodeCtrl: FormControl;  // 回収区分
  public collectCategoryNameCtrl: FormControl;

  public currencyCodeCtrl: FormControl; // 通貨コード

  public departmentCodeFromCtrl: FormControl;  // 請求部門コード
  public departmentNameFromCtrl: FormControl;
  public departmentCodeToCtrl: FormControl;
  public departmentNameToCtrl: FormControl;
  public cbxDepartmentCtrl: FormControl;

  public staffCodeFromCtrl: FormControl;  // 担当者コード
  public staffNameFromCtrl: FormControl;
  public staffCodeToCtrl: FormControl;
  public staffNameToCtrl: FormControl;
  public cbxStaffCtrl: FormControl;

  public customerCodeFromCtrl: FormControl;  // 得意先コード
  public customerNameFromCtrl: FormControl;
  public customerCodeToCtrl: FormControl;
  public customerNameToCtrl: FormControl;
  public cbxCustomerCtrl: FormControl;
  public customerNameCtrl: FormControl;

  public cbxCustomerAggregateCtrl: FormControl; // 得意先毎に集計

  public undefineCtrl: FormControl; // 未定用

  @ViewChild('collectCategoryCodeInput', { read: MatAutocompleteTrigger }) collectCategoryCodeTrigger: MatAutocompleteTrigger;
  @ViewChild('departmentCodeFromInput', { read: MatAutocompleteTrigger }) departmentCodeFromigger: MatAutocompleteTrigger;
  @ViewChild('departmentCodeToInput', { read: MatAutocompleteTrigger }) departmentCodeToigger: MatAutocompleteTrigger;
  @ViewChild('staffCodeFromInput', { read: MatAutocompleteTrigger }) staffCodeFromTrigger: MatAutocompleteTrigger;
  @ViewChild('staffCodeToInput', { read: MatAutocompleteTrigger }) staffCodeToTrigger: MatAutocompleteTrigger;
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
    public columnNameSettingService: ColumnNameSettingMasterService,
    public categoryService: CategoryMasterService,
    public departmentService: DepartmentMasterService,
    public staffService: StaffMasterService,
    public customerService: CustomerMasterService,
    public currencyService: CurrencyMasterService,
    public reportService: ReportService,
    public reportSettingService: ReportSettingMasterService,
    public calendar: NgbCalendar,
    public processResultService: ProcessResultService,
    public localStorageManageService:LocalStorageManageService,
    private datePipe: DatePipe,

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

    this.columnNameSettingService.Get(CategoryType.Billing)
      .subscribe(response => {
        this.processCustomResult = this.processResultService.processAtGetData(
          this.processCustomResult, response, false, this.partsResultMessageComponent);
        if (this.processCustomResult.result == PROCESS_RESULT_RESULT_TYPE.SUCCESS) {
          this.columnNameSettingsResult = new ColumnNameSettingsResult();
          this.columnNameSettingsResult.columnNames = response;
        }
      })

  }

  ngAfterViewInit(){
    HtmlUtil.nextFocusByName(this.elementRef, 'baseDateCtrl', EVENT_TYPE.NONE);
    
  }

  public setControlInit() {

    this.baseDateCtrl = new FormControl("", [Validators.required, Validators.maxLength(10)]);  // 基準日

    this.billedAtFromCtrl = new FormControl("", [Validators.maxLength(10)]);  // 請求日
    this.billedAtToCtrl = new FormControl("", [Validators.maxLength(10)]);

    this.dueAtFromCtrl = new FormControl("", [Validators.maxLength(10)]);  // 入金予定日
    this.dueAtToCtrl = new FormControl("", [Validators.maxLength(10)]);

    this.closingAtFromCtrl = new FormControl("", [Validators.maxLength(10)]);  // 請求締日
    this.closingAtToCtrl = new FormControl("", [Validators.maxLength(10)]);

    this.invoiceCodeFromCtrl = new FormControl("", [Validators.maxLength(20)]);  // 請求書番号
    this.invoiceCodeToCtrl = new FormControl("", [Validators.maxLength(20)]);
    this.invoiceCodeCtrl = new FormControl("", [Validators.maxLength(20)]);

    this.collectCategoryCodeCtrl = new FormControl("", [Validators.maxLength(2)]);  // 回収区分
    this.collectCategoryNameCtrl = new FormControl("");

    this.currencyCodeCtrl = new FormControl("", [Validators.maxLength(3)]);  // 通貨コード

    this.departmentCodeFromCtrl = new FormControl("", [Validators.maxLength(this.userInfoService.ApplicationControl.departmentCodeLength)]);  // 請求部門コード
    this.departmentNameFromCtrl = new FormControl("");
    this.departmentCodeToCtrl = new FormControl("", [Validators.maxLength(this.userInfoService.ApplicationControl.departmentCodeLength)]);
    this.departmentNameToCtrl = new FormControl("");
    this.cbxDepartmentCtrl = new FormControl("");

    this.staffCodeFromCtrl = new FormControl("", [Validators.maxLength(this.userInfoService.ApplicationControl.staffCodeLength)]);  // 担当者コード
    this.staffNameFromCtrl = new FormControl("");
    this.staffCodeToCtrl = new FormControl("", [Validators.maxLength(this.userInfoService.ApplicationControl.staffCodeLength)]);
    this.staffNameToCtrl = new FormControl("");
    this.cbxStaffCtrl = new FormControl("");

    this.customerCodeFromCtrl = new FormControl("", [Validators.maxLength(this.userInfoService.ApplicationControl.customerCodeLength)]);  // 得意先コード
    this.customerNameFromCtrl = new FormControl("");
    this.customerCodeToCtrl = new FormControl("", [Validators.maxLength(this.userInfoService.ApplicationControl.customerCodeLength)]);
    this.customerNameToCtrl = new FormControl("");
    this.cbxCustomerCtrl = new FormControl("");
    this.customerNameCtrl = new FormControl("");

    this.cbxCustomerAggregateCtrl = new FormControl(""); // 得意先毎に集計

    this.undefineCtrl = new FormControl(""); // 未定用;

  }

  public setValidator() {
    this.MyFormGroup = new FormGroup({

      baseDateCtrl: this.baseDateCtrl,  // 基準日
      billedAtFromCtrl: this.billedAtFromCtrl,  // 請求日
      billedAtToCtrl: this.billedAtToCtrl,
      dueAtFromCtrl: this.dueAtFromCtrl,  // 入金予定日
      dueAtToCtrl: this.dueAtToCtrl,

      closingAtFromCtrl: this.closingAtFromCtrl,  // 請求締日
      closingAtToCtrl: this.closingAtToCtrl,

      invoiceCodeFromCtrl: this.invoiceCodeFromCtrl,  // 請求書番号
      invoiceCodeToCtrl: this.invoiceCodeToCtrl,
      invoiceCodeCtrl: this.invoiceCodeCtrl,
      collectCategoryCodeCtrl: this.collectCategoryCodeCtrl,  // 回収区分
      collectCategoryNameCtrl: this.collectCategoryNameCtrl,
      currencyCodeCtrl: this.currencyCodeCtrl,  // 通貨コード
      departmentCodeFromCtrl: this.departmentCodeFromCtrl,  // 請求部門コード
      departmentNameFromCtrl: this.departmentNameFromCtrl,
      departmentCodeToCtrl: this.departmentCodeToCtrl,
      departmentNameToCtrl: this.departmentNameToCtrl,
      cbxDepartmentCtrl: this.cbxDepartmentCtrl,
      staffCodeFromCtrl: this.staffCodeFromCtrl,  // 担当者コード
      staffNameFromCtrl: this.staffNameFromCtrl,
      staffCodeToCtrl: this.staffCodeToCtrl,
      staffNameToCtrl: this.staffNameToCtrl,
      cbxStaffCtrl: this.cbxStaffCtrl,
      customerCodeFromCtrl: this.customerCodeFromCtrl,  // 得意先コード
      customerNameFromCtrl: this.customerNameFromCtrl,
      customerCodeToCtrl: this.customerCodeToCtrl,
      customerNameToCtrl: this.customerNameToCtrl,
      cbxCustomerCtrl: this.cbxCustomerCtrl,
      customerNameCtrl: this.customerNameCtrl,
      cbxCustomerAggregateCtrl: this.cbxCustomerAggregateCtrl,   // 得意先毎に集計

      undefineCtrl: this.undefineCtrl, // 未定用;

    });

  }

  public setFormatter() {

    FormatterUtil.setNumberFormatter(this.collectCategoryCodeCtrl);  // 回収区分

    FormatterUtil.setAlphabetFormatter(this.currencyCodeCtrl);  // 通貨コード

    if (this.userInfoService.ApplicationControl.departmentCodeType == CODE_TYPE.NUMBER) {
      FormatterUtil.setNumberFormatter(this.departmentCodeFromCtrl); // 請求部門コード
      FormatterUtil.setNumberFormatter(this.departmentCodeToCtrl);
    }
    else {
      FormatterUtil.setCodeFormatter(this.departmentCodeFromCtrl);
      FormatterUtil.setCodeFormatter(this.departmentCodeToCtrl);
    }

    if (this.userInfoService.ApplicationControl.staffCodeType == CODE_TYPE.NUMBER) {
      FormatterUtil.setNumberFormatter(this.staffCodeFromCtrl); // 担当者コード
      FormatterUtil.setNumberFormatter(this.staffCodeToCtrl);
    }
    else {
      FormatterUtil.setCodeFormatter(this.staffCodeFromCtrl);
      FormatterUtil.setCodeFormatter(this.staffCodeToCtrl);
    }

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

  }


  public setAutoComplete(){

    // 回収区分
    this.initAutocompleteCategories(CategoryType.Collection,this.collectCategoryCodeCtrl,this.categoryService,1);
    // 請求部門
    this.initAutocompleteDepartments(this.departmentCodeFromCtrl,this.departmentService,0);
    this.initAutocompleteDepartments(this.departmentCodeToCtrl,this.departmentService,1);
    // 担当者
    this.initAutocompleteStaffs(this.staffCodeFromCtrl,this.staffService,0);
    this.initAutocompleteStaffs(this.staffCodeToCtrl,this.staffService,1);
    // 得意先
    this.initAutocompleteCustomers(this.customerCodeFromCtrl,this.customerService,0);
    this.initAutocompleteCustomers(this.customerCodeToCtrl,this.customerService,1);

  }


  /**
   * 設定モーダル呼出
   */
  public openFromSettingModal() {
    let componentFactory = this.componentFactoryResolver.resolveComponentFactory(ModalFormSettingComponent);
    let componentRef = this.viewContainerRef.createComponent(componentFactory);

    componentRef.instance.ReportId = COMPONENT_INFO.PF0301.name;
    componentRef.instance.processModalCustomResult = this.processModalCustomResult;

    componentRef.instance.Closing.subscribe(() => {
      componentRef.destroy();
    });
  }

  /**
   * マスターの値を取得する
   * @param table Master名称
   * @param type モーダルのボタン操作種別
   * @param keyCode キーコードがある場合はF9のみモーダルを開く
   * @param index 明細行の行No
   */
  public openMasterModal(table: TABLE_INDEX, type: string = null) {

    this.collectCategoryCodeTrigger.closePanel();
    this.departmentCodeFromigger.closePanel();
    this.departmentCodeToigger.closePanel();
    this.staffCodeFromTrigger.closePanel();
    this.staffCodeToTrigger.closePanel();
    this.customerCodeFromTrigger.closePanel();
    this.customerCodeToTrigger.closePanel();

    let componentFactory = this.componentFactoryResolver.resolveComponentFactory(ModalMasterComponent);
    let componentRef = this.viewContainerRef.createComponent(componentFactory);

    componentRef.instance.TableIndex = table;

    componentRef.instance.Closing.subscribe(() => {

      if (componentRef.instance.ModalStatus == MODAL_STATUS_TYPE.SELECT) {
        switch (table) {

          case TABLE_INDEX.MASTER_COLLECT_CATEGORY:
            {
              this.collectCategoryCodeCtrl.setValue(componentRef.instance.SelectedCode);
              this.collectCategoryNameCtrl.setValue(componentRef.instance.SelectedName);
              break;
            }
          case TABLE_INDEX.MASTER_DEPARTMENT:
            {
              if (type === "from") {
                this.departmentCodeFromCtrl.setValue(componentRef.instance.SelectedCode);
                this.departmentNameFromCtrl.setValue(componentRef.instance.SelectedName);

                if (this.cbxDepartmentCtrl.value == true) {
                  this.departmentCodeToCtrl.setValue(componentRef.instance.SelectedCode);
                  this.departmentNameToCtrl.setValue(componentRef.instance.SelectedName);
                }

              }
              else {
                this.departmentCodeToCtrl.setValue(componentRef.instance.SelectedCode);
                this.departmentNameToCtrl.setValue(componentRef.instance.SelectedName);
              }
              break;
            }
          case TABLE_INDEX.MASTER_STAFF:
            {
              if (type === "from") {
                this.staffCodeFromCtrl.setValue(componentRef.instance.SelectedCode);
                this.staffNameFromCtrl.setValue(componentRef.instance.SelectedName);

                if (this.cbxStaffCtrl.value == true) {
                  this.staffCodeToCtrl.setValue(componentRef.instance.SelectedCode);
                  this.staffNameToCtrl.setValue(componentRef.instance.SelectedName);
                }

              }
              else {
                this.staffCodeToCtrl.setValue(componentRef.instance.SelectedCode);
                this.staffNameToCtrl.setValue(componentRef.instance.SelectedName);
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
        }
      }
      componentRef.destroy();
    });
  }

  public clear() {
    this.panelOpenState = true;
    this.panel.open();
    
    this.MyFormGroup.reset();

    //this.scheduledPaymentListsResult = null;
    this.scheduledPaymentListSearch = null;

    this.baseDateCtrl.setValue(this.calendar.getToday());

    HtmlUtil.nextFocusByName(this.elementRef, 'baseDateCtrl', EVENT_TYPE.NONE);

    this.setRangeCheckbox();
  }


  public setRangeCheckbox() {
    let cbxCustomer = this.localStorageManageService.get(RangeSearchKey.PF0301_CUSTOMER);
    let cbxDepartment = this.localStorageManageService.get(RangeSearchKey.PF0301_DEPARTMENT);
    let cbxStaff = this.localStorageManageService.get(RangeSearchKey.PF0301_STAFF);

    if (cbxCustomer != null) {
      this.cbxCustomerCtrl.setValue(cbxCustomer.value);
    }

    if (cbxDepartment != null) {
      this.cbxDepartmentCtrl.setValue(cbxDepartment.value);
    }

    if (cbxStaff != null) {
      this.cbxStaffCtrl.setValue(cbxStaff.value);
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

  public getColumnAlias(columnName: string): string {
    if (this.columnNameSettingsResult != null) {
      let tmp = ColumnNameSettigUtil.getColumnAlias(this.columnNameSettingsResult.columnNames, columnName);
      return tmp;
    }
    return "";
  }

  /**
   * データ照会
   */
  public search() {
    let componentFactory = this.componentFactoryResolver.resolveComponentFactory(ModalRouterProgressComponent);
    let componentRef = this.viewContainerRef.createComponent(componentFactory);
    componentRef.instance.Closing.subscribe(() => {
      componentRef.destroy();
    });

    this.scheduledPaymentListSearch = this.GetSearchDataCondition();

    this.reportService.ScheduledPaymentList(this.scheduledPaymentListSearch)
      .subscribe(response => {
        if (response != PROCESS_RESULT_RESULT_TYPE.FAILURE) {
          this.scheduledPaymentListsResult = new ScheduledPaymentListsResult();

          this.scheduledPaymentListsResult.scheduledPaymentLists = response;

          if (this.scheduledPaymentListsResult.scheduledPaymentLists == null
            || this.scheduledPaymentListsResult.scheduledPaymentLists.length == 0
          ) {
            this.panelOpenState = true;
            this.processResultService.processAtWarning(
              this.processCustomResult, MSG_WNG.NOT_EXIST_SEARCH_DATA, this.partsResultMessageComponent);
          }
          else {
            this.panelOpenState = false;
            let tmpAmount = 0;
            this.scheduledPaymentListsResult.scheduledPaymentLists.forEach(element => {
              tmpAmount += element.remainAmount;
            });
            this.sumAmount = tmpAmount;
            this.processResultService.processAtSuccess(
              this.processCustomResult, "照会が完了しました。", this.partsResultMessageComponent);
            this.processResultService.createdLog(this.processCustomResult.logData);        
          }

        }
        else {
          this.panelOpenState = true;
          this.processResultService.processAtFailure(this.processCustomResult,
            MSG_ERR.SOMETHING_ERROR.replace(MSG_ITEM_NUM.FIRST, '入金予定明細表の検索'),
            this.partsResultMessageComponent);
        }
        componentRef.destroy();
      })
  }

  public print() {
    let componentFactory = this.componentFactoryResolver.resolveComponentFactory(ModalRouterProgressComponent);
    let componentRef = this.viewContainerRef.createComponent(componentFactory);
    componentRef.instance.Closing.subscribe(() => {
      componentRef.destroy();
    });

    let result: boolean = false;
    // API呼出引数
    let reportSetting = new ReportSetting();
    reportSetting.companyId = this.userInfoService.Company.id;
    reportSetting.reportId = COMPONENT_INFO.PF0301.name;

    let reportSettingsResult: ReportSettingsResult;
    this.reportSettingService.GetItems(reportSetting)
      .subscribe(response => {
        if (response != PROCESS_RESULT_RESULT_TYPE.FAILURE) {
          reportSettingsResult = new ReportSettingsResult();
          reportSettingsResult.reportSettings = response;
  
          this.scheduledPaymentListSearch = this.GetSearchDataCondition();
  
          this.scheduledPaymentListSearch.reportSettings = reportSettingsResult.reportSettings;
  
          this.reportService.GetScheduledPaymentReport(this.scheduledPaymentListSearch)
            .subscribe(response => {
              if (response != PROCESS_RESULT_RESULT_TYPE.FAILURE) {
                FileUtil.download([response.body], this.Title + DateUtil.getYYYYMMDD(0), FILE_EXTENSION.PDF);
                result = true;
                this.processResultService.processAtOutput(
                  this.processCustomResult, result, 0, this.partsResultMessageComponent);
              }
              else {
                this.processResultService.processAtOutput(
                  this.processCustomResult, result, 0, this.partsResultMessageComponent);
              }

            });          
        }
        else{
          this.processResultService.processAtOutput(
            this.processCustomResult, result, 0, this.partsResultMessageComponent);
        }
        componentRef.destroy();

      });

  }


  /**
   * エクスポート処理
   */
  public export() {
    let data: string = "";
    let dataList = this.scheduledPaymentListsResult.scheduledPaymentLists;
    let dataItems = Array<string>();

    let componentFactory = this.componentFactoryResolver.resolveComponentFactory(ModalRouterProgressComponent);
    let componentRef = this.viewContainerRef.createComponent(componentFactory);
    // componentRef.destroy();

    dataItems[0] = FileUtil.encloseItemBySymbol(REPORT_HEADER.SCHEDULED_PAYMENT_LIST_FORM).toString();
    dataItems[1] = FileUtil.encloseItemBySymbol([ColumnNameSettigUtil.getColumnAlias(this.columnNameSettingsResult.columnNames, "Note1")]).toString();
    dataItems[2] = FileUtil.encloseItemBySymbol([ColumnNameSettigUtil.getColumnAlias(this.columnNameSettingsResult.columnNames, "Note2")]).toString();
    dataItems[3] = FileUtil.encloseItemBySymbol([ColumnNameSettigUtil.getColumnAlias(this.columnNameSettingsResult.columnNames, "Note3")]).toString();
    dataItems[4] = FileUtil.encloseItemBySymbol([ColumnNameSettigUtil.getColumnAlias(this.columnNameSettingsResult.columnNames, "Note4")]).toString();
    data = dataItems.join(',') + LINE_FEED_CODE;

    for (let index = 0; index < dataList.length; index++) {
      let dataItem: Array<any> = [];

      dataList[index].baseDate      = this.baseDate;
      dataList[index].remainAmount  = NumberUtil.Round(dataList[index].remainAmount);

      dataItem.push(dataList[index].baseDate);
      dataItem.push(dataList[index].id);
      dataItem.push(dataList[index].customerCode);
      dataItem.push(dataList[index].customerName);

      dataItem.push(DateUtil.convertDateString(dataList[index].billedAt));
      dataItem.push(DateUtil.convertDateString(dataList[index].salesAt));
      dataItem.push(DateUtil.convertDateString(dataList[index].closingAt));
      dataItem.push(DateUtil.convertDateString(dataList[index].dueAt));
      dataItem.push(DateUtil.convertDateString(dataList[index].originalDueAt));

      dataItem.push(dataList[index].remainAmount);
      dataItem.push(dataList[index].delayDivision);
      dataItem.push(dataList[index].collectCategoryCode + ":" + dataList[index].collectCategoryName);
      dataItem.push(dataList[index].invoiceCode);
      dataItem.push(dataList[index].departmentCode);
      dataItem.push(dataList[index].departmentName);
      dataItem.push(dataList[index].staffCode);
      dataItem.push(dataList[index].staffName);
      dataItem.push(dataList[index].note1);
      dataItem.push(dataList[index].note2);
      dataItem.push(dataList[index].note3);
      dataItem.push(dataList[index].note4);

      dataItem = FileUtil.encloseItemBySymbol(dataItem);
      data = data + dataItem.join(",") + LINE_FEED_CODE;
    }

    let isTryResult: boolean = false;
    let resultDatas: Array<any> = [];
    resultDatas.push(data);

    FileUtil.download(resultDatas, this.Title + DateUtil.getYYYYMMDD(0), FILE_EXTENSION.CSV);
    isTryResult = true;
    this.processResultService.processAtOutput(
      this.processCustomResult, isTryResult, 0, this.partsResultMessageComponent);

    componentRef.destroy();

  }


  public GetSearchDataCondition(): ScheduledPaymentListSearch {
    let paymentListSearch = new ScheduledPaymentListSearch();

    paymentListSearch.reportSettings = new Array<ReportSetting>();

    paymentListSearch.companyId = this.userInfoService.Company.id;    

    paymentListSearch.baseDate = DateUtil.ConvertFromDatepickerToStart(this.baseDateCtrl);
    this.baseDate = this.datePipe.transform(DateUtil.ConvertFromDatepicker(this.baseDateCtrl), 'yyyy/MM/dd')  ;

    if (!StringUtil.IsNullOrEmpty(this.billedAtFromCtrl.value)) {
      paymentListSearch.billedAtFrom = DateUtil.ConvertFromDatepickerToStart(this.billedAtFromCtrl);
    }

    if (!StringUtil.IsNullOrEmpty(this.billedAtToCtrl.value)) {
      paymentListSearch.billedAtTo = DateUtil.ConvertFromDatepickerToEnd(this.billedAtToCtrl);
    }

    if (!StringUtil.IsNullOrEmpty(this.dueAtFromCtrl.value)) {
      paymentListSearch.dueAtFrom = DateUtil.ConvertFromDatepickerToStart(this.dueAtFromCtrl);
    }

    if (!StringUtil.IsNullOrEmpty(this.dueAtToCtrl.value)) {
      paymentListSearch.dueAtTo = DateUtil.ConvertFromDatepickerToEnd(this.dueAtToCtrl);
    }

    if (!StringUtil.IsNullOrEmpty(this.closingAtFromCtrl.value)) {
      paymentListSearch.closingAtFrom = DateUtil.ConvertFromDatepickerToStart(this.closingAtFromCtrl);
    }

    if (!StringUtil.IsNullOrEmpty(this.closingAtToCtrl.value)) {
      paymentListSearch.closingAtTo = DateUtil.ConvertFromDatepickerToEnd(this.closingAtToCtrl);
    }

    if (!StringUtil.IsNullOrEmpty(this.invoiceCodeFromCtrl.value)) {
      paymentListSearch.invoiceCodeFrom = this.invoiceCodeFromCtrl.value;
    }

    if (!StringUtil.IsNullOrEmpty(this.invoiceCodeToCtrl.value)) {
      paymentListSearch.invoiceCodeTo = this.invoiceCodeToCtrl.value;
    }

    if (!StringUtil.IsNullOrEmpty(this.invoiceCodeCtrl.value)) {
      paymentListSearch.invoiceCode = this.invoiceCodeCtrl.value;
    }

    if (!StringUtil.IsNullOrEmpty(this.collectCategoryCodeCtrl.value)) {
      paymentListSearch.categoryCode = this.collectCategoryCodeCtrl.value;
    }

    if (!StringUtil.IsNullOrEmpty(this.currencyCodeCtrl.value)) {
      paymentListSearch.currencyCode = this.currencyCodeCtrl.value;
    }
    else {
      paymentListSearch.currencyCode = this.userInfoService.Currency.code;
      paymentListSearch.precision = this.userInfoService.Currency.precision;
    }

    if (!StringUtil.IsNullOrEmpty(this.departmentCodeFromCtrl.value)) {
      paymentListSearch.departmentCodeFrom = this.departmentCodeFromCtrl.value;
    }

    if (!StringUtil.IsNullOrEmpty(this.departmentCodeToCtrl.value)) {
      paymentListSearch.departmentCodeTo = this.departmentCodeToCtrl.value;
    }

    if (!StringUtil.IsNullOrEmpty(this.staffCodeFromCtrl.value)) {
      paymentListSearch.staffCodeFrom = this.staffCodeFromCtrl.value;
    }

    if (!StringUtil.IsNullOrEmpty(this.staffCodeToCtrl.value)) {
      paymentListSearch.staffCodeTo = this.staffCodeToCtrl.value;
    }

    if (!StringUtil.IsNullOrEmpty(this.customerCodeFromCtrl.value)) {
      paymentListSearch.customerCodeFrom = this.customerCodeFromCtrl.value;
    }

    if (!StringUtil.IsNullOrEmpty(this.customerCodeToCtrl.value)) {
      paymentListSearch.customerCodeTo = this.customerCodeToCtrl.value;
    }
    else {
      paymentListSearch.customerCodeTo = null;
    }

    if (this.cbxCustomerAggregateCtrl.value) {
      paymentListSearch.customerSummaryFlag = true;
    }
    else {
      paymentListSearch.customerSummaryFlag = false;
    }

    return paymentListSearch;
  }

  public selectLine(lineNo: number) {
    this.router.navigate(['main/PC0201']);
    this.processResultService.clearProcessCustomMsg(this.processCustomResult);
  }

  ///////////////////////////////////////////////////////////////////////////
  public setBaseDate(eventType: string) {
    HtmlUtil.nextFocusByName(this.elementRef, 'billedAtFromCtrl', eventType);
  }

  ///////////////////////////////////////////////////////////////////////////
  public setBilledAtFrom(eventType: string) {
    HtmlUtil.nextFocusByName(this.elementRef, 'billedAtToCtrl', eventType);
  }

  public setBilledAtTo(eventType: string) {
    HtmlUtil.nextFocusByName(this.elementRef, 'dueAtFromCtrl', eventType);
  }

  ///////////////////////////////////////////////////////////////////////////
  public setDueAtFrom(eventType: string) {
    HtmlUtil.nextFocusByName(this.elementRef, 'dueAtToCtrl', eventType);
  }

  public setDueAtTo(eventType: string) {
    HtmlUtil.nextFocusByName(this.elementRef, 'closingAtFromCtrl', eventType);
  }

  ///////////////////////////////////////////////////////////////////////////
  public setClosingAtFrom(eventType: string) {
    HtmlUtil.nextFocusByName(this.elementRef, 'closingAtToCtrl', eventType);
  }

  public setClosingAtTo(eventType: string) {
    HtmlUtil.nextFocusByName(this.elementRef, 'invoiceCodeFromCtrl', eventType);
  }

  ///////////////////////////////////////////////////////////////////////////
  public setInvoiceCodeFrom(eventType: string) {
    HtmlUtil.nextFocusByName(this.elementRef, 'invoiceCodeToCtrl', eventType);
  }

  public setInvoiceCodeTo(eventType: string) {
    HtmlUtil.nextFocusByName(this.elementRef, 'invoiceCodeCtrl', eventType);
  }

  ///////////////////////////////////////////////////////////////////////////
  public setInvoiceCode(eventType: string) {
    HtmlUtil.nextFocusByName(this.elementRef, 'collectCategoryCodeCtrl', eventType);
  }

  /////////////////////////////////////////////////////////////////////////////////

  public setCollectCategoryCode(eventType: string) {

    if(eventType!=EVENT_TYPE.BLUR){
      this.collectCategoryCodeTrigger.closePanel();
    }

  if (StringUtil.IsNullOrEmpty(this.collectCategoryCodeCtrl.value)) {
      HtmlUtil.nextFocusByNames(this.elementRef, ['currencyCodeCtrl','departmentCodeFromCtrl'], eventType);
      return;
    }

    this.loadStart();
    this.categoryService.GetItems(CategoryType.Collection, this.collectCategoryCodeCtrl.value)
    .subscribe(response=>{
      this.loadEnd();
      if(response!=PROCESS_RESULT_RESULT_TYPE.FAILURE && 0 < response.length){
        this.collectCategoryCodeCtrl.setValue(response[0].code);
        this.collectCategoryNameCtrl.setValue(response[0].name);
        HtmlUtil.nextFocusByNames(this.elementRef, ['currencyCodeCtrl','departmentCodeFromCtrl'], eventType);
      }
      else {
        // this.collectCategoryCodeCtrl.setValue("");
        this.collectCategoryNameCtrl.setValue("");
      }
    });
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
            HtmlUtil.nextFocusByName(this.elementRef, 'departmentCodeFromCtrl', eventType);
          }
          else {
            this.currencyCodeCtrl.setValue("");
            HtmlUtil.nextFocusByName(this.elementRef, 'departmentCodeFromCtrl', eventType);
          }
        });
    }
    else {
      this.currencyCodeCtrl.setValue("");
      HtmlUtil.nextFocusByName(this.elementRef, 'departmentCodeFromCtrl', eventType);
    }

  }

  /////////////////////////////////////////////////////////////////////////////////

  public setDepartmentCodeFrom(eventType: string) {

    if(eventType!=EVENT_TYPE.BLUR){
      this.departmentCodeFromigger.closePanel();
    }

    if (!StringUtil.IsNullOrEmpty(this.departmentCodeFromCtrl.value)) {

      if (this.userInfoService.ApplicationControl.departmentCodeType == CODE_TYPE.NUMBER) {
        this.departmentCodeFromCtrl.setValue(StringUtil.setPaddingFrontZero(this.departmentCodeFromCtrl.value, this.userInfoService.ApplicationControl.departmentCodeLength));
      }      

      this.loadStart();
      this.departmentService.GetItems(this.departmentCodeFromCtrl.value)
        .subscribe(response => {
          this.loadEnd();
          if (response != PROCESS_RESULT_RESULT_TYPE.FAILURE && response.length > 0) {
            this.departmentCodeFromCtrl.setValue(response[0].code);
            this.departmentNameFromCtrl.setValue(response[0].name);
            if (this.cbxDepartmentCtrl.value == true) {
              this.departmentCodeToCtrl.setValue(response[0].code);
              this.departmentNameToCtrl.setValue(response[0].name);
            }
            HtmlUtil.nextFocusByName(this.elementRef, 'departmentCodeToCtrl', eventType);
          }
          else {
            //this.departmentCodeFromCtrl.setValue("");
            this.departmentNameFromCtrl.setValue("");
            HtmlUtil.nextFocusByName(this.elementRef, 'departmentCodeToCtrl', eventType);
            if (this.cbxDepartmentCtrl.value == true) {
              this.departmentCodeToCtrl.setValue(this.departmentCodeFromCtrl.value);
              this.departmentNameToCtrl.setValue("");
            }            
          }
        });
    }
    else {
      this.departmentCodeFromCtrl.setValue("");
      this.departmentNameFromCtrl.setValue("");
      HtmlUtil.nextFocusByName(this.elementRef, 'departmentCodeToCtrl', eventType);
    }

  }

  public setDepartmentCodeTo(eventType: string) {

    if(eventType!=EVENT_TYPE.BLUR){
      this.departmentCodeToigger.closePanel();
    }

    if (!StringUtil.IsNullOrEmpty(this.departmentCodeToCtrl.value)) {

      if (this.userInfoService.ApplicationControl.departmentCodeType == CODE_TYPE.NUMBER) {
        this.departmentCodeToCtrl.setValue(StringUtil.setPaddingFrontZero(this.departmentCodeToCtrl.value, this.userInfoService.ApplicationControl.departmentCodeLength));
      }      

      this.loadStart();
      this.departmentService.GetItems(this.departmentCodeToCtrl.value)
        .subscribe(response => {
          this.loadEnd();
          if (response != PROCESS_RESULT_RESULT_TYPE.FAILURE && response.length > 0) {
            this.departmentCodeToCtrl.setValue(response[0].code);
            this.departmentNameToCtrl.setValue(response[0].name);

            HtmlUtil.nextFocusByName(this.elementRef, 'staffCodeFromCtrl', eventType);
          }
          else {
            //this.departmentCodeToCtrl.setValue("");
            this.departmentNameToCtrl.setValue("");
            HtmlUtil.nextFocusByName(this.elementRef, 'staffCodeFromCtrl', eventType);
          }
        });
    }
    else {
      this.departmentCodeToCtrl.setValue("");
      this.departmentNameToCtrl.setValue("");
      HtmlUtil.nextFocusByName(this.elementRef, 'staffCodeFromCtrl', eventType);
    }

  }

  public setCbxDepartment(eventType: string) {

    let localstorageItem = new LocalStorageItem();
    localstorageItem.key = RangeSearchKey.PF0301_DEPARTMENT;
    localstorageItem.value = this.cbxDepartmentCtrl.value;
    this.localStorageManageService.set(localstorageItem);


    HtmlUtil.nextFocusByName(this.elementRef, 'departmentCodeFromCtrl', eventType);

  }

  /////////////////////////////////////////////////////////////////////////////////

  public setStaffCodeFrom(eventType: string) {

    if(eventType!=EVENT_TYPE.BLUR){
      this.staffCodeFromTrigger.closePanel();
    }

    if (!StringUtil.IsNullOrEmpty(this.staffCodeFromCtrl.value)) {

      if (this.userInfoService.ApplicationControl.staffCodeType == CODE_TYPE.NUMBER) {
        this.staffCodeFromCtrl.setValue(StringUtil.setPaddingFrontZero(this.staffCodeFromCtrl.value, this.userInfoService.ApplicationControl.staffCodeLength));
      }      

      this.loadStart();
      this.staffService.GetItems(this.staffCodeFromCtrl.value)
        .subscribe(response => {
          this.loadEnd();
          if (response != PROCESS_RESULT_RESULT_TYPE.FAILURE && response.length > 0) {
            this.staffCodeFromCtrl.setValue(response[0].code);
            this.staffNameFromCtrl.setValue(response[0].name);
            if (this.cbxStaffCtrl.value == true) {
              this.staffCodeToCtrl.setValue(response[0].code);
              this.staffNameToCtrl.setValue(response[0].name);
            }
            HtmlUtil.nextFocusByName(this.elementRef, 'staffCodeToCtrl', eventType);
          }
          else {
            //this.staffCodeFromCtrl.setValue("");
            this.staffNameFromCtrl.setValue("");
            if (this.cbxStaffCtrl.value == true) {
              this.staffCodeToCtrl.setValue(this.staffCodeFromCtrl.value);
              this.staffNameToCtrl.setValue("");
            }            
            HtmlUtil.nextFocusByName(this.elementRef, 'staffCodeToCtrl', eventType);
          }
        });
    }
    else {
      this.staffCodeFromCtrl.setValue("");
      this.staffNameFromCtrl.setValue("");
      HtmlUtil.nextFocusByName(this.elementRef, 'staffCodeToCtrl', eventType);
    }

  }


  public setStaffCodeTo(eventType: string) {

    if(eventType!=EVENT_TYPE.BLUR){
      this.staffCodeToTrigger.closePanel();
    }


    if (!StringUtil.IsNullOrEmpty(this.staffCodeToCtrl.value)) {

      if (this.userInfoService.ApplicationControl.staffCodeType == CODE_TYPE.NUMBER) {
        this.staffCodeToCtrl.setValue(StringUtil.setPaddingFrontZero(this.staffCodeToCtrl.value, this.userInfoService.ApplicationControl.staffCodeLength));
      }      

      this.loadStart();
      this.staffService.GetItems(this.staffCodeToCtrl.value)
        .subscribe(response => {
          this.loadEnd();
          if (response != PROCESS_RESULT_RESULT_TYPE.FAILURE && response.length > 0) {
            this.staffCodeToCtrl.setValue(response[0].code);
            this.staffNameToCtrl.setValue(response[0].name);

            HtmlUtil.nextFocusByName(this.elementRef, 'customerCodeFromCtrl', eventType);
          }
          else {
            //this.staffCodeToCtrl.setValue("");
            this.staffNameToCtrl.setValue("");
            HtmlUtil.nextFocusByName(this.elementRef, 'customerCodeFromCtrl', eventType);
          }
        });
    }
    else {
      this.staffCodeToCtrl.setValue("");
      this.staffNameToCtrl.setValue("");
      HtmlUtil.nextFocusByName(this.elementRef, 'customerCodeFromCtrl', eventType);
    }

  }

  public setCbxStaff(eventType: string) {

    let localstorageItem = new LocalStorageItem();
    localstorageItem.key = RangeSearchKey.PF0301_STAFF;
    localstorageItem.value = this.cbxStaffCtrl.value;
    this.localStorageManageService.set(localstorageItem);

    HtmlUtil.nextFocusByName(this.elementRef, 'staffCodeFromCtrl', eventType);

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

      this.loadStart();
      this.customerService.GetItems(this.customerCodeToCtrl.value)
        .subscribe(response => {
          this.loadEnd();
          if (response != PROCESS_RESULT_RESULT_TYPE.FAILURE && response.length > 0) {
            this.customerCodeToCtrl.setValue(response[0].code);
            this.customerNameToCtrl.setValue(response[0].name);

            HtmlUtil.nextFocusByNames(this.elementRef, ['currencyCodeCtrl','baseDateCtrl'], eventType);
          }
          else {
            //this.customerCodeToCtrl.setValue("");
            this.customerNameToCtrl.setValue("");
            HtmlUtil.nextFocusByNames(this.elementRef, ['currencyCodeCtrl','baseDateCtrl'], eventType);
          }
        });
    }
    else {
      this.customerCodeToCtrl.setValue("");
      this.customerNameToCtrl.setValue("");
      HtmlUtil.nextFocusByNames(this.elementRef, ['currencyCodeCtrl','baseDateCtrl'], eventType);
    }
  }

  public setCbxCustomer(eventType: string) {

    let localstorageItem = new LocalStorageItem();
    localstorageItem.key = RangeSearchKey.PF0301_CUSTOMER;
    localstorageItem.value = this.cbxCustomerCtrl.value;
    this.localStorageManageService.set(localstorageItem);

    HtmlUtil.nextFocusByName(this.elementRef, 'customerCodeFromCtrl', eventType);

  }


}
