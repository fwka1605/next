import { Component, OnInit, ElementRef, ComponentFactoryResolver, ViewContainerRef, AfterViewInit, ViewChild } from '@angular/core';
import { BaseComponent } from 'src/app/component/common/base/base-component';
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
import { MODAL_STATUS_TYPE, PROCESS_RESULT_RESULT_TYPE } from 'src/app/common/const/status.const';
import { UserInfoService } from 'src/app/service/common/user-info.service';
import { DateAdapter, MAT_DATE_LOCALE, MAT_DATE_FORMATS, MatDatepicker, MatAutocompleteTrigger, MatExpansionPanel } from '@angular/material';
import { MomentDateAdapter } from '@angular/material-moment-adapter';
import { YMPICKER_FORMAT, moment } from 'src/app/common/const/ympicker-format.const';
import { Moment } from 'moment';
import { CollectionScheduleSearch } from 'src/app/model/collection-schedule-search.model';
import { CollectionSchedulesResult } from 'src/app/model/collection-schedules-result.model';
import { ReportDoOrNot, ReportStaffSelection, ReportSubtotalUnit, REPORT_ITEM_ID, ReportUnitPrice, REPORT_HEADER, LINE_FEED_CODE } from 'src/app/common/const/report-setting-kbn.const';
import { ReportSettigUtil } from 'src/app/common/util/report-setting-util';
import { ReportSettingsResult } from 'src/app/model/report-settings-result.model';
import { ReportSettingMasterService } from 'src/app/service/Master/report-setting-master.service';
import { ReportSetting } from 'src/app/model/report-setting.model';
import { SettingsResult } from 'src/app/model/settings-result.model';
import { PF0601 } from 'src/app/model/report/settings/pf0601';
import { CollectionScheduleService } from 'src/app/service/collection-schedule.service';
import { ModalRouterProgressComponent } from 'src/app/component/modal/modal-router-progress/modal-router-progress.component';
import { FileUtil } from 'src/app/common/util/file.util';
import { DateUtil } from 'src/app/common/util/date-util';
import { FILE_EXTENSION } from 'src/app/common/const/eb-file.const';
import { ProcessResultService } from 'src/app/service/common/process-result.service';
import { MSG_WNG, MSG_ERR, MSG_ITEM_NUM, MSG_INF } from 'src/app/common/const/message.const';
import { Router, ActivatedRoute, NavigationEnd } from '@angular/router';
import { PageUtil } from 'src/app/common/util/page-util';
import { COMPONENT_INFO } from 'src/app/common/const/component-name.const';
import { FormatterUtil } from 'src/app/common/util/formatter.util';
import { CODE_TYPE } from 'src/app/common/const/kbn.const';
import { NumberUtil } from 'src/app/common/util/number-util';
import { LocalStorageItem } from 'src/app/model/custom-model/local-storage-item';
import { RangeSearchKey } from 'src/app/common/const/local-storage-key-const';
import { LocalStorageManageService } from 'src/app/service/common/local-storage-manage.service';
import { CollectionSchedule } from 'src/app/model/collection-schedule.model';

@Component({
  selector: 'app-pf0601-collection-schedule',
  templateUrl: './pf0601-collection-schedule.component.html',
  styleUrls: ['./pf0601-collection-schedule.component.css'],
  providers: [
    { provide: DateAdapter, useClass: MomentDateAdapter, deps: [MAT_DATE_LOCALE] },
    { provide: MAT_DATE_FORMATS, useValue: YMPICKER_FORMAT }
  ]

})
export class Pf0601CollectionScheduleComponent extends BaseComponent implements OnInit, AfterViewInit {

  // パネルの開閉フラグ
  public panelOpenState: boolean;

  public Math: typeof Math = Math;

  public date = new FormControl(moment());

  public collectionSchedulesResult: CollectionSchedulesResult;
  public collectionScheduleSearch: CollectionScheduleSearch;

  public reportSettingsResult: ReportSettingsResult;

  // これをしないとEnum型がテンプレートで見えなかったため
  public TABLE_INDEX: typeof TABLE_INDEX = TABLE_INDEX;

  public baseMonthCtrl: FormControl; // 指定基準月

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

  public dispSubtotalUnitCtrl: FormControl;  // 得意先集計方法
  public dispStaffSelectionCtrl: FormControl;  // 担当者集計方法
  public displPriceUnitCtrl: FormControl;  // 金額単位

  public undefineCtrl: FormControl; // 未定用


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
    public reportSettingService: ReportSettingMasterService,
    public departmentService: DepartmentMasterService,
    public staffService: StaffMasterService,
    public customerService: CustomerMasterService,
    public collectionScheduleService: CollectionScheduleService,
    public processResultService: ProcessResultService,
    public localStorageManageService: LocalStorageManageService
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
    this.setAutoComplete();

    // API呼出引数
    let reportSetting = new ReportSetting();
    reportSetting.companyId = this.userInfoService.Company.id;
    reportSetting.reportId = COMPONENT_INFO.PF0601.name;

    // API呼出結果
    let settingsResult = new SettingsResult();
    this.reportSettingService.GetItems(reportSetting)
      .subscribe(response => {
        this.reportSettingsResult = new ReportSettingsResult();
        this.reportSettingsResult.reportSettings = response;
      });

  }

  ngAfterViewInit() {
    HtmlUtil.nextFocusByName(this.elementRef, 'baseMonthCtrl', EVENT_TYPE.NONE);

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

  public setControlInit() {

    this.baseMonthCtrl = new FormControl("", [Validators.required, Validators.maxLength(7)]);  // 基準日

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

    this.undefineCtrl = new FormControl(""); // 未定用;

    this.dispSubtotalUnitCtrl = new FormControl("");  // 得意先集計方法
    this.dispStaffSelectionCtrl = new FormControl("");  // 担当者集計方法
    this.displPriceUnitCtrl = new FormControl("");  // 金額単位

  }

  public setValidator() {
    this.MyFormGroup = new FormGroup({

      baseMonthCtrl: this.baseMonthCtrl,  // 基準日

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

      dispSubtotalUnitCtrl: this.dispSubtotalUnitCtrl,  // 得意先集計方法
      dispStaffSelectionCtrl: this.dispStaffSelectionCtrl,  // 担当者集計方法
      displPriceUnitCtrl: this.displPriceUnitCtrl,  // 金額単位

      undefineCtrl: this.undefineCtrl, // 未定用;

    });


  }

  public setFormatter() {
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
    else if (this.userInfoService.ApplicationControl.customerCodeType == CODE_TYPE.HANKANA) {
      FormatterUtil.setCustomerCodeKanaFormatter(this.customerCodeFromCtrl);
      FormatterUtil.setCustomerCodeKanaFormatter(this.customerCodeToCtrl);
    }
    else {
      FormatterUtil.setCustomerCodeAlphaFormatter(this.customerCodeFromCtrl);
      FormatterUtil.setCustomerCodeAlphaFormatter(this.customerCodeToCtrl);
    }
  }

  public setAutoComplete() {

    // 請求部門
    this.initAutocompleteDepartments(this.departmentCodeFromCtrl, this.departmentService, 0);
    this.initAutocompleteDepartments(this.departmentCodeToCtrl, this.departmentService, 1);
    // 担当者
    this.initAutocompleteStaffs(this.staffCodeFromCtrl, this.staffService, 0);
    this.initAutocompleteStaffs(this.staffCodeToCtrl, this.staffService, 1);
    // 得意先
    this.initAutocompleteCustomers(this.customerCodeFromCtrl, this.customerService, 0);
    this.initAutocompleteCustomers(this.customerCodeToCtrl, this.customerService, 1);
  }


  /**
   * 設定のモーダルを呼出
   */
  public openFromSettingModal() {
    let componentFactory = this.componentFactoryResolver.resolveComponentFactory(ModalFormSettingComponent);
    let componentRef = this.viewContainerRef.createComponent(componentFactory);

    componentRef.instance.ReportId = COMPONENT_INFO.PF0601.name;
    componentRef.instance.processModalCustomResult = this.processModalCustomResult;

    componentRef.instance.Closing.subscribe(() => {
      componentRef.destroy();
    });

  }

  /**
   * 
   * @param table Master名称
   * @param type モーダルの操作種別
   * @param keyCode キーコードがある場合はF9のみモーダルを開く
   * @param index 明細行の行No
   */
  public openMasterModal(table: TABLE_INDEX, type: string = null) {
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
    //this.collectionSchedulesResult = null;

    HtmlUtil.nextFocusByName(this.elementRef, 'baseMonthCtrl', EVENT_TYPE.NONE);


    this.setRangeCheckbox();
  }

  public setRangeCheckbox() {
    let cbxCustomer = this.localStorageManageService.get(RangeSearchKey.PF0601_CUSTOMER);
    let cbxDepartment = this.localStorageManageService.get(RangeSearchKey.PF0601_DEPARTMENT);
    let cbxStaff = this.localStorageManageService.get(RangeSearchKey.PF0601_STAFF);

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

  /**
   * データ照会
   */
  public search() {
    // API呼出引数
    let reportSetting = new ReportSetting();
    reportSetting.companyId = this.userInfoService.Company.id;
    reportSetting.reportId = COMPONENT_INFO.PF0601.name;

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

    this.collectionScheduleSearch = this.GetSearchCondition();

    let componentFactory = this.componentFactoryResolver.resolveComponentFactory(ModalRouterProgressComponent);
    let componentRef = this.viewContainerRef.createComponent(componentFactory);
    // componentRef.destroy();


    this.collectionScheduleService.Get(this.collectionScheduleSearch)
      .subscribe(response => {
        if (response != PROCESS_RESULT_RESULT_TYPE.FAILURE) {
          this.collectionSchedulesResult = new CollectionSchedulesResult();
          this.collectionSchedulesResult.collectionSchedules = response;

          if (this.collectionSchedulesResult == null || this.collectionSchedulesResult.collectionSchedules.length == 0) {
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
            MSG_ERR.SOMETHING_ERROR.replace(MSG_ITEM_NUM.FIRST, '回収予定表の検索'),
            this.partsResultMessageComponent);

        }
        componentRef.destroy();
      });
  }

  public GetSearchCondition(): CollectionScheduleSearch {
    var option = new CollectionScheduleSearch();

    option.companyId = this.userInfoService.Company.id;

    if (!StringUtil.IsNullOrEmpty(this.baseMonthCtrl.value)) {
      let baseMoment: Moment = this.baseMonthCtrl.value;
      let closingDay = this.userInfoService.Company.closingDay;

      if (closingDay == 99) {
        option.yearMonth = baseMoment.endOf('month').format("YYYY/MM/DD HH:mm:ss");
      }
      else {
        option.yearMonth = baseMoment.format("YYYY/MM/DD HH:mm:ss");
        option.yearMonth = option.yearMonth.substr(1, 7) + closingDay + '59:59:99';
      }
    }

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

    let tmpDisplayDepartment = parseInt(ReportSettigUtil.getItemKey(
      this.reportSettingsResult.reportSettings,
      REPORT_ITEM_ID.ReportDoOrNot,
      PF0601.DisplayDepartment));

    option.displayDepartment = tmpDisplayDepartment == ReportDoOrNot.Do;

    let tmpDisplayParent = parseInt(ReportSettigUtil.getItemKey(
      this.reportSettingsResult.reportSettings,
      REPORT_ITEM_ID.ReportSubtotalUnit,
      PF0601.CustomerType));

    option.displayParent = tmpDisplayParent == ReportSubtotalUnit.CustomerGroup;

    let tmpUseMasterStaff = parseInt(ReportSettigUtil.getItemKey(
      this.reportSettingsResult.reportSettings,
      REPORT_ITEM_ID.ReportStaffSelection,
      PF0601.StaffSelection));

    option.useMasterStaff = tmpUseMasterStaff == ReportStaffSelection.ByCustomerMaster;

    let tmpUnitPrice = parseInt(ReportSettigUtil.getItemKey(
      this.reportSettingsResult.reportSettings,
      REPORT_ITEM_ID.ReportUnitPrice,
      PF0601.UnitPrice));

    option.unitPrice =
      this.userInfoService.ApplicationControl.useForeignCurrency ? 1 :
        tmpUnitPrice == ReportUnitPrice.Per1 ? 1 :
          tmpUnitPrice == ReportUnitPrice.Per1000 ? 1000 :
            tmpUnitPrice == ReportUnitPrice.Per10000 ? 10000 :
              tmpUnitPrice == ReportUnitPrice.Per1000000 ? 1000000 : 1;

    let tmpNewPagePerDepartment = parseInt(ReportSettigUtil.getItemKey(
      this.reportSettingsResult.reportSettings,
      REPORT_ITEM_ID.ReportDoOrNot,
      PF0601.DepartmentNewPage));

    option.newPagePerDepartment = tmpNewPagePerDepartment == ReportDoOrNot.Do;

    let tmpNewPagePerStaff = parseInt(ReportSettigUtil.getItemKey(
      this.reportSettingsResult.reportSettings,
      REPORT_ITEM_ID.ReportDoOrNot,
      PF0601.StaffNewPage));

    option.newPagePerStaff = tmpNewPagePerDepartment == ReportDoOrNot.Do;

    return option;

  }

  public SetSearchOptionInformation() {

    let tmpDisplayParent = parseInt(ReportSettigUtil.getItemKey(
      this.reportSettingsResult.reportSettings,
      REPORT_ITEM_ID.ReportSubtotalUnit,
      PF0601.CustomerType));

    this.dispSubtotalUnitCtrl.setValue(tmpDisplayParent == ReportSubtotalUnit.PlainCustomer ? "得意先" : "債権代表者");

    let tmpUseMasterStaff = parseInt(ReportSettigUtil.getItemKey(
      this.reportSettingsResult.reportSettings,
      REPORT_ITEM_ID.ReportStaffSelection,
      PF0601.StaffSelection));

    this.dispStaffSelectionCtrl.setValue(tmpUseMasterStaff == ReportStaffSelection.ByBillingData ? "請求データ" : "得意先マスター");

    let tmpUnitPrice = parseInt(ReportSettigUtil.getItemKey(
      this.reportSettingsResult.reportSettings,
      REPORT_ITEM_ID.ReportUnitPrice,
      PF0601.UnitPrice));

    this.displPriceUnitCtrl.setValue(
      tmpUnitPrice == ReportUnitPrice.Per1 ? "円" :
        tmpUnitPrice == ReportUnitPrice.Per1000 ? "千円" :
          tmpUnitPrice == ReportUnitPrice.Per10000 ? "万円" :
            tmpUnitPrice == ReportUnitPrice.Per1000000 ? "百万円" : ""
    );

  }

  public getDisplayMonth(pastMonth: number) {
    if (StringUtil.IsNullOrEmpty(this.baseMonthCtrl.value)) return "XX";
    let month: Moment = this.baseMonthCtrl.value.clone();
    return month.add("M", pastMonth).format("MM");
  }

  /**
   * 印刷
   */
  public print() {
    let componentFactory = this.componentFactoryResolver.resolveComponentFactory(ModalRouterProgressComponent);
    let componentRef = this.viewContainerRef.createComponent(componentFactory);
    componentRef.instance.Closing.subscribe(() => {
      componentRef.destroy();
    });

    let result: boolean = false;
    this.collectionScheduleSearch = this.GetSearchCondition();

    this.collectionScheduleService.GetSpreadSheet(this.collectionScheduleSearch)
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
      });

  }


  /**
   * エクスポート
   */
  public export() {
    let data: string = "";
    let dataList = this.collectionSchedulesResult.collectionSchedules;
    let dataItems = Array<string>();

    let componentFactory = this.componentFactoryResolver.resolveComponentFactory(ModalRouterProgressComponent);
    let componentRef = this.viewContainerRef.createComponent(componentFactory);
    // componentRef.destroy();

    dataItems[0] = FileUtil.encloseItemBySymbol(REPORT_HEADER.COLLECTION_SCHEDULE_FORM).toString();
    dataItems[1] = FileUtil.encloseItemBySymbol([this.getDisplayMonth(-1) + "月迄未回収"]).toString();
    dataItems[2] = FileUtil.encloseItemBySymbol([this.getDisplayMonth(0) + "月"]).toString();
    dataItems[3] = FileUtil.encloseItemBySymbol([this.getDisplayMonth(1) + "月"]).toString();
    dataItems[4] = FileUtil.encloseItemBySymbol([this.getDisplayMonth(2) + "月"]).toString();
    dataItems[5] = FileUtil.encloseItemBySymbol([this.getDisplayMonth(3) + "月以降"]).toString();
    dataItems[6] = FileUtil.encloseItemBySymbol(["合計"]).toString();
    data = dataItems.join(',') + LINE_FEED_CODE;

    for (let index = 0; index < dataList.length; index++) {

      dataList[index].uncollectedAmountLast = NumberUtil.Round(dataList[index].uncollectedAmountLast);
      dataList[index].uncollectedAmount0 = NumberUtil.Round(dataList[index].uncollectedAmount0);
      dataList[index].uncollectedAmount1 = NumberUtil.Round(dataList[index].uncollectedAmount1);
      dataList[index].uncollectedAmount2 = NumberUtil.Round(dataList[index].uncollectedAmount2);
      dataList[index].uncollectedAmount3 = NumberUtil.Round(dataList[index].uncollectedAmount3);

      let dataItem: Array<any> = [];
      dataItem.push(dataList[index].customerInfo);
      dataItem.push(dataList[index].closingDay);
      dataItem.push(dataList[index].staffName);
      dataItem.push(dataList[index].departmentName);
      dataItem.push(dataList[index].collectCategoryName);
      dataItem.push(dataList[index].uncollectedAmountLast);
      dataItem.push(dataList[index].uncollectedAmount0);
      dataItem.push(dataList[index].uncollectedAmount1);
      dataItem.push(dataList[index].uncollectedAmount2);
      dataItem.push(dataList[index].uncollectedAmount3);
      dataItem.push(this.getUncollectedAmountTotal(dataList[index]));

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

  ///////////////////////////////////////////////////////////////////////////
  public setBaseMonth(eventType: string) {
    HtmlUtil.nextFocusByName(this.elementRef, 'departmentCodeFromCtrl', eventType);
  }

  /////////////////////////////////////////////////////////////////////////////////

  public setDepartmentCodeFrom(eventType: string) {

    if (eventType != EVENT_TYPE.BLUR) {
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

    if (eventType != EVENT_TYPE.BLUR) {
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
    localstorageItem.key = RangeSearchKey.PF0601_DEPARTMENT;
    localstorageItem.value = this.cbxDepartmentCtrl.value;
    this.localStorageManageService.set(localstorageItem);


    HtmlUtil.nextFocusByName(this.elementRef, 'departmentCodeFromCtrl', eventType);

  }

  /////////////////////////////////////////////////////////////////////////////////

  public setStaffCodeFrom(eventType: string) {

    if (eventType != EVENT_TYPE.BLUR) {
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

    if (eventType != EVENT_TYPE.BLUR) {
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
    localstorageItem.key = RangeSearchKey.PF0601_STAFF;
    localstorageItem.value = this.cbxStaffCtrl.value;
    this.localStorageManageService.set(localstorageItem);

    HtmlUtil.nextFocusByName(this.elementRef, 'staffCodeFromCtrl', eventType);

  }

  /////////////////////////////////////////////////////////////////////////////////

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

            HtmlUtil.nextFocusByName(this.elementRef, 'baseMonthCtrl', eventType);
          }
          else {
            //this.customerCodeToCtrl.setValue("");
            this.customerNameToCtrl.setValue("");
            HtmlUtil.nextFocusByName(this.elementRef, 'baseMonthCtrl', eventType);
          }
        });
    }
    else {
      this.customerCodeToCtrl.setValue("");
      this.customerNameToCtrl.setValue("");
      HtmlUtil.nextFocusByName(this.elementRef, 'baseMonthCtrl', eventType);
    }
  }

  public setCbxCustomer(eventType: string) {

    let localstorageItem = new LocalStorageItem();
    localstorageItem.key = RangeSearchKey.PF0601_CUSTOMER;
    localstorageItem.value = this.cbxCustomerCtrl.value;
    this.localStorageManageService.set(localstorageItem);

    HtmlUtil.nextFocusByName(this.elementRef, 'customerCodeFromCtrl', eventType);

  }

  public getUncollectedAmountTotal(collectionSchedule: CollectionSchedule): number {

    let uncollectedAmountTotal = collectionSchedule.uncollectedAmountLast +
      collectionSchedule.uncollectedAmount0 +
      collectionSchedule.uncollectedAmount1 +
      collectionSchedule.uncollectedAmount2 +
      collectionSchedule.uncollectedAmount3;

    return uncollectedAmountTotal;
  }

}
