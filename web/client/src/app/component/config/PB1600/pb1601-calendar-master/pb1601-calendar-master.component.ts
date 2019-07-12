import { Component, OnInit, ComponentFactoryResolver, ViewContainerRef, ElementRef } from '@angular/core';
import { BaseComponent } from 'src/app/component/common/base/base-component';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { HolidayCalendar } from 'src/app/model/holiday-calendar.model';
import { HolidayCalendarsResult } from 'src/app/model/holiday-calendars-result.model';
import { HolidayCalendarMasterService } from 'src/app/service/Master/holiday-calendar-master.service';
import { CustomValidators } from 'ng5-validation';
import { HtmlUtil } from 'src/app/common/util/html.util';
import { NgbDateCustomParserFormatter } from '../../../../common/util/dateformat-util';
import { NgbDateParserFormatter, NgbDate } from '@ng-bootstrap/ng-bootstrap';
import { FormatterUtil } from 'src/app/common/util/formatter.util';
import { UserInfoService } from 'src/app/service/common/user-info.service';
import { HolidayCalendarSearch } from 'src/app/model/holiday-calendar-search.model';
import { DateUtil } from 'src/app/common/util/date-util';
import { COMPONENT_STATUS_TYPE, MODAL_STATUS_TYPE, PROCESS_RESULT_RESULT_TYPE, PROCESS_RESULT_STATUS_TYPE } from 'src/app/common/const/status.const';
import { FILE_EXTENSION } from 'src/app/common/const/eb-file.const';
import { FileUtil } from 'src/app/common/util/file.util';
import { REPORT_HEADER, LINE_FEED_CODE } from 'src/app/common/const/report-setting-kbn.const';
import { ModalConfirmComponent } from 'src/app/component/modal/modal-confirm/modal-confirm.component';
import { ModalImportMethodSelectorComponent } from 'src/app/component/modal/modal-import-method-selector/modal-import-method-selector.component';
import { TABLE_INDEX } from 'src/app/common/const/table-name.const';
import { BUTTON_ACTION } from 'src/app/common/const/event.const';
import { MSG_WNG, MSG_ITEM_NUM } from 'src/app/common/const/message.const';
import { ProcessResultService } from 'src/app/service/common/process-result.service';
import { Router, ActivatedRoute, NavigationEnd } from '@angular/router';
import { PageUtil } from 'src/app/common/util/page-util';
import { ModalRouterProgressComponent } from 'src/app/component/modal/modal-router-progress/modal-router-progress.component';
import { FunctionType } from 'src/app/common/const/kbn.const';

@Component({
  selector: 'app-pb1601-calendar-master',
  templateUrl: './pb1601-calendar-master.component.html',
  styleUrls: ['./pb1601-calendar-master.component.css'],
  providers: [
    { provide: NgbDateParserFormatter, useClass: NgbDateCustomParserFormatter }
  ]
})
export class Pb1601CalendarMasterComponent extends BaseComponent implements OnInit {

  public FunctionType: typeof FunctionType = FunctionType;

  /** 削除対象 */
  public deleteDateList: Array<boolean>;
  /** 対象期間 */
  public fromHolidayCtrl: FormControl;
  public toHolidayCtrl: FormControl;
  /** 設定日 */
  public setHolidayCtrl;

  public holidayCalendarsResult: HolidayCalendarsResult;
  public cbxDetailDelFlagCtrls: Array<FormControl>;
  public isSearch: boolean = false;

  constructor(
    public router: Router,
    public activatedRoute: ActivatedRoute,
    public elementRef: ElementRef,
    public componentFactoryResolver: ComponentFactoryResolver,
    public viewContainerRef: ViewContainerRef,
    public userInfoService: UserInfoService,
    public holidayCalendarService: HolidayCalendarMasterService,
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
  }

  public setControlInit() {
    // 対象期間
    this.fromHolidayCtrl = new FormControl('', [Validators.required], CustomValidators.data);
    this.toHolidayCtrl = new FormControl('');
    // 設定日
    this.setHolidayCtrl = new FormControl('');
  }

  public setValidator() {
    this.MyFormGroup = new FormGroup({
      setHolidayCtrl: this.setHolidayCtrl,
      fromHolidayCtrl: this.fromHolidayCtrl,
      toHolidayCtrl: this.toHolidayCtrl,
    })
  }

  public setFormatter() {
  }

  public clear() {
    this.MyFormGroup.reset();
    this.holidayCalendarsResult = null;
    this.ComponentStatus = null;
    this.isSearch = false;

    let date = new Date();
    this.fromHolidayCtrl.setValue(new NgbDate(date.getFullYear(), 1, 1));
    this.toHolidayCtrl.setValue(new NgbDate(date.getFullYear(), 12, 31));

    this.search();
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
        this.isSearch = true;
        this.search();
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

      case BUTTON_ACTION.REGISTRY:
        this.addHoliday();
        break;

      default:
        console.log('buttonAction Error.');
        break;
    }
  }

  /**
   * データ取得・検索
   */
  public search() {
    let option = new HolidayCalendarSearch();

    let processComponentFactory = this.componentFactoryResolver.resolveComponentFactory(ModalRouterProgressComponent);
    let processComponentRef = this.viewContainerRef.createComponent(processComponentFactory);
    // processComponentRef.destroy();   


    option.companyId = this.userInfoService.Company.id;
    option.holiday = DateUtil.ConvertFromDatepicker(this.fromHolidayCtrl);
    option.fromHoliday = DateUtil.ConvertFromDatepicker(this.fromHolidayCtrl);
    option.toHoliday = DateUtil.ConvertFromDatepicker(this.toHolidayCtrl);
    this.holidayCalendarService.GetItems(option)
      .subscribe(result => {
        this.processCustomResult = this.processResultService.processAtGetData(
          this.processCustomResult, result, this.isSearch, this.partsResultMessageComponent);
        if (this.processCustomResult.result == PROCESS_RESULT_RESULT_TYPE.SUCCESS) {
          this.holidayCalendarsResult = new HolidayCalendarsResult();
          this.holidayCalendarsResult.holidayCalendars = new Array<HolidayCalendar>();
          this.holidayCalendarsResult.holidayCalendars = result;
          this.cbxDetailDelFlagCtrls = new Array<FormControl>(this.holidayCalendarsResult.holidayCalendars.length);
          this.deleteDateList = new Array<boolean>();

          for (let index = 0; index < this.holidayCalendarsResult.holidayCalendars.length; index++) {
            this.cbxDetailDelFlagCtrls[index] = new FormControl("");
            this.MyFormGroup.addControl("cbxDetailDelFlagCtrl" + index, this.cbxDetailDelFlagCtrls[index]);

            let holidayCalendarsItem = this.holidayCalendarsResult.holidayCalendars[index];
            this.holidayCalendarsResult.holidayCalendars[index].holiday
              = DateUtil.convertDateString(holidayCalendarsItem.holiday);

            this.deleteDateList.push(false);
          }
        }
        processComponentRef.destroy();  
      })
  }

  /**
   * データ追加
   */
  public addHoliday() {
    let addDay = DateUtil.convertStringFromData(this.setHolidayCtrl);
    let fromHoliday = DateUtil.convertStringFromData(this.fromHolidayCtrl);
    let toHoliday = DateUtil.convertStringFromData(this.toHolidayCtrl);
    let isAdd: boolean = true;
    let msg: string = "";

    if (addDay.length == 0) {
      this.processCustomResult = this.processResultService.processAtWarning(
        this.processCustomResult, MSG_WNG.SELECTION_REQUIRED.replace(MSG_ITEM_NUM.FIRST, '追加する日付'),
        this.partsResultMessageComponent);
      return;
    }

    // 入力チェック
    if (addDay < fromHoliday || toHoliday < addDay) {
      msg = MSG_WNG.INPUT_RANGE_VIOLATION.replace(MSG_ITEM_NUM.FIRST, '追加する日付');
      msg = msg.replace(MSG_ITEM_NUM.SECOND, '対象期間');
      isAdd = false;
    }

    if (isAdd && addDay.getDay() == 0 || addDay.getDay() == 6) {
      msg = '指定した日付が土日のため追加できません。';
      isAdd = false;
    }

    if (isAdd) {
      let holidayCalendars = new Array<string>();
      for (let i = 0; i < this.holidayCalendarsResult.holidayCalendars.length; i++) {
        holidayCalendars.push(this.holidayCalendarsResult.holidayCalendars[i].holiday);
      }

      let setHolodayStr = DateUtil.convertYYYYMMDD(this.setHolidayCtrl, true);
      if (0 <= holidayCalendars.indexOf(setHolodayStr)) {
        msg = '指定した日付はすでに登録済です。';
        isAdd = false;
      }
    }

    if (!isAdd) {
      this.processCustomResult = this.processResultService.processAtWarning(
        this.processCustomResult, msg, this.partsResultMessageComponent);
      return;
    }


    let processComponentFactory = this.componentFactoryResolver.resolveComponentFactory(ModalRouterProgressComponent);
    let processComponentRef = this.viewContainerRef.createComponent(processComponentFactory);
    // processComponentRef.destroy();       

    let holidays = new Array<HolidayCalendar>();
    let holiday = new HolidayCalendar();
    holiday.companyId = this.userInfoService.Company.id;
    holiday.holiday = DateUtil.ConvertFromDatepicker(this.setHolidayCtrl);
    holidays.push(holiday);

    this.holidayCalendarService.Save(holidays)
      .subscribe(result => {
        this.processCustomResult = this.processResultService.processAtSave(
          this.processCustomResult, result, true, this.partsResultMessageComponent);
        if (this.processCustomResult.result == PROCESS_RESULT_RESULT_TYPE.SUCCESS) {
         // this.clear();
          this.search();
          processComponentRef.destroy();   
        }
      })
  }

  /**
   * データ削除
   */
  public delete() {
    let holidays = new Array<HolidayCalendar>();
    for (let i = 0; i < this.deleteDateList.length; i++) {
      if (this.deleteDateList[i]) {
        let holiday = new HolidayCalendar();
        holiday.companyId = this.userInfoService.Company.id;
        holiday.holiday = this.holidayCalendarsResult.holidayCalendars[i].holiday;
        holidays.push(holiday);
        this.deleteDateList[i] = false;
      }
    }

    let componentFactory = this.componentFactoryResolver.resolveComponentFactory(ModalConfirmComponent);
    let componentRef = this.viewContainerRef.createComponent(componentFactory);
    componentRef.instance.ActionName = "削除"
    componentRef.instance.Closing.subscribe(() => {

      if (componentRef.instance.ModalStatus == MODAL_STATUS_TYPE.OK) {

        let processComponentFactory = this.componentFactoryResolver.resolveComponentFactory(ModalRouterProgressComponent);
        let processComponentRef = this.viewContainerRef.createComponent(processComponentFactory);
        // processComponentRef.destroy();   
    

        this.holidayCalendarService.Delete(holidays)
          .subscribe(result => {
            this.processCustomResult = this.processResultService.processAtDelete(
              this.processCustomResult, result, this.partsResultMessageComponent);
            if (this.processCustomResult.result == PROCESS_RESULT_RESULT_TYPE.SUCCESS) {
              //this.clear();
              this.search();
              this.allcheckFalse();
            }
            processComponentRef.destroy();   
          })
      }
      componentRef.destroy();
    });
  }

  /**
   * チェックした項目を判定
   * @param index 行番号
   */
  public onChecked(index: number) {
    this.deleteDateList[index] = !this.cbxDetailDelFlagCtrls[index].value;
    if (this.deleteDateList.indexOf(true) < 0) {
      this.ComponentStatus = null;
    } else {
      this.ComponentStatus = COMPONENT_STATUS_TYPE.UPDATE;
    }
  }

  /**
   * インポート
   */
  public openImportMethodSelector() {
    let componentFactory = this.componentFactoryResolver.resolveComponentFactory(ModalImportMethodSelectorComponent);
    let componentRef = this.viewContainerRef.createComponent(componentFactory);

    componentRef.instance.TableIndex = TABLE_INDEX.MASTER_HOLIDAY_CALENDAR;
    componentRef.instance.processModalCustomResult = this.processModalCustomResult;

    componentRef.instance.Closing.subscribe(() => {
      componentRef.destroy();
      this.search();
    });
    this.openOptions();
  }

  /**
   * エクスポート
   */
  public export() {
    let headers = FileUtil.encloseItemBySymbol(REPORT_HEADER.HOLIDAY_CALENDAR_MASTER);
    let data: string = headers.join(",") + LINE_FEED_CODE;
    let dataList = this.holidayCalendarsResult.holidayCalendars;


    // 件数チェック
    if (dataList.length <= 0) {
      this.processCustomResult = this.processResultService.processAtWarning(
        this.processCustomResult, MSG_WNG.NO_EXPORT_DATA, this.partsResultMessageComponent);
      return;
    }


    let processComponentFactory = this.componentFactoryResolver.resolveComponentFactory(ModalRouterProgressComponent);
    let processComponentRef = this.viewContainerRef.createComponent(processComponentFactory);
    // processComponentRef.destroy();       

    for (let index = 0; index < dataList.length; index++) {
      let dataItem: Array<any> = [];
      dataItem.push(this.userInfoService.Company.code);
      dataItem.push(dataList[index].holiday);
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

  public zeroPadding(num, length) {
    return ('0000000000' + num).slice(-length);
  }

  ///// Enterキー押下時の処理 //////////////////////////////////////////////////////
  public setFromHoliday(eventType: string) {
    HtmlUtil.nextFocusByName(this.elementRef, 'toHolidayCtrl', eventType);
  }

  public setToHoliday(eventType: string) {
    HtmlUtil.nextFocusByName(this.elementRef, 'setHolidayCtrl', eventType);
  }

  /**
   * すべてのチェック項目のチェックを外す
   */
  public allcheckFalse() {
    for (let i = 0; i < this.cbxDetailDelFlagCtrls.length; i++) {
      this.cbxDetailDelFlagCtrls[i].setValue(false);
    }
  }
}
