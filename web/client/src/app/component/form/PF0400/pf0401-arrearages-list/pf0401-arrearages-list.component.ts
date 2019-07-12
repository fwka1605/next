import { Component, OnInit, ElementRef, ComponentFactoryResolver, ViewContainerRef, AfterViewInit, ViewChild } from '@angular/core';
import { BaseComponent } from 'src/app/component/common/base/base-component';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Router, ActivatedRoute, NavigationEnd } from '@angular/router';
import { TABLE_INDEX } from 'src/app/common/const/table-name.const';
import { KEY_CODE, EVENT_TYPE, BUTTON_ACTION } from 'src/app/common/const/event.const';
import { ModalMasterComponent } from 'src/app/component/modal/modal-master/modal-master.component';
import { HtmlUtil } from 'src/app/common/util/html.util';
import { StringUtil } from 'src/app/common/util/string-util';
import { DepartmentMasterService } from 'src/app/service/Master/department-master.service';
import { StaffMasterService } from 'src/app/service/Master/staff-master.service';
import { CustomerMasterService } from 'src/app/service/Master/customer-master.service';
import { ModalFormSettingComponent } from 'src/app/component/modal/modal-form-setting/modal-form-setting.component';
import { MODAL_STATUS_TYPE, PROCESS_RESULT_RESULT_TYPE } from 'src/app/common/const/status.const';
import { FormatterUtil } from 'src/app/common/util/formatter.util';
import { UserInfoService } from 'src/app/service/common/user-info.service';
import { NgbDateParserFormatter, NgbCalendar } from '@ng-bootstrap/ng-bootstrap';
import { NgbDateCustomParserFormatter } from 'src/app/common/util/dateformat-util';
import { CurrencyMasterService } from 'src/app/service/Master/currency-master.service';
import { COMPONENT_INFO } from 'src/app/common/const/component-name.const';
import { ColumnNameSettingsResult } from 'src/app/model/column-name-settings-result.model';
import { ColumnNameSettingMasterService } from 'src/app/service/Master/column-name-setting-master.service';
import { CategoryType, CODE_TYPE } from 'src/app/common/const/kbn.const';
import { ColumnNameSettigUtil } from 'src/app/common/util/column-name-setting-util';
import { ArrearagesListSearch } from 'src/app/model/arrearages-list-search.model';
import { ModalRouterProgressComponent } from 'src/app/component/modal/modal-router-progress/modal-router-progress.component';
import { ArrearagesListsResult } from 'src/app/model/arrearages-lists-result.model';
import { ReportService } from 'src/app/service/report.service';
import { ReportSettingMasterService } from 'src/app/service/Master/report-setting-master.service';
import { ReportSetting } from 'src/app/model/report-setting.model';
import { ReportSettingsResult } from 'src/app/model/report-settings-result.model';
import { FILE_EXTENSION } from 'src/app/common/const/eb-file.const';
import { FileUtil } from 'src/app/common/util/file.util';
import { DateUtil } from 'src/app/common/util/date-util';
import { REPORT_HEADER, LINE_FEED_CODE } from 'src/app/common/const/report-setting-kbn.const';
import { ProcessResultService } from 'src/app/service/common/process-result.service';
import { MSG_WNG, MSG_ERR, MSG_ITEM_NUM, MSG_INF } from 'src/app/common/const/message.const';
import { PageUtil } from 'src/app/common/util/page-util';
import { NumberUtil } from 'src/app/common/util/number-util';
import { LocalStorageItem } from 'src/app/model/custom-model/local-storage-item';
import { RangeSearchKey } from 'src/app/common/const/local-storage-key-const';
import { LocalStorageManageService } from 'src/app/service/common/local-storage-manage.service';
import { MatAutocompleteTrigger, MatExpansionPanel } from '@angular/material';

@Component({
  selector: 'app-pf0401-arrearages-list',
  templateUrl: './pf0401-arrearages-list.component.html',
  styleUrls: ['./pf0401-arrearages-list.component.css'],
  providers: [
    { provide: NgbDateParserFormatter, useClass: NgbDateCustomParserFormatter }
  ]

})
export class Pf0401ArrearagesListComponent extends BaseComponent implements OnInit,AfterViewInit {

  // パネルの開閉フラグ
  public panelOpenState: boolean;

  public columnNameSettingsResult: ColumnNameSettingsResult;

  public arrearagesListsResult: ArrearagesListsResult;
  public arrearagesListSearch: ArrearagesListSearch;

  public sumAmount: number; // 合計

  public paymentBaseDateCtrl: FormControl; // 入金基準日

  public cbxCustomerCtrl: FormControl;  // 得意先毎に集計

  public cbxMemoCtrl: FormControl;  // メモ
  public memoCtrl: FormControl;

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

  public undefineCtrl = new FormControl; // 未定用;  

  @ViewChild('departmentCodeFromInput', { read: MatAutocompleteTrigger }) departmentCodeFromigger: MatAutocompleteTrigger;
  @ViewChild('departmentCodeToInput', { read: MatAutocompleteTrigger }) departmentCodeToigger: MatAutocompleteTrigger;
  @ViewChild('staffCodeFromInput', { read: MatAutocompleteTrigger }) staffCodeFromTrigger: MatAutocompleteTrigger;
  @ViewChild('staffCodeToInput', { read: MatAutocompleteTrigger }) staffCodeToTrigger: MatAutocompleteTrigger;

  @ViewChild('panel') panel:MatExpansionPanel;

  constructor(
    public router: Router,
    public activatedRoute: ActivatedRoute,
    public elementRef: ElementRef,
    public componentFactoryResolver: ComponentFactoryResolver,
    public viewContainerRef: ViewContainerRef,
    public userInfoService: UserInfoService,
    public columnNameSettingService: ColumnNameSettingMasterService,
    public departmentService: DepartmentMasterService,
    public staffService: StaffMasterService,
    public customerService: CustomerMasterService,
    public currencyService: CurrencyMasterService,
    public reportService: ReportService,
    public reportSettingService: ReportSettingMasterService,
    public calendar: NgbCalendar,
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

    this.columnNameSettingService.Get(CategoryType.Billing)
      .subscribe(response => {
        this.columnNameSettingsResult = new ColumnNameSettingsResult();
        this.columnNameSettingsResult.columnNames = response;
      })
  }

  ngAfterViewInit(){
    HtmlUtil.nextFocusByName(this.elementRef, 'paymentBaseDateCtrl', EVENT_TYPE.NONE);
  }

  public setControlInit() {

    this.paymentBaseDateCtrl = new FormControl("", [Validators.required, Validators.maxLength(10)]);  // 入金基準日

    this.cbxCustomerCtrl = new FormControl("");  // 得意先毎に集計

    this.cbxMemoCtrl = new FormControl("");  // メモ
    this.memoCtrl = new FormControl("", [Validators.maxLength(100)]);

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

    this.undefineCtrl = new FormControl(""); // 未定用

  }

  public setValidator() {
    this.MyFormGroup = new FormGroup({

      paymentBaseDateCtrl: this.paymentBaseDateCtrl,  // 入金基準日

      cbxCustomerCtrl: this.cbxCustomerCtrl,  // 得意先毎に集計

      cbxMemoCtrl: this.cbxMemoCtrl,  // メモ
      memoCtrl: this.memoCtrl,

      currencyCodeCtrl: this.currencyCodeCtrl,   // 通貨コード

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

      undefineCtrl: this.undefineCtrl, // 未定用;

    });

  }

  public setFormatter() {

    FormatterUtil.setAlphabetFormatter(this.currencyCodeCtrl); // 通貨コード

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

  }


  public setAutoComplete(){

    // 請求部門
    this.initAutocompleteDepartments(this.departmentCodeFromCtrl,this.departmentService,0);
    this.initAutocompleteDepartments(this.departmentCodeToCtrl,this.departmentService,1);
    // 担当者
    this.initAutocompleteStaffs(this.staffCodeFromCtrl,this.staffService,0);
    this.initAutocompleteStaffs(this.staffCodeToCtrl,this.staffService,1);

  }  

  public openFromSettingModal() {

    let componentFactory = this.componentFactoryResolver.resolveComponentFactory(ModalFormSettingComponent);
    let componentRef = this.viewContainerRef.createComponent(componentFactory);

    componentRef.instance.ReportId = COMPONENT_INFO.PF0401.name;
    componentRef.instance.processModalCustomResult = this.processModalCustomResult;

    componentRef.instance.Closing.subscribe(() => {
      componentRef.destroy();
    });

  }

  public openMasterModal(table: TABLE_INDEX, type: string = null) {

    this.departmentCodeFromigger.closePanel();
    this.departmentCodeToigger.closePanel();
    this.staffCodeFromTrigger.closePanel();
    this.staffCodeToTrigger.closePanel();

    let componentFactory = this.componentFactoryResolver.resolveComponentFactory(ModalMasterComponent);
    let componentRef = this.viewContainerRef.createComponent(componentFactory);

    componentRef.instance.TableIndex = table;

    componentRef.instance.Closing.subscribe(() => {

      if (componentRef.instance.ModalStatus == MODAL_STATUS_TYPE.SELECT) {
        switch (table) {

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

        }
      }

      componentRef.destroy();
    });
  }

  public clear() {
    this.panelOpenState = true;
    this.panel.open();
    
    this.MyFormGroup.reset();

    //this.memoCtrl.disable();

    //this.arrearagesListsResult = null;

    this.paymentBaseDateCtrl.setValue(this.calendar.getToday());

    HtmlUtil.nextFocusByName(this.elementRef, 'paymentBaseDateCtrl', EVENT_TYPE.NONE);

    this.sumAmount=0;

    this.setRangeCheckbox();
  }


  public setRangeCheckbox() {
    let cbxDepartment = this.localStorageManageService.get(RangeSearchKey.PF0401_DEPARTMENT);
    let cbxStaff = this.localStorageManageService.get(RangeSearchKey.PF0401_STAFF);

    if (cbxDepartment != null) {
      this.cbxDepartmentCtrl.setValue(cbxDepartment.value);
    }

    if (cbxStaff != null) {
      this.cbxStaffCtrl.setValue(cbxStaff.value);
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
  public search() {
    let componentFactory = this.componentFactoryResolver.resolveComponentFactory(ModalRouterProgressComponent);
    let componentRef = this.viewContainerRef.createComponent(componentFactory);
    componentRef.instance.Closing.subscribe(() => {
      componentRef.destroy();
    });

    this.arrearagesListSearch = this.GetSearchDataCondition();

    this.reportService.ArrearagesList(this.arrearagesListSearch)
      .subscribe(response => {
        if (response != PROCESS_RESULT_RESULT_TYPE.FAILURE) {
          this.arrearagesListsResult = new ArrearagesListsResult();

          this.arrearagesListsResult.arrearagesLists = response;

          if (this.arrearagesListsResult.arrearagesLists == null
            || this.arrearagesListsResult.arrearagesLists.length == 0
          ) {
            this.panelOpenState = true;
            this.processResultService.processAtWarning(
              this.processCustomResult, MSG_WNG.NOT_EXIST_SEARCH_DATA, this.partsResultMessageComponent);
          }
          else {
            this.panelOpenState = false;
            let tmpAmount = 0;
            this.arrearagesListsResult.arrearagesLists.forEach(element => {
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
            MSG_ERR.SOMETHING_ERROR.replace(MSG_ITEM_NUM.FIRST, '滞留明細一覧表の検索'),
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

    let result: boolean = false;

    // API呼出引数
    let reportSetting = new ReportSetting();
    reportSetting.companyId = this.userInfoService.Company.id;
    reportSetting.reportId = COMPONENT_INFO.PF0401.name;

    let reportSettingsResult: ReportSettingsResult;
    this.reportSettingService.GetItems(reportSetting)
      .subscribe(response => {
        if (response != PROCESS_RESULT_RESULT_TYPE.FAILURE) {
          reportSettingsResult = new ReportSettingsResult();
          reportSettingsResult.reportSettings = response;
  
          this.arrearagesListSearch = this.GetSearchDataCondition();
  
          this.arrearagesListSearch.reportSettings = reportSettingsResult.reportSettings;
  
          this.reportService.getArrearagesShpreadsheet(this.arrearagesListSearch)
            .subscribe(response => {
              if (response != PROCESS_RESULT_RESULT_TYPE.FAILURE) {
                FileUtil.download([response.body], this.Title + DateUtil.getYYYYMMDD(0), FILE_EXTENSION.XLSX);                
                result = true;
                this.processResultService.processAtOutput(
                  this.processCustomResult, result, 0, this.partsResultMessageComponent);
              }
              else {
                this.processResultService.processAtOutput(
                  this.processCustomResult, result, 0, this.partsResultMessageComponent);
              }
              componentRef.destroy();
              this.openOptions();

            });
        }
        else{
          this.processResultService.processAtOutput(
            this.processCustomResult, result, 0, this.partsResultMessageComponent);
          componentRef.destroy();
          this.openOptions();
          }

      });

  }


  /**
   * エクスポート処理
   */
  public export() {
    let data: string = "";
    let dataList = this.arrearagesListsResult.arrearagesLists;
    let dataItems = Array<string>();

    let componentFactory = this.componentFactoryResolver.resolveComponentFactory(ModalRouterProgressComponent);
    let componentRef = this.viewContainerRef.createComponent(componentFactory);
    // componentRef.destroy();

    dataItems[0] = FileUtil.encloseItemBySymbol(REPORT_HEADER.ARREARAGES_LIST_FORM_P1).toString();
    dataItems[1] = FileUtil.encloseItemBySymbol([ColumnNameSettigUtil.getColumnAlias(this.columnNameSettingsResult.columnNames, "Note1")]).toString();
    dataItems[2] = FileUtil.encloseItemBySymbol(REPORT_HEADER.ARREARAGES_LIST_FORM_P2).toString();
    dataItems[3] = FileUtil.encloseItemBySymbol([ColumnNameSettigUtil.getColumnAlias(this.columnNameSettingsResult.columnNames, "Note2")]).toString();
    dataItems[4] = FileUtil.encloseItemBySymbol([ColumnNameSettigUtil.getColumnAlias(this.columnNameSettingsResult.columnNames, "Note3")]).toString();
    dataItems[5] = FileUtil.encloseItemBySymbol([ColumnNameSettigUtil.getColumnAlias(this.columnNameSettingsResult.columnNames, "Note4")]).toString();
    data = dataItems.join(',') + LINE_FEED_CODE;

    for (let index = 0; index < dataList.length; index++) {
      let dataItem: Array<any> = [];

      dataList[index].remainAmount = NumberUtil.Round(dataList[index].remainAmount);

      dataItem.push(DateUtil.convertDateString(dataList[index].baseDate));
      dataItem.push(dataList[index].id);
      dataItem.push(dataList[index].customerCode);
      dataItem.push(dataList[index].customerName);

      dataItem.push(DateUtil.convertDateString(dataList[index].billedAt));
      dataItem.push(DateUtil.convertDateString(dataList[index].salesAt));
      dataItem.push(DateUtil.convertDateString(dataList[index].closingAt));
      dataItem.push(DateUtil.convertDateString(dataList[index].dueAt));
      dataItem.push(DateUtil.convertDateString(dataList[index].originalDueAt));

      dataItem.push(dataList[index].remainAmount);
      dataItem.push(dataList[index].arrearagesDayCount);
      dataItem.push(dataList[index].collectCategoryCode + ":" + dataList[index].collectCategoryName);
      dataItem.push(dataList[index].invoiceCode);
      dataItem.push(dataList[index].note1);
      dataItem.push(dataList[index].memo);
      dataItem.push(dataList[index].customerStaffName);
      dataItem.push(dataList[index].customerNote);
      dataItem.push(dataList[index].tel);
      dataItem.push(dataList[index].departmentCode);
      dataItem.push(dataList[index].departmentName);
      dataItem.push(dataList[index].staffCode);
      dataItem.push(dataList[index].staffName);
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

    this.openOptions();

    componentRef.destroy();

  }


  public GetSearchDataCondition(): ArrearagesListSearch {
    var arrearagesListSearch = new ArrearagesListSearch();

    arrearagesListSearch.companyId = this.userInfoService.Company.id;

    arrearagesListSearch.baseDate = DateUtil.ConvertFromDatepicker(this.paymentBaseDateCtrl);

    if (this.cbxMemoCtrl.value) {
      arrearagesListSearch.existsMemo = true;
    }
    else {
      arrearagesListSearch.existsMemo = false;
    }

    if (!StringUtil.IsNullOrEmpty(this.memoCtrl.value)) {
      arrearagesListSearch.billingMemo = this.memoCtrl.value;
    }

    if (!StringUtil.IsNullOrEmpty(this.currencyCodeCtrl.value)) {
      arrearagesListSearch.currencyCode = this.currencyCodeCtrl.value;
    }
    else {
      arrearagesListSearch.currencyCode = this.userInfoService.Currency.code;
    }

    if (!StringUtil.IsNullOrEmpty(this.departmentCodeFromCtrl.value)) {
      arrearagesListSearch.departmentCodeFrom = this.departmentCodeFromCtrl.value;
    }

    if (!StringUtil.IsNullOrEmpty(this.departmentCodeToCtrl.value)) {
      arrearagesListSearch.departmentCodeTo = this.departmentCodeToCtrl.value;
    }

    if (!StringUtil.IsNullOrEmpty(this.staffCodeFromCtrl.value)) {
      arrearagesListSearch.staffCodeFrom = this.staffCodeFromCtrl.value;
    }

    if (!StringUtil.IsNullOrEmpty(this.staffCodeToCtrl.value)) {
      arrearagesListSearch.staffCodeTo = this.staffCodeToCtrl.value;
    }

    if (this.cbxCustomerCtrl.value) {
      arrearagesListSearch.customerSummaryFlag = true;
    }
    else {
      arrearagesListSearch.customerSummaryFlag = false;
    }

    return arrearagesListSearch;
  }

  ///////////////////////////////////////////////////////
  public setPaymentBaseDate(eventType:string){
    HtmlUtil.nextFocusByNames(this.elementRef, ['memoCtrl','currencyCodeCtrl','departmentCodeFromCtrl'], eventType);
  }

  ///////////////////////////////////////////////////////
  public setCbxMemo(eventType: string) {

    if (this.cbxMemoCtrl.value == true) {
      //this.memoCtrl.enable();
      HtmlUtil.nextFocusByName(this.elementRef, 'memoCtrl', eventType);
    }
    else {
      this.memoCtrl.setValue("");
      //this.memoCtrl.disable();
    }
  }

  ///////////////////////////////////////////////////////
  public setMemo(eventType:string){
    HtmlUtil.nextFocusByNames(this.elementRef, ['currencyCodeCtrl','departmentCodeFromCtrl'], eventType);
  }

  /////////////////////////////////////////////////////////////////////////////////
  public setCurrencyCode(eventType: string) {

    if (!StringUtil.IsNullOrEmpty(this.currencyCodeCtrl.value)) {

      this.currencyService.GetItems(this.currencyCodeCtrl.value)
        .subscribe(response => {
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
    localstorageItem.key = RangeSearchKey.PF0401_DEPARTMENT;
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

            HtmlUtil.nextFocusByName(this.elementRef, 'paymentBaseDateCtrl', eventType);
          }
          else {
            //this.staffCodeToCtrl.setValue("");
            this.staffNameToCtrl.setValue("");
            HtmlUtil.nextFocusByName(this.elementRef, 'paymentBaseDateCtrl', eventType);
          }
        });
    }
    else {
      this.staffCodeToCtrl.setValue("");
      this.staffNameToCtrl.setValue("");
      HtmlUtil.nextFocusByName(this.elementRef, 'paymentBaseDateCtrl', eventType);
    }

  }

  public setCbxStaff(eventType: string) {

    let localstorageItem = new LocalStorageItem();
    localstorageItem.key = RangeSearchKey.PF0401_STAFF;
    localstorageItem.value = this.cbxStaffCtrl.value;
    this.localStorageManageService.set(localstorageItem);

    HtmlUtil.nextFocusByName(this.elementRef, 'staffCodeFromCtrl', eventType);

  }


}
