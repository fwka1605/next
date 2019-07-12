import { Component, OnInit, ElementRef, ComponentFactoryResolver, ViewContainerRef, AfterViewInit, ViewChild } from '@angular/core';
import { BaseComponent } from 'src/app/component/common/base/base-component';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { FormatterUtil } from 'src/app/common/util/formatter.util';
import { Router, ActivatedRoute, NavigationEnd } from '@angular/router';
import { TABLE_INDEX } from 'src/app/common/const/table-name.const';
import { KEY_CODE, BUTTON_ACTION, EVENT_TYPE } from 'src/app/common/const/event.const';
import { ModalMasterComponent } from 'src/app/component/modal/modal-master/modal-master.component';
import { HtmlUtil } from 'src/app/common/util/html.util';
import { StringUtil } from 'src/app/common/util/string-util';
import { DepartmentMasterService } from 'src/app/service/Master/department-master.service';
import { StaffMasterService } from 'src/app/service/Master/staff-master.service';
import { CustomerMasterService } from 'src/app/service/Master/customer-master.service';
import { ModalFormSettingComponent } from 'src/app/component/modal/modal-form-setting/modal-form-setting.component';
import { CreditAgingListService } from 'src/app/service/credit-aging-list.service';
import { CreditAgingListsResult } from 'src/app/model/credit-aging-lists-result.model';
import { CreditAgingListSearch } from 'src/app/model/credit-aging-list-search.model';
import { CreditAgingList } from 'src/app/model/credit-aging-list.model';
import { MODAL_STATUS_TYPE, PROCESS_RESULT_RESULT_TYPE } from 'src/app/common/const/status.const';
import { UserInfoService } from 'src/app/service/common/user-info.service';

import { NgbDateParserFormatter } from '@ng-bootstrap/ng-bootstrap';
import { NgbDateCustomParserFormatter } from 'src/app/common/util/dateformat-util';
import { CurrencyMasterService } from 'src/app/service/Master/currency-master.service';
import { COMPONENT_INFO } from 'src/app/common/const/component-name.const';
import { ClosingInformation } from 'src/app/model/closing-information.model';
import { ReportSetting } from 'src/app/model/report-setting.model';
import { SettingsResult } from 'src/app/model/settings-result.model';
import { ReportSettingMasterService } from 'src/app/service/Master/report-setting-master.service';
import { ReportSettingsResult } from 'src/app/model/report-settings-result.model';
import { DateAdapter, MAT_DATE_LOCALE, MAT_DATE_FORMATS, MatDatepicker, MatAutocompleteTrigger, MatExpansionPanel } from '@angular/material';
import { MomentDateAdapter } from '@angular/material-moment-adapter';
import { YMPICKER_FORMAT, moment } from 'src/app/common/const/ympicker-format.const';
import { Moment } from 'moment';
import { ClosingService } from 'src/app/service/closing.service';
import { ReportSettigUtil } from 'src/app/common/util/report-setting-util';
import { REPORT_ITEM_ID, ReportStaffSelection, ReportDoOrNot, ReportCustomerGroup, ReportUnitPrice, ReportTargetDate, ReportReceiptType, ReportCreditLimitType, REPORT_HEADER, LINE_FEED_CODE } from 'src/app/common/const/report-setting-kbn.const';
import { PF0201 } from 'src/app/model/report/settings/pf0201';
import { DateUtil, CALENDAR_CONVERT_TYPE } from 'src/app/common/util/date-util';
import { FileUtil } from 'src/app/common/util/file.util';
import { FILE_EXTENSION } from 'src/app/common/const/eb-file.const';
import { ModalRouterProgressComponent } from 'src/app/component/modal/modal-router-progress/modal-router-progress.component';
import { ProcessResultService } from 'src/app/service/common/process-result.service';
import { MSG_ERR, MSG_WNG, MSG_ITEM_NUM, MSG_INF } from 'src/app/common/const/message.const';
import { PageUtil } from 'src/app/common/util/page-util';
import { CODE_TYPE } from 'src/app/common/const/kbn.const';
import { NumberUtil } from 'src/app/common/util/number-util';
import { LocalStorageItem } from 'src/app/model/custom-model/local-storage-item';
import { RangeSearchKey } from 'src/app/common/const/local-storage-key-const';
import { LocalStorageManageService } from 'src/app/service/common/local-storage-manage.service';


@Component({
  selector: 'app-pf0201-credit-aging-list',
  templateUrl: './pf0201-credit-aging-list.component.html',
  styleUrls: ['./pf0201-credit-aging-list.component.css'],
  providers: [
    { provide: NgbDateParserFormatter, useClass: NgbDateCustomParserFormatter },
    { provide: DateAdapter, useClass: MomentDateAdapter, deps: [MAT_DATE_LOCALE] },
    { provide: MAT_DATE_FORMATS, useValue: YMPICKER_FORMAT }
  ]

})
export class Pf0201CreditAgingListComponent extends BaseComponent implements OnInit,AfterViewInit {
  
    // パネルの開閉フラグ
    public panelOpenState: boolean;
  
    public date = new FormControl(moment());
  
    public closingInformation: ClosingInformation;
    public reportSettingsResult: ReportSettingsResult;
  
    public creditAgingListSearch: CreditAgingListSearch;
    public creditAgingListsResult: CreditAgingListsResult;    
  
    public baseMonthCtrl: FormControl; // 処理月
  
    public rdoRemainTypeCtrl: FormControl;  // 残高タイプ
  
    public closingDateCtrl: FormControl;  // 締め日
  
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
  
    public currencyCodeCtrl: FormControl; // 通貨コード

    public cbxFilterPositiveCreditBalanceCtrl:FormControl;  // 与信残高がマイナスの物のみ表示
    public rdoFilterCustomerTypeCtrl:FormControl; // 債権代表者の総計額で判定、子会社の個別残高で判定

    public cbxCalculateCreditLimitRegisteredCtrl:FormControl; // 与信限度額が0のものは、残計算をしない。

    public dispSelectedBaseDateCtrl: FormControl; // 基準日
    public dispSubTotalStaffCtrl: FormControl; // 担当者計
    public dispSubTotalDepartmentCtrl: FormControl; // 部門計
    public dispStaffSelectionCtrl: FormControl; // 担当者集計
    public dispCustomerGroupCtrl: FormControl; // 得意先集計
    public dispUnitPriceCtrl: FormControl; // 金額単位

    public dispReportReceiptTypeCtrl: FormControl; // 債権総額計算
    public dispReportCreditLimitTypeCtrl: FormControl; // 与信限度額集計
    
    public undefineCtrl = new FormControl; // 未定用;  
  
    @ViewChild('departmentCodeFromInput', { read: MatAutocompleteTrigger }) departmentCodeFromTrigger: MatAutocompleteTrigger;
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
      public departmentService: DepartmentMasterService,
      public staffService: StaffMasterService,
      public customerService: CustomerMasterService,
      public creditAgingListService: CreditAgingListService,
      public currencyService: CurrencyMasterService,
      public reportSettingService: ReportSettingMasterService,
      public closingService: ClosingService,
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
      reportSetting.reportId = COMPONENT_INFO.PF0201.name;
  
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
  
      this.creditAgingListsResult = null;
    
    }
  
    ngAfterViewInit(){
      HtmlUtil.nextFocusByName(this.elementRef, 'baseMonthCtrl', EVENT_TYPE.NONE);
    }
  
    public setControlInit() {
  
      this.baseMonthCtrl = new FormControl("", [Validators.required, Validators.maxLength(7)]);  // 処理月
  
      this.rdoRemainTypeCtrl = new FormControl("");  // 残高タイプ
  
      this.closingDateCtrl = new FormControl("", [Validators.maxLength(2)]);  // 締め日
  
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
  
      this.currencyCodeCtrl = new FormControl("", [Validators.maxLength(3)]); // 通貨コード
  
      this.cbxFilterPositiveCreditBalanceCtrl = new FormControl("");// 与信残高がマイナスの物のみ表示
      this.rdoFilterCustomerTypeCtrl = new FormControl("");// 債権代表者の総計算で判定、子会社の個別残高で判定
      this.cbxCalculateCreditLimitRegisteredCtrl = new FormControl(""); // 与信限度額が0のものは、残計算をしない。

      this.undefineCtrl = new FormControl(""); // 未定用
  
      this.dispSelectedBaseDateCtrl = new FormControl(""); // 基準日
      this.dispUnitPriceCtrl = new FormControl(""); // 金額単位
      this.dispSubTotalStaffCtrl = new FormControl(""); // 担当者計
      this.dispSubTotalDepartmentCtrl = new FormControl(""); // 部門計
      this.dispStaffSelectionCtrl = new FormControl(""); // 担当者集計
      this.dispCustomerGroupCtrl = new FormControl(""); // 得意先集計

      this.dispReportReceiptTypeCtrl = new FormControl(""); // 債権総額計算
      this.dispReportCreditLimitTypeCtrl = new FormControl(""); // 与信限度額集計

    }
  
    public setValidator() {
      this.MyFormGroup = new FormGroup({
  
        baseMonthCtrl: this.baseMonthCtrl,  // 処理月
        rdoRemainTypeCtrl: this.rdoRemainTypeCtrl,  // 残高タイプ
        closingDateCtrl: this.closingDateCtrl,  // 締め日
  
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
  
        currencyCodeCtrl: this.currencyCodeCtrl, // 通貨コード;
  
        cbxFilterPositiveCreditBalanceCtrl: this.cbxFilterPositiveCreditBalanceCtrl,// 与信残高がマイナスの物のみ表示
        rdoFilterCustomerTypeCtrl: this.rdoFilterCustomerTypeCtrl,// 債権代表者の総計算で判定、子会社の個別残高で判定
        cbxCalculateCreditLimitRegisteredCtrl: this.cbxCalculateCreditLimitRegisteredCtrl, // 与信限度額が0のものは、残計算をしない。
  

        undefineCtrl: this.undefineCtrl, // 未定用;
  
        dispSelectedBaseDateCtrl: this.dispSelectedBaseDateCtrl, // 基準日;
        dispSubTotalStaffCtrl: this.dispSubTotalStaffCtrl, // 担当者計;
        dispSubTotalDepartmentCtrl: this.dispSubTotalDepartmentCtrl, // 部門計;
        dispStaffSelectionCtrl: this.dispStaffSelectionCtrl, // 担当者集計;
        dispCustomerGroupCtrl: this.dispCustomerGroupCtrl, // 得意先集計;
        dispReportReceiptTypeCtrl:this.dispReportReceiptTypeCtrl, // 債権総額計算
        dispReportCreditLimitTypeCtrl:this.dispReportCreditLimitTypeCtrl, // 与信限度額集計
        dispUnitPriceCtrl: this.dispUnitPriceCtrl, // 金額単位;
  
      });
  
    }
  
    public setFormatter() {
  
      FormatterUtil.setNumberFormatter(this.closingDateCtrl); // 締め日
  
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
  
      FormatterUtil.setAlphabetFormatter(this.currencyCodeCtrl);  // 通貨コード;
  
    }
  
    public setAutoComplete(){
  
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
  
    chosenYearHandler(normalizedYear: Moment) {
      const ctrlValue = this.date.value;
      ctrlValue.year(normalizedYear.year());
      this.date.setValue(ctrlValue);
    }
  
    chosenMonthHandler(normlizedMonth: Moment, datepicker: MatDatepicker<Moment>) {
      const ctrlValue = this.date.value;
      ctrlValue.month(normlizedMonth.month());
      this.baseMonthCtrl.setValue(ctrlValue);
      datepicker.close();
      this.setBaseMonth('keyup')
    }
  
    /**
     * 設定モーダル呼出
     */
    public openFromSettingModal() {
      let componentFactory = this.componentFactoryResolver.resolveComponentFactory(ModalFormSettingComponent);
      let componentRef = this.viewContainerRef.createComponent(componentFactory);
  
      componentRef.instance.ReportId = COMPONENT_INFO.PF0201.name;
      componentRef.instance.processModalCustomResult = this.processModalCustomResult;
  
      componentRef.instance.Closing.subscribe(() => {
        let reportSetting = new ReportSetting();
        reportSetting.companyId = this.userInfoService.Company.id;
        reportSetting.reportId = COMPONENT_INFO.PF0201.name;

        this.reportSettingService.GetItems(reportSetting)
        .subscribe(response => {
          this.reportSettingsResult.reportSettings = response;
          this.setCbxFilterPositiveCreditBalance();
        });        

        componentRef.destroy();
      });
  
    }
  
  public openMasterModal(table: TABLE_INDEX, type: string = null) {

    this.departmentCodeFromTrigger.closePanel();
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
      //this.creditAgingListsResult = null;
      this.rdoRemainTypeCtrl.setValue("0");
  
      this.closingDateCtrl.setValidators([]);
      this.closingDateCtrl.updateValueAndValidity();
      this.closingDateCtrl.disable();

      this.cbxFilterPositiveCreditBalanceCtrl.setValue(false);
      this.rdoFilterCustomerTypeCtrl.setValue("0");
      this.rdoFilterCustomerTypeCtrl.disable();
  
      HtmlUtil.nextFocusByName(this.elementRef, 'baseMonthCtrl', EVENT_TYPE.NONE);
      
      this.setRangeCheckbox();
    }
  
    public setRangeCheckbox() {
      let cbxCustomer = this.localStorageManageService.get(RangeSearchKey.PF0201_CUSTOMER);
      let cbxDepartment = this.localStorageManageService.get(RangeSearchKey.PF0201_DEPARTMENT);
      let cbxStaff = this.localStorageManageService.get(RangeSearchKey.PF0201_STAFF);
  
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
          //this.print();
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
      reportSetting.reportId = COMPONENT_INFO.PF0201.name;
  
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
      //if (!this.validateForSearch()) return;
  
      // 動作中
      let componentFactory = this.componentFactoryResolver.resolveComponentFactory(ModalRouterProgressComponent);
      let componentRef = this.viewContainerRef.createComponent(componentFactory);
      componentRef.instance.Closing.subscribe(() => {
        componentRef.destroy();
      });
    
      this.creditAgingListSearch = this.getSearchOption();
      this.creditAgingListService.Get(this.creditAgingListSearch)
        .subscribe(response => {
          if (response != PROCESS_RESULT_RESULT_TYPE.FAILURE) {
            this.creditAgingListsResult = new CreditAgingListsResult();
            this.creditAgingListsResult.creditAgingLists = response;
  
            if (this.creditAgingListsResult.creditAgingLists == null || this.creditAgingListsResult.creditAgingLists.length == 0) {
              this.panelOpenState = true;
              this.processResultService.processAtWarning(
                this.processCustomResult, MSG_WNG.NOT_EXIST_SEARCH_DATA, this.partsResultMessageComponent);
            }
            else {
              this.panelOpenState = false;
              this.SetSearchOptionInformation();
  
              this.processResultService.processAtSuccess(
                this.processCustomResult, "照会が完了しました。", this.partsResultMessageComponent);
              this.processResultService.createdLog(this.processCustomResult.logData);
  
            }
          }
          else {
            this.panelOpenState = true;
            this.processResultService.processAtFailure(this.processCustomResult,
              MSG_ERR.SOMETHING_ERROR.replace(MSG_ITEM_NUM.FIRST, '債権総額年齢表の取得'),
              this.partsResultMessageComponent);
  
          }
          componentRef.destroy();
        });

    }
  
    public print() {
  
      //if (!this.validateForSearch()) return;
  
      let result: boolean = false;
      let componentFactory = this.componentFactoryResolver.resolveComponentFactory(ModalRouterProgressComponent);
      let componentRef = this.viewContainerRef.createComponent(componentFactory);
      componentRef.instance.Closing.subscribe(() => {
        componentRef.destroy();
      });
  
        this.creditAgingListSearch = this.getSearchOption();

        this.creditAgingListService.GetSpreadsheet(this.creditAgingListSearch)
        .subscribe(response => {
          if (response != PROCESS_RESULT_RESULT_TYPE.FAILURE) {
  
            if (response.body.size > 0) {
              FileUtil.download([response.body], this.Title + DateUtil.getYYYYMMDD(0), FILE_EXTENSION.XLSX);
              result = true;
              this.processResultService.processAtOutput(
                this.processCustomResult, result, 0, this.partsResultMessageComponent);
            }
            else {
              this.processResultService.processAtWarning(
                this.processCustomResult, MSG_WNG.NOT_EXIST_SEARCH_DATA, this.partsResultMessageComponent);
            }
          }
          else {
            this.processResultService.processAtOutput(
              this.processCustomResult, result, 0, this.partsResultMessageComponent);
          }
          componentRef.destroy();
          this.openOptions();
  
        });        
  
    }
  
    /**
     * エクスポート
     */
    public export() {
      let data: string = "";
      let dataList = this.creditAgingListsResult.creditAgingLists;
      let dataItems = Array<string>();
  
      let componentFactory = this.componentFactoryResolver.resolveComponentFactory(ModalRouterProgressComponent);
      let modalRouterProgressComponentRef = this.viewContainerRef.createComponent(componentFactory);
  
      dataItems[0] = FileUtil.encloseItemBySymbol(REPORT_HEADER.CREDIT_AGING_LIST_FORM).toString();
      dataItems[1] = FileUtil.encloseItemBySymbol([this.getDisplayMonth(1) + "月期日到来"]).toString();
      dataItems[2] = FileUtil.encloseItemBySymbol([this.getDisplayMonth(2) + "月期日到来"]).toString();
      dataItems[3] = FileUtil.encloseItemBySymbol([this.getDisplayMonth(3) + "月期日到来"]).toString();
      dataItems[4] = FileUtil.encloseItemBySymbol([this.getDisplayMonth(4) + "月以降期日到来"]).toString();
      data = dataItems.join(',') + LINE_FEED_CODE;
  
      for (let index = 0; index < dataList.length; index++) {
        let dataItem: Array<any> = [];
  
        dataList[index].creditAmount = dataList[index].creditAmount == undefined ? null : NumberUtil.Round(dataList[index].creditAmount);
        dataList[index].unsettledRemain = dataList[index].unsettledRemain == undefined ? null : NumberUtil.Round(dataList[index].unsettledRemain);
        dataList[index].billingRemain = dataList[index].billingRemain == undefined ? null : NumberUtil.Round(dataList[index].billingRemain);
        dataList[index].creditLimit = dataList[index].creditLimit == undefined ? null : NumberUtil.Round(dataList[index].creditLimit);
        dataList[index].creditBalance = dataList[index].creditBalance == undefined ? null : NumberUtil.Round(dataList[index].creditBalance);
        dataList[index].arrivalDueDate1 = dataList[index].arrivalDueDate1 == undefined ? null : NumberUtil.Round(dataList[index].arrivalDueDate1);
        dataList[index].arrivalDueDate2 = dataList[index].arrivalDueDate2 == undefined ? null : NumberUtil.Round(dataList[index].arrivalDueDate2);
        dataList[index].arrivalDueDate3 = dataList[index].arrivalDueDate3 == undefined ? null : NumberUtil.Round(dataList[index].arrivalDueDate3);
        dataList[index].arrivalDueDate4 = dataList[index].arrivalDueDate4 == undefined ? null : NumberUtil.Round(dataList[index].arrivalDueDate4);

        dataItem.push(this.getCustomerCode(dataList[index]));
        dataItem.push(this.getCustomerName(dataList[index]));
        dataItem.push(dataList[index].collectCategory);

        dataItem.push(dataList[index].creditAmount);
        dataItem.push(dataList[index].unsettledRemain);
        dataItem.push(dataList[index].billingRemain);
        dataItem.push(dataList[index].creditLimit);
        dataItem.push(dataList[index].creditBalance);

        dataItem.push(dataList[index].arrivalDueDate1);
        dataItem.push(dataList[index].arrivalDueDate2);
        dataItem.push(dataList[index].arrivalDueDate3);
        dataItem.push(dataList[index].arrivalDueDate4);
  
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
  
      modalRouterProgressComponentRef.destroy();    
    }
  
  
    // public validateForSearch() {
    //   // 繰越年月の確認
    //   let closingDay = this.rdoRemainTypeCtrl.value == "0" ? this.userInfoService.Company.closingDay : parseInt(this.closingDateCtrl.value);
    //   if (this.billingBalancesResult.billingBalances.length > 0) {
  
    //     return false;
    //   }
    //   return true;
    // }

    /**
     * 照会条件のフォームの値取得
     */
    public getSearchOption(): CreditAgingListSearch {
      let option = new CreditAgingListSearch();
      option.companyId = this.userInfoService.Company.id;

      if (!StringUtil.IsNullOrEmpty(this.closingDateCtrl.value)) {
        option.closingDay = parseInt(this.closingDateCtrl.value);
      }
  
      option.yearMonth = this.getYearMonth();
  
      //option.targetDate = parseInt(this.GetReportSettingTargetDate());
  
      const staffType = parseInt(ReportSettigUtil.getItemKey(
        this.reportSettingsResult.reportSettings,
        REPORT_ITEM_ID.ReportStaffSelection,
        PF0201.StaffType));
  
      option.useMasterStaff = staffType == ReportStaffSelection.ByCustomerMaster;
  
      const subStaff = parseInt(ReportSettigUtil.getItemKey(
        this.reportSettingsResult.reportSettings,
        REPORT_ITEM_ID.ReportDoOrNot,
        PF0201.StaffTotal));
      option.requireStaffTotal = subStaff == ReportDoOrNot.Do;
  
      const subDept = parseInt(ReportSettigUtil.getItemKey(
        this.reportSettingsResult.reportSettings,
        REPORT_ITEM_ID.ReportDoOrNot,
        PF0201.DepartmentTotal));
      option.requireDepartmentTotal = subDept == ReportDoOrNot.Do;
  
      const group = parseInt(ReportSettigUtil.getItemKey(
        this.reportSettingsResult.reportSettings,
        REPORT_ITEM_ID.ReportCustomerGroup,
        PF0201.CustomerGroupType));
      option.considerCustomerGroup = group != ReportCustomerGroup.PlainCusomter;
  
      const unit = parseInt(ReportSettigUtil.getItemKey(
        this.reportSettingsResult.reportSettings,
        REPORT_ITEM_ID.ReportUnitPrice,
        PF0201.UnitPrice));
      option.unitPrice =
        this.userInfoService.ApplicationControl.useForeignCurrency ? 1 :
          unit == ReportUnitPrice.Per1 ? 1 :
            unit == ReportUnitPrice.Per1000 ? 1000 :
              unit == ReportUnitPrice.Per10000 ? 10000 :
                unit == ReportUnitPrice.Per1000000 ? 1000000 : 1;
  
  
      if (!StringUtil.IsNullOrEmpty(this.departmentCodeFromCtrl.value))
        option.departmentCodeFrom = this.departmentCodeFromCtrl.value;
  
      if (!StringUtil.IsNullOrEmpty(this.departmentCodeToCtrl.value))
        option.departmentCodeTo = this.departmentCodeToCtrl.value;
  
      if (!StringUtil.IsNullOrEmpty(this.staffCodeFromCtrl.value))
        option.staffCodeFrom = this.staffCodeFromCtrl.value;
  
      if (!StringUtil.IsNullOrEmpty(this.staffCodeToCtrl.value))
        option.staffCodeTo = this.staffCodeToCtrl.value;
  
      if (!StringUtil.IsNullOrEmpty(this.customerCodeFromCtrl.value))
        option.customerCodeFrom = this.customerCodeFromCtrl.value;
  
      if (!StringUtil.IsNullOrEmpty(this.customerCodeToCtrl.value))
        option.customerCodeTo = this.customerCodeToCtrl.value;
  
      option.filterPositiveCreditBalance = this.cbxFilterPositiveCreditBalanceCtrl.value;
      option.considerGroupWithCalculate = this.cbxFilterPositiveCreditBalanceCtrl.value && this.rdoFilterCustomerTypeCtrl.value == 0;
      option.calculateCreditLimitRegistered = this.cbxCalculateCreditLimitRegisteredCtrl.value;

      const creditLimitType = parseInt(ReportSettigUtil.getItemKey(
        this.reportSettingsResult.reportSettings,
        REPORT_ITEM_ID.ReportCreditLimitType,
        PF0201.CreditLimitType));      

      option.useParentCustomerCredit = creditLimitType == ReportCreditLimitType.UseParentCustomerCredit;

      return option;
    }
  
    public GetReportSettingTargetDate(): string {
      if (this.closingInformation.useClosing) {
        return "";
      }
      else {
        return ReportSettigUtil.getItemKey(
          this.reportSettingsResult.reportSettings,
          REPORT_ITEM_ID.ReportTargetDate,
          PF0201.TargetDate
        );
      }
    }
  
  
    /**
     * 照会結果のフォームに値を設定する
     */
    public SetSearchOptionInformation() {

      this.dispSelectedBaseDateCtrl.setValue(
        this.creditAgingListSearch.useBilledAt ? "請求日" : "売上日"
      );

      this.dispSubTotalStaffCtrl.setValue(
        this.creditAgingListSearch.requireStaffTotal ? "する" : "しない"
      )
  
      this.dispSubTotalDepartmentCtrl.setValue(
        this.creditAgingListSearch.requireDepartmentTotal ? "する" : "しない"
      );
  
      this.dispStaffSelectionCtrl.setValue(
        this.creditAgingListSearch.useMasterStaff ? "得意先マスタ" : "請求データ"
      );
  
      const group = parseInt(ReportSettigUtil.getItemKey(
        this.reportSettingsResult.reportSettings,
        REPORT_ITEM_ID.ReportCustomerGroup,
        PF0201.CustomerGroupType));
  
      this.dispCustomerGroupCtrl.setValue(
        group == ReportCustomerGroup.PlainCusomter ? "得意先" :
          group == ReportCustomerGroup.ParentWithChildren ? "債権代表者/得意先" :
            group == ReportCustomerGroup.ParentOnly ? "債権代表者" : ""
      );
  
      const receiptType = parseInt(ReportSettigUtil.getItemKey(
        this.reportSettingsResult.reportSettings,
        REPORT_ITEM_ID.ReportReceiptType,
        PF0201.ReceiptType));

      this.dispReportReceiptTypeCtrl.setValue(
        receiptType == ReportReceiptType.UseMatchingAmount ? "消込額を使用する" :
          receiptType == ReportReceiptType.UseReceiptAmount ? "入金額を使用する" : ""
      );

      this.dispReportCreditLimitTypeCtrl.setValue(
        this.creditAgingListSearch.useParentCustomerCredit  ? "債権代表者" : "得意先"
      );

      const unit = parseInt(ReportSettigUtil.getItemKey(
        this.reportSettingsResult.reportSettings,
        REPORT_ITEM_ID.ReportUnitPrice,
        PF0201.UnitPrice));
  
      this.dispUnitPriceCtrl.setValue(
        unit == ReportUnitPrice.Per1 ? "円" :
          unit == ReportUnitPrice.Per1000 ? "千円" :
            unit == ReportUnitPrice.Per10000 ? "万円" :
              unit == ReportUnitPrice.Per1000000 ? "百万円" : ""
      );
    }

    public getDisplayMonth(pastMonth: number) {
      if (StringUtil.IsNullOrEmpty(this.baseMonthCtrl.value)) return "XX";
      let month: Moment = this.baseMonthCtrl.value.clone();
      return month.add("M", pastMonth).format("MM");
    }
  
    ///////////////////////////////////////////////////////
    public setBaseMonth(eventType: string) {
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
        this.closingDateCtrl.setValue("");
        this.closingDateCtrl.setValidators([]);
        this.closingDateCtrl.updateValueAndValidity();
        this.closingDateCtrl.disable();
        HtmlUtil.nextFocusByName(this.elementRef, 'departmentCodeFromCtrl', eventType);
      }
    }
  
    ///////////////////////////////////////////////////////
    public setClosingDate(eventType: string) {
  
      if (this.closingDateCtrl.value != undefined && !StringUtil.IsNullOrEmpty(this.closingDateCtrl.value)) {
        let closingDay:number = parseInt(this.closingDateCtrl.value);
        if (closingDay < 1 || 28 <= closingDay) {
          closingDay = 99;
        }
        this.closingDateCtrl.setValue(StringUtil.setPaddingFrontZero(closingDay.toString(), 2, true));
      }
      HtmlUtil.nextFocusByName(this.elementRef, 'departmentCodeFromCtrl', eventType);
    }
  
    /////////////////////////////////////////////////////////////////////////////////
  
    public setDepartmentCodeFrom(eventType: string) {
  
      if(eventType!=EVENT_TYPE.BLUR){
        this.departmentCodeFromTrigger.closePanel();
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
      localstorageItem.key = RangeSearchKey.PF0201_DEPARTMENT;
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
      localstorageItem.key = RangeSearchKey.PF0201_STAFF;
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
      localstorageItem.key = RangeSearchKey.PF0201_CUSTOMER;
      localstorageItem.value = this.cbxCustomerCtrl.value;
      this.localStorageManageService.set(localstorageItem);
  
      HtmlUtil.nextFocusByName(this.elementRef, 'customerCodeFromCtrl', eventType);
  
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
              HtmlUtil.nextFocusByName(this.elementRef, 'baseDateCtrl', eventType);
            }
            else {
              this.currencyCodeCtrl.setValue("");
              HtmlUtil.nextFocusByName(this.elementRef, 'baseDateCtrl', eventType);
            }
          });
      }
      else {
        this.currencyCodeCtrl.setValue("");
        HtmlUtil.nextFocusByName(this.elementRef, 'baseDateCtrl', eventType);
      }
  
    }

  /////////////////////////////////////////////////////////////////////////////////
  public getCustomerCode(creditAgingList: CreditAgingList): string {

    let code = "";

    switch (creditAgingList.recordType) {
      case 0:
          code = creditAgingList.customerCode;
        break;
      case 1:
        code = creditAgingList.staffCode;
        break;
      case 2:
        code = creditAgingList.departmentCode;
        break;
      default:
        code = "";
    }

    return code;
  }    
  
    /////////////////////////////////////////////////////////////////////////////////
    public getCustomerName(creditAgingList: CreditAgingList): string {
  
      let name = "";
  
      if (creditAgingList.customerName == undefined) {
        switch (creditAgingList.recordType) {
          case 1:
            name = creditAgingList.staffName + "　【担当計】";
            break;
          case 2:
            name = creditAgingList.departmentName + "　【部門計】";
            break;
          case 3:
            name = "総合計";
            break;
        }        
      }
      else if (creditAgingList.parentCustomerId != undefined && creditAgingList.parentCustomerFlag == 0){
        name = " *" + creditAgingList.customerName;
      }
      else {
        name = creditAgingList.customerName;
      }
  
      return name;
    }

    public getYearMonth(): string {
  
      let tmpDate: string;
      tmpDate = DateUtil.ConvertFromMomentToIso(this.baseMonthCtrl);
      tmpDate = DateUtil.ConvertFromIso(tmpDate, CALENDAR_CONVERT_TYPE.HOUR_START);
  
      let date = new Date(tmpDate);
      let day: number;
      if (this.closingDateCtrl.value != undefined) {
        day = this.closingDateCtrl.value;
      }
      else {
        day = this.userInfoService.Company.closingDay;
      }
  
      if (day < 1 || 28 < day) {
        day = new Date(date.getFullYear(), date.getMonth() + 1, 0).getDate();
      }
  
      let result = tmpDate.substr(0, 8) + StringUtil.setPaddingFrontZero(day.toString(), 2, true) + tmpDate.substr(10,14);
  
      return result;
    }

    public setCbxFilterPositiveCreditBalance() {

      const group = parseInt(ReportSettigUtil.getItemKey(
        this.reportSettingsResult.reportSettings,
        REPORT_ITEM_ID.ReportCustomerGroup,
        PF0201.CustomerGroupType));
      
      const creditLimitType = parseInt(ReportSettigUtil.getItemKey(
        this.reportSettingsResult.reportSettings,
        REPORT_ITEM_ID.ReportCreditLimitType,
        PF0201.CreditLimitType));

      if (this.cbxFilterPositiveCreditBalanceCtrl.value &&
          group != ReportCustomerGroup.PlainCusomter &&
          creditLimitType == ReportCreditLimitType.UseCustomerSummaryCredit) {
            
            this.rdoFilterCustomerTypeCtrl.enable();
      }
      else {
        this.rdoFilterCustomerTypeCtrl.setValue("0");
        this.rdoFilterCustomerTypeCtrl.disable();
      }

    }

  }
  